using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TinyJson;
using System.Linq;
using BepInEx.Logging;
using static TunicRandomizer.GhostHints;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnhollowerBaseLib;
using static TunicRandomizer.ItemPatches;

namespace TunicRandomizer {
    public class PlayerCharacterPatches {
        private static ManualLogSource Logger = TunicRandomizer.Logger;

        public static string SaveName = null;
        public static int HeirAssistModeDamageValue = 0;
        public static bool StungByBee = false;
        public static int index = 0;

        public static bool LoadSecondSword = false;
        public static bool LoadThirdSword = false;
        public static float LoadSwordTimer = 0.0f;
        public static bool LoadCustomTexture = false;
        public static bool WearHat = false;
        public static float TimeWhenLastChangedDayNight = 0.0f;
        public static float FinishLineSwordTimer = 0.0f;
        public static float CompletionTimer = 0.0f;
        public static float ResetDayNightTimer = -1.0f;
        public static string MailboxHintId = "";

        public static void PlayerCharacter_Update_PostfixPatch(PlayerCharacter __instance) {

            Cheats.FastForward = Input.GetKey(KeyCode.Backslash);
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                if (GameObject.Find("_FinishlineDisplay(Clone)/").transform.childCount >= 3) {
                    GameObject.Find("_FinishlineDisplay(Clone)/").transform.GetChild(2).gameObject.SetActive(!GameObject.Find("_FinishlineDisplay(Clone)/").transform.GetChild(2).gameObject.active);
                }
            }
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
                GenericPrompt.ShowPrompt($"\"Copy Current Game Settings?\"\n\"-----------------\"\n" +
                    $"\"Seed.................{SaveFile.GetInt("seed").ToString().PadLeft(12, '.')}\"\n" +
                    $"\"Game Mode............{SaveFile.GetString("randomizer game mode").PadLeft(12, '.')}\"\n" +
                    $"\"Keys Behind Bosses...{(SaveFile.GetInt("randomizer keys behind bosses") == 0 ? "<#ff0000>Off" : "<#00ff00>On").PadLeft(21, '.')}\"\n" +
                    $"\"Sword Progression....{(SaveFile.GetInt("randomizer sword progression enabled") == 0 ? "<#ff0000>Off" : "<#00ff00>On").PadLeft(21, '.')}\"\n" +
                    $"\"Started With Sword...{(SaveFile.GetInt("randomizer started with sword") == 0 ? "<#ff0000>No" : "<#00ff00>Yes").PadLeft(21, '.')}\"\n" +
                    $"\"Shuffled Abilities...{(SaveFile.GetInt("randomizer shuffled abilities") == 0 ? "<#ff0000>Off" : "<#00ff00>On").PadLeft(21, '.')}\"", 
                    (Il2CppSystem.Action)QuickSettings.CopyQuickSettingsInGame, null);
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
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                if (OptionsGUIPatches.BonusOptionsUnlocked) {
                    PlayerCharacter.instance.GetComponent<Animator>().SetBool("wave", true);
                }
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
            if ((LoadSecondSword || LoadThirdSword || WearHat) && (GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy/") != null)) {
                try {
                    if (LoadSecondSword) { SwordProgression.EnableSecondSword(); }
                    if (LoadThirdSword) { SwordProgression.EnableThirdSword(); }
                    if (WearHat) { 
                        GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/head/floppy hat").SetActive(true);
                        WearHat = false;
                    }
                } catch (Exception ex) {
                    Logger.LogError("Error applying upgraded sword!");
                }
            }
            if (LoadCustomTexture && GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/head/GameObject") != null) {
                PaletteEditor.LoadCustomTexture();
                LoadCustomTexture = false;
            }
            if (SpeedrunData.timerRunning && ResetDayNightTimer != -1.0f && SaveFile.GetInt("randomizer died to heir") != 1) {
                ResetDayNightTimer += Time.fixedUnscaledDeltaTime;
                CycleController.IsNight = false;
                if (ResetDayNightTimer >= 5.0f) {
                    CycleController.AnimateSunrise();
                    SaveFile.SetInt("randomizer died to heir", 1);
                    ResetDayNightTimer = -1.0f;
                }
            }
            if (SpeedrunFinishlineDisplayPatches.ShowSwordAfterDelay) {
                FinishLineSwordTimer += Time.fixedUnscaledDeltaTime;
                if (FinishLineSwordTimer > 3.5f) {
                    FinishLineSwordTimer = 0.0f;
                    int SwordLevel = SaveFile.GetInt("randomizer sword progression level");
                    GameObject.Find("_FinishlineDisplay(Clone)/Finishline Camera/Vertical Group/Item Parade Group/").transform.GetChild(1).GetChild(1).GetComponent<Image>().enabled = false;
                    GameObject.Instantiate(SwordLevel == 3 ? ModelSwaps.SecondSwordImage : ModelSwaps.ThirdSwordImage, GameObject.Find("_FinishlineDisplay(Clone)/Finishline Camera/Vertical Group/Item Parade Group/").transform.GetChild(1)).GetComponent<RawImage>().color = new Color(1, 1, 1, 0.65f);
                    SpeedrunFinishlineDisplayPatches.ShowSwordAfterDelay = false;
                }
            }
            if (SpeedrunFinishlineDisplayPatches.ShowCompletionStatsAfterDelay) {
                CompletionTimer += Time.fixedUnscaledDeltaTime;
                if (CompletionTimer > 6.0f) {
                    CompletionTimer = 0.0f;
                    SpeedrunFinishlineDisplay.instance.transform.GetChild(2).gameObject.SetActive(true);
                    SpeedrunFinishlineDisplayPatches.ShowCompletionStatsAfterDelay = false;
                }
            }
            if (SpeedrunData.timerRunning && SceneLoaderPatches.SceneName != null && Hints.AllScenes.Count > 0) {
                float AreaPlaytime = SaveFile.GetFloat($"randomizer play time {SceneLoaderPatches.SceneName}");
                SaveFile.SetFloat($"randomizer play time {SceneLoaderPatches.SceneName}", AreaPlaytime + Time.unscaledDeltaTime);
            }
            if (CustomItemBehaviors.IsTeleporting) {
                PlayerCharacter.instance.cheapIceParticleSystemEmission.enabled = true;
                PlayerCharacter.instance.damageBoostParticleSystemEmission.enabled = true;
                PlayerCharacter.instance.staminaBoostParticleSystemEmission.enabled = true;
                PlayerCharacter.instance._CompletelyInvulnerableEvenToIFrameIgnoringAttacks_k__BackingField = true;
                PlayerCharacter.instance.AddPoison(1f);
                if (PlayerCharacter.instance.gameObject.GetComponent<Rotate>() != null) {
                    PlayerCharacter.instance.gameObject.GetComponent<Rotate>().eulerAnglesPerSecond += new Vector3(0, 3.5f, 0);
                }
            }
            if (SaveFile.GetInt("randomizer shuffled abilities") == 1 && SaveFile.GetInt("randomizer prayer unlocked") == 0) {
                __instance.prayerBeginTimer = 0;
            }
            if (SaveFile.GetInt("randomizer shuffled abilities") == 1 && SaveFile.GetInt("randomizer ice rod unlocked") == 0) {
                TechbowItemBehaviour.kIceShotWindow = 0;
            }

            foreach (string Key in EnemyRandomizer.Enemies.Keys.ToList()) {
                EnemyRandomizer.Enemies[Key].SetActive(false);
                EnemyRandomizer.Enemies[Key].transform.position = new Vector3(-30000f, -30000f, -30000f);
            }

        }

