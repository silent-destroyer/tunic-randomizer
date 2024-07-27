using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Il2CppSystem.Uri;

namespace TunicRandomizer {
    public class RecentItemsDisplay : MonoBehaviour {
        public struct RecentItemData {
            public Check check;
            public ItemInfo itemInfo;
            public bool isForYou;
            public RecentItemData(Check Check) {
                check = Check;
                itemInfo = null;
                isForYou = false;
            }
            public RecentItemData(ItemInfo ItemInfo, bool ForYou) {
                itemInfo = ItemInfo;
                check = null;
                isForYou = ForYou;
            }
        }

        public static RecentItemsDisplay instance;
        public static List<GameObject> recentItems = new List<GameObject>();
        public static GameObject title;
        public static Queue<RecentItemData> recentItemsQueue = new Queue<RecentItemData>();
        public float itemQueueTimer = 0f;
        public float itemQueueDelay = 3f;
        public void Start() {
            
        }
        public void Update() {
            title.SetActive(TunicRandomizer.Settings.ShowRecentItems);
            if (!TunicRandomizer.Settings.ShowRecentItems) {
                foreach (GameObject obj in recentItems) {
                    obj.SetActive(false);
                }
                return;
            }
            if (PlayerCharacter.Instanced) {
                itemQueueTimer += Time.fixedUnscaledDeltaTime;
                if (itemQueueTimer > (recentItemsQueue.Count > 10 ? 1.5 : itemQueueDelay)) {
                    if (recentItemsQueue.Count > 5) {
                        recentItemsQueue.Dequeue();
                        UpdateItemDisplay();
                    }
                    itemQueueTimer = 0;
                }
            }
        }

