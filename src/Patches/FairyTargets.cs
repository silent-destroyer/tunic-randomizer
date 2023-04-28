using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TunicRandomizer {
    public class FairyTargets {

        public static void CreateFairyTargets() {
            if (RandomItemPatches.ItemList.Count > 0) {
                List<string> ItemIdsInScene = RandomItemPatches.ItemList.Keys.Where(itemId => RandomItemPatches.ItemList[itemId].Location.SceneName == SceneLoaderPatches.SceneName && !RandomItemPatches.ItemsPickedUp[itemId]).ToList();
                if (ItemIdsInScene.Count > 0) {
                    foreach (string ItemId in ItemIdsInScene) {
                        ItemData Item = RandomItemPatches.ItemList[ItemId];

                        if (GameObject.Find($"fairy target {Item.Location.Position}") == null) {
                            CreateFairyTarget($"fairy target {Item.Location.Position}", StringToVector3(Item.Location.Position));
                        }
                    }
                    if (GameObject.FindObjectOfType<TrinketWell>() != null) {
                        int CoinCount = Inventory.GetItemByName("Trinket Coin").Quantity + TunicRandomizer.Tracker.ImportantItems["Coins Tossed"];
                        Dictionary<int, int> CoinLevels = new Dictionary<int, int>() { { 0, 3 }, { 1, 6 }, { 2, 10 }, { 3, 15 }, { 4, 20 } };
                        int CoinsNeededForNextReward = CoinLevels[RandomItemPatches.ItemList.Keys.Where(ItemId => RandomItemPatches.ItemList[ItemId].Location.SceneName == "Trinket Well" && RandomItemPatches.ItemsPickedUp[ItemId]).ToList().Count];

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

            foreach (string ItemId in RandomItemPatches.ItemList.Keys.Where(itemId => RandomItemPatches.ItemList[itemId].Location.SceneName != SceneLoaderPatches.SceneName && !RandomItemPatches.ItemsPickedUp[itemId])) {
                ScenesWithItems.Add(RandomItemPatches.ItemList[ItemId].Location.SceneName);
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
