using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TunicRandomizer {
    public class SceneSelectionButton : MonoBehaviour {
        public Image image;
        public Button button;
        public Color colorWhenSelected = new Color(0.92f, 0.65f, 0.08f);
        public bool isSceneButton = false;
        public Image dartIcon;

        public void Update() {
            if (image == null && GetComponent<Image>() != null) {
                image = GetComponent<Image>();
            }
            if (button == null && GetComponent<Button>() != null) {
                button = GetComponent<Button>();
            }
            if (button != null && image != null && EventSystem.current.currentSelectedGameObject == this.gameObject && colorWhenSelected != null) {
                image.color = colorWhenSelected;
                if (isSceneButton && EntranceSelector.WaitingForDartSelection) {
                    image.color = Color.cyan;
                }
                if (dartIcon != null) {
                    dartIcon.gameObject.SetActive(EntranceSelector.WaitingForDartSelection); // Todo add logic to show the icon if the entrance is the pinned entrance
                }
            } else {
                image.color = Color.white;
                if (dartIcon != null) {
                    dartIcon.gameObject.SetActive(false); // Todo add logic to show the icon if the entrance is the pinned entrance
                }
            }
            GetComponent<Button>().enabled = isSceneButton || !EntranceSelector.WaitingForDartSelection;
        }
    }
}
