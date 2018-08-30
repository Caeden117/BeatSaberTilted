# Tilted - A Beat Saber Plugin
A plugin that fucks up the scene when you fuck up. *This plugin may crash the game when exiting the level.*

# Setup
Add the release .dll to `<Beat Saber Directory>/Plugins` and start the game. A configuration file will automatically be created in `<Beat Saber Directory>/UserData/Tilted.txt`.

# Configuration
`enabled` *(Default: true)* | Toggles the plugin on/off.

`scalar` *(Default: 10)* |  A float value of how fucked up the scene will get every time the function gets called. Negative numbers will work!

`avoidCameras` *(Default: true)* | **MIGHT NOT WORK!** Avoids GameObjects with Cameras tied to them. This ensures any outside Cameras (CameraPlus first/third person, Dynamic Camera, etc.) do not get messed with. ***Ignored when `avoidFilters` is enabled.***

`avoidFiltersBecauseYouAreAFuckingMadManAndItWillMakeBeatSaberUnplayable` *(Default: false)* | Toggles the built-in filters that make the plugin actually playable. ***BY ENABLING THIS OPTION, BEAT SABER WILL BECOME UNPLAYABLE AND MOST LIKELY CRASH WHEN EXITING A LEVEL.***

# Advanced Details
By default, the plugin will ignore any GameObject with the following components added on to it:

```
NoteData (Generic notes)
NoteController (What spawns the notes)
Sabers (What you control and swing)
ObstacleData (Obstacles that you dodge)
ObstacleControllers (What spawns the obstacles)
Cameras (Although it has its own configuration setting, the avoidCameras config will be ignored if the avoidFilters option is enabled)
```

This ensures that while the rest of the scene will be messed up, Beat Saber will still be playable and you will still be able to complete the level (With a chance of having your view cut off by the level itself)

When enabling the `avoidFilters` configuration option, the plugin will skip this check, causing obstacles to spawn weirdly, notes to be rotated randomly. I believe it will also make sabers stop following your controllers, so it makes the entire level unbeatable.