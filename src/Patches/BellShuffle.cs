using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using Newtonsoft.Json;
using UnityEngine;

namespace TunicRandomizer {
    public class BellShuffle {

        public static string BellCheckJson = @"[
            {
                ""Reward"": {
                    ""Name"": ""West Bell"",
                    ""Type"": ""INVENTORY"",
                    ""Amount"": 1
                },
                ""Location"": {
                    ""LocationId"": ""tuning fork"",
                    ""SceneName"": ""Overworld Redux"",
                    ""SceneId"": 25,
                    ""Position"": ""(-131.5, 52.0, -40.0)"",
                    ""Requirements"": [
                        {
                            ""Stick"": 1,
                            ""Overworld Belltower at Bell"": 1
                        },
                        {
                            ""Sword"": 1,
                            ""Overworld Belltower at Bell"": 1
                        },
                        {
                            ""Techbow"": 1,
                            ""Overworld Belltower at Bell"": 1
                        }
                    ]
                }
            },
            {
                ""Reward"": {
                    ""Name"": ""East Bell"",
                    ""Type"": ""INVENTORY"",
                    ""Amount"": 1
                },
                ""Location"": {
                    ""LocationId"": ""tuning fork"",
                    ""SceneName"": ""Forest Belltower"",
                    ""SceneId"": 36,
                    ""Position"": ""(494.0, 62.0, 105.0)"",
                    ""Requirements"": [
                        {
                            ""Stick"": 1,
                            ""Forest Belltower Upper"": 1
                        },
                        {
                            ""Sword"": 1,
                            ""Forest Belltower Upper"": 1
                        },
                        {
                            ""Techbow"": 1,
                            ""Forest Belltower Upper"": 1
                        }
                    ]
                }
            },
        ]";
        public static Dictionary<string, string> BellLocationNames = new Dictionary<string, string>() {
            {"tuning fork [Forest Belltower]", "Forest Belltower - Ring the East Bell"},
            {"tuning fork [Overworld Redux]", "Overworld - [West] Ring the West Bell"}
        };
        public static Dictionary<string, Check> BellChecks = new Dictionary<string, Check>();
        public static GameObject BellPrefab;
        public static Dictionary<string, StateVariable> BellStateVariables;
        public static StateVariable EastBellStateVar;
        public static StateVariable WestBellStateVar;

        public static void Setup() {
            LoadBellChecks();
            CreateBellItems();
            CreateStateVars();
            InstantiateBellPrefab();
            ItemPresentationPatches.SetupBellPresentation();
        }

        public static void LoadBellChecks() {
            BellChecks.Clear();
            List<Check> checks = JsonConvert.DeserializeObject<List<Check>>(BellCheckJson);
            foreach (Check check in checks) { 
                BellChecks.Add(check.CheckId, check);
            }
            foreach (KeyValuePair<string, string> pair in BellLocationNames) { 
                Locations.LocationIdToDescription.Add(pair.Key, pair.Value);
                Locations.LocationDescriptionToId.Add(pair.Value, pair.Key);
            }
        }

        private static void CreateBellItems() {
            CreateBellItem("West Bell");
            CreateBellItem("East Bell");
        }

        private static void CreateBellItem(string itemName) {
            SpecialItem item = ScriptableObject.CreateInstance<SpecialItem>();
            item.name = itemName;
            item.collectionMessage = TunicUtils.CreateLanguageLine($"\"{itemName}\"");
            item.controlAction = "";
            item.icon = ModelSwaps.FindSprite("Randomizer items_bell");
            Inventory.itemList.Add(item);
        }


        public static void InstantiateBellPrefab() {

        
        }

        public static void CreateStateVars() {
            EastBellStateVar = ScriptableObject.CreateInstance<StateVariable>();
            EastBellStateVar.name = "randomizer Rung Bell 1 (East)";
            WestBellStateVar = ScriptableObject.CreateInstance<StateVariable>();
            WestBellStateVar.name = "randomizer Rung Bell 2 (West)";
            StateVariable.stateVariableList.Add(WestBellStateVar);
            StateVariable.stateVariableList.Add(EastBellStateVar);
            BellStateVariables = new Dictionary<string, StateVariable>() {
                {"East Bell", EastBellStateVar},
                {"West Bell", WestBellStateVar}
            };
        }

