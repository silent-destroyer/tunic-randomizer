using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TunicRandomizer {
    public class ArachnophobiaMode : MonoBehaviour {

        public static Dictionary<string, Sprite> runes = new Dictionary<string, Sprite>();
        public static bool DidArachnophobiaModeAlready = false;

        public static void GetRuneSprites() {
            if (runes.Count == 0) {
                runes.Add("s", ModelSwaps.FindSprite("Alphabet New_33"));
                runes.Add("p", ModelSwaps.FindSprite("Alphabet New_21"));
                runes.Add("i", ModelSwaps.FindSprite("Alphabet New_13"));
                runes.Add("d", ModelSwaps.FindSprite("Alphabet New_24"));
                runes.Add("ur", ModelSwaps.FindSprite("Alphabet New_8"));

                runes.Add("eh", ModelSwaps.FindSprite("Alphabet New_3"));
                runes.Add("n", ModelSwaps.FindSprite("Alphabet New_19"));
                runes.Add("t", ModelSwaps.FindSprite("Alphabet New_23"));
                runes.Add("uh", ModelSwaps.FindSprite("Alphabet New_2"));
                runes.Add("E", ModelSwaps.FindSprite("Alphabet New_6"));
            }
        }

        public static void ToggleArachnophobiaMode() {
            Material material = ModelSwaps.FindMaterial("UI Add");
            foreach (Spider spider in Resources.FindObjectsOfTypeAll<Spider>().Where(spider => spider.gameObject.scene.name == SceneManager.GetActiveScene().name)) {
                ApplyArachnophobiaModel(spider);
            }
            foreach (Centipede centipede in Resources.FindObjectsOfTypeAll<Centipede>().Where(centipede => centipede.gameObject.scene.name == SceneManager.GetActiveScene().name)) { 
                ApplyArachnophobiaModel(centipede);
            }

            DidArachnophobiaModeAlready = true;
        }

        private static void ApplyArachnophobiaModel(Spider spider) {
            Material material = ModelSwaps.FindMaterial("UI Add");
            if (spider.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>() != null) {
                spider.transform.GetChild(0).gameObject.SetActive(false);
            }
            Color color = spider.name.ToLower().Contains("small") ? new Color(1, 0.5f, 0.2f, 1) : new Color(0, 0.5f, 1, 1);
            float localXAngle = spider.name.ToLower().Contains("small") ? 0.8f : 0.6f;
            GameObject runes = new GameObject("runes");
            runes.transform.parent = spider.transform;
            runes.transform.localPosition = spider.transform.localPosition;
            GameObject s = CreateRune("s", runes.transform, new Vector3(-0.7f, 1, -0.1f), new Vector3(1.5f, 1.5f, 1.5f), color);
            GameObject p = CreateRune("p", runes.transform, new Vector3(0.8f, 1f, -0.1f), new Vector3(1.5f, 1.5f, 1.5f), color, material);
            GameObject i = CreateRune("i", runes.transform, new Vector3(0.8f, 1f, -0.1f), new Vector3(1.5f, 1.5f, 1.5f), color);
            GameObject d = CreateRune("d", runes.transform, new Vector3(2.3f, 1f, -0.1f), new Vector3(1.5f, 1.5f, 1.5f), color, material);
            GameObject ur = CreateRune("ur", runes.transform, new Vector3(2.3f, 1f, -0.1f), new Vector3(1.5f, 1.5f, 1.5f), color);
            runes.transform.localPosition = new Vector3(localXAngle, 0, 0);
            runes.transform.localEulerAngles = new Vector3(0, 180, 0);
        }

        private static void ApplyArachnophobiaModel(Centipede centipede) {
            Material material = ModelSwaps.FindMaterial("UI Add");
            if (centipede.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>() != null) {
                centipede.transform.GetChild(0).gameObject.SetActive(false);
            }

            Color color = new Color(1, 0, 1, 1);

            Transform armature = centipede.transform.GetChild(5);
            Vector3 localScale = new Vector3(1.1278f, 1.1278f, 1.1278f);
            CreateRune("s", armature.GetChild(0).GetChild(1), Vector3.zero, localScale, color).transform.localEulerAngles = new Vector3(0, 90, 90);
            CreateRune("eh", armature.GetChild(0).GetChild(1), Vector3.zero, localScale, color, material).transform.localEulerAngles = new Vector3(0, 90, 90);
            CreateRune("n", armature.GetChild(0).GetChild(1).GetChild(4), Vector3.zero, localScale, color).transform.localEulerAngles = new Vector3(0, 90, 90);
            CreateRune("t", armature.GetChild(0).GetChild(1).GetChild(4).GetChild(4), Vector3.zero, localScale, color).transform.localEulerAngles = new Vector3(0, 90, 90);
            CreateRune("uh", armature.GetChild(0).GetChild(1).GetChild(4).GetChild(4), Vector3.zero, localScale, color, material).transform.localEulerAngles = new Vector3(0, 90, 90);
            CreateRune("p", armature.GetChild(0).GetChild(1).GetChild(4).GetChild(4).GetChild(4), Vector3.zero, localScale, color).transform.localEulerAngles = new Vector3(0, 90, 90);
            CreateRune("E", armature.GetChild(0).GetChild(1).GetChild(4).GetChild(4).GetChild(4), Vector3.zero, localScale, color, material).transform.localEulerAngles = new Vector3(0, 90, 90);
            CreateRune("E", armature.GetChild(0).GetChild(1).GetChild(4).GetChild(4).GetChild(4).GetChild(4), Vector3.zero, localScale, color).transform.localEulerAngles = new Vector3(0, 90, 90);
        }

        private static GameObject CreateRune(string name, Transform parent, Vector3 localPosition, Vector3 localScale, Color color, Material material = null) {
            GameObject rune = new GameObject(name);
            rune.transform.parent = parent;
            rune.transform.localPosition = localPosition;
            rune.transform.localScale = localScale;
            rune.AddComponent<SpriteRenderer>().sprite = runes[name];
            rune.GetComponent<SpriteRenderer>().color = color;
            if (material != null) {
                rune.GetComponent<SpriteRenderer>().material = material;
            }
            return rune;
        }

        public static void Centipede_monster_Start_PostfixPatch(Centipede __instance) {
            if (__instance.name.Contains("from egg") && __instance.gameObject.scene.buildIndex != -1 && TunicRandomizer.Settings.ArachnophobiaMode) {
                ApplyArachnophobiaModel(__instance);
            }
        }

    }
}
