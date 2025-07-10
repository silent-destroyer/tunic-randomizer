using System.Collections.Generic;
using static TunicRandomizer.SaveFlags;

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

            // add connections to the Overworld LS regions
            foreach (KeyValuePair<string, Dictionary<string, List<List<string>>>> keyValuePair in traversalReqs) {
                string originRegion = keyValuePair.Key;
                if (RegionLadders.ContainsKey(originRegion)) {
                    foreach (string ladder in RegionLadders[originRegion]) {
                        foreach (KeyValuePair<string, OWLadderInfo> ladderGroup in OWLadderGroups) {
                            string lselev = ladderGroup.Key;
                            OWLadderInfo ladderInfo = ladderGroup.Value;
                            if (ladderInfo.Ladders.Contains(ladder)) {
                                //traversalReqsWithLS[originRegion].SetDefault(lselev, new List<List<string>> { new List<string> { "LS1", ladder } } );
                                if (!traversalReqsWithLS[originRegion].ContainsKey(lselev)) {
                                    traversalReqsWithLS[originRegion].Add(lselev, new List<List<string>> { new List<string> { "LS1", ladder } });
                                } else {
                                    traversalReqsWithLS[originRegion][lselev].AddRange(new List<List<string>> { new List<string> { "LS1", ladder } });
                                }
                            }
                        }
                    }
                }
            }

            // add the Overworld LS regions
            foreach (KeyValuePair<string, OWLadderInfo> keyValuePair in OWLadderGroups) {
                string lselev = keyValuePair.Key;
                OWLadderInfo ladderInfo = keyValuePair.Value;
                traversalReqsWithLS.Add(lselev, new Dictionary<string, List<List<string>>>());

                Dictionary<string, List<List<string>>> destinations = new Dictionary<string, List<List<string>>>();
                // build the rules for each destination, add them to the dictionary
                foreach (string destinationPortal in ladderInfo.Portals) {
                    string destinationRegion = ERScripts.FindPairedPortalRegionFromSDT("Overworld Redux, " + destinationPortal);
                    List<List<string>> allReqsForDestination = new List<List<string>>();
                    foreach (string ladder in ladderInfo.Ladders) {
                        allReqsForDestination.Add(new List<string> { "LS1", ladder });
                    }
                    // add the destination region and its rules
                    if (!destinations.ContainsKey(destinationRegion)) {
                        destinations.Add(destinationRegion, allReqsForDestination);
                    } else {
                        destinations[destinationRegion].AddRange(allReqsForDestination);
                    }
                    //destinations.SetDefault(destinationRegion, allReqsForDestination);
                }
                foreach (string destinationRegion in ladderInfo.Regions) {
                    List<List<string>> allReqsForDestination = new List<List<string>>();
                    foreach (string ladder in ladderInfo.Ladders) {
                        allReqsForDestination.Add(new List<string> { "LS2", ladder });
                    }
                    // add the destination region and its rules
                    if (!destinations.ContainsKey(destinationRegion)) {
                        destinations.Add(destinationRegion, allReqsForDestination);
                    } else {
                        destinations[destinationRegion].AddRange(allReqsForDestination);
                    }
                    //destinations.SetDefault(destinationRegion, allReqsForDestination);
                }
                foreach (KeyValuePair<string, List<List<string>>> destination in destinations) {
                    //traversalReqsWithLS[lselev].SetDefault(destination.Key, destination.Value);
                    if (!traversalReqsWithLS[lselev].ContainsKey(destination.Key)) {
                        traversalReqsWithLS[lselev].Add(destination.Key, destination.Value);
                    } else {
                        traversalReqsWithLS[lselev][destination.Key].AddRange(destination.Value);
                    }
                }
            }

            // add all the connections between LS Elev regions
            string baseString = "LS Elev ";
            for (int i = 0; i < OWLadderGroups.Count - 1; i++) {
                //traversalReqsWithLS.SetDefault(baseString + i.ToString(), new Dictionary<string, List<List<string>>> { { baseString + (i + 1).ToString(), new List<List<string>> { new List<string> { "LS2" } } } });
                if (!traversalReqsWithLS.ContainsKey(baseString + i.ToString())) {
                    traversalReqsWithLS.Add(baseString + i.ToString(), new Dictionary<string, List<List<string>>> { { baseString + (i + 1).ToString(), new List<List<string>> { new List<string> { "LS2" } } } });
                } else {
                    traversalReqsWithLS[baseString + i.ToString()].Add(baseString + (i + 1).ToString(), new List<List<string>> { new List<string> { "LS2" } });
                }
            }

            // add all the special OW LS connections
            foreach (LSElevConnect connection in LSElevConnections) {
                string destination = ERScripts.FindPairedPortalRegionFromSDT(connection.Destination);
                List<List<string>> rules = new List<List<string>> { new List<string> { "LS" + connection.Difficulty.ToString() } };
                //traversalReqsWithLS[connection.Origin].SetDefault(destination, rules);
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
                    //traversalReqsWithLS.SetDefault(ladderInfo.Origin, new Dictionary<string, List<List<string>>> { { ladderInfo.Destination, rules } });
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


            Dictionary<string, PortalCombo> portalList;
            if (GetBool(EntranceRando)) {
                portalList = ERData.RandomizedPortals;
            } else {
                if (ERData.VanillaPortals.Count == 0) {
                    ERScripts.SetupVanillaPortals();
                }
                portalList = ERData.VanillaPortals;
            }

            // while we're here, let's just add the portal combo connections to the traversal reqs too for completion's sake
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
            //TunicLogger.LogInfo("Test start");
            //foreach (KeyValuePair<string, Dictionary<string, List<List<string>>>> keyValuePair in traversalReqsWithLS) {
            //    string originRegion = keyValuePair.Key;
            //    TunicLogger.LogInfo("-----------------------------------------------------------");
            //    TunicLogger.LogInfo("Origin region is " + originRegion);
            //    foreach (KeyValuePair<string, List<List<string>>> kvp in keyValuePair.Value) {
            //        string destRegion = kvp.Key;
            //        List<List<string>> rules = kvp.Value;
            //        TunicLogger.LogInfo("Destination region is " + destRegion);
            //        TunicLogger.LogInfo("Rules are as follows:");
            //        foreach (List<string> ruleSet in rules) {
            //            string ruleString = "";
            //            foreach (string rule in ruleSet) {
            //                ruleString += rule;
            //                ruleString += ", ";
            //            }
            //            TunicLogger.LogInfo(ruleString);
            //        }
            //        TunicLogger.LogInfo("-----------------------------------------------------------");
            //    }
            //    TunicLogger.LogInfo("===========================================================");
            //}

            return traversalReqsWithLS;
        }

    }
}
