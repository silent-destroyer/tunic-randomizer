using FMODUnity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {
    public class OptionsGUIPatches {

        public static bool BonusOptionsUnlocked = false;

        public static bool OptionsGUI_page_root_PrefixPatch(OptionsGUI __instance) {
            addPageButton("Randomizer Settings", RandomizerSettingsPage);
            return true;
        }

        public static void RandomizerSettingsPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("Randomizer");
            addPageButton("General Settings", GeneralSettingsPage);
            addPageButton("Single Player Settings", LogicSettingsPage);
            addPageButton("Archipelago Settings", ArchipelagoSettingsPage);
            addPageButton("Hint Settings", HintsSettingsPage);
            addPageButton("Enemy Randomizer Settings", EnemyRandomizerSettings);
            addPageButton("Fox Customization Settings", CustomFoxSettingsPage);
            addPageButton("Race Mode Settings", RaceSettingsPage);
            addPageButton("Music Shuffle Settings", MusicSettingsPage);
            addPageButton("Other Settings", OtherSettingsPage);
        }

        public static void ArchipelagoSettingsPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("Archipelago");
            OptionsGUI.addToggle("Death Link", "Off", "On", TunicRandomizer.Settings.DeathLinkEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleDeathLink);
            OptionsGUI.addToggle("Auto-open !collect-ed Checks", "Off", "On", TunicRandomizer.Settings.CollectReflectsInWorld ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleUpdateOnCollect);
            OptionsGUI.addToggle("Send Hints to Server", "Off", "On", TunicRandomizer.Settings.SendHintsToServer ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleSendHintsToServer);
        }

        public static void LogicSettingsPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            Il2CppStringArray GameModes = (Il2CppStringArray)new string[] { "<#FFA300>Randomizer", "<#ffd700>Hexagon Quest", "<#4FF5D4>Vanilla" };
            Il2CppStringArray LaurelsLocations = (Il2CppStringArray)new string[] { "<#FFA300>Random", "<#ffd700>6 Coins", "<#ffd700>10 Coins", "<#ffd700>10 Fairies" };
            Il2CppStringArray FoolTrapOptions = (Il2CppStringArray)new string[] { "<#FFFFFF>None", "<#4FF5D4>Normal", "<#E3D457>Double", "<#FF3333>Onslaught" };

            OptionsGUI.setHeading("Single Player");

            if (SceneManager.GetActiveScene().name == "TitleScreen" || IsArchipelago()) {
                OptionsGUI.addMultiSelect("Game Mode", GameModes, GetGameModeIndex(), (OptionsGUIMultiSelect.MultiSelectAction)ChangeGameMode).wrap = true;
                OptionsGUI.addToggle("Keys Behind Bosses", "Off", "On", TunicRandomizer.Settings.KeysBehindBosses ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleKeysBehindBosses);
                OptionsGUI.addToggle("Sword Progression", "Off", "On", TunicRandomizer.Settings.SwordProgressionEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleSwordProgression);
                OptionsGUI.addToggle("Start With Sword", "Off", "On", TunicRandomizer.Settings.StartWithSwordEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleStartWithSword);
                OptionsGUI.addToggle("Shuffle Abilities", "Off", "On", TunicRandomizer.Settings.ShuffleAbilities ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleAbilityShuffling);
                OptionsGUI.addToggle("Shuffle Ladders", "Off", "On", TunicRandomizer.Settings.ShuffleLadders ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleLadderShuffle);
                OptionsGUI.addToggle("Entrance Randomizer", "Off", "On", TunicRandomizer.Settings.EntranceRandoEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleEntranceRando);
                OptionsGUI.addToggle("Entrance Randomizer: Fewer Shops", "Off", "On", TunicRandomizer.Settings.ERFixedShop ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleFixedShop);
                OptionsGUI.addToggle("Entrance Randomizer: Direction Pairs", "Off", "On", TunicRandomizer.Settings.PortalDirectionPairs ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)TogglePortalDirectionPairs);
                OptionsGUI.addMultiSelect("Fool Traps", FoolTrapOptions, GetFoolTrapIndex(), (OptionsGUIMultiSelect.MultiSelectAction)ChangeFoolTrapFrequency).wrap = true;
                OptionsGUI.addMultiSelect("Laurels Location", LaurelsLocations, GetLaurelsLocationIndex(), (OptionsGUIMultiSelect.MultiSelectAction)ChangeLaurelsLocation).wrap = true;
                OptionsGUI.addToggle("Lanternless Logic", "Off", "On", TunicRandomizer.Settings.Lanternless ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleLanternless);
                OptionsGUI.addToggle("Maskless Logic", "Off", "On", TunicRandomizer.Settings.Maskless ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleMaskless);
                OptionsGUI.addToggle("Mystery Seed", "Off", "On", TunicRandomizer.Settings.MysterySeed ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleMysterySeed);
            } else {
                if (SaveFile.GetInt("randomizer mystery seed") == 1) {
                    OptionsGUI.addButton("Mystery Seed", "<#00ff00>On", null);
                    return;
                }
                OptionsGUI.addButton("Game Mode", SaveFile.GetString("randomizer game mode"), null);
                if (SaveFile.GetInt("randomizer hexagon quest enabled") == 1) {
                    OptionsGUI.addButton("Hexagon Quest Goal", SaveFile.GetInt("randomizer hexagon quest goal").ToString(), null);
                    OptionsGUI.addButton("Hexagons in Item Pool", ((int)Math.Round((100f + SaveFile.GetInt("randomizer hexagon quest extras")) / 100f * SaveFile.GetInt("randomizer hexagon quest goal"))).ToString(), null);
                }
                OptionsGUI.addButton("Keys Behind Bosses", SaveFile.GetInt("randomizer keys behind bosses") == 1 ? "<#00ff00>On" : "<#ff0000>Off", null);
                OptionsGUI.addButton("Sword Progression", SaveFile.GetInt("randomizer sword progression enabled") == 1 ? "<#00ff00>On" : "<#ff0000>Off", null);
                OptionsGUI.addButton("Started With Sword", SaveFile.GetInt("randomizer started with sword") == 1 ? "<#00ff00>Yes" : "<#ff0000>No", null);
                OptionsGUI.addButton("Shuffled Abilities", SaveFile.GetInt("randomizer shuffled abilities") == 1 ? "<#00ff00>On" : "<#ff0000>Off", null);
                OptionsGUI.addButton("Shuffled Ladders", SaveFile.GetInt("randomizer ladder rando enabled") == 1 ? "<#00ff00>On" : "<#ff0000>Off", null);
                OptionsGUI.addButton("Entrance Randomizer", SaveFile.GetInt("randomizer entrance rando enabled") == 1 ? "<#00ff00>On" : "<#ff0000>Off", null);
                if (SaveFile.GetInt("randomizer entrance rando enabled") == 1 && IsSinglePlayer()) {
                    OptionsGUI.addButton("Entrance Randomizer: Fewer Shops", SaveFile.GetInt(FixedShop) == 1 ? "<#00ff00>On" : "<#ff0000>Off", null);
                }
                OptionsGUI.addButton("Laurels Location", LaurelsLocations[SaveFile.GetInt("randomizer laurels location")], null);
                OptionsGUI.addButton("Lanternless Logic", SaveFile.GetInt(LanternlessLogic) == 1 ? "<#00ff00>On" : "<#ff0000>Off", null);
                OptionsGUI.addButton("Maskless Logic", SaveFile.GetInt(MasklessLogic) == 1 ? "<#00ff00>On" : "<#ff0000>Off", null);
                OptionsGUI.addMultiSelect("Fool Traps", FoolTrapOptions, GetFoolTrapIndex(), (OptionsGUIMultiSelect.MultiSelectAction)ChangeFoolTrapFrequency).wrap = true;
                OptionsGUI.addButton("Copy Settings String", (Action)RandomizerSettings.copySettings);
            }
        }

        public static void HintsSettingsPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.addToggle("Path of the Hero Hints", "Off", "On", TunicRandomizer.Settings.HeroPathHintsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)TogglePathOfHeroHints);
            OptionsGUI.addToggle("Ghost Fox Hints", "Off", "On", TunicRandomizer.Settings.GhostFoxHintsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleGhostFoxHints);
            OptionsGUI.addToggle("Freestanding Items Match Contents", "Off", "On", TunicRandomizer.Settings.ShowItemsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleShowItems);
            OptionsGUI.addToggle("Chests Match Contents", "Off", "On", TunicRandomizer.Settings.ChestsMatchContentsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleChestsMatchContents);
            OptionsGUI.addToggle("Display Hints in Trunic", "Off", "On", TunicRandomizer.Settings.UseTrunicTranslations ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleTrunicHints);
            OptionsGUI.addToggle("Seeking Spell Uses Logic", "Off", "On", TunicRandomizer.Settings.SeekingSpellLogic ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleFairyLogic);
            if (!Archipelago.instance.integration.disableSpoilerLog) {
                OptionsGUI.addToggle("Spoiler Log", "Off", "On", TunicRandomizer.Settings.CreateSpoilerLog ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleSpoilerLog);
                OptionsGUI.addButton("Open Spoiler Log", (Action)OpenLocalSpoilerLog);
            }
            OptionsGUI.setHeading("Hints");
        }

        public static void GeneralSettingsPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("General");
            OptionsGUI.addToggle("Easier Heir Fight", "Off", "On", TunicRandomizer.Settings.HeirAssistModeEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleHeirAssistMode);
            OptionsGUI.addToggle("Clear Early Bushes", "Off", "On", TunicRandomizer.Settings.ClearEarlyBushes ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleClearEarlyBushes);
            OptionsGUI.addToggle("Enable All Checkpoints", "Off", "On", TunicRandomizer.Settings.EnableAllCheckpoints ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleAllCheckpoints);
            OptionsGUI.addToggle("Cheaper Shop Items", "Off", "On", TunicRandomizer.Settings.CheaperShopItemsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleCheaperShopItems);
            OptionsGUI.addToggle("Bonus Upgrades", "Off", "On", TunicRandomizer.Settings.BonusStatUpgradesEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleBonusStatUpgrades);
            OptionsGUI.addToggle("Disable Chest Interruption", "Off", "On", TunicRandomizer.Settings.DisableChestInterruption ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleChestInterruption);
            OptionsGUI.addToggle("Skip Item Popups", "Off", "On", TunicRandomizer.Settings.SkipItemAnimations ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleSkipItemAnimations);
            OptionsGUI.addToggle("Skip Upgrade Animations", "Off", "On", TunicRandomizer.Settings.FasterUpgrades ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleFasterUpgrades);
        }

        public static void EnemyRandomizerSettings() {
            Il2CppStringArray EnemyDifficulties = (Il2CppStringArray)new string[] { "Random", "Balanced" };
            Il2CppStringArray EnemyGenerationTypes = (Il2CppStringArray)new string[] { "Random", "Seeded" };
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("Enemy Randomization");
            OptionsGUI.addToggle("Enemy Randomizer", "Off", "On", TunicRandomizer.Settings.EnemyRandomizerEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleEnemyRandomizer);
            OptionsGUI.addToggle("Extra Enemies", "Off", "On", TunicRandomizer.Settings.ExtraEnemiesEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleExtraEnemies);
            OptionsGUI.addToggle("Balanced Enemies", "Off", "On", TunicRandomizer.Settings.BalancedEnemies ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleBalancedEnemies);
            OptionsGUI.addToggle("Seeded Enemies", "Off", "On", TunicRandomizer.Settings.SeededEnemies ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleSeededEnemies);
            OptionsGUI.addToggle("Limit Boss Spawns", "Off", "On", TunicRandomizer.Settings.LimitBossSpawns ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)((int index) => { TunicRandomizer.Settings.LimitBossSpawns = !TunicRandomizer.Settings.LimitBossSpawns; SaveSettings(); }));
            OptionsGUI.addToggle("Oops! All [Enemy]", "Off", "On", TunicRandomizer.Settings.OopsAllEnemy ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)((int index) => { TunicRandomizer.Settings.OopsAllEnemy = !TunicRandomizer.Settings.OopsAllEnemy; SaveSettings(); }));
            OptionsGUI.addToggle("Use Enemy Toggles", "Off", "On", TunicRandomizer.Settings.UseEnemyToggles ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleExcludeEnemies);
            addPageButton("Configure Enemy Toggles", EnemyTogglesPage);
        }

        public static void EnemyTogglesPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("Enemy Toggles");

            Action<int, bool> toggleAllEnemies = (int index, bool toggle) => {
                GameObject.FindObjectsOfType<OptionsGUIMultiSelect>().ToList().ForEach((button) => {
                    button.SelectIndex(index);
                    TunicRandomizer.Settings.EnemyToggles[button.leftAlignedText.text] = toggle;
                });
                SaveSettings();
            };

            OptionsGUI.addButton("Toggle All Enemies ON", (Action)(() => {
                toggleAllEnemies(1, true);
            })); 
            OptionsGUI.addButton("Toggle All Enemies OFF", (Action)(() => {
                toggleAllEnemies(0, false);
            }));

            OptionsGUI.addButton("Randomize All", (Action)(() => {
                System.Random random = new System.Random();
                GameObject.FindObjectsOfType<OptionsGUIMultiSelect>().ToList().ForEach((button) => {
                    int selection = random.Next(2);
                    button.SelectIndex(selection);
                    TunicRandomizer.Settings.EnemyToggles[button.leftAlignedText.text] = selection == 1;
                }); SaveSettings();
            }));
            Dictionary<string, Action<int>> toggles = new Dictionary<string, Action<int>>();
            foreach(string enemy in EnemyRandomizer.EnemyToggleOptionNames.Values) {
                toggles.Add(enemy, (int index) => {
                    TunicRandomizer.Settings.EnemyToggles[enemy] = !TunicRandomizer.Settings.EnemyToggles[enemy];
                    SaveSettings();
                });
            }
            foreach (string enemy in EnemyRandomizer.EnemyToggleOptionNames.Values) {
                OptionsGUI.addToggle(enemy, "Off", "On", TunicRandomizer.Settings.EnemyToggles[enemy] ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)toggles[enemy]);
            }
        }

        public static void CustomFoxSettingsPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("Fox Customization");
            OptionsGUI.addToggle("Random Fox Colors", "Off", "On", TunicRandomizer.Settings.RandomFoxColorsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleRandomFoxPalette);
            OptionsGUI.addToggle("Keepin' It Real", "Off", "On", TunicRandomizer.Settings.RealestAlwaysOn ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleSunglasses);
            OptionsGUI.addButton($"Open Fox Color Editor", (Action)(() => {
                if (SceneManager.GetActiveScene().name == "TitleScreen") {
                    GenericMessage.ShowMessage($"\"Fox Color Editor can only\"\n\"be opened while in-game.\"");
                } else {
                    PaletteEditor.EditorOpen = true; 
                    CameraController.DerekZoom = 0.35f; 
                    GUIMode.ClearStack(); 
                    GUIMode.PushGameMode(); 
                }
            }));
            OptionsGUI.addToggle("Use Custom Texture", "Off", "On", TunicRandomizer.Settings.UseCustomTexture ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleCustomTexture);
            if (BonusOptionsUnlocked && SceneLoaderPatches.SceneName != "TitleScreen") {
                OptionsGUI.addToggle("<#FFA500>BONUS: Cel Shaded Fox", "Off", "On", PaletteEditor.CelShadingEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleCelShading);
                OptionsGUI.addToggle("<#00FFFF>BONUS: Party Hat", "Off", "On", PaletteEditor.PartyHatEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)TogglePartyHat);
            }
            if (StateVariable.GetStateVariableByName("Granted Cape").BoolValue && SceneLoaderPatches.SceneName != "TitleScreen") {
                OptionsGUI.addToggle("<#FF69B4>BONUS: Cape", "Off", "On", Inventory.GetItemByName("Cape").Quantity == 1 ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleCape);

            }
            OptionsGUI.addButton("Reset to Defaults", (Action)ResetToDefaults);
        }

        public static void RaceSettingsPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("Race Time");
            OptionsGUI.addToggle("Race Mode (Enables Race Options)", "Off", "On", TunicRandomizer.Settings.RaceMode ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleRaceMode);
            OptionsGUI.addToggle("Disable Icebolt in Heir Fight", "Off", "On", TunicRandomizer.Settings.DisableIceboltInHeirFight ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleDisableHeirIcebolt);
            OptionsGUI.addToggle("Disable Distant West Bell Shot", "Off", "On", TunicRandomizer.Settings.DisableDistantBellShots ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleDisableDistantDong);
            OptionsGUI.addToggle("Disable Ice Grappling Enemies", "Off", "On", TunicRandomizer.Settings.DisableIceGrappling ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleDisableIceGrapples);
            OptionsGUI.addToggle("Disable Ladder Storage", "Off", "On", TunicRandomizer.Settings.DisableLadderStorage ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleDisableLadderStorage);
            OptionsGUI.addToggle("Disable Upgrade Stealing", "Off", "On", TunicRandomizer.Settings.DisableUpgradeStealing ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleDisableUpgradeStealing);
        }

        public static void OtherSettingsPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("Other");
            OptionsGUI.addToggle("???", "Off", "On", TunicRandomizer.Settings.CameraFlip ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleWeirdMode);
            OptionsGUI.addToggle("More Skulls", "Off", "On", TunicRandomizer.Settings.MoreSkulls ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleMoreSkulls);
            OptionsGUI.addToggle("Arachnophobia Mode", "Off", "On", TunicRandomizer.Settings.ArachnophobiaMode ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleArachnophobiaMode);
            OptionsGUI.addToggle("Holy Cross DDR", "Off", "On", TunicRandomizer.Settings.HolyCrossVisualizer ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleHolyCrossViewer);
            OptionsGUI.addToggle("Bigger Head Mode", "Off", "On", TunicRandomizer.Settings.BiggerHeadMode ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)((int index) => { TunicRandomizer.Settings.BiggerHeadMode = !TunicRandomizer.Settings.BiggerHeadMode; SaveSettings(); }));
            OptionsGUI.addToggle("Tinier Fox Mode", "Off", "On", TunicRandomizer.Settings.TinierFoxMode ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)((int index) => { TunicRandomizer.Settings.TinierFoxMode = !TunicRandomizer.Settings.TinierFoxMode; SaveSettings(); }));
            if (SecretMayor.shouldBeActive || SecretMayor.isCorrectDate()) {
                OptionsGUI.addToggle("Mr Mayor", "Off", "On", SecretMayor.shouldBeActive ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)SecretMayor.ToggleMayorSecret);
            }
            addPageButton("Debug Menu", DebugFunctionsPage);
        }

        public static void MusicSettingsPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("Music Shuffle");
            OptionsGUI.addToggle("Music Shuffle", "Off", "On", TunicRandomizer.Settings.MusicShuffle ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)((int index) => { TunicRandomizer.Settings.MusicShuffle = !TunicRandomizer.Settings.MusicShuffle; SaveSettings(); }));
            OptionsGUI.addToggle("Seeded Music", "Off", "On", TunicRandomizer.Settings.SeededMusic ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)((int index) => { TunicRandomizer.Settings.SeededMusic = !TunicRandomizer.Settings.SeededMusic; SaveSettings(); }));
            addPageButton("Configure Music Toggles", MusicTogglesPage);
            addPageButton("Jukebox", JukeboxPage);
        }

        public static void MusicTogglesPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("Music Toggles");

            Action<int, bool> toggleAllSongs = (int index, bool toggle) => {
                GameObject.FindObjectsOfType<OptionsGUIMultiSelect>().ToList().ForEach((button) => {
                    button.SelectIndex(index);
                    TunicRandomizer.Settings.MusicToggles[button.leftAlignedText.text] = toggle;
                });
                SaveSettings();
            };

            OptionsGUI.addButton("Toggle All Tracks ON", (Action)(() => {
                toggleAllSongs(1, true);
            }));
            OptionsGUI.addButton("Toggle All Tracks OFF", (Action)(() => {
                toggleAllSongs(0, false);
            }));

            OptionsGUI.addButton("Randomize All", (Action)(() => {
                System.Random random = new System.Random();
                GameObject.FindObjectsOfType<OptionsGUIMultiSelect>().ToList().ForEach((button) => {
                    int selection = random.Next(2);
                    button.SelectIndex(selection);
                    TunicRandomizer.Settings.MusicToggles[button.leftAlignedText.text] = selection == 1;
                }); SaveSettings();
            }));
            Dictionary<string, Action<int>> toggles = new Dictionary<string, Action<int>>();
            foreach (string track in MusicShuffler.Tracks.Keys) {
                toggles.Add(track, (int index) => {
                    TunicRandomizer.Settings.MusicToggles[track] = !TunicRandomizer.Settings.MusicToggles[track];
                    SaveSettings();
                });
            }
            foreach (string track in MusicShuffler.Tracks.Keys) {
                OptionsGUI.addToggle(track, "Off", "On", TunicRandomizer.Settings.MusicToggles[track] ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)toggles[track]);
            }
        }

        public static void JukeboxPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("Jukebox");
            OptionsGUI.addButton("Play Random Track", (Action)(() => {
                string randomTrack = MusicShuffler.Tracks.Keys.ToList()[new System.Random().Next(MusicShuffler.Tracks.Count)];
                MusicShuffler.PlayTrack(randomTrack, MusicShuffler.Tracks[randomTrack]); 
            }));
            foreach (KeyValuePair<string, EventReference> pair in MusicShuffler.Tracks) {
                OptionsGUI.addButton(pair.Key, (Action)(() => { MusicShuffler.PlayTrack(pair.Key, pair.Value); }));
            }
        }

        public static void DebugFunctionsPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("Debug");
            OptionsGUI.addButton("Open Saves Folder", (Action)(() => { System.Diagnostics.Process.Start(Application.persistentDataPath + "/SAVES"); }));
            OptionsGUI.addButton("Open Log File", (Action)(() => { System.Diagnostics.Process.Start(Application.dataPath + "/../BepInEx/LogOutput.log"); }));
        }

        public static void addPageButton(string pageName, Action pageMethod) {
            Action<Action> pushPageAction = new Action<Action>(pushPage);
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.addButton(pageName, (Action)delegate () {
                pushPageAction.Invoke(pageMethod);
            });
        }
        
        public static void pushPage(Action pageMethod) {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.pushPage(DelegateSupport.ConvertDelegate<OptionsGUI.PageMethod>(pageMethod));
            OptionsGUI.addButton("Return", new Action(OptionsGUI.popPage));
        }

        // Logic Settings

        public static void ChangeGameMode(int index) {
            TunicRandomizer.Settings.GameMode = (RandomizerSettings.GameModes)index;
            SaveSettings();
        }

        public static int GetGameModeIndex() {
            return (int)TunicRandomizer.Settings.GameMode;
        }

        public static void ChangeLaurelsLocation(int index) {
            TunicRandomizer.Settings.FixedLaurelsOption = (RandomizerSettings.FixedLaurelsType)index;
            SaveSettings();
        }

        public static int GetLaurelsLocationIndex() {
            return (int)TunicRandomizer.Settings.FixedLaurelsOption;
        }

        public static void ToggleKeysBehindBosses(int index) {
            TunicRandomizer.Settings.KeysBehindBosses = !TunicRandomizer.Settings.KeysBehindBosses;
            SaveSettings();
        }

        public static void ToggleStartWithSword(int index) {
            TunicRandomizer.Settings.StartWithSwordEnabled = !TunicRandomizer.Settings.StartWithSwordEnabled;
            SaveSettings();
        }

        public static void ToggleSwordProgression(int index) {
            TunicRandomizer.Settings.SwordProgressionEnabled = !TunicRandomizer.Settings.SwordProgressionEnabled;
            SaveSettings();
        }

        public static void ToggleAbilityShuffling(int index) {
            TunicRandomizer.Settings.ShuffleAbilities = !TunicRandomizer.Settings.ShuffleAbilities;
            SaveSettings();
        }

        public static void ToggleLadderShuffle(int index) {
            TunicRandomizer.Settings.ShuffleLadders = !TunicRandomizer.Settings.ShuffleLadders;
            SaveSettings();
        }

        public static void ToggleEntranceRando(int index) {
            TunicRandomizer.Settings.EntranceRandoEnabled = !TunicRandomizer.Settings.EntranceRandoEnabled;
            SaveSettings();
        }

        public static void ToggleFixedShop(int index) {
            TunicRandomizer.Settings.ERFixedShop = !TunicRandomizer.Settings.ERFixedShop;
            SaveSettings();
        }

        public static void TogglePortalDirectionPairs(int index) {
            TunicRandomizer.Settings.PortalDirectionPairs = !TunicRandomizer.Settings.PortalDirectionPairs;
            SaveSettings();
        }

        public static void ToggleLanternless(int index) {
            TunicRandomizer.Settings.Lanternless = !TunicRandomizer.Settings.Lanternless;
            SaveSettings();
        }

        public static void ToggleMaskless(int index) {
            TunicRandomizer.Settings.Maskless = !TunicRandomizer.Settings.Maskless;
            SaveSettings();
        }

        public static void ToggleMysterySeed(int index) {
            TunicRandomizer.Settings.MysterySeed = !TunicRandomizer.Settings.MysterySeed;
            SaveSettings();
        }

        public static int GetFoolTrapIndex() {
            return (int)TunicRandomizer.Settings.FoolTrapIntensity;
        }

        public static void ChangeFoolTrapFrequency(int index) {

            TunicRandomizer.Settings.FoolTrapIntensity = (RandomizerSettings.FoolTrapOption)index;
            SaveSettings();
        }

        public static void ToggleDeathLink(int index) {
            TunicRandomizer.Settings.DeathLinkEnabled = !TunicRandomizer.Settings.DeathLinkEnabled;

            if (Archipelago.instance.integration != null) {
                if (TunicRandomizer.Settings.DeathLinkEnabled) {
                    Archipelago.instance.integration.EnableDeathLink();
                } else {
                    Archipelago.instance.integration.DisableDeathLink();
                }
            }

            SaveSettings();
        }

        public static void ToggleUpdateOnCollect(int index) {
            TunicRandomizer.Settings.CollectReflectsInWorld = !TunicRandomizer.Settings.CollectReflectsInWorld;
            SaveSettings();
        }

        public static void ToggleSendHintsToServer(int index) {
            TunicRandomizer.Settings.SendHintsToServer = !TunicRandomizer.Settings.SendHintsToServer;
            SaveSettings();
        }

        public static void OpenLocalSpoilerLog() {
            if (File.Exists(TunicRandomizer.SpoilerLogPath)) {
                System.Diagnostics.Process.Start(TunicRandomizer.SpoilerLogPath);
            }
        }

        public static void ToggleSunglasses(int index) {
            TunicRandomizer.Settings.RealestAlwaysOn = !TunicRandomizer.Settings.RealestAlwaysOn;
            if (TunicRandomizer.Settings.RealestAlwaysOn) {
                if (GameObject.FindObjectOfType<RealestSpell>() != null) {
                    GameObject.FindObjectOfType<RealestSpell>().SpellEffect();
                }
            }
            if (!TunicRandomizer.Settings.RealestAlwaysOn) {
                GameObject Realest = GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/head/therealest");
                if (Realest != null) {
                    Realest.SetActive(false);
                }
            }
            SaveSettings();
        }

        public static void SaveSettings() {
            if (!File.Exists(TunicRandomizer.SettingsPath)) {
                File.WriteAllText(TunicRandomizer.SettingsPath, JsonConvert.SerializeObject(TunicRandomizer.Settings, Formatting.Indented));
            } else {
                File.Delete(TunicRandomizer.SettingsPath);
                File.WriteAllText(TunicRandomizer.SettingsPath, JsonConvert.SerializeObject(TunicRandomizer.Settings, Formatting.Indented));
            }
        }

        // Hints
        public static void TogglePathOfHeroHints(int index) {
            TunicRandomizer.Settings.HeroPathHintsEnabled = !TunicRandomizer.Settings.HeroPathHintsEnabled;
            SaveSettings();
        }

        public static void ToggleGhostFoxHints(int index) {
            TunicRandomizer.Settings.GhostFoxHintsEnabled = !TunicRandomizer.Settings.GhostFoxHintsEnabled;
            SaveSettings();
        }


        public static void ToggleShowItems(int index) {
            TunicRandomizer.Settings.ShowItemsEnabled = !TunicRandomizer.Settings.ShowItemsEnabled;
            SaveSettings();
        }

        public static void ToggleChestsMatchContents(int index) {
            TunicRandomizer.Settings.ChestsMatchContentsEnabled = !TunicRandomizer.Settings.ChestsMatchContentsEnabled;
            SaveSettings();
        }

        public static void ToggleTrunicHints(int index) {
            TunicRandomizer.Settings.UseTrunicTranslations = !TunicRandomizer.Settings.UseTrunicTranslations;
            if (SceneManager.GetActiveScene().name != "TitleScreen") {
                Hints.PopulateHints();
                GhostHints.GenerateHints();
                Hints.SetupHeroGraveToggle();
            }
            SaveSettings();
        }

        public static void ToggleSpoilerLog(int index) {
            TunicRandomizer.Settings.CreateSpoilerLog = !TunicRandomizer.Settings.CreateSpoilerLog;
            SaveSettings();
        }

        // Gameplay

        public static void ToggleHeirAssistMode(int index) {
            TunicRandomizer.Settings.HeirAssistModeEnabled = !TunicRandomizer.Settings.HeirAssistModeEnabled;
            SaveSettings();
        }

        public static void ToggleClearEarlyBushes(int index) {
            TunicRandomizer.Settings.ClearEarlyBushes = !TunicRandomizer.Settings.ClearEarlyBushes;
            SaveSettings();
        }

        public static void ToggleCheaperShopItems(int index) {
            TunicRandomizer.Settings.CheaperShopItemsEnabled = !TunicRandomizer.Settings.CheaperShopItemsEnabled;
            SaveSettings();
        }

        public static void ToggleBonusStatUpgrades(int index) {
            TunicRandomizer.Settings.BonusStatUpgradesEnabled = !TunicRandomizer.Settings.BonusStatUpgradesEnabled;
            SaveSettings();
        }

        public static void ToggleChestInterruption(int index) {
            TunicRandomizer.Settings.DisableChestInterruption = !TunicRandomizer.Settings.DisableChestInterruption;
            SaveSettings();
        }

        public static void ToggleSkipItemAnimations(int index) {
            TunicRandomizer.Settings.SkipItemAnimations = !TunicRandomizer.Settings.SkipItemAnimations;
            SaveSettings();
        }


        public static void ToggleFasterUpgrades(int index) {
            TunicRandomizer.Settings.FasterUpgrades = !TunicRandomizer.Settings.FasterUpgrades;
            SaveSettings();
        }

        public static void ToggleFairyLogic(int index) {
            TunicRandomizer.Settings.SeekingSpellLogic = !TunicRandomizer.Settings.SeekingSpellLogic;
            SaveSettings();
        }

        public static void ToggleAllCheckpoints(int index) {
            TunicRandomizer.Settings.EnableAllCheckpoints = !TunicRandomizer.Settings.EnableAllCheckpoints;
            foreach(Campfire campfire in GameObject.FindObjectsOfType<Campfire>().Where(campfire => campfire.gameObject.scene.name == SceneManager.GetActiveScene().name)) {
                campfire.updateUnlockGraphics();
            }
            SaveSettings();
        }

        public static void ToggleMoreSkulls(int index) {
            TunicRandomizer.Settings.MoreSkulls = !TunicRandomizer.Settings.MoreSkulls;
            SaveSettings();
        }

        public static void ToggleArachnophobiaMode(int index) {
            TunicRandomizer.Settings.ArachnophobiaMode = !TunicRandomizer.Settings.ArachnophobiaMode;
            SaveSettings();
        }

        public static void ToggleHolyCrossViewer(int index) {
            TunicRandomizer.Settings.HolyCrossVisualizer = !TunicRandomizer.Settings.HolyCrossVisualizer;
            SaveSettings();
        }

        // Enemy Randomizer
        public static void ToggleEnemyRandomizer(int index) {
            TunicRandomizer.Settings.EnemyRandomizerEnabled = !TunicRandomizer.Settings.EnemyRandomizerEnabled;
            SaveSettings();
        }

        public static void ToggleExtraEnemies(int index) {
            TunicRandomizer.Settings.ExtraEnemiesEnabled = !TunicRandomizer.Settings.ExtraEnemiesEnabled;
            SaveSettings();
        }

        public static void ToggleBalancedEnemies(int index) {
            TunicRandomizer.Settings.BalancedEnemies = !TunicRandomizer.Settings.BalancedEnemies;
            SaveSettings();
        }

        public static void ToggleSeededEnemies(int index) {
            TunicRandomizer.Settings.SeededEnemies = !TunicRandomizer.Settings.SeededEnemies;
            SaveSettings();
        }

        public static void ToggleExcludeEnemies(int index) {
            TunicRandomizer.Settings.UseEnemyToggles = !TunicRandomizer.Settings.UseEnemyToggles;
            SaveSettings();
        }

        // Other
        public static void ToggleWeirdMode(int index) {
            TunicRandomizer.Settings.CameraFlip = !TunicRandomizer.Settings.CameraFlip;
            CameraController.Flip = TunicRandomizer.Settings.CameraFlip;
            SaveSettings();
        }

        public static void ToggleRandomFoxPalette(int index) {
            TunicRandomizer.Settings.RandomFoxColorsEnabled = !TunicRandomizer.Settings.RandomFoxColorsEnabled;
            if (TunicRandomizer.Settings.RandomFoxColorsEnabled) {
                PaletteEditor.RandomizeFoxColors();
            } else {
                if (TunicRandomizer.Settings.UseCustomTexture) {
                    PaletteEditor.LoadCustomTexture();
                } else {
                    PaletteEditor.RevertFoxColors();
                }
            }
            SaveSettings();
        }

        public static void ToggleCustomTexture(int index) {
            TunicRandomizer.Settings.UseCustomTexture = !TunicRandomizer.Settings.UseCustomTexture;
            try {
                if (TunicRandomizer.Settings.UseCustomTexture) {
                    PaletteEditor.LoadCustomTexture();
                } else {
                    if (TunicRandomizer.Settings.RandomFoxColorsEnabled) {
                        PaletteEditor.RandomizeFoxColors();
                    } else {
                        PaletteEditor.RevertFoxColors();
                    }
                }
            } catch (Exception e) {

            }
        }

        public static void ToggleCelShading(int index) {
            if (PaletteEditor.CelShadingEnabled) {
                PaletteEditor.DisableCelShading();
            } else {
                PaletteEditor.ApplyCelShading();
            }
        }

        public static void ToggleCape(int index) {
            Inventory.GetItemByName("Cape").Quantity = index;
        }

        public static void TogglePartyHat(int index) {
            try {
                GameObject PartyHat = GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/head/floppy hat");
                PartyHat.SetActive(!PartyHat.active);
                PaletteEditor.PartyHatEnabled = PartyHat.active;
            } catch (Exception ex) {

            }
        }

        public static void ResetToDefaults() {
            PaletteEditor.RevertFoxColors();
        }

        // Race Settings
        public static void ToggleRaceMode(int index) {
            TunicRandomizer.Settings.RaceMode = !TunicRandomizer.Settings.RaceMode;
            SaveSettings();
        }

        public static void ToggleDisableHeirIcebolt(int index) {
            TunicRandomizer.Settings.DisableIceboltInHeirFight = !TunicRandomizer.Settings.DisableIceboltInHeirFight;
            SaveSettings();
        }

        public static void ToggleDisableDistantDong(int index) {
            TunicRandomizer.Settings.DisableDistantBellShots = !TunicRandomizer.Settings.DisableDistantBellShots;
            SaveSettings();
        }

        public static void ToggleDisableIceGrapples(int index) {
            TunicRandomizer.Settings.DisableIceGrappling = !TunicRandomizer.Settings.DisableIceGrappling;
            SaveSettings();
        }

        public static void ToggleDisableLadderStorage(int index) {
            TunicRandomizer.Settings.DisableLadderStorage = !TunicRandomizer.Settings.DisableLadderStorage;
            SaveSettings();
        }
        public static void ToggleDisableUpgradeStealing(int index) {
            TunicRandomizer.Settings.DisableUpgradeStealing = !TunicRandomizer.Settings.DisableUpgradeStealing;
            SaveSettings();
        }

        public static void SaveFile_GetNewSaveFileName_PostfixPatch(SaveFile __instance, ref string __result) {

            __result = $"{__result.Split('.')[0]}-{(TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO ? "archipelago" : "randomizer")}.tunic";
        }

        public static void FileManagementGUI_rePopulateList_PostfixPatch(FileManagementGUI __instance) {
            Sprite ArchipelagoSprite = ModelSwaps.CustomItemImages["Archipelago Item"].GetComponent<Image>().sprite;
            string[] fileNameList = SaveFile.GetRootSaveFileNameList();
            foreach (FileManagementGUIButton button in GameObject.FindObjectsOfType<FileManagementGUIButton>()) {
                SaveFile.LoadFromPath(fileNameList[button.index]);
                if ((SaveFile.GetInt("archipelago") != 0 || SaveFile.GetInt("randomizer") != 0) && !button.isSpecial) {
                    // Display special icon and "randomized" text to indicate randomizer file
                    button.specialBadge.gameObject.active = true;

                    if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                        button.manpageTMP.transform.parent.GetComponent<Image>().sprite = Inventory.GetItemByName("Hexagon Gold").icon;
                        button.manpageTMP.text = $"{SaveFile.GetInt(GoldHexagonQuantity)}/{SaveFile.GetInt(HexagonQuestGoal)}";
                    } else {
                        // Display randomized page count instead of "vanilla" pages picked up
                        int Pages = 0;
                        for (int i = 0; i < 28; i++) {
                            if (SaveFile.GetInt($"randomizer obtained page {i}") == 1) {
                                Pages++;
                            }
                        }
                        button.manpageTMP.text = Pages.ToString();
                    }
                    List<ItemPresentationGraphic> itemPgs = button.itemPresentationGraphics.ToList();
                    GameObject dathStone = GameObject.Instantiate(itemPgs[0].gameObject);
                    dathStone.transform.parent = button.transform.GetChild(3);
                    dathStone.transform.localPosition = Vector3.zero;
                    dathStone.transform.localScale = Vector3.one * 1.15f;
                    dathStone.GetComponent<Image>().sprite = Inventory.GetItemByName("Dath Stone").icon;
                    dathStone.GetComponent<ItemPresentationGraphic>().items = new Item[] { Inventory.GetItemByName("Dath Stone") };
                    itemPgs.Add(dathStone.GetComponent<ItemPresentationGraphic>());
                    button.itemPresentationGraphics = itemPgs.ToArray();
                    dathStone.SetActive(Inventory.GetItemByName("Dath Stone").Quantity > 0);

                    if (SaveFile.GetInt(SwordProgressionEnabled) == 1) {
                        if (SaveFile.GetInt(SwordProgressionLevel) == 3) {
                            button.itemPresentationGraphics[0].GetComponent<Image>().sprite = Inventory.GetItemByName("Librarian Sword").icon;
                            button.itemPresentationGraphics[0].items = new Item[] { Inventory.GetItemByName("Librarian Sword") };
                        }
                        if (SaveFile.GetInt(SwordProgressionLevel) == 3) {
                            button.itemPresentationGraphics[0].GetComponent<Image>().sprite = Inventory.GetItemByName("Heir Sword").icon;
                            button.itemPresentationGraphics[0].items = new Item[] { Inventory.GetItemByName("Heir Sword") };
                        }
                    }

                    button.playtimeString.enableAutoSizing = false;
                    if (SaveFile.GetInt("archipelago") != 0) {
                        button.playtimeString.text += $" <size=55%>archipelago";
                        button.filenameTMP.text += $" <size=65%>({SaveFile.GetString("archipelago player name")})";
                        button.specialBadge.sprite = ArchipelagoSprite;
                    } else if (SaveFile.GetInt("randomizer") != 0) {
                        button.playtimeString.text += $" <size=60%>randomized";
                    }
                }
            }
        }

    }
}
