using System.Collections.Generic;

namespace TunicRandomizer {
    public class TrickLogic {

        public static Dictionary<string, Dictionary<string, List<List<string>>>> LogicTrickTraversalReqs = new Dictionary<string, Dictionary<string, List<List<string>>>>();

        public class OWLadderInfo {
            public List<string> Ladders = new List<string>();  // ladders where the top or bottom is at the same elevation
            public List<string> Portals = new List<string>();  // portals at the same elevation, only those without doors
            public List<string> Regions = new List<string>();  // regions where a melee enemy can hit you out of ladder storage

            public OWLadderInfo(List<string> ladders, List<string> portals, List<string> regions) {
                Ladders = ladders;
                Portals = portals;
                Regions = regions;
            }
        }

        public static Dictionary<string, OWLadderInfo> OWLadderGroups = new Dictionary<string, OWLadderInfo> {
            {"LS Elev 0", new OWLadderInfo(
                ladders: new List<string> {"Ladders in Overworld Town", "Ladder to Ruined Atoll", "Ladder to Swamp"},
                portals: new List<string> {"Swamp Redux 2_conduit", "Overworld Cave_", "Atoll Redux_lower", "Maze Room_",
                                           "Town Basement_beach", "Archipelagos Redux_lower", "Archipelagos Redux_lowest"},
                regions: new List<string> {"Overworld Beach"}
                ) },
            {"LS Elev 1", new OWLadderInfo(
                ladders: new List<string> {"Ladders near Weathervane", "Ladders in Overworld Town", "Ladder to Swamp"},
                portals: new List<string> {"Furnace_gyro_lower", "Furnace_gyro_west", "Swamp Redux 2_wall"},
                regions: new List<string> {"Overworld Tunnel Turret"}
                ) },
            {"LS Elev 2", new OWLadderInfo(
                ladders: new List<string> {"Ladders near Weathervane", "Ladders to West Bell"},
                portals: new List<string> {"Archipelagos Redux_upper", "Ruins Passage_east"},
                regions: new List<string> {"After Ruined Passage"}
                ) },
            {"LS Elev 3", new OWLadderInfo(
                ladders: new List<string> {"Ladders near Weathervane", "Ladder to Quarry", "Ladders to West Bell", "Ladders in Overworld Town"},
                portals: new List<string> {},
                regions: new List<string> {"Overworld after Envoy", "East Overworld"}
                ) },
            {"LS Elev 4", new OWLadderInfo(
                ladders: new List<string> {"Ladders near Dark Tomb", "Ladder to Quarry", "Ladders to West Bell", "Ladders in Well", "Ladders in Overworld Town"},
                portals: new List<string> {"Darkwoods Tunnel_"},
                regions: new List<string> {}
                ) },
            {"LS Elev 5", new OWLadderInfo(
                ladders: new List<string> {"Ladders near Overworld Checkpoint", "Ladders near Patrol Cave"},
                portals: new List<string> {"PatrolCave_", "Forest Belltower_", "Fortress Courtyard_", "ShopSpecial_"},
                regions: new List<string> {"East Overworld"}
                ) },
            {"LS Elev 6", new OWLadderInfo(
                ladders: new List<string> {"Ladders near Patrol Cave", "Ladder near Temple Rafters"},
                portals: new List<string> {"Temple_rafters"},
                regions: new List<string> {"Overworld above Patrol Cave"}
                ) },
            {"LS Elev 7", new OWLadderInfo(
                ladders: new List<string> {"Ladders near Patrol Cave", "Ladder near Temple Rafters", "Ladders near Dark Tomb"},
                portals: new List<string> {"Mountain_"},
                regions: new List<string> {"Upper Overworld"}
                ) },
        };

        public static Dictionary<string, List<string>> RegionLadders = new Dictionary<string, List<string>> {
            {"Overworld", new List<string> {"Ladders near Weathervane", "Ladders near Overworld Checkpoint", "Ladders near Dark Tomb",
                                            "Ladders in Overworld Town", "Ladder to Swamp", "Ladders in Well"}},
            {"Overworld Beach", new List<string> {"Ladder to Ruined Atoll"}},
            {"Overworld at Patrol Cave", new List<string> {"Ladders near Patrol Cave"}},
            {"Overworld Quarry Entry", new List<string> {"Ladder to Quarry"}},
            {"Overworld Belltower", new List<string> {"Ladders to West Bell"}},
            {"Overworld after Temple Rafters", new List<string> {"Ladders near Temple Rafters"}},
        };

        public class LadderInfo {
            public string Origin;  // origin region
            public string Destination;  // destination portal
            public string LaddersReq;  // ladders required to do this
            public bool DestIsRegion;  // whether it is a region that you are going to

