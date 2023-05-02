using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine;
using TinyJson;
using System.Linq;
using BepInEx.Logging;
using UnityEngine.SceneManagement;
using UnhollowerBaseLib;
using TMPro;
using static TunicRandomizer.GhostHints;

namespace TunicRandomizer {
    public class PlayerCharacterPatches {
        private static ManualLogSource Logger = TunicRandomizer.Logger;

        public static string SaveName = null;
        public static int HeirAssistModeDamageValue = 0;
        public static bool StungByBee = false;
        public static bool IsTeleporting = false;
        public static int index = 0;

        public static bool LoadSecondSword = false;
        public static bool LoadThirdSword = false;
        public static readonly string[] ProgressionNames = {
             "Hyperdash", "Wand", "Techbow", "Stundagger", "Trinket Coin", "Lantern", "Stick", "Sword", "Sword Progression", "Key", "Key (House)", "Mask", "Vault Key (Red)" };

        public static void PlayerCharacter_Update_PostfixPatch(PlayerCharacter __instance) {
            Cheats.FastForward = Input.GetKey(KeyCode.Backslash);

            if (Input.GetKeyDown(KeyCode.Alpha1) && SaveFile.GetString("randomizer game mode") != "HEXAGONQUEST") {
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
                GenericMessage.ShowMessage($"\"Game Settings\"\n\"-----------------\"\n" +
                    $"\"Seed.................{SaveFile.GetInt("seed").ToString().PadLeft(12, '.')}\"\n" +
                    $"\"Game Mode............{SaveFile.GetString("randomizer game mode").PadLeft(12, '.')}\"\n" +
                    $"\"Keys Behind Bosses...{(SaveFile.GetInt("randomizer keys behind bosses") == 0 ? "Off" : "On").PadLeft(12, '.')}\"\n" +
                    $"\"Sword Progression....{(SaveFile.GetInt("randomizer sword progression enabled") == 0 ? "Off" : "On").PadLeft(12, '.')}\"\n" +
                    $"\"Started With Sword...{(SaveFile.GetInt("randomizer started with sword") == 0 ? "No" : "Yes").PadLeft(12, '.')}\"");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                string FurColor = ColorPalette.GetColorStringForPopup(ColorPalette.Fur, 0);
                string PuffColor = ColorPalette.GetColorStringForPopup(ColorPalette.Puff, 1);
                string DetailsColor = ColorPalette.GetColorStringForPopup(ColorPalette.Details, 2);
                string TunicColor = ColorPalette.GetColorStringForPopup(ColorPalette.Tunic, 3);
                string ScarfColor = ColorPalette.GetColorStringForPopup(ColorPalette.Scarf, 4);
                string Palette = $"\"Color Palette\"\n\"-----------------\"\n" +
                    $"\"(0) Fur:     {FurColor.PadLeft(25)}\"\n" +
                    $"\"(1) Puff:    {PuffColor.PadLeft(25)}\"\n" +
                    $"\"(2) Details: {DetailsColor.PadLeft(25)}\"\n" +
                    $"\"(3) Tunic:   {TunicColor.PadLeft(25)}\"\n" +
                    $"\"(4) Scarf:   {ScarfColor.PadLeft(25)}\"";
                GenericMessage.ShowMessage(Palette);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                int ObtainedItemCount = TunicRandomizer.Tracker.ItemsCollected.Count;
                int ObtainedItemCountInCurrentScene = TunicRandomizer.Tracker.ItemsCollected.Where(item => item.Location.SceneName == SceneLoaderPatches.SceneName).ToList().Count;
                int TotalItemCountInCurrentScene = RandomItemPatches.ItemList.Values.Where(item => item.Location.SceneName == SceneLoaderPatches.SceneName).ToList().Count;
                int ObtainedPagesCount = TunicRandomizer.Tracker.ImportantItems["Pages"];
                int ObtainedFairiesCount = TunicRandomizer.Tracker.ImportantItems["Fairies"];
                int ObtainedGoldenTrophiesCount = TunicRandomizer.Tracker.ImportantItems["Golden Trophies"];

                string BossesAndKeys = $"{(StateVariable.GetStateVariableByName("SV_Fortress Arena_Spidertank Is Dead").BoolValue ? "<#FF3333>" : "<#FFFFFF>")}[death]  " +
                    $"{(StateVariable.GetStateVariableByName("Placed Hexagon 1 Red").BoolValue || Inventory.GetItemByName("Hexagon Red").Quantity == 1 ? "<#FF3333>" : "<#FFFFFF>")}[hexagram]  " +
                    $"{(StateVariable.GetStateVariableByName("Librarian Dead Forever").BoolValue ? "<#33FF33>" : "<#FFFFFF>")}[death]  " +
                    $"{(StateVariable.GetStateVariableByName("Placed Hexagon 2 Green").BoolValue || Inventory.GetItemByName("Hexagon Green").Quantity == 1 ? "<#33FF33>" : "<#FFFFFF>")}[hexagram]  " +
                    $"{(StateVariable.GetStateVariableByName("SV_ScavengerBossesDead").BoolValue ? "<#3333FF>" : "<#FFFFFF>")}[death]  " +
                    $"{(StateVariable.GetStateVariableByName("Placed Hexagon 3 Blue").BoolValue || Inventory.GetItemByName("Hexagon Blue").Quantity == 1 ? "<#3333FF>" : "<#FFFFFF>")}[hexagram]";
                GenericMessage.ShowMessage($"\"Collected Items\"\n\"-----------------\"\n\"Pages......{string.Format("{0}/{1}", ObtainedPagesCount, 28).PadLeft(9, '.')}\"\n" +
                    $"\"Fairies....{string.Format("{0}/{1}", ObtainedFairiesCount, 20).PadLeft(9, '.')}\"\n" +
                    $"\"Treasures..{string.Format("{0}/{1}", ObtainedGoldenTrophiesCount, 12).PadLeft(9, '.')}\"\n" +
                    $"\"This Area..{string.Format("{0}/{1}", ObtainedItemCountInCurrentScene, TotalItemCountInCurrentScene).PadLeft(9, '.')}\"\n" +
                    $"\"Overall....{string.Format("{0}/{1}", ObtainedItemCount, RandomItemPatches.ItemList.Count).PadLeft(9, '.')}\"\n" +
                    $"{BossesAndKeys}");
            }
            if (Input.GetKeyDown(KeyCode.Alpha5)) {
                PaletteEditor.RandomizeFoxColors();
            }
            if (Input.GetKeyDown(KeyCode.Alpha6)) {
                PaletteEditor.LoadCustomTexture();
            }

            if (StungByBee) {
                __instance.gameObject.transform.Find("Fox/root/pelvis/chest/head").localScale = new Vector3(3f, 3f, 3f);
            }
            if (LoadSecondSword) {
                try {
                    SwordProgression.EnableSecondSword();
                    LoadSecondSword = false;
                } catch (Exception e) { }
            }
            if (LoadThirdSword) {
                try {
                    SwordProgression.EnableThirdSword();
                    LoadThirdSword = false;
                } catch (Exception e) { }
            }

            if (IsTeleporting) {
                PlayerCharacter.instance.cheapIceParticleSystemEmission.enabled = true;
                PlayerCharacter.instance.damageBoostParticleSystemEmission.enabled = true;
                PlayerCharacter.instance.staminaBoostParticleSystemEmission.enabled = true;
                PlayerCharacter.instance.AddPoison(1f);
            }
        }
        
