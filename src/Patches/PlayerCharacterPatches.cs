using Archipelago.MultiClient.Net.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {
    public class PlayerCharacterPatches {

        public static string SaveName = null;
        public static int HeirAssistModeDamageValue = 0;
        public static bool StungByBee = false;
        public static bool TinierFox = false;
        public static bool IsTeleporting = false;
        public static bool DiedToDeathLink = false;
        public static string DeathLinkMessage = "";
        public static int index = 0;

        public static bool LoadSwords = false;
        public static float LoadSwordTimer = 0.0f;
        public static bool LoadCustomTexture = false;
        public static bool WearHat = false;
        public static float TimeWhenLastChangedDayNight = 0.0f;
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
                if (TunicRandomizer.Settings.DeathLinkEffect == RandomizerSettings.DeathLinkType.DEATH) {
                    __instance.hp = -1;
                } else if (TunicRandomizer.Settings.DeathLinkEffect == RandomizerSettings.DeathLinkType.FOOLTRAP) {
                    ItemPatches.ApplyFoolEffect(Archipelago.instance.GetPlayerSlot(), true);
                    DiedToDeathLink = false;
                }
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
                    $"\"Hexagon Quest........{(SaveFile.GetInt(HexagonQuestEnabled) == 0 ? "<#ff0000>Off" : "<#00ff00>On").PadLeft(21, '.')}\"\n" +
                    $"\"Keys Behind Bosses...{(SaveFile.GetInt(KeysBehindBosses) == 0 ? "<#ff0000>Off" : "<#00ff00>On").PadLeft(21, '.')}\"\n" +
                    $"\"Sword Progression....{(SaveFile.GetInt(SwordProgressionEnabled) == 0 ? "<#ff0000>Off" : "<#00ff00>On").PadLeft(21, '.')}\"\n" +
                    $"\"Shuffled Abilities...{(SaveFile.GetInt(AbilityShuffle) == 0 ? "<#ff0000>Off" : "<#00ff00>On").PadLeft(21, '.')}\"\n" +
                    $"\"Shuffled Ladders.....{(SaveFile.GetInt(LadderRandoEnabled) == 0 ? "<#ff0000>Off" : "<#00ff00>On").PadLeft(21, '.')}\"\n" +
                    $"\"Entrance Randomizer..{(SaveFile.GetInt(EntranceRando) == 0 ? "<#ff0000>Off" : "<#00ff00>On").PadLeft(21, '.')}\"",
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

            if (StungByBee || TunicRandomizer.Settings.BiggerHeadMode) {
                __instance.gameObject.transform.Find("Fox/root/pelvis/chest/head").localScale = Vector3.one * 3f;
            }
            if (TinierFox || TunicRandomizer.Settings.TinierFoxMode) {
                __instance.gameObject.transform.localScale = Vector3.one * 0.5f;
                PlayerCharacter.kStopDropRollDistancePerSecondThreshold = 5;
            } else {
                __instance.gameObject.transform.localScale = Vector3.one;
                PlayerCharacter.kStopDropRollDistancePerSecondThreshold = 10;
            }

            if (SaveFile.GetInt(AbilityShuffle) == 1) { 
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

            if (SceneManager.GetActiveScene().name == "FinalBossBefriend" && GameObject.FindObjectOfType<FoxgodCutscenePatch>() == null) {
                new GameObject("foxgod cutscene patcher").gameObject.AddComponent<FoxgodCutscenePatch>();
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
            if (Inventory.GetItemByName("Hyperdash").Quantity == 1) {
                Inventory.GetItemByName("Hyperdash Toggle").Quantity = 1;
            }
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

            // this is here for the first time you're loading in, assumes you're in Overworld
            if (SaveFile.GetInt(EntranceRando) == 1) {
                ERScripts.ModifyPortals("Overworld Redux");
                ERScripts.ModifyPortals("Overworld Redux", sending: true);
                GhostHints.SpawnTorchHintGhost();
            } else {
                ERData.RandomizedPortals = ERData.VanillaPortals;
                ERScripts.ModifyPortalNames("Overworld Redux");
            }

            TunicRandomizer.Tracker.PopulateDiscoveredEntrances();

            try {
                TunicUtils.FindChecksInLogic();
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

            InventoryDisplayPatches.UpdateAbilitySection();

            RandomizerSettings.SaveSettings();

            if (!ModelSwaps.SwappedThisSceneAlready) {
                ModelSwaps.SwapItemsInScene();
            }

            if (!EnemyRandomizer.RandomizedThisSceneAlready && SaveFile.GetInt("seed") != 0 && TunicRandomizer.Settings.EnemyRandomizerEnabled && EnemyRandomizer.Enemies.Count > 0 && !EnemyRandomizer.ExcludedScenes.Contains(SceneManager.GetActiveScene().name)) {
                EnemyRandomizer.SpawnNewEnemies();
            }

            if (TunicRandomizer.Settings.ArachnophobiaMode && !ArachnophobiaMode.DidArachnophobiaModeAlready) {
                ArachnophobiaMode.ToggleArachnophobiaMode();
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

            Vector3 scale = AreaLabel.instance.transform.GetChild(0).localScale;
            if (SaveFile.GetInt(GrassRandoEnabled) == 1 && SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                AreaLabel.instance.transform.GetChild(0).localScale = new Vector3(1.5f, scale.y, scale.z);
            } else {
                AreaLabel.instance.transform.GetChild(0).localScale = new Vector3(1.0777f, scale.y, scale.z);
            }
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
                    System.Random random = new System.Random(seed);

                    SaveFile.SetString("randomizer game mode", System.Enum.GetName(typeof(RandomizerSettings.GameModes), TunicRandomizer.Settings.GameMode));

                    if (TunicRandomizer.Settings.SwordProgressionEnabled) {
                        SaveFile.SetInt(SwordProgressionEnabled, 1);
                        SaveFile.SetInt(SwordProgressionLevel, 0);
                    }
                    if (TunicRandomizer.Settings.StartWithSwordEnabled) {
                        Inventory.GetItemByName("Sword").Quantity = 1;
                        SaveFile.SetInt(StartWithSword, 1);
                    }
                    if (TunicRandomizer.Settings.ShuffleAbilities) {
                        SaveFile.SetInt(AbilityShuffle, 1);
                    }

                    if (SaveFile.GetString("randomizer game mode") != "VANILLA") {
                        if (TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST) {
                            SaveFile.SetInt(HexagonQuestEnabled, 1);
                            if (TunicRandomizer.Settings.HexQuestAbilitiesUnlockedByPages) {
                                SaveFile.SetInt(HexagonQuestPageAbilities, 1);
                            }
                            if (TunicRandomizer.Settings.RandomizeHexQuest) {
                                switch (TunicRandomizer.Settings.HexagonQuestRandomGoal) {
                                    default:
                                    case RandomizerSettings.HexQuestValue.RANDOM:
                                        SaveFile.SetInt(HexagonQuestGoal, random.Next(10, 51));
                                        break;
                                    case RandomizerSettings.HexQuestValue.LOW:
                                        SaveFile.SetInt(HexagonQuestGoal, random.Next(10, 24));
                                        break;
                                    case RandomizerSettings.HexQuestValue.MEDIUM:
                                        SaveFile.SetInt(HexagonQuestGoal, random.Next(24, 38));
                                        break;
                                    case RandomizerSettings.HexQuestValue.HIGH:
                                        SaveFile.SetInt(HexagonQuestGoal, random.Next(38, 51));
                                        break;
                                }
                                switch (TunicRandomizer.Settings.HexagonQuestRandomExtras) {
                                    default:
                                    case RandomizerSettings.HexQuestValue.RANDOM:
                                        SaveFile.SetInt(HexagonQuestExtras, random.Next(101));
                                        break;
                                    case RandomizerSettings.HexQuestValue.LOW:
                                        SaveFile.SetInt(HexagonQuestExtras, random.Next(0, 34));
                                        break;
                                    case RandomizerSettings.HexQuestValue.MEDIUM:
                                        SaveFile.SetInt(HexagonQuestExtras, random.Next(34, 67));
                                        break;
                                    case RandomizerSettings.HexQuestValue.HIGH:
                                        SaveFile.SetInt(HexagonQuestExtras, random.Next(67, 101));
                                        break;
                                }
                                SaveFile.SetInt("randomizer hexagon quest random goal", (int)TunicRandomizer.Settings.HexagonQuestRandomGoal);
                                SaveFile.SetInt("randomizer hexagon quest random extras", (int)TunicRandomizer.Settings.HexagonQuestRandomExtras);
                            } else {
                                SaveFile.SetInt(HexagonQuestGoal, TunicRandomizer.Settings.HexagonQuestGoal);
                                SaveFile.SetInt(HexagonQuestExtras, TunicRandomizer.Settings.HexagonQuestExtraPercentage);
                            }

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

                        if (TunicRandomizer.Settings.KeysBehindBosses) {
                            SaveFile.SetInt(KeysBehindBosses, 1);
                        }

                        if (TunicRandomizer.Settings.Maskless) {
                            SaveFile.SetInt(MasklessLogic, 1);
                        }
                        if (TunicRandomizer.Settings.Lanternless) {
                            SaveFile.SetInt(LanternlessLogic, 1);
                        }

                        SaveFile.SetInt(LaurelsLocation, (int)TunicRandomizer.Settings.FixedLaurelsOption);

                        if (TunicRandomizer.Settings.EntranceRandoEnabled) {
                            Inventory.GetItemByName("Torch").Quantity = 1;
                            SaveFile.SetInt(EntranceRando, 1);

                            if (TunicRandomizer.Settings.ERFixedShop) {
                                SaveFile.SetInt(ERFixedShop, 1);
                            }
                            if (TunicRandomizer.Settings.PortalDirectionPairs) {
                                SaveFile.SetInt(PortalDirectionPairs, 1);
                            }
                            if (TunicRandomizer.Settings.DecoupledER) {
                                SaveFile.SetInt(Decoupled, 1);
                            }
                        }
                        if (TunicRandomizer.Settings.ShuffleLadders) {
                            SaveFile.SetInt(LadderRandoEnabled, 1);
                        }
                        if (TunicRandomizer.Settings.GrassRandomizer) {
                            SaveFile.SetInt(GrassRandoEnabled, 1);
                        }
                        if (TunicRandomizer.Settings.BreakableShuffle) {
                            SaveFile.SetInt(BreakableShuffleEnabled, 1);
                        }

                        if (GetBool(HexagonQuestEnabled) && GetBool(AbilityShuffle) && !GetBool(HexagonQuestPageAbilities)) {
                            int goldHexagons = TunicUtils.GetMaxGoldHexagons();
                            int minHexes = 3;
                            if (GetBool(KeysBehindBosses)) {
                                minHexes = 15;
                            }
                            if (goldHexagons < minHexes) {
                                TunicLogger.LogWarning("Gold Hexagon amount is too low for hexagon ability unlocks, switching to page unlocks.");
                                SaveFile.SetInt(HexagonQuestPageAbilities, 1);
                            }
                        }
                    }
                }

                foreach (string Scene in Locations.AllScenes) {
                    SaveFile.SetFloat($"randomizer play time {Scene}", 0.0f);
                }

                EnemyRandomizer.CreateAreaSeeds();

                SaveFile.SaveToDisk();
            }
            if (TunicRandomizer.Tracker != null && seed != TunicRandomizer.Tracker.Seed) {
                RecentItemsDisplay.instance.ResetQueue();
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

                SaveFile.SetString(ArchipelagoPlayerName, Archipelago.instance.GetPlayerName(Archipelago.instance.GetPlayerSlot()));
                SaveFile.SetString(ArchipelagoPort, TunicRandomizer.Settings.ConnectionSettings.Port);
                SaveFile.SetString(ArchipelagoHostname, TunicRandomizer.Settings.ConnectionSettings.Hostname);
                SaveFile.SetString(ArchipelagoPassword, TunicRandomizer.Settings.ConnectionSettings.Password);

                if (slotData.TryGetValue("hexagon_quest", out var hexagonQuest)) {
                    if (SaveFile.GetInt(HexagonQuestEnabled) == 0 && hexagonQuest.ToString() == "1") {
                        SaveFile.SetInt(HexagonQuestEnabled, 1);
                        if (slotData.TryGetValue("hexagon_quest_ability_type", out var hexagonQuestAbilityType)) {
                            if (hexagonQuestAbilityType.ToString() == "1") {
                                SaveFile.SetInt(HexagonQuestPageAbilities, 1);
                            }
                        }
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
                        if (IsHexQuestWithHexAbilities()) {
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
                if (slotData.TryGetValue("decoupled", out var decoupled)) {
                    if (SaveFile.GetInt(Decoupled) == 0 && decoupled.ToString() == "1") {
                        SaveFile.SetInt(Decoupled, 1);
                    }
                }
                if (slotData.TryGetValue("Entrance Rando", out var entranceRandoPortals)) {
                    ERScripts.CreatePortalPairs(((JObject)slotData["Entrance Rando"]).ToObject<Dictionary<string, string>>());
                    ERScripts.ModifyPortals("Overworld Redux");
                    ERScripts.ModifyPortals("Overworld Redux", sending:true);
                } else {
                    ERScripts.ModifyPortalNames("Overworld Redux");
                }
                if (slotData.TryGetValue("shuffle_ladders", out var ladderRando)) {
                    if (SaveFile.GetInt(LadderRandoEnabled) == 0 && ladderRando.ToString() == "1") {
                        SaveFile.SetInt(LadderRandoEnabled, 1);
                    }
                }
                if (slotData.TryGetValue("breakable_shuffle", out var breakableShuffle)) {
                    if (SaveFile.GetInt(BreakableShuffleEnabled) == 0 && breakableShuffle.ToString() == "1") {
                        SaveFile.SetInt(BreakableShuffleEnabled, 1);
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
                    if (TunicRandomizer.Tracker != null && Seed.ToString() != TunicRandomizer.Tracker.Seed.ToString()) {
                        RecentItemsDisplay.instance.ResetQueue();
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
                if (slotData.TryGetValue("ice_grappling", out var iceGrappling)) {
                    if (iceGrappling.ToString() != "0") {
                        Inventory.GetItemByName("Torch").Quantity = 1;
                    }
                }
                if (slotData.TryGetValue("ladder_storage", out var ladderStorage)) {
                    if (ladderStorage.ToString() != "0") {
                        Inventory.GetItemByName("Torch").Quantity = 1;
                    }
                }
                if (slotData.TryGetValue("grass_randomizer", out var grassRandomizer)) {
                    if (SaveFile.GetInt(GrassRandoEnabled) == 0 && grassRandomizer.ToString() != "0") {
                        SaveFile.SetInt(GrassRandoEnabled, 1);
                    }
                }
                SaveFile.SaveToDisk();

                Locations.RandomizedLocations.Clear();
                Locations.CheckedLocations.Clear();
                ItemLookup.ItemList.Clear();
                List<long> LocationIDs = new List<long>();
                List<Check> ChecksInUse = TunicUtils.GetAllInUseChecks();
                foreach (Check Check in ChecksInUse) {
                    Locations.CheckedLocations.Add(Check.CheckId, SaveFile.GetInt($"randomizer picked up {Check.CheckId}") == 1);
                    long id = Archipelago.instance.integration.session.Locations.GetLocationIdFromName("TUNIC", Locations.LocationIdToDescription[Check.CheckId]);
                    LocationIDs.Add(id);
                    if (Locations.CheckedLocations[Check.CheckId] && !Archipelago.instance.integration.session.Locations.AllLocationsChecked.Contains(id)) {
                        TunicLogger.LogInfo("Checked in save file but not on AP: " + id + " " + Check.CheckId + "[" + Locations.LocationIdToDescription[Check.CheckId] + "]");
                    }
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

                Locations.PopulateMajorItemLocations(slotData);

            }
        }

        public static void GenerateMysterySettings() { 
            System.Random random = new System.Random(SaveFile.GetInt("seed"));

            if (TunicRandomizer.Settings.StartWithSwordEnabled) {
                Inventory.GetItemByName("Sword").Quantity = 1;
                SaveFile.SetInt("randomizer started with sword", 1);
            }

            if (random.Next(100) <= TunicRandomizer.Settings.MysterySeedWeights.SwordProgression) {
                SaveFile.SetInt(SwordProgressionEnabled, 1);
                SaveFile.SetInt(SwordProgressionLevel, 0);
            }
            if (random.Next(100) <= TunicRandomizer.Settings.MysterySeedWeights.KeysBehindBosses) {
                SaveFile.SetInt(KeysBehindBosses, 1);
            }
            if (random.Next(100) <= TunicRandomizer.Settings.MysterySeedWeights.ShuffleAbilities) {
                SaveFile.SetInt(AbilityShuffle, 1);
            }
            if (random.Next(100) <= TunicRandomizer.Settings.MysterySeedWeights.ShuffleLadders) {
                SaveFile.SetInt(LadderRandoEnabled, 1);
            }
            if (random.Next(100) <= TunicRandomizer.Settings.MysterySeedWeights.EntranceRando) {
                SaveFile.SetInt(EntranceRando, 1);
                Inventory.GetItemByName("Torch").Quantity = 1;
            }
            if (random.Next(100) <= TunicRandomizer.Settings.MysterySeedWeights.ERFixedShop) {
                SaveFile.SetInt(ERFixedShop, 1);
            }
            if (random.Next(100) <= TunicRandomizer.Settings.MysterySeedWeights.ERDirectionPairs) {
                SaveFile.SetInt(PortalDirectionPairs, 1);
            }
            if (SaveFile.GetInt(ERFixedShop) == 1 && SaveFile.GetInt(PortalDirectionPairs) == 1) {
                bool chooseOne = random.Next(2) == 1;
                SaveFile.SetInt(ERFixedShop, chooseOne ? 1 : 0);
                SaveFile.SetInt(PortalDirectionPairs, !chooseOne ? 1 : 0);
            }
            if (random.Next(100) <= TunicRandomizer.Settings.MysterySeedWeights.ERDecoupled) {
                SaveFile.SetInt(Decoupled, 1);
            }
            if (random.Next(100) <= TunicRandomizer.Settings.MysterySeedWeights.Maskless) {
                SaveFile.SetInt(MasklessLogic, 1);
            }
            if (random.Next(100) <= TunicRandomizer.Settings.MysterySeedWeights.Lanternless) {
                SaveFile.SetInt(LanternlessLogic, 1);
            }
            if (random.Next(100) <= TunicRandomizer.Settings.MysterySeedWeights.GrassRando) {
                SaveFile.SetInt(GrassRandoEnabled, 1);
            }
            if (random.Next(100) <= TunicRandomizer.Settings.MysterySeedWeights.HexagonQuest) {
                SaveFile.SetInt(HexagonQuestEnabled, 1);
                if (random.Next(100) < TunicRandomizer.Settings.MysterySeedWeights.HexQuestAbilityShufflePages) {
                    SaveFile.SetInt(HexagonQuestPageAbilities, 1);
                }
                bool[] goalOptions = new bool[4] { 
                    TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalRandom,
                    TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalLow,
                    TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalMedium,
                    TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalHigh,
                };
                bool[] extrasOptions = new bool[4] {
                    TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasRandom,
                    TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasLow,
                    TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasMedium,
                    TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasHigh,
                };
                int goalIndex = random.Next(goalOptions.Length);
                if (goalOptions.All(x => !x)) {
                    SaveFile.SetInt(HexagonQuestGoal, random.Next(10, 51));
                } else {
                    while (!goalOptions[goalIndex]) {
                        goalIndex = random.Next(goalOptions.Length);
                    }
                    switch (goalIndex) {
                        default:
                        case 0:
                            SaveFile.SetInt(HexagonQuestGoal, random.Next(10, 51));
                            break;
                        case 1:
                            SaveFile.SetInt(HexagonQuestGoal, random.Next(10, 24));
                            break;
                        case 2:
                            SaveFile.SetInt(HexagonQuestGoal, random.Next(24, 38));
                            break;
                        case 3:
                            SaveFile.SetInt(HexagonQuestGoal, random.Next(38, 51));
                            break;
                    }
                }
                int extrasIndex = random.Next(extrasOptions.Length);
                if (extrasOptions.All(x => !x)) {
                    SaveFile.SetInt(HexagonQuestExtras, random.Next(101));
                } else {
                    while (!extrasOptions[extrasIndex]) {
                        extrasIndex = random.Next(extrasOptions.Length);
                    }
                    switch (extrasIndex) {
                        default:
                        case 0:
                            SaveFile.SetInt(HexagonQuestExtras, random.Next(101));
                            break;
                        case 1:
                            SaveFile.SetInt(HexagonQuestExtras, random.Next(0, 34));
                            break;
                        case 2:
                            SaveFile.SetInt(HexagonQuestExtras, random.Next(34, 67));
                            break;
                        case 3:
                            SaveFile.SetInt(HexagonQuestExtras, random.Next(67, 101));
                            break;
                    }
                }

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

            bool[] foolOptions = new bool[4] {
                TunicRandomizer.Settings.MysterySeedWeights.FoolTrapNone,
                TunicRandomizer.Settings.MysterySeedWeights.FoolTrapNormal,
                TunicRandomizer.Settings.MysterySeedWeights.FoolTrapDouble,
                TunicRandomizer.Settings.MysterySeedWeights.FoolTrapOnslaught,
            };
            int foolIndex = random.Next(foolOptions.Length);
            if (foolOptions.All(x => !x)) {
                TunicRandomizer.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.NONE;
            } else {
                while (!foolOptions[foolIndex]) {
                    foolIndex = random.Next(foolOptions.Length);
                }
                TunicRandomizer.Settings.FoolTrapIntensity = (RandomizerSettings.FoolTrapOption)foolIndex;
            }
            bool[] laurelsOptions = new bool[4] {
                TunicRandomizer.Settings.MysterySeedWeights.LaurelsRandom,
                TunicRandomizer.Settings.MysterySeedWeights.LaurelsSixCoins,
                TunicRandomizer.Settings.MysterySeedWeights.LaurelsTenCoins,
                TunicRandomizer.Settings.MysterySeedWeights.LaurelsTenFairies,
            };
            int laurelsIndex = random.Next(laurelsOptions.Length);
            if (laurelsOptions.All(x => !x)) {
                SaveFile.SetInt(LaurelsLocation, 0);
            } else {
                while (!laurelsOptions[laurelsIndex]) {
                    laurelsIndex = random.Next(laurelsOptions.Length);
                }
                SaveFile.SetInt(LaurelsLocation, laurelsIndex);
            }

            if (GetBool(HexagonQuestEnabled) && GetBool(AbilityShuffle) && !GetBool(HexagonQuestPageAbilities)) {
                int goldHexagons = TunicUtils.GetMaxGoldHexagons();
                int minHexes = 3;
                if (GetBool(KeysBehindBosses)) {
                    minHexes = 15;
                }
                if (goldHexagons < minHexes) {
                    TunicLogger.LogWarning("Gold Hexagon amount is too low for hexagon ability unlocks, switching to page unlocks.");
                    SaveFile.SetInt(HexagonQuestPageAbilities, 1);
                }
            }

            SaveFile.SetString("randomizer mystery seed weights", TunicRandomizer.Settings.MysterySeedWeights.ToSettingsString());
        }

        public static void PlayerCharacter_Die_MoveNext_PostfixPatch(PlayerCharacter._Die_d__481 __instance, ref bool __result) {

            if (!__result) {
                int Deaths = SaveFile.GetInt(PlayerDeathCount);
                SaveFile.SetInt(PlayerDeathCount, Deaths + 1);
                if (IsArchipelago() && TunicRandomizer.Settings.DeathLinkEnabled && Archipelago.instance.integration.session.ConnectionInfo.Tags.Contains("DeathLink") && !DiedToDeathLink) {
                    Archipelago.instance.integration.SendDeathLink();
                }
                DiedToDeathLink = false;
            }
        }

        public static bool PlayerCharacter_OnTouchKillbox_PrefixPatch(PlayerCharacter __instance) {
            if (__instance.GetComponent<PlayerCharacter>() != null && SceneManager.GetActiveScene().name != "Library Arena" && TunicRandomizer.Settings.DeathplanePatch) {
                TunicLogger.LogInfo("rescuing the fox from a deathplane");
                __instance.transform.position = __instance.lastValidNavmeshPosition;
                return false;
            }
            return true;
        }

        public static bool Ladder_ClimbOn_PrefixPatch(Ladder __instance, LadderEnd ladderEnd) {
            LastLadder = ladderEnd;
            return true;
        }

    }
}
