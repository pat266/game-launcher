using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using System.Reflection;
// using OcrLiteLib;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Threading.Tasks;
using GTranslate.Translators;
using AxShockwaveFlashObjects;
using System.Xml;

namespace Launcher_VLCM_niua_lsaj.Forms
{
    public partial class Game : Form
    {
        [DllImport("winmm.dll")]
        private static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

        [DllImport("winmm.dll")]
        private static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

        private bool isMute = false;
        private ToolTip toolTip;

        // make the winform draggable
        private bool _Moving = false;
        private bool _FullSreen = false;
        private bool _AfterFullSreen = false;
        private Point _Offset;
        
        private OcrLite ocrEngine;
        private AggregateTranslator translator;
        private TranslatedImage translatedImageForm;

        // store the information of the zoom level and its information
        private float[] zoomLevels;
        private int mapZoomIndex;
        private int menuZoomIndex;
        
        /**
         * Constructor
         */
        public Game()
        {
            InitializeComponent();
            this.ShowInTaskbar = true;

            this.SetStyle(ControlStyles.ResizeRedraw, true); // avoid visual artifacts
            this.Icon = (Icon) Properties.Resources.ResourceManager.GetObject("app_icon"); ;

            // add KeyEvent to the form
            this.KeyUp += new KeyEventHandler(KeyEvent);
            this.MouseWheel += new MouseEventHandler(MouseScroll);

            // initialize the OCR Engine
            ocrEngine = new OcrLite();

            // mute the game
            Mute_Game();

            // set basic buttons
            Set_Control_Buttons();

            // set the tool tip for the buttons
            toolTip = new ToolTip();
            SetToolTip();

            // load the Onnx model
            loadOnnxModel();

            // set dark mode
            setDarkMode();

            // initialize the Translator
            translator = new AggregateTranslator();

            // initialize the values of the zoom levels
            zoomLevels = new float[] {0.1f, 0.25f, 0.5f, 1.0f, 1.25f, 1.5f, 2.0f, 2.5f, 3.0f};
            mapZoomIndex = 3;
            menuZoomIndex = 3;
        }

        /**
         * Load the game
         */
        private void Game_Load(object sender, EventArgs e)
        {
            // set the necessary information to flash control of form game
            string movie = string.Format("{0}?{1}", Controller.flash_movie, Controller.flash_vars);
            // axShockwaveFlash.Movie = movie;
            // axShockwaveFlash.LoadMovie(0, movie);
            
            
            var localSWF = Application.StartupPath + @"\AS3Game.swf";
            // receive data from AS3
            axShockwaveFlash.FlashCall += new _IShockwaveFlashEvents_FlashCallEventHandler(AS3_Receive); 
            axShockwaveFlash.LoadMovie(0, localSWF);
            // sending movie data to AS3
            axShockwaveFlash.CallFunction("<invoke name=\"loadMovie\" returntype=\"xml\"><arguments><string>" + movie + "</string></arguments></invoke>");
            
            Adjust_Gameform();

            Adjust_FormBorder();
        }
        
        /**
         * Method to handle various methods from pressing down the key
         */
        private async void KeyEvent(object sender, KeyEventArgs e) //Keyup Event 
        {
            // press F8 to open up the translation feature
            if (e.KeyCode == Keys.F8)
            {
                await StartTranslatingProcess();
            }
            // press F11 to toggle full screen
            if (e.KeyCode == Keys.F11)
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
            exit.Image = (Bitmap) Properties.Resources.ResourceManager.GetObject("close");
            exit.MinimumSize = new Size(30, 30);
            exit.MaximumSize = new Size(30, 30);

            // maximize
            maximize.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("maximize");
            maximize.MinimumSize = new Size(30, 30);
            maximize.MaximumSize = new Size(30, 30);
            
            // minimize
            minimize.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("minimize");
            minimize.MinimumSize = new Size(30, 30);
            minimize.MaximumSize = new Size(30, 30);

            // translation
            translationButton.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("translation");
            translationButton.MinimumSize = new Size(30, 30);
            translationButton.MaximumSize = new Size(30, 30);

            // reset
            reset.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("reset");
            reset.MinimumSize = new Size(30, 30);
            reset.MaximumSize = new Size(30, 30);

            // restart
            restart.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("restart");
            restart.MinimumSize = new Size(30, 30);
            restart.MaximumSize = new Size(30, 30);
        }

