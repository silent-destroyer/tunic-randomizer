using System.Collections.Generic;

namespace TunicRandomizer {
    public class PortalCombo {
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
        public List<string> comboRewards(Dictionary<string, int> inventory)
        {
            List<string> rewardsList = new List<string>();

            // first, let's just see if we have both portals here already so we can skip processing them
            if (!inventory.ContainsKey(this.Portal1.SceneDestinationTag) || !inventory.ContainsKey(this.Portal2.SceneDestinationTag))
            {
                // check if we can get to the other portal from the first portal
                if (this.Portal1.reachable(inventory))
                {
                    List<string> entryItems = new List<string>();
                    if (this.Portal1.PrayerPortal)
                    {
                        entryItems.Add("12");
                    }
                    if (this.Portal1.EntryItems.Count != 0)
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
                        rewardsList.Add(this.Portal2.SceneDestinationTag);
                        // and we might as well just add it to the inventory now
                        inventory.Add(this.Portal2.SceneDestinationTag, 1);
                    }
                }

                // and then we do the same for the second portal
                if (this.Portal2.reachable(inventory))
                {
                    List<string> entryItems = new List<string>();
                    if (this.Portal2.PrayerPortal)
                    {
                        entryItems.Add("12");
                    }
                    if (this.Portal2.EntryItems.Count != 0)
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

            if (inventory.ContainsKey(this.Portal1.SceneDestinationTag))
            {

            }

            return rewardsList;
        }

    }
}
