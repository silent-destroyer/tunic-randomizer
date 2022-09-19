# Tunic Randomizer
An extensive randomizer mod for the tiny fox game TUNIC.
<br>
Created by SilentDestroyer and Glacia (Spaceglace)

## Features
- Randomizes every item in the game, including:
  - All Chests
  - Instruction Manual Pages
  - Fairies
  - Item Pickups (Sword, Keys, etc.)
  - Hero Relics
  - Unique items in the shop (Flasks and Coins)
  - Trinket card slots from tossing coins into wells
  - A number of ice traps replacing less valuable rewards
- Custom seed support
- Randomized Fox colors upon every scene transition (!!)
- Hint System
- Spoiler log
- Randomizer Settings Menu

## Installation
- Must use the Windows Steam version of TUNIC
- Go to https://builds.bepinex.dev/projects/bepinex_be and find Artifact #572 and download the "BepInEx Unity (IL2CPP) for Windows (x64) games" build. (The mod is currently NOT compatible with newer BepInEx builds so #572 is recommended. Here is a direct link for the correct download: https://builds.bepinex.dev/projects/bepinex_be/572/BepInEx_UnityIL2CPP_x64_9c2b17f_6.0.0-be.572.zip)

  ![image](https://user-images.githubusercontent.com/110704408/188519149-d9476aa9-55f6-4f38-9ce9-93d137fa71af.png)

- Extract the zip folder into your game's install directory (For example: C:\Program Files (x86)\Steam\steamapps\common\TUNIC)
- Launch the game and close it. This will finalize the BepInEx installation.
- Download the TunicRandomizer.dll file and copy it into BepInEx/plugins under your game's install directory
- Launch the game again and start a new game!

## Tips
- The following keyboard shortcuts are built-in to the mod:
  - Press '1' to switch between day/night time. (Note: This is locked until after the first time you fight The Heir)
  - Press '2' to display the current seed.
  - Press '3' to display the fox's color palette (important).
  - Press '4' to view how many items have been checked overall and in the current area.
- Upon selecting New Game or Continue, a spoiler log file named "RandomizerSpoiler.log" will be created in the AppData folder for the game (same folder that the save file folder is located, typically C:\Users\You\AppData\LocalLow\Andrew Shouldice\Secret Legend)
- To use a custom seed:
  - Start a New Game from the main menu.
  - Save and quit after the intro cutscene plays.
  - Open the new save file in a text editor 
    - Save files can typically be found at C:\Users\You\AppData\LocalLow\Andrew Shouldice\Secret Legend\SAVES
  - Change the seed value on the line that looks like "seed|seed value" to your custom seed, then save the file.
  - Close and relaunch the game, then load the edited save file by pressing Continue or from the Load Game menu.
- The randomizer is not intended to be played with pre-existing save files from the vanilla version of the game, and files made with the randomizer are not intended to be played in the vanilla version of the game (i.e. with the randomizer disabled), as it may prevent progress from being made in the game in either case.
  
## Credits
- Tiny Json - zanders3 (https://github.com/zanders3/json)

## Screenshots
<img src="https://user-images.githubusercontent.com/110704408/185547173-e9328f5f-431c-4611-8cbf-9fba98eb0a1e.png" height="40%" width="40%"/> <img src="https://user-images.githubusercontent.com/110704408/185546864-9546ec58-c9a9-4e5d-8a74-bce0a697a6eb.png" height="40%" width="40%"/>
<img src="https://user-images.githubusercontent.com/110704408/185547851-676b91e8-15d5-403b-b6b4-1b0b8ddb445e.png" height="40%" width="40%"/> <img src="https://user-images.githubusercontent.com/110704408/185547944-b4317305-2e95-4d92-b8ae-35850bb21731.png" height="40%" width="40%"/>
