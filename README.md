# Launcher for Game

## Purpose:
My childhood game requires multiple steps to login and load the game. Moreover, their website is very slow and would take ages to load.<br>

I created this application to login with a click of button.

## Features
### **Login Window**
* Load login credentials from .env file
* Solve Captcha Automatically (86% correct)
* Check for the newest server number to validate user input

### **Game Window**
* Custom TitleBar
* Mute/unmute application
* Check for the newest server number to validate user input

## Captcha Stats
### **Average time taken to load up the form** (load server, get captcha, solve captcha, login credentials)
* load server (thread); get captcha (thread); solve captcha (none); login credentials (thread): 2.9 - 4 seconds
* load server (thread); get captcha (thread); solve captcha (async); login credentials (thread): 3.4 - 3.8 seconds
* load server (async); get captcha (thread); solve captcha (async); login credentials (thread): 1.1 - 1.7 seconds
* load server (async); get captcha (thread); solve captcha (none); login credentials (thread): 1.1 - 1.5 seconds
* load server (async); get captcha (async); solve captcha (async); login credentials (none): 0.4 - 0.6 seconds
* load server (async); get captcha (async); solve captcha (async); login credentials (thread): ~0.3 seconds **<= best**

## Image Translator
### Current process:
* IronOCR-Chinese to detect the Chinese characters
* Google Translate API (free) to translate it to English
* Detecting text block ?! (https://devindeep.com/text-detection-with-c/)
    * https://devindeep.com/text-detection-with-c/
    * https://www.youtube.com/watch?v=KHes5M7zpGg

### Some other failed processes
* Capture2Text (extracting text)
    * Inspired by https://github.com/phatjkk/DragonTranslator
    * http://capture2text.sourceforge.net/#text_line_capture
    * `./Capture2Text_CLI.exe -l "Chinese - Simplified" --trim-capture --line-breaks -i "testImg/sample.PNG" -o capture2text_result.txt`
    * The result is not very good. Many Chinese characters were not recognized.
    * See `ImageTranslator/sample/capture2text_result.txt` for the sample result.

* 

## Current Images
* Gif of the current application:<br><br>
![](./images/sampleVid.gif)

* Some sample images:
    * Loaded login screen:<br><br>
    ![](./images/login.png)

    * Loading game screen:<br><br>
    ![](./images/loading_game.png)