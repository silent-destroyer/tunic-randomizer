﻿using System;
using System.Collections.Generic;
using UnityEngine;
using BepInEx.Logging;
using UnityEngine.UI;
using System.Linq;
using System.Globalization;
using UnityEngine.SceneManagement;

namespace TunicRandomizer {
    public class ItemPatches {
        private static ManualLogSource Logger = TunicRandomizer.Logger;

        public struct BonusUpgrade {
            public string LevelUp;
            public string CustomPickupMessage;

            public BonusUpgrade(string levelUp, string customPickupMessage) {
                LevelUp = levelUp;
                CustomPickupMessage = customPickupMessage;
            }
        }

        public static Dictionary<string, ItemData> ItemList = new Dictionary<string, ItemData>();
        public static Dictionary<string, bool> ItemsPickedUp = new Dictionary<string, bool>();
        public static Dictionary<string, Fairy> FairyLookup = new Dictionary<string, Fairy>() {
            {"Overworld Redux-(64.5, 44.0, -40.0)", new Fairy("SV_Fairy_1_Overworld_Flowers_Upper_Opened", $"flowurz \"1\"")},
            {"Overworld Redux-(-52.0, 2.0, -174.8)", new Fairy("SV_Fairy_2_Overworld_Flowers_Lower_Opened", $"flowurz \"2\"")},
            {"Overworld Redux-(-132.0, 28.0, -55.5)", new Fairy("SV_Fairy_3_Overworld_Moss_Opened", $"maws")},
            {"Overworld Cave-(-90.4, 515.0, -738.9)", new Fairy("SV_Fairy_4_Caustics_Opened", $"kawstik lIt")},
            {"Waterfall-(-47.0, 45.0, 10.0)", new Fairy("SV_Fairy_5_Waterfall_Opened", $"\"SECRET GATHERING PLACE\"")},
            {"Temple-(14.0, 0.1, 42.4)", new Fairy("SV_Fairy_6_Temple_Opened", $"\"SEALED TEMPLE\"")},
            {"Quarry Redux-(0.7, 68.0, 84.7)", new Fairy("SV_Fairy_7_Quarry_Opened", $"\"THE QUARRY\"")},
            {"East Forest Redux-(104.0, 16.0, 61.0)", new Fairy("SV_Fairy_8_Dancer_Opened", $"\"EAST FOREST\"")},
            {"Library Hall-(133.3, 10.0, -43.2)", new Fairy("SV_Fairy_9_Library_Rug_Opened", $"\"THE GREAT LIBRARY\"")},
            {"Town Basement-(-202.0, 28.0, 150.0)", new Fairy("SV_Fairy_10_3DPillar_Opened", $"mAz (kawluhm)")},
            {"Overworld Redux-(90.4, 36.0, -122.1)", new Fairy("SV_Fairy_11_WeatherVane_Opened", $"vAn")},
            {"Overworld Interiors-(-28.0, 27.0, -50.5)", new Fairy("SV_Fairy_12_House_Opened", $"hOs (hows)")},
            {"PatrolCave-(74.0, 46.0, 24.0)", new Fairy("SV_Fairy_13_Patrol_Opened", $"puhtrOl")},
            {"CubeRoom-(321.1, 3.0, 217.0)", new Fairy("SV_Fairy_14_Cube_Opened", $"kyoob")},
            {"Maze Room-(1.0, 0.0, -1.0)", new Fairy("SV_Fairy_15_Maze_Opened", $"mAz (inviziboul)")},
            {"Overworld Redux-(-83.0, 20.0, -117.5)", new Fairy("SV_Fairy_16_Fountain_Opened", $"fowntin")},
            {"Archipelagos Redux-(-396.3, 1.4, 42.3)", new Fairy("SV_Fairy_17_GardenTree_Opened", $"\"WEST GARDEN\"")},
            {"Archipelagos Redux-(-236.0, 8.0, 86.3)", new Fairy("SV_Fairy_18_GardenCourtyard_Opened", $"\"WEST GARDEN\"")},
            {"Fortress Main-(-75.0, -1.0, 17.0)", new Fairy("SV_Fairy_19_FortressCandles_Opened", $"\"FORTRESS OF THE EASTERN VAULT\"")},
            {"East Forest Redux-(164.0, -25.0, -56.0)", new Fairy("SV_Fairy_20_ForestMonolith_Opened", $"\"EAST FOREST\"")}
        };
        public static Dictionary<string, HeroRelic> HeroRelicLookup = new Dictionary<string, HeroRelic>() {
            {"Relic - Hero Pendant SP", new HeroRelic("SV_RelicVoid_Got_Pendant_SP", "Upgrade Offering - Stamina SP - Feather", "Hero Relic - <#8ddc6e>SP", "Relic PIckup (1) (SP) [RelicVoid]", "Level Up - Stamina")},
            {"Relic - Hero Crown", new HeroRelic("SV_RelicVoid_Got_Crown_DEF", "Upgrade Offering - DamageResist - Effigy", "Hero Relic - <#5de7cf>DEF", "Relic PIckup (2) (Crown) [RelicVoid]", "Level Up - DamageResist")},
            {"Relic - Hero Pendant HP", new HeroRelic("SV_RelicVoid_Got_Pendant_HP", "Upgrade Offering - Health HP - Flower", "Hero Relic - <#f03c67>HP", "Relic PIckup (3) (HP) [RelicVoid]", "Level Up - Health")},
            {"Relic - Hero Water", new HeroRelic("SV_RelicVoid_Got_Water_POT", "Upgrade Offering - PotionEfficiency Swig - Ash", "Hero Relic - <#ca7be4>POTION", "Relic PIckup (4) (water) [RelicVoid]", "Level Up - PotionEfficiency")},
            {"Relic - Hero Pendant MP", new HeroRelic("SV_RelicVoid_Got_Pendant_MP", "Upgrade Offering - Magic MP - Mushroom", "Hero Relic - <#2a8fed>MP", "Relic PIckup (5) (MP) [RelicVoid]", "Level Up - Magic")},
            {"Relic - Hero Sword", new HeroRelic("SV_RelicVoid_Got_Sword_ATT", "Upgrade Offering - Attack - Tooth", "Hero Relic - <#e99d4c>ATT", "Relic PIckup (6) Sword) [RelicVoid]", "Level Up - Attack")},
        };

