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
        // leave these ones alone
        public static bool testBool = false;
        public static bool testBool2 = false;
        
        // essentially fake items for the purpose of logic
        public static Dictionary<string, int> PrecollectedItems = new Dictionary<string, int>();

        public static List<string> LadderItems = ItemLookup.Items.Where(item => item.Value.Type == ItemTypes.LADDER).Select(item => item.Value.Name).ToList();
        public static List<string> FuseItems = ItemLookup.Items.Where(item => item.Value.Type == ItemTypes.FUSE).Select(item => item.Value.Name).ToList();
        public static List<string> BellItems = ItemLookup.Items.Where(item => item.Value.Type == ItemTypes.BELL).Select(item => item.Value.Name).ToList();

        public static Dictionary<string, Check> ProgressionLocations = new Dictionary<string, Check>();

        // plando items, first string is the item name, second is the location id
        public static List<Tuple<string, string>> PlandoItems = new List<Tuple<string, string>>();
        public static bool InitialRandomizationDone = false;
        
        // Items you start with or effectively start with
        public static Dictionary<string, int> PopulatePrecollected() {
            Dictionary<string, int> precollectedItems = new Dictionary<string, int>();
            if (!GetBool(LadderRandoEnabled)) {
                TunicUtils.AddListToDict(precollectedItems, LadderItems);
            }
            if (GetBool(MasklessLogic)) {
                precollectedItems.Add("Mask", 1);
            }
            if (GetBool(LanternlessLogic)) {
                precollectedItems.Add("Lantern", 1);
            }
            if (!GetBool(AbilityShuffle)) {
                TunicUtils.AddListToDict(precollectedItems, new List<string> { "12", "21", "26" });
            }
            if (GetBool(StartWithSword)) {
                precollectedItems.Add("Sword", 1);
            }

            // Fake items to differentiate between fuse/non-fuse rules
            if (GetBool(FuseShuffleEnabled)) {
                precollectedItems.Add(ERData.FUSE_SHUFFLE, 1);
                if (GetBool(EntranceRando)) {
                    // Since the elevator is always active in ER, just ignore it in logic
                    precollectedItems.Add("Cathedral Elevator Fuse", 1);
                }
            } else {
                precollectedItems.Add(ERData.NO_FUSE_SHUFFLE, 1);
            }

            if (GetBool(BellShuffleEnabled)) {
                precollectedItems.Add(ERData.BELL_SHUFFLE, 1);
            } else {
                precollectedItems.Add(ERData.NO_BELL_SHUFFLE, 1);
            }

            return precollectedItems;
        }

        public static void RandomizeAndPlaceItems(Random random = null) {
            TunicLogger.LogInfo("Randomizing and placing items");

            if (testLocations) {
                testBool2 = true;
            }

            if (random == null) {
                random = new Random(SaveFile.GetInt("seed"));
            }

            Locations.RandomizedLocations.Clear();
            Locations.CheckedLocations.Clear();
            InitialRandomizationDone = false;

            PopulatePrecollected();
            List<string> ProgressionNames = new List<string> { "Hyperdash", "Wand", "Techbow", "Stundagger", "Trinket Coin", "Lantern", "Stick", "Sword", "Sword Progression", "Key", "Key (House)", "Mask", "Vault Key (Red)", "Shotgun" };
            List<string> Ladders = new List<string>(LadderItems);
            List<string> Fuses = new List<string>(FuseItems);
            List<string> Bells = new List<string>(BellItems);
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
                ProgressionNames.AddRange(Ladders);
            }
            if (SaveFile.GetInt(FuseShuffleEnabled) == 1) {
                ProgressionNames.AddRange(Fuses);
            }
            if (SaveFile.GetInt(BellShuffleEnabled) == 1) {
                ProgressionNames.AddRange(Bells);
            }

            List<Check> InitialItems = TunicUtils.GetAllInUseChecks();
            if (SaveFile.GetInt(GrassRandoEnabled) == 1) {
                ProgressionNames.AddRange(GrassCutters);
            }

            if (SaveFile.GetInt(BreakableShuffleEnabled) == 1) {
                if (!ProgressionNames.Contains("Trinket - Glass Cannon")) {
                    ProgressionNames.Add("Trinket - Glass Cannon");
                }
                foreach (Check check in InitialItems.Where(check => BreakableShuffle.BreakableChecks.ContainsKey(check.CheckId))) {
                    check.Reward.Amount = random.Next(1, 6);
                }
            }

            List<Reward> InitialRewards = new List<Reward>();
            List<Location> InitialLocations = new List<Location>();
            // the list of progression items
            List<Reward> ProgressionRewards = new List<Reward>();
            // inventory of progression items that have not been placed yet
            Dictionary<string, int> UnplacedInventory = new Dictionary<string, int>();
            // this is set at the top now, so it can be referenced by the Blue Prince stuff
            ProgressionLocations.Clear();

            int GoldHexagonsAdded = 0;
            int HexagonsToAdd = TunicUtils.GetMaxGoldHexagons();

            //int KobansAdded = 0;

            if (IsHexQuestWithHexAbilities()) {
                int HexGoal = SaveFile.GetInt(HexagonQuestGoal);
                List<string> abilities = new List<string>() { "prayer", "holy cross", "icebolt" }.OrderBy(r => random.Next()).ToList();
                List<int> ability_unlocks = new List<int>() { (int)(HexGoal / 4f), (int)((HexGoal / 4f) * 2), (int)((HexGoal / 4f) * 3) }.OrderBy(r => random.Next()).ToList();
                if (HexGoal == 3 || HexagonsToAdd == 3 || ability_unlocks.Any(req => req == 0)) {
                    ability_unlocks = new List<int>() { 1, 2, 3 };
                }
                for (int i = 0; i < 3; i++) {
                    int index = random.Next(abilities.Count);
                    int index2 = random.Next(ability_unlocks.Count);
                    SaveFile.SetInt($"randomizer hexagon quest {abilities[index]} requirement", ability_unlocks[index2]);
                    abilities.RemoveAt(index);
                    ability_unlocks.RemoveAt(index2);
                }
            }
            Shuffle(InitialItems, random);

            if (GetBool(KeysBehindBosses)) {
                List<Check> bossChecks = new List<Check>();
                for (int i = 0; i < InitialItems.Count; i++) {
                    if (InitialItems[i].Reward.Name.Contains("Hexagon") || InitialItems[i].Reward.Name == "Vault Key (Red)") {
                        bossChecks.Add(InitialItems[i]);
                        InitialItems.RemoveAt(i);
                    }
                }
                InitialItems.AddRange(bossChecks);
                InitialItems.Reverse();
            }

            foreach (Check Item in InitialItems) {
                bool lockedItem = false;

                if (SaveFile.GetInt(KeysBehindBosses) != 0) {
                    if (Item.Reward.Name == "Vault Key (Red)") {
                        Item.Reward.Name = "Hexagon Red";
                    } else if (Item.Reward.Name == "Hexagon Red") {
                        Item.Reward.Name = "Vault Key (Red)";
                    }
                }
                if (SaveFile.GetInt(SwordProgressionEnabled) != 0 && (Item.Reward.Name == "Stick" || Item.Reward.Name == "Sword" || Item.Location.LocationId == "5")) {
                    Item.Reward.Name = "Sword Progression";
                    Item.Reward.Type = "SPECIAL";
                }
                if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                    if (Item.Reward.Type == "PAGE") {
                        if (!(IsHexQuestWithPageAbilities() && abilityPages.Contains(Item.Reward.Name))) {
                            string FillerItem = ItemLookup.FillerItems.Keys.ToList()[random.Next(ItemLookup.FillerItems.Count)];
                            Item.Reward.Name = FillerItem;
                            Item.Reward.Type = FillerItem == "money" ? "MONEY" : "INVENTORY";
                            Item.Reward.Amount = ItemLookup.FillerItems[FillerItem][random.Next(ItemLookup.FillerItems[FillerItem].Count)];
                        }
                    }
                    if (Item.Reward.Name.Contains("Hexagon")) {
                        if (SaveFile.GetInt(KeysBehindBosses) == 1 && GoldHexagonsAdded < HexagonsToAdd) {
                            lockedItem = true;
                            Item.Reward.Name = "Hexagon Gold";
                            Item.Reward.Type = "SPECIAL";
                            Item.Reward.Amount = 1;
                            GoldHexagonsAdded++;
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

                // in case we use this for something
                //if (GetBool(FoxPrinceEnabled) && ItemLookup.FillerItems.ContainsKey(Item.Reward.Name) && KobansAdded < 15) {
                //    Item.Reward.Name = "Koban";
                //    Item.Reward.Amount = 1;
                //    Item.Reward.Type = "INVENTORY";
                //    KobansAdded++;
                //}

                if ((ProgressionNames.Contains(Item.Reward.Name) || ItemLookup.FairyLookup.Keys.Contains(Item.Reward.Name)) && !lockedItem) {
                    ProgressionRewards.Add(Item.Reward);
                } else {
                    InitialRewards.Add(Item.Reward);
                }
                InitialLocations.Add(Item.Location);
            }

            void addToPlandoItems(string item, string location) {
                PlandoItems.Add(new Tuple<string, string>(item, location));
            }

            if (SaveFile.GetInt(LaurelsLocation) == 1) {
                addToPlandoItems("Hyperdash", "Well Reward (6 Coins)");
            } else if (SaveFile.GetInt(LaurelsLocation) == 2) {
                addToPlandoItems("Hyperdash", "Well Reward (10 Coins)");
            } else if (SaveFile.GetInt(LaurelsLocation) == 3) {
                addToPlandoItems("Hyperdash", "waterfall");
            }

            if (GetBool(KeysBehindBosses)) {
                List<string> hexes;
                if (GetBool(HexagonQuestEnabled)) {
                    hexes = new List<string> { "Hexagon Gold", "Hexagon Gold", "Hexagon Gold" };
                } else {
                    hexes = new List<string> { "Hexagon Red", "Hexagon Blue", "Hexagon Green" };
                    TunicUtils.ShuffleList(hexes, SaveFile.GetInt("seed"));
                }
                addToPlandoItems(hexes[0], "Vault Key (Red)");
                addToPlandoItems(hexes[1], "Hexagon Green");
                addToPlandoItems(hexes[2], "Hexagon Blue");
            }

            foreach (Tuple<string, string> plandoPair in PlandoItems) {
                foreach (Reward item in ProgressionRewards.ToList()) {
                    if (item.Name == plandoPair.Item1) {
                        foreach (Location location in InitialLocations.ToList()) {
                            if (location.LocationId == plandoPair.Item2) {
                                Check check = new Check(item, location);
                                string dictionaryId = check.CheckId;
                                ProgressionLocations.Add(dictionaryId, check);
                                InitialLocations.Remove(location);
                                ProgressionRewards.Remove(item);
                                break;
                            }
                        }
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
                if (GetBool(FoxPrinceEnabled)) {
                    FoxPrince.ClearFoxPrinceCaches();
                }
                ERScripts.CreateRandomizedPortals(SaveFile.GetInt("seed"));
            } else {
                ERData.RandomizedPortals = ERData.GetVanillaPortals();
            }

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
                TunicUtils.AddDictToDict(FullInventory, PrecollectedItems);

                // fill method: reverse fill and anything you can get with your remaining inventory is assumed to be in your inventory for the purpose of placing the next item
                (FullInventory, _) = ERScripts.UpdateReachableRegionsAndPickUpItems(FullInventory);


                // this is for testing fill, ignore if not testing
                // using a full inventory of items, checks whether each location is reachable, and prints an error if any aren't
                // change the testLocations bool to true to have it to test whether all locations can be reached
                if (testBool2) {
                    TunicLogger.LogInfo("test starts here");
                    Dictionary<string, int> testUnplacedInventory = new Dictionary<string, int>();
                    Dictionary<string, int> testFullInventory = new Dictionary<string, int>();
                    List<Check> testChecksAlreadyAdded = new List<Check>();

                    TunicUtils.AddDictToDict(testUnplacedInventory, UnplacedInventory);
                    TunicUtils.AddStringToDict(testUnplacedInventory, itemName);

                    // this should keep looping until every portal either doesn't give a reward, or has already given its reward
                    testFullInventory.Clear();
                    testFullInventory.Add("Overworld", 1);
                    TunicUtils.AddDictToDict(testFullInventory, testUnplacedInventory);
                    TunicUtils.AddDictToDict(testFullInventory, PrecollectedItems);

                    // fill up our FullInventory with regions until we stop getting new regions -- these are the portals and regions we can currently reach
                    while (true) {
                        int start_num = 0;
                        foreach (int count in testFullInventory.Values) {
                            start_num += count;
                        }
                        testFullInventory = ERScripts.UpdateReachableRegions(testFullInventory);
                        foreach (PortalCombo portalCombo in ERData.RandomizedPortals) {
                            testFullInventory = portalCombo.AddComboRegion(testFullInventory);
                        }

                        foreach (Check placedLocation in ProgressionLocations.Values) {
                            if (placedLocation.Location.reachable(testFullInventory) && !testChecksAlreadyAdded.Contains(placedLocation)) {
                                string item_name = ItemLookup.FairyLookup.Keys.Contains(placedLocation.Reward.Name) ? "Fairy" : placedLocation.Reward.Name;
                                TunicUtils.AddStringToDict(testFullInventory, item_name);
                                testChecksAlreadyAdded.Add(placedLocation);
                            }
                        }

                        int end_num = 0;
                        foreach (int count in testFullInventory.Values) {
                            end_num += count;
                        }
                        if (start_num == end_num) {
                            break;
                        }
                    }

                    TunicLogger.LogInfo("testing location accessibility now");
                    testBool = true;
                    foreach (Location loc in InitialLocations) {
                        if (!loc.reachable(testFullInventory)) {
                            TunicLogger.LogInfo("Location " + loc.LocationId + " is not reachable, investigate");
                        }
                    }
                    TunicLogger.LogInfo("test ends here, if you didn't see any locations above this message then there were no unreachable locations");
                    testBool = false;
                    testBool2 = false;
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
                        LogicChecker.WriteLogicSummaryFile();
                        TunicLogger.LogInfo("Logic file has been written");
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

            // Add grass checks back in that shouldn't be randomized (ones that are affected by clear early bushes)
            if (SaveFile.GetInt(GrassRandoEnabled) == 1) {
                foreach (KeyValuePair<string, Check> pair in GrassRandomizer.GrassChecks.Where(grass => GrassRandomizer.ExcludedGrassChecks.Contains(grass.Key))) {
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
            int foolTrapsAdded = 0;
            foreach (string key in Locations.RandomizedLocations.Keys.ToList()) {
                Check check = Locations.RandomizedLocations[key];
                if (check.Reward.Type == "MONEY") {
                    if (((TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.NORMAL && check.Reward.Amount < 20)
                    || (TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.DOUBLE && check.Reward.Amount <= 20)
                    || (TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.ONSLAUGHT && check.Reward.Amount <= 30))
                    && foolTrapsAdded < ItemLookup.FoolTrapAmounts[TunicRandomizer.Settings.FoolTrapIntensity]) {
                        check.Reward.Name = "Fool Trap";
                        check.Reward.Type = "FOOL";
                        foolTrapsAdded++;
                    }
                }
            }

            if (SaveFlags.GetBool(FoxPrinceEnabled)) {
                int diceAdded = 0;
                int dartsAdded = 0;
                foreach (string key in Locations.RandomizedLocations.Keys.ToList()) {
                    Check check = Locations.RandomizedLocations[key];
                    bool swapCheck = false;
                    // prioritize swapping grass/money first on other settings, then swap filler items
                    if (GetBool(GrassRandoEnabled)) {
                        if (check.Reward.Name == "Grass" && !GrassRandomizer.ExcludedGrassChecks.Contains(check.CheckId)) {
                            swapCheck = true;
                        }
                    } else if (GetBool(BreakableShuffleEnabled)) {
                        if (check.Reward.Name == "money" && check.Reward.Amount < 10) {
                            swapCheck = true;
                        }
                    } else if (ItemLookup.FillerItems.ContainsKey(check.Reward.Name)) {
                        swapCheck = true;
                    }
                    if (swapCheck) { 
                        if (diceAdded < 6) {
                            check.Reward.Name = "Soul Dice";
                            check.Reward.Type = "INVENTORY";
                            check.Reward.Amount = 1;
                            diceAdded++;
                        } else if (dartsAdded < 3) {
                            TunicLogger.LogInfo("swapping item");
                            check.Reward.Name = "Dart";
                            check.Reward.Type = "INVENTORY";
                            check.Reward.Amount = 1;
                            dartsAdded++;
                        }
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
                if (SaveFile.GetInt(StartWithSword) == 1) {
                    TunicRandomizer.Tracker.ImportantItems["Sword"] += 1;
                }
            }

            InitialRandomizationDone = true;
            TunicUtils.CheckAllLocsReachable();
            TunicLogger.LogInfo("Successfully randomized and placed items!");
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
            List<PortalCombo> vanillaPortals = ERData.GetVanillaPortals();
            if (startInventory == null) {
                TunicUtils.AddDictToDict(Inventory, PrecollectedItems);
            } else {
                TunicUtils.AddDictToDict(Inventory, startInventory);
            }

            while (true) {
                int start_num = Inventory.Count;
                Inventory = ERScripts.UpdateReachableRegions(Inventory);
                foreach (PortalCombo portalCombo in vanillaPortals) {
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
            List<PortalCombo> portalList;
            if (SaveFile.GetInt(EntranceRando) == 1) {
                portalList = ERData.RandomizedPortals;
            } else {
                portalList = ERData.GetVanillaPortals();
            }

            while (true) {
                int start_num = inventory.Count;
                inventory = ERScripts.UpdateReachableRegions(inventory);
                foreach (PortalCombo portalCombo in portalList) {
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
                TunicUtils.AddDictToDict(Inventory, PrecollectedItems);
            } else {
                TunicUtils.AddDictToDict(Inventory, startInventory);
            }
            
            Inventory = ERScripts.UpdateReachableRegions(Inventory);
            
            // find which portals you can reach from spawn without additional progression
            foreach (PortalCombo portalCombo in ERData.RandomizedPortals) {
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
            Inventory = ERScripts.UpdateReachableRegions(Inventory);
            return Inventory;
        }

        // for testing item model appearances everywhere
        public static void testChangeEveryItem(string itemName, int quantity, string type) {
            foreach (Check check in Locations.RandomizedLocations.Values) { 
                check.Reward.Name = itemName;
                check.Reward.Amount = quantity;
                check.Reward.Type = type;
            }
        }
    }
}
