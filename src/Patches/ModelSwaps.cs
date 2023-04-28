using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TunicRandomizer {

    public class ModelSwaps {
        public static ManualLogSource Logger = TunicRandomizer.Logger;
        public static ItemTracker Tracker = TunicRandomizer.Tracker;
        
        public static Dictionary<string, Sprite> Cards = new Dictionary<string, Sprite>();
        public static Dictionary<string, GameObject> Items = new Dictionary<string, GameObject>();
        public static Dictionary<string, GameObject> Chests = new Dictionary<string, GameObject>();

        public static GameObject SecondSword = null;
        public static GameObject ThirdSword = null;
        public static GameObject PagePickup = null;
        public static bool SwappedThisSceneAlready = false;
        public static GameObject SecondSwordImage;
        public static GameObject ThirdSwordImage;
        public static GameObject HexagonGoldImage;
        public static GameObject TuncTitleImage;
        public static GameObject TuncImage;
        public static GameObject GardenKnightVoid;
        public static GameObject MoneySfx;
        public static GameObject FairyAnimation;

        public static GameObject RedKeyMaterial;
        public static GameObject GreenKeyMaterial;
        public static GameObject BlueKeyMaterial;
        public static GameObject HeroRelicMaterial;
        public static bool SetupDathStonePresentation = false;

        public static GameObject GlowEffect;

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

            Items["Key"] = ItemRoot.transform.GetChild(4).gameObject;
            Items["Key (House)"] = ItemRoot.transform.GetChild(4).gameObject;
            Items["Vault Key (Red)"] = ItemRoot.transform.GetChild(23).gameObject;

            Items["Hexagon Red"] = ItemRoot.transform.GetChild(24).GetChild(0).gameObject;
            Items["Hexagon Green"] = ItemRoot.transform.GetChild(27).GetChild(0).gameObject;
            Items["Hexagon Blue"] = ItemRoot.transform.GetChild(28).GetChild(0).gameObject;

            RedKeyMaterial = new GameObject();
            RedKeyMaterial.AddComponent<MeshRenderer>().materials = new Material[] {Resources.FindObjectsOfTypeAll<Material>().Where(Material => Material.name == "Questagon (1) R").ToList()[0] };
            GameObject.DontDestroyOnLoad(RedKeyMaterial);
            GreenKeyMaterial = new GameObject();
            GreenKeyMaterial.AddComponent<MeshRenderer>().materials = new Material[] { Resources.FindObjectsOfTypeAll<Material>().Where(Material => Material.name == "Questagon (2) G").ToList()[0] };
            GameObject.DontDestroyOnLoad(GreenKeyMaterial);
            BlueKeyMaterial = new GameObject();
            BlueKeyMaterial.AddComponent<MeshRenderer>().materials = new Material[] { Resources.FindObjectsOfTypeAll<Material>().Where(Material => Material.name == "Questagon (3) B").ToList()[0] };
            GameObject.DontDestroyOnLoad(BlueKeyMaterial);
            Items["Hexagon Gold"] = GameObject.Instantiate(ItemRoot.transform.GetChild(28).GetChild(0).gameObject);
            Items["Hexagon Gold"].GetComponent<MeshRenderer>().material = Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().material;
            Items["Hexagon Gold"].GetComponent<MeshRenderer>().materials[0] = Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().material;
            Items["Hexagon Gold"].SetActive(false);
            GameObject.DontDestroyOnLoad(Items["Hexagon Gold"]);

            Items["Dath Stone"] = GameObject.Instantiate(ItemRoot.transform.GetChild(28).GetChild(0).gameObject);
            Items["Dath Stone"].SetActive(false);
            Material DathStoneMaterial = Items["Lantern"].GetComponent<MeshRenderer>().material;
            DathStoneMaterial.color = new Color(1, 1, 1);
            Items["Dath Stone"].GetComponent<MeshRenderer>().material = DathStoneMaterial;
            Items["Dath Stone"].GetComponent<MeshRenderer>().materials[0] = DathStoneMaterial;
            GameObject.Destroy(Items["Dath Stone"].GetComponent<Rotate>());
            GameObject.DontDestroyOnLoad(Items["Dath Stone"]);

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
            MoneySfx = new GameObject("moneysfx");
            MoneySfx.AddComponent<FMODUnity.StudioEventEmitter>().EventReference = Resources.FindObjectsOfTypeAll<ItemPickup>().Where(ItemPickup => ItemPickup.name == "Coin Pickup 1(Clone)").ToList()[0].pickupSFX;
            GameObject.DontDestroyOnLoad(MoneySfx);
            Items["money small"].SetActive(false);
            Items["money medium"].SetActive(false);
            Items["money large"].SetActive(false);
            MoneySfx.SetActive(false);
            //sol coin - purple
            //sol coin - gold
            //sol coin - blue
            foreach (TrinketItem TrinketItem in Resources.FindObjectsOfTypeAll<TrinketItem>()) {
                Cards[TrinketItem.name] = TrinketItem.CardGraphic;
            }

            SetupDathStoneItemPresentation();
            LoadTexture();
            InitializeExtras();
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

        public static void InitializeHeroRelics() {
            GameObject ItemRoot = Resources.FindObjectsOfTypeAll<GameObject>().Where(Item => Item.name == "User Rotation Root").ToList()[0];

            Material RelicMaterial = FindMaterial("ghost material_offerings");
            List<string> RelicItems = new List<string>() { "Relic - Hero Sword", "Relic - Hero Crown", "Relic - Hero Pendant HP", "Relic - Hero Pendant MP", "Relic - Hero Water", "Relic - Hero Pendant SP" };
            List<int> ItemPositions = new List<int>() { 15, 13, 18, 19, 12, 14 };
            for (int i = 0; i < RelicItems.Count; i++) {
                Items[RelicItems[i]] = GameObject.Instantiate(ItemRoot.transform.GetChild(ItemPositions[i]).gameObject);
                if (Items[RelicItems[i]].GetComponent<MeshRenderer>() != null) {
                    Items[RelicItems[i]].GetComponent<MeshRenderer>().material = RelicMaterial;
                }
                for (int j = 0; j < Items[RelicItems[i]].transform.childCount; j++) {
                    Items[RelicItems[i]].transform.GetChild(j).GetComponent<MeshRenderer>().material = RelicMaterial;
                }
                Items[RelicItems[i]].SetActive(false);
                GameObject.DontDestroyOnLoad(Items[RelicItems[i]]);
            }
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

            if (SceneLoaderPatches.SceneName == "Shop") {
                SetupShopItems();
            } else {
                if (TunicRandomizer.Settings.ShowItemsEnabled) {
                    foreach (ItemPickup ItemPickup in Resources.FindObjectsOfTypeAll<ItemPickup>()) {
                        if (ItemPickup != null && ItemPickup.itemToGive != null) {
                            if (ItemPickup.itemToGive.name == "Hexagon Red") {
                                SetupRedHexagonPlinth();
                            } else if (ItemPickup.itemToGive.name == "Hexagon Blue") {
                                SetupBlueHexagonPlinth();
                            } else {
                                SetupItemPickup(ItemPickup);
                            }
                        }
                    }

                    foreach (PagePickup PagePickup in Resources.FindObjectsOfTypeAll<PagePickup>()) {
                        SetupPagePickup(PagePickup);
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
            SwappedThisSceneAlready = true;
        }

        public static void ApplyChestTexture(Chest Chest) {
            //hyperdash chest Spirit Realm Doorway Glow (Instance)
            if (Chest != null) {
                string ItemId = Chest.chestID == 0 ? $"{SceneLoaderPatches.SceneName}-{Chest.transform.position.ToString()} [{SceneLoaderPatches.SceneName}]" : $"{Chest.chestID} [{SceneLoaderPatches.SceneName}]";
                if (RandomItemPatches.ItemList.ContainsKey(ItemId)) {
                    ItemData Item = RandomItemPatches.ItemList[ItemId];
                    //TODO: questagon chest textures
                    if (Item.Reward.Type == "FAIRY") {
                        Chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = Chests["Fairy"].GetComponent<MeshRenderer>().materials;
                        Chest.GetComponent<FMODUnity.StudioEventEmitter>().EventReference = Chests["Fairy"].GetComponent<FMODUnity.StudioEventEmitter>().EventReference;
                    } else if (Item.Reward.Name.Contains("GoldenTrophy")) {
                        Chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = Chests["GoldenTrophy"].GetComponent<MeshRenderer>().materials;
                        Chest.GetComponent<FMODUnity.StudioEventEmitter>().EventReference = Chests["GoldenTrophy"].GetComponent<FMODUnity.StudioEventEmitter>().EventReference;
                    } else if (Item.Reward.Name == "Hyperdash") {
                        Chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = Chests["Hyperdash"].GetComponent<MeshRenderer>().materials;
                        Chest.GetComponent<FMODUnity.StudioEventEmitter>().EventReference = Chests["Hyperdash"].GetComponent<FMODUnity.StudioEventEmitter>().EventReference;
                    } else if (Item.Reward.Name.Contains("Hexagon") && SaveFile.GetString("randomizer game mode") == "RANDOMIZER") {
                        Material[] Mats = new Material[] { Items[Item.Reward.Name].GetComponent<MeshRenderer>().material, Items[Item.Reward.Name].GetComponent<MeshRenderer>().material };
                        Chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = Mats;
                        Chest.GetComponent<FMODUnity.StudioEventEmitter>().EventReference = Chests["Normal"].GetComponent<FMODUnity.StudioEventEmitter>().EventReference;
                    } else {
                        Chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = Chests["Normal"].GetComponent<MeshRenderer>().materials;
                        Chest.GetComponent<FMODUnity.StudioEventEmitter>().EventReference = Chests["Normal"].GetComponent<FMODUnity.StudioEventEmitter>().EventReference;
                    }
                }
                Chest.GetComponent<FMODUnity.StudioEventEmitter>().Lookup();
            }
        }

        public static void SetupGardenKnightVoid() {
            GardenKnightVoid = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<TunicKnightVoid>().ToList()[0].gameObject);
            GardenKnightVoid.SetActive(false);
            GameObject.DontDestroyOnLoad(GardenKnightVoid);
        }

        public static void SetupHeroRelicPickup(HeroRelicPickup HeroRelicPickup) {
            string ItemId = $"{HeroRelicPickup.name} [{SceneLoaderPatches.SceneName}]";
            if (RandomItemPatches.ItemList.ContainsKey(ItemId)) {
                ItemData Item = RandomItemPatches.ItemList[ItemId];
/*                if (Item.Reward.Name == HeroRelicPickup.relicItem.name) {
                    return;
                }*/
                for (int i = 0; i < HeroRelicPickup.transform.childCount; i++) {
                    HeroRelicPickup.transform.GetChild(i).gameObject.SetActive(false);
                }
                if (HeroRelicPickup.GetComponent<MeshFilter>() != null) {
                    GameObject.Destroy(HeroRelicPickup.GetComponent<MeshFilter>());
                    GameObject.Destroy(HeroRelicPickup.GetComponent<MeshRenderer>());
                }
                GameObject NewItem = SetupItemBase(HeroRelicPickup.transform, Item);

                NewItem.transform.localRotation = ItemPositions.ItemPickupRotations.ContainsKey(Item.Reward.Name) ? ItemPositions.ItemPickupRotations[Item.Reward.Name] : Quaternion.Euler(0, 0, 0);
                NewItem.transform.localPosition = ItemPositions.ItemPickupPositions.ContainsKey(Item.Reward.Name) ? ItemPositions.ItemPickupPositions[Item.Reward.Name] : Vector3.zero;
                NewItem.transform.localScale *= 2;
                if (Item.Reward.Type == "FAIRY") {
                    NewItem.transform.localScale = Vector3.one;
                }
                if (NewItem.GetComponent<Rotate>() == null) {
                    NewItem.AddComponent<Rotate>().eulerAnglesPerSecond = (Item.Reward.Name == "Stick" || Item.Reward.Name == "Techbow") ? new Vector3(0f, 0f, 25f) : new Vector3(0f, 25f, 0f);
                }
                NewItem.SetActive(true);
            }
        }

        public static void SetupItemPickup(ItemPickup ItemPickup) {
            if (ItemPickup != null && ItemPickup.itemToGive != null) {
                string ItemId = $"{ItemPickup.itemToGive.name} [{SceneLoaderPatches.SceneName}]";

                if (RandomItemPatches.ItemsPickedUp.ContainsKey(ItemId) && RandomItemPatches.ItemsPickedUp[ItemId]) {
                    return;
                }
                if (RandomItemPatches.ItemList.ContainsKey(ItemId)) {
                    ItemData Item = RandomItemPatches.ItemList[ItemId];
                    if (Item.Reward.Name == ItemPickup.itemToGive.name) {
                        //return;
                    }
                    for (int i = 0; i < ItemPickup.transform.childCount; i++) {
                        ItemPickup.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    if (ItemPickup.GetComponent<MeshFilter>() != null) {
                        GameObject.Destroy(ItemPickup.GetComponent<MeshFilter>());
                        GameObject.Destroy(ItemPickup.GetComponent<MeshRenderer>());
                    }
                    GameObject NewItem = SetupItemBase(ItemPickup.transform, Item);

                    TransformData TransformData; 

                    if (ItemPositions.SpecificItemPlacement.ContainsKey(ItemPickup.itemToGive.name)) {
                        if (ItemPickup.itemToGive.name == "Stundagger" && SceneLoaderPatches.SceneName == "archipelagos_house") {
                            GameObject.Find("lanterndagger/").transform.localRotation = new Quaternion(0, 0, 0, 1);
                        }
                        if (ItemPickup.itemToGive.name == "Key" || ItemPickup.itemToGive.name == "Key (House)") {
                            NewItem.transform.parent.localRotation = SceneLoaderPatches.SceneName == "Overworld Redux" ? new Quaternion(0f, 0f, 0f, 0f) : new Quaternion(0f, 0.7071f, 0f, 0.7071f);
                        }
                        if (Item.Reward.Name.Contains("Trinket - ") || Item.Reward.Name == "Mask") {
                            TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name]["Trinket Card"];
                        } else if (Item.Reward.Type == "PAGE" || Item.Reward.Type == "FAIRY") {
                            TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name][Item.Reward.Type];
                        } else if (Item.Reward.Name == "money") {
                            if (Item.Reward.Amount < 30) {
                                TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name]["money small"];
                            } else if (Item.Reward.Amount >= 30 && Item.Reward.Amount < 100) {
                                TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name]["money medium"];
                            } else {
                                TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name]["money large"];
                            }
                        } else {
                            TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name][Item.Reward.Name];
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
                if (RandomItemPatches.ItemList.ContainsKey(ItemId)) {
                    ItemData Item = RandomItemPatches.ItemList[ItemId];
                    if (Item.Reward.Type == "PAGE") {
                        return;
                    }
                    Page.transform.localScale = Vector3.one;
                    GameObject.Destroy(Page.transform.GetChild(1));
                    GameObject.Destroy(Page.GetComponent<MeshFilter>());
                    GameObject.Destroy(Page.GetComponent<MeshRenderer>());

                    for (int i = 1; i < Page.transform.childCount; i++) {
                        Page.transform.GetChild(i).gameObject.SetActive(false);
                    }

                    GameObject NewItem = SetupItemBase(Page.transform, Item);

                    Page.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    TransformData TransformData;
                    if (Item.Reward.Name.Contains("Trinket - ") || Item.Reward.Name == "Mask") {
                        TransformData = ItemPositions.Techbow["Trinket Card"];
                    } else if (Item.Reward.Name == "money") {
                        if (Item.Reward.Amount < 30) {
                            TransformData = ItemPositions.Techbow["money small"];
                        } else if (Item.Reward.Amount >= 30 && Item.Reward.Amount < 100) {
                            TransformData = ItemPositions.Techbow["money medium"];
                        } else {
                            TransformData = ItemPositions.Techbow["money large"];
                        }
                    } else {
                        TransformData = ItemPositions.Techbow.ContainsKey(Item.Reward.Name) ? ItemPositions.Techbow[Item.Reward.Name] : ItemPositions.Techbow[Item.Reward.Type];
                    }
                    NewItem.transform.localPosition = TransformData.pos;
                    NewItem.transform.localRotation = TransformData.rot;
                    NewItem.transform.localScale = TransformData.scale;
                    NewItem.SetActive(true);
                    Page.transform.GetChild(1).gameObject.SetActive(false);
                    Page.transform.GetChild(0).gameObject.GetComponent<ParticleSystemRenderer>().material.color = Color.cyan;
                }
            }
        }

        public static void SetupShopItems() {
            List<string> ShopItemIDs = new List<string>() { 
                "Potion (First) [Shop]", 
                "Potion (West Garden) [Shop]", 
                "Trinket Coin 1 (day) [Shop]", 
                "Trinket Coin 2 (night) [Shop]" 
            };
            List<string> ShopGameObjectIDs = new List<string>() { 
                "Shop/Item Holder/Potion (First)/rotation/potion", 
                "Shop/Item Holder/Potion (West Garden)/rotation/potion", 
                "Shop/Item Holder/Trinket Coin 1 (day)/rotation/Trinket Coin", 
                "Shop/Item Holder/Trinket Coin 2 (night)/rotation/Trinket Coin" 
            };
            for (int i = 0; i < ShopItemIDs.Count; i++) {
                if (!RandomItemPatches.ItemsPickedUp[ShopItemIDs[i]]) {
                    GameObject ItemHolder = GameObject.Find(ShopGameObjectIDs[i]);
                    ItemData ShopItem = RandomItemPatches.ItemList[ShopItemIDs[i]];
                    GameObject NewItem;

                    if (ItemHolder.name.Contains("Trinket Coin")) {
                        ItemHolder.transform.rotation = new Quaternion(.7071f, 0f, 0f, -.7071f);
                    }
                    // Destroy original coin child meshes if they exist;
                    for (int j = 0; j < ItemHolder.transform.childCount; j++) {
                        GameObject.Destroy(ItemHolder.transform.GetChild(j).gameObject);
                    }

                    if (ShopItem.Reward.Type == "PAGE") {
                        NewItem = SetupPageShopItem(ItemHolder);
                    } else if (ShopItem.Reward.Type == "FAIRY") {
                        NewItem = SetupFairyShopItem(ItemHolder);
                    } else {
                        NewItem = SetupRegularShopItem(ItemHolder, ShopItem);
                    }

                    NewItem.layer = 12;
                    NewItem.transform.localPosition = Vector3.zero;
                    for (int j = 0; j < NewItem.transform.childCount; j++) {
                        NewItem.transform.GetChild(j).gameObject.layer = 12;
                    }
                    if (ShopItem.Reward.Name == "money" && ShopItem.Reward.Amount > 100) {
                        NewItem.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                    }
                    ItemHolder.SetActive(false);
                }
            }
        }
        public static GameObject SetupRegularShopItem(GameObject ItemHolder, ItemData Item) {
            GameObject NewItem;
            if (Item.Reward.Name.Contains("Trinket - ") || Item.Reward.Name == "Mask") {
                NewItem = GameObject.Instantiate(Items["Trinket Card"], ItemHolder.transform.position, ItemHolder.transform.rotation);
                NewItem.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Cards[Item.Reward.Name];
            } else if (Item.Reward.Name == "money" || Item.Reward.Name == "Fool") {
                if (Item.Reward.Amount < 30) {
                    NewItem = GameObject.Instantiate(Items["money small"], ItemHolder.transform.position, ItemHolder.transform.rotation);
                } else if (Item.Reward.Amount >= 30 && Item.Reward.Amount < 100) {
                    NewItem = GameObject.Instantiate(Items["money medium"], ItemHolder.transform.position, ItemHolder.transform.rotation);
                } else {
                    NewItem = GameObject.Instantiate(Items["money large"], ItemHolder.transform.position, ItemHolder.transform.rotation);
                }
            } else {
                NewItem = GameObject.Instantiate(Items[Item.Reward.Name], ItemHolder.transform.position, ItemHolder.transform.rotation);
            }
            NewItem.transform.parent = ItemHolder.transform.parent;
            NewItem.transform.localScale = ItemPositions.ShopItemScales.ContainsKey(Item.Reward.Name) ? ItemPositions.ShopItemScales[Item.Reward.Name] : NewItem.transform.parent.localScale;
            NewItem.transform.localRotation = ItemPositions.ShopItemRotations.ContainsKey(Item.Reward.Name) ? ItemPositions.ShopItemRotations[Item.Reward.Name] : new Quaternion(0, 180f, 0, 0);
            NewItem.SetActive(true);
            return NewItem;
        }
        public static GameObject SetupFairyShopItem(GameObject ItemHolder) {
            GameObject NewItem = GameObject.Instantiate(Chests["Fairy"], ItemHolder.transform.position, ItemHolder.transform.rotation);
            NewItem.transform.parent = ItemHolder.transform.parent;
            NewItem.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            NewItem.transform.rotation = Quaternion.identity;
            NewItem.SetActive(true);
            return NewItem;
        }
        public static GameObject SetupPageShopItem(GameObject ItemHolder) {
            GameObject NewItem = GameObject.Instantiate(PagePickup, ItemHolder.transform.position, ItemHolder.transform.rotation);
            GameObject.Destroy(NewItem.GetComponent<Rotate>());
            GameObject.Destroy(NewItem.transform.GetChild(0).GetChild(0).gameObject);
            
            NewItem.transform.parent = ItemHolder.transform.parent;
            NewItem.transform.localScale = PagePickup == null ? new Vector3(0.33f, 0.33f, 0.33f) : Vector3.one;
            NewItem.transform.localRotation = PagePickup == null ? new Quaternion(0f, 0f, 0.3827f, -0.9239f) : Quaternion.identity;
            NewItem.SetActive(true);
            return NewItem;
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

        public static void SetupDathStoneItemPresentation() {
            try {
                GameObject KeySpecial = Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(Item => Item.gameObject.name == "key twist (special)").ToList()[0].gameObject;
                if (KeySpecial.GetComponent<MeshFilter>() != null) {
                    GameObject.Destroy(KeySpecial.GetComponent<MeshRenderer>());
                    GameObject.Destroy(KeySpecial.GetComponent<MeshFilter>());
                }

                KeySpecial.AddComponent<SpriteRenderer>().sprite = Inventory.GetItemByName("Dash Stone").Icon;
                KeySpecial.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
                KeySpecial.GetComponent<SpriteRenderer>().material = Resources.FindObjectsOfTypeAll<Material>().Where(mat => mat.name == "UI Add").ToList()[0];

                KeySpecial.transform.localScale = Vector3.one;
                KeySpecial.transform.localPosition = Vector3.zero;
                KeySpecial.transform.localRotation = Quaternion.identity;
                Inventory.GetItemByName("Key Special").collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                Inventory.GetItemByName("Key Special").collectionMessage.text = $"dah% stOn!?";
            } catch (Exception e) {
            }
        }

        public static void SetupHexagonQuest() {
            try {
                Items["Hexagon Blue"].GetComponent<MeshRenderer>().materials = Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                Inventory.GetItemByName("Hexagon Blue").collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                Inventory.GetItemByName("Hexagon Blue").collectionMessage.text = $"    #uh sEl wEkinz\"...\"";
            } catch (Exception e) {
                Logger.LogInfo(e.Message);
            }
        }

        public static void RestoreOriginalHexagons() {
            try {
                Items["Hexagon Red"].GetComponent<MeshRenderer>().materials = RedKeyMaterial.GetComponent<MeshRenderer>().materials;
                Items["Hexagon Green"].GetComponent<MeshRenderer>().materials = GreenKeyMaterial.GetComponent<MeshRenderer>().materials;
                Items["Hexagon Blue"].GetComponent<MeshRenderer>().materials = BlueKeyMaterial.GetComponent<MeshRenderer>().materials;
                Inventory.GetItemByName("Hexagon Blue").collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                Inventory.GetItemByName("Hexagon Blue").collectionMessage.text = $"fownd ahn Itehm!";
            } catch (Exception e) {
                Logger.LogInfo(e.Message);
            }
        }

        public static void SwapSiegeEngineCrown() {
            GameObject VaultKey = GameObject.Find("Spidertank/Spidertank_skeleton/root/thorax/vault key graphic");
            ItemData VaultKeyItem = RandomItemPatches.ItemList["Vault Key (Red) [Fortress Arena]"];
            if (VaultKey != null) {
                if (VaultKeyItem.Reward.Name == "Vault Key (Red)") {
                    return;
                }
                if (VaultKey.GetComponent<MeshFilter>() != null) {
                    GameObject.Destroy(VaultKey.GetComponent<MeshRenderer>());
                    GameObject.Destroy(VaultKey.GetComponent<MeshFilter>());
                }

                for (int i = 0; i < VaultKey.transform.childCount; i++) {
                    VaultKey.transform.GetChild(i).gameObject.SetActive(false);
                }

                GameObject NewItem = SetupItemBase(VaultKey.transform, VaultKeyItem);

                TransformData TransformData;
                if (VaultKeyItem.Reward.Name.Contains("Trinket - ") || VaultKeyItem.Reward.Name == "Mask") {
                    TransformData = ItemPositions.VaultKeyRed["Trinket Card"];
                } else if (VaultKeyItem.Reward.Name == "money") {
                    if (VaultKeyItem.Reward.Amount < 30) {
                        TransformData = ItemPositions.VaultKeyRed["money small"];
                    } else if (VaultKeyItem.Reward.Amount >= 30 && VaultKeyItem.Reward.Amount < 100) {
                        TransformData = ItemPositions.VaultKeyRed["money medium"];
                    } else {
                        TransformData = ItemPositions.VaultKeyRed["money large"];
                    }
                } else {
                    TransformData = ItemPositions.VaultKeyRed.ContainsKey(VaultKeyItem.Reward.Name) ? ItemPositions.VaultKeyRed[VaultKeyItem.Reward.Name] : ItemPositions.VaultKeyRed[VaultKeyItem.Reward.Type];
                }
                NewItem.transform.localPosition = TransformData.pos;
                NewItem.transform.localRotation = TransformData.rot;
                NewItem.transform.localScale = TransformData.scale;
                NewItem.SetActive(true);
            }

        }

        public static GameObject SetupItemBase(Transform Parent, ItemData Item) {
            GameObject NewItem;
            if (Item.Reward.Name.Contains("Trinket - ") || Item.Reward.Name == "Mask") {
                NewItem = GameObject.Instantiate(Items["Trinket Card"], Parent.transform.position, Parent.transform.rotation);
                NewItem.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Cards[Item.Reward.Name];
            } else if (Item.Reward.Name == "money" || Item.Reward.Name == "Fool") {
                if (Item.Reward.Amount < 30) {
                    NewItem = GameObject.Instantiate(Items["money small"], Parent.transform.position, Parent.transform.rotation);
                } else if (Item.Reward.Amount >= 30 && Item.Reward.Amount < 100) {
                    NewItem = GameObject.Instantiate(Items["money medium"], Parent.transform.position, Parent.transform.rotation);
                } else {
                    NewItem = GameObject.Instantiate(Items["money large"], Parent.transform.position, Parent.transform.rotation);
                }
            } else if (Item.Reward.Type == "PAGE") {
                NewItem = GameObject.Instantiate(PagePickup, Parent.transform.position, Parent.transform.rotation);
            } else if (Item.Reward.Type == "FAIRY") {
                NewItem = GameObject.Instantiate(Chests["Fairy"], Parent.transform.position, Parent.transform.rotation);
            } else {
                NewItem = GameObject.Instantiate(Items[Item.Reward.Name], Parent.transform.position, Parent.transform.rotation);
            }
            NewItem.transform.parent = Parent.transform;
            NewItem.layer = 0;
            for (int i = 0; i < NewItem.transform.childCount; i++) {
                NewItem.transform.GetChild(i).gameObject.layer = 0;
            }
            NewItem.SetActive(true);
            return NewItem;
        }

        public static void SetupRedHexagonPlinth() {
            GameObject Plinth = GameObject.Find("_Hexagon Plinth Assembly/hexagon plinth/PRISM/questagon");
            ItemData Item = RandomItemPatches.ItemList["Hexagon Red [Fortress Arena]"];
            if (Plinth != null && Item != null) {
                if (Item.Reward.Name == "Hexagon Red") {
                    return;
                }
                if (Plinth.GetComponent<MeshFilter>() != null) {
                    GameObject.Destroy(Plinth.GetComponent<MeshRenderer>());
                    GameObject.Destroy(Plinth.GetComponent<MeshFilter>());
                }

                GameObject NewItem = SetupItemBase(Plinth.transform, Item);
                TransformData TransformData;
                if (Item.Reward.Name.Contains("Trinket - ") || Item.Reward.Name == "Mask") {
                    TransformData = ItemPositions.HexagonRed["Trinket Card"];
                } else if (Item.Reward.Name == "money") {
                    if (Item.Reward.Amount < 30) {
                        TransformData = ItemPositions.HexagonRed["money small"];
                    } else if (Item.Reward.Amount >= 30 && Item.Reward.Amount < 100) {
                        TransformData = ItemPositions.HexagonRed["money medium"];
                    } else {
                        TransformData = ItemPositions.HexagonRed["money large"];
                    }
                } else {
                    TransformData = ItemPositions.HexagonRed.ContainsKey(Item.Reward.Name) ? ItemPositions.HexagonRed[Item.Reward.Name] : ItemPositions.HexagonRed[Item.Reward.Type];
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
            ItemData Item = RandomItemPatches.ItemList["Hexagon Blue [ziggurat2020_3]"];
            if (Plinth != null && Item != null) {
                if (Item.Reward.Name == "Hexagon Blue") {
                    return;
                }
                if (Plinth.GetComponent<MeshFilter>() != null) {
                    GameObject.Destroy(Plinth.GetComponent<MeshRenderer>());
                    GameObject.Destroy(Plinth.GetComponent<MeshFilter>());
                }

                GameObject NewItem = SetupItemBase(Plinth.transform, Item);
                TransformData TransformData;
                if (Item.Reward.Name.Contains("Trinket - ") || Item.Reward.Name == "Mask") {
                    TransformData = ItemPositions.HexagonRed["Trinket Card"];
                } else if (Item.Reward.Name == "money") {
                    if (Item.Reward.Amount < 30) {
                        TransformData = ItemPositions.HexagonRed["money small"];
                    } else if (Item.Reward.Amount >= 30 && Item.Reward.Amount < 100) {
                        TransformData = ItemPositions.HexagonRed["money medium"];
                    } else {
                        TransformData = ItemPositions.HexagonRed["money large"];
                    }
                } else {
                    TransformData = ItemPositions.HexagonRed.ContainsKey(Item.Reward.Name) ? ItemPositions.HexagonRed[Item.Reward.Name] : ItemPositions.HexagonRed[Item.Reward.Type];
                }
                NewItem.transform.localPosition = TransformData.pos;
                NewItem.transform.localRotation = TransformData.rot;
                NewItem.transform.localScale = TransformData.scale;
                if (Item.Reward.Type == "FAIRY") {
                    NewItem.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
                if (Item.Reward.Type == "PAGE") {
                    NewItem.transform.localScale = Vector3.one;
                }
                Plinth.transform.localPosition = new Vector3(-0.1f, 4.7f, -1.6f);
                Plinth.transform.localScale = Vector3.one;
                NewItem.SetActive(true);
            }
        }

        public static void LoadTexture() {

            Texture2D GoldHexTexture = new Texture2D(160, 160, TextureFormat.DXT1, false);
            ImageConversion.LoadImage(GoldHexTexture, Convert.FromBase64String(ImageData.GoldHex));
            Texture2D SecondSwordTexture = new Texture2D(160, 160, TextureFormat.DXT1, false);
            ImageConversion.LoadImage(SecondSwordTexture, Convert.FromBase64String(ImageData.SecondSword));
            Texture2D ThirdSwordTexture = new Texture2D(160, 160, TextureFormat.DXT1, false);
            ImageConversion.LoadImage(ThirdSwordTexture, Convert.FromBase64String(ImageData.ThirdSword));
            Texture2D TuncTitleTexture = new Texture2D(1400, 742, TextureFormat.DXT1, false);
            ImageConversion.LoadImage(TuncTitleTexture, Convert.FromBase64String(ImageData.TuncTitle));
            Texture2D TuncTexture = new Texture2D(148, 148, TextureFormat.DXT1, false);
            ImageConversion.LoadImage(TuncTexture, Convert.FromBase64String(ImageData.Tunc));

            Material ImageMaterial = FindMaterial("UI Add");
            HexagonGoldImage = new GameObject("hexagon gold");
            HexagonGoldImage.AddComponent<RawImage>().texture = GoldHexTexture;
            HexagonGoldImage.GetComponent<RawImage>().material = ImageMaterial;
            GameObject.DontDestroyOnLoad(HexagonGoldImage);

            SecondSwordImage = new GameObject("second sword");
            SecondSwordImage.AddComponent<RawImage>().texture = SecondSwordTexture;
            SecondSwordImage.GetComponent<RawImage>().material = ImageMaterial;
            GameObject.DontDestroyOnLoad(SecondSwordImage);

            ThirdSwordImage = new GameObject("third sword");
            ThirdSwordImage.AddComponent<RawImage>().texture = ThirdSwordTexture;
            ThirdSwordImage.GetComponent<RawImage>().material = ImageMaterial;
            GameObject.DontDestroyOnLoad(ThirdSwordImage);

            TuncTitleImage = new GameObject("tunc title logo");
            TuncTitleImage.AddComponent<RawImage>().texture = TuncTitleTexture;
            TuncTitleImage.GetComponent<RawImage>().material = ImageMaterial;
            GameObject.DontDestroyOnLoad(TuncTitleImage);

            TuncImage = new GameObject("TUNC");
            TuncImage.AddComponent<RawImage>().texture = TuncTexture;
            //TuncImage.GetComponent<RawImage>().material = ImageMaterial;
            GameObject.DontDestroyOnLoad(TuncImage);

        }

        public static Material FindMaterial(string MaterialName) {
            List<Material> Material = Resources.FindObjectsOfTypeAll<Material>().Where(Mat => Mat.name == MaterialName).ToList();
            if (Material != null && Material.Count > 0) {
                return Material[0];
            } else {
                return null;
            }
        }
    }
}
