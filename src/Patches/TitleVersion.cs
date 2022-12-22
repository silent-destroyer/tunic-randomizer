using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace TunicRandomizer {
    public class TitleVersion : MonoBehaviour {

        private Matrix4x4 GuiMatrix;
        public Rect textureCrop = new Rect(0.1f, 0.1f, 0.5f, 0.25f);
        public Vector2 position = new Vector2(10, 10);
        private void Awake() {

            this.GuiMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, 1f, 1f));
        }

        private void OnGUI() {
            Matrix4x4 matrixx = GUI.matrix;
            GUI.matrix = this.GuiMatrix;
            if (SceneLoaderPatches.SceneName == "TitleScreen") {
                GUI.color = new Color(1f, 147f / 255f, 0f, 1f);
                GUI.skin.label.fontSize = 38;
                if (Profile.GetAccessibilityPref(Profile.AccessibilityPrefs.SpeedrunMode)) {
                    GUI.Label(new Rect(20f, 75f, 700f, 100f), "Randomizer Mod Ver. " + PluginInfo.VERSION);
                } else {
                    GUI.Label(new Rect(20f, 30f, 700f, 100f), "Randomizer Mod Ver. " + PluginInfo.VERSION);
                }
            }
            
            if(SceneLoaderPatches.SceneName == "Shop" && TunicRandomizer.Settings.ShowShopItemsEnabled) {
                GUI.skin.label.fontSize = 19;
                foreach (ItemData ItemData in RandomItemPatches.ItemList.Values) {
                    if (ItemData.Location.SceneName == "Shop" && !RandomItemPatches.ItemsPickedUp[$"{ItemData.Location.LocationId} [Shop]"]) {
                        var position = Camera.main.WorldToScreenPoint(GameObject.Find(ItemData.Location.LocationId).gameObject.transform.position);
                        if (GameObject.Find(ItemData.Location.LocationId).transform.localScale.x >= 1) {
                            GUI.Label(new Rect(position.x - 100, Screen.currentResolution.height - position.y - 100, 500, 50), $"{Hints.SimplifiedItemNames[ItemData.Reward.Name]} {(ItemData.Reward.Amount > 1 ? $"x{ItemData.Reward.Amount}" : "")}");
                        }
                    }
                }
            }
        }
    }
}
