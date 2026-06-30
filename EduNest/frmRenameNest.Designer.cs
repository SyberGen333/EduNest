namespace EduNest
{
    partial class frmRenameNest
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
            txtname = new TextBox();
            btnrename = new Button();
            SuspendLayout();
            // 
            // txtname
            // 
            txtname.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtname.Location = new Point(12, 12);
            txtname.Name = "txtname";
            txtname.Size = new Size(599, 30);
            txtname.TabIndex = 2;
            // 
            // btnrename
            // 
            btnrename.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnrename.BackColor = Color.White;
            btnrename.Cursor = Cursors.Hand;
            btnrename.Enabled = false;
            btnrename.FlatAppearance.BorderSize = 0;
            btnrename.FlatStyle = FlatStyle.Flat;
            btnrename.Location = new Point(488, 56);
            btnrename.Name = "btnrename";
            btnrename.Size = new Size(123, 42);
            btnrename.TabIndex = 9;
            btnrename.Text = "&Rename";
            btnrename.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnrename.UseVisualStyleBackColor = false;
            // 
            // frmRenameNest
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.MistyRose;
            ClientSize = new Size(623, 110);
            Controls.Add(btnrename);
            Controls.Add(txtname);
            Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmRenameNest";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Rename Nest";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtname;
        private Button btnrename;
    }
}