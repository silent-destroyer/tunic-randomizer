using Archipelago.MultiClient.Net.Enums;
using InControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {
    public class SpeedrunFinishlineDisplayPatches {
        
        public static Dictionary<string, string> ReportGroupItems = new Dictionary<string, string>(){
            {"Inventory items_stick", "Stick"},
            {"Inventory items_sword", "Sword"},
            {"Inventory items_shield", "Shield"},
            {"Inventory items_lantern", "Lantern"},
            {"Inventory items_stundagger", "Stundagger"},
            {"Inventory items_techbow", "Techbow"},
            {"Inventory items_hourglass", "SlowmoItem"},
            {"Inventory items_forcewand", "Wand"},
            {"Inventory items_shotgun", "Shotgun"},
            {"Inventory items_cape", "Hyperdash"},
            {"Inventory items_potion", "Flask Container"},
            {"Inventory items_trinketcard", "Trinket Cards"},
            {"Inventory items_trinketslot", "Trinket Slot"},
            {"Inventory items_fairy", "Fairies"},
            {"Inventory items_trophy", "Golden Trophies"},
            {"Inventory items_offering_tooth", "Upgrade Offering - Attack - Tooth"},
            {"Inventory items_offering_effigy", "Upgrade Offering - DamageResist - Effigy"},
            {"Inventory items_offering_ash", "Upgrade Offering - PotionEfficiency Swig - Ash"},
            {"Inventory items_offering_flower", "Upgrade Offering - Health HP - Flower"},
            {"Inventory items_offering_feather", "Upgrade Offering - Stamina SP - Feather"},
            {"Inventory items_offering_orb", "Upgrade Offering - Magic MP - Mushroom"},
            {"Inventory items_dash stone", "Dath Stone"},
            {"Inventory items_money triangle", "Golden Item"},
            {"Inventory items_book", "Pages"},
            {"Randomizer items_Gold Questagon", "Hexagon Gold"}
        };
        public static Dictionary<string, (string, string)> HeroRelicIcons = new Dictionary<string, (string, string)>() {
            {"Inventory items_offering_tooth", ("Relic - Hero Sword", "Randomizer items_Hero Relic - ATT")},
            {"Inventory items_offering_effigy", ("Relic - Hero Crown", "Randomizer items_Hero Relic - DEF")},
            {"Inventory items_offering_ash", ("Relic - Hero Water", "Randomizer items_Hero Relic - POTION")},
            {"Inventory items_offering_flower", ("Relic - Hero Pendant HP", "Randomizer items_Hero Relic - HP")},
            {"Inventory items_offering_feather", ("Relic - Hero Pendant SP", "Randomizer items_Hero Relic - SP")},
            {"Inventory items_offering_orb", ("Relic - Hero Pendant MP", "Randomizer items_Hero Relic - MP")},
        };

        public static Dictionary<string, string> StatsScreenSecret = new Dictionary<string, string>() {
            { "Overworld", "Overwor<#eaa614>ld<#ffffff>" },
            { "West Garden", "West Gar<#eaa614>d<#ffffff>en" },
            { "Ruined Atoll", "<#eaa614>Ru<#ffffff>ined Atoll" },
            { "Quarry/Mountain", "Q<#eaa614>u<#ffffff>ar<#eaa614>r<#ffffff>y/Mo<#eaa614>u<#ffffff>ntain" },
            { "Swamp", "S<#eaa614>w<#ffffff>amp" },
            { "East Forest", "East Fo<#eaa614>r<#ffffff>est" },
            { "Eastern Vault Fortress", "Easte<#eaa614>r<#ffffff>n Va<#eaa614>u<#ffffff>lt Fo<#eaa614>r<#ffffff>tress" },
            { "Library", "<#eaa614>L<#ffffff>ibrary" },
            { "Rooted Ziggurat", "<#eaa614>R<#ffffff>ooted Ziggu<#eaa614>r<#ffffff>at" },
            { "Cathedral", "Cathedra<#eaa614>l<#ffffff>" },
            { "Dark Tomb", "<#eaa614>D<#ffffff>ark Tomb" },
            { "Frog's Domain", "F<#eaa614>r<#ffffff>og's <#eaa614>D<#ffffff>omain" },
            { "Shop/Coin Wells", "Shop/Coin We<#eaa614>l<#ffffff>ls" },
            { "Bosses Defeated", "Bosses <#eaa614>D<#ffffff>efeate<#eaa614>d<#ffffff>" },
            { "Beneath the Well", "Beneath the We<#eaa614>ll<#ffffff>" },
            { "Far Shore/Hero's Grave", "Far Sho<#eaa614>r<#ffffff>e/Hero's Grave" },
            { "Holy Cross Checks", "Ho<#eaa614>l<#ffffff>y Cross Checks" },
            { "Player Deaths", "P<#eaa614>l<#ffffff>ayer Deaths" },
            { "Time Found", "Time Foun<#eaa614>d<#ffffff>" },
        };

        public static GameObject CompletionRate;
        public static GameObject CompletionCanvas;

        public static bool GameCompleted = false;

        public static GameObject ActionGroup;
        public static GameObject Release;
        public static GameObject Collect;

        public static Dictionary<string, GameObject> StatSections = new Dictionary<string, GameObject>();

        public static bool SpeedrunFinishlineDisplay_showFinishline_PrefixPatch(SpeedrunFinishlineDisplay __instance) {

            SpeedrunReportItem DathStone = ScriptableObject.CreateInstance<SpeedrunReportItem>();
            DathStone.icon = Inventory.GetItemByName("Dath Stone").icon;
            SpeedrunReportItem ManualOrGoldHex = ScriptableObject.CreateInstance<SpeedrunReportItem>();
            if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                ManualOrGoldHex.icon = Inventory.GetItemByName("Hexagon Gold").icon;
            } else {
                ManualOrGoldHex.icon = Inventory.GetItemByName("Book").icon;
            }
            SpeedrunReportItem Grass = ScriptableObject.CreateInstance<SpeedrunReportItem>();
            Grass.icon = Inventory.GetItemByName("Grass").icon;
            SpeedrunReportItem Ladders = ScriptableObject.CreateInstance<SpeedrunReportItem>();
            Ladders.icon = ModelSwaps.FindSprite("Randomizer items_ladder");
            SpeedrunReportItem Fuses = ScriptableObject.CreateInstance<SpeedrunReportItem>();
            Fuses.icon = ModelSwaps.FindSprite("Randomizer items_fuse");
            SpeedrunReportItem Bells = ScriptableObject.CreateInstance<SpeedrunReportItem>();
            Bells.icon = ModelSwaps.FindSprite("Randomizer items_bell");

            List<SpeedrunReportItem> items = SpeedrunFinishlineDisplay.instance.reportGroup_items.ToList();
            items.Add(DathStone);
            SpeedrunFinishlineDisplay.instance.reportGroup_items = items.ToArray();

            SpeedrunFinishlineDisplay.instance.reportGroup_secrets = new SpeedrunReportItem[] {
                SpeedrunFinishlineDisplay.instance.reportGroup_secrets[0],
                SpeedrunFinishlineDisplay.instance.reportGroup_secrets[1],
                SpeedrunFinishlineDisplay.instance.reportGroup_secrets[2],
                SpeedrunFinishlineDisplay.instance.reportGroup_secrets[3],
                ManualOrGoldHex,
                Ladders,
                Fuses,
                Bells,
                Grass,
            };

            Inventory.GetItemByName("Firecracker").Quantity += 1;

            foreach (SpeedrunReportItem item in __instance.reportGroup_items) {
                item.itemsForQuantity = new Item[] { Inventory.GetItemByName("Firecracker") };
                item.chestIDs = new int[] { };
                item.tallyStateVars = new StateVariable[] { };
            }
            foreach (SpeedrunReportItem secret in __instance.reportGroup_secrets) {
                secret.itemsForQuantity = new Item[] { Inventory.GetItemByName("Firecracker") };
                secret.chestIDs = new int[] { };
                secret.tallyStateVars = new StateVariable[] { };
            }
            foreach (SpeedrunReportItem upgrade in __instance.reportGroup_upgrades) {
                upgrade.itemsForQuantity = new Item[] { Inventory.GetItemByName("Firecracker") };
                upgrade.chestIDs = new int[] { };
                upgrade.tallyStateVars = new StateVariable[] { };
            }
            return true;
        }
        public static void SpeedrunFinishlineDisplay_showFinishline_PostfixPatch(SpeedrunFinishlineDisplay __instance) {
            Inventory.GetItemByName("Firecracker").Quantity -= 1;
        }

        public static bool SpeedrunFinishlineDisplay_addParadeIcon_PrefixPatch(SpeedrunFinishlineDisplay __instance, ref Sprite icon, ref int quantity, ref RectTransform rt) {
            if (icon.name == "Inventory items_sword" && Inventory.GetItemByName("Sword").Quantity > 0) {
                if (GetBool(SwordProgressionEnabled)) {
                    int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                    if (SwordLevel == 3) {
                        icon = Inventory.GetItemByName("Librarian Sword").icon;
                    } else if (SwordLevel >= 4) {
                        icon = Inventory.GetItemByName("Heir Sword").icon;
                    }
                }
                quantity = 1;
                return true;
            }

            if (icon.name == "Randomizer items_grass") {
                quantity = Inventory.GetItemByName("Grass").Quantity;
                return SaveFile.GetInt(GrassRandoEnabled) == 1;
            }
            if (icon.name == "Randomizer items_ladder") {
                quantity = ItemRandomizer.LadderItems.Where(item => Inventory.GetItemByName(item).Quantity > 0).Count();
                return SaveFile.GetInt(LadderRandoEnabled) == 1;
            }
            if (icon.name == "Randomizer items_fuse") {
                quantity = ItemRandomizer.FuseItems.Where(item => Inventory.GetItemByName(item).Quantity > 0).Count();
                return SaveFile.GetInt(FuseShuffleEnabled) == 1;
            }
            if (icon.name == "Randomizer items_bell") {
                quantity = ItemRandomizer.BellItems.Where(item => Inventory.GetItemByName(item).Quantity > 0).Count();
                return SaveFile.GetInt(BellShuffleEnabled) == 1;
            }
            if (TunicRandomizer.Tracker.ImportantItems[ReportGroupItems[icon.name]] == 0) {
                return false;
            }

            quantity = TunicRandomizer.Tracker.ImportantItems[ReportGroupItems[icon.name]];
            if (HeroRelicIcons.ContainsKey(icon.name) && Inventory.GetItemByName(HeroRelicIcons[icon.name].Item1).Quantity > 0) {
                icon = ModelSwaps.FindSprite(HeroRelicIcons[icon.name].Item2);
            }
            return true;
        }

        public static bool SpeedrunFinishlineDisplay_HideFinishline_PrefixPatch(SpeedrunFinishlineDisplay __instance) {
            if (GameObject.Find("_FinishlineDisplay(Clone)/").transform.childCount >= 3) {
                GameObject.Destroy(GameObject.Find("_FinishlineDisplay(Clone)/").transform.GetChild(2).gameObject);
            }
            return true;
        }

        public static void SpeedrunFinishlineDisplay_AndTime_PostfixPatch(SpeedrunFinishlineDisplay __instance) {
            if (GameObject.Find("_FinishlineDisplay(Clone)/").transform.childCount >= 3) {
                GameObject.Destroy(GameObject.Find("_FinishlineDisplay(Clone)/").transform.GetChild(2).gameObject);
            }
        }

        public static void SetupCompletionStatsDisplay() {

            if (CompletionRate != null && CompletionCanvas != null) {
                return;
            }

            StatSections.Clear();

            CompletionRate = new GameObject("completion rate");
            CompletionRate.transform.parent = GameObject.Find("_FinishlineDisplay(Clone)/").transform.GetChild(0).GetChild(0);
            CompletionRate.AddComponent<TextMeshPro>().fontMaterial = ModelSwaps.FindMaterial("Latin Rounded - Quantity Outline");
            CompletionRate.GetComponent<TextMeshPro>().font = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().Where(Font => Font.name == "Latin Rounded").ToList()[0];
            CompletionRate.GetComponent<TextMeshPro>().text = $"Completion Rates";
            CompletionRate.GetComponent<TextMeshPro>().fontSize = 100f;
            CompletionRate.layer = 5;
            CompletionRate.transform.position = new Vector3(-345f, 142.5f, 55f);
            CompletionRate.transform.localScale = new Vector3(5f, 5f, 5f);
            CompletionRate.SetActive(false);
            CompletionCanvas = new GameObject("completion stats");
            CompletionCanvas.layer = 5;
            CompletionCanvas.transform.parent = GameObject.Find("_FinishlineDisplay(Clone)/").transform;
            CompletionCanvas.AddComponent<Canvas>();
            CompletionCanvas.transform.position = new Vector3(0f, 0f, 300f);
            CompletionCanvas.SetActive(false);

            GameObject TotalCompletion = GameObject.Instantiate(CompletionRate.gameObject, GameObject.Find("_FinishlineDisplay(Clone)/").transform.GetChild(2));
            TotalCompletion.transform.position = new Vector3(0, -30f, 55f);
            TotalCompletion.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);

            TotalCompletion.SetActive(true);
            List<List<string>> Columns = new List<List<string>>() {
                new List<string>(){"Overworld", "West Garden", "Ruined Atoll", "Quarry/Mountain", "Swamp"},
                new List<string>(){"East Forest", "Eastern Vault Fortress", "Library", "Rooted Ziggurat", "Cathedral"},
                new List<string>(){"Dark Tomb", "Frog's Domain", "Shop/Coin Wells", "Bosses Defeated", "Time Found"},
                new List<string>(){"Beneath the Well", "Far Shore/Hero's Grave", "Holy Cross Checks", "Player Deaths"},
            };
            List<int> Spacings = new List<int>() { -345, -243, 410, 503 };

            for (int i = 0; i < 5; i++) {
                SetupCompletionCount(Columns[0][i], i, Spacings[0]);
            }
            for (int i = 0; i < 5; i++) {
                SetupCompletionCount(Columns[1][i], i, Spacings[1]);
            }
            for (int i = 0; i < 5; i++) {
                SetupCompletionCount(Columns[2][i], i, Spacings[2]);
            }
            for (int i = 0; i < 4; i++) {
                SetupCompletionCount(Columns[3][i], i, Spacings[3]);
            }

            CompletionCanvas.transform.parent = GameObject.Find("_GameGUI(Clone)/AreaLabels/").transform;
            CompletionCanvas.transform.localScale = new Vector3(2, 2, 2);
            CompletionCanvas.transform.position = new Vector3(0, 0, 200);

            GameObject TimeText = GameObject.Instantiate(SpeedrunFinishlineDisplay.instance.transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
            TimeText.transform.parent = CompletionCanvas.transform;
            TimeText.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            TimeText.transform.position = new Vector3(0, 185f, 210f);
            TimeText.GetComponent<TextMeshPro>().text = FormatTime(SpeedrunData.inGameTime, false, true);
            TimeText.transform.GetChild(2).GetComponent<TextMeshPro>().text = FormatTime(SpeedrunData.inGameTime, false, true);
            TimeText.name = "randomizer finish line time text";
            TimeText.SetActive(false);
            StatSections.Add("Timer", TimeText);

            StatSections.Add("Total Completion", TotalCompletion);

            RandoActionSet randoActionSet = RandoActionSet.CreateWithDefaultBindings();
            InputManager.AttachPlayerActionSet(randoActionSet);
            InputSequenceAssistanceMenu menuBase = Resources.FindObjectsOfTypeAll<InputSequenceAssistanceMenu>().First();
            GameObject PlatformIcon = menuBase.transform.GetChild(0).GetChild(6).gameObject;
            GameObject HideStats = new GameObject("Hide Stats");
            HideStats.transform.parent = CompletionCanvas.transform;
            HideStats.layer = 5;
            HideStats.AddComponent<CanvasRenderer>();

            GameObject NewButton = GameObject.Instantiate(PlatformIcon);
            NewButton.layer = 5;
            NewButton.transform.parent = HideStats.transform;
            NewButton.name = "button icon";
            NewButton.GetComponent<PlatformSprite>().ActionName = "Hide Stats";
            NewButton.GetComponent<PlatformSprite>().actionSet = randoActionSet;
            NewButton.GetComponent<PlatformSprite>().action = randoActionSet.actionsByName["Hide Stats"];
            NewButton.transform.localScale = Vector3.one * 0.2f;

            GameObject ActionText = GameObject.Instantiate(CompletionCanvas.transform.GetChild(1)).gameObject;
            ActionText.name = "text";
            ActionText.layer = 5;
            ActionText.transform.parent = HideStats.transform;
            ActionText.GetComponent<TextMeshPro>().text = "Hide Stats";
            ActionText.transform.localPosition = new Vector3(145f, -147.5f, 0);

            GameObject SkipCredits = GameObject.Instantiate(HideStats);
            SkipCredits.name = "Skip Credits";
            SkipCredits.layer = 5;
            SkipCredits.transform.parent = HideStats.transform.parent;
            SkipCredits.transform.GetChild(0).GetComponent<PlatformSprite>().ActionName = "Skip Credits";
            SkipCredits.transform.GetChild(0).GetComponent<PlatformSprite>().actionSet = randoActionSet;
            SkipCredits.transform.GetChild(0).GetComponent<PlatformSprite>().action = randoActionSet.actionsByName["Skip Credits"];
            SkipCredits.transform.GetChild(1).GetComponent<TextMeshPro>().text = "Skip Credits\n<size=80%>(Hold 3s)";
            SkipCredits.transform.GetChild(1).localPosition = new Vector3(145f, -142.5f, 0);

            Release = GameObject.Instantiate(HideStats);
            Release.name = "Release";
            Release.layer = 5;
            Release.transform.parent = HideStats.transform.parent;
            Release.transform.GetChild(0).GetComponent<PlatformSprite>().ActionName = "Release";
            Release.transform.GetChild(0).GetComponent<PlatformSprite>().actionSet = randoActionSet;
            Release.transform.GetChild(0).GetComponent<PlatformSprite>().action = randoActionSet.actionsByName["Release"];
            Release.transform.GetChild(1).GetComponent<TextMeshPro>().text = "Release";
            Release.transform.GetChild(1).localPosition = new Vector3(145f, -147.5f, 0);

            Collect = GameObject.Instantiate(HideStats);
            Collect.name = "Collect";
            Collect.layer = 5;
            Collect.transform.parent = HideStats.transform.parent;
            Collect.transform.GetChild(0).GetComponent<PlatformSprite>().ActionName = "Collect";
            Collect.transform.GetChild(0).GetComponent<PlatformSprite>().actionSet = randoActionSet;
            Collect.transform.GetChild(0).GetComponent<PlatformSprite>().action = randoActionSet.actionsByName["Collect"];
            Collect.transform.GetChild(1).GetComponent<TextMeshPro>().text = "Collect";
            Collect.transform.GetChild(1).localPosition = new Vector3(145f, -147.5f, 0);

            HideStats.transform.position = new Vector3(-460, 75, 200);
            SkipCredits.transform.position = new Vector3(-355, 75, 200);
            Release.transform.position = new Vector3(-460, 35, 0);
            Collect.transform.position = new Vector3(-355, 35, 200);

            ActionGroup = new GameObject("Action Group");
            ActionGroup.transform.parent = HideStats.transform.parent;
            HideStats.transform.parent = ActionGroup.transform;
            SkipCredits.transform.parent = ActionGroup.transform;
            Collect.transform.parent = ActionGroup.transform;
            Release.transform.parent = ActionGroup.transform;

            UpdateCounters();
        }

        public static void SetupCompletionCount(string Area, int index, int x) {

            GameObject AreaCount = GameObject.Instantiate(CompletionRate.gameObject, GameObject.Find("_FinishlineDisplay(Clone)/").transform.GetChild(2));
            AreaCount.name = Area;
            AreaCount.GetComponent<TextMeshPro>().fontSize = 45f;
            AreaCount.layer = 5;
            AreaCount.transform.position = new Vector3(x, (140 - (index * 40)), 55f);
            AreaCount.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            AreaCount.SetActive(true);
            AreaCount.GetComponent<TextMeshPro>().text = $"";
            

            if (!StatSections.ContainsKey(Area)) { 
                StatSections.Add(Area, AreaCount);
            } else {
                StatSections[Area] = AreaCount;
            }
        }

        public static void UpdateCounters() {
            Dictionary<string, int> ChecksPerArea = new Dictionary<string, int>();
            Dictionary<string, int> CompletedChecksPerArea = new Dictionary<string, int>();
            int HolyCrossChecks = 0;
            int HolyCrossChecksFound = 0;
            foreach (string Area in StatSections.Keys) {
                if (Locations.MainAreasToSubAreas.ContainsKey(Area)) {
                    foreach (string SubArea in Locations.MainAreasToSubAreas[Area]) {
                        ChecksPerArea.Add(SubArea, 0);
                        CompletedChecksPerArea.Add(SubArea, 0);
                    }
                }
            }
            List<Check> Checks = TunicUtils.GetAllInUseChecks();
            foreach (Check check in Checks) {
                if (ChecksPerArea.ContainsKey(check.Location.SceneName)) {
                    ChecksPerArea[check.Location.SceneName]++;
                    if (check.IsCompletedOrCollected) {
                        CompletedChecksPerArea[check.Location.SceneName]++;
                    }
                }
                if (Locations.VanillaLocations.ContainsKey(check.CheckId) && check.Location.Requirements.Any(req => req.ContainsKey("21")) || new List<string>() { "Mountaintop", "Town_FiligreeRoom", "EastFiligreeCache" }.Contains(check.Location.SceneName)) {
                    HolyCrossChecks++;
                    if (check.IsCompletedOrCollected) {
                        HolyCrossChecksFound++;
                    }
                }
            }
            foreach (string Area in StatSections.Keys) {
                GameObject AreaCount = StatSections[Area];
                if (Locations.MainAreasToSubAreas.ContainsKey(Area)) {
                    float TotalAreaTime = 0.0f;
                    int TotalAreaChecks = 0;
                    int AreaChecksFound = 0;
                    float Percentage = 0;
                    bool IncludeCollected = SaveFlags.IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld;
                    foreach (string SubArea in Locations.MainAreasToSubAreas[Area]) {
                        TotalAreaChecks += ChecksPerArea[SubArea];
                        AreaChecksFound += CompletedChecksPerArea[SubArea];
                        TotalAreaTime += SaveFile.GetFloat($"randomizer play time {SubArea}");
                    }
                    if (TotalAreaChecks > 0) {
                        Percentage = ((float)AreaChecksFound / TotalAreaChecks) * 100.0f;
                    }
                    string TimeString = FormatTime(TotalAreaTime, false);// time.Hours == 0 ? $"{time.Minutes}m {time.Seconds}s" : $"{time.Hours}h {time.Minutes}m {time.Seconds}s";
                    string Color = AreaChecksFound == TotalAreaChecks ? $"<#{new Color32(234, 166, 20, 255).ColorToHex()}>" : "<#FFFFFF>";
                    AreaCount.GetComponent<TextMeshPro>().text = $"{(AreaChecksFound == TotalAreaChecks ? StatsScreenSecret[Area] : Area)}\n<size=80%>{TimeString}\n{Color}{AreaChecksFound} / {TotalAreaChecks} ({(int)Percentage}%)";

                    if (Area == "Swamp") {
                        if (IsArchipelago()) {
                            Release.SetActive(true);
                            Collect.SetActive(true);
                            if (Archipelago.instance.integration.session.RoomState.ReleasePermissions == Permissions.Disabled) {
                                Release.transform.GetChild(1).GetComponent<TextMeshPro>().text = "Release\n<size=80%>(disabled by host)";
                                Release.transform.GetChild(1).localPosition = new Vector3(145f, -142.5f, 0);
                                Release.transform.GetChild(0).GetComponent<PlatformSprite>().keySpriteStyle = KeySpriteStyle.Dark;
                                Release.transform.GetChild(0).GetComponent<PlatformSprite>().buttonStyle = KeySpriteStyle.Dark;
                            } else {
                                Release.transform.GetChild(1).GetComponent<TextMeshPro>().text = "Release";
                                Release.transform.GetChild(1).localPosition = new Vector3(145f, -147.5f, 0);
                                Release.transform.GetChild(0).GetComponent<PlatformSprite>().keySpriteStyle = KeySpriteStyle.Light;
                                Release.transform.GetChild(0).GetComponent<PlatformSprite>().buttonStyle = KeySpriteStyle.Light;
                            }
                            if (Archipelago.instance.integration.session.RoomState.CollectPermissions == Permissions.Disabled) {
                                Collect.transform.GetChild(1).GetComponent<TextMeshPro>().text = "Collect\n<size=80%>(disabled by host)";
                                Collect.transform.GetChild(1).localPosition = new Vector3(145f, -142.5f, 0);
                                Collect.transform.GetChild(0).GetComponent<PlatformSprite>().keySpriteStyle = KeySpriteStyle.Dark;
                                Collect.transform.GetChild(0).GetComponent<PlatformSprite>().buttonStyle = KeySpriteStyle.Dark;
                            } else {
                                Collect.transform.GetChild(1).GetComponent<TextMeshPro>().text = "Collect";
                                Collect.transform.GetChild(1).localPosition = new Vector3(145f, -147.5f, 0);
                                Collect.transform.GetChild(0).GetComponent<PlatformSprite>().keySpriteStyle = KeySpriteStyle.Light;
                                Collect.transform.GetChild(0).GetComponent<PlatformSprite>().buttonStyle = KeySpriteStyle.Light;
                            }
                        } else {
                            Release.SetActive(false);
                            Collect.SetActive(false);
                        }
                    }
                    if (Area == "Far Shore/Hero's Grave") {
                        AreaCount.GetComponent<TextMeshPro>().text = $"<size=90%>{AreaCount.GetComponent<TextMeshPro>().text}";
                    }
                }

                if (Area == "Holy Cross Checks") {
                    float Percentage = ((float)HolyCrossChecksFound / HolyCrossChecks) * 100;
                    string Color = HolyCrossChecksFound == HolyCrossChecks ? $"<#eaa614>" : "<#FFFFFF>";

                    AreaCount.GetComponent<TextMeshPro>().text = $"{(HolyCrossChecksFound == HolyCrossChecks ? StatsScreenSecret[Area] : Area)}\n<size=80%>{Color}{HolyCrossChecksFound} / {HolyCrossChecks} ({(int)Percentage}%)";
                }

                if (Area == "Player Deaths") {
                    AreaCount.GetComponent<TextMeshPro>().text = $"{StatsScreenSecret[Area]}\n{SaveFile.GetInt(PlayerDeathCount)}";
                }

                if (Area == "Bosses Defeated") {
                    int BossesDefeated = 0;
                    foreach (string BossState in EnemyRandomizer.CustomBossFlags) {
                        if (SaveFile.GetInt(BossState) == 1) {
                            BossesDefeated++;
                        }
                    }
                    string Color = BossesDefeated == 5 ? $"<#eaa614>" : "<#FFFFFF>";

                    AreaCount.GetComponent<TextMeshPro>().text = $"{(BossesDefeated == 5 ? StatsScreenSecret[Area] : Area)}\n<size=80%>{Color}{BossesDefeated} / 5 ({(BossesDefeated * 100) / 5}%)\n<#FFFFFF>Enemies Defeated: {SaveFile.GetInt(EnemiesDefeatedCount)}";
                }

                if (Area == "Time Found") {
                    string Text = $"<size=78%><#FFFFFF>";
                    List<float> HexTimes = new List<float>();
                    List<string> Times = new List<string>();
                    if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                        int HexGoal = SaveFile.GetInt(HexagonQuestGoal);
                        int HalfGoal = HexGoal / 2;
                        Text += $"1st Hex:\t{FormatTime(SaveFile.GetFloat("randomizer Gold Questagon 1 time"), true)}\t" +
                                $"{HalfGoal}{GetOrdinalSuffix(HalfGoal)} Hex:\t{FormatTime(SaveFile.GetFloat($"randomizer Gold Questagon {HalfGoal} time"), true)}\n" +
                                $"{HexGoal}{GetOrdinalSuffix(HexGoal)} Hex:\t{FormatTime(SaveFile.GetFloat($"randomizer Gold Questagon {SaveFile.GetInt(HexagonQuestGoal)} time"), true)}\t";
                    } else {
                        Text += $"Red Hex:\t{FormatTime(SaveFile.GetFloat("randomizer Red Questagon 1 time"), true)}\t" +
                                $"Green Hex:\t{FormatTime(SaveFile.GetFloat("randomizer Green Questagon 1 time"), true)}\n" +
                                $"Blue Hex:\t{FormatTime(SaveFile.GetFloat("randomizer Blue Questagon 1 time"), true)}\t";
                    }
                    System.Random random = new System.Random();
                    float SwordTime = SaveFile.GetInt(SwordProgressionEnabled) == 1 ? SaveFile.GetFloat("randomizer Sword Progression 2 time") : SaveFile.GetFloat("randomizer Sword 1 time");
                    float GrappleTime = SaveFile.GetFloat("randomizer Magic Orb 1 time");
                    float HyperdashTime = SaveFile.GetFloat("randomizer Hero's Laurels 1 time");
                    Text += $"Sword:\t{FormatTime(SwordTime, true)}\n" +
                            $"Magic Orb:\t{FormatTime(GrappleTime, true)}\t" +
                            $"Laurels:\t{FormatTime(HyperdashTime, true)}\n";
                    int Total = 6;
                    if (SaveFile.GetInt(AbilityShuffle) == 1) {
                        float PrayerTime = SaveFile.GetFloat(PrayerUnlockedTime);
                        float HolyCrossTime = SaveFile.GetFloat(HolyCrossUnlockedTime);
                        Text += $"Prayer:\t{FormatTime(PrayerTime, true)}\t" +
                                $"Holy Cross:\t{FormatTime(HolyCrossTime, true)}";
                        Total = 8;
                    }
                    int NotFound = Regex.Matches(Text, "Not Found").Count;
                    Text = $"\t\t{(NotFound > 0 ? Area : StatsScreenSecret[Area])} <size=80%>{(NotFound == 0 ? $"<#eaa614>" : "<#FFFFFF>")}({(Total - NotFound)} / {Total})\n" + Text;
                    AreaCount.GetComponent<TextMeshPro>().text = Text;
                }
                if (Area == "Total Completion") {
                    int CompletedCheckCount = TunicUtils.GetCompletedChecksCount(Checks);
                    int ChecksCollectedByOthers = Checks.Where(check => check.IsCollectedInAP).Count();
                    bool IncludeCollected = SaveFlags.IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld;
                    float CheckPercentage = 0;
                    int GrassCount = 0;
                    float GrassPercentage = 0f;
                    int TotalCheckCount = Checks.Count;
                    string Color = "<#FFFFFF>";
                    if (SaveFile.GetInt(GrassRandoEnabled) == 1) {
                        GrassCount = TunicUtils.GetCompletedChecksCount(GrassRandomizer.GrassChecks.Values.ToList());
                        GrassPercentage = ((float)GrassCount / GrassRandomizer.GrassChecks.Count) * 100.0f;
                    }
                    CheckPercentage = ((float)CompletedCheckCount / TotalCheckCount) * 100.0f;
                    Color = CompletedCheckCount == TotalCheckCount ? $"<#eaa614>" : "<#FFFFFF>";
                    AreaCount.GetComponent<TextMeshPro>().text = $"Overall Completion: {Color}{CompletedCheckCount}/{TotalCheckCount}" +
                        $"{(IncludeCollected ? "*" : "")} " +
                        $"({Math.Round(CheckPercentage, 2)}%) {((int)CheckPercentage == 69 ? "<size=40%>nice</size>" : "")}<#FFFFFF>" +
                        $"{(SaveFile.GetInt(GrassRandoEnabled) == 1 ? $"\n<size=80%>Grass Cut: {(GrassCount == GrassRandomizer.GrassChecks.Count ? "<#00FF00>" : "<#FFFFFF>")}{GrassCount}/{GrassRandomizer.GrassChecks.Count}{(IncludeCollected ? "*" : "")} ({Math.Round(GrassPercentage, 2)}%) {((int)GrassPercentage == 69 ? "<size=40%>nice</size>" : "")} {((int)GrassPercentage == 69 && (int)CheckPercentage == 69 ? "<size=40%>x2</size>" : "")} " : $"")}" +
                        $"<#FFFFFF>{(IncludeCollected ? $"\n<size=60%>*includes {ChecksCollectedByOthers} locations collected by others" : "")}";
                    AreaCount.GetComponent<TextMeshPro>().horizontalAlignment = HorizontalAlignmentOptions.Center;
                    AreaCount.GetComponent<TextMeshPro>().fontSize = 100f;
                }
                if (Area == "Timer") {
                    AreaCount.GetComponent<TextMeshPro>().text = FormatTime(SpeedrunData.inGameTime, false, true);
                    AreaCount.transform.GetChild(2).GetComponent<TextMeshPro>().text = FormatTime(SpeedrunData.inGameTime, false, true);
                }
            }

            if (((float)Screen.width / Screen.height) < 1.7f) {
                Vector3 pos = ActionGroup.transform.localPosition;
                ActionGroup.transform.localPosition = new Vector3(50f, pos.y, pos.z);
            }
        }

        public static string FormatTime(float Seconds, bool ItemTime, bool isIgt = false) {
            if (Seconds == 0.0f && ItemTime) {
                return "Not Found".PadRight(10);
            }

            TimeSpan TimeSpan = TimeSpan.FromSeconds(Seconds);
            string TimeString = $"{TimeSpan.Hours.ToString().PadLeft(2, '0')}:{TimeSpan.Minutes.ToString().PadLeft(2, '0')}:{TimeSpan.Seconds.ToString().PadLeft(2, '0')}{(isIgt ? $".{TimeSpan.Milliseconds.ToString().PadRight(3, '0')}" : "")}";
            return isIgt ? TimeString : TimeString.PadRight(10).Replace(":", "<size=100%>:<size=80%>");
        }

        private static string GetOrdinalSuffix(int num) {
            string number = num.ToString();
            if (number.EndsWith("11") || number.EndsWith("12") || number.EndsWith("13")) return "th";
            if (number.EndsWith("1")) return "st";
            if (number.EndsWith("2")) return "nd";
            if (number.EndsWith("3")) return "rd";
            return "th";
        }

        public static bool GameOverDecision___retry_PrefixPatch(GameOverDecision __instance) {
            for (int i = 0; i < 28; i++) {
                SaveFile.SetInt("unlocked page " + i, SaveFile.GetInt("randomizer picked up page " + i) == 1 ? 1 : 0);
            }
            if (CompletionCanvas != null) {
                CompletionCanvas.SetActive(false);
            }

            if (IsArchipelago()) {
                Archipelago.instance.integration.sentCompletion = false;
                Archipelago.instance.integration.sentCollect = false;
                Archipelago.instance.integration.sentRelease = false;
            }

            GameCompleted = false;
            
            return true;
        }

        public static bool GameOverDecision___newgame_PrefixPatch(GameOverDecision __instance) {
            if (CompletionCanvas != null) {
                CompletionCanvas.SetActive(false);
            }
            if (IsArchipelago()) {
                Archipelago.instance.integration.sentCompletion = false;
                Archipelago.instance.integration.sentCollect = false;
                Archipelago.instance.integration.sentRelease = false;
            }
            if (Profile.GetAccessibilityPref(Profile.AccessibilityPrefs.SpeedrunMode)) {
                SpeedrunFinishlineDisplay.HideFinishline();
            }
            GameCompleted = false;
            SpeedrunData.gameComplete = 0;
            PauseMenu.ReturnToTitleScreen();
            SceneLoaderPatches.SceneName = "TitleScreen";
            return false; 
        }

        public static void GameOverDecision_Start_PostfixPatch(GameOverDecision __instance) {
            int MissingPageCount = (28 - TunicRandomizer.Tracker.ImportantItems["Pages"]);
            string MissingPageText = $"Missing {MissingPageCount} page{(MissingPageCount != 1 ? "s" : "")}. Return to seek another path.";
            __instance.retryKey_plural = MissingPageText;
            __instance.retryKey_single = MissingPageText;
            __instance.newgame_option.transform.GetChild(1).GetComponent<LocalizeTMP>().enabled = false;
            __instance.newgame_option.transform.GetChild(1).GetComponent<RTLTMPro.RTLTextMeshPro>().text = "Return to Title Screen";
            __instance.newgame_option.transform.GetChild(2).GetComponent<LocalizeTMP>().enabled = false;
            __instance.newgame_option.transform.GetChild(2).GetComponent<RTLTMPro.RTLTextMeshPro>().text = "Thank you for playing the TUNIC Randomizer!";
        }
    }
}
