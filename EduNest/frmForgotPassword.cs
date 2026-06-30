using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using EduNest.Helpers.Password;

namespace EduNest
{
    public partial class frmForgotPassword : Form
    {
        public frmForgotPassword()
        {
            InitializeComponent();
            // Deprecated form — immediately inform and close
            Shown += (s, e) =>
            {
                MessageBox.Show("This dialog is deprecated. Use Windows Hello via the main password dialog.", "Deprecated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            };
        }

        // P/Invoke to lock the current workstation (safer than logging off)
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool LockWorkStation();

        private static void LockCurrentWorkstation()
        {
            // Attempt to lock the workstation; ignore result
            _ = LockWorkStation();
        }

        // The send button is deprecated — the form is now useless and closes immediately
    }
}
