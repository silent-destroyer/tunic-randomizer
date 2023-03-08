using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TinyJson;
using UnityEngine;
using UnityEngine.UI;
using UnhollowerRuntimeLib;

namespace TunicRandomizer {
    public class OptionsGUIPatches {

        private static OptionsGUIMultiSelect SettingsViewer;
        private static OptionsGUIButton GameModeButton;
        private static OptionsGUIButton HexagonQuestWinConButton;
        private static OptionsGUIButton KeysBehindBossesButton;
        private static OptionsGUIButton SwordProgressionButton;
        private static OptionsGUIButton StartWithSwordButton;

        private static OptionsGUIMultiSelect HintsButton;
        private static OptionsGUIMultiSelect ShowItemsButton;
        private static OptionsGUIMultiSelect ChestsMatchContentsButton;

        private static OptionsGUIMultiSelect HeirAssistModeButton;
        private static OptionsGUIMultiSelect CheaperShopItemsButton;
        private static OptionsGUIMultiSelect BonusUpgradesButton;
        private static OptionsGUIMultiSelect ItemTrackerOverlayButton;
        private static OptionsGUIMultiSelect FoolTrapButton;

        private static OptionsGUIMultiSelect ItemTrackerFileButton;
        private static OptionsGUIMultiSelect WeirdButton;

        private static OptionsGUIMultiSelect PaletteViewer;
        private static OptionsGUIMultiSelect RandomFoxColorsButton;
        private static OptionsGUIMultiSelect FurButton;
        private static OptionsGUIMultiSelect PuffButton;
        private static OptionsGUIMultiSelect DetailsButton;
        private static OptionsGUIMultiSelect TunicButton;
        private static OptionsGUIMultiSelect ScarfButton;
        private static OptionsGUIButton SaveColorPaletteButton;
        private static OptionsGUIButton LoadColorPaletteButton;
        private static OptionsGUIButton ResetColorButton;

        public static void OptionsGUI_page_root_PostfixPatch(OptionsGUI __instance) {
            foreach (OptionsGUIButton Button in GameObject.FindObjectsOfType<OptionsGUIButton>()) {
                if (Button.centerAlignedText.text == "Extra Options") {
                    Button.centerAlignedText.text = "Extra Options & Randomizer Settings";
                }
            }
        }