        public static List<string> LevelUpItemNames = new List<string>() { "Level Up - Attack", "Level Up - DamageResist", "Level Up - PotionEfficiency", "Level Up - Health", "Level Up - Stamina", "Level Up - Magic" };

        public static Dictionary<string, BonusUpgrade> BonusUpgrades = new Dictionary<string, BonusUpgrade>() {
            { "GoldenTrophy_1", new BonusUpgrade("Level Up - Stamina", $"kawngrahJoulA$uhnz! \"(<#8ddc6e>+1 SP<#FFFFFF>)\"") },
            { "GoldenTrophy_2", new BonusUpgrade("Level Up - DamageResist", $"kawngrahJoulA$uhnz! \"(<#5de7cf>+1 DEF<#FFFFFF>)\"") },
            { "GoldenTrophy_3", new BonusUpgrade("Level Up - Magic", $"kawngrahJoulA$uhnz! \"(<#2a8fed>+1 MP<#FFFFFF>)\"") },
            { "GoldenTrophy_4", new BonusUpgrade("Level Up - Magic", $"kawngrahJoulA$uhnz! \"(<#2a8fed>+1 MP<#FFFFFF>)\"") },
            { "GoldenTrophy_5", new BonusUpgrade("Level Up - PotionEfficiency", $"kawngrahJoulA$uhnz! \"(<#ca7be4>+1 POTION<#FFFFFF>)\"") },
            { "GoldenTrophy_6", new BonusUpgrade("Level Up - Stamina", $"kawngrahJoulA$uhnz! \"(<#8ddc6e>+1 SP<#FFFFFF>)\"") },
            { "GoldenTrophy_7", new BonusUpgrade("Level Up - PotionEfficiency", $"kawngrahJoulA$uhnz! \"(<#ca7be4>+1 POTION<#FFFFFF>)\"") },
            { "GoldenTrophy_8", new BonusUpgrade("Level Up - Stamina", $"kawngrahJoulA$uhnz! \"(<#8ddc6e>+1 SP<#FFFFFF>)\"") },
            { "GoldenTrophy_9", new BonusUpgrade("Level Up - PotionEfficiency", $"kawngrahJoulA$uhnz! \"(<#ca7be4>+1 POTION<#FFFFFF>)\"") },
            { "GoldenTrophy_10", new BonusUpgrade("Level Up - DamageResist", $"kawngrahJoulA$uhnz! \"(<#5de7cf>+1 DEF<#FFFFFF>)\"") },
            { "GoldenTrophy_11", new BonusUpgrade("Level Up - Magic", $"kawngrahJoulA$uhnz! \"(<#2a8fed>+1 MP<#FFFFFF>)\"") },
            { "GoldenTrophy_12", new BonusUpgrade("Level Up - Stamina", $"kawngrahJoulA$uhnz! \"(<#8ddc6e>+1 SP<#FFFFFF>)\"") },

            { "Relic - Hero Sword", new BonusUpgrade("Level Up - Attack", $"Hero Relic - <#e99d4c>ATT") },
            { "Relic - Hero Crown", new BonusUpgrade("Level Up - DamageResist", $"Hero Relic - <#5de7cf>DEF") },
            { "Relic - Hero Water", new BonusUpgrade("Level Up - PotionEfficiency", $"Hero Relic - <#ca7be4>POTION") },
            { "Relic - Hero Pendant HP", new BonusUpgrade("Level Up - Health", $"Hero Relic - <#f03c67>HP") },
            { "Relic - Hero Pendant SP", new BonusUpgrade("Level Up - Stamina", $"Hero Relic - <#8ddc6e>SP") },
            { "Relic - Hero Pendant MP", new BonusUpgrade("Level Up - Magic", $"Hero Relic - <#2a8fed>MP") },
        };

