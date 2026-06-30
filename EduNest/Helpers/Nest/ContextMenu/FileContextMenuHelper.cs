using Microsoft.VisualBasic.FileIO; // Recycle bin use for Delete

namespace EduNest.Helpers.Nest
{
    public static class FileContextMenuHelper
    {
        public static void Rename(DataGridView grid)
        {
            if (grid.SelectedRows.Count == 0) return;

            var row = grid.SelectedRows[0];
            try
            {
                var parentForm = grid.FindForm() as EduNest.frmNest;
                if (parentForm != null)
                {
                    parentForm.BeginInvoke((Action)(() =>
                    {
                        try
                        {
                            grid.Columns["fileName"].ReadOnly = false;
                            grid.BeginEdit(true); // allow rename
                        }
                        catch { }
                    }));
                }
                else
                {
                    grid.Columns["fileName"].ReadOnly = false;
                    grid.BeginEdit(true);
                }
            }
            catch { }
        }

        public static void Delete(DataGridView grid, TreeView tree, Action<TreeViewEventArgs> refreshAction)
        {
            if (grid.SelectedRows.Count == 0) return;

            var row = grid.SelectedRows[0];
            string path = row.Tag as string;
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return;

            if (MessageBox.Show($"Delete '{Path.GetFileName(path)}'?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                // Send file to Recycle Bin instead of permanent delete
                FileSystem.DeleteFile(path,
                    UIOption.OnlyErrorDialogs,
                    RecycleOption.SendToRecycleBin);

                // refresh current folder in tree (deferred on UI thread)
                var parentForm = tree.FindForm() as EduNest.frmNest;
                if (parentForm != null)
                {
                    try
                    {
                        parentForm.BeginInvoke((Action)(() =>
                        {
                            if (tree.SelectedNode != null)
                                refreshAction(new TreeViewEventArgs(tree.SelectedNode));
                        }));
                    }
                    catch { }
                }
                else
                {
                    if (tree.SelectedNode != null)
                        refreshAction(new TreeViewEventArgs(tree.SelectedNode));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete failed: " + ex.Message);
            }
        }

        public static void MoveOutOfNest(DataGridView grid, TreeView tree, Action<TreeViewEventArgs> refreshAction)
        {
            if (grid.SelectedRows.Count == 0) return;

            var row = grid.SelectedRows[0];
            string path = row.Tag as string;
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return;

            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select destination folder";
                if (fbd.ShowDialog() != DialogResult.OK || !Directory.Exists(fbd.SelectedPath))
                    return;

                string newPath = Path.Combine(fbd.SelectedPath, Path.GetFileName(path));
                if (File.Exists(newPath))
                {
                    MessageBox.Show("A file with this name already exists in the destination.");
                    return;
                }

                try
                {
                    File.Move(path, newPath);

                    // refresh current folder in tree (deferred on UI thread)
                    var parentForm = tree.FindForm() as EduNest.frmNest;
                    if (parentForm != null)
                    {
                        try
                        {
                            parentForm.BeginInvoke((Action)(() =>
                            {
                                if (tree.SelectedNode != null)
                                    refreshAction(new TreeViewEventArgs(tree.SelectedNode));
                            }));
                        }
                        catch { }
                    }
                    else
                    {
                        if (tree.SelectedNode != null)
                            refreshAction(new TreeViewEventArgs(tree.SelectedNode));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Move failed: " + ex.Message);
                }
            }
        }
    }
}