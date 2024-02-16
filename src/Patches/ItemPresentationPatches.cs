using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TunicRandomizer {
    public class ItemPresentationPatches {
        private static ManualLogSource Logger = TunicRandomizer.Logger;

        public static bool DathStonePresentationAlreadyCreated = false;

        public static bool ItemPresentation_presentItem_PrefixPatch(ItemPresentation __instance, Item item) {
            if (TunicRandomizer.Settings.SkipItemAnimations && item.name != "Cape") {
                return false;
            }
            return true;
        }

        public static void SetupHexagonQuestItemPresentation() {
            try {
                GameObject HexagonRoot = ModelSwaps.Items["Hexagon Blue"].transform.parent.gameObject;
                GameObject GoldHexagon = GameObject.Instantiate(HexagonRoot);
                GoldHexagon.transform.parent = HexagonRoot.transform.parent;
                GoldHexagon.transform.GetChild(0).GetComponent<MeshRenderer>().material = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().material;
                GoldHexagon.transform.GetChild(0).GetComponent<MeshRenderer>().materials[0] = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().material; GoldHexagon.SetActive(false);
                GoldHexagon.transform.localPosition = Vector3.zero;
                GameObject.DontDestroyOnLoad(GoldHexagon);

                GoldHexagon.GetComponent<ItemPresentationGraphic>().items = new List<Item>() { Inventory.GetItemByName("Hexagon Gold") }.ToArray();

                RegisterNewItemPresentation(GoldHexagon.GetComponent<ItemPresentationGraphic>());

                ModelSwaps.Items["Hexagon Gold"] = GoldHexagon.transform.GetChild(0).gameObject;
            } catch (Exception e) {
                Logger.LogError("Setup Hexagon Quest: " + e.Message);
            }
        }

        public static void SetupOldHouseKeyItemPresentation() {
            try {
                Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(item => item.gameObject.name == "key twist")
                    .ToList()[0].gameObject.GetComponent<ItemPresentationGraphic>().items = new Item[] { Inventory.GetItemByName("Key") };
                GameObject housekey = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(item => item.gameObject.name == "key twist (special)")
                    .ToList()[0].gameObject);
                housekey.name = "old house key";
                housekey.SetActive(false);
                housekey.transform.parent = Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(item => item.gameObject.name == "key twist (special)")
                    .ToList()[0].gameObject.transform.parent;
                housekey.transform.localPosition = new Vector3(-0.071f, -0.123f, 0f);
                housekey.GetComponent<ItemPresentationGraphic>().items = new Item[] { Inventory.GetItemByName("Key (House)") };
                GameObject.DontDestroyOnLoad(housekey);
 
                RegisterNewItemPresentation(housekey.GetComponent<ItemPresentationGraphic>());
            } catch (Exception e) {
                Logger.LogError("Setup Old House Key Item Presentation: " + e.Message);
            }
        }

        public static void SetupCustomSwordItemPresentations() {
            try {
                GameObject SwordPresentation = Resources.FindObjectsOfTypeAll<GameObject>().Where(Item => Item.name == "User Rotation Root").ToList()[0].transform.GetChild(9).gameObject;
                GameObject LibrarianSword = GameObject.Instantiate(SwordPresentation);
                LibrarianSword.transform.parent = SwordPresentation.transform.parent;
                LibrarianSword.GetComponent<MeshFilter>().mesh = ModelSwaps.SecondSword.GetComponent<MeshFilter>().mesh;
                LibrarianSword.GetComponent<MeshRenderer>().materials = ModelSwaps.SecondSword.GetComponent<MeshRenderer>().materials;
                LibrarianSword.transform.localScale = new Vector3(0.25f, 0.2f, 0.25f);
                LibrarianSword.transform.localRotation = new Quaternion(-0.2071f, -0.1216f, 0.3247f, -0.9148f);
                LibrarianSword.transform.localPosition = SwordPresentation.transform.localPosition;
                LibrarianSword.SetActive(false);
                GameObject.DontDestroyOnLoad(LibrarianSword);

                GameObject HeirSword = GameObject.Instantiate(SwordPresentation);
                HeirSword.transform.parent = SwordPresentation.transform.parent;
                HeirSword.GetComponent<MeshFilter>().mesh = ModelSwaps.ThirdSword.GetComponent<MeshFilter>().mesh;
                HeirSword.GetComponent<MeshRenderer>().materials = ModelSwaps.ThirdSword.GetComponent<MeshRenderer>().materials;
                HeirSword.transform.localScale = new Vector3(0.175f, 0.175f, 0.175f);
                HeirSword.transform.localRotation = new Quaternion(-0.6533f, 0.2706f, -0.2706f, 0.6533f);
                HeirSword.transform.localPosition = SwordPresentation.transform.localPosition;
                HeirSword.SetActive(false);
                GameObject.DontDestroyOnLoad(HeirSword);

                LibrarianSword.GetComponent<ItemPresentationGraphic>().items = new List<Item>() { Inventory.GetItemByName("Librarian Sword") }.ToArray();
                HeirSword.GetComponent<ItemPresentationGraphic>().items = new List<Item>() { Inventory.GetItemByName("Heir Sword") }.ToArray();

                RegisterNewItemPresentation(LibrarianSword.GetComponent<ItemPresentationGraphic>());
                RegisterNewItemPresentation(HeirSword.GetComponent<ItemPresentationGraphic>());

            } catch (Exception e) {
                Logger.LogError("Setup Custom Sword Item Presentation: " + e.Message);
            }
        }


        public static void SetupDathStoneItemPresentation() {
            try {
                GameObject KeySpecial = Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(Item => Item.gameObject.name == "key twist (special)").ToList()[0].gameObject;
                GameObject DathStone = GameObject.Instantiate(KeySpecial);

                DathStone.transform.parent = KeySpecial.transform.parent;
                DathStone.name = "dath stone";
                DathStone.layer = KeySpecial.layer;
                DathStone.GetComponent<MeshFilter>().mesh = ModelSwaps.Items["Dath Stone"].GetComponent<MeshFilter>().mesh;
                DathStone.GetComponent<MeshRenderer>().material = ModelSwaps.Items["Dath Stone"].GetComponent<MeshRenderer>().material;
                DathStone.transform.localEulerAngles = new Vector3(345.5225f, 153.9672f, 344.4959f);

                GameObject Torch = new GameObject("torch");
                Torch.AddComponent<SpriteRenderer>().sprite = ModelSwaps.FindSprite("Randomizer items_Torch redux");
                Torch.GetComponent<SpriteRenderer>().material = ModelSwaps.FindMaterial("UI Add");
                Torch.layer = KeySpecial.layer;
                Torch.transform.parent = DathStone.transform;
                Torch.transform.localPosition = new Vector3(-0.9f, 0.2f, 0f);
                Torch.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
                Torch.transform.localEulerAngles = new Vector3(0, 180, 0);

                GameObject Plus = new GameObject("plus");
                Plus.AddComponent<SpriteRenderer>().sprite = ModelSwaps.FindSprite("game gui_plus sign");
                Plus.GetComponent<SpriteRenderer>().material = ModelSwaps.FindMaterial("UI Add");
                Plus.layer = KeySpecial.layer;
                Plus.transform.parent = DathStone.transform;
                Plus.transform.localPosition = new Vector3(-0.75f, 0.2f, 0f);
                Plus.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                Plus.transform.localEulerAngles = new Vector3(0, 0, 15);

                Torch.SetActive(SaveFile.GetInt("randomizer entrance rando enabled") == 0);
                Plus.SetActive(SaveFile.GetInt("randomizer entrance rando enabled") == 0);
                DathStone.GetComponent<ItemPresentationGraphic>().items = new List<Item>() { Inventory.GetItemByName("Dath Stone") }.ToArray();

                RegisterNewItemPresentation(DathStone.GetComponent<ItemPresentationGraphic>());

                DathStone.transform.localScale = Vector3.one;
                DathStone.transform.localPosition = Vector3.zero;

                GameObject.DontDestroyOnLoad(DathStone);
                DathStone.SetActive(false);
            } catch (Exception e) {
                Logger.LogError("Setup dath stone presentation error: " + e.Message);
            }
        }

        public static void SwitchDathStonePresentation() {
            try {
                GameObject DathStone = Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(Item => Item.gameObject.name == "dath stone").ToList()[0].gameObject;

                for (int i = 0; i < DathStone.transform.childCount; i++) {
                    DathStone.transform.GetChild(i).gameObject.SetActive(SaveFile.GetInt("randomizer entrance rando enabled") == 0);
                }
            } catch (Exception e) {
                Logger.LogError("Switch dath stone presentation error: " + e.Message);
            }
        }

        public static void SetupCapePresentation() { 
            try {
                GameObject PresentationBase = Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(item => item.name == "key twist").First().gameObject;

                GameObject CapePresentation = GameObject.Instantiate(PresentationBase);
                CapePresentation.transform.parent = PresentationBase.transform.parent;
                CapePresentation.transform.localScale = Vector3.one;
                CapePresentation.transform.localPosition = new Vector3(0f, -0.7f, 0f);
                CapePresentation.transform.localEulerAngles = Vector3.zero;
                CapePresentation.name = "cape";
                CapePresentation.GetComponent<ItemPresentationGraphic>().items = new Item[] { Inventory.GetItemByName("Cape") };
                CapePresentation.SetActive(false);
                ModelSwaps.Items["Cape"] = CapePresentation;

                RegisterNewItemPresentation(CapePresentation.GetComponent<ItemPresentationGraphic>());
            } catch (Exception e) {
                Logger.LogError("Cape presentation error: " + e.Message);
            }
        }

        private static void RegisterNewItemPresentation(ItemPresentationGraphic itemPresentationGraphic) {
            List<ItemPresentationGraphic> newipgs = ItemPresentation.instance.itemGraphics.ToList();
            newipgs.Add(itemPresentationGraphic);
            ItemPresentation.instance.itemGraphics = newipgs.ToArray();
        }

    }
}
