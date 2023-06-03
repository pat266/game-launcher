using System;
using System.Windows.Forms;

namespace Launcher_VLCM_niua_lsaj.Forms
{
    public partial class Game : Form
    {
        public Game()
        {
            InitializeComponent();
        }

        private void Game_Load(object sender, EventArgs e)
        {
            axShockwaveFlash.Movie = string.Format("{0}?{1}", Program.flash_movie, Program.flash_vars); // đưa dữ liệu cần để load game vào flash control trên form game
        }
    }
}