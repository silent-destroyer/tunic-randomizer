﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TinyJson;
using System.IO;
using BepInEx.Logging;
using UnityEngine.Playables;
using JetBrains.Annotations;
using System.Net;
using System.Diagnostics.Eventing.Reader;

namespace TunicRandomizer {
    public class ItemRandomizer {
        private static ManualLogSource Logger = TunicRandomizer.Logger;

        public static bool CreateSpoilerLog = true;
        public static Dictionary<string, int> SphereZero = new Dictionary<string, int>();

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
            Logger.LogInfo("laurels option is " + SaveFile.GetInt("randomizer laurels location"));
            ItemPatches.ItemList.Clear();
            ItemPatches.ItemsPickedUp.Clear();

            List<string> ProgressionNames = new List<string>{
                "Hyperdash", "Wand", "Techbow", "Stundagger", "Trinket Coin", "Lantern", "Stick", "Sword", "Sword Progression", "Key", "Key (House)", "Mask", "Vault Key (Red)" };
            if (SaveFile.GetInt("randomizer shuffled abilities") == 1) {
                if (SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST") {
                    ProgressionNames.Add("Hexagon Gold");
                } else {
                    ProgressionNames.Add("12"); // Prayer
                    ProgressionNames.Add("21"); // Holy Cross
                    ProgressionNames.Add("26"); // Ice Rod
                }
            }

