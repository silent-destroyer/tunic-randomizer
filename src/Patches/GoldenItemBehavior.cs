using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib;
using UnityEngine;
using BepInEx.Logging;

namespace TunicRandomizer {
    public class GoldenItemBehavior {
        public static ManualLogSource Logger = TunicRandomizer.Logger;

        public static bool CanTakeGoldenHit = false;
        public static bool CanSwingGoldenSword = false;
        public static GameObject FoxBody;
        public static GameObject FoxHair;
        public static GameObject Sword;
        public static bool SpearItemBehaviour_onActionButtonDown_PrefixPatch(SpearItemBehaviour __instance) {
            if (PlayerCharacter.GetMP() != 0 && (!CanTakeGoldenHit || !CanSwingGoldenSword)) {
                PlayerCharacter.SetMP(PlayerCharacter.GetMP() - 40 > 0 ? PlayerCharacter.GetMP() - 40 : 0);
                SFX.PlayAudioClipAtFox(PlayerCharacter.instance.blockSFX);
                FoxBody = new GameObject();
                FoxBody.AddComponent<MeshRenderer>().materials = GameObject.Find("_Fox(Clone)/fox").GetComponent<CreatureMaterialManager>().originalMaterials;
                FoxHair = new GameObject();
                FoxHair.AddComponent<MeshRenderer>().materials = GameObject.Find("_Fox(Clone)/fox hair").GetComponent<CreatureMaterialManager>().originalMaterials;
                Sword = new GameObject();
                if (SaveFile.GetInt("randomizer sword progression level") >= 3) {
                    Sword.AddComponent<MeshRenderer>().materials = GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy").transform.GetChild(4).GetComponent<MeshRenderer>().materials;
                } else {
                    Sword.AddComponent<MeshRenderer>().materials = GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy").GetComponent<MeshRenderer>().materials;
                }
                GameObject.Find("_Fox(Clone)/fox").GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                GameObject.Find("_Fox(Clone)/fox hair").GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                if (SaveFile.GetInt("randomizer sword progression level") >= 3) {
                    GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy").transform.GetChild(4).GetComponent<MeshRenderer>().materials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                } else {
                    GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy").GetComponent<MeshRenderer>().materials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                }

                GameObject.DontDestroyOnLoad(FoxBody);
                GameObject.DontDestroyOnLoad(FoxHair);
                GameObject.DontDestroyOnLoad(Sword);
                
                CanTakeGoldenHit = true;
                CanSwingGoldenSword = true;
            }
            return false;
        }
    }
}
