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

            // initialize platform combobox
            comboBox_platform.DataSource = Enum.GetValues(typeof(Platform));

            // set default platform
            comboBox_platform.SelectedItem = Platform.Game2cn;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // load_captcha();
        }

        private void load_captcha()
        {
            byte[] captcha_data;

            // Get the selected Platform from the ComboBox
            Platform selectedPlatform = (Platform)comboBox_platform.SelectedItem;

            // Use a switch statement to handle each Platform
            switch (selectedPlatform)
            {
                case Platform.Niua:
                    captcha_data = Web_Request.Web_Request.send_request(
                        "http://www.niua.com/seccode.php",
                        "GET",
                        null,
                        Program.cookies);
                    break;

                case Platform.Game2cn:
                    captcha_data = Web_Request.Web_Request.send_request(
                        "https://www.game2.cn/verifyCode.php",
                        "GET",
                        null,
                        Program.cookies);
                    break;

                default:
                    // Optionally, handle any other cases
                    throw new Exception("Unknown platform selected");
            }

            if (captcha_data == null)
                return;

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
                MessageBox.Show(string.Format("There is an error when loading captcha!\n{0}", exception.Message), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login_Niua()
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
                MessageBox.Show("Login information is incorrect!", "Error", MessageBoxButtons.OK,
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
                MessageBox.Show("Cannot get the embedded SWF Object!", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            Close(); // đóng form đăng nhập
        }

        private void Login_Game2cn()
        {
            Program.flash_movie = "";
            Program.flash_vars = "";

            // tạo dữ liệu đăng nhập
            byte[] login_data = Encoding.UTF8.GetBytes(string.Format("code={0}&password={1}&vcode={2}&usercode={0}",
                    textBox_username.Text, LoginUtil.md5_encrypt(textBox_password.Text), textBox_captcha.Text));

            // gửi yêu cầu đăng nhập và lấy dữ liệu phản hồi
            byte[] response_data_for_login =
                Web_Request.Web_Request.send_request("https://www.game2.cn/websiteAjax/op/login/", "POST", login_data,
                    Program.cookies);
            if (response_data_for_login == null)
            {
                return;
            }

            // kiểm tra yêu cầu đăng nhập có thành công hay không
            if (!Encoding.UTF8.GetString(response_data_for_login).Contains("1"))
            {
                MessageBox.Show("Login information is incorrect!", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                load_captcha(); // load lại mã xác nhận
                return;
            }

            // gửi yêu cầu vào game và lấy dữ liệu phản hồi
            byte[] response_data_for_game = Web_Request.Web_Request.send_request(
                string.Format("http://www.game2.cn/playGame/code/lsaj{0}/", textBox_server.Text),
                    "GET",
                    null,
                    Program.cookies);
            if (response_data_for_game == null)
            {
                return;
            }

            // get the data to load game
            string game_data = Encoding.UTF8.GetString(response_data_for_game);
            // find the SWF object to load to flash
            Program.flash_movie = LoginUtil.find_string(game_data, "(?<=swfobject\\.embedSWF\\(\").*?(?=\".*?\\))");
            // find the parameters (variables) to load to flash
            Program.flash_vars = LoginUtil.find_string(game_data, "(?<=parameters\\s*?=\\s*?{)[^\\0]*?(?=};)");
            Program.flash_vars = LoginUtil.parse_to_query_string(Program.flash_vars);

            // kiểm tra dữ liệu cần để load game có lấy được hay không
            if (Program.flash_movie == "" || Program.flash_vars == "")
            {
                MessageBox.Show("Cannot get the embedded SWF Object!", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            Close(); // đóng form đăng nhập
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

            // Get the selected Platform from the ComboBox
            Platform selectedPlatform = (Platform)comboBox_platform.SelectedItem;

            // Use a switch statement to handle each Platform
            switch (selectedPlatform)
            {
                case Platform.Niua:
                    Login_Niua();
                    break;

                case Platform.Game2cn:
                    Login_Game2cn();
                    break;

                default:
                    // Optionally, handle any other cases
                    throw new Exception("Unknown platform selected");
            }

        }

        private void comboBox_platform_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_captcha(); // reload captcha
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
                MessageBox.Show("Server is invalid! Server has to be a number.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
            if (server <= 0)
            {
                MessageBox.Show("Server is invalid! Server has to be a positive number.", "Error", MessageBoxButtons.OK,
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

    }

    enum Platform
    {
        Niua,
        Game2cn
    }
}