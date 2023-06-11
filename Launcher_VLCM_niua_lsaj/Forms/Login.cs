using Captcha;
using Launcher_VLCM_niua_lsaj.Utils;
using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Launcher_VLCM_niua_lsaj.Forms
{
    public partial class Login : Form
    {
        CaptchaSolver captchaSolver;
        public Login(CaptchaSolver captchaSolver)
        {
            InitializeComponent();
            this.Icon = (Icon)Properties.Resources.ResourceManager.GetObject("app_icon");
            this.captchaSolver = captchaSolver;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            load_captcha(); // load mã xác nhận
            
        }

        private void load_captcha()
        {
            // lấy dữ liệu mã xác nhận từ web
            byte[] captcha_data =
                Web_Request.Web_Request.send_request("http://www.niua.com/seccode.php", "GET", null, Program.cookies);
            if (captcha_data == null)
            {
                return;
            }
            // đưa dữ liệu mã xác nhận vào picture box control trên form đăng nhập
            try
            {
                textBox_captcha.Text = this.captchaSolver.SolveCaptcha(captcha_data);

                // convert byte array to image
                using (MemoryStream memory_stream = new MemoryStream(captcha_data))
                {
                    pictureBox_captcha.Image = Image.FromStream(memory_stream);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(string.Format("Có lỗi xảy ra khi lấy mã xác nhận!\n{0}", exception.Message), "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    button_ok_Click(sender, e);
                    return;
            }
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            // kiểm tra thông tin nhập vào có hợp lệ hay không
            if (!check_input_data())
            {
                return;
            }

            login(); // đăng nhập
        }

        private bool check_input_data()
        {
            if (textBox_username.Text == "")
            {
                textBox_username.Focus();
                return false;
            }

            if (textBox_password.Text == "")
            {
                textBox_password.Focus();
                return false;
            }
            if (textBox_server.Text == "")
            {
                textBox_server.Focus();
                return false;
            }
            int server;
            if (!int.TryParse(textBox_server.Text, out server))
            {
                MessageBox.Show("Server không hợp lệ! Server phải là một số.", "Lỗi", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
            if (server <= 0)
            {
                MessageBox.Show("Server không hợp lệ! Server phải là một số dương.", "Lỗi", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
            if (textBox_captcha.Text == "")
            {
                textBox_captcha.Focus();
                return false;
            }
            return true;
        }

        private void login()
        {
            Program.flash_movie = "";
            Program.flash_vars = "";

            // tạo dữ liệu đăng nhập
            byte[] login_data = Encoding.UTF8.GetBytes(string.Format("op=login&email={0}&password={1}&seccode={2}",
                textBox_username.Text, LoginUtil.md5_encrypt(textBox_password.Text), textBox_captcha.Text));
            
            // gửi yêu cầu đăng nhập và lấy dữ liệu phản hồi
            byte[] response_data_for_login =
                Web_Request.Web_Request.send_request("http://www.niua.com/loginWin.php?g=lsaj", "POST", login_data,
                    Program.cookies);
            if (response_data_for_login == null)
            {
                return;
            }

            // kiểm tra yêu cầu đăng nhập có thành công hay không
            if (!Encoding.UTF8.GetString(response_data_for_login).Contains("欢迎您登录！"))
            {
                MessageBox.Show("Thông tin đăng nhập hoặc mã xác nhận không chính xác!", "Lỗi", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                load_captcha(); // load lại mã xác nhận
                return;
            }
            
            // gửi yêu cầu vào game và lấy dữ liệu phản hồi
            byte[] response_data_for_game = Web_Request.Web_Request.send_request(
                string.Format("http://www.niua.com/playGame/code/lsaj{0}/", textBox_server.Text), "GET", null,
                Program.cookies);
            if (response_data_for_game == null)
            {
                return;
            }

            // lấy dữ liệu cần để load game
            string game_data = Encoding.UTF8.GetString(response_data_for_game);
            Program.flash_movie = LoginUtil.find_string(game_data, "(?<=swfobject\\.embedSWF\\(\").*?(?=\".*?\\))");
            Program.flash_vars = LoginUtil.find_string(game_data, "(?<=parameters\\s*?=\\s*?{)[^\\0]*?(?=};)");
            Program.flash_vars = LoginUtil.parse_to_query_string(Program.flash_vars);

            // kiểm tra dữ liệu cần để load game có lấy được hay không
            if (Program.flash_movie == "" || Program.flash_vars == "")
            {
                MessageBox.Show("Không lấy được dữ liệu cần để load game!", "Lỗi", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            Close(); // đóng form đăng nhập
        }

    }
}