using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {
    public class TunicUtils {
        // list of the CheckIds for checks that are currently in logic
        public static List<string> ChecksInLogic = new List<string>();
        public static Dictionary<string, List<string>> ChecksInLogicPerScene = new Dictionary<string, List<string>>();
        // items the player has received
        public static Dictionary<string, int> PlayerItemsAndRegions = new Dictionary<string, int>();

        public static void ShuffleList<T>(IList<T> list, int seed) {
            var rng = new System.Random(seed);
            int n = list.Count;

            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        // add keys if they don't exist, otherwise increment their values by 1
        public static Dictionary<string, int> AddListToDict(Dictionary<string, int> dictionary, List<string> list) {
            foreach (string item in list) {
                dictionary.TryGetValue(item, out var count);
                dictionary[item] = count + 1;
            }
            return dictionary;
        }

        // add a key if it doesn't exist, otherwise increment the value by 1
        public static Dictionary<string, int> AddStringToDict(Dictionary<string, int> dictionary, string item) {
            dictionary.TryGetValue(item, out var count);
            dictionary[item] = count + 1;
            return dictionary;
        }

        // for combining dictionaries of item quantities
        public static Dictionary<string, int> AddDictToDict(Dictionary<string, int> primaryDictionary, Dictionary<string, int> secondaryDictionary) {
            foreach (KeyValuePair<string, int> pair in secondaryDictionary) {
                primaryDictionary.TryGetValue(pair.Key, out var count);
                primaryDictionary[pair.Key] = count + pair.Value;
            }
            return primaryDictionary;
        }

        // for changing lists of requirements into dictionaries of them
        public static Dictionary<string, int> ChangeListToDict(List<string> list) {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            foreach (string item in list) {
                if (dictionary.ContainsKey(item)) {
                    dictionary[item]++;
                } else {
                    dictionary[item] = 1;
                }
            }
            return dictionary;
        }

        // for filtering items out in PlayerItemsAndRegions
        public static List<string> AllProgressionNames = new List<string>() {
            "Stick", "Sword", "Sword Upgrade", "Sword Progression", "Stundagger", "Techbow", "Wand", "Hyperdash", "Lantern", "Shotgun",
            "Key (House)", "Key", "Vault Key (Red)", "Trinket Coin", "Hexagon Red", "Hexagon Green", "Hexagon Blue", "Hexagon Gold", "Mask",
            "Fairy", "12", "21", "26", "Trinket - Glass Cannon",
            "Ladders in Overworld Town", "Ladders near Weathervane", "Ladders near Overworld Checkpoint", "Ladder to East Forest",
            "Ladders to Lower Forest", "Ladders near Patrol Cave", "Ladders in Well", "Ladders to West Bell", "Ladder to Quarry",
            "Ladder in Dark Tomb", "Ladders near Dark Tomb", "Ladder near Temple Rafters", "Ladder to Swamp", "Ladders in Swamp",
            "Ladder to Ruined Atoll", "Ladders in South Atoll", "Ladders to Frog's Domain", "Ladders in Hourglass Cave",
            "Ladder to Beneath the Vault", "Ladders in Lower Quarry", "Ladders in Library",
            "Swamp Fuse 1", "Swamp Fuse 2", "Swamp Fuse 3", "Cathedral Elevator Fuse",
            "Quarry Fuse 1", "Quarry Fuse 2", "Ziggurat Miniboss Fuse", "Ziggurat Teleporter Fuse",
            "Fortress Exterior Fuse 1", "Fortress Exterior Fuse 2", "Fortress Courtyard Upper Fuse",
            "Fortress Courtyard Fuse", "Beneath the Vault Fuse", "Fortress Candles Fuse",
            "Fortress Door Left Fuse", "Fortress Door Right Fuse", "West Furnace Fuse",
            "West Garden Fuse", "Atoll Northeast Fuse", "Atoll Northwest Fuse", "Atoll Southeast Fuse",
            "Atoll Southwest Fuse", "Library Lab Fuse", "West Bell", "East Bell"
        };


        // sets ChecksInLogic to contain a list of CheckIds for all checks that are currently in logic with the items you have received
        public static void FindChecksInLogic() {
            PlayerItemsAndRegions.Clear();
            ChecksInLogic.Clear();
            ChecksInLogicPerScene.Clear();

            foreach (string sceneName in Locations.AllScenes) {
                ChecksInLogicPerScene.Add(sceneName, new List<string>());
            }
            ChecksInLogicPerScene.Add("Trinket Well", new List<string>());

            AddDictToDict(PlayerItemsAndRegions, ItemRandomizer.PopulatePrecollected());
            PlayerItemsAndRegions.Add("Overworld", 1);

            // archipelago and standalone have different methods to state which items have been received
            if (SaveFlags.IsArchipelago()) {
                AddDictToDict(PlayerItemsAndRegions, Archipelago.instance.integration.GetStartInventory());
                foreach (var itemInfo in Archipelago.instance.integration.session.Items.AllItemsReceived) {
                    string itemName = itemInfo.ItemDisplayName;
                    // convert display name to internal name
                    foreach (KeyValuePair<string, string> namePair in ItemLookup.SimplifiedItemNames) {
                        // fairies have weird names
                        if (itemName == "Fairy") {
                            break;
                        }
                        if (namePair.Value == itemName) {
                            itemName = namePair.Key;
                            break;
                        }
                    }
                    if (AllProgressionNames.Contains(itemName)) {
                        AddStringToDict(PlayerItemsAndRegions, itemName);
                    }
                }
            } else {
                foreach (Check locationCheck in Locations.RandomizedLocations.Values) {
                    string itemName = ItemLookup.FairyLookup.ContainsKey(locationCheck.Reward.Name) ? "Fairy" : locationCheck.Reward.Name;
                    if (Locations.CheckedLocations.ContainsKey(locationCheck.CheckId) && Locations.CheckedLocations[locationCheck.CheckId] == true
                        && AllProgressionNames.Contains(itemName)) {
                        AddStringToDict(PlayerItemsAndRegions, itemName);
                    }
                }
            }
            if (PlayerItemsAndRegions.ContainsKey("Hexagon Gold") && SaveFile.GetInt(HexagonQuestPageAbilities) != 1) {
                if (PlayerItemsAndRegions["Hexagon Gold"] >= SaveFile.GetInt("randomizer hexagon quest prayer requirement")) {
                    AddStringToDict(PlayerItemsAndRegions, "12");
                }
                if (PlayerItemsAndRegions["Hexagon Gold"] >= SaveFile.GetInt("randomizer hexagon quest holy cross requirement")) {
                    AddStringToDict(PlayerItemsAndRegions, "21");
                }
                if (PlayerItemsAndRegions["Hexagon Gold"] >= SaveFile.GetInt("randomizer hexagon quest icebolt requirement")) {
                    AddStringToDict(PlayerItemsAndRegions, "26");
                }
            }

            UpdateChecksInLogic();
        }

        public static bool HasReq(string req, Dictionary<string, int> inventory) {
            if (req == "Sword") {
                if ((inventory.ContainsKey("Sword Progression") && inventory["Sword Progression"] >= 2) || inventory.ContainsKey("Sword")) {
                    return true;
                }
            } else if (req == "Stick") {
                if (inventory.ContainsKey("Sword Progression") || inventory.ContainsKey("Stick") || inventory.ContainsKey("Sword")) {
                    return true;
                }
            } else if (req == "Heir Sword") {
                if (inventory.ContainsKey("Sword Progression") && inventory["Sword Progression"] >= 4) {
                    return true;
                }
            } else if (req == "12" && IsHexQuestWithHexAbilities()) {
                if (inventory.ContainsKey("Hexagon Gold") && inventory["Hexagon Gold"] >= SaveFile.GetInt(HexagonQuestPrayer)) {
                    return true;
                }
            } else if (req == "21" && IsHexQuestWithHexAbilities()) {
                if (inventory.ContainsKey("Hexagon Gold") && inventory["Hexagon Gold"] >= SaveFile.GetInt(HexagonQuestHolyCross)) {
                    return true;
                }
            } else if (req == "26" && IsHexQuestWithHexAbilities()) {
                if (inventory.ContainsKey("Hexagon Gold") && inventory["Hexagon Gold"] >= SaveFile.GetInt(HexagonQuestIcebolt)) {
                    return true;
                }
            } else if (req == "Key") {  // need both keys or you could potentially use them in the wrong order
                if (inventory.ContainsKey("Key") && inventory["Key"] == 2) {
                    return true;
                }
            } else if (req.StartsWith("IG")) {
                int difficulty = Convert.ToInt32(req.Substring(2, 1));
                string range = req.Substring(3, 1);
                bool met_difficulty = SaveFile.GetInt(IceGrapplingDifficulty) >= difficulty;
                if (met_difficulty && inventory.ContainsKey("Wand") && inventory.ContainsKey("Stundagger")) {
                    if (range == "S") {
                        return true;
                    } else {
                        if (inventory.ContainsKey("Techbow")
                            && (inventory.ContainsKey("26")
                                || (IsHexQuestWithHexAbilities() && inventory.ContainsKey("Hexagon Gold") && inventory["Hexagon Gold"] >= SaveFile.GetInt(HexagonQuestIcebolt)))) {
                            return true;
                        }
                    }
                }
            } else if (req.StartsWith("LS")) {
                int difficulty = Convert.ToInt32(req.Substring(2, 1));
                bool met_difficulty = SaveFile.GetInt(LadderStorageDifficulty) >= difficulty;
                if (met_difficulty &&
                    (GetBool(LadderStorageWithoutItems)
                     || inventory.ContainsKey("Stick") || inventory.ContainsKey("Sword")
                     || inventory.ContainsKey("Shield") || inventory.ContainsKey("Wand")
                     || inventory.ContainsKey("Sword Progression"))) {
                    return true;
                }
            } else if (req == "Zip") {
                if (inventory.ContainsKey("Hyperdash") && GetBool(LaurelsZips)) {
                    return true;
                }
            } else if (inventory.ContainsKey(req)) {
                return true;
            }
            return false;
        }

        public static Dictionary<string, Dictionary<string, List<List<string>>>> DeepCopyTraversalReqs() {
            Dictionary<string, Dictionary<string, List<List<string>>>> deepCopied = new Dictionary<string, Dictionary<string, List<List<string>>>>();
            foreach (KeyValuePair<string, Dictionary<string, List<List<string>>>> kvp in ERData.TraversalReqs) {
                Dictionary<string, List<List<string>>> newDict = new Dictionary<string, List<List<string>>>();
                foreach (KeyValuePair<string, List<List<string>>> kvp2 in kvp.Value) {
                    List<List<string>> newListList = new List<List<string>>();
                    foreach (List<string> list in kvp2.Value) {
                        List<string> newList = new List<string>(list);
                        newListList.Add(list);
                    }
                    newDict.Add(kvp2.Key, newListList);
                }
                deepCopied.Add(kvp.Key, newDict);
            }
            return deepCopied;
        }

        public static LanguageLine CreateLanguageLine(string text) {
            LanguageLine languageLine = ScriptableObject.CreateInstance<LanguageLine>();
            languageLine.text = text;
            return languageLine;
        }

        // updates PlayerItemsAndRegions based on which items the player has received, then updates ChecksInLogic based on the player's items/accessible regions
        public static void UpdateChecksInLogic() {
            ItemRandomizer.GetReachableRegions(PlayerItemsAndRegions);
            List<Check> checks = GetAllInUseChecks();
            foreach (Check check in checks) {
                // only put in unchecked locations
                if (!ChecksInLogic.Contains(check.CheckId) && check.Location.reachable(PlayerItemsAndRegions) && !Locations.CheckedLocations[check.CheckId] 
                    && ((SaveFlags.IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld) ? SaveFile.GetInt($"randomizer {check.CheckId} was collected") == 0 : true)) {
                    ChecksInLogic.Add(check.CheckId);
                    if (!ChecksInLogicPerScene.ContainsKey(check.Location.SceneName)) {
                        ChecksInLogicPerScene.Add(check.Location.SceneName, new List<string>());
                    }
                    ChecksInLogicPerScene[check.Location.SceneName].Add(check.CheckId);
                }
            }
        }

        public static List<Check> GetAllInUseChecks(bool getAll = false, bool exceptGrass = false) {
            // Get a list of all default checks based on settings
            List<Check> checks = Locations.VanillaLocations.Values.ToList();
            if ((SaveFile.GetInt(SaveFlags.GrassRandoEnabled) == 1 || getAll) && !exceptGrass) {
                checks.AddRange(GrassRandomizer.GrassChecks.Values.ToList());
            }
            if (SaveFile.GetInt(SaveFlags.BreakableShuffleEnabled) == 1 || getAll) {
                bool erEnabled = SaveFile.GetInt(SaveFlags.EntranceRando) == 1;
                checks.AddRange(BreakableShuffle.BreakableChecks.Values.ToList().Where(check => erEnabled || check.Location.SceneName != "Purgatory"));
            }
            if (SaveFile.GetInt(SaveFlags.FuseShuffleEnabled) == 1 || getAll) { 
                checks.AddRange(FuseRandomizer.FuseChecks.Values.ToList());
            }
            if (SaveFile.GetInt(SaveFlags.BellShuffleEnabled) == 1 || getAll) {
                checks.AddRange(BellShuffle.BellChecks.Values.ToList());
            }
            return CopyListOfChecks(checks);
        }

        public static Dictionary<string, Check> GetAllInUseChecksDictionary(bool getAll = false) {
            // Get a list of all default checks based on settings
            Dictionary<string, Check> Checks = new Dictionary<string, Check>();
            foreach (Check check in GetAllInUseChecks(getAll)) {
                Checks.Add(check.CheckId, check);
            }
            return Checks;
        }

        public static List<Check> CopyListOfChecks(List<Check> Checks) {
            return Checks.Select(Check => new Check(Check)).ToList();
        }
        
        public static int GetCompletedChecksCountInCurrentScene() {
            return GetCompletedChecksCountByScene(GetAllInUseChecks(), SceneLoaderPatches.SceneName);
        }

        public static int GetCompletedChecksCountByScene(List<Check> checks, string scene) {
            return checks.Where(check => check.Location.SceneName == scene && check.IsCompletedOrCollected).ToList().Count;
        }

        public static int GetCompletedChecksCount(List<Check> checks) {
            return checks.Where(check => check.IsCompletedOrCollected).ToList().Count;
        }

        public static int GetCheckCountInCurrentScene() {
            return GetCheckCountInScene(SceneLoaderPatches.SceneName);
        }

        public static int GetCheckCountInScene(string scene) {
            return GetAllInUseChecks().Where(check => check.Location.SceneName == scene).Count();
        }

        public static int GetMaxGoldHexagons() {
            return Math.Min((int)Math.Round((100f + SaveFile.GetInt(HexagonQuestExtras)) / 100f * SaveFile.GetInt(HexagonQuestGoal)), 100);
        }

        public static bool IsCheckCompletedOrCollected(string CheckId) {
            return SaveFile.GetInt("randomizer picked up " + CheckId) == 1 || (SaveFlags.IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld && SaveFile.GetInt($"randomizer {CheckId} was collected") == 1);
        }

        public static bool IsCheckCompletedInAP(string CheckId) {
            return IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld && SaveFile.GetInt($"randomizer {CheckId} was collected") == 1;
        }

        public static string RemoveParenNumber(string name) {
            name = name.Split('(')[0];
            if (name.EndsWith(" ")) {
                name = name.Remove(name.Length - 1);
            }
            return name;
        }

        public static void TryWriteFile(string filePath, string fileContents) {
            try {
                if (File.Exists(filePath)) {
                    File.Delete(filePath);
                }
                File.WriteAllText(filePath, fileContents);
            } catch (Exception e) {
                TunicLogger.LogError(e.Message + e.Source + e.StackTrace);
            }
        }

        public static void TryWriteFile(string filePath, List<string> fileContents) {
            try {
                if (File.Exists(filePath)) {
                    File.Delete(filePath);
                }
                File.WriteAllLines(filePath, fileContents);
            } catch (Exception e) {
                TunicLogger.LogError(e.Message + e.Source + e.StackTrace);
            }
        }

        public static void TryDeleteFile(string filePath) {
            try {
                if (File.Exists(filePath)) {
                    File.Delete(filePath);
                }
            } catch (Exception e) {
                TunicLogger.LogError(e.Message + e.Source + e.StackTrace);
            }
        }

        public static Vector3 StringToVector3(string Position) {
            Position = Position.Replace("(", "").Replace(")", "");
            string[] coords = Position.Split(',');
            Vector3 vector = new Vector3(float.Parse(coords[0], CultureInfo.InvariantCulture), float.Parse(coords[1], CultureInfo.InvariantCulture), float.Parse(coords[2], CultureInfo.InvariantCulture));
            return vector;
        }

        public static float calcGuiScale() {
            float guiScale = 1f;
            int width = Camera.main.pixelWidth;
            int height = Camera.main.pixelHeight;
            if (width <= 3840 && height <= 2160) {
                guiScale = 1.25f;
            } 
            if (width <= 2560 && height <= 1440) {
                guiScale = 1f;
            }
            if (width <= 1920 && height <= 1080) {
                guiScale = 0.9f;
            }
            if (width <= 1400 && height <= 800) {
                guiScale = 0.75f;
            }
            if (width <= 800 && height <= 600) {
                guiScale = 0.6f;
            }
            return guiScale;
        }

    }
}