        public static void PlayerCharacter_Start_PostfixPatch(PlayerCharacter __instance) {

            StateVariable.GetStateVariableByName("SV_ShopTrigger_Fortress").BoolValue = true;
            StateVariable.GetStateVariableByName("SV_ShopTrigger_Sewer").BoolValue = true;
            StateVariable.GetStateVariableByName("SV_ShopTrigger_Swamp(Night)").BoolValue = true;
            StateVariable.GetStateVariableByName("SV_ShopTrigger_WestGarden").BoolValue = true;

            int seed = SaveFile.GetInt("seed");

            if (seed == 0) {
                seed = QuickSettings.CustomSeed == 0 ? new System.Random().Next() : QuickSettings.CustomSeed;
                Logger.LogInfo($"Generated new seed: " + seed);
                SaveFile.SetString("randomizer game mode", Enum.GetName(typeof(RandomizerSettings.GameModes), TunicRandomizer.Settings.GameMode));
                if (TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST) {
                    for (int i = 0; i < 28; i++) {
                        SaveFile.SetInt($"randomizer obtained page {i}", 1);
                    }
                    StateVariable.GetStateVariableByName("Placed Hexagon 1 Red").BoolValue = true;
                    StateVariable.GetStateVariableByName("Placed Hexagon 2 Green").BoolValue = true;
                    StateVariable.GetStateVariableByName("Placed Hexagon 3 Blue").BoolValue = true;
                    StateVariable.GetStateVariableByName("Placed Hexagons ALL").BoolValue = true;
                    StateVariable.GetStateVariableByName("Has Been Betrayed").BoolValue = true;
                    StateVariable.GetStateVariableByName("Has Died To God").BoolValue = true;
                }
                if (TunicRandomizer.Settings.SwordProgressionEnabled) {
                    SaveFile.SetInt("randomizer sword progression enabled", 1);
                    SaveFile.SetInt("randomizer sword progression level", 0);
                }
                if (TunicRandomizer.Settings.KeysBehindBosses) {
                    SaveFile.SetInt("randomizer keys behind bosses", 1);
                }
                if (TunicRandomizer.Settings.StartWithSwordEnabled) {
                    Inventory.GetItemByName("Sword").Quantity = 1;
                    SaveFile.SetInt("randomizer started with sword", 1);
                }

                SaveFile.SetInt("seed", seed);
                SaveFile.SaveToDisk();
            }
            if (TunicRandomizer.Tracker.Seed == 0 || TunicRandomizer.Tracker.Seed != seed) {
                TunicRandomizer.Tracker = new ItemTracker(seed);
                SceneLoaderPatches.UpdateTrackerSceneInfo();
                ModelSwaps.SwappedThisSceneAlready = false;
            }
            Logger.LogInfo("Loading seed: " + seed);
            TunicRandomizer.Randomizer = new System.Random(seed);
            SaveName = SaveFile.saveDestinationName;

            RandomItemPatches.ItemList.Clear();
            RandomItemPatches.ItemsPickedUp.Clear();
            Hints.HintMessages.Clear();

            List<ItemData> InitialItems = JSONParser.FromJson<List<ItemData>>(ItemListJson.ItemList);
            List<Reward> InitialRewards = new List<Reward>();
            List<Location> InitialLocations = new List<Location>();
            List<ItemData> Hexagons = new List<ItemData>();
            List<Reward> ProgressionRewards = new List<Reward>();
            Dictionary<string, int> PlacedInventory = new Dictionary<string, int>{};
            Dictionary<string, ItemData> ProgressionLocations = new Dictionary<string, ItemData>{};

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
                    }
                    if (ProgressionNames.Contains(Item.Reward.Name) || RandomItemPatches.FairyLookup.Keys.Contains(Item.Reward.Name)) {
                        ProgressionRewards.Add(Item.Reward);
                    } else {
                       InitialRewards.Add(Item.Reward);
                    }
                    InitialLocations.Add(Item.Location);
                }
            }

            // Randomize the rewards and locations
            // put progression items in locations
            foreach (Reward item in ProgressionRewards.OrderBy(r => TunicRandomizer.Randomizer.Next())) {
                // pick a location 
                int l;
                l = TunicRandomizer.Randomizer.Next(InitialLocations.Count);
            
                // if location isn't reachable with placed inv, pick a new location
                while(!InitialLocations[l].reachable(PlacedInventory)) {
                    l = TunicRandomizer.Randomizer.Next(InitialLocations.Count);
                }

                // add item to placed inv for future reachability checks
                string itemName = RandomItemPatches.FairyLookup.Keys.Contains(item.Name) ? "Fairy" : item.Name;
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
                RandomItemPatches.ItemList.Add(DictionaryId, ItemData);
            }

            // add progression items and locations back
            foreach (string key in ProgressionLocations.Keys) {
                RandomItemPatches.ItemList.Add(key, ProgressionLocations[key]);
            }

            if (SaveFile.GetInt("randomizer keys behind bosses") != 0) {
                foreach (ItemData Hexagon in Hexagons) {
                    // second quest?
                    if (SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST") {
                        Hexagon.Reward.Name = "Hexagon Gold";
                        Hexagon.Reward.Type = "SPECIAL";
                    }
                    string DictionaryId = $"{Hexagon.Location.LocationId} [{Hexagon.Location.SceneName}]";
                    RandomItemPatches.ItemList.Add(DictionaryId, Hexagon);
                }
            }
            foreach (string Key in RandomItemPatches.ItemList.Keys) {
                int ItemPickedUp = SaveFile.GetInt($"randomizer picked up {Key}");
                RandomItemPatches.ItemsPickedUp.Add(Key, ItemPickedUp == 1 ? true : false);
            }
            if (TunicRandomizer.Tracker.ItemsCollected.Count == 0) {
                foreach (KeyValuePair<string, bool> PickedUpItem in RandomItemPatches.ItemsPickedUp.Where(item => item.Value)) {
                    RandomItemPatches.UpdateItemTracker(PickedUpItem.Key);
                }
                ItemTracker.SaveTrackerFile();
                TunicRandomizer.Tracker.ImportantItems["Flask Container"] += TunicRandomizer.Tracker.ItemsCollected.Where(Item => Item.Reward.Name == "Flask Shard").Count() / 3;
                if (SaveFile.GetInt("randomizer started with sword") == 1) {
                    TunicRandomizer.Tracker.ImportantItems["Sword"] += 1;
                }
            }

            TunicRandomizer.Tracker.ImportantItems["Coins Tossed"] = StateVariable.GetStateVariableByName("Trinket Coins Tossed").IntValue;
            HeirAssistModeDamageValue = RandomItemPatches.ItemsPickedUp.Values.ToList().Where(item => item == true).ToList().Count / 15;
            Inventory.GetItemByName("Homeward Bone Statue").icon = Inventory.GetItemByName("Dash Stone").icon;
            Inventory.GetItemByName("Spear").icon = Inventory.GetItemByName("MoneyBig").icon;
            Inventory.GetItemByName("MoneyLevelItem").Quantity = 1;
            if (SaveFile.GetInt("randomizer sword progression enabled") != 0) {
                int SwordLevel = SaveFile.GetInt("randomizer sword progression level");
                TunicRandomizer.Tracker.ImportantItems["Sword Progression"] = SwordLevel;
                if (SwordLevel >= 1) {
                    TunicRandomizer.Tracker.ImportantItems["Stick"] = 1;
                }
                if (SwordLevel >= 2) {
                    TunicRandomizer.Tracker.ImportantItems["Sword"] = 1;
                }
                if (SwordLevel == 3) {
                    LoadSecondSword = true;
                }
                if (SwordLevel == 4) {
                    LoadThirdSword = true;
                }
            }
            if (SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST") {
                TunicRandomizer.Tracker.ImportantItems["Pages"] = 28;
                TunicRandomizer.Tracker.ImportantItems["Hexagon Gold"] = SaveFile.GetInt("randomizer inventory quantity Hexagon Gold");
                SaveFile.SetInt("last page viewed", 0);
                ModelSwaps.SetupHexagonQuest();
            } else {
                ModelSwaps.RestoreOriginalHexagons();
            }
            ModelSwaps.SetupDathStoneItemPresentation();
            SetupGoldenTrophyCollectionLines();
            PopulateSpoilerLog();
            Logger.LogInfo("Wrote Spoiler Log to " + TunicRandomizer.SpoilerLogPath);
            PopulateHints();

            if (!ModelSwaps.SwappedThisSceneAlready) {
                ModelSwaps.SwapItemsInScene();
            }

            if (TunicRandomizer.Settings.RandomFoxColorsEnabled) {
                PaletteEditor.RandomizeFoxColors();
            }
            if (TunicRandomizer.Settings.UseCustomTexture) {
                PaletteEditor.LoadCustomTexture();
            }
            if (TunicRandomizer.Settings.RealestAlwaysOn) {
                GameObject.FindObjectOfType<RealestSpell>().SpellEffect();
            }
            FairyTargets.CreateFairyTargets();
            GhostHints.GenerateHints();
            OptionsGUIPatches.SaveSettings();
            if (TunicRandomizer.Settings.GhostFoxHintsEnabled && !SceneLoaderPatches.SpawnedGhosts) {
                GhostHints.SpawnHintGhosts(SceneLoaderPatches.SceneName);
            }
        }
        
        public static bool Monster_IDamageable_ReceiveDamage_PrefixPatch(Monster __instance) {

            if (__instance.name == "Foxgod" && SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST") {
                return false;
            }
            if (__instance.name == "_Fox(Clone)") {
                if (GoldenItemBehavior.CanTakeGoldenHit) {
                    GameObject.Find("_Fox(Clone)/fox").GetComponent<CreatureMaterialManager>().originalMaterials = GoldenItemBehavior.FoxBody.GetComponent<MeshRenderer>().materials;
                    GameObject.Find("_Fox(Clone)/fox hair").GetComponent<CreatureMaterialManager>().originalMaterials = GoldenItemBehavior.FoxHair.GetComponent<MeshRenderer>().materials;
                    SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
                    GoldenItemBehavior.CanTakeGoldenHit = false;
                    return false;
                }
            } else {
                if (__instance.name == "Foxgod" && TunicRandomizer.Settings.HeirAssistModeEnabled) {
                    __instance.hp -= HeirAssistModeDamageValue;
                }
                if (GoldenItemBehavior.CanSwingGoldenSword) {
                    __instance.hp -= 30;
                    if (SaveFile.GetInt("randomizer sword progression level") == 3) {
                        GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy").transform.GetChild(4).GetComponent<MeshRenderer>().materials = ModelSwaps.SecondSword.GetComponent<MeshRenderer>().materials;
                    } else if (SaveFile.GetInt("randomizer sword progression level") == 4) {
                        GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy").transform.GetChild(4).GetComponent<MeshRenderer>().materials = ModelSwaps.ThirdSword.GetComponent<MeshRenderer>().materials;
                    } else {
                        GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy").GetComponent<MeshRenderer>().materials = GoldenItemBehavior.Sword.GetComponent<MeshRenderer>().materials;
                    }
                    SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
                    GoldenItemBehavior.CanSwingGoldenSword = false;
                }
            }
            return true;
        }

        public static bool BoneItemBehavior_onActionButtonDown_PrefixPatch(BoneItemBehaviour __instance) {
            __instance.confirmationPromptLine.text = $"wAk fruhm #is drEm \n ahnd rEturn too \"{Hints.SimplifiedSceneNames[SaveFile.GetString("last campfire scene name")]}\"?";
            if (GameObject.FindObjectsOfType<Monster>().Where(Monster => Monster.IsAggroed).Count() > 0) {
                return false;
            }
            
            return true;
        }

        public static bool BoneItemBehavior_confirmBoneUseCallback_PrefixPatch(BoneItemBehaviour __instance) {
            IsTeleporting = true;
            return true;
        }

        public static void ResetSword() {
            string SwordPath = "_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy/";
            GameObject SwordProxy = GameObject.Find(SwordPath);
            if (SwordProxy.GetComponent<MeshFilter>() == null) {
                GameObject.Destroy(SwordProxy.GetComponent<MeshFilter>());
            }
            if (SwordProxy.GetComponent<MeshRenderer>() == null) {
                GameObject.Destroy(SwordProxy.GetComponent<MeshRenderer>());
            }
            SwordProxy.AddComponent<MeshFilter>().mesh = ModelSwaps.Items["Sword"].GetComponent<MeshFilter>().mesh;
            SwordProxy.AddComponent<MeshRenderer>().materials = ModelSwaps.Items["Sword"].GetComponent<MeshRenderer>().materials;
            SwordProxy.transform.GetChild(0).localPosition = new Vector3(0f, 0.7653f, 0f);
            SwordProxy.transform.GetChild(1).GetComponent<BoxCollider>().size = new Vector3(0.33f, 1.25f, 1f);
            SwordProxy.transform.GetChild(2).GetComponent<BoxCollider>().size = new Vector3(0.2f, 1.32f, 0.2f);
            if (SwordProxy.transform.childCount == 5) {
                GameObject.Destroy(SwordProxy.transform.GetChild(4).gameObject);
            }
        }

        public static bool InteractionTrigger_Interact_PrefixPatch(Item item, InteractionTrigger __instance) {
            string InteractionLocation = SceneLoaderPatches.SceneName + " " + __instance.transform.position;
            if (Hints.HintLocations.ContainsKey(InteractionLocation) && TunicRandomizer.Settings.HeroPathHintsEnabled) {
                LanguageLine Hint = ScriptableObject.CreateInstance<LanguageLine>();
                Hint.text = Hints.HintMessages[Hints.HintLocations[InteractionLocation]];
                GenericMessage.ShowMessage(Hint);
                return false;
            }
            if (SceneLoaderPatches.SceneName == "Waterfall" && __instance.transform.position.ToString() == "(-47.4, 46.9, 3.0)" && TunicRandomizer.Tracker.ImportantItems["Fairies"] < 10) {
                GenericMessage.ShowMessage($"\"Locked. (10\" fArEz \"required)\"");
                return false;
            }
            if (SceneLoaderPatches.SceneName == "Waterfall" && __instance.transform.position.ToString() == "(-47.5, 45.0, -0.5)" && TunicRandomizer.Tracker.ImportantItems["Fairies"] < 20) {
                GenericMessage.ShowMessage($"\"Locked. (20\" fArEz \"required)\"");
                return false;
            }
            if (SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST") {
                if(__instance.transform.position.ToString() == "(0.0, 0.0, 0.0)" && SceneLoaderPatches.SceneName == "Spirit Arena" && TunicRandomizer.Tracker.ImportantItems["Hexagon Gold"] < 20) {
                    GenericMessage.ShowMessage($"\"<#EAA615>Sealed Forever.\"");
                    return false;
                }
                if (__instance.transform.position.ToString() == "(2.0, 46.0, 0.0)" && SceneLoaderPatches.SceneName == "Overworld Redux" && !(StateVariable.GetStateVariableByName("Rung Bell 1 (East)").BoolValue && StateVariable.GetStateVariableByName("Rung Bell 2 (West)").BoolValue)) {
                    GenericMessage.ShowMessage($"\"Sealed Forever.\"");
                    return false;
                }
            }

            return true;
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

        public static void PopulateSpoilerLog() {
            int seed = SaveFile.GetInt("seed");
            Dictionary<string, List<string>> SpoilerLog = new Dictionary<string, List<string>>();
            foreach (string Key in Hints.SceneNamesForSpoilerLog.Keys) {
                SpoilerLog[Key] = new List<string>();
            }
            Dictionary<string, string> Descriptions = JSONParser.FromJson<Dictionary<string, string>>(Hints.LocationDescriptionsJson);
            foreach (string Key in RandomItemPatches.ItemList.Keys) {
                ItemData ItemData = RandomItemPatches.ItemList[Key];
                string Spoiler = $"\t{(RandomItemPatches.ItemsPickedUp[Key] ? "x" : "-")} {Descriptions[Key]}: {Hints.SimplifiedItemNames[ItemData.Reward.Name]} x{ItemData.Reward.Amount}";

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
            SpoilerLogLines.Add("Lines that start with 'x' instead of '-' represent items that have been collected");
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

        private static void PopulateHints() {
            ItemData HintItem;
            string HintMessage;
            List<char> Vowels = new List<char>() { 'A', 'E', 'I', 'O', 'U' };
            // Mailbox Hint
            HintItem = FindRandomizedItemByName("Lantern");
            string Scene = Hints.SimplifiedSceneNames[HintItem.Location.SceneName];
            string ScenePrefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
            HintMessage = $"lehjehnd sehz {ScenePrefix} \"{Scene.ToUpper()}\"\nwil hehlp yoo \"<#00FFFF>LIGHT THE WAY<#ffffff>...\"";
            Hints.HintMessages.Add("Mailbox", HintMessage);

            // Golden Path hints
            HintItem = FindRandomizedItemByName("Hyperdash");
            Scene = Hints.SimplifiedSceneNames[HintItem.Location.SceneName];
            ScenePrefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
            HintMessage = $"lehjehnd sehz <#FF00FF>suhm%i^ ehkstruhordinArE\n<#FFFFFF>uhwAts yoo aht {ScenePrefix} \"{Scene.ToUpper()}...\"";
            Hints.HintMessages.Add("Temple Statue", HintMessage);
            
            List<string> HintItems = new List<string>() { "Techbow", "Stundagger", "Wand" };
            List<string> HintScenes = new List<string>() { "East Forest Relic", "Fortress Relic", "West Garden Relic" };
            for (int i = 0; i < 3; i++) {
                HintItem = FindRandomizedItemByName(HintItems[TunicRandomizer.Randomizer.Next(HintItems.Count)]);
                string HintScene = HintScenes[TunicRandomizer.Randomizer.Next(HintScenes.Count)];

                Scene = Hints.SimplifiedSceneNames[HintItem.Location.SceneName];
                ScenePrefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                HintMessage = $"lehjehnd sehz {ScenePrefix} \"{Scene.ToUpper()}\"\niz lOkAtid awn #uh \"<#ffd700>PATH OF THE HERO<#ffffff>...\"";
                Hints.HintMessages.Add(HintScene, HintMessage);

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
                Hints.HintMessages.Add(HexagonHintArea, HintMessage);

                Hexagons.Remove(Hexagon);
                HexagonHintAreas.Remove(HexagonHintArea);
            }

        }


        public static void SetupGoldenTrophyCollectionLines() {
            //kawngrahJoulA$uhnz!
            if (TunicRandomizer.Settings.BonusStatUpgradesEnabled) {
                foreach (string StaminaTrophy in RandomItemPatches.GoldenTrophyStatUpgrades["Level Up - Stamina"]) {
                    Inventory.GetItemByName(StaminaTrophy).collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    Inventory.GetItemByName(StaminaTrophy).collectionMessage.text = $"kawngrahJoulA$uhnz! \"(<#8ddc6e>+1 SP<#FFFFFF>)\"";

                }
                foreach (string MagicTrophy in RandomItemPatches.GoldenTrophyStatUpgrades["Level Up - Magic"]) {
                    Inventory.GetItemByName(MagicTrophy).collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    Inventory.GetItemByName(MagicTrophy).collectionMessage.text = $"kawngrahJoulA$uhnz! \"(<#2a8fed>+1 MP<#FFFFFF>)\"";
                }
                foreach (string DefenseTrophy in RandomItemPatches.GoldenTrophyStatUpgrades["Level Up - DamageResist"]) {
                    Inventory.GetItemByName(DefenseTrophy).collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    Inventory.GetItemByName(DefenseTrophy).collectionMessage.text = $"kawngrahJoulA$uhnz! \"(<#5de7cf>+1 DEF<#FFFFFF>)\"";
                }
                foreach (string PotionTrophy in RandomItemPatches.GoldenTrophyStatUpgrades["Level Up - PotionEfficiency"]) {
                    Inventory.GetItemByName(PotionTrophy).collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    Inventory.GetItemByName(PotionTrophy).collectionMessage.text = $"kawngrahJoulA$uhnz! \"(<#ca7be4>+1 POTION<#FFFFFF>)\"";
                }
            } else {
                for (int i = 1; i < 13; i++) {
                    Inventory.GetItemByName($"GoldenTrophy_{i}").collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    Inventory.GetItemByName($"GoldenTrophy_{i}").collectionMessage.text = $"kawngrahJoulA$uhnz!";
                }
            }
        }

        public static ItemData FindRandomizedItemByName(string Name) {
            foreach (ItemData ItemData in RandomItemPatches.ItemList.Values) {
                if (ItemData.Reward.Name == Name) {
                    return ItemData;
                }
            }
            return null;
        }

        public static void SaveFile_GetNewSaveFileName_PostfixPatch(SaveFile __instance, ref string __result) {

            __result = $"{__result.Split('.')[0]}-randomizer.tunic";
        }

        public static void FileManagementGUI_rePopulateList_PostfixPatch(FileManagementGUI __instance) {
            foreach (FileManagementGUIButton button in GameObject.FindObjectsOfType<FileManagementGUIButton>()) {
                SaveFile.LoadFromPath(SaveFile.GetRootSaveFileNameList()[button.index]);
                if (SaveFile.GetInt("seed") != 0 && !button.isSpecial) {
                    // Display special icon and "randomized" text to indicate randomizer file
                    button.specialBadge.gameObject.active = true;
                    button.specialBadge.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    button.specialBadge.transform.localPosition = new Vector3(-75f, -27f, 0f);
                    button.playtimeString.enableAutoSizing = false;
                    button.playtimeString.text += $" <size=70%>randomized";
                    // Display randomized page count instead of "vanilla" pages picked up
                    int Pages = 0;
                    for (int i = 0; i < 28; i++) {
                        if (SaveFile.GetInt($"randomizer obtained page {i}") == 1) {
                            Pages++;
                        }
                    }
                    button.manpageTMP.text = Pages.ToString();
                }
            }
        }

        public static bool BloodstainChest_IInteractionReceiver_Interact_PrefixPatch(Item i, BloodstainChest __instance) {
            if (SceneLoaderPatches.SceneName == "Changing Room") {
                CoinSpawner.SpawnCoins(20, __instance.transform.position);
                __instance.doPushbackBlast();
                return false;
            }
            return true;
        }
    }
}