            public LadderInfo(string origin, string destination, string laddersReq = null, bool destIsRegion = false) {
                Origin = origin;
                Destination = destination;
                LaddersReq = laddersReq;
                DestIsRegion = destIsRegion;
            }
        }

        public static List<LadderInfo> EasyLS = new List<LadderInfo> {
            new LadderInfo(origin: "Furnace Ladder Area", destination: "Furnace, Overworld Redux_gyro_upper_north"),
            new LadderInfo(origin: "Furnace Ladder Area", destination: "Furnace, Crypt Redux_"),
            new LadderInfo(origin: "Furnace Ladder Area", destination: "Furnace, Overworld Redux_gyro_west"),
            new LadderInfo(origin: "West Garden before Boss", destination: "Archipelagos Redux, Overworld Redux_upper"),
            new LadderInfo(origin: "West Garden after Terry", destination: "Archipelagos Redux, Overworld Redux_lowest"),
            new LadderInfo(origin: "West Garden after Terry", destination: "Archipelagos Redux, archipelagos_house_"),
            new LadderInfo(origin: "Ruined Atoll", destination: "Atoll Redux, Overworld Redux_lower"),
            new LadderInfo(origin: "Ruined Atoll", destination: "Atoll Redux, Frog Stairs_mouth"),
            new LadderInfo(origin: "East Forest", destination: "East Forest Redux, East Forest Redux Laddercave_upper"),
            new LadderInfo(origin: "Guard House 1 West", destination: "East Forest Redux Laddercave, East Forest Redux_gate"),
            new LadderInfo(origin: "Guard House 1 West", destination: "East Forest Redux Laddercave, Forest Boss Room_"),
            new LadderInfo(origin: "Fortress Exterior from Overworld", destination: "Fortress Courtyard, Shop_"),
            new LadderInfo(origin: "Fortress Exterior from Overworld", destination: "Fortress Courtyard, Fortress Main_Big Door"),
            new LadderInfo(origin: "Fortress Exterior from Overworld", destination: "Fortress Courtyard, Fortress Reliquary_Lower"),
            new LadderInfo(origin: "Fortress Exterior from Overworld", destination: "Fortress Courtyard, Fortress Reliquary_Upper"),
            new LadderInfo(origin: "Fortress Exterior from Overworld", destination: "Fortress Courtyard, Fortress East_"),
            new LadderInfo(origin: "Fortress Exterior from East Forest", destination: "Fortress Courtyard, Overworld Redux_"),
            new LadderInfo(origin: "Fortress Exterior from East Forest", destination: "Fortress Courtyard, Shop_"),
            new LadderInfo(origin: "Fortress Exterior from East Forest", destination: "Fortress Courtyard, Fortress Main_Big Door"),
            new LadderInfo(origin: "Fortress Exterior from East Forest", destination: "Fortress Courtyard, Fortress Reliquary_Lower"),
            new LadderInfo(origin: "Fortress Exterior near cave", destination: "Fortress Courtyard, Overworld Redux_", laddersReq: "Ladder to Beneath the Vault"),
            new LadderInfo(origin: "Fortress Exterior near cave", destination: "Fortress Courtyard, Fortress Main_Big Door", laddersReq: "Ladder to Beneath the Vault"),
            new LadderInfo(origin: "Fortress Exterior near cave", destination: "Fortress Courtyard, Fortress Reliquary_Lower", laddersReq: "Ladder to Beneath the Vault"),
            new LadderInfo(origin: "Swamp Mid", destination: "Swamp Redux 2, Cathedral Arena_", laddersReq: "Ladders in Swamp"),
            new LadderInfo(origin: "Back of Swamp", destination: "Swamp Redux 2, Overworld Redux_conduit"),
            new LadderInfo(origin: "Back of Swamp", destination: "Swamp Redux 2, Shop_"),
        };

