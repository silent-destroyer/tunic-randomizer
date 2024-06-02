using System.Collections.Generic;
using System.Linq;

namespace TunicRandomizer {
    public class PortalCombo {
        public Portal Portal1 { get; set; }

        public Portal Portal2 { get; set; }

        public List<Portal> Portals { get; set; }

        public PortalCombo() {}

        public PortalCombo(Portal portal1, Portal portal2) {
            Portal1 = portal1;
            Portal2 = portal2;
            Portals = new List<Portal> {
                portal1,
                portal2
            };
        }

        public bool CanReach(Dictionary<string, int> inventory) {
            if (inventory.ContainsKey(this.Portal1.Region) || inventory.ContainsKey(this.Portal2.Region)) {
                return true;
            } else {
                return false;
            }
        }

        public Dictionary<string, int> AddComboRegions(Dictionary<string, int> inventory) {
            // if both regions are in here already, just skip it
            if (inventory.ContainsKey(this.Portal1.Region) && inventory.ContainsKey(this.Portal2.Region)) {
                return inventory;
            }
            // if you can reach this portal combo, add whichever region you don't have yet
            if (this.CanReach(inventory)) {
                if (!inventory.ContainsKey(this.Portal1.Region)) {
                    inventory.Add(this.Portal1.Region, 1);
                }
                if (!inventory.ContainsKey(this.Portal2.Region)) {
                    inventory.Add(this.Portal2.Region, 1);
                }
            }
            return inventory;
        }
    }
}
