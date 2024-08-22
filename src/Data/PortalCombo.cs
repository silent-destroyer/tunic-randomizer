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
                // for shop portals that are not in standalone
                if (!TunicPortals.RegionDict.ContainsKey(Portal2.Region)) {
                    inventory[Portal2.Region] = 1;
                    return inventory;
                }
                string outletRegion = TunicPortals.RegionDict[Portal2.Region].OutletRegion ?? Portal2.Region;
                inventory[outletRegion] = 1;
            }
            return inventory;
        }

        // check if this portal is oriented such that we should use the left-to-right shop look
        public bool FlippedShop() {
            if (Portal1.Name.StartsWith("Shop Portal") && (Portal2.Direction == (int)TunicPortals.PDir.EAST || Portal2.Direction == (int)TunicPortals.PDir.SOUTH || Portal1.Direction == (int)TunicPortals.PDir.WEST)) {
                return true;
            }
            if (Portal2.Name.StartsWith("Shop Portal") && (Portal1.Direction == (int)TunicPortals.PDir.EAST || Portal1.Direction == (int)TunicPortals.PDir.SOUTH || Portal2.Direction == (int)TunicPortals.PDir.WEST)) {
                return true;
            }
            return false;
        }

    }
}
