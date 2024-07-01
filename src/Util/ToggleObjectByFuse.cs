using UnityEngine;

namespace TunicRandomizer {
    public class ToggleObjectByFuse : MonoBehaviour{

        public int fuseId = -1;
        public bool stateWhenClosed = true;

        public void Update() {
            bool active = SaveFile.GetInt("fuseClosed " + fuseId) == 1;
            for(int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).gameObject.SetActive(active ? stateWhenClosed : !stateWhenClosed);
            }
        }
    }
}
