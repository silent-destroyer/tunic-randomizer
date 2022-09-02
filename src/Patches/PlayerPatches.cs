using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TinyJson;

namespace TunicRandomizer {
    public class PlayerPatches {

        public static bool biggerHeadModeEnabled = false;
        public static float biggerHeadModeMultiplier = 4;

        public static List<Reward> VanillaRewards = new List<Reward>();
        public static List<Location> VanillaLocations = new List<Location>();
        public static List<ItemData> AllItemData = new List<ItemData>();

        public static string SaveName = null;

        public static void Update_PlayerPatches(PlayerCharacter __instance) {
            Cheats.FastForward = Input.GetKey(KeyCode.Semicolon);

            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                bool isNight = !CycleController.IsNight;
                if (isNight) {
                    CycleController.AnimateSunset();
                } else {
                    CycleController.AnimateSunrise();
                }
                CycleController.IsNight = isNight;
                CycleController.nightStateVar.BoolValue = isNight;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                GenericMessage.ShowMessage("\"Seed: " + SaveFile.GetInt("seed") + "\"");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { 
                string Palette = "\"Color Palette\"\n\"-----------------\"\n\"" + "(0) Fur:".PadRight(17) + SaveFile.GetInt("customFoxPalette 0").ToString().PadLeft(2) + "\"\n\"" + "(1) Hair:".PadRight(17) + SaveFile.GetInt("customFoxPalette 1").ToString().PadLeft(2) + "\"\n\"" + "(2) Paws/Nose:".PadRight(17) + SaveFile.GetInt("customFoxPalette 2").ToString().PadLeft(2) + "\"\n\"" + "(3) Tunic:".PadRight(17) + SaveFile.GetInt("customFoxPalette 3").ToString().PadLeft(2) + "\"\n\"" + "(4) Scarf:".PadRight(17) + SaveFile.GetInt("customFoxPalette 4").ToString().PadLeft(2) + "\"";
                GenericMessage.ShowMessage(Palette);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                int ObtainedItemCount = 0;
                int ObtainedItemCountInCurrentScene = 0;
                int TotalItemCountInCurrentScene = 0;
                foreach (string Key in ItemPatches.ItemsPickedUp.Keys) {
                    if (ItemPatches.ItemsPickedUp[Key]) {
                        ObtainedItemCount++;
                    }
                    if (ItemPatches.ItemList[Key].Location.SceneName == ScenePatches.SceneName) {
                        TotalItemCountInCurrentScene++;
                        if (ItemPatches.ItemsPickedUp[Key]) {
                            ObtainedItemCountInCurrentScene++;
                        }
                    }
                }
                GenericMessage.ShowMessage("\"Items Found\"\n\"-----------------\"\n\"In This Area: " + string.Format("{0}/{1}", ObtainedItemCountInCurrentScene, TotalItemCountInCurrentScene).PadLeft(7) + "\"\n\"In All Areas: " + string.Format("{0}/{1}", ObtainedItemCount, (ItemPatches.ItemList.Count - 1)).PadLeft(7) + "\"");
            }

            if (Input.GetKeyDown(KeyCode.Alpha9)) {
                int ItemCount = 0;
                foreach (string Key in ItemPatches.ItemsPickedUp.Keys) {
                    if (ItemPatches.ItemsPickedUp[Key]) {
                        ItemCount++;
                    }
                }
                if (ItemCount >= 150) {
                    if (Inventory.GetItemByName("Hyperdash").Quantity == 0 || Inventory.GetItemByName("Wand").Quantity == 0) {
                        Inventory.GetItemByName("Hyperdash").Quantity = 1;
                        Inventory.GetItemByName("Wand").Quantity = 1;
                        TunicRandomizer.Logger.LogInfo("Granted Hyperdash and Grapple via failsafe option!");
                        LanguageLine FailsafeText = ScriptableObject.CreateInstance<LanguageLine>();
                        FailsafeText.text = "\"The Heir is giving you a \"\n\"second chance...\"";
                        NPCDialogue.DisplayDialogue(FailsafeText, true);
                        ItemPresentation.PresentItem(Inventory.GetItemByName("Hyperdash"));
                    }
                } else {
                    GenericMessage.ShowMessage("\"Not enough items...\"");
                }
            }
        }

