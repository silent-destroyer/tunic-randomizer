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
        public bool IsDeadEnd { get; set; }

        public Portal(string name, string destination, string tag, string scene, string region, bool isDeadEnd) {
            Name = name;
            Destination = destination;
            Tag = tag;
            Scene = scene;
            Region = region;
            DestinationTag = (Destination + "_" + Tag);
            SceneDestinationTag = (Scene + ", " + DestinationTag);
            DestinationSceneTag = (Destination + ", " + Scene + "_" + Tag);  // for finding the vanilla connection
            Direction = (int)ERData.PDir.NONE;
            IsDeadEnd = isDeadEnd;
        }

        public Portal(string name, string destination, string tag, string scene, string region, int direction, bool isDeadEnd) {
            Name = name;
            Destination = destination;
            Tag = tag;
            Scene = scene;
            Region = region;
            DestinationTag = (Destination + "_" + Tag);
            SceneDestinationTag = (Scene + ", " + DestinationTag);
            DestinationSceneTag = (Destination + ", " + Scene + "_" + Tag);
            Direction = direction;
            IsDeadEnd = isDeadEnd;
        }

        public bool CanReach(Dictionary<string, int> inventory) {
            if (inventory.ContainsKey(this.Region)) {
                return true;
            } else {
                return false;
            }
        }

        // for portals that lead directly to a different region, ie: appearing at a yellow portal square
        public string OutletRegion() {
            // for shop portals past number 8, we need an override here
            if (Region.StartsWith("Shop") && !ERData.RegionDict.ContainsKey(Region)) {
                return Region;
            }
            return ERData.RegionDict[Region].OutletRegion ?? Region;
        }
    }
}
