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
        public static Dictionary<string, PortalCombo> RandomizedPortals = new Dictionary<string, PortalCombo>();

        public class TunicPortal
        {
            public string SceneName; // the scene the portal is in
            public string Destination; // the vanilla destination scene
            public string DestinationTag; // the vanilla destination tag, aka ID
            public string PortalName; // a human-readable name for the portal
            public string GranularRegion; // a sub-region name, if there is one for that scene. For use in making sure everything can be accessed
            public Dictionary<string, int> RequiredItems; // required items if there is only one item or one set of items required. A string like "scene, destination_tag" counts as an item.
            public List<Dictionary<string, int>> RequiredItemsOr; // required items if there are multiple different possible requirements. A string like "scene, destination_tag" counts as an item.
            public List<string> GivesAccess; // portals that you are given access to by this portal. ex: the dance fox portal to the lower east forest portal in guardhouse 1.
            public Dictionary<string, int> EntryItems; // portals that require items to enter, but not exit from. ex: hero's graves, the yellow prayer portal pads, and the fountain holy cross door in overworld.
            public bool DeadEnd; // portals that are dead ends, like stick house or the gauntlet lower entry.
            public bool PrayerPortal; // portals that require prayer to enter. This is a more convenient version of GivesAccess for prayer portals.
            public bool OneWay; // portals that are one-way, such as the back entrance to monastery and the forest belltower top portal
            public bool IgnoreScene; // portals that cannot reach the center of the region, and as such do not give region access, like the rail between bottom of the well and furnace

            public TunicPortal() { }

            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, bool prayerPortal = false, bool deadEnd = false, bool oneWay = false, bool ignoreScene = false)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                PrayerPortal = prayerPortal;
                DeadEnd = deadEnd;
                OneWay = oneWay;
                IgnoreScene = ignoreScene;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, Dictionary<string, int> entryItems, bool prayerPortal = false, bool deadEnd = false, bool oneWay = false, bool ignoreScene = false)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                EntryItems = entryItems;
                PrayerPortal = prayerPortal;
                DeadEnd = deadEnd;
                OneWay = oneWay;
                IgnoreScene = ignoreScene;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, Dictionary<string, int> requiredItems, bool prayerPortal = false, bool deadEnd = false, bool ignoreScene = false)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                RequiredItems = requiredItems;
                PrayerPortal = prayerPortal;
                DeadEnd = deadEnd;
                IgnoreScene = ignoreScene;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, List<string> givesAccess, bool ignoreScene = false, bool oneWay = false)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                GivesAccess = givesAccess;
                IgnoreScene = ignoreScene;
                OneWay = oneWay;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, Dictionary<string, int> requiredItems)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                RequiredItems = requiredItems;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, Dictionary<string, int> requiredItems, List<string> givesAccess, bool ignoreScene = false)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                RequiredItems = requiredItems;
                IgnoreScene = ignoreScene;
                GivesAccess = givesAccess;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, List<Dictionary<string, int>> requiredItemsOr, bool prayerPortal = false, bool ignoreScene = false)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                RequiredItemsOr = requiredItemsOr;
                PrayerPortal = prayerPortal;
                IgnoreScene = ignoreScene;
            }

            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, Dictionary<string, int> requiredItems, List<string> givesAccess)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
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
                    new TunicPortal("Sword Cave", "", "Stick House Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Windmill", "", "Windmill Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Sewer", "entrance", "Well Ladder Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Sewer", "west_aqueduct", "Entrance to Well from Well Rail", granularRegion: "Overworld Well to Furnace Rail", ignoreScene: true, givesAccess: new List<string> { "Overworld Redux, Furnace_gyro_upper_north" }, requiredItems: new Dictionary<string, int> { { "Overworld Redux, Furnace_gyro_upper_north", 1 } }),
                    new TunicPortal("Overworld Interiors", "house", "Old House Entry Door", granularRegion: "Overworld", requiredItems: new Dictionary<string, int> { {"Key (House)", 1} }), // make this match actual item name
                    new TunicPortal("Overworld Interiors", "under_checkpoint", "Old House Waterfall Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Furnace", "gyro_upper_north", "Entrance to Furnace from Well Rail", granularRegion: "Overworld Well to Furnace Rail", ignoreScene: true, givesAccess: new List<string> { "Overworld Redux, Sewer_west_aqueduct" }, requiredItems: new Dictionary<string, int> { { "Overworld Redux, Sewer_west_aqueduct", 1 } }),
                    new TunicPortal("Furnace", "gyro_upper_east", "Entrance to Furnace from Windmill", granularRegion: "Overworld"),
                    new TunicPortal("Furnace", "gyro_west", "Entrance to Furnace from West Garden", granularRegion: "Overworld", ignoreScene: true, givesAccess: new List<string> {"Overworld Redux, Archipelagos Redux_lower"}, requiredItems: new Dictionary<string, int> { { "Overworld Redux, Archipelagos Redux_lower", 1 } }),
                    new TunicPortal("Furnace", "gyro_lower", "Entrance to Furnace from Beach", granularRegion: "Overworld"),
                    new TunicPortal("Overworld Cave", "", "Rotating Lights Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Swamp Redux 2", "wall", "Swamp Upper Entrance", granularRegion: "Overworld", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1} }),
                    new TunicPortal("Swamp Redux 2", "conduit", "Swamp Lower Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Ruins Passage", "east", "Ruined Hall Entrance Not-Door", granularRegion: "Overworld"),
                    new TunicPortal("Ruins Passage", "west", "Ruined Hall Entrance Door", granularRegion: "Overworld", requiredItems: new Dictionary<string, int> { { "Key", 2 } }), // and access to any overworld portal, but we start in overworld so no need to put it here
                    new TunicPortal("Atoll Redux", "upper", "Atoll Upper Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Atoll Redux", "lower", "Atoll Lower Entrance", granularRegion: "Overworld"),
                    new TunicPortal("ShopSpecial", "", "Special Shop Entrance", granularRegion: "Overworld", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1} }),
                    new TunicPortal("Maze Room", "", "Maze Cave Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Archipelagos Redux", "upper", "West Garden Entrance by Belltower", granularRegion: "Overworld", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } }),
                    new TunicPortal("Archipelagos Redux", "lower", "West Garden Entrance by Dark Tomb", granularRegion: "Overworld", ignoreScene: true, givesAccess: new List<string> {"Overworld Redux, Furnace_gyro_west"}, requiredItems: new Dictionary<string, int> {{"Overworld Redux, Furnace_gyro_west", 1}}),
                    new TunicPortal("Archipelagos Redux", "lowest", "West Garden Laurel Entrance", granularRegion: "Overworld", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } }),
                    new TunicPortal("Temple", "main", "Temple Door Entrance", granularRegion: "Overworld", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Forest Belltower, Forest Boss Room_", 1 }, { "Overworld Redux, Archipelagos Redux_upper", 1 }, { "Stick", 1 } }, new Dictionary<string, int> { { "Forest Belltower, Forest Boss Room_", 1 }, { "Overworld Redux, Archipelagos Redux_upper", 1 }, { "Techbow", 1 } }, new Dictionary<string, int> { { "Forest Belltower, Forest Boss Room_", 1 }, { "Hyperdash", 1 }, { "Stick", 1 } }, new Dictionary<string, int> { { "Forest Belltower, Forest Boss Room_", 1 }, { "Hyperdash", 1 }, { "Techbow", 1 } } }),
                    new TunicPortal("Temple", "rafters", "Temple Rafters Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Ruined Shop", "", "Ruined Shop Entrance", granularRegion: "Overworld"),
                    new TunicPortal("PatrolCave", "", "Patrol Cave Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Town Basement", "beach", "Hourglass Cave Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Changing Room", "", "Changing Room Entrance", granularRegion: "Overworld"),
                    new TunicPortal("CubeRoom", "", "Cube Room Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Mountain", "", "Stairs from Overworld to Mountain", granularRegion: "Overworld"),
                    new TunicPortal("Fortress Courtyard", "", "Overworld to Fortress", granularRegion: "Overworld"),
                    new TunicPortal("Town_FiligreeRoom", "", "HC Room Entrance next to Changing Room", granularRegion: "Overworld", entryItems: new Dictionary<string, int> { { "21", 1 } }), // this is entry items because when you exit from this portal, you end up in front of the door
                    new TunicPortal("EastFiligreeCache", "", "Glass Cannon HC Room Entrance", granularRegion: "Overworld", requiredItems: new Dictionary<string, int> { { "21", 1 } }), // this is required items because when you exit from this portal, you end up behind the door
                    new TunicPortal("Darkwoods Tunnel", "", "Overworld to Quarry Connector", granularRegion: "Overworld"),
                    new TunicPortal("Crypt Redux", "", "Dark Tomb Main Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Forest Belltower", "", "Overworld to Forest Belltower", granularRegion: "Overworld"),
                    new TunicPortal("Transit", "teleporter_town", "Town Portal", granularRegion: "Overworld", prayerPortal: true),
                    new TunicPortal("Transit", "teleporter_starting island", "Spawn Portal", granularRegion: "Overworld", prayerPortal: true),
                    new TunicPortal("Waterfall", "", "Entrance to Fairy Cave", granularRegion: "Overworld"),

                    // new TunicPortal("_", "", "Portal"), // ?
                    // new TunicPortal("Forest Belltower_", "showfloordemo2022", "Portal (12)"), // ?
                    // new TunicPortal("DEMO_altEnd_", "", "_Portal (Secret Demo End)"), // ?
                }
            },
            {
                "Waterfall", // fairy cave
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Fairy Cave Exit", granularRegion: "Waterfall", deadEnd: true),
                }
            },
            {
                "Windmill",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Windmill Exit", granularRegion: "Windmill"),
                    new TunicPortal("Shop", "", "Windmill Shop", granularRegion: "Windmill"),
                }
            },
            {
                "Overworld Interiors", // House in town
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "house", "Front Door of Old House Exit", granularRegion: "Old House Front"),
                    new TunicPortal("g_elements", "", "Teleport to Glyph Tower", granularRegion: "Old House Front"),
                    new TunicPortal("Overworld Redux", "under_checkpoint", "Exit from Old House Back Door", granularRegion: "Old House Back", ignoreScene: true, requiredItems: new Dictionary<string, int> { { "Overworld Interiors, Overworld Redux_house", 1 } }), // since you get access to the center of a region from either portal, only one of these two is actually needed

                    // new TunicPortal("Archipelagos Redux_", "", "_ShowfloorDemo2022 Portal"), // unused and disabled
                }
            },
            {
                "g_elements", // Relic tower
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Interiors", "", "Glyph Tower Exit", granularRegion: "g_elements", deadEnd: true),
                }
            },
            {
                "Changing Room",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Changing Room Exit", granularRegion: "Changing Room", deadEnd: true),
                }
            },
            {
                "Town_FiligreeRoom", // the one next to the fountain
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Fountain HC Room Exit", granularRegion: "Town_FiligreeRoom", deadEnd: true),
                }
            },
            {
                "CubeRoom",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Cube Room Exit", granularRegion: "CubeRoom", deadEnd: true),
                }
            },
            {
                "PatrolCave",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Guard Patrol Cave Exit", granularRegion: "PatrolCave", deadEnd: true),
                }
            },
            {
                "Ruined Shop",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Ruined Shop Exit", granularRegion: "Ruined Shop", deadEnd: true),
                }
            },
            {
                "Furnace", // Under the west belltower
                // I'm calling the "center" of this region the space accessible by the windmill and beach
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "gyro_upper_north", "Furnace to Well Rail", granularRegion: "Furnace", requiredItems: new Dictionary<string, int> { {"Hyperdash", 1} }),
                    new TunicPortal("Crypt Redux", "", "Furnace to Dark Tomb", granularRegion: "Furnace", requiredItems: new Dictionary<string, int> { {"Hyperdash", 1} }, givesAccess: new List<string> {"Furnace, Overworld Redux_gyro_west"}),
                    new TunicPortal("Overworld Redux", "gyro_west", "Furnace to West Garden", granularRegion: "Furnace", requiredItems : new Dictionary<string, int> { {"Hyperdash", 1} }, givesAccess : new List<string> {"Furnace, Crypt Redux_"}),
                    new TunicPortal("Overworld Redux", "gyro_lower", "Furnace to Beach", granularRegion: "Furnace"),
                    new TunicPortal("Overworld Redux", "gyro_upper_east", "Furnace to Windmill", granularRegion: "Furnace"),
                }
            },
            {
                "Sword Cave", // Stick house
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Stick House Exit", granularRegion: "Sword Cave", deadEnd: true),
                }
            },
            {
                "Ruins Passage", // That little hallway with the key door near the start in Overworld
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "east", "Ruined Hall Not-Door Exit", granularRegion: "Ruined Hall"),
                    new TunicPortal("Overworld Redux", "west", "Ruined Hall Door Exit", granularRegion: "Ruined Hall"),
                }
            },
            {
                "EastFiligreeCache", // The holy cross room with the 3 chests near swamp entrance
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Glass Cannon HC Room Exit", granularRegion: "EastFiligreeCache", deadEnd: true),
                }
            },
            {
                "Overworld Cave", // East beach, next to swamp entrance, rotating lights room
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Rotating Lights Exit", granularRegion: "Overworld Cave", deadEnd: true),
                }
            },
            {
                "Maze Room", // Invisible maze
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Maze Cave Exit", granularRegion: "Maze Room", deadEnd: true),
                }
            },
            {
                "Town Basement", // Hourglass cave
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "beach", "Hourglass Cave Exit", granularRegion: "Town Basement", deadEnd: true), // yes, it has a tag even though it doesn't need one
                }
            },
            {
                "ShopSpecial", // Special shop, laurel across from that platform between east forest and fortress
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Special Shop Exit", granularRegion: "ShopSpecial", deadEnd: true),
                }
            },
            {
                "Temple", // Where you put the hexes
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "rafters", "Temple Rafters Exit", granularRegion: "Temple"),
                    new TunicPortal("Overworld Redux", "main", "Temple Door Exit", granularRegion: "Temple"),
                }
            },
            {
                "Sewer", // Bottom of the Well
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "entrance", "Well Ladder Exit", granularRegion: "Sewer"),
                    new TunicPortal("Sewer_Boss", "", "Well to Well Boss", granularRegion: "Sewer"),
                    new TunicPortal("Overworld Redux", "west_aqueduct", "Well Rail Exit", granularRegion: "Sewer"),
                }
            },
            {
                "Sewer_Boss", // Boss room in the Bottom of the Well
                new List<TunicPortal>
                {
                    new TunicPortal("Sewer", "", "Well Boss to Well", granularRegion: "Sewer_Boss"),
                    new TunicPortal("Crypt Redux", "", "Checkpoint to Dark Tomb", granularRegion: "Sewer_Boss", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Sewer_Boss, Sewer_", 1 } }, new Dictionary<string, int> { { "Hyperdash", 1 } } }),
                }
            },
            {
                "Crypt Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Dark Tomb to Overworld", granularRegion: "Crypt Redux"),
                    new TunicPortal("Furnace", "", "Dark Tomb to Furnace", granularRegion: "Crypt Redux", requiredItems: new Dictionary<string, int> { {"Lantern", 1} }),
                    new TunicPortal("Sewer_Boss", "", "Dark Tomb to Checkpoint", granularRegion: "Crypt Redux"),
                }
            },
            {
                "Archipelagos Redux", // West Garden
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "lower", "West Garden towards Dark Tomb", granularRegion: "West Garden"),
                    new TunicPortal("archipelagos_house", "", "Magic Dagger House Entrance", granularRegion: "West Garden"),
                    new TunicPortal("Overworld Redux", "upper", "West Garden after Boss", granularRegion: "West Garden", ignoreScene: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Sword", 1 }, { "Archipelagos Redux, Overworld Redux_lower", 1 } }, new Dictionary<string, int> { { "Hyperdash", 1 }, {"Archipelagos Redux", 1 } } }),
                    new TunicPortal("Shop", "", "West Garden Shop", granularRegion: "West Garden"), // there's two of these, one is unused and disabled
                    new TunicPortal("Overworld Redux", "lowest", "West Garden Laurel Exit", granularRegion: "West Garden", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } }),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "West Garden Hero's Grave", granularRegion: "West Garden", prayerPortal: true), // Hero grave
                    new TunicPortal("Transit", "teleporter_archipelagos_teleporter", "West Garden Portal", granularRegion: "West Garden Portal", prayerPortal: true, deadEnd: true), // Portal to the thing behind dagger house
                }
            },
            {
                "archipelagos_house", // Magic Dagger house in West Garden
                new List<TunicPortal>
                {
                    new TunicPortal("Archipelagos Redux", "", "Magic Dagger House Exit", granularRegion: "Magic Dagger House", deadEnd: true),
                }
            },
            {
                "Atoll Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Frog Stairs", "eye", "Frog Eye Entrance", granularRegion: "Atoll"),
                    new TunicPortal("Library Exterior", "", "Atoll to Library", granularRegion: "Atoll", prayerPortal: true),
                    new TunicPortal("Overworld Redux", "upper", "Upper Atoll Exit", granularRegion: "Atoll"),
                    new TunicPortal("Overworld Redux", "lower", "Lower Atoll Exit", granularRegion: "Atoll", requiredItems: new Dictionary<string, int> {{"Hyperdash", 1}}),
                    new TunicPortal("Frog Stairs", "mouth", "Frog Mouth Entrance", granularRegion: "Atoll", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Wand", 1 } }, new Dictionary<string, int> { { "Hyperdash", 1 } } }),
                    new TunicPortal("Shop", "", "Atoll Shop", granularRegion: "Atoll"),
                    new TunicPortal("Transit", "teleporter_atoll", "Atoll Portal", granularRegion: "Atoll", prayerPortal: true),
                    // new TunicPortal("Forest Lake_", "teleporter", "Portal"), // Unused portal, same spot as library portal
                }
            },
            {
                "Frog Stairs", // Entrance to frog's domain
                new List<TunicPortal>
                {
                    new TunicPortal("Atoll Redux", "mouth", "Frog Mouth Exit", granularRegion: "Frog Stairs"),
                    new TunicPortal("frog cave main", "Exit", "Upper Frog to Lower Frog Exit", granularRegion: "Frog Stairs"),
                    new TunicPortal("Atoll Redux", "eye", "Frog Eye Exit", granularRegion: "Frog Stairs"),
                    new TunicPortal("frog cave main", "Entrance", "Upper Frog to Lower Frog Entrance", granularRegion: "Frog Stairs"),
                }
            },
            {
                "frog cave main", // Frog's domain, yes it's lowercase
                new List<TunicPortal>
                {
                    new TunicPortal("Frog Stairs", "Exit", "Lower Frog Orb Exit", granularRegion: "Frog's Domain Back", ignoreScene: true, requiredItems: new Dictionary<string, int> { { "Wand", 1 }, { "frog cave main, Frog Stairs_Entrance", 1 } }),
                    new TunicPortal("Frog Stairs", "Entrance", "Lower Frog Ladder Exit", granularRegion: "Frog's Domain Front", ignoreScene: true, oneWay: true),
                }
            },
            {
                "Library Exterior",
                new List<TunicPortal>
                {
                    new TunicPortal("Library Hall", "", "Library Entry Ladder", granularRegion: "Library Exterior", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Hyperdash", 1 } }, new Dictionary<string, int> { { "Wand", 1} } }),
                    new TunicPortal("Atoll Redux", "", "Library to Atoll", granularRegion: "Library Exterior", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Hyperdash", 1 }, { "12", 1 } }, new Dictionary<string, int> { { "Wand", 1}, { "12", 1 } } }),
                }
            },
            {
                "Library Hall", // Entry area with hero grave
                new List<TunicPortal>
                {
                    new TunicPortal("Library Rotunda", "", "Lower Library to Rotunda", granularRegion: "Library Hall"),
                    new TunicPortal("Library Exterior", "", "Library Bookshelf Exit", granularRegion: "Library Hall"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Library Hero's Grave", granularRegion: "Library Hall", prayerPortal: true),
                }
            },
            {
                "Library Rotunda", // The circular room with the ladder
                new List<TunicPortal>
                {
                    new TunicPortal("Library Hall", "", "Library Rotunda Lower Exit", granularRegion: "Library Rotunda"),
                    new TunicPortal("Library Lab", "", "Library Rotunda Upper Exit", granularRegion: "Library Rotunda"),
                }
            },
            {
                "Library Lab",
                new List<TunicPortal>
                {
                    new TunicPortal("Library Arena", "", "Upper Library to Librarian", granularRegion: "Library Lab", ignoreScene: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Hyperdash", 1 }, { "Library Lab", 1 } }, new Dictionary<string, int> { { "Wand", 1}, {"Library Lab", 1 } } }),
                    new TunicPortal("Library Rotunda", "", "Upper Library to Rotunda", granularRegion: "Library Lab", ignoreScene: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Hyperdash", 1 }, { "Library Lab", 1 } }, new Dictionary<string, int> { { "Wand", 1}, {"Library Lab, Library Rotunda_", 1 } } }),
                    new TunicPortal("Transit", "teleporter_library teleporter", "Library Portal", granularRegion: "Library Lab", ignoreScene: true, prayerPortal: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Hyperdash", 1 }, { "Library Lab, Library Rotunda_", 1 } }, new Dictionary<string, int> { { "Wand", 1}, {"Library Lab, Library Rotunda_", 1 } }, new Dictionary<string, int> { { "Library Lab", 1 } } }),
                }
            },
            {
                "Library Arena",
                new List<TunicPortal>
                {
                    new TunicPortal("Library Lab", "", "Library Librarian Arena Exit", granularRegion: "Library Arena", deadEnd: true),
                }
            },
            {
                "East Forest Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Sword Access", "lower", "Forest Grave Path Lower Entrance", granularRegion: "East Forest"),
                    new TunicPortal("East Forest Redux Laddercave", "upper", "Forest Fox Dance Outside Doorway", granularRegion: "East Forest", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } } ),
                    new TunicPortal("East Forest Redux Interior", "lower", "Forest Guard House 2 Lower Entrance", granularRegion: "East Forest"),
                    new TunicPortal("East Forest Redux Laddercave", "gate", "Forest Guard House 1 Gate Entrance", granularRegion: "East Forest"),
                    new TunicPortal("Sword Access", "upper", "Forest Grave Path Upper Entrance", granularRegion: "East Forest"),
                    new TunicPortal("East Forest Redux Interior", "upper", "Forest Guard House 2 Upper Entrance", granularRegion: "East Forest"),
                    new TunicPortal("East Forest Redux Laddercave", "lower", "Forest Guard House 1 Lower Entrance", granularRegion: "East Forest"),
                    new TunicPortal("Forest Belltower", "", "Forest to Belltower", granularRegion: "East Forest"),
                    new TunicPortal("Transit", "teleporter_forest teleporter", "Forest Portal", granularRegion: "East Forest", prayerPortal: true),
                }
            },
            {
                "East Forest Redux Laddercave", // the place with the two ladders that leads to the boss room
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux", "upper", "Guard House 1 Dance Exit", "Laddercave", ignoreScene: true, givesAccess: new List<string> { "East Forest Redux Laddercave, East Forest Redux_upper" }, requiredItems: new Dictionary<string, int> { { "East Forest Redux Laddercave, East Forest Redux_gate", 1 } }), // making the upper ones the "center" for easier logic writing
                    new TunicPortal("East Forest Redux", "lower", "Guard House 1 Lower Exit", "Laddercave", ignoreScene: true, givesAccess: new List<string> { "East Forest Redux Laddercave, East Forest Redux_lower" }, requiredItems: new Dictionary<string, int> { { "East Forest Redux Laddercave, East Forest Redux_gate", 1 } }),
                    new TunicPortal("East Forest Redux", "gate", "Guard House 1 Upper Forest Exit", "Laddercave"),
                    new TunicPortal("Forest Boss Room", "", "Guard House 1 to Guard Captain Room", "Laddercave"),
                }
            },
            {
                "Sword Access", // East forest hero grave area
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux", "upper", "Upper Forest Grave Path Exit", granularRegion: "Sword Access", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } }),
                    new TunicPortal("East Forest Redux", "lower", "Lower Forest Grave Path Exit", granularRegion: "Sword Access"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "East Forest Hero's Grave", granularRegion: "Sword Access Back", ignoreScene: true, prayerPortal: true, requiredItems: new Dictionary<string, int> { {"Sword Access, East Forest Redux_lower", 1 } }), // Can't open the gate from behind
                    
                    // new TunicPortal("Forest 1_", "lower", "Portal (1)"),
                    // new TunicPortal("Forest 1_", "", "Portal"),
                    // new TunicPortal("Forest 1_", "upper", "Portal (2)"),
                }
            },
            {
                "East Forest Redux Interior", // Guardhouse 2
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux", "lower", "Guard House 2 Lower Exit", "Guardhouse 2"),
                    new TunicPortal("East Forest Redux", "upper", "Guard House 2 Upper Exit", "Guardhouse 2"),
                }
            },
            {
                "Forest Boss Room",
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux Laddercave", "", "Guard Captain Room Non-Gate Exit", "Forest Boss"), // entering it from behind puts you in the room, not behind the gate
                    new TunicPortal("Forest Belltower", "", "Guard Captain Room Gate Exit", "Forest Boss"),

                    // new TunicPortal("Archipelagos Redux_", "showfloordemo2022", "Portal (2)"),
                }
            },
            {
                "Forest Belltower",
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Courtyard", "", "Forest Belltower to Fortress", granularRegion: "Forest Belltower Main"),
                    new TunicPortal("East Forest Redux", "", "Forest Belltower to Forest", granularRegion: "Forest Belltower Lower"),
                    new TunicPortal("Overworld Redux", "", "Forest Belltower to Overworld", granularRegion: "Forest Belltower Main"),
                    new TunicPortal("Forest Boss Room", "", "Forest Belltower to Guard Captain Room", granularRegion: "Forest Belltower Upper", ignoreScene: true, oneWay: true),
                }
            },
            {
                "Fortress Courtyard", // Outside the fortress, the area connected to east forest and overworld. Center of the area is on the fortress-side of the bridge
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Reliquary", "Lower", "Lower Fortress Grave Path Entrance", granularRegion: "Fortress Courtyard"),
                    new TunicPortal("Fortress Reliquary", "Upper", "Upper Fortress Grave Path Entrance", granularRegion: "Fortress Courtyard Upper", ignoreScene: true, oneWay: true, givesAccess: new List<string> { "Fortress Courtyard, Fortress East_" }),
                    new TunicPortal("Fortress Main", "Big Door", "Fortress Courtyard to Fortress Interior", granularRegion: "Fortress Courtyard"),
                    new TunicPortal("Fortress East", "", "Fortress Courtyard to Fortress East", granularRegion: "Fortress Courtyard Upper", ignoreScene: true, oneWay: true, givesAccess: new List<string> { "Fortress Courtyard, Fortress Reliquary_Upper" }),
                    new TunicPortal("Fortress Basement", "", "Fortress Courtyard to Beneath the Earth", granularRegion: "Fortress Courtyard", ignoreScene: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "12", 1 }, { "Fortress Courtyard, Overworld Redux_", 1 } }, new Dictionary<string, int> { { "Hyperdash", 1 }, { "Fortress Courtyard", 1 } }, new Dictionary<string, int> { {"Fortress Courtyard, Shop_", 1 } } }),
                    new TunicPortal("Forest Belltower", "", "Fortress Courtyard to Forest Belltower", granularRegion: "Fortress Courtyard", requiredItems: new Dictionary<string, int>{ { "Hyperdash", 1 } }),
                    new TunicPortal("Overworld Redux", "", "Fortress Courtyard to Overworld", granularRegion: "Fortress Courtyard", ignoreScene: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { {"Hyperdash", 1}, { "Fortress Courtyard", 1 } }, new Dictionary<string, int> { {"Fortress Courtyard, Fortress East_", 1} }, new Dictionary<string, int> { { "Wand", 1 }, { "Fortress Courtyard, Forest Belltower_", 1 } } }), // remember, required items is just what you need to get to the center of a region -- prayer only gets you to the shop and beneath the earth
                    new TunicPortal("Shop", "", "Fortress Courtyard Shop", granularRegion: "Fortress Courtyard", ignoreScene: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "12", 1 }, { "Fortress Courtyard, Overworld Redux_", 1 } }, new Dictionary<string, int> { { "Hyperdash", 1 }, { "Fortress Courtyard", 1 } }, new Dictionary<string, int> { {"Fortress Courtyard, Fortress Basement_", 1 } } }),

                    // new TunicPortal("Overworld Redux_", "", "Portal (4)"), // unused and disabled
                }
            },
            {
                "Fortress Basement", // Under the fortress
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Main", "", "Beneath the Earth to Fortress Interior", "Fortress Basement"),
                    new TunicPortal("Fortress Courtyard", "", "Beneath the Earth to Fortress Courtyard", "Fortress Basement"),
                }
            },
            {
                "Fortress Main", // Inside the fortress
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Courtyard", "Big Door", "Fortress Main Exit", "Fortress Main"),
                    new TunicPortal("Fortress Basement", "", "Fortress Interior to Beneath the Earth", "Fortress Main"),
                    new TunicPortal("Fortress Arena", "", "Fortress Interior to Siege Engine", "Fortress Main", requiredItems: new Dictionary<string, int> { { "12", 1 }, {"Fortress Courtyard, Overworld Redux_", 1}, { "Fortress Courtyard, Fortress Reliquary_upper", 1 }, {"Fortress Main, Fortress Courtyard_Big Door", 1 } }), // requires that one prayer thing to be down
                    new TunicPortal("Shop", "", "Fortress Interior Shop", "Fortress Main"),
                    new TunicPortal("Fortress East", "upper", "Fortress Interior to East Fortress Upper", "Fortress Main"),
                    new TunicPortal("Fortress East", "lower", "Fortress Interior to East Fortress Lower", "Fortress Main"),
                }
            },
            {
                "Fortress East", // that tiny area with the couple mages up high, and the ladder in the lower right
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Main", "lower", "East Fortress to Interior Lower", granularRegion: "Fortress East Lower", ignoreScene: true, requiredItems: new Dictionary<string, int> { { "Fortress East, Fortress Main_upper", 1} }),
                    new TunicPortal("Fortress Courtyard", "", "East Fortress to Courtyard", granularRegion: "Fortress East"),
                    new TunicPortal("Fortress Main", "upper", "East Fortress to Interior Upper", granularRegion: "Fortress East"),
                }
            },
            {
                "Fortress Reliquary", // Where the grave is
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Courtyard", "Lower", "Lower Fortress Grave Path Exit", granularRegion: "Fortress Grave Path"),
                    new TunicPortal("Dusty", "", "Fortress Grave Path Dusty Entrance", granularRegion: "Fortress Grave Path", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } }),
                    new TunicPortal("Fortress Courtyard", "Upper", "Upper Fortress Grave Path Exit", granularRegion: "Fortress Grave Path Upper", deadEnd: true),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Fortress Hero's Grave", granularRegion: "Fortress Grave Path", prayerPortal: true),
                }
            },
            {
                "Fortress Arena", // Where the boss is
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Main", "", "Siege Engine Arena to Fortress", "Fortress Arena"),
                    new TunicPortal("Transit", "teleporter_spidertank", "Fortress Portal", "Fortress Arena", entryItems: new Dictionary<string, int> { { "12", 1 }, { "Fortress Basement, Fortress Main_", 1 }, {"Fortress Main, Fortress Courtyard_", 1}, { "Fortress Courtyard, Fortress Main_", 1 } }),
                    // new TunicPortal("Fortress Main_", "", "Portal"), // There's two of these, one is disabled
                }
            },
            {
                "Dusty", // broom
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Reliquary", "", "Dusty Exit", "Dusty", deadEnd: true),
                }
            },
            {
                "Mountain",
                new List<TunicPortal>
                {
                    new TunicPortal("Mountaintop", "", "Stairs to Top of the Mountain", "Mountain", requiredItems: new Dictionary<string, int> { { "21", 1 } }),
                    new TunicPortal("Quarry Redux", "", "Mountain to Quarry", "Mountain"),
                    new TunicPortal("Overworld Redux", "", "Mountain to Overworld", "Mountain"),
                }
            },
            {
                "Mountaintop",
                new List<TunicPortal>
                {
                    new TunicPortal("Mountain", "", "Top of the Mountain Exit", "Mountaintop", deadEnd: true),
                }
            },
            {
                "Darkwoods Tunnel", // connector between overworld and quarry
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Quarry Connector to Overworld", "Darkwoods"),
                    new TunicPortal("Quarry Redux", "", "Quarry Connector to Quarry", "Darkwoods"),
                }
            },
            {
                "Quarry Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Darkwoods Tunnel", "", "Quarry to Overworld Exit", granularRegion: "Quarry"),
                    new TunicPortal("Shop", "", "Quarry Shop", granularRegion: "Quarry"),
                    new TunicPortal("Monastery", "front", "Quarry to Monastery Front", granularRegion: "Quarry"),
                    new TunicPortal("Monastery", "back", "Quarry to Monastery Back", granularRegion: "Monastery Rope", ignoreScene: true, oneWay: true),
                    new TunicPortal("Mountain", "", "Quarry to Mountain", granularRegion: "Quarry"),
                    new TunicPortal("ziggurat2020_0", "", "Quarry Zig Entrance", granularRegion: "Quarry", entryItems: new Dictionary<string, int> { { "Wand", 1 }, { "Darkwood Tunnel, Quarry Redux_", 1 }, { "12", 1 } }),
                    new TunicPortal("Transit", "teleporter_quarry teleporter", "Quarry Portal", granularRegion: "Quarry", prayerPortal: true, entryItems: new Dictionary<string, int> { { "Wand", 1 }, { "Darkwood Tunnel, Quarry Redux_", 1 }, { "12", 1 } }),
                }
            },
            {
                "Monastery",
                new List<TunicPortal>
                {
                    new TunicPortal("Quarry Redux", "back", "Monastery Rear Exit", "Monastery"),
                    new TunicPortal("Quarry Redux", "front", "Monastery Front Exit", "Monastery"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Monastery Hero's Grave", "Monastery", prayerPortal: true),

                    // new TunicPortal("Quarry_", "lower", "Portal (1)"), // Unused portal, disabled, and very high up
                }
            },
            {
                "ziggurat2020_0", // Zig entrance hallway
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_1", "", "Zig Entry Hallway to Zig 1", "Zig 0"),
                    new TunicPortal("Quarry Redux", "", "Zig Entry Hallway to Quarry", "Zig 0"),
                }
            },
            {
                "ziggurat2020_1", // Upper zig
                new List<TunicPortal>
                {
                    // new TunicPortal("ziggurat2020_3", "zig2_skip", "Zig Skip"), // the elevator skip to lower zig, put a secret here later
                    new TunicPortal("ziggurat2020_0", "", "Zig 1 to Zig Entry", granularRegion: "Zig 1 Top", ignoreScene: true, oneWay: true),
                    new TunicPortal("ziggurat2020_2", "", "Zig 1 to Zig 2", granularRegion: "Zig 1 Bottom", deadEnd: true, ignoreScene: true, requiredItems: new Dictionary<string, int>{{"ziggurat2020_1, ziggurat2020_0_", 1}}),
                }
            },
            {
                "ziggurat2020_2", // Zig intermediate elevator
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_1", "", "Zig 2 to Zig 1", granularRegion: "Zig 2 Top", ignoreScene: true, oneWay: true),
                    new TunicPortal("ziggurat2020_3", "", "Zig 2 to Zig 3", granularRegion: "Zig 2 Bottom", deadEnd: true, ignoreScene: true, requiredItems: new Dictionary<string, int>{{"ziggurat2020_2, ziggurat2020_1_", 1}}),
                }
            },
            {
                "ziggurat2020_3", // Lower zig, center is designated as before the prayer spot with the two cube minibosses
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_FTRoom", "", "Zig Portal Room Entrance", granularRegion: "Zig 3", ignoreScene: true, prayerPortal: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { {"Hyperdash", 1 }, { "ziggurat2020_3, ziggurat2020_2_", 1 } }, new Dictionary<string, int> { { "12", 1 }, { "ziggurat2020_3, ziggurat2020_2_", 1 } } }), // Prayer portal room
                    // new TunicPortal("ziggurat2020_1", "zig2_skip", "Zig Skip Exit"), // the elevator skip to lower zig
                    new TunicPortal("ziggurat2020_2", "", "Zig 3 to Zig 2", granularRegion: "Zig 3"),
                }
            },
            {
                "ziggurat2020_FTRoom", // The room with the prayer portal
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_3", "", "Zig Portal Room Exit", "Zig Portal Room", ignoreScene: true, requiredItems: new Dictionary<string, int> { { "12", 1 }, { "ziggurat2020_3, ziggurat2020_FTRoom", 1 } }),
                    new TunicPortal("Transit", "teleporter_ziggurat teleporter", "Zig Portal", "Zig Portal Room", prayerPortal: true),
                }
            },
            {
                "Swamp Redux 2",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "conduit", "Lower Swamp Exit", granularRegion: "Swamp Front"),
                    new TunicPortal("Cathedral Redux", "main", "Swamp to Cathedral Main Entrance", granularRegion: "Swamp Front", requiredItems: new Dictionary<string, int> { { "12", 1 }, { "Hyperdash", 1 }, { "Overworld Redux, Swamp Redux 2_wall", 1 } } ),
                    new TunicPortal("Cathedral Redux", "secret", "Swamp to Cathedral Treasure Room Entrance", granularRegion: "Swamp Front", requiredItems: new Dictionary<string, int> { { "21", 1 } }),
                    new TunicPortal("Cathedral Arena", "", "Swamp to Gauntlet", granularRegion: "Swamp Back", ignoreScene: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Hyperdash", 1 }, {"Swamp Redux 2, Overworld Redux_wall", 1 } }, new Dictionary<string, int> { { "Swamp Redux 2, RelicVoid_teleporter_relic plinth", 1 } } }),
                    new TunicPortal("Shop", "", "Swamp Shop", granularRegion: "Swamp Front"),
                    new TunicPortal("Overworld Redux", "wall", "Upper Swamp Exit", granularRegion: "Swamp Back", ignoreScene: true, requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 }, { "Swamp Redux 2, Cathedral Arena_", 1 } }),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Swamp Hero's Grave", granularRegion: "Swamp Back", ignoreScene: true, prayerPortal: true, requiredItems: new Dictionary<string, int> { { "Swamp Redux 2, Cathedral Arena_", 1 } }),
                }
            },
            {
                "Cathedral Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Swamp Redux 2", "main", "Cathedral Main Exit", granularRegion: "Cathedral"),
                    new TunicPortal("Cathedral Arena", "", "Cathedral Elevator", granularRegion: "Cathedral"),
                    new TunicPortal("Swamp Redux 2", "secret", "Cathedral Treasure Room Exit", granularRegion: "Cathedral Secret Legend", ignoreScene: true, deadEnd: true), // only one chest, just use item access rules for it
                }
            },
            {
                "Cathedral Arena", // Gauntlet
                new List<TunicPortal>
                {
                    new TunicPortal("Swamp Redux 2", "", "Gauntlet to Swamp", granularRegion: "Gauntlet Bottom", ignoreScene: true, deadEnd: true, requiredItems: new Dictionary<string, int>{{"Cathedral Arena, Cathedral Redux_", 1}, {"Hyperdash", 1}}),
                    new TunicPortal("Cathedral Redux", "", "Gauntlet Elevator", granularRegion: "Gauntlet Top", ignoreScene: true, givesAccess: new List<string> {"Cathedral Arena, Shop_"}, requiredItems: new Dictionary<string, int>{{"Cathedral Arena, Shop_", 1}}),
                    new TunicPortal("Shop", "", "Gauntlet Shop", granularRegion: "Gauntlet Top", ignoreScene: true, givesAccess: new List<string> {"Cathedral Arena, Swamp Redux 2_"}, requiredItems: new Dictionary<string, int>{{"Cathedral Arena, Cathedral Redux_", 1}}), // we love gauntlet shop
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
                    new TunicPortal("Fortress Reliquary", "teleporter_relic plinth", "Hero Relic to Fortress", "RelicVoid", ignoreScene: true, deadEnd: true),
                    new TunicPortal("Monastery", "teleporter_relic plinth", "Hero Relic to Monastery", "RelicVoid", ignoreScene: true, deadEnd: true),
                    new TunicPortal("Archipelagos Redux", "teleporter_relic plinth", "Hero Relic to West Garden", "RelicVoid", ignoreScene: true, deadEnd: true),
                    new TunicPortal("Sword Access", "teleporter_relic plinth", "Hero Relic to East Forest", "RelicVoid", ignoreScene: true, deadEnd: true),
                    new TunicPortal("Library Hall", "teleporter_relic plinth", "Hero Relic to Library", "RelicVoid", ignoreScene: true, deadEnd: true),
                    new TunicPortal("Swamp Redux 2", "teleporter_relic plinth", "Hero Relic to Swamp", "RelicVoid", ignoreScene: true, deadEnd: true),
                }
            },
            {
                "Transit", // Teleporter hub
                new List<TunicPortal>
                {
                    new TunicPortal("Archipelagos Redux", "teleporter_archipelagos_teleporter", "Far Shore to West Garden", "Transit", entryItems: new Dictionary<string, int>{ { "12", 1 }, { "Archipelagos Redux, Overworld Redux_lower", 1} }),
                    new TunicPortal("Library Lab", "teleporter_library teleporter", "Far Shore to Library", "Transit", entryItems: new Dictionary<string, int>{ { "12", 1 }, { "Library Lab, Library Arena_", 1} }),
                    new TunicPortal("Quarry Redux", "teleporter_quarry teleporter", "Far Shore to Quarry", "Transit", entryItems: new Dictionary<string, int>{ { "12", 1 }, { "Quarry Redux, Darkwoods Tunnel_", 1 }, {"Darkwoods Tunnel, Quarry Redux_", 1 }, { "Wand", 1 } }),
                    new TunicPortal("East Forest Redux", "teleporter_forest teleporter", "Far Shore to East Forest", "Transit", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } }),
                    new TunicPortal("Fortress Arena", "teleporter_spidertank", "Far Shore to Fortress", "Transit", entryItems: new Dictionary<string, int> { { "12", 1 }, { "Fortress Basement, Fortress Main_", 1 }, {"Fortress Courtyard, Overworld Redux_", 1}, { "Fortress Courtyard, Fortress Main_", 1 } }),
                    new TunicPortal("Atoll Redux", "teleporter_atoll", "Far Shore to Atoll", "Transit"),
                    new TunicPortal("ziggurat2020_FTRoom", "teleporter_ziggurat teleporter", "Far Shore to Zig", "Transit"),
                    new TunicPortal("Spirit Arena", "teleporter_spirit arena", "Far Shore to Heir", "Transit"),
                    new TunicPortal("Overworld Redux", "teleporter_town", "Far Shore to Town", "Transit"),
                    new TunicPortal("Overworld Redux", "teleporter_starting island", "Far Shore to Spawn", "Transit", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } }),

                    // new TunicPortal("Transit_", "teleporter_", "Portal"), // Unused portal, far away and not enabled
                }
            },
            {
                "Spirit Arena", // Heir fight
                new List<TunicPortal>
                {
                    new TunicPortal("Transit", "teleporter_spirit arena", "Heir Arena Exit", "Heir Arena", deadEnd: true),
                }
            },
            {
                "Purgatory", // Second save hallway
                new List<TunicPortal>
                {
                    new TunicPortal("Purgatory", "bottom", "Purgatory Bottom Exit", "Purgatory"),
                    new TunicPortal("Purgatory", "top", "Purgatory Top Exit", "Purgatory"),
                }
            },
        };

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

        // function to see if we're placing a lock before its key, since doing that can possibly self-lock
        public static bool LockBeforeKey(Portal checkPortal)
        {
            if (checkPortal.SceneDestinationTag == "Overworld Redux, Temple_main")
            {
                // check if the belltower upper has been placed yet, if not then reshuffle the two plus portals list (since this list is gonna be the bigger one)
                int i = 0;
                foreach (Portal portal in deadEndPortals)
                {
                    if (portal.SceneDestinationTag == "Forest Belltower, Forest Boss Room_")
                    { 
                        i++;
                        break; 
                    }
                }
                if (i == 1)
                { return true; }
            }
            else if (checkPortal.SceneDestinationTag == "Fortress Main, Fortress Arena_")
            {
                // check if none of the portals that lead to the necessary fuses have been placed
                int i = 0;
                int j = 0;
                int k = 0;
                foreach (Portal portal in twoPlusPortals)
                {
                    if (portal.SceneDestinationTag == "Fortress Courtyard, Fortress Reliquary_upper" 
                        || portal.SceneDestinationTag == "Fortress Courtyard, Fortress East_")
                    { i++; }
                    if (portal.Scene == "Fortress Basement")
                    { j++; }
                    if (portal.Scene == "Fortress Main")
                    { k++; }
                }
                if (i == 2 || j == 2 || k == 6)
                { return true; }
            }
            else if (checkPortal.SceneDestinationTag == "Fortress Arena, Transit_teleporter_spidertank"
                || checkPortal.SceneDestinationTag == "Transit, Fortress Arena_teleporter_spidertank")
            {
                // check if none of the portals that lead to the necessary fuses have been placed
                int i = 0;
                int j = 0;
                int k = 0;
                foreach (Portal portal in twoPlusPortals)
                {
                    if (portal.Scene == "Fortress Courtyard")
                    { i++; }
                    if (portal.Scene == "Fortress Basement")
                    { j++; }
                    if (portal.Scene == "Fortress Main")
                    { k++; }
                }
                if (i == 8 || j == 2 || k == 6)
                { return true; }
            }
            else if (checkPortal.SceneDestinationTag == "Swamp Redux 2, Cathedral Redux_main")
            {
                int i = 0;
                foreach (Portal portal in twoPlusPortals)
                {
                    if (portal.SceneDestinationTag == "Swamp Redux 2, Overworld Redux_conduit" 
                        || portal.SceneDestinationTag == "Swamp Redux 2, Shop_"
                        || portal.SceneDestinationTag == "Swamp Redux 2, Cathedral Redux_secret")
                    { i++; }
                }
                if (i == 3)
                { return true; }
            }
            else if (checkPortal.SceneDestinationTag == "ziggurat2020_FTRoom, ziggurat2020_3")
            {
                int i = 0;
                foreach (Portal portal in twoPlusPortals)
                {
                    if (portal.Scene == "ziggurat2020_3")
                    { i++; }
                }
                if (i == 2)
                { return true; }
            }
            else if (checkPortal.SceneDestinationTag == "Quarry Redux, Transit_teleporter_quarry teleporter")
            {
                int i = 0;
                foreach (Portal portal in twoPlusPortals)
                {
                    if (portal.Scene == "Darkwoods Tunnel")
                    { i++; }
                }
                if (i == 2)
                { return true; }
            }
            else if (checkPortal.SceneDestinationTag == "Transit, Quarry Redux_teleporter_quarry teleporter")
            {
                int i = 0;
                int j = 0;
                foreach (Portal portal in twoPlusPortals)
                {
                    if (portal.Scene == "Darkwoods Tunnel")
                    { i++; }
                    if (portal.Scene == "Quarry Redux") 
                    { j++; }
                }
                if (i == 2 || j == 7)
                { return true; }
            }
            else if (checkPortal.SceneDestinationTag == "Transit, Library Lab_teleporter_library teleporter")
            {
                int i = 0;
                foreach (Portal portal in twoPlusPortals)
                {
                    if (portal.Scene == "Library Lab") 
                    { i++; }
                }
                if (i == 3)
                { return true; }
            }
            else if (checkPortal.SceneDestinationTag == "Transit, Archipelagos Redux_teleporter_archipelagos_teleporter")
            {
                int i = 0;
                foreach (Portal portal in twoPlusPortals)
                {
                    if (portal.Scene == "Archipelagos Redux")
                    { i++; }
                }
                if (i == 7)
                { return true; }
            }
            return false;
        }

        // if we have some granular regions, we get another one. This is for one-way connections, basically
        // so that we don't unnecessarily force the back of house to be connected to a non-dead-end, for example
        public static List<string> AddDependentRegions(string region) {
            List<string> regions = new List<string>();
            // idk if we need to clear it
            regions.Clear();
            regions.Add(region);

            if (region == "Old House Front") {
                regions.Add("Old House Back");
            }
            else if (region == "Frog's Domain Front") {
                regions.Add("Frog's Domain Back");
            }
            else if (region == "Sword Access") {
                regions.Add("Sword Access Back");
            }
            else if (region == "Forest Belltower Upper") {
                regions.Add("Forest Belltower Main");
                regions.Add("Forest Belltower Lower");
            }
            else if (region == "Forest Beltower Main") {
                regions.Add("Forest Belltower Lower");
            }
            else if (region == "Fortress Courtyard Upper") {
                regions.Add("Fortress Courtyard");
            }
            else if (region == "Fortress East") {
                regions.Add("Fortress East Lower");
            }
            else if (region == "Monastery Rope") {
                regions.Add("Quarry");
            }
            else if (region == "Zig 1 Top") {
                regions.Add("Zig 1 Bottom");
            }
            else if (region == "Zig 2 Top") {
                regions.Add("Zig 2 Bottom");
            }
            else if (region == "Gauntlet Top") {
                regions.Add("Gauntlet Bottom");
            }
    
            return regions;
        }
        
        // making a separate lists for portals connected to one, two, or three+ regions, to be populated by the foreach coming up next
        public static List<Portal> deadEndPortals = new List<Portal>();
        public static List<Portal> twoPlusPortals = new List<Portal>();
        // create a list of all portals with their information loaded in, just a slightly expanded version of the above to include destinations
        public static void RandomizePortals(int seed)
        {
            RandomizedPortals.Clear();

            // separate the portals into their respective lists
            foreach (KeyValuePair<string, List<TunicPortal>> region_group in PortalList) {
                string region_name = region_group.Key;
                List<TunicPortal> region_portals = region_group.Value;
                foreach (TunicPortal portal in region_portals)
                {
                    Portal newPortal = new Portal(destination: portal.Destination, tag: portal.DestinationTag, name: portal.PortalName, scene: region_name, region: portal.GranularRegion, requiredItems: portal.RequiredItems, requiredItemsOr: portal.RequiredItemsOr, entryItems: portal.EntryItems, givesAccess: portal.GivesAccess, deadEnd: portal.DeadEnd, prayerPortal: portal.PrayerPortal, oneWay: portal.OneWay, ignoreScene: portal.IgnoreScene);
                    if (newPortal.DeadEnd == true)
                    { deadEndPortals.Add(newPortal); }
                    else twoPlusPortals.Add(newPortal);
                }
            }
            if (SaveFile.GetInt("randomizer ER fixed shop") == 1)
            {
                foreach (Portal portal in twoPlusPortals)
                {
                    if (portal.SceneDestinationTag == "Overworld Redux, Windmill_")
                    { 
                        twoPlusPortals.Remove(portal);
                        break;
                    }
                }
            }

            // making a list of accessible regions that will be updated as we gain access to more regions
            List<string> accessibleRegions = new List<string>();
            accessibleRegions.Clear();

            // just picking a static start region for now, can modify later if we want to do random start location
            string start_region = "Overworld";
            accessibleRegions.Add(start_region);
            
            int comboNumber = 0;

            // This might be way too much shuffling -- was done to not favor connecting new regions to the first regions added to the list
            // create a portal combo for every region in the threePlusRegions list, so that every region can now be accessed (ignoring rules for now)
            // todo: make it add regions to the list based on previously gotten regions
            while (accessibleRegions.Count < 56)
            {
                ShuffleList(twoPlusPortals, seed);
                // later on, start by making the first several portals into shop portals
                Portal portal1 = null;
                Portal portal2 = null;
                foreach (Portal portal in twoPlusPortals)
                {
                    // find a portal in a region we can't access yet
                    if (LockBeforeKey(portal) == false && !accessibleRegions.Contains(portal.Region))
                    {
                        portal1 = portal;
                    }
                }
                if (portal1 == null)
                { Logger.LogInfo("something messed up in portal pairing for portal 1"); }
                twoPlusPortals.Remove(portal1);
                ShuffleList(twoPlusPortals, seed);
                foreach (Portal secondPortal in twoPlusPortals)
                {
                    if (LockBeforeKey(secondPortal) == false && accessibleRegions.Contains(secondPortal.Region))
                    {
                        portal2 = secondPortal;
                        twoPlusPortals.Remove(secondPortal);
                        break;
                    }
                }
                if (portal2 == null)
                { Logger.LogInfo("something messed up in portal pairing for portal 2"); }
                // add the portal combo to the randomized portals list
                RandomizedPortals.Add(comboNumber.ToString(), new PortalCombo(portal1, portal2));
                foreach (string region in AddDependentRegions(portal1.Region)) {
                    if (!accessibleRegions.Contains(region)) {
                        accessibleRegions.Add(region);
                    }
                }
                comboNumber++;
            }

            // since the dead ends only have one exit, we just append them 1 to 1 to a random portal in the two plus list
            ShuffleList(deadEndPortals, seed);
            ShuffleList(twoPlusPortals, seed);
            while (deadEndPortals.Count > 0)
            {
                if (LockBeforeKey(twoPlusPortals[0]) == true) 
                { ShuffleList(twoPlusPortals, seed); }
                else 
                {
                    comboNumber++;
                    RandomizedPortals.Add(comboNumber.ToString(), new PortalCombo(deadEndPortals[0], twoPlusPortals[0]));
                    deadEndPortals.RemoveAt(0);
                    twoPlusPortals.RemoveAt(0);
                }
            }
            List<string> shopRegionList = new List<string>();
            int shopCount = 6;
            if (SaveFile.GetInt("randomizer ER fixed shop") == 1)
            {
                shopCount = 1;
                Portal windmillPortal = new Portal(destination: "Windmill", tag: "", name: "Windmill Entrance", scene: "Overworld Redux");
                Portal shopPortal = new Portal(destination: "Previous Region", tag: "", name: "Shop portal", scene: "Shop", region: "Shop");
                RandomizedPortals.Add("fixedshop", new PortalCombo(windmillPortal, shopPortal));
                shopRegionList.Add("Overworld Redux");
            }
            int regionNumber = 0;
            while (shopCount > 0)
            {
                // manually making a portal for the shop, because it has some special properties
                Portal shopPortal = new Portal(destination: "Previous Region", tag: "", name: "Shop portal", scene: "Shop", region: "Shop");
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
                    Logger.LogInfo("too many shops, not enough regions, add more shops");
                }
            }

            // now we have every region accessible
            // the twoPlusPortals list still has items left in it, so now we pair them off
            while (twoPlusPortals.Count > 1)
            {
                // I don't think the LockBeforeKey check can lead to an infinite loop?
                if (LockBeforeKey(twoPlusPortals[0]) == true || LockBeforeKey(twoPlusPortals[1]) == true)
                { ShuffleList(twoPlusPortals, seed); }
                else
                {
                comboNumber++;
                RandomizedPortals.Add(comboNumber.ToString(), new PortalCombo(twoPlusPortals[0], twoPlusPortals[1]));
                twoPlusPortals.RemoveAt(1); // I could do removeat0 twice, but I don't like how that looks
                twoPlusPortals.RemoveAt(0);
                }
            }
            if (twoPlusPortals.Count == 1)
            {
                // if this triggers, increase or decrease shop count by 1
                Logger.LogInfo("one extra dead end remaining alone, rip. It's " + twoPlusPortals[0].Name);
            }

            // todo: figure out why the quarry portal isn't working right
            //Portal betaQuarryPortal = new Portal(destination: "Darkwoods", tag: "", name: "Beta Quarry", scene: "Quarry", region: "Quarry", requiredItems: new Dictionary<string, int>(), givesAccess: new List<string>(), deadEnd: true, prayerPortal: false, oneWay: false, ignoreScene: false);
            //Portal zigSkipPortal = new Portal(destination: "ziggurat2020_3", tag: "zig2_skip", name: "Zig Skip", scene: "ziggurat2020_1", region: "Zig 1", requiredItems: new Dictionary<string, int>(), givesAccess: new List<string>(), deadEnd: true, prayerPortal: false, oneWay: false, ignoreScene: false);
            //RandomizedPortals.Add("zigsecret", new PortalCombo(betaQuarryPortal, zigSkipPortal));
        }

        // a function to apply the randomized portal list to portals during on scene loaded
        public static void ModifyPortals(Scene loadingScene)
        {
            var Portals = Resources.FindObjectsOfTypeAll<ScenePortal>();
            foreach (var portal in Portals)
            {
                // go through the list of randomized portals and see if either the first or second portal matches the one we're looking at
                foreach (KeyValuePair<string, PortalCombo> portalCombo in RandomizedPortals)
                {
                    string comboTag = portalCombo.Key;
                    Portal portal1 = portalCombo.Value.Portal1;
                    Portal portal2 = portalCombo.Value.Portal2;
                    Logger.LogInfo("portal combo is " + portal1.Name + " " + portal2.Name + " " + comboTag);
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
        public static void AltModifyPortals()
        {
            var Portals = Resources.FindObjectsOfTypeAll<ScenePortal>();
            foreach (var portal in Portals)
            {
                // go through the list of randomized portals and see if either the first or second portal matches the one we're looking at
                foreach (KeyValuePair<string, PortalCombo> portalCombo in RandomizedPortals)
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
