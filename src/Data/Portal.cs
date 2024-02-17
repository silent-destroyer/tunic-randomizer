using System.Collections.Generic;

namespace TunicRandomizer {
    public class Portal {
        //private static ManualLogSource Logger = TunicRandomizer.Logger;
        public string Scene { get; set; }
        public string Destination { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string SceneDestinationTag { get; set; }

        public Portal(string name, string destination, string scene, string region) {
            Name = name;
            Destination = destination;
            Scene = scene;
            Region = region;
            SceneDestinationTag = (Scene + ", " + Destination);
        }

        public bool CanReach(Dictionary<string, int> inventory) {
            if (inventory.ContainsKey(this.Region)) {
                return true;
            } else {
                return false;
            }
        }
    }
}
