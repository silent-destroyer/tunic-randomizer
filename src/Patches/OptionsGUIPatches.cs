using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TinyJson;
using UnityEngine;
using UnityEngine.UI;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using BepInEx.Logging;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
namespace TunicRandomizer {
    public class OptionsGUIPatches {

        private static ManualLogSource Logger = TunicRandomizer.Logger;
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
                OptionsGUI.addToggle("Game Mode", "<#FFA300>Randomizer", "<#ffd700>Hexagon Quest", (TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST) ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleGameMode);
                OptionsGUI.addToggle("Keys Behind Bosses", "Off", "On", TunicRandomizer.Settings.KeysBehindBosses ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleKeysBehindBosses);
                OptionsGUI.addToggle("Sword Progression", "Off", "On", TunicRandomizer.Settings.SwordProgressionEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleSwordProgression);
                OptionsGUI.addToggle("Start With Sword", "Off", "On", TunicRandomizer.Settings.StartWithSwordEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleStartWithSword);
            } else {
                OptionsGUI.addButton("Game Mode", SaveFile.GetString("randomizer game mode"), null);
                OptionsGUI.addButton("Keys Behind Bosses", SaveFile.GetInt("randomizer keys behind bosses") == 1 ? "<#00ff00>On" : "<#ff0000>Off", null);
                OptionsGUI.addButton("Sword Progression", SaveFile.GetInt("randomizer sword progression enabled") == 1 ? "<#00ff00>On" : "<#ff0000>Off", null);
                OptionsGUI.addButton("Start With Sword", SaveFile.GetInt("randomizer sword progression enabled") == 1 ? "<#00ff00>On" : "<#ff0000>Off", null);
            }
            OptionsGUI.setHeading("Logic");
        }

        public static void HintsSettingsPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.addToggle("Path of the Hero Hints", "Off", "On", TunicRandomizer.Settings.HeroPathHintsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)TogglePathOfHeroHints);
            OptionsGUI.addToggle("Ghost Fox Hints", "Off", "On", TunicRandomizer.Settings.GhostFoxHintsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleGhostFoxHints);
            OptionsGUI.addToggle("Freestanding Items Match Contents", "Off", "On", TunicRandomizer.Settings.ShowItemsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleShowItems);
            OptionsGUI.addToggle("Chests Match Contents", "Off", "On", TunicRandomizer.Settings.ChestsMatchContentsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleChestsMatchContents);
            OptionsGUI.setHeading("Hints");
        }

        public static void GeneralSettingsPage() {
            Il2CppStringArray FoolTrapOptions = (Il2CppStringArray)new string[] { "<#FFFFFF>None", "<#4FF5D4>Normal", "<#E3D457>Double", "<#FF3333>Onslaught" };

            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("General");
            OptionsGUI.addToggle("Easier Heir Fight", "Off", "On", TunicRandomizer.Settings.HeirAssistModeEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleHeirAssistMode);
            OptionsGUI.addToggle("Cheaper Shop Items", "Off", "On", TunicRandomizer.Settings.CheaperShopItemsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleCheaperShopItems);
            OptionsGUI.addToggle("Bonus Upgrades", "Off", "On", TunicRandomizer.Settings.BonusStatUpgradesEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleBonusStatUpgrades);
            //ItemTrackerOverlayButton = __instance.addToggle("Item Tracker Overlay", "Off", "On", TunicRandomizer.Settings.ItemTrackerOverlayEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleTrackerOverlay);
            OptionsGUI.addMultiSelect("Fool Traps", FoolTrapOptions, GetFoolTrapIndex(), (OptionsGUIMultiSelect.MultiSelectAction)ChangeFoolTrapFrequency).wrap = true;
            OptionsGUI.addToggle("???", "Off", "On", CameraController.Flip ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleWeirdMode);
        }

        public static void EnemyRandomizerSettings() {
            Il2CppStringArray EnemyGenerationTypes = (Il2CppStringArray)new string[] { "Random", "Balanced" };
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("Enemy Randomization");
            OptionsGUI.addToggle("Enemy Randomizer", "Off", "On", TunicRandomizer.Settings.EnemyRandomizerEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleEnemyRandomizer);
            OptionsGUI.addMultiSelect("Enemy Types", EnemyGenerationTypes, GetEnemyGenerationType(), (OptionsGUIMultiSelect.MultiSelectAction)ChangeEnemyRandomizerGenerationType).wrap = true;

        }

        public static void CustomFoxSettingsPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("Fox Customization");
            OptionsGUI.addToggle("Random Fox Colors", "Off", "On", TunicRandomizer.Settings.RandomFoxColorsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleRandomFoxPalette);
            OptionsGUI.addToggle("Keepin' It Real", "Off", "On", TunicRandomizer.Settings.RealestAlwaysOn ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleSunglasses);
            OptionsGUI.addToggle("Show Fox Color Editor", "Off", "On", PaletteEditor.EditorOpen ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)TogglePaletteEditor);
            OptionsGUI.addToggle("Use Custom Texture", "Off", "On", TunicRandomizer.Settings.UseCustomTexture ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleCustomTexture);
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

        public static void ToggleGameMode(int index) {
            TunicRandomizer.Settings.GameMode = (RandomizerSettings.GameModes) index;
            SaveSettings();
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
            PlayerCharacterPatches.SetupGoldenTrophyCollectionLines();
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

        public static int GetEnemyGenerationType() {
            return (int)TunicRandomizer.Settings.EnemyGeneration;
        }

        public static void ChangeEnemyRandomizerGenerationType(int index) {
            TunicRandomizer.Settings.EnemyGeneration = (RandomizerSettings.EnemyRandomizationType)index;
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
                PaletteEditor.RevertFoxColors();
            }
            SaveSettings();
        }

        public static void ToggleCustomTexture(int index) {
            TunicRandomizer.Settings.UseCustomTexture = !TunicRandomizer.Settings.UseCustomTexture;
            TunicRandomizer.Settings.RandomFoxColorsEnabled = false;
            PaletteEditor.LoadCustomTexture();
        }


        public static void ResetToDefaults() {
            PaletteEditor.RevertFoxColors();
        }

    }
}
