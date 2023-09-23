using BepInEx.Logging;
using System.Collections.Generic;
using System.Linq;

namespace TunicRandomizer {
    public class PortalCombo {
        private static ManualLogSource Logger = TunicRandomizer.Logger;
        public Portal Portal1 {
            get;
            set;
        }

        public Portal Portal2 {
            get;
            set;
        }

        public List<Portal> Portals
        {
            get;
            set;
        }

        public PortalCombo() {}

        public PortalCombo(Portal portal1, Portal portal2) {
            Portal1 = portal1;
            Portal2 = portal2;
            Portals = new List<Portal>
            {
                portal1,
                portal2
            };
        }

        // the idea is to do something like, we check if we can get any rewards from this portal combo based on what we have
        // if we have the region a portal is in, we add that portal to the rewards list (if it meets the requirements)
        public List<string> ComboRewards(Dictionary<string, int> inventory)
        {
            Logger.LogInfo("combo rewards started");
            List<string> rewardsList = new List<string>();

            Logger.LogInfo("portal 1 is " + Portal1.Name);
            Logger.LogInfo("portal 2 is " + Portal2.Name);

            // first, let's just see if we have both portals here already so we can skip processing them
            if (!inventory.ContainsKey(this.Portal1.SceneDestinationTag) || !inventory.ContainsKey(this.Portal2.SceneDestinationTag))
            {
                Logger.LogInfo("!inventory contains key");
                // check if we can get to the other portal from the first portal
                if (this.Portal1.Reachable(inventory))
                {
                    Logger.LogInfo("this portal1 reachable inventory");
                    List<string> entryItems = new List<string>();
                    if (this.Portal1.PrayerPortal)
                    {
                        entryItems.Add("12");
                    }
                    if (this.Portal1.EntryItems != null)
                    {
                        foreach (KeyValuePair<string, int> items in this.Portal1.EntryItems)
                        {
                            entryItems.Add(items.Key);
                        }
                    }
                    Logger.LogInfo("doing the entry items checks");
                    int count = 0;
                    foreach (string item in entryItems)
                    {
                        // if it's the regular key, check that we have both keys already
                        if (item == "key")
                        {
                            if (inventory[item] == 2)
                            {
                                count++;
                            }
                        }
                        else if (inventory.ContainsKey(item))
                        {
                            count++;
                        }
                    }
                    Logger.LogInfo("checking entry items count");
                    // if we have all of the entry items, we get the scene
                    if (count == entryItems.Count)
                    {
                        Logger.LogInfo("adding rewards to list");
                        rewardsList.Add(this.Portal2.SceneDestinationTag);
                        // and we might as well just add it to the inventory now
                        inventory.Add(this.Portal2.SceneDestinationTag, 1);
                    }
                }

                // and then we do the same for the second portal
                if (this.Portal2.Reachable(inventory))
                {
                    List<string> entryItems = new List<string>();
                    if (this.Portal2.PrayerPortal)
                    {
                        entryItems.Add("12");
                    }
                    if (this.Portal2.EntryItems != null)
                    {
                        foreach (KeyValuePair<string, int> items in this.Portal2.EntryItems)
                        {
                            entryItems.Add(items.Key);
                        }
                    }
                    int count = 0;
                    foreach (string item in entryItems)
                    {
                        // if it's the regular key, check that we have both keys already
                        if (item == "key")
                        {
                            if (inventory[item] == 2)
                            {
                                count++;
                            }
                        }
                        else if (inventory.ContainsKey(item))
                        {
                            count++;
                        }
                    }
                    // if we have all of the entry items, we get the scene
                    if (count == entryItems.Count)
                    {
                        rewardsList.Add(this.Portal1.SceneDestinationTag);
                        inventory.Add(this.Portal1.SceneDestinationTag, 1);
                    }
                }
            }
            Logger.LogInfo("and now, we grab the regions or portals");
            // and now, we grab the regions or portals that we're given access to from having either portal and add them to the rewards list
            if (inventory.ContainsKey(this.Portal1.SceneDestinationTag))
            {
                rewardsList.AddRange(this.Portal1.Rewards().Except(rewardsList));
            }
            Logger.LogInfo("list 1 done");

            if (inventory.ContainsKey(this.Portal2.SceneDestinationTag))
            {
                rewardsList.AddRange(this.Portal2.Rewards().Except(rewardsList));
            }

            return rewardsList;
        }
    }
}