        private void setDarkMode()
        {
            this.BackColor = Color.Black;
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
            ApplicationSound.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("mute"); ;
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
            ApplicationSound.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("unmute"); ;
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

        private async void translationButton_Click(object sender, EventArgs e)
        {
            await StartTranslatingProcess();
        }

        /**
         * Call the function in AS3 to restart the game
         */
        private void restart_Click(object sender, EventArgs e)
        {
            // call the function to reset the zoom to default from AS3
            axShockwaveFlash.CallFunction("<invoke name=\"restartGame\" returntype=\"xml\"><arguments><string>" +
                "something" + "</string></arguments></invoke>");
        }

        /**
         * Set tool tip for the buttons
         */
        private void SetToolTip()
        {
            toolTip.SetToolTip(ApplicationSound, "Mute/Unmute");
            toolTip.SetToolTip(translationButton, "Translate Screen");
            toolTip.SetToolTip(reset, "Reset Zoom Level");
            toolTip.SetToolTip(restart, "Restart");
            toolTip.SetToolTip(minimize, "Minimize");
            toolTip.SetToolTip(maximize, "Maximize");
            toolTip.SetToolTip(exit, "Close");
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

        #region "Translation Methods"
        /**
         * Helper Method to load ONNX model
         */
        private void loadOnnxModel(
            string detName = "dbnet.onnx",
            string clsName = "angle_net.onnx",
            string recName = "crnn_lite_lstm.onnx",
            string keysName = "keys.txt",
            int numThread = 4
            )
        {

            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            string modelsDir = appPath + "models";
            // modelsTextBox.Text = modelsDir;
            string detPath = modelsDir + "\\" + detName;
            string clsPath = modelsDir + "\\" + clsName;
            string recPath = modelsDir + "\\" + recName;
            string keysPath = modelsDir + "\\" + keysName;
            bool isDetExists = File.Exists(detPath);
            if (!isDetExists)
            {
                MessageBox.Show("Model file not found at: " + detPath);
            }
            bool isClsExists = File.Exists(clsPath);
            if (!isClsExists)
            {
                MessageBox.Show("Model file not found at: " + clsPath);
            }
            bool isRecExists = File.Exists(recPath);
            if (!isRecExists)
            {
                MessageBox.Show("Model file not found at: " + recPath);
            }
            bool isKeysExists = File.Exists(keysPath);
            if (!isKeysExists)
            {
                MessageBox.Show("Keys file not found at: " + keysPath);
            }
            if (isDetExists && isClsExists && isRecExists && isKeysExists)
            {
                if (ocrEngine != null)
                {
                    ocrEngine = null;
                    System.GC.Collect();
                }
                ocrEngine = new OcrLite();
                ocrEngine.InitModels(detPath, clsPath, recPath, keysPath, (int)numThread);
            }
            else
            {
                MessageBox.Show("Initialization failed, please confirm the model folder and file, and then reinitialize!");
            }
            System.GC.Collect();
        }

        /**
         * Detect text in the image and draw Bounding Rectangles around it.
         * Using IronOCR to get both bounding rectangles and Onnx model for extracting text
         */
        /*private async Task<OcrResult> ProcessText_Onnx(
            Bitmap bitmap,
            int imgResize,
            int padding = 50,
            float boxScoreThresh = 0.618f,
            float boxThresh = 0.300f,
            float unClipRatio = 2.0f,
            bool doAngle = true,
            bool mostAngle = true,
            bool extractText = false,
            bool translateText = false
            )
        {
            if (ocrEngine == null)
            {
                MessageBox.Show("OCR Engine is uninitialized, cannot execute!");
                return null;
            }
            Image<Bgr, byte> imageCV = bitmap.ToImage<Bgr, byte>(); //Image Class from Emgu.CV
            Mat mat = imageCV.Mat;

            OcrResult ocrResult = await Task.Run(() => ocrEngine.Detect(
                mat, padding, imgResize, boxScoreThresh, boxThresh, unClipRatio,
                doAngle, mostAngle, extractText, translateText, translator));

            System.GC.Collect(); // clean up the memory
            return ocrResult;
        }*/

        private async Task StartTranslatingProcess()
        {
            //this is the screen the current form is on
            Screen screen = Screen.FromControl(this);
            // Utilizes snipping tool to capture the part of the screen
            var img = SnippingTool.Snip(screen);
            // if the SnippingTool is cancelled, break out of the function
            if (img == null)
                return;

            Bitmap bitmap = new Bitmap(img);
            bitmap.ToImage<Bgr, byte>();
            this.WindowState = FormWindowState.Normal;

            LoadingScreen loadingScreen = new LoadingScreen();
            loadingScreen.Show();
            loadingScreen.TopMost = true;

            if (translatedImageForm != null)
            {
                translatedImageForm.Close();
                translatedImageForm = null;
                System.GC.Collect();
            }
            translatedImageForm = new TranslatedImage();
            OcrResult ocrResult = await ProcessText_Onnx(bitmap, bitmap.Width, translateText: true);
            // show the translated image

            Mat matImg = ocrResult.BoxImg;
            translatedImageForm.SetImage(matImg.ToImage<Bgr, Byte>().ToBitmap());


            // loadingFormThread.Abort(); // remove loading form
            loadingScreen.Close();

            // close the translated image form when the main form is closed
            this.FormClosing += (s, args) =>
            {
                translatedImageForm.Close();
            };

            translatedImageForm.ShowInTaskbar = false;
            translatedImageForm.Show(); // show the translated image form
        }

        #endregion

        #region "Zoom In/Out - AS3"
        /**
         * A Function to receive data from ActionScript3 loaded SWF file
         */
        public void AS3_Receive(object sender, _IShockwaveFlashEvents_FlashCallEvent e)
        {
            string message = "";

            // message is in xml format so we need to parse it
            XmlDocument document = new XmlDocument();
            document.LoadXml(e.request);
            // get attributes to see which command flash is trying to call
            XmlAttributeCollection attributes = document.FirstChild.Attributes;
            String command = attributes.Item(0).InnerText;
            // get parameters
            XmlNodeList list = document.GetElementsByTagName("arguments");
            // Interpret command
            switch (command)
            {
                case "as3ToC#": message = list[0].InnerText; break;
                case "Some_Other_Command": break;
            }
            // MessageBox.Show(message, "Received", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /**
         * Method to handle various methods from pressing down the key
         */
        private void MouseScroll(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                // The user scrolled up.
                Zoom(true);
            }
            else
            {
                // The user scrolled down.
                Zoom(false);
            }

        }

        /**
         * A function to call the method in AS3 to zoom in/out the map or menus
         */
        private void Zoom(Boolean increase)
        {
            if ((Control.ModifierKeys & Keys.Shift) != 0)
            {
                // if pressing both control and shift keys, zoom the menus
                if (increase)
                {
                    if (menuZoomIndex < zoomLevels.Length - 1)
                    {
                        menuZoomIndex++;
                    }
                }
                else
                {
                    if (menuZoomIndex > 0)
                    {
                        menuZoomIndex--;
                    }
                }
                // call the function to zoom menus from AS3
                axShockwaveFlash.CallFunction("<invoke name=\"zoomMenu\" returntype=\"xml\"><arguments><string>" +
                    zoomLevels[menuZoomIndex].ToString("N2") + "</string></arguments></invoke>");
            }
            else if (Control.ModifierKeys == Keys.Control)
            {
                // if pressing only control key, zoom the map
                if (increase)
                {
                    if (mapZoomIndex < zoomLevels.Length - 1)
                    {
                        mapZoomIndex++;
                    }
                }
                else
                {
                    if (mapZoomIndex > 0)
                    {
                        mapZoomIndex--;
                    }
                }
                // call the function to zoom map from AS3
                axShockwaveFlash.CallFunction("<invoke name=\"zoomMap\" returntype=\"xml\"><arguments><string>" +
                    zoomLevels[mapZoomIndex].ToString("N2") + "</string></arguments></invoke>");
            }
        }

        /**
         * Call the function in AS3 to reset both of the zoom levels
         */
        private void reset_Click(object sender, EventArgs e)
        {
            // call the function to reset the zoom to default from AS3
            axShockwaveFlash.CallFunction("<invoke name=\"resetZoom\" returntype=\"xml\"><arguments><string>" +
                "something" + "</string></arguments></invoke>");
        }

        #endregion

    }
}