        public static Dictionary<string, List<int>> FillerItems = new Dictionary<string, List<int>>() {
            { "Firecracker", new List<int>() { 2, 3, 4, 5, 6 } },
            { "Firebomb", new List<int>() { 2, 3 } },
            { "Ice Bomb", new List<int>() { 2, 3, 5 } },
            { "Bait", new List<int>() { 1, 2 } },
            { "Pepper", new List<int>() { 2 } },
            { "Ivy", new List<int>() { 3 } },
            { "Berry_HP", new List<int>() { 1, 2, 3 } },
            { "Berry_MP", new List<int>() { 1, 2, 3 } },
            { "money", new List<int>() { 20, 25, 30, 32, 40, 48, 50 } },
        };

        private static string GetChestRewardID(Chest Chest) {
            return Chest.chestID == 0 ? $"{SceneLoaderPatches.SceneName}-{Chest.transform.position.ToString()} [{SceneLoaderPatches.SceneName}]" : $"{Chest.chestID} [{SceneLoaderPatches.SceneName}]";
        }

        public static bool Chest_IInteractionReceiver_Interact_PrefixPatch(Item i, Chest __instance) {
            string RewardId = GetChestRewardID(__instance);
            CheckFoolTrapSetting(RewardId);
            __instance.isFairy = false;
            return true;
        }

        public static void Chest_openSequence_MoveNext_PostfixPatch(Chest._openSequence_d__35 __instance, ref bool __result) {

            string RewardId = GetChestRewardID(__instance.__4__this);
            ItemData Item = ItemList[RewardId];

            if (Item.Reward.Type != "MONEY") {
                __instance._delay_5__2 = 1.35f;
            }

            if (!__result && !__instance.__4__this.interrupted) {
            
                if (__instance.__4__this.chestID == 0) {
                    string FairyId = $"{__instance.__4__this.gameObject.scene.name}-{__instance.__4__this.transform.position.ToString()}";
                    SaveFile.SetInt(FairyLookup[FairyId].Flag, 1);
                    SaveFile.SetInt($"randomizer opened fairy chest {FairyId}", 1);
                }
                
                if (!ItemsPickedUp[RewardId]) {
                    if (Item.Reward.Type != "MONEY") {
                        GiveReward(ItemList[RewardId]);
                    }
                    SetCollectedReward(RewardId);
                }
            
            }

        }

