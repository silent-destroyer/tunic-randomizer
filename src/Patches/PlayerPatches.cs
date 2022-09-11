using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine;
using TinyJson;
using System.Linq;

namespace TunicRandomizer {
    public class PlayerPatches {

        public static bool biggerHeadModeEnabled = false;
        public static float biggerHeadModeMultiplier = 4;

        public static List<Reward> VanillaRewards = new List<Reward>();
        public static List<Location> VanillaLocations = new List<Location>();
        public static List<ItemData> AllItemData = new List<ItemData>();

        
        public static string SaveName = null;
        public static bool ShownHeirAssistModePrompt = false;
        public static bool HeirAssistMode = false;
        public static int HeirAssistModeDamageValue = 0;
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
                int ObtainedPagesCount = 0;
                int ObtainedFairiesCount = 0;
                int ObtainedGoldenTrophiesCount = 0;
                foreach (string Key in ItemPatches.ItemsPickedUp.Keys) {
                    if (ItemPatches.ItemsPickedUp[Key]) {
                        ObtainedItemCount++;
                        if (ItemPatches.ItemList[Key].Reward.Type == "PAGE") {
                            ObtainedPagesCount++;
                        }
                        if (ItemPatches.ItemList[Key].Reward.Type == "FAIRY") {
                            ObtainedFairiesCount++;
                        }
                        if (ItemPatches.ItemList[Key].Reward.Name.Contains("GoldenTrophy")) {
                            ObtainedGoldenTrophiesCount++;
                        }
                    }
                    if (ItemPatches.ItemList[Key].Location.SceneName == ScenePatches.SceneName) {
                        TotalItemCountInCurrentScene++;
                        if (ItemPatches.ItemsPickedUp[Key]) {
                            ObtainedItemCountInCurrentScene++;
                        }
                    }
                }
                
                GenericMessage.ShowMessage($"\"Collected Items\"\n\"-----------------\"\n\"Pages......" + string.Format("{0}/{1}", ObtainedPagesCount, 28).PadLeft(9, '.')
                    + "\"\n\"Fairies...." + string.Format("{0}/{1}", ObtainedFairiesCount, 20).PadLeft(9, '.')
                    + "\"\n\"Treasures.." + string.Format("{0}/{1}", ObtainedGoldenTrophiesCount, 12).PadLeft(9, '.')
                    + "\"\n\"This Area.." + string.Format("{0}/{1}", ObtainedItemCountInCurrentScene, TotalItemCountInCurrentScene).PadLeft(9, '.')
                    + "\"\n\"Overall...." + string.Format("{0}/{1}", ObtainedItemCount, (ItemPatches.ItemList.Count - 1)).PadLeft(9, '.') + "\"");
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
                Hints.HintMessages.Clear();
                ItemPatches.CoinsTossed = SaveFile.GetInt("randomizer coins tossed");
                ItemPatches.FairiesCollected = 0;

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

                foreach (string Key in ItemPatches.FairyLookup.Keys) {
                    if (SaveFile.GetInt("randomizer obtained fairy " + Key) == 1) {
                        ItemPatches.FairiesCollected++;
                    }
                }

