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
            TimerOverlayButton = OptionsGUI.addButton("Timer Overlay", TunicRandomizer.Settings.TimerOverlayEnabled ? "<#33FF33>On" : "<#FF3333>Off", (Il2CppSystem.Action)ToggleTimerOverlay);
            FoxColorOptionButton = OptionsGUI.addButton("Random Fox Colors", TunicRandomizer.Settings.RandomFoxColorsEnabled ? "<#33FF33>On" : "<#FF3333>Off", (Il2CppSystem.Action)ToggleRandomFoxPalette);
            if (SceneLoaderPatches.SceneName != "TitleScreen") {
                OptionsGUI.addButton("Change Fox Colors", (Il2CppSystem.Action)LoadColorPaletteSettings);
            }
            OptionsGUI.addButton_CancelSFX("Return", true, (Il2CppSystem.Action)GameObject.FindObjectOfType<OptionsGUI>().pushDefault);
            OptionsGUI.setHeading("Randomizer v0.0.4");
        }

        public static void ToggleTimerOverlay() {
            TunicRandomizer.Settings.TimerOverlayEnabled = !TunicRandomizer.Settings.TimerOverlayEnabled;
            SpeedrunTimerDisplay.Visible = TunicRandomizer.Settings.TimerOverlayEnabled;
            SpeedrunTimerDisplay.instance.sceneText.text = "";
            SpeedrunTimerDisplay.instance.timerText.transform.position = new Vector3(-454.1f, 245.4f, -197.0f);
            SpeedrunTimerDisplay.instance.timerText.fontSize = 64;
            TimeSpan timespan = TimeSpan.FromSeconds(SpeedrunData.inGameTime);
            SpeedrunTimerDisplay.instance.timerText.text = timespan.ToString("hh':'mm':'ss'.'ff");
            TimerOverlayButton.secondaryText.text = TunicRandomizer.Settings.TimerOverlayEnabled ? "<#33FF33>On" : "<#FF3333>Off";
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
            OptionsGUIButton TopButton = OptionsGUI.addButton("", null);
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
            //GameObject.FindObjectsOfType<OptionsGUIButton>().Where(obj => obj.leftAlignedText.text == "Fur").ToList()[0].secondaryText.text = FurColor;
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
