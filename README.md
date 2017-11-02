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
 
 ##Setup Steps
-------------------
###Step 1:  Create Google Drive API Key

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
 
 
###Step 2:  Setup Google Drive Project Folder

> - Go to [Google Drive](https://drive.google.com/drive/my-drive) -  - If asked to sign in, make sure you use the same google account you used to generate the api key.

> - Create a new folder in Google drive for your updater. You can call it anything.
 
> - Upload the contents of your client folder into the Google drive folder you created. The resulting folder should look like this:
  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ![](http://www.ascensiongamedev.com/resources/filehost/fc44a8ed7cd735e3b7dcc5be30c2621a.png)
 
> - Once your client files are uploaded, right click on the folder name that contains your Client.exe and Get a Shareable Link
 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ![](http://www.ascensiongamedev.com/resources/filehost/129d3cc59ec274576ff62b09b1d7a1c5.png)
 
> - Google Drive will tell you that link sharing is ON, this is a good thing.
 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ![](http://www.ascensiongamedev.com/resources/filehost/e785ba5c77e70d369d968975ec62a8be.png)
 
> - Finally, while you are in the same folder as your Client.exe, look at your browser URL, it should look something like this:
 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ![](http://www.ascensiongamedev.com/resources/filehost/623f65a8606f38c5219d301700cb2f05.png)
 
> - Click "APIs and Services" in the navigation menu on the left
 
 https://drive.google.com/drive/folders/BBBBBBBBBBBBBBBBBBBBBBBBBBBBB
  
 We need to get the folder id for our updater, it is the long list of characters after /folders/. So my folder id is:

 BBBBBBBBBBBBBBBBBBBBBBBBBBBBB
 
 > - Save your folder id somewhere safe, we will need it in the final step.
 
 
###Step 3:  Configuring the Updater

> - Download compile the updater from source, or download the updater on our releases page.

> - Extract the Updater in an empty folder on your PC. Your folder should look like this:

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ![](http://www.ascensiongamedev.com/resources/filehost/aff82e9c8cb2932c3ac1ee119d2186f0.png)
 
> - Open settings.json, in the json, put your folder id (from step 2)  and api key (from step 1) inside the empty quotes.  When you're done, your settings.json should look like this:
  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ![](http://www.ascensiongamedev.com/resources/filehost/e960004f6772c251d72dc64150fdd28e.png)
 
> - Save the settings.json file after you enter your api key and folder id.


###Step 4:  Test the Updater!

> - Run Updater.exe and it should start downloading everything in your Google Drive folder. Sometimes there is a delay between the updater seeing changes, so if no files are downloaded give it 10 minutes and try again. Once the update it done the updater will automatically launch Intersect Client.exe, or whatever application is listed in the settings.json file.


###tep ???:  Releasing Future Update

> - To release updates, just go to your Google drive folder and add/remove/replace files. The updater will do the rest!


 


