using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using TinyJson;

namespace TunicRandomizer {
    
    public class TitleVersion {

        public static bool Loaded = false;
        public static GameObject TitleLogo;
        public static void Initialize() {
            if (!Loaded) {
                bool UpdateAvailable = false;
                string UpdateVersion = PluginInfo.VERSION;
                try {
                    HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://api.github.com/repos/silent-destroyer/tunic-randomizer/releases");
                    Request.UserAgent = "request";
                    HttpWebResponse response = (HttpWebResponse)Request.GetResponse();
                    StreamReader Reader = new StreamReader(response.GetResponseStream());
                    string JsonResponse = Reader.ReadToEnd();
                    dynamic Releases = JSONParser.FromJson<dynamic>(JsonResponse);
                    UpdateAvailable = Releases[0]["tag_name"] != PluginInfo.VERSION;
                    UpdateVersion = Releases[0]["tag_name"];
                } catch (Exception e) { 
                }

                TMP_FontAsset FontAsset = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().Where(Font => Font.name == "Latin Rounded").ToList()[0];
                Material FontMaterial = Resources.FindObjectsOfTypeAll<Material>().Where(Material => Material.name == "Latin Rounded - Quantity Outline").ToList()[0];
                GameObject TitleVersion = new GameObject("randomizer version");
                TitleVersion.AddComponent<TextMeshProUGUI>().text = $"Randomizer Mod Ver. {PluginInfo.VERSION}";
                if (UpdateAvailable) {
                    TitleVersion.GetComponent<TextMeshProUGUI>().text += $" (Update Available: v{UpdateVersion}!)";
                }
                TitleVersion.GetComponent<TextMeshProUGUI>().color = new Color(1.0f, 0.64f, 0.0f);
                TitleVersion.GetComponent<TextMeshProUGUI>().fontMaterial = FontMaterial;
                TitleVersion.GetComponent<TextMeshProUGUI>().font = FontAsset;
                TitleVersion.GetComponent<TextMeshProUGUI>().fontSize = 24;
                TitleVersion.layer = 5;
                TitleVersion.transform.parent = GameObject.Find("_GameGUI(Clone)/Title Canvas/Title Screen Root/").transform;
                TitleVersion.GetComponent<RectTransform>().sizeDelta = new Vector2(700f, 50f);
                if (Screen.width <= 1280 && Screen.height <= 800) {
                    TitleVersion.transform.localPosition = new Vector3(-122f, 240f, 0f);
                } else {
                    TitleVersion.transform.localPosition = new Vector3(-176f, 240f, 0f);
                }
                TitleVersion.transform.localScale = Vector3.one;
                GameObject.DontDestroyOnLoad(TitleVersion);
                System.Random Random = new System.Random();

                if (Random.Next(100) < 10) {
                    GameObject Title = GameObject.Find("_GameGUI(Clone)/Title Canvas/Title Screen Root/Image");
                    GameObject.Destroy(Title.GetComponent<Image>());
                    ModelSwaps.TuncTitleImage.transform.parent = Title.transform;
                    ModelSwaps.TuncTitleImage.transform.localScale = new Vector3(3.5f, 2f, 1f);
                    ModelSwaps.TuncTitleImage.transform.localPosition = Vector3.zero;
                    ModelSwaps.TuncTitleImage.SetActive(true);
                }
            }
            Loaded = true;

        }

    }

}
