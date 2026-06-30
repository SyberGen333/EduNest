namespace EduNest
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainAppContext());
        }

        private class MainAppContext : ApplicationContext
        {
            private frmMain mainForm;

            public MainAppContext()
            {
                // create and show main form
                mainForm = new frmMain();
                mainForm.FormClosed += MainForm_FormClosed;
                mainForm.Show();
            }

            private void MainForm_FormClosed(object? sender, FormClosedEventArgs e)
            {
                // Wait 0.5 seconds; if no forms are open then exit, otherwise attach to remaining forms
                var t = new System.Windows.Forms.Timer { Interval = 500, Enabled = false };
                t.Tick += (s, ev) =>
                {
                    try
                    {
                        t.Stop();
                        t.Dispose();

                        if (Application.OpenForms.Count == 0)
                        {
                            ExitThread();
                            return;
                        }

                        // attach handler to remaining open forms so we can exit when last closes
                        foreach (Form f in Application.OpenForms)
                        {
                            // avoid attaching to disposed forms
                            try { f.FormClosed -= OtherForm_FormClosed; } catch { }
                            f.FormClosed += OtherForm_FormClosed;
                        }
                    }
                    catch
                    {
                        try { t.Dispose(); } catch { }
                    }
                };

                t.Start();
            }

            private void OtherForm_FormClosed(object? sender, FormClosedEventArgs e)
            {
                // if there are no more open forms, exit
                if (Application.OpenForms.Count == 0)
                {
                    ExitThread();
                }
            }
        }
    }
}