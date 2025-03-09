using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using UnityEngine.UI;

namespace TunicRandomizer {

    public class FuseCheckHelper : MonoBehaviour {
        public int OriginalGuid;
        public int FakeGuid;
        public GameObject Sign;
        public string CheckId {
            get => $"{OriginalGuid} [{gameObject.scene.name}]";
        }
    }

    public class FuseTrapAppearanceHelper : MonoBehaviour {
        public UVScroller UVScroller;

        public void Start() {
            if (GetComponent<UVScroller>() != null) {
                UVScroller = (UVScroller)GetComponent<UVScroller>();
            }
        }
        public void Update() {
            if (GetComponent<UVScroller>() != null && UVScroller == null) {
                UVScroller = (UVScroller)GetComponent<UVScroller>();
            }
            if (UVScroller == null) {
                return;
            }

            if (UVScroller != null) {
                if (UVScroller.material != null) {
                    UVScroller.material.SetTexture("_EmissionMap", ModelSwaps.FuseAltLights.GetComponent<Image>().mainTexture);
                }
            }

        }
    }

    public struct FuseInformation {
        public int RealGuid;
        public int FakeGuid;
        public Vector3 Position;
        public string SceneName;
        public string FuseItem;
        public List<string> PowerRequirements;
        public List<string> FusePath;
        public string FuseCheckId {
            get => $"{RealGuid} [{SceneName}]";
        }

        public FuseInformation(int realGuid, int fakeGuid, Vector3 position, string sceneName, string fuseItem, List<string> powerRequirements, List<string> fusePath) {
            RealGuid = realGuid;
            FakeGuid = fakeGuid;
            Position = position;
            SceneName = sceneName;
            FuseItem = fuseItem;
            PowerRequirements = powerRequirements;
            FusePath = fusePath;
        }
    }

    public class FuseRandomizer {

        public const string fusePrefabRoot = "_Checkpoints, Fuses, Platform/TUNIC_Fuse_Big";
        public static string fuseFilePath = $"{Application.persistentDataPath}/Randomizer/Fuses.json";

        public static Dictionary<int, FuseInformation> Fuses = new Dictionary<int, FuseInformation>();
        public static Dictionary<string, Check> FuseChecks = new Dictionary<string, Check>();

        public static GameObject FusePrefab;
        
        public static bool ModifiedFusesAlready = false;

        public static void Setup() {
            LoadFuseChecks();
            CreateFuseItems();
            InstantiateFusePrefab();
            ItemPresentationPatches.SetupFusePresentation();
        }
        public static void LoadFuseChecks() {
            FuseChecks.Clear();
            var assembly = Assembly.GetExecutingAssembly();
            var fuseJson = "TunicRandomizer.src.Data.Fuses.json";
            var fuseLocationNames = "TunicRandomizer.src.Data.FuseDescriptions.json";

            using (Stream fuseStream = assembly.GetManifestResourceStream(fuseJson))
            using (StreamReader reader = new StreamReader(fuseStream)) {
                List<Check> checks = JsonConvert.DeserializeObject<List<Check>>(reader.ReadToEnd());
                foreach (Check check in checks) { 
                    int fuseGuid = int.Parse(check.Location.LocationId);
                    int fakeGuid = 9000 + FuseChecks.Count;
                    
                    ConduitDataEntry entry = new ConduitDataEntry();
                    entry.connections = new int[] { 0 };
                    entry.guid = fakeGuid;
                    entry.sceneName = check.Location.SceneName;
                    entry.prettyName = check.Reward.Name;
                    entry.isFuse = true;
                    entry.isLockingTerminus = true;
                    ConduitData.DataObject.data.Add(entry);

                    FuseChecks.Add(check.CheckId, check);

                    List<string> requiredFuseItems = new List<string>();
                    foreach (Dictionary<string, int> reqs in check.Location.Requirements) {
                        foreach(string key in reqs.Keys) {
                            if (!requiredFuseItems.Contains(key) && ItemLookup.Items.ContainsKey(key) && ItemLookup.Items[key].Type == ItemTypes.FUSE) {
                                requiredFuseItems.Add(key);
                            }
                        }
                    }
                    FuseInformation fuseInfo = new FuseInformation(fuseGuid, fakeGuid, TunicUtils.StringToVector3(check.Location.Position), check.Location.SceneName, check.Reward.Name, requiredFuseItems, FusePaths[fuseGuid]);
                    Fuses.Add(fuseGuid, fuseInfo);
                    Fuses.Add(fakeGuid, fuseInfo);
                }
            }

            using (Stream fuseNameStream = assembly.GetManifestResourceStream(fuseLocationNames))
            using (StreamReader reader = new StreamReader(fuseNameStream)) {
                Dictionary<string, string> map = JsonConvert.DeserializeObject<Dictionary<string, string>>(reader.ReadToEnd());
                foreach (KeyValuePair<string, string> pair in map) { 
                    Locations.LocationIdToDescription.Add(pair.Key, pair.Value);
                    Locations.LocationDescriptionToId.Add(pair.Value, pair.Key);
                }
            }
        }

