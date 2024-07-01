using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TunicRandomizer {

    public class LadderInfo {

        public string LadderNameAndPosition;

        public List<TransformData> ConstructionPlacements;

        public List<string> OptionalObjectsToDisable;

        public bool IsSpecialLadder;

        public bool IsEntrance;
        public bool IsExit;

        public bool LargerColliders;

        public string OptionalStateVar;

        public LadderInfo() { }

        public LadderInfo(string ladder, List<TransformData> constructionPlacements, bool isSpecialLadder = false, bool isEntrance = false, bool isExit = false, string optionalStateVar = "", bool largerColliders = false) {
            LadderNameAndPosition = ladder;
            ConstructionPlacements = constructionPlacements;
            OptionalObjectsToDisable = new List<string>();
            IsSpecialLadder = isSpecialLadder;
            IsEntrance = isEntrance;
            IsExit = isExit;
            OptionalStateVar = optionalStateVar;
            LargerColliders = largerColliders;
        }
        public LadderInfo(string ladder, List<TransformData> constructionPlacements, List<string> optionalObjectsToDisable, bool isSpecialLadder = false, bool isEntrance = false, bool isExit = false, string optionalStateVar = "", bool largerColliders = false) {
            LadderNameAndPosition = ladder;
            ConstructionPlacements = constructionPlacements;
            OptionalObjectsToDisable = optionalObjectsToDisable;
            IsSpecialLadder = isSpecialLadder;
            IsEntrance = isEntrance;
            IsExit = isExit;
            OptionalStateVar = optionalStateVar;
            LargerColliders = largerColliders;
        }

    }

    public class LadderToggles {

        public static void CreateLadderItems() {
            foreach (ItemData LadderItem in ItemLookup.Items.Values.Where(item => item.Type == ItemTypes.LADDER)) {
                CreateLadderItem(LadderItem.Name);
            }
        }

        private static void CreateLadderItem(string name) {

            SpecialItem Ladder = ScriptableObject.CreateInstance<SpecialItem>();

            Ladder.name = name;
            Ladder.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
            Ladder.collectionMessage.text = $"\"{LadderCollectionMessages[name]}\"";
            Ladder.controlAction = "";

            Inventory.itemList.Add(Ladder);
        }

        public static Dictionary<string, string> LadderCollectionMessages = new Dictionary<string, string>() {
            { "Ladders in Overworld Town", "Overworld Town" },
            { "Ladders near Weathervane", "Near Weathervane" },
            { "Ladders near Overworld Checkpoint", "By Overworld Checkpoint" },
            { "Ladder to East Forest", "To East Forest" },
            { "Ladders to Lower Forest", "To Lower East Forest" },
            { "Ladders near Patrol Cave", "Near Patrol Cave" },
            { "Ladders in Well", "Ladders in Well" },
            { "Ladders to West Bell", "To West Bell" },
            { "Ladder to Quarry", "To Quarry" },
            { "Ladder in Dark Tomb", "In Dark Tomb" },
            { "Ladders near Dark Tomb", "Near Dark Tomb" },
            { "Ladder near Temple Rafters", "Near Temple Rafters" },
            { "Ladder to Swamp", "Ladder to Swamp" },
            { "Ladders in Swamp", "Ladders in Swamp" },
            { "Ladder to Ruined Atoll", "To Ruined Atoll" },
            { "Ladders in South Atoll", "South Atoll" },
            { "Ladders to Frog's Domain", "Frog's Domain" },
            { "Ladders in Hourglass Cave", "Hourglass Cave Tower" },
            { "Ladder to Beneath the Vault", "Beneath the Vault" },
            { "Ladders in Lower Quarry", "Lower Quarry" },
            { "Ladders in Library", "Library Ladders" },
        };

        public static void ToggleLadders() {
            if (PlayerCharacter.instance == null) {
                return;
            }

            string scene = SceneManager.GetActiveScene().name;
            List<Ladder> ladders = Resources.FindObjectsOfTypeAll<Ladder>().Where(ladder => ladder.gameObject.scene.name == scene).ToList();

            if (LaddersUnderConstruction.ContainsKey(scene)) {
                foreach(string LadderItem in LaddersUnderConstruction[scene].Keys) {
                    foreach(LadderInfo ladderInfo in LaddersUnderConstruction[scene][LadderItem]) {
                        GameObject Ladder = ladders.Where(ladder => $"{ladder.name} {ladder.transform.position.ToString()}" == ladderInfo.LadderNameAndPosition).DefaultIfEmpty(null).First().gameObject;
                        if (Ladder != null) {
                            if (ladderInfo.IsSpecialLadder && Ladder.transform.parent != null) {
                                Ladder = Ladder.transform.parent.gameObject;
                            }

                            Ladder.gameObject.AddComponent<ToggleLadderByLadderItem>().ladderItem = Inventory.GetItemByName(LadderItem);
                            Ladder.gameObject.GetComponent<ToggleLadderByLadderItem>().ladderInfo = ladderInfo;
                            Ladder.gameObject.GetComponent<ToggleLadderByLadderItem>().SpawnBlockers();
                        }
                    }
                }
            }

            if (scene == "Overworld Redux") {
                SpawnLadderChecklist();
            }
        }

        private static void SpawnLadderChecklist() {
            GameObject sign = GameObject.Instantiate(ModelSwaps.UnderConstruction);
            sign.GetComponent<MeshFilter>().mesh = ModelSwaps.Signpost.GetComponent<MeshFilter>().mesh;
            sign.GetComponent<MeshRenderer>().materials = ModelSwaps.Signpost.GetComponent<MeshRenderer>().materials;
            sign.GetComponent<Signpost>().message = ScriptableObject.CreateInstance<LanguageLine>();
            sign.transform.position = new Vector3(-9.2078f, 44.0833f, -23.3063f);
            sign.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            sign.transform.localScale = Vector3.one * 1.5f;
            sign.GetComponent<UnderConstruction>().isChecklistSign = true;
            sign.SetActive(true);

            GameObject construction = GameObject.Instantiate(ModelSwaps.UnderConstruction);
            construction.transform.position = new Vector3(-5.6409f, 44.0833f, -23.2322f);
            construction.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            construction.transform.localScale = Vector3.one;
            construction.GetComponent<UnderConstruction>().isChecklistSign = true;
            construction.GetComponent<Signpost>().message = ScriptableObject.CreateInstance<LanguageLine>();
            construction.SetActive(true);
        }

        public static string GetLadderChecklist() {
            string ladders = "          <#FF0000>[death] kuhnstruhk$uhn stahtuhs [death]\n";
            int i = 0;
            foreach(ItemData ladder in ItemLookup.Items.Values.Where(item => item.Type == ItemTypes.LADDER)) {
                ladders += $"\"{LadderCollectionMessages[ladder.Name]}{new String('.', 24-LadderCollectionMessages[ladder.Name].Length)}{(Inventory.GetItemByName(ladder.Name).Quantity == 0 ? "<#FF0000>Not Found" : "....<#00FF00>Found")}\"\n";
                
                i++;
                if(i % 7 == 0 && i < 20) {
                    ladders += "---          <#FF0000>[death] kuhnstruhk$uhn stahtuhs [death]\n";
                }
            }
            return ladders;
        }

        public static Dictionary<string, Dictionary<string, List<LadderInfo>>> LaddersUnderConstruction = new Dictionary<string, Dictionary<string, List<LadderInfo>>>() {
            {
                "Overworld Redux", new Dictionary<string, List<LadderInfo>>() {
                    {
                        "Ladders in Overworld Town", new List<LadderInfo>() {
                            new LadderInfo(
                                "Wooden Ladder (12 unit) (-45.0, 28.0, -44.0)",
                                new List<TransformData>() {
                                    new TransformData(new Vector3(-45.1047f, 40.0833f, -43.0333f), new Quaternion(0, 0, 0, 1), Vector3.one),
                                    new TransformData(new Vector3(-45.0384f, 28.1202f, -45.1667f), new Quaternion(0, 0, 0, 1), Vector3.one)
                                }
                            ),
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (-56.0, 4.0, -146.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-55.91198f, 3.811328f, -147.1667f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                    new TransformData(new Vector3(-61.25909f, 4.383893f, -150.3633f), new Quaternion(0f, 0.2127466f, 0f, 0.9771075f), Vector3.one),
                                    new TransformData(new Vector3(-58.04097f, 12.08334f, -145.8f), new Quaternion(0f, 0.298155f, 0f, -0.9545175f), Vector3.one),
                                    new TransformData(new Vector3(-55.32391f, 12.18787f, -145.1f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one)
                                }
                            ),
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (10) (-136.0, 4.0, -88.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-137.1667f, 4.083336f, -88.17893f), new Quaternion(0f, 0.6696451f, 0f, 0.7426812f), Vector3.one),
                                    new TransformData(new Vector3(-135.0333f, 12.08334f, -88.02832f), new Quaternion(0f, 0.7269877f, 0f, -0.6866505f), Vector3.one),
                                }
                            )
                        }
                    },
                    {
                        "Ladders near Weathervane", new List<LadderInfo>() {
                            // ladder above doorway
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (4) (day only) (67.5, 28.0, -126.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(68.37374f, 36.08334f, -126.0132f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                    new TransformData(new Vector3(65.12504f, 28.08334f, -125.325f), new Quaternion(0f, 0.2191043f, 0f, -0.9757015f), Vector3.one),
                                },
                                largerColliders: true
                            ),
                            // middle ladder next to chest
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (1) (66.0, 20.0, -130.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(63.70097f, 28.08334f, -128.5632f), new Quaternion(0f, -0.7047275f, 0f, 0.7094781f), Vector3.one),
                                    new TransformData(new Vector3(65.81023f, 20.08334f, -131.4398f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                    new TransformData(new Vector3(68.37518f, 30f, -136.1049f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                    //new TransformData(new Vector3(68.35239f, 28.08334f, -139.5678f), new Quaternion(0f, -0.700497f, 0f, 0.7136553f), Vector3.one),
                                },
                                largerColliders: true
                            ),
                            // lowest ladder after bushes
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (2) (61.5, 12.0, -146.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(61.45924f, 20.08334f, -145.0333f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), Vector3.one),
                                    new TransformData(new Vector3(61.22833f, 12.08334f, -147.7217f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), Vector3.one),
                                }
                            ),
                        }
                    },
                    {
                        "Ladders near Overworld Checkpoint", new List<LadderInfo>() {
                            // right ladder (closer to east forest)
                            new LadderInfo(
                                "Ladder (12.5, 44.0, -60.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(14.92617f, 52.08334f, -59.99998f), new Quaternion(0f, 0.7071042f, 0f, 0.7071094f), Vector3.one),
                                    new TransformData(new Vector3(15.95162f, 52.08334f, -64.00012f), new Quaternion(0f, 0.9537169f, 0f, -0.3007058f), Vector3.one),
                                    new TransformData(new Vector3(13.2333f, 44.08334f, -60.09396f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                },
                                new List<string>() {
                                    "_Shortcuts Etc/Stone Switch/",
                                    "_Shortcuts Etc/stone ladder hidden/stone ladder hidden steps/"
                                },
                                true,
                                optionalStateVar: "SV_postForestLadderToCheckpoint"
                            ),
                            new LadderInfo(
                                "Ladder (18.0, 45.0, -37.5)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(12.95f, 44.1255f, -38.2087f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                    new TransformData(new Vector3(12.95f, 44.1255f, -41.7378f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                    new TransformData(new Vector3(17.23743f, 52.08334f, -25.11396f), new Quaternion(0f, 0.5318049f, 0f, 0.846867f), Vector3.one),
                                    new TransformData(new Vector3(19.63233f, 52.12299f, -28.12213f), new Quaternion(0f, 0.2643822f, 0f, 0.964418f), Vector3.one),
                                },
                                new List<string>() {
                                    "_Shortcuts Etc/Stone Switch (1)/"
                                },
                                true,
                                optionalStateVar: "shortcut out of questagon temple"
                            ),
                        }
                    },
                    {
                        "Ladder to Ruined Atoll", new List<LadderInfo>() {
                            new LadderInfo(
                                "Wooden Ladder (12 unit) (2) (-81.0, 2.5, -196.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-80.53674f, 12.08334f, -203.4905f), new Quaternion(0f, 0.1536293f, 0f, 0.9881286f), Vector3.one),
                                    new TransformData(new Vector3(-85.23936f, 12.08334f, -203.5708f), new Quaternion(0f, 0.9848078f, 0f, 0.1736482f), Vector3.one),
                                    new TransformData(new Vector3(-86.2956f, 2.2675f, -193.2215f), new Quaternion(0.0997f, 0.8272f, 0.0662f, 0.549f), Vector3.one),
                                }
                            ),
                        }
                    },
                    {
                        "Ladder to Swamp", new List<LadderInfo>() {
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (3) (91.0, 4.0, -158.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(90.99981f, 3.422601f, -159.4951f), new Quaternion(0f, -1.632343E-36f, 0f, 1f), Vector3.one),
                                    new TransformData(new Vector3(91f, 12.08334f, -157.0632f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                },
                                new List<string>() {
                                    "_Bush and Grass/bush (41)/",
                                    "_Bush and Grass/bush (42)/",
                                    "_Bush and Grass/bush (43)/"
                                }
                            ),
                        }
                    },
                    {
                        "Ladders near Dark Tomb", new List<LadderInfo>() {
                            new LadderInfo(
                                "Wooden Ladder (12 unit) (3) (-123.0, 40.0, 26.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-123.3621f, 40.08334f, 24.30477f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                    new TransformData(new Vector3(-123.2551f, 52.08334f, 27.31147f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), Vector3.one),
                                }
                            ),
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (8) (-124.0, 52.0, 44.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-124.8937f, 52f, 43.93379f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                    new TransformData(new Vector3(-123.1313f, 60.08334f, 44.13654f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                }
                            ),
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (9) (-120.0, 60.0, 42.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-121.0667f, 60.08334f, 41.78677f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                    new TransformData(new Vector3(-119.0632f, 66.08334f, 42.00001f), new Quaternion(0f, 0.7071055f, 0f, 0.7071081f), Vector3.one),
                                }
                            ),
                        }
                    },
                    {
                        "Ladder to Quarry", new List<LadderInfo>() {
                            new LadderInfo(
                                "Wooden Ladder (12 unit) (6) (-144.0, 28.5, 38.5)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-145.8903f, 28.08334f, 37.85971f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                    new TransformData(new Vector3(-145.5476f, 28.08334f, 13.37513f), new Quaternion(0f, 0.9414406f, 0f, -0.3371789f), Vector3.one),
                                    new TransformData(new Vector3(-143.0562f, 40.08334f, 38.97885f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), Vector3.one),
                                }
                            ),
                        }
                    },
                    {
                        "Ladders to West Bell", new List<LadderInfo>() {
                            // Ladder on the belltower
                            new LadderInfo(
                                "Wooden Ladder (12 unit) (5) (-135.5, 40.0, -40.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-134.4333f, 52.08334f, -40.16007f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                    new TransformData(new Vector3(-136.9177f, 40.08334f, -40.1241f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                }
                            ),
                            // Middle ladder
                            new LadderInfo(
                                "Wooden Ladder (12 unit) (4) (-152.0, 28.0, -37.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-150.9632f, 40.08334f, -37.59999f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                    new TransformData(new Vector3(-153.5178f, 28.28334f, -37.86783f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                }
                            ),
                            // Ladder from West Garden
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (6) (-160.0, 20.0, -42.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-158.9632f, 28.08334f, -42f), new Quaternion(0f, 0.7071068f, 0f, 0.7071067f), Vector3.one),
                                    new TransformData(new Vector3(-161.4174f, 20.08334f, -42.16743f), new Quaternion(0f, 0.7071068f, 0f, 0.7071067f), Vector3.one),
                                }
                            ),
                        }
                    },
                    { 
                        "Ladders in Well", new List<LadderInfo>() {
                            new LadderInfo(
                                "Wooden Ladder (12 unit) (1) (-34.0, 28.0, 1.5)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-32.66321f, 40.08334f, 1.444585f), new Quaternion(0f, 0.9909583f, 0f, 0.1341702f), Vector3.one),
                                },
                                isEntrance: true
                            ),
                        }
                    },
                    {
                        "Ladder near Temple Rafters", new List<LadderInfo>() {
                            new LadderInfo(
                                "Wooden Ladder (12 unit) (11) (-47.0, 53.8, 24.7)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-47.8368f, 66.08334f, 24.13f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                    new TransformData(new Vector3(-47.00589f, 66.08334f, 26.90848f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), Vector3.one),
                                    new TransformData(new Vector3(-38.65792f, 54.08334f, 25.06667f), new Quaternion(0f, 0.6177738f, 0f, 0.7863559f), Vector3.one),
                                }
                            ),
                        }
                    },
                    {
                        "Ladders near Patrol Cave", new List<LadderInfo>() {
                            new LadderInfo(
                                "Wooden Ladder (12 unit) (10) (63.0, 44.0, 20.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(63.00001f, 44f, 18.95f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                    new TransformData(new Vector3(63f, 55.08334f, 21.1368f), new Quaternion(0f, -1.930792E-06f, 0f, 1f), Vector3.one),
                                }
                            ),
                            new LadderInfo(
                                "Wooden Ladder (12 unit) (9) (49.0, 55.0, 24.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(49.00001f, 55f, 22.95f), new Quaternion(0f, -1.930792E-06f, 0f, 1f), Vector3.one),
                                    new TransformData(new Vector3(49f, 66.08334f, 24.9368f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                }
                            ),
                        }
                    }
                }
            },
            {
                "Forest Belltower", new Dictionary<string, List<LadderInfo>>() {
                    {
                        "Ladder to East Forest", new List<LadderInfo>() {
                            new LadderInfo(
                                "Wooden Ladder (12 unit) (508.0, 26.0, 65.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(508f, 38.1052f, 65.87f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                    new TransformData(new Vector3(511.9763f, 14.00518f, 59.7477f), new Quaternion(0f, 0.5735765f, 0f, 0.8191521f), Vector3.one),
                                    new TransformData(new Vector3(509.4159f, 14.00518f, 63.55082f), new Quaternion(0f, 0.08679222f, 0f, 0.9962264f), Vector3.one),
                                },
                                new List<string>() {
                                    "ladder graphic"
                                }
                            ),
                        }
                    }
                }
            },
            { 
                "East Forest Redux", new Dictionary<string, List<LadderInfo>>() {
                    { 
                        "Ladders to Lower Forest", new List<LadderInfo>() {
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (4) (82.0, -12.0, -4.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(82f, -3.916664f, -3.063198f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                    new TransformData(new Vector3(81.75916f, -11.91667f, -4.885747f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                }
                            ),
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (5) (84.0, -20.0, -18.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(84.9368f, -11.91667f, -17.99999f), new Quaternion(0f, 0.7071055f, 0f, 0.7071081f), Vector3.one),
                                    new TransformData(new Vector3(82.64679f, -19.91667f, -18.10323f), new Quaternion(0f, 0.7071055f, 0f, 0.7071081f), Vector3.one),
                                }
                            ),
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (6) (84.0, -28.0, -24.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(84f, -19.91667f, -23.0632f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                    new TransformData(new Vector3(83.89732f, -27.91667f, -25.35268f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                }
                            ),
                        }
                    }
                }
            },
            {
                "East Forest Redux Interior", new Dictionary<string, List<LadderInfo>>() {
                    { 
                        "Ladders to Lower Forest", new List<LadderInfo>() {
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (164.0, -16.0, -16.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(164.9667f, 0f, -16f), new Quaternion(0f, 0.7071055f, 0f, 0.7071081f), Vector3.one),
                                    new TransformData(new Vector3(162.5605f, -15.91667f, -16.19449f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                }
                            ),
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (1) (151.0, -24.0, -22.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(151f, -15.91667f, -21.13333f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), Vector3.one),
                                    new TransformData(new Vector3(150.9267f, -23.91667f, -22.86667f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                }
                            ),
                        }
                    }
                }
            },
            { 
                "Fortress Courtyard", new Dictionary<string, List<LadderInfo>>() {
                    { 
                        "Ladder to Beneath the Vault", new List<LadderInfo>() {
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (-52.0, -8.0, -41.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-51.06321f, 0.04166794f, -40.99999f), new Quaternion(0f, 0.7071055f, 0f, 0.7071081f), Vector3.one),
                                },
                                isEntrance: true
                            ),
                        }
                    }
                }
            },
            { 
                "Fortress Basement", new Dictionary<string, List<LadderInfo>>() {
                    {
                        "Ladder to Beneath the Vault", new List<LadderInfo>() {
                            new LadderInfo(
                                "ladder (0.5, -8.0, -61.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-0.2499952f, -8f, -61.00268f), new Quaternion(0f, 0.7058433f, 0f, 0.7083681f), Vector3.one),
                                },
                                new List<string>() {
                                    "Room -  Basement/Wooden Ladder (12 unit)",
                                    "Room -  Basement/Wooden Ladder (12 unit) (1)"
                                },
                                isExit: true
                            ),
                        }
                    }
                }
            },
            {
                "Crypt Redux", new Dictionary<string, List<LadderInfo>>() {
                    {
                        "Ladder in Dark Tomb", new List<LadderInfo>() {
                            new LadderInfo(
                                "Ladder (-77.6, 25.0, 76.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-79.11993f, 25.08333f, 74.63903f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                    new TransformData(new Vector3(-76.08321f, 57.08334f, 76.00002f), new Quaternion(0f, 0.7071041f, 0f, 0.7071094f), Vector3.one),
                                    new TransformData(new Vector3(-79.01984f, 57.1981f, 73.16723f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), Vector3.one),
                                }
                            ),
                        }
                    }
                }
            },
            {
                "Town Basement", new Dictionary<string, List<LadderInfo>>() {
                    {
                        "Ladders in Hourglass Cave", new List<LadderInfo>() {
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (2) (-202.0, 3.0, 155.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-202.1221f, 3.083336f, 141.9757f), new Quaternion(0f, -0.02394745f, 0f, 0.9997132f), Vector3.one),
                                }
                            ),
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (-207.0, 8.0, 150.0)",
                                new List<TransformData>(){
                                }
                            ),
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (1) (-202.0, 8.0, 145.0)",
                                new List<TransformData>(){
                                }
                            ),
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (5) (-197.0, 8.0, 150.0)",
                                new List<TransformData>(){
                                }
                            ),
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (6) (-202.0, 13.0, 155.0)",
                                new List<TransformData>(){
                                }
                            ),
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (3) (-207.0, 18.0, 150.0)",
                                new List<TransformData>(){
                                }
                            ),
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (4) (-202.0, 23.0, 145.0)",
                                new List<TransformData>(){
                                }
                            ),
                        }
                    },
                }
            },
            { 
                "Sewer", new Dictionary<string, List<LadderInfo>>() {
                    {
                        "Ladders in Well", new List<LadderInfo>() {
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (320.0, 1.0, 82.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(320.9369f, 11.0622f, 82f), new Quaternion(0f, 0.7071068f, 0f, 0.7071067f), Vector3.one),
                                    new TransformData(new Vector3(319.0251f, 1.002909f, 81.77502f), new Quaternion(0f, 0.7071068f, 0f, 0.7071067f), Vector3.one),
                                }
                            ),
                            new LadderInfo(
                                "Wooden Ladder (12 unit) (392.0, 15.0, 25.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(390.75f, 15.00021f, 25f), new Quaternion(0f, 0.7071068f, 0f, 0.7071067f), Vector3.one),
                                },
                                isExit: true
                            ),
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (374.0, 7.0, 25.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(374.9369f, 15.08333f, 24.99999f), new Quaternion(0f, 0.7071088f, 0f, 0.7071047f), Vector3.one),
                                    new TransformData(new Vector3(372.75f, 7f, 25.00001f), new Quaternion(0f, 0.7071088f, 0f, 0.7071047f), Vector3.one),
                                }
                            ),
                        }
                    }
                }
            },
            { 
                "Atoll Redux", new Dictionary<string, List<LadderInfo>>() {
                    { 
                        "Ladders to Frog's Domain", new List<LadderInfo>() {
                            new LadderInfo(
                                // ladder outside of frog eye
                                "Wooden Ladder (12 unit) (6) (93.5, 5.5, 63.5)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(93.85339f, 4.588062f, 64.3034f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                }
                            ),
                        }
                    },
                    { 
                        "Ladders in South Atoll", new List<LadderInfo>() {
                            new LadderInfo(
                                // ladder left of gate
                                "Wooden Ladder (8 unit) (-32.0, 4.9, -74.8)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-32.01513f, 13.01159f, -72.88746f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), Vector3.one),
                                    new TransformData(new Vector3(-32.14867f, 4.639985f, -75.40778f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                }
                            ),
                            new LadderInfo(
                                // ladder right of gate (broken building)
                                "Wooden Ladder (12 unit) (3) (6.0, 1.9, -77.3)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(6.933334f, 16.99422f, -77.32423f), new Quaternion(0f, 0.7071041f, 0f, 0.7071094f), Vector3.one),
                                    new TransformData(new Vector3(4.934871f, 1.444999f, -77.42425f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                }
                            ),
                            new LadderInfo(
                                // ladder right side of power line
                                "Wooden Ladder (12 unit) (2) (37.0, 3.0, -70.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(36.77802f, 16.07755f, -70.89441f), new Quaternion(0f, 1f, 0f, 4.247823E-06f), Vector3.one),
                                    new TransformData(new Vector3(36.99999f, 3f, -68.35001f), new Quaternion(0f, 1f, 0f, 3.770986E-06f), Vector3.one),
                                }
                            ),
                            new LadderInfo(
                                // lower ladder near fuse
                                "Wooden Ladder (8 unit) (72.0, 5.0, -96.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(71.83218f, 4.852236f, -97.01783f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                    new TransformData(new Vector3(71.99625f, 13.07755f, -83.59938f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), Vector3.one),
                                }
                            ),
                            new LadderInfo(
                                // ladder to fuse
                                "Wooden Ladder (12 unit) (5) (64.0, 13.0, -72.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(66.90861f, 25.09135f, -71.92496f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                    new TransformData(new Vector3(63.15f, 13f, -72f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                }
                            ),
                        }
                    }
                }
            },
            { 
                "Frog Stairs", new Dictionary<string, List<LadderInfo>>() {
                    { 
                        "Ladders to Frog's Domain", new List<LadderInfo>() {
                            new LadderInfo(
                                // ladder inside of frog eye
                                "Wooden Ladder (12 unit) (196.0, 106.0, -63.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(196f, 118.0417f, -62.06322f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), Vector3.one),
                                    new TransformData(new Vector3(196.1115f, 106.125f, -63.7667f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                }
                            ),
                            new LadderInfo(
                                // ladder near mouth
                                "Wooden Ladder (8 unit) (2) (206.0, 74.3, -69.1)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(207.0368f, 106.0937f, -69.125f), new Quaternion(0f, 0.7071068f, 0f, 0.7071067f), Vector3.one),
                                    new TransformData(new Vector3(205.193f, 74.29167f, -69.31693f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), Vector3.one),
                                }
                            ),
                            new LadderInfo(
                                // small ladder after big one
                                "Wooden Ladder (8 unit) (202.0, 66.4, -68.5)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(203.0368f, 74.29167f, -68.5f), new Quaternion(0f, 0.7071068f, 0f, 0.7071067f), Vector3.one),
                                    new TransformData(new Vector3(201f, 66.45834f, -68.66667f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                }
                            ),
                            new LadderInfo(
                                // ladder after frog
                                "Wooden Ladder (8 unit) (1) (180.0, 38.4, -74.8)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(180.013f, 50.45834f, -73.80001f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                    new TransformData(new Vector3(180f, 38.475f, -75.50001f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), Vector3.one),
                                }
                            ),
                            new LadderInfo(
                                // lowest ladder (to frog's domain)
                                "Wooden Ladder (12 unit) to frog cave (180.0, -5.3, -90.6)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(180f, 18.9084f, -90.0186f), new Quaternion(0.2164397f, 0f, 0f, -0.976296f), Vector3.one),
                                },
                                isEntrance: true
                            ),
                        }
                    }
                }
            },
            { 
                "frog cave main", new Dictionary<string, List<LadderInfo>>() {
                    { 
                        "Ladders to Frog's Domain", new List<LadderInfo>() {
                            new LadderInfo(
                                // upper ladder
                                "Wooden Ladder (8 unit) (4) (36.0, 52.0, -16.1)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(34.95f, 52.1f, -16.10754f), new Quaternion(0f, 0.7071061f, 0f, 0.7071075f), Vector3.one),
                                },
                                isExit: true
                            ),
                            new LadderInfo(
                                // middle ladder
                                "Wooden Ladder (8 unit) (3) (32.0, 44.0, -16.1)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(31.15f, 44.1f, -16.10754f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                }
                            ),
                            new LadderInfo(
                                // lower ladder
                                "Wooden Ladder (8 unit) (28.0, 28.0, -16.1)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(27.15f, 28f, -15.90754f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                }
                            ),
                        }
                    }
                }
            },
            {
                "Swamp Redux 2", new Dictionary<string, List<LadderInfo>>() {
                    {
                        "Ladders in Swamp", new List<LadderInfo>() {
                            new LadderInfo(
                                // shortcut ladder past night bridge
                                "Ladder (13.9, 0.0, 42.5)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(25.68414f, 18.04625f, 45.95081f), new Quaternion(0f, 0.3826835f, 0f, 0.9238795f), Vector3.one),
                                    new TransformData(new Vector3(13.81997f, 18.04625f, 44.89999f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), Vector3.one),
                                    new TransformData(new Vector3(13.64049f, -0.03708649f, 41.53049f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                },
                                true,
                                optionalStateVar: "SV_Swamp Redux 2_rope shortcut 8u"
                            ),
                            new LadderInfo(
                                // ladder near cathedral door
                                "Wooden Ladder (12 unit) (100.0, 14.0, 141.3)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(98.53879f, 26.05005f, 138.8478f), new Quaternion(0f, 0.3821941f, 0f, 0.9240821f), Vector3.one),
                                    new TransformData(new Vector3(92.34142f, 14.04625f, 132.0744f), new Quaternion(0f, 0.6987836f, 0f, 0.7153332f), Vector3.one),
                                }
                            ),
                            new LadderInfo(
                                // Central swamp ladder
                                "Wooden Ladder (8 unit) (28.0, 0.0, -32.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(29.88001f, 8.118482f, -33.28009f), new Quaternion(0f, 0.9999996f, 0f, 0.0009153643f), Vector3.one),
                                    new TransformData(new Vector3(26.94685f, -0.03708649f, -31.93066f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                }
                            ),
                        }
                    }
                }
            },
            { 
                "Quarry Redux", new Dictionary<string, List<LadderInfo>>() {
                    {
                        "Ladders in Lower Quarry", new List<LadderInfo>() {
                            new LadderInfo(
                                "ladder (-91.3, -52.0, -89.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-89.26022f, -44.06429f, -89.06386f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                    new TransformData(new Vector3(-91.11002f, -52.89999f, -89f), new Quaternion(0f, 0.7071068f, 0f, 0.7071067f), Vector3.one),
                                },
                                isSpecialLadder: true
/*                                optionalObjectsToDisable: new List<string>() {
                                    "_Environment-Prefabs/quarry_ladder_8u/quarry_ladder_top graphic"
                                }*/
                            ),
                            new LadderInfo(
                                "ladder (-102.3, -61.0, -86.1)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-100.1232f, -52.88604f, -86.125f), new Quaternion(0f, 0.7071068f, 0f, 0.7071067f), Vector3.one),
                                    new TransformData(new Vector3(-102.3911f, -68.74783f, -86.45453f), new Quaternion(0f, 0.7071068f, 0f, 0.7071067f), Vector3.one),
                                },
                                isSpecialLadder: true
/*                                optionalObjectsToDisable: new List<string>() {
                                    "_Environment-Prefabs/quarry_ladder_8u (1)/quarry_ladder_top graphic"
                                }*/
                            ),
                        }
                    }
                }
            },
            { 
                "Library Exterior", new Dictionary<string, List<LadderInfo>>() {
                    { 
                        "Ladders in Library", new List<LadderInfo>() {
                            new LadderInfo(
                                "Wooden Ladder (12 unit) (22.0, 44.0, 6.3)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(22.95f, 44f, 6.250004f), new Quaternion(0f, 0.7071078f, 0f, -0.7071058f), Vector3.one),
                                    new TransformData(new Vector3(21.0632f, 54.06445f, 6.249998f), new Quaternion(0f, 0.7071078f, 0f, -0.7071058f), Vector3.one),
                                }
                            ),
                        }
                    }
                }
            },
            { 
                "Library Hall", new Dictionary<string, List<LadderInfo>>() {
                    { 
                        "Ladders in Library", new List<LadderInfo>() {
                            new LadderInfo(
                                "ladder (161.3, 9.9, -70.4)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(162.6465f, 21.01357f, -70.46925f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                    new TransformData(new Vector3(161.1601f, 9.959583f, -70.4287f), new Quaternion(0f, 0.7124169f, 0f, 0.7017565f), Vector3.one),
                                },
                                isSpecialLadder: true
                            ),
                            new LadderInfo(
                                "Wooden Ladder (12 unit) to upper level (115.0, 26.3, 8.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(115f, 26.11068f, 6.95f), new Quaternion(0f, -3.861584E-06f, 0f, 1f), Vector3.one),
                                },
                                isExit: true
                            ),
                        }
                    }
                }
            },
            { 
                "Library Rotunda", new Dictionary<string, List<LadderInfo>>() {
                    { 
                        "Ladders in Library", new List<LadderInfo>() {
                            new LadderInfo(
                                "Wooden Ladder (8 unit) (-30.1, -16.0, 33.1)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-30.6415f, -7.829804f, 32.47433f), new Quaternion(0f, 0.9415439f, 0f, -0.3368903f), Vector3.one),
                                },
                                isEntrance: true
                            ),
                            new LadderInfo(
                                "Wooden Ladder (12 unit) (-4.1, -5.6, 20.4)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-2.881019f, -5.925f, 20.49752f), new Quaternion(0f, 0.7409512f, 0f, -0.6715589f), Vector3.one),
                                },
                                isExit: true
                            ),
                        }
                    }
                }
            },
            { 
                "Library Lab", new Dictionary<string, List<LadderInfo>>() {
                    {
                        "Ladders in Library", new List<LadderInfo>() {
                            new LadderInfo(
                                "Wooden Ladder (8 unit) entrance (139.0, 79.0, -118.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(139.9368f, 87.08334f, -118f), new Quaternion(0f, 0.7071068f, 0f, 0.7071067f), Vector3.one),
                                },
                                isEntrance: true
                            ),
                            new LadderInfo(
                                "Wooden Ladder (12 unit) (123.5, 95.0, -44.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(123.9998f, 107.1384f, -43.06321f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                    new TransformData(new Vector3(123.5f, 95.04955f, -44.75001f), new Quaternion(0f, -3.861584E-06f, 0f, 1f), Vector3.one),
                                }
                            ),
                            new LadderInfo(
                                "Wooden Ladder (12 unit) (1) (147.5, 95.0, -50.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(147.5f, 99.08334f, -49.06321f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                    new TransformData(new Vector3(147.5f, 95.04958f, -50.95f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                }
                            ),
                            new LadderInfo(
                                "Wooden Ladder (to arena) (155.0, 95.0, -59.0)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(154.05f, 95.04998f, -59.00001f), new Quaternion(0f, 0.7071068f, 0f, 0.7071068f), Vector3.one),
                                },
                                isExit: true
                            ),
                        }
                    }
                }
            },
            { 
                "Library Arena", new Dictionary<string, List<LadderInfo>>() {
                    {
                        "Ladders in Library", new List<LadderInfo>() {
                            new LadderInfo(
                                "ladders upper (-0.1, -5.0, -11.8)",
                                new List<TransformData>(){
                                    new TransformData(new Vector3(-0.06000001f, -0.9255302f, -10.0132f), new Quaternion(0f, 0f, 0f, 1f), Vector3.one),
                                    new TransformData(new Vector3(-4.428625E-05f, -5.926672f, -11.71998f), new Quaternion(0.1464072f, 0f, 0f, -0.9892244f), Vector3.one),
                                },
                                optionalObjectsToDisable: new List<string>() {
                                    "_Environment (Tower Geo)/ladder graphics/"
                                }
                            ),
                        }
                    }
                }
            }
        };
    }
}