            List<ItemData> InitialItems = JSONParser.FromJson<List<ItemData>>(ItemListJson.ItemList);
            List<Reward> InitialRewards = new List<Reward>();
            List<Location> InitialLocations = new List<Location>();
            List<ItemData> Hexagons = new List<ItemData>();
            ItemData Laurels = new ItemData();
            List<Reward> ProgressionRewards = new List<Reward>();
            Dictionary<string, int> UnplacedInventory = new Dictionary<string, int>(SphereZero);
            Dictionary<string, int> SphereZeroInventory = new Dictionary<string, int>(SphereZero);
            Dictionary<string, ItemData> ProgressionLocations = new Dictionary<string, ItemData> { };
            int GoldHexagonsAdded = 0;
            int HexagonsToAdd = (int)Math.Round((100f + TunicRandomizer.Settings.HexagonQuestExtraPercentage) / 100f * TunicRandomizer.Settings.HexagonQuestGoal);
            if (SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST" && SaveFile.GetInt("randomizer shuffled abilities") == 1) {
                int HexGoal = SaveFile.GetInt("randomizer hexagon quest goal");
                List<string> abilities = new List<string>() { "prayer", "holy cross", "ice rod" }.OrderBy(r => TunicRandomizer.Randomizer.Next()).ToList();
                List<int> ability_unlocks = new List<int>() { (int)(HexGoal/4f), (int)((HexGoal/4f)*2), (int)((HexGoal/4f)*3) }.OrderBy(r => TunicRandomizer.Randomizer.Next()).ToList();
                for (int i = 0; i < 3; i++) {
                    int index = TunicRandomizer.Randomizer.Next(abilities.Count);
                    int index2 = TunicRandomizer.Randomizer.Next(ability_unlocks.Count);
                    SaveFile.SetInt($"randomizer hexagon quest {abilities[index]} requirement", ability_unlocks[index2]);
                    abilities.RemoveAt(index);
                    ability_unlocks.RemoveAt(index2);
                }
            }
            Shuffle(InitialItems);
            foreach (ItemData Item in InitialItems) {

                if (SaveFile.GetInt("randomizer keys behind bosses") != 0 && (Item.Reward.Name.Contains("Hexagon") || Item.Reward.Name == "Vault Key (Red)")) {
                    if (Item.Reward.Name == "Hexagon Green" || Item.Reward.Name == "Hexagon Blue") {
                        Hexagons.Add(Item);
                    } else if (Item.Reward.Name == "Vault Key (Red)") {
                        Item.Reward.Name = "Hexagon Red";
                        Hexagons.Add(Item);
                    } else if (Item.Reward.Name == "Hexagon Red") {
                        Item.Reward.Name = "Vault Key (Red)";
                        InitialRewards.Add(Item.Reward);
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
                    if (SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST") {
                        if (Item.Reward.Type == "PAGE" || Item.Reward.Name.Contains("Hexagon")) {
                            string FillerItem = ItemPatches.FillerItems.Keys.ToList()[TunicRandomizer.Randomizer.Next(ItemPatches.FillerItems.Count)];
                            Item.Reward.Name = FillerItem;
                            Item.Reward.Type = FillerItem == "money" ? "MONEY": "INVENTORY";
                            Item.Reward.Amount = ItemPatches.FillerItems[FillerItem][TunicRandomizer.Randomizer.Next(ItemPatches.FillerItems[FillerItem].Count)];
                        }
                        if(ItemPatches.FillerItems.ContainsKey(Item.Reward.Name) && ItemPatches.FillerItems[Item.Reward.Name].Contains(Item.Reward.Amount) && GoldHexagonsAdded < HexagonsToAdd) {
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
                                        Item.Location.RequiredItems[i].Add("Hexagon Gold", SaveFile.GetInt($"randomizer hexagon quest ice rod requirement"));
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
                                        Item.Location.RequiredItemsDoors[i].Add("Hexagon Gold", SaveFile.GetInt($"randomizer hexagon quest ice rod requirement"));
                                    }
                                }
                            }
                        }
                    }
                    if (ProgressionNames.Contains(Item.Reward.Name) || ItemPatches.FairyLookup.Keys.Contains(Item.Reward.Name)) {
                        ProgressionRewards.Add(Item.Reward);
                    } else {
                        InitialRewards.Add(Item.Reward);
                    }
                    InitialLocations.Add(Item.Location);
                }
            }

            // adding the progression rewards to the start inventory, so we can reverse fill
            foreach (Reward item in ProgressionRewards) {
                string itemName = ItemPatches.FairyLookup.Keys.Contains(item.Name) ? "Fairy" : item.Name;
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
            Dictionary<string, int> SceneInventory = new Dictionary<string, int>();
            Dictionary<string, int> CombinedInventory = new Dictionary<string, int>();
            TunicPortals.RandomizePortals(SaveFile.GetInt("seed"));
            int fairyCount = 0;
            bool laurelsPlaced = false;

            // put progression items in locations
            foreach (Reward item in ProgressionRewards.OrderBy(r => TunicRandomizer.Randomizer.Next())) {

                // pick an item
                string itemName = ItemPatches.FairyLookup.Keys.Contains(item.Name) ? "Fairy" : item.Name;
                //Logger.LogInfo("placing the item " + itemName);
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

                // door rando time
                if (SaveFile.GetInt("randomizer entrance rando enabled") == 1) {
                    // this should keep looping until every portal either doesn't give a reward, or has already given its reward
                    int checkP = 0;
                    SceneInventory.Clear();
                    SceneInventory.Add("Overworld Redux", 1);
                    // fill up our SceneInventory with scenes until we stop getting new scenes -- these are of the portals and regions we can currently reach
                    //Logger.LogInfo("number of portals is " + TunicPortals.RandomizedPortals.Count.ToString());
                    while (checkP < TunicPortals.RandomizedPortals.Count) {
                        checkP = 0;
                        CombinedInventory.Clear();
                        foreach (KeyValuePair<string, int> sceneItem in SceneInventory) {
                            CombinedInventory.Add(sceneItem.Key, sceneItem.Value);
                        }
                        foreach (KeyValuePair<string, int> placedItem in UnplacedInventory) {
                            CombinedInventory.Add(placedItem.Key, placedItem.Value);
                        }

                        foreach (PortalCombo portalCombo in TunicPortals.RandomizedPortals.Values) {
                            if (portalCombo.ComboRewards(CombinedInventory).Count > 0) {
                                int testValue = 0;
                                int testValue2 = 0;
                                foreach (string itemDoors in portalCombo.ComboRewards(CombinedInventory)) {
                                    testValue2++;
                                    if (!SceneInventory.ContainsKey(itemDoors)) {
                                        SceneInventory.Add(itemDoors, 1);
                                    } else {
                                        testValue++;
                                    }
                                }
                                if (testValue == testValue2) {
                                    checkP++;
                                }
                            } else {
                                checkP++;
                            }
                        }
                        //Logger.LogInfo("checkP equals " + checkP.ToString());
                    }
                }

                //Logger.LogInfo("scene inventory contains");
                //foreach (string sceneItem in SceneInventory.Keys)
                //{ Logger.LogInfo(sceneItem); }

                // pick a location
                int l;
                l = TunicRandomizer.Randomizer.Next(InitialLocations.Count);

                // empty combined inventory, refill it with whatever is currently in scene inventory and placed inventory
                CombinedInventory.Clear();
                foreach (KeyValuePair<string, int> sceneItem in SceneInventory) {
                    CombinedInventory.Add(sceneItem.Key, sceneItem.Value);
                }
                foreach (KeyValuePair<string, int> placedItem in UnplacedInventory) {
                    CombinedInventory.Add(placedItem.Key, placedItem.Value);
                }

                //foreach (Location loc in InitialLocations)
                //{ if (!loc.reachable(CombinedInventory)) { Logger.LogInfo("location " + loc.SceneName + " " + loc.LocationId + " is not reachable"); } }
                //Logger.LogInfo(InitialLocations[l].SceneName + " " + InitialLocations[l].LocationId);

                // if location isn't reachable with current inventory excluding the item to be placed, pick a new location
                while (!InitialLocations[l].reachable(CombinedInventory)) {
                    //Logger.LogInfo(InitialLocations[l].SceneName + " " + InitialLocations[l].LocationId);
                    //Logger.LogInfo("the above location failed");
                    l = TunicRandomizer.Randomizer.Next(InitialLocations.Count);
                }

                // prepare matched list of progression items and locations
                string DictionaryId = $"{InitialLocations[l].LocationId} [{InitialLocations[l].SceneName}]";
                ItemData ItemData = new ItemData(item, InitialLocations[l]);
                ProgressionLocations.Add(DictionaryId, ItemData);

                InitialLocations.Remove(InitialLocations[l]);
            }
            
            SphereZero = CombinedInventory;

            if (SaveFile.GetInt("randomizer entrance rando enabled") == 1) {
                SphereZero = GetERSphereOne();
            }

            // shuffle remaining rewards and locations
            Shuffle(InitialRewards, InitialLocations);

            for (int i = 0; i < InitialRewards.Count; i++) {
                string DictionaryId = $"{InitialLocations[i].LocationId} [{InitialLocations[i].SceneName}]";
                ItemData ItemData = new ItemData(InitialRewards[i], InitialLocations[i]);
                ItemPatches.ItemList.Add(DictionaryId, ItemData);
            }

            // add progression items and locations back
            foreach (string key in ProgressionLocations.Keys) {
                ItemPatches.ItemList.Add(key, ProgressionLocations[key]);
            }

            if (SaveFile.GetInt("randomizer keys behind bosses") != 0) {
                foreach (ItemData Hexagon in Hexagons) {
                    if (SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST") {
                        Hexagon.Reward.Name = "Hexagon Gold";
                        Hexagon.Reward.Type = "SPECIAL";
                    }
                    string DictionaryId = $"{Hexagon.Location.LocationId} [{Hexagon.Location.SceneName}]";
                    ItemPatches.ItemList.Add(DictionaryId, Hexagon);
                }
            }

            if (TunicRandomizer.Settings.FixedLaurelsOption != 0) {
                string DictionaryId = $"{Laurels.Location.LocationId} [{Laurels.Location.SceneName}]";
                ItemPatches.ItemList.Add(DictionaryId, Laurels);
            }

            if (SaveFile.GetString("randomizer game mode") == "VANILLA") {
                ItemPatches.ItemList.Clear();
                foreach (ItemData item in JSONParser.FromJson<List<ItemData>>(ItemListJson.ItemList)) {
                    if (SaveFile.GetInt("randomizer sword progression enabled") != 0) {
                        if (item.Reward.Name == "Stick" || item.Reward.Name == "Sword" || item.Location.LocationId == "5") {
                            item.Reward.Name = "Sword Progression";
                            item.Reward.Type = "SPECIAL";
                        }
                    }
                    string DictionaryId = $"{item.Location.LocationId} [{item.Location.SceneName}]";
                    ItemPatches.ItemList.Add(DictionaryId, item);
                }
            }

            foreach (string Key in ItemPatches.ItemList.Keys) {
                int ItemPickedUp = SaveFile.GetInt($"randomizer picked up {Key}");
                ItemPatches.ItemsPickedUp.Add(Key, ItemPickedUp == 1 ? true : false);
            }
            if (TunicRandomizer.Tracker.ItemsCollected.Count == 0) {
                foreach (KeyValuePair<string, bool> PickedUpItem in ItemPatches.ItemsPickedUp.Where(item => item.Value)) {
                    ItemPatches.UpdateItemTracker(PickedUpItem.Key);
                }
                ItemTracker.SaveTrackerFile();
                TunicRandomizer.Tracker.ImportantItems["Flask Container"] += TunicRandomizer.Tracker.ItemsCollected.Where(Item => Item.Reward.Name == "Flask Shard").Count() / 3;
                if (SaveFile.GetInt("randomizer started with sword") == 1) {
                    TunicRandomizer.Tracker.ImportantItems["Sword"] += 1;
                }
            }
        }

        private static void Shuffle(List<Reward> Rewards, List<Location> Locations) {
            int n = Rewards.Count;
            int r;
            int l;
            while (n > 1) {
                n--;
                r = TunicRandomizer.Randomizer.Next(n + 1);
                l = TunicRandomizer.Randomizer.Next(n + 1);

                Reward Reward = Rewards[r];
                Rewards[r] = Rewards[n];
                Rewards[n] = Reward;

                Location Location = Locations[l];
                Locations[l] = Locations[n];
                Locations[n] = Location;
            }
        }

        private static void Shuffle(List<ItemData> list) {
            int n = list.Count;
            int r;
            while (n > 1) {
                n--;
                r = TunicRandomizer.Randomizer.Next(n + 1);

                ItemData holder = list[r];
                list[r] = list[n];
                list[n] = holder;
            }
        }

        public static void PopulateHints() {
            Hints.HintMessages.Clear();
            Hints.TrunicHintMessages.Clear();
            ItemData HintItem = null;
            string HintMessage;
            string TrunicHint;
            List<char> Vowels = new List<char>() { 'A', 'E', 'I', 'O', 'U' };
            bool techbowHinted = false;
            bool wandHinted = false;
            bool prayerHinted = false;
            bool hcHinted = false;
            string Scene;
            string ScenePrefix;
            int Seed = SaveFile.GetInt("seed");

            // Mailbox Hint
            List<string> mailboxNames = new List<string>() { "Wand", "Lantern", "Gun", "Techbow", SaveFile.GetInt("randomizer sword progression enabled") != 0 ? "Sword Progression" : "Sword" };
            if (SaveFile.GetInt("randomizer shuffled abilities") == 1 && SaveFile.GetString("randomizer game mode") != "HEXAGONQUEST") {
                mailboxNames.Add("12");
                mailboxNames.Add("21");
            }
            List<ItemData> mailboxHintables = new List<ItemData>();
            foreach (string Item in mailboxNames) {
                mailboxHintables.AddRange(FindAllRandomizedItemsByName(Item));
            }
            Shuffle(mailboxHintables);
            int n = 0;
            while (HintItem == null && n < mailboxHintables.Count) {
                if (mailboxHintables[n].Location.reachable(SphereZero)) {
                    HintItem = mailboxHintables[n];
                }
                n++;
            }
            if (HintItem == null) {
                n = 0;
                while (HintItem == null && n < mailboxHintables.Count) {
                    if (mailboxHintables[n].Location.SceneName == "Trinket Well") {
                        foreach (ItemData itemData in FindAllRandomizedItemsByName("Trinket Coin")) {
                            if (itemData.Location.reachable(SphereZero)) {
                                HintItem = itemData;
                            }
                        }
                    } else if (mailboxHintables[n].Location.SceneName == "Waterfall") {
                        foreach (ItemData itemData in FindAllRandomizedItemsByType("Fairy")) {
                            if (itemData.Location.reachable(SphereZero)) {
                                HintItem = itemData;
                            }
                        }
                    } else if (mailboxHintables[n].Location.SceneName == "Overworld Interiors" && SaveFile.GetInt("randomizer entrance rando enabled") == 0) {
                        ItemData itemData = FindRandomizedItemByName("Key (House)");
                        if (itemData.Location.reachable(SphereZero)) {
                            HintItem = itemData;
                        }
                    } else if (mailboxHintables[n].Location.LocationId == "71" || mailboxHintables[n].Location.LocationId == "73") {
                        foreach (ItemData itemData in FindAllRandomizedItemsByName("Key")) {
                            if (itemData.Location.reachable(SphereZero)) {
                                HintItem = itemData;
                            }
                        }
                    } else if (SaveFile.GetInt("randomizer entrance rando enabled") == 1 && mailboxHintables[n].Location.RequiredItemsDoors.Count == 1 && mailboxHintables[n].Location.RequiredItemsDoors[0].ContainsKey("Mask")
                        || mailboxHintables[n].Location.RequiredItems.Count == 1 && mailboxHintables[n].Location.RequiredItems[0].ContainsKey("Mask")) {
                        ItemData itemData = FindRandomizedItemByName("Mask");
                        if (itemData.Location.reachable(SphereZero)) {
                            HintItem = itemData;
                        }
                    }
                    n++;
                }
            }
            if (HintItem == null) {
                HintMessage = "nO lehjehnd forsaw yor uhrIvuhl, rooin sEker.\nyoo hahv uh difikuhlt rOd uhhehd. \"GOOD LUCK\".";
                TrunicHint = HintMessage;
            } else {
                Scene = Hints.SimplifiedSceneNames[HintItem.Location.SceneName];
                ScenePrefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                HintMessage = $"lehjehnd sehz {ScenePrefix} \"{Scene.ToUpper()}\"\nkuhntAnz wuhn uhv mehnE \"<#00FFFF>FIRST STEPS<#ffffff>\" ahn yor jurnE.";
                TrunicHint = $"lehjehnd sehz {ScenePrefix} {Translations.Translate(Scene, false)}\nkuhntAnz wuhn uhv mehnE <#00FFFF>furst stehps<#ffffff> ahn yor jurnE.";

                if (HintItem.Reward.Name == "Techbow") { techbowHinted = true; }
                if (HintItem.Reward.Name == "Wand") { wandHinted = true; }
                if (HintItem.Reward.Name == "12") { prayerHinted = true; }
                if (HintItem.Reward.Name == "21") { hcHinted = true; }
                PlayerCharacterPatches.MailboxHintId = $"{HintItem.Location.LocationId} [{HintItem.Location.SceneName}]";
            }
            Hints.HintMessages.Add("Mailbox", HintMessage);
            Hints.TrunicHintMessages.Add("Mailbox", TrunicHint);

            // Golden Path hints
            HintItem = FindRandomizedItemByName("Hyperdash");
            Scene = Hints.SimplifiedSceneNames[HintItem.Location.SceneName];
            ScenePrefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
            HintMessage = $"lehjehnd sehz <#FF00FF>suhm%i^ ehkstruhordinArE  <#FFFFFF>[laurels]\n<#FFFFFF>uhwAts yoo aht {ScenePrefix} \"{Scene.ToUpper()}...\"";
            TrunicHint = $"lehjehnd sehz <#FF00FF>suhm%i^ ehkstruhordinArE  <#FFFFFF>[laurels]\n<#FFFFFF>uhwAts yoo aht {ScenePrefix} {Translations.Translate(Scene, false)}\"...\"";

            Hints.HintMessages.Add("Temple Statue", HintMessage);
            Hints.TrunicHintMessages.Add("Temple Statue", TrunicHint);

            List<string> HintItems = new List<string>() { techbowHinted ? "Lantern" : "Techbow", wandHinted ? "Lantern" : "Wand", "Stundagger" };
            if (SaveFile.GetInt("randomizer shuffled abilities") == 1 && SaveFile.GetString("randomizer game mode") != "HEXAGONQUEST") {
                HintItems.Add(prayerHinted ? "Lantern" : "12");
                HintItems.Add(hcHinted ? "Lantern" : "21");
                HintItems.Remove("Stundagger");
            }
            List<string> HintScenes = new List<string>() { "East Forest Relic", "Fortress Relic", "West Garden Relic" };
            while (HintScenes.Count > 0) {
                HintItem = FindRandomizedItemByName(HintItems[TunicRandomizer.Randomizer.Next(HintItems.Count)]);
                string HintScene = HintScenes[TunicRandomizer.Randomizer.Next(HintScenes.Count)];
                if (HintItem.Reward.Name == "12" && HintScene == "Fortress Relic") {
                    continue;
                }
                Scene = Hints.SimplifiedSceneNames[HintItem.Location.SceneName];
                ScenePrefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                HintMessage = $"lehjehnd sehz {ScenePrefix} \"{Scene.ToUpper()}\"\niz lOkAtid awn #uh \"<#ffd700>PATH OF THE HERO<#ffffff>...\"";
                TrunicHint = $"lehjehnd sehz {ScenePrefix} {Translations.Translate(Scene, false)}\niz lOkAtid awn #uh <#ffd700>pah% uhv #uh hErO<#ffffff>\"...\"";
                Hints.HintMessages.Add(HintScene, HintMessage);
                Hints.TrunicHintMessages.Add(HintScene, TrunicHint);

                if (HintScene == "East Forest Relic") {
                    PlayerCharacterPatches.ForestHintId = $"{HintItem.Location.LocationId} [{HintItem.Location.SceneName}]"; 
                } else if (HintScene == "Fortress Relic") {
                    PlayerCharacterPatches.FortressHintId = $"{HintItem.Location.LocationId} [{HintItem.Location.SceneName}]";
                } else if (HintScene == "West Garden Relic") {
                    PlayerCharacterPatches.GardenHintId = $"{HintItem.Location.LocationId} [{HintItem.Location.SceneName}]";
                }

                HintItems.Remove(HintItem.Reward.Name);
                HintScenes.Remove(HintScene);
            }

            // Questagon Hints
            List<string> Hexagons;
            Dictionary<string, string> HexagonColors;
            if (SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST") {
                Hexagons = new List<string>() { "Hexagon Gold", "Hexagon Gold", "Hexagon Gold" };
                HexagonColors = new Dictionary<string, string>() { { "Hexagon Gold", "<#ffd700>" } };
            } else {
                Hexagons = new List<string>() { "Hexagon Red", "Hexagon Green", "Hexagon Blue" };
                HexagonColors = new Dictionary<string, string>() { { "Hexagon Red", "<#FF3333>" }, { "Hexagon Green", "<#33FF33>" }, { "Hexagon Blue", "<#3333FF>" } };
            }
            List<string> HexagonHintAreas = new List<string>() { "Swamp Relic", "Library Relic", "Monastery Relic" };
            for (int i = 0; i < 3; i++) {
                string Hexagon = Hexagons[TunicRandomizer.Randomizer.Next(Hexagons.Count)];
                if (Hexagon == "Hexagon Gold") {
                    HintItem = FindAllRandomizedItemsByName(Hexagon)[i];
                } else {
                    HintItem = FindRandomizedItemByName(Hexagon);
                }
                string HexagonHintArea = HexagonHintAreas[TunicRandomizer.Randomizer.Next(HexagonHintAreas.Count)];
                Scene = Hints.SimplifiedSceneNames[HintItem.Location.SceneName];
                ScenePrefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                HintMessage = $"#A sA {ScenePrefix} \"{Scene.ToUpper()}\" iz \nwAr #uh {HexagonColors[Hexagon]}kwehstuhgawn [hexagram]<#FFFFFF> iz fownd\"...\"";
                TrunicHint = $"#A sA {ScenePrefix} {Translations.Translate(Scene, false)} iz \nwAr #uh {HexagonColors[Hexagon]}kwehstuhgawn [hexagram]<#FFFFFF> iz fownd\"...\"";
                Hints.HintMessages.Add(HexagonHintArea, HintMessage);
                Hints.TrunicHintMessages.Add(HexagonHintArea, TrunicHint);

                if (HexagonHintArea == "Swamp Relic") {
                    PlayerCharacterPatches.SwampHintId = $"{HintItem.Location.LocationId} [{HintItem.Location.SceneName}]";
                } else if (HexagonHintArea == "Library Relic") {
                    PlayerCharacterPatches.LibraryHintId = $"{HintItem.Location.LocationId} [{HintItem.Location.SceneName}]";
                } else if (HexagonHintArea == "Monastery Relic") {
                    PlayerCharacterPatches.MonasteryHintId = $"{HintItem.Location.LocationId} [{HintItem.Location.SceneName}]";
                }

                Hexagons.Remove(Hexagon);
                HexagonHintAreas.Remove(HexagonHintArea);
            }

            if (SaveFile.GetInt("randomizer entrance rando enabled") == 1)
            {
                foreach (PortalCombo Portal in TunicPortals.RandomizedPortals.Values)
                {
                    if (Portal.Portal1.SceneDestinationTag == "Overworld Redux, Forest Belltower_") { 
                        Hints.HintMessages.Add("East Forest Sign", $"\"{Hints.SimplifiedSceneNames[Portal.Portal2.Scene]}\" [arrow_right]"); 
                    }
                    if (Portal.Portal2.SceneDestinationTag == "Overworld Redux, Forest Belltower_") { 
                        Hints.HintMessages.Add("East Forest Sign", $"\"{Hints.SimplifiedSceneNames[Portal.Portal1.Scene]}\" [arrow_right]"); 
                    }
                    
                    if (Portal.Portal1.SceneDestinationTag == "Overworld Redux, Archipelagos Redux_lower") { 
                        Hints.HintMessages.Add("West Garden Sign", $"[arrow_left] \"{Hints.SimplifiedSceneNames[Portal.Portal2.Scene]}\""); 
                    }
                    if (Portal.Portal2.SceneDestinationTag == "Overworld Redux, Archipelagos Redux_lower") { 
                        Hints.HintMessages.Add("West Garden Sign", $"[arrow_left] \"{Hints.SimplifiedSceneNames[Portal.Portal1.Scene]}\""); 
                    }

                    if (Portal.Portal1.SceneDestinationTag == "Overworld Redux, Fortress Courtyard_") { 
                        Hints.HintMessages.Add("Fortress Sign", $"\"{Hints.SimplifiedSceneNames[Portal.Portal2.Scene]}\" [arrow_right]"); 
                    }
                    if (Portal.Portal2.SceneDestinationTag == "Overworld Redux, Fortress Courtyard_") { 
                        Hints.HintMessages.Add("Fortress Sign", $"\"{Hints.SimplifiedSceneNames[Portal.Portal1.Scene]}\" [arrow_right]"); 
                    }

                    if (Portal.Portal1.SceneDestinationTag == "Overworld Redux, Darkwoods Tunnel_") { 
                        Hints.HintMessages.Add("Quarry Sign", $"\"{Hints.SimplifiedSceneNames[Portal.Portal2.Scene]}\" [arrow_up]"); 
                    }
                    if (Portal.Portal2.SceneDestinationTag == "Overworld Redux, Darkwoods Tunnel_") { 
                        Hints.HintMessages.Add("Quarry Sign", $"\"{Hints.SimplifiedSceneNames[Portal.Portal1.Scene]}\" [arrow_up]"); 
                    }

                    if (Portal.Portal1.SceneDestinationTag == "Overworld Redux, Ruins Passage_west") { 
                        Hints.HintMessages.Add("Ruined Hall Sign", $"\"{Hints.SimplifiedSceneNames[Portal.Portal2.Scene]}\" [arrow_right]"); 
                    }
                    if (Portal.Portal2.SceneDestinationTag == "Overworld Redux, Ruins Passage_west") { 
                        Hints.HintMessages.Add("Ruined Hall Sign", $"\"{Hints.SimplifiedSceneNames[Portal.Portal1.Scene]}\" [arrow_right]"); 
                    }

                    if (Portal.Portal1.SceneDestinationTag == "Overworld Redux, Overworld Interiors_house") { 
                        Hints.HintMessages.Add("Town Sign", $"[arrow_left] \"{Hints.SimplifiedSceneNames[Portal.Portal2.Scene]}\""); 
                    }
                    if (Portal.Portal2.SceneDestinationTag == "Overworld Redux, Overworld Interiors_house") { 
                        Hints.HintMessages.Add("Town Sign", $"[arrow_left] \"{Hints.SimplifiedSceneNames[Portal.Portal1.Scene]}\""); 
                    }

                    if (Portal.Portal1.SceneDestinationTag == "East Forest Redux, Sword Access_lower") { 
                        Hints.HintMessages.Add("East East Forest Sign", $"\"{Hints.SimplifiedSceneNames[Portal.Portal2.Scene]}\" [arrow_right]"); 
                    }
                    if (Portal.Portal2.SceneDestinationTag == "East Forest Redux, Sword Access_lower") { 
                        Hints.HintMessages.Add("East East Forest Sign", $"\"{Hints.SimplifiedSceneNames[Portal.Portal1.Scene]}\" [arrow_right]"); 
                    }

                    if (Portal.Portal1.SceneDestinationTag == "East Forest Redux, East Forest Redux Laddercave_lower") {
                        Hints.HintMessages.Add("West East Forest Sign", $"[arrow_left] \"{Hints.SimplifiedSceneNames[Portal.Portal2.Scene]}\"");
                    }
                    if (Portal.Portal2.SceneDestinationTag == "East Forest Redux, East Forest Redux Laddercave_lower") {
                        Hints.HintMessages.Add("West East Forest Sign", $"[arrow_left] \"{Hints.SimplifiedSceneNames[Portal.Portal1.Scene]}\"");
                    }
                }
            }

        }

        public static void PopulateSpoilerLog() {
            int seed = SaveFile.GetInt("seed");
            Dictionary<string, List<string>> SpoilerLog = new Dictionary<string, List<string>>();
            foreach (string Key in Hints.SceneNamesForSpoilerLog.Keys) {
                SpoilerLog[Key] = new List<string>();
            }
            Dictionary<string, string> Descriptions = JSONParser.FromJson<Dictionary<string, string>>(Hints.LocationDescriptionsJson);
            foreach (string Key in ItemPatches.ItemList.Keys) {
                ItemData ItemData = ItemPatches.ItemList[Key];
                string Spoiler = $"\t{(ItemPatches.ItemsPickedUp[Key] ? "x" : "-")} {Descriptions[Key]}: {Hints.SimplifiedItemNames[ItemData.Reward.Name]} x{ItemData.Reward.Amount}";

                if (ItemData.Reward.Type == "MONEY") {
                    if (ItemData.Reward.Amount < 20) {
                        Spoiler += " OR Fool Trap (if set to Normal/Double/Onslaught)";
                    } else if (ItemData.Reward.Amount <= 20) {
                        Spoiler += " OR Fool Trap (if set to Double/Onslaught)";
                    } else if (ItemData.Reward.Amount <= 30) {
                        Spoiler += " OR Fool Trap (if set to Onslaught)";
                    }
                }
                SpoilerLog[ItemData.Location.SceneName].Add(Spoiler);
            }
            List<string> SpoilerLogLines = new List<string> {
                "Seed: " + seed,
                "Lines that start with 'x' instead of '-' represent items that have been collected",
                "Major Items",
            };
            List<string> MajorItems = new List<string>() { "Sword", "Sword Progression", "Stundagger", "Techbow", "Wand", "Hyperdash", "Lantern", "Shield", "Shotgun",
                "Key (House)", "Vault Key (Red)", "Dath Stone", "Relic - Hero Sword", "Relic - Hero Crown", "Relic - Hero Water", "Relic - Hero Pendant HP", "Relic - Hero Pendant SP",
                "Relic - Hero Pendant MP", "Hexagon Red", "Hexagon Green", "Hexagon Blue", "12", "21", "26"
            };
            foreach (string MajorItem in MajorItems) {
                foreach (ItemData Item in FindAllRandomizedItemsByName(MajorItem)) {
                    string Key = $"{Item.Location.LocationId} [{Item.Location.SceneName}]";
                    string Spoiler = $"\t{(ItemPatches.ItemsPickedUp[Key] ? "x" : "-")} {Hints.SimplifiedItemNames[Item.Reward.Name]}: {Hints.SceneNamesForSpoilerLog[Item.Location.SceneName]} - {Descriptions[Key]}";
                    SpoilerLogLines.Add(Spoiler);
                }
            }
            if(SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST" && SaveFile.GetInt("randomizer shuffled abilities") == 1) {
                int Prayer = SaveFile.GetInt($"randomizer hexagon quest prayer requirement");
                int HolyCross = SaveFile.GetInt($"randomizer hexagon quest holy cross requirement");
                int IceRod = SaveFile.GetInt($"randomizer hexagon quest ice rod requirement");
                int Hexagons = SaveFile.GetInt("randomizer inventory quantity Hexagon Gold");

                SpoilerLogLines.Add($"\t{(Hexagons < Prayer ? "-" : "x")} Prayer: {Prayer} Gold Hexagons");
                SpoilerLogLines.Add($"\t{(Hexagons < HolyCross ? "-" : "x")} Holy Cross: {HolyCross} Gold Hexagons");
                SpoilerLogLines.Add($"\t{(Hexagons < IceRod ? "-" : "x")} Ice Rod: {IceRod} Gold Hexagons");
            }
            foreach (string Key in SpoilerLog.Keys) {
                SpoilerLogLines.Add(Hints.SceneNamesForSpoilerLog[Key]);
                SpoilerLog[Key].Sort();
                foreach (string line in SpoilerLog[Key]) {
                    SpoilerLogLines.Add(line);
                }
            }


            if (SaveFile.GetInt("randomizer entrance rando enabled") == 1) {
                List<string> PortalSpoiler = new List<string>();
                SpoilerLogLines.Add("\nEntrance Connections");
                foreach (PortalCombo portalCombo in TunicPortals.RandomizedPortals.Values)
                { PortalSpoiler.Add("\t- " + portalCombo.Portal1.Name + " -- " + portalCombo.Portal2.Name); }
                foreach (string combo in PortalSpoiler)
                { SpoilerLogLines.Add(combo); }
            }

            if (!File.Exists(TunicRandomizer.SpoilerLogPath)) {
                File.WriteAllLines(TunicRandomizer.SpoilerLogPath, SpoilerLogLines);
            } else {
                File.Delete(TunicRandomizer.SpoilerLogPath);
                File.WriteAllLines(TunicRandomizer.SpoilerLogPath, SpoilerLogLines);
            }
        }

        public static ItemData FindRandomizedItemByName(string Name) {
            foreach (ItemData ItemData in ItemPatches.ItemList.Values) {
                if (ItemData.Reward.Name == Name) {
                    return ItemData;
                }
            }
            return null;
        }

        public static List<ItemData> FindAllRandomizedItemsByName(string Name) {
            List<ItemData> results = new List<ItemData>();

            foreach (ItemData ItemData in ItemPatches.ItemList.Values) {
                if (ItemData.Reward.Name == Name) {
                    results.Add(ItemData);
                }
            }

            return results;
        }

        public static List<ItemData> FindAllRandomizedItemsByType(string type) {
            List<ItemData> results = new List<ItemData>();

            foreach (ItemData itemData in ItemPatches.ItemList.Values) {
                if (itemData.Reward.Type == type) {
                    results.Add(itemData);
                }
            }

            return results;
        }

        // Used for getting what sphere 1 is if you have ER on
        // Gives you items in Overworld or items in adjacent scenes
        // will need updating if/when we do a different starting spot
        public static Dictionary<string, int> GetERSphereOne()
        {
            List<Portal> PortalInventory = new List<Portal>();
            Dictionary<string, int> CombinedInventory = new Dictionary<string, int> { { "Overworld Redux", 1 } };

            // add starting sword and abilities if applicable
            if (SaveFile.GetInt("randomizer started with sword") == 1)
            { CombinedInventory.Add("Sword", 1); }
            if (SaveFile.GetInt("randomizer shuffled abilities") == 0)
            {
                CombinedInventory.Add("12", 1);
                CombinedInventory.Add("21", 1);
            }

            // find which portals you can reach from spawn without additional progression
            foreach (PortalCombo portalCombo in TunicPortals.RandomizedPortals.Values)
            {
                if (portalCombo.Portal1.Scene == "Overworld Redux" && portalCombo.Portal1.Region != "Overworld Well to Furnace Rail" && portalCombo.Portal1.Region != "Overworld West Garden Furnace Connector")
                {
                    var portal = portalCombo.Portal1;
                    if (!portal.IgnoreScene && portal.RequiredItemsOr != null)
                    {
                        if (SaveFile.GetInt("randomizer shuffled abilities") == 1 && portal.RequiredItems == null && portal.EntryItems == null && !portal.PrayerPortal)
                        { PortalInventory.Add(portalCombo.Portal2); }
                        else if (SaveFile.GetInt("randomizer shuffled abilities") == 0 && (portal.SceneDestinationTag == "Overworld Redux, Town_FiligreeRoom_" || portal.SceneDestinationTag == "Overworld Redux, EastFiligreeCache_" || portal.PrayerPortal || portal.RequiredItems == null))
                        { PortalInventory.Add(portalCombo.Portal2); }
                    }
                }
                if (portalCombo.Portal2.Scene == "Overworld Redux" && portalCombo.Portal2.Region != "Overworld Well to Furnace Rail" && portalCombo.Portal2.Region != "Overworld West Garden Furnace Connector")
                {
                    var portal = portalCombo.Portal2;
                    if (!portal.IgnoreScene && portal.RequiredItemsOr == null)
                    {
                        if (SaveFile.GetInt("randomizer shuffled abilities") == 1 && portal.RequiredItems == null && portal.EntryItems == null && !portal.PrayerPortal)
                        { PortalInventory.Add(portalCombo.Portal1); }
                        else if (SaveFile.GetInt("randomizer shuffled abilities") == 0 && (portal.SceneDestinationTag == "Overworld Redux, Town_FiligreeRoom_" || portal.SceneDestinationTag == "Overworld Redux, EastFiligreeCache_" || portal.PrayerPortal || portal.RequiredItems == null))
                        { PortalInventory.Add(portalCombo.Portal1); }
                    }
                }
            }

            // add the new portals and any applicable new scenes to the inventory
            foreach (Portal portal in PortalInventory)
            {
                if (!CombinedInventory.ContainsKey(portal.SceneDestinationTag))
                { CombinedInventory.Add(portal.SceneDestinationTag, 1); }
                
                foreach (string reward in portal.Rewards(CombinedInventory))
                {
                    if (!CombinedInventory.ContainsKey(reward))
                    {
                        CombinedInventory.Add(reward, 1);
                    }
                }
            }
            return CombinedInventory;
        }
    }
}
