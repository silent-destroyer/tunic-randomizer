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
        public Queue<(string, int)> paramsToSetRealtime;
        public static MusicShuffler instance;
        public float TimeSinceMusicStart = 0f;

        public void Start() {
            instance = this;
            paramsToSet = new Queue<(string, int)>();
            paramsToSetRealtime = new Queue<(string, int)>();
        }

        public void Update() {
            if (Time.time > TimeSinceMusicStart + 1.5f) {
                if (paramsToSet.Count > 0) {
                    (string, int) param = paramsToSet.Dequeue();
                    MusicManager.SetParam(param.Item1, param.Item2);
                }
            }

            if (Time.realtimeSinceStartup > TimeSinceMusicStart + 2f) {
                if (paramsToSetRealtime.Count > 0) {
                    (string, int) param = paramsToSetRealtime.Dequeue();
                    MusicManager.SetParam(param.Item1, param.Item2);
                }
            }
        }

        public static void PlayMusicOnLoad_Start_PostfixPatch(PlayMusicOnLoad __instance) {
            if (TunicRandomizer.Settings.MusicShuffle) {
                foreach(AudioParamTrigger trigger in GameObject.FindObjectsOfType<AudioParamTrigger>()) {
                    trigger.enabled = false;
                }
                List<string> songOptions = Tracks.Keys.Where(track => TunicRandomizer.Settings.MusicToggles[track]).ToList();
                if (songOptions.Count == 0) {
                    return;
                }
                System.Random random;
                if (TunicRandomizer.Settings.SeededMusic && __instance.gameObject.scene.name != "TitleScreen") {
                    random = new System.Random(SaveFile.GetInt($"randomizer enemy seed {__instance.gameObject.scene.name}"));
                } else {
                    random = new System.Random();
                }

                string trackName = songOptions[random.Next(songOptions.Count)];
                MusicShuffler.instance.trackToShuffle = trackName;
                MusicShuffler.instance.paramsToSet.Clear();
                __instance.track = Tracks[trackName];
                if (MusicManager.playingEventRef.Guid.ToString() == Tracks[trackName].Guid.ToString()) {
                    MusicManager.StopImmediate();
                    MusicManager.PlayNewTrackIfDifferent(Tracks[trackName]);
                }
                if (TrackParams.ContainsKey(trackName)) {
                    foreach ((string, int) param in TrackParams[trackName]) {
                        MusicShuffler.instance.paramsToSet.Enqueue(param);
                    }
                }
                if (trackName == "Cube Cave" && GameObject.FindObjectOfType<RotatingCubeClue>() == null) {
                    GameObject cube = new GameObject("cube for music track");
                    cube.AddComponent<RotatingCubeClue>();
                    cube.GetComponent<RotatingCubeClue>().sequence = "rrrruuuurrruuurruuru";
                    cube.transform.parent = __instance.transform;
                }
                MusicShuffler.instance.TimeSinceMusicStart = Time.time;
                TunicLogger.LogInfo("playing track " + __instance.track.Guid.ToString() + " " + trackName);
            }
        }

        public static void PlayTrack(string trackName, EventReference track) {
            if (track.Guid.ToString() == MusicManager.playingEventRef.Guid.ToString()) {
                MusicManager.StopImmediate();
            } else {
                MusicManager.Stop();
            }
            MusicManager.PlayNewTrackIfDifferent(track);
            MusicShuffler.instance.TimeSinceMusicStart = Time.realtimeSinceStartup;
            if (MusicShuffler.TrackParams.ContainsKey(trackName)) {
                foreach ((string, int) param in MusicShuffler.TrackParams[trackName]) {
                    MusicShuffler.instance.paramsToSetRealtime.Enqueue(param);
                }
            }
            if (trackName == "Cube Cave" && GameObject.FindObjectOfType<RotatingCubeClue>() == null && GameObject.FindObjectOfType<PlayMusicOnLoad>() != null) {
                GameObject cube = new GameObject("cube for music track");
                cube.AddComponent<RotatingCubeClue>();
                cube.GetComponent<RotatingCubeClue>().sequence = "rrrruuuurrruuurruuru";
                cube.transform.parent = GameObject.FindObjectOfType<PlayMusicOnLoad>().transform;
            }
        }

        public static void MusicManager_PlayCuedTrack_PostfixPatch(ref string paramString, ref int value) {
        }

        public static Dictionary<string, EventReference> Tracks = new Dictionary<string, EventReference>() {
            { "Title Screen", CreateEventReference("4fe73372-6fde-463e-9610-128523c3fb2c") },
            { "Overworld East", CreateEventReference("017d5f1e-bd27-468b-abd6-7fdef923f0aa") },
            { "Overworld West", CreateEventReference("017d5f1e-bd27-468b-abd6-7fdef923f0aa") },
            { "Overworld Subarea", CreateEventReference("017d5f1e-bd27-468b-abd6-7fdef923f0aa") },
            { "Intro Cutscene Area", CreateEventReference("077841ff-76f6-445a-9a4e-2f79a43c4b0f") },
            { "East Forest", CreateEventReference("89e5d635-fa19-4740-9264-d11e065f24c4") },
            { "Guard Captain", CreateEventReference("ec13435f-fba0-42e7-8278-d990f2b68f2b") },
            { "Resurrection", CreateEventReference("b733e6d5-eef5-49b4-9366-ff8d808c7cc2") },
            { "Shop", CreateEventReference("efd407da-eca0-4d4e-9d3a-088708245e9d") },
            { "Old House", CreateEventReference("16b40cc0-d208-45be-a4bf-0f88d450331f") },
            { "Changing Room", CreateEventReference("cca553ae-48bb-401b-bd67-970273469fb8") },
            { "Hourglass Cave", CreateEventReference("b952baa7-7650-42b1-8ce7-a0bbbc7f2c48") },
            { "Caustic Lights", CreateEventReference("31c4fbbd-b9ee-4b09-a90a-890960bd3218") },
            { "Cube Cave", CreateEventReference("5fc4f540-24c1-4fa6-b5fc-eafe34e5fc7a") },
            { "Secret Gathering Place", CreateEventReference("b7f9dae0-180c-4f6c-8c51-9ae776f9f1a4") },
            { "Beneath the Well", CreateEventReference("c604f3e9-9e43-4e4d-90f8-ac083bfbf3c5") },
            { "West Furnace", CreateEventReference("521eb8bd-41fb-4d6b-a768-057484cae6ad") },
            { "Dark Tomb", CreateEventReference("2ec4f10f-0411-47b2-b7d2-8cf7b90915a4") },
            { "West Garden", CreateEventReference("a66bad80-45fa-41a4-a3a3-2a324b053df9") },
            { "Garden Knight", CreateEventReference("17d077e6-339e-429f-aed1-b1265e1765b6") },
            { "Sealed Temple", CreateEventReference("1e9ff9c7-c594-4323-948d-dd1673c83af3") },
            { "Far Shore", CreateEventReference("d3cdded7-01d4-470f-815f-44da75c006b5") },
            { "Fortress Courtyard", CreateEventReference("bc9a21da-a3bd-4c95-bdd0-cda8cb2c5a75") },
            { "Beneath the Vault", CreateEventReference("11dec94a-4683-4130-95c9-cdedd7796b8d") },
            { "Eastern Vault Fortress", CreateEventReference("57f6adc6-e38b-4fb0-8939-b9d0db907ee7") },
            { "Fortress Leaf Piles", CreateEventReference("194c4ddb-576d-426a-bdc5-f643421be010") },
            { "Siege Engine", CreateEventReference("6b50e51c-ef86-4073-a688-c72bc2fdcd10") },
            { "Ruined Atoll", CreateEventReference("a1564c7c-c6c7-4bee-874b-6ed9c13ce9dd") },
            { "Frog's Domain", CreateEventReference("1e666263-aca1-49ab-a5b2-8730ef0b2e9f") },
            { "Library Exterior", CreateEventReference("cd23eb08-387f-422e-8ea2-e7adef2e58fb") },
            { "Library Interior", CreateEventReference("32cfeb38-3fd4-435c-91fa-0d1b24e156dd") },
            { "Librarian", CreateEventReference("f81317c6-0b17-4532-b4df-8cee40efb48f") },
            { "Quarry Entrance", CreateEventReference("ac3dcf2b-d7bb-47a0-ab3a-52ff47d177eb") },
            { "Quarry", CreateEventReference("971a6110-1297-42a5-a1b8-e6c4a277a266") },
            { "Upper Ziggurat", CreateEventReference("bedf573b-c194-47b7-8ccd-e0c2f1f13e67") },
            { "Ziggurat Tower", CreateEventReference("80ced6fe-8a9a-41d2-ae76-d7329a281229") },
            { "Lower Ziggurat", CreateEventReference("889c2828-480a-4c8e-85b8-edc132d7f62a") },
            { "Administrators", CreateEventReference("889c2828-480a-4c8e-85b8-edc132d7f62a") },
            { "Boss Scavenger Approach", CreateEventReference("889c2828-480a-4c8e-85b8-edc132d7f62a") },
            { "Boss Scavenger", CreateEventReference("889c2828-480a-4c8e-85b8-edc132d7f62a") },
            { "Swamp", CreateEventReference("b5c30739-61f7-4b9e-a32b-afb59a74fe18") },
            { "Cathedral", CreateEventReference("736aa4f4-e742-47c5-ad43-1da341de0f3c") },
            { "Gauntlet Ambience", CreateEventReference("47cf75f3-eff4-4644-9db1-f68569778b27") },
            { "Gauntlet Fight", CreateEventReference("266d48e6-bc9e-4a3f-ae46-a845ea3cd925") },
            { "Mountain", CreateEventReference("0bfe5f44-c371-4e7f-81db-e2c16c353b53") },
            { "Mountain (Golden Path)", CreateEventReference("0bfe5f44-c371-4e7f-81db-e2c16c353b53") },
            { "Hero's Grave", CreateEventReference("e850a9d9-8bcf-4f9d-aa2c-e222cc25d9f7") },
            { "Purgatory (Secret Save)", CreateEventReference("71025492-2ca8-4f59-8db2-fe6b13d1379f") },
            { "Secret Legend", CreateEventReference("16b40cc0-d208-45be-a4bf-0f88d450331f") },
            { "Spirit Arena", CreateEventReference("02895d64-5723-4514-badb-dc4a42d4416b") },
            { "The Heir (1st Encounter)", CreateEventReference("02895d64-5723-4514-badb-dc4a42d4416b") },
            { "The Heir", CreateEventReference("02895d64-5723-4514-badb-dc4a42d4416b") },
            { "Credits A", CreateEventReference("5587b38b-32ff-4fe3-b46a-00214874cb71") },
            { "Credits B", CreateEventReference("106735eb-a4d2-49bf-a40f-5157a6a86155") },
            { "Game Over", CreateEventReference("8a1af5ad-d7ee-44c8-9ec1-4f975a44a23c") },
            { "The End", CreateEventReference("259824e8-5ebb-40bc-aa21-9600a7867c2e") },
        };

    public static Dictionary<string, List<(string, int)>> TrackParams = new Dictionary<string, List<(string, int)>>() {
            { 
                "Lower Ziggurat", new List<(string, int)>() {
                    ( "zig_3_boss_dead", 0 ),
                    ( "zig_3_fuse_closed", 0 ),
                    ( "fuse_expanse_already_seen", 0 ),
                    ( "boss_aggroed", 0 ),
                    ( "zig_3_boss_dead", 0 ),
                    ( "zig_3_fuse_closed", 0 ),
                    ( "fuse_expanse_already_seen", 0 ),
                    ( "boss_aggroed", 0 ),
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
                    ( "zig_3_boss_dead", 0 ),
                    ( "zig_3_fuse_closed", 0 ),
                    ( "fuse_expanse_already_seen", 0 ),
                    ( "boss_aggroed", 0 ),
                    ( "zig_3_boss_dead", 0 ),
                    ( "zig_3_fuse_closed", 0 ),
                    ( "fuse_expanse_already_seen", 0 ),
                    ( "boss_aggroed", 0 ),
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
                    ( "zig_3_boss_dead", 0 ),
                    ( "zig_3_fuse_closed", 0 ),
                    ( "fuse_expanse_already_seen", 0 ),
                    ( "boss_aggroed", 0 ),
                    ( "zig_3_boss_dead", 0 ),
                    ( "zig_3_fuse_closed", 0 ),
                    ( "fuse_expanse_already_seen", 0 ),
                    ( "boss_aggroed", 0 ),
                    ( "zig_3_miniboss_dead", 0),
                    ( "fuse_expanse_already_seen", 1 ),
                    ( "fuse_expanse_boss_approach", 1),
                    ( "zig_3_location_index", 2 ),
                    ( "zig_3_location_index", 3 ),
                    ( "boss_layerB_volume", 1 ),
                    ( "zig_boss_layerB_volume", 1 ),
                    ( "boss_aggroed", 0 ),
                    ( "zig_3_boss_dead", 0 ),
                    ( "zig_3_fuse_closed", 0 ),
                    ( "fuse_expanse_already_seen", 0 ),
                    ( "boss_aggroed", 0 ),
                    ( "zig_3_boss_dead", 0 ),
                    ( "zig_3_fuse_closed", 0 ),
                    ( "fuse_expanse_already_seen", 0 ),
                    ( "boss_aggroed", 0 ),
                    ( "zig_3_miniboss_dead", 0),
                    ( "fuse_expanse_already_seen", 1 ),
                    ( "fuse_expanse_boss_approach", 1),
                    ( "zig_3_location_index", 2 ),
                    ( "zig_3_location_index", 3 ),
                    ( "boss_layerB_volume", 1 ),
                    ( "zig_boss_layerB_volume", 1 ),
                    ( "boss_aggroed", 0 ),
                    ( "zig_3_boss_dead", 0 ),
                    ( "zig_3_fuse_closed", 0 ),
                    ( "fuse_expanse_already_seen", 0 ),
                    ( "boss_aggroed", 0 ),
                    ( "zig_3_boss_dead", 0 ),
                    ( "zig_3_fuse_closed", 0 ),
                    ( "fuse_expanse_already_seen", 0 ),
                    ( "boss_aggroed", 0 ),
                    ( "zig_3_miniboss_dead", 0),
                    ( "fuse_expanse_already_seen", 1 ),
                    ( "fuse_expanse_boss_approach", 1),
                    ( "zig_3_location_index", 2 ),
                    ( "zig_3_location_index", 3 ),
                    ( "boss_layerB_volume", 1 ),
                    ( "zig_boss_layerB_volume", 1 ),
                    ( "boss_aggroed", 0 ),
                }
            },
            {
                "Boss Scavenger", new List<(string, int)>() {
                    ( "zig_3_boss_dead", 0 ),
                    ( "zig_3_fuse_closed", 0 ),
                    ( "fuse_expanse_already_seen", 0 ),
                    ( "boss_aggroed", 0 ),
                    ( "zig_3_boss_dead", 0 ),
                    ( "zig_3_fuse_closed", 0 ),
                    ( "fuse_expanse_already_seen", 0 ),
                    ( "boss_aggroed", 0 ),
                    ( "boss_aggroed", 0 ),
                    ( "fuse_expanse_already_seen", 1 ),
                    ( "fuse_expanse_boss_approach", 1),
                    ( "zig_3_location_index", 2 ),
                    ( "zig_3_location_index", 3 ),
                    ( "boss_layerB_volume", 1 ),
                    ( "zig_boss_layerB_volume", 1 ),
                    ( "boss_aggroed", 1 ),
                    ( "boss_aggroed", 1 ),
                    ( "boss_aggroed", 1 ),
                    ( "boss_aggroed", 1 ),
                    ( "zig_3_boss_dead", 0 ),
                    ( "zig_3_fuse_closed", 0 ),
                    ( "fuse_expanse_already_seen", 0 ),
                    ( "boss_aggroed", 0 ),
                    ( "zig_3_boss_dead", 0 ),
                    ( "zig_3_fuse_closed", 0 ),
                    ( "fuse_expanse_already_seen", 0 ),
                    ( "boss_aggroed", 0 ),
                    ( "boss_aggroed", 0 ),
                    ( "fuse_expanse_already_seen", 1 ),
                    ( "fuse_expanse_boss_approach", 1),
                    ( "zig_3_location_index", 2 ),
                    ( "zig_3_location_index", 3 ),
                    ( "boss_layerB_volume", 1 ),
                    ( "zig_boss_layerB_volume", 1 ),
                    ( "boss_aggroed", 1 ),
                    ( "boss_aggroed", 1 ),
                    ( "boss_aggroed", 1 ),
                    ( "boss_aggroed", 1 ),
                    ( "zig_3_boss_dead", 0 ),
                    ( "zig_3_fuse_closed", 0 ),
                    ( "fuse_expanse_already_seen", 0 ),
                    ( "boss_aggroed", 0 ),
                    ( "zig_3_boss_dead", 0 ),
                    ( "zig_3_fuse_closed", 0 ),
                    ( "fuse_expanse_already_seen", 0 ),
                    ( "boss_aggroed", 0 ),
                    ( "boss_aggroed", 0 ),
                    ( "fuse_expanse_already_seen", 1 ),
                    ( "fuse_expanse_boss_approach", 1),
                    ( "zig_3_location_index", 2 ),
                    ( "zig_3_location_index", 3 ),
                    ( "boss_layerB_volume", 1 ),
                    ( "zig_boss_layerB_volume", 1 ),
                    ( "boss_aggroed", 1 ),
                    ( "boss_aggroed", 1 ),
                    ( "boss_aggroed", 1 ),
                    ( "boss_aggroed", 1 ),
                }
            },
            {
                "Overworld East", new List<(string, int)>() {
                    ( "overworld_layer_1", 1 ),
                    ( "overworld_layer_2", 0 ),
                    ( "overworld_layer_3", 0 ),
                }
            },
            {
                "Overworld West", new List<(string, int)>() {
                    ( "overworld_layer_1", 0 ),
                    ( "overworld_layer_2", 1 ),
                    ( "overworld_layer_3", 0 ),
                }
            },
            {
                "Overworld Subarea", new List<(string, int)>() {
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
                "Secret Legend", new List<(string, int)>() {
                    ( "overworldInteriors_location", 1 ),
                    ( "overworldInteriors_trophyRoom", 1 ),
                    ( "overworldInteriors_oldHouse", 0 ),
                    ( "overworld_layer_1", 0 ),
                    ( "overworld_layer_2", 0 ),
                    ( "overworld_layer_3", 1 ),
                }
            },
            {
                "The Heir", new List<(string, int)>() {
                    ( "foxgod_phase", 0 ),
                    ( "foxgod_phase", 1 ),
                }
            },
            {
                "The Heir (1st Encounter)", new List<(string, int)>() {
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
                "Mountain (Golden Path)", new List<(string, int)>() {
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
                "Upper Ziggurat", new List<(string, int)>() {
                    ( "zig1_elevator_finish", 1 ),
                }
            },
            {
                "Beneath the Vault", new List<(string, int)>() {
                    ( "fortressBasement_musicBegin", 1 ),
                }
            },
            {
                "Fortress Courtyard", new List<(string, int)>() {
                    ( "layer_insideWalls", 0 ),
                    ( "layer_outsideWalls", 1 ),
                    ( "layer_prefuse", 1 ),
                    ( "inside_the_walls", 1 ),
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
