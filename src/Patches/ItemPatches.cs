using Archipelago.MultiClient.Net.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {

    public class ItemPatches {
        
        public enum ItemResult {
            Success,
            TemporaryFailure, // Can't accept right now, but can accept in the future
            PermanentFailure // Can never accept it
        }

        public static string SaveFileCollectedKey = "randomizer picked up ";

        public static string GetChestRewardID(Chest Chest) {
            return Chest.chestID == 0 ? $"{SceneLoaderPatches.SceneName}-{Chest.transform.position.ToString()} [{SceneLoaderPatches.SceneName}]" : $"{Chest.chestID} [{SceneLoaderPatches.SceneName}]";
        }

        public static bool Chest_IInteractionReceiver_Interact_PrefixPatch(Item i, Chest __instance) {
            if (SaveFile.GetInt("archipelago") == 1 && !Archipelago.instance.IsConnected()) {
                Archipelago.instance.integration.ShowNotConnectedError();
                return false;
            }
            __instance.isFairy = false;
            return true;
        }

        public static void Chest_openSequence_MoveNext_PostfixPatch(Chest._openSequence_d__35 __instance, ref bool __result) {
            __instance._delay_5__2 = 1.35f;
            if (!__result && !__instance.__4__this.interrupted) {
                string LocationId = GetChestRewardID(__instance.__4__this);
                if (__instance.__4__this.chestID == 0) {
                    string FairyId = $"{__instance.__4__this.gameObject.scene.name}-{__instance.__4__this.gameObject.transform.position.ToString()}";
                    SaveFile.SetInt(ItemLookup.FairyLookup[FairyId].Flag, 1);
                    SaveFile.SetInt($"randomizer opened fairy chest {FairyId}", 1);
                }
                TunicLogger.LogInfo("Checking Location: " + LocationId + " - " + Locations.LocationIdToDescription[LocationId]);
                if (IsArchipelago()) {
                    if (TunicRandomizer.Settings.SkipItemAnimations || ItemLookup.ItemList[LocationId].Player != Archipelago.instance.GetPlayerSlot()) {
                        ModelSwaps.SetupItemMoveUp(__instance.__4__this.transform, itemInfo: ItemLookup.ItemList[LocationId]);
                    }
                    Archipelago.instance.ActivateCheck(Locations.LocationIdToDescription[LocationId]);
                } else if (IsSinglePlayer()) {
                    Check check = Locations.RandomizedLocations[LocationId];
                    if (TunicRandomizer.Settings.SkipItemAnimations) {
                        ModelSwaps.SetupItemMoveUp(__instance.__4__this.transform, check: check);
                    }
                    GiveItem(check);
                }
            }

        }

        public static bool Chest_InterruptOpening_PrefixPatch(Chest __instance) {
            if (TunicRandomizer.Settings.DisableChestInterruption) {
                return false;
            }

            return true;
        }

        public static bool Chest_moneySprayQuantityFromDatabase_GetterPatch(Chest __instance, ref int __result) {

            __result = 0;

            return false;
        }

        public static bool Chest_itemContentsfromDatabase_GetterPatch(Chest __instance, ref Item __result) {
            
            __result = null;
            
            return false;
        }

        public static bool Chest_itemQuantityFromDatabase_GetterPatch(Chest __instance, ref int __result) {

            __result = 0;

            return false;
        }

        public static bool Chest_shouldShowAsOpen_GetterPatch(Chest __instance, ref bool __result) {
            string ActiveScene = SceneManager.GetActiveScene().name;
            if (ActiveScene == "Quarry" || ActiveScene == "Crypt") {
                __result = true;
                return false;
            }

            string ChestObjectId = __instance.chestID == 0 ? $"{__instance.gameObject.scene.name}-{__instance.transform.position.ToString()} [{ActiveScene}]" : $"{__instance.chestID} [{ActiveScene}]";
            if (Locations.LocationIdToDescription.ContainsKey(ChestObjectId)) {
                __result = SaveFile.GetInt($"randomizer picked up {ChestObjectId}") == 1 || (TunicRandomizer.Settings.CollectReflectsInWorld && SaveFile.GetInt($"randomizer {ChestObjectId} was collected") == 1);
                if (__result && __instance.GetComponentInParent<ToggleObjectBySpell>() != null && TunicRandomizer.Settings.CollectReflectsInWorld) {
                    __instance.GetComponentInParent<ToggleObjectBySpell>().stateVar = StateVariable.GetStateVariableByName("true");
                }
                return false;
            }
            return true;
        }

        public static bool ItemPickup_onGetIt_PrefixPatch(ItemPickup __instance) {
            if (__instance.itemToGive.Type == Item.ItemType.MONEY) {
                return true;
            }

            string LocationId = $"{__instance.itemToGive.name} [{SceneLoaderPatches.SceneName}]";
            if (IsArchipelago()) {
                Archipelago.instance.ActivateCheck(Locations.LocationIdToDescription[LocationId]);
            } else if (IsSinglePlayer()) {
                GiveItem(Locations.RandomizedLocations[LocationId]);
            }

            __instance.pickupStateVar.BoolValue = true;
            return false;
        }

        public static bool HeroRelicPickup_onGetIt_PrefixPatch(HeroRelicPickup __instance) {
            string LocationId = $"{__instance.name} [{SceneLoaderPatches.SceneName}]";
            if (IsArchipelago()) {
                Archipelago.instance.ActivateCheck(Locations.LocationIdToDescription[LocationId]);
            } else if (IsSinglePlayer()) {
                GiveItem(Locations.RandomizedLocations[LocationId]);
            }

            __instance.pickupStateVar.BoolValue = true;
            __instance.destroyOrDisable();
            return false;
        }

        public static bool PagePickup_onGetIt_PrefixPatch(PagePickup __instance) {
            string LocationId = $"{__instance.pageName} [{SceneLoaderPatches.SceneName}]";
            if (IsArchipelago()) {
                Archipelago.instance.ActivateCheck(Locations.LocationIdToDescription[LocationId]);
            } else if (IsSinglePlayer()) {
                GiveItem(Locations.RandomizedLocations[LocationId]);
            }

            SaveFile.SetInt($"unlocked page {PagePickup.LeafNameToLeafNumber(__instance.pageName)}", 1);
            SaveFile.SetInt($"randomizer picked up page {PagePickup.LeafNameToLeafNumber(__instance.pageName)}", 1);
            return false;
        }

        public static bool ShopItem_IInteractionReceiver_Interact_PrefixPatch(Item i, ShopItem __instance) {
            string LocationId = $"{__instance.name} [Shop]";
            if (Locations.LocationIdToDescription.ContainsKey(LocationId)) {
                int Price = TunicRandomizer.Settings.CheaperShopItemsEnabled ? 300 : __instance.price;
                string itemToDisplay = "";
                if (IsArchipelago()) {
                    ItemInfo ShopItem = ItemLookup.ItemList[LocationId];
                    itemToDisplay = Archipelago.instance.IsTunicPlayer(ShopItem.Player) && TextBuilderPatches.ItemNameToAbbreviation.ContainsKey(ShopItem.ItemName) ? TextBuilderPatches.ItemNameToAbbreviation[ShopItem.ItemName] : "[archipelago]";
                    if (itemToDisplay == "[realsword]" && SaveFile.GetInt(SwordProgressionEnabled) == 1) {
                        itemToDisplay = ShopItem.Player == Archipelago.instance.GetPlayerSlot() ? TextBuilderPatches.GetSwordIconName(SaveFile.GetInt(SwordProgressionLevel) + 1) : itemToDisplay;
                    }
                    __instance.confirmPurchaseFormattedLanguageLine.text = $"bI for {Price} [money]?\n    {itemToDisplay} " + GhostHints.WordWrapString($"\"{Archipelago.instance.GetPlayerName(ShopItem.Player).ToUpper().Replace(" ", "\" \"")}'S\" \"{ShopItem.ItemName.ToUpper().Replace("_", " ").Replace($" ", $"\" \"")}\"");
                } else if (IsSinglePlayer()) {
                    ItemData itemData = ItemLookup.GetItemDataFromCheck(Locations.RandomizedLocations[LocationId]);
                    itemToDisplay = TextBuilderPatches.ItemNameToAbbreviation.ContainsKey(itemData.Name) ? TextBuilderPatches.ItemNameToAbbreviation[itemData.Name] : "";
                    if (itemToDisplay == "[realsword]" && SaveFile.GetInt(SwordProgressionEnabled) == 1) {
                        itemToDisplay = TextBuilderPatches.GetSwordIconName(SaveFile.GetInt(SwordProgressionLevel) + 1);
                    }
                    __instance.confirmPurchaseFormattedLanguageLine.text = $"bI for {Price} [money]?";
                    if (TunicRandomizer.Settings.ShowItemsEnabled) {
                        __instance.confirmPurchaseFormattedLanguageLine.text += $"\n{itemToDisplay} \"{itemData.Name}\"";
                    }
                }

                string CheckName = Locations.LocationIdToDescription[LocationId];
                if (IsArchipelago() && TunicRandomizer.Settings.SendHintsToServer && SaveFile.GetInt($"archipelago sent optional hint to server {CheckName}") == 0) {
                    Archipelago.instance.integration.session.Locations.ScoutLocationsAsync(true, Archipelago.instance.GetLocationId(CheckName, "TUNIC"));
                    SaveFile.SetInt($"archipelago sent optional hint to server {CheckName}", 1);
                }
            } else {
                __instance.confirmPurchaseFormattedLanguageLine.text = $"bI for {__instance.price} [money]?";
            }

            return true;
        }

        public static bool ShopItem_buy_PrefixPatch(ShopItem __instance) {
            string LocationId = $"{__instance.name} [Shop]";

            int Price = TunicRandomizer.Settings.CheaperShopItemsEnabled && Locations.LocationIdToDescription.ContainsKey(LocationId) ? 300 : __instance.price;
            if (Inventory.GetItemByName("MoneySmall").Quantity < Price) {
                GenericMessage.ShowMessage($"nawt Enuhf [money]...");
            } else {
                if (!Locations.LocationIdToDescription.ContainsKey(LocationId)) {
                    if (TunicRandomizer.Settings.SkipItemAnimations && __instance.itemToGive != null) {
                        string itemName = ItemLookup.SimplifiedItemNames[__instance.itemToGive.name] + (__instance.quantityToGive > 1 ? "s" : "");
                        Notifications.Show($"{TextBuilderPatches.ItemNameToAbbreviation[ItemLookup.SimplifiedItemNames[__instance.itemToGive.name]]} \"Bought {itemName}!\"", $"{ItemLookup.ShopkeeperLines[new System.Random().Next(ItemLookup.ShopkeeperLines.Count)]}");
                    }
                    return true;
                }
                Inventory.GetItemByName("MoneySmall").Quantity -= Price;
                if (IsArchipelago()) {
                    Archipelago.instance.ActivateCheck(Locations.LocationIdToDescription[LocationId]);
                } else if (IsSinglePlayer()) {
                    GiveItem(Locations.RandomizedLocations[LocationId]);
                }
                __instance.boughtStatevar.BoolValue = true;
            }
            
            InShopPlayerMoneyDisplay.Show = false;
            return false;
        }

        public static void TrinketWell_TossedInCoin_PostfixPatch(TrinketWell __instance) {
            TunicRandomizer.Tracker.ImportantItems["Coins Tossed"]++;
            ItemTracker.SaveTrackerFile();
        }

        public static bool TrinketWell_giveTrinketUpgrade_PrefixPatch(TrinketWell._giveTrinketUpgrade_d__14 __instance) {
            string LocationId = $"Well Reward ({StateVariable.GetStateVariableByName("Trinket Coins Tossed").IntValue} Coins) [Trinket Well]";
            if (SaveFile.GetInt("archipelago") == 1 && !Archipelago.instance.IsConnected()) {
                Archipelago.instance.integration.ShowNotConnectedError();
                SaveFile.SetInt(ItemCollectedKey + LocationId, 1);
                return false;
            }
            if (IsArchipelago() && Locations.LocationIdToDescription.ContainsKey(LocationId)) {
                if (TunicRandomizer.Settings.SkipItemAnimations) {
                    ModelSwaps.SetupItemMoveUp(__instance.__4__this.transform, itemInfo: ItemLookup.ItemList[LocationId]);
                }
                Archipelago.instance.ActivateCheck(Locations.LocationIdToDescription[LocationId]);
            } else if (IsSinglePlayer()) {
                Check check = Locations.RandomizedLocations[LocationId];
                if (TunicRandomizer.Settings.SkipItemAnimations) {
                    ModelSwaps.SetupItemMoveUp(__instance.__4__this.transform, check: check);
                }
                GiveItem(check);
            }
            return false;
        }

        public static ItemResult GiveItem(string ItemName, ItemInfo itemInfo) {
            if (ItemPresentation.instance.isActiveAndEnabled || GenericMessage.instance.isActiveAndEnabled || 
                NPCDialogue.instance.isActiveAndEnabled || PageDisplay.instance.isActiveAndEnabled || GenericPrompt.instance.isActiveAndEnabled ||
                GameObject.Find("_GameGUI(Clone)/PauseMenu/") != null || GameObject.Find("_OptionsGUI(Clone)") != null || PlayerCharacter.InstanceIsDead) {
                return ItemResult.TemporaryFailure;
            }
            
            if (!ItemLookup.Items.ContainsKey(ItemName)) {
                return ItemResult.PermanentFailure;
            }

            bool SkipAnimationsValue = TunicRandomizer.Settings.SkipItemAnimations;

            if (itemInfo.Player == Archipelago.instance.GetPlayerSlot() && itemInfo.LocationName != "Cheat Console" && GrassRandomizer.GrassChecks.ContainsKey(Locations.LocationDescriptionToId[itemInfo.LocationName])) {
                TunicRandomizer.Settings.SkipItemAnimations = true;
            }

            string NotificationTop = "";
            string NotificationBottom = "";
            bool DisplayMessageAnyway = false;

            ItemData Item = ItemLookup.Items[ItemName];
            string itemDisplay = TextBuilderPatches.ItemNameToAbbreviation.ContainsKey(ItemName) ? TextBuilderPatches.ItemNameToAbbreviation[ItemName] : "";
            
            if (Item.Type == ItemTypes.GRASS) {
                ItemPresentation.PresentItem(Inventory.GetItemByName("Grass"));
                Inventory.GetItemByName("Grass").Quantity += 1;
            }

            if (Item.Type == ItemTypes.MONEY) {
                int AmountToGive = Item.QuantityToGive;

                Dictionary<string, int> OriginalShopPrices = new Dictionary<string, int>() {
                    { "Shop - Potion 1", 300 },
                    { "Shop - Potion 2", 1000 },
                    { "Shop - Coin 1", 999 },
                    { "Shop - Coin 2", 999 }
                };
                // If buying your own money item from the shop, increase amount rewarded
                if (itemInfo.LocationName != null && OriginalShopPrices.ContainsKey(itemInfo.LocationName) && (itemInfo.Player == Archipelago.instance.GetPlayerSlot())) {
                    AmountToGive += TunicRandomizer.Settings.CheaperShopItemsEnabled ? 300 : OriginalShopPrices[itemInfo.LocationName];
                }

                if (TunicRandomizer.Settings.SkipItemAnimations) {
                    MoneyGoUp.IncrementMoneyGoUp(AmountToGive);
                    Inventory.GetItemByName("MoneySmall").Quantity += AmountToGive;
                } else {
                    CoinSpawner.SpawnCoins(AmountToGive, PlayerCharacter.instance.transform.position);
                }
            }

            if (Item.Type == ItemTypes.INVENTORY || Item.Type == ItemTypes.TRINKET || Item.Type == ItemTypes.LADDER || Item.Type == ItemTypes.FUSE) {
                Item InventoryItem = Inventory.GetItemByName(Item.ItemNameForInventory);
                InventoryItem.Quantity += Item.QuantityToGive;
                if (Item.Name == "Stick" || Item.Name == "Sword") {
                    InventoryItem.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    InventoryItem.collectionMessage.text = $"fownd ahn Itehm!";
                }
                if (Item.Name == "Dath Stone") {
                    Inventory.GetItemByName("Torch").Quantity = 1;
                }
                if (Item.ItemNameForInventory == "Hyperdash") {
                    Inventory.GetItemByName("Hyperdash Toggle").Quantity = 1;
                }
                if (Item.Type == ItemTypes.LADDER) {
                    InventoryItem.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    InventoryItem.collectionMessage.text = TunicRandomizer.Settings.UseTrunicTranslations ? Translations.Translate(Item.Name, false) : $"\"{LadderToggles.LadderCollectionMessages[Item.Name]}\"";
                }
                if (Item.Type == ItemTypes.FUSE) {
                    FuseInformation fuseInformation = FuseRandomizer.GetFuseInformationByFuseItem(Item.Name);
                    if (fuseInformation.RealGuid != 0) {
                        if (GetBool(FuseShuffleEnabled)) {
                            SaveFile.SetInt($"fuseClosed {fuseInformation.RealGuid}", 1);
                            FuseRandomizer.UpdateFuseVisualState(fuseInformation.RealGuid);
                            NotificationBottom = $"yoo hEr uh strAnj huhm...";
                        } else {
                            NotificationBottom = $"\"...but nothing happened.\"";
                        }
                    }
                }
                ItemPresentation.PresentItem(InventoryItem, Item.QuantityToGive);
                if (TunicRandomizer.Settings.SkipItemAnimations && Item.Name == "Flask Shard" && Inventory.GetItemByName("Flask Shard").Quantity >= 3) {
                    Inventory.GetItemByName("Flask Shard").Quantity -= 3;
                    Inventory.GetItemByName("Flask Container").Quantity += 1;
                    NotificationBottom = $"kuhmplEtid A flahsk! \"<#f03c67>+1\" [flask]\"<#f03c67>!\"";
                }
            }

            if (Item.Type == ItemTypes.SWORDUPGRADE) {

                if (SaveFile.GetInt(SwordProgressionEnabled) == 1 && Item.Name == "Sword Upgrade") {
                    int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                    SwordProgression.UpgradeSword(SwordLevel + 1);
                    itemDisplay = TextBuilderPatches.GetSwordIconName(SwordLevel + 1);
                }
            }

            if (Item.Type == ItemTypes.FAIRY) {
                foreach(string Fairy in ItemLookup.FairyLookup.Keys) { 
                    if(SaveFile.GetInt($"randomizer obtained fairy {Fairy}") == 0) {
                        SaveFile.SetInt($"randomizer obtained fairy {Fairy}", 1);
                        break;
                    }
                }
                GameObject.Instantiate(ModelSwaps.FairyAnimation, PlayerCharacter.instance.transform.position, Quaternion.identity).SetActive(true);
                NotificationBottom = $"\"{(TunicRandomizer.Tracker.ImportantItems["Fairies"] + 1)}/20\" fArEz fownd.";
            }

            if (Item.Type == ItemTypes.PAGE) {
                SaveFile.SetInt($"randomizer obtained page {Item.ItemNameForInventory}", 1);
                if (SaveFile.GetInt(AbilityShuffle) == 1) {
                    Dictionary<string, (string, string, string)> pagesForAbilities = new Dictionary<string, (string, string, string)>() {
                        { "12", (PrayerUnlocked, PrayerUnlockedTime, ItemLookup.PrayerUnlockedLine) },
                        { "21", (HolyCrossUnlocked, HolyCrossUnlockedTime, ItemLookup.HolyCrossUnlockedLine) },
                        { "26", (IceBoltUnlocked, IceboltUnlockedTime, ItemLookup.IceboltUnlockedLine) },
                    };
                    if (pagesForAbilities.ContainsKey(Item.ItemNameForInventory)) {
                        SaveFile.SetInt(pagesForAbilities[Item.ItemNameForInventory].Item1, 1);
                        SaveFile.SetFloat(pagesForAbilities[Item.ItemNameForInventory].Item2, SpeedrunData.inGameTime);
                        NotificationBottom = pagesForAbilities[Item.ItemNameForInventory].Item3;
                        DisplayMessageAnyway = true;
                        if (Item.ItemNameForInventory == "21") {
                            ToggleHolyCrossObjects(true);
                        }
                        InventoryDisplayPatches.UpdateAbilitySection();
                    }
                }
                if (!TunicRandomizer.Settings.SkipItemAnimations) {
                    PageDisplay.ShowPage(int.Parse(Item.ItemNameForInventory, CultureInfo.InvariantCulture));
                } else {
                    SaveFile.SetInt("last page viewed", int.Parse(Item.ItemNameForInventory, CultureInfo.InvariantCulture));
                }
            }

            if (Item.Type == ItemTypes.GOLDENTROPHY) {

                Item GoldenTrophy = Inventory.GetItemByName(Item.ItemNameForInventory);
                GoldenTrophy.Quantity += Item.QuantityToGive;
                // Apply bonus upgrade text
                if (TunicRandomizer.Settings.BonusStatUpgradesEnabled) {
                    GoldenTrophy.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    GoldenTrophy.collectionMessage.text = Translations.TranslateDefaultNoQuotes(ItemLookup.BonusUpgrades[Item.ItemNameForInventory].CustomPickupMessage, true);
                    string bonusUpgrade = ItemLookup.BonusUpgrades[Item.ItemNameForInventory].LevelUp;
                    Inventory.GetItemByName(bonusUpgrade).Quantity += 1;
                    string saveFlag = $"randomizer bonus upgrade {bonusUpgrade}";
                    SaveFile.SetInt(saveFlag, SaveFile.GetInt(saveFlag) + 1);
                } else {
                    GoldenTrophy.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    GoldenTrophy.collectionMessage.text = $"kawngrahJoulA$uhnz!";
                }

                ItemPresentation.PresentItem(GoldenTrophy);
                NotificationBottom = TunicRandomizer.Settings.BonusStatUpgradesEnabled ? Translations.TranslateDefaultNoQuotes(ItemLookup.BonusUpgrades[Item.ItemNameForInventory].CustomPickupMessage, true) : $"kawngrahJoulA$uhnz!";
            }

            if (Item.Type == ItemTypes.RELIC) {

                Item RelicItem = Inventory.GetItemByName(Item.ItemNameForInventory);
                RelicItem.Quantity += Item.QuantityToGive;
                if (TunicRandomizer.Settings.BonusStatUpgradesEnabled) {
                    string bonusUpgrade = ItemLookup.BonusUpgrades[Item.ItemNameForInventory].LevelUp;
                    Inventory.GetItemByName(bonusUpgrade).Quantity += 1;
                    string saveFlag = $"randomizer bonus upgrade {bonusUpgrade}";
                    SaveFile.SetInt(saveFlag, SaveFile.GetInt(saveFlag) + 1);
                }

                // Apply custom pickup text
                RelicItem.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                RelicItem.collectionMessage.text = Translations.Translate(ItemLookup.BonusUpgrades[Item.ItemNameForInventory].CustomPickupMessage, true);
                
                ItemPresentation.PresentItem(RelicItem);
            }

            if (Item.Type == ItemTypes.FOOLTRAP) {
                (NotificationTop, NotificationBottom) = FoolTrap.ApplyRandomFoolEffect(itemInfo.Player);
                DisplayMessageAnyway = true;
            }

            if(Item.Type == ItemTypes.HEXAGONQUEST) {
                Inventory.GetItemByName("Hexagon Gold").Quantity += 1;
                int GoldHexes = Inventory.GetItemByName("Hexagon Gold").Quantity;

                if (IsHexQuestWithHexAbilities()) {
                    Dictionary<int, (string, string, string)> hexesForAbilities = new Dictionary<int, (string, string, string)>() {
                        { SaveFile.GetInt(HexagonQuestPrayer), (PrayerUnlocked, PrayerUnlockedTime, ItemLookup.PrayerUnlockedLine) },
                        { SaveFile.GetInt(HexagonQuestHolyCross), (HolyCrossUnlocked, HolyCrossUnlockedTime, ItemLookup.HolyCrossUnlockedLine) },
                        { SaveFile.GetInt(HexagonQuestIcebolt), (IceBoltUnlocked, IceboltUnlockedTime, ItemLookup.IceboltUnlockedLine) },
                    };
                    if (hexesForAbilities.ContainsKey(GoldHexes)) {
                        SaveFile.SetInt(hexesForAbilities[GoldHexes].Item1, 1);
                        SaveFile.SetFloat(hexesForAbilities[GoldHexes].Item2, SpeedrunData.inGameTime);
                        NotificationBottom = hexesForAbilities[GoldHexes].Item3;
                        DisplayMessageAnyway = true;
                        if (GoldHexes == SaveFile.GetInt(HexagonQuestHolyCross)) {
                            ToggleHolyCrossObjects(true);
                        }

                        InventoryDisplayPatches.UpdateAbilitySection();
                    }
                }

                ItemPresentation.PresentItem(Inventory.GetItemByName(Item.ItemNameForInventory));
            }

            if (ItemLookup.MajorItems.Contains(Item.Name)) {
                if (Item.Type == ItemTypes.SWORDUPGRADE && SaveFile.GetInt(SwordProgressionEnabled) == 1) {
                    SaveFile.SetFloat($"randomizer Sword Progression {SaveFile.GetInt(SwordProgressionLevel)} time", SpeedrunData.inGameTime);
                }
                if (Item.Type == ItemTypes.PAGE) {
                    SaveFile.SetFloat($"randomizer {Item.Name} 1 time", SpeedrunData.inGameTime);
                } else {
                    SaveFile.SetFloat($"randomizer {Item.Name} {(TunicRandomizer.Tracker.ImportantItems[Item.ItemNameForInventory] + 1)} time", SpeedrunData.inGameTime);
                }
            }

            if (itemInfo.Player != Archipelago.instance.GetPlayerSlot()) {
                var sender = itemInfo.Player.Name;
                NotificationTop = NotificationTop == "" ? $"\"{sender}\" sehnt yoo  {itemDisplay}  \"{ItemName}!\"" : NotificationTop;
                NotificationBottom = NotificationBottom == "" ? $"Rnt #A nIs\"?\"" : NotificationBottom;
                Notifications.Show(NotificationTop, NotificationBottom);
            }

            if (itemInfo.Player == Archipelago.instance.GetPlayerSlot() && (TunicRandomizer.Settings.SkipItemAnimations || DisplayMessageAnyway)) {
                NotificationTop = NotificationTop == "" ? $"yoo fownd  {itemDisplay}  \"{ItemName}!\"" : NotificationTop;
                NotificationBottom = NotificationBottom == "" ? $"$oud bE yoosfuhl!" : NotificationBottom;
                Notifications.Show(NotificationTop, NotificationBottom);
            }

            string slotLoc = $"{itemInfo.Player.Slot}, {itemInfo.LocationName}";
            if (Hints.HeroGraveHints.Values.Where(hint => hint.PathHintId == slotLoc || hint.RelicHintId == slotLoc).Any()) {
                SaveFile.SetInt($"randomizer hint found {slotLoc}", 1);
            }
            if (Hints.HeroGraveHints.Values.Where(hint => SaveFile.GetInt($"randomizer hint found {hint.PathHintId}") == 1).Count() == 6) {
                StateVariable.GetStateVariableByName("randomizer got all 6 grave items").BoolValue = true;
            }

            TunicRandomizer.Tracker.SetCollectedItem(ItemName, true);

            if (SaveFile.GetInt(GrassRandoEnabled) == 0) {
                FairyTargets.UpdateFairyTargetsInLogic(ItemName);
            }

            if (TunicRandomizer.Settings.ShowItemsEnabled && Item.Type == ItemTypes.SWORDUPGRADE) {
                ModelSwaps.SwapItemsInScene();
            }

            TunicRandomizer.Settings.SkipItemAnimations = SkipAnimationsValue;

            RecentItemsDisplay.instance.EnqueueItem(itemInfo, true);

            return ItemResult.Success;
        }

        public static void GiveItem(Check Check, bool alwaysSkip = false) {

            string NotificationTop = "";
            string NotificationBottom = "";
            bool DisplayMessageAnyway = false;

            bool SkipAnimationsValue = TunicRandomizer.Settings.SkipItemAnimations;

            if (alwaysSkip) {
                TunicRandomizer.Settings.SkipItemAnimations = true;
            }

            ItemData Item = ItemLookup.GetItemDataFromCheck(Check);
            string itemDisplay = TextBuilderPatches.ItemNameToAbbreviation.ContainsKey(Item.Name) ? TextBuilderPatches.ItemNameToAbbreviation[Item.Name] : "";

            if (Item.Type == ItemTypes.GRASS) {
                ItemPresentation.PresentItem(Inventory.GetItemByName("Grass"));
                Inventory.GetItemByName("Grass").Quantity += 1;
            }

            if (Item.Type == ItemTypes.MONEY) {
                int AmountToGive = Check.Reward.Amount;

                Dictionary<string, int> OriginalShopPrices = new Dictionary<string, int>() {
                    { "Potion (First)", 300 },
                    { "Potion (West Garden)", 1000 },
                    { "Trinket Coin 1 (day)", 999 },
                    { "Trinket Coin 2 (night)", 999 }
                };
                if (OriginalShopPrices.ContainsKey(Check.Location.LocationId)) {
                    AmountToGive += TunicRandomizer.Settings.CheaperShopItemsEnabled ? 300 : OriginalShopPrices[Check.Location.LocationId];
                }

                if (TunicRandomizer.Settings.SkipItemAnimations) {
                    MoneyGoUp.IncrementMoneyGoUp(AmountToGive);
                    Inventory.GetItemByName("MoneySmall").Quantity += AmountToGive;
                } else {
                    CoinSpawner.SpawnCoins(AmountToGive, PlayerCharacter.instance.transform.position);
                }
            }

            if (Item.Type == ItemTypes.INVENTORY || Item.Type == ItemTypes.TRINKET || Item.Type == ItemTypes.LADDER || Item.Type == ItemTypes.FUSE) {
                Item InventoryItem = Inventory.GetItemByName(Item.ItemNameForInventory);
                InventoryItem.Quantity += Check.Reward.Amount;
                if (Item.Name == "Stick" || Item.Name == "Sword") {
                    InventoryItem.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    InventoryItem.collectionMessage.text = $"fownd ahn Itehm!";
                }
                if (Item.Name == "Dath Stone") {
                    Inventory.GetItemByName("Torch").Quantity = 1;
                }
                if (Item.ItemNameForInventory == "Hyperdash") {
                    Inventory.GetItemByName("Hyperdash Toggle").Quantity = 1;
                }
                if (Item.Type == ItemTypes.LADDER) {
                    InventoryItem.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    InventoryItem.collectionMessage.text = TunicRandomizer.Settings.UseTrunicTranslations ? Translations.Translate(Item.Name, false) : $"\"{LadderToggles.LadderCollectionMessages[Item.Name]}\"";
                }
                if (Item.Type == ItemTypes.FUSE) {
                    FuseInformation fuseInformation = FuseRandomizer.GetFuseInformationByFuseItem(Item.Name);
                    if (fuseInformation.RealGuid != 0) {
                        if (GetBool(FuseShuffleEnabled)) {
                            SaveFile.SetInt($"fuseClosed {fuseInformation.RealGuid}", 1);
                            FuseRandomizer.UpdateFuseVisualState(fuseInformation.RealGuid);
                            NotificationBottom = $"yoo hEr uh strAnj huhm...";
                        } else {
                            NotificationBottom = $"\"...but nothing happened.\"";
                        }
                    }
                }
                ItemPresentation.PresentItem(InventoryItem, Check.Reward.Amount);
                if (TunicRandomizer.Settings.SkipItemAnimations && Item.Name == "Flask Shard" && Inventory.GetItemByName("Flask Shard").Quantity >= 3) {
                    Inventory.GetItemByName("Flask Shard").Quantity -= 3;
                    Inventory.GetItemByName("Flask Container").Quantity += 1;
                    NotificationBottom = $"kuhmplEtid A flahsk! \"<#f03c67>+1\" [flask]\"<#f03c67>!\"";
                }
            }

            if (Item.Type == ItemTypes.SWORDUPGRADE) {

                if (SaveFile.GetInt(SwordProgressionEnabled) == 1 && Item.Name == "Sword Upgrade") {
                    int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                    SwordProgression.UpgradeSword(SwordLevel + 1);
                    itemDisplay = TextBuilderPatches.GetSwordIconName(SwordLevel + 1);
                }
            }

            if (Item.Type == ItemTypes.FAIRY) {
                foreach (string Fairy in ItemLookup.FairyLookup.Keys) {
                    if (SaveFile.GetInt($"randomizer obtained fairy {Fairy}") == 0) {
                        SaveFile.SetInt($"randomizer obtained fairy {Fairy}", 1);
                        break;
                    }
                }
                GameObject.Instantiate(ModelSwaps.FairyAnimation, PlayerCharacter.instance.transform.position, Quaternion.identity).SetActive(true);
                NotificationBottom = $"\"{(TunicRandomizer.Tracker.ImportantItems["Fairies"] + 1)}/20\" fArEz fownd.";
            }

            if (Item.Type == ItemTypes.PAGE) {
                SaveFile.SetInt($"randomizer obtained page {Item.ItemNameForInventory}", 1);
                if (SaveFile.GetInt(AbilityShuffle) == 1) {
                    Dictionary<string, (string, string, string)> pagesForAbilities = new Dictionary<string, (string, string, string)>() {
                        { "12", (PrayerUnlocked, PrayerUnlockedTime, ItemLookup.PrayerUnlockedLine) },
                        { "21", (HolyCrossUnlocked, HolyCrossUnlockedTime, ItemLookup.HolyCrossUnlockedLine) },
                        { "26", (IceBoltUnlocked, IceboltUnlockedTime, ItemLookup.IceboltUnlockedLine) },
                    };
                    if (pagesForAbilities.ContainsKey(Item.ItemNameForInventory)) {
                        SaveFile.SetInt(pagesForAbilities[Item.ItemNameForInventory].Item1, 1);
                        SaveFile.SetFloat(pagesForAbilities[Item.ItemNameForInventory].Item2, SpeedrunData.inGameTime);
                        NotificationBottom = pagesForAbilities[Item.ItemNameForInventory].Item3;
                        DisplayMessageAnyway = true;
                        if (Item.ItemNameForInventory == "21") {
                            ToggleHolyCrossObjects(true);
                        }
                        InventoryDisplayPatches.UpdateAbilitySection();
                    }
                }
                if (!TunicRandomizer.Settings.SkipItemAnimations) {
                    PageDisplay.ShowPage(int.Parse(Item.ItemNameForInventory, CultureInfo.InvariantCulture));
                } else {
                    SaveFile.SetInt("last page viewed", int.Parse(Item.ItemNameForInventory, CultureInfo.InvariantCulture));
                }
            }
            
            if (Item.Type == ItemTypes.GOLDENTROPHY) {

                Item GoldenTrophy = Inventory.GetItemByName(Item.ItemNameForInventory);
                GoldenTrophy.Quantity += Check.Reward.Amount;
                // Apply bonus upgrade text
                if (TunicRandomizer.Settings.BonusStatUpgradesEnabled) {
                    GoldenTrophy.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    GoldenTrophy.collectionMessage.text = Translations.TranslateDefaultNoQuotes(ItemLookup.BonusUpgrades[Item.ItemNameForInventory].CustomPickupMessage, true);
                    string bonusUpgrade = ItemLookup.BonusUpgrades[Item.ItemNameForInventory].LevelUp;
                    Inventory.GetItemByName(bonusUpgrade).Quantity += 1;
                    string saveFlag = $"randomizer bonus upgrade {bonusUpgrade}";
                    SaveFile.SetInt(saveFlag, SaveFile.GetInt(saveFlag) + 1);
                } else {
                    GoldenTrophy.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    GoldenTrophy.collectionMessage.text = $"kawngrahJoulA$uhnz!";
                }

                ItemPresentation.PresentItem(GoldenTrophy);
                NotificationBottom = TunicRandomizer.Settings.BonusStatUpgradesEnabled ? Translations.TranslateDefaultNoQuotes(ItemLookup.BonusUpgrades[Item.ItemNameForInventory].CustomPickupMessage, true) : $"kawngrahJoulA$uhnz!";
            }

            if (Item.Type == ItemTypes.RELIC) {

                Item RelicItem = Inventory.GetItemByName(Item.ItemNameForInventory);
                RelicItem.Quantity += Check.Reward.Amount;
                if (TunicRandomizer.Settings.BonusStatUpgradesEnabled) {
                    string bonusUpgrade = ItemLookup.BonusUpgrades[Item.ItemNameForInventory].LevelUp;
                    Inventory.GetItemByName(bonusUpgrade).Quantity += 1;
                    string saveFlag = $"randomizer bonus upgrade {bonusUpgrade}";
                    SaveFile.SetInt(saveFlag, SaveFile.GetInt(saveFlag) + 1);
                }

                // Apply custom pickup text
                RelicItem.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                RelicItem.collectionMessage.text = Translations.Translate(ItemLookup.BonusUpgrades[Item.ItemNameForInventory].CustomPickupMessage, true);

                ItemPresentation.PresentItem(RelicItem);
            }

            if (Item.Type == ItemTypes.FOOLTRAP) {
                (NotificationTop, NotificationBottom) = FoolTrap.ApplyRandomFoolEffect(-1);
                DisplayMessageAnyway = true;
            }

            if (Item.Type == ItemTypes.HEXAGONQUEST) {
                Inventory.GetItemByName("Hexagon Gold").Quantity += 1;
                int GoldHexes = Inventory.GetItemByName("Hexagon Gold").Quantity;

                if (IsHexQuestWithHexAbilities()) {
                    Dictionary<int, (string, string, string)> hexesForAbilities = new Dictionary<int, (string, string, string)>() {
                        { SaveFile.GetInt(HexagonQuestPrayer), (PrayerUnlocked, PrayerUnlockedTime, ItemLookup.PrayerUnlockedLine) },
                        { SaveFile.GetInt(HexagonQuestHolyCross), (HolyCrossUnlocked, HolyCrossUnlockedTime, ItemLookup.HolyCrossUnlockedLine) },
                        { SaveFile.GetInt(HexagonQuestIcebolt), (IceBoltUnlocked, IceboltUnlockedTime, ItemLookup.IceboltUnlockedLine) },
                    };
                    if (hexesForAbilities.ContainsKey(GoldHexes)) {
                        SaveFile.SetInt(hexesForAbilities[GoldHexes].Item1, 1);
                        SaveFile.SetFloat(hexesForAbilities[GoldHexes].Item2, SpeedrunData.inGameTime);
                        NotificationBottom = hexesForAbilities[GoldHexes].Item3;
                        DisplayMessageAnyway = true;
                        if (GoldHexes == SaveFile.GetInt(HexagonQuestHolyCross)) {
                            ToggleHolyCrossObjects(true);
                        }

                        InventoryDisplayPatches.UpdateAbilitySection();
                    }
                }

                ItemPresentation.PresentItem(Inventory.GetItemByName(Item.ItemNameForInventory));
            }

            if (ItemLookup.MajorItems.Contains(Item.Name)) {
                if (Item.Type == ItemTypes.SWORDUPGRADE && SaveFile.GetInt(SwordProgressionEnabled) == 1) {
                    SaveFile.SetFloat($"randomizer Sword Progression {SaveFile.GetInt(SwordProgressionLevel)} time", SpeedrunData.inGameTime);
                } else if (Item.Type == ItemTypes.PAGE) {
                    SaveFile.SetFloat($"randomizer {Item.Name} 1 time", SpeedrunData.inGameTime);
                } else {
                    SaveFile.SetFloat($"randomizer {Item.Name} {(TunicRandomizer.Tracker.ImportantItems[Item.ItemNameForInventory] + 1)} time", SpeedrunData.inGameTime);
                }
            }

            if (TunicRandomizer.Settings.SkipItemAnimations || DisplayMessageAnyway) {
                NotificationTop = NotificationTop == "" ? $"yoo fownd  {itemDisplay}  \"{Item.Name}!\"" : NotificationTop;
                NotificationBottom = NotificationBottom == "" ? $"$oud bE yoosfuhl!" : NotificationBottom;
                Notifications.Show(NotificationTop, NotificationBottom);
            }

            string slotLoc = Check.CheckId;
            if (Hints.HeroGraveHints.Values.Where(hint => hint.PathHintId == slotLoc || hint.RelicHintId == slotLoc).Any()) {
                SaveFile.SetInt($"randomizer hint found {slotLoc}", 1);
            }
            if (Hints.HeroGraveHints.Values.Where(hint => SaveFile.GetInt($"randomizer hint found {hint.PathHintId}") == 1).Count() == 6) {
                StateVariable.GetStateVariableByName("randomizer got all 6 grave items").BoolValue = true;
            }

            TunicRandomizer.Tracker.SetCollectedItem(Item.Name, true);

            string CheckId = Check.CheckId;
            TunicLogger.LogInfo("Picked up item " + CheckId + " (" + Item.Name + ")");

            Locations.CheckedLocations[CheckId] = true;
            SaveFile.SetInt($"randomizer picked up {CheckId}", 1);
            GameObject FairyTarget = GameObject.Find($"fairy target {CheckId}");
            if (FairyTarget != null) {
                GameObject.Destroy(FairyTarget);
            }

            RecentItemsDisplay.instance.EnqueueItem(Check);

            if (TunicRandomizer.Settings.ShowItemsEnabled && Item.Type == ItemTypes.SWORDUPGRADE) {
                ModelSwaps.SwapItemsInScene();
            }
            FairyTargets.UpdateFairyTargetsInLogic(ItemLookup.SimplifiedItemNames[Check.Reward.Name]);
            TunicRandomizer.Settings.SkipItemAnimations = SkipAnimationsValue;
            if (SaveFile.GetInt(GrassRandoEnabled) == 0 && TunicRandomizer.Settings.CreateSpoilerLog && !TunicRandomizer.Settings.RaceMode) {
                ItemTracker.PopulateSpoilerLog();
            }
        }

        public static void ToggleHolyCrossObjects(bool isEnabled) {
            foreach (ToggleObjectBySpell SpellToggle in Resources.FindObjectsOfTypeAll<ToggleObjectBySpell>().Where(toggle => toggle.GetComponent<AllowHolyCross>() == null)) {
                foreach (ToggleObjectBySpell Spell in SpellToggle.gameObject.GetComponents<ToggleObjectBySpell>()) {
                    Spell.enabled = isEnabled;
                }
            }
        }

        public static void PotionCombine_Show_PostFixPatch(PotionCombine __instance) {
            //TunicRandomizer.Tracker.ImportantItems["Flask Container"]++;
            //ItemTracker.SaveTrackerFile();
        }

        public static void ButtonAssignableItem_CheckFreeItemSpell_PostfixPatch(ButtonAssignableItem __instance, ref string s) {
            if (ItemLookup.BombCodes.ContainsKey(s) && StateVariable.GetStateVariableByName(ItemLookup.BombCodes[s]).BoolValue) {
                if (SaveFile.GetInt($"randomizer used free bomb code {s}") == 0) {
                    if (IsArchipelago()) {
                        Archipelago.instance.UpdateDataStorage(ItemLookup.BombCodes[s], true);
                    }
                    SaveFile.SetInt($"randomizer used free bomb code {s}", 1);
                    if (TunicRandomizer.Settings.SkipItemAnimations) { 
                        switch(ItemLookup.BombCodes[s]) {
                            case "Granted Firecracker":
                                Notifications.Show($"[firecracker] \"Firecracker Granted!\"", $"mAd fruhm slorm, #uh poudi^ #aht gOz boom.");
                                break;
                            case "Granted Firebomb":
                                Notifications.Show($"[firebomb] \"Fire Bomb Granted!\"", $"fIur fIur ehvurEwAr, ahnd ow ow ow ow ow.");
                                break;
                            case "Granted Icebomb":
                                Notifications.Show($"[icebomb] \"Ice Bomb Granted!\"", $"uhnstAboul powdur mAd fruhm #uh fArE uhv #uh wehst gRdin.");
                                break;
                            default:
                                break;
                        }
                    }
                    PlayerCharacter.instance.GetComponent<DDRSpell>().CompletedSpellEffect();
                }
            }
            ButtonAssignableItem cape = Inventory.GetItemByName("Cape").TryCast<ButtonAssignableItem>();
            if (!cape.freeItemCountStateVar.BoolValue && s == cape.spellStringForFreeItem) {
                PlayerCharacter.instance.GetComponent<DDRSpell>().CompletedSpellEffect();
            }
        }

        public static bool UpgradeMenu___Buy_PrefixPatch(UpgradeMenu __instance) {
            
            if (TunicRandomizer.Settings.RaceMode && TunicRandomizer.Settings.DisableUpgradeStealing && UpgradeAltar.nearbyThings.Count == 0) {
                UpgradeMenu.instance.__Exit();
                return false;
            }

            return true;
        }

        public static bool UpgradeAltar_DoOfferingSequence_PrefixPatch(UpgradeAltar __instance, OfferingItem offeringItemToOffer) {
            
            if (TunicRandomizer.Settings.FasterUpgrades) {
                Notifications.Show($"{TextBuilderPatches.SpriteNameToAbbreviation[offeringItemToOffer.icon.name]} \"{offeringItemToOffer.statLabelLocKey}\" wehnt uhp fruhm {offeringItemToOffer.upgradeItemReceived.Quantity} [arrow_right] {offeringItemToOffer.upgradeItemReceived.Quantity+1}!", $"#E Ar ahksehpts yor awfuri^.");
                UpgradePresentation.instance.afterEnable().MoveNext();
                UpgradeMenu.instance.__Exit();
                return false;
            }

            return true;
        }

        public static void UpgradeAltar_DoOfferingSequence_PostfixPatch(UpgradeAltar __instance) {
            foreach (string LevelUp in ItemLookup.LevelUpItems) {
                TunicRandomizer.Tracker.ImportantItems[LevelUp] = Inventory.GetItemByName(LevelUp).Quantity;
            }
        }

        public static void OfferingItem_PriceForNext_PostfixPatch(OfferingItem __instance, ref int __result) {
            int freeUpgradeCount = SaveFile.GetInt($"randomizer bonus upgrade {__instance.upgradeItemReceived.name}");
            // potion upgrades cost 100 for the first, 300 for the second, 1000 for the third, and +200 for each one after the third
            if (__instance.name == "Upgrade Offering - PotionEfficiency Swig - Ash"
                && __result >= 1000 && freeUpgradeCount >= (__result - 800) / 200) {
                __result -= Mathf.RoundToInt(500 + 200 * freeUpgradeCount);
            } else {
                __result -= Mathf.RoundToInt(__instance.priceIncreasePerLevelup * (float)freeUpgradeCount);
            }
        }

        public static bool FairyCollection_getFairyCount_PrefixPatch(FairyCollection __instance, ref int __result) {
            __result = TunicRandomizer.Tracker.ImportantItems["Fairies"];
            return false;
        }

        // Extender for debug/cheat function
        public static void Cheats_giveLotsOfItems_PostfixPatch(Cheats __instance) {
            Inventory.GetItemByName("Librarian Sword").Quantity = 1;
            Inventory.GetItemByName("Heir Sword").Quantity = 1;
            Inventory.GetItemByName("Dath Stone").Quantity = 1;
            Inventory.GetItemByName("Torch").Quantity = 1;
            Inventory.GetItemByName("Bait").Quantity = 99;
            Inventory.GetItemByName("Berry_MP").Quantity = 99;
            Inventory.GetItemByName("Berry_HP").Quantity = 99;
            Inventory.GetItemByName("Ivy").Quantity = 99;
            Inventory.GetItemByName("Pepper").Quantity = 99;
            Inventory.GetItemByName("Ice Bomb").Quantity = 99;
            Inventory.GetItemByName("Piggybank L1").Quantity = 99;
            Inventory.GetItemByName("Trinket Coin").Quantity = 17;
            Inventory.GetItemByName("Hyperdash").Quantity = 1;
            Inventory.GetItemByName("Hyperdash Toggle").Quantity = 1;
            Inventory.GetItemByName("Key (House)").Quantity = 1;
            Inventory.GetItemByName("Vault Key (Red)").Quantity = 1;
        }
    }
}
