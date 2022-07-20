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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.label_username = new System.Windows.Forms.Label();
            this.label_password = new System.Windows.Forms.Label();
            this.label_server = new System.Windows.Forms.Label();
            this.label_captcha = new System.Windows.Forms.Label();
            this.reveal = new System.Windows.Forms.Button();
            this.pictureBox_captcha = new System.Windows.Forms.PictureBox();
            this.button_ok = new Launcher_VLCM_niua_lsaj.CustomControls.CButton();
            this.combo_server = new Launcher_VLCM_niua_lsaj.CustomControls.CTextBox();
            this.textBox_captcha = new Launcher_VLCM_niua_lsaj.CustomControls.CTextBox();
            this.textBox_password = new Launcher_VLCM_niua_lsaj.CustomControls.CTextBox();
            this.textBox_username = new Launcher_VLCM_niua_lsaj.CustomControls.CTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_captcha)).BeginInit();
            this.SuspendLayout();
            // 
            // label_username
            // 
            this.label_username.AutoSize = true;
            this.label_username.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label_username.Location = new System.Drawing.Point(6, 28);
            this.label_username.Name = "label_username";
            this.label_username.Size = new System.Drawing.Size(81, 19);
            this.label_username.TabIndex = 0;
            this.label_username.Text = "Username:";
            // 
            // label_password
            // 
            this.label_password.AutoSize = true;
            this.label_password.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label_password.Location = new System.Drawing.Point(9, 67);
            this.label_password.Name = "label_password";
            this.label_password.Size = new System.Drawing.Size(78, 19);
            this.label_password.TabIndex = 2;
            this.label_password.Text = "Password:";
            // 
            // label_server
            // 
            this.label_server.AutoSize = true;
            this.label_server.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label_server.Location = new System.Drawing.Point(30, 109);
            this.label_server.Name = "label_server";
            this.label_server.Size = new System.Drawing.Size(57, 19);
            this.label_server.TabIndex = 4;
            this.label_server.Text = "Server:";
            // 
            // label_captcha
            // 
            this.label_captcha.AutoSize = true;
            this.label_captcha.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label_captcha.Location = new System.Drawing.Point(19, 154);
            this.label_captcha.Name = "label_captcha";
            this.label_captcha.Size = new System.Drawing.Size(68, 19);
            this.label_captcha.TabIndex = 6;
            this.label_captcha.Text = "Captcha:";
            // 
            // reveal
            // 
            this.reveal.BackColor = System.Drawing.Color.Transparent;
            this.reveal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.reveal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reveal.Image = global::Launcher_VLCM_niua_lsaj.Properties.Resources.reveal;
            this.reveal.Location = new System.Drawing.Point(266, 55);
            this.reveal.Name = "reveal";
            this.reveal.Size = new System.Drawing.Size(30, 25);
            this.reveal.TabIndex = 19;
            this.reveal.UseVisualStyleBackColor = false;
            this.reveal.Click += new System.EventHandler(this.reveal_Click);
            // 
            // pictureBox_captcha
            // 
            this.pictureBox_captcha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_captcha.ImageLocation = "";
            this.pictureBox_captcha.Location = new System.Drawing.Point(232, 145);
            this.pictureBox_captcha.Name = "pictureBox_captcha";
            this.pictureBox_captcha.Size = new System.Drawing.Size(50, 22);
            this.pictureBox_captcha.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_captcha.TabIndex = 8;
            this.pictureBox_captcha.TabStop = false;
            this.pictureBox_captcha.Click += new System.EventHandler(this.pictureBox_captcha_Click);
            // 
            // button_ok
            // 
            this.button_ok.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.button_ok.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            this.button_ok.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.button_ok.BorderRadius = 15;
            this.button_ok.BorderSize = 0;
            this.button_ok.FlatAppearance.BorderSize = 0;
            this.button_ok.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_ok.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.button_ok.ForeColor = System.Drawing.Color.White;
            this.button_ok.Location = new System.Drawing.Point(110, 190);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(108, 41);
            this.button_ok.TabIndex = 18;
            this.button_ok.Text = "Submit";
            this.button_ok.TextColor = System.Drawing.Color.White;
            this.button_ok.UseVisualStyleBackColor = false;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // combo_server
            // 
            this.combo_server.BackColor = System.Drawing.SystemColors.Window;
            this.combo_server.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.combo_server.BorderFocusColor = System.Drawing.Color.HotPink;
            this.combo_server.BorderRadius = 0;
            this.combo_server.BorderSize = 2;
            this.combo_server.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combo_server.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.combo_server.Location = new System.Drawing.Point(99, 94);
            this.combo_server.Margin = new System.Windows.Forms.Padding(4);
            this.combo_server.Multiline = false;
            this.combo_server.Name = "combo_server";
            this.combo_server.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.combo_server.PasswordChar = false;
            this.combo_server.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.combo_server.PlaceholderText = "Enter your Server";
            this.combo_server.Size = new System.Drawing.Size(200, 34);
            this.combo_server.TabIndex = 17;
            this.combo_server.UnderlinedStyle = true;
            // 
            // textBox_captcha
            // 
            this.textBox_captcha.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_captcha.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.textBox_captcha.BorderFocusColor = System.Drawing.Color.HotPink;
            this.textBox_captcha.BorderRadius = 0;
            this.textBox_captcha.BorderSize = 2;
            this.textBox_captcha.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_captcha.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBox_captcha.Location = new System.Drawing.Point(99, 139);
            this.textBox_captcha.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_captcha.Multiline = false;
            this.textBox_captcha.Name = "textBox_captcha";
            this.textBox_captcha.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.textBox_captcha.PasswordChar = false;
            this.textBox_captcha.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.textBox_captcha.PlaceholderText = "";
            this.textBox_captcha.Size = new System.Drawing.Size(105, 34);
            this.textBox_captcha.TabIndex = 16;
            this.textBox_captcha.UnderlinedStyle = true;
            // 
            // textBox_password
            // 
            this.textBox_password.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_password.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.textBox_password.BorderFocusColor = System.Drawing.Color.HotPink;
            this.textBox_password.BorderRadius = 0;
            this.textBox_password.BorderSize = 2;
            this.textBox_password.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_password.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBox_password.Location = new System.Drawing.Point(99, 52);
            this.textBox_password.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_password.Multiline = false;
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.textBox_password.PasswordChar = true;
            this.textBox_password.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.textBox_password.PlaceholderText = "Enter your Password";
            this.textBox_password.Size = new System.Drawing.Size(200, 34);
            this.textBox_password.TabIndex = 14;
            this.textBox_password.UnderlinedStyle = true;
            // 
            // textBox_username
            // 
            this.textBox_username.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_username.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.textBox_username.BorderFocusColor = System.Drawing.Color.HotPink;
            this.textBox_username.BorderRadius = 0;
            this.textBox_username.BorderSize = 2;
            this.textBox_username.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_username.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBox_username.Location = new System.Drawing.Point(99, 13);
            this.textBox_username.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_username.Multiline = false;
            this.textBox_username.Name = "textBox_username";
            this.textBox_username.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.textBox_username.PasswordChar = false;
            this.textBox_username.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.textBox_username.PlaceholderText = "Enter your Username";
            this.textBox_username.Size = new System.Drawing.Size(200, 34);
            this.textBox_username.TabIndex = 13;
            this.textBox_username.UnderlinedStyle = true;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(341, 243);
            this.Controls.Add(this.reveal);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.combo_server);
            this.Controls.Add(this.textBox_captcha);
            this.Controls.Add(this.textBox_password);
            this.Controls.Add(this.textBox_username);
            this.Controls.Add(this.pictureBox_captcha);
            this.Controls.Add(this.label_captcha);
            this.Controls.Add(this.label_server);
            this.Controls.Add(this.label_password);
            this.Controls.Add(this.label_username);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login - lsaj.niua.com";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Login_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_captcha)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_username;
        private System.Windows.Forms.Label label_password;
        private System.Windows.Forms.Label label_server;
        private System.Windows.Forms.Label label_captcha;
        private System.Windows.Forms.PictureBox pictureBox_captcha;
        private CustomControls.CTextBox textBox_username;
        private CustomControls.CTextBox textBox_password;
        private CustomControls.CTextBox textBox_captcha;
        private CustomControls.CTextBox combo_server;
        private CustomControls.CButton button_ok;
        private System.Windows.Forms.Button reveal;
    }
}