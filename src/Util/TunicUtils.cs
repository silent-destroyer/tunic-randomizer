﻿using System.Collections.Generic;
using System.Linq;

namespace TunicRandomizer {
    public class TunicUtils {
        // list of the CheckIds for checks that are currently in logic
        public static List<string> ChecksInLogic = new List<string>();
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

        // sets ChecksInLogic to contain a list of CheckIds for all checks that are currently in logic with the items you have received
        public static void FindChecksInLogic() {
            PlayerItemsAndRegions.Clear();
            ChecksInLogic.Clear();

            AddListToDict(PlayerItemsAndRegions, ItemRandomizer.PrecollectedItems);
            PlayerItemsAndRegions.Add("Overworld", 1);

            // archipelago and standalone have different methods to state which items have been received
            if (SaveFlags.IsArchipelago()) {
                AddDictToDict(PlayerItemsAndRegions, Archipelago.instance.integration.GetStartInventory());
                foreach (var itemInfo in Archipelago.instance.integration.session.Items.AllItemsReceived) {
                    string itemName = itemInfo.ItemName;
                    // convert display name to internal name
                    foreach (KeyValuePair<string, string> namePair in ItemLookup.SimplifiedItemNames) {
                        if (namePair.Value == itemName) {
                            itemName = namePair.Key;
                        }
                    }
                    AddStringToDict(PlayerItemsAndRegions, itemName);
                }
            } else {
                foreach (Check locationCheck in Locations.RandomizedLocations.Values) {
                    if (Locations.CheckedLocations.ContainsKey(locationCheck.CheckId) && Locations.CheckedLocations[locationCheck.CheckId] == true) {
                        AddStringToDict(PlayerItemsAndRegions, locationCheck.Reward.Name);
                    }
                }
            }
            if (PlayerItemsAndRegions.ContainsKey("Hexagon Gold")) {
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


        // updates PlayerItemsAndRegions based on which items the player has received, then updates ChecksInLogic based on the player's items/accessible regions
        public static void UpdateChecksInLogic() {
            ItemRandomizer.GetReachableRegions(PlayerItemsAndRegions);
            List<Check> checks = Locations.VanillaLocations.Values.ToList();
            if (SaveFile.GetInt(SaveFlags.GrassRandoEnabled) == 1) {
                checks.AddRange(GrassRandomizer.GrassChecks.Values);
            }
            foreach (Check check in checks) {
                // only put in unchecked locations
                if (!ChecksInLogic.Contains(check.CheckId) && check.Location.reachable(PlayerItemsAndRegions) && !Locations.CheckedLocations[check.CheckId] 
                    && ((SaveFlags.IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld) ? SaveFile.GetInt($"randomizer {check.CheckId} was collected") == 0 : true)) {
                    ChecksInLogic.Add(check.CheckId);
                }
            }
        }

    }
}
