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

        private static OptionsGUIMultiSelect FurButton;
        private static OptionsGUIMultiSelect PuffButton;
        private static OptionsGUIMultiSelect DetailsButton;
        private static OptionsGUIMultiSelect TunicButton;
        private static OptionsGUIMultiSelect ScarfButton;
        private static OptionsGUIButton ResetColorButton;
        private static OptionsGUIMultiSelect.MultiSelectAction ToggleHintsAction = (OptionsGUIMultiSelect.MultiSelectAction) ToggleHints;
        private static OptionsGUIMultiSelect.MultiSelectAction ToggleHeirAssistAction = (OptionsGUIMultiSelect.MultiSelectAction) ToggleHeirAssistMode;
        private static OptionsGUIMultiSelect.MultiSelectAction ToggleFoolTrapAction = (OptionsGUIMultiSelect.MultiSelectAction) ChangeFoolTrapFrequency;
        private static OptionsGUIMultiSelect.MultiSelectAction ToggleCheaperShopAction = (OptionsGUIMultiSelect.MultiSelectAction) ToggleCheaperShopItems;
        private static OptionsGUIMultiSelect.MultiSelectAction ToggleDisplayShopAction = (OptionsGUIMultiSelect.MultiSelectAction) ToggleShopItemDisplay;
        private static OptionsGUIMultiSelect.MultiSelectAction ToggleFreeSwordAction = (OptionsGUIMultiSelect.MultiSelectAction) ToggleFreeSword;
        private static OptionsGUIMultiSelect.MultiSelectAction ToggleFoxColorsAction = (OptionsGUIMultiSelect.MultiSelectAction) ToggleRandomFoxPalette;

        public static void OptionsGUI_page_root_PostfixPatch(OptionsGUI __instance) {
            foreach (OptionsGUIButton Button in GameObject.FindObjectsOfType<OptionsGUIButton>()) {
                if (Button.centerAlignedText.text == "Extra Options") {
                    Button.centerAlignedText.text = "Extra Options & Randomizer Settings";
                }
            }
        }

        public static void OptionsGUI_popPage_PostfixPatch(OptionsGUI __instance) {
            if (__instance.pageStack.Count == 0) {
                __instance.pushDefault();
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
            __instance.addToggle("Hints", "Off", "On", TunicRandomizer.Settings.HintsEnabled ? 1 : 0, ToggleHintsAction);
            __instance.addToggle("Easier Heir Fight", "Off", "On", TunicRandomizer.Settings.HeirAssistModeEnabled ? 1 : 0, ToggleHeirAssistAction);
            UnhollowerBaseLib.Il2CppStringArray FoolTrapOptions = (UnhollowerBaseLib.Il2CppStringArray)new string[] { "<#FFFFFF>None", "<#4FF5D4>Normal", "<#E3D457>Double", "<#FF3333>Onslaught" };
            OptionsGUIMultiSelect ms = __instance.addMultiSelect("Fool Trap Frequency", FoolTrapOptions, GetFoolTrapIndex(), ToggleFoolTrapAction);
            ms.wrap = true;
            __instance.addToggle("Show Shop Items", "Off", "On", TunicRandomizer.Settings.ShowShopItemsEnabled ? 1: 0, ToggleDisplayShopAction);
            __instance.addToggle("Cheaper Shop Items", "Off", "On", TunicRandomizer.Settings.CheaperShopItemsEnabled ? 1 : 0, ToggleCheaperShopAction);
            __instance.addToggle("Start With Sword", "Off", "On", TunicRandomizer.Settings.StartWithSwordEnabled ? 1 : 0, ToggleFreeSwordAction);
            __instance.addToggle("Random Fox Colors", "Off", "On", TunicRandomizer.Settings.RandomFoxColorsEnabled ? 1 : 0, ToggleFoxColorsAction);

            if (SceneLoaderPatches.SceneName != "TitleScreen") {
                __instance.addButton("Change Fox Colors", (Il2CppSystem.Action)LoadColorPaletteSettings);
                FurButton = __instance.addMultiSelect("Fur", (UnhollowerBaseLib.Il2CppStringArray)FurOptions.ToArray(), PlayerPalette.selectionIndices[0], (OptionsGUIMultiSelect.MultiSelectAction)IncrementFur);
                PuffButton = __instance.addMultiSelect("Fur", (UnhollowerBaseLib.Il2CppStringArray)PuffOptions.ToArray(), PlayerPalette.selectionIndices[1], (OptionsGUIMultiSelect.MultiSelectAction)IncrementPuff);
                DetailsButton = __instance.addMultiSelect("Fur", (UnhollowerBaseLib.Il2CppStringArray)DetailsOptions.ToArray(), PlayerPalette.selectionIndices[2], (OptionsGUIMultiSelect.MultiSelectAction)IncrementDetails);
                TunicButton = __instance.addMultiSelect("Fur", (UnhollowerBaseLib.Il2CppStringArray)TunicOptions.ToArray(), PlayerPalette.selectionIndices[3], (OptionsGUIMultiSelect.MultiSelectAction)IncrementTunic);
                ScarfButton = __instance.addMultiSelect("Fur", (UnhollowerBaseLib.Il2CppStringArray)ScarfOptions.ToArray(), PlayerPalette.selectionIndices[4], (OptionsGUIMultiSelect.MultiSelectAction)IncrementScarf);
                ResetColorButton = __instance.addButton("Reset Fox Colors", (Il2CppSystem.Action) ResetToDefaults);
                FurButton.wrap = true;
                PuffButton.wrap = true;
                DetailsButton.wrap = true;
                TunicButton.wrap = true;
                ScarfButton.wrap = true;
                FurButton.gameObject.active = false;
                PuffButton.gameObject.active = false;
                DetailsButton.gameObject.active = false;
                TunicButton.gameObject.active = false;
                ScarfButton.gameObject.active = false;
                ResetColorButton.gameObject.active = false;
            }

            return true;
        }

        public static void OptionsGUI_page_extras_PostfixPatch(OptionsGUI __instance) {
            __instance.setHeading("Extra + Randomizer");
        }

        public static void ToggleHints(int index) {
            TunicRandomizer.Settings.HintsEnabled = !TunicRandomizer.Settings.HintsEnabled;
            SaveSettings();
        }

        public static void ToggleHeirAssistMode(int index) {
            TunicRandomizer.Settings.HeirAssistModeEnabled = !TunicRandomizer.Settings.HeirAssistModeEnabled;
            SaveSettings();
        }

        public static void ToggleShopItemDisplay(int index) { 
            TunicRandomizer.Settings.ShowShopItemsEnabled = !TunicRandomizer.Settings.ShowShopItemsEnabled;
            SaveSettings();
        }

        public static void ToggleCheaperShopItems(int index) { 
            TunicRandomizer.Settings.CheaperShopItemsEnabled = !TunicRandomizer.Settings.CheaperShopItemsEnabled;
            SaveSettings();
        }

        public static void ToggleFreeSword(int index) {
            TunicRandomizer.Settings.StartWithSwordEnabled = !TunicRandomizer.Settings.StartWithSwordEnabled;
            Item Sword = Inventory.GetItemByName("Sword");
            if (TunicRandomizer.Settings.StartWithSwordEnabled && Sword.Quantity == 0) {
                SaveFile.SetInt("randomizer free sword given", 1);
                Sword.Quantity = 1;
            } else if (!TunicRandomizer.Settings.StartWithSwordEnabled && SaveFile.GetInt("randomizer free sword given") == 1) {
                SaveFile.SetInt("randomizer free sword given", 0);
                Sword.Quantity--;
            }
            SaveSettings();
        }

        public static void ToggleRandomFoxPalette(int index) {
            TunicRandomizer.Settings.RandomFoxColorsEnabled = !TunicRandomizer.Settings.RandomFoxColorsEnabled;
            SaveSettings();
        }

        public static int GetFoolTrapIndex() {
   
            return (int)TunicRandomizer.Settings.FoolTrapIntensity;
        }

        public static void ChangeFoolTrapFrequency(int index) {

            TunicRandomizer.Settings.FoolTrapIntensity = (RandomizerSettings.FoolTrapOption) index;
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

            FurButton.gameObject.active = !FurButton.gameObject.active;
            PuffButton.gameObject.active = !PuffButton.gameObject.active;
            DetailsButton.gameObject.active = !DetailsButton.gameObject.active;
            TunicButton.gameObject.active = !TunicButton.gameObject.active;
            ScarfButton.gameObject.active = !ScarfButton.gameObject.active;
            ResetColorButton.gameObject.active = !ResetColorButton.gameObject.active;

        }

        public static void IncrementFur(int index) {
            PlayerPalette.selectionIndices[0] = index;
            PlayerPalette.ChangeColourByDelta(0, 0);
        }

        public static void IncrementPuff(int index) {
            PlayerPalette.selectionIndices[1] = index;
            PlayerPalette.ChangeColourByDelta(0, 0);
        }

        public static void IncrementDetails(int index) {
            PlayerPalette.selectionIndices[2] = index;
            PlayerPalette.ChangeColourByDelta(0, 0);
        }

        public static void IncrementTunic(int index) {
            PlayerPalette.selectionIndices[3] = index;
            PlayerPalette.ChangeColourByDelta(0, 0);
        }

        public static void IncrementScarf(int index) {
            PlayerPalette.selectionIndices[4] = index;
            PlayerPalette.ChangeColourByDelta(0, 0);
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
