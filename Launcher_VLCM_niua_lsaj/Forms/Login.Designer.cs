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
            this.label_username = new System.Windows.Forms.Label();
            this.textBox_username = new System.Windows.Forms.TextBox();
            this.label_password = new System.Windows.Forms.Label();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.label_server = new System.Windows.Forms.Label();
            this.textBox_server = new System.Windows.Forms.TextBox();
            this.label_captcha = new System.Windows.Forms.Label();
            this.textBox_captcha = new System.Windows.Forms.TextBox();
            this.pictureBox_captcha = new System.Windows.Forms.PictureBox();
            this.button_ok = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_captcha)).BeginInit();
            this.SuspendLayout();
            // 
            // label_username
            // 
            this.label_username.AutoSize = true;
            this.label_username.Location = new System.Drawing.Point(3, 9);
            this.label_username.Name = "label_username";
            this.label_username.Size = new System.Drawing.Size(60, 13);
            this.label_username.TabIndex = 0;
            this.label_username.Text = "Tài khoản:";
            // 
            // textBox_username
            // 
            this.textBox_username.BackColor = System.Drawing.Color.White;
            this.textBox_username.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_username.ForeColor = System.Drawing.Color.Black;
            this.textBox_username.Location = new System.Drawing.Point(84, 7);
            this.textBox_username.Name = "textBox_username";
            this.textBox_username.Size = new System.Drawing.Size(183, 22);
            this.textBox_username.TabIndex = 1;
            this.textBox_username.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_password
            // 
            this.label_password.AutoSize = true;
            this.label_password.Location = new System.Drawing.Point(3, 39);
            this.label_password.Name = "label_password";
            this.label_password.Size = new System.Drawing.Size(59, 13);
            this.label_password.TabIndex = 2;
            this.label_password.Text = "Mật khẩu:";
            // 
            // textBox_password
            // 
            this.textBox_password.BackColor = System.Drawing.Color.White;
            this.textBox_password.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_password.ForeColor = System.Drawing.Color.Black;
            this.textBox_password.Location = new System.Drawing.Point(84, 37);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.Size = new System.Drawing.Size(183, 22);
            this.textBox_password.TabIndex = 3;
            this.textBox_password.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_password.UseSystemPasswordChar = true;
            // 
            // label_server
            // 
            this.label_server.AutoSize = true;
            this.label_server.Location = new System.Drawing.Point(3, 67);
            this.label_server.Name = "label_server";
            this.label_server.Size = new System.Drawing.Size(41, 13);
            this.label_server.TabIndex = 4;
            this.label_server.Text = "Server:";
            // 
            // textBox_server
            // 
            this.textBox_server.BackColor = System.Drawing.Color.White;
            this.textBox_server.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_server.ForeColor = System.Drawing.Color.Black;
            this.textBox_server.Location = new System.Drawing.Point(84, 65);
            this.textBox_server.Name = "textBox_server";
            this.textBox_server.Size = new System.Drawing.Size(183, 22);
            this.textBox_server.TabIndex = 5;
            this.textBox_server.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_captcha
            // 
            this.label_captcha.AutoSize = true;
            this.label_captcha.Location = new System.Drawing.Point(3, 96);
            this.label_captcha.Name = "label_captcha";
            this.label_captcha.Size = new System.Drawing.Size(75, 13);
            this.label_captcha.TabIndex = 6;
            this.label_captcha.Text = "Mã xác nhận:";
            // 
            // textBox_captcha
            // 
            this.textBox_captcha.BackColor = System.Drawing.Color.White;
            this.textBox_captcha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_captcha.ForeColor = System.Drawing.Color.Black;
            this.textBox_captcha.Location = new System.Drawing.Point(84, 94);
            this.textBox_captcha.Name = "textBox_captcha";
            this.textBox_captcha.Size = new System.Drawing.Size(127, 22);
            this.textBox_captcha.TabIndex = 7;
            this.textBox_captcha.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pictureBox_captcha
            // 
            this.pictureBox_captcha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_captcha.ImageLocation = "";
            this.pictureBox_captcha.Location = new System.Drawing.Point(217, 94);
            this.pictureBox_captcha.Name = "pictureBox_captcha";
            this.pictureBox_captcha.Size = new System.Drawing.Size(50, 22);
            this.pictureBox_captcha.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_captcha.TabIndex = 8;
            this.pictureBox_captcha.TabStop = false;
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(115, 122);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(45, 23);
            this.button_ok.TabIndex = 9;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(274, 151);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.pictureBox_captcha);
            this.Controls.Add(this.textBox_captcha);
            this.Controls.Add(this.textBox_server);
            this.Controls.Add(this.label_captcha);
            this.Controls.Add(this.label_server);
            this.Controls.Add(this.textBox_password);
            this.Controls.Add(this.label_password);
            this.Controls.Add(this.textBox_username);
            this.Controls.Add(this.label_username);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập";
            this.Load += new System.EventHandler(this.Login_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Login_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_captcha)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
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
    }
}