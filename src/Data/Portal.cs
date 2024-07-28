using System.Collections.Generic;

namespace TunicRandomizer {
    public class Portal {
        public string Name { get; set; }
        public string Destination { get; set; }
        public string Tag { get; set; }
        public string Scene { get; set; }
        public string Region { get; set; }
        public string DestinationTag { get; set; }
        public string SceneDestinationTag { get; set; }
        public string DestinationSceneTag { get; set; }
        public int Direction { get; set; }

        public Portal(string name, string destination, string tag, string scene, string region) {
            Name = name;
            Destination = destination;
            Tag = tag;
            Scene = scene;
            Region = region;
            DestinationTag = (Destination + Tag);
            SceneDestinationTag = (Scene + ", " + DestinationTag);
            DestinationSceneTag = (Destination + ", " + Scene + Tag);  // for finding the vanilla connection
        }

        public Portal(string name, string destination, string tag, string scene, string region, int direction) {
            Name = name;
            Destination = destination;
            Tag = tag;
            Scene = scene;
            Region = region;
            DestinationTag = (Destination + Tag);
            SceneDestinationTag = (Scene + ", " + DestinationTag);
            DestinationSceneTag = (Destination + ", " + Scene + Tag);
            Direction = direction;
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
