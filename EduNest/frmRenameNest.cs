using EduNest.Helpers;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System;
using System.IO;

namespace EduNest
{
    public partial class frmRenameNest : Form
    {
        private readonly string currentFolderName; // actual folder folder name on disk

        // Parameterless constructor kept for designer support
        public frmRenameNest()
        {
            InitializeComponent();
            currentFolderName = string.Empty;
        }

        // Main constructor used at runtime
        public frmRenameNest(string currentFolderName)
        {
            InitializeComponent();
            this.currentFolderName = currentFolderName ?? string.Empty;

            Load += (s, e) =>
            {
                // Prefill textbox with the human-friendly name (without timestamp and course)
                try
                {
                    if (NestNameHelper.TryParseNameAndCourse(this.currentFolderName, out var name, out var course))
                    {
                        txtname.Text = name;
                    }
                    else
                    {
                        txtname.Text = this.currentFolderName;
                    }
                }
                catch
                {
                    txtname.Text = this.currentFolderName;
                }

                btnrename.Enabled = !string.IsNullOrWhiteSpace(txtname.Text);
            };

            // sanitize input and enable/disable button
            txtname.TextChanged += (s, e) =>
            {
                int caret = txtname.SelectionStart;
                string cleaned = NestNameHelper.SanitizeUserName(txtname.Text);
                if (txtname.Text != cleaned)
                {
                    txtname.Text = cleaned;
                    txtname.SelectionStart = Math.Min(caret, txtname.Text.Length);
                }
                btnrename.Enabled = !string.IsNullOrWhiteSpace(txtname.Text);
            };

            txtname.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter && btnrename.Enabled)
                {
                    e.SuppressKeyPress = true;
                    btnrename.PerformClick();
                }
            };

            btnrename.Click += (s, e) =>
            {
                var input = txtname.Text.Trim();
                if (string.IsNullOrWhiteSpace(input))
                {
                    MessageBox.Show("Please enter a name for the nest.", "Missing name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // parse current folder name to get course and timestamp
                if (!NestNameHelper.TryParseNameAndCourse(this.currentFolderName, out var oldName, out var course))
                {
                    MessageBox.Show("Current nest name is invalid or has unexpected format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.Equals(input, oldName, StringComparison.Ordinal))
                {
                    MessageBox.Show("New name must be different from the current name.", "No change", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // extract timestamp suffix from current folder
                var tsMatch = Regex.Match(this.currentFolderName, "-\\d{4}-\\d{2}-\\d{2}-\\d{2}-\\d{2}-\\d{2}$");
                if (!tsMatch.Success)
                {
                    MessageBox.Show("Cannot determine timestamp from current folder name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var tsSuffix = tsMatch.Value; // includes leading dash
                var newFolderName = input + "-" + course + tsSuffix; // input already sanitized

                var baseNestsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EduNest", "Nests");
                var oldFull = Path.Combine(baseNestsFolder, this.currentFolderName);
                var newFull = Path.Combine(baseNestsFolder, newFolderName);

                if (!Directory.Exists(oldFull))
                {
                    MessageBox.Show("The selected nest folder no longer exists.", "Not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }

                if (Directory.Exists(newFull))
                {
                    MessageBox.Show("A nest with the chosen name already exists.", "Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    Directory.Move(oldFull, newFull);

                    try { LogMaker.Log($"Nest renamed: \"{this.currentFolderName}\" -> \"{newFolderName}\""); } catch { }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    LogMaker.LogException(ex, "Failed to rename nest");
                    MessageBox.Show("Failed to rename nest:\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
        }
    }
}
