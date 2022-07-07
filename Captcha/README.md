# Captcha class

## Goal
Attempt to solve captcha required to login automatically.

## Process
* Pre-processing: Can be found in `Captcha.Util.cs`.
    * Attempt to convert colored captchas into black & white captchas
    * Multiple methods, some of the methods were found [here](https://github.com/livezingy/CaptchaProcess/tree/master/CaptchaProcess/CaptchaProcess)
    * Original Image:<br> ![Alt text](./sampleImages/before-5585.png)
    * Processed Image:<br> ![Alt text](./sampleImages/after-5485.png)
* Text extractor: Can be found in `Captcha.CaptchaSolver.cs`
    * Utilizes Tesseract-OCR 4.01 to "read" the numeric values from the image
        * Utilizes LSTM model. The overall size is much lighter.
    * Can be trained on the captcha for better performance better.
* How I test: Can be found in `Captcha.CaptchaTest.cs`
    * Description: Compare each pre-processing methods (including no method) against each other to see which one would make the text extractor (Tesseract) the most accurate.
    * Here are the steps below:
        * Firstly, 100 captcha images are generated (value can be changed). The name of the image needs to be the image text.
        * Secondly, manually check all of the images' name for correctness, and make changes if necessary.
        * Thirdly, run the `check_correctness()`, and it will output the result (similar to below).

## Results
Normal process:
```
Total number of captchas: 100 captchas
Without image processing: 66% correct
Primitive Gray Bitmap 1: 71% correct
Primitive Gray Bitmap 2: 72% correct
Sauvola binarization: 72% correct
Otsu binarization: 70% correct
Iterative binarization: 67% correct
Zhang-Suen skelenton: 50% correct
Combination of without image processing and Sauvola: 69% correct
```

Enlarges the images by x2
```
Total number of captchas: 100 captchas
Without image processing: 7% correct
Primitive Gray Bitmap 1: 10% correct
Primitive Gray Bitmap 2: 8% correct
Sauvola binarization: 6% correct
Otsu binarization: 7% correct
Iterative binarization: 8% correct
Zhang-Suen skelenton: 46% correct
```