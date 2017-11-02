# Simple Updater (Google Drive Powered)
Designed for Intersect ORPGs, easily distribute game updates to players using your personal Google Drive account.

This is a very basic game updater/launcher for Intersect games and all kinds of projects in general. This updater is open source so you're free to modify it to your liking. This updater is powered by Google drive, you create a google drive folder with your game client. When you add/remove/change files in your google drive folder, the updater will make those same changes for all of your players.


Updater Pros & Cons
-------------------

**Pros:**
 - Easy to setup.
 - Free.
 - Open source.
 - Very easy to release updates.
 - Only downloads changed files.
 - Can resume updates that fail. (or if the program is restarted)
 
**Cons:**
 - Limited customization.
 - Windows only.
 - Several setup steps, but easy if you can follow directions.
 - Slower than other updating methods.
 - Likely will not be developed further!
 
 Setup Steps
-------------------
> **Step 1:  Create Google Drive API Key**

> - Visit [Google Cloud Projects Console](https://console.cloud.google.com/) - Sign in with your Google Account
> - Click on "Select a Project" at the top of the page.
  
 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;![](http://www.ascensiongamedev.com/resources/filehost/988f78c82c0acd78e8247651933cf277.png) 
 
> - Click the "+" (Create Project) button in the popup window
  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ![](http://www.ascensiongamedev.com/resources/filehost/ac94d332637e0d837b9276ef0c2911c4.png)
 
> - Give your project a name. I recommend "GameName-Updater" where you replace GameName with the name of your game. After naming your project, click "Create"
 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ![](http://www.ascensiongamedev.com/resources/filehost/ebd9b2ec314bb03b0ea58fa49ace6240.png)
 
> - Give Google a minute or two to create the project.
 
> - Click "Select Project" at the top of your Window again
 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ![](http://www.ascensiongamedev.com/resources/filehost/988f78c82c0acd78e8247651933cf277.png)
 
> - Click into your new project
 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ![](http://www.ascensiongamedev.com/resources/filehost/623f65a8606f38c5219d301700cb2f05.png)
 
> - Click "APIs and Services" in the navigation menu on the left
 
 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;![](http://www.ascensiongamedev.com/resources/filehost/eda659c3f30038f02a5d1d979b106796.png)
 
> - Click "Enable APIs and Services" at the top of the new page
 
 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;![](http://www.ascensiongamedev.com/resources/filehost/f9593b713ef08b87734e8979bb68223a.png) 
 
> - Find and click on the Google Drive API Tile
 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ![](http://www.ascensiongamedev.com/resources/filehost/adb3e7292b80515cd75e77d7f8ec6b2a.png)
 
> - Click the big blue enable button
 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ![](http://www.ascensiongamedev.com/resources/filehost/df0ec8db7fcc1a9e96e87bf4ae64948f.png)
 
> - Once the Google Drive API page loads, click on "Credentials" in the navigation on the left
 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ![](http://www.ascensiongamedev.com/resources/filehost/7f73336f44ecf0213e4ca5602153eaa8.png)
 
> - On the credentials page, select the create credentials button, and then the API Key option 
 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ![](http://www.ascensiongamedev.com/resources/filehost/0d91af9df12b71d0f6219a9159a4afc1.png)
 
> - Copy the API Key somewhere safe, and close this browser window
 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ![](http://www.ascensiongamedev.com/resources/filehost/ed09f28e688147e248efce82ad58ae10.png)
 
