using Archipelago.MultiClient.Net.Models;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.Globalization;
using Archipelago.MultiClient.Net.Enums;
using static TunicRandomizer.SaveFlags;
using Newtonsoft.Json.Linq;

namespace TunicRandomizer {
    public class PlayerCharacterPatches {

        public static string SaveName = null;
        public static int HeirAssistModeDamageValue = 0;
        public static bool StungByBee = false;
        public static bool IsTeleporting = false;
        public static bool DiedToDeathLink = false;
        public static string DeathLinkMessage = "";
        public static int index = 0;

        public static bool LoadSwords = false;
        public static float LoadSwordTimer = 0.0f;
        public static bool LoadCustomTexture = false;
        public static bool WearHat = false;
        public static float TimeWhenLastChangedDayNight = 0.0f;
        public static float FinishLineSwordTimer = 0.0f;
        public static float CompletionTimer = 0.0f;
        public static float ResetDayNightTimer = -1.0f;
        public static LadderEnd LastLadder = null;

        public static void PlayerCharacter_creature_Awake_PostfixPatch(PlayerCharacter __instance) {

            __instance.gameObject.AddComponent<WaveSpell>();
            __instance.gameObject.AddComponent<EntranceSeekerSpell>();
            __instance.gameObject.AddComponent<DDRSpell>();
            DDRSpell.SetupDPADTester(__instance);
        }

