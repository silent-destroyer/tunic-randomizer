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

        public static int CustomSeed = 0;
        public static Font OdinRounded;
        private void OnGUI() {
            Resources.FindObjectsOfTypeAll<Font>().Where(Font => Font.name == "Odin Rounded").ToList();
            GUI.skin.font = OdinRounded == null ? GUI.skin.font : OdinRounded;
            if (SceneLoaderPatches.SceneName == "TitleScreen") {
                Cursor.visible = true;
                GUI.Window(101, new Rect(20f, 150f, 400f, 300f), new Action<int>(QuickSettingsWindow), "Quick Settings");
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

            GUI.skin.button.fontSize = 20;
            GUI.Label(new Rect(10f, 220f, 300f, 30f), $"Custom Seed: {(CustomSeed == 0 ? "Not Set" : CustomSeed.ToString())}");
            bool PasteSeed = GUI.Button(new Rect(10f, 260f, 250f, 30f), "Paste Custom Seed");
            if (PasteSeed) {
                try {
                    CustomSeed = int.Parse(GUIUtility.systemCopyBuffer);
                } catch (System.Exception e) {

                }
            }
            if (CustomSeed != 0) {
                bool ClearSeed = GUI.Button(new Rect(270f, 260f, 100f, 30f), "Clear");
                if (ClearSeed) {
                    CustomSeed = 0;
                }
            }
        }
    }
}
