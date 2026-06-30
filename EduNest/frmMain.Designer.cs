namespace EduNest
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            label1 = new Label();
            btnnewnest = new Button();
            btnopennest = new Button();
            btnselectnest = new Button();
            panelTitle = new Panel();
            panelAccount = new Panel();
            labelPasswordStatus = new Label();
            btnpassword = new Button();
            btnappdirectory = new Button();
            btnsettings = new Button();
            panelTitle.SuspendLayout();
            panelAccount.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI Variable Display", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(250, 353);
            label1.TabIndex = 1;
            label1.Text = "Welcome to\r\nEduNest!";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnnewnest
            // 
            btnnewnest.BackColor = Color.White;
            btnnewnest.Cursor = Cursors.Hand;
            btnnewnest.FlatAppearance.BorderSize = 0;
            btnnewnest.FlatAppearance.MouseDownBackColor = Color.DarkGray;
            btnnewnest.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 224, 224);
            btnnewnest.FlatStyle = FlatStyle.Flat;
            btnnewnest.Image = Properties.Resources.edunest_new_nest;
            btnnewnest.Location = new Point(273, 74);
            btnnewnest.Name = "btnnewnest";
            btnnewnest.Size = new Size(150, 160);
            btnnewnest.TabIndex = 3;
            btnnewnest.Text = "&New Nest";
            btnnewnest.TextImageRelation = TextImageRelation.ImageAboveText;
            btnnewnest.UseVisualStyleBackColor = false;
            // 
            // btnopennest
            // 
            btnopennest.BackColor = Color.White;
            btnopennest.Cursor = Cursors.Hand;
            btnopennest.Enabled = false;
            btnopennest.FlatAppearance.BorderSize = 0;
            btnopennest.FlatAppearance.MouseDownBackColor = Color.DarkGray;
            btnopennest.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 224, 224);
            btnopennest.FlatStyle = FlatStyle.Flat;
            btnopennest.Image = (Image)resources.GetObject("btnopennest.Image");
            btnopennest.Location = new Point(429, 74);
            btnopennest.Name = "btnopennest";
            btnopennest.Size = new Size(150, 160);
            btnopennest.TabIndex = 4;
            btnopennest.Text = "&Load Last Nest";
            btnopennest.TextImageRelation = TextImageRelation.ImageAboveText;
            btnopennest.UseVisualStyleBackColor = false;
            // 
            // btnselectnest
            // 
            btnselectnest.BackColor = Color.White;
            btnselectnest.Cursor = Cursors.Hand;
            btnselectnest.Enabled = false;
            btnselectnest.FlatAppearance.BorderSize = 0;
            btnselectnest.FlatAppearance.MouseDownBackColor = Color.DarkGray;
            btnselectnest.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 224, 224);
            btnselectnest.FlatStyle = FlatStyle.Flat;
            btnselectnest.Image = (Image)resources.GetObject("btnselectnest.Image");
            btnselectnest.Location = new Point(585, 74);
            btnselectnest.Name = "btnselectnest";
            btnselectnest.Size = new Size(150, 160);
            btnselectnest.TabIndex = 5;
            btnselectnest.Text = "&Select Nest";
            btnselectnest.TextImageRelation = TextImageRelation.ImageAboveText;
            btnselectnest.UseVisualStyleBackColor = false;
            // 
            // panelTitle
            // 
            panelTitle.BackColor = Color.White;
            panelTitle.Controls.Add(label1);
            panelTitle.Dock = DockStyle.Left;
            panelTitle.Location = new Point(0, 0);
            panelTitle.Name = "panelTitle";
            panelTitle.Size = new Size(250, 353);
            panelTitle.TabIndex = 7;
            // 
            // panelAccount
            // 
            panelAccount.BackColor = Color.FromArgb(192, 64, 0);
            panelAccount.Controls.Add(labelPasswordStatus);
            panelAccount.Controls.Add(btnpassword);
            panelAccount.Dock = DockStyle.Bottom;
            panelAccount.Location = new Point(250, 293);
            panelAccount.Name = "panelAccount";
            panelAccount.Padding = new Padding(5);
            panelAccount.Size = new Size(497, 60);
            panelAccount.TabIndex = 8;
            // 
            // labelPasswordStatus
            // 
            labelPasswordStatus.Dock = DockStyle.Fill;
            labelPasswordStatus.ForeColor = Color.White;
            labelPasswordStatus.Location = new Point(5, 5);
            labelPasswordStatus.Name = "labelPasswordStatus";
            labelPasswordStatus.Size = new Size(287, 50);
            labelPasswordStatus.TabIndex = 1;
            labelPasswordStatus.Text = "Password protect this app >";
            labelPasswordStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnpassword
            // 
            btnpassword.BackColor = Color.White;
            btnpassword.Cursor = Cursors.Hand;
            btnpassword.Dock = DockStyle.Right;
            btnpassword.FlatAppearance.BorderSize = 0;
            btnpassword.FlatStyle = FlatStyle.Flat;
            btnpassword.Location = new Point(292, 5);
            btnpassword.Name = "btnpassword";
            btnpassword.Size = new Size(200, 50);
            btnpassword.TabIndex = 0;
            btnpassword.Text = "&Setup a Password";
            btnpassword.UseVisualStyleBackColor = false;
            // 
            // btnappdirectory
            // 
            btnappdirectory.BackColor = Color.White;
            btnappdirectory.Cursor = Cursors.Hand;
            btnappdirectory.FlatAppearance.BorderSize = 0;
            btnappdirectory.FlatStyle = FlatStyle.Flat;
            btnappdirectory.Image = Properties.Resources.folder;
            btnappdirectory.Location = new Point(629, 240);
            btnappdirectory.Name = "btnappdirectory";
            btnappdirectory.Size = new Size(50, 30);
            btnappdirectory.TabIndex = 2;
            btnappdirectory.Text = "&";
            btnappdirectory.UseVisualStyleBackColor = false;
            // 
            // btnsettings
            // 
            btnsettings.BackColor = Color.White;
            btnsettings.Cursor = Cursors.Hand;
            btnsettings.FlatAppearance.BorderSize = 0;
            btnsettings.FlatStyle = FlatStyle.Flat;
            btnsettings.Image = Properties.Resources.cogwheel;
            btnsettings.Location = new Point(685, 240);
            btnsettings.Name = "btnsettings";
            btnsettings.Size = new Size(50, 30);
            btnsettings.TabIndex = 9;
            btnsettings.Text = "&";
            btnsettings.UseVisualStyleBackColor = false;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(255, 224, 192);
            ClientSize = new Size(747, 353);
            Controls.Add(btnsettings);
            Controls.Add(btnappdirectory);
            Controls.Add(panelAccount);
            Controls.Add(panelTitle);
            Controls.Add(btnselectnest);
            Controls.Add(btnopennest);
            Controls.Add(btnnewnest);
            Font = new Font("Segoe UI Variable Small", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.Black;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "frmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "EduNest - Educational Workload Organizer";
            FormClosing += frmMain_FormClosing;
            Load += frmMain_Load;
            panelTitle.ResumeLayout(false);
            panelAccount.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Label label1;
        private Button btnnewnest;
        private Button btnopennest;
        private Button btnselectnest;
        private Panel panelTitle;
        private Panel panelAccount;
        private Button btnpassword;
        private Label labelPasswordStatus;
        private Button btnappdirectory;
        private Button btnsettings;
    }
}