        public static List<LadderInfo> MediumLS = new List<LadderInfo> {
            new LadderInfo(origin: "East Forest", destination: "East Forest Dance Fox Spot", destIsRegion: true),
            new LadderInfo(origin: "Back of Swamp", destination: "Swamp Mid", destIsRegion: true),
            new LadderInfo(origin: "Back of Swamp", destination: "Swamp Front", destIsRegion: true),
            new LadderInfo(origin: "Ruined Atoll", destination: "Atoll Redux, Frog Stairs_eye"),
            new LadderInfo(origin: "Forest Grave Path Main", destination: "Sword Access, East Forest Redux_upper"),
            new LadderInfo(origin: "Fortress Exterior from Overworld", destination: "Fortress Courtyard Upper", destIsRegion: true),
            new LadderInfo(origin: "Fortress Exterior from East Forest", destination: "Fortress Courtyard, Fortress Reliquary_Upper"),
            new LadderInfo(origin: "Fortress Exterior from East Forest", destination: "Fortress Courtyard, Fortress East_"),
            new LadderInfo(origin: "Fortress Exterior from East Forest", destination: "Fortress Courtyard Upper", destIsRegion: true),
            new LadderInfo(origin: "Fortress Exterior near cave", destination: "Fortress Courtyard, Fortress Reliquary_Upper", laddersReq: "Ladder to Beneath the Vault"),
            new LadderInfo(origin: "Fortress Exterior near cave", destination: "Fortress Courtyard, Fortress East_", laddersReq: "Ladder to Beneath the Vault"),
            new LadderInfo(origin: "Fortress Exterior near cave", destination: "Fortress Courtyard Upper", laddersReq: "Ladder to Beneath the Vault", destIsRegion: true),
            new LadderInfo(origin: "Lower Mountain", destination: "Mountain, Mountaintop_"),
            new LadderInfo(origin: "Quarry Entry", destination: "Quarry Redux, Monastery_back"),
            new LadderInfo(origin: "Quarry Monastery Entry", destination: "Quarry Redux, Monastery_back"),
            new LadderInfo(origin: "Quarry Back", destination: "Quarry Redux, Monastery_back"),
            new LadderInfo(origin: "Rooted Ziggurat Lower Back", destination: "ziggurat2020_3, ziggurat2020_2_"),
            new LadderInfo(origin: "Rooted Ziggurat Lower Back", destination: "Rooted Ziggurat Lower Entry", destIsRegion: true),
            new LadderInfo(origin: "Rooted Ziggurat Lower Back", destination: "Rooted Ziggurat Lower Mid Checkpoint", destIsRegion: true),
            new LadderInfo(origin: "Swamp Mid", destination: "Swamp Redux 2, Overworld Redux_wall", laddersReq: "Ladders in Swamp"),
            new LadderInfo(origin: "Back of Swamp", destination: "Swamp Redux 2, Overworld Redux_wall"),
        };

        public static List<LadderInfo> HardLS = new List<LadderInfo> {
            new LadderInfo(origin: "Beneath the Well Front", destination: "Sewer, Sewer_Boss_", laddersReq: "Ladders in Well"),
            new LadderInfo(origin: "Beneath the Well Front", destination: "Sewer, Overworld Redux_west_aqueduct", laddersReq: "Ladders in Well"),
            new LadderInfo(origin: "Beneath the Well Front", destination: "Beneath the Well Back", laddersReq: "Ladders in Well", destIsRegion: true),
            new LadderInfo(origin: "Frog's Domain Front", destination: "frog cave main, Frog Stairs_Exit", laddersReq: "Ladders to Frog's Domain"),
            new LadderInfo(origin: "Frog's Domain Front", destination: "Frog's Domain Back", laddersReq: "Ladders to Frog's Domain", destIsRegion: true),
            new LadderInfo(origin: "Swamp Mid", destination: "Swamp Redux 2, Cathedral Redux_main", laddersReq: "Ladders in Swamp"),
            new LadderInfo(origin: "Back of Swamp", destination: "Swamp Redux 2, Cathedral Redux_main"),
            new LadderInfo(origin: "Swamp Mid", destination: "Swamp Redux 2, Cathedral Redux_secret", laddersReq: "Ladders in Swamp"),
            new LadderInfo(origin: "Back of Swamp", destination: "Swamp Redux 2, Cathedral Redux_secret"),
        };

        public class LSElevConnect {
            public string Origin;  // the LS elevation region you start in
            public string Destination;  // the portal you enter using LS
            public int Difficulty;  // 1 is easy, 2 is medium, 3 is hard

            public LSElevConnect(string origin, string destination, int difficulty) {
                Origin = origin;
                Destination = destination;
                Difficulty = difficulty;
            }
        }

        public List<LSElevConnect> LSElevConnections = new List<LSElevConnect> {
            new LSElevConnect(origin: "LS Elev 0", destination: "Overworld Redux, Furnace_gyro_west", difficulty: 1),

            new LSElevConnect(origin: "LS Elev 3", destination: "Overworld Redux, Sewer_west_aqueduct", difficulty: 2),
            new LSElevConnect(origin: "LS Elev 3", destination: "Overworld Redux, Furnace_gyro_upper_north", difficulty: 2),

            new LSElevConnect(origin: "LS Elev 1", destination: "Overworld Redux, EastFiligreeCache_", difficulty: 3),
            new LSElevConnect(origin: "LS Elev 2", destination: "Overworld Redux, Town_FiligreeRoom_", difficulty: 3),
            new LSElevConnect(origin: "LS Elev 2", destination: "Overworld Redux, Ruins Passage_west", difficulty: 3),
            new LSElevConnect(origin: "LS Elev 3", destination: "Overworld Redux, Overworld Interiors_house", difficulty: 3),
            new LSElevConnect(origin: "LS Elev 5", destination: "Overworld Redux, Temple_main", difficulty: 3),
        };

    }
}
