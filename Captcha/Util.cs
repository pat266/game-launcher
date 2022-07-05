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
using System.Windows.Media.Imaging;
using System.Net;

namespace Captcha
{
    public class Util
    {
        /**
         * Helper method:
         * Retrieve the captcha image from the server and display it in the form.
         */
        public static Image get_random_captcha(CookieContainer cookieContainer)
        {
            // get the captcha based on the cookies
            byte[] captcha_data =
                Web_Request.Web_Request.send_request("http://www.niua.com/seccode.php",
                                                    "GET",
                                                    null,
                                                    cookieContainer);
            if (captcha_data == null)
                return null;

            // put the captcha in picture box control in the login form
            using (MemoryStream memory_stream = new MemoryStream(captcha_data))
            {
                return Image.FromStream(memory_stream);
            }
        }
        /**
         * Helper method:
         * Replace all of the white spaces in a string and retrieve only the first 4 numerical values.
         */
        public static string cleanResult(string text)
        {
            // remove all spaces
            text = Regex.Replace(text, @"\s+", "");
            // get only the first 4 numerical values
            if (text.Length > 4)
                text = text.Substring(0, 4);
            return text;
        }

        /**
         * Helper method:
         * Remove extensions from the filename.
         */
        public static string removeExtensions(string filename)
        {
            filename = Path.GetFileNameWithoutExtension(filename);
            /**
            int index = filename.LastIndexOf('.');
            if (index > 0)
                filename = filename.Substring(0, index);
            **/
            return filename;
        }

        /**
         * Calculate percentage given 2 numbers
         */
        public static double percentage(int num1, int num2)
        {
            return (double)num1 / (double)num2 * 100;
        }

        //Convert Bitmap to BitmapImage
        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            Bitmap bitmapSource = new Bitmap(bitmap.Width, bitmap.Height);
            int i, j;
            for (i = 0; i < bitmap.Width; i++)
                for (j = 0; j < bitmap.Height; j++)
                {
                    System.Drawing.Color pixelColor = bitmap.GetPixel(i, j);
                    System.Drawing.Color newColor = System.Drawing.Color.FromArgb(pixelColor.R, pixelColor.G, pixelColor.B);
                    bitmapSource.SetPixel(i, j, newColor);
                }
            MemoryStream ms = new MemoryStream();
            bitmapSource.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(ms.ToArray());
            bitmapImage.EndInit();

            return bitmapImage;
        }

        /** 
         * New method to process the image:
         * Ultilize Sauvola binarization to process colored image to gray image.
         */
        public static Bitmap SauvolaBinarization(Bitmap bitmap)
        {
            Byte[,] BinaryArray = new Byte[bitmap.Height, bitmap.Width];

            Byte[,] grayArraySrc = Preprocess.Preprocess.ToGrayArray(bitmap);
            BinaryArray = Preprocess.Preprocess.Sauvola(grayArraySrc);

            Bitmap GrayBmp = Preprocess.Preprocess.BinaryArrayToBinaryBitmap(BinaryArray);

            return GrayBmp;
        }

        /** 
         * New method to process the image:
         * Ultilize Otsu binarization to process colored image to gray image.
         */
        public static Bitmap OtsuBinarization(Bitmap bitmap)
        {
            Byte[,] BinaryArray = new Byte[bitmap.Height, bitmap.Width];

            int threshold;
            BinaryArray = Preprocess.Preprocess.ToBinaryArray(bitmap, Preprocess.Preprocess.BinarizationMethods.Otsu, out threshold);

            Bitmap GrayBmp = Preprocess.Preprocess.BinaryArrayToBinaryBitmap(BinaryArray);

            return GrayBmp;
        }

        /** 
         * New method to process the image:
         * Ultilize Iterative binarization to process colored image to gray image.
         */
        public static Bitmap IterativeBinarization(Bitmap bitmap)
        {
            Byte[,] BinaryArray = new Byte[bitmap.Height, bitmap.Width];

            int threshold;
            BinaryArray = Preprocess.Preprocess.ToBinaryArray(bitmap, Preprocess.Preprocess.BinarizationMethods.Iterative, out threshold);

            Bitmap GrayBmp = Preprocess.Preprocess.BinaryArrayToBinaryBitmap(BinaryArray);

            return GrayBmp;
        }

        /** 
         * New method to process the image:
         * Ultilize Zhang-Suen skelenton binarization to process colored image to gray image.
         */
        public static Bitmap SkeletonBinarization(Bitmap bitmap)
        {
            Byte[,] BinaryArray = new Byte[bitmap.Height, bitmap.Width];

            Byte[,] grayArraySrc = Preprocess.Preprocess.ToGrayArray(bitmap);
            BinaryArray = Preprocess.Preprocess.ToGrayArray(bitmap);
            BinaryArray = Preprocess.Preprocess.Sauvola(BinaryArray);
            BinaryArray = Preprocess.Preprocess.ThinPicture(BinaryArray);

            Bitmap GrayBmp = Preprocess.Preprocess.BinaryArrayToBinaryBitmap(BinaryArray);

            return GrayBmp;
        }

