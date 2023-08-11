using System.Collections.Generic;
using System.IO;
using TinyJson;

namespace TunicRandomizer {

    public class ItemTracker {
        public struct SceneInfo {
            public int SceneId;
            public string SceneName;
            public ColorPalette Fur;
            public ColorPalette Puff;
            public ColorPalette Details;
            public ColorPalette Tunic;
            public ColorPalette Scarf;
        }

        public int Seed;

        public SceneInfo CurrentScene;

        public Dictionary<string, int> ImportantItems = new Dictionary<string, int>() {
            {"Stick", 0},
            {"Sword", 0},
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
            {"Flask Container", 0},
            {"Pages", 0},
            {"Prayer Page", 0},
            {"Holy Cross Page", 0},
            {"Ice Rod Page", 0},
            {"Fairies", 0},
            {"Golden Trophies", 0},
            {"Dath Stone", 0},
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
            {"Sword Progression", 0},
        };
        
        public List<ItemData> ItemsCollected = new List<ItemData>();

        public ItemTracker() {
            CurrentScene = new SceneInfo();
            Seed = 0;
        }

        public ItemTracker(int seed) {
            CurrentScene = new SceneInfo();
            Seed = seed;
        }

        public static void SaveTrackerFile() {
            if (File.Exists(TunicRandomizer.ItemTrackerPath)) {
                File.Delete(TunicRandomizer.ItemTrackerPath);
            }
            File.WriteAllText(TunicRandomizer.ItemTrackerPath, JSONWriter.ToJson(TunicRandomizer.Tracker));
        }
    }
}
