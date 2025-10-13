using System.Collections.Generic;
using System.Linq;
using RTLTMPro;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static TunicRandomizer.SceneImageData;

namespace TunicRandomizer {
    public class EntranceSelector : GUIMode {

        public static List<PortalCombo> EntranceOptions;
        public static EntranceSelector instance;
        public static GameObject Layout1;
        public static GameObject Layout2;
        public static GameObject Layout3;
        public static GameObject ButtonObj1;
        public static GameObject ButtonObj2;
        public static GameObject ButtonObj3;
        public static Button SceneButton1;
        public static Button SceneButton2;
        public static Button SceneButton3;

        public static Button RerollButton;

        // placeholders for anything else we add
        public static Button ItemButton2;
        public static Button ItemButton3;
        public static Button ItemButton4;

        public static GameObject SceneText1;
        public static GameObject SceneText2;
        public static GameObject SceneText3;

        public static SpriteRenderer SceneSprite1;
        public static SpriteRenderer SceneSprite2;
        public static SpriteRenderer SceneSprite3;

        public static bool WaitingForDartSelection = false;

        private IEnumerator<bool> animationHandler;
        private bool readyForInputs = false;

        public static void CreateEntranceSelector() {
            InputSequenceAssistanceMenu menuBase = Resources.FindObjectsOfTypeAll<InputSequenceAssistanceMenu>().First();
            GameObject newMenu = GameObject.Instantiate(menuBase.gameObject);
            newMenu.name = "EntranceSelector";
            newMenu.transform.parent = menuBase.transform.parent;
            GameObject.Destroy(newMenu.GetComponent<InputSequenceAssistanceMenu>());
            EntranceSelector entranceSelector = newMenu.AddComponent<EntranceSelector>();
            EntranceSelector.instance = entranceSelector;
            GUIMode.guiModes.Add(entranceSelector);
            newMenu.GetComponent<CanvasScaler>().matchWidthOrHeight = 0.5f;
            newMenu.GetComponent<CanvasScaler>().scaleFactor = 0.5f;
            GameObject mainBox = newMenu.transform.GetChild(0).gameObject;
            
            GameObject.Destroy(mainBox.GetComponent<Image>());
            for(int i = 2; i < 9; i++) {
                mainBox.transform.GetChild(i).gameObject.SetActive(false);
            }

            GameObject.Destroy(mainBox.transform.GetChild(0).GetChild(0).GetComponent<LocalizeTMP>());
            mainBox.transform.GetChild(0).GetChild(0).GetComponent<RTLTextMeshPro>().text = $"choose wisely, tiny fox...";
            mainBox.transform.GetChild(1).localPosition = Vector3.zero;
            mainBox.transform.GetChild(1).localScale = new Vector3(1.3f, 1f, 1f);

            Sprite box = ModelSwaps.FindSprite("UI_box");
            Material UIAdd = ModelSwaps.FindMaterial("UI Add");
            TMP_FontAsset odin = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().Where(Font => Font.name == "Latin Rounded").ToList()[0];

            GameObject ButtonsRoot = new GameObject("Scene Image Buttons");
            ButtonsRoot.transform.parent = mainBox.transform;
            ButtonsRoot.transform.localScale = Vector3.one * 2;
            ButtonsRoot.AddComponent<HorizontalLayoutGroup>();
            ButtonsRoot.GetComponent<RectTransform>().sizeDelta = new Vector2(950, 250);
            ButtonsRoot.layer = 5;
            ButtonsRoot.transform.localPosition = new Vector3(0, -850, 0);

            Layout1 = new GameObject("Layout");
            Layout1.AddComponent<VerticalLayoutGroup>();
            Layout1.transform.parent = ButtonsRoot.transform;
            Layout1.layer = 5;
            Layout1.SetActive(true);

            ButtonObj1 = new GameObject("Button");
            ButtonObj1.transform.parent = Layout1.transform;
            ButtonObj1.layer = 5;
            ButtonObj1.AddComponent<Image>().sprite = box;
            ButtonObj1.transform.localPosition = Vector3.zero;
            ButtonObj1.transform.localScale = Vector3.one;
            ButtonObj1.SetActive(true);
            SceneButton1 = ButtonObj1.AddComponent<Button>();
            GameObject DartIcon1 = new GameObject("dart icon");
            DartIcon1.AddComponent<Image>().sprite = Inventory.GetItemByName("Dart").icon;
            DartIcon1.GetComponent<Image>().material = UIAdd;
            DartIcon1.transform.parent = ButtonObj1.transform;
            DartIcon1.layer = 5;
            DartIcon1.transform.localScale = Vector3.one * 0.5f;
            DartIcon1.transform.localPosition = new Vector3(66, 55, 0);
            DartIcon1.SetActive(false);

            SceneText1 = new GameObject("Scene Text");
            SceneText1.layer = 5;
            SceneText1.transform.parent = Layout1.transform;
            SceneText1.transform.localPosition = Vector3.zero;
            SceneText1.SetActive(true);
            RTLTextMeshPro st1 = SceneText1.AddComponent<RTLTextMeshPro>();
            st1.fontSize = 12;
            st1.fontMaterial = ModelSwaps.FindMaterial("Latin Rounded - Quantity Outline");
            st1.font = odin;
            st1.alignment = TextAlignmentOptions.Center;
            st1.text = "Scene Choice 1 Scene Choice 1 Scene Choice 1 Scene Choice 1";
            st1.enableWordWrapping = true;

            GameObject sceneImage = new GameObject("scene image");
            SceneSprite1 = sceneImage.AddComponent<SpriteRenderer>();
            sceneImage.transform.parent = ButtonObj1.transform;
            sceneImage.layer = 5;
            sceneImage.transform.localScale = new Vector3(70, 80, 70);
            sceneImage.transform.localPosition = new Vector3(0, 12, 7);
            sceneImage.SetActive(true);

            Layout2 = GameObject.Instantiate(Layout1);
            Layout2.transform.parent = Layout1.transform.parent;
            Layout2.SetActive(true);
            ButtonObj2 = Layout2.transform.GetChild(0).gameObject;
            SceneButton2 = ButtonObj2.GetComponent<Button>();
            SceneSprite2 = ButtonObj2.transform.GetChild(1).GetComponent<SpriteRenderer>(); 
            SceneText2 = Layout2.transform.GetChild(1).gameObject;

            Layout3 = GameObject.Instantiate(Layout1);
            Layout3.transform.parent = Layout1.transform.parent;
            Layout3.SetActive(true);
            ButtonObj3 = Layout3.transform.GetChild(0).gameObject;
            SceneButton3 = ButtonObj3.GetComponent<Button>();
            SceneSprite3 = ButtonObj3.transform.GetChild(1).GetComponent<SpriteRenderer>();
            SceneText3 = Layout3.transform.GetChild(1).gameObject;

            GameObject ItemButtons = new GameObject("Item Buttons");
            ItemButtons.layer = 5;
            ItemButtons.transform.parent = mainBox.transform;
            ItemButtons.transform.localScale = Vector3.one * 2f;
            ItemButtons.transform.localPosition = new Vector3(-500f, -420f, 200f);
            ItemButtons.AddComponent<HorizontalLayoutGroup>();

            GameObject RerollButtonObj = new GameObject("Reroll button");
            RerollButtonObj.layer = 5;
            RerollButtonObj.transform.localScale = Vector3.one;
            RerollButtonObj.transform.parent = ItemButtons.transform;
            RerollButtonObj.transform.localPosition = new Vector3(RerollButtonObj.transform.localPosition.x, RerollButtonObj.transform.localPosition.y, 200);
            RerollButtonObj.AddComponent<LayoutElement>().minWidth = 150;
            RerollButtonObj.AddComponent<Image>().sprite = box;
            RerollButton = RerollButtonObj.AddComponent<Button>();

            GameObject RerollIcon = new GameObject("item icon");
            RerollIcon.transform.parent = RerollButtonObj.transform;
            RerollIcon.transform.localScale = Vector3.one;
            RerollIcon.transform.localPosition = new Vector3(-23f, 3f, 0f);
            RerollIcon.transform.localScale = Vector3.one * 0.5f;
            RerollIcon.layer = 5;
            RerollIcon.AddComponent<Image>();
            RerollIcon.GetComponent<Image>().material = UIAdd;

            GameObject RerollText = new GameObject("item quantity");
            RerollText.transform.localScale = Vector3.one;
            RerollText.transform.parent = RerollButtonObj.transform;
            RerollText.transform.localPosition = new Vector3(110.6092f, 0f, -76.9091f);
            RerollText.transform.localScale = Vector3.one;
            RerollText.layer = 5;
            RTLTextMeshPro rerollText = RerollText.AddComponent<RTLTextMeshPro>();
            rerollText.font = odin;

            GameObject ItemButtonObj2 = GameObject.Instantiate(RerollButtonObj);
            ItemButtonObj2.transform.parent = ItemButtons.transform;
            ItemButtonObj2.transform.localScale = Vector3.one;
            ItemButtonObj2.transform.localPosition = new Vector3(ItemButtonObj2.transform.localPosition.x, ItemButtonObj2.transform.localPosition.y, 200);
            ItemButtonObj2.transform.GetChild(0).transform.localScale = Vector3.one * 0.75f;
            ItemButton2 = ItemButtonObj2.GetComponent<Button>();

            GameObject ItemButtonObj3 = GameObject.Instantiate(RerollButtonObj);
            ItemButtonObj3.transform.parent = ItemButtons.transform;
            ItemButtonObj3.transform.localScale = Vector3.one;
            ItemButtonObj3.transform.localPosition = new Vector3(ItemButtonObj3.transform.localPosition.x, ItemButtonObj3.transform.localPosition.y, 200);
            ItemButton3 = ItemButtonObj3.GetComponent<Button>();

            GameObject ItemButtonObj4 = GameObject.Instantiate(RerollButtonObj);
            ItemButtonObj4.transform.parent = ItemButtons.transform;
            ItemButtonObj4.transform.localPosition = new Vector3(ItemButtonObj4.transform.localPosition.x, ItemButtonObj4.transform.localPosition.y, 200);
            ItemButtonObj4.transform.localScale = Vector3.one;
            ItemButton4 = ItemButtonObj4.GetComponent<Button>();

            RerollButtonObj.transform.localScale = Vector3.one;
        }

