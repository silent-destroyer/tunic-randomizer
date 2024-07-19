using Archipelago.MultiClient.Net.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TunicRandomizer {
    public class GrassRandomizer {

        public static Dictionary<string, Check> GrassChecks = new Dictionary<string, Check>();
        public static Dictionary<string, int> GrassChecksPerScene = new Dictionary<string, int>() {
            {"Trinket Well", 0},
        };
        public static List<string> ExcludedGrassChecks = new List<string>() {
            "bush (7)~(-39.0, 40.0, -41.0)",
            "bush (2)~(-41.0, 40.0, -41.0)",
            "bush (16)~(53.0, 12.0, -151.0)",
            "bush (9)~(-19.0, 28.0, -103.0)",
            "bush (23)~(-19.0, 28.0, -105.0)",
            "bush (26)~(-19.0, 28.0, -107.0)",
            "bush (47)~(91.0, 12.0, -155.0)",
            "bush (42)~(91.0, 12.0, -157.0)",
            "bush (58)~(58.0, 44.0, -109.0)",
            "bush (62)~(66.5, 44.0, -111.0)",
            "bush (64)~(56.0, 44.0, -107.0)",
        };
        public static void LoadGrassChecks() {
            var assembly = Assembly.GetExecutingAssembly();
            var grassJson = "TunicRandomizer.src.Data.Grass.json";
            var grassReqsJson = "TunicRandomizer.src.Data.GrassReqs.json";
            List<List<string>> grassCutters = new List<List<string>>() {
                new List<string>() {"Sword"},
                new List<string>() {"Stick", "Trinket - Glass Cannon"},
            };
            using (Stream stream = assembly.GetManifestResourceStream(grassJson))
            using (StreamReader reader = new StreamReader(stream)) {
                Dictionary<string, List<string>> grassRegions = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(reader.ReadToEnd());
                StreamReader extraReader = new StreamReader(assembly.GetManifestResourceStream(grassReqsJson));
                Dictionary<string, List<List<string>>> extraGrassReqs = JsonConvert.DeserializeObject<Dictionary<string, List<List<string>>>>(extraReader.ReadToEnd());
                foreach(KeyValuePair<string, List<string>> pair in grassRegions) {
                    foreach(string grass in pair.Value) {
                        Check check = new Check(new Reward(), new Location());
                        check.Reward.Name = "Grass";
                        check.Reward.Type = "Grass";
                        check.Reward.Amount = 1;
                        
                        check.Location.SceneName = TunicPortals.RegionDict[pair.Key].Scene;
                        check.Location.SceneId = 0; // Update this if sceneid ever actually gets used for anything
                        check.Location.LocationId = grass;
                        check.Location.Position = grass.Split('~')[1];
                        check.Location.Requirements = new List<Dictionary<string, int>>();
                        if (grass == "grass (1)~(72.0, 8.0, -29.0)") {
                            // Special case for long distance grass in fortress courtyard
                            check.Location.Requirements.Add(new Dictionary<string, int>() {
                                {pair.Key, 1},
                                {"Techbow", 1},
                            });
                        } else if (extraGrassReqs.ContainsKey(grass)) {
                            foreach (List<string> extraReqs in extraGrassReqs[extraGrassReqs.ContainsKey(grass) ? grass : pair.Key]) {
                                foreach (List<string> items in grassCutters) {
                                    Dictionary<string, int> reqs = new Dictionary<string, int> { };
                                    foreach (string item in items) {
                                        reqs.Add(item, 1);
                                    }
                                    foreach (string item in extraReqs) {
                                        reqs.Add(item, 1);
                                    }
                                    check.Location.Requirements.Add(reqs);
                                }
                            }
                        } else {
                            foreach (List<string> items in grassCutters) {
                                Dictionary<string, int> reqs = new Dictionary<string, int> {
                                    { pair.Key, 1 }
                                };
                                foreach (string item in items) {
                                    reqs.Add(item, 1);
                                }
                                check.Location.Requirements.Add(reqs);
                            }
                        }

                        GrassChecks.Add(check.CheckId, check);
                        Locations.LocationIdToDescription.Add(check.CheckId, check.CheckId);
                        Locations.LocationDescriptionToId.Add(check.CheckId, check.CheckId);
                        if (!GrassChecksPerScene.ContainsKey(check.Location.SceneName)) {
                            GrassChecksPerScene.Add(check.Location.SceneName, 0);
                        }
                        GrassChecksPerScene[check.Location.SceneName]++;
                    }
                }
                extraReader.Close();
            }
        }

        public static bool PermanentStateByPosition_onKilled_PrefixPatch(PermanentStateByPosition __instance) {
            if (__instance.GetComponent<Grass>() != null) {
                if (SaveFile.GetInt(SaveFlags.GrassRandoEnabled) == 1) {
                    string grassId = $"{__instance.name}~{__instance.transform.position.ToString()} [{__instance.gameObject.scene.name}]";
                        
                    if (SaveFlags.IsArchipelago() && ItemLookup.ItemList.ContainsKey(grassId) && !Locations.CheckedLocations[grassId]) {
                        ItemInfo ItemInfo = ItemLookup.ItemList[grassId];
                        SaveFile.SetInt("randomizer grass cut " + __instance.gameObject.scene.name, SaveFile.GetInt("randomizer grass cut " + __instance.gameObject.scene.name) + 1);
                        SaveFile.SetInt("randomizer total grass cut", SaveFile.GetInt("randomizer total grass cut") + 1);
                        if (Archipelago.instance.IsTunicPlayer(ItemInfo.Player)) {
                            ItemData item = ItemLookup.Items[ItemInfo.ItemName];
                            if (item.Type != ItemTypes.GRASS) {
                                Archipelago.instance.ActivateCheck(grassId);
                                if (item.Name == "Fool Trap") {
                                    foreach (Transform child in __instance.GetComponentsInChildren<Transform>()) {
                                        if (child.name == __instance.name) { continue; }
                                        child.localEulerAngles = Vector3.zero;
                                        child.position -= new Vector3(0, 2, 0);
                                    }
                                }
                            } else {
                                Archipelago.instance.CompleteLocationCheck(grassId);
                                SaveFile.SetInt("randomizer picked up " + grassId, 1);
                                Locations.CheckedLocations[grassId] = true;
                                if (GameObject.Find($"fairy target {grassId}")) {
                                    GameObject.Destroy(GameObject.Find($"fairy target {grassId}"));
                                }
                            }
                        } else {
                            Archipelago.instance.ActivateCheck(grassId);
                        }
                        if (__instance.transform.GetChild(1).childCount == 1) {
                            __instance.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                        }
                    } else if (SaveFlags.IsSinglePlayer() && Locations.RandomizedLocations.ContainsKey(grassId) && !Locations.CheckedLocations[grassId]) {
                        Check check = Locations.RandomizedLocations[grassId];
                        ItemData item = ItemLookup.GetItemDataFromCheck(Locations.RandomizedLocations[grassId]);
                        SaveFile.SetInt("randomizer grass cut " + __instance.gameObject.scene.name, SaveFile.GetInt("randomizer grass cut " + __instance.gameObject.scene.name) + 1);

                        SaveFile.SetInt("randomizer total grass cut", SaveFile.GetInt("randomizer total grass cut") + 1);
                        if (item.Name == "Grass") {
                            Inventory.GetItemByName("Grass").Quantity += 1;
                            Locations.CheckedLocations[grassId] = true;
                            SaveFile.SetInt("randomizer picked up " + check.CheckId, 1);
                            TunicRandomizer.Tracker.SetCollectedItem("Grass", false);
                        } else {
                            if (item.Name == "Fool Trap") {
                                foreach (Transform child in __instance.GetComponentsInChildren<Transform>()) {
                                    if (child.name == __instance.name) { continue; }
                                    child.localEulerAngles = Vector3.zero;
                                    child.position -= new Vector3(0, 2, 0);
                                }
                            }
                            ItemPatches.GiveItem(check, isGrassCheck: true);
                        }
                        GameObject FairyTarget = GameObject.Find($"fairy target {check.CheckId}");
                        if (FairyTarget != null) {
                            GameObject.Destroy(FairyTarget);
                        }
                    }
                }
                if (__instance.GetComponentInChildren<MoveUp>(true) != null) {
                    __instance.GetComponentInChildren<MoveUp>(true).gameObject.SetActive(true);
                }
            }
            return true;
        }

        public static bool PauseMenu___button_ReturnToTitle_PrefixPatch(PauseMenu __instance) {
            Profile.SavePermanentStatesByPosition(SceneManager.GetActiveScene().buildIndex, PermanentStateByPositionManager.deadPositionsInCurrentScene);
            return true;
        }
    }
}
