using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TunicRandomizer {
    public class VisibleByNotHavingItem : MonoBehaviour {

        public Item Item { get; set; }
        public List<Renderer> Renderers { get; set; }

        public void Awake() {
            Renderers = new List<Renderer>();
            Renderers.AddRange(base.GetComponents<Renderer>());
            Renderers.AddRange(base.GetComponentsInChildren<Renderer>());
        }

        public void Update() {
            foreach(Renderer renderer in Renderers) {
                renderer.enabled = Item != null && Item.Quantity == 0;
            }
        }
    }
}
