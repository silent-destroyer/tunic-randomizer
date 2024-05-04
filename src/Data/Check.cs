using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunicRandomizer {

    public struct Location {
        public string LocationId;
        public string Position;
        public List<Dictionary<string, int>> RequiredItems;
        public List<Dictionary<string, int>> RequiredItemsDoors;
        public int SceneId;
        public string SceneName;

        public bool reachable(Dictionary<string, int> inventory) {
            List<Dictionary<string, int>> itemsRequired;
            if (SaveFile.GetInt("randomizer entrance rando enabled") == 1) {
                itemsRequired = this.RequiredItemsDoors;
            } else {
                itemsRequired = this.RequiredItems;
            }

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
                    //Logger.LogInfo(item);
                    if (!inventory.ContainsKey(item)) {
                        if (ItemRandomizer.testBool) {
                            TunicRandomizer.Logger.LogInfo("LocationID is " + this.LocationId);
                            TunicRandomizer.Logger.LogInfo("inventory does not contain " + item);
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
                TunicRandomizer.Logger.LogInfo("No requirements met for " + this.LocationId + ", returning false");
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
