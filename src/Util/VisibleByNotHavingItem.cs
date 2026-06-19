using System.Collections.Generic;
using UnityEngine;

namespace TunicRandomizer {
    public class VisibleByNotHavingItem : MonoBehaviour {

        public Item Item { get; set; }
        public List<Renderer> Renderers { get; set; }
        public List<Collider> Colliders { get; set; }

        public void Awake() {
            Renderers = new List<Renderer>();
            Renderers.AddRange(base.GetComponents<Renderer>());
            Renderers.AddRange(base.GetComponentsInChildren<Renderer>());
            Colliders = new List<Collider>();
            Colliders.AddRange(base.GetComponents<Collider>());
            Colliders.AddRange(base.GetComponentsInChildren<Collider>());
        }

        public void Update() {
            foreach(Renderer renderer in Renderers) {
                renderer.enabled = Item != null && Item.Quantity == 0;
            }
            foreach (Collider collider in Colliders) {
                collider.enabled = Item != null && Item.Quantity == 0;
            }
        }
    }
}
