using EduNest.Helpers;
using EduNest.Helpers.Nest;
using EduNest.Helpers.Nest.UndoRedo;
using EduNest.Helpers.Nest.Adding;
using EduNest.Helpers.Nest.ContextMenu;
using System.ComponentModel;

namespace EduNest
{
    public partial class frmNest : Form
    {
        // Separate Undo/Redo managers for tree (folders) and files
        private readonly UndoRedoManager _treeUndo = new UndoRedoManager();
        private readonly UndoRedoManager _filesUndo = new UndoRedoManager();
        // guard to avoid scheduling multiple concurrent refresh operations
        private bool _refreshScheduled = false;
        // Variables
        private readonly string folderName; // e.g. "test name1-BSCS-2025-08-24-09-08-14"
        private string fullFolderPath;     // full path built from Documents\EduNest\Nests\<folderName>
        private string currentSelectedFolder = null;    // class-level fields
        public static DateTime LastNestOpenedTime { get; private set; } // to track last opened time

        public frmNest(string folderName)
        {
            InitializeComponent();
            InitializeEvents();
            LastNestOpenedTime = DateTime.Now; // set last opened time
            this.folderName = folderName ?? string.Empty; // avoid null
            RoundedButtonHelper.Install(this, 12);
            // apply small rounded corners to the files DataGridView (keep border visible)
            DataGridViewRoundedHelper.Install(dgvfiles, 5);

            // Log creation/opening of this nest form
            LogMaker.Log($"Opened nest: \"{this.folderName}\"");
        }

        private void UpdateUndoRedoMenuItems()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((Action)UpdateUndoRedoMenuItems);
                    return;
                }

                // files menu items
                undoToolStripMenuItem.Enabled = _filesUndo.CanUndo;
                redoToolStripMenuItem.Enabled = _filesUndo.CanRedo;

