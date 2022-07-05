using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;
using Tesseract;
using System.Text.RegularExpressions;

using System.Runtime.InteropServices;

using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Net;
using System.Web.Hosting;

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
            int total = 0, original = 0, sauvola = 0, otsu = 0, iterative = 0, skeleton = 0, grayBitmap1 = 0, grayBitmap2 = 0;
            
            // for each file, check the correctness
            foreach (FileInfo file in Files)
            {
                // remove the extension of the original filename to get the answer
                string answer = Util.removeExtensions(file.Name);
                total++;

                // get the image
                Bitmap image = new Bitmap(file.FullName);

                // test the effect of resizing the image (not effective)
                // image = Util.ResizeImage(image, (int) (image.Width * 1.2), (int) (image.Height * 1.2));

                // check original (without image processing) correct
                if (original_correct(image, answer))
                {
                    original++;
                }
                
                // check sauvola correct
                if (sauvola_correct(image, answer))
                {
                    sauvola++;
                }

                // check otsu correct
                if (otsu_correct(image, answer))
                {
                    otsu++;
                }

                // check iterative correct
                if (iterative_correct(image, answer))
                {
                    iterative++;
                }

                // check skeleton correct
                if (skeleton_correct(image, answer))
                {
                    skeleton++;
                }

                // check grayBitmap1 correct
                if (gray_bitmap1_correct(image, answer))
                {
                    grayBitmap1++;
                }

                // check grayBitmap2 correct
                if (gray_bitmap2_correct(image, answer))
                {
                    grayBitmap2++;
                }
            }
            
            // print out the result
            Console.WriteLine("Total number of captchas: {0} captchas", total);
            Console.WriteLine("Without image processing: number of correct: {0}, percent: {1}%", original, Util.percentage(original, total));
            Console.WriteLine("Primitive Gray Bitmap 1: number of correct: {0}, percent: {1}%", grayBitmap1, Util.percentage(grayBitmap1, total));
            Console.WriteLine("Primitive Gray Bitmap 2: number of correct: {0}, percent: {1}%", grayBitmap2, Util.percentage(grayBitmap2, total));
            Console.WriteLine("Sauvola binarization: number of correct: {0}, percent: {1}%", sauvola, Util.percentage(sauvola, total));
            Console.WriteLine("Otsu binarization: number of correct: {0}, percent: {1}%", otsu, Util.percentage(otsu, total));
            Console.WriteLine("Iterative binarization: number of correct: {0}, percent: {1}%", iterative, Util.percentage(iterative, total));
            Console.WriteLine("Zhang-Suen skelenton: number of correct: {0}, percent: {1}%", skeleton, Util.percentage(skeleton, total));

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
    }
}
