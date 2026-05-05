using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

namespace TunicRandomizer {
    public class CRTMode : MonoBehaviour {
        
        public List<Camera> cameras;
        public GameObject crtHolder;
        public static CRTMode instance;
        public MeshRenderer meshRenderer;
        public PageDisplay pageDisplay;

        public static List<string> CameraNames = new List<string>() {
            "_SpeedrunTimer(Clone)",
            "_Screenspace Effects(Clone)",
            "_OptionsGUI(Clone)",
            "_GameGUI(Clone)",
            //"_FinishlineDisplay(Clone)",
            "_FileManagementGUI(Clone)",
            "Page Display Camera",
            "Camera 4 - Blit",
            "Credits Camera - Environment",
            "Camera - GUI",
        };

        public void Awake() {
            cameras = new List<Camera>();
            pageDisplay = PageDisplay.instance;
            crtHolder = Instantiate(PageDisplay.instance.gameObject);
            GameObject.Destroy(crtHolder.GetComponent<PageDisplay>());
            GameObject.Destroy(crtHolder.transform.GetChild(7).gameObject);
            GameObject.Destroy(crtHolder.transform.GetChild(6).gameObject);
            GameObject.Destroy(crtHolder.transform.GetChild(3).gameObject);
            GameObject.Destroy(crtHolder.transform.GetChild(2).gameObject);
            GameObject.Destroy(crtHolder.transform.GetChild(0).gameObject);
            crtHolder.transform.parent = transform;
            crtHolder.transform.Find("CRT plane").localScale = new Vector3(3.5f, 2.5f, 2.5f);
            crtHolder.transform.Find("CRT_body").localPosition = new Vector3(0f, -1.753f, 1.58f);
            crtHolder.SetActive(true);
            crtHolder.transform.Find("Offset").GetComponentInChildren<Kino.Bloom>().enabled = false;
            crtHolder.transform.Find("Offset").GetComponentInChildren<DepthOfField>().aperture = 0.75f;
            foreach (Camera cam in crtHolder.transform.GetComponentsInChildren<Camera>()) {
                cam.gameObject.tag = "";
            }
            meshRenderer = crtHolder.transform.GetChild(1).GetComponent<MeshRenderer>();
        }

        public void Start() {
            GUIMode.guiModes.RemoveAt(GUIMode.guiModes.Count-1);
            PageDisplay.instance = pageDisplay;
            PageDisplay.instance.transform.localPosition -= new Vector3(0, 5, 0);
            PageDisplay.instance.transform.GetChild(6).gameObject.SetActive(false);
            Toggle();
        }

        public void Update() {
            if (SceneManager.GetActiveScene().name == "Loading") {
                meshRenderer.material.SetTexture("_render", Texture2D.blackTexture);
            }
        }

        public void Enable(bool withCrtFrame = false) {
            gameObject.SetActive(true);
            cameras.Clear();
            Camera main = Resources.FindObjectsOfTypeAll<Camera>().Where(c => c.name == "Camera 1 - Main" ||
            (c.gameObject.scene.name == "Credits Scroll" && c.name == "Credits Camera - Environment")).First();
            meshRenderer.material.SetTexture("_render", main.targetTexture);
            foreach (Camera camera in Resources.FindObjectsOfTypeAll<Camera>()) {
                if (CameraNames.Contains(camera.name) && camera.GetComponentInParent<CRTMode>() == null) {
                    if (gameObject.active) {
                        camera.targetTexture = main.targetTexture;
                    }
                    cameras.Add(camera);
                }
            }
            if ((float)Screen.width / Screen.height < 1.7f) {
                TitleVersion.VersionString.transform.localPosition = new Vector3(44f, 240f, 0f);
            } else {
                TitleVersion.VersionString.transform.localPosition = new Vector3(-10f, 240f, 0f);
            }

            if (withCrtFrame) {
                crtHolder.transform.Find("CRT_body").gameObject.SetActive(true);
                crtHolder.transform.Find("CRT plane").localScale = Vector3.one * 2.0f;
            } else {
                crtHolder.transform.Find("CRT_body").gameObject.SetActive(false);
                crtHolder.transform.Find("CRT plane").localScale = new Vector3(3.5f, 2.5f, 2.5f);
            }
        }

        public void Disable() {
            gameObject.SetActive(false);
            foreach (Camera c in cameras) {
                if (c != null) {
                    c.targetTexture = null;
                }
            }
            if ((float)Screen.width / Screen.height < 1.7f) {
                TitleVersion.VersionString.transform.localPosition = new Vector3(29f, 240f, 0f);
            } else {
                TitleVersion.VersionString.transform.localPosition = new Vector3(-25f, 240f, 0f);
            }
            crtHolder.transform.Find("CRT_body").gameObject.SetActive(false);
            crtHolder.transform.Find("CRT plane").localScale = new Vector3(3.5f, 2.5f, 2.5f);
        }

        public static void SetupCRTMode() {
            GameObject crtMode = new GameObject("crt mode");
            crtMode.layer = 5;
            GameObject.DontDestroyOnLoad(crtMode);
            CRTMode.instance = crtMode.AddComponent<CRTMode>();
            crtMode.SetActive(true);
        }

        public static void Toggle() {
            if (instance != null) {
                if (TunicRandomizer.Settings.RetroFilterEnabled) {
                    instance.Enable();
                } else {
                    instance.Disable();
                }
            }
        }

        public static void CreditsCardController_Awake_PostfixPatch(CreditsCardController __instance) {
            if (instance != null && TunicRandomizer.Settings.RetroFilterEnabled) {
                instance.Enable();
            }
        }
        public static void CreditsScrollController_Start_PrefixPatch(CreditsScrollController __instance) {
            if (instance != null && TunicRandomizer.Settings.RetroFilterEnabled) {
                instance.Enable();
            }
        }
    }
}
