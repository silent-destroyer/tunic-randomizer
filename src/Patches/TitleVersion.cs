using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TunicRandomizer {
    public class TitleVersion {

        public static bool Loaded = false;
        public static GameObject VersionString;
        public static bool DevBuild = true;
        public static string BuildDescription = "";
        public static bool UpdateAvailable = false;
        public static string UpdateVersion = "";
        public const string UpdateUrl = "https://github.com/silent-destroyer/tunic-randomizer/releases/latest";
        public const string ReportIssueUrl = "https://github.com/silent-destroyer/tunic-randomizer/issues";
        public static GameObject Logo;

        public static void Initialize() {
            UpdateVersion = PluginInfo.VERSION;
            try {
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://api.github.com/repos/silent-destroyer/tunic-randomizer/releases");
                Request.UserAgent = "request";
                HttpWebResponse response = (HttpWebResponse)Request.GetResponse();
                StreamReader Reader = new StreamReader(response.GetResponseStream());
                string JsonResponse = Reader.ReadToEnd();
                dynamic Releases = JsonConvert.DeserializeObject<dynamic>(JsonResponse);
                UpdateVersion = Releases[0]["tag_name"].ToString();
                UpdateAvailable = isNewerVersion(UpdateVersion);
            } catch (Exception e) {
                TunicLogger.LogInfo(e.Message);
            }
            TMP_FontAsset FontAsset = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().Where(Font => Font.name == "Latin Rounded").ToList()[0];
            Material FontMaterial = Resources.FindObjectsOfTypeAll<Material>().Where(Material => Material.name == "Latin Rounded - Quantity Outline").ToList()[0];
            VersionString = new GameObject("randomizer version");
            VersionString.AddComponent<TextMeshProUGUI>().text = $"Randomizer Mod Ver. {PluginInfo.VERSION}";
            if (DevBuild) {
                VersionString.GetComponent<TextMeshProUGUI>().text += "-dev";
            }
            if (BuildDescription != "") {
                VersionString.GetComponent<TextMeshProUGUI>().text += $" ({BuildDescription})";
            }
            //if (UpdateAvailable) {
            //    VersionString.GetComponent<TextMeshProUGUI>().text += $" (Update Available: v{UpdateVersion}!)";
            //}
            VersionString.GetComponent<TextMeshProUGUI>().color = new Color(1.0f, 0.64f, 0.0f);
            VersionString.GetComponent<TextMeshProUGUI>().fontMaterial = FontMaterial;
            VersionString.GetComponent<TextMeshProUGUI>().font = FontAsset;
            VersionString.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.TopLeft;
            VersionString.GetComponent<TextMeshProUGUI>().fontSize = 24;
            VersionString.layer = 5;
            VersionString.transform.parent = GameObject.Find("_GameGUI(Clone)/Title Canvas/Title Screen Root/").transform;
            VersionString.GetComponent<RectTransform>().sizeDelta = new Vector2(1000f, 50f);
            if ((float)Screen.width / Screen.height < 1.7f) {
                VersionString.transform.localPosition = new Vector3(29f, 240f, 0f);
            } else {
                VersionString.transform.localPosition = new Vector3(-25f, 240f, 0f);
            }
            VersionString.transform.localScale = Vector3.one;
            GameObject.DontDestroyOnLoad(VersionString);
            System.Random Random = new System.Random();
            Logo = GameObject.Find("_GameGUI(Clone)/Title Canvas/Title Screen Root/Image");
            string[] args = Il2CppSystem.Environment.GetCommandLineArgs();
            if (Random.Next(100) < 10 || args.Contains("-tunc")) {
                Logo.GetComponent<Image>().sprite = ModelSwaps.TuncTitleImage.GetComponent<Image>().sprite;
            }

            if (SecretMayor.shouldBeActive) {
                Logo.GetComponent<Image>().sprite = ModelSwaps.FindSprite("Randomizer secret_mayor");
            }
        }

        private static bool isNewerVersion(string newVersion) {
            Version currentVersion = new Version(PluginInfo.VERSION);
            Version latestVersion = new Version(newVersion);

            return latestVersion.CompareTo(currentVersion) > 0 || (currentVersion.Equals(latestVersion) && DevBuild);
        }
    }
}
