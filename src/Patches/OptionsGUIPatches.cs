using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TinyJson;
using UnityEngine;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using BepInEx.Logging;
using static TunicRandomizer.RandomizerSettings;
using UnityEngine.Playables;

namespace TunicRandomizer {
    public class OptionsGUIPatches {

        private static ManualLogSource Logger = TunicRandomizer.Logger;

        public static bool BonusOptionsUnlocked = false;

        public static bool OptionsGUI_page_root_PrefixPatch(OptionsGUI __instance) {
            addPageButton("Randomizer Settings", RandomizerSettingsPage);
            return true;
        }

        public static void RandomizerSettingsPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("Randomizer");
            addPageButton("Logic Settings", LogicSettingsPage);
            addPageButton("Hint Settings", HintsSettingsPage);
            addPageButton("General Settings", GeneralSettingsPage);
            addPageButton("Enemy Randomizer Settings", EnemyRandomizerSettings);
            addPageButton("Fox Customization", CustomFoxSettingsPage);
        }

        public static void LogicSettingsPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            if (SceneLoaderPatches.SceneName == "TitleScreen") {
                Il2CppStringArray GameModes = (Il2CppStringArray)new string[] { "<#FFA300>Randomizer", "<#ffd700>Hexagon Quest", "<#4FF5D4>Vanilla" };
                OptionsGUI.addMultiSelect("Game Mode", GameModes, GetGameModeIndex(), (OptionsGUIMultiSelect.MultiSelectAction)ChangeGameMode).wrap = true;
                OptionsGUI.addToggle("Keys Behind Bosses", "Off", "On", TunicRandomizer.Settings.KeysBehindBosses ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleKeysBehindBosses);
                OptionsGUI.addToggle("Sword Progression", "Off", "On", TunicRandomizer.Settings.SwordProgressionEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleSwordProgression);
                OptionsGUI.addToggle("Start With Sword", "Off", "On", TunicRandomizer.Settings.StartWithSwordEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleStartWithSword);
                OptionsGUI.addToggle("Shuffle Abilities", "Off", "On", TunicRandomizer.Settings.ShuffleAbilities ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleAbilityShuffling);
                OptionsGUI.addToggle("Entrance Randomizer", "Off", "On", TunicRandomizer.Settings.EntranceRandoEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleEntranceRando);
            } else {
                OptionsGUI.addButton("Game Mode", SaveFile.GetString("randomizer game mode"), null);
                if(SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST") { 
                    OptionsGUI.addButton("Hexagon Quest Goal", SaveFile.GetInt("randomizer hexagon quest goal").ToString(), null);
                    OptionsGUI.addButton("Hexagons in Item Pool", ((int)Math.Round((100f + SaveFile.GetInt("randomizer hexagon quest extras")) / 100f * SaveFile.GetInt("randomizer hexagon quest goal"))).ToString(), null);
                }
                OptionsGUI.addButton("Keys Behind Bosses", SaveFile.GetInt("randomizer keys behind bosses") == 1 ? "<#00ff00>On" : "<#ff0000>Off", null);
                OptionsGUI.addButton("Sword Progression", SaveFile.GetInt("randomizer sword progression enabled") == 1 ? "<#00ff00>On" : "<#ff0000>Off", null);
                OptionsGUI.addButton("Started With Sword", SaveFile.GetInt("randomizer started with sword") == 1 ? "<#00ff00>Yes" : "<#ff0000>No", null);
                OptionsGUI.addButton("Shuffled Abilities", SaveFile.GetInt("randomizer shuffled abilities") == 1 ? "<#00ff00>On" : "<#ff0000>Off", null);
                OptionsGUI.addButton("Entrance Randomizer", SaveFile.GetInt("randomizer entrance rando enabled") == 1 ? "<#00ff00>On" : "<#ff0000>Off", null);
            }
            OptionsGUI.setHeading("Logic");
        }

        public static void HintsSettingsPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.addToggle("Path of the Hero Hints", "Off", "On", TunicRandomizer.Settings.HeroPathHintsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)TogglePathOfHeroHints);
            OptionsGUI.addToggle("Ghost Fox Hints", "Off", "On", TunicRandomizer.Settings.GhostFoxHintsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleGhostFoxHints);
            OptionsGUI.addToggle("Freestanding Items Match Contents", "Off", "On", TunicRandomizer.Settings.ShowItemsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleShowItems);
            OptionsGUI.addToggle("Chests Match Contents", "Off", "On", TunicRandomizer.Settings.ChestsMatchContentsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleChestsMatchContents);
            OptionsGUI.addToggle("Untranslated Text", "Off", "On", TunicRandomizer.Settings.UseTrunicTranslations ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleUntranslatedHints);
            OptionsGUI.setHeading("Hints");
        }

        public static void GeneralSettingsPage() {
            Il2CppStringArray FoolTrapOptions = (Il2CppStringArray)new string[] { "<#FFFFFF>None", "<#4FF5D4>Normal", "<#E3D457>Double", "<#FF3333>Onslaught" };

            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("General");
            OptionsGUI.addToggle("Easier Heir Fight", "Off", "On", TunicRandomizer.Settings.HeirAssistModeEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleHeirAssistMode);
            OptionsGUI.addToggle("Cheaper Shop Items", "Off", "On", TunicRandomizer.Settings.CheaperShopItemsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleCheaperShopItems);
            OptionsGUI.addToggle("Bonus Upgrades", "Off", "On", TunicRandomizer.Settings.BonusStatUpgradesEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleBonusStatUpgrades);
            OptionsGUI.addToggle("Disable Chest Interruption", "Off", "On", TunicRandomizer.Settings.DisableChestInterruption ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleChestInterruption);
            OptionsGUI.addMultiSelect("Fool Traps", FoolTrapOptions, GetFoolTrapIndex(), (OptionsGUIMultiSelect.MultiSelectAction)ChangeFoolTrapFrequency).wrap = true;
            OptionsGUI.addToggle("???", "Off", "On", CameraController.Flip ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleWeirdMode);
        }

        public static void EnemyRandomizerSettings() {
            Il2CppStringArray EnemyDifficulties = (Il2CppStringArray)new string[] { "Random", "Balanced" };
            Il2CppStringArray EnemyGenerationTypes = (Il2CppStringArray)new string[] { "Random", "Seeded" };
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("Enemy Randomization");
            OptionsGUI.addToggle("Enemy Randomizer", "Off", "On", TunicRandomizer.Settings.EnemyRandomizerEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleEnemyRandomizer);
            OptionsGUI.addToggle("Extra Enemies", "Off", "On", TunicRandomizer.Settings.ExtraEnemiesEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleExtraEnemies);
            OptionsGUI.addMultiSelect("Enemy Difficulty", EnemyDifficulties, GetEnemyDifficulty(), (OptionsGUIMultiSelect.MultiSelectAction)ChangeEnemyRandomizerDifficulty).wrap = true;
            OptionsGUI.addMultiSelect("Enemy Generation", EnemyGenerationTypes, GetEnemyGenerationType(), (OptionsGUIMultiSelect.MultiSelectAction)ChangeEnemyRandomizerGenerationType).wrap = true;

        }

        public static void CustomFoxSettingsPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("Fox Customization");
            OptionsGUI.addToggle("Random Fox Colors", "Off", "On", TunicRandomizer.Settings.RandomFoxColorsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleRandomFoxPalette);
            OptionsGUI.addToggle("Keepin' It Real", "Off", "On", TunicRandomizer.Settings.RealestAlwaysOn ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleSunglasses);
            OptionsGUI.addToggle("Show Fox Color Editor", "Off", "On", PaletteEditor.EditorOpen ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)TogglePaletteEditor);
            OptionsGUI.addToggle("Use Custom Texture", "Off", "On", TunicRandomizer.Settings.UseCustomTexture ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleCustomTexture);
            if (BonusOptionsUnlocked && SceneLoaderPatches.SceneName != "TitleScreen") {
                OptionsGUI.addToggle("<#FFA500>BONUS: Cel Shaded Fox", "Off", "On", PaletteEditor.CelShadingEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleCelShading);
                OptionsGUI.addToggle("<#00FFFF>BONUS: Party Hat", "Off", "On", PaletteEditor.PartyHatEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)TogglePartyHat);
            }
            OptionsGUI.addButton("Reset to Defaults", (Action)ResetToDefaults);
        }

        public static void addPageButton(string pageName, Action pageMethod) {
            Action<Action> pushPageAction = new Action<Action>(pushPage);
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.addButton(pageName, (Action) delegate () {
                pushPageAction.Invoke(pageMethod);
            });
        }

        public static void pushPage(Action pageMethod) {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.pushPage(DelegateSupport.ConvertDelegate<OptionsGUI.PageMethod>(pageMethod));
            OptionsGUI.addButton("Return", new Action(OptionsGUI.popPage));
        }


        public static void TogglePaletteEditor(int index) { 
            PaletteEditor.EditorOpen = !PaletteEditor.EditorOpen;
            CameraController.DerekZoom = PaletteEditor.EditorOpen ? 0.35f : 1f;
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
                File.WriteAllText(TunicRandomizer.SettingsPath, JSONWriter.ToJson(TunicRandomizer.Settings));
            } else {
                File.Delete(TunicRandomizer.SettingsPath);
                File.WriteAllText(TunicRandomizer.SettingsPath, JSONWriter.ToJson(TunicRandomizer.Settings));
            }
        }

        // Logic Settings

        public static void ChangeGameMode(int index) {
            TunicRandomizer.Settings.GameMode = (RandomizerSettings.GameModes)index;
            SaveSettings();
        }

        public static int GetGameModeIndex() { 
               
            return (int)TunicRandomizer.Settings.GameMode;
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

        public static void ToggleEntranceRando(int index)
        {
            TunicRandomizer.Settings.EntranceRandoEnabled = !TunicRandomizer.Settings.EntranceRandoEnabled;
            SaveSettings();
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

        public static void ToggleUntranslatedHints(int index) {
            TunicRandomizer.Settings.UseTrunicTranslations = !TunicRandomizer.Settings.UseTrunicTranslations;
            SaveSettings();
        }

        // Gameplay

        public static void ToggleHeirAssistMode(int index) {
            TunicRandomizer.Settings.HeirAssistModeEnabled = !TunicRandomizer.Settings.HeirAssistModeEnabled;
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

        public static int GetFoolTrapIndex() {

            return (int)TunicRandomizer.Settings.FoolTrapIntensity;
        }

        public static void ChangeFoolTrapFrequency(int index) {

            TunicRandomizer.Settings.FoolTrapIntensity = (RandomizerSettings.FoolTrapOption)index;
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
        
        public static int GetEnemyDifficulty() {
            return (int)TunicRandomizer.Settings.EnemyDifficulty;
        }

        public static int GetEnemyGenerationType() {
            return (int)TunicRandomizer.Settings.EnemyGeneration;
        }

        public static void ChangeEnemyRandomizerDifficulty(int index) {
            TunicRandomizer.Settings.EnemyDifficulty = (RandomizerSettings.EnemyRandomizationType)index;
            SaveSettings();
        }

        public static void ChangeEnemyRandomizerGenerationType(int index) {
            TunicRandomizer.Settings.EnemyGeneration = (RandomizerSettings.EnemyGenerationType)index;
            SaveSettings();
        }

        public static void ToggleWeirdMode(int index) {
            CameraController.Flip = !CameraController.Flip;
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
            if (TunicRandomizer.Settings.UseCustomTexture) {
                PaletteEditor.LoadCustomTexture();
            } else {
                if (TunicRandomizer.Settings.RandomFoxColorsEnabled) {
                    PaletteEditor.RandomizeFoxColors();
                } else {
                    PaletteEditor.RevertFoxColors();
                }
            }
        }

        public static void ToggleCelShading(int index) {
            if (PaletteEditor.CelShadingEnabled) {
                PaletteEditor.DisableCelShading();
            } else {
                PaletteEditor.ApplyCelShading();
            }
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

    }
}
