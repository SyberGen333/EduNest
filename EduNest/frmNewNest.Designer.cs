namespace EduNest
{
    partial class frmNewNest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNewNest));
            label1 = new Label();
            txtname = new TextBox();
            label2 = new Label();
            cmbcourse = new ComboBox();
            btncreate = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(64, 24);
            label1.TabIndex = 0;
            label1.Text = "Name:";
            // 
            // txtname
            // 
            txtname.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtname.Location = new Point(12, 36);
            txtname.Name = "txtname";
            txtname.Size = new Size(428, 31);
            txtname.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 70);
            label2.Name = "label2";
            label2.Size = new Size(97, 24);
            label2.TabIndex = 2;
            label2.Text = "Year Level:";
            // 
            // cmbcourse
            // 
            cmbcourse.Cursor = Cursors.Hand;
            cmbcourse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbcourse.DropDownWidth = 428;
            cmbcourse.FormattingEnabled = true;
            cmbcourse.Items.AddRange(new object[] { "None", "1st", "2nd", "3rd", "4th" });
            cmbcourse.Location = new Point(12, 97);
            cmbcourse.Name = "cmbcourse";
            cmbcourse.Size = new Size(428, 32);
            cmbcourse.TabIndex = 3;
            // 
            // btncreate
            // 
            btncreate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btncreate.Cursor = Cursors.Hand;
            btncreate.Enabled = false;
            btncreate.Image = Properties.Resources.plus;
            btncreate.Location = new Point(244, 148);
            btncreate.Name = "btncreate";
            btncreate.Size = new Size(196, 42);
            btncreate.TabIndex = 6;
            btncreate.Text = "&Create New Nest";
            btncreate.TextAlign = ContentAlignment.MiddleRight;
            btncreate.TextImageRelation = TextImageRelation.ImageBeforeText;
            btncreate.UseVisualStyleBackColor = true;
            // 
            // frmNewNest
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(255, 224, 192);
            ClientSize = new Size(452, 202);
            Controls.Add(btncreate);
            Controls.Add(cmbcourse);
            Controls.Add(label2);
            Controls.Add(txtname);
            Controls.Add(label1);
            Font = new Font("Segoe UI Variable Small", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.Black;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmNewNest";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "New Nest";
            Load += frmNewNest_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtname;
        private Label label2;
        private ComboBox cmbcourse;
        private Button btncreate;
    }
}