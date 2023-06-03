using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.OnnxRuntime;

namespace Captcha
{
    public class CaptchaSolver : IDisposable
    {
        private InferenceSession session;

        public CaptchaSolver(string modelPath)
        {
            this.session = new InferenceSession(modelPath);
        }

        public void Dispose()
        {
            session.Dispose();
        }

        public string SolveCaptcha(string path)
        {
            DenseTensor<float> inputTensor = PreprocessImage(path);

            var results = RunModel(inputTensor);

            var maxIndices = PostprocessResults(results);

            return IndexToString(maxIndices);
        }

        public string SolveCaptcha(byte[] imageData)
        {
            DenseTensor<float> inputTensor = PreprocessImage(imageData);

            var results = RunModel(inputTensor);

            var maxIndices = PostprocessResults(results);

            return IndexToString(maxIndices);
        }

        /**
         * The input of the model is a tensor of shape (1, 3, 22, 54).
         * 1 is the batch size, 3 is a random number, 22 is the height of the image, and 54 is the width of the image.
         */
        private DenseTensor<float> PreprocessImage(string path)
        {
            // Open file
            using var img = SixLabors.ImageSharp.Image.Load<Rgb24>(path);
            // Resize to (22, 54)
            img.Mutate(x =>
            {
                x.Resize(new ResizeOptions
                {
                    Size = new SixLabors.ImageSharp.Size(54, 22),
                    Mode = ResizeMode.Crop // Crop the image to match the target size
                });
            });

            // Change to float for normalization
            var imageArray = new DenseTensor<float>(new[] { 1, 3, 22, 54 });

            for (int c = 0; c < 3; c++)
            {
                for (int y = 0; y < 22; y++)
                {
                    for (int x = 0; x < 54; x++)
                    {
                        // Normalize and fill tensor
                        Rgb24 pixel = img[x, y];
                        if (c == 0)
                            imageArray[0, c, y, x] = pixel.R / 255.0f;
                        else if (c == 1)
                            imageArray[0, c, y, x] = pixel.G / 255.0f;
                        else if (c == 2)
                            imageArray[0, c, y, x] = pixel.B / 255.0f;
                    }
                }
            }

            // Normalize the image using the mean and standard deviation (from the python code)
            var mean = new DenseTensor<float>(new[] { 3 });
            mean[0] = 0.485f;
            mean[1] = 0.456f;
            mean[2] = 0.406f;

            var stdDev = new DenseTensor<float>(new[] { 3 });
            stdDev[0] = 0.229f;
            stdDev[1] = 0.224f;
            stdDev[2] = 0.225f;

            for (int c = 0; c < 3; c++)
            {
                for (int y = 0; y < 22; y++)
                {
                    for (int x = 0; x < 54; x++)
                    {
                        // Subtract mean and divide by standard deviation
                        imageArray[0, c, y, x] = (imageArray[0, c, y, x] - mean[c]) / stdDev[c];
                    }
                }
            }

            return imageArray;
        }

        /**
         * The input of the model is a tensor of shape (1, 3, 22, 54).
         * 1 is the batch size, 3 is a random number, 22 is the height of the image, and 54 is the width of the image.
         */
        private DenseTensor<float> PreprocessImage(byte[] imageData)
        {
            // Open file
            using var img = SixLabors.ImageSharp.Image.Load<Rgb24>(imageData);
            // Resize to (22, 54)
            img.Mutate(x =>
            {
                x.Resize(new ResizeOptions
                {
                    Size = new SixLabors.ImageSharp.Size(54, 22),
                    Mode = ResizeMode.Crop // Crop the image to match the target size
                });
            });

            // Change to float for normalization
            var imageArray = new DenseTensor<float>(new[] { 1, 3, 22, 54 });

            for (int c = 0; c < 3; c++)
            {
                for (int y = 0; y < 22; y++)
                {
                    for (int x = 0; x < 54; x++)
                    {
                        // Normalize and fill tensor
                        Rgb24 pixel = img[x, y];
                        if (c == 0)
                            imageArray[0, c, y, x] = pixel.R / 255.0f;
                        else if (c == 1)
                            imageArray[0, c, y, x] = pixel.G / 255.0f;
                        else if (c == 2)
                            imageArray[0, c, y, x] = pixel.B / 255.0f;
                    }
                }
            }

            // Normalize the image using the mean and standard deviation (from the python code)
            var mean = new DenseTensor<float>(new[] { 3 });
            mean[0] = 0.485f;
            mean[1] = 0.456f;
            mean[2] = 0.406f;

            var stdDev = new DenseTensor<float>(new[] { 3 });
            stdDev[0] = 0.229f;
            stdDev[1] = 0.224f;
            stdDev[2] = 0.225f;

            for (int c = 0; c < 3; c++)
            {
                for (int y = 0; y < 22; y++)
                {
                    for (int x = 0; x < 54; x++)
                    {
                        // Subtract mean and divide by standard deviation
                        imageArray[0, c, y, x] = (imageArray[0, c, y, x] - mean[c]) / stdDev[c];
                    }
                }
            }

            return imageArray;
        }

        private string IndexToString(int[] list)
        {
            string result = "";
            foreach (int i in list)
            {
                if (i < 10)
                {
                    result += (char)(i + '0');
                }
                else if (i < 36)
                {
                    result += (char)(i + 'a' - 10);
                }
                else
                {
                    result += (char)(i + 'A' - 36);
                }
            }
            return result;
        }

        public IEnumerable<DisposableNamedOnnxValue> RunModel(DenseTensor<float> inputTensor)
        {
            // Prepare the input data.
            var inputs = new NamedOnnxValue[] { NamedOnnxValue.CreateFromTensor("actual_input", inputTensor) };

            // Run the model.
            var resultCollection = session.Run(inputs);
            return resultCollection;
        }

        public int[] PostprocessResults(IEnumerable<DisposableNamedOnnxValue> resultCollection)
        {
            int[] maxIndices = new int[4];
            var results = resultCollection.ToList();

            // Get the first result tensor.
            var resultTensor = results[0].AsTensor<float>();

            // Find the index of the maximum value along the last dimension.
            for (int i = 0; i < 4; i++)
            {
                float maxVal = float.MinValue;
                for (int j = 0; j < 36; j++)
                {
                    if (resultTensor[0, i, j] > maxVal)
                    {
                        maxVal = resultTensor[0, i, j];
                        maxIndices[i] = j;
                    }
                }
            }

            // Dispose all DisposableNamedOnnxValue in the list manually
            foreach (var result in results)
            {
                result.Dispose();
            }

            return maxIndices;
        }
    }

}
