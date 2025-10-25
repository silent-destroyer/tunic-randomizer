using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TunicRandomizer {
    public class SceneSelectionButton : MonoBehaviour {
        public Image image;
        public Button button;
        public Color colorWhenSelected = new Color(0.92f, 0.65f, 0.08f);
        public bool isSceneButton = false;
        public GameObject dart;
        public string EntranceName;

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
                if (dart != null) {
                    dart.SetActive(EntranceSelector.WaitingForDartSelection || (FoxPrince.PinnedPortal != "" && FoxPrince.PinnedPortal == EntranceName));
                }
            } else {
                image.color = Color.white;
                if (dart != null) {
                    dart.SetActive(FoxPrince.PinnedPortal != "" && FoxPrince.PinnedPortal == EntranceName);
                }
            }
            GetComponent<Button>().enabled = isSceneButton || !EntranceSelector.WaitingForDartSelection;
        }
    }
}
