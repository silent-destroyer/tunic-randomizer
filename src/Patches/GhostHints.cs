using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BepInEx.Logging;


namespace TunicRandomizer {


public class GhostHints {

        public class HintGhost {
            public string Name;
            public string SceneName;
            public Vector3 Position;
            public Quaternion Rotation;
            public NPC.NPCAnimState AnimState;
            public string Dialogue;
            public string TrunicDialogue;
            public string Hint;
            public string TrunicHint;

            public HintGhost() { }

            public HintGhost(string name, string sceneName, Vector3 position, Quaternion rotation, NPC.NPCAnimState animState, string dialogue) {
                Name = name;
                SceneName = sceneName;
                Position = position;
                Rotation = rotation;
                Dialogue = dialogue;
                TrunicDialogue = dialogue;
                AnimState = animState;
                Hint = "";
                TrunicHint = "";
            }

            public HintGhost(string name, string sceneName, Vector3 position, Quaternion rotation, NPC.NPCAnimState animState, string dialogue, string trunicDialogue) {
                Name = name;
                SceneName = sceneName;
                Position = position;
                Rotation = rotation;
                Dialogue = dialogue;
                TrunicDialogue = trunicDialogue;
                AnimState = animState;
                Hint = "";
                TrunicHint = "";
            }
        }

        private static ManualLogSource Logger = TunicRandomizer.Logger;

        public static GameObject GhostFox;

        public static List<char> Vowels = new List<char>() { 'A', 'E', 'I', 'O', 'U' };
        public static List<(string, string)> LocationHints = new List<(string, string)>();
        public static List<(string, string)> ItemHints = new List<(string, string)>();
        public static List<(string, string)> BarrenAndTreasureHints = new List<(string, string)>();
        public static (string, string) HeirHint;

        public static Dictionary<string, HintGhost> HintGhosts = new Dictionary<string, HintGhost>();

        public static Dictionary<string, string> HintableLocationIds = new Dictionary<string, string>() {
            { "1005 [Swamp Redux 2]", "FOUR SKULLS" },
            { "1006 [East Forest Redux]", "FOREST SLIME" },
            { "999 [Cathedral Arena]", "CATHEDRAL GAUNTLET" },
            { "Vault Key (Red) [Fortress Arena]", "SIEGE ENGINE" },
            { "Hexagon Green [Library Arena]", "LIBRARIAN" },
            { "Hexagon Blue [ziggurat2020_3]", "SCAVENGER BOSS" },
            { "Hexagon Red [Fortress Arena]", "VAULT KEY" },
            { "1007 [Waterfall]", "20 FAIRIES" },
            { "Well Reward (10 Coins) [Trinket Well]", "10 COIN TOSSES" },
            { "Well Reward (15 Coins) [Trinket Well]", "15 COIN TOSSES" },
            { "1011 [Dusty]", "PILES OF LEAVES" },
            { "Archipelagos Redux-(-396.3, 1.4, 42.3) [Archipelagos Redux]", "GARDEN TREE" },
            { "final [Mountaintop]", "TOP OF THE MOUNTAIN" }
        };
        
        public static List<string> HintableItemNames = new List<string>() {
            "Stick",
            "Sword",
            "Sword Progression",
            "Shotgun",
            "Shield",
            "SlowmoItem",
            "Mask",
            "Key (House)",
            "Relic - Hero Sword",
            "Relic - Hero Pendant MP",
            "Relic - Hero Water",
            "Relic - Hero Pendant HP",
            "Relic - Hero Crown",
            "Relic - Hero Pendant SP",
            "Dath Stone",
        };

        public static List<string> BarrenItemNames = new List<string>() {
            "Firecracker",
            "Ice Bomb",
            "Firebomb",
            "Pepper",
            "Ivy",
            "Bait",
            "money",
            "Piggybank L1",
            "Berry_MP",
            "Berry_HP"
        };

