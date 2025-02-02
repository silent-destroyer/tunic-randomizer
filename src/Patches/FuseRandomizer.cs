using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

namespace TunicRandomizer {

    public class FuseCheckHelper : MonoBehaviour {
        public int OriginalGuid;
        public int FakeGuid;
        public GameObject Sign;
        public string CheckId {
            get => $"{OriginalGuid} [{gameObject.scene.name}]";
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
            Fuse.collectionMessage = TunicUtils.CreateLanguageLine($"\"{Fuse.name.Replace("Fuse", "")}\"");
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
                    FusePrefab.SetActive(false);
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
                        // todo
                    } else if (SaveFlags.IsSinglePlayer() && Locations.RandomizedLocations.ContainsKey(CheckId)) {
                        Check check = Locations.RandomizedLocations[CheckId];
                        ItemPatches.GiveItem(check);
                    }

                    if (fuseHelper.Sign != null) {
                        fuseHelper.Sign.SetActive(true);
                    }

                    SaveFile.SetInt("randomizer fuse closed " + node.Guid, 1);
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
            foreach (Fuse fuse in Resources.FindObjectsOfTypeAll<Fuse>().Where(fuse => fuse.gameObject.scene.name == SceneManager.GetActiveScene().name && fuse.GetComponent<ConduitNode>() != null)) {
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
                    TunicLogger.LogInfo("finished setting up fuse " +  newNode.Guid);
                }
            }

            if (SceneManager.GetActiveScene().name == "Overworld Redux" && GameObject.Find("ladder and fuse checklist") == null) {
                LadderToggles.SpawnOverworldChecklistSign();
            }

            ModifiedFusesAlready = true;
        }

        public static void ApplyFuseTexture(Fuse fuse, Material material = null) {
            foreach (MeshRenderer r in fuse.transform.GetChild(0).GetChild(0).GetComponentsInChildren<MeshRenderer>()) {
                if (r.name.Contains("lights")) {
                    continue;
                }
                r.material = material;
            }
        }

        public static bool ConduitNode_CheckConnectedToPower_PrefixPatch(ConduitNode __instance, ref bool __result) {
            if (TunicRandomizer.Settings.EnableAllCheckpoints && __instance != null && __instance.GetComponent<Campfire>() != null && __instance.GetComponent<UpgradeAltar>() != null) {
                __result = true;
                return false;
            }
            if (Fuses.ContainsKey(__instance.Guid) && SaveFlags.GetBool(SaveFlags.FuseShuffleEnabled)) {
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
            if (Fuses.ContainsKey(guid) && SaveFlags.GetBool(SaveFlags.FuseShuffleEnabled)) {
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

        public static string GetFuseStatusForSign(List<string> Fuses) {
            string header = $"             <#FF0000>[death] kawndooit rehstorA$uhn <#FF0000>[death]\n";
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
                1305, // Furnace to Well Fuse
                new List<string>() {
                    "Furnace to Well Fuse",
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
