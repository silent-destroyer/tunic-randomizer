using UnityEngine;
using static TunicRandomizer.Hints;

namespace TunicRandomizer {
    public class HeroGraveToggle : MonoBehaviour {

        public HeroGraveHint heroGravehint;
        public StateVariable round2StateVar;
        public GameObject Candle;
        public GameObject BlueFlame;

        public void Awake() {
            Candle = base.transform.GetChild(7).gameObject;
            if (BlueFlame == null) {
                BlueFlame = GameObject.Instantiate(ModelSwaps.BlueFire, Candle.transform.localPosition, Quaternion.identity, base.transform);
                BlueFlame.transform.localEulerAngles = Vector3.zero;
                BlueFlame.transform.localPosition = Candle.transform.localPosition;
                BlueFlame.SetActive(false);
            }
            Candle.SetActive(true);
            round2StateVar = StateVariable.GetStateVariableByName("randomizer got all 6 grave items");
        }

        public void Update() {
            if (TunicRandomizer.Settings.HeroPathHintsEnabled) {
                base.transform.GetChild(4).gameObject.SetActive((heroGravehint.PointLight || SaveFile.GetInt($"randomizer hint found {heroGravehint.PathHintId}") == 1));
                Candle.gameObject.SetActive(SaveFile.GetInt($"randomizer hint found {heroGravehint.PathHintId}") == 1);
                BlueFlame.SetActive(round2StateVar.BoolValue);
            } else {
                base.transform.GetChild(4).gameObject.SetActive(true);
                Candle.SetActive(true);
                BlueFlame.SetActive(false);
            }
        }
        

    }
}
