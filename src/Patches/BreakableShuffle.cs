using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace TunicRandomizer {
    public class BreakableShuffle {

        public static Dictionary<string, Check> BreakableChecks = new Dictionary<string, Check>();
        public static Dictionary<string, int> BreakableChecksPerScene = new Dictionary<string, int>();

        // keys are the auto-genned name, values are the nicer name (or the key if the key is already a nice name)
        public static Dictionary<string, string> BreakableBetterNames = new Dictionary<string, string>() {
            { "Urn", "Pot" },
            { "Urn Firey", "Fire Pot" },
            { "Urn Explosive", "Explosive Pot" },
            { "Physical Post", "Sign" },
            { "sewer_barrel", "Barrel" },
            { "crate", "Crate" },
            { "crate quarry", "Crate" },
            { "table", "Table" },
            { "library_lab_pageBottle", "Library Glass" },  // probably rename this one
            { "leaf pile", "Leaf Pile" },
        };

        public static void LoadBreakableChecks() {
            System.Random random = new System.Random(SaveFile.GetInt("seed"));
            List<int> moneyAmounts = new List<int> { 1, 2, 3, 4, 5 };  // change this later
            var assembly = Assembly.GetExecutingAssembly();
            var breakableJson = "TunicRandomizer.src.Data.Breakables.json";
            var breakableReqsJson = "TunicRandomizer.src.Data.BreakableReqs.json";
            List<string> breakers = new List<string>() { "Stick", "Sword", "Techbow", "Shotgun" };
            //List<string> breakers = new List<string>() { "Stick", "Sword" };

            using (Stream stream = assembly.GetManifestResourceStream(breakableJson))
            using (StreamReader reader = new StreamReader(stream)) {
                Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> BreakableData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>>(reader.ReadToEnd());
                StreamReader extraReader = new StreamReader(assembly.GetManifestResourceStream(breakableReqsJson));
                Dictionary<string, List<List<string>>> extraBreakableReqs = JsonConvert.DeserializeObject<Dictionary<string, List<List<string>>>>(extraReader.ReadToEnd());

                foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, List<string>>>> sceneGroup in BreakableData) {
                    string sceneName = sceneGroup.Key;
                    foreach (KeyValuePair<string, Dictionary<string, List<string>>> regionGroup in sceneGroup.Value) {
                        foreach (KeyValuePair<string, List<string>> breakableGroup in regionGroup.Value) {
                            string breakableName = breakableGroup.Key;
                            int breakableNumber = 1;
                            int customNumber = 1;  // for breakables that get a unique description
                            // for places that need more unique numbers, mostly fortress and frog's domain
                            int customNumber2 = 1; int customNumber3 = 1; int customNumber4 = 1; int customNumber5 = 1; int customNumber6 = 1;
                            foreach (string breakablePosition in breakableGroup.Value) {
                                string regionName = regionGroup.Key;  // assigned here so specific name overrides can happen
                                string breakableId = $"{sceneName}~{breakableName}~{breakablePosition}";  // also used for checkId, and checkId has scene name in it already
                                Check check = new Check(new Reward(), new Location());
                                check.Reward.Name = "money";
                                check.Reward.Type = "MONEY";
                                check.Reward.Amount = moneyAmounts[random.Next(moneyAmounts.Count)];

                                check.Location.SceneName = sceneName;
                                check.Location.SceneId = 0;  // Update this if sceneid ever actually gets used for anything
                                check.Location.LocationId = breakableId;
                                check.Location.Position = breakablePosition;
                                check.Location.Requirements = new List<Dictionary<string, int>>();

                                // add each breaker and the region to the rules
                                foreach (string breaker in breakers) {
                                    // can't break the signs with the stick
                                    if (breakableName == "Physical Post" && breaker == "Stick") {
                                        continue;
                                    }
                                    // we're just doing this separately in BreakableReqs cause it'll be a mess if done here
                                    if (breakableName == "leaf pile") {
                                        continue;
                                    }
                                    check.Location.Requirements.Add(new Dictionary<string, int> { { breaker, 1 }, { regionName, 1 } });
                                }
                                if (extraBreakableReqs.ContainsKey(breakableId)) {
                                    foreach (List<string> req in extraBreakableReqs[breakableId]) {
                                        Dictionary<string, int> reqDict = TunicUtils.ChangeListToDict(req);
                                        check.Location.Requirements.Add(reqDict);
                                    }
                                }
                                // giving more descriptive names for some areas where just the region isn't enough
                                string customDescription = null;
                                if (regionName == "Forest Belltower Upper" && breakableName == "Urn") {
                                    regionName = "Forest Belltower after Boss";
                                } else if (regionName == "East Forest" && breakablePosition.Contains("8.0,")) {
                                    customDescription = $"East Forest by Envoy - {BreakableBetterNames[breakableName]} {customNumber}";
                                    customNumber++;
                                } else if (regionName == "Beneath the Well Main") {
                                    if (breakableName == "Urn" || breakablePosition.StartsWith("(37")) {
                                        customDescription = $"Beneath the Well East - {BreakableBetterNames[breakableName]} {customNumber}";
                                        customNumber++;
                                    } else {
                                        regionName = "Beneath the Well West";
                                    }
                                } else if (regionName == "Dark Tomb Main") {
                                    if (breakablePosition.Contains("17.0,")) {
                                        customDescription = $"Dark Tomb Final Chamber - {BreakableBetterNames[breakableName]} {customNumber}";
                                        customNumber++;
                                    } else {
                                        regionName = "Dark Tomb Pot Hallway";
                                    }
                                } else if (regionName == "Fortress Grave Path" && breakablePosition.StartsWith("(17")) {
                                    customDescription = $"Fortress Grave Path by Grave - {BreakableBetterNames[breakableName]} {customNumber}";
                                    customNumber++;
                                } else if (regionName == "Eastern Vault Fortress") {
                                    if (breakablePosition.StartsWith("(3")) {
                                        customDescription = $"{regionName} by Broken Checkpoint - {BreakableBetterNames[breakableName]} {customNumber}";
                                        customNumber++;
                                    } else if (breakablePosition.StartsWith("(-22.5")) {
                                        customDescription = $"{regionName} by Checkpoint - {BreakableBetterNames[breakableName]} {customNumber2}";
                                        customNumber2++;
                                    } else if (breakablePosition.Contains(", 15.0, ")) {
                                        customDescription = $"{regionName} by Overlook - {BreakableBetterNames[breakableName]} {customNumber3}";
                                        customNumber3++;
                                    } else if (breakablePosition.StartsWith("(-4")) {
                                        customDescription = $"{regionName} Slorm Room - {BreakableBetterNames[breakableName]} {customNumber4}";
                                        customNumber4++;
                                    } else if (breakablePosition.EndsWith("58.9)")) {
                                        customDescription = $"{regionName} Chest Room - {BreakableBetterNames[breakableName]} {customNumber5}";
                                        customNumber5++;
                                    } else if (breakablePosition.StartsWith("(-5")) {
                                        customDescription = $"{regionName} by Stairs to Basement - {BreakableBetterNames[breakableName]} {customNumber6}";
                                        customNumber6++;
                                    } else {
                                        regionName = $"{regionName} by Door";
                                    }
                                } else if (regionName == "Ruined Atoll") {
                                    if (breakableName == "Urn Explosive") {
                                        customDescription = $"Atoll Near Birds - {BreakableBetterNames[breakableName]}";
                                    } else {
                                        regionName = "Atoll Southwest";
                                    }
                                } else if (regionName == "Frog's Domain") {
                                    if (breakableName == "Urn Explosive") {
                                        regionName = "Frog's Domain Orb Room";
                                    } else if (breakablePosition.EndsWith("-3.0)")) {
                                        customDescription = $"{regionName} above Orb Altar - {BreakableBetterNames[breakableName]} {customNumber}";
                                        customNumber++;
                                    } else if (breakablePosition.StartsWith("(-35")) {
                                        customDescription = $"{regionName} after Gate - {BreakableBetterNames[breakableName]} {customNumber2}";
                                        customNumber2++;
                                    } else if (breakablePosition.Contains(", 8")) {
                                        customDescription = $"{regionName} Main Room - {BreakableBetterNames[breakableName]} {customNumber3}";
                                        customNumber3++;
                                    } else if (breakablePosition == "(44.3, 0.0, -70.5") {
                                        customDescription = $"{regionName} after Slorm Room - {BreakableBetterNames[breakableName]}";
                                    } else {
                                        customDescription = $"{regionName} Side Room - {BreakableBetterNames[breakableName]} {customNumber4}";
                                        customNumber4++;
                                    }
                                } else if (regionName == "Quarry") {
                                    if (breakablePosition.EndsWith("1.4)")) {
                                        customDescription = $"{regionName} East - {BreakableBetterNames[breakableName]} {customNumber}";
                                        customNumber++;
                                    } else {
                                        customDescription = $"{regionName} East beneath Scaffolding - {BreakableBetterNames[breakableName]}";
                                    }
                                } else if (regionName == "Quarry Back") {
                                    if (breakableName == "crate quarry" || breakablePosition.StartsWith("(-2")) {
                                        customDescription = $"Quarry near Shortcut Ladder - {BreakableBetterNames[breakableName]} {customNumber}";
                                        customNumber++;
                                    }
                                } else if (regionName == "Lower Quarry") {
                                    if (breakablePosition.StartsWith("(-15") || breakablePosition.StartsWith("(-12") || breakablePosition.StartsWith("(-6")) {
                                        customDescription = $"{regionName} on Scaffolding - {BreakableBetterNames[breakableName]} {customNumber}";
                                        customNumber++;
                                    } else if (breakableName == "crate quarry") {
                                        regionName = "Lower Quarry Shooting Range";
                                    }
                                }
                                string description = $"{regionName} - {BreakableBetterNames[breakableName]} {breakableNumber}";
                                if (breakableGroup.Value.Count == 1) {
                                    description = $"{regionName} - {BreakableBetterNames[breakableName]}";
                                }
                                if (customDescription != null) {
                                    description = customDescription;
                                    breakableNumber--;
                                }
                                breakableNumber++;
                                Locations.LocationIdToDescription.Add(check.CheckId, description);
                                Locations.LocationDescriptionToId.Add(description, check.CheckId);
                                BreakableChecks.Add(check.CheckId, check);
                                if (!BreakableChecksPerScene.ContainsKey(check.Location.SceneName)) {
                                    BreakableChecksPerScene.Add(check.Location.SceneName, 0);
                                }
                                BreakableChecksPerScene[check.Location.SceneName]++;
                            }
                        }
                    }
                }
                extraReader.Close();
            }
        }

        public static string getBreakableGameObjectId(GameObject gameObject, bool isLeafPile = false) {
            string name = gameObject.name;
            name = name.Split('(')[0];
            if (name.EndsWith(" ")) {
                name = name.Remove(name.Length - 1);
            }
            string scene = gameObject.scene.name;
            string position;
            if (isLeafPile) {
                position = gameObject.transform.position.ToString();
            } else {
                position = gameObject.GetComponent<SmashableObject>().initialPosition.ToString();
                // if we're looking too early, initial position hasn't been set yet, but then the actual position is the one we want
                if (position == "(0.0, 0.0, 0.0)") {
                    position = gameObject.transform.position.ToString();
                }
            }
            
            return $"{scene}~{name}~{position} [{scene}]";
        }

        public static bool PermanentStateByPosition_onKilled_PrefixPatch(PermanentStateByPosition __instance) {
            SmashableObject smashComp = __instance.GetComponent<SmashableObject>();
            if (smashComp != null && SaveFile.GetInt(SaveFlags.BreakableShuffleEnabled) == 1) {
                if (SaveFile.GetInt("archipelago") == 1 && !Archipelago.instance.IsConnected()) {
                    return false;
                }
                string breakableId = getBreakableGameObjectId(__instance.gameObject);
                if (SaveFlags.IsSinglePlayer() && Locations.RandomizedLocations.ContainsKey(breakableId) && !Locations.CheckedLocations[breakableId]) {
                    Check check = Locations.RandomizedLocations[breakableId];

                    // grass rando messes with it if it's a fool trap, figure out what it is and do that too here if we should
                    ItemPatches.GiveItem(check, alwaysSkip: true);

                    GameObject fairyTarget = GameObject.Find($"fairy target {check.CheckId}");
                    if (fairyTarget != null) {
                        GameObject.Destroy(fairyTarget);
                    }
                }
                if (__instance.GetComponentInChildren<MoveUp>(true) != null) {
                    __instance.GetComponentInChildren<MoveUp>(true).gameObject.SetActive(true);
                }
                return false;
            }
            return true;
        }


        public static bool DustyPile_scatter_PrefixPatch(DustyPile __instance) {
            if (SaveFile.GetInt(SaveFlags.BreakableShuffleEnabled) == 1) {
                if (SaveFile.GetInt("archipelago") == 1 && !Archipelago.instance.IsConnected()) {
                    return false;
                }
                string breakableId = getBreakableGameObjectId(__instance.gameObject, isLeafPile: true);

                if (SaveFlags.IsSinglePlayer() && Locations.RandomizedLocations.ContainsKey(breakableId) && !Locations.CheckedLocations[breakableId]) {
                    Check check = Locations.RandomizedLocations[breakableId];

                    // grass rando messes with it if it's a fool trap, figure out what it is and do that too here if we should
                    ItemPatches.GiveItem(check, alwaysSkip: true);

                    GameObject fairyTarget = GameObject.Find($"fairy target {check.CheckId}");
                    if (fairyTarget != null) {
                        GameObject.Destroy(fairyTarget);
                    }
                }
                if (__instance.GetComponentInChildren<MoveUp>(true) != null) {
                    __instance.GetComponentInChildren<MoveUp>(true).gameObject.SetActive(true);
                }
            }
            return true;
        }

    }
}
