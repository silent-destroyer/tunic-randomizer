using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TunicRandomizer {
    public class QuickSettings : MonoBehaviour {

        private static ManualLogSource Logger = TunicRandomizer.Logger;

        public static string CustomSeed = "";
        public static Font OdinRounded;
        public static List<string> FoolChoices = new List<string>() { "Off", "Normal", "Double", "<size=19>Onslaught</size>" };
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
        
        private void OnGUI() {
            if (SceneManager.GetActiveScene().name == "TitleScreen" && GameObject.FindObjectOfType<TitleScreen>() != null) {
                GUI.skin.font = PaletteEditor.OdinRounded == null ? GUI.skin.font : PaletteEditor.OdinRounded;
                Cursor.visible = true;
                switch (TunicRandomizer.Settings.Mode) {
                    case RandomizerSettings.RandomizerType.SINGLEPLAYER:
                        GUI.Window(101, new Rect(20f, 150f, 430f, TunicRandomizer.Settings.MysterySeed ? 430f : 510f), new Action<int>(SinglePlayerQuickSettingsWindow), "Single Player Settings");
                        ShowAPSettingsWindow = false;
                        editingPlayer = false;
                        editingHostname = false;
                        editingPort = false;
                        editingPassword = false;
                        break;
                    case RandomizerSettings.RandomizerType.ARCHIPELAGO:
                        GUI.Window(101, new Rect(20f, 150f, 430f, 540f), new Action<int>(ArchipelagoQuickSettingsWindow), "Archipelago Settings");
                        break;
                }

                if (ShowAPSettingsWindow && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO) {
                    GUI.Window(103, new Rect(460f, 150f, 350f, 490f), new Action<int>(ArchipelagoConfigEditorWindow), "Archipelago Config");
                }
                if (ShowAdvancedSinglePlayerOptions && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER && !TunicRandomizer.Settings.MysterySeed) {
                    GUI.Window(105, new Rect(460f, 150f, 405f, 485f), new Action<int>(AdvancedLogicOptionsWindow), "Advanced Logic Options");
                }
                GameObject.Find("elderfox_sword graphic").GetComponent<Renderer>().enabled = !ShowAdvancedSinglePlayerOptions && !ShowAPSettingsWindow;
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
            GUI.skin.label.fontSize = 25;
            GUI.skin.button.fontSize = 20;
            GUI.skin.toggle.fontSize = 20;
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.skin.label.clipping = TextClipping.Overflow;
            GUI.color = Color.white;
            GUI.DragWindow(new Rect(500f, 50f, 500f, 30f));

            float y = 20f;

            GUI.skin.toggle.fontSize = 15; 
            GUI.skin.button.fontSize = 15;
            if (TunicRandomizer.Settings.RaceMode) {
                TunicRandomizer.Settings.RaceMode = GUI.Toggle(new Rect(330f, y, 90f, 30f), TunicRandomizer.Settings.RaceMode, "Race Mode");
            } else {
                bool ToggleSpoilerLog = GUI.Toggle(new Rect(TunicRandomizer.Settings.CreateSpoilerLog ? 280f : 330f, y, 90f, 30f), TunicRandomizer.Settings.CreateSpoilerLog, "Spoiler Log");
                TunicRandomizer.Settings.CreateSpoilerLog = ToggleSpoilerLog;
                if (ToggleSpoilerLog) {
                    GUI.skin.button.fontSize = 15;
                    bool OpenSpoilerLog = GUI.Button(new Rect(370f, y, 50f, 25f), "Open");
                    if (OpenSpoilerLog) {
                        if (File.Exists(TunicRandomizer.SpoilerLogPath)) {
                            System.Diagnostics.Process.Start(TunicRandomizer.SpoilerLogPath);
                        }
                    }
                }
            }

            GUI.skin.toggle.fontSize = 20;
            GUI.skin.button.fontSize = 20;

            GUI.Label(new Rect(10f, 20f, 300f, 30f), "Randomizer Mode");
            y += 40f;
            bool ToggleSinglePlayer = GUI.Toggle(new Rect(10f, y, 130f, 30f), TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER, "Single Player");
            if (ToggleSinglePlayer && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO) {
                TunicRandomizer.Settings.Mode = RandomizerSettings.RandomizerType.SINGLEPLAYER;
                OptionsGUIPatches.SaveSettings();
            }
            bool ToggleArchipelago = GUI.Toggle(new Rect(150f, y, 150f, 30f), TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO, "Archipelago");
            if (ToggleArchipelago && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER) {
                TunicRandomizer.Settings.Mode = RandomizerSettings.RandomizerType.ARCHIPELAGO;
                OptionsGUIPatches.SaveSettings();
            }
            y += 40f;
            GUI.Label(new Rect(10f, y, 500f, 30f), $"Player: {(TunicRandomizer.Settings.ConnectionSettings.Player)}");
            y += 40f;
            GUI.Label(new Rect(10f, y, 80f, 30f), $"Status:");
            if (Archipelago.instance.integration != null && Archipelago.instance.integration.connected) {
                GUI.color = Color.green;
                GUI.Label(new Rect(95f, y, 150f, 30f), $"Connected!");
                GUI.color = Color.white;
                GUI.Label(new Rect(250f, y, 300f, 30f), $"(world {Archipelago.instance.integration.session.ConnectionInfo.Slot} of {Archipelago.instance.integration.session.Players.Players[0].Count-1})");
            } else {
                GUI.color = Color.red;
                GUI.Label(new Rect(95f, y, 300f, 30f), $"Disconnected");
            }
            GUI.color = Color.white;
            y += 40f;
            bool Connect = GUI.Button(new Rect(10f, y, 200f, 30f), "Connect");
            if (Connect) {
                Archipelago.instance.Connect();
            }

            bool Disconnect = GUI.Button(new Rect(220f, y, 200f, 30f), "Disconnect");
            if (Disconnect) {
                Archipelago.instance.Disconnect();
            }
            y += 40f;
            bool OpenSettings = GUI.Button(new Rect(10f, y, 200f, 30f), "Open Settings File");
            if (OpenSettings) {
                try {
                    System.Diagnostics.Process.Start(TunicRandomizer.SettingsPath);
                } catch (Exception e) {
                    Logger.LogError(e);
                }
            }
            bool OpenAPSettings = GUI.Button(new Rect(220f, y, 200f, 30f), ShowAPSettingsWindow ? "Close AP Config" : "Edit AP Config");
            if (OpenAPSettings) {
                if (ShowAPSettingsWindow) {
                    CloseAPSettingsWindow();
                    Archipelago.instance.Connect();
                } else {
                    ShowAPSettingsWindow = true;
                }
            }
            y += 40f;
            GUI.Label(new Rect(10f, y, 200f, 30f), $"World Settings");
            if (Archipelago.instance.integration != null && Archipelago.instance.integration.connected) {
                Dictionary<string, object> slotData = Archipelago.instance.GetPlayerSlotData();
                y += 40f;
                GUI.Toggle(new Rect(10f, y, 180f, 30f), slotData["keys_behind_bosses"].ToString() == "1", "Keys Behind Bosses");
                GUI.Toggle(new Rect(220f, y, 210f, 30f), slotData["sword_progression"].ToString() == "1", "Sword Progression");
                y += 40f;
                GUI.Toggle(new Rect(10f, y, 175f, 30f), slotData["start_with_sword"].ToString() == "1", "Start With Sword");
                GUI.Toggle(new Rect(220f, y, 175f, 30f), slotData["ability_shuffling"].ToString() == "1", "Shuffled Abilities");
                y += 40f;
                GUI.Toggle(new Rect(10f, y, 185f, 30f), slotData["hexagon_quest"].ToString() == "1", slotData["hexagon_quest"].ToString() == "1" ? 
                    $"Hexagon Quest (<color=#E3D457>{slotData["Hexagon Quest Goal"].ToString()}</color>)" : $"Hexagon Quest");
                int FoolIndex = int.Parse(slotData["fool_traps"].ToString());
                GUI.Toggle(new Rect(220f, y, 195f, 60f), FoolIndex != 0, $"Fool Traps: {(FoolIndex == 0 ? "Off" : $"<color={FoolColors[FoolIndex]}>{FoolChoices[FoolIndex]}</color>")}");

                if (slotData.ContainsKey("entrance_rando")) {
                    y += 40f;
                    GUI.Toggle(new Rect(10f, y, 195f, 30f), slotData["entrance_rando"].ToString() == "1", $"Entrance Randomizer");
                } else {
                    y += 40f;
                    GUI.Toggle(new Rect(10f, y, 195f, 30f), false, $"Entrance Randomizer");
                }
                if (slotData.ContainsKey("shuffle_ladders")) {
                    GUI.Toggle(new Rect(220f, y, 195f, 30f), slotData["shuffle_ladders"].ToString() == "1", $"Shuffled Ladders");
                } else {
                    GUI.Toggle(new Rect(220f, y, 195f, 30f), false, $"Shuffled Ladders");
                }
            } else {
                y += 40f;
                GUI.Toggle(new Rect(10f, y, 180f, 30f), false, "Keys Behind Bosses");
                GUI.Toggle(new Rect(220f, y, 210f, 30f), false, "Sword Progression");
                y += 40f;
                GUI.Toggle(new Rect(10f, y, 175f, 30f), false, "Start With Sword");
                GUI.Toggle(new Rect(220f, y, 175f, 30f), false, "Shuffled Abilities");
                y += 40f;
                GUI.Toggle(new Rect(10f, y, 175f, 30f), false, "Hexagon Quest");
                GUI.Toggle(new Rect(220f, y, 175f, 30f), false, "Fool Traps: Off");
                y += 40f;
                GUI.Toggle(new Rect(10f, y, 195f, 30f), false, $"Entrance Randomizer");
                GUI.Toggle(new Rect(220f, y, 195f, 30f), false, $"Shuffled Ladders");
            }
            y += 40f;
            GUI.Label(new Rect(10f, y, 400f, 30f), "Other Settings <size=18>(more in options menu!)</size>");
            y += 40f;
            bool DeathLink = GUI.Toggle(new Rect(10f, y, 115f, 30f), TunicRandomizer.Settings.DeathLinkEnabled, "Death Link");
            TunicRandomizer.Settings.DeathLinkEnabled = DeathLink;
            bool EnemyRandomizer = GUI.Toggle(new Rect(150f, y, 180f, 30f), TunicRandomizer.Settings.EnemyRandomizerEnabled, "Enemy Randomizer");
            TunicRandomizer.Settings.EnemyRandomizerEnabled = EnemyRandomizer;
            GUI.skin.label.fontSize = 20;
        }

        private static void SinglePlayerQuickSettingsWindow(int windowID) {
            GUI.skin.label.fontSize = 25;
            //GUI.skin.toggle.fontSize = 25;
            //GUI.skin.toggle.alignment = TextAnchor.UpperLeft;
            GUI.skin.toggle.fontSize = 20;
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.skin.label.clipping = TextClipping.Overflow;
            GUI.color = Color.white;
            GUI.DragWindow(new Rect(500f, 50f, 500f, 30f));
            float y = 20f;

            GUI.skin.toggle.fontSize = 15;
            if (TunicRandomizer.Settings.RaceMode) {
                TunicRandomizer.Settings.RaceMode = GUI.Toggle(new Rect(330f, y, 90f, 30f), TunicRandomizer.Settings.RaceMode, "Race Mode");
            } else {
                bool ToggleSpoilerLog = GUI.Toggle(new Rect(TunicRandomizer.Settings.CreateSpoilerLog ? 280f : 330f, y, 90f, 30f), TunicRandomizer.Settings.CreateSpoilerLog, "Spoiler Log");
                TunicRandomizer.Settings.CreateSpoilerLog = ToggleSpoilerLog;
                if (ToggleSpoilerLog) {
                    GUI.skin.button.fontSize = 15;
                    bool OpenSpoilerLog = GUI.Button(new Rect(370f, y, 50f, 25f), "Open");
                    if (OpenSpoilerLog) {
                        if (File.Exists(TunicRandomizer.SpoilerLogPath)) {
                            System.Diagnostics.Process.Start(TunicRandomizer.SpoilerLogPath);
                        }
                    }
                }
            }

            GUI.skin.toggle.fontSize = 20;

            GUI.Label(new Rect(10f, 20f, 300f, 30f), "Randomizer Mode");
            y += 40f;
            bool ToggleSinglePlayer = GUI.Toggle(new Rect(10f, y, 130f, 30f), TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER, "Single Player");
            if (ToggleSinglePlayer && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO) {
                TunicRandomizer.Settings.Mode = RandomizerSettings.RandomizerType.SINGLEPLAYER;
                OptionsGUIPatches.SaveSettings();
            }
            bool ToggleArchipelago = GUI.Toggle(new Rect(150f, y, 150f, 30f), TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO, "Archipelago");
            if (ToggleArchipelago && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER) {
                TunicRandomizer.Settings.Mode = RandomizerSettings.RandomizerType.ARCHIPELAGO;
                OptionsGUIPatches.SaveSettings();
            }

            GUI.skin.toggle.fontSize = 20;
            y += 40f;
            GUI.Label(new Rect(10f, y, 200f, 30f), "Logic Settings");
            TunicRandomizer.Settings.MysterySeed = GUI.Toggle(new Rect(240f, y+5, 200f, 30f), TunicRandomizer.Settings.MysterySeed, "Mystery Seed");
            y += 45f; 
            if (TunicRandomizer.Settings.MysterySeed) {
                GUI.Label(new Rect(10f, y, 400f, 30f), "Mystery Seed Enabled!");
                GUI.skin.label.fontSize = 20;
                y += 40f;
                GUI.Label(new Rect(10f, y, 400f, 30f), "Settings will be chosen randomly on New Game.");
                y += 40f;
            } else {
                bool ToggleHexagonQuest = GUI.Toggle(new Rect(10f, y, 175f, 30f), TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST, "Hexagon Quest");
                if (ToggleHexagonQuest) {
                    TunicRandomizer.Settings.GameMode = RandomizerSettings.GameModes.HEXAGONQUEST;
                } else if (!ToggleHexagonQuest && TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST) {
                    TunicRandomizer.Settings.GameMode = RandomizerSettings.GameModes.RANDOMIZER;
                }
                TunicRandomizer.Settings.SwordProgressionEnabled = GUI.Toggle(new Rect(240f, y, 180f, 30f), TunicRandomizer.Settings.SwordProgressionEnabled, "Sword Progression");
                y += 40f; 
                TunicRandomizer.Settings.KeysBehindBosses = GUI.Toggle(new Rect(10f, y, 200f, 30f), TunicRandomizer.Settings.KeysBehindBosses, "Keys Behind Bosses");
                TunicRandomizer.Settings.ShuffleAbilities  = GUI.Toggle(new Rect(240f, y, 175f, 30f), TunicRandomizer.Settings.ShuffleAbilities, "Shuffle Abilities");
                y += 40f;
                TunicRandomizer.Settings.EntranceRandoEnabled = GUI.Toggle(new Rect(10f, y, 200f, 30f), TunicRandomizer.Settings.EntranceRandoEnabled, "Entrance Randomizer");
                TunicRandomizer.Settings.StartWithSwordEnabled = GUI.Toggle(new Rect(240f, y, 175f, 30f), TunicRandomizer.Settings.StartWithSwordEnabled, "Start With Sword");

                y += 40f;
                GUI.skin.button.fontSize = 20;
                bool ShowAdvancedOptions = GUI.Button(new Rect(10f, y, 410f, 30f), $"{(ShowAdvancedSinglePlayerOptions ? "Hide" : "Show")} Advanced Options");
                if (ShowAdvancedOptions) {
                    ShowAdvancedSinglePlayerOptions = !ShowAdvancedSinglePlayerOptions;
                }
                y += 40f;

            }
            GUI.Label(new Rect(10f, y, 400f, 30f), "Other Settings <size=18>(more in options menu!)</size>");
            y += 40f;
            TunicRandomizer.Settings.EnemyRandomizerEnabled = GUI.Toggle(new Rect(10f, y, 200f, 30f), TunicRandomizer.Settings.EnemyRandomizerEnabled, "Enemy Randomizer");
            GUI.skin.button.fontSize = 20;
            y += 40f;
            GUI.Label(new Rect(10f, y, 300f, 30f), $"Custom Seed: {(CustomSeed == "" ? "Not Set" : CustomSeed.ToString())}");
            y += 40f;
            bool GenerateSeed = GUI.Button(new Rect(10f, y, 200f, 30f), "Generate Seed");
            if (GenerateSeed) {
                CustomSeed = new System.Random().Next().ToString();
            }

            bool ClearSeed = GUI.Button(new Rect(220f, y, 200f, 30f), "Clear Seed");
            if (ClearSeed) {
                CustomSeed = "";
            }
            y += 40f;
            bool CopySettings = GUI.Button(new Rect(10f, y, 200f, 30f), "Copy Seed + Settings");
            if (CopySettings) {
                TunicRandomizer.Settings.GetSettingsString();
            }
            bool PasteSettings = GUI.Button(new Rect(220f, y, 200f, 30f), "Paste Seed + Settings");
            if (PasteSettings) {
                TunicRandomizer.Settings.ParseSettingsString(GUIUtility.systemCopyBuffer);
            }
        }

        private static void AdvancedLogicOptionsWindow(int windowID) {
            GUI.skin.label.fontSize = 25;
            float y = 20f;
            GUI.Label(new Rect(10f, y, 300f, 30f), $"Hexagon Quest");
            y += 30;
            GUI.Label(new Rect(10f, y, 220f, 20f), $"<size=18>Hexagons Required:</size>");
            GUI.Label(new Rect(190f, y, 30f, 30f), $"<size=18>{(TunicRandomizer.Settings.HexagonQuestGoal)}</size>");
            TunicRandomizer.Settings.HexagonQuestGoal = (int)GUI.HorizontalSlider(new Rect(220f, y + 15, 175f, 20f), TunicRandomizer.Settings.HexagonQuestGoal, 15, 50);
            y += 30f;
            GUI.Label(new Rect(10f, y, 220f, 30f), $"<size=18>Hexagons in Item Pool:</size>");
            GUI.Label(new Rect(190f, y, 30f, 30f), $"<size=18>{((int)Math.Round((100f + TunicRandomizer.Settings.HexagonQuestExtraPercentage) / 100f * TunicRandomizer.Settings.HexagonQuestGoal))}</size>");
            TunicRandomizer.Settings.HexagonQuestExtraPercentage = (int)GUI.HorizontalSlider(new Rect(220f, y + 15, 175f, 30f), TunicRandomizer.Settings.HexagonQuestExtraPercentage, 0, 100);
            y += 40f;
            GUI.Label(new Rect(10f, y, 300f, 30f), $"Entrance Randomizer");
            y += 40f;
            TunicRandomizer.Settings.ERFixedShop = GUI.Toggle(new Rect(10f, y, 200f, 30f), TunicRandomizer.Settings.ERFixedShop, "Fewer Shop Entrances");
            y += 40f;
            GUI.Label(new Rect(10f, y, 300f, 30f), $"Fool Traps");
            y += 40f;
            bool NoFools = GUI.Toggle(new Rect(10f, y, 90f, 30f), TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.NONE, "None");
            if (NoFools) {
                TunicRandomizer.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.NONE;
            }
            bool NormalFools = GUI.Toggle(new Rect(110f, y, 90f, 30f), TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.NORMAL, "<color=#4FF5D4>Normal</color>");
            if (NormalFools) {
                TunicRandomizer.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.NORMAL;
            }
            bool DoubleFools = GUI.Toggle(new Rect(200f, y, 90f, 30f), TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.DOUBLE, "<color=#E3D457>Double</color>");
            if (DoubleFools) {
                TunicRandomizer.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.DOUBLE;
            }
            bool OnslaughtFools = GUI.Toggle(new Rect(290f, y, 100f, 30f), TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.ONSLAUGHT, "<color=#FF3333>Onslaught</color>");
            if (OnslaughtFools) {
                TunicRandomizer.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.ONSLAUGHT;
            }
            y += 40f;
            GUI.Label(new Rect(10f, y, 300f, 30f), $"Hero's Laurels Location");
            y += 40f;
            bool RandomLaurels = GUI.Toggle(new Rect(10f, y, 90f, 30f), TunicRandomizer.Settings.FixedLaurelsOption == RandomizerSettings.FixedLaurelsType.RANDOM, "Random");
            if (RandomLaurels) {
                TunicRandomizer.Settings.FixedLaurelsOption = RandomizerSettings.FixedLaurelsType.RANDOM;
            }
            bool SixCoinsLaurels = GUI.Toggle(new Rect(110f, y, 90f, 30f), TunicRandomizer.Settings.FixedLaurelsOption == RandomizerSettings.FixedLaurelsType.SIXCOINS, "6 Coins");
            if (SixCoinsLaurels) {
                TunicRandomizer.Settings.FixedLaurelsOption = RandomizerSettings.FixedLaurelsType.SIXCOINS;
            }
            bool TenCoinsLaurels = GUI.Toggle(new Rect(200f, y, 90f, 30f), TunicRandomizer.Settings.FixedLaurelsOption == RandomizerSettings.FixedLaurelsType.TENCOINS, "10 Coins");
            if (TenCoinsLaurels) {
                TunicRandomizer.Settings.FixedLaurelsOption = RandomizerSettings.FixedLaurelsType.TENCOINS;
            }
            bool TenFairiesLaurels = GUI.Toggle(new Rect(290f, y, 100f, 30f), TunicRandomizer.Settings.FixedLaurelsOption == RandomizerSettings.FixedLaurelsType.TENFAIRIES, "10 Fairies");
            if (TenFairiesLaurels) {
                TunicRandomizer.Settings.FixedLaurelsOption = RandomizerSettings.FixedLaurelsType.TENFAIRIES;
            }
            y += 40f;
            GUI.Label(new Rect(10f, y, 300f, 30f), $"Difficulty Options");
            y += 40f;
            TunicRandomizer.Settings.Lanternless = GUI.Toggle(new Rect(10f, y, 175f, 30f), TunicRandomizer.Settings.Lanternless, "Lanternless Logic");
            TunicRandomizer.Settings.Maskless = GUI.Toggle(new Rect(195f, y, 175f, 30f), TunicRandomizer.Settings.Maskless, "Maskless Logic");
            y += 40f;
            bool Close = GUI.Button(new Rect(10f, y, 200f, 30f), "Close");
            if (Close) {
                ShowAdvancedSinglePlayerOptions = false;
                OptionsGUIPatches.SaveSettings();
            }
        }

        private static void ArchipelagoConfigEditorWindow(int windowID) {
            GUI.skin.label.fontSize = 25;
            GUI.skin.button.fontSize = 17;
            GUI.Label(new Rect(10f, 20f, 300f, 30f), $"Player: {(TunicRandomizer.Settings.ConnectionSettings.Player)}");
            bool EditPlayer = GUI.Button(new Rect(10f, 70f, 75f, 30f), editingPlayer ? "Save" : "Edit");
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
            bool PastePlayer = GUI.Button(new Rect(100f, 70f, 75f, 30f), "Paste");
            if (PastePlayer) {
                TunicRandomizer.Settings.ConnectionSettings.Player = GUIUtility.systemCopyBuffer;
                editingPlayer = false;
                OptionsGUIPatches.SaveSettings();
            }
            bool ClearPlayer = GUI.Button(new Rect(190f, 70f, 75f, 30f), "Clear");
            if (ClearPlayer) {
                if (editingPlayer) { 
                    stringToEdit = "";
                }
                TunicRandomizer.Settings.ConnectionSettings.Player = "";
                OptionsGUIPatches.SaveSettings();
            }

            GUI.Label(new Rect(10f, 120f, 300f, 30f), $"Host: {(TunicRandomizer.Settings.ConnectionSettings.Hostname)}");
            bool setLocalhost = GUI.Toggle(new Rect(160f, 160f, 90f, 30f), TunicRandomizer.Settings.ConnectionSettings.Hostname == "localhost", "localhost");
            if (setLocalhost && TunicRandomizer.Settings.ConnectionSettings.Hostname != "localhost") {
                TunicRandomizer.Settings.ConnectionSettings.Hostname = "localhost";
                OptionsGUIPatches.SaveSettings();
            }
            bool setArchipelagoHost = GUI.Toggle(new Rect(10f, 160f, 140f, 30f), TunicRandomizer.Settings.ConnectionSettings.Hostname == "archipelago.gg", "archipelago.gg");
            if (setArchipelagoHost && TunicRandomizer.Settings.ConnectionSettings.Hostname != "archipelago.gg") {
                TunicRandomizer.Settings.ConnectionSettings.Hostname = "archipelago.gg";
                OptionsGUIPatches.SaveSettings();
            }
            bool EditHostname = GUI.Button(new Rect(10f, 200f, 75f, 30f), editingHostname ? "Save" : "Edit");
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
            bool PasteHostname = GUI.Button(new Rect(100f, 200f, 75f, 30f), "Paste");
            if (PasteHostname) {
                TunicRandomizer.Settings.ConnectionSettings.Hostname = GUIUtility.systemCopyBuffer;
                editingHostname = false;
                OptionsGUIPatches.SaveSettings();
            }
            bool ClearHost = GUI.Button(new Rect(190f, 200f, 75f, 30f), "Clear");
            if (ClearHost) {
                if (editingHostname) {
                    stringToEdit = "";
                }
                TunicRandomizer.Settings.ConnectionSettings.Hostname = "";
                OptionsGUIPatches.SaveSettings();
            }

            GUI.Label(new Rect(10f, 250f, 300f, 30f), $"Port: {(editingPort ? (showPort ? stringToEdit : new string('*', stringToEdit.Length)) : (showPort ? TunicRandomizer.Settings.ConnectionSettings.Port.ToString() : new string('*', TunicRandomizer.Settings.ConnectionSettings.Port.ToString().Length)))}");
            showPort = GUI.Toggle(new Rect(270f, 260f, 75f, 30f), showPort, "Show");
            bool EditPort = GUI.Button(new Rect(10f, 300f, 75f, 30f), editingPort ? "Save" : "Edit");
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
            bool PastePort = GUI.Button(new Rect(100f, 300f, 75f, 30f), "Paste");
            if (PastePort) {
                try {
                    if (int.TryParse(GUIUtility.systemCopyBuffer, out int num)) {
                        TunicRandomizer.Settings.ConnectionSettings.Port = GUIUtility.systemCopyBuffer;
                    }
                    editingPort = false;
                    OptionsGUIPatches.SaveSettings();
                } catch (Exception e) {
                    Logger.LogError("invalid input pasted for port number!");
                }
            }
            bool ClearPort = GUI.Button(new Rect(190f, 300f, 75f, 30f), "Clear");
            if (ClearPort) {
                if (editingPort) {
                    stringToEdit = "";
                }
                TunicRandomizer.Settings.ConnectionSettings.Port = "";
                OptionsGUIPatches.SaveSettings();
            }

            GUI.Label(new Rect(10f, 350f, 300f, 30f), $"Password: {(showPassword ? TunicRandomizer.Settings.ConnectionSettings.Password : new string('*', TunicRandomizer.Settings.ConnectionSettings.Password.Length))}");
            showPassword = GUI.Toggle(new Rect(270f, 360f, 75f, 30f), showPassword, "Show");
            bool EditPassword = GUI.Button(new Rect(10f, 400f, 75f, 30f), editingPassword ? "Save" : "Edit");
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

            bool PastePassword = GUI.Button(new Rect(100f, 400f, 75f, 30f), "Paste");
            if (PastePassword) {
                TunicRandomizer.Settings.ConnectionSettings.Password = GUIUtility.systemCopyBuffer;
                editingPassword = false;
                OptionsGUIPatches.SaveSettings();
            }
            bool ClearPassword = GUI.Button(new Rect(190f, 400f, 75f, 30f), "Clear");
            if (ClearPassword) {
                if (editingPassword) {
                    stringToEdit = "";
                }
                TunicRandomizer.Settings.ConnectionSettings.Password = "";
                OptionsGUIPatches.SaveSettings();
            }
            bool Close = GUI.Button(new Rect(10f, 450f, 165f, 30f), "Close");
            if (Close) {
                CloseAPSettingsWindow();
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
            return true;
        }

        public static bool FileManagement_LoadFileAndStart_PrefixPatch(FileManagementGUI __instance, string filename) {
            CloseAPSettingsWindow();
            SaveFile.LoadFromFile(filename);
            if (SaveFile.GetInt("archipelago") == 0 && SaveFile.GetInt("randomizer") == 0) {
                Logger.LogInfo("Non-Randomizer file selected!");
                GenericMessage.ShowMessage("<#FF0000>[death] \"<#FF0000>warning!\" <#FF0000>[death]\n\"Non-Randomizer file selected.\"\n\"Returning to menu.\"");
                return false;
            }
            if (SaveFile.GetString("archipelago player name") != "") {
                if (SaveFile.GetString("archipelago player name") != TunicRandomizer.Settings.ConnectionSettings.Player || (Archipelago.instance.integration.connected && int.Parse(Archipelago.instance.integration.slotData["seed"].ToString()) != SaveFile.GetInt("seed"))) {
                    Logger.LogInfo("Save does not match connected slot! Connected to " + TunicRandomizer.Settings.ConnectionSettings.Player + " [seed " + Archipelago.instance.integration.slotData["seed"].ToString() + "] but slot name in save file is " + SaveFile.GetString("archipelago player name") + " [seed " + SaveFile.GetInt("seed") + "]");
                    GenericMessage.ShowMessage("<#FF0000>[death] \"<#FF0000>warning!\" <#FF0000>[death]\n\"Save does not match connected slot.\"\n\"Returning to menu.\"");
                    return false;
                }
            }
            return true;
        }

    }

}
