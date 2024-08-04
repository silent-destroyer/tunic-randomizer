using System.Linq;
using UnityEngine;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {
    public class FoxgodCutscenePatch : MonoBehaviour {
        public void Awake() {
            Mesh mesh = null; 
            Material[] materials = null;
            Material[] foxGodMaterials = GameObject.Find("Foxgod").transform.GetChild(0).GetComponent<CreatureMaterialManager>().originalMaterials;
            Vector3 bookScale = GameObject.Find("manual for cutscene").transform.localScale;
            if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                mesh = ModelSwaps.Items["Hexagon Gold"].GetComponent<MeshFilter>().mesh;
                materials = ModelSwaps.Items["Hexagon Gold"].GetComponent<MeshRenderer>().materials;
                foxGodMaterials = ModelSwaps.Items["Hexagon Gold"].GetComponent<MeshRenderer>().materials;
            }
            if (SaveFile.GetInt(GrassRandoEnabled) == 1) {
                if (GrassRandomizer.GrassChecks.All(check => Locations.CheckedLocations[check.Value.CheckId])) {
                    mesh = ModelSwaps.Items["Grass"].GetComponent<MeshFilter>().mesh;
                    bookScale *= 0.75f;
                    if (materials == null) {
                        materials = ModelSwaps.Items["Grass"].GetComponent<MeshRenderer>().materials;
                    }
                }
            }
            if (mesh != null && materials != null) {
                DoEdits(mesh, materials, bookScale, foxGodMaterials);
            }
        }

        private void DoEdits(Mesh bookReplacement, Material[] bookMaterials, Vector3 bookScale, Material[] foxgodMaterials) {
            GameObject manual = GameObject.Find("manual for cutscene");
            GameObject foxgod = GameObject.Find("Foxgod");
            manual.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().sharedMesh = bookReplacement;
            manual.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials = bookMaterials;
            manual.transform.GetChild(1).gameObject.AddComponent<Rotate>().eulerAnglesPerSecond = new Vector3(0, 25, 0);
            manual.transform.localScale = bookScale;

            foxgod.transform.GetChild(0).GetComponent<CreatureMaterialManager>().originalMaterials = foxgodMaterials;
            foxgod.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials = foxgodMaterials;

            GameObject light = new GameObject("light");
            light.AddComponent<Light>();
            light.transform.position = new Vector3(0, 6.3f, 0);
        }
    }
}
