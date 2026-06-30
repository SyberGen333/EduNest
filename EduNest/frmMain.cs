using EduNest.Helpers;
using EduNest.Helpers.Main;
using System.Diagnostics;

namespace EduNest
{
    public partial class frmMain : Form
    {
        private bool _exitHandled = false;
        private bool _suppressBackup = false; // when true, skip backup and exit immediately

        public frmMain()
        {
            InitializeComponent();
            InitializeButtons();
            RoundedButtonHelper.Install(this, 12);
        }

        // (Form)
        // Load for nest buttons and password handling
        private void frmMain_Load(object sender, EventArgs e)
        {
            HandleNestButtonsAndPassword();

            // Attempt to load EduNest from ZIP if present and folder is missing/empty.
            try
            {
                string docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string rootFolder = Path.Combine(docs, "EduNest");
                string zipPath = Path.Combine(docs, "EduNest.zip");

                bool folderExists = Directory.Exists(rootFolder);
                bool folderHasContents = folderExists && Directory.EnumerateFileSystemEntries(rootFolder).Any();
                bool zipExists = File.Exists(zipPath);

                if (zipExists && (!folderExists || !folderHasContents))
                {
                    // Use the local method to decompress (shows progress and logs)
                    TryDecompressEduNest(rootFolder, zipPath);
                }
                else if (!folderExists)
                {
                    // Ensure folder exists if nothing to extract
                    Directory.CreateDirectory(rootFolder);
                }
            }
            catch (Exception ex)
            {
                LogMaker.LogException(ex, "Startup: failed to ensure EduNest folder");
            }

            // Log main form load
            LogMaker.Log("Main form loaded");
        }

        // Form close - Create rotating backup + delete original folder
        private async void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Prevent re-entrancy
            if (_exitHandled)
                return;

            _exitHandled = true;

            // If exit was requested because the password dialog was closed/cancelled,
            // skip any backup attempts and exit immediately to avoid running backup logic on a non-authenticated
            // shutdown which was causing crashes.
            if (_suppressBackup)
            {
                LogMaker.Log("Exit requested from password dialog; skipping backup and exiting.");

                // If no nest windows are open, ensure the application exits to avoid lingering process
                bool anyNestOpenSuppress = Application.OpenForms.OfType<frmNest>().Any();
                if (!anyNestOpenSuppress)
                {
                    LogMaker.Log("No nest windows open; exiting application (password cancelled)");
                    Application.Exit();
                }

                return;
            }

            string docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string source = Path.Combine(docs, "EduNest");
            string zip = Path.Combine(docs, "EduNest.zip");

            LogMaker.Log("Application closing initiated");

            // Respect user setting: if backups disabled, skip backup entirely
            if (!Settings.Default.MakeABackup)
            {
                LogMaker.Log("Skipping backup because user disabled backups in settings");

                // If no nest windows are open, ensure the application exits to avoid lingering process
                bool anyNestOpenSkip = Application.OpenForms.OfType<frmNest>().Any();
                if (!anyNestOpenSkip)
                {
                    LogMaker.Log("No nest windows open; exiting application (backups disabled)");
                    // Make sure the app shuts down
                    Application.Exit();
                    return;
                }

                // If there are nest windows open, allow normal closing flow (do not attempt backup)
                return;
            }

            // --------------------------------------------------------------------
            // 1. Skip backup if frmNest was opened recently (< 800ms)
            // --------------------------------------------------------------------
            double msSinceNestOpened = (DateTime.Now - frmNest.LastNestOpenedTime).TotalMilliseconds;

            if (msSinceNestOpened < 800)
            {
                // Skip backup entirely — probably navigating UI
                LogMaker.Log("Skipping backup due to recent nest open (likely UI navigation)");
                return;
            }

            // --------------------------------------------------------------------
            // 2. If there's nothing to back up, just exit normally
            // --------------------------------------------------------------------
            if (!Directory.Exists(source))
            {
                bool hasNestOpen = Application.OpenForms.OfType<frmNest>().Any();
                if (!hasNestOpen)
                {
                    LogMaker.Log("No EduNest folder found in Documents; exiting without backup");
                    Application.Exit();
                }
                return;
            }

            // --------------------------------------------------------------------
            // 3. Prevent the form from closing immediately while running backup
            // --------------------------------------------------------------------
            e.Cancel = true;

