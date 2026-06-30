using EduNest.Helpers;

namespace EduNest
{
    public partial class frmNewNest : Form
    {

        public frmNewNest()
        {
            InitializeComponent();
            InitializeEvents();
        }

        private void frmNewNest_Load(object sender, EventArgs e)
        {
            cmbcourse.SelectedIndex = 0;
        }

        // Events
        private void InitializeEvents()
        {
            // (Text Changed)
            txtname.TextChanged += (s, e) =>
            {
                int caret = txtname.SelectionStart;
                string cleaned = NestNameHelper.SanitizeUserName(txtname.Text);
                if (txtname.Text != cleaned)
                {
                    txtname.Text = cleaned;
                    txtname.SelectionStart = Math.Min(caret, txtname.Text.Length);
                }
                btncreate.Enabled = !string.IsNullOrWhiteSpace(txtname.Text);
            };
            // (Key Presses)
            txtname.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter && btncreate.Enabled == true)
                {
                    e.SuppressKeyPress = true; // prevent ding
                    btncreate.PerformClick();
                }
            };
            txtname.KeyPress += (s, e) =>
            {
                // block invalid filename chars (existing) + explicitly block dash '-'
                char[] invalid = Path.GetInvalidFileNameChars();
                if ((invalid.Contains(e.KeyChar) && !char.IsControl(e.KeyChar)) || e.KeyChar == '-')
                {
                    e.Handled = true; // block input
                    System.Media.SystemSounds.Beep.Play();
                }
            };
            // (Button)
            btncreate.Click += (s, e) =>
            {
                // basic validation
                var nestNameInput = txtname.Text.Trim();
                if (string.IsNullOrEmpty(nestNameInput))
                {
                    MessageBox.Show("Please enter a name for the nest.", "Missing name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbcourse.SelectedItem == null && string.IsNullOrWhiteSpace(cmbcourse.Text))
                {
                    MessageBox.Show("Please select a course.", "Missing course", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // get course code (text before " - ")
                var courseFull = cmbcourse.Text.Trim();
                string courseCode;
                const string separator = " - ";
                int sepIndex = courseFull.IndexOf(separator, StringComparison.Ordinal);
                if (sepIndex >= 0)
                    courseCode = courseFull.Substring(0, sepIndex).Trim();
                else
                    courseCode = courseFull; // fallback: whole text

                // timestamp to seconds
                var ts = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"); // e.g. 2025-08-24-09-08-14

                // folder name format: (txtname)-(course)-(yyyy)-(MM)-(dd)-(HH)-(mm)-(ss)
                var folderName = $"{nestNameInput}-{courseCode}-{ts}";

                try
                {
                    // base folder for nests (change if you want another location)
                    var baseNestsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EduNest", "Nests");
                    Directory.CreateDirectory(baseNestsFolder);

                    var fullFolderPath = Path.Combine(baseNestsFolder, folderName);

                    // create the main folder
                    Directory.CreateDirectory(fullFolderPath);

                    // create the three subfolders
                    var subfolders = new[] { "01 - Lectures", "02 - Activities", "03 - Answersheets" };
                    foreach (var sub in subfolders)
                    {
                        Directory.CreateDirectory(Path.Combine(fullFolderPath, sub));
                    }

                    // open frmNest with the new folder's name sent
                    var nestForm = new frmNest(folderName);
                    nestForm.Show();

                    // close these forms
                    this.Close();

                    // find frmMain and close it
                    foreach (Form f in Application.OpenForms)
                    {
                        if (f is frmMain)
                        {
                            f.Close();
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to create nest folder:\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
        }
    }
}
