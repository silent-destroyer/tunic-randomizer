using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace TunicRandomizer {
    public class QuickSettings : MonoBehaviour {
        private static ManualLogSource Logger = TunicRandomizer.Logger;

        public static int CustomSeed = 0;
        public static Font OdinRounded;
        public static bool ShowHexQuestSliders = false;
        private void OnGUI() {
            if (SceneLoaderPatches.SceneName == "TitleScreen" && OdinRounded != null && GameObject.FindObjectOfType<TitleScreen>() != null) {
                GUI.skin.font = OdinRounded;
                Cursor.visible = true;
                GUI.Window(101, new Rect(20f, 150f, 430f, ShowHexQuestSliders && TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST ? 520f : 465f), new Action<int>(QuickSettingsWindow), "Quick Settings");
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
            bool ToggleHexagonQuest = GUI.Toggle(new Rect(140f, 60f, 150f, 30f), TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST, "Hexagon Quest");

            if (ToggleHexagonQuest) {
                TunicRandomizer.Settings.GameMode = RandomizerSettings.GameModes.HEXAGONQUEST;
                GUI.skin.button.fontSize = 16;
                bool ConfigureHexQuest = GUI.Button(new Rect(300f, 62.5f, 100f, 25f), ShowHexQuestSliders ? "Hide" : "Configure");
                if (ConfigureHexQuest) {
                    ShowHexQuestSliders = !ShowHexQuestSliders;
                }
                if(ShowHexQuestSliders) {
                    TunicRandomizer.Settings.HexagonQuestGoal = (int)GUI.HorizontalSlider(new Rect(220f, 105f, 200f, 20f), TunicRandomizer.Settings.HexagonQuestGoal, 15, 50);
                    TunicRandomizer.Settings.HexagonQuestExtraPercentage = (int)GUI.HorizontalSlider(new Rect(220f, 135f, 200f, 30f), TunicRandomizer.Settings.HexagonQuestExtraPercentage, 0, 100);
                    
                    GUI.Label(new Rect(10f, 90f, 220f, 20f),  $"<size=18>Hexagons Required:</size>");
                    GUI.Label(new Rect(10f, 120f, 220f, 30f),  $"<size=18>Hexagons in Item Pool:</size>");
                    GUI.Label(new Rect(190f, 90f, 30f, 30f), $"<size=18>{TunicRandomizer.Settings.HexagonQuestGoal}</size>");
                    GUI.Label(new Rect(190f, 120f, 30f, 30f), $"<size=18>{(int)Math.Round((100f + TunicRandomizer.Settings.HexagonQuestExtraPercentage) / 100f * TunicRandomizer.Settings.HexagonQuestGoal)}</size>");
                }
            }

            //TunicRandomizer.Settings.HexagonQuestGoal = (int)GUI.HorizontalSlider(new Rect(140f, 90f, 175f, 30f), (float)TunicRandomizer.Settings.HexagonQuestGoal, 15f, 50f);

            GUI.skin.toggle.fontSize = 15;
            bool ToggleSpoilerLog = GUI.Toggle(new Rect(330f, 20f, 90f, 30f), ItemRandomizer.CreateSpoilerLog, "Spoiler Log");
            ItemRandomizer.CreateSpoilerLog = ToggleSpoilerLog;
            GUI.skin.toggle.fontSize = 20;
            float y = ShowHexQuestSliders && ToggleHexagonQuest ? 155f : 95f;
            GUI.Label(new Rect(10f, y, 200f, 30f), "Logic Settings");
            y += 45f;
            bool TopggleBossKeys = GUI.Toggle(new Rect(10f, y, 200f, 30f), TunicRandomizer.Settings.KeysBehindBosses, "Keys Behind Bosses");
            TunicRandomizer.Settings.KeysBehindBosses = TopggleBossKeys;
            bool ToggleSwordProgression = GUI.Toggle(new Rect(240f, y, 180f, 30f), TunicRandomizer.Settings.SwordProgressionEnabled, "Sword Progression");
            TunicRandomizer.Settings.SwordProgressionEnabled = ToggleSwordProgression;
            y += 40f;
            bool ToggleSwordStart = GUI.Toggle(new Rect(10f, y, 175f, 30f), TunicRandomizer.Settings.StartWithSwordEnabled, "Start With Sword");
            TunicRandomizer.Settings.StartWithSwordEnabled = ToggleSwordStart;
            bool ToggleAbilityShuffle = GUI.Toggle(new Rect(240f, y, 175f, 30f), TunicRandomizer.Settings.ShuffleAbilities, "Shuffle Abilities");
            TunicRandomizer.Settings.ShuffleAbilities = ToggleAbilityShuffle;
            y += 40f;
            bool ToggleEntranceRando = GUI.Toggle(new Rect(10f, y, 200f, 30f), TunicRandomizer.Settings.EntranceRandoEnabled, "Entrance Randomizer");
            TunicRandomizer.Settings.EntranceRandoEnabled = ToggleEntranceRando;
            y += 40f;
            GUI.Label(new Rect(10f, y, 400f, 30f), "Other Settings <size=18>(more in options menu!)</size>");
            y += 40f;
            bool ToggleEnemyRandomizer = GUI.Toggle(new Rect(10f, y, 200f, 30f), TunicRandomizer.Settings.EnemyRandomizerEnabled, "Enemy Randomizer");
            TunicRandomizer.Settings.EnemyRandomizerEnabled = ToggleEnemyRandomizer;
            GUI.skin.button.fontSize = 20;
            y += 40f;
            GUI.Label(new Rect(10f, y, 300f, 30f), $"Custom Seed: {(CustomSeed == 0 ? "Not Set" : CustomSeed.ToString())}");
            if (CustomSeed != 0) {
                bool ClearSeed = GUI.Button(new Rect(300f, y, 110f, 30f), "Clear");
                if (ClearSeed) {
                    CustomSeed = 0;
                }
            }
            y += 40f;
            bool GenerateSeed = GUI.Button(new Rect(10f, y, 200f, 30f), "Generate Seed");
            if(GenerateSeed) {
                CustomSeed = new System.Random().Next();
            }

            bool PasteSeed = GUI.Button(new Rect(220f, y, 200f, 30f), "Paste Seed");
            if (PasteSeed) {
                try {
                    CustomSeed = int.Parse(GUIUtility.systemCopyBuffer, CultureInfo.InvariantCulture);
                } catch (System.Exception e) {

                }
            }
            y += 40f;
            bool CopySettings = GUI.Button(new Rect(10f, y, 200f, 30f), "Copy Seed + Settings");
            if(CopySettings) {
                CopyQuickSettings();
            }
            bool PasteSettings = GUI.Button(new Rect(220f, y, 200f, 30f), "Paste Seed + Settings");
            if(PasteSettings) {
                PasteQuickSettings();
            }


        }

        public static void CopyQuickSettings() {
            List<string> Settings = new List<string>() { CustomSeed.ToString(), Enum.GetName(typeof(RandomizerSettings.GameModes), TunicRandomizer.Settings.GameMode).ToLower() };
            if (TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST) {
                Settings.Add($"hexagon_quest_goal={TunicRandomizer.Settings.HexagonQuestGoal}=");
                Settings.Add($"hexagon_quest_extras~{TunicRandomizer.Settings.HexagonQuestExtraPercentage}~");
            }
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
            if (TunicRandomizer.Settings.EntranceRandoEnabled) {
                Settings.Add("entrance_randomizer");
            }
            GUIUtility.systemCopyBuffer = string.Join(",", Settings.ToArray());
        }

        public static void CopyQuickSettingsInGame() {
            List<string> Settings = new List<string>() { SaveFile.GetInt("seed").ToString(), SaveFile.GetString("randomizer game mode").ToLower() };
            if (SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST") {

                Settings.Add($"hexagon_quest_goal={SaveFile.GetInt("randomizer hexagon quest goal")}=");
                Settings.Add($"hexagon_quest_extras~{SaveFile.GetInt("randomizer hexagon quest extras")}~");
            }
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
            if (SaveFile.GetInt("randomizer entrance rando enabled") == 1) {
                Settings.Add("entrance_randomizer");
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
                if (SettingsString.Split('=').Count() > 1) {
                    try {
                        TunicRandomizer.Settings.HexagonQuestGoal = int.Parse(SettingsString.Split('=')[1]);
                    } catch (Exception e) {
                        TunicRandomizer.Settings.HexagonQuestGoal = 20;
                    }
                }
                if (SettingsString.Split('~').Count() > 1) {
                    try {
                        TunicRandomizer.Settings.HexagonQuestExtraPercentage = int.Parse(SettingsString.Split('~')[1]);
                    } catch (Exception e) {
                        TunicRandomizer.Settings.HexagonQuestExtraPercentage = 50;
                    }
                }
                TunicRandomizer.Settings.KeysBehindBosses = SettingsString.Contains("keys_behind_bosses");
                TunicRandomizer.Settings.SwordProgressionEnabled = SettingsString.Contains("sword_progression");
                TunicRandomizer.Settings.StartWithSwordEnabled = SettingsString.Contains("start_with_sword");
                TunicRandomizer.Settings.ShuffleAbilities = SettingsString.Contains("shuffle_abilities");
                TunicRandomizer.Settings.EntranceRandoEnabled = SettingsString.Contains("entrance_randomizer");
            } catch (Exception e) {
                Logger.LogError("Error parsing quick settings string!");
            }

        }
    }
}
