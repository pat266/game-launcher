using System;
using System.Drawing;
using System.Media;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Threading;

using Captcha;
using DotNetEnv;
using System.Threading.Tasks;

namespace Launcher_VLCM_niua_lsaj.Forms
{
    public partial class Login : Form
    {
        int max_server = Int32.MaxValue;

        CaptchaSolver captchaSolver;

        private static Bitmap revealImg = new Bitmap(Properties.Resources.reveal);
        private static Bitmap hideImg = new Bitmap(Properties.Resources.hide);

        public static string success_login = "欢迎您登录！"; // success login

        public Login()
        {
            // track form loading time
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            
            // load up the captcha solver
            captchaSolver = new CaptchaSolver();

            // /**
            // load up the login window
            InitializeComponent();
            // prettify reveal password button
            Prettify_Reveal_Button();

            Console.WriteLine("Start a separate thread to retrieve the server list");
            // dedicate a separate thread to load the server
            _ = Load_Server_Async();

            // dedicate another thread to load captcha
            _ = Load_Captcha_Async();

            // dedicate another thread to load login credentials
            var thread3 = new Thread(new ThreadStart(Load_Credentials));
            thread3.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            thread3.Start(); // starts thread
            // **/

            watch.Stop();

            Console.WriteLine($"Loading Time: {watch.ElapsedMilliseconds / 1000.0} s");
        }

        /**
         * Helper method: 
         * Make the reveal password button prettier.
         */
        private void Prettify_Reveal_Button()
        {
            // set the reveal button to transparent
            reveal.TabStop = false;
            reveal.FlatStyle = FlatStyle.Flat;
            reveal.FlatAppearance.BorderSize = 0;
            reveal.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255); //transparent
        }

        /**
         * Helper method: 
         * Load up the credentials from the .env file.
         */
        private void Load_Credentials()
        {
            // load the login values from the .env file
            Env.Load("../../.env");

            textBox_username.Text = Environment.GetEnvironmentVariable("USERNAME");
            textBox_password.Text = Environment.GetEnvironmentVariable("PASSWORD");
        }

        /**
         * Helper method: 
         * Load up the values of server for the ComboBox in Login form.
         */
        private async Task Load_Server_Async()
        {
            if (max_server == Int32.MaxValue)
            {
                // retrieve the max number of server
                max_server = await Task.Run(() => Login_Helper.get_max_server());

                if (combo_server.Text == "")
                    combo_server.Text = max_server.ToString();
            }
        }

