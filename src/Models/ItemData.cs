namespace TunicRandomizer {
    public class ItemData {
        public Reward Reward {
            get;
            set;
        }

        public Location Location {
            get;
            set;
        }

        public ItemData() {}

        public ItemData(Reward item, Location location) {
            Reward = item;
            Location = location;
        }
    }
}
