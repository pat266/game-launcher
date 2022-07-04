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
            this.label_captcha = new System.Windows.Forms.Label();
            this.textBox_captcha = new System.Windows.Forms.TextBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.combo_server = new System.Windows.Forms.ComboBox();
            this.pictureBox_captcha = new System.Windows.Forms.PictureBox();
            this.cTextBox1 = new Launcher_VLCM_niua_lsaj.CustomControls.CTextBox();
            this.cComboBox1 = new Launcher_VLCM_niua_lsaj.CustomControls.CComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_captcha)).BeginInit();
            this.SuspendLayout();
            // 
            // label_username
            // 
            this.label_username.AutoSize = true;
            this.label_username.Location = new System.Drawing.Point(3, 9);
            this.label_username.Name = "label_username";
            this.label_username.Size = new System.Drawing.Size(61, 13);
            this.label_username.TabIndex = 0;
            this.label_username.Text = "Username:";
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
            this.label_password.Text = "Password:";
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
            // label_captcha
            // 
            this.label_captcha.AutoSize = true;
            this.label_captcha.Location = new System.Drawing.Point(3, 96);
            this.label_captcha.Name = "label_captcha";
            this.label_captcha.Size = new System.Drawing.Size(52, 13);
            this.label_captcha.TabIndex = 6;
            this.label_captcha.Text = "Captcha:";
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
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(115, 122);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(72, 23);
            this.button_ok.TabIndex = 9;
            this.button_ok.Text = "Submit";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // combo_server
            // 
            this.combo_server.ForeColor = System.Drawing.SystemColors.WindowText;
            this.combo_server.FormattingEnabled = true;
            this.combo_server.Location = new System.Drawing.Point(84, 65);
            this.combo_server.Name = "combo_server";
            this.combo_server.Size = new System.Drawing.Size(183, 21);
            this.combo_server.TabIndex = 10;
            this.combo_server.Click += new System.EventHandler(this.Load_Server);
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
            // cTextBox1
            // 
            this.cTextBox1.BackColor = System.Drawing.SystemColors.Window;
            this.cTextBox1.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.cTextBox1.BorderFocusColor = System.Drawing.Color.HotPink;
            this.cTextBox1.BorderRadius = 0;
            this.cTextBox1.BorderSize = 2;
            this.cTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cTextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cTextBox1.Location = new System.Drawing.Point(67, 164);
            this.cTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.cTextBox1.Multiline = false;
            this.cTextBox1.Name = "cTextBox1";
            this.cTextBox1.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.cTextBox1.PasswordChar = false;
            this.cTextBox1.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.cTextBox1.PlaceholderText = "Enter Your name here";
            this.cTextBox1.Size = new System.Drawing.Size(250, 31);
            this.cTextBox1.TabIndex = 12;
            this.cTextBox1.Texts = "";
            this.cTextBox1.UnderlinedStyle = false;
            // 
            // cComboBox1
            // 
            this.cComboBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cComboBox1.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.cComboBox1.BorderSize = 1;
            this.cComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cComboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cComboBox1.ForeColor = System.Drawing.Color.DimGray;
            this.cComboBox1.IconColor = System.Drawing.Color.MediumSlateBlue;
            this.cComboBox1.ListBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.cComboBox1.ListTextColor = System.Drawing.Color.DimGray;
            this.cComboBox1.Location = new System.Drawing.Point(84, 216);
            this.cComboBox1.MinimumSize = new System.Drawing.Size(200, 30);
            this.cComboBox1.Name = "cComboBox1";
            this.cComboBox1.Padding = new System.Windows.Forms.Padding(1);
            this.cComboBox1.Size = new System.Drawing.Size(200, 30);
            this.cComboBox1.TabIndex = 13;
            this.cComboBox1.Texts = "";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(416, 258);
            this.Controls.Add(this.cComboBox1);
            this.Controls.Add(this.cTextBox1);
            this.Controls.Add(this.combo_server);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.pictureBox_captcha);
            this.Controls.Add(this.textBox_captcha);
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
            this.Text = "Login - LSAJ";
            this.Load += new System.EventHandler(this.Load_Initial_Captcha);
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
        private System.Windows.Forms.Label label_captcha;
        private System.Windows.Forms.TextBox textBox_captcha;
        private System.Windows.Forms.PictureBox pictureBox_captcha;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.ComboBox combo_server;
        private CustomControls.CTextBox cTextBox1;
        private CustomControls.CComboBox cComboBox1;
    }
}