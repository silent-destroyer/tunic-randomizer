using Archipelago.MultiClient.Net.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static TunicRandomizer.GhostHints;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {

    public class ItemTracker {
        
        public struct SceneInfo {
            public int SceneId;
            public string SceneName;
        }

        public int Seed;

        public SceneInfo CurrentScene;

        public Dictionary<string, int> ImportantItems = new Dictionary<string, int>() {
            {"Stick", 0},
            {"Sword", 0},
            {"Sword Progression", 0},
            {"Stundagger", 0},
            {"Techbow", 0},
            {"Wand", 0},
            {"Hyperdash", 0},
            {"Lantern", 0},
            {"Shield", 0},
            {"Shotgun", 0},
            {"SlowmoItem", 0},
            {"Key (House)", 0},
            {"Vault Key (Red)", 0},
            {"Trinket Coin", 0},
            {"Coins Tossed", 0},
            {"Trinket Slot", 1},
            {"Trinket Cards", 0},
            {"Flask Shard", 0},
            {"Flask Container", 0},
            {"Pages", 0},
            {"Prayer", 0},
            {"Holy Cross", 0},
            {"Icebolt", 0},
            {"Fairies", 0},
            {"Golden Trophies", 0},
            {"Dath Stone", 0},
            {"Mask", 0},
            {"Upgrade Offering - Attack - Tooth", 0},
            {"Upgrade Offering - DamageResist - Effigy", 0},
            {"Upgrade Offering - PotionEfficiency Swig - Ash", 0},
            {"Upgrade Offering - Health HP - Flower", 0},
            {"Upgrade Offering - Magic MP - Mushroom", 0},
            {"Upgrade Offering - Stamina SP - Feather", 0},
            {"Relic - Hero Sword", 0},
            {"Relic - Hero Crown", 0},
            {"Relic - Hero Water", 0},
            {"Relic - Hero Pendant HP", 0},
            {"Relic - Hero Pendant MP", 0},
            {"Relic - Hero Pendant SP", 0},
            {"Level Up - Attack", 0},
            {"Level Up - DamageResist", 0},
            {"Level Up - PotionEfficiency", 0},
            {"Level Up - Health", 0},
            {"Level Up - Stamina", 0},
            {"Level Up - Magic", 0},
            {"Hexagon Red", 0},
            {"Hexagon Green", 0},
            {"Hexagon Blue", 0},
            {"Hexagon Gold", 0},
            {"Grass", 0}
        };

        public Dictionary<string, string> DiscoveredEntrances = new Dictionary<string, string>();

        public List<ItemData> ItemsCollected = new List<ItemData>();

        public ItemTracker() {
            CurrentScene = new SceneInfo();

            Seed = 0;
            foreach (string Key in ImportantItems.Keys.ToList()) {
                ImportantItems[Key] = 0;
            }
            if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                TunicRandomizer.Tracker.ImportantItems["Pages"] = 28;
            }
            ImportantItems["Trinket Slot"] = 1;
            ImportantItems["Coins Tossed"] = StateVariable.GetStateVariableByName("Trinket Coins Tossed").IntValue;
            ItemsCollected = new List<ItemData>();
            DiscoveredEntrances = new Dictionary<string, string>();
        }

        public ItemTracker(int seed) {
            CurrentScene = new SceneInfo();
            Seed = seed;
            ItemsCollected = new List<ItemData>();
            DiscoveredEntrances = new Dictionary<string, string>();
        }

        public void ResetTracker() {
            Seed = SaveFile.GetInt("seed");
            foreach (string Key in ImportantItems.Keys.ToList()) {
                ImportantItems[Key] = 0;
            }
            if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                TunicRandomizer.Tracker.ImportantItems["Pages"] = 28;
            }
            ImportantItems["Trinket Slot"] = 1;
            ImportantItems["Coins Tossed"] = StateVariable.GetStateVariableByName("Trinket Coins Tossed").IntValue;
            ItemsCollected = new List<ItemData>();
            DiscoveredEntrances = new Dictionary<string, string>();
        }

        public void SetCollectedItem(string ItemName, bool WriteToDisk) {

            ItemData Item = ItemLookup.Items[ItemName];

            if (ImportantItems.ContainsKey(Item.ItemNameForInventory) && Item.Type != ItemTypes.SWORDUPGRADE) {
                ImportantItems[Item.ItemNameForInventory]++;

                if (Item.ItemNameForInventory == "Flask Shard" && ImportantItems["Flask Shard"] % 3 == 0) {
                    ImportantItems["Flask Container"]++;
                }
            }

            if(Item.Type == ItemTypes.FAIRY) {
                ImportantItems["Fairies"]++;
            }

            if (Item.Type == ItemTypes.TRINKET) {
                ImportantItems["Trinket Cards"]++;
            }
            
            if (Item.Type == ItemTypes.PAGE) {
                if (!IsHexQuestWithPageAbilities()) {
                    ImportantItems["Pages"]++;
                }

                if (Item.Name == "Pages 24-25 (Prayer)") { ImportantItems["Prayer"]++; }
                if (Item.Name == "Pages 42-43 (Holy Cross)") { ImportantItems["Holy Cross"]++; }
                if (Item.Name == "Pages 52-53 (Icebolt)") { ImportantItems["Icebolt"]++; }
            }
            
            if (Item.Type == ItemTypes.GOLDENTROPHY) {
                ImportantItems["Golden Trophies"]++;

                if (ImportantItems["Golden Trophies"] >= 12) {
                    Inventory.GetItemByName("Spear").Quantity = 1;
                }
            }

            if (IsHexQuestWithHexAbilities()) {
                if (Inventory.GetItemByName("Hexagon Gold").Quantity == SaveFile.GetInt(HexagonQuestPrayer)) { ImportantItems["Prayer"]++; }
                if (Inventory.GetItemByName("Hexagon Gold").Quantity == SaveFile.GetInt(HexagonQuestHolyCross)) { ImportantItems["Holy Cross"]++; }
                if (Inventory.GetItemByName("Hexagon Gold").Quantity == SaveFile.GetInt(HexagonQuestIcebolt)) { ImportantItems["Icebolt"]++; }
            }
            
            if (Item.Type == ItemTypes.SWORDUPGRADE) {
                if (SaveFile.GetInt(SwordProgressionEnabled) == 1) {
                    int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                    ImportantItems["Stick"] = 1;
                    if (SwordLevel > 1) {
                        ImportantItems["Sword"] = SwordLevel-1;
                    }
                    ImportantItems["Sword Progression"]++;
                } else {
                    ImportantItems[Item.ItemNameForInventory]++;
                }
            }

            foreach (string LevelUp in ItemLookup.LevelUpItems) {
                ImportantItems[LevelUp] = Inventory.GetItemByName(LevelUp).Quantity;
            }

            ItemsCollected.Add(Item);
            if (WriteToDisk) {
                SaveTrackerFile();
            }
        }

        public void PopulateTrackerForAP() {
            if (IsArchipelago()) {
                for (int i = 0; i < Archipelago.instance.integration.session.Items.AllItemsReceived.Count; i++) {
                    if (SaveFile.GetInt($"randomizer processed item index {i}") == 1) {
                        SetCollectedItem(Archipelago.instance.integration.session.Items.AllItemsReceived[i].ItemDisplayName, false);
                    }
                }
                SaveTrackerFile();
            }
        }

        public void PopulateDiscoveredEntrances() {
            foreach (PortalCombo portalCombo in ERData.RandomizedPortals) {
                if (SaveFile.GetInt("randomizer entered portal " + portalCombo.Portal1.Name) == 1) {
                    DiscoveredEntrances[portalCombo.Portal1.SceneDestinationTag] = portalCombo.Portal2.SceneDestinationTag;
                }
                if (!GetBool(Decoupled)) {
                    if (SaveFile.GetInt("randomizer entered portal " + portalCombo.Portal2.Name) == 1) {
                        DiscoveredEntrances[portalCombo.Portal2.SceneDestinationTag] = portalCombo.Portal1.SceneDestinationTag;
                    }
                }
            }
            if (SaveFile.GetInt(EntranceRando) == 1) { 
                try {
                    WriteEntranceFile();
                } catch (Exception e) {
                    TunicLogger.LogError("Error generated entrance tracker file: " + e.Message);
                }
            }
            SaveTrackerFile();
        }

        public static List<PortalCombo> EntranceFileAllPortals = new List<PortalCombo>();
        public void WriteEntranceFile() {
            string fileContents = "";
            List<string> allInUsePortalNames = new List<string>();
            if (GetBool(FoxPrinceEnabled)) {
                foreach (PortalCombo portalCombo in EntranceFileAllPortals) {
                    allInUsePortalNames.Add(portalCombo.Portal1.Name);
                    if (!GetBool(Decoupled)) {
                        allInUsePortalNames.Add(portalCombo.Portal2.Name);
                    }
                }
            } else {
                allInUsePortalNames = ERData.RandomizedPortals.Select(p => p.Portal1.Name).ToList();
            }
            int numberOfShops = allInUsePortalNames.Where(p => p.StartsWith("Shop Portal")).Count();

            Dictionary<string, PortalCombo> portalNameToPair = new Dictionary<string, PortalCombo>();
            
            foreach (PortalCombo portalCombo in ERData.RandomizedPortals) {
                portalNameToPair.Add(portalCombo.Portal1.Name, portalCombo);
            }

            Dictionary<string, List<string>> regionsToPortals = new Dictionary<string, List<string>>();

            List<string> addedPortals = new List<string>();

            void addPortal(string portalName, string portalRegion) {
                addedPortals.Add(portalName);
                PortalCombo portalCombo = portalNameToPair[portalName];
                string portalLine = portalCombo.Portal1.Name + ",-->,";
                if (SaveFile.GetInt("randomizer entered portal " + portalName) == 1) {
                    portalLine += portalCombo.Portal2.Name;
                }
                regionsToPortals[Locations.SimplifiedSceneNames[portalRegion]].Add(portalLine);
            }

            // list of all portals in order, for the purpose of sorting the entrance file
            foreach (KeyValuePair<string, Dictionary<string, List<ERData.TunicPortal>>> portalGroup in ERData.RegionPortalsList) {
                if (!regionsToPortals.ContainsKey(portalGroup.Key) && Locations.SimplifiedSceneNames.ContainsKey(portalGroup.Key)) {
                    regionsToPortals.Add(Locations.SimplifiedSceneNames[portalGroup.Key], new List<string>());
                }
                foreach (List<ERData.TunicPortal> portalList in portalGroup.Value.Values) {
                    foreach (ERData.TunicPortal portal in portalList) {
                        if (allInUsePortalNames.Contains(portal.Name)) {
                            addPortal(portal.Name, portalGroup.Key);
                        }
                    }
                }
            }

            // the sorting stuff above skips shops, and they're at the end anyway so let's just add them here
            List<string> leftoverPortals = allInUsePortalNames.Where(x => !addedPortals.Contains(x) && x.StartsWith("Shop")).OrderBy(x => int.Parse(x.Split(' ').Last())).ToList();
            foreach (string shopPortal in leftoverPortals) {
                addPortal(shopPortal, "Shop");
            }

            fileContents = "From,,To\n";
            foreach (KeyValuePair<string, List<string>> pair in regionsToPortals) {
                fileContents += pair.Key + ",,\n";
                foreach (string portalLine in pair.Value) {
                    fileContents += portalLine + "\n";
                }
                fileContents += ",,\n";
            }

            TunicUtils.TryWriteFile(TunicRandomizer.EntranceTrackerPath, fileContents);
        }

        public static void SaveTrackerFile() {
            TunicUtils.TryWriteFile(TunicRandomizer.ItemTrackerPath, JsonConvert.SerializeObject(TunicRandomizer.Tracker, Formatting.Indented));
        }

        public static void PopulateSpoilerLog() {
            if (TunicRandomizer.Settings.RaceMode) { return; }
            if (IsArchipelago() && Archipelago.instance.integration.disableSpoilerLog) {
                TunicUtils.TryDeleteFile(TunicRandomizer.SpoilerLogPath);
                return;
            }

            Dictionary<string, Check> AllLocations = TunicUtils.GetAllInUseChecksDictionary();

            int seed = SaveFile.GetInt("seed");
            Dictionary<string, List<string>> SpoilerLog = new Dictionary<string, List<string>>();
            foreach (string Key in Locations.SceneNamesForSpoilerLog.Keys) {
                SpoilerLog[Key] = new List<string>();
            }

            if (IsArchipelago()) {
                foreach (string Key in ItemLookup.ItemList.Keys) {
                    ItemInfo Item = ItemLookup.ItemList[Key];

                    string Spoiler = $"\t{((Locations.CheckedLocations[Key] || SaveFile.GetInt($"randomizer picked up {Key}") == 1 || (TunicRandomizer.Settings.CollectReflectsInWorld && SaveFile.GetInt($"randomizer {Key} was collected") == 1)) ? "x" : "-")} {Locations.LocationIdToDescription[Key]}: {Item.ItemDisplayName} ({Item.Player.Name})";

                    SpoilerLog[AllLocations[Key].Location.SceneName].Add(Spoiler);
                }
            }
            if (IsSinglePlayer()) {
                foreach(string Key in Locations.RandomizedLocations.Keys) {
                    Check Check = Locations.RandomizedLocations[Key];
                    ItemData Item = ItemLookup.GetItemDataFromCheck(Check);
                    string Spoiler = $"\t{(Locations.CheckedLocations[Key] ? "x" : "-")} {Locations.LocationIdToDescription[Key]}: {Item.Name}";
                    SpoilerLog[Check.Location.SceneName].Add(Spoiler);
                }
            }
            List<string> SpoilerLogLines = new List<string>() {
                IsSinglePlayer() ? $"Seed + Settings: {TunicRandomizer.Settings.GetSettingsString()}" : $"Seed: {seed}",
                "\nLines that start with 'x' instead of '-' represent items that have been collected\n",
            };
            if (IsArchipelago()) {
                SpoilerLogLines.Add("Major Items");
                foreach (string MajorItem in ItemLookup.MajorItems) {
                    if(MajorItem == "Gold Questagon") { continue; }
                    if(Locations.MajorItemLocations.ContainsKey(MajorItem) && Locations.MajorItemLocations[MajorItem].Count > 0) {
                        foreach (ArchipelagoHint apHint in Locations.MajorItemLocations[MajorItem]) {
                            bool HasItem = false;
                            if (Archipelago.instance.integration.session.Items.AllItemsReceived.Any(itemInfo => itemInfo.LocationDisplayName == apHint.Location && itemInfo.Player == (int)apHint.Player)) {
                                HasItem = true;
                            }
                            string Spoiler = $"\t{(HasItem ? "x" : "-")} {MajorItem}: {apHint.Location} ({Archipelago.instance.GetPlayerName((int)apHint.Player)}'s World)";
                            SpoilerLogLines.Add(Spoiler);
                        }
                    }
                }
            }
            if (IsSinglePlayer()) {
                SpoilerLogLines.AddRange(GetMysterySeedSettingsForSpoilerLog());

                SpoilerLogLines.Add("Major Items");
                List<string> MajorItems = new List<string>(ItemLookup.LegacyMajorItems);
                if (SaveFile.GetInt(GrassRandoEnabled) == 1) {
                    MajorItems.Add("Trinket - Glass Cannon");
                }
                if (SaveFile.GetInt(LadderRandoEnabled) == 1) {
                    MajorItems.AddRange(ItemRandomizer.LadderItems);
                }
                if (SaveFile.GetInt(FuseShuffleEnabled) == 1) {
                    MajorItems.AddRange(ItemRandomizer.FuseItems);
                }
                if (SaveFile.GetInt(BellShuffleEnabled) == 1) {
                    MajorItems.AddRange(ItemRandomizer.BellItems);
                }
                foreach (string MajorItem in MajorItems) {
                    foreach (Check Check in ItemRandomizer.FindAllRandomizedItemsByName(MajorItem)) {
                        ItemData ItemData = ItemLookup.GetItemDataFromCheck(Check);
                        string Key = Check.CheckId;
                        string Spoiler = $"\t{(Locations.CheckedLocations[Key] ? "x" : "-")} {ItemData.Name}: {Locations.SceneNamesForSpoilerLog[Check.Location.SceneName]} - {Locations.LocationIdToDescription[Key]}";
                        SpoilerLogLines.Add(Spoiler);
                    }
                }
            }

            if (IsHexQuestWithHexAbilities()) {
                SpoilerLogLines.Add($"\t{(SaveFile.GetInt(PrayerUnlocked) == 1 ? "x" : "-")} Prayer: {SaveFile.GetInt(HexagonQuestPrayer)} Gold Questagons");
                SpoilerLogLines.Add($"\t{(SaveFile.GetInt(HolyCrossUnlocked) == 1 ? "x" : "-")} Holy Cross: {SaveFile.GetInt(HexagonQuestHolyCross)} Gold Questagons");
                SpoilerLogLines.Add($"\t{(SaveFile.GetInt(IceBoltUnlocked) == 1 ? "x" : "-")} Icebolt: {SaveFile.GetInt(HexagonQuestIcebolt)} Gold Questagons");
            }
            
            foreach (string Key in SpoilerLog.Keys) {
                if (SpoilerLog[Key].Count == 0) {
                    continue;
                }
                SpoilerLogLines.Add(Locations.SceneNamesForSpoilerLog[Key]);
                SpoilerLog[Key].Sort();
                foreach (string line in SpoilerLog[Key]) {
                    SpoilerLogLines.Add(line);
                }
            }

            if (SaveFile.GetInt(EntranceRando) == 1) {
                List<string> portalPairs = new List<string>();
                SpoilerLogLines.Add("\nEntrance Connections");
                foreach (PortalCombo portalCombo in ERData.RandomizedPortals) {
                    portalPairs.Add(portalCombo.Portal1.Name + " --> " + portalCombo.Portal2.Name);
                }
                // list of all portals in order, for the purpose of sorting the portal spoiler
                List<string> refList = new List<string>();
                foreach (Dictionary<string, List<ERData.TunicPortal>> portalGroup in ERData.RegionPortalsList.Values) {
                    foreach (List<ERData.TunicPortal> portalList in portalGroup.Values) {
                        foreach (ERData.TunicPortal portal in portalList) {
                            refList.Add(portal.Name);
                        }
                    }
                }
                refList.AddRange(new List<string> { "Shop Portal 1", "Shop Portal 2", "Shop Portal 3", "Shop Portal 4", "Shop Portal 5", "Shop Portal 6", "Shop Portal 7", "Shop Portal 8" });

                List<string> sortedPairs = portalPairs.OrderBy(pair => refList.IndexOf(pair.Split(new[] { " --> " }, System.StringSplitOptions.None)[0])).ToList();

                foreach (string combo in sortedPairs) {
                    SpoilerLogLines.Add("\t " + combo);
                }
            }
            TunicUtils.TryWriteFile(TunicRandomizer.SpoilerLogPath, SpoilerLogLines);
        }

        private static List<string> GetMysterySeedSettingsForSpoilerLog() {
            if (SaveFile.GetInt(MysterySeedEnabled) == 0) { return new List<string>(); };
            List<string> MysterySettings = new List<string>() {
                "Mystery Seed Settings:",
                $"\t- Hexagon Quest: {SaveFile.GetInt(HexagonQuestEnabled) == 1}",
                SaveFile.GetInt(HexagonQuestEnabled) == 1 ? $"\t- Hexagon Quest Goal: {SaveFile.GetInt(HexagonQuestGoal)}" : "",
                SaveFile.GetInt(HexagonQuestEnabled) == 1 ? $"\t- Extra Hexagons: {SaveFile.GetInt(HexagonQuestExtras)}%" : "",
                $"\t- Sword Progression: {SaveFile.GetInt(SwordProgressionEnabled) == 1}",
                $"\t- Keys Behind Bosses: {SaveFile.GetInt(KeysBehindBosses) == 1}",
                $"\t- Start with Sword: {SaveFile.GetInt(StartWithSword) == 1}",
                $"\t- Shuffled Abilities: {SaveFile.GetInt(AbilityShuffle) == 1}",
                $"\t- Shuffled Ladders: {SaveFile.GetInt(LadderRandoEnabled) == 1}",
                $"\t- Shuffled Breakables: {SaveFile.GetInt(BreakableShuffleEnabled) == 1}",
                $"\t- Grass Randomizer: {SaveFile.GetInt(GrassRandoEnabled) == 1}",
                $"\t- Entrance Randomizer: {SaveFile.GetInt(EntranceRando) == 1}",
                SaveFile.GetInt(EntranceRando) == 1 ? $"\t- Entrance Randomizer (Fewer Shops): {SaveFile.GetInt(ERFixedShop) == 1}" : "",
                SaveFile.GetInt(EntranceRando) == 1 ? $"\t- Entrance Randomizer (Matching Direction): {SaveFile.GetInt(PortalDirectionPairs) == 1}" : "",
                SaveFile.GetInt(EntranceRando) == 1 ? $"\t- Entrance Randomizer (Decoupled): {SaveFile.GetInt(Decoupled) == 1}" : "",
                $"\t- Maskless Logic: {SaveFile.GetInt(MasklessLogic) == 1}",
                $"\t- Lanternless Logic: {SaveFile.GetInt(LanternlessLogic) == 1}",
                $"\t- Laurels Location: {((RandomizerSettings.FixedLaurelsType)SaveFile.GetInt(LaurelsLocation)).ToString()}\n",
            };
            MysterySettings.RemoveAll(x => x == "");
            return MysterySettings;
        }

        public static string GetItemCountsByRegion() {
            string title = $"\"- - - - - \"  rahnduhmIzur prawgrehs\"  - - - - -\"\n";
            string displayText = title;
            int TotalAreaChecks = 0;
            int AreaChecksFound = 0;
            bool IncludeCollected = IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld;
            List<Check> Checks = TunicUtils.GetAllInUseChecks();
            foreach (string Area in Locations.MainAreasToSubAreas.Keys) {
                TotalAreaChecks = 0;
                AreaChecksFound = 0;
                foreach (string SubArea in Locations.MainAreasToSubAreas[Area]) {
                    TotalAreaChecks += TunicUtils.GetCheckCountInScene(SubArea);
                    AreaChecksFound += TunicUtils.GetCompletedChecksCountByScene(Checks, SubArea);
                }
                displayText += $"\"{(AreaChecksFound == TotalAreaChecks ? "<#eaa614>" : "<#ffffff>")}{Area.PadRight(24, '.')}{$"{AreaChecksFound}/{TotalAreaChecks}".PadLeft(9, '.')}\"\n";
                if (Area == "Rooted Ziggurat") {
                    displayText += "---" + title;
                }
            }

            int TotalChecksFound = TunicUtils.GetCompletedChecksCount(Checks);
            int TotalChecks = Checks.Count;

            displayText += $"\"{(TotalChecksFound == TotalChecks ? "<#eaa614>" : "<#ffffff>")}{"Total".PadRight(24, '.')}{$"{TotalChecksFound}/{TotalChecks}".PadLeft(9, '.')}\"";
            if (TotalChecksFound == TotalChecks) {
                displayText += $"---\"<#eaa614>- - - - - - {TotalChecksFound.ToString().PadLeft(4)}/{TotalChecks.ToString().PadRight(4)} - - - - - -\"\n\n    ";
                int i = 0;
                foreach(string s in WaveSpell.CustomInputs.Select(input => $"[arrow_{input.ToString().ToLower()}]")) {
                    displayText += $"  <#eaa614>{s}  ";
                    i++;
                    if (i % 6 == 0 && i != 30) {
                        displayText += "\n    ";
                    }
                }
            }
            return displayText;
        }
    }
}
