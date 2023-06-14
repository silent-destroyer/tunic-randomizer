using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace TunicRandomizer {
    public class QuickSettings : MonoBehaviour {
        private static ManualLogSource Logger = TunicRandomizer.Logger;

        public static int CustomSeed = 0;
        public static Font OdinRounded;
        private void OnGUI() {
            Resources.FindObjectsOfTypeAll<Font>().Where(Font => Font.name == "Odin Rounded").ToList();
            GUI.skin.font = OdinRounded == null ? GUI.skin.font : OdinRounded;
            if (SceneLoaderPatches.SceneName == "TitleScreen") {
                Cursor.visible = true;
                GUI.Window(101, new Rect(20f, 150f, 400f, 345f), new Action<int>(QuickSettingsWindow), "Quick Settings");
            }
        }

        private static void QuickSettingsWindow(int windowID) {
            GUI.skin.label.fontSize = 25;
            //GUI.skin.toggle.fontSize = 25;
            //GUI.skin.toggle.alignment = TextAnchor.UpperLeft;
            GUI.skin.toggle.fontSize = 20;
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.skin.label.clipping = TextClipping.Overflow;
            GUI.color = Color.white;
            GUI.DragWindow(new Rect(500f, 50f, 500f, 30f));
            GUI.Label(new Rect(10f, 20f, 200f, 30f), "Game Mode");
            bool ToggleRandomizer = GUI.Toggle(new Rect(10f, 60f, 125f, 30f), TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.RANDOMIZER, "Randomizer");
            if (ToggleRandomizer) {
                TunicRandomizer.Settings.GameMode = RandomizerSettings.GameModes.RANDOMIZER;
            }
            bool ToggleHexagonQuest = GUI.Toggle(new Rect(140f, 60f, 175f, 30f), TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST, "Hexagon Quest");

            if (ToggleHexagonQuest) {
                TunicRandomizer.Settings.GameMode = RandomizerSettings.GameModes.HEXAGONQUEST;
            }
            GUI.Label(new Rect(10f, 95f, 200f, 30f), "Logic Settings");
            bool TopggleBossKeys = GUI.Toggle(new Rect(10f, 140f, 180f, 30f), TunicRandomizer.Settings.KeysBehindBosses, "Keys Behind Bosses");
            TunicRandomizer.Settings.KeysBehindBosses = TopggleBossKeys;
            bool ToggleSwordProgression = GUI.Toggle(new Rect(210f, 140f, 180f, 30f), TunicRandomizer.Settings.SwordProgressionEnabled, "Sword Progression");
            TunicRandomizer.Settings.SwordProgressionEnabled = ToggleSwordProgression;
            bool ToggleSwordStart = GUI.Toggle(new Rect(10f, 180f, 175f, 30f), TunicRandomizer.Settings.StartWithSwordEnabled, "Start With Sword");
            TunicRandomizer.Settings.StartWithSwordEnabled = ToggleSwordStart;
            bool ToggleAbilityShuffle = GUI.Toggle(new Rect(210f, 180f, 175f, 30f), TunicRandomizer.Settings.ShuffleAbilities, "Shuffle Abilities");
            TunicRandomizer.Settings.ShuffleAbilities = ToggleAbilityShuffle;
            GUI.skin.button.fontSize = 20;
            GUI.Label(new Rect(10f, 220f, 300f, 30f), $"Custom Seed: {(CustomSeed == 0 ? "Not Set" : CustomSeed.ToString())}");

            bool GenerateSeed = GUI.Button(new Rect(10f, 260f, 185f, 30f), "Generate Seed");
            if(GenerateSeed) {
                CustomSeed = new System.Random().Next();
            }
            bool CopySettings = GUI.Button(new Rect(10f, 300f, 185f, 30f), "Copy Settings");
            if(CopySettings) {
                CopyQuickSettings();
            }
            bool PasteSettings = GUI.Button(new Rect(205f, 300f, 185f, 30f), "Paste Settings");
            if(PasteSettings) {
                PasteQuickSettings();
            }

            if (CustomSeed != 0) {
                bool ClearSeed = GUI.Button(new Rect(205f, 260f, 185f, 30f), "Clear Seed");
                if (ClearSeed) {
                    CustomSeed = 0;
                }
            }
        }

        public static void CopyQuickSettings() {
            string SettingsString = $"{CustomSeed},";
            SettingsString += $"{(int)TunicRandomizer.Settings.GameMode},";
            SettingsString += $"{(TunicRandomizer.Settings.SwordProgressionEnabled ? "1" : "0")},";
            SettingsString += $"{(TunicRandomizer.Settings.KeysBehindBosses ? "1" : "0")},";
            SettingsString += $"{(TunicRandomizer.Settings.StartWithSwordEnabled ? "1" : "0")},";
            SettingsString += $"{(TunicRandomizer.Settings.ShuffleAbilities ? "1" : "0")}";
            GUIUtility.systemCopyBuffer = SettingsString;
        }

        public static void CopyQuickSettingsInGame() {
            string SettingsString = $"{SaveFile.GetInt("seed")},";
            SettingsString += $"{(int)TunicRandomizer.Settings.GameMode},";
            SettingsString += $"{SaveFile.GetInt("randomizer sword progression enabled")},";
            SettingsString += $"{SaveFile.GetInt("randomizer keys behind bosses")},";
            SettingsString += $"{SaveFile.GetInt("randomizer started with sword")},";
            SettingsString += $"{SaveFile.GetInt("randomizer shuffled abilities")}";
            GUIUtility.systemCopyBuffer = SettingsString;
        }

        public static void PasteQuickSettings() {
            try {
                string SettingsString = GUIUtility.systemCopyBuffer;
                string[] SplitSettings = SettingsString.Split(',');
                if (SplitSettings.Count() == 6) {
                    CustomSeed = int.Parse((string)SplitSettings[0]);
                    TunicRandomizer.Settings.GameMode = (RandomizerSettings.GameModes)int.Parse(SplitSettings[1]);
                    TunicRandomizer.Settings.SwordProgressionEnabled = SplitSettings[2] == "1";
                    TunicRandomizer.Settings.KeysBehindBosses = SplitSettings[3] == "1";
                    TunicRandomizer.Settings.StartWithSwordEnabled = SplitSettings[4] == "1";
                    TunicRandomizer.Settings.ShuffleAbilities = SplitSettings[5] == "1";
                }
            } catch (Exception e) {
                Logger.LogError("Error parsing quick settings string!");
            }

        }
    }
}
