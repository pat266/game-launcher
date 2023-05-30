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
            this.BackColor = ColorTranslator.FromHtml("#121212"); // dark primary
            this.saveImage.BackColor = ColorTranslator.FromHtml("#1F1B24"); // dark secondary
            this.saveImage.ForeColor = Color.White;

            picture = new PictureBox();
            picture.SizeMode = PictureBoxSizeMode.AutoSize;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(picture);

            this.KeyPreview = true; // Enable key event handling for the form
            this.KeyDown += MainForm_KeyDown; // Attach the event handler
        }

        public void SetImage(Image image)
        {
            picture.Image = image;
            this.Width = image.Width + 40;
            this.Height = image.Height + 100;
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Set the form's border style to fixed single
            this.MaximizeBox = false; // Disable maximizing the form
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

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close(); // Close the form when the Escape key is pressed
            }
        }
    }
}
