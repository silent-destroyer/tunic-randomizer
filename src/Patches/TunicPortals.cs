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
using System.Globalization;
using HarmonyLib.Tools;

namespace TunicRandomizer
{
    public class TunicPortals {
        private static ManualLogSource Logger = TunicRandomizer.Logger;

        public class TunicPortal
        {
            public string SceneName; // the scene the portal is in
            public string Destination; // the vanilla destination scene
            public string DestinationTag; // the vanilla destination tag, aka ID
            public string PortalName; // a human-readable name for the portal
            public Dictionary<string, int> RequiredItems; // required items if there is only one item or one set of items required. A string like "scene, destination_tag" counts as an item.
            public List<Dictionary<string, int>> RequiredItemsOr; // required items if there are multiple different possible requirements. A string like "scene, destination_tag" counts as an item.
            public List<string> GivesAccess; // portals that you are given access to by this portal. ex: the dance fox portal to the lower east forest portal in guardhouse 1.
            public Dictionary<string, int> EntryItems; // portals that require items to enter, but not exit from. ex: hero's graves, the yellow prayer portal pads, and the fountain holy cross door in overworld.
            public bool DeadEnd; // portals that are dead ends, like stick house or the gauntlet lower entry.
            public bool PrayerPortal; // portals that require prayer to enter. This is a more convenient version of GivesAccess for prayer portals.
            public bool OneWay; // portals that are one-way, such as the back entrance to monastery and the forest belltower top portal
            public bool CantReach; // portals that cannot reach the center of the region, and as such do not give region access, like the rail between bottom of the well and furnace

            public TunicPortal() { }

