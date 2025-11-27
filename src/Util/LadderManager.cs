using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {

    public class LadderZone {
        public GameObject Ladder;
        public List<GameObject> ConstructionItems = new List<GameObject>() { };
        public List<GameObject> OptionalObjectsToDisable = new List<GameObject>() { };
        public List<Collider> ExtraColliders = new List<Collider>();
        public Item LadderItem;
        public LadderInfo LadderInfo;
        public StateVariable StateVariable;

        public LadderZone() { }

        public LadderZone(GameObject ladder, List<GameObject> constructionItems, List<GameObject> optionalObjectsToDisable, List<Collider> extraColliders, Item ladderItem, LadderInfo ladderInfo, StateVariable stateVariable) {
            Ladder = ladder;
            ConstructionItems = constructionItems;
            OptionalObjectsToDisable = optionalObjectsToDisable;
            ExtraColliders = extraColliders;
            LadderItem = ladderItem;
            LadderInfo = ladderInfo;
            StateVariable = stateVariable;
        }
    }
    public class LadderManager : MonoBehaviour {

        private IEnumerator<bool> ladderHandler;
        public Dictionary<string, List<LadderZone>> ladderZones;

        public void Awake() {
            ladderHandler = HandleLadderUpdates();
            ladderZones = new Dictionary<string, List<LadderZone>>();
        }

        public void Update() {
            if (!GetBool(LadderRandoEnabled)) { return; }

            if (ladderHandler != null && ladderZones.Count > 0) { 
                ladderHandler.MoveNext();
            }
        }

        public IEnumerator<bool> HandleLadderUpdates() {
            while (true) {
                foreach (List<LadderZone> zones in ladderZones.Values) {
                    foreach (LadderZone zone in zones) { 
                        if (zone.LadderItem == null) {
                            continue;
                        }
                        bool hasLadder = zone.LadderItem.Quantity > 0;
                        for (int i = 0; i < zone.ConstructionItems.Count; i++) {
                            zone.ConstructionItems[i].SetActive(!hasLadder);
                        }

                        yield return true;

                        for (int i = 0; i < zone.OptionalObjectsToDisable.Count; i++) {
                            zone.OptionalObjectsToDisable[i].SetActive(hasLadder);
                        }
                    
                        yield return true;

                        for (int i = 0; i < zone.Ladder.transform.childCount; i++) {
                            if (PlayerCharacter.instance.currentLadder == zone.Ladder.GetComponent<Ladder>() && zone.Ladder.GetComponent<Ladder>() != null) {
                                continue;
                            }
                            zone.Ladder.transform.GetChild(i).gameObject.SetActive(hasLadder);
                        }

                        yield return true;

                        if (zone.Ladder.GetComponent<Renderer>() != null) {
                            zone.Ladder.GetComponent<Renderer>().enabled = hasLadder;
                        }

                        yield return true;

                        for (int i = 0; i < zone.ExtraColliders.Count; i++) {
                            if (zone.ExtraColliders[i] != null) {
                                zone.ExtraColliders[i].enabled = !hasLadder;
                            }
                        }

                        if (zone.StateVariable != null && zone.StateVariable.BoolValue != hasLadder) {
                            zone.StateVariable.BoolValue = hasLadder;
                        }
                        yield return true;
                    }
                    yield return true;
                }
            }
        }

        public void AddLadderZone(GameObject ladder, Item ladderItem, LadderInfo ladderInfo) {
            if (ladderInfo == null) {
                return;
            }

            List<GameObject> constructionItems = new List<GameObject>() { };
            List<GameObject> optionalObjectsToDisable = new List<GameObject>() { };
            List<Collider> extraColliders = new List<Collider>();
            StateVariable stateVariable = null;

            if (ladder.GetComponent<Ladder>() != null && !ladderInfo.IsSpecialLadder && ladderItem.Quantity == 0) {
                foreach (LadderEnd end in ladder.GetComponent<Ladder>().ladderEnds) {
                    if (end != null && ((end.name.ToLower().Contains("bottom") && ladderInfo.IsExit) || (end.name.ToLower().Contains("top") && ladderInfo.IsEntrance))) {
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
                barrier.GetComponent<Signpost>().message = ScriptableObject.CreateInstance<LanguageLine>();
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

            LadderZone ladderZone = new LadderZone(ladder, constructionItems, optionalObjectsToDisable, extraColliders, ladderItem, ladderInfo, stateVariable);
            if (!ladderZones.ContainsKey(ladderItem.name)) {
                ladderZones.Add(ladderItem.name, new List<LadderZone>());
            }
            ladderZones[ladderItem.name].Add(ladderZone);
        }
    }
}
