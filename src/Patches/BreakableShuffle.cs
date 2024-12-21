using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace TunicRandomizer {
    public class BreakableShuffle {

        public static Dictionary<string, Check> BreakableChecks = new Dictionary<string, Check>();
        public static Dictionary<string, int> BreakableChecksPerScene = new Dictionary<string, int>();
        public static Dictionary<string, string> BreakableCheckDescriptions = new Dictionary<string, string>();
        //public static List<string> APConvertedLines = new List<string>();

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
            List<int> moneyAmounts = new List<int> { 1, 2, 3, 4, 5 };  // change this later
            var assembly = Assembly.GetExecutingAssembly();
            var breakableJson = "TunicRandomizer.src.Data.Breakables.json";
            var breakableReqsJson = "TunicRandomizer.src.Data.BreakableReqs.json";
            var breakableDescriptionsJson = "TunicRandomizer.src.Data.BreakableDescriptions.json";
            List<string> breakers = new List<string>() { "Stick", "Sword", "Techbow", "Shotgun" };

            using (Stream stream = assembly.GetManifestResourceStream(breakableJson))
            using (StreamReader reader = new StreamReader(stream)) {
                Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> BreakableData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>>(reader.ReadToEnd());
                StreamReader extraReader = new StreamReader(assembly.GetManifestResourceStream(breakableReqsJson));
                Dictionary<string, List<List<string>>> extraBreakableReqs = JsonConvert.DeserializeObject<Dictionary<string, List<List<string>>>>(extraReader.ReadToEnd());
                StreamReader descriptionReader = new StreamReader(assembly.GetManifestResourceStream(breakableDescriptionsJson));
                BreakableCheckDescriptions = JsonConvert.DeserializeObject<Dictionary<string, string>>(descriptionReader.ReadToEnd());
                foreach (KeyValuePair<string, string> pair in BreakableCheckDescriptions) {
                    Locations.LocationIdToDescription.Add(pair.Key, pair.Value);
                    Locations.LocationDescriptionToId.Add(pair.Value, pair.Key);
                }
                foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, List<string>>>> sceneGroup in BreakableData) {
                    string sceneName = sceneGroup.Key;
                    foreach (KeyValuePair<string, Dictionary<string, List<string>>> regionGroup in sceneGroup.Value) {
                        foreach (KeyValuePair<string, List<string>> breakableGroup in regionGroup.Value) {
                            string breakableName = breakableGroup.Key;
                            foreach (string breakablePosition in breakableGroup.Value) {
                                string regionName = regionGroup.Key;  // assigned here so specific name overrides can happen
                                string breakableId = $"{breakableName}~{breakablePosition}";  // also used for checkId, and checkId has scene name in it already
                                Check check = new Check(new Reward(), new Location());
                                check.Reward.Name = "money";
                                check.Reward.Type = "MONEY";
                                check.Reward.Amount = 1;

                                check.Location.SceneName = sceneName;
                                check.Location.SceneId = 0;  // Update this if sceneid ever actually gets used for anything
                                check.Location.LocationId = breakableId;
                                check.Location.Position = breakablePosition;
                                check.Location.Requirements = new List<Dictionary<string, int>>();

                                // add each breaker and the region to the rules
                                foreach (string breaker in breakers) {
                                    // can't break the signs or the table with the stick
                                    if (breaker == "Stick" && (breakableName == "Physical Post" || breakableName == "table")) {
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

                                BreakableChecks.Add(check.CheckId, check);
                                if (!BreakableChecksPerScene.ContainsKey(check.Location.SceneName)) {
                                    BreakableChecksPerScene.Add(check.Location.SceneName, 0);
                                }
                                BreakableChecksPerScene[check.Location.SceneName]++;
                                //APConvertedLines.Add(ConvertToAPLine(description, regionGroup.Key, BreakableBetterNames[breakableName]));
                            }
                        }
                    }
                }
                extraReader.Close();
            }
            //string convertedBlock = "";
            //foreach (string APLine in APConvertedLines) {
            //    convertedBlock += APLine;
            //    convertedBlock += "\n";
            //}
            //TunicLogger.LogInfo(convertedBlock);
        }

        public static string ConvertToAPLine(string description, string region, string breakableName) {
            string converted = "    ";
            converted += "\"";
            converted += description;
            converted += "\": TunicLocationData(\"";
            converted += region;
            converted += "\", breakable=\"";
            converted += breakableName;
            converted += "\"),";

            return converted;
        }

        public static string getBreakableGameObjectId(GameObject gameObject, bool isLeafPile = false) {
            string name = TunicUtils.RemoveParenNumber(gameObject.name);
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
            
            return $"{name}~{position} [{scene}]";
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
                    ItemPatches.GiveItem(check, alwaysSkip: true);

                    GameObject fairyTarget = GameObject.Find($"fairy target {check.CheckId}");
                    if (fairyTarget != null) {
                        GameObject.Destroy(fairyTarget);
                    }
                }
                if (__instance.GetComponentInChildren<MoveUp>(true) != null) {
                    GameObject moveUp = __instance.GetComponentInChildren<MoveUp>(true).gameObject;
                    moveUp.transform.parent = __instance.transform.parent;
                    moveUp.transform.rotation = new Quaternion(moveUp.transform.rotation.x, 180f, moveUp.transform.rotation.z, moveUp.transform.rotation.w);
                    if (moveUp.name == "Card") {
                        moveUp.transform.rotation = new Quaternion(moveUp.transform.rotation.x, 0f, moveUp.transform.rotation.z, moveUp.transform.rotation.w);
                    }
                    moveUp.SetActive(true);
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
                    GameObject moveUp = __instance.GetComponentInChildren<MoveUp>(true).gameObject;
                    moveUp.transform.parent = __instance.transform.parent;
                    moveUp.transform.rotation = new Quaternion(moveUp.transform.rotation.x, 180f, moveUp.transform.rotation.z, moveUp.transform.rotation.w);
                    if (moveUp.name == "Card") {
                        moveUp.transform.rotation = new Quaternion(moveUp.transform.rotation.x, 0f, moveUp.transform.rotation.z, moveUp.transform.rotation.w);
                    }
                    moveUp.SetActive(true);
                }
            }
            return true;
        }

    }
}
