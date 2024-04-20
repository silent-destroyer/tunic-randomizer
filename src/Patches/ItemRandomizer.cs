using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using UnityEngine.InputSystem.Utilities;
using Newtonsoft.Json;

namespace TunicRandomizer {
    public class ItemRandomizer {
        private static ManualLogSource Logger = TunicRandomizer.Logger;

        public static Dictionary<string, int> SphereZero = new Dictionary<string, int>();

        // set this to true to test location access
        public static bool testLocations = false;
        // leave this one alone
        public static bool testBool = false;

        public static void PopulateSphereZero() {
            SphereZero.Clear();
            if (SaveFile.GetInt("randomizer shuffled abilities") == 0) {
                SphereZero.Add("12", 1);
                SphereZero.Add("21", 1);
                SphereZero.Add("26", 1);
            }
            if (SaveFile.GetInt("randomizer started with sword") == 1) {
                SphereZero.Add("Sword", 1);
            }
        }

        public static void RandomizeAndPlaceItems() {
            Logger.LogInfo("randomize and place items starting");

            if (testLocations) {
                testBool = true;
            }
            System.Random random = new System.Random(SaveFile.GetInt("seed"));
            Locations.RandomizedLocations.Clear();
            Locations.CheckedLocations.Clear();

            List<string> ProgressionNames = new List<string>{ "Hyperdash", "Wand", "Techbow", "Stundagger", "Trinket Coin", "Lantern", "Stick", "Sword", "Sword Progression", "Key", "Key (House)", "Mask", "Vault Key (Red)" };
            if (SaveFile.GetInt("randomizer shuffled abilities") == 1) {
                if (SaveFile.GetInt(SaveFlags.HexagonQuestEnabled) == 1) {
                    ProgressionNames.Add("Hexagon Gold");
                } else {
                    ProgressionNames.Add("12"); // Prayer
                    ProgressionNames.Add("21"); // Holy Cross
                    ProgressionNames.Add("26"); // Icebolt
                }
            }
            if (SaveFile.GetInt(SaveFlags.LanternlessLogic) == 1) {
                ProgressionNames.Remove("Lantern");
            }
            if (SaveFile.GetInt(SaveFlags.MasklessLogic) == 1) {
                ProgressionNames.Remove("Mask");
            }

            List<Check> InitialItems = JsonConvert.DeserializeObject<List<Check>>(ItemListJson.ItemList);
            List<Reward> InitialRewards = new List<Reward>();
            List<Location> InitialLocations = new List<Location>();
            List<Check> Hexagons = new List<Check>();
            Check Laurels = new Check();
            List<Reward> ProgressionRewards = new List<Reward>();
            Dictionary<string, int> UnplacedInventory = new Dictionary<string, int>(SphereZero);
            Dictionary<string, int> SphereZeroInventory = new Dictionary<string, int>(SphereZero);
            Dictionary<string, Check> ProgressionLocations = new Dictionary<string, Check> { };
            int GoldHexagonsAdded = 0;
            int HexagonsToAdd = (int)Math.Round((100f + SaveFile.GetInt("randomizer hexagon quest extras")) / 100f * SaveFile.GetInt("randomizer hexagon quest goal"));
            if (SaveFile.GetInt(SaveFlags.HexagonQuestEnabled) == 1 && SaveFile.GetInt("randomizer shuffled abilities") == 1) {
                int HexGoal = SaveFile.GetInt("randomizer hexagon quest goal");
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

                if (SaveFile.GetInt(SaveFlags.MasklessLogic) == 1 || SaveFile.GetInt(SaveFlags.LanternlessLogic) == 1) {
                    if (Item.Location.RequiredItems.Count > 0 && Item.Location.RequiredItems.Where(dict => dict.ContainsKey("Mask") || dict.ContainsKey("Lantern")).Count() > 0) {
                        for (int i = 0; i < Item.Location.RequiredItems.Count; i++) {
                            if (Item.Location.RequiredItems[i].ContainsKey("Mask") && SaveFile.GetInt(SaveFlags.MasklessLogic) == 1) {
                                Item.Location.RequiredItems[i].Remove("Mask");
                            }
                            if (Item.Location.RequiredItems[i].ContainsKey("Lantern") && SaveFile.GetInt(SaveFlags.LanternlessLogic) == 1) {
                                Item.Location.RequiredItems[i].Remove("Lantern");
                            }
                        }
                    }
                }

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
                } else if ((SaveFile.GetInt("randomizer laurels location") == 1 && Item.Location.LocationId == "Well Reward (6 Coins)")
                    || (SaveFile.GetInt("randomizer laurels location") == 2 && Item.Location.LocationId == "Well Reward (10 Coins)")
                    || (SaveFile.GetInt("randomizer laurels location") == 3 && Item.Location.LocationId == "waterfall")) {
                    InitialRewards.Add(Item.Reward);
                    Laurels.Location = Item.Location;
                } else if (SaveFile.GetInt("randomizer laurels location") != 0 && Item.Reward.Name == "Hyperdash") {
                    InitialLocations.Add(Item.Location);
                    Laurels.Reward = Item.Reward;
                } else {
                    if (SaveFile.GetInt("randomizer sword progression enabled") != 0 && (Item.Reward.Name == "Stick" || Item.Reward.Name == "Sword" || Item.Location.LocationId == "5")) {
                        Item.Reward.Name = "Sword Progression";
                        Item.Reward.Type = "SPECIAL";
                    }
                    if (SaveFile.GetInt(SaveFlags.HexagonQuestEnabled) == 1) {
                        if (Item.Reward.Type == "PAGE" || Item.Reward.Name.Contains("Hexagon")) {
                            string FillerItem = ItemLookup.FillerItems.Keys.ToList()[random.Next(ItemLookup.FillerItems.Count)];
                            Item.Reward.Name = FillerItem;
                            Item.Reward.Type = FillerItem == "money" ? "MONEY" : "INVENTORY";
                            Item.Reward.Amount = ItemLookup.FillerItems[FillerItem][random.Next(ItemLookup.FillerItems[FillerItem].Count)];
                        }
                        if (ItemLookup.FillerItems.ContainsKey(Item.Reward.Name) && ItemLookup.FillerItems[Item.Reward.Name].Contains(Item.Reward.Amount) && GoldHexagonsAdded < HexagonsToAdd) {
                            Item.Reward.Name = "Hexagon Gold";
                            Item.Reward.Type = "SPECIAL";
                            Item.Reward.Amount = 1;
                            GoldHexagonsAdded++;
                        }
                        if (SaveFile.GetInt("randomizer shuffled abilities") == 1) {
                            if (Item.Location.RequiredItems.Count > 0) {
                                for (int i = 0; i < Item.Location.RequiredItems.Count; i++) {
                                    if (Item.Location.RequiredItems[i].ContainsKey("12") && Item.Location.RequiredItems[i].ContainsKey("21")) {
                                        int amt = Math.Max(SaveFile.GetInt($"randomizer hexagon quest prayer requirement"), SaveFile.GetInt($"randomizer hexagon quest holy cross requirement"));
                                        Item.Location.RequiredItems[i].Remove("12");
                                        Item.Location.RequiredItems[i].Remove("21");
                                        Item.Location.RequiredItems[i].Add("Hexagon Gold", amt);
                                    }
                                    if (Item.Location.RequiredItems[i].ContainsKey("12")) {
                                        Item.Location.RequiredItems[i].Remove("12");
                                        Item.Location.RequiredItems[i].Add("Hexagon Gold", SaveFile.GetInt($"randomizer hexagon quest prayer requirement"));
                                    }
                                    if (Item.Location.RequiredItems[i].ContainsKey("21")) {
                                        Item.Location.RequiredItems[i].Remove("21");
                                        Item.Location.RequiredItems[i].Add("Hexagon Gold", SaveFile.GetInt($"randomizer hexagon quest holy cross requirement"));
                                    }
                                    if (Item.Location.RequiredItems[i].ContainsKey("26")) {
                                        Item.Location.RequiredItems[i].Remove("26");
                                        Item.Location.RequiredItems[i].Add("Hexagon Gold", SaveFile.GetInt($"randomizer hexagon quest icebolt requirement"));
                                    }
                                }
                            }
                            if (Item.Location.RequiredItemsDoors.Count > 0) {
                                for (int i = 0; i < Item.Location.RequiredItemsDoors.Count; i++) {
                                    if (Item.Location.RequiredItemsDoors[i].ContainsKey("12") && Item.Location.RequiredItemsDoors[i].ContainsKey("21")) {
                                        int amt = Math.Max(SaveFile.GetInt($"randomizer hexagon quest prayer requirement"), SaveFile.GetInt($"randomizer hexagon quest holy cross requirement"));
                                        Item.Location.RequiredItemsDoors[i].Remove("12");
                                        Item.Location.RequiredItemsDoors[i].Remove("21");
                                        Item.Location.RequiredItemsDoors[i].Add("Hexagon Gold", amt);
                                    }
                                    if (Item.Location.RequiredItemsDoors[i].ContainsKey("12")) {
                                        Item.Location.RequiredItemsDoors[i].Remove("12");
                                        Item.Location.RequiredItemsDoors[i].Add("Hexagon Gold", SaveFile.GetInt($"randomizer hexagon quest prayer requirement"));
                                    }
                                    if (Item.Location.RequiredItemsDoors[i].ContainsKey("21")) {
                                        Item.Location.RequiredItemsDoors[i].Remove("21");
                                        Item.Location.RequiredItemsDoors[i].Add("Hexagon Gold", SaveFile.GetInt($"randomizer hexagon quest holy cross requirement"));
                                    }
                                    if (Item.Location.RequiredItemsDoors[i].ContainsKey("26")) {
                                        Item.Location.RequiredItemsDoors[i].Remove("26");
                                        Item.Location.RequiredItemsDoors[i].Add("Hexagon Gold", SaveFile.GetInt($"randomizer hexagon quest icebolt requirement"));
                                    }
                                }
                            }
                        }
                    }
                    if (ProgressionNames.Contains(Item.Reward.Name) || ItemLookup.FairyLookup.Keys.Contains(Item.Reward.Name)) {
                        ProgressionRewards.Add(Item.Reward);
                    } else {
                        InitialRewards.Add(Item.Reward);
                    }
                    InitialLocations.Add(Item.Location);
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
            // if laurels location is on, manually add laurels to the unplaced inventory
            if (!UnplacedInventory.ContainsKey("Hyperdash")) {
                UnplacedInventory.Add("Hyperdash", 1);
            }

            // make a scene inventory, so we can keep the item inventory separated. Add overworld to start (change later if we do start rando)
            Dictionary<string, int> FullInventory = new Dictionary<string, int>();
            TunicPortals.RandomizePortals(SaveFile.GetInt("seed"));
            int fairyCount = 0;
            bool laurelsPlaced = false;

            // put progression items in locations
            foreach (Reward item in ProgressionRewards.OrderBy(r => random.Next())) {
                FullInventory.Clear();

                // pick an item
                string itemName = ItemLookup.FairyLookup.Keys.Contains(item.Name) ? "Fairy" : item.Name;
                // remove item from inventory for reachability checks
                if (UnplacedInventory.Keys.Contains(itemName)) {
                    UnplacedInventory[itemName] -= 1;
                }
                if (UnplacedInventory[itemName] == 0) {
                    UnplacedInventory.Remove(itemName);
                }

                if (itemName == "Fairy") {
                    fairyCount++;
                }
                if (SaveFile.GetInt("randomizer laurels location") != 0 && !laurelsPlaced && (
                    (SaveFile.GetInt("randomizer laurels location") == 1 && UnplacedInventory["Trinket Coin"] == 10)
                    || (SaveFile.GetInt("randomizer laurels location") == 2 && UnplacedInventory["Trinket Coin"] == 6)
                    || (SaveFile.GetInt("randomizer laurels location") == 3 && fairyCount == 11))) {
                    // laurels will no longer be accessible, remove it from the pool
                    laurelsPlaced = true;
                    UnplacedInventory.Remove("Hyperdash");
                }

                foreach (KeyValuePair<string, int> unplacedItem in UnplacedInventory) {
                    FullInventory.Add(unplacedItem.Key, unplacedItem.Value);
                }

                if (SaveFile.GetInt(SaveFlags.LanternlessLogic) == 1) {
                    FullInventory.Add("Lantern", 1);
                }
                if (SaveFile.GetInt(SaveFlags.MasklessLogic) == 1) {
                    FullInventory.Add("Mask", 1);
                }

                // door rando time
                if (SaveFile.GetInt("randomizer entrance rando enabled") == 1) {
                    // this should keep looping until every portal either doesn't give a reward, or has already given its reward

                    FullInventory.Clear();
                    FullInventory.Add("Overworld", 1);
                    foreach (KeyValuePair<string, int> unplacedItem in UnplacedInventory) {
                        FullInventory.Add(unplacedItem.Key, unplacedItem.Value);
                    }
                    if (SaveFile.GetInt(SaveFlags.LanternlessLogic) == 1) {
                        FullInventory.Add("Lantern", 1);
                    }
                    if (SaveFile.GetInt(SaveFlags.MasklessLogic) == 1) {
                        FullInventory.Add("Mask", 1);
                    }
                    
                    // fill up our FullInventory with regions until we stop getting new regions -- these are the portals and regions we can currently reach
                    while (true) {
                        int start_num = FullInventory.Count;
                        FullInventory = TunicPortals.UpdateReachableRegions(FullInventory);
                        foreach (PortalCombo portalCombo in TunicPortals.RandomizedPortals.Values) {
                            FullInventory = portalCombo.AddComboRegions(FullInventory);
                        }
                        int end_num = FullInventory.Count;
                        if (start_num == end_num) {
                            break;
                        }
                    }
                }

                if (testBool) {
                    Logger.LogInfo("test starts here");
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

                    if (SaveFile.GetInt(SaveFlags.LanternlessLogic) == 1) {
                        testUnplacedInventory.Add("Lantern", 1);
                    }
                    if (SaveFile.GetInt(SaveFlags.MasklessLogic) == 1) {
                        testUnplacedInventory.Add("Mask", 1);
                    }

                    foreach (KeyValuePair<string, int> testUnplacedItem in testUnplacedInventory) {
                        testFullInventory.Add(testUnplacedItem.Key, testUnplacedItem.Value);
                    }

                    if (SaveFile.GetInt("randomizer entrance rando enabled") == 1) {
                        // this should keep looping until every portal either doesn't give a reward, or has already given its reward

                        testFullInventory.Clear();
                        testFullInventory.Add("Overworld", 1);
                        foreach (KeyValuePair<string, int> unplacedItem in testUnplacedInventory) {
                            testFullInventory.Add(unplacedItem.Key, unplacedItem.Value);
                        }

                        // fill up our FullInventory with regions until we stop getting new regions -- these are the portals and regions we can currently reach
                        while (true) {
                            int start_num = testFullInventory.Count;
                            testFullInventory = TunicPortals.UpdateReachableRegions(testFullInventory);
                            foreach (PortalCombo portalCombo in TunicPortals.RandomizedPortals.Values) {
                                testFullInventory = portalCombo.AddComboRegions(testFullInventory);
                            }
                            int end_num = testFullInventory.Count;
                            if (start_num == end_num) {
                                break;
                            }
                        }
                    }

                    Logger.LogInfo("testing location accessibility now");
                    foreach (Location loc in InitialLocations) {
                        if (!loc.reachable(testFullInventory)) {
                            Logger.LogInfo("Location " + loc.LocationId + " is not reachable, investigate");
                        }
                    }
                    Logger.LogInfo("test ends here");
                    testBool = false;
                }

                // pick a location
                int l;
                l = random.Next(InitialLocations.Count);

                while (!InitialLocations[l].reachable(FullInventory)) {
                    l = random.Next(InitialLocations.Count);
                }

                // prepare matched list of progression items and locations
                string DictionaryId = $"{InitialLocations[l].LocationId} [{InitialLocations[l].SceneName}]";
                Check Check = new Check(item, InitialLocations[l]);
                ProgressionLocations.Add(DictionaryId, Check);

                InitialLocations.Remove(InitialLocations[l]);
            }

            SphereZero = FullInventory;

            // if we're in ER, sphere zero is also the regions we can get to with our starting inventory
            if (SaveFile.GetInt("randomizer entrance rando enabled") == 1) {
                List<string> sphere_zero_list = GetERSphereOne();
                SphereZero.Clear();
                foreach (string sphere_zero_item in sphere_zero_list) {
                    if (!SphereZero.ContainsKey(sphere_zero_item)) {
                        SphereZero.Add(sphere_zero_item, 1);
                    }
                }
            }

            // shuffle remaining rewards and locations
            Shuffle(InitialRewards, InitialLocations, random);

            for (int i = 0; i < InitialRewards.Count; i++) {
                string DictionaryId = $"{InitialLocations[i].LocationId} [{InitialLocations[i].SceneName}]";
                Check Check = new Check(InitialRewards[i], InitialLocations[i]);
                Locations.RandomizedLocations.Add(DictionaryId, Check);
            }

            // add progression items and locations back
            foreach (string key in ProgressionLocations.Keys) {
                Locations.RandomizedLocations.Add(key, ProgressionLocations[key]);
            }

            if (SaveFile.GetInt("randomizer keys behind bosses") != 0) {
                foreach (Check Hexagon in Hexagons) {
                    if (SaveFile.GetInt(SaveFlags.HexagonQuestEnabled) == 1) {
                        Hexagon.Reward.Name = "Hexagon Gold";
                        Hexagon.Reward.Type = "SPECIAL";
                    }
                    string DictionaryId = $"{Hexagon.Location.LocationId} [{Hexagon.Location.SceneName}]";
                    Locations.RandomizedLocations.Add(DictionaryId, Hexagon);
                }
            }

            if (SaveFile.GetInt("randomizer laurels location") != 0) {
                string DictionaryId = $"{Laurels.Location.LocationId} [{Laurels.Location.SceneName}]";
                Locations.RandomizedLocations.Add(DictionaryId, Laurels);
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
                    string DictionaryId = $"{item.Location.LocationId} [{item.Location.SceneName}]";
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

        // In ER, we want sphere 1 to be in Overworld or adjacent to Overworld
        public static List<string> GetERSphereOne() {
            List<Portal> PortalInventory = new List<Portal>();
            List<string> CombinedInventory = new List<string> { "Overworld" };

            // add starting sword and abilities as applicable
            if (SaveFile.GetInt("randomizer started with sword") == 1) {
                CombinedInventory.Add("Sword");
            }
            if (SaveFile.GetInt("randomizer shuffled abilities") == 0) {
                CombinedInventory.Add("12");
                CombinedInventory.Add("21");
            }
            // add these too if you're ignoring them in logic
            if (SaveFile.GetInt(SaveFlags.MasklessLogic) == 1) {
                CombinedInventory.Add("Mask");
            }
            if (SaveFile.GetInt(SaveFlags.LanternlessLogic) == 1) {
                CombinedInventory.Add("Lantern");
            }
            CombinedInventory = TunicPortals.FirstStepsUpdateReachableRegions(CombinedInventory);
            
            // find which portals you can reach from spawn without additional progression
            foreach (PortalCombo portalCombo in TunicPortals.RandomizedPortals.Values) {
                if (CombinedInventory.Contains(portalCombo.Portal1.Region)) {
                    PortalInventory.Add(portalCombo.Portal2);
                }
                if (CombinedInventory.Contains(portalCombo.Portal2.Region)) {
                    PortalInventory.Add(portalCombo.Portal1);
                }
            }

            // add the regions you can reach as your first steps to the inventory
            foreach (Portal portal in PortalInventory) {
                if (!CombinedInventory.Contains(portal.Region)) {
                    CombinedInventory.Add(portal.Region);
                }
            }
            CombinedInventory = TunicPortals.FirstStepsUpdateReachableRegions(CombinedInventory);
            return CombinedInventory;
        }
    }
}
