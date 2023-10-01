using System.Collections.Generic;
using BepInEx.Logging;
using HarmonyLib;

namespace TunicRandomizer {
    public class Location
    {
        private static ManualLogSource Logger = TunicRandomizer.Logger;
        public string LocationId
        {
            get;
            set;
        }
        public string SceneName
        {
            get;
            set;
        }
        public int SceneId
        {
            get;
            set;
        }
        public string Position
        {
            get;
            set;
        }
        public List<Dictionary<string, int>> RequiredItems
        {
            get;
            set;
        }
        public List<Dictionary<string, int>> RequiredItemsDoors
        {
            get;
            set;
        }

        public Location() { }

        public Location(string locationId)
        {
            LocationId = locationId;
        }

        public Location(string locationId, string sceneName)
        {
            LocationId = locationId;
            SceneName = sceneName;
        }

        public Location(string locationId, string sceneName, int sceneId)
        {
            LocationId = locationId;
            SceneName = sceneName;
            SceneId = sceneId;
        }

        public Location(string locationId, string sceneName, int sceneId, string position)
        {
            LocationId = locationId;
            SceneName = sceneName;
            SceneId = sceneId;
            Position = position;
        }

        public Location(string locationId, string sceneName, int sceneId, string position, List<Dictionary<string, int>> requiredItems)
        {
            LocationId = locationId;
            SceneName = sceneName;
            SceneId = sceneId;
            Position = position;
            RequiredItems = requiredItems;
        }

        public Location(string locationId, string sceneName, int sceneId, string position, List<Dictionary<string, int>> requiredItems, List<Dictionary<string, int>> requiredItemsDoors)
        {
            LocationId = locationId;
            SceneName = sceneName;
            SceneId = sceneId;
            Position = position;
            RequiredItems = requiredItems;
            RequiredItemsDoors = requiredItemsDoors;
        }


        public bool reachable(Dictionary<string, int> inventory)
        {
            List<Dictionary<string, int>> itemsRequired;
            if (SaveFile.GetInt("randomizer entrance rando enabled") == 1)
            {
                itemsRequired = this.RequiredItemsDoors;
            }
            else
            {
                itemsRequired = this.RequiredItems;
            }

            //if there are no requirements, the location is reachable
            if (itemsRequired.Count == 0)
            {
                return true;
            }

            //if there are requirements, loop through each requirement to see if any are fully met
            foreach (Dictionary<string, int> req in itemsRequired)
            {
                //ensure req and items use same terms
                if (SaveFile.GetInt("randomizer sword progression enabled") != 0)
                {
                    if (req.ContainsKey("Stick"))
                    {
                        req["Sword Progression"] = 1;
                        req.Remove("Stick");
                    }
                    if (req.ContainsKey("Sword"))
                    {
                        req["Sword Progression"] = 2;
                        req.Remove("Sword");
                    }
                }

                //check if this requirement is fully met, otherwise move to the next requirement
                int met = 0;
                foreach (string item in req.Keys)
                {
                    if (!inventory.ContainsKey(item))
                    {
                        break;
                    }
                    else if (inventory[item] >= req[item])
                    {
                        met += 1;
                    }
                }
                if (met == req.Count)
                {
                    return true;
                }
            }
            //if no requirements are met, the location isn't reachable
            return false;
        }
    }
}
