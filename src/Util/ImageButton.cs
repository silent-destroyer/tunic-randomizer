using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TunicRandomizer {
    public class ImageButton : MonoBehaviour {
        public Image image;
        public Button button;
        public Color colorWhenSelected = new Color(0.92f, 0.65f, 0.08f);

        public void Update() {
            if (image == null && GetComponent<Image>() != null) {
                image = GetComponent<Image>();
            }
            if (button == null && GetComponent<Button>() != null) {
                button = GetComponent<Button>();
            }
            if (button != null && image != null && button.hasSelection && colorWhenSelected != null) {
                image.color = colorWhenSelected;
            } else {
                image.color = Color.white;
            }
        }
    }
}