        public static void CreateFuseItems() {
            foreach (ItemData FuseItem in ItemLookup.Items.Values.Where(item => item.Type == ItemTypes.FUSE)) {
                CreateFuseItem(FuseItem.Name);
                TextBuilderPatches.ItemNameToAbbreviation.Add(FuseItem.Name, "[fuse]");
            }
        }

        private static void CreateFuseItem(string name) {
            SpecialItem Fuse = ScriptableObject.CreateInstance<SpecialItem>();

            Fuse.name = name;
            Fuse.collectionMessage = TunicUtils.CreateLanguageLine($"\"{Fuse.name.Replace(" Fuse", "")}\"");
            Fuse.controlAction = "";
            Fuse.icon = ModelSwaps.FindSprite("Randomizer items_fuse");
            Inventory.itemList.Add(Fuse);
        }

        public static void InstantiateFusePrefab() {
            if (FusePrefab == null) {
                GameObject fuse = GameObject.Find(fusePrefabRoot);
                if (fuse != null) {
                    FusePrefab = GameObject.Instantiate(fuse);
                    FusePrefab.GetComponent<ConduitNode>().externalConnection.guid = -1000;
                    //FusePrefab.GetComponent<ConduitNode>().Guid = -1;
                    FusePrefab.SetActive(true);
                    FusePrefab.transform.position = new Vector3(-30000, -30000, -30000);
                    GameObject.DontDestroyOnLoad(FusePrefab);
                }
            }
        }

        public static void FuseCloseAnimationHelper___animationEvent_fuseCloseAnimationDone_PostfixPatch(FuseCloseAnimationHelper __instance) {
            if (SaveFile.GetInt(SaveFlags.FuseShuffleEnabled) == 1) {
                Fuse fuse = __instance.GetComponentInParent<Fuse>();
                ConduitNode node = __instance.GetComponentInParent<ConduitNode>();
                FuseCheckHelper fuseHelper = __instance.GetComponentInParent<FuseCheckHelper>();
                if (fuse != null && node != null && fuseHelper != null) {
                    TunicLogger.LogInfo("fuse closed " + node.Guid);
                    string CheckId = fuseHelper.CheckId;
                
                    if (SaveFlags.IsArchipelago()) {
                        Archipelago.instance.ActivateCheck(Locations.LocationIdToDescription[CheckId]);
                    } else if (SaveFlags.IsSinglePlayer() && Locations.RandomizedLocations.ContainsKey(CheckId)) {
                        Check check = Locations.RandomizedLocations[CheckId];
                        ItemPatches.GiveItem(check);
                    }

                    if (fuseHelper.Sign != null) {
                        fuseHelper.Sign.SetActive(true);
                    }

                    MoveUp moveUp = fuse.GetComponentInChildren<MoveUp>(includeInactive: true);
                    if (moveUp != null) {
                        moveUp.transform.position += new Vector3(0, 2, 0);
                        moveUp.gameObject.SetActive(true);
                    }
                    SaveFile.SetInt("randomizer fuse closed " + node.Guid, 1);

                    if (node.Guid == 9003 && node.gameObject.scene.name == "Cathedral Redux") {
                        StateVariable.GetStateVariableByName("SV_cathedral elevator").BoolValue = false;
                        StateVariable.GetStateVariableByName("SV_cathedral elevator ever been on").BoolValue = false;
                    }
                }
            }
        }