            using (var dlg = new frmProgressBar("Preparing backup..."))
            {
                dlg.Show();

                try
                {
                    dlg.UpdateStatus("Starting...", 0);

                    LogMaker.Log($"Backup starting: source=\"{source}\", zip=\"{zip}\"");

                    // Heavy zipper class (async inside the Task.Run sections)
                    await ZipperBeforeClose.ZipAndDeleteAsync(source, zip, dlg);

                    dlg.SetCompleted("Backup complete");

                    LogMaker.Log($"Backup complete: \"{zip}\"");
                }
                catch (OperationCanceledException)
                {
                    LogMaker.Log("Backup cancelled by user");
                    MessageBox.Show(this,
                        "Backup cancelled.",
                        "Backup",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    LogMaker.LogException(ex, "Backup Error");
                    MessageBox.Show(this,
                        "An error occurred while creating backup:\n\n" + ex.Message,
                        "Backup Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                finally
                {
                    // allow user to see 100%
                    try { await Task.Delay(300); } catch { }

                    if (!dlg.IsDisposed)
                        dlg.Close();
                }
            }

            // --------------------------------------------------------------------
            // 4. Allow form to finish closing
            // --------------------------------------------------------------------
            e.Cancel = false;

            // If no nest windows are open, exit the app
            bool anyNestOpen = Application.OpenForms.OfType<frmNest>().Any();
            if (!anyNestOpen)
            {
                LogMaker.Log("No nest windows open; exiting application");
                Application.Exit();
            }
        }

        // ============================================================
        // Helper: Enable/disable buttons, handle password
        // ============================================================
        private void HandleNestButtonsAndPassword()
        {
            string nestsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "EduNest", "Nests");

            // Enable or disable "Select Nest"
            btnselectnest.Enabled =
                Directory.Exists(nestsPath) &&
                Directory.GetDirectories(nestsPath).Length > 0;

            // Enable/disable Load Last Nest
            try
            {
                var last = SettingsHelper.GetSavedLastNestPath();
                btnopennest.Enabled = (!string.IsNullOrWhiteSpace(last) &&
                                       Directory.Exists(last));
            }
            catch
            {
                btnopennest.Enabled = false;
            }

            // Password handling
            if (!string.IsNullOrWhiteSpace(Settings.Default.LocalPassword))
            {
                using (var pwd = new frmPassword(frmPassword.Mode.Login))
                {
                    if (pwd.ShowDialog(this) != DialogResult.OK)
                    {
                        // If the user closed or cancelled the password dialog, suppress backups
                        // and exit immediately to avoid running backup logic on a non-authenticated
                        // shutdown which was causing crashes.
                        _suppressBackup = true;
                        Close();
                        return;
                    }
                }

                labelPasswordStatus.Text = "Password Implemented!";
                btnpassword.Text = "&Change Password";
            }
            else
            {
                labelPasswordStatus.Text = "Password protect this app >";
                btnpassword.Text = "&Setup a Password";
            }
        }

        private void TryDecompressEduNest(string outputFolder, string zipPath)
        {
            try
            {
                using (frmProgressBar p = new frmProgressBar("Loading EduNest..."))
                {
                    p.Show();
                    p.Update();

                    System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, outputFolder, true);

                    p.SetCompleted();

                    LogMaker.Log($"Decompressed EduNest from \"{zipPath}\" to \"{outputFolder}\"");
                }
            }
            catch (Exception ex)
            {
                LogMaker.LogException(ex, "Failed to decompress EduNest.zip");

                MessageBox.Show(
                    "Failed to load EduNest.zip.\nCreating a fresh folder instead.\n\n" +
                    ex.Message,
                    "EduNest Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                Directory.CreateDirectory(outputFolder);
            }
        }


        // for button clicks
        // Initialize button click events
        private void InitializeButtons()
        {
            btnnewnest.Click += (s, e) =>
            {
                LogMaker.Log("Opened New Nest dialog");
                new frmNewNest().ShowDialog();
            };
            btnselectnest.Click += (s, e) =>
            {
                LogMaker.Log("Opened Select Nest dialog");
                new frmSelectNest().ShowDialog();
            };

            // Load Last Nest button (btnopennest)
            btnopennest.Click += (s, e) =>
            {
                var last = SettingsHelper.GetSavedLastNestPath();
                if (string.IsNullOrWhiteSpace(last) || !Directory.Exists(last))
                {
                    MessageBox.Show("Last saved nest is not available.", "Not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LogMaker.Log("Attempted to open last nest but path not available");
                    return;
                }

                try
                {
                    // derive folder name from path
                    var folderName = Path.GetFileName(last.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                    var nestForm = new frmNest(folderName);
                    nestForm.Show();

                    LogMaker.Log($"Opened last nest: \"{folderName}\"");

                    // close main
                    this.Close();
                }
                catch (Exception ex)
                {
                    LogMaker.LogException(ex, "Failed to open last nest");
                    MessageBox.Show("Failed to open last nest:\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            // Password button
            btnpassword.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(Settings.Default.LocalPassword))
                {
                    // Create new password
                    using (var pwd = new frmPassword(frmPassword.Mode.Create))
                    {
                        var dr = pwd.ShowDialog(this);
                        if (dr == DialogResult.OK)
                        {
                            labelPasswordStatus.Text = "Password Implemented!";
                            btnpassword.Text = "&Change Password";
                            LogMaker.Log("Local password created");
                        }
                        else
                        {
                            LogMaker.Log("Local password creation cancelled");
                        }
                    }
                }
                else
                {
                    // Change password
                    using (var pwd = new frmPassword(frmPassword.Mode.Change))
                    {
                        var dr = pwd.ShowDialog(this);
                        if (dr == DialogResult.OK)
                        {
                            labelPasswordStatus.Text = "Password Implemented!";
                            btnpassword.Text = "&Change Password";
                            LogMaker.Log("Local password changed");
                        }
                        else
                        {
                            LogMaker.Log("Local password change cancelled");
                        }
                    }
                }
            };

            // Opens app directory
            btnappdirectory.Click += (s, e) =>
            {
                try
                {
                    string appDir = AppContext.BaseDirectory ?? Environment.CurrentDirectory;

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "explorer.exe",
                        Arguments = $"\"{appDir}\"",
                        UseShellExecute = true
                    });

                    LogMaker.Log($"Opened app directory: \"{appDir}\"");
                }
                catch (Exception ex)
                {
                    LogMaker.LogException(ex, "Failed to open app directory");
                    MessageBox.Show(this,
                        "Failed to open application directory:\n\n" + ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            };

            // Opens settings
            btnsettings.Click += (s, e) =>
            {
                LogMaker.Log("Opened Settings dialog");
                using (var settingsForm = new frmSettings())
                {
                    settingsForm.ShowDialog(this);
                    LogMaker.Log("Closed Settings dialog");
                }
            };
        }
    }
}
