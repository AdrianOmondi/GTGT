******************************************************************************
****************************** Skillz Unity SDK ******************************
******************************************************************************

Documentation: https://docs.skillz.com/

Upgrade Information: https://docs.skillz.com/docs/upgrade-sdk

Developer Portal: https://developers.skillz.com/


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
4. Follow the build instructions for your SDK version and desired platform at:
   https://docs.skillz.com/docs/installing-skillz-unity