        public static string GetFuseCheckId(Fuse __instance) {
            ConduitNode node = __instance.GetComponent<ConduitNode>();
            if (node != null) {
                return $"{node.Guid} [{__instance.gameObject.scene.name}]";
            }
            TunicLogger.LogError("Could not find fuse check id for fuse " + __instance.name + " in " + __instance.gameObject.scene.name);
            return null;
        }

        public static FuseInformation GetFuseInformationByFuseItem(string FuseItem) {
            return Fuses.Values.Where(fuse => fuse.FuseItem == FuseItem).First();
        }

        public static void ModifyFuses() {
            string sceneName = SceneManager.GetActiveScene().name;

            foreach (Fuse fuse in Resources.FindObjectsOfTypeAll<Fuse>().Where(fuse => fuse.gameObject.scene.name == sceneName && fuse.GetComponent<ConduitNode>() != null)) {
                string fuseId = GetFuseCheckId(fuse);
                if (FuseChecks.ContainsKey(fuseId)) {
                    GameObject newFuseObject = GameObject.Instantiate(fuse.gameObject, fuse.transform.position, fuse.transform.rotation);

                    Fuse newFuse = newFuseObject.GetComponent<Fuse>();
                    newFuse.startTall = fuse.startTall;
 
                    ConduitNode newNode = newFuse.GetComponent<ConduitNode>();
                    ConduitNode oldNode = fuse.GetComponent<ConduitNode>();
                    newNode.Guid = Fuses[oldNode.Guid].FakeGuid;
                    newNode.externalConnection.guid = 0;
                    newNode.internalConnections.Clear();
                    
                    FuseCheckHelper helper = newFuseObject.gameObject.AddComponent<FuseCheckHelper>();
                    helper.OriginalGuid = oldNode.Guid;
                    helper.FakeGuid = newNode.Guid;
                    
                    newFuseObject.SetActive(true);
                    newFuseObject.name = "randomizer fake fuse " + newNode.Guid;

                    newFuseObject.transform.GetChild(1).GetComponent<MeshRenderer>().materials =
                        fuse.transform.GetChild(1).GetComponent<MeshRenderer>().materials;
                    newFuse.Start();
                    fuse.gameObject.SetActive(false);

                    GameObject rigidBody = new GameObject($"fuse construction sign {newNode.Guid}");
                    rigidBody.transform.position = fuse.transform.position;
                    rigidBody.transform.rotation = fuse.transform.rotation;

                    GameObject ConstructionSign = GameObject.Instantiate(ModelSwaps.UnderConstruction, fuse.transform.position, fuse.transform.rotation);
                    ConstructionSign.transform.parent = rigidBody.transform;
                    if (!SaveFlags.GetBool($"fuseClosed {newNode.Guid}")) {
                        rigidBody.transform.localPosition += new Vector3(0, 20, 0);
                        rigidBody.SetActive(false);
                    }
                    ConstructionSign.GetComponent<UnderConstruction>().isFuseSign = true;
                    ConstructionSign.GetComponent<UnderConstruction>().fuses = Fuses[newNode.Guid].FusePath;
                    ConstructionSign.GetComponent<Signpost>().message = ScriptableObject.CreateInstance<LanguageLine>();
                    ConstructionSign.GetComponent<Signpost>().message.text = $"<#FF0000>[death] kawndooit rehstorA$uhn <#FF0000>[death]";
                    ConstructionSign.SetActive(SaveFlags.GetBool($"fuseClosed {newNode.Guid}"));
                    ConstructionSign.SetActive(true);
                    ConstructionSign.AddComponent<ToggleObjectByFuseItem>().Fuse = Fuses[newNode.Guid];
                    helper.Sign = rigidBody;
                    rigidBody.AddComponent<Rigidbody>();
                }
            }

            if (sceneName == "Overworld Redux" && GameObject.Find("ladder and fuse checklist") == null) {
                LadderToggles.SpawnOverworldChecklistSign();
            }

            if ((sceneName == "ziggurat2020_3" || sceneName == "ziggurat2020_1") && !SaveFlags.GetBool(SaveFlags.EntranceRando)) {
                if ((SaveFile.GetInt("fuseClosed 1117") == 0 || SaveFile.GetInt("fuseClosed 1121") == 0 || Inventory.GetItemByName("Sword").Quantity == 0 ||
                    (SaveFile.GetInt(SaveFlags.PrayerUnlocked) == 0 && SaveFile.GetInt(SaveFlags.AbilityShuffle) == 1)) 
                    && Inventory.GetItemByName("Torch").Quantity == 0) {
                    SpawnZigguratEscapePortal();
                }
            }

            if (sceneName == "Library Lab" && !SaveFlags.GetBool(SaveFlags.EntranceRando)) {
                if ((Inventory.GetItemByName("Hyperdash").Quantity == 0 || SaveFile.GetInt("fuseClosed 1055") == 0 || 
                    (SaveFile.GetInt(SaveFlags.PrayerUnlocked) == 0 && SaveFile.GetInt(SaveFlags.AbilityShuffle) == 1))
                    && Inventory.GetItemByName("Torch").Quantity == 0) {
                    SpawnLibraryEscapePortal();
                }
            }

            if (FarShoreSignPlacements.ContainsKey(sceneName)) {
                foreach (KeyValuePair<string, (int, Vector3)> pair in FarShoreSignPlacements[sceneName]) {
                    GameObject teleporter = GameObject.Find(pair.Key);
                    if (teleporter != null) {
                        GameObject rigidBody = new GameObject($"teleporter construction sign {sceneName}");
                        rigidBody.transform.parent = teleporter.transform;
                        rigidBody.transform.position = teleporter.transform.position;
                        rigidBody.transform.rotation = teleporter.transform.rotation;
                        GameObject sign = GameObject.Instantiate(ModelSwaps.UnderConstruction, teleporter.transform.position, teleporter.transform.rotation);
                        sign.transform.parent = rigidBody.transform;
                        sign.transform.localEulerAngles = pair.Value.Item2;
                        sign.transform.localPosition = new Vector3(0, 0.6f, 0);
                        sign.GetComponent<UnderConstruction>().isFuseSign = true;
                        sign.GetComponent<Signpost>().message = ScriptableObject.CreateInstance<LanguageLine>();
                        sign.GetComponent<Signpost>().message.text = $"<#FF0000>[death] kawndooit rehstorA$uhn <#FF0000>[death]";
                        sign.SetActive(true);
                        sign.AddComponent<ToggleObjectByFuseItem>().Fuse = Fuses[pair.Value.Item1];
                        sign.GetComponent<UnderConstruction>().fuses = Fuses[pair.Value.Item1].FusePath;
                        rigidBody.AddComponent<Rigidbody>();
                    }
                }
            }

            ModifiedFusesAlready = true;
        }

