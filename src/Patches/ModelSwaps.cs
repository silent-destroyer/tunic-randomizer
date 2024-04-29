using Archipelago.MultiClient.Net.Enums;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {
    public class ModelSwaps {
        public static ManualLogSource Logger = TunicRandomizer.Logger;

        public static Dictionary<string, Sprite> Cards = new Dictionary<string, Sprite>();
        public static Dictionary<string, GameObject> Items = new Dictionary<string, GameObject>();
        public static Dictionary<string, GameObject> Chests = new Dictionary<string, GameObject>();
        public static GameObject SecondSword = null;
        public static GameObject ThirdSword = null;
        public static GameObject PagePickup = null;
        public static bool SwappedThisSceneAlready = false;
        public static GameObject HexagonGoldImage;
        public static GameObject TuncTitleImage;
        public static GameObject TuncImage;

        public static GameObject FairyAnimation;
        public static GameObject IceFlask;

        public static GameObject HeroRelicMaterial;

        public static Dictionary<string, GameObject> CustomItemImages = new Dictionary<string, GameObject>();

        public static GameObject GlowEffect;
        public static GameObject StarburstEffect;
        public static GameObject BlueFire;

        public static GameObject LadderGraphic;
        public static GameObject UnderConstruction;
        public static GameObject Signpost;

        public static GameObject FishingRod;

        public static List<string> ShopItemIDs = new List<string>() {
            "Potion (First) [Shop]",
            "Potion (West Garden) [Shop]",
            "Trinket Coin 1 (day) [Shop]",
            "Trinket Coin 2 (night) [Shop]"
        };
        public static List<string> ShopGameObjectIDs = new List<string>() {
            "Shop/Item Holder/Potion (First)/rotation/potion",
            "Shop/Item Holder/Potion (West Garden)/rotation/potion",
            "Shop/Item Holder/Trinket Coin 1 (day)/rotation/Trinket Coin",
            "Shop/Item Holder/Trinket Coin 2 (night)/rotation/Trinket Coin"
        };

        public static void InitializeItems() {
            GameObject ItemRoot = Resources.FindObjectsOfTypeAll<GameObject>().Where(Item => Item.name == "User Rotation Root").ToList()[0];
            Items["Firecracker"] = ItemRoot.transform.GetChild(3).GetChild(0).gameObject;
            Items["Firebomb"] = ItemRoot.transform.GetChild(21).GetChild(0).gameObject;
            Items["Ice Bomb"] = ItemRoot.transform.GetChild(2).GetChild(0).gameObject;

            Items["Flask Shard"] = ItemRoot.transform.GetChild(22).gameObject;
            Items["Flask Container"] = ItemRoot.transform.GetChild(10).gameObject;

            Items["Berry_HP"] = ItemRoot.transform.GetChild(1).gameObject;
            Items["Berry_MP"] = ItemRoot.transform.GetChild(25).gameObject;
            Items["Pepper"] = ItemRoot.transform.GetChild(5).gameObject;
            Items["Ivy"] = ItemRoot.transform.GetChild(45).gameObject;
            Items["Bait"] = ItemRoot.transform.GetChild(0).GetChild(0).gameObject;

            Items["Piggybank L1"] = ItemRoot.transform.GetChild(6).gameObject;

            Items["Trinket Coin"] = ItemRoot.transform.GetChild(29).gameObject;
            Items["Trinket Card"] = ItemRoot.transform.GetChild(47).gameObject;
            Items["Trinket Slot"] = ItemRoot.transform.GetChild(30).gameObject;

            Items["Sword"] = GameObject.Instantiate(ItemRoot.transform.GetChild(9).gameObject);
            Items["Sword Progression"] = GameObject.Instantiate(ItemRoot.transform.GetChild(9).gameObject);
            Items["Sword"].SetActive(false);
            Items["Sword Progression"].SetActive(false);
            GameObject.DontDestroyOnLoad(Items["Sword"]);
            GameObject.DontDestroyOnLoad(Items["Sword Progression"]);
            Items["Stick"] = ItemRoot.transform.GetChild(8).gameObject;
            Items["Shield"] = ItemRoot.transform.GetChild(7).gameObject;
            Items["Stundagger"] = ItemRoot.transform.GetChild(16).gameObject;
            Items["Techbow"] = ItemRoot.transform.GetChild(11).gameObject;
            Items["Wand"] = ItemRoot.transform.GetChild(41).gameObject;
            Items["Shotgun"] = ItemRoot.transform.GetChild(46).gameObject;
            Items["SlowmoItem"] = ItemRoot.transform.GetChild(26).gameObject;
            Items["Lantern"] = ItemRoot.transform.GetChild(17).gameObject;
            Items["Hyperdash"] = ItemRoot.transform.GetChild(40).gameObject;

            Items["Upgrade Offering - Attack - Tooth"] = ItemRoot.transform.GetChild(15).gameObject;
            Items["Upgrade Offering - DamageResist - Effigy"] = ItemRoot.transform.GetChild(13).gameObject;
            Items["Upgrade Offering - Health HP - Flower"] = ItemRoot.transform.GetChild(18).gameObject;
            Items["Upgrade Offering - Magic MP - Mushroom"] = ItemRoot.transform.GetChild(19).gameObject;
            Items["Upgrade Offering - PotionEfficiency Swig - Ash"] = ItemRoot.transform.GetChild(12).gameObject;
            Items["Upgrade Offering - Stamina SP - Feather"] = ItemRoot.transform.GetChild(14).gameObject;

            Items["GoldenTrophy_1"] = ItemRoot.transform.GetChild(31).gameObject;
            Items["GoldenTrophy_2"] = ItemRoot.transform.GetChild(32).gameObject;
            Items["GoldenTrophy_3"] = ItemRoot.transform.GetChild(33).gameObject;
            Items["GoldenTrophy_4"] = ItemRoot.transform.GetChild(34).gameObject;
            Items["GoldenTrophy_5"] = ItemRoot.transform.GetChild(35).gameObject;
            Items["GoldenTrophy_6"] = ItemRoot.transform.GetChild(36).gameObject;
            Items["GoldenTrophy_7"] = ItemRoot.transform.GetChild(37).gameObject;
            Items["GoldenTrophy_8"] = ItemRoot.transform.GetChild(38).gameObject;
            Items["GoldenTrophy_9"] = ItemRoot.transform.GetChild(39).gameObject;
            Items["GoldenTrophy_10"] = ItemRoot.transform.GetChild(42).gameObject;
            Items["GoldenTrophy_11"] = ItemRoot.transform.GetChild(43).gameObject;
            Items["GoldenTrophy_12"] = ItemRoot.transform.GetChild(44).gameObject;
            SecretMayor.MrMayor = Items["GoldenTrophy_1"];
            Items["Key"] = ItemRoot.transform.GetChild(4).gameObject;
            Items["Vault Key (Red)"] = ItemRoot.transform.GetChild(23).gameObject;

            Items["Hexagon Red"] = ItemRoot.transform.GetChild(24).GetChild(0).gameObject;
            Items["Hexagon Green"] = ItemRoot.transform.GetChild(27).GetChild(0).gameObject;
            Items["Hexagon Blue"] = ItemRoot.transform.GetChild(28).GetChild(0).gameObject;

            Items["money small"] = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<ItemPickup>().Where(ItemPickup => ItemPickup.name == "Coin Pickup 1(Clone)").ToList()[0].gameObject.transform.GetChild(0).gameObject);
            Items["money medium"] = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<ItemPickup>().Where(ItemPickup => ItemPickup.name == "Coin Pickup 2(Clone)").ToList()[0].gameObject.transform.GetChild(0).gameObject);
            Items["money large"] = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<ItemPickup>().Where(ItemPickup => ItemPickup.name == "Coin Pickup 3(Clone)").ToList()[0].gameObject.transform.GetChild(0).gameObject);
            Items["money large"].transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            GameObject.Destroy(Items["money small"].GetComponent<Rotate>());
            GameObject.Destroy(Items["money medium"].GetComponent<Rotate>());
            GameObject.Destroy(Items["money large"].GetComponent<Rotate>());
            GameObject.DontDestroyOnLoad(Items["money small"]);
            GameObject.DontDestroyOnLoad(Items["money medium"]);
            GameObject.DontDestroyOnLoad(Items["money large"]);
            Items["money small"].SetActive(false);
            Items["money medium"].SetActive(false);
            Items["money large"].SetActive(false);

            PaletteEditor.ToonFox = new GameObject("toon fox");
            GameObject.DontDestroyOnLoad(PaletteEditor.ToonFox);
            PaletteEditor.RegularFox = new GameObject("regular fox");
            PaletteEditor.RegularFox.AddComponent<MeshRenderer>().material = FindMaterial("fox redux");
            GameObject.DontDestroyOnLoad(PaletteEditor.RegularFox);
            PaletteEditor.GhostFox = new GameObject("ghost fox");
            PaletteEditor.GhostFox.AddComponent<MeshRenderer>().material = FindMaterial("ghost material");
            GameObject.DontDestroyOnLoad(PaletteEditor.GhostFox);

            foreach (TrinketItem TrinketItem in Resources.FindObjectsOfTypeAll<TrinketItem>()) {
                Cards[TrinketItem.name] = TrinketItem.CardGraphic;
            }

            LoadTextures();

            Items["Dath Stone"] = new GameObject();
            Items["Dath Stone"].AddComponent<MeshFilter>().mesh = MeshData.CreateMesh(MeshData.DathStone);
            Items["Dath Stone"].AddComponent<MeshRenderer>().material = FindMaterial("grass tuft");
            Items["Dath Stone"].GetComponent<MeshRenderer>().material.mainTexture = CustomItemImages["Dath Stone Texture"].GetComponent<Image>().sprite.texture;
            Items["Dath Stone"].GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1);
            Items["Dath Stone"].SetActive(false);
            Items["Dath Stone"].name = "dath stone";
            GameObject.DontDestroyOnLoad(Items["Dath Stone"]);

            Signpost = new GameObject("signpost");
            Signpost.AddComponent<MeshFilter>().mesh = Resources.FindObjectsOfTypeAll<Mesh>().Where(mesh => mesh.name == "signpost pointing right").First();
            Signpost.AddComponent<MeshRenderer>().material = FindMaterial("signpost");
            Signpost.SetActive(false);
            GameObject.DontDestroyOnLoad(Signpost);

            ItemPresentationPatches.SetupOldHouseKeyItemPresentation();
            Items["Key (House)"] = ItemRoot.transform.GetChild(48).gameObject;
            ItemPresentationPatches.SetupDathStoneItemPresentation();
            ItemPresentationPatches.SetupHexagonQuestItemPresentation();
            ItemPresentationPatches.SetupCapePresentation();
            InitializeExtras();

            // make it so you can pick up money from further away
            List<ItemPickup> coins = Resources.FindObjectsOfTypeAll<ItemPickup>().Where(coin => coin.gameObject.scene.name == "DontDestroyOnLoad").ToList();
            foreach (ItemPickup coin in coins) {
                coin.minimumAttractDistance = 6.5f;
            }
        }

        public static void CreateOtherWorldItemBlocks() {

            Items[$"Other World {ItemFlags.Advancement}"] = new GameObject("other world");
            Items[$"Other World {ItemFlags.Advancement}"].AddComponent<SpriteRenderer>().sprite = FindSprite("trinkets 1_slot_grey");

            GameObject Front = GameObject.Instantiate(Items[$"Other World {ItemFlags.Advancement}"]);
            Front.transform.localPosition = new Vector3(0f, 0f, -1.9f); 
            GameObject Top = GameObject.Instantiate(Items[$"Other World {ItemFlags.Advancement}"]);
            Top.transform.localPosition = new Vector3(0f, 1.9f, 0f);
            Top.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
            GameObject Bottom = GameObject.Instantiate(Items[$"Other World {ItemFlags.Advancement}"]);
            Bottom.transform.localPosition = new Vector3(0, -1.9f, 0f);
            Bottom.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
            GameObject Left = GameObject.Instantiate(Items[$"Other World {ItemFlags.Advancement}"]);
            Left.transform.localPosition = new Vector3(-1.9f, 0f, 0f);
            Left.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
            GameObject Right = GameObject.Instantiate(Items[$"Other World {ItemFlags.Advancement}"]);
            Right.transform.localPosition = new Vector3(1.9f, 0f, 0f);
            Right.transform.localEulerAngles = new Vector3(0f, 270f, 0f);
            GameObject Back = GameObject.Instantiate(Items[$"Other World {ItemFlags.Advancement}"]);
            Back.transform.localPosition = new Vector3(0f, 0f, 1.9f);
            Back.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            GameObject Cube = new GameObject("cube");
            Cube.AddComponent<MeshFilter>().mesh = GameObject.Find("Group/Jelly Cube/Jelly Cube").GetComponent<SkinnedMeshRenderer>().sharedMesh;
            Cube.AddComponent<MeshRenderer>().materials = Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
            Cube.transform.localScale = new Vector3(1.89f, 1.89f, 1.89f);
            Cube.transform.localPosition = new Vector3(0f, -1.9f, 0f);
            Top.transform.parent = Items[$"Other World {ItemFlags.Advancement}"].transform;
            Bottom.transform.parent = Items[$"Other World {ItemFlags.Advancement}"].transform;
            Left.transform.parent = Items[$"Other World {ItemFlags.Advancement}"].transform;
            Right.transform.parent = Items[$"Other World {ItemFlags.Advancement}"].transform;
            Back.transform.parent = Items[$"Other World {ItemFlags.Advancement}"].transform;
            Front.transform.parent = Items[$"Other World {ItemFlags.Advancement}"].transform;
            Cube.transform.parent = Items[$"Other World {ItemFlags.Advancement}"].transform;
            Items[$"Other World {ItemFlags.Advancement}"].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Items[$"Other World {ItemFlags.Advancement}"].SetActive(false);

            Items[$"Other World {ItemFlags.None}"] = GameObject.Instantiate(Items[$"Other World {ItemFlags.Advancement}"]);
            Items[$"Other World {ItemFlags.None}"].transform.GetChild(6).gameObject.GetComponent<MeshRenderer>().materials = GameObject.Find("Group/Jelly Cube/Jelly Cube").GetComponent<CreatureMaterialManager>().originalMaterials;
            Items[$"Other World {ItemFlags.None}"].transform.GetChild(6).gameObject.GetComponent<MeshRenderer>().material.color = new UnityEngine.Color(0, 0.5f, 0, 1);

            Items[$"Other World {ItemFlags.NeverExclude}"] = GameObject.Instantiate(Items[$"Other World {ItemFlags.Advancement}"]);
            Items[$"Other World {ItemFlags.NeverExclude}"].transform.GetChild(6).gameObject.GetComponent<MeshRenderer>().materials = GameObject.Find("Group/Jelly Cube/Jelly Cube").GetComponent<CreatureMaterialManager>().originalMaterials;
            Items[$"Other World {ItemFlags.NeverExclude}"].transform.GetChild(6).gameObject.GetComponent<MeshRenderer>().material.color = new UnityEngine.Color(0, 0.15f, 1, 1);

            Items[$"Other World {ItemFlags.Trap}"] = GameObject.Instantiate(Items[$"Other World {ItemFlags.Advancement}"]);

            GameObject.DontDestroyOnLoad(Items[$"Other World {ItemFlags.None}"]);
            GameObject.DontDestroyOnLoad(Items[$"Other World {ItemFlags.Advancement}"]);
            GameObject.DontDestroyOnLoad(Items[$"Other World {ItemFlags.NeverExclude}"]);
            GameObject.DontDestroyOnLoad(Items[$"Other World {ItemFlags.Trap}"]);

            GameObject.Destroy(Items[$"Other World {ItemFlags.None}"].GetComponent<SpriteRenderer>());
            GameObject.Destroy(Items[$"Other World {ItemFlags.Advancement}"].GetComponent<SpriteRenderer>());
            GameObject.Destroy(Items[$"Other World {ItemFlags.NeverExclude}"].GetComponent<SpriteRenderer>());
            GameObject.Destroy(Items[$"Other World {ItemFlags.Trap}"].GetComponent<SpriteRenderer>());
        }

        public static void InitializeExtras() {
            InitializeChestType("Fairy");
            InitializeChestType("GoldenTrophy");
            InitializeChestType("Normal");

            List<PagePickup> PagePickups = Resources.FindObjectsOfTypeAll<PagePickup>().ToList();
            if (PagePickups.Count > 0) {
                PagePickup = GameObject.Instantiate(PagePickups[0].gameObject.transform.GetChild(2).gameObject);
                PagePickup.SetActive(false);
                GameObject.DontDestroyOnLoad(PagePickup);
            }
        }

        public static void SetupGlowEffect() {
            GlowEffect = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Night Glow").ToList()[0]);
            GameObject.Destroy(GlowEffect.GetComponent<StatefulActive>());
            GlowEffect.SetActive(false);
            GameObject.DontDestroyOnLoad(GlowEffect);
        }

        public static void SetupStarburstEffect() {
            StarburstEffect = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<ParticleSystem>().Where(ps => ps.name == "PS: starburst" && ps.transform.parent.name == "zzz_trash").FirstOrDefault().gameObject);
            StarburstEffect.SetActive(false);
            GameObject.DontDestroyOnLoad(StarburstEffect);
        }

        public static void InstantiateFishingRod() {
            FishingRod = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "fishingpole").ToList()[0]);
            GameObject.Destroy(FishingRod.GetComponent<StatefulActive>());
            FishingRod.SetActive(false);
            GameObject.DontDestroyOnLoad(FishingRod);
        }

        public static void InitializeSecondSword() {
            GameObject Temp = Resources.FindObjectsOfTypeAll<GameObject>().Where(Obj => Obj.name == "Sword").ToList()[0];
            SecondSword = new GameObject("second sword");
            SecondSword.AddComponent<MeshFilter>().mesh = Temp.GetComponent<MeshFilter>().mesh;
            SecondSword.AddComponent<MeshRenderer>().materials = Temp.GetComponent<MeshRenderer>().materials;
            SecondSword.SetActive(false);
            GameObject.DontDestroyOnLoad(SecondSword);
        }

        public static void InitializeThirdSword() {
            GameObject Temp = GameObject.Find("_NON-BOSSFIGHT ROOT/elderfox_sword graphic");
            ThirdSword = new GameObject("third sword");
            ThirdSword.AddComponent<MeshFilter>().mesh = Temp.GetComponent<MeshFilter>().mesh;
            ThirdSword.AddComponent<MeshRenderer>().materials = Temp.GetComponent<MeshRenderer>().materials;
            ThirdSword.SetActive(false);
            GameObject.DontDestroyOnLoad(ThirdSword);
        }

        public static void InitializeHeroRelics() {
            GameObject ItemRoot = Resources.FindObjectsOfTypeAll<GameObject>().Where(Item => Item.name == "User Rotation Root").ToList()[0];

            Items["Relic - Hero Sword"] = SetupRelicItemPresentation(ItemRoot.transform, 15, "Relic - Hero Sword");
            Items["Relic - Hero Crown"] = SetupRelicItemPresentation(ItemRoot.transform, 13, "Relic - Hero Crown");
            Items["Relic - Hero Pendant HP"] = SetupRelicItemPresentation(ItemRoot.transform, 18, "Relic - Hero Pendant HP");
            Items["Relic - Hero Pendant MP"] = SetupRelicItemPresentation(ItemRoot.transform, 19, "Relic - Hero Pendant MP");
            Items["Relic - Hero Water"] = SetupRelicItemPresentation(ItemRoot.transform, 12, "Relic - Hero Water");
            Items["Relic - Hero Pendant SP"] = SetupRelicItemPresentation(ItemRoot.transform, 14, "Relic - Hero Pendant SP");

            List<ItemPresentationGraphic> newipgs = new List<ItemPresentationGraphic>() { };
            foreach (ItemPresentationGraphic ipg in ItemPresentation.instance.itemGraphics) {
                newipgs.Add(ipg);
            }
            newipgs.Add(Items["Relic - Hero Sword"].GetComponent<ItemPresentationGraphic>());
            newipgs.Add(Items["Relic - Hero Crown"].GetComponent<ItemPresentationGraphic>());
            newipgs.Add(Items["Relic - Hero Pendant HP"].GetComponent<ItemPresentationGraphic>());
            newipgs.Add(Items["Relic - Hero Pendant MP"].GetComponent<ItemPresentationGraphic>());
            newipgs.Add(Items["Relic - Hero Water"].GetComponent<ItemPresentationGraphic>());
            newipgs.Add(Items["Relic - Hero Pendant SP"].GetComponent<ItemPresentationGraphic>());
            ItemPresentation.instance.itemGraphics = newipgs.ToArray();
        }

        private static GameObject SetupRelicItemPresentation(Transform parent, int index, string itemname) {
            GameObject relic = GameObject.Instantiate(parent.GetChild(index).gameObject);
            relic.transform.parent = parent;
            relic.SetActive(false);
            relic.transform.localPosition = Vector3.zero;
            relic.GetComponent<ItemPresentationGraphic>().items = new Item[] { Inventory.GetItemByName(itemname) };

            Material RelicMaterial = FindMaterial("ghost material_offerings");

            if (relic.GetComponent<MeshRenderer>() != null) {
                relic.GetComponent<MeshRenderer>().material = RelicMaterial;
            }

            for (int i = 0; i < relic.transform.childCount; i++) {
                relic.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = RelicMaterial;
            }
            GameObject.DontDestroyOnLoad(relic);
            return relic;
        }

        public static void InitializeChestType(string ChestType) {
            if (!Chests.ContainsKey(ChestType)) {
                List<Chest> SceneChests = new List<Chest>();
                if (ChestType == "Normal") {
                    SceneChests = Resources.FindObjectsOfTypeAll<Chest>().Where(Chest => Chest.gameObject.name != "Chest: Fairy" && !Chest.gameObject.name.Contains("Chest: Golden Trophy")).ToList();
                } else {
                    SceneChests = Resources.FindObjectsOfTypeAll<Chest>().Where(Chest => Chest.gameObject.name.Contains($"Chest: {ChestType}")).ToList();
                }
                if (SceneChests.Count > 0) {
                    Chests[ChestType] = GameObject.Instantiate(SceneChests[0].gameObject.transform.GetChild(1).gameObject);
                    Il2CppReferenceArray<Material> ChestMaterials = Chests[ChestType].GetComponent<SkinnedMeshRenderer>().materials;
                    GameObject.Destroy(Chests[ChestType].GetComponent<SkinnedMeshRenderer>());
                    Chests[ChestType].AddComponent<MeshRenderer>().materials = ChestMaterials;
                    Chests[ChestType].AddComponent<MeshFilter>().mesh = Resources.FindObjectsOfTypeAll<Mesh>().Where(Mesh => Mesh.name == "chest").ToList()[0];
                    Chests[ChestType].AddComponent<FMODUnity.StudioEventEmitter>().EventReference = SceneChests[0].gameObject.GetComponent<FMODUnity.StudioEventEmitter>().EventReference;
                    Chests[ChestType].SetActive(false);
                    GameObject.DontDestroyOnLoad(Chests[ChestType]);
                    if (ChestType == "Fairy") {
                        FairyAnimation = GameObject.Instantiate(SceneChests[0].transform.parent.GetChild(1).gameObject);
                        FairyAnimation.SetActive(false);
                        GameObject.DontDestroyOnLoad(FairyAnimation);
                    }
                }
            }
        }

        public static void SwapItemsInScene() {

            if (IceFlask == null) {
                if (Resources.FindObjectsOfTypeAll<BombFlask>().Where(bomb => bomb.name == "Ice flask").Count() > 0) {
                    IceFlask = Resources.FindObjectsOfTypeAll<BombFlask>().Where(bomb => bomb.name == "Ice flask").ToList()[0].transform.GetChild(0).gameObject;
                    Items["Ice Bomb"] = IceFlask;
                }
            }

            if (IsArchipelago()) {
                CheckCollectedItemFlags();
            }

            if (SceneLoaderPatches.SceneName == "Shop") {
                SetupShopItems();
            } else {
                if (TunicRandomizer.Settings.ShowItemsEnabled) {
                    foreach (ItemPickup ItemPickup in Resources.FindObjectsOfTypeAll<ItemPickup>()) {
                        if (ItemPickup != null && ItemPickup.itemToGive != null) {
                            string checkId = $"{ItemPickup.itemToGive.name} [{SceneLoaderPatches.SceneName}]";
                            if(ItemLookup.ItemList.ContainsKey(checkId) || Locations.RandomizedLocations.ContainsKey(checkId)) {
                                if (ItemPickup.itemToGive.name == "Hexagon Red") {
                                    SetupRedHexagonPlinth();
                                } else if (ItemPickup.itemToGive.name == "Hexagon Blue") {
                                    SetupBlueHexagonPlinth();
                                } else if (ItemPickup.itemToGive.name != "MoneySmall") {
                                    SetupItemPickup(ItemPickup);
                                }
                            }
                        }
                    }

                    foreach (PagePickup PagePickup in Resources.FindObjectsOfTypeAll<PagePickup>()) {
                        string ItemId = $"{PagePickup.pageName} [{SceneLoaderPatches.SceneName}]";
                        if(ItemLookup.ItemList.ContainsKey(ItemId) || Locations.RandomizedLocations.ContainsKey(ItemId)) {
                            SetupPagePickup(PagePickup);
                        }
                    }

                    foreach (HeroRelicPickup HeroRelicPickup in Resources.FindObjectsOfTypeAll<HeroRelicPickup>()) {
                        SetupHeroRelicPickup(HeroRelicPickup);
                    }
                }
                if (TunicRandomizer.Settings.ChestsMatchContentsEnabled) {
                    foreach (Chest Chest in Resources.FindObjectsOfTypeAll<Chest>()) {
                        ApplyChestTexture(Chest);
                    }
                }

                if (SceneLoaderPatches.SceneName == "Fortress Arena") {
                    SwapSiegeEngineCrown();
                }
            }
            ItemLookup.Items["Fool Trap"].QuantityToGive = 1;
            SwappedThisSceneAlready = true;
        }

        public static void ApplyChestTexture(Chest Chest) {
            if (Chest != null) {

                string ItemId = Chest.chestID == 0 ? $"{SceneLoaderPatches.SceneName}-{Chest.transform.position.ToString()} [{SceneLoaderPatches.SceneName}]" : $"{Chest.chestID} [{SceneLoaderPatches.SceneName}]";
                string ItemName = "Stick";
                if (IsArchipelago() && ItemLookup.ItemList.ContainsKey(ItemId)) {
                    ArchipelagoItem APItem = ItemLookup.ItemList[ItemId];
                    ItemName = APItem.ItemName;
                    if (!Archipelago.instance.IsTunicPlayer(APItem.Player) || !ItemLookup.Items.ContainsKey(APItem.ItemName)) {
                        Chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = Chests["Normal"].GetComponent<MeshRenderer>().materials;
                        Chest.GetComponent<FMODUnity.StudioEventEmitter>().EventReference = Chests["Normal"].GetComponent<FMODUnity.StudioEventEmitter>().EventReference;
                        Chest.GetComponent<FMODUnity.StudioEventEmitter>().Lookup();
                        ApplyAPChestTexture(Chest, APItem);
                        return;
                    }
                } else if (IsSinglePlayer() && Locations.RandomizedLocations.ContainsKey(ItemId)) {
                    Check check = Locations.RandomizedLocations[ItemId];
                    ItemName = ItemLookup.GetItemDataFromCheck(check).Name;
                }

                ItemData Item = ItemLookup.Items[ItemName];
                if (Item.Type == ItemTypes.FAIRY) {
                    Chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = Chests["Fairy"].GetComponent<MeshRenderer>().materials;
                    Chest.GetComponent<FMODUnity.StudioEventEmitter>().EventReference = Chests["Fairy"].GetComponent<FMODUnity.StudioEventEmitter>().EventReference;
                } else if (Item.Type == ItemTypes.GOLDENTROPHY) {
                    Chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = Chests["GoldenTrophy"].GetComponent<MeshRenderer>().materials;
                    Chest.GetComponent<FMODUnity.StudioEventEmitter>().EventReference = Chests["GoldenTrophy"].GetComponent<FMODUnity.StudioEventEmitter>().EventReference;
                } else if (Item.ItemNameForInventory == "Hyperdash") {
                    Chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = Chests["Hyperdash"].GetComponent<MeshRenderer>().materials;
                    Chest.GetComponent<FMODUnity.StudioEventEmitter>().EventReference = Chests["Hyperdash"].GetComponent<FMODUnity.StudioEventEmitter>().EventReference;
                } else if (Item.ItemNameForInventory.Contains("Hexagon") && Item.Type != ItemTypes.HEXAGONQUEST) {
                    Material[] Mats = new Material[] { Items[Item.ItemNameForInventory].GetComponent<MeshRenderer>().material, Items[Item.ItemNameForInventory].GetComponent<MeshRenderer>().material };
                    Chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = Mats;
                    Chest.GetComponent<FMODUnity.StudioEventEmitter>().EventReference = Chests["Normal"].GetComponent<FMODUnity.StudioEventEmitter>().EventReference;
                } else {
                    Chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = Chests["Normal"].GetComponent<MeshRenderer>().materials;
                    Chest.GetComponent<FMODUnity.StudioEventEmitter>().EventReference = Chests["Normal"].GetComponent<FMODUnity.StudioEventEmitter>().EventReference;
                }
                
            }
        }

        public static void ApplyAPChestTexture(Chest chest, ArchipelagoItem APItem) {

            GameObject ChestTop = new GameObject("sprite");
            ChestTop.transform.parent = chest.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
            ChestTop.AddComponent<SpriteRenderer>().sprite = FindSprite("trinkets 1_slot_grey");
            ChestTop.transform.localPosition = new Vector3(0f, 0.1f, 1.2f);
            ChestTop.transform.localEulerAngles = new Vector3(57f, 180f, 0f);
            ChestTop.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            ItemFlags flag = APItem.Classification;
            if (flag == ItemFlags.Trap) {
                flag = new List<ItemFlags>() { ItemFlags.Advancement, ItemFlags.NeverExclude, ItemFlags.None }[new System.Random().Next(3)];
            }

            if (flag == ItemFlags.None) {
                chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials[0].color = new UnityEngine.Color(0f, 0.75f, 0f, 1f);
            } else if(flag == ItemFlags.NeverExclude) {
                chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials[0].color = new UnityEngine.Color(0f, 0.5f, 0.75f, 1f);
            } else if(flag == ItemFlags.Advancement) {
                chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = new Material[] {
                    ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().material,
                    chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials[1]
                };
            }
            
            if(APItem.Classification == ItemFlags.Trap) {
                ChestTop.transform.localEulerAngles = new Vector3(57f, 180f, 180f);
            }
        }

        public static void CheckCollectedItemFlags() {
            string Scene = SceneManager.GetActiveScene().name;
            if (TunicRandomizer.Settings.CollectReflectsInWorld) {
                foreach (PagePickup PagePickup in Resources.FindObjectsOfTypeAll<PagePickup>()) {
                    if (PagePickup != null && PagePickup.pageName != null) {
                        string PageId = $"{PagePickup.pageName} [{Scene}]";
                        if (SaveFile.GetInt($"randomizer {PageId} was collected") == 1) {
                            GameObject.Destroy(PagePickup.gameObject);
                        }
                    }
                }

                foreach (ItemPickup ItemPickup in Resources.FindObjectsOfTypeAll<ItemPickup>()) {
                    if (ItemPickup != null && ItemPickup.itemToGive != null) {
                        string ItemId = $"{ItemPickup.itemToGive.name} [{Scene}]";
                        if (SaveFile.GetInt($"randomizer {ItemId} was collected") == 1) {
                            if (ItemPickup.gameObject.scene.name == "Sword Access") {
                                GameObject.Find("_Setpieces/RelicPlinth (1)/").transform.GetChild(5).gameObject.SetActive(true);
                            }
                            GameObject.Destroy(ItemPickup.gameObject);
                        }
                    }
                }

                foreach (HeroRelicPickup RelicPickup in Resources.FindObjectsOfTypeAll<HeroRelicPickup>()) {
                    if (RelicPickup != null && RelicPickup.name != null) {
                        string RelicId = $"{RelicPickup.name} [{Scene}]";
                        if (SaveFile.GetInt($"randomizer {RelicId} was collected") == 1) {
                            GameObject.Destroy(RelicPickup.gameObject);
                        }
                    }
                }

                foreach (ShopItem ShopItem in Resources.FindObjectsOfTypeAll<ShopItem>()) {
                    if (ShopItem != null) {
                        string ShopId = $"{ShopItem.name} [{Scene}]";
                        if (SaveFile.GetInt($"randomizer {ShopId} was collected") == 1) {
                            ShopItem.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        public static void SetupItemPickup(ItemPickup ItemPickup) {
            
            if (ItemPickup != null && ItemPickup.itemToGive != null) {
                string ItemId = $"{ItemPickup.itemToGive.name} [{SceneLoaderPatches.SceneName}]";

                ArchipelagoItem ApItem = null;
                Check Check = null;
                ItemData ItemData =  null;

                if (ItemLookup.ItemList.ContainsKey(ItemId) || Locations.RandomizedLocations.ContainsKey(ItemId)) { 
                    if (IsArchipelago()) {
                        ApItem = ItemLookup.ItemList[ItemId];
                        if (Archipelago.instance.IsTunicPlayer(ApItem.Player)) {
                            ItemData = ItemLookup.Items[ApItem.ItemName];
                        }
                    }
                    if (IsSinglePlayer()) {
                        Check = Locations.RandomizedLocations[ItemId];
                        ItemData = ItemLookup.GetItemDataFromCheck(Check);
                    }
                    if (Locations.CheckedLocations[ItemId] || SaveFile.GetInt($"{ItemPatches.SaveFileCollectedKey} {ItemId}") == 1) {
                        return;
                    }

                    for (int i = 0; i < ItemPickup.transform.childCount; i++) {
                        ItemPickup.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    if (ItemPickup.GetComponent<MeshFilter>() != null) {
                        GameObject.Destroy(ItemPickup.GetComponent<MeshFilter>());
                        GameObject.Destroy(ItemPickup.GetComponent<MeshRenderer>());
                    }

                    GameObject NewItem = SetupItemBase(ItemPickup.transform, ApItem, Check);

                    TransformData TransformData;

                    if (ItemPositions.SpecificItemPlacement.ContainsKey(ItemPickup.itemToGive.name)) {
                        if (ItemPickup.itemToGive.name == "Stundagger" && SceneLoaderPatches.SceneName == "archipelagos_house") {
                            GameObject.Find("lanterndagger/").transform.localRotation = new Quaternion(0, 0, 0, 1);
                        }
                        if (ItemPickup.itemToGive.name == "Key" || ItemPickup.itemToGive.name == "Key (House)") {
                            NewItem.transform.parent.localRotation = SceneLoaderPatches.SceneName == "Overworld Redux" ? new Quaternion(0f, 0f, 0f, 0f) : new Quaternion(0f, 0.7071f, 0f, 0.7071f);
                            if (ItemData != null && ItemData.Type == ItemTypes.LADDER) {
                                NewItem.transform.GetChild(0).localEulerAngles = new Vector3(90, 0, 0);
                                NewItem.transform.GetChild(1).localEulerAngles = new Vector3(90, 0, 0);
                                NewItem.transform.GetChild(1).localPosition = new Vector3(2.6f, 1.9f, 0.7f);
                            }
                        }
                        if (IsArchipelago() && ItemData == null && (ApItem != null && !Archipelago.instance.IsTunicPlayer(ApItem.Player) || !ItemLookup.Items.ContainsKey(ApItem.ItemName))) {
                            TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name]["Other World"];
                        } else {
                            if (ItemData.ItemNameForInventory.Contains("Trinket - ") || ItemData.ItemNameForInventory == "Mask") {
                                TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name]["Trinket Card"];
                            } else if (ItemData.Type == ItemTypes.PAGE || ItemData.Type == ItemTypes.FAIRY) {
                                TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name][Enum.GetName(typeof(ItemTypes), ItemData.Type)];
                            } else if (ItemData.Type == ItemTypes.MONEY || ItemData.Type == ItemTypes.FOOLTRAP) {
                                if (ItemData.QuantityToGive < 30) {
                                    TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name]["money small"];
                                } else if (ItemData.QuantityToGive >= 30 && ItemData.QuantityToGive < 100) {
                                    TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name]["money medium"];
                                } else {
                                    TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name]["money large"];
                                }
                            } else if (ItemData.Type == ItemTypes.SWORDUPGRADE && (SaveFile.GetInt(SwordProgressionEnabled) == 1) && (IsSinglePlayer() || ApItem.Player == Archipelago.instance.GetPlayerSlot())) {
                                int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                                TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name].ContainsKey($"Sword Progression {SwordLevel}") ? ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name][$"Sword Progression {SwordLevel}"] : ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name][ItemData.ItemNameForInventory];
                            } else {
                                TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name].ContainsKey(ItemData.ItemNameForInventory) ? ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name][ItemData.ItemNameForInventory] : ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name][Enum.GetName(typeof(ItemTypes), ItemData.Type)];
                            }

                        }

                    } else {
                        TransformData = new TransformData(Vector3.one, Quaternion.identity, Vector3.one);
                    }

                    if (ItemPickup.itemToGive.name == "Sword") {
                        ItemPickup.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
                    }
                    NewItem.transform.localPosition = TransformData.pos;
                    NewItem.transform.localRotation = TransformData.rot;
                    NewItem.transform.localScale = TransformData.scale;
                    NewItem.transform.parent.gameObject.SetActive(true);

                    NewItem.SetActive(true);
                }
            }
        }

        public static void SetupPagePickup(PagePickup PagePickup) {
            if (PagePickup != null) {
                GameObject Page = PagePickup.gameObject.transform.GetChild(2).GetChild(0).gameObject;
                string ItemId = $"{PagePickup.pageName} [{SceneLoaderPatches.SceneName}]";

                ArchipelagoItem ApItem = null;
                Check Check = null;
                ItemData Item = null;

                if (ItemLookup.ItemList.ContainsKey(ItemId) || Locations.RandomizedLocations.ContainsKey(ItemId)) {
                    if (Locations.CheckedLocations[ItemId] || SaveFile.GetInt($"{ItemPatches.SaveFileCollectedKey} {ItemId}") == 1) {
                        GameObject.Destroy(PagePickup.gameObject);
                        return;
                    }
                    if (IsArchipelago()) {
                        int Player = Archipelago.instance.GetPlayerSlot();
                        ApItem = ItemLookup.ItemList[ItemId];
                        if (Archipelago.instance.IsTunicPlayer(ApItem.Player)) {
                            Item = ItemLookup.Items[ApItem.ItemName];
                            if (ItemLookup.Items.ContainsKey(ApItem.ItemName) && ItemLookup.Items[ApItem.ItemName].Type == ItemTypes.PAGE) {
                                return;
                            }
                        }
                    }
                    if (IsSinglePlayer()) {
                        Check = Locations.RandomizedLocations[ItemId];
                        Item = ItemLookup.GetItemDataFromCheck(Check);
                        if (Item.Type == ItemTypes.PAGE) {
                            return;
                        }
                    }


                    Page.transform.localScale = Vector3.one;
                    GameObject.Destroy(Page.transform.GetChild(1));
                    GameObject.Destroy(Page.GetComponent<MeshFilter>());
                    GameObject.Destroy(Page.GetComponent<MeshRenderer>());

                    for (int i = 1; i < Page.transform.childCount; i++) {
                        Page.transform.GetChild(i).gameObject.SetActive(false);
                    }

                    GameObject NewItem = SetupItemBase(Page.transform, ApItem, Check);

                    Page.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    TransformData TransformData;
                    if (IsArchipelago() && Item == null && (ApItem != null && !Archipelago.instance.IsTunicPlayer(ApItem.Player) || !ItemLookup.Items.ContainsKey(ApItem.ItemName))) {
                        TransformData = ItemPositions.Techbow["Other World"];
                        PagePickup.transform.GetChild(2).GetComponent<Rotate>().eulerAnglesPerSecond = new Vector3(0f, 45f, 0f);
                    } else {
                        if (Item.Type == ItemTypes.TRINKET) {
                            TransformData = ItemPositions.Techbow["Trinket Card"];
                        } else if (Item.Type == ItemTypes.MONEY || Item.Type == ItemTypes.FOOLTRAP) {
                            if (Item.QuantityToGive < 30) {
                                TransformData = ItemPositions.Techbow["money small"];
                            } else if (Item.QuantityToGive >= 30 && Item.QuantityToGive < 100) {
                                TransformData = ItemPositions.Techbow["money medium"];
                            } else {
                                TransformData = ItemPositions.Techbow["money large"];
                            }
                        } else if (Item.Type == ItemTypes.SWORDUPGRADE && (SaveFile.GetInt(SwordProgressionEnabled) == 1) && (IsSinglePlayer() || ApItem.Player == Archipelago.instance.GetPlayerSlot())) {
                            int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                            TransformData = ItemPositions.Techbow.ContainsKey($"Sword Progression {SwordLevel}") ? ItemPositions.Techbow[$"Sword Progression {SwordLevel}"] : ItemPositions.Techbow[Item.ItemNameForInventory];
                        } else {
                            TransformData = ItemPositions.Techbow.ContainsKey(Item.ItemNameForInventory) ? ItemPositions.Techbow[Item.ItemNameForInventory] : ItemPositions.Techbow[Enum.GetName(typeof(ItemTypes), Item.Type)];
                        }
                    }

                    NewItem.transform.localPosition = TransformData.pos;
                    NewItem.transform.localRotation = TransformData.rot;
                    NewItem.transform.localScale = TransformData.scale;
                    NewItem.SetActive(true);
                    Page.transform.GetChild(1).gameObject.SetActive(false);
                    Page.transform.GetChild(0).gameObject.GetComponent<ParticleSystemRenderer>().material.color = UnityEngine.Color.cyan;
                    PagePickup.optionalPickupPrompt = ScriptableObject.CreateInstance<LanguageLine>();
                    PagePickup.optionalPickupPrompt.text = $"tAk Ituhm?";
                }
            }
        }


        public static GameObject SetupItemBase(Transform Parent, ArchipelagoItem APItem = null, Check Check = null) {
            GameObject NewItem;
            if (IsArchipelago() && (!Archipelago.instance.IsTunicPlayer(APItem.Player) || !ItemLookup.Items.ContainsKey(APItem.ItemName))) {
                ItemFlags flag = APItem.Classification;
                if (flag == ItemFlags.Trap) {
                    flag = new List<ItemFlags>() { ItemFlags.Advancement, ItemFlags.NeverExclude, ItemFlags.None}[new System.Random().Next(3)];
                }
                NewItem = GameObject.Instantiate(Items[$"Other World {flag}"], Parent.transform.position, Parent.transform.rotation);
                if (APItem.Classification == ItemFlags.Trap) {
                    for (int i = 2; i < 6; i++) {
                        Vector3 flipped = NewItem.transform.GetChild(i).localEulerAngles;
                        NewItem.transform.GetChild(i).gameObject.transform.localEulerAngles = new Vector3(180, flipped.y, flipped.z);
                    }
                }
            } else {
                ItemData Item = ItemLookup.Items["Stick"];
                if (IsArchipelago() && APItem != null) {
                    Item = ItemLookup.Items[APItem.ItemName];
                } else if (IsSinglePlayer() && Check != null) {
                    Item = ItemLookup.GetItemDataFromCheck(Check);
                }
                if (Item.Type == ItemTypes.TRINKET) {
                    NewItem = GameObject.Instantiate(Items["Trinket Card"], Parent.transform.position, Parent.transform.rotation);
                    NewItem.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Cards[Item.ItemNameForInventory];
                } else if (Item.Type == ItemTypes.MONEY || Item.Type == ItemTypes.FOOLTRAP) {
                    if (Item.Type == ItemTypes.FOOLTRAP) {
                        Item.QuantityToGive = new System.Random().Next(150);
                    }
                    NewItem = MoneyObject(Parent, Item.QuantityToGive);
                } else if (Item.Type == ItemTypes.PAGE) {
                    NewItem = GameObject.Instantiate(PagePickup, Parent.transform.position, Parent.transform.rotation);
                } else if (Item.Type == ItemTypes.FAIRY) {
                    NewItem = GameObject.Instantiate(Chests["Fairy"], Parent.transform.position, Parent.transform.rotation);
                } else if (Item.Type == ItemTypes.SWORDUPGRADE && (SaveFile.GetInt(SwordProgressionEnabled) == 1) && (IsSinglePlayer() || APItem.Player == Archipelago.instance.GetPlayerSlot())) {
                    NewItem = SwordProgressionObject(Parent);
                } else if (Item.Type == ItemTypes.LADDER) {
                    NewItem = GameObject.Instantiate(Items["Ladder"], Parent.transform.position, Parent.transform.rotation);
                } else {
                    NewItem = GameObject.Instantiate(Items[Item.ItemNameForInventory], Parent.transform.position, Parent.transform.rotation);
                }
            }
            NewItem.transform.parent = Parent.transform;
            NewItem.layer = 0;
            for (int i = 0; i < NewItem.transform.childCount; i++) {
                NewItem.transform.GetChild(i).gameObject.layer = 0;
            }
            NewItem.SetActive(true);
            return NewItem;
        }

        private static GameObject MoneyObject(Transform Parent, int Quantity) {
            if (Quantity < 30) {
                return GameObject.Instantiate(Items["money small"], Parent.transform.position, Parent.transform.rotation);
            } else if (Quantity >= 30 && Quantity < 100) {
                return GameObject.Instantiate(Items["money medium"], Parent.transform.position, Parent.transform.rotation);
            } else {
                return GameObject.Instantiate(Items["money large"], Parent.transform.position, Parent.transform.rotation);
            }
        }

        private static GameObject SwordProgressionObject(Transform Parent) {
            int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
            GameObject NewItem;
            if (SwordLevel == 0) {
                NewItem = GameObject.Instantiate(Items["Stick"], Parent.transform.position, Parent.transform.rotation);
            } else if (SwordLevel == 1) {
                NewItem = GameObject.Instantiate(Items["Sword"], Parent.transform.position, Parent.transform.rotation);
            } else if (SwordLevel == 2) {
                NewItem = GameObject.Instantiate(SecondSword, Parent.transform.position, Parent.transform.rotation);
            } else if (SwordLevel == 3) {
                NewItem = GameObject.Instantiate(ThirdSword, Parent.transform.position, Parent.transform.rotation);
            } else {
                NewItem = GameObject.Instantiate(Items["Sword"], Parent.transform.position, Parent.transform.rotation);
            }
            return NewItem;
        }

        public static void SetupRedHexagonPlinth() {
            GameObject Plinth = GameObject.Find("_Hexagon Plinth Assembly/hexagon plinth/PRISM/questagon");
            string ItemId = "Hexagon Red [Fortress Arena]";

            ArchipelagoItem ApItem = null;
            Check Check = null;
            ItemData HexagonItem = null;

            if (IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld && SaveFile.GetInt($"randomizer {ItemId} was collected") == 1) {
                GameObject.Destroy(Plinth);
                return;
            }

            if (Plinth != null && (ItemLookup.ItemList.ContainsKey(ItemId) || Locations.RandomizedLocations.ContainsKey(ItemId))) {
                if (IsArchipelago()) {
                    ApItem = ItemLookup.ItemList[ItemId];
                    if (Archipelago.instance.IsTunicPlayer(ApItem.Player)) {
                        HexagonItem = ItemLookup.Items[ApItem.ItemName];
                        if (HexagonItem.ItemNameForInventory == "Hexagon Red") {
                            return;
                        }
                    }
                }
                if(IsSinglePlayer()) {
                    Check = Locations.RandomizedLocations[ItemId];
                    HexagonItem = ItemLookup.GetItemDataFromCheck(Check);
                    if (HexagonItem.ItemNameForInventory == "Hexagon Red") {
                        return;
                    }
                }

                if (Plinth.GetComponent<MeshFilter>() != null) {
                    GameObject.Destroy(Plinth.GetComponent<MeshRenderer>());
                    GameObject.Destroy(Plinth.GetComponent<MeshFilter>());
                }

                for (int i = 0; i < Plinth.transform.childCount; i++) {
                    Plinth.transform.GetChild(i).gameObject.SetActive(false);
                }

                GameObject NewItem = SetupItemBase(Plinth.transform, ApItem, Check);

                TransformData TransformData;
                if (IsArchipelago() && HexagonItem == null && (ApItem != null && !Archipelago.instance.IsTunicPlayer(ApItem.Player) || !ItemLookup.Items.ContainsKey(ApItem.ItemName))) {
                    TransformData = ItemPositions.HexagonRed["Other World"];
                } else {
                    if (HexagonItem.Type == ItemTypes.TRINKET) {
                        TransformData = ItemPositions.HexagonRed["Trinket Card"];
                    } else if (HexagonItem.Type == ItemTypes.MONEY || HexagonItem.Type == ItemTypes.FOOLTRAP) {
                        if (HexagonItem.QuantityToGive < 30) {
                            TransformData = ItemPositions.HexagonRed["money small"];
                        } else if (HexagonItem.QuantityToGive >= 30 && HexagonItem.QuantityToGive < 100) {
                            TransformData = ItemPositions.HexagonRed["money medium"];
                        } else {
                            TransformData = ItemPositions.HexagonRed["money large"];
                        }
                    } else if (HexagonItem.Type == ItemTypes.SWORDUPGRADE && (SaveFile.GetInt(SwordProgressionEnabled) == 1) && (IsSinglePlayer() || ApItem.Player == Archipelago.instance.GetPlayerSlot())) {
                        int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                        TransformData = ItemPositions.HexagonRed.ContainsKey($"Sword Progression {SwordLevel}") ? ItemPositions.HexagonRed[$"Sword Progression {SwordLevel}"] : ItemPositions.HexagonRed[HexagonItem.ItemNameForInventory];
                    } else {
                        TransformData = ItemPositions.HexagonRed.ContainsKey(HexagonItem.ItemNameForInventory) ? ItemPositions.HexagonRed[HexagonItem.ItemNameForInventory] : ItemPositions.HexagonRed[Enum.GetName(typeof(ItemTypes), HexagonItem.Type)];
                    }
                }

                NewItem.transform.localPosition = TransformData.pos;
                NewItem.transform.localRotation = TransformData.rot;
                NewItem.transform.localScale = TransformData.scale;
                Plinth.transform.localPosition = new Vector3(-1.4f, -8.7f, -1.5f);
                Plinth.transform.localScale = Vector3.one;
                NewItem.SetActive(true);
            }
        }

        public static void SetupBlueHexagonPlinth() {
            GameObject Plinth = GameObject.Find("_Plinth/turn off when taken/questagon");
            string ItemId = "Hexagon Blue [ziggurat2020_3]";

            ArchipelagoItem ApItem = null;
            Check Check = null;
            ItemData HexagonItem = null;

            if (IsArchipelago() && (TunicRandomizer.Settings.CollectReflectsInWorld && SaveFile.GetInt($"randomizer {ItemId} was collected") == 1)) {
                GameObject.Destroy(Plinth);
                return;
            }
            if (Plinth != null && (ItemLookup.ItemList.ContainsKey(ItemId) || Locations.RandomizedLocations.ContainsKey(ItemId))) {
                if (IsArchipelago()) {
                    int Player = Archipelago.instance.GetPlayerSlot();
                    ApItem = ItemLookup.ItemList[ItemId];
                    if (Archipelago.instance.IsTunicPlayer(ApItem.Player)) {
                        HexagonItem = ItemLookup.Items[ApItem.ItemName];
                        if (HexagonItem.ItemNameForInventory == "Hexagon Blue") {
                            return;
                        }
                    }
                }
                if (IsSinglePlayer()) {
                    Check = Locations.RandomizedLocations[ItemId];
                    HexagonItem = ItemLookup.GetItemDataFromCheck(Check);
                    if (HexagonItem.ItemNameForInventory == "Hexagon Blue") {
                        return;
                    }
                }

                if (Plinth.GetComponent<MeshFilter>() != null) {
                    GameObject.Destroy(Plinth.GetComponent<MeshRenderer>());
                    GameObject.Destroy(Plinth.GetComponent<MeshFilter>());
                }

                for (int i = 0; i < Plinth.transform.childCount; i++) {
                    Plinth.transform.GetChild(i).gameObject.SetActive(false);
                }

                GameObject NewItem = SetupItemBase(Plinth.transform, ApItem, Check);

                TransformData TransformData;
                if (IsArchipelago() && HexagonItem == null && (ApItem != null && !Archipelago.instance.IsTunicPlayer(ApItem.Player) || !ItemLookup.Items.ContainsKey(ApItem.ItemName))) {
                    TransformData = ItemPositions.HexagonRed["Other World"];
                } else {
                    if (HexagonItem.Type == ItemTypes.TRINKET) {
                        TransformData = ItemPositions.HexagonRed["Trinket Card"];
                    } else if (HexagonItem.Type == ItemTypes.MONEY || HexagonItem.Type == ItemTypes.FOOLTRAP) {
                        if (HexagonItem.QuantityToGive < 30) {
                            TransformData = ItemPositions.HexagonRed["money small"];
                        } else if (HexagonItem.QuantityToGive >= 30 && HexagonItem.QuantityToGive < 100) {
                            TransformData = ItemPositions.HexagonRed["money medium"];
                        } else {
                            TransformData = ItemPositions.HexagonRed["money large"];
                        }
                    } else if (HexagonItem.Type == ItemTypes.SWORDUPGRADE && (SaveFile.GetInt(SwordProgressionEnabled) == 1) && (IsSinglePlayer() || ApItem.Player == Archipelago.instance.GetPlayerSlot())) {
                        int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                        TransformData = ItemPositions.HexagonRed.ContainsKey($"Sword Progression {SwordLevel}") ? ItemPositions.HexagonRed[$"Sword Progression {SwordLevel}"] : ItemPositions.HexagonRed[HexagonItem.ItemNameForInventory];
                    } else {
                        TransformData = ItemPositions.HexagonRed.ContainsKey(HexagonItem.ItemNameForInventory) ? ItemPositions.HexagonRed[HexagonItem.ItemNameForInventory] : ItemPositions.HexagonRed[Enum.GetName(typeof(ItemTypes), HexagonItem.Type)];
                    }
                }

                NewItem.transform.localPosition = TransformData.pos;
                NewItem.transform.localRotation = TransformData.rot;
                NewItem.transform.localScale = TransformData.scale;
                Plinth.transform.localPosition = new Vector3(-0.1f, 4.7f, -1.6f);
                Plinth.transform.localScale = Vector3.one;
                NewItem.SetActive(true);
            }
        }

        public static void SwapSiegeEngineCrown() {
            GameObject VaultKey = GameObject.Find("Spidertank/Spidertank_skeleton/root/thorax/vault key graphic");

            ArchipelagoItem ApItem = null;
            Check Check = null;
            ItemData VaultKeyItem = null;

            string ItemId = "Vault Key (Red) [Fortress Arena]";
            if (IsArchipelago() && (TunicRandomizer.Settings.CollectReflectsInWorld && SaveFile.GetInt($"randomizer {ItemId} was collected") == 1)) {
                GameObject.Destroy(VaultKey);
                return;
            }
            if (VaultKey != null && (ItemLookup.ItemList.ContainsKey(ItemId) || Locations.RandomizedLocations.ContainsKey(ItemId))) {
                if (IsArchipelago()) {
                    ApItem = ItemLookup.ItemList[ItemId];
                    if (Archipelago.instance.IsTunicPlayer(ApItem.Player)) {
                        VaultKeyItem = ItemLookup.Items[ApItem.ItemName];
                        if (VaultKeyItem.ItemNameForInventory == "Vault Key (Red)") {
                            return;
                        }
                    }
                }
                if (IsSinglePlayer()) {
                    Check = Locations.RandomizedLocations[ItemId];
                    VaultKeyItem = ItemLookup.GetItemDataFromCheck(Check);
                    if (VaultKeyItem.ItemNameForInventory == "Vault Key (Red)") {
                        return;
                    }
                }
                if (VaultKey.GetComponent<MeshFilter>() != null) {
                    GameObject.Destroy(VaultKey.GetComponent<MeshRenderer>());
                    GameObject.Destroy(VaultKey.GetComponent<MeshFilter>());
                }

                for (int i = 0; i < VaultKey.transform.childCount; i++) {
                    VaultKey.transform.GetChild(i).gameObject.SetActive(false);
                }

                GameObject NewItem = SetupItemBase(VaultKey.transform, ApItem, Check);

                TransformData TransformData;
                if (IsArchipelago() && VaultKeyItem == null && (ApItem != null && !Archipelago.instance.IsTunicPlayer(ApItem.Player) || !ItemLookup.Items.ContainsKey(ApItem.ItemName))) {
                    TransformData = ItemPositions.VaultKeyRed["Other World"];
                } else {
                    if (VaultKeyItem.Type == ItemTypes.TRINKET) {
                        TransformData = ItemPositions.VaultKeyRed["Trinket Card"];
                    } else if (VaultKeyItem.Type == ItemTypes.MONEY || VaultKeyItem.Type == ItemTypes.FOOLTRAP) {
                        if (VaultKeyItem.QuantityToGive < 30) {
                            TransformData = ItemPositions.VaultKeyRed["money small"];
                        } else if (VaultKeyItem.QuantityToGive >= 30 && VaultKeyItem.QuantityToGive < 100) {
                            TransformData = ItemPositions.VaultKeyRed["money medium"];
                        } else {
                            TransformData = ItemPositions.VaultKeyRed["money large"];
                        }
                    } else if (VaultKeyItem.Type == ItemTypes.SWORDUPGRADE && (SaveFile.GetInt(SwordProgressionEnabled) == 1) && (IsSinglePlayer() || ApItem.Player == Archipelago.instance.GetPlayerSlot())) {
                        int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                        TransformData = ItemPositions.VaultKeyRed.ContainsKey($"Sword Progression {SwordLevel}") ? ItemPositions.VaultKeyRed[$"Sword Progression {SwordLevel}"] : ItemPositions.VaultKeyRed[VaultKeyItem.ItemNameForInventory];
                    } else {
                        TransformData = ItemPositions.VaultKeyRed.ContainsKey(VaultKeyItem.ItemNameForInventory) ? ItemPositions.VaultKeyRed[VaultKeyItem.ItemNameForInventory] : ItemPositions.VaultKeyRed[Enum.GetName(typeof(ItemTypes), VaultKeyItem.Type)];
                    }
                }

                NewItem.transform.localPosition = TransformData.pos;
                NewItem.transform.localRotation = TransformData.rot;
                NewItem.transform.localScale = TransformData.scale;
                NewItem.SetActive(true);
            }

        }

        public static void SetupShopItems() {

            for (int i = 0; i < ShopItemIDs.Count; i++) {

                ArchipelagoItem ApItem = null;
                Check Check = null;
                ItemData Item = null;

                if (!Locations.CheckedLocations[ShopItemIDs[i]]) {
                    
                    GameObject ItemHolder = GameObject.Find(ShopGameObjectIDs[i]);
                    GameObject NewItem;

                    if (ItemHolder.name.Contains("Trinket Coin")) {
                        ItemHolder.transform.rotation = new Quaternion(.7071f, 0f, 0f, -.7071f);
                    }
                    // Destroy original coin child meshes if they exist;
                    for (int j = 0; j < ItemHolder.transform.childCount; j++) {
                        GameObject.Destroy(ItemHolder.transform.GetChild(j).gameObject);
                    }
                    for (int j = 1; j < ItemHolder.transform.parent.childCount; j++) {
                        GameObject.Destroy(ItemHolder.transform.parent.GetChild(j).gameObject);
                    }
                    if (IsArchipelago()) {
                        ApItem = ItemLookup.ItemList[ShopItemIDs[i]];
                        if (Archipelago.instance.IsTunicPlayer(ApItem.Player)) {
                            Item = ItemLookup.Items[ApItem.ItemName];
                        }
                    }
                    if (IsSinglePlayer()) {
                        Check = Locations.RandomizedLocations[ShopItemIDs[i]];
                        Item = ItemLookup.GetItemDataFromCheck(Check);
                    }

                    NewItem = SetupItemBase(ItemHolder.transform, ApItem, Check);

                    TransformData TransformData;
                    if (IsArchipelago() && Item == null && (ApItem != null && !Archipelago.instance.IsTunicPlayer(ApItem.Player) || !ItemLookup.Items.ContainsKey(ApItem.ItemName))) {
                        TransformData = ItemPositions.Shop["Other World"];
                    } else {
                        if (Item.Type == ItemTypes.TRINKET) {
                            TransformData = ItemPositions.Shop["Trinket Card"];
                        } else if (Item.Type == ItemTypes.MONEY || Item.Type == ItemTypes.FOOLTRAP) {
                            if (Item.QuantityToGive < 30) {
                                TransformData = ItemPositions.Shop["money small"];
                            } else if (Item.QuantityToGive >= 30 && Item.QuantityToGive < 100) {
                                TransformData = ItemPositions.Shop["money medium"];
                            } else {
                                TransformData = ItemPositions.Shop["money large"];
                            }
                        } else if (Item.Type == ItemTypes.SWORDUPGRADE && (SaveFile.GetInt(SwordProgressionEnabled) == 1) && (IsSinglePlayer() || ApItem.Player == Archipelago.instance.GetPlayerSlot())) {
                            int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                            TransformData = ItemPositions.Shop.ContainsKey($"Sword Progression {SwordLevel}") ? ItemPositions.Shop[$"Sword Progression {SwordLevel}"] : ItemPositions.Shop[Item.ItemNameForInventory];
                        } else {
                            TransformData = ItemPositions.Shop.ContainsKey(Item.ItemNameForInventory) ? ItemPositions.Shop[Item.ItemNameForInventory] : ItemPositions.Shop[Enum.GetName(typeof(ItemTypes), Item.Type)];
                        }

                        if (Item.Type == ItemTypes.PAGE) {
                            GameObject.Destroy(NewItem.GetComponent<Rotate>());
                            GameObject.Destroy(NewItem.transform.GetChild(0).GetChild(0).gameObject);
                        }
                    }

                    NewItem.transform.parent = ItemHolder.transform.parent;
                    NewItem.transform.localPosition = TransformData.pos;
                    NewItem.transform.localRotation = TransformData.rot;
                    NewItem.transform.localScale = TransformData.scale;
                    NewItem.SetActive(true);

                    NewItem.layer = 12;
                    NewItem.transform.localPosition = Vector3.zero;
                    for (int j = 0; j < NewItem.transform.childCount; j++) {
                        NewItem.transform.GetChild(j).gameObject.layer = 12;
                    }

                    if ((IsSinglePlayer() || Archipelago.instance.IsTunicPlayer(ApItem.Player)) && Item.Name.Contains("Ice Bomb")) {
                        NewItem.transform.GetChild(0).gameObject.SetActive(false);
                    }

                    ItemHolder.SetActive(false);
                }
            }
        }

        public static void SetupHeroRelicPickup(HeroRelicPickup HeroRelicPickup) {
            string ItemId = $"{HeroRelicPickup.name} [{SceneLoaderPatches.SceneName}]";
            if (ItemLookup.ItemList.ContainsKey(ItemId) || Locations.RandomizedLocations.ContainsKey(ItemId)) {

                ArchipelagoItem ApItem = null;
                Check Check = null;
                ItemData Item = null;

                for (int i = 0; i < HeroRelicPickup.transform.childCount; i++) {
                    HeroRelicPickup.transform.GetChild(i).gameObject.SetActive(false);
                }
                if (HeroRelicPickup.GetComponent<MeshFilter>() != null) {
                    GameObject.Destroy(HeroRelicPickup.GetComponent<MeshFilter>());
                    GameObject.Destroy(HeroRelicPickup.GetComponent<MeshRenderer>());
                }
                if (IsArchipelago()) {
                    ApItem = ItemLookup.ItemList[ItemId];
                    if (Archipelago.instance.IsTunicPlayer(ApItem.Player)) {
                        Item = ItemLookup.Items[ApItem.ItemName];
                    }
                }
                if (IsSinglePlayer()) {
                    Check = Locations.RandomizedLocations[ItemId];
                    Item = ItemLookup.GetItemDataFromCheck(Check);
                }
                GameObject NewItem = SetupItemBase(HeroRelicPickup.transform, ApItem, Check);

                if (IsArchipelago() && Item == null && (ApItem != null && !Archipelago.instance.IsTunicPlayer(ApItem.Player) || !ItemLookup.Items.ContainsKey(ApItem.ItemName))) {
                    if (NewItem.GetComponent<Rotate>() == null) {
                        NewItem.AddComponent<Rotate>().eulerAnglesPerSecond = new Vector3(0f, 45f, 0f);
                    }
                    NewItem.transform.localRotation = Quaternion.identity;
                    NewItem.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                } else {
                    NewItem.transform.localRotation = ItemPositions.ItemPickupRotations.ContainsKey(Item.ItemNameForInventory) ? ItemPositions.ItemPickupRotations[Item.ItemNameForInventory] : Quaternion.Euler(0, 0, 0);
                    NewItem.transform.localPosition = ItemPositions.ItemPickupPositions.ContainsKey(Item.ItemNameForInventory) ? ItemPositions.ItemPickupPositions[Item.ItemNameForInventory] : Vector3.zero;
                    if (Item.Type == ItemTypes.SWORDUPGRADE && SaveFile.GetInt(SwordProgressionEnabled) == 1 && (IsSinglePlayer() || ApItem.Player == Archipelago.instance.GetPlayerSlot())) {
                        int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                        TransformData TransformData = ItemPositions.Techbow.ContainsKey($"Sword Progression {SwordLevel}") ? ItemPositions.Techbow[$"Sword Progression {SwordLevel}"] : ItemPositions.Techbow[Item.ItemNameForInventory];
                        NewItem.transform.localPosition = TransformData.pos;
                        NewItem.transform.localRotation = TransformData.rot;
                        NewItem.transform.localScale = TransformData.scale;
                    }
                    NewItem.transform.localScale *= 2;
                    if ((Item.Type == ItemTypes.MONEY || Item.Type == ItemTypes.FOOLTRAP) && Item.QuantityToGive >= 100) {
                        NewItem.transform.localScale *= 3;
                    }
                    if (Item.Type == ItemTypes.FAIRY) {
                        NewItem.transform.localScale = Vector3.one;
                    }
                    if (Item.Type == ItemTypes.LADDER) {
                        NewItem.transform.localScale *= 2;
                    }
                    if (NewItem.GetComponent<Rotate>() == null) {
                        NewItem.AddComponent<Rotate>().eulerAnglesPerSecond = (Item.ItemNameForInventory == "Relic - Hero Water" || Item.ItemNameForInventory == "Upgrade Offering - PotionEfficiency Swig - Ash" || Item.ItemNameForInventory == "Techbow") ? new Vector3(0f, 0f, 25f) : new Vector3(0f, 25f, 0f);
                        if (Item.ItemNameForInventory == "Sword Progression" && (SaveFile.GetInt(SwordProgressionLevel) == 0 || SaveFile.GetInt(SwordProgressionLevel) == 3)) {
                            NewItem.GetComponent<Rotate>().eulerAnglesPerSecond = new Vector3(0f, 0f, 25f);
                        }
                    }
                }

                NewItem.SetActive(true);
            }
        }

        public static void AddNewShopItems() {
            try {
                if (IceFlask == null) {
                    if (Resources.FindObjectsOfTypeAll<BombFlask>().Where(bomb => bomb.name == "Ice flask").Count() > 0) {
                        IceFlask = Resources.FindObjectsOfTypeAll<BombFlask>().Where(bomb => bomb.name == "Ice flask").ToList()[0].transform.GetChild(0).gameObject;
                        Items["Ice Bomb"] = IceFlask;
                    }
                }
                GameObject IceBombs = GameObject.Instantiate(GameObject.Find("Shop/Item Holder/Firebombs/"));
                GameObject Pepper = GameObject.Instantiate(GameObject.Find("Shop/Item Holder/Ivy/"));
                IceBombs.name = "Ice Bombs";
                Pepper.name = "Pepper";
                IceBombs.transform.parent = GameObject.Find("Shop/Item Holder/Firebombs/").transform.parent;
                Pepper.transform.parent = GameObject.Find("Shop/Item Holder/Ivy/").transform.parent;

                IceBombs.GetComponent<ShopItem>().itemToGive = Inventory.GetItemByName("Ice Bomb");
                IceBombs.GetComponent<ShopItem>().price = 200;
                Pepper.GetComponent<ShopItem>().itemToGive = Inventory.GetItemByName("Pepper");
                Pepper.GetComponent<ShopItem>().price = 130;
                
                for (int i = 0; i < 3; i++) {
                    GameObject.Destroy(IceBombs.transform.GetChild(0).GetChild(i).GetChild(0).gameObject);
                    if (IceFlask != null) { 
                        GameObject newIceBomb = GameObject.Instantiate(IceFlask);
                        newIceBomb.transform.position = IceBombs.transform.GetChild(0).GetChild(i).position;
                        newIceBomb.transform.localEulerAngles = IceBombs.transform.GetChild(0).GetChild(i).localEulerAngles;
                        GameObject.Destroy(IceBombs.transform.GetChild(0).GetChild(i).gameObject);
                        newIceBomb.transform.parent = IceBombs.transform.GetChild(0);
                        newIceBomb.transform.GetChild(0).gameObject.SetActive(false);
                        newIceBomb.transform.localScale = Vector3.one;
                    } else {
                        IceBombs.transform.GetChild(0).GetChild(i).GetComponent<MeshFilter>().mesh = Items["Ice Bomb"].GetComponent<MeshFilter>().mesh;
                        IceBombs.transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>().materials = Items["Ice Bomb"].GetComponent<MeshRenderer>().materials;
                        IceBombs.transform.GetChild(0).GetChild(i).localScale = Vector3.one;
                    }
                }

                Pepper.transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = Items["Pepper"].GetComponent<MeshFilter>().mesh;
                Pepper.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().materials = Items["Pepper"].GetComponent<MeshRenderer>().materials;
                Pepper.transform.GetChild(0).GetChild(0).localScale = new Vector3(1.2f, 1.2f, 1.2f);

                List<ShopItem> items = ShopManager.cachedShopItems != null ? ShopManager.cachedShopItems.ToList() : new List<ShopItem>();
                items.Add(IceBombs.GetComponent<ShopItem>());
                items.Add(Pepper.GetComponent<ShopItem>());
                ShopManager.cachedShopItems = items.ToArray();
            } catch (Exception e) {
                Logger.LogError("Failed to create permanent ice bomb and/or pepper items in the shop.");
            }
        }

        public static void ShopManager_entrySequence_MoveNext_PostfixPatch(ShopManager._entrySequence_d__14 __instance, ref bool __result) {
            if (SceneManager.GetActiveScene().name == "Shop" && __instance._f_5__2 > 0.5f && __instance._f_5__2 < 0.6f) {
                for (int i = 0; i < 3; i++) {
                    if (IceFlask != null) {
                        try {
                            __instance.__4__this.transform.GetChild(0).GetChild(10).GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(true);
                            foreach (ShopItem shopItem in ShopManager.cachedShopItems) {
                                if (ShopItemIDs.Contains($"{shopItem.name} [Shop]") && !Locations.CheckedLocations[$"{shopItem.name} [Shop]"]
                                    && ((IsSinglePlayer() && Locations.RandomizedLocations[$"{shopItem.name} [Shop]"].Reward.Name.Contains("Ice Bomb")) 
                                    || (IsArchipelago() && ItemLookup.ItemList[$"{shopItem.name} [Shop]"].ItemName.Contains("Ice Bomb") && Archipelago.instance.IsTunicPlayer(ItemLookup.ItemList[$"{shopItem.name} [Shop]"].Player)))) {
                                    shopItem.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(true);
                                }
                            }
                        } catch (Exception e) {

                        }
                    }
                }
            }
        }

        public static void CreateConstructionObject() {
            UnderConstruction = GameObject.Instantiate(GameObject.Find("_Signposts/Signpost/"));
            UnderConstruction.transform.GetChild(1).localPosition /= 2;
            GameObject.Destroy(UnderConstruction.transform.GetChild(2).gameObject);
            GameObject.Destroy(UnderConstruction.transform.GetChild(0).gameObject);
            UnderConstruction.AddComponent<MeshFilter>().mesh = Resources.FindObjectsOfTypeAll<Mesh>().Where(mesh => mesh.name == "under construction").First();
            UnderConstruction.AddComponent<MeshRenderer>().material = ModelSwaps.FindMaterial("under construction");
            UnderConstruction.AddComponent<BoxCollider>();
            GameObject.Destroy(UnderConstruction.GetComponent<Signpost>());
            UnderConstruction.AddComponent<UnderConstruction>();
            UnderConstruction.GetComponent<InteractionTrigger>().Start();
            UnderConstruction.GetComponent<SphereCollider>().radius = 2.5f;
            UnderConstruction.transform.localScale = Vector3.one;
            UnderConstruction.layer = 0;
            UnderConstruction.name = "under construction";
            UnderConstruction.SetActive(false);
            GameObject.DontDestroyOnLoad(UnderConstruction);
        }

        public static void LoadTextures() {

            Material ImageMaterial = FindMaterial("UI Add");

            HexagonGoldImage = CreateSprite(ImageData.GoldHex, ImageMaterial, 160, 160, "Hexagon Quest");
            TuncTitleImage = CreateSprite(ImageData.TuncTitle, ImageMaterial, 1400, 742, "tunc title logo");
            TuncImage = CreateSprite(ImageData.Tunc, ImageMaterial, 148, 148, "tunc sprite");

            CustomItemImages.Add("Librarian Sword", CreateSprite(ImageData.SecondSword, ImageMaterial, SpriteName: "Randomizer items_Librarian Sword"));
            CustomItemImages.Add("Heir Sword", CreateSprite(ImageData.ThirdSword, ImageMaterial, SpriteName: "Randomizer items_Heir Sword"));
            CustomItemImages.Add("Mr Mayor", CreateSprite(ImageData.MrMayor, ImageMaterial, SpriteName: "Randomizer items_Mr Mayor"));
            CustomItemImages.Add("Secret Legend", CreateSprite(ImageData.SecretLegend, ImageMaterial, SpriteName: "Randomizer items_Secret Legend"));
            CustomItemImages.Add("Sacred Geometry", CreateSprite(ImageData.SacredGeometry, ImageMaterial, SpriteName: "Randomizer items_Sacred Geometry"));
            CustomItemImages.Add("Vintage", CreateSprite(ImageData.Vintage, ImageMaterial, SpriteName: "Randomizer items_Vintage"));
            CustomItemImages.Add("Just Some Pals", CreateSprite(ImageData.JustSomePals, ImageMaterial, SpriteName: "Randomizer items_Just Some Pals"));
            CustomItemImages.Add("Regal Weasel", CreateSprite(ImageData.RegalWeasel, ImageMaterial, SpriteName: "Randomizer items_Regal Weasel"));
            CustomItemImages.Add("Spring Falls", CreateSprite(ImageData.SpringFalls, ImageMaterial, SpriteName: "Randomizer items_Spring Falls"));
            CustomItemImages.Add("Power Up", CreateSprite(ImageData.PowerUp, ImageMaterial, SpriteName: "Randomizer items_Power Up"));
            CustomItemImages.Add("Back To Work", CreateSprite(ImageData.BackToWork, ImageMaterial, SpriteName: "Randomizer items_Back To Work"));
            CustomItemImages.Add("Phonomath", CreateSprite(ImageData.Phonomath, ImageMaterial, SpriteName: "Randomizer items_Phonomath"));
            CustomItemImages.Add("Dusty", CreateSprite(ImageData.Dusty, ImageMaterial, SpriteName: "Randomizer items_Dusty"));
            CustomItemImages.Add("Forever Friend", CreateSprite(ImageData.ForeverFriend, ImageMaterial, SpriteName: "Randomizer items_Forever Friend"));
            CustomItemImages.Add("Red Questagon", CreateSprite(ImageData.RedQuestagon, ImageMaterial, SpriteName: "Randomizer items_Red Questagon"));
            CustomItemImages.Add("Green Questagon", CreateSprite(ImageData.GreenQuestagon, ImageMaterial, SpriteName: "Randomizer items_Green Questagon"));
            CustomItemImages.Add("Blue Questagon", CreateSprite(ImageData.BlueQuestagon, ImageMaterial, SpriteName: "Randomizer items_Blue Questagon"));
            CustomItemImages.Add("Gold Questagon", CreateSprite(ImageData.GoldHex, ImageMaterial, SpriteName: "Randomizer items_Gold Questagon"));
            CustomItemImages.Add("Hero Relic - ATT", CreateSprite(ImageData.HeroRelicATT, ImageMaterial, SpriteName: "Randomizer items_Hero Relic - ATT"));
            CustomItemImages.Add("Hero Relic - DEF", CreateSprite(ImageData.HeroRelicDef, ImageMaterial, SpriteName: "Randomizer items_Hero Relic - DEF"));
            CustomItemImages.Add("Hero Relic - POTION", CreateSprite(ImageData.HeroRelicPotion, ImageMaterial, SpriteName: "Randomizer items_Hero Relic - POTION"));
            CustomItemImages.Add("Hero Relic - HP", CreateSprite(ImageData.HeroRelicHP, ImageMaterial, SpriteName: "Randomizer items_Hero Relic - HP"));
            CustomItemImages.Add("Hero Relic - SP", CreateSprite(ImageData.HeroRelicSP, ImageMaterial, SpriteName: "Randomizer items_Hero Relic - SP"));
            CustomItemImages.Add("Hero Relic - MP", CreateSprite(ImageData.HeroRelicMP, ImageMaterial, SpriteName: "Randomizer items_Hero Relic - MP"));
            CustomItemImages.Add("Fool Trap", CreateSprite(ImageData.TinyFox, ImageMaterial, SpriteName: "Randomizer items_Fool Trap"));
            CustomItemImages.Add("Archipelago Item", CreateSprite(ImageData.ArchipelagoItem, ImageMaterial, 128, 128, SpriteName: "Randomizer items_Archipelago Item"));
            CustomItemImages.Add("Torch Redux", CreateSprite(ImageData.TorchRedux, ImageMaterial, 160, 160, SpriteName: "Randomizer items_Torch redux"));
            CustomItemImages.Add("AbilityShuffle", CreateSprite(ImageData.Abilities, ImageMaterial, 200, 100, SpriteName: "Randomizer heading_Abilities"));
            CustomItemImages.Add("Dath Stone Texture", CreateSprite(ImageData.DathSteneTexture, ImageMaterial, 200, 100, SpriteName: "Randomizer dath stone texture"));
            CustomItemImages.Add("Ladder", CreateSprite(ImageData.Ladder, ImageMaterial, 160, 160, SpriteName: "Randomizer items_ladder"));
            CustomItemImages.Add("Secret Mayor", CreateSprite(ImageData.SecretMayor, ImageMaterial, 1400, 675, SpriteName: "Randomizer secret_mayor"));

            Inventory.GetItemByName("Librarian Sword").icon = CustomItemImages["Librarian Sword"].GetComponent<Image>().sprite;
            Inventory.GetItemByName("Heir Sword").icon = CustomItemImages["Heir Sword"].GetComponent<Image>().sprite;
            Inventory.GetItemByName("Dath Stone").icon = Inventory.GetItemByName("Dash Stone").icon;
            Inventory.GetItemByName("Hexagon Gold").icon = CustomItemImages["Gold Questagon"].GetComponent<Image>().sprite;
            Inventory.GetItemByName("Torch").icon = CustomItemImages["Torch Redux"].GetComponent<Image>().sprite;
        }

        public static GameObject CreateSprite(string ImageData, Material imgMaterial, int Width = 160, int Height = 160, string SpriteName = "") {

            Texture2D Texture = new Texture2D(Width, Height, TextureFormat.DXT1, false);
            ImageConversion.LoadImage(Texture, Convert.FromBase64String(ImageData));

            GameObject obj = new GameObject(SpriteName + " image");
            //Sprite.Create(Texture2D, Rect, Vector2, float, uint, SpriteMeshType, Vector4, bool)
            Sprite sprite = Sprite.CreateSprite(Texture, new Rect(0, 0, Width, Height), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect, Vector4.zero, false);
            sprite.name = SpriteName;
            obj.AddComponent<Image>().sprite = sprite;
            obj.GetComponent<Image>().material = imgMaterial;
            GameObject.DontDestroyOnLoad(obj);
            obj.transform.position = new Vector3(-30000f, -30000f, -30000f);
            obj.SetActive(false);
            return obj;
        }

        public static Material FindMaterial(string MaterialName) {
            List<Material> Material = Resources.FindObjectsOfTypeAll<Material>().Where(Mat => Mat.name == MaterialName).ToList();
            if (Material != null && Material.Count > 0) {
                return Material[0];
            } else {
                return null;
            }
        }

        public static Sprite FindSprite(string SpriteName) {
            List<Sprite> Sprites = Resources.FindObjectsOfTypeAll<Sprite>().Where(Sprite => Sprite.name == SpriteName).ToList();
            if (Sprites != null && Sprites.Count > 0) {
                return Sprites[0];
            } else {
                return null;
            }
        }
    }
}
