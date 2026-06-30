using System.Media;

namespace EduNest.Helpers.Nest
{
    public static class NestTreeHelper
    {
        // optional undo manager set by the form
        public static UndoRedo.UndoRedoManager? UndoManager { get; set; }
        // Handle AfterSelect (refresh DataGridView files)
        public static void HandleAfterSelect(TreeViewEventArgs e, Action<string> displayFilesInFolder)
        {
            var tag = e.Node?.Tag as string;
            if (string.IsNullOrEmpty(tag)) return;

            string folderToUse = Directory.Exists(tag)
                ? tag
                : (File.Exists(tag) ? Path.GetDirectoryName(tag) : null);

            if (!string.IsNullOrEmpty(folderToUse) && Directory.Exists(folderToUse))
                displayFilesInFolder(folderToUse);
        }

        // Handle right-click context menu on TreeView
        public static void HandleNodeMouseClick(TreeNodeMouseClickEventArgs e, TreeView tree, ContextMenuStrip cmsTreeView)
        {
            if (e.Button != MouseButtons.Right) return;

            tree.SelectedNode = e.Node; // always select right-clicked

            if (e.Node.Parent == null) return; // root node -> no menu
            cmsTreeView.Show(tree, e.Location);
        }

        // Handle rename (AfterLabelEdit)
        public static void HandleAfterLabelEdit(NodeLabelEditEventArgs e)
        {
            if (e.Label == null) return; // user pressed ESC

            string oldPath = e.Node.Tag as string;
            if (string.IsNullOrEmpty(oldPath) || !Directory.Exists(oldPath))
            {
                e.CancelEdit = true;
                return;
            }

            string newPath = Path.Combine(Path.GetDirectoryName(oldPath)!, e.Label);

            // Same path, no need to rename
            if (string.Equals(oldPath, newPath, StringComparison.OrdinalIgnoreCase))
            {
                e.CancelEdit = true;
                return;
            }

            if (Directory.Exists(newPath))
            {
                e.CancelEdit = true;
                MessageBox.Show("A folder with this name already exists.");
                return;
            }

            try
            {
                Directory.Move(oldPath, newPath);
                e.Node.Tag = newPath;
                e.Node.Text = e.Label;
            }
            catch (Exception ex)
            {
                e.CancelEdit = true;
                MessageBox.Show("Rename failed: " + ex.Message);
            }
        }

        // Prevent root node rename
        public static void HandleBeforeLabelEdit(NodeLabelEditEventArgs e)
        {
            if (e.Node.Parent == null)
                e.CancelEdit = true;
        }

        // --- DRAG & DROP HANDLERS ---

        public static void HandleItemDrag(object sender, ItemDragEventArgs e)
        {
            if (sender is TreeView tree && e.Item is TreeNode node)
                tree.DoDragDrop(node, DragDropEffects.Move);
        }

        public static void HandleDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        public static void HandleDragOver(object sender, DragEventArgs e)
        {
            if (sender is TreeView tree)
            {
                Point targetPoint = tree.PointToClient(new Point(e.X, e.Y));
                tree.SelectedNode = tree.GetNodeAt(targetPoint);
            }
        }

        public static void HandleDragDrop(object sender, DragEventArgs e)
        {
            if (sender is not TreeView tree) return;
            if (!e.Data.GetDataPresent(typeof(TreeNode))) return;

            Point targetPoint = tree.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = tree.GetNodeAt(targetPoint);
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            if (targetNode == null || draggedNode == null)
            {
                SystemSounds.Hand.Play();
                return;
            }

            // Prevent dropping into itself or descendants
            if (draggedNode == targetNode || ContainsNode(draggedNode, targetNode))
            {
                SystemSounds.Hand.Play();
                MessageBox.Show("You cannot move a folder into itself or one of its subfolders.",
                                "Invalid Move", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sourcePath = draggedNode.Tag?.ToString();
            string targetPath = targetNode.Tag?.ToString();

            if (string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(targetPath))
            {
                SystemSounds.Hand.Play();
                return;
            }

            try
            {
                string destPath = Path.Combine(targetPath, Path.GetFileName(sourcePath));

                // Prevent recursive or invalid moves (path-level check)
                if (targetPath.StartsWith(sourcePath + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                {
                    SystemSounds.Hand.Play();
                    MessageBox.Show("You cannot move a folder inside one of its subfolders.",
                                    "Invalid Move", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Prevent duplicate target folder
                if (Directory.Exists(destPath))
                {
                    SystemSounds.Hand.Play();
                    MessageBox.Show("A folder with the same name already exists in the target directory.",
                                    "Duplicate Folder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Try moving with retry logic to avoid OneDrive/sync interference
                const int maxRetries = 3;
                int attempt = 0;
                bool success = false;

                while (attempt < maxRetries && !success)
                {
                    try
                    {
                        Directory.Move(sourcePath, destPath);
                        success = true;
                    }
                    catch (IOException ioEx)
                    {
                        attempt++;
                        if (attempt < maxRetries)
                        {
                            // Wait before retry (OneDrive or antivirus lock)
                            System.Threading.Thread.Sleep(500);
                        }
                        else
                        {
                            throw new IOException("Move operation failed after multiple attempts.\r\n" + ioEx.Message, ioEx);
                        }
                    }
                }

                if (!success)
                {
                    SystemSounds.Hand.Play();
                    MessageBox.Show("Failed to move the folder after several attempts.",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Update tree node hierarchy
                draggedNode.Remove();
                targetNode.Nodes.Add(draggedNode);
                draggedNode.Tag = destPath;
                targetNode.Expand();

                try
                {
                    // register undo action for folder move
                    UndoManager?.AddAction(new UndoRedo.UndoAction(sourcePath, destPath, UndoRedo.UndoActionKind.Move, "Folder move"));
                }
                catch { }

                // attempt to refresh the tree view on the parent form if available
                try
                {
                    var parentForm = tree.FindForm() as EduNest.frmNest;
                    parentForm?.Invoke(() => parentForm.RefreshTree());
                }
                catch { }

                // Update all child nodes' Tag paths (recursive resync)
                UpdateNodeTagPaths(draggedNode, destPath);

                SystemSounds.Asterisk.Play();
            }
            catch (Exception ex)
            {
                SystemSounds.Hand.Play();
                MessageBox.Show("Failed to move folder:\r\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Recursively updates all Tag paths of moved nodes to match new folder structure.
        /// </summary>
        private static void UpdateNodeTagPaths(TreeNode node, string currentPath)
        {
            node.Tag = currentPath;

            foreach (TreeNode child in node.Nodes)
            {
                string childPath = Path.Combine(currentPath, Path.GetFileName(child.Tag?.ToString() ?? string.Empty));
                UpdateNodeTagPaths(child, childPath);
            }
        }

        // --- HELPER FUNCTION ---
        private static bool ContainsNode(TreeNode parent, TreeNode child)
        {
            if (child == null) return false;
            if (child.Parent == parent) return true;
            return ContainsNode(parent, child.Parent);
        }
    }
}