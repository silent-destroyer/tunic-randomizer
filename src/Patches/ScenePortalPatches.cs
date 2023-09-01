using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;
using BepInEx.Logging;
using Lib;

namespace TunicRandomizer
{
    public class ScenePortalPatches {
        private static ManualLogSource Logger = TunicRandomizer.Logger;

        //public class TunicPortal
        //{
        //    public string SceneName;
        //    public string Destination;
        //    public string DestinationTag;
        //    public string DestinationPair;
        //    public string PortalName;
        //    public Vector3 Position;
        //    public Quaternion Rotation;

        //    public TunicPortal() { }

        //    public TunicPortal(string destination, string destinationTag, string portalName)
        //    {
        //        Destination = destination;
        //        DestinationTag = destinationTag;
        //        PortalName = portalName;
        //        DestinationPair = destination + "_" + destinationTag;
        //    }
        //}

        //public static Dictionary<string, List<TunicPortal>> PortalList = new Dictionary<string, List<TunicPortal>>
        //{   // formatted with region the portal is in, with the dict being the tag combo and portal name
        //    {   
        //        "Overworld Redux",
        //        new List<TunicPortal>
        //        {
        //            new TunicPortal("Windmill", "", "Portal (4)"),
        //            new TunicPortal("Sewer_entrance", "", "Portal (9)"),
        //            new TunicPortal("Overworld Interiors", "house", "Portal (3)"),
        //            new TunicPortal("Furnace", "gyro_upper_east", "Portal (1)")
        //        }
        //    },
        //    {
        //        "Sword Cave",
        //        new List<TunicPortal>
        //        {
        //            new TunicPortal("Overworld Redux", "", "Portal")
        //        }
        //    },
        //    {
        //        "Ruins Passage",
        //        new List<TunicPortal>
        //        {
        //            new TunicPortal("Overworld Redux", "east", "Portal (9)"),
        //            new TunicPortal("Overworld Redux", "west", "Portal (8)")
        //        }
        //    },
        //};

        public static void ScenePortal_DepartToScene_PrefixPatch(MonoBehaviour coroutineRunner, bool whiteout, float transitionDuration, string destinationSceneName, string id, bool pauseTime, float delay)
        {
            Logger.LogInfo("AAAAAAAAAAHHHHHHHHHHHHHHHHHHHHHHHHHHHHH");
            if (destinationSceneName == "Swamp Redux 2" && id == "conduit")
            {
                Logger.LogInfo("AAAAAAAAAAHHHHHHHHHHHHHHHHHHHHHHHHHHHHH");
            };
            //var Portals = Resources.FindObjectsOfTypeAll<ScenePortal>();
            //foreach (var portal in Portals)
            //{
            //    if (portal.FullID == "Overworld Redux_conduit")
            //    {
            //        portal.optionalIDToSpawnAt = "wall";
            //    }
            //    if (portal.FullID == "Swamp Redux 2_conduit")
            //    {
            //        portal.optionalIDToSpawnAt = "conduit";
            //    }
            //    if (portal.FullID == "Overworld Redux_wall")
            //    {
            //        portal.optionalIDToSpawnAt = "conduit";
            //    }
            //    if (portal.FullID == "Swamp Redux 2_wall")
            //    {
            //        portal.optionalIDToSpawnAt = "wall";
            //    }
            // compare this list of portals to one created to assign the tags we want after randomizing the portals
            // then change the destination and destinationtag to match
        }
            // It makes the most sense to just straight number all of the portal tags
            // that way like numbers can match up and be two sides of the same portal

            // Resources.FindObjectsOfTypeAll<ScenePortal>()
            //foreach (KeyValuePair<string, List<TunicPortal>> RegionGroup in PortalList)
            //{
            //    string RegionName = RegionGroup.Key;
            //}
            // string TestString = "ScenePortal.DepartToScene(MonoBehaviour coroutineRunner, bool whiteout, float transitionDuration, string id, bool pauseTime, float delay"
    }
}
