using System;
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
        public static Dictionary<string, string> PlandoPortals = new Dictionary<string, string>();

        // returns an inventory of items and regions with the regions you can reach added in, does not traverse entrances
        public static Dictionary<string, int> UpdateReachableRegions(Dictionary<string, int> inventory) {
            if (TunicLogger.Testing) {
                TunicLogger.LogTesting("Starting UpdateReachableRegions, current inventory is as follows:");
                foreach (string itemName in inventory.Keys) {
                    TunicLogger.LogTesting(itemName);
                }
            }
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
                                if (TunicUtils.HasReq(req, inventory)) {
                                    met_count++;
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


        public static (Dictionary<string, int>, List<Check>) UpdateReachableRegionsAndPickUpItems(Dictionary<string, int> inventory, List<Check> alreadyCheckedLocations = null) {
            if (alreadyCheckedLocations == null) {
                alreadyCheckedLocations = new List<Check>();
            }
            while (true) {
                // start count is the quantity of items in full inventory. If this stays the same between loops, then you are done getting items
                int start_count = 0;
                foreach (int count in inventory.Values) {
                    start_count += count;
                }

                while (true) {
                    // since regions always have a count of 1, we can just use .count instead of counting up all the values
                    int start_num = inventory.Count;
                    inventory = UpdateReachableRegions(inventory);
                    foreach (PortalCombo portalCombo in RandomizedPortals) {
                        inventory = portalCombo.AddComboRegion(inventory);
                    }
                    if (start_num == inventory.Count) {
                        break;
                    }
                }

                // pick up all items you can reach with your current inventory
                foreach (Check placedLocation in ItemRandomizer.ProgressionLocations.Values) {
                    if (!alreadyCheckedLocations.Contains(placedLocation) && placedLocation.Location.reachable(inventory)) {
                        string item_name = ItemLookup.FairyLookup.Keys.Contains(placedLocation.Reward.Name) ? "Fairy" : placedLocation.Reward.Name;
                        TunicUtils.AddStringToDict(inventory, item_name);
                        alreadyCheckedLocations.Add(placedLocation);
                    }
                }

                int end_count = 0;
                foreach (int count in inventory.Values) {
                    end_count += count;
                }

                // if these two are equal, then we've gotten everything we have access to
                if (start_count == end_count) {
                    break;
                }
            }
            return (inventory, alreadyCheckedLocations);
        }


        public static bool CanUsePortal(Portal portal, Dictionary<string, int> inventory) {
            if (inventory.ContainsKey(portal.Region)) {
                return true;
            }

            // trick logic is weird, it's probably easier to manually check like this
            int lsdiff = SaveFile.GetInt(LadderStorageDifficulty);
            if (lsdiff >= 1 && TunicUtils.HasReq("LS1", inventory)) {
                foreach (TrickLogic.LSElevConnect cxn in TrickLogic.OWLSElevConnections) {
                    if (portal.SceneDestinationTag != cxn.Destination) continue;
                    if (!inventory.ContainsKey(cxn.Origin)) continue;
                    if (lsdiff >= cxn.Difficulty) {
                        return true;
                    }
                }
                foreach (TrickLogic.LadderInfo cxn in TrickLogic.EasyLS) {
                    if (portal.SceneDestinationTag != cxn.Destination) continue;
                    if (!inventory.ContainsKey(cxn.Origin)) continue;
                    if (cxn.LaddersReq == null || TunicUtils.HasReq(cxn.LaddersReq, inventory)) {
                        return true;
                    }
                }
                if (lsdiff >= 2) {
                    foreach (TrickLogic.LadderInfo cxn in TrickLogic.MediumLS) {
                        if (portal.SceneDestinationTag != cxn.Destination) continue;
                        if (!inventory.ContainsKey(cxn.Origin)) continue;
                        if (cxn.LaddersReq == null || TunicUtils.HasReq(cxn.LaddersReq, inventory)) {
                            return true;
                        }
                    }
                }
                if (lsdiff >= 3) {
                    foreach (TrickLogic.LadderInfo cxn in TrickLogic.HardLS) {
                        if (portal.SceneDestinationTag != cxn.Destination) continue;
                        if (!inventory.ContainsKey(cxn.Origin)) continue;
                        if (cxn.LaddersReq == null || TunicUtils.HasReq(cxn.LaddersReq, inventory)) {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        public static void SetupVanillaPortals() {
            List<PortalCombo> portalCombos = new List<PortalCombo>();
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
                        portalList.Add(new Portal(name: tunicPortal.Name, destination: tunicPortal.Destination, tag: tunicPortal.Tag, scene: scene_name, region: region_name, isDeadEnd: true));  // dead end doesn't matter to vanilla portals
                    }
                }
            }
            int count = 0;
            while (portalList.Count > 0) {
                Portal portal1 = portalList[0];
                Portal portal2 = new Portal("placeholder", "placeholder", "placeholder", "placeholder", "placeholder", false);
                string portal2_sdt = portal1.DestinationSceneTag;
                int dir = (int)PDir.SOUTH;
                if (shop_num > 6) {
                    dir = (int)PDir.EAST;
                }
                if (portal2_sdt.StartsWith("Shop,")) {
                    portal2 = new Portal(name: "Shop Portal", destination: $"Previous Region {shop_num}", tag: "", scene: "Shop", region: $"Shop Entrance {shop_num}", direction: dir, isDeadEnd: true);
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
                portalCombos.Add(new PortalCombo(portal1, portal2));
                count++;
                portalCombos.Add(new PortalCombo(portal2, portal1));
                count++;
            }
            ERData.VanillaPortals = portalCombos;
        }

        public static void SetupVanillaPortalsAndTraversalReqs() {
            if (VanillaPortals.Count == 0) {
                SetupVanillaPortals();
            }
            ModifiedTraversalReqs = TrickLogic.TraversalReqsWithLS(TunicUtils.DeepCopyTraversalReqs());
        }

        public static void CreateRandomizedPortals(int seed) {
            TunicLogger.LogTesting("Randomizing portals");
            RandomizedPortals.Clear();
            FoxPrince.BPRandomizedPortals.Clear();
            ModifiedTraversalReqs = TunicUtils.DeepCopyTraversalReqs();

            List<PortalCombo> randomizedPortals = RandomizePortals(seed);

            // now we add it to the actual, kept portal list, based on whether decoupled is on
            foreach (PortalCombo portalCombo in randomizedPortals) {
                RandomizedPortals.Add(portalCombo);
                if (!GetBool(Decoupled)) {
                    RandomizedPortals.Add(new PortalCombo(portalCombo.Portal2, portalCombo.Portal1));
                }
            }
            ModifiedTraversalReqs = TrickLogic.TraversalReqsWithLS(ModifiedTraversalReqs);
        }


        // randomize the portals with logic to ensure all regions are reachable
        // deplando is for Fox Pricne, basically it says "this portal cannot connect to this other portal"
        // canFail is to say whether it should try to reroll or just return null if it reaches a failure condition, it is also for Fox Prince
        public static List<PortalCombo> RandomizePortals(int seed, List<Tuple<string, string>> deplando = null, bool canFail = false) {
            TunicLogger.LogInfo("Randomize Portals started");
            List<PortalCombo> randomizedPortals = new List<PortalCombo>();
            // making a separate lists for portals connected to one, two, or three+ regions, to be populated by the foreach coming up next
            List<Portal> portalsList = new List<Portal>();
            bool decoupledEnabled = GetBool(Decoupled);
            bool dirPairsEnabled = GetBool(PortalDirectionPairs);
            bool foxPrinceEnabled = GetBool(FoxPrinceEnabled);
            

            // keeping track of how many portals of each are left while pairing portals
            Dictionary<int, int> twoPlusPortalDirectionTracker = new Dictionary<int, int> { { (int)PDir.NORTH, 0 }, { (int)PDir.SOUTH, 0 }, { (int)PDir.EAST, 0 }, { (int)PDir.WEST, 0 }, { (int)PDir.FLOOR, 0 }, { (int)PDir.LADDER_DOWN, 0 }, { (int)PDir.LADDER_UP, 0 }, { (int)PDir.NONE, 0 } };
            Dictionary<int, int> deadEndPortalDirectionTracker = new Dictionary<int, int> { { (int)PDir.NORTH, 0 }, { (int)PDir.SOUTH, 0 }, { (int)PDir.EAST, 0 }, { (int)PDir.WEST, 0 }, { (int)PDir.FLOOR, 0 }, { (int)PDir.LADDER_DOWN, 0 }, { (int)PDir.LADDER_UP, 0 }, { (int)PDir.NONE, 0 } };
            // quick reference for which directions you can pair to which
            Dictionary<int, int> directionPairs = new Dictionary<int, int> { { (int)PDir.NORTH, (int)PDir.SOUTH }, { (int)PDir.SOUTH, (int)PDir.NORTH }, { (int)PDir.EAST, (int)PDir.WEST }, { (int)PDir.WEST, (int)PDir.EAST }, { (int)PDir.LADDER_UP, (int)PDir.LADDER_DOWN }, { (int)PDir.LADDER_DOWN, (int)PDir.LADDER_UP }, { (int)PDir.FLOOR, (int)PDir.FLOOR }, };

            if (deplando == null) {
                deplando = new List<Tuple<string, string>>();
            }

            // separate the portals into their respective lists
            foreach (KeyValuePair<string, Dictionary<string, List<TunicPortal>>> scene_group in RegionPortalsList) {
                string scene_name = scene_group.Key;
                if (scene_name == "Shop") {
                    continue;
                }
                foreach (KeyValuePair<string, List<TunicPortal>> region_group in scene_group.Value) {
                    // if fixed shop is off or decoupled is on, don't add zig skip exit to the portal list
                    string region_name = region_group.Key;
                    if (region_name == "Zig Skip Exit" && (!GetBool(ERFixedShop) || GetBool(Decoupled))) {
                        continue;
                    }
                    List<TunicPortal> region_portals = region_group.Value;
                    foreach (TunicPortal tunicPortal in region_portals) {
                        Portal portal = new Portal(name: tunicPortal.Name, destination: tunicPortal.Destination, tag: tunicPortal.Tag, scene: scene_name, region: region_name, direction: tunicPortal.Direction, isDeadEnd: RegionDict[region_name].DeadEnd == true);
                        portalsList.Add(portal);
                        if (RegionDict[region_name].DeadEnd == true) {
                            deadEndPortalDirectionTracker[portal.Direction]++;
                        } else {
                            twoPlusPortalDirectionTracker[portal.Direction]++;
                        }
                    }
                }
            }

            // for keeping track of which regions and items are in logic during portal pairing
            Dictionary<string, int> FullInventory = new Dictionary<string, int>();

            // shops get added separately cause they're weird
            int shopCount = 6;
            if (GetBool(ERFixedShop)) {
                shopCount = 1;
            }
            if (GetBool(PortalDirectionPairs)) {
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
                // manually making portals for the shops because they're special
                Portal shopPortal = new Portal(name: $"Shop Portal {i}", destination: $"Previous Region {i}", tag: "", scene: "Shop", region: $"Shop Entrance {i}", direction: dir, isDeadEnd: true);
                portalsList.Add(shopPortal);
                i++;
                deadEndPortalDirectionTracker[dir]++;
            }

            FullInventory.Add("Overworld", 1);

            TunicUtils.AddDictToDict(FullInventory, ItemRandomizer.PopulatePrecollected());

            // if blue prince mode is off or this is the first time through, add all progression to the FullInventory
            if (!ItemRandomizer.InitialRandomizationDone || !foxPrinceEnabled) {
                // it doesn't really matter if there's duplicates from the above for this
                Dictionary<string, int> MaxItems = new Dictionary<string, int> {
                    { "Stick", 1 }, { "Sword", 1 }, { "Wand", 1 }, { "Stundagger", 1 }, { "Techbow", 1 }, { "Shotgun", 1 }, { "Mask", 1 },
                    { "Lantern", 1 }, { "12", 1 }, { "21", 1 }, { "26", 1 }, { "Key", 2 }, { "Key (House)", 1 }, { "Hexagon Gold", 50 }
                };
                TunicUtils.AddDictToDict(FullInventory, MaxItems);

                // if laurels is not at 10 fairies, add laurels to the inventory
                if (SaveFile.GetInt(LaurelsLocation) != 3) {
                    FullInventory.Add("Hyperdash", 1);
                }

                TunicUtils.AddStringToDict(FullInventory, PAIRING_ONLY);

                FullInventory = TunicUtils.AddListToDict(FullInventory, ItemRandomizer.LadderItems);
                FullInventory = TunicUtils.AddListToDict(FullInventory, ItemRandomizer.FuseItems);
                FullInventory = TunicUtils.AddListToDict(FullInventory, ItemRandomizer.BellItems);
            }

            // making a second list if we're doing decoupled, a reference to the first list if not
            List<Portal> portalsList2;
            if (GetBool(Decoupled)) {
                portalsList2 = new List<Portal>(portalsList);
            } else {
                portalsList2 = portalsList;
            }

            // -----------------------------------------------
            // Plando Connections (for Blue Prince stuff)
            // -----------------------------------------------

            // combining this one with the next just creates pain, I promise
            if (GetBool(FoxPrinceEnabled) && (!ItemRandomizer.InitialRandomizationDone || FoxPrince.BPRandomizedPortals.Count == 0)) {
                foreach (string key in SaveFile.stringStore.Keys) {
                    if (key.StartsWith("randomizer bp ")) {
                        string origin = key.Substring("randomizer bp ".Length);
                        string destination = SaveFile.stringStore[key];
                        Portal portal1 = portalsList.First(portal => portal.Name == origin);
                        Portal portal2 = portalsList.First(portal => portal.Name == destination);
                        FoxPrince.BPRandomizedPortals.Add(new PortalCombo(portal1, portal2));
                    }
                }
            }

            if (GetBool(FoxPrinceEnabled) && PlandoPortals.Count == 0) {
                foreach (string key in SaveFile.stringStore.Keys) {
                    if (key.StartsWith("randomizer bp ")) {
                        string origin = key.Substring("randomizer bp ".Length);
                        string destination = SaveFile.stringStore[key];
                        if (ItemRandomizer.InitialRandomizationDone) {
                            // this is to avoid duplicates being made later on
                            if (!GetBool(Decoupled) && PlandoPortals.ContainsKey(destination)) {
                                continue;
                            }
                            PlandoPortals.Add(origin, destination);
                        }
                    }
                }
            }

            foreach (KeyValuePair<string, string> portalPair in PlandoPortals) {
                string portal1name = portalPair.Key;
                string portal2name = portalPair.Value;
                Portal portal1 = null;
                Portal portal2 = null;
                foreach (Portal portal in portalsList) {
                    if (portal.Name == portal1name) {
                        portal1 = portal;
                        break;
                    }
                }
                foreach (Portal portal in portalsList2) {
                    if (portal.Name == portal2name) {
                        portal2 = portal;
                        break;
                    }
                }

                if (portal1 != null) {
                    portalsList.Remove(portal1);
                } else {
                    TunicLogger.LogError($"Error finding portal1 {portal1name} in plando stuff");
                }

                if (portal2 != null) {
                    portalsList2.Remove(portal2);
                } else {
                    TunicLogger.LogError($"Error finding portal2 {portal2name} in plando stuff");
                }

                // this probably can't get hit, remove later after confirming that
                if (portal1 == null || portal2 == null) {
                    TunicLogger.LogError("Error in plando connection stuff while finding the portal names!");
                }

                randomizedPortals.Add(new PortalCombo(portal1, portal2));

                if (portal1.IsDeadEnd) {
                    deadEndPortalDirectionTracker[portal1.Direction]--;
                } else {
                    twoPlusPortalDirectionTracker[portal1.Direction]--;
                }
                if (portal2.IsDeadEnd) {
                    deadEndPortalDirectionTracker[portal2.Direction]--;
                } else {
                    twoPlusPortalDirectionTracker[portal2.Direction]--;
                }
            }

            // add the plando'd connections to the traversal reqs
            foreach (PortalCombo portalCombo in randomizedPortals) {
                Portal p1 = portalCombo.Portal1;
                Portal p2 = portalCombo.Portal2;
                if (!ModifiedTraversalReqs.ContainsKey(p1.Region)) {
                    ModifiedTraversalReqs[p1.Region] = new Dictionary<string, List<List<string>>>();
                }
                ModifiedTraversalReqs[portalCombo.Portal1.Region][portalCombo.Portal2.OutletRegion()] = new List<List<string>>();
                if (!decoupledEnabled) {
                    if (!ModifiedTraversalReqs.ContainsKey(p2.Region)) {
                        ModifiedTraversalReqs[p2.Region] = new Dictionary<string, List<List<string>>>();
                    }
                    ModifiedTraversalReqs[portalCombo.Portal2.Region][portalCombo.Portal1.OutletRegion()] = new List<List<string>>();
                }
            }

            bool TooFewPortalsForDirectionPairs(int direction, int offset = 0) {
                if (twoPlusPortalDirectionTracker[direction] <= deadEndPortalDirectionTracker[directionPairs[direction]] + offset) {
                    return true;
                }
                if (twoPlusPortalDirectionTracker[directionPairs[direction]] <= deadEndPortalDirectionTracker[direction] + offset) {
                    return true;
                }
                return false;
            }

            bool VerifyDirectionPair(Portal portal1, Portal portal2) {
                return portal1.Direction == directionPairs[portal2.Direction];
            }

            // -----------------------------------------------
            // Portal Pairing Starts Here
            // -----------------------------------------------

            List<Check> alreadyCheckedLocations = new List<Check>();

            if (foxPrinceEnabled) {
                foreach (Check check in ItemRandomizer.ProgressionLocations.Values) {
                    if (check.IsCompletedOrCollected) {
                        alreadyCheckedLocations.Add(check);
                        TunicUtils.AddStringAndQuantityToDict(FullInventory, check.Reward.Name, check.Reward.Amount);
                    }
                }
                (FullInventory, alreadyCheckedLocations) = UpdateReachableRegionsAndPickUpItems(FullInventory, alreadyCheckedLocations);
            } else {
                FullInventory = UpdateReachableRegions(FullInventory);
            }

            TunicUtils.ShuffleList(portalsList, seed);
            TunicUtils.ShuffleList(portalsList2, seed);
            List<string> southProblems = new List<string> { "Ziggurat Upper to Ziggurat Entry Hallway", "Ziggurat Tower to Ziggurat Upper", "Forest Belltower to Guard Captain Room" };
            int failCount = 0;
            int previousConnNum = 0;
            List<string> allRegions = new List<string>();
            foreach (KeyValuePair<string, RegionInfo> region in RegionDict) {
                if (region.Value.SkipCounting) continue;
                if (!dirPairsEnabled && (region.Key == "Shop Entrance 7" || region.Key == "Shop Entrance 8")) continue;
                if (GetBool(ERFixedShop) && region.Key.StartsWith("Shop Entrance") && region.Key != "Shop Entrance 1") continue;
                if (region.Key.StartsWith("LS Elev") && SaveFile.GetInt(LadderStorageDifficulty) == 0) continue;
                allRegions.Add(region.Key);
            }

            while (FullInventory.Where(region => RegionDict.ContainsKey(region.Key) && !RegionDict[region.Key].SkipCounting).ToList().Count < allRegions.Count()) {
                Portal portal1 = null;
                Portal portal2 = null;

                if (previousConnNum == FullInventory.Count) {
                    failCount++;
                    if (failCount > 500) {
                        TunicLogger.LogInfo("---------------------------------------");
                        TunicLogger.LogInfo("Remaining portals in portalsList are:");
                        foreach (Portal debugportal in portalsList) {
                            TunicLogger.LogInfo(debugportal.Name);
                        }
                        TunicLogger.LogInfo("---------------------------------------");
                        TunicLogger.LogInfo("The contents of FullInventory are:");
                        foreach (string thing in FullInventory.Keys) {
                            TunicLogger.LogInfo(thing);
                        }
                        TunicLogger.LogInfo("---------------------------------------");
                        TunicLogger.LogInfo("This will now reroll the entrances and try again.");
                        TunicLogger.LogInfo("If you see this, please report it to the TUNIC rando devs, and give them the log file.");
                        // reroll, hopefully this shouldn't be common at all
                        return RandomizePortals(seed + 1);
                    }
                } else {
                    failCount = 0;
                }
                previousConnNum = FullInventory.Count;

                // find a portal in a region we can currently reach
                foreach (Portal portal in portalsList) {
                    if (CanUsePortal(portal, FullInventory)) {
                        portal1 = portal;
                        portalsList.Remove(portal);
                        break;
                    }
                }

                if (portal1 == null) {
                    // it will fail after this
                    TunicLogger.LogError("something messed up in portal pairing for portal 1 in RandomizePortals");
                    TunicLogger.LogInfo("if there are regions missing, they are as follows:");
                    foreach (string region in allRegions) {
                        if (!FullInventory.ContainsKey(region)) {
                            TunicLogger.LogInfo(region);
                        }
                    }
                    TunicLogger.LogInfo("remaining portals in portalsList are:");
                    foreach (Portal debugportal in portalsList) {
                        TunicLogger.LogInfo(debugportal.Name);
                    }
                    // reroll, hopefully this shouldn't be common at all
                    TunicLogger.LogInfo("Rerolling in first phase in RandomizePortals");
                    if (canFail) {
                        return null;
                    }
                    return RandomizePortals(seed + 1);
                }

                foreach (Portal portal in portalsList2) {
                    if (!FullInventory.ContainsKey(portal.Region)) {
                        if (deplando.Any(item => item.Item1 == portal1.Name && item.Item2 == portal.Name)) continue;
                        if (dirPairsEnabled) {
                            if (!VerifyDirectionPair(portal1, portal)) continue;
                            if (!decoupledEnabled) {
                                // we don't want to run out of spots for dead ends to go
                                if (!portal.IsDeadEnd && TooFewPortalsForDirectionPairs(portal1.Direction)) continue;
                                bool shouldContinue = false;
                                // the south problem portals are all effectively one ways
                                southProblems.RemoveAll(item => FullInventory.ContainsKey(item));
                                if (portal.Direction == (int)PDir.SOUTH && !southProblems.Contains(portal.Name) && southProblems.Count > 0 && TooFewPortalsForDirectionPairs(portal.Direction, offset: southProblems.Count)) {
                                    foreach (Portal testPortal in portalsList) {
                                        if (southProblems.Contains(testPortal.Name)) {
                                            TunicLogger.LogTesting("Will continue to avoid south problems");
                                            shouldContinue = true;
                                        }
                                    }
                                }
                                if (portal.Direction == (int)PDir.LADDER_DOWN
                                    || portal.Direction == (int)PDir.LADDER_UP && portal.Name != "Frog's Domain Ladder Exit" && TooFewPortalsForDirectionPairs(portal.Direction, offset: 1)) {
                                    foreach (Portal testPortal in portalsList) {
                                        if (testPortal.Name == "Frog's Domain Ladder Exit") {
                                            TunicLogger.LogTesting("Will continue to avoid Frog's Domain Ladder Exit issue");
                                            shouldContinue = true;
                                        }
                                    }
                                }
                                if (shouldContinue) continue;
                            }
                        }

                        // if secret gathering place gets paired really late and you have the laurels plando on, you can run out pretty easily
                        if (portalsList2.Count < 80 && portal.Region != "Secret Gathering Place" && !FullInventory.ContainsKey("Hyperdash") && SaveFile.GetInt(LaurelsLocation) == 3 && !foxPrinceEnabled) {
                            TunicLogger.LogTesting("Continuing to wait for Secret Gathering Place");
                            continue;
                        }

                        portal2 = portal;
                        if (!FullInventory.ContainsKey(portal2.OutletRegion())) {
                            FullInventory.Add(portal2.OutletRegion(), 1);
                        }
                        portalsList2.Remove(portal);
                        break;
                    }
                }
                if (portal2 == null) {
                    if (dirPairsEnabled) {
                        portalsList.Add(portal1);
                        continue;
                    } else {
                        TunicLogger.LogInfo("---------------------------------------");
                        TunicLogger.LogError("something messed up in portal pairing for portal 2, rerolling in RandomizePortals");
                        TunicLogger.LogInfo("if there are regions missing, they are as follows:");
                        foreach (string region in allRegions) {
                            if (!FullInventory.ContainsKey(region)) {
                                TunicLogger.LogInfo(region);
                            }
                        }
                        TunicLogger.LogInfo("remaining portals in portalsList are:");
                        foreach (Portal debugportal in portalsList) {
                            TunicLogger.LogInfo(debugportal.Name);
                        }
                        // reroll, hopefully this shouldn't be common at all
                        if (canFail) {
                            return null;
                        }
                        return RandomizePortals(seed + 1);
                    }
                }

                if (foxPrinceEnabled) {
                    (FullInventory, alreadyCheckedLocations) = UpdateReachableRegionsAndPickUpItems(FullInventory, alreadyCheckedLocations);
                } else {
                    FullInventory = UpdateReachableRegions(FullInventory);
                }

                // add the portal combo to the randomized portals list
                randomizedPortals.Add(new PortalCombo(portal1, portal2));
                if (portal1.IsDeadEnd) {
                    deadEndPortalDirectionTracker[portal1.Direction]--;
                } else {
                    twoPlusPortalDirectionTracker[portal1.Direction]--;
                }
                if (portal2.IsDeadEnd) {
                    deadEndPortalDirectionTracker[portal2.Direction]--;
                } else {
                    twoPlusPortalDirectionTracker[portal2.Direction]--;
                }

                TunicUtils.ShuffleList(portalsList, seed);
                if (portalsList != portalsList2) {
                    TunicUtils.ShuffleList(portalsList2, seed);
                }
            }
            TunicLogger.LogTesting("done pairing portalsList");

            // now we have every region accessible, and every dead end should be connected, so it's time to connect loose ends
            int finalPairLoopNumber = 0;
            while (portalsList.Count > 0) {
                finalPairLoopNumber++;
                if (finalPairLoopNumber > 10000) {
                    TunicLogger.LogError("Failed to pair portals while pairing the final entrances off to each other");
                    TunicLogger.LogInfo("Remaining portals in portalsList:");
                    foreach (Portal portal in portalsList) {
                        TunicLogger.LogInfo(portal.Name);
                    }
                    TunicLogger.LogInfo("Remaining portals in portalsList2:");
                    foreach (Portal portal in portalsList2) {
                        TunicLogger.LogInfo(portal.Name);
                    }
                    // reroll, hopefully this shouldn't be common at all
                    if (canFail) {
                        return null;
                    }
                    TunicLogger.LogInfo("Rerolling during last phase in RandomizePortals");
                    return RandomizePortals(seed + 1);
                }
                Portal portal1 = portalsList[0];
                portalsList.RemoveAt(0);
                Portal portal2 = null;
                if (!dirPairsEnabled) {
                    portal2 = portalsList2[0];
                    portalsList2.RemoveAt(0);
                } else {
                    foreach (Portal portal in portalsList2) {
                        if (directionPairs[portal1.Direction] == portal.Direction) {
                            portal2 = portal;
                            portalsList2.Remove(portal);
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
                    foreach (Portal debugportal in portalsList) {
                        TunicLogger.LogInfo(debugportal.Name);
                    }
                    foreach (PortalCombo combo in randomizedPortals) {
                        if (combo.Portal1.Direction == portal1.Direction || combo.Portal2.Direction == portal1.Direction) {
                            TunicLogger.LogInfo("Possible error combo: " + combo.Portal1.Name + " and " + combo.Portal2.Name);
                        }
                    }
                    // reroll, hopefully this shouldn't be common at all
                    if (canFail) {
                        return null;
                    }
                    TunicLogger.LogInfo("Rerolling portals in last phase because we couldn't find a match in RandomizePortals");
                    return RandomizePortals(seed + 1);
                }
                if (deplando.Any(item => item.Item1 == portal1.Name && item.Item2 == portal2.Name)) {
                    portalsList.Add(portal1);
                    portalsList2.Add(portal2);
                    TunicUtils.ShuffleList(portalsList, seed);
                    TunicUtils.ShuffleList(portalsList2, seed);
                    continue;
                }

                randomizedPortals.Add(new PortalCombo(portal1, portal2));
            }
            TunicLogger.LogInfo("Randomize Portals done");
            return randomizedPortals;
        }

        // shops will be formatted like "Shop, 3_" for Shop Portal 3, instead of the current "Shop, Previous Region_"
        // it also needs to be able to handle an arbitrary number of shops, up to like 500 maybe
        // this is for using the info from Archipelago to pair up the portals
        public static void CreatePortalPairs(Dictionary<string, string> APPortalStrings) {
            RandomizedPortals.Clear();
            List<Portal> portalsList = new List<Portal>();
            // equivalent of doing TraversalReqs.copy() in python
            ModifiedTraversalReqs = TunicUtils.DeepCopyTraversalReqs();

            foreach (KeyValuePair<string, Dictionary<string, List<TunicPortal>>> scene_group in RegionPortalsList) {
                string scene_name = scene_group.Key;
                foreach (KeyValuePair<string, List<TunicPortal>> region_group in scene_group.Value) {
                    string region_name = region_group.Key;
                    List<TunicPortal> region_portals = region_group.Value;
                    foreach (TunicPortal tunicPortal in region_portals) {
                        Portal portal = new Portal(name: tunicPortal.Name, destination: tunicPortal.Destination, tag: tunicPortal.Tag, scene: scene_name, region: region_name, direction: tunicPortal.Direction, isDeadEnd: true);
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
                    portal2 = new Portal(name: $"Shop Portal {backwards_compat_shop_num}", destination: $"Previous Region {backwards_compat_shop_num}", tag: "", scene: "Shop", region: $"Shop Entrance {backwards_compat_shop_num}", direction: (int)PDir.NONE, isDeadEnd: true);
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
                    portal1 = new Portal(name: $"Shop Portal {shop_num}", destination: $"Previous Region {shop_num}", tag: "", scene: "Shop", region: $"Shop Entrance {shop_num}", direction: dir, isDeadEnd: true);
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
                    portal2 = new Portal(name: $"Shop Portal {shop_num}", destination: $"Previous Region {shop_num}", tag: "", scene: "Shop", region: $"Shop Entrance {shop_num}", direction: dir, isDeadEnd: true);
                    if (!ModifiedTraversalReqs.ContainsKey($"Shop Entrance {shop_num}")) {
                        ModifiedTraversalReqs.Add($"Shop Entrance {shop_num}", new Dictionary<string, List<List<string>>> { { "Shop", new List<List<string>> { } } });
                    }
                }
                PortalCombo portalCombo = new PortalCombo(portal1, portal2);
                RandomizedPortals.Add(portalCombo);
                // if decoupled is off, we want to make reverse pairs
                if (!GetBool(Decoupled)) {
                    PortalCombo reversePortalCombo = new PortalCombo(portal2, portal1);
                    RandomizedPortals.Add(reversePortalCombo);
                }
            }
            ModifiedTraversalReqs = TrickLogic.TraversalReqsWithLS(ModifiedTraversalReqs);
        }

        // a function to apply the randomized portal list to portals during onSceneLoaded
        public static void ModifyPortals(string scene_name, bool sending = false) {
            if (GetBool(FoxPrinceEnabled)) {
                RandomizedPortals = new List<PortalCombo>(FoxPrince.BPRandomizedPortals);
            }
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
                        foreach (PortalCombo portalCombo in RandomizedPortals) {
                            if (PlayerCharacterSpawn.portalIDToSpawnAt.EndsWith(portalCombo.ComboTag)) {
                                TunicLogger.LogTesting("Portal 1 is " + portalCombo.Portal1.Name);
                                TunicLogger.LogTesting("Portal 2 is " + portalCombo.Portal2.Name);
                                newPortal.GetComponent<ScenePortal>().destinationSceneName = portalCombo.Portal2.Destination;
                                newPortal.GetComponent<ScenePortal>().id = portalCombo.Portal2.Tag;
                                if (portalCombo.FlippedShop) {
                                    shopPortal.TryCast<ShopScenePortal>().flippedCameraZone.enabled = true;
                                } else {
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
                foreach (PortalCombo portalCombo in RandomizedPortals) {
                    Portal portal1 = portalCombo.Portal1;
                    Portal portal2 = portalCombo.Portal2;
                    string comboTag = portalCombo.ComboTag;

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
                                if (portal2.SceneDestinationTag == "Overworld Redux, Overworld Interiors_house" && !GetBool("SV_Overworld_House Door Opened")) {
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
                                    if (GetBool(LadderRandoEnabled)) {
                                        foreach (KeyValuePair<string, List<string>> problems in problemSpots) {
                                            string ladderName = problems.Key;
                                            List<string> ladderPortals = problems.Value;
                                            // if the old house door connects to a problem spot and you don't have the ladder
                                            if (ladderPortals.Contains(portal1.SceneDestinationTag)
                                            && (!GetBool($"randomizer picked up {ladderName}"))) {
                                                // if you're not in decoupled, then we can be sure this is a problem
                                                if (!GetBool(Decoupled)) {
                                                    OldHouseDoorUnstuck = true;
                                                } else {
                                                    // if you're in decoupled, we want to make sure the door leads to a problem spot, not just from a problem spot
                                                    foreach (PortalCombo portalCombo2 in RandomizedPortals) {
                                                        if (portalCombo2.Portal1.Name == "Overworld Redux, Overworld Interiors_house") {
                                                            if (ladderPortals.Contains(portalCombo2.Portal2.SceneDestinationTag)) {
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
            
            if (GetBool(FoxPrinceEnabled)) {
                ModifyPortalNames(scene_name);
            }
        }

        // for non-ER, just modifies the portal names -- this is useful for the FairyTargets for the entrance seeking spell
        public static void ModifyPortalNames(string scene_name) {
            var Portals = Resources.FindObjectsOfTypeAll<ScenePortal>().Where(portal => portal.gameObject.scene.name == SceneManager.GetActiveScene().name
            && !portal.FullID.Contains("customfasttravel") && !portal.id.Contains("customfasttravel"));
            if (ERData.VanillaPortals.Count == 0) {
                ERScripts.SetupVanillaPortals();
            }
            foreach (var portal in Portals) {
                // skips the extra west garden shop portal
                if (!portal.isActiveAndEnabled) {
                    continue;
                }
                // go through the list of randomized portals and see if the first portal matches the one we're looking at
                foreach (PortalCombo portalCombo in ERData.VanillaPortals) {
                    Portal portal1 = portalCombo.Portal1;
                    Portal portal2 = portalCombo.Portal2;

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
                    foreach (PortalCombo portalCombo in RandomizedPortals) {
                        if (portal.name == portalCombo.Portal2.Name && (portal.name != "Shop Portal" || (portal.name == "Shop Portal" && portalCombo.Portal1.Scene == SceneManager.GetActiveScene().name))) {
                            if (SaveFile.GetInt("randomizer entered portal " + portalCombo.Portal1.Name) == 0) {
                                SaveFile.SetInt("randomizer entered portal " + portalCombo.Portal1.Name, 1);
                                if (IsArchipelago()) {
                                    string key = $"{portalCombo.Portal1.Scene}, {portalCombo.Portal1.Destination}_{portalCombo.Portal1.Tag}";
                                    Archipelago.instance.integration.UpdateDataStorage(key, true);
                                }
                            }
                            // if decoupled is off, we can just mark the other side of the portal too
                            if (!GetBool(Decoupled)) {
                                if (SaveFile.GetInt("randomizer entered portal " + portalCombo.Portal2.Name) == 0) {
                                    SaveFile.SetInt("randomizer entered portal " + portalCombo.Portal2.Name, 1);
                                    if (IsArchipelago()) {
                                        string key = $"{portalCombo.Portal2.Scene}, {portalCombo.Portal2.Destination}_{portalCombo.Portal2.Tag}";
                                        Archipelago.instance.integration.UpdateDataStorage(key, true);
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
            List<PortalCombo> portalList;
            if (GetBool(EntranceRando)) {
                portalList = ERData.RandomizedPortals;
            } else {
                portalList = ERData.VanillaPortals;
            }
            foreach (PortalCombo portalCombo in portalList) {
                if (portalCombo.Portal1.Name == portalName) {
                    return portalCombo.Portal2.Scene;
                }
            }
            // returning this if it fails, since that makes some FairyTarget stuff easier
            return "FindPairedPortalSceneFromName failed to find a match";
        }

        public static string FindPairedPortalRegionFromSDT(string portalSDT) {
            List<PortalCombo> portalList;
            if (GetBool(EntranceRando)) {
                portalList = ERData.RandomizedPortals;
            } else {
                if (ERData.VanillaPortals.Count == 0) {
                    SetupVanillaPortals();
                }
                portalList = ERData.VanillaPortals;
            }
            foreach (PortalCombo portalCombo in portalList) {
                if (portalCombo.Portal1.SceneDestinationTag == portalSDT) {
                    return portalCombo.Portal2.OutletRegion();
                }
            }

            // returning this if it fails, since that makes some FairyTarget stuff easier
            return "FindPairedPortalRegionFromSDT failed to find a match";
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
