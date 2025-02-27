using System.Linq;
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
    public class ToggleObjectByFuseItem : MonoBehaviour {
        public FuseInformation Fuse;

        public void Update() {
            if (Fuse.FuseItem == null || Fuse.FakeGuid == 0) {
                return;
            }

            bool hasFuseItems = Fuse.FusePath.All(fuse => Inventory.GetItemByName(fuse).Quantity > 0);
            if (hasFuseItems) {
                Destroy(gameObject);
            }
        }
    }
}
