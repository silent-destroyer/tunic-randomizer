using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TunicRandomizer {
    public class FairyTargets {

        public static void CreateFairyTargets() {
            if (ItemRandomizer.ItemList.Count > 0) {
                List<string> ItemIdsInScene = ItemRandomizer.ItemList.Keys.Where(itemId => ItemRandomizer.ItemList[itemId].Location.SceneName == SceneLoaderPatches.SceneName && !ItemRandomizer.ItemsPickedUp[itemId]).ToList();
                if (ItemIdsInScene.Count > 0) {
                    foreach (string ItemId in ItemIdsInScene) {
                        ItemData Item = ItemRandomizer.ItemList[ItemId];

                        if (GameObject.Find($"fairy target {Item.Location.Position}") == null) {
                            CreateFairyTarget($"fairy target {Item.Location.Position}", StringToVector3(Item.Location.Position));
                        }
                    }
                    if (GameObject.FindObjectOfType<TrinketWell>() != null) {
                        int CoinCount = Inventory.GetItemByName("Trinket Coin").Quantity + TunicRandomizer.Tracker.ImportantItems["Coins Tossed"];
                        Dictionary<int, int> CoinLevels = new Dictionary<int, int>() { { 0, 3 }, { 1, 6 }, { 2, 10 }, { 3, 15 }, { 4, 20 } };
                        int CoinsNeededForNextReward = CoinLevels[ItemRandomizer.ItemList.Keys.Where(ItemId => ItemRandomizer.ItemList[ItemId].Location.SceneName == "Trinket Well" && ItemRandomizer.ItemsPickedUp[ItemId]).ToList().Count];

                        if ((Inventory.GetItemByName("Trinket Coin").Quantity + TunicRandomizer.Tracker.ImportantItems["Coins Tossed"]) > CoinsNeededForNextReward) {
                            CreateFairyTarget($"fairy target trinket well", GameObject.FindObjectOfType<TrinketWell>().transform.position);
                        }
                    }
                } else {
                    CreateLoadZoneTargets();
                }
            }
        }

        public static void CreateLoadZoneTargets() {
            HashSet<string> ScenesWithItems = new HashSet<string>();

            foreach (FairyTarget FairyTarget in Resources.FindObjectsOfTypeAll<FairyTarget>()) {
                FairyTarget.enabled = false;
            }

            foreach (string ItemId in ItemRandomizer.ItemList.Keys.Where(itemId => ItemRandomizer.ItemList[itemId].Location.SceneName != SceneLoaderPatches.SceneName && !ItemRandomizer.ItemsPickedUp[itemId])) {
                ScenesWithItems.Add(ItemRandomizer.ItemList[ItemId].Location.SceneName);
            }

            foreach (ScenePortal ScenePortal in Resources.FindObjectsOfTypeAll<ScenePortal>()) {
                if (ScenesWithItems.Contains(ScenePortal.destinationSceneName)) {
                    CreateFairyTarget($"fairy target {ScenePortal.destinationSceneName}", ScenePortal.transform.position);
                }
            }
        }

        private static void CreateFairyTarget(string Name, Vector3 Position) {
            GameObject FairyTarget = new GameObject(Name);
            FairyTarget.SetActive(true);
            FairyTarget.AddComponent<FairyTarget>();
            FairyTarget.GetComponent<FairyTarget>().stateVariable = StateVariable.GetStateVariableByName("false");
            FairyTarget.transform.position = Position;
        }

        private static Vector3 StringToVector3(string Position) {
            Position = Position.Replace("(", "").Replace(")", "");
            string[] coords = Position.Split(',');
            Vector3 vector = new Vector3(float.Parse(coords[0]), float.Parse(coords[1]), float.Parse(coords[2]));
            return vector;
        }

    }
}