        public static bool Chest_InterruptOpening_PrefixPatch(Chest __instance) {
            if (TunicRandomizer.Settings.DisableChestInterruption) {
                return false;
            }
            if (__instance.chestID == 5 || __instance.chestID == 0) {
                return false;
            }
            return true;
        }

        public static bool Chest_moneySprayQuantityFromDatabase_GetterPatch(Chest __instance, ref int __result) {
            string RewardId = GetChestRewardID(__instance);
            ItemData Item = ItemList[RewardId];

            if (Item.Reward.Type == "MONEY") {
                __result = Item.Reward.Amount;
            } else {
                __result = 0;
            }

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
            if (SceneManager.GetActiveScene().name == "Quarry") {
                __result = false;
                return false;
            }
            if (__instance.chestID == 19) {
                if (__instance.transform.position.ToString() == "(8.8, 0.0, 9.9)") {
                    __result = SaveFile.GetInt("randomizer picked up 19 [Sword Cave]") == 1;
                } else {
                    __result = SaveFile.GetInt("randomizer picked up 19 [Forest Belltower]") == 1;
                }
                return false;
            }
            if (__instance.chestID == 5) {
                __result = SaveFile.GetInt("randomizer picked up 5 [Overworld Redux]") == 1;
                return false;
            }
            string FairyId = $"{__instance.gameObject.scene.name}-{__instance.transform.position.ToString()}";
            if (FairyLookup.ContainsKey(FairyId)) {
                __result = SaveFile.GetInt($"randomizer opened fairy chest {FairyId}") == 1;
                return false;
            }

            return true;
        }

        public static bool ItemPickup_onGetIt_PrefixPatch(ItemPickup __instance) {

            if (__instance.itemToGive.Type == Item.ItemType.MONEY) {
                return true;
            } else {
                string RewardId = $"{__instance.itemToGive.name} [{SceneLoaderPatches.SceneName}]";
                CheckFoolTrapSetting(RewardId);

                ItemData Reward = ItemList[RewardId];
                GiveReward(Reward);
                SetCollectedReward(RewardId);

            }

            __instance.pickupStateVar.BoolValue = true;
            return false;
        }

        public static bool HeroRelicPickup_onGetIt_PrefixPatch(HeroRelicPickup __instance) {
            string RewardId = $"{__instance.name} [{SceneLoaderPatches.SceneName}]";
            CheckFoolTrapSetting(RewardId);
            ItemData Reward = ItemList[RewardId];

            GiveReward(Reward);
            SetCollectedReward(RewardId);

            __instance.pickupStateVar.BoolValue = true;
            __instance.destroyOrDisable();
            return false;
        }

        public static bool PagePickup_onGetIt_PrefixPatch(PagePickup __instance) {
            string RewardId = $"{__instance.pageName} [{SceneLoaderPatches.SceneName}]";
            CheckFoolTrapSetting(RewardId);
            ItemData Reward = ItemList[RewardId];

            GiveReward(Reward);
            SetCollectedReward(RewardId);

            SaveFile.SetInt($"unlocked page {PagePickup.LeafNameToLeafNumber(__instance.pageName)}", 1);
            SaveFile.SetInt($"randomizer picked up page {PagePickup.LeafNameToLeafNumber(__instance.pageName)}", 1);
            return false;
        }

