using UnityEngine;

namespace TunicRandomizer {
    public class PlayerPositionDisplay : MonoBehaviour {
        private GUIStyle m_Style;

        public void Awake() {
            m_Style = new GUIStyle();
            m_Style.alignment = TextAnchor.MiddleRight;
            m_Style.normal.textColor = Color.white;
            m_Style.fontSize = 36;
        }

        private void OnGUI() {
            if (PlayerCharacter.Instanced && TunicRandomizer.Settings.ShowPlayerPosition) {
                GUI.Label(new Rect(Screen.width-300, Screen.height-30, 300, 30), PlayerCharacter.Transform.position.ToString(), m_Style);
            }
        }
    }
}
