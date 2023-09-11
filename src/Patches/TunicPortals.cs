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
                    new TunicPortal("Sword Cave_", "", "Sword Cave Entrance"),
                    new TunicPortal("Windmill_", "", "Windmill Entrance"),
                    new TunicPortal("Sewer_", "entrance", "Well Entrance"),
                    new TunicPortal("Sewer_", "west_aqueduct", "Well Rail Left Entrance", new Dictionary<string, int> { {"Aqueduct Rail access", 1} }),
                    new TunicPortal("Overworld Interiors_", "house", "Old House Entry Door", new Dictionary<string, int> { {"old house key", 1}, {"Overworld Redux access", 1} }),
                    new TunicPortal("Overworld Interiors_", "under_checkpoint", "Old House Other Entrance"),
                    new TunicPortal("Furnace_", "gyro_upper_north", "OW Furnace Upper North Entrance"),
                    new TunicPortal("Furnace_", "gyro_upper_east", "OW Furance Upper East Entrance"),
                    new TunicPortal("Furnace_", "gyro_west", "OW Furnace West Entrance"),
                    new TunicPortal("Furnace_", "gyro_lower", "OW Furnace Lower Entrance"),
                    new TunicPortal("Overworld Cave_", "", "Rotating Lights Entrance"),
                    new TunicPortal("Swamp Redux 2_", "wall", "Swamp Upper Entrance"),
                    new TunicPortal("Swamp Redux 2_", "conduit", "Swamp Lower Entrance"),
                    new TunicPortal("Ruins Passage_", "east", "Ruins Hall Entrance Not-door"),
                    new TunicPortal("Ruins Passage_", "west", "Ruins Hall Entrance Door"),
                    new TunicPortal("Atoll Redux_", "upper", "Atoll Upper Entrance"),
                    new TunicPortal("Atoll Redux_", "lower", "Atoll Lower Entrance"),
                    new TunicPortal("ShopSpecial_", "", "Special Shop Entrance"),
                    new TunicPortal("Maze Room_", "", "Maze Entrance"),
                    new TunicPortal("Archipelagos Redux_", "upper", "West Garden Entrance by Belltower"),
                    new TunicPortal("Archipelagos Redux_", "lower", "West Garden Entrance by Dark Tomb"),
                    new TunicPortal("Archipelagos Redux_", "lowest", "West Garden Laurel Entrance"),
                    new TunicPortal("Temple_", "main", "Temple Door Entrance"),
                    new TunicPortal("Temple_", "rafters", "Temple Upper Entrance"),
                    new TunicPortal("Ruined Shop_", "", "Ruined Shop Entrance"),
                    new TunicPortal("PatrolCave_", "", "Patrol Cave Entrance"),
                    new TunicPortal("Town Basement_", "beach", "Hourglass Cave Entrance"),
                    new TunicPortal("Changing Room_", "", "Changing Room Entrance"),
                    new TunicPortal("CubeRoom_", "", "Cube Entrance"),
                    new TunicPortal("Mountain_", "", "Stairs from Overworld to Mountain"),
                    new TunicPortal("Fortress Courtyard_", "", "Fortress Entrance"),
                    new TunicPortal("Town_FiligreeRoom_", "", "HC Room Entrance next to Changing Room"), // ? verify this is the one in the middle
                    new TunicPortal("EastFiligreeCache_", "", "Glass Cannon HC Room Entrance"),
                    new TunicPortal("Darkwoods Tunnel_", "", "Entrance to Quarry Connector"),
                    new TunicPortal("Crypt Redux_", "", "Dark Tomb Entrance"),
                    new TunicPortal("Forest Belltower_", "", "East Forest Entrance"),
                    new TunicPortal("Transit_", "teleporter_town", "Town Portal"),
                    new TunicPortal("Transit_", "teleporter_starting island", "Spawn Portal"),

                    new TunicPortal("Waterfall_", "", "Portal (38)"), // Is this unused?
                    new TunicPortal("_", "", "Portal"), // ?
                    new TunicPortal("Forest Belltower_", "showfloordemo2022", "Portal (12)"), // ?
                    new TunicPortal("DEMO_altEnd_", "", "_Portal (Secret Demo End)"), // ?
                }
            },
            {
                "Windmill",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "", "Windmill Exit"),
                    new TunicPortal("Shop_", "", "Windmill Shop Entrance"),
                }
            },
            {
                "Overworld Interiors", // House in town
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "house", "Exit from Front Door of Old House"),
                    new TunicPortal("g_elements_", "", "Teleport to Secret Treasure Room"),
                    new TunicPortal("Overworld Redux_", "under_checkpoint", "Exit from Old House from not the door"),

                    new TunicPortal("Archipelagos Redux_", "", "_ShowfloorDemo2022 Portal"), // unused and disabled
                }
            },
            {
                "g_elements", // Secret treasure room
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Interiors_", "", "Exit from Secret Treasure Room"),
                }
            },
            {
                "Furnace", // Under the west belltower
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "gyro_upper_north", "Furnace Upper North Exit"),
                    new TunicPortal("Crypt Redux_", "", "Furnace Dark Tomb Exit"),
                    new TunicPortal("Overworld Redux_", "gyro_west", "Furnace West Exit"),
                    new TunicPortal("Overworld Redux_", "gyro_lower", "Furnace Lower Exit"),
                    new TunicPortal("Overworld Redux_", "gyro_upper_east", "Furnace Upper East Exit"),
                }
            },
            {
                "Sword Cave", // Stick house
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "", "Sword Cave Exit"),
                }
            },
            {
                "Ruins Passage", // That little hallway with the key door near the start in Overworld
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "east", "Ruins Passage Door Exit"),
                    new TunicPortal("Overworld Redux_", "west", "Ruins Passage Not-door Exit")
                }
            },
            {
                "EastFiligreeCache", // The holy cross room with the 3 chests near swamp entrance
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "", "Glass Cannon HC Exit"),
                }
            },
            {
                "Overworld Cave", // East beach, next to swamp entrance, rotating lights room
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "", "Rotating Lights Exit")
                }
            },
            {
                "Maze Room", // Invisible maze
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "", "Maze Exit"),
                }
            },
            {
                "Town Basement", // Hourglass cave
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "beach", "Hourglass Exit"), // yes, it has a tag even though it doesn't need one
                }
            },
            {
                "ShopSpecial", // Special shop, laurel across from that platform between east forest and fortress
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "", "Special Shop Exit"),
                }
            },
            {
                "Temple", // Where you put the hexes
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "rafters", "Temple Upper Exit"),
                    new TunicPortal("Overworld Redux_", "main", "Temple Main Exit"),
                }
            },
            {
                "Sewer", // Bottom of the Well
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "entrance", "Well Exit from main entrance"),
                    new TunicPortal("Sewer_Boss_", "", "Well to Well Boss"),
                    new TunicPortal("Overworld Redux_", "west_aqueduct", "Well Rail Exit"),
                }
            },
            {
                "Sewer_Boss", // Boss room in the Bottom of the Well
                new List<TunicPortal>
                {
                    new TunicPortal("Sewer_", "", "Well Boss to Well"),
                    new TunicPortal("Crypt Redux_", "", "Well Boss to Dark Tomb"),
                }
            },
            {
                "Crypt Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "", "Dark Tomb Entrance"),
                    new TunicPortal("Furnace_", "", "Dark Tomb main exit"),
                    new TunicPortal("Sewer_Boss_", "", "Dark Tomb to Well Boss"),
                }
            },
            {
                "Archipelagos Redux", // West Garden
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "lower", "West Garden Exit to Dark Tomb"),
                    new TunicPortal("archipelagos_house_", "", "Magic Dagger House Entrance"),
                    new TunicPortal("Overworld Redux_", "upper", "West Garden after boss"),
                    new TunicPortal("Shop_", "", "West Garden to Shop"), // there's two of these, one is unused and disabled
                    new TunicPortal("Overworld Redux_", "lowest", "West Garden Laurel Exit"),
                    new TunicPortal("RelicVoid_", "teleporter_relic plinth", "West Garden Hero Grave"), // Hero grave
                    new TunicPortal("Transit_", "teleporter_archipelagos_teleporter", "West Garden Portal"), // Portal to the thing behind dagger house
                }
            },
            {
                "archipelagos_house", // Magic Dagger house in West Garden
                new List<TunicPortal>
                {
                    new TunicPortal("Archipelagos Redux_", "", "Magic Dagger House Exit"),
                }
            },
            {
                "Atoll Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Frog Stairs_", "eye", "Frog Eye Entrance"),
                    new TunicPortal("Library Exterior_", "", "Atoll to Library"),
                    new TunicPortal("Overworld Redux_", "upper", "Upper Atoll Exit"),
                    new TunicPortal("Overworld Redux_", "lower", "Lower Atoll Exit"),
                    new TunicPortal("Frog Stairs_", "mouth", "Frog Mouth Entrance"),
                    new TunicPortal("Shop_", "", "Atoll Shop"),
                    new TunicPortal("Transit_", "teleporter_atoll", "Atoll Portal"),
                    new TunicPortal("Forest Lake_", "teleporter", "Portal"), // Unused portal, same spot as library portal
                }
            },
            {
                "Frog Stairs", // Entrance to frog's domain
                new List<TunicPortal>
                {
                    new TunicPortal("Atoll Redux_", "mouth", "Frog Mouth Exit"),
                    new TunicPortal("frog cave main_", "Exit", "Upper Frog to lower frog exit"),
                    new TunicPortal("Atoll Redux_", "eye", "Frog Eye Exit"),
                    new TunicPortal("frog cave main_", "Entrance", "Upper frog to lower frog entrance"),
                }
            },
            {
                "frog cave main", // Frog's domain, yes it's lowercase
                new List<TunicPortal>
                {
                    new TunicPortal("Frog Stairs_", "Exit", "Lower frog exit exit"),
                    new TunicPortal("Frog Stairs_", "Entrance", "Lower frog entrance exit"),
                }
            },
            {
                "Library Exterior",
                new List<TunicPortal>
                {
                    new TunicPortal("Library Hall_", "", "Library entry ladder"),
                    new TunicPortal("Atoll Redux_", "", "Can't go to library with no hook or laurels dummy"),
                }
            },
            {
                "Library Hall", // Entry area with hero grave
                new List<TunicPortal>
                {
                    new TunicPortal("Library Rotunda_", "", "Library entrance to circle room"),
                    new TunicPortal("Library Exterior_", "", "Library Bookshelf"),
                    new TunicPortal("RelicVoid_", "teleporter_relic plinth", "Library hero grave"),
                }
            },
            {
                "Library Rotunda", // The circular room with the ladder
                new List<TunicPortal>
                {
                    new TunicPortal("Library Hall_", "", "Library circle down"),
                    new TunicPortal("Library Lab_", "", "Library circle up"),
                }
            },
            {
                "Library Lab",
                new List<TunicPortal>
                {
                    new TunicPortal("Library Arena_", "", "Library lab ladder"),
                    new TunicPortal("Library Rotunda_", "", "Library lab to circle"),
                    new TunicPortal("Transit_", "teleporter_library teleporter", "Library Portal"),
                }
            },
            {
                "Library Arena",
                new List<TunicPortal>
                {
                    new TunicPortal("Library Lab_", "", "Library Boss Arena exit")
                }
            },
            {
                "East Forest Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Sword Access_", "lower", "Forest Hero Grave Lower Entrance"),
                    new TunicPortal("East Forest Redux Laddercave_", "upper", "Fox Dance Door"),
                    new TunicPortal("East Forest Redux Interior_", "lower", "Guard House 2 Lower Entrance"),
                    new TunicPortal("East Forest Redux Laddercave_", "gate", "Guard House 1 Lower Entrance"),
                    new TunicPortal("Sword Access_", "upper", "Forest Hero Grave Upper Entrance"),
                    new TunicPortal("East Forest Redux Interior_", "upper", "Guard House 2 Upper Entrance"),
                    new TunicPortal("East Forest Redux Laddercave_", "lower", "Guard House 1 Lower Entrance"),
                    new TunicPortal("Forest Belltower_", "", "East Forest main entry point"),
                    new TunicPortal("Transit_", "teleporter_forest teleporter", "East Forest Portal"),
                }
            },
            {
                "East Forest Redux Laddercave", // the place with the two ladders that leads to the boss room
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux_", "upper", "Guard House 1 Dance Exit"),
                    new TunicPortal("East Forest Redux_", "lower", "Guard House 1 Lower Exit"),
                    new TunicPortal("East Forest Redux_", "gate", "Guard House 1 Exit to Upper Forest"),
                    new TunicPortal("Forest Boss Room_", "", "Guard House to Boss Room"),
                }
            },
            {
                "Sword Access", // East forest hero grave area
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux_", "upper", "Upper Forest Grave Exit"),
                    new TunicPortal("East Forest Redux_", "lower", "Lower Forest Grave Exit"),
                    new TunicPortal("RelicVoid_", "teleporter_relic plinth", "East Forest Hero Grave"), // There's two of these, one is inactive
                    
                    new TunicPortal("Forest 1_", "lower", "Portal (1)"),
                    new TunicPortal("Forest 1_", "", "Portal"),
                    new TunicPortal("Forest 1_", "upper", "Portal (2)"),
                }
            },
            {
                "East Forest Redux Interior", // Guardhouse 2
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux_", "lower", "Guard House 2 Lower Exit"),
                    new TunicPortal("East Forest Redux_", "upper", "Guard House 2 Upper Exit"),
                }
            },
            {
                "Forest Boss Room",
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux Laddercave_", "", "Forest Boss to Forest"),
                    new TunicPortal("Forest Belltower_", "", "Forest Boss to Belltower"),

                    new TunicPortal("Archipelagos Redux_", "showfloordemo2022", "Portal (2)"),
                }
            },
            {
                "Forest Belltower",
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Courtyard_", "", "Forest Bell to Fortress"),
                    new TunicPortal("East Forest Redux_", "", "Forest Bell to Forest"),
                    new TunicPortal("Overworld Redux_", "", "Forest Bell to Overworld"),
                    new TunicPortal("Forest Boss Room_", "", "Forest Bell to Boss"),
                }
            },
            {
                "Fortress Courtyard", // Outside the fortress, the area connected to east forest and overworld
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Reliquary_", "Lower", "Lower Fortress Grave Path Entrance"),
                    new TunicPortal("Fortress Reliquary_", "Upper", "Upper Fortress Grave Path Entrance"),
                    new TunicPortal("Fortress Main_", "Big Door", "Fortress Main Entrance"),
                    new TunicPortal("Fortress East_", "", "Fortress Outside to Fortress Mage Area"),
                    new TunicPortal("Fortress Basement_", "", "Fortress to Under Fortress outside"),
                    new TunicPortal("Forest Belltower_", "", "Fortress to Forest Bell"),
                    new TunicPortal("Overworld Redux_", "", "Fortress to Overworld"),
                    new TunicPortal("Shop_", "", "Fortress outside shop"),

                    // new TunicPortal("Overworld Redux_", "", "Portal (4)"), // unused and disabled
                }
            },
            {
                "Fortress Basement", // Under the fortress
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Main_", "", "Under Fortress to inside"),
                    new TunicPortal("Fortress Courtyard_", "", "Under Fortress to outside"),
                }
            },
            {
                "Fortress Main", // Inside the fortress
                new List<TunicPortal>
                {
                    new TunicPortal("Shop_", "", "Inside Fortress Shop"),
                    new TunicPortal("Fortress Basement_", "", "Fortress inside to under fortress"),
                    new TunicPortal("Fortress Courtyard_", "Big Door", "Fortress Main Exit"),
                    new TunicPortal("Fortress Arena_", "", "Fortress big gold door"),
                    new TunicPortal("Fortress East_", "upper", "Fortress to East Fortress Upper"),
                    new TunicPortal("Fortress East_", "lower", "Fortress to East Fortress Lower"),
                }
            },
            {
                "Fortress East", // that tiny area with the couple mages up high, and the ladder in the lower right
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Main_", "lower", "East Fortress to Inside Lower"),
                    new TunicPortal("Fortress Courtyard_", "", "East Fortress to Courtyard"),
                    new TunicPortal("Fortress Main_", "upper", "East Fortress to Inside Upper"),
                }
            },
            {
                "Fortress Reliquary", // Where the grave is
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Courtyard_", "Lower", "Bottom Fortress Grave Path Exit"),
                    new TunicPortal("Dusty_", "", "Dusty Entrance"),
                    new TunicPortal("Fortress Courtyard_", "Upper", "Top Fortress Grave Path Exit"),
                    new TunicPortal("RelicVoid_", "teleporter_relic plinth", "Fortress Hero Grave"),
                }
            },
            {
                "Fortress Arena", // Where the boss is
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Main_", "", "Boss to Fortress"),
                    new TunicPortal("Transit_", "teleporter_spidertank", "Fortress Portal"),

                    // new TunicPortal("Fortress Main_", "", "Portal"), // There's two of these?
                }
            },
            {
                "Dusty", // broom
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Reliquary_", "", "Dusty Exit"),
                }
            },
            {
                "Mountain",
                new List<TunicPortal>
                {
                    new TunicPortal("Mountaintop_", "", "Follow the Golden Path"),
                    new TunicPortal("Quarry Redux_", "", "Mountain to Quarry"),
                    new TunicPortal("Overworld Redux_", "", "Mountain to Overworld"),
                }
            },
            {
                "Mountaintop",
                new List<TunicPortal>
                {
                    new TunicPortal("Mountain_", "", "Top of the Mountain exit"),
                }
            },
            {
                "Darkwoods Tunnel", // tunnel between overworld and quarry
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "", "Quarry Connector to Overworld"),
                    new TunicPortal("Quarry Redux_", "", "Quarry Connector to Quarry"),
                }
            },
            {
                "Quarry Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Monastery_", "front", "Quarry to Monastery Front"),
                    new TunicPortal("Shop_", "", "Quarry Shop"),
                    new TunicPortal("Monastery_", "back", "Quarry to Monastery Back"),
                    new TunicPortal("Mountain_", "", "Quarry to Mountain"),
                    new TunicPortal("ziggurat2020_0_", "", "Zig Entrance"),
                    new TunicPortal("Darkwoods Tunnel_", "", "Quarry to Overworld Connector"),
                    new TunicPortal("Transit_", "teleporter_quarry teleporter", "Quarry Portal")
                }
            },
            {
                "Monastery",
                new List<TunicPortal>
                {
                    new TunicPortal("Quarry Redux_", "back", "Monastery to Quarry Back"),
                    new TunicPortal("Quarry Redux_", "front", "Monastery to Quarry Front"),
                    new TunicPortal("RelicVoid_", "teleporter_relic plinth", "Quarry Hero Grave"),

                    // new TunicPortal("Quarry_", "lower", "Portal (1)"), // Unused portal, disabled, and very high up
                }
            },
            {
                "ziggurat2020_0", // Zig entrance
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_1_", "", "Zig to Zig 1"),
                    new TunicPortal("Quarry Redux_", "", "Zig to Quarry"),
                }
            },
            {
                "ziggurat2020_1", // Upper zig
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_3_", "zig2_skip", "Zig Skip"), // the elevator skip to lower zig
                    new TunicPortal("ziggurat2020_0_", "", "Zig 1 to 0"),
                    new TunicPortal("ziggurat2020_2_", "", "Zig 1 to 2"),
                }
            },
            {
                "ziggurat2020_2", // Zig intermediate elevator
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_1_", "", "Zig 2 to 1"),
                    new TunicPortal("ziggurat2020_3_", "", "Zig 2 to 3"),
                }
            },
            {
                "ziggurat2020_3", // Lower zig
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_FTRoom_", "", "Zig Portal Room"), // Prayer portal room
                    new TunicPortal("ziggurat2020_1_", "zig2_skip", "Zig Skip Exit"), // the elevator skip to lower zig
                    new TunicPortal("ziggurat2020_2_", "", "Zig 3 to Zig 2"),
                }
            },
            {
                "ziggurat2020_FTRoom", // The room with the prayer portal
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_3_", "", "Zig Portal Room to Zig"),
                    new TunicPortal("Transit_", "teleporter_ziggurat teleporter", "Zig Portal"),
                }
            },
            {
                "Swamp Redux 2",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "conduit", "Bottom Swamp Exit"),
                    new TunicPortal("Cathedral Redux_", "main", "Swamp Entrance to Cathedral"),
                    new TunicPortal("Cathedral Redux_", "secret", "Swamp to Treasure Room"),
                    new TunicPortal("Cathedral Arena_", "", "Swamp to Gauntlet"),
                    new TunicPortal("Shop_", "", "Swamp Shop"),
                    new TunicPortal("Overworld Redux_", "wall", "Top Swamp Exit"),
                    new TunicPortal("RelicVoid_", "teleporter_relic plinth", "Swamp Hero Grave"),
                }
            },
            {
                "Cathedral Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Swamp Redux 2_", "main", "Cathedral Main Exit"),
                    new TunicPortal("Cathedral Arena_", "", "Cathedral Elevator"),
                    new TunicPortal("Swamp Redux 2_", "secret", "Cathedral Treasure Exit"),
                }
            },
            {
                "Cathedral Arena", // Gauntlet
                new List<TunicPortal>
                {
                    new TunicPortal("Swamp Redux 2_", "", "Gauntlet to Swamp"),
                    new TunicPortal("Cathedral Redux_", "", "Gauntlet to Cathedral"),
                    new TunicPortal("Shop_", "", "Gauntlet Shop"),
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
                    new TunicPortal("Fortress Reliquary_", "teleporter_relic plinth", "Hero Relic to Fortress"),
                    new TunicPortal("Monastery_", "teleporter_relic plinth", "Hero Relic to Monastery"),
                    new TunicPortal("Archipelagos Redux_", "teleporter_relic plinth", "Hero Relic to West Garden"),
                    new TunicPortal("Sword Access_", "teleporter_relic plinth", "Hero Relic to East Forest"),
                    new TunicPortal("Library Hall_", "teleporter_relic plinth", "Hero Relic to Library"),
                    new TunicPortal("Swamp Redux 2_", "teleporter_relic plinth", "Hero Relic to Swamp"),
                }
            },
            {
                "Transit", // Teleporter hub
                new List<TunicPortal>
                {
                    new TunicPortal("Archipelagos Redux_", "teleporter_archipelagos_teleporter", "Transit to West Garden"),
                    new TunicPortal("Library Lab_", "teleporter_library teleporter", "Transit to Library"),
                    new TunicPortal("Quarry Redux_", "teleporter_quarry teleporter", "Transit to Quarry"),
                    new TunicPortal("East Forest Redux_", "teleporter_forest teleporter", "Transit to East Forest"),
                    new TunicPortal("Fortress Arena_", "teleporter_spidertank", "Transit to Fortress"),
                    new TunicPortal("Atoll Redux_", "teleporter_atoll", "Transit to Atoll"),
                    new TunicPortal("ziggurat2020_FTRoom_", "teleporter_ziggurat teleporter", "Transit to Zig"),
                    new TunicPortal("Spirit Arena_", "teleporter_spirit arena", "Transit to Heir"),
                    new TunicPortal("Overworld Redux_", "teleporter_town", "Transit to Town"),
                    new TunicPortal("Overworld Redux_", "teleporter_starting island", "Transit to Spawn"),

                    // new TunicPortal("Transit_", "teleporter_", "Portal"), // Unused portal, far away and not enabled
                }
            },
            {
                "Spirit Arena", // Heir fight
                new List<TunicPortal>
                {
                    new TunicPortal("Transit_", "teleporter_spirit arena", "Heir Exit"),
                }
            },
            {
                "Purgatory", // Second save hallway
                new List<TunicPortal>
                {
                    new TunicPortal("Purgatory_", "bottom", "Purgatory Top Exit"),
                    new TunicPortal("Purgatory_", "top", "Purgatory Bottom Exit"),
                }
            },
        };
        public static List<List<Portal>> PortalsList = new List<List<Portal>>();
        public static Dictionary<string, PortalCombo> RandomizedPortals = new Dictionary<string, PortalCombo>();
        
        public static List<string> deadEndNames = new List<string> {"g_elements", "Sword Cave", "EastFiligreeCache", "Overworld Cave", "Maze Room", "Town Basement", "ShopSpecial", "archipelagos_house", "Library Arena", "Dusty", "Mountaintop", "RelicVoid", "Spirit Arena" };
        public static List<string> hallwayNames = new List<string> { "Windmill", "Ruins Passage", "Temple", "Sewer_Boss", "frog cave main", "Library Exterior", "Library Rotunda", "East Forest Interior Redux", "Forest Boss Room", "Fortress Basement", "Darkwoods Tunnel", "ziggurat2020_0", "ziggurat2020_2", "ziggurat2020_FTRoom", "Purgatory" };
        public static List<string> twoPlusNames = new List<string>();

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
            Logger.LogInfo("randomize portals started");
            PortalList.Clear();

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
                    if (deadEndNames.Contains(portal.SceneName))
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
            ShuffleList(deadEndPortals, seed);
            ShuffleList(twoPlusPortals, seed);
            while (deadEndPortals.Count > 0)
            {
                RandomizedPortals.Add(comboNumber.ToString(), new PortalCombo(deadEndPortals[0], twoPlusPortals[0]));
                comboNumber++;
            }
            
            // now we have every region accessible (if we ignore rules -- that's a problem for later)
            // the twoPlusPortals list still has items left in it, so now we pair them off
            while (twoPlusPortals.Count > 1)
            {
                RandomizedPortals.Add(comboNumber.ToString(), new PortalCombo(twoPlusPortals[0], twoPlusPortals[1]));
            }
            if (twoPlusPortals.Count == 1)
            {
                // ya dun fucked up
                Logger.LogInfo("one extra dead end remaining alone, rip. It's " + twoPlusPortals[0].Name);
            }
            return RandomizedPortals;
        }

        // a function to apply the randomized portal list to portals during on scene loaded
        public static void ModifyPortals(Scene loadingScene, Dictionary<string, PortalCombo> portalComboList)
        {
            var Portals = Resources.FindObjectsOfTypeAll<ScenePortal>();
            foreach (var portal in Portals)
            {
                Logger.LogInfo("portal in world is this " + portal.name + portal.destinationSceneName + portal.id);
                foreach (KeyValuePair<string, PortalCombo> portalCombo in portalComboList)
                {
                    string comboTag = portalCombo.Key;
                    Portal portal1 = portalCombo.Value.Portal1;
                    Portal portal2 = portalCombo.Value.Portal2;

                    Logger.LogInfo("portal1 is " + portal1.Name);
                    Logger.LogInfo("portal2 is " + portal2.Name);

                    if (portal1.Scene == loadingScene.name)
                    {
                        portal.destinationSceneName = portal2.Scene;
                        portal.id = comboTag;
                        portal.optionalIDToSpawnAt = comboTag + comboTag + comboTag; // tripling since doubling can have overlaps
                    }

                    if (portal2.Scene == loadingScene.name)
                    {
                        portal.destinationSceneName = portal1.Scene;
                        portal.id = comboTag + comboTag + comboTag; // tripling since doubling can have overlaps
                        portal.optionalIDToSpawnAt = comboTag;
                    }
                }
            }
        }
    }
}