        public void FirstChoice() {
            if (!readyForInputs) {
                return;
            }
            if (WaitingForDartSelection) {
                PinSelection(EntranceOptions[0].Portal2.Name);
                // todo: maybe also some way to graphically show that it's pinned
                return;
            }
            if (EntranceOptions.Count > 0) {
                TunicLogger.LogInfo("Chose first scene");
                Notifications.Show($"\"Chose first option.\"", $"\"{EntranceOptions[0].Portal1.Name} -> {EntranceOptions[0].Portal2.Name}\"");
                FoxPrince.FPPortalChosen(EntranceOptions[0]);
                cleanup();
            }
        }

        public void SecondChoice() {
            if (!readyForInputs) {
                return;
            }
            if (WaitingForDartSelection) {
                PinSelection(EntranceOptions[1].Portal2.Name);
                return;
            }
            if (EntranceOptions.Count > 0) {
            TunicLogger.LogInfo("Chose second scene");
                Notifications.Show($"\"Chose second option.\"", $"\"{EntranceOptions[1].Portal1.Name} -> {EntranceOptions[1].Portal2.Name}\"");
                FoxPrince.FPPortalChosen(EntranceOptions[1]);
                cleanup();
            }
        }

        public void ThirdChoice() {
            if (!readyForInputs) {
                return;
            }
            if (WaitingForDartSelection) {
                PinSelection(EntranceOptions[2].Portal2.Name);
                return;
            }
            TunicLogger.LogInfo("Chose third scene");
            if (EntranceOptions.Count > 0) {
                Notifications.Show($"\"Chose third option.\"", $"\"{EntranceOptions[2].Portal1.Name} -> {EntranceOptions[2].Portal2.Name}\"");
                FoxPrince.FPPortalChosen(EntranceOptions[2]);
                cleanup();
            }
        }

