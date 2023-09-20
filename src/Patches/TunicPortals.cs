﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;
using BepInEx.Logging;
using Lib;
using HarmonyLib;
using JetBrains.Annotations;
using System.Globalization;

namespace TunicRandomizer
{
    public class TunicPortals {
        private static ManualLogSource Logger = TunicRandomizer.Logger;

        public class TunicPortal
        {
            public string SceneName; // the scene the portal is in
            public string Destination; // the vanilla destination scene
            public string DestinationTag; // the vanilla destination tag
            public string PortalName; // a human-readable name for the portal
            public Dictionary<string, int> RequiredItems; // required items if there is only one item or set of items required. A FullID for a portal or a scene name counts as an item.
            public List<Dictionary<string, int>> RequiredItemsOr; // required items if there are multiple different possible requirements. A FullID for a portal or a scene name counts as an item.
            public List<string> GivesAccess; // portals that are given access to by this portal. If empty, then it defaults to giving access to the center of the region
            public Dictionary<string, int> EntryItems; // portals that require some item(s) to enter. To be used when these entry items don't block you from getting to the center of the region (hero's grave, prayer pad, etc.)
            public bool IsDeadEnd; // portals that are dead ends but share the region with other portals, like the gauntlet lower entry or zig 2 from behind
            public bool PrayerPortal; // portals that require prayer to enter
            public bool OneWay; // portals that are one-way, such as the back entrance to monastery and the forest belltower top portal
            public bool CantReach; // portals that cannot reach the center of the region

            public TunicPortal() { }

