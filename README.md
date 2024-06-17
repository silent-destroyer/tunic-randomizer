# Tunic Randomizer and Archipelago Client

This randomizer features item, entrance, and enemy randomization, as well as a variety of additional settings and features to help customize the experience, such as hints, custom items, and custom fox colors!

This mod contains both the standalone, single-player randomizer and the Archipelago Multiworld integration.

For questions, feedback, bug reports, or other discussion related to the randomizer, please visit the dedicated randomizer channel in the [Tunic Speedrunning Discord](https://discord.gg/HXkztJgQWj)! 

For discussion around the Archipelago side of things, please visit the dedicated Tunic channel in the [Archipelago Discord](https://discord.gg/8Z65BR2).

## Installation
- Must use a compatible PC version of TUNIC on the latest update. The mod has been tested on Steam and PC Game Pass versions, but should realistically work on any PC version (including Steam Deck).
    - If playing on Steam Deck or Linux, first follow this guide to [setting up BepInEx via Proton/Wine](https://docs.bepinex.dev/articles/advanced/proton_wine.html).

- Download the appropriate IL2CPP release of [BepInEx 6](https://github.com/BepInEx/BepInEx/releases/download/v6.0.0-pre.1/BepInEx_UnityIL2CPP_x64_6.0.0-pre.1.zip).

- Extract the BepInEx zip folder you downloaded from the previous step into your game's install directory (For example: C:\Program Files (x86)\Steam\steamapps\common\TUNIC)
  - For the PC Game Pass version, extract the zip into the `Content` folder, i.e. C:\XboxGames\Tunic\Content
- Launch the game and close it. This will finalize the BepInEx installation.
- [Download and extract the `Tunic Randomizer.zip` file from the latest release.](https://github.com/silent-destroyer/tunic-randomizer/releases/latest)
  - Copy the `Tunic Randomizer` folder from the release zip into `BepInEx/plugins` under your game's install directory.
  - Your plugins folder should have a `Tunic Randomizer` folder with `three .dll files` inside.
- Launch the game again and you should see `Randomizer Mod Ver. x.y.z` on the top left of the title screen!
- To uninstall the mod, either remove/delete the `Tunic Randomizer` folder from the plugins folder or rename the winhttp.dll file located in the game's root directory (this will disable all installed mods from running).

## Starting a Single Player Randomizer
- On the Title Screen, select `Single Player`, and select any additional settings you wish to play with. Descriptions of all of them can be found below.
- Optionally, clicking Generate Seed will create a seed that you can then share with others by pressing the Copy Seed/Settings button. This will generate a settings string that another player can import by copying and then pressing Paste Seed/Settings.
- Click New Game and have fun!

## Generating a Multiworld with Archipelago
- Head to the [TUNIC Options Page](https://archipelago.gg/games/TUNIC/player-options) on the Archipelago website.
- Select your desired settings and enter a player name, then click `Export Options`. This will download a yaml file, which contains the information you selected on the page.
  - If playing solo, you can simply click `Generate Game` instead and skip the next two steps.
  - If playing with multiple people, you will need to send your yaml file to the person creating the game, or have everyone send you theirs if you are the one creating the game.
- Once you have every player's yaml file, compress them into a zip file and upload it (here)[https://archipelago.gg/generate].
- Click `Create New Room`, and you will see something like `/connect archipelago.gg:38281`. Archipelago.gg is the host and the last 5 numbers here are the port number, which you will need for the next step.
- Launch the game and select `Archipelago` on the Title Screen, then click `Edit AP Config` and fill in your connection details. Player name must match what you entered in the first step, and hostname/port should match the info from the previous step.
- All that's left is to press Connect, and if it says `Connected`, simply start a New Game and have fun!
- For more information, see the official [Archipelago Setup Guide](https://archipelago.gg/tutorial/TUNIC/setup/en).
- Note: These steps are for playing with games that are supported on the website. For beta/wip games, you will need to run the Archipelago software on your machine to generate the game instead by using (this guide)[https://archipelago.gg/tutorial/Archipelago/setup/en].

## Helpful Tips
- Regarding Logic:
  - West Garden is logically accessible with either the Lantern or Hero's Laurels.
  - The Eastern Vault Fortress is accessible with either the Lantern (and Prayer) or Hero's Laurels (with or without Prayer).
  - The Library is accessible with either the Hero's Laurels or the Magic Orb.
  - The Swamp is available from the start via a secret path beneath the Overworld fuse, and another through the water to the right of the Swamp save statue.
  - The Cathedral is accessible during the day by using the Hero's Laurels to access and activate the Overworld fuse near the Swamp entrance.
  - The Scavenger's Mask is in logic to not be shuffled into the lower areas of Quarry or within the Ziggurat.
  - The Holy Cross chest from the dancing fox ghost in East Forest can be spawned in during the daytime. 
  - The Secret Legend chest in the Cathedral can similarly be obtained during the day by opening the Holy Cross door from the outside.
  - Nighttime is not considered in logic. Every check in the game is logically obtainable before the first visit to The Heir, and since nighttime restricts access to several locations it would limit the amount of options overall if certain items were required to always be obtainable at night. The game will automatically switch back to daytime if you die to The Heir on the first visit, but you can optionally switch to nighttime at any point after that by interacting with a special object in the Old House.
  - For Entrance Rando:
    - You can use the Torch to return to the Overworld save point at any time.
    - The dancing ghost fox area in East Forest can be accessed using the Hero's Laurels from the gate entrance of Guard House 1.
    - Using the Hero's Laurels, you can dash through the gate in the Bottom of the Well boss room and Monastery.
    - Activating a fuse to turn on a Prayer teleporter also activates its counterpart in the Far Shore.
    - The West Garden fuse can be activated from below.
    - You can reach the main part of Ruined Atoll from the lower entrance area using the Hero's Laurels.
    - The elevators in Ziggurat only go down.
    - The portal in the trophy room in the back of the Old House is active from the start.
    - The elevator in Cathedral is immediately accessible without activating the fuse. Activating the fuse does nothing.
    - There is an [entrance tracker](https://scipiowright.gitlab.io/tunic-tracker/), created by ScipioWright.
- Regarding Hints:
  - The Mailbox will give a "First Steps" hint, pointing you in the direction of a useful/progression item that can be reached from the start of the game.
    - If playing with ladder shuffle, this hint will still let you know where a useful item is, but it may require you to find some ladders first to get to it and thus won't always be immediately available.
  - The Hero's Graves in the Swamp, Monastery, and Library will hint the location of the three Hexagon keys.
  - The Hero's Graves in East Forest, West Garden, and the Eastern Vault Fortress hint towards a major progression item, such as the Magic Orb, Lantern, Magic Wand, Magic Dagger, and/or the Prayer/Holy Cross pages if abilities are shuffled.
  - The statue in the Sealed Temple will always hint the general location of the Hero's Laurels.
- The "Fairy Seeker" Holy Cross spell (ULURDR) can  be used to seek out all items in an area, instead of just fairies. If all items in an area have been found, the fairy seeker will seek out the closest load zone that has items immediately beyond it. Useful for finding missing checks in areas with lots of obscured or hidden items!
- Save files created by the randomizer will be marked with a "randomizer" or "archipelago" tag in the file select screen to help differentiate them from your vanilla save files while the mod is loaded. The mod has protections in place to avoid loading vanilla saves on accident as well.
- The mod will routinely write to a couple of files in the Randomizer folder under the game's AppData directory (typically C:\Users\You\AppData\LocalLow\Andrew Shouldice\Secret Legend\Randomizer):
  - Spoiler.log - This file lists every check in the game and what item they contain. It will also mark off which checks you have collected.
  - ItemTracker.json - This file contains information such as the current seed number, what important items have been obtained, and a running list of all checks that have been found. Can be useful for creating external programs that interface with the randomizer, such as this [Item Tracker](https://github.com/radicoon/tunic-rando-tracker) by Radicoon.
- Custom seeds can be set on the title screen before starting a New Game.
- The Randomizer will notify you on the title screen if a new update is available.
- A [Map Tracker Package](https://github.com/SapphireSapphic/TunicTracker) exists for this game with a full map tracker, created by SapphireSapphic and ScoutJD. It can also be used with Poptracker, thanks to Br00ty.

## Settings
With the exception of the Logic settings (which are determined in your Archipelago yaml settings), all options can be freely toggled or changed at any point while playing.
### Logic
- Hexagon Quest
    - Gold Hexagons are shuffled into the item pool. Find the required amount of them and visit The Heir to win. The required amount can be customized in the Advanced Options menu.
- Keys Behind Bosses 
  - Places the three Hexagon Keys behind their respective bossfight. In Hexagon Quest, this option guarantees a Gold Hexagon is placed behind each of the three major bosses.
- Sword Progression
  - Replaces the stick and swords in the item pool with four Sword Upgrades that progressively level up as you find them, with the final two upgrades being custom swords that offer extended reach compared to the standard Sword and a free +1 to your Attack level when found.
  - Level 1: Stick -> Level 2: Sword -> Level 3: Librarian's Sword -> Level 4: Heir's Sword
- Start With Sword
  - The player will spawn with a Sword in the inventory on New Game. Does not count towards Sword Progression.
- Shuffle Abilities
  - Locks the ability to use Prayer, most Holy Cross codes*, and the Icebolt combo technique until the respective manual page for each ability is found.
  - Prayer is unlocked by Page 24, Holy Cross is unlocked by Page 43, and the Icebolt technique is unlocked by Page 53. If playing Hexagon Quest, abilities are unlocked when reaching 25%, 50%, and 75% of the required amount of Gold Hexagons.
  - *This option only locks Holy Cross codes that block access to checks in the randomizer. The free bomb codes and other player-facing codes like Big Head Mode, Sunglasses, Fairy Seeker, etc. are still usable from the start.
- Shuffle Ladders
  - Turns several ladders in the game into items that must be found before they can be climbed on, blocking off several paths early on and adding more layers of progression.
  - "Ladders were a mistake." —Andrew Shouldice
- Entrance Randomizer
  - Shuffles all the connections between doors, teleporters, portals, and more. Where will the fox end up?
- Entrance Randomizer: Fewer Shop Entrances
  - Reduces the amount of possible shops that can be found, and places a guaranteed shop entrance at the Overworld Windmill entrance.
- Fool Traps
  - Enables fool traps, which replace low-value money rewards when enabled and apply damage/other negative effects to the player. Turning the setting up increases the amount of traps in the item pool.
  - For Single Player seeds, this option can be changed mid-run by changing the value and reloading the save file. For Archipelago games, this is a yaml option and cannot be changed after the game starts.
- Hero Laurels Location
  - Place the Laurels at a predetermined location, currently the options are the 6 coin reward, 10 coin reward, or 10 fairy reward.
- Lanternless Logic
  - Removes the Lantern as a requirement for dark areas, allowing it (or items that grant access to it) to be shuffled into places like Dark Tomb, etc.
- Maskless Logic
  - Removes the Mask as a requirement for the miasma in Quarry, allowing it to get shuffled into lower Quarry or beyond.
- Mystery Seed
  - Randomly chooses logic options for you on New Game. Good luck!
### Archipelago-Specific Settings
- Death Link
  - Want a more chaotic experience? When a player with Death Link enabled dies, everyone else with the setting on *also* dies. Be careful!
- Auto-open !collect-ed Checks
  - Makes checks that you haven't found but were completed by another player (via !collect, slot co-op, etc) appear as already been opened/picked up. Also reflects the item counts on the inventory screen/ending summary. Can be toggled on/off freely, and will revert the appearance of checks to their previous state on the next scene transition.
- Send hints to server
  - This setting will record certain hints from the ghost foxes and any shop items you inspect in the Archipelago Text Client.
### Hints
- Path of the Hero
  - Places a major hint at specific locations around the world, including the Mailbox, the Hero Graves, and the statue in the Sealed Temple. These hint towards major progression items, such as Magic Items, Laurels, Hexagons, and more.
- Ghost Foxes
  - Spawns 15 Ghost Fox NPCs around the world that give minor hints. These hints include the locations of useful non-progression items, items in hard-to-reach locations, and barren locations.
  - There are over 50 unique Ghost Fox spawns, so be on the lookout!
- Freestanding Items and Chests Match Contents
  - All freestanding items (Item Pickups, Page Pickups, Shop Items, Hero's Grave Relics, etc.) will have their model swapped to appear as the item they are randomized as.
  - Chest textures will be swapped to indicate what item is in them. Currently, the items with different chest textures are Fairies, Golden Trophies, the three Hexagons, and the Hero's Laurels.
- Display Hints in Trunic
  - For the experienced Ruin Seekers out there, this option removes most English words from custom dialogue, hints, or other text produced by the randomizer, leaving it up to your own knowledge to figure out what is where.
### General
- Easier Heir Fight
  - Attacks deal additional damage to The Heir based on the total number of checks found.
- Clear Early Bushes
  - Removes a number of bushes from the Overworld when enabled, allowing free access to the East and West sides of the map.
- Enable All Checkpoints
  - Allows all checkpoints (save statues) to be used regardless of if they are powered or not.
- Cheaper Shop Items
  - Reduces the cost of the four randomized Shop items to 300 bits each.
- Bonus Upgrades
  - Makes the Golden Trophy items give free Level Ups for certain stats, allowing you to get up to +8 in every stat in a single playthrough when combined with the regular stat upgrades and the +2 Attack levels from Sword Progression.
  - Note: Bonus upgrades will not be retroactively awarded if this setting is turned on after obtaining Golden Trophies with it disabled. The +2 Attack Levels from Sword Progression are always awarded and are not affected by this setting.
- Disable Chest Interruption
  - Enemies will not be able to interrupt you while opening chests if this option is turned on.
- Skip Item Popups
  - Turns off the item/page popups when receiving items.
- Skip Upgrade Animations
  - Skips the animation that plays when upgrading stats.
### Enemy Randomization 
- Enemy Randomizer
  - Randomly swaps out enemies with new ones when you load into a scene. You may even see some enemies you've never seen before!
- Extra Enemies
  - Enables certain NG+ and nighttime enemy slots to add more enemies into the mix for increased chaos.
- Balanced Enemies
  - Enemies are randomized based on the difficulty of the original enemy being swapped out.
- Seeded Enemies
  - Enemy spawns will remain consistent everytime an area is reloaded.
- Limit Boss Spawns
  - Limits the number of boss-type enemies that can spawn in the enemy randomizer.
  - Turn off at own risk! Too many bosses can lag the game in certain areas.
- Enemy Toggles
  - Allows you to customize the enemy randomizer experience by choosing which enemies you want to appear.
  - Turn on the `Use Enemy Toggles` option and then use the Enemy Toggle List to make your choices.
### Fox Customization 🦊
- Random Fox Colors
  - Fox colors will randomize on every load/scene transition.
- Keepin' It real
  - Toggles Sunglasses on/off without needing to use the Holy Cross code.
- Fox Color Editor
  - Allows you to customize the colors of your fox with more options than what the base game offers, and save it as a custom texture for future use.
- Custom Fox Texture
  - When enabled, will always apply the saved custom texture to the fox. The custom texture can be found under AppData in the same folder as the Spoiler log and Item Tracker file.
### Music Shuffler
- Music Shuffle
  - Plays a random music track when everytime you enter an area.
- Seeded Music
  - Plays a set track per scene.
- Music Toggles
  - Allows you to customize which tracks play when the shuffler is turned on.
### Race Mode Settings
- Race Mode
  - An option to help facilitate ranomizer races more easily. Disables the spoiler log, and enables the options below to be used.
- Disable Icebolt in The Heir Fight
  - Disables use of the icebolt technique when fighting The Heir.
- Disable Long-Distance Bell Shots
  - Prevents you from ringing the West Bell early, by shoowing it with the Magic Wand from far away.
- Disable Ice Grappling
  - Prevents you from pulling yourself towards frozen enemies with the Magic Orb. (The East Forest blob is excluded from this.)
- Disable Ladder Storage
  - Prevents the Ladder Storage glitch from being used.
- Disable Upgrade Stealing
  - Prevents the Upgrade Stealing glitch from being used.
### Other Settings
- ???
  - !esirprus a rof no nruT
- More Skulls
  - Does exactly what it says on the tin.
- Arachnophobia Mode
  - Turns the spiders and another mulit-legged enemy into...something else.
- Holy Cross DDR
  - Spawns DDR-style arrows when you use the holy cross to help visualize your inputs.
- Bigger Head Mode
  - Beeg
- Tinier Fox Mode
  - Smol
  
## Credits
- Glace and RisingStar111 for helping research how to mod this game, and Jabberrock for creating an initial Archipelago integration.
- Scipio for helping out with the entrance randomizer and Archipelago implementation.
- Radicoon, Glace, RisingStar111, kingsamps0n, Landie, JimTheEternal, SapphireSapphic, ScoutJD, and many others for playtesting and helping to improve the mod.
- Andrew Shouldice, Kevin Regamey, Finji, and everyone else involved in making this wonderful game.
