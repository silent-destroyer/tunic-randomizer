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
            Logger.LogInfo("starting location.reachable");
            List<Dictionary<string, int>> itemsRequired;
            if (SaveFile.GetInt("randomizer door rando enabled") == 1)
            {
                itemsRequired = this.RequiredItemsDoors;
            }
            else
            {
                itemsRequired = this.RequiredItems;
            }

            //if there are no requirements, the location is reachable
            Logger.LogInfo("location reachable step 1");
            if (itemsRequired.Count == 0)
            {
                Logger.LogInfo("location reachable step 2");
                return true;
            }

            //if there are requirements, loop through each requirement to see if any are fully met
            foreach (Dictionary<string, int> req in itemsRequired)
            {
                Logger.LogInfo("location reachable step 3");
                //ensure req and items use same terms
                if (SaveFile.GetInt("randomizer sword progression enabled") != 0)
                {
                    Logger.LogInfo("location reachable step 4");
                    if (req.ContainsKey("Stick"))
                    {
                        Logger.LogInfo("location reachable step 5");
                        req["Sword Progression"] = 1;
                        req.Remove("Stick");
                    }
                    if (req.ContainsKey("Sword"))
                    {
                        Logger.LogInfo("location reachable step 6");
                        req["Sword Progression"] = 2;
                        req.Remove("Sword");
                    }
                }

                //check if this requirement is fully met, otherwise move to the next requirement
                int met = 0;
                Logger.LogInfo("location reachable step 7");
                foreach (string item in req.Keys)
                {
                    Logger.LogInfo("location reachable step 8");
                    if (!inventory.ContainsKey(item))
                    {
                        Logger.LogInfo("location reachable step 9");
                        break;
                    }
                    else if (inventory[item] >= req[item])
                    {
                        Logger.LogInfo("location reachable step 10");
                        met += 1;
                    }
                }
                if (met == req.Count)
                {
                    Logger.LogInfo("location reachable step 11");
                    return true;
                }
            }
            Logger.LogInfo("location reachable step 12");
            //if no requirements are met, the location isn't reachable
            return false;
        }
    }
}