        public static Dictionary<string, List<HintGhost>> GhostLocations = new Dictionary<string, List<HintGhost>>() {
            { "Sword Cave", new List<HintGhost>() {
                new HintGhost("Hint Ghost Sword Cave", "Sword Cave", new Vector3(5.1151f, 0.0637f, 12.6657f), new Quaternion(0f, 0.9642988f, 0f, 0.2648164f), NPC.NPCAnimState.SIT, $"its dAnjuris too gO uhlOn, tAk #is hint:"), }
            },
            { "Far Shore", new List<HintGhost>() {
                new HintGhost("Hint Ghost Far Shore 1", "Overworld Redux", new Vector3(8.45f, 12.0833f, -204.9243f), new Quaternion(0f, 0.4226183f, 0f, -0.9063078f), NPC.NPCAnimState.IDLE, $"wAr did yoo kuhm fruhm, tInE fawks?"),
                new HintGhost("Hint Ghost Far Shore 2", "Transit", new Vector3(-18.6177f, 8.0314f, -81.6153f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, "I stoud awn #aht skwAr ahnd ehndid uhp hEr suhmhow.\nwAr igzahktlE R wE?" ) }
            },
            { "Ruined Passage", new List<HintGhost>() {
                new HintGhost("Hint Ghost Ruins Passage", "Ruins Passage", new Vector3(184.1698f, 17.3268f, 40.54981f), new Quaternion(0f, 0.9659258f, 0f, 0.2588191f), NPC.NPCAnimState.TIRED, $"nahp tIm! haw haw haw... geht it?") }
            },
            { "Windmill", new List<HintGhost>() {
                new HintGhost("Hint Ghost Windmill", "Windmill", new Vector3(-58.33329f, 54.0833f, -27.8653f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"viziti^ #uh \"SHOPKEEPER?\" doo nawt bE uhlRmd, #A R\nA frehnd.", $"viziti^ #uh $awpkEpur? doo nawt bE uhlRmd, #A R A frehnd.") }
            },
            { "Old House Back", new List<HintGhost>() {
                new HintGhost("Hint Ghost Overworld Interiors 1", "Overworld Interiors", new Vector3(11.0359f, 29.0833f, -7.3707f), new Quaternion(0f, 0.8660254f, 0f, -0.5000001f), NPC.NPCAnimState.PRAY, $"nuh%i^ wurks! mAbE #Arz suhm trik too #is dor...") }
            },
            { "Old House Front", new List<HintGhost>() {
                new HintGhost("Hint Ghost Overworld Interiors 2", "Overworld Interiors", new Vector3(-24.06613f, 27.39948f, -47.9316f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.TIRED, $"juhst fIv mor minits..."),
                new HintGhost("Hint Ghost Overworld Interiors 3", "Overworld Interiors", new Vector3(12.0368f, 21.1446f, -72.81052f), new Quaternion(0f, 0.8660254f, 0f, -0.5000001f), NPC.NPCAnimState.SIT, $"wuht R #Ez pehduhstuhlz for? doo yoo nO?") }
            },
            { "Overworld Above Ruins", new List<HintGhost>() {
               new HintGhost("Hint Ghost Overworld Above Ruins 1", "Overworld Redux", new Vector3(28.53184f, 36.0833f, -108.3734f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.IDLE, $"I wuhz hIdi^ fruhm #uh \"SLIMES,\" buht yoo dOnt louk\nlIk wuhn uhv #ehm.", $"I wuhz hIdi^ fruhm #uh slImz, buht yoo dOnt louk\nlIk wuhn uhv #ehm."),
               new HintGhost("Hint Ghost Overworld Above Ruins 2", "Overworld Redux", new Vector3(22.3667f, 27.9833f, -126.3728f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"wAr did I lEv #aht kE..."),
               new HintGhost("Hint Ghost Overworld Above Ruins 3", "Overworld Redux", new Vector3(51.20462f, 28.00694f, -129.722f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.SIT, $"I %awt #aht Jehst wuhz ehmptE. how suhspi$is.") }
            },
            { "Early Overworld Spawns", new List<HintGhost>() {
               new HintGhost("Hint Ghost Early Overworld Spawns 1", "Overworld Redux", new Vector3(-9.441f, 43.9363f, -8.4385f), new Quaternion(0f, 0.7069f, 0f, -0.7069f), NPC.NPCAnimState.SIT, $"sEld forehvur? nO... #Ar muhst bE uhnuh#ur wA..."),
               new HintGhost("Hint Ghost Early Overworld Spawns 2", "Overworld Redux", new Vector3(-34.0649f, 37.9833f, -59.2506f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.GAZE, $"sO mehnE roodli^z. Im stAi^ uhp hEr.") }
            },
            { "Inside Temple", new List<HintGhost>() {
                new HintGhost("Hint Ghost Inside Temple 1", "Temple", new Vector3(7.067f, -0.224f, 59.9285f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.IDLE, $"yur naht uh \"RUIN SEEKER,\" R yoo? mAbE yoo $oud gO\nsuhmwAr ehls.", $"yur naht uh rooin sEkur, R yoo? mAbE yoo $oud gO suhmwAr ehls."),
                new HintGhost("Hint Ghost Inside Temple 2", "Temple", new Vector3(0.9350182f, 4.076f, 133.7965f), new Quaternion(0f, 0.8660254f, 0f, 0.5f), NPC.NPCAnimState.GAZE_UP, $"yur guhnuh frE \"THE HEIR?\" iznt #aht... bahd?", $"yur guhnuh frE #uh Ar? iznt #aht... bahd?") }
            },
            { "Ruined Shop", new List<HintGhost>() {
                new HintGhost("Hint Ghost Ruined Shop 1", "Ruined Shop", new Vector3(16.5333f, 8.983299f, -45.60382f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"hehlO. wuht iz yor nAm?---...tuhnk? wuht A strAnj nAm."),
                new HintGhost("Hint Ghost Ruined Shop 2", "Ruined Shop", new Vector3(9.8111f, 8.0833f, -37.52119f), new Quaternion(0f, 0.9659258f, 0f, 0.2588191f), NPC.NPCAnimState.IDLE, $"wehl, if yur nawt bIi^ ehnE%i^..." ) }
            },
            { "West Filigree", new List<HintGhost>() {
                new HintGhost("Hint Ghost West Filigree", "Town_FiligreeRoom", new Vector3(-79.4348f, 22.0379f, -59.8104f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.PRAY, $"wow, yoo hahv #uh powur uhv #uh \"Holy Cross!\"", $"wow, yoo hahv #uh powur uhv #uh hOlE kraws!") }
            },
            { "East Filigree", new List<HintGhost>() {
                new HintGhost("Hint Ghost East Filigree", "EastFiligreeCache", new Vector3(14.3719f, 0.0167f, -8.8614f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"wAt, how did yoo Opehn #aht dOr?") }
            },
            { "Maze Room", new List<HintGhost>() {
                new HintGhost("Hint Ghost Maze Room", "Maze Room", new Vector3(3.5129f,-0.1167f,-9.4481f), new Quaternion(0f,0f,0f,1f), NPC.NPCAnimState.IDLE, $"wAt... how kahn yoo wahk in hEr? #Arz nO flOr!" ) }
            },
            { "Changing Room", new List<HintGhost>() {
                new HintGhost("Hint Ghost Changing Room 1", "Changing Room", new Vector3(14.9876f, 6.9379f, 14.6771f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.PRAY, $"doo yoo %ink #is louks goud awn mE?"),
                new HintGhost("Hint Ghost Changing Room 2", "Changing Room", new Vector3(14.9876f, 6.9379f, 14.6771f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.DANCE,  $"doo yoo nO sIlehntdistroiur? hE iz kool.") }
            },
            { "Waterfall", new List<HintGhost>() {
                new HintGhost("Hint Ghost Waterfall", "Waterfall", new Vector3(-41.13461f, 44.9833f, -0.6913f), new Quaternion(0f, 0.6755902f, 0f, -0.7372773f), NPC.NPCAnimState.IDLE, $"doo yoo nO wuht #uh fArEz R sAi^?") }
            },
            { "Hourglass Cave", new List<HintGhost>() {
                new HintGhost("Hint Ghost Hourglass Cave", "Town Basement", new Vector3(-211.3147f, 1.0833f, 35.7667f), new Quaternion(0f, 0.4226183f, 0f, 0.9063078f), NPC.NPCAnimState.GAZE, $"dOnt gO in #Ar. kahnt yoo rEd %uh sIn?") }
            },
            { "Overworld Cave", new List<HintGhost>() {
                new HintGhost("Hint Ghost Overworld Cave", "Overworld Cave", new Vector3(-88.0794f, 515.1076f, -741.0837f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"sO pritE!" ) }
            },
            { "Patrol Cave", new List<HintGhost>() {
                new HintGhost("Hint Ghost Patrol Cave 1", "PatrolCave", new Vector3(80.41302f, 46.0686f, -48.0821f), new Quaternion(0f, 0.9848078f, 0f, -0.1736482f), NPC.NPCAnimState.GAZE, $"#Arz ahlwAz uh sEkrit bEhInd #uh wahturfahl!"),
                new HintGhost("Hint Ghost Patrol Cave 2", "PatrolCave", new Vector3(72.6667f, 46.0686f, 14.9446f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.GAZE, $"klA^ klA^ klA^... duhzint hE ehvur stawp?") }
            },
            { "Cube Room", new List<HintGhost>() {
                new HintGhost("Hint Ghost Cube Room", "CubeRoom", new Vector3(326.784f, 3.0833f, 207.0065f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.DANCE, $"rIt ahnd uhp! rit ahnd uhp!") }
            },
            { "Furnace", new List<HintGhost>() {
                new HintGhost("Hint Ghost Furnace", "Furnace", new Vector3(-131.9886f, 12.0833f, -51.0197f), new Quaternion(0f, 0f, 0f, 1f), NPC.NPCAnimState.GAZE_UP, $"#Ez powur sorsehz... I dOnt truhst #ehm.") }
            },
            { "Golden Obelisk", new List<HintGhost>() {
                new HintGhost("Hint Ghost Golden Obelisk", "Overworld Redux", new Vector3(-94.5973f, 70.0937f, 36.38749f), new Quaternion(0f, 0f, 0f, 1f), NPC.NPCAnimState.FISHING, $"pEpuhl yoost too wur$ip #is. it rehprEzehnts #uh\n\"Holy Cross.\"", $"pEpuhl yoost too wur$ip #is. it rehprEzehnts #uh\nhOlE kraws.") }
            },
            { "Overworld Before Garden", new List<HintGhost>(){
                new HintGhost("Hint Ghost Overworld Before Garden", "Overworld Redux", new Vector3(-146.1464f, 11.6929f, -67.55009f), new Quaternion(0f, 0.3007058f, 0f, 0.9537169f), NPC.NPCAnimState.IDLE, "A vi$is baws blawks #uh wA too #uh behl uhp #Ar.\nbE kArfuhl, it wil kil yoo.") }
            },
            { "West Garden", new List<HintGhost>() {
                new HintGhost("Hint Ghost West Garden 1", "Archipelagos Redux", new Vector3(-290.3334f, 4.0667f, 153.9145f), new Quaternion(0f, 0.9659259f, 0f, -0.2588191f), NPC.NPCAnimState.GAZE, $"wawJ owt for tArE uhp uhhehd. hE wil trI too Jawmp yoo."),
                new HintGhost("Hint Ghost West Garden 2", "Archipelagos Redux", new Vector3(-137.9978f, 2.0781f, 150.5348f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.GAZE, $"iz #is pRt uhv suhm%i^ ehls? hmm..."),
                new HintGhost("Hint Ghost West Garden 3", "Archipelagos Redux", new Vector3(-190.6887f, 2.0667f, 126.7101f), new Quaternion(0f, 0.3826835f, 0f, -0.9238795f), NPC.NPCAnimState.FISHING, $"bE kArfuhl if yoo R gOi^ uhp #Ar. #aht mawnstur iz nO jOk."),
                new HintGhost("Hint Ghost West Garden 4", "Archipelagos Redux", new Vector3(-256.3194f, 4.1667f, 168.15f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.FISHING, $"doo #A louk fuhmilyur too yoo?") }
            },
            { "West Bell", new List<HintGhost>() {
                new HintGhost("Hint Ghost West Bell", "Overworld Redux", new Vector3(-130.929f, 40.0833f, -51.5f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.SIT, $"its sO kwIeht hEr... goud #i^ nObuhdE rA^ #aht behl.") }
            },
            { "Ice Dagger House", new List<HintGhost>() {
                new HintGhost("Hint Ghost Ice Dagger House", "archipelagos_house", new Vector3(-201.1842f, 3.1209f, 38.4875f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.PRAY, $"Is too mEt yoo!") }
            },
            { "East Belltower Lower", new List<HintGhost>() {
                new HintGhost("Hint Ghost East Belltower Lower", "Forest Belltower", new Vector3(500.9258f, 13.9394f, 63.79896f), new Quaternion(0f, 0.9659258f, 0f, 0.2588191f), NPC.NPCAnimState.SIT, $"#uh lahdur brOk ahnd #Ar R ehnehmEz owtsId, buht #is stahJoo\nawfurz sAftE.") }
            },
            { "East Belltower Upper", new List<HintGhost>() {
                new HintGhost("Hint Ghost East Belltower Upper 1", "Forest Belltower", new Vector3(500.3264f, 62.012f, 107.5831f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.GAZE, $"di^ daw^! doo yoo hahv wuht it tAks?"),
                new HintGhost("Hint Ghost East Belltower Upper 2", "Forest Belltower", new Vector3(593.9467f, 14.0052f, 84.43121f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.IDLE, $"wow... yoo did it!"),
                new HintGhost("Hint Ghost East Belltower Upper 3", "East Forest Redux Laddercave", new Vector3(159.0245f, 17.89421f, 78.52466f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.PRAY, $"#Arz uh baws uhhehd... buht hE hahz por vi&uhn.")}
            },
            { "Swamp", new List<HintGhost>() {
                new HintGhost("Hint Ghost Swamp 1", "Swamp Redux 2", new Vector3(-47f, 16.0463f, -31.3333f), new Quaternion(0f, 0f, 0f, 1f), NPC.NPCAnimState.GAZE, $"I kahn sE mI hows fruhm hEr!" ),
                new HintGhost("Hint Ghost Swamp 2", "Swamp Redux 2", new Vector3(-90.55162f, 3.0462f, 6.2667f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"suhm%i^ $oud bE hEr rIt? I dOnt rEmehmbur..." ),
                new HintGhost("Hint Ghost Swamp 3", "Swamp Redux 2", new Vector3(-100.5333f, 3.3462f, 25.0965f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"I %ink #is wuhz pRt uhv suhm%i^ ehls wuhns." ) }
            },
            { "Dark Tomb", new List<HintGhost>() {
                new HintGhost("Hint Ghost Dark Tomb 1","Crypt Redux", new Vector3(-75.8704f, 57.0833f, -56.2025f), new Quaternion(0f, 0.3826835f, 0f, -0.9238795f), NPC.NPCAnimState.GAZE, $"dRk! its sO dRk!"),
                new HintGhost("Hint Ghost Dark Tomb 2","Sewer_Boss", new Vector3(70.30289f, 9.4138f, -9.387097f), new Quaternion(0f, 0.7660444f, 0f, 0.6427876f), NPC.NPCAnimState.GAZE, $"wuht koud bE bahk #Ar?") }
            },
            { "Fortress Courtyard", new List<HintGhost>() {
                new HintGhost("Hint Ghost Fortress Courtyard 1", "Fortress Courtyard", new Vector3(-50.54346f, 0.0417f, -36.46348f), new Quaternion(0f, 0.9659259f, 0f, -0.2588191f), NPC.NPCAnimState.GAZE, $"yoo gO furst. spIdurs giv mE #uh krEps..."),
                new HintGhost("Hint Ghost Fortress Courtyard 2", "Fortress Courtyard", new Vector3(6.967727f, 0.0417f, -74.5881f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.IDLE, $"wehl wehl wehl... sawrE."),
                new HintGhost("Hint Ghost Fortress Courtyard 3", "Fortress Courtyard", new Vector3(7.299674f, 9.0417f, -89.57533f), new Quaternion(0f, 0f, 0f, 1f), NPC.NPCAnimState.FISHING, $"sO mehnE kahndlz! hahpE bur%dA!"),
                new HintGhost("Hint Ghost Fortress Courtyard 4", "Fortress Courtyard", new Vector3(11.6453f, 4.0203f, -115.355f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.GAZE, $"I woud nawt tuhJ #aht wuhn. hoo nOz wuht #Al doo wi%\n#uh powur?") }
            },
            { "Mountain Door", new List<HintGhost>() {
                new HintGhost("Hint Ghost Mountain", "Mountain", new Vector3(54.7674f, 41.5568f, 4.4282f), new Quaternion(0f, 0.3826835f, 0f, -0.9238795f), NPC.NPCAnimState.GAZE_UP, $"yoo kahn Opehn #is? uhmAzi^!") }
            },
            { "Atoll Entrance", new List<HintGhost>() {
                new HintGhost("Hint Ghost Atoll Entrance 1", "Atoll Redux", new Vector3(-3.5443f, 1.0174f, 120.0543f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"burdsaw^ iz sO rElahksi^. twEt twEt twEt!"),
                new HintGhost("Hint Ghost Atoll Entrance 2", "Atoll Redux", new Vector3(4.7f, 16.0776f, 101.9315f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"#Ez skwArz...wuht purpis doo #A surv?"),
                new HintGhost("Hint Ghost Atoll Entrance 3", "Atoll Redux", new Vector3(0.4395638f, 16.0874f, 64.47866f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.GAZE, $"tIm hahz tAkin ahtOl awn #is plAs.") }
            },
            { "Frog's Domain", new List<HintGhost>() {
                new HintGhost("Hint Ghost Frog's Domain 1", "frog cave main", new Vector3(19.7682f, 9.1943f, -23.3269f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.FISHING, $"I wuhndur wAr #uh kwehstuhgawn iz?"),
                new HintGhost("Hint Ghost Frog's Domain 2", "frog cave main", new Vector3(27.09619f, 9.2581f, -37.28336f), new Quaternion(0f, 0.5000001f, 0f, -0.8660254f), NPC.NPCAnimState.FISHING, $"$hhh. Im hIdi^ fruhm #uh frawgs.") }
            },
            { "Purgatory", new List<HintGhost>() {
                new HintGhost("Hint Ghost Purgatory", "Purgatory", new Vector3(27.1514f, 38.018f, 74.7217f), new Quaternion(0f, 0.9585385f, 0f, -0.2849632f), NPC.NPCAnimState.DANCE, $"doo yoo nO skipEO? hE brOk awl uhv #uh dorz.") }
            },
        };

        public static void InitializeGhostFox() {
            try {
                GhostFox = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<NPC>().Where(npc => npc.name == "NPC_greeter").ToList()[0].gameObject);
                GameObject.DontDestroyOnLoad(GhostFox);
                GhostFox.SetActive(false);
            } catch (Exception e) {
                Logger.LogInfo("Error initalizing ghost foxes for hints!");
            }
        }
         
        public static void SpawnHintGhosts(string SceneName) {
            foreach (HintGhost HintGhost in HintGhosts.Values) {
                if (HintGhost.SceneName == SceneName) {
                    GhostFox.GetComponent<NPC>().nPCAnimState = HintGhost.AnimState;
                    GameObject NewGhostFox = GameObject.Instantiate(GhostFox);
                    NewGhostFox.name = HintGhost.Name;
                    NewGhostFox.transform.position = HintGhost.Position;
                    NewGhostFox.transform.rotation = HintGhost.Rotation;
                    LanguageLine HintText = ScriptableObject.CreateInstance<LanguageLine>();
                    HintText.text = TunicRandomizer.Settings.UseTrunicTranslations ? $"{HintGhost.TrunicDialogue}---{HintGhost.TrunicHint}" : $"{HintGhost.Dialogue}---{HintGhost.Hint}";
                    NewGhostFox.GetComponent<NPC>().script = HintText;
                    if (PaletteEditor.CelShadingEnabled && PaletteEditor.ToonFox != null) {
                        NewGhostFox.transform.GetChild(2).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = PaletteEditor.ToonFox.GetComponent<MeshRenderer>().material;
                    }
                    NewGhostFox.SetActive(true);
                }
            }
        }

        public static void GenerateHints() {
            HintGhosts.Clear();
            List<string> GhostSpawns = GhostLocations.Keys.ToList();
            if (SaveFile.GetInt("randomizer entrance rando enabled") == 0 && GhostLocations.ContainsKey("Hint Ghost Purgatory"))
            { GhostLocations.Remove("Hint Ghost Purgatory"); }
            List<string> SelectedSpawns = new List<string>();
            for (int i = 0; i < 15; i++) {
                string Location = GhostSpawns[TunicRandomizer.Randomizer.Next(GhostSpawns.Count)];
                SelectedSpawns.Add(Location);
                GhostSpawns.Remove(Location);
            }
            foreach (string Location in SelectedSpawns) {
                HintGhost HintGhost = GhostLocations[Location][TunicRandomizer.Randomizer.Next(GhostLocations[Location].Count)];
                HintGhosts.Add(HintGhost.Name, HintGhost);
            }
            GenerateLocationHints();
            GenerateItemHints();
            GenerateBarrenAndMoneySceneHints();

            List<(string, string)> Hints = new List<(string, string)>();
            for (int i = 0; i < 5; i++) {
                (string, string) LocationHint = LocationHints[TunicRandomizer.Randomizer.Next(LocationHints.Count)];
                Hints.Add(LocationHint);
                LocationHints.Remove(LocationHint);

                (string, string) ItemHint = ItemHints[TunicRandomizer.Randomizer.Next(ItemHints.Count)];
                Hints.Add(ItemHint);
                ItemHints.Remove(ItemHint);

                if (i == 0 && SaveFile.GetInt("randomizer entrance rando enabled") == 1)
                {
                    GenerateHeirHint();
                    Hints.Add(HeirHint);
                }
                try {
                    (string, string) BarrenHint = BarrenAndTreasureHints[TunicRandomizer.Randomizer.Next(BarrenAndTreasureHints.Count)];
                    Hints.Add(BarrenHint);
                    BarrenAndTreasureHints.Remove(BarrenHint);
                } catch (Exception e) {
                    int flip = TunicRandomizer.Randomizer.Next(2);
                    if (flip == 0) {
                        LocationHint = LocationHints[TunicRandomizer.Randomizer.Next(LocationHints.Count)];
                        Hints.Add(LocationHint);
                        LocationHints.Remove(LocationHint);
                    } else {
                        ItemHint = ItemHints[TunicRandomizer.Randomizer.Next(ItemHints.Count)];
                        Hints.Add(ItemHint);
                        ItemHints.Remove(ItemHint);
                    }
                }
            }
            foreach (string key in HintGhosts.Keys.ToList()) {
                (string, string) Hint = Hints[TunicRandomizer.Randomizer.Next(Hints.Count)];
                HintGhosts[key].Hint = Hint.Item1;
                HintGhosts[key].TrunicHint = Hint.Item2;
                Hints.Remove(Hint);
            }
        }

        public static void GenerateLocationHints() {
            LocationHints.Clear();
            List<String> HintableLocations = HintableLocationIds.Keys.ToList();
            if (SaveFile.GetInt("randomizer keys behind bosses") == 1) {
                // Remove boss hints if keys behind bosses is on
                HintableLocations.Remove("Vault Key (Red) [Fortress Arena]");
                HintableLocations.Remove("Hexagon Green [Library Arena]");
                HintableLocations.Remove("Hexagon Blue [ziggurat2020_3]");
            }

            if (SaveFile.GetInt("randomizer entrance rando enabled") == 1)
            {
                // Remove dead end hints because they literally don't matter
                HintableLocations.Remove("1011 [Dusty]");
                HintableLocations.Remove("final [Mountaintop]");
            }

            foreach (string Key in HintableLocations) {
                string ItemName = Hints.SimplifiedItemNames[ItemPatches.ItemList[Key].Reward.Name];
                string LocationSuffix = HintableLocationIds[Key][HintableLocationIds[Key].Length-1] == 'S' ? "R" : "iz";
                string ItemPrefix = ItemName == "Money" ? "suhm" : Vowels.Contains(ItemName.ToUpper()[0]) ? "ahn" : "uh";
                string Hint = $"bI #uh wA, I hurd #aht \"{HintableLocationIds[Key]}\"\n{LocationSuffix} gRdi^ {ItemPrefix} \"{ItemName.ToUpper()}.\"";
                string TrunicHint = $"bI #uh wA, I hurd #aht {Translations.Translate(HintableLocationIds[Key], false)}\n{LocationSuffix} gRdi^ {ItemPrefix} {Translations.Translate(ItemName, false)}.";

                LocationHints.Add((Hint, TrunicHint));
            }
        }

        public static void GenerateItemHints() {
            ItemHints.Clear();
            List<string> HintableItems = new List<string>(HintableItemNames);
            if (SaveFile.GetInt("randomizer shuffled abilities") == 1) {
                HintableItems.Add("26");
            }
            foreach (string Key in HintableItems) {
                List<ItemData> Items = FindAllRandomizedItemsByName(Key);
                foreach (ItemData Item in Items) {
                    string ScenePrefix = Item.Location.SceneName == "Trinket Well" ? "%rOi^" : "aht #uh";
                    string Scene = Hints.SimplifiedSceneNames[Item.Location.SceneName];
                    string Hint = "";
                    string TrunicHint = $"";
                    if (Scene.Length > 15) {
                        string[] SceneSplit = Scene.Split(' ');
                        Hint = $"bI #uh wA, I saw A \"{Hints.SimplifiedItemNames[Item.Reward.Name].ToUpper()}\" #uh\nlahst tIm I wuhs {ScenePrefix} \"{String.Join(" ", SceneSplit.Take(SceneSplit.Length - 1)).ToUpper()}\"\n\"{SceneSplit[SceneSplit.Length-1].ToUpper()}.\"";
                    } else {
                        Hint = $"bI #uh wA, I saw A \"{Hints.SimplifiedItemNames[Item.Reward.Name].ToUpper()}\" #uh\nlahst tIm I wuhs {ScenePrefix} \"{Scene.ToUpper()}.\"";
                    }
                    TrunicHint = $"bI #uh wA, I saw A {Translations.Translate(Hints.SimplifiedItemNames[Item.Reward.Name], false)} #uh\nlahst tIm I wuhs {ScenePrefix} {Translations.Translate(Scene, false)}.";
                    ItemHints.Add((Hint, TrunicHint));
                }
            }
        }

        public static void GenerateBarrenAndMoneySceneHints() {
            BarrenAndTreasureHints.Clear();
            foreach (string Key in Hints.SimplifiedSceneNames.Keys) {
                HashSet<string> ItemsInScene = new HashSet<string>();
                string Scene = Hints.SimplifiedSceneNames[Key];
                int SceneItemCount = 0;
                int MoneyInScene = 0;
                foreach (ItemData Item in ItemPatches.ItemList.Values.Where(item => item.Location.SceneName == Key).ToList()) {
                    ItemsInScene.Add(Item.Reward.Name);
                    if (Item.Reward.Name == "money") {
                        MoneyInScene += Item.Reward.Amount;
                    }
                    SceneItemCount++;
                }

                if (SceneItemCount == 0)
                { continue; }

                if (MoneyInScene >= 200 && SceneItemCount < 10) {
                    string ScenePrefix = Vowels.Contains(Scene.ToUpper()[0]) ? "#E" : "#uh";
                    string Hint = $"ahn EzE plAs too fInd A \"LOT OF MONEY\" iz {ScenePrefix}\n\"{Scene.ToUpper()}.\"";
                    string TrunicHint = $"ahn EzE plAs too fInd A lawt uhv muhnE iz\n{ScenePrefix}\n{Translations.Translate(Scene, false)}.";
                    BarrenAndTreasureHints.Add((Hint, TrunicHint));
                } else {
                    bool BarrenArea = true;
                    foreach (string Item in ItemsInScene) {
                        if (!BarrenItemNames.Contains(Item)) {
                            BarrenArea = false;
                            break;
                        }
                        if (HintGhosts.Values.Where(HintGhost => HintGhost.SceneName == Key).ToList().Count > 0) {
                            BarrenArea = false;
                            break;
                        }
                    }
                    if (BarrenArea) {
                        string Hint = "";
                        string TrunicHint = "";
                        if (Scene.Length > 15) {
                            string[] SceneSplit = Scene.ToUpper().Split(' ');
                            Hint = $"if I wur yoo, I woud uhvoid \"{String.Join(" ", SceneSplit.Take(SceneSplit.Length - 1))}\"\n\"{SceneSplit[SceneSplit.Length-1]}.\" #aht plAs iz \"NOT IMPORTANT.\"";
                        } else {
                            Hint = $"if I wur yoo, I woud uhvoid \"{Scene.ToUpper()}.\"\n#aht plAs iz \"NOT IMPORTANT.\"";
                        }
                        TrunicHint = $"if I wur yoo, I woud uhvoid {Translations.Translate(Scene, false)}.\n#aht plAs iz nawt importahnt.";
                        BarrenAndTreasureHints.Add((Hint, TrunicHint));
                    }
                }
            }
        }

        public static void GenerateHeirHint()
        {
            //Logger.LogInfo("heir hint creation started");
            string heirPortal = "error finding heir";
            foreach (PortalCombo portalCombo in TunicPortals.RandomizedPortals.Values)
            {
                if (portalCombo.Portal1.Scene == "Spirit Arena")
                {
                    //Logger.LogInfo("found the heir, they're at " + portalCombo.Portal2.Name);
                    heirPortal = portalCombo.Portal2.Name;
                    break;
                }
                if (portalCombo.Portal2.Scene == "Spirit Arena")
                {
                    //Logger.LogInfo("found the heir, they're at " + portalCombo.Portal1.Name);
                    heirPortal = portalCombo.Portal1.Name;
                    break;
                }
            }
            HeirHint = ($"bI #uh wA, I hurd #aht \"THE HEIR\" moovd, #A liv \naht \"{heirPortal.ToUpper()}\" now.",
                        $"bI #uh wA, I hurd #aht \"THE HEIR\" moovd, #A liv \naht \"{heirPortal.ToUpper()}\" now.");
        }

        public static void SpawnTorchHintGhost() {
            GhostHints.GhostFox.GetComponent<NPC>().nPCAnimState = NPC.NPCAnimState.SIT;
            GameObject TorchFox = GameObject.Instantiate(GhostHints.GhostFox);
            TorchFox.transform.position = new Vector3(-12.3128f, 11.9833f, -145.3333f);
            TorchFox.transform.transform.localEulerAngles = new Vector3(0f, 180f, 0f);

            LanguageLine TorchFoxScript = ScriptableObject.CreateInstance<LanguageLine>();
            TorchFoxScript.text = $"bE kArfuhl, tInE fawks. %i^z Ruhnt #uh wA #A sEm.---I sE yoo hahv A torJ [torch]?\n\"USE\" it too rEturn hEr, \"IF\" yoo bEkuhm \"LOST.\"";
            TorchFox.GetComponent<NPC>().script = TorchFoxScript;

            TorchFox.SetActive(true);
        }

        public static void SpawnLostGhostFox() {
            if (SaveFile.GetInt("randomizer sent lost fox home") == 0) {
                GhostHints.GhostFox.GetComponent<NPC>().nPCAnimState = NPC.NPCAnimState.SIT;
                GameObject LostFox = GameObject.Instantiate(GhostHints.GhostFox);
                LostFox.transform.position = new Vector3(-1.4098f, 0.0585f, 12.9491f);
                LostFox.transform.transform.rotation = new Quaternion(0f, 0f, 0f, 1f);

                LanguageLine LostFoxScript = ScriptableObject.CreateInstance<LanguageLine>();
                if (Inventory.GetItemByName("Homeward Bone Statue").Quantity == 0) {
                    LostFoxScript.text = $"I lawst mI mahjik stOn ahnd kahnt gO hOm...---if yoo fInd it, kahn yoo bri^ it too mE?\nit louks lIk #is: [dath]";
                } else {
                    LostFoxScript.text = $"I lawst mI mahjik stOn [dath] ahnd kahnt gO hOm...---... wAt, yoo fownd it! plEz, yooz it now!";
                }
                LostFox.GetComponent<NPC>().script = LostFoxScript;

                LostFox.SetActive(true);
            }
        }

        public static void SpawnRescuedGhostFox() {
            if (SaveFile.GetInt("randomizer sent lost fox home") == 1) {
                GhostHints.GhostFox.GetComponent<NPC>().nPCAnimState = NPC.NPCAnimState.SIT;
                GameObject SavedFox = GameObject.Instantiate(GhostHints.GhostFox);
                SavedFox.transform.position = new Vector3(80.6991f, 15.9245f, 115.0217f);
                SavedFox.transform.transform.localEulerAngles = new Vector3(0f, 270f, 0f);

                LanguageLine SavedFoxScript = ScriptableObject.CreateInstance<LanguageLine>();
                SavedFoxScript.text = $"%ah^k yoo for sehnding mE hOm.---plEz kEp #aht stOn ahs A rEword. it wil tAk yoo\nbahk too yor wurld.";
                SavedFox.GetComponent<NPC>().script = SavedFoxScript;

                SavedFox.SetActive(true);
            }
        }

        public static void SpawnCathedralDoorGhost() {
            GhostHints.GhostFox.GetComponent<NPC>().nPCAnimState = NPC.NPCAnimState.GAZE;
            GameObject DoorHint = GameObject.Instantiate(GhostHints.GhostFox);
            DoorHint.transform.position = new Vector3(82.5f, 14f, 143.7f);
            DoorHint.transform.transform.rotation = new Quaternion(0f, 0f, 0f, 1f);
            LanguageLine DoorSecret = ScriptableObject.CreateInstance<LanguageLine>();
            DoorSecret.text = $"$$$... dOnt tehl ehnEwuhn, buht #aht \"DOOR\" bahk #Ar\nkahn bE \"OPENED\" fruhm #E \"OUTSIDE...\"";
            DoorHint.GetComponent<NPC>().script = DoorSecret;
            DoorHint.SetActive(true);

        }

        public static List<ItemData> FindAllRandomizedItemsByName(string ItemName) {
            List<ItemData> Items = new List<ItemData>();
            foreach (string Key in ItemPatches.ItemList.Keys) {
                ItemData Item = ItemPatches.ItemList[Key];
                if (Item.Reward.Name == ItemName) {
                    Items.Add(Item);
                }
            }
            return Items;
        }

        public static void ResetGhostHints() {
            HintGhosts.Clear();
            LocationHints.Clear();
            ItemHints.Clear();
            BarrenAndTreasureHints.Clear();
        }

    }
}
