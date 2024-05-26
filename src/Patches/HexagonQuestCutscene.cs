using UnityEngine;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {
    public class HexagonQuestCutscene: MonoBehaviour {
        public void Awake() {
            if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                GameObject manual = GameObject.Find("manual for cutscene");
                GameObject foxgod = GameObject.Find("Foxgod");
                manual.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().sharedMesh = ModelSwaps.Items["Hexagon Gold"].GetComponent<MeshFilter>().mesh;
                manual.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials = ModelSwaps.Items["GoldenTrophy_1"].GetComponent<MeshRenderer>().materials;
                manual.transform.GetChild(1).gameObject.AddComponent<Rotate>().eulerAnglesPerSecond = new Vector3(0, 25, 0);

                foxgod.transform.GetChild(0).GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["GoldenTrophy_1"].GetComponent<MeshRenderer>().materials;
                foxgod.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["GoldenTrophy_1"].GetComponent<MeshRenderer>().materials;
                
                GameObject light = new GameObject("light");
                light.AddComponent<Light>();
                light.transform.position = new Vector3(0, 6.3f, 0);
            }
        }
    }
}
