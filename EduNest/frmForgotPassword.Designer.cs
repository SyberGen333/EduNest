namespace EduNest
{
    partial class frmForgotPassword
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
            label1 = new Label();
            txtemail = new TextBox();
            btnsendverif = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(131, 25);
            label1.TabIndex = 0;
            label1.Text = "Email (Google):";
            // 
            // txtemail
            // 
            txtemail.Location = new Point(12, 37);
            txtemail.Name = "txtemail";
            txtemail.Size = new Size(458, 31);
            txtemail.TabIndex = 1;
            // 
            // btnsendverif
            // 
            btnsendverif.AutoSize = true;
            btnsendverif.BackColor = Color.White;
            btnsendverif.Cursor = Cursors.Hand;
            btnsendverif.FlatAppearance.BorderSize = 0;
            btnsendverif.FlatStyle = FlatStyle.Flat;
            btnsendverif.Location = new Point(316, 91);
            btnsendverif.Name = "btnsendverif";
            btnsendverif.Size = new Size(154, 50);
            btnsendverif.TabIndex = 3;
            btnsendverif.Text = "&Send Verification";
            btnsendverif.UseVisualStyleBackColor = false;
            // 
            // frmForgotPassword
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 224, 192);
            ClientSize = new Size(482, 153);
            Controls.Add(btnsendverif);
            Controls.Add(txtemail);
            Controls.Add(label1);
            Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.Black;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmForgotPassword";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Verification";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtemail;
        private Button btnsendverif;
    }
}