using System.Collections.Generic;

namespace TunicRandomizer {
    public class Tooltips {

        public static Dictionary<string, string> OptionTooltips = new Dictionary<string, string>() {
            // Logic
            { "Sword Progression", "Replaces the Stick and Swords in the item pool with Sword Upgrades that progressively unlock better melee weapons, including two new swords with increased range." },
            { "Hexagon Quest", "Adds a number of Gold Questagons into the item pool. Find the required amount and visit the heir to beat the game. Can be configured in the Advanced Options menu." },
            { "Keys Behind Bosses", "Places the Red, Green, and Blue Questagons behind their respective boss fights.\nIf playing Hexagon Quest, it will place Gold Questagons on the bosses." },
            { "Shuffle Abilities", "Locks the abilities of Prayer, Holy Cross*, and Icebolt until their respective pages of the manual are found." +
                "\n*Some Holy Cross spells can still be used, such as the free bombs, seeking spell, heal spell, etc." },
            { "Entrance Randomizer", "Shuffles the connections between doors, teleporters, portals, and more. Where will the fox end up?\nCan be configured with additional options in the Advanced Options menu." },
            { "Shuffle Ladders", "Turns several ladders in the game into items that must be found before they can be climbed on, blocking off several paths early on and adding more layers of progression.\n\"Ladders were a mistake.\" —Andrew Shouldice" },
            { "Start With Sword", "Start with a sword in the player's inventory. Does not replace finding the first two upgrades if Sword Progression is on." },
            { "Mystery Seed", "Settings will be randomly selected on New Game. See the Mystery Seed Configuration menu to configure option weights." },
            { "Hexagons Required", "How many Gold Questagons are required to beat the game for Hexagon Quest. Can be set to any number 1-100, or randomized within certain ranges. Default is 20 required with 30 total in the item pool." },
            { "Hexagons in Item Pool", "How many Gold Questagons are put into the item pool. The max amount is twice the required amount or 100, whichever is lower." },
            { "Randomize Hexagon Quest Amounts", "Randomly choose the amount required to win, and how many extras.\n" +
                "Required amounts: Random (10-50), Low (10-23), Medium (24-37), High (38-50)\n" +
                "Extras (% of required amount): Random (0-100%), Low (0-33%), Medium (34-66%), High (67-100%)" },
            { "Hexagon Quest Ability Shuffle Mode", "Choose how abilities are unlocked for Hexagon Quest + Shuffled Abilities.\n" +
                "Hexagons: Abilities are unlocked when reaching 25%, 50%, and 75% of the required amount of Questagons.\n" +
                "Pages: Abilities are unlocked by finding their manual page." },
            { "Fewer Shops", "Places a single, fixed shop entrance at the Overworld windmill and removes all other potential shop entrances from the pool.\nNote: This option is not compatible with the Matching Directions option." },
            { "Matching Directions", "Entrances will be paired with other entrances based on their direction or type (i.e. North/South, East/West, Ladders Up/Down, and Teleporters).\nNote: This option is not compatible with the Fewer Shops option." },
            { "Decoupled Entrances", "Every entrance turns into a one-way. Leaving the way you came from will take you somewhere different." },
            { "Grass Randomizer", "Turns over 6,000 bushes and pieces of grass into potential item locations." },
            { "Fool Traps", "Replaces money and other low-value items with fool traps that apply negative effects to the player.\nNormal adds 15 traps, Double adds 32, and Onslaught adds 50." },
            { "Hero's Laurels Location", "Places the Hero's Laurels at a fixed location." },
            { "Lanternless Logic", "Makes dark areas not require the Lantern in logic, potentially requiring you to navigate them without it." },
            { "Maskless Logic", "Makes Lower Quarry not require the Scavenger's Mask in logic, potentially requiring you to navigate there without it."},
            { "Enemy Randomizer", "Randomizes the enemies that appear throughout the game. See the in-game options menu for configuration options, as well as for a variety of other options not shown here." },
            { "Music Shuffle", "Shuffles the music that plays throughout the game. See the in-game options menu to select which music tracks can be played, as well as for a variety of other options not shown here." },
            { "Death Link", "Enables Death Link when playing on Archipelago. When someone with Death Link enabled dies, everyone else that has it enabled also dies.\nSee the in-game options menu for a variety of other options not shown here." },
            { "Trap Link", "Enables Trap Link when playing on Archipelago. When someone with Trap Link enabled receives a Trap item, everyone else that has it enabled will receive the same or a similar Trap (if their game supports it).\nSee the in-game options menu for a variety of other options not shown here." },
            { "Shuffle Breakable Objects", "Turns over 250 ordinary breakable objects in the game into checks, including pots, boxes, barrels, signposts, bombable walls, and more." },
            { "Shuffle Fuses", "Praying at a fuse now rewards an item instead of turning on the power.\nInstead, the power connections between fuses are shuffled into the item pool and unlocked separately." },
            { "Shuffle Bells", "The East and West bells are shuffled into the item pool and must be found in order to unlock the Sealed Temple.\nRinging the bells will instead now reward a random item." },
            { "Laurels Zips", "Adds a number of gates, doors, and other tricky spots that can be bypassed using the Hero's Laurels into logic. Notable inclusions are the Monastery gate, Ruined Passage door, Old House gate, Forest Grave Path gate, and going between the back and middle of Swamp." },
            { "Ice Grapples", "Adds logic for grappling to frozen enemies to reach new areas.\nMay include pushing enemies through walls or luring them far distances." },
            { "Ice Grapples Off", "Off: Ice Grapples are not in logic, except for the chest in East Forest." },
            { "Ice Grapples Easy", "Adds logic for grappling to frozen enemies to reach new areas.\nEasy: Includes enemies that can be targered or are in range without luring them." },
            { "Ice Grapples Medium", "Adds logic for grappling to frozen enemies to reach new areas.\nMedium: Includes Easy difficulty plus pushing nearby enemies through walls/doors or off ledges. Also includes luring an enemy to the Sealed Temple door." },
            { "Ice Grapples Hard", "Adds logic for grappling to frozen enemies to reach new areas.\nHard: Includes Easy and Medium difficulties plus luring enemies to get to where you want to go." },
            { "Ladder Storage", "Adds ladder storage to the logic. Ladder storage is performed by using an item (logically Stick, Sword, Magic Orb, or Shield) and climbing a ladder at the same time, then quickly rolling away from the ladder to store the ladder state." },
            { "Ladder Storage Off", "Off: Ladder storage is not considered in logic." },
            { "Ladder Storage Easy", "Easy: Includes uses of Ladder Storage to get to open doors over a long distance without too much difficulty. May include convenient elevation changes (going up Mountain stairs, stairs in front of Special Shop, etc.)." },
            { "Ladder Storage Medium", "Medium: Includes Easy difficulty plus changing your elevation using the environment and getting knocked out of ladder storage by melee attacks from enemies." },
            { "Ladder Storage Hard", "Hard: Includes Easy and Medium difficulties plus going out of bounds to enter closed doors from behind and shooting fuses up close with the Magic Wand to knock yourself out of ladder storage." },
            { "Ladder Storage Without Items", "If enabled, you will be expected to perform Ladder Storage without any specific items. This can be done with the plushie code, a Golden Coin, Prayer, and more. This option has no effect if you do not have Ladder Storage Logic enabled." },

            // Seed
            { "Generate Seed", "(Optional) Generate a random number to be used as the randomizer seed. If no seed is set, the game will generate one upon starting a New Game." },
            { "Copy Seed + Settings", "Copies a string to the clipboard containing the seed and all relevant settings that can be shared with others and applied using the Paste Seed + Settings button." },
            { "Paste Seed + Settings", "Applies the seed number and all relevant settings from a given settings string generated with the Copy Seed + Settings button." },
            { "Connect via Room Link", "On the Archipelago room page, right click your Player Name, then click Copy Link. With the link copied, press this button to connect automatically." },

            // General
            { "Easier Heir Fight", "Attacks deal additional damage to The Heir based on the percentage of checks found." },
            { "Clear Early Bushes", "Removes a couple of bushes that block access to the East and West sides of the Overworld.\nNote: The logic does not account for these bushes. If this setting is turned off you are otherwise expected to clear them using the free bomb codes or by luring enemies to break them." },
            { "Enable All Checkpoints", "Allows all checkpoints (save statues) to be used regardless of if they are powered or not." },
            { "Cheaper Shop Items", "Makes the four randomized Shop items cost 300 each." },
            { "Bonus Upgrades", "Makes the Golden Trophy and Hero Relic items give free stat level ups, allowing you to get up to +8 in every stat in a single playthrough." },
            { "Disable Chest Interruption", "Prevents the chest opening animation from being interrupted if you take damage." },
            { "Show Recent Items", "Enables a UI element showing the five most recent items that were sent or received." },
            { "Skip Item Popups", "Turns off item and page popups." },
            { "Skip Upgrade Animations", "Skips the animation that plays when upgrading stats at a checkpoint." },
            { "Holy Cross DDR", "Spawns DDR-style arrows above your head to help visualize Holy Cross inputs. The arrows will change color to indicate an incorrect sequence." },
            { "Arachnophobia Mode", "Turns the Spider enemies into floating signs that say 'Spider' in Trunic." },
            { "Show Player Position", "Shows the xyz coordinates of the player in the lower right corner of the screen." },
            { "???", "!esirprus a rof no nruT" },
            { "More Skulls", "Spawns four additional gold skulls in the Swamp :)" },

            // Hints
            { "Path of the Hero", "Places a major hint at specific locations throughout the game, including the Mailbox, the Hero Graves, and the statue in the Sealed Temple." },
            { "Ghost Foxes", "Spawns 15 Ghost Fox NPCs around the world that give minor hints about the location of useful items and items in hard-to-reach locations.\nThere are over 70 unique Ghost Foxes, each with their own unique dialogue, so be on the lookout!" },
            { "Seeking Spell Logic", "Makes it so the seeking spell only targets checks that can be reached in logic. By default, the seeking spell will target the closest item regardless of if it can be reached or not." },
            { "Freestanding Items Match Contents", "All freestanding items will have their model swapped to the item they are randomized as.\n\n" +
                "For Archipelago items: Green = filler item, Blue = useful item, and Gold = progression item." },
            { "Chests Match Contents", "Swaps chest textures to indicate what item is in them. This applies to Fairies, Golden Trophies, Red/Green/Blue Questagons, and the Hero's Laurels.\n" +
                "For Archipelago item chests: Green = filler item, Blue = useful item, and Gold = progression item." },
            { "Write Hints In Trunic", "Replaces most English words in the custom randomizer dialogue and hints with Trunic, " +
                "leaving it up to your own knowledge to figure out what is where.\nSome text, like Archipelago player/item names, will not be translated to Trunic." },
            { "Entrance Tracker", "Opens the Entrance Tracker website. The Randomizer produces a .csv file that can be uploaded to sync the tracker. Auto-tracking is supported for Archipelago games." },
        };
    }
}
