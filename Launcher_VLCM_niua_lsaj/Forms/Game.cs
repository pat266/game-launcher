using AxShockwaveFlashObjects;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Launcher_VLCM_niua_lsaj.Forms
{
    public partial class Game : Form
    {
        private AxShockwaveFlashObjects.AxShockwaveFlash axShockwaveFlash;
        public Game()
        {
            InitializeComponent();
            Initialize_Flash();
        }

        private void Initialize_Flash()
        {
            // Create the object
            this.axShockwaveFlash = new AxShockwaveFlashObjects.AxShockwaveFlash();

            // Set properties
            this.axShockwaveFlash.Dock = DockStyle.Fill;
            this.axShockwaveFlash.Enabled = true;
            this.axShockwaveFlash.Location = new Point(3, 33);
            this.axShockwaveFlash.Name = "axShockwaveFlash";
            this.axShockwaveFlash.Size = new Size(778, 525);
            this.axShockwaveFlash.TabIndex = 2;
            // Add the component to the form's control collection
            this.tableLayoutPanel1.Controls.Add(this.axShockwaveFlash, 0, 1);
        }

        private void Game_Load(object sender, EventArgs e)
        {
            // set the necessary information to flash control of form game
            string movie = string.Format("{0}?{1}", Program.flash_movie, Program.flash_vars);
            // axShockwaveFlash.Movie = movie;
            // axShockwaveFlash.LoadMovie(0, movie);


            var localSWF = Application.StartupPath + @"\AS3Game.swf";
            // receive data from AS3
            // axShockwaveFlash.FlashCall += new _IShockwaveFlashEvents_FlashCallEventHandler(AS3_Receive);
            axShockwaveFlash.LoadMovie(0, localSWF);
            // sending movie data to AS3
            axShockwaveFlash.CallFunction("<invoke name=\"loadMovie\" returntype=\"xml\"><arguments><string>" + movie + "</string></arguments></invoke>");
        }
    }
}