        /**
         * Main method:
         * Retrieve the captcha image from the server and display it in the form.
         */
        private void Load_Captcha()
        {
            // put the captcha in picture box control in the login form
            try
            {
                // set the captcha image
                Image captcha = Login_Helper.get_random_captcha(Program.cookies);
                pictureBox_captcha.Image = captcha;
                // solve captcha automatically after image is loaded
                Solve_Captcha();
            }
            catch (Exception exception)
            {
                MessageBox.Show(string.Format("There is an error getting captcha~!\n{0}", exception.Message),
                                "Captcha Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        /**
         * Main method:
         * Retrieve the captcha image from the server and display it in the form.
         */
        private async Task Load_Captcha_Async()
        {
            // put the captcha in picture box control in the login form
            try
            {
                // set the captcha image
                Image captcha = await Task.Run(() => Login_Helper.get_random_captcha(Program.cookies));
                pictureBox_captcha.Image = captcha;
                // solve captcha automatically after image is loaded
                Solve_Captcha();
            }
            catch (Exception exception)
            {
                MessageBox.Show(string.Format("There is an error getting captcha~!\n{0}", exception.Message),
                                "Captcha Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        /**
         * Helper method:
         * Solve the captcha and put the result in the text box.
         */
        private void Solve_Captcha()
        {
            // Apply simple gray bitmap filter
            pictureBox_captcha.Image = Util.ToGrayBitmap((Bitmap)pictureBox_captcha.Image);

            // get the result
            textBox_captcha.Text = captchaSolver.solveCaptcha((Bitmap) pictureBox_captcha.Image);

        }

        /**
         * Main method:
         * KeyEventHandler of the login window
         */
        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            // treat hitting enter the same as clicking ok button
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    button_ok_Click(sender, e);
                    return;
            }
        }

        /**
         * Main method:
         * EventHandler of the the login window
         */
        private async void button_ok_Click(object sender, EventArgs e)
        {
            // basic check of login info
            if (!check_input_data())
                return;
            
            await Login_Async();
        }
        
        /**
         * Helper method:
         * Perform basic check on the input data in the login window
         */
        private bool check_input_data()
        {
            // if the application has not finished loading
            if (textBox_captcha.Text == "" && pictureBox_captcha.Image == null)
            {
                MessageBox.Show("Please wait until the application finishes loading.",
                                "Loading Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
            if (textBox_username.Text == "")
            {
                SystemSounds.Beep.Play();
                textBox_username.Focus();
                return false;
            }

            if (textBox_password.Text == "")
            {
                SystemSounds.Beep.Play();
                textBox_password.Focus();
                return false;
            }
            if (combo_server.Text == "")
            {
                SystemSounds.Beep.Play();
                combo_server.Focus();
                return false;
            }
            int server;
            if (!int.TryParse(combo_server.Text, out server))
            {
                MessageBox.Show("Server is invalid! Server has to be a number.",
                                "Server Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                combo_server.Focus();
                return false;
            }
            if (server <= 0)
            {
                MessageBox.Show("Server is invalid! Server has to be a positive number.",
                                "Server Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                combo_server.Focus();
                return false;
            }
            if (server > max_server)
            {
                MessageBox.Show("Server is invalid! Server has to be less than or equal to " + max_server + ".",
                                "Server Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                combo_server.Focus();
                return false;
            }
            if (textBox_captcha.Text == "")
            {
                SystemSounds.Beep.Play();
                textBox_captcha.Focus();
                return false;
            }
            return true;
        }

        /**
         * Main method:
         * Utilize the input data and login to the server
         */
        private void login()
        {
            // attempt to login 5 times
            for (int i = 0; i < 5; i++)
            {
                Program.flash_movie = "";
                Program.flash_vars = "";

                // create the appropriate data format to login
                byte[] login_data = Encoding.UTF8.GetBytes(string.Format("op=login&email={0}&password={1}&seccode={2}",
                    textBox_username.Text, Login_Helper.md5_encrypt(textBox_password.Text), textBox_captcha.Text));
                Console.WriteLine("Login data: " + Encoding.UTF8.GetString(login_data));

                // send login request
                byte[] response_data_for_login =
                    Web_Request.Web_Request.send_request("http://www.niua.com/loginWin.php?g=lsaj",
                                                        "POST",
                                                        login_data,
                                                        Program.cookies);
                if (response_data_for_login == null)
                    return;
                
                // check if login is successful
                if (!Encoding.UTF8.GetString(response_data_for_login).Contains(success_login))
                {
                    // if it still fails after 5 times, show error message and return
                    if (i >= 4)
                    {
                        MessageBox.Show("Login information or captcha is incorrect!",
                                    "Login Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                        // reload captcha
                        Load_Captcha();
                        return;
                    }
                    else
                    {
                        // if it fails, reload captcha and solve it again
                        Load_Captcha();
                    }
                    
                }
                else // if the login is successful
                {
                    // break out of the login attempt
                    break;
                }
            }
            
            
            // send request to access the game
            byte[] response_data_for_game =
                Web_Request.Web_Request.send_request(string.Format("http://www.niua.com/playGame/code/lsaj{0}/",
                                                                    combo_server.Text),
                                                     "GET",
                                                     null,
                                                     Program.cookies);

            if (response_data_for_game == null)
                return;

            // get the data to load game
            string game_data = Encoding.UTF8.GetString(response_data_for_game);
            // find the SWF object to load to flash
            Program.flash_movie = Login_Helper.find_string(game_data, "(?<=swfobject\\.embedSWF\\(\").*?(?=\".*?\\))");
            // find the parameters (variables) to load to flash
            Program.flash_vars = Login_Helper.find_string(game_data, "(?<=parameters\\s*?=\\s*?{)[^\\0]*?(?=};)");
            Program.flash_vars = Login_Helper.parse_to_query_string(Program.flash_vars);

            // basic check to see if we can load the game
            if (Program.flash_movie == "" || Program.flash_vars == "")
            {
                MessageBox.Show("Cannot get data to load game!",
                                "Flash Info Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            // close the login window
            Close();
        }

        /**
         * Main method:
         * Utilize the input data and login to the server
         */
        private async Task Login_Async()
        {
            // attempt to login 5 times
            for (int i = 0; i < 5; i++)
            {
                Program.flash_movie = "";
                Program.flash_vars = "";

                // create the appropriate data format to login
                byte[] login_data = Encoding.UTF8.GetBytes(string.Format("op=login&email={0}&password={1}&seccode={2}",
                    textBox_username.Text, Login_Helper.md5_encrypt(textBox_password.Text), textBox_captcha.Text));
                // Console.WriteLine("Login data: " + Encoding.UTF8.GetString(login_data));

                // send login request
                byte[] response_data_for_login = await Task.Run(() =>
                    Web_Request.Web_Request.send_request("http://www.niua.com/loginWin.php?g=lsaj",
                                                        "POST",
                                                        login_data,
                                                        Program.cookies));
                if (response_data_for_login == null)
                    return;

                // check if login is successful
                if (!Encoding.UTF8.GetString(response_data_for_login).Contains(success_login))
                {
                    // if it still fails after 5 times, show error message and return
                    if (i >= 4)
                    {
                        MessageBox.Show("Login information or captcha is incorrect!",
                                    "Login Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                        // reload captcha
                        Load_Captcha();
                        return;
                    }
                    else
                    {
                        // if it fails, reload captcha and solve it again
                        Load_Captcha();
                    }

                }
                else // if the login is successful
                {
                    // break out of the login attempt
                    break;
                }
            }


            // send request to access the game
            byte[] response_data_for_game = await Task.Run(() =>
                Web_Request.Web_Request.send_request(string.Format("http://www.niua.com/playGame/code/lsaj{0}/",
                                                                    combo_server.Text),
                                                     "GET",
                                                     null,
                                                     Program.cookies));

            if (response_data_for_game == null)
                return;

            // get the data to load game
            string game_data = Encoding.UTF8.GetString(response_data_for_game);
            // find the SWF object to load to flash
            Program.flash_movie = Login_Helper.find_string(game_data, "(?<=swfobject\\.embedSWF\\(\").*?(?=\".*?\\))");
            // find the parameters (variables) to load to flash
            Program.flash_vars = Login_Helper.find_string(game_data, "(?<=parameters\\s*?=\\s*?{)[^\\0]*?(?=};)");
            Program.flash_vars = Login_Helper.parse_to_query_string(Program.flash_vars);

            // basic check to see if we can load the game
            if (Program.flash_movie == "" || Program.flash_vars == "")
            {
                MessageBox.Show("Cannot get data to load game!",
                                "Flash Info Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            // close the login window
            Close();
        }

        private void pictureBox_captcha_Click(object sender, EventArgs e)
        {
            // the load captcha method will reload the captcha (already call the solve method)
            Load_Captcha();
        }

        /**
         * Reveal password or cover it up
         */
        private void reveal_Click(object sender, EventArgs e)
        {
            if (textBox_password.PasswordChar)
            {
                textBox_password.PasswordChar = false;
                reveal.Image = Login.hideImg;
            }

            else
            {
                textBox_password.PasswordChar = true;
                reveal.Image = Login.revealImg;
            }
                
        }
    }
}