namespace Launcher_VLCM_niua_lsaj.Forms
{
    partial class Login
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
            label_username = new System.Windows.Forms.Label();
            textBox_username = new System.Windows.Forms.TextBox();
            label_password = new System.Windows.Forms.Label();
            textBox_password = new System.Windows.Forms.TextBox();
            label_server = new System.Windows.Forms.Label();
            textBox_server = new System.Windows.Forms.TextBox();
            label_captcha = new System.Windows.Forms.Label();
            textBox_captcha = new System.Windows.Forms.TextBox();
            pictureBox_captcha = new System.Windows.Forms.PictureBox();
            button_ok = new System.Windows.Forms.Button();
            label_platform = new System.Windows.Forms.Label();
            comboBox_platform = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox_captcha).BeginInit();
            SuspendLayout();
            // 
            // label_username
            // 
            label_username.AutoSize = true;
            label_username.Location = new System.Drawing.Point(3, 9);
            label_username.Name = "label_username";
            label_username.Size = new System.Drawing.Size(60, 13);
            label_username.TabIndex = 0;
            label_username.Text = "Tài khoản:";
            // 
            // textBox_username
            // 
            textBox_username.BackColor = System.Drawing.Color.White;
            textBox_username.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            textBox_username.ForeColor = System.Drawing.Color.Black;
            textBox_username.Location = new System.Drawing.Point(84, 7);
            textBox_username.Name = "textBox_username";
            textBox_username.Size = new System.Drawing.Size(183, 22);
            textBox_username.TabIndex = 1;
            textBox_username.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_password
            // 
            label_password.AutoSize = true;
            label_password.Location = new System.Drawing.Point(3, 39);
            label_password.Name = "label_password";
            label_password.Size = new System.Drawing.Size(59, 13);
            label_password.TabIndex = 2;
            label_password.Text = "Mật khẩu:";
            // 
            // textBox_password
            // 
            textBox_password.BackColor = System.Drawing.Color.White;
            textBox_password.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            textBox_password.ForeColor = System.Drawing.Color.Black;
            textBox_password.Location = new System.Drawing.Point(84, 37);
            textBox_password.Name = "textBox_password";
            textBox_password.Size = new System.Drawing.Size(183, 22);
            textBox_password.TabIndex = 3;
            textBox_password.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            textBox_password.UseSystemPasswordChar = true;
            // 
            // label_server
            // 
            label_server.AutoSize = true;
            label_server.Location = new System.Drawing.Point(3, 71);
            label_server.Name = "label_server";
            label_server.Size = new System.Drawing.Size(41, 13);
            label_server.TabIndex = 4;
            label_server.Text = "Server:";
            // 
            // textBox_server
            // 
            textBox_server.BackColor = System.Drawing.Color.White;
            textBox_server.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            textBox_server.ForeColor = System.Drawing.Color.Black;
            textBox_server.Location = new System.Drawing.Point(83, 69);
            textBox_server.Name = "textBox_server";
            textBox_server.Size = new System.Drawing.Size(183, 22);
            textBox_server.TabIndex = 5;
            textBox_server.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_captcha
            // 
            label_captcha.AutoSize = true;
            label_captcha.Location = new System.Drawing.Point(2, 135);
            label_captcha.Name = "label_captcha";
            label_captcha.Size = new System.Drawing.Size(75, 13);
            label_captcha.TabIndex = 6;
            label_captcha.Text = "Mã xác nhận:";
            // 
            // textBox_captcha
            // 
            textBox_captcha.BackColor = System.Drawing.Color.White;
            textBox_captcha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            textBox_captcha.ForeColor = System.Drawing.Color.Black;
            textBox_captcha.Location = new System.Drawing.Point(83, 133);
            textBox_captcha.Name = "textBox_captcha";
            textBox_captcha.Size = new System.Drawing.Size(127, 22);
            textBox_captcha.TabIndex = 7;
            textBox_captcha.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pictureBox_captcha
            // 
            pictureBox_captcha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pictureBox_captcha.ImageLocation = "";
            pictureBox_captcha.Location = new System.Drawing.Point(216, 133);
            pictureBox_captcha.Name = "pictureBox_captcha";
            pictureBox_captcha.Size = new System.Drawing.Size(50, 22);
            pictureBox_captcha.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            pictureBox_captcha.TabIndex = 8;
            pictureBox_captcha.TabStop = false;
            // 
            // button_ok
            // 
            button_ok.Location = new System.Drawing.Point(112, 161);
            button_ok.Name = "button_ok";
            button_ok.Size = new System.Drawing.Size(45, 23);
            button_ok.TabIndex = 9;
            button_ok.Text = "OK";
            button_ok.UseVisualStyleBackColor = true;
            button_ok.Click += button_ok_Click;
            // 
            // label_platform
            // 
            label_platform.AutoSize = true;
            label_platform.Location = new System.Drawing.Point(3, 103);
            label_platform.Name = "label_platform";
            label_platform.Size = new System.Drawing.Size(53, 13);
            label_platform.TabIndex = 10;
            label_platform.Text = "Platform:";
            // 
            // comboBox_platform
            // 
            comboBox_platform.FormattingEnabled = true;
            comboBox_platform.Location = new System.Drawing.Point(82, 100);
            comboBox_platform.Name = "comboBox_platform";
            comboBox_platform.Size = new System.Drawing.Size(184, 21);
            comboBox_platform.TabIndex = 11;
            comboBox_platform.SelectedIndexChanged += comboBox_platform_SelectedIndexChanged;
            // 
            // Login
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(279, 191);
            Controls.Add(comboBox_platform);
            Controls.Add(label_platform);
            Controls.Add(button_ok);
            Controls.Add(pictureBox_captcha);
            Controls.Add(textBox_captcha);
            Controls.Add(textBox_server);
            Controls.Add(label_captcha);
            Controls.Add(label_server);
            Controls.Add(textBox_password);
            Controls.Add(label_password);
            Controls.Add(textBox_username);
            Controls.Add(label_username);
            DoubleBuffered = true;
            Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            ForeColor = System.Drawing.Color.Black;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            Name = "Login";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Đăng nhập";
            Load += Login_Load;
            KeyDown += Login_KeyDown;
            ((System.ComponentModel.ISupportInitialize)pictureBox_captcha).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label_username;
        private System.Windows.Forms.TextBox textBox_username;
        private System.Windows.Forms.Label label_password;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.Label label_server;
        private System.Windows.Forms.TextBox textBox_server;
        private System.Windows.Forms.Label label_captcha;
        private System.Windows.Forms.TextBox textBox_captcha;
        private System.Windows.Forms.PictureBox pictureBox_captcha;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Label label_platform;
        private System.Windows.Forms.ComboBox comboBox_platform;
    }
}