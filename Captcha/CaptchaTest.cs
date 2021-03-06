using System;

using System.IO;
using System.Drawing;

/**
 * A class to generate 100 captchas, then utilize different image
 * processing techniques to test how effective it would help
 * Tesseract-OCR to recognize the captchas.
 */

namespace Captcha
{
    internal class CaptchaTest
    {
        static CaptchaSolver captchaSolver;
        static string mainDir;
        [STAThread]
        static void Main()
        {
            captchaSolver = new CaptchaSolver();

            // generate 100 sample captchas
            // have to manually review the filename (its predicted content) and change
            // if it is wrong.
            generate_sample_captchas(100);

            // check the result of the image processing (only run this after manual check)
            check_correctness();

        }

        /**
         * Create a directory to store all of the generated captchas.
         * Only generate until the directory has `num` (param) captchas
         */
        private static void generate_sample_captchas(int num)
        {
            // get the path of the current exe directory
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            // Console.WriteLine(exePath);
            string exeDir = System.IO.Path.GetDirectoryName(exePath);
            // Console.WriteLine(exeDir);

            // get grand-parent directory
            string grandparentDir = Path.GetFullPath(Path.Combine(exeDir, @"..\.."));

            // Console.WriteLine(grandparentDir);

            // create a main directory to store all of the images
            mainDir = Path.Combine(grandparentDir, "CaptchaTestImages");
            Directory.CreateDirectory(mainDir);

            // begin generating captchas
            for (int i = 0; i < num; i++)
            {
                // searches the current directory to see if there have already been enough images
                int fCount = Directory.GetFiles(mainDir, "*.png", SearchOption.TopDirectoryOnly).Length;

                if (fCount >= num)
                {
                    Console.WriteLine("Already has enough captcha files of {0}", num);
                    // quit the program if there are already enough images
                    return;
                }
                
                // get a random captcha with no cookies
                Image random_captcha = Util.get_random_captcha(null);

                // get its value as name.
                string captcha_name = captchaSolver.solveCaptcha((Bitmap)random_captcha);

                // save the captcha to a file
                string imageDirectory = Path.Combine(mainDir, captcha_name);
                imageDirectory += ".png";

                // save the image to a file
                random_captcha.Save(imageDirectory);

            }
        }

        /**
         * Check the correctness of the pre-process methods by comparing their outputs
         * versus the name of the image (correct value) in the `CaptchaTestImages` folder
         * (or the value of the mainDir variable).
         * Assumption: the name of the image is the correct value. (by manual check)
         */
        public static void check_correctness()
        {
            // get all of the .png files in the directory
            DirectoryInfo d = new DirectoryInfo(mainDir);
            FileInfo[] Files = d.GetFiles("*.png");

            // keep track of the total number of captchas and the number of correct captchas for each pre-process method
            int total = 0, original = 0, sauvola = 0, otsu = 0, iterative = 0, skeleton = 0, grayBitmap1 = 0, grayBitmap2 = 0, combination = 0;
            
            // for each file, check the correctness
            foreach (FileInfo file in Files)
            {
                // remove the extension of the original filename to get the answer
                string answer = Util.removeExtensions(file.Name);
                total++;

                // get the image
                Bitmap image = new Bitmap(file.FullName);

                // test the effect of resizing the image (not effective)
                // image = Util.ResizeImage(image, (int) (image.Width * 2), (int) (image.Height * 2));

                // deep clone the image
                Bitmap[] bitmaps = new Bitmap[8];
                for (int i = 0; i < bitmaps.Length; i++)
                {
                    bitmaps[i] = Util.Clone(image);
                }
                // check original (without image processing) correct
                if (original_correct(bitmaps[0], answer))
                {
                    original++;
                }

                // check sauvola correct                
                if (sauvola_correct(bitmaps[1], answer))
                {
                    sauvola++;
                }

                // check otsu correct              
                if (otsu_correct(bitmaps[2], answer))
                {
                    otsu++;
                }

                // check iterative correct
                if (iterative_correct(bitmaps[3], answer))
                {
                    iterative++;
                }

                // check skeleton correct
                if (skeleton_correct(bitmaps[4], answer))
                {
                    skeleton++;
                }

                // check grayBitmap1 correct
                if (gray_bitmap1_correct(bitmaps[5], answer))
                {
                    grayBitmap1++;
                }

                // check grayBitmap2 correct
                if (gray_bitmap2_correct(bitmaps[6], answer))
                {
                    grayBitmap2++;
                }

                // check combination correct
                if (combination_correct(bitmaps[7], answer))
                {
                    combination++;
                }
            }
            
            // print out the result
            Console.WriteLine("Total number of captchas: {0} captchas", total);
            Console.WriteLine("Without image processing: {0}% correct", Util.percentage(original, total));
            Console.WriteLine("Primitive Gray Bitmap 1: {0}% correct", Util.percentage(grayBitmap1, total));
            Console.WriteLine("Primitive Gray Bitmap 2: {0}% correct", Util.percentage(grayBitmap2, total));
            Console.WriteLine("Sauvola binarization: {0}% correct", Util.percentage(sauvola, total));
            Console.WriteLine("Otsu binarization: {0}% correct", Util.percentage(otsu, total));
            Console.WriteLine("Iterative binarization: {0}% correct", Util.percentage(iterative, total));
            Console.WriteLine("Zhang-Suen skelenton: {0}% correct", Util.percentage(skeleton, total));
            Console.WriteLine("Combination of without image processing and Sauvola: {0}% correct", Util.percentage(combination, total));

        }

