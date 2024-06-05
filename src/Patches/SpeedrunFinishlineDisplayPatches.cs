using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
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

        public static bool ShowCompletionStatsAfterDelay = false;
        public static bool GameCompleted = false;

        public static bool SpeedrunFinishlineDisplay_showFinishline_PrefixPatch(SpeedrunFinishlineDisplay __instance) {

            SpeedrunReportItem DathStone = ScriptableObject.CreateInstance<SpeedrunReportItem>();
            DathStone.icon = Inventory.GetItemByName("Dath Stone").icon;
            SpeedrunReportItem GoldenItem = ScriptableObject.CreateInstance<SpeedrunReportItem>();
            GoldenItem.icon = Inventory.GetItemByName("Spear").icon;
            SpeedrunReportItem ManualOrGoldHex = ScriptableObject.CreateInstance<SpeedrunReportItem>();
            if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                ManualOrGoldHex.icon = Inventory.GetItemByName("Hexagon Gold").icon;
            } else {
                ManualOrGoldHex.icon = Inventory.GetItemByName("Book").icon;
            }

            SpeedrunFinishlineDisplay.instance.reportGroup_secrets = new SpeedrunReportItem[] {
                SpeedrunFinishlineDisplay.instance.reportGroup_secrets[0],
                SpeedrunFinishlineDisplay.instance.reportGroup_secrets[1],
                SpeedrunFinishlineDisplay.instance.reportGroup_secrets[2],
                SpeedrunFinishlineDisplay.instance.reportGroup_secrets[3],
                ManualOrGoldHex,
                DathStone,
                GoldenItem
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
            if (icon.name == "Inventory items_money triangle") {
                if (TunicRandomizer.Tracker.ImportantItems["Golden Trophies"] == 12) {
                    quantity = 1;
                    return true;
                } else {
                    return false;
                }
            }
            if (icon.name == "Inventory items_sword" && Inventory.GetItemByName("Sword").Quantity > 0) {
                quantity = 1;
                return true;
            }
            if (TunicRandomizer.Tracker.ImportantItems[ReportGroupItems[icon.name]] == 0) {
                return false;
            }

            quantity = TunicRandomizer.Tracker.ImportantItems[ReportGroupItems[icon.name]];
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
            try {
                foreach (ItemIcon Icon in Resources.FindObjectsOfTypeAll<ItemIcon>().Where(icon => icon.transform.parent.name.Contains("Item Parade Group"))) {
                    string SpriteName = Icon.transform.GetChild(1).GetComponent<Image>().sprite.name;
                    // Change quantity text to gold if hero relic obtained
                    if (HeroRelicIcons.ContainsKey(SpriteName) && Inventory.GetItemByName(HeroRelicIcons[SpriteName].Item1).Quantity > 0) {
                        Icon.transform.GetChild(1).GetComponent<Image>().sprite = ModelSwaps.FindSprite(HeroRelicIcons[SpriteName].Item2);
                    }
                    // Change sword icon for custom swords
                    if (SpriteName == "Inventory items_sword") {
                        if (SaveFile.GetInt(SwordProgressionEnabled) == 1) {
                            int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                            if (SwordLevel == 3) {
                                Icon.transform.GetChild(1).GetComponent<Image>().sprite = Inventory.GetItemByName("Librarian Sword").icon;
                            } else if(SwordLevel == 4) {
                                Icon.transform.GetChild(1).GetComponent<Image>().sprite = Inventory.GetItemByName("Heir Sword").icon;
                            }
                        }
                    }
                }
            } catch (Exception ex) { }
        }

        public static void SetupCompletionStatsDisplay() {

            if (CompletionRate != null) {
                GameObject.Destroy(CompletionRate.gameObject);
            }
            if (CompletionCanvas != null) {
                GameObject.Destroy(CompletionCanvas.gameObject);
            }
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

            int CheckCount = 0;
            int ChecksCollectedByOthers = 0;
            float CheckPercentage = 0;
            string Color = "<#FFFFFF>";


            CheckCount = Locations.VanillaLocations.Keys.Where(Check => Locations.CheckedLocations[Check] || (IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld && SaveFile.GetInt($"randomizer {Check} was collected") == 1)).Count();
            ChecksCollectedByOthers = IsArchipelago() ? Locations.VanillaLocations.Keys.Where(Check => !Locations.CheckedLocations[Check] && SaveFile.GetInt($"randomizer {Check} was collected") == 1).Count() : 0;
            CheckPercentage = ((float)CheckCount / Locations.VanillaLocations.Count) * 100.0f;
            Color = CheckCount == Locations.VanillaLocations.Count ? $"<#eaa614>" : "<#FFFFFF>";

            GameObject TotalCompletion = GameObject.Instantiate(CompletionRate.gameObject, GameObject.Find("_FinishlineDisplay(Clone)/").transform.GetChild(2));
            TotalCompletion.transform.position = new Vector3(-60f, -30f, 55f);
            TotalCompletion.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);

            TotalCompletion.GetComponent<TextMeshPro>().text = $"Overall Completion: {Color}{CheckCount}/{Locations.VanillaLocations.Count}" +
                $"{(SaveFlags.IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld ? "*" : "")} " +
                $"({Math.Round(CheckPercentage, 2)}%) {((int)CheckPercentage == 69 ? "<size=40%>nice</size>" : "")}" +
                $"{(SaveFlags.IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld ? $"\n\t<size=60%>*includes {ChecksCollectedByOthers} locations collected by others" : "")}";

            TotalCompletion.GetComponent<TextMeshPro>().fontSize = 100f;
            TotalCompletion.SetActive(true);
            List<List<string>> Columns = new List<List<string>>() {
                new List<string>(){"Overworld", "West Garden", "Ruined Atoll", "Quarry/Mountain", "Swamp"},
                new List<string>(){"East Forest", "Eastern Vault Fortress", "Library", "Rooted Ziggurat", "Cathedral"},
                new List<string>(){"Dark Tomb", "Frog's Domain", "Shop/Coin Wells", "Bosses Defeated", "Time Found"},
                new List<string>(){"Beneath the Well", "Far Shore/Hero's Grave", "Holy Cross Checks", "Player Deaths"},
            };
            List<int> Spacings = new List<int>() { -300, -198, 370, 465 };
            if ((Screen.width == 1920 && Screen.height == 1080) || (Screen.width == 2560 && Screen.height == 1440)) {
                Spacings = new List<int>() { -345, -243, 410, 503 };
            }
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

            if (!Profile.GetAccessibilityPref(Profile.AccessibilityPrefs.SpeedrunMode)) {
                GameObject TimeText = GameObject.Instantiate(SpeedrunFinishlineDisplay.instance.transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
                TimeText.transform.parent = CompletionCanvas.transform;
                TimeText.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                TimeText.transform.position = new Vector3(0, 185f, 210f);
                TimeText.GetComponent<TextMeshPro>().text = FormatTime(SpeedrunData.inGameTime, false, true);
                TimeText.transform.GetChild(2).GetComponent<TextMeshPro>().text = FormatTime(SpeedrunData.inGameTime, false, true);
            }

            ShowCompletionStatsAfterDelay = true;
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

            if (Locations.MainAreasToSubAreas.ContainsKey(Area)) {
                float TotalAreaTime = 0.0f;
                int TotalAreaChecks = 0;
                int AreaChecksFound = 0;
                float Percentage = 0;
                foreach (string SubArea in Locations.MainAreasToSubAreas[Area]) {
                    TotalAreaChecks += Locations.VanillaLocations.Keys.Where(Check => Locations.VanillaLocations[Check].Location.SceneName == SubArea).Count();
                    AreaChecksFound += Locations.VanillaLocations.Keys.Where(Check => Locations.VanillaLocations[Check].Location.SceneName == SubArea && (Locations.CheckedLocations[Check] || (SaveFlags.IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld && SaveFile.GetInt($"randomizer {Check} was collected") == 1))).Count();
                    TotalAreaTime += SaveFile.GetFloat($"randomizer play time {SubArea}");
                }
                if (TotalAreaChecks > 0) {
                    Percentage = ((float)AreaChecksFound / TotalAreaChecks) * 100.0f;
                }
                string TimeString = FormatTime(TotalAreaTime, false);// time.Hours == 0 ? $"{time.Minutes}m {time.Seconds}s" : $"{time.Hours}h {time.Minutes}m {time.Seconds}s";
                string Color = AreaChecksFound == TotalAreaChecks ? $"<#{new Color32(234, 166, 20, 255).ColorToHex()}>" : "<#FFFFFF>";
                AreaCount.GetComponent<TextMeshPro>().text = $"{(AreaChecksFound == TotalAreaChecks ? StatsScreenSecret[Area] : Area)}\n<size=80%>{TimeString}\n{Color}{AreaChecksFound} / {TotalAreaChecks} ({(int)Percentage}%)";

                if (Area == "Swamp") {
                    AreaCount.GetComponent<TextMeshPro>().text += "\n<size=100%><#FFFFFF>Press 1 to hide stats.";
                    if (IsArchipelago()) {
                        AreaCount.GetComponent<TextMeshPro>().text += "\nPress R to release items.\nPress C to collect items.";
                    }
                }
                if (Area == "Far Shore/Hero's Grave") {
                    AreaCount.GetComponent<TextMeshPro>().text = $"<size=90%>{AreaCount.GetComponent<TextMeshPro>().text}";
                }
            }

            if (Area == "Holy Cross Checks") {
                int TotalChecks = 0;
                int ChecksFound = 0;
                float Percentage = 0;
                foreach (string Key in Locations.VanillaLocations.Keys) {
                    Check check = Locations.VanillaLocations[Key];
                    if (check.Location.Requirements.Any(req => req.ContainsKey("21")) || new List<string>() { "Mountaintop", "Town_FiligreeRoom", "EastFiligreeCache" }.Contains(check.Location.SceneName)) {
                        TotalChecks++;
                        if (Locations.CheckedLocations[Key] || (SaveFlags.IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld && SaveFile.GetInt($"randomizer {Key} was collected") == 1)) {
                            ChecksFound++;
                        }
                    }
                }
                Percentage = ((float)ChecksFound / TotalChecks) * 100;
                string Color = ChecksFound == TotalChecks ? $"<#eaa614>" : "<#FFFFFF>";

                AreaCount.GetComponent<TextMeshPro>().text = $"{(ChecksFound == TotalChecks ? StatsScreenSecret[Area] : Area)}\n<size=80%>{Color}{ChecksFound} / {TotalChecks} ({(int)Percentage}%)";
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
                GameObject.Destroy(CompletionCanvas);
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
                GameObject.Destroy(CompletionCanvas);
            }
            if (IsArchipelago()) {
                Archipelago.instance.integration.sentCompletion = false;
                Archipelago.instance.integration.sentCollect = false;
                Archipelago.instance.integration.sentRelease = false;
            }

            GameCompleted = false;
            
            return true;
        }

        public static void GameOverDecision_Start_PostfixPatch(GameOverDecision __instance) {
            int MissingPageCount = (28 - TunicRandomizer.Tracker.ImportantItems["Pages"]);
            __instance.retryKey_plural = $"Missing {MissingPageCount} pages. Return to seek another path.";
            __instance.retryKey_single = $"Missing {MissingPageCount} page. Return to seek another path.";
        }
    
    }
}
