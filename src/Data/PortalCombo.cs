using System.Collections.Generic;

namespace TunicRandomizer {
    public class PortalCombo {
        public Portal Portal1 { get; set; }  // the source portal

        public Portal Portal2 { get; set; }  // the destination portal

        public PortalCombo() {}

        public bool FlippedShop = false;

        public string ComboTag;

        public PortalCombo(Portal portal1, Portal portal2) {
            Portal1 = portal1;
            Portal2 = portal2;
            if ((Portal1.Name.StartsWith("Shop Portal") && (Portal2.Direction == (int)ERData.PDir.EAST || Portal2.Direction == (int)ERData.PDir.SOUTH || Portal1.Direction == (int)ERData.PDir.WEST))
                || (Portal2.Name.StartsWith("Shop Portal") && (Portal1.Direction == (int)ERData.PDir.EAST || Portal1.Direction == (int)ERData.PDir.SOUTH || Portal2.Direction == (int)ERData.PDir.WEST))) {
                FlippedShop = true;
            }
            ComboTag = $"{portal1.Name}--{portal2.Name}";
        }

        public Dictionary<string, int> AddComboRegion(Dictionary<string, int> inventory) {
            if (inventory.ContainsKey(Portal1.Region)) {
                // for shop portals that are not in standalone
                if (!ERData.RegionDict.ContainsKey(Portal2.Region)) {
                    inventory[Portal2.Region] = 1;
                    return inventory;
                }
                string outletRegion = ERData.RegionDict[Portal2.Region].OutletRegion ?? Portal2.Region;
                inventory[outletRegion] = 1;
            }
            return inventory;
        }

    }
}
