# Tunic Randomizer
An extensive randomizer mod for TUNIC, the action adventure game about a tiny fox in a big world.

For questions, feedback, or discussion related to the randomizer, please visit the dedicated randomizer channel in the [Tunic Speedrunning Discord](https://discord.gg/HXkztJgQWj)! 

## Features
- Randomizes every item in the game, including:
  - All Chests
  - Instruction Manual Pages
  - Fairies
  - Item Pickups (Sword, Keys, etc.)
  - Hero Relics
  - Unique items in the shop (Flasks and Coins)
  - Trinket card slots from tossing coins into wells
  - A (customizable) number of fool traps replacing less valuable money rewards
- Custom seed support
- Randomized Fox colors upon every scene transition
- Custom Fox Color texture support
- Spoiler Log
- Fool Traps, with different intensity options (None, Normal, Double, Onslaught)
- An in-depth hint system with several different hint types.
  
## Installation
- Must use the latest PC version of Tunic. The mod has been tested on the Steam and PC Game Pass version, but should work on any PC version. The mod can also be installed on the Steam Deck via Proton using this guide: https://docs.bepinex.dev/articles/advanced/proton_wine.html.
- Go to https://builds.bepinex.dev/projects/bepinex_be and find Artifact #572 and download the "BepInEx Unity (IL2CPP) for Windows (x64) games" build. (The mod is currently NOT compatible with newer BepInEx builds so #572 is recommended. Here is a direct link for the correct download: https://builds.bepinex.dev/projects/bepinex_be/572/BepInEx_UnityIL2CPP_x64_9c2b17f_6.0.0-be.572.zip)

  ![image](https://user-images.githubusercontent.com/110704408/188519149-d9476aa9-55f6-4f38-9ce9-93d137fa71af.png)

- Extract the zip folder into your game's install directory (For example: C:\Program Files (x86)\Steam\steamapps\common\TUNIC)
  - For the PC Game Pass version, extract the zip into the "Content" folder, i.e. C:\XboxGames\Tunic\Content
- Launch the game and close it. This will finalize the BepInEx installation.
- Download the TunicRandomizer.dll file and copy it into BepInEx/plugins under your game's install directory
- Launch the game again and start a new game!

## Tips
- The following keyboard shortcuts are built-in to the mod:
  - Press '1' to switch between day/night time. (Note: This is locked until after the first time you fight The Heir)
  - Press '2' to display the current seed.
  - Press '3' to display the fox's color palette (important).
  - Press '4' to view how many items have been checked overall and in the current area.
  - Press '5' to randomize the fox's color palette on demand (bonus feature).
- Upon selecting New Game or Continue, a spoiler log file named "Spoiler.log" will be created in the Randomizer folder in the AppData folder for the game (same folder that the save file folder is located, typically C:\Users\You\AppData\LocalLow\Andrew Shouldice\Secret Legend)
- To use a custom seed:
  - Start a New Game from the main menu.
  - Save and quit after the intro cutscene plays.
  - Open the new save file in a text editor 
    - Save files can typically be found at C:\Users\You\AppData\LocalLow\Andrew Shouldice\Secret Legend\SAVES
  - Change the seed value on the line that looks like "seed|seed value" to your custom seed, then save the file.
  - Load the edited save file by pressing Continue or from the Load Game menu.
- The randomizer is not intended to be played with pre-existing save files from the vanilla version of the game, and files made with the randomizer are not intended to be played in the vanilla version of the game (i.e. with the randomizer disabled), as it may prevent progress from being made in the game in either case.
- To uninstall the mod, either remove/delete the TunicRandomizer.dll file from the plugins folder or rename the winhttp.dll file located in the game's root directory (this will disable all installed mods from running).

## Credits
- zanders3 for TinyJson (https://github.com/zanders3/json)
- Glacia and RisingStar111 for helping research how to mod this game
- Radicoon, kingsamps0n, Landie, and JimTheEternal for playtesting
- Andrew Shouldice, Kevin Regamey, Finji, and everyone else who made this wonderful game.

## Screenshots
![image](https://user-images.githubusercontent.com/110704408/193220644-e62bc84b-ccaa-4245-b080-797e17b5d640.png)
![image](https://user-images.githubusercontent.com/110704408/193220673-15a35c0d-fd42-43b0-a946-a007dc671cdd.png)
![image](https://user-images.githubusercontent.com/110704408/193220692-01f3d497-db4c-4200-a2b4-28abba4fdd96.png)
![image](https://user-images.githubusercontent.com/110704408/193220725-e61f149f-14be-4a95-9088-7081926cd3ec.png)
