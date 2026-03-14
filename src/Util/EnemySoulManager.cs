using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TunicRandomizer {
    public class EnemySoulManager : MonoBehaviour {
        public Dictionary<string, List<GameObject>> monsterSouls = new Dictionary<string, List<GameObject>>();
        public void Update() {
            if (!SaveFlags.GetBool(SaveFlags.ShuffleEnemySoulsEnabled)) { return; }
            foreach (KeyValuePair<string, List<GameObject>> pair in monsterSouls) {
                if (Inventory.GetItemByName(pair.Key).Quantity == 0) {
                    foreach (GameObject m in pair.Value) {
                        m.SetActive(false);
                    }
                }
            }
        }

        public void registerMonster(GameObject monster, EnemyDropShuffle.EnemyInfo enemyInfo) {
            if (!monsterSouls.ContainsKey(EnemyDropShuffle.EnemyTypeToSoul[enemyInfo.EnemyType])) {
                if (Inventory.GetItemByName(EnemyDropShuffle.EnemyTypeToSoul[enemyInfo.EnemyType]).Quantity > 0) {
                    return;
                }
                monsterSouls.Add(EnemyDropShuffle.EnemyTypeToSoul[enemyInfo.EnemyType], new List<GameObject>());
            }
            monsterSouls[EnemyDropShuffle.EnemyTypeToSoul[enemyInfo.EnemyType]].Add(monster);
        }

        public void onItemGet(string ItemName) {
            if (monsterSouls.ContainsKey(ItemName)) {
                foreach (GameObject obj in monsterSouls[ItemName]) {
                    obj.SetActive(true);
                }
            }
            monsterSouls.Remove(ItemName);
        }
    }

    public class LockEnemyInteraction : MonoBehaviour {

        public struct EnemyInteractionData {
            public string itemName;
            public Vector3 position;

            public EnemyInteractionData(string item, Vector3 pos) {
                itemName = item;
                position = pos;
            }
        }

        public static Dictionary<string, EnemyInteractionData> interactionData = new Dictionary<string, EnemyInteractionData>() {
            {"cathedral_wavealtar (probes)", new EnemyInteractionData("Enemy Soul (Fairies)", new Vector3(0, 3f, 0)) },
            {"cathedral_wavealtar (skuladins)", new EnemyInteractionData("Enemy Soul (Rudelings)", new Vector3(0, 3.5f, -1f)) },
            {"cathedral_wavealtar (knight)", new EnemyInteractionData("Enemy Soul (Garden Knight)", new Vector3(0, 4, -2)) },
            {"cathedral_wavealtar (frogs)", new EnemyInteractionData("Enemy Soul (Frogs)", new Vector3(0, 3.5f, -1f)) },
            {"cathedral_wavealtar (skeletons)", new EnemyInteractionData("Enemy Soul (Fleemers)", new Vector3(0, 3f, 0)) },
            {"cathedral_wavealtar (wizards)", new EnemyInteractionData("Enemy Soul (Custodians)", new Vector3(0, 3f, 0)) },
            {"_CUTSCENE", new EnemyInteractionData("Enemy Soul (The Heir)", new Vector3(0, 5f, -0.5f)) },
        };

        public GameObject graphic;
        public InteractionTrigger trigger;
        public Item item;
        public void Awake() {
            if (interactionData.TryGetValue(name, out var data)) {
                graphic = GameObject.Instantiate(ModelSwaps.ShadowOubliette);
                foreach (Transform transform in graphic.GetComponentsInChildren<Transform>(true)) {
                    transform.gameObject.layer = 0;
                }
                if (name == "_CUTSCENE" && GameObject.Find("_BOSSFIGHT ROOT/Foxgod/") != null) {
                    graphic.transform.parent = GameObject.Find("_BOSSFIGHT ROOT/Foxgod/").transform;
                    graphic.transform.localScale = Vector3.one * 0.625f;
                } else {
                    graphic.transform.parent = transform;
                    graphic.transform.localScale = Vector3.one / 2f;
                    graphic.AddComponent<SphereCollider>().radius = 8;
                }
                graphic.transform.localPosition = data.position;
                graphic.SetActive(true);
                item = Inventory.GetItemByName(data.itemName);
                trigger = GetComponent<InteractionTrigger>();
                TunicLogger.LogInfo("setup interaction lock on " + name);
            } else {
                TunicLogger.LogInfo("destroying " + name + " as it is not a valid enemy interaction");
                Destroy(this);
            }
        }

        public void Update() {
            if (item != null && trigger != null && graphic != null) {
                trigger.enabled = item.Quantity > 0;
                graphic.SetActive(item.Quantity == 0);
            }
        }
    }
}
