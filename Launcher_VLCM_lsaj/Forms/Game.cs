using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace Launcher_VLCM_niua_lsaj.Forms
{
    public partial class Game : Form
    {
        [DllImport("winmm.dll")]
        private static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

        [DllImport("winmm.dll")]
        private static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

        private bool isMute = false;

        // make the winform draggable
        private bool _Moving = false;
        private bool _FullSreen = false;
        private bool _AfterFullSreen = false;
        private Point _Offset;


        /**
         * Constructor
         */
        public Game()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.ResizeRedraw, true);    

            // mute the game
            Mute_Game();

            // set basic buttons
            Set_Control_Buttons();
            
        }

        /**
         * Load the game
         */
        private void Game_Load(object sender, EventArgs e)
        {
            // set the necessary information to flash control of form game
            axShockwaveFlash.Movie = string.Format("{0}?{1}", Program.flash_movie, Program.flash_vars);
            
            Adjust_Gameform();

            Adjust_FormBorder();

            // axShockwaveFlash.s
            
        }

        #region "Basic Visual Changes"
        private void Adjust_Gameform()
        {
            // set the intial size
            this.Size = new Size(1024, 768);

            // center the application
            this.CenterToScreen();

        }

        /**
         * Make the custom FormBorder to only be horizontally extendable.
         */
        private void Adjust_FormBorder()
        {
            // disable title bar 
            this.ControlBox = false;
            this.Text = String.Empty;
            
            // fixed the form border to only be expandable horizontally
            FormBorder.MinimumSize = new Size(0, 30);
            FormBorder.MaximumSize = new Size(Int32.MaxValue, 30);
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
        #endregion

        #region "Application Volume"
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

        /**
         * Set the volume and change its icon appropriately
         */
        private void Mute_Game()
        {
            // mute the game
            SetVolume(0);
            // change the icon
            ApplicationSound.Image = Properties.Resources.mute;
            // set the boolean value
            isMute = true;
        }

        /**
         * Set the volume and change its icon appropriately
         */
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
         * Mute/unmute the application when it is clicked
         */
        private void ApplicationSound_Click(object sender, EventArgs e)
        {
            if (isMute)
                Unmute_Game();
            else
                Mute_Game();
        }
        #endregion

        #region "Standard Buttons"
        /**
         * Close the application when it is clicked
         */
        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /**
         * Maximize the application when the button is clicked
         */
        private void maximize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        /**
         * Minimize the application when the button is clicked
         */
        private void minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        #endregion

        #region "Make Form Draggable"
        /**
         * Allow to drag the form from the panel
         */
        private void FormBorder_MouseDown(object sender, MouseEventArgs e)
        {
            _Moving = true;
            if (this.WindowState == FormWindowState.Maximized)
            {
                _FullSreen = true;
                _Moving = false;
            }
            _Offset = new Point(e.X, e.Y);
        }

        /**
         * Allow to drag the form from the panel
         */
        private void FormBorder_MouseMove(object sender, MouseEventArgs e)
        {
            if (_FullSreen)
            {
                this.WindowState = FormWindowState.Normal;
                this.Location = new Point(_Offset.X - (this.Width / 2), _Offset.Y);
                _FullSreen = false;
                _AfterFullSreen = true;
                // _Moving = true;
            }
            else if (_AfterFullSreen)
            {
                Point newlocation = this.Location;
                newlocation.X += e.X - (this.Width / 2);
                newlocation.Y += e.Y;
                this.Location = newlocation;
            }
            else if (_Moving)
            {
                Point newlocation = this.Location;
                newlocation.X += e.X - _Offset.X;
                newlocation.Y += e.Y - _Offset.Y;
                this.Location = newlocation;
            }
        }

        /**
         * Allow to drag the form from the panel
         */
        private void FormBorder_MouseUp(object sender, MouseEventArgs e)
        {
            _FullSreen = false;
            _Moving = false;
            _AfterFullSreen = false;
        }

        /**
         * Allow to drag the form from the text label
         */
        private void nameLabel_MouseDown(object sender, MouseEventArgs e)
        {
            _Moving = true;
            if (this.WindowState == FormWindowState.Maximized)
            {
                _FullSreen = true;
                _Moving = false;
            }
            _Offset = new Point(e.X, e.Y);
        }
        
        /**
         * Allow to drag the form from the text label
         */
        private void nameLabel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_FullSreen)
            {
                this.WindowState = FormWindowState.Normal;
                this.Location = new Point(e.X + this.Location.X, e.Y + this.Location.Y);
                _FullSreen = false;
                _Offset = new Point(e.X, e.Y);
                _Moving = true;
            }
            else if (_Moving)
            {
                Point newlocation = this.Location;
                newlocation.X += e.X - _Offset.X;
                newlocation.Y += e.Y - _Offset.Y;
                this.Location = newlocation;
            }
        }

        /**
         * Allow to drag the form from the text label
         */
        private void nameLabel_MouseUp(object sender, MouseEventArgs e)
        {
            _FullSreen = false;
            _Moving = false;
            _AfterFullSreen = false;
        }
        #endregion

        #region "Double Click Maximize"
        /**
         * Maximize or return to normal state when double clicked
         */
        private void FormBorder_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }
        
        /**
         * Maximize or return to normal state when double clicked
         */
        private void nameLabel_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        /**
         * Set the maximum size of the FlashObject
         */
        private void Game_Resize(object sender, EventArgs e)
        {
            axShockwaveFlash.MaximumSize = new Size(this.Width, this.Height - FormBorder.Height);
        }
        #endregion

        
    }
}