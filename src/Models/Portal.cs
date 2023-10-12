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

        public bool CantReach
        {
            get;
            set;
        } = false;

        public string SceneDestinationTag
        {
            get;
            set;
        }

        public Portal(string destination, string tag, string name, string scene, bool deadEnd = false, bool prayerPortal = false, bool oneWay = false, bool cantReach = false)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            DeadEnd = deadEnd;
            PrayerPortal = prayerPortal;
            OneWay = oneWay;
            CantReach = cantReach;
            SceneDestinationTag = (Scene + ", " + Destination + "_" + Tag);
        }
        public Portal(string destination, string tag, string name, string scene, Dictionary<string, int> requiredItems)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            RequiredItems = requiredItems;
            SceneDestinationTag = (Scene + ", " + Destination + "_" + Tag);
        }
        public Portal(string destination, string tag, string name, string scene, List<Dictionary<string, int>> requiredItemsOr)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            RequiredItemsOr = requiredItemsOr;
            SceneDestinationTag = (Scene + ", " + Destination + "_" + Tag);
        }
        public Portal(string destination, string tag, string name, string scene, Dictionary<string, int> entryItems, bool deadEnd = false, bool prayerPortal = false, bool oneWay = false, bool cantReach = false)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            EntryItems = entryItems;
            DeadEnd = deadEnd;
            PrayerPortal = prayerPortal;
            OneWay = oneWay;
            CantReach = cantReach;
            SceneDestinationTag = (Scene + ", " + Destination + "_" + Tag);
        }
        public Portal(string destination, string tag, string name, string scene, List<string> givesAccess, bool deadEnd = false, bool prayerPortal = false, bool oneWay = false, bool cantReach = false)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            GivesAccess = givesAccess;
            DeadEnd = deadEnd;
            PrayerPortal = prayerPortal;
            OneWay = oneWay;
            CantReach = cantReach;
            SceneDestinationTag = (Scene + ", " + Destination + "_" + Tag);
        }
        public Portal(string destination, string tag, string name, string scene, Dictionary<string, int> requiredItems, List<string> givesAccess, bool deadEnd = false, bool prayerPortal = false, bool oneWay = false, bool cantReach = false)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            RequiredItems = requiredItems;
            GivesAccess = givesAccess;
            DeadEnd = deadEnd;
            PrayerPortal = prayerPortal;
            OneWay = oneWay;
            CantReach = cantReach;
            SceneDestinationTag = (Scene + ", " + Destination + "_" + Tag);
        }
        public Portal(string destination, string tag, string name, string scene, Dictionary<string, int> requiredItems, List<Dictionary<string, int>> requiredItemsOr, Dictionary<string, int> entryItems, List<string> givesAccess, bool deadEnd = false, bool prayerPortal = false, bool oneWay = false, bool cantReach = false)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            RequiredItems = requiredItems;
            RequiredItemsOr = requiredItemsOr;
            EntryItems = entryItems;
            GivesAccess = givesAccess;
            DeadEnd = deadEnd;
            PrayerPortal = prayerPortal;
            OneWay = oneWay;
            CantReach = cantReach;
            SceneDestinationTag = (Scene + ", " + Destination + "_" + Tag);
        }

        public bool CanReachCenterFromPortal(Dictionary<string, int> inventory)
        {
            if (this.CantReach == true || DeadEnd == true)
            { return false; }

            // create our list of dicts of required items
            List<Dictionary<string, int>> itemsRequired = new List<Dictionary<string, int>>();
            if (this.RequiredItems != null)
            {
                if (this.RequiredItems.Count != 0)
                {
                    itemsRequired.Add(new Dictionary<string, int>(this.RequiredItems));
                }
            }
            else if (this.RequiredItemsOr != null)
            {
                if (this.RequiredItemsOr.Count != 0)
                {
                    foreach (Dictionary<string, int> reqSet in this.RequiredItemsOr)
                    {
                        itemsRequired.Add(reqSet);
                    }
                }
            }

            // see if we meet any of the requirement dicts for the portal
            if (itemsRequired != null)
            {
                if (itemsRequired.Count == 0)
                {
                    return true;
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
            }
            else
            {
                Logger.LogInfo("returning true because itemsRequired is null in canreachcenter for " + this.Name);
                return true;
            }
            return false;
        }

        public bool Reachable(Dictionary<string, int> inventory)
        {
            // if the portal is already in our inventory, no need to go through this process
            if (inventory.ContainsKey(this.SceneDestinationTag))
            {
                Logger.LogInfo("returning true because the portal " + this.Name + " is already in the inventory");
                return true;
            }
            // create our list of dicts of required items
            List <Dictionary<string, int>> itemsRequired = new List<Dictionary<string, int>>();
            if (this.RequiredItems != null)
            {
                if (this.RequiredItems.Count != 0)
                {
                    // if neither of these are set, we still need the scene (since we already check if we have the other portal in the pair elsewhere)
                    if ((this.CantReach == false || this.OneWay == false) && !this.RequiredItems.ContainsKey(this.Scene))
                    {
                        this.RequiredItems.Add(this.Scene, 1);
                    }
                    itemsRequired.Add(new Dictionary<string, int>(this.RequiredItems));
                }
            }
            else if (this.RequiredItemsOr != null)
            {
                if (this.RequiredItemsOr.Count != 0)
                {
                    foreach (Dictionary<string, int> reqSet in this.RequiredItemsOr)
                    {
                        if ((this.CantReach == false || this.OneWay == false) && !reqSet.ContainsKey(this.Scene))
                        {
                            reqSet.Add(this.Scene, 1);
                        }
                        itemsRequired.Add(reqSet);
                    }
                }
            }
            else if (this.CantReach == false || this.DeadEnd == false)
            {
                itemsRequired.Add(new Dictionary<string, int> { { this.Scene, 1 } });
            }

            // see if we meet any of the requirement dicts for the portal
            if (itemsRequired != null && DeadEnd == false && CantReach == false)
            {
                if (itemsRequired.Count == 0)
                {
                    Logger.LogInfo("Portal " + this.Name + " has no requirements, so it's probably a dead end or cant reach");
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
                    foreach (string item in req.Keys)
                    {
                        if (!inventory.ContainsKey(item))
                        {
                            break;
                        }
                        else if (inventory[item] >= req[item])
                        {
                            Logger.LogInfo("for the portal " + this.Name + ", you needed " + item + " and you had it");
                            met += 1;
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
                Logger.LogInfo("returning true because itemsRequired is null in reachable for " + this.Name);
                return true;
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
            if (CanReachCenterFromPortal(inventory))
            {
                rewardsList.Add(this.Scene);
            }

            return rewardsList;
        }
    }
}
