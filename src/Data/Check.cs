using System;
using System.Collections.Generic;

namespace TunicRandomizer {

    public struct Location {
        public string LocationId;
        public string Position;
        public List<Dictionary<string, int>> Requirements;
        public int SceneId;
        public string SceneName;

        public Location(Location location) {
            LocationId = location.LocationId;
            Position = location.Position;
            Requirements = new List<Dictionary<string, int>>();
            foreach (Dictionary<string, int> dictionary in location.Requirements) {
                Dictionary<string, int> d = new Dictionary<string, int>();
                TunicUtils.AddDictToDict(d, dictionary);
                Requirements.Add(d);
            }
            SceneId = location.SceneId;
            SceneName = location.SceneName;
        }

        public bool reachable(Dictionary<string, int> inventory) {
            List<Dictionary<string, int>> itemsRequired;
            
            itemsRequired = this.Requirements;

            //if there are no requirements, the location is reachable
            if (itemsRequired.Count == 0) {
                return true;
            }

            //if there are requirements, loop through each requirement to see if any are fully met
            foreach (Dictionary<string, int> req in itemsRequired) {
                //check if this requirement is fully met, otherwise move to the next requirement
                int met = 0;
                foreach (string item in req.Keys) {
                    //if (ItemRandomizer.testBool) {
                    //    TunicLogger.LogInfo("Item is " + item);
                    //}
                    // don't need to check if ability shuffle is on since the abilities are precollected if ability shuffle is off
                    if (!inventory.ContainsKey(item)) {
                        if (item == "Sword") {
                            if (inventory.ContainsKey("Sword Progression") && inventory["Sword Progression"] >= 2) {
                                met++;
                            }
                        } else if (item == "Stick") { 
                            if (inventory.ContainsKey("Sword Progression") && inventory["Sword Progression"] >= 1) {
                                met++;
                            }
                        } else if (item == "12") {
                            if (SaveFlags.IsHexQuestWithHexAbilities() && inventory.ContainsKey("Hexagon Gold")
                                    && inventory["Hexagon Gold"] >= SaveFile.GetInt(SaveFlags.HexagonQuestPrayer)) {
                                met++;
                            }
                        } else if (item == "21") {
                            if (SaveFlags.IsHexQuestWithHexAbilities() && inventory.ContainsKey("Hexagon Gold")
                                    && inventory["Hexagon Gold"] >= SaveFile.GetInt(SaveFlags.HexagonQuestHolyCross)) {
                                met++;
                            }
                        } else if (item == "26") {
                            if (SaveFlags.IsHexQuestWithHexAbilities() && inventory.ContainsKey("Hexagon Gold")
                                    && inventory["Hexagon Gold"] >= SaveFile.GetInt(SaveFlags.HexagonQuestIcebolt)) {
                                met++;
                            }
                        } else if (item.StartsWith("IG")) {
                            int difficulty = Convert.ToInt32(item.Substring(2, 2));
                            string range = item.Substring(3, 3);
                            bool met_difficulty = SaveFile.GetInt(SaveFlags.IceGrapplingDifficulty) >= difficulty;
                            if (met_difficulty && inventory.ContainsKey("Wand") && inventory.ContainsKey("Stundagger")) {
                                if (range == "S") {
                                    met++;
                                } else {
                                    if (inventory.ContainsKey("Techbow")
                                        && (inventory.ContainsKey("26")
                                            || (SaveFlags.IsHexQuestWithHexAbilities() && inventory.ContainsKey("Hexagon Gold") && inventory["Hexagon Gold"] >= SaveFile.GetInt(SaveFlags.HexagonQuestIcebolt)))) {
                                        met++;
                                    }
                                }
                            }
                        }
                    } else if (inventory[item] >= req[item]) {
                        met++;
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

        public Reward(Reward reward) {
            this.Amount = reward.Amount;
            this.Name = reward.Name;
            this.Type = reward.Type;
        }
    }
    public class Check {
        public Location Location;
        public Reward Reward;
        public string CheckId {
            get => $"{Location.LocationId} [{Location.SceneName}]";
        }
        public bool IsCompletedOrCollected {
            get {
                return SaveFile.GetInt("randomizer picked up " + CheckId) == 1 || (SaveFlags.IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld && SaveFile.GetInt($"randomizer {CheckId} was collected") == 1);
            }
        }
        public bool IsCollectedInAP {
            get {
                return SaveFlags.IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld && SaveFile.GetInt($"randomizer {CheckId} was collected") == 1;
            }
        }
        public Check() { }

        public Check(Check check) {
            Location = new Location(check.Location);
            Reward = new Reward(check.Reward);
        }

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