        /// <summary>
        /// Past method to convert colored Bitmay to Bitmap grayscale
        /// </summary>
        /// < param  name = " bmp " >Original Bitmap</ param >
        /// < returns >Grayscale Bitmap</ returns >
        public static Bitmap ToGrayBitmap(Bitmap bmp)
        {
            Int32 PixelHeight = bmp.Height; // Image height
            Int32 PixelWidth = bmp.Width;    // image width
            Int32 Stride = ((PixelWidth * 3 + 3) >> 2) << 2;     // stride width
            Byte[] Pixels = new Byte[PixelHeight * Stride];

            // Lock the bitmap to system memory
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, PixelWidth, PixelHeight), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            Marshal.Copy(bmpData.Scan0, Pixels, 0, Pixels.Length);   // Copy data from unmanaged memory to managed memory
            bmp.UnlockBits(bmpData);     // Unlock the bitmap from system memory

            // Convert pixel data to grayscale data
            Int32 GrayStride = ((PixelWidth + 3) >> 2) << 2;
            Byte[] GrayPixels = new Byte[PixelHeight * GrayStride];
            for (Int32 i = 0; i < PixelHeight; i++)
            {
                Int32 Index = i * Stride;
                Int32 GrayIndex = i * GrayStride;
                for (Int32 j = 0; j < PixelWidth; j++)
                {
                    GrayPixels[GrayIndex++] = Convert.ToByte((Pixels[Index + 2] * 19595 + Pixels[Index + 1] * 38469 + Pixels[Index] * 7471 + 32768) >> 16);
                    Index += 3;
                }
            }

            // create grayscale image
            Bitmap GrayBmp = new Bitmap(PixelWidth, PixelHeight, PixelFormat.Format8bppIndexed);

            // set color table
            ColorPalette cp = GrayBmp.Palette;
            for (int i = 0; i < 256; i++) cp.Entries[i] = Color.FromArgb(i, i, i);
            GrayBmp.Palette = cp;

            // Set the bitmap image properties
            BitmapData GrayBmpData = GrayBmp.LockBits(new Rectangle(0, 0, PixelWidth, PixelHeight), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            Marshal.Copy(GrayPixels, 0, GrayBmpData.Scan0, GrayPixels.Length);
            GrayBmp.UnlockBits(GrayBmpData);

            return GrayBmp;
        }

        /// <summary>
        /// Past method to convert colored Bitmay to Bitmap grayscale
        /// </summary>
        /// < param  name = " bmp " >Original Bitmap</ param >
        /// < returns >Grayscale Bitmap</ returns >
        public static Bitmap ToGrayBitmap2(Bitmap bmp)
        {
            Int32 PixelHeight = bmp.Height; // Image height
            Int32 PixelWidth = bmp.Width;    // image width
            Int32 Stride = ((PixelWidth * 3 + 3) >> 2) << 2;     // stride width
            Byte[] Pixels = new Byte[PixelHeight * Stride];

            // Lock the bitmap to system memory
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, PixelWidth, PixelHeight), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            Marshal.Copy(bmpData.Scan0, Pixels, 0, Pixels.Length);   // Copy data from unmanaged memory to managed memory
            bmp.UnlockBits(bmpData);     // Unlock the bitmap from system memory

            // Convert pixel data to grayscale data
            Int32 GrayStride = ((PixelWidth + 3) >> 2) << 2;
            Byte[] GrayPixels = new Byte[PixelHeight * GrayStride];
            for (Int32 i = 0; i < PixelHeight; i++)
            {
                Int32 Index = i * Stride;
                Int32 GrayIndex = i * GrayStride;
                for (Int32 j = 0; j < PixelWidth; j++)
                {
                    GrayPixels[GrayIndex++] = Convert.ToByte((Pixels[Index + 2] * 19595 + Pixels[Index + 1] * 38469 + Pixels[Index] * 7471 + 32768) >> 16);
                    Index += 3;
                }
            }

            // create grayscale image
            Bitmap GrayBmp = new Bitmap(PixelWidth, PixelHeight, PixelFormat.Format8bppIndexed);

            // set color table
            ColorPalette cp = GrayBmp.Palette;
            for (int i = 0; i < 256; i++) cp.Entries[i] = Color.FromArgb(i, i, i);
            GrayBmp.Palette = cp;

            // Set the bitmap image properties
            BitmapData GrayBmpData = GrayBmp.LockBits(new Rectangle(0, 0, PixelWidth, PixelHeight), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            Marshal.Copy(GrayPixels, 0, GrayBmpData.Scan0, GrayPixels.Length);
            GrayBmp.UnlockBits(GrayBmpData);

            return GrayBmp;
        }
    }
}