        public static void UpdateFuseVisualState(int fuseId) {
            ConduitNode node = FusePrefab.GetComponent<ConduitNode>();
            Fuse fuse = FusePrefab.GetComponent<Fuse>();
            if (node != null && fuse != null) {
                node.Guid = fuseId;
                fuse.mpToGiveDat.Value = 0;
                FuseCloseAnimationHelper helper = FusePrefab.GetComponentInChildren<FuseCloseAnimationHelper>(true);
                if (helper != null) {
                    helper.__animationEvent_fuseCloseAnimationDone();
                }
            }
            if (fuseId == 1300) {
                StateVariable.GetStateVariableByName("SV_cathedral elevator").BoolValue = true;
                StateVariable.GetStateVariableByName("SV_cathedral elevator ever been on").BoolValue = true;
            }
        }

        public static bool ConduitNode_CheckConnectedToPower_PrefixPatch(ConduitNode __instance, ref bool __result) {
            if (TunicRandomizer.Settings.EnableAllCheckpoints && __instance != null && __instance.GetComponent<Campfire>() != null && __instance.GetComponent<UpgradeAltar>() != null) {
                __result = true;
                return false;
            }
            if (__instance.Guid >= 9000 && Fuses.ContainsKey(__instance.Guid) && SaveFlags.GetBool(SaveFlags.FuseShuffleEnabled)) {
                __result = true;
                foreach (string fuseItem in Fuses[__instance.Guid].PowerRequirements) {
                    if (Inventory.GetItemByName(fuseItem) != null && Inventory.GetItemByName(fuseItem).Quantity == 0) {
                        __result = false;
                    }
                }
                return false;
            }
            return true;
        }

