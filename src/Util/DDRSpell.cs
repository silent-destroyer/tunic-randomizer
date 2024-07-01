using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnhollowerBaseLib;
using UnityEngine;

namespace TunicRandomizer {
    public class DDRSpell : MagicSpell {

        public static GameObject DPADPool;

        public ToggleObjectBySpell[] spellToggles;
        public List<GameObject> Arrows;

        List<string> closestSpellStrings = new List<string>();

        private Dictionary<DPAD, string> dpadToChar = new Dictionary<DPAD, string>();

        public static void CopyDPADTester() {
            DPADPool = GameObject.Instantiate(GameObject.FindObjectOfType<DPADTester>().transform.GetChild(0).gameObject);
            GameObject.DontDestroyOnLoad(DPADPool);
            DPADPool.GetComponent<PooledFX>().dontDestroyOnLoad = true;
            GameObject[] pooledArrows = new GameObject[] { };
            GameObject ArrowRoot = new GameObject("arrow root");
            foreach(GameObject arrow in Resources.FindObjectsOfTypeAll<MoveUp>().Where(x => x.name == "game gui_arrow(Clone)").Select(x => x.gameObject)) {
                GameObject.DontDestroyOnLoad(arrow);
                arrow.transform.parent = ArrowRoot.transform;
                arrow.transform.localEulerAngles = new Vector3(0, 225, arrow.transform.localEulerAngles.z);
                pooledArrows.AddItem(arrow);
            }
            DPADPool.GetComponent<PooledFX>().pool = pooledArrows;
            GameObject.DontDestroyOnLoad(ArrowRoot);
        }

        public static void SetupDPADTester(PlayerCharacter instance) {
            instance.gameObject.AddComponent<DPADTester>();
            instance.gameObject.GetComponent<DPADTester>().pool = DPADPool.GetComponent<PooledFX>();
            instance.gameObject.GetComponent<DPADTester>().targetSpell = "";
            DPADPool.GetComponent<PooledFX>().pool = Resources.FindObjectsOfTypeAll<MoveUp>().Select(x => x.gameObject).ToArray();
            foreach(GameObject arrow in DPADPool.GetComponent<PooledFX>().pool) {
                arrow.transform.localEulerAngles = Vector3.zero;
            }
        }

        private void SpawnArrow(DPAD input, bool incorrect) {
            GameObject arrow = PlayerCharacter.instance.GetComponent<DPADTester>().pool.GetNext();
            arrow.transform.position = PlayerCharacter.instance.transform.position;
            switch (input) {
                case DPAD.UP:
                    arrow.transform.localEulerAngles = new Vector3(0, 225, 90);
                    arrow.transform.position += new Vector3(1.25f, 0, 0);
                    break;
                case DPAD.DOWN:
                    arrow.transform.localEulerAngles = new Vector3(0, 225, 270);
                    arrow.transform.position -= new Vector3(1.25f, -1f, 0);
                    break;
                case DPAD.LEFT:
                    arrow.transform.localEulerAngles = new Vector3(0, 225, 0);
                    arrow.transform.position -= new Vector3(3.75f, -1.5f, 0);
                    break;
                case DPAD.RIGHT:
                default:
                    arrow.transform.localEulerAngles = new Vector3(0, 225, 180);
                    arrow.transform.position += new Vector3(3.75f, -2.25f, 0);
                    break;
            }

            if (incorrect) {
                arrow.GetComponent<SpriteRenderer>().color = PaletteEditor.Red;
            } else {
                arrow.GetComponent<SpriteRenderer>().color = PaletteEditor.Green;
            }
            arrow.SetActive(true);
            Arrows.Add(arrow);
        }

        private void Awake() {
            inputsToCast = new UnhollowerBaseLib.Il2CppStructArray<DPAD>(1L);
            spellToggles = GameObject.FindObjectsOfType<ToggleObjectBySpell>().ToArray();
            dpadToChar.Add(DPAD.UP, "u");
            dpadToChar.Add(DPAD.DOWN, "d");
            dpadToChar.Add(DPAD.LEFT, "l");
            dpadToChar.Add(DPAD.RIGHT, "r");
            dpadToChar.Add(DPAD.NONE, "");
            Arrows = new List<GameObject>();
        }

        public override bool CheckInput(Il2CppStructArray<DPAD> inputs, int length) {
            if (TunicRandomizer.Settings.HolyCrossVisualizer) {
                bool incorrect = false;
                bool completedSpell = false;
                if (closestSpellStrings.Count != 0) {
                    if (length > closestSpellStrings[0].Length || (SaveFile.GetInt(SaveFlags.AbilityShuffle) == 1 && SaveFile.GetInt(SaveFlags.HolyCrossUnlocked) == 0)) {
                        incorrect = true;
                    } else {
                        incorrect = closestSpellStrings.All(spell => {
                            for(int i = 0; i < length; i++) {
                                if (dpadToChar[inputs[i]] != spell[i].ToString()) {
                                    return true;
                                }
                            }
                            return false;
                        });
                        if (!incorrect && length == closestSpellStrings[0].Length) {
                            SpawnArrow(inputs[length - 1], incorrect);
                            CompletedSpellEffect();
                            completedSpell = true;
                        }
                    }
                }
                if (!completedSpell) {
                    SpawnArrow(inputs[length-1], incorrect);
                }
            }
            return false;
        }

        public override void SpellEffect() { 
        }

        private void Update() {
            if (TunicRandomizer.Settings.HolyCrossVisualizer) {
                float num = 1000000f;
                closestSpellStrings.Clear();
                foreach (ToggleObjectBySpell toggleObjectBySpell in spellToggles) {
                    if (toggleObjectBySpell != null) {
                        
                        float sqrMagnitude = (toggleObjectBySpell.gameObject.transform.position - this.gameObject.transform.position).sqrMagnitude;
                        float spellDistance = new Vector3(toggleObjectBySpell.minDistance, toggleObjectBySpell.minDistance, toggleObjectBySpell.minDistance).sqrMagnitude / 2f;
                        
                        if (toggleObjectBySpell.targetSpell != null && sqrMagnitude < num && sqrMagnitude < spellDistance && toggleObjectBySpell.stateVar != null && !toggleObjectBySpell.stateVar.BoolValue) {
                            num = sqrMagnitude;

                            toggleObjectBySpell.GetComponents<ToggleObjectBySpell>().ToList().ForEach(toggleObject => {
                                closestSpellStrings.Add(toggleObject.targetSpell);
                                if (toggleObject.acceptLRMirror) {
                                    closestSpellStrings.Add(mirrorString(toggleObject.targetSpell));
                                }
                            });
                        }
                    }
                }
                Arrows = Arrows.Where(arrow => arrow.active).ToList();
            }
        }

        private string mirrorString(string input) {
            return new string(input.Select(x => x == 'l' ? 'r' : (x == 'r' ? 'l' : x)).ToArray());
        }

        public void CompletedSpellEffect(bool iceBoltSpell = false) {
            Arrows.ForEach(arrow => {
                arrow.GetComponent<SpriteRenderer>().color = iceBoltSpell ? Color.cyan : PaletteEditor.Gold;
            });
        }

        public static void MagicSpell_SpellEffect_PostfixPatch(MagicSpell __instance) {
            __instance.GetComponent<DDRSpell>().CompletedSpellEffect(__instance.TryCast<CheapIceboltSpell>());
        }
    }
}
