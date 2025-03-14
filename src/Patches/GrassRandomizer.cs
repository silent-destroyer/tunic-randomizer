using Archipelago.MultiClient.Net.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
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
                        
                        check.Location.SceneName = ERData.RegionDict[pair.Key].Scene;
                        check.Location.SceneId = 0;  // Update this if sceneid ever actually gets used for anything
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
                        string description = "";
                        if (grass.Contains("bush")) {
                            description = $"{Locations.SimplifiedSceneNames[check.Location.SceneName]} - {pair.Key} {grass.Split('~')[0].Replace("bush", "Bush")} {check.Location.Position}";
                        } else {
                            description = $"{Locations.SimplifiedSceneNames[check.Location.SceneName]} - {pair.Key} {grass.Split('~')[0].Replace("grass", "Grass")} {check.Location.Position}";
                        }
                        Locations.LocationIdToDescription.Add(check.CheckId, description);
                        Locations.LocationDescriptionToId.Add(description, check.CheckId);
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
                    string grassId = getGrassGameObjectId(__instance.GetComponent<Grass>());
                    if (SaveFile.GetInt("archipelago") == 1 && !Archipelago.instance.IsConnected()) {
                        return false;
                    }
                    
                    if (SaveFlags.IsArchipelago() && ItemLookup.ItemList.ContainsKey(grassId) && !Locations.CheckedLocations[grassId]) {
                        ItemInfo ItemInfo = ItemLookup.ItemList[grassId];
                        bool isForTunicPlayer = Archipelago.instance.IsTunicPlayer(ItemInfo.Player);
                        if (isForTunicPlayer && ItemInfo.ItemName != "Grass") {
                            if (ItemInfo.ItemName == "Fool Trap") {
                                foreach (Transform child in __instance.GetComponentsInChildren<Transform>()) {
                                    if (child.name == __instance.name) { continue; }
                                    child.localEulerAngles = Vector3.zero;
                                    child.position -= new Vector3(0, 2, 0);
                                }
                            }
                        } 
                        if (isForTunicPlayer && ItemInfo.ItemName == "Grass") {
                            TunicLogger.LogInfo("Adding location id to queue " + ItemInfo.LocationId);
                            Archipelago.instance.integration.locationsToSend.Add(ItemInfo.LocationId);
                        } else {
                            Task.Run(() => Archipelago.instance.integration.session.Locations.CompleteLocationChecks(ItemInfo.LocationId));
                        }
                        SaveFile.SetInt("randomizer picked up " + grassId, 1);
                        Locations.CheckedLocations[grassId] = true;
                        TunicLogger.LogInfo("Cut Grass: " + grassId + " at location id " + ItemInfo.LocationId);
                        if (GameObject.Find($"fairy target {grassId}")) {
                            GameObject.Destroy(GameObject.Find($"fairy target {grassId}"));
                            FairyTargets.ChooseFairyTargetList();
                        }
                        string receiver = ItemInfo.Player.Name;
                        string itemName = ItemInfo.ItemName;
                        TunicLogger.LogInfo("Sent " + ItemInfo.ItemName + " at " + ItemInfo.LocationName + " to " + receiver);
                        if (ItemInfo.Player != Archipelago.instance.GetPlayerSlot() && (isForTunicPlayer ? ItemInfo.ItemName != "Grass" : true)) {
                            SaveFile.SetInt("archipelago items sent to other players", SaveFile.GetInt("archipelago items sent to other players") + 1);
                            Notifications.Show($"yoo sehnt  {(TextBuilderPatches.ItemNameToAbbreviation.ContainsKey(itemName) && isForTunicPlayer ? TextBuilderPatches.ItemNameToAbbreviation[itemName] : "[archipelago]")}  \"{itemName.Replace("_", " ")}\" too \"{receiver}!\"", $"hOp #A lIk it!");
                            RecentItemsDisplay.instance.EnqueueItem(ItemInfo, false);
                        }
                        if (__instance.transform.GetChild(1).childCount == 1) {
                            __instance.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                        }
                    } else if (SaveFlags.IsSinglePlayer() && Locations.RandomizedLocations.ContainsKey(grassId) && !Locations.CheckedLocations[grassId]) {
                        Check check = Locations.RandomizedLocations[grassId];
                        ItemData item = ItemLookup.GetItemDataFromCheck(Locations.RandomizedLocations[grassId]);
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
                            ItemPatches.GiveItem(check, alwaysSkip: true);
                        }
                        GameObject FairyTarget = GameObject.Find($"fairy target {check.CheckId}");
                        if (FairyTarget != null) {
                            GameObject.Destroy(FairyTarget);
                            FairyTargets.ChooseFairyTargetList();
                        }
                    }
                    if (__instance.GetComponentInChildren<MoveUp>(true) != null) {
                        __instance.GetComponentInChildren<MoveUp>(true).gameObject.SetActive(true);
                    }
                    __instance.GetComponent<Grass>().goToDeadState();
                    return false;
                }
            }
            return true;
        }

        public static bool PauseMenu___button_ReturnToTitle_PrefixPatch(PauseMenu __instance) {
            Profile.SavePermanentStatesByPosition(SceneManager.GetActiveScene().buildIndex, PermanentStateByPositionManager.deadPositionsInCurrentScene);
            return true;
        }

        public static string getGrassGameObjectId(Grass grass) {
            return $"{grass.name}~{grass.transform.position.ToString()} [{grass.gameObject.scene.name}]";
        }

        public static bool HitReceiver_ReceiveHit_PrefixPatch(HitReceiver __instance, ref HitType hitType, ref bool unblockable, ref bool isPlayerCharacterMelee) {

            // Prevent grass from being cut if AP connection is lost
            if (__instance.GetComponent<Grass>() != null && SaveFile.GetInt(SaveFlags.GrassRandoEnabled) == 1) {
                if (SaveFile.GetInt("archipelago") == 1 && !Archipelago.instance.IsConnected()) {
                    return false;
                }
            }

            return true;
        }
    }
}
