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

## Some Stats
### **Average time taken to load up the form** (load server, get captcha, solve captcha, login credentials)
* load server (thread); get captcha (thread); solve captcha (none); login credentials (thread): 2.9 - 4 seconds
* load server (thread); get captcha (thread); solve captcha (async); login credentials (thread): 3.4 - 3.8 seconds
* load server (async); get captcha (thread); solve captcha (async); login credentials (thread): 1.1 - 1.7 seconds
* load server (async); get captcha (thread); solve captcha (none); login credentials (thread): 1.1 - 1.5 seconds
* load server (async); get captcha (async); solve captcha (async); login credentials (none): 0.4 - 0.6 seconds
* load server (async); get captcha (async); solve captcha (async); login credentials (thread): ~0.3 seconds **<= best**