        public static List<PortalCombo> RerollAlreadySeen = new List<PortalCombo>();

        public void RerollChoices() {
            if (!readyForInputs) {
                return;
            }
            // todo add sfx on button press for successful or 
            Item SoulDice = Inventory.GetItemByName("Soul Dice");
            List<PortalCombo> newPortals = null;
            if (SoulDice.Quantity > 0 && EntranceOptions.Count == 3) {
                TunicLogger.LogInfo("rerolling");
                foreach (PortalCombo portalCombo in EntranceOptions) {
                    if (!RerollAlreadySeen.Contains(portalCombo)) {
                        RerollAlreadySeen.Add(portalCombo);
                    }
                }
                newPortals = FoxPrince.FPGetThreePortals(SaveFile.GetInt("seed"), FoxPrince.CurrentPortal.name, RerollAlreadySeen);
                if (newPortals != null) {
                    SoulDice.Quantity -= 1;
                    // if it gave us less than 3 portals, we need to reuse some of the previous ones
                    System.Random random = new System.Random(SaveFile.GetInt("seed"));
                    TunicUtils.ShuffleList(RerollAlreadySeen, SaveFile.GetInt("seed"));
                    HashSet<int> usedIndexes = new HashSet<int>();
                    while (newPortals.Count < 3) {
                        int index = random.Next(RerollAlreadySeen.Count);
                        while (usedIndexes.Contains(index)) {
                            index = random.Next(RerollAlreadySeen.Count);
                        }
                        newPortals.Add(RerollAlreadySeen[index]);
                    }
                }
                EntranceOptions = newPortals;
                ShowSelection(EntranceOptions);
            }
            // indicates that either you have no dice left or there are less than 2 options
            if (newPortals == null) {
                // do whatever we would do if you can't reroll
                TunicLogger.LogInfo("Cannot reroll at this time");
            }
        }