        public static void ModifyTempleDoor() {
            TempleDoor templeDoor = GameObject.FindObjectOfType<TempleDoor>();
            if (templeDoor != null) {
                templeDoor.bell_east = EastBellStateVar;
                templeDoor.bell_west = WestBellStateVar;

                foreach (StatefulActive active in templeDoor.GetComponentsInChildren<StatefulActive>(true)) {
                    if (active.name == "door statue indicator R" && active.GetComponent<VisibleByHavingInventoryItem>() == null) {
                        active.enabled = false;
                        active.gameObject.AddComponent<VisibleByHavingInventoryItem>().enablingItem = Inventory.GetItemByName("East Bell");
                        active.gameObject.GetComponent<VisibleByHavingInventoryItem>().renderers = new Renderer[] {
                            active.GetComponent<MeshRenderer>(),
                        };
                        active.gameObject.GetComponent<VisibleByHavingInventoryItem>().lights = new Light[] { };
                        active.gameObject.SetActive(true);
                    } else if (active.name == "door statue indicator L" && active.GetComponent<VisibleByHavingInventoryItem>() == null) {
                        active.enabled = false;
                        active.gameObject.AddComponent<VisibleByHavingInventoryItem>().enablingItem = Inventory.GetItemByName("West Bell"); 
                        active.gameObject.GetComponent<VisibleByHavingInventoryItem>().renderers = new Renderer[] {
                            active.GetComponent<MeshRenderer>(),
                        };
                        active.gameObject.GetComponent<VisibleByHavingInventoryItem>().lights = new Light[] { };
                        active.gameObject.SetActive(true);
                    }
                }

                GameObject sword1 = GameObject.Find("_Environment").transform.GetChild(3).Find("Sword").gameObject;
                if (sword1 != null && sword1.GetComponent<VisibleByNotHavingItem>() == null) { 
                    sword1.AddComponent<VisibleByNotHavingItem>().Item = Inventory.GetItemByName("West Bell");
                }
                GameObject sword2 = GameObject.Find("_Environment").transform.GetChild(3).Find("Sword (1)").gameObject;
                if (sword2 != null && sword2.GetComponent<VisibleByNotHavingItem>() == null) {
                    sword2.AddComponent<VisibleByNotHavingItem>().Item = Inventory.GetItemByName("East Bell");
                }

                foreach (TuningForkBell bell in Resources.FindObjectsOfTypeAll<TuningForkBell>().Where(bell => bell.name.Contains("Door,") && bell.GetComponent<VisibleByHavingInventoryItem>() == null)) {
                    bell.transform.parent = null;
                    bell.gameObject.SetActive(false);
                    if (bell.name == "tuning fork (Door, West)") {
                        bell.stateVar = StateVariable.GetStateVariableByName("true");
                        bell.gameObject.AddComponent<VisibleByHavingInventoryItem>().enablingItem = Inventory.GetItemByName("West Bell");
                    } else if (bell.name == "tuning fork (Door, East)") {
                        bell.stateVar = StateVariable.GetStateVariableByName("true");
                        bell.gameObject.AddComponent<VisibleByHavingInventoryItem>().enablingItem = Inventory.GetItemByName("East Bell");
                    }
                    bell.transform.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(false);
                    bell.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
                    bell.transform.GetChild(3).gameObject.SetActive(false);
                    GameObject aura = GameObject.Instantiate(bell.transform.GetChild(3).GetChild(0).gameObject);
                    aura.transform.parent = bell.transform;
                    aura.transform.localPosition = new Vector3(0, 5, 0);
                    aura.transform.localEulerAngles = new Vector3(180, 0, 0);
                    aura.transform.localScale = Vector3.one * 0.5f;
                    aura.SetActive(true);
                    GameObject.Destroy(bell.transform.GetChild(5).GetComponent<StudioEventEmitter>());
                    bell.gameObject.GetComponent<VisibleByHavingInventoryItem>().renderers = new Renderer[] {
                        bell.transform.GetChild(1).GetComponent<MeshRenderer>(),
                        bell.transform.GetChild(2).GetChild(0).GetComponent<MeshRenderer>(),
                        bell.transform.GetChild(2).GetComponent<MeshRenderer>(),
                        aura.GetComponent<MeshRenderer>(),
                    };
                    bell.gameObject.GetComponent<VisibleByHavingInventoryItem>().lights = new Light[] { };
                    bell.gameObject.SetActive(true);
                    bell.Start();
                }
            }
        }
        
        public static string GetBellCheckId(TuningForkBell bell) {
            return $"{bell.name} [{bell.gameObject.scene.name}]";
        }

        public static void TuningForkBell_onStateChange_PostfixPatch(TuningForkBell __instance) {
            string checkId = GetBellCheckId(__instance);

            if (Locations.RandomizedLocations.ContainsKey(checkId) || ItemLookup.ItemList.ContainsKey(checkId)) {
                if (!TunicUtils.IsCheckCompletedOrCollected(checkId)) { 
                    if (SaveFile.GetInt("archipelago") == 1 && !SaveFlags.IsArchipelago()) {
                        SaveFile.SetInt("randomizer picked up " + checkId, 1);
                    }
                    if (SaveFlags.IsArchipelago()) {
                        Archipelago.instance.ActivateCheck(Locations.LocationIdToDescription[checkId]);
                    } else if (SaveFlags.IsSinglePlayer()) {
                        Check check = Locations.RandomizedLocations[checkId];
                        ItemPatches.GiveItem(check);
                    }                
                }
            }            
        }
    }
}
