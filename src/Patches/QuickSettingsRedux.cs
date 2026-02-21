using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TunicRandomizer {
    public class QuickSettingsRedux : MonoBehaviour {

        public static string CustomSeed = "";
        public static Font OdinRounded;
        public static List<string> FoolChoices = new List<string>() { "Off", "Normal", "Double", "Onslaught" };
        public static List<string> FoolColors = new List<string>() { "white", "#4FF5D4", "#E3D457", "#FF3333" };
        private static bool ShowAPSettingsWindow = false;
        private static string stringToEdit = "";
        private static int stringCursorPosition = 0;
        private static bool showPort = false;
        private static bool showPassword = false;

        private static float guiScale = 1f;
        public static float y = 0f;

        private static int logicPage = 1;

        private static bool showSettings = true;
        private static bool showLogic = true;
        private static bool showHexQuestOptions = true;
        private static bool showGeneral = false;
        private static bool showHints = false;
        private static bool showEnemyRando = false;
        public static bool showFoxCustomization = false;
        private static bool showMisc = false;

        private static bool foxSetupComplete = false;
        public static GameObject fox = null;
        public static Transform foxHead = null;
        public static GameObject sunglasses = null;
        public static GameObject laurels = null;
        public static bool hideLaurels = false;

        private static GameObject sword = null;
        private static TitleScreen titleScreen = null;

        public static bool titleScreenShowMusicToggles = false;
        public static bool titleScreenShowJukebox = false;
        public static bool titleScreenShowEnemyToggles = false;

        public static QuickSettingsRedux instance;

        private static Dictionary<string, bool> editingFlags = new Dictionary<string, bool>() {
            {"Player", false},
            {"Hostname", false},
            {"Port", false},
            {"Password", false},
        };

        // For showing option tooltips
        private static string hoveredOption = "";
        public static Rect windowRect = new Rect();

        //Get a conenction setting value by fieldname
        private string getConnectionSetting(string fieldName) {
            switch (fieldName) {
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
        private void setConnectionSetting(string fieldName, string value) {
            switch (fieldName) {
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
        private string textWithCursor(string text, bool isEditing, bool showText) {
            string baseText = showText ? text : new string('*', text.Length);
            if (!isEditing) return baseText;
            if (stringCursorPosition > baseText.Length) stringCursorPosition = baseText.Length;
            return baseText.Insert(stringCursorPosition, "<color=#EAA614>|</color>");
        }

        //Clear all field editing flags (since we do this in a few places)
        private void clearAllEditingFlags() {

            List<string> fieldKeys = new List<string>(editingFlags.Keys);
            foreach (string fieldKey in fieldKeys) {
                editingFlags[fieldKey] = false;
            }
        }

        //Initialize a text field for editing
        private void beginEditingTextField(string fieldName) {
            if (editingFlags[fieldName]) return; //can't begin if we're already editing this field

            //check and finalize if another field was mid-edit
            List<string> fieldKeys = new List<string>(editingFlags.Keys);
            foreach (string fieldKey in fieldKeys) {
                if (editingFlags[fieldKey]) finishEditingTextField(fieldKey);
            }

            stringToEdit = getConnectionSetting(fieldName);
            stringCursorPosition = stringToEdit.Length;
            editingFlags[fieldName] = true;
        }

        //finalize editing a text field and save the changes
        private void finishEditingTextField(string fieldName) {
            if (!editingFlags[fieldName]) return; //can't finish if we're not editing this field

            stringToEdit = "";
            stringCursorPosition = 0;
            RandomizerSettings.SaveSettings();
            editingFlags[fieldName] = false;
        }

        private void handleEditButton(string fieldName) {
            if (editingFlags[fieldName]) {
                Invoke("UnlockButtons", 1);
                finishEditingTextField(fieldName);
            } else {
                LockButtons();
                beginEditingTextField(fieldName);
            }
        }

        private void handlePasteButton(string fieldName) {

            setConnectionSetting(fieldName, GUIUtility.systemCopyBuffer);
            if (editingFlags[fieldName]) {
                stringToEdit = GUIUtility.systemCopyBuffer;
                finishEditingTextField(fieldName);
            }
            RandomizerSettings.SaveSettings();
        }

        private void handleClearButton(string fieldName) {
            setConnectionSetting(fieldName, "");
            if (editingFlags[fieldName]) stringToEdit = "";
            RandomizerSettings.SaveSettings();
        }

        private static Rect scRect(float x, float y, float width, float height, bool scaleX = true, bool scaleY = true, bool scaleW = true, bool scaleH = true, string tooltip = "") {
            if (tooltip != "") {
                return ShowTooltip(new Rect(x * (scaleX ? guiScale : 1.0f), y * (scaleY ? guiScale : 1.0f), width * (scaleW ? guiScale : 1.0f), height * (scaleH ? guiScale : 1.0f)), tooltip);
            }
            return new Rect(x * (scaleX ? guiScale : 1.0f), y * (scaleY ? guiScale : 1.0f), width * (scaleW ? guiScale : 1.0f), height * (scaleH ? guiScale : 1.0f));
        }

        private static int scFont(float size) {
            return (int)(size * guiScale);
        }

        private float calcWindowY() {
            float windowY = (float)Screen.height * 0.11f;
            if (Screen.height != Camera.main.pixelHeight) {
                int diff = Screen.height - Camera.main.pixelHeight;
                windowY = (float)((Screen.height - diff) * 0.11f) + (diff / 2);
            }
            return windowY;
        }

        private float calcWindowX() {
            float windowX = 20f;
            if (Screen.width != Camera.main.pixelWidth) {
                int diff = Screen.width - Camera.main.pixelWidth;
                windowX += (diff / 2);
            }
            return windowX;
        }

        private void OnGUI() {
            if (SceneManager.GetActiveScene().name == "TitleScreen" && GameObject.FindObjectOfType<TitleScreen>() != null) {

                guiScale = TunicUtils.calcGuiScale();

                GUI.skin.font = PaletteEditor.OdinRounded == null ? GUI.skin.font : PaletteEditor.OdinRounded;
                Cursor.visible = true;

                float windowX = calcWindowX();
                float windowY = calcWindowY();

                windowRect = scRect(windowX, (float)windowY, 658f, y, scaleY: false);
                GUI.Window(101, windowRect, new Action<int>(QuickSettingsWindow), "Quick Settings");

                if (hoveredOption != "" && TunicRandomizer.Settings.OptionTooltips) {
                    GUI.Window(107, scRect(windowX, (float)Screen.height - (110f * guiScale), 1000f, 110f, scaleY: false), new Action<int>(TooltipWindow), hoveredOption);
                }
                if (sword != null) {
                    sword.SetActive(!showFoxCustomization);
                    sword.transform.localPosition = new Vector3(-14.3f, 23.1f, -1.1f);
                }
                GameObject.Find("__Spirit").transform.GetChild(2).gameObject.SetActive(!showFoxCustomization);
                GameObject.Find("soft edged plane (3)").GetComponent<Renderer>().enabled = !showFoxCustomization;
                GameObject.Find("PS: dust motes").GetComponent<Renderer>().enabled = !showFoxCustomization;

                CameraController.Flip = TunicRandomizer.Settings.CameraFlip && !showFoxCustomization;
            }
        }

        private void Update() {
            if (titleScreen == null) {
                titleScreen = Resources.FindObjectsOfTypeAll<TitleScreen>().FirstOrDefault();
            }
            if (sword == null) {
                sword = GameObject.Find("elderfox_sword graphic");
            }
            if ((TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO && ShowAPSettingsWindow) && SceneManager.GetActiveScene().name == "TitleScreen") {
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
                    ) {

                    bool inputValid = true;

                    //validation for any fields that require it
                    if (editingFlags["Port"] && !int.TryParse(Input.inputString, out int num)) inputValid = false;

                    if (inputValid) {
                        stringToEdit = stringToEdit.Insert(stringCursorPosition, Input.inputString);
                        stringCursorPosition++;
                    }
                }

                //handle backspacing
                if (Input.GetKeyDown(KeyCode.Backspace)) {
                    if (stringToEdit.Length > 0 && stringCursorPosition > 0) {
                        stringToEdit = stringToEdit.Remove(stringCursorPosition - 1, 1);
                        stringCursorPosition--;
                    }
                }

                //handle delete
                if (Input.GetKeyDown(KeyCode.Delete)) {
                    if (stringToEdit.Length > 0 && stringCursorPosition < stringToEdit.Length) {
                        stringToEdit = stringToEdit.Remove(stringCursorPosition, 1);
                    }
                }

                //handle cursor navigation
                if (Input.GetKeyDown(KeyCode.LeftArrow) && stringCursorPosition > 0) {
                    stringCursorPosition--;
                }
                if (Input.GetKeyDown(KeyCode.RightArrow) && stringCursorPosition < stringToEdit.Length) {
                    stringCursorPosition++;
                }

                //handle Enter/Esc
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Escape)) {
                    if (!editingFlags["Player"] && !editingFlags["Hostname"] && !editingFlags["Port"] && !editingFlags["Password"]) {
                        CloseAPSettingsWindow();
                    }

                    submitKeyPressed = true;
                }

                //update the relevant connection setting field
                Dictionary<string, bool> originalEditingFlags = new Dictionary<string, bool>(editingFlags);
                foreach (KeyValuePair<string, bool> editingFlag in originalEditingFlags) {
                    if (!editingFlag.Value) continue;
                    setConnectionSetting(editingFlag.Key, stringToEdit);
                    if (submitKeyPressed) finishEditingTextField(editingFlag.Key);
                }
            }
        }

        public void Start() {
            instance = this;
            SetupFoxEditor();
        }

        private static Rect ShowTooltip(Rect rect, string option) {
            bool hovered = CombineRects(windowRect, rect).Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y));
            if (hovered && Tooltips.OptionTooltips.ContainsKey(option)) {
                hoveredOption = option;
            }
            if (hoveredOption == option && !hovered) {
                hoveredOption = "";
            }
            return rect;
        }

        private static void TooltipWindow(int windowID) {
            GUI.skin.label.fontSize = scFont(22f);
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.skin.label.clipping = TextClipping.Overflow;
            GUI.color = Color.white;
            GUI.DragWindow(scRect(0, Screen.height - 100, 1000f, 80));
            GUI.Label(scRect(10f, 20f, 980f, 100), Tooltips.OptionTooltips.ContainsKey(hoveredOption) ? Tooltips.OptionTooltips[hoveredOption] : hoveredOption);
        }

        private static Rect CombineRects(Rect r1, Rect r2) {
            Rect r3 = r1;
            r3.x += r2.x;
            r3.y += r2.y;
            r3.height = r2.height;
            r3.width = r2.width;
            return r3;
        }

        private void ResetButtons(bool logic = false, bool general = false, bool hints = false, bool enemyRando = false, bool fox = false, bool misc = false) {
            showLogic = logic;
            showGeneral = general;
            showHints = hints;
            showEnemyRando = enemyRando;
            showFoxCustomization = fox;
            showMisc = misc;
            RandomizerSettings.SaveSettings();
        }

        private void QuickSettingsWindow(int windowID) {
            GUI.skin.toggle.wordWrap = true;
            GUI.skin.label.fontSize = scFont(25f);
            GUI.skin.toggle.fontSize = scFont(20);
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.skin.label.clipping = TextClipping.Overflow;
            GUI.color = Color.white;
            GUI.DragWindow(scRect(500f, 50f, 500f, 30f));
            y = 20f;
            if (TitleVersion.UpdateAvailable) {
                bool DownloadUpdate = GUI.Button(scRect(10f, y, 638f, 30f), $"<color=#FFA500>Update Available! Download Randomizer Mod Ver. {TitleVersion.UpdateVersion}</color>");
                if (DownloadUpdate) {
                    System.Diagnostics.Process.Start(TitleVersion.UpdateUrl);
                }
                y += 35f;
            }
            GUI.skin.toggle.fontSize = scFont(15);
            if (TunicRandomizer.Settings.RaceMode) {
                TunicRandomizer.Settings.RaceMode = GUI.Toggle(scRect(510f, y, 150f, 30f), TunicRandomizer.Settings.RaceMode, "Race Mode Enabled");
            } else {
                if (TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO && Archipelago.instance.integration.disableSpoilerLog) {
                    GUI.skin.label.fontSize = scFont(16);
                    GUI.Label(scRect(455f, y, 206f, 30f), "Spoiler Log Disabled by Host");
                    GUI.skin.label.fontSize = scFont(25);
                } else {
                    bool ToggleSpoilerLog = GUI.Toggle(scRect(TunicRandomizer.Settings.CreateSpoilerLog ? 510f : 560f, y, 90f, 30f), TunicRandomizer.Settings.CreateSpoilerLog, "Spoiler Log");
                    TunicRandomizer.Settings.CreateSpoilerLog = ToggleSpoilerLog;
                    if (ToggleSpoilerLog) {
                        GUI.skin.button.fontSize = scFont(15);
                        bool OpenSpoilerLog = GUI.Button(scRect(600f, y, 50f, 25f), "Open");
                        if (OpenSpoilerLog) {
                            if (File.Exists(TunicRandomizer.SpoilerLogPath)) {
                                System.Diagnostics.Process.Start(TunicRandomizer.SpoilerLogPath);
                            }
                        }
                    }
                }
            }

            GUI.skin.toggle.fontSize = scFont(20);

            GUI.Label(scRect(10f, y, 300f, 30f), "Randomizer Mode");
            y += 40f;
            bool ToggleSinglePlayer = GUI.Toggle(scRect(10f, y, 130f, 30f), TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER, "Single Player");
            if (ToggleSinglePlayer && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO) {
                TunicRandomizer.Settings.Mode = RandomizerSettings.RandomizerType.SINGLEPLAYER;
                Archipelago.instance.Disconnect();
                CloseAPSettingsWindow();
                RandomizerSettings.SaveSettings();
            }
            bool ToggleArchipelago = GUI.Toggle(scRect(150f, y, 150f, 30f), TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO, "Archipelago");
            if (ToggleArchipelago && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER) {
                TunicRandomizer.Settings.Mode = RandomizerSettings.RandomizerType.ARCHIPELAGO;
                RandomizerSettings.SaveSettings();
            }

            GUI.skin.button.fontSize = scFont(20f);
            y += 40f;
            string buttonLabel = "";
            switch (TunicRandomizer.Settings.Mode) {
                case RandomizerSettings.RandomizerType.SINGLEPLAYER:
                    buttonLabel = "Seed";
                    y = SeedSection(y);
                    break;
                case RandomizerSettings.RandomizerType.ARCHIPELAGO:
                    buttonLabel = "Archipelago";
                    y = ArchipelagoPlayerSection(y);
                    break;
            }
            y += 40f;
            Divider(y);
            y += 10f;
            GUI.Label(scRect(10f, y, 250f, 30f), "Randomizer Settings");
            GUI.skin.button.fontSize = scFont(15);
            bool ShowSettings = GUI.Button(scRect(236f, y + (5f), 50f, 25f), showSettings ? "Hide" : "Show");
            if (ShowSettings) {
                showSettings = !showSettings;
            }
            GUI.skin.button.fontSize = scFont(20);
            y += 40f;
            if (showSettings) {
                GUI.backgroundColor = showLogic ? Color.black : Color.white;
                bool ShowLogic = GUI.Button(scRect(10f, y, 118f, 30f), buttonLabel);
                if (ShowLogic) {
                    ResetButtons(logic: true);
                }
                GUI.backgroundColor = showGeneral ? Color.black : Color.white;
                bool ShowGeneral = GUI.Button(scRect(138f, y, 98f, 30f), "General");
                if (ShowGeneral) {
                    ResetButtons(general: true);
                }
                GUI.backgroundColor = showHints ? Color.black : Color.white;
                bool ShowHints = GUI.Button(scRect(246f, y, 98f, 30f), "Hints");
                if (ShowHints) {
                    ResetButtons(hints: true);
                }
                GUI.backgroundColor = showEnemyRando ? Color.black : Color.white;
                bool ShowEnemyRando = GUI.Button(scRect(354f, y, 98f, 30f), "Enemy");
                if (ShowEnemyRando) {
                    ResetButtons(enemyRando: true);
                }
                GUI.backgroundColor = showFoxCustomization ? Color.black : Color.white;
                bool ShowFox = GUI.Button(scRect(462f, y, 78f, 30f), "Fox");
                if (ShowFox) {
                    if (TunicRandomizer.Settings.UseCustomTexture && !showFoxCustomization) {
                        Invoke("loadCustomTexture", 0.05f);
                    }
                    
                    ResetButtons(fox: true);
                }
                GUI.backgroundColor = showMisc ? Color.black : Color.white;
                bool ShowMisc = GUI.Button(scRect(550f, y, 98f, 30f), "Other");
                if (ShowMisc) {
                    ResetButtons(misc: true);
                }
                GUI.backgroundColor = Color.white;
                y += 40f;
                string label = "";
                if (showLogic) {
                    if (TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER) {
                        y = LogicSettingsSection(y);
                    } else if (TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO) {
                        y = ArchipelagoSettingsSection(y);
                    }
                }
                if (showGeneral) {
                    y = GeneralSettingsSection(y);
                }
                if (showHints) { 
                    y = HintSettingsSection(y);
                }
                if (showEnemyRando) {
                    y = EnemyRandoSection(y);
                }
                if (showFoxCustomization) {
                    y = FoxCustomizationSection(y);
                }
                if (fox != null) {
                    fox.SetActive(showFoxCustomization);
                    laurels.GetComponent<Renderer>().enabled = showFoxCustomization && !hideLaurels;
                    sunglasses.SetActive(showFoxCustomization && TunicRandomizer.Settings.RealestAlwaysOn);
                }
                if (titleScreen != null) { 
                    titleScreen.transform.GetChild(0).gameObject.SetActive(!showFoxCustomization);
                    titleScreen.transform.GetChild(1).gameObject.SetActive(!showFoxCustomization);
                }
                if (showMisc) {
                    y = OtherSettingsSection(y);
                }
            }
        }

        private void loadCustomTexture() {
            PaletteEditor.LoadCustomTexture();
        }
        
        private void Divider(float y) {
            GUI.skin.horizontalSlider.fixedHeight = 8;
            GUI.HorizontalSlider(scRect(10f, y, 638, 20f), 0, 0, 0);
            GUI.skin.horizontalSlider.fixedHeight = 12;
        }

        private float ArchipelagoPlayerSection(float y) {
            void StatusLabel() {
                GUI.Label(scRect(10f, y, 80f, 30f), $"Status:");
                if (Archipelago.instance.integration != null && Archipelago.instance.integration.connected) {
                    GUI.Label(scRect(95f, y, 150f, 30f), $"<color=#00FF00>Connected!</color>");
                    int playerCount = 0;
                    foreach (var player in Archipelago.instance.integration.session.Players.AllPlayers) {
                        if (player.Slot > 0 && player.GetGroupMembers(Archipelago.instance.integration.session.Players) == null) {
                            playerCount++;
                        }
                    }
                    bool shrinkText = playerCount > 1000 || (playerCount > 100 && Archipelago.instance.integration.session.Locations.AllLocations.Count > 1000);
                    GUI.Label(scRect(250f, y, 408f, 30f), $"<size={scFont(shrinkText ? 21f : 23f)}>(world: {Archipelago.instance.integration.session.ConnectionInfo.Slot} of {playerCount}, checks: {Archipelago.instance.integration.session.Locations.AllLocationsChecked.Count} of {Archipelago.instance.integration.session.Locations.AllLocations.Count})</size>");
                } else {
                    GUI.Label(scRect(95f, y, 300f, 30f), $"<color=#FF0000>Disconnected</color>");
                }
            }
            if (ShowAPSettingsWindow) {
                y = ArchipelagoConnectionSettings(y);
            } else {
                GUI.Label(scRect(10f, y, 500f, 30f), $"Player: {(TunicRandomizer.Settings.ConnectionSettings.Player)}");
            }
            y += 40f;
            StatusLabel();
            y += 40f;
            bool Connection = GUI.Button(scRect(10f, y, 160f, 30f), Archipelago.instance.IsConnected() ? "Disconnect" : "Connect");
            if (Connection) {
                if (Archipelago.instance.IsConnected()) {
                    Archipelago.instance.Disconnect();
                } else {
                    Archipelago.instance.Connect();
                    if (Archipelago.instance.IsConnected()) {
                        ShowAPSettingsWindow = false;
                    }
                }
            }

            bool OpenAPSettings = GUI.Button(scRect(180f, y, 220f, 30f), ShowAPSettingsWindow ? "Close Connection Info" : "Edit Connection Info");
            if (OpenAPSettings) {
                if (ShowAPSettingsWindow) {
                    CloseAPSettingsWindow();
                    Archipelago.instance.Connect();
                } else {
                    ShowAPSettingsWindow = true;
                }
            }
            bool ConnectViaRoomLink = GUI.Button(ShowTooltip(scRect(410f, y, 238f, 30f), "Connect via Player Link"), "Connect via Player Link");
            if (ConnectViaRoomLink) {
                try {
                    Archipelago.instance.Disconnect();
                    Archipelago.instance.ParseUri(GUIUtility.systemCopyBuffer);
                    Archipelago.instance.Connect();
                } catch (Exception e) {
                    TunicLogger.LogInfo($"{e.Message} {e.Source} {e.StackTrace}");
                    Notifications.Show($"\"Could not parse link.\"", $"");
                }
            }

            return y;
        }

        private float ArchipelagoMiscSettings(float y) {
            GUI.Label(scRect(10f, y, 400f, 30f), $"Archipelago Settings");
            y += 40f;
            TunicRandomizer.Settings.DeathLinkEnabled = GUI.Toggle(ShowTooltip(scRect(10f, y, 115f, 30f), "Death Link"), TunicRandomizer.Settings.DeathLinkEnabled, "Death Link");
            TunicRandomizer.Settings.TrapLinkEnabled = GUI.Toggle(ShowTooltip(scRect(127.5f, y, 105.5f, 30f), "Trap Link"), TunicRandomizer.Settings.TrapLinkEnabled, "Trap Link");
            TunicRandomizer.Settings.SendHintsToServer = GUI.Toggle(ShowTooltip(scRect(236f, y, 200f, 30f), "Send Hints to Server"), TunicRandomizer.Settings.SendHintsToServer, "Send Hints to Server");
            TunicRandomizer.Settings.CollectReflectsInWorld = GUI.Toggle(ShowTooltip(scRect(442f, y, 198f, 30f), "Hide Collected Items"), TunicRandomizer.Settings.CollectReflectsInWorld, "Hide Collected Items");
            y += 40f;
            return y;
        }

        private float ArchipelagoSettingsSection(float y) {
            y = ArchipelagoWorldSettings(y);
            y += 40f;
            y = ArchipelagoMiscSettings(y);

            return y;
        }

        private float ArchipelagoConnectionSettings(float y) {
            GUI.skin.label.fontSize = scFont(25);
            GUI.skin.button.fontSize = scFont(17);

            //Player & Hostname
            GUI.Label(scRect(10f, y, 300f, 30f), $"Player: {textWithCursor(getConnectionSetting("Player"), editingFlags["Player"], true)}");
            GUI.Label(scRect(334f, y, 300f, 30f), $"Host: {textWithCursor(getConnectionSetting("Hostname"), editingFlags["Hostname"], true)}");
            y += 30f;

            GUI.skin.toggle.fontSize = scFont(16);
            bool setArchipelagoHost = GUI.Toggle(scRect(334f, y, 140f, 25f), TunicRandomizer.Settings.ConnectionSettings.Hostname == "archipelago.gg", "archipelago.gg");
            if (setArchipelagoHost && TunicRandomizer.Settings.ConnectionSettings.Hostname != "archipelago.gg") {
                setConnectionSetting("Hostname", "archipelago.gg");
                RandomizerSettings.SaveSettings();
            }

            bool setLocalhost = GUI.Toggle(scRect(500f, y, 90f, 25f), TunicRandomizer.Settings.ConnectionSettings.Hostname == "localhost", "localhost");
            if (setLocalhost && TunicRandomizer.Settings.ConnectionSettings.Hostname != "localhost") {
                setConnectionSetting("Hostname", "localhost");
                RandomizerSettings.SaveSettings();
            }

            GUI.skin.toggle.fontSize = scFont(20);

            y += 30f;

            //y += 40f;
            bool EditPlayer = GUI.Button(scRect(10f, y, 75f, 30f), editingFlags["Player"] ? "Save" : "Edit");
            if (EditPlayer) handleEditButton("Player");

            bool PastePlayer = GUI.Button(scRect(100f, y, 75f, 30f), "Paste");
            if (PastePlayer) handlePasteButton("Player");

            bool ClearPlayer = GUI.Button(scRect(190f, y, 75f, 30f), "Clear");
            if (ClearPlayer) handleClearButton("Player");

            bool EditHostname = GUI.Button(scRect(334f, y, 75f, 30f), editingFlags["Hostname"] ? "Save" : "Edit");
            if (EditHostname) handleEditButton("Hostname");

            bool PasteHostname = GUI.Button(scRect(424f, y, 75f, 30f), "Paste");
            if (PasteHostname) handlePasteButton("Hostname");

            bool ClearHostname = GUI.Button(scRect(514f, y, 75f, 30f), "Clear");
            if (ClearHostname) handleClearButton("Hostname");

            // Port & Password
            y += 40f;
            GUI.Label(scRect(10f, y, 300f, 30f), $"Port: {textWithCursor(getConnectionSetting("Port"), editingFlags["Port"], showPort)}");
            showPort = GUI.Toggle(scRect(251f, y, 75f, 30f), showPort, "Show");
            GUI.Label(scRect(334f, y, 300f, 30f), $"Password: {textWithCursor(getConnectionSetting("Password"), editingFlags["Password"], showPassword)}");
            showPassword = GUI.Toggle(scRect(575f, y, 75f, 30f), showPassword, "Show");

            y += 40f;

            bool EditPort = GUI.Button(scRect(10f, y, 75f, 30f), editingFlags["Port"] ? "Save" : "Edit");
            if (EditPort) handleEditButton("Port");

            bool PastePort = GUI.Button(scRect(100f, y, 75f, 30f), "Paste");
            if (PastePort) handlePasteButton("Port");

            bool ClearPort = GUI.Button(scRect(190f, y, 75f, 30f), "Clear");
            if (ClearPort) handleClearButton("Port");
            //Password

            bool EditPassword = GUI.Button(scRect(334f, y, 75f, 30f), editingFlags["Password"] ? "Save" : "Edit");
            if (EditPassword) handleEditButton("Password");

            bool PastePassword = GUI.Button(scRect(424f, y, 75f, 30f), "Paste");
            if (PastePassword) handlePasteButton("Password");

            bool ClearPassword = GUI.Button(scRect(514f, y, 75f, 30f), "Clear");
            if (ClearPassword) handleClearButton("Password");
            GUI.skin.button.fontSize = scFont(20);
            return y;
        }

        private float ArchipelagoWorldSettings(float y) {
            GUI.Label(scRect(10f, y, 200f, 30f), $"World Settings");

            GUI.skin.button.fontSize = scFont(15);
            bool ToggleSettings = GUI.Button(scRect(180f, y + 7.5f, 50f, 25f), TunicRandomizer.Settings.ShowSlotSettings ? "Hide" : "Show");
            if (ToggleSettings) {
                TunicRandomizer.Settings.ShowSlotSettings = !TunicRandomizer.Settings.ShowSlotSettings;
                RandomizerSettings.SaveSettings();
            }
            if (TunicRandomizer.Settings.ShowSlotSettings) {
                if (Archipelago.instance.integration != null && Archipelago.instance.integration.connected) {
                    Dictionary<string, object> slotData = Archipelago.instance.GetPlayerSlotData();

                    y += 40f;
                    
                    GUI.Toggle(scRect(10f, y, 206f, 30f, tooltip: "Hexagon Quest"), slotData["hexagon_quest"].ToString() == "1", slotData["hexagon_quest"].ToString() == "1" ?
                        $"Hexagon Quest (<color=#E3D457>{slotData["Hexagon Quest Goal"].ToString()}</color>)" : $"Hexagon Quest");
                    GUI.Toggle(scRect(226f, y, 206f, 30f, tooltip: "Sword Progression"), slotData["sword_progression"].ToString() == "1", "Sword Progression");
                    GUI.Toggle(scRect(442f, y, 206f, 30f, tooltip: "Start With Sword"), slotData["start_with_sword"].ToString() == "1", "Start With Sword");
                    
                    y += 40f;
                    
                    GUI.Toggle(scRect(10f, y, 206f, 30f, tooltip: "Keys Behind Bosses"), slotData["keys_behind_bosses"].ToString() == "1", "Keys Behind Bosses");
                    GUI.Toggle(scRect(226f, y, 206f, 30f, tooltip: "Shuffle Abilities"), slotData["ability_shuffling"].ToString() == "1", "Shuffled Abilities");
                    GUI.Toggle(scRect(442f, y, 206f, 30f, tooltip: "Shuffle Ladders"), slotData["shuffle_ladders"].ToString() == "1", $"Shuffled Ladders");

                    y += 40f;

                    GUI.Toggle(scRect(10f, y, 206f, 30f, tooltip: "Entrance Randomizer"), slotData["entrance_rando"].ToString() == "1", $"Entrance Randomizer");
                    GUI.Toggle(scRect(226f, y, 206f, 30f, tooltip: "Grass Randomizer"), slotData.ContainsKey("grass_randomizer") && slotData["grass_randomizer"].ToString() == "1", $"Grass Randomizer");
                    GUI.Toggle(scRect(442f, y, 206f, 30f, tooltip: "Shuffle Breakable Objects"), slotData.ContainsKey("breakable_shuffle") && slotData["breakable_shuffle"].ToString() == "1", $"Shuffled Breakables");

                    y += 40f;

                    GUI.Toggle(scRect(10f, y, 206f, 30f, tooltip: "Shuffle Fuses"), slotData.ContainsKey("shuffle_fuses") && slotData["shuffle_fuses"].ToString() == "1", $"Shuffled Fuses");
                    GUI.Toggle(scRect(226f, y, 206f, 30f, tooltip: "Shuffle Bells"), slotData.ContainsKey("shuffle_bells") && slotData["shuffle_bells"].ToString() == "1", $"Shuffled Bells");
                    int FoolIndex = int.Parse(slotData["fool_traps"].ToString());
                    GUI.Toggle(scRect(442f, y, 206f, 60f, tooltip: "Fool Traps"), FoolIndex != 0, $"Fool Traps: {(FoolIndex == 0 ? "Off" : $"<color={FoolColors[FoolIndex]}>{FoolChoices[FoolIndex]}</color>")}");

                } else {
                    y += 40f;
                    GUI.Toggle(scRect(10f, y, 206f, 30f, tooltip: "Hexagon Quest"), false, "Hexagon Quest");
                    GUI.Toggle(scRect(226f, y, 206f, 30, tooltip: "Sword Progression"), false, "Sword Progression");
                    GUI.Toggle(scRect(442f, y, 206f, 30f, tooltip: "Start With Sword"), false, "Start With Sword");
                    y += 40f;
                    GUI.Toggle(scRect(10f, y, 206f, 30f, tooltip: "Keys Behind Bosses"), false, "Keys Behind Bosses");
                    GUI.Toggle(scRect(226f, y, 206f, 30f, tooltip: "Shuffle Abilities"), false, "Shuffled Abilities");
                    GUI.Toggle(scRect(442f, y, 206f, 30f, tooltip: "Shuffle Ladders"), false, $"Shuffled Ladders");
                    y += 40f;
                    GUI.Toggle(scRect(10f, y, 206f, 30f, tooltip: "Entrance Randomizer"), false, $"Entrance Randomizer");
                    GUI.Toggle(scRect(226f, y, 206f, 30f, tooltip: "Grass Randomizer"), false, $"Grass Randomizer");
                    GUI.Toggle(scRect(442f, y, 206f, 30f, tooltip: "Shuffle Breakable Objects"), false, $"Shuffled Breakables");
                    y += 40f;
                    GUI.Toggle(scRect(10f, y, 206f, 30f, tooltip: "Shuffle Fuses"), false, $"Shuffled Fuses");
                    GUI.Toggle(scRect(226f, y, 206f, 30f, tooltip: "Shuffle Bells"), false, $"Shuffled Bells");
                    GUI.Toggle(scRect(442f, y, 206f, 30f, tooltip: "Fool Traps"), false, "Fool Traps: Off");
                }
            }
            GUI.skin.button.fontSize = scFont(20);

            return y;
        }

        private float SeedSection(float y) {
            GUI.Label(scRect(10f, y, 300f, 30f), $"Custom Seed: {(CustomSeed == "" ? "Not Set" : CustomSeed.ToString())}");
            if (CustomSeed != "") {
                GUI.skin.button.fontSize = scFont(17);
                bool ClearSeed = GUI.Button(scRect(300f, y + (5f), 50f, 25f), "Clear");
                GUI.skin.button.fontSize = scFont(20);
                if (ClearSeed) {
                    CustomSeed = "";
                }
            }
            GUI.Label(scRect(442f, y, 206f, 30f), $"Checks: {(TunicRandomizer.Settings.MysterySeed ? "???" : calculateCheckCount().ToString())}");
            y += 40f;
            bool GenerateSeed = GUI.Button(ShowTooltip(scRect(10f, y, 206f, 30f), "Generate Seed"), new GUIContent("Generate Seed", null, "Generates a Seed"));
            if (GenerateSeed) {
                CustomSeed = new System.Random().Next().ToString();
            }

            bool CopySettings = GUI.Button(ShowTooltip(scRect(226f, y, 206f, 30f), "Copy Seed + Settings"), "Copy Seed + Settings");
            if (CopySettings) {
                GUIUtility.systemCopyBuffer = TunicRandomizer.Settings.GetSettingsString();
            }
            bool PasteSettings = GUI.Button(ShowTooltip(scRect(442f, y, 206f, 30f), "Paste Seed + Settings"), "Paste Seed + Settings");
            if (PasteSettings) {
                TunicRandomizer.Settings.ParseSettingsString(GUIUtility.systemCopyBuffer);
            }

            return y;
        }

        private int calculateCheckCount() {
            int count = Locations.VanillaLocations.Count;
            if (TunicRandomizer.Settings.BreakableShuffle) {
                count += BreakableShuffle.BreakableChecksBaseCount;
                if (TunicRandomizer.Settings.EntranceRandoEnabled) {
                    count += BreakableShuffle.BreakableChecksEntranceRandoCount;
                }
            }
            if (TunicRandomizer.Settings.GrassRandomizer) {
                count += GrassRandomizer.GrassChecks.Count;
            }
            if (TunicRandomizer.Settings.FuseShuffle) {
                count += FuseRandomizer.FuseChecks.Count;
            }
            if (TunicRandomizer.Settings.BellShuffle) {
                count += BellShuffle.BellChecks.Count;
            }
            return count;
        }
        private float LogicSettingsSection(float y) {
            GUI.Label(scRect(10f, y, 400f, 30f), logicPage == 2 ? "Advanced Options" : $"Seed & Logic Settings");
            GUI.backgroundColor = Color.white;
            TunicRandomizer.Settings.MysterySeed = GUI.Toggle(ShowTooltip(scRect(442f, y, 206f, 30f), "Mystery Seed"), TunicRandomizer.Settings.MysterySeed, "Mystery Seed");
            y += 40f;
            // 10
            // 226
            // 442
            if (logicPage == 1) {
                if (TunicRandomizer.Settings.MysterySeed) {
                    y = MysterySeedMainLogicSection(y);
                } else {
                    y = MainLogicSection(y);
                }
            } else if (logicPage == 2) {
                if (TunicRandomizer.Settings.MysterySeed) {
                    y = MysterySeedAdvancedLogicSection(y);
                } else {
                    y = AdvancedLogicSection(y);
                }
            } else {
                logicPage = 1;
            }
            y += 37.5f;
            GUI.skin.label.fontSize = scFont(25);
            GUI.skin.button.fontSize = scFont(20);
            GUI.Label(scRect(275f, y, 150f, 30f), $"Page {logicPage} / 2");
            y += 2.5f;
            //if (logicPage == 2) {
                bool BackPage = GUI.Button(scRect(10f, y, 206f, 30f), "<< Previous Page");
                if (BackPage) {
                logicPage = logicPage == 1 ? 2 : 1;
            }
            //}
           // if (logicPage == 1) {
                bool NextPage = GUI.Button(scRect(442f, y, 206f, 30f), "Next Page >>");
                if (NextPage) {
                    logicPage = logicPage == 1 ? 2 : 1;
                }
           // }
            y += 40f;
            GUI.skin.button.fontSize = scFont(20);
            return y;
        }

        private float MainLogicSection(float y) {
            TunicRandomizer.Settings.SwordProgressionEnabled = GUI.Toggle(ShowTooltip(scRect(10f, y, 206f, 30f), "Sword Progression"), TunicRandomizer.Settings.SwordProgressionEnabled, "Sword Progression");
            TunicRandomizer.Settings.KeysBehindBosses = GUI.Toggle(ShowTooltip(scRect(226f, y, 206f, 30f), "Keys Behind Bosses"), TunicRandomizer.Settings.KeysBehindBosses, "Keys Behind Bosses");
            TunicRandomizer.Settings.StartWithSwordEnabled = GUI.Toggle(ShowTooltip(scRect(442f, y, 206f, 30f), "Start With Sword"), TunicRandomizer.Settings.StartWithSwordEnabled, "Start With Sword");
            y += 40f;
            TunicRandomizer.Settings.ShuffleAbilities = GUI.Toggle(ShowTooltip(scRect(10f, y, 206f, 30f), "Shuffle Abilities"), TunicRandomizer.Settings.ShuffleAbilities, "Shuffle Abilities");
            //TunicRandomizer.Settings.EntranceRandoEnabled = GUI.Toggle(ShowTooltip(scRect(226f, y, 206f, 30f), "Entrance Randomizer"), TunicRandomizer.Settings.EntranceRandoEnabled, "Entrance Randomizer");
            TunicRandomizer.Settings.ShuffleLadders = GUI.Toggle(ShowTooltip(scRect(226f, y, 206f, 30f), "Shuffle Ladders"), TunicRandomizer.Settings.ShuffleLadders, "Shuffle Ladders");
            TunicRandomizer.Settings.BreakableShuffle = GUI.Toggle(ShowTooltip(scRect(442f, y, 206f, 30f), "Shuffle Breakable Objects"), TunicRandomizer.Settings.BreakableShuffle, "Shuffle Breakables");
            y += 40f;
            TunicRandomizer.Settings.FuseShuffle = GUI.Toggle(ShowTooltip(scRect(10f, y, 206f, 30f), "Shuffle Fuses"), TunicRandomizer.Settings.FuseShuffle, "Shuffle Fuses");
            TunicRandomizer.Settings.BellShuffle = GUI.Toggle(ShowTooltip(scRect(226f, y, 206f, 30f), "Shuffle Bells"), TunicRandomizer.Settings.BellShuffle, "Shuffle Bells");
            TunicRandomizer.Settings.GrassRandomizer = GUI.Toggle(ShowTooltip(scRect(442f, y, 206f, 30f), "Grass Randomizer"), TunicRandomizer.Settings.GrassRandomizer, "Grass Randomizer");
            y += 40f;
            GUI.skin.toggle.fontSize = scFont(22.5f);
            TunicRandomizer.Settings.EntranceRandoEnabled = GUI.Toggle(ShowTooltip(scRect(10f, y, 400f, 30f), "Entrance Randomizer"), TunicRandomizer.Settings.EntranceRandoEnabled, "Entrance Randomizer");
            GUI.skin.toggle.fontSize = scFont(20f);
            y += 30f;
            GUI.skin.label.fontSize = scFont(20f);
            GUI.Label(scRect(25f, y, 206f, 30f), "Entrance Layout:");
            GUI.skin.label.fontSize = scFont(25f);
            TunicRandomizer.Settings.ERFixedShop = GUI.Toggle(ShowTooltip(scRect(226f, y, 206f, 30f), "Fewer Shops"), TunicRandomizer.Settings.ERFixedShop, "Fewer Shops");
            if (TunicRandomizer.Settings.ERFixedShop) {
                TunicRandomizer.Settings.PortalDirectionPairs = false;
            }
            TunicRandomizer.Settings.PortalDirectionPairs = GUI.Toggle(ShowTooltip(scRect(442f, y, 190f, 30f), "Matching Directions"), TunicRandomizer.Settings.PortalDirectionPairs, "Matching Directions");
            if (TunicRandomizer.Settings.PortalDirectionPairs) {
                TunicRandomizer.Settings.ERFixedShop = false;
            }
            y += 30f;
            TunicRandomizer.Settings.DecoupledER = GUI.Toggle(ShowTooltip(scRect(25f, y, 206f, 30f), "Decoupled Entrances"), TunicRandomizer.Settings.DecoupledER, "Decoupled Entrances");
            y += 40f;
            y = HexagonQuestSection(y);
            return y;
        }

        private float HexagonQuestSection(float y) {
            GUI.skin.toggle.fontSize = scFont(22.5f);
            bool ToggleHexagonQuest = GUI.Toggle(ShowTooltip(scRect(10f, y, 240f, 35f), "Hexagon Quest"), TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST, $"Hexagon Quest <size={scFont(22.5f)}>{$"(<color=#E3D457>{(TunicRandomizer.Settings.RandomizeHexQuest ? "?" : $"{TunicRandomizer.Settings.HexagonQuestGoal.ToString()}")}</color>)"}</size>");
            if (ToggleHexagonQuest) {
                TunicRandomizer.Settings.GameMode = RandomizerSettings.GameModes.HEXAGONQUEST;
            } else if (!ToggleHexagonQuest && TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST) {
                TunicRandomizer.Settings.GameMode = RandomizerSettings.GameModes.RANDOMIZER;
            }
            if (TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST) {
                GUI.skin.button.fontSize = scFont(15);
                bool showConfiguration = GUI.Button(scRect(270f, y + (5), 90f, 25f), showHexQuestOptions ? "Hide Config" : "Configure");
                if (showConfiguration) {
                    showHexQuestOptions = !showHexQuestOptions;
                }
                GUI.skin.button.fontSize = scFont(20);
            }
            if (TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST && showHexQuestOptions) {
                //y += 5f;
                y += 30f;
                GUI.skin.label.fontSize = scFont(20f);
                if (TunicRandomizer.Settings.RandomizeHexQuest) {
                    GUI.Label(scRect(25f, y, 220f, 20f), $"Hexagons Required:");
                    ShowTooltip(scRect(25f, y, 600f, 70f), "Randomize Hexagon Quest Amounts");
                    bool RandomGoal = GUI.Toggle(scRect(200f, y, 100f, 30f), TunicRandomizer.Settings.HexagonQuestRandomGoal == RandomizerSettings.HexQuestValue.RANDOM, "Random");
                    if (RandomGoal) {
                        TunicRandomizer.Settings.HexagonQuestRandomGoal = RandomizerSettings.HexQuestValue.RANDOM;
                    }
                    bool LowGoal = GUI.Toggle(scRect(310f, y, 100f, 30f), TunicRandomizer.Settings.HexagonQuestRandomGoal == RandomizerSettings.HexQuestValue.LOW, "Low");
                    if (LowGoal) {
                        TunicRandomizer.Settings.HexagonQuestRandomGoal = RandomizerSettings.HexQuestValue.LOW;
                    }
                    bool MediumGoal = GUI.Toggle(scRect(390f, y, 100f, 30f), TunicRandomizer.Settings.HexagonQuestRandomGoal == RandomizerSettings.HexQuestValue.MEDIUM, "Medium");
                    if (MediumGoal) {
                        TunicRandomizer.Settings.HexagonQuestRandomGoal = RandomizerSettings.HexQuestValue.MEDIUM;
                    }
                    bool HighGoal = GUI.Toggle(scRect(500f, y, 100f, 30f), TunicRandomizer.Settings.HexagonQuestRandomGoal == RandomizerSettings.HexQuestValue.HIGH, "High");
                    if (HighGoal) {
                        TunicRandomizer.Settings.HexagonQuestRandomGoal = RandomizerSettings.HexQuestValue.HIGH;
                    }
                    y += 30f;
                    GUI.Label(scRect(25f, y, 220f, 30f), $"Extra Hexagons:");
                    bool RandomExtras = GUI.Toggle(scRect(200f, y, 100f, 30f), TunicRandomizer.Settings.HexagonQuestRandomExtras == RandomizerSettings.HexQuestValue.RANDOM, "Random");
                    if (RandomExtras) {
                        TunicRandomizer.Settings.HexagonQuestRandomExtras = RandomizerSettings.HexQuestValue.RANDOM;
                    }
                    bool LowExtras = GUI.Toggle(scRect(310f, y, 100f, 30f), TunicRandomizer.Settings.HexagonQuestRandomExtras == RandomizerSettings.HexQuestValue.LOW, "Low");
                    if (LowExtras) {
                        TunicRandomizer.Settings.HexagonQuestRandomExtras = RandomizerSettings.HexQuestValue.LOW;
                    }
                    bool MediumExtras = GUI.Toggle(scRect(390f, y, 100f, 30f), TunicRandomizer.Settings.HexagonQuestRandomExtras == RandomizerSettings.HexQuestValue.MEDIUM, "Medium");
                    if (MediumExtras) {
                        TunicRandomizer.Settings.HexagonQuestRandomExtras = RandomizerSettings.HexQuestValue.MEDIUM;
                    }
                    bool HighExtras = GUI.Toggle(scRect(500f, y, 100f, 30f), TunicRandomizer.Settings.HexagonQuestRandomExtras == RandomizerSettings.HexQuestValue.HIGH, "High");
                    if (HighExtras) {
                        TunicRandomizer.Settings.HexagonQuestRandomExtras = RandomizerSettings.HexQuestValue.HIGH;
                    }
                } else {
                    ShowTooltip(scRect(25f, y, 385f, 30f), "Hexagons Required");
                    GUI.Label(scRect(25f, y, 270f, 20f), $"Hexagons Required:");
                    GUI.Label(scRect(240f, y, 30f, 30f), $"{(TunicRandomizer.Settings.HexagonQuestGoal)}");
                    TunicRandomizer.Settings.HexagonQuestGoal = (int)GUI.HorizontalSlider(scRect(270f, y + (15f), 363f, 20f), TunicRandomizer.Settings.HexagonQuestGoal, 1, 100);
                    y += 30f;
                    ShowTooltip(scRect(25f, y, 385f, 30f), "Hexagons in Item Pool");
                    GUI.Label(scRect(25f, y, 270f, 30f), $"Hexagons in Item Pool:");
                    GUI.Label(scRect(240f, y, 30f, 30f), $"{(Math.Min((int)Math.Round((100f + TunicRandomizer.Settings.HexagonQuestExtraPercentage) / 100f * TunicRandomizer.Settings.HexagonQuestGoal), 100))}");
                    TunicRandomizer.Settings.HexagonQuestExtraPercentage = (int)GUI.HorizontalSlider(scRect(270f, y + (15f), 363f, 30f), TunicRandomizer.Settings.HexagonQuestExtraPercentage, 0, 100);
                }
                y += 30f;
                GUI.skin.toggle.fontSize = scFont(20f);
                TunicRandomizer.Settings.RandomizeHexQuest = GUI.Toggle(ShowTooltip(scRect(25f, y, 300f, 30f), "Randomize Hexagon Quest Amounts"), TunicRandomizer.Settings.RandomizeHexQuest, $"Randomize # of Gold Hexagons");
                y += 30f;
                GUI.Label(scRect(25f, y, 350f, 30f), "Ability Shuffle Mode:");
                ShowTooltip(scRect(25f, y, 630f, 30f), "Hexagon Quest Ability Shuffle Mode");
                TunicRandomizer.Settings.HexQuestAbilitiesUnlockedByPages = !GUI.Toggle(scRect(240f, y, 120f, 30f), !TunicRandomizer.Settings.HexQuestAbilitiesUnlockedByPages, "Hexagons");
                TunicRandomizer.Settings.HexQuestAbilitiesUnlockedByPages = GUI.Toggle(scRect(390f, y, 90f, 30f), TunicRandomizer.Settings.HexQuestAbilitiesUnlockedByPages, "Pages");
                GUI.skin.label.fontSize = scFont(20);
            }
            return y;
        }

        private float AdvancedLogicSection(float y) {
            GUI.skin.label.fontSize = scFont(21f);
            GUI.Label(scRect(10f, y, 150f, 30f), $"Fool Traps:");
            ShowTooltip(scRect(10f, y, 638f, 80f), "Fool Traps");
            y += 2.5f;
            bool NoFools = GUI.Toggle(scRect(226f, y, 90f, 30f), TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.NONE, "None");
            if (NoFools) {
                TunicRandomizer.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.NONE;
            }
            bool NormalFools = GUI.Toggle(scRect(326f, y, 90f, 30f), TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.NORMAL, "<color=#4FF5D4>Normal</color>");
            if (NormalFools) {
                TunicRandomizer.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.NORMAL;
            }
            bool DoubleFools = GUI.Toggle(scRect(426f, y, 90f, 30f), TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.DOUBLE, "<color=#E3D457>Double</color>");
            if (DoubleFools) {
                TunicRandomizer.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.DOUBLE;
            }
            bool OnslaughtFools = GUI.Toggle(scRect(526f, y, 110f, 30f), TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.ONSLAUGHT, "<color=#FF3333>Onslaught</color>");
            if (OnslaughtFools) {
                TunicRandomizer.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.ONSLAUGHT;
            }
            y += 37.5f;
            GUI.Label(scRect(10f, y, 240f, 30f), $"Hero's Laurels Location:");
            ShowTooltip(scRect(10f, y, 638f, 80f), "Hero's Laurels Location");
            y += 2.5f;
            bool RandomLaurels = GUI.Toggle(scRect(226f, y, 90f, 30f), TunicRandomizer.Settings.FixedLaurelsOption == RandomizerSettings.FixedLaurelsType.RANDOM, "Random");
            if (RandomLaurels) {
                TunicRandomizer.Settings.FixedLaurelsOption = RandomizerSettings.FixedLaurelsType.RANDOM;
            }
            bool SixCoinsLaurels = GUI.Toggle(scRect(326f, y, 90f, 30f), TunicRandomizer.Settings.FixedLaurelsOption == RandomizerSettings.FixedLaurelsType.SIXCOINS, "6 Coins");
            if (SixCoinsLaurels) {
                TunicRandomizer.Settings.FixedLaurelsOption = RandomizerSettings.FixedLaurelsType.SIXCOINS;
            }
            bool TenCoinsLaurels = GUI.Toggle(scRect(426f, y, 90f, 30f), TunicRandomizer.Settings.FixedLaurelsOption == RandomizerSettings.FixedLaurelsType.TENCOINS, "10 Coins");
            if (TenCoinsLaurels) {
                TunicRandomizer.Settings.FixedLaurelsOption = RandomizerSettings.FixedLaurelsType.TENCOINS;
            }
            bool TenFairiesLaurels = GUI.Toggle(scRect(526f, y, 100f, 30f), TunicRandomizer.Settings.FixedLaurelsOption == RandomizerSettings.FixedLaurelsType.TENFAIRIES, "10 Fairies");
            if (TenFairiesLaurels) {
                TunicRandomizer.Settings.FixedLaurelsOption = RandomizerSettings.FixedLaurelsType.TENFAIRIES;
            }
            y += 37.5f;
            GUI.Label(scRect(10f, y, 200f, 30f), $"Difficulty Options:");
            y += 2.5f;
            TunicRandomizer.Settings.Lanternless = GUI.Toggle(ShowTooltip(scRect(226f, y, 206f, 30f), "Lanternless Logic"), TunicRandomizer.Settings.Lanternless, "Lanternless Logic");
            TunicRandomizer.Settings.Maskless = GUI.Toggle(ShowTooltip(scRect(426f, y, 206f, 30f), "Maskless Logic"), TunicRandomizer.Settings.Maskless, "Maskless Logic");
            y += 37.5f;
            GUI.skin.label.fontSize = scFont(25f);
            y = TrickLogicSection(y);
            return y;
        }

        private float TrickLogicSection(float y) {
            GUI.Label(scRect(10f, y, 300f, 30f), $"Trick & Glitch Logic");
            GUI.skin.label.fontSize = scFont(21f);
            y += 40f;
            GUI.Label(scRect(10f, y, 300f, 30f), $"Ice Grapples in Logic:");
            ShowTooltip(scRect(10f, y, 385f, 30f), "Ice Grapples");
            bool IceGrapplesOff = GUI.Toggle(scRect(226f, y, 90f, 30f), TunicRandomizer.Settings.IceGrappling == RandomizerSettings.IceGrapplingType.OFF, "Off");
            ShowTooltip(scRect(226f, y, 90f, 30f), "Ice Grapples Off");
            if (IceGrapplesOff) {
                TunicRandomizer.Settings.IceGrappling = RandomizerSettings.IceGrapplingType.OFF;
            }
            bool IceGrapplesEasy = GUI.Toggle(scRect(326f, y, 90f, 30f), TunicRandomizer.Settings.IceGrappling == RandomizerSettings.IceGrapplingType.EASY, "Easy");
            ShowTooltip(scRect(326f, y, 90f, 30f), "Ice Grapples Easy");
            if (IceGrapplesEasy) {
                TunicRandomizer.Settings.IceGrappling = RandomizerSettings.IceGrapplingType.EASY;
            }
            bool IceGrapplesMedium = GUI.Toggle(scRect(426f, y, 90f, 30f), TunicRandomizer.Settings.IceGrappling == RandomizerSettings.IceGrapplingType.MEDIUM, "Medium");
            ShowTooltip(scRect(426f, y, 90f, 30f), "Ice Grapples Medium");
            if (IceGrapplesMedium) {
                TunicRandomizer.Settings.IceGrappling = RandomizerSettings.IceGrapplingType.MEDIUM;
            }
            bool IceGrapplesHard = GUI.Toggle(scRect(526f, y, 90f, 30f), TunicRandomizer.Settings.IceGrappling == RandomizerSettings.IceGrapplingType.HARD, "Hard");
            ShowTooltip(scRect(526f, y, 90f, 30f), "Ice Grapples Hard");
            if (IceGrapplesHard) {
                TunicRandomizer.Settings.IceGrappling = RandomizerSettings.IceGrapplingType.HARD;
            }
            y += 40f;
            GUI.Label(scRect(10f, y, 300f, 30f), $"Ladder Storage in Logic:");
            ShowTooltip(scRect(10f, y, 385f, 30f), "Ladder Storage");
            bool LadderStorageOff = GUI.Toggle(scRect(226f, y, 90f, 30f), TunicRandomizer.Settings.LadderStorage == RandomizerSettings.LadderStorageType.OFF, "Off");
            ShowTooltip(scRect(226f, y, 90f, 30f), "Ladder Storage Off");
            if (LadderStorageOff) {
                TunicRandomizer.Settings.LadderStorage = RandomizerSettings.LadderStorageType.OFF;
            }
            bool LadderStorageEasy = GUI.Toggle(scRect(326f, y, 90f, 30f), TunicRandomizer.Settings.LadderStorage == RandomizerSettings.LadderStorageType.EASY, "Easy");
            ShowTooltip(scRect(326f, y, 90f, 30f), "Ladder Storage Easy");
            if (LadderStorageEasy) {
                TunicRandomizer.Settings.LadderStorage = RandomizerSettings.LadderStorageType.EASY;
            }
            bool LadderStorageMedium = GUI.Toggle(scRect(426f, y, 90f, 30f), TunicRandomizer.Settings.LadderStorage == RandomizerSettings.LadderStorageType.MEDIUM, "Medium");
            ShowTooltip(scRect(426f, y, 90f, 30f), "Ladder Storage Medium");
            if (LadderStorageMedium) {
                TunicRandomizer.Settings.LadderStorage = RandomizerSettings.LadderStorageType.MEDIUM;
            }
            bool LadderStorageHard = GUI.Toggle(scRect(526f, y, 90f, 30f), TunicRandomizer.Settings.LadderStorage == RandomizerSettings.LadderStorageType.HARD, "Hard");
            ShowTooltip(scRect(526f, y, 90f, 30f), "Ladder Storage Hard");
            if (LadderStorageHard) {
                TunicRandomizer.Settings.LadderStorage = RandomizerSettings.LadderStorageType.HARD;
            }
            y += 40f;
            TunicRandomizer.Settings.LaurelsZips = GUI.Toggle(ShowTooltip(scRect(10f, y, 200f, 30f), "Laurels Zips"), TunicRandomizer.Settings.LaurelsZips, "Laurels Zips");
            TunicRandomizer.Settings.LadderStorageWithoutItems = GUI.Toggle(ShowTooltip(scRect(226f, y, 300f, 30f), "Ladder Storage Without Items"), TunicRandomizer.Settings.LadderStorageWithoutItems, "Ladder Storage Without Items");
            GUI.skin.label.fontSize = scFont(25f);
            return y;
        }

        private float GeneralSettingsSection(float y) {
            GUI.Label(scRect(10f, y, 400f, 30f), $"General Settings & Quality of Life");
            GUI.backgroundColor = Color.white;
            y += 40f;
            TunicRandomizer.Settings.ClearEarlyBushes = GUI.Toggle(ShowTooltip(scRect(10f, y, 206f, 30f), "Clear Early Bushes"), TunicRandomizer.Settings.ClearEarlyBushes, "Clear Early Bushes");
            TunicRandomizer.Settings.HeirAssistModeEnabled = GUI.Toggle(ShowTooltip(scRect(226f, y, 206f, 30f), "Easier Heir Fight"), TunicRandomizer.Settings.HeirAssistModeEnabled, "Easier Heir Fight");
            GUI.skin.toggle.fontSize = scFont(19);
            TunicRandomizer.Settings.EnableAllCheckpoints = GUI.Toggle(ShowTooltip(scRect(442f, y, 206f, 30f), "Enable All Checkpoints"), TunicRandomizer.Settings.EnableAllCheckpoints, "Enable All Checkpoints");
            GUI.skin.toggle.fontSize = scFont(20);
            y += 40f;
            TunicRandomizer.Settings.CheaperShopItemsEnabled = GUI.Toggle(ShowTooltip(scRect(10f, y, 206f, 30f), "Cheaper Shop Items"), TunicRandomizer.Settings.CheaperShopItemsEnabled, "Cheaper Shop Items");
            TunicRandomizer.Settings.BonusStatUpgradesEnabled = GUI.Toggle(ShowTooltip(scRect(226f, y, 206f, 30f), "Bonus Upgrades"), TunicRandomizer.Settings.BonusStatUpgradesEnabled, "Bonus Upgrades");
            GUI.skin.toggle.fontSize = scFont(17);
            TunicRandomizer.Settings.DisableChestInterruption = GUI.Toggle(ShowTooltip(scRect(442f, y, 206f, 30f), "Disable Chest Interruption"), TunicRandomizer.Settings.DisableChestInterruption, "Disable Chest Interruption");
            GUI.skin.toggle.fontSize = scFont(20);
            y += 40f;
            TunicRandomizer.Settings.ShowRecentItems = GUI.Toggle(ShowTooltip(scRect(10f, y, 206f, 30f), "Show Recent Items"), TunicRandomizer.Settings.ShowRecentItems, "Show Recent Items");
            TunicRandomizer.Settings.SkipItemAnimations = GUI.Toggle(ShowTooltip(scRect(226f, y, 206f, 30f), "Skip Item Popups"), TunicRandomizer.Settings.SkipItemAnimations, "Skip Item Popups");

            GUI.skin.toggle.fontSize = scFont(18);
            TunicRandomizer.Settings.FasterUpgrades = GUI.Toggle(ShowTooltip(scRect(442f, y, 206f, 30f), "Skip Upgrade Animations"), TunicRandomizer.Settings.FasterUpgrades, "Skip Upgrade Animations");
            GUI.skin.toggle.fontSize = scFont(20);
            y += 40f;
            TunicRandomizer.Settings.HolyCrossVisualizer = GUI.Toggle(ShowTooltip(scRect(10f, y, 206f, 30f), "Holy Cross DDR"), TunicRandomizer.Settings.HolyCrossVisualizer, "Holy Cross DDR");
            TunicRandomizer.Settings.ShowPlayerPosition = GUI.Toggle(ShowTooltip(scRect(226f, y, 206f, 30f), "Show Player Position"), TunicRandomizer.Settings.ShowPlayerPosition, "Show Player Position");
            TunicRandomizer.Settings.ArachnophobiaMode = GUI.Toggle(ShowTooltip(scRect(442f, y, 206f, 30f), "Arachnophobia Mode"), TunicRandomizer.Settings.ArachnophobiaMode, "Arachnophobia Mode");
            y += 40f;
            TunicRandomizer.Settings.MoreSkulls = GUI.Toggle(ShowTooltip(scRect(10f, y, 206f, 30f), "More Skulls"), TunicRandomizer.Settings.MoreSkulls, "More Skulls");
            TunicRandomizer.Settings.CameraFlip = GUI.Toggle(ShowTooltip(scRect(226f, y, 206f, 30f), "???"), TunicRandomizer.Settings.CameraFlip, "???");
            if (SecretMayor.shouldBeActive || SecretMayor.checkIfActive()) {
                bool mayorToggle = GUI.Toggle(ShowTooltip(scRect(442f, y, 206f, 30f), "Mr Mayor"), SecretMayor.shouldBeActive, "<color=#ffd700>Mr Mayor</color>");
                if ((mayorToggle && !SecretMayor.shouldBeActive) || (!mayorToggle && SecretMayor.shouldBeActive)) {
                    SecretMayor.ToggleMayorSecret(0);
                }
            }
            y += 40f;

            GUI.Label(scRect(10f, y, 200f, 30f), $"Music Shuffle");
            y += 40f;

            TunicRandomizer.Settings.MusicShuffle = GUI.Toggle(ShowTooltip(scRect(10f, y, 206f, 30f), "Music Shuffle"), TunicRandomizer.Settings.MusicShuffle, "Music Shuffle");
            TunicRandomizer.Settings.SeededMusic = GUI.Toggle(ShowTooltip(scRect(226f, y, 206f, 30f), "Seeded Music"), TunicRandomizer.Settings.SeededMusic, "Seeded Music");
            bool openMusicToggles = GUI.Button(scRect(442f, y, 206f, 30f, tooltip: "Music Toggles"), "Music Toggles");
            if (openMusicToggles) {
                OptionsGUI options = Resources.FindObjectsOfTypeAll<OptionsGUI>().FirstOrDefault();
                TitleScreen titleScreen = Resources.FindObjectsOfTypeAll<TitleScreen>().FirstOrDefault();
                if (options != null && titleScreen != null) {
                    titleScreenShowMusicToggles = true;
                    GUIMode.PushMode(options);
                }
            }
            y += 40f;
            bool openJukebox = GUI.Button(scRect(10f, y, 206f, 30f, tooltip: "Jukebox"), "Jukebox");
            if (openJukebox) {
                OptionsGUI options = Resources.FindObjectsOfTypeAll<OptionsGUI>().FirstOrDefault();
                TitleScreen titleScreen = Resources.FindObjectsOfTypeAll<TitleScreen>().FirstOrDefault();
                if (options != null && titleScreen != null) {
                    titleScreenShowJukebox = true;
                    GUIMode.PushMode(options);
                }
            }
            y += 40f;
            GUI.skin.button.fontSize = scFont(20);
            return y;
        }

        private float HintSettingsSection(float y) {
            GUI.Label(scRect(10f, y, 400f, 30f), $"Hint Settings");
            y += 40f;
            TunicRandomizer.Settings.HeroPathHintsEnabled = GUI.Toggle(ShowTooltip(scRect(10f, y, 206f, 30f), "Path of the Hero"), TunicRandomizer.Settings.HeroPathHintsEnabled, "Path of the Hero");
            TunicRandomizer.Settings.GhostFoxHintsEnabled = GUI.Toggle(ShowTooltip(scRect(226f, y, 206f, 30f), "Ghost Foxes"), TunicRandomizer.Settings.GhostFoxHintsEnabled, "Ghost Foxes");
            TunicRandomizer.Settings.SeekingSpellLogic = GUI.Toggle(ShowTooltip(scRect(442f, y, 206f, 30f), "Seeking Spell Logic"), TunicRandomizer.Settings.SeekingSpellLogic, "Seeking Spell Logic");
            y += 40f;
            GUI.skin.toggle.fontSize = scFont(19);
            TunicRandomizer.Settings.ChestsMatchContentsEnabled = GUI.Toggle(ShowTooltip(scRect(10f, y, 206f, 30f), "Chests Match Contents"), TunicRandomizer.Settings.ChestsMatchContentsEnabled, "Chests Match Contents");
            GUI.skin.toggle.fontSize = scFont(20);
            TunicRandomizer.Settings.ShowItemsEnabled = GUI.Toggle(ShowTooltip(scRect(226f, y, 206f, 60f), "Freestanding Items Match Contents"), TunicRandomizer.Settings.ShowItemsEnabled, "Freestanding Items Match Contents");
            TunicRandomizer.Settings.UseTrunicTranslations = GUI.Toggle(ShowTooltip(scRect(442f, y, 206f, 30f), "Write Hints In Trunic"), TunicRandomizer.Settings.UseTrunicTranslations, "Write Hints In Trunic");
            y += 60f;
            GUI.skin.button.fontSize = scFont(20);
            bool OpenEntranceTracker = GUI.Button(ShowTooltip(scRect(10f, y, 206f, 30f), "Entrance Tracker"), "Entrance Tracker");
            if (OpenEntranceTracker) {
                System.Diagnostics.Process.Start("https://scipiowright.gitlab.io/tunic-tracker/");
            }
            GUI.backgroundColor = Color.white;
            y += 40f;
            return y;
        }

        private float EnemyRandoSection(float y) {
            GUI.Label(scRect(10f, y, 400f, 30f), $"Enemy Randomizer Settings");
            y += 40f;
            TunicRandomizer.Settings.EnemyRandomizerEnabled = GUI.Toggle(scRect(10f, y, 206f, 30f, tooltip: "Enemy Randomizer"), TunicRandomizer.Settings.EnemyRandomizerEnabled, "Enemy Randomizer");
            TunicRandomizer.Settings.RandomEnemySizes = GUI.Toggle(scRect(226f, y, 206f, 30f, tooltip: "Random Enemy Sizes"), TunicRandomizer.Settings.RandomEnemySizes, "Random Enemy Sizes");
            TunicRandomizer.Settings.ExtraEnemiesEnabled = GUI.Toggle(scRect(442f, y, 206f, 30f, tooltip: "Extra Enemies"), TunicRandomizer.Settings.ExtraEnemiesEnabled, "Extra Enemies");
            y += 40f;
            TunicRandomizer.Settings.BalancedEnemies = GUI.Toggle(scRect(10f, y, 206f, 30f, tooltip: "Balanced Enemies"), TunicRandomizer.Settings.BalancedEnemies, "Balanced Enemies");
            TunicRandomizer.Settings.SeededEnemies = GUI.Toggle(scRect(226f, y, 206f, 30f, tooltip: "Seeded Enemies"), TunicRandomizer.Settings.SeededEnemies, "Seeded Enemies");
            TunicRandomizer.Settings.LimitBossSpawns = GUI.Toggle(scRect(442f, y, 206f, 30f, tooltip: "Limit Boss Spawns"), TunicRandomizer.Settings.LimitBossSpawns, "Limit Boss Spawns");
            y += 40f;
            TunicRandomizer.Settings.OopsAllEnemy = GUI.Toggle(scRect(10f, y, 206f, 30f, tooltip: "Oops! All Enemy"), TunicRandomizer.Settings.OopsAllEnemy, "Oops! All [Enemy]");
            TunicRandomizer.Settings.UseEnemyToggles = GUI.Toggle(scRect(226f, y, 206f, 30f, tooltip: "Use Enemy Toggles"), TunicRandomizer.Settings.UseEnemyToggles, "Use Enemy Toggles");
            bool openEnemyToggles = GUI.Button(scRect(442f, y, 206f, 30f, tooltip: "Enemy Toggles"), "Enemy Toggles");
            if (openEnemyToggles) {
                OptionsGUI options = Resources.FindObjectsOfTypeAll<OptionsGUI>().FirstOrDefault();
                TitleScreen titleScreen = Resources.FindObjectsOfTypeAll<TitleScreen>().FirstOrDefault();
                if (options != null && titleScreen != null) {
                    titleScreenShowEnemyToggles = true;
                    GUIMode.PushMode(options);
                }
            }
            y += 40f;
            return y;
        }

        private float FoxCustomizationSection(float y) {
            GUI.Label(scRect(10f, y, 300f, 30f), "Fox Customization");
            y += 40f;
            bool toggleRandomFoxColors = GUI.Toggle(scRect(10f, y, 206f, 30f, tooltip: "Random Fox Colors"), TunicRandomizer.Settings.RandomFoxColorsEnabled, "Random Fox Colors");
            if (toggleRandomFoxColors && !TunicRandomizer.Settings.RandomFoxColorsEnabled) {
                TunicRandomizer.Settings.UseCustomTexture = false;
                PaletteEditor.RandomizeFoxColors();
            } else if (!toggleRandomFoxColors && TunicRandomizer.Settings.RandomFoxColorsEnabled) {
                PaletteEditor.RevertFoxColors();
            }
            TunicRandomizer.Settings.RandomFoxColorsEnabled = toggleRandomFoxColors;

            bool toggleCustomTexture = GUI.Toggle(scRect(226f, y, 206f, 30f, tooltip: "Use Custom Texture"), TunicRandomizer.Settings.UseCustomTexture, "Use Custom Texture");
            if (toggleCustomTexture && !TunicRandomizer.Settings.UseCustomTexture) {
                TunicRandomizer.Settings.RandomFoxColorsEnabled = false;
                PaletteEditor.LoadCustomTexture();
            } else if (!toggleCustomTexture && TunicRandomizer.Settings.UseCustomTexture) {
                PaletteEditor.RevertFoxColors();
            }
            TunicRandomizer.Settings.UseCustomTexture = toggleCustomTexture;
            
            TunicRandomizer.Settings.RealestAlwaysOn = GUI.Toggle(scRect(442f, y, 206f, 30f, tooltip: "Keepin' It Real"), TunicRandomizer.Settings.RealestAlwaysOn, "Keepin' It Real");
            y += 40f;
            TunicRandomizer.Settings.BiggerHeadMode = GUI.Toggle(scRect(10f, y, 206f, 30f, tooltip: "Bigger Head Mode"), TunicRandomizer.Settings.BiggerHeadMode, "Bigger Head Mode");
            TunicRandomizer.Settings.TinierFoxMode = GUI.Toggle(scRect(226f, y, 206f, 30f, tooltip: "Tinier Fox Mode"), TunicRandomizer.Settings.TinierFoxMode, "Tinier Fox Mode");
            y += 40f;
            return y;
        }

        private float OtherSettingsSection(float y) {
            GUI.skin.toggle.fontSize = scFont(25);
            TunicRandomizer.Settings.RaceMode = GUI.Toggle(scRect(10f, y, 400f, 30f, tooltip: "Race Mode"), TunicRandomizer.Settings.RaceMode, "Race Mode");
            GUI.skin.toggle.fontSize = scFont(20);
            y += 40f;
            TunicRandomizer.Settings.DisableIceboltInHeirFight = GUI.Toggle(scRect(10f, y, 206f, 30f, tooltip: "Disable Heir Icebolt"), TunicRandomizer.Settings.DisableIceboltInHeirFight, "Disable Icebolt vs. Heir");
            TunicRandomizer.Settings.DisableDistantBellShots = GUI.Toggle(scRect(226f, y, 206f, 30f, tooltip: "Disable Distant West Bell"), TunicRandomizer.Settings.DisableDistantBellShots, "Disable West Bell Shot");
            TunicRandomizer.Settings.DisableIceGrappling = GUI.Toggle(scRect(442f, y, 206f, 30f, tooltip: "Disable Ice Grappling"), TunicRandomizer.Settings.DisableIceGrappling, "Disable Ice Grappling");
            y += 40f;
            TunicRandomizer.Settings.DisableLadderStorage = GUI.Toggle(scRect(10f, y, 206f, 30f, tooltip: "Disable Ladder Storage"), TunicRandomizer.Settings.DisableLadderStorage, "Disable Ladder Storage");
            TunicRandomizer.Settings.DisableUpgradeStealing = GUI.Toggle(scRect(226f, y, 300f, 30f, tooltip: "Disable Upgrade Stealing"), TunicRandomizer.Settings.DisableUpgradeStealing, "Disable Upgrade Stealing");
            y += 40f;
            GUI.Label(scRect(10f, y, 500f, 30f), "Misc. Settings");
            y += 40f;
            TunicRandomizer.Settings.OptionTooltips = GUI.Toggle(scRect(10f, y, 206f, 30f, tooltip: "Tooltips"), TunicRandomizer.Settings.OptionTooltips, "Main Menu Tooltips");
            TunicRandomizer.Settings.RunInBackground = GUI.Toggle(scRect(226f, y, 206f, 30f, tooltip: "Run Game in Background"), TunicRandomizer.Settings.RunInBackground, "Run In Background");
            Application.runInBackground = TunicRandomizer.Settings.RunInBackground;
            TunicRandomizer.Settings.DeathplanePatch = GUI.Toggle(scRect(442f, y, 206f, 30f, tooltip: "Deathplane/OoB Patch"), TunicRandomizer.Settings.DeathplanePatch, "Deathplane/OoB Patch");
            y += 40f;
            GUI.Label(scRect(10f, y, 500f, 30f), "Links & Resources");
            y += 40f;
            bool openRandomizerWebsite = GUI.Button(scRect(10f, y, 206f, 30f, tooltip: "Randomizer Website"), "Randomizer Website");
            if (openRandomizerWebsite) {
                System.Diagnostics.Process.Start("https://rando.tunic.run/");
            }
            bool discordLink = GUI.Button(scRect(226f, y, 206f, 30f, tooltip: "Community Discord"), "Community Discord");
            if (discordLink) {
                System.Diagnostics.Process.Start("https://discord.gg/HXkztJgQWj");
            }
            bool leaderboards = GUI.Button(scRect(442f, y, 206f, 30f, tooltip: "Speedrun Leaderboards"), "Speedrun Leaderboard");
            if (leaderboards) {
                System.Diagnostics.Process.Start("https://www.speedrun.com/tunic_rando");
            }
            y += 40f;
            bool reportAnIssue = GUI.Button(scRect(10f, y, 206f, 30f, tooltip: "Report an Issue"), "Report an Issue");
            if (reportAnIssue) {
                System.Diagnostics.Process.Start(TitleVersion.ReportIssueUrl);
            }
            bool openLogFile = GUI.Button(scRect(226f, y, 206f, 30f), "Open Log File");
            if (openLogFile) {
                System.Diagnostics.Process.Start(Application.dataPath + "/../BepInEx/LogOutput.log");
            }
            bool openSavesFolder = GUI.Button(scRect(442f, y, 206f, 30f), "Open Saves Folder");
            if (openSavesFolder) { System.Diagnostics.Process.Start(Application.persistentDataPath + "/SAVES");
            }
            y += 40f;
            return y;
        }

        private float MysterySeedMainLogicSection(float y) {
            GUI.skin.label.fontSize = scFont(20);
            GUI.skin.button.fontSize = scFont(17);
            GUI.Label(scRect(10f, y, 600f, 30f), "Mystery Seed enabled! Settings will be randomly chosen on New Game.");
            y += 30f;
            // 226f
            // 442f
            GUI.Label(scRect(10f, y, 206f, 60f, tooltip: "Hexagon Quest"), "Hexagon Quest");
            GUI.Label(scRect(226f, y, 206f, 60f, tooltip: "Sword Progression"), "Sword Progression");
            GUI.Label(scRect(442f, y, 206f, 60f, tooltip: "Keys Behind Bosses"), "Keys Behind Bosses");
            y += 25f;
            GUI.Label(scRect(170f, y, 56f, 30f), $"{TunicRandomizer.Settings.MysterySeedWeights.HexagonQuest}%");
            GUI.Label(scRect(386f, y, 56f, 30f), $"{TunicRandomizer.Settings.MysterySeedWeights.SwordProgression}%");
            GUI.Label(scRect(602f, y, 56f, 30f), $"{TunicRandomizer.Settings.MysterySeedWeights.KeysBehindBosses}%");
            y += 10f;
            TunicRandomizer.Settings.MysterySeedWeights.HexagonQuest = (int)GUI.HorizontalSlider(scRect(10f, y, 150f, 30f), TunicRandomizer.Settings.MysterySeedWeights.HexagonQuest, 0, 100);
            TunicRandomizer.Settings.MysterySeedWeights.SwordProgression = (int)GUI.HorizontalSlider(scRect(226f, y, 150f, 30f), TunicRandomizer.Settings.MysterySeedWeights.SwordProgression, 0, 100);
            TunicRandomizer.Settings.MysterySeedWeights.KeysBehindBosses = (int)GUI.HorizontalSlider(scRect(442f, y, 150f, 30f), TunicRandomizer.Settings.MysterySeedWeights.KeysBehindBosses, 0, 100);
            y += 20f;
            GUI.Label(scRect(10f, y, 206f, 60f, tooltip: "Shuffle Abilities"), "Shuffle Abilities");
            GUI.Label(scRect(226f, y, 206f, 60f, tooltip: "Shuffle Ladders"), "Shuffle Ladders");
            GUI.Label(scRect(442f, y, 206f, 60f, tooltip: "Shuffle Breakable Objects"), "Shuffle Breakables");
            y += 25f;
            GUI.Label(scRect(170f, y, 56f, 30f), $"{TunicRandomizer.Settings.MysterySeedWeights.ShuffleAbilities}%");
            GUI.Label(scRect(386f, y, 56f, 30f), $"{TunicRandomizer.Settings.MysterySeedWeights.ShuffleLadders}%");
            GUI.Label(scRect(602f, y, 56f, 30f), $"{TunicRandomizer.Settings.MysterySeedWeights.ShuffleBreakables}%");
            y += 10f; 
            TunicRandomizer.Settings.MysterySeedWeights.ShuffleAbilities = (int)GUI.HorizontalSlider(scRect(10f, y, 150f, 30f), TunicRandomizer.Settings.MysterySeedWeights.ShuffleAbilities, 0, 100);
            TunicRandomizer.Settings.MysterySeedWeights.ShuffleLadders = (int)GUI.HorizontalSlider(scRect(226f, y, 150f, 30f), TunicRandomizer.Settings.MysterySeedWeights.ShuffleLadders, 0, 100);
            TunicRandomizer.Settings.MysterySeedWeights.ShuffleBreakables = (int)GUI.HorizontalSlider(scRect(442f, y, 150f, 30f), TunicRandomizer.Settings.MysterySeedWeights.ShuffleBreakables, 0, 100);
            y += 20f;
            GUI.Label(scRect(10f, y, 206f, 60f, tooltip: "Shuffle Fuses"), "Shuffle Fuses");
            GUI.Label(scRect(226f, y, 206f, 60f, tooltip: "Shuffle Bells"), "Shuffle Bells");
            GUI.Label(scRect(442f, y, 206f, 60f, tooltip: "Grass Randomizer"), "Grass Randomizer");
            y += 25f;
            GUI.Label(scRect(170f, y, 56f, 30f), $"{TunicRandomizer.Settings.MysterySeedWeights.ShuffleFuses}%");
            GUI.Label(scRect(386f, y, 56f, 30f), $"{TunicRandomizer.Settings.MysterySeedWeights.ShuffleBells}%");
            GUI.Label(scRect(602f, y, 56f, 30f), $"{TunicRandomizer.Settings.MysterySeedWeights.GrassRando}%");
            y += 10f; 
            TunicRandomizer.Settings.MysterySeedWeights.ShuffleFuses = (int)GUI.HorizontalSlider(scRect(10f, y, 150f, 30f), TunicRandomizer.Settings.MysterySeedWeights.ShuffleFuses, 0, 100);
            TunicRandomizer.Settings.MysterySeedWeights.ShuffleBells = (int)GUI.HorizontalSlider(scRect(226f, y, 150f, 30f), TunicRandomizer.Settings.MysterySeedWeights.ShuffleBells, 0, 100);
            TunicRandomizer.Settings.MysterySeedWeights.GrassRando = (int)GUI.HorizontalSlider(scRect(442f, y, 150f, 30f), TunicRandomizer.Settings.MysterySeedWeights.GrassRando, 0, 100);
            y += 20f;
            GUI.Label(scRect(10f, y, 206f, 60f, tooltip: "Entrance Randomizer"), "Entrance Randomizer");
            GUI.Label(scRect(226f, y, 206f, 60f, tooltip: "Fewer Shops"), "ER: Fewer Shops");
            GUI.Label(scRect(442f, y, 206f, 60f, tooltip: "Matching Directions"), "ER: Matching Directions");
            y += 25f;
            GUI.Label(scRect(170f, y, 56f, 30f), $"{TunicRandomizer.Settings.MysterySeedWeights.EntranceRando}%");
            GUI.Label(scRect(386f, y, 56f, 30f), $"{TunicRandomizer.Settings.MysterySeedWeights.ERFixedShop}%");
            GUI.Label(scRect(602f, y, 56f, 30f), $"{TunicRandomizer.Settings.MysterySeedWeights.ERDirectionPairs}%");
            y += 10f; 
            TunicRandomizer.Settings.MysterySeedWeights.EntranceRando = (int)GUI.HorizontalSlider(scRect(10f, y, 150f, 30f), TunicRandomizer.Settings.MysterySeedWeights.EntranceRando, 0, 100);
            TunicRandomizer.Settings.MysterySeedWeights.ERFixedShop = (int)GUI.HorizontalSlider(scRect(226f, y, 150f, 30f), TunicRandomizer.Settings.MysterySeedWeights.ERFixedShop, 0, 100);
            TunicRandomizer.Settings.MysterySeedWeights.ERDirectionPairs = (int)GUI.HorizontalSlider(scRect(442f, y, 150f, 30f), TunicRandomizer.Settings.MysterySeedWeights.ERDirectionPairs, 0, 100);
            y += 20f;
            GUI.Label(scRect(10f, y, 206f, 60f, tooltip: "Decoupled Entrances"), "ER: Decoupled");
            y += 25f;
            GUI.Label(scRect(170f, y, 56f, 30f), $"{TunicRandomizer.Settings.MysterySeedWeights.ERDecoupled}%");
            y += 10f; 
            TunicRandomizer.Settings.MysterySeedWeights.ERDecoupled = (int)GUI.HorizontalSlider(scRect(10f, y, 150f, 30f), TunicRandomizer.Settings.MysterySeedWeights.ERDecoupled, 0, 100);
            TunicRandomizer.Settings.StartWithSwordEnabled = GUI.Toggle(scRect(442f, y - 25, 206f, 30f, tooltip: "Start With Sword"), TunicRandomizer.Settings.StartWithSwordEnabled, "Start With Sword");
            return y;
        }

        private float MysterySeedAdvancedLogicSection(float y) {
            GUI.Label(scRect(10f, y, 300f, 30f), "Hexagon Quest");
            y += 30f;
            GUI.skin.label.fontSize = scFont(20);
            GUI.Label(scRect(20f, y, 638f, 125f, tooltip: "Randomize Hexagon Quest Amounts"), $"Goal Amount:");
            TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalRandom = GUI.Toggle(scRect(210f, y, 90f, 30f), TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalRandom, "Random");
            TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalLow = GUI.Toggle(scRect(320f, y, 90f, 30f), TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalLow, " Low");
            TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalMedium = GUI.Toggle(scRect(420f, y, 90f, 30f), TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalMedium, "Medium");
            TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalHigh = GUI.Toggle(scRect(520f, y, 90f, 30f), TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalHigh, "High");
            y += 35f;
            GUI.Label(scRect(20f, y, 638f, 60f), $"Extra Hexagons:");
            TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasRandom = GUI.Toggle(scRect(210f, y, 90f, 30f), TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasRandom, "Random");
            TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasLow = GUI.Toggle(scRect(320f, y, 90f, 30f), TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasLow, "Low");
            TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasMedium = GUI.Toggle(scRect(420f, y, 90f, 30f), TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasMedium, "Medium");
            TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasHigh = GUI.Toggle(scRect(520f, y, 90f, 30f), TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasHigh, "High");
            y += 35f;
            GUI.Label(scRect(20f, y, 638f, 30f, tooltip: "Hexagon Quest Ability Shuffle Mode"), $"Ability Shuffle Mode:");
            GUI.Label(scRect(210f, y, 150f, 30f), $"Hexagons: {100 - TunicRandomizer.Settings.MysterySeedWeights.HexQuestAbilityShufflePages}%");
            GUI.Label(scRect(545f, y, 110f, 30f), $"Pages: {TunicRandomizer.Settings.MysterySeedWeights.HexQuestAbilityShufflePages}%");
            TunicRandomizer.Settings.MysterySeedWeights.HexQuestAbilityShufflePages = (int)GUI.HorizontalSlider(scRect(350f, y+10, 185f, 20f), TunicRandomizer.Settings.MysterySeedWeights.HexQuestAbilityShufflePages, 0, 100);
            GUI.skin.label.fontSize = scFont(22.5f);
            y += 40f;
            GUI.Label(scRect(10f, y, 638f, 30f, tooltip: "Fool Traps"), "Fool Traps:");
            TunicRandomizer.Settings.MysterySeedWeights.FoolTrapNone = GUI.Toggle(scRect(210f, y, 90f, 30f), TunicRandomizer.Settings.MysterySeedWeights.FoolTrapNone, "None");
            TunicRandomizer.Settings.MysterySeedWeights.FoolTrapNormal = GUI.Toggle(scRect(320f, y, 90f, 30f), TunicRandomizer.Settings.MysterySeedWeights.FoolTrapNormal, $"<color={FoolColors[1]}>{FoolChoices[1]}</color>");
            TunicRandomizer.Settings.MysterySeedWeights.FoolTrapDouble = GUI.Toggle(scRect(420f, y, 90f, 30f), TunicRandomizer.Settings.MysterySeedWeights.FoolTrapDouble, $"<color={FoolColors[2]}>{FoolChoices[2]}</color>");
            TunicRandomizer.Settings.MysterySeedWeights.FoolTrapOnslaught = GUI.Toggle(scRect(520f, y, 110f, 30f), TunicRandomizer.Settings.MysterySeedWeights.FoolTrapOnslaught, $"<color={FoolColors[3]}>{FoolChoices[3]}</color>");
            y += 35f;
            GUI.skin.label.fontSize = scFont(20f);
            GUI.Label(scRect(10f, y, 638f, 30f, tooltip: "Hero's Laurels Location"), "Hero's Laurels Location:");
            TunicRandomizer.Settings.MysterySeedWeights.LaurelsRandom = GUI.Toggle(scRect(210f, y, 90f, 30f), TunicRandomizer.Settings.MysterySeedWeights.LaurelsRandom, "Random");
            TunicRandomizer.Settings.MysterySeedWeights.LaurelsSixCoins = GUI.Toggle(scRect(320f, y, 90f, 30f), TunicRandomizer.Settings.MysterySeedWeights.LaurelsSixCoins, $"6 Coins");
            TunicRandomizer.Settings.MysterySeedWeights.LaurelsTenCoins = GUI.Toggle(scRect(420f, y, 90f, 30f), TunicRandomizer.Settings.MysterySeedWeights.LaurelsTenCoins, $"10 Coins");
            TunicRandomizer.Settings.MysterySeedWeights.LaurelsTenFairies = GUI.Toggle(scRect(520f, y, 90f, 30f), TunicRandomizer.Settings.MysterySeedWeights.LaurelsTenFairies, $"10 Fairies");
            y += 35f;
            GUI.Label(scRect(10f, y + 10, 206f, 30f), "Difficulty Options:");
            GUI.Label(scRect(226f, y, 206f, 60f, tooltip: "Lanternless Logic"), "Lanternless Logic");
            GUI.Label(scRect(442f, y, 206f, 60f, tooltip: "Maskless Logic"), "Maskless Logic");
            y += 25f;
            GUI.Label(scRect(386f, y, 56f, 30f), $"{TunicRandomizer.Settings.MysterySeedWeights.Lanternless}%");
            GUI.Label(scRect(602f, y, 56f, 30f), $"{TunicRandomizer.Settings.MysterySeedWeights.Maskless}%");
            y += 10f;
            TunicRandomizer.Settings.MysterySeedWeights.Lanternless = (int)GUI.HorizontalSlider(scRect(226f, y, 150f, 30f), TunicRandomizer.Settings.MysterySeedWeights.Lanternless, 0, 100);
            TunicRandomizer.Settings.MysterySeedWeights.Maskless = (int)GUI.HorizontalSlider(scRect(442f, y, 150f, 30f), TunicRandomizer.Settings.MysterySeedWeights.Maskless, 0, 100);
            y += 10f;
            GUI.skin.label.fontSize = scFont(25f);
            GUI.Label(scRect(10f, y, 206f, 30f), "Trick & Glitch Logic");
            GUI.skin.label.fontSize = scFont(20f);
            y += 30f;
            GUI.Label(scRect(20f, y, 190f, 30f, tooltip: "Ice Grapples"), "Ice Grapples:");
            TunicRandomizer.Settings.MysterySeedWeights.IceGrappleOff = GUI.Toggle(scRect(210f, y, 90f, 30f, tooltip: "Ice Grapples Off"), TunicRandomizer.Settings.MysterySeedWeights.IceGrappleOff, "Off");
            TunicRandomizer.Settings.MysterySeedWeights.IceGrappleEasy = GUI.Toggle(scRect(320f, y, 90f, 30f, tooltip: "Ice Grapples Easy"), TunicRandomizer.Settings.MysterySeedWeights.IceGrappleEasy, $"Easy");
            TunicRandomizer.Settings.MysterySeedWeights.IceGrappleMedium = GUI.Toggle(scRect(420f, y, 90f, 30f, tooltip: "Ice Grapples Medium"), TunicRandomizer.Settings.MysterySeedWeights.IceGrappleMedium, $"Medium");
            TunicRandomizer.Settings.MysterySeedWeights.IceGrappleHard = GUI.Toggle(scRect(520f, y, 90f, 30f, tooltip: "Ice Grapples Hard"), TunicRandomizer.Settings.MysterySeedWeights.IceGrappleHard, $"Hard");
            y += 35f;
            GUI.Label(scRect(20f, y, 190f, 30f, tooltip: "Ladder Storage"), "Ladder Storage:");
            TunicRandomizer.Settings.MysterySeedWeights.LadderStorageOff = GUI.Toggle(scRect(210f, y, 90f, 30f, tooltip: "Ladder Storage Off"), TunicRandomizer.Settings.MysterySeedWeights.LadderStorageOff, "Off");
            TunicRandomizer.Settings.MysterySeedWeights.LadderStorageEasy = GUI.Toggle(scRect(320f, y, 90f, 30f, tooltip: "Ladder Storage Easy"), TunicRandomizer.Settings.MysterySeedWeights.LadderStorageEasy, $"Easy");
            TunicRandomizer.Settings.MysterySeedWeights.LadderStorageMedium = GUI.Toggle(scRect(420f, y, 90f, 30f, tooltip: "Ladder Storage Medium"), TunicRandomizer.Settings.MysterySeedWeights.LadderStorageMedium, $"Medium");
            TunicRandomizer.Settings.MysterySeedWeights.LadderStorageHard = GUI.Toggle(scRect(520f, y, 90f, 30f, tooltip: "Ladder Storage Hard"), TunicRandomizer.Settings.MysterySeedWeights.LadderStorageHard, $"Hard");
            y += 35f;
            GUI.Label(scRect(20f, y, 206f, 60f, tooltip: "Laurels Zips"), "Laurels Zips");
            GUI.Label(scRect(236f, y, 350f, 60f, tooltip: "Ladder Storage Without Items"), "Ladder Storage Without Items");
            y += 25f;
            GUI.Label(scRect(180f, y, 56f, 30f), $"{TunicRandomizer.Settings.MysterySeedWeights.LaurelsZips}%");
            GUI.Label(scRect(396f, y, 56f, 30f), $"{TunicRandomizer.Settings.MysterySeedWeights.LadderStorageWithoutItems}%");
            y += 10f;
            TunicRandomizer.Settings.MysterySeedWeights.LaurelsZips = (int)GUI.HorizontalSlider(scRect(20f, y, 150f, 30f), TunicRandomizer.Settings.MysterySeedWeights.LaurelsZips, 0, 100);
            TunicRandomizer.Settings.MysterySeedWeights.LadderStorageWithoutItems = (int)GUI.HorizontalSlider(scRect(236f, y, 150f, 30f), TunicRandomizer.Settings.MysterySeedWeights.LadderStorageWithoutItems, 0, 100);
            GUI.skin.label.fontSize = scFont(25);
            return y;
        }

        private void SetupFoxEditor() {
            GameObject foxPrefab = Resources.Load<GameObject>("_Fox");
            fox = GameObject.Instantiate(foxPrefab);
            fox.transform.localScale = Vector3.one * 12f;
            fox.transform.localPosition = new Vector3(8.3728f, -12.1807f, -1.1f);
            fox.transform.localEulerAngles = new Vector3(0f, 250f, 0f);
            fox.SetActive(false);
            for (int i = 61; i > 3; i--) { 
                GameObject.Destroy(fox.transform.GetChild(i).gameObject);
            }
            GameObject.Destroy(fox.GetComponent<PlayerCharacter>());
            GameObject.Destroy(fox.transform.GetChild(0).GetChild(0).GetChild(8).GetChild(0).GetChild(3).GetComponent<TrackingBone>());
            GameObject.DontDestroyOnLoad(fox);
            TunicLogger.LogInfo(fox.name);
            GameObject head = fox.transform.Find("Fox/root/pelvis/chest/head/").gameObject;
            TunicLogger.LogInfo(head.name);
            foxHead = head.transform;
            laurels = head.transform.GetChild(7).gameObject;
            GameObject.Destroy(laurels.GetComponent<VisibleByHavingInventoryItem>());
            foreach (ItemBehaviour itemBehavior in fox.GetComponents<ItemBehaviour>()) { 
                GameObject.Destroy(itemBehavior);
            }
            foreach (MagicSpell spell in fox.GetComponents<MagicSpell>()) {
                GameObject.Destroy(spell);
            }
            GameObject.Destroy(fox.GetComponent<DPADTester>());
            GameObject.Destroy(fox.GetComponent<FireController>());
            GameObject.Destroy(fox.GetComponent<TrapTileTrigger>());
            GameObject.Destroy(fox.GetComponent<HitReceiver>());

            sunglasses = head.transform.GetChild(8).gameObject;
            fox.layer = 0;
            fox.name = "quick settings fox model";
            foxSetupComplete = true;
        }

        private void CloseAPSettingsWindow() {
            ShowAPSettingsWindow = false;
            stringToEdit = "";
            clearAllEditingFlags();
            RandomizerSettings.SaveSettings();
            Invoke("UnlockButtons", 1);
        }

        private void LockButtons() {
            if (titleScreen != null) {
                titleScreen.lockout = true;
            }
        }

        private void UnlockButtons() {
            if (titleScreen != null) {
                titleScreen.lockout = false;
            }
        }

        public static bool TitleScreen___NewGame_PrefixPatch(TitleScreen __instance) {
            instance.CloseAPSettingsWindow();
            RecentItemsDisplay.instance.ResetQueue();
            if (SaveFlags.IsArchipelago()) {
                Archipelago.instance.integration.ItemIndex = 0;
                Archipelago.instance.integration.ClearQueue();
            }
            return true;
        }

        public static bool FileManagement_LoadFileAndStart_PrefixPatch(FileManagementGUI __instance, string filename) {
            instance.CloseAPSettingsWindow();
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
                                $"\"Found {locationsInLimbo.Count} location{(locationsInLimbo.Count != 1 ? "s" : "")} in the save file\"\n\"that {(locationsInLimbo.Count != 1 ? "were" : "was")} not sent to Archipelago.\"\n" +
                                $"\"Send {(locationsInLimbo.Count != 1 ? "these" : "this")} location{(locationsInLimbo.Count != 1 ? "s" : "")} now?\"";
                            if (TunicRandomizer.Settings.UseTrunicTranslations) {
                                line.text = $"<#FFFF00>[death] <#FFFF00>uhtehn$uhn! <#FFFF00>[death]\nfownd \"{locationsInLimbo.Count}\" lOkA$uhn{(locationsInLimbo.Count != 1 ? "z" : "")} in #uh sAv fIl #aht\n{(locationsInLimbo.Count != 1 ? "wur" : "wawz")} nawt sehnt too RkipehluhgO.\nsehnd {(locationsInLimbo.Count != 1 ? "#Ez" : "#is")} lOkA$uhn{(locationsInLimbo.Count != 1 ? "z" : "")} now?";
                            }
                            GenericPrompt.ShowPrompt(line, (Action)(() => { Archipelago.instance.integration.session.Locations.CompleteLocationChecks(locationsInLimbo.ToArray()); }), (Action)(() => { }));
                        }
                    });
                }
            }
            // if this isn't here then it'll fail to place you at an entrance when you hit load
            ERData.RandomizedPortals.Clear();
            return true;
        }

    }

}
