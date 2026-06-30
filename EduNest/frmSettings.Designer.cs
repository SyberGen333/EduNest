namespace EduNest
{
    partial class frmSettings
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
            cbmakeabackup = new CheckBox();
            cbdologadding = new CheckBox();
            SuspendLayout();
            // 
            // cbmakeabackup
            // 
            cbmakeabackup.AutoSize = true;
            cbmakeabackup.Checked = true;
            cbmakeabackup.CheckState = CheckState.Checked;
            cbmakeabackup.Cursor = Cursors.Hand;
            cbmakeabackup.ForeColor = Color.Black;
            cbmakeabackup.Location = new Point(13, 13);
            cbmakeabackup.Margin = new Padding(4);
            cbmakeabackup.Name = "cbmakeabackup";
            cbmakeabackup.Size = new Size(252, 29);
            cbmakeabackup.TabIndex = 0;
            cbmakeabackup.Text = "&Create a Backup every close";
            cbmakeabackup.UseVisualStyleBackColor = true;
            // 
            // cbdologadding
            // 
            cbdologadding.AutoSize = true;
            cbdologadding.Checked = true;
            cbdologadding.CheckState = CheckState.Checked;
            cbdologadding.Cursor = Cursors.Hand;
            cbdologadding.ForeColor = Color.Black;
            cbdologadding.Location = new Point(13, 50);
            cbdologadding.Margin = new Padding(4);
            cbdologadding.Name = "cbdologadding";
            cbdologadding.Size = new Size(150, 29);
            cbdologadding.TabIndex = 1;
            cbdologadding.Text = "&Do log adding";
            cbdologadding.UseVisualStyleBackColor = true;
            // 
            // frmSettings
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 224, 192);
            ClientSize = new Size(382, 253);
            Controls.Add(cbdologadding);
            Controls.Add(cbmakeabackup);
            Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmSettings";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Settings";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox cbmakeabackup;
        private CheckBox cbdologadding;
    }
}