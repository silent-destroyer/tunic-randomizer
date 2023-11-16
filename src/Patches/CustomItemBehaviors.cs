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

                GameObject.Find("_Fox(Clone)/fox").GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                GameObject.Find("_Fox(Clone)/fox hair").GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                GameObject.Find("_Fox(Clone)/fox").GetComponent<CreatureMaterialManager>()._ghostMaterialArray = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                GameObject.Find("_Fox(Clone)/fox hair").GetComponent<CreatureMaterialManager>()._ghostMaterialArray = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                GameObject Hand = GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R");
                if (Hand != null) {
                    Hand.transform.GetChild(1).GetComponent<MeshRenderer>().materials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                    if (Hand.transform.childCount >= 12) {
                        Hand.transform.GetChild(12).GetChild(4).GetComponent<MeshRenderer>().materials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                        Hand.transform.GetChild(13).GetChild(4).GetComponent<MeshRenderer>().materials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                    }
                }

                GameObject.DontDestroyOnLoad(FoxBody);
                GameObject.DontDestroyOnLoad(FoxHair);
                GameObject.DontDestroyOnLoad(GhostFoxBody);
                GameObject.DontDestroyOnLoad(GhostFoxHair);
                
                CanTakeGoldenHit = true;
                CanSwingGoldenSword = true;
            }
            return false;
        }


        public static bool BoneItemBehavior_onActionButtonDown_PrefixPatch(BoneItemBehaviour __instance) {
            if (__instance.item.name == "Torch") {
                __instance.confirmationPromptLine.text = $"wAk fruhm #is drEm\nahnd rEturn too {Translations.Translate("Overworld", true)}?";
            } else {
                if (SceneLoaderPatches.SceneName == "g_elements") {
                    __instance.confirmationPromptLine.text = $"wAk fruhm #is drEm ahnd rEturn too ???";
                } else if (SceneLoaderPatches.SceneName == "Posterity") {
                    __instance.confirmationPromptLine.text = $"wAk fruhm #is drEm\nahnd rEturn too {Translations.Translate("Overworld", true)}?";
                } else {
                    if (SaveFile.GetString("randomizer last campfire scene name for dath stone") != "" && SaveFile.GetString("randomizer last campfire id for dath stone") != "") {
                        __instance.confirmationPromptLine.text = $"wAk fruhm #is drEm\nahnd rEturn too {Translations.Translate(Hints.SimplifiedSceneNames[SaveFile.GetString("randomizer last campfire scene name for dath stone")], true)}?";
                    } else {
                        __instance.confirmationPromptLine.text = $"wAk fruhm #is drEm\nahnd rEturn too {Translations.Translate(Hints.SimplifiedSceneNames[SaveFile.GetString("last campfire scene name")], true)}?";
                    }
                }
            }
            return true;
        }

        public static bool BoneItemBehavior_confirmBoneUseCallback_PrefixPatch(BoneItemBehaviour __instance) {
            if (__instance.item.name == "Torch") {
                SaveFile.SetString("last campfire scene name", "Overworld Redux");
                SaveFile.SetString("last campfire id", "checkpoint");
            } else {
                if (SceneLoaderPatches.SceneName == "g_elements") {
                    SaveFile.SetString("last campfire scene name", "Posterity");
                    SaveFile.SetString("last campfire id", "campfire");
                    SaveFile.SetInt("randomizer sent lost fox home", 1);
                } else if (SceneLoaderPatches.SceneName == "Posterity") {
                    SaveFile.SetString("last campfire scene name", "Overworld Redux");
                    SaveFile.SetString("last campfire id", "checkpoint");
                } else if (SaveFile.GetString("randomizer last campfire scene name for dath stone") != "" && SaveFile.GetString("randomizer last campfire id for dath stone") != "") {
                    SaveFile.SetString("last campfire scene name", SaveFile.GetString("randomizer last campfire scene name for dath stone"));
                    SaveFile.SetString("last campfire id", SaveFile.GetString("randomizer last campfire id for dath stone"));
                }
            }
            PlayerCharacter.instance.gameObject.AddComponent<Rotate>();
            IsTeleporting = true;
            return true;
        }

        public static void SetupTorchItemBehaviour(PlayerCharacter instance) {
            List<ItemBehaviour> itemBehaviours = instance.itemBehaviours.ToList();
            BoneItemBehaviour bone = instance.gameObject.AddComponent<BoneItemBehaviour>();
            bone.confirmationPromptLine = instance.gameObject.GetComponent<BoneItemBehaviour>().confirmationPromptLine;
            bone.item = Inventory.GetItemByName("Torch").TryCast<ButtonAssignableItem>();
            itemBehaviours.Add(bone);
            instance.itemBehaviours = itemBehaviours.ToArray();
        }
    }
}
