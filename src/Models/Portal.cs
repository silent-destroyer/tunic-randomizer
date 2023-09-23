using BepInEx.Logging;
using System.Collections.Generic;

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

        public bool IsDeadEnd
        {
            get;
            set;
        }

        public bool PrayerPortal
        {
            get;
            set;
        }

        public bool OneWay
        {
            get;
            set;
        }

        public bool CantReach
        {
            get;
            set;
        }

        public string SceneDestinationTag
        {
            get;
            set;
        }

        public Portal(string destination, string tag, string name, string scene, bool isDeadEnd = false, bool prayerPortal = false, bool oneWay = false, bool cantReach = false)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            IsDeadEnd = isDeadEnd;
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
        public Portal(string destination, string tag, string name, string scene, Dictionary<string, int> entryItems, bool isDeadEnd = false, bool prayerPortal = false, bool oneWay = false, bool cantReach = false)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            EntryItems = entryItems;
            IsDeadEnd = isDeadEnd;
            PrayerPortal = prayerPortal;
            OneWay = oneWay;
            CantReach = cantReach;
            SceneDestinationTag = (Scene + ", " + Destination + "_" + Tag);
        }
        public Portal(string destination, string tag, string name, string scene, List<string> givesAccess, bool isDeadEnd = false, bool prayerPortal = false, bool oneWay = false, bool cantReach = false)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            GivesAccess = givesAccess;
            IsDeadEnd = isDeadEnd;
            PrayerPortal = prayerPortal;
            OneWay = oneWay;
            CantReach = cantReach;
            SceneDestinationTag = (Scene + ", " + Destination + "_" + Tag);
        }
        public Portal(string destination, string tag, string name, string scene, Dictionary<string, int> requiredItems, List<string> givesAccess, bool isDeadEnd = false, bool prayerPortal = false, bool oneWay = false, bool cantReach = false)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            RequiredItems = requiredItems;
            GivesAccess = givesAccess;
            IsDeadEnd = isDeadEnd;
            PrayerPortal = prayerPortal;
            OneWay = oneWay;
            CantReach = cantReach;
            SceneDestinationTag = (Scene + ", " + Destination + "_" + Tag);
        }
        public Portal(string destination, string tag, string name, string scene, Dictionary<string, int> requiredItems, List<Dictionary<string, int>> requiredItemsOr, Dictionary<string, int> entryItems, List<string> givesAccess, bool isDeadEnd = false, bool prayerPortal = false, bool oneWay = false, bool cantReach = false)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            RequiredItems = requiredItems;
            RequiredItemsOr = requiredItemsOr;
            EntryItems = entryItems;
            GivesAccess = givesAccess;
            IsDeadEnd = isDeadEnd;
            PrayerPortal = prayerPortal;
            OneWay = oneWay;
            CantReach = cantReach;
            SceneDestinationTag = (Scene + ", " + Destination + "_" + Tag);
        }

        public bool Reachable(Dictionary<string, int> inventory)
        {
            // if the portal is already in our inventory, no need to go through this process
            if (inventory.ContainsKey(this.SceneDestinationTag))
            {
                return true;
            }
            // create our list of dicts of required items
            List <Dictionary<string, int>> itemsRequired = new List<Dictionary<string, int>>();
            if (this.RequiredItems != null)
            {
                // if neither of these are set, we still need the scene (since we already check if we have the other portal in the pair elsewhere)
                if (this.CantReach == false && this.OneWay == false && !this.RequiredItems.ContainsKey(this.Scene))
                {
                    this.RequiredItems.Add(this.Scene, 1);
                }
                itemsRequired.Add(new Dictionary<string, int>(this.RequiredItems));
            }
            else if (this.RequiredItemsOr != null)
            {
                foreach (Dictionary<string, int> reqSet in this.RequiredItemsOr)
                {
                    if (this.CantReach == false && this.OneWay == false && !reqSet.ContainsKey(this.Scene))
                    {
                        reqSet.Add(this.Scene, 1);
                    }
                    itemsRequired.Add(reqSet);
                }
            }

            // see if we meet any of the requirement dicts for the portal
            if (itemsRequired != null)
            {
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
            return false;
        }

        // separate function to say "this is what you get if you have access to this portal"
        public List<string> Rewards()
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

            // if you canreach, you get the center of the region. One-ways give you the center too
            if (this.CantReach == false)
            {
                rewardsList.Add(this.Scene);
            }

            return rewardsList;
        }
    }
}
