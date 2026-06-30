using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EduNest.Helpers
{
    /// <summary>
    /// Helper to apply small rounded corners to a DataGridView while keeping its border visible.
    /// Usage: DataGridViewRoundedHelper.Install(myDataGridView, 5);
    /// </summary>
    public static class DataGridViewRoundedHelper
    {
        public static void Install(DataGridView dgv, int radius = 5)
        {
            if (dgv == null) return;

            // detach first to avoid double subscription
            dgv.SizeChanged -= Dgv_SizeChanged;
            dgv.Paint -= Dgv_Paint;
            dgv.HandleCreated -= Dgv_HandleCreated;
            dgv.ParentChanged -= Dgv_ParentChanged;

            dgv.SizeChanged += Dgv_SizeChanged;
            dgv.Paint += Dgv_Paint;
            dgv.HandleCreated += Dgv_HandleCreated;
            dgv.ParentChanged += Dgv_ParentChanged;

            // store radius in Tag to avoid additional data structures
            dgv.Tag = radius;

            UpdateRegion(dgv, radius);

            void Dgv_SizeChanged(object? s, System.EventArgs e)
            {
                if (s is DataGridView d) UpdateRegion(d, radius);
            }

            void Dgv_HandleCreated(object? s, System.EventArgs e)
            {
                if (s is DataGridView d) UpdateRegion(d, radius);
            }

            void Dgv_ParentChanged(object? s, System.EventArgs e)
            {
                if (s is DataGridView d) UpdateRegion(d, radius);
            }
        }

        private static void UpdateRegion(DataGridView dgv, int radius)
        {
            if (dgv.Width <= 0 || dgv.Height <= 0)
                return;

            int r = Math.Max(0, radius);

            // Dispose previous region to avoid leaks
            try { dgv.Region?.Dispose(); } catch { }

            if (r == 0)
            {
                try { dgv.Region = null; } catch { }
                dgv.Invalidate();
                return;
            }

            using (var path = GetRoundedRectPath(new Rectangle(0, 0, dgv.Width, dgv.Height), r))
            {
                dgv.Region = new Region(path);
            }

            dgv.Invalidate();
        }

        private static void Dgv_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not DataGridView dgv) return;

            int radius = 5;
            if (dgv.Tag is int t) radius = t;

            if (radius <= 0) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Choose border color depending on BorderStyle
            Color borderColor = dgv.GridColor;
            float penWidth = 1f;

            if (dgv.BorderStyle == BorderStyle.Fixed3D)
            {
                // Fixed3D looks better with a slightly thicker and darker border
                penWidth = 2f;
                borderColor = SystemColors.ControlDark;
            }
            else if (dgv.BorderStyle == BorderStyle.None)
            {
                // no border requested
                return;
            }

            using (var pen = new Pen(borderColor, penWidth))
            using (var path = GetRoundedRectPath(new RectangleF(0 + penWidth / 2f, 0 + penWidth / 2f, dgv.Width - penWidth, dgv.Height - penWidth), radius))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        private static GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            return GetRoundedRectPath(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), radius);
        }

        private static GraphicsPath GetRoundedRectPath(RectangleF rect, int radius)
        {
            var path = new GraphicsPath();
            float r = Math.Max(0, radius);

            // Clamp radius to rectangle size
            if (r > rect.Width) r = rect.Width;
            if (r > rect.Height) r = rect.Height;

            if (r <= 0)
            {
                path.AddRectangle(rect);
                path.CloseFigure();
                return path;
            }

            float diameter = r;
            // top-left arc
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            // top edge
            path.AddLine(rect.X + diameter / 2f, rect.Y, rect.Right - diameter / 2f, rect.Y);
            // top-right arc
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            // right edge
            path.AddLine(rect.Right, rect.Y + diameter / 2f, rect.Right, rect.Bottom - diameter / 2f);
            // bottom-right arc
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            // bottom edge
            path.AddLine(rect.Right - diameter / 2f, rect.Bottom, rect.X + diameter / 2f, rect.Bottom);
            // bottom-left arc
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            // left edge
            path.AddLine(rect.X, rect.Bottom - diameter / 2f, rect.X, rect.Y + diameter / 2f);

            path.CloseFigure();
            return path;
        }
    }
}
