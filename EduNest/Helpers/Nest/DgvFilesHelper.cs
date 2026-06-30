using System.Diagnostics;
using EduNest.Helpers.Nest.UndoRedo;

namespace EduNest.Helpers.Nest
{
    public static class DgvFilesHelper
    {
        // optional undo manager set by the form using this helper
        public static UndoRedo.UndoRedoManager? UndoManager { get; set; }
        public static void HandleCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // ignore headers

            var grid = sender as DataGridView;
            if (grid == null) return;

            var fullPath = grid.Rows[e.RowIndex].Tag as string;
            if (string.IsNullOrEmpty(fullPath) || !File.Exists(fullPath))
            {
                MessageBox.Show("File not found.");
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo(fullPath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open file: " + ex.Message);
            }
        }

        public static void HandleCellMouseDown(object sender, DataGridViewCellMouseEventArgs e, ContextMenuStrip cms)
        {
            var grid = sender as DataGridView;
            if (grid == null) return;

            // Top-left corner click
            if (e.RowIndex == -1 && e.ColumnIndex == -1)
            {
                grid.ClearSelection();
            }

            // Right-click on a valid row
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                grid.ClearSelection();
                grid.Rows[e.RowIndex].Selected = true;
                grid.CurrentCell = grid.Rows[e.RowIndex].Cells["fileName"]; // use fileName col
                cms.Show(grid, e.Location);
            }
        }

        public static void HandleMouseClick(object sender, MouseEventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid == null) return;

            var hit = grid.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.None)
            {
                grid.ClearSelection(); // clicked blank area
            }
        }

        public static void HandleKeyDown(object sender, KeyEventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid == null) return;

            if (e.KeyCode == Keys.Enter && grid.CurrentRow != null)
            {
                e.SuppressKeyPress = true; // prevent ding
                HandleCellDoubleClick(grid,
                    new DataGridViewCellEventArgs(grid.CurrentCell.ColumnIndex, grid.CurrentRow.Index));
            }
        }

        public static void HandleCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid == null || e.ColumnIndex < 0 || e.RowIndex < 0) return;

            if (grid.Columns[e.ColumnIndex].Name != "fileName") return;

            var row = grid.Rows[e.RowIndex];
            string oldPath = row.Tag as string;
            if (string.IsNullOrEmpty(oldPath) || !File.Exists(oldPath)) return;

            string folder = Path.GetDirectoryName(oldPath)!;
            string ext = Path.GetExtension(oldPath);
            string newName = row.Cells["fileName"].Value?.ToString() ?? "";
            string newPath = Path.Combine(folder, newName + ext);

            if (string.Equals(oldPath, newPath, StringComparison.OrdinalIgnoreCase))
                return;

            if (File.Exists(newPath))
            {
                MessageBox.Show("A file with this name already exists.");
                row.Cells["fileName"].Value = Path.GetFileNameWithoutExtension(oldPath); // revert
                return;
            }

            try
            {
                File.Move(oldPath, newPath);
                row.Tag = newPath; // update cached path
                try
                {
                    // register undo action: newPath -> oldPath (rename)
                    UndoManager?.AddAction(new UndoRedo.UndoAction(oldPath, newPath, UndoRedo.UndoActionKind.Rename, "File rename"));
                }
                catch { }
                // refresh tree view and files on parent form (deferred on UI thread)
                try
                {
                    var parentForm = grid.FindForm() as EduNest.frmNest;
                    if (parentForm != null)
                    {
                        parentForm.BeginInvoke((Action)(() =>
                        {
                            try { parentForm.RefreshTree(); } catch { }
                            try { parentForm.RefreshFiles(); } catch { }
                        }));
                    }
                    else
                    {
                        try { (grid.FindForm() as EduNest.frmNest)?.RefreshTree(); } catch { }
                    }
                }
                catch { }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Rename failed: " + ex.Message);
                row.Cells["fileName"].Value = Path.GetFileNameWithoutExtension(oldPath); // revert
            }
            finally
            {
                grid.Columns["fileName"].ReadOnly = true; // lock column again
            }
        }
    }
}