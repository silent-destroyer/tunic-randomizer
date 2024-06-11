using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using FMODUnity;

namespace TunicRandomizer {
    public class MusicShuffler : MonoBehaviour {

        public string trackToShuffle = "";
        public Queue<(string, int)> paramsToSet;
        public static MusicShuffler instance;
        public float TimeSinceMusicStart = 0f;

        public void Start() {
            instance = this;
            paramsToSet = new Queue<(string, int)>();
        }

        public void Update() {
            if (Time.time > TimeSinceMusicStart + 1f) {
                if (paramsToSet.Count > 0) {
                    (string, int) param = paramsToSet.Dequeue();
                    MusicManager.SetParam(param.Item1, param.Item2);
                }
            }    
        }

        public static void PlayMusicOnLoad_Start_PostfixPatch(PlayMusicOnLoad __instance) {
            if (TunicRandomizer.Settings.MusicShuffle) {
                List<string> songOptions = Tracks.Keys.Where(t => t != MusicShuffler.instance.trackToShuffle).ToList();
/*                songOptions = new List<string>() {
                    "Guard Captain", "Gauntlet Fight"
                };*/
                string trackName = songOptions[new System.Random().Next(songOptions.Count)];
                MusicShuffler.instance.trackToShuffle = trackName;
                MusicShuffler.instance.paramsToSet.Clear();
                __instance.track = Tracks[trackName];
                if (MusicManager.playingEventRef.Guid.ToString() == Tracks[trackName].Guid.ToString()) {
                    MusicManager.Stop();
                    MusicManager.PlayNewTrackIfDifferent(Tracks[trackName]);
                }
                if (TrackParams.ContainsKey(trackName)) {
                    foreach ((string, int) param in TrackParams[trackName]) {
                        MusicShuffler.instance.paramsToSet.Enqueue(param);
                    }
                }
                MusicShuffler.instance.TimeSinceMusicStart = Time.time;
                TunicLogger.LogInfo("playing track " + __instance.track.Guid.ToString() + " " + trackName);
            }
        }

        public static void MusicManager_PlayCuedTrack_PostfixPatch(ref string paramString, ref int value) {
            TunicLogger.LogInfo("Music Manager setparam " + paramString + " to " + value);
        }

