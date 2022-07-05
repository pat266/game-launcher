using System;
using System.Drawing;
using System.Media;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Threading;

using Captcha;

namespace Launcher_VLCM_niua_lsaj.Forms
{
    public partial class Login : Form
    {
        int max_server;

        CaptchaSolver captchaSolver;
        
        public Login()
        {
            // load up the captcha solver
            captchaSolver = new CaptchaSolver();

            ///**
            // load up the login window
            InitializeComponent();

            // add load server to be loaded before the game window is loaded
            // this.Load += new EventHandler(this.Load_Server);
            Console.WriteLine("Start a separate thread to retrieve the server list");
            // dedicate a separate thread to load the server
            var thread1 = new Thread(new ThreadStart(Load_Server));
            thread1.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            thread1.Start(); // starts thread
            thread1.Join(); // wait for thread to finish

            // dedicate another thread to load captcha
            var thread2 = new Thread(new ThreadStart(load_captcha));
            thread2.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            thread2.Start(); // starts thread
            thread2.Join(); // wait for thread to finish

            // dedicate another thread to solve captcha
            var thread3 = new Thread(new ThreadStart(solve_captcha));
            thread3.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            thread3.Start(); // starts thread
            // thread3.Join(); // wait for thread to finish

        }

        /**
         * Helper method: 
         * Load up the values of server for the ComboBox in Login form.
         */
        private void Load_Server()
        {
            if (max_server == 0)
            {
                // retrieve the max number of server
                max_server = Login_Helper.get_max_server();
                Console.WriteLine("The current max server is: " + max_server);

                // load the available server in the ComboBox
                combo_server.DataSource = Enumerable.Range(1, max_server).Reverse().ToList();

                combo_server.DisplayMember = "Server";
                combo_server.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                combo_server.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
        }

        /**
         * Main method:
         * Utilize the input data and login to the server
         */
        private void login()
        {
            Program.flash_movie = "";
            Program.flash_vars = "";

            // create the appropriate data format to login
            byte[] login_data = Encoding.UTF8.GetBytes(string.Format("op=login&email={0}&password={1}&seccode={2}",
                textBox_username.Text, Login_Helper.md5_encrypt(textBox_password.Text), textBox_captcha.Text));

            // send login request
            byte[] response_data_for_login =
                Web_Request.Web_Request.send_request("http://www.niua.com/loginWin.php?g=lsaj",
                                                    "POST",
                                                    login_data,
                                                    Program.cookies);
            if (response_data_for_login == null)
                return;
            // Console.WriteLine(Encoding.UTF8.GetString(response_data_for_login));

            // check if login is successful
            if (!Encoding.UTF8.GetString(response_data_for_login).Contains("欢迎您登录！"))
            {
                MessageBox.Show("Login information or captcha is incorrect!",
                                "Login Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                // reload captcha
                load_captcha();
                // solve it again
                solve_captcha();
                return;
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
         * Retrieve the captcha image from the server and display it in the form.
         */
        private void load_captcha()
        {
            // put the captcha in picture box control in the login form
            try
            {
                // set the captcha image
                pictureBox_captcha.Image = Login_Helper.get_random_captcha(Program.cookies);
                // solve captcha automatically after image is loaded
                solve_captcha();
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
        private void solve_captcha()
        {
            // process the image using Sauvola method
            Image filteredImg = captchaSolver.SelectionProcessImage(0, pictureBox_captcha.Image);
            // get the result
            textBox_captcha.Text = captchaSolver.solveCaptcha((Bitmap) filteredImg);
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
        private void button_ok_Click(object sender, EventArgs e)
        {
            // basic check of login info
            if (!check_input_data())
                return;
            
            login();
        }

        /**
         * Helper method:
         * Perform basic check on the input data in the login window
         * TODO: https://www.codeproject.com/Questions/5061322/Set-focus-on-textbox-combobox-in-winforms-if-it-is
         */
        private bool check_input_data()
        {
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
                return false;
            }
            if (server <= 0)
            {
                MessageBox.Show("Server is invalid! Server has to be a positive number.",
                                "Server Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
            if (server > max_server)
            {
                MessageBox.Show("Server is invalid! Server has to be less than or equal to " + max_server + ".",
                                "Server Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
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

        
        

    }
}