using Emgu.CV;
using Emgu.CV.CvEnum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using GTranslate.Translators;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace OcrLiteLib
{
    /**
     * A class based on Emgu.CV and Onnx pre-trained model to recognize
     * Chinese characters in an image.
     * https://github.com/DayBreak-u/chineseocr_lite/tree/onnx/dotnet_projects/OcrLiteOnnxCs
     **/
    public class OcrLite
    {
        public bool isPartImg { get; set; }
        public bool isDebugImg { get; set; }
        private string dbNetPath, angleNetPath, crnnNetPath, keysPath;
        private DbNet dbNet;
        private AngleNet angleNet;
        private CrnnNet crnnNet;

        public OcrLite()
        {
            dbNet = new DbNet();
            angleNet = new AngleNet();
            crnnNet = new CrnnNet();
        }

        public void InitModels(string detPath,string clsPath,string recPath,string keysPath,int numThread)
        {
            try
            {
                dbNet.InitModel(detPath,numThread);
                angleNet.InitModel(clsPath, numThread);
                crnnNet.InitModel(recPath, keysPath, numThread);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        public async Task<OcrResult> Detect(Mat brgSrc, int padding, int imgResize, float boxScoreThresh, float boxThresh,
                              float unClipRatio, bool doAngle, bool mostAngle, bool extractText,
                              bool translateText, AggregateTranslator translator)
        {
            // Mat brgSrc = CvInvoke.Imread(img, ImreadModes.Color);//default : BGR
            Mat originSrc = new Mat();
            CvInvoke.CvtColor(brgSrc, originSrc, ColorConversion.Bgr2Rgb);// convert to RGB
            Rectangle originRect = new Rectangle(padding, padding, originSrc.Cols, originSrc.Rows);
            Mat paddingSrc = OcrUtils.MakePadding(originSrc, padding);

            int resize;
            if (imgResize <= 0)
            {
                resize = Math.Max(paddingSrc.Cols, paddingSrc.Rows);
            }
            else
            {
                resize = imgResize;
            }
            ScaleParam scale = ScaleParam.GetScaleParam(paddingSrc, resize);

            OcrResult ocrResult =  await Task.Run(() => DetectOnce(
                paddingSrc, originRect, scale, boxScoreThresh, boxThresh, unClipRatio, doAngle,
                mostAngle, extractText).Result);
            
            if (translateText && translator != null)
                await TranslateText(ocrResult, translator);

            // translating text or extracting text
            if (translateText && translator != null)
            {
                ocrResult.BoxImg = await Task.Run(() => OcrUtils.WriteTextInBoxes(ocrResult.BoxImg, ocrResult.TextBlocks, translateText));
            }
            
            CropImageToOriginalSize(ocrResult);
            return ocrResult;
        }

        private async Task<OcrResult> DetectOnce(Mat src, Rectangle originRect, ScaleParam scale, float boxScoreThresh,
            float boxThresh, float unClipRatio, bool doAngle, bool mostAngle, bool extractText)
        {
            Mat textBoxPaddingImg = src.Clone();
            int thickness = OcrUtils.GetThickness(src);
            Console.WriteLine("=====Start detect=====");
            var startTicks = DateTime.Now.Ticks;

            Console.WriteLine("---------- step: dbNet getTextBoxes ----------");
            var textBoxes = dbNet.GetTextBoxes(src, scale, boxScoreThresh, boxThresh, unClipRatio);
            var dbNetTime = (DateTime.Now.Ticks - startTicks) / 10000F;

            Console.WriteLine($"TextBoxesSize({textBoxes.Count})");
            textBoxes.ForEach(x => Console.WriteLine(x));
            //Console.WriteLine($"dbNetTime({dbNetTime}ms)");

            //---------- getPartImages ----------
            List<Mat> partImages = OcrUtils.GetPartImages(src, textBoxes);
            if (isPartImg)
            {
                Console.WriteLine("---------- step: isPartImg ----------");
                for (int i = 0; i < partImages.Count; i++)
                {
                    CvInvoke.Imshow($"PartImg({i})", partImages[i]);
                }
            }

            Console.WriteLine("---------- step: angleNet getAngles ----------");
            List<Angle> angles = angleNet.GetAngles(partImages, doAngle, mostAngle);
            //angles.ForEach(x => Console.WriteLine(x));

            //Rotate partImgs
            for (int i = 0; i < partImages.Count; ++i)
            {
                if (angles[i].Index == 0)
                {
                    partImages[i] = OcrUtils.MatRotateClockWise180(partImages[i]);
                }
                if (isDebugImg)
                {
                    CvInvoke.Imshow($"DebugImg({i})", partImages[i]);
                }
            }

            Console.WriteLine("---------- step: crnnNet getTextLines ----------");
            List<TextLine> textLines = crnnNet.GetTextLines(partImages);
            //textLines.ForEach(x => Console.WriteLine(x));

            List<TextBlock> textBlocks = new List<TextBlock>();
            for (int i = 0; i < textLines.Count; ++i)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.BoxPoints = textBoxes[i].Points;
                textBlock.BoxScore = textBoxes[i].Score;
                textBlock.AngleIndex = angles[i].Index;
                textBlock.AngleScore = angles[i].Score;
                textBlock.AngleTime = angles[i].Time;
                textBlock.Text = textLines[i].Text;
                textBlock.CharScores = textLines[i].CharScores;
                textBlock.CrnnTime = textLines[i].Time;
                textBlock.BlockTime = angles[i].Time + textLines[i].Time;
                textBlocks.Add(textBlock);
            }
            
            //textBlocks.ForEach(x => Console.WriteLine(x));

            // DO WORK HERE BEFORE SIZE IS CHANGED

            // draw the textboxes
            OcrUtils.DrawTextBoxes(textBoxPaddingImg, textBoxes, thickness);
            // translating text or extracting text
            if (extractText)
            {
                textBoxPaddingImg = await Task.Run(() => OcrUtils.WriteTextInBoxes(textBoxPaddingImg, textBlocks, false));
            }

            var endTicks = DateTime.Now.Ticks;
            var fullDetectTime = (endTicks - startTicks) / 10000F;
            
            
            StringBuilder strRes = new StringBuilder();
            textBlocks.ForEach(x => strRes.AppendLine(x.Text));

            OcrResult ocrResult = new OcrResult();
            ocrResult.TextBlocks = textBlocks;
            ocrResult.DbNetTime = dbNetTime;
            // ocrResult.BoxImg = boxImg;
            ocrResult.BoxImg = textBoxPaddingImg;
            ocrResult.OriginalRect = originRect; // keep this to crop this later
            ocrResult.DetectTime = fullDetectTime;
            ocrResult.StrRes = strRes.ToString();
            
            System.GC.Collect(); // clean unwanted memory
            return ocrResult;
        }

        /**
         * A static method to crop the image to original size
         */
        public static void CropImageToOriginalSize(OcrResult ocrResult)
        {
            //cropped to original size
            Mat rgbBoxImg = new Mat(ocrResult.BoxImg, ocrResult.OriginalRect);
            Mat boxImg = new Mat();
            CvInvoke.CvtColor(rgbBoxImg, boxImg, ColorConversion.Rgb2Bgr);//convert to BGR for Output Result Img

            ocrResult.BoxImg = boxImg;
            System.GC.Collect(); // clean unwanted memory
        }

        /**
         * A static method to combine all of the text in TextBlocks into one string
         * for bulk translation (can separate into 2 or more due to characters contrainst)
         * Add a delimiter between each text block
         */
        public static List<string> GetRawTextFromTextBlocks(List<TextBlock> textBlocks, string separator, int maxStrSize=1000)
        {
            List<string> result = new List<string>();
            string combinedText = "";

            for (int i = 0; i < textBlocks.Count; i++)
            {
                TextBlock textBlock = textBlocks[i];
                combinedText += textBlock.Text;
                if (combinedText.ToCharArray().Length > maxStrSize - separator.Length)
                {
                    result.Add(combinedText);
                    combinedText = "";
                }
                else if (textBlock != textBlocks.Last())
                {
                    combinedText += separator;
                }
            }
            result.Add(combinedText);

            return result;
        }

        /**
         * Combine text in a 
         */
        public static async Task TranslateText(OcrResult ocrResult, AggregateTranslator translator, string separator = "\n\n")
        {
            // combine text in TextBlocks into one (or more) string
            List<string> rawTexts = GetRawTextFromTextBlocks(ocrResult.TextBlocks, separator);
            List<string> translatedText = new List<string>();

            foreach (string rawText in rawTexts)
            {
                var result = await translator.TranslateAsync(rawText, "en");
                string translatedTextStr = result.Translation;

                List<string> resultList = Regex.Split(translatedTextStr, @separator).ToList();
                // List<string> resultList = result.Translation.Split(separator.ToCharArray()).ToList();
                translatedText.AddRange(resultList);
            }

            // change the value of TranslatedText in TextBlocks
            for (int i = 0; i < translatedText.Count; i++)
            {
                ocrResult.TextBlocks[i].TranslatedText = translatedText[i];
            }
        }
    }
}
