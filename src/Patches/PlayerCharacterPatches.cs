using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TinyJson;
using System.Linq;
using BepInEx.Logging;
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
        public static Dictionary<string, int> SphereZero = new Dictionary<string, int>();
        public static float TimeWhenLastChangedDayNight = 0.0f;

        public static void PlayerCharacter_Update_PostfixPatch(PlayerCharacter __instance) {
            Cheats.FastForward = Input.GetKey(KeyCode.Backslash);
/*
            if (Input.GetKeyDown(KeyCode.Alpha1) && SaveFile.GetString("randomizer game mode") != "HEXAGONQUEST" && (TimeWhenLastChangedDayNight + 3.0f < Time.fixedTime)) {
                TimeWhenLastChangedDayNight = Time.fixedTime;
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
            }*/
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                GenericMessage.ShowMessage($"\"Game Settings\"\n\"-----------------\"\n" +
                    $"\"Seed.................{SaveFile.GetInt("seed").ToString().PadLeft(12, '.')}\"\n" +
                    $"\"Game Mode............{SaveFile.GetString("randomizer game mode").PadLeft(12, '.')}\"\n" +
                    $"\"Keys Behind Bosses...{(SaveFile.GetInt("randomizer keys behind bosses") == 0 ? "Off" : "On").PadLeft(12, '.')}\"\n" +
                    $"\"Sword Progression....{(SaveFile.GetInt("randomizer sword progression enabled") == 0 ? "Off" : "On").PadLeft(12, '.')}\"\n" +
                    $"\"Started With Sword...{(SaveFile.GetInt("randomizer started with sword") == 0 ? "No" : "Yes").PadLeft(12, '.')}\"\n" +
                    $"\"Shuffled Abilities...{(SaveFile.GetInt("randomizer shuffled abilities") == 0 ? "No" : "Yes").PadLeft(12, '.')}\"");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                GUIUtility.systemCopyBuffer = SaveFile.GetInt("seed").ToString();
            }
