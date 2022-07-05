# Captcha class

## Goal
Attempt to solve captcha required to login automatically.

## Process
* Pre-processing: Can be found in `Captcha.Util.cs`.
    * Attempt to convert colored captchas into black & white captchas
    * Multiple methods, some of the methods were found [here](https://github.com/livezingy/CaptchaProcess/tree/master/CaptchaProcess/CaptchaProcess)
* Text extractor: Can be found in `Captcha.CaptchaSolver.cs`
    * Utilizes Tesseract-OCR 3.02 to "read" the numeric values from the image
    * Although there is higher version (5.0+), it uses LSTM model
        * Cons: Data is too big - 500MB - and is slower so I did not use it.
        * Pros: its accuracy would definitely be improved.
        * Explanation: we do not care about accuracy when it is above 70% since we could try again. No need to get 100% accuracy.
* How I test: Can be found in `Captcha.CaptchaTest.cs`
    * Description: Compare each pre-processing methods (including no method) against each other to see which one would make the text extractor (Tesseract) the most accurate.
    * Here are the steps below:
        * Firstly, 100 captcha images are generated (value can be changed). The name of the image needs to be the image text.
        * Secondly, manually check all of the images' name for correctness, and make changes if necessary.
        * Thirdly, run the `check_correctness()`, and it will output the result (similar to below).

## Results
Normal process:<br>
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
Without image processing: number of correct: 7, percent: 7%
Primitive Gray Bitmap 1: number of correct: 11, percent: 11%
Primitive Gray Bitmap 2: number of correct: 9, percent: 9%
Sauvola binarization: number of correct: 6, percent: 6%
Otsu binarization: number of correct: 9, percent: 9%
Iterative binarization: number of correct: 10, percent: 10%
Zhang-Suen skelenton: number of correct: 47, percent: 47%
```