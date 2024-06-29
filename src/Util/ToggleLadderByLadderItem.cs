using System.Collections.Generic;
using UnityEngine;

namespace TunicRandomizer {
    public class ToggleLadderByLadderItem : MonoBehaviour {
        public List<GameObject> constructionItems = new List<GameObject>() { };
        public List<GameObject> optionalObjectsToDisable = new List<GameObject>() { };
        public List<Collider> extraColliders = new List<Collider>();
        public Item ladderItem;
        public LadderInfo ladderInfo;
        public StateVariable stateVariable;

        public void Update() {
            if (ladderItem == null) {
                return;
            }
            bool hasLadder = ladderItem.Quantity > 0;

            foreach (GameObject gameObject in constructionItems) {
                gameObject.SetActive(!hasLadder);
            }

            foreach (GameObject gameObject in optionalObjectsToDisable) {
                gameObject.SetActive(hasLadder);
            }

            for (int i = 0; i < this.transform.childCount; i++) {
                if (PlayerCharacter.instance.currentLadder == this.GetComponent<Ladder>() && this.GetComponent<Ladder>() != null) {
                    continue;
                }
                this.transform.GetChild(i).gameObject.SetActive(hasLadder);
            }

            if (this.GetComponent<Renderer>() != null) {
                this.GetComponent<Renderer>().enabled = hasLadder;
            }
            foreach(Collider c in extraColliders) {
                if (c != null) {
                    c.enabled = !hasLadder;
                }
            }

            if (stateVariable != null && stateVariable.BoolValue != hasLadder)  {
                stateVariable.BoolValue = hasLadder;
            }
        }

        public void SpawnBlockers() {
            if (ladderInfo == null) {
                return;
            }

            foreach (GameObject gameObject in constructionItems) {
                GameObject.Destroy(gameObject);
            }

            if (this.GetComponent<Ladder>() != null && !this.ladderInfo.IsSpecialLadder && ladderItem.Quantity == 0) {
                foreach (LadderEnd end in this.GetComponent<Ladder>().ladderEnds) {
                    if ((end.name.ToLower().Contains("bottom") && ladderInfo.IsExit) || end.name.ToLower().Contains("top") && ladderInfo.IsEntrance) {
                        if (end.gameObject.GetComponent<BoxCollider>() == null) {
                            end.gameObject.AddComponent<BoxCollider>();
                        }
                        end.gameObject.GetComponent<BoxCollider>().size = Vector3.one * 5f;
                        extraColliders.Add(end.gameObject.GetComponent<BoxCollider>());
                    }
                }
            }

            foreach (TransformData transformData in ladderInfo.ConstructionPlacements) {
                GameObject barrier = GameObject.Instantiate(ModelSwaps.UnderConstruction, transformData.pos, transformData.rot);
                barrier.GetComponent<Signpost>().message = new LanguageLine();
                barrier.GetComponent<Signpost>().message.text = $"<#FF0000>[death] uhndur kuhnstruhk$uhn <#FF0000>[death]\n\n\"{ladderItem.name.ToUpper()}\"";
                barrier.SetActive(true);
                if (ladderInfo.LargerColliders) {
                    barrier.GetComponent<BoxCollider>().size = new Vector3(5f, 5f, 5f);
                }
                constructionItems.Add(barrier);
            }

            if (ladderInfo.OptionalObjectsToDisable != null) {
                foreach (string s in ladderInfo.OptionalObjectsToDisable) {
                    if (GameObject.Find(s) != null) {
                        optionalObjectsToDisable.Add(GameObject.Find(s));
                    }
                }
            }

            if (ladderInfo.OptionalStateVar != "") {
                stateVariable = StateVariable.GetStateVariableByName(ladderInfo.OptionalStateVar);
            }

        }
    }
}