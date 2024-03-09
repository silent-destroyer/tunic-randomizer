using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TunicRandomizer {
    public class ToggleLadderByLadderItem : MonoBehaviour {
        public List<GameObject> constructionItems = new List<GameObject>() { };
        public List<GameObject> optionalObjectsToDisable = new List<GameObject>() { };
        public Item ladderItem;
        public LadderInfo ladderInfo;
        public StateVariable stateVariable;

        public void Update() {
            if (ladderItem == null) {
                return;
            }

            foreach (GameObject gameObject in constructionItems) {
                gameObject.SetActive(ladderItem.Quantity == 0);
            }

            foreach (GameObject gameObject in optionalObjectsToDisable) {
                gameObject.SetActive(ladderItem.Quantity > 0);
            }

            for (int i = 0; i < this.transform.childCount; i++) {
                if (PlayerCharacter.instance.currentLadder == this.GetComponent<Ladder>()) {
                    continue;
                }
                this.transform.GetChild(i).gameObject.SetActive(ladderItem.Quantity > 0);
            }

            if (this.GetComponent<Renderer>() != null) {
                this.GetComponent<Renderer>().enabled = ladderItem.Quantity > 0;
            }

            if (stateVariable != null) {
                stateVariable.BoolValue = ladderItem.Quantity > 0;
            }
        }

        public void SpawnBlockers() {
            if (ladderInfo == null) {
                return;
            }

            foreach (GameObject gameObject in constructionItems) {
                GameObject.Destroy(gameObject);
            }

            if (this.GetComponent<Ladder>() != null && !this.ladderInfo.IsStoneLadder && ladderItem.Quantity == 0) {
                foreach (LadderEnd end in this.GetComponent<Ladder>().ladderEnds) {
                    if ((end.name.ToLower().Contains("bottom") && ladderInfo.IsExit) || end.name.ToLower().Contains("top") && ladderInfo.IsEntrance) {
                        if (end.gameObject.GetComponent<BoxCollider>() == null) {
                            end.gameObject.AddComponent<BoxCollider>();
                        }
                        end.gameObject.GetComponent<BoxCollider>().size = Vector3.one * 5f;
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