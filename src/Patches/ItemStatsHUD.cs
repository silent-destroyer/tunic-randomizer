using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using BepInEx.Logging;

namespace TunicRandomizer {
    public class ItemStatsHUD {
        private static ManualLogSource Logger = TunicRandomizer.Logger;

        public static bool Loaded = false;
        public static GameObject Title;
        public static GameObject Pages;
        public static GameObject Fairies;
        public static GameObject Treasures;
        public static GameObject CoinsTossed;
        public static GameObject ThisArea;
        public static GameObject Total;
        public static GameObject GoldHexagons;

        public static GameObject GuardCaptain;
        public static GameObject Ding;
        public static GameObject GardenKnight;
        public static GameObject Dong;
        public static GameObject SiegeEngine;
        public static GameObject RedHexagon;
        public static GameObject Librarian;
        public static GameObject GreenHexagon;
        public static GameObject BossScavenger;
        public static GameObject BlueHexagon;
        public static GameObject HexagonQuest;
        public static GameObject QuestionMark;
        public static List<GameObject> SecondSwordIcons = new List<GameObject>();
        public static List<GameObject> ThirdSwordIcons = new List<GameObject>();
        public static List<GameObject> EquipButtons = new List<GameObject>();
        public static void Initialize() {
            if (!Loaded) {
                TMP_FontAsset FontAsset = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().Where(Font => Font.name == "Latin Rounded").ToList()[0];
                Material FontMaterial = Resources.FindObjectsOfTypeAll<Material>().Where(Material => Material.name == "Latin Rounded - Quantity Outline").ToList()[0];
                GameObject Stats = new GameObject("randomizer stats");
                Stats.transform.parent = GameObject.Find("_GameGUI(Clone)/HUD Canvas/Scaler/Inventory/Inventory Subscreen/").transform;

                GameObject Backing = new GameObject("backing");
                Backing.AddComponent<Image>().sprite = Resources.FindObjectsOfTypeAll<Sprite>().Where(Sprite => Sprite.name == "UI_offeringBacking").ToList()[0];
                Backing.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
                Backing.transform.localScale = new Vector3(5f, 3.75f, 0f);
                Backing.transform.position = new Vector3(220f, -35f, 0);
                Backing.layer = 5;
                Backing.transform.parent = Stats.transform;
                GameObject.DontDestroyOnLoad(Backing);

                Title = SetupText("title", new Vector3(275f, 35f, 0f), Stats.transform, 24f, FontAsset, FontMaterial);
                //Title = SetupTitle(Stats.transform);
                // sep line 3.4464 -0.0155 0.05, 217.3751 27.471 0
                Sprite SepLine = Resources.FindObjectsOfTypeAll<Sprite>().Where(Sprite => Sprite.name == "UI_separator_line_single").ToList()[0];
                GameObject SepLine1 = new GameObject("sep line");
                SepLine1.AddComponent<Image>().sprite = SepLine;
                SepLine1.transform.parent = Stats.transform;
                SepLine1.transform.localScale = new Vector3(1.0337f, -0.0155f, 0.05f);
                SepLine1.transform.position = new Vector3(97.41f, 29.9839f, 0f);
                GameObject SepLine2 = new GameObject("sep line");
                SepLine2.AddComponent<Image>().sprite = SepLine;
                SepLine2.transform.parent = Stats.transform;
                SepLine2.transform.localScale = new Vector3(3.4464f, -0.0155f, 0.05f);
                SepLine2.transform.position = new Vector3(217.3751f, 27.471f, 0f);
                // sep line 1.0337 -0.0155 0.05, 97.41 29.9839 0
                Pages = SetupText("pages", new Vector3(195f, 0f, 0f), Stats.transform, 20f, FontAsset, FontMaterial);
                Fairies = SetupText("fairies", new Vector3(370f, 0f, 0f), Stats.transform, 20f, FontAsset, FontMaterial);
                Treasures = SetupText("treasures", new Vector3(195f, -30f, 0f), Stats.transform, 20f, FontAsset, FontMaterial);
                CoinsTossed = SetupText("coins tossed", new Vector3(370f, -30f, 0f), Stats.transform, 20f, FontAsset, FontMaterial);
                ThisArea = SetupText("this area", new Vector3(195f, -60f, 0f), Stats.transform, 20f, FontAsset, FontMaterial);
                Total = SetupText("total", new Vector3(370f, -60f, 0f), Stats.transform, 20f, FontAsset, FontMaterial);
                GameObject.DontDestroyOnLoad(Title);
                GameObject.DontDestroyOnLoad(SepLine1);
                GameObject.DontDestroyOnLoad(SepLine2);
                GameObject.DontDestroyOnLoad(Pages);
                GameObject.DontDestroyOnLoad(Fairies);
                GameObject.DontDestroyOnLoad(Treasures);
                GameObject.DontDestroyOnLoad(CoinsTossed);
                GameObject.DontDestroyOnLoad(ThisArea);
                GameObject.DontDestroyOnLoad(Total);
                HexagonQuest = new GameObject("hexagon quest");
                HexagonQuest.transform.parent = GameObject.Find("_GameGUI(Clone)/AreaLabels").transform;
                HexagonQuest.transform.position = Vector3.zero;
                GameObject HexBacking = new GameObject("backing");
                HexBacking.AddComponent<Image>().sprite = Resources.FindObjectsOfTypeAll<Sprite>().Where(sprite => sprite.name == "game gui_button circle").ToList()[0];
                HexBacking.transform.parent = HexagonQuest.transform;
                HexBacking.transform.position = new Vector3(-445f, 210f, 0f);
                HexBacking.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                GameObject.Instantiate(ModelSwaps.HexagonGoldImage, HexBacking.transform);
                HexBacking.transform.GetChild(0).localScale = new Vector3(0.75f, 0.75f, 0.75f);
                GoldHexagons = SetupText("gold hexagons", new Vector3(-237f, 200f, 0f), HexagonQuest.transform, 24f, FontAsset, FontMaterial);
                GoldHexagons.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 50f);
                GoldHexagons.transform.parent = HexagonQuest.transform;
                if (Screen.width <= 1280 && Screen.height <= 800) {
                    HexagonQuest.transform.position = new Vector3(50f, 0f, 100f);
                }
                GameObject.DontDestroyOnLoad(HexagonQuest);
 

                Material ImageMaterial = Resources.FindObjectsOfTypeAll<Material>().Where(mat => mat.name == "UI Add").ToList()[0];

                GuardCaptain = SetupImage("guard captain", "Alphabet New_91", Stats.transform, new Vector3(57.5f, -85f, 0f), ImageMaterial);
                Ding = SetupImage("ding", "Alphabet New_93", Stats.transform, new Vector3(92.5f, -85f, 0f), ImageMaterial);
                GardenKnight = SetupImage("garden knight", "Alphabet New_91", Stats.transform, new Vector3(127.5f, -85f, 0f), ImageMaterial);
                //Dong = SetupImage("dong", "Alphabet New_93", Stats.transform, new Vector3(162.5f, -85f, 0f), ImageMaterial);
                SiegeEngine = SetupImage("siege engine", "Alphabet New_91", Stats.transform, new Vector3(197.5f, -85f, 0f), ImageMaterial);
                RedHexagon = SetupImage("red hexagon", "Alphabet New_98", Stats.transform, new Vector3(232.5f, -84f, 0f), ImageMaterial);
                Librarian = SetupImage("librarian", "Alphabet New_91", Stats.transform, new Vector3(267.5f, -85f, 0f), ImageMaterial);
                GreenHexagon = SetupImage("green hexagon", "Alphabet New_98", Stats.transform, new Vector3(302.5f, -84f, 0f), ImageMaterial);
                BossScavenger = SetupImage("boss scavenger", "Alphabet New_91", Stats.transform, new Vector3(337.5f, -85f, 0f), ImageMaterial);
                BlueHexagon = SetupImage("blue hexagon", "Alphabet New_98", Stats.transform, new Vector3(372.5f, -84f, 0f), ImageMaterial);

                RedHexagon.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                GreenHexagon.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                BlueHexagon.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                GameObject D = SetupImage("d", "Alphabet New_24", Ding.transform, new Vector3(87f, -110f, 0f), ImageMaterial);
                D.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                GameObject I = SetupImage("i", "Alphabet New_2", D.transform, new Vector3(87f, -110f, 0f), ImageMaterial);
                I.transform.localScale = Vector3.one;
                GameObject NG = SetupImage("ng", "Alphabet New_20", Ding.transform, new Vector3(98f, -110f, 0f), ImageMaterial);
                NG.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                Dong = GameObject.Instantiate(Ding, Stats.transform);
                Dong.transform.position = new Vector3(162.5f, -85f, 0f);
                Dong.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.FindObjectsOfTypeAll<Sprite>().Where(sprite => sprite.name == "Alphabet New_1").ToList()[0];
                Dong.transform.GetChild(1).transform.position = new Vector3(167f, -110f, 0f);
                
                QuestionMark = new GameObject("question mark");
                QuestionMark.transform.parent = Stats.transform;
                QuestionMark.transform.position = new Vector3(25f, -17f, 0f);
                QuestionMark.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                QuestionMark.transform.localRotation = new Quaternion(0f, 0f, 0.1305f, -0.9914f);
                QuestionMark.AddComponent<Image>().sprite = Resources.FindObjectsOfTypeAll<Sprite>().Where(sprite => sprite.name == "trinkets 1_slot_grey").ToList()[0];
                GameObject.DontDestroyOnLoad(QuestionMark);
                Stats.transform.SetAsFirstSibling();
                if (SecondSwordIcons.Count < 3) {
                    for (int i = 1; i < 4; i++) {
                        GameObject SecondSwordImg = GameObject.Instantiate(ModelSwaps.SecondSwordImage);
                        GameObject EquipButton = Resources.FindObjectsOfTypeAll<GameObject>().Where(GameObj => GameObj.name == $"Equip Button {i}").ToList()[0].transform.GetChild(0).gameObject;
                        SecondSwordImg.transform.parent = EquipButton.transform;
                        SecondSwordImg.transform.localScale = Vector3.one;
                        SecondSwordImg.transform.localPosition = Vector3.one;
                        GameObject.DontDestroyOnLoad(SecondSwordImg);
                        SecondSwordImg.SetActive(false);
                        SecondSwordIcons.Add(SecondSwordImg);
                        GameObject ThirdSwordImg = GameObject.Instantiate(ModelSwaps.ThirdSwordImage);
                        ThirdSwordImg.transform.parent = EquipButton.transform;
                        ThirdSwordImg.transform.localScale = Vector3.one;
                        ThirdSwordImg.transform.localPosition = Vector3.one;
                        GameObject.DontDestroyOnLoad(ThirdSwordImg);
                        ThirdSwordImg.SetActive(false);
                        ThirdSwordIcons.Add(ThirdSwordImg);
                        EquipButtons.Add(EquipButton);
                    }
                }
                if (Screen.width <= 1280 && Screen.height <= 800) {
                    Stats.transform.localScale = new Vector3(3.6f, 3.6f, 3.6f);
                }
            }
            Loaded = true;
        }

