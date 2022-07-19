using System;
using System.Net;
using System.Windows.Forms;
using Launcher_VLCM_niua_lsaj.Forms;

using Microsoft.Win32;
using System.Deployment.Application;
using System.IO;

namespace Launcher_VLCM_niua_lsaj
{
    static class Program
    {
        // login window
        private static Login login { get; set; }

        // game window
        private static Game game { get; set; }

        // cookies of the session
        public static CookieContainer cookies { get; set; }

        // needed info to load into the game (set in Login.cs)
        public static string flash_movie { get; set; }
        public static string flash_vars { get; set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            login = new Login();
            game = new Game();
            cookies = new CookieContainer();
            flash_movie = "";
            flash_vars = "";
            // show login form
            login.ShowDialog();

            // check if the flash info is not empty to load game
            if (flash_movie != "" && flash_vars != "")
            {
                // dispaly window and load game
                Application.Run(game);
            }
            
        }
    }
}