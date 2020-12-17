# Skype-Auto-Replies
Simple console application that is configured to automatically reply to new incoming Skype conversations.

Due to increasing workload and the fact that I am contactable by various stakeholders both inside and outside of my department, I decided to explore the possibility of setting up an auto-responder that would be configurable based on custom groups I set up within Skype for business.

My initial use case was wanting anyone who I had not designated to a custom group that represented my close team would receive an instant reply asking them to contact a mailing list if their query was regarding support for a particular product otherwise I would reply when I could.

# Installation Instructions
I absolutely would like to work on some kind of installer that will install everything as necessary without having to go through the hoops of messing about with building the code. For now the below is required...

## 1. Download and install the Lync 2013 SDK here: https://www.microsoft.com/en-gb/download/details.aspx?id=36824
After downloading the file the installer will look for an older (possible 2013?) version of Skype for Business but if you open the .msi file with a tool such as 7-Zip you will find a 64-bit and 32-bit installer for the SDK. Either will work for this.

## 2. Clone this repo and alter the dependencies of the project to reference your copy of Microsoft.Lync.Model.dll.
Currently only this DLL is required.

## 3. Build the project and copy AutoReply.ini into the same directory as the resultant binaries.
Currently the code is hardcoded to look for AutoReply.ini in the same folder as the executable.

## 4. Ensure Skype For Business is already running and launch the executable!
The console application hooks into the already running Client so it must be running before you launch or it will error! 
