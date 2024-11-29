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
    public class BreakableShuffle {

        public static Dictionary<string, Check> BreakableChecks = new Dictionary<string, Check>();
        public static Dictionary<string, int> BreakableChecksPerScene = new Dictionary<string, int>();

        public static void LoadBreakableChecks() {
            var assembly = Assembly.GetExecutingAssembly();
            var breakableJson = "TunicRandomizer.src.Data.Breakables.json";
            //List<string> breakers = new List<string>() { "Stick", "Sword", "Techbow", "Gun" };
            List<string> breakers = new List<string>() { "Stick", "Sword" };

            // todo: make reqs json, with assumption that it'll auto-generate combinations of breakers and the region it is already in
            // account for shooting across regions, especially to fire pots
            // todo: finish copying from GrassRandomizer.cs

            using (Stream stream = assembly.GetManifestResourceStream(breakableJson))
            using (StreamReader reader = new StreamReader(stream)) {

                Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> BreakableData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>>(reader.ReadToEnd());
             
                foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, List<string>>>> sceneGroup in BreakableData) {
                    string sceneName = sceneGroup.Key;
                    foreach (KeyValuePair<string, Dictionary<string, List<string>>> regionGroup in sceneGroup.Value) {
                        string regionName = regionGroup.Key;
                        foreach (KeyValuePair<string, List<string>> breakableGroup in regionGroup.Value) {
                            string breakableName = breakableGroup.Key;
                            foreach (string breakablePosition in breakableGroup.Value) {
                                breakableName = $"{sceneName}~{regionName}~{breakableName}~{breakablePosition}";
                                Check check = new Check(new Reward(), new Location());
                                check.Reward.Name = "Small Money";  // change later
                                check.Reward.Type = "idk";
                                check.Reward.Amount = 1;

                                check.Location.SceneName = sceneName;
                                check.Location.SceneId = 0;  // Update this if sceneid ever actually gets used for anything
                                check.Location.LocationId = breakableName;
                                check.Location.Position = breakablePosition;
                                check.Location.Requirements = new List<Dictionary<string, int>>();
                                // check extra reqs

                                // add a breaker and the region to the rules
                                foreach (string breaker in breakers) {
                                    check.Location.Requirements.Add(new Dictionary<string, int> { { breaker, 1 }, { regionName, 1 } });
                                }
                            }
                        }
                    }
                }

            }

            
        }
    }
}
