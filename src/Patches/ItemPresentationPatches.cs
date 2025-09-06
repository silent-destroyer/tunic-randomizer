using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TunicRandomizer {
    public class ItemPresentationPatches {

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
                Material goldMaterial = ModelSwaps.FindMaterial("goldenTrophy light");
                GoldHexagon.transform.GetChild(0).GetComponent<MeshRenderer>().material = goldMaterial;
                GoldHexagon.transform.GetChild(0).GetComponent<MeshRenderer>().materials = new Material[] { goldMaterial, goldMaterial, goldMaterial };
                GoldHexagon.SetActive(false);
                GoldHexagon.transform.localPosition = Vector3.zero;
                GameObject.DontDestroyOnLoad(GoldHexagon);

                GoldHexagon.GetComponent<ItemPresentationGraphic>().items = new List<Item>() { Inventory.GetItemByName("Hexagon Gold") }.ToArray();

                RegisterNewItemPresentation(GoldHexagon.GetComponent<ItemPresentationGraphic>());

                ModelSwaps.Items["Hexagon Gold"] = GoldHexagon.transform.GetChild(0).gameObject;
            } catch (Exception e) {
                TunicLogger.LogError("Setup Hexagon Quest: " + e.Message);
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
                TunicLogger.LogError("Setup Old House Key Item Presentation: " + e.Message);
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
                TunicLogger.LogError("Setup Custom Sword Item Presentation: " + e.Message);
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
                TunicLogger.LogError("Setup dath stone presentation error: " + e.Message);
            }
        }

        public static void SwitchDathStonePresentation() {
            try {
                GameObject DathStone = Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(Item => Item.gameObject.name == "dath stone").ToList()[0].gameObject;

                for (int i = 0; i < DathStone.transform.childCount; i++) {
                    DathStone.transform.GetChild(i).gameObject.SetActive(SaveFile.GetInt("randomizer entrance rando enabled") == 0);
                }
            } catch (Exception e) {
                TunicLogger.LogError("Switch dath stone presentation error: " + e.Message);
            }
        }

        public static void SetupTorchItemPresentation() {
            try {
                GameObject PresentationBase = Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(item => item.name == "VaultKey").First().gameObject;

                GameObject Torch = GameObject.Instantiate(PresentationBase);
                Torch.transform.parent = PresentationBase.transform.parent;
                Torch.transform.localScale = Vector3.one * 0.7477f;
                Torch.transform.localPosition = new Vector3(-0.239f, -0.314f, 0f);
                Torch.transform.localEulerAngles = new Vector3(0f, 0f, 318.4006f);
                Torch.name = "torch";
                Torch.GetComponent<MeshFilter>().mesh = MeshData.CreateMesh(MeshData.Torch);
                Torch.GetComponent<MeshRenderer>().materials = new Material[] { Material.GetDefaultMaterial() };
                Torch.GetComponent<MeshRenderer>().material.mainTexture = Texture2D.whiteTexture;
                Torch.GetComponent<MeshRenderer>().material.color = new Color(0.584f, 0.4927f, 0.1892f, 1f);
                Torch.GetComponent<ItemPresentationGraphic>().items = new Item[] { Inventory.GetItemByName("Torch") };

                GameObject Fire = GameObject.Instantiate(GameObject.Find("_Environment/_Decorations/wall torch (2)/lamp fire"));
                Fire.transform.parent = Torch.transform;
                Fire.transform.localPosition = new Vector3(0, 1 ,0);
                Fire.transform.localScale = Vector3.one;
                Fire.transform.localEulerAngles = Vector3.zero;
                Fire.layer = 5;
                Fire.GetComponent<ParticleSystemRenderer>().material.color = Color.yellow;
                Fire.AddComponent<UnscaledParticleSystem>();
                Torch.SetActive(false);

                ModelSwaps.Torch = Torch;

                ModelSwaps.Items["Torch"] = Torch;
                
                RegisterNewItemPresentation(Torch.GetComponent<ItemPresentationGraphic>());
            } catch (Exception e) {
                TunicLogger.LogError("Torch presentation error: " + e.Message);
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
                TunicLogger.LogError("Cape presentation error: " + e.Message);
            }
        }

        public static void SetupLadderPresentation() {

            try {

                GameObject PresentationBase = Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(item => item.name == "cape").First().gameObject;

                GameObject LadderPresentation = GameObject.Instantiate(PresentationBase);

                LadderPresentation.transform.parent = PresentationBase.transform.parent;
                LadderPresentation.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                LadderPresentation.transform.localPosition = new Vector3(-0.1f, -0.7f, 0f);
                LadderPresentation.transform.localEulerAngles = new Vector3(20, 20, 0);
                LadderPresentation.GetComponent<MeshFilter>().mesh = ModelSwaps.LadderGraphic.GetComponent<MeshFilter>().mesh;
                LadderPresentation.GetComponent<MeshRenderer>().materials = ModelSwaps.LadderGraphic.GetComponent<MeshRenderer>().materials;
                LadderPresentation.name = "ladder";
                LadderPresentation.GetComponent<ItemPresentationGraphic>().items = ItemLookup.Items.Values.Where(item => item.Type == ItemTypes.LADDER).Select(item => Inventory.GetItemByName(item.Name)).ToArray();
                LadderPresentation.SetActive(false);

                GameObject UC1 = GameObject.Instantiate(ModelSwaps.UnderConstruction);
                UC1.transform.parent = LadderPresentation.transform;
                UC1.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                UC1.transform.localPosition = new Vector3(-3.1f, 4.3f, 0f);
                UC1.transform.localEulerAngles = new Vector3(349.4676f, 312.0545f, 0f);
                UC1.layer = LadderPresentation.layer;
                GameObject.Destroy(UC1.GetComponent<UnderConstruction>());
                GameObject.Destroy(UC1.GetComponent<InteractionTrigger>());
                UC1.SetActive(true);

                GameObject UC2 = GameObject.Instantiate(ModelSwaps.UnderConstruction);
                UC2.transform.parent = LadderPresentation.transform;
                UC2.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                UC2.transform.localPosition = new Vector3(2.6f, 1.9f, 1.5f);
                UC2.transform.localEulerAngles = Vector3.zero;
                UC2.layer = LadderPresentation.layer;
                GameObject.Destroy(UC2.GetComponent<UnderConstruction>());
                GameObject.Destroy(UC2.GetComponent<InteractionTrigger>());
                UC2.SetActive(true);

                ModelSwaps.Items["Ladder"] = GameObject.Instantiate(LadderPresentation);
                ModelSwaps.Items["Ladder"].SetActive(false);
                GameObject.DontDestroyOnLoad(ModelSwaps.Items["Ladder"]);
                for (int i = 0; i < ModelSwaps.Items["Ladder"].transform.childCount; i++) {
                    GameObject.Destroy(ModelSwaps.Items["Ladder"].transform.GetChild(i).gameObject.GetComponent<SphereCollider>());
                    GameObject.Destroy(ModelSwaps.Items["Ladder"].transform.GetChild(i).gameObject.GetComponent<BoxCollider>());
                }
                GameObject.Destroy(ModelSwaps.Items["Ladder"].GetComponent<ItemPresentationGraphic>());

                RegisterNewItemPresentation(LadderPresentation.GetComponent<ItemPresentationGraphic>());

            } catch (Exception e) {
                TunicLogger.LogError("Ladder presentation error: " + e.Message);
            }
        }

        public static void SetupGrassPresentation() {
            try {
                GameObject PresentationBase = Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(item => item.name == "VaultKey").First().gameObject;

                GameObject GrassPresentation = GameObject.Instantiate(PresentationBase);
                GrassPresentation.transform.parent = PresentationBase.transform.parent;
                GrassPresentation.transform.localScale = Vector3.one/2;
                GrassPresentation.transform.localPosition = new Vector3(0f, -0.4f, 0.1f);
                GrassPresentation.transform.localEulerAngles = new Vector3(345f, 45f, 345f);
                GrassPresentation.name = "grass";
                GrassPresentation.GetComponent<MeshFilter>().mesh = Resources.FindObjectsOfTypeAll<Mesh>().Where(mesh => mesh.name == "bush cube").First();
                GrassPresentation.GetComponent<MeshRenderer>().materials = new Material[] { ModelSwaps.FindMaterial("bush") };
                GrassPresentation.GetComponent<ItemPresentationGraphic>().items = new Item[] { Inventory.GetItemByName("Grass") };
                GrassPresentation.SetActive(false);
                ModelSwaps.Items["Grass"] = GrassPresentation;

                RegisterNewItemPresentation(GrassPresentation.GetComponent<ItemPresentationGraphic>());
            } catch (Exception e) {
                TunicLogger.LogError("Grass presentation error: " + e.Message);
            }
        }

        public static void SetupFusePresentation() {
            try {
                GameObject PresentationBase = Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(item => item.name == "VaultKey").First().gameObject;
                GameObject FusePresentation = GameObject.Instantiate(PresentationBase);
                GameObject FuseObject = GameObject.Instantiate(FuseRandomizer.FusePrefab);

                if (FusePresentation.GetComponent<MeshFilter>() != null && FusePresentation.GetComponent<MeshRenderer>() != null) {
                    GameObject.Destroy(FusePresentation.GetComponent<MeshFilter>());
                    GameObject.Destroy(FusePresentation.GetComponent<MeshRenderer>());
                }

                FusePresentation.transform.parent = PresentationBase.transform.parent;
                FusePresentation.transform.localPosition = Vector3.zero;
                FusePresentation.transform.localScale = Vector3.one;
                FuseObject.transform.parent = FusePresentation.transform;
                FuseObject.transform.localScale = Vector3.one * 0.125f;
                FuseObject.transform.localPosition = Vector3.zero;
                FuseObject.transform.localEulerAngles = new Vector3(15f, 180f, 330f);
                FuseObject.layer = 5;
                FuseObject.transform.localPosition = new Vector3(0.3f, -0.3f, 0f);
                FuseObject.transform.GetChild(0).transform.localPosition = new Vector3(0f, 5f, 0f);
                foreach (Transform transform in FuseObject.GetComponentsInChildren<Transform>(true)) {
                    transform.gameObject.layer = 5;
                }

                FuseObject.GetComponent<Fuse>().Start();
                GameObject.Destroy(FuseObject.GetComponent<Fuse>());
                GameObject.Destroy(FuseObject.GetComponent<ConduitNode>());

                FusePresentation.name = "fuse";
                FusePresentation.layer = 5;
                
                FusePresentation.GetComponent<ItemPresentationGraphic>().items = ItemLookup.Items.Values.Where(item => item.Type == ItemTypes.FUSE).Select(item => Inventory.GetItemByName(item.Name)).ToArray();

                FusePresentation.SetActive(false);
                FuseObject.SetActive(true);

                // change the appearance of the default textured box covering the bottom of the fuse
                GameObject underside = FuseObject.transform.GetChild(2).gameObject;
                underside.GetComponent<MeshRenderer>().material = ModelSwaps.FindMaterial("omnifuse");
                underside.transform.localPosition = new Vector3(-1.3f, -0.3f, 1.3f);
                underside.transform.localScale = new Vector3(0.65f, 1f, 0.65f);
                FuseObject.transform.GetChild(0).GetChild(0).GetChild(12).GetChild(8).gameObject.SetActive(false);

                RegisterNewItemPresentation(FusePresentation.GetComponent<ItemPresentationGraphic>());

                ModelSwaps.Items["Fuse"] = FuseObject;

            } catch (Exception e) {
                TunicLogger.LogError("Fuse presentation error: " + e.Message);
            }
        }

        public static void SetupBellPresentation() {
            try {
                GameObject PresentationBase = Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(item => item.name == "VaultKey").First().gameObject;
                GameObject BellPresentation = GameObject.Instantiate(PresentationBase);
                GameObject BellObject = GameObject.Instantiate(GameObject.FindObjectOfType<TuningForkBell>().gameObject);

                if (BellPresentation.GetComponent<MeshFilter>() != null && BellPresentation.GetComponent<MeshRenderer>() != null) {
                    GameObject.Destroy(BellPresentation.GetComponent<MeshFilter>());
                    GameObject.Destroy(BellPresentation.GetComponent<MeshRenderer>());
                }

                BellPresentation.transform.parent = PresentationBase.transform.parent;
                BellPresentation.transform.localPosition = Vector3.zero;
                BellPresentation.transform.localEulerAngles = new Vector3(345, 45, 345);
                BellPresentation.transform.localScale = Vector3.one;
                BellObject.transform.parent = BellPresentation.transform;
                BellObject.transform.localScale = Vector3.one * 0.175f;
                BellObject.transform.localEulerAngles = Vector3.zero;
                BellObject.layer = 5;
                BellObject.transform.localPosition = new Vector3(0f, -1, 0f);
                foreach (Transform transform in BellObject.GetComponentsInChildren<Transform>(true)) {
                    transform.gameObject.layer = 5;
                    transform.gameObject.SetActive(false);
                }
                BellObject.transform.GetChild(1).gameObject.SetActive(true);
                BellObject.transform.GetChild(2).gameObject.SetActive(false);
                GameObject innerRing = GameObject.Instantiate(BellObject.transform.GetChild(2).gameObject);
                innerRing.SetActive(true);
                innerRing.transform.parent = BellObject.transform;
                innerRing.transform.localScale = Vector3.one;
                innerRing.transform.localPosition = new Vector3(0, 4.952f ,0);
                innerRing.transform.localEulerAngles = new Vector3(0, 180, 0);
                innerRing.transform.GetChild(0).gameObject.SetActive(true);
                BellObject.transform.GetChild(3).gameObject.SetActive(false);
                GameObject outerRing = GameObject.Instantiate(BellObject.transform.GetChild(3).GetChild(0).gameObject);
                outerRing.SetActive(true);
                outerRing.transform.parent = BellObject.transform;
                outerRing.transform.localEulerAngles = Vector3.zero;
                outerRing.transform.localPosition = Vector3.zero;

                BellObject.transform.GetChild(3).gameObject.SetActive(true);
                BellObject.transform.GetChild(3).localEulerAngles = Vector3.zero;
                BellObject.transform.GetChild(3).GetChild(0).gameObject.SetActive(true);

                BellObject.GetComponent<TuningForkBell>().stateVar = StateVariable.GetStateVariableByName("true");
                BellObject.GetComponent<TuningForkBell>().Start();
                GameObject.Destroy(BellObject.GetComponent<HitReceiver>());
                GameObject.Destroy(BellObject.GetComponent<InteractionTrigger>());

                BellPresentation.name = "tuning fork bell";
                BellPresentation.layer = 5;

                BellPresentation.GetComponent<ItemPresentationGraphic>().items = ItemRandomizer.BellItems.Select(item => Inventory.GetItemByName(item)).ToArray();

                BellPresentation.SetActive(false);
                BellObject.SetActive(true);

                RegisterNewItemPresentation(BellPresentation.GetComponent<ItemPresentationGraphic>());

                ModelSwaps.Items["Bell"] = BellObject;

            } catch (Exception e) {
                TunicLogger.LogError("Bell presentation error: " + e.Message);
            }
        }

        public static void SetupFoxPrinceItemPresentations() {
            try {
                GameObject PresentationBase = Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(item => item.name == "shield").First().gameObject;
                GameObject DicePresentation = GameObject.Instantiate(PresentationBase);
                GameObject DiceObject = Resources.FindObjectsOfTypeAll<LoadingSpinner>().First().gameObject;

                DicePresentation.transform.parent = PresentationBase.transform.parent;
                DicePresentation.transform.localPosition = Vector3.zero;
                DicePresentation.transform.localEulerAngles = new Vector3(345f, 45f, 345f);
                DicePresentation.transform.localScale = Vector3.one * 0.5f;
                DicePresentation.GetComponent<MeshFilter>().mesh = DiceObject.transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh;
                DicePresentation.GetComponent<MeshRenderer>().materials = DiceObject.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().materials;

                DicePresentation.name = "soul dice";
                DicePresentation.layer = 5;

                DicePresentation.GetComponent<ItemPresentationGraphic>().items = new Item[] { Inventory.GetItemByName("Soul Dice") };
                DicePresentation.AddComponent<Rotate>().unscaled = true;
                DicePresentation.GetComponent<Rotate>().eulerAnglesPerSecond = new Vector3(10f, 30f, 10f);

                DicePresentation.SetActive(false);

                RegisterNewItemPresentation(DicePresentation.GetComponent<ItemPresentationGraphic>());

                ModelSwaps.Items["Soul Dice"] = DicePresentation;

            } catch (Exception e) {
                TunicLogger.LogError("Fox prince presentation error: " + e.Message);
            }
        }

        private static void RegisterNewItemPresentation(ItemPresentationGraphic itemPresentationGraphic) {
            List<ItemPresentationGraphic> newipgs = ItemPresentation.instance.itemGraphics.ToList();
            newipgs.Add(itemPresentationGraphic);
            ItemPresentation.instance.itemGraphics = newipgs.ToArray();
        }

    }
}
