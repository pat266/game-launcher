using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GTranslate.Translators;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace OcrLiteLib
{
    public class OcrUtils
    {
        public static Tensor<float> SubstractMeanNormalize(Mat src, float[] meanVals, float[] normVals)
        {
            int cols = src.Cols;
            int rows = src.Rows;
            int channels = src.NumberOfChannels;
            Image<Rgb, byte> srcImg = src.ToImage<Rgb, byte>();
            byte[,,] imgData = srcImg.Data;
            Tensor<float> inputTensor = new DenseTensor<float>(new[] { 1, channels, rows, cols });
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    for (int ch = 0; ch < channels; ch++)
                    {
                        var value = imgData[r, c, ch];
                        float data = (float)(value * normVals[ch] - meanVals[ch] * normVals[ch]);
                        inputTensor[0, ch, r, c] = data;
                    }
                }
            }
            return inputTensor;
        }

        public static Mat MakePadding(Mat src, int padding)
        {
            if (padding <= 0) return src;
            MCvScalar paddingScalar = new MCvScalar(255, 255, 255);
            Mat paddingSrc = new Mat();
            CvInvoke.CopyMakeBorder(src, paddingSrc, padding, padding, padding, padding, BorderType.Isolated, paddingScalar);
            return paddingSrc;
        }

        public static int GetThickness(Mat boxImg)
        {
            int minSize = boxImg.Cols > boxImg.Rows ? boxImg.Rows : boxImg.Cols;
            int thickness = minSize / 1000 + 2;
            return thickness;
        }

        public static void DrawTextBox(Mat boxImg, List<Point> box, int thickness)
        {
            if (box == null || box.Count == 0)
            {
                return;
            }
            var color = new MCvScalar(255, 0, 0);//R(255) G(0) B(0)

            /**
            CvInvoke.Line(boxImg, box[0], box[1], color, thickness);
            CvInvoke.Line(boxImg, box[1], box[2], color, thickness);
            CvInvoke.Line(boxImg, box[2], box[3], color, thickness);
            CvInvoke.Line(boxImg, box[3], box[0], color, thickness);
            **/

            // /**
            int width = GetWidth(box);
            int height = GetHeight(box);
            Point topLeftPoint = GetTopLeftPoint(box);
            // define the area of the triangle
            Rectangle rectangle = new Rectangle(topLeftPoint, new Size(width, height));

            // fill the area with the color
            CvInvoke.Rectangle(boxImg, rectangle, color, thickness);
            // **/
        }

        public static void DrawTextBoxes(Mat src, List<TextBox> textBoxes, int thickness)
        {
            for (int i = 0; i < textBoxes.Count; i++)
            {
                TextBox t = textBoxes[i];
                DrawTextBox(src, t.Points, thickness);
            }
        }

        public static int GetWidth(List<Point> box)
        {
            /**
             * The points in the rectangle is always at
             * 0 -> 1
             *      |
             * 3 <- 2
             * with the number representing the index of the point in the list
             */
            int widthTop = Math.Abs(box[0].X - box[1].X);
            int widthBottom = Math.Abs(box[2].X - box[3].X);
            return Math.Max(widthTop, widthBottom);
        }

        public static int GetHeight(List<Point> box)
        {
            /**
             * The points in the rectangle is always at
             * 0 -> 1
             *      |
             * 3 <- 2
             * with the number representing the index of the point in the list
             */
            int heightLeft = Math.Abs(box[0].Y - box[3].Y);
            int heightRight = Math.Abs(box[1].Y - box[2].Y);
            return Math.Max(heightLeft, heightRight);
        }

        public static Point GetTopLeftPoint(List<Point> box)
        {
            /**
             * The points in the rectangle is always at
             * 0 -> 1
             *      |
             * 3 <- 2
             * with the number representing the index of the point in the list
             */
            return box[0];
        }

        public static void DrawBackgroundBox(Mat src, List<Point> box)
        {
            int width = GetWidth(box);
            int height = GetHeight(box);
            Point topLeftPoint = GetTopLeftPoint(box);
            // define the area of the triangle
            Rectangle rectangle = new Rectangle(topLeftPoint, new Size(width, height));
            // fill the area with light gray
            CvInvoke.Rectangle(src, rectangle, new MCvScalar(150, 152, 146), -1);
        }

        public static void DrawBackgroundBoxes(Mat src, List<TextBox> textBoxes)
        {
            for (int i = 0; i < textBoxes.Count; i++)
            {
                TextBox t = textBoxes[i];
                DrawBackgroundBox(src, t.Points);
            }
        }

        public static Mat WriteTextInBoxes(Mat src, List<TextBlock> textBlocks, bool translateText)
        {
            // List<TextBlock> textblocks = result.TextBlocks;
            // OcrUtils.DrawTextBoxes(textBoxPaddingImg, textBoxes, thickness);
            // OcrUtils.DrawBackgroundBoxes(src, textblocks);
            foreach (var block in textBlocks)
            {
                // paint the background of the rectangle
                src = WriteTextInBox(src, block.BoxPoints, block.Text,
                    block.TranslatedText, translateText);
            }
            return src;
        }

        public static Mat WriteTextInBox(Mat src, List<Point> box, string text,
            string translatedText, bool translateText)
        {
            // declare the font to write
            Font font = new Font("Tahoma", 24, GraphicsUnit.Point);
            
            // draw the background of the text
            DrawBackgroundBox(src, box);
            
            // define the width and height of the box for text
            int width = GetWidth(box);
            int height = GetHeight(box);
            Point topLeftPoint = GetTopLeftPoint(box);
            // topLeftPoint.Y += 3; // move the text down to make it look clearly
            
            // define the area of the rectangle
            Rectangle rectangle = new Rectangle(topLeftPoint, new Size(width, height));

            // define the string format
            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Center;
            strFormat.LineAlignment = StringAlignment.Center;

            // draw the text
            Image<Bgr, byte> srcImg = src.ToImage<Bgr, byte>();
            using (Graphics g = Graphics.FromImage(srcImg.AsBitmap()))
            {
                int alpha = 255; // from 0-255, 128 is 50% opacity
                int red = 220, green = 220, blue = 220;
                using (Brush brush = new SolidBrush(Color.FromArgb(alpha, red, green, blue)))
                {
                    if (translateText && translatedText != string.Empty)
                    {
                        drawTranslatedText(g, brush, font, translatedText, rectangle);
                    }
                    else
                    {
                        drawTranslatedText(g, brush, font, text, rectangle);
                    }
                }
            }
            return srcImg.Mat;
        }

        /**
         * Helper method to draw the rectangles and translated text on the image
         * If the text is too long, it will be resized horizontally
         */
        private static void drawTranslatedText(Graphics g, Brush brush, Font font, string translatedText, Rectangle rect)
        {
            // change the font size if it does not fit vertically
            font = changeTextFont(g, font, translatedText, rect);

            // improve the quality of the text rendering
            g.InterpolationMode = InterpolationMode.High;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // fill in the background of the rectangle
            g.FillRectangle(brush, rect);
            var textSize = g.MeasureString(translatedText, font);
            if (textSize.Width > rect.Width)
            {
                // if the text is too wide, scale it down horizontally
                var state = g.Save();
                g.TranslateTransform(rect.Left, rect.Top);

                // make the font smaller until it fits nicely
                while ((float)(rect.Width / textSize.Width) < 0.5)
                {
                    if (font.Size < 3)
                    {
                        font = new Font(font.FontFamily, 3, font.Style);
                        textSize = g.MeasureString(translatedText, font);
                        break;
                    }
                    font = new Font(font.FontFamily, font.Size - 1, font.Style);
                    textSize = g.MeasureString(translatedText, font);
                }
                // scale horizontally, keep the vertical size the same
                g.ScaleTransform((float)(rect.Width / textSize.Width), 1);
                Console.WriteLine("Ratio: {0}, text: {1}", (float)(rect.Width / textSize.Width), translatedText);
                g.DrawString(translatedText, font, Brushes.Black, PointF.Empty);
                g.Restore(state);
            }
            else
            {
                // if the text is smaller than the rectangle, write it
                g.DrawString(translatedText, font, Brushes.Black, rect);
            }
            System.GC.Collect();
        }

        /**
         * Change the text font size if it does not fit vertically
         */
        private static Font changeTextFont(Graphics g, Font font, string translatedText, Rectangle rect)
        {
            var textSize = g.MeasureString(translatedText, font);
            bool fitVertically = textSize.Height <= rect.Height;
            while (!fitVertically)
            {
                font = new Font(font.FontFamily, font.Size - 1, font.Style);
                textSize = g.MeasureString(translatedText, font);
                fitVertically = textSize.Height <= rect.Height;
            }
            System.GC.Collect();
            return font;
        }

        public static List<Mat> GetPartImages(Mat src, List<TextBox> textBoxes)
        {
            List<Mat> partImages = new List<Mat>();
            for (int i = 0; i < textBoxes.Count; ++i)
            {
                Mat partImg = GetRotateCropImage(src, textBoxes[i].Points);
                //Mat partImg = new Mat();
                //GetRoiFromBox(src, partImg, textBoxes[i].Points);
                partImages.Add(partImg);
            }
            return partImages;
        }

        public static Mat GetRotateCropImage(Mat src, List<Point> box)
        {
            Mat image = new Mat();
            src.CopyTo(image);
            List<Point> points = new List<Point>();
            points.AddRange(box);

            int[] collectX = { box[0].X, box[1].X, box[2].X, box[3].X };
            int[] collectY = { box[0].Y, box[1].Y, box[2].Y, box[3].Y };
            int left = collectX.Min();
            int right = collectX.Max();
            int top = collectY.Min();
            int bottom = collectY.Max();

            Rectangle rect = new Rectangle(left, top, right - left, bottom - top);
            Mat imgCrop = new Mat(image, rect);

            for (int i = 0; i < points.Count; i++)
            {
                var pt = points[i];
                pt.X -= left;
                pt.Y -= top;
                points[i] = pt;
            }

            int imgCropWidth = (int)(Math.Sqrt(Math.Pow(points[0].X - points[1].X, 2) +
                                        Math.Pow(points[0].Y - points[1].Y, 2)));
            int imgCropHeight = (int)(Math.Sqrt(Math.Pow(points[0].X - points[3].X, 2) +
                                         Math.Pow(points[0].Y - points[3].Y, 2)));

            var ptsDst0 = new PointF(0, 0);
            var ptsDst1 = new PointF(imgCropWidth, 0);
            var ptsDst2 = new PointF(imgCropWidth, imgCropHeight);
            var ptsDst3 = new PointF(0, imgCropHeight);

            PointF[] ptsDst = { ptsDst0, ptsDst1, ptsDst2, ptsDst3 };


            var ptsSrc0 = new PointF(points[0].X, points[0].Y);
            var ptsSrc1 = new PointF(points[1].X, points[1].Y);
            var ptsSrc2 = new PointF(points[2].X, points[2].Y);
            var ptsSrc3 = new PointF(points[3].X, points[3].Y);

            PointF[] ptsSrc = { ptsSrc0, ptsSrc1, ptsSrc2, ptsSrc3 };

            Mat M = CvInvoke.GetPerspectiveTransform(ptsSrc, ptsDst);

            Mat partImg = new Mat();
            CvInvoke.WarpPerspective(imgCrop, partImg, M,
                                new Size(imgCropWidth, imgCropHeight), Inter.Nearest, Warp.Default,
                               BorderType.Replicate);

            if (partImg.Rows >= partImg.Cols * 1.5)
            {
                Mat srcCopy = new Mat();
                CvInvoke.Transpose(partImg, srcCopy);
                CvInvoke.Flip(srcCopy, srcCopy, 0);
                return srcCopy;
            }
            else
            {
                return partImg;
            }
        }

        public static Mat MatRotateClockWise180(Mat src)
        {
            CvInvoke.Flip(src, src, FlipType.Vertical);
            CvInvoke.Flip(src, src, FlipType.Horizontal);
            return src;
        }

        public static Mat MatRotateClockWise90(Mat src)
        {
            CvInvoke.Rotate(src, src, RotateFlags.Rotate90CounterClockwise);
            return src;
        }

    }
}

