namespace EduNest.Helpers.Nest.Adding
{
    using EduNest.Helpers.Nest.UndoRedo;
    public static class FilesHelper
    {
        // -----------------------
        // METHODS
        // -----------------------

        // Ask whether to Move (Yes), Copy (No) or Cancel
        private static DialogResult AskMoveOrCopy(IWin32Window owner, int count)
        {
            string msg = count == 1
                ? "Do you want to move or copy the selected file?"
                : $"Do you want to move or copy the {count} selected files?";
            msg += "\nYes = Move\nNo = Copy";
            // Yes = Move, No = Copy, Cancel = Cancel
            return MessageBox.Show(owner, msg, "Add Files", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        }

        // Create a unique filename by adding (1), (2), ... before extension
        private static string GetUniqueFilePath(string path)
        {
            var dir = Path.GetDirectoryName(path) ?? "";
            var name = Path.GetFileNameWithoutExtension(path);
            var ext = Path.GetExtension(path);
            int i = 1;
            string candidate;
            do
            {
                candidate = Path.Combine(dir, $"{name} ({i}){ext}");
                i++;
            }
            while (File.Exists(candidate));
            return candidate;
        }

        /// <summary>
        /// Core: add files/folders into targetFolder. If a source is a folder, it will add the files found at the folder root (non-recursive).
        /// The method asks the user Move/Copy/Cancel and performs action accordingly. After operation completes it calls refreshCallback
        /// with the currently selected node (if provided).
        /// </summary>
        /// <param name="owner">Owner window for dialogs (pass 'this' from the form)</param>
        /// <param name="sourcePaths">Files and/or folders dropped/selected</param>
        /// <param name="targetFolder">Destination folder path</param>
        /// <param name="treeView">TreeView (used to get the selected node for refresh)</param>
        /// <param name="refreshCallback">Action to call to refresh the current node: e.g. args => treeView1_AfterSelect(treeView1, args)</param>
        public static void AddFilesToFolder(IWin32Window owner, IEnumerable<string> sourcePaths, string targetFolder, TreeView treeView, Action<TreeViewEventArgs> refreshCallback, UndoRedo.UndoRedoManager? manager = null)
        {
            if (sourcePaths == null) return;
            if (string.IsNullOrWhiteSpace(targetFolder) || !Directory.Exists(targetFolder)) return;

            var sources = sourcePaths.ToArray();
            var dlg = AskMoveOrCopy(owner, sources.Length);
            if (dlg == DialogResult.Cancel) return;

            bool doMove = dlg == DialogResult.Yes; // Yes => Move, No => Copy
            var errors = new List<string>();
            int successCount = 0;

            foreach (var src in sources)
            {
                try
                {
                    if (File.Exists(src))
                    {
                        string destPath = Path.Combine(targetFolder, Path.GetFileName(src));
                        if (File.Exists(destPath))
                            destPath = GetUniqueFilePath(destPath);

                        if (doMove) 
                        {
                            File.Move(src, destPath);
                            try { manager?.AddAction(new UndoRedo.UndoAction(src, destPath, UndoRedo.UndoActionKind.Move, "Move file")); } catch { }
                        }
                        else 
                        {
                            File.Copy(src, destPath);
                            try { manager?.AddAction(new UndoRedo.UndoAction(src, destPath, UndoRedo.UndoActionKind.Copy, "Copy file")); } catch { }
                        }

                        successCount++;
                    }
                    else if (Directory.Exists(src))
                    {
                        // add files inside this folder (non-recursive)
                        var files = Directory.GetFiles(src);
                        foreach (var f in files)
                        {
                            try
                            {
                                string destPath = Path.Combine(targetFolder, Path.GetFileName(f));
                                if (File.Exists(destPath))
                                    destPath = GetUniqueFilePath(destPath);

                                if (doMove) 
                                {
                                    File.Move(f, destPath);
                                    try { manager?.AddAction(new UndoRedo.UndoAction(f, destPath, UndoRedo.UndoActionKind.Move, "Move file")); } catch { }
                                }
                                else
                                {
                                    File.Copy(f, destPath);
                                    try { manager?.AddAction(new UndoRedo.UndoAction(f, destPath, UndoRedo.UndoActionKind.Copy, "Copy file")); } catch { }
                                }

                                successCount++;
                            }
                            catch (Exception exFile)
                            {
                                errors.Add($"'{f}': {exFile.Message}");
                            }
                        }
                    }
                    else
                    {
                        errors.Add($"Source not found: {src}");
                    }
                }
                catch (Exception ex)
                {
                    errors.Add($"'{src}': {ex.Message}");
                }
            }

            // show summary of operation if there were errors
            if (errors.Count > 0)
            {
                var sb = "Some items could not be processed:\r\n\r\n" + string.Join("\r\n", errors.Take(25));
                if (errors.Count > 25) sb += $"\r\n...and {errors.Count - 25} more.";
                MessageBox.Show(owner, sb, "Add Files - Errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // optional short success message
            if (successCount > 0)
            {
                // no popup required; uncomment the following line to show a confirmation
                // MessageBox.Show(owner, $"Successfully added {successCount} file(s).", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // refresh the currently selected node in the TreeView via provided callback
            if (refreshCallback != null && treeView?.SelectedNode != null)
            {
                refreshCallback(new TreeViewEventArgs(treeView.SelectedNode));
                try { (treeView.FindForm() as EduNest.frmNest)?.Invoke(() => (treeView.FindForm() as EduNest.frmNest)?.RefreshTree()); } catch { }
            }
        }

        // -----------------------
        // EVENTS (helpers to wire from frmNest)
        // -----------------------

        /// <summary>
        /// Call from your frmNest.btnaddfiles_Click: 
        /// FilesHelper.BtnAddFiles_Click(this, treeView1, dgvfiles, args => treeView1_AfterSelect(treeView1, args));
        /// </summary>
        public static void BtnAddFiles_Click(IWin32Window owner, TreeView treeView, DataGridView dgvfiles, Action<TreeViewEventArgs> refreshCallback)
        {
            if (treeView?.SelectedNode == null) return;

            string targetFolder = treeView.SelectedNode.Tag as string;
            if (string.IsNullOrWhiteSpace(targetFolder) || !Directory.Exists(targetFolder)) return;

            using (var ofd = new OpenFileDialog())
            {
                ofd.Multiselect = true;
                ofd.Title = "Select files to add";
                ofd.Filter = "All files (*.*)|*.*";
                if (ofd.ShowDialog(owner) != DialogResult.OK) return;

                var files = ofd.FileNames;
                if (files == null || files.Length == 0) return;

                AddFilesToFolder(owner, files, targetFolder, treeView, refreshCallback, DgvFilesHelper.UndoManager);
            }
        }

        /// <summary>
        /// Wire to dgvfiles.DragEnter
        /// </summary>
        public static void Dgvfiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Wire to dgvfiles.DragDrop. Example call:
        /// FilesHelper.Dgvfiles_DragDrop(sender, e, this, treeView1, dgvfiles, args => treeView1_AfterSelect(treeView1, args));
        /// </summary>
        public static void Dgvfiles_DragDrop(object sender, DragEventArgs e, IWin32Window owner, TreeView treeView, DataGridView dgvfiles, Action<TreeViewEventArgs> refreshCallback)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            if (treeView?.SelectedNode == null) return;

            var targetFolder = treeView.SelectedNode.Tag as string;
            if (string.IsNullOrWhiteSpace(targetFolder) || !Directory.Exists(targetFolder)) return;

            var paths = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (paths == null || paths.Length == 0) return;

            AddFilesToFolder(owner, paths, targetFolder, treeView, refreshCallback, DgvFilesHelper.UndoManager);
        }
    }
}