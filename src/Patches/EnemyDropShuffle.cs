using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Reflection;
using UnityEngine.AI;
using FMODUnity;

namespace TunicRandomizer {

    public class EnemyCheck : MonoBehaviour {

        public string CheckId;
        public Material ScavengerBombMaterial;

        public void Awake() {
            if (TunicRandomizer.Settings.ChestsMatchContentsEnabled) {
                EnemyModelSwaps.SetupEnemyTexture(this);
            }
        }

        public void ActivateEnemyCheck(Transform transform) {
            if (SaveFile.GetInt("archipelago") == 1 && !Archipelago.instance.IsConnected()) {
                return;
            }
            if (CheckId != null) {
                if (SaveFlags.IsSinglePlayer() && Locations.RandomizedLocations.ContainsKey(CheckId) && !Locations.CheckedLocations[CheckId] && SaveFile.GetInt($"randomizer picked up {CheckId}") == 0) {
                    Check check = Locations.RandomizedLocations[CheckId];
                    ItemPatches.GiveItem(check, alwaysSkip: true);
                    ModelSwaps.SetupItemMoveUp(transform, check: check);
                } else if (SaveFlags.IsArchipelago() && ItemLookup.ItemList.ContainsKey(CheckId) && !Locations.CheckedLocations[CheckId] && SaveFile.GetInt($"randomizer picked up {CheckId}") == 0) {
                    Archipelago.instance.ActivateCheck(Locations.LocationIdToDescription[CheckId]);
                    ModelSwaps.SetupItemMoveUp(transform, itemInfo: ItemLookup.ItemList[CheckId]);
                }
                Destroy(this);
            }
        }
    }

