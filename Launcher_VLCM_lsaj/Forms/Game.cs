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
        
        public Game()
        {
            // mute the site?!
            Game.SetVolume(0);

            

            InitializeComponent();
            
        }

        private void Game_Load(object sender, EventArgs e)
        {
            // set the necessary information to flash control of form game
            axShockwaveFlash.Movie = string.Format("{0}?{1}", Program.flash_movie, Program.flash_vars);

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

        /// <summary>
        /// Returns volume from 0 to 10
        /// </summary>
        /// <returns>Volume from 0 to 10</returns>
        public static int GetVolume()
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
        public static void SetVolume(int volume)
        {
            int NewVolume = ((ushort.MaxValue / 10) * volume);
            uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
            waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);
        }

    }
}