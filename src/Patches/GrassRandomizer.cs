using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TunicRandomizer {
    public class GrassRandomizer {

        public static Dictionary<string, Check> GrassChecks = new Dictionary<string, Check>();

        public static void LoadGrassJson() {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "TunicRandomizer.src.Data.Grass.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream)) {
                Dictionary<string, List<string>> grassRegions = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(reader.ReadToEnd());

                foreach(KeyValuePair<string, List<string>> pair in grassRegions) {
                    foreach(string grass in pair.Value) {
                        Check check = new Check(new Reward(), new Location());
                        check.Reward.Name = "Grass";
                        check.Reward.Type = "GRASS";
                        check.Reward.Amount = 1;
                        
                        check.Location.SceneName = TunicPortals.RegionDict.ContainsKey(pair.Key) ? TunicPortals.RegionDict[pair.Key].Scene : "Atoll Redux";
                        check.Location.LocationId = grass;
                        if (pair.Key == "Ruined Atoll Kevin") {
                            check.Location.Requirements = new List<Dictionary<string, int>>() {
                                new Dictionary<string, int>() {
                                    {"Ruined Atoll", 1},
                                    {"Hyperdash", 1},
                                    {"Sword", 1}
                                },
                                new Dictionary<string, int>() {
                                    {"Ruined Atoll", 1},
                                    {"Hyperdash", 1},
                                    {"Techbow", 1 }
                                }
                            };
                            check.Location.SceneId = 0;
                        } else if (pair.Key == "Ruined Atoll Frog Fuse") {
                            check.Location.Requirements = new List<Dictionary<string, int>>() {
                                new Dictionary<string, int>() {
                                    {"Ruined Atoll", 1},
                                    {"Hyperdash", 1},
                                    {"Sword", 1},
                                },
                                new Dictionary<string, int>() {
                                    {"Ruined Atoll", 1},
                                    {"Wand", 1},
                                    {"Sword", 1},
                                },
                                new Dictionary<string, int>() {
                                    {"Ruined Atoll", 1},
                                    {"Hyperdash", 1},
                                    {"Techbow", 1},
                                },
                                new Dictionary<string, int>() {
                                    {"Ruined Atoll", 1},
                                    {"Wand", 1},
                                    {"Techbow", 1},
                                },
                            };
                            check.Location.SceneId = 0;
                        } else if (pair.Key == "Frog Stairs Lower") {
                            check.Location.Requirements = new List<Dictionary<string, int>>() {
                                new Dictionary<string, int>() {
                                    {"Frog Stairs Lower", 1},
                                    {"Wand", 1},
                                    {"Sword", 1},
                                },
                                new Dictionary<string, int>() {
                                    {"Frog Stairs Lower", 1},
                                    {"Wand", 1},
                                    {"Techbow", 1},
                                },
                                new Dictionary<string, int>() {
                                    {"Frog Stairs Lower", 1},
                                    {"Ladders to Frog's Domain", 1},
                                    {"Sword", 1},
                                },
                                new Dictionary<string, int>() {
                                    {"Frog Stairs Lower", 1},
                                    {"Ladders to Frog's Domain", 1},
                                    {"Techbow", 1},
                                },
                            };
                            check.Location.SceneId = 0;
                        } else {
                            check.Location.Requirements = new List<Dictionary<string, int>>() {
                                new Dictionary<string, int>() {
                                    {pair.Key, 1},
                                    {"Sword", 1}
                                },
                                new Dictionary<string, int>() {
                                    {pair.Key, 1},
                                    {"Techbow", 1}
                                }
                            };
                            check.Location.SceneId = 0;
                        }
                        check.Location.Position = grass.Split('~')[1];
                        GrassChecks.Add(check.CheckId, check);
                        Locations.LocationIdToDescription.Add(check.CheckId, check.CheckId);
                        Locations.LocationDescriptionToId.Add(check.CheckId, check.CheckId); 
                    }
                }
            }
        }

        public static bool PermanentStateByPosition_onKilled_PrefixPatch(PermanentStateByPosition __instance) {
            if (__instance.GetComponent<Grass>() != null) {
                TunicLogger.LogInfo("Grass destroyed: " + __instance.transform.position + " " + __instance.name);
                /*                if (!GrassInfo.GrassInRegion.ContainsKey(GrassInfo.GrassRegion)) {
                                    GrassInfo.GrassInRegion[GrassInfo.GrassRegion] = new List<string>();
                                }

                                if (!GrassInfo.GrassInRegion[GrassInfo.GrassRegion].Contains(__instance.transform.position.ToString())) {
                                    Inventory.GetItemByName("Grass").Quantity += 1;
                                    GrassInfo.GrassInRegion[GrassInfo.GrassRegion].Add(__instance.name + "~" + __instance.transform.position.ToString());
                                    SceneLoaderPatches.GrassInArea--;
                                }*/
                string grassId = $"{__instance.name}~{__instance.transform.position.ToString()} [{__instance.gameObject.scene.name}]";
                if (Locations.RandomizedLocations.ContainsKey(grassId) && !Locations.CheckedLocations[grassId]) {
                    Check check = Locations.RandomizedLocations[grassId];
                    ItemData item = ItemLookup.GetItemDataFromCheck(Locations.RandomizedLocations[grassId]);
                    TunicLogger.LogInfo(item.Name);
                    if (item.Name == "Grass") {
                        Inventory.GetItemByName("Grass").Quantity += 1;
                        Locations.CheckedLocations[grassId] = true;
                    } else {
                        ItemPatches.GiveItem(check);
                    }
                }
            }
            return true;
        }
    }
}
