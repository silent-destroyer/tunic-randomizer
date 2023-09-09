namespace TunicRandomizer {
    public class Portal {
        public string Destination {
            get;
            set;
        }

        public string Tag {
            get;
            set;
        }

        public string Name {
            get;
            set;
        }

        public string Scene
        {
            get;
            set;
        }

        public Portal() {

        }

        public Portal(string destination, string tag, string name, string scene) {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
        }
    }
}
