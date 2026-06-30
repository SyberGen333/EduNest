using EduNest.Helpers.Password;
using EduNest.Helpers;

namespace EduNest
{
    public partial class frmPassword : Form
    {
        public enum Mode
        {
            Create,
            Login,
            Change,
            ChangeAfterVerification
        }

        private Mode currentMode = Mode.Create;
        private PasswordHiderButton _hider;

        public frmPassword()
        {
            InitializeComponent();
            TopMost = true;
            // Wire handlers
            btncreateorlogin.Click += BtnCreateOrLogin_Click;
            btndelete.Click += Btndelete_Click;
            btnforgotpassword.Click += Btnforgotpassword_Click;

            // Initialize password hider control
            _hider = new PasswordHiderButton(txtpassword, passwordHiderLabel);
        }

        public frmPassword(Mode mode) : this()
        {
            SetupMode(mode);
        }

        private void SetupMode(Mode mode)
        {
            currentMode = mode;
            txtpassword.UseSystemPasswordChar = true;

            switch (mode)
            {
                case Mode.Create:
                    Text = "Create a Password";
                    label1.Text = "Create a Password:";
                    btncreateorlogin.Text = "&Create";
                    btndelete.Visible = false;
                    btnforgotpassword.Visible = false;
                    break;
                case Mode.Login:
                    Text = "Enter your password";
                    label1.Text = "Enter your password:";
                    btncreateorlogin.Text = "&Login";
                    btndelete.Visible = false;
                    btnforgotpassword.Visible = true;
                    break;
                case Mode.Change:
                    Text = "Change Password";
                    label1.Text = "Enter new password:";
                    btncreateorlogin.Text = "&Change";
                    btndelete.Visible = true;
                    btnforgotpassword.Visible = false;
                    break;
                case Mode.ChangeAfterVerification:
                    Text = "Change Password";
                    label1.Text = "Enter new password:";
                    btncreateorlogin.Text = "&Change";
                    // Hide delete when user is changing password immediately after verification
                    btndelete.Visible = false;
                    btnforgotpassword.Visible = false;
                    break;
            }

            Shown += (s, e) => txtpassword.Focus();
            AcceptButton = btncreateorlogin;
        }

        private void BtnCreateOrLogin_Click(object? sender, EventArgs e)
        {
            var input = txtpassword.Text ?? string.Empty;

            if (currentMode == Mode.Login)
            {
                // Verify password
                if (input == Settings.Default.LocalPassword || ForgotPassword.IsMasterToken(input))
                {
                    LogMaker.Log("Password login successful");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    LogMaker.Log("Password login failed");
                    MessageBox.Show("Incorrect password.", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                return;
            }

            // Create or Change mode
            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Please enter a password.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Save the password
            try
            {
                Settings.Default.LocalPassword = input;
                Settings.Default.Save();

                if (currentMode == Mode.Create)
                    LogMaker.Log("Local password created");
                else
                    LogMaker.Log("Local password changed");
            }
            catch (Exception ex)
            {
                LogMaker.LogException(ex, "Failed to save local password");
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void Btnforgotpassword_Click(object? sender, EventArgs e)
        {
            // Immediately attempt Windows Hello verification. If available and verified, allow password reset flow.
            _ = AttemptWindowsHelloThenFallbackAsync();
        }

        private async Task AttemptWindowsHelloThenFallbackAsync()
        {
            try
            {
                var (success, cancelled, error) = await ForgotPassword.VerifyWithWindowsHelloAsync("Verify to reset password");
                if (success)
                {
                    MessageBox.Show("Windows Hello verified. You may now reset your password.", "Verified", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Open change mode where delete is hidden because user just verified
                    using var f = new frmPassword(Mode.ChangeAfterVerification);
                    f.ShowDialog();
                    return;
                }
                // If verification was cancelled by the user, just show the error and do nothing further.
                if (cancelled)
                {
                    MessageBox.Show(error ?? "Windows Hello verification canceled.", "Verification canceled", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Otherwise fall back to existing offline/email flow by opening the deprecated form which now instructs users.
                MessageBox.Show(error ?? "Windows Hello verification failed or unavailable. Falling back to email/token flow.", "Fallback", MessageBoxButtons.OK, MessageBoxIcon.Information);
                using var fallback = new frmForgotPassword();
                fallback.ShowDialog();
            }
            catch (Exception ex)
            {
                LogMaker.LogException(ex, "Failed to perform Windows Hello verification");
                using var fallback = new frmForgotPassword();
                fallback.ShowDialog();
            }
        }

        private void Btndelete_Click(object? sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Are you sure you want to delete the password? This will remove password protection.", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    Settings.Default.LocalPassword = string.Empty;
                    Settings.Default.Save();
                    LogMaker.Log("Local password deleted");
                }
                catch (Exception ex)
                {
                    LogMaker.LogException(ex, "Failed to delete local password");
                }

                Application.Restart();
                Environment.Exit(0);
            }
            else
            {
                LogMaker.Log("Local password delete cancelled");
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
