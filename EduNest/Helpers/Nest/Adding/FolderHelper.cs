using EduNest.Helpers.Nest;

namespace EduNest.Helpers
{
    public static class FolderHelper
    {
        public static void AddSubfolder(TreeView treeView, TreeNode parentNode)
        {
            if (parentNode == null) return;

            string parentPath = parentNode.Tag as string;
            if (string.IsNullOrEmpty(parentPath) || !Directory.Exists(parentPath))
            {
                MessageBox.Show("The selected folder no longer exists.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Find available folder name
            string newFolderPath = Path.Combine(parentPath, "New Folder");
            int counter = 1;
            while (Directory.Exists(newFolderPath))
            {
                newFolderPath = Path.Combine(parentPath, $"New Folder ({counter})");
                counter++;
            }

            try
            {
                // Create the folder
                Directory.CreateDirectory(newFolderPath);

                // Add new node
                TreeNode newNode = parentNode.Nodes.Add(Path.GetFileName(newFolderPath));
                newNode.Tag = newFolderPath;
                parentNode.Expand();

                // Select new node
                treeView.SelectedNode = newNode;

                // Trigger rename immediately
                TreeContextMenuHelper.Rename(treeView);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create folder: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
