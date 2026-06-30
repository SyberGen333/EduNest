namespace EduNest
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Setup "Make a Backup" checkbox
            cbmakeabackup.CheckedChanged -= CheckBox_CheckedChangedInternal;
            cbmakeabackup.Checked = Settings.Default.MakeABackup;
            cbmakeabackup.Tag = new Action<bool>(v => Settings.Default.MakeABackup = v);
            cbmakeabackup.CheckedChanged += CheckBox_CheckedChangedInternal;

            // Setup "Do log adding" checkbox
            cbdologadding.CheckedChanged -= CheckBox_CheckedChangedInternal;
            cbdologadding.Checked = Settings.Default.DoLogAdding;
            cbdologadding.Tag = new Action<bool>(v => Settings.Default.DoLogAdding = v);
            cbdologadding.CheckedChanged += CheckBox_CheckedChangedInternal;
        }

        // Unified handler: retrieves setter from Tag and applies, then saves settings immediately
        private void CheckBox_CheckedChangedInternal(object sender, EventArgs e)
        {
            if (sender is CheckBox cb && cb.Tag is Action<bool> setter)
            {
                setter(cb.Checked);
                Settings.Default.Save();
            }
        }
    }
}
