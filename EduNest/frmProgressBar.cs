namespace EduNest
{
    public partial class frmProgressBar : Form
    {
        // Optional parameter lets callers pass a starting message
        public frmProgressBar(string message = "Working...")
        {
            InitializeComponent();

            // Recommended designer settings:
            // FormBorderStyle = None; TopMost = true; ShowInTaskbar = false;
            // progressBar1.Minimum = 0; progressBar1.Maximum = 100;

            label1.Text = message;
            progressBar1.Value = 0;
        }

        /// <summary>
        /// Thread-safe UI update for label and progressBar.
        /// value is clamped to [progressBar1.Minimum, progressBar1.Maximum].
        /// </summary>
        public void UpdateStatus(string text, int value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateStatus(text, value)));
                return;
            }

            label1.Text = text ?? string.Empty;

            // clamp value
            int v = Math.Max(progressBar1.Minimum, Math.Min(progressBar1.Maximum, value));
            // Ensure we don't throw if the progressBar isn't yet fully initialized
            try
            {
                progressBar1.Value = v;
            }
            catch
            {
                // ignore if control not ready
            }

            // keep UI responsive
            label1.Refresh();
            progressBar1.Refresh();
            Application.DoEvents();
        }

        /// <summary>
        /// Convenience to set the bar to complete and show a final message.
        /// </summary>
        public void SetCompleted(string finalMessage = "Done")
        {
            UpdateStatus(finalMessage, progressBar1.Maximum);
        }
    }
}