        public static void PlayerCharacter_Start_PostfixPatch(PlayerCharacter __instance) {
            
            if (Hints.AllScenes.Count == 0) {
                for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
                    string SceneName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                    Hints.AllScenes.Add(SceneName);
                }
            }
            if (PaletteEditor.ToonFox.GetComponent<MeshRenderer>() == null) { 
                PaletteEditor.ToonFox.AddComponent<MeshRenderer>().material = __instance.transform.GetChild(25).GetComponent<SkinnedMeshRenderer>().material;
            }
            StateVariable.GetStateVariableByName("SV_ShopTrigger_Fortress").BoolValue = true;
            StateVariable.GetStateVariableByName("SV_ShopTrigger_Sewer").BoolValue = true;
            StateVariable.GetStateVariableByName("SV_ShopTrigger_Swamp(Night)").BoolValue = true;
            StateVariable.GetStateVariableByName("SV_ShopTrigger_WestGarden").BoolValue = true;

            CustomItemBehaviors.CanTakeGoldenHit = false;
            CustomItemBehaviors.CanSwingGoldenSword = false;

            int seed = SaveFile.GetInt("seed");

            if (seed == 0) {
                seed = QuickSettings.CustomSeed == 0 ? new System.Random().Next() : QuickSettings.CustomSeed;
                Logger.LogInfo($"Generated new seed: " + seed);
                SaveFile.SetInt("seed", seed);

                SaveFile.SetString("randomizer game mode", Enum.GetName(typeof(RandomizerSettings.GameModes), TunicRandomizer.Settings.GameMode));
                if (TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST) {

                    SaveFile.SetInt("randomizer hexagon quest goal", TunicRandomizer.Settings.HexagonQuestGoal);
                    SaveFile.SetInt("randomizer hexagon quest extras", TunicRandomizer.Settings.HexagonQuestExtraPercentage);

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
                if (TunicRandomizer.Settings.EntranceRandoEnabled)
                {
                    Inventory.GetItemByName("Torch").Quantity = 1;
                    SaveFile.SetInt("randomizer entrance rando enabled", 1);
                }
                if (TunicRandomizer.Settings.ERFixedShop)
                {
                    SaveFile.SetInt("randomizer ER fixed shop", 1);
                }
                if (TunicRandomizer.Settings.ShuffleAbilities) {
                    SaveFile.SetInt("randomizer shuffled abilities", 1);
                }
                foreach (string Scene in Hints.AllScenes) {
                    SaveFile.SetFloat($"randomizer play time {Scene}", 0.0f);
                }

                EnemyRandomizer.CreateAreaSeeds();

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

            TextBuilderPatches.SetupCustomGlyphSprites();

            ItemRandomizer.PopulateSphereZero();
            ItemRandomizer.RandomizeAndPlaceItems();
            TunicRandomizer.Tracker.ImportantItems["Coins Tossed"] = StateVariable.GetStateVariableByName("Trinket Coins Tossed").IntValue;
            HeirAssistModeDamageValue = ItemPatches.ItemsPickedUp.Values.ToList().Where(item => item == true).ToList().Count / 15;
            Inventory.GetItemByName("Homeward Bone Statue").icon = Inventory.GetItemByName("Dash Stone").icon;
            Inventory.GetItemByName("Spear").icon = Inventory.GetItemByName("MoneyBig").icon;
            if (Inventory.GetItemByName("Spear").TryCast<ButtonAssignableItem>() != null) {
                Inventory.GetItemByName("Spear").TryCast<ButtonAssignableItem>().useMPUsesForQuantity = true;
                Dat.floatDatabase["mpCost_Spear_mp2"] = 40f;
            }
            /*            if (Inventory.GetItemByName("Crystal Ball").TryCast<ButtonAssignableItem>() != null) {
                            Inventory.GetItemByName("Crystal Ball").TryCast<ButtonAssignableItem>().useMPUsesForQuantity = true;
                            Dat.floatDatabase["mpCost_Crystal Ball_mp2"] = 40f;
                        }*/
            foreach (string RelicItem in ItemPatches.HeroRelicLookup.Keys) { 
                Inventory.GetItemByName(RelicItem).collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                Inventory.GetItemByName(RelicItem).collectionMessage.text = $"\"{ItemPatches.HeroRelicLookup[RelicItem].CollectionMessage}\"";
            }
            Inventory.GetItemByName("Crystal Ball").icon = ModelSwaps.FindSprite("Inventory items_specialitem");
            Inventory.GetItemByName("Key (House)").icon = Inventory.GetItemByName("Key Special").icon;
            Inventory.GetItemByName("MoneyLevelItem").Quantity = 1;
            CustomItemBehaviors.SetupTorchItemBehaviour(__instance);

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
            if (ItemRandomizer.CreateSpoilerLog) {
                ItemRandomizer.PopulateSpoilerLog();
            }
            Logger.LogInfo("Wrote Spoiler Log to " + TunicRandomizer.SpoilerLogPath);
            ItemRandomizer.PopulateHints();


            if (TunicRandomizer.Settings.UseCustomTexture) {
                LoadCustomTexture = true;
            }

            if (TunicRandomizer.Settings.RandomFoxColorsEnabled) {
                PaletteEditor.RandomizeFoxColors();
            }

            if (!ModelSwaps.SwappedThisSceneAlready) {
                ModelSwaps.SwapItemsInScene();
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

            if (SaveFile.GetInt("randomizer shuffled abilities") == 1 && SaveFile.GetInt("randomizer holy cross unlocked") == 0) {
                foreach (ToggleObjectBySpell SpellToggle in Resources.FindObjectsOfTypeAll<ToggleObjectBySpell>()) {
                    foreach (ToggleObjectBySpell Spell in SpellToggle.gameObject.GetComponents<ToggleObjectBySpell>()) {
                        Spell.enabled = false;
                    }
                }
            }

            // this is here for the first time you're loading in, assumes you're in Overworld
            if (SaveFile.GetInt("randomizer entrance rando enabled") == 1) {
                TunicPortals.AltModifyPortals();
            }

            PaletteEditor.SetupPartyHat(__instance);
            
            if (PaletteEditor.CelShadingEnabled) { 
                PaletteEditor.ApplyCelShading();
            }
            if (PaletteEditor.PartyHatEnabled) {
                WearHat = true;
            }

        }

        public static void PlayerCharacter_creature_Awake_PostfixPatch(PlayerCharacter __instance) {
            __instance.gameObject.AddComponent<WaveSpell>();
        }

        public static bool Monster_IDamageable_ReceiveDamage_PrefixPatch(Monster __instance) {

            if (__instance.name == "Foxgod" && SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST") {
                return false;
            }
            if (__instance.name == "_Fox(Clone)") {
                if (CustomItemBehaviors.CanTakeGoldenHit) {
                    GameObject.Find("_Fox(Clone)/fox").GetComponent<CreatureMaterialManager>().originalMaterials = CustomItemBehaviors.FoxBody.GetComponent<MeshRenderer>().materials;
                    GameObject.Find("_Fox(Clone)/fox hair").GetComponent<CreatureMaterialManager>().originalMaterials = CustomItemBehaviors.FoxHair.GetComponent<MeshRenderer>().materials;
                    GameObject.Find("_Fox(Clone)/fox").GetComponent<CreatureMaterialManager>()._ghostMaterialArray = CustomItemBehaviors.GhostFoxBody.GetComponent<MeshRenderer>().materials;
                    GameObject.Find("_Fox(Clone)/fox hair").GetComponent<CreatureMaterialManager>()._ghostMaterialArray = CustomItemBehaviors.GhostFoxHair.GetComponent<MeshRenderer>().materials;
                    SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
                    CustomItemBehaviors.CanTakeGoldenHit = false;
                    return false;
                }
            } else {
                if (__instance.name == "Foxgod" && TunicRandomizer.Settings.HeirAssistModeEnabled) {
                    __instance.hp -= HeirAssistModeDamageValue;
                }
                if (CustomItemBehaviors.CanSwingGoldenSword) {
                    __instance.hp -= 30;
                    if (SaveFile.GetInt("randomizer sword progression level") == 3) {
                        GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy").transform.GetChild(4).GetComponent<MeshRenderer>().materials = ModelSwaps.SecondSword.GetComponent<MeshRenderer>().materials;
                    } else if (SaveFile.GetInt("randomizer sword progression level") == 4) {
                        GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy").transform.GetChild(4).GetComponent<MeshRenderer>().materials = ModelSwaps.ThirdSword.GetComponent<MeshRenderer>().materials;
                    } else {
                        GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy").GetComponent<MeshRenderer>().materials = CustomItemBehaviors.Sword.GetComponent<MeshRenderer>().materials;
                    }
                    SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
                    CustomItemBehaviors.CanSwingGoldenSword = false;
                }
            }
            return true;
        }

        public static void PlayerCharacter_Die_MoveNext_PostfixPatch(PlayerCharacter._Die_d__481 __instance, ref bool __result) {

            if (!__result) {
                int Deaths = SaveFile.GetInt("randomizer death count");
                SaveFile.SetInt("randomizer death count", Deaths + 1);
            }
        }

        /*        public static void CrossbowItemBehavior___fireBow_PostfixPatch(CrossbowItemBehaviour __instance) {

                }

                public static bool CrossbowItemBehavior_onActionButtonDown_PrefixPatch(CrossbowItemBehaviour __instance) {

                    return true;
                }*/

    }
}