        public static bool ConduitData_CheckConnectedToPower_PrefixPatch(ConduitData __instance, ref int guid, ref bool __result) {
            if (SceneManager.GetActiveScene().name == "Quarry") {
                __result = true;
                return false;
            }
            if (guid >= 9000 && Fuses.ContainsKey(guid) && SaveFlags.GetBool(SaveFlags.FuseShuffleEnabled)) {
                __result = true;
                foreach (string fuseItem in Fuses[guid].PowerRequirements) {
                    if (Inventory.GetItemByName(fuseItem) != null && Inventory.GetItemByName(fuseItem).Quantity == 0) {
                        __result = false;
                    }
                }
                return false;
            }
            return true;
        }

        public static bool ConduitData_IsFuseClosedByID_PrefixPatch(ConduitData __instance, ref int guid, ref bool __result) {
            if (SceneManager.GetActiveScene().name == "Quarry") {
                __result = true;
                return false;
            }
            return true;
        }


        public static void SpawnZigguratEscapePortal() {
            GameObject portal = GameObject.Instantiate(SceneLoaderPatches.SpiritArenaTeleporterPrefab);
            portal.transform.localScale = Vector3.one;
            if (portal.GetComponentInChildren<ScenePortal>() != null) {
                portal.GetComponentInChildren<ScenePortal>().id = "customfasttravel_spawnid";
                portal.GetComponentInChildren<ScenePortal>().destinationSceneName = "ziggurat2020_0";
                portal.GetComponentInChildren<ScenePortal>().optionalIDToSpawnAt = "customfasttravel_spawnid";
                portal.SetActive(true);
            }

            if (portal.scene.name == "ziggurat2020_3") {
                portal.transform.position = new Vector3(77.1016f, 3.5704f, 60.888f);
                GameObject portal2 = GameObject.Instantiate(portal);
                portal2.transform.localScale = Vector3.one;
                portal2.transform.position = new Vector3(393.0816f, -48.4296f, -128.7851f);
                portal2.SetActive(true);
            } else if(portal.scene.name == "ziggurat2020_1") {
                portal.transform.position = new Vector3(111.7893f, 137.6833f, 77.0541f);
            }
        }

        public static void SpawnZigguratEscapePoint() {
            GameObject spawn = new GameObject("ziggurat escape spawn point from zig 1");
            spawn.AddComponent<PlayerCharacterSpawn>();
            spawn.GetComponent<PlayerCharacterSpawn>().id = "ziggurat2020_1_customfasttravel_spawnid";
            spawn.transform.position = new Vector3(15.9234f, 2.0023f, -4.1708f);
            spawn.transform.localEulerAngles = new Vector3(180, 0, 0);
            spawn.SetActive(true);

            GameObject spawn2 = new GameObject("ziggurat escape spawn point from zig 3");
            spawn2.AddComponent<PlayerCharacterSpawn>();
            spawn2.GetComponent<PlayerCharacterSpawn>().id = "ziggurat2020_3_customfasttravel_spawnid";
            spawn2.transform.position = new Vector3(15.9234f, 2.0023f, -4.1708f);
            spawn2.transform.localEulerAngles = new Vector3(180, 0, 0);
            spawn2.SetActive(true);
        }

        public static void SpawnLibraryEscapePortal() {
            GameObject portal = GameObject.Instantiate(SceneLoaderPatches.SpiritArenaTeleporterPrefab);
            portal.transform.localScale = Vector3.one;
            portal.transform.position = new Vector3(142.9628f, 96.7144f, -34.6715f);
            if (portal.GetComponentInChildren<ScenePortal>() != null) {
                portal.GetComponentInChildren<ScenePortal>().id = "customfasttravel_spawnid";
                portal.GetComponentInChildren<ScenePortal>().destinationSceneName = "Library Exterior";
                portal.GetComponentInChildren<ScenePortal>().optionalIDToSpawnAt = "customfasttravel_spawnid";
                portal.SetActive(true);
            }
        }

