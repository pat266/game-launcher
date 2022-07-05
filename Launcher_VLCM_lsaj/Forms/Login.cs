using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using System.Runtime.InteropServices;

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

            

            // generate 100 captcha images to test
            // dedicate another thread to solve captcha
            // var thread4 = new Thread(() => generate_captcha(100));
            // thread4.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            // thread4.Start(); // starts thread
            // thread4.Join(); // wait for thread to finish
            // generate_captcha(100);
            // generate_captcha(1);
            //generate_captcha(2);

            //**/
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
                max_server = get_max_server();
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
                textBox_username.Text, md5_encrypt(textBox_password.Text), textBox_captcha.Text));

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
            Program.flash_movie = find_string(game_data, "(?<=swfobject\\.embedSWF\\(\").*?(?=\".*?\\))");
            // find the parameters (variables) to load to flash
            Program.flash_vars = find_string(game_data, "(?<=parameters\\s*?=\\s*?{)[^\\0]*?(?=};)");
            Program.flash_vars = parse_to_query_string(Program.flash_vars);

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
         * Helper method:
         * Retrieve the captcha image from the server and display it in the form.
         */
        private void load_captcha()
        {
            // get the captcha based on the cookies
            byte[] captcha_data =
                Web_Request.Web_Request.send_request("http://www.niua.com/seccode.php",
                                                    "GET",
                                                    null,
                                                    Program.cookies);
            if (captcha_data == null)
                return;

            // put the captcha in picture box control in the login form
            try
            {
                using (MemoryStream memory_stream = new MemoryStream(captcha_data))
                {
                    pictureBox_captcha.Image = Image.FromStream(memory_stream);
                }
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
            // process the image
            Image filteredImg = captchaSolver.SelectionProcessImage(1, pictureBox_captcha.Image);
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

        /**
         * Helper method:
         * Encrypt the password using MD5 since the server does not accept plain text password.
         */
        private string md5_encrypt(string text)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = Encoding.UTF8.GetBytes(text);
                byte[] result = md5.ComputeHash(data);
                StringBuilder string_builder = new StringBuilder();
                foreach (byte b in result)
                {
                    string_builder.Append(b.ToString("X2"));
                }
                return string_builder.ToString().ToLower();
            }
        }

        /**
         * Helper method:
         * Find the string given a pattern and a string.
         */
        public string find_string(string input, string pattern) // hàm tìm kiếm chuỗi và trả về kết quả
        {
            Match match = Regex.Match(input, pattern);
            
            return match.Success ? match.Value : "";
        }

        /**
         * Helper method:
         * Replace all of the white spaces in a string.
         */
        public string remove_white_space(string text)
        {
            return Regex.Replace(text, "\\s", "");
        }

        /**
         * Helper method:
         * Clean the input string to make it ready to be used in the query string.
         */
        public string parse_to_query_string(string text)
        {
            string result = "";
            string[] text_split = Regex.Split(text, "\\n");
            for (int i = 0; i < text_split.Length; i++)
            {
                text_split[i] = remove_white_space(text_split[i]);
                if (text_split[i] == "")
                    continue;
                
                text_split[i] = Regex.Replace(text_split[i], ",$", "");
                string[] text_split_split = Regex.Split(text_split[i], "(?<=^[^:]*?):");
                text_split_split[1] = Regex.Replace(text_split_split[1], "^\"", "");
                text_split_split[1] = Regex.Replace(text_split_split[1], "\"$", "");
                result += string.Format("{0}={1}&", text_split_split[0], text_split_split[1]);
            }
            result = Regex.Replace(result, "&$", "");
            return result;
        }

        /**
         * Helper method:
         * Get the maximum server number
         */
        public int get_max_server()
        {
            // send a get request to the link to get the HTML page
            byte[] html = Web_Request.Web_Request.send_request("http://www.niua.com/server/code/lsaj/",
                                                                "GET",
                                                                null,
                                                                null);
            if (html == null)
                return 0;
            // convert the html to string
            string html_string = Encoding.UTF8.GetString(html);
            // a regex to find the maximum server number
            Regex regex = new Regex("var qServer = {\"([0-9]{0,9})\":\\[");
            // temp value to hold the extracted values (3 parts)
            var temp = regex.Match(html_string);
            // retrieve the maximum server number from the second part of the temp value
            string server = temp.Groups[1].ToString();
            return int.Parse(server);
        }

        /**
         * Helper method:
         * Retrieve the captcha image from the server (without cookies).
         */
        private void generate_captcha(int number)
        {
            for (int i = 0; i < number; i++)
            {
                // get the captcha based on the cookies
                byte[] captcha_data =
                Web_Request.Web_Request.send_request("http://www.niua.com/seccode.php",
                                                    "GET",
                                                    null,
                                                    null);
                if (captcha_data == null)
                    continue;

                Image retImg;
                // put the captcha in picture box control in the login form
                try
                {
                    using (MemoryStream memory_stream = new MemoryStream(captcha_data))
                    {
                        retImg = Image.FromStream(memory_stream);
                                               
                        // resize the image by 300%
                        retImg = ResizeImage(retImg, retImg.Width * 2, retImg.Height * 2);

                        // convert the image to black and white
                        // SetPixelColor((Bitmap) retImg);
                        // retImg = ToGrayBitmap((Bitmap) retImg);
                        // retImg = MainHub.processImage((Bitmap) retImg);

                        
                        // get the result of the captcha as name
                        string name = captchaSolver.solveCaptcha((Bitmap) retImg);
                        Console.WriteLine(name);
                        
                        // save the captcha to a file
                        string imagesDirectory = Path.Combine(@"C:\Users\nili266\Desktop\GitHub Repo\Launcher_VLCM_lsaj\Captcha", "sampleImages", name);
                        imagesDirectory += ".png";
                        Console.WriteLine(imagesDirectory);

                        // save the image to a file
                        retImg.Save(imagesDirectory);
                        /****/
                    }

                }
                catch (Exception exception)
                {
                    MessageBox.Show(string.Format("There is an error getting captcha in generate_captcha()!\n{0}", exception.Message),
                                    "Captcha Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    continue;
                }
            }
            
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        private Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        

    }
}