        public static bool OptionsGUI_page_extras_PrefixPatch(OptionsGUI __instance) {
            List<string> FurOptions = new List<string>();
            List<string> PuffOptions = new List<string>();
            List<string> DetailsOptions = new List<string>();
            List<string> TunicOptions = new List<string>();
            List<string> ScarfOptions = new List<string>();
            foreach (ColorPalette Color in ColorPalette.Fur.Values) {
                FurOptions.Add(Color.ToString());
            }
            foreach (ColorPalette Color in ColorPalette.Puff.Values) {
                PuffOptions.Add(Color.ToString());
            }
            foreach (ColorPalette Color in ColorPalette.Details.Values) {
                DetailsOptions.Add(Color.ToString());
            }
            foreach (ColorPalette Color in ColorPalette.Tunic.Values) {
                TunicOptions.Add(Color.ToString());
            }
            foreach (ColorPalette Color in ColorPalette.Scarf.Values) {
                ScarfOptions.Add(Color.ToString());
            }
            UnhollowerBaseLib.Il2CppStringArray Options = (UnhollowerBaseLib.Il2CppStringArray)new string[] { "Logic", "Hint", "Gameplay", "Fox Colors", "Misc." };
            UnhollowerBaseLib.Il2CppStringArray FoolTrapOptions = (UnhollowerBaseLib.Il2CppStringArray)new string[] { "<#FFFFFF>None", "<#4FF5D4>Normal", "<#E3D457>Double", "<#FF3333>Onslaught" };
            UnhollowerBaseLib.Il2CppStringArray Palettes = (UnhollowerBaseLib.Il2CppStringArray)new string[] { "Fur", "Puff", "Details", "Tunic", "Scarf" };
            UnhollowerBaseLib.Il2CppStringArray HexQuestOptions = (UnhollowerBaseLib.Il2CppStringArray)new string[] { "<#ffd700>20", "<#ffd700>25", "<#ffd700>30" };

            // Logic
            SettingsViewer = __instance.addMultiSelect("Randomizer Settings", Options, 0, (OptionsGUIMultiSelect.MultiSelectAction)ChangeOptionsPage);
            SettingsViewer.wrap = true;
            if (SceneLoaderPatches.SceneName == "TitleScreen") {
                GameModeButton = __instance.addToggle("Game Mode", "<#FFA300>Randomizer", "<#ffd700>Hexagon Quest", (TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST) ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleGameMode);
                KeysBehindBossesButton = __instance.addToggle("Keys Behind Bosses", "Off", "On", TunicRandomizer.Settings.KeysBehindBosses ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleKeysBehindBosses);
                SwordProgressionButton = __instance.addToggle("Sword Progression", "Off", "On", TunicRandomizer.Settings.SwordProgressionEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleSwordProgression);
                StartWithSwordButton = __instance.addToggle("Start With Sword", "Off", "On", TunicRandomizer.Settings.StartWithSwordEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleStartWithSword);
            } else {
                GameModeButton = __instance.addButton("Game Mode", SaveFile.GetString("randomizer game mode"), null);
                KeysBehindBossesButton = __instance.addButton("Keys Behind Bosses", SaveFile.GetInt("randomizer keys behind bosses") == 1 ? "<#00ff00>On" : "<#ff0000>Off", null);
                SwordProgressionButton = __instance.addButton("Sword Progression", SaveFile.GetInt("randomizer sword progression enabled") == 1 ? "<#00ff00>On" : "<#ff0000>Off", null);
                StartWithSwordButton = __instance.addButton("Start With Sword", SaveFile.GetInt("randomizer sword progression enabled") == 1 ? "<#00ff00>On" : "<#ff0000>Off", null);
                
            }
            // Hints
            HintsButton = __instance.addToggle("Hints", "Off", "On", TunicRandomizer.Settings.HintsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleHints);
            ShowItemsButton = __instance.addToggle("Show Item Pickups", "Off", "On", TunicRandomizer.Settings.ShowItemsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleShowItems);
            ChestsMatchContentsButton = __instance.addToggle("Chests Match Contents", "Off", "On", TunicRandomizer.Settings.ChestsMatchContentsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleChestsMatchContents);

            // Gameplay
            HeirAssistModeButton = __instance.addToggle("Easier Heir Fight", "Off", "On", TunicRandomizer.Settings.HeirAssistModeEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleHeirAssistMode);
            CheaperShopItemsButton = __instance.addToggle("Cheaper Shop Items", "Off", "On", TunicRandomizer.Settings.CheaperShopItemsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleCheaperShopItems);
            BonusUpgradesButton = __instance.addToggle("Bonus Upgrades", "Off", "On", TunicRandomizer.Settings.BonusStatUpgradesEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleBonusStatUpgrades);
            //ItemTrackerOverlayButton = __instance.addToggle("Item Tracker Overlay", "Off", "On", TunicRandomizer.Settings.ItemTrackerOverlayEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleTrackerOverlay);
            FoolTrapButton = __instance.addMultiSelect("Fool Trap Frequency", FoolTrapOptions, GetFoolTrapIndex(), (OptionsGUIMultiSelect.MultiSelectAction)ChangeFoolTrapFrequency);
            FoolTrapButton.wrap = true;

            // Item Tracker

            RandomFoxColorsButton = __instance.addToggle("Random Fox Colors", "Off", "On", TunicRandomizer.Settings.RandomFoxColorsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleRandomFoxPalette);
            PaletteViewer = __instance.addMultiSelect("Change Palette", Palettes, 0, (OptionsGUIMultiSelect.MultiSelectAction)ChangePalette);
            FurButton = __instance.addMultiSelect("Fur", (UnhollowerBaseLib.Il2CppStringArray)FurOptions.ToArray(), PlayerPalette.selectionIndices[0], (OptionsGUIMultiSelect.MultiSelectAction)IncrementFur);
            PuffButton = __instance.addMultiSelect("Puff", (UnhollowerBaseLib.Il2CppStringArray)PuffOptions.ToArray(), PlayerPalette.selectionIndices[1], (OptionsGUIMultiSelect.MultiSelectAction)IncrementPuff);
            DetailsButton = __instance.addMultiSelect("Details", (UnhollowerBaseLib.Il2CppStringArray)DetailsOptions.ToArray(), PlayerPalette.selectionIndices[2], (OptionsGUIMultiSelect.MultiSelectAction)IncrementDetails);
            TunicButton = __instance.addMultiSelect("Tunic", (UnhollowerBaseLib.Il2CppStringArray)TunicOptions.ToArray(), PlayerPalette.selectionIndices[3], (OptionsGUIMultiSelect.MultiSelectAction)IncrementTunic);
            ScarfButton = __instance.addMultiSelect("Scarf", (UnhollowerBaseLib.Il2CppStringArray)ScarfOptions.ToArray(), PlayerPalette.selectionIndices[4], (OptionsGUIMultiSelect.MultiSelectAction)IncrementScarf);
            SaveColorPaletteButton = __instance.addButton("Save Color Palette", (Il2CppSystem.Action)SaveColorPalette);
            LoadColorPaletteButton = __instance.addButton("Load Saved Palette", (Il2CppSystem.Action)LoadColorPalette);
            ResetColorButton = __instance.addButton("Reset Fox Colors", (Il2CppSystem.Action)ResetToDefaults);
            PaletteViewer.wrap = true;
            FurButton.wrap = true;
            PuffButton.wrap = true;
            DetailsButton.wrap = true;
            TunicButton.wrap = true;
            ScarfButton.wrap = true;

            ItemTrackerFileButton = __instance.addToggle("Item Tracker File", "Off", "On", TunicRandomizer.Settings.ItemTrackerFileEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleTrackerFile);
            WeirdButton = __instance.addToggle("???", "Off", "On", CameraController.Flip ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleWeirdMode);
            __instance.addButton("-------------------------", null);
            ToggleHintSettingsVisiblity();
            ToggleGameplaySettingsVisiblity();
            ToggleMiscSettingsVisibility();
            ToggleColorPaletteSettingsVisibility();
            return true;
        }

        public static void OptionsGUI_page_extras_PostfixPatch(OptionsGUI __instance) {
            __instance.setHeading("Extra + Randomizer");
        }

        public static void SaveSettings() {
            if (!File.Exists(TunicRandomizer.SettingsPath)) {
                File.WriteAllText(TunicRandomizer.SettingsPath, JSONWriter.ToJson(TunicRandomizer.Settings));
            } else {
                File.Delete(TunicRandomizer.SettingsPath);
                File.WriteAllText(TunicRandomizer.SettingsPath, JSONWriter.ToJson(TunicRandomizer.Settings));
            }
        }

        public static void ChangeOptionsPage(int index) {
            ToggleAllOff();
            if (index == 0) {
                ToggleLogicSettingsVisibility();
            } else if (index == 1) {
                ToggleHintSettingsVisiblity();
            } else if (index == 2) {
                ToggleGameplaySettingsVisiblity();
            } else if (index == 3) {
                ToggleColorPaletteSettingsVisibility();
            } else if (index == 4) {
                ToggleMiscSettingsVisibility();
            }

        }

        public static void ToggleAllOff() {
            GameModeButton.gameObject.active = false;
            KeysBehindBossesButton.gameObject.active = false;
            SwordProgressionButton.gameObject.active = false;
            StartWithSwordButton.gameObject.active = false;

            HintsButton.gameObject.active = false;
            ShowItemsButton.gameObject.active = false;
            ChestsMatchContentsButton.gameObject.active = false;

            HeirAssistModeButton.gameObject.active = false;
            CheaperShopItemsButton.gameObject.active = false;
            BonusUpgradesButton.gameObject.active = false;
            //ItemTrackerOverlayButton.gameObject.active = false;
            FoolTrapButton.gameObject.active = false;

            ItemTrackerFileButton.gameObject.active = false;
            WeirdButton.gameObject.active = false;

            PaletteViewer.gameObject.active = false;
            RandomFoxColorsButton.gameObject.active = false;
            FurButton.gameObject.active = false;
            PuffButton.gameObject.active = false;
            DetailsButton.gameObject.active = false;
            TunicButton.gameObject.active = false;
            ScarfButton.gameObject.active = false;
            ResetColorButton.gameObject.active = false;

            SaveColorPaletteButton.gameObject.active = false;
            LoadColorPaletteButton.gameObject.active = false;
        }
        public static void ToggleLogicSettingsVisibility() {
            GameModeButton.gameObject.active = !GameModeButton.gameObject.active;
            KeysBehindBossesButton.gameObject.active = !KeysBehindBossesButton.gameObject.active;
            SwordProgressionButton.gameObject.active = !SwordProgressionButton.gameObject.active;
            StartWithSwordButton.gameObject.active = !StartWithSwordButton.gameObject.active;
        }

        public static void ToggleHintSettingsVisiblity() {
            HintsButton.gameObject.active = !HintsButton.gameObject.active;
            ShowItemsButton.gameObject.active = !ShowItemsButton.gameObject.active;
            ChestsMatchContentsButton.gameObject.active = !ChestsMatchContentsButton.gameObject.active;
        }

        public static void ToggleGameplaySettingsVisiblity() {
            HeirAssistModeButton.gameObject.active = !HeirAssistModeButton.gameObject.active;
            CheaperShopItemsButton.gameObject.active = !CheaperShopItemsButton.gameObject.active;
            BonusUpgradesButton.gameObject.active = !BonusUpgradesButton.gameObject.active;
            //ItemTrackerOverlayButton.gameObject.active = !ItemTrackerOverlayButton.gameObject.active;
            FoolTrapButton.gameObject.active = !FoolTrapButton.gameObject.active;
        }

        public static void ToggleMiscSettingsVisibility() {
            ItemTrackerFileButton.gameObject.active = !ItemTrackerFileButton.gameObject.active;
            WeirdButton.gameObject.active = !WeirdButton.gameObject.active;
        }

        public static void ToggleColorPaletteSettingsVisibility() {
            PaletteViewer.gameObject.active = !PaletteViewer.gameObject.active;
            RandomFoxColorsButton.gameObject.active = !RandomFoxColorsButton.gameObject.active;
            ChangePalette(PaletteViewer.selectedIndex);
/*            FurButton.gameObject.active = !FurButton.gameObject.active;
            PuffButton.gameObject.active = !PuffButton.gameObject.active;
            DetailsButton.gameObject.active = !DetailsButton.gameObject.active;
            TunicButton.gameObject.active = !TunicButton.gameObject.active;
            ScarfButton.gameObject.active = !ScarfButton.gameObject.active;
            ResetColorButton.gameObject.active = !ResetColorButton.gameObject.active;*/
            
            SaveColorPaletteButton.gameObject.active = !SaveColorPaletteButton.gameObject.active;
            LoadColorPaletteButton.gameObject.active = !LoadColorPaletteButton.gameObject.active;

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
        public static void ToggleHints(int index) {
            TunicRandomizer.Settings.HintsEnabled = !TunicRandomizer.Settings.HintsEnabled;
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

        // Item Tracker Settings

        public static void ToggleTrackerOverlay(int index) { 
            TunicRandomizer.Settings.ItemTrackerOverlayEnabled = !TunicRandomizer.Settings.ItemTrackerOverlayEnabled;
            SaveSettings();
        }

        public static void ToggleTrackerFile(int index) {
            TunicRandomizer.Settings.ItemTrackerFileEnabled = !TunicRandomizer.Settings.ItemTrackerFileEnabled;
            if (TunicRandomizer.Settings.ItemTrackerFileEnabled) {
                ItemTracker.SaveTrackerFile();
            } else {
                File.Delete(TunicRandomizer.ItemTrackerPath);
            }

            SaveSettings();
        }

        public static void ToggleWeirdMode(int index) {
            CameraController.Flip = !CameraController.Flip;
        }

        public static void ToggleRandomFoxPalette(int index) {
            TunicRandomizer.Settings.RandomFoxColorsEnabled = !TunicRandomizer.Settings.RandomFoxColorsEnabled;
            SaveSettings();
        }

        public static void ChangePalette(int index) {
            FurButton.gameObject.active = false;
            PuffButton.gameObject.active = false;
            DetailsButton.gameObject.active = false;
            TunicButton.gameObject.active = false;
            ScarfButton.gameObject.active = false;
            if (index == 0) {
                FurButton.gameObject.active = true;
            } else if (index == 1) {
                PuffButton.gameObject.active = true;
            } else if (index == 2) {
                DetailsButton.gameObject.active = true;
            } else if (index == 3) {
                TunicButton.gameObject.active = true;
            } else if (index == 4) {
                ScarfButton.gameObject.active = true;
            }
        }

        public static void SaveColorPalette() {
            TunicRandomizer.Settings.SavedColorPalette = PlayerPalette.selectionIndices;
            SaveSettings();
        }

        public static void LoadColorPalette() {
            PlayerPalette.selectionIndices = TunicRandomizer.Settings.SavedColorPalette;
            for (int i = 0; i < PlayerPalette.selectionIndices.Count; i++) {
                PlayerPalette.ChangeColourByDelta(i, 0);
            }
            FurButton.selectedIndex = PlayerPalette.selectionIndices[0];
            FurButton.secondaryText.text = ColorPalette.Fur[PlayerPalette.selectionIndices[0]].ToString();
            PuffButton.selectedIndex = PlayerPalette.selectionIndices[1];
            PuffButton.secondaryText.text = ColorPalette.Puff[PlayerPalette.selectionIndices[1]].ToString();
            DetailsButton.selectedIndex = PlayerPalette.selectionIndices[2];
            DetailsButton.secondaryText.text = ColorPalette.Details[PlayerPalette.selectionIndices[2]].ToString();
            TunicButton.selectedIndex = PlayerPalette.selectionIndices[3];
            TunicButton.secondaryText.text = ColorPalette.Tunic[PlayerPalette.selectionIndices[3]].ToString();
            ScarfButton.selectedIndex = PlayerPalette.selectionIndices[4];
            ScarfButton.secondaryText.text = ColorPalette.Scarf[PlayerPalette.selectionIndices[4]].ToString();
            SceneLoaderPatches.UpdateTrackerSceneInfo();
        }

        public static void IncrementFur(int index) {
            PlayerPalette.selectionIndices[0] = index;
            PlayerPalette.ChangeColourByDelta(0, 0);
            SceneLoaderPatches.UpdateTrackerSceneInfo();
        }

        public static void IncrementPuff(int index) {
            PlayerPalette.selectionIndices[1] = index;
            PlayerPalette.ChangeColourByDelta(0, 0);
            SceneLoaderPatches.UpdateTrackerSceneInfo();
        }

        public static void IncrementDetails(int index) {
            PlayerPalette.selectionIndices[2] = index;
            PlayerPalette.ChangeColourByDelta(0, 0);
            SceneLoaderPatches.UpdateTrackerSceneInfo();
        }

        public static void IncrementTunic(int index) {
            PlayerPalette.selectionIndices[3] = index;
            PlayerPalette.ChangeColourByDelta(0, 0);
            SceneLoaderPatches.UpdateTrackerSceneInfo();
        }

        public static void IncrementScarf(int index) {
            PlayerPalette.selectionIndices[4] = index;
            PlayerPalette.ChangeColourByDelta(0, 0);
            SceneLoaderPatches.UpdateTrackerSceneInfo();
        }



        public static void ResetToDefaults() {
            for(int i = 0; i < 5; i++) {
                PlayerPalette.ResetToDefaults(i);
            }

            FurButton.selectedIndex = 0;
            FurButton.secondaryText.text = ColorPalette.Fur[0].ToString();
            PuffButton.selectedIndex = 0;
            PuffButton.secondaryText.text = ColorPalette.Puff[0].ToString();
            DetailsButton.selectedIndex = 0;
            DetailsButton.secondaryText.text = ColorPalette.Details[0].ToString();
            TunicButton.selectedIndex = 0;
            TunicButton.secondaryText.text = ColorPalette.Tunic[0].ToString();
            ScarfButton.selectedIndex = 0;
            ScarfButton.secondaryText.text = ColorPalette.Scarf[0].ToString();
        }
    }
}
