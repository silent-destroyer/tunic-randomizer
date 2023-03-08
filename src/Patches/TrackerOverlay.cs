using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TunicRandomizer {
    public class TrackerOverlay {
        public static ManualLogSource Logger = TunicRandomizer.Logger;
        public static ItemTracker Tracker = TunicRandomizer.Tracker;
        // create holder under HUD GUI
        // or reuse _GameGUI(Clone)/HUD Canvas/Scaler/Tracker/GROUP: Equipment(Clone)/
        // -460 225 0 1st row
        // -460 200 0 2nd row
        public static GameObject Overlay;
        public static Dictionary<string, GameObject> OverlayItems = new Dictionary<string, GameObject>() {
            {"Inventory items_stick", null},
            {"Inventory items_sword", null},
            {"Inventory items_stundagger", null},
            {"Inventory items_techbow", null},
            {"Inventory items_forcewand", null},
            {"Inventory items_cape", null},
            {"Inventory items_lantern", null},
            {"Inventory items_shield", null},
            {"Inventory items_hourglass", null},
            {"Inventory items_shotgun", null},
            {"Inventory items_dash stone", null},
            {"Inventory items_key", null},
            {"Inventory items_vault key", null},
            {"Inventory items 3_shard", null},
            {"Inventory items_potion", null},
            {"Inventory items_coin question mark", null},
            {"Inventory items_trinketslot", null},
            {"Inventory items_trinketcard", null},
            {"Inventory items_book", null},
            {"Inventory items_fairy", null},
            {"Inventory items_trophy", null},
            {"Inventory items_offering_ash", null},
            {"Inventory items_offering_effigy", null},
            {"Inventory items_offering_feather", null},
            {"Inventory items_offering_tooth", null},
            {"Inventory items_offering_flower", null},
            {"Inventory items_offering_orb", null},
            {"UI_hexagon_R", null},
            {"UI_hexagon_G", null},
            {"UI_hexagon_B", null},
        };
        public static Dictionary<string, string> ItemNamesToSpriteNames = new Dictionary<string, string>() {
            {"Inventory items_stick", "Stick"},
            {"Inventory items_sword", "Sword"},
            {"Inventory items_stundagger", "Stundagger"},
            {"Inventory items_techbow", "Techbow"},
            {"Inventory items_forcewand", "Wand"},
            {"Inventory items_cape", "Hyperdash"},
            {"Inventory items_lantern", "Lantern"},
            {"Inventory items_shield", "Shield"},
            {"Inventory items_hourglass", "SlowmoItem"},
            {"Inventory items_shotgun", "Shotgun"},
            {"Inventory items_dash stone", "Dath Stone"},
            {"Inventory items_key", "Key"},
            {"Inventory items_vault key", "Vault Key (Red)"},
            {"Inventory items 3_shard", "Flask Shard"},
            {"Inventory items_potion", "Flask Container"},
            {"Inventory items_coin question mark", "Trinket Coin"},
            {"Inventory items_trinketslot", "Trinket Slot"},
            {"Inventory items_trinketcard", "Trinket Card"},
            {"Inventory items_book", "Pages"},
            {"Inventory items_fairy", "Fairies"},
            {"Inventory items_trophy", "Golden Trophies"},
            {"Inventory items_offering_ash", null},
            {"Inventory items_offering_effigy", null},
            {"Inventory items_offering_feather", null},
            {"Inventory items_offering_tooth", null},
            {"Inventory items_offering_flower", null},
            {"Inventory items_offering_orb", null},
            {"UI_hexagon_R", null},
            {"UI_hexagon_G", null},
            {"UI_hexagon_B", null},
        };
        public static bool TrackerLoaded = false;


        public static void Initialize() {
            GameObject Base = GameObject.Find("_GameGUI(Clone)/HUD Canvas/Scaler");
            Material UIMat = Resources.FindObjectsOfTypeAll<Material>().Where(Material => Material.name == "UI Add").ToList()[0];
            Overlay = new GameObject("Tracker");
            Overlay.layer = 5;
            Overlay.transform.parent = Base.transform;
            Overlay.transform.position = new Vector3(-460f, 225f, 0);
            float x = -460f;
            float y = 225f;
            foreach (Sprite ItemSprite in Resources.FindObjectsOfTypeAll<Sprite>().Where(Sprite => OverlayItems.Keys.ToList().Contains(Sprite.name))) {
                OverlayItems[ItemSprite.name] = new GameObject(ItemSprite.name);
                OverlayItems[ItemSprite.name].transform.parent = Overlay.transform;
                OverlayItems[ItemSprite.name].AddComponent<CanvasRenderer>();
                OverlayItems[ItemSprite.name].AddComponent<Image>().sprite = ItemSprite;
                OverlayItems[ItemSprite.name].GetComponent<Image>().useSpriteMesh = true;
                OverlayItems[ItemSprite.name].GetComponent<Image>().color = new Color(.9f, .9f, .9f, 1);
                OverlayItems[ItemSprite.name].GetComponent<Image>().material = UIMat;
                OverlayItems[ItemSprite.name].transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            }
            for (int i = 0; i < OverlayItems.Count; i++) {
                OverlayItems[OverlayItems.Keys.ToList()[i]].transform.position = new Vector3(x, y, 0);
                SetupHexagonBackground(OverlayItems[OverlayItems.Keys.ToList()[i]]);
                x += 25f;
                if (i == 9) {
                    x = -460f;
                    y = 200f;
                }
                if (i == 19) {
                    x = -460f;
                    y = 175f;
                }
            }
/*            SetupHexagonBackground(OverlayItems["UI_hexagon_R"]);
            SetupHexagonBackground(OverlayItems["UI_hexagon_G"]);
            SetupHexagonBackground(OverlayItems["UI_hexagon_B"]);*/

            TrackerLoaded = true;
        }

        private static void SetupHexagonBackground(GameObject Hexagon) {
            Sprite BackingSprite = Resources.FindObjectsOfTypeAll<Sprite>().Where(Sprite => Sprite.name == "UI_offeringBacking").ToList()[0];
            GameObject Backing = new GameObject(Hexagon.name + " backing");
            Backing.AddComponent<CanvasRenderer>();
            Backing.AddComponent<Image>().color = new Color(0, 0, 0, 0.75f);
            //Backing.GetComponent<Image>().sprite = BackingSprite;
            Backing.transform.position = Hexagon.transform.position;
            Backing.transform.parent = Overlay.transform;
            Backing.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            Hexagon.transform.parent = Backing.transform;
        }

        public static void UpdateSprite() { 
            
        }
    }
}
