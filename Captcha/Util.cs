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

namespace Captcha
{
    internal class Util
    {
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
         * An old method that uses AForge.NET to clean the image.
         * NOT EFFECTIVE!!
         */
        private static Bitmap cleanImage(Image img)
        {
            // img.Save(@"C:\Users\nili266\Desktop\GitHub Repo\Launcher_VLCM_lsaj\Captcha\1.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            Bitmap bmp = new Bitmap(img);
            bmp = bmp.Clone(new Rectangle(0, 0, img.Width, img.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Erosion erosion = new Erosion();
            Dilatation dilatation = new Dilatation();
            Invert inverter = new Invert();
            ColorFiltering cor = new ColorFiltering();
            cor.Blue = new AForge.IntRange(200, 255);
            cor.Red = new AForge.IntRange(200, 255);
            cor.Green = new AForge.IntRange(200, 255);
            Opening open = new Opening();
            BlobsFiltering bc = new BlobsFiltering() { MinHeight = 10 };
            Closing close = new Closing();
            GaussianSharpen gs = new GaussianSharpen();
            ContrastCorrection cc = new ContrastCorrection();
            FiltersSequence seq = new FiltersSequence(gs, inverter, open, inverter, bc, inverter, open, cc, cor, bc, inverter);
            var filteredImg = seq.Apply(bmp);
            return (Bitmap)filteredImg;
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
