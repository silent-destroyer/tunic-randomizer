using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TunicRandomizer {

    public enum ItemTypes { 
        MONEY,
        INVENTORY,
        SWORDUPGRADE,
        GOLDENTROPHY,
        TRINKET,
        FAIRY,
        PAGE,
        RELIC,
        SPECIAL,
        FOOLTRAP,
        HEXAGONQUEST,
        LADDER,
    }

    public struct BonusUpgrade {
        public string LevelUp;
        public string CustomPickupMessage;

        public BonusUpgrade(string levelUp, string customPickupMessage) {
            LevelUp = levelUp;
            CustomPickupMessage = customPickupMessage;
        }
    }
    public struct HeroRelic {
        public string Flag;
        public string ItemPresentedOnCollection;
        public string CollectionMessage;
        public string OriginalPickupLocation;
        public string CorrespondingStat;

        public HeroRelic(string flag, string itemPresentedOnCollection, string collectionMessage, string originalPickupLocation, string correspondingStat) {
            Flag = flag;
            ItemPresentedOnCollection = itemPresentedOnCollection;
            CollectionMessage = collectionMessage;
            OriginalPickupLocation = originalPickupLocation;
            CorrespondingStat = correspondingStat;
        }
    }

    public struct Fairy {
        public string Flag;
        public string Translation;

        public Fairy(String flag, string translation) {
            Flag = flag;
            Translation = translation;
        }
    }

    public class ItemData {

        public string Name {
            get;
            set;
        }

        public string Classification {
            get;
            set;
        }

        public string ItemNameForInventory {
            get;
            set;
        }

        public ItemTypes Type {
            get;
            set;
        }

        public int QuantityToGive {
            get;
            set;
        }

        public ItemData() {
        }

        public ItemData(string name, string classification, string itemNameForInventory, ItemTypes type, int quantityToGive) {
            Name = name;
            Classification = classification;
            ItemNameForInventory = itemNameForInventory;
            Type = type;
            QuantityToGive = quantityToGive;
        }

        public ItemData(string name, string classification, string itemNameForInventory, int quantityToGive) {
            Name = name;
            Classification = classification;
            ItemNameForInventory = itemNameForInventory;
            Type = ItemTypes.INVENTORY;
            QuantityToGive = quantityToGive;
        }

    }

    public class ItemLookup {

        public static Dictionary<string, ItemInfo> ItemList = new Dictionary<string, ItemInfo>() { };

        public static Dictionary<string, ItemData> Items = new Dictionary<string, ItemData>() {
            // Consumables
            { "Firecracker x2", new ItemData("Firecracker x2", "useful", "Firecracker", ItemTypes.INVENTORY, 2) },
            { "Firecracker x3", new ItemData("Firecracker x3", "useful", "Firecracker", ItemTypes.INVENTORY, 3) },
            { "Firecracker x4", new ItemData("Firecracker x4", "useful", "Firecracker", ItemTypes.INVENTORY, 4) },
            { "Firecracker x5", new ItemData("Firecracker x5", "useful", "Firecracker", ItemTypes.INVENTORY, 5) },
            { "Firecracker x6", new ItemData("Firecracker x6", "useful", "Firecracker", ItemTypes.INVENTORY, 6) },
            { "Fire Bomb x2", new ItemData("Fire Bomb x2", "useful", "Firebomb", ItemTypes.INVENTORY, 2) },
            { "Fire Bomb x3", new ItemData("Fire Bomb x3", "useful", "Firebomb", ItemTypes.INVENTORY, 3) },
            { "Ice Bomb x2", new ItemData("Ice Bomb x2", "useful", "Ice Bomb", ItemTypes.INVENTORY, 2) },
            { "Ice Bomb x3", new ItemData("Ice Bomb x3", "useful", "Ice Bomb", ItemTypes.INVENTORY, 3) },
            { "Ice Bomb x5", new ItemData("Ice Bomb x5", "useful", "Ice Bomb", ItemTypes.INVENTORY, 5) },
            { "Lure", new ItemData("Lure", "useful", "Bait", ItemTypes.INVENTORY, 1) },
            { "Lure x2", new ItemData("Lure x2", "useful", "Bait", ItemTypes.INVENTORY, 2) },
            { "Pepper x2", new ItemData("Pepper x2", "useful", "Pepper", ItemTypes.INVENTORY, 2) },
            { "Ivy x3", new ItemData("Ivy x3", "useful", "Ivy", ItemTypes.INVENTORY, 3) },
            { "Effigy", new ItemData("Effigy", "useful", "Piggybank L1", ItemTypes.INVENTORY, 1) },
            { "HP Berry", new ItemData("HP Berry", "useful", "Berry_HP", ItemTypes.INVENTORY, 1) },
            { "HP Berry x2", new ItemData("HP Berry x2", "useful", "Berry_HP", ItemTypes.INVENTORY, 2) },
            { "HP Berry x3", new ItemData("HP Berry x3", "useful", "Berry_HP", ItemTypes.INVENTORY, 3) },
            { "MP Berry", new ItemData("MP Berry", "useful", "Berry_MP", ItemTypes.INVENTORY, 1) },
            { "MP Berry x2", new ItemData("MP Berry x2", "useful", "Berry_MP", ItemTypes.INVENTORY, 2) },
            { "MP Berry x3", new ItemData("MP Berry x3", "useful", "Berry_MP", ItemTypes.INVENTORY, 3) },

            // Fairy
            { "Fairy", new ItemData("Fairy", "progression", "Fairy", ItemTypes.FAIRY, 1) },

            // Regular Items
            { "Stick", new ItemData("Stick", "progression", "Stick", ItemTypes.INVENTORY, 1) },
            { "Sword", new ItemData("Sword", "progression", "Sword", ItemTypes.INVENTORY, 1) },
            { "Sword Upgrade", new ItemData("Sword Upgrade", "progression", "Sword", ItemTypes.SWORDUPGRADE, 1) },
            { "Magic Wand", new ItemData("Magic Wand", "progression", "Techbow", ItemTypes.INVENTORY, 1) },
            { "Magic Dagger", new ItemData("Magic Dagger", "progression", "Stundagger", ItemTypes.INVENTORY, 1) },
            { "Magic Orb", new ItemData("Magic Orb", "progression", "Wand", ItemTypes.INVENTORY, 1) },
            { "Hero's Laurels", new ItemData("Hero's Laurels", "progression", "Hyperdash", ItemTypes.INVENTORY, 1) },
            { "Lantern", new ItemData("Lantern", "progression", "Lantern", ItemTypes.INVENTORY, 1) },
            { "Gun", new ItemData("Gun", "useful", "Shotgun", ItemTypes.INVENTORY, 1) },
            { "Shield", new ItemData("Shield", "useful", "Shield", ItemTypes.INVENTORY, 1) },
            { "Dath Stone", new ItemData("Dath Stone", "useful", "Dath Stone", ItemTypes.INVENTORY, 1) },
            { "Hourglass", new ItemData("Hourglass", "useful", "SlowmoItem", ItemTypes.INVENTORY, 1) },
            { "Old House Key", new ItemData("Old House Key", "progression", "Key (House)", ItemTypes.INVENTORY, 1) },
            { "Key", new ItemData("Key", "progression", "Key", ItemTypes.INVENTORY, 1) },
            { "Fortress Vault Key", new ItemData("Fortress Vault Key", "progression", "Vault Key (Red)", ItemTypes.INVENTORY, 1) },
            { "Flask Shard", new ItemData("Flask Shard", "useful", "Flask Shard", ItemTypes.INVENTORY, 1) },
            { "Potion Flask", new ItemData("Potion Flask", "useful", "Flask Container", ItemTypes.INVENTORY, 1) },
            { "Golden Coin", new ItemData("Golden Coin", "progression", "Trinket Coin", ItemTypes.INVENTORY, 1) },
            { "Card Slot", new ItemData("Card Slot", "useful", "Trinket Slot", ItemTypes.INVENTORY, 1) },
            { "Red Questagon", new ItemData("Red Questagon", "useful", "Hexagon Red", ItemTypes.INVENTORY, 1) },
            { "Green Questagon", new ItemData("Green Questagon", "useful", "Hexagon Green", ItemTypes.INVENTORY, 1) },
            { "Blue Questagon", new ItemData("Blue Questagon", "useful", "Hexagon Blue", ItemTypes.INVENTORY, 1) },
            { "Gold Questagon", new ItemData("Gold Questagon", "useful", "Hexagon Gold", ItemTypes.HEXAGONQUEST, 1) },

            // Upgrades and Relics
            { "ATT Offering", new ItemData("ATT Offering", "useful", "Upgrade Offering - Attack - Tooth", ItemTypes.INVENTORY, 1) },
            { "DEF Offering", new ItemData("DEF Offering", "useful", "Upgrade Offering - DamageResist - Effigy", ItemTypes.INVENTORY, 1) },
            { "Potion Offering", new ItemData("Potion Offering", "useful", "Upgrade Offering - PotionEfficiency Swig - Ash", ItemTypes.INVENTORY, 1) },
            { "HP Offering", new ItemData("HP Offering", "useful", "Upgrade Offering - Health HP - Flower", ItemTypes.INVENTORY, 1) },
            { "MP Offering", new ItemData("MP Offering", "useful", "Upgrade Offering - Magic MP - Mushroom", ItemTypes.INVENTORY, 1) },
            { "SP Offering", new ItemData("SP Offering", "useful", "Upgrade Offering - Stamina SP - Feather", ItemTypes.INVENTORY, 1) },
            { "Hero Relic - ATT", new ItemData("Hero Relic - ATT", "useful", "Relic - Hero Sword", ItemTypes.RELIC, 1) },
            { "Hero Relic - DEF", new ItemData("Hero Relic - DEF", "useful", "Relic - Hero Crown", ItemTypes.RELIC, 1) },
            { "Hero Relic - POTION", new ItemData("Hero Relic - POTION", "useful", "Relic - Hero Water", ItemTypes.RELIC, 1) },
            { "Hero Relic - HP", new ItemData("Hero Relic - HP", "useful", "Relic - Hero Pendant HP", ItemTypes.RELIC, 1) },
            { "Hero Relic - MP", new ItemData("Hero Relic - MP", "useful", "Relic - Hero Pendant MP", ItemTypes.RELIC, 1) },
            { "Hero Relic - SP", new ItemData("Hero Relic - SP", "useful", "Relic - Hero Pendant SP", ItemTypes.RELIC, 1) },

            // Trinket Cards
            { "Orange Peril Ring", new ItemData("Orange Peril Ring", "useful", "Trinket - RTSR", ItemTypes.TRINKET, 1) },
            { "Tincture", new ItemData("Tincture", "useful", "Trinket - Attack Up Defense Down", ItemTypes.TRINKET, 1) },
            { "Scavenger Mask", new ItemData("Scavenger Mask", "progression", "Mask", ItemTypes.TRINKET, 1) },
            { "Cyan Peril Ring", new ItemData("Cyan Peril Ring", "useful", "Trinket - BTSR", ItemTypes.TRINKET, 1) },
            { "Bracer", new ItemData("Bracer", "useful", "Trinket - Block Plus", ItemTypes.TRINKET, 1) },
            { "Dagger Strap", new ItemData("Dagger Strap", "useful", "Trinket - Fast Icedagger", ItemTypes.TRINKET, 1) },
            { "Inverted Ash", new ItemData("Inverted Ash", "useful", "Trinket - MP Flasks", ItemTypes.TRINKET, 1) },
            { "Lucky Cup", new ItemData("Lucky Cup", "useful", "Trinket - Heartdrops", ItemTypes.TRINKET, 1) },
            { "Magic Echo", new ItemData("Magic Echo", "useful", "Trinket - Bloodstain MP", ItemTypes.TRINKET, 1) },
            { "Anklet", new ItemData("Anklet", "useful", "Trinket - Walk Speed Plus", ItemTypes.TRINKET, 1) },
            { "Muffling Bell", new ItemData("Muffling Bell", "useful", "Trinket - Sneaky", ItemTypes.TRINKET, 1) },
            { "Glass Cannon", new ItemData("Glass Cannon", "useful", "Trinket - Glass Cannon", ItemTypes.TRINKET, 1) },
            { "Perfume", new ItemData("Perfume", "useful", "Trinket - Stamina Recharge Plus", ItemTypes.TRINKET, 1) },
            { "Louder Echo", new ItemData("Louder Echo", "useful", "Trinket - Bloodstain Plus", ItemTypes.TRINKET, 1) },
            { "Aura's Gem", new ItemData("Aura's Gem", "useful", "Trinket - Parry Window", ItemTypes.TRINKET, 1) },
            { "Bone Card", new ItemData("Bone Card", "useful", "Trinket - IFrames", ItemTypes.TRINKET, 1) },

            // Golden Trophies
            { "Mr Mayor", new ItemData("Mr Mayor", "useful", "GoldenTrophy_1", ItemTypes.GOLDENTROPHY, 1) },
            { "Secret Legend", new ItemData("Secret Legend", "useful", "GoldenTrophy_2", ItemTypes.GOLDENTROPHY, 1) },
            { "Sacred Geometry", new ItemData("Sacred Geometry", "useful", "GoldenTrophy_3", ItemTypes.GOLDENTROPHY, 1) },
            { "Vintage", new ItemData("Vintage", "useful", "GoldenTrophy_4", ItemTypes.GOLDENTROPHY, 1) },
            { "Just Some Pals", new ItemData("Just Some Pals", "useful", "GoldenTrophy_5", ItemTypes.GOLDENTROPHY, 1) },
            { "Regal Weasel", new ItemData("Regal Weasel", "useful", "GoldenTrophy_6", ItemTypes.GOLDENTROPHY, 1) },
            { "Spring Falls", new ItemData("Spring Falls", "useful", "GoldenTrophy_7", ItemTypes.GOLDENTROPHY, 1) },
            { "Power Up", new ItemData("Power Up", "useful", "GoldenTrophy_8", ItemTypes.GOLDENTROPHY, 1) },
            { "Back To Work", new ItemData("Back To Work", "useful", "GoldenTrophy_9", ItemTypes.GOLDENTROPHY, 1) },
            { "Phonomath", new ItemData("Phonomath", "useful", "GoldenTrophy_10", ItemTypes.GOLDENTROPHY, 1) },
            { "Dusty", new ItemData("Dusty", "useful", "GoldenTrophy_11", ItemTypes.GOLDENTROPHY, 1) },
            { "Forever Friend", new ItemData("Forever Friend", "useful", "GoldenTrophy_12", ItemTypes.GOLDENTROPHY, 1) },

            // Fool Trap
            { "Fool Trap", new ItemData("Fool Trap", "trap", "Fool Trap", ItemTypes.FOOLTRAP, 1) },

            // Money
            { "Money x1", new ItemData("Money x1", "useful", "money", ItemTypes.MONEY, 1) },
            { "Money x10", new ItemData("Money x10", "useful", "money", ItemTypes.MONEY, 10) },
            { "Money x15", new ItemData("Money x15", "useful", "money", ItemTypes.MONEY, 15) },
            { "Money x16", new ItemData("Money x16", "useful", "money", ItemTypes.MONEY, 16) },
            { "Money x20", new ItemData("Money x20", "useful", "money", ItemTypes.MONEY, 20) },
            { "Money x25", new ItemData("Money x25", "useful", "money", ItemTypes.MONEY, 25) },
            { "Money x30", new ItemData("Money x30", "useful", "money", ItemTypes.MONEY, 30) },
            { "Money x32", new ItemData("Money x32", "useful", "money", ItemTypes.MONEY, 32) },
            { "Money x40", new ItemData("Money x40", "useful", "money", ItemTypes.MONEY, 40) },
            { "Money x48", new ItemData("Money x48", "useful", "money", ItemTypes.MONEY, 48) },
            { "Money x50", new ItemData("Money x50", "useful", "money", ItemTypes.MONEY, 50) },
            { "Money x64", new ItemData("Money x64", "useful", "money", ItemTypes.MONEY, 64) },
            { "Money x100", new ItemData("Money x100", "useful", "money", ItemTypes.MONEY, 100) },
            { "Money x128", new ItemData("Money x128", "useful", "money", ItemTypes.MONEY, 128) },
            { "Money x200", new ItemData("Money x200", "useful", "money", ItemTypes.MONEY, 200) },
            { "Money x255", new ItemData("Money x255", "useful", "money", ItemTypes.MONEY, 255) },

            // Pages
            { "Pages 0-1", new ItemData("Pages 0-1", "useful", "0", ItemTypes.PAGE, 1) },
            { "Pages 2-3", new ItemData("Pages 2-3", "useful", "1", ItemTypes.PAGE, 1) },
            { "Pages 4-5", new ItemData("Pages 4-5", "useful", "2", ItemTypes.PAGE, 1) },
            { "Pages 6-7", new ItemData("Pages 6-7", "useful", "3", ItemTypes.PAGE, 1) },
            { "Pages 8-9", new ItemData("Pages 8-9", "useful", "4", ItemTypes.PAGE, 1) },
            { "Pages 10-11", new ItemData("Pages 10-11", "useful", "5", ItemTypes.PAGE, 1) },
            { "Pages 12-13", new ItemData("Pages 12-13", "useful", "6", ItemTypes.PAGE, 1) },
            { "Pages 14-15", new ItemData("Pages 14-15", "useful", "7", ItemTypes.PAGE, 1) },
            { "Pages 16-17", new ItemData("Pages 16-17", "useful", "8", ItemTypes.PAGE, 1) },
            { "Pages 18-19", new ItemData("Pages 18-19", "useful", "9", ItemTypes.PAGE, 1) },
            { "Pages 20-21", new ItemData("Pages 20-21", "useful", "10", ItemTypes.PAGE, 1) },
            { "Pages 22-23", new ItemData("Pages 22-23", "useful", "11", ItemTypes.PAGE, 1) },
            { "Pages 24-25 (Prayer)", new ItemData("Pages 24-25 (Prayer)", "progression", "12", ItemTypes.PAGE, 1) },
            { "Pages 26-27", new ItemData("Pages 26-27", "useful", "13", ItemTypes.PAGE, 1) },
            { "Pages 28-29", new ItemData("Pages 28-29", "useful", "14", ItemTypes.PAGE, 1) },
            { "Pages 30-31", new ItemData("Pages 30-31", "useful", "15", ItemTypes.PAGE, 1) },
            { "Pages 32-33", new ItemData("Pages 32-33", "useful", "16", ItemTypes.PAGE, 1) },
            { "Pages 34-35", new ItemData("Pages 34-35", "useful", "17", ItemTypes.PAGE, 1) },
            { "Pages 36-37", new ItemData("Pages 36-37", "useful", "18", ItemTypes.PAGE, 1) },
            { "Pages 38-39", new ItemData("Pages 38-39", "useful", "19", ItemTypes.PAGE, 1) },
            { "Pages 40-41", new ItemData("Pages 40-41", "useful", "20", ItemTypes.PAGE, 1) },
            { "Pages 42-43 (Holy Cross)", new ItemData("Pages 42-43 (Holy Cross)", "progression", "21", ItemTypes.PAGE, 1) },
            { "Pages 44-45", new ItemData("Pages 44-45", "useful", "22", ItemTypes.PAGE, 1) },
            { "Pages 46-47", new ItemData("Pages 46-47", "useful", "23", ItemTypes.PAGE, 1) },
            { "Pages 48-49", new ItemData("Pages 48-49", "useful", "24", ItemTypes.PAGE, 1) },
            { "Pages 50-51", new ItemData("Pages 50-51", "useful", "25", ItemTypes.PAGE, 1) },
            { "Pages 52-53 (Icebolt)", new ItemData("Pages 52-53 (Icebolt)", "progression", "26", ItemTypes.PAGE, 1) },
            { "Pages 54-55", new ItemData("Pages 54-55", "useful", "27", ItemTypes.PAGE, 1) },

            // Ladders
            { "Ladders in Overworld Town", new ItemData("Ladders in Overworld Town", "progression", "Ladders in Overworld Town", ItemTypes.LADDER, 1) }, // DONE
            { "Ladders near Weathervane", new ItemData("Ladders near Weathervane", "progression", "Ladders near Weathervane", ItemTypes.LADDER, 1) }, // DONE
            { "Ladders near Overworld Checkpoint", new ItemData("Ladders near Overworld Checkpoint", "progression", "Ladders near Overworld Checkpoint", ItemTypes.LADDER, 1) }, // DONE
            { "Ladder to East Forest", new ItemData("Ladder to East Forest", "progression", "Ladder to East Forest", ItemTypes.LADDER, 1) }, // DONE
            { "Ladders to Lower Forest", new ItemData("Ladders to Lower Forest", "progression", "Ladders to Lower Forest", ItemTypes.LADDER, 1) }, // DONE
            { "Ladders near Patrol Cave", new ItemData("Ladders near Patrol Cave", "progression", "Ladders near Patrol Cave", ItemTypes.LADDER, 1) }, // DONE
            { "Ladders in Well", new ItemData("Ladders in Well", "progression", "Ladders in Well", ItemTypes.LADDER, 1) }, // DONE
            { "Ladders to West Bell", new ItemData("Ladders to West Bell", "progression", "Ladders to West Bell", ItemTypes.LADDER, 1) }, // DONE
            { "Ladder to Quarry", new ItemData("Ladder to Quarry", "progression", "Ladder to Quarry", ItemTypes.LADDER, 1) }, // DONE
            { "Ladder in Dark Tomb", new ItemData("Ladder in Dark Tomb", "progression", "Ladder in Dark Tomb", ItemTypes.LADDER, 1) }, // DONE
            { "Ladders near Dark Tomb", new ItemData("Ladders near Dark Tomb", "progression", "Ladders near Dark Tomb", ItemTypes.LADDER, 1) }, // DONE
            { "Ladder near Temple Rafters", new ItemData("Ladder near Temple Rafters", "progression", "Ladder near Temple Rafters", ItemTypes.LADDER, 1) }, // DONE
            { "Ladder to Swamp", new ItemData("Ladder to Swamp", "progression", "Ladder to Swamp", ItemTypes.LADDER, 1) }, // DONE
            { "Ladders in Swamp", new ItemData("Ladders in Swamp", "progression", "Ladders in Swamp", ItemTypes.LADDER, 1) }, // DONE
            { "Ladder to Ruined Atoll", new ItemData("Ladder to Ruined Atoll", "progression", "Ladder to Ruined Atoll", ItemTypes.LADDER, 1) }, // DONE
            { "Ladders in South Atoll", new ItemData("Ladders in South Atoll", "progression", "Ladders in South Atoll", ItemTypes.LADDER, 1) }, // DONE
            { "Ladders to Frog's Domain", new ItemData("Ladders to Frog's Domain", "progression", "Ladders to Frog's Domain", ItemTypes.LADDER, 1) }, // DONE
            { "Ladders in Hourglass Cave", new ItemData("Ladders in Hourglass Cave", "progression", "Ladders in Hourglass Cave", ItemTypes.LADDER, 1) }, // DONE
            { "Ladder to Beneath the Vault", new ItemData("Ladder to Beneath the Vault", "progression", "Ladder to Beneath the Vault", ItemTypes.LADDER, 1) }, // DONE
            { "Ladders in Lower Quarry", new ItemData("Ladders in Lower Quarry", "progression", "Ladders in Lower Quarry", ItemTypes.LADDER, 1) }, // DONE
            { "Ladders in Library", new ItemData("Ladders in Library", "progression", "Ladders in Library", ItemTypes.LADDER, 1) }, // DONE
        };

        public static ItemData GetItemDataFromCheck(Check Check) {
            if (ItemLookup.FairyLookup.ContainsKey(Check.Reward.Name)) {
                return Items["Fairy"];
            } else if (Check.Reward.Name == "Sword Progression") {
                return Items["Sword Upgrade"];
            } else if (Check.Reward.Name == "Fool Trap") {
                return Items["Fool Trap"];
            } else {
                string itemName = ItemLookup.Items.Values.Where(itemdata => itemdata.ItemNameForInventory == Check.Reward.Name && itemdata.QuantityToGive == Check.Reward.Amount).FirstOrDefault().Name;
                return Items.ContainsKey(itemName) ? Items[itemName] : Items["Money x1"];
            }
        }

        public static List<string> LevelUpItems = new List<string>() { "Level Up - Attack", "Level Up - DamageResist", "Level Up - PotionEfficiency", "Level Up - Health", "Level Up - Stamina", "Level Up - Magic" };

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

            { "Relic - Hero Sword", new BonusUpgrade("Level Up - Attack", $"\"Hero Relic - <#e99d4c>ATT\"") },
            { "Relic - Hero Crown", new BonusUpgrade("Level Up - DamageResist", $"\"Hero Relic - <#5de7cf>DEF\"") },
            { "Relic - Hero Water", new BonusUpgrade("Level Up - PotionEfficiency", $"\"Hero Relic - <#ca7be4>POTION\"") },
            { "Relic - Hero Pendant HP", new BonusUpgrade("Level Up - Health", $"\"Hero Relic - <#f03c67>HP\"") },
            { "Relic - Hero Pendant SP", new BonusUpgrade("Level Up - Stamina", $"\"Hero Relic - <#8ddc6e>SP\"") },
            { "Relic - Hero Pendant MP", new BonusUpgrade("Level Up - Magic", $"\"Hero Relic - <#2a8fed>MP\"") },
        };

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

        public static List<string> MajorItems = new List<string>() { "Stick", "Sword", "Sword Upgrade", "Magic Dagger", "Magic Wand", "Magic Orb", "Hero's Laurels", "Lantern", "Shield", "Gun", "Scavenger Mask",
                "Old House Key", "Fortress Vault Key", "Dath Stone", "Hourglass", "Hero Relic - ATT", "Hero Relic - DEF", "Hero Relic - POTION", "Hero Relic - HP", "Hero Relic - SP",
                "Hero Relic - MP", "Red Questagon", "Green Questagon", "Blue Questagon", "Gold Questagon", "Pages 24-25 (Prayer)", "Pages 42-43 (Holy Cross)", "Pages 52-53 (Icebolt)"
        };

        public static List<string> LegacyMajorItems = new List<string>() { "Sword", "Sword Progression", "Stundagger", "Techbow", "Wand", "Hyperdash", "Lantern", "Shield", "Shotgun", "Mask",
                "Key (House)", "Vault Key (Red)", "Dath Stone", "Relic - Hero Sword", "Relic - Hero Crown", "Relic - Hero Water", "Relic - Hero Pendant HP", "Relic - Hero Pendant SP",
                "Relic - Hero Pendant MP", "Hexagon Red", "Hexagon Green", "Hexagon Blue", "12", "21", "26"
        };

        public static string PrayerUnlockedLine = $"\"PRAYER Unlocked.\" Jahnuhl yor wizduhm, rooin sEkur.";
        public static string HolyCrossUnlockedLine = $"\"HOLY CROSS Unlocked.\" sEk wuht iz rItfuhlE yorz.";
        public static string IceboltUnlockedLine = $"\"ICEBOLT Unlocked.\" #A wOnt nO wuht hit #ehm";

        public static Dictionary<string, string> SimplifiedItemNames = new Dictionary<string, string>() {
            {"Firecracker", "Firecracker"},
            {"Firebomb", "Fire Bomb"},
            {"Ice Bomb", "Ice Bomb"},
            {"Bait", "Lure"},
            {"Pepper", "Pepper"},
            {"Ivy", "Ivy"},
            {"Piggybank L1", "Effigy"},
            {"money", "Money"},
            {"Berry_HP", "HP Berry"},
            {"Berry_MP", "MP Berry"},
            {"Fool Trap", "Fool Trap"},
            {"Stick", "Stick"},
            {"Sword", "Sword"},
            {"Sword Progression", "Sword Upgrade"},
            {"Sword Upgrade", "Sword Upgrade"},
            {"Stundagger", "Magic Dagger"},
            {"Techbow", "Magic Wand"},
            {"Wand", "Magic Orb"},
            {"Hyperdash", "Hero's Laurels"},
            {"Lantern", "Lantern"},
            {"Shotgun", "Gun"},
            {"Shield", "Shield"},
            {"Dath Stone", "Dath Stone"},
            {"SlowmoItem", "Hourglass"},
            {"Key (House)", "Old House Key"},
            {"Key", "Key"},
            {"Vault Key (Red)", "Fortress Vault Key"},
            {"Flask Shard", "Flask Shard"},
            {"Flask Container", "Potion Flask"},
            {"Trinket Coin", "Golden Coin"},
            {"Trinket Slot", "Card Slot"},
            {"Hexagon Red", "Red Questagon"},
            {"Hexagon Green", "Green Questagon"},
            {"Hexagon Blue", "Blue Questagon"},
            {"Hexagon Gold", "Gold Questagon"},
            {"Upgrade Offering - Attack - Tooth", "ATT Offering"},
            {"Upgrade Offering - DamageResist - Effigy", "DEF Offering"},
            {"Upgrade Offering - PotionEfficiency Swig - Ash", "Potion Offering"},
            {"Upgrade Offering - Health HP - Flower", "HP Offering"},
            {"Upgrade Offering - Stamina SP - Feather", "SP Offering"},
            {"Upgrade Offering - Magic MP - Mushroom", "MP Offering"},
            {"Relic - Hero Sword", "Hero Relic - ATT"},
            {"Relic - Hero Crown", "Hero Relic - DEF"},
            {"Relic - Hero Water", "Hero Relic - POTION"},
            {"Relic - Hero Pendant HP", "Hero Relic - HP"},
            {"Relic - Hero Pendant SP", "Hero Relic - SP"},
            {"Relic - Hero Pendant MP", "Hero Relic - MP"},
            {"Trinket - RTSR", "Orange Peril Ring"},
            {"Trinket - Attack Up Defense Down", "Tincture"},
            {"Mask", "Scavenger Mask"},
            {"Trinket - BTSR", "Cyan Peril Ring"},
            {"Trinket - Block Plus", "Bracer"},
            {"Trinket - Fast Icedagger", "Dagger Strap"},
            {"Trinket - MP Flasks", "Inverted Ash"},
            {"Trinket - Heartdrops", "Lucky Cup"},
            {"Trinket - Bloodstain MP", "Magic Echo"},
            {"Trinket - Walk Speed Plus", "Anklet"},
            {"Trinket - Sneaky", "Muffling Bell"},
            {"Trinket - Glass Cannon", "Glass Cannon"},
            {"Trinket - Stamina Recharge Plus", "Perfume"},
            {"Trinket - Bloodstain Plus", "Louder Echo"},
            {"Trinket - Parry Window", "Aura's Gem"},
            {"Trinket - IFrames", "Bone Card"},
            {"Overworld Redux-(64.5, 44.0, -40.0)", "Fairy"},
            {"Overworld Redux-(-52.0, 2.0, -174.8)", "Fairy"},
            {"Overworld Redux-(-132.0, 28.0, -55.5)", "Fairy"},
            {"Overworld Cave-(-90.4, 515.0, -738.9)", "Fairy"},
            {"Waterfall-(-47.0, 45.0, 10.0)", "Fairy"},
            {"Temple-(14.0, 0.1, 42.4)", "Fairy"},
            {"Quarry Redux-(0.7, 68.0, 84.7)", "Fairy"},
            {"East Forest Redux-(104.0, 16.0, 61.0)", "Fairy"},
            {"Library Hall-(133.3, 10.0, -43.2)", "Fairy"},
            {"Town Basement-(-202.0, 28.0, 150.0)", "Fairy"},
            {"Overworld Redux-(90.4, 36.0, -122.1)", "Fairy"},
            {"Overworld Interiors-(-28.0, 27.0, -50.5)", "Fairy"},
            {"PatrolCave-(74.0, 46.0, 24.0)", "Fairy"},
            {"CubeRoom-(321.1, 3.0, 217.0)", "Fairy"},
            {"Maze Room-(1.0, 0.0, -1.0)", "Fairy"},
            {"Overworld Redux-(-83.0, 20.0, -117.5)", "Fairy"},
            {"Archipelagos Redux-(-396.3, 1.4, 42.3)", "Fairy"},
            {"Archipelagos Redux-(-236.0, 8.0, 86.3)", "Fairy"},
            {"Fortress Main-(-75.0, -1.0, 17.0)", "Fairy"},
            {"East Forest Redux-(164.0, -25.0, -56.0)", "Fairy"},
            {"Fairy", "Fairy"},
            {"GoldenTrophy_1", "Mr Mayor"},
            {"GoldenTrophy_2", "Secret Legend"},
            {"GoldenTrophy_3", "Sacred Geometry"},
            {"GoldenTrophy_4", "Vintage"},
            {"GoldenTrophy_5", "Just Some Pals"},
            {"GoldenTrophy_6", "Regal Weasel"},
            {"GoldenTrophy_7", "Spring Falls"},
            {"GoldenTrophy_8", "Power Up"},
            {"GoldenTrophy_9", "Back To Work"},
            {"GoldenTrophy_10", "Phonomath"},
            {"GoldenTrophy_11", "Dusty"},
            {"GoldenTrophy_12", "Forever Friend"},
            {"0", "Pages 0-1"},
            {"1", "Pages 2-3"},
            {"2", "Pages 4-5"},
            {"3", "Pages 6-7"},
            {"4", "Pages 8-9"},
            {"5", "Pages 10-11"},
            {"6", "Pages 12-13"},
            {"7", "Pages 14-15"},
            {"8", "Pages 16-17"},
            {"9", "Pages 18-19"},
            {"10", "Pages 20-21"},
            {"11", "Pages 22-23"},
            {"12", "Pages 24-25 (Prayer)"},
            {"13", "Pages 26-27"},
            {"14", "Pages 28-29"},
            {"15", "Pages 30-31"},
            {"16", "Pages 32-33"},
            {"17", "Pages 34-35"},
            {"18", "Pages 36-37"},
            {"19", "Pages 38-39"},
            {"20", "Pages 40-41"},
            {"21", "Pages 42-43 (Holy Cross)"},
            {"22", "Pages 44-45"},
            {"23", "Pages 46-47"},
            {"24", "Pages 48-49"},
            {"25", "Pages 50-51"},
            {"26", "Pages 52-53 (Icebolt)"},
            {"27", "Pages 54-55"},
            { "Ladders in Overworld Town", "Ladders in Overworld Town" },
            { "Ladders near Weathervane", "Ladders near Weathervane" },
            { "Ladders near Overworld Checkpoint", "Ladders near Overworld Checkpoint" },
            { "Ladder to East Forest", "Ladder to East Forest" },
            { "Ladders to Lower Forest", "Ladders to Lower Forest" },
            { "Ladders near Patrol Cave", "Ladders near Patrol Cave" },
            { "Ladders in Well", "Ladders in Well" },
            { "Ladders to West Bell", "Ladders to West Bell" },
            { "Ladder to Quarry", "Ladder to Quarry" },
            { "Ladder in Dark Tomb", "Ladder in Dark Tomb" },
            { "Ladders near Dark Tomb", "Ladders near Dark Tomb" },
            { "Ladder near Temple Rafters", "Ladder near Temple Rafters" },
            { "Ladder to Swamp", "Ladder to Swamp" },
            { "Ladders in Swamp", "Ladders in Swamp" },
            { "Ladder to Ruined Atoll", "Ladder to Ruined Atoll" },
            { "Ladders in South Atoll", "Ladders in South Atoll" },
            { "Ladders to Frog's Domain", "Ladders to Frog's Domain" },
            { "Ladders in Hourglass Cave", "Ladders in Hourglass Cave" },
            { "Ladder to Beneath the Vault", "Ladder to Beneath the Vault" },
            { "Ladders in Lower Quarry", "Ladders in Lower Quarry" },
            { "Ladders in Library", "Ladders in Library" },
        };

        public static Dictionary<string, string> BombCodes = new Dictionary<string, string>() {
            { "drurululdldr", "Granted Firecracker" },
            { "lurdrurdrurdl", "Granted Firebomb" },
            { "ldrurdrurdrul", "Granted Icebomb" },
        };

        public static List<string> ShopkeeperLines = new List<string>() {
            "nO rEfuhndz.",
            "awl sAlz fInuhl.",
            "tipi^ ehnkurijd.",
            "#Ar mahsturwurks awl.",
            "nO diskownts.",
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
    }

}
