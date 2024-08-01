using System.Collections.Generic;

namespace TunicRandomizer {
    public class PortalCombo {
        public Portal Portal1 { get; set; }  // the source portal

        public Portal Portal2 { get; set; }  // the destination portal

        public PortalCombo() {}

        public PortalCombo(Portal portal1, Portal portal2) {
            Portal1 = portal1;
            Portal2 = portal2;
        }

        public Dictionary<string, int> AddComboRegion(Dictionary<string, int> inventory) {
            if (inventory.ContainsKey(Portal1.Region)) {
                string outletRegion = DestinationRegion();
                if (!inventory.ContainsKey(outletRegion)) {
                    inventory.Add(outletRegion, 1);
                }
            }
            return inventory;
        }

        // check if this portal is oriented such that we should use the left-to-right shop look
        public bool FlippedShop() {
            if (Portal2.Name == "Shop Portal" && (Portal1.Direction == (int)TunicPortals.PDir.EAST || Portal1.Direction == (int)TunicPortals.PDir.SOUTH)) {
                return true;
            }
            return false;
        }

        public string DestinationRegion() {
            return TunicPortals.RegionDict[Portal2.Region].OutletRegion ?? Portal2.Region;
        }

    }
}
