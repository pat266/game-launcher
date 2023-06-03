﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launcher_VLCM_niua_lsaj.Forms
{
    public partial class LoadingScreen : Form
    {
        public LoadingScreen()
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml("#121212"); // dark primary
            this.progressBar1.BackColor = Color.FromArgb(64, 64, 64);
            this.progressBar1.ForeColor = ColorTranslator.FromHtml("#FF9800"); // accent
            this.label1.ForeColor = Color.White;
        }
    }
}