        public void UpdateItemDisplay() {
            for (int i = 0; i < 5; i++) {
                recentItems[i].transform.GetChild(2).gameObject.SetActive(false);
                recentItems[i].transform.GetChild(3).transform.localScale = Vector3.one * 0.35f;
                recentItems[i].transform.GetChild(3).transform.localEulerAngles = Vector3.zero;
                if (recentItems[i].transform.GetChild(3).GetComponent<Image>().material.name != "UI Add") {
                    recentItems[i].transform.GetChild(3).GetComponent<Image>().material = ModelSwaps.FindMaterial("UI Add");
                }
                bool isMoney = false;
                bool isTrap = false;
                bool isTrinket = false;
                if (recentItemsQueue.Count > i) {
                    RecentItemData item = recentItemsQueue.ElementAt(i);
                    if (item.check != null) {
                        ItemData itemData = ItemLookup.GetItemDataFromCheck(item.check);
                        isMoney = itemData.Type == ItemTypes.MONEY;
                        isTrinket = itemData.Type == ItemTypes.TRINKET;
                        isTrap = itemData.Type == ItemTypes.FOOLTRAP;
                        recentItems[i].transform.GetChild(3).GetComponent<Image>().sprite = ModelSwaps.FindSprite(TextBuilderPatches.CustomSpriteIcons[TextBuilderPatches.ItemNameToAbbreviation[itemData.Name]]);
                        recentItems[i].GetComponentInChildren<TextMeshProUGUI>().text = itemData.Name;
                    } else if (item.itemInfo != null) {
                        isTrap = item.itemInfo.Flags.HasFlag(ItemFlags.Trap);
                        if (item.itemInfo.Player.Game != "TUNIC" && !item.isForYou) {
                            recentItems[i].transform.GetChild(3).GetComponent<Image>().sprite = ModelSwaps.FindSprite("Randomizer items_Archipelago Item");
                            string itemFormatted = item.itemInfo.ItemName.Length > 20 ? item.itemInfo.ItemName.Substring(0, 20) + "..." : item.itemInfo.ItemName;
                            itemFormatted = itemFormatted.Replace("_", " ");
                            recentItems[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{itemFormatted}\nsent to {(item.itemInfo.Player.Name.Length > 15 ? "\n" + item.itemInfo.Player.Name : item.itemInfo.Player.Name)}";
                        } else {
                            ItemData itemData = ItemLookup.Items[item.itemInfo.ItemName];
                            isMoney = itemData.Type == ItemTypes.MONEY;
                            isTrinket = itemData.Type == ItemTypes.TRINKET;
                            recentItems[i].transform.GetChild(3).GetComponent<Image>().sprite = ModelSwaps.FindSprite(TextBuilderPatches.CustomSpriteIcons[TextBuilderPatches.ItemNameToAbbreviation[item.itemInfo.ItemName]]);
                            if (item.isForYou) {
                                recentItems[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{item.itemInfo.ItemName}";
                                if (item.itemInfo.Player != Archipelago.instance.GetPlayerSlot()) {
                                    recentItems[i].GetComponentInChildren<TextMeshProUGUI>().text += $"\nfrom {item.itemInfo.Player.Name}";
                                }
                            } else {
                                recentItems[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{item.itemInfo.ItemName}\nsent to {(item.itemInfo.Player.Name.Length > 15 ? "\n" + item.itemInfo.Player.Name : item.itemInfo.Player.Name)}";
                            }
                        }
                    }
                    if (isMoney) {
                        recentItems[i].transform.GetChild(3).transform.localScale = Vector3.one * 0.2f;
                    }
                    if (isTrap) {
                        if(!recentItems[i].transform.GetChild(3).GetComponent<Image>().sprite.name.Contains("Archipelago")) {
                            recentItems[i].transform.GetChild(3).transform.localScale = Vector3.one * 0.2f;
                        }
                        recentItems[i].transform.GetChild(3).transform.localEulerAngles = new Vector3(180, 0, 0);
                    }
                    if (isTrinket) {
                        recentItems[i].transform.GetChild(2).gameObject.SetActive(true);
                        recentItems[i].transform.GetChild(3).GetComponent<Image>().material = ModelSwaps.FindMaterial("UI-trinket");
                    }
                    recentItems[i].SetActive(true);
                } else {
                    recentItems[i].transform.GetChild(3).GetComponent<Image>().sprite = null;
                    recentItems[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                    recentItems[i].SetActive(false);
                }
            }
        }

        public void ResetQueue() {
            recentItemsQueue.Clear();
            UpdateItemDisplay();
        }

        public void EnqueueItem(Check check) {
            if (GrassRandomizer.GrassChecks.ContainsKey(check.CheckId) && check.Reward.Name == "Grass") {
                return;
            }
            recentItemsQueue.Enqueue(new RecentItemData(check));
            UpdateItemDisplay();
        }

        public void EnqueueItem(ItemInfo itemInfo, bool forYou) {
            if (Locations.LocationDescriptionToId.ContainsKey(itemInfo.LocationName) && itemInfo.LocationGame == "TUNIC" && GrassRandomizer.GrassChecks.ContainsKey(Locations.LocationDescriptionToId[itemInfo.LocationName]) && itemInfo.ItemName == "Grass") {
                return;
            }
            recentItemsQueue.Enqueue(new RecentItemData(itemInfo, forYou));
            UpdateItemDisplay();
        }

        public static void SetupRecentItemsDisplay() {
            TMP_FontAsset FontAsset = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().Where(Font => Font.name == "Latin Rounded").ToList()[0];
            Material FontMaterial = Resources.FindObjectsOfTypeAll<Material>().Where(Material => Material.name == "Latin Rounded - Quantity Outline").ToList()[0];

            GameObject potionDisplay = Resources.FindObjectsOfTypeAll<PotionDisplay>().First().gameObject;
            title = new GameObject("randomizer recent items text");
            title.transform.parent = potionDisplay.transform;
            title.AddComponent<TextMeshProUGUI>().text = "Recent Items";
            title.GetComponent<TextMeshProUGUI>().fontSize = 24;
            title.GetComponent<TextMeshProUGUI>().font = FontAsset;
            title.GetComponent<TextMeshProUGUI>().fontMaterial = FontMaterial;
            title.transform.localPosition = new Vector3(-205f, -205f, 0f);
            GameObject recentItemsDisplay = new GameObject("randomizer recent items display");
            for(int i = 0; i < 5; i++) {
                recentItems.Add(SetupItemTextAndSprite(recentItemsDisplay.transform, i));
            }
            RecentItemsDisplay.instance = recentItemsDisplay.AddComponent<RecentItemsDisplay>();
            recentItemsDisplay.transform.parent = potionDisplay.transform;
            recentItemsDisplay.transform.localPosition = new Vector3(-166f, -305f, 0f);
            recentItemsDisplay.AddComponent<VerticalLayoutGroup>().spacing = 50f;
        }

        public static GameObject SetupItemTextAndSprite(Transform parent, int i) {
            TMP_FontAsset FontAsset = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().Where(Font => Font.name == "Latin Rounded").ToList()[0];
            Material FontMaterial = Resources.FindObjectsOfTypeAll<Material>().Where(Material => Material.name == "Latin Rounded - Quantity Outline").ToList()[0];
            GameObject item = new GameObject("item");
            item.transform.parent = parent.transform;

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
            backing.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1, 0.6f);
            backing.transform.localScale = new Vector3(2, 1, 1);
            backing.transform.localPosition = Vector3.zero;

            sprite.AddComponent<Image>().sprite = ModelSwaps.FindSprite("UI_soft");
            sprite.GetComponent<Image>().material = ModelSwaps.FindMaterial("UI Add");
            sprite.transform.localPosition = new Vector3(-53, 0, 0);
            sprite.transform.localScale = Vector3.one * 0.35f;

            spriteBacking.AddComponent<Image>().sprite = ModelSwaps.FindSprite("UI_soft");
            spriteBacking.transform.localPosition = new Vector3(-53, 0, 0);
            spriteBacking.transform.localScale = Vector3.one * 0.5f;

            trinketBacking.AddComponent<Image>().sprite = ModelSwaps.FindSprite("trinkets 2_backing");
            trinketBacking.transform.localPosition = new Vector3(-53, 0, 0);
            trinketBacking.transform.localScale = Vector3.one * 0.35f;

            text.AddComponent<TextMeshProUGUI>().text = "";
            text.GetComponent<TextMeshProUGUI>().fontSize = 12;
            text.GetComponent<TextMeshProUGUI>().font = FontAsset;
            text.GetComponent<TextMeshProUGUI>().fontMaterial = FontMaterial;
            text.GetComponent<TextMeshProUGUI>().autoSizeTextContainer = true;
            text.GetComponent<TextMeshProUGUI>().verticalAlignment = VerticalAlignmentOptions.Middle;
            text.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 50);
            text.transform.localPosition = new Vector3(22f, 0, 0f);
            item.transform.localPosition += new Vector3(0, 40-(i*50), 0);
            return item;
        }

    }
}