        public void ActivatePin() {
            if (!readyForInputs) {
                return;
            }
            Item Pin = Inventory.GetItemByName("Dart");
            if (Pin.Quantity == 0) { return; }
            WaitingForDartSelection = true;
            EventSystem.current.SetSelectedGameObject(EntranceSelector.ButtonObj1);
        }

        public void PinSelection(string portalName) {
            SaveFile.SetString(SaveFlags.FPPinnedPortalFlag, portalName);
            FoxPrince.PinnedPortal = portalName;
            Item Pin = Inventory.GetItemByName("Dart");
            Pin.Quantity -= 1;
            WaitingForDartSelection = false;
        }

        public void Item3() {

        }

        public void Item4() {
        
        }

        public void Awake() {
            ButtonObj1.AddComponent<SceneSelectionButton>().isSceneButton = true;
            ButtonObj1.GetComponent<SceneSelectionButton>().dartIcon = ButtonObj1.transform.GetChild(0).GetComponent<Image>();
            ButtonObj2.AddComponent<SceneSelectionButton>().isSceneButton = true;
            ButtonObj2.GetComponent<SceneSelectionButton>().dartIcon = ButtonObj2.transform.GetChild(0).GetComponent<Image>();
            ButtonObj3.AddComponent<SceneSelectionButton>().isSceneButton = true;
            ButtonObj3.GetComponent<SceneSelectionButton>().dartIcon = ButtonObj3.transform.GetChild(0).GetComponent<Image>();
            RerollButton.gameObject.AddComponent<SceneSelectionButton>();
            ItemButton2.gameObject.AddComponent<SceneSelectionButton>();
            ItemButton3.gameObject.AddComponent<SceneSelectionButton>();
            ItemButton4.gameObject.AddComponent<SceneSelectionButton>();

            SceneButton1.onClick.AddListener((UnityAction)FirstChoice);
            SceneButton2.onClick.AddListener((UnityAction)SecondChoice);
            SceneButton3.onClick.AddListener((UnityAction)ThirdChoice);
            RerollButton.onClick.AddListener((UnityAction)RerollChoices);
            ItemButton2.onClick.AddListener((UnityAction)ActivatePin);
            ItemButton3.onClick.AddListener((UnityAction)Item3);
            ItemButton4.onClick.AddListener((UnityAction)Item4);

            animationHandler = AnimationHandler();
        }

