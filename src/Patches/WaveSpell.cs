using System;
using System.Collections.Generic;
using UnhollowerBaseLib;
using UnityEngine;

namespace TunicRandomizer {
    public class WaveSpell : HealSpell {

        public static List<DPAD> CustomInputs = new List<DPAD>() { };

        public WaveSpell(IntPtr ptr) : base(ptr) { }

        private void Awake() {
            base.inputsToCast = new UnhollowerBaseLib.Il2CppStructArray<DPAD>(1L);
            base.manaCost = 0;
            base.hpToGive = 0;
            base.particles = PlayerCharacter.instance.transform.GetChild(8).gameObject.GetComponent<ParticleSystem>();
            List<DPAD> dPADs = new List<DPAD>() { DPAD.UP, DPAD.RIGHT, DPAD.LEFT, DPAD.DOWN };
            List<int> inputs = new List<int>() { 2, 3, 1, 3, 2, 2, 3, 1, 0, 1, 1, 3, 1, 1, 0, 2, 2, 2, 0, 1, 0, 1, 1, 3, 3, 2, 0, 0, 2, 3 };
            CustomInputs.Clear();
            for (int i = 0; i < inputs.Count; i++) {
                CustomInputs.Add(dPADs[inputs[i]]);
            }
        }

        public override bool CheckInput(Il2CppStructArray<DPAD> inputs, int length) {
            if (length == CustomInputs.Count) {
                for (int i = 0; i < length; i++) {
                    if (inputs[i] != CustomInputs[i]) {
                        return false;
                    }
                }
                DoWave();
            }
            return false;
        }

        public void DoWave() {
            base.SpellEffect();
            PlayerCharacter.instance.transform.localRotation = new Quaternion(0, 0.9239f, 0, -0.3827f);
            PlayerCharacter.instance.GetComponent<Animator>().SetBool("wave", true);
            GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/head/floppy hat").SetActive(true);
            PaletteEditor.PartyHatEnabled = true;
            PaletteEditor.ApplyCelShading();
            if (!OptionsGUIPatches.BonusOptionsUnlocked) {
                AreaData AreaData = ScriptableObject.CreateInstance<AreaData>();
                AreaData.topLine = ScriptableObject.CreateInstance<LanguageLine>();
                AreaData.topLine.text = TunicRandomizer.Settings.UseTrunicTranslations ? "bOnuhs kuhstuhmizA$uhn uhnlawkd" : $"\"BONUS CUSTOMIZATION UNLOCKED\"";
                AreaData.bottomLine = ScriptableObject.CreateInstance<LanguageLine>();
                AreaData.bottomLine.text = $"%ah^ks for plAi^! (prehs 3 too wAv)";
                AreaLabel.ShowLabel(AreaData);
                OptionsGUIPatches.BonusOptionsUnlocked = true;
            }
        }

        public static void MagicSpell_CheckInput_PostfixPatch(MagicSpell __instance, Il2CppStructArray<DPAD> inputs, int length, ref bool __result) {
            WaveSpell WaveSpell = __instance.TryCast<WaveSpell>();
            if (WaveSpell != null) {
                WaveSpell.CheckInput(inputs, length);
            }

            EntranceSeekerSpell EntranceSeekerSpell = __instance.TryCast<EntranceSeekerSpell>();
            if (EntranceSeekerSpell != null) {
                EntranceSeekerSpell.CheckInput(inputs, length);
            }
        }
    }
}
