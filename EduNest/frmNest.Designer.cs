namespace EduNest
{
    partial class frmNest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNest));
            treeView1 = new TreeView();
            dgvfiles = new DataGridView();
            fileType = new DataGridViewImageColumn();
            fileName = new DataGridViewTextBoxColumn();
            fileDate = new DataGridViewTextBoxColumn();
            cmsFiles = new ContextMenuStrip(components);
            renameToolStripMenuItem = new ToolStripMenuItem();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            copyToToolStripMenuItem = new ToolStripMenuItem();
            moveToToolStripMenuItem = new ToolStripMenuItem();
            undoToolStripMenuItem = new ToolStripMenuItem();
            redoToolStripMenuItem = new ToolStripMenuItem();
            txtsearch = new TextBox();
            btnaddfiles = new Button();
            btnaddfolder = new Button();
            cmsTreeView = new ContextMenuStrip(components);
            renameToolStripMenuItem1 = new ToolStripMenuItem();
            deleteToolStripMenuItem1 = new ToolStripMenuItem();
            undoToolStripMenuItem1 = new ToolStripMenuItem();
            redoToolStripMenuItem1 = new ToolStripMenuItem();
            searchTimer = new System.Windows.Forms.Timer(components);
            btnopeninexplorer = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvfiles).BeginInit();
            cmsFiles.SuspendLayout();
            cmsTreeView.SuspendLayout();
            SuspendLayout();
            // 
            // treeView1
            // 
            treeView1.Dock = DockStyle.Left;
            treeView1.Location = new Point(0, 0);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(250, 522);
            treeView1.TabIndex = 0;
            // 
            // dgvfiles
            // 
            dgvfiles.AllowDrop = true;
            dgvfiles.AllowUserToAddRows = false;
            dgvfiles.AllowUserToDeleteRows = false;
            dgvfiles.AllowUserToResizeColumns = false;
            dgvfiles.AllowUserToResizeRows = false;
            dgvfiles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvfiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvfiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvfiles.Columns.AddRange(new DataGridViewColumn[] { fileType, fileName, fileDate });
            dgvfiles.Location = new Point(256, 37);
            dgvfiles.Name = "dgvfiles";
            dgvfiles.RowHeadersVisible = false;
            dgvfiles.RowHeadersWidth = 51;
            dgvfiles.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvfiles.Size = new Size(652, 427);
            dgvfiles.TabIndex = 1;
            // 
            // fileType
            // 
            fileType.FillWeight = 14F;
            fileType.HeaderText = "Type";
            fileType.MinimumWidth = 6;
            fileType.Name = "fileType";
            fileType.ReadOnly = true;
            // 
            // fileName
            // 
            fileName.FillWeight = 89.0909042F;
            fileName.HeaderText = "Name";
            fileName.MinimumWidth = 6;
            fileName.Name = "fileName";
            fileName.ReadOnly = true;
            // 
            // fileDate
            // 
            fileDate.FillWeight = 32F;
            fileDate.HeaderText = "Date";
            fileDate.MinimumWidth = 6;
            fileDate.Name = "fileDate";
            fileDate.ReadOnly = true;
            // 
            // cmsFiles
            // 
            cmsFiles.ImageScalingSize = new Size(20, 20);
            cmsFiles.Items.AddRange(new ToolStripItem[] { renameToolStripMenuItem, deleteToolStripMenuItem, undoToolStripMenuItem, redoToolStripMenuItem, copyToToolStripMenuItem, moveToToolStripMenuItem });
            cmsFiles.Name = "contextMenuStrip1";
            cmsFiles.Size = new Size(140, 160);
            // 
            // renameToolStripMenuItem
            // 
            renameToolStripMenuItem.Image = Properties.Resources.pencil;
            renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            renameToolStripMenuItem.Size = new Size(139, 26);
            renameToolStripMenuItem.Text = "&Rename";
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Image = Properties.Resources.close;
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new Size(139, 26);
            deleteToolStripMenuItem.Text = "&Delete";
            // 
            // copyToToolStripMenuItem
            // 
            copyToToolStripMenuItem.Image = Properties.Resources.file;
            copyToToolStripMenuItem.Name = "copyToToolStripMenuItem";
            copyToToolStripMenuItem.Size = new Size(139, 26);
            copyToToolStripMenuItem.Text = "Copy To";
            // 
            // moveToToolStripMenuItem
            // 
            moveToToolStripMenuItem.Image = Properties.Resources.file;
            moveToToolStripMenuItem.Name = "moveToToolStripMenuItem";
            moveToToolStripMenuItem.Size = new Size(139, 26);
            moveToToolStripMenuItem.Text = "Move To";
            // 
            // undoToolStripMenuItem
            // 
            undoToolStripMenuItem.Enabled = false;
            undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            undoToolStripMenuItem.Size = new Size(139, 26);
            undoToolStripMenuItem.Text = "Undo";
            // 
            // redoToolStripMenuItem
            // 
            redoToolStripMenuItem.Enabled = false;
            redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            redoToolStripMenuItem.Size = new Size(139, 26);
            redoToolStripMenuItem.Text = "Redo";
            // 
            // txtsearch
            // 
            txtsearch.Dock = DockStyle.Top;
            txtsearch.Location = new Point(250, 0);
            txtsearch.Name = "txtsearch";
            txtsearch.PlaceholderText = "Search";
            txtsearch.Size = new Size(670, 31);
            txtsearch.TabIndex = 3;
            // 
            // btnaddfiles
            // 
            btnaddfiles.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnaddfiles.BackColor = Color.White;
            btnaddfiles.Cursor = Cursors.Hand;
            btnaddfiles.FlatAppearance.BorderSize = 0;
            btnaddfiles.FlatAppearance.MouseDownBackColor = Color.Silver;
            btnaddfiles.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 224, 224);
            btnaddfiles.FlatStyle = FlatStyle.Flat;
            btnaddfiles.Image = Properties.Resources.plus;
            btnaddfiles.Location = new Point(257, 470);
            btnaddfiles.Name = "btnaddfiles";
            btnaddfiles.Size = new Size(213, 40);
            btnaddfiles.TabIndex = 4;
            btnaddfiles.Text = "&Add File(s)";
            btnaddfiles.TextAlign = ContentAlignment.MiddleRight;
            btnaddfiles.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnaddfiles.UseVisualStyleBackColor = false;
            // 
            // btnaddfolder
            // 
            btnaddfolder.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnaddfolder.BackColor = Color.White;
            btnaddfolder.Cursor = Cursors.Hand;
            btnaddfolder.FlatAppearance.BorderSize = 0;
            btnaddfolder.FlatAppearance.MouseDownBackColor = Color.Silver;
            btnaddfolder.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 224, 224);
            btnaddfolder.FlatStyle = FlatStyle.Flat;
            btnaddfolder.Image = Properties.Resources.folder;
            btnaddfolder.Location = new Point(476, 470);
            btnaddfolder.Name = "btnaddfolder";
            btnaddfolder.Size = new Size(213, 40);
            btnaddfolder.TabIndex = 5;
            btnaddfolder.Text = "&Add Folder";
            btnaddfolder.TextAlign = ContentAlignment.MiddleRight;
            btnaddfolder.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnaddfolder.UseVisualStyleBackColor = false;
            // 
            // cmsTreeView
            // 
            cmsTreeView.ImageScalingSize = new Size(20, 20);
            cmsTreeView.Items.AddRange(new ToolStripItem[] { renameToolStripMenuItem1, deleteToolStripMenuItem1, undoToolStripMenuItem1, redoToolStripMenuItem1 });
            cmsTreeView.Name = "cmsTreeView";
            cmsTreeView.Size = new Size(133, 100);
            // 
            // renameToolStripMenuItem1
            // 
            renameToolStripMenuItem1.Name = "renameToolStripMenuItem1";
            renameToolStripMenuItem1.Size = new Size(132, 24);
            renameToolStripMenuItem1.Text = "Rename";
            // 
            // deleteToolStripMenuItem1
            // 
            deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            deleteToolStripMenuItem1.Size = new Size(132, 24);
            deleteToolStripMenuItem1.Text = "Delete";
            // 
            // undoToolStripMenuItem1
            // 
            undoToolStripMenuItem1.Enabled = false;
            undoToolStripMenuItem1.Name = "undoToolStripMenuItem1";
            undoToolStripMenuItem1.Size = new Size(132, 24);
            undoToolStripMenuItem1.Text = "Undo";
            // 
            // redoToolStripMenuItem1
            // 
            redoToolStripMenuItem1.Enabled = false;
            redoToolStripMenuItem1.Name = "redoToolStripMenuItem1";
            redoToolStripMenuItem1.Size = new Size(132, 24);
            redoToolStripMenuItem1.Text = "Redo";
            // 
            // searchTimer
            // 
            searchTimer.Interval = 200;
            searchTimer.Tick += searchTimer_Tick;
            // 
            // btnopeninexplorer
            // 
            btnopeninexplorer.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnopeninexplorer.BackColor = Color.White;
            btnopeninexplorer.Cursor = Cursors.Hand;
            btnopeninexplorer.FlatAppearance.BorderSize = 0;
            btnopeninexplorer.FlatAppearance.MouseDownBackColor = Color.Silver;
            btnopeninexplorer.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 224, 224);
            btnopeninexplorer.FlatStyle = FlatStyle.Flat;
            btnopeninexplorer.Image = Properties.Resources.folder;
            btnopeninexplorer.Location = new Point(695, 470);
            btnopeninexplorer.Name = "btnopeninexplorer";
            btnopeninexplorer.Size = new Size(213, 40);
            btnopeninexplorer.TabIndex = 6;
            btnopeninexplorer.Text = "&Open in Explorer";
            btnopeninexplorer.TextAlign = ContentAlignment.MiddleRight;
            btnopeninexplorer.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnopeninexplorer.UseVisualStyleBackColor = false;
            // 
            // frmNest
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(255, 192, 192);
            ClientSize = new Size(920, 522);
            Controls.Add(btnopeninexplorer);
            Controls.Add(btnaddfolder);
            Controls.Add(btnaddfiles);
            Controls.Add(txtsearch);
            Controls.Add(dgvfiles);
            Controls.Add(treeView1);
            Font = new Font("Segoe UI Variable Small", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmNest";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmNest";
            ((System.ComponentModel.ISupportInitialize)dgvfiles).EndInit();
            cmsFiles.ResumeLayout(false);
            cmsTreeView.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TreeView treeView1;
        private DataGridView dgvfiles;
        private ContextMenuStrip cmsFiles;
        private TextBox txtsearch;
        private Button btnaddfiles;
        private Button btnaddfolder;
        private ToolStripMenuItem renameToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem moveToToolStripMenuItem;
        private ToolStripMenuItem copyToToolStripMenuItem;
        private ContextMenuStrip cmsTreeView;
        private ToolStripMenuItem renameToolStripMenuItem1;
        private ToolStripMenuItem deleteToolStripMenuItem1;
        private DataGridViewImageColumn fileType;
        private DataGridViewTextBoxColumn fileName;
        private DataGridViewTextBoxColumn fileDate;
        private System.Windows.Forms.Timer searchTimer;
        private Button btnopeninexplorer;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripMenuItem undoToolStripMenuItem1;
        private ToolStripMenuItem redoToolStripMenuItem1;
    }
}