            public TunicPortal(string destination, string destinationTag, string portalName)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, bool prayerPortal = false, bool isDeadEnd = false, bool oneWay = false, bool cantReach = false)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                PrayerPortal = prayerPortal;
                IsDeadEnd = isDeadEnd;
                OneWay = oneWay;
                CantReach = cantReach;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, Dictionary<string, int> entryItems, bool prayerPortal = false, bool isDeadEnd = false, bool oneWay = false, bool cantReach = false)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                EntryItems = entryItems;
                PrayerPortal = prayerPortal;
                IsDeadEnd = isDeadEnd;
                OneWay = oneWay;
                CantReach = cantReach;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, Dictionary<string, int> requiredItems, bool prayerPortal = false, bool isDeadEnd = false, bool cantReach = false)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                RequiredItems = requiredItems;
                PrayerPortal = prayerPortal;
                IsDeadEnd = isDeadEnd;
                CantReach = cantReach;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, List<string> givesAccess, bool cantReach = false)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GivesAccess = givesAccess;
                CantReach = cantReach;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, Dictionary<string, int> requiredItems)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                RequiredItems = requiredItems;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, List<Dictionary<string, int>> requiredItemsOr, bool prayerPortal = false, bool cantReach = false)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                RequiredItemsOr = requiredItemsOr;
                PrayerPortal = prayerPortal;
                CantReach = cantReach;
            }

            public TunicPortal(string destination, string destinationTag, string portalName, Dictionary<string, int> requiredItems, List<string> givesAccess)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                RequiredItems = requiredItems;
                GivesAccess = givesAccess;
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
                    new TunicPortal("Sewer", "entrance", "Well Main Entrance"),
                    new TunicPortal("Sewer", "west_aqueduct", "Entrance to Well from Well Rail", cantReach: true, givesAccess: new List<string> { "Furnace_gyro_upper_north" }),
                    new TunicPortal("Overworld Interiors", "house", "Old House Entry Door", requiredItems: new Dictionary<string, int> { {"old house key", 1} }), // make this match actual item name
                    new TunicPortal("Overworld Interiors", "under_checkpoint", "Old House Other Entrance"),
                    new TunicPortal("Furnace", "gyro_upper_north", "Entrance to Furnace from Well Rail", cantReach: true, givesAccess : new List<string> { "Sewer_west_aqueduct" }),
                    new TunicPortal("Furnace", "gyro_upper_east", "Entrance to Furnace from Windmill"),
                    new TunicPortal("Furnace", "gyro_west", "Entrance to Furnace from West Garden"),
                    new TunicPortal("Furnace", "gyro_lower", "Entrance to Furnace from Beach"),
                    new TunicPortal("Overworld Cave", "", "Rotating Lights Entrance"),
                    new TunicPortal("Swamp Redux 2", "wall", "Swamp Upper Entrance", requiredItems: new Dictionary<string, int> { {"laurels", 1} }),
                    new TunicPortal("Swamp Redux 2", "conduit", "Swamp Lower Entrance"),
                    new TunicPortal("Ruins Passage", "east", "Ruins Hall Entrance Not-door"),
                    new TunicPortal("Ruins Passage", "west", "Ruins Hall Entrance Door", requiredItems: new Dictionary<string, int> { { "normal key", 2 } }), // and access to any overworld portal, but we start in overworld so no need to put it here
                    new TunicPortal("Atoll Redux", "upper", "Atoll Upper Entrance"),
                    new TunicPortal("Atoll Redux", "lower", "Atoll Lower Entrance"),
                    new TunicPortal("ShopSpecial", "", "Special Shop Entrance", requiredItems: new Dictionary<string, int> { {"laurels", 1} }),
                    new TunicPortal("Maze Room", "", "Maze Entrance"),
                    new TunicPortal("Archipelagos Redux", "upper", "West Garden Entrance by Belltower", requiredItems: new Dictionary<string, int> { { "laurels", 1 } }),
                    new TunicPortal("Archipelagos Redux", "lower", "West Garden Entrance by Dark Tomb", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "laurels", 1 } }, new Dictionary<string, int> { { "Overworld Redux, Furnace_gyro_west", 1 } } }),
                    new TunicPortal("Archipelagos Redux", "lowest", "West Garden Laurel Entrance", requiredItems: new Dictionary<string, int> { { "laurels", 1 } }),
                    new TunicPortal("Temple", "main", "Temple Door Entrance", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Forest Belltower, Forest Boss Room_", 1 }, { "Overworld Redux, Archipelagos Redux_upper", 1 }, { "stick", 1 } }, new Dictionary<string, int> { { "Forest Belltower, Forest Boss Room_", 1 }, { "Overworld Redux, Archipelagos Redux_upper", 1 }, { "wand", 1 } } }), // technically laurels too
                    new TunicPortal("Temple", "rafters", "Temple Upper Entrance"),
                    new TunicPortal("Ruined Shop", "", "Ruined Shop Entrance"),
                    new TunicPortal("PatrolCave", "", "Patrol Cave Entrance"),
                    new TunicPortal("Town Basement", "beach", "Hourglass Cave Entrance"),
                    new TunicPortal("Changing Room", "", "Changing Room Entrance"),
                    new TunicPortal("CubeRoom", "", "Cube Entrance"),
                    new TunicPortal("Mountain", "", "Stairs from Overworld to Mountain"),
                    new TunicPortal("Fortress Courtyard", "", "Overworld to Fortress"),
                    new TunicPortal("Town_FiligreeRoom", "", "HC Room Entrance next to Changing Room", entryItems: new Dictionary<string, int> { { "holy cross", 1 } }),
                    new TunicPortal("EastFiligreeCache", "", "Glass Cannon HC Room Entrance", entryItems: new Dictionary<string, int> { { "holy cross", 1 } }),
                    new TunicPortal("Darkwoods Tunnel", "", "Entrance to Quarry Connector"),
                    new TunicPortal("Crypt Redux", "", "Dark Tomb Entrance"),
                    new TunicPortal("Forest Belltower", "", "East Forest Entrance"),
                    new TunicPortal("Transit", "teleporter_town", "Town Portal", prayerPortal: true),
                    new TunicPortal("Transit", "teleporter_starting island", "Spawn Portal", prayerPortal: true),
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
                    new TunicPortal("Overworld Redux", "", "Waterfall exit", isDeadEnd: true),
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
                    new TunicPortal("Overworld Redux", "under_checkpoint", "Exit from Old House from not the door", requiredItems: new Dictionary<string, int> { { "Overworld Interiors, Overworld Redux_house", 1 } }), // since you get access to the center of a region from either portal, only one of these two is actually needed

                    // new TunicPortal("Archipelagos Redux_", "", "_ShowfloorDemo2022 Portal"), // unused and disabled
                }
            },
            {
                "g_elements", // Secret treasure room
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Interiors", "", "Exit from Secret Treasure Room", isDeadEnd: true),
                }
            },
            {
                "Furnace", // Under the west belltower
                // I'm calling the "center" of this region the space accessible by the windmill and beach
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "gyro_upper_north", "Furnace to Well Rail", requiredItems: new Dictionary<string, int> { {"laurels", 1} }),
                    new TunicPortal("Crypt Redux", "", "Furnace to Dark Tomb", requiredItems: new Dictionary<string, int> { {"laurels", 1} }, givesAccess: new List<string> {"Overworld Redux_gyro_west"}),
                    new TunicPortal("Overworld Redux", "gyro_west", "Furnace to West Garden", requiredItems : new Dictionary<string, int> { {"laurels", 1} }, givesAccess : new List<string> {"Crypt Redux_"}),
                    new TunicPortal("Overworld Redux", "gyro_lower", "Furnace to Beach"),
                    new TunicPortal("Overworld Redux", "gyro_upper_east", "Furnace to Windmill"),
                }
            },
            {
                "Sword Cave", // Stick house
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Sword Cave Exit", isDeadEnd: true),
                }
            },
            {
                "Ruins Passage", // That little hallway with the key door near the start in Overworld
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "east", "Ruins Passage Door Exit"),
                    new TunicPortal("Overworld Redux", "west", "Ruins Passage Not-door Exit"),
                }
            },
            {
                "EastFiligreeCache", // The holy cross room with the 3 chests near swamp entrance
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Glass Cannon HC Exit", isDeadEnd : true),
                }
            },
            {
                "Overworld Cave", // East beach, next to swamp entrance, rotating lights room
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Rotating Lights Exit", isDeadEnd: true),
                }
            },
            {
                "Maze Room", // Invisible maze
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Maze Exit", isDeadEnd: true),
                }
            },
            {
                "Town Basement", // Hourglass cave
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "beach", "Hourglass Exit", isDeadEnd: true), // yes, it has a tag even though it doesn't need one
                }
            },
            {
                "ShopSpecial", // Special shop, laurel across from that platform between east forest and fortress
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Special Shop Exit", isDeadEnd: true),
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
                    new TunicPortal("Crypt Redux", "", "Well Boss to Dark Tomb", requiredItems: new Dictionary<string, int> { { "Sewer_Boss, Crypt Redux_", 1 } }),
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
                    new TunicPortal("Overworld Redux", "upper", "West Garden after boss", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "sword", 1 } }, new Dictionary<string, int> { { "laurels", 1 } } }),
                    new TunicPortal("Shop", "", "West Garden to Shop"), // there's two of these, one is unused and disabled
                    new TunicPortal("Overworld Redux", "lowest", "West Garden Laurel Exit"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "West Garden Hero Grave", prayerPortal: true), // Hero grave
                    new TunicPortal("Transit", "teleporter_archipelagos_teleporter", "West Garden Portal", prayerPortal: true, isDeadEnd: true), // Portal to the thing behind dagger house
                }
            },
            {
                "archipelagos_house", // Magic Dagger house in West Garden
                new List<TunicPortal>
                {
                    new TunicPortal("Archipelagos Redux", "", "Magic Dagger House Exit", isDeadEnd: true),
                }
            },
            {
                "Atoll Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Frog Stairs", "eye", "Frog Eye Entrance"),
                    new TunicPortal("Library Exterior", "", "Atoll to Library", prayerPortal : true),
                    new TunicPortal("Overworld Redux", "upper", "Upper Atoll Exit"),
                    new TunicPortal("Overworld Redux", "lower", "Lower Atoll Exit", isDeadEnd: true),
                    new TunicPortal("Frog Stairs", "mouth", "Frog Mouth Entrance", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "orb", 1 } }, new Dictionary<string, int> { { "laurels", 1 } } }),
                    new TunicPortal("Shop", "", "Atoll Shop"),
                    new TunicPortal("Transit", "teleporter_atoll", "Atoll Portal", prayerPortal: true),
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
                    new TunicPortal("Frog Stairs", "Exit", "Lower frog exit exit", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "laurels", 1 } }, new Dictionary<string, int> { { "orb", 1} } }),
                    new TunicPortal("Frog Stairs", "Entrance", "Lower frog entrance exit"),
                }
            },
            {
                "Library Exterior",
                new List<TunicPortal>
                {
                    new TunicPortal("Library Hall", "", "Library entry ladder", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "laurels", 1 } }, new Dictionary<string, int> { { "orb", 1} } }),
                    new TunicPortal("Atoll Redux", "", "Can't go to library with no hook or laurels dummy", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "laurels", 1 } }, new Dictionary<string, int> { { "orb", 1} } }),
                }
            },
            {
                "Library Hall", // Entry area with hero grave
                new List<TunicPortal>
                {
                    new TunicPortal("Library Rotunda", "", "Library entrance to circle room"),
                    new TunicPortal("Library Exterior", "", "Library Bookshelf"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Library hero grave", prayerPortal: true),
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
                    new TunicPortal("Library Rotunda", "", "Library lab to circle", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "laurels", 1 } }, new Dictionary<string, int> { { "orb", 1} } }),
                    new TunicPortal("Transit", "teleporter_library teleporter", "Library Portal", prayerPortal : true),
                }
            },
            {
                "Library Arena",
                new List<TunicPortal>
                {
                    new TunicPortal("Library Lab", "", "Library Boss Arena exit", isDeadEnd: true),
                }
            },
            {
                "East Forest Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Sword Access", "lower", "Forest Hero Grave Lower Entrance"),
                    new TunicPortal("East Forest Redux Laddercave", "upper", "Fox Dance Door outside", isDeadEnd: true),
                    new TunicPortal("East Forest Redux Interior", "lower", "Guard House 2 Lower Entrance"),
                    new TunicPortal("East Forest Redux Laddercave", "gate", "Guard House 1 Gate Entrance"),
                    new TunicPortal("Sword Access", "upper", "Forest Hero Grave Upper Entrance"),
                    new TunicPortal("East Forest Redux Interior", "upper", "Guard House 2 Upper Entrance"),
                    new TunicPortal("East Forest Redux Laddercave", "lower", "Guard House 1 Lower Entrance"),
                    new TunicPortal("Forest Belltower", "", "East Forest main entry point"),
                    new TunicPortal("Transit", "teleporter_forest teleporter", "East Forest Portal", prayerPortal: true),
                }
            },
            {
                "East Forest Redux Laddercave", // the place with the two ladders that leads to the boss room
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux", "upper", "Guard House 1 Dance Exit", givesAccess: new List<string> { "East Forest Redux_upper" }, requiredItems: new Dictionary<string, int> { { "East Forest Redux Laddercave, East Forest Redux_gate", 1 } }), // making the upper ones the "center" for easier logic writing
                    new TunicPortal("East Forest Redux", "lower", "Guard House 1 Lower Exit", givesAccess: new List<string> { "East Forest Redux_lower" }, requiredItems: new Dictionary<string, int> { { "East Forest Redux Laddercave, East Forest Redux_gate", 1 } }),
                    new TunicPortal("East Forest Redux", "gate", "Guard House 1 Exit to Upper Forest"),
                    new TunicPortal("Forest Boss Room", "", "Guard House to Boss Room"),
                }
            },
            {
                "Sword Access", // East forest hero grave area
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux", "upper", "Upper Forest Grave Exit", requiredItems: new Dictionary<string, int> { { "laurels", 1 } }),
                    new TunicPortal("East Forest Redux", "lower", "Lower Forest Grave Exit"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "East Forest Hero Grave", prayerPortal: true, requiredItems: new Dictionary<string, int> { {"Sword Access, East Forest Redux_lower", 1 } }), // Can't open the gate from behind
                    
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
                    new TunicPortal("East Forest Redux Laddercave", "", "Forest Boss to Forest"), // entering it from behind puts you in the room, not behind the gate
                    new TunicPortal("Forest Belltower", "", "Forest Boss to Belltower"),

                    // new TunicPortal("Archipelagos Redux_", "showfloordemo2022", "Portal (2)"),
                }
            },
            {
                "Forest Belltower",
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Courtyard", "", "Forest Bell to Fortress"),
                    new TunicPortal("East Forest Redux", "", "Forest Bell to Forest", isDeadEnd: true),
                    new TunicPortal("Overworld Redux", "", "Forest Bell to Overworld"),
                    new TunicPortal("Forest Boss Room", "", "Forest Bell to Boss", oneWay: true),
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
                    new TunicPortal("Fortress Basement", "", "Fortress to Under Fortress outside", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { {"prayer", 1 } }, new Dictionary<string, int> { { "laurels", 1 } } }),
                    new TunicPortal("Forest Belltower", "", "Fortress to Forest Bell", requiredItems: new Dictionary<string, int>{ { "laurels", 1 } }),
                    new TunicPortal("Overworld Redux", "", "Fortress to Overworld"),
                    new TunicPortal("Shop", "", "Fortress outside shop", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { {"prayer", 1 } }, new Dictionary<string, int> { { "laurels", 1 } } }),

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
                    new TunicPortal("Fortress Arena", "", "Fortress big gold door", requiredItems: new Dictionary<string, int> { { "prayer", 1 }, {"Fortress Courtyard, Overworld Redux_", 1}, { "Fortress Courtyard, Fortress Reliquary_upper", 1 }, {"Fortress Main, Fortress Courtyard_Big Door", 1 } }), // requires that one prayer thing to be down
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
                    new TunicPortal("Dusty", "", "Dusty Entrance", requiredItems: new Dictionary<string, int> { { "laurels", 1 } }),
                    new TunicPortal("Fortress Courtyard", "Upper", "Top Fortress Grave Path Exit"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Fortress Hero Grave", prayerPortal: true),
                }
            },
            {
                "Fortress Arena", // Where the boss is
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Main", "", "Boss to Fortress"),
                    new TunicPortal("Transit", "teleporter_spidertank", "Fortress Portal", prayerPortal: true, entryItems: new Dictionary<string, int> { { "prayer", 1 }, { "Fortress Basement, Fortress Main_", 1 }, {"Fortress Courtyard, Overworld Redux_", 1}, { "Fortress Courtyard, Fortress Main_", 1 } }),

                    // new TunicPortal("Fortress Main_", "", "Portal"), // There's two of these?
                }
            },
            {
                "Dusty", // broom
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Reliquary", "", "Dusty Exit", isDeadEnd: true),
                }
            },
            {
                "Mountain",
                new List<TunicPortal>
                {
                    new TunicPortal("Mountaintop", "", "Follow the Golden Path", requiredItems: new Dictionary<string, int> { { "holy cross", 1 } }),
                    new TunicPortal("Quarry Redux", "", "Mountain to Quarry"),
                    new TunicPortal("Overworld Redux", "", "Mountain to Overworld"),
                }
            },
            {
                "Mountaintop",
                new List<TunicPortal>
                {
                    new TunicPortal("Mountain", "", "Top of the Mountain exit", isDeadEnd: true),
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
                    new TunicPortal("Monastery", "back", "Quarry to Monastery Back", oneWay: true),
                    new TunicPortal("Mountain", "", "Quarry to Mountain"),
                    new TunicPortal("ziggurat2020_0", "", "Zig Entrance", entryItems: new Dictionary<string, int> { { "orb", 1 }, { "Darkwood Tunnel, Quarry Redux_", 1 }, { "prayer", 1 } }),
                    new TunicPortal("Darkwoods Tunnel", "", "Quarry to Overworld Connector"),
                    new TunicPortal("Transit", "teleporter_quarry teleporter", "Quarry Portal", prayerPortal: true),
                }
            },
            {
                "Monastery",
                new List<TunicPortal>
                {
                    new TunicPortal("Quarry Redux", "back", "Monastery to Quarry Back"),
                    new TunicPortal("Quarry Redux", "front", "Monastery to Quarry Front"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Quarry Hero Grave", prayerPortal: true),

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
                    // new TunicPortal("ziggurat2020_3", "zig2_skip", "Zig Skip"), // the elevator skip to lower zig, put a secret here later
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
                "ziggurat2020_3", // Lower zig, center is designated as before the prayer spot with the two cube minibosses
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_FTRoom", "", "Zig Portal Room", prayerPortal: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { {"laurels", 1 } }, new Dictionary<string, int> { { "prayer", 1 }, { "ziggurat2020_3, ziggurat2020_2_", 1 } } }), // Prayer portal room
                    // new TunicPortal("ziggurat2020_1", "zig2_skip", "Zig Skip Exit"), // the elevator skip to lower zig
                    new TunicPortal("ziggurat2020_2", "", "Zig 3 to Zig 2"),
                }
            },
            {
                "ziggurat2020_FTRoom", // The room with the prayer portal
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_3", "", "Zig Portal Room to Zig", requiredItems: new Dictionary<string, int> { { "prayer", 1 }, { "ziggurat2020_3, ziggurat2020_FTRoom", 1 } }),
                    new TunicPortal("Transit", "teleporter_ziggurat teleporter", "Zig Portal", prayerPortal: true),
                }
            },
            {
                "Swamp Redux 2",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "conduit", "Bottom Swamp Exit"),
                    new TunicPortal("Cathedral Redux", "main", "Swamp Entrance to Cathedral", requiredItems: new Dictionary<string, int> { { "prayer", 1 }, { "laurels", 1 }, { "Overworld Redux, Swamp Redux 2_wall", 1 } } ),
                    new TunicPortal("Cathedral Redux", "secret", "Swamp to Treasure Room", requiredItems: new Dictionary<string, int> { { "holy cross", 1 } }),
                    new TunicPortal("Cathedral Arena", "", "Swamp to Gauntlet", cantReach: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { {"laurels", 1 }, {"Swamp Redux 2, Overworld Redux_wall", 1 } }, new Dictionary<string, int> { { "Swamp Redux 2, RelicVoid_teleporter_relic plinth", 1 } } }),
                    new TunicPortal("Shop", "", "Swamp Shop"),
                    new TunicPortal("Overworld Redux", "wall", "Top Swamp Exit", cantReach: true, requiredItems: new Dictionary<string, int> { { "laurels", 1 }, { "Swamp Redux 2, Cathedral Arena_", 1 } }),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Swamp Hero Grave", cantReach: true, prayerPortal : true, requiredItems: new Dictionary<string, int> { { "Swamp Redux 2, Cathedral Arena_", 1 } }),
                }
            },
            {
                "Cathedral Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Swamp Redux 2", "main", "Cathedral Main Exit"),
                    new TunicPortal("Cathedral Arena", "", "Cathedral Elevator"),
                    new TunicPortal("Swamp Redux 2", "secret", "Cathedral Treasure Exit", cantReach: true, requiredItems: new Dictionary<string, int> { { "holy cross", 1 } }),
                }
            },
            {
                "Cathedral Arena", // Gauntlet
                new List<TunicPortal>
                {
                    new TunicPortal("Swamp Redux 2", "", "Gauntlet to Swamp", cantReach: true),
                    new TunicPortal("Cathedral Redux", "", "Gauntlet to Cathedral", givesAccess: new List<string> {"Cathedral Arena, Swamp Redux 2_"}),
                    new TunicPortal("Shop", "", "Gauntlet Shop", givesAccess: new List<string> {"Cathedral Arena, Swamp Redux 2_"}),
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
                    new TunicPortal("Fortress Reliquary", "teleporter_relic plinth", "Hero Relic to Fortress", isDeadEnd: true),
                    new TunicPortal("Monastery", "teleporter_relic plinth", "Hero Relic to Monastery", isDeadEnd: true),
                    new TunicPortal("Archipelagos Redux", "teleporter_relic plinth", "Hero Relic to West Garden", isDeadEnd: true),
                    new TunicPortal("Sword Access", "teleporter_relic plinth", "Hero Relic to East Forest", isDeadEnd: true),
                    new TunicPortal("Library Hall", "teleporter_relic plinth", "Hero Relic to Library", isDeadEnd: true),
                    new TunicPortal("Swamp Redux 2", "teleporter_relic plinth", "Hero Relic to Swamp", isDeadEnd: true),
                }
            },
            // can have issues coming to transit -- sometimes it locks up for some reason
            {
                "Transit", // Teleporter hub
                new List<TunicPortal>
                {
                    new TunicPortal("Archipelagos Redux", "teleporter_archipelagos_teleporter", "Transit to West Garden", entryItems: new Dictionary<string, int>{ { "prayer", 1 }, { "Archipelagos Redux, Overworld Redux_lower", 1} }),
                    new TunicPortal("Library Lab", "teleporter_library teleporter", "Transit to Library", requiredItems: new Dictionary<string, int>{ { "prayer", 1 }, { "Library Lab, Library Arena_", 1} }),
                    new TunicPortal("Quarry Redux", "teleporter_quarry teleporter", "Transit to Quarry", requiredItems: new Dictionary<string, int>{ { "prayer", 1 }, { "Quarry Redux, Darkwoods Tunnel_", 1 }, {"Darkwoods Tunnel, Quarry Redux_", 1 }, { "orb", 1 } }),
                    new TunicPortal("East Forest Redux", "teleporter_forest teleporter", "Transit to East Forest", requiredItems: new Dictionary<string, int> { { "laurels", 1 } }),
                    new TunicPortal("Fortress Arena", "teleporter_spidertank", "Transit to Fortress", entryItems: new Dictionary<string, int> { { "prayer", 1 }, { "Fortress Basement, Fortress Main_", 1 }, {"Fortress Courtyard, Overworld Redux_", 1}, { "Fortress Courtyard, Fortress Main_", 1 } }),
                    new TunicPortal("Atoll Redux", "teleporter_atoll", "Transit to Atoll"),
                    new TunicPortal("ziggurat2020_FTRoom", "teleporter_ziggurat teleporter", "Transit to Zig", requiredItems: new Dictionary<string, int>{ { "prayer", 1 }, { "ziggurat2020_3, ziggurat2020_FTRoom_", 1 } }),
                    new TunicPortal("Spirit Arena", "teleporter_spirit arena", "Transit to Heir"),
                    new TunicPortal("Overworld Redux", "teleporter_town", "Transit to Town"),
                    new TunicPortal("Overworld Redux", "teleporter_starting island", "Transit to Spawn", requiredItems: new Dictionary<string, int> { { "laurels", 1 } }),

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
                                twoPlusPortals.Remove(secondPortal);
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
                twoPlusPortals.RemoveAt(0);
            }
            List<string> shopRegionList = new List<string>();
            int shopCount = 7;
            int regionNumber = 0;
            while (shopCount > 0)
            {
                // manually making a portal for the shop, because it has some special properties
                Portal shopPortal = new Portal("Previous Region", "", "Shop portal", "Shop", new List<Dictionary<string, int>>());
                // check that a shop has not already been added to this region, since two shops in the same region causes problems
                if (!shopRegionList.Contains(twoPlusPortals[regionNumber].Scene))
                {
                    comboNumber++;
                    shopRegionList.Add(twoPlusPortals[regionNumber].Scene);
                    RandomizedPortals.Add(comboNumber.ToString(), new PortalCombo(shopPortal, twoPlusPortals[regionNumber]));
                    twoPlusPortals.RemoveAt(regionNumber);
                    shopCount--;
                }
                else
                {
                    regionNumber++;
                }
                if (regionNumber == twoPlusPortals.Count - 1)
                {
                    Logger.LogInfo("too many shops, not enough regions");
                }
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
                // if this triggers, increase or decrease shop count by 1
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
                        Logger.LogInfo("portal 1 is " + portal1.Name);
                        Logger.LogInfo("portal 2 is " + portal2.Name);

                        if (portal2.Scene == "Shop")
                        {
                            portal.destinationSceneName = portal2.Scene;
                            portal.id = "";
                            portal.optionalIDToSpawnAt = "";
                        }
                        else
                        {
                            portal.destinationSceneName = portal2.Scene;
                            portal.id = comboTag;
                            portal.optionalIDToSpawnAt = comboTag + comboTag + comboTag + comboTag; // quadrupling since doubling and tripling can have overlaps
                        }
                        Logger.LogInfo("finished, moving on to next portal");
                    }

                    if (portal2.Scene == loadingScene.name && portal2.Tag == portal.id && portal2.Destination == portal.destinationSceneName)
                    {
                        Logger.LogInfo("portal 2 matched");
                        Logger.LogInfo("current scene is " + loadingScene.name);
                        Logger.LogInfo("portal destination was " + portal.destinationSceneName + "_" + portal.id);
                        Logger.LogInfo("portal destination is now " + portal1.Scene + "_" + comboTag);
                        Logger.LogInfo("portal 1 is " + portal1.Name);
                        Logger.LogInfo("portal 2 is " + portal2.Name);
                        if (portal1.Scene == "Shop")
                        {
                            portal.destinationSceneName = portal1.Scene;
                            portal.id = "";
                            portal.optionalIDToSpawnAt = "";
                        }
                        else
                        {
                            portal.destinationSceneName = portal1.Scene;
                            portal.id = comboTag + comboTag + comboTag + comboTag; // quadrupling since doubling and tripling can have overlaps
                            portal.optionalIDToSpawnAt = comboTag;
                        }

                        Logger.LogInfo("finished, moving on to next portal");
                    }
                }
            }
        }
        // this is for use in PlayerCharacterPatches. Will probably need a refactor later if we do random player spawn
        public static void AltModifyPortals(Dictionary<string, PortalCombo> portalComboList)
        {
            Logger.LogInfo("starting alt modify portals");
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
                    if (portal1.Tag == portal.id && portal1.Destination == portal.destinationSceneName)
                    {
                        Logger.LogInfo("portal 1 matched");
                        Logger.LogInfo("portal destination was " + portal.destinationSceneName + "_" + portal.id);
                        Logger.LogInfo("portal destination is now " + portal2.Scene+ "_" + comboTag);
                        Logger.LogInfo("portal 1 is " + portal1.Name);
                        Logger.LogInfo("portal 2 is " + portal2.Name);

                        if (portal2.Scene == "Shop")
                        {
                            portal.destinationSceneName = portal2.Scene;
                            portal.id = "";
                            portal.optionalIDToSpawnAt = "";
                        }
                        else
                        {
                            portal.destinationSceneName = portal2.Scene;
                            portal.id = comboTag;
                            portal.optionalIDToSpawnAt = comboTag + comboTag + comboTag + comboTag; // quadrupling since doubling and tripling can have overlaps
                        }
                        Logger.LogInfo("finished, moving on to next portal");
                    }

                    if (portal2.Tag == portal.id && portal2.Destination == portal.destinationSceneName)
                    {
                        Logger.LogInfo("portal 2 matched");
                        Logger.LogInfo("portal destination was " + portal.destinationSceneName + "_" + portal.id);
                        Logger.LogInfo("portal destination is now " + portal1.Scene + "_" + comboTag);
                        Logger.LogInfo("portal 1 is " + portal1.Name);
                        Logger.LogInfo("portal 2 is " + portal2.Name);
                        if (portal1.Scene == "Shop")
                        {
                            portal.destinationSceneName = portal1.Scene;
                            portal.id = "";
                            portal.optionalIDToSpawnAt = "";
                        }
                        else
                        {
                            portal.destinationSceneName = portal1.Scene;
                            portal.id = comboTag + comboTag + comboTag + comboTag; // quadrupling since doubling and tripling can have overlaps
                            portal.optionalIDToSpawnAt = comboTag;
                        }

                        Logger.LogInfo("finished, moving on to next portal");
                    }
                }
            }
        }
    }
}
