namespace EduNest.Helpers.Nest
{
    public static class TreeContextMenuHelper
    {
        public static void Rename(TreeView tree)
        {
            if (tree.SelectedNode == null) return;

            tree.LabelEdit = true;
            tree.SelectedNode.BeginEdit();
        }

        public static void Delete(TreeView tree)
        {
            if (tree.SelectedNode == null) return;

            var node = tree.SelectedNode;

            // determine root node and its path
            var rootNode = node;
            while (rootNode.Parent != null) rootNode = rootNode.Parent;
            string rootPath = rootNode.Tag as string;

            string folderPath = node.Tag as string;

            // Ask user whether to delete the whole Nest (root) or only the selected folder
            var result = MessageBox.Show(
                $"Choose deletion scope:\r\n\r\nYes = Delete the whole Nest '{rootNode.Text}' (will remove '{rootPath}' and close this window).\r\nNo  = Delete only '{node.Text}' and its contents.\r\nCancel = Do nothing.",
                "Confirm Delete",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Cancel) return;

            // decide target path based on user's choice
            string targetPath = result == DialogResult.Yes ? rootPath : folderPath;

            if (string.IsNullOrEmpty(targetPath) || !Directory.Exists(targetPath))
            {
                MessageBox.Show("Invalid folder.");
                return;
            }

            try
            {
                // clear read-only/system flags
                ClearAttributes(new DirectoryInfo(targetPath));

                Directory.Delete(targetPath, true);

                if (result == DialogResult.Yes)
                {
                    // deleting whole nest: close the containing form to return to main
                    var parentForm = tree.FindForm();
                    try { parentForm?.Close(); } catch { }
                }
                else
                {
                    // deleting only the selected folder: remove node from UI
                    node.Remove();
                }

                // refresh parent form tree view if present
                try
                {
                    var parent = tree.FindForm() as EduNest.frmNest;
                    parent?.Invoke(() => parent.RefreshTree());
                }
                catch { }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Access denied. Please close any open files or check folder permissions.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete failed: " + ex.Message);
            }
        }

        private static void ClearAttributes(DirectoryInfo dir)
        {
            foreach (var subDir in dir.GetDirectories())
                ClearAttributes(subDir);
            foreach (var file in dir.GetFiles())
                file.Attributes = FileAttributes.Normal;
            dir.Attributes = FileAttributes.Normal;
        }
    }
}
