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
            public string SceneName;
            public Vector3 Position;
            public Quaternion Rotation;
            public NPC.NPCAnimState AnimState;
            public string Dialogue;
            public string Hint;

            public HintGhost() { }

            public HintGhost(string sceneName, Vector3 position, Quaternion rotation, NPC.NPCAnimState animState, string dialogue) {
                SceneName = sceneName;
                Position = position;
                Rotation = rotation;
                Dialogue = dialogue;
                AnimState = animState;
                Hint = "";
            }
        }

        private static ManualLogSource Logger = TunicRandomizer.Logger;

        public static GameObject GhostFox;

        public static List<char> Vowels = new List<char>() { 'A', 'E', 'I', 'O', 'U' };
        public static List<string> LocationHints = new List<string>();
        public static List<string> ItemHints = new List<string>();
        public static List<string> BarrenAndTreasureHints = new List<string>();

        public static List<HintGhost> HintGhosts = new List<HintGhost>();

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
            "Relic - Hero Sword",
            "Relic - Hero Pendant MP",
            "Relic - Hero Water",
            "Relic - Hero Pendant HP",
            "Relic - Hero Crown",
            "Relic - Hero Pendant SP",
            "Dath Stone"
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
                new HintGhost("Sword Cave", new Vector3(5.1151f, 0.0637f, 12.6657f), new Quaternion(0f, 0.9642988f, 0f, 0.2648164f), NPC.NPCAnimState.SIT, $"its dAnjuris too gO uhlOn, tAk #is hint:"), }
            },
            { "Far Shore", new List<HintGhost>() {
                new HintGhost("Overworld Redux", new Vector3(8.45f, 12.0833f, -204.9243f), new Quaternion(0f, 0.4226183f, 0f, -0.9063078f), NPC.NPCAnimState.IDLE, $"wAr did yoo kuhm fruhm, tInE fawks?"),
                new HintGhost("Transit", new Vector3(-18.6177f, 8.0314f, -81.6153f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, "I stoud awn #aht skwAr ahnd ehndid uhp hEr suhmhow.\nwAr igzahktlE R wE?" ) }
            },
            { "Ruined Passage", new List<HintGhost>() {
                new HintGhost("Ruins Passage", new Vector3(184.1698f, 17.3268f, 40.54981f), new Quaternion(0f, 0.9659258f, 0f, 0.2588191f), NPC.NPCAnimState.TIRED, $"nahp tIm! haw haw haw... geht it?") }
            },
            { "Windmill", new List<HintGhost>() {
                new HintGhost("Windmill", new Vector3(-58.33329f, 54.0833f, -27.8653f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"viziti^ #uh \"SHOPKEEPER\"? doo nawt bE uhlRmd, #A R\nA frehnd.") }
            },
            { "Old House Back", new List<HintGhost>() {
                new HintGhost("Overworld Interiors", new Vector3(11.0359f, 29.0833f, -7.3707f), new Quaternion(0f, 0.8660254f, 0f, -0.5000001f), NPC.NPCAnimState.PRAY, $"nuh%i^ wurks! mAbE #Arz suhm trik too #is dor...") }
            },
            { "Old House Front", new List<HintGhost>() {
                new HintGhost("Overworld Interiors", new Vector3(-24.06613f, 27.39948f, -47.9316f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.TIRED, $"juhst fIv mor minits..."),
                new HintGhost("Overworld Interiors", new Vector3(12.0368f, 21.1446f, -72.81052f), new Quaternion(0f, 0.8660254f, 0f, -0.5000001f), NPC.NPCAnimState.SIT, $"wuht R #Ez pehduhstuhlz for? doo yoo nO?") }
            },
            { "Overworld Above Ruins", new List<HintGhost>() {
               new HintGhost("Overworld Redux", new Vector3(28.53184f, 36.0833f, -108.3734f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.IDLE, $"I wuhz hIdi^ fruhm #uh \"SLIMES,\" buht yoo dOnt louk\nlIk wuhn uhv #ehm."),
               new HintGhost("Overworld Redux", new Vector3(22.3667f, 27.9833f, -126.3728f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"wAr did I lEv %aht kE..."),
               new HintGhost("Overworld Redux", new Vector3(51.20462f, 28.00694f, -129.722f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.SIT, $"I %awt #aht Jehst wuhz ehmptE. how suhspi$is.") }
            },
            { "Early Overworld Spawns", new List<HintGhost>() {
               new HintGhost("Overworld Redux", new Vector3(-5.4483f, 44.1363f, -8.43854f), new Quaternion(0f, 0.9396926f, 0f, 0.3420202f), NPC.NPCAnimState.WOE, $"sEld forehvur? nO... #Ar muhst bE uhnuh#ur wA..."),
               new HintGhost("Overworld Redux", new Vector3(-34.0649f, 37.9833f, -59.2506f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.GAZE, $"sO mehnE roodli^z. Im stAi^ uhp hEr.") }
            },
            { "Inside Temple", new List<HintGhost>() {
                new HintGhost("Temple", new Vector3(7.067f, -0.224f, 59.9285f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.IDLE, $"yur naht uh \"RUIN SEEKER,\" R yoo? mAbE yoo $oud gO\nsuhmwAr ehls."),
                new HintGhost("Temple", new Vector3(0.9350182f, 4.076f, 133.7965f), new Quaternion(0f, 0.8660254f, 0f, 0.5f), NPC.NPCAnimState.GAZE_UP, $"yur guhnuh frE \"THE HEIR\"? iznt #aht... bahd?") }
            },
            { "Ruined Shop", new List<HintGhost>() {
                new HintGhost("Ruined Shop", new Vector3(16.5333f, 8.983299f, -45.60382f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"hehlO. wuht iz yor nAm?---...tuhnk? wuht A strAnj nAm."),
                new HintGhost("Ruined Shop", new Vector3(9.8111f, 8.0833f, -37.52119f), new Quaternion(0f, 0.9659258f, 0f, 0.2588191f), NPC.NPCAnimState.IDLE, $"wehl, if yur nawt bIi^ ehnE%i^..." ) }
            },
            { "West Filigree", new List<HintGhost>() {
                new HintGhost("Town_FiligreeRoom", new Vector3(-79.4348f, 22.0379f, -59.8104f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.PRAY, $"wow, yoo hahv #uh powur uhv #uh \"Holy Cross\"!") }
            },
            { "East Filigree", new List<HintGhost>() {
                new HintGhost("EastFiligreeCache", new Vector3(14.3719f, 0.0167f, -8.8614f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"wAt, how did yoo Opehn #aht dOr?") }
            },
            { "Maze Room", new List<HintGhost>() {
                new HintGhost("Maze Room", new Vector3(3.5129f,-0.1167f,-9.4481f), new Quaternion(0f,0f,0f,1f), NPC.NPCAnimState.IDLE, $"wAt... how kahn yoo wahk in hEr? #Arz nO flOr!" ) }
            },
            { "Changing Room", new List<HintGhost>() {
                new HintGhost("Changing Room", new Vector3(14.9876f, 6.9379f, 14.6771f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.PRAY, $"doo yoo %ink #is louks goud awn mE?"),
                new HintGhost("Changing Room", new Vector3(14.9876f, 6.9379f, 14.6771f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.DANCE,  $"doo yoo nO sIlehntdistroiur? hE iz kool.") }
            },
            { "Waterfall", new List<HintGhost>() {
                new HintGhost("Waterfall", new Vector3(-41.13461f, 44.9833f, -0.6913f), new Quaternion(0f, 0.6755902f, 0f, -0.7372773f), NPC.NPCAnimState.IDLE, $"doo yoo nO wuht #uh fArEz R sAi^?") }
            },
            { "Hourglass Cave", new List<HintGhost>() {
                new HintGhost("Town Basement", new Vector3(-211.3147f, 1.0833f, 35.7667f), new Quaternion(0f, 0.4226183f, 0f, 0.9063078f), NPC.NPCAnimState.GAZE, $"dOnt gO in #Ar. kahnt yoo rEd %uh sIn?") }
            },
            { "Overworld Cave", new List<HintGhost>() {
                new HintGhost("Overworld Cave", new Vector3(-88.0794f, 515.1076f, -741.0837f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"sO pritE!" ) }
            },
            { "Patrol Cave", new List<HintGhost>() {
                new HintGhost("PatrolCave", new Vector3(80.41302f, 46.0686f, -48.0821f), new Quaternion(0f, 0.9848078f, 0f, -0.1736482f), NPC.NPCAnimState.GAZE, $"#Arz ahlwAz uh sEkrit bEhInd #uh wahturfahl!"),
                new HintGhost("PatrolCave", new Vector3(72.6667f, 46.0686f, 14.9446f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.GAZE, $"klA^ klA^ klA^... duhzint hE ehvur stawp?") }
            },
            { "Cube Room", new List<HintGhost>() {
                new HintGhost("CubeRoom", new Vector3(326.784f, 3.0833f, 207.0065f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.DANCE, $"rIt ahnd uhp! rit ahnd uhp!") }
            },
            { "Furnace", new List<HintGhost>() {
                new HintGhost("Furnace", new Vector3(-131.9886f, 12.0833f, -51.0197f), new Quaternion(0f, 0f, 0f, 1f), NPC.NPCAnimState.GAZE_UP, $"#Ez powur sorsehz... I dOnt truhst #ehm.") }
            },
            { "Golden Obelisk", new List<HintGhost>() {
                new HintGhost("Overworld Redux", new Vector3(-94.5973f, 70.0937f, 36.38749f), new Quaternion(0f, 0f, 0f, 1f), NPC.NPCAnimState.FISHING, $"pEpuhl yoost too wur$ip #is. it rehprEzehnts #uh\n\"Holy Cross.\"") }
            },
            { "Overworld Before Garden", new List<HintGhost>(){
                new HintGhost("Overworld Redux", new Vector3(-146.1464f, 11.6929f, -67.55009f), new Quaternion(0f, 0.3007058f, 0f, 0.9537169f), NPC.NPCAnimState.IDLE, "A vi$is baws blawks #uh wA too #uh behl uhp #Ar.\nbE kArfuhl, it wil kil yoo.") }
            },
            { "West Garden", new List<HintGhost>() {
                new HintGhost("Archipelagos Redux", new Vector3(-290.3334f, 4.0667f, 153.9145f), new Quaternion(0f, 0.9659259f, 0f, -0.2588191f), NPC.NPCAnimState.GAZE, $"wawJ owt for tArE uhp uhhehd. hE wil trI too Jawmp yoo."),
                new HintGhost("Archipelagos Redux", new Vector3(-137.9978f, 2.0781f, 150.5348f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.GAZE, $"iz #is pRt uhv suhm%i^ ehls? hmm..."),
                new HintGhost("Archipelagos Redux", new Vector3(-164.9391f, 10.1164f, 144.4981f), new Quaternion(0f, 0.3826835f, 0f, -0.9238795f), NPC.NPCAnimState.WOE, $"wil #uh hErO ehvur kuhm bahk?---...R yoo #uh hErO?"),
                new HintGhost("Archipelagos Redux", new Vector3(-256.3194f, 4.1667f, 168.15f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.FISHING, $"doo #A louk fuhmilyur too yoo?") }
            },
            { "West Bell", new List<HintGhost>() {
                new HintGhost("Overworld Redux", new Vector3(-130.929f, 40.0833f, -51.5f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.SIT, $"its sO kwIeht hEr... goud #i^ nObuhdE rA^ #aht behl.") }
            },
            { "Ice Dagger House", new List<HintGhost>() {
                new HintGhost("archipelagos_house", new Vector3(-201.1842f, 3.1209f, 38.4875f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.PRAY, $"Is too mEt yoo!") }
            },
            { "East Belltower Lower", new List<HintGhost>() {
                new HintGhost("Forest Belltower", new Vector3(500.9258f, 13.9394f, 63.79896f), new Quaternion(0f, 0.9659258f, 0f, 0.2588191f), NPC.NPCAnimState.SIT, $"#uh lahdur brOk ahnd #Ar R ehnehmEz owtsId, buht #is stahJoo\nawfurz sAftE.") }
            },
            { "East Belltower Upper", new List<HintGhost>() {
                new HintGhost("Forest Belltower", new Vector3(500.3264f, 62.012f, 107.5831f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.GAZE, $"di^ daw^! doo yoo hahv wuht it tAks?"),
                new HintGhost("Forest Belltower", new Vector3(593.9467f, 14.0052f, 84.43121f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.IDLE, $"wow... yoo did it!"),
                new HintGhost("East Forest Redux Laddercave", new Vector3(159.0245f, 17.89421f, 78.52466f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.PRAY, $"#Arz uh baws uhhehd... buht hE hahz por vi&uhn.")}
            },
            { "Swamp Redux 2", new List<HintGhost>() {
                new HintGhost("Swamp Redux 2", new Vector3(-47f, 16.0463f, -31.3333f), new Quaternion(0f, 0f, 0f, 1f), NPC.NPCAnimState.GAZE, $"I kahn sE mI hows fruhm hEr!" ),
                new HintGhost("Swamp Redux 2", new Vector3(-90.55162f, 3.0462f, 6.2667f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"suhm%i^ $oud bE hEr rIt? I dOnt rEmehmbur..." ),
                new HintGhost("Swamp Redux 2", new Vector3(-100.5333f, 3.3462f, 25.0965f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"I %ink #is wuhz pRt uhv suhm%i^ ehls wuhns." ) }
            },
            { "Dark Tomb", new List<HintGhost>() {
                new HintGhost("Crypt Redux", new Vector3(-75.8704f, 57.0833f, -56.2025f), new Quaternion(0f, 0.3826835f, 0f, -0.9238795f), NPC.NPCAnimState.GAZE, $"dRk! its sO dRk!"),
                new HintGhost("Sewer_Boss", new Vector3(70.30289f, 9.4138f, -9.387097f), new Quaternion(0f, 0.7660444f, 0f, 0.6427876f), NPC.NPCAnimState.GAZE, $"wuht koud bE bahk #Ar?") }
            },
            { "Fortress Courtyard", new List<HintGhost>() {
                new HintGhost("Fortress Courtyard", new Vector3(-50.54346f, 0.0417f, -36.46348f), new Quaternion(0f, 0.9659259f, 0f, -0.2588191f), NPC.NPCAnimState.GAZE, $"yoo gO furst. spIdurs giv mE #uh krEps..."),
                new HintGhost("Fortress Courtyard", new Vector3(6.967727f, 0.0417f, -74.5881f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.IDLE, $"wehl wehl wehl... sawrE."),
                new HintGhost("Fortress Courtyard", new Vector3(7.299674f, 9.0417f, -89.57533f), new Quaternion(0f, 0f, 0f, 1f), NPC.NPCAnimState.FISHING, $"sO mehnE kahndlz! hahpE bur%dA!"),
                new HintGhost("Fortress Courtyard", new Vector3(11.6453f, 4.0203f, -115.355f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.GAZE, $"I woud nawt tuhJ #aht wuhn. hoo nOz wuht #Al doo wi%\n#uh powur?") }
            },
            { "Mountain Door", new List<HintGhost>() {
                new HintGhost("Mountain", new Vector3(54.7674f, 41.5568f, 4.4282f), new Quaternion(0f, 0.3826835f, 0f, -0.9238795f), NPC.NPCAnimState.GAZE_UP, $"yoo kahn Opehn #is? uhmAzi^!") }
            },
            { "Atoll Entrance", new List<HintGhost>() {
                new HintGhost("Atoll Redux", new Vector3(-3.5443f, 1.0174f, 120.0543f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"burdsaw^ iz sO rElahksi^. twEt twEt twEt!"),
                new HintGhost("Atoll Redux", new Vector3(4.7f, 16.0776f, 101.9315f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"#Ez skwArz...wuht purpis doo #A surv?"),
                new HintGhost("Atoll Redux", new Vector3(0.4395638f, 16.0874f, 64.47866f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.GAZE, $"tIm hahz tAkin ahtOl awn #is plAs.") }
            },
            { "Frog's Domain", new List<HintGhost>() {
                new HintGhost("frog cave main", new Vector3(19.7682f, 9.1943f, -23.3269f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.FISHING, $"I wuhndur wAr #uh kwehstuhgawn iz?"),
                new HintGhost("frog cave main", new Vector3(27.09619f, 9.2581f, -37.28336f), new Quaternion(0f, 0.5000001f, 0f, -0.8660254f), NPC.NPCAnimState.FISHING, $"$hhh. Im hIdi^ fruhm #uh frawgs.") }
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
            foreach (HintGhost HintGhost in HintGhosts) {
                if (HintGhost.SceneName == SceneName) {
                    GhostFox.GetComponent<NPC>().nPCAnimState = HintGhost.AnimState;
                    GameObject NewGhostFox = GameObject.Instantiate(GhostFox);
                    NewGhostFox.transform.position = HintGhost.Position;
                    NewGhostFox.transform.rotation = HintGhost.Rotation;
                    LanguageLine HintText = ScriptableObject.CreateInstance<LanguageLine>();
                    HintText.text = $"{HintGhost.Dialogue}---{HintGhost.Hint}";
                    NewGhostFox.GetComponent<NPC>().script = HintText;
                    NewGhostFox.SetActive(true);
                }
            }
        }

        public static void GenerateHints() {
            HintGhosts.Clear();
            List<string> GhostSpawns = GhostLocations.Keys.ToList();
            List<string> SelectedSpawns = new List<string>();
            for (int i = 0; i < 15; i++) {
                string Location = GhostSpawns[TunicRandomizer.Randomizer.Next(GhostSpawns.Count)];
                SelectedSpawns.Add(Location);
                GhostSpawns.Remove(Location);
            }
            foreach (string Location in SelectedSpawns) {
                HintGhost HintGhost = GhostLocations[Location][TunicRandomizer.Randomizer.Next(GhostLocations[Location].Count)];
                HintGhosts.Add(HintGhost);
            }
            GenerateLocationHints();
            GenerateItemHints();
            GenerateBarrenAndMoneySceneHints();
            
            List<string> Hints = new List<string>();
            for (int i = 0; i < 5; i++) {
                string LocationHint = LocationHints[TunicRandomizer.Randomizer.Next(LocationHints.Count)];
                Hints.Add(LocationHint);
                LocationHints.Remove(LocationHint);

                string ItemHint = ItemHints[TunicRandomizer.Randomizer.Next(ItemHints.Count)];
                Hints.Add(ItemHint);
                ItemHints.Remove(ItemHint);
                try {
                    string BarrenHint = BarrenAndTreasureHints[TunicRandomizer.Randomizer.Next(BarrenAndTreasureHints.Count)];
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
            foreach (HintGhost HintGhost in HintGhosts) {
                string Hint = Hints[TunicRandomizer.Randomizer.Next(Hints.Count)];
                HintGhost.Hint = Hint;
                Hints.Remove(Hint);
            }
        }

        public static void GenerateLocationHints() {
            LocationHints.Clear();
            foreach (string Key in HintableLocationIds.Keys) {
                string ItemName = Hints.SimplifiedItemNames[RandomItemPatches.ItemList[Key].Reward.Name];
                string LocationSuffix = HintableLocationIds[Key][HintableLocationIds[Key].Length-1] == 'S' ? "R" : "iz";
                string ItemPrefix = ItemName == "Money" ? "suhm" : Vowels.Contains(ItemName.ToUpper()[0]) ? "ahn" : "uh";
                string Hint = $"bI #uh wA, I hird #aht \"{HintableLocationIds[Key]}\"\n{LocationSuffix} gRdi^ {ItemPrefix} \"{ItemName.ToUpper()}.\"";

                LocationHints.Add(Hint);
            }
        }

        public static void GenerateItemHints() {
            ItemHints.Clear();
            foreach (string Key in HintableItemNames) {
                List<ItemData> Items = FindAllRandomizedItemsByName(Key);
                foreach (ItemData Item in Items) {
                    string ScenePrefix = Item.Location.SceneName == "Trinket Well" ? "%rOi^" : "aht #uh";
                    string Scene = Hints.SimplifiedSceneNames[Item.Location.SceneName].ToUpper();
                    string Hint = "";
                    if (Scene.Length > 15) {
                        string[] SceneSplit = Scene.Split(' ');
                        Hint = $"bI #uh wA, I saw A \"{Hints.SimplifiedItemNames[Item.Reward.Name].ToUpper()}\" #uh\nlahst tIm I wuhs {ScenePrefix} \"{String.Join(" ", SceneSplit.Take(SceneSplit.Length - 1))}\"\n\"{SceneSplit[SceneSplit.Length-1]}.\"";
                    } else {
                        Hint = $"bI #uh wA, I saw A \"{Hints.SimplifiedItemNames[Item.Reward.Name].ToUpper()}\" #uh\nlahst tIm I wuhs {ScenePrefix} \"{Scene}.\"";
                    }
                    ItemHints.Add(Hint);
                }
            }
        }

        public static void GenerateBarrenAndMoneySceneHints() {
            BarrenAndTreasureHints.Clear();
            foreach (string Key in Hints.SimplifiedSceneNames.Keys) {
                HashSet<string> ItemsInScene = new HashSet<string>();
                string Scene = Hints.SimplifiedSceneNames[Key].ToUpper();
                int SceneItemCount = 0;
                int MoneyInScene = 0;
                foreach (ItemData Item in RandomItemPatches.ItemList.Values.Where(item => item.Location.SceneName == Key).ToList()) {
                    ItemsInScene.Add(Item.Reward.Name);
                    if (Item.Reward.Name == "money") {
                        MoneyInScene += Item.Reward.Amount;
                    }
                    SceneItemCount++;
                }
                if (MoneyInScene >= 200 && SceneItemCount < 10) {
                    string ScenePrefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                    BarrenAndTreasureHints.Add($"ahn EzE plAs too fInd A \"LOT OF MONEY\" iz {ScenePrefix}\n\"{Scene}.\"");
                } else {
                    bool BarrenArea = true;
                    foreach (string Item in ItemsInScene) {
                        if (!BarrenItemNames.Contains(Item)) {
                            BarrenArea = false;
                            break;
                        }
                        if (HintGhosts.Where(HintGhost => HintGhost.SceneName == Key).ToList().Count > 0) {
                            BarrenArea = false;
                            break;
                        }
                    }
                    if (BarrenArea) {
                        string Hint = "";
                        if (Scene.Length > 15) {
                            string[] SceneSplit = Scene.Split(' ');
                            Hint = $"if I wur yoo, I wood uhvoid \"{String.Join(" ", SceneSplit.Take(SceneSplit.Length - 1))}\"\n\"{SceneSplit[SceneSplit.Length-1]}.\" #aht plAs iz \"NOT IMPORTANT.\"";
                        } else {
                            Hint = $"if I wur yoo, I wood uhvoid \"{Scene}.\"\n#aht plAs iz \"NOT IMPORTANT.\"";
                        }
                        BarrenAndTreasureHints.Add(Hint);
                    }
                }
            }
        }

        public static List<ItemData> FindAllRandomizedItemsByName(string ItemName) {
            List<ItemData> Items = new List<ItemData>();
            foreach (string Key in RandomItemPatches.ItemList.Keys) {
                ItemData Item = RandomItemPatches.ItemList[Key];
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
