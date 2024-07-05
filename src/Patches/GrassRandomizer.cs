using System.Collections.Generic;

namespace TunicRandomizer {
    public class GrassRandomizer {

        public static bool PermanentStateByPosition_onKilled_PrefixPatch(PermanentStateByPosition __instance) {
            if (__instance.GetComponent<Grass>() != null) {
                TunicLogger.LogInfo("Grass destroyed: " + __instance.transform.position + " " + __instance.name);
                if (!GrassInfo.GrassInRegion.ContainsKey(GrassInfo.GrassRegion)) {
                    GrassInfo.GrassInRegion[GrassInfo.GrassRegion] = new List<string>();
                }

                if (!GrassInfo.GrassInRegion[GrassInfo.GrassRegion].Contains(__instance.transform.position.ToString())) {
                    Inventory.GetItemByName("Grass").Quantity += 1;
                    GrassInfo.GrassInRegion[GrassInfo.GrassRegion].Add(__instance.name + "~" + __instance.transform.position.ToString());
                    SceneLoaderPatches.GrassInArea--;
                }
/*                Check check = new Check();
                Location location = new Location();
                location.LocationId = $"grass {__instance.transform.position.ToString()}";
                location.SceneId = __instance.gameObject.scene.buildIndex;
                location.SceneName = __instance.gameObject.scene.name;
                location.Position = __instance.gameObject.transform.position.ToString();
                Reward reward = new Reward();
                reward.Amount = 1;
                reward.Type = "GRASS";
                reward.Name = __instance.name.ToLower().Contains("bush") ? "Bush" : "Grass";

                List<Dictionary<string, int>> reqs = new List<Dictionary<string, int>>();
                Dictionary<string, int> Sword = new Dictionary<string, int>() {
                    { "Sword", 1 }
                };
                Dictionary<string, int> Techbow = new Dictionary<string, int>() {
                    { "Techbow", 1 }
                };
                if (GrassInfo.Laurels) {
                    Sword.Add("Hyperdash", 1);
                    Techbow.Add("Hyperdash", 1);
                }
                if (GrassInfo.Grapple) {
                    Sword.Add("Wand", 1);
                    Techbow.Add("Wand", 1);
                }
                if (GrassInfo.Mask) {
                    Sword.Add("Mask", 1);
                    Techbow.Add("Mask", 1);
                }
                if (GrassInfo.Lantern) {
                    Sword.Add("Lantern", 1);
                    Techbow.Add("Lantern", 1);
                }
                if (GrassInfo.Prayer) {
                    Sword.Add("12", 1);
                    Techbow.Add("12", 1);
                }
                if (GrassInfo.HolyCross) {
                    Sword.Add("21", 1);
                    Techbow.Add("21", 1);
                }
                reqs.Add(Sword);
                reqs.Add(Techbow);
                //location.RequiredItems = reqs;

                check.Reward = reward;
                check.Location = location;
                if (!GrassInfo.GrassNames.Contains(location.LocationId)) {
                    GrassInfo.GrassNames.Add(location.LocationId);
                    GrassInfo.GrassChecks.Add(check);
                    //SceneLoaderPatches.GrassInArea--;
                    Inventory.GetItemByName("Grass").Quantity += 1;
                }*/
            }
            return true;
        }
    }
}
