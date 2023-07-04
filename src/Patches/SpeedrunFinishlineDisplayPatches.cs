using BepInEx.Logging;
using Il2CppSystem.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TunicRandomizer {
    public class SpeedrunFinishlineDisplayPatches {
        private static ManualLogSource Logger = TunicRandomizer.Logger;

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
            {"Inventory items_book", "Pages"}
        };
        public static Dictionary<string, string> HeroRelicIcons = new Dictionary<string, string>() {
            {"Inventory items_offering_tooth", "Relic - Hero Sword"},
            {"Inventory items_offering_effigy", "Relic - Hero Crown"},
            {"Inventory items_offering_ash", "Relic - Hero Water"},
            {"Inventory items_offering_flower", "Relic - Hero Pendant HP"},
            {"Inventory items_offering_feather", "Relic - Hero Pendant MP"},
            {"Inventory items_offering_orb", "Relic - Hero Pendant SP"},
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
            { "Bottom of the Well", "Bottom of the We<#eaa614>ll<#ffffff>" },
            { "Spirit Areas", "Spi<#eaa614>r<#ffffff>it Areas" },
            { "Holy Cross Checks", "Ho<#eaa614>l<#ffffff>y Cross Checks" },
            { "Player Deaths", "P<#eaa614>l<#ffffff>ayer Deaths" },
            { "Time Found", "Time Foun<#eaa614>d<#ffffff>" },
        };

        public static GameObject CompletionRate;

        public static bool ShowSwordAfterDelay = false;
        public static bool ShowCompletionStatsAfterDelay = false;
        public static bool SpeedrunFinishlineDisplay_showFinishline_PrefixPatch(SpeedrunFinishlineDisplay __instance) {

            SpeedrunReportItem DathStone = ScriptableObject.CreateInstance<SpeedrunReportItem>();
            DathStone.icon = Inventory.GetItemByName("Homeward Bone Statue").icon;
            SpeedrunReportItem GoldenItem = ScriptableObject.CreateInstance<SpeedrunReportItem>();
            GoldenItem.icon = Inventory.GetItemByName("Spear").icon;
            SpeedrunReportItem Manual = ScriptableObject.CreateInstance<SpeedrunReportItem>();
            Manual.icon = Inventory.GetItemByName("Book").icon;

            SpeedrunFinishlineDisplay.instance.reportGroup_secrets = new SpeedrunReportItem[] {
                SpeedrunFinishlineDisplay.instance.reportGroup_secrets[0],
                SpeedrunFinishlineDisplay.instance.reportGroup_secrets[1],
                SpeedrunFinishlineDisplay.instance.reportGroup_secrets[2],
                SpeedrunFinishlineDisplay.instance.reportGroup_secrets[3],
                Manual,
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
                    if (HeroRelicIcons.ContainsKey(SpriteName) && Inventory.GetItemByName(HeroRelicIcons[SpriteName]).Quantity > 0) {
                        Icon.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().faceColor = new Color32(234, 166, 20, 255);
                    }
                    // Change sword icon for custom swords
                    if (SpriteName == "Inventory items_sword") {
                        if (SaveFile.GetInt("randomizer sword progression enabled") == 1) {
                            int SwordLevel = SaveFile.GetInt("randomizer sword progression level");
                            if (SwordLevel >= 3) {
                                Icon.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().faceColor = SwordLevel == 3 ? new Color32(255, 50, 150, 255) : new Color32(93, 231, 207, 255);
                                ShowSwordAfterDelay = true;
                            }
                        }
                    }
                }
            } catch (Exception ex) { }

            if (CompletionRate != null) { 
                GameObject.Destroy(CompletionRate.gameObject);
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
            GameObject CompletionCanvas = new GameObject("completion stats");
            CompletionCanvas.layer = 5;
            CompletionCanvas.transform.parent = GameObject.Find("_FinishlineDisplay(Clone)/").transform;
            CompletionCanvas.AddComponent<Canvas>();
            CompletionCanvas.transform.position = new Vector3(0f, 0f, 300f);
            CompletionCanvas.SetActive(false);

            int CheckCount = ItemRandomizer.ItemsPickedUp.Values.Where(Item => Item).ToList().Count;
            float CheckPercentage = ((float)CheckCount /ItemRandomizer.ItemList.Count) * 100.0f;
            GameObject TotalCompletion = GameObject.Instantiate(CompletionRate.gameObject, GameObject.Find("_FinishlineDisplay(Clone)/").transform.GetChild(2));
            TotalCompletion.transform.position = new Vector3(-60f, -30f, 55f);
            TotalCompletion.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            TotalCompletion.GetComponent<TextMeshPro>().text = $"Overall Completion: {CheckCount}/{ItemRandomizer.ItemList.Count} ({Math.Round(CheckPercentage, 2)}%)";
            if ((int)CheckPercentage == 69) {
                TotalCompletion.GetComponent<TextMeshPro>().text += " <size=40%>nice";
            }
            if ((int)CheckPercentage == 100) {
                TotalCompletion.GetComponent<TextMeshPro>().color = new Color32(234, 166, 20, 255);
            }
            TotalCompletion.GetComponent<TextMeshPro>().fontSize = 100f;
            TotalCompletion.SetActive(true);
            List<List<string>> Columns = new List<List<string>>() { 
                new List<string>(){"Overworld", "West Garden", "Ruined Atoll", "Quarry/Mountain", "Swamp"},
                new List<string>(){"East Forest", "Eastern Vault Fortress", "Library", "Rooted Ziggurat", "Cathedral"},
                new List<string>(){"Dark Tomb", "Frog's Domain", "Shop/Coin Wells", "Bosses Defeated", "Time Found"},
                new List<string>(){"Bottom of the Well", "Spirit Areas", "Holy Cross Checks", "Player Deaths"},
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
            
            if(Hints.MainAreasToSubAreas.ContainsKey(Area)) {
                float TotalAreaTime = 0.0f;
                int TotalAreaChecks = 0;
                int AreaChecksFound = 0;
                float Percentage = 0;
                foreach (string SubArea in Hints.MainAreasToSubAreas[Area]) {
                    TotalAreaChecks += ItemRandomizer.ItemList.Values.Where(Item => Item.Location.SceneName == SubArea).Count();
                    AreaChecksFound += ItemRandomizer.ItemList.Values.Where(Item => Item.Location.SceneName == SubArea && ItemRandomizer.ItemsPickedUp[$"{Item.Location.LocationId} [{Item.Location.SceneName}]"]).Count();
                    TotalAreaTime += SaveFile.GetFloat($"randomizer play time {SubArea}");
                }
                if (TotalAreaChecks > 0) {
                    Percentage = ((float)AreaChecksFound / TotalAreaChecks) * 100.0f;
                }
                string TimeString = FormatTime(TotalAreaTime, false);// time.Hours == 0 ? $"{time.Minutes}m {time.Seconds}s" : $"{time.Hours}h {time.Minutes}m {time.Seconds}s";
                string Color = AreaChecksFound == TotalAreaChecks ? $"<#{new Color32(234, 166, 20, 255).ColorToHex()}>" : "<#FFFFFF>";
                AreaCount.GetComponent<TextMeshPro>().text = $"{(AreaChecksFound == TotalAreaChecks ? StatsScreenSecret[Area] : Area)}\n<size=80%>{TimeString}\n{Color}{AreaChecksFound} / {TotalAreaChecks} ({(int)Percentage}%)";

                if (Area == "Swamp") {
                    AreaCount.GetComponent<TextMeshPro>().text += "\n<size=100%><#FFFFFF>(Press 1 to hide stats)";
                }
            }

            if (Area == "Holy Cross Checks") {
                int TotalChecks = 0;
                int ChecksFound = 0;
                float Percentage = 0;
                foreach (ItemData Item in ItemRandomizer.ItemList.Values) {
                    foreach (Dictionary<string, int> items in Item.Location.RequiredItems) {
                        if (items.ContainsKey("21")) {
                            TotalChecks++;
                            if (ItemRandomizer.ItemsPickedUp[$"{Item.Location.LocationId} [{Item.Location.SceneName}]"]) {
                                ChecksFound++;
                            }
                            continue;
                        }
                    }
                }
                Percentage = ((float)ChecksFound / TotalChecks) * 100;
                string Color = ChecksFound == TotalChecks ? $"<#eaa614>" : "<#FFFFFF>";

                AreaCount.GetComponent<TextMeshPro>().text = $"{(ChecksFound == TotalChecks ? StatsScreenSecret[Area] : Area)}\n<size=80%>{Color}{ChecksFound} / {TotalChecks} ({(int)Percentage}%)";
            }

            if (Area == "Player Deaths") {
                AreaCount.GetComponent<TextMeshPro>().text = $"{StatsScreenSecret[Area]}\n{SaveFile.GetInt("randomizer death count")}";
            }

            if (Area == "Bosses Defeated") {
                List<string> BossStateVars = new List<string>() {
                    "SV_Forest Boss Room_Skuladot redux Big",
                    "SV_Archipelagos Redux TUNIC Knight is Dead",
                    "SV_Fortress Arena_Spidertank Is Dead",
                    "Librarian Dead Forever",
                    "SV_ScavengerBossesDead"
                };
                int BossesDefeated = 0;
                foreach (string BossState in BossStateVars) {
                    if (StateVariable.GetStateVariableByName(BossState).BoolValue) {
                        BossesDefeated++;
                    }
                }
                string Color = BossesDefeated == 5 ? $"<#eaa614>" : "<#FFFFFF>";

                AreaCount.GetComponent<TextMeshPro>().text = $"{(BossesDefeated == 5 ? StatsScreenSecret[Area] : Area)}\n<size=80%>{Color}{BossesDefeated} / 5 ({(BossesDefeated*100)/5}%)\n<#FFFFFF>Enemies Defeated: {SaveFile.GetInt("randomizer enemies defeated")}";
            }

            if (Area == "Time Found") {
                string Text = $"<size=78%><#FFFFFF>";
                List<float> HexTimes = new List<float>();
                List<string> Times = new List<string>();
                if (SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST") {
                    Text += $"1st Hex:\t{FormatTime(SaveFile.GetFloat("randomizer Hexagon Gold 1 time"), true)}\t" +
                            $"10th Hex:\t{FormatTime(SaveFile.GetFloat("randomizer Hexagon Gold 10 time"), true)}\n" +
                            $"20th Hex:\t{FormatTime(SaveFile.GetFloat("randomizer Hexagon Gold 20 time"), true)}\t";
                } else {
                    Text += $"Red Hex:\t{FormatTime(SaveFile.GetFloat("randomizer Hexagon Red 1 time"), true)}\t" +
                            $"Green Hex:\t{FormatTime(SaveFile.GetFloat("randomizer Hexagon Green 1 time"), true)}\n" +
                            $"Blue Hex:\t{FormatTime(SaveFile.GetFloat("randomizer Hexagon Blue 1 time"), true)}\t";
                }
                System.Random random = new System.Random();
                float SwordTime = SaveFile.GetInt("randomizer sword progression enabled") == 1 ? SaveFile.GetFloat("randomizer Sword Progression 2 time") : SaveFile.GetFloat("randomizer Sword 1 time");
                float GrappleTime = SaveFile.GetFloat("randomizer Wand 1 time");
                float HyperdashTime = SaveFile.GetFloat("randomizer Hyperdash 1 time");
                Text += $"Sword:\t{FormatTime(SwordTime, true)}\n" +
                        $"Grapple:\t{FormatTime(GrappleTime, true)}\t" +
                        $"Laurels:\t{FormatTime(HyperdashTime, true)}\n";
                int Total = 6;
                if (SaveFile.GetInt("randomizer shuffled abilities") == 1) {
                    float PrayerTime = SaveFile.GetFloat("randomizer Page 12 time");
                    float HolyCrossTime = SaveFile.GetFloat("randomizer Page 21 time");
                    Text += $"Prayer:\t{FormatTime(PrayerTime, true)}\t" +
                            $"Holy Cross:\t{FormatTime(HolyCrossTime, true)}";
                    Total = 8;
                }
                int NotFound = Regex.Matches(Text, "Not Found").Count;
                Text = $"\t\t{(NotFound > 0 ? Area : StatsScreenSecret[Area])} <size=80%>{(NotFound == 0 ? $"<#eaa614>" : "<#FFFFFF>" )}({(Total-NotFound)} / {Total})\n" + Text;
                AreaCount.GetComponent<TextMeshPro>().text = Text;
            }

        }

        public static string FormatTime(float Seconds, bool ItemTime) {
            if (Seconds == 0.0f && ItemTime) {
                return "Not Found".PadRight(10); 
            }

            TimeSpan TimeSpan = TimeSpan.FromSeconds(Seconds);
            string TimeString = $"{TimeSpan.Hours.ToString().PadLeft(2, '0')}:{TimeSpan.Minutes.ToString().PadLeft(2, '0')}:{TimeSpan.Seconds.ToString().PadLeft(2, '0')}";
            return TimeString.PadRight(10).Replace(":", "<size=100%>:<size=80%>");
        }

        public static bool GameOverDecision___retry_PrefixPatch(GameOverDecision __instance) {
            for (int i = 0; i < 28; i++) {
                SaveFile.SetInt("unlocked page " + i, SaveFile.GetInt("randomizer picked up page " + i) == 1 ? 1 : 0);
            }
            return true;
        }

        public static void GameOverDecision_Start_PostfixPatch(GameOverDecision __instance) {
            int MissingPageCount = (28 - TunicRandomizer.Tracker.ImportantItems["Pages"]);
            __instance.retryKey_plural = $"Missing {MissingPageCount} pages. Return to seek another path.";
            __instance.retryKey_single = $"Missing {MissingPageCount} page. Return to seek another path.";
        }

    }
}
