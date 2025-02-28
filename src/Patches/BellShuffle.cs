using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                {"Rung Bell 1 (East)", EastBellStateVar},
                {"Rung Bell 2 (West)", WestBellStateVar},
                {"East Bell", StateVariable.GetStateVariableByName("Rung Bell 1 (East)")},
                {"West Bell", StateVariable.GetStateVariableByName("Rung Bell 2 (West)")}
            };
        }

        public static void ModifyBells() {
            foreach (TuningForkBell bell in GameObject.FindObjectsOfType<TuningForkBell>()) {
                // "Rung Bell 1 (East)"
                // "Rung Bell 2 (West)"
                string checkId = $"{bell.name} [{bell.gameObject.scene.name}]";
                if (BellChecks.ContainsKey(checkId)) {
                    if (bell.gameObject.scene.name == "Forest Belltower") {
                        bell.stateVar = EastBellStateVar;
                    } else if (bell.gameObject.scene.name == "Overworld Redux") {
                        bell.stateVar = WestBellStateVar;
                    }
                    bell.Start();
                }
            }
        }
        
        public static void TuningForkBell_onStateChange_PostfixPatch(TuningForkBell __instance) {
            TunicLogger.LogInfo("bell rung");
            string checkId = $"tuning fork [{SceneManager.GetActiveScene().name}]";

            if (Locations.RandomizedLocations.ContainsKey(checkId) || ItemLookup.ItemList.ContainsKey(checkId)) {
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
            
            MoveUp moveUp = __instance.GetComponentInChildren<MoveUp>();
            if (moveUp != null) {
                moveUp.gameObject.SetActive(true);
            }
            SaveFile.SetInt("randomizer " + __instance.stateVar.name, 1);
        }
    }
}