        public static Dictionary<string, EventReference> Tracks = new Dictionary<string, EventReference>() {
            { "TitleScreen", CreateEventReference("4fe73372-6fde-463e-9610-128523c3fb2c") },
            //{ "Sword Cave", CreateEventReference("017d5f1e-bd27-468b-abd6-7fdef923f0aa") },
            //{ "Ruined Shop", CreateEventReference("017d5f1e-bd27-468b-abd6-7fdef923f0aa") },
            { "Town Basement", CreateEventReference("b952baa7-7650-42b1-8ce7-a0bbbc7f2c48") },
            //{ "Ruins Passage", CreateEventReference("017d5f1e-bd27-468b-abd6-7fdef923f0aa") },
            //{ "Mountaintop", CreateEventReference("0bfe5f44-c371-4e7f-81db-e2c16c353b53") },
            //{ "Forest Boss Room", CreateEventReference("3656fc56-77ff-4482-8a30-7ce0ca203f80") },
            //{ "Sword Access", CreateEventReference("89e5d635-fa19-4740-9264-d11e065f24c4") },
            { "Fortress Main", CreateEventReference("57f6adc6-e38b-4fb0-8939-b9d0db907ee7") },
            //{ "Fortress Basement", CreateEventReference("11dec94a-4683-4130-95c9-cdedd7796b8d") },
            { "Fortress Courtyard", CreateEventReference("bc9a21da-a3bd-4c95-bdd0-cda8cb2c5a75") }, // Needs work
            { "Dusty", CreateEventReference("194c4ddb-576d-426a-bdc5-f643421be010") }, // Needs work
            // { "Fortress Arena", CreateEventReference("3656fc56-77ff-4482-8a30-7ce0ca203f80") },
            // { "Ziggurat_Arena", CreateEventReference("b1c8230c-fdc7-4709-9655-e65906ccb778") },
            { "Library", CreateEventReference("32cfeb38-3fd4-435c-91fa-0d1b24e156dd") },
            //{ "Library Hall", CreateEventReference("32cfeb38-3fd4-435c-91fa-0d1b24e156dd") },
            //{ "Library Rotunda", CreateEventReference("32cfeb38-3fd4-435c-91fa-0d1b24e156dd") },
            //{ "Quarry", CreateEventReference("971a6110-1297-42a5-a1b8-e6c4a277a266") },
            //{ "Monastery", CreateEventReference("971a6110-1297-42a5-a1b8-e6c4a277a266") },
            { "Darkwoods Tunnel", CreateEventReference("ac3dcf2b-d7bb-47a0-ab3a-52ff47d177eb") },
            { "Temple", CreateEventReference("1e9ff9c7-c594-4323-948d-dd1673c83af3") },
            { "Overworld Redux East", CreateEventReference("017d5f1e-bd27-468b-abd6-7fdef923f0aa") },
            { "Overworld Redux West", CreateEventReference("017d5f1e-bd27-468b-abd6-7fdef923f0aa") },
            { "Overworld Redux Subarea", CreateEventReference("017d5f1e-bd27-468b-abd6-7fdef923f0aa") },
            { "Old House", CreateEventReference("16b40cc0-d208-45be-a4bf-0f88d450331f") },
            { "Sewer", CreateEventReference("c604f3e9-9e43-4e4d-90f8-ac083bfbf3c5") },
            //{ "Library Arena", CreateEventReference("3656fc56-77ff-4482-8a30-7ce0ca203f80") },
            //{ "Crypt", CreateEventReference("c604f3e9-9e43-4e4d-90f8-ac083bfbf3c5") },
            //{ "Town_FiligreeRoom", CreateEventReference("017d5f1e-bd27-468b-abd6-7fdef923f0aa") },
            { "Archipelagos Redux", CreateEventReference("a66bad80-45fa-41a4-a3a3-2a324b053df9") },
            { "Atoll Redux", CreateEventReference("a1564c7c-c6c7-4bee-874b-6ed9c13ce9dd") },
            //{ "Frog Stairs", CreateEventReference("a1564c7c-c6c7-4bee-874b-6ed9c13ce9dd") },
            { "Library Exterior", CreateEventReference("cd23eb08-387f-422e-8ea2-e7adef2e58fb") },
            //{ "Void", CreateEventReference("3656fc56-77ff-4482-8a30-7ce0ca203f80") },
            //{ "Forest Belltower", CreateEventReference("3656fc56-77ff-4482-8a30-7ce0ca203f80") },
            { "Playable Intro", CreateEventReference("077841ff-76f6-445a-9a4e-2f79a43c4b0f") },
            { "Resurrection", CreateEventReference("b733e6d5-eef5-49b4-9366-ff8d808c7cc2") },
            { "Transit", CreateEventReference("d3cdded7-01d4-470f-815f-44da75c006b5") },
            //{ "archipelagos_house", CreateEventReference("a66bad80-45fa-41a4-a3a3-2a324b053df9") },
            { "ziggurat2020_1", CreateEventReference("bedf573b-c194-47b7-8ccd-e0c2f1f13e67") },
            { "ziggurat2020_2", CreateEventReference("80ced6fe-8a9a-41d2-ae76-d7329a281229") },
            { "Lower Ziggurat", CreateEventReference("889c2828-480a-4c8e-85b8-edc132d7f62a") },
            { "Administrators", CreateEventReference("889c2828-480a-4c8e-85b8-edc132d7f62a") },
            { "Boss Scavenger Approach", CreateEventReference("889c2828-480a-4c8e-85b8-edc132d7f62a") },
            { "Boss Scavenger", CreateEventReference("889c2828-480a-4c8e-85b8-edc132d7f62a") },
            //{ "ziggurat2020_0", CreateEventReference("3656fc56-77ff-4482-8a30-7ce0ca203f80") },
            //{ "ziggurat2020_FTRoom", CreateEventReference("3656fc56-77ff-4482-8a30-7ce0ca203f80") },
            //{ "Fortress East", CreateEventReference("89e5d635-fa19-4740-9264-d11e065f24c4") },
            //{ "Fortress Reliquary", CreateEventReference("89e5d635-fa19-4740-9264-d11e065f24c4") },
            { "Waterfall", CreateEventReference("b7f9dae0-180c-4f6c-8c51-9ae776f9f1a4") },
            { "Overworld Cave", CreateEventReference("31c4fbbd-b9ee-4b09-a90a-890960bd3218") },
            //{ "Sewer_Boss", CreateEventReference("c604f3e9-9e43-4e4d-90f8-ac083bfbf3c5") },
            { "frog cave main", CreateEventReference("1e666263-aca1-49ab-a5b2-8730ef0b2e9f") },
            { "East Forest Redux", CreateEventReference("89e5d635-fa19-4740-9264-d11e065f24c4") },
            //{ "East Forest Redux Interior", CreateEventReference("89e5d635-fa19-4740-9264-d11e065f24c4") },
            //{ "East Forest Redux Laddercave", CreateEventReference("89e5d635-fa19-4740-9264-d11e065f24c4") },
            { "Shop", CreateEventReference("efd407da-eca0-4d4e-9d3a-088708245e9d") },
            { "Furnace", CreateEventReference("521eb8bd-41fb-4d6b-a768-057484cae6ad") },
            //{ "Windmill", CreateEventReference("017d5f1e-bd27-468b-abd6-7fdef923f0aa") },
            { "Swamp Redux 2", CreateEventReference("b5c30739-61f7-4b9e-a32b-afb59a74fe18") },
            { "Quarry Redux", CreateEventReference("971a6110-1297-42a5-a1b8-e6c4a277a266") },
            { "Cathedral Arena", CreateEventReference("47cf75f3-eff4-4644-9db1-f68569778b27") },
            { "RelicVoid", CreateEventReference("e850a9d9-8bcf-4f9d-aa2c-e222cc25d9f7") },
            { "Spirit Arena", CreateEventReference("02895d64-5723-4514-badb-dc4a42d4416b") },
            { "Crypt Redux", CreateEventReference("2ec4f10f-0411-47b2-b7d2-8cf7b90915a4") },
            //{ "ShopSpecial", CreateEventReference("efd407da-eca0-4d4e-9d3a-088708245e9d") },
            { "CubeRoom", CreateEventReference("5fc4f540-24c1-4fa6-b5fc-eafe34e5fc7a") },
            //{ "PatrolCave", CreateEventReference("017d5f1e-bd27-468b-abd6-7fdef923f0aa") },
            //{ "Maze Room", CreateEventReference("3656fc56-77ff-4482-8a30-7ce0ca203f80") },
            { "Cathedral Redux", CreateEventReference("736aa4f4-e742-47c5-ad43-1da341de0f3c") },
            { "Mountain", CreateEventReference("0bfe5f44-c371-4e7f-81db-e2c16c353b53") },
            { "Golden Path", CreateEventReference("0bfe5f44-c371-4e7f-81db-e2c16c353b53") },
            { "Changing Room", CreateEventReference("cca553ae-48bb-401b-bd67-970273469fb8") },
            //{ "DungeonAudioTest", CreateEventReference("017d5f1e-bd27-468b-abd6-7fdef923f0aa") },
            { "Purgatory", CreateEventReference("71025492-2ca8-4f59-8db2-fe6b13d1379f") },
            //{ "g_elements", CreateEventReference("077841ff-76f6-445a-9a4e-2f79a43c4b0f") },
            { "Guard Captain", CreateEventReference("ec13435f-fba0-42e7-8278-d990f2b68f2b") },
            { "Garden Knight", CreateEventReference("17d077e6-339e-429f-aed1-b1265e1765b6") },
            { "Siege Engine", CreateEventReference("6b50e51c-ef86-4073-a688-c72bc2fdcd10") },
            { "Librarian", CreateEventReference("f81317c6-0b17-4532-b4df-8cee40efb48f") },
            { "Gauntlet Fight", CreateEventReference("266d48e6-bc9e-4a3f-ae46-a845ea3cd925") },
            { "Credits Good", CreateEventReference("5587b38b-32ff-4fe3-b46a-00214874cb71") },
            { "Credits Bad", CreateEventReference("106735eb-a4d2-49bf-a40f-5157a6a86155") },
            { "Game Over", CreateEventReference("8a1af5ad-d7ee-44c8-9ec1-4f975a44a23c") },
            { "The End", CreateEventReference("259824e8-5ebb-40bc-aa21-9600a7867c2e") },
            { "The Heir Phase 1", CreateEventReference("02895d64-5723-4514-badb-dc4a42d4416b") },
            { "The Heir Phase 2", CreateEventReference("02895d64-5723-4514-badb-dc4a42d4416b") },
        };

