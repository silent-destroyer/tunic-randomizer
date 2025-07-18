﻿using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
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
        public static GameObject FuseAltLights;

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
        public static GameObject Chalkboard;
        public static GameObject Torch;

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
            Items["Dath Stone"].GetComponent<MeshRenderer>().material.color = new UnityEngine.Color(1, 1, 1, 1);
            Items["Dath Stone"].SetActive(false);
            Items["Dath Stone"].name = "dath stone";
            GameObject.DontDestroyOnLoad(Items["Dath Stone"]);

            Signpost = new GameObject("signpost");
            Signpost.AddComponent<MeshFilter>().mesh = Resources.FindObjectsOfTypeAll<Mesh>().Where(mesh => mesh.name == "signpost pointing right").First();
            Signpost.AddComponent<MeshRenderer>().material = FindMaterial("signpost");
            Signpost.SetActive(false);
            GameObject.DontDestroyOnLoad(Signpost);
            
            InitializeExtras();

            ItemPresentationPatches.SetupOldHouseKeyItemPresentation();
            Items["Key (House)"] = ItemRoot.transform.GetChild(48).gameObject;
            ItemPresentationPatches.SetupTorchItemPresentation();
            ItemPresentationPatches.SetupDathStoneItemPresentation();
            ItemPresentationPatches.SetupHexagonQuestItemPresentation();
            ItemPresentationPatches.SetupCapePresentation();
            ItemPresentationPatches.SetupGrassPresentation();

            ArachnophobiaMode.GetRuneSprites();

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
            Cube.AddComponent<MeshRenderer>().materials = Items["Hexagon Gold"].GetComponent<MeshRenderer>().materials;
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

            if (TunicRandomizer.Settings.ShowItemsEnabled && SceneLoaderPatches.SceneName == "Shop") {
                SetupShopItems();
            } else {
                if (TunicRandomizer.Settings.ShowItemsEnabled) {
                    foreach (ItemPickup ItemPickup in Resources.FindObjectsOfTypeAll<ItemPickup>()) {
                        if (ItemPickup != null && ItemPickup.itemToGive != null) {
                            string checkId = $"{ItemPickup.itemToGive.name} [{SceneLoaderPatches.SceneName}]";
                            if(ItemLookup.ItemList.ContainsKey(checkId) || Locations.RandomizedLocations.ContainsKey(checkId)) {
                                if (SwappedThisSceneAlready && !IsSwordCheck(checkId)) {
                                    continue;
                                }
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
                            if (SwappedThisSceneAlready && !IsSwordCheck(ItemId)) {
                                continue;
                            }
                            SetupPagePickup(PagePickup);
                        }
                    }

                    foreach (HeroRelicPickup HeroRelicPickup in Resources.FindObjectsOfTypeAll<HeroRelicPickup>()) {
                        SetupHeroRelicPickup(HeroRelicPickup);
                    }
                    if (SceneLoaderPatches.SceneName == "Fortress Arena") {
                        if (!SwappedThisSceneAlready || IsSwordCheck("Vault Key (Red) [Fortress Arena]")) {
                            SwapSiegeEngineCrown();
                        }
                    }
                }
                if (TunicRandomizer.Settings.ChestsMatchContentsEnabled) {
                    foreach (Chest Chest in Resources.FindObjectsOfTypeAll<Chest>()) {
                        if (SwappedThisSceneAlready && !IsSwordCheck(ItemPatches.GetChestRewardID(Chest))) {
                            continue;
                        }
                        ApplyChestTexture(Chest);
                    }
                    if (SaveFile.GetInt(GrassRandoEnabled) == 1) {
                        foreach(Grass grass in Resources.FindObjectsOfTypeAll<Grass>()) {
                            string grassId = GrassRandomizer.getGrassGameObjectId(grass);
                            if (Locations.RandomizedLocations.ContainsKey(grassId) || ItemLookup.ItemList.ContainsKey(grassId)) {
                                if (SwappedThisSceneAlready && !IsSwordCheck(grassId)) {
                                    continue;
                                }
                                ApplyGrassTexture(grass);
                            }
                        }
                    }
                    if (SaveFile.GetInt(FuseShuffleEnabled) == 1) { 
                        foreach (Fuse fuse in Resources.FindObjectsOfTypeAll<Fuse>().Where(fuse => fuse.gameObject.scene.name == SceneLoaderPatches.SceneName)) {
                            string fuseId = FuseRandomizer.GetFuseCheckId(fuse);
                            if (Locations.RandomizedLocations.ContainsKey(fuseId) || ItemLookup.ItemList.ContainsKey(fuseId)) {
                                if (SwappedThisSceneAlready && !IsSwordCheck(fuseId)) {
                                    continue;
                                }
                                ApplyFuseTexture(fuse);
                            }
                        }
                    }
                    if (SaveFile.GetInt(BreakableShuffleEnabled) == 1) {
                        foreach (SmashableObject breakableObject in Resources.FindObjectsOfTypeAll<SmashableObject>()) {
                            string breakableId = BreakableShuffle.getBreakableGameObjectId(breakableObject.gameObject);
                            if (Locations.RandomizedLocations.ContainsKey(breakableId) || ItemLookup.ItemList.ContainsKey(breakableId)) {
                                if (SwappedThisSceneAlready && !IsSwordCheck(breakableId)) {
                                    continue;
                                }
                                ApplyBreakableTexture(breakableObject.gameObject);
                                breakableObject.maxCoinDrop = 0;
                                breakableObject.minCoinDrop = 0;
                            }
                        }
                        foreach (SecretPassagePanel bombWall in Resources.FindObjectsOfTypeAll<SecretPassagePanel>()) {
                            string breakableId = BreakableShuffle.getBreakableGameObjectId(bombWall.gameObject);
                            if (Locations.RandomizedLocations.ContainsKey(breakableId) || ItemLookup.ItemList.ContainsKey(breakableId)) {
                                if (SwappedThisSceneAlready && !IsSwordCheck(breakableId)) {
                                    continue;
                                }
                                ApplyBreakableTexture(bombWall.gameObject);
                            }
                        }
                        if (SceneManager.GetActiveScene().name == "Dusty") {
                            foreach (DustyPile leafPile in Resources.FindObjectsOfTypeAll<DustyPile>()) {
                                string leafId = BreakableShuffle.getBreakableGameObjectId(leafPile.gameObject);
                                if (Locations.RandomizedLocations.ContainsKey(leafId) || ItemLookup.ItemList.ContainsKey(leafId)) {
                                    if ((SwappedThisSceneAlready && !IsSwordCheck(leafId)) || Locations.CheckedLocations[leafId]) {
                                        continue;
                                    }
                                    ApplyBreakableTexture(leafPile.gameObject);
                                }
                            }
                        }
                    }
                    if (GetBool(BellShuffleEnabled)) {
                        TuningForkBell bell = GameObject.FindObjectOfType<TuningForkBell>();
                        if (bell != null && bell.gameObject.scene.name == SceneLoaderPatches.SceneName) {
                            string bellCheckId = BellShuffle.GetBellCheckId(bell);
                            if (Locations.RandomizedLocations.ContainsKey(bellCheckId) || ItemLookup.ItemList.ContainsKey(bellCheckId)) {
                                if ((SwappedThisSceneAlready && !IsSwordCheck(bellCheckId))) {
                                    
                                } else {
                                    ApplyBellTexture(bell);
                                }
                            }
                        }
                    }
                }
            }
            ItemLookup.Items["Fool Trap"].QuantityToGive = 1;
            SwappedThisSceneAlready = true;
        }

        private static bool IsSwordCheck(string CheckId) {
            if (IsSinglePlayer() && Locations.RandomizedLocations.ContainsKey(CheckId)) {
                return ItemLookup.GetItemDataFromCheck(Locations.RandomizedLocations[CheckId]).Type == ItemTypes.SWORDUPGRADE;
            }
            if (IsArchipelago() && ItemLookup.ItemList.ContainsKey(CheckId)) {
                ItemInfo itemInfo = ItemLookup.ItemList[CheckId];
                return itemInfo.ItemGame == "TUNIC" && ItemLookup.Items[itemInfo.ItemDisplayName].Type == ItemTypes.SWORDUPGRADE;
            }
            return false;
        }

        public static void ApplyChestTexture(Chest Chest) {
            if (Chest != null) {
                string ItemId = ItemPatches.GetChestRewardID(Chest);
                string ItemName = "Stick";
                if (IsArchipelago() && ItemLookup.ItemList.ContainsKey(ItemId)) {
                    ItemInfo APItem = ItemLookup.ItemList[ItemId];
                    ItemName = APItem.ItemDisplayName;
                    if (!Archipelago.instance.IsTunicPlayer(APItem.Player) || !ItemLookup.Items.ContainsKey(ItemName)) {
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

        public static void ApplyAPChestTexture(Chest chest, ItemInfo APItem) {
            GameObject ChestTop = new GameObject("sprite");
            ChestTop.transform.parent = chest.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
            ChestTop.AddComponent<SpriteRenderer>().sprite = FindSprite("trinkets 1_slot_grey");
            ChestTop.transform.localPosition = new Vector3(0f, 0.1f, 1.2f);
            ChestTop.transform.localEulerAngles = new Vector3(57f, 180f, 0f);
            ChestTop.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            ItemFlags flag = APItem.Flags;
            int randomFlag = new System.Random().Next(3);

            if (flag == ItemFlags.None || (flag == ItemFlags.Trap && randomFlag == 0)) {
                chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials[0].color = new UnityEngine.Color(0f, 0.75f, 0f, 1f);
            }
            if(flag.HasFlag(ItemFlags.NeverExclude) || (flag == ItemFlags.Trap && randomFlag == 1)) {
                chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials[0].color = new UnityEngine.Color(0f, 0.5f, 0.75f, 1f);
            } 
            if(flag.HasFlag(ItemFlags.Advancement) || (flag == ItemFlags.Trap && randomFlag == 2)) {
                chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = new Material[] {
                    ModelSwaps.Items["Hexagon Gold"].GetComponent<MeshRenderer>().material,
                    chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials[1]
                };
                if (flag.HasFlag(ItemFlags.Advancement) && flag.HasFlag(ItemFlags.NeverExclude)) {
                    ChestTop.GetComponent<SpriteRenderer>().color = UnityEngine.Color.cyan;
                    ChestTop.GetComponent<SpriteRenderer>().material.color = UnityEngine.Color.cyan;
                }
            }
            
            if(APItem.Flags.HasFlag(ItemFlags.Trap)) {
                ChestTop.transform.localEulerAngles = new Vector3(57f, 180f, 180f);
            }
        }

        public static void ApplyGrassTexture(Grass grass) {
            string GrassId = $"{grass.name}~{grass.transform.position.ToString()} [{grass.gameObject.scene.name}]";
            if(Locations.RandomizedLocations.ContainsKey(GrassId) || ItemLookup.ItemList.ContainsKey(GrassId)) {
                ItemData Item = ItemLookup.Items["Money x1"];
                if (IsSinglePlayer()) {
                    Check check = Locations.RandomizedLocations[GrassId];
                    Item = ItemLookup.GetItemDataFromCheck(check);
                    if (!check.IsCompletedOrCollected) {
                        SetupItemMoveUp(grass.transform, check: check);
                    }
                } else if (IsArchipelago()) {
                    ItemInfo itemInfo = ItemLookup.ItemList[GrassId];
                    if (!TunicUtils.IsCheckCompletedInAP(GrassId)) {
                        SetupItemMoveUp(grass.transform, itemInfo: itemInfo);
                    }
                    if (!Archipelago.instance.IsTunicPlayer(itemInfo.Player) || !ItemLookup.Items.ContainsKey(itemInfo.ItemDisplayName)) {
                        ApplyAPGrassTexture(grass, itemInfo, Locations.CheckedLocations[GrassId] || (TunicRandomizer.Settings.CollectReflectsInWorld && SaveFile.GetInt($"randomizer {GrassId} was collected") == 1));
                        return;
                    }
                    Item = ItemLookup.Items[itemInfo.ItemDisplayName];
                }
                if (Item.Type == ItemTypes.GRASS) {
                    return;
                }
                Material material = GetMaterialType(Item);

                if (Item.Name == "Fool Trap") {
                    foreach (Transform child in grass.GetComponentsInChildren<Transform>()) {
                        if (child.name == grass.name) { continue; }
                        child.localEulerAngles = new Vector3(180, 0, 0);
                        child.position += new Vector3(0, 2, 0);
                    }
                }

                if (material != null) {
                    foreach(MeshRenderer r in grass.GetComponentsInChildren<MeshRenderer>(includeInactive: true)) {
                        if (r.gameObject.GetComponent<MoveUp>() != null || r.gameObject.GetComponentInParent<MoveUp>(includeInactive: true) != null) { continue; }
                        r.material = material;
                    }
                }
            }
        }

        public static void ApplyAPGrassTexture(Grass grass, ItemInfo itemInfo, bool Checked) {
            GameObject questionMark = new GameObject("question mark");
            for(int i = 0; i < grass.transform.GetChild(1).childCount; i++) {
                GameObject.Destroy(grass.transform.GetChild(1).GetChild(i).gameObject);
            }
            questionMark.transform.parent = grass.transform.GetChild(1);
            questionMark.AddComponent<SpriteRenderer>().sprite = FindSprite("trinkets 1_slot_grey");
            if (grass.name.Contains("bush")) {
                questionMark.transform.localPosition = new Vector3(0f, 1.7709f, 0f);
                questionMark.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
                questionMark.transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);
            } else {
                questionMark.transform.localPosition = new Vector3(0f, 1.9f, 0f);
                questionMark.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
                questionMark.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            }
            ItemFlags flag = itemInfo.Flags;
            int randomFlag = new System.Random().Next(3);
            UnityEngine.Color color;
            if (flag == ItemFlags.None || (flag == ItemFlags.Trap && randomFlag == 0)) {
                foreach (MeshRenderer r in grass.GetComponentsInChildren<MeshRenderer>(includeInactive: true)) {
                    r.material.color = new UnityEngine.Color(0f, 0.75f, 0f, 1f);
                    grass.materialForBase = r.material;
                }
            }
            if (flag.HasFlag(ItemFlags.NeverExclude) || (flag == ItemFlags.Trap && randomFlag == 1)) {
                foreach (MeshRenderer r in grass.GetComponentsInChildren<MeshRenderer>(includeInactive: true)) {
                    r.material.color = color = new UnityEngine.Color(0f, 0.5f, 0.75f, 1f);
                    grass.materialForBase = r.material;
                }
            }
            if (flag.HasFlag(ItemFlags.Advancement) || (flag == ItemFlags.Trap && randomFlag == 2)) {
                foreach (MeshRenderer r in grass.GetComponentsInChildren<MeshRenderer>(includeInactive: true)) {
                    r.material = Items["Hexagon Gold"].GetComponent<MeshRenderer>().material;
                    grass.materialForBase = r.material;
                }
                if (flag.HasFlag(ItemFlags.Advancement) && flag.HasFlag(ItemFlags.NeverExclude)) {
                    questionMark.GetComponent<SpriteRenderer>().color = UnityEngine.Color.cyan;
                    questionMark.GetComponent<SpriteRenderer>().material.color = UnityEngine.Color.cyan;
                }
            }

            if (itemInfo.Flags.HasFlag(ItemFlags.Trap)) {
                foreach (Transform child in grass.GetComponentsInChildren<Transform>()) {
                    if (child.name == grass.name) { continue; }
                    child.localEulerAngles = new Vector3(180, 0, 0);
                    child.position += new Vector3(0, 2, 0);
                }
                questionMark.transform.localEulerAngles = new Vector3(90, 180, 0);
            }
            questionMark.SetActive(!Checked);
        }

        public static void ApplyBreakableTexture(GameObject breakableObject) {
            string breakableId = BreakableShuffle.getBreakableGameObjectId(breakableObject);
            if (Locations.RandomizedLocations.ContainsKey(breakableId) || ItemLookup.ItemList.ContainsKey(breakableId)) {
                ItemData Item = ItemLookup.Items["Money x1"];
                if (IsSinglePlayer()) {
                    Check check = Locations.RandomizedLocations[breakableId];
                    Item = ItemLookup.GetItemDataFromCheck(check);
                    if (!check.IsCompletedOrCollected) { 
                        SetupItemMoveUp(breakableObject.transform, check: check);
                    }
                } else if (IsArchipelago()) {
                    ItemInfo itemInfo = ItemLookup.ItemList[breakableId];
                    if (!TunicUtils.IsCheckCompletedInAP(breakableId)) {
                        SetupItemMoveUp(breakableObject.transform, itemInfo: itemInfo);
                    }
                    if (!Archipelago.instance.IsTunicPlayer(itemInfo.Player) || !ItemLookup.Items.ContainsKey(itemInfo.ItemDisplayName)) {
                        ApplyAPBreakableTexture(breakableObject, itemInfo, Locations.CheckedLocations[breakableId] || (TunicRandomizer.Settings.CollectReflectsInWorld && SaveFile.GetInt($"randomizer {breakableId} was collected") == 1));
                        return;
                    }
                    Item = ItemLookup.Items[itemInfo.ItemDisplayName];
                }
                if (Item.Type == ItemTypes.GRASS) {
                    return;
                }
                
                Material material = GetMaterialType(Item);

                if (material != null) {
                    List<MeshRenderer> renderers;
                    if (breakableObject.name == "Physical Post") {
                        renderers = breakableObject.transform.parent.GetComponentsInChildren<MeshRenderer>(includeInactive: true).ToList();
                    } else {
                        renderers = breakableObject.GetComponentsInChildren<MeshRenderer>(includeInactive: true).ToList();
                    }
                    foreach (MeshRenderer r in renderers) {
                        if (r.name == "cathedral_candles_single" || r.name == "cathedral_candleflame" || r.name == "library_lab_pageBottle_glass") { continue; }
                        if (r.gameObject.GetComponent<MoveUp>() != null || r.gameObject.GetComponentInParent<MoveUp>(includeInactive: true) != null) { continue; }
                        if (breakableObject.GetComponent<SecretPassagePanel>() != null) {
                            r.materials = new Material[] { material, material };
                        } else {
                            r.material = material;
                        }
                    }
                }
            }
        }

        public static void ApplyAPBreakableTexture(GameObject breakableObject, ItemInfo itemInfo, bool Checked) {
            GameObject questionMark = new GameObject("question mark");
            questionMark.transform.parent = breakableObject.transform;
            questionMark.AddComponent<SpriteRenderer>().sprite = FindSprite("trinkets 1_slot_grey");
            questionMark.transform.localPosition = new Vector3(0f, 1.7709f, 0f);
            questionMark.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
            questionMark.transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);

            ItemFlags flag = itemInfo.Flags;
            int randomFlag = new System.Random().Next(3);
            UnityEngine.Color color = new UnityEngine.Color();
            bool customColor = false;

            MeshRenderer meshRenderer = breakableObject.GetComponentInChildren<MeshRenderer>();
            // signs already look good without changing the material
            if (breakableObject.name != "Physical Post") {
                meshRenderer.material = Chests["Normal"].GetComponent<MeshRenderer>().material;
            }

            MeshFilter meshFilter = breakableObject.GetComponentInChildren<MeshFilter>();

            if (flag == ItemFlags.None || (flag == ItemFlags.Trap && randomFlag == 0)) {
                color = new UnityEngine.Color(0f, 0.75f, 0f, 1f);
                customColor = true;
            }
            if (flag.HasFlag(ItemFlags.NeverExclude) || (flag == ItemFlags.Trap && randomFlag == 1)) {
                color = new UnityEngine.Color(0f, 0.5f, 0.75f, 1f);
                customColor = true;
            }
            if (flag.HasFlag(ItemFlags.Advancement) || (flag == ItemFlags.Trap && randomFlag == 2)) {
                foreach (MeshRenderer r in breakableObject.GetComponentsInChildren<MeshRenderer>(includeInactive: true)) {
                    if (r.name == "cathedral_candles_single" || r.name == "cathedral_candleflame" || r.name == "library_lab_pageBottle_glass") { continue; }
                    r.material = Items["Hexagon Gold"].GetComponent<MeshRenderer>().material;
                }
                if (flag.HasFlag(ItemFlags.Advancement) && flag.HasFlag(ItemFlags.NeverExclude)) {
                    questionMark.GetComponent<SpriteRenderer>().color = UnityEngine.Color.cyan;
                    questionMark.GetComponent<SpriteRenderer>().material.color = UnityEngine.Color.cyan;
                }
            } else if (customColor) {
                Material outerMaterial = null;
                if (breakableObject.GetComponent<MeshRenderer>() != null) {
                    outerMaterial = breakableObject.GetComponent<MeshRenderer>().material;
                }
                foreach (MeshRenderer r in breakableObject.GetComponentsInChildren<MeshRenderer>(includeInactive: true)) {
                    if (r.name == "cathedral_candles_single" || r.name == "cathedral_candleflame" || r.name == "library_lab_pageBottle_glass") { continue; }
                    if (!r.transform.gameObject.active && outerMaterial != null) {
                        r.material = outerMaterial;
                    }
                    r.material.color = color;
                }
            }

            if (breakableObject.GetComponent<SecretPassagePanel>() != null) {
                meshRenderer.materials = new Material[] { meshRenderer.material, meshRenderer.material };
            }

            if (meshFilter != null) {
                if (meshFilter.mesh.name.Contains("sewer_barrel")) {
                    questionMark.transform.localPosition += new Vector3(0f, 0.9f, 0f);
                }
                if (meshFilter.mesh.name.Contains("crate")) {
                    questionMark.transform.localPosition += new Vector3(0f, 0.2f, 0f);
                }
            }
            
            if (itemInfo.Flags.HasFlag(ItemFlags.Trap)) {
                questionMark.transform.localEulerAngles = new Vector3(90, 180, 0);
            }
            
            if (breakableObject.name == "Physical Post") {
                questionMark.transform.localPosition = new Vector3(0f, 1f, 0.3f);
                questionMark.transform.localScale = Vector3.one * 0.2f;
                questionMark.transform.localEulerAngles = itemInfo.Flags.HasFlag(ItemFlags.Trap) ? new Vector3(0f, 0f, 180f) : Vector3.zero;
            }
            if (breakableObject.GetComponent<SecretPassagePanel>() != null 
                && ItemPositions.BreakablePositionExtras.ContainsKey(breakableObject.transform.position.ToString())) {
                questionMark.transform.localPosition = ItemPositions.BreakablePositionExtras[breakableObject.transform.position.ToString()].Item2;
                questionMark.transform.localEulerAngles = new Vector3(0, ItemPositions.BreakablePositionExtras[breakableObject.transform.position.ToString()].Item3, 0);
            }
            if (flag.HasFlag(ItemFlags.Trap)) {
                questionMark.transform.localEulerAngles += new Vector3(0, 0, 180);
            }
            questionMark.SetActive(!Checked);
        }

        public static void ApplyFuseTexture(Fuse fuse) {
            string fuseId = FuseRandomizer.GetFuseCheckId(fuse);
            if (Locations.RandomizedLocations.ContainsKey(fuseId) || ItemLookup.ItemList.ContainsKey(fuseId)) {
                ItemData Item = ItemLookup.Items["Money x1"];
                if (IsSinglePlayer()) {
                    Check check = Locations.RandomizedLocations[fuseId];
                    Item = ItemLookup.GetItemDataFromCheck(check);
                    if (!check.IsCompletedOrCollected) {
                        SetupItemMoveUp(fuse.transform, check: check);
                    }
                } else if (IsArchipelago()) {
                    ItemInfo itemInfo = ItemLookup.ItemList[fuseId];
                    if (!TunicUtils.IsCheckCompletedInAP(fuseId)) {
                        SetupItemMoveUp(fuse.transform, itemInfo: itemInfo);
                    }
                    if (!Archipelago.instance.IsTunicPlayer(itemInfo.Player) || !ItemLookup.Items.ContainsKey(itemInfo.ItemDisplayName)) {
                        ApplyAPFuseTexture(fuse, itemInfo, TunicUtils.IsCheckCompletedOrCollected(fuseId));
                        return;
                    }
                    Item = ItemLookup.Items[itemInfo.ItemDisplayName];
                }

                Material material = GetMaterialType(Item);

                if (Item.Name == "Fool Trap") {
                    doFuseFoolTrapSetup(fuse);
                }

                if (material != null) {
                    foreach (MeshRenderer r in fuse.transform.GetChild(0).GetChild(0).GetComponentsInChildren<MeshRenderer>(includeInactive: true)) {
                        if (r.gameObject.GetComponent<MoveUp>() != null || r.gameObject.GetComponentInParent<MoveUp>() != null) { continue; }
                        if (r.name.Contains("omnifuse corner mid") || r.name.Contains("omnifuse corner top") || r.name.Contains("omnifuse slide")) {
                            r.material = material;
                        }
                    }
                }
            }
        }
        
        public static void ApplyAPFuseTexture(Fuse fuse, ItemInfo itemInfo, bool Checked) {
            List<MeshRenderer> renderers = new List<MeshRenderer>();
            foreach (MeshRenderer r in fuse.transform.GetChild(0).GetChild(0).GetComponentsInChildren<MeshRenderer>(includeInactive: true)) {
                if (r.gameObject.GetComponent<MoveUp>() != null || r.gameObject.GetComponentInParent<MoveUp>() != null) { continue; }
                if (r.name.Contains("omnifuse corner mid") || r.name.Contains("omnifuse corner top") || r.name.Contains("omnifuse slide")) {
                    renderers.Add(r);
                }
            }
            ItemFlags flag = itemInfo.Flags;
            int randomFlag = new System.Random().Next(3);
            Material material = FindMaterial("treasure chest trim (Instance) (Instance)");
            UnityEngine.Color color;
            if (flag == ItemFlags.None || (flag == ItemFlags.Trap && randomFlag == 0)) {
                foreach (MeshRenderer r in renderers) {
                    r.material = material;
                    r.material.color = new UnityEngine.Color(0f, 0.75f, 0f, 1f);
                }
            }
            if (flag.HasFlag(ItemFlags.NeverExclude) || (flag == ItemFlags.Trap && randomFlag == 1)) {
                foreach (MeshRenderer r in renderers) {
                    r.material = material;
                    r.material.color = color = new UnityEngine.Color(0f, 0.5f, 0.75f, 1f);
                }
            }
            if (flag.HasFlag(ItemFlags.Advancement) || (flag == ItemFlags.Trap && randomFlag == 2)) {
                foreach (MeshRenderer r in renderers) {
                    r.material = Items["Hexagon Gold"].GetComponent<MeshRenderer>().material;
                    if (r.name.Contains("omnifuse corner top") && flag.HasFlag(ItemFlags.Advancement) && flag.HasFlag(ItemFlags.NeverExclude)) {
                        r.material = material;
                        r.material.color = UnityEngine.Color.cyan;
                    }
                }
            }
            if (itemInfo.Flags.HasFlag(ItemFlags.Trap)) {
                doFuseFoolTrapSetup(fuse);
            }
        }

        private static void doFuseFoolTrapSetup(Fuse fuse) {
            // Flip lights upside down and change from rgb -> cmy
            UVScroller uvScroller = fuse.GetComponentInChildren<UVScroller>(true);
            if (uvScroller != null) {
                uvScroller.gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);
                uvScroller.gameObject.transform.localPosition = new Vector3(0, 17.4809f, 0);
                uvScroller.gameObject.AddComponent<FuseTrapAppearanceHelper>();
                uvScroller.gameObject.GetComponent<FuseTrapAppearanceHelper>().UVScroller = uvScroller;
            }
        }

        public static void ApplyBellTexture(TuningForkBell bell) {
            string bellId = BellShuffle.GetBellCheckId(bell);
            if (Locations.RandomizedLocations.ContainsKey(bellId) || ItemLookup.ItemList.ContainsKey(bellId)) {
                ItemData Item = ItemLookup.Items["Money x1"];
                if (IsSinglePlayer()) {
                    Check check = Locations.RandomizedLocations[bellId];
                    Item = ItemLookup.GetItemDataFromCheck(check);
                } else if (IsArchipelago()) {
                    ItemInfo itemInfo = ItemLookup.ItemList[bellId];
                    if (!Archipelago.instance.IsTunicPlayer(itemInfo.Player) || !ItemLookup.Items.ContainsKey(itemInfo.ItemDisplayName)) {
                        ApplyAPBellTexture(bell, itemInfo, TunicUtils.IsCheckCompletedOrCollected(bellId));
                        return;
                    }
                    Item = ItemLookup.Items[itemInfo.ItemDisplayName];
                }

                Material material = GetMaterialType(Item);

                if (Item.Name == "Fool Trap") {
                    bell.transform.GetChild(1).localEulerAngles = new Vector3(180, 0, 0);
                    bell.transform.GetChild(1).localPosition = new Vector3(0, 11, 0);
                    bell.transform.GetChild(2).localPosition = new Vector3(0, 6, 0);
                }

                if (material != null) { 
                    bell.transform.GetChild(1).GetComponent<MeshRenderer>().material = material;
                    bell.transform.GetChild(2).GetComponent<MeshRenderer>().material = material;
                    bell.transform.GetChild(2).GetChild(0).GetComponent<MeshRenderer>().material = material;
                }
            }
        }

        public static void ApplyAPBellTexture(TuningForkBell bell, ItemInfo itemInfo, bool Checked) {
            List<MeshRenderer> renderers = new List<MeshRenderer>();
            renderers.Add(bell.transform.GetChild(1).GetComponent<MeshRenderer>());
            renderers.Add(bell.transform.GetChild(2).GetComponent<MeshRenderer>());
            renderers.Add(bell.transform.GetChild(2).GetChild(0).GetComponent<MeshRenderer>());

            ItemFlags flag = itemInfo.Flags;
            int randomFlag = new System.Random().Next(3);
            UnityEngine.Color color;
            if (flag == ItemFlags.None || (flag == ItemFlags.Trap && randomFlag == 0)) {
                foreach (MeshRenderer r in renderers) {
                    r.material.color = new UnityEngine.Color(0f, 0.85f, 0f, 1f);
                }
            }
            if (flag.HasFlag(ItemFlags.NeverExclude) || (flag == ItemFlags.Trap && randomFlag == 1)) {
                foreach (MeshRenderer r in renderers) {
                    r.material.color = color = new UnityEngine.Color(0f, 0.5f, 1f, 1f);
                }
            }
            if (flag.HasFlag(ItemFlags.Advancement) || (flag == ItemFlags.Trap && randomFlag == 2)) {
                foreach (MeshRenderer r in renderers) {
                    if (r.name.Contains("gyro") && flag.HasFlag(ItemFlags.Advancement) && flag.HasFlag(ItemFlags.NeverExclude)) {
                        r.material.color = UnityEngine.Color.cyan;
                    } else {
                        r.material = Items["Hexagon Gold"].GetComponent<MeshRenderer>().material;
                    }
                }
            }

            GameObject questionMark = new GameObject("question mark");
            questionMark.transform.parent = bell.transform;
            questionMark.AddComponent<SpriteRenderer>().sprite = FindSprite("trinkets 1_slot_grey");
            questionMark.transform.localPosition = new Vector3(0f, 0.8f, 1f);
            questionMark.transform.localEulerAngles = new Vector3(45f, 180f, 0f);
            questionMark.transform.localScale = Vector3.one * 0.25f;

            if (itemInfo.Flags.HasFlag(ItemFlags.Trap)) {
                bell.transform.GetChild(1).localEulerAngles = new Vector3(180, 0, 0);
                bell.transform.GetChild(1).localPosition = new Vector3(0, 11, 0);
                bell.transform.GetChild(2).localPosition = new Vector3(0, 6, 0);
                questionMark.transform.localEulerAngles = new Vector3(45f, 180f, 180f);
            }

            questionMark.SetActive(!Checked);
        }

        private static Material GetMaterialType(ItemData Item) {
            Material material = null;
            if (Item.Type == ItemTypes.FAIRY) {
                material = Chests["Fairy"].GetComponent<MeshRenderer>().material;
            } else if (Item.Type == ItemTypes.GOLDENTROPHY) {
                material = Chests["GoldenTrophy"].GetComponent<MeshRenderer>().material;
            } else if (Item.ItemNameForInventory == "Hyperdash") {
                material = Chests["Hyperdash"].GetComponent<MeshRenderer>().material;
            } else if (Item.ItemNameForInventory.Contains("Hexagon") && Item.Type != ItemTypes.HEXAGONQUEST) {
                material = Items[Item.ItemNameForInventory].GetComponent<MeshRenderer>().material;
            }
            return material;
        }

        public static void SetupItemMoveUp(Transform transform, Check check = null, ItemInfo itemInfo = null) {
            if (check == null && itemInfo == null) { return; }

            if (transform.GetComponentInChildren<MoveUp>(true) != null && !transform.GetComponentInChildren<MoveUp>(true).gameObject.active) {
                GameObject.Destroy(transform.GetComponentInChildren<MoveUp>(true).gameObject);
            }

            ItemData Item = null;

            if (check != null) {
                Item = ItemLookup.GetItemDataFromCheck(check);
            } else if (itemInfo != null && Archipelago.instance.IsTunicPlayer(itemInfo.Player) && ItemLookup.Items.ContainsKey(itemInfo.ItemDisplayName)) {
                Item = ItemLookup.Items[itemInfo.ItemDisplayName];
            }
            if (transform.GetComponent<Grass>() != null && Item != null && Item.Type == ItemTypes.GRASS) {
                return;
            }
            if (transform.GetComponent<Chest>() != null && Item != null && Item.Type == ItemTypes.FAIRY) {
                return;
            }

            GameObject moveUp = SetupItemBase(transform, itemInfo, check);
            TransformData TransformData;
            if (IsArchipelago() && Item == null && itemInfo != null && !Archipelago.instance.IsTunicPlayer(itemInfo.Player)) {
                TransformData = ItemPositions.Techbow["Other World"];
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
                } else if (Item.Type == ItemTypes.SWORDUPGRADE && (SaveFile.GetInt(SwordProgressionEnabled) == 1) && (IsSinglePlayer() || itemInfo.Player == Archipelago.instance.GetPlayerSlot())) {
                    int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                    TransformData = ItemPositions.Techbow.ContainsKey($"Sword Progression {SwordLevel}") ? ItemPositions.Techbow[$"Sword Progression {SwordLevel}"] : ItemPositions.Techbow[Item.ItemNameForInventory];
                } else {
                    TransformData = ItemPositions.Techbow.ContainsKey(Item.ItemNameForInventory) ? ItemPositions.Techbow[Item.ItemNameForInventory] : ItemPositions.Techbow[Enum.GetName(typeof(ItemTypes), Item.Type)];
                    if (transform.GetComponent<Grass>() != null) {
                        TransformData.rot = new Quaternion(0, 0.9239f, 0, -0.3827f);
                    }
                }
            }

            moveUp.transform.localPosition = TransformData.pos;
            moveUp.transform.localRotation = TransformData.rot;
            if (transform.GetComponent<Chest>() != null || transform.GetComponent<TrinketWell>() != null) {
                moveUp.transform.localPosition += new Vector3(0, 0.5f, 0);
                if (Item != null) {
                    if (Item.Name == "Just Some Pals") {
                        moveUp.transform.localEulerAngles += new Vector3(0, 90, 0);
                    } else if (Item.Type == ItemTypes.TRINKET) {
                        moveUp.transform.localEulerAngles += new Vector3(0, 135, 0);
                    }
                }
            }

            moveUp.transform.localScale = TransformData.scale;
            moveUp.transform.localPosition += new Vector3(0, 0.5f, 0);

            moveUp.layer = 0;
            moveUp.AddComponent<DestroyAfterTime>().lifetime = 2f;
            moveUp.AddComponent<MoveUp>().speed = 0.5f;
            moveUp.SetActive(transform.GetComponent<Chest>() != null || transform.GetComponent<TrinketWell>() != null);

            if (transform.GetComponent<SmashableObject>() != null || transform.GetComponent<DustyPile>() != null || transform.GetComponent<SecretPassagePanel>() != null) {
                moveUp.transform.parent = transform;
                // so we can rotate it properly
                if (Item != null && Item.Type == ItemTypes.TRINKET) {
                    moveUp.name = "Card";
                }
                if (transform.name == "Physical Post") {
                    moveUp.transform.localScale *= 0.66f;
                }
                if (transform.GetComponent<SecretPassagePanel>() != null && ItemPositions.BreakablePositionExtras.ContainsKey(transform.position.ToString())) {
                    moveUp.transform.localPosition = ItemPositions.BreakablePositionExtras[transform.position.ToString()].Item1;
                }
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
                            if (ItemPickup.gameObject.scene.name == "Fortress Arena" && ItemPickup.itemToGive.name == "Vault Key (Red)") {
                                continue;
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

                ItemInfo ApItem = null;
                Check Check = null;
                ItemData ItemData =  null;

                if (ItemLookup.ItemList.ContainsKey(ItemId) || Locations.RandomizedLocations.ContainsKey(ItemId)) { 
                    if (IsArchipelago()) {
                        ApItem = ItemLookup.ItemList[ItemId];
                        if (Archipelago.instance.IsTunicPlayer(ApItem.Player)) {
                            ItemData = ItemLookup.Items[ApItem.ItemDisplayName];
                        }
                    }
                    if (IsSinglePlayer()) {
                        Check = Locations.RandomizedLocations[ItemId];
                        ItemData = ItemLookup.GetItemDataFromCheck(Check);
                    }

                    if (Locations.CheckedLocations[ItemId] || SaveFile.GetInt($"{ItemPatches.SaveFileCollectedKey} {ItemId}") == 1) {
                        GameObject.Destroy(ItemPickup.gameObject);
                        return;
                    }

                    if (ItemData != null && ItemData.ItemNameForInventory == ItemPickup.itemToGive.name) {
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
                        if (IsArchipelago() && ItemData == null && (ApItem != null && !Archipelago.instance.IsTunicPlayer(ApItem.Player) || !ItemLookup.Items.ContainsKey(ApItem.ItemDisplayName))) {
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

                ItemInfo ApItem = null;
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
                            Item = ItemLookup.Items[ApItem.ItemDisplayName];
                            if (ItemLookup.Items.ContainsKey(ApItem.ItemDisplayName) && ItemLookup.Items[ApItem.ItemDisplayName].Type == ItemTypes.PAGE) {
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
                    if (IsArchipelago() && Item == null && (ApItem != null && !Archipelago.instance.IsTunicPlayer(ApItem.Player) || !ItemLookup.Items.ContainsKey(ApItem.ItemDisplayName))) {
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
                        if (Item.Type == ItemTypes.FUSE) {
                            PagePickup.GetComponent<SphereCollider>().radius = 2f;
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


        public static GameObject SetupItemBase(Transform Parent, ItemInfo APItem = null, Check Check = null) {
            GameObject NewItem;
            if (IsArchipelago() && (!Archipelago.instance.IsTunicPlayer(APItem.Player) || !ItemLookup.Items.ContainsKey(APItem.ItemDisplayName))) {
                ItemFlags flag = ItemFlags.None;
                if (APItem.Flags == ItemFlags.Trap) {
                    flag = new List<ItemFlags>() { ItemFlags.Advancement, ItemFlags.NeverExclude, ItemFlags.None}[new System.Random().Next(3)];
                }
                if (APItem.Flags.HasFlag(ItemFlags.NeverExclude)) {
                    flag = ItemFlags.NeverExclude;
                }
                if (APItem.Flags.HasFlag(ItemFlags.Advancement)) {
                    flag = ItemFlags.Advancement;
                }
                NewItem = GameObject.Instantiate(Items[$"Other World {flag}"], Parent.transform.position, Parent.transform.rotation);
                if (APItem.Flags.HasFlag(ItemFlags.Trap)) {
                    for (int i = 2; i < 6; i++) {
                        Vector3 flipped = NewItem.transform.GetChild(i).localEulerAngles;
                        NewItem.transform.GetChild(i).gameObject.transform.localEulerAngles = new Vector3(180, flipped.y, flipped.z);
                    }
                }
                if (APItem.Flags.HasFlag(ItemFlags.Advancement) && APItem.Flags.HasFlag(ItemFlags.NeverExclude)) {
                    foreach(SpriteRenderer sp in NewItem.GetComponentsInChildren<SpriteRenderer>()) {
                        sp.color = UnityEngine.Color.cyan;
                        sp.material.color = UnityEngine.Color.cyan;
                    }
                }
            } else {
                ItemData Item = ItemLookup.Items["Stick"];
                if (IsArchipelago() && APItem != null) {
                    Item = ItemLookup.Items[APItem.ItemDisplayName];
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
                } else if (Item.Type == ItemTypes.FUSE) {
                    NewItem = GameObject.Instantiate(Items["Fuse"], Parent.transform.position, Parent.transform.rotation);
                    NewItem.transform.GetChild(1).gameObject.SetActive(false);
                    NewItem.transform.GetChild(3).gameObject.SetActive(false);
                } else if (Item.Type == ItemTypes.BELL) {
                    NewItem = GameObject.Instantiate(Items["Bell"], Parent.transform.position, Parent.transform.rotation);
                    NewItem.transform.GetChild(3).gameObject.SetActive(false);
                    GameObject.Destroy(NewItem.GetComponent<TuningForkBell>());
                    GameObject.Destroy(NewItem.GetComponent<SphereCollider>());
                    GameObject.Destroy(NewItem.GetComponent<BoxCollider>());
                } else {
                    NewItem = GameObject.Instantiate(Items[Item.ItemNameForInventory], Parent.transform.position, Parent.transform.rotation);
                }
            }

            NewItem.transform.parent = Parent.transform;
            NewItem.layer = 0;
            foreach (Transform transform in NewItem.GetComponentsInChildren<Transform>(true)) {
                transform.gameObject.layer = 0;
            }
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

            ItemInfo ApItem = null;
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
                        HexagonItem = ItemLookup.Items[ApItem.ItemDisplayName];
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
                if (IsArchipelago() && HexagonItem == null && (ApItem != null && !Archipelago.instance.IsTunicPlayer(ApItem.Player) || !ItemLookup.Items.ContainsKey(ApItem.ItemDisplayName))) {
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

            ItemInfo ApItem = null;
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
                        HexagonItem = ItemLookup.Items[ApItem.ItemDisplayName];
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
                if (IsArchipelago() && HexagonItem == null && (ApItem != null && !Archipelago.instance.IsTunicPlayer(ApItem.Player) || !ItemLookup.Items.ContainsKey(ApItem.ItemDisplayName))) {
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
            if (VaultKey != null) {
                ItemInfo ApItem = null;
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
                            VaultKeyItem = ItemLookup.Items[ApItem.ItemDisplayName];
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
                    if (IsArchipelago() && VaultKeyItem == null && (ApItem != null && !Archipelago.instance.IsTunicPlayer(ApItem.Player) || !ItemLookup.Items.ContainsKey(ApItem.ItemDisplayName))) {
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


        }

        public static void SetupShopItems() {

            for (int i = 0; i < ShopItemIDs.Count; i++) {

                ItemInfo ApItem = null;
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
                            Item = ItemLookup.Items[ApItem.ItemDisplayName];
                        }
                    }
                    if (IsSinglePlayer()) {
                        Check = Locations.RandomizedLocations[ShopItemIDs[i]];
                        Item = ItemLookup.GetItemDataFromCheck(Check);
                    }

                    NewItem = SetupItemBase(ItemHolder.transform, ApItem, Check);

                    TransformData TransformData;
                    if (IsArchipelago() && Item == null && (ApItem != null && !Archipelago.instance.IsTunicPlayer(ApItem.Player) || !ItemLookup.Items.ContainsKey(ApItem.ItemDisplayName))) {
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

                ItemInfo ApItem = null;
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
                        Item = ItemLookup.Items[ApItem.ItemDisplayName];
                    }
                }
                if (IsSinglePlayer()) {
                    Check = Locations.RandomizedLocations[ItemId];
                    Item = ItemLookup.GetItemDataFromCheck(Check);
                }
                GameObject NewItem = SetupItemBase(HeroRelicPickup.transform, ApItem, Check);

                if (IsArchipelago() && Item == null && (ApItem != null && !Archipelago.instance.IsTunicPlayer(ApItem.Player) || !ItemLookup.Items.ContainsKey(ApItem.ItemDisplayName))) {
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
                    if (Item.Type == ItemTypes.LADDER || Item.Type == ItemTypes.FUSE || Item.Type == ItemTypes.BELL) {
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
                TunicLogger.LogError("Failed to create permanent ice bomb and/or pepper items in the shop.");
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
                                    || (IsArchipelago() && ItemLookup.ItemList[$"{shopItem.name} [Shop]"].ItemDisplayName.Contains("Ice Bomb") && Archipelago.instance.IsTunicPlayer(ItemLookup.ItemList[$"{shopItem.name} [Shop]"].Player)))) {
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

        public static void CreateChalkboard() {
            Chalkboard = GameObject.Instantiate(UnderConstruction);
            Chalkboard.name = "chalkboard";
            Chalkboard.GetComponent<MeshFilter>().mesh = MeshData.CreateMesh(MeshData.Chalkboard);
            Chalkboard.GetComponent<MeshRenderer>().materials = GameObject.Find("chalkboard (3)").GetComponent<MeshRenderer>().materials;
            Chalkboard.GetComponent<BoxCollider>().size = new Vector3(8, 4, 3);
            Chalkboard.GetComponent<BoxCollider>().center = Vector3.zero;
            Chalkboard.GetComponent<SphereCollider>().center = Vector3.zero;
            Chalkboard.GetComponent<SphereCollider>().radius = 4;
            Chalkboard.GetComponent<Signpost>().message = ScriptableObject.CreateInstance<LanguageLine>();
            Chalkboard.transform.GetChild(0).localPosition = new Vector3(0, -3, 0);
            Chalkboard.transform.localScale *= 0.75f;

            GameObject diagram = new GameObject("diagram");
            diagram.transform.parent = Chalkboard.transform;
            diagram.transform.localPosition = new Vector3(0.1067f, -0.126f, -0.2497f);
            diagram.transform.localEulerAngles = Vector3.zero;
            diagram.transform.localScale = Vector3.one;
            Sprite sprite = Sprite.CreateSprite(FindSprite("science diagrams_0").texture, new Rect(512, 341, 512, 341), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect, Vector4.zero, false);
            diagram.AddComponent<SpriteRenderer>().sprite = sprite;
            diagram.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1, 1, 1, 0.5f);
            Chalkboard.SetActive(false);
            GameObject.DontDestroyOnLoad(Chalkboard);
        }

        public static void LoadTextures() {

            Material ImageMaterial = FindMaterial("UI Add");

            HexagonGoldImage = CreateSprite(ImageData.GoldHex, ImageMaterial, 160, 160, "Hexagon Quest");
            TuncTitleImage = CreateSprite(ImageData.TuncTitle, ImageMaterial, 1400, 742, "tunc title logo");
            TuncImage = CreateSprite(ImageData.Tunc, ImageMaterial, 148, 148, "tunc sprite");
            FuseAltLights = CreateSprite(ImageData.FuseAltIndicatorLights, ImageMaterial, 32, 32, "fuse alt indicator lights");

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
            CustomItemImages.Add("Grass", CreateSprite(ImageData.Grass, ImageMaterial, 160, 160, SpriteName: "Randomizer items_grass"));
            CustomItemImages.Add("Fuse", CreateSprite(ImageData.Fuse, ImageMaterial, 160, 160, SpriteName: "Randomizer items_fuse"));
            CustomItemImages.Add("Bell", CreateSprite(ImageData.Bell, ImageMaterial, 160, 160, SpriteName: "Randomizer items_bell"));

            Inventory.GetItemByName("Librarian Sword").icon = CustomItemImages["Librarian Sword"].GetComponent<Image>().sprite;
            Inventory.GetItemByName("Heir Sword").icon = CustomItemImages["Heir Sword"].GetComponent<Image>().sprite;
            Inventory.GetItemByName("Dath Stone").icon = Inventory.GetItemByName("Dash Stone").icon;
            Inventory.GetItemByName("Hexagon Gold").icon = CustomItemImages["Gold Questagon"].GetComponent<Image>().sprite;
            Inventory.GetItemByName("Torch").icon = CustomItemImages["Torch Redux"].GetComponent<Image>().sprite;
            Inventory.GetItemByName("Grass").icon = CustomItemImages["Grass"].GetComponent<Image>().sprite;
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

        public static Mesh FindMesh(string MeshName) {
            List<Mesh> Meshes = Resources.FindObjectsOfTypeAll<Mesh>().Where(Sprite => Sprite.name == MeshName).ToList();
            if (Meshes != null && Meshes.Count > 0) {
                return Meshes[0];
            } else {
                return null;
            }
        }
    }
}
