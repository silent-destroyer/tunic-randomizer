using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunicRandomizer {
    public class TunicUtils {
        // list of the CheckIds for checks that are currently in logic
        public static List<string> ChecksInLogic = new List<string>();
        // progression items the player has received
        public static Dictionary<string, int> PlayerItemsAndRegions = new Dictionary<string, int>();

        // add a key if it doesn't exist, otherwise increment the value by 1
        public static Dictionary<string, int> AddListToDict(Dictionary<string, int> dictionary, List<string> list) {
            foreach (string item in list) {
                dictionary.TryGetValue(item, out var count);
                dictionary[item] = count + 1;
            }
            return dictionary;
        }

        public static Dictionary<string, int> AddStringToDict(Dictionary<string, int> dictionary, string item) {
            dictionary.TryGetValue(item, out var count);
            dictionary[item] = count + 1;
            return dictionary;
        }

        public static Dictionary<string, int> AddDictToDict(Dictionary<string, int> dictionary1, Dictionary<string, int> dictionary2) {
            foreach (KeyValuePair<string, int> pair in dictionary2) {
                dictionary1.TryGetValue(pair.Key, out var count);
                dictionary1[pair.Key] = count + pair.Value;
            }
            return dictionary1;
        }

        // sets ChecksInLogic to contain a list of CheckIds for all checks that are currently in logic with the items you have received
        public static void FindChecksInLogic() {
            PlayerItemsAndRegions.Clear();
            ChecksInLogic.Clear();

            TunicUtils.AddListToDict(PlayerItemsAndRegions, ItemRandomizer.PrecollectedItems);
            PlayerItemsAndRegions.Add("Overworld", 1);

            if (SaveFlags.IsArchipelago()) {
                TunicUtils.AddDictToDict(PlayerItemsAndRegions, Archipelago.instance.integration.GetStartInventory());
                foreach (var itemInfo in Archipelago.instance.integration.session.Items.AllItemsReceived) {
                    string itemName = itemInfo.ItemName;
                    // convert display name to internal name
                    foreach (KeyValuePair<string, string> namePair in ItemLookup.SimplifiedItemNames) {
                        if (namePair.Value == itemName) {
                            itemName = namePair.Key;
                        }
                    }
                    TunicUtils.AddStringToDict(PlayerItemsAndRegions, itemName);
                }
            } else {
                foreach (Check locationCheck in Locations.RandomizedLocations.Values) {
                    if (Locations.CheckedLocations.ContainsKey(locationCheck.CheckId) && Locations.CheckedLocations[locationCheck.CheckId] == true) {
                        TunicUtils.AddStringToDict(PlayerItemsAndRegions, locationCheck.Reward.Name);
                    }
                }
            }
            UpdateChecksInLogic();
        }

        public static void UpdateChecksInLogic() {
            ItemRandomizer.GetReachableRegions(PlayerItemsAndRegions);
            foreach (Check check in Locations.VanillaLocations.Values) {
                if (!ChecksInLogic.Contains(check.CheckId) && check.Location.reachable(PlayerItemsAndRegions) && Locations.CheckedLocations[check.CheckId] == false) {
                    ChecksInLogic.Add(check.CheckId);
                }
            }
        }

    }
}
