using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InControl;
using RTLTMPro;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TunicRandomizer {
    public class EntranceSelector : GUIMode {

        public static List<PortalCombo> EntranceOptions;
        public static EntranceSelector instance;
        public static GameObject ButtonObj1;
        public static GameObject ButtonObj2;
        public static GameObject ButtonObj3;
        public static Button SceneButton1;
        public static Button SceneButton2;
        public static Button SceneButton3;

        public static Image SceneImage1;
        public static Image SceneImage2;
        public static Image SceneImage3;

        public static Button RerollButton;

        // placeholders for anything else we add
        public static Button ItemButton2;
        public static Button ItemButton3;
        public static Button ItemButton4;

        public static GameObject SceneText1;
        public static GameObject SceneText2;
        public static GameObject SceneText3;

        public static bool WaitingForDartSelection = false;

        public static void CreateEntranceSelector() {
            InputSequenceAssistanceMenu menuBase = Resources.FindObjectsOfTypeAll<InputSequenceAssistanceMenu>().First();
            GameObject newMenu = GameObject.Instantiate(menuBase.gameObject);
            newMenu.name = "EntranceSelector";
            newMenu.transform.parent = menuBase.transform.parent;
            GameObject.Destroy(newMenu.GetComponent<InputSequenceAssistanceMenu>());
            EntranceSelector entranceSelector = newMenu.AddComponent<EntranceSelector>();
            EntranceSelector.instance = entranceSelector;
            GUIMode.guiModes.Add(entranceSelector);
            GameObject mainBox = newMenu.transform.GetChild(0).gameObject;
            
            GameObject.Destroy(mainBox.GetComponent<Image>());
            for(int i = 2; i < 9; i++) {
                mainBox.transform.GetChild(i).gameObject.SetActive(false);
            }

            GameObject.Destroy(mainBox.transform.GetChild(0).GetChild(0).GetComponent<LocalizeTMP>());
            mainBox.transform.GetChild(0).GetChild(0).GetComponent<RTLTextMeshPro>().text = $"choose wisely, tiny fox...";
            mainBox.transform.GetChild(1).localPosition = new Vector3(0f, -20f, 0f);

            Sprite box = ModelSwaps.FindSprite("UI_box");
            Material UIAdd = ModelSwaps.FindMaterial("UI Add");
            TMP_FontAsset odin = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().Where(Font => Font.name == "Latin Rounded").ToList()[0];

            GameObject ButtonsRoot = new GameObject("Scene Image Buttons");
            ButtonsRoot.transform.parent = mainBox.transform;
            ButtonsRoot.AddComponent<HorizontalLayoutGroup>();
            ButtonsRoot.layer = 5;
            ButtonsRoot.transform.localPosition = new Vector3(-825f, 150f, 0f);

            ButtonObj1 = new GameObject("Button 1");
            ButtonObj1.transform.parent = ButtonsRoot.transform;
            ButtonObj1.layer = 5;
            ButtonObj1.AddComponent<Image>().sprite = box;
            ButtonObj1.transform.localPosition = Vector3.zero;
            SceneButton1 = ButtonObj1.AddComponent<Button>();
            LayoutElement le = ButtonObj1.AddComponent<LayoutElement>();
            le.minHeight = 180;
            le.minWidth = 240;
            GameObject DartIcon1 = new GameObject("dart icon");
            DartIcon1.AddComponent<Image>().sprite = Inventory.GetItemByName("Dart").icon;
            DartIcon1.GetComponent<Image>().material = UIAdd;
            DartIcon1.transform.parent = ButtonObj1.transform;
            DartIcon1.layer = 5;
            DartIcon1.transform.localScale = Vector3.one * 0.5f;
            DartIcon1.transform.localPosition = new Vector3(66, 55, 0);
            DartIcon1.SetActive(false);

            ButtonObj2 = new GameObject("Button 2");
            ButtonObj2.transform.parent = ButtonsRoot.transform;
            ButtonObj2.layer = 5;
            ButtonObj2.AddComponent<Image>().sprite = box;
            ButtonObj2.transform.localPosition = Vector3.zero;
            SceneButton2 = ButtonObj2.AddComponent<Button>();
            LayoutElement le2 = ButtonObj2.AddComponent<LayoutElement>();
            le2.minHeight = 180;
            le2.minWidth = 240;
            GameObject.Instantiate(DartIcon1).transform.parent = ButtonObj2.transform;
            ButtonObj2.transform.GetChild(0).localPosition = new Vector3(66, 55, 0);

            ButtonObj3 = new GameObject("Button 3");
            ButtonObj3.transform.parent = ButtonsRoot.transform;
            ButtonObj3.layer = 5;
            ButtonObj3.AddComponent<Image>().sprite = box;
            ButtonObj3.transform.localPosition = Vector3.zero;
            SceneButton3 = ButtonObj3.AddComponent<Button>();
            LayoutElement le3 = ButtonObj3.AddComponent<LayoutElement>();
            le3.minHeight = 180;
            le3.minWidth = 240;
            GameObject.Instantiate(DartIcon1).transform.parent = ButtonObj3.transform;
            ButtonObj3.transform.GetChild(0).localPosition = new Vector3(66, 55, 0);

            GameObject SceneNamesRoot = new GameObject("Scene Names");
            SceneNamesRoot.transform.parent = mainBox.transform;
            SceneNamesRoot.AddComponent<HorizontalLayoutGroup>().spacing = 40;
            SceneNamesRoot.layer = 5;
            SceneNamesRoot.transform.localPosition = new Vector3(-760f, -150f, 0f);

            SceneText1 = new GameObject("Scene Text 1");
            SceneText1.layer = 5;
            SceneText1.transform.parent = SceneNamesRoot.transform;
            SceneText1.transform.localPosition = Vector3.zero;
            LayoutElement le4 = SceneText1.AddComponent<LayoutElement>();
            le4.minHeight = 150;
            le4.minWidth = 200;
            RTLTextMeshPro st1 = SceneText1.AddComponent<RTLTextMeshPro>();
            st1.fontSize = 12;
            st1.fontMaterial = ModelSwaps.FindMaterial("Latin Rounded - Quantity Outline");
            st1.font = odin;
            st1.alignment = TextAlignmentOptions.Center;
            st1.text = "Scene Choice 1 Scene Choice 1 Scene Choice 1 Scene Choice 1";
            st1.enableWordWrapping = true;

            SceneText2 = new GameObject("Scene Text 2");
            SceneText2.layer = 5;
            SceneText2.transform.parent = SceneNamesRoot.transform;
            SceneText2.transform.localPosition = Vector3.zero;
            LayoutElement le5 = SceneText2.AddComponent<LayoutElement>();
            le5.minHeight = 150;
            le5.minWidth = 200;
            RTLTextMeshPro st2 = SceneText2.AddComponent<RTLTextMeshPro>();
            st2.fontSize = 12;
            st2.fontMaterial = ModelSwaps.FindMaterial("Latin Rounded - Quantity Outline");
            st2.font = odin;
            st2.alignment = TextAlignmentOptions.Center;
            st2.text = "Scene Choice 2 Scene Choice 2 Scene Choice 2 Scene Choice 2";
            st2.enableWordWrapping = true;

            SceneText3 = new GameObject("Scene Text 3");
            SceneText3.layer = 5;
            SceneText3.transform.parent = SceneNamesRoot.transform;
            SceneText3.transform.localPosition = Vector3.zero;
            LayoutElement le6 = SceneText3.AddComponent<LayoutElement>();
            le6.minHeight = 150;
            le6.minWidth = 200;
            RTLTextMeshPro st3 = SceneText3.AddComponent<RTLTextMeshPro>();
            st3.fontSize = 12;
            st3.fontMaterial = ModelSwaps.FindMaterial("Latin Rounded - Quantity Outline");
            st3.font = odin;
            st3.alignment = TextAlignmentOptions.Center;
            st3.text = "Scene Choice 3 Scene Choice 3 Scene Choice 3 Scene Choice 3";
            st3.enableWordWrapping = true;

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
            ItemButton3 = ItemButtonObj2.GetComponent<Button>();

            GameObject ItemButtonObj4 = GameObject.Instantiate(RerollButtonObj);
            ItemButtonObj4.transform.parent = ItemButtons.transform;
            ItemButtonObj4.transform.localPosition = new Vector3(ItemButtonObj4.transform.localPosition.x, ItemButtonObj4.transform.localPosition.y, 200);
            ItemButtonObj4.transform.localScale = Vector3.one;
            ItemButton4 = ItemButtonObj2.GetComponent<Button>();

            RerollButtonObj.transform.localScale = Vector3.one;
        }

        public void FirstChoice() {
            if (WaitingForDartSelection) {
                // TODO save the pinned entrance in save file or something
                WaitingForDartSelection = false;
                return;
            }
            if (EntranceOptions.Count > 0) {
                TunicLogger.LogInfo("Chose first scene");
                Notifications.Show($"\"Chose first option.\"", $"\"{EntranceOptions[0].Portal1.Name} -> {EntranceOptions[0].Portal2.Name}\"");
                FoxPrince.BPPortalChosen(EntranceOptions[0]);
                cleanup();
            }
        }

        public void PinSelection(string portalName) {
            SaveFile.SetString($"randomizer bp pinned portal", portalName);
        }

        public void SecondChoice() {
            if (WaitingForDartSelection) {
                WaitingForDartSelection = false;
                return;
            }
            if (EntranceOptions.Count > 0) {
            TunicLogger.LogInfo("Chose second scene");
                Notifications.Show($"\"Chose second option.\"", $"\"{EntranceOptions[1].Portal1.Name} -> {EntranceOptions[1].Portal2.Name}\"");
                FoxPrince.BPPortalChosen(EntranceOptions[1]);
                cleanup();
            }
        }

        public void ThirdChoice() {
            if (WaitingForDartSelection) {
                WaitingForDartSelection = false;
                return;
            }
            TunicLogger.LogInfo("Chose third scene");
            if (EntranceOptions.Count > 0) {
                Notifications.Show($"\"Chose third option.\"", $"\"{EntranceOptions[2].Portal1.Name} -> {EntranceOptions[2].Portal2.Name}\"");
                FoxPrince.BPPortalChosen(EntranceOptions[2]);
                cleanup();
            }
        }

        public static List<PortalCombo> RerollAlreadySeen = new List<PortalCombo>();

        public void RerollChoices() {
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
                newPortals = FoxPrince.BPGetThreePortals(SaveFile.GetInt("seed"), FoxPrince.CurrentPortal.name, RerollAlreadySeen);
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
            WaitingForDartSelection = true;
            EventSystem.current.SetSelectedGameObject(EntranceSelector.ButtonObj1);
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
            ItemButton3.gameObject.AddComponent<SceneSelectionButton>();
            ItemButton3.gameObject.AddComponent<SceneSelectionButton>();
            ItemButton4.gameObject.AddComponent<SceneSelectionButton>();

            SceneButton1.onClick.AddListener((UnityAction)FirstChoice);
            SceneButton2.onClick.AddListener((UnityAction)SecondChoice);
            SceneButton3.onClick.AddListener((UnityAction)ThirdChoice);
            RerollButton.onClick.AddListener((UnityAction)RerollChoices);
            ItemButton2.onClick.AddListener((UnityAction)ActivatePin);
            ItemButton3.onClick.AddListener((UnityAction)Item3);
            ItemButton4.onClick.AddListener((UnityAction)Item4);
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
            }
            EntranceOptions = portalChoices;

            RerollButton.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.GetItemByName("Soul Dice").icon;
            RerollButton.transform.GetChild(1).GetComponent<RTLTextMeshPro>().text = Inventory.GetItemByName("Soul Dice").Quantity.ToString();
            ItemButton2.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.GetItemByName("Dart").icon;
            ItemButton2.transform.GetChild(1).GetComponent<RTLTextMeshPro>().text = Inventory.GetItemByName("Dart").Quantity.ToString();
            GUIMode.PushMode(EntranceSelector.instance);
            EventSystem.current.SetSelectedGameObject(EntranceSelector.ButtonObj1);
        }

        public void cleanup() {
            TunicLogger.LogInfo("cleanup started");
            RerollAlreadySeen.Clear();
            GUIMode.ClearStack();
            GUIMode.PushGameMode();
            TunicLogger.LogInfo("cleanup done");
        }

        public void Update() {
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