    public class RegionSelector : MonoBehaviour {
        //private Matrix4x4 guiMatrix;
        private void Awake() {
            //this.guiMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(((float)Screen.width) / 1920f, ((float)Screen.height) / 1080f, 1f));
        }
        private void OnGUI() {
            //if (!PlayerCharacter.Instanced) { return; }
            //string scene = SceneManager.GetActiveScene().name;
            //Monster[] monsters = GameObject.FindObjectsOfType<Monster>();
            //Matrix4x4 matrix = GUI.matrix;
            //GUI.matrix = matrix;
            //for (int i = 0; i < monsters.Length; i++) {
            //    Vector2 vector2 = CameraController.instance.cachedCamera.WorldToScreenPoint(monsters[i].gameObject.transform.position);
            //    string text = $"{monsters[i].name} {monsters[i].GetComponent<RuntimeStableID>().ID}";
            //    if (EnemyDropShuffle.EnemyDrops.ContainsKey($"{monsters[i].GetComponent<RuntimeStableID>().ID} [{scene}]")) {
            //        text = EnemyDropShuffle.EnemyDrops[$"{monsters[i].GetComponent<RuntimeStableID>().ID} [{scene}]"].EnemyDescription;
            //    }
            //    monsters[i].monsterAggroDistance = 0;
            //    monsters[i].GetComponent<Rigidbody>().isKinematic = true;
            //    if (vector2.x > 0f && vector2.x < (float)Screen.width && vector2.y > 0f && vector2.y < (float)Screen.height) {
            //        GUI.skin.label.fontSize = 28;
            //        GUI.skin.label.fontStyle = FontStyle.Bold;
            //        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            //        float num = (float)Screen.height - vector2.y;
            //        GUI.color = Color.black;
            //        GUI.Label(new Rect(vector2.x - 252.5f, num + 2.5f, 700f, 22f), text);
            //        GUI.color = Color.white;
            //        GUI.Label(new Rect(vector2.x - 250, num, 700f, 22f), text);
            //    }
            //}

            //TurretTrap[] traps = GameObject.FindObjectsOfType<TurretTrap>();
            //for (int i = 0; i < traps.Length; i++) {
            //    Vector2 vector2 = CameraController.instance.cachedCamera.WorldToScreenPoint(traps[i].gameObject.transform.position); 
            //    string text = $"{traps[i].name} {traps[i].GetComponent<RuntimeStableID>().ID}";
            //    if (EnemyDropShuffle.EnemyDrops.ContainsKey($"{traps[i].GetComponent<PermanentStateByPosition>().initialPosition.ToString()} [{scene}]")) {
            //        text = EnemyDropShuffle.EnemyDrops[$"{traps[i].GetComponent<PermanentStateByPosition>().initialPosition.ToString()} [{scene}]"].EnemyDescription;
            //    }
            //    if (traps[i].GetComponent<Rigidbody>() != null) {
            //        traps[i].GetComponent<Rigidbody>().isKinematic = true;
            //    }
            //    if (vector2.x > 0f && vector2.x < (float)Screen.width && vector2.y > 0f && vector2.y < (float)Screen.height) {
            //        GUI.skin.label.fontSize = 28;
            //        GUI.skin.label.fontStyle = FontStyle.Bold;
            //        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            //        float num = (float)Screen.height - vector2.y;
            //        GUI.color = Color.black;
            //        GUI.Label(new Rect(vector2.x - 252.5f, num + 2.5f, 700f, 22f), text);
            //        GUI.color = Color.white;
            //        GUI.Label(new Rect(vector2.x - 250, num, 700f, 22f), text);
            //    }
            //}
            //if (UnityEngine.Input.GetKeyUp(KeyCode.Mouse0)) {
            //    Ray ray = CameraController.instance.cachedCamera.ScreenPointToRay(UnityEngine.Input.mousePosition);
            //    LayerMask mask = LayerMask.GetMask(new string[]
            //    {
            //        LayerMask.LayerToName(8)
            //    });
            //    RaycastHit raycastHit;
            //    if (Physics.Raycast(ray, out raycastHit, 1000f, mask)) {
            //        if (raycastHit.collider != null) {
            //            if (raycastHit.collider.GetComponent<RuntimeStableID>() != null) {
            //                GUIUtility.systemCopyBuffer = $"{raycastHit.collider.GetComponent<RuntimeStableID>().ID} [{scene}]";
            //            }
            //            if (raycastHit.collider.GetComponent<TurretTrap>() != null) {
            //                GUIUtility.systemCopyBuffer = $"{raycastHit.collider.GetComponent<PermanentStateByPosition>().initialPosition.ToString()} {scene}";
            //            }
            //            TunicLogger.LogInfo("raycast hit");
            //        }
            //    }
            //}
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
        public static Dictionary<string, string> EnemyTypeToSoul = new Dictionary<string, string>();
        public static string EnemyInfoPath = Application.persistentDataPath + "/Randomizer/EnemyData.json";
        public static Dictionary<string, Check> AllEnemyDropChecks = new Dictionary<string, Check>();
        public static Dictionary<string, Check> BaseEnemyDropChecks = new Dictionary<string, Check>();
        public static Dictionary<string, Check> ExtraEnemyDropChecks = new Dictionary<string, Check>();

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
            public List<Dictionary<string, int>> ExtraReqs;
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
                        {"Enemy Soul (Blobs)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Blobs)", 1}
                    },
                }
            },
            {
                "Bumblebones", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Stick", 1},
                        {"Enemy Soul (Fleemers)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Fleemers)", 1}
                    },
                }
            },
            {
                "Bumblebones Big", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Fleemers)", 1}
                    },
                }
            },
            {
                "Crabbit", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Stick", 1},
                        {"Enemy Soul (Crabs)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Crabs)", 1}
                    },
                }
            },
            {
                "Crabbit Shell", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Wand", 1},
                        {"Stick", 1},
                        {"Enemy Soul (Crabs)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Wand", 1},
                        {"Sword", 1},
                        {"Enemy Soul (Crabs)", 1}
                    },
                }
            },
            {
                "Crabbo", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Stick", 1},
                        {"Enemy Soul (Crabs)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Crabs)", 1}
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
                        {"Wand", 1},
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
                    new Dictionary<string, int>() {
                        {"Shotgun", 1},
                        {"Enemy Soul (Autobolt)", 1}
                    },
                }
            },
            {
                "Fencer", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Fleemers)", 1}
                    },
                }
            },
            {
                "FoxEnemy", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Zombie Foxes)", 1}
                    },
                }
            },
            {
                "FoxEnemyZombie", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Zombie Foxes)", 1}
                    },
                }
            },
            {
                "Frog", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Frogs)", 1}
                    },
                }
            },
            {
                "Frog Small", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Frogs)", 1}
                    },
                }
            },
            {
                "Frog Spear", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Frogs)", 1}
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
                        {"Enemy Soul (Hedgehogs)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Hedgehogs)", 1}
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
                        {"Shop", 1},
                        {"Enemy Soul (Envoy)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Wand", 1},
                        {"Enemy Soul (Envoy)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Shotgun", 1},
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
                        {"Wand", 1},
                        {"Techbow", 1},
                        {"Enemy Soul (Librarian)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Wand", 1},
                        {"Shotgun", 1},
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
                        {"Enemy Soul (Fairies)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Fairies)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Techbow", 1},
                        {"Enemy Soul (Fairies)", 1}
                    },
                }
            },
            {
                "Scavenger", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Scavengers)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Techbow", 1},
                        {"Enemy Soul (Scavengers)", 1}
                    },
                }
            },
            {
                "Scavenger Miner", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Scavengers)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Techbow", 1},
                        {"Enemy Soul (Scavengers)", 1}
                    },
                }
            },
            {
                "Scavenger Support", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Scavengers)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Techbow", 1},
                        {"Enemy Soul (Scavengers)", 1}
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
                        {"Enemy Soul (Rudelings)", 1}
                    },
                }
            },
            {
                "Skuladin Big", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Rudelings)", 1}
                    },
                }
            },
            {
                "Skuladin Shield", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Rudelings)", 1}
                    },
                }
            },
            {
                "Spider", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Spiders)", 1}
                    },
                }
            },
            {
                "Spidertank", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Techbow", 1},
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
                    new Dictionary<string, int>() {
                        {"Techbow", 1},
                        {"Enemy Soul (Baby Slorm)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Shotgun", 1 },
                        {"Enemy Soul (Baby Slorm)", 1}
                    }
                }
            },
            {
                "Voidling", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Voidling)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Shotgun", 1},
                        {"Enemy Soul (Voidling)", 1}
                    },
                }
            },
            {
                "Voidtouched", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Hyperdash", 1},
                        {"Enemy Soul (Voidtouched)", 1}
                    },
                }
            },
            {
                "Wizard", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Enemy Soul (Custodians)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Shotgun", 1},
                        {"Enemy Soul (Custodians)", 1}
                    },
                }
            },
            {
                "Wizard Candleabra", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Stundagger", 1},
                        {"Enemy Soul (Custodians)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Shotgun", 1},
                        {"Enemy Soul (Custodians)", 1}
                    },
                }
            },
            {
                "Wizard Sword", new List<Dictionary<string, int>>() {
                    new Dictionary<string, int>() {
                        {"Sword", 1},
                        {"Stundagger", 1},
                        {"Enemy Soul (Custodians)", 1}
                    },
                    new Dictionary<string, int>() {
                        {"Shotgun", 1},
                        {"Enemy Soul (Custodians)", 1}
                    },
                }
            },
        };

        public static string GetEnemyCheckId(GameObject Enemy) {
            if (Enemy.GetComponent<RuntimeStableID>() != null) {
                return $"{Enemy.GetComponent<RuntimeStableID>().ID} [{Enemy.gameObject.scene.name}]";
            }
            if (Enemy.GetComponent<TurretTrap>() != null) {
                string position = Enemy.GetComponent<SmashableObject>().initialPosition.ToString();
                if (position == "(0.0, 0.0, 0.0)") {
                    position = Enemy.transform.position.ToString();
                }
                return $"{position} [{SceneManager.GetActiveScene().name}]";
            }
            return null;
        }

        public static void SaveEnemyData() {
            TunicUtils.TryWriteFile(EnemyInfoPath, JsonConvert.SerializeObject(EnemyDrops, Formatting.Indented));
        } 

        public static void LoadEnemyData() {
            EnemyDrops.Clear();
            var assembly = Assembly.GetExecutingAssembly();
            var enemyDropJson = "TunicRandomizer.src.Data.EnemyData.json";
            AllEnemyDropChecks.Clear();
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
                    List<Dictionary<string, int>> reqsToUse = null;
                    if (enemy.Value.ExtraReqs.Count > 0) {
                        reqsToUse = enemy.Value.ExtraReqs;
                    } else if (enemyRequirements.ContainsKey(enemy.Value.EnemyType)) {
                        reqsToUse = enemyRequirements[enemy.Value.EnemyType];
                    }
                    if (enemy.Value.EnemyType == "Probe" && enemy.Value.EnemyScene == "Cathedral Arena") {
                        reqsToUse = new List<Dictionary<string, int>>() {
                            new Dictionary<string, int>() {
                                {"Wand", 1},
                                {"Sword", 1},
                                {"Enemy Soul (Fairies)", 1},
                            },
                            new Dictionary<string, int>() {
                                {"Techbow", 1},
                                {"Enemy Soul (Fairies)", 1},
                            },
                        };
                    }
                    if (reqsToUse != null) {
                        foreach (Dictionary<string, int> requirements in reqsToUse) {
                            Dictionary<string, int> newReqs = requirements.ToDictionary(entry => entry.Key, entry => entry.Value);
                            newReqs.Add(enemy.Value.EnemyRegion, 1);
                            check.Location.Requirements.Add(newReqs);
                            foreach (var x in newReqs) {
                                if (x.Key.Contains("Enemy Soul")) {
                                    if (!ItemLookup.Items.ContainsKey(x.Key)) {
                                        TunicLogger.LogInfo("ERROR " + x.Key);
                                    } else {
                                        if (!EnemyTypeToSoul.ContainsKey(enemy.Value.EnemyType)) {
                                            EnemyTypeToSoul[enemy.Value.EnemyType] = x.Key;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    AllEnemyDropChecks.Add(enemy.Key, check);
                    if (enemy.Value.IsNGPlusEnemy || enemy.Value.IsNightEnemy) { 
                        ExtraEnemyDropChecks.Add(enemy.Key, check);
                    } else {
                        BaseEnemyDropChecks.Add(enemy.Key, check);
                    }
                    Locations.LocationIdToDescription.Add(check.CheckId, enemy.Value.EnemyDescription);
                    Locations.LocationDescriptionToId.Add(enemy.Value.EnemyDescription, check.CheckId);
                }
            }

            CreateEnemyItems();
        }

        public static void CreateEnemyItems() {
            foreach (ItemData item in ItemLookup.Items.Values.Where(item => item.Type == ItemTypes.ENEMY)) {
                SpecialItem EnemySoul = ScriptableObject.CreateInstance<SpecialItem>();

                EnemySoul.name = item.Name;
                EnemySoul.collectionMessage = TunicUtils.CreateLanguageLine($"ehnuhmE sOl \"- {item.Name.Split('(')[1].Replace(")", "").ToUpper()}\"");
                EnemySoul.controlAction = "";
                Sprite sprite = ModelSwaps.FindSprite("Randomizer items_enemysoul");
                EnemySoul.icon = sprite;
                ItemLookup.SimplifiedItemNames.Add(item.Name, item.Name);
                Translations.EnglishToTrunic.Add($"\"{item.Name}\"", $"\"{item.Name}\"");
                TextBuilderPatches.ItemNameToAbbreviation.Add(item.Name, "[enemysoul]");
                Inventory.itemList.Add(EnemySoul);
                TunicUtils.AllProgressionNames.Add(item.Name);
            }
        }

        public static void SetupEnemyChecks() {
            if (GameObject.FindObjectOfType<EnemySoulManager>() != null) { return; }

            GameObject soulManager = new GameObject("enemy soul manager");
            soulManager.AddComponent<EnemySoulManager>();
            foreach (Monster monster in Resources.FindObjectsOfTypeAll<Monster>().Where(m => m.gameObject.scene.name == SceneManager.GetActiveScene().name && m.GetComponent<EnemyCheck>() == null)) {
                if (monster.GetComponent<RuntimeStableID>() != null) {
                    string id = GetEnemyCheckId(monster.gameObject);
                    if (EnemyDrops.ContainsKey(id) && !SaveFlags.GetBool($"randomizer picked up {id}")) {
                        if (ExtraEnemyDropChecks.ContainsKey(id) && !SaveFlags.GetBool(SaveFlags.ExtraEnemyDropsEnabled)) { continue; }
                        monster.gameObject.AddComponent<EnemyCheck>();
                        monster.gameObject.GetComponent<EnemyCheck>().CheckId = id;
                        soulManager.GetComponent<EnemySoulManager>().registerMonster(monster.gameObject, EnemyDrops[id]);
                    }
                }
            }
            foreach (TurretTrap turretTrap in Resources.FindObjectsOfTypeAll<TurretTrap>().Where(t => t.gameObject.scene.name == SceneManager.GetActiveScene().name && t.GetComponent<EnemyCheck>() == null)) {
                string id = GetEnemyCheckId(turretTrap.gameObject);
                if (EnemyDrops.ContainsKey(id) && !SaveFlags.GetBool($"randomizer picked up {id}")) {
                    if (ExtraEnemyDropChecks.ContainsKey(id) && !SaveFlags.GetBool(SaveFlags.ExtraEnemyDropsEnabled)) { continue; }
                    turretTrap.gameObject.AddComponent<EnemyCheck>();
                    turretTrap.gameObject.GetComponent<EnemyCheck>().CheckId = id;
                    soulManager.GetComponent<EnemySoulManager>().registerMonster(turretTrap.gameObject, EnemyDrops[id]);
                }
            }

            if (SaveFlags.GetBool(SaveFlags.ShuffleEnemySoulsEnabled)) {
                string scene = SceneManager.GetActiveScene().name;
                if (scene == "Overworld Redux") {
                    SetupOverworldEnvoyStatue();
                }
                if (scene == "Spirit Arena") {
                    GameObject cutsceneRoot = GameObject.Find("_BOSSFIGHT ROOT/_CUTSCENE/");
                    if (cutsceneRoot != null) {
                        cutsceneRoot.AddComponent<LockEnemyInteraction>();
                    }
                }
                if (scene == "Cathedral Arena") {
                    foreach (CathedralGauntletSummoner summoner in GameObject.FindObjectsOfType<CathedralGauntletSummoner>()) {
                        summoner.gameObject.AddComponent<LockEnemyInteraction>();
                    }
                    if (GameObject.Find("_Fog/quarry_fogplane_round") != null) {
                        GameObject.Find("_Fog/quarry_fogplane_round").transform.localPosition = new Vector3(-2f, -2f, 31f);
                    }
                }
            }
        }

        public static void SetupOverworldEnvoyStatue() {
            HonourGuard envoy = Resources.FindObjectsOfTypeAll<HonourGuard>().Where(hg => TunicUtils.IsInActiveScene(hg.gameObject) 
            && hg.name == "Honourguard" && hg.GetComponent<RuntimeStableID>().ID == 25000036).FirstOrDefault();
            if (envoy != null) {
                GameObject envoyStatue = GameObject.Instantiate(envoy.gameObject);
                envoyStatue.name = "enemy soul envoy statue";
                envoyStatue.transform.position = envoy.transform.position;
                envoyStatue.transform.localScale = Vector3.one;
                GameObject.Destroy(envoyStatue.GetComponent<Animator>());
                GameObject.Destroy(envoyStatue.GetComponent<NavMeshAgent>());
                GameObject.Destroy(envoyStatue.GetComponent<RuntimeStableID>());
                GameObject.Destroy(envoyStatue.GetComponent<HitReceiver>());
                GameObject.Destroy(envoyStatue.GetComponent<Rigidbody>());
                GameObject.DestroyImmediate(envoyStatue.GetComponent<HonourGuard>());
                GameObject.Destroy(envoyStatue.GetComponent<ZTarget>());
                GameObject.Destroy(envoyStatue.GetComponent<FireController>());
                GameObject.Destroy(envoyStatue.GetComponent<StudioEventEmitter>());
                GameObject.DestroyImmediate(envoyStatue.GetComponent<EnemyCheck>());
                Material mat = ModelSwaps.FindMaterial("granite");
                foreach (CreatureMaterialManager manager in envoyStatue.GetComponentsInChildren<CreatureMaterialManager>()) {
                    GameObject.Destroy(manager);
                }
                foreach (SkinnedMeshRenderer renderer in envoyStatue.GetComponentsInChildren<SkinnedMeshRenderer>()) {
                    renderer.material = mat;
                }
                foreach (MeshRenderer renderer in envoyStatue.GetComponentsInChildren<MeshRenderer>()) { 
                    renderer.material = mat;
                }
                envoyStatue.SetActive(true);
                envoyStatue.AddComponent<VisibleByNotHavingItem>().Item = Inventory.GetItemByName("Enemy Soul (Envoy)");
                TunicLogger.LogInfo("created overworld envoy statue near quarry");
            } else {
                TunicLogger.LogError("overworld envoy not found near quarry for statue");
            }
        }

        public static bool IsValidEnemy(GameObject monster) {
            string checkId = GetEnemyCheckId(monster);
            return checkId != null && AllEnemyDropChecks.ContainsKey(checkId);
        }

        public static bool Administrator_monster_preDestroy_PrefixPatch(Administrator __instance) {

            return true;
        }
    }
}