        /**
         * Method to determine if the image process helps to determine the correct answer
         * Without any image process methods.
         */
        private static Boolean original_correct(Bitmap randomCaptcha, string value)
        {
            string result = captchaSolver.solveCaptcha(randomCaptcha);
            return value.Equals(result);
        }
        
        /**
         * Method to determine if the image process helps to determine the correct answer
         * Util.SauvolaBinarization()
         */
        private static Boolean sauvola_correct(Bitmap randomCaptcha, string value)
        {
            Bitmap sauvola = Util.SauvolaBinarization(randomCaptcha);
            string result = captchaSolver.solveCaptcha(sauvola);
            return value.Equals(result);
        }

        /**
         * Method to determine if the image process helps to determine the correct answer
         * Util.OtsuBinarization()
         */
        private static Boolean otsu_correct(Bitmap randomCaptcha, string value)
        {
            Bitmap otsu = Util.OtsuBinarization(randomCaptcha);
            string result = captchaSolver.solveCaptcha(otsu);
            return value.Equals(result);
        }

        /**
         * Method to determine if the image process helps to determine the correct answer
         * Util.IterativeBinarization()
         */
        private static Boolean iterative_correct(Bitmap randomCaptcha, string value)
        {
            Bitmap iterative = Util.IterativeBinarization(randomCaptcha);
            string result = captchaSolver.solveCaptcha(iterative);
            return value.Equals(result);
        }

        /**
         * Method to determine if the image process helps to determine the correct answer
         * Util.SkeletonBinarization()
         */
        private static Boolean skeleton_correct(Bitmap randomCaptcha, string value)
        {
            Bitmap skeleton = Util.SkeletonBinarization(randomCaptcha);
            string result = captchaSolver.solveCaptcha(skeleton);
            return value.Equals(result);
        }

        /**
         * Method to determine if the image process helps to determine the correct answer
         * Util.ToGrayBitmap()
         */
        private static Boolean gray_bitmap1_correct(Bitmap randomCaptcha, string value)
        {
            Bitmap grayBitmap1 = Util.ToGrayBitmap(randomCaptcha);
            string result = captchaSolver.solveCaptcha(grayBitmap1);
            return value.Equals(result);
        }

        /**
         * Method to determine if the image process helps to determine the correct answer
         * Util.ToGrayBitmap2()
         */
        private static Boolean gray_bitmap2_correct(Bitmap randomCaptcha, string value)
        {
            Bitmap grayBitmap2 = Util.ToGrayBitmap2(randomCaptcha);
            string result = captchaSolver.solveCaptcha(grayBitmap2);
            return value.Equals(result);
        }

        /**
         * Method to determine if the image process helps to determine the correct answer
         * When no mask returns no answer, attempt to use Sauvola Binarization to get it
         */
        private static Boolean combination_correct(Bitmap randomCaptcha, string value)
        {
            // attempt to solve it without masking
            string result = captchaSolver.solveCaptcha(randomCaptcha);
            if (result.Length < 4)
            {
                // attempt to solve it with masking
                result = captchaSolver.solveCaptcha(Util.SauvolaBinarization(randomCaptcha));
            }

            return value.Equals(result);
        }
    }
}