/*            if (Input.GetKeyDown(KeyCode.Alpha3)) {
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
                int TotalItemCountInCurrentScene = ItemRandomizer.ItemList.Values.Where(item => item.Location.SceneName == SceneLoaderPatches.SceneName).ToList().Count;
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
                    $"\"Overall....{string.Format("{0}/{1}", ObtainedItemCount, ItemRandomizer.ItemList.Count).PadLeft(9, '.')}\"\n" +
                    $"{BossesAndKeys}");
            }*/
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
                PlayerCharacter.instance._CompletelyInvulnerableEvenToIFrameIgnoringAttacks_k__BackingField = true;
                PlayerCharacter.instance.AddPoison(1f);
            }
            if (SaveFile.GetInt("randomizer shuffled abilities") == 1 && SaveFile.GetInt($"randomizer obtained page 12") == 0) {
                __instance.prayerBeginTimer = 0;
            }
            if (SaveFile.GetInt("randomizer shuffled abilities") == 1 && SaveFile.GetInt($"randomizer obtained page 26") == 0) {
                TechbowItemBehaviour.kIceShotWindow = 0;
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
                if (TunicRandomizer.Settings.ShuffleAbilities && TunicRandomizer.Settings.GameMode != RandomizerSettings.GameModes.HEXAGONQUEST) {
                    SaveFile.SetInt("randomizer shuffled abilities", 1);
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

            ItemRandomizer.ItemList.Clear();
            ItemRandomizer.ItemsPickedUp.Clear();
            Hints.HintMessages.Clear();

            if (SphereZero.Count == 0) {
                PopulateSphereZero();
            }
            RandomizeAndPlaceItems();

            TunicRandomizer.Tracker.ImportantItems["Coins Tossed"] = StateVariable.GetStateVariableByName("Trinket Coins Tossed").IntValue;
            HeirAssistModeDamageValue = ItemRandomizer.ItemsPickedUp.Values.ToList().Where(item => item == true).ToList().Count / 15;
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
                    TunicRandomizer.Tracker.ImportantItems["Sword"] = SwordLevel;
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
            if (SaveFile.GetInt("randomizer shuffled abilities") == 1 && SaveFile.GetInt("randomizer obtained page 21") == 0) {
                foreach (ToggleObjectBySpell SpellToggle in Resources.FindObjectsOfTypeAll<ToggleObjectBySpell>()) {
                    SpellToggle.gameObject.GetComponent<ToggleObjectBySpell>().enabled = false;
                }
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
                    GameObject.Find("_Fox(Clone)/fox").GetComponent<CreatureMaterialManager>()._ghostMaterialArray = GoldenItemBehavior.GhostFoxBody.GetComponent<MeshRenderer>().materials;
                    GameObject.Find("_Fox(Clone)/fox hair").GetComponent<CreatureMaterialManager>()._ghostMaterialArray = GoldenItemBehavior.GhostFoxHair.GetComponent<MeshRenderer>().materials;
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
            if (SceneLoaderPatches.SceneName == "Overworld Interiors" && __instance.transform.position.ToString() == "(-25.7, 28.4, -54.4)") { 
                if ((StateVariable.GetStateVariableByName("Has Been Betrayed").BoolValue || StateVariable.GetStateVariableByName("Has Died To God").BoolValue) && (TimeWhenLastChangedDayNight + 3.0f < Time.fixedTime)) {
                    GenericPrompt.ShowPrompt(CycleController.IsNight ? $"wAk fruhm #is drEm?" : $"rEtirn too yor drEm?", (Il2CppSystem.Action)ChangeDayNightHourglass, null);
                }
                return false;
            }
            if (SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST") {
                if (__instance.transform.position.ToString() == "(0.0, 0.0, 0.0)" && SceneLoaderPatches.SceneName == "Spirit Arena" && TunicRandomizer.Tracker.ImportantItems["Hexagon Gold"] < 20) {
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

        private static void ChangeDayNightHourglass() {
            TimeWhenLastChangedDayNight = Time.fixedTime;
            bool isNight = CycleController.IsNight;
            if (isNight) {
                CycleController.AnimateSunrise();
            } else {
                CycleController.AnimateSunset();
            }
            CycleController.IsNight = !isNight;
            CycleController.nightStateVar.BoolValue = !isNight;
            GameObject.Find("day night hourglass/rotation/hourglass").GetComponent<MeshRenderer>().materials[0].color = CycleController.IsNight ? new Color(1f, 0f, 1f, 1f) : new Color(1f, 1f, 0f, 1f);
        }

        private static void PopulateSphereZero() {
            if (SaveFile.GetInt("randomizer shuffled abilities") == 0) {
                SphereZero.Add("12", 1);
                SphereZero.Add("21", 1);
                SphereZero.Add("26", 1);
            }
            if (SaveFile.GetInt("randomizer started with sword") == 1) {
                SphereZero.Add("Sword", 1);
            }
        }

        private static void RandomizeAndPlaceItems() {
            List<string> ProgressionNames = new List<string>{
                "Hyperdash", "Wand", "Techbow", "Stundagger", "Trinket Coin", "Lantern", "Stick", "Sword", "Sword Progression", "Key", "Key (House)", "Mask", "Vault Key (Red)" };
            if (SaveFile.GetInt("randomizer shuffled abilities") == 1) {
                ProgressionNames.Add("12"); // Prayer
                ProgressionNames.Add("21"); // Holy Cross
                ProgressionNames.Add("26"); // Ice Rod
            }

            List<ItemData> InitialItems = JSONParser.FromJson<List<ItemData>>(ItemListJson.ItemList);
            List<Reward> InitialRewards = new List<Reward>();
            List<Location> InitialLocations = new List<Location>();
            List<ItemData> Hexagons = new List<ItemData>();
            List<Reward> ProgressionRewards = new List<Reward>();
            Dictionary<string, int> PlacedInventory = new Dictionary<string, int>(SphereZero);
            Dictionary<string, ItemData> ProgressionLocations = new Dictionary<string, ItemData> { };

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
                    if (ProgressionNames.Contains(Item.Reward.Name) || ItemRandomizer.FairyLookup.Keys.Contains(Item.Reward.Name)) {
                        ProgressionRewards.Add(Item.Reward);
                    } else {
                        InitialRewards.Add(Item.Reward);
                    }
                    InitialLocations.Add(Item.Location);
                }
            }

            // put progression items in locations
            foreach (Reward item in ProgressionRewards.OrderBy(r => TunicRandomizer.Randomizer.Next())) {
                // pick a location 
                int l;
                l = TunicRandomizer.Randomizer.Next(InitialLocations.Count);

                // if location isn't reachable with placed inv, pick a new location
                while (!InitialLocations[l].reachable(PlacedInventory)) {
                    l = TunicRandomizer.Randomizer.Next(InitialLocations.Count);
                }

                // add item to placed inv for future reachability checks
                string itemName = ItemRandomizer.FairyLookup.Keys.Contains(item.Name) ? "Fairy" : item.Name;
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
                ItemRandomizer.ItemList.Add(DictionaryId, ItemData);
            }

            // add progression items and locations back
            foreach (string key in ProgressionLocations.Keys) {
                ItemRandomizer.ItemList.Add(key, ProgressionLocations[key]);
            }

            if (SaveFile.GetInt("randomizer keys behind bosses") != 0) {
                foreach (ItemData Hexagon in Hexagons) {
                    if (SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST") {
                        Hexagon.Reward.Name = "Hexagon Gold";
                        Hexagon.Reward.Type = "SPECIAL";
                    }
                    string DictionaryId = $"{Hexagon.Location.LocationId} [{Hexagon.Location.SceneName}]";
                    ItemRandomizer.ItemList.Add(DictionaryId, Hexagon);
                }
            }
            foreach (string Key in ItemRandomizer.ItemList.Keys) {
                int ItemPickedUp = SaveFile.GetInt($"randomizer picked up {Key}");
                ItemRandomizer.ItemsPickedUp.Add(Key, ItemPickedUp == 1 ? true : false);
            }
            if (TunicRandomizer.Tracker.ItemsCollected.Count == 0) {
                foreach (KeyValuePair<string, bool> PickedUpItem in ItemRandomizer.ItemsPickedUp.Where(item => item.Value)) {
                    ItemRandomizer.UpdateItemTracker(PickedUpItem.Key);
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

        public static void PopulateSpoilerLog() {
            int seed = SaveFile.GetInt("seed");
            Dictionary<string, List<string>> SpoilerLog = new Dictionary<string, List<string>>();
            foreach (string Key in Hints.SceneNamesForSpoilerLog.Keys) {
                SpoilerLog[Key] = new List<string>();
            }
            Dictionary<string, string> Descriptions = JSONParser.FromJson<Dictionary<string, string>>(Hints.LocationDescriptionsJson);
            foreach (string Key in ItemRandomizer.ItemList.Keys) {
                ItemData ItemData = ItemRandomizer.ItemList[Key];
                string Spoiler = $"\t{(ItemRandomizer.ItemsPickedUp[Key] ? "x" : "-")} {Descriptions[Key]}: {Hints.SimplifiedItemNames[ItemData.Reward.Name]} x{ItemData.Reward.Amount}";

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
                "Key (House)", "Dath Stone", "Relic - Hero Sword", "Hexagon Red", "Hexagon Green", "Hexagon Blue", "12", "21", "26"
            };
            foreach (string MajorItem in MajorItems) { 
                foreach(ItemData Item in FindAllRandomizedItemsByName(MajorItem)) {
                    string Key = $"{Item.Location.LocationId} [{Item.Location.SceneName}]";
                    string Spoiler = $"\t{(ItemRandomizer.ItemsPickedUp[Key] ? "x" : "-")} {Hints.SimplifiedItemNames[Item.Reward.Name]}: {Hints.SceneNamesForSpoilerLog[Item.Location.SceneName]} - {Descriptions[Key]}";
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

        private static void PopulateHints() {
            ItemData HintItem = null;
            string HintMessage;
            List<char> Vowels = new List<char>() { 'A', 'E', 'I', 'O', 'U' };
            bool techbowHinted = false;
            bool wandHinted = false;
            bool prayerHinted = false;
            bool hcHinted = false;
            string Scene;
            string ScenePrefix;

            // Mailbox Hint
            List<string> mailboxNames = new List<string>() { "Wand", "Lantern", "Gun", "Techbow", SaveFile.GetInt("randomizer sword progression enabled") != 0 ? "Sword Progression" : "Sword" };
            if (SaveFile.GetInt("randomizer shuffled abilities") == 1) {
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
            } else {
                Scene = Hints.SimplifiedSceneNames[HintItem.Location.SceneName];
                ScenePrefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                HintMessage = $"lehjehnd sehz {ScenePrefix} \"{Scene.ToUpper()}\"\nkuhntAnz wuhn uhv mehnE \"<#00FFFF>First Steps<#ffffff>\" ahn yor jurnE.";
                if (HintItem.Reward.Name == "Techbow") { techbowHinted = true; }
                if (HintItem.Reward.Name == "Wand") { wandHinted = true; }
                if (HintItem.Reward.Name == "12") { prayerHinted = true; }
                if (HintItem.Reward.Name == "21") { hcHinted = true; }
            }
            Hints.HintMessages.Add("Mailbox", HintMessage);

            // Golden Path hints
            HintItem = FindRandomizedItemByName("Hyperdash");
            Scene = Hints.SimplifiedSceneNames[HintItem.Location.SceneName];
            ScenePrefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
            HintMessage = $"lehjehnd sehz <#FF00FF>suhm%i^ ehkstruhordinArE\n<#FFFFFF>uhwAts yoo aht {ScenePrefix} \"{Scene.ToUpper()}...\"";
            Hints.HintMessages.Add("Temple Statue", HintMessage);

            List<string> HintItems = new List<string>() { techbowHinted ? "Lantern" : "Techbow", wandHinted ? "Lantern" : "Wand", "Stundagger" };
            if (SaveFile.GetInt("randomizer shuffled abilities") == 1) {
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
                foreach (string StaminaTrophy in ItemRandomizer.GoldenTrophyStatUpgrades["Level Up - Stamina"]) {
                    Inventory.GetItemByName(StaminaTrophy).collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    Inventory.GetItemByName(StaminaTrophy).collectionMessage.text = $"kawngrahJoulA$uhnz! \"(<#8ddc6e>+1 SP<#FFFFFF>)\"";

                }
                foreach (string MagicTrophy in ItemRandomizer.GoldenTrophyStatUpgrades["Level Up - Magic"]) {
                    Inventory.GetItemByName(MagicTrophy).collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    Inventory.GetItemByName(MagicTrophy).collectionMessage.text = $"kawngrahJoulA$uhnz! \"(<#2a8fed>+1 MP<#FFFFFF>)\"";
                }
                foreach (string DefenseTrophy in ItemRandomizer.GoldenTrophyStatUpgrades["Level Up - DamageResist"]) {
                    Inventory.GetItemByName(DefenseTrophy).collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    Inventory.GetItemByName(DefenseTrophy).collectionMessage.text = $"kawngrahJoulA$uhnz! \"(<#5de7cf>+1 DEF<#FFFFFF>)\"";
                }
                foreach (string PotionTrophy in ItemRandomizer.GoldenTrophyStatUpgrades["Level Up - PotionEfficiency"]) {
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
            foreach (ItemData ItemData in ItemRandomizer.ItemList.Values) {
                if (ItemData.Reward.Name == Name) {
                    return ItemData;
                }
            }
            return null;
        }

        public static List<ItemData> FindAllRandomizedItemsByName(string Name) {
            List<ItemData> results = new List<ItemData>();

            foreach (ItemData ItemData in ItemRandomizer.ItemList.Values) {
                if (ItemData.Reward.Name == Name) {
                    results.Add(ItemData);
                }
            }

            return results;
        }

        public static List<ItemData> FindAllRandomizedItemsByType(string type) {
            List<ItemData> results = new List<ItemData>();

            foreach (ItemData itemData in ItemRandomizer.ItemList.Values) {
                if (itemData.Reward.Type == type) {
                    results.Add(itemData);
                }
            }

            return results;
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

        public static void UpgradeAltar_DoOfferingSequence_PostfixPatch(UpgradeAltar __instance) {
            foreach (string LevelUp in ItemRandomizer.LevelUpItemNames) {
                TunicRandomizer.Tracker.ImportantItems[LevelUp] = Inventory.GetItemByName(LevelUp).Quantity;
            }
        }

/*        public static void CrossbowItemBehavior___fireBow_PostfixPatch(CrossbowItemBehaviour __instance) {

        }

        public static bool CrossbowItemBehavior_onActionButtonDown_PrefixPatch(CrossbowItemBehaviour __instance) {
            
            return true;
        }*/

    }
}
