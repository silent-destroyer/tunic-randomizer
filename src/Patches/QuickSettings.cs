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
        private static bool ShowMysterySeedWindow = false;
        private static int MysteryWindowPage = 0;
        private static string stringToEdit = "";
        private static int stringCursorPosition = 0;
        private static bool showPort = false;
        private static bool showPassword = false;
        private static float guiScale = 1f;
        private static Dictionary<string, bool> editingFlags = new Dictionary<string, bool>() {
            {"Player", false},
            {"Hostname", false},
            {"Port", false},
            {"Password", false},
        };

        //Get a conenction setting value by fieldname
        private static string getConnectionSetting(string fieldName)
        {
            switch(fieldName)
            {
                case "Player":
                    return TunicRandomizer.Settings.ConnectionSettings.Player;
                case "Hostname":
                    return TunicRandomizer.Settings.ConnectionSettings.Hostname;
                case "Port":
                    return TunicRandomizer.Settings.ConnectionSettings.Port;
                case "Password":
                    return TunicRandomizer.Settings.ConnectionSettings.Password;
                default:
                    return "";
            }
        }

        //Set a conenction setting value by fieldname
        private static void setConnectionSetting(string fieldName, string value)
        {
            switch (fieldName)
            {
                case "Player":
                    TunicRandomizer.Settings.ConnectionSettings.Player = value;
                    return;
                case "Hostname":
                    TunicRandomizer.Settings.ConnectionSettings.Hostname = value;
                    return;
                case "Port":
                    TunicRandomizer.Settings.ConnectionSettings.Port = value;
                    return;
                case "Password":
                    TunicRandomizer.Settings.ConnectionSettings.Password = value;
                    return;
                default:
                    return;
            }
        }

        //Place a visible cursor in a text label when editing the field
        private static string textWithCursor(string text, bool isEditing, bool showText)
        {
            string baseText = showText ? text : new string('*', text.Length);
            if (!isEditing) return baseText;
            if (stringCursorPosition > baseText.Length) stringCursorPosition = baseText.Length;
            return baseText.Insert(stringCursorPosition, "<color=#EAA614>|</color>");
        }

        //Clear all field editing flags (since we do this in a few places)
        private static void clearAllEditingFlags()
        {

            List<string> fieldKeys = new List<string>(editingFlags.Keys);
            foreach (string fieldKey in fieldKeys)
            {
                editingFlags[fieldKey] = false;
            }
        }

        //Initialize a text field for editing
        private static void beginEditingTextField(string fieldName)
        {
            if (editingFlags[fieldName]) return; //can't begin if we're already editing this field

            //check and finalize if another field was mid-edit
            List<string> fieldKeys = new List<string>(editingFlags.Keys);
            foreach (string fieldKey in fieldKeys)
            {
                if (editingFlags[fieldKey]) finishEditingTextField(fieldKey);
            }

            stringToEdit = getConnectionSetting(fieldName);
            stringCursorPosition = stringToEdit.Length;
            GUIInput.instance.actionSet.Enabled = false; //prevent keypresses from interacting with the menu while editing
            editingFlags[fieldName] = true;
        }

        //finalize editing a text field and save the changes
        private static void finishEditingTextField(string fieldName)
        {
            if (!editingFlags[fieldName]) return; //can't finish if we're not editing this field

            stringToEdit = "";
            stringCursorPosition = 0;
            OptionsGUIPatches.SaveSettings();
            GUIInput.instance.actionSet.Enabled = true;
            editingFlags[fieldName] = false;
        }

        private static void handleEditButton(string fieldName)
        {
            if (editingFlags[fieldName])
            {
                finishEditingTextField(fieldName);
            }
            else
            {
                beginEditingTextField(fieldName);
            }
        }

        private static void handlePasteButton(string fieldName)
        {
            
            setConnectionSetting(fieldName, GUIUtility.systemCopyBuffer);
            if (editingFlags[fieldName])
            {
                stringToEdit = GUIUtility.systemCopyBuffer;
                finishEditingTextField(fieldName);
            }
            OptionsGUIPatches.SaveSettings();
        }

        private static void handleClearButton(string fieldName)
        {
            setConnectionSetting(fieldName, "");
            if (editingFlags[fieldName]) stringToEdit = "";
            OptionsGUIPatches.SaveSettings();
        }

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
                        GUI.Window(101, new Rect(20f, (float)Screen.height * 0.12f, 430f * guiScale, TunicRandomizer.Settings.MysterySeed ? 510f * guiScale : 550f * guiScale), new Action<int>(SinglePlayerQuickSettingsWindow), "Single Player Settings");
                        ShowAPSettingsWindow = false;
                        clearAllEditingFlags();
                        break;
                    case RandomizerSettings.RandomizerType.ARCHIPELAGO:
                        GUI.Window(101, new Rect(20f, (float)Screen.height * 0.12f, 430f * guiScale, 540f * guiScale), new Action<int>(ArchipelagoQuickSettingsWindow), "Archipelago Settings");
                        break;
                }

                if (ShowAPSettingsWindow && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO) {
                    GUI.Window(103, new Rect(460f * guiScale, (float)Screen.height * 0.12f, 350f * guiScale, 490f * guiScale), new Action<int>(ArchipelagoConfigEditorWindow), "Archipelago Config");
                }
                if (ShowAdvancedSinglePlayerOptions && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER && !TunicRandomizer.Settings.MysterySeed) {
                    GUI.Window(105, new Rect(460f * guiScale, (float)Screen.height * 0.12f, 405f * guiScale, 485f * guiScale), new Action<int>(AdvancedLogicOptionsWindow), "Advanced Logic Options");
                }
                if (ShowMysterySeedWindow && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER && TunicRandomizer.Settings.MysterySeed) {
                    GUI.Window(106, new Rect(460f * guiScale, (float)Screen.height * 0.12f, 405f * guiScale, 430f * guiScale), new Action<int>(MysterySeedWeightsWindow), "Mystery Seed Settings");
                }
                GameObject.Find("elderfox_sword graphic").GetComponent<Renderer>().enabled = !ShowAdvancedSinglePlayerOptions && !ShowAPSettingsWindow && !ShowMysterySeedWindow;
                if (TitleVersion.TitleButtons != null) {
                    foreach (Button button in TitleVersion.TitleButtons.GetComponentsInChildren<Button>()) {
                        button.enabled = !ShowAPSettingsWindow;
                    }
                }
            }
        }

        private void Update()
        {
            if ((TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO && ShowAPSettingsWindow) && SceneManager.GetActiveScene().name == "TitleScreen")
            {
                bool submitKeyPressed = false;
                
                //handle text input
                if (Input.anyKeyDown 
                    && !Input.GetKeyDown(KeyCode.Return) 
                    && !Input.GetKeyDown(KeyCode.Escape) 
                    && !Input.GetKeyDown(KeyCode.Tab) 
                    && !Input.GetKeyDown(KeyCode.Backspace) 
                    && !Input.GetKeyDown(KeyCode.Delete) 
                    && !Input.GetKeyDown(KeyCode.LeftArrow) 
                    && !Input.GetKeyDown(KeyCode.RightArrow)
                    && Input.inputString != ""
                    )
                {

                    bool inputValid = true;

                    //validation for any fields that require it
                    if (editingFlags["Port"] && !int.TryParse(Input.inputString, out int num)) inputValid = false;

                    if(inputValid)
                    {
                        stringToEdit = stringToEdit.Insert(stringCursorPosition, Input.inputString);
                        stringCursorPosition++;
                    }
                }

                //handle backspacing
                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    if (stringToEdit.Length > 0 && stringCursorPosition > 0)
                    {
                        stringToEdit = stringToEdit.Remove(stringCursorPosition - 1, 1);
                        stringCursorPosition--;
                    }
                }
                
                //handle delete
                if (Input.GetKeyDown(KeyCode.Delete))
                {
                    if (stringToEdit.Length > 0 && stringCursorPosition < stringToEdit.Length)
                    {
                        stringToEdit = stringToEdit.Remove(stringCursorPosition, 1);
                    }
                }
                
                //handle cursor navigation
                if (Input.GetKeyDown(KeyCode.LeftArrow) && stringCursorPosition > 0)
                {
                    stringCursorPosition--;
                }
                if (Input.GetKeyDown(KeyCode.RightArrow) && stringCursorPosition < stringToEdit.Length)
                {
                    stringCursorPosition++;
                }

                //handle Enter/Esc
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Escape))
                {
                    if (!editingFlags["Player"] && !editingFlags["Hostname"] && !editingFlags["Port"] && !editingFlags["Password"])
                    {
                        CloseAPSettingsWindow();
                    }

                    submitKeyPressed = true;
                }

                //update the relevant connection setting field
                Dictionary<string, bool> originalEditingFlags = new Dictionary<string, bool>(editingFlags);
                foreach(KeyValuePair<string,bool> editingFlag in originalEditingFlags)
                {
                    if (!editingFlag.Value) continue;
                    setConnectionSetting(editingFlag.Key, stringToEdit);
                    if (submitKeyPressed) finishEditingTextField(editingFlag.Key);
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
                bool ShowAdvancedOptions = GUI.Button(new Rect(10f * guiScale, y, 410f * guiScale, 30f * guiScale), $"Configure Mystery Seed");
                if (ShowAdvancedOptions) {
                    ShowMysterySeedWindow = !ShowMysterySeedWindow;
                    ShowAdvancedSinglePlayerOptions = false;
                }
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
                    ShowMysterySeedWindow = false;
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
            
            //Player name
            GUI.Label(new Rect(10f * guiScale, 20f * guiScale, 300f * guiScale, 30f * guiScale), $"Player: {textWithCursor(getConnectionSetting("Player"), editingFlags["Player"], true)}");
            
            bool EditPlayer = GUI.Button(new Rect(10f * guiScale, 70f * guiScale, 75f * guiScale, 30f * guiScale), editingFlags["Player"] ? "Save" : "Edit");
            if (EditPlayer) handleEditButton("Player");
            
            bool PastePlayer = GUI.Button(new Rect(100f * guiScale, 70f * guiScale, 75f * guiScale, 30f * guiScale), "Paste");
            if (PastePlayer) handlePasteButton("Player");
            
            bool ClearPlayer = GUI.Button(new Rect(190f * guiScale, 70f * guiScale, 75f * guiScale, 30f * guiScale), "Clear");
            if (ClearPlayer) handleClearButton("Player");

            //Hostname
            GUI.Label(new Rect(10f * guiScale, 120f * guiScale, 300f * guiScale, 30f * guiScale), $"Host: {textWithCursor(getConnectionSetting("Hostname"), editingFlags["Hostname"], true)}");
            
            bool setLocalhost = GUI.Toggle(new Rect(160f * guiScale, 160f * guiScale, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.ConnectionSettings.Hostname == "localhost", "localhost");
            if (setLocalhost && TunicRandomizer.Settings.ConnectionSettings.Hostname != "localhost") {
                setConnectionSetting("Hostname", "localhost");
                OptionsGUIPatches.SaveSettings();
            }
            
            bool setArchipelagoHost = GUI.Toggle(new Rect(10f * guiScale, 160f * guiScale, 140f * guiScale, 30f * guiScale), TunicRandomizer.Settings.ConnectionSettings.Hostname == "archipelago.gg", "archipelago.gg");
            if (setArchipelagoHost && TunicRandomizer.Settings.ConnectionSettings.Hostname != "archipelago.gg") {
                setConnectionSetting("Hostname", "archipelago.gg");
                OptionsGUIPatches.SaveSettings();
            }
            
            bool EditHostname = GUI.Button(new Rect(10f * guiScale, 200f * guiScale, 75f * guiScale, 30f * guiScale), editingFlags["Hostname"] ? "Save" : "Edit");
            if (EditHostname) handleEditButton("Hostname");
            
            bool PasteHostname = GUI.Button(new Rect(100f * guiScale, 200f * guiScale, 75f * guiScale, 30f * guiScale), "Paste");
            if (PasteHostname) handlePasteButton("Hostname");
            
            bool ClearHostname = GUI.Button(new Rect(190f * guiScale, 200f * guiScale, 75f * guiScale, 30f * guiScale), "Clear");
            if (ClearHostname) handleClearButton("Hostname");

            //Port
            GUI.Label(new Rect(10f * guiScale, 250f * guiScale, 300f * guiScale, 30f * guiScale), $"Port: {textWithCursor(getConnectionSetting("Port"), editingFlags["Port"], showPort)}");
            
            showPort = GUI.Toggle(new Rect(270f * guiScale, 260f * guiScale, 75f * guiScale, 30f * guiScale), showPort, "Show");
            
            bool EditPort = GUI.Button(new Rect(10f * guiScale, 300f * guiScale, 75f * guiScale, 30f * guiScale), editingFlags["Port"] ? "Save" : "Edit");
            if (EditPort) handleEditButton("Port");
            
            bool PastePort = GUI.Button(new Rect(100f * guiScale, 300f * guiScale, 75f * guiScale, 30f * guiScale), "Paste");
            if (PastePort) handlePasteButton("Port");
            
            bool ClearPort = GUI.Button(new Rect(190f * guiScale, 300f * guiScale, 75f * guiScale, 30f * guiScale), "Clear");
            if (ClearPort) handleClearButton("Port");

            //Password
            GUI.Label(new Rect(10f * guiScale, 350f * guiScale, 300f * guiScale, 30f * guiScale), $"Password: {textWithCursor(getConnectionSetting("Password"), editingFlags["Password"], showPassword)}");
            
            showPassword = GUI.Toggle(new Rect(270f * guiScale, 360f * guiScale, 75f * guiScale, 30f * guiScale), showPassword, "Show");
            bool EditPassword = GUI.Button(new Rect(10f * guiScale, 400f * guiScale, 75f * guiScale, 30f * guiScale), editingFlags["Password"] ? "Save" : "Edit");
            if (EditPassword) handleEditButton("Password");

            bool PastePassword = GUI.Button(new Rect(100f * guiScale, 400f * guiScale, 75f * guiScale, 30f * guiScale), "Paste");
            if (PastePassword) handlePasteButton("Password");
            
            bool ClearPassword = GUI.Button(new Rect(190f * guiScale, 400f * guiScale, 75f * guiScale, 30f * guiScale), "Clear");
            if (ClearPassword) handleClearButton("Password");
            
            //Close button
            bool Close = GUI.Button(new Rect(10f * guiScale, 450f * guiScale, 165f * guiScale, 30f * guiScale), "Close");
            if (Close) {
                CloseAPSettingsWindow();
                Archipelago.instance.Disconnect();
                Archipelago.instance.Connect();
            }

        }

        private static void MysterySeedWeightsWindow(int windowID) {
            GUI.skin.label.fontSize = (int)(20 * guiScale);
            GUI.skin.button.fontSize = (int)(17 * guiScale);
            float y = 25f * guiScale;
            if (MysteryWindowPage == 0) {
                GUI.Label(new Rect(10f * guiScale, y, 300f * guiScale, 30f * guiScale), $"Sword Progression");
                GUI.Label(new Rect(210f * guiScale, y, 300f * guiScale, 30f * guiScale), $"Hexagon Quest");
                y += 25f * guiScale;
                GUI.Label(new Rect(160f * guiScale, y + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.SwordProgression}%");
                TunicRandomizer.Settings.MysterySeedWeights.SwordProgression = (int)GUI.HorizontalSlider(new Rect(10f * guiScale, y + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.SwordProgression, 0, 100);

                GUI.Label(new Rect(360f * guiScale, y + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.HexagonQuest}%");
                TunicRandomizer.Settings.MysterySeedWeights.HexagonQuest = (int)GUI.HorizontalSlider(new Rect(210f * guiScale, y + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexagonQuest, 0, 100);

                y += 40f * guiScale;
                GUI.Label(new Rect(10f * guiScale, y, 300f * guiScale, 30f * guiScale), $"Keys Behind Bosses");
                GUI.Label(new Rect(210f * guiScale, y, 300f * guiScale, 30f * guiScale), $"Shuffle Abilities");
                y += 25f * guiScale;
                GUI.Label(new Rect(160f * guiScale, y + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.KeysBehindBosses}%");
                TunicRandomizer.Settings.MysterySeedWeights.KeysBehindBosses = (int)GUI.HorizontalSlider(new Rect(10f * guiScale, y + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.KeysBehindBosses, 0, 100);
                GUI.Label(new Rect(360f * guiScale, y + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.ShuffleAbilities}%");
                TunicRandomizer.Settings.MysterySeedWeights.ShuffleAbilities = (int)GUI.HorizontalSlider(new Rect(210f * guiScale, y + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.ShuffleAbilities, 0, 100);

                y += 40f * guiScale;
                GUI.Label(new Rect(10f * guiScale, y, 300f * guiScale, 30f * guiScale), $"Entrance Randomizer");
                GUI.Label(new Rect(210f * guiScale, y, 300f * guiScale, 30f * guiScale), $"ER: Fewer Shops");
                y += 25f * guiScale;
                GUI.Label(new Rect(160f * guiScale, y + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.EntranceRando}%");
                TunicRandomizer.Settings.MysterySeedWeights.EntranceRando = (int)GUI.HorizontalSlider(new Rect(10f * guiScale, y + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.EntranceRando, 0, 100);
                GUI.Label(new Rect(360f * guiScale, y + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.ERFixedShop}%");
                TunicRandomizer.Settings.MysterySeedWeights.ERFixedShop = (int)GUI.HorizontalSlider(new Rect(210f * guiScale, y + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.ERFixedShop, 0, 100);

                y += 40f * guiScale;
                GUI.Label(new Rect(10f * guiScale, y, 300f * guiScale, 30f * guiScale), $"Shuffle Ladders");
                GUI.Label(new Rect(210f * guiScale, y, 300f * guiScale, 30f * guiScale), $"Grass Randomizer");
                y += 25f * guiScale;
                GUI.Label(new Rect(160f * guiScale, y + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.ShuffleLadders}%");
                TunicRandomizer.Settings.MysterySeedWeights.ShuffleLadders = (int)GUI.HorizontalSlider(new Rect(10f * guiScale, y + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.ShuffleLadders, 0, 100);
                // Todo add grass randomizer
                GUI.Label(new Rect(360f * guiScale, y + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.ShuffleLadders}%");
                TunicRandomizer.Settings.MysterySeedWeights.ShuffleLadders = (int)GUI.HorizontalSlider(new Rect(210f * guiScale, y + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.ShuffleLadders, 0, 100);

                y += 40f * guiScale;
                GUI.Label(new Rect(10f * guiScale, y, 300f * guiScale, 30f * guiScale), $"Lanternless Logic");
                GUI.Label(new Rect(210f * guiScale, y, 300f * guiScale, 30f * guiScale), $"Maskless Logic");
                y += 25f * guiScale;
                GUI.Label(new Rect(160f * guiScale, y + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.Lanternless}%");
                TunicRandomizer.Settings.MysterySeedWeights.Lanternless = (int)GUI.HorizontalSlider(new Rect(10f * guiScale, y + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.Lanternless, 0, 100);
                GUI.Label(new Rect(360f * guiScale, y + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.Maskless}%");
                TunicRandomizer.Settings.MysterySeedWeights.Maskless = (int)GUI.HorizontalSlider(new Rect(210f * guiScale, y + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.Maskless, 0, 100);
            } else if (MysteryWindowPage == 1) {
                GUI.Label(new Rect(10f * guiScale, y, 400f * guiScale, 30f * guiScale), $"Hexagon Quest - Goal Amount");
                y += 30f * guiScale;
                TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalRandom = GUI.Toggle(new Rect(10f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalRandom, "Random");
                TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalLow = GUI.Toggle(new Rect(110f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalLow, " Low");
                TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalMedium = GUI.Toggle(new Rect(210f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalMedium, "Medium");
                TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalHigh = GUI.Toggle(new Rect(310f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalHigh, "High");

                y += 35f * guiScale;
                GUI.Label(new Rect(10f * guiScale, y, 400f * guiScale, 30f * guiScale), $"Hexagon Quest - # of Extra Hexagons");
                y += 30f * guiScale;
                TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasRandom = GUI.Toggle(new Rect(10f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasRandom, "Random");
                TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasLow = GUI.Toggle(new Rect(110f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasLow, "Low");
                TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasMedium = GUI.Toggle(new Rect(210f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasMedium, "Medium");
                TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasHigh = GUI.Toggle(new Rect(310f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasHigh, "High");

                y += 35f * guiScale;
                GUI.Label(new Rect(10f * guiScale, y, 300f * guiScale, 30f * guiScale), $"Fool Traps");
                y += 30f * guiScale;
                TunicRandomizer.Settings.MysterySeedWeights.FoolTrapNone = GUI.Toggle(new Rect(10f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.FoolTrapNone, $"None");
                TunicRandomizer.Settings.MysterySeedWeights.FoolTrapNormal = GUI.Toggle(new Rect(110f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.FoolTrapNormal, $"<color={FoolColors[1]}>{FoolChoices[1]}</color>");
                TunicRandomizer.Settings.MysterySeedWeights.FoolTrapDouble = GUI.Toggle(new Rect(210f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.FoolTrapDouble, $"<color={FoolColors[2]}>{FoolChoices[2]}</color>");
                TunicRandomizer.Settings.MysterySeedWeights.FoolTrapOnslaught = GUI.Toggle(new Rect(300f * guiScale, y, 100f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.FoolTrapOnslaught, $"<color={FoolColors[3]}>{FoolChoices[3]}</color>");

                y += 35f * guiScale;
                GUI.Label(new Rect(10f * guiScale, y, 400f * guiScale, 30f * guiScale), $"Hero's Laurels Location");
                y += 30f * guiScale;
                TunicRandomizer.Settings.MysterySeedWeights.LaurelsRandom = GUI.Toggle(new Rect(10f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.LaurelsRandom, $"Random");
                TunicRandomizer.Settings.MysterySeedWeights.LaurelsSixCoins = GUI.Toggle(new Rect(110f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.LaurelsSixCoins, $"6 Coins");
                TunicRandomizer.Settings.MysterySeedWeights.LaurelsTenCoins = GUI.Toggle(new Rect(210f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.LaurelsTenCoins, $"10 Coins");
                TunicRandomizer.Settings.MysterySeedWeights.LaurelsTenFairies = GUI.Toggle(new Rect(310f * guiScale, y, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.LaurelsTenFairies, $"10 Fairies");
            }
            GUI.skin.button.fontSize = (int)(20 * guiScale);
            y += 50f;
            bool Close = GUI.Button(new Rect(10f * guiScale, 350f * guiScale, 187.5f, 30f), "Close");
            if (Close) {
                ShowMysterySeedWindow = false;
                MysteryWindowPage = 0;
                OptionsGUIPatches.SaveSettings();
            }
            bool SwitchPage = GUI.Button(new Rect(207.5f * guiScale, 350f * guiScale, 187.5f, 30f), MysteryWindowPage == 0 ? "Next Page >" : "< Prev Page");
            if (SwitchPage) {
                MysteryWindowPage = MysteryWindowPage == 0 ? 1 : 0;
                OptionsGUIPatches.SaveSettings();
            }
            y += 40f;
            bool ResetToDefault = GUI.Button(new Rect(10f * guiScale, 390f * guiScale, 385f, 30f), "Reset To Defaults");
            if (ResetToDefault) {
                TunicRandomizer.Settings.MysterySeedWeights = new MysterySeedWeights();
                OptionsGUIPatches.SaveSettings();
            }

        }

        private static void CloseAPSettingsWindow() {
            ShowAPSettingsWindow = false;
            stringToEdit = "";
            clearAllEditingFlags();
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
            if (SaveFile.GetString("archipelago player name") != "") {
                if (SaveFile.GetString("archipelago player name") != TunicRandomizer.Settings.ConnectionSettings.Player || (Archipelago.instance.integration.connected && int.Parse(Archipelago.instance.integration.slotData["seed"].ToString()) != SaveFile.GetInt("seed"))) {
                    TunicLogger.LogInfo("Save does not match connected slot! Connected to " + TunicRandomizer.Settings.ConnectionSettings.Player + " [seed " + Archipelago.instance.integration.slotData["seed"].ToString() + "] but slot name in save file is " + SaveFile.GetString("archipelago player name") + " [seed " + SaveFile.GetInt("seed") + "]");
                    GenericMessage.ShowMessage("<#FF0000>[death] \"<#FF0000>warning!\" <#FF0000>[death]\n\"Save does not match connected slot.\"\n\"Returning to menu.\"");
                    return false;
                }
                Archipelago.instance.Connect();
                if (!Archipelago.instance.integration.connected) {
                    GenericMessage.ShowMessage("<#FF0000>[death] \"<#FF0000>warning!\" <#FF0000>[death]\n\"Failed to connect to Archipelago.\"\n\"Returning to menu.\"");
                    return false;
                }
            }
            return true;
        }

    }

}
