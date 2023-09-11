using System.Collections.Generic;

namespace TunicRandomizer {
    public class Portal {
        public string Scene {
            get;
            set;
        }
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

        public List<Dictionary<string, int>> RequiredItems {
            get;
            set;
        }

        public Portal(string destination, string tag, string name, string scene, List<Dictionary<string, int>> requiredItems)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            RequiredItems = requiredItems;
        }
    }
}