        public void ShowSelection(List<PortalCombo> portalChoices) {
            TunicLogger.LogInfo("Showing Selections:");
            foreach (PortalCombo combo in portalChoices) {
                TunicLogger.LogInfo($"{combo.Portal1.Name}, {combo.Portal2.Name}");
            }
            if (portalChoices.Count > 0) { 
                SceneText1.GetComponent<RTLTextMeshPro>().text = portalChoices[0].Portal2.Name;
                SceneText2.GetComponent<RTLTextMeshPro>().text = portalChoices[1].Portal2.Name;
                SceneText3.GetComponent<RTLTextMeshPro>().text = portalChoices[2].Portal2.Name;
                if (SceneImageData.SceneImages.ContainsKey(portalChoices[0].Portal2.Name)) {
                    SceneImage image = SceneImageData.SceneImages[portalChoices[0].Portal2.Name];
                    SceneSprite1.sprite = SceneImageData.createSprite(SceneImageData.SceneImages[portalChoices[0].Portal2.Name]);
                    ResizeImage(image, SceneSprite1);
                }
                if (SceneImageData.SceneImages.ContainsKey(portalChoices[1].Portal2.Name)) {
                    SceneImage image = SceneImageData.SceneImages[portalChoices[1].Portal2.Name];
                    SceneSprite2.sprite = SceneImageData.createSprite(image);
                    ResizeImage(image, SceneSprite2);
                }
                if (SceneImageData.SceneImages.ContainsKey(portalChoices[2].Portal2.Name)) {
                    SceneImage image = SceneImageData.SceneImages[portalChoices[2].Portal2.Name];
                    SceneSprite3.sprite = SceneImageData.createSprite(image);
                    ResizeImage(image, SceneSprite3);
                }
            }
            EntranceOptions = portalChoices;

            RerollButton.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.GetItemByName("Soul Dice").icon;
            RerollButton.transform.GetChild(1).GetComponent<RTLTextMeshPro>().text = Inventory.GetItemByName("Soul Dice").Quantity.ToString();
            ItemButton2.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.GetItemByName("Dart").icon;
            ItemButton2.transform.GetChild(1).GetComponent<RTLTextMeshPro>().text = Inventory.GetItemByName("Dart").Quantity.ToString();
            ToggleItemRow(false);
            readyForInputs = false;
            animationHandler = AnimationHandler();
            GUIMode.PushMode(EntranceSelector.instance);
            EventSystem.current.SetSelectedGameObject(ButtonObj1);
        }

        private void ResizeImage(SceneImage image, SpriteRenderer sr) {
            if (image.Width > 300) {
                sr.transform.localScale = new Vector3(35, 40, 35);
            } else {
                sr.transform.localScale = new Vector3(70, 80, 70);
            }
        }

        public void cleanup() {
            Layout1.transform.localPosition = new Vector3(Layout1.transform.localPosition.x, 0, 200);
            Layout2.transform.localPosition = new Vector3(Layout2.transform.localPosition.x, 0, 200);
            Layout3.transform.localPosition = new Vector3(Layout3.transform.localPosition.x, 0, 200);
            ToggleItemRow(false);
            readyForInputs = false;
            TunicLogger.LogInfo("cleanup started");
            RerollAlreadySeen.Clear();
            GUIMode.ClearStack();
            GUIMode.PushGameMode();
            TunicLogger.LogInfo("cleanup done");
        }

        public void Update() {
            if (animationHandler != null) {
                animationHandler.MoveNext();
            }
        }

        private IEnumerator<bool> AnimationHandler() {
            yield return true;
            Layout1.transform.localPosition = new Vector3(Layout1.transform.localPosition.x, 0, 200);
            Layout2.transform.localPosition = new Vector3(Layout2.transform.localPosition.x, 0, 200);
            Layout3.transform.localPosition = new Vector3(Layout3.transform.localPosition.x, 0, 200);
            SceneSprite1.transform.localPosition = new Vector3(SceneSprite1.transform.localPosition.x, SceneSprite1.transform.localPosition.y, 200);
            SceneSprite2.transform.localPosition = new Vector3(SceneSprite2.transform.localPosition.x, SceneSprite2.transform.localPosition.y, 200);
            SceneSprite3.transform.localPosition = new Vector3(SceneSprite3.transform.localPosition.x, SceneSprite3.transform.localPosition.y, 200);
            yield return true;
            while (Layout3.transform.localPosition.y < 435) {
                if (Layout1.transform.localPosition.y < 435) {
                    Layout1.transform.localPosition += new Vector3(0, 15, 0);
                }
                if (Layout2.transform.localPosition.y < 435 && Layout1.transform.localPosition.y > 100) {
                    Layout2.transform.localPosition += new Vector3(0, 15, 0);
                }
                if (Layout3.transform.localPosition.y < 435 && Layout2.transform.localPosition.y > 100) {
                    Layout3.transform.localPosition += new Vector3(0, 15, 0);
                }
                yield return true;
            }
            ToggleItemRow(true);
            readyForInputs = true;
        }

        private static void ToggleItemRow(bool state) {
            RerollButton.gameObject.SetActive(state);
            ItemButton2.gameObject.SetActive(state);
            ItemButton3.gameObject.SetActive(state);
            ItemButton4.gameObject.SetActive(state);
        }

        public static bool GUIMode_PauseTime_GetterPatch(GUIMode __instance, ref bool __result) {

            if (GUIMode.topMode != null && GUIMode.topMode.TryCast<EntranceSelector>() != null) {
                __result = true;
                return false;
            }

            return true;
        }
    }
}
