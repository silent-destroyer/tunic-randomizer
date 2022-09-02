using System.Collections.Generic;
using HarmonyLib;

namespace TunicRandomizer {
    public class Location {
        public string LocationId {
            get;
            set;
        }
        public string SceneName {
            get;
            set;
        }
        public int SceneId {
            get;
            set;
        }
        public string Position {
            get;
            set;
        }
        public List<string> RequiredItems {
            get;
            set;
        }

        public Location() { }

        public Location(string locationId) {
            LocationId = locationId;
        }

        public Location(string locationId, string sceneName) { 
            LocationId = locationId;
            SceneName = sceneName;
        }

        public Location(string locationId, string sceneName, int sceneId) {
            LocationId = locationId;
            SceneName = sceneName;
            SceneId = sceneId;
        }

        public Location(string locationId, string sceneName, int sceneId, string position) {
            LocationId = locationId;
            SceneName = sceneName;
            SceneId = sceneId;
            Position = position;
        }
        
        public Location(string locationId, string sceneName, int sceneId, string position, List<string> requiredItems) {
            LocationId = locationId;
            SceneName = sceneName;
            SceneId = sceneId;
            Position = position;
            RequiredItems = requiredItems;
        }
    }
}
