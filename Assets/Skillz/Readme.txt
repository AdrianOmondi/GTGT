******************************************************************************
****************************** Skillz Unity SDK ******************************
******************************************************************************

Documentation: https://docs.skillz.com/

Upgrade Information: https://docs.skillz.com/docs/upgrade-sdk

Portal: https://developers.skillz.com/


--------------------------- Included Examples --------------------------------

-----Basic Example Setup-----

Location: Assets/Skillz/Examples/Basic Example

---To Run in Unity Editor---
1. File->Build Settings
2. Add the 'StartMenu' and 'Game' scenes to the 'Scenes in Build'.
(These can be found at 'Assets/Skillz/Examples/Basic Example/Scenes')
3. Make sure the StartMenu scene is at index 0.
4. From the menu bar Go to 'Skillz->Settings', open the SIDEkick section and
Click 'Launch Game'.
5. If 'Import TMP Essentials' window appears, select Import.

note: This will only use the 'Unity Companion'. This means that Skillz API
      calls will NOT hit skillz servers. For some API calls to work as expected
      you must build the game to either IOS or Android platforms.


---To Run in IOS/Android---
1. File->Build Settings
2. Add the 'StartMenu' and 'Game' scenes to the 'Scenes in Build'.
3.  Make sure the StartMenu scene is at index 0.
4. Follow the build instructions for your SDK version at:
   https://docs.skillz.com/docs/installing-skillz-unity



-----Brick Breaker Example-----

Location: Assets/Skillz/Examples/Brick Break

---To Run in Unity Editor---
1. File->Build Settings
2. Add the following scenes into the 'Scenes in Build' in the build settings:
   (These can be found in the 'Assets/Skillz/Examples/Brick Break/Scenes' folder)
     'BB_StartMenu'
     'BB_Game'
     'BB_ProgressionRoom'
2a. Make sure the 'BB_StartMenu' scene is at index 0
3. Go to Skillz->Settings
4. Under the SIDEkick section add the following SIDEkick templates to the sidekick settings
   (These can be found in the 'Assets/Skillz/Examples/Brick Break/SIDEkick' folder)
     'BB Match Types'
     'BB Match Parameters'
     'BB Players'
     'BB Progression Responses'
     'BB Seasons'
5. Click the 'Launch Game' button in the SIDEkick settings
6. If 'Import TMP Essentials' window appears, select Import.

note: This will only use the 'Skillz SIDEkick'. This means that Skillz API
      calls will NOT hit skillz servers. For some API calls to work as expected
      you must build the game to either IOS or Android platforms.

---To Run in IOS/Android---
1. File->Build Settings
2. Add the following scenes into the 'Scenes in Build' in the build settings:
     'BB_StartMenu'
     'BB_Game'
     'BB_ProgressionRoom'
2a. Make sure the 'BB_StartMenu' scene is at index 0
3. Go to Skillz Settings and set the Skillz Orientation to 'Portrait'
4. Follow instructions for setting up a game at https://developers.skillz.com/
5. Follow the build instructions for setting up the example project at
https://docs.skillz.com/



---------------------------- Included Prefabs ---------------------------------

Location: Assets/Skillz/Prefabs

note: If included prefabs/scripts are used, it is recommended to make a copy
outside the skillz folder, as the prefabs/scripts can be changed or moved
during a future upgrade to the skillz SDK.


---To Use Skillz Manager---
1. Add Skillz Manager to your start menu scene.
2. Add scenes and/or additional logic in the available fields
3. Call LaunchSkillz() when the start/play button is pressed


---To Use Example Match Manager---
1. Add the skillz Game Manager to the game scene.

note: The 'Example Game Scene Manager' can be used to get up and running, but
is it recommened to create your own game manager using this as a boilerplate.


---To Use Skillz Debug Canvas---
1. Add the 'Skillz Debug Canvas' to the game scene or progression room scene.
2. Ensure the 'Skillz Debug Canvas' is the last item at the top level of your 
hierarchy.