                string SpoilerLogPath = Application.dataPath + "/SpoilerLog.json"; 
                if (!File.Exists(SpoilerLogPath)) {
                    File.WriteAllText(SpoilerLogPath, "[{\"Seed\": " + seed + "},\n" + JSONWriter.ToJson(ItemPatches.ItemList) + "]");
                } else {
                    File.Delete(SpoilerLogPath);
                    File.WriteAllText(SpoilerLogPath, "[{\"Seed\": " + seed + "},\n" + JSONWriter.ToJson(ItemPatches.ItemList) + "]");
                }
                TunicRandomizer.Logger.LogInfo("Wrote Spoiler Log to " + SpoilerLogPath);
                PopulateHints();
            }
        }

        public static bool OnFlinchlessHit_Patches(Foxgod __instance) {
            if (!ShownHeirAssistModePrompt) {
                GenericPrompt.ShowPrompt($"yoo fEl A \"<#ffd700>strange power\" rehzuhnAti^\nwi%in yoo... wil yoo \"<#ffd700>accept it\"?", (Il2CppSystem.Action)ActivateHeirAssist, null);
                ShownHeirAssistModePrompt = true;
                __instance.Flinch(true);
            }
            if (HeirAssistMode) {
                __instance.hp -= HeirAssistModeDamageValue;
            }
            TunicRandomizer.Logger.LogInfo(__instance.hp);
            return true;
        }

        public static void ActivateHeirAssist() {
            HeirAssistMode = true;
            int damageMultiplier = 0;
            for (int i = 0; i < 12; i++) {
                if (SaveFile.GetInt("inventory quantity GoldenTrophy_" + i) == 1) {
                    damageMultiplier++;
                }
            }
            for (int i = 0; i < 28; i++) {
                if (SaveFile.GetInt("randomizer obtained page " + i) == 1) {
                    damageMultiplier++;
                }
            }
            foreach (string Key in ItemPatches.HeroRelicLookup.Keys) {
                if (SaveFile.GetInt("inventory quantity " + Key) == 1) {
                    damageMultiplier++;
                }
            }
            foreach (string Key in ItemPatches.FairyLookup.Keys) {
                if (SaveFile.GetInt("randomizer obtained fairy " + Key) == 1) {
                    damageMultiplier++;
                }
            }
            TunicRandomizer.Logger.LogInfo(damageMultiplier);
            HeirAssistModeDamageValue = damageMultiplier / 5;
            SFX.PlayAudioClipAtFox(GameObject.FindObjectOfType<Foxgod>().finalDeathSFX);
        }


        public static bool Interact_Patches(Item item, InteractionTrigger __instance) {
            string InteractionLocation = ScenePatches.SceneName + " " + __instance.transform.position;
            TunicRandomizer.Logger.LogInfo(__instance.transform.position);
            if (Hints.HintLocations.ContainsKey(InteractionLocation)) {
                LanguageLine Hint = ScriptableObject.CreateInstance<LanguageLine>();
                Hint.text = Hints.HintMessages[Hints.HintLocations[InteractionLocation]];
                GenericMessage.ShowMessage(Hint);
                return false;
            }
            if (ScenePatches.SceneName == "Waterfall" && __instance.transform.position.ToString() == "(-47.4, 46.9, 3.0)" && ItemPatches.FairiesCollected < 10) {
                GenericMessage.ShowMessage($"\"Locked. (10\" fArEz \"required)\"");
                return false;
            }
            if (ScenePatches.SceneName == "Waterfall" && __instance.transform.position.ToString() == "(-47.5, 45.0, -0.5)" && ItemPatches.FairiesCollected < 20) {
                GenericMessage.ShowMessage($"\"Locked. (20\" fArEz \"required)\"");
                return false;
            }
            return true;
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

        private static void PopulateHints() {
            ItemData HintItem;
            string HintMessage;
            // Mailbox Hint
            HintItem = FindRandomizedItemByName("Lantern");
            HintMessage = $"lehjehnd sehs #E \"" + Hints.SimplifiedSceneNames[HintItem.Location.SceneName] + "\"\nwil hlp yoo \"light the way...\"";
            Hints.HintMessages.Add("Mailbox", HintMessage);
            
            // Golden Path hints
            HintItem = FindRandomizedItemByName("Sword");
            HintMessage = $"lehjehnd sehs #E \"" + Hints.SimplifiedSceneNames[HintItem.Location.SceneName] + "\"\niz lOkAtid awn \"<#ffd700>Path of the Hero<#ffffff>...\"";
            Hints.HintMessages.Add("East Forest Relic", HintMessage);

            List<string> HintItems = new List<string>() { "Techbow", "Stundagger" };
            string FirstItem = HintItems[TunicRandomizer.Randomizer.Next(HintItems.Count)];
            HintItem = FindRandomizedItemByName(FirstItem);
            HintMessage = $"lehjehnd sehs #E \"" + Hints.SimplifiedSceneNames[HintItem.Location.SceneName] + "\"\niz lOkAtid awn \"<#ffd700>Path of the Hero<#ffffff>...\""; ;
            Hints.HintMessages.Add("Fortress Relic", HintMessage);
            HintItems.Remove(FirstItem);
            string SecondItem = HintItems[0];
            HintItem = FindRandomizedItemByName(SecondItem);
            HintMessage = $"lehjehnd sehs #E \"" + Hints.SimplifiedSceneNames[HintItem.Location.SceneName] + "\"\niz lOkAtid awn #E \"<#ffd700>Path of the Hero<#ffffff>...\""; ;
            Hints.HintMessages.Add("West Garden Relic", HintMessage);

            HintItem = FindRandomizedItemByName("Hyperdash");
            HintMessage = $"lehjehnd sehs #E \"" + Hints.SimplifiedSceneNames[HintItem.Location.SceneName] + "\"\niz lOkAtid awn \"<#ffd700>Path of the Hero<#ffffff>...\""; ;
            Hints.HintMessages.Add("Temple Statue", HintMessage);

            // Questagon Hints
            HintItem = FindRandomizedItemByName("Hexagon Red");
            HintMessage = $"#A sA \"" + Hints.SimplifiedSceneNames[HintItem.Location.SceneName] + "\" iz \nwAr #E <#FF3333>kwehstuhgawn [hexagram]<#FFFFFF> iz fownd...";
            Hints.HintMessages.Add("Swamp Relic", HintMessage);

            HintItem = FindRandomizedItemByName("Hexagon Green");
            HintMessage = $"#A sA \"" + Hints.SimplifiedSceneNames[HintItem.Location.SceneName] + "\" iz \nwAr #E <#33FF33>kwehstuhgawn [hexagram]<#FFFFFF> iz fownd...";
            Hints.HintMessages.Add("Library Relic", HintMessage);

            HintItem = FindRandomizedItemByName("Hexagon Blue");
            HintMessage = $"#A sA \"" + Hints.SimplifiedSceneNames[HintItem.Location.SceneName] + "\" iz \nwAr #E <#3333FF>kwehstuhgawn [hexagram]<#FFFFFF> iz fownd...";
            Hints.HintMessages.Add("Monastery Relic", HintMessage);

            // Generic Hints
            List<string> EastForestHintAreas = new List<string>() {"Forest Belltower", "East Forest Redux Interior", "East Forest Redux Laddercave", "Sword Access", "East Forest Redux" };
            Hints.HintMessages.Add("East Forest Sign", CreateGenericHint(EastForestHintAreas, "East Forest"));

            List<string> OverworldHintAreas = new List<string>() { "Overworld Redux", "CubeRoom", "Sword Cave", "EastFiligreeCache", "Overworld Cave", "Ruins Passage", "Ruined Shop", "Town_FiligreeRoom", "Changing Room", "Town Basement", "Maze Room", "Overworld Interiors", "ShopSpecial" };
            Hints.HintMessages.Add("Overworld Sign", CreateGenericHint(OverworldHintAreas, "Overworld"));

            List<string> SwampHintAreas = new List<string>() { "Cathedral Arena", "Swamp Redux 2", "Cathedral Redux" };
            Hints.HintMessages.Add("Swamp Sign", CreateGenericHint(SwampHintAreas, "Swamp"));

            List<string> WestGardenHintAreas = new List<string>() { "Archipelagos Redux", "archipelagos_house" };
            Hints.HintMessages.Add("West Garden Sign", CreateGenericHint(WestGardenHintAreas, "West Garden"));

            List<string> FortressHintAreas = new List<string>() { "Fortress Courtyard", "Fortress Basement", "Fortress Main", "Fortress Reliquary", "Dusty", "Fortress East", "Fortress Arena" };
            Hints.HintMessages.Add("Fortress Sign", CreateGenericHint(WestGardenHintAreas, "Eastern Vault"));

            List<string> QuarryHintAreas = new List<string>() { "Quarry Redux", "Monastery", "ziggurat2020_1", "ziggurat2020_2", "ziggurat2020_3" };
            Hints.HintMessages.Add("Quarry Sign", CreateGenericHint(QuarryHintAreas, "Quarry"));
        }

        private static string CreateGenericHint(List<string> SceneNames, string Scene) {
            Dictionary<string, double> HintWeights = new Dictionary<string, double>(){
                {"Fools", 0},
                {"Pages", 0},
                {"Treasures", 0},
                {"Upgrades", 0}
            };
            foreach (ItemData ItemData in ItemPatches.ItemList.Values) {
                if (SceneNames.Contains(ItemData.Location.SceneName)) {
                    if (ItemData.Reward.Type == "PAGE") {
                        HintWeights["Pages"] += 1.0/28.0;
                    }
                    if (ItemData.Reward.Name.Contains("GoldenTrophy")) {
                        HintWeights["Treasures"] += 1.0/12.0;
                    }
                    if (ItemData.Reward.Name.Contains("Upgrade Offering")) {
                        HintWeights["Upgrades"] += 1.0/22.0;
                    }
                    if (ItemData.Reward.Type == "FOOL") {
                        HintWeights["Fools"] += 1.0/16.0;
                    }
                }
            }
            List<string> maxWeightedItems = new List<string>();
            double maxWeight = new List<double>(HintWeights.Values).Max();
            foreach (string Key in HintWeights.Keys) {
                if (HintWeights[Key] == maxWeight) {
                    maxWeightedItems.Add(Key);
                }
            }
            string chosenHint;
            if (maxWeightedItems.Count == 1) {
                chosenHint = maxWeightedItems[0];
            } else { 
                chosenHint = maxWeightedItems[TunicRandomizer.Randomizer.Next(maxWeightedItems.Count)];
            }
            string HintMessage = "";
            switch (chosenHint) {
                case "Fools":
                    HintMessage = $"lehjehnd sehs #E \"" + Scene + "\" iz \nawn #E \"path of the fool...";
                    break;
                case "Pages":
                    HintMessage = $"lehjehnd sehs #E \"" + Scene + "\" iz \nawn #E \"path of knowledge...";
                    break;
                case "Treasures":
                    HintMessage = $"lehjehnd sehs #E \"" + Scene + "\" iz \nawn #E \"path of treasure...";
                    break;
                case "Upgrades":
                    HintMessage = $"lehjehnd sehs #E \"" + Scene + "\" iz \nawn #E \"path of strength...";
                    break;
                default:
                    break;
            }
            return HintMessage;
        }

        private static ItemData FindRandomizedItemByName(string Name) {
            foreach (ItemData ItemData in ItemPatches.ItemList.Values) {
                if (ItemData.Reward.Name == Name) {
                    return ItemData;
                }
            }
            return null;
        }
    }
}
