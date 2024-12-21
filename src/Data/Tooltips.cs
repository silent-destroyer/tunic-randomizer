using System.Collections.Generic;

namespace TunicRandomizer {
    public class Tooltips {

        public static Dictionary<string, string> OptionTooltips = new Dictionary<string, string>() {
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
            { "Clear Early Bushes", "Clears some of the early bush paths that block the way to other parts of the Overworld.\n(This option is not exclusive to the Grass Randomizer, and can be turned on at any point after starting.)" },
            { "Fool Traps", "Replaces money and other low-value items with fool traps that apply negative effects to the player.\nNormal adds 15 traps, Double adds 32, and Onslaught adds 50." },
            { "Hero's Laurels Location", "Places the Hero's Laurels at a fixed location." },
            { "Lanternless Logic", "Makes dark areas not require the Lantern in logic, potentially requiring you to navigate them without it." },
            { "Maskless Logic", "Makes Lower Quarry not require the Scavenger's Mask in logic, potentially requiring you to navigate there without it."},
            { "Enemy Randomizer", "Randomizes the enemies that appear throughout the game. See the in-game options menu for configuration options, as well as for a variety of other options not shown here." },
            { "Music Shuffle", "Shuffles the music that plays throughout the game. See the in-game options menu to select which music tracks can be played, as well as for a variety of other options not shown here." },
            { "Death Link", "Enables Death Link when playing on Archipelago. When someone with Death Link enabled dies, everyone else that has it enabled also dies.\nSee the in-game options menu for a variety of other options not shown here." },
            { "Shuffle Breakable Objects", "Turns over 250 ordinary breakable objects in the game into checks, including pots, boxes, barrels, signposts, and more." }
        };
    }
}