        public static GameObject SetupTitle(Transform parent) {
            Material ImageMaterial = Resources.FindObjectsOfTypeAll<Material>().Where(mat => mat.name == "UI Add").ToList()[0];
            List<Sprite> Alphabet = Resources.FindObjectsOfTypeAll<Sprite>().Where(sprite => sprite.name.Contains("Alphabet New")).ToList();

            GameObject Title = new GameObject("title");
            GameObject Randomizer = new GameObject("randomizer");
            Randomizer.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            Randomizer.transform.parent = parent;
            Randomizer.transform.position = new Vector3(50f, 42f, 0f);
/*            GameObject Glyph1 = new GameObject("glyph 1").AddComponent;
            Ra.AddComponent<Image>().sprite = Alphabet[38];
            Ra.transform.position = new Vector3(50f, 42f, 0f);
            Ra.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            GameObject*/

            return Title;
        }

        public static GameObject SetupText(string name, Vector3 position, Transform parent, float fontSize, TMP_FontAsset fontAsset, Material fontMaterial) { 
            GameObject Counter = new GameObject(name);
            Counter.AddComponent<TextMeshProUGUI>().text = $"";
            Counter.GetComponent<TextMeshProUGUI>().fontMaterial = fontMaterial;
            Counter.GetComponent<TextMeshProUGUI>().font = fontAsset;
            Counter.GetComponent<TextMeshProUGUI>().fontSize = fontSize;
            Counter.layer = 5;
            Counter.transform.parent = parent;
            Counter.GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 50f);
            Counter.transform.position = position;
            Counter.transform.localScale = Vector3.one;
            return Counter;
        }

        public static GameObject SetupImage(string objectName, string spriteName, Transform parent, Vector3 position, Material material) {
            GameObject Image = new GameObject(objectName);
            Sprite Sprite = Resources.FindObjectsOfTypeAll<Sprite>().Where(sprite => sprite.name == spriteName).ToList()[0];
            Image.AddComponent<Image>().sprite = Sprite;
            Image.GetComponent<Image>().material = material;
            Image.transform.parent = parent;
            Image.layer = 5;
            Image.transform.position = position;
            Image.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            GameObject.DontDestroyOnLoad(Image);
            return Image;
        }

        public static void Update() {
            try {
                int ObtainedItemCount = TunicRandomizer.Tracker.ItemsCollected.Count;
                int ObtainedItemCountInCurrentScene = TunicRandomizer.Tracker.ItemsCollected.Where(item => item.Location.SceneName == SceneLoaderPatches.SceneName).ToList().Count;
                int TotalItemCountInCurrentScene = ItemRandomizer.ItemList.Values.Where(item => item.Location.SceneName == SceneLoaderPatches.SceneName).ToList().Count;
                Title.GetComponent<TextMeshProUGUI>().text = $"Randomizer Stats";
                Pages.GetComponent<TextMeshProUGUI>().text = $"Pages:\t\t{TunicRandomizer.Tracker.ImportantItems["Pages"]}/28";
                Pages.GetComponent<TextMeshProUGUI>().color = TunicRandomizer.Tracker.ImportantItems["Pages"] == 28 ? new Color(0.917f, 0.65f, .08f) : Color.white;
                Fairies.GetComponent<TextMeshProUGUI>().text = $"Fairies:\t  {TunicRandomizer.Tracker.ImportantItems["Fairies"]}/20";
                Fairies.GetComponent<TextMeshProUGUI>().color = TunicRandomizer.Tracker.ImportantItems["Fairies"] == 20 ? new Color(0.917f, 0.65f, .08f) : Color.white;
                Treasures.GetComponent<TextMeshProUGUI>().text = $"Treasures:\t{TunicRandomizer.Tracker.ImportantItems["Golden Trophies"]}/12";
                Treasures.GetComponent<TextMeshProUGUI>().color = TunicRandomizer.Tracker.ImportantItems["Golden Trophies"] == 12 ? new Color(0.917f, 0.65f, .08f) : Color.white;
                CoinsTossed.GetComponent<TextMeshProUGUI>().text = $"Coins Tossed: {TunicRandomizer.Tracker.ImportantItems["Coins Tossed"]}/15";
                CoinsTossed.GetComponent<TextMeshProUGUI>().color = TunicRandomizer.Tracker.ImportantItems["Coins Tossed"] >= 15 ? new Color(0.917f, 0.65f, .08f) : Color.white;
                ThisArea.GetComponent<TextMeshProUGUI>().text = $"This Area:\t{ObtainedItemCountInCurrentScene}/{TotalItemCountInCurrentScene}";
                ThisArea.GetComponent<TextMeshProUGUI>().color = (ObtainedItemCountInCurrentScene == TotalItemCountInCurrentScene) ? new Color(0.917f, 0.65f, .08f) : Color.white;
                Total.GetComponent<TextMeshProUGUI>().text = $"Total:\t\t  {ObtainedItemCount}/302";
                if (GoldHexagons != null) {
                    GoldHexagons.GetComponent<TextMeshProUGUI>().text = $"{TunicRandomizer.Tracker.ImportantItems["Hexagon Gold"]}/20";
                    GoldHexagons.GetComponent<TextMeshProUGUI>().color = TunicRandomizer.Tracker.ImportantItems["Hexagon Gold"] >= 20 ? new Color(0.917f, 0.65f, .08f) : Color.white;
                }
                if (Inventory.GetItemByName("Spear").Quantity == 1) {
                    QuestionMark.SetActive(false);
                }
                Total.GetComponent<TextMeshProUGUI>().color = (ObtainedItemCount >= 302) ? new Color(0.917f, 0.65f, .08f) : Color.white;


            } catch (Exception e) { 
                
            }
        }

        public static bool InventoryDisplay_Update_PrefixPatch(InventoryDisplay __instance) {
            try {
                Initialize();
            } catch (Exception e) { }
            GameObject Equipment = GameObject.Find("_GameGUI(Clone)/HUD Canvas/Scaler/Inventory/Inventory Subscreen/Body/Section 5 Equipment/GROUP: Equipment/");
            Update();

            if (InventoryDisplay.InventoryOpen) {
                GuardCaptain.GetComponent<Image>().color = Color.white;
                Ding.GetComponent<Image>().color = Color.white;
                GardenKnight.GetComponent<Image>().color = Color.white;
                Dong.GetComponent<Image>().color = Color.white;
                SiegeEngine.GetComponent<Image>().color = Color.white;
                RedHexagon.GetComponent<Image>().color = Color.white;
                Librarian.GetComponent<Image>().color = Color.white;
                GreenHexagon.GetComponent<Image>().color = Color.white;
                BossScavenger.GetComponent<Image>().color = Color.white;
                BlueHexagon.GetComponent<Image>().color = Color.white;
                if (Inventory.GetItemByName("Hexagon Red").Quantity == 1 || StateVariable.GetStateVariableByName("Placed Hexagon 1 Red").BoolValue) {
                    __instance.hexagonImages[0].enabled = true;
                    RedHexagon.GetComponent<Image>().color = SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST" ? new Color(0.917f, 0.65f, .08f) : new Color(1f, 0f, 0f, 0.75f);
                } else {
                    __instance.hexagonImages[0].enabled = false;
                }
                if (Inventory.GetItemByName("Hexagon Green").Quantity == 1 || StateVariable.GetStateVariableByName("Placed Hexagon 2 Green").BoolValue) {
                    __instance.hexagonImages[1].enabled = true;
                    GreenHexagon.GetComponent<Image>().color = SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST" ? new Color(0.917f, 0.65f, .08f) : new Color(0f, 1f, 0f, 0.75f);
                } else {
                    __instance.hexagonImages[1].enabled = false;
                }
                if (Inventory.GetItemByName("Hexagon Blue").Quantity == 1 || StateVariable.GetStateVariableByName("Placed Hexagon 3 Blue").BoolValue) {
                    __instance.hexagonImages[2].enabled = true;
                    BlueHexagon.GetComponent<Image>().color = SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST" ? new Color(0.917f, 0.65f, .08f) : new Color(0f, 0f, 1f, 1f);
                } else {
                    __instance.hexagonImages[2].enabled = false;
                }
                if (StateVariable.GetStateVariableByName("SV_Forest Boss Room_Skuladot redux Big").BoolValue) {
                    GuardCaptain.GetComponent<Image>().color = new Color(.627f, .125f, .941f);
                }
                if (StateVariable.GetStateVariableByName("Rung Bell 1 (East)").BoolValue) {
                    Ding.GetComponent<Image>().color = new Color(1f, 0.84f, 0f);
                }
                if (StateVariable.GetStateVariableByName("SV_Archipelagos Redux TUNIC Knight is Dead").BoolValue) {
                    GardenKnight.GetComponent<Image>().color = Color.cyan;
                }
                if (StateVariable.GetStateVariableByName("Rung Bell 2 (West)").BoolValue) {
                    Dong.GetComponent<Image>().color = new Color(1f, 0.84f, 0f);
                }
                if (StateVariable.GetStateVariableByName("SV_Fortress Arena_Spidertank Is Dead").BoolValue) {
                    SiegeEngine.GetComponent<Image>().color = new Color(1f, 0f, 0f, 0.75f);
                }
                if (StateVariable.GetStateVariableByName("Librarian Dead Forever").BoolValue) {
                    Librarian.GetComponent<Image>().color = new Color(0f, 1f, 0f, 0.75f);
                }
                if (StateVariable.GetStateVariableByName("SV_ScavengerBossesDead").BoolValue) {
                    BossScavenger.GetComponent<Image>().color = new Color(0f, 0f, 1f, 1f);
                }
                
                if (Inventory.GetItemByName("Spear").Quantity == 1) {
                    Equipment.transform.GetChild(0).transform.position = new Vector3(20f, -20f, 0);
                    Equipment.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 1);
                    for (int i = 1; i < Equipment.transform.childCount; i++) {
                        Vector3 Position = Equipment.transform.GetChild(i).transform.position;
                        Equipment.transform.GetChild(i).transform.position = new Vector3(-331.975f + ((i - 1) * 52.5f), Position.y, Position.z);
                    }
                }

                if (Inventory.GetItemByName("Homeward Bone Statue").Quantity == 1 && Equipment.transform.childCount > 7) {
                    Equipment.transform.GetChild(Equipment.transform.childCount - 1).transform.position = new Vector3(-331.975f, -245.9078f, -98f);
                }

                for (int i = 0; i < Equipment.transform.childCount; i++) {
                    if (Equipment.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite.name == "Inventory items_sword") {
                        if (Equipment.transform.GetChild(i).childCount == 3) {
                        } else {
                            if (SaveFile.GetInt("randomizer sword progression level") == 3) {
                                GameObject NewSword = GameObject.Instantiate(ModelSwaps.SecondSwordImage);
                                NewSword.transform.parent = Equipment.transform.GetChild(i);
                                NewSword.transform.localScale = new Vector3(2f, 2f, 2f);
                                NewSword.transform.localPosition = Vector3.zero;
                                Equipment.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                            } else if (SaveFile.GetInt("randomizer sword progression level") == 4) {
                                GameObject NewSword = GameObject.Instantiate(ModelSwaps.ThirdSwordImage);
                                NewSword.transform.parent = Equipment.transform.GetChild(i);
                                NewSword.transform.localScale = new Vector3(2f, 2f, 2f);
                                NewSword.transform.localPosition = Vector3.zero;
                                Equipment.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                            } 
                        }
                    }
                }
                // -331.975
                // -279.475
                //TrackerOverlay.Overlay.transform.position = new Vector3(0, 50f, 0);
            }

            if (Loaded) {
                for (int i = 0; i < 3; i++) {
                    GameObject EquipButton = EquipButtons[i];
                    if (EquipButton.transform.GetChild(0).GetComponent<Image>().enabled && EquipButton.transform.GetChild(0).GetComponent<Image>().sprite != null && EquipButton.transform.GetChild(0).GetComponent<Image>().sprite.name == "Inventory items_sword") {

                        if (SaveFile.GetInt("randomizer sword progression level") == 3) {
                            EquipButton.transform.GetChild(0).gameObject.SetActive(false);
                            EquipButton.transform.GetChild(2).gameObject.SetActive(true);
                            EquipButton.transform.GetChild(3).gameObject.SetActive(false);
                        } else if (SaveFile.GetInt("randomizer sword progression level") == 4) {
                            EquipButton.transform.GetChild(0).gameObject.SetActive(false);
                            EquipButton.transform.GetChild(2).gameObject.SetActive(false);
                            EquipButton.transform.GetChild(3).gameObject.SetActive(true);
                        }
                    } else {
                        EquipButton.transform.GetChild(0).gameObject.SetActive(true);
                        EquipButton.transform.GetChild(2).gameObject.SetActive(false);
                        EquipButton.transform.GetChild(3).gameObject.SetActive(false);
                    }
                }
            }
            if (SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST") {
                HexagonQuest.SetActive(true);
            } else {
                HexagonQuest.SetActive(false);
            }

            return true;
        }
    }
}
