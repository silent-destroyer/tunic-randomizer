using FMODUnity;
using InControl;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TunicRandomizer {
    public class CreditsSkipper : MonoBehaviour {

        public float holdTime;
        public static float CompletionTimer = 0.0f;

        public void Awake() {
            holdTime = 0f;
        }

        public void Update() {
            if (Input.GetKey(KeyCode.Space) || InputManager.ActiveDevice.Command.IsPressed || InputManager.ActiveDevice.RightCommand.IsPressed) { 
                if (holdTime >= 3f && SpeedrunData.gameComplete != 0 && SceneManager.GetActiveScene().name != "GameOverDecision") {
                    TunicLogger.LogInfo("Skipping credits!");
                    foreach(StudioEventEmitter sfx in GameObject.FindObjectsOfType<StudioEventEmitter>()) {
                        sfx.Stop();
                    }
                    SceneLoader.LoadScene("GameOverDecision");
                }
                holdTime += Time.unscaledDeltaTime;
            } else {
                holdTime = 0f;
            }
            if (SpeedrunData.gameComplete != 0 && !SpeedrunFinishlineDisplayPatches.GameCompleted) {
                SpeedrunFinishlineDisplayPatches.GameCompleted = true;
                SpeedrunFinishlineDisplayPatches.ShowCompletionStatsAfterDelay = true;
                if (InventoryDisplayPatches.HexagonQuest != null) {
                    InventoryDisplayPatches.HexagonQuest.SetActive(false);
                }
                if (InventoryDisplayPatches.GrassCounter != null) {
                    InventoryDisplayPatches.GrassCounter.SetActive(false);
                }
            }
            if (SpeedrunFinishlineDisplayPatches.ShowCompletionStatsAfterDelay) {
                CompletionTimer += Time.fixedUnscaledDeltaTime;
                if (CompletionTimer > 6.0f) {
                    CompletionTimer = 0.0f;
                    SpeedrunFinishlineDisplayPatches.UpdateCounters();
                    SpeedrunFinishlineDisplayPatches.StatSections["Timer"].SetActive(!Profile.GetAccessibilityPref(Profile.AccessibilityPrefs.SpeedrunMode));
                    SpeedrunFinishlineDisplayPatches.CompletionCanvas.SetActive(true);
                    SpeedrunFinishlineDisplayPatches.ShowCompletionStatsAfterDelay = false;
                }
            }
        }
    }
}