        public static void PlayerCharacter_Update_PostfixPatch(PlayerCharacter __instance) {
            Cheats.FastForward = Input.GetKey(KeyCode.Backslash) && !TunicRandomizer.Settings.RaceMode;

            if (DiedToDeathLink) {
                if (DeathLinkMessage != "") {
                    Notifications.Show(DeathLinkMessage, DeathLinkMessages.SecondaryMessages[new System.Random().Next(DeathLinkMessages.SecondaryMessages.Count)]);
                    DeathLinkMessage = "";
                }
                __instance.hp = -1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                if (SpeedrunFinishlineDisplayPatches.CompletionCanvas != null) {
                    SpeedrunFinishlineDisplayPatches.CompletionCanvas.SetActive(!SpeedrunFinishlineDisplayPatches.CompletionCanvas.active);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && IsSinglePlayer()) {
                if (SaveFile.GetInt("randomizer mystery seed") == 1) {
                    GenericPrompt.ShowPrompt($"\"Copy Current Game Settings?\"\n\"-----------------\"\n" +
                    $"\"Seed.................{SaveFile.GetInt("seed").ToString().PadLeft(12, '.')}\"\n" +
                    $"\"Mystery Seed.........{"<#00ff00>On".PadLeft(21, '.')}\"",
                    (Il2CppSystem.Action)RandomizerSettings.copySettings, null);
                } else {
                    GenericPrompt.ShowPrompt($"\"Copy Current Game Settings?\"\n\"-----------------\"\n" +
                    $"\"Seed.................{SaveFile.GetInt("seed").ToString().PadLeft(12, '.')}\"\n" +
                    $"\"Game Mode............{SaveFile.GetString("randomizer game mode").PadLeft(12, '.')}\"\n" +
                    $"\"Keys Behind Bosses...{(SaveFile.GetInt("randomizer keys behind bosses") == 0 ? "<#ff0000>Off" : "<#00ff00>On").PadLeft(21, '.')}\"\n" +
                    $"\"Sword Progression....{(SaveFile.GetInt("randomizer sword progression enabled") == 0 ? "<#ff0000>Off" : "<#00ff00>On").PadLeft(21, '.')}\"\n" +
                    $"\"Shuffled Abilities...{(SaveFile.GetInt("randomizer shuffled abilities") == 0 ? "<#ff0000>Off" : "<#00ff00>On").PadLeft(21, '.')}\"\n" +
                    $"\"Shuffled Ladders.....{(SaveFile.GetInt("randomizer ladder rando enabled") == 0 ? "<#ff0000>Off" : "<#00ff00>On").PadLeft(21, '.')}\"\n" +
                    $"\"Entrance Randomizer..{(SaveFile.GetInt("randomizer entrance rando enabled") == 0 ? "<#ff0000>Off" : "<#00ff00>On").PadLeft(21, '.')}\"",
                    (Il2CppSystem.Action)RandomizerSettings.copySettings, null);
                }
            }

            if (Input.GetKeyDown(KeyCode.R) && IsArchipelago()) {
                Archipelago.instance.Release();
            }

            if (Input.GetKeyDown(KeyCode.C) && IsArchipelago()) {
                Archipelago.instance.Collect();
            }

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
            if (LoadSwords && (GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy/") != null)) {
                try {
                    SwordProgression.CreateSwordItemBehaviours(__instance);
                    LoadSwords = false;
                } catch (Exception ex) {
                    TunicLogger.LogError("Error applying upgraded sword!");
                }
            }
            if (WearHat && (GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/head/floppy hat") != null)) {
                GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/head/floppy hat").SetActive(true);
                WearHat = false;
            }
            if (LoadCustomTexture && GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/head/GameObject") != null) {
                PaletteEditor.LoadCustomTexture();
                LoadCustomTexture = false;
            }
            if (SpeedrunData.timerRunning && ResetDayNightTimer != -1.0f && SaveFile.GetInt(DiedToHeir) != 1) {
                ResetDayNightTimer += Time.fixedUnscaledDeltaTime;
                CycleController.IsNight = false;
                if (ResetDayNightTimer >= 5.0f) {
                    CycleController.AnimateSunrise();
                    SaveFile.SetInt(DiedToHeir, 1);
                    ResetDayNightTimer = -1.0f;
                }
            }
            if (SpeedrunData.gameComplete != 0 && !SpeedrunFinishlineDisplayPatches.GameCompleted) {
                SpeedrunFinishlineDisplayPatches.GameCompleted = true;
                SpeedrunFinishlineDisplayPatches.SetupCompletionStatsDisplay();
            }
            if (SpeedrunFinishlineDisplayPatches.ShowCompletionStatsAfterDelay) {
                CompletionTimer += Time.fixedUnscaledDeltaTime;
                if (CompletionTimer > 6.0f) {
                    CompletionTimer = 0.0f;
                    SpeedrunFinishlineDisplayPatches.CompletionCanvas.SetActive(true);
                    SpeedrunFinishlineDisplayPatches.ShowCompletionStatsAfterDelay = false;
                }
            }
            if (SpeedrunData.timerRunning && SceneLoaderPatches.SceneName != null && Locations.AllScenes.Count > 0) {
                float AreaPlaytime = SaveFile.GetFloat($"randomizer play time {SceneLoaderPatches.SceneName}");
                SaveFile.SetFloat($"randomizer play time {SceneLoaderPatches.SceneName}", AreaPlaytime + Time.unscaledDeltaTime);
            }
            if (IsTeleporting) {
                PlayerCharacter.instance.cheapIceParticleSystemEmission.enabled = true;
                PlayerCharacter.instance.damageBoostParticleSystemEmission.enabled = true;
                PlayerCharacter.instance.staminaBoostParticleSystemEmission.enabled = true;
                PlayerCharacter.instance._CompletelyInvulnerableEvenToIFrameIgnoringAttacks_k__BackingField = true;
                PlayerCharacter.instance.AddPoison(1f);
                if (PlayerCharacter.instance.gameObject.GetComponent<Rotate>() != null) {
                    PlayerCharacter.instance.gameObject.GetComponent<Rotate>().eulerAnglesPerSecond += new Vector3(0, 3.5f, 0);
                }
            }

            if(SaveFile.GetInt(AbilityShuffle) == 1) { 
                if(SaveFile.GetInt(PrayerUnlocked) == 0) {
                    __instance.prayerBeginTimer = 0;
                }
                if(SaveFile.GetInt(IceBoltUnlocked) == 0) {
                    TechbowItemBehaviour.kIceShotWindow = 0;
                }
            }

            if (TunicRandomizer.Settings.RaceMode) {
                // Disables icebolt in heir arena
                if (TunicRandomizer.Settings.DisableIceboltInHeirFight && SceneManager.GetActiveScene().name == "Spirit Arena") {
                    TechbowItemBehaviour.kIceShotWindow = 0;
                }
                // Prevents ladder storage from being used
                if (TunicRandomizer.Settings.DisableLadderStorage && __instance.currentLadder != null) {
                    if (__instance.cachedAnimator.GetBool("climbing") && __instance.cachedAnimator.GetBool("sprint")) {
                        if (__instance.transform.position.x > LastLadder.transform.position.x + 5 || __instance.transform.position.x < LastLadder.transform.position.x - 5
                            || __instance.transform.position.z > LastLadder.transform.position.z + 5 || __instance.transform.position.z < LastLadder.transform.position.z - 5) {

                            if (LastLadder != null) {
                                __instance.currentLadder.ClimbOn(LastLadder);
                            } else {
                                __instance.cachedAnimator.SetBool("climbing", false);
                                __instance.currentLadder = null;
                                __instance.Flinch(true);
                            }
                        }
                    }
                    if (__instance.cachedAnimator.GetBool("climbing") && (__instance.cachedAnimator.GetBool("swing sword") || __instance.cachedAnimator.GetBool("swing stick"))) {
                        __instance.cachedAnimator.SetBool("climbing", false);
                        __instance.currentLadder = null;
                        __instance.Flinch(true);
                    }
                }
            }

            if (__instance.currentLadder == null && LastLadder != null) {
                LastLadder = null;
            }

            if (PaletteEditor.FoxCape != null) {
                PaletteEditor.FoxCape.GetComponent<CreatureMaterialManager>().UseSpecialGhostMat = __instance.transform.GetChild(1).GetComponent<CreatureMaterialManager>().UseSpecialGhostMat;
            }

            if (SceneManager.GetActiveScene().name == "FinalBossBefriend" && GameObject.FindObjectOfType<HexagonQuestCutscene>() == null && SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                new GameObject("hex quest cutscene").gameObject.AddComponent<HexagonQuestCutscene>();
            }

            foreach (string Key in EnemyRandomizer.Enemies.Keys.ToList()) {
                EnemyRandomizer.Enemies[Key].SetActive(false);
                EnemyRandomizer.Enemies[Key].transform.position = new Vector3(-30000f, -30000f, -30000f);
            }

        }

        public static void PlayerCharacter_Start_PostfixPatch(PlayerCharacter __instance) {
            SceneLoaderPatches.TimeOfLastSceneTransition = SaveFile.GetFloat("playtime");

            // hide inventory prompt button so it doesn't overlap item messages
            GameObject InvButton = Resources.FindObjectsOfTypeAll<Animator>().Where(animator => animator.gameObject.name == "LB Prompt").ToList()[0].gameObject;
            if (InvButton != null) {
                InvButton.transform.GetChild(0).gameObject.SetActive(false);
                InvButton.transform.GetChild(1).gameObject.SetActive(false);
                InvButton.SetActive(false);
            }

            if (Locations.AllScenes.Count == 0) {
                for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
                    string SceneName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                    Locations.AllScenes.Add(SceneName);
                }
            }

            StateVariable.GetStateVariableByName("SV_ShopTrigger_Fortress").BoolValue = true;
            StateVariable.GetStateVariableByName("SV_ShopTrigger_Sewer").BoolValue = true;
            StateVariable.GetStateVariableByName("SV_ShopTrigger_Swamp(Night)").BoolValue = true;
            StateVariable.GetStateVariableByName("SV_ShopTrigger_WestGarden").BoolValue = true;

            CustomItemBehaviors.CanTakeGoldenHit = false;
            CustomItemBehaviors.CanSwingGoldenSword = false;

            TunicRandomizer.Tracker.ImportantItems["Coins Tossed"] = StateVariable.GetStateVariableByName("Trinket Coins Tossed").IntValue;

            Inventory.GetItemByName("Spear").icon = Inventory.GetItemByName("MoneyBig").icon;
            if (Inventory.GetItemByName("Spear").TryCast<ButtonAssignableItem>() != null) {
                Inventory.GetItemByName("Spear").TryCast<ButtonAssignableItem>().useMPUsesForQuantity = true;
                Dat.floatDatabase["mpCost_Spear_mp2"] = 40f;
            }
            Inventory.GetItemByName("MoneyLevelItem").Quantity = 1;
            Inventory.GetItemByName("Key (House)").icon = Inventory.GetItemByName("Key Special").icon;

            CustomItemBehaviors.SetupTorchItemBehaviour(__instance);

            LoadSwords = true;

            ItemPresentationPatches.SwitchDathStonePresentation();

            int seed = SaveFile.GetInt("seed");

            if (seed == 0 && SaveFile.GetInt("archipelago") == 0 && SaveFile.GetInt("randomizer") == 0) {
                if (TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER) {
                    SaveFile.SetInt("randomizer", 1);
                } else if (TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO) {
                    SaveFile.SetInt("archipelago", 1);
                }
                SaveFile.SaveToDisk();
            }

            if (IsSinglePlayer()) {
                Archipelago.instance.Disconnect();
                PlayerCharacter_Start_SinglePlayerSetup();
            } else if (IsArchipelago()) {
                PlayerCharacter_Start_ArchipelagoSetup();
            }

            if (TunicRandomizer.Settings.CreateSpoilerLog && !TunicRandomizer.Settings.RaceMode) {
                ItemTracker.PopulateSpoilerLog();
            }

            Hints.PopulateHints();
            
            GhostHints.GenerateHints();
            
            if (Hints.HeroGraveHints.Count != 0) {
                Hints.SetupHeroGraveToggle();
            }

            if (SaveFile.GetInt(AbilityShuffle) == 1 && SaveFile.GetInt(HolyCrossUnlocked) == 0) {
                ItemPatches.ToggleHolyCrossObjects(false);
            }

            if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                TunicRandomizer.Tracker.ImportantItems["Pages"] = 28;
                SaveFile.SetInt("last page viewed", 0);
            }

            try {
                FairyTargets.CreateFairyTargets();
                FairyTargets.CreateEntranceTargets();
                FairyTargets.FindFairyTargets();
            } catch (Exception ex) {
                TunicLogger.LogError("An error occurred creating new fairy seeker spell targets:");
                TunicLogger.LogError(ex.Message + " " + ex.StackTrace);
            }

            if (!SceneLoaderPatches.SpawnedGhosts && TunicRandomizer.Settings.GhostFoxHintsEnabled) {
                GhostHints.SpawnHintGhosts(SceneLoaderPatches.SceneName);
            }

            ItemStatsHUD.UpdateAbilitySection();

            OptionsGUIPatches.SaveSettings();

            if (!ModelSwaps.SwappedThisSceneAlready) {
                ModelSwaps.SwapItemsInScene();
            }
            // this is here for the first time you're loading in, assumes you're in Overworld
            if (SaveFile.GetInt("randomizer entrance rando enabled") == 1) {
                TunicPortals.ModifyPortals("Overworld Redux");
            }

            try {
                if (SaveFile.GetInt(LadderRandoEnabled) == 1) {
                    LadderToggles.ToggleLadders();
                }
            } catch (Exception e) {
                TunicLogger.LogError("Error toggling ladders! " + e.Source + " " + e.Message + " " + e.StackTrace);
            }

            if (PaletteEditor.ToonFox.GetComponent<MeshRenderer>() == null) {
                PaletteEditor.ToonFox.AddComponent<MeshRenderer>().material = __instance.transform.GetChild(25).GetComponent<SkinnedMeshRenderer>().material;
            }

            PaletteEditor.GatherHyperdashRenderers();
            PaletteEditor.SetupPartyHat(__instance);
            PaletteEditor.SetupFoxCape(__instance);

            if (TunicRandomizer.Settings.RandomFoxColorsEnabled) {
                try {
                    PaletteEditor.RandomizeFoxColors();
                } catch(Exception e) {
                    TunicLogger.LogInfo("Error randomizing fox colors!");
                }
            }

            if (TunicRandomizer.Settings.UseCustomTexture) {
                LoadCustomTexture = true;
            }

            if (TunicRandomizer.Settings.RealestAlwaysOn) {
                GameObject.FindObjectOfType<RealestSpell>().SpellEffect();
            }

            if (PaletteEditor.CelShadingEnabled) {
                PaletteEditor.ApplyCelShading();
            }

            if (PaletteEditor.PartyHatEnabled) {
                WearHat = true;
            }
            List<MagicSpell> spells = __instance.spells.ToList();
            spells.Reverse();
            __instance.spells = spells.ToArray();
        }

        private static void PlayerCharacter_Start_SinglePlayerSetup() {
            int seed = SaveFile.GetInt("seed");

            if (seed == 0) {
                seed = QuickSettings.CustomSeed == "" ? new System.Random().Next() : int.Parse(QuickSettings.CustomSeed);
                TunicLogger.LogInfo($"Starting new single player file with seed: " + seed);
                SaveFile.SetInt("seed", seed);
                SaveFile.SetInt("randomizer", 1);

                if (TunicRandomizer.Settings.MysterySeed) {
                    SaveFile.SetInt("randomizer mystery seed", 1);
                    GenerateMysterySettings();
                } else {
                    SaveFile.SetString("randomizer game mode", Enum.GetName(typeof(RandomizerSettings.GameModes), TunicRandomizer.Settings.GameMode));
                    if (TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST) {
                        SaveFile.SetInt(HexagonQuestEnabled, 1);
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

                    if (TunicRandomizer.Settings.Maskless) {
                        SaveFile.SetInt(MasklessLogic, 1);
                    }
                    if (TunicRandomizer.Settings.Lanternless) {
                        SaveFile.SetInt(LanternlessLogic, 1);
                    }

                    SaveFile.SetInt("randomizer laurels location", (int)TunicRandomizer.Settings.FixedLaurelsOption);

                    if (TunicRandomizer.Settings.EntranceRandoEnabled) {
                        Inventory.GetItemByName("Torch").Quantity = 1;
                        SaveFile.SetInt("randomizer entrance rando enabled", 1);
                    }
                    if (TunicRandomizer.Settings.ERFixedShop) {
                        SaveFile.SetInt("randomizer ER fixed shop", 1);
                    }
                    if (TunicRandomizer.Settings.ShuffleAbilities) {
                        SaveFile.SetInt("randomizer shuffled abilities", 1);
                    }
                    if (TunicRandomizer.Settings.ShuffleLadders) {
                        SaveFile.SetInt(LadderRandoEnabled, 1);
                    }
                }

                foreach (string Scene in Locations.AllScenes) {
                    SaveFile.SetFloat($"randomizer play time {Scene}", 0.0f);
                }

                EnemyRandomizer.CreateAreaSeeds();

                SaveFile.SaveToDisk();
            }
            TunicRandomizer.Tracker = new ItemTracker();
            TunicRandomizer.Tracker.Seed = seed;
            TunicLogger.LogInfo("Loading single player seed: " + TunicRandomizer.Settings.GetSettingsString());
            ItemRandomizer.PopulatePrecollected();
            ItemRandomizer.RandomizeAndPlaceItems();
        }

        private static void PlayerCharacter_Start_ArchipelagoSetup() {
            if (!Archipelago.instance.integration.connected) {
                TunicLogger.LogInfo("player start connecting to ap");
                Archipelago.instance.Connect();
            } else {
                if (TunicRandomizer.Settings.DeathLinkEnabled) {
                    Archipelago.instance.integration.EnableDeathLink();
                } else {
                    Archipelago.instance.integration.DisableDeathLink();
                }
            }

            if (Archipelago.instance.integration.connected) {
                Archipelago.instance.integration.sentCompletion = false;
                Archipelago.instance.integration.sentRelease = false;
                Archipelago.instance.integration.sentCollect = false;

                Dictionary<string, object> slotData = Archipelago.instance.GetPlayerSlotData();
                if (SaveFile.GetString("archipelago player name") == "") {
                    SaveFile.SetString("archipelago player name", Archipelago.instance.GetPlayerName(Archipelago.instance.GetPlayerSlot()));
                }

                if (slotData.TryGetValue("hexagon_quest", out var hexagonQuest)) {
                    if (SaveFile.GetInt(HexagonQuestEnabled) == 0 && hexagonQuest.ToString() == "1") {
                        SaveFile.SetInt(HexagonQuestEnabled, 1);
                        for (int i = 0; i < 28; i++) {
                            SaveFile.SetInt($"randomizer obtained page {i}", 1);
                        }
                        StateVariable.GetStateVariableByName("Placed Hexagon 1 Red").BoolValue = true;
                        StateVariable.GetStateVariableByName("Placed Hexagon 2 Green").BoolValue = true;
                        StateVariable.GetStateVariableByName("Placed Hexagon 3 Blue").BoolValue = true;
                        StateVariable.GetStateVariableByName("Placed Hexagons ALL").BoolValue = true;
                        StateVariable.GetStateVariableByName("Has Been Betrayed").BoolValue = true;
                        StateVariable.GetStateVariableByName("Has Died To God").BoolValue = true;

                        if (slotData.TryGetValue("Hexagon Quest Goal", out var hexagonGoal)) {
                            SaveFile.SetInt(HexagonQuestGoal, int.Parse(hexagonGoal.ToString()));
                        }
                    }
                }
                if (slotData.TryGetValue("start_with_sword", out var startWithSword)) {
                    if (SaveFile.GetInt("randomizer started with sword") == 0 && startWithSword.ToString() == "1") {
                        SaveFile.SetInt("randomizer started with sword", 1);
                    }
                }
                if (slotData.TryGetValue("ability_shuffling", out var abilityShuffling)) {
                    if (SaveFile.GetInt(AbilityShuffle) == 0 && abilityShuffling.ToString() == "1") {
                        SaveFile.SetInt(AbilityShuffle, 1);
                        if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                            SaveFile.SetInt(HexagonQuestPrayer, int.Parse(slotData["Hexagon Quest Prayer"].ToString(), CultureInfo.InvariantCulture));
                            SaveFile.SetInt(HexagonQuestHolyCross, int.Parse(slotData["Hexagon Quest Holy Cross"].ToString(), CultureInfo.InvariantCulture));
                            SaveFile.SetInt(HexagonQuestIcebolt, int.Parse(slotData["Hexagon Quest Icebolt"].ToString(), CultureInfo.InvariantCulture));
                        }
                    }
                    if (abilityShuffling.ToString() == "0") {
                        SaveFile.SetInt(PrayerUnlocked, 1);
                        SaveFile.SetInt(HolyCrossUnlocked, 1);
                        SaveFile.SetInt(IceBoltUnlocked, 1);
                    }
                }
                if (slotData.TryGetValue("sword_progression", out var swordProgression)) {
                    if (SaveFile.GetInt(SwordProgressionEnabled) == 0 && swordProgression.ToString() == "1") {
                        TunicLogger.LogInfo("sword progression enabled");
                        SaveFile.SetInt(SwordProgressionEnabled, 1);
                    }
                }
                if (slotData.TryGetValue("keys_behind_bosses", out var keysBehindBosses)) {
                    if (SaveFile.GetInt(KeysBehindBosses) == 0 && keysBehindBosses.ToString() == "1") {
                        TunicLogger.LogInfo("keys behind bosses enabled");
                        SaveFile.SetInt(KeysBehindBosses, 1);
                    }
                }
                if (slotData.TryGetValue("entrance_rando", out var entranceRando)) {
                    if (SaveFile.GetInt(EntranceRando) == 0 && entranceRando.ToString() == "1") {
                        SaveFile.SetInt(EntranceRando, 1);
                        Inventory.GetItemByName("Torch").Quantity = 1;
                    }
                }
                if (slotData.TryGetValue("Entrance Rando", out var entranceRandoPortals)) {
                    TunicPortals.CreatePortalPairs(((JObject)slotData["Entrance Rando"]).ToObject<Dictionary<string, string>>());
                    TunicPortals.ModifyPortals("Overworld Redux");
                }
                if (slotData.TryGetValue("shuffle_ladders", out var ladderRando)) {
                    if (SaveFile.GetInt(LadderRandoEnabled) == 0 && ladderRando.ToString() == "1") {
                        SaveFile.SetInt(LadderRandoEnabled, 1);
                    }
                }
                if (slotData.TryGetValue("seed", out var Seed)) {
                    if (SaveFile.GetInt("seed") == 0) {
                        SaveFile.SetInt("seed", int.Parse(Seed.ToString(), CultureInfo.InvariantCulture));
                        EnemyRandomizer.CreateAreaSeeds();
                        TunicLogger.LogInfo("Starting new archipelago file with seed: " + Seed);
                    } else {
                        TunicLogger.LogInfo("Loading archipelago seed: " + SaveFile.GetInt("seed"));
                    }
                    TunicRandomizer.Tracker = new ItemTracker();
                    TunicRandomizer.Tracker.Seed = int.Parse(Seed.ToString());
                    TunicRandomizer.Tracker.PopulateTrackerForAP();
                }
                if (slotData.TryGetValue("logic_rules", out var logicRules)) {
                    if (logicRules.ToString() == "2") {
                        Inventory.GetItemByName("Torch").Quantity = 1;
                    }
                }
                SaveFile.SaveToDisk();

                Locations.PopulateMajorItemLocations(slotData);

                Locations.RandomizedLocations.Clear();
                Locations.CheckedLocations.Clear();
                ItemLookup.ItemList.Clear();
                List<long> LocationIDs = new List<long>();
                foreach (string Key in Locations.VanillaLocations.Keys) {
                    Locations.CheckedLocations.Add(Key, SaveFile.GetInt($"randomizer picked up {Key}") == 1);
                    LocationIDs.Add(Archipelago.instance.integration.session.Locations.GetLocationIdFromName("TUNIC", Locations.LocationIdToDescription[Key]));
                }
                if (LocationIDs.Contains(-1L)) {
                    Notifications.Show($"\"An error has occurred!\"", $"\"Connected slot is incompatible with this client version.\"");
                    TunicLogger.LogInfo("Error: Connected slot is incompatible with this client version.");
                    Archipelago.instance.Disconnect();
                } else {
                    Archipelago.instance.integration.session.Locations.ScoutLocationsAsync(LocationIDs.ToArray()).ContinueWith(locationInfoPacket => {
                        foreach (ItemInfo ItemInfo in locationInfoPacket.Result.Values) {
                            ItemLookup.ItemList.Add(Locations.LocationDescriptionToId[ItemInfo.LocationName], ItemInfo);
                        }
                    }).Wait(TimeSpan.FromSeconds(5.0f));
                    TunicLogger.LogInfo("Successfully scouted locations for item placements");

                    Archipelago.instance.integration.UpdateDataStorageOnLoad();
                }

            }
        }

        public static void GenerateMysterySettings() { 
            System.Random random = new System.Random(SaveFile.GetInt("seed"));

            SaveFile.SetString("randomizer game mode", ((RandomizerSettings.GameModes)random.Next(2)).ToString());
            if (SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST") {
                SaveFile.SetInt(HexagonQuestEnabled, 1);
                SaveFile.SetInt("randomizer hexagon quest goal", random.Next(15, 51));
                SaveFile.SetInt("randomizer hexagon quest extras", random.Next(101));

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

            SaveFile.SetInt("randomizer sword progression enabled", 1);
            SaveFile.SetInt("randomizer sword progression level", 0);

            if (random.Next(2) == 1) {
                SaveFile.SetInt("randomizer keys behind bosses", 1);
            }
            if (random.Next(2) == 1) {
                Inventory.GetItemByName("Sword").Quantity = 1;
                SaveFile.SetInt("randomizer started with sword", 1);
            }

            if (random.NextDouble() < 0.25) {
                SaveFile.SetInt(MasklessLogic, 1);
            }
            if (random.NextDouble() < 0.25) {
                SaveFile.SetInt(LanternlessLogic, 1);
            }

            TunicRandomizer.Settings.FoolTrapIntensity = (RandomizerSettings.FoolTrapOption)random.Next(4);

            SaveFile.SetInt("randomizer laurels location", random.NextDouble() < 0.75 ? 0 : random.Next(1, 4));

            if (random.Next(2) == 1) {
                SaveFile.SetInt("randomizer entrance rando enabled", 1);
                Inventory.GetItemByName("Torch").Quantity = 1;
            }
            if (random.Next(2) == 1) {
                SaveFile.SetInt("randomizer ER fixed shop", 1);
            }
            if (random.Next(2) == 1) {
                SaveFile.SetInt("randomizer shuffled abilities", 1);
            }
            if (random.Next(2) == 1) {
                SaveFile.SetInt(LadderRandoEnabled, 1);
            }
        }

        public static void PlayerCharacter_Die_MoveNext_PostfixPatch(PlayerCharacter._Die_d__481 __instance, ref bool __result) {

            if (!__result) {
                int Deaths = SaveFile.GetInt(PlayerDeathCount);
                SaveFile.SetInt(PlayerDeathCount, Deaths + 1);
                if (TunicRandomizer.Settings.DeathLinkEnabled && Archipelago.instance.integration.session.ConnectionInfo.Tags.Contains("DeathLink") && !DiedToDeathLink) {
                    Archipelago.instance.integration.SendDeathLink();
                }
                DiedToDeathLink = false;
            }
        }

        public static bool Ladder_ClimbOn_PrefixPatch(Ladder __instance, LadderEnd ladderEnd) {
            LastLadder = ladderEnd;
            return true;
        }

    }
}
