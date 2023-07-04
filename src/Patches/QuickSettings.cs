using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            if (SceneLoaderPatches.SceneName == "TitleScreen" && OdinRounded != null && GameObject.FindObjectOfType<TitleScreen>() != null) {
                GUI.skin.font = OdinRounded;
                Cursor.visible = true;
                GUI.Window(101, new Rect(20f, 150f, 430f, 345f), new Action<int>(QuickSettingsWindow), "Quick Settings");
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
            bool ToggleSwordProgression = GUI.Toggle(new Rect(240f, 140f, 180f, 30f), TunicRandomizer.Settings.SwordProgressionEnabled, "Sword Progression");
            TunicRandomizer.Settings.SwordProgressionEnabled = ToggleSwordProgression;
            bool ToggleSwordStart = GUI.Toggle(new Rect(10f, 180f, 175f, 30f), TunicRandomizer.Settings.StartWithSwordEnabled, "Start With Sword");
            TunicRandomizer.Settings.StartWithSwordEnabled = ToggleSwordStart;
            bool ToggleAbilityShuffle = GUI.Toggle(new Rect(240f, 180f, 175f, 30f), TunicRandomizer.Settings.ShuffleAbilities, "Shuffle Abilities");
            TunicRandomizer.Settings.ShuffleAbilities = ToggleAbilityShuffle;
            GUI.skin.button.fontSize = 20;
            GUI.Label(new Rect(10f, 220f, 300f, 30f), $"Custom Seed: {(CustomSeed == 0 ? "Not Set" : CustomSeed.ToString())}");

            bool GenerateSeed = GUI.Button(new Rect(10f, 260f, 200f, 30f), "Generate Seed");
            if(GenerateSeed) {
                CustomSeed = new System.Random().Next();
            }
            bool PasteSeed = GUI.Button(new Rect(220f, 260f, 200f, 30f), "Paste Seed");
            if (PasteSeed) {
                try {
                    CustomSeed = int.Parse(GUIUtility.systemCopyBuffer, CultureInfo.InvariantCulture);
                } catch (System.Exception e) {

                }
            }
            bool CopySettings = GUI.Button(new Rect(10f, 300f, 200f, 30f), "Copy Seed + Settings");
            if(CopySettings) {
                CopyQuickSettings();
            }
            bool PasteSettings = GUI.Button(new Rect(220f, 300f, 200f, 30f), "Paste Seed + Settings");
            if(PasteSettings) {
                PasteQuickSettings();
            }

            if (CustomSeed != 0) {
                bool ClearSeed = GUI.Button(new Rect(300f, 220f, 110f, 30f), "Clear");
                if (ClearSeed) {
                    CustomSeed = 0;
                }
            }
        }

        public static void CopyQuickSettings() {
            List<string> Settings = new List<string>() { CustomSeed.ToString(), Enum.GetName(typeof(RandomizerSettings.GameModes), TunicRandomizer.Settings.GameMode).ToLower() };

            if (TunicRandomizer.Settings.KeysBehindBosses) {
                Settings.Add("keys_behind_bosses");
            }
            if (TunicRandomizer.Settings.SwordProgressionEnabled) {
                Settings.Add("sword_progression");
            }
            if (TunicRandomizer.Settings.StartWithSwordEnabled) {
                Settings.Add("start_with_sword");
            }
            if (TunicRandomizer.Settings.ShuffleAbilities) {
                Settings.Add("shuffle_abilities");
            }
            GUIUtility.systemCopyBuffer = string.Join(",", Settings.ToArray());
        }

        public static void CopyQuickSettingsInGame() {
            List<string> Settings = new List<string>() { SaveFile.GetInt("seed").ToString(), SaveFile.GetString("randomizer game mode").ToLower() };
            if (SaveFile.GetInt("randomizer keys behind bosses") == 1) {
                Settings.Add("keys_behind_bosses");
            }
            if (SaveFile.GetInt("randomizer sword progression enabled") == 1) {
                Settings.Add("sword_progression");
            }
            if (SaveFile.GetInt("randomizer started with sword") == 1) {
                Settings.Add("start_with_sword");
            }
            if (SaveFile.GetInt("randomizer shuffled abilities") == 1) {
                Settings.Add("shuffle_abilities");
            }
            GUIUtility.systemCopyBuffer = string.Join(",", Settings.ToArray());
        }

        public static void PasteQuickSettings() {
            try {
                string SettingsString = GUIUtility.systemCopyBuffer;
                string[] SplitSettings = SettingsString.Split(',');
                CustomSeed = int.Parse(SplitSettings[0], CultureInfo.InvariantCulture);
                RandomizerSettings.GameModes NewGameMode;
                if (Enum.TryParse<RandomizerSettings.GameModes>(SplitSettings[1].ToUpper(), true, out NewGameMode)) {
                    TunicRandomizer.Settings.GameMode = NewGameMode;
                } else {
                    TunicRandomizer.Settings.GameMode = RandomizerSettings.GameModes.RANDOMIZER;
                }
                TunicRandomizer.Settings.KeysBehindBosses = SettingsString.Contains("keys_behind_bosses");
                TunicRandomizer.Settings.SwordProgressionEnabled = SettingsString.Contains("sword_progression");
                TunicRandomizer.Settings.StartWithSwordEnabled = SettingsString.Contains("start_with_sword");
                TunicRandomizer.Settings.ShuffleAbilities = SettingsString.Contains("shuffle_abilities");
            } catch (Exception e) {
                Logger.LogError("Error parsing quick settings string!");
            }

        }
    }
}
