using System;
using System.Collections.Generic;
using System.Linq;

namespace TunicRandomizer {
    public class ERData {
        public static Dictionary<string, PortalCombo> RandomizedPortals = new Dictionary<string, PortalCombo>();
        public static Dictionary<string, PortalCombo> VanillaPortals = new Dictionary<string, PortalCombo>();
        public static Dictionary<string, Dictionary<string, List<List<string>>>> ModifiedTraversalReqs = new Dictionary<string, Dictionary<string, List<List<string>>>>();
        public static Dictionary<string, string> PlandoPortals = new Dictionary<string, string>();

        public const string FUSE_SHUFFLE = "Fuse Shuffle";
        public const string NO_FUSE_SHUFFLE = "No Fuse Shuffle";
        public const string BELL_SHUFFLE = "Bell Shuffle";
        public const string NO_BELL_SHUFFLE = "No Bell Shuffle";
        // the direction you move while entering the portal
        public enum PDir {
            NONE,
            NORTH,
            SOUTH,
            EAST,
            WEST,
            FLOOR,
            LADDER_UP,
            LADDER_DOWN
        }

        public class TunicPortal {
            public string Name;
            public string Destination;
            public string Tag;
            public int Direction;

            public TunicPortal(string name, string destination, string tag) {
                Name = name;
                Destination = destination;
                Tag = tag;
                Direction = (int)PDir.NONE;
            }

            public TunicPortal(string name, string destination, string tag, PDir direction) {
                Name = name;
                Destination = destination;
                Tag = tag;
                Direction = (int)direction;
            }
        }

        public class RegionInfo {
            public string Scene;
            public bool DeadEnd;
            public string OutletRegion;  // for where a portal forces you to another region immediately, example: Forest Hero's Grave immediately sends you to Forest Grave Path by Grave
            public bool SkipCounting;  // for ones that we skip every time because they're weird (zig skip, ls elevation regions) and shouldn't be counted in portal pairing the same way


            public RegionInfo(string scene, bool deadEnd) {
                Scene = scene;
                DeadEnd = deadEnd;
                OutletRegion = null;
                SkipCounting = false;
            }

            public RegionInfo(string scene, bool deadEnd, string outletRegion) {
                Scene = scene;
                DeadEnd = deadEnd;
                OutletRegion = outletRegion;
                SkipCounting = false;
            }

            public RegionInfo(string scene, bool deadEnd, bool skipCounting) {
                Scene = scene;
                DeadEnd = deadEnd;
                OutletRegion = null;
                SkipCounting = skipCounting;
            }

            public RegionInfo(string scene, bool deadEnd, string outletRegion, bool skipCounting) {
                Scene = scene;
                DeadEnd = deadEnd;
                OutletRegion = outletRegion;
                SkipCounting = skipCounting;
            }

        }

        public static Dictionary<string, PortalCombo> GetVanillaPortals() {
            return VanillaPortals.ToDictionary(k => k.Key, k => k.Value);
        }

