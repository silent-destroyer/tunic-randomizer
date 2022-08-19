namespace TunicRandomizer {
    public class Reward {
        public string Name {
            get;
            set;
        }
        public string Type {
            get;
            set;
        }
        public int Amount {
            get;
            set;
        }
        public string Icon {
            get;
            set;
        }
        public string Description {
            get;
            set;
        }

        private void setup(string name, string type, int amount, string description, string icon) {
            Name = name;
            Type = type;
            Amount = amount;
            Description = description;
            Icon = icon;
        }

        public Reward() {

        }

        public Reward(string name, string type, int amount) {
            setup(name, type, amount, "", "");
        }

        public Reward(string name, string type, int amount, string description) {
            setup(name, type, amount, description, "");
        }

        public Reward(string name, string type, int amount, string description, string icon) {
            setup(name, type, amount, description, icon);
        }


    }
}
