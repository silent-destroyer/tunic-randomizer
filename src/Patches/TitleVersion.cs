using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Net;
using UnityEngine.InputSystem.Utilities;
using System.IO;
using Newtonsoft.Json;

namespace TunicRandomizer {
    public class TitleVersion {

        public static bool Loaded = false;
        public static GameObject VersionString;
        public static bool DevBuild = true;
        public static bool UpdateAvailable = false;
        public static string UpdateVersion = "";
        public static GameObject Logo;
        public static GameObject TitleButtons;
        public static void Initialize() {
            UpdateVersion = PluginInfo.VERSION;
            try {
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://api.github.com/repos/silent-destroyer/tunic-randomizer/releases");
                Request.UserAgent = "request";
                HttpWebResponse response = (HttpWebResponse)Request.GetResponse();
                StreamReader Reader = new StreamReader(response.GetResponseStream());
                string JsonResponse = Reader.ReadToEnd();
                dynamic Releases = JsonConvert.DeserializeObject<dynamic>(JsonResponse);
                UpdateAvailable = Releases[0]["tag_name"].ToString() != PluginInfo.VERSION && !DevBuild;
                UpdateVersion = Releases[0]["tag_name"].ToString();
            } catch (Exception e) {
                TunicLogger.LogInfo(e.Message);
            }
            TMP_FontAsset FontAsset = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().Where(Font => Font.name == "Latin Rounded").ToList()[0];
            Material FontMaterial = Resources.FindObjectsOfTypeAll<Material>().Where(Material => Material.name == "Latin Rounded - Quantity Outline").ToList()[0];
            VersionString = new GameObject("randomizer version");
            VersionString.AddComponent<TextMeshProUGUI>().text = $"Randomizer Mod Ver. {PluginInfo.VERSION}{(DevBuild ? "-dev" : "")}";
            if (UpdateAvailable) {
                VersionString.GetComponent<TextMeshProUGUI>().text += $" (Update Available: v{UpdateVersion}!)";
            }
            VersionString.GetComponent<TextMeshProUGUI>().color = new Color(1.0f, 0.64f, 0.0f);
            VersionString.GetComponent<TextMeshProUGUI>().fontMaterial = FontMaterial;
            VersionString.GetComponent<TextMeshProUGUI>().font = FontAsset;
            VersionString.GetComponent<TextMeshProUGUI>().fontSize = 24;
            VersionString.layer = 5;
            VersionString.transform.parent = GameObject.Find("_GameGUI(Clone)/Title Canvas/Title Screen Root/").transform;
            VersionString.GetComponent<RectTransform>().sizeDelta = new Vector2(700f, 50f);
            if ((float)Screen.width/Screen.height < 1.7f) {
                VersionString.transform.localPosition = new Vector3(-122f, 240f, 0f);
            } else {
                VersionString.transform.localPosition = new Vector3(-176f, 240f, 0f);
            }
            VersionString.transform.localScale = Vector3.one;
            GameObject.DontDestroyOnLoad(VersionString);
            System.Random Random = new System.Random();
            Logo = GameObject.Find("_GameGUI(Clone)/Title Canvas/Title Screen Root/Image");
            if (Random.Next(100) < 10) {
                Logo.GetComponent<Image>().sprite = ModelSwaps.TuncTitleImage.GetComponent<Image>().sprite;
            }

            if (SecretMayor.shouldBeActive) {
                Logo.GetComponent<Image>().sprite = ModelSwaps.FindSprite("Randomizer secret_mayor");
            }
            TitleButtons = GameObject.Find("_GameGUI(Clone)/Title Canvas/Title Screen Root/Button Group/");
        }

    }
}