        public static void Start_PlayerPatches(PlayerCharacter __instance) {
            
            int seed = SaveFile.GetInt("seed");

            if (seed == 0) {
                seed = new System.Random().Next();
                TunicRandomizer.Logger.LogInfo("Generated new seed: " + seed);
                SaveFile.SetInt("seed", seed);
                SaveFile.SaveToDisk();
            }

            if (SaveName != SaveFile.saveDestinationName || TunicRandomizer.Randomizer == null) {
                TunicRandomizer.Randomizer = new System.Random(seed);
                SaveName = SaveFile.saveDestinationName;

                ItemPatches.ItemList.Clear();
                ItemPatches.ItemsPickedUp.Clear();

                List<ItemData> InitialItems = JSONParser.FromJson<List<ItemData>>(ItemPatches.ItemListJson);
                List<Reward> InitialRewards = new List<Reward>();
                List<Location> InitialLocations = new List<Location>();
                foreach (ItemData item in InitialItems) {
                    InitialRewards.Add(item.Reward);
                    InitialLocations.Add(item.Location);
                }

                // Randomize the rewards and locations
                Shuffle(InitialRewards, InitialLocations);

                for (int i = 0; i < InitialRewards.Count; i++) {
                    string DictionaryId = InitialLocations[i].LocationId + " [" + InitialLocations[i].SceneName + "]";
                    ItemData ItemData = new ItemData(InitialRewards[i], InitialLocations[i]);
                    ItemPatches.ItemList.Add(DictionaryId, ItemData);

                    // Add stick house reward to forest stick location so that either chest will give the correct reward and the other will be opened automatically
                    if (DictionaryId == "19 [Sword Cave]") {
                        ItemPatches.ItemList.Add("19 [Forest Belltower]", new ItemData(InitialRewards[i], new Location("19", "Forest Belltower", 35, "(512.6, 14.0, 51.9)")));
                    }
                }

                foreach (string Key in ItemPatches.ItemList.Keys) {
                    int ItemPickedUp = SaveFile.GetInt("randomizer picked up " + Key);
                    if (ItemPickedUp == 1) {
                        ItemPatches.ItemsPickedUp.Add(Key, true);
                    } else {
                        ItemPatches.ItemsPickedUp.Add(Key, false);
                    }
                }

                String SpoilerLogPath = Application.dataPath + "/SpoilerLog.json"; 
                if (!File.Exists(SpoilerLogPath)) {
                    File.WriteAllText(SpoilerLogPath, "[{\"Seed\": " + seed + "},\n" + JSONWriter.ToJson(ItemPatches.ItemList) + "]");
                } else {
                    File.Delete(SpoilerLogPath);
                    File.WriteAllText(SpoilerLogPath, "[{\"Seed\": " + seed + "},\n" + JSONWriter.ToJson(ItemPatches.ItemList) + "]");
                }
                TunicRandomizer.Logger.LogInfo("Wrote Spoiler Log to " + SpoilerLogPath);
            }
        }

        private static int FindIndexByName(List<Reward> Items, string Name) {
            for (int i = 0; i < Items.Count; i++) {
                if (Items[i].Name == Name) {
                    return i;
                }
            }
            return -1;
        }

        private static void Shuffle(List<Reward> Rewards, List<Location> Locations) {
            int n = Rewards.Count;
            int r;
            int l;
            while (n > 1) {
                n--;
                do {
                    r = TunicRandomizer.Randomizer.Next(n + 1);
                    l = TunicRandomizer.Randomizer.Next(n + 1);
                } while (Locations[l].RequiredItems.Contains(Rewards[r].Name));
                Reward Reward = Rewards[r];
                Rewards[r] = Rewards[n];
                Rewards[n] = Reward;

                Location Location = Locations[l];
                Locations[l] = Locations[n];
                Locations[n] = Location;
            }
        }

        private static void Shuffle<T>(List<T> list) {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = TunicRandomizer.Randomizer.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
