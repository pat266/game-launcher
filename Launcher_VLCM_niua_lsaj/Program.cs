using System;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Captcha;
using Launcher_VLCM_niua_lsaj.Forms;

namespace Launcher_VLCM_niua_lsaj
{
    static class Program
    {
        private static Login login { get; set; } // form đăng nhập

        private static Game game { get; set; } // form game

        public static CookieContainer cookies { get; set; } // dữ liệu đăng nhập

        public static string flash_movie { get; set; } // dữ liệu cần để load game

        public static string flash_vars { get; set; } // dữ liệu cần để load game

        static CaptchaSolver captchaSolver;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);

            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            string captchaPath = appPath + "models\\captcha\\captcha_model.onnx";
            Program.captchaSolver = new CaptchaSolver(captchaPath);

            login = new Login(Program.captchaSolver);
            game = new Game();
            cookies = new CookieContainer();
            flash_movie = "";
            flash_vars = "";

            login.ShowDialog(); // hiện form đăng nhập để nhập thông tin

            // kiểm tra kết quả trả về (dữ liệu cần để laod game) của form đăng nhập
            if (flash_movie != "" && flash_vars != "")
            {
                Application.Run(game); // hiện form game để load game
            }
        }
    }
}