        public static void SpawnLibraryEscapePoint() {
            GameObject spawn = new GameObject("library escape spawn point from lab");
            spawn.AddComponent<PlayerCharacterSpawn>();
            spawn.GetComponent<PlayerCharacterSpawn>().id = "Library Lab_customfasttravel_spawnid";
            spawn.transform.position = new Vector3(1.8175f, 44.0645f, -11.6797f);
            spawn.transform.localEulerAngles = new Vector3(0, 180, 0);
            spawn.SetActive(true);
        }

        public static string GetFuseStatusForSign(List<string> Fuses) {
            string header = $"             <#FF0000>[death] kawndooit rehstorA$uhn <#FF0000>[death]<#FFFFFF>         [fuse]\n";
            string message = header;
            int i = 0;
            foreach (string fuse in Fuses) { 
                Item item = Inventory.GetItemByName(fuse);
                message += $"\"<size=85%>{item.name}{new String('.', 30 - item.name.Length)}{(item.Quantity == 0 ? "<#FF0000>Not Found" : "....<#00FF00>Found")}\"\n";

                i++;
                if (i % 8 == 0 && Fuses.Count != i) {
                    message += $"---{header}\n";
                }
            }
            
            return message;
        }


        public static Dictionary<string, Dictionary<string, (int, Vector3)>> FarShoreSignPlacements = new Dictionary<string, Dictionary<string, (int, Vector3)>>() {
            {
                "Transit",
                new Dictionary<string, (int, Vector3)>() {
                    {
                        "_Platforms/TUNIC_Platform_Small/", // fortress
                        (1015, new Vector3(0, 180, 0))
                    },
                    {
                        "_Platforms/TUNIC_Platform_Small (6)/", // quarry
                        (1262, new Vector3(0, 180, 0))
                    },
                    {
                        "_Platforms/TUNIC_Platform_Small (2)/", // west garden
                        (1032, new Vector3(0, 90, 0))
                    },
                    {
                        "_Platforms/TUNIC_Platform_Small (4)/", // library
                        (1055, new Vector3 (0, 180, 0))
                    },
                }
            },
            {
                "Quarry Redux",
                new Dictionary<string, (int, Vector3)>() {
                    {
                        "TUNIC_Platform_Small/",
                        (1262, new Vector3(0, 90, 0))
                    }
                }
            },
            {
                "Fortress Arena",
                new Dictionary<string, (int, Vector3)>() {
                    {
                        "_Conduit Stuff/TUNIC_Platform_Small (1)/",
                        (1015, new Vector3(0, 180, 0))
                    }
                }
            },
            {
                "Archipelagos Redux",
                new Dictionary<string, (int, Vector3)>() {
                    {
                        "_Checkpoints, Fuses, Platform/TUNIC_Platform_Small/",
                        (1032, new Vector3(0, 90, 0))
                    }
                }
            },
            {
                "Library Lab",
                new Dictionary<string, (int, Vector3)>() {
                    {
                        "Room - Lab/STOLEN TECH/TUNIC_Platform_Small/",
                        (1055, new Vector3(0, 180, 0))
                    }
                }
            }
        };

