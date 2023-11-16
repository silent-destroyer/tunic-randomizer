using BepInEx.Logging;
using System.Collections.Generic;
using UnityEngine.UI;

namespace TunicRandomizer {
    public class Portal {
        private static ManualLogSource Logger = TunicRandomizer.Logger;
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

        public string Region
        {
            get;
            set;
        }

        public Dictionary<string, int> RequiredItems
        {
            get;
            set;
        } = new Dictionary<string, int>();

        public List<Dictionary<string, int>> RequiredItemsOr
        {
            get;
            set;
        } = new List<Dictionary<string, int>>();

        public Dictionary<string, int> EntryItems
        {
            get;
            set;
        } = new Dictionary<string, int>();

        public List<string> GivesAccess
        {
            get;
            set;
        } = new List<string>();

        public bool DeadEnd
        {
            get;
            set;
        } = false;

        public bool PrayerPortal
        {
            get;
            set;
        } = false;

        public bool OneWay
        {
            get;
            set;
        } = false;

        public bool IgnoreScene
        {
            get;
            set;
        } = false;

        public string SceneDestinationTag
        {
            get;
            set;
        }
        public Portal(string destination, string tag, string name, string scene)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            SceneDestinationTag = (Scene + ", " + Destination + "_" + Tag);
        }
        public Portal(string destination, string tag, string name, string scene, string region, bool deadEnd = false, bool prayerPortal = false, bool oneWay = false, bool ignoreScene = false)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            Region = region;
            DeadEnd = deadEnd;
            PrayerPortal = prayerPortal;
            OneWay = oneWay;
            IgnoreScene = ignoreScene;
            SceneDestinationTag = (Scene + ", " + Destination + "_" + Tag);
        }
        public Portal(string destination, string tag, string name, string scene, string region, Dictionary<string, int> requiredItems)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            Region = region;
            RequiredItems = requiredItems;
            SceneDestinationTag = (Scene + ", " + Destination + "_" + Tag);
        }
        public Portal(string destination, string tag, string name, string scene, string region, List<Dictionary<string, int>> requiredItemsOr)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            Region = region;
            RequiredItemsOr = requiredItemsOr;
            SceneDestinationTag = (Scene + ", " + Destination + "_" + Tag);
        }
        public Portal(string destination, string tag, string name, string scene, string region, Dictionary<string, int> entryItems, bool deadEnd = false, bool prayerPortal = false, bool oneWay = false, bool ignoreScene = false)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            Region = region;
            EntryItems = entryItems;
            DeadEnd = deadEnd;
            PrayerPortal = prayerPortal;
            OneWay = oneWay;
            IgnoreScene = ignoreScene;
            SceneDestinationTag = (Scene + ", " + Destination + "_" + Tag);
        }
        public Portal(string destination, string tag, string name, string scene, string region, List<string> givesAccess, bool deadEnd = false, bool prayerPortal = false, bool oneWay = false, bool ignoreScene = false)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            Region = region;
            GivesAccess = givesAccess;
            DeadEnd = deadEnd;
            PrayerPortal = prayerPortal;
            OneWay = oneWay;
            IgnoreScene = ignoreScene;
            SceneDestinationTag = (Scene + ", " + Destination + "_" + Tag);
        }
        public Portal(string destination, string tag, string name, string scene, string region, Dictionary<string, int> requiredItems, List<string> givesAccess, bool deadEnd = false, bool prayerPortal = false, bool oneWay = false, bool ignoreScene = false)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            Region = region;
            RequiredItems = requiredItems;
            GivesAccess = givesAccess;
            DeadEnd = deadEnd;
            PrayerPortal = prayerPortal;
            OneWay = oneWay;
            IgnoreScene = ignoreScene;
            SceneDestinationTag = (Scene + ", " + Destination + "_" + Tag);
        }
        public Portal(string destination, string tag, string name, string scene, string region, Dictionary<string, int> requiredItems, List<Dictionary<string, int>> requiredItemsOr, Dictionary<string, int> entryItems, List<string> givesAccess, bool deadEnd = false, bool prayerPortal = false, bool oneWay = false, bool ignoreScene = false)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            Region = region;
            RequiredItems = requiredItems;
            RequiredItemsOr = requiredItemsOr;
            EntryItems = entryItems;
            GivesAccess = givesAccess;
            DeadEnd = deadEnd;
            PrayerPortal = prayerPortal;
            OneWay = oneWay;
            IgnoreScene = ignoreScene;
            SceneDestinationTag = (Scene + ", " + Destination + "_" + Tag);
        }

        public bool CanReachCenterFromPortal(Dictionary<string, int> inventory)
        {
            // if ignore scene is set, we don't care about this function since we give the region elsewhere
            if (this.IgnoreScene == true)
            { return false; }

            // create our list of dicts of required items
            List<Dictionary<string, int>> itemsRequired = new List<Dictionary<string, int>>();
            if (this.RequiredItems != null)
            {
                if (this.RequiredItems.Count != 0)
                { itemsRequired.Add(new Dictionary<string, int>(this.RequiredItems)); }
            }
            else if (this.RequiredItemsOr != null)
            {
                if (this.RequiredItemsOr.Count != 0)
                {
                    foreach (Dictionary<string, int> reqSet in this.RequiredItemsOr)
                    { itemsRequired.Add(reqSet); }
                }
            }

            // see if we meet any of the requirement dicts for the portal
            if (itemsRequired != null)
            {
                // if there are no required items, we can reach the center of the region without items (can just walk there)
                if (itemsRequired.Count == 0)
                { return true; }

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
                        { break; }
                        else if (inventory[item] >= req[item])
                        { met += 1; }
                    }
                    if (met == req.Count)
                    { return true; }
                }
            }
            else
            {
                Logger.LogInfo("returning true because itemsRequired is null in canReachCenter for " + this.Name);
                return true;
            }
            return false;
        }

        public bool Reachable(Dictionary<string, int> inventory)
        {
            // if the portal is already in our inventory, no need to go through this process
            if (inventory.ContainsKey(this.SceneDestinationTag))
            {
                //Logger.LogInfo("returning true because the portal " + this.Name + " is already in the inventory");
                return true;
            }
            // create our list of dicts of required items
            List <Dictionary<string, int>> itemsRequired = new List<Dictionary<string, int>>();
            if (this.RequiredItems != null)
            {
                if (this.RequiredItems.Count != 0)
                {
                    //itemsRequired.Add(new Dictionary<string, int>(this.RequiredItems));
                    itemsRequired.Add(this.RequiredItems);
                    // if neither of these are set, we still need the scene (since we already check if we have the other portal in the pair elsewhere)
                    if (this.IgnoreScene == false && !this.RequiredItems.ContainsKey(this.Scene))
                    {
                        this.RequiredItems.Add(this.Scene, 1);
                    }
                }
            }
            else if (this.RequiredItemsOr != null)
            {
                if (this.RequiredItemsOr.Count != 0)
                {
                    foreach (Dictionary<string, int> reqSet in this.RequiredItemsOr)
                    {
                        if (this.IgnoreScene == false && !reqSet.ContainsKey(this.Scene))
                        {
                            reqSet.Add(this.Scene, 1);
                        }
                        itemsRequired.Add(reqSet);
                    }
                }
            }
            else if (this.IgnoreScene == false)
            {
                itemsRequired.Add(new Dictionary<string, int> { { this.Scene, 1 } });
            }

            // see if we meet any of the requirement dicts for the portal
            if (itemsRequired != null && DeadEnd == false)
            {
                if (itemsRequired.Count == 0)
                {
                    //Logger.LogInfo("Portal " + this.Name + " has no requirements, so it's probably a dead end or cant reach");
                    return false;
                }
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
                    //Logger.LogInfo("req.Count is " + req.Count + " for the portal " + this.Name);
                    foreach (string item in req.Keys)
                    {
                        if (!inventory.ContainsKey(item))
                        {
                            //Logger.LogInfo("you do not have " + item + ", so you don't get " + this.Name);
                            break;
                        }
                        else if (inventory[item] >= req[item])
                        {
                            //Logger.LogInfo("for the portal " + this.Name + ", you needed " + item + " and you had it");
                            met++;
                            //Logger.LogInfo("met is now equal to " + met);
                        }
                    }
                    if (met == req.Count)
                    {
                        return true;
                    }
                }
            }
            else
            {
                //Logger.LogInfo("returning false because itemsRequired is null in reachable for " + this.Name);
                return false;
            }
            return false;
        }

        // separate function to say "this is what you get if you have access to this portal"
        public List<string> Rewards(Dictionary<string, int> inventory)
        {
            List<string> rewardsList = new List<string>();

            // GivesAccess means the portal gives access to a specific other portal immediately (ex: fortress exterior shop and beneath the earth)
            if (this.GivesAccess != null)
            {
                foreach (string accessiblePortal in this.GivesAccess)
                {
                    rewardsList.Add(accessiblePortal);
                }
            }

            // if you can reach, you get the center of the region. One-ways give you the center too
            if (CanReachCenterFromPortal(inventory) || this.OneWay == true)
            {
                rewardsList.Add(this.Scene);
            }

            return rewardsList;
        }
    }
}
