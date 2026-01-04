using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Reflection;
using RTLTMPro;
using static Il2CppSystem.Uri;
using System.Xml.Linq;

namespace TunicRandomizer {

    public class EnemyCheck : MonoBehaviour {

        public string CheckId;


        public void ActivateCheck(Transform transform) {
            if (CheckId != null && Locations.RandomizedLocations.ContainsKey(CheckId)) { 
                Check check = Locations.RandomizedLocations[CheckId];
                ItemPatches.GiveItem(check, true);
                ModelSwaps.SetupItemMoveUp(transform, check);
            }
        }
    }

    public class RegionSelector : MonoBehaviour {
        //private Matrix4x4 guiMatrix;
        private void Awake() {
            //this.guiMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(((float)Screen.width) / 1920f, ((float)Screen.height) / 1080f, 1f));
        }
        private void OnGUI() {
            if (!PlayerCharacter.Instanced) { return; }
            string scene = SceneManager.GetActiveScene().name;
            Monster[] monsters = GameObject.FindObjectsOfType<Monster>();
            Matrix4x4 matrix = GUI.matrix;
            GUI.matrix = matrix;
            for (int i = 0; i < monsters.Length; i++) {
                Vector2 vector2 = CameraController.instance.cachedCamera.WorldToScreenPoint(monsters[i].gameObject.transform.position);
                string text = $"{monsters[i].name} {monsters[i].GetComponent<RuntimeStableID>().ID}";
                if (EnemyDropShuffle.EnemyDrops.ContainsKey($"{monsters[i].GetComponent<RuntimeStableID>().ID} [{scene}]")) {
                    text = EnemyDropShuffle.EnemyDrops[$"{monsters[i].GetComponent<RuntimeStableID>().ID} [{scene}]"].EnemyDescription;
                }
                monsters[i].monsterAggroDistance = 0;
                monsters[i].GetComponent<Rigidbody>().isKinematic = true;
                if (vector2.x > 0f && vector2.x < (float)Screen.width && vector2.y > 0f && vector2.y < (float)Screen.height) {
                    GUI.skin.label.fontSize = 28;
                    GUI.skin.label.fontStyle = FontStyle.Bold;
                    GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                    float num = (float)Screen.height - vector2.y;
                    GUI.color = Color.black;
                    GUI.Label(new Rect(vector2.x - 252.5f, num + 2.5f, 700f, 22f), text);
                    GUI.color = Color.white;
                    GUI.Label(new Rect(vector2.x - 250, num, 700f, 22f), text);
                }
            }

            TurretTrap[] traps = GameObject.FindObjectsOfType<TurretTrap>();
            for (int i = 0; i < traps.Length; i++) {
                Vector2 vector2 = CameraController.instance.cachedCamera.WorldToScreenPoint(traps[i].gameObject.transform.position); 
                string text = $"{traps[i].name} {traps[i].GetComponent<RuntimeStableID>().ID}";
                if (EnemyDropShuffle.EnemyDrops.ContainsKey($"{traps[i].GetComponent<PermanentStateByPosition>().initialPosition.ToString()} [{scene}]")) {
                    text = EnemyDropShuffle.EnemyDrops[$"{traps[i].GetComponent<PermanentStateByPosition>().initialPosition.ToString()} [{scene}]"].EnemyDescription;
                }
                if (traps[i].GetComponent<Rigidbody>() != null) {
                    traps[i].GetComponent<Rigidbody>().isKinematic = true;
                }
                if (vector2.x > 0f && vector2.x < (float)Screen.width && vector2.y > 0f && vector2.y < (float)Screen.height) {
                    GUI.skin.label.fontSize = 28;
                    GUI.skin.label.fontStyle = FontStyle.Bold;
                    GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                    float num = (float)Screen.height - vector2.y;
                    GUI.color = Color.black;
                    GUI.Label(new Rect(vector2.x - 252.5f, num + 2.5f, 700f, 22f), text);
                    GUI.color = Color.white;
                    GUI.Label(new Rect(vector2.x - 250, num, 700f, 22f), text);
                }
            }
            if (UnityEngine.Input.GetKeyUp(KeyCode.Mouse0)) {
                Ray ray = CameraController.instance.cachedCamera.ScreenPointToRay(UnityEngine.Input.mousePosition);
                LayerMask mask = LayerMask.GetMask(new string[]
                {
                    LayerMask.LayerToName(8)
                });
                RaycastHit raycastHit;
                if (Physics.Raycast(ray, out raycastHit, 1000f, mask)) {
                    if (raycastHit.collider != null) {
                        if (raycastHit.collider.GetComponent<RuntimeStableID>() != null) {
                            GUIUtility.systemCopyBuffer = $"{raycastHit.collider.GetComponent<RuntimeStableID>().ID} [{scene}]";
                        }
                        if (raycastHit.collider.GetComponent<TurretTrap>() != null) {
                            GUIUtility.systemCopyBuffer = $"{raycastHit.collider.GetComponent<PermanentStateByPosition>().initialPosition.ToString()} {scene}";
                        }
                        TunicLogger.LogInfo("raycast hit");
                    }
                }
            }
            //List<string> regions = new List<string>();
            //foreach (KeyValuePair<string, ERData.RegionInfo> pair in ERData.RegionDict) {
            //    if (pair.Value.Scene == scene) {
            //        regions.Add(pair.Key);
            //    }
            //}
            //int j = 0;
            //int k = 0;
            //GUI.Label(new Rect(10f, 60f, 500f, 30f), $"Selected Region: {EnemyDropShuffle.Region}");
            //GUI.skin.button.fontSize = 15;
            //bool night = GUI.Button(new Rect(10f, 100f, 250, 30), $"Is Night: {StateVariable.GetStateVariableByName("Is Night").BoolValue}");
            //if (night) {
            //    StateVariable.GetStateVariableByName("Is Night").BoolValue = !StateVariable.GetStateVariableByName("Is Night").BoolValue;
            //    if (StateVariable.GetStateVariableByName("Is Night").BoolValue) {
            //        CycleController.AnimateSunset();
            //    } else {
            //        CycleController.AnimateSunrise();
            //    }
            //}
            //bool ngp = GUI.Button(new Rect(260f, 100f, 250, 30), $"New Game+: {NGPActive.NGPStateVar.IntValue > 0}");
            //if (ngp) {
            //    if (NGPActive.NGPStateVar.IntValue == 0) {
            //        NGPActive.NGPStateVar.IntValue = 1;
            //    } else {
            //        NGPActive.NGPStateVar.IntValue = 0;
            //    }
            //}
            //for (int i = 0; i < regions.Count; i++) {
            //    bool setRegion = GUI.Button(new Rect(10f + (j* 250), 130f + (k * 30), 250, 30), regions[i]);
            //    if (setRegion) {
            //        EnemyDropShuffle.Region = regions[i];
            //    }
            //    k++;
            //    if (i > 0 && i % 20 == 0) {
            //        j++;
            //        k = 0;
            //    }
            //}
        }
    }

