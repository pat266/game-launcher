using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;
using AForge.Imaging.Filters;
using Tesseract;
using System.Text.RegularExpressions;

using System.Runtime.InteropServices;

using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

/**
 * Code taken from https://foxlearn.com/windows-forms/recaptcha-using-tesseract-ocr-in-csharp-373.html
 */

namespace Captcha
{
    public class CaptchaSolver
    {
        TesseractEngine engine;

        public static int m_SauvolaWidth = 100;
        public static double m_SauvolaFactor = 0.3;
        
        [STAThread]
        static void Main()
        {
            
        }

        // constructor
        public CaptchaSolver()
        {
            engine = new TesseractEngine(@"tessdata", "eng", EngineMode.Default);
            //For Numeric Captcha//
            engine.SetVariable("tessedit_char_whitelist", "1234567890");
            engine.SetVariable("tessedit_unrej_any_wd", true);
        }
        /**
         * Main method: clean the image using AForge.NET and
         * retrieve text using Tesseract.
         */
        public string solveCaptcha(Bitmap bitmap)
        {
            string res = string.Empty;
            using (var page = engine.Process(bitmap, PageSegMode.SingleLine))
                res = page.GetText();
            // Console.WriteLine("This is the result: ");
            return Util.cleanResult(res);
        }

        /**
         * Select which Bitmap to return
         */
        public Bitmap SelectionChangedMethod(int selectedIndex, Image img)
        {
            Bitmap srcBmp = new Bitmap(img);

            Byte[,] BinaryArray = new Byte[srcBmp.Height, srcBmp.Width];

            int threshold;

            switch (selectedIndex)
            {
                case 0:
                    Byte[,] grayArraySrc = Preprocess.Preprocess.ToGrayArray(srcBmp);
                    BinaryArray = Preprocess.Preprocess.Sauvola(grayArraySrc);
                    break;

                case 1:
                    BinaryArray = Preprocess.Preprocess.ToBinaryArray(srcBmp, Preprocess.Preprocess.BinarizationMethods.Otsu, out threshold);
                    break;

                case 2:
                    BinaryArray = Preprocess.Preprocess.ToBinaryArray(srcBmp, Preprocess.Preprocess.BinarizationMethods.Iterative, out threshold);
                    break;

                case 3:
                    BinaryArray = Preprocess.Preprocess.ToGrayArray(srcBmp);
                    BinaryArray = Preprocess.Preprocess.Sauvola(BinaryArray);
                    BinaryArray = Preprocess.Preprocess.ThinPicture(BinaryArray);
                    break;

                case 4:
                    BinaryArray = CaptchaSegment.CaptchaSegmentFun(srcBmp);
                    break;

                default: break;
            }

            Bitmap GrayBmp = Preprocess.Preprocess.BinaryArrayToBinaryBitmap(BinaryArray);

            return GrayBmp;
        }

    }
}
