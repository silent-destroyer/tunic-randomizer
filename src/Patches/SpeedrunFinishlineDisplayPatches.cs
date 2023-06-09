using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            {"Inventory items_money triangle", "Golden Item"}
        };
        
        public static bool SpeedrunFinishlineDisplay_showFinishline_PrefixPatch(SpeedrunFinishlineDisplay __instance) {

            SpeedrunReportItem DathStone = ScriptableObject.CreateInstance<SpeedrunReportItem>();
            DathStone.chestIDs = new int[] { };
            DathStone.tallyStateVars = new StateVariable[] { };
            DathStone.itemsForQuantity = new Item[] { Inventory.GetItemByName("Homeward Bone Statue") };
            DathStone.icon = Inventory.GetItemByName("Homeward Bone Statue").icon;
            SpeedrunReportItem GoldenItem = ScriptableObject.CreateInstance<SpeedrunReportItem>();
            GoldenItem.chestIDs = new int[] { };
            GoldenItem.tallyStateVars = new StateVariable[] { };
            GoldenItem.itemsForQuantity = new Item[] { Inventory.GetItemByName("Spear") };
            GoldenItem.icon = Inventory.GetItemByName("Spear").icon;

            SpeedrunFinishlineDisplay.instance.reportGroup_secrets = new SpeedrunReportItem[] {
                SpeedrunFinishlineDisplay.instance.reportGroup_secrets[0],
                SpeedrunFinishlineDisplay.instance.reportGroup_secrets[1],
                SpeedrunFinishlineDisplay.instance.reportGroup_secrets[2],
                SpeedrunFinishlineDisplay.instance.reportGroup_secrets[3],
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

        public static void GameOverDecision_Start_PostfixPatch(GameOverDecision __instance) {
            int MissingPageCount = (28 - TunicRandomizer.Tracker.ImportantItems["Pages"]);
            __instance.retryKey_plural = $"Missing {MissingPageCount} pages. Return to seek another path.";
            __instance.retryKey_single = $"Missing {MissingPageCount} page. Return to seek another path.";
        }

    }
}
