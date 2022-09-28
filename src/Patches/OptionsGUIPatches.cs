using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TinyJson;
using UnityEngine;

namespace TunicRandomizer {
    public class OptionsGUIPatches {

        public static OptionsGUIButton HintsButton;
        public static OptionsGUIButton FoxColorOptionButton;
        public static OptionsGUIButton TimerOverlayButton;
        public static OptionsGUIButton HeirAssistModeButton;
        public static OptionsGUIButton FoolTrapSettingButton;
        public static OptionsGUIButton FurButton;
        public static OptionsGUIButton PuffButton;
        public static OptionsGUIButton DetailsButton;
        public static OptionsGUIButton TunicButton;
        public static OptionsGUIButton ScarfButton;

        public static void OptionsGUI_page_root_PostfixPatch(OptionsGUI __instance) {
            __instance.addButton("Randomizer Settings", (Il2CppSystem.Action)LoadRandomizerSettings);
        }

        public static void OptionsGUI_popPage_PostfixPatch(OptionsGUI __instance) {
            if (__instance.pageStack.Count == 0) {
                __instance.pushDefault();
            }
        }

        public static void LoadRandomizerSettings() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.clearPage();
            HintsButton = OptionsGUI.addButton("Hints", TunicRandomizer.Settings.HintsEnabled ? "<#33FF33>On" : "<#FF3333>Off", (Il2CppSystem.Action)ToggleHints);
            HeirAssistModeButton = OptionsGUI.addButton("Easier Heir Fight", TunicRandomizer.Settings.HeirAssistModeEnabled ? "<#33FF33>On" : "<#FF3333>Off", (Il2CppSystem.Action)ToggleHeirAssistMode);
            FoolTrapSettingButton = OptionsGUI.addButton("Fool Trap Frequency", GetFoolTrapString(), (Il2CppSystem.Action)ChangeFoolTrapFrequency);
            FoxColorOptionButton = OptionsGUI.addButton("Random Fox Colors", TunicRandomizer.Settings.RandomFoxColorsEnabled ? "<#33FF33>On" : "<#FF3333>Off", (Il2CppSystem.Action)ToggleRandomFoxPalette);
            if (SceneLoaderPatches.SceneName != "TitleScreen") {
                OptionsGUI.addButton("Change Fox Colors", (Il2CppSystem.Action)LoadColorPaletteSettings);
            }
            OptionsGUI.addButton_CancelSFX("Return", true, (Il2CppSystem.Action)GameObject.FindObjectOfType<OptionsGUI>().pushDefault);
            OptionsGUI.setHeading("Randomizer");
        }

        public static string GetFoolTrapString() {
            if (TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.NONE) {
                return "<#FFFFFF>None";
            } else if (TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.NORMAL) {
                return "<#4FF5D4>Normal";
            } else if (TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.DOUBLE) {
                return "<#E3D457>Double";
            } else if (TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.ONSLAUGHT) {
                return "<#FF3333>Onslaught";
            }
            return "";
        }

        public static void ChangeFoolTrapFrequency() {

            if (TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.NONE) {
                TunicRandomizer.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.NORMAL;
                FoolTrapSettingButton.secondaryText.text = "<#4FF5D4>Normal";
            } else if (TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.NORMAL) {
                TunicRandomizer.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.DOUBLE;
                FoolTrapSettingButton.secondaryText.text = "<#E3D457>Double";
            } else if (TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.DOUBLE) {
                TunicRandomizer.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.ONSLAUGHT;
                FoolTrapSettingButton.secondaryText.text = "<#FF3333>Onslaught";
            } else if (TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.ONSLAUGHT) {
                TunicRandomizer.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.NONE;
                FoolTrapSettingButton.secondaryText.text = "<#FFFFFF>None";
            }
            SaveSettings();
        }

        public static void ToggleHeirAssistMode() {
            TunicRandomizer.Settings.HeirAssistModeEnabled = !TunicRandomizer.Settings.HeirAssistModeEnabled;
            HeirAssistModeButton.secondaryText.text = TunicRandomizer.Settings.HeirAssistModeEnabled ? "<#33FF33>On" : "<#FF3333>Off";
            SaveSettings();
        }

