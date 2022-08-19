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
        public List<Restrictions> LocationRestrictions {
            get;
            set;
        }

        // CHEST 1006 requires ice dagger and fire rod
        public enum Restrictions {
            GRAPPLE,
            HYPERDASH,
            LANTERN
        }

        public Location() { }

        public Location(string locationId) {
            LocationId = locationId;
            LocationRestrictions = new List<Restrictions>();
        }

        public Location(string locationId, Restrictions locationRestrictions) {
            LocationId = locationId;
            LocationRestrictions = new List<Restrictions>();
            LocationRestrictions.AddItem(locationRestrictions);
        }

        public Location(string locationId, string sceneName, int sceneId, string position) {
            LocationId = locationId;
            SceneName = sceneName;
            SceneId = sceneId;
            Position = position;
            LocationRestrictions = new List<Restrictions>();
        }

        public Location(string locationId, string sceneName, int sceneId, string position, Restrictions locationRestrictions) : this(locationId) {
            SceneName = sceneName;
            SceneId = sceneId;
            Position = position;
            LocationRestrictions = new List<Restrictions>();
            LocationRestrictions.AddItem(locationRestrictions);
        }
    }
}
