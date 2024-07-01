using System.Collections.Generic;

namespace TunicRandomizer {

    public struct Location {
        public string LocationId;
        public string Position;
        public List<Dictionary<string, int>> Requirements;
        public int SceneId;
        public string SceneName;

        public bool reachable(Dictionary<string, int> inventory) {
            List<Dictionary<string, int>> itemsRequired;
            
            itemsRequired = this.Requirements;

            //if there are no requirements, the location is reachable
            if (itemsRequired.Count == 0) {
                return true;
            }

            //if there are requirements, loop through each requirement to see if any are fully met
            foreach (Dictionary<string, int> req in itemsRequired) {
                //ensure req and items use same terms
                if (SaveFile.GetInt("randomizer sword progression enabled") != 0) {
                    if (req.ContainsKey("Stick")) {
                        req["Sword Progression"] = 1;
                        req.Remove("Stick");
                    }
                    if (req.ContainsKey("Sword")) {
                        req["Sword Progression"] = 2;
                        req.Remove("Sword");
                    }
                }

                //check if this requirement is fully met, otherwise move to the next requirement
                int met = 0;
                foreach (string item in req.Keys) {
                    //TunicLogger.LogInfo(item);
                    if (!inventory.ContainsKey(item)) {
                        if (ItemRandomizer.testBool) {
                            TunicLogger.LogInfo("LocationID is " + this.LocationId);
                            TunicLogger.LogInfo("inventory does not contain " + item);
                        }
                        break;
                    } else if (inventory[item] >= req[item]) {
                        met += 1;
                    }
                }
                if (met == req.Count) {
                    return true;
                }
            }
            //if no requirements are met, the location isn't reachable
            if (ItemRandomizer.testBool) {
                TunicLogger.LogInfo("No requirements met for " + this.LocationId + ", returning false");
            }
            return false;
        }
    }
    public struct Reward {
        public int Amount;
        public string Name;
        public string Type;
    }
    public class Check {
        public Location Location;
        public Reward Reward;
        public string CheckId {
            get => $"{Location.LocationId} [{Location.SceneName}]";
        }

        public Check() { }

        public Check(Location location, Reward reward) {
            Location = location;
            Reward = reward;
        }
        public Check(Reward reward, Location location) {
            Location = location;
            Reward = reward;
        }
    }
}
