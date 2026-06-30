namespace EduNest
{
    partial class frmPassword
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
            txtpassword = new TextBox();
            passwordHiderLabel = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            btndelete = new Button();
            btncreateorlogin = new Button();
            btnforgotpassword = new Label();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(160, 25);
            label1.TabIndex = 0;
            label1.Text = "Create a Password:";
            // 
            // txtpassword
            // 
            txtpassword.Location = new Point(12, 37);
            txtpassword.Name = "txtpassword";
            txtpassword.Size = new Size(376, 31);
            txtpassword.TabIndex = 1;
            // 
            // passwordHiderLabel
            // 
            passwordHiderLabel.BackColor = Color.White;
            passwordHiderLabel.Cursor = Cursors.Hand;
            passwordHiderLabel.Location = new Point(394, 37);
            passwordHiderLabel.Name = "passwordHiderLabel";
            passwordHiderLabel.Padding = new Padding(6, 0, 6, 0);
            passwordHiderLabel.Size = new Size(35, 30);
            passwordHiderLabel.TabIndex = 5;
            passwordHiderLabel.Text = "🔒";
            passwordHiderLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(btndelete);
            flowLayoutPanel1.Controls.Add(btncreateorlogin);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(0, 101);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(441, 60);
            flowLayoutPanel1.TabIndex = 2;
            flowLayoutPanel1.Paint += flowLayoutPanel1_Paint;
            // 
            // btndelete
            // 
            btndelete.AutoSize = true;
            btndelete.BackColor = Color.White;
            btndelete.Cursor = Cursors.Hand;
            btndelete.FlatAppearance.BorderSize = 0;
            btndelete.FlatStyle = FlatStyle.Flat;
            btndelete.Location = new Point(344, 3);
            btndelete.Name = "btndelete";
            btndelete.Size = new Size(94, 50);
            btndelete.TabIndex = 3;
            btndelete.Text = "&Delete";
            btndelete.UseVisualStyleBackColor = false;
            btndelete.Visible = false;
            // 
            // btncreateorlogin
            // 
            btncreateorlogin.AutoSize = true;
            btncreateorlogin.BackColor = Color.White;
            btncreateorlogin.Cursor = Cursors.Hand;
            btncreateorlogin.FlatAppearance.BorderSize = 0;
            btncreateorlogin.FlatStyle = FlatStyle.Flat;
            btncreateorlogin.Location = new Point(244, 3);
            btncreateorlogin.Name = "btncreateorlogin";
            btncreateorlogin.Size = new Size(94, 50);
            btncreateorlogin.TabIndex = 2;
            btncreateorlogin.Text = "&Create";
            btncreateorlogin.UseVisualStyleBackColor = false;
            // 
            // btnforgotpassword
            // 
            btnforgotpassword.AutoSize = true;
            btnforgotpassword.Cursor = Cursors.Hand;
            btnforgotpassword.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnforgotpassword.ForeColor = Color.Blue;
            btnforgotpassword.Location = new Point(12, 71);
            btnforgotpassword.Name = "btnforgotpassword";
            btnforgotpassword.Size = new Size(161, 25);
            btnforgotpassword.TabIndex = 4;
            btnforgotpassword.Text = "&Forgot Password?";
            // 
            // frmPassword
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 224, 192);
            ClientSize = new Size(441, 161);
            Controls.Add(passwordHiderLabel);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(btnforgotpassword);
            Controls.Add(txtpassword);
            Controls.Add(label1);
            Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.Black;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmPassword";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Create a Password";
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtpassword;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btndelete;
        private Button btncreateorlogin;
        private Label passwordHiderLabel;
        private Label btnforgotpassword;
    }
}