        public static void ToggleRandomFoxPalette() {
            TunicRandomizer.Settings.RandomFoxColorsEnabled = !TunicRandomizer.Settings.RandomFoxColorsEnabled;
            FoxColorOptionButton.secondaryText.text = TunicRandomizer.Settings.RandomFoxColorsEnabled ? "<#33FF33>On" : "<#FF3333>Off";
            SaveSettings();
        }

        public static void ToggleHints() {
            TunicRandomizer.Settings.HintsEnabled = !TunicRandomizer.Settings.HintsEnabled;
            HintsButton.secondaryText.text = TunicRandomizer.Settings.HintsEnabled ? "<#33FF33>On" : "<#FF3333>Off";
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

        public static void LoadColorPaletteSettings() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.clearPage();
            OptionsGUIButton TopButton = OptionsGUI.addButton("  ", null);
            TopButton.button.enabled = false;
            FurButton = OptionsGUI.addButton("Fur", ColorPalette.Fur[PlayerPalette.selectionIndices[0]].ToString(), (Il2CppSystem.Action)IncrementFurColor);
            PuffButton = OptionsGUI.addButton("Puff", PlayerPalette.selectionIndices[1] == 0 ? ColorPalette.getDefaultPuffColor() : ColorPalette.Puff[PlayerPalette.selectionIndices[1]].ToString(), (Il2CppSystem.Action)IncrementPuffColor);
            DetailsButton = OptionsGUI.addButton("Details", ColorPalette.Details[PlayerPalette.selectionIndices[2]].ToString(), (Il2CppSystem.Action)IncrementDetailsColor);
            TunicButton = OptionsGUI.addButton("Tunic", ColorPalette.Tunic[PlayerPalette.selectionIndices[3]].ToString(), (Il2CppSystem.Action)IncrementTunicColor);
            ScarfButton = OptionsGUI.addButton("Scarf", ColorPalette.Scarf[PlayerPalette.selectionIndices[4]].ToString(), (Il2CppSystem.Action)IncrementScarfColor);
            OptionsGUI.addButton("Reset to Default", true, (Il2CppSystem.Action)ResetToDefaults);
            OptionsGUI.addButton_CancelSFX("Return", (Il2CppSystem.Action)LoadRandomizerSettings);
            OptionsGUI.setHeading("Fox Palette");
        }

        public static void IncrementFurColor() {
            int FurIndex = PlayerPalette.ChangeColourByDelta(0, 1);
            string FurColor = ColorPalette.Fur[FurIndex].ToString();
            FurButton.secondaryText.text = FurColor;
            if (PlayerPalette.selectionIndices[1] == 0) {
                PuffButton.secondaryText.text = ColorPalette.getDefaultPuffColor();
            }
        }

        public static void IncrementPuffColor() {
            int PuffIndex = PlayerPalette.ChangeColourByDelta(1, 1);
            string PuffColor = PuffIndex == 0 ? ColorPalette.getDefaultPuffColor() : ColorPalette.Puff[PuffIndex].ToString();
            PuffButton.secondaryText.text = PuffColor;
        }

        public static void IncrementDetailsColor() {
            int DetailsIndex = PlayerPalette.ChangeColourByDelta(2, 1);
            string DetailsColor = ColorPalette.Details[DetailsIndex].ToString();
            DetailsButton.secondaryText.text = DetailsColor;
        }

        public static void IncrementTunicColor() {
            int TunicIndex = PlayerPalette.ChangeColourByDelta(3, 1);
            string TunicColor = ColorPalette.Tunic[TunicIndex].ToString();
            TunicButton.secondaryText.text = TunicColor;
        }

        public static void IncrementScarfColor() {
            int ScarfIndex = PlayerPalette.ChangeColourByDelta(4, 1);
            string ScarfColor = ColorPalette.Scarf[ScarfIndex].ToString();
            ScarfButton.secondaryText.text = ScarfColor;
        }

        public static void ResetToDefaults() {
            for(int i = 0; i < 5; i++) {
                PlayerPalette.ResetToDefaults(i);
            }

            FurButton.secondaryText.text = ColorPalette.Fur[0].ToString();
            PuffButton.secondaryText.text = ColorPalette.getDefaultPuffColor();
            DetailsButton.secondaryText.text = ColorPalette.Details[0].ToString();
            TunicButton.secondaryText.text = ColorPalette.Tunic[0].ToString();
            ScarfButton.secondaryText.text = ColorPalette.Scarf[0].ToString();
        }
    }
}
