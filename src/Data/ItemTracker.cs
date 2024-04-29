using BepInEx.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static TunicRandomizer.GhostHints;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {

    public class ItemTracker {
        private static ManualLogSource Logger = TunicRandomizer.Logger;

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
        };

        public List<ItemData> ItemsCollected = new List<ItemData>();

        public ItemTracker() {
            CurrentScene = new SceneInfo();

            Seed = 0;
           
            foreach (string Key in ImportantItems.Keys.ToList()) {
                ImportantItems[Key] = 0;
            }
            ImportantItems["Trinket Slot"] = 1;
            ImportantItems["Coins Tossed"] = StateVariable.GetStateVariableByName("Trinket Coins Tossed").IntValue;

            ItemsCollected = new List<ItemData>();
        }

        public ItemTracker(int seed) {
            CurrentScene = new SceneInfo();
            Seed = seed;
        }

        public void ResetTracker() {
            Seed = 0;
            foreach (string Key in ImportantItems.Keys.ToList()) {
                ImportantItems[Key] = 0;
            }
            ImportantItems["Trinket Slot"] = 1;
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
                ImportantItems["Pages"]++;

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

            if (Item.Type == ItemTypes.HEXAGONQUEST && SaveFile.GetInt(AbilityShuffle) == 1) {
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

        public static void SaveTrackerFile() {
            if (File.Exists(TunicRandomizer.ItemTrackerPath)) {
                File.Delete(TunicRandomizer.ItemTrackerPath);
            }
            File.WriteAllText(TunicRandomizer.ItemTrackerPath, JsonConvert.SerializeObject(TunicRandomizer.Tracker, Formatting.Indented));
        }

        public static void PopulateSpoilerLog() {
            if (TunicRandomizer.Settings.RaceMode) { return; }

            int seed = SaveFile.GetInt("seed");
            Dictionary<string, List<string>> SpoilerLog = new Dictionary<string, List<string>>();
            foreach (string Key in Locations.SceneNamesForSpoilerLog.Keys) {
                SpoilerLog[Key] = new List<string>();
            }

            if (IsArchipelago()) {
                foreach (string Key in ItemLookup.ItemList.Keys) {
                    ArchipelagoItem Item = ItemLookup.ItemList[Key];

                    string Spoiler = $"\t{((Locations.CheckedLocations[Key] || SaveFile.GetInt($"randomizer picked up {Key}") == 1 || (TunicRandomizer.Settings.CollectReflectsInWorld && SaveFile.GetInt($"randomizer {Key} was collected") == 1)) ? "x" : "-")} {Locations.LocationIdToDescription[Key]}: {Item.ItemName} ({Archipelago.instance.GetPlayerName(Item.Player)})";

                    SpoilerLog[Locations.VanillaLocations[Key].Location.SceneName].Add(Spoiler);
                }
            }
            if (IsSinglePlayer()) {
                foreach(string Key in Locations.RandomizedLocations.Keys) {
                    Check Check = Locations.RandomizedLocations[Key];
                    ItemData Item = ItemLookup.GetItemDataFromCheck(Check);
                    string Spoiler = $"\t{(Locations.CheckedLocations[Key] ? "x" : "-")} {Locations.LocationIdToDescription[Key]}: {Item.Name}";
                    SpoilerLog[Locations.VanillaLocations[Key].Location.SceneName].Add(Spoiler);
                }
            }
            List<string> SpoilerLogLines = new List<string>() {
                "Seed: " + seed,
                "Seed Paste: " + TunicRandomizer.Settings.GetSettingsString(),
                "\nLines that start with 'x' instead of '-' represent items that have been collected\n",
            };
            if (IsArchipelago()) {
                SpoilerLogLines.Add("Major Items");
                foreach (string MajorItem in ItemLookup.MajorItems) {
                if(MajorItem == "Gold Questagon") { continue; }
                    if(Locations.MajorItemLocations.ContainsKey(MajorItem) && Locations.MajorItemLocations[MajorItem].Count > 0) {
                        foreach (ArchipelagoHint apHint in Locations.MajorItemLocations[MajorItem]) {

                            bool HasItem = false;
                            if (Archipelago.instance.integration.session.Locations.AllLocationsChecked.Contains(Archipelago.instance.integration.session.Locations.GetLocationIdFromName(Archipelago.instance.GetPlayerGame((int)apHint.Player), apHint.Location))) { 
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
                foreach (string MajorItem in ItemLookup.LegacyMajorItems) {
                    foreach (Check Check in ItemRandomizer.FindAllRandomizedItemsByName(MajorItem)) {
                        ItemData ItemData = ItemLookup.GetItemDataFromCheck(Check);
                        string Key = $"{Check.Location.LocationId} [{Check.Location.SceneName}]";
                        string Spoiler = $"\t{(Locations.CheckedLocations[Key] ? "x" : "-")} {ItemData.Name}: {Locations.SceneNamesForSpoilerLog[Check.Location.SceneName]} - {Locations.LocationIdToDescription[Key]}";
                        SpoilerLogLines.Add(Spoiler);
                    }
                }
            }

            if (SaveFile.GetInt(HexagonQuestEnabled) == 1 && SaveFile.GetInt(AbilityShuffle) == 1) {
                SpoilerLogLines.Add($"\t{(SaveFile.GetInt(PrayerUnlocked) == 1 ? "x" : "-")} Prayer: {SaveFile.GetInt(HexagonQuestPrayer)} Gold Questagons");
                SpoilerLogLines.Add($"\t{(SaveFile.GetInt(HolyCrossUnlocked) == 1 ? "x" : "-")} Holy Cross: {SaveFile.GetInt(HexagonQuestHolyCross)} Gold Questagons");
                SpoilerLogLines.Add($"\t{(SaveFile.GetInt(IceBoltUnlocked) == 1 ? "x" : "-")} Icebolt: {SaveFile.GetInt(HexagonQuestIcebolt)} Gold Questagons");
            }
            
            foreach (string Key in SpoilerLog.Keys) {
                SpoilerLogLines.Add(Locations.SceneNamesForSpoilerLog[Key]);
                SpoilerLog[Key].Sort();
                foreach (string line in SpoilerLog[Key]) {
                    SpoilerLogLines.Add(line);
                }
            }

            if (SaveFile.GetInt(EntranceRando) == 1)
            {
                List<string> PortalSpoiler = new List<string>();
                SpoilerLogLines.Add("\nEntrance Connections");
                foreach (PortalCombo portalCombo in TunicPortals.RandomizedPortals.Values)
                {
                    PortalSpoiler.Add("\t- " + portalCombo.Portal1.Name + " -- " + portalCombo.Portal2.Name);
                }
                foreach (string combo in PortalSpoiler)
                {
                    SpoilerLogLines.Add(combo);
                }
            }
            if (!File.Exists(TunicRandomizer.SpoilerLogPath)) {
                File.WriteAllLines(TunicRandomizer.SpoilerLogPath, SpoilerLogLines);
            } else {
                File.Delete(TunicRandomizer.SpoilerLogPath);
                File.WriteAllLines(TunicRandomizer.SpoilerLogPath, SpoilerLogLines);
            }
            Logger.LogInfo("Wrote spoiler log to " + TunicRandomizer.SpoilerLogPath);
        }

        private static List<string> GetMysterySeedSettingsForSpoilerLog() {
            if (SaveFile.GetInt("randomizer mystery seed") == 0) { return new List<string>(); };
            List<string> MysterySettings = new List<string>() {
                "Mystery Seed Settings:",
                $"\t- Hexagon Quest: {SaveFile.GetInt(HexagonQuestEnabled) == 1}",
                SaveFile.GetInt(HexagonQuestEnabled) == 1 ? $"\t- Hexagon Quest Goal: {SaveFile.GetInt("randomizer hexagon quest goal")}" : "",
                SaveFile.GetInt(HexagonQuestEnabled) == 1 ? $"\t- Extra Hexagons: {SaveFile.GetInt("randomizer hexagon quest extras")}%" : "",
                $"\t- Sword Progression: {SaveFile.GetInt(SwordProgressionEnabled) == 1}",
                $"\t- Keys Behind Bosses: {SaveFile.GetInt(KeysBehindBosses) == 1}",
                $"\t- Start with Sword: {SaveFile.GetInt("randomizer started with sword") == 1}",
                $"\t- Shuffled Abilities: {SaveFile.GetInt(AbilityShuffle) == 1}",
                $"\t- Entrance Randomizer: {SaveFile.GetInt(EntranceRando) == 1}",
                SaveFile.GetInt(EntranceRando) == 1 ? $"\t- Entrance Randomizer (Fewer Shops): {SaveFile.GetInt("randomizer ER fixed shop") == 1}" : "",
                $"\t- Maskless Logic: {SaveFile.GetInt(MasklessLogic) == 1}",
                $"\t- Lanternless Logic: {SaveFile.GetInt(LanternlessLogic) == 1}",
                $"\t- Laurels Location: {((RandomizerSettings.FixedLaurelsType)SaveFile.GetInt("randomizer laurels location")).ToString()}\n",
            };
            MysterySettings.RemoveAll(x => x == "");
            return MysterySettings;
        }
    }
}