    public class EnemyDropShuffle {

        public static Dictionary<string, EnemyInfo> EnemyDrops = new Dictionary<string, EnemyInfo>();
        public static string EnemyInfoPath = Application.persistentDataPath + "/Randomizer/EnemyData.json";
        public static Dictionary<string, Check> EnemyDropChecks = new Dictionary<string, Check>();

        public static bool IsNighttime = false;
        public static bool IsNGPlus = false;
        public static string Region = "";

        public class EnemyInfo {
            public string EnemyName;
            public string EnemyRuntimeID;
            public string EnemyType;
            public string EnemyScene;
            public string EnemyPosition;
            public string EnemyRegion;
            public int EnemyDropValue;
            public bool IsNGPlusEnemy;
            public bool IsNightEnemy;
            public string EnemyDescription;
            public List<List<string>> ExtraReqs;
        }

        public static Dictionary<string, List<Dictionary<string, int>>> enemyRequirements = new Dictionary<string, List<Dictionary<string, int>>>() {
            {
                "Administrator", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Administrator)", 1}
                    }
                }
            },
            {
                "Bat", new List<Dictionary<string, int>>() {                   
                    new Dictionary<string, int>() {
                        {"Stick", 1},
                        {"Enemy Soul (Phrend)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Phrend)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Techbow", 1},
                        {"Enemy Soul (Phrend)", 1}
                    },
                }
            },
            {
                "Beefboy", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Beefboy)", 1}
                    }
                }
            },
            {
                "Blob", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Stick", 1},
                        {"Enemy Soul (Blob)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Blob)", 1}
                    },
                }
            },
            {
                "Bumblebones", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Fleemer)", 1}
                    },
                }
            },
            {
                "Bumblebones Big", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Fleemer)", 1}
                    },
                }
            },
            {
                "Crabbit", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Crabbit)", 1}
                    },
                }
            },
            {
                "Crabbit Shell", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Wand", 1},
                        {"Sword", 1},
                        {"Enemy Soul (Crabbit)", 1}
                    },
                }
            },
            {
                "Crabbo", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Crabbo)", 1}
                    },
                }
            },
            {
                "Crocodoo", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Chompignom)", 1}
                    },
                }
            },
            {
                "Crow", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Husher)", 1}
                    },
                }
            },
            {
                "DefenseTurret", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Autobolt)", 1}
                    },
                }
            },
            {
                "Fencer", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Fleemer)", 1}
                    },
                }
            },
            {
                "FoxEnemy", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Zombie Fox)", 1}
                    },
                }
            },
            {
                "FoxEnemyZombie", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Zombie Fox)", 1}
                    },
                }
            },
            {
                "Frog", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Frog)", 1}
                    },
                }
            },
            {
                "Frog Small", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Frog)", 1}
                    },
                }
            },
            {
                "Frog Spear", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Frog)", 1}
                    },
                }
            },
            {
                "Gost", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Lost Echo)", 1}
                    },
                }
            },
            {
                "Gunman", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Gunslinger)", 1}
                    },
                }
            },
            {
                "Hedgehog", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Stick", 1},
                        {"Enemy Soul (Hedgehog)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Hedgehog)", 1}
                    },
                }
            },
            {
                "Hedgehog Trap", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Stick", 1},
                        {"Enemy Soul (Laser Trap)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Laser Trap)", 1}
                    },
                }
            },
            {
                "HonourGuard", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Envoy)", 1}
                    },
                }
            },
            {
                "Knightbot", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Garden Knight)", 1}
                    },
                }
            },
            {
                "Librarian", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Librarian)", 1}
                    }
                }
            },
            {
                "Plover", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Stick", 1},
                        {"Enemy Soul (Plover)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Plover)", 1}
                    },
                }
            },
            {
                "Probe", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Wand", 1},
                        {"Stick", 1},
                        {"Enemy Soul (Fairy)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Fairy)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Techbow", 1},
                        {"Enemy Soul (Fairy)", 1}
                    },
                }
            },
            {
                "Scavenger", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Scavenger)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Techbow", 1},
                        {"Enemy Soul (Scavenger)", 1}
                    },
                }
            },
            {
                "Scavenger Miner", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Scavenger)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Techbow", 1},
                        {"Enemy Soul (Scavenger)", 1}
                    },
                }
            },
            {
                "Scavenger Support", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Scavenger)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Techbow", 1},
                        {"Enemy Soul (Scavenger)", 1}
                    },
                }
            },
            {
                "ScavengerBoss", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Boss Scavenger)", 1}
                    }
                }
            },
            {
                "SewerTentacle", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Tentacle)", 1}
                    }
                }
            },
            {
                "Skuladin", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Rudeling)", 1}
                    },
                }
            },
            {
                "Skuladin Big", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Rudeling)", 1}
                    },
                }
            },
            {
                "Skuladin Shield", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Rudeling)", 1}
                    },
                }
            },
            {
                "Spider", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Spider)", 1}
                    },
                }
            },
            {
                "Spidertank", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Siege Engine)", 1}
                    }
                }
            },
            {
                "Spinnerbot", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Slorm)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Techbow", 1},
                        {"Enemy Soul (Slorm)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Shotgun", 1 },
                        {"Enemy Soul (Slorm)", 1}
                    }
                }
            },
            {
                "Spinnerbot Baby", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Stick", 1},
                        {"Enemy Soul (Baby Slorm)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Baby Slorm)", 1}
                    },
                }
            },
            {
                "Voidling", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Voidling)", 1}
                    },
                }
            },
            {
                "Voidtouched", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Voidtouched)", 1}
                    },
                }
            },
            {
                "Wizard", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Custodian)", 1}
                    },
                }
            },
            {
                "Wizard Candleabra", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Custodian)", 1}
                    },
                }
            },
            {
                "Wizard Sword", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Custodian)", 1}
                    },
                }
            },
        };

        public static void SaveEnemyData() {
            TunicUtils.TryWriteFile(EnemyInfoPath, JsonConvert.SerializeObject(EnemyDrops, Formatting.Indented));
        } 

        public static void LoadEnemyData() {
            EnemyDrops.Clear();
            var assembly = Assembly.GetExecutingAssembly();
            var enemyDropJson = "TunicRandomizer.src.Data.EnemyData.json";
            EnemyDropChecks.Clear();
            System.Random random = new System.Random();
            List<ItemData> itemNames = ItemLookup.Items.Values.Where(item => ItemLookup.FillerItems.ContainsKey(item.ItemNameForInventory)).ToList();
            using (Stream stream = assembly.GetManifestResourceStream(enemyDropJson))
            using (StreamReader reader = new StreamReader(stream)) {
                EnemyDrops = JsonConvert.DeserializeObject<Dictionary<string, EnemyInfo>>(reader.ReadToEnd());
                foreach (KeyValuePair<string, EnemyInfo> enemy in EnemyDrops) { 
                    Check check = new Check();
                    check.Reward = new Reward();
                    int i = random.Next(itemNames.Count);
                    check.Reward.Name = itemNames[i].ItemNameForInventory;
                    check.Reward.Type = "INVENTORY";
                    check.Reward.Amount = itemNames[i].QuantityToGive;
                    check.Location = new Location();
                    check.Location.LocationId = enemy.Value.EnemyRuntimeID;
                    check.Location.SceneName = enemy.Value.EnemyScene;
                    check.Location.Position = enemy.Value.EnemyPosition;
                    check.Location.SceneId = 0;
                    check.Location.Requirements = new List<Dictionary<string, int>>();
                    if (enemyRequirements.ContainsKey(enemy.Value.EnemyType)) {
                        foreach (Dictionary<string, int> requirements in enemyRequirements[enemy.Value.EnemyType]) {
                            Dictionary<string, int> newReqs = requirements.ToDictionary(entry => entry.Key, entry => entry.Value);
                            newReqs.Add(enemy.Value.EnemyRegion, 1);
                            check.Location.Requirements.Add(newReqs);
                            foreach (var x in newReqs) {
                                if (x.Key.Contains("Enemy Soul")) {
                                    if (!ItemLookup.Items.ContainsKey(x.Key)) {
                                        TunicLogger.LogInfo("ERROR " + x.Key);
                                    } else {
                                        TunicLogger.LogInfo(x.Key);
                                    }
                                }
                            }
                        }
                    }
                    EnemyDropChecks.Add(enemy.Key, check);
                    Locations.LocationIdToDescription.Add(check.CheckId, enemy.Value.EnemyDescription);
                    Locations.LocationDescriptionToId.Add(enemy.Value.EnemyDescription, check.CheckId);
                }
            }

            foreach (string type in enemyRequirements.Keys) {
                TunicLogger.LogInfo($"{{ \"Enemy Soul ({type})\", new ItemData(\"Enemy Soul ({type})\", \"progression\", \"Enemy Soul ({type})\", ItemTypes.ENEMY, 1) }},");
            }
            CreateEnemyItems();
            ItemPresentationPatches.SetupEnemyPresentation();
        }

        public static void CreateEnemyItems() {
            foreach (ItemData item in ItemLookup.Items.Values.Where(item => item.Type == ItemTypes.ENEMY)) {
                SpecialItem EnemySoul = ScriptableObject.CreateInstance<SpecialItem>();

                EnemySoul.name = item.Name;
                EnemySoul.collectionMessage = TunicUtils.CreateLanguageLine($"ehnuhmE sOl \"- {item.Name.Split('(')[1].Replace(")", "").ToUpper()}\"");
                EnemySoul.controlAction = "";
                EnemySoul.icon = ModelSwaps.FindSprite("Randomizer items_fuse");
                Inventory.itemList.Add(EnemySoul);
            }
        }

        public static void RemoveRecordedEnemies() {
            foreach (Monster monster in Resources.FindObjectsOfTypeAll<Monster>().Where(m => m.gameObject.scene.name == SceneManager.GetActiveScene().name)) {
                if (monster.GetComponent<RuntimeStableID>() != null) {
                    string id = $"{monster.GetComponent<RuntimeStableID>().ID} [{SceneManager.GetActiveScene().name}]";
                    if (EnemyDrops.ContainsKey(id)) {
                        //EnemyDrops[id].EnemyPosition = monster.transform.position.ToString();
                        //if (monster.dropValue != null) {
                        //    EnemyDrops[id].EnemyDropValue = monster.dropValue.IntValue;
                        //}
                        //GameObject.Destroy(monster.gameObject);
                        monster.gameObject.AddComponent<EnemyCheck>();
                        monster.gameObject.GetComponent<EnemyCheck>().CheckId = id;
                    }
                }
            }
        }

        public static void SetupEnemyChecks() {
            foreach (Monster monster in Resources.FindObjectsOfTypeAll<Monster>().Where(m => m.gameObject.scene.name == SceneManager.GetActiveScene().name)) {
                if (monster.GetComponent<RuntimeStableID>() != null) {
                    string id = $"{monster.GetComponent<RuntimeStableID>().ID} [{SceneManager.GetActiveScene().name}]";
                    if (EnemyDrops.ContainsKey(id)) {
                        monster.gameObject.AddComponent<EnemyCheck>();
                        monster.gameObject.GetComponent<EnemyCheck>().CheckId = id;
                    }
                }
            }
        }
    }
}
