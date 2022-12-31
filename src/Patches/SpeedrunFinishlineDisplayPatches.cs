using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
            {"Inventory items_offering_orb", "Upgrade Offering - Magic MP - Mushroom"}
        };

        public static bool SpeedrunFinishlineDisplay_showFinishline_PrefixPatch(SpeedrunFinishlineDisplay __instance) {
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
            if (TunicRandomizer.Tracker.ImportantItems[ReportGroupItems[icon.name]] == 0) {
                return false;
            }

            quantity = TunicRandomizer.Tracker.ImportantItems[ReportGroupItems[icon.name]];
            return true;
        }

        public static bool GameOverDecision___retry_PrefixPatch(GameOverDecision __instance) {
            for (int i = 0; i < 28; i++) {
                SaveFile.SetInt("unlocked page " + i, SaveFile.GetInt("randomizer picked up page " + i) == 1 ? 1 : 0);
            }
            return true;
        }
    }
}
