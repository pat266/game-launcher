using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launcher_VLCM_niua_lsaj
{
    public partial class TranslatedImage : Form
    {
        private PictureBox picture;
        public TranslatedImage()
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml("#1C1C1C");

            picture = new PictureBox();
            picture.SizeMode = PictureBoxSizeMode.AutoSize;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(picture);
        }

        public void SetImage(Image image)
        {
            picture.Image = image;
            this.Width = image.Width + 40;
            this.Height = image.Height + 100;
        }

        private void cButton1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Images|*.png;*.bmp;*.jpg",
                OverwritePrompt = true
            };
            ImageFormat format = ImageFormat.Png;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string ext = System.IO.Path.GetExtension(sfd.FileName);
                switch (ext)
                {
                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;
                    case ".bmp":
                        format = ImageFormat.Bmp;
                        break;
                }
                picture.Image.Save(sfd.FileName, format);
            }
        }
    }
}
