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
        private static float advHeight = 0f;
        private static float mystHeight = 0f;
        private static Dictionary<string, bool> editingFlags = new Dictionary<string, bool>() {
            {"Player", false},
            {"Hostname", false},
            {"Port", false},
            {"Password", false},
        };

        // For showing option tooltips
        private static string hoveredOption = "";
        private static Dictionary<string, Rect> windowRects = new Dictionary<string, Rect>() {
            { "singlePlayer", new Rect() },
            { "advancedSinglePlayer", new Rect() },
            { "mysterySeed", new Rect() },
            { "archipelago", new Rect() },
            { "archipelagoConfig", new Rect() },
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
            RandomizerSettings.SaveSettings();
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
            RandomizerSettings.SaveSettings();
        }

        private static void handleClearButton(string fieldName)
        {
            setConnectionSetting(fieldName, "");
            if (editingFlags[fieldName]) stringToEdit = "";
            RandomizerSettings.SaveSettings();
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
                        windowRects["singlePlayer"] = new Rect(20f, (float)Screen.height * 0.12f, 430f * guiScale, TunicRandomizer.Settings.MysterySeed ? 510f * guiScale : 550f * guiScale);
                        GUI.Window(101, windowRects["singlePlayer"], new Action<int>(SinglePlayerQuickSettingsWindow), "Single Player Settings");
                        ShowAPSettingsWindow = false;
                        clearAllEditingFlags();
                        break;
                    case RandomizerSettings.RandomizerType.ARCHIPELAGO:
                        windowRects["archipelago"] = new Rect(20f, (float)Screen.height * 0.12f, 430f * guiScale, 540f * guiScale);
                        GUI.Window(101, windowRects["archipelago"], new Action<int>(ArchipelagoQuickSettingsWindow), "Archipelago Settings");
                        break;
                }

                if (ShowAPSettingsWindow && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO) {
                    windowRects["archipelagoConfig"] = new Rect(460f * guiScale, (float)Screen.height * 0.12f, 350f * guiScale, 490f * guiScale);
                    GUI.Window(103, windowRects["archipelagoConfig"], new Action<int>(ArchipelagoConfigEditorWindow), "Archipelago Connection");
                }
                if (ShowAdvancedSinglePlayerOptions && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER && !TunicRandomizer.Settings.MysterySeed) {
                    windowRects["advancedSinglePlayer"] = new Rect(460f * guiScale, (float)Screen.height * 0.12f, 405f * guiScale, advHeight * guiScale);
                    GUI.Window(105, windowRects["advancedSinglePlayer"], new Action<int>(AdvancedLogicOptionsWindow), "Advanced Logic Options");
                }
                if (ShowMysterySeedWindow && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER && TunicRandomizer.Settings.MysterySeed) {
                    windowRects["mysterySeed"] = new Rect(460f * guiScale, (float)Screen.height * 0.12f, 405f * guiScale, mystHeight * guiScale);
                    GUI.Window(106, windowRects["mysterySeed"], new Action<int>(MysterySeedWeightsWindow), "Mystery Seed Settings");
                }
                if (hoveredOption != "" && TunicRandomizer.Settings.OptionTooltips) {
                    GUI.Window(107, new Rect(20f, (float)Screen.height - (110f * guiScale), 1000f * guiScale, 110f * guiScale), new Action<int>(TooltipWindow), hoveredOption);
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

        private static Rect ShowTooltip(Rect rect, string window, string option) {
            bool hovered = CombineRects(windowRects[window], rect).Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y));
            if (hovered && Tooltips.OptionTooltips.ContainsKey(option)) {
                hoveredOption = option;
            }
            if (hoveredOption == option && !hovered) {
                hoveredOption = "";
            }
            return rect;
        }

        private static void TooltipWindow(int windowID) {
            GUI.skin.label.fontSize = (int)(22f * guiScale);
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.skin.label.clipping = TextClipping.Overflow;
            GUI.color = Color.white;
            GUI.DragWindow(new Rect(0, Screen.height - 100, 1000f * guiScale, 80 * guiScale));
            GUI.Label(new Rect(10f * guiScale, 20f * guiScale, 980f * guiScale, 100 * guiScale), Tooltips.OptionTooltips.ContainsKey(hoveredOption) ? Tooltips.OptionTooltips[hoveredOption] : hoveredOption);
        }

        private static Rect CombineRects(Rect r1, Rect r2) {
            Rect r3 = r1;
            r3.x += r2.x;
            r3.y += r2.y;
            r3.height = r2.height;
            r3.width = r2.width;
            return r3;
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
                RandomizerSettings.SaveSettings();
            }
            bool ToggleArchipelago = GUI.Toggle(new Rect(150f * guiScale, y, 150f * guiScale, 30f * guiScale), TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO, "Archipelago");
            if (ToggleArchipelago && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER) {
                TunicRandomizer.Settings.Mode = RandomizerSettings.RandomizerType.ARCHIPELAGO;
                RandomizerSettings.SaveSettings();
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
            bool Connection = GUI.Button(new Rect(10f * guiScale, y, 160f * guiScale, 30f * guiScale), Archipelago.instance.IsConnected() ? "Disconnect" : "Connect");
            if (Connection) {
                if (Archipelago.instance.IsConnected()) {
                    Archipelago.instance.Disconnect();
                } else {
                    Archipelago.instance.Connect();
                }
            }

            bool OpenAPSettings = GUI.Button(new Rect(180f * guiScale, y, 240f * guiScale, 30f * guiScale), ShowAPSettingsWindow ? "Close Connection Info" : "Edit Connection Info");
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

                y += 40f * guiScale;
                
                if (slotData.ContainsKey("entrance_rando")) {
                    GUI.Toggle(new Rect(10f * guiScale, y, 195f * guiScale, 30f * guiScale), slotData["entrance_rando"].ToString() == "1", $"Entrance Randomizer");
                } else {
                    GUI.Toggle(new Rect(10f * guiScale, y, 195f * guiScale, 30f * guiScale), false, $"Entrance Randomizer");
                }
                if (slotData.ContainsKey("shuffle_ladders")) {
                    GUI.Toggle(new Rect(220f * guiScale, y, 195f * guiScale, 30f * guiScale), slotData["shuffle_ladders"].ToString() == "1", $"Shuffled Ladders");
                } else {
                    GUI.Toggle(new Rect(220f * guiScale, y, 195f * guiScale, 30f * guiScale), false, $"Shuffled Ladders");
                }

                y += 40f * guiScale;
             
                if (slotData.ContainsKey("grass_randomizer")) {
                    GUI.Toggle(new Rect(10f * guiScale, y, 195f * guiScale, 30f * guiScale), slotData["grass_randomizer"].ToString() == "1", $"Grass Randomizer");
                } else {
                    GUI.Toggle(new Rect(10f * guiScale, y, 195f * guiScale, 30f * guiScale), false, $"Grass Randomizer");
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
                y += 40f * guiScale;
                GUI.Toggle(new Rect(10f * guiScale, y, 195f * guiScale, 30f * guiScale), false, $"Grass Randomizer");

            }
            y += 40f * guiScale;
            GUI.Label(new Rect(10f * guiScale, y, 400f * guiScale, 30f * guiScale), $"Other Settings <size={(int)(18 * guiScale)}>(see in-game options menu!)</size>");
            y += 40f * guiScale;
            TunicRandomizer.Settings.DeathLinkEnabled = GUI.Toggle(ShowTooltip(new Rect(10f * guiScale, y, 105f * guiScale, 30f * guiScale), "archipelago", "Death Link"), TunicRandomizer.Settings.DeathLinkEnabled, "Death Link");
            TunicRandomizer.Settings.EnemyRandomizerEnabled = GUI.Toggle(ShowTooltip(new Rect(120f * guiScale, y, 170f * guiScale, 30f * guiScale), "archipelago", "Enemy Randomizer"), TunicRandomizer.Settings.EnemyRandomizerEnabled, "Enemy Randomizer");
            TunicRandomizer.Settings.MusicShuffle = GUI.Toggle(ShowTooltip(new Rect(295f * guiScale, y, 130f * guiScale, 30f * guiScale), "archipelago", "Music Shuffle"), TunicRandomizer.Settings.MusicShuffle, "Music Shuffle");
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
                RandomizerSettings.SaveSettings();
            }
            bool ToggleArchipelago = GUI.Toggle(new Rect(150f * guiScale, y, 150f * guiScale, 30f * guiScale), TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO, "Archipelago");
            if (ToggleArchipelago && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER) {
                TunicRandomizer.Settings.Mode = RandomizerSettings.RandomizerType.ARCHIPELAGO;
                RandomizerSettings.SaveSettings();
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
                TunicRandomizer.Settings.StartWithSwordEnabled = GUI.Toggle(ShowTooltip(new Rect(10f * guiScale, y, 175f * guiScale, 30f * guiScale), "singlePlayer", "Start With Sword"), TunicRandomizer.Settings.StartWithSwordEnabled, "Start With Sword");
                TunicRandomizer.Settings.MysterySeed = GUI.Toggle(ShowTooltip(new Rect(240f * guiScale, y, 200f * guiScale, 30f * guiScale), "singlePlayer", "Mystery Seed"), TunicRandomizer.Settings.MysterySeed, "Mystery Seed");
                y += 40f * guiScale;
                bool ShowAdvancedOptions = GUI.Button(new Rect(10f * guiScale, y, 410f * guiScale, 30f * guiScale), $"Configure Mystery Seed");
                if (ShowAdvancedOptions) {
                    ShowMysterySeedWindow = !ShowMysterySeedWindow;
                    ShowAdvancedSinglePlayerOptions = false;
                }
                y += 40f * guiScale;
            } else {
                bool ToggleHexagonQuest = GUI.Toggle(ShowTooltip(new Rect(10f * guiScale, y, 185f * guiScale, 30f * guiScale), "singlePlayer", "Hexagon Quest"), TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST, $"Hexagon Quest {(TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST ? $"(<color=#E3D457>{(TunicRandomizer.Settings.RandomizeHexQuest ? "?" : TunicRandomizer.Settings.HexagonQuestGoal.ToString())}</color>)" : "")}");
                if (ToggleHexagonQuest) {
                    TunicRandomizer.Settings.GameMode = RandomizerSettings.GameModes.HEXAGONQUEST;
                } else if (!ToggleHexagonQuest && TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST) {
                    TunicRandomizer.Settings.GameMode = RandomizerSettings.GameModes.RANDOMIZER;
                }
                TunicRandomizer.Settings.SwordProgressionEnabled = GUI.Toggle(ShowTooltip(new Rect(240f * guiScale, y, 180f * guiScale, 30f * guiScale), "singlePlayer", "Sword Progression"), TunicRandomizer.Settings.SwordProgressionEnabled, "Sword Progression");
                y += 40f * guiScale; 
                TunicRandomizer.Settings.KeysBehindBosses = GUI.Toggle(ShowTooltip(new Rect(10f * guiScale, y, 200f * guiScale, 30f * guiScale), "singlePlayer", "Keys Behind Bosses"), TunicRandomizer.Settings.KeysBehindBosses, "Keys Behind Bosses");
                TunicRandomizer.Settings.ShuffleAbilities  = GUI.Toggle(ShowTooltip(new Rect(240f * guiScale, y, 175f * guiScale, 30f * guiScale), "singlePlayer", "Shuffle Abilities"), TunicRandomizer.Settings.ShuffleAbilities, "Shuffle Abilities");
                y += 40f * guiScale;
                TunicRandomizer.Settings.EntranceRandoEnabled = GUI.Toggle(ShowTooltip(new Rect(10f * guiScale, y, 200f * guiScale, 30f * guiScale), "singlePlayer", "Entrance Randomizer"), TunicRandomizer.Settings.EntranceRandoEnabled, "Entrance Randomizer"); 
                TunicRandomizer.Settings.ShuffleLadders = GUI.Toggle(ShowTooltip(new Rect(240f * guiScale, y, 200f * guiScale, 30f * guiScale), "singlePlayer", "Shuffle Ladders"), TunicRandomizer.Settings.ShuffleLadders, "Shuffle Ladders");
                y += 40f * guiScale;
                TunicRandomizer.Settings.StartWithSwordEnabled = GUI.Toggle(ShowTooltip(new Rect(10f * guiScale, y, 175f * guiScale, 30f * guiScale), "singlePlayer", "Start With Sword"), TunicRandomizer.Settings.StartWithSwordEnabled, "Start With Sword");
                TunicRandomizer.Settings.MysterySeed = GUI.Toggle(ShowTooltip(new Rect(240f * guiScale, y, 200f * guiScale, 30f * guiScale), "singlePlayer", "Mystery Seed"), TunicRandomizer.Settings.MysterySeed, "Mystery Seed");
                y += 40f * guiScale;
                GUI.skin.button.fontSize = (int)(20 * guiScale);
                bool ShowAdvancedOptions = GUI.Button(new Rect(10f * guiScale, y, 410f * guiScale, 30f * guiScale), $"{(ShowAdvancedSinglePlayerOptions ? "Hide" : "Show")} Advanced Options");
                if (ShowAdvancedOptions) {
                    ShowAdvancedSinglePlayerOptions = !ShowAdvancedSinglePlayerOptions;
                    ShowMysterySeedWindow = false;
                }
                y += 40f * guiScale;

            }
            GUI.Label(new Rect(10f * guiScale, y, 400f * guiScale, 30f * guiScale), $"Other Settings <size={(int)(18 * guiScale)}>(see in-game options menu!)</size>");
            y += 40f * guiScale;
            TunicRandomizer.Settings.EnemyRandomizerEnabled = GUI.Toggle(ShowTooltip(new Rect(10f * guiScale, y, 180f * guiScale, 30f * guiScale), "singlePlayer", "Enemy Randomizer"), TunicRandomizer.Settings.EnemyRandomizerEnabled, "Enemy Randomizer");
            TunicRandomizer.Settings.MusicShuffle = GUI.Toggle(ShowTooltip(new Rect(210f * guiScale, y, 150f * guiScale, 30f * guiScale), "singlePlayer", "Music Shuffle"), TunicRandomizer.Settings.MusicShuffle, "Music Shuffle");
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
            advHeight = 20f * guiScale;
            GUI.Label(new Rect(10f * guiScale, advHeight, 300f * guiScale, 30f * guiScale), $"Hexagon Quest");
            GUI.skin.toggle.fontSize = (int)(15 * guiScale);
            advHeight += 30 * guiScale;
            if (TunicRandomizer.Settings.RandomizeHexQuest) {
                GUI.Label(new Rect(10f * guiScale, advHeight, 220f * guiScale, 20f * guiScale), $"<size={(int)(18 * guiScale)}>Hexagons Required:</size>");
                ShowTooltip(new Rect(10f * guiScale, advHeight, 385f * guiScale, 70f * guiScale), "advancedSinglePlayer", "Randomize Hexagon Quest Amounts");
                bool RandomGoal = GUI.Toggle(new Rect(165f * guiScale, advHeight + 10f, 70f * guiScale, 30f * guiScale), TunicRandomizer.Settings.HexagonQuestRandomGoal == RandomizerSettings.HexQuestValue.RANDOM, "Random");
                if (RandomGoal) {
                    TunicRandomizer.Settings.HexagonQuestRandomGoal = RandomizerSettings.HexQuestValue.RANDOM;
                }
                bool LowGoal = GUI.Toggle(new Rect(235f * guiScale, advHeight + 10f, 50f * guiScale, 30f * guiScale), TunicRandomizer.Settings.HexagonQuestRandomGoal == RandomizerSettings.HexQuestValue.LOW, "Low");
                if (LowGoal) {
                    TunicRandomizer.Settings.HexagonQuestRandomGoal = RandomizerSettings.HexQuestValue.LOW;
                }
                bool MediumGoal = GUI.Toggle(new Rect(285f * guiScale, advHeight + 10f, 60f * guiScale, 30f * guiScale), TunicRandomizer.Settings.HexagonQuestRandomGoal == RandomizerSettings.HexQuestValue.MEDIUM, "Medium");
                if (MediumGoal) {
                    TunicRandomizer.Settings.HexagonQuestRandomGoal = RandomizerSettings.HexQuestValue.MEDIUM;
                }
                bool HighGoal = GUI.Toggle(new Rect(355f * guiScale, advHeight + 10f, 50f * guiScale, 30f * guiScale), TunicRandomizer.Settings.HexagonQuestRandomGoal == RandomizerSettings.HexQuestValue.HIGH, "High");
                if (HighGoal) {
                    TunicRandomizer.Settings.HexagonQuestRandomGoal = RandomizerSettings.HexQuestValue.HIGH;
                }
                advHeight += 30f;
                GUI.Label(new Rect(10f * guiScale, advHeight, 220f * guiScale, 30f * guiScale), $"<size={(int)(18 * guiScale)}>Extra Hexagons:</size>");
                bool RandomExtras = GUI.Toggle(new Rect(165f * guiScale, advHeight + 10f, 70f * guiScale, 30f * guiScale), TunicRandomizer.Settings.HexagonQuestRandomExtras == RandomizerSettings.HexQuestValue.RANDOM, "Random");
                if (RandomExtras) {
                    TunicRandomizer.Settings.HexagonQuestRandomExtras = RandomizerSettings.HexQuestValue.RANDOM;
                }
                bool LowExtras = GUI.Toggle(new Rect(235f * guiScale, advHeight + 10f, 50f * guiScale, 30f * guiScale), TunicRandomizer.Settings.HexagonQuestRandomExtras == RandomizerSettings.HexQuestValue.LOW, "Low");
                if (LowExtras) {
                    TunicRandomizer.Settings.HexagonQuestRandomExtras = RandomizerSettings.HexQuestValue.LOW;
                }
                bool MediumExtras = GUI.Toggle(new Rect(285f * guiScale, advHeight + 10f, 60f * guiScale, 30f * guiScale), TunicRandomizer.Settings.HexagonQuestRandomExtras == RandomizerSettings.HexQuestValue.MEDIUM, "Medium");
                if (MediumExtras) {
                    TunicRandomizer.Settings.HexagonQuestRandomExtras = RandomizerSettings.HexQuestValue.MEDIUM;
                }
                bool HighExtras = GUI.Toggle(new Rect(355f * guiScale, advHeight + 10f, 50f * guiScale, 30f * guiScale), TunicRandomizer.Settings.HexagonQuestRandomExtras == RandomizerSettings.HexQuestValue.HIGH, "High");
                if (HighExtras) {
                    TunicRandomizer.Settings.HexagonQuestRandomExtras = RandomizerSettings.HexQuestValue.HIGH;
                }

            } else {
                ShowTooltip(new Rect(10f, advHeight, 385f * guiScale, 30f * guiScale), "advancedSinglePlayer", "Hexagons Required");
                GUI.Label(new Rect(10f * guiScale, advHeight, 220f * guiScale, 20f * guiScale), $"<size={(int)(18 * guiScale)}>Hexagons Required:</size>");
                GUI.Label(new Rect(190f * guiScale, advHeight, 30f * guiScale, 30f * guiScale), $"<size={(int)(18 * guiScale)}>{(TunicRandomizer.Settings.HexagonQuestGoal)}</size>");
                TunicRandomizer.Settings.HexagonQuestGoal = (int)GUI.HorizontalSlider(new Rect(220f * guiScale, advHeight + 15, 175f * guiScale, 20f * guiScale), TunicRandomizer.Settings.HexagonQuestGoal, 1, 100);
                advHeight += 30f * guiScale;
                ShowTooltip(new Rect(10f, advHeight, 385f * guiScale, 30f * guiScale), "advancedSinglePlayer", "Hexagons in Item Pool");
                GUI.Label(new Rect(10f * guiScale, advHeight, 220f * guiScale, 30f * guiScale), $"<size={(int)(18 * guiScale)}>Hexagons in Item Pool:</size>");
                GUI.Label(new Rect(190f * guiScale, advHeight, 30f * guiScale, 30f * guiScale), $"<size={(int)(18 * guiScale)}>{(Math.Min((int)Math.Round((100f + TunicRandomizer.Settings.HexagonQuestExtraPercentage) / 100f * TunicRandomizer.Settings.HexagonQuestGoal), 100))}</size>");
                TunicRandomizer.Settings.HexagonQuestExtraPercentage = (int)GUI.HorizontalSlider(new Rect(220f * guiScale, advHeight + 15, 175f * guiScale, 30f * guiScale), TunicRandomizer.Settings.HexagonQuestExtraPercentage, 0, 100);
            }
            advHeight += 30f;
            GUI.skin.toggle.fontSize = (int)(20 * guiScale);
            TunicRandomizer.Settings.RandomizeHexQuest = GUI.Toggle(new Rect(10f * guiScale, advHeight, 300f, 30f), TunicRandomizer.Settings.RandomizeHexQuest, $"Randomize # of Gold Hexagons");
            advHeight += 30f * guiScale;
            if (TunicRandomizer.Settings.ShuffleAbilities) {
                GUI.skin.label.fontSize = (int)(20 * guiScale);
                GUI.Label(new Rect(10f, advHeight, 200f * guiScale, 30f * guiScale), "Ability Shuffle Mode:");
                ShowTooltip(new Rect(10f, advHeight, 385f * guiScale, 30f * guiScale), "advancedSinglePlayer", "Hexagon Quest Ability Shuffle Mode");
                TunicRandomizer.Settings.HexQuestAbilitiesUnlockedByPages = !GUI.Toggle(new Rect(200f, advHeight, 120f, 30f), !TunicRandomizer.Settings.HexQuestAbilitiesUnlockedByPages, "Hexagons");
                TunicRandomizer.Settings.HexQuestAbilitiesUnlockedByPages = GUI.Toggle(new Rect(310f, advHeight, 90f, 30f), TunicRandomizer.Settings.HexQuestAbilitiesUnlockedByPages, "Pages");
                advHeight += 40f * guiScale;
            }
            GUI.skin.label.fontSize = (int)(25 * guiScale);
            GUI.Label(new Rect(10f * guiScale, advHeight, 300f * guiScale, 30f * guiScale), $"Entrance Randomizer");
            advHeight += 40f * guiScale;
            TunicRandomizer.Settings.ERFixedShop = GUI.Toggle(ShowTooltip(new Rect(10f * guiScale, advHeight, 120f * guiScale, 30f * guiScale), "advancedSinglePlayer", "Fewer Shops"), TunicRandomizer.Settings.ERFixedShop, "Fewer Shops");
            if (TunicRandomizer.Settings.ERFixedShop) {
                TunicRandomizer.Settings.PortalDirectionPairs = false;
            }
            TunicRandomizer.Settings.PortalDirectionPairs = GUI.Toggle(ShowTooltip(new Rect(195f * guiScale, advHeight, 190f * guiScale, 30f * guiScale), "advancedSinglePlayer", "Matching Directions"), TunicRandomizer.Settings.PortalDirectionPairs, "Matching Directions");
            if (TunicRandomizer.Settings.PortalDirectionPairs) {
                TunicRandomizer.Settings.ERFixedShop = false;
            }
            advHeight += 40f * guiScale;
            TunicRandomizer.Settings.DecoupledER = GUI.Toggle(ShowTooltip(new Rect(10f * guiScale, advHeight, 200f * guiScale, 30f * guiScale), "advancedSinglePlayer", "Decoupled Entrances"), TunicRandomizer.Settings.DecoupledER, "Decoupled Entrances");
            advHeight += 40f * guiScale;
            GUI.skin.label.fontSize = (int)(25 * guiScale);
            GUI.Label(new Rect(10f * guiScale, advHeight, 300f * guiScale, 30f * guiScale), $"Grass Randomizer");
            advHeight += 40f * guiScale;
            TunicRandomizer.Settings.GrassRandomizer = GUI.Toggle(ShowTooltip(new Rect(10f * guiScale, advHeight, 175f * guiScale, 30f * guiScale), "advancedSinglePlayer", "Grass Randomizer"), TunicRandomizer.Settings.GrassRandomizer, "Grass Randomizer");
            TunicRandomizer.Settings.ClearEarlyBushes = GUI.Toggle(ShowTooltip(new Rect(195f * guiScale, advHeight, 195f * guiScale, 30f * guiScale), "advancedSinglePlayer", "Clear Early Bushes"), TunicRandomizer.Settings.ClearEarlyBushes, "Clear Early Bushes");
            advHeight += 40f * guiScale;
            GUI.Label(new Rect(10f * guiScale, advHeight, 300f * guiScale, 30f * guiScale), $"Fool Traps");
            ShowTooltip(new Rect(10f * guiScale, advHeight, 385f * guiScale, 100f * guiScale), "advancedSinglePlayer", "Fool Traps");
            advHeight += 40f * guiScale;
            bool NoFools = GUI.Toggle(new Rect(10f * guiScale, advHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.NONE, "None");
            if (NoFools) {
                TunicRandomizer.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.NONE;
            }
            bool NormalFools = GUI.Toggle(new Rect(110f * guiScale, advHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.NORMAL, "<color=#4FF5D4>Normal</color>");
            if (NormalFools) {
                TunicRandomizer.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.NORMAL;
            }
            bool DoubleFools = GUI.Toggle(new Rect(200f * guiScale, advHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.DOUBLE, "<color=#E3D457>Double</color>");
            if (DoubleFools) {
                TunicRandomizer.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.DOUBLE;
            }
            bool OnslaughtFools = GUI.Toggle(new Rect(290f * guiScale, advHeight, 100f * guiScale, 30f * guiScale), TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.ONSLAUGHT, "<color=#FF3333>Onslaught</color>");
            if (OnslaughtFools) {
                TunicRandomizer.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.ONSLAUGHT;
            }
            advHeight += 40f * guiScale;
            GUI.Label(new Rect(10f * guiScale, advHeight, 300f * guiScale, 30f * guiScale), $"Hero's Laurels Location");
            ShowTooltip(new Rect(10f * guiScale, advHeight, 385f * guiScale, 100f * guiScale), "advancedSinglePlayer", "Hero's Laurels Location");
            advHeight += 40f * guiScale;
            bool RandomLaurels = GUI.Toggle(new Rect(10f * guiScale, advHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.FixedLaurelsOption == RandomizerSettings.FixedLaurelsType.RANDOM, "Random");
            if (RandomLaurels) {
                TunicRandomizer.Settings.FixedLaurelsOption = RandomizerSettings.FixedLaurelsType.RANDOM;
            }
            bool SixCoinsLaurels = GUI.Toggle(new Rect(110f * guiScale, advHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.FixedLaurelsOption == RandomizerSettings.FixedLaurelsType.SIXCOINS, "6 Coins");
            if (SixCoinsLaurels) {
                TunicRandomizer.Settings.FixedLaurelsOption = RandomizerSettings.FixedLaurelsType.SIXCOINS;
            }
            bool TenCoinsLaurels = GUI.Toggle(new Rect(200f * guiScale, advHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.FixedLaurelsOption == RandomizerSettings.FixedLaurelsType.TENCOINS, "10 Coins");
            if (TenCoinsLaurels) {
                TunicRandomizer.Settings.FixedLaurelsOption = RandomizerSettings.FixedLaurelsType.TENCOINS;
            }
            bool TenFairiesLaurels = GUI.Toggle(new Rect(290f * guiScale, advHeight, 100f * guiScale, 30f * guiScale), TunicRandomizer.Settings.FixedLaurelsOption == RandomizerSettings.FixedLaurelsType.TENFAIRIES, "10 Fairies");
            if (TenFairiesLaurels) {
                TunicRandomizer.Settings.FixedLaurelsOption = RandomizerSettings.FixedLaurelsType.TENFAIRIES;
            }
            advHeight += 40f * guiScale;
            GUI.Label(new Rect(10f * guiScale, advHeight, 300f * guiScale, 30f * guiScale), $"Difficulty Options");
            advHeight += 40f * guiScale;
            TunicRandomizer.Settings.Lanternless = GUI.Toggle(ShowTooltip(new Rect(10f * guiScale, advHeight, 175f * guiScale, 30f * guiScale), "advancedSinglePlayer", "Lanternless Logic"), TunicRandomizer.Settings.Lanternless, "Lanternless Logic");
            TunicRandomizer.Settings.Maskless = GUI.Toggle(ShowTooltip(new Rect(195f * guiScale, advHeight, 175f * guiScale, 30f * guiScale), "advancedSinglePlayer", "Maskless Logic"), TunicRandomizer.Settings.Maskless, "Maskless Logic");
            advHeight += 40f * guiScale;
            bool Close = GUI.Button(new Rect(10f * guiScale, advHeight, 200f * guiScale, 30f * guiScale), "Close");
            if (Close) {
                ShowAdvancedSinglePlayerOptions = false;
                RandomizerSettings.SaveSettings();
            }
            advHeight += 40f * guiScale;
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
                RandomizerSettings.SaveSettings();
            }
            
            bool setArchipelagoHost = GUI.Toggle(new Rect(10f * guiScale, 160f * guiScale, 140f * guiScale, 30f * guiScale), TunicRandomizer.Settings.ConnectionSettings.Hostname == "archipelago.gg", "archipelago.gg");
            if (setArchipelagoHost && TunicRandomizer.Settings.ConnectionSettings.Hostname != "archipelago.gg") {
                setConnectionSetting("Hostname", "archipelago.gg");
                RandomizerSettings.SaveSettings();
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
            mystHeight = 25f * guiScale;
            if (MysteryWindowPage == 0) {
                ShowTooltip(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Sword Progression");
                ShowTooltip(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Hexagon Quest");
                GUI.Label(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Sword Progression");
                GUI.Label(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Hexagon Quest");
                mystHeight += 25f * guiScale;
                GUI.Label(new Rect(160f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.SwordProgression}%");
                TunicRandomizer.Settings.MysterySeedWeights.SwordProgression = (int)GUI.HorizontalSlider(new Rect(10f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.SwordProgression, 0, 100);

                GUI.Label(new Rect(360f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.HexagonQuest}%");
                TunicRandomizer.Settings.MysterySeedWeights.HexagonQuest = (int)GUI.HorizontalSlider(new Rect(210f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexagonQuest, 0, 100);

                mystHeight += 40f * guiScale;
                ShowTooltip(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Keys Behind Bosses");
                ShowTooltip(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Shuffle Abilities");
                GUI.Label(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Keys Behind Bosses");
                GUI.Label(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Shuffle Abilities");
                mystHeight += 25f * guiScale;
                GUI.Label(new Rect(160f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.KeysBehindBosses}%");
                TunicRandomizer.Settings.MysterySeedWeights.KeysBehindBosses = (int)GUI.HorizontalSlider(new Rect(10f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.KeysBehindBosses, 0, 100);
                GUI.Label(new Rect(360f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.ShuffleAbilities}%");
                TunicRandomizer.Settings.MysterySeedWeights.ShuffleAbilities = (int)GUI.HorizontalSlider(new Rect(210f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.ShuffleAbilities, 0, 100);

                mystHeight += 40f * guiScale;
                ShowTooltip(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Entrance Randomizer");
                ShowTooltip(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Decoupled Entrances");
                GUI.Label(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Entrance Randomizer");
                GUI.skin.label.fontSize = (int)(18 * guiScale);
                GUI.Label(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"ER: Decoupled Entrances");
                GUI.skin.label.fontSize = (int)(20 * guiScale);
                mystHeight += 25f * guiScale;
                GUI.Label(new Rect(160f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.EntranceRando}%");
                TunicRandomizer.Settings.MysterySeedWeights.EntranceRando = (int)GUI.HorizontalSlider(new Rect(10f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.EntranceRando, 0, 100);
                GUI.Label(new Rect(360f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.ERDecoupled}%");
                TunicRandomizer.Settings.MysterySeedWeights.ERDecoupled = (int)GUI.HorizontalSlider(new Rect(210f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.ERDecoupled, 0, 100);
                
                mystHeight += 40f * guiScale;
                ShowTooltip(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Fewer Shops");
                ShowTooltip(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Matching Directions");
                GUI.Label(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"ER: Fewer Shops");
                GUI.Label(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"ER: Matching Directions");
                mystHeight += 25f * guiScale;
                GUI.Label(new Rect(160f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.ERFixedShop}%");
                TunicRandomizer.Settings.MysterySeedWeights.ERFixedShop = (int)GUI.HorizontalSlider(new Rect(10f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.ERFixedShop, 0, 100);
                GUI.Label(new Rect(360f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.ERDirectionPairs}%");
                TunicRandomizer.Settings.MysterySeedWeights.ERDirectionPairs = (int)GUI.HorizontalSlider(new Rect(210f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.ERDirectionPairs, 0, 100);

                mystHeight += 40f * guiScale;
                ShowTooltip(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Shuffle Ladders");
                ShowTooltip(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Grass Randomizer");
                GUI.Label(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Shuffle Ladders");
                GUI.Label(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Grass Randomizer");
                mystHeight += 25f * guiScale;
                GUI.Label(new Rect(160f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.ShuffleLadders}%");
                TunicRandomizer.Settings.MysterySeedWeights.ShuffleLadders = (int)GUI.HorizontalSlider(new Rect(10f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.ShuffleLadders, 0, 100);
                // Todo add grass randomizer
                GUI.Label(new Rect(360f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.GrassRando}%");
                TunicRandomizer.Settings.MysterySeedWeights.GrassRando = (int)GUI.HorizontalSlider(new Rect(210f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.GrassRando, 0, 100);
                
                mystHeight += 40f * guiScale;
                ShowTooltip(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Lanternless Logic");
                ShowTooltip(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Maskless Logic");
                GUI.Label(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Lanternless Logic");
                GUI.Label(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Maskless Logic");
                mystHeight += 25f * guiScale;
                GUI.Label(new Rect(160f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.Lanternless}%");
                TunicRandomizer.Settings.MysterySeedWeights.Lanternless = (int)GUI.HorizontalSlider(new Rect(10f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.Lanternless, 0, 100);
                GUI.Label(new Rect(360f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.Maskless}%");
                TunicRandomizer.Settings.MysterySeedWeights.Maskless = (int)GUI.HorizontalSlider(new Rect(210f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.Maskless, 0, 100);
            } else if (MysteryWindowPage == 1) {

                GUI.Label(new Rect(10f * guiScale, mystHeight, 400f * guiScale, 30f * guiScale), $"Hexagon Quest - Goal Amount");
                ShowTooltip(new Rect(10f * guiScale, mystHeight, 400f * guiScale, 125f * guiScale), "mysterySeed", "Randomize Hexagon Quest Amounts");
                mystHeight += 30f * guiScale;
                TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalRandom = GUI.Toggle(new Rect(10f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalRandom, "Random");
                TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalLow = GUI.Toggle(new Rect(110f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalLow, " Low");
                TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalMedium = GUI.Toggle(new Rect(210f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalMedium, "Medium");
                TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalHigh = GUI.Toggle(new Rect(310f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalHigh, "High");

                mystHeight += 35f * guiScale;
                GUI.Label(new Rect(10f * guiScale, mystHeight, 400f * guiScale, 30f * guiScale), $"Hexagon Quest - # of Extra Hexagons");
                mystHeight += 30f * guiScale;
                TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasRandom = GUI.Toggle(new Rect(10f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasRandom, "Random");
                TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasLow = GUI.Toggle(new Rect(110f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasLow, "Low");
                TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasMedium = GUI.Toggle(new Rect(210f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasMedium, "Medium");
                TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasHigh = GUI.Toggle(new Rect(310f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasHigh, "High");

                mystHeight += 35f * guiScale;
                GUI.Label(new Rect(10f * guiScale, mystHeight, 400f * guiScale, 30f * guiScale), $"Hexagon Quest - Shuffled Abilities Unlock By");
                ShowTooltip(new Rect(10f * guiScale, mystHeight, 400f * guiScale, 60f * guiScale), "mysterySeed", "Hexagon Quest Ability Shuffle Mode");
                mystHeight += 35f * guiScale;
                GUI.skin.label.fontSize = (int)(16 * guiScale); 
                GUI.Label(new Rect(10f * guiScale, mystHeight, 120f * guiScale, 30f * guiScale), $"Hexagons: {100 - TunicRandomizer.Settings.MysterySeedWeights.HexQuestAbilityShufflePages}%");
                GUI.Label(new Rect(320f * guiScale, mystHeight, 100f * guiScale, 30f * guiScale), $"Pages: {TunicRandomizer.Settings.MysterySeedWeights.HexQuestAbilityShufflePages}%");
                TunicRandomizer.Settings.MysterySeedWeights.HexQuestAbilityShufflePages = (int)GUI.HorizontalSlider(new Rect(120f * guiScale, mystHeight + 5f, 190f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestAbilityShufflePages, 0, 100);
                GUI.skin.label.fontSize = (int)(20 * guiScale);

                mystHeight += 30f * guiScale;
                GUI.Label(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Fool Traps");
                ShowTooltip(new Rect(10f * guiScale, mystHeight, 400f * guiScale, 60f * guiScale), "mysterySeed", "Fool Traps");
                mystHeight += 30f * guiScale;
                TunicRandomizer.Settings.MysterySeedWeights.FoolTrapNone = GUI.Toggle(new Rect(10f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.FoolTrapNone, $"None");
                TunicRandomizer.Settings.MysterySeedWeights.FoolTrapNormal = GUI.Toggle(new Rect(110f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.FoolTrapNormal, $"<color={FoolColors[1]}>{FoolChoices[1]}</color>");
                TunicRandomizer.Settings.MysterySeedWeights.FoolTrapDouble = GUI.Toggle(new Rect(210f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.FoolTrapDouble, $"<color={FoolColors[2]}>{FoolChoices[2]}</color>");
                TunicRandomizer.Settings.MysterySeedWeights.FoolTrapOnslaught = GUI.Toggle(new Rect(300f * guiScale, mystHeight, 100f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.FoolTrapOnslaught, $"<color={FoolColors[3]}>{FoolChoices[3]}</color>");

                mystHeight += 35f * guiScale;
                GUI.Label(new Rect(10f * guiScale, mystHeight, 400f * guiScale, 30f * guiScale), $"Hero's Laurels Location");
                ShowTooltip(new Rect(10f * guiScale, mystHeight, 400f * guiScale, 60f * guiScale), "mysterySeed", "Hero's Laurels Location");
                mystHeight += 30f * guiScale;
                TunicRandomizer.Settings.MysterySeedWeights.LaurelsRandom = GUI.Toggle(new Rect(10f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.LaurelsRandom, $"Random");
                TunicRandomizer.Settings.MysterySeedWeights.LaurelsSixCoins = GUI.Toggle(new Rect(110f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.LaurelsSixCoins, $"6 Coins");
                TunicRandomizer.Settings.MysterySeedWeights.LaurelsTenCoins = GUI.Toggle(new Rect(210f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.LaurelsTenCoins, $"10 Coins");
                TunicRandomizer.Settings.MysterySeedWeights.LaurelsTenFairies = GUI.Toggle(new Rect(310f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.LaurelsTenFairies, $"10 Fairies");
            }
            GUI.skin.button.fontSize = (int)(20 * guiScale);
            mystHeight += 40f * guiScale;
            bool RandomizeAll = GUI.Button(new Rect(10f * guiScale, mystHeight, 187.5f * guiScale, 30f * guiScale), "Randomize All");
            if (RandomizeAll) {
                TunicRandomizer.Settings.MysterySeedWeights.Randomize();
                RandomizerSettings.SaveSettings();
            }
            bool ResetToDefault = GUI.Button(new Rect(207.5f * guiScale, mystHeight, 187.5f * guiScale, 30f * guiScale), "Reset To Defaults");
            if (ResetToDefault) {
                TunicRandomizer.Settings.MysterySeedWeights = new MysterySeedWeights();
                RandomizerSettings.SaveSettings();
            }
            mystHeight += 40f * guiScale;
            bool Close = GUI.Button(new Rect(10f * guiScale, mystHeight, 187.5f * guiScale, 30f * guiScale), "Close");
            if (Close) {
                ShowMysterySeedWindow = false;
                MysteryWindowPage = 0;
                RandomizerSettings.SaveSettings();
            }
            bool SwitchPage = GUI.Button(new Rect(207.5f * guiScale, mystHeight, 187.5f * guiScale, 30f * guiScale), MysteryWindowPage == 0 ? "Next Page >" : "< Prev Page");
            if (SwitchPage) {
                MysteryWindowPage = MysteryWindowPage == 0 ? 1 : 0;
                RandomizerSettings.SaveSettings();
            }
            mystHeight += 40f * guiScale;
        }

        private static void CloseAPSettingsWindow() {
            ShowAPSettingsWindow = false;
            stringToEdit = "";
            clearAllEditingFlags();
            RandomizerSettings.SaveSettings();
        }

        public static bool TitleScreen___NewGame_PrefixPatch(TitleScreen __instance) {
            CloseAPSettingsWindow();
            RecentItemsDisplay.instance.ResetQueue();
            if (SaveFlags.IsArchipelago()) {
                Archipelago.instance.integration.ItemIndex = 0;
                Archipelago.instance.integration.ClearQueue();
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
                    if (SaveFile.GetString(SaveFlags.ArchipelagoHostname) != "" && SaveFile.GetInt(SaveFlags.ArchipelagoPort) != 0) { 
                        TunicRandomizer.Settings.ReadConnectionSettingsFromSaveFile();
                    }
                    Archipelago.instance.Disconnect();
                    errorMessage = Archipelago.instance.Connect();
                }
                if (!Archipelago.instance.IsConnected()) {
                    GenericMessage.ShowMessage($"<#FF0000>[death] \"<#FF0000>warning!\" <#FF0000>[death]\n\"Failed to connect to Archipelago:\"\n{errorMessage}\n\"Returning to title screen.\"");
                    return false;
                } else if (SaveFlags.IsArchipelago()) {
                    if (SaveFile.GetString("archipelago player name") != Archipelago.instance.GetPlayerName(Archipelago.instance.GetPlayerSlot()) || int.Parse(Archipelago.instance.integration.slotData["seed"].ToString()) != SaveFile.GetInt("seed")) {
                        TunicLogger.LogInfo("Save does not match connected slot! Connected to " + TunicRandomizer.Settings.ConnectionSettings.Player + " [seed " + Archipelago.instance.integration.slotData["seed"].ToString() + "] but slot name in save file is " + SaveFile.GetString("archipelago player name") + " [seed " + SaveFile.GetInt("seed") + "]");
                        GenericMessage.ShowMessage("<#FF0000>[death] \"<#FF0000>warning!\" <#FF0000>[death]\n\"Save does not match connected slot.\"\n\"Returning to menu.\"");
                        return false;
                    }
                    PlayerCharacterSpawn.OnArrivalCallback += (Action)(() => {
                        List<long> locationsInLimbo = new List<long>();
                        foreach (KeyValuePair<string, long> pair in Locations.LocationIdToArchipelagoId) {
                            if (SaveFile.GetInt("randomizer picked up " + pair.Key) == 1 && !Archipelago.instance.integration.session.Locations.AllLocationsChecked.Contains(pair.Value) && Archipelago.instance.integration.session.Locations.AllMissingLocations.Contains(pair.Value)) {
                                locationsInLimbo.Add(pair.Value);
                            }
                        }
                        if (locationsInLimbo.Count > 0) {
                            LanguageLine line = ScriptableObject.CreateInstance<LanguageLine>();
                            line.text = $"<#FFFF00>[death] \"<#FFFF00>attention!\" <#FFFF00>[death]\n" +
                                $"\"Found {locationsInLimbo.Count} location{(locationsInLimbo.Count != 1 ? "s": "")} in the save file\"\n\"that {(locationsInLimbo.Count != 1 ? "were" : "was")} not sent to Archipelago.\"\n" +
                                $"\"Send {(locationsInLimbo.Count != 1 ? "these" : "this")} location{(locationsInLimbo.Count != 1 ? "s" : "")} now?\"";
                            if (TunicRandomizer.Settings.UseTrunicTranslations) {
                                line.text = $"<#FFFF00>[death] <#FFFF00>uhtehn$uhn! <#FFFF00>[death]\nfownd \"{locationsInLimbo.Count}\" lOkA$uhn{(locationsInLimbo.Count != 1 ? "z" : "")} in #uh sAv fIl #aht\n{(locationsInLimbo.Count != 1 ? "wur" : "wawz")} nawt sehnt too RkipehluhgO.\nsehnd {(locationsInLimbo.Count != 1 ? "#Ez" : "#is")} lOkA$uhn{(locationsInLimbo.Count != 1 ? "z" : "")} now?";
                            }
                            GenericPrompt.ShowPrompt(line, (Action)(() => { Archipelago.instance.integration.session.Locations.CompleteLocationChecks(locationsInLimbo.ToArray()); }), (Action)(() => { }));
                        }
                    });
                }
            }
            return true;
        }

    }

}
