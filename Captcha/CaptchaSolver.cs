using System;

using System.Drawing;
using Tesseract;

/**
 * Utilizing Tesseract-OCR and custom image processing methods to attempt to solve the captcha.
 */

namespace Captcha
{
    public class CaptchaSolver
    {
        TesseractEngine engine;

        // constructor
        public CaptchaSolver()
        {
            engine = new TesseractEngine(@"tessdata", "eng", EngineMode.Default);
            //For Numeric Captcha//
            engine.SetVariable("tessedit_char_whitelist", "1234567890");
            engine.SetVariable("tessedit_unrej_any_wd", true);
        }
        /**
         * Main method: clean the image using custom preprocessing and
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
         * "selectedIndex" = 0: Sauvola binarization
         * "selectedIndex" = 1: Otsu binarization
         * "selectedIndex" = 2: Iterative binarization
         * "selectedIndex" = 3: Zhang-Suen skelenton
         */
        public Bitmap SelectionProcessImage(int selectedIndex, Image img)
        {
            switch (selectedIndex)
            {
                case 0:
                    return Util.SauvolaBinarization((Bitmap) img);

                case 1:
                    return Util.OtsuBinarization((Bitmap) img);

                case 2:
                    return Util.IterativeBinarization((Bitmap) img);

                case 3:
                    return Util.SkeletonBinarization((Bitmap) img);

                default:
                    throw new ArgumentException("The selectedIndex is not valid");
            }
            
        }

    }
}