        public static Dictionary<int, List<string>> FusePaths = new Dictionary<int, List<string>>() {
            { 
                1096, // Swamp Fuse 1
                new List<string>() {
                    "Swamp Fuse 1",
                    "Swamp Fuse 2",
                    "Swamp Fuse 3",
                }
            },
            {
                1240, // Swamp Fuse 2
                new List<string>() {
                    "Swamp Fuse 1",
                    "Swamp Fuse 2",
                    "Swamp Fuse 3",
                }
            },
            {
                1239, // Swamp Fuse 3
                new List<string>() {
                    "Swamp Fuse 1",
                    "Swamp Fuse 2",
                    "Swamp Fuse 3",
                }
            },
            {
                1300, // Cathedral Elevator Fuse
                new List<string>() {
                    "Cathedral Elevator Fuse",
                }
            },
            {
                1305, // West Furnace Fuse
                new List<string>() {
                    "West Furnace Fuse",
                }
            },
            {
                1032, // West Garden Fuse
                new List<string>() {
                    "West Garden Fuse",
                }
            },
            {
                949, // Atoll Northwest Fuse
                new List<string>() {
                    "Atoll Northwest Fuse",
                    "Atoll Northeast Fuse",
                    "Atoll Southeast Fuse",
                    "Atoll Southwest Fuse",
                }
            },
            {
                952, // Atoll Northeast Fuse
                new List<string>() {
                    "Atoll Northwest Fuse",
                    "Atoll Northeast Fuse",
                    "Atoll Southeast Fuse",
                    "Atoll Southwest Fuse",
                }
            },
            {
                951, // Atoll Southeast Fuse
                new List<string>() {
                    "Atoll Northwest Fuse",
                    "Atoll Northeast Fuse",
                    "Atoll Southeast Fuse",
                    "Atoll Southwest Fuse",
                }
            },
            {
                950, // Atoll Southwest Fuse
                new List<string>() {
                    "Atoll Northwest Fuse",
                    "Atoll Northeast Fuse",
                    "Atoll Southeast Fuse",
                    "Atoll Southwest Fuse",
                }
            },
            {
                1055, // Library Lab Fuse
                new List<string>() {
                    "Library Lab Fuse",
                }
            },
            {
                1218, // Fortress Exterior Fuse 1
                new List<string>() {
                    "Fortress Exterior Fuse 1",
                    "Fortress Exterior Fuse 2",
                    "Beneath the Vault Fuse",
                    "Fortress Candles Fuse",
                    "Fortress Door Left Fuse",
                    "Fortress Courtyard Upper Fuse",
                    "Fortress Courtyard Fuse",
                    "Fortress Door Right Fuse",
                }
            },
            {
                1235, // Fortress Exterior Fuse 2
                new List<string>() {
                    "Fortress Exterior Fuse 1",
                    "Fortress Exterior Fuse 2",
                    "Beneath the Vault Fuse",
                    "Fortress Candles Fuse",
                    "Fortress Door Left Fuse",
                }
            },
            {
                1229, // Fortress Courtyard Upper Fuse
                new List<string>() {
                    "Fortress Exterior Fuse 1",
                    "Fortress Courtyard Upper Fuse",
                    "Fortress Courtyard Fuse",
                    "Fortress Door Right Fuse",
                }
            },
            {
                1236, // Fortress Courtyard Fuse
                new List<string>() {
                    "Fortress Exterior Fuse 1",
                    "Fortress Courtyard Upper Fuse",
                    "Fortress Courtyard Fuse",
                    "Fortress Door Right Fuse",
                }
            },
            {
                1013, // Beneath the Vault Fuse
                new List<string>() {
                    "Fortress Exterior Fuse 1",
                    "Fortress Exterior Fuse 2",
                    "Beneath the Vault Fuse",
                    "Fortress Candles Fuse",
                    "Fortress Door Left Fuse",
                }
            },
            {
                1016, // Fortress Candles Fuse
                new List<string>() {
                    "Fortress Exterior Fuse 1",
                    "Fortress Exterior Fuse 2",
                    "Beneath the Vault Fuse",
                    "Fortress Candles Fuse",
                    "Fortress Door Left Fuse",
                }
            },
            {
                1015, // Fortress Door Left Fuse
                new List<string>() {
                    "Fortress Exterior Fuse 1",
                    "Fortress Exterior Fuse 2",
                    "Beneath the Vault Fuse",
                    "Fortress Candles Fuse",
                    "Fortress Door Left Fuse",
                }
            },
            {
                1014, // Fortress Door Right Fuse
                new List<string>() {
                    "Fortress Exterior Fuse 1",
                    "Fortress Courtyard Upper Fuse",
                    "Fortress Courtyard Fuse",
                    "Fortress Door Right Fuse",
                }
            },
            {
                1101, // Quarry Fuse 1
                new List<string>() {
                    "Quarry Fuse 1",
                    "Quarry Fuse 2",
                }
            },
            {
                1262, // Quarry Fuse 2
                new List<string>() {
                    "Quarry Fuse 1",
                    "Quarry Fuse 2",
                }
            },
            {
                1117, // Ziggurat Miniboss Fuse
                new List<string>() {
                    "Ziggurat Miniboss Fuse",
                }
            },
            {
                1121, // Ziggurat Teleporter Fuse
                new List<string>() {
                    "Ziggurat Teleporter Fuse",
                }
            },
        };

    }

}
