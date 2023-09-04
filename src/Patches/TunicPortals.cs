using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;
using BepInEx.Logging;
using Lib;

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
            public Vector3 Position;
            public Quaternion Rotation;

            public TunicPortal() { }

            public TunicPortal(string destination, string destinationTag, string portalName)
            {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                DestinationPair = destination + destinationTag;
            }
        }

        public static Dictionary<string, List<TunicPortal>> PortalList = new Dictionary<string, List<TunicPortal>>
        {   // formatted with region the portal is in, with the dict being the tag combo and portal name
            {
                "Overworld Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Sword Cave_", "", "Portal (31)"),
                    new TunicPortal("Windmill_", "", "Portal (4)"),
                    new TunicPortal("Waterfall_", "", "Portal (38)"),
                    new TunicPortal("Sewer_", "entrance", "Portal (9)"),
                    new TunicPortal("Sewer_", "west_aqueduct", "Portal (11)"),
                    new TunicPortal("Overworld Interiors_", "house", "Portal (3)"),
                    new TunicPortal("Overworld Interiors_", "under_checkpoint", "Portal (7)"),
                    new TunicPortal("Furnace_", "gyro_upper_north", "Portal (10)"),
                    new TunicPortal("Furnace_", "gyro_upper_east", "Portal (1)"),
                    new TunicPortal("Furnace_", "gyro_west", "Portal (20)"),
                    new TunicPortal("Furnace_", "gyro_lower", "Portal"),
                    new TunicPortal("Overworld Cave_", "", "Portal (39)"),
                    new TunicPortal("Swamp Redux 2_", "wall", "Portal (26)"),
                    new TunicPortal("Swamp Redux 2_", "conduit", "Portal (35)"),
                    new TunicPortal("Ruins Passage_", "east", "Portal (14)"),
                    new TunicPortal("Ruins Passage_", "west", "_Portal (15)"),
                    new TunicPortal("Atoll Redux_", "upper", "Portal (24)"),
                    new TunicPortal("Atoll Redux_", "lower", "Portal (21)"),
                    new TunicPortal("ShopSpecial_", "", "Portal (36)"),
                    new TunicPortal("Maze Room_", "", "Portal (1)"),
                    new TunicPortal("Archipelagos Redux_", "upper", "Portal (23)"),
                    new TunicPortal("Archipelagos Redux_", "lower", "Portal (22)"),
                    new TunicPortal("Archipelagos Redux_", "lowest", "Portal (37)"),
                    new TunicPortal("Temple_", "main", "Portal (8)"),
                    new TunicPortal("Temple_", "rafters", "Portal (33)"),
                    new TunicPortal("Ruined Shop_", "", "Portal (19)"),
                    new TunicPortal("PatrolCave_", "", "Portal"),
                    new TunicPortal("Town Basement_", "beach", "Portal (32)"),
                    new TunicPortal("Changing Room_", "", "Portal"),
                    new TunicPortal("CubeRoom_", "", "Portal (15)"),
                    new TunicPortal("Mountain_", "", "Portal (27)"),
                    new TunicPortal("Fortress Courtyard_", "", "Portal (18)"),
                    new TunicPortal("Town_FiligreeRoom_", "", "Portal (34)"),
                    new TunicPortal("EastFiligreeCache_", "", "Portal"),
                    new TunicPortal("Darkwoods Tunnel_", "", "Portal (25)"),
                    new TunicPortal("Crypt Redux_", "", "Portal (30)"),
                    new TunicPortal("Forest Belltower_", "", "Portal (17)"),
                    new TunicPortal("Transit_", "teleporter_town", "Portal"),
                    new TunicPortal("Transit_", "teleporter_starting island", "Portal"),

                    new TunicPortal("_", "", "Portal"), // ?
                    new TunicPortal("Forest Belltower_", "showfloordemo2022", "Portal (12)"), // ?
                    new TunicPortal("DEMO_altEnd_", "", "_Portal (Secret Demo End)"), // ?
                }
            },
            {
                "Windmill",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "", "Portal (4)"),
                    new TunicPortal("Shop_", "", "Portal (5)"),
                }
            },
            {
                "Overworld Interiors", // House in town
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "house", "Portal"),
                    new TunicPortal("Archipelagos Redux_", "", "_ShowfloorDemo2022 Portal"), // unused and disabled
                    new TunicPortal("g_elements_", "", "Portal"),
                    new TunicPortal("Overworld Redux_", "under_checkpoint", "Portal (1)"),
                }
            },
            {
                "g_elements", // Secret treasures room
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Interiors_", "", "Portal"),
                }
            },
            {
                "Furnace", // Under the west belltower
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "gyro_upper_north", "Portal (10)"),
                    new TunicPortal("Crypt Redux_", "", "Portal (6)"),
                    new TunicPortal("Overworld Redux_", "gyro_west", "Portal (2)"),
                    new TunicPortal("Overworld Redux_", "gyro_lower", "Portal"),
                    new TunicPortal("Overworld Redux_", "gyro_upper_east", "Portal (1)"),
                }
            },
            {
                "Sword Cave", // Stick house
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "", "Portal"),
                }
            },
            {
                "Ruins Passage", // That little hallway with the key door near the start in Overworld
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "east", "Portal (9)"),
                    new TunicPortal("Overworld Redux_", "west", "Portal (8)")
                }
            },
            {
                "EastFiligreeCache", // The holy cross room with the 3 chests near swamp entrance
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "", "Portal"),
                }
            },
            {
                "Overworld Cave", // East beach, next to swamp entrance, rotating lights room
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "", "Portal")
                }
            },
            {
                "Maze Room", // Invisible maze
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "", "Portal"),
                }
            },
            {
                "Town Basement", // Hourglass cave
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "beach", "Portal (33)"), // yes, it has a tag even though it doesn't need one
                }
            },
            {
                "ShopSpecial", // Special shop, laurel across from that platform between east forest and fortress
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "", "Portal"),
                }
            },
            {
                "Temple", // Where you put the hexes
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "rafters", "Portal (2)"),
                    new TunicPortal("Overworld Redux_", "main", "Portal"),
                }
            },
            {
                "Sewer", // Bottom of the Well
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "entrance", "Portal"),
                    new TunicPortal("Sewer_Boss_", "", "Portal (3)"),
                    new TunicPortal("Overworld Redux_", "west_aqueduct", "Portal (2)"),
                }
            },
            {
                "Sewer_Boss", // Boss room in the Bottom of the Well
                new List<TunicPortal>
                {
                    new TunicPortal("Sewer_", "", "Portal"),
                    new TunicPortal("Crypt Redux_", "", "Portal (2)"),
                }
            },
            {
                "Crypt Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "", "Portal (1)"),
                    new TunicPortal("Furnace_", "", "Portal (2)"),
                    new TunicPortal("Sewer_Boss_", "", "Portal"),
                }
            },
            {
                "Archipelagos Redux", // West Garden
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "lower", "Portal"),
                    new TunicPortal("archipelagos_house_", "", "Portal (4)"),
                    new TunicPortal("Overworld Redux_", "upper", "Portal (1)"),
                    new TunicPortal("Shop_", "", "Portal (6)"),
                    new TunicPortal("Overworld Redux_", "lowest", "Portal (2)"),
                    new TunicPortal("Shop_", "", "Portal (5)"),
                    new TunicPortal("RelicVoid_", "teleporter_relic plinth", "Portal"), // Hero grave
                    new TunicPortal("Transit_", "teleporter_archipelagos_teleporter", "Portal"), // Portal to the thing behind dagger house
                }
            },
            {
                "archipelagos_house", // Magic Dagger house in West Garden
                new List<TunicPortal>
                {
                    new TunicPortal("Archipelagos Redux_", "", "Portal"),
                }
            },
            {
                "Atoll Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Frog Stairs_", "eye", "Portal (1)"),
                    new TunicPortal("Library Exterior_", "", "Portal"),
                    new TunicPortal("Overworld Redux_", "upper", "Portal (24)"),
                    new TunicPortal("Overworld Redux_", "lower", "Portal (21)"),
                    new TunicPortal("Frog Stairs_", "mouth", "Portal"),
                    new TunicPortal("Shop_", "", "Portal"),
                    new TunicPortal("Transit_", "teleporter_atoll", "Portal"),
                    new TunicPortal("Forest Lake_", "teleporter", "Portal"), // Unused portal, same spot as library portal
                }
            },
            {
                "Frog Stairs", // Entrance to frog's domain
                new List<TunicPortal>
                {
                    new TunicPortal("Atoll Redux_", "mouth", "Portal"),
                    new TunicPortal("frog cave main_", "Exit", "Portal (3)"),
                    new TunicPortal("Atoll Redux_", "eye", "Portal (2)"),
                    new TunicPortal("frog cave main_", "Entrance", "Portal (1)"),
                }
            },
            {
                "frog cave main", // Frog's domain, yes it's lowercase
                new List<TunicPortal>
                {
                    new TunicPortal("Frog Stairs_", "Exit", "Portal (1)"),
                    new TunicPortal("Frog Stairs_", "Entrance", "Portal"),
                }
            },
            {
                "Library Exterior",
                new List<TunicPortal>
                {
                    new TunicPortal("Library Hall_", "", "Portal"),
                    new TunicPortal("Atoll Redux_", "", "Portal"),
                }
            },
            {
                "Library Hall", // Entry area with hero grave
                new List<TunicPortal>
                {
                    new TunicPortal("Library Rotunda_", "", "Portal"),
                    new TunicPortal("Library Exterior_", "", "Portal (1)"),
                    new TunicPortal("RelicVoid_", "teleporter_relic plinth", "Portal"),
                }
            },
            {
                "Library Rotunda", // The circular room with the ladder
                new List<TunicPortal>
                {
                    new TunicPortal("Library Hall_", "", "Portal"),
                    new TunicPortal("Library Lab_", "", "Portal (1)"),
                }
            },
            {
                "Library Lab",
                new List<TunicPortal>
                {
                    new TunicPortal("Library Arena_", "", "Portal (3)"),
                    new TunicPortal("Library Rotunda_", "", "Portal (2)"),
                    new TunicPortal("Transit_", "teleporter_library teleporter", "Portal"),
                }
            },
            {
                "Library Arena",
                new List<TunicPortal>
                {
                    new TunicPortal("Library Lab_", "", "Portal")
                }
            },
            {
                "East Forest Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Sword Access_", "lower", "Portal (5)"),
                    new TunicPortal("East Forest Redux Laddercave_", "upper", "Portal (2)"),
                    new TunicPortal("East Forest Redux Interior_", "lower", "Portal (7)"),
                    new TunicPortal("East Forest Redux Laddercave_", "gate", "Portal (3)"),
                    new TunicPortal("Sword Access_", "upper", "Portal (6)"),
                    new TunicPortal("East Forest Redux Interior_", "upper", "Portal (4)"),
                    new TunicPortal("East Forest Redux Laddercave_", "lower", "Portal (1)"),
                    new TunicPortal("Forest Belltower_", "", "Portal"),
                    new TunicPortal("Transit_", "teleporter_forest teleporter", "Portal"),
                }
            },
            {
                "East Forest Redux Laddercave", // the place with the two ladders that leads to the boss room
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux_", "upper", "Portal (2)"),
                    new TunicPortal("East Forest Redux_", "lower", "Portal (1)"),
                    new TunicPortal("East Forest Redux_", "gate", "Portal (3)"),
                    new TunicPortal("Forest Boss Room_", "", "Portal"),
                }
            },
            {
                "Sword Access", // East forest hero grave area
                new List<TunicPortal>
                {
                    new TunicPortal("Forest 1_", "upper", "Portal (2)"),
                    new TunicPortal("East Forest Redux_", "upper", "Portal (1)"),
                    new TunicPortal("Forest 1_", "lower", "Portal (1)"),
                    new TunicPortal("Forest 1_", "", "Portal"),
                    new TunicPortal("East Forest Redux_", "lower", "Portal"),
                    new TunicPortal("RelicVoid_", "teleporter_relic plinth", "Portal"),
                    new TunicPortal("RelicVoid_", "teleporter_relic plinth", "Portal"),
                }
            },
            {
                "East Forest Redux Interior", // Guardhouse 2
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux_", "lower", "Portal (1)"),
                    new TunicPortal("East Forest Redux_", "upper", "Portal"),
                }
            },
            {
                "Forest Boss Room",
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux Laddercave_", "", "Portal"),
                    new TunicPortal("Archipelagos Redux_", "showfloordemo2022", "Portal (2)"),
                    new TunicPortal("Forest Belltower_", "", "Portal (1)"),
                }
            },
            {
                "Forest Belltower",
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Courtyard_", "", "Portal (2)"),
                    new TunicPortal("East Forest Redux_", "", "Portal (3)"),
                    new TunicPortal("Overworld Redux_", "", "Portal (1)"),
                    new TunicPortal("Forest Boss Room_", "", "Portal"),
                }
            },
            {
                "Fortress Courtyard", // Outside the fortress, the area connected to east forest and overworld
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Reliquary_", "Lower", "Portal (6)"),
                    new TunicPortal("Fortress Reliquary_", "Upper", "Portal (5)"),
                    new TunicPortal("Fortress Main_", "Big Door", "Portal"),
                    new TunicPortal("Fortress East_", "", "Portal (1)"),
                    new TunicPortal("Fortress Basement_", "", "Portal (3)"),
                    new TunicPortal("Forest Belltower_", "", "Portal (7)"),
                    new TunicPortal("Overworld Redux_", "", "Portal (8)"),  // Why are there two of these????
                    new TunicPortal("Overworld Redux_", "", "Portal (4)"),
                    new TunicPortal("Shop_", "", "Portal (2)"),
                }
            },
            {
                "Fortress Basement", // Under the fortress
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Main_", "", "Portal (1)"),
                    new TunicPortal("Fortress Courtyard_", "", "Portal"),
                }
            },
            {
                "Fortress Main", // Inside the fortress
                new List<TunicPortal>
                {
                    new TunicPortal("Shop_", "", "Portal (2)"),
                    new TunicPortal("Fortress Basement_", "", "Portal"),
                    new TunicPortal("Fortress Courtyard_", "Big Door", "Portal (1)"),
                    new TunicPortal("Fortress Arena_", "", "Portal (3)"),
                    new TunicPortal("Fortress East_", "upper", "Portal (4)"),
                    new TunicPortal("Fortress East_", "lower", "Portal (5)"),
                }
            },
            {
                "Fortress East", // that tiny area with the couple mages up high, and the ladder in the lower right
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Main_", "lower", "Portal (5)"),
                    new TunicPortal("Fortress Courtyard_", "", "Portal (2)"),
                    new TunicPortal("Fortress Main_", "upper", "Portal (4)"),
                }
            },
            {
                "Fortress Reliquary", // Where the grave is
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Courtyard_", "Lower", "Portal"),
                    new TunicPortal("Dusty_", "", "Portal (2)"),
                    new TunicPortal("Fortress Courtyard_", "Upper", "Portal (1)"),
                    new TunicPortal("RelicVoid_", "teleporter_relic plinth", "Portal"),
                }
            },
            {
                "Fortress Arena", // Where the boss is
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Main_", "", "Portal"),
                    new TunicPortal("Fortress Main_", "", "Portal"),
                    new TunicPortal("Transit_", "teleporter_spidertank", "Portal"),
                }
            },
            {
                "Dusty", // broom
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Reliquary_", "", "Portal"),
                }
            },
            {
                "Mountain",
                new List<TunicPortal>
                {
                    new TunicPortal("Mountaintop_", "", "Portal"),
                    new TunicPortal("Quarry Redux_", "", "Portal (2)"),
                    new TunicPortal("Overworld Redux_", "", "Portal (1)"),
                }
            },
            {
                "Mountaintop",
                new List<TunicPortal>
                {
                    new TunicPortal("Mountain_", "", "Portal"),
                }
            },
            {
                "Darkwoods Tunnel", // tunnel between overworld and quarry
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "", "Portal"),
                    new TunicPortal("Quarry Redux_", "", "Portal (1)"),
                }
            },
            {
                "Quarry Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Monastery_", "front", "Portal"),
                    new TunicPortal("Shop_", "", "Portal"),
                    new TunicPortal("Monastery_", "back", "Portal (1)"),
                    new TunicPortal("Mountain_", "", "Portal (2)"),
                    new TunicPortal("ziggurat2020_0_", "", "Portal (3)"),
                    new TunicPortal("Darkwoods Tunnel_", "", "Portal"),
                    new TunicPortal("Transit_", "teleporter_quarry teleporter", "Portal")
                }
            },
            {
                "Monastery",
                new List<TunicPortal>
                {
                    new TunicPortal("Quarry Redux_", "back", "Portal (2)"),
                    new TunicPortal("Quarry Redux_", "front", "Portal (1)"),
                    new TunicPortal("Quarry_", "lower", "Portal (1)"), // Unused portal, disabled, and very high up
                    new TunicPortal("RelicVoid_", "teleporter_relic plinth", "Portal"),
                }
            },
            {
                "ziggurat2020_0", // Zig entrance
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_1_", "", "Portal (1)"),
                    new TunicPortal("Quarry Redux_", "", "Portal"),
                }
            },
            {
                "ziggurat2020_1", // Upper zig
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_3_", "zig2_skip", "Portal (1)"), // the elevator skip to lower zig
                    new TunicPortal("ziggurat2020_0_", "", "Portal"),
                    new TunicPortal("ziggurat2020_2_", "", "Portal"),
                }
            },
            {
                "ziggurat2020_2", // Zig intermediate elevator
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_1_", "", "Portal"),
                    new TunicPortal("ziggurat2020_3_", "", "Portal (1)"),
                }
            },
            {
                "ziggurat2020_3", // Lower zig
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_FTRoom_", "", "Portal (1)"), // Prayer portal room
                    new TunicPortal("ziggurat2020_1_", "zig2_skip", "Portal (2)"), // the elevator skip to lower zig
                    new TunicPortal("ziggurat2020_2_", "", "Portal"),
                }
            },
            {
                "ziggurat2020_FTRoom", // The room with the prayer portal
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_3_", "", "Portal"),
                    new TunicPortal("Transit_", "teleporter_ziggurat teleporter", "Portal"),
                }
            },
            {
                "Swamp Redux 2",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux_", "conduit", "Portal"),
                    new TunicPortal("Cathedral Redux_", "main", "Portal (4)"),
                    new TunicPortal("Cathedral Redux_", "secret", "Portal (3)"),
                    new TunicPortal("Cathedral Arena_", "", "Portal (5)"),
                    new TunicPortal("Shop_", "", "Portal (2)"),
                    new TunicPortal("Overworld Redux_", "wall", "Portal (1)"),
                    new TunicPortal("RelicVoid_", "teleporter_relic plinth", "Portal"),
                }
            },
            {
                "Cathedral Redux", // Gauntlet
                new List<TunicPortal>
                {
                    new TunicPortal("Swamp Redux 2_", "main", "Portal (1)"),
                    new TunicPortal("Cathedral Arena_", "", "Portal"),
                    new TunicPortal("Swamp Redux 2_", "secret", "Portal (2)"),
                }
            },
            {
                "Cathedral Arena", // Gauntlet
                new List<TunicPortal>
                {
                    new TunicPortal("Swamp Redux 2_", "", "Portal"),
                    new TunicPortal("Cathedral Redux_", "", "Portal (1)"),
                    new TunicPortal("Shop_", "", "Portal (1)"),
                }
            },
            {
                "Shop", // Every shop is just this region. Need to figure out how it determines where to go back to, since its destination is _
                new List<TunicPortal>
                {
                    new TunicPortal("_", "", "Portal"),
                }
            },
            {
                "RelicVoid", // Hero relic area
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Reliquary_", "teleporter_relic plinth", "Portal"),
                    new TunicPortal("Monastery_", "teleporter_relic plinth", "Portal"),
                    new TunicPortal("Archipelagos Redux_", "teleporter_relic plinth", "Portal"),
                    new TunicPortal("Sword Access_", "teleporter_relic plinth", "Portal"),
                    new TunicPortal("Library Hall_", "teleporter_relic plinth", "Portal"),
                    new TunicPortal("Swamp Redux 2_", "teleporter_relic plinth", "Portal"),
                }
            },
            {
                "Transit", // Teleporter hub
                new List<TunicPortal>
                {
                    new TunicPortal("Archipelagos Redux_", "teleporter_archipelagos_teleporter", "Portal"),
                    new TunicPortal("Library Lab_", "teleporter_library teleporter", "Portal"),
                    new TunicPortal("Transit_", "teleporter_", "Portal"), // Unused portal, far away and not enabled
                    new TunicPortal("Quarry Redux_", "teleporter_quarry teleporter", "Portal"),
                    new TunicPortal("East Forest Redux_", "teleporter_forest teleporter", "Portal"),
                    new TunicPortal("Fortress Arena_", "teleporter_spidertank", "Portal"),
                    new TunicPortal("Atoll Redux_", "teleporter_atoll", "Portal"),
                    new TunicPortal("ziggurat2020_FTRoom_", "teleporter_ziggurat teleporter", "Portal"),
                    new TunicPortal("Spirit Arena_", "teleporter_spirit arena", "Portal"),
                    new TunicPortal("Overworld Redux_", "teleporter_town", "Portal"),
                    new TunicPortal("Overworld Redux_", "teleporter_starting island", "Portal"),
                }
            },
            {
                "Spirit Arena", // Heir fight
                new List<TunicPortal>
                {
                    new TunicPortal("Transit_", "teleporter_spirit arena", "Portal"),
                }
            },
            {
                "Purgatory", // Second save hallway
                new List<TunicPortal>
                {
                    new TunicPortal("Purgatory_", "bottom", "Portal"),
                    new TunicPortal("Purgatory_", "top", "Portal (1)"),
                }
            },
        };

        //public static void ScenePortal_DepartToScene_PrefixPatch(MonoBehaviour coroutineRunner, bool whiteout, float transitionDuration, string destinationSceneName, string id, bool pauseTime, float delay)
        public static void SwapSwampPortals()
        {
            var Portals = Resources.FindObjectsOfTypeAll<ScenePortal>();
            foreach (var portal in Portals)
            {
                //Logger.LogInfo("new TunicPortal(\"" + portal.destinationSceneName + "_\", \"" + portal.id + "\", \"" + portal.name + "\"),");
                if (portal.FullID == "Overworld Redux_conduit")
                {
                    portal.optionalIDToSpawnAt = "wall";
                }
                if (portal.FullID == "Swamp Redux 2_conduit")
                {
                    portal.optionalIDToSpawnAt = "wall";
                }
                if (portal.FullID == "Overworld Redux_wall")
                {
                    portal.optionalIDToSpawnAt = "conduit";
                }
                if (portal.FullID == "Swamp Redux 2_wall")
                {
                    portal.optionalIDToSpawnAt = "conduit";
                }
            }
        }
        // [Message:UnityExplorer] [Unity] [Player Character Spawn] going to Shop, Cathedral Arena_

        //   compare this list of portals to one created to assign the tags we want after randomizing the portals
        //then change the destination and destinationtag to match
        //   It makes the most sense to just straight number all of the portal tags
        //that way like numbers can match up and be two sides of the same portal

        //Resources.FindObjectsOfTypeAll<ScenePortal>()
        //foreach (KeyValuePair<string, List<TunicPortal>> RegionGroup in PortalList)
        //    {
        //        string RegionName = RegionGroup.Key;
        //    }
        //    string TestString = "ScenePortal.DepartToScene(MonoBehaviour coroutineRunner, bool whiteout, float transitionDuration, string id, bool pauseTime, float delay"
    }
}
