using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using BepInEx.Logging;
using static TunicRandomizer.SaveFlags;
using UnityEngine.PlayerLoop;
using AmplifyColor;
using BepInEx;

namespace TunicRandomizer {
    public class ItemStatsHUD {

        public static bool Loaded = false;
        public static GameObject Title;
        public static GameObject Pages;
        public static GameObject Fairies;
        public static GameObject Treasures;
        public static GameObject CoinsTossed;
        public static GameObject ThisArea;
        public static GameObject Total;
        public static GameObject GoldHexagons;
        public static GameObject AbilityShuffle;

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
                Title.GetComponent<TextMeshProUGUI>().text = $"Randomizer Stats";
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
                Pages.GetComponent<TextMeshProUGUI>().text = $"Pages:\t\t0/28";
                Fairies.GetComponent<TextMeshProUGUI>().text = $"Fairies:\t  0/20";
                Treasures.GetComponent<TextMeshProUGUI>().text = $"Treasures:\t0/12";
                CoinsTossed.GetComponent<TextMeshProUGUI>().text = $"Coins Tossed: 0/15";
                ThisArea.GetComponent<TextMeshProUGUI>().text = $"This Area:\t0/0";
                Total.GetComponent<TextMeshProUGUI>().text = $"Total:\t\t  0/302";
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
                GameObject GoldHex = GameObject.Instantiate(ModelSwaps.HexagonGoldImage, HexBacking.transform);
                GoldHex.transform.localPosition = Vector3.zero;
                GoldHex.SetActive(true);
                HexBacking.transform.GetChild(0).localScale = new Vector3(0.75f, 0.75f, 0.75f);
                GoldHexagons = SetupText("gold hexagons", new Vector3(-237f, 200f, 0f), HexagonQuest.transform, 24f, FontAsset, FontMaterial);
                GoldHexagons.GetComponent<TextMeshProUGUI>().text = $"0/0";
                GoldHexagons.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 50f);
                GoldHexagons.transform.parent = HexagonQuest.transform;
                if ((float)Screen.width/Screen.height < 1.7f) {
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
                CreateAbilitySection();
                Stats.transform.SetAsFirstSibling();
                if ((float)Screen.width/Screen.height < 1.7f) {
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

        public static void CreateAbilitySection() {
            GameObject sectionPrefab = GameObject.Find("_GameGUI(Clone)/HUD Canvas/Scaler/Inventory/Inventory Subscreen/Body/Section 1 Bits/");
            if (sectionPrefab != null && AbilityShuffle == null) {

                AbilityShuffle = GameObject.Instantiate(sectionPrefab, new Vector3(-239.5023f, 216.6f, -98f), sectionPrefab.transform.rotation, sectionPrefab.transform.parent);
                AbilityShuffle.transform.GetChild(0).localScale = new Vector3(0.7f, 1f, 1f);
                AbilityShuffle.transform.GetChild(1).GetComponent<Image>().sprite = ModelSwaps.CustomItemImages["AbilityShuffle"].GetComponent<Image>().sprite;
                AbilityShuffle.transform.GetChild(1).localScale = new Vector3(1.3f, 1f, 1f);
                AbilityShuffle.transform.GetChild(1).transform.localPosition = new Vector3(-12f, -4f, 0f);

                for (int i = 2; i < AbilityShuffle.transform.childCount; i++) {
                    AbilityShuffle.transform.GetChild(i).gameObject.SetActive(false);
                }

                for (int i = 0; i < 3; i++) {
                    GameObject icon = new GameObject($"icon {i}");
                    icon.transform.parent = AbilityShuffle.transform;
                    icon.transform.localScale = Vector3.one;
                    icon.transform.localPosition = new Vector3(-560f, 42 - (i * 96), 0f);
                    icon.AddComponent<Image>().sprite = Inventory.GetItemByName("Book").icon;
                    icon.GetComponent<Image>().material = ModelSwaps.FindMaterial("UI Add");
                }

                for (int i = 0; i < 3; i++) {
                    GameObject icon = new GameObject($"hex icon {i}");
                    icon.transform.parent = AbilityShuffle.transform;
                    icon.transform.localScale = Vector3.one;
                    icon.transform.localPosition = new Vector3(-560f, 42 - (i * 96), 0f);
                    icon.AddComponent<Image>().sprite = Inventory.GetItemByName("Hexagon Gold").icon;
                    icon.GetComponent<Image>().material = ModelSwaps.FindMaterial("UI Add");
                    icon.SetActive(false);
                }

                List<string> abilities = new List<string>() { "Prayer", "Holy Cross", "Icebolt" };
                TMP_FontAsset odin = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().Where(Font => Font.name == "Latin Rounded").ToList()[0];
                Material fontMaterial = ModelSwaps.FindMaterial("Latin Rounded - Quantity Outline");
                for (int i = 0; i < 3; i++) {
                    GameObject ability = new GameObject($"ability {i}");
                    ability.transform.parent = AbilityShuffle.transform;
                    ability.transform.localScale = Vector3.one;
                    ability.transform.localPosition = new Vector3(-385f, 42 - (i * 96), 0f);
                    ability.AddComponent<TextMeshProUGUI>().font = odin;
                    ability.GetComponent<TextMeshProUGUI>().fontMaterial = fontMaterial;
                    ability.GetComponent<TextMeshProUGUI>().fontSize = 42.5f;
                    ability.GetComponent<TextMeshProUGUI>().horizontalAlignment = HorizontalAlignmentOptions.Left;
                    ability.GetComponent<TextMeshProUGUI>().text = abilities[i];
                }

                List<string> subscripts = new List<string>() { "24-25", "42-43", "52-53" };

                for (int i = 0; i < 3; i++) {
                    GameObject subscript = new GameObject($"subscript {i}");
                    subscript.transform.parent = AbilityShuffle.transform;
                    subscript.transform.localScale = Vector3.one;
                    subscript.AddComponent<TextMeshProUGUI>().font = odin;
                    subscript.GetComponent<TextMeshProUGUI>().fontMaterial = fontMaterial;
                    subscript.GetComponent<TextMeshProUGUI>().fontSize = 36;
                    subscript.GetComponent<TextMeshProUGUI>().text = subscripts[i];
                    subscript.GetComponent<TextMeshProUGUI>().autoSizeTextContainer = true;
                    subscript.GetComponent<TextMeshProUGUI>().horizontalAlignment = HorizontalAlignmentOptions.Center;
                }

                for (int i = 0; i < 3; i++) {
                    GameObject subscript = new GameObject($"hex subscript {i}");
                    subscript.transform.parent = AbilityShuffle.transform;
                    subscript.transform.localScale = Vector3.one;
                    subscript.AddComponent<TextMeshProUGUI>().font = odin;
                    subscript.GetComponent<TextMeshProUGUI>().fontMaterial = fontMaterial;
                    subscript.GetComponent<TextMeshProUGUI>().fontSize = 36;
                    subscript.GetComponent<TextMeshProUGUI>().text = "0";
                    subscript.GetComponent<TextMeshProUGUI>().autoSizeTextContainer = true;
                    subscript.GetComponent<TextMeshProUGUI>().horizontalAlignment = HorizontalAlignmentOptions.Right;
                }
                GameObject.DontDestroyOnLoad(AbilityShuffle);

                UpdateAbilitySection();
            }
        }

        public static void UpdateAbilitySection() {
            if (AbilityShuffle == null) {
                return;
            }
            if (SaveFile.GetInt(SaveFlags.AbilityShuffle) == 0) {
                AbilityShuffle.SetActive(false);
                return;
            } else {
                AbilityShuffle.SetActive(true);
            }

            bool HasPrayer = SaveFile.GetInt(PrayerUnlocked) == 1;
            bool HasHolyCross = SaveFile.GetInt(HolyCrossUnlocked) == 1;
            bool HasIcebolt = SaveFile.GetInt(IceBoltUnlocked) == 1;
            Color Full = new Color(1, 1, 1, 1);
            Color Faded = new Color(1, 1, 1, 0.5f);
            bool isHexQuest = SaveFile.GetInt(HexagonQuestEnabled) == 1;

            if (isHexQuest) {
                SortedDictionary<int, string> HexUnlocks = new SortedDictionary<int, string>() {
                    { SaveFile.GetInt(HexagonQuestPrayer), "Prayer" },
                    { SaveFile.GetInt(HexagonQuestHolyCross), "Holy Cross" },
                    { SaveFile.GetInt(HexagonQuestIcebolt), "Icebolt" },
                };

                int GoldHexes = Inventory.GetItemByName("Hexagon Gold").Quantity;
                int[] unlocks = HexUnlocks.Keys.ToArray();
                string[] abilities = HexUnlocks.Values.ToArray();

                bool hasAbility1 = GoldHexes >= unlocks[0];
                bool hasAbility2 = GoldHexes >= unlocks[1];
                bool hasAbility3 = GoldHexes >= unlocks[2];

                bool readHint1 = SaveFile.GetInt($"randomizer hex quest read {abilities[0]} hint") == 1;
                bool readHint2 = SaveFile.GetInt($"randomizer hex quest read {abilities[1]} hint") == 1;
                bool readHint3 = SaveFile.GetInt($"randomizer hex quest read {abilities[2]} hint") == 1;
                AbilityShuffle.transform.GetChild(13).GetComponent<TextMeshProUGUI>().text = hasAbility1 || readHint1 || (readHint2 && readHint3) ? abilities[0] : "?????";
                AbilityShuffle.transform.GetChild(14).GetComponent<TextMeshProUGUI>().text = hasAbility2 || readHint2 || (readHint1 && readHint3) ? abilities[1] : "?????";
                AbilityShuffle.transform.GetChild(15).GetComponent<TextMeshProUGUI>().text = hasAbility3 || readHint3 || (readHint1 && readHint2) || (hasAbility1 && hasAbility2) ? abilities[2] : "?????";

                AbilityShuffle.transform.GetChild(13).GetComponent<TextMeshProUGUI>().color = hasAbility1 ? Full : Faded;
                AbilityShuffle.transform.GetChild(14).GetComponent<TextMeshProUGUI>().color = hasAbility2 ? Full : Faded;
                AbilityShuffle.transform.GetChild(15).GetComponent<TextMeshProUGUI>().color = hasAbility3 ? Full : Faded;

                AbilityShuffle.transform.GetChild(19).GetComponent<TextMeshProUGUI>().text = $"{unlocks[0]}";
                AbilityShuffle.transform.GetChild(20).GetComponent<TextMeshProUGUI>().text = $"{unlocks[1]}";
                AbilityShuffle.transform.GetChild(21).GetComponent<TextMeshProUGUI>().text = $"{unlocks[2]}";
                for (int i = 19; i < 22; i++) {
                    AbilityShuffle.transform.GetChild(i).GetComponent<TextMeshProUGUI>().horizontalAlignment = HorizontalAlignmentOptions.Right;
                    AbilityShuffle.transform.GetChild(i).GetComponent<TextMeshProUGUI>().autoSizeTextContainer = false;
                    AbilityShuffle.transform.GetChild(i).GetComponent<TextMeshProUGUI>().autoSizeTextContainer = true;
                }

                AbilityShuffle.transform.GetChild(19).GetComponent<TextMeshProUGUI>().color = hasAbility1 ? Full : Faded;
                AbilityShuffle.transform.GetChild(20).GetComponent<TextMeshProUGUI>().color = hasAbility2 ? Full : Faded;
                AbilityShuffle.transform.GetChild(21).GetComponent<TextMeshProUGUI>().color = hasAbility3 ? Full : Faded;

                AbilityShuffle.transform.GetChild(10).GetComponent<Image>().color = hasAbility1 ? Full : Faded;
                AbilityShuffle.transform.GetChild(11).GetComponent<Image>().color = hasAbility2 ? Full : Faded;
                AbilityShuffle.transform.GetChild(12).GetComponent<Image>().color = hasAbility3 ? Full : Faded;

            } else {
                AbilityShuffle.transform.GetChild(13).GetComponent<TextMeshProUGUI>().text = "Prayer";
                AbilityShuffle.transform.GetChild(14).GetComponent<TextMeshProUGUI>().text = "Holy Cross";
                AbilityShuffle.transform.GetChild(15).GetComponent<TextMeshProUGUI>().text = "Icebolt";

                AbilityShuffle.transform.GetChild(13).GetComponent<TextMeshProUGUI>().color = HasPrayer ? Full : Faded;
                AbilityShuffle.transform.GetChild(14).GetComponent<TextMeshProUGUI>().color = HasHolyCross ? Full : Faded;
                AbilityShuffle.transform.GetChild(15).GetComponent<TextMeshProUGUI>().color = HasIcebolt ? Full : Faded;

                AbilityShuffle.transform.GetChild(16).GetComponent<TextMeshProUGUI>().text = "24-25";
                AbilityShuffle.transform.GetChild(17).GetComponent<TextMeshProUGUI>().text = "42-43";
                AbilityShuffle.transform.GetChild(18).GetComponent<TextMeshProUGUI>().text = "52-53";
                for (int i = 16; i < 19; i++) {
                    AbilityShuffle.transform.GetChild(i).GetComponent<TextMeshProUGUI>().horizontalAlignment = HorizontalAlignmentOptions.Center;
                    AbilityShuffle.transform.GetChild(i).GetComponent<TextMeshProUGUI>().autoSizeTextContainer = false;
                    AbilityShuffle.transform.GetChild(i).GetComponent<TextMeshProUGUI>().autoSizeTextContainer = true;
                }

                AbilityShuffle.transform.GetChild(16).GetComponent<TextMeshProUGUI>().color = HasPrayer ? Full : Faded;
                AbilityShuffle.transform.GetChild(17).GetComponent<TextMeshProUGUI>().color = HasHolyCross ? Full : Faded;
                AbilityShuffle.transform.GetChild(18).GetComponent<TextMeshProUGUI>().color = HasIcebolt ? Full : Faded;

                AbilityShuffle.transform.GetChild(7).GetComponent<Image>().color = HasPrayer ? Full : Faded;
                AbilityShuffle.transform.GetChild(8).GetComponent<Image>().color = HasHolyCross ? Full : Faded;
                AbilityShuffle.transform.GetChild(9).GetComponent<Image>().color = HasIcebolt ? Full : Faded;
            }

        }

        public static void Update() {
            try {
                if (Locations.VanillaLocations.Count > 0) {
                    int ObtainedItemCount = Locations.VanillaLocations.Keys.Where(loc => Locations.CheckedLocations[loc] || (SaveFlags.IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld && SaveFile.GetInt($"randomizer {loc} was collected") == 1)).ToList().Count;
                    int ObtainedItemCountInCurrentScene = Locations.VanillaLocations.Keys.Where(loc => Locations.VanillaLocations[loc].Location.SceneName == SceneLoaderPatches.SceneName && (Locations.CheckedLocations[loc] || (SaveFlags.IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld && SaveFile.GetInt($"randomizer {loc} was collected") == 1))).ToList().Count;
                    int TotalItemCountInCurrentScene = Locations.VanillaLocations.Keys.Where(loc => Locations.VanillaLocations[loc].Location.SceneName == SceneLoaderPatches.SceneName).ToList().Count;
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
                        GoldHexagons.GetComponent<TextMeshProUGUI>().text = $"{Inventory.GetItemByName("Hexagon Gold").Quantity}/{SaveFile.GetInt(HexagonQuestGoal)}";
                        GoldHexagons.GetComponent<TextMeshProUGUI>().color = Inventory.GetItemByName("Hexagon Gold").Quantity >= SaveFile.GetInt(HexagonQuestGoal) ? new Color(0.917f, 0.65f, .08f) : Color.white;
                    }
                    if (Inventory.GetItemByName("Spear").Quantity == 1) {
                        QuestionMark.SetActive(false);
                    }
                    Total.GetComponent<TextMeshProUGUI>().color = (ObtainedItemCount >= 302) ? new Color(0.917f, 0.65f, .08f) : Color.white;
                }
            } catch (Exception e) {

            }
        }

        public static bool InventoryDisplay_Update_PrefixPatch(InventoryDisplay __instance) {
            try {
                Initialize();
            } catch (Exception e) {
                TunicLogger.LogError(e + " " + e.Message);
            }
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
                    RedHexagon.GetComponent<Image>().color = SaveFile.GetInt(HexagonQuestEnabled) == 1 ? new Color(0.917f, 0.65f, .08f) : new Color(1f, 0f, 0f, 0.75f);
                } else {
                    __instance.hexagonImages[0].enabled = false;
                }
                if (Inventory.GetItemByName("Hexagon Green").Quantity == 1 || StateVariable.GetStateVariableByName("Placed Hexagon 2 Green").BoolValue) {
                    __instance.hexagonImages[1].enabled = true;
                    GreenHexagon.GetComponent<Image>().color = SaveFile.GetInt(HexagonQuestEnabled) == 1 ? new Color(0.917f, 0.65f, .08f) : new Color(0f, 1f, 0f, 0.75f);
                } else {
                    __instance.hexagonImages[1].enabled = false;
                }
                if (Inventory.GetItemByName("Hexagon Blue").Quantity == 1 || StateVariable.GetStateVariableByName("Placed Hexagon 3 Blue").BoolValue) {
                    __instance.hexagonImages[2].enabled = true;
                    BlueHexagon.GetComponent<Image>().color = SaveFile.GetInt(HexagonQuestEnabled) == 1 ? new Color(0.917f, 0.65f, .08f) : new Color(0f, 0f, 1f, 1f);
                } else {
                    __instance.hexagonImages[2].enabled = false;
                }
                if (SaveFile.GetInt(EnemyRandomizer.CustomBossFlags[0]) == 1) {
                    GuardCaptain.GetComponent<Image>().color = new Color(.627f, .125f, .941f);
                }
                if (StateVariable.GetStateVariableByName("Rung Bell 1 (East)").BoolValue) {
                    Ding.GetComponent<Image>().color = new Color(1f, 0.84f, 0f);
                }
                if (SaveFile.GetInt(EnemyRandomizer.CustomBossFlags[1]) == 1) {
                    GardenKnight.GetComponent<Image>().color = Color.cyan;
                }
                if (StateVariable.GetStateVariableByName("Rung Bell 2 (West)").BoolValue) {
                    Dong.GetComponent<Image>().color = new Color(1f, 0.84f, 0f);
                }
                if (SaveFile.GetInt(EnemyRandomizer.CustomBossFlags[2]) == 1) {
                    SiegeEngine.GetComponent<Image>().color = new Color(1f, 0f, 0f, 0.75f);
                }
                if (SaveFile.GetInt(EnemyRandomizer.CustomBossFlags[3]) == 1) {
                    Librarian.GetComponent<Image>().color = new Color(0f, 1f, 0f, 0.75f);
                }
                if (SaveFile.GetInt(EnemyRandomizer.CustomBossFlags[4]) == 1) {
                    BossScavenger.GetComponent<Image>().color = new Color(0f, 0f, 1f, 1f);
                }

                if (Inventory.GetItemByName("Spear").Quantity == 1) {
                    Equipment.transform.GetChild(0).transform.position = new Vector3(20f, -20f, 0);
                    Equipment.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 1);
                    for (int i = 1; i < Equipment.transform.childCount; i++) {
                        Vector3 Position = Equipment.transform.GetChild(i).transform.position;
                        Equipment.transform.GetChild(i).transform.position = new Vector3(-331.975f + ((i - 1) * 52.5f), Position.y, Position.z);
                    }
                    if (Equipment.transform.childCount > 8) {
                        for (int i = 8; i < Equipment.transform.childCount; i++) {
                            Equipment.transform.GetChild(i).transform.position = new Vector3(-331.975f + ((i - 8) * 52.5f), -245.9078f, -98f);
                        }
                    }
                } else {
                    if (Equipment.transform.childCount > 7) {
                        for (int i = 7; i < Equipment.transform.childCount; i++) {
                            Equipment.transform.GetChild(i).transform.position = new Vector3(-331.975f + ((i - 7) * 52.5f), -245.9078f, -98f);
                        }
                    }
                }

                AbilityShuffle.transform.localPosition = new Vector3(465f, 0f, 0f);
                AbilityShuffle.transform.GetChild(0).localPosition = new Vector3(146.9f, -72.5f, 0f);
                bool hexQuest = SaveFile.GetInt(HexagonQuestEnabled) == 1;
                for (int i = 16; i < 19; i++) {
                    AbilityShuffle.transform.GetChild(i).localPosition = new Vector3(52, -197 - ((i - 16) * 96), 0f);
                    AbilityShuffle.transform.GetChild(i).gameObject.SetActive(!hexQuest);
                    AbilityShuffle.transform.GetChild(i - 9).gameObject.SetActive(!hexQuest);
                }
                for (int i = 19; i < AbilityShuffle.transform.childCount; i++) {
                    AbilityShuffle.transform.GetChild(i).localPosition = new Vector3(77, -197 - ((i - 19) * 96), 0f);
                    AbilityShuffle.transform.GetChild(i).gameObject.SetActive(hexQuest);
                    AbilityShuffle.transform.GetChild(i - 9).gameObject.SetActive(hexQuest);
                }
            }

            HexagonQuest.SetActive(SaveFile.GetInt(HexagonQuestEnabled) == 1);

            return true;
        }
    }
}
