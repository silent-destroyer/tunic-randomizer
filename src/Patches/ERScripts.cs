using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TunicRandomizer.ERData;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {
    public class ERScripts {
        public static GameObject storedPortal = null;
        // just for solving the case where old house door connects to itself in decoupled
        public static bool OldHouseDoorUnstuck = false;

        // returns an inventory of items and regions with the regions you can reach added in, does not traverse entrances
        public static Dictionary<string, int> UpdateReachableRegions(Dictionary<string, int> inventory) {
            int inv_count = inventory.Count;
            // for each origin region
            foreach (KeyValuePair<string, Dictionary<string, List<List<string>>>> traversal_group in ModifiedTraversalReqs) {
                string origin = traversal_group.Key;
                if (!inventory.ContainsKey(origin)) {
                    continue;
                }
                TunicLogger.LogTesting("checking traversal for " + origin);
                // for each destination in an origin's group
                foreach (KeyValuePair<string, List<List<string>>> destination_group in traversal_group.Value) {
                    string destination = destination_group.Key;
                    TunicLogger.LogTesting("checking traversal to " + destination);
                    // if we can already reach this region, skip it
                    if (inventory.ContainsKey(destination)) {
                        TunicLogger.LogTesting("we already have it");
                        continue;
                    }
                    // met is whether you meet any of the requirement lists for a destination
                    bool met = false;
                    if (destination_group.Value.Count == 0) {
                        TunicLogger.LogTesting("no requirement groups, met is true");
                        met = true;
                    }
                    // check through each list of requirements
                    int met_count = 0;
                    foreach (List<string> reqs in destination_group.Value) {
                        if (reqs.Count == 0) {
                            // if a group is empty, you can just walk there
                            met = true;
                            TunicLogger.LogTesting("group is empty, so met is true");
                        } else {
                            // check if we have the items in our inventory to traverse this path
                            met_count = 0;
                            foreach (string req in reqs) {
                                TunicLogger.LogTesting("req is " + req);
                                // if sword progression is on, check for this too
                                if (req == "Sword") {
                                    if ((inventory.ContainsKey("Sword Progression") && inventory["Sword Progression"] >= 2) || inventory.ContainsKey("Sword")) {
                                        met_count++;
                                    }
                                } else if (req == "Stick") {
                                    if (inventory.ContainsKey("Sword Progression") || inventory.ContainsKey("Stick") || inventory.ContainsKey("Sword")) {
                                        met_count++;
                                    }
                                } else if (req == "Heir Sword") {
                                    if (inventory.ContainsKey("Sword Progression") && inventory["Sword Progression"] >= 4) {
                                        met_count++;
                                    }
                                } else if (req == "12" && IsHexQuestWithHexAbilities()) {
                                    foreach (KeyValuePair<string, int> item in inventory) {
                                        if (item.Key == "Hexagon Gold") {
                                            if (item.Value >= SaveFile.GetInt($"randomizer hexagon quest prayer requirement")) {
                                                met_count++;
                                            }
                                            break;
                                        }
                                    }
                                } else if (req == "21" && IsHexQuestWithHexAbilities()) {
                                    foreach (KeyValuePair<string, int> item in inventory) {
                                        if (item.Key == "Hexagon Gold") {
                                            if (item.Value >= SaveFile.GetInt($"randomizer hexagon quest holy cross requirement")) {
                                                met_count++;
                                            }
                                            break;
                                        }
                                    }
                                } else if (req == "26" && IsHexQuestWithHexAbilities()) {
                                    foreach (KeyValuePair<string, int> item in inventory) {
                                        if (item.Key == "Hexagon Gold") {
                                            if (item.Value >= SaveFile.GetInt($"randomizer hexagon quest icebolt requirement")) {
                                                met_count++;
                                            }
                                            break;
                                        }
                                    }
                                } else if (req == "Key") {  // need both keys or you could potentially use them in the wrong order
                                    if (inventory.ContainsKey("Key") && inventory["Key"] == 2) {
                                        met_count++;
                                    }
                                } else if (inventory.ContainsKey(req)) {
                                    met_count++;
                                    TunicLogger.LogTesting("met_count is " + met_count);
                                    TunicLogger.LogTesting("reqs.count is " + reqs.Count);
                                    TunicLogger.LogTesting("we met this requirement");
                                }
                            }
                            // if you have all the requirements, you can traverse this path
                            if (met_count == reqs.Count) {
                                TunicLogger.LogTesting("met is true");
                                met = true;
                            }
                        }
                        // if we meet one list of requirements, we don't have to do the rest
                        if (met == true) {
                            break;
                        }
                    }
                    if (met == true) {
                        TunicLogger.LogTesting("adding " + destination + " to inventory");
                        inventory.Add(destination, 1);
                    } else {
                        TunicLogger.LogTesting("did not add " + destination + ", we did not meet the requirements");
                    }
                }
            }
            // if we gained any regions, rerun this to get any new regions
            if (inv_count != inventory.Count) {
                UpdateReachableRegions(inventory);
            }
            return inventory;
        }

        public static Dictionary<string, int> FirstStepsUpdateReachableRegions(Dictionary<string, int> inventory) {
            int inv_count = inventory.Count;
            // add all regions in Overworld that you can currently reach to the inventory
            foreach (KeyValuePair<string, Dictionary<string, List<List<string>>>> traversal_group in ModifiedTraversalReqs) {
                string origin_region = traversal_group.Key;
                if (!inventory.ContainsKey(origin_region)) {
                    continue;
                }
                foreach (KeyValuePair<string, List<List<string>>> destination_group in traversal_group.Value) {
                    string destination = destination_group.Key;
                    if (inventory.ContainsKey(destination)) {
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
                                if (inventory.ContainsKey(req)) {
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
                        inventory.Add(destination, 1);
                    }
                }
            }
            // if we gained any regions, rerun this to get any new regions
            if (inv_count != inventory.Count) {
                FirstStepsUpdateReachableRegions(inventory);
            }
            return inventory;
        }

        public static void SetupVanillaPortals() {
            ModifiedTraversalReqs = TraversalReqs;
            Dictionary<string, PortalCombo> portalCombos = new Dictionary<string, PortalCombo>();
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
            int count = 0;
            while (portalList.Count > 0) {
                Portal portal1 = portalList[0];
                Portal portal2 = new Portal("placeholder", "placeholder", "placeholder", "placeholder", "placeholder");  // I <3 csharp
                string portal2_sdt = portal1.DestinationSceneTag;
                int dir = (int)PDir.SOUTH;
                if (shop_num > 6) {
                    dir = (int)PDir.EAST;
                }
                if (portal2_sdt.StartsWith("Shop,")) {
                    portal2 = new Portal(name: "Shop Portal", destination: $"Previous Region {shop_num}", tag: "", scene: "Shop", region: $"Shop Entrance {shop_num}", direction: dir);
                    shop_num++;
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
                portalCombos.Add(count.ToString(), new PortalCombo(portal1, portal2));
                count++;
                portalCombos.Add(count.ToString(), new PortalCombo(portal2, portal1));
                count++;
            }
            ERData.VanillaPortals = portalCombos;
        }

        // create a list of all portals with their information loaded in, just a slightly expanded version of the above to include destinations
        public static void RandomizePortals(int seed) {
            TunicLogger.LogTesting("Randomizing portals");
            RandomizedPortals.Clear();
            Dictionary<string, PortalCombo> randomizedPortals = new Dictionary<string, PortalCombo>();
            // making a separate lists for portals connected to one, two, or three+ regions, to be populated by the foreach coming up next
            List<Portal> deadEndPortals = new List<Portal>();
            List<Portal> twoPlusPortals = new List<Portal>();
            ModifiedTraversalReqs = TraversalReqs;

            // keeping track of how many portals of each are left while pairing portals
            Dictionary<int, int> twoPlusPortalDirectionTracker = new Dictionary<int, int> { { (int)PDir.NORTH, 0 }, { (int)PDir.SOUTH, 0 }, { (int)PDir.EAST, 0 }, { (int)PDir.WEST, 0 }, { (int)PDir.FLOOR, 0 }, { (int)PDir.LADDER_DOWN, 0 }, { (int)PDir.LADDER_UP, 0 }, { (int)PDir.NONE, 0 } };
            Dictionary<int, int> deadEndPortalDirectionTracker = new Dictionary<int, int> { { (int)PDir.NORTH, 0 }, { (int)PDir.SOUTH, 0 }, { (int)PDir.EAST, 0 }, { (int)PDir.WEST, 0 }, { (int)PDir.FLOOR, 0 }, { (int)PDir.LADDER_DOWN, 0 }, { (int)PDir.LADDER_UP, 0 }, { (int)PDir.NONE, 0 } };
            // quick reference for which directions you can pair to which
            Dictionary<int, int> directionPairs = new Dictionary<int, int> { { (int)PDir.NORTH, (int)PDir.SOUTH }, { (int)PDir.SOUTH, (int)PDir.NORTH}, { (int)PDir.EAST, (int)PDir.WEST }, { (int)PDir.WEST, (int)PDir.EAST }, { (int)PDir.LADDER_UP, (int)PDir.LADDER_DOWN }, { (int)PDir.LADDER_DOWN, (int)PDir.LADDER_UP }, { (int)PDir.FLOOR, (int)PDir.FLOOR }, };

            // separate the portals into their respective lists
            foreach (KeyValuePair<string, Dictionary<string, List<TunicPortal>>> scene_group in RegionPortalsList) {
                string scene_name = scene_group.Key;
                if (scene_name == "Shop") {
                    continue;
                }
                foreach (KeyValuePair<string, List<TunicPortal>> region_group in scene_group.Value) {
                    // if fixed shop is off or decoupled is on, don't add zig skip exit to the portal list
                    string region_name = region_group.Key;
                        if (region_name == "Zig Skip Exit" && (SaveFile.GetInt(ERFixedShop) != 1 || SaveFile.GetInt(Decoupled) == 1)) {
                        continue;
                    }
                    List<TunicPortal> region_portals = region_group.Value;
                    foreach (TunicPortal tunicPortal in region_portals) {
                        Portal portal = new Portal(name: tunicPortal.Name, destination: tunicPortal.Destination, tag: tunicPortal.Tag, scene: scene_name, region: region_name, direction: tunicPortal.Direction);
                        if (RegionDict[region_name].DeadEnd == true) {
                            deadEndPortals.Add(portal);
                            deadEndPortalDirectionTracker[portal.Direction]++;
                        } else {
                            twoPlusPortals.Add(portal);
                            twoPlusPortalDirectionTracker[portal.Direction]++;
                        }
                        // need to throw fairy cave into the twoPlus list if laurels is at 10 fairies
                        if (portal.Region == "Secret Gathering Place" && SaveFile.GetInt(LaurelsLocation) == 3) {
                            deadEndPortals.Remove(portal);
                            twoPlusPortals.Add(portal);
                            twoPlusPortalDirectionTracker[portal.Direction]++;
                            deadEndPortalDirectionTracker[portal.Direction]--;
                        }
                    }
                }
            }
            TunicLogger.LogTesting("got through step 1 of entrance rando setup");

            // for keeping track of which regions and items are in logic during portal pairing
            Dictionary<string, int> FullInventory = new Dictionary<string, int>();
            // for the portal combos
            int comboNumber = 1000;

            // shops get added separately cause they're weird
            int shopCount = 6;
            if (SaveFile.GetInt(ERFixedShop) == 1) {
                shopCount = 0;
                Portal windmillPortal = null;
                foreach (Portal portal in twoPlusPortals) {
                    if (portal.SceneDestinationTag == "Overworld Redux, Windmill_") {
                        windmillPortal = portal;
                        twoPlusPortals.Remove(portal);
                        break;
                    }
                }
                Portal shopPortal = new Portal(name: "Shop Portal 1", destination: "Previous Region 1", tag: "", scene: "Shop", region: "Shop Entrance 1", direction: (int)PDir.SOUTH);
                randomizedPortals.Add(comboNumber.ToString(), new PortalCombo(windmillPortal, shopPortal));
                comboNumber++;
                
                // manually add the shop region to the inventory since it doesn't get added the normal way
                FullInventory.Add("Shop Entrance 1", 1);
            }
            if (SaveFile.GetInt(PortalDirectionPairs) == 1) {
                // need all 8 shops to match up everything nicely
                shopCount = 8;
            }
            int i = 1;
            while (i <= shopCount) {
                // 6 of the shops south, 2 of them are west in vanilla
                int dir = (int)PDir.SOUTH;
                if (i > 6) {
                    dir = (int)PDir.WEST;
                }
                // manually making a portal for the shop, because it has some special properties
                Portal shopPortal = new Portal(name: $"Shop Portal {i}", destination: $"Previous Region {i}", tag: "", scene: "Shop", region: $"Shop Entrance {i}", direction: dir);
                deadEndPortals.Add(shopPortal);
                i++;
                deadEndPortalDirectionTracker[dir]++;
            }

            // modify later if we ever do random start location
            string start_region = "Overworld";

            Dictionary<string, int> MaxItems = new Dictionary<string, int> {
                { "Stick", 1 }, { "Sword", 1 }, { "Wand", 1 }, { "Stundagger", 1 }, { "Techbow", 1 }, { "Gun", 1 }, { "Hyperdash", 1 }, { "Mask", 1 },
                { "Lantern", 1 }, { "12", 1 }, { "21", 1 }, { "26", 1 }, { "Key", 2 }, { "Key (House)", 1 }, { "Hexagon Gold", 50 }
            };

            // todo: swap this to use AddDictToDict in a later PR
            foreach (KeyValuePair<string, int> item in MaxItems) {
                FullInventory.Add(item.Key, item.Value);
            }
            // if laurels is at 10 fairies, remove laurels until the fairy cave is connected
            if (SaveFile.GetInt(LaurelsLocation) == 3) {
                FullInventory.Remove("Hyperdash");
            }
            FullInventory.Add(start_region, 1);
            FullInventory = TunicUtils.AddListToDict(FullInventory, ItemRandomizer.LadderItems);
            FullInventory = TunicUtils.AddListToDict(FullInventory, ItemRandomizer.FuseItems);
            FullInventory = TunicUtils.AddListToDict(FullInventory, ItemRandomizer.BellItems);
            if (GetBool(FuseShuffleEnabled)) {
                FullInventory.Add(FUSE_SHUFFLE, 1);
            } else {
                FullInventory.Add(NO_FUSE_SHUFFLE, 1);
            }
            if (GetBool(BellShuffleEnabled)) {
                FullInventory.Add(BELL_SHUFFLE, 1);
            } else {
                FullInventory.Add(NO_BELL_SHUFFLE, 1);
            }
            FullInventory = UpdateReachableRegions(FullInventory);

            // get the total number of regions to get before doing dead ends
            int total_nondeadend_count = 0;
            // used for generating useful error messages
            List<string> nondeadend_regions = new List<string>();
            foreach (KeyValuePair<string, RegionInfo> region in RegionDict) {
                // the region isn't an actual region anymore, since outlet regions exists now
                if (region.Key == "Zig Skip Exit") {
                    continue;
                }
                // in decoupled, dead ends aren't real, they can't hurt you
                if (region.Value.DeadEnd == false || SaveFile.GetInt(Decoupled) == 1) {
                    nondeadend_regions.Add(region.Key);
                    total_nondeadend_count++;
                }
            }
            // if you have decoupled on, you add all shop entrance regions, but you only have 6 shops with direction pairs off
            if (SaveFile.GetInt(Decoupled) == 1) {
                if (SaveFile.GetInt(ERFixedShop) == 1) {
                    // fixed shop only has 1 shop entrance region
                    total_nondeadend_count -= 7;
                } else if (SaveFile.GetInt(PortalDirectionPairs) != 1) {
                    // if fixed shop and direction pairs are both off, there's only 6 shop entrances
                    total_nondeadend_count -= 2;
                }   
            } else if (SaveFile.GetInt(ERFixedShop) == 1) {
                // if decoupled is off and fixed shop is on, the shop entrance is added to full inventory early, so the shop will make it in, inflating the count by 2
                total_nondeadend_count += 2;
            }
            // added fairy cave to the non-dead end regions, so it should increase the count here too
            if (SaveFile.GetInt(LaurelsLocation) == 3 && SaveFile.GetInt(Decoupled) != 1) {
                total_nondeadend_count++;
            }
            TunicLogger.LogTesting("step 2 of entrance rando setup done");

            // making a second list if we're doing decoupled, a reference to the first list if not
            List<Portal> twoPlusPortals2;
            if (SaveFile.GetInt(Decoupled) == 1) {
                // dead ends aren't real in decoupled
                twoPlusPortals.AddRange(deadEndPortals);
                deadEndPortals.Clear();
                twoPlusPortals2 = new List<Portal>(twoPlusPortals);
                if (SaveFile.GetInt(ERFixedShop) == 1) {
                    twoPlusPortals.Add(new Portal(name: "Shop Portal 1", destination: "Previous Region 1", tag: "", scene: "Shop", region: "Shop Entrance 1", direction: (int)PDir.SOUTH));
                    twoPlusPortals2.Add(new Portal(name: "Windmill Entrance", destination: "Windmill", tag: "", scene: "Overworld Redux", region: "Overworld", direction: (int)PDir.NORTH));
                }
            } else {
                twoPlusPortals2 = twoPlusPortals;
            }

            TunicLogger.LogTesting("created the secondary portal lists");

            bool TooFewPortalsForDirectionPairs(int direction, int offset = 0) {
                if (twoPlusPortalDirectionTracker[direction] <= deadEndPortalDirectionTracker[directionPairs[direction]] + offset) {
                    return false;
                }
                if (twoPlusPortalDirectionTracker[directionPairs[direction]] <= deadEndPortalDirectionTracker[direction] + offset) {
                    return false;
                }
                return true;
            }

            bool VerifyDirectionPair(Portal portal1, Portal portal2) {
                return portal1.Direction == directionPairs[portal2.Direction];
            }

            bool decoupledEnabled = SaveFile.GetInt(Decoupled) == 1;
            bool dirPairsEnabled = SaveFile.GetInt(PortalDirectionPairs) == 1;

            TunicUtils.ShuffleList(twoPlusPortals, seed);
            List<string> southProblems = new List<string> { "Ziggurat Upper to Ziggurat Entry Hallway", "Ziggurat Tower to Ziggurat Upper", "Forest Belltower to Guard Captain Room" };
            int failCount = 0;
            int previousConnNum = 0;
            while (FullInventory.Count - MaxItems.Count - ItemRandomizer.LadderItems.Count < total_nondeadend_count) {
                Portal portal1 = null;
                Portal portal2 = null;

                if (previousConnNum == FullInventory.Count) {
                    failCount++;
                    if (failCount > 500) {
                        TunicLogger.LogInfo("---------------------------------------");
                        TunicLogger.LogError("Failed to pair regions in ER.");
                        TunicLogger.LogInfo("If there are regions missing, they are as follows:");
                        foreach (string region in nondeadend_regions) {
                            if (!FullInventory.ContainsKey(region)) {
                                TunicLogger.LogInfo(region);
                            }
                        }
                        TunicLogger.LogInfo("---------------------------------------");
                        TunicLogger.LogInfo("Remaining portals in twoPlusPortals are:");
                        foreach (Portal debugportal in twoPlusPortals) {
                            TunicLogger.LogInfo(debugportal.Name);
                        }
                        TunicLogger.LogInfo("---------------------------------------");
                        TunicLogger.LogInfo("The contents of FullInventory are:");
                        foreach (string thing in FullInventory.Keys) {
                            TunicLogger.LogInfo(thing);
                        }
                        //throw new System.Exception("Failed to pair portals.");
                        // reroll, hopefully this shouldn't be common at all
                        RandomizePortals(seed + 1);
                        return;
                    }
                } else {
                    failCount = 0;
                }
                previousConnNum = FullInventory.Count;

                // find a portal in a region we can currently reach
                foreach (Portal portal in twoPlusPortals) {
                    if (FullInventory.ContainsKey(portal.Region)) {
                        // if direction pairs is enabled, we need to make sure we're not taking up too many portals in one direction
                        if (!decoupledEnabled && dirPairsEnabled) {
                            if (!TooFewPortalsForDirectionPairs(portal.Direction)) {
                                continue;
                            }
                        }
                        portal1 = portal;
                        twoPlusPortals.Remove(portal);
                        break;
                    }
                }

                if (portal1 == null) {
                    // it will fail after this
                    TunicLogger.LogError("something messed up in portal pairing for portal 1");
                    TunicLogger.LogInfo("current region count is " + (FullInventory.Count - MaxItems.Count - ItemRandomizer.LadderItems.Count).ToString());
                    TunicLogger.LogInfo("goal region count is " + total_nondeadend_count.ToString());
                    TunicLogger.LogInfo("if there are regions missing, they are as follows:");
                    foreach (string region in nondeadend_regions) {
                        if (!FullInventory.ContainsKey(region)) {
                            TunicLogger.LogInfo(region);
                        }
                    }
                    TunicLogger.LogInfo("remaining portals in twoPlusPortals are:");
                    foreach (Portal debugportal in twoPlusPortals) {
                        TunicLogger.LogInfo(debugportal.Name);
                    }
                    // reroll, hopefully this shouldn't be common at all
                    RandomizePortals(seed + 1);
                    return;
                }

                foreach (Portal portal in twoPlusPortals2) {
                    if (!FullInventory.ContainsKey(portal.Region)) {
                        // if secret gathering place gets paired really late and you have the laurels plando on, you can run out pretty easily
                        if (twoPlusPortals2.Count < 80 && portal.Region != "Secret Gathering Place" && !FullInventory.ContainsKey("Hyperdash") && SaveFile.GetInt(LaurelsLocation) == 3) {
                            TunicLogger.LogTesting("Continuing to wait for Secret Gathering Place");
                            continue;
                        }
                        if (dirPairsEnabled && !VerifyDirectionPair(portal1, portal)) {
                            continue;
                        }
                        if (!decoupledEnabled && dirPairsEnabled) {
                            bool shouldContinue = false;
                            // the south problem portals are all effectively one ways
                            southProblems.RemoveAll(item => FullInventory.ContainsKey(item));
                            if (portal.Direction == (int)PDir.SOUTH && !southProblems.Contains(portal.Name) && southProblems.Count > 0 && !TooFewPortalsForDirectionPairs(portal.Direction, offset: southProblems.Count)) {
                                foreach (Portal testPortal in twoPlusPortals) {
                                    if (southProblems.Contains(testPortal.Name)) {
                                        TunicLogger.LogTesting("Will continue to avoid south problems");
                                        shouldContinue = true;
                                    }
                                }
                            }
                            if (portal.Direction == (int)PDir.LADDER_DOWN
                                || portal.Direction == (int)PDir.LADDER_UP && portal.Name != "Frog's Domain Ladder Exit" && !TooFewPortalsForDirectionPairs(portal.Direction, offset: 1)) {
                                foreach (Portal testPortal in twoPlusPortals) {
                                    if (testPortal.Name == "Frog's Domain Ladder Exit") {
                                        TunicLogger.LogTesting("Will continue to avoid Frog's Domain Ladder Exit issue");
                                        shouldContinue = true;
                                    }
                                }
                            }
                            if (shouldContinue) {
                                continue;
                            }
                        }
                        portal2 = portal;
                        if (!FullInventory.ContainsKey(portal2.OutletRegion())) {
                            FullInventory.Add(portal2.OutletRegion(), 1);
                        }
                        twoPlusPortals2.Remove(portal);
                        break;
                    }
                }
                if (portal2 == null) {
                    if (dirPairsEnabled) {
                        twoPlusPortals.Add(portal1);
                        continue;
                    } else {
                        TunicLogger.LogInfo("---------------------------------------");
                        TunicLogger.LogError("something messed up in portal pairing for portal 2");
                        // reroll, hopefully this shouldn't be common at all
                        RandomizePortals(seed + 1);
                        return;
                    }
                }

                FullInventory = UpdateReachableRegions(FullInventory);

                // add the portal combo to the randomized portals list
                randomizedPortals.Add(comboNumber.ToString(), new PortalCombo(portal1, portal2));
                twoPlusPortalDirectionTracker[portal1.Direction]--;
                twoPlusPortalDirectionTracker[portal2.Direction]--;
                comboNumber++;

                if (!FullInventory.ContainsKey(portal1.OutletRegion())) {
                    FullInventory.Add(portal1.OutletRegion(), 1);
                }
                // if laurels is at fairy cave, add it when we connect fairy cave
                if (portal2.Region == "Secret Gathering Place" && SaveFile.GetInt(LaurelsLocation) == 3) {
                    TunicLogger.LogTesting("Adding the laurels, aka hyperdash, to the inventory");
                    FullInventory.Add("Hyperdash", 1);
                }
                
                TunicUtils.ShuffleList(twoPlusPortals, seed);
                if (twoPlusPortals != twoPlusPortals2) {
                    TunicUtils.ShuffleList(twoPlusPortals2, seed);
                }
            }
            TunicLogger.LogTesting("done pairing twoplusportals");

            // if we run into issues again, these were very helpful in diagnosing them
            //TunicLogger.LogInfo("if there are regions missing, they are as follows:");
            //foreach (string region in nondeadend_regions) {
            //    if (!FullInventory.ContainsKey(region)) {
            //        TunicLogger.LogInfo(region);
            //    }
            //}
            //TunicLogger.LogInfo("inventory:");
            //foreach (string thing in FullInventory.Keys) {
            //    TunicLogger.LogInfo(thing);
            //}

            // since the dead ends only have one exit, we just append them 1 to 1 to a random portal in the two plus list
            TunicUtils.ShuffleList(deadEndPortals, seed);
            TunicUtils.ShuffleList(twoPlusPortals, seed);

            // pair dead end portals to non-dead end portals
            while (deadEndPortals.Count > 0) {
                Portal portal1 = deadEndPortals[0];
                bool foundPair = false;
                foreach (Portal portal2 in twoPlusPortals) {
                    if (SaveFile.GetInt(PortalDirectionPairs) == 1 && directionPairs[portal1.Direction] != portal2.Direction) {
                        continue;
                    }
                    randomizedPortals.Add(comboNumber.ToString(), new PortalCombo(portal1, portal2));
                    comboNumber++;

                    twoPlusPortals.Remove(portal2);
                    deadEndPortals.RemoveAt(0);
                    foundPair = true;
                    break;
                }
                if (foundPair == false) {
                    TunicUtils.ShuffleList(deadEndPortals, seed);
                }
            }

            // now we have every region accessible
            // the twoPlusPortals list still has items left in it, so now we pair them off
            int finalPairLoopNumber = 0;
            while (twoPlusPortals.Count > 0) {
                finalPairLoopNumber++;
                if (finalPairLoopNumber > 10000) {
                    TunicLogger.LogError("Failed to pair portals while pairing the final entrances off to each other");
                    TunicLogger.LogInfo("Remaining portals in twoPlusPortals:");
                    foreach (Portal portal in twoPlusPortals) {
                        TunicLogger.LogInfo(portal.Name);
                    }
                    TunicLogger.LogInfo("Remaining portals in twoPlusPortals2:");
                    foreach (Portal portal in twoPlusPortals2) {
                        TunicLogger.LogInfo(portal.Name);
                    }
                    // reroll, hopefully this shouldn't be common at all
                    RandomizePortals(seed + 1);
                    return;
                }
                Portal portal1 = twoPlusPortals[0];
                twoPlusPortals.RemoveAt(0);
                Portal portal2 = null;
                if (!dirPairsEnabled) {
                    portal2 = twoPlusPortals2[0];
                    twoPlusPortals2.RemoveAt(0);
                } else {
                    foreach (Portal portal in twoPlusPortals2) {
                        if (directionPairs[portal1.Direction] == portal.Direction) {
                            portal2 = portal;
                            twoPlusPortals2.Remove(portal);
                            break;
                        }
                    }
                }
                if (portal2 == null) {
                    // it will fail after this
                    TunicLogger.LogInfo("---------------------------------------");
                    TunicLogger.LogError("something went wrong with the remaining two plus portals");
                    TunicLogger.LogInfo("portal 1 is: " + portal1.Name);
                    TunicLogger.LogInfo("remaining portals in twoPlusPortals are:");
                    foreach (Portal debugportal in twoPlusPortals) {
                        TunicLogger.LogInfo(debugportal.Name);
                    }
                    foreach (PortalCombo combo in randomizedPortals.Values) {
                        if (combo.Portal1.Direction == portal1.Direction || combo.Portal2.Direction == portal1.Direction) {
                            TunicLogger.LogInfo("Possible error combo: " + combo.Portal1.Name + " and " + combo.Portal2.Name);
                        }
                    }
                    //throw new System.Exception("Failed to pair portals in the end step");
                    // reroll, hopefully this shouldn't be common at all
                    RandomizePortals(seed + 1);
                    return;
                }
                randomizedPortals.Add(comboNumber.ToString(), new PortalCombo(portal1, portal2));
                comboNumber++;
            }

            // now we add it to the actual, kept portal list, based on whether decoupled is on
            foreach (KeyValuePair<string, PortalCombo> portalCombo in randomizedPortals) {
                RandomizedPortals.Add(portalCombo.Key, portalCombo.Value);
                if (SaveFile.GetInt(Decoupled) != 1) {
                    RandomizedPortals.Add(comboNumber.ToString(), new PortalCombo(portalCombo.Value.Portal2, portalCombo.Value.Portal1));
                    comboNumber++;
                }
            }
        }

        // shops will be formatted like "Shop, 3_" for Shop Portal 3, instead of the current "Shop, Previous Region_"
        // it also needs to be able to handle an arbitrary number of shops, up to like 500 maybe
        // this is for using the info from Archipelago to pair up the portals
        public static void CreatePortalPairs(Dictionary<string, string> APPortalStrings) {
            RandomizedPortals.Clear();
            List<Portal> portalsList = new List<Portal>();
            // equivalent of doing TraversalReqs.copy() in python
            ModifiedTraversalReqs = TraversalReqs.ToDictionary(entry => entry.Key, entry => entry.Value);
            int comboNumber = 1000;

            foreach (KeyValuePair<string, Dictionary<string, List<TunicPortal>>> scene_group in RegionPortalsList) {
                string scene_name = scene_group.Key;
                foreach (KeyValuePair<string, List<TunicPortal>> region_group in scene_group.Value) {
                    string region_name = region_group.Key;
                    List<TunicPortal> region_portals = region_group.Value;
                    foreach (TunicPortal tunicPortal in region_portals) {
                        Portal portal = new Portal(name: tunicPortal.Name, destination: tunicPortal.Destination, tag: tunicPortal.Tag, scene: scene_name, region: region_name, direction: tunicPortal.Direction);
                        portalsList.Add(portal);
                    }
                }
            }
            // for backwards compatibility, we want to mark all shop portals with a number as we see them
            int backwards_compat_shop_num = 1;
            // make the PortalCombo dictionary
            foreach (KeyValuePair<string, string> stringPair in APPortalStrings) {
                string portal1SDT = stringPair.Key;
                string portal2SDT = stringPair.Value;
                Portal portal1 = null;
                Portal portal2 = null;
                // for backwards compatibility with older apworlds that don't have numbered shop portals
                if (portal2SDT == "Shop, Previous Region_") {
                    portal2 = new Portal(name: $"Shop Portal {backwards_compat_shop_num}", destination: $"Previous Region {backwards_compat_shop_num}", tag: "", scene: "Shop", region: $"Shop Entrance {backwards_compat_shop_num}", direction: (int)PDir.NONE);
                    backwards_compat_shop_num++;
                }
                foreach (Portal portal in portalsList) {
                    if (portal1 != null && portal2 != null) {
                        break;
                    }
                    if (portal1 == null && portal1SDT == portal.SceneDestinationTag) {
                        portal1 = portal;
                    }
                    if (portal2 == null && portal2SDT == portal.SceneDestinationTag) {
                        portal2 = portal;
                    }
                }
                // if it's null still, it's a shop
                if (portal1 == null) {
                    string shop_num = new string(portal1SDT.Where(char.IsDigit).ToArray());
                    int dir = (int)PDir.SOUTH;
                    if (shop_num == "7" || shop_num == "8") {
                        dir = (int)PDir.WEST;
                    }
                    portal1 = new Portal(name: $"Shop Portal {shop_num}", destination: $"Previous Region {shop_num}", tag: "", scene: "Shop", region: $"Shop Entrance {shop_num}", direction: dir);
                    if (!ModifiedTraversalReqs.ContainsKey($"Shop Entrance {shop_num}")) {
                        ModifiedTraversalReqs.Add($"Shop Entrance {shop_num}", new Dictionary<string, List<List<string>>> { { "Shop", new List<List<string>> { } } });
                    }
                }
                if (portal2 == null) {
                    string shop_num = new string(portal2SDT.Where(char.IsDigit).ToArray());
                    int dir = (int)PDir.SOUTH;
                    if (shop_num == "7" || shop_num == "8") {
                        dir = (int)PDir.WEST;
                    }
                    portal2 = new Portal(name: $"Shop Portal {shop_num}", destination: $"Previous Region {shop_num}", tag: "", scene: "Shop", region: $"Shop Entrance {shop_num}", direction: dir);
                    if (!ModifiedTraversalReqs.ContainsKey($"Shop Entrance {shop_num}")) {
                        ModifiedTraversalReqs.Add($"Shop Entrance {shop_num}", new Dictionary<string, List<List<string>>> { { "Shop", new List<List<string>> { } } });
                    }
                }
                PortalCombo portalCombo = new PortalCombo(portal1, portal2);
                RandomizedPortals.Add(comboNumber.ToString(), portalCombo);
                comboNumber++;
                // if decoupled is off, we want to make reverse pairs
                if (SaveFile.GetInt(Decoupled) != 1) {
                    PortalCombo reversePortalCombo = new PortalCombo(portal2, portal1);
                    RandomizedPortals.Add(comboNumber.ToString(), reversePortalCombo);
                    comboNumber++;
                }
            }
        }

        // a function to apply the randomized portal list to portals during onSceneLoaded
        public static void ModifyPortals(string scene_name, bool sending = false) {
            // we turn this off to not let you walk back through before it is modified the second time
            if (sending == true && storedPortal != null) {
                // if the old house door paired itself, move the fox outside the door so they don't get locked in an infinite loop, even though that's kinda funny
                if (OldHouseDoorUnstuck == true) {
                    PlayerCharacter.instance.transform.position = new Vector3(-32, 31, -59);
                    PlayerCharacter.instance.transform.rotation = new Quaternion(0, 180, 0, 0);
                    OldHouseDoorUnstuck = false;
                }
                storedPortal.GetComponent<BoxCollider>().isTrigger = true;
                storedPortal = null;
            }
            // if we're spawning at a shop, we need to create a custom ScenePortal to receive you
            if (sending == false && scene_name == "Shop") {
                var ShopPortals = Resources.FindObjectsOfTypeAll<ScenePortal>().Where(portal => portal.gameObject.scene.name == SceneManager.GetActiveScene().name
                    && !portal.FullID.Contains("customfasttravel") && !portal.id.Contains("customfasttravel"));
                foreach (var shopPortal in ShopPortals) {
                    if (shopPortal.TryCast<ShopScenePortal>() != null) {
                        GameObject newPortal = new GameObject("Custom Rando Shop Portal");
                        GameObject newSpawn = new GameObject("Spawn point");
                        newSpawn.transform.parent = newPortal.transform;
                        newPortal.AddComponent<ScenePortal>();
                        // find which portal combo led to this shop, also flip the shop if it should be flipped
                        foreach (KeyValuePair<string, PortalCombo> portalCombo in RandomizedPortals) {
                            if (PlayerCharacterSpawn.portalIDToSpawnAt.EndsWith(portalCombo.Key)) {
                                TunicLogger.LogTesting("Portal 1 is " + portalCombo.Value.Portal1.Name);
                                TunicLogger.LogTesting("Portal 2 is " + portalCombo.Value.Portal2.Name);
                                newPortal.GetComponent<ScenePortal>().destinationSceneName = portalCombo.Value.Portal2.Destination;
                                newPortal.GetComponent<ScenePortal>().id = portalCombo.Value.Portal2.Tag;
                                if (portalCombo.Value.FlippedShop() == true) {
                                    TunicLogger.LogTesting("Flipped shop true");
                                    shopPortal.TryCast<ShopScenePortal>().flippedCameraZone.enabled = true;
                                } else {
                                    TunicLogger.LogTesting("Flipped shop false");
                                    shopPortal.TryCast<ShopScenePortal>().flippedCameraZone.enabled = false;
                                }
                            }
                        }
                        // I don't actually know if all of these are necessary, but it does work with all of them so
                        newPortal.transform.position = shopPortal.transform.position;
                        newPortal.transform.localPosition = shopPortal.transform.localPosition;
                        newPortal.transform.rotation = shopPortal.transform.rotation;
                        newPortal.transform.localRotation = shopPortal.transform.localRotation;
                        newPortal.transform.localScale = shopPortal.transform.localScale;

                        newSpawn.transform.position = shopPortal.transform.GetChild(0).transform.position;
                        newSpawn.transform.localPosition = shopPortal.transform.GetChild(0).transform.localPosition;
                        newSpawn.transform.rotation = shopPortal.transform.GetChild(0).transform.rotation;
                        newSpawn.transform.localRotation = shopPortal.transform.GetChild(0).transform.localRotation;
                        newPortal.GetComponent<ScenePortal>().spawnTransform = newSpawn.transform;
                        newPortal.GetComponent<BoxCollider>().size = new Vector3(2, 2, 2);
                        newPortal.layer = 2;
                        newPortal.GetComponent<BoxCollider>().isTrigger = true;
                        // now that we've copied over the relevant info, disable it
                        shopPortal.gameObject.SetActive(false);
                    }
                }
            }

            var Portals = Resources.FindObjectsOfTypeAll<ScenePortal>().Where(portal => portal.gameObject.scene.name == SceneManager.GetActiveScene().name
                && !portal.FullID.Contains("customfasttravel") && !portal.id.Contains("customfasttravel"));
            foreach (var portal in Portals) {
                // skips the extra west garden shop portal
                if (!portal.isActiveAndEnabled) {
                    continue;
                }
                // portal for stopping you from dying in zig skip
                if (portal.FullID == "ziggurat2020_3_zig2_skip") {
                    portal.name = "Zig Skip";
                    portal.destinationSceneName = "ziggurat2020_1";
                    portal.optionalIDToSpawnAt = "zig_skip_recovery";
                    continue;
                }

                // go through the list of randomized portals and see if the first portal matches the one we're looking at
                foreach (KeyValuePair<string, PortalCombo> portalCombo in RandomizedPortals) {
                    string comboTag = portalCombo.Key;
                    Portal portal1 = portalCombo.Value.Portal1;
                    Portal portal2 = portalCombo.Value.Portal2;

                    if (sending == false) {
                        if (portal2.Scene == scene_name && portal2.DestinationTag == portal.FullID) {
                            portal.destinationSceneName = portal1.Scene;
                            portal.id = comboTag;
                            portal.optionalIDToSpawnAt = comboTag + comboTag;
                            portal.name = portal2.Name;
                            // if you're spawning there, we need to make sure you can't walk back out before the arrival callback
                            if (PlayerCharacterSpawn.portalIDToSpawnAt == portal.FullID 
                                && (portal.transform.parent?.name != "FT Platform Animator" || portal.transform.parent == null)) {
                                storedPortal = portal.gameObject;
                                storedPortal.GetComponent<BoxCollider>().isTrigger = false;

                                // there's a few spots where you can get stuck if you ladder storage or ice grapple into the old house door
                                // so if you manage to get one of these rare combinations, throw the fox out the window
                                // Defennecstration -hatkirby
                                if (portal2.SceneDestinationTag == "Overworld Redux, Overworld Interiors_house" && SaveFile.GetInt("SV_Overworld_House Door Opened") != 1) {
                                    if (portal1.SceneDestinationTag == "Overworld Redux, Overworld Interiors_house") {
                                        OldHouseDoorUnstuck = true;
                                    }

                                    Dictionary<string, List<string>> problemSpots = new Dictionary<string, List<string>> {
                                        {"Ladders in Library",
                                            new List<string> { "Library Hall, Library Rotunda_", "Library Rotunda, Library Hall_", "Library Rotunda, Library Lab_",
                                                "Library Lab, Library Rotunda_", "Library Lab, Library Arena_" } },
                                        {"Ladders in Well",
                                            new List<string> { "Overworld Redux, Sewer_entrance", "Sewer, Overworld Redux_entrance" } },
                                        {"Ladders to Frog's Domain",
                                            new List<string> {"Frog Stairs, frog cave main_Entrance", "frog cave main, Frog Stairs_Entrance"} },
                                    };
                                    if (SaveFile.GetInt(LadderRandoEnabled) == 1) {
                                        foreach (KeyValuePair<string, List<string>> problems in problemSpots) {
                                            string ladderName = problems.Key;
                                            List<string> ladderPortals = problems.Value;
                                            // if the old house door connects to a problem spot and you don't have the ladder
                                            if (ladderPortals.Contains(portal1.SceneDestinationTag)
                                            && (SaveFile.GetInt($"randomizer picked up {ladderName}") != 1)) {
                                                // if you're not in decoupled, then we can be sure this is a problem
                                                if (SaveFile.GetInt(Decoupled) != 1) {
                                                    OldHouseDoorUnstuck = true;
                                                } else {
                                                    // if you're in decoupled, we want to make sure the door leads to a problem spot, not just from a problem spot
                                                    foreach (KeyValuePair<string, PortalCombo> portalCombo2 in RandomizedPortals) {
                                                        if (portalCombo2.Value.Portal1.Name == "Overworld Redux, Overworld Interiors_house") {
                                                            if (ladderPortals.Contains(portalCombo2.Value.Portal2.SceneDestinationTag)) {
                                                                OldHouseDoorUnstuck = true;
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            break;
                        }
                    } else {
                        // sending is true, so we want to grab all portals by name (since they were modified when sending == false) and change their destinations
                        if (portal1.Name == portal.name) {
                            portal.destinationSceneName = portal2.Scene;
                            portal.id = comboTag + comboTag;
                            portal.optionalIDToSpawnAt = comboTag;
                            break;
                        }
                    }
                }
            }
        }

        // for non-ER, just modifies the portal names -- this is useful for the FairyTargets for the entrance seeking spell
        public static void ModifyPortalNames(string scene_name) {
            var Portals = Resources.FindObjectsOfTypeAll<ScenePortal>().Where(portal => portal.gameObject.scene.name == SceneManager.GetActiveScene().name
            && !portal.FullID.Contains("customfasttravel") && !portal.id.Contains("customfasttravel"));
            foreach (var portal in Portals) {
                // skips the extra west garden shop portal
                if (!portal.isActiveAndEnabled) {
                    continue;
                }
                // go through the list of randomized portals and see if the first portal matches the one we're looking at
                foreach (KeyValuePair<string, PortalCombo> portalCombo in ERData.VanillaPortals) {
                    Portal portal1 = portalCombo.Value.Portal1;
                    Portal portal2 = portalCombo.Value.Portal2;

                    if (portal1.Scene == scene_name && portal1.DestinationTag == portal.FullID) {
                        portal.name = portal1.Name;
                        break;
                    }
                }
            }
        }

        // this marks a portal as checked, for the entrance seeking spell and for datastorage for the eventual poptracker update
        public static void MarkPortals() {
            var Portals = Resources.FindObjectsOfTypeAll<ScenePortal>().Where(portal => portal.gameObject.scene.name == SceneManager.GetActiveScene().name
            && !portal.FullID.Contains("customfasttravel") && !portal.id.Contains("customfasttravel"));

            foreach (var portal in Portals) {
                // skips the extra west garden shop portal
                if (!portal.isActiveAndEnabled) {
                    continue;
                }
                if (portal.FullID == PlayerCharacterSpawn.portalIDToSpawnAt) {
                    foreach (KeyValuePair<string, PortalCombo> portalCombo in RandomizedPortals) {
                        if (portal.name == portalCombo.Value.Portal2.Name && (portal.name != "Shop Portal" || (portal.name == "Shop Portal" && portalCombo.Value.Portal1.Scene == SceneManager.GetActiveScene().name))) {
                            if (SaveFile.GetInt("randomizer entered portal " + portalCombo.Value.Portal1.Name) == 0) {
                                SaveFile.SetInt("randomizer entered portal " + portalCombo.Value.Portal1.Name, 1);
                                if (IsArchipelago()) {
                                    Archipelago.instance.integration.UpdateDataStorage($"{portalCombo.Value.Portal1.Scene}, {portalCombo.Value.Portal1.Destination}{portalCombo.Value.Portal1.Tag}", true);
                                }
                            }
                            // if decoupled is off, we can just mark the other side of the portal too
                            if (SaveFile.GetInt(Decoupled) != 1) {
                                if (SaveFile.GetInt("randomizer entered portal " + portalCombo.Value.Portal2.Name) == 0) {
                                    SaveFile.SetInt("randomizer entered portal " + portalCombo.Value.Portal2.Name, 1);
                                    if (IsArchipelago()) {
                                        Archipelago.instance.integration.UpdateDataStorage($"{portalCombo.Value.Portal2.Scene}, {portalCombo.Value.Portal2.Destination}{portalCombo.Value.Portal2.Tag}", true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static string FindPortalRegionFromName(string portalName) {
            foreach (Dictionary<string, List<TunicPortal>> regionGroups in RegionPortalsList.Values) {
                foreach (KeyValuePair<string, List<TunicPortal>> regionGroup in regionGroups) {
                    string regionName = regionGroup.Key;
                    foreach (TunicPortal portal in  regionGroup.Value) {
                        if (portal.Name == portalName) {
                            return regionName;
                        }
                    }
                }
            }
            // returning this if it fails, since that makes some FairyTarget stuff easier
            return "FindPortalRegionFromName failed to find a match";
        }

        public static string FindPairedPortalSceneFromName(string portalName) {
            foreach (PortalCombo portalCombo in RandomizedPortals.Values) {
                if (portalCombo.Portal1.Name == portalName) {
                    return portalCombo.Portal2.Scene;
                }
                if (portalCombo.Portal2.Name == portalName) {
                    return portalCombo.Portal1.Scene;
                }
            }
            // returning this if it fails, since that makes some FairyTarget stuff easier
            return "FindPairedPortalSceneFromName failed to find a match";
        }
        
    }

    public class FoxgodDecoupledTeleporter : MonoBehaviour {

        public Foxgod foxgod;

        public void Awake() {
            foxgod = Resources.FindObjectsOfTypeAll<Foxgod>().Where(foxgod => foxgod.gameObject.scene.name == "Spirit Arena").ToList()[0];
        }

        public void Update() {
            if (foxgod != null && foxgod.monsterAggroed) {
                if (foxgod.hp < foxgod.maxhp) {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
