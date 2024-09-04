using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem.Utilities;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {
    public class ItemRandomizer {
        
        public static Dictionary<string, int> SphereZero = new Dictionary<string, int>();

        // set this to true to test location access
        public static bool testLocations = false;
        // leave this one alone
        public static bool testBool = false;
        
        // essentially fake items for the purpose of logic
        public static List<string> PrecollectedItems = new List<string>();

        public static List<string> LadderItems = ItemLookup.Items.Where(item => item.Value.Type == ItemTypes.LADDER).Select(item => item.Value.Name).ToList();

        public static void PopulatePrecollected() {
            PrecollectedItems.Clear();
            if (SaveFile.GetInt(LadderRandoEnabled) == 0) {
                PrecollectedItems.AddRange(LadderItems);
            }
            if (SaveFile.GetInt(MasklessLogic) == 1) {
                PrecollectedItems.Add("Mask");
            }
            if (SaveFile.GetInt(LanternlessLogic) == 1) {
                PrecollectedItems.Add("Lantern");
            }
            if (SaveFile.GetInt(AbilityShuffle) == 0) {
                PrecollectedItems.AddRange(new List<string> { "12", "21", "26" });
            }
            if (SaveFile.GetInt(StartWithSword) == 1) {
                PrecollectedItems.Add("Sword");
            }
        }

        public static void RandomizeAndPlaceItems(Random random = null) {
            TunicLogger.LogInfo("randomize and place items starting");

            if (testLocations) {
                testBool = true;
            }

            if (random == null) {
                random = new Random(SaveFile.GetInt("seed"));
            }

            Locations.RandomizedLocations.Clear();
            Locations.CheckedLocations.Clear();

            PopulatePrecollected();
            List<string> ProgressionNames = new List<string> { "Hyperdash", "Wand", "Techbow", "Stundagger", "Trinket Coin", "Lantern", "Stick", "Sword", "Sword Progression", "Key", "Key (House)", "Mask", "Vault Key (Red)", "Gun" };
            List<string> Ladders = new List<string>(LadderItems);
            List<string> GrassCutters = new List<string>() { "Trinket - Glass Cannon", };
            List<string> abilityPages = new List<string>() { "12", "21", "26" };
            if (SaveFile.GetInt(AbilityShuffle) == 1) {
                if (IsHexQuestWithHexAbilities()) {
                    ProgressionNames.Add("Hexagon Gold");
                } else {
                    ProgressionNames.AddRange(abilityPages);
                }
            }

            // these stop being progression if they aren't required in logic
            if (SaveFile.GetInt(LanternlessLogic) == 1) {
                ProgressionNames.Remove("Lantern");
            }
            if (SaveFile.GetInt(MasklessLogic) == 1) {
                ProgressionNames.Remove("Mask");
            }
            if (SaveFile.GetInt(LadderRandoEnabled) == 1) {
                ProgressionNames.AddRange(LadderItems);
            }

            List<Check> InitialItems = JsonConvert.DeserializeObject<List<Check>>(ItemListJson.ItemList);
            if (SaveFile.GetInt(GrassRandoEnabled) == 1) {
                InitialItems.AddRange(GrassRandomizer.GrassChecks.Values.Where(check => !GrassRandomizer.ExcludedGrassChecks.Contains(check.CheckId)));
                ProgressionNames.AddRange(GrassCutters);
            }
            List<Reward> InitialRewards = new List<Reward>();
            List<Location> InitialLocations = new List<Location>();
            List<Check> Hexagons = new List<Check>();
            // the list of progression items
            List<Reward> ProgressionRewards = new List<Reward>();
            // inventory of progression items that have not been placed yet
            Dictionary<string, int> UnplacedInventory = new Dictionary<string, int>();
            // locations that progression is placed at
            Dictionary<string, Check> ProgressionLocations = new Dictionary<string, Check> { };

            int GoldHexagonsAdded = 0;
            int HexagonsToAdd = (int)Math.Round((100f + SaveFile.GetInt(HexagonQuestExtras)) / 100f * SaveFile.GetInt(HexagonQuestGoal));
            if (SaveFile.GetInt(KeysBehindBosses) == 1) {
                HexagonsToAdd -= 3;
            }
            if (IsHexQuestWithHexAbilities()) {
                int HexGoal = SaveFile.GetInt(HexagonQuestGoal);
                List<string> abilities = new List<string>() { "prayer", "holy cross", "icebolt" }.OrderBy(r => random.Next()).ToList();
                List<int> ability_unlocks = new List<int>() { (int)(HexGoal / 4f), (int)((HexGoal / 4f) * 2), (int)((HexGoal / 4f) * 3) }.OrderBy(r => random.Next()).ToList();
                for (int i = 0; i < 3; i++) {
                    int index = random.Next(abilities.Count);
                    int index2 = random.Next(ability_unlocks.Count);
                    SaveFile.SetInt($"randomizer hexagon quest {abilities[index]} requirement", ability_unlocks[index2]);
                    abilities.RemoveAt(index);
                    ability_unlocks.RemoveAt(index2);
                }
            }
            Shuffle(InitialItems, random);
            foreach (Check Item in InitialItems) {
                if (SaveFile.GetInt("randomizer keys behind bosses") != 0 && (Item.Reward.Name.Contains("Hexagon") || Item.Reward.Name == "Vault Key (Red)")) {
                    if (Item.Reward.Name == "Hexagon Green" || Item.Reward.Name == "Hexagon Blue") {
                        Hexagons.Add(Item);
                    } else if (Item.Reward.Name == "Vault Key (Red)") {
                        Item.Reward.Name = "Hexagon Red";
                        Hexagons.Add(Item);
                    } else if (Item.Reward.Name == "Hexagon Red") {
                        Item.Reward.Name = "Vault Key (Red)";
                        ProgressionRewards.Add(Item.Reward);
                        InitialLocations.Add(Item.Location);
                    }
                } else {
                    if (SaveFile.GetInt("randomizer sword progression enabled") != 0 && (Item.Reward.Name == "Stick" || Item.Reward.Name == "Sword" || Item.Location.LocationId == "5")) {
                        Item.Reward.Name = "Sword Progression";
                        Item.Reward.Type = "SPECIAL";
                    }
                    if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                        if (Item.Reward.Type == "PAGE" || Item.Reward.Name.Contains("Hexagon")) {
                            if (IsHexQuestWithPageAbilities() && abilityPages.Contains(Item.Reward.Name)) {

                            } else {
                                string FillerItem = ItemLookup.FillerItems.Keys.ToList()[random.Next(ItemLookup.FillerItems.Count)];
                                Item.Reward.Name = FillerItem;
                                Item.Reward.Type = FillerItem == "money" ? "MONEY" : "INVENTORY";
                                Item.Reward.Amount = ItemLookup.FillerItems[FillerItem][random.Next(ItemLookup.FillerItems[FillerItem].Count)];
                            }
                        }
                        if (ItemLookup.FillerItems.ContainsKey(Item.Reward.Name) && ItemLookup.FillerItems[Item.Reward.Name].Contains(Item.Reward.Amount) && GoldHexagonsAdded < HexagonsToAdd) {
                            Item.Reward.Name = "Hexagon Gold";
                            Item.Reward.Type = "SPECIAL";
                            Item.Reward.Amount = 1;
                            GoldHexagonsAdded++;
                        }

                        // can probably be removed now that Check.reachable got updated to check hex counts
                        if (IsHexQuestWithHexAbilities()) {
                            if (Item.Location.Requirements.Count > 0) {
                                for (int i = 0; i < Item.Location.Requirements.Count; i++) {
                                    if (Item.Location.Requirements[i].ContainsKey("12") && Item.Location.Requirements[i].ContainsKey("21")) {
                                        int amt = Math.Max(SaveFile.GetInt($"randomizer hexagon quest prayer requirement"), SaveFile.GetInt($"randomizer hexagon quest holy cross requirement"));
                                        Item.Location.Requirements[i].Remove("12");
                                        Item.Location.Requirements[i].Remove("21");
                                        Item.Location.Requirements[i].Add("Hexagon Gold", amt);
                                    }
                                    if (Item.Location.Requirements[i].ContainsKey("12")) {
                                        Item.Location.Requirements[i].Remove("12");
                                        Item.Location.Requirements[i].Add("Hexagon Gold", SaveFile.GetInt($"randomizer hexagon quest prayer requirement"));
                                    }
                                    if (Item.Location.Requirements[i].ContainsKey("21")) {
                                        Item.Location.Requirements[i].Remove("21");
                                        Item.Location.Requirements[i].Add("Hexagon Gold", SaveFile.GetInt($"randomizer hexagon quest holy cross requirement"));
                                    }
                                    if (Item.Location.Requirements[i].ContainsKey("26")) {
                                        Item.Location.Requirements[i].Remove("26");
                                        Item.Location.Requirements[i].Add("Hexagon Gold", SaveFile.GetInt($"randomizer hexagon quest icebolt requirement"));
                                    }
                                }
                            }
                        }
                    }
                    if (SaveFile.GetInt(LadderRandoEnabled) == 1 && ItemLookup.FillerItems.ContainsKey(Item.Reward.Name) && Ladders.Count > 0) {
                        Item.Reward.Name = Ladders[random.Next(Ladders.Count)];
                        Item.Reward.Amount = 1;
                        Item.Reward.Type = "INVENTORY";
                        Ladders.Remove(Item.Reward.Name);
                    }
                    if (ProgressionNames.Contains(Item.Reward.Name) || ItemLookup.FairyLookup.Keys.Contains(Item.Reward.Name)) {
                        ProgressionRewards.Add(Item.Reward);
                    } else {
                        InitialRewards.Add(Item.Reward);
                    }
                    InitialLocations.Add(Item.Location);
                }
            }

            // pre-place laurels in ProgressionLocations, so that fill can collect it as needed
            if (SaveFile.GetInt(LaurelsLocation) != 0) {
                foreach (Reward item in ProgressionRewards) {
                    if (item.Name == "Hyperdash") {
                        foreach (Location location in InitialLocations) {
                            if ((location.LocationId == "Well Reward (6 Coins)" && SaveFile.GetInt(LaurelsLocation) == 1)
                                || (location.LocationId == "Well Reward (10 Coins)" && SaveFile.GetInt(LaurelsLocation) == 2)
                                || (location.LocationId == "waterfall" && SaveFile.GetInt(LaurelsLocation) == 3)) {
                                Check Check = new Check(item, location);
                                string DictionaryId = Check.CheckId;
                                ProgressionLocations.Add(DictionaryId, Check);
                                InitialLocations.Remove(location);
                                ProgressionRewards.Remove(item);
                                break;
                            }
                        }
                        break;
                    }
                }
            }

            // adding the progression rewards to the start inventory, so we can reverse fill
            foreach (Reward item in ProgressionRewards) {
                string itemName = ItemLookup.FairyLookup.Keys.Contains(item.Name) ? "Fairy" : item.Name;
                if (UnplacedInventory.ContainsKey(itemName)) {
                    UnplacedInventory[itemName] += 1;
                } else {
                    UnplacedInventory.Add(itemName, 1);
                }
            }

            if (SaveFile.GetInt(EntranceRando) == 1) {
                ERScripts.RandomizePortals(SaveFile.GetInt("seed"));
            } else {
                ERData.RandomizedPortals = ERScripts.VanillaPortals();
            }

            // used in fill to keep checks that you've re-collected items from from being collected again
            List<Check> checksAlreadyAdded = new List<Check>();

            // full inventory is to separate out "fake" items from real ones
            Dictionary<string, int> FullInventory = new Dictionary<string, int>();
            int iteration_number = 0;
            // put progression items in locations
            foreach (Reward item in ProgressionRewards.OrderBy(r => random.Next())) {
                iteration_number++;
                // pick an item
                string itemName = ItemLookup.FairyLookup.Keys.Contains(item.Name) ? "Fairy" : item.Name;
                // remove item from inventory for reachability checks
                if (UnplacedInventory.Keys.Contains(itemName)) {
                    UnplacedInventory[itemName] -= 1;
                }
                if (UnplacedInventory[itemName] == 0) {
                    UnplacedInventory.Remove(itemName);
                }

                // clear the inventory we use to determine access, put the Overworld region, unplaced items, and precollected items in it
                FullInventory.Clear();
                FullInventory.Add("Overworld", 1);
                foreach (KeyValuePair<string, int> unplacedItem in UnplacedInventory) {
                    FullInventory.Add(unplacedItem.Key, unplacedItem.Value);
                }
                TunicUtils.AddListToDict(FullInventory, PrecollectedItems);

                // cache for picking up items during fill
                checksAlreadyAdded.Clear();

                // fill method: reverse fill and anything you can get with your remaining inventory is assumed to be in your inventory for the purpose of placing the next item
                while (true) {
                    // start count is the quantity of items in full inventory. If this stays the same between loops, then you are done getting items
                    int start_count = 0;
                    foreach (int count in FullInventory.Values) {
                        start_count += count;
                    }

                    // fill up our FullInventory with regions until we stop getting new regions -- these are the regions we can currently access
                    while (true) {
                        // since regions always have a count of 1, we can just use .count instead of counting up all the values
                        int start_num = FullInventory.Count;
                        FullInventory = ERScripts.UpdateReachableRegions(FullInventory);
                        foreach (PortalCombo portalCombo in ERData.RandomizedPortals.Values) {
                            FullInventory = portalCombo.AddComboRegion(FullInventory);
                        }
                        if (start_num == FullInventory.Count) {
                            break;
                        }
                    }

                    // for debugging to check inventory item counts
                    //TunicLogger.LogInfo("iteration number is " + iteration_number.ToString());
                    //TunicLogger.LogInfo("item being placed is " + item.Name);
                    //TunicLogger.LogInfo("Full Inventory is");
                    //foreach (string iname in FullInventory.Keys) {
                    //    TunicLogger.LogInfo(iname + ": " + FullInventory[iname].ToString());
                    //}

                    // pick up all items you can reach with your current inventory
                    foreach (Check placedLocation in ProgressionLocations.Values) {
                        if (placedLocation.Location.reachable(FullInventory) && !checksAlreadyAdded.Contains(placedLocation)) {
                            //TunicLogger.LogInfo("Location " + Locations.LocationIdToDescription[placedLocation.CheckId] + " is reachable");
                            //TunicLogger.LogInfo("Adding " + placedLocation.Reward.Name + " to inventory");
                            string item_name = ItemLookup.FairyLookup.Keys.Contains(placedLocation.Reward.Name) ? "Fairy" : placedLocation.Reward.Name;
                            TunicUtils.AddStringToDict(FullInventory, item_name);
                            checksAlreadyAdded.Add(placedLocation);
                        }
                    }

                    int end_count = 0;
                    foreach (int count in FullInventory.Values) {
                        end_count += count;
                    }

                    // if these two are equal, then we've gotten everything we have access to
                    if (start_count == end_count) {
                        break;
                    }
                }


                // this is for testing fill, ignore if not testing
                // using a full inventory of items, checks whether each location is reachable, and prints an error if any aren't
                // change the testLocations bool to true to have it to test whether all locations can be reached
                if (testBool) {
                    TunicLogger.LogInfo("test starts here");
                    Dictionary<string, int> testUnplacedInventory = new Dictionary<string, int>();
                    Dictionary<string, int> testFullInventory = new Dictionary<string, int>();
                    foreach (KeyValuePair<string, int> kvp in UnplacedInventory) {
                        testUnplacedInventory.Add(kvp.Key, kvp.Value);
                    }
                    if (testUnplacedInventory.ContainsKey(itemName)) {
                        testUnplacedInventory[itemName] += 1;
                    } else {
                        testUnplacedInventory.Add(itemName, 1);
                    }

                    foreach (KeyValuePair<string, int> testUnplacedItem in testUnplacedInventory) {
                        testFullInventory.Add(testUnplacedItem.Key, testUnplacedItem.Value);
                    }

                    TunicUtils.AddListToDict(testFullInventory, PrecollectedItems);

                    if (SaveFile.GetInt(EntranceRando) == 1) {
                        // this should keep looping until every portal either doesn't give a reward, or has already given its reward
                        testFullInventory.Clear();
                        testFullInventory.Add("Overworld", 1);
                        foreach (KeyValuePair<string, int> unplacedItem in testUnplacedInventory) {
                            testFullInventory.Add(unplacedItem.Key, unplacedItem.Value);
                        }
                        TunicUtils.AddListToDict(testFullInventory, PrecollectedItems);

                        // fill up our FullInventory with regions until we stop getting new regions -- these are the portals and regions we can currently reach
                        while (true) {
                            int start_num = testFullInventory.Count;
                            testFullInventory = ERScripts.UpdateReachableRegions(testFullInventory);
                            foreach (PortalCombo portalCombo in ERData.RandomizedPortals.Values) {
                                testFullInventory = portalCombo.AddComboRegion(testFullInventory);
                            }
                            int end_num = testFullInventory.Count;
                            if (start_num == end_num) {
                                break;
                            }
                        }
                    }

                    TunicLogger.LogInfo("testing location accessibility now");
                    foreach (Location loc in InitialLocations) {
                        if (!loc.reachable(testFullInventory)) {
                            TunicLogger.LogInfo("Location " + loc.LocationId + " is not reachable, investigate");
                        }
                    }
                    TunicLogger.LogInfo("test ends here, if you didn't see any locations above this message then there were no unreachable locations");
                    testBool = false;
                }


                // pick a location
                int l;
                l = random.Next(InitialLocations.Count);

                int counter = 0;
                while (!InitialLocations[l].reachable(FullInventory)) {
                    l = random.Next(InitialLocations.Count);
                    counter++;
                    // If it fails to place an item, start over with the current seed progress
                    // This is almost exclusively for ladder shuffle due to the small sphere one size, and will likely never get called otherwise
                    if (counter >= InitialLocations.Count) {
                        TunicLogger.LogInfo("Failed to find more spots to place items. Debug info below. It will attempt to re-randomize afterwards. If you see this, please report it to the TUNIC devs along with the seed paste.");
                        TunicLogger.LogInfo("item being placed is " + item.Name);
                        TunicLogger.LogInfo("unplaced inventory contents:");
                        foreach (KeyValuePair<string, int> itemgroup in UnplacedInventory) {
                            TunicLogger.LogInfo($"{itemgroup.Key}, {itemgroup.Value}");
                        }
                        TunicLogger.LogInfo("full inventory contents:");
                        foreach (KeyValuePair<string, int> itemgroup in FullInventory) {
                            TunicLogger.LogInfo($"{itemgroup.Key}, {itemgroup.Value}");
                        }
                        RandomizeAndPlaceItems(random);
                        return;
                    }
                }

                // prepare matched list of progression items and locations
                Check Check = new Check(item, InitialLocations[l]);
                string DictionaryId = Check.CheckId;
                ProgressionLocations.Add(DictionaryId, Check);

                InitialLocations.Remove(InitialLocations[l]);
            }

            if (SaveFile.GetInt(EntranceRando) == 1) {
                SphereZero = GetERSphereOne();
            } else {
                SphereZero = GetSphereOne();
            }

            // shuffle remaining rewards and locations
            Shuffle(InitialRewards, InitialLocations, random);

            for (int i = 0; i < InitialRewards.Count; i++) {
                Check Check = new Check(InitialRewards[i], InitialLocations[i]);
                string DictionaryId = Check.CheckId;
                Locations.RandomizedLocations.Add(DictionaryId, Check);
            }

            // add progression items and locations back
            foreach (string key in ProgressionLocations.Keys) {
                Locations.RandomizedLocations.Add(key, ProgressionLocations[key]);
            }

            if (SaveFile.GetInt("randomizer keys behind bosses") != 0) {
                foreach (Check Hexagon in Hexagons) {
                    if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                        Hexagon.Reward.Name = "Hexagon Gold";
                        Hexagon.Reward.Type = "SPECIAL";
                    }
                    string DictionaryId = Hexagon.CheckId;
                    Locations.RandomizedLocations.Add(DictionaryId, Hexagon);
                }
            }

            // Add grass checks back in that shouldn't be randomized (ones that are affected by clear early bushes)
            if (SaveFile.GetInt(GrassRandoEnabled) == 1) {
                foreach(KeyValuePair<string, Check> pair in GrassRandomizer.GrassChecks.Where(grass => GrassRandomizer.ExcludedGrassChecks.Contains(grass.Key))) {
                    Locations.RandomizedLocations.Add(pair.Key, pair.Value);
                }
            }

            if (SaveFile.GetString("randomizer game mode") == "VANILLA") {
                Locations.RandomizedLocations.Clear();
                foreach (Check item in JsonConvert.DeserializeObject<List<Check>>(ItemListJson.ItemList)) {
                    if (SaveFile.GetInt("randomizer sword progression enabled") != 0) {
                        if (item.Reward.Name == "Stick" || item.Reward.Name == "Sword" || item.Location.LocationId == "5") {
                            item.Reward.Name = "Sword Progression";
                            item.Reward.Type = "SPECIAL";
                        }
                    }
                    string DictionaryId = item.CheckId;
                    Locations.RandomizedLocations.Add(DictionaryId, item);
                }
            }

            foreach (string key in Locations.RandomizedLocations.Keys.ToList()) {
                Check check = Locations.RandomizedLocations[key];
                if (check.Reward.Type == "MONEY") {
                    if ((TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.NORMAL && check.Reward.Amount < 20)
                    || (TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.DOUBLE && check.Reward.Amount <= 20)
                    || (TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.ONSLAUGHT && check.Reward.Amount <= 30)) {
                        check.Reward.Name = "Fool Trap";
                        check.Reward.Type = "FOOL";
                    }
                }
            }

            foreach (string Key in Locations.RandomizedLocations.Keys) {
                int ItemPickedUp = SaveFile.GetInt($"randomizer picked up {Key}");
                Locations.CheckedLocations.Add(Key, ItemPickedUp == 1 ? true : false);
            }
            if (TunicRandomizer.Tracker.ItemsCollected.Count == 0) {
                foreach (KeyValuePair<string, bool> PickedUpItem in Locations.CheckedLocations.Where(item => item.Value)) {
                    Check check = Locations.RandomizedLocations[PickedUpItem.Key];
                    ItemData itemData = ItemLookup.GetItemDataFromCheck(check);
                    TunicRandomizer.Tracker.SetCollectedItem(itemData.Name, false);
                }
                ItemTracker.SaveTrackerFile();
                TunicRandomizer.Tracker.ImportantItems["Flask Container"] += TunicRandomizer.Tracker.ItemsCollected.Where(Item => Item.Name == "Flask Shard").Count() / 3;
                if (SaveFile.GetInt("randomizer started with sword") == 1) {
                    TunicRandomizer.Tracker.ImportantItems["Sword"] += 1;
                }
            }
        }

        private static void Shuffle(List<Reward> Rewards, List<Location> Locations, System.Random random) {
            int n = Rewards.Count;
            int r;
            int l;
            while (n > 1) {
                n--;
                r = random.Next(n + 1);
                l = random.Next(n + 1);

                Reward Reward = Rewards[r];
                Rewards[r] = Rewards[n];
                Rewards[n] = Reward;

                Location Location = Locations[l];
                Locations[l] = Locations[n];
                Locations[n] = Location;
            }
        }

        private static void Shuffle(List<Check> list, System.Random random) {
            int n = list.Count;
            int r;
            while (n > 1) {
                n--;
                r = random.Next(n + 1);

                Check holder = list[r];
                list[r] = list[n];
                list[n] = holder;
            }
        }

        public static Check FindRandomizedItemByName(string Name) {
            foreach (Check Check in Locations.RandomizedLocations.Values) {
                if (Check.Reward.Name == Name) {
                    return Check;
                }
            }
            return null;
        }

        public static List<Check> FindAllRandomizedItemsByName(string Name) {
            List<Check> results = new List<Check>();

            foreach (Check Check in Locations.RandomizedLocations.Values) {
                if (Check.Reward.Name == Name) {
                    results.Add(Check);
                }
            }

            return results;
        }

        public static List<Check> FindAllRandomizedItemsByType(string type) {
            List<Check> results = new List<Check>();

            foreach (Check Check in Locations.RandomizedLocations.Values) {
                if (Check.Reward.Type == type) {
                    results.Add(Check);
                }
            }

            return results;
        }

        // in non-ER, we want the actual sphere 1
        public static Dictionary<string, int> GetSphereOne(Dictionary<string, int> startInventory = null) {
            Dictionary<string, int> Inventory = new Dictionary<string, int>() { { "Overworld", 1 } };
            Dictionary<string, PortalCombo> vanillaPortals = ERScripts.VanillaPortals();
            if (startInventory == null) {
                TunicUtils.AddListToDict(Inventory, PrecollectedItems);
            } else {
                TunicUtils.AddDictToDict(Inventory, startInventory);
            }

            while (true) {
                int start_num = Inventory.Count;
                Inventory = ERScripts.UpdateReachableRegions(Inventory);
                foreach (PortalCombo portalCombo in vanillaPortals.Values) {
                    Inventory = portalCombo.AddComboRegion(Inventory);
                }
                int end_num = Inventory.Count;
                if (start_num == end_num) {
                    break;
                }
            }
            return Inventory;
        }

        // gets all regions that can be reached based on the current inventory
        public static Dictionary<string, int> GetReachableRegions(Dictionary<string, int> inventory = null) {
            Dictionary<string, PortalCombo> portalList;
            if (SaveFile.GetInt(EntranceRando) == 1) {
                portalList = ERData.RandomizedPortals;
            } else {
                portalList = ERScripts.VanillaPortals();
            }

            while (true) {
                int start_num = inventory.Count;
                inventory = ERScripts.UpdateReachableRegions(inventory);
                foreach (PortalCombo portalCombo in portalList.Values) {
                    inventory = portalCombo.AddComboRegion(inventory);
                }
                int end_num = inventory.Count;
                if (start_num == end_num) {
                    break;
                }
            }
            return inventory;
        }

        // In ER, we want sphere 1 to be in Overworld or adjacent to Overworld
        public static Dictionary<string, int> GetERSphereOne(Dictionary<string, int> startInventory = null) {
            List<Portal> PortalInventory = new List<Portal>();
            Dictionary<string, int> Inventory = new Dictionary<string, int>() { { "Overworld", 1 } };

            if (startInventory == null) {
                TunicUtils.AddListToDict(Inventory, PrecollectedItems);
            } else {
                TunicUtils.AddDictToDict(Inventory, startInventory);
            }
            
            Inventory = ERScripts.FirstStepsUpdateReachableRegions(Inventory);
            
            // find which portals you can reach from spawn without additional progression
            foreach (PortalCombo portalCombo in ERData.RandomizedPortals.Values) {
                if (Inventory.ContainsKey(portalCombo.Portal1.Region)) {
                    PortalInventory.Add(portalCombo.Portal2);
                }
                if (Inventory.ContainsKey(portalCombo.Portal2.Region)) {
                    PortalInventory.Add(portalCombo.Portal1);
                }
            }

            // add the regions you can reach as your first steps to the inventory
            foreach (Portal portal in PortalInventory) {
                if (!Inventory.ContainsKey(portal.Region)) {
                    Inventory.Add(portal.Region, 1);
                }
            }
            Inventory = ERScripts.FirstStepsUpdateReachableRegions(Inventory);
            return Inventory;
        }
    }
}
