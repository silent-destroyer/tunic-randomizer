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

        public PortalCombo() {}

        public PortalCombo(Portal portal1, Portal portal2) {
            Portal1 = portal1;
            Portal2 = portal2;
        }
    }
}