                // tree menu items (in tree context menu)
                undoToolStripMenuItem1.Enabled = _treeUndo.CanUndo;
                redoToolStripMenuItem1.Enabled = _treeUndo.CanRedo;
            }
            catch { }
        }

        // Refresh files displayed in the DataGridView for the currently selected folder
        public void RefreshFiles()
        {
            try
            {
                if (treeView1.SelectedNode != null)
                {
                    HandleTreeSelection(new TreeViewEventArgs(treeView1.SelectedNode));
                }
                else if (!string.IsNullOrEmpty(currentSelectedFolder) && Directory.Exists(currentSelectedFolder))
                {
                    DisplayFilesInFolder(currentSelectedFolder);
                }
            }
            catch (Exception ex)
            {
                try { LogMaker.LogException(ex, "RefreshFiles failed"); } catch { }
            }
        }

        // Refresh the entire tree view from current fullFolderPath
        // Use BeginInvoke to defer the update to avoid re-entrancy into TreeView events
        // and protect against concurrent scheduling.
        public void RefreshTree()
        {
            if (string.IsNullOrWhiteSpace(fullFolderPath) || !Directory.Exists(fullFolderPath)) return;

            // If a refresh is already scheduled, skip scheduling another one
            if (_refreshScheduled) return;
            _refreshScheduled = true;

            try
            {
                // Ensure update runs on the UI thread and after current event handlers complete
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    this.BeginInvoke((Action)(() =>
                    {
                        try
                        {
                            treeView1.BeginUpdate();
                            try
                            {
                                treeView1.Nodes.Clear();
                                var rootNode = new TreeNode(folderName) { Tag = fullFolderPath };
                                AddDirectoryNodes(rootNode, fullFolderPath);
                                treeView1.Nodes.Add(rootNode);
                                rootNode.Expand();
                            }
                            finally
                            {
                                try { treeView1.EndUpdate(); } catch { }
                            }
                        }
                        catch (Exception ex)
                        {
                            try { LogMaker.LogException(ex, "RefreshTree failed"); } catch { }
                        }
                        finally
                        {
                            _refreshScheduled = false;
                        }
                    }));
                }
                else
                {
                    // fallback: perform synchronous update if handle not ready
                    try
                    {
                        treeView1.BeginUpdate();
                        try
                        {
                            treeView1.Nodes.Clear();
                            var rootNode = new TreeNode(folderName) { Tag = fullFolderPath };
                            AddDirectoryNodes(rootNode, fullFolderPath);
                            treeView1.Nodes.Add(rootNode);
                            rootNode.Expand();
                        }
                        finally { treeView1.EndUpdate(); }
                    }
                    finally { _refreshScheduled = false; }
                }
            }
            catch
            {
                // ensure flag cleared on unexpected failure
                _refreshScheduled = false;
            }
        }

        // Arrays
        // simple extensible map: extension (without dot) -> resource key name
        private readonly Dictionary<string, string> extMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // document types
            { "doc", "doc" }, { "docx", "doc" },
            { "ppt", "ppt" }, { "pptx", "ppt" },
            { "xls", "xls" }, { "xlsx", "xls" },
            { "pdf", "pdf" },
            { "txt", "txt" },

            // code files
            { "cs", "code" }, { "java", "code" }, { "cpp", "code" }, { "py", "code" }, { "js", "code" }, { "html", "html" }, { "css", "css" },

            // images
            { "png", "img" }, { "jpg", "img" }, { "jpeg", "img" }, { "gif", "img" },

            // archives
            { "zip", "zip" }, { "rar", "zip" }, { "7z", "zip" }, { "tar", "zip" }, { "gz", "zip" },

            // fallback key -> "file"
            { "default", "file" }
        };

        // Methods
        // (frmNest)
        // Setting up name for Nest
        private void SetTitleFromFolderName(string folderName)
        {
            // try using helper (no known codes provided here; heuristic will run)
            var (namePart, courseCode) = NestHelpers.CourseCodeCheck(folderName, null);

            if (!string.IsNullOrEmpty(courseCode))
            {
                if (string.IsNullOrWhiteSpace(namePart))
                    this.Text = $"{courseCode}";
                else
                    this.Text = $"{namePart} ({courseCode})";
            }
            else
            {
                // fallback to raw folderName
                this.Text = folderName;
            }
        }
        // recursively add directories (and optionally files) as child nodes
        private void AddDirectoryNodes(TreeNode parentNode, string directoryPath)
        {
            if (!Directory.Exists(directoryPath)) return;

            // add directories first
            try
            {
                var dirs = Directory.GetDirectories(directoryPath);
                foreach (var dir in dirs)
                {
                    var dirInfo = new DirectoryInfo(dir);
                    var childNode = new TreeNode(dirInfo.Name) { Tag = dir };
                    parentNode.Nodes.Add(childNode);

                    // recursive call to add nested directories
                    AddDirectoryNodes(childNode, dir);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // skip folders we can't access
                var blockedNode = new TreeNode("Access denied") { ForeColor = System.Drawing.Color.Gray };
                parentNode.Nodes.Add(blockedNode);
            }
        }
        // (treeView1)
        private void HandleTreeSelection(TreeViewEventArgs e)
        {
            // Log selected folder when available
            try
            {
                var selectedPath = e.Node?.Tag as string;
                if (!string.IsNullOrWhiteSpace(selectedPath))
                    LogMaker.Log($"Selected folder in nest: \"{selectedPath}\"");
            }
            catch { }

            NestTreeHelper.HandleAfterSelect(e, path => DisplayFilesInFolder(path, txtsearch.Text));
        }
        // (dgvfiles)
        // ensure dgvfiles has the expected columns (call once at load)
        private void EnsureDgvColumns()
        {
            if (dgvfiles.Columns.Count == 0)
            {
                var colImg = new DataGridViewImageColumn
                {
                    Name = "fileType",
                    HeaderText = "",
                    Width = 20,
                    ImageLayout = DataGridViewImageCellLayout.Zoom
                };
                dgvfiles.Columns.Add(colImg);

                var colName = new DataGridViewTextBoxColumn
                {
                    Name = "fileName",
                    HeaderText = "Name",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                };
                dgvfiles.Columns.Add(colName);

                var colDate = new DataGridViewTextBoxColumn
                {
                    Name = "fileDate",
                    HeaderText = "Created",
                    Width = 160
                };
                dgvfiles.Columns.Add(colDate);
            }

            dgvfiles.RowHeadersVisible = false;
            dgvfiles.AllowUserToAddRows = false;
            dgvfiles.AllowUserToDeleteRows = false;
            dgvfiles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        // load an Image from resources according to extension; fallback to 'file'
        private Image GetIconForExtension(string ext)
        {
            // ext may be ".pdf" or "pdf"; normalize
            if (string.IsNullOrEmpty(ext)) ext = "default";
            ext = ext.TrimStart('.');

            string key = extMap.ContainsKey(ext) ? extMap[ext] : extMap["default"];

            try
            {
                var obj = Properties.Resources.ResourceManager.GetObject(key);
                if (obj is System.Drawing.Image img) return img;
            }
            catch { /* ignore */ }

            // final fallback
            try
            {
                var obj2 = Properties.Resources.ResourceManager.GetObject("file");
                if (obj2 is System.Drawing.Image fallback) return fallback;
            }
            catch { }

            return null;
        }
        // Display files in folder for dgvfiles. If folderPath is null, use currently selected node.
        // IMPORTANT: If no node is selected, do nothing (keeps previous selection)
        private void DisplayFilesInFolder(string folderPath, string searchText = null)
        {
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
                return;

            currentSelectedFolder = folderPath;
            var files = Directory.GetFiles(folderPath);

            // apply search filter if any
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                string lowered = searchText.ToLowerInvariant();

                files = files.Where(f =>
                {
                    string nameNoExt = Path.GetFileNameWithoutExtension(f).ToLowerInvariant();

                    // "ignoring characters in between" rule
                    int index = 0;
                    foreach (char c in lowered)
                    {
                        index = nameNoExt.IndexOf(c, index);
                        if (index == -1) return false; // char not found in order
                        index++; // continue search after this char
                    }
                    return true;
                }).ToArray();
            }

            dgvfiles.SuspendLayout();
            dgvfiles.Rows.Clear();

            if (files.Length == 0)
            {
                dgvfiles.ResumeLayout();
                // Log empty folder display
                LogMaker.Log($"Displayed files in folder: \"{folderPath}\" (0 files)");
                return; // leave blank if none
            }

            foreach (var path in files.OrderBy(p => p, StringComparer.OrdinalIgnoreCase))
            {
                try
                {
                    var fi = new FileInfo(path);
                    string nameNoExt = Path.GetFileNameWithoutExtension(fi.Name);
                    string ext = fi.Extension; // includes dot

                    var icon = GetIconForExtension(ext);

                    int r = dgvfiles.Rows.Add();
                    var row = dgvfiles.Rows[r];

                    row.Cells["fileType"].Value = icon;
                    row.Cells["fileName"].Value = nameNoExt;
                    row.Cells["fileDate"].Value = fi.CreationTime.ToString("yyyy/MM/dd-hh-mm-ss");

                    row.Tag = fi.FullName;
                }
                catch
                {
                    // skip problematic files
                }
            }

            if (dgvfiles.Columns.Contains("fileName"))
                dgvfiles.Sort(dgvfiles.Columns["fileName"], ListSortDirection.Ascending);

            dgvfiles.ResumeLayout();

            // Log successful display with count
            try { LogMaker.Log($"Displayed files in folder: \"{folderPath}\" ({files.Length} files)"); } catch { }
        }
        private void ClearAttributes(DirectoryInfo dir)
        {
            foreach (var subDir in dir.GetDirectories())
            {
                ClearAttributes(subDir);
            }

            foreach (var file in dir.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;
            }

            dir.Attributes = FileAttributes.Normal;
        }

        // New helper: copy or move selected files to chosen folder
        private void CopyOrMoveSelectedFiles(bool move)
        {
            // gather selected file paths
            var selected = new List<string>();
            foreach (DataGridViewRow row in dgvfiles.SelectedRows)
            {
                if (row?.Tag is string path && File.Exists(path))
                    selected.Add(path);
            }

            if (selected.Count == 0) return;

            using var dlg = new FolderBrowserDialog();
            dlg.Description = move ? "Select destination folder to move file(s)" : "Select destination folder to copy file(s)";
            dlg.ShowNewFolderButton = true;
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            string destFolder = dlg.SelectedPath;

            try
            {
                foreach (var src in selected)
                {
                    var fileName = Path.GetFileName(src);
                    var destPath = Path.Combine(destFolder, fileName);

                    // ensure unique destination
                    destPath = GetUniqueDestinationPath(destPath);

                    if (move)
                    {
                        File.Move(src, destPath);
                        LogMaker.Log($"Moved file: \"{src}\" -> \"{destPath}\"");
                    }
                    else
                    {
                        File.Copy(src, destPath);
                        LogMaker.Log($"Copied file: \"{src}\" -> \"{destPath}\"");
                    }
                }

                // refresh view
                if (treeView1.SelectedNode != null)
                    HandleTreeSelection(new TreeViewEventArgs(treeView1.SelectedNode));
            }
            catch (Exception ex)
            {
                LogMaker.LogException(ex, move ? "Move failed" : "Copy failed");
                MessageBox.Show((move ? "Move" : "Copy") + " failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string GetUniqueDestinationPath(string path)
        {
            if (!File.Exists(path)) return path;

            var dir = Path.GetDirectoryName(path) ?? "";
            var name = Path.GetFileNameWithoutExtension(path);
            var ext = Path.GetExtension(path);
            int i = 1;
            string candidate;
            do
            {
                candidate = Path.Combine(dir, $"{name} ({i}){ext}");
                i++;
            } while (File.Exists(candidate));
            return candidate;
        }

        // FOR SEARCH
        private void searchTimer_Tick(object sender, EventArgs e)
        {
            searchTimer.Stop();

            string query = txtsearch.Text.Trim();
            if (treeView1.SelectedNode == null) return; // no folder selected yet

            if (string.IsNullOrWhiteSpace(query))
            {
                // refresh original view
                HandleTreeSelection(new TreeViewEventArgs(treeView1.SelectedNode));
                return;
            }

            // get folder path from selected node
            string folderPath = treeView1.SelectedNode.Tag as string;
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath)) return;

            try
            {
                var files = Directory.GetFiles(folderPath);

                // filter: case-insensitive "contains in sequence" match
                var results = files.Where(file =>
                    ContainsInOrder(Path.GetFileNameWithoutExtension(file), query)).ToList();

                if (string.IsNullOrWhiteSpace(txtsearch.Text))
                    DisplayFilesInFolder(currentSelectedFolder); // plain refresh
                else
                    DisplayFilesInFolder(currentSelectedFolder, txtsearch.Text);
            }
            catch (Exception ex)
            {
                LogMaker.LogException(ex, "Search failed");
                MessageBox.Show("Search failed: " + ex.Message);
            }
        }
        private bool ContainsInOrder(string text, string query)
        {
            if (string.IsNullOrEmpty(query)) return true;

            int ti = 0;
            foreach (char qc in query.ToLower())
            {
                ti = text.ToLower().IndexOf(qc, ti);
                if (ti == -1) return false;
                ti++; // move forward
            }
            return true;
        }

        // Open the currently selected folder in File Explorer
        private void OpenSelectedFolderInExplorer()
        {
            string path = currentSelectedFolder;
            if (string.IsNullOrEmpty(path) && treeView1.SelectedNode != null)
                path = treeView1.SelectedNode.Tag as string;

            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                MessageBox.Show("No folder selected or folder does not exist.", "Open in Explorer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("explorer.exe", $"\"{path}\"") { UseShellExecute = true });
                try { LogMaker.Log($"Opened in Explorer: \"{path}\""); } catch { }
            }
            catch (Exception ex)
            {
                LogMaker.LogException(ex, "Open in Explorer failed");
                MessageBox.Show("Failed to open folder in Explorer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Events
        private void InitializeEvents()
        {
            // (Form)
            Load += (s, e) =>
            {
                // build full path using same base as frmNewNest
                var baseNestsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EduNest", "Nests");
                fullFolderPath = Path.Combine(baseNestsFolder, folderName);

                // set form title to "name (COURSECODE)" if possible
                SetTitleFromFolderName(folderName);

                // populate treeView1: root node is the folder's name (no change)
                try
                {
                    treeView1.Nodes.Clear();
                    var rootNode = new TreeNode(folderName) { Tag = fullFolderPath };
                    AddDirectoryNodes(rootNode, fullFolderPath);
                    treeView1.Nodes.Add(rootNode);
                    rootNode.Expand();
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("Nest folder not found: " + fullFolderPath, "Not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load nest structure:\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                EnsureDgvColumns();

                // provide UndoRedoManager instances to helpers that support it
                DgvFilesHelper.UndoManager = _filesUndo;
                NestTreeHelper.UndoManager = _treeUndo;

                // subscribe to undo/redo stack changes to update menu item enabled state
                try
                {
                    _filesUndo.Changed += (s2, e2) => UpdateUndoRedoMenuItems();
                    _treeUndo.Changed += (s2, e2) => UpdateUndoRedoMenuItems();
                }
                catch { }

                // set initial enabled state
                try { UpdateUndoRedoMenuItems(); } catch { }

                // Log that nest was loaded
                try { LogMaker.Log($"Loaded nest form for: \"{folderName}\" path: \"{fullFolderPath}\""); } catch { }
            };
            FormClosed += (s, e) =>
            {
                try
                {
                    // save last opened nest
                    if (!string.IsNullOrWhiteSpace(fullFolderPath))
                        SettingsHelper.SaveLastNest(fullFolderPath);

                    LogMaker.Log($"Closed nest: \"{folderName}\" path: \"{fullFolderPath}\"");
                }
                catch
                {
                    // ignore
                }

                // return to main form
                new frmMain().Show();
            };
            // (treeView1)
            // wire this to treeView1.AfterSelect (call EnsureDgvColumns at load)
            treeView1.AfterSelect += (s, e) => HandleTreeSelection(e);
            treeView1.NodeMouseClick += (s, e) => NestTreeHelper.HandleNodeMouseClick(e, treeView1, cmsTreeView);
            // Updated AfterLabelEdit: call helper then refresh if root node was renamed
            treeView1.AfterLabelEdit += (s, e) =>
            {
                // first let the helper handle renaming on disk / validation
                // capture old path for undo
                string oldPath = e.Node?.Tag as string;
                NestTreeHelper.HandleAfterLabelEdit(e);

                // if rename succeeded, push undo action (oldPath -> newPath)
                try
                {
                    string newPath = e.Node?.Tag as string;
                    if (!string.IsNullOrEmpty(oldPath) && !string.IsNullOrEmpty(newPath) && !string.Equals(oldPath, newPath, StringComparison.OrdinalIgnoreCase))
                    {
                        _treeUndo.AddAction(new UndoAction(oldPath, newPath, UndoActionKind.Rename, "Folder rename"));
                    }
                }
                catch { }

                // refresh tree view after any rename (root or subfolder)
                try { RefreshTree(); } catch { }
            };
            treeView1.BeforeLabelEdit += (s, e) => NestTreeHelper.HandleBeforeLabelEdit(e);
            treeView1.MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    treeView1.SelectedNode = treeView1.GetNodeAt(e.Location);
                    if (treeView1.SelectedNode != null)
                    {
                        ContextMenuPos.AtBottomRight(cmsTreeView);
                        try { LogMaker.Log($"TreeView right-click on node: \"{treeView1.SelectedNode.Tag as string}\""); } catch { }
                    }
                }
            };
            // DRAG & DROP NODE MOVEMENT (handled by helper)
            treeView1.AllowDrop = true;
            treeView1.ItemDrag += NestTreeHelper.HandleItemDrag;
            treeView1.DragEnter += NestTreeHelper.HandleDragEnter;
            treeView1.DragOver += NestTreeHelper.HandleDragOver;
            treeView1.DragDrop += NestTreeHelper.HandleDragDrop;
            // (dgvfiles)
            // Open file when double clicking a row
            dgvfiles.CellDoubleClick += (s, e) =>
            {
                try
                {
                    var filePath = e.RowIndex >= 0 ? dgvfiles.Rows[e.RowIndex].Tag as string : null;
                    LogMaker.Log($"File opened: \"{filePath ?? "<unknown>"}\"");
                }
                catch { }
                DgvFilesHelper.HandleCellDoubleClick(s, e);
            };
            // If user right/left clicks inside the grid but not on a row cell
            dgvfiles.CellMouseClick += (s, e) => DgvFilesHelper.HandleCellMouseDown(s, e, cmsFiles);
            // Handles clicks in the empty/blank space
            dgvfiles.MouseClick += DgvFilesHelper.HandleMouseClick;
            dgvfiles.KeyDown += DgvFilesHelper.HandleKeyDown;
            dgvfiles.CellEndEdit += DgvFilesHelper.HandleCellEndEdit;
            // intercept file rename to add undo action
            dgvfiles.CellEndEdit += (s, e) =>
            {
                // only handle fileName renames
                if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
                var grid = s as DataGridView;
                if (grid == null) return;
                if (grid.Columns[e.ColumnIndex].Name != "fileName") return;

                var row = grid.Rows[e.RowIndex];
                string newPath = row.Tag as string;
                // Note: DgvFilesHelper already moved the file and updated row.Tag
                // We cannot easily get the old path here; attempt to infer from cell's previous value is not trivial.
                // To avoid duplicating logic, do nothing here. The DgvFilesHelper.Move operation could be extended to push undo actions.
            };
            // Drag to add file(s).
            dgvfiles.DragEnter += FilesHelper.Dgvfiles_DragEnter;
            dgvfiles.DragDrop += (s, e) =>
            {
                try { LogMaker.Log("Files dropped into folder via drag-and-drop"); } catch { }
                FilesHelper.Dgvfiles_DragDrop(s, e, this, treeView1, dgvfiles, HandleTreeSelection);
            };
            dgvfiles.MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    var hit = dgvfiles.HitTest(e.X, e.Y);
                    if (hit.RowIndex >= 0)
                    {
                        dgvfiles.ClearSelection();
                        dgvfiles.Rows[hit.RowIndex].Selected = true;

                        ContextMenuPos.AtBottomRight(cmsFiles);
                        try { LogMaker.Log($"Files grid right-click on file: \"{dgvfiles.Rows[hit.RowIndex].Tag as string}\""); } catch { }
                    }
                }
            };

            // CONTEXT MENUS
            // (cmsTreeView)
            renameToolStripMenuItem1.Click += (s, e) => { LogMaker.Log("Tree: rename requested"); TreeContextMenuHelper.Rename(treeView1); };
            deleteToolStripMenuItem1.Click += (s, e) => { LogMaker.Log("Tree: delete requested"); TreeContextMenuHelper.Delete(treeView1); };
            // Tree undo/redo
            undoToolStripMenuItem1.Click += (s, e) =>
            {
                try
                {
                    var act = _treeUndo.Undo();
                    if (act != null)
                    {
                        if (act.Kind == Helpers.Nest.UndoRedo.UndoActionKind.Rename)
                        {
                            // reverse rename: move newPath -> oldPath
                            if (Directory.Exists(act.DestinationPath) && !Directory.Exists(act.SourcePath))
                            {
                                Directory.Move(act.DestinationPath!, act.SourcePath!);
                                // refresh tree
                                try { RefreshTree(); } catch { }
                            }
                        }
                    }
                }
                catch (Exception ex) { LogMaker.LogException(ex, "Undo failed"); }
            };
            redoToolStripMenuItem1.Click += (s, e) =>
            {
                try
                {
                    var act = _treeUndo.Redo();
                    if (act != null)
                    {
                        if (act.Kind == Helpers.Nest.UndoRedo.UndoActionKind.Rename)
                        {
                            if (Directory.Exists(act.SourcePath) && !Directory.Exists(act.DestinationPath))
                            {
                                Directory.Move(act.SourcePath!, act.DestinationPath!);
                                try { RefreshTree(); } catch { }
                            }
                        }
                    }
                }
                catch (Exception ex) { LogMaker.LogException(ex, "Redo failed"); }
            };
            // (cmsFiles)
            renameToolStripMenuItem.Click += (s, e) =>
            {
                try { LogMaker.Log("File: rename requested"); } catch { }
                FileContextMenuHelper.Rename(dgvfiles);
            };
            deleteToolStripMenuItem.Click += (s, e) =>
            {
                try { LogMaker.Log("File: delete requested"); } catch { }
                FileContextMenuHelper.Delete(dgvfiles, treeView1, HandleTreeSelection);
                try
                {
                    this.BeginInvoke((Action)(() =>
                    {
                        try { RefreshTree(); } catch { }
                        try { RefreshFiles(); } catch { }
                    }));
                }
                catch { }
            };
            // Files undo/redo
            undoToolStripMenuItem.Click += (s, e) =>
            {
                try
                {
                    var act = _filesUndo.Undo();
                    if (act != null)
                    {
                        switch (act.Kind)
                        {
                            case Helpers.Nest.UndoRedo.UndoActionKind.Move:
                                // reverse move: move dest -> src
                                if (File.Exists(act.DestinationPath) && !File.Exists(act.SourcePath))
                                {
                                    File.Move(act.DestinationPath!, act.SourcePath!);
                                    try
                                    {
                                        this.BeginInvoke((Action)(() =>
                                        {
                                            try { RefreshTree(); } catch { }
                                            try { RefreshFiles(); } catch { }
                                        }));
                                    }
                                    catch { }
                                }
                                break;
                            case Helpers.Nest.UndoRedo.UndoActionKind.Copy:
                                // undo copy -> delete destination
                                if (File.Exists(act.DestinationPath))
                                {
                                    File.Delete(act.DestinationPath!);
                                    try
                                    {
                                        this.BeginInvoke((Action)(() =>
                                        {
                                            try { RefreshTree(); } catch { }
                                            try { RefreshFiles(); } catch { }
                                        }));
                                    }
                                    catch { }
                                }
                                break;
                            case Helpers.Nest.UndoRedo.UndoActionKind.Rename:
                                if (File.Exists(act.DestinationPath) && !File.Exists(act.SourcePath))
                                {
                                    File.Move(act.DestinationPath!, act.SourcePath!);
                                    try
                                    {
                                        this.BeginInvoke((Action)(() =>
                                        {
                                            try { RefreshTree(); } catch { }
                                            try { RefreshFiles(); } catch { }
                                        }));
                                    }
                                    catch { }
                                }
                                break;
                        }
                    }
                }
                catch (Exception ex) { LogMaker.LogException(ex, "Undo failed"); }
            };
            redoToolStripMenuItem.Click += (s, e) =>
            {
                try
                {
                    var act = _filesUndo.Redo();
                    if (act != null)
                    {
                        switch (act.Kind)
                        {
                            case Helpers.Nest.UndoRedo.UndoActionKind.Move:
                                if (File.Exists(act.SourcePath) && !File.Exists(act.DestinationPath))
                                {
                                    File.Move(act.SourcePath!, act.DestinationPath!);
                                    try
                                    {
                                        this.BeginInvoke((Action)(() =>
                                        {
                                            try { RefreshTree(); } catch { }
                                            try { RefreshFiles(); } catch { }
                                        }));
                                    }
                                    catch { }
                                }
                                break;
                            case Helpers.Nest.UndoRedo.UndoActionKind.Copy:
                                if (File.Exists(act.SourcePath) && !File.Exists(act.DestinationPath))
                                {
                                    File.Copy(act.SourcePath!, act.DestinationPath!);
                                    try
                                    {
                                        this.BeginInvoke((Action)(() =>
                                        {
                                            try { RefreshTree(); } catch { }
                                            try { RefreshFiles(); } catch { }
                                        }));
                                    }
                                    catch { }
                                }
                                break;
                            case Helpers.Nest.UndoRedo.UndoActionKind.Rename:
                                if (File.Exists(act.SourcePath) && !File.Exists(act.DestinationPath))
                                {
                                    File.Move(act.SourcePath!, act.DestinationPath!);
                                    try
                                    {
                                        this.BeginInvoke((Action)(() =>
                                        {
                                            try { RefreshTree(); } catch { }
                                            try { RefreshFiles(); } catch { }
                                        }));
                                    }
                                    catch { }
                                }
                                break;
                        }
                    }
                }
                catch (Exception ex) { LogMaker.LogException(ex, "Redo failed"); }
            };

            // make Copy To functional
            copyToToolStripMenuItem.Click += (s, e) =>
            {
                try { LogMaker.Log("File: copy to requested"); } catch { }
                CopyOrMoveSelectedFiles(false);
                try
                {
                    this.BeginInvoke((Action)(() =>
                    {
                        try { RefreshTree(); } catch { }
                        try { RefreshFiles(); } catch { }
                    }));
                }
                catch { }
            };

            // make Move To functional
            moveToToolStripMenuItem.Click += (s, e) =>
            {
                try { LogMaker.Log("File: move to requested"); } catch { }
                CopyOrMoveSelectedFiles(true);
                try
                {
                    this.BeginInvoke((Action)(() =>
                    {
                        try { RefreshTree(); } catch { }
                        try { RefreshFiles(); } catch { }
                    }));
                }
                catch { }
            };

            // SEARCH TEXTBOX
            txtsearch.TextChanged += (s, e) => SearchHelper.HandleTextChanged(searchTimer);
            txtsearch.KeyPress += (s, e) => SearchHelper.HandleKeyPress(e);
            // BUTTONS
            btnaddfiles.Click += (s, e) => { LogMaker.Log("Add files dialog opened"); FilesHelper.BtnAddFiles_Click(this, treeView1, dgvfiles, HandleTreeSelection); };
            btnaddfolder.Click += (s, e) => { LogMaker.Log($"Add subfolder requested under: \"{treeView1.SelectedNode?.Tag as string}\""); FolderHelper.AddSubfolder(treeView1, treeView1.SelectedNode); };
            // open in Explorer button
            btnopeninexplorer.Click += (s, e) =>
            {
                try { LogMaker.Log("Open in Explorer requested"); } catch { }
                OpenSelectedFolderInExplorer();
            };
        }
    }
}