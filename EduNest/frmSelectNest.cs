using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using EduNest.Helpers.SelectNest;
using EduNest.Helpers;

namespace EduNest
{
    public partial class frmSelectNest : Form
    {
        private string baseNestsFolder;

        public frmSelectNest()
        {
            InitializeComponent();
            IntializeEvents();
        }

        // Methods
        // small helper to hold display text + actual folder name
        private class NestItem
        {
            public string Display { get; }
            public string FullName { get; } // folder name like "test-BSCS-2025-08-24-09-08-14"

            public NestItem(string display, string fullName)
            {
                Display = display;
                FullName = fullName;
            }

            public override string ToString() => Display; // ListBox will show Display
        }
        // Convert "name-COURSE-YYYY-MM-DD-HH-MM-SS" => "name (COURSE)"
        // If pattern doesn't match, return the raw folderName
        private string FormatDisplayName(string folderName)
        {
            var (namePart, courseCode) = NestHelpers.CourseCodeCheck(folderName, null);

            if (!string.IsNullOrEmpty(courseCode))
            {
                if (string.IsNullOrEmpty(namePart))
                    return $"{courseCode}";
                return $"{namePart} ({courseCode})";
            }

            // fallback
            return folderName;
        }

        // refresh the listbox contents from disk
        private void RefreshList()
        {
            lbnests.Items.Clear();
            var dirs = Directory.GetDirectories(baseNestsFolder);
            foreach (var dir in dirs)
            {
                var folderName = Path.GetFileName(dir);
                var display = FormatDisplayName(folderName);
                lbnests.Items.Add(new NestItem(display, folderName));
            }
            btnselect.Enabled = false;
            btnrename.Enabled = false;
            btndelete.Enabled = false;

            try { LogMaker.Log("Refreshed select-nest list"); } catch { }
        }

        // Events
        private void IntializeEvents()
        {
            // (Form)
            Load += (s, e) => {
                // build base folder path and make sure it exists
                baseNestsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EduNest", "Nests");
                Directory.CreateDirectory(baseNestsFolder);

                // initial UI state
                lbnests.Items.Clear();
                btnselect.Enabled = false;
                btnrename.Enabled = false;
                btndelete.Enabled = false;

                // load only immediate subfolders (no recursion)
                var dirs = Directory.GetDirectories(baseNestsFolder);
                foreach (var dir in dirs)
                {
                    var folderName = Path.GetFileName(dir);
                    var display = FormatDisplayName(folderName); // e.g. "My Nest (BSCS)"
                    lbnests.Items.Add(new NestItem(display, folderName));
                }

                try { LogMaker.Log("Opened Select Nest dialog"); } catch { }
            };
            // (Selected Index Changed)
            lbnests.SelectedIndexChanged += (s, e) =>
            {
                var item = lbnests.SelectedItem as NestItem;
                if (item == null)
                {
                    btnselect.Enabled = false;
                    btnrename.Enabled = false;
                    btndelete.Enabled = false;
                    return;
                }

                var fullPath = Path.Combine(baseNestsFolder, item.FullName);
                var exists = Directory.Exists(fullPath);
                btnselect.Enabled = exists;
                btnrename.Enabled = exists;
                btndelete.Enabled = exists;

                try { LogMaker.Log($"Selected nest in list: \"{item.FullName}\""); } catch { }
            };
            // (Clicks)
            btnselect.Click += (s, e) =>
            {
                var item = lbnests.SelectedItem as NestItem;
                if (item == null) return;

                var fullPath = Path.Combine(baseNestsFolder, item.FullName);
                if (!Directory.Exists(fullPath))
                {
                    MessageBox.Show("The selected nest folder no longer exists.", "Not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // open frmNest with the actual folder name (same behavior as frmNewNest)
                var nestForm = new frmNest(item.FullName);
                nestForm.Show();

                LogMaker.Log($"Selected nest opened: \"{item.FullName}\"");

                // close selector and frmMain
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
            };

            // rename click - open dialog and refresh on success
            btnrename.Click += (s, e) =>
            {
                var item = lbnests.SelectedItem as NestItem;
                if (item == null) return;

                var fullPath = Path.Combine(baseNestsFolder, item.FullName);
                if (!Directory.Exists(fullPath))
                {
                    MessageBox.Show("The selected nest folder no longer exists.", "Not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    RefreshList();
                    return;
                }

                using var dlg = new frmRenameNest(item.FullName);
                var res = dlg.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    // reload list
                    RefreshList();
                    try { LogMaker.Log($"Nest renamed via selector: \"{item.FullName}\""); } catch { }
                }
            };

            // delete click - confirm then delete and refresh
            btndelete.Click += async (s, e) =>
            {
                var item = lbnests.SelectedItem as NestItem;
                if (item == null) return;

                var fullPath = Path.Combine(baseNestsFolder, item.FullName);
                if (!Directory.Exists(fullPath))
                {
                    MessageBox.Show("The selected nest folder no longer exists.", "Not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    RefreshList();
                    return;
                }

                var ok = MessageBox.Show($"Delete nest '{item.Display}' and all its contents?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (ok != DialogResult.Yes) return;

                try
                {
                    UseWaitCursor = true;
                    Cursor.Current = Cursors.WaitCursor;
                    this.Enabled = false;

                    var success = await NestDeleter.DeleteNestAsync(this, fullPath);

                    if (success)
                    {
                        RefreshList();
                        try { LogMaker.Log($"Nest deleted via selector: \"{item.FullName}\""); } catch { }
                        this.Activate();
                        return;
                    }
                    else
                    {
                        RefreshList();
                    }
                }
                finally
                {
                    UseWaitCursor = false;
                    Cursor.Current = Cursors.Default;
                    this.Enabled = true;

                    // ensure frmNest regains focus
                    this.BeginInvoke(new Action(() => this.Activate()));
                }
            };


            lbnests.DoubleClick += (s, e) =>
            {
                if (btnselect.Enabled) btnselect.PerformClick();
            };
        }
    }
}
