using BepInEx.Logging;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TunicRandomizer {
    public class TunicPortals {
        private static ManualLogSource Logger = TunicRandomizer.Logger;
        public static Dictionary<string, PortalCombo> RandomizedPortals = new Dictionary<string, PortalCombo>();

        public class TunicPortal {
            public string Name;
            public string Destination;
            public string Tag;

            public TunicPortal(string name, string destination, string tag) {
                Name = name;
                Destination = destination;
                Tag = tag;
            }
        }
        
        public class RegionInfo {
            public string Scene;
            public bool DeadEnd;

            public RegionInfo(string scene, bool deadEnd) {
                Scene = scene;
                DeadEnd = deadEnd;
            }
        }

        public static Dictionary<string, Dictionary<string, List<TunicPortal>>> RegionPortalsList = new Dictionary<string, Dictionary<string, List<TunicPortal>>> {
            {
                "Overworld Redux",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Overworld",
                        new List<TunicPortal> {
                            new TunicPortal("Stick House Entrance", "Sword Cave", "_"),
                            new TunicPortal("Windmill Entrance", "Windmill", "_"),
                            new TunicPortal("Well Ladder Entrance", "Sewer", "_entrance"),
                            new TunicPortal("Old House Waterfall Entrance", "Overworld Interiors", "_under_checkpoint"),
                            new TunicPortal("Entrance to Furnace under Windmill", "Furnace", "_gyro_upper_east"),
                            new TunicPortal("Entrance to Furnace from Beach", "Furnace", "_gyro_lower"),
                            new TunicPortal("Caustic Light Cave Entrance", "Overworld Cave", "_"),
                            new TunicPortal("Swamp Lower Entrance", "Swamp Redux 2", "_conduit"),
                            new TunicPortal("Ruined Passage Not-Door Entrance", "Ruins Passage", "_east"),
                            new TunicPortal("Atoll Upper Entrance", "Atoll Redux", "_upper"),
                            new TunicPortal("Atoll Lower Entrance", "Atoll Redux", "_lower"),
                            new TunicPortal("Maze Cave Entrance", "Maze Room", "_"),
                            new TunicPortal("Temple Rafters Entrance", "Temple", "_rafters"),
                            new TunicPortal("Ruined Shop Entrance", "Ruined Shop", "_"),
                            new TunicPortal("Patrol Cave Entrance", "PatrolCave", "_"),
                            new TunicPortal("Hourglass Cave Entrance", "Town Basement", "_beach"),
                            new TunicPortal("Changing Room Entrance", "Changing Room", "_"),
                            new TunicPortal("Cube Cave Entrance", "CubeRoom", "_"),
                            new TunicPortal("Stairs from Overworld to Mountain", "Mountain", "_"),
                            new TunicPortal("Overworld to Fortress", "Fortress Courtyard", "_"),
                            new TunicPortal("Overworld to Quarry Connector", "Darkwoods Tunnel", "_"),
                            new TunicPortal("Dark Tomb Main Entrance", "Crypt Redux", "_"),
                            new TunicPortal("Overworld to Forest Belltower", "Forest Belltower", "_"),
                            new TunicPortal("Secret Gathering Place Entrance", "Waterfall", "_"),
                        }
                    },
                    {
                        "Overworld Well to Furnace Rail",
                        new List<TunicPortal> {
                            new TunicPortal("Entrance to Well from Well Rail", "Sewer", "_west_aqueduct"),
                            new TunicPortal("Entrance to Furnace from Well Rail", "Furnace", "_gyro_upper_north"),
                        }
                    },
                    {
                        "Overworld Old House Door",
                        new List<TunicPortal> {
                            new TunicPortal("Old House Door Entrance", "Overworld Interiors", "_house"),
                        }
                    },
                    {
                        "Overworld to West Garden from Furnace",
                        new List<TunicPortal> {
                            new TunicPortal("Entrance to Furnace near West Garden", "Furnace", "_gyro_west"),
                            new TunicPortal("West Garden Entrance from Furnace", "Archipelagos Redux", "_lower"),
                        }
                    },
                    {
                        "Overworld Swamp Upper Entry",
                        new List<TunicPortal> {
                            new TunicPortal("Swamp Upper Entrance", "Swamp Redux 2", "_wall"),
                        }
                    },
                    {
                        "Overworld Ruined Passage Door",
                        new List<TunicPortal> {
                            new TunicPortal("Ruined Passage Door Entrance", "Ruins Passage", "_west"),
                        }
                    },
                    {
                        "Overworld Special Shop Entry",
                        new List<TunicPortal> {
                            new TunicPortal("Special Shop Entrance", "ShopSpecial", "_"),
                        }
                    },
                    {
                        "Overworld Belltower",
                        new List<TunicPortal> {
                            new TunicPortal("West Garden Entrance near Belltower", "Archipelagos Redux", "_upper"),
                        }
                    },
                    {
                        "Overworld West Garden Laurels Entry",
                        new List<TunicPortal> {
                            new TunicPortal("West Garden Laurels Entrance", "Archipelagos Redux", "_lowest"),
                        }
                    },
                    {
                        "Overworld Temple Door",
                        new List<TunicPortal> {
                            new TunicPortal("Temple Door Entrance", "Temple", "_main"),
                        }
                    },
                    {
                        "Overworld Fountain Cross Door",
                        new List<TunicPortal> {
                            new TunicPortal("Fountain HC Door Entrance", "Town_FiligreeRoom", "_"),
                        }
                    },
                    {
                        "Overworld Southeast Cross Door",
                        new List<TunicPortal> {
                            new TunicPortal("Southeast HC Door Entrance", "EastFiligreeCache", "_"),
                        }
                    },
                    {
                        "Overworld Town Portal",
                        new List<TunicPortal> {
                            new TunicPortal("Town to Far Shore", "Transit", "_teleporter_town"),
                        }
                    },
                    {
                        "Overworld Spawn Portal",
                        new List<TunicPortal> {
                            new TunicPortal("Spawn to Far Shore", "Transit", "_teleporter_starting island"),
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
                            new TunicPortal("Secret Gathering Place Exit", "Overworld Redux", "_"),
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
                            new TunicPortal("Windmill Exit", "Overworld Redux", "_"),
                            new TunicPortal("Windmill Shop", "Shop", "_"),
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
                            new TunicPortal("Old House Door Exit", "Overworld Redux", "_house"),
                            new TunicPortal("Old House to Glyph Tower", "g_elements", "_"),
                        }
                    },
                    {
                        "Old House Back",
                        new List<TunicPortal> {
                            new TunicPortal("Old House Waterfall Exit", "Overworld Redux", "_under_checkpoint"),
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
                            new TunicPortal("Glyph Tower Exit", "Overworld Interiors", "_"),
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
                            new TunicPortal("Changing Room Exit", "Overworld Redux", "_"),
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
                            new TunicPortal("Fountain HC Room Exit", "Overworld Redux", "_"),
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
                            new TunicPortal("Cube Cave Exit", "Overworld Redux", "_"),
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
                            new TunicPortal("Guard Patrol Cave Exit", "Overworld Redux", "_"),
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
                            new TunicPortal("Ruined Shop Exit", "Overworld Redux", "_"),
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
                            new TunicPortal("Furnace Exit towards Well", "Overworld Redux", "_gyro_upper_north"),
                        }
                    },
                    {
                        "Furnace Walking Path",
                        new List<TunicPortal> {
                            new TunicPortal("Furnace Exit to Dark Tomb", "Crypt Redux", "_"),
                            new TunicPortal("Furnace Exit towards West Garden", "Overworld Redux", "_gyro_west"),
                        }
                    },
                    {
                        "Furnace Ladder Area",
                        new List<TunicPortal> {
                            new TunicPortal("Furnace Exit to Beach", "Overworld Redux", "_gyro_lower"),
                            new TunicPortal("Furnace Exit under Windmill", "Overworld Redux", "_gyro_upper_east"),
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
                            new TunicPortal("Stick House Exit", "Overworld Redux", "_"),
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
                            new TunicPortal("Ruined Passage Not-Door Exit", "Overworld Redux", "_east"),
                            new TunicPortal("Ruined Passage Door Exit", "Overworld Redux", "_west"),
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
                            new TunicPortal("Southeast HC Room Exit", "Overworld Redux", "_"),
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
                            new TunicPortal("Caustic Light Cave Exit", "Overworld Redux", "_"),
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
                            new TunicPortal("Maze Cave Exit", "Overworld Redux", "_"),
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
                            new TunicPortal("Hourglass Cave Exit", "Overworld Redux", "_beach"),
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
                            new TunicPortal("Special Shop Exit", "Overworld Redux", "_"),
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
                            new TunicPortal("Temple Rafters Exit", "Overworld Redux", "_rafters"),
                        }
                    },
                    {
                        "Sealed Temple",
                        new List<TunicPortal> {
                            new TunicPortal("Temple Door Exit", "Overworld Redux", "_main"),
                        }
                    },
                }
            },
            {
                "Sewer",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Beneath the Well Front",
                        new List<TunicPortal> {
                            new TunicPortal("Well Ladder Exit", "Overworld Redux", "_entrance"),
                        }
                    },
                    {
                        "Beneath the Well Back",
                        new List<TunicPortal> {
                            new TunicPortal("Well to Well Boss", "Sewer_Boss", "_"),
                            new TunicPortal("Well Exit towards Furnace", "Overworld Redux", "_west_aqueduct"),
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
                            new TunicPortal("Well Boss to Well", "Sewer", "_"),
                        }
                    },
                    {
                        "Dark Tomb Checkpoint",
                        new List<TunicPortal> {
                            new TunicPortal("Checkpoint to Dark Tomb", "Crypt Redux", "_"),
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
                            new TunicPortal("Dark Tomb to Overworld", "Overworld Redux", "_"),
                            new TunicPortal("Dark Tomb to Checkpoint", "Sewer_Boss", "_"),
                        }
                    },
                    {
                        "Dark Tomb Dark Exit",
                        new List<TunicPortal> {
                            new TunicPortal("Dark Tomb to Furnace", "Furnace", "_"),
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
                            new TunicPortal("West Garden Exit near Hero's Grave", "Overworld Redux", "_lower"),
                            new TunicPortal("West Garden to Magic Dagger House", "archipelagos_house", "_"),
                            new TunicPortal("West Garden Shop", "Shop", "_"),
                        }
                    },
                    {
                        "West Garden after Boss",
                        new List<TunicPortal> {
                            new TunicPortal("West Garden Exit after Boss", "Overworld Redux", "_upper"),
                        }
                    },
                    {
                        "West Garden Laurels Exit",
                        new List<TunicPortal> {
                            new TunicPortal("West Garden Laurels Exit", "Overworld Redux", "_lowest"),
                        }
                    },
                    {
                        "West Garden Hero's Grave",
                        new List<TunicPortal> {
                            new TunicPortal("West Garden Hero's Grave", "RelicVoid", "_teleporter_relic plinth"),
                        }
                    },
                    {
                        "West Garden Portal",
                        new List<TunicPortal> {
                            new TunicPortal("West Garden to Far Shore", "Transit", "_teleporter_archipelagos_teleporter"),
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
                            new TunicPortal("Magic Dagger House Exit", "Archipelagos Redux", "_"),
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
                            new TunicPortal("Atoll Upper Exit", "Overworld Redux", "_upper"),
                            new TunicPortal("Atoll Shop", "Shop", "_"),
                            new TunicPortal("Frog Stairs Eye Entrance", "Frog Stairs", "_eye"),
                        }
                    },
                    {
                        "Ruined Atoll Lower Entry Area",
                        new List<TunicPortal> {
                            new TunicPortal("Atoll Lower Exit", "Overworld Redux", "_lower"),
                        }
                    },
                    {
                        "Ruined Atoll Portal",
                        new List<TunicPortal> {
                            new TunicPortal("Atoll to Far Shore", "Transit", "_teleporter_atoll"),
                        }
                    },
                    {
                        "Ruined Atoll Statue",
                        new List<TunicPortal> {
                            new TunicPortal("Atoll Statue Teleporter", "Library Exterior", "_"),
                        }
                    },
                    {
                        "Ruined Atoll Frog Mouth",
                        new List<TunicPortal> {
                            new TunicPortal("Frog Stairs Mouth Entrance", "Frog Stairs", "_mouth"),
                        }
                    },
                }
            },
            {
                "Frog Stairs",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Frog's Domain Entry",
                        new List<TunicPortal> {
                            new TunicPortal("Frog Stairs Eye Exit", "Atoll Redux", "_eye"),
                            new TunicPortal("Frog Stairs Mouth Exit", "Atoll Redux", "_mouth"),
                            new TunicPortal("Frog Stairs to Frog's Domain's Entrance", "frog cave main", "_Entrance"),
                            new TunicPortal("Frog Stairs to Frog's Domain's Exit", "frog cave main", "_Exit"),
                        }
                    },
                }
            },
            {
                "frog cave main",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Frog's Domain",
                        new List<TunicPortal> {
                            new TunicPortal("Frog's Domain Ladder Exit", "Frog Stairs", "_Entrance"),
                        }
                    },
                    {
                        "Frog's Domain Back",
                        new List<TunicPortal> {
                            new TunicPortal("Frog's Domain Orb Exit", "Frog Stairs", "_Exit"),
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
                            new TunicPortal("Library Exterior Tree", "Atoll Redux", "_"),
                        }
                    },
                    {
                        "Library Exterior Ladder",
                        new List<TunicPortal> {
                            new TunicPortal("Library Exterior Ladder", "Library Hall", "_"),
                        }
                    },
                }
            },
            {
                "Library Hall",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Library Hall",
                        new List<TunicPortal> {
                            new TunicPortal("Library Hall Bookshelf Exit", "Library Exterior", "_"),
                            new TunicPortal("Library Hall to Rotunda", "Library Rotunda", "_"),
                        }
                    },
                    {
                        "Library Hero's Grave",
                        new List<TunicPortal> {
                            new TunicPortal("Library Hero's Grave", "RelicVoid", "_teleporter_relic plinth"),
                        }
                    },
                }
            },
            {
                "Library Rotunda",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Library Rotunda",
                        new List<TunicPortal> {
                            new TunicPortal("Library Rotunda Lower Exit", "Library Hall", "_"),
                            new TunicPortal("Library Rotunda Upper Exit", "Library Lab", "_"),
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
                            new TunicPortal("Library Lab to Rotunda", "Library Rotunda", "_"),
                        }
                    },
                    {
                        "Library Portal",
                        new List<TunicPortal> {
                            new TunicPortal("Library to Far Shore", "Transit", "_teleporter_library teleporter"),
                        }
                    },
                    {
                        "Library Lab",
                        new List<TunicPortal> {
                            new TunicPortal("Library Lab to Librarian Arena", "Library Arena", "_"),
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
                            new TunicPortal("Librarian Arena Exit", "Library Lab", "_"),
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
                            new TunicPortal("Forest to Belltower", "Forest Belltower", "_"),
                            new TunicPortal("Forest Guard House 1 Lower Entrance", "East Forest Redux Laddercave", "_lower"),
                            new TunicPortal("Forest Guard House 1 Gate Entrance", "East Forest Redux Laddercave", "_gate"),
                            new TunicPortal("Forest Guard House 2 Lower Entrance", "East Forest Redux Interior", "_lower"),
                            new TunicPortal("Forest Guard House 2 Upper Entrance", "East Forest Redux Interior", "_upper"),
                            new TunicPortal("Forest Grave Path Lower Entrance", "Sword Access", "_lower"),
                            new TunicPortal("Forest Grave Path Upper Entrance", "Sword Access", "_upper"),
                        }
                    },
                    {
                        "East Forest Dance Fox Spot",
                        new List<TunicPortal> {
                            new TunicPortal("Forest Dance Fox Outside Doorway", "East Forest Redux Laddercave", "_upper"),
                        }
                    },
                    {
                        "East Forest Portal",
                        new List<TunicPortal> {
                            new TunicPortal("Forest to Far Shore", "Transit", "_teleporter_forest teleporter"),
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
                            new TunicPortal("Guard House 1 Dance Fox Exit", "East Forest Redux", "_upper"),
                            new TunicPortal("Guard House 1 Lower Exit", "East Forest Redux", "_lower"),
                        }
                    },
                    {
                        "Guard House 1 East",
                        new List<TunicPortal> {
                            new TunicPortal("Guard House 1 Upper Forest Exit", "East Forest Redux", "_gate"),
                            new TunicPortal("Guard House 1 to Guard Captain Room", "Forest Boss Room", "_"),
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
                            new TunicPortal("Forest Grave Path Upper Exit", "East Forest Redux", "_upper"),
                        }
                    },
                    {
                        "Forest Grave Path Main",
                        new List<TunicPortal> {
                            new TunicPortal("Forest Grave Path Lower Exit", "East Forest Redux", "_lower"),
                        }
                    },
                    {
                        "Forest Hero's Grave",
                        new List<TunicPortal> {
                            new TunicPortal("East Forest Hero's Grave", "RelicVoid", "_teleporter_relic plinth"),
                        }
                    },
                }
            },
            {
                "East Forest Redux Interior",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Guard House 2",
                        new List<TunicPortal> {
                            new TunicPortal("Guard House 2 Lower Exit", "East Forest Redux", "_lower"),
                            new TunicPortal("Guard House 2 Upper Exit", "East Forest Redux", "_upper"),
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
                            new TunicPortal("Guard Captain Room Non-Gate Exit", "East Forest Redux Laddercave", "_"),
                            new TunicPortal("Guard Captain Room Gate Exit", "Forest Belltower", "_"),
                        }
                    },
                }
            },
            {
                "Forest Belltower",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Forest Belltower Main",
                        new List<TunicPortal> {
                            new TunicPortal("Forest Belltower to Fortress", "Fortress Courtyard", "_"),
                            new TunicPortal("Forest Belltower to Overworld", "Overworld Redux", "_"),
                        }
                    },
                    {
                        "Forest Belltower Lower",
                        new List<TunicPortal> {
                            new TunicPortal("Forest Belltower to Forest", "East Forest Redux", "_"),
                        }
                    },
                    {
                        "Forest Belltower Upper",
                        new List<TunicPortal> {
                            new TunicPortal("Forest Belltower to Guard Captain Room", "Forest Boss Room", "_"),
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
                            new TunicPortal("Fortress Courtyard to Fortress Grave Path Lower", "Fortress Reliquary", "_Lower"),
                            new TunicPortal("Fortress Courtyard to Fortress Interior", "Fortress Main", "_Big Door"),
                        }
                    },
                    {
                        "Fortress Courtyard Upper",
                        new List<TunicPortal> {
                            new TunicPortal("Fortress Courtyard to Fortress Grave Path Upper", "Fortress Reliquary", "_Upper"),
                            new TunicPortal("Fortress Courtyard to East Fortress", "Fortress East", "_"),
                        }
                    },
                    {
                        "Fortress Exterior near cave",
                        new List<TunicPortal> {
                            new TunicPortal("Fortress Courtyard to Beneath the Earth", "Fortress Basement", "_"),
                            new TunicPortal("Fortress Courtyard Shop", "Shop", "_"),
                        }
                    },
                    {
                        "Fortress Exterior from East Forest",
                        new List<TunicPortal> {
                            new TunicPortal("Fortress Courtyard to Forest Belltower", "Forest Belltower", "_"),
                        }
                    },
                    {
                        "Fortress Exterior from Overworld",
                        new List<TunicPortal> {
                            new TunicPortal("Fortress Courtyard to Overworld", "Overworld Redux", "_"),
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
                            new TunicPortal("Beneath the Earth to Fortress Interior", "Fortress Main", "_"),
                        }
                    },
                    {
                        "Beneath the Vault Front",
                        new List<TunicPortal> {
                            new TunicPortal("Beneath the Earth to Fortress Courtyard", "Fortress Courtyard", "_"),
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
                            new TunicPortal("Fortress Interior Main Exit", "Fortress Courtyard", "_Big Door"),
                            new TunicPortal("Fortress Interior to Beneath the Earth", "Fortress Basement", "_"),
                            new TunicPortal("Fortress Interior Shop", "Shop", "_"),
                            new TunicPortal("Fortress Interior to East Fortress Upper", "Fortress East", "_upper"),
                            new TunicPortal("Fortress Interior to East Fortress Lower", "Fortress East", "_lower"),
                        }
                    },
                    {
                        "Eastern Vault Fortress Gold Door",
                        new List<TunicPortal> {
                            new TunicPortal("Fortress Interior to Siege Engine Arena", "Fortress Arena", "_"),
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
                            new TunicPortal("East Fortress to Interior Lower", "Fortress Main", "_lower"),
                        }
                    },
                    {
                        "Fortress East Shortcut Upper",
                        new List<TunicPortal> {
                            new TunicPortal("East Fortress to Courtyard", "Fortress Courtyard", "_"),
                            new TunicPortal("East Fortress to Interior Upper", "Fortress Main", "_upper"),
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
                            new TunicPortal("Fortress Grave Path Lower Exit", "Fortress Courtyard", "_Lower"),
                            new TunicPortal("Fortress Hero's Grave", "RelicVoid", "_teleporter_relic plinth"),
                        }
                    },
                    {
                        "Fortress Grave Path Upper",
                        new List<TunicPortal> {
                            new TunicPortal("Fortress Grave Path Upper Exit", "Fortress Courtyard", "_Upper"),
                        }
                    },
                    {
                        "Fortress Grave Path Dusty Entrance",
                        new List<TunicPortal> {
                            new TunicPortal("Fortress Grave Path Dusty Entrance", "Dusty", "_"),
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
                            new TunicPortal("Dusty Exit", "Fortress Reliquary", "_"),
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
                            new TunicPortal("Siege Engine Arena to Fortress", "Fortress Main", "_"),
                        }
                    },
                    {
                        "Fortress Arena Portal",
                        new List<TunicPortal> {
                            new TunicPortal("Fortress to Far Shore", "Transit", "_teleporter_spidertank"),
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
                            new TunicPortal("Stairs to Top of the Mountain", "Mountaintop", "_"),
                        }
                    },
                    {
                        "Lower Mountain",
                        new List<TunicPortal> {
                            new TunicPortal("Mountain to Quarry", "Quarry Redux", "_"),
                            new TunicPortal("Mountain to Overworld", "Overworld Redux", "_"),
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
                            new TunicPortal("Top of the Mountain Exit", "Mountain", "_"),
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
                            new TunicPortal("Quarry Connector to Overworld", "Overworld Redux", "_"),
                            new TunicPortal("Quarry Connector to Quarry", "Quarry Redux", "_"),
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
                            new TunicPortal("Quarry to Overworld Exit", "Darkwoods Tunnel", "_"),
                            new TunicPortal("Quarry Shop", "Shop", "_"),
                        }
                    },
                    {
                        "Quarry Monastery Entry",
                        new List<TunicPortal> {
                            new TunicPortal("Quarry to Monastery Front", "Monastery", "_front"),
                        }
                    },
                    {
                        "Monastery Rope",
                        new List<TunicPortal> {
                            new TunicPortal("Quarry to Monastery Back", "Monastery", "_back"),
                        }
                    },
                    {
                        "Quarry Back",
                        new List<TunicPortal> {
                            new TunicPortal("Quarry to Mountain", "Mountain", "_"),
                        }
                    },
                    {
                        "Lower Quarry Zig Door",
                        new List<TunicPortal> {
                            new TunicPortal("Quarry to Ziggurat", "ziggurat2020_0", "_"),
                        }
                    },
                    {
                        "Quarry Portal",
                        new List<TunicPortal> {
                            new TunicPortal("Quarry to Far Shore", "Transit", "_teleporter_quarry teleporter"),
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
                            new TunicPortal("Monastery Rear Exit", "Quarry Redux", "_back"),
                        }
                    },
                    {
                        "Monastery Front",
                        new List<TunicPortal> {
                            new TunicPortal("Monastery Front Exit", "Quarry Redux", "_front"),
                        }
                    },
                    {
                        "Monastery Hero's Grave",
                        new List<TunicPortal> {
                            new TunicPortal("Monastery Hero's Grave", "RelicVoid", "_teleporter_relic plinth"),
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
                            new TunicPortal("Ziggurat Entry Hallway to Ziggurat Upper", "ziggurat2020_1", "_"),
                            new TunicPortal("Ziggurat Entry Hallway to Quarry", "Quarry Redux", "_"),
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
                            new TunicPortal("Ziggurat Upper to Ziggurat Entry Hallway", "ziggurat2020_0", "_"),
                        }
                    },
                    {
                        "Rooted Ziggurat Upper Back",
                        new List<TunicPortal> {
                            new TunicPortal("Ziggurat Upper to Ziggurat Tower", "ziggurat2020_2", "_"),
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
                            new TunicPortal("Ziggurat Tower to Ziggurat Upper", "ziggurat2020_1", "_"),
                        }
                    },
                    {
                        "Rooted Ziggurat Middle Bottom",
                        new List<TunicPortal> {
                            new TunicPortal("Ziggurat Tower to Ziggurat Lower", "ziggurat2020_3", "_"),
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
                            new TunicPortal("Ziggurat Lower to Ziggurat Tower", "ziggurat2020_2", "_"),
                        }
                    },
                    {
                        "Rooted Ziggurat Portal Room Entrance",
                        new List<TunicPortal> {
                            new TunicPortal("Ziggurat Portal Room Entrance", "ziggurat2020_FTRoom", "_"),
                        }
                    },
                    {
                        "Zig Skip Exit",
                        new List<TunicPortal> {
                            new TunicPortal("Ziggurat Lower Falling Entrance", "ziggurat2020_1", "_zig2_skip"),
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
                            new TunicPortal("Ziggurat Portal Room Exit", "ziggurat2020_3", "_"),
                        }
                    },
                    {
                        "Rooted Ziggurat Portal",
                        new List<TunicPortal> {
                            new TunicPortal("Ziggurat to Far Shore", "Transit", "_teleporter_ziggurat teleporter"),
                        }
                    },
                }
            },
            {
                "Swamp Redux 2",
                new Dictionary<string, List<TunicPortal>> {
                    {
                        "Swamp",
                        new List<TunicPortal> {
                            new TunicPortal("Swamp Lower Exit", "Overworld Redux", "_conduit"),
                            new TunicPortal("Swamp Shop", "Shop", "_"),
                        }
                    },
                    {
                        "Swamp to Cathedral Main Entrance",
                        new List<TunicPortal> {
                            new TunicPortal("Swamp to Cathedral Main Entrance", "Cathedral Redux", "_main"),
                        }
                    },
                    {
                        "Swamp to Cathedral Treasure Room",
                        new List<TunicPortal> {
                            new TunicPortal("Swamp to Cathedral Secret Legend Room Entrance", "Cathedral Redux", "_secret"),
                        }
                    },
                    {
                        "Back of Swamp",
                        new List<TunicPortal> {
                            new TunicPortal("Swamp to Gauntlet", "Cathedral Arena", "_"),
                        }
                    },
                    {
                        "Back of Swamp Laurels Area",
                        new List<TunicPortal> {
                            new TunicPortal("Swamp Upper Exit", "Overworld Redux", "_wall"),
                        }
                    },
                    {
                        "Swamp Hero's Grave",
                        new List<TunicPortal> {
                            new TunicPortal("Swamp Hero's Grave", "RelicVoid", "_teleporter_relic plinth"),
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
                            new TunicPortal("Cathedral Main Exit", "Swamp Redux 2", "_main"),
                            new TunicPortal("Cathedral Elevator", "Cathedral Arena", "_"),
                        }
                    },
                    {
                        "Cathedral Secret Legend Room",
                        new List<TunicPortal> {
                            new TunicPortal("Cathedral Secret Legend Room Exit", "Swamp Redux 2", "_secret"),
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
                            new TunicPortal("Gauntlet to Swamp", "Swamp Redux 2", "_"),
                        }
                    },
                    {
                        "Cathedral Gauntlet Checkpoint",
                        new List<TunicPortal> {
                            new TunicPortal("Gauntlet Elevator", "Cathedral Redux", "_"),
                            new TunicPortal("Gauntlet Shop", "Shop", "_"),
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
                            new TunicPortal("Hero's Grave to Fortress", "Fortress Reliquary", "_teleporter_relic plinth"),
                        }
                    },
                    {
                        "Hero Relic - Quarry",
                        new List<TunicPortal> {
                            new TunicPortal("Hero's Grave to Monastery", "Monastery", "_teleporter_relic plinth"),
                        }
                    },
                    {
                        "Hero Relic - West Garden",
                        new List<TunicPortal> {
                            new TunicPortal("Hero's Grave to West Garden", "Archipelagos Redux", "_teleporter_relic plinth"),
                        }
                    },
                    {
                        "Hero Relic - East Forest",
                        new List<TunicPortal> {
                            new TunicPortal("Hero's Grave to East Forest", "Sword Access", "_teleporter_relic plinth"),
                        }
                    },
                    {
                        "Hero Relic - Library",
                        new List<TunicPortal> {
                            new TunicPortal("Hero's Grave to Library", "Library Hall", "_teleporter_relic plinth"),
                        }
                    },
                    {
                        "Hero Relic - Swamp",
                        new List<TunicPortal> {
                            new TunicPortal("Hero's Grave to Swamp", "Swamp Redux 2", "_teleporter_relic plinth"),
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
                            new TunicPortal("Far Shore to West Garden", "Archipelagos Redux", "_teleporter_archipelagos_teleporter"),
                        }
                    },
                    {
                        "Far Shore to Library",
                        new List<TunicPortal> {
                            new TunicPortal("Far Shore to Library", "Library Lab", "_teleporter_library teleporter"),
                        }
                    },
                    {
                        "Far Shore to Quarry",
                        new List<TunicPortal> {
                            new TunicPortal("Far Shore to Quarry", "Quarry Redux", "_teleporter_quarry teleporter"),
                        }
                    },
                    {
                        "Far Shore to East Forest",
                        new List<TunicPortal> {
                            new TunicPortal("Far Shore to East Forest", "East Forest Redux", "_teleporter_forest teleporter"),
                        }
                    },
                    {
                        "Far Shore to Fortress",
                        new List<TunicPortal> {
                            new TunicPortal("Far Shore to Fortress", "Fortress Arena", "_teleporter_spidertank"),
                        }
                    },
                    {
                        "Far Shore",
                        new List<TunicPortal> {
                            new TunicPortal("Far Shore to Atoll", "Atoll Redux", "_teleporter_atoll"),
                            new TunicPortal("Far Shore to Ziggurat", "ziggurat2020_FTRoom", "_teleporter_ziggurat teleporter"),
                            new TunicPortal("Far Shore to Heir", "Spirit Arena", "_teleporter_spirit arena"),
                            new TunicPortal("Far Shore to Town", "Overworld Redux", "_teleporter_town"),
                        }
                    },
                    {
                        "Far Shore to Spawn",
                        new List<TunicPortal> {
                            new TunicPortal("Far Shore to Spawn", "Overworld Redux", "_teleporter_starting island"),
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
                            new TunicPortal("Heir Arena Exit", "Transit", "_teleporter_spirit arena"),
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
                            new TunicPortal("Purgatory Bottom Exit", "Purgatory", "_bottom"),
                            new TunicPortal("Purgatory Top Exit", "Purgatory", "_top"),
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
                            new TunicPortal("Shop", "Previous Region", "_"),  // "Previous Region" is just a placeholder
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
                "Overworld Swamp Upper Entry",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Special Shop Entry",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld West Garden Laurels Entry",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld to West Garden from Furnace",
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
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Temple Door",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Town Portal",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Spawn Portal",
                new RegionInfo("Overworld Redux", false)
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
                "Guard House 2",
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
                new RegionInfo("Sword Access", false)
            },
            {
                "Dark Tomb Entry Point",
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
                new RegionInfo("Archipelagos Redux", false)
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
                "Ruined Atoll Frog Mouth",
                new RegionInfo("Atoll Redux", false)
            },
            {
                "Ruined Atoll Portal",
                new RegionInfo("Atoll Redux", false)
            },
            {
                "Ruined Atoll Statue",
                new RegionInfo("Atoll Redux", false)
            },
            {
                "Frog's Domain Entry",
                new RegionInfo("Frog Stairs", false)
            },
            {
                "Frog's Domain",
                new RegionInfo("frog cave main", false)
            },
            {
                "Frog's Domain Back",
                new RegionInfo("frog cave main", false)
            },
            {
                "Library Exterior Tree",
                new RegionInfo("Library Exterior", false)
            },
            {
                "Library Exterior Ladder",
                new RegionInfo("Library Exterior", false)
            },
            {
                "Library Hall",
                new RegionInfo("Library Hall", false)
            },
            {
                "Library Hero's Grave",
                new RegionInfo("Library Hall", false)
            },
            {
                "Library Rotunda",
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
                "Library Portal",
                new RegionInfo("Library Lab", false)
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
                "Fortress Courtyard",
                new RegionInfo("Fortress Courtyard", false)
            },
            {
                "Fortress Courtyard Upper",
                new RegionInfo("Fortress Courtyard", false)
            },
            {
                "Beneath the Vault Front",
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
                new RegionInfo("Fortress Reliquary", false)
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
                new RegionInfo("Fortress Arena", false)
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
                new RegionInfo("Quarry Redux", false)
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
                new RegionInfo("Monastery", false)
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
                "Rooted Ziggurat Lower Back",
                new RegionInfo("ziggurat2020_3", false)
            },
            {
                "Zig Skip Exit",
                new RegionInfo("ziggurat2020_3", false)
            },
            {
                "Rooted Ziggurat Portal Room Entrance",
                new RegionInfo("ziggurat2020_3", false)
            },
            {
                "Rooted Ziggurat Portal",
                new RegionInfo("ziggurat2020_FTRoom", false)
            },
            {
                "Rooted Ziggurat Portal Room Exit",
                new RegionInfo("ziggurat2020_FTRoom", false)
            },
            {
                "Swamp",
                new RegionInfo("Swamp Redux 2", false)
            },
            {
                "Swamp to Cathedral Treasure Room",
                new RegionInfo("Swamp Redux 2", false)
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
                new RegionInfo("Swamp Redux 2", false)
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
                new RegionInfo("Transit", false)
            },
            {
                "Far Shore to Fortress",
                new RegionInfo("Transit", false)
            },
            {
                "Far Shore to Library",
                new RegionInfo("Transit", false)
            },
            {
                "Far Shore to West Garden",
                new RegionInfo("Transit", false)
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
        };

        public static Dictionary<string, Dictionary<string, List<List<string>>>> TraversalReqs = new Dictionary<string, Dictionary<string, List<List<string>>>> {
            {
                "Overworld",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Overworld Belltower",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Ladder Storage",
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
                                "Ladder Storage",
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
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Overworld West Garden Laurels Entry",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Overworld Southeast Cross Door",
                        new List<List<string>> {
                            new List<string> {
                                "21",
                            },
                        }
                    },
                    {
                        "Overworld Ruined Passage Door",
                        new List<List<string>> {
                            new List<string> {
                                "Key",
                            },
                            new List<string> {
                                "Hyperdash", "nmg",
                            },
                        }
                    },
                    {
                        "Overworld Temple Door",
                        new List<List<string>> {
                            new List<string> {
                                "26", "Techbow", "Wand", "Stundagger", "nmg",
                            },
                            new List<string> {
                                "Techbow", "Forest Belltower Upper", "nmg",
                            },
                            new List<string> {
                                "Stick", "Forest Belltower Upper", "Overworld Belltower",
                            },
                            new List<string> {
                                "Techbow", "Forest Belltower Upper", "Overworld Belltower",
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
                                "26", "Techbow", "Wand", "Stundagger", "nmg",
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
                        "Overworld Well to Furnace Rail",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder Storage",
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
                                "Stundagger", "Wand", "nmg",
                            },
                        }
                    },
                }
            },
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
                                "Hyperdash", "nmg",
                            },
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
                                "Ladder Storage",
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
                                "Ladder Storage",
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
                "East Forest",
                new Dictionary<string, List<List<string>>> {
                    {
                        "East Forest Dance Fox Spot",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "26", "nmg",
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
                                "26", "nmg",
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
                                "26", "nmg",
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
                                "Hyperdash", "nmg",
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
                "Beneath the Well Front",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Beneath the Well Main",
                        new List<List<string>> {
                            new List<string> {
                                "Stick", "Lantern"
                            },
                            new List<string> {
                                "Techbow", "Lantern"
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
                                "Stick"
                            },
                            new List<string> {
                                "Techbow"
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
                                "Hyperdash", "nmg",
                            },
                        }
                    },
                }
            },
            {
                "Dark Tomb Entry Point",
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
                "Dark Tomb Main",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Dark Tomb Dark Exit",
                        new List<List<string>> {
                        }
                    },
                    {
                        "Dark Tomb Entry Point",
                        new List<List<string>> {
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
                                "Ladder Storage"
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
                                "Ladder Storage"
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
                                "26", "Wand", "Stundagger", "Techbow", "nmg"
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
                        }
                    },
                }
            },
            {
                "West Garden Portal Item",
                new Dictionary<string, List<List<string>>> {
                    {
                        "West Garden",
                        new List<List<string>> {
                            new List<string> {
                                "26", "Wand", "Stundagger", "Techbow", "nmg"
                            },
                        }
                    },
                    {
                        "West Garden Portal",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash", "12", "West Garden"
                            },
                        }
                    },
                }
            },
            {
                "West Garden Portal",
                new Dictionary<string, List<List<string>>> {
                    {
                        "West Garden Portal Item",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash"
                            },
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
                                "Ladder Storage",
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
                            new List<string> {
                                "Ladder Storage",
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
                                "12",
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
                "Frog's Domain",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Frog's Domain Back",
                        new List<List<string>> {
                            new List<string> {
                                "Wand",
                            },
                        }
                    },
                }
            },
            {
                "Library Exterior Ladder",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Exterior Tree",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash", "12",
                            },
                            new List<string> {
                                "Wand", "12",
                            },
                        }
                    },
                }
            },
            {
                "Library Exterior Tree",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Exterior Ladder",
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
                "Library Hall",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Hero's Grave",
                        new List<List<string>> {
                            new List<string> {
                                "12",
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
                "Library Lab Lower",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Lab",
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
                "Library Lab",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Lab Lower",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                    {
                        "Library Portal",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                }
            },
            {
                "Library Portal",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Lab",
                        new List<List<string>> {
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
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Fortress Courtyard Upper",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Fortress Exterior near cave",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Fortress Courtyard",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder Storage",
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
                                "12",
                            },
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Ladder Storage",
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
                                "26", "Techbow", "Wand", "Stundagger", "nmg",
                            },
                            new List<string> {
                                "Ladder Storage",
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
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Fortress Courtyard",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Fortress Courtyard Upper",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                }
            },
            {
                "Fortress Courtyard",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Courtyard Upper",
                        new List<List<string>> {
                            new List<string> {
                                "26", "Techbow", "Wand", "Stundagger", "nmg",
                            },
                        }
                    },
                    {
                        "Fortress Exterior from Overworld",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
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
                "Beneath the Vault Front",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Beneath the Vault Back",
                        new List<List<string>> {
                            new List<string> {
                                "Lantern",
                            },
                        }
                    },
                }
            },
            {
                "Beneath the Vault Back",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Beneath the Vault Front",
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
                                "26", "Techbow", "Wand", "Stundagger", "nmg",
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
                "Eastern Vault Fortress",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Eastern Vault Fortress Gold Door",
                        new List<List<string>> {
                            new List<string> {
                                "Wand", "Stundagger", "nmg",
                            },
                            new List<string> {
                                "12", "Fortress Exterior from Overworld", "Beneath the Vault Back", "Fortress Courtyard Upper",
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
                                "Wand", "Stundagger", "nmg",
                            },
                        }
                    },
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
                                "26", "Stundagger", "Techbow", "Wand", "nmg",
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
                    {
                        "Fortress Arena Portal",
                        new List<List<string>> {
                            new List<string> {
                                "12", "Fortress Exterior from Overworld", "Beneath the Vault Back", "Eastern Vault Fortress",
                            },
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
                "Monastery Back",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Monastery Front",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash", "nmg",
                            },
                        }
                    },
                    {
                        "Monastery Hero's Grave",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
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
                "Monastery Front",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Monastery Back",
                        new List<List<string>> {
                            new List<string> {
                                "Sword"
                            },
                            new List<string> {
                                "Techbow"
                            }
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
                                "12", "Quarry Connector", "Wand"
                            },
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
                                "Ladder Storage",
                            },
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
                }
            },
            {
                "Lower Quarry",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Lower Quarry Zig Door",
                        new List<List<string>> {
                            new List<string> {
                                "Quarry", "Quarry Connector", "Wand"
                            }
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
                                "Sword", "12",
                            },
                        }
                    },
                }
            },
            {
                "Rooted Ziggurat Lower Back",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Rooted Ziggurat Lower Front",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash", "Sword", "12"
                            },
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Rooted Ziggurat Portal Room Entrance",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                }
            },
            {
                "Zig Skip Exit",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Rooted Ziggurat Lower Front",
                        new List<List<string>> {
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
                        "Rooted Ziggurat Portal Room Exit",
                        new List<List<string>> {
                            new List<string> {
                                "12", "Rooted Ziggurat Lower Back",
                            },
                        }
                    },
                }
            },
            {
                "Swamp",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Swamp to Cathedral Main Entrance",
                        new List<List<string>> {
                            new List<string> {
                                "12", "Hyperdash",
                            },
                            new List<string> {
                                "Stundagger", "Wand", "nmg",
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
                    {
                        "Back of Swamp",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                }
            },
            {
                "Swamp to Cathedral Treasure Room Entrance",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Swamp",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Swamp to Cathedral Main Entrance",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Swamp",
                        new List<List<string>> {
                            new List<string> {
                                "Stundagger", "Wand", "nmg",
                            },
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
                            new List<string> {
                                "Ladder Storage",
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
                        "Swamp",
                        new List<List<string>> {
                            new List<string> {
                                "26", "Wand", "Techbow", "Stundagger", "nmg",
                            },
                        }
                    },
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
                                "12", "Quarry Connector", "Quarry", "Wand"
                            },
                        }
                    },
                    {
                        "Far Shore to Library",
                        new List<List<string>> {
                            new List<string> {
                                "12", "Library Lab",
                            },
                        }
                    },
                    {
                        "Far Shore to West Garden",
                        new List<List<string>> {
                            new List<string> {
                                "12", "West Garden",
                            },
                        }
                    },
                    {
                        "Far Shore to Fortress",
                        new List<List<string>> {
                            new List<string> {
                                "12", "Fortress Exterior from Overworld", "Beneath the Vault Back", "Eastern Vault Fortress",
                            },
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

        public static void ShuffleList<T>(IList<T> list, int seed) {
            var rng = new System.Random(seed);
            int n = list.Count;

            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }


        // making a separate lists for portals connected to one, two, or three+ regions, to be populated by the foreach coming up next
        public static List<Portal> deadEndPortals = new List<Portal>();
        public static List<Portal> twoPlusPortals = new List<Portal>();

        public static Dictionary<string, int> UpdateReachableRegions(Dictionary<string, int> inventory) {
            int inv_count = inventory.Count;
            // for each origin region
            foreach (KeyValuePair<string, Dictionary<string, List<List<string>>>> traversal_group in TraversalReqs) {
                string origin = traversal_group.Key;
                if (!inventory.ContainsKey(origin)) {
                    continue;
                }
                //Logger.LogInfo("checking traversal for " + origin);
                // for each destination in an origin's group
                foreach (KeyValuePair<string, List<List<string>>> destination_group in traversal_group.Value) {
                    string destination = destination_group.Key;
                    //Logger.LogInfo("checking traversal to " + destination);
                    // if we can already reach this region, skip it
                    if (inventory.ContainsKey(destination)) {
                        //Logger.LogInfo("we already have it");
                        continue;
                    }
                    // met is whether you meet any of the requirement lists for a destination
                    bool met = false;
                    if (destination_group.Value.Count == 0) {
                        //Logger.LogInfo("no requirement groups, met is true");
                        met = true;
                    }
                    // check through each list of requirements
                    foreach (List<string> reqs in destination_group.Value) {
                        if (reqs.Count == 0) {
                            // if a group is empty, you can just walk there
                            met = true;
                            //Logger.LogInfo("group is empty, so met is true");
                        } else {
                            // check if we have the items in our inventory to traverse this path
                            int met_count = 0;
                            foreach (string req in reqs) {
                                //Logger.LogInfo("req is " + req);
                                // if sword progression is on, check for this too
                                if (req == "Sword") {
                                    if (inventory.ContainsKey("Sword Progression") && inventory["Sword Progression"] >= 2) {
                                        met_count++;
                                    }
                                }

                                if (req == "Stick") {
                                    if (inventory.ContainsKey("Sword Progression")) {
                                        met_count++;
                                    }
                                }

                                if (inventory.ContainsKey(req)) {
                                    met_count++;
                                    //Logger.LogInfo("we met this requirement");
                                }
                            }
                            // if you have all the requirements, you can traverse this path
                            if (met_count == reqs.Count) {
                                //Logger.LogInfo("met is true");
                                met = true;
                            }
                        }
                        // if we meet one list of requirements, we don't have to do the rest
                        if (met == true) {
                            break;
                        }
                    }
                    if (met == true) {
                        //Logger.LogInfo("adding " + destination + " to inventory");
                        inventory.Add(destination, 1);
                    } else {
                        //Logger.LogInfo("did not add " + destination + ", we did not meet the requirements");
                    }
                }
            }
            // if we gained any regions, rerun this to get any new regions
            if (inv_count != inventory.Count) {
                UpdateReachableRegions(inventory);
            }
            return inventory;
        }

        public static List<string> FirstStepsUpdateReachableRegions(List<string> inventory) {
            int inv_count = inventory.Count;
            // add all regions in Overworld that you can currently reach to the inventory
            // this could just not be a foreach, but it'll need to be one when ladders gets merged in
            foreach (KeyValuePair<string, Dictionary<string, List<List<string>>>> traversal_group in TunicPortals.TraversalReqs) {
                string origin_region = traversal_group.Key;
                if (!inventory.Contains(origin_region)) {
                    continue;
                }
                foreach (KeyValuePair<string, List<List<string>>> destination_group in traversal_group.Value) {
                    string destination = destination_group.Key;
                    if (inventory.Contains(destination)) {
                        continue;
                    }
                    // met is whether you meet any of the requirement lists for a destination
                    bool met = false;
                    // check through each list of requirements
                    foreach (List<string> reqs in destination_group.Value) {
                        if (reqs.Count == 0) {
                            // if a group is empty, you can just walk there
                            met = true;
                        } else {
                            // check if we have the items in our inventory to traverse this path
                            int met_count = 0;
                            foreach (string req in reqs) {
                                if (inventory.Contains(req)) {
                                    met_count++;
                                }
                            }
                            // if you have all the requirements, you can traverse this path
                            if (met_count == reqs.Count) {
                                met = true;
                            }
                        }
                        // if we meet one list of requirements, we don't have to do the rest
                        if (met == true) {
                            break;
                        }
                    }
                    if (met == true) {
                        inventory.Add(destination);
                    }
                }
            }
            // if we gained any regions, rerun this to get any new regions
            if (inv_count != inventory.Count) {
                FirstStepsUpdateReachableRegions(inventory);
            }
            return inventory;
        }

        public static Dictionary<Portal, Portal> VanillaPortals() {
            RandomizedPortals.Clear();
            Dictionary<Portal, Portal> portalPairs = new Dictionary<Portal, Portal>();
            List<Portal> portalList = new List<Portal>();
            int shop_num = 1;

            foreach (KeyValuePair<string, Dictionary<string, List<TunicPortal>>> scene_group in RegionPortalsList) {
                string scene_name = scene_group.Key;
                if (scene_name == "Shop") {
                    continue;
                }
                foreach (KeyValuePair<string, List<TunicPortal>> region_group in scene_group.Value) {
                    string region_name = region_group.Key;
                    if (region_name == "Zig Skip Exit") {
                        continue;
                    }
                    List<TunicPortal> region_portals = region_group.Value;
                    foreach (TunicPortal tunicPortal in region_portals) {
                        portalList.Add(new Portal(name: tunicPortal.Name, destination: tunicPortal.Destination, tag: tunicPortal.Tag, scene: scene_name, region: region_name));
                    }
                }
            }
            while (portalList.Count > 0) {
                Portal portal1 = portalList[0];
                Portal portal2 = new Portal("placeholder", "placeholder", "placeholder", "placeholder", "placeholder");  // I <3 csharp
                string portal2_sdt = portal1.DestinationSceneTag;
                if (portal2_sdt.StartsWith("Shop,")) {
                    portal2 = new Portal(name: "Shop Portal", destination: "Previous Region", tag: "", scene: "Shop", region: $"Shop Entrance {shop_num}");
                }
                else if (portal2_sdt == "Purgatory, Purgatory_bottom") {
                    portal2_sdt = "Purgatory, Purgatory_top";
                }

                foreach (Portal portal in portalList) {
                    if (portal.SceneDestinationTag == portal2_sdt) {
                        portal2 = portal;
                        break;
                    }
                }
                portalPairs[portal1] = portal2;
                portalList.Remove(portal1);
                if (!portal2_sdt.StartsWith("Shop,")) {
                    portalList.Remove(portal2);
                }
            }
            return portalPairs;
        }

        // create a list of all portals with their information loaded in, just a slightly expanded version of the above to include destinations
        public static void RandomizePortals(int seed) {
            RandomizedPortals.Clear();

            // separate the portals into their respective lists
            foreach (KeyValuePair<string, Dictionary<string, List<TunicPortal>>> scene_group in RegionPortalsList) {
                string scene_name = scene_group.Key;
                if (scene_name == "Shop") {
                    continue;
                }
                foreach (KeyValuePair<string, List<TunicPortal>> region_group in scene_group.Value) {
                    // if fixed shop is off, don't add zig skip exit to the portal list
                    string region_name = region_group.Key;
                    if (region_name == "Zig Skip Exit" && SaveFile.GetInt("randomizer ER fixed shop") != 1) {
                        continue;
                    }
                    List<TunicPortal> region_portals = region_group.Value;
                    foreach (TunicPortal tunicPortal in region_portals) {
                        Portal portal = new Portal(name: tunicPortal.Name, destination: tunicPortal.Destination, tag: tunicPortal.Tag, scene: scene_name, region: region_name);
                        if (RegionDict[region_name].DeadEnd == true) {
                            deadEndPortals.Add(portal);
                        } else {
                            twoPlusPortals.Add(portal);
                        }
                        // need to throw fairy cave into the twoPlus list if laurels is at 10 fairies
                        if (portal.Region == "Secret Gathering Place" && SaveFile.GetInt("randomizer laurels location") == 3) {
                            deadEndPortals.Remove(portal);
                            twoPlusPortals.Add(portal);
                        }
                    }
                }
            }
            if (SaveFile.GetInt("randomizer ER fixed shop") == 1) {
                foreach (Portal portal in twoPlusPortals) {
                    if (portal.SceneDestinationTag == "Overworld Redux, Windmill_") {
                        twoPlusPortals.Remove(portal);
                        break;
                    }
                }
            }

            // modify later if we ever do random start location
            string start_region = "Overworld";

            Dictionary<string, int> MaxItems = new Dictionary<string, int> {
                { "Stick", 1 }, { "Sword", 1 }, { "Wand", 1 }, { "Stundagger", 1 }, { "Techbow", 1 }, { "Hyperdash", 1 }, { "Mask", 1 }, { "Lantern", 1 }, { "12", 1 }, { "21", 1 }, { "26", 1 }, { "Key", 2 }, { "Key (House)", 1 }
            };

            Dictionary<string, int> FullInventory = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> item in MaxItems) {
                FullInventory.Add(item.Key, item.Value);
            }
            // if laurels is at 10 fairies, remove laurels until the fairy cave is connected
            if (SaveFile.GetInt("randomizer laurels location") == 3) {
                FullInventory.Remove("Hyperdash");
            }
            FullInventory.Add(start_region, 1);
            FullInventory = UpdateReachableRegions(FullInventory);

            // get the total number of regions to get before doing dead ends
            int total_nondeadend_count = 0;
            foreach (KeyValuePair<string, RegionInfo> region in RegionDict) {
                // if fixed shop is off, don't add the zig skip exit region to the nondeadend count
                if (region.Key == "Zig Skip Exit" && SaveFile.GetInt("randomizer ER fixed shop") != 1) {
                    continue;
                }
                if (region.Value.DeadEnd == false) {
                    total_nondeadend_count++;
                }
            }
            // added fairy cave to the non-dead end regions, so it should increase the count here too
            if (SaveFile.GetInt("randomizer laurels location") == 3) {
                total_nondeadend_count++;
            }

            int comboNumber = 0;
            while (FullInventory.Count < total_nondeadend_count + MaxItems.Count) {
                ShuffleList(twoPlusPortals, seed);
                Portal portal1 = null;
                Portal portal2 = null;
                foreach (Portal portal in twoPlusPortals) {
                    // find a portal in a region we can't access yet
                    if (!FullInventory.ContainsKey(portal.Region)) {
                        portal1 = portal;
                        twoPlusPortals.Remove(portal1);
                        break;
                    }
                }
                if (portal1 == null) {
                    Logger.LogInfo("something messed up in portal pairing for portal 1");
                }

                ShuffleList(twoPlusPortals, seed);
                foreach (Portal secondPortal in twoPlusPortals) {
                    if (FullInventory.ContainsKey(secondPortal.Region)) {
                        portal2 = secondPortal;
                        twoPlusPortals.Remove(secondPortal);
                        break;
                    }
                }
                if (portal2 == null) { 
                    Logger.LogInfo("something messed up in portal pairing for portal 2");
                }

                // add the portal combo to the randomized portals list
                RandomizedPortals.Add(comboNumber.ToString(), new PortalCombo(portal1, portal2));

                FullInventory.Add(portal1.Region, 1);
                // if laurels is at fairy cave, add it when we connect fairy cave
                if (portal1.Region == "Secret Gathering Place" && SaveFile.GetInt("randomizer laurels location") == 3) {
                    FullInventory.Add("Hyperdash", 1);
                }
                FullInventory = UpdateReachableRegions(FullInventory);
                comboNumber++;
            }

            // since the dead ends only have one exit, we just append them 1 to 1 to a random portal in the two plus list
            ShuffleList(deadEndPortals, seed);
            ShuffleList(twoPlusPortals, seed);
            while (deadEndPortals.Count > 0) {
                comboNumber++;
                RandomizedPortals.Add(comboNumber.ToString(), new PortalCombo(deadEndPortals[0], twoPlusPortals[0]));
                deadEndPortals.RemoveAt(0);
                twoPlusPortals.RemoveAt(0);
            }

            // shops get added separately cause they're weird
            List<string> shopSceneList = new List<string>();
            int shopCount = 6;
            if (SaveFile.GetInt("randomizer ER fixed shop") == 1) {
                shopCount = 0;
                Portal windmillPortal = new Portal(name: "Windmill Entrance", destination: "Windmill", tag: "_", scene: "Overworld Redux", region: "Overworld");
                Portal shopPortal = new Portal(name: "Shop Portal", destination: "Previous Region", tag: "", scene: "Shop", region: "Shop Entrance 2");
                RandomizedPortals.Add("fixedshop", new PortalCombo(windmillPortal, shopPortal));
                shopSceneList.Add("Overworld Redux");
            }
            int regionNumber = 0;
            while (shopCount > 0) {
                // manually making a portal for the shop, because it has some special properties
                Portal shopPortal = new Portal(name: "Shop Portal", destination: "Previous Region", tag: "", scene: "Shop", region: $"Shop Entrance {shopCount}");
                // check that a shop has not already been added to this region, since two shops in the same region causes problems
                if (!shopSceneList.Contains(twoPlusPortals[regionNumber].Scene)) {
                    comboNumber++;
                    shopSceneList.Add(twoPlusPortals[regionNumber].Scene);
                    //Logger.LogInfo("adding scene " + twoPlusPortals[regionNumber].Scene + " to shop scene list");
                    RandomizedPortals.Add(comboNumber.ToString(), new PortalCombo(twoPlusPortals[regionNumber], shopPortal));
                    twoPlusPortals.RemoveAt(regionNumber);
                    shopCount--;
                } else {
                    regionNumber++;
                }
                if (regionNumber == twoPlusPortals.Count - 1) {
                    Logger.LogInfo("too many shops, not enough regions, add more shops");
                }
            }
            // now we have every region accessible
            // the twoPlusPortals list still has items left in it, so now we pair them off
            while (twoPlusPortals.Count > 1) {
                comboNumber++;
                RandomizedPortals.Add(comboNumber.ToString(), new PortalCombo(twoPlusPortals[0], twoPlusPortals[1]));
                twoPlusPortals.RemoveAt(1); // I could do removeat0 twice, but I don't like how that looks
                twoPlusPortals.RemoveAt(0);
            }
            if (twoPlusPortals.Count == 1) {
                // if this triggers, there's an odd number of portals total
                Logger.LogInfo("one extra dead end remaining alone, rip. It's " + twoPlusPortals[0].Name);
            }
            // todo: figure out why the quarry portal isn't working right
            //Portal betaQuarryPortal = new Portal(destination: "Darkwoods", tag: "", name: "Beta Quarry", scene: "Quarry", region: "Quarry", requiredItems: new Dictionary<string, int>(), givesAccess: new List<string>(), deadEnd: true, prayerPortal: false, oneWay: false, ignoreScene: false);
            //Portal zigSkipPortal = new Portal(destination: "ziggurat2020_3", tag: "zig2_skip", name: "Zig Skip", scene: "ziggurat2020_1", region: "Zig 1", requiredItems: new Dictionary<string, int>(), givesAccess: new List<string>(), deadEnd: true, prayerPortal: false, oneWay: false, ignoreScene: false);
            //RandomizedPortals.Add("zigsecret", new PortalCombo(betaQuarryPortal, zigSkipPortal));
        }

        // this is for using the info from Archipelago to pair up the portals
        public static void CreatePortalPairs(Dictionary<string, string> APPortalStrings) {
            RandomizedPortals.Clear();
            List<Portal> portalsList = new List<Portal>();
            int comboNumber = 0;

            foreach (KeyValuePair<string, Dictionary<string, List<TunicPortal>>> scene_group in RegionPortalsList) {
                string scene_name = scene_group.Key;
                foreach (KeyValuePair<string, List<TunicPortal>> region_group in scene_group.Value) {
                    string region_name = region_group.Key;
                    List<TunicPortal> region_portals = region_group.Value;
                    foreach (TunicPortal tunicPortal in region_portals) {
                        Portal portal = new Portal(name: tunicPortal.Name, destination: tunicPortal.Destination, tag: tunicPortal.Tag, scene: scene_name, region: region_name);
                        portalsList.Add(portal);
                    }
                }
            }

            // make the PortalCombo dictionary
            foreach (KeyValuePair<string, string> stringPair in APPortalStrings) {
                string portal1SDT = stringPair.Key;
                string portal2SDT = stringPair.Value;
                Portal portal1 = null;
                Portal portal2 = null;

                foreach (Portal portal in portalsList) {
                    if (portal1SDT == portal.SceneDestinationTag) {
                        portal1 = portal;
                    }
                    if (portal2SDT == portal.SceneDestinationTag) {
                        portal2 = portal;
                    }
                }
                PortalCombo portalCombo = new PortalCombo(portal1, portal2);
                RandomizedPortals.Add(comboNumber.ToString(), portalCombo);
                comboNumber++;
            }
        }

        // a function to apply the randomized portal list to portals during onSceneLoaded
        public static void ModifyPortals(string scene_name) {
            var Portals = Resources.FindObjectsOfTypeAll<ScenePortal>().Where(portal => portal.gameObject.scene.name == SceneManager.GetActiveScene().name
            && !portal.FullID.Contains("heirfasttravel") && !portal.id.Contains("heirfasttravel"));
            foreach (var portal in Portals) {
                // go through the list of randomized portals and see if either the first or second portal matches the one we're looking at
                foreach (KeyValuePair<string, PortalCombo> portalCombo in RandomizedPortals) {
                    string comboTag = portalCombo.Key;
                    Portal portal1 = portalCombo.Value.Portal1;
                    Portal portal2 = portalCombo.Value.Portal2;

                    if (portal1.Scene == scene_name && portal1.DestinationTag == portal.FullID) {
                        if (portal2.Scene == "Shop") {
                            portal.destinationSceneName = portal2.Scene;
                            portal.id = "";
                            portal.optionalIDToSpawnAt = "";
                            portal.name = portal1.Name;
                        } else {
                            portal.destinationSceneName = portal2.Scene;
                            portal.id = comboTag;
                            portal.optionalIDToSpawnAt = comboTag + comboTag + comboTag + comboTag; // quadrupling since doubling and tripling can have overlaps
                            portal.name = portal1.Name;
                        }
                        break;
                    }

                    if (portal2.Scene == scene_name && portal2.DestinationTag == portal.FullID) {
                        if (portal1.Scene == "Shop") {
                            portal.destinationSceneName = portal1.Scene;
                            portal.id = "";
                            portal.optionalIDToSpawnAt = "";
                            portal.name = portal2.Name;
                        } else {
                            portal.destinationSceneName = portal1.Scene;
                            portal.id = comboTag + comboTag + comboTag + comboTag;
                            portal.optionalIDToSpawnAt = comboTag; // quadrupling since doubling and tripling can have overlaps
                            portal.name = portal2.Name;
                        }
                        break;
                    }
                }
            }
        }

        public static void MarkPortals() {
            var Portals = Resources.FindObjectsOfTypeAll<ScenePortal>().Where(portal => portal.gameObject.scene.name == SceneManager.GetActiveScene().name
            && !portal.FullID.Contains("heirfasttravel") && !portal.id.Contains("heirfasttravel"));

            foreach (var portal in Portals) {
                if (portal.FullID == PlayerCharacterSpawn.portalIDToSpawnAt) {
                    foreach (KeyValuePair<string, PortalCombo> portalCombo in TunicPortals.RandomizedPortals) {
                        if (portal.name == portalCombo.Value.Portal1.Name && (portal.name != "Shop Portal" || (portal.name == "Shop Portal" && portalCombo.Value.Portal2.Scene == SceneManager.GetActiveScene().name))) {
                            SaveFile.SetInt("randomizer entered portal " + portalCombo.Value.Portal1.Name, 1);
                            SaveFile.SetInt("randomizer entered portal " + portalCombo.Value.Portal2.Name, 1);
                        }
                        if (portal.name == portalCombo.Value.Portal2.Name && (portal.name != "Shop Portal" || (portal.name == "Shop Portal" && portalCombo.Value.Portal1.Scene == SceneManager.GetActiveScene().name))) {
                            SaveFile.SetInt("randomizer entered portal " + portalCombo.Value.Portal1.Name, 1);
                            SaveFile.SetInt("randomizer entered portal " + portalCombo.Value.Portal2.Name, 1);
                        }
                    }
                }
            }
        }

    }
}
