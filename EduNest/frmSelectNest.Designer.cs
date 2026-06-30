namespace EduNest
{
    partial class frmSelectNest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectNest));
            lbnests = new ListBox();
            btnselect = new Button();
            btnrename = new Button();
            btndelete = new Button();
            SuspendLayout();
            // 
            // lbnests
            // 
            lbnests.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lbnests.Cursor = Cursors.Hand;
            lbnests.FormattingEnabled = true;
            lbnests.Location = new Point(13, 12);
            lbnests.Name = "lbnests";
            lbnests.Size = new Size(458, 388);
            lbnests.TabIndex = 0;
            // 
            // btnselect
            // 
            btnselect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnselect.BackColor = Color.White;
            btnselect.Cursor = Cursors.Hand;
            btnselect.Enabled = false;
            btnselect.FlatAppearance.BorderSize = 0;
            btnselect.FlatStyle = FlatStyle.Flat;
            btnselect.Location = new Point(348, 423);
            btnselect.Name = "btnselect";
            btnselect.Size = new Size(123, 42);
            btnselect.TabIndex = 7;
            btnselect.Text = "&Select";
            btnselect.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnselect.UseVisualStyleBackColor = false;
            // 
            // btnrename
            // 
            btnrename.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnrename.BackColor = Color.White;
            btnrename.Cursor = Cursors.Hand;
            btnrename.Enabled = false;
            btnrename.FlatAppearance.BorderSize = 0;
            btnrename.FlatStyle = FlatStyle.Flat;
            btnrename.Location = new Point(12, 423);
            btnrename.Name = "btnrename";
            btnrename.Size = new Size(123, 42);
            btnrename.TabIndex = 8;
            btnrename.Text = "&Rename";
            btnrename.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnrename.UseVisualStyleBackColor = false;
            // 
            // btndelete
            // 
            btndelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btndelete.BackColor = Color.White;
            btndelete.Cursor = Cursors.Hand;
            btndelete.Enabled = false;
            btndelete.FlatAppearance.BorderSize = 0;
            btndelete.FlatStyle = FlatStyle.Flat;
            btndelete.Location = new Point(141, 423);
            btndelete.Name = "btndelete";
            btndelete.Size = new Size(123, 42);
            btndelete.TabIndex = 9;
            btndelete.Text = "&Delete";
            btndelete.TextImageRelation = TextImageRelation.ImageBeforeText;
            btndelete.UseVisualStyleBackColor = false;
            // 
            // frmSelectNest
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(255, 224, 192);
            ClientSize = new Size(482, 477);
            Controls.Add(btndelete);
            Controls.Add(btnrename);
            Controls.Add(btnselect);
            Controls.Add(lbnests);
            Font = new Font("Segoe UI Variable Small", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.Black;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmSelectNest";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Select a Nest";
            ResumeLayout(false);
        }

        #endregion

        private ListBox lbnests;
        private Button btnselect;
        private Button btnrename;
        private Button btndelete;
    }
}