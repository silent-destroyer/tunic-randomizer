using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine;
using TinyJson;
using System.Linq;

namespace TunicRandomizer {
    public class PlayerCharacterPatches {
        
        public static string SaveName = null;
        public static int HeirAssistModeDamageValue = 0;
        public static bool StungByBee = false;
        public static void PlayerCharacter_Update_PostfixPatch(PlayerCharacter __instance) {
            Cheats.FastForward = Input.GetKey(KeyCode.Semicolon);

            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                if (StateVariable.GetStateVariableByName("Has Been Betrayed").BoolValue || StateVariable.GetStateVariableByName("Has Died To God").BoolValue) {
                    bool isNight = !CycleController.IsNight;
                    if (isNight) {
                        CycleController.AnimateSunset();
                    } else {
                        CycleController.AnimateSunrise();
                    }
                    CycleController.IsNight = isNight;
                    CycleController.nightStateVar.BoolValue = isNight;
                } else {
                    GenericMessage.ShowMessage($"OnlE #Oz hoo \"face The Heir\" R Abl\ntoo\"receive the power of time.\""); 
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                GenericMessage.ShowMessage("\"Seed: " + SaveFile.GetInt("seed") + "\"");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                string FurColor = ColorPalette.Fur[PlayerPalette.selectionIndices[0]].HexValue + "(" + ColorPalette.Fur[PlayerPalette.selectionIndices[0]].ColorName + ") " + PlayerPalette.selectionIndices[0].ToString().PadLeft(2);
                string PuffColor;
                if (PlayerPalette.selectionIndices[1] == 0) {
                    PuffColor = ColorPalette.Fur[PlayerPalette.selectionIndices[0]].HexValue + "(Match Fur)  0";
                } else {
                    PuffColor = ColorPalette.Puff[PlayerPalette.selectionIndices[1]].HexValue + "(" + ColorPalette.Puff[PlayerPalette.selectionIndices[1]].ColorName + ") " + PlayerPalette.selectionIndices[1].ToString().PadLeft(2);
                }
                string DetailsColor = ColorPalette.Details[PlayerPalette.selectionIndices[2]].HexValue + "(" + ColorPalette.Details[PlayerPalette.selectionIndices[2]].ColorName + ") " + PlayerPalette.selectionIndices[2].ToString().PadLeft(2);
                string TunicColor = ColorPalette.Tunic[PlayerPalette.selectionIndices[3]].HexValue + "(" + ColorPalette.Tunic[PlayerPalette.selectionIndices[3]].ColorName + ") " + PlayerPalette.selectionIndices[3].ToString().PadLeft(2);
                string ScarfColor = ColorPalette.Scarf[PlayerPalette.selectionIndices[4]].HexValue + "(" + ColorPalette.Scarf[PlayerPalette.selectionIndices[4]].ColorName + ") " + PlayerPalette.selectionIndices[4].ToString().PadLeft(2);
                string Palette = "\"Color Palette\"\n\"-----------------\"\n\"" + 
                    "(0) Fur:     " + FurColor.PadLeft(25) + "\"\n\"" +
                    "(1) Puff:    " + PuffColor.PadLeft(25) + "\"\n\"" +
                    "(2) Details: " + DetailsColor.PadLeft(25) + "\"\n\"" +
                    "(3) Tunic:   " + TunicColor.PadLeft(25) + "\"\n\"" +
                    "(4) Scarf:   " + ScarfColor.PadLeft(25) + "\"";
                GenericMessage.ShowMessage(Palette);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                int ObtainedItemCount = 0;
                int ObtainedItemCountInCurrentScene = 0;
                int TotalItemCountInCurrentScene = 0;
                int ObtainedPagesCount = 0;
                int ObtainedFairiesCount = 0;
                int ObtainedGoldenTrophiesCount = 0;
                foreach (string Key in RandomItemPatches.ItemsPickedUp.Keys) {
                    if (RandomItemPatches.ItemsPickedUp[Key]) {
                        ObtainedItemCount++;
                        if (RandomItemPatches.ItemList[Key].Reward.Type == "PAGE") {
                            ObtainedPagesCount++;
                        }
                        if (RandomItemPatches.ItemList[Key].Reward.Type == "FAIRY") {
                            ObtainedFairiesCount++;
                        }
                        if (RandomItemPatches.ItemList[Key].Reward.Name.Contains("GoldenTrophy")) {
                            ObtainedGoldenTrophiesCount++;
                        }
                    }
                    if (RandomItemPatches.ItemList[Key].Location.SceneName == SceneLoaderPatches.SceneName) {
                        TotalItemCountInCurrentScene++;
                        if (RandomItemPatches.ItemsPickedUp[Key]) {
                            ObtainedItemCountInCurrentScene++;
                        }
                    }
                }
                string BossesAndKeys = (StateVariable.GetStateVariableByName("SV_Fortress Arena_Spidertank Is Dead").BoolValue ? "<#FF3333>" : "<#FFFFFF>") + "[death]  "
                    + (StateVariable.GetStateVariableByName("Placed Hexagon 1 Red").BoolValue || Inventory.GetItemByName("Hexagon Red").Quantity == 1 ? "<#FF3333>" : "<#FFFFFF>") + "[hexagram]  "
                    + (StateVariable.GetStateVariableByName("Librarian Dead Forever").BoolValue ? "<#33FF33>" : "<#FFFFFF>") + "[death]  "
                    + (StateVariable.GetStateVariableByName("Placed Hexagon 2 Green").BoolValue || Inventory.GetItemByName("Hexagon Green").Quantity == 1 ? "<#33FF33>" : "<#FFFFFF>") + "[hexagram]  "
                    + (StateVariable.GetStateVariableByName("SV_ScavengerBossesDead").BoolValue ? "<#3333FF>" : "<#FFFFFF>") + "[death]  "
                    + (StateVariable.GetStateVariableByName("Placed Hexagon 3 Blue").BoolValue || Inventory.GetItemByName("Hexagon Blue").Quantity == 1 ? "<#3333FF>" : "<#FFFFFF>") + "[hexagram]";
                GenericMessage.ShowMessage($"\"Collected Items\"\n\"-----------------\"\n\"Pages......" + string.Format("{0}/{1}", ObtainedPagesCount, 28).PadLeft(9, '.')
                + "\"\n\"Fairies...." + string.Format("{0}/{1}", ObtainedFairiesCount, 20).PadLeft(9, '.')
                + "\"\n\"Treasures.." + string.Format("{0}/{1}", ObtainedGoldenTrophiesCount, 12).PadLeft(9, '.')
                + "\"\n\"This Area.." + string.Format("{0}/{1}", ObtainedItemCountInCurrentScene, TotalItemCountInCurrentScene).PadLeft(9, '.')
                + "\"\n\"Overall...." + string.Format("{0}/{1}", ObtainedItemCount, (RandomItemPatches.ItemList.Count - 1)).PadLeft(9, '.')
                + "\"\n" + BossesAndKeys);
            }
            System.Random rnd = new System.Random();
            if (Input.GetKeyDown(KeyCode.Alpha5)) {
                PlayerPalette.ChangeColourByDelta(0, rnd.Next(1, 16));
                PlayerPalette.ChangeColourByDelta(1, rnd.Next(1, 12));
                PlayerPalette.ChangeColourByDelta(2, rnd.Next(1, 12));
                PlayerPalette.ChangeColourByDelta(3, rnd.Next(1, 16));
                PlayerPalette.ChangeColourByDelta(4, rnd.Next(1, 11));
            }
            if (StungByBee) {
                __instance.gameObject.transform.Find("Fox/root/pelvis/chest/head").localScale = new Vector3(3f, 3f, 3f);
            }
            TimeSpan timespan = TimeSpan.FromSeconds(SpeedrunData.inGameTime);
            SpeedrunTimerDisplay.instance.timerText.text = timespan.ToString("hh':'mm':'ss'.'ff");

        }

        public static void PlayerCharacter_Start_PostfixPatch(PlayerCharacter __instance) {

            SpeedrunTimerDisplay.Visible = TunicRandomizer.Settings.TimerOverlayEnabled;
            SpeedrunTimerDisplay.instance.sceneText.text = "";
            SpeedrunTimerDisplay.instance.timerText.transform.position = new Vector3(-454.1f, 245.4f, -197.0f);
            SpeedrunTimerDisplay.instance.timerText.fontSize = 64;
            StateVariable.GetStateVariableByName("SV_ShopTrigger_Fortress").BoolValue = true;
            StateVariable.GetStateVariableByName("SV_ShopTrigger_Sewer").BoolValue = true;
            StateVariable.GetStateVariableByName("SV_ShopTrigger_Swamp(Night)").BoolValue = true;
            StateVariable.GetStateVariableByName("SV_ShopTrigger_WestGarden").BoolValue = true;

            int seed = SaveFile.GetInt("seed");

            if (seed == 0) {
                seed = new System.Random().Next();
                TunicRandomizer.Logger.LogInfo("Generated new seed: " + seed);
                SaveFile.SetInt("seed", seed);
                SaveFile.SaveToDisk();
            }

            TunicRandomizer.Logger.LogInfo("Loading seed: " + seed);
            TunicRandomizer.Randomizer = new System.Random(seed);
            SaveName = SaveFile.saveDestinationName;

            RandomItemPatches.ItemList.Clear();
            RandomItemPatches.ItemsPickedUp.Clear();
            Hints.HintMessages.Clear();
            RandomItemPatches.CoinsTossed = SaveFile.GetInt("randomizer coins tossed");
            RandomItemPatches.FairiesCollected = 0;

            List<ItemData> InitialItems = JSONParser.FromJson<List<ItemData>>(RandomItemPatches.ItemListJson);
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
                RandomItemPatches.ItemList.Add(DictionaryId, ItemData);
            }

            foreach (string Key in RandomItemPatches.ItemList.Keys) {
                int ItemPickedUp = SaveFile.GetInt("randomizer picked up " + Key);
                if (ItemPickedUp == 1) {
                    RandomItemPatches.ItemsPickedUp.Add(Key, true);
                } else {
                    RandomItemPatches.ItemsPickedUp.Add(Key, false);
                }
            }
            HeirAssistModeDamageValue = RandomItemPatches.ItemsPickedUp.Values.ToList().Where(item => item == true).ToList().Count / 15;
            
            foreach (string Key in RandomItemPatches.FairyLookup.Keys) {
                if (SaveFile.GetInt("randomizer obtained fairy " + Key) == 1) {
                    RandomItemPatches.FairiesCollected++;
                }
            }

            PopulateSpoilerLog();
            PopulateHints();
        }

        public static bool Foxgod_OnFlinchlessHit_PrefixPatch(Foxgod __instance) {
            if (TunicRandomizer.Settings.HeirAssistModeEnabled) {
                __instance.hp -= HeirAssistModeDamageValue;
            }
            return true;
        }

        public static bool InteractionTrigger_Interact_PrefixPatch(Item item, InteractionTrigger __instance) {
            string InteractionLocation = SceneLoaderPatches.SceneName + " " + __instance.transform.position;

            if (Hints.HintLocations.ContainsKey(InteractionLocation) && TunicRandomizer.Settings.HintsEnabled) {
                LanguageLine Hint = ScriptableObject.CreateInstance<LanguageLine>();
                Hint.text = Hints.HintMessages[Hints.HintLocations[InteractionLocation]];
                GenericMessage.ShowMessage(Hint);
                return false;
            }
            if (SceneLoaderPatches.SceneName == "Waterfall" && __instance.transform.position.ToString() == "(-47.4, 46.9, 3.0)" && RandomItemPatches.FairiesCollected < 10) {
                GenericMessage.ShowMessage($"\"Locked. (10\" fArEz \"required)\"");
                return false;
            }
            if (SceneLoaderPatches.SceneName == "Waterfall" && __instance.transform.position.ToString() == "(-47.5, 45.0, -0.5)" && RandomItemPatches.FairiesCollected < 20) {
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

        public static void PopulateSpoilerLog() {
            string SpoilerLogPath = Application.persistentDataPath + "/RandomizerSpoiler.log";
            int seed = SaveFile.GetInt("seed");
            Dictionary<string, List<string>> SpoilerLog = new Dictionary<string, List<string>>();
            foreach (string Key in Hints.SceneNamesForSpoilerLog.Keys) {
                SpoilerLog[Key] = new List<string>();
            }
            foreach (ItemData ItemData in RandomItemPatches.ItemList.Values) {
                string Spoiler = "\t- " + Hints.SimplifiedItemNames[ItemData.Reward.Name] + " x" + ItemData.Reward.Amount;
                if (ItemData.Location.LocationId == "1007") {
                    Spoiler += " (10 Fairy Reward)";
                } else if (ItemData.Location.LocationId == "waterfall") {
                    Spoiler += " (20 Fairy Reward)";
                } else if (ItemData.Location.SceneName == "Trinket Well") {
                    Spoiler += " (" + ItemData.Location.LocationId + ")";
                }
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
            List<string> SpoilerLogLines = new List<string>();
            SpoilerLogLines.Add("Seed: " + seed);
            foreach (string Key in SpoilerLog.Keys) {
                SpoilerLogLines.Add(Hints.SceneNamesForSpoilerLog[Key]);
                foreach (string line in SpoilerLog[Key]) {
                    SpoilerLogLines.Add(line);
                }
            }

            if (!File.Exists(SpoilerLogPath)) {
                File.WriteAllLines(SpoilerLogPath, SpoilerLogLines);
            } else {
                File.Delete(SpoilerLogPath);
                File.WriteAllLines(SpoilerLogPath, SpoilerLogLines);
            }
            TunicRandomizer.Logger.LogInfo("Wrote Spoiler Log to " + SpoilerLogPath);
        }

        private static void PopulateHints() {
            ItemData HintItem;
            string HintMessage;

            // Mailbox Hint
            HintItem = FindRandomizedItemByName("Lantern");
            HintMessage = $"lehjehnd sehs #E \"" + Hints.SimplifiedSceneNames[HintItem.Location.SceneName] + "\"\nwil hlp yoo \"light the way...\"";
            Hints.HintMessages.Add("Mailbox", HintMessage);

            // Golden Path hints
            List<string> HintItems = new List<string>() { "Stick", "Sword", "Techbow", "Stundagger", "Wand", "Hyperdash" };
            List<string> HintScenes = new List<string>() { "East Forest Relic", "Fortress Relic", "West Garden Relic", "Temple Statue" };
            for (int i = 0; i < 4; i++) {
                HintItem = FindRandomizedItemByName(HintItems[TunicRandomizer.Randomizer.Next(HintItems.Count)]);
                string HintScene = HintScenes[TunicRandomizer.Randomizer.Next(HintScenes.Count)];

                HintMessage = $"lehjehnd sehs #E \"" + Hints.SimplifiedSceneNames[HintItem.Location.SceneName] + "\"\niz lOkAtid awn \"<#ffd700>Path of the Hero<#ffffff>...\"";
                Hints.HintMessages.Add(HintScene, HintMessage);

                HintItems.Remove(HintItem.Reward.Name);
                HintScenes.Remove(HintScene);
            }

            // Questagon Hints
            List<string> Hexagons = new List<string>() { "Hexagon Red", "Hexagon Green", "Hexagon Blue"};
            Dictionary<string, string> HexagonColors = new Dictionary<string, string>() { { "Hexagon Red", "<#FF3333>" }, { "Hexagon Green", "<#33FF33>" }, { "Hexagon Blue", "<#3333FF>" } };
            List<string> HexagonHintAreas = new List<string>() { "Swamp Relic", "Library Relic", "Monastery Relic" };
            for (int i = 0; i < 3; i++) {
                string Hexagon = Hexagons[TunicRandomizer.Randomizer.Next(Hexagons.Count)];
                HintItem = FindRandomizedItemByName(Hexagon);
                string HexagonHintArea = HexagonHintAreas[TunicRandomizer.Randomizer.Next(HexagonHintAreas.Count)];

                HintMessage = $"#A sA \"" + Hints.SimplifiedSceneNames[HintItem.Location.SceneName] + "\" iz \nwAr #E " + HexagonColors[Hexagon] + "kwehstuhgawn [hexagram]<#FFFFFF> iz fownd\"...\"";
                Hints.HintMessages.Add(HexagonHintArea, HintMessage);

                Hexagons.Remove(Hexagon);
                HexagonHintAreas.Remove(HexagonHintArea);
            }

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
            Dictionary<string, int> HintWeights = new Dictionary<string, int>(){
                {"Fools", 0},
                {"Pages", 0},
                {"Treasures", 0},
                {"Upgrades", 0}
            };
            foreach (ItemData ItemData in RandomItemPatches.ItemList.Values) {
                if (SceneNames.Contains(ItemData.Location.SceneName)) {
                    if (ItemData.Reward.Type == "PAGE") {
                        HintWeights["Pages"] += 1;
                    }
                    if (ItemData.Reward.Name.Contains("GoldenTrophy")) {
                        HintWeights["Treasures"] += 1;
                    }
                    if (ItemData.Reward.Name.Contains("Upgrade Offering")) {
                        HintWeights["Upgrades"] += 1;
                    }
                    if (ItemData.Reward.Type == "FOOL") {
                        HintWeights["Fools"] += 1;
                    }
                }
            }
            List<string> maxWeightedItems = new List<string>();
            int maxWeight = new List<int>(HintWeights.Values).Max();
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
            foreach (ItemData ItemData in RandomItemPatches.ItemList.Values) {
                if (ItemData.Reward.Name == Name) {
                    return ItemData;
                }
            }
            return null;
        }
    }
}
