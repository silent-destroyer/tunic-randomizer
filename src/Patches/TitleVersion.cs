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

            this.GuiMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / 1920f, Screen.height / 1080f, 1f));
        }

        private void OnGUI() {
            Matrix4x4 matrixx = GUI.matrix;
            GUI.matrix = this.GuiMatrix;
            if (SceneLoaderPatches.SceneName == "TitleScreen") {
                GUI.color = new Color(1f, 147f / 255f, 0f, 1f);
                GUI.skin.label.fontSize = 40;
                GUI.Label(new Rect(10f, 10f, 700f, 100f), "Randomizer Mod Ver. 0.0.4");
            } 
        }
    }
}
