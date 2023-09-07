namespace TunicRandomizer {
    public class Portal {
        public string Scene {
            get;
            set;
        }
        public string Destination {
            get;
            set;
        }
        public string Tag {
            get;
            set;
        }
        public string Name {
            get;
            set;
        }
        public List<Dictionary<string, int>> RequiredItems {
            get;
            set;
        }

        public Portal(string scene, string destination, string tag, string name) {
            Scene = scene
            Destination = destination;
            Tag = tag;
            Name = name;
        }

        public Portal(string scene, string destination, string tag, string name, List<Dictionary<string, int>> requiredItems) {
            Scene = scene
            Destination = destination;
            Tag = tag;
            Name = name;
            RequiredItems = requiredItems;
        }
     
        // this is to see if you can reach the center of a region
        // if you can reach the center, you can reach other portals (or check this same requirement to reach those portals)
        public bool canReachCenter(Dictionary<string, int> inventory) {
            // if there are no requirements, you can just walk to this portal
            if (this.RequiredItems.Count == 0) {
                return true;
            }

            // if there are requirements, loop through them to see if any are met
            foreach (Dictionary<string, int> req in this.RequiredItems) {
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

                // check if this requirement is fully met, otherwise move to the next requirement
                int met = 0;
                foreach (string item in req.Keys) {
                    if (!inventory.ContainsKey(item)) {
                        break;
                    } else if(inventory[item] >= req[item]) {
                        met += 1;
                    }
                }
                if (met == req.Count) {
                    return true;
                }
            }

            // if no requirements are met, you cannot reach this portal
            return false;
        }
    }
}
