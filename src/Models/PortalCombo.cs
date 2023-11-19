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
            List<string> rewardsList = new List<string>();

            if (this.Portal1.Reachable(inventory))
            {
                rewardsList.Add(this.Portal1.SceneDestinationTag);
            }
            if (this.Portal2.Reachable(inventory))
            {
                rewardsList.Add(this.Portal2.SceneDestinationTag);
            }

            // check if we can get to the other portal from the first portal
            if (this.Portal1.Reachable(inventory))
            {
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
                int count = 0;
                foreach (string item in entryItems)
                {
                    // if it's the regular key, check that we have both keys already
                    if (item == "Key")
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
                    //else
                    //{
                    //    Logger.LogInfo("inventory does not contain " + item);
                    //}
                }
                // if we have all of the entry items, we get the portal
                if (count >= entryItems.Count)
                {
                    if (!inventory.ContainsKey(this.Portal2.SceneDestinationTag))
                    {
                        inventory.Add(this.Portal2.SceneDestinationTag, 1);
                    }
                }
                //else
                //{
                //    Logger.LogInfo("entry items did not succeed for " + this.Portal1.Name + " <-> " + this.Portal2.Name);
                //}

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
                        if (item == "Key")
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
                        //else
                        //{
                        //    Logger.LogInfo("inventory does not contain " + item);
                        //}
                    }
                    // if we have all of the entry items, we get the portal
                    if (count == entryItems.Count)
                    {
                        if (!inventory.ContainsKey(this.Portal1.SceneDestinationTag))
                        {
                            inventory.Add(this.Portal1.SceneDestinationTag, 1);
                        }
                    }
                    //else
                    //{
                    //    Logger.LogInfo("entry items did not succeed for " + this.Portal2.Name + " <-> " + this.Portal1.Name);
                    //}
                }
            //}
            // and now, we grab the regions or portals that we're given access to from having either portal and add them to the rewards list
            if (inventory.ContainsKey(this.Portal1.SceneDestinationTag))
            {
                rewardsList.AddRange(this.Portal1.Rewards(inventory).Except(rewardsList));
            }

            if (inventory.ContainsKey(this.Portal2.SceneDestinationTag))
            {
                rewardsList.AddRange(this.Portal2.Rewards(inventory).Except(rewardsList));
            }

            return rewardsList;
        }
    }
}
