using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;
using UnityEngine.SceneManagement;
using System.Linq;

namespace TunicRandomizer {
    public struct FuseInformation {
        public string Name;
        public string Guid;
        public string Position;
        public string SceneName;
        public bool StartTall;
        public string Fuse;
    }

    public class FuseRandomizer {

        public const string fusePrefabRoot = "_Checkpoints, Fuses, Platform/TUNIC_Fuse_Big";

        public static List<FuseInformation> Fuses = new List<FuseInformation>();
        public static string fuseFilePath = $"{Application.persistentDataPath}/Randomizer/Fuses.json";
        public static GameObject FusePrefab;
        public static Dictionary<int, int> FakeFuseIds = new Dictionary<int, int>();
        
        public static Dictionary<string, Check> FuseChecks = new Dictionary<string, Check>();

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
            using (Stream stream = assembly.GetManifestResourceStream(fuseJson))
            using (StreamReader reader = new StreamReader(stream)) {
                List<Check> checks = JsonConvert.DeserializeObject<List<Check>>(reader.ReadToEnd());
                foreach (Check check in checks) { 
                    FuseChecks.Add(check.CheckId, check);
                    FakeFuseIds.Add(int.Parse(check.Location.LocationId), 9000 + FakeFuseIds.Count);
                }
            }
        }

        public static void CreateFuseItems() {
            foreach (ItemData FuseItem in ItemLookup.Items.Values.Where(item => item.Type == ItemTypes.FUSE)) {
                CreateFuseItem(FuseItem.Name);
            }
        }

        private static void CreateFuseItem(string name) {

            SpecialItem Fuse = ScriptableObject.CreateInstance<SpecialItem>();

            Fuse.name = name;
            Fuse.collectionMessage = TunicUtils.CreateLanguageLine($"\"{Fuse.name.Replace("Fuse", "")}\"");
            Fuse.controlAction = "";

            Inventory.itemList.Add(Fuse);
        }

        public static void InstantiateFusePrefab() {
            if (FusePrefab == null) {
                GameObject fuse = GameObject.Find(fusePrefabRoot);
                if (fuse != null) {
                    FusePrefab = GameObject.Instantiate(fuse);
                    //FusePrefab.GetComponent<ConduitNode>().Guid = -1;
                    FusePrefab.SetActive(false);
                    FusePrefab.transform.position = new Vector3(-30000, -30000, -30000);
                    GameObject.DontDestroyOnLoad(FusePrefab);
                }
            }
        }

        public static void FuseCloseAnimationHelper___animationEvent_fuseCloseAnimationDone_PostfixPatch(FuseCloseAnimationHelper __instance) {
            Fuse fuse = __instance.GetComponentInParent<Fuse>();
            ConduitNode node = __instance.GetComponentInParent<ConduitNode>();
            if (fuse != null && node != null) {
                TunicLogger.LogInfo("fuse animator closed " + node.Guid);
            }
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
            return true;
        }

        public static bool ConduitData_CheckConnectedToPower_PrefixPatch(ConduitData __instance, ref int guid, ref bool __result) {
            if (SceneManager.GetActiveScene().name == "Quarry") {
                __result = true;
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

    }

    public class ToggleFuseByFuseItem : MonoBehaviour {

    }
}
