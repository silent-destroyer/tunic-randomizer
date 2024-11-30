using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

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
            { "Leaf Pile", "Leaf Pile" },  // actually go find this one
        };

        public static void LoadBreakableChecks() {
            System.Random random = new System.Random(SaveFile.GetInt("seed"));
            List<int> moneyAmounts = new List<int> { 1, 1, 1, 5, 5, 10 };
            var assembly = Assembly.GetExecutingAssembly();
            var breakableJson = "TunicRandomizer.src.Data.Breakables.json";
            var breakableReqsJson = "TunicRandomizer.src.Data.BreakableReqs.json";
            //List<string> breakers = new List<string>() { "Stick", "Sword", "Techbow", "Gun" };
            List<string> breakers = new List<string>() { "Stick", "Sword" };

            // todo: make reqs json, with assumption that it'll auto-generate combinations of breakers and the region it is already in
            // account for shooting across regions, especially to fire pots
            // todo: finish copying from GrassRandomizer.cs

            using (Stream stream = assembly.GetManifestResourceStream(breakableJson))
            using (StreamReader reader = new StreamReader(stream)) {
                Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> BreakableData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>>(reader.ReadToEnd());
                StreamReader extraReader = new StreamReader(assembly.GetManifestResourceStream(breakableReqsJson));
                Dictionary<string, List<List<string>>> extraBreakableReqs = JsonConvert.DeserializeObject<Dictionary<string, List<List<string>>>>(extraReader.ReadToEnd());

                foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, List<string>>>> sceneGroup in BreakableData) {
                    string sceneName = sceneGroup.Key;
                    foreach (KeyValuePair<string, Dictionary<string, List<string>>> regionGroup in sceneGroup.Value) {
                        string regionName = regionGroup.Key;
                        foreach (KeyValuePair<string, List<string>> breakableGroup in regionGroup.Value) {
                            string breakableName = breakableGroup.Key;
                            foreach (string breakablePosition in breakableGroup.Value) {
                                string breakableFullName = $"{regionName}~{breakableName}~{breakablePosition}";  // also used for checkId, and checkId has scene name in it already
                                Check check = new Check(new Reward(), new Location());
                                check.Reward.Name = "money";
                                check.Reward.Type = "MONEY";
                                check.Reward.Amount = moneyAmounts[random.Next(moneyAmounts.Count)];  // todo: maybe change later

                                check.Location.SceneName = sceneName;
                                check.Location.SceneId = 0;  // Update this if sceneid ever actually gets used for anything
                                check.Location.LocationId = breakableFullName;
                                check.Location.Position = breakablePosition;
                                check.Location.Requirements = new List<Dictionary<string, int>>();

                                // add each breaker and the region to the rules
                                foreach (string breaker in breakers) {
                                    check.Location.Requirements.Add(new Dictionary<string, int> { { breaker, 1 }, { regionName, 1 } });
                                }
                                if (extraBreakableReqs.ContainsKey(breakableFullName)) {
                                    foreach (List<string> req in extraBreakableReqs[breakableFullName]) {
                                        Dictionary<string, int> reqDict = new Dictionary<string, int>();
                                        TunicUtils.AddListToDict(reqDict, req);
                                        check.Location.Requirements.Add(reqDict);
                                    }
                                }
                                // todo: consider having actual location names instead of just coordinates
                                string description = $"{Locations.SimplifiedSceneNames[sceneName]} - {BreakableBetterNames[breakableName]} ({breakablePosition})";
                                Locations.LocationIdToDescription.Add(check.CheckId, description);
                                Locations.LocationDescriptionToId.Add(description, check.CheckId);
                            }
                        }
                    }
                }
                extraReader.Close();
            }
            
        }
    }
}