        public static bool ShopItem_IInteractionReceiver_Interact_PrefixPatch(Item i, ShopItem __instance) {
            string RewardId = $"{__instance.name} [Shop]";
            if (ItemList.ContainsKey(RewardId)) {
                ItemData Item = ItemList[RewardId];
                int Price = TunicRandomizer.Settings.CheaperShopItemsEnabled ? 300 : __instance.price;
                __instance.confirmPurchaseFormattedLanguageLine.text = $"bI for {Price} [money]?";
            } else {
                __instance.confirmPurchaseFormattedLanguageLine.text = $"bI for {__instance.price} [money]?";
            }
            return true;
        }
        public static bool ShopItem_buy_PrefixPatch(ShopItem __instance) {
            string RewardId = $"{__instance.name} [Shop]";
            if (!ItemList.ContainsKey(RewardId)) {
                return true;
            }
            int Price = TunicRandomizer.Settings.CheaperShopItemsEnabled ? 300 : __instance.price;
            if (Inventory.GetItemByName("MoneySmall").Quantity < Price) {
                GenericMessage.ShowMessage($"nawt Enuhf [money]...");
            } else {
                Inventory.GetItemByName("MoneySmall").Quantity -= Price;
                CheckFoolTrapSetting(RewardId);
                ItemData Reward = ItemList[RewardId];
                if (Reward.Reward.Type == "MONEY") {
                    Reward.Reward.Amount += Price;
                }
                GiveReward(Reward);
                SetCollectedReward(RewardId);
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
            string RewardId = $"Well Reward ({StateVariable.GetStateVariableByName("Trinket Coins Tossed").IntValue} Coins) [Trinket Well]";
            if (ItemList.ContainsKey(RewardId)) {
                CheckFoolTrapSetting(RewardId);
                ItemData Reward = ItemList[RewardId];
                GiveReward(Reward);
                SetCollectedReward(RewardId);
            }
            return false;
        }

        public static bool FairyCollection_getFairyCount_PrefixPatch(FairyCollection __instance, ref int __result) {
            __result = TunicRandomizer.Tracker.ImportantItems["Fairies"];
            return false;
        }

        public static void UpgradeAltar_DoOfferingSequence_PostfixPatch(UpgradeAltar __instance) {
            foreach (string LevelUp in ItemPatches.LevelUpItemNames) {
                TunicRandomizer.Tracker.ImportantItems[LevelUp] = Inventory.GetItemByName(LevelUp).Quantity;
            }
        }

        public static void GiveReward(ItemData ItemData) {
            Reward Reward = ItemData.Reward;
            if (Reward.Type == "INVENTORY") {
                Item Item = Inventory.GetItemByName(Reward.Name);
                Item.Quantity += Reward.Amount;

                if (BonusUpgrades.ContainsKey(Reward.Name)) {
                    Item.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    if (TunicRandomizer.Settings.BonusStatUpgradesEnabled) {
                        Inventory.GetItemByName(BonusUpgrades[Reward.Name].LevelUp).Quantity += 1;
                        Item.collectionMessage.text = Translations.TranslateDefaultNoQuotes(BonusUpgrades[Reward.Name].CustomPickupMessage);
                    } else {
                        Item.collectionMessage.text = $"kawngrahJoulA$uhnz!";
                    }
                }
                if (Reward.Name == "Stick" || Reward.Name == "Sword") {
                    Item.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    Item.collectionMessage.text = $"fownd ahn Itehm!";
                }
                if (Reward.Name == "Key (House)") {
                    Item.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    Item.collectionMessage.text = TunicRandomizer.Settings.UseTrunicTranslations ? $"[key] too Old hows" : $"[key] too \"Old House\"";
                }

                ItemPresentation.PresentItem(Item, Reward.Amount);
            } else if (Reward.Type == "MONEY") {
                Vector3 SpawnPosition;

                if (ItemData.Location.SceneName == "Trinket Well") {
                    SpawnPosition = GameObject.FindObjectOfType<TrinketWell>().transform.position;
                    SpawnPosition.Set(SpawnPosition.x, SpawnPosition.y + 3.25f, SpawnPosition.z);
                } else if (ItemData.Location.SceneName == "Shop") {
                    SpawnPosition = PlayerCharacter.instance.transform.position;
                } else {
                    SpawnPosition = StringToVector3(ItemData.Location.Position);
                }

                CoinSpawner.SpawnCoins(Reward.Amount, SpawnPosition);
            } else if (Reward.Type == "FAIRY") {
                SaveFile.SetInt($"randomizer obtained fairy {Reward.Name}", 1);
                Vector3 SpawnPosition;

                if (ItemData.Location.SceneName == "Trinket Well") {
                    SpawnPosition = GameObject.FindObjectOfType<TrinketWell>().transform.position;
                    SpawnPosition.Set(SpawnPosition.x, SpawnPosition.y + 3.25f, SpawnPosition.z);
                } else if (ItemData.Location.SceneName == "Shop") {
                    SpawnPosition = PlayerCharacter.instance.transform.position;
                } else {
                    SpawnPosition = StringToVector3(ItemData.Location.Position);
                }
                GameObject.Instantiate(ModelSwaps.FairyAnimation, SpawnPosition, Quaternion.identity).SetActive(true);
            } else if (Reward.Type == "PAGE") {
                Dictionary<string, string> pagesForAbilities = new Dictionary<string, string>() {
                    { "12", "randomizer prayer unlocked" },
                    { "21", "randomizer holy cross unlocked" },
                    { "26", "randomizer ice rod unlocked" },
                };
                SaveFile.SetInt($"randomizer obtained page {Reward.Name}", 1);
                PageDisplay.ShowPage(int.Parse(Reward.Name, CultureInfo.InvariantCulture));
                if (SaveFile.GetInt("randomizer shuffled abilities") == 1) {
                    if (Reward.Name == "12" || Reward.Name == "21" || Reward.Name == "26") {
                        SaveFile.SetInt(pagesForAbilities[Reward.Name], 1);
                        SaveFile.SetFloat($"{pagesForAbilities[Reward.Name]} time", SpeedrunData.inGameTime);
                        PageDisplayPatches.ShowAbilityUnlock = true;
                        PageDisplayPatches.AbilityUnlockPage = Reward.Name;
                    }
                }
            } else if (Reward.Type == "RELIC") {
                Item Relic = Inventory.GetItemByName(Reward.Name);
                Relic.Quantity = 1;

                Relic.collectionMessage = new LanguageLine();
                Relic.CollectionMessage.text = Translations.Translate(BonusUpgrades[Reward.Name].CustomPickupMessage, true);
                ItemPresentation.PresentItem(Relic);

                if (SceneLoaderPatches.SceneName == "Overworld Interiors") {
                    foreach (string Key in HeroRelicLookup.Keys) {
                        StateVariable.GetStateVariableByName(HeroRelicLookup[Key].Flag).BoolValue = Inventory.GetItemByName(Key).Quantity == 1;
                    }
                }
                if (TunicRandomizer.Settings.BonusStatUpgradesEnabled) {
                    Inventory.GetItemByName(BonusUpgrades[Reward.Name].LevelUp).Quantity += 1;
                }
                
            } else if (Reward.Type == "FOOL") {
                ApplyFoolEffect();
            } else if (Reward.Type == "SPECIAL") {
                if (Reward.Name == "Dath Stone") {
                    ItemPresentation.PresentItem(Inventory.GetItemByName("Key Special"));
                    Inventory.GetItemByName("Homeward Bone Statue").Quantity = 1;
                } else if (Reward.Name == "Sword Progression") {
                    TunicRandomizer.Tracker.ImportantItems["Sword Progression"]++;
                    SaveFile.SetInt("randomizer sword progression level", TunicRandomizer.Tracker.ImportantItems["Sword Progression"]);
                    SwordProgression.UpgradeSword(TunicRandomizer.Tracker.ImportantItems["Sword Progression"]);
                    if (TunicRandomizer.Settings.ShowItemsEnabled) {
                        ModelSwaps.SwapItemsInScene();
                    }
                } else if (Reward.Name == "Hexagon Gold") {
                    TunicRandomizer.Tracker.ImportantItems["Hexagon Gold"]++;
                    SaveFile.SetInt("randomizer inventory quantity Hexagon Gold", TunicRandomizer.Tracker.ImportantItems["Hexagon Gold"]);
                    int GoldHexes = SaveFile.GetInt("randomizer inventory quantity Hexagon Gold");

                    if (GoldHexes == SaveFile.GetInt("randomizer hexagon quest prayer requirement")) {
                        SaveFile.SetInt("randomizer prayer unlocked", 1);
                        SaveFile.SetFloat("randomizer prayer unlocked time", SpeedrunData.inGameTime);
                        ShowNotification($"{Translations.Translate("PRAYER Unlocked", true)}", $"Jahnuhl yor wizduhm, rooin sEkur");
                    }
                    if (GoldHexes == SaveFile.GetInt("randomizer hexagon quest holy cross requirement")) {
                        SaveFile.SetInt("randomizer holy cross unlocked", 1);
                        SaveFile.SetFloat("randomizer holy cross unlocked time", SpeedrunData.inGameTime);
                        ShowNotification($"{Translations.Translate("HOLY CROSS Unlocked", true)}", $"sEk wuht iz rItfuhlE yorz");
                        foreach (ToggleObjectBySpell SpellToggle in Resources.FindObjectsOfTypeAll<ToggleObjectBySpell>()) {
                            foreach (ToggleObjectBySpell Spell in SpellToggle.gameObject.GetComponents<ToggleObjectBySpell>()) {
                                Spell.enabled = true;
                            }
                        }
                    }
                    if (GoldHexes == SaveFile.GetInt("randomizer hexagon quest ice rod requirement")) {
                        SaveFile.SetInt("randomizer ice rod unlocked", 1);
                        SaveFile.SetFloat("randomizer ice rod unlocked time", SpeedrunData.inGameTime);
                        ShowNotification($"{Translations.Translate("ICE ROD Unlocked", true)}", $"#A wOnt nO wuht hit #ehm");
                    }
                    ItemPresentation.PresentItem(Inventory.GetItemByName("Hexagon Blue"));
                }
            }
        }

        private static void ApplyFoolEffect() {
            System.Random Random = new System.Random();
            int FoolType = PlayerCharacterPatches.StungByBee ? Random.Next(21, 100) : Random.Next(100);
            if (FoolType < 35) {
                SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
                PlayerCharacter.instance.IDamageable_ReceiveDamage(PlayerCharacter.instance.hp / 3, 0, Vector3.zero, 0, 0);
                ShowNotification($"yoo R A \"<#ffd700>FOOL<#ffffff>!!\"", $"\"(\"it wuhz A swRm uhv <#ffd700>bEz\"...)\"");
                PlayerCharacterPatches.StungByBee = true;
                PlayerCharacter.instance.Flinch(true);
            } else if (FoolType >= 35 && FoolType < 50) {
                PlayerCharacter.ApplyRadiationAsDamageInHP(0f);
                PlayerCharacter.instance.stamina = 0;
                PlayerCharacter.instance.cachedFireController.FireAmount = 3f;
                SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
                ShowNotification($"yoo R A \"<#FF3333>FOOL<#ffffff>!!\"", $"iz it hawt in hEr?");
                PlayerCharacter.instance.Flinch(true);
            } else if (FoolType >= 50) {
                PlayerCharacter.ApplyRadiationAsDamageInHP(PlayerCharacter.instance.maxhp * .2f);
                SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
                SFX.PlayAudioClipAtFox(PlayerCharacter.standardFreezeSFX);
                PlayerCharacter.instance.AddFreezeTime(3f);
                ShowNotification($"yoo R A \"<#86A5FF>FOOL<#ffffff>!!\"", $"hahvi^ ahn Is tIm?");
            }
        }

        private static void SetCollectedReward(string RewardId) {

            SaveFile.SetInt("randomizer picked up " + RewardId, 1);
            ItemsPickedUp[RewardId] = true;
            ItemData ItemData = ItemList[RewardId];
            Logger.LogInfo("Picked up item " + RewardId + " (" + ItemData.Reward.Name + " - " + ItemData.Reward.Amount + ")");

            UpdateItemTracker(RewardId);
            ItemTracker.SaveTrackerFile();
            if (ItemRandomizer.CreateSpoilerLog) {
                ItemRandomizer.PopulateSpoilerLog();
            }
            if (RewardId == PlayerCharacterPatches.MailboxHintId) {
                SaveFile.SetInt("randomizer got mailbox hint item", 1);
            }

            GameObject FairyTarget = ItemData.Location.SceneName == "Trinket Well" ? GameObject.Find($"fairy target trinket well"): GameObject.Find($"fairy target {ItemData.Location.Position}");
            if (FairyTarget != null) {
                GameObject.Destroy(FairyTarget);
            }
            if (ItemList.Keys.Where(ItemId => ItemList[ItemId].Location.SceneName == SceneLoaderPatches.SceneName && !ItemsPickedUp[ItemId]).ToList().Count == 0) {
                FairyTargets.CreateLoadZoneTargets();
            }
            List<string> TimedItems = new List<string>() { "Hexagon Red", "Hexagon Green", "Hexagon Blue", "Hexagon Gold", "Wand", "Hyperdash", "Sword", "Sword Progression", "12", "21"};
            string ItemName = ItemData.Reward.Name;
            if (TimedItems.Contains(ItemName)) {
                if (ItemData.Reward.Type == "PAGE") {
                    SaveFile.SetFloat($"randomizer Page {ItemName} time", SpeedrunData.inGameTime);
                } else {
                    SaveFile.SetFloat($"randomizer {ItemName} {TunicRandomizer.Tracker.ImportantItems[ItemName]} time", SpeedrunData.inGameTime);
                }
            }
        }

        public static void CheckFoolTrapSetting(string RewardId) {
            Reward Reward = ItemList[RewardId].Reward;
            if (Reward.Type == "MONEY") {
                if ((TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.NORMAL && Reward.Amount < 20)
                || (TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.DOUBLE && Reward.Amount <= 20)
                || (TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.ONSLAUGHT && Reward.Amount <= 30)) { 
                    Reward.Name = "Fool";
                    Reward.Type = "FOOL";
                    Reward.Amount = 1;
                }
            }
        }

        public static void UpdateItemTracker(string RewardId) {
            Reward Reward = ItemList[RewardId].Reward;

            if (TunicRandomizer.Tracker.ImportantItems.ContainsKey(Reward.Name) && Reward.Name != "Sword Progression" && Reward.Name != "Hexagon Gold") {
                TunicRandomizer.Tracker.ImportantItems[Reward.Name]++;
            }
            if (Reward.Name.Contains("GoldenTrophy")) {
                TunicRandomizer.Tracker.ImportantItems["Golden Trophies"]++;

                if (TunicRandomizer.Tracker.ImportantItems["Golden Trophies"] == 12) {
                    Inventory.GetItemByName("Spear").Quantity = 1;
                }
            }
            if (Reward.Type == "PAGE") {
                TunicRandomizer.Tracker.ImportantItems["Pages"]++;
            }
            if (Reward.Type == "FAIRY") {
                TunicRandomizer.Tracker.ImportantItems["Fairies"]++;
/*                if (TunicRandomizer.Tracker.ImportantItems["Fairies"] == 20) {
                    Inventory.GetItemByName("Crystal Ball").Quantity = 1;
                }*/
            }
            if (Reward.Name.Contains("Trinket -") || Reward.Name == "Mask") {
                TunicRandomizer.Tracker.ImportantItems["Trinket Cards"]++;
            }
            TunicRandomizer.Tracker.ItemsCollected.Add(ItemList[RewardId]);
            foreach (string LevelUp in LevelUpItemNames) {
                TunicRandomizer.Tracker.ImportantItems[LevelUp] = Inventory.GetItemByName(LevelUp).Quantity;
            }
        }

        public static void PotionCombine_Show_PostFixPatch(PotionCombine __instance) {
            TunicRandomizer.Tracker.ImportantItems["Flask Container"]++;
            ItemTracker.SaveTrackerFile();
        }

        private static Vector3 StringToVector3(string Position) {
            try {
                Position = Position.Replace("(", "").Replace(")", "");
                string[] coords = Position.Split(',');
                Vector3 vector = new Vector3(float.Parse(coords[0], CultureInfo.InvariantCulture), float.Parse(coords[1], CultureInfo.InvariantCulture), float.Parse(coords[2], CultureInfo.InvariantCulture));
                return vector;
            } catch (Exception e) {
                return PlayerCharacter.instance.transform.position;
            }
        }

        public static void ShowNotification(string topLine, string bottomLine) {
            LanguageLine topLineObject = ScriptableObject.CreateInstance<LanguageLine>();
            topLineObject.text = topLine;

            LanguageLine bottomLineObject = ScriptableObject.CreateInstance<LanguageLine>();
            bottomLineObject.text = bottomLine;

            AreaData areaData = ScriptableObject.CreateInstance<AreaData>();
            areaData.topLine = topLineObject;
            areaData.bottomLine = bottomLineObject;

            AreaLabel.ShowLabel(areaData);
        }
    }
}
