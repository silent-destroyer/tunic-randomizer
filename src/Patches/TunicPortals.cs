using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;
using BepInEx.Logging;
using Lib;
using HarmonyLib;
using JetBrains.Annotations;

namespace TunicRandomizer
{
    public class TunicPortals {
        private static ManualLogSource Logger = TunicRandomizer.Logger;

        public class TunicPortal
        {
            public string SceneName;
            public string Destination;
            public string DestinationTag;
            public string DestinationPair;
            public string PortalName;
            public Dictionary<string, int> RequiredItemCount;
            public List<Dictionary<string, int>> RequiredItems;

            public TunicPortal() { }

            public TunicPortal(string destination, string destinationTag, string portalName)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName; // this is name we gave the portals to make them easier to identify
                DestinationPair = destination + destinationTag;
            }
            
            public TunicPortal(string destination, string destinationTag, string portalName, Dictionary<string, int> requiredItems)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName; // this is name we gave the portals to make them easier to identify
                RequiredItemCount = requiredItems; // the requirements to get from the center of a region to the center of it
                DestinationPair = destination + destinationTag;
            }

            // if there are different requirements that can be met (x OR y), use a list instead
            public TunicPortal(string destination, string destinationTag, string portalName, List<Dictionary<string, int>> requiredItems)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName; // this is name we gave the portals to make them easier to identify
                RequiredItems = requiredItems; // the requirements to get from the center of a region to the center of it
                DestinationPair = destination + destinationTag;
            }
        }

        // this is a big list of every portal in the game
        // we'll need to convert these into Portals (see Models/Portal.cs)
        // maybe it'll need to be less overcomplicated later to remove the conversion? Hard to say, will have to see how it goes
        public static Dictionary<string, List<TunicPortal>> PortalList = new Dictionary<string, List<TunicPortal>>
        {
            {
                "Overworld Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Sword Cave", "", "Sword Cave Entrance"),
                    new TunicPortal("Windmill", "", "Windmill Entrance"),
                    new TunicPortal("Sewer", "entrance", "Well Entrance"),
                    new TunicPortal("Sewer", "west_aqueduct", "Well Rail Left Entrance", new Dictionary<string, int> { {"Aqueduct Rail access", 1} }),
                    new TunicPortal("Overworld Interiors", "house", "Old House Entry Door", new Dictionary<string, int> { {"old house key", 1}, {"Overworld Redux access", 1} }),
                    new TunicPortal("Overworld Interiors", "under_checkpoint", "Old House Other Entrance"),
                    new TunicPortal("Furnace", "gyro_upper_north", "OW Furnace Upper North Entrance"),
                    new TunicPortal("Furnace", "gyro_upper_east", "OW Furance Upper East Entrance"),
                    new TunicPortal("Furnace", "gyro_west", "OW Furnace West Entrance"),
                    new TunicPortal("Furnace", "gyro_lower", "OW Furnace Lower Entrance"),
                    new TunicPortal("Overworld Cave", "", "Rotating Lights Entrance"),
                    new TunicPortal("Swamp Redux 2", "wall", "Swamp Upper Entrance"),
                    new TunicPortal("Swamp Redux 2", "conduit", "Swamp Lower Entrance"),
                    new TunicPortal("Ruins Passage", "east", "Ruins Hall Entrance Not-door"),
                    new TunicPortal("Ruins Passage", "west", "Ruins Hall Entrance Door"),
                    new TunicPortal("Atoll Redux", "upper", "Atoll Upper Entrance"),
                    new TunicPortal("Atoll Redux", "lower", "Atoll Lower Entrance"),
                    new TunicPortal("ShopSpecial", "", "Special Shop Entrance"),
                    new TunicPortal("Maze Room", "", "Maze Entrance"),
                    new TunicPortal("Archipelagos Redux", "upper", "West Garden Entrance by Belltower"),
                    new TunicPortal("Archipelagos Redux", "lower", "West Garden Entrance by Dark Tomb"),
                    new TunicPortal("Archipelagos Redux", "lowest", "West Garden Laurel Entrance"),
                    new TunicPortal("Temple", "main", "Temple Door Entrance"),
                    new TunicPortal("Temple", "rafters", "Temple Upper Entrance"),
                    new TunicPortal("Ruined Shop", "", "Ruined Shop Entrance"),
                    new TunicPortal("PatrolCave", "", "Patrol Cave Entrance"),
                    new TunicPortal("Town Basement", "beach", "Hourglass Cave Entrance"),
                    new TunicPortal("Changing Room", "", "Changing Room Entrance"),
                    new TunicPortal("CubeRoom", "", "Cube Entrance"),
                    new TunicPortal("Mountain", "", "Stairs from Overworld to Mountain"),
                    new TunicPortal("Fortress Courtyard", "", "Fortress Entrance"),
                    new TunicPortal("Town_FiligreeRoom", "", "HC Room Entrance next to Changing Room"), // ? verify this is the one in the middle
                    new TunicPortal("EastFiligreeCache", "", "Glass Cannon HC Room Entrance"),
                    new TunicPortal("Darkwoods Tunnel", "", "Entrance to Quarry Connector"),
                    new TunicPortal("Crypt Redux", "", "Dark Tomb Entrance"),
                    new TunicPortal("Forest Belltower", "", "East Forest Entrance"),
                    new TunicPortal("Transit", "teleporter_town", "Town Portal"),
                    new TunicPortal("Transit", "teleporter_starting island", "Spawn Portal"),
                    new TunicPortal("Waterfall", "", "Entrance to fairy cave"),

                    // new TunicPortal("_", "", "Portal"), // ?
                    // new TunicPortal("Forest Belltower_", "showfloordemo2022", "Portal (12)"), // ?
                    // new TunicPortal("DEMO_altEnd_", "", "_Portal (Secret Demo End)"), // ?
                }
            },
            {
                "Waterfall", // fairy cave
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Waterfall exit"),
                }
            },
            {
                "Windmill",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Windmill Exit"),
                    new TunicPortal("Shop", "", "Windmill Shop Entrance"),
                }
            },
            {
                "Overworld Interiors", // House in town
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "house", "Exit from Front Door of Old House"),
                    new TunicPortal("g_elements", "", "Teleport to Secret Treasure Room"),
                    new TunicPortal("Overworld Redux", "under_checkpoint", "Exit from Old House from not the door"),

                    // new TunicPortal("Archipelagos Redux_", "", "_ShowfloorDemo2022 Portal"), // unused and disabled
                }
            },
            {
                "g_elements", // Secret treasure room
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Interiors", "", "Exit from Secret Treasure Room"),
                }
            },
            {
                "Furnace", // Under the west belltower
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "gyro_upper_north", "Furnace Upper North Exit"),
                    new TunicPortal("Crypt Redux", "", "Furnace Dark Tomb Exit"),
                    new TunicPortal("Overworld Redux", "gyro_west", "Furnace West Exit"),
                    new TunicPortal("Overworld Redux", "gyro_lower", "Furnace Lower Exit"),
                    new TunicPortal("Overworld Redux", "gyro_upper_east", "Furnace Upper East Exit"),
                }
            },
            {
                "Sword Cave", // Stick house
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Sword Cave Exit"),
                }
            },
            {
                "Ruins Passage", // That little hallway with the key door near the start in Overworld
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "east", "Ruins Passage Door Exit"),
                    new TunicPortal("Overworld Redux", "west", "Ruins Passage Not-door Exit")
                }
            },
            {
                "EastFiligreeCache", // The holy cross room with the 3 chests near swamp entrance
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Glass Cannon HC Exit"),
                }
            },
            {
                "Overworld Cave", // East beach, next to swamp entrance, rotating lights room
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Rotating Lights Exit")
                }
            },
            {
                "Maze Room", // Invisible maze
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Maze Exit"),
                }
            },
            {
                "Town Basement", // Hourglass cave
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "beach", "Hourglass Exit"), // yes, it has a tag even though it doesn't need one
                }
            },
            {
                "ShopSpecial", // Special shop, laurel across from that platform between east forest and fortress
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Special Shop Exit"),
                }
            },
            {
                "Temple", // Where you put the hexes
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "rafters", "Temple Upper Exit"),
                    new TunicPortal("Overworld Redux", "main", "Temple Main Exit"),
                }
            },
            {
                "Sewer", // Bottom of the Well
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "entrance", "Well Exit from main entrance"),
                    new TunicPortal("Sewer_Boss", "", "Well to Well Boss"),
                    new TunicPortal("Overworld Redux", "west_aqueduct", "Well Rail Exit"),
                }
            },
            {
                "Sewer_Boss", // Boss room in the Bottom of the Well
                new List<TunicPortal>
                {
                    new TunicPortal("Sewer", "", "Well Boss to Well"),
                    new TunicPortal("Crypt Redux", "", "Well Boss to Dark Tomb"),
                }
            },
            {
                "Crypt Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Dark Tomb Entrance"),
                    new TunicPortal("Furnace", "", "Dark Tomb main exit"),
                    new TunicPortal("Sewer_Boss", "", "Dark Tomb to Well Boss"),
                }
            },
            {
                "Archipelagos Redux", // West Garden
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "lower", "West Garden Exit to Dark Tomb"),
                    new TunicPortal("archipelagos_house", "", "Magic Dagger House Entrance"),
                    new TunicPortal("Overworld Redux", "upper", "West Garden after boss"),
                    new TunicPortal("Shop", "", "West Garden to Shop"), // there's two of these, one is unused and disabled
                    new TunicPortal("Overworld Redux", "lowest", "West Garden Laurel Exit"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "West Garden Hero Grave"), // Hero grave
                    new TunicPortal("Transit", "teleporter_archipelagos_teleporter", "West Garden Portal"), // Portal to the thing behind dagger house
                }
            },
            {
                "archipelagos_house", // Magic Dagger house in West Garden
                new List<TunicPortal>
                {
                    new TunicPortal("Archipelagos Redux", "", "Magic Dagger House Exit"),
                }
            },
            {
                "Atoll Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Frog Stairs", "eye", "Frog Eye Entrance"),
                    new TunicPortal("Library Exterior", "", "Atoll to Library"),
                    new TunicPortal("Overworld Redux", "upper", "Upper Atoll Exit"),
                    new TunicPortal("Overworld Redux", "lower", "Lower Atoll Exit"),
                    new TunicPortal("Frog Stairs", "mouth", "Frog Mouth Entrance"),
                    new TunicPortal("Shop", "", "Atoll Shop"),
                    new TunicPortal("Transit", "teleporter_atoll", "Atoll Portal"),
                    // new TunicPortal("Forest Lake_", "teleporter", "Portal"), // Unused portal, same spot as library portal
                }
            },
            {
                "Frog Stairs", // Entrance to frog's domain
                new List<TunicPortal>
                {
                    new TunicPortal("Atoll Redux", "mouth", "Frog Mouth Exit"),
                    new TunicPortal("frog cave main", "Exit", "Upper Frog to lower frog exit"),
                    new TunicPortal("Atoll Redux", "eye", "Frog Eye Exit"),
                    new TunicPortal("frog cave main", "Entrance", "Upper frog to lower frog entrance"),
                }
            },
            {
                "frog cave main", // Frog's domain, yes it's lowercase
                new List<TunicPortal>
                {
                    new TunicPortal("Frog Stairs", "Exit", "Lower frog exit exit"),
                    new TunicPortal("Frog Stairs", "Entrance", "Lower frog entrance exit"),
                }
            },
            {
                "Library Exterior",
                new List<TunicPortal>
                {
                    new TunicPortal("Library Hall", "", "Library entry ladder"),
                    new TunicPortal("Atoll Redux", "", "Can't go to library with no hook or laurels dummy"),
                }
            },
            {
                "Library Hall", // Entry area with hero grave
                new List<TunicPortal>
                {
                    new TunicPortal("Library Rotunda", "", "Library entrance to circle room"),
                    new TunicPortal("Library Exterior", "", "Library Bookshelf"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Library hero grave"),
                }
            },
            {
                "Library Rotunda", // The circular room with the ladder
                new List<TunicPortal>
                {
                    new TunicPortal("Library Hall", "", "Library circle down"),
                    new TunicPortal("Library Lab", "", "Library circle up"),
                }
            },
            {
                "Library Lab",
                new List<TunicPortal>
                {
                    new TunicPortal("Library Arena", "", "Library lab ladder"),
                    new TunicPortal("Library Rotunda", "", "Library lab to circle"),
                    new TunicPortal("Transit", "teleporter_library teleporter", "Library Portal"),
                }
            },
            {
                "Library Arena",
                new List<TunicPortal>
                {
                    new TunicPortal("Library Lab", "", "Library Boss Arena exit")
                }
            },
            {
                "East Forest Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Sword Access", "lower", "Forest Hero Grave Lower Entrance"),
                    new TunicPortal("East Forest Redux Laddercave", "upper", "Fox Dance Door"),
                    new TunicPortal("East Forest Redux Interior", "lower", "Guard House 2 Lower Entrance"),
                    new TunicPortal("East Forest Redux Laddercave", "gate", "Guard House 1 Lower Entrance"),
                    new TunicPortal("Sword Access", "upper", "Forest Hero Grave Upper Entrance"),
                    new TunicPortal("East Forest Redux Interior", "upper", "Guard House 2 Upper Entrance"),
                    new TunicPortal("East Forest Redux Laddercave", "lower", "Guard House 1 Lower Entrance"),
                    new TunicPortal("Forest Belltower", "", "East Forest main entry point"),
                    new TunicPortal("Transit", "teleporter_forest teleporter", "East Forest Portal"),
                }
            },
            {
                "East Forest Redux Laddercave", // the place with the two ladders that leads to the boss room
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux", "upper", "Guard House 1 Dance Exit"),
                    new TunicPortal("East Forest Redux", "lower", "Guard House 1 Lower Exit"),
                    new TunicPortal("East Forest Redux", "gate", "Guard House 1 Exit to Upper Forest"),
                    new TunicPortal("Forest Boss Room", "", "Guard House to Boss Room"),
                }
            },
            {
                "Sword Access", // East forest hero grave area
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux", "upper", "Upper Forest Grave Exit"),
                    new TunicPortal("East Forest Redux", "lower", "Lower Forest Grave Exit"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "East Forest Hero Grave"), // There's two of these, one is inactive
                    
                    // new TunicPortal("Forest 1_", "lower", "Portal (1)"),
                    // new TunicPortal("Forest 1_", "", "Portal"),
                    // new TunicPortal("Forest 1_", "upper", "Portal (2)"),
                }
            },
            {
                "East Forest Redux Interior", // Guardhouse 2
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux", "lower", "Guard House 2 Lower Exit"),
                    new TunicPortal("East Forest Redux", "upper", "Guard House 2 Upper Exit"),
                }
            },
            {
                "Forest Boss Room",
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux Laddercave", "", "Forest Boss to Forest"),
                    new TunicPortal("Forest Belltower", "", "Forest Boss to Belltower"),

                    // new TunicPortal("Archipelagos Redux_", "showfloordemo2022", "Portal (2)"),
                }
            },
            {
                "Forest Belltower",
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Courtyard", "", "Forest Bell to Fortress"),
                    new TunicPortal("East Forest Redux", "", "Forest Bell to Forest"),
                    new TunicPortal("Overworld Redux", "", "Forest Bell to Overworld"),
                    new TunicPortal("Forest Boss Room", "", "Forest Bell to Boss"),
                }
            },
            {
                "Fortress Courtyard", // Outside the fortress, the area connected to east forest and overworld
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Reliquary", "Lower", "Lower Fortress Grave Path Entrance"),
                    new TunicPortal("Fortress Reliquary", "Upper", "Upper Fortress Grave Path Entrance"),
                    new TunicPortal("Fortress Main", "Big Door", "Fortress Main Entrance"),
                    new TunicPortal("Fortress East", "", "Fortress Outside to Fortress Mage Area"),
                    new TunicPortal("Fortress Basement", "", "Fortress to Under Fortress outside"),
                    new TunicPortal("Forest Belltower", "", "Fortress to Forest Bell"),
                    new TunicPortal("Overworld Redux", "", "Fortress to Overworld"),
                    new TunicPortal("Shop", "", "Fortress outside shop"),

                    // new TunicPortal("Overworld Redux_", "", "Portal (4)"), // unused and disabled
                }
            },
            {
                "Fortress Basement", // Under the fortress
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Main", "", "Under Fortress to inside"),
                    new TunicPortal("Fortress Courtyard", "", "Under Fortress to outside"),
                }
            },
            {
                "Fortress Main", // Inside the fortress
                new List<TunicPortal>
                {
                    new TunicPortal("Shop", "", "Inside Fortress Shop"),
                    new TunicPortal("Fortress Basement", "", "Fortress inside to under fortress"),
                    new TunicPortal("Fortress Courtyard", "Big Door", "Fortress Main Exit"),
                    new TunicPortal("Fortress Arena", "", "Fortress big gold door"),
                    new TunicPortal("Fortress East", "upper", "Fortress to East Fortress Upper"),
                    new TunicPortal("Fortress East", "lower", "Fortress to East Fortress Lower"),
                }
            },
            {
                "Fortress East", // that tiny area with the couple mages up high, and the ladder in the lower right
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Main", "lower", "East Fortress to Inside Lower"),
                    new TunicPortal("Fortress Courtyard", "", "East Fortress to Courtyard"),
                    new TunicPortal("Fortress Main", "upper", "East Fortress to Inside Upper"),
                }
            },
            {
                "Fortress Reliquary", // Where the grave is
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Courtyard", "Lower", "Bottom Fortress Grave Path Exit"),
                    new TunicPortal("Dusty", "", "Dusty Entrance"),
                    new TunicPortal("Fortress Courtyard", "Upper", "Top Fortress Grave Path Exit"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Fortress Hero Grave"),
                }
            },
            {
                "Fortress Arena", // Where the boss is
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Main", "", "Boss to Fortress"),
                    new TunicPortal("Transit", "teleporter_spidertank", "Fortress Portal"),

                    // new TunicPortal("Fortress Main_", "", "Portal"), // There's two of these?
                }
            },
            {
                "Dusty", // broom
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Reliquary", "", "Dusty Exit"),
                }
            },
            {
                "Mountain",
                new List<TunicPortal>
                {
                    new TunicPortal("Mountaintop", "", "Follow the Golden Path"),
                    new TunicPortal("Quarry Redux", "", "Mountain to Quarry"),
                    new TunicPortal("Overworld Redux", "", "Mountain to Overworld"),
                }
            },
            {
                "Mountaintop",
                new List<TunicPortal>
                {
                    new TunicPortal("Mountain", "", "Top of the Mountain exit"),
                }
            },
            {
                "Darkwoods Tunnel", // tunnel between overworld and quarry
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Quarry Connector to Overworld"),
                    new TunicPortal("Quarry Redux", "", "Quarry Connector to Quarry"),
                }
            },
            {
                "Quarry Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Monastery", "front", "Quarry to Monastery Front"),
                    new TunicPortal("Shop", "", "Quarry Shop"),
                    new TunicPortal("Monastery", "back", "Quarry to Monastery Back"),
                    new TunicPortal("Mountain", "", "Quarry to Mountain"),
                    new TunicPortal("ziggurat2020_0", "", "Zig Entrance"),
                    new TunicPortal("Darkwoods Tunnel", "", "Quarry to Overworld Connector"),
                    new TunicPortal("Transit", "teleporter_quarry teleporter", "Quarry Portal"),
                }
            },
            {
                "Monastery",
                new List<TunicPortal>
                {
                    new TunicPortal("Quarry Redux", "back", "Monastery to Quarry Back"),
                    new TunicPortal("Quarry Redux", "front", "Monastery to Quarry Front"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Quarry Hero Grave"),

                    // new TunicPortal("Quarry_", "lower", "Portal (1)"), // Unused portal, disabled, and very high up
                }
            },
            {
                "ziggurat2020_0", // Zig entrance
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_1", "", "Zig to Zig 1"),
                    new TunicPortal("Quarry Redux", "", "Zig to Quarry"),
                }
            },
            {
                "ziggurat2020_1", // Upper zig
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_3", "zig2_skip", "Zig Skip"), // the elevator skip to lower zig
                    new TunicPortal("ziggurat2020_0", "", "Zig 1 to 0"),
                    new TunicPortal("ziggurat2020_2", "", "Zig 1 to 2"),
                }
            },
            {
                "ziggurat2020_2", // Zig intermediate elevator
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_1", "", "Zig 2 to 1"),
                    new TunicPortal("ziggurat2020_3", "", "Zig 2 to 3"),
                }
            },
            {
                "ziggurat2020_3", // Lower zig
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_FTRoom", "", "Zig Portal Room"), // Prayer portal room
                    new TunicPortal("ziggurat2020_1", "zig2_skip", "Zig Skip Exit"), // the elevator skip to lower zig
                    new TunicPortal("ziggurat2020_2", "", "Zig 3 to Zig 2"),
                }
            },
            {
                "ziggurat2020_FTRoom", // The room with the prayer portal
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_3", "", "Zig Portal Room to Zig"),
                    new TunicPortal("Transit", "teleporter_ziggurat teleporter", "Zig Portal"),
                }
            },
            {
                "Swamp Redux 2",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "conduit", "Bottom Swamp Exit"),
                    new TunicPortal("Cathedral Redux", "main", "Swamp Entrance to Cathedral"),
                    new TunicPortal("Cathedral Redux", "secret", "Swamp to Treasure Room"),
                    new TunicPortal("Cathedral Arena", "", "Swamp to Gauntlet"),
                    new TunicPortal("Shop", "", "Swamp Shop"),
                    new TunicPortal("Overworld Redux", "wall", "Top Swamp Exit"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Swamp Hero Grave"),
                }
            },
            {
                "Cathedral Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Swamp Redux 2", "main", "Cathedral Main Exit"),
                    new TunicPortal("Cathedral Arena", "", "Cathedral Elevator"),
                    new TunicPortal("Swamp Redux 2", "secret", "Cathedral Treasure Exit"),
                }
            },
            {
                "Cathedral Arena", // Gauntlet
                new List<TunicPortal>
                {
                    new TunicPortal("Swamp Redux 2", "", "Gauntlet to Swamp"),
                    new TunicPortal("Cathedral Redux", "", "Gauntlet to Cathedral"),
                    new TunicPortal("Shop", "", "Gauntlet Shop"),
                }
            },
            //{
            //    "Shop", // Every shop is just this region. Need to figure out how it determines where to go back to, since its destination is _
            //    new List<TunicPortal>
            //    {
            //        new TunicPortal("_", "", "Shop Exit Portal"),
            //    }
            //},
            {
                "RelicVoid", // Hero relic area
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Reliquary", "teleporter_relic plinth", "Hero Relic to Fortress"),
                    new TunicPortal("Monastery", "teleporter_relic plinth", "Hero Relic to Monastery"),
                    new TunicPortal("Archipelagos Redux", "teleporter_relic plinth", "Hero Relic to West Garden"),
                    new TunicPortal("Sword Access", "teleporter_relic plinth", "Hero Relic to East Forest"),
                    new TunicPortal("Library Hall", "teleporter_relic plinth", "Hero Relic to Library"),
                    new TunicPortal("Swamp Redux 2", "teleporter_relic plinth", "Hero Relic to Swamp"),
                }
            },
            // can have issues coming to transit -- sometimes it locks up for some reason
            {
                "Transit", // Teleporter hub
                new List<TunicPortal>
                {
                    new TunicPortal("Archipelagos Redux", "teleporter_archipelagos_teleporter", "Transit to West Garden"),
                    new TunicPortal("Library Lab", "teleporter_library teleporter", "Transit to Library"),
                    new TunicPortal("Quarry Redux", "teleporter_quarry teleporter", "Transit to Quarry"),
                    new TunicPortal("East Forest Redux", "teleporter_forest teleporter", "Transit to East Forest"),
                    new TunicPortal("Fortress Arena", "teleporter_spidertank", "Transit to Fortress"),
                    new TunicPortal("Atoll Redux", "teleporter_atoll", "Transit to Atoll"),
                    new TunicPortal("ziggurat2020_FTRoom", "teleporter_ziggurat teleporter", "Transit to Zig"),
                    new TunicPortal("Spirit Arena", "teleporter_spirit arena", "Transit to Heir"),
                    new TunicPortal("Overworld Redux", "teleporter_town", "Transit to Town"),
                    new TunicPortal("Overworld Redux", "teleporter_starting island", "Transit to Spawn"),

                    // new TunicPortal("Transit_", "teleporter_", "Portal"), // Unused portal, far away and not enabled
                }
            },
            {
                "Spirit Arena", // Heir fight
                new List<TunicPortal>
                {
                    new TunicPortal("Transit", "teleporter_spirit arena", "Heir Exit"),
                }
            },
            {
                "Purgatory", // Second save hallway
                new List<TunicPortal>
                {
                    new TunicPortal("Purgatory", "bottom", "Purgatory Top Exit"),
                    new TunicPortal("Purgatory", "top", "Purgatory Bottom Exit"),
                }
            },
        };

        // public static List<string> deadEndNames = new List<string> { "g_elements", "Sword Cave", "EastFiligreeCache", "Overworld Cave", "Maze Room", "Town Basement", "ShopSpecial", "archipelagos_house", "Library Arena", "Dusty", "Mountaintop", "RelicVoid", "Spirit Arena" };
        public static List<string> hallwayNames = new List<string> { "Windmill", "Ruins Passage", "Temple", "Sewer_Boss", "frog cave main", "Library Exterior", "Library Rotunda", "East Forest Interior Redux", "Forest Boss Room", "Fortress Basement", "Darkwoods Tunnel", "ziggurat2020_0", "ziggurat2020_2", "ziggurat2020_FTRoom", "Purgatory" };
        // public static List<string> twoPlusNames = new List<string>();

        // taken from the internet, don't fully understand how it works but as long as it works, whatever
        public static void ShuffleList<T>(IList<T> list, int seed)
        {
            var rng = new System.Random(seed);
            int n = list.Count;

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        // create a list of all portals with their information loaded in, just a slightly expanded version of the above to include destinations
        public static Dictionary<string, PortalCombo> RandomizePortals(int seed)
        {
            Dictionary<string, PortalCombo> RandomizedPortals = new Dictionary<string, PortalCombo>();
            List<string> twoPlusNames = new List<string>();
            RandomizedPortals.Clear();
            twoPlusNames.Clear();
            Logger.LogInfo("randomize portals started");
            List<string> deadEndNames = new List<string> { "g_elements", "Sword Cave", "EastFiligreeCache", "Overworld Cave", "Maze Room", "Town Basement", "ShopSpecial", "archipelagos_house", "Library Arena", "Dusty", "Mountaintop", "RelicVoid", "Spirit Arena" };
            // making a separate lists for portals connected to one, two, or three+ regions, to be populated by the foreach coming up next
            List<Portal> deadEndPortals = new List<Portal>();
            // List<Portal> hallwayPortals = new List<Portal>();
            List<Portal> twoPlusPortals = new List<Portal>();

            // separate the portals into their respective lists
            foreach (KeyValuePair<string, List<TunicPortal>> region_group in PortalList) {
                string region_name = region_group.Key;
                List<TunicPortal> region_portals = region_group.Value;
                // populating twoPlusNames here since we're looping through the list anyway
                if (!deadEndNames.Contains(region_name))
                {
                    twoPlusNames.Add(region_name);
                }
                foreach (TunicPortal portal in region_portals)
                {
                    Portal newPortal = new Portal(portal.Destination, portal.DestinationTag, portal.PortalName, region_name, portal.RequiredItems);
                    if (deadEndNames.Contains(newPortal.Scene))
                    {
                        deadEndPortals.Add(newPortal);
                    }
                    //else if (hallwayNames.Contains(portal.SceneName))
                    //{
                    //    hallwayPortals.Add(newPortal);
                    //}
                    else twoPlusPortals.Add(newPortal);
                }
            }

            // making a list of accessible regions that will be updated as we gain access to more regions
            List<string> accessibleRegions = new List<string>();
            accessibleRegions.Clear();

            // just picking a static start region for now, can modify later if we want to do random start location
            string start_region = "Overworld Redux";
            accessibleRegions.Add(start_region);
            
            int comboNumber = 0;

            // This might be way too much shuffling -- was done to not favor connecting new regions to the first regions added to the list
            // create a portal combo for every region in the threePlusRegions list, so that every region can now be accessed (ignoring rules for now)
            while (accessibleRegions.Count < twoPlusNames.Count)
            {
                ShuffleList(twoPlusPortals, seed);
                // later on, start by making the first several portals into shop portals
                foreach (Portal portal in twoPlusPortals)
                {
                    // find a portal in a region we can't access yet
                    if (!accessibleRegions.Contains(portal.Scene))
                    {
                        Portal portal1 = portal;
                        twoPlusPortals.Remove(portal);
                        Portal portal2 = null;

                        // find a portal in a region we can access
                        ShuffleList(twoPlusPortals, seed);
                        foreach (Portal secondPortal in twoPlusPortals)
                        {
                            if (accessibleRegions.Contains(secondPortal.Scene))
                            {
                                portal2 = secondPortal;
                                break;
                            }
                        }
                        // add the portal combo to the randomized portals list
                        RandomizedPortals.Add(comboNumber.ToString(), new PortalCombo(portal1, portal2));
                        accessibleRegions.Add(portal.Scene);
                        comboNumber++;
                        break;
                    }
                }
            }

            // since the dead ends only have one exit, we just append them 1 to 1 to a random portal in the two plus list
            Logger.LogInfo("successfully added regions to accessible regions and portals to randomized portals list");
            ShuffleList(deadEndPortals, seed);
            ShuffleList(twoPlusPortals, seed);
            while (deadEndPortals.Count > 0)
            {
                comboNumber++;
                RandomizedPortals.Add(comboNumber.ToString(), new PortalCombo(deadEndPortals[0], twoPlusPortals[0]));
                deadEndPortals.RemoveAt(0);
            }
            
            // now we have every region accessible (if we ignore rules -- that's a problem for later)
            // the twoPlusPortals list still has items left in it, so now we pair them off
            while (twoPlusPortals.Count > 1)
            {
                comboNumber++;
                RandomizedPortals.Add(comboNumber.ToString(), new PortalCombo(twoPlusPortals[0], twoPlusPortals[1]));
                twoPlusPortals.RemoveAt(1); // I could do removeat0 twice, but I don't like how that looks
                twoPlusPortals.RemoveAt(0);
            }
            if (twoPlusPortals.Count == 1)
            {
                // if this triggers, increase shop count by one (at least, when we actually have that as a thing later, for now just ignore it)
                Logger.LogInfo("one extra dead end remaining alone, rip. It's " + twoPlusPortals[0].Name);
            }
            return RandomizedPortals;
        }

        // a function to apply the randomized portal list to portals during on scene loaded
        public static void ModifyPortals(Scene loadingScene, Dictionary<string, PortalCombo> portalComboList)
        {
            Logger.LogInfo("current time is " + SpeedrunData.inGameTime);
            var Portals = Resources.FindObjectsOfTypeAll<ScenePortal>();
            foreach (var portal in Portals)
            {
                Logger.LogInfo("portal in world is this " + portal.name + portal.destinationSceneName + portal.id);
                // go through the list of randomized portals and see if either the first or second portal matches the one we're looking at
                foreach (KeyValuePair<string, PortalCombo> portalCombo in portalComboList)
                {
                    string comboTag = portalCombo.Key;
                    Portal portal1 = portalCombo.Value.Portal1;
                    Portal portal2 = portalCombo.Value.Portal2;
                    if (portal1.Scene == loadingScene.name && portal1.Tag == portal.id && portal1.Destination == portal.destinationSceneName)
                    {
                        Logger.LogInfo("portal 1 matched");
                        Logger.LogInfo("current scene is " + loadingScene.name);
                        Logger.LogInfo("portal destination was " + portal.destinationSceneName + "_" + portal.id);
                        Logger.LogInfo("portal destination is now " + portal2.Scene+ "_" + comboTag);
                        Logger.LogInfo("portal 1 is " + portal1.Scene + "_" + portal1.Tag);
                        Logger.LogInfo("portal 2 is " + portal2.Scene + "_" + portal2.Tag);
                        Logger.LogInfo("finished, moving on to next portal");
                        
                        portal.destinationSceneName = portal2.Scene;
                        portal.id = comboTag;
                        portal.optionalIDToSpawnAt = comboTag + comboTag + comboTag + comboTag; // quadrupling since doubling and tripling can have overlaps
                    }

                    if (portal2.Scene == loadingScene.name && portal2.Tag == portal.id && portal2.Destination == portal.destinationSceneName)
                    {
                        Logger.LogInfo("portal 2 matched");
                        Logger.LogInfo("current scene is " + loadingScene.name);
                        Logger.LogInfo("portal destination was " + portal.destinationSceneName + "_" + portal.id);
                        Logger.LogInfo("portal destination is now " + portal1.Scene+ "_" + comboTag);
                        Logger.LogInfo("portal 1 is " + portal1.Scene + "_" + portal1.Tag);
                        Logger.LogInfo("portal 2 is " + portal2.Scene + "_" + portal2.Tag);
                        Logger.LogInfo("finished, moving on to next portal");
                        portal.destinationSceneName = portal1.Scene;
                        portal.id = comboTag + comboTag + comboTag + comboTag; // quadrupling since doubling and tripling can have overlaps
                        portal.optionalIDToSpawnAt = comboTag;
                    }
                }
            }
        }
    }
}
