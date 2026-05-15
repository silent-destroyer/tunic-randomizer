using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TunicRandomizer {
    public class RecentItemsDisplay : MonoBehaviour {
        public struct RecentItemData {
            public Check check;
            public ItemInfo itemInfo;
            public bool isForYou;
            public bool isTrinket;
            public bool isHexagonUnlock;
            public string hexagonAbility;
            public bool isShopPurchase;
            public ItemData shopItem;

            public RecentItemData(Check Check) {
                check = Check;
                itemInfo = null;
                isForYou = true;
                isTrinket = false;
                isHexagonUnlock = false;
                hexagonAbility = null;
                isShopPurchase = false;
                shopItem = null;
            }
            public RecentItemData(ItemInfo ItemInfo, bool ForYou) {
                itemInfo = ItemInfo;
                check = null;
                isForYou = ForYou;
                isTrinket = false;
                isHexagonUnlock = false;
                hexagonAbility = null;
                isShopPurchase = false;
                shopItem = null;
            }
            public RecentItemData(Check Check = null, ItemInfo ItemInfo = null, bool ForYou = false, bool IsTrinket = false, bool IsHexagonUnlock = false, string HexagonAbility = null, bool IsShopPurchase = false, ItemData ShopItem = null) {
                check = Check;
                itemInfo = ItemInfo;
                isForYou = ForYou;
                isTrinket = IsTrinket;
                isHexagonUnlock = IsHexagonUnlock;
                hexagonAbility = HexagonAbility;
                isShopPurchase = IsShopPurchase;
                shopItem = ShopItem;
            }
        }

        public static RecentItemsDisplay instance;
        public static List<GameObject> recentItems = new List<GameObject>();
        public static GameObject title;
        public static List<RecentItemData> recentItemsQueue = new List<RecentItemData>();
        public float itemQueueTimer = 0f;
        public float itemQueueDelay = 1f;

        public void Update() {
            if (PlayerCharacter.Instanced && recentItemsQueue.Count > 5) {
                itemQueueTimer += Time.fixedUnscaledDeltaTime;
                if (itemQueueTimer > (itemQueueDelay)) {
                    recentItemsQueue.RemoveAt(0);
                    UpdateItemDisplay();
                    itemQueueTimer = 0;
                }
            }
        }

        public void UpdateItemDisplay() {
            for(int i = 0; i < 5; i++) {
                recentItems[i].transform.GetChild(2).gameObject.SetActive(false);
                recentItems[i].transform.GetChild(3).transform.localScale = Vector3.one * 0.35f;
                recentItems[i].transform.GetChild(3).transform.localEulerAngles = Vector3.zero;
                if (recentItems[i].transform.GetChild(3).GetComponent<Image>().material.name != "UI Add") {
                    recentItems[i].transform.GetChild(3).GetComponent<Image>().material = ModelSwaps.FindMaterial("UI Add");
                }
                recentItems[i].transform.GetChild(3).GetComponent<Image>().sprite = null;
                recentItems[i].GetComponentInChildren<TextMeshProUGUI>(true).text = "";
            }
            for (int i = 0; i < 5; i++) {
                int index = recentItemsQueue.Count - 1 - i;
                if (recentItemsQueue.Count >= recentItems.Count) {
                    index = recentItems.Count - 1 - i;
                }
                if (index >= 0) {
                    recentItems[index].transform.GetChild(2).gameObject.SetActive(false);
                    recentItems[index].transform.GetChild(3).transform.localScale = Vector3.one * 0.35f;
                    recentItems[index].transform.GetChild(3).transform.localEulerAngles = Vector3.zero;
                    if (recentItems[index].transform.GetChild(3).GetComponent<Image>().material.name != "UI Add") {
                        recentItems[index].transform.GetChild(3).GetComponent<Image>().material = ModelSwaps.FindMaterial("UI Add");
                    }
                    bool isMoney = false;
                    bool isTrap = false;
                    bool isTrinket = false;
                    bool isSwordUpgrade = false;
                    if (recentItemsQueue.Count > i) {
                        RecentItemData item = recentItemsQueue[i];
                        if (item.check != null) {
                            ItemData itemData = ItemLookup.GetItemDataFromCheck(item.check);
                            isMoney = itemData.Type == ItemTypes.MONEY;
                            isTrinket = itemData.Type == ItemTypes.TRINKET;
                            isTrap = itemData.Type == ItemTypes.FOOLTRAP;
                            isSwordUpgrade = itemData.Type == ItemTypes.SWORDUPGRADE;
                            recentItems[index].transform.GetChild(3).GetComponent<Image>().sprite = ModelSwaps.FindSprite(TextBuilderPatches.CustomSpriteIcons[TextBuilderPatches.ItemNameToAbbreviation[itemData.Name]]);
                            string itemFormatted = itemData.Name;
                            int split = itemFormatted.LastIndexOf(' ');
                            if (itemFormatted.Length > 20) {
                                if (split > 0) {
                                    itemFormatted = itemFormatted.Substring(0, split) + "\n" + itemFormatted.Substring(split+1);
                                }
                            }
                            recentItems[index].GetComponentInChildren<TextMeshProUGUI>(true).text = itemFormatted;
                            if (item.isHexagonUnlock && item.hexagonAbility != null) {
                                recentItems[index].GetComponentInChildren<TextMeshProUGUI>(true).text += $"\n({item.hexagonAbility} Unlocked)";
                            }
                            if (item.check.Location.SceneName == "Shop" || item.check.Location.SceneName == "ShopSpecial") {
                                recentItems[index].GetComponentInChildren<TextMeshProUGUI>(true).text += "\nfrom Shopkeeper";
                            }
                        } else if (item.itemInfo != null) {
                            isTrap = item.itemInfo.Flags.HasFlag(ItemFlags.Trap);
                            if (item.itemInfo.Player.Game != "TUNIC" && !item.isForYou) {
                                recentItems[index].transform.GetChild(3).GetComponent<Image>().sprite = ModelSwaps.FindSprite("Randomizer items_Archipelago Item");
                                string itemFormatted = item.itemInfo.ItemDisplayName.Length > 20 ? item.itemInfo.ItemDisplayName.Substring(0, 20) + "..." : item.itemInfo.ItemDisplayName;
                                itemFormatted = itemFormatted.Replace("_", " ");
                                recentItems[index].GetComponentInChildren<TextMeshProUGUI>(true).text = $"{itemFormatted}\nsent to {(item.itemInfo.Player.Name.Length > 15 ? item.itemInfo.Player.Name.Substring(0, 14) + "..." : item.itemInfo.Player.Name)}";
                            } else {
                                ItemData itemData = ItemLookup.Items[item.itemInfo.ItemDisplayName];
                                isTrap = isTrap || itemData.Type == ItemTypes.FOOLTRAP;
                                isMoney = itemData.Type == ItemTypes.MONEY;
                                isTrinket = itemData.Type == ItemTypes.TRINKET;
                                isSwordUpgrade = itemData.Type == ItemTypes.SWORDUPGRADE;
                                recentItems[index].transform.GetChild(3).GetComponent<Image>().sprite = ModelSwaps.FindSprite(TextBuilderPatches.CustomSpriteIcons[TextBuilderPatches.ItemNameToAbbreviation[item.itemInfo.ItemName]]);
                                string itemFormatted = item.itemInfo.ItemDisplayName;
                                int split = itemFormatted.LastIndexOf(' ');
                                if (itemFormatted.Length > 20) {
                                    if (split > 0) {
                                        itemFormatted = itemFormatted.Substring(0, split) + "\n" + itemFormatted.Substring(split + 1);
                                    }
                                }
                                if (item.isForYou) {
                                    recentItems[index].GetComponentInChildren<TextMeshProUGUI>(true).text = $"{itemFormatted}";
                                    if (item.isHexagonUnlock && item.hexagonAbility != null) {
                                        recentItems[index].GetComponentInChildren<TextMeshProUGUI>(true).text += $"\n({item.hexagonAbility} Unlocked)";
                                    }
                                    if (item.itemInfo.Player != Archipelago.instance.GetPlayerSlot()) {
                                        recentItems[index].GetComponentInChildren<TextMeshProUGUI>(true).text += $"\nfrom {(item.itemInfo.Player.Name.Length > 15 ? item.itemInfo.Player.Name.Substring(0, 14) + "..." : item.itemInfo.Player.Name)}";
                                    } else if (item.itemInfo.Player == Archipelago.instance.GetPlayerSlot()) {
                                        if (Locations.LocationDescriptionToId.ContainsKey(item.itemInfo.LocationDisplayName) && Locations.VanillaLocations.ContainsKey(Locations.LocationDescriptionToId[item.itemInfo.LocationDisplayName])) {
                                            Check c = Locations.VanillaLocations[Locations.LocationDescriptionToId[item.itemInfo.LocationDisplayName]];
                                            if (c.Location.SceneName == "Shop" || c.Location.SceneName == "ShopSpecial") {
                                                recentItems[index].GetComponentInChildren<TextMeshProUGUI>(true).text += "\nfrom Shopkeeper";
                                            }
                                        }
                                    }
                                } else {
                                    recentItems[index].GetComponentInChildren<TextMeshProUGUI>(true).text = $"{itemFormatted}\nsent to {(item.itemInfo.Player.Name.Length > 15 ? item.itemInfo.Player.Name.Substring(0, 14) + "..." : item.itemInfo.Player.Name)}";
                                }
                            } 
                        } else if (item.isShopPurchase) {
                            if (item.shopItem != null) {
                                recentItems[index].transform.GetChild(3).GetComponent<Image>().sprite = ModelSwaps.FindSprite(TextBuilderPatches.CustomSpriteIcons[TextBuilderPatches.ItemNameToAbbreviation[item.shopItem.Name]]);
                                recentItems[index].GetComponentInChildren<TextMeshProUGUI>(true).text = $"{item.shopItem.Name}\nfrom Shopkeeper";
                            }
                        }
                        if (isMoney) {
                            recentItems[index].transform.GetChild(3).transform.localScale = Vector3.one * 0.2f;
                        }
                        if (isTrap) {
                            if (!recentItems[index].transform.GetChild(3).GetComponent<Image>().sprite.name.Contains("Archipelago")) {
                                recentItems[index].transform.GetChild(3).transform.localScale = Vector3.one * 0.2f;
                            }
                            recentItems[index].transform.GetChild(3).transform.localEulerAngles = new Vector3(180, 0, 0);
                        }
                        if (isTrinket) {
                            item.isTrinket = true;
                            recentItems[index].transform.GetChild(2).gameObject.SetActive(true);
                            recentItems[index].transform.GetChild(3).GetComponent<Image>().material = ModelSwaps.FindMaterial("UI-trinket");
                        }
                        if (item.isForYou && isSwordUpgrade) {
                            recentItems[index].transform.GetChild(3).GetComponent<Image>().sprite = ModelSwaps.FindSprite(TextBuilderPatches.CustomSpriteIcons[TextBuilderPatches.GetSwordIconName(SaveFile.GetInt(SaveFlags.SwordProgressionLevel))]);
                        }
                        recentItemsQueue[i] = item;
                    } else {
                        recentItems[index].transform.GetChild(3).GetComponent<Image>().sprite = null;
                        recentItems[index].GetComponentInChildren<TextMeshProUGUI>(true).text = "";
                    }
                }
            }
            for (int i = 0; i < recentItems.Count; i++) {
                for (int j = 0; j < recentItems[i].transform.childCount; j++) {
                    recentItems[i].transform.GetChild(j).gameObject.SetActive(recentItemsQueue.Count > i && !InventoryDisplay.InventoryOpen && (j == 2 ? recentItemsQueue.Count >= recentItems.Count ? recentItemsQueue[recentItems.Count - 1 - i].isTrinket : recentItemsQueue[recentItemsQueue.Count - 1 - i].isTrinket : true));
                }
            }
        }

        public void ResetQueue() {
            recentItemsQueue.Clear();
            UpdateItemDisplay();
        }

        public void EnqueueShopPurchase(ShopItem shopItem) {
            ItemData itemData = ItemLookup.Items.Values.Where(item => item.ItemNameForInventory == shopItem.itemToGive.name && item.QuantityToGive == shopItem.quantityToGive).First();
            if (itemData != null) {
                recentItemsQueue.Add(new RecentItemData(IsShopPurchase: true, ShopItem: itemData));
                UpdateItemDisplay();
            }
        }

        public void EnqueueItem(Check check, bool hexagonUnlock = false, string hexagonAbility = null) {
            if (GrassRandomizer.GrassChecks.ContainsKey(check.CheckId) && check.Reward.Name == "Grass") {
                return;
            }
            if ((SaveFlags.GetBool(SaveFlags.BreakableShuffleEnabled) || SaveFlags.GetBool(SaveFlags.ShuffleEnemyDropsEnabled)) && check.Reward.Name == "money" && check.Reward.Amount <= 5) {
                return;
            }
            recentItemsQueue.Add(new RecentItemData(Check: check, ForYou: true, IsHexagonUnlock: hexagonUnlock, HexagonAbility: hexagonAbility));
            UpdateItemDisplay();
        }

        public void EnqueueItem(ItemInfo itemInfo, bool forYou, bool hexagonUnlock = false, string hexagonAbility = null) {
            if (Locations.LocationDescriptionToId.ContainsKey(itemInfo.LocationDisplayName) && itemInfo.LocationGame == "TUNIC" && GrassRandomizer.GrassChecks.ContainsKey(Locations.LocationDescriptionToId[itemInfo.LocationDisplayName]) && itemInfo.ItemDisplayName == "Grass") {
                return;
            }
            if ((SaveFlags.GetBool(SaveFlags.BreakableShuffleEnabled) || SaveFlags.GetBool(SaveFlags.ShuffleEnemyDropsEnabled)) && itemInfo.ItemGame == "TUNIC" && itemInfo.ItemDisplayName.Contains("Money")
                && ItemLookup.Items.ContainsKey(itemInfo.ItemDisplayName) && ItemLookup.Items[itemInfo.ItemDisplayName].QuantityToGive <= 5) {

                return;
            }
            recentItemsQueue.Add(new RecentItemData(ItemInfo: itemInfo, ForYou: forYou, IsHexagonUnlock: hexagonUnlock, HexagonAbility: hexagonAbility));
            UpdateItemDisplay();
        }

        public static void SetupRecentItemsDisplay() {
            TMP_FontAsset FontAsset = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().Where(Font => Font.name == "Latin Rounded").ToList()[0];
            Material FontMaterial = Resources.FindObjectsOfTypeAll<Material>().Where(Material => Material.name == "Latin Rounded - Quantity Outline").ToList()[0];

            GameObject potionDisplay = Resources.FindObjectsOfTypeAll<PotionDisplay>().First().gameObject;
            title = new GameObject("randomizer recent items text");
            title.AddComponent<TextMeshProUGUI>().text = "Recent Items";
            title.GetComponent<TextMeshProUGUI>().fontSize = 24;
            title.GetComponent<TextMeshProUGUI>().font = FontAsset;
            title.GetComponent<TextMeshProUGUI>().fontMaterial = FontMaterial;
            title.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
            GameObject recentItemsDisplay = new GameObject("randomizer recent items display");
            title.transform.parent = recentItemsDisplay.transform;
            RecentItemsDisplay.instance = recentItemsDisplay.AddComponent<RecentItemsDisplay>();
            recentItemsDisplay.transform.parent = potionDisplay.transform;
            recentItemsDisplay.transform.localPosition = new Vector3(-166f, -250f, 0f);
            recentItemsDisplay.layer = 5;
            recentItemsDisplay.AddComponent<Canvas>();
            recentItemsDisplay.AddComponent<CanvasScaler>();
            recentItemsDisplay.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            recentItemsDisplay.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
            recentItemsDisplay.AddComponent<VerticalLayoutGroup>();
            title.AddComponent<LayoutElement>().minWidth = 130;
            for(int i = 0; i < 5; i++) {
                recentItems.Add(SetupItemTextAndSprite(recentItemsDisplay.transform, i));
            }
            recentItemsDisplay.GetComponent<VerticalLayoutGroup>().spacing = 50f;
            recentItemsDisplay.GetComponent<VerticalLayoutGroup>().childAlignment = TextAnchor.UpperCenter;
            recentItemsDisplay.transform.localScale = Vector3.one * 2;
            recentItemsDisplay.SetActive(TunicRandomizer.Settings.ShowRecentItems);
        }

        public static GameObject SetupItemTextAndSprite(Transform parent, int i) {
            TMP_FontAsset FontAsset = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().Where(Font => Font.name == "Latin Rounded").ToList()[0];
            Material FontMaterial = Resources.FindObjectsOfTypeAll<Material>().Where(Material => Material.name == "Latin Rounded - Quantity Outline").ToList()[0];
            GameObject item = new GameObject("item " + i);
            item.transform.parent = parent.transform;
            item.AddComponent<LayoutElement>();
            GameObject backing = new GameObject("backing");
            GameObject sprite = new GameObject("sprite");
            GameObject spriteBacking = new GameObject("sprite backing");
            GameObject trinketBacking = new GameObject("trinket backing");
            GameObject text = new GameObject("text");
            backing.transform.parent = item.transform;
            spriteBacking.transform.parent = item.transform;
            trinketBacking.transform.parent = item.transform;
            sprite.transform.parent = item.transform;
            text.transform.parent = item.transform;

            backing.AddComponent<Image>().sprite = ModelSwaps.FindSprite("UI_offeringBacking");
            backing.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1, 0.5f);
            backing.transform.localScale = new Vector3(2, 1, 1);
            backing.transform.localPosition = Vector3.zero;

            sprite.AddComponent<Image>().sprite = ModelSwaps.FindSprite("UI_soft");
            sprite.GetComponent<Image>().material = ModelSwaps.FindMaterial("UI Add");
            sprite.transform.localPosition = new Vector3(-53, -5, 0);
            sprite.transform.localScale = Vector3.one * 0.35f;

            spriteBacking.AddComponent<Image>().sprite = ModelSwaps.FindSprite("UI_soft");
            spriteBacking.transform.localPosition = new Vector3(-53, -5, 0);
            spriteBacking.transform.localScale = Vector3.one * 0.5f;

            trinketBacking.AddComponent<Image>().sprite = ModelSwaps.FindSprite("trinkets 2_backing");
            trinketBacking.transform.localPosition = new Vector3(-53, -5, 0);
            trinketBacking.transform.localScale = Vector3.one * 0.35f;

            text.AddComponent<TextMeshProUGUI>().text = "";
            text.GetComponent<TextMeshProUGUI>().fontSize = 12;
            text.GetComponent<TextMeshProUGUI>().font = FontAsset;
            text.GetComponent<TextMeshProUGUI>().fontMaterial = FontMaterial;
            text.GetComponent<TextMeshProUGUI>().autoSizeTextContainer = true;
            text.GetComponent<TextMeshProUGUI>().verticalAlignment = VerticalAlignmentOptions.Middle;
            text.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 50);
            text.transform.localPosition = new Vector3(22f, -5, 0f);
            //item.transform.localPosition += new Vector3(0, 40-(i*50), 0);
            item.SetActive(true);
            backing.SetActive(false);
            sprite.SetActive(false);
            spriteBacking.SetActive(false);
            trinketBacking.SetActive(false);
            text.SetActive(false);
            return item;
        }

    }
}