        public static Dictionary<string, Dictionary<string, List<TunicPortal>>> RegionPortalsList = new Dictionary<string, Dictionary<string, List<TunicPortal>>> {
            {
                "Overworld Redux",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Overworld",
                        new List<TunicPortal> {
                            new TunicPortal("Stick House Entrance", "Sword Cave", "", PDir.NORTH),
                            new TunicPortal("Windmill Entrance", "Windmill", "", PDir.NORTH),
                            new TunicPortal("Old House Waterfall Entrance", "Overworld Interiors", "under_checkpoint", PDir.EAST),
                            new TunicPortal("Entrance to Furnace under Windmill", "Furnace", "gyro_upper_east", PDir.WEST),
                            new TunicPortal("Ruined Shop Entrance", "Ruined Shop", "", PDir.EAST),
                            new TunicPortal("Changing Room Entrance", "Changing Room", "", PDir.SOUTH),
                            new TunicPortal("Dark Tomb Main Entrance", "Crypt Redux", "", PDir.NORTH),
                            new TunicPortal("Secret Gathering Place Entrance", "Waterfall", "", PDir.NORTH),
                        }
                    },
                    {
                        "East Overworld",
                        new List<TunicPortal> {
                            new TunicPortal("Overworld to Forest Belltower", "Forest Belltower", "", PDir.EAST),
                            new TunicPortal("Overworld to Fortress", "Fortress Courtyard", "", PDir.EAST),
                        }
                    },
                    {
                        "Overworld at Patrol Cave",
                        new List<TunicPortal> {
                            new TunicPortal("Patrol Cave Entrance", "PatrolCave", "", PDir.NORTH),
                        }
                    },
                    {
                        "Upper Overworld",
                        new List<TunicPortal> {
                            new TunicPortal("Stairs from Overworld to Mountain", "Mountain", "", PDir.NORTH),
                        }
                    },
                    {
                        "Overworld after Temple Rafters",
                        new List<TunicPortal> {
                            new TunicPortal("Temple Rafters Entrance", "Temple", "rafters", PDir.EAST),
                        }
                    },
                    {
                        "Overworld Quarry Entry",
                        new List<TunicPortal> {
                            new TunicPortal("Overworld to Quarry Connector", "Darkwoods Tunnel", "", PDir.NORTH),
                        }
                    },
                    {
                        "Overworld Beach",
                        new List<TunicPortal> {
                            new TunicPortal("Atoll Lower Entrance", "Atoll Redux", "lower", PDir.SOUTH),
                            new TunicPortal("Hourglass Cave Entrance", "Town Basement", "beach", PDir.NORTH),
                            new TunicPortal("Maze Cave Entrance", "Maze Room", "", PDir.NORTH),
                        }
                    },
                    {
                        "Overworld Tunnel Turret",
                        new List<TunicPortal> {
                            new TunicPortal("Entrance to Furnace from Beach", "Furnace", "gyro_lower", PDir.NORTH),
                        }
                    },
                    {
                        "Overworld to Atoll Upper",
                        new List<TunicPortal> {
                            new TunicPortal("Atoll Upper Entrance", "Atoll Redux", "upper", PDir.SOUTH),
                        }
                    },
                    {
                        "Overworld Well Ladder",
                        new List<TunicPortal> {
                            new TunicPortal("Well Ladder Entrance", "Sewer", "entrance", PDir.LADDER_DOWN),
                        }
                    },
                    {
                        "Overworld Well to Furnace Rail",
                        new List<TunicPortal> {
                            new TunicPortal("Entrance to Well from Well Rail", "Sewer", "west_aqueduct", PDir.NORTH),
                            new TunicPortal("Entrance to Furnace from Well Rail", "Furnace", "gyro_upper_north", PDir.SOUTH),
                        }
                    },
                    {
                        "Overworld Old House Door",
                        new List<TunicPortal> {
                            new TunicPortal("Old House Door Entrance", "Overworld Interiors", "house", PDir.EAST),
                        }
                    },
                    {
                        "Overworld to West Garden Upper",
                        new List<TunicPortal> {
                            new TunicPortal("West Garden Entrance near Belltower", "Archipelagos Redux", "upper", PDir.WEST),
                        }
                    },
                    {
                        "Overworld to West Garden from Furnace",
                        new List<TunicPortal> {
                            new TunicPortal("Entrance to Furnace near West Garden", "Furnace", "gyro_west", PDir.EAST),
                            new TunicPortal("West Garden Entrance from Furnace", "Archipelagos Redux", "lower", PDir.WEST),
                        }
                    },
                    {
                        "Overworld Swamp Lower Entry",
                        new List<TunicPortal> {
                            new TunicPortal("Caustic Light Cave Entrance", "Overworld Cave", "", PDir.NORTH),
                            new TunicPortal("Swamp Lower Entrance", "Swamp Redux 2", "conduit", PDir.SOUTH),
                        }
                    },
                    {
                        "Overworld Swamp Upper Entry",
                        new List<TunicPortal> {
                            new TunicPortal("Swamp Upper Entrance", "Swamp Redux 2", "wall", PDir.SOUTH),
                        }
                    },
                    {
                        "Overworld Ruined Passage Door",
                        new List<TunicPortal> {
                            new TunicPortal("Ruined Passage Door Entrance", "Ruins Passage", "west", PDir.EAST),
                        }
                    },
                    {
                        "After Ruined Passage",
                        new List<TunicPortal> {
                            new TunicPortal("Ruined Passage Not-Door Entrance", "Ruins Passage", "east", PDir.NORTH),
                        }
                    },
                    {
                        "Overworld Special Shop Entry",
                        new List<TunicPortal> {
                            new TunicPortal("Special Shop Entrance", "ShopSpecial", "", PDir.EAST),
                        }
                    },
                    {
                        "Overworld West Garden Laurels Entry",
                        new List<TunicPortal> {
                            new TunicPortal("West Garden Laurels Entrance", "Archipelagos Redux", "lowest", PDir.WEST),
                        }
                    },
                    {
                        "Overworld Temple Door",
                        new List<TunicPortal> {
                            new TunicPortal("Temple Door Entrance", "Temple", "main", PDir.NORTH),
                        }
                    },
                    {
                        "Overworld Fountain Cross Door",
                        new List<TunicPortal> {
                            new TunicPortal("Fountain HC Door Entrance", "Town_FiligreeRoom", "", PDir.NORTH),
                        }
                    },
                    {
                        "Overworld Southeast Cross Door",
                        new List<TunicPortal> {
                            new TunicPortal("Southeast HC Door Entrance", "EastFiligreeCache", "", PDir.NORTH),
                        }
                    },
                    {
                        "Overworld Town Portal",
                        new List<TunicPortal> {
                            new TunicPortal("Town to Far Shore", "Transit", "teleporter_town", PDir.FLOOR),
                        }
                    },
                    {
                        "Overworld Spawn Portal",
                        new List<TunicPortal> {
                            new TunicPortal("Spawn to Far Shore", "Transit", "teleporter_starting island", PDir.FLOOR),
                        }
                    },
                    {
                        "Cube Cave Entrance Region",
                        new List<TunicPortal> {
                            new TunicPortal("Cube Cave Entrance", "CubeRoom", "", PDir.NORTH),
                        }
                    },
                }
            },
            {
                "Waterfall",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Secret Gathering Place",
                        new List<TunicPortal> {
                            new TunicPortal("Secret Gathering Place Exit", "Overworld Redux", "", PDir.SOUTH),
                        }
                    },
                }
            },
            {
                "Windmill",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Windmill",
                        new List<TunicPortal> {
                            new TunicPortal("Windmill Exit", "Overworld Redux", "", PDir.SOUTH),
                            new TunicPortal("Windmill Shop", "Shop", "", PDir.NORTH),
                        }
                    },
                }
            },
            {
                "Overworld Interiors",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Old House Front",
                        new List<TunicPortal> {
                            new TunicPortal("Old House Door Exit", "Overworld Redux", "house", PDir.WEST),
                            new TunicPortal("Old House to Glyph Tower", "g_elements", "", PDir.SOUTH),  // weird case, going off of the connecting
                        }
                    },
                    {
                        "Old House Back",
                        new List<TunicPortal> {
                            new TunicPortal("Old House Waterfall Exit", "Overworld Redux", "under_checkpoint", PDir.WEST),
                        }
                    },
                }
            },
            {
                "g_elements",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Relic Tower",
                        new List<TunicPortal> {
                            new TunicPortal("Glyph Tower Exit", "Overworld Interiors", "", PDir.NORTH),
                        }
                    },
                }
            },
            {
                "Changing Room",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Changing Room",
                        new List<TunicPortal> {
                            new TunicPortal("Changing Room Exit", "Overworld Redux", "", PDir.NORTH),
                        }
                    },
                }
            },
            {
                "Town_FiligreeRoom",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Fountain Cross Room",
                        new List<TunicPortal> {
                            new TunicPortal("Fountain HC Room Exit", "Overworld Redux", "", PDir.SOUTH),
                        }
                    },
                }
            },
            {
                "CubeRoom",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Cube Cave",
                        new List<TunicPortal> {
                            new TunicPortal("Cube Cave Exit", "Overworld Redux", "", PDir.SOUTH),
                        }
                    },
                }
            },
            {
                "PatrolCave",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Patrol Cave",
                        new List<TunicPortal> {
                            new TunicPortal("Guard Patrol Cave Exit", "Overworld Redux", "", PDir.SOUTH),
                        }
                    },
                }
            },
            {
                "Ruined Shop",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Ruined Shop",
                        new List<TunicPortal> {
                            new TunicPortal("Ruined Shop Exit", "Overworld Redux", "", PDir.WEST),
                        }
                    },
                }
            },
            {
                "Furnace",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Furnace Fuse",
                        new List<TunicPortal> {
                            new TunicPortal("Furnace Exit towards Well", "Overworld Redux", "gyro_upper_north", PDir.NORTH),
                        }
                    },
                    {
                        "Furnace Walking Path",
                        new List<TunicPortal> {
                            new TunicPortal("Furnace Exit to Dark Tomb", "Crypt Redux", "", PDir.EAST),
                            new TunicPortal("Furnace Exit towards West Garden", "Overworld Redux", "gyro_west", PDir.WEST),
                        }
                    },
                    {
                        "Furnace Ladder Area",
                        new List<TunicPortal> {
                            new TunicPortal("Furnace Exit to Beach", "Overworld Redux", "gyro_lower", PDir.SOUTH),
                            new TunicPortal("Furnace Exit under Windmill", "Overworld Redux", "gyro_upper_east", PDir.EAST),
                        }
                    },
                }
            },
            {
                "Sword Cave",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Stick House",
                        new List<TunicPortal> {
                            new TunicPortal("Stick House Exit", "Overworld Redux", "", PDir.SOUTH),
                        }
                    },
                }
            },
            {
                "Ruins Passage",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Ruined Passage",
                        new List<TunicPortal> {
                            new TunicPortal("Ruined Passage Not-Door Exit", "Overworld Redux", "east", PDir.SOUTH),
                            new TunicPortal("Ruined Passage Door Exit", "Overworld Redux", "west", PDir.WEST),
                        }
                    },
                }
            },
            {
                "EastFiligreeCache",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Southeast Cross Room",
                        new List<TunicPortal> {
                            new TunicPortal("Southeast HC Room Exit", "Overworld Redux", "", PDir.SOUTH),
                        }
                    },
                }
            },
            {
                "Overworld Cave",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Caustic Light Cave",
                        new List<TunicPortal> {
                            new TunicPortal("Caustic Light Cave Exit", "Overworld Redux", "", PDir.SOUTH),
                        }
                    },
                }
            },
            {
                "Maze Room",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Maze Cave",
                        new List<TunicPortal> {
                            new TunicPortal("Maze Cave Exit", "Overworld Redux", "", PDir.SOUTH),
                        }
                    },
                }
            },
            {
                "Town Basement",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Hourglass Cave",
                        new List<TunicPortal> {
                            new TunicPortal("Hourglass Cave Exit", "Overworld Redux", "beach", PDir.SOUTH),
                        }
                    },
                }
            },
            {
                "ShopSpecial",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Special Shop",
                        new List<TunicPortal> {
                            new TunicPortal("Special Shop Exit", "Overworld Redux", "", PDir.WEST),
                        }
                    },
                }
            },
            {
                "Temple",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Sealed Temple Rafters",
                        new List<TunicPortal> {
                            new TunicPortal("Temple Rafters Exit", "Overworld Redux", "rafters", PDir.WEST),
                        }
                    },
                    {
                        "Sealed Temple",
                        new List<TunicPortal> {
                            new TunicPortal("Temple Door Exit", "Overworld Redux", "main", PDir.SOUTH),
                        }
                    },
                }
            },

            {
                "Forest Belltower",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Forest Belltower Upper",
                        new List<TunicPortal> {
                            new TunicPortal("Forest Belltower to Guard Captain Room", "Forest Boss Room", "", PDir.SOUTH),
                        }
                    },
                    {
                        "Forest Belltower Main",
                        new List<TunicPortal> {
                            new TunicPortal("Forest Belltower to Overworld", "Overworld Redux", "", PDir.WEST),
                        }
                    },
                    {
                        "Forest Belltower Main behind bushes",
                        new List<TunicPortal> {
                            new TunicPortal("Forest Belltower to Fortress", "Fortress Courtyard", "", PDir.NORTH),
                        }
                    },
                    {
                        "Forest Belltower Lower",
                        new List<TunicPortal> {
                            new TunicPortal("Forest Belltower to Forest", "East Forest Redux", "", PDir.SOUTH),
                        }
                    },
                }
            },
            {
                "East Forest Redux",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "East Forest",
                        new List<TunicPortal> {
                            new TunicPortal("Forest to Belltower", "Forest Belltower", "", PDir.NORTH),
                            new TunicPortal("Forest Guard House 1 Lower Entrance", "East Forest Redux Laddercave", "lower", PDir.NORTH),
                            new TunicPortal("Forest Guard House 1 Gate Entrance", "East Forest Redux Laddercave", "gate", PDir.NORTH),
                            new TunicPortal("Forest Guard House 2 Upper Entrance", "East Forest Redux Interior", "upper", PDir.EAST),
                            new TunicPortal("Forest Grave Path Lower Entrance", "Sword Access", "lower", PDir.EAST),
                            new TunicPortal("Forest Grave Path Upper Entrance", "Sword Access", "upper", PDir.EAST),
                        }
                    },
                    {
                        "East Forest Dance Fox Spot",
                        new List<TunicPortal> {
                            new TunicPortal("Forest Dance Fox Outside Doorway", "East Forest Redux Laddercave", "upper", PDir.EAST),
                        }
                    },
                    {
                        "East Forest Portal",
                        new List<TunicPortal> {
                            new TunicPortal("Forest to Far Shore", "Transit", "teleporter_forest teleporter", PDir.FLOOR),
                        }
                    },
                    {
                        "Lower Forest",
                        new List<TunicPortal> {
                            new TunicPortal("Forest Guard House 2 Lower Entrance", "East Forest Redux Interior", "lower", PDir.NORTH),
                        }
                    },
                }
            },
            {
                "Sword Access",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Forest Grave Path Upper",
                        new List<TunicPortal> {
                            new TunicPortal("Forest Grave Path Upper Exit", "East Forest Redux", "upper", PDir.WEST),
                        }
                    },
                    {
                        "Forest Grave Path Main",
                        new List<TunicPortal> {
                            new TunicPortal("Forest Grave Path Lower Exit", "East Forest Redux", "lower", PDir.WEST),
                        }
                    },
                    {
                        "Forest Hero's Grave",
                        new List<TunicPortal> {
                            new TunicPortal("East Forest Hero's Grave", "RelicVoid", "teleporter_relic plinth", PDir.FLOOR),
                        }
                    },
                }
            },
            {
                "East Forest Redux Laddercave",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Guard House 1 West",
                        new List<TunicPortal> {
                            new TunicPortal("Guard House 1 Dance Fox Exit", "East Forest Redux", "upper", PDir.WEST),
                            new TunicPortal("Guard House 1 Lower Exit", "East Forest Redux", "lower", PDir.SOUTH),
                        }
                    },
                    {
                        "Guard House 1 East",
                        new List<TunicPortal> {
                            new TunicPortal("Guard House 1 Upper Forest Exit", "East Forest Redux", "gate", PDir.SOUTH),
                            new TunicPortal("Guard House 1 to Guard Captain Room", "Forest Boss Room", "", PDir.NORTH),
                        }
                    },
                }
            },
            {
                "East Forest Redux Interior",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Guard House 2 Upper",
                        new List<TunicPortal> {
                            new TunicPortal("Guard House 2 Upper Exit", "East Forest Redux", "upper", PDir.WEST),
                        }
                    },
                    {
                        "Guard House 2 Lower",
                        new List<TunicPortal> {
                            new TunicPortal("Guard House 2 Lower Exit", "East Forest Redux", "lower", PDir.SOUTH),
                        }
                    },
                }
            },
            {
                "Forest Boss Room",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Forest Boss Room",
                        new List<TunicPortal> {
                            new TunicPortal("Guard Captain Room Non-Gate Exit", "East Forest Redux Laddercave", "", PDir.SOUTH),
                            new TunicPortal("Guard Captain Room Gate Exit", "Forest Belltower", "", PDir.NORTH),
                        }
                    },
                }
            },

            {
                "Sewer",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Beneath the Well Ladder Exit",
                        new List<TunicPortal> {
                            new TunicPortal("Well Ladder Exit", "Overworld Redux", "entrance", PDir.LADDER_UP),
                        }
                    },
                    {
                        "Beneath the Well Back",
                        new List<TunicPortal> {
                            new TunicPortal("Well to Well Boss", "Sewer_Boss", "", PDir.EAST),
                            new TunicPortal("Well Exit towards Furnace", "Overworld Redux", "west_aqueduct", PDir.SOUTH),
                        }
                    },
                }
            },
            {
                "Sewer_Boss",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Well Boss",
                        new List<TunicPortal> {
                            new TunicPortal("Well Boss to Well", "Sewer", "", PDir.WEST),
                        }
                    },
                    {
                        "Dark Tomb Checkpoint",
                        new List<TunicPortal> {
                            new TunicPortal("Checkpoint to Dark Tomb", "Crypt Redux", "", PDir.LADDER_UP),
                        }
                    },
                }
            },

            {
                "Crypt Redux",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Dark Tomb Entry Point",
                        new List<TunicPortal> {
                            new TunicPortal("Dark Tomb to Overworld", "Overworld Redux", "", PDir.SOUTH),
                            new TunicPortal("Dark Tomb to Checkpoint", "Sewer_Boss", "", PDir.LADDER_DOWN),
                        }
                    },
                    {
                        "Dark Tomb Dark Exit",
                        new List<TunicPortal> {
                            new TunicPortal("Dark Tomb to Furnace", "Furnace", "", PDir.WEST),
                        }
                    },
                }
            },

            {
                "Archipelagos Redux",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "West Garden",
                        new List<TunicPortal> {
                            new TunicPortal("West Garden Exit near Hero's Grave", "Overworld Redux", "lower", PDir.EAST),
                            new TunicPortal("West Garden to Magic Dagger House", "archipelagos_house", "", PDir.EAST),
                            new TunicPortal("West Garden Shop", "Shop", "", PDir.EAST),
                        }
                    },
                    {
                        "West Garden after Boss",
                        new List<TunicPortal> {
                            new TunicPortal("West Garden Exit after Boss", "Overworld Redux", "upper", PDir.EAST),
                        }
                    },
                    {
                        "West Garden Laurels Exit",
                        new List<TunicPortal> {
                            new TunicPortal("West Garden Laurels Exit", "Overworld Redux", "lowest", PDir.EAST),
                        }
                    },
                    {
                        "West Garden Hero's Grave",
                        new List<TunicPortal> {
                            new TunicPortal("West Garden Hero's Grave", "RelicVoid", "teleporter_relic plinth", PDir.FLOOR),
                        }
                    },
                    {
                        "West Garden Portal",
                        new List<TunicPortal> {
                            new TunicPortal("West Garden to Far Shore", "Transit", "teleporter_archipelagos_teleporter", PDir.FLOOR),
                        }
                    },
                }
            },
            {
                "archipelagos_house",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Magic Dagger House",
                        new List<TunicPortal> {
                            new TunicPortal("Magic Dagger House Exit", "Archipelagos Redux", "", PDir.WEST),
                        }
                    },
                }
            },

            {
                "Fortress Courtyard",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Fortress Courtyard",
                        new List<TunicPortal> {
                            new TunicPortal("Fortress Courtyard to Fortress Grave Path Lower", "Fortress Reliquary", "Lower", PDir.EAST),
                            new TunicPortal("Fortress Courtyard to Fortress Interior", "Fortress Main", "Big Door", PDir.NORTH),
                        }
                    },
                    {
                        "Fortress Courtyard Upper",
                        new List<TunicPortal> {
                            new TunicPortal("Fortress Courtyard to Fortress Grave Path Upper", "Fortress Reliquary", "Upper", PDir.EAST),
                            new TunicPortal("Fortress Courtyard to East Fortress", "Fortress East", "", PDir.NORTH),
                        }
                    },
                    {
                        "Fortress Exterior near cave",
                        new List<TunicPortal> {
                            new TunicPortal("Fortress Courtyard Shop", "Shop", "", PDir.NORTH),
                        }
                    },
                    {
                        "Beneath the Vault Entry",
                        new List<TunicPortal> {
                            new TunicPortal("Fortress Courtyard to Beneath the Earth", "Fortress Basement", "", PDir.LADDER_DOWN),
                        }
                    },
                    {
                        "Fortress Exterior from East Forest",
                        new List<TunicPortal> {
                            new TunicPortal("Fortress Courtyard to Forest Belltower", "Forest Belltower", "", PDir.SOUTH),
                        }
                    },
                    {
                        "Fortress Exterior from Overworld",
                        new List<TunicPortal> {
                            new TunicPortal("Fortress Courtyard to Overworld", "Overworld Redux", "", PDir.WEST),
                        }
                    },
                }
            },
            {
                "Fortress Basement",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Beneath the Vault Back",
                        new List<TunicPortal> {
                            new TunicPortal("Beneath the Earth to Fortress Interior", "Fortress Main", "", PDir.EAST),
                        }
                    },
                    {
                        "Beneath the Vault Ladder Exit",
                        new List<TunicPortal> {
                            new TunicPortal("Beneath the Earth to Fortress Courtyard", "Fortress Courtyard", "", PDir.LADDER_UP),
                        }
                    },
                }
            },
            {
                "Fortress Main",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Eastern Vault Fortress",
                        new List<TunicPortal> {
                            new TunicPortal("Fortress Interior Main Exit", "Fortress Courtyard", "Big Door", PDir.SOUTH),
                            new TunicPortal("Fortress Interior to Beneath the Earth", "Fortress Basement", "", PDir.WEST),
                            new TunicPortal("Fortress Interior Shop", "Shop", "", PDir.NORTH),
                            new TunicPortal("Fortress Interior to East Fortress Upper", "Fortress East", "upper", PDir.EAST),
                            new TunicPortal("Fortress Interior to East Fortress Lower", "Fortress East", "lower", PDir.EAST),
                        }
                    },
                    {
                        "Eastern Vault Fortress Gold Door",
                        new List<TunicPortal> {
                            new TunicPortal("Fortress Interior to Siege Engine Arena", "Fortress Arena", "", PDir.NORTH),
                        }
                    },
                }
            },
            {
                "Fortress East",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Fortress East Shortcut Lower",
                        new List<TunicPortal> {
                            new TunicPortal("East Fortress to Interior Lower", "Fortress Main", "lower", PDir.WEST),
                        }
                    },
                    {
                        "Fortress East Shortcut Upper",
                        new List<TunicPortal> {
                            new TunicPortal("East Fortress to Courtyard", "Fortress Courtyard", "", PDir.SOUTH),
                            new TunicPortal("East Fortress to Interior Upper", "Fortress Main", "upper", PDir.WEST),
                        }
                    },
                }
            },
            {
                "Fortress Reliquary",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Fortress Grave Path",
                        new List<TunicPortal> {
                            new TunicPortal("Fortress Grave Path Lower Exit", "Fortress Courtyard", "Lower", PDir.WEST),
                        }
                    },
                    {
                        "Fortress Grave Path Upper",
                        new List<TunicPortal> {
                            new TunicPortal("Fortress Grave Path Upper Exit", "Fortress Courtyard", "Upper", PDir.WEST),
                        }
                    },
                    {
                        "Fortress Grave Path Dusty Entrance",
                        new List<TunicPortal> {
                            new TunicPortal("Fortress Grave Path Dusty Entrance", "Dusty", "", PDir.NORTH),
                        }
                    },
                    {
                        "Fortress Hero's Grave",
                        new List<TunicPortal> {
                            new TunicPortal("Fortress Hero's Grave", "RelicVoid", "teleporter_relic plinth", PDir.FLOOR),
                        }
                    },
                }
            },
            {
                "Dusty",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Fortress Leaf Piles",
                        new List<TunicPortal> {
                            new TunicPortal("Dusty Exit", "Fortress Reliquary", "", PDir.SOUTH),
                        }
                    },
                }
            },
            {
                "Fortress Arena",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Fortress Arena",
                        new List<TunicPortal> {
                            new TunicPortal("Siege Engine Arena to Fortress", "Fortress Main", "", PDir.SOUTH),
                        }
                    },
                    {
                        "Fortress Arena Portal",
                        new List<TunicPortal> {
                            new TunicPortal("Fortress to Far Shore", "Transit", "teleporter_spidertank", PDir.FLOOR),
                        }
                    },
                }
            },

            {
                "Atoll Redux",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Ruined Atoll",
                        new List<TunicPortal> {
                            new TunicPortal("Atoll Upper Exit", "Overworld Redux", "upper", PDir.NORTH),
                            new TunicPortal("Atoll Shop", "Shop", "", PDir.NORTH),
                        }
                    },
                    {
                        "Ruined Atoll Lower Entry Area",
                        new List<TunicPortal> {
                            new TunicPortal("Atoll Lower Exit", "Overworld Redux", "lower", PDir.NORTH),
                        }
                    },
                    {
                        "Ruined Atoll Portal",
                        new List<TunicPortal> {
                            new TunicPortal("Atoll to Far Shore", "Transit", "teleporter_atoll", PDir.FLOOR),
                        }
                    },
                    {
                        "Ruined Atoll Statue",
                        new List<TunicPortal> {
                            new TunicPortal("Atoll Statue Teleporter", "Library Exterior", "", PDir.FLOOR),
                        }
                    },
                    {
                        "Ruined Atoll Frog Eye",
                        new List<TunicPortal> {
                            new TunicPortal("Frog Stairs Eye Entrance", "Frog Stairs", "eye", PDir.SOUTH),  // camera rotates to be north
                        }
                    },
                    {
                        "Ruined Atoll Frog Mouth",
                        new List<TunicPortal> {
                            new TunicPortal("Frog Stairs Mouth Entrance", "Frog Stairs", "mouth", PDir.EAST),
                        }
                    },
                }
            },
            {
                "Frog Stairs",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Frog Stairs Eye Exit",
                        new List<TunicPortal> {
                            new TunicPortal("Frog Stairs Eye Exit", "Atoll Redux", "eye", PDir.NORTH),
                        }
                    },
                    {
                        "Frog Stairs Upper",
                        new List<TunicPortal> {
                            new TunicPortal("Frog Stairs Mouth Exit", "Atoll Redux", "mouth", PDir.WEST),
                        }
                    },
                    {
                        "Frog Stairs Lower",
                        new List<TunicPortal> {
                            new TunicPortal("Frog Stairs to Frog's Domain's Exit", "frog cave main", "Exit", PDir.EAST),
                        }
                    },
                    {
                        "Frog Stairs to Frog's Domain",
                        new List<TunicPortal> {
                            new TunicPortal("Frog Stairs to Frog's Domain's Entrance", "frog cave main", "Entrance", PDir.LADDER_DOWN),
                        }
                    },
                }
            },
            {
                "frog cave main",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Frog's Domain Entry",
                        new List<TunicPortal> {
                            new TunicPortal("Frog's Domain Ladder Exit", "Frog Stairs", "Entrance", PDir.LADDER_UP),
                        }
                    },
                    {
                        "Frog's Domain Back",
                        new List<TunicPortal> {
                            new TunicPortal("Frog's Domain Orb Exit", "Frog Stairs", "Exit", PDir.WEST),
                        }
                    },
                }
            },
            {
                "Library Exterior",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Library Exterior Tree",
                        new List<TunicPortal> {
                            new TunicPortal("Library Exterior Tree", "Atoll Redux", "", PDir.FLOOR),
                        }
                    },
                    {
                        "Library Exterior Ladder",
                        new List<TunicPortal> {
                            new TunicPortal("Library Exterior Ladder", "Library Hall", "", PDir.WEST),  // camera rotates to appear north
                        }
                    },
                }
            },
            {
                "Library Hall",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Library Hall Bookshelf",
                        new List<TunicPortal> {
                            new TunicPortal("Library Hall Bookshelf Exit", "Library Exterior", "", PDir.EAST),
                        }
                    },
                    {
                        "Library Hero's Grave",
                        new List<TunicPortal> {
                            new TunicPortal("Library Hero's Grave", "RelicVoid", "teleporter_relic plinth", PDir.FLOOR),
                        }
                    },
                    {
                        "Library Hall to Rotunda",
                        new List<TunicPortal> {
                            new TunicPortal("Library Hall to Rotunda", "Library Rotunda", "", PDir.LADDER_UP),
                        }
                    },
                }
            },
            {
                "Library Rotunda",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Library Rotunda to Hall",
                        new List<TunicPortal> {
                            new TunicPortal("Library Rotunda Lower Exit", "Library Hall", "", PDir.LADDER_DOWN),
                        }
                    },
                    {
                        "Library Rotunda to Lab",
                        new List<TunicPortal> {
                            new TunicPortal("Library Rotunda Upper Exit", "Library Lab", "", PDir.LADDER_UP),
                        }
                    },
                }
            },
            {
                "Library Lab",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Library Lab Lower",
                        new List<TunicPortal> {
                            new TunicPortal("Library Lab to Rotunda", "Library Rotunda", "", PDir.LADDER_DOWN),
                        }
                    },
                    {
                        "Library Portal",
                        new List<TunicPortal> {
                            new TunicPortal("Library to Far Shore", "Transit", "teleporter_library teleporter", PDir.FLOOR),
                        }
                    },
                    {
                        "Library Lab to Librarian",
                        new List<TunicPortal> {
                            new TunicPortal("Library Lab to Librarian Arena", "Library Arena", "", PDir.LADDER_UP),
                        }
                    },
                }
            },
            {
                "Library Arena",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Library Arena",
                        new List<TunicPortal> {
                            new TunicPortal("Librarian Arena Exit", "Library Lab", "", PDir.LADDER_DOWN),
                        }
                    },
                }
            },

            {
                "Mountain",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Lower Mountain Stairs",
                        new List<TunicPortal> {
                            new TunicPortal("Stairs to Top of the Mountain", "Mountaintop", "", PDir.NORTH),
                        }
                    },
                    {
                        "Lower Mountain",
                        new List<TunicPortal> {
                            new TunicPortal("Mountain to Quarry", "Quarry Redux", "", PDir.SOUTH),  // starts north, rotates to be east, but the connecting one is north so it's south
                            new TunicPortal("Mountain to Overworld", "Overworld Redux", "", PDir.SOUTH),
                        }
                    },
                }
            },
            {
                "Mountaintop",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Top of the Mountain",
                        new List<TunicPortal> {
                            new TunicPortal("Top of the Mountain Exit", "Mountain", "", PDir.SOUTH),
                        }
                    },
                }
            },
            {
                "Darkwoods Tunnel",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Quarry Connector",
                        new List<TunicPortal> {
                            new TunicPortal("Quarry Connector to Overworld", "Overworld Redux", "", PDir.SOUTH),
                            new TunicPortal("Quarry Connector to Quarry", "Quarry Redux", "", PDir.NORTH),  // rotates, but the connecting is south
                        }
                    },
                }
            },
            {
                "Quarry Redux",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Quarry Entry",
                        new List<TunicPortal> {
                            new TunicPortal("Quarry to Overworld Exit", "Darkwoods Tunnel", "", PDir.SOUTH),
                            new TunicPortal("Quarry Shop", "Shop", "", PDir.NORTH),
                        }
                    },
                    {
                        "Quarry Monastery Entry",
                        new List<TunicPortal> {
                            new TunicPortal("Quarry to Monastery Front", "Monastery", "front", PDir.NORTH),
                        }
                    },
                    {
                        "Monastery Rope",
                        new List<TunicPortal> {
                            new TunicPortal("Quarry to Monastery Back", "Monastery", "back", PDir.EAST),
                        }
                    },
                    {
                        "Quarry Back",
                        new List<TunicPortal> {
                            new TunicPortal("Quarry to Mountain", "Mountain", "", PDir.NORTH),
                        }
                    },
                    {
                        "Lower Quarry Zig Door",
                        new List<TunicPortal> {
                            new TunicPortal("Quarry to Ziggurat", "ziggurat2020_0", "", PDir.NORTH),
                        }
                    },
                    {
                        "Quarry Portal",
                        new List<TunicPortal> {
                            new TunicPortal("Quarry to Far Shore", "Transit", "teleporter_quarry teleporter", PDir.FLOOR),
                        }
                    },
                }
            },
            {
                "Monastery",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Monastery Back",
                        new List<TunicPortal> {
                            new TunicPortal("Monastery Rear Exit", "Quarry Redux", "back", PDir.WEST),
                        }
                    },
                    {
                        "Monastery Front",
                        new List<TunicPortal> {
                            new TunicPortal("Monastery Front Exit", "Quarry Redux", "front", PDir.SOUTH),
                        }
                    },
                    {
                        "Monastery Hero's Grave",
                        new List<TunicPortal> {
                            new TunicPortal("Monastery Hero's Grave", "RelicVoid", "teleporter_relic plinth", PDir.FLOOR),
                        }
                    },
                }
            },

            {
                "ziggurat2020_0",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Rooted Ziggurat Entry",
                        new List<TunicPortal> {
                            new TunicPortal("Ziggurat Entry Hallway to Ziggurat Upper", "ziggurat2020_1", "", PDir.NORTH),
                            new TunicPortal("Ziggurat Entry Hallway to Quarry", "Quarry Redux", "", PDir.SOUTH),
                        }
                    },
                }
            },
            {
                "ziggurat2020_1",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Rooted Ziggurat Upper Entry",
                        new List<TunicPortal> {
                            new TunicPortal("Ziggurat Upper to Ziggurat Entry Hallway", "ziggurat2020_0", "", PDir.SOUTH),
                        }
                    },
                    {
                        "Rooted Ziggurat Upper Back",
                        new List<TunicPortal> {
                            new TunicPortal("Ziggurat Upper to Ziggurat Tower", "ziggurat2020_2", "", PDir.NORTH),  // lots of rotation, connecting is south
                        }
                    },
                }
            },
            {
                "ziggurat2020_2",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Rooted Ziggurat Middle Top",
                        new List<TunicPortal> {
                            new TunicPortal("Ziggurat Tower to Ziggurat Upper", "ziggurat2020_1", "", PDir.SOUTH),
                        }
                    },
                    {
                        "Rooted Ziggurat Middle Bottom",
                        new List<TunicPortal> {
                            new TunicPortal("Ziggurat Tower to Ziggurat Lower", "ziggurat2020_3", "", PDir.SOUTH),
                        }
                    },
                }
            },
            {
                "ziggurat2020_3",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Rooted Ziggurat Lower Front",
                        new List<TunicPortal> {
                            new TunicPortal("Ziggurat Lower to Ziggurat Tower", "ziggurat2020_2", "", PDir.NORTH),
                        }
                    },
                    {
                        "Rooted Ziggurat Portal Room Entrance",
                        new List<TunicPortal> {
                            new TunicPortal("Ziggurat Portal Room Entrance", "ziggurat2020_FTRoom", "", PDir.NORTH),
                        }
                    },
                    {
                        "Zig Skip Exit",
                        new List<TunicPortal> {
                            new TunicPortal("Ziggurat Lower Falling Entrance", "ziggurat2020_1", "zig2_skip", PDir.FLOOR),  // floor is weird but oh well
                        }
                    },
                }
            },
            {
                "ziggurat2020_FTRoom",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Rooted Ziggurat Portal Room Exit",
                        new List<TunicPortal> {
                            new TunicPortal("Ziggurat Portal Room Exit", "ziggurat2020_3", "", PDir.SOUTH),
                        }
                    },
                    {
                        "Rooted Ziggurat Portal",
                        new List<TunicPortal> {
                            new TunicPortal("Ziggurat to Far Shore", "Transit", "teleporter_ziggurat teleporter", PDir.FLOOR),
                        }
                    },
                }
            },

            {
                "Swamp Redux 2",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Swamp Front",
                        new List<TunicPortal> {
                            new TunicPortal("Swamp Lower Exit", "Overworld Redux", "conduit", PDir.NORTH),
                            new TunicPortal("Swamp Shop", "Shop", "", PDir.NORTH),
                        }
                    },
                    {
                        "Swamp to Cathedral Main Entrance",
                        new List<TunicPortal> {
                            new TunicPortal("Swamp to Cathedral Main Entrance", "Cathedral Redux", "main", PDir.NORTH),
                        }
                    },
                    {
                        "Swamp to Cathedral Treasure Room",
                        new List<TunicPortal> {
                            new TunicPortal("Swamp to Cathedral Secret Legend Room Entrance", "Cathedral Redux", "secret", PDir.SOUTH),
                        }
                    },
                    {
                        "Back of Swamp",
                        new List<TunicPortal> {
                            new TunicPortal("Swamp to Gauntlet", "Cathedral Arena", "", PDir.NORTH),
                        }
                    },
                    {
                        "Back of Swamp Laurels Area",
                        new List<TunicPortal> {
                            new TunicPortal("Swamp Upper Exit", "Overworld Redux", "wall", PDir.NORTH),
                        }
                    },
                    {
                        "Swamp Hero's Grave",
                        new List<TunicPortal> {
                            new TunicPortal("Swamp Hero's Grave", "RelicVoid", "teleporter_relic plinth", PDir.FLOOR),
                        }
                    },
                }
            },
            {
                "Cathedral Redux",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Cathedral",
                        new List<TunicPortal> {
                            new TunicPortal("Cathedral Main Exit", "Swamp Redux 2", "main", PDir.SOUTH),
                        }
                    },
                    {
                        "Cathedral Elevator",
                        new List<TunicPortal> {
                            new TunicPortal("Cathedral Elevator", "Cathedral Arena", "", PDir.LADDER_DOWN),
                        }
                    },
                    {
                        "Cathedral Secret Legend Room",
                        new List<TunicPortal> {
                            new TunicPortal("Cathedral Secret Legend Room Exit", "Swamp Redux 2", "secret", PDir.NORTH),
                        }
                    },
                }
            },
            {
                "Cathedral Arena",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Cathedral Gauntlet Exit",
                        new List<TunicPortal> {
                            new TunicPortal("Gauntlet to Swamp", "Swamp Redux 2", "", PDir.SOUTH),
                        }
                    },
                    {
                        "Cathedral Gauntlet Checkpoint",
                        new List<TunicPortal> {
                            new TunicPortal("Gauntlet Elevator", "Cathedral Redux", "", PDir.LADDER_UP),
                            new TunicPortal("Gauntlet Shop", "Shop", "", PDir.EAST),
                        }
                    },
                }
            },

            {
                "Transit",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Far Shore to West Garden",
                        new List<TunicPortal> {
                            new TunicPortal("Far Shore to West Garden", "Archipelagos Redux", "teleporter_archipelagos_teleporter", PDir.FLOOR),
                        }
                    },
                    {
                        "Far Shore to Library",
                        new List<TunicPortal> {
                            new TunicPortal("Far Shore to Library", "Library Lab", "teleporter_library teleporter", PDir.FLOOR),
                        }
                    },
                    {
                        "Far Shore to Quarry",
                        new List<TunicPortal> {
                            new TunicPortal("Far Shore to Quarry", "Quarry Redux", "teleporter_quarry teleporter", PDir.FLOOR),
                        }
                    },
                    {
                        "Far Shore to East Forest",
                        new List<TunicPortal> {
                            new TunicPortal("Far Shore to East Forest", "East Forest Redux", "teleporter_forest teleporter", PDir.FLOOR),
                        }
                    },
                    {
                        "Far Shore to Fortress",
                        new List<TunicPortal> {
                            new TunicPortal("Far Shore to Fortress", "Fortress Arena", "teleporter_spidertank", PDir.FLOOR),
                        }
                    },
                    {
                        "Far Shore",
                        new List<TunicPortal> {
                            new TunicPortal("Far Shore to Atoll", "Atoll Redux", "teleporter_atoll", PDir.FLOOR),
                            new TunicPortal("Far Shore to Ziggurat", "ziggurat2020_FTRoom", "teleporter_ziggurat teleporter", PDir.FLOOR),
                            new TunicPortal("Far Shore to Heir", "Spirit Arena", "teleporter_spirit arena", PDir.FLOOR),
                            new TunicPortal("Far Shore to Town", "Overworld Redux", "teleporter_town", PDir.FLOOR),
                        }
                    },
                    {
                        "Far Shore to Spawn",
                        new List<TunicPortal> {
                            new TunicPortal("Far Shore to Spawn", "Overworld Redux", "teleporter_starting island", PDir.FLOOR),
                        }
                    },
                }
            },
            {
                "RelicVoid",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Hero Relic - Fortress",
                        new List<TunicPortal> {
                            new TunicPortal("Hero's Grave to Fortress", "Fortress Reliquary", "teleporter_relic plinth", PDir.FLOOR),
                        }
                    },
                    {
                        "Hero Relic - Quarry",
                        new List<TunicPortal> {
                            new TunicPortal("Hero's Grave to Monastery", "Monastery", "teleporter_relic plinth", PDir.FLOOR),
                        }
                    },
                    {
                        "Hero Relic - West Garden",
                        new List<TunicPortal> {
                            new TunicPortal("Hero's Grave to West Garden", "Archipelagos Redux", "teleporter_relic plinth", PDir.FLOOR),
                        }
                    },
                    {
                        "Hero Relic - East Forest",
                        new List<TunicPortal> {
                            new TunicPortal("Hero's Grave to East Forest", "Sword Access", "teleporter_relic plinth", PDir.FLOOR),
                        }
                    },
                    {
                        "Hero Relic - Library",
                        new List<TunicPortal> {
                            new TunicPortal("Hero's Grave to Library", "Library Hall", "teleporter_relic plinth", PDir.FLOOR),
                        }
                    },
                    {
                        "Hero Relic - Swamp",
                        new List<TunicPortal> {
                            new TunicPortal("Hero's Grave to Swamp", "Swamp Redux 2", "teleporter_relic plinth", PDir.FLOOR),
                        }
                    },
                }
            },
            {
                "Spirit Arena",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Spirit Arena",
                        new List<TunicPortal> {
                            new TunicPortal("Heir Arena Exit", "Transit", "teleporter_spirit arena", PDir.FLOOR),
                        }
                    },
                }
            },
            {
                "Purgatory",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Purgatory",  // requires special handling for vanilla portals
                        new List<TunicPortal> {
                            new TunicPortal("Purgatory Bottom Exit", "Purgatory", "bottom", PDir.SOUTH),  // inaccurate but eh
                            new TunicPortal("Purgatory Top Exit", "Purgatory", "top", PDir.NORTH),
                        }
                    },
                }
            },

            {
                "Shop",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Shop",
                        new List<TunicPortal> {
                            new TunicPortal("Shop Portal", "Previous Region", ""),  // "Previous Region" is just a placeholder
                            // 6 shops connect to a north portal, 2 shops connect to an east portal (West Garden and Gauntlet)
                        }
                    },
                }
            },
        };

        public static Dictionary<string, RegionInfo> RegionDict = new Dictionary<string, RegionInfo> {
            {
                "Overworld",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Belltower",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Belltower at Bell",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Swamp Upper Entry",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Swamp Lower Entry",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "After Ruined Passage",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Above Ruined Passage",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "East Overworld",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Special Shop Entry",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Upper Overworld",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld above Quarry Entrance",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld after Temple Rafters",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Quarry Entry",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld after Envoy",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld at Patrol Cave",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld above Patrol Cave",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld West Garden Laurels Entry",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld to West Garden Upper",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld to West Garden from Furnace",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Well Ladder",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Beach",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Tunnel Turret",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld to Atoll Upper",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Well to Furnace Rail",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Ruined Passage Door",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Old House Door",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Southeast Cross Door",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Fountain Cross Door",
                new RegionInfo("Overworld Redux", false, outletRegion:"Overworld")
            },
            {
                "Overworld Temple Door",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Town Portal",
                new RegionInfo("Overworld Redux", false, outletRegion:"Overworld")
            },
            {
                "Overworld Spawn Portal",
                new RegionInfo("Overworld Redux", false, outletRegion:"Overworld")
            },
            {
                "Cube Cave Entrance Region",
                new RegionInfo("Overworld Redux", false, outletRegion:"Overworld")
            },
            {
                "Stick House",
                new RegionInfo("Sword Cave", true)
            },
            {
                "Windmill",
                new RegionInfo("Windmill", false)
            },
            {
                "Old House Back",
                new RegionInfo("Overworld Interiors", false)
            },
            {
                "Old House Front",
                new RegionInfo("Overworld Interiors", false)
            },
            {
                "Relic Tower",
                new RegionInfo("g_elements", true)
            },
            {
                "Furnace Fuse",
                new RegionInfo("Furnace", false)
            },
            {
                "Furnace Ladder Area",
                new RegionInfo("Furnace", false)
            },
            {
                "Furnace Walking Path",
                new RegionInfo("Furnace", false)
            },
            {
                "Secret Gathering Place",
                new RegionInfo("Waterfall", true)
            },
            {
                "Changing Room",
                new RegionInfo("Changing Room", true)
            },
            {
                "Patrol Cave",
                new RegionInfo("PatrolCave", true)
            },
            {
                "Ruined Shop",
                new RegionInfo("Ruined Shop", true)
            },
            {
                "Ruined Passage",
                new RegionInfo("Ruins Passage", false)
            },
            {
                "Special Shop",
                new RegionInfo("ShopSpecial", true)
            },
            {
                "Caustic Light Cave",
                new RegionInfo("Overworld Cave", true)
            },
            {
                "Maze Cave",
                new RegionInfo("Maze Room", true)
            },
            {
                "Cube Cave",
                new RegionInfo("CubeRoom", true)
            },
            {
                "Southeast Cross Room",
                new RegionInfo("EastFiligreeCache", true)
            },
            {
                "Fountain Cross Room",
                new RegionInfo("Town_FiligreeRoom", true)
            },
            {
                "Hourglass Cave",
                new RegionInfo("Town Basement", true)
            },
            {
                "Hourglass Cave Tower",
                new RegionInfo("Town Basement", true)
            },
            {
                "Sealed Temple",
                new RegionInfo("Temple", false)
            },
            {
                "Sealed Temple Rafters",
                new RegionInfo("Temple", false)
            },
            {
                "Forest Belltower Upper",
                new RegionInfo("Forest Belltower", false)
            },
            {
                "Forest Belltower Main",
                new RegionInfo("Forest Belltower", false)
            },
            {
                "Forest Belltower Main behind bushes",
                new RegionInfo("Forest Belltower", false)
            },
            {
                "Forest Belltower Lower",
                new RegionInfo("Forest Belltower", false)
            },
            {
                "East Forest",
                new RegionInfo("East Forest Redux", false)
            },
            {
                "East Forest Dance Fox Spot",
                new RegionInfo("East Forest Redux", false)
            },
            {
                "East Forest Portal",
                new RegionInfo("East Forest Redux", false, outletRegion:"East Forest")
            },
            {
                "Lower Forest",
                new RegionInfo("East Forest Redux", false)
            },
            {
                "Guard House 1 East",
                new RegionInfo("East Forest Redux Laddercave", false)
            },
            {
                "Guard House 1 West",
                new RegionInfo("East Forest Redux Laddercave", false)
            },
            {
                "Guard House 2 Upper",
                new RegionInfo("East Forest Redux Interior", false)
            },
            {
                "Guard House 2 Upper after bushes",
                new RegionInfo("East Forest Redux Interior", false)
            },
            {
                "Guard House 2 Lower",
                new RegionInfo("East Forest Redux Interior", false)
            },
            {
                "Forest Boss Room",
                new RegionInfo("Forest Boss Room", false)
            },
            {
                "Forest Grave Path Main",
                new RegionInfo("Sword Access", false)
            },
            {
                "Forest Grave Path Upper",
                new RegionInfo("Sword Access", false)
            },
            {
                "Forest Grave Path by Grave",
                new RegionInfo("Sword Access", false)
            },
            {
                "Forest Hero's Grave",
                new RegionInfo("Sword Access", false, outletRegion:"Forest Grave Path by Grave")
            },
            {
                "Dark Tomb Entry Point",
                new RegionInfo("Crypt Redux", false)
            },
            {
                "Dark Tomb Upper",
                new RegionInfo("Crypt Redux", false)
            },
            {
                "Dark Tomb Main",
                new RegionInfo("Crypt Redux", false)
            },
            {
                "Dark Tomb Dark Exit",
                new RegionInfo("Crypt Redux", false)
            },
            {
                "Dark Tomb Checkpoint",
                new RegionInfo("Sewer_Boss", false)
            },
            {
                "Well Boss",
                new RegionInfo("Sewer_Boss", false)
            },
            {
                "Beneath the Well Ladder Exit",
                new RegionInfo("Sewer", false)
            },
            {
                "Beneath the Well Front",
                new RegionInfo("Sewer", false)
            },
            {
                "Beneath the Well Main",
                new RegionInfo("Sewer", false)
            },
            {
                "Beneath the Well Back",
                new RegionInfo("Sewer", false)
            },
            {
                "West Garden",
                new RegionInfo("Archipelagos Redux", false)
            },
            {
                "West Garden Portal",
                new RegionInfo("Archipelagos Redux", true, outletRegion:"West Garden by Portal")
            },
            {
                "West Garden by Portal",
                new RegionInfo("Archipelagos Redux", true)
            },
            {
                "West Garden Portal Item",
                new RegionInfo("Archipelagos Redux", true)
            },
            {
                "West Garden Laurels Exit",
                new RegionInfo("Archipelagos Redux", false)
            },
            {
                "West Garden after Boss",
                new RegionInfo("Archipelagos Redux", false)
            },
            {
                "West Garden Hero's Grave",
                new RegionInfo("Archipelagos Redux", false, outletRegion:"West Garden")
            },
            {
                "Magic Dagger House",
                new RegionInfo("archipelagos_house", true)
            },
            {
                "Ruined Atoll",
                new RegionInfo("Atoll Redux", false)
            },
            {
                "Ruined Atoll Lower Entry Area",
                new RegionInfo("Atoll Redux", false)
            },
            {
                "Ruined Atoll Ladder Tops",
                new RegionInfo("Atoll Redux", false)
            },
            {
                "Ruined Atoll Frog Eye",
                new RegionInfo("Atoll Redux", false)
            },
            {
                "Ruined Atoll Frog Mouth",
                new RegionInfo("Atoll Redux", false)
            },
            {
                "Ruined Atoll Portal",
                new RegionInfo("Atoll Redux", false, outletRegion:"Ruined Atoll")
            },
            {
                "Ruined Atoll Statue",
                new RegionInfo("Atoll Redux", false, outletRegion:"Ruined Atoll")
            },
            {
                "Frog Stairs Eye Exit",
                new RegionInfo("Frog Stairs", false)
            },
            {
                "Frog Stairs Upper",
                new RegionInfo("Frog Stairs", false)
            },
            {
                "Frog Stairs Lower",
                new RegionInfo("Frog Stairs", false)
            },
            {
                "Frog Stairs to Frog's Domain",
                new RegionInfo("Frog Stairs", false)
            },
            {
                "Frog's Domain Entry",
                new RegionInfo("frog cave main", false)
            },
            {  // for breakable shuffle, need combat logic or wand for the pots in the orb room
                "Frog's Domain",
                new RegionInfo("frog cave main", false)
            },
            {
                "Frog's Domain Back",
                new RegionInfo("frog cave main", false)
            },
            {
                "Library Exterior Tree",
                new RegionInfo("Library Exterior", false, outletRegion:"Library Exterior by Tree")
            },
            {
                "Library Exterior by Tree",
                new RegionInfo("Library Exterior", false)
            },
            {
                "Library Exterior Ladder",
                new RegionInfo("Library Exterior", false)
            },
            {
                "Library Hall Bookshelf",
                new RegionInfo("Library Hall", false)
            },
            {
                "Library Hall",
                new RegionInfo("Library Hall", false)
            },
            {
                "Library Hero's Grave",
                new RegionInfo("Library Hall", false, outletRegion:"Library Hall")
            },
            {
                "Library Hall to Rotunda",
                new RegionInfo("Library Hall", false)
            },
            {
                "Library Rotunda to Hall",
                new RegionInfo("Library Rotunda", false)
            },
            {
                "Library Rotunda",
                new RegionInfo("Library Rotunda", false)
            },
            {
                "Library Rotunda to Lab",
                new RegionInfo("Library Rotunda", false)
            },
            {
                "Library Lab",
                new RegionInfo("Library Lab", false)
            },
            {
                "Library Lab Lower",
                new RegionInfo("Library Lab", false)
            },
            {
                "Library Lab on Portal Pad",
                new RegionInfo("Library Lab", false)
            },
            {
                "Library Portal",
                new RegionInfo("Library Lab", false, outletRegion:"Library Lab on Portal Pad")
            },
            {
                "Library Lab to Librarian",
                new RegionInfo("Library Arena", false)
            },
            {
                "Library Arena",
                new RegionInfo("Library Arena", true)
            },
            {
                "Fortress Exterior from East Forest",
                new RegionInfo("Fortress Courtyard", false)
            },
            {
                "Fortress Exterior from Overworld",
                new RegionInfo("Fortress Courtyard", false)
            },
            {
                "Fortress Exterior near cave",
                new RegionInfo("Fortress Courtyard", false)
            },
            {
                "Beneath the Vault Entry",
                new RegionInfo("Fortress Courtyard", false)
            },
            {
                "Fortress Courtyard",
                new RegionInfo("Fortress Courtyard", false)
            },
            {
                "Fortress Courtyard Upper",
                new RegionInfo("Fortress Courtyard", false)
            },
            {
                "Beneath the Vault Ladder Exit",
                new RegionInfo("Fortress Basement", false)
            },
            {  // at the bottom of the ladder, added for breakable shuffle
                "Beneath the Vault Entry Spot",
                new RegionInfo("Fortress Basement", false)
            },
            {
                "Beneath the Vault Main",
                new RegionInfo("Fortress Basement", false)
            },
            {
                "Beneath the Vault Back",
                new RegionInfo("Fortress Basement", false)
            },
            {
                "Eastern Vault Fortress",
                new RegionInfo("Fortress Main", false)
            },
            {
                "Eastern Vault Fortress Gold Door",
                new RegionInfo("Fortress Main", false)
            },
            {
                "Fortress East Shortcut Upper",
                new RegionInfo("Fortress East", false)
            },
            {
                "Fortress East Shortcut Lower",
                new RegionInfo("Fortress East", false)
            },
            {
                "Fortress Grave Path",
                new RegionInfo("Fortress Reliquary", false)
            },
            {
                "Fortress Grave Path Upper",
                new RegionInfo("Fortress Reliquary", true)
            },
            {
                "Fortress Grave Path Dusty Entrance",
                new RegionInfo("Fortress Reliquary", false)
            },
            {
                "Fortress Hero's Grave",
                new RegionInfo("Fortress Reliquary", false, outletRegion:"Fortress Grave Path")
            },
            {
                "Fortress Leaf Piles",
                new RegionInfo("Dusty", true)
            },
            {
                "Fortress Arena",
                new RegionInfo("Fortress Arena", false)
            },
            {
                "Fortress Arena Portal",
                new RegionInfo("Fortress Arena", false, outletRegion:"Fortress Arena")
            },
            {
                "Lower Mountain",
                new RegionInfo("Mountain", false)
            },
            {
                "Lower Mountain Stairs",
                new RegionInfo("Mountain", false)
            },
            {
                "Top of the Mountain",
                new RegionInfo("Mountaintop", true)
            },
            {
                "Quarry Connector",
                new RegionInfo("Darkwoods Tunnel", false)
            },
            {
                "Quarry Entry",
                new RegionInfo("Quarry Redux", false)
            },
            {
                "Quarry",
                new RegionInfo("Quarry Redux", false)
            },
            {
                "Quarry Portal",
                new RegionInfo("Quarry Redux", false, outletRegion:"Quarry Entry")
            },
            {
                "Quarry Back",
                new RegionInfo("Quarry Redux", false)
            },
            {
                "Quarry Monastery Entry",
                new RegionInfo("Quarry Redux", false)
            },
            {
                "Monastery Front",
                new RegionInfo("Monastery", false)
            },
            {
                "Monastery Back",
                new RegionInfo("Monastery", false)
            },
            {
                "Monastery Hero's Grave",
                new RegionInfo("Monastery", false, outletRegion:"Monastery Back")
            },
            {
                "Monastery Rope",
                new RegionInfo("Quarry Redux", false)
            },
            {
                "Lower Quarry",
                new RegionInfo("Quarry Redux", false)
            },
            {
                "Even Lower Quarry",
                new RegionInfo("Quarry Redux", false)
            },
            {
                "Even Lower Quarry Isolated Chest",
                new RegionInfo("Quarry Redux", false)
            },
            {
                "Lower Quarry Zig Door",
                new RegionInfo("Quarry Redux", false)
            },
            {
                "Rooted Ziggurat Entry",
                new RegionInfo("ziggurat2020_0", false)
            },
            {
                "Rooted Ziggurat Upper Entry",
                new RegionInfo("ziggurat2020_1", false)
            },
            {
                "Rooted Ziggurat Upper Front",
                new RegionInfo("ziggurat2020_1", false)
            },
            {
                "Rooted Ziggurat Upper Back",
                new RegionInfo("ziggurat2020_1", false)
            },
            {
                "Rooted Ziggurat Middle Top",
                new RegionInfo("ziggurat2020_2", false)
            },
            {
                "Rooted Ziggurat Middle Bottom",
                new RegionInfo("ziggurat2020_2", false)
            },
            {
                "Rooted Ziggurat Lower Front",
                new RegionInfo("ziggurat2020_3", false)
            },
            {
                "Rooted Ziggurat Lower Mid Checkpoint",
                new RegionInfo("ziggurat2020_3", false)
            },
            {
                "Rooted Ziggurat Lower Back",
                new RegionInfo("ziggurat2020_3", false)
            },
            {
                "Zig Skip Exit",
                new RegionInfo("ziggurat2020_3", false, outletRegion:"Rooted Ziggurat Lower Front", skipCounting:true)
            },
            {
                "Rooted Ziggurat Portal Room Entrance",
                new RegionInfo("ziggurat2020_3", false)
            },
            {
                "Rooted Ziggurat Portal",
                new RegionInfo("ziggurat2020_FTRoom", false, outletRegion:"Rooted Ziggurat Portal Room")
            },
            {
                "Rooted Ziggurat Portal Room",
                new RegionInfo("ziggurat2020_FTRoom", false)
            },
            {
                "Rooted Ziggurat Portal Room Exit",
                new RegionInfo("ziggurat2020_FTRoom", false, outletRegion:"Rooted Ziggurat Portal Room")
            },
            {
                "Swamp Front",
                new RegionInfo("Swamp Redux 2", false)
            },
            {
                "Swamp Mid",
                new RegionInfo("Swamp Redux 2", false)
            },
            {
                "Swamp Ledge under Cathedral Door",
                new RegionInfo("Swamp Redux 2", false)
            },
            {
                "Swamp to Cathedral Treasure Room",
                new RegionInfo("Swamp Redux 2", false, outletRegion:"Swamp Ledge under Cathedral Door")
            },
            {
                "Swamp to Cathedral Main Entrance",
                new RegionInfo("Swamp Redux 2", false)
            },
            {
                "Back of Swamp",
                new RegionInfo("Swamp Redux 2", false)
            },
            {
                "Swamp Hero's Grave",
                new RegionInfo("Swamp Redux 2", false, outletRegion:"Back of Swamp")
            },
            {
                "Back of Swamp Laurels Area",
                new RegionInfo("Swamp Redux 2", false)
            },
            {
                "Cathedral",
                new RegionInfo("Cathedral Redux", false)
            },
            {
                "Cathedral Elevator",
                new RegionInfo("Cathedral Redux", false)
            },
            {
                "Cathedral Secret Legend Room",
                new RegionInfo("Cathedral Redux", true)
            },
            {
                "Cathedral Gauntlet Checkpoint",
                new RegionInfo("Cathedral Arena", false)
            },
            {
                "Cathedral Gauntlet",
                new RegionInfo("Cathedral Arena", false)
            },
            {
                "Cathedral Gauntlet Exit",
                new RegionInfo("Cathedral Arena", false)
            },
            {
                "Far Shore",
                new RegionInfo("Transit", false)
            },
            {
                "Far Shore to Spawn",
                new RegionInfo("Transit", false)
            },
            {
                "Far Shore to East Forest",
                new RegionInfo("Transit", false)
            },
            {
                "Far Shore to Quarry",
                new RegionInfo("Transit", false, outletRegion:"Far Shore")
            },
            {
                "Far Shore to Fortress",
                new RegionInfo("Transit", false, outletRegion:"Far Shore")
            },
            {
                "Far Shore to Library",
                new RegionInfo("Transit", false, outletRegion:"Far Shore")
            },
            {
                "Far Shore to West Garden",
                new RegionInfo("Transit", false, outletRegion:"Far Shore")
            },
            {
                "Hero Relic - Fortress",
                new RegionInfo("RelicVoid", true)
            },
            {
                "Hero Relic - Quarry",
                new RegionInfo("RelicVoid", true)
            },
            {
                "Hero Relic - West Garden",
                new RegionInfo("RelicVoid", true)
            },
            {
                "Hero Relic - East Forest",
                new RegionInfo("RelicVoid", true)
            },
            {
                "Hero Relic - Library",
                new RegionInfo("RelicVoid", true)
            },
            {
                "Hero Relic - Swamp",
                new RegionInfo("RelicVoid", true)
            },
            {
                "Purgatory",
                new RegionInfo("Purgatory", false)
            },
            // shops don't get outlet regions because we don't want it to think you can walk from one shop exit to another
            {
                "Shop Entrance 1",
                new RegionInfo("Shop", true)
            },
            {
                "Shop Entrance 2",
                new RegionInfo("Shop", true)
            },
            {
                "Shop Entrance 3",
                new RegionInfo("Shop", true)
            },
            {
                "Shop Entrance 4",
                new RegionInfo("Shop", true)
            },
            {
                "Shop Entrance 5",
                new RegionInfo("Shop", true)
            },
            {
                "Shop Entrance 6",
                new RegionInfo("Shop", true)
            },
            {
                "Shop Entrance 7",
                new RegionInfo("Shop", true)
            },
            {
                "Shop Entrance 8",
                new RegionInfo("Shop", true)
            },
            {
                "Shop",
                new RegionInfo("Shop", true)
            },
            {
                "Spirit Arena",
                new RegionInfo("Spirit Arena", true)
            },
            {
                "LS Elev 0",
                new RegionInfo("Overworld Redux", false, skipCounting:true)
            },
            {
                "LS Elev 1",
                new RegionInfo("Overworld Redux", false, skipCounting:true)
            },
            {
                "LS Elev 2",
                new RegionInfo("Overworld Redux", false, skipCounting:true)
            },
            {
                "LS Elev 3",
                new RegionInfo("Overworld Redux", false, skipCounting:true)
            },
            {
                "LS Elev 4",
                new RegionInfo("Overworld Redux", false, skipCounting:true)
            },
            {
                "LS Elev 5",
                new RegionInfo("Overworld Redux", false, skipCounting:true)
            },
            {
                "LS Elev 6",
                new RegionInfo("Overworld Redux", false, skipCounting:true)
            },
            {
                "LS Elev 7",
                new RegionInfo("Overworld Redux", false, skipCounting:true)
            },
        };

        // LS gets you to a portal rather than a region usually, but during portal pairing that doesn't actually matter since we only have one portal you cannot walk out from (zig skip)
        // so, special keyword for when we just want to care about a connection during portal pairing, for use with LS
        // it should not be used for connections where you can do the connection with something other than Laurels, LS, or IG (since it'll auto-succeed during pairing)
        public static string PAIRING_ONLY = "Portal Pairing Only";

        // these are the traversal rules for getting from one region to another
        // for example, with the first one on the list, if you are in Overworld, you can get to Overworld Beach if you have Laurels, Orb, or Ladders in Overworld Town
        public static Dictionary<string, Dictionary<string, List<List<string>>>> TraversalReqs = new Dictionary<string, Dictionary<string, List<List<string>>>> {
            {
                "Overworld",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Overworld Beach",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Wand",
                            },
                            new List<string> {
                                "Ladders in Overworld Town",
                            },
                        }
                    },
                    {  // this is the direct path, see Overworld -> Overworld Beach -> Overworld to Atoll Upper for more pathing
                        "Overworld to Atoll Upper",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                    {
                        "Overworld Belltower",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "IG2S"
                            },
                            new List<string> {
                                "LS1", PAIRING_ONLY
                            },
                        }
                    },
                    {
                        "Overworld Special Shop Entry",
                        new List<List<string>> {
                            new List<string> {
                                "LS1", PAIRING_ONLY
                            },
                        }
                    },
                    {
                        "Overworld Swamp Upper Entry",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "LS1", PAIRING_ONLY
                            },
                        }
                    },
                    {
                        "Overworld Swamp Lower Entry",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder to Swamp",
                            },
                            new List<string> {
                                "IG3S"
                            },
                        }
                    },
                    {
                        "Overworld Well Ladder",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Well",
                            },
                        }
                    },
                    {  // need both keys, which isn't expressed here
                        "Overworld Ruined Passage Door",
                        new List<List<string>> {
                            new List<string> {
                                "Key",
                            },
                            new List<string> {
                                "Zip"
                            },
                        }
                    },
                    {
                        "After Ruined Passage",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders near Weathervane",
                            },
                            new List<string> {
                                "IG1L"
                            }
                        }
                    },
                    {
                        "Above Ruined Passage",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders near Weathervane",
                            },
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "IG1L"
                            }
                        }
                    },
                    {
                        "East Overworld",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders near Overworld Checkpoint",
                            },
                            new List<string> {
                                "IG1L"
                            },
                        }
                    },
                    {  // where the access to that orb-locked chest is
                        "Overworld above Patrol Cave",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders near Overworld Checkpoint",
                            },
                            new List<string> {
                                "Wand",
                            },
                        }
                    },
                    {
                        "Overworld above Quarry Entrance",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders near Dark Tomb",
                            },
                        }
                    },
                    {
                        "Overworld after Envoy",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Wand",
                            },
                            new List<string> {
                                "Heir Sword",
                            },
                            new List<string> {
                                "Shotgun",
                            },
                        }
                    },
                    {
                        "Overworld Tunnel Turret",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "LS1", PAIRING_ONLY
                            },
                        }
                    },
                    {
                        "Overworld to West Garden from Furnace",
                        new List<List<string>> {
                            new List<string> {
                                "IG3L"
                            },
                        }
                    },
                    {
                        "Overworld Southeast Cross Door",
                        new List<List<string>> {
                            new List<string> {
                                "21",
                            },
                            new List<string> {
                                "IG2S"
                            },
                        }
                    },
                    {  // stick just checks if you have at least one sword progression, the stick, or the sword
                        "Overworld Temple Door",
                        new List<List<string>> {
                            new List<string> {
                                "Stick", "Forest Belltower Upper", "Overworld Belltower at Bell", NO_BELL_SHUFFLE,
                            },
                            new List<string> {
                                "Techbow", "Forest Belltower Upper", "Overworld Belltower at Bell", NO_BELL_SHUFFLE,
                            },
                            new List<string> {
                                "West Bell", "East Bell", BELL_SHUFFLE,
                            },
                            new List<string> {
                                "IG2S"
                            },
                            new List<string> {
                                "LS3", PAIRING_ONLY
                            },
                        }
                    },
                    {
                        "Overworld Quarry Entry",
                        new List<List<string>> {
                            new List<string> {
                                "IG2S"
                            },
                            new List<string> {
                                "LS1", PAIRING_ONLY
                            },
                        }
                    },
                    {
                        "Overworld Fountain Cross Door",
                        new List<List<string>> {
                            new List<string> {
                                "21",
                            },
                            new List<string> {
                                "IG2S"
                            },
                        }
                    },
                    {
                        "Overworld Town Portal",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                    {
                        "Overworld Spawn Portal",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                    {
                        "Overworld Old House Door",
                        new List<List<string>> {
                            new List<string> {
                                "Key (House)",
                            },
                            new List<string> {
                                "IG2S"
                            },
                        }
                    },
                    {
                        "Cube Cave Entrance Region",
                        new List<List<string>> {
                            new List<string> {
                                "Shotgun",
                            },
                            new List<string> {
                                "Sword", "Shop",
                            },
                        }
                    },
                    {
                        "Overworld Well to Furnace Rail",
                        new List<List<string>> {
                            // because you can get to either end of the rail in LS, and the middle of the rail is worthless, this doesn't need pairing only on it
                            new List<string> {
                                "LS2"
                            },
                        }
                    },
                    {
                        "LS Elev 0",
                        new List<List<string>> {
                            new List<string> {
                                "LS1", "Ladders in Overworld Town"
                            },
                            new List<string> {
                                "LS1", "Ladder to Swamp"
                            }
                        }
                    },
                    {
                        "LS Elev 1",
                        new List<List<string>> {
                            new List<string> {
                                "LS1", "Ladders near Weathervane"
                            },
                            new List<string> {
                                "LS1", "Ladders in Overworld Town"
                            },
                            new List<string> {
                                "LS1", "Ladder to Swamp"
                            }
                        }
                    },
                    {
                        "LS Elev 2",
                        new List<List<string>> {
                            new List<string> {
                                "LS1", "Ladders near Weathervane"
                            }
                        }
                    },
                    {
                        "LS Elev 3",
                        new List<List<string>> {
                            new List<string> {
                                "LS1", "Ladders near Weathervane"
                            },
                            new List<string> {
                                "LS1", "Ladders in Overworld Town"
                            }
                        }
                    },
                    {
                        "LS Elev 4",
                        new List<List<string>> {
                            new List<string> {
                                "LS1", "Ladders near Dark Tomb"
                            },
                            new List<string> {
                                "LS1", "Ladders in Overworld Town"
                            },
                            new List<string> {
                                "LS1", "Ladders in Well"
                            }
                        }
                    },
                    {
                        "LS Elev 5",
                        new List<List<string>> {
                            new List<string> {
                                "LS1", "Ladders near Overworld Checkpoint"
                            }
                        }
                    },
                    {
                        "LS Elev 7",
                        new List<List<string>> {
                            new List<string> {
                                "LS1", "Ladders near Dark Tomb"
                            }
                        }
                    },
                }
            },

            {
                "LS Elev 0",
                new Dictionary<string, List<List<string>>> {
                    {
                        "LS Elev 1",
                        new List<List<string>> {
                            new List<string> {
                                "LS2"
                            }
                        }
                    },
                    {
                        "Overworld Beach",
                        new List<List<string>> {
                            new List<string> {
                                "LS2"
                            }
                        }
                    }
                }
            },

            {
                "LS Elev 1",
                new Dictionary<string, List<List<string>>> {
                    {
                        "LS Elev 2",
                        new List<List<string>> {
                            new List<string> {
                                "LS2"
                            }
                        }
                    },
                    {
                        "Overworld Tunnel Turret",
                        new List<List<string>> {
                            new List<string> {
                                "LS2"
                            }
                        }
                    }
                }
            },

            {
                "LS Elev 2",
                new Dictionary<string, List<List<string>>> {
                    {
                        "LS Elev 3",
                        new List<List<string>> {
                            new List<string> {
                                "LS2"
                            }
                        }
                    },
                    {
                        "After Ruined Passage",
                        new List<List<string>> {
                            new List<string> {
                                "LS2"
                            }
                        }
                    }
                }
            },

            {
                "LS Elev 3",
                new Dictionary<string, List<List<string>>> {
                    {
                        "LS Elev 4",
                        new List<List<string>> {
                            new List<string> {
                                "LS2"
                            }
                        }
                    },
                    {
                        "Overworld after Envoy",
                        new List<List<string>> {
                            new List<string> {
                                "LS2"
                            }
                        }
                    },
                    {
                        "East Overworld",
                        new List<List<string>> {
                            new List<string> {
                                "LS2"
                            }
                        }
                    }
                }
            },

            {
                "LS Elev 4",
                new Dictionary<string, List<List<string>>> {
                    {
                        "LS Elev 5",
                        new List<List<string>> {
                            new List<string> {
                                "LS2"
                            }
                        }
                    }
                }
            },

            {
                "LS Elev 5",
                new Dictionary<string, List<List<string>>> {
                    {
                        "LS Elev 6",
                        new List<List<string>> {
                            new List<string> {
                                "LS2"
                            }
                        }
                    },
                    {
                        "East Overworld",
                        new List<List<string>> {
                            new List<string> {
                                "LS2"
                            }
                        }
                    }
                }
            },

            {
                "LS Elev 6",
                new Dictionary<string, List<List<string>>> {
                    {
                        "LS Elev 7",
                        new List<List<string>> {
                            new List<string> {
                                "LS2"
                            }
                        }
                    },
                    {
                        "Overworld above Patrol Cave",
                        new List<List<string>> {
                            new List<string> {
                                "LS2"
                            }
                        }
                    }
                }
            },

            {
                "LS Elev 7",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Upper Overworld",
                        new List<List<string>> {
                            new List<string> {
                                "LS2"
                            }
                        }
                    }
                }
            },

            {
                "East Overworld",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Above Ruined Passage",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders near Weathervane",
                            },
                        }
                    },
                    {
                        "After Ruined Passage",
                        new List<List<string>> {
                            new List<string> {
                                "IG1L"
                            },
                            new List<string> {
                                "LS1", PAIRING_ONLY
                            },
                        }
                    },
                    //{
                    //    "Overworld",
                    //    new List<List<string>> {
                    //        new List<string> {
                    //            "Ladders near Overworld Checkpoint",
                    //        },
                    //    }
                    //},
                    {
                        "Overworld at Patrol Cave",
                        new List<List<string>> {
                        }
                    },
                    {
                        "Overworld above Patrol Cave",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders near Overworld Checkpoint"
                            },
                            new List<string> {
                                "IG1L"
                            },
                        }
                    },
                    {
                        "Overworld Special Shop Entry",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "LS1", PAIRING_ONLY
                            },
                        }
                    },
                }
            },
            {
                "Overworld Special Shop Entry",
                new Dictionary<string, List<List<string>>> {
                    {
                        "East Overworld",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Overworld Belltower",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Overworld Belltower at Bell",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders to West Bell",
                            },
                        }
                    },
                    //{
                    //    "Overworld",
                    //    new List<List<string>> {
                    //    }
                    //},
                    {
                        "Overworld to West Garden Upper",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders to West Bell",
                            },
                        }
                    },
                    {
                        "LS Elev 2",
                        new List<List<string>> {
                            new List<string> {
                                "LS1", "Ladders to West Bell"
                            },
                        }
                    },
                    {
                        "LS Elev 3",
                        new List<List<string>> {
                            new List<string> {
                                "LS1", "Ladders to West Bell"
                            },
                        }
                    },
                    {
                        "LS Elev 4",
                        new List<List<string>> {
                            new List<string> {
                                "LS1", "Ladders to West Bell"
                            },
                        }
                    },
                }
            },
            {
                "Overworld to West Garden Upper",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Overworld Belltower",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders to West Bell",
                            },
                        }
                    },
                }
            },
            //{
            //    "Overworld Swamp Upper Entry",
            //    new Dictionary<string, List<List<string>>> {
            //        {
            //            "Overworld",
            //            new List<List<string>> {
            //                new List<string> {
            //                    "Hyperdash",
            //                },
            //            }
            //        },
            //    }
            //},
            //{
            //    "Overworld Swamp Lower Entry",
            //    new Dictionary<string, List<List<string>>> {
            //        {
            //            "Overworld",
            //            new List<List<string>> {
            //                new List<string> {
            //                    "Ladder to Swamp",
            //                },
            //            }
            //        },
            //    }
            //},
            {
                "Overworld Beach",
                new Dictionary<string, List<List<string>>> {
                    //{
                    //    "Overworld",
                    //    new List<List<string>> {
                    //        new List<string> {
                    //            "Hyperdash",
                    //        },
                    //        new List<string> {
                    //            "Wand",
                    //        },
                    //        new List<string> {
                    //            "Ladders in Overworld Town",
                    //        },
                    //    }
                    //},
                    {
                        "Overworld West Garden Laurels Entry",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "LS1", PAIRING_ONLY
                            },
                        }
                    },
                    {
                        "Overworld to Atoll Upper",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder to Ruined Atoll",
                            },
                        }
                    },
                    {
                        "Overworld Tunnel Turret",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Overworld Town",
                            },
                            new List<string> {
                                "IG1S"
                            },
                        }
                    },
                    {
                        "LS Elev 0",
                        new List<List<string>> {
                            new List<string> {
                                "LS1", "Ladder to Ruined Atoll"
                            }
                        }
                    },
                }
            },
            //{  // cannot be logically relevant since you can get to overworld beach with laurels from overworld
            //    "Overworld West Garden Laurels Entry",
            //    new Dictionary<string, List<List<string>>> {
            //        {
            //            "Overworld Beach",
            //            new List<List<string>> {
            //                new List<string> {
            //                    "Hyperdash",
            //                },
            //            }
            //        },
            //    }
            //},
            {
                "Overworld to Atoll Upper",
                new Dictionary<string, List<List<string>>> {
                    //{
                    //    "Overworld",
                    //    new List<List<string>> {
                    //        new List<string> {
                    //            "Hyperdash",
                    //        },
                    //        new List<string> {
                    //            "Wand",
                    //        },
                    //    }
                    //},
                    {
                        "Overworld Beach",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder to Ruined Atoll",
                            },
                        }
                    },
                }
            },
            {
                "Overworld Tunnel Turret",
                new Dictionary<string, List<List<string>>> {
                    //{
                    //    "Overworld",
                    //    new List<List<string>> {
                    //        new List<string> {
                    //            "Hyperdash",
                    //        },
                    //        new List<string> {
                    //            "Wand",
                    //        },
                    //    }
                    //},
                    {
                        "Overworld Beach",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Overworld Town",
                            },
                            new List<string> {
                                "Wand",
                            },
                        }
                    },
                }
            },
            //{
            //    "Overworld Well Ladder",
            //    new Dictionary<string, List<List<string>>> {
            //        {
            //            "Overworld",
            //            new List<List<string>> {
            //                new List<string> {
            //                    "Ladders in Well",
            //                },
            //            }
            //        },
            //    }
            //},
            {
                "Overworld at Patrol Cave",
                new Dictionary<string, List<List<string>>> {
                    {
                        "East Overworld",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "IG1L"
                            },
                            new List<string> {
                                "LS1", PAIRING_ONLY
                            },
                        }
                    },
                    {
                        "Overworld above Patrol Cave",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders near Patrol Cave",
                            },
                            new List<string> {
                                "IG1L"
                            }
                        }
                    },
                    {
                        "LS Elev 5",
                        new List<List<string>> {
                            new List<string> {
                                "LS1", "Ladders near Patrol Cave"
                            }
                        }
                    },
                    {
                        "LS Elev 6",
                        new List<List<string>> {
                            new List<string> {
                                "LS1", "Ladders near Patrol Cave"
                            }
                        }
                    },
                    {
                        "LS Elev 7",
                        new List<List<string>> {
                            new List<string> {
                                "LS1", "Ladders near Patrol Cave"
                            }
                        }
                    },
                }
            },
            {
                "Overworld above Patrol Cave",
                new Dictionary<string, List<List<string>>> {
                    //{
                    //    "Overworld",
                    //    new List<List<string>> {
                    //        new List<string> {
                    //            "Ladders near Overworld Checkpoint",
                    //        },
                    //    }
                    //},
                    {
                        "Upper Overworld",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders near Patrol Cave",
                            },
                            new List<string> {
                                "IG1L"
                            }
                        }
                    },
                    {
                        "Overworld at Patrol Cave",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders near Patrol Cave",
                            },
                        }
                    },
                }
            },
            {
                "Upper Overworld",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Overworld above Patrol Cave",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders near Patrol Cave",
                            },
                            new List<string> {
                                "Wand",
                            },
                        }
                    },
                    {
                        "Overworld above Quarry Entrance",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Wand",
                            },
                        }
                    },
                    {
                        "Overworld after Temple Rafters",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder near Temple Rafters",
                            },
                            new List<string> {
                                "IG2L"
                            },
                        }
                    },
                }
            },
            {
                "Overworld after Temple Rafters",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Upper Overworld",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder near Temple Rafters",
                            },
                            new List<string> {
                                "IG1L"
                            },
                        }
                    },
                    {
                        "LS Elev 6",
                        new List<List<string>> {
                            new List<string> {
                                "LS1", "Ladder near Temple Rafters"
                            },
                        }
                    },
                    {
                        "LS Elev 7",
                        new List<List<string>> {
                            new List<string> {
                                "LS1", "Ladder near Temple Rafters"
                            },
                        }
                    },
                }
            },
            {
                "Overworld above Quarry Entrance",
                new Dictionary<string, List<List<string>>> {
                    //{
                    //    "Overworld",
                    //    new List<List<string>> {
                    //        new List<string> {
                    //            "Ladders near Dark Tomb",
                    //        },
                    //    }
                    //},
                    {
                        "Upper Overworld",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Wand",
                            },
                        }
                    },
                }
            },
            {
                "Overworld Quarry Entry",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Overworld after Envoy",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder to Quarry",
                            },
                        }
                    },
                    {
                        "LS Elev 3",
                        new List<List<string>> {
                            new List<string> {
                                "LS1", "Ladders to Quarry"
                            }
                        }
                    },
                    {
                        "LS Elev 4",
                        new List<List<string>> {
                            new List<string> {
                                "LS1", "Ladders to Quarry"
                            }
                        }
                    },
                }
            },
            {
                "Overworld after Envoy",
                new Dictionary<string, List<List<string>>> {
                    //{
                    //    "Overworld",
                    //    new List<List<string>> {
                    //        new List<string> {
                    //            "Hyperdash",
                    //        },
                    //        new List<string> {
                    //            "Wand",
                    //        },
                    //        new List<string> {
                    //            "Heir Sword",
                    //        },
                    //        new List<string> {
                    //            "Shotgun",
                    //        },
                    //    }
                    //},
                    {
                        "Overworld Quarry Entry",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder to Quarry",
                            },
                        }
                    },
                }
            },
            {
                "After Ruined Passage",
                new Dictionary<string, List<List<string>>> {
                    //{
                    //    "Overworld",
                    //    new List<List<string>> {
                    //        new List<string> {
                    //            "Ladders near Weathervane",
                    //        },
                    //    }
                    //},
                    {
                        "Above Ruined Passage",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders near Weathervane",
                            },
                        }
                    },
                    {
                        "East Overworld",
                        new List<List<string>> {
                            new List<string> {
                                "IG1L"
                            }
                        }
                    }
                }
            },
            {
                "Above Ruined Passage",
                new Dictionary<string, List<List<string>>> {
                    //{
                    //    "Overworld",
                    //    new List<List<string>> {
                    //        new List<string> {
                    //            "Ladders near Weathervane",
                    //        },
                    //        new List<string> {
                    //            "Hyperdash",
                    //        },
                    //    }
                    //},
                    {
                        "After Ruined Passage",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders near Weathervane",
                            },
                        }
                    },
                    {
                        "East Overworld",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders near Weathervane",
                            },
                            new List<string> {
                                "IG1L"
                            }
                        }
                    },
                }
            },
            //{
            //    "Cube Cave Entrance Region",
            //    new Dictionary<string, List<List<string>>> {
            //        {
            //            "Overworld",
            //            new List<List<string>> {
            //            }
            //        },
            //    }
            //},
            {
                "Old House Front",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Old House Back",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Old House Back",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Old House Front",
                        new List<List<string>> {
                            new List<string> {
                                "Zip"
                            }
                        }
                    },
                }
            },

            {
                "Furnace Fuse",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Furnace Ladder Area",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Furnace Ladder Area",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Furnace Fuse",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "LS1", PAIRING_ONLY
                            },
                        }
                    },
                    {
                        "Furnace Walking Path",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "LS1", PAIRING_ONLY
                            },
                        }
                    },
                }
            },
            {
                "Furnace Walking Path",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Furnace Ladder Area",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },

            {
                "Sealed Temple",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Sealed Temple Rafters",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Sealed Temple Rafters",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Sealed Temple",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },

            {
                "Hourglass Cave",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Hourglass Cave Tower",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Hourglass Cave",
                            },
                        }
                    },
                }
            },

            {
                "Forest Belltower Upper",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Forest Belltower Main",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Forest Belltower Main",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Forest Belltower Lower",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder to East Forest",
                            },
                        }
                    },
                    {
                        "Forest Belltower Main behind bushes",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Forest Belltower Main behind bushes",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Forest Belltower Main",
                        new List<List<string>> {
                            new List<string> {
                                "Sword",
                            },
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Techbow",
                            },
                            new List<string> {
                                "Shotgun",
                            },
                            new List<string> {
                                "Stick", "Trinket - Glass Cannon",
                            },
                            new List<string> {
                                "IG1S"
                            },
                        }
                    },
                }
            },

            {
                "East Forest",
                new Dictionary<string, List<List<string>>> {
                    {
                        "East Forest Dance Fox Spot",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "IG1L"
                            },
                            new List<string> {
                                "LS1", PAIRING_ONLY
                            },
                            new List<string> {
                                "LS2"
                            },
                        }
                    },
                    {
                        "East Forest Portal",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                    {
                        "Lower Forest",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders to Lower Forest",
                            },
                            new List<string> {
                                "IG1L"
                            },
                        }
                    },
                }
            },
            {
                "East Forest Dance Fox Spot",
                new Dictionary<string, List<List<string>>> {
                    {
                        "East Forest",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "IG1L"
                            },
                        }
                    },
                }
            },
            {
                "East Forest Portal",
                new Dictionary<string, List<List<string>>> {
                    {
                        "East Forest",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Lower Forest",
                new Dictionary<string, List<List<string>>> {
                    {
                        "East Forest",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders to Lower Forest",
                            },
                        }
                    },
                }
            },

            {
                "Guard House 1 East",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Guard House 1 West",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Guard House 1 West",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Guard House 1 East",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "LS1", PAIRING_ONLY
                            },
                        }
                    },
                }
            },

            {
                "Guard House 2 Upper",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Guard House 2 Upper after bushes",
                        new List<List<string>> {
                            new List<string> {
                                "Sword",
                            },
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Techbow",
                            },
                            new List<string> {
                                "Shotgun",
                            },
                            new List<string> {
                                "Stick", "Trinket - Glass Cannon",
                            },
                        }
                    },
                }
            },
            {
                "Guard House 2 Upper after bushes",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Guard House 2 Lower",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders to Lower Forest",
                            },
                        }
                    },
                    {
                        "Guard House 2 Upper",
                        new List<List<string>> {
                            new List<string> {
                                "Sword",
                            },
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Techbow",
                            },
                            new List<string> {
                                "Shotgun",
                            },
                            new List<string> {
                                "Stick", "Trinket - Glass Cannon",
                            },
                        }
                    },
                }
            },
            {
                "Guard House 2 Lower",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Guard House 2 Upper after bushes",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders to Lower Forest",
                            },
                        }
                    },
                }
            },

            {
                "Forest Grave Path Main",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Forest Grave Path Upper",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "IG3S"
                            },
                            new List<string> {
                                "LS2", PAIRING_ONLY
                            },
                        }
                    },
                    {
                        "Forest Grave Path by Grave",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Forest Grave Path Upper",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Forest Grave Path Main",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "IG1L"
                            },
                        }
                    },
                }
            },
            {
                "Forest Grave Path by Grave",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Forest Hero's Grave",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                    {
                        "Forest Grave Path Main",
                        new List<List<string>> {
                            new List<string> {
                                "IG1S"
                            },
                            new List<string> {
                                "Zip"
                            },
                        }
                    },
                }
            },
            {
                "Forest Hero's Grave",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Forest Grave Path by Grave",
                        new List<List<string>> {
                        }
                    },
                }
            },

            {
                "Beneath the Well Ladder Exit",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Beneath the Well Front",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Well",
                            },
                        }
                    },
                }
            },
            {
                "Beneath the Well Front",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Beneath the Well Ladder Exit",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Well",
                            },
                        }
                    },
                    {
                        "Beneath the Well Main",
                        new List<List<string>> {
                            new List<string> {
                                "Stick",
                            },
                            new List<string> {
                                "Techbow",
                            },
                        }
                    },
                    {
                        "Beneath the Well Back",
                        new List<List<string>> {
                            new List<string> {
                                "LS3", "Ladders in Well"
                            },
                        }
                    },
                }
            },
            {
                "Beneath the Well Main",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Beneath the Well Front",
                        new List<List<string>> {
                        }
                    },
                    {
                        "Beneath the Well Back",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Well",
                            },
                        }
                    },
                }
            },
            {
                "Beneath the Well Back",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Beneath the Well Main",
                        new List<List<string>> {
                            new List<string> {
                                "Stick", "Ladders in Well"
                            },
                            new List<string> {
                                "Techbow", "Ladders in Well"
                            },
                        }
                    },
                }
            },

            {
                "Well Boss",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Dark Tomb Checkpoint",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Dark Tomb Checkpoint",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Well Boss",
                        new List<List<string>> {
                            new List<string> {
                                "Zip"
                            }
                        }
                    }
                }
            },

            {
                "Dark Tomb Entry Point",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Dark Tomb Upper",
                        new List<List<string>> {
                            new List<string> {
                                "Lantern"
                            },
                        }
                    },
                }
            },
            {
                "Dark Tomb Upper",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Dark Tomb Entry Point",
                        new List<List<string>> {
                        }
                    },
                    {
                        "Dark Tomb Main",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder in Dark Tomb"
                            },
                            new List<string> {
                                "IG3S"
                            },
                        }
                    },
                }
            },
            {
                "Dark Tomb Main",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Dark Tomb Dark Exit",
                        new List<List<string>> {
                        }
                    },
                    {
                        "Dark Tomb Upper",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder in Dark Tomb"
                            },
                        }
                    },
                }
            },
            {
                "Dark Tomb Dark Exit",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Dark Tomb Main",
                        new List<List<string>> {
                            new List<string> {
                                "Lantern"
                            },
                        }
                    },
                }
            },

            {
                "West Garden",
                new Dictionary<string, List<List<string>>> {
                    {
                        "West Garden Laurels Exit",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash"
                            },
                            new List<string> {
                                "LS1", PAIRING_ONLY
                            },
                        }
                    },
                    {
                        "West Garden after Boss",
                        new List<List<string>> {
                            new List<string> {
                                "Sword"
                            },
                            new List<string> {
                                "Hyperdash"
                            },
                            new List<string> {
                                "IG2S"
                            },
                        }
                    },
                    {
                        "West Garden Hero's Grave",
                        new List<List<string>> {
                            new List<string> {
                                "12"
                            },
                        }
                    },
                    {
                        "West Garden Portal Item",
                        new List<List<string>> {
                            new List<string> {
                                "IG2L"
                            },
                        }
                    },
                }
            },
            {
                "West Garden Laurels Exit",
                new Dictionary<string, List<List<string>>> {
                    {
                        "West Garden",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash"
                            },
                        }
                    },
                }
            },
            {
                "West Garden after Boss",
                new Dictionary<string, List<List<string>>> {
                    {
                        "West Garden",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash"
                            },
                            // ice grapple to the boss, then ice grapple them off
                            new List<string> {
                                "IG2S"
                            },
                            // ice grapple to the boss, then fight
                            new List<string> {
                                "IG1S", "Sword"
                            }
                        }
                    },
                }
            },
            {
                "West Garden Portal Item",
                new Dictionary<string, List<List<string>>> {
                    {
                        "West Garden by Portal",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                    {
                        "West Garden",
                        new List<List<string>> {
                            new List<string> {
                                "IG1L"
                            },
                        }
                    }
                }
            },
            {
                "West Garden by Portal",
                new Dictionary<string, List<List<string>>> {
                    {
                        "West Garden Portal Item",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                    {
                        "West Garden Portal",
                        new List<List<string>> {
                            new List<string> {
                                "12", "West Garden", NO_FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "12", "West Garden Fuse", FUSE_SHUFFLE,
                            }
                        }
                    },
                }
            },
            {
                "West Garden Portal",
                new Dictionary<string, List<List<string>>> {
                    {
                        "West Garden by Portal",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "West Garden Hero's Grave",
                new Dictionary<string, List<List<string>>> {
                    {
                        "West Garden",
                        new List<List<string>> {
                        }
                    },
                }
            },

            {
                "Ruined Atoll",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Ruined Atoll Lower Entry Area",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "IG1L"
                            },
                            new List<string> {
                                "LS1", PAIRING_ONLY
                            },
                        }
                    },
                    {
                        "Ruined Atoll Ladder Tops",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in South Atoll",
                            },
                        }
                    },
                    {
                        "Ruined Atoll Frog Mouth",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Wand",
                            },
                        }
                    },
                    {
                        "Ruined Atoll Frog Eye",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders to Frog's Domain",
                            },
                        }
                    },
                    {
                        "Ruined Atoll Portal",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                    {
                        "Ruined Atoll Statue",
                        new List<List<string>> {
                            new List<string> {
                                "12", "Ladders in South Atoll", "Hyperdash", "Sword", NO_FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "12", "Ladders in South Atoll", "Hyperdash", "Shotgun", NO_FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "12", "Ladders in South Atoll", "Hyperdash", "Techbow", NO_FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "12", "Ladders in South Atoll", "Wand", "Sword", NO_FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "12", "Ladders in South Atoll", "Wand", "Bow", NO_FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "12", "Ladders in South Atoll", "Wand", "Techbow", NO_FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "12", "LS3", "Techbow", NO_FUSE_SHUFFLE
                            },
                            new List<string> {
                                "12",
                                "Atoll Northeast Fuse",
                                "Atoll Northwest Fuse",
                                "Atoll Southeast Fuse",
                                "Atoll Southwest Fuse",
                                FUSE_SHUFFLE,
                            },
                        }
                    },
                }
            },
            {
                "Ruined Atoll Lower Entry Area",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Ruined Atoll",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Wand",
                            },
                        }
                    },
                }
            },
            {
                "Ruined Atoll Frog Mouth",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Ruined Atoll",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Wand",
                            },
                        }
                    },
                }
            },
            {
                "Ruined Atoll Frog Eye",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Ruined Atoll",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders to Frog's Domain",
                            },
                        }
                    },
                }
            },
            {
                "Ruined Atoll Portal",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Ruined Atoll",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Ruined Atoll Statue",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Ruined Atoll",
                        new List<List<string>> {
                        }
                    },
                }
            },

            {
                "Frog Stairs Eye Exit",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Frog Stairs Upper",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders to Frog's Domain",
                            },
                            new List<string> {
                                "IG1L"
                            },
                        }
                    },
                }
            },
            {
                "Frog Stairs Upper",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Frog Stairs Eye Exit",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders to Frog's Domain",
                            },
                        }
                    },
                    {
                        "Frog Stairs Lower",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders to Frog's Domain",
                            },
                        }
                    },
                }
            },
            {
                "Frog Stairs Lower",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Frog Stairs Upper",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders to Frog's Domain",
                            },
                        }
                    },
                    {
                        "Frog Stairs to Frog's Domain",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders to Frog's Domain",
                            },
                        }
                    },
                }
            },
            {
                "Frog Stairs to Frog's Domain",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Frog Stairs Lower",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders to Frog's Domain",
                            },
                        }
                    },
                }
            },

            {
                "Frog's Domain Entry",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Frog's Domain",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders to Frog's Domain",
                            },
                        }
                    },
                }
            },
            {
                "Frog's Domain",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Frog's Domain Back",
                        new List<List<string>> {
                            new List<string> {
                                "Wand",
                            },
                            new List<string> {
                                "LS3", "Ladders to Frog's Domain"
                            },
                        }
                    },
                }
            },

            {
                "Library Exterior Tree",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Exterior by Tree",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Library Exterior by Tree",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Exterior Tree",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                    {
                        "Library Exterior Ladder",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash", "Ladders in Library",
                            },
                            new List<string> {
                                "Wand", "Ladders in Library",
                            },
                        }
                    },
                }
            },
            {
                "Library Exterior Ladder",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Exterior by Tree",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash", "Ladders in Library",
                            },
                            new List<string> {
                                "Wand",
                            },
                        }
                    },
                }
            },

            {
                "Library Hall Bookshelf",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Hall",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Library",
                            },
                        }
                    },
                }
            },
            {
                "Library Hall",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Hall Bookshelf",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Library",
                            },
                        }
                    },
                    {
                        "Library Hero's Grave",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                    {
                        "Library Hall to Rotunda",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Library",
                            },
                        }
                    },
                }
            },
            {
                "Library Hero's Grave",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Hall",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Library Hall to Rotunda",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Hall",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Library",
                            },
                        }
                    },
                }
            },

            {
                "Library Rotunda to Hall",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Rotunda",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Library",
                            },
                        }
                    },
                }
            },
            {
                "Library Rotunda",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Rotunda to Hall",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Library",
                            },
                        }
                    },
                    {
                        "Library Rotunda to Lab",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Library",
                            },
                        }
                    },
                }
            },
            {
                "Library Rotunda to Lab",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Rotunda",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Library",
                            },
                        }
                    },
                }
            },

            {
                "Library Lab Lower",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Lab",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash", "Ladders in Library",
                            },
                            new List<string> {
                                "Wand", "Ladders in Library",
                            },
                        }
                    },
                }
            },
            {
                "Library Lab",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Lab Lower",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash", "Ladders in Library",
                            },
                        }
                    },
                    {
                        "Library Lab on Portal Pad",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Library",
                            },
                        }
                    },
                    {
                        "Library Lab to Librarian",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Library",
                            },
                        }
                    }
                }
            },
            {
                "Library Lab on Portal Pad",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Portal",
                        new List<List<string>> {
                            new List<string> {
                                "12", "Ladders in Library", NO_FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "12", "Library Lab Fuse", FUSE_SHUFFLE,
                            }
                        }
                    },
                    {
                        "Library Lab",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Library",
                            },
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Library Portal",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Lab on Portal Pad",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Library Lab to Librarian",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Lab",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Library",
                            },
                        }
                    },
                }
            },

            {
                "Fortress Exterior from East Forest",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Exterior from Overworld",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Wand",
                            },
                        }
                    },
                    {
                        "Fortress Courtyard Upper",
                        new List<List<string>> {
                            new List<string> {
                                // LS2 includes getting knocked down by melee enemies, so this does not need the pairing only tag
                                "LS2"
                            },
                            new List<string> {
                                "LS1", PAIRING_ONLY
                            },
                        }
                    },
                }
            },
            {
                "Fortress Exterior from Overworld",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Exterior from East Forest",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                    {
                        "Fortress Exterior near cave",
                        new List<List<string>> {
                            new List<string> {
                                "12", NO_FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "12", "Fortress Exterior Fuse 1", FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                    {
                        "Fortress Courtyard",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "IG1L"
                            },
                            new List<string> {
                                "LS1", PAIRING_ONLY
                            },
                            new List<string> {
                                "LS2"
                            },
                        }
                    },
                    {
                        "Fortress Courtyard Upper",
                        new List<List<string>> {
                            new List<string> {
                                "LS2"
                            },
                        }
                    },
                }
            },
            {
                "Fortress Exterior near cave",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Exterior from Overworld",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "12", "Fortress Exterior Fuse 1", FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "LS1", PAIRING_ONLY
                            },
                        }
                    },
                    {
                        "Beneath the Vault Entry",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder to Beneath the Vault",
                            },
                        }
                    },
                    {
                        "Fortress Courtyard",
                        new List<List<string>> {
                            new List<string> {
                                "IG3L"
                            },
                            new List<string> {
                                "LS1", PAIRING_ONLY
                            },
                            new List<string> {
                                "LS2", "Ladder to Beneath the Vault"
                            },
                        }
                    },
                    {
                        "Fortress Courtyard Upper",
                        new List<List<string>> {
                            new List<string> {
                                // LS2 includes getting knocked down by melee enemies, so this does not need the pairing only tag
                                "LS2"
                            },
                        }
                    },
                }
            },
            {
                "Beneath the Vault Entry",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Exterior near cave",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder to Beneath the Vault",
                            },
                        }
                    },
                }
            },
            {
                "Fortress Courtyard",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Exterior from Overworld",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                    {
                        "Fortress Courtyard Upper",
                        new List<List<string>> {
                            new List<string> {
                                "IG1L"
                            },
                        }
                    },
                }
            },
            {
                "Fortress Courtyard Upper",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Courtyard",
                        new List<List<string>> {
                        }
                    },
                }
            },

            {
                "Beneath the Vault Ladder Exit",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Beneath the Vault Entry Spot",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder to Beneath the Vault",
                            },
                        }
                    },
                }
            },
            {
                "Beneath the Vault Entry Spot",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Beneath the Vault Ladder Exit",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder to Beneath the Vault",
                            },
                        }
                    },
                    {
                        "Beneath the Vault Main",
                        new List<List<string>> {
                            new List<string> {
                                "Lantern", "Stick",
                            },
                            new List<string> {
                                "Lantern", "Sword",
                            },
                            new List<string> {
                                "Lantern", "Wand",
                            },
                            new List<string> {
                                "Lantern", "Techbow",
                            },
                            new List<string> {
                                "Lantern", "Shotgun",
                            },
                            new List<string> {
                                "Lantern", "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Beneath the Vault Main",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Beneath the Vault Entry Spot",
                        new List<List<string>> {
                        }
                    },
                    {
                        "Beneath the Vault Back",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Beneath the Vault Back",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Beneath the Vault Main",
                        new List<List<string>> {
                            new List<string> {
                                "Lantern",
                            },
                        }
                    },
                    {
                        "Beneath the Vault Ladder Exit",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder to Beneath the Vault",
                            },
                        }
                    },
                }
            },

            {
                "Fortress East Shortcut Upper",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress East Shortcut Lower",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Fortress East Shortcut Lower",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress East Shortcut Upper",
                        new List<List<string>> {
                            new List<string> {
                                "IG1L"
                            }
                        }
                    },
                }
            },

            {
                "Eastern Vault Fortress",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Eastern Vault Fortress Gold Door",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                                "Fortress Exterior from Overworld",
                                "Beneath the Vault Back",
                                "Fortress Courtyard Upper",
                                NO_FUSE_SHUFFLE
                            },
                            new List<string> {
                                "Fortress Exterior Fuse 1",
                                "Fortress Exterior Fuse 2",
                                "Fortress Courtyard Upper Fuse",
                                "Fortress Courtyard Fuse",
                                "Beneath the Vault Fuse",
                                "Fortress Candles Fuse",
                                "Fortress Door Left Fuse",
                                "Fortress Door Right Fuse",
                                FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "IG2S"
                            },
                        }
                    },
                }
            },
            {
                "Eastern Vault Fortress Gold Door",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Eastern Vault Fortress",
                        new List<List<string>> {
                            new List<string> {
                                "IG1S"
                            },
                            new List<string> {
                                FUSE_SHUFFLE,
                                "Fortress Exterior Fuse 1",
                                "Fortress Exterior Fuse 2",
                                "Fortress Courtyard Upper Fuse",
                                "Fortress Courtyard Fuse",
                                "Beneath the Vault Fuse",
                                "Fortress Candles Fuse",
                                "Fortress Door Left Fuse",
                                "Fortress Door Right Fuse",
                            },
                        }
                    }
                }
            },

            {
                "Fortress Grave Path",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Hero's Grave",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                    {
                        "Fortress Grave Path Dusty Entrance",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Fortress Grave Path Upper",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Grave Path",
                        new List<List<string>> {
                            new List<string> {
                                "IG1L",
                            },
                        }
                    },
                }
            },
            {
                "Fortress Grave Path Dusty Entrance",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Grave Path",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Fortress Hero's Grave",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Grave Path",
                        new List<List<string>> {
                        }
                    },
                }
            },

            {
                "Fortress Arena",
                new Dictionary<string, List<List<string>>> {
                    {  // only the left fuses
                        "Fortress Arena Portal",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                                "Fortress Exterior from Overworld",
                                "Beneath the Vault Back",
                                "Eastern Vault Fortress",
                                NO_FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "12",
                                "Fortress Exterior Fuse 1",
                                "Fortress Exterior Fuse 2",
                                "Beneath the Vault Fuse",
                                "Fortress Candles Fuse",
                                "Fortress Door Left Fuse",
                                FUSE_SHUFFLE,
                            }
                        }
                    },
                }
            },
            {
                "Fortress Arena Portal",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Arena",
                        new List<List<string>> {
                        }
                    },
                }
            },

            {
                "Lower Mountain",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Lower Mountain Stairs",
                        new List<List<string>> {
                            new List<string> {
                                "21",
                            },
                        }
                    },
                }
            },
            {
                "Lower Mountain Stairs",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Lower Mountain",
                        new List<List<string>> {
                            new List<string> {
                                "21",
                            },
                        }
                    },
                }
            },

            {
                "Monastery Front",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Monastery Back",
                        new List<List<string>> {
                            new List<string> {
                                "Techbow",
                            },
                            new List<string> {
                                "Sword",
                            },
                            new List<string> {
                                "Zip"
                            }
                        }
                    },
                }
            },
            {
                "Monastery Back",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Monastery Hero's Grave",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                    {
                        "Monastery Front",
                        new List<List<string>> {
                            new List<string> {
                                "Zip"
                            }
                        }
                    }
                }
            },
            {
                "Monastery Hero's Grave",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Monastery Back",
                        new List<List<string>> {
                        }
                    },
                }
            },

            {
                "Quarry Entry",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Quarry Portal",
                        new List<List<string>> {
                            new List<string> {
                                "12", "Quarry Connector", "Wand", NO_FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "12", "Quarry Fuse 1", "Quarry Fuse 2", FUSE_SHUFFLE,
                            }
                        }
                    },
                    {
                        "Quarry",
                        new List<List<string>> {
                            new List<string> {
                                "Techbow",
                            },
                            new List<string> {
                                "Sword",
                            },
                        }
                    },
                    {
                        "Monastery Rope",
                        new List<List<string>> {
                            new List<string> {
                                "LS2", PAIRING_ONLY,
                            }
                        }
                    },
                }
            },
            {
                "Quarry Portal",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Quarry Entry",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Quarry Monastery Entry",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Quarry",
                        new List<List<string>> {
                            new List<string> {
                                "Techbow",
                            },
                            new List<string> {
                                "Sword",
                            },
                        }
                    },
                    {
                        "Quarry Back",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                    {
                        "Monastery Rope",
                        new List<List<string>> {
                            new List<string> {
                                "LS2", PAIRING_ONLY,
                            }
                        }
                    },
                }
            },
            {
                "Quarry Back",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Quarry",
                        new List<List<string>> {
                            new List<string> {
                                "Techbow",
                            },
                            new List<string> {
                                "Sword",
                            },
                        }
                    },
                    {
                        "Quarry Monastery Entry",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Quarry",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Lower Quarry",
                        new List<List<string>> {
                            new List<string> {
                                "Mask",
                            },
                        }
                    },
                    {
                        "Quarry Entry",
                        new List<List<string>> {
                        }
                    },
                    {
                        "Quarry Back",
                        new List<List<string>> {
                        }
                    },
                    {
                        "Quarry Monastery Entry",
                        new List<List<string>> {
                        }
                    },
                    {
                        "Even Lower Quarry Isolated Chest",
                        new List<List<string>> {
                            new List<string> {
                                "IG3S"
                            }
                        }
                    },
                }
            },
            {
                "Lower Quarry",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Even Lower Quarry",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Lower Quarry",
                            },
                            new List<string> {
                                "IG1L"
                            },
                        }
                    },
                }
            },
            {
                "Even Lower Quarry",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Even Lower Quarry Isolated Chest",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Even Lower Quarry Isolated Chest",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Lower Quarry Zig Door",
                        new List<List<string>> {
                            new List<string> {
                                "Quarry", "Quarry Connector", "Wand", "12", NO_FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "Quarry Fuse 1", "Quarry Fuse 2", FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "IG3S"
                            },
                        }
                    },
                    {
                        "Even Lower Quarry",
                        new List<List<string>> {
                            new List<string> {
                                "Mask"
                            }
                        }
                    },
                }
            },
            {
                "Lower Quarry Zig Door",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Even Lower Quarry",
                        new List<List<string>> {
                            new List<string> {
                                "Quarry Fuse 1", "Quarry Fuse 2", FUSE_SHUFFLE,
                            },
                        }
                    },
                }
            },
            {
                "Monastery Rope",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Quarry Back",
                        new List<List<string>> {
                        }
                    },
                }
            },

            {
                "Rooted Ziggurat Upper Entry",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Rooted Ziggurat Upper Front",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Rooted Ziggurat Upper Front",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Rooted Ziggurat Upper Back",
                        new List<List<string>> {
                            new List<string> {
                                "Sword",
                            },
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Rooted Ziggurat Upper Back",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Rooted Ziggurat Upper Front",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },

            {
                "Rooted Ziggurat Middle Top",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Rooted Ziggurat Middle Bottom",
                        new List<List<string>> {
                        }
                    },
                }
            },

            {
                "Rooted Ziggurat Lower Front",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Rooted Ziggurat Lower Back",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Sword", "12", NO_FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "Ziggurat Miniboss Fuse", FUSE_SHUFFLE,
                            }
                        }
                    },
                    {
                        "Rooted Ziggurat Lower Mid Checkpoint",
                        new List<List<string>> {
                        }
                    }
                }
            },
            {
                "Rooted Ziggurat Lower Back",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Rooted Ziggurat Lower Front",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash", "Sword", "12", NO_FUSE_SHUFFLE
                            },
                            new List<string> {
                                "Ziggurat Miniboss Fuse", FUSE_SHUFFLE
                            },
                            new List<string> {
                                "IG1L"
                            },
                            // LS2 includes getting knocked down by melee enemies, so this does not need the pairing only tag
                            new List<string> {
                                "LS2"
                            },
                        }
                    },
                    {  // can't get to checkpoint if enemies aggro, gap too big
                        "Rooted Ziggurat Lower Mid Checkpoint",
                        new List<List<string>> {
                            new List<string> {
                                "LS2"
                            },
                        }
                    },
                    {
                        "Rooted Ziggurat Portal Room Entrance",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                                NO_FUSE_SHUFFLE
                            },
                            new List<string> {
                                "Ziggurat Teleporter Fuse",
                                FUSE_SHUFFLE
                            }
                        }
                    },
                }
            },
            {
                "Rooted Ziggurat Portal Room Entrance",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Rooted Ziggurat Lower Back",
                        new List<List<string>> {
                        }
                    },
                }
            },

            {
                "Rooted Ziggurat Portal Room Exit",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Rooted Ziggurat Portal Room",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Rooted Ziggurat Portal Room",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Rooted Ziggurat Portal Room Exit",
                        new List<List<string>> {
                            new List<string> {
                                "12", "Rooted Ziggurat Lower Back", NO_FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "Ziggurat Teleporter Fuse", FUSE_SHUFFLE,
                            }
                        }
                    },
                    {
                        "Rooted Ziggurat Portal",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                }
            },
            {
                "Rooted Ziggurat Portal",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Rooted Ziggurat Portal Room",
                        new List<List<string>> {
                        }
                    },
                }
            },

            {
                "Swamp Front",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Swamp Mid",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Swamp",
                            },
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "IG3S"
                            }
                        }
                    },
                    {
                        "Back of Swamp Laurels Area",
                        new List<List<string>> {
                            new List<string> {
                                "Zip"
                            }
                        }
                    }
                }
            },
            {
                "Swamp Mid",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Swamp Front",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Swamp",
                            },
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "IG3S"
                            },
                        }
                    },
                    {
                        "Swamp to Cathedral Main Entrance",
                        new List<List<string>> {
                            new List<string> {
                                "12", "Hyperdash", NO_FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "Swamp Fuse 1", "Swamp Fuse 2",
                                "Swamp Fuse 3", FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "IG2S"
                            },
                            new List<string> {
                                "LS3", PAIRING_ONLY
                            },
                        }
                    },
                    {
                        "Swamp Ledge under Cathedral Door",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Swamp",
                            },
                            new List<string> {
                                "IG3L"
                            }
                        }
                    },
                    {
                        "Back of Swamp",
                        new List<List<string>> {
                            new List<string> {
                                "LS1", PAIRING_ONLY
                            },
                        }
                    },
                }
            },
            {
                "Swamp to Cathedral Main Entrance",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Swamp Mid",
                        new List<List<string>> {
                            new List<string> {
                                "Swamp Fuse 1", "Swamp Fuse 2",
                                "Swamp Fuse 3", FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "IG1S"
                            },
                        }
                    }
                }
            },
            {
                "Swamp Ledge under Cathedral Door",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Swamp Mid",
                        new List<List<string>> {
                            new List<string> {
                                "Ladders in Swamp",
                            },
                            new List<string> {
                                "IG1L"
                            },
                        }
                    },
                    {
                        "Swamp to Cathedral Treasure Room",
                        new List<List<string>> {
                            new List<string> {
                                "21",
                            },
                        }
                    },
                }
            },
            {
                "Swamp to Cathedral Treasure Room",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Swamp Ledge under Cathedral Door",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Back of Swamp",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Back of Swamp Laurels Area",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                    {
                        "Swamp Hero's Grave",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                    {
                        "Swamp Mid",
                        new List<List<string>> {
                            new List<string> {
                                // LS2 includes getting knocked down by melee enemies, so this does not need the pairing only tag
                                "LS2",
                            },
                        }
                    },
                    {
                        "Swamp Front",
                        new List<List<string>> {
                            new List<string> {
                                "LS1", PAIRING_ONLY
                            },
                            new List<string> {
                                "LS2"
                            },
                        }
                    },
                    {
                        "Swamp to Cathedral Main Entrance",
                        new List<List<string>> {
                            new List<string> {
                                "LS3", PAIRING_ONLY
                            },
                        }
                    },
                    {
                        "Swamp to Cathedral Treasure Room",
                        new List<List<string>> {
                            new List<string> {
                                "LS3", PAIRING_ONLY
                            },
                        }
                    },
                }
            },
            {
                "Back of Swamp Laurels Area",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Back of Swamp",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                    {
                        "Swamp Front",
                        new List<List<string>> {
                            new List<string> {
                                "Zip"
                            }
                        }
                    },
                    {
                        "Swamp Mid",
                        new List<List<string>> {
                            new List<string> {
                                "Zip"
                            },
                            new List<string> {
                                "Hyperdash", "IG1L"
                            },
                        }
                    }
                }
            },
            {
                "Swamp Hero's Grave",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Back of Swamp",
                        new List<List<string>> {
                        }
                    },
                }
            },

            {
                "Cathedral",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Cathedral Elevator",
                        new List<List<string>> {
                            new List<string> {
                                "Cathedral Elevator Fuse", FUSE_SHUFFLE
                            },
                            new List<string> {
                                "12", NO_FUSE_SHUFFLE
                            },
                            new List<string> {
                                "IG2S"
                            },
                            new List<string> {
                                "ER on"
                            },
                        }
                    }
                }
            },
            {
                "Cathedral Elevator",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Cathedral",
                        new List<List<string>> {
                        }
                    }
                }
            },

            {
                "Cathedral Gauntlet Checkpoint",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Cathedral Gauntlet",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Cathedral Gauntlet",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Cathedral Gauntlet Exit",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Cathedral Gauntlet Exit",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Cathedral Gauntlet",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },

            {
                "Far Shore",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Far Shore to Spawn",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                    {
                        "Far Shore to East Forest",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                    {
                        "Far Shore to Quarry",
                        new List<List<string>> {
                            new List<string> {
                                "12", "Quarry Connector", "Quarry", "Wand", NO_FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "Quarry Fuse 1", "Quarry Fuse 2", FUSE_SHUFFLE,
                            }
                        }
                    },
                    {
                        "Far Shore to Library",
                        new List<List<string>> {
                            new List<string> {
                                "12", "Library Lab", NO_FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "Library Lab Fuse", FUSE_SHUFFLE,
                            }
                        }
                    },
                    {
                        "Far Shore to West Garden",
                        new List<List<string>> {
                            new List<string> {
                                "12", "West Garden", NO_FUSE_SHUFFLE
                            },
                            new List<string> {
                                "West Garden Fuse", FUSE_SHUFFLE,
                            }
                        }
                    },
                    {
                        "Far Shore to Fortress",
                        new List<List<string>> {
                            new List<string> {  // only left fuses required
                                "12",
                                "Fortress Exterior from Overworld",
                                "Beneath the Vault Back",
                                "Eastern Vault Fortress",
                                NO_FUSE_SHUFFLE,
                            },
                            new List<string> {
                                "Fortress Exterior Fuse 1",
                                "Fortress Exterior Fuse 2",
                                "Beneath the Vault Fuse",
                                "Fortress Candles Fuse",
                                "Fortress Door Left Fuse",
                                FUSE_SHUFFLE,
                            }
                        }
                    },
                }
            },
            {
                "Far Shore to Spawn",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Far Shore",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Far Shore to East Forest",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Far Shore",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Far Shore to Quarry",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Far Shore",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Far Shore to Library",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Far Shore",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Far Shore to West Garden",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Far Shore",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Far Shore to Fortress",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Far Shore",
                        new List<List<string>> {
                        }
                    },
                }
            },

            {
                "Shop Entrance 1",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Shop",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Shop Entrance 2",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Shop",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Shop Entrance 3",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Shop",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Shop Entrance 4",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Shop",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Shop Entrance 5",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Shop",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Shop Entrance 6",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Shop",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Shop Entrance 7",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Shop",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Shop Entrance 8",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Shop",
                        new List<List<string>> {
                        }
                    },
                }
            },
        };

    }
}
