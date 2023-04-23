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
                            GameObject FairyTarget = new GameObject($"fairy target {Item.Location.Position}");
                            FairyTarget.SetActive(true);
                            FairyTarget.AddComponent<FairyTarget>();
                            FairyTarget.GetComponent<FairyTarget>().stateVariable = StateVariable.GetStateVariableByName("false");
                            FairyTarget.transform.position = StringToVector3(Item.Location.Position);
                        }
                    }
                } else {
                    HashSet<string> ScenesWithItems = new HashSet<string>();

                    foreach (FairyTarget FairyTarget in Resources.FindObjectsOfTypeAll<FairyTarget>()) {
                        FairyTarget.enabled = false;
                    }

                    foreach (string ItemId in RandomItemPatches.ItemList.Keys.Where(itemId => RandomItemPatches.ItemList[itemId].Location.SceneName != SceneLoaderPatches.SceneName && !RandomItemPatches.ItemsPickedUp[itemId])) {
                        ScenesWithItems.Add(RandomItemPatches.ItemList[ItemId].Location.SceneName);
                    }

                    foreach (ScenePortal ScenePortal in Resources.FindObjectsOfTypeAll<ScenePortal>()) {
                        if (ScenesWithItems.Contains(ScenePortal.destinationSceneName)) {
                            GameObject FairyTarget = new GameObject($"fairy target {ScenePortal.destinationSceneName}");
                            FairyTarget.SetActive(true);
                            FairyTarget.AddComponent<FairyTarget>();
                            FairyTarget.GetComponent<FairyTarget>().stateVariable = StateVariable.GetStateVariableByName("false");
                            FairyTarget.transform.position = ScenePortal.transform.position;
                        }
                    }
                }
            }
        }

        private static Vector3 StringToVector3(string Position) {
            Position = Position.Replace("(", "").Replace(")", "");
            string[] coords = Position.Split(',');
            Vector3 vector = new Vector3(float.Parse(coords[0]), float.Parse(coords[1]), float.Parse(coords[2]));
            return vector;
        }

    }
}