    public static Dictionary<string, List<(string, int)>> TrackParams = new Dictionary<string, List<(string, int)>>() {
            { 
                "Lower Ziggurat", new List<(string, int)>() {
                    ( "zig_3_location_index", 0 ),
                    ( "zig_3_location_index", 0 ),
                    ( "zig_3_location_index", 0 ),
                    ( "zig_3_location_index", 0 ),
                    ( "zig_3_location_index", 0 ),
                    ( "zig_3_location_index", 0 ),
                    ( "zig_3_location_index", 0 ),
                    ( "zig_3_location_index", 0 ),
                    ( "zig_3_location_index", 0 ),
                    ( "zig_3_location_index", 0 ),
                    ( "zig_3_location_index", 0 ),
                    ( "zig_3_location_index", 0 ),
                    ( "zig_3_location_index", 0 ),
                    ( "zig_3_location_index", 0 ),
                    ( "overworld_layer_2", 1 ),
                    ( "overworld_layer_1", 0 ),
                }
            },
            {
                "Administrators", new List<(string, int)>() {
                    ( "zig_3_location_index", 0 ),
                    ( "zig_3_location_index", 0 ),
                    ( "zig_3_location_index", 0 ),
                    ( "zig_3_location_index", 0 ),
                    ( "zig_3_location_index", 0 ),
                    ( "zig_3_location_index", 0 ),
                    ( "zig_3_location_index", 0 ),
                    ( "zig_3_location_index", 1 ),
                }
            },
            {
                "Boss Scavenger Approach", new List<(string, int)>() {
                    ( "zig_3_miniboss_dead", 0),
                    ( "fuse_expanse_boss_approach", 0),
                    ( "zig_3_location_index", 2 ),
                    ( "zig_3_location_index", 3 ),
                    ( "boss_layerB_volume", 1 ),
                    ( "boss_aggroed", 0 ),
                    ( "zig_3_location_index", 2 ),
                    ( "zig_3_location_index", 3 ),
                }
            },
            {
                "Boss Scavenger", new List<(string, int)>() {
                    ( "boss_aggroed", 0 ),
                    ( "zig_3_location_index", 2 ),
                    ( "zig_3_location_index", 3 ),
                    ( "boss_layerB_volume", 1 ),
                    ( "boss_aggroed", 1 ),
                }
            },
            {
                "Overworld Redux East", new List<(string, int)>() {
                    ( "overworld_layer_1", 1 ),
                    ( "overworld_layer_2", 0 ),
                    ( "overworld_layer_3", 0 ),
                }
            },
            {
                "Overworld Redux West", new List<(string, int)>() {
                    ( "overworld_layer_1", 0 ),
                    ( "overworld_layer_2", 1 ),
                    ( "overworld_layer_3", 0 ),
                }
            },
            {
                "Overworld Redux Subarea", new List<(string, int)>() {
                    ( "overworld_layer_1", 0 ),
                    ( "overworld_layer_2", 0 ),
                    ( "overworld_layer_3", 1 ),
                }
            },
            {
                "Old House", new List<(string, int)>() {
                    ( "overworldInteriors_oldHouse", 1 ),
                    ( "overworld_layer_1", 0 ),
                    ( "overworld_layer_2", 0 ),
                    ( "overworld_layer_3", 1 ),
                }
            },
            {
                "The Heir Phase 1", new List<(string, int)>() {
                    ( "foxgod_phase", 0 ),
                    ( "foxgod_phase", 1 ),
                }
            },
            {
                "The Heir Phase 2", new List<(string, int)>() {
                    ( "foxgod_phase", 1 ),
                    ( "foxgod_phase", 1 ),
                    ( "foxgod_phase", 1 ),
                    ( "foxgod_phase", 1 ),
                    ( "foxgod_phase", 1 ),
                    ( "foxgod_phase", 1 ),
                    ( "foxgod_phase", 2 ),
                    ( "foxgod_phase", 2 ),
                    ( "foxgod_phase", 2 ),
                    ( "foxgod_phase", 2 ),
                }
            },
            {
                "Golden Path", new List<(string, int)>() {
                    ( "mountain_door_state", 1 ),
                    ( "mountain_door_state", 2 ),
                    ( "mountain_door_state", 2 ),
                    ( "mountain_door_state", 2 ),
                    ( "mountain_door_state", 2 ),
                    ( "mountain_door_state", 2 ),
                    ( "mountain_door_state", 2 ),
                    ( "mountain_door_state", 2 ),
                    ( "mountain_door_state", 2 ),
                }
            },
            {
                "ziggurat2020_1", new List<(string, int)>() {
                    ( "zig1_elevator_finish", 1 ),
                }
            }
        };

        public static EventReference CreateEventReference(string Guid) {
            EventReference e = new EventReference();
            e.Guid = FMOD.GUID.Parse(Guid);
            return e;
        }
    }
}
