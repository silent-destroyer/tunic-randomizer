using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TinyJson;
using System.IO;
using BepInEx.Logging;

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
            if (SaveFile.GetInt("randomizer door rando enabled") == 1)
            {
                SphereZero.Add("Dath Stone", 1);
            }
        }

        public static void RandomizeAndPlaceItems() {
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
            List<Reward> ProgressionRewards = new List<Reward>();
            Dictionary<string, int> PlacedInventory = new Dictionary<string, int>(SphereZero);
            Dictionary<string, ItemData> ProgressionLocations = new Dictionary<string, ItemData> { };

            if (SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST" && SaveFile.GetInt("randomizer shuffled abilities") == 1) {
                List<string> abilities = new List<string>() { "prayer", "holy cross", "ice rod" }.OrderBy(r => TunicRandomizer.Randomizer.Next()).ToList();
                List<int> ability_unlocks = new List<int>() { 5, 10, 15 }.OrderBy(r => TunicRandomizer.Randomizer.Next()).ToList();
                for (int i = 0; i < 3; i++) {
                    int index = TunicRandomizer.Randomizer.Next(abilities.Count);
                    int index2 = TunicRandomizer.Randomizer.Next(ability_unlocks.Count);
                    SaveFile.SetInt($"randomizer hexagon quest {abilities[index]} requirement", ability_unlocks[index2]);
                    abilities.RemoveAt(index);
                    ability_unlocks.RemoveAt(index2);
                }
            }

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
                } else {
                    if (SaveFile.GetInt("randomizer sword progression enabled") != 0) {
                        if (Item.Reward.Name == "Stick" || Item.Reward.Name == "Sword" || Item.Location.LocationId == "5") {
                            Item.Reward.Name = "Sword Progression";
                            Item.Reward.Type = "SPECIAL";
                        }
                    }
                    if (SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST") {
                        if (Item.Reward.Type == "PAGE" || Item.Reward.Name.Contains("Hexagon")) {
                            if (Item.Reward.Name == "0") {
                                Item.Reward.Name = "money";
                                Item.Reward.Type = "MONEY";
                                Item.Reward.Amount = 1;
                            } else {
                                Item.Reward.Name = "Hexagon Gold";
                                Item.Reward.Type = "SPECIAL";
                            }
                        }
                        if (SaveFile.GetInt("randomizer shuffled abilities") == 1) {
                            if (Item.Location.RequiredItems.Count > 0) {
                                for (int i = 0; i < Item.Location.RequiredItems.Count; i++) {
                                    if (Item.Location.RequiredItems[i].ContainsKey("12") && Item.Location.RequiredItems[i].ContainsKey("21")) {
                                        int amt = SaveFile.GetInt($"randomizer hexagon quest prayer requirement") > SaveFile.GetInt($"randomizer hexagon quest holy cross requirement") ? SaveFile.GetInt($"randomizer hexagon quest prayer requirement") : SaveFile.GetInt($"randomizer hexagon quest holy cross requirement");
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

            // put progression items in locations
            foreach (Reward item in ProgressionRewards.OrderBy(r => TunicRandomizer.Randomizer.Next())) {
                // door rando time
                if (SaveFile.GetInt("randomizer door rando enabled") == 1)
                {
                    // start by adding overworld if we don't have it already, change this later if we do random starting location
                    if (!PlacedInventory.ContainsKey("Overworld Redux"))
                    {
                        PlacedInventory.Add("Overworld Redux", 1);
                    }
                    // getting the randomized portal list the same way as we randomize it normally
                    Dictionary<string, PortalCombo> randomizedPortalsList = new Dictionary<string, PortalCombo>(TunicPortals.RandomizePortals(SaveFile.GetInt("seed")));

                    Logger.LogInfo("testing portal rando now");
                    // this should keep looping until every portal either doesn't give a reward, or has already given its reward
                    int checkP = 0;
                    while (checkP < randomizedPortalsList.Count)
                    {
                        checkP = 0;
                        foreach (PortalCombo portalCombo in randomizedPortalsList.Values)
                        {
                            if (portalCombo.ComboRewards(PlacedInventory).Count > 0)
                            {
                                foreach (string itemDoors in portalCombo.ComboRewards(PlacedInventory))
                                {
                                    if (!PlacedInventory.ContainsKey(itemDoors))
                                    {
                                        PlacedInventory.Add(itemDoors, 1);
                                    }
                                    else { checkP++; }
                                }
                            }
                            else { checkP++; }
                        }
                    }
                }

                // pick a location
                int l;
                l = TunicRandomizer.Randomizer.Next(InitialLocations.Count);

                // if location isn't reachable with placed inv, pick a new location
                while (!InitialLocations[l].reachable(PlacedInventory)) {
                    l = TunicRandomizer.Randomizer.Next(InitialLocations.Count);
                }

                // add item to placed inv for future reachability checks
                string itemName = ItemPatches.FairyLookup.Keys.Contains(item.Name) ? "Fairy" : item.Name;
                if (PlacedInventory.Keys.Contains(itemName)) {
                    PlacedInventory[itemName] += 1;
                } else {
                    PlacedInventory.Add(itemName, 1);
                }

                // prepare matched list of progression items and locations
                string DictionaryId = $"{InitialLocations[l].LocationId} [{InitialLocations[l].SceneName}]";
                ItemData ItemData = new ItemData(item, InitialLocations[l]);
                ProgressionLocations.Add(DictionaryId, ItemData);

                InitialLocations.Remove(InitialLocations[l]);
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
                    } else if (mailboxHintables[n].Location.SceneName == "Overworld Interiors") {
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
                    } else if (mailboxHintables[n].Location.RequiredItems.Count == 1 && mailboxHintables[n].Location.RequiredItems[0].ContainsKey("Mask")) {
                        ItemData itemData = FindRandomizedItemByName("Mask");
                        if (itemData.Location.reachable(SphereZero)) {
                            HintItem = itemData;
                        }
                    }
                    n++;
                }
            }
            if (HintItem == null) {
                HintMessage = "nO lehjehnd forsaw yor uhrIvuhl, rooin sEker.\nyoo hahv uh difikuhlt rOd uhhehd.";
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
            HintMessage = $"lehjehnd sehz <#FF00FF>suhm%i^ ehkstruhordinArE\n<#FFFFFF>uhwAts yoo aht {ScenePrefix} \"{Scene.ToUpper()}...\"";
            TrunicHint = $"lehjehnd sehz <#FF00FF>suhm%i^ ehkstruhordinArE\n<#FFFFFF>uhwAts yoo aht {ScenePrefix} {Translations.Translate(Scene, false)}\"...\"";

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
                HintItem = FindRandomizedItemByName(Hexagon);
                string HexagonHintArea = HexagonHintAreas[TunicRandomizer.Randomizer.Next(HexagonHintAreas.Count)];
                Scene = Hints.SimplifiedSceneNames[HintItem.Location.SceneName];
                ScenePrefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                HintMessage = $"#A sA {ScenePrefix} \"{Scene.ToUpper()}\" iz \nwAr #uh {HexagonColors[Hexagon]}kwehstuhgawn [hexagram]<#FFFFFF> iz fownd\"...\"";
                TrunicHint = $"#A sA {ScenePrefix} {Translations.Translate(Scene, false)} iz \nwAr #uh {HexagonColors[Hexagon]}kwehstuhgawn [hexagram]<#FFFFFF> iz fownd\"...\"";
                Hints.HintMessages.Add(HexagonHintArea, HintMessage);
                Hints.TrunicHintMessages.Add(HexagonHintArea, TrunicHint);

                Hexagons.Remove(Hexagon);
                HexagonHintAreas.Remove(HexagonHintArea);
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
            foreach (string Key in SpoilerLog.Keys) {
                SpoilerLogLines.Add(Hints.SceneNamesForSpoilerLog[Key]);
                SpoilerLog[Key].Sort();
                foreach (string line in SpoilerLog[Key]) {
                    SpoilerLogLines.Add(line);
                }
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
    }
}
