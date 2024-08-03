using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TunicRandomizer {
    public class QuickSettings : MonoBehaviour {

        public static string CustomSeed = "";
        public static Font OdinRounded;
        public static List<string> FoolChoices = new List<string>() { "Off", "Normal", "Double", "Onslaught" };
        public static List<string> FoolColors = new List<string>() { "white", "#4FF5D4", "#E3D457", "#FF3333" };
        private static bool ShowAdvancedSinglePlayerOptions = false;
        private static bool ShowAPSettingsWindow = false;
        private static string stringToEdit = "";
        private static bool editingPlayer = false;
        private static bool editingHostname = false;
        private static bool editingPort = false;
        private static bool editingPassword = false;
        private static bool showPort = false;
        private static bool showPassword = false;
        private static float guiScale = 1f;

        private void OnGUI() {
            if (SceneManager.GetActiveScene().name == "TitleScreen" && GameObject.FindObjectOfType<TitleScreen>() != null) {
                if (Screen.width == 3840 && Screen.height == 2160) {
                    guiScale = 1.25f;
                } else if (Screen.width == 1280 && Screen.height <= 800) {
                    guiScale = 0.75f;
                } else {
                    guiScale = 1f;
                }
                GUI.skin.font = PaletteEditor.OdinRounded == null ? GUI.skin.font : PaletteEditor.OdinRounded;
                Cursor.visible = true;
                switch (TunicRandomizer.Settings.Mode) {
                    case RandomizerSettings.RandomizerType.SINGLEPLAYER:
                        GUI.Window(101, new Rect(20f, (float)Screen.height * 0.12f, 430f * guiScale, TunicRandomizer.Settings.MysterySeed ? 470f * guiScale : 550f * guiScale), new Action<int>(SinglePlayerQuickSettingsWindow), "Single Player Settings");
                        ShowAPSettingsWindow = false;
                        editingPlayer = false;
                        editingHostname = false;
                        editingPort = false;
                        editingPassword = false;
                        break;
                    case RandomizerSettings.RandomizerType.ARCHIPELAGO:
                        GUI.Window(101, new Rect(20f, (float)Screen.height * 0.12f, 430f * guiScale, 540f * guiScale), new Action<int>(ArchipelagoQuickSettingsWindow), "Archipelago Settings");
                        break;
                }

                if (ShowAPSettingsWindow && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO) {
                    GUI.Window(103, new Rect(460f * guiScale, (float)Screen.height * 0.12f, 350f * guiScale, 490f * guiScale), new Action<int>(ArchipelagoConfigEditorWindow), "Archipelago Config");
                }
                if (ShowAdvancedSinglePlayerOptions && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER && !TunicRandomizer.Settings.MysterySeed) {
                    GUI.Window(105, new Rect(460f * guiScale, (float)Screen.height * 0.12f, 405f * guiScale, 565f * guiScale), new Action<int>(AdvancedLogicOptionsWindow), "Advanced Logic Options");
                }
                GameObject.Find("elderfox_sword graphic").GetComponent<Renderer>().enabled = !ShowAdvancedSinglePlayerOptions && !ShowAPSettingsWindow;
                if (TitleVersion.TitleButtons != null) {
                    foreach (Button button in TitleVersion.TitleButtons.GetComponentsInChildren<Button>()) {
                        button.enabled = !ShowAPSettingsWindow;
                    }
                }
            }
        }

        private void Update() {
            if (TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO && ShowAPSettingsWindow && SceneManager.GetActiveScene().name == "TitleScreen") {
                if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.Tab) && !Input.GetKeyDown(KeyCode.Backspace)) {
                    if (editingPort && Input.inputString != "" && int.TryParse(Input.inputString, out int num)) {
                        stringToEdit += Input.inputString;
                    } else if (!editingPort && Input.inputString != "") {
                        stringToEdit += Input.inputString;
                    }
                }
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                    if (!editingPlayer && !editingHostname && !editingPort && !editingHostname) {
                        CloseAPSettingsWindow();
                    } else {
                        editingPlayer = false;
                        editingHostname = false;
                        editingPort = false;
                        editingPassword = false;
                        stringToEdit = "";
                        OptionsGUIPatches.SaveSettings();
                    }
                }
                if (Input.GetKeyDown(KeyCode.Backspace)) {
                    if (stringToEdit.Length >= 2) {
                        stringToEdit = stringToEdit.Substring(0, stringToEdit.Length - 1);
                    } else {
                        stringToEdit = "";
                    }
                }
                if (editingPlayer) {
                    TunicRandomizer.Settings.ConnectionSettings.Player = stringToEdit;
                }
                if (editingHostname) {
                    TunicRandomizer.Settings.ConnectionSettings.Hostname = stringToEdit;
                }
                if (editingPort) {
                    TunicRandomizer.Settings.ConnectionSettings.Port = stringToEdit;
                }
                if (editingPassword) {
                    TunicRandomizer.Settings.ConnectionSettings.Password = stringToEdit;
                }
            }
        }

        private static void ArchipelagoQuickSettingsWindow(int windowID) {
            GUI.skin.label.fontSize = (int)(25 * guiScale);
            GUI.skin.button.fontSize = (int)(20 * guiScale);
            GUI.skin.toggle.fontSize = (int)(20 * guiScale);
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.skin.label.clipping = TextClipping.Overflow;
            GUI.color = Color.white;
            GUI.DragWindow(new Rect(500f * guiScale, 50f * guiScale, 500f * guiScale, 30f * guiScale));

            float y = 20f * guiScale;

            GUI.skin.toggle.fontSize = (int)(15 * guiScale); 
            GUI.skin.button.fontSize = (int)(15 * guiScale); 
            GUI.skin.label.fontSize = (int)(15 * guiScale);

            if (TunicRandomizer.Settings.RaceMode) {
                TunicRandomizer.Settings.RaceMode = GUI.Toggle(new Rect(330f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.RaceMode, "Race Mode");
            } else {
                if (Archipelago.instance.integration.disableSpoilerLog) {
                    GUI.Label(new Rect(240f * guiScale, y, 200f * guiScale, 30f * guiScale), "Spoiler Log Disabled by Host");
                } else {
                    bool ToggleSpoilerLog = GUI.Toggle(new Rect(TunicRandomizer.Settings.CreateSpoilerLog ? 280f * guiScale : 330f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.CreateSpoilerLog, "Spoiler Log");
                    TunicRandomizer.Settings.CreateSpoilerLog = ToggleSpoilerLog;
                    if (ToggleSpoilerLog) {
                        GUI.skin.button.fontSize = (int)(15 * guiScale);
                        bool OpenSpoilerLog = GUI.Button(new Rect(370f * guiScale, y, 50f * guiScale, 25f * guiScale), "Open");
                        if (OpenSpoilerLog) {
                            if (File.Exists(TunicRandomizer.SpoilerLogPath)) {
                                System.Diagnostics.Process.Start(TunicRandomizer.SpoilerLogPath);
                            }
                        }
                    }
                }
            }

            GUI.skin.label.fontSize = (int)(25 * guiScale);
            GUI.skin.toggle.fontSize = (int)(20 * guiScale);
            GUI.skin.button.fontSize = (int)(20 * guiScale);

            GUI.Label(new Rect(10f * guiScale, 20f * guiScale, 300f * guiScale, 30f * guiScale), "Randomizer Mode");
            y += 40f * guiScale;
            bool ToggleSinglePlayer = GUI.Toggle(new Rect(10f * guiScale, y, 130f * guiScale, 30f * guiScale), TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER, "Single Player");
            if (ToggleSinglePlayer && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO) {
                TunicRandomizer.Settings.Mode = RandomizerSettings.RandomizerType.SINGLEPLAYER;
                OptionsGUIPatches.SaveSettings();
            }
            bool ToggleArchipelago = GUI.Toggle(new Rect(150f * guiScale, y, 150f * guiScale, 30f * guiScale), TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO, "Archipelago");
            if (ToggleArchipelago && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER) {
                TunicRandomizer.Settings.Mode = RandomizerSettings.RandomizerType.ARCHIPELAGO;
                OptionsGUIPatches.SaveSettings();
            }
            y += 40f * guiScale;
            GUI.Label(new Rect(10f * guiScale, y, 500f * guiScale, 30f * guiScale), $"Player: {(TunicRandomizer.Settings.ConnectionSettings.Player)}");
            y += 40f * guiScale;
            GUI.Label(new Rect(10f * guiScale, y, 80f * guiScale, 30f * guiScale), $"Status:");
            if (Archipelago.instance.integration != null && Archipelago.instance.integration.connected) {
                GUI.color = Color.green;
                GUI.Label(new Rect(95f * guiScale, y, 150f * guiScale, 30f * guiScale), $"Connected!");
                GUI.color = Color.white;
                GUI.Label(new Rect(250f * guiScale, y, 300f * guiScale, 30f * guiScale), $"(world {Archipelago.instance.integration.session.ConnectionInfo.Slot} of {Archipelago.instance.integration.session.Players.Players[0].Count-1})");
            } else {
                GUI.color = Color.red;
                GUI.Label(new Rect(95f * guiScale, y, 300f * guiScale, 30f * guiScale), $"Disconnected");
            }
            GUI.color = Color.white;
            y += 40f * guiScale;
            bool Connect = GUI.Button(new Rect(10f * guiScale, y, 200f * guiScale, 30f * guiScale), "Connect");
            if (Connect) {
                Archipelago.instance.Connect();
            }

            bool Disconnect = GUI.Button(new Rect(220f * guiScale, y, 200f * guiScale, 30f * guiScale), "Disconnect");
            if (Disconnect) {
                Archipelago.instance.Disconnect();
            }
            y += 40f * guiScale;
            bool OpenSettings = GUI.Button(new Rect(10f * guiScale, y, 200f * guiScale, 30f * guiScale), "Open Settings File");
            if (OpenSettings) {
                try {
                    System.Diagnostics.Process.Start(TunicRandomizer.SettingsPath);
                } catch (Exception e) {
                    TunicLogger.LogError(e.Message);
                }
            }
            bool OpenAPSettings = GUI.Button(new Rect(220f * guiScale, y, 200f * guiScale, 30f * guiScale), ShowAPSettingsWindow ? "Close AP Config" : "Edit AP Config");
            if (OpenAPSettings) {
                if (ShowAPSettingsWindow) {
                    CloseAPSettingsWindow();
                    Archipelago.instance.Disconnect();
                    Archipelago.instance.Connect();
                } else {
                    ShowAPSettingsWindow = true;
                }
            }
            y += 40f * guiScale;
            GUI.Label(new Rect(10f * guiScale, y, 200f * guiScale, 30f * guiScale), $"World Settings");
            if (Archipelago.instance.integration != null && Archipelago.instance.integration.connected) {
                Dictionary<string, object> slotData = Archipelago.instance.GetPlayerSlotData();
                y += 40f * guiScale;
                GUI.Toggle(new Rect(10f * guiScale, y, 180f * guiScale, 30f * guiScale), slotData["keys_behind_bosses"].ToString() == "1", "Keys Behind Bosses");
                GUI.Toggle(new Rect(220f * guiScale, y, 210f * guiScale, 30f * guiScale), slotData["sword_progression"].ToString() == "1", "Sword Progression");
                y += 40f * guiScale;
                GUI.Toggle(new Rect(10f * guiScale, y, 175f * guiScale, 30f * guiScale), slotData["start_with_sword"].ToString() == "1", "Start With Sword");
                GUI.Toggle(new Rect(220f * guiScale, y, 175f * guiScale, 30f * guiScale), slotData["ability_shuffling"].ToString() == "1", "Shuffled Abilities");
                y += 40f * guiScale;
                GUI.Toggle(new Rect(10f * guiScale, y, 185f * guiScale, 30f * guiScale), slotData["hexagon_quest"].ToString() == "1", slotData["hexagon_quest"].ToString() == "1" ? 
                    $"Hexagon Quest (<color=#E3D457>{slotData["Hexagon Quest Goal"].ToString()}</color>)" : $"Hexagon Quest");
                int FoolIndex = int.Parse(slotData["fool_traps"].ToString());
                GUI.Toggle(new Rect(220f * guiScale, y, 195f * guiScale, 60f * guiScale), FoolIndex != 0, $"Fool Traps: {(FoolIndex == 0 ? "Off" : $"<color={FoolColors[FoolIndex]}>{FoolChoices[FoolIndex]}</color>")}");

                if (slotData.ContainsKey("entrance_rando")) {
                    y += 40f * guiScale;
                    GUI.Toggle(new Rect(10f * guiScale, y, 195f * guiScale, 30f * guiScale), slotData["entrance_rando"].ToString() == "1", $"Entrance Randomizer");
                } else {
                    y += 40f * guiScale;
                    GUI.Toggle(new Rect(10f * guiScale, y, 195f * guiScale, 30f * guiScale), false, $"Entrance Randomizer");
                }
                if (slotData.ContainsKey("shuffle_ladders")) {
                    GUI.Toggle(new Rect(220f * guiScale, y, 195f * guiScale, 30f * guiScale), slotData["shuffle_ladders"].ToString() == "1", $"Shuffled Ladders");
                } else {
                    GUI.Toggle(new Rect(220f * guiScale, y, 195f * guiScale, 30f * guiScale), false, $"Shuffled Ladders");
                }
            } else {
                y += 40f * guiScale;
                GUI.Toggle(new Rect(10f * guiScale, y, 180f * guiScale, 30f * guiScale), false, "Keys Behind Bosses");
                GUI.Toggle(new Rect(220f * guiScale, y, 210f * guiScale, 30f * guiScale), false, "Sword Progression");
                y += 40f * guiScale;
                GUI.Toggle(new Rect(10f * guiScale, y, 175f * guiScale, 30f * guiScale), false, "Start With Sword");
                GUI.Toggle(new Rect(220f * guiScale, y, 175f * guiScale, 30f * guiScale), false, "Shuffled Abilities");
                y += 40f * guiScale;
                GUI.Toggle(new Rect(10f * guiScale, y, 175f * guiScale, 30f * guiScale), false, "Hexagon Quest");
                GUI.Toggle(new Rect(220f * guiScale, y, 175f * guiScale, 30f * guiScale), false, "Fool Traps: Off");
                y += 40f * guiScale;
                GUI.Toggle(new Rect(10f * guiScale, y, 195f * guiScale, 30f * guiScale), false, $"Entrance Randomizer");
                GUI.Toggle(new Rect(220f * guiScale, y, 195f * guiScale, 30f * guiScale), false, $"Shuffled Ladders");
            }
            y += 40f * guiScale;
            GUI.Label(new Rect(10f * guiScale, y, 400f * guiScale, 30f * guiScale), $"Other Settings <size={(int)(18 * guiScale)}>(more in options menu!)</size>");
            y += 40f * guiScale;
            TunicRandomizer.Settings.DeathLinkEnabled = GUI.Toggle(new Rect(10f * guiScale, y, 105f * guiScale, 30f * guiScale), TunicRandomizer.Settings.DeathLinkEnabled, "Death Link");
            TunicRandomizer.Settings.EnemyRandomizerEnabled = GUI.Toggle(new Rect(120f * guiScale, y, 170f * guiScale, 30f * guiScale), TunicRandomizer.Settings.EnemyRandomizerEnabled, "Enemy Randomizer");
            TunicRandomizer.Settings.MusicShuffle = GUI.Toggle(new Rect(295f * guiScale, y, 130f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MusicShuffle, "Music Shuffle");
            GUI.skin.label.fontSize = (int)(20 * guiScale);
        }

        private static void SinglePlayerQuickSettingsWindow(int windowID) {
            GUI.skin.label.fontSize = (int)(25 * guiScale);
            //GUI.skin.toggle.fontSize = 25 * multiplier;
            //GUI.skin.toggle.alignment = TextAnchor.UpperLeft;
            GUI.skin.toggle.fontSize = (int)(20 * guiScale);
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.skin.label.clipping = TextClipping.Overflow;
            GUI.color = Color.white;
            GUI.DragWindow(new Rect(500f * guiScale, 50f * guiScale, 500f * guiScale, 30f * guiScale));
            float y = 20f * guiScale;

            GUI.skin.toggle.fontSize = (int)(15 * guiScale);
            if (TunicRandomizer.Settings.RaceMode) {
                TunicRandomizer.Settings.RaceMode = GUI.Toggle(new Rect(330f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.RaceMode, "Race Mode");
            } else {
                bool ToggleSpoilerLog = GUI.Toggle(new Rect(TunicRandomizer.Settings.CreateSpoilerLog ? 280f * guiScale : 330f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.CreateSpoilerLog, "Spoiler Log");
                TunicRandomizer.Settings.CreateSpoilerLog = ToggleSpoilerLog;
                if (ToggleSpoilerLog) {
                    GUI.skin.button.fontSize = (int)(15 * guiScale);
                    bool OpenSpoilerLog = GUI.Button(new Rect(370f * guiScale, y, 50f * guiScale, 25f * guiScale), "Open");
                    if (OpenSpoilerLog) {
                        if (File.Exists(TunicRandomizer.SpoilerLogPath)) {
                            System.Diagnostics.Process.Start(TunicRandomizer.SpoilerLogPath);
                        }
                    }
                }
            }

            GUI.skin.toggle.fontSize = (int)(20 * guiScale);

            GUI.Label(new Rect(10f * guiScale, 20f * guiScale, 300f * guiScale, 30f * guiScale), "Randomizer Mode");
            y += 40f * guiScale;
            bool ToggleSinglePlayer = GUI.Toggle(new Rect(10f * guiScale, y, 130f * guiScale, 30f * guiScale), TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER, "Single Player");
            if (ToggleSinglePlayer && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO) {
                TunicRandomizer.Settings.Mode = RandomizerSettings.RandomizerType.SINGLEPLAYER;
                OptionsGUIPatches.SaveSettings();
            }
            bool ToggleArchipelago = GUI.Toggle(new Rect(150f * guiScale, y, 150f * guiScale, 30f * guiScale), TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO, "Archipelago");
            if (ToggleArchipelago && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER) {
                TunicRandomizer.Settings.Mode = RandomizerSettings.RandomizerType.ARCHIPELAGO;
                OptionsGUIPatches.SaveSettings();
            }

            GUI.skin.button.fontSize = (int)(20 * guiScale);
            y += 40f * guiScale;
            GUI.Label(new Rect(10f * guiScale, y, 200f * guiScale, 30f * guiScale), "Logic Settings");
            y += 45f * guiScale; 
            if (TunicRandomizer.Settings.MysterySeed) {
                GUI.Label(new Rect(10f * guiScale, y, 400f * guiScale, 30f * guiScale), "Mystery Seed Enabled!");
                GUI.skin.label.fontSize = (int)(20 * guiScale);
                y += 40f * guiScale;
                GUI.Label(new Rect(10f * guiScale, y, 400f * guiScale, 30f * guiScale), "Settings will be chosen randomly on New Game.");
                y += 40f * guiScale;
                TunicRandomizer.Settings.StartWithSwordEnabled = GUI.Toggle(new Rect(10f * guiScale, y, 175f * guiScale, 30f * guiScale), TunicRandomizer.Settings.StartWithSwordEnabled, "Start With Sword");
                TunicRandomizer.Settings.MysterySeed = GUI.Toggle(new Rect(240f * guiScale, y, 200f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeed, "Mystery Seed");
                y += 40f * guiScale;
            } else {
                bool ToggleHexagonQuest = GUI.Toggle(new Rect(10f * guiScale, y, 185f * guiScale, 30f * guiScale), TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST, $"Hexagon Quest {(TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST ? $"(<color=#E3D457>{TunicRandomizer.Settings.HexagonQuestGoal}</color>)" : "")}");
                if (ToggleHexagonQuest) {
                    TunicRandomizer.Settings.GameMode = RandomizerSettings.GameModes.HEXAGONQUEST;
                } else if (!ToggleHexagonQuest && TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST) {
                    TunicRandomizer.Settings.GameMode = RandomizerSettings.GameModes.RANDOMIZER;
                }
                TunicRandomizer.Settings.SwordProgressionEnabled = GUI.Toggle(new Rect(240f * guiScale, y, 180f * guiScale, 30f * guiScale), TunicRandomizer.Settings.SwordProgressionEnabled, "Sword Progression");
                y += 40f * guiScale; 
                TunicRandomizer.Settings.KeysBehindBosses = GUI.Toggle(new Rect(10f * guiScale, y, 200f * guiScale, 30f * guiScale), TunicRandomizer.Settings.KeysBehindBosses, "Keys Behind Bosses");
                TunicRandomizer.Settings.ShuffleAbilities  = GUI.Toggle(new Rect(240f * guiScale, y, 175f * guiScale, 30f * guiScale), TunicRandomizer.Settings.ShuffleAbilities, "Shuffle Abilities");
                y += 40f * guiScale;
                TunicRandomizer.Settings.EntranceRandoEnabled = GUI.Toggle(new Rect(10f * guiScale, y, 200f * guiScale, 30f * guiScale), TunicRandomizer.Settings.EntranceRandoEnabled, "Entrance Randomizer"); 
                TunicRandomizer.Settings.ShuffleLadders = GUI.Toggle(new Rect(240f * guiScale, y, 200f * guiScale, 30f * guiScale), TunicRandomizer.Settings.ShuffleLadders, "Shuffle Ladders");
                y += 40f * guiScale;
                TunicRandomizer.Settings.StartWithSwordEnabled = GUI.Toggle(new Rect(10f * guiScale, y, 175f * guiScale, 30f * guiScale), TunicRandomizer.Settings.StartWithSwordEnabled, "Start With Sword");
                TunicRandomizer.Settings.MysterySeed = GUI.Toggle(new Rect(240f * guiScale, y, 200f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeed, "Mystery Seed");
                y += 40f * guiScale;
                GUI.skin.button.fontSize = (int)(20 * guiScale);
                bool ShowAdvancedOptions = GUI.Button(new Rect(10f * guiScale, y, 410f * guiScale, 30f * guiScale), $"{(ShowAdvancedSinglePlayerOptions ? "Hide" : "Show")} Advanced Options");
                if (ShowAdvancedOptions) {
                    ShowAdvancedSinglePlayerOptions = !ShowAdvancedSinglePlayerOptions;
                }
                y += 40f * guiScale;

            }
            GUI.Label(new Rect(10f * guiScale, y, 400f * guiScale, 30f * guiScale), $"Other Settings <size={(int)(18 * guiScale)}>(more in options menu!)</size>");
            y += 40f * guiScale;
            TunicRandomizer.Settings.EnemyRandomizerEnabled = GUI.Toggle(new Rect(10f * guiScale, y, 180f * guiScale, 30f * guiScale), TunicRandomizer.Settings.EnemyRandomizerEnabled, "Enemy Randomizer");
            TunicRandomizer.Settings.MusicShuffle = GUI.Toggle(new Rect(210f * guiScale, y, 150f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MusicShuffle, "Music Shuffle");
            y += 40f * guiScale;
            GUI.Label(new Rect(10f * guiScale, y, 300f * guiScale, 30f * guiScale), $"Custom Seed: {(CustomSeed == "" ? "Not Set" : CustomSeed.ToString())}");
            y += 40f * guiScale;
            bool GenerateSeed = GUI.Button(new Rect(10f * guiScale, y, 200f * guiScale, 30f * guiScale), "Generate Seed");
            if (GenerateSeed) {
                CustomSeed = new System.Random().Next().ToString();
            }

            bool ClearSeed = GUI.Button(new Rect(220f * guiScale, y, 200f * guiScale, 30f * guiScale), "Clear Seed");
            if (ClearSeed) {
                CustomSeed = "";
            }
            y += 40f * guiScale;
            bool CopySettings = GUI.Button(new Rect(10f * guiScale, y, 200f * guiScale, 30f * guiScale), "Copy Seed + Settings");
            if (CopySettings) {
                GUIUtility.systemCopyBuffer = TunicRandomizer.Settings.GetSettingsString();
            }
            bool PasteSettings = GUI.Button(new Rect(220f * guiScale, y, 200f * guiScale, 30f * guiScale), "Paste Seed + Settings");
            if (PasteSettings) {
                TunicRandomizer.Settings.ParseSettingsString(GUIUtility.systemCopyBuffer);
            }
        }

        private static void AdvancedLogicOptionsWindow(int windowID) {
            GUI.skin.label.fontSize = (int)(25 * guiScale);
            float y = 20f * guiScale;
            GUI.Label(new Rect(10f * guiScale, y, 300f * guiScale, 30f * guiScale), $"Hexagon Quest");
            y += 30 * guiScale;
            GUI.Label(new Rect(10f * guiScale, y, 220f * guiScale, 20f * guiScale), $"<size={(int)(18 * guiScale)}>Hexagons Required:</size>");
            GUI.Label(new Rect(190f * guiScale, y, 30f * guiScale, 30f * guiScale), $"<size={(int)(18 * guiScale)}>{(TunicRandomizer.Settings.HexagonQuestGoal)}</size>");
            TunicRandomizer.Settings.HexagonQuestGoal = (int)GUI.HorizontalSlider(new Rect(220f * guiScale, y + 15, 175f * guiScale, 20f * guiScale), TunicRandomizer.Settings.HexagonQuestGoal, 15, 50);
            y += 30f * guiScale;
            GUI.Label(new Rect(10f * guiScale, y, 220f * guiScale, 30f * guiScale), $"<size={(int)(18 * guiScale)}>Hexagons in Item Pool:</size>");
            GUI.Label(new Rect(190f * guiScale, y, 30f * guiScale, 30f * guiScale), $"<size={(int)(18 * guiScale)}>{((int)Math.Round((100f + TunicRandomizer.Settings.HexagonQuestExtraPercentage) / 100f * TunicRandomizer.Settings.HexagonQuestGoal))}</size>");
            TunicRandomizer.Settings.HexagonQuestExtraPercentage = (int)GUI.HorizontalSlider(new Rect(220f * guiScale, y + 15, 175f * guiScale, 30f * guiScale), TunicRandomizer.Settings.HexagonQuestExtraPercentage, 0, 100);
            y += 40f * guiScale;
            GUI.Label(new Rect(10f * guiScale, y, 300f * guiScale, 30f * guiScale), $"Entrance Randomizer");
            y += 40f * guiScale;
            TunicRandomizer.Settings.ERFixedShop = GUI.Toggle(new Rect(10f * guiScale, y, 200f * guiScale, 30f * guiScale), TunicRandomizer.Settings.ERFixedShop, "Fewer Shop Entrances");
            y += 40f * guiScale;
            GUI.Label(new Rect(10f * guiScale, y, 300f * guiScale, 30f * guiScale), $"Fool Traps");
            y += 40f * guiScale;
            bool NoFools = GUI.Toggle(new Rect(10f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.NONE, "None");
            if (NoFools) {
                TunicRandomizer.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.NONE;
            }
            bool NormalFools = GUI.Toggle(new Rect(110f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.NORMAL, "<color=#4FF5D4>Normal</color>");
            if (NormalFools) {
                TunicRandomizer.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.NORMAL;
            }
            bool DoubleFools = GUI.Toggle(new Rect(200f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.DOUBLE, "<color=#E3D457>Double</color>");
            if (DoubleFools) {
                TunicRandomizer.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.DOUBLE;
            }
            bool OnslaughtFools = GUI.Toggle(new Rect(290f * guiScale, y, 100f * guiScale, 30f * guiScale), TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.ONSLAUGHT, "<color=#FF3333>Onslaught</color>");
            if (OnslaughtFools) {
                TunicRandomizer.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.ONSLAUGHT;
            }
            y += 40f * guiScale;
            GUI.Label(new Rect(10f * guiScale, y, 300f * guiScale, 30f * guiScale), $"Hero's Laurels Location");
            y += 40f * guiScale;
            bool RandomLaurels = GUI.Toggle(new Rect(10f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.FixedLaurelsOption == RandomizerSettings.FixedLaurelsType.RANDOM, "Random");
            if (RandomLaurels) {
                TunicRandomizer.Settings.FixedLaurelsOption = RandomizerSettings.FixedLaurelsType.RANDOM;
            }
            bool SixCoinsLaurels = GUI.Toggle(new Rect(110f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.FixedLaurelsOption == RandomizerSettings.FixedLaurelsType.SIXCOINS, "6 Coins");
            if (SixCoinsLaurels) {
                TunicRandomizer.Settings.FixedLaurelsOption = RandomizerSettings.FixedLaurelsType.SIXCOINS;
            }
            bool TenCoinsLaurels = GUI.Toggle(new Rect(200f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.FixedLaurelsOption == RandomizerSettings.FixedLaurelsType.TENCOINS, "10 Coins");
            if (TenCoinsLaurels) {
                TunicRandomizer.Settings.FixedLaurelsOption = RandomizerSettings.FixedLaurelsType.TENCOINS;
            }
            bool TenFairiesLaurels = GUI.Toggle(new Rect(290f * guiScale, y, 100f * guiScale, 30f * guiScale), TunicRandomizer.Settings.FixedLaurelsOption == RandomizerSettings.FixedLaurelsType.TENFAIRIES, "10 Fairies");
            if (TenFairiesLaurels) {
                TunicRandomizer.Settings.FixedLaurelsOption = RandomizerSettings.FixedLaurelsType.TENFAIRIES;
            }
            y += 40f * guiScale;
            GUI.Label(new Rect(10f * guiScale, y, 300f * guiScale, 30f * guiScale), $"Grass Randomizer");
            y += 40f * guiScale;
            TunicRandomizer.Settings.GrassRandomizer = GUI.Toggle(new Rect(10f * guiScale, y, 175f * guiScale, 30f * guiScale), TunicRandomizer.Settings.GrassRandomizer, "Grass Randomizer");
            TunicRandomizer.Settings.ClearEarlyBushes = GUI.Toggle(new Rect(195 * guiScale, y, 195f * guiScale, 30f * guiScale), TunicRandomizer.Settings.ClearEarlyBushes, "Clear Early Bushes");
            y += 40f * guiScale;
            GUI.Label(new Rect(10f * guiScale, y, 300f * guiScale, 30f * guiScale), $"Difficulty Options");
            y += 40f * guiScale;
            TunicRandomizer.Settings.Lanternless = GUI.Toggle(new Rect(10f * guiScale, y, 175f * guiScale, 30f * guiScale), TunicRandomizer.Settings.Lanternless, "Lanternless Logic");
            TunicRandomizer.Settings.Maskless = GUI.Toggle(new Rect(195f * guiScale, y, 175f * guiScale, 30f * guiScale), TunicRandomizer.Settings.Maskless, "Maskless Logic");
            y += 40f * guiScale;
            bool Close = GUI.Button(new Rect(10f * guiScale, y, 200f * guiScale, 30f * guiScale), "Close");
            if (Close) {
                ShowAdvancedSinglePlayerOptions = false;
                OptionsGUIPatches.SaveSettings();
            }
        }

        private static void ArchipelagoConfigEditorWindow(int windowID) {
            GUI.skin.label.fontSize = (int)(25 * guiScale);
            GUI.skin.button.fontSize = (int)(17 * guiScale);
            GUI.Label(new Rect(10f * guiScale, 20f * guiScale, 300f * guiScale, 30f * guiScale), $"Player: {(TunicRandomizer.Settings.ConnectionSettings.Player)}");
            bool EditPlayer = GUI.Button(new Rect(10f * guiScale, 70f * guiScale, 75f * guiScale, 30f * guiScale), editingPlayer ? "Save" : "Edit");
            if (EditPlayer) {
                if (editingPlayer) {
                    stringToEdit = "";
                    editingPlayer = false;
                    OptionsGUIPatches.SaveSettings();
                } else {
                    stringToEdit = TunicRandomizer.Settings.ConnectionSettings.Player;
                    editingPlayer = true;
                    editingHostname = false;
                    editingPort = false;
                    editingPassword = false;
                }
            }
            bool PastePlayer = GUI.Button(new Rect(100f * guiScale, 70f * guiScale, 75f * guiScale, 30f * guiScale), "Paste");
            if (PastePlayer) {
                TunicRandomizer.Settings.ConnectionSettings.Player = GUIUtility.systemCopyBuffer;
                editingPlayer = false;
                OptionsGUIPatches.SaveSettings();
            }
            bool ClearPlayer = GUI.Button(new Rect(190f * guiScale, 70f * guiScale, 75f * guiScale, 30f * guiScale), "Clear");
            if (ClearPlayer) {
                if (editingPlayer) { 
                    stringToEdit = "";
                }
                TunicRandomizer.Settings.ConnectionSettings.Player = "";
                OptionsGUIPatches.SaveSettings();
            }

            GUI.Label(new Rect(10f * guiScale, 120f * guiScale, 300f * guiScale, 30f * guiScale), $"Host: {(TunicRandomizer.Settings.ConnectionSettings.Hostname)}");
            bool setLocalhost = GUI.Toggle(new Rect(160f * guiScale, 160f * guiScale, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.ConnectionSettings.Hostname == "localhost", "localhost");
            if (setLocalhost && TunicRandomizer.Settings.ConnectionSettings.Hostname != "localhost") {
                TunicRandomizer.Settings.ConnectionSettings.Hostname = "localhost";
                OptionsGUIPatches.SaveSettings();
            }
            bool setArchipelagoHost = GUI.Toggle(new Rect(10f * guiScale, 160f * guiScale, 140f * guiScale, 30f * guiScale), TunicRandomizer.Settings.ConnectionSettings.Hostname == "archipelago.gg", "archipelago.gg");
            if (setArchipelagoHost && TunicRandomizer.Settings.ConnectionSettings.Hostname != "archipelago.gg") {
                TunicRandomizer.Settings.ConnectionSettings.Hostname = "archipelago.gg";
                OptionsGUIPatches.SaveSettings();
            }
            bool EditHostname = GUI.Button(new Rect(10f * guiScale, 200f * guiScale, 75f * guiScale, 30f * guiScale), editingHostname ? "Save" : "Edit");
            if (EditHostname) {
                if (editingHostname) {
                    stringToEdit = "";
                    editingHostname = false;
                    OptionsGUIPatches.SaveSettings();
                } else {
                    stringToEdit = TunicRandomizer.Settings.ConnectionSettings.Hostname;
                    editingPlayer = false;
                    editingHostname = true;
                    editingPort = false;
                    editingPassword = false;
                }
            }
            bool PasteHostname = GUI.Button(new Rect(100f * guiScale, 200f * guiScale, 75f * guiScale, 30f * guiScale), "Paste");
            if (PasteHostname) {
                TunicRandomizer.Settings.ConnectionSettings.Hostname = GUIUtility.systemCopyBuffer;
                editingHostname = false;
                OptionsGUIPatches.SaveSettings();
            }
            bool ClearHost = GUI.Button(new Rect(190f * guiScale, 200f * guiScale, 75f * guiScale, 30f * guiScale), "Clear");
            if (ClearHost) {
                if (editingHostname) {
                    stringToEdit = "";
                }
                TunicRandomizer.Settings.ConnectionSettings.Hostname = "";
                OptionsGUIPatches.SaveSettings();
            }

            GUI.Label(new Rect(10f * guiScale, 250f * guiScale, 300f * guiScale, 30f * guiScale), $"Port: {(editingPort ? (showPort ? stringToEdit : new string('*', stringToEdit.Length)) : (showPort ? TunicRandomizer.Settings.ConnectionSettings.Port.ToString() : new string('*', TunicRandomizer.Settings.ConnectionSettings.Port.ToString().Length)))}");
            showPort = GUI.Toggle(new Rect(270f * guiScale, 260f * guiScale, 75f * guiScale, 30f * guiScale), showPort, "Show");
            bool EditPort = GUI.Button(new Rect(10f * guiScale, 300f * guiScale, 75f * guiScale, 30f * guiScale), editingPort ? "Save" : "Edit");
            if (EditPort) {
                if (editingPort) {
                    stringToEdit = "";
                    editingPort = false;
                    OptionsGUIPatches.SaveSettings();
                } else {
                    stringToEdit = TunicRandomizer.Settings.ConnectionSettings.Port.ToString();
                    editingPlayer = false;
                    editingHostname = false;
                    editingPort = true;
                    editingPassword = false;
                }
            }
            bool PastePort = GUI.Button(new Rect(100f * guiScale, 300f * guiScale, 75f * guiScale, 30f * guiScale), "Paste");
            if (PastePort) {
                try {
                    if (int.TryParse(GUIUtility.systemCopyBuffer, out int num)) {
                        TunicRandomizer.Settings.ConnectionSettings.Port = GUIUtility.systemCopyBuffer;
                    }
                    editingPort = false;
                    OptionsGUIPatches.SaveSettings();
                } catch (Exception e) {
                    TunicLogger.LogError("invalid input pasted for port number!");
                }
            }
            bool ClearPort = GUI.Button(new Rect(190f * guiScale, 300f * guiScale, 75f * guiScale, 30f * guiScale), "Clear");
            if (ClearPort) {
                if (editingPort) {
                    stringToEdit = "";
                }
                TunicRandomizer.Settings.ConnectionSettings.Port = "";
                OptionsGUIPatches.SaveSettings();
            }

            GUI.Label(new Rect(10f * guiScale, 350f * guiScale, 300f * guiScale, 30f * guiScale), $"Password: {(showPassword ? TunicRandomizer.Settings.ConnectionSettings.Password : new string('*', TunicRandomizer.Settings.ConnectionSettings.Password.Length))}");
            showPassword = GUI.Toggle(new Rect(270f * guiScale, 360f * guiScale, 75f * guiScale, 30f * guiScale), showPassword, "Show");
            bool EditPassword = GUI.Button(new Rect(10f * guiScale, 400f * guiScale, 75f * guiScale, 30f * guiScale), editingPassword ? "Save" : "Edit");
            if (EditPassword) {
                if (editingPassword) {
                    stringToEdit = "";
                    editingPassword = false;
                    OptionsGUIPatches.SaveSettings();
                } else {
                    stringToEdit = TunicRandomizer.Settings.ConnectionSettings.Password;
                    editingPlayer = false;
                    editingHostname = false;
                    editingPort = false;
                    editingPassword = true;
                }
            }

            bool PastePassword = GUI.Button(new Rect(100f * guiScale, 400f * guiScale, 75f * guiScale, 30f * guiScale), "Paste");
            if (PastePassword) {
                TunicRandomizer.Settings.ConnectionSettings.Password = GUIUtility.systemCopyBuffer;
                editingPassword = false;
                OptionsGUIPatches.SaveSettings();
            }
            bool ClearPassword = GUI.Button(new Rect(190f * guiScale, 400f * guiScale, 75f * guiScale, 30f * guiScale), "Clear");
            if (ClearPassword) {
                if (editingPassword) {
                    stringToEdit = "";
                }
                TunicRandomizer.Settings.ConnectionSettings.Password = "";
                OptionsGUIPatches.SaveSettings();
            }
            bool Close = GUI.Button(new Rect(10f * guiScale, 450f * guiScale, 165f * guiScale, 30f * guiScale), "Close");
            if (Close) {
                CloseAPSettingsWindow();
                Archipelago.instance.Disconnect();
                Archipelago.instance.Connect();
            }

        }

        private static void CloseAPSettingsWindow() {
            ShowAPSettingsWindow = false;
            stringToEdit = "";
            editingPlayer = false;
            editingHostname = false;
            editingPort = false;
            editingPassword = false;
            OptionsGUIPatches.SaveSettings();
        }

        public static bool TitleScreen___NewGame_PrefixPatch(TitleScreen __instance) {
            CloseAPSettingsWindow();
            if (Archipelago.instance != null && Archipelago.instance.integration != null) {
                Archipelago.instance.integration.ItemIndex = 0;
            }
            return true;
        }

        public static bool FileManagement_LoadFileAndStart_PrefixPatch(FileManagementGUI __instance, string filename) {
            CloseAPSettingsWindow();
            SaveFile.LoadFromFile(filename);
            if (SaveFile.GetInt("archipelago") == 0 && SaveFile.GetInt("randomizer") == 0) {
                TunicLogger.LogInfo("Non-Randomizer file selected!");
                GenericMessage.ShowMessage("<#FF0000>[death] \"<#FF0000>warning!\" <#FF0000>[death]\n\"Non-Randomizer file selected.\"\n\"Returning to menu.\"");
                return false;
            }
            string errorMessage = "";
            if (SaveFile.GetInt("archipelago") == 1 && SaveFile.GetString("archipelago player name") != "") {
                if (!Archipelago.instance.IsConnected() || (Archipelago.instance.integration.connected && (SaveFile.GetString("archipelago player name") != Archipelago.instance.GetPlayerName(Archipelago.instance.GetPlayerSlot()) || int.Parse(Archipelago.instance.integration.slotData["seed"].ToString()) != SaveFile.GetInt("seed")))) {
                    TunicRandomizer.Settings.ReadConnectionSettingsFromSaveFile();
                    Archipelago.instance.Disconnect();
                    errorMessage = Archipelago.instance.Connect();
                }
            }
            if (!Archipelago.instance.integration.connected) {
                GenericMessage.ShowMessage($"<#FF0000>[death] \"<#FF0000>warning!\" <#FF0000>[death]\n\"Failed to connect to Archipelago:\"\n{errorMessage}\n\"Returning to title screen.\"");
                return false;
            }
            return true;
        }

    }

}
