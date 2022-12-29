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

        public Reward() {

        }

        public Reward(string name, string type, int amount) {
            Name = name;
            Type = type;
            Amount = amount;
        }


    }
}
