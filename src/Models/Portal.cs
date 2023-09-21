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

        public Dictionary<string, int> RequiredItems
        {
            get;
            set;
        }

        public List<Dictionary<string, int>> RequiredItemsOr {
            get;
            set;
        }

        public Dictionary<string, int> EntryItems
        {
            get;
            set;
        }

        public List<string> GivesAccess
        {
            get;
            set;
        }

        public bool IsDeadEnd
        {
            get;
            set;
        }

        public bool PrayerPortal
        {
            get;
            set;
        }
        
        public bool OneWay
        {
            get;
            set;
        }

        public bool CantReach
        {
            get;
            set;
        }

        public Portal(string destination, string tag, string name, string scene, bool isDeadEnd = false, bool prayerPortal = false, bool oneWay = false, bool cantReach = false)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            IsDeadEnd = isDeadEnd;
            PrayerPortal = prayerPortal;
            OneWay = oneWay;
            CantReach = cantReach;
        }
        public Portal(string destination, string tag, string name, string scene, Dictionary<string, int> requiredItems, List<Dictionary<string, int>> requiredItemsOr, Dictionary<string, int> entryItems, List<string> givesAccess, bool isDeadEnd = false, bool prayerPortal = false, bool oneWay = false, bool cantReach = false)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            RequiredItemsOr = requiredItemsOr;
            RequiredItems = requiredItems;
            EntryItems = entryItems;
            GivesAccess = givesAccess;
            IsDeadEnd = isDeadEnd;
            PrayerPortal = prayerPortal;
            OneWay = oneWay;
            CantReach = cantReach;
        }
        public Portal(string destination, string tag, string name, string scene, Dictionary<string, int> requiredItems)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            RequiredItems = requiredItems;
        }
        public Portal(string destination, string tag, string name, string scene, Dictionary<string, int> entryItems, bool isDeadEnd = false, bool prayerPortal = false, bool oneWay = false, bool cantReach = false)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            EntryItems = entryItems;
            IsDeadEnd = isDeadEnd;
            PrayerPortal = prayerPortal;
            OneWay = oneWay;
            CantReach = cantReach;
        }
        public Portal(string destination, string tag, string name, string scene, List<string> givesAccess, bool isDeadEnd = false, bool prayerPortal = false, bool oneWay = false, bool cantReach = false)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            GivesAccess = givesAccess;
            IsDeadEnd = isDeadEnd;
            PrayerPortal = prayerPortal;
            OneWay = oneWay;
            CantReach = cantReach;
        }
        public Portal(string destination, string tag, string name, string scene, Dictionary<string, int> requiredItems, List<string> givesAccess, bool isDeadEnd = false, bool prayerPortal = false, bool oneWay = false, bool cantReach = false)
        {
            Destination = destination;
            Tag = tag;
            Name = name;
            Scene = scene;
            RequiredItems = requiredItems;
            GivesAccess = givesAccess;
            IsDeadEnd = isDeadEnd;
            PrayerPortal = prayerPortal;
            OneWay = oneWay;
            CantReach = cantReach;
        }
    }
}
