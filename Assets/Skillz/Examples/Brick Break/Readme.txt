******************************************************************************
****************************** Skillz Unity SDK ******************************
******************************************************************************

Documentation: https://docs.skillz.com/

Portal: https://developers.skillz.com/


--------------------------- Included Examples --------------------------------


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
