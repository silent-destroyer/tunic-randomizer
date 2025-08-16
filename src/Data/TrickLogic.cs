using System.Collections.Generic;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {
    public class TrickLogic {

        public static Dictionary<string, Dictionary<string, List<List<string>>>> LogicTrickTraversalReqs = new Dictionary<string, Dictionary<string, List<List<string>>>>();

        public static Dictionary<string, List<string>> OWElevPortals = new Dictionary<string, List<string>> {
            {"LS Elev 0", new List<string> {"Swamp Redux 2_conduit", "Overworld Cave_", "Atoll Redux_lower", "Maze Room_",
                                           "Town Basement_beach", "Archipelagos Redux_lower", "Archipelagos Redux_lowest"}},
            {"LS Elev 1", new List<string> {"Furnace_gyro_lower", "Furnace_gyro_west", "Swamp Redux 2_wall"}},
            {"LS Elev 2", new List<string> {"Archipelagos Redux_upper", "Ruins Passage_east"}},
            {"LS Elev 3", new List<string> {}},
            {"LS Elev 4", new List<string> {"Darkwoods Tunnel_"}},
            {"LS Elev 5", new List<string> {"PatrolCave_", "Forest Belltower_", "Fortress Courtyard_", "ShopSpecial_"}},
            {"LS Elev 6", new List<string> {"Temple_rafters"}},
            {"LS Elev 7", new List<string> {"Mountain_"}},
        };

        public class LadderInfo {
            public string Origin;  // origin region
            public string Destination;  // destination portal
            public string LaddersReq;  // ladders required to do this

            public LadderInfo(string origin, string destination, string laddersReq = null) {
                Origin = origin;
                Destination = destination;
                LaddersReq = laddersReq;
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
            new LadderInfo(origin: "Ruined Atoll", destination: "Atoll Redux, Frog Stairs_eye"),
            new LadderInfo(origin: "Forest Grave Path Main", destination: "Sword Access, East Forest Redux_upper"),
            new LadderInfo(origin: "Fortress Exterior from East Forest", destination: "Fortress Courtyard, Fortress Reliquary_Upper"),
            new LadderInfo(origin: "Fortress Exterior from East Forest", destination: "Fortress Courtyard, Fortress East_"),
            new LadderInfo(origin: "Fortress Exterior near cave", destination: "Fortress Courtyard, Fortress Reliquary_Upper", laddersReq: "Ladder to Beneath the Vault"),
            new LadderInfo(origin: "Fortress Exterior near cave", destination: "Fortress Courtyard, Fortress East_", laddersReq: "Ladder to Beneath the Vault"),
            new LadderInfo(origin: "Lower Mountain", destination: "Mountain, Mountaintop_"),
            new LadderInfo(origin: "Quarry Entry", destination: "Quarry Redux, Monastery_back"),
            new LadderInfo(origin: "Quarry Monastery Entry", destination: "Quarry Redux, Monastery_back"),
            new LadderInfo(origin: "Quarry Back", destination: "Quarry Redux, Monastery_back"),
            new LadderInfo(origin: "Rooted Ziggurat Lower Back", destination: "ziggurat2020_3, ziggurat2020_2_"),
            new LadderInfo(origin: "Swamp Mid", destination: "Swamp Redux 2, Overworld Redux_wall", laddersReq: "Ladders in Swamp"),
            new LadderInfo(origin: "Back of Swamp", destination: "Swamp Redux 2, Overworld Redux_wall"),
        };

        public static List<LadderInfo> HardLS = new List<LadderInfo> {
            new LadderInfo(origin: "Beneath the Well Front", destination: "Sewer, Sewer_Boss_", laddersReq: "Ladders in Well"),
            new LadderInfo(origin: "Beneath the Well Front", destination: "Sewer, Overworld Redux_west_aqueduct", laddersReq: "Ladders in Well"),
            new LadderInfo(origin: "Frog's Domain Front", destination: "frog cave main, Frog Stairs_Exit", laddersReq: "Ladders to Frog's Domain"),
            new LadderInfo(origin: "Swamp Mid", destination: "Swamp Redux 2, Cathedral Redux_main", laddersReq: "Ladders in Swamp"),
            new LadderInfo(origin: "Back of Swamp", destination: "Swamp Redux 2, Cathedral Redux_main"),
            new LadderInfo(origin: "Swamp Mid", destination: "Swamp Redux 2, Cathedral Redux_secret", laddersReq: "Ladders in Swamp"),
            new LadderInfo(origin: "Back of Swamp", destination: "Swamp Redux 2, Cathedral Redux_secret"),
        };


        // this is for Overworld portals that have weird rules
        // ex: change elevations on difficulty 1 (if there's convenient stairs), going behind the map, etc.
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

        public static List<LSElevConnect> LSElevConnections = new List<LSElevConnect> {
            new LSElevConnect(origin: "LS Elev 0", destination: "Overworld Redux, Furnace_gyro_west", difficulty: 1),

            new LSElevConnect(origin: "LS Elev 3", destination: "Overworld Redux, Sewer_west_aqueduct", difficulty: 2),
            new LSElevConnect(origin: "LS Elev 3", destination: "Overworld Redux, Furnace_gyro_upper_north", difficulty: 2),

            new LSElevConnect(origin: "LS Elev 1", destination: "Overworld Redux, EastFiligreeCache_", difficulty: 3),
            new LSElevConnect(origin: "LS Elev 2", destination: "Overworld Redux, Town_FiligreeRoom_", difficulty: 3),
            new LSElevConnect(origin: "LS Elev 2", destination: "Overworld Redux, Ruins Passage_west", difficulty: 3),
            new LSElevConnect(origin: "LS Elev 3", destination: "Overworld Redux, Overworld Interiors_house", difficulty: 3),
            new LSElevConnect(origin: "LS Elev 5", destination: "Overworld Redux, Temple_main", difficulty: 3),
        };

        public static Dictionary<string, Dictionary<string, List<List<string>>>> TraversalReqsWithLS(Dictionary<string, Dictionary<string, List<List<string>>>> traversalReqs) {
            Dictionary<string, Dictionary<string, List<List<string>>>> traversalReqsWithLS = traversalReqs;
            Dictionary<string, PortalCombo> portalList;
            if (GetBool(EntranceRando)) {
                portalList = ERData.RandomizedPortals;
            } else {
                if (ERData.VanillaPortals.Count == 0) {
                    ERScripts.SetupVanillaPortals();
                }
                portalList = ERData.VanillaPortals;
            }

            // add the Overworld LS regions
            foreach (KeyValuePair<string, List<string>> keyValuePair in OWElevPortals) {
                string lselev = keyValuePair.Key;
                List<string> portals = keyValuePair.Value;

                // traverse each portal, add the destination region and its rules
                foreach (string destinationPortal in portals) {
                    string destinationRegion = ERScripts.FindPairedPortalRegionFromSDT("Overworld Redux, " + destinationPortal);
                    // this is to handle when we have a partial randomized portals list, mostly because of Blue Prince stuff
                    if (destinationRegion == "FindPairedPortalRegionFromSDT failed to find a match") continue;
                    traversalReqsWithLS[lselev][destinationRegion] = new List<List<string>>();
                }
            }

            // add all the special OW LS connections
            foreach (LSElevConnect connection in LSElevConnections) {
                string destination = ERScripts.FindPairedPortalRegionFromSDT(connection.Destination);
                List<List<string>> rules = new List<List<string>> { new List<string> { "LS" + connection.Difficulty.ToString() } };
                if (!traversalReqsWithLS.ContainsKey(connection.Origin)) {
                    traversalReqsWithLS.Add(connection.Origin, new Dictionary<string, List<List<string>>> { { destination, rules } });
                } else {
                    if (!traversalReqsWithLS[connection.Origin].ContainsKey(destination)) {
                        traversalReqsWithLS[connection.Origin].Add(destination, rules);
                    } else {
                        traversalReqsWithLS[connection.Origin][destination].AddRange(rules);
                    }
                }
            }

            // add all the Easy, Medium, and Hard LS connections
            void DifficultyLS(List<LadderInfo> ladderInfos, string difficultyString) {
                foreach (LadderInfo ladderInfo in ladderInfos) {
                    List<List<string>> rules;
                    if (ladderInfo.LaddersReq == null) {
                        rules = new List<List<string>> { new List<string> { difficultyString } };
                    } else {
                        rules = new List<List<string>> { new List<string> { difficultyString, ladderInfo.LaddersReq } };
                    }
                    string destination = ERScripts.FindPairedPortalRegionFromSDT(ladderInfo.Destination);
                    if (!traversalReqsWithLS.ContainsKey(ladderInfo.Origin)) {
                        traversalReqsWithLS.Add(ladderInfo.Origin, new Dictionary<string, List<List<string>>> { { destination, rules } });
                    } else {
                        if (!traversalReqsWithLS[ladderInfo.Origin].ContainsKey(destination)) {
                            traversalReqsWithLS[ladderInfo.Origin].Add(destination, rules);
                        } else {
                            traversalReqsWithLS[ladderInfo.Origin][destination].AddRange(rules);
                        }
                    }
                }
            }
            DifficultyLS(EasyLS, "LS1");
            DifficultyLS(MediumLS, "LS2");
            DifficultyLS(HardLS, "LS3");

            // while we're here, let's just add the portal combo connections to the traversal reqs too for completion's sake
            // we could do this in another function, but ideally we always do this right after the trick logic (or right before I guess)
            foreach (PortalCombo portalCombo in portalList.Values) {
                string p1region = portalCombo.Portal1.Region;
                string p2region = portalCombo.Portal2.OutletRegion();

                // skip self-connections, cause why bother
                if (p1region == p2region) {
                    continue;
                }
                if (!traversalReqsWithLS.ContainsKey(p1region)) {
                    traversalReqsWithLS.Add(p1region, new Dictionary<string, List<List<string>>> { { p2region, new List<List<string>>() } });
                } else {
                    if (!traversalReqsWithLS[p1region].ContainsKey(p2region)) {
                        traversalReqsWithLS[p1region].Add(p2region, new List<List<string>>());
                    } else {
                        traversalReqsWithLS[p1region][p2region] = new List<List<string>>();
                    }
                }
            }

            return traversalReqsWithLS;
        }

    }
}