            public TunicPortal(string destination, string destinationTag, string portalName)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, bool prayerPortal = false, bool deadEnd = false, bool oneWay = false, bool cantReach = false)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                PrayerPortal = prayerPortal;
                DeadEnd = deadEnd;
                OneWay = oneWay;
                CantReach = cantReach;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, Dictionary<string, int> entryItems, bool prayerPortal = false, bool deadEnd = false, bool oneWay = false, bool cantReach = false)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                EntryItems = entryItems;
                PrayerPortal = prayerPortal;
                DeadEnd = deadEnd;
                OneWay = oneWay;
                CantReach = cantReach;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, Dictionary<string, int> requiredItems, bool prayerPortal = false, bool deadEnd = false, bool cantReach = false)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                RequiredItems = requiredItems;
                PrayerPortal = prayerPortal;
                DeadEnd = deadEnd;
                CantReach = cantReach;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, List<string> givesAccess, bool cantReach = false, bool oneWay = false)
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

        // this is a big list of every portal in the game, along with their access requirements
        // a portal without access requirements just means you can get to the center of the region from that portal and vice versa
        public static Dictionary<string, List<TunicPortal>> PortalList = new Dictionary<string, List<TunicPortal>>
        {
            {
                "Overworld Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Sword Cave", "", "Stick House Entrance"),
                    new TunicPortal("Windmill", "", "Windmill Entrance"),
                    new TunicPortal("Sewer", "entrance", "Well Ladder Entrance"),
                    new TunicPortal("Sewer", "west_aqueduct", "Entrance to Well from Well Rail", cantReach: true, givesAccess: new List<string> { "Overworld Redux, Furnace_gyro_upper_north" }),
                    new TunicPortal("Overworld Interiors", "house", "Old House Entry Door", requiredItems: new Dictionary<string, int> { {"Key (House)", 1} }), // make this match actual item name
                    new TunicPortal("Overworld Interiors", "under_checkpoint", "Old House Waterfall Entrance"),
                    new TunicPortal("Furnace", "gyro_upper_north", "Entrance to Furnace from Well Rail", cantReach: true, givesAccess : new List<string> { "Overworld Redux, Sewer_west_aqueduct" }),
                    new TunicPortal("Furnace", "gyro_upper_east", "Entrance to Furnace from Windmill"),
                    new TunicPortal("Furnace", "gyro_west", "Entrance to Furnace from West Garden", cantReach: true, givesAccess: new List<string> {"Overworld Redux, Archipelagos Redux_lower"}),
                    new TunicPortal("Furnace", "gyro_lower", "Entrance to Furnace from Beach"),
                    new TunicPortal("Overworld Cave", "", "Rotating Lights Entrance"),
                    new TunicPortal("Swamp Redux 2", "wall", "Swamp Upper Entrance", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1} }),
                    new TunicPortal("Swamp Redux 2", "conduit", "Swamp Lower Entrance"),
                    new TunicPortal("Ruins Passage", "east", "Ruined Hall Entrance Not-Door"),
                    new TunicPortal("Ruins Passage", "west", "Ruined Hall Entrance Door", requiredItems: new Dictionary<string, int> { { "Key", 2 } }), // and access to any overworld portal, but we start in overworld so no need to put it here
                    new TunicPortal("Atoll Redux", "upper", "Atoll Upper Entrance"),
                    new TunicPortal("Atoll Redux", "lower", "Atoll Lower Entrance"),
                    new TunicPortal("ShopSpecial", "", "Special Shop Entrance", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1} }),
                    new TunicPortal("Maze Room", "", "Maze Cave Entrance"),
                    new TunicPortal("Archipelagos Redux", "upper", "West Garden Entrance by Belltower", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } }),
                    new TunicPortal("Archipelagos Redux", "lower", "West Garden Entrance by Dark Tomb", cantReach: true, givesAccess: new List<string> {"Overworld Redux, Furnace_gyro_west"}),
                    new TunicPortal("Archipelagos Redux", "lowest", "West Garden Laurel Entrance", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } }),
                    new TunicPortal("Temple", "main", "Temple Door Entrance", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Forest Belltower, Forest Boss Room_", 1 }, { "Overworld Redux, Archipelagos Redux_upper", 1 }, { "Stick", 1 } }, new Dictionary<string, int> { { "Forest Belltower, Forest Boss Room_", 1 }, { "Overworld Redux, Archipelagos Redux_upper", 1 }, { "Techbow", 1 } }, new Dictionary<string, int> { { "Forest Belltower, Forest Boss Room_", 1 }, { "Hyperdash", 1 }, { "Stick", 1 } }, new Dictionary<string, int> { { "Forest Belltower, Forest Boss Room_", 1 }, { "Hyperdash", 1 }, { "Techbow", 1 } } }),
                    new TunicPortal("Temple", "rafters", "Temple Rafters Entrance"),
                    new TunicPortal("Ruined Shop", "", "Ruined Shop Entrance"),
                    new TunicPortal("PatrolCave", "", "Patrol Cave Entrance"),
                    new TunicPortal("Town Basement", "beach", "Hourglass Cave Entrance"),
                    new TunicPortal("Changing Room", "", "Changing Room Entrance"),
                    new TunicPortal("CubeRoom", "", "Cube Room Entrance"),
                    new TunicPortal("Mountain", "", "Stairs from Overworld to Mountain"),
                    new TunicPortal("Fortress Courtyard", "", "Overworld to Fortress"),
                    new TunicPortal("Town_FiligreeRoom", "", "HC Room Entrance next to Changing Room", entryItems: new Dictionary<string, int> { { "21", 1 } }), // this is entry items because when you exit from this portal, you end up in front of the door
                    new TunicPortal("EastFiligreeCache", "", "Glass Cannon HC Room Entrance", requiredItems: new Dictionary<string, int> { { "21", 1 } }), // this is required items because when you exit from this portal, you end up behind the door
                    new TunicPortal("Darkwoods Tunnel", "", "Overworld to Quarry Connector"),
                    new TunicPortal("Crypt Redux", "", "Dark Tomb Main Entrance"),
                    new TunicPortal("Forest Belltower", "", "Overworld to Forest Belltower"),
                    new TunicPortal("Transit", "teleporter_town", "Town Portal", prayerPortal: true),
                    new TunicPortal("Transit", "teleporter_starting island", "Spawn Portal", prayerPortal: true),
                    new TunicPortal("Waterfall", "", "Entrance to Fairy Cave"),

                    // new TunicPortal("_", "", "Portal"), // ?
                    // new TunicPortal("Forest Belltower_", "showfloordemo2022", "Portal (12)"), // ?
                    // new TunicPortal("DEMO_altEnd_", "", "_Portal (Secret Demo End)"), // ?
                }
            },
            {
                "Waterfall", // fairy cave
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Fairy Cave Exit", deadEnd: true),
                }
            },
            {
                "Windmill",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Windmill Exit"),
                    new TunicPortal("Shop", "", "Windmill Shop"),
                }
            },
            {
                "Overworld Interiors", // House in town
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "house", "Front Door of Old House Exit"),
                    new TunicPortal("g_elements", "", "Teleport to Secret Treasure Room"),
                    new TunicPortal("Overworld Redux", "under_checkpoint", "Exit from Old House Back Door", requiredItems: new Dictionary<string, int> { { "Overworld Interiors, Overworld Redux_house", 1 } }), // since you get access to the center of a region from either portal, only one of these two is actually needed

                    // new TunicPortal("Archipelagos Redux_", "", "_ShowfloorDemo2022 Portal"), // unused and disabled
                }
            },
            {
                "g_elements", // Secret treasure room
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Interiors", "", "Secret Treasure Room Exit", deadEnd: true),
                }
            },
            {
                "Changing Room", // Secret treasure room
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Changing Room Exit", deadEnd: true),
                }
            },
            {
                "Furnace", // Under the west belltower
                // I'm calling the "center" of this region the space accessible by the windmill and beach
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "gyro_upper_north", "Furnace to Well Rail", requiredItems: new Dictionary<string, int> { {"Hyperdash", 1} }),
                    new TunicPortal("Crypt Redux", "", "Furnace to Dark Tomb", requiredItems: new Dictionary<string, int> { {"Hyperdash", 1} }, givesAccess: new List<string> {"Furnace, Overworld Redux_gyro_west"}),
                    new TunicPortal("Overworld Redux", "gyro_west", "Furnace to West Garden", requiredItems : new Dictionary<string, int> { {"Hyperdash", 1} }, givesAccess : new List<string> {"Furnace, Crypt Redux_"}),
                    new TunicPortal("Overworld Redux", "gyro_lower", "Furnace to Beach"),
                    new TunicPortal("Overworld Redux", "gyro_upper_east", "Furnace to Windmill"),
                }
            },
            {
                "Sword Cave", // Stick house
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Stick House Exit", deadEnd: true),
                }
            },
            {
                "Ruins Passage", // That little hallway with the key door near the start in Overworld
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "east", "Ruined Passage Door Exit"),
                    new TunicPortal("Overworld Redux", "west", "Ruined Passage Not-door Exit"),
                }
            },
            {
                "EastFiligreeCache", // The holy cross room with the 3 chests near swamp entrance
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Glass Cannon HC Room Exit", deadEnd : true),
                }
            },
            {
                "Overworld Cave", // East beach, next to swamp entrance, rotating lights room
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Rotating Lights Exit", deadEnd: true),
                }
            },
            {
                "Maze Room", // Invisible maze
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Maze Cave Exit", deadEnd: true),
                }
            },
            {
                "Town Basement", // Hourglass cave
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "beach", "Hourglass Cave Exit", deadEnd: true), // yes, it has a tag even though it doesn't need one
                }
            },
            {
                "ShopSpecial", // Special shop, laurel across from that platform between east forest and fortress
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Special Shop Exit", deadEnd: true),
                }
            },
            {
                "Temple", // Where you put the hexes
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "rafters", "Temple Rafters Exit"),
                    new TunicPortal("Overworld Redux", "main", "Temple Door Exit"),
                }
            },
            {
                "Sewer", // Bottom of the Well
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "entrance", "Well Ladder Exit"),
                    new TunicPortal("Sewer_Boss", "", "Well to Well Boss"),
                    new TunicPortal("Overworld Redux", "west_aqueduct", "Well Rail Exit"),
                }
            },
            {
                "Sewer_Boss", // Boss room in the Bottom of the Well
                new List<TunicPortal>
                {
                    new TunicPortal("Sewer", "", "Well Boss to Well"),
                    new TunicPortal("Crypt Redux", "", "Well Boss to Dark Tomb", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Sewer_Boss, Sewer_", 1 } }, new Dictionary<string, int> { { "Hyperdash", 1 } } }),
                }
            },
            {
                "Crypt Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Dark Tomb to Overworld"),
                    new TunicPortal("Furnace", "", "Dark Tomb to Furnace", requiredItems: new Dictionary<string, int> { {"Lantern", 1} }),
                    new TunicPortal("Sewer_Boss", "", "Dark Tomb to Well Boss"),
                }
            },
            {
                "Archipelagos Redux", // West Garden
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "lower", "West Garden towards Dark Tomb"),
                    new TunicPortal("archipelagos_house", "", "Magic Dagger House Entrance"),
                    new TunicPortal("Overworld Redux", "upper", "West Garden after boss", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Sword", 1 }, { "Archipelagos Redux, Overworld Redux_lower", 1 } }, new Dictionary<string, int> { { "Hyperdash", 1 } } }),
                    new TunicPortal("Shop", "", "West Garden Shop"), // there's two of these, one is unused and disabled
                    new TunicPortal("Overworld Redux", "lowest", "West Garden Laurel Exit"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "West Garden Hero's Grave", prayerPortal: true), // Hero grave
                    new TunicPortal("Transit", "teleporter_archipelagos_teleporter", "West Garden Portal", prayerPortal: true, deadEnd: true), // Portal to the thing behind dagger house
                }
            },
            {
                "archipelagos_house", // Magic Dagger house in West Garden
                new List<TunicPortal>
                {
                    new TunicPortal("Archipelagos Redux", "", "Magic Dagger House Exit", deadEnd: true),
                }
            },
            {
                "Atoll Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Frog Stairs", "eye", "Frog Eye Entrance"),
                    new TunicPortal("Library Exterior", "", "Atoll to Library", prayerPortal : true),
                    new TunicPortal("Overworld Redux", "upper", "Upper Atoll Exit"),
                    new TunicPortal("Overworld Redux", "lower", "Lower Atoll Exit", requiredItems: new Dictionary<string, int> {{"Hyperdash", 1}}),
                    new TunicPortal("Frog Stairs", "mouth", "Frog Mouth Entrance", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Wand", 1 } }, new Dictionary<string, int> { { "Hyperdash", 1 } } }),
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
                    new TunicPortal("frog cave main", "Exit", "Upper Frog to Lower Frog Exit"),
                    new TunicPortal("Atoll Redux", "eye", "Frog Eye Exit"),
                    new TunicPortal("frog cave main", "Entrance", "Upper Frog to Lower Frog Entrance"),
                }
            },
            {
                "frog cave main", // Frog's domain, yes it's lowercase
                new List<TunicPortal>
                {
                    new TunicPortal("Frog Stairs", "Exit", "Lower Frog Orb Exit", requiredItems: new Dictionary<string, int> { { "Wand", 1 }, { "frog cave main, Frog Stairs_Entrance", 1 } }),
                    new TunicPortal("Frog Stairs", "Entrance", "Lower Frog Ladder Exit", oneWay: true),
                }
            },
            {
                "Library Exterior",
                new List<TunicPortal>
                {
                    new TunicPortal("Library Hall", "", "Library Entry Ladder", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Hyperdash", 1 } }, new Dictionary<string, int> { { "Wand", 1} } }),
                    new TunicPortal("Atoll Redux", "", "Library to Atoll", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Hyperdash", 1 } }, new Dictionary<string, int> { { "Wand", 1} } }),
                }
            },
            {
                "Library Hall", // Entry area with hero grave
                new List<TunicPortal>
                {
                    new TunicPortal("Library Rotunda", "", "Lower Library to Rotunda"),
                    new TunicPortal("Library Exterior", "", "Library Bookshelf Exit"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Library Hero's Grave", prayerPortal: true),
                }
            },
            {
                "Library Rotunda", // The circular room with the ladder
                new List<TunicPortal>
                {
                    new TunicPortal("Library Hall", "", "Library Rotunda Lower Exit"),
                    new TunicPortal("Library Lab", "", "Library Rotunda Upper Exit"),
                }
            },
            {
                "Library Lab",
                new List<TunicPortal>
                {
                    new TunicPortal("Library Arena", "", "Upper Library to Librarian"),
                    new TunicPortal("Library Rotunda", "", "Upper Library to Rotunda", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Hyperdash", 1 } }, new Dictionary<string, int> { { "Wand", 1}, {"Library Lab, Library Rotunda_", 1 } } }),
                    new TunicPortal("Transit", "teleporter_library teleporter", "Library Portal", prayerPortal : true),
                }
            },
            {
                "Library Arena",
                new List<TunicPortal>
                {
                    new TunicPortal("Library Lab", "", "Library Librarian Arena Exit", deadEnd: true),
                }
            },
            {
                "East Forest Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Sword Access", "lower", "Forest Grave Path Lower Entrance"),
                    new TunicPortal("East Forest Redux Laddercave", "upper", "Forest Fox Dance Outside Doorway", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } } ),
                    new TunicPortal("East Forest Redux Interior", "lower", "Forest Guard House 2 Lower Entrance"),
                    new TunicPortal("East Forest Redux Laddercave", "gate", "Forest Guard House 1 Gate Entrance"),
                    new TunicPortal("Sword Access", "upper", "Forest Grave Path Upper Entrance"),
                    new TunicPortal("East Forest Redux Interior", "upper", "Forest Guard House 2 Upper Entrance"),
                    new TunicPortal("East Forest Redux Laddercave", "lower", "Forest Guard House 1 Lower Entrance"),
                    new TunicPortal("Forest Belltower", "", "Forest to Belltower"),
                    new TunicPortal("Transit", "teleporter_forest teleporter", "Forest Portal", prayerPortal: true),
                }
            },
            {
                "East Forest Redux Laddercave", // the place with the two ladders that leads to the boss room
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux", "upper", "Guard House 1 Dance Exit", givesAccess: new List<string> { "East Forest Redux Laddercave, East Forest Redux_upper" }, requiredItems: new Dictionary<string, int> { { "East Forest Redux Laddercave, East Forest Redux_gate", 1 } }), // making the upper ones the "center" for easier logic writing
                    new TunicPortal("East Forest Redux", "lower", "Guard House 1 Lower Exit", givesAccess: new List<string> { "East Forest Redux Laddercave, East Forest Redux_lower" }, requiredItems: new Dictionary<string, int> { { "East Forest Redux Laddercave, East Forest Redux_gate", 1 } }),
                    new TunicPortal("East Forest Redux", "gate", "Guard House 1 Upper Forest Exit"),
                    new TunicPortal("Forest Boss Room", "", "Guard House 1 to Guard Captain Room"),
                }
            },
            {
                "Sword Access", // East forest hero grave area
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux", "upper", "Upper Forest Grave Path Exit", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } }),
                    new TunicPortal("East Forest Redux", "lower", "Lower Forest Grave Path Exit"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "East Forest Hero's Grave", prayerPortal: true, requiredItems: new Dictionary<string, int> { {"Sword Access, East Forest Redux_lower", 1 } }), // Can't open the gate from behind
                    
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
                    new TunicPortal("East Forest Redux Laddercave", "", "Guard Captain Room Non-Gate Exit"), // entering it from behind puts you in the room, not behind the gate
                    new TunicPortal("Forest Belltower", "", "Guard Captain Room Gate Exit"),

                    // new TunicPortal("Archipelagos Redux_", "showfloordemo2022", "Portal (2)"),
                }
            },
            {
                "Forest Belltower",
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Courtyard", "", "Forest Belltower to Fortress"),
                    new TunicPortal("East Forest Redux", "", "Forest Belltower to Forest", deadEnd: true),
                    new TunicPortal("Overworld Redux", "", "Forest Belltower to Overworld"),
                    new TunicPortal("Forest Boss Room", "", "Forest Belltower to Guard Captain Room", oneWay: true),
                }
            },
            {
                "Fortress Courtyard", // Outside the fortress, the area connected to east forest and overworld. Center of the area is on the fortress-side of the bridge
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Reliquary", "Lower", "Lower Fortress Grave Path Entrance"),
                    new TunicPortal("Fortress Reliquary", "Upper", "Upper Fortress Grave Path Entrance", oneWay: true, givesAccess: new List<string> { "Fortress Courtyard, Fortress East_" }),
                    new TunicPortal("Fortress Main", "Big Door", "Fortress Courtyard to Fortress Interior"),
                    new TunicPortal("Fortress East", "", "Fortress Courtyard to Fortress East", oneWay: true, givesAccess: new List<string> { "Fortress Courtyard, Fortress Reliquary_Upper" }),
                    new TunicPortal("Fortress Basement", "", "Fortress Courtyard to Beneath the Earth", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "12", 1 }, { "Fortress Courtyard, Overworld Redux_", 1 } }, new Dictionary<string, int> { { "Hyperdash", 1 } }, new Dictionary<string, int> { {"Fortress Courtyard, Shop_", 1 } } }),
                    new TunicPortal("Forest Belltower", "", "Fortress Courtyard to Forest Belltower", requiredItems: new Dictionary<string, int>{ { "Hyperdash", 1 } }),
                    new TunicPortal("Overworld Redux", "", "Fortress Courtyard to Overworld", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { {"Hyperdash", 1} }, new Dictionary<string, int> { {"Fortress Courtyard, Fortress East_", 1} }, new Dictionary<string, int> { { "Wand", 1 }, { "Fortress Courtyard, Forest Belltower_", 1 } } }), // remember, required items is just what you need to get to the center of a region -- prayer only gets you to the shop and beneath the earth
                    new TunicPortal("Shop", "", "Fortress Courtyard Shop", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "12", 1 }, { "Fortress Courtyard, Overworld Redux_", 1 } }, new Dictionary<string, int> { { "Hyperdash", 1 } }, new Dictionary<string, int> { {"Fortress Courtyard, Fortress Basement_", 1 } } }),

                    // new TunicPortal("Overworld Redux_", "", "Portal (4)"), // unused and disabled
                }
            },
            {
                "Fortress Basement", // Under the fortress
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Main", "", "Beneath the Earth to Fortress Interior"),
                    new TunicPortal("Fortress Courtyard", "", "Beneath the Earth to Fortress Courtyard"),
                }
            },
            {
                "Fortress Main", // Inside the fortress
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Courtyard", "Big Door", "Fortress Main Exit"),
                    new TunicPortal("Fortress Basement", "", "Fortress Interior to Beneath the Earth"),
                    new TunicPortal("Fortress Arena", "", "Fortress Interior to Siege Engine", requiredItems: new Dictionary<string, int> { { "12", 1 }, {"Fortress Courtyard, Overworld Redux_", 1}, { "Fortress Courtyard, Fortress Reliquary_upper", 1 }, {"Fortress Main, Fortress Courtyard_Big Door", 1 } }), // requires that one prayer thing to be down
                    new TunicPortal("Shop", "", "Fortress Interior Shop"),
                    new TunicPortal("Fortress East", "upper", "Fortress Interior to East Fortress Upper"),
                    new TunicPortal("Fortress East", "lower", "Fortress Interior to East Fortress Lower"),
                }
            },
            {
                "Fortress East", // that tiny area with the couple mages up high, and the ladder in the lower right
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Main", "lower", "East Fortress to Interior Lower", requiredItems: new Dictionary<string, int> { { "Fortress East, Fortress Main_upper", 1} }),
                    new TunicPortal("Fortress Courtyard", "", "East Fortress to Courtyard"),
                    new TunicPortal("Fortress Main", "upper", "East Fortress to Interior Upper"),
                }
            },
            {
                "Fortress Reliquary", // Where the grave is
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Courtyard", "Lower", "Lower Fortress Grave Path Exit"),
                    new TunicPortal("Dusty", "", "Fortress Grave Path Dusty Entrance", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } }),
                    new TunicPortal("Fortress Courtyard", "Upper", "Upper Fortress Grave Path Exit"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Fortress Hero's Grave", prayerPortal: true),
                }
            },
            {
                "Fortress Arena", // Where the boss is
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Main", "", "Siege Engine Arena to Fortress"),
                    new TunicPortal("Transit", "teleporter_spidertank", "Fortress Portal", prayerPortal: true, entryItems: new Dictionary<string, int> { { "Fortress Basement, Fortress Main_", 1 }, {"Fortress Courtyard, Overworld Redux_", 1}, { "Fortress Courtyard, Fortress Main_", 1 } }),
                    // new TunicPortal("Fortress Main_", "", "Portal"), // There's two of these, one is disabled
                }
            },
            {
                "Dusty", // broom
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Reliquary", "", "Dusty Exit", deadEnd: true),
                }
            },
            {
                "Mountain",
                new List<TunicPortal>
                {
                    new TunicPortal("Mountaintop", "", "Stairs to Top of the Mountain", requiredItems: new Dictionary<string, int> { { "21", 1 } }),
                    new TunicPortal("Quarry Redux", "", "Mountain to Quarry"),
                    new TunicPortal("Overworld Redux", "", "Mountain to Overworld"),
                }
            },
            {
                "Mountaintop",
                new List<TunicPortal>
                {
                    new TunicPortal("Mountain", "", "Top of the Mountain Exit", deadEnd: true),
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
                    new TunicPortal("Darkwoods Tunnel", "", "Quarry to Overworld Exit"),
                    new TunicPortal("Shop", "", "Quarry Shop"),
                    new TunicPortal("Monastery", "front", "Quarry to Monastery Front"),
                    new TunicPortal("Monastery", "back", "Quarry to Monastery Back", oneWay: true),
                    new TunicPortal("Mountain", "", "Quarry to Mountain"),
                    new TunicPortal("ziggurat2020_0", "", "Quarry Zig Entrance", entryItems: new Dictionary<string, int> { { "Wand", 1 }, { "Darkwood Tunnel, Quarry Redux_", 1 }, { "12", 1 } }),
                    new TunicPortal("Transit", "teleporter_quarry teleporter", "Quarry Portal", prayerPortal: true),
                }
            },
            {
                "Monastery",
                new List<TunicPortal>
                {
                    new TunicPortal("Quarry Redux", "back", "Monastery Rear Exit"),
                    new TunicPortal("Quarry Redux", "front", "Monastery Front Exit"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Monastery Hero's Grave", prayerPortal: true),

                    // new TunicPortal("Quarry_", "lower", "Portal (1)"), // Unused portal, disabled, and very high up
                }
            },
            {
                "ziggurat2020_0", // Zig entrance hallway
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_1", "", "Zig Entry Hallway to Zig 1"),
                    new TunicPortal("Quarry Redux", "", "Zig Entry Hallway to Quarry"),
                }
            },
            {
                "ziggurat2020_1", // Upper zig
                new List<TunicPortal>
                {
                    // new TunicPortal("ziggurat2020_3", "zig2_skip", "Zig Skip"), // the elevator skip to lower zig, put a secret here later
                    new TunicPortal("ziggurat2020_0", "", "Zig 1 to Zig Entry"),
                    new TunicPortal("ziggurat2020_2", "", "Zig 1 to Zig 2"),
                }
            },
            {
                "ziggurat2020_2", // Zig intermediate elevator
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_1", "", "Zig 2 to Zig 1", oneWay: true),
                    new TunicPortal("ziggurat2020_3", "", "Zig 2 to Zig 3"),
                }
            },
            {
                "ziggurat2020_3", // Lower zig, center is designated as before the prayer spot with the two cube minibosses
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_FTRoom", "", "Zig Portal Room Entrance", prayerPortal: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { {"Hyperdash", 1 }, { "ziggurat2020_3, ziggurat2020_2_", 1 } }, new Dictionary<string, int> { { "12", 1 }, { "ziggurat2020_3, ziggurat2020_2_", 1 } } }), // Prayer portal room
                    // new TunicPortal("ziggurat2020_1", "zig2_skip", "Zig Skip Exit"), // the elevator skip to lower zig
                    new TunicPortal("ziggurat2020_2", "", "Zig 3 to Zig 2"),
                }
            },
            {
                "ziggurat2020_FTRoom", // The room with the prayer portal
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_3", "", "Zig Portal Room Exit", requiredItems: new Dictionary<string, int> { { "12", 1 }, { "ziggurat2020_3, ziggurat2020_FTRoom", 1 } }),
                    new TunicPortal("Transit", "teleporter_ziggurat teleporter", "Zig Portal", prayerPortal: true),
                }
            },
            {
                "Swamp Redux 2",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "conduit", "Lower Swamp Exit"),
                    new TunicPortal("Cathedral Redux", "main", "Swamp to Cathedral Main Entrance", requiredItems: new Dictionary<string, int> { { "12", 1 }, { "Hyperdash", 1 }, { "Overworld Redux, Swamp Redux 2_conduit", 1 } } ),
                    new TunicPortal("Cathedral Redux", "secret", "Swamp to Cathedral Treasure Room Entrance", requiredItems: new Dictionary<string, int> { { "21", 1 } }),
                    new TunicPortal("Cathedral Arena", "", "Swamp to Gauntlet", cantReach: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Hyperdash", 1 }, {"Swamp Redux 2, Overworld Redux_wall", 1 } }, new Dictionary<string, int> { { "Swamp Redux 2, RelicVoid_teleporter_relic plinth", 1 } } }),
                    new TunicPortal("Shop", "", "Swamp Shop"),
                    new TunicPortal("Overworld Redux", "wall", "Upper Swamp Exit", cantReach: true, requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 }, { "Swamp Redux 2, Cathedral Arena_", 1 } }),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Swamp Hero's Grave", cantReach: true, prayerPortal : true, requiredItems: new Dictionary<string, int> { { "Swamp Redux 2, Cathedral Arena_", 1 } }),
                }
            },
            {
                "Cathedral Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Swamp Redux 2", "main", "Cathedral Main Exit"),
                    new TunicPortal("Cathedral Arena", "", "Cathedral Elevator"),
                    new TunicPortal("Swamp Redux 2", "secret", "Cathedral Treasure Room Exit", cantReach: true, deadEnd: true), // only one chest, just use item access rules for it
                }
            },
            {
                "Cathedral Arena", // Gauntlet
                new List<TunicPortal>
                {
                    new TunicPortal("Swamp Redux 2", "", "Gauntlet to Swamp", deadEnd: true, cantReach: true),
                    new TunicPortal("Cathedral Redux", "", "Gauntlet Elevator", givesAccess: new List<string> {"Cathedral Arena, Swamp Redux 2_"}),
                    new TunicPortal("Shop", "", "Gauntlet Shop", givesAccess: new List<string> {"Cathedral Arena, Swamp Redux 2_"}), // we love gauntlet shop
                }
            },
            //{
            //    "Shop", // Every shop is just this region, adding them in later to avoid issues with the combo number
            //    new List<TunicPortal>
            //    {
            //        new TunicPortal("_", "", "Shop Exit Portal"),
            //    }
            //},
            {
                "RelicVoid", // Hero relic area
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Reliquary", "teleporter_relic plinth", "Hero Relic to Fortress", deadEnd: true),
                    new TunicPortal("Monastery", "teleporter_relic plinth", "Hero Relic to Monastery", deadEnd: true),
                    new TunicPortal("Archipelagos Redux", "teleporter_relic plinth", "Hero Relic to West Garden", deadEnd: true),
                    new TunicPortal("Sword Access", "teleporter_relic plinth", "Hero Relic to East Forest", deadEnd: true),
                    new TunicPortal("Library Hall", "teleporter_relic plinth", "Hero Relic to Library", deadEnd: true),
                    new TunicPortal("Swamp Redux 2", "teleporter_relic plinth", "Hero Relic to Swamp", deadEnd: true),
                }
            },
            {
                "Transit", // Teleporter hub
                new List<TunicPortal>
                {
                    new TunicPortal("Archipelagos Redux", "teleporter_archipelagos_teleporter", "Far Shore to West Garden", entryItems: new Dictionary<string, int>{ { "12", 1 }, { "Archipelagos Redux, Overworld Redux_lower", 1} }),
                    new TunicPortal("Library Lab", "teleporter_library teleporter", "Far Shore to Library", entryItems: new Dictionary<string, int>{ { "12", 1 }, { "Library Lab, Library Arena_", 1} }),
                    new TunicPortal("Quarry Redux", "teleporter_quarry teleporter", "Far Shore to Quarry", entryItems: new Dictionary<string, int>{ { "12", 1 }, { "Quarry Redux, Darkwoods Tunnel_", 1 }, {"Darkwoods Tunnel, Quarry Redux_", 1 }, { "Wand", 1 } }),
                    new TunicPortal("East Forest Redux", "teleporter_forest teleporter", "Far Shore to East Forest", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } }),
                    new TunicPortal("Fortress Arena", "teleporter_spidertank", "Far Shore to Fortress", entryItems: new Dictionary<string, int> { { "12", 1 }, { "Fortress Basement, Fortress Main_", 1 }, {"Fortress Courtyard, Overworld Redux_", 1}, { "Fortress Courtyard, Fortress Main_", 1 } }),
                    new TunicPortal("Atoll Redux", "teleporter_atoll", "Far Shore to Atoll"),
                    new TunicPortal("ziggurat2020_FTRoom", "teleporter_ziggurat teleporter", "Far Shore to Zig"),
                    new TunicPortal("Spirit Arena", "teleporter_spirit arena", "Far Shore to Heir"),
                    new TunicPortal("Overworld Redux", "teleporter_town", "Far Shore to Town"),
                    new TunicPortal("Overworld Redux", "teleporter_starting island", "Far Shore to Spawn", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } }),

                    // new TunicPortal("Transit_", "teleporter_", "Portal"), // Unused portal, far away and not enabled
                }
            },
            {
                "Spirit Arena", // Heir fight
                new List<TunicPortal>
                {
                    new TunicPortal("Transit", "teleporter_spirit arena", "Heir Arena Exit", deadEnd: true),
                }
            },
            {
                "Purgatory", // Second save hallway
                new List<TunicPortal>
                {
                    new TunicPortal("Purgatory", "bottom", "Purgatory Bottom Exit"),
                    new TunicPortal("Purgatory", "top", "Purgatory Top Exit"),
                }
            },
        };

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
            List<string> deadEndNames = new List<string> { "g_elements", "Sword Cave", "EastFiligreeCache", "Overworld Cave", "Maze Room", "Town Basement", "ShopSpecial", "archipelagos_house", "Library Arena", "Dusty", "Mountaintop", "RelicVoid", "Spirit Arena" };
            // making a separate lists for portals connected to one, two, or three+ regions, to be populated by the foreach coming up next
            List<Portal> deadEndPortals = new List<Portal>();
            List<Portal> twoPlusPortals = new List<Portal>();

            // separate the portals into their respective lists
            foreach (KeyValuePair<string, List<TunicPortal>> region_group in PortalList) {
                string region_name = region_group.Key;
                List<TunicPortal> region_portals = region_group.Value;
                foreach (TunicPortal portal in region_portals)
                {
                    Portal newPortal = new Portal(destination: portal.Destination, tag: portal.DestinationTag, name: portal.PortalName, scene: region_name, requiredItems: portal.RequiredItems, requiredItemsOr: portal.RequiredItemsOr, entryItems: portal.EntryItems, givesAccess: portal.GivesAccess, deadEnd: portal.DeadEnd, prayerPortal: portal.PrayerPortal, oneWay: portal.OneWay, cantReach: portal.CantReach);
                    if (newPortal.DeadEnd == true || newPortal.OneWay == true)
                    {
                        deadEndPortals.Add(newPortal);
                    }
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
            while (accessibleRegions.Count < 43)
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
            int shopCount = 8;
            int regionNumber = 0;
            while (shopCount > 0)
            {
                // manually making a portal for the shop, because it has some special properties
                Portal shopPortal = new Portal(destination: "Previous Region", tag: "", name: "Shop portal", scene: "Shop", requiredItems: new Dictionary<string, int>(), givesAccess: new List<string>(), deadEnd: true, prayerPortal: false, oneWay: false, cantReach: false);
                // check that a shop has not already been added to this region, since two shops in the same region causes problems
                if (!shopRegionList.Contains(twoPlusPortals[regionNumber].Scene))
                {
                    comboNumber++;
                    shopRegionList.Add(twoPlusPortals[regionNumber].Scene);
                    RandomizedPortals.Add(comboNumber.ToString(), new PortalCombo(twoPlusPortals[regionNumber], shopPortal));
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
            
            // now we have every region accessible
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
            var Portals = Resources.FindObjectsOfTypeAll<ScenePortal>();
            foreach (var portal in Portals)
            {
                // go through the list of randomized portals and see if either the first or second portal matches the one we're looking at
                foreach (KeyValuePair<string, PortalCombo> portalCombo in portalComboList)
                {
                    string comboTag = portalCombo.Key;
                    Portal portal1 = portalCombo.Value.Portal1;
                    Portal portal2 = portalCombo.Value.Portal2;

                    if (portal1.Scene == loadingScene.name && portal1.Tag == portal.id && portal1.Destination == portal.destinationSceneName)
                    {
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
                        break;
                    }


                    if (portal2.Scene == loadingScene.name && portal2.Tag == portal.id && portal2.Destination == portal.destinationSceneName)
                    {
                        if (portal1.Scene == "Shop")
                        {
                            portal.destinationSceneName = portal1.Scene;
                            portal.id = "";
                            portal.optionalIDToSpawnAt = "";
                        }
                        else
                        {
                            portal.destinationSceneName = portal1.Scene;
                            portal.id = comboTag + comboTag + comboTag + comboTag;
                            portal.optionalIDToSpawnAt = comboTag; // quadrupling since doubling and tripling can have overlaps
                        }
                        break;
                    }
                }
            }
        }
        // this is for use in PlayerCharacterPatches. Will need to refactor later if we do random player spawn
        public static void AltModifyPortals(Dictionary<string, PortalCombo> portalComboList)
        {
            var Portals = Resources.FindObjectsOfTypeAll<ScenePortal>();
            foreach (var portal in Portals)
            {
                // go through the list of randomized portals and see if either the first or second portal matches the one we're looking at
                foreach (KeyValuePair<string, PortalCombo> portalCombo in portalComboList)
                {
                    string comboTag = portalCombo.Key;
                    Portal portal1 = portalCombo.Value.Portal1;
                    Portal portal2 = portalCombo.Value.Portal2;

                    if (portal1.Scene == "Overworld Redux" && portal1.Tag == portal.id && portal1.Destination == portal.destinationSceneName)
                    {
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
                        break;
                    }


                    if (portal2.Scene == "Overworld Redux" && portal2.Tag == portal.id && portal2.Destination == portal.destinationSceneName)
                    {
                        if (portal1.Scene == "Shop")
                        {
                            portal.destinationSceneName = portal1.Scene;
                            portal.id = "";
                            portal.optionalIDToSpawnAt = "";
                        }
                        else
                        {
                            portal.destinationSceneName = portal1.Scene;
                            portal.id = comboTag + comboTag + comboTag + comboTag;
                            portal.optionalIDToSpawnAt = comboTag; // quadrupling since doubling and tripling can have overlaps
                        }
                        break;
                    }
                }
            }
        }
    }
}
