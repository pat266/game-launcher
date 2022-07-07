using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Launcher_VLCM_niua_lsaj.Forms
{
    public partial class Game : Form
    {
        [DllImport("winmm.dll")]
        private static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

        [DllImport("winmm.dll")]
        private static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

        private bool isMute = false;

        public Game()
        {
            InitializeComponent();

            // mute the game
            Mute_Game();

            // set basic buttons
            Set_Control_Buttons();

        }

        private void Game_Load(object sender, EventArgs e)
        {
            // set the necessary information to flash control of form game
            axShockwaveFlash.Movie = string.Format("{0}?{1}", Program.flash_movie, Program.flash_vars);

            Adjust_Gameform();

            Adjust_FormBorder();    
        }

        private void Adjust_Gameform()
        {
            // set it so that the application is resizable
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.SizeGripStyle = SizeGripStyle.Auto;

            // set the intial size
            this.Size = new Size(1024, 768);


            // center the application
            this.CenterToScreen();

            // this.FormBorderStyle = FormBorderStyle.None;

            // this.WindowState = FormWindowState.Maximized;
        }

        private void Adjust_FormBorder()
        {
            // fixed the form border to only be expandable horizontally
            FormBorder.MinimumSize = new Size(0, 30);
            FormBorder.MaximumSize = new Size(Int32.MaxValue, 30);
        }

        private void Mute_Game(){
            // mute the game
            SetVolume(0);
            // change the icon
            ApplicationSound.Image = Properties.Resources.mute;
            // set the boolean value
            isMute = true;
        }

        private void Unmute_Game()
        {
            // unmute the game
            SetVolume(10);
            // change the icon
            ApplicationSound.Image = Properties.Resources.unmute;
            // set the boolean value
            isMute = false;
        }

        /**
         * Set the image and the size of the three buttons:
         * exit, maximize, minimize
         */
        private void Set_Control_Buttons()
        {
            // exit
            exit.Image = Properties.Resources.close;
            exit.MinimumSize = new Size(30, 30);
            exit.MaximumSize = new Size(30, 30);

            // maximize
            maximize.Image = Properties.Resources.maximize;
            maximize.MinimumSize = new Size(30, 30);
            maximize.MaximumSize = new Size(30, 30);

            // minimize
            minimize.Image = Properties.Resources.minimize;
            minimize.MinimumSize = new Size(30, 30);
            minimize.MaximumSize = new Size(30, 30);
        }

        /// <summary>
        /// Returns volume from 0 to 10
        /// </summary>
        /// <returns>Volume from 0 to 10</returns>
        public int GetVolume()
        {
            uint CurrVol = 0;
            waveOutGetVolume(IntPtr.Zero, out CurrVol);
            ushort CalcVol = (ushort)(CurrVol & 0x0000ffff);
            int volume = CalcVol / (ushort.MaxValue / 10);
            return volume;
        }

        /// <summary>
        /// Sets volume from 0 to 10
        /// </summary>
        /// <param name="volume">Volume from 0 to 10</param>
        public void SetVolume(int volume)
        {
            int NewVolume = ((ushort.MaxValue / 10) * volume);
            uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
            waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);
        }

        private void ApplicationSound_Click(object sender, EventArgs e)
        {
            if (isMute)
                Unmute_Game();
            else
                Mute_Game();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void maximize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}