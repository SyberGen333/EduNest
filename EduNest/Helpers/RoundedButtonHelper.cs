using System.Drawing.Drawing2D;

namespace EduNest.Helpers
{
    /// <summary>
    /// Helper to apply rounded corner region to all buttons with FlatStyle.Flat.
    /// Call `RoundedButtonHelper.Install(this, radius)` from a Form (for example in its constructor after InitializeComponent)
    /// </summary>
    public static class RoundedButtonHelper
    {
        /// <summary>
        /// Install rounded corner handling for all existing and future flat-style buttons in the given root control.
        /// </summary>
        /// <param name="root">Form or container control to scan</param>
        /// <param name="radius">Corner radius in pixels</param>
        public static void Install(Control root, int radius = 12)
        {
            if (root == null) return;

            // Apply to existing buttons
            foreach (Control c in GetAllControls(root))
            {
                if (c is Button btn && btn.FlatStyle == FlatStyle.Flat)
                {
                    ApplyRoundedRegion(btn, radius);
                    btn.SizeChanged -= Btn_SizeChanged;
                    btn.SizeChanged += Btn_SizeChanged;
                    btn.HandleCreated -= Btn_HandleCreated;
                    btn.HandleCreated += Btn_HandleCreated;
                }
            }

            // Attach handler to new controls added at the root level
            root.ControlAdded -= Root_ControlAdded;
            root.ControlAdded += Root_ControlAdded;

            void Root_ControlAdded(object sender, ControlEventArgs e)
            {
                // If a button was added, apply the rounded region
                if (e.Control is Button btn && btn.FlatStyle == FlatStyle.Flat)
                {
                    ApplyRoundedRegion(btn, radius);
                    btn.SizeChanged -= Btn_SizeChanged;
                    btn.SizeChanged += Btn_SizeChanged;
                    btn.HandleCreated -= Btn_HandleCreated;
                    btn.HandleCreated += Btn_HandleCreated;
                }

                // Also install recursively for containers that were added
                Install(e.Control, radius);
            }

            void Btn_SizeChanged(object? s, EventArgs ea)
            {
                if (s is Button b) ApplyRoundedRegion(b, radius);
            }

            void Btn_HandleCreated(object? s, EventArgs ea)
            {
                if (s is Button b) ApplyRoundedRegion(b, radius);
            }
        }

        private static IEnumerable<Control> GetAllControls(Control root)
        {
            foreach (Control c in root.Controls)
            {
                yield return c;
                foreach (var child in GetAllControls(c)) yield return child;
            }
        }

        private static void ApplyRoundedRegion(Button btn, int radius)
        {
            if (btn.Width <= 0 || btn.Height <= 0) return;

            int r = Math.Max(0, radius);

            if (r == 0)
            {
                // remove custom region
                try { btn.Region = null; } catch { }
                return;
            }

            using (var path = new GraphicsPath())
            {
                var rect = new Rectangle(0, 0, btn.Width, btn.Height);
                int w = r;
                // ensure arcs fit inside the rectangle
                if (w > rect.Width) w = rect.Width;
                if (w > rect.Height) w = rect.Height;

                path.AddArc(rect.X, rect.Y, w, w, 180, 90);
                path.AddArc(rect.Right - w, rect.Y, w, w, 270, 90);
                path.AddArc(rect.Right - w, rect.Bottom - w, w, w, 0, 90);
                path.AddArc(rect.X, rect.Bottom - w, w, w, 90, 90);
                path.CloseFigure();

                try
                {
                    // dispose existing region if any
                    btn.Region?.Dispose();
                }
                catch { }

                btn.Region = new Region(path);
                btn.Invalidate();
            }
        }
    }
}
