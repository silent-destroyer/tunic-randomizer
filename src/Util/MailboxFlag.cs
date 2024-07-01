using UnityEngine;

namespace TunicRandomizer {
    public class MailboxFlag : MonoBehaviour {

        public void Update() { 
            base.transform.localEulerAngles = new Vector3(90, 0, 0);
            if (SaveFile.GetInt($"randomizer picked up {SaveFile.GetString("randomizer mailbox hint location")}") == 1 || SaveFile.GetString("randomizer mailbox hint location") == "no first steps") {
                base.transform.localEulerAngles = new Vector3(0, 0, 0);
                Destroy(this);
            }
        }
    }
}
