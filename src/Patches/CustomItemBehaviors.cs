using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib;
using UnityEngine;
using BepInEx.Logging;

namespace TunicRandomizer {
    public class CustomItemBehaviors {
        public static ManualLogSource Logger = TunicRandomizer.Logger;

        public static bool CanTakeGoldenHit = false;
        public static bool CanSwingGoldenSword = false;
        public static GameObject FoxBody;
        public static GameObject FoxHair;
        public static GameObject GhostFoxBody;
        public static GameObject GhostFoxHair;
        public static GameObject Sword;
        public static bool IsTeleporting = false;

        public static bool SpearItemBehaviour_onActionButtonDown_PrefixPatch(SpearItemBehaviour __instance) {
            if (PlayerCharacter.GetMP() != 0 && (!CanTakeGoldenHit || !CanSwingGoldenSword)) {
                PlayerCharacter.SetMP(PlayerCharacter.GetMP() - 40 > 0 ? PlayerCharacter.GetMP() - 40 : 0);
                SFX.PlayAudioClipAtFox(PlayerCharacter.instance.blockSFX);
                if (!CanTakeGoldenHit) {
                    FoxBody = new GameObject();
                    FoxBody.AddComponent<MeshRenderer>().materials = GameObject.Find("_Fox(Clone)/fox").GetComponent<CreatureMaterialManager>().originalMaterials;
                    FoxHair = new GameObject();
                    FoxHair.AddComponent<MeshRenderer>().materials = GameObject.Find("_Fox(Clone)/fox hair").GetComponent<CreatureMaterialManager>().originalMaterials;
                    GhostFoxBody = new GameObject();
                    GhostFoxBody.AddComponent<MeshRenderer>().materials = GameObject.Find("_Fox(Clone)/fox").GetComponent<CreatureMaterialManager>().ghostMaterialArray;
                    GhostFoxHair = new GameObject();
                    GhostFoxHair.AddComponent<MeshRenderer>().materials = GameObject.Find("_Fox(Clone)/fox hair").GetComponent<CreatureMaterialManager>().ghostMaterialArray;
                }
                Sword = new GameObject();
                if (SaveFile.GetInt("randomizer sword progression level") >= 3) {
                    Sword.AddComponent<MeshRenderer>().materials = GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy").transform.GetChild(4).GetComponent<MeshRenderer>().materials;
                } else {
                    Sword.AddComponent<MeshRenderer>().materials = GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy").GetComponent<MeshRenderer>().materials;
                }
                GameObject.Find("_Fox(Clone)/fox").GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                GameObject.Find("_Fox(Clone)/fox hair").GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                GameObject.Find("_Fox(Clone)/fox").GetComponent<CreatureMaterialManager>()._ghostMaterialArray = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                GameObject.Find("_Fox(Clone)/fox hair").GetComponent<CreatureMaterialManager>()._ghostMaterialArray = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                if (SaveFile.GetInt("randomizer sword progression level") >= 3) {
                    GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy").transform.GetChild(4).GetComponent<MeshRenderer>().materials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                } else {
                    GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy").GetComponent<MeshRenderer>().materials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                }

                GameObject.DontDestroyOnLoad(FoxBody);
                GameObject.DontDestroyOnLoad(FoxHair);
                GameObject.DontDestroyOnLoad(GhostFoxBody);
                GameObject.DontDestroyOnLoad(GhostFoxHair);
                GameObject.DontDestroyOnLoad(Sword);
                
                CanTakeGoldenHit = true;
                CanSwingGoldenSword = true;
            }
            return false;
        }


        public static bool BoneItemBehavior_onActionButtonDown_PrefixPatch(BoneItemBehaviour __instance) {
            if (SceneLoaderPatches.SceneName == "g_elements") {
                __instance.confirmationPromptLine.text = $"wAk fruhm #is drEm \n ahnd rEturn too \"???\"";
            } else if (SceneLoaderPatches.SceneName == "Posterity") {
                __instance.confirmationPromptLine.text = $"wAk fruhm #is drEm \n ahnd rEturn too \"Overworld\"?";
            } else {
                __instance.confirmationPromptLine.text = $"wAk fruhm #is drEm \n ahnd rEturn too \"{Hints.SimplifiedSceneNames[SaveFile.GetString("last campfire scene name")]}\"?";
            }

            return true;
        }

        public static bool BoneItemBehavior_confirmBoneUseCallback_PrefixPatch(BoneItemBehaviour __instance) {
            if (SceneLoaderPatches.SceneName == "g_elements") {
                SaveFile.SetString("last campfire scene name", "Posterity");
                SaveFile.SetString("last campfire id", "campfire");
            }
            if (SceneLoaderPatches.SceneName == "Posterity") {
                SaveFile.SetString("last campfire scene name", "Overworld Redux");
                SaveFile.SetString("last campfire id", "checkpoint");
            }
            PlayerCharacter.instance.gameObject.AddComponent<Rotate>();
            IsTeleporting = true;
            return true;
        }
    }
}
