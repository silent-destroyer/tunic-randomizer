using BepInEx.Logging;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {
    public class CustomItemBehaviors {

        public static bool CanTakeGoldenHit = false;
        public static bool CanSwingGoldenSword = false;
        public static GameObject FoxBody;
        public static GameObject FoxHair;
        public static GameObject FoxCape;
        public static GameObject GhostFoxBody;
        public static GameObject GhostFoxHair;
        public static void CreateCustomItems() {
            ButtonAssignableItem LibrarianSword = ScriptableObject.CreateInstance<ButtonAssignableItem>();
            ButtonAssignableItem HeirSword = ScriptableObject.CreateInstance<ButtonAssignableItem>();
            Item GoldQuestagon = ScriptableObject.CreateInstance<Item>();
            ButtonAssignableItem DathStone = ScriptableObject.CreateInstance<ButtonAssignableItem>();
            ButtonAssignableItem Cape = ScriptableObject.CreateInstance<ButtonAssignableItem>();

            LibrarianSword.name = "Librarian Sword";
            LibrarianSword.collectionMessage = new LanguageLine();
            LibrarianSword.collectionMessage.text = $"\"        ? ? ? (<#ca7be4>Lv. 3<#FFFFFF>)\"";
            LibrarianSword.controlAction = "";
            LibrarianSword.suppressQuantity = true;

            HeirSword.name = "Heir Sword";
            HeirSword.collectionMessage = new LanguageLine();
            HeirSword.collectionMessage.text = $"\"        ! ! ! (<#5de7cf>Lv. 4<#FFFFFF>)\"";
            HeirSword.controlAction = "";
            HeirSword.suppressQuantity = true;

            GoldQuestagon.name = "Hexagon Gold";
            GoldQuestagon.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
            GoldQuestagon.collectionMessage.text = $"    #uh sEl wEkinz\"...\"";
            GoldQuestagon.controlAction = "";

            DathStone.name = "Dath Stone";
            DathStone.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
            DathStone.collectionMessage.text = $"dah% stOn\"!?\"";
            DathStone.controlAction = "";
            DathStone.icon = Inventory.GetItemByName("Dash Stone").icon;
            DathStone.suppressQuantity = true;

            Cape.name = "Cape";
            Cape.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
            Cape.collectionMessage.text = $"A kAp fruhm law^ uhgO...";
            Cape.controlAction = "";
            Cape.icon = null;
            Cape.spellStringForFreeItem = "dldrulurdldrulu";
            StateVariable GrantedCape = ScriptableObject.CreateInstance<StateVariable>();
            GrantedCape.name = "Granted Cape";
            StateVariable.stateVariableList.Add(GrantedCape);
            Cape.freeItemCountStateVar = GrantedCape;

            StateVariable GraveHintStateVar = ScriptableObject.CreateInstance<StateVariable>();
            GraveHintStateVar.name = "randomizer got all 6 grave items";
            StateVariable.stateVariableList.Add(GraveHintStateVar);

            StateVariable CryptSecretStateVar = ScriptableObject.CreateInstance<StateVariable>();
            CryptSecretStateVar.name = "randomizer crypt secret filigree door opened";
            StateVariable.stateVariableList.Add(CryptSecretStateVar);

            Inventory.itemList.Add(GoldQuestagon);
            Inventory.itemList.Add(DathStone);
            Inventory.itemList.Add(Cape);
            Item Torch = Inventory.GetItemByName("Torch");
            for (int i = 0; i < Inventory.itemList.Count; i++) {
                if (Inventory.itemList[i].name == "Sword") {
                    Inventory.itemList.Insert(i + 1, LibrarianSword);
                    Inventory.itemList.Insert(i + 2, HeirSword);
                    break;
                }
                if (Inventory.itemList[i].name == "Torch") {
                    Inventory.itemList.RemoveAt(i);
                    Inventory.itemList.Add(Torch);
                }
            }
        }

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
                    FoxCape = new GameObject();
                    FoxCape.AddComponent<MeshRenderer>().materials = PaletteEditor.FoxCape.GetComponent<CreatureMaterialManager>().originalMaterials;
                }

                
                GameObject.Find("_Fox(Clone)/fox").GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                GameObject.Find("_Fox(Clone)/fox hair").GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                GameObject.Find("_Fox(Clone)/fox").GetComponent<CreatureMaterialManager>()._ghostMaterialArray = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                GameObject.Find("_Fox(Clone)/fox hair").GetComponent<CreatureMaterialManager>()._ghostMaterialArray = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                PaletteEditor.FoxCape.GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                PaletteEditor.FoxCape.GetComponent<CreatureMaterialManager>()._ghostMaterialArray = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
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
                GameObject.DontDestroyOnLoad(FoxCape);

                CanTakeGoldenHit = true;
                CanSwingGoldenSword = true;
            }
            return false;
        }

        public static bool BoneItemBehavior_onActionButtonDown_PrefixPatch(BoneItemBehaviour __instance) {
            if (__instance.item.name == "Torch") {
                if (StateVariable.GetStateVariableByName("Is Night").BoolValue) {
                    __instance.confirmationPromptLine.text = $"wAk fruhm #is drEm\nahnd rEturn too \"Old House\"?";
                } else {
                    __instance.confirmationPromptLine.text = $"wAk fruhm #is drEm\nahnd rEturn too \"Overworld\"?";
                }
            } else {
                if (SceneLoaderPatches.SceneName == "g_elements") {
                    __instance.confirmationPromptLine.text = $"wAk fruhm #is drEm\nahnd rEturn too \"???\"";
                } else if (SceneLoaderPatches.SceneName == "Posterity") {
                    __instance.confirmationPromptLine.text = $"wAk fruhm #is drEm\nahnd rEturn too \"Overworld\"?";
                } else {
                    if (SaveFile.GetString("randomizer last campfire scene name for dath stone") != "" && SaveFile.GetString("randomizer last campfire id for dath stone") != "") {
                        __instance.confirmationPromptLine.text = $"wAk fruhm #is drEm\nahnd rEturn too \"{Locations.SimplifiedSceneNames[SaveFile.GetString("randomizer last campfire scene name for dath stone")]}?\"";
                    } else {
                        __instance.confirmationPromptLine.text = $"wAk fruhm #is drEm\nahnd rEturn too \"{Locations.SimplifiedSceneNames[SaveFile.GetString("last campfire scene name")]}?\"";
                    }
                }
            }

            return true;
        }

        public static bool BoneItemBehavior_confirmBoneUseCallback_PrefixPatch(BoneItemBehaviour __instance) {

            if (__instance.item.name == "Torch") {
                if (StateVariable.GetStateVariableByName("Is Night").BoolValue) {
                    SaveFile.SetString("last campfire scene name", "Overworld Interiors");
                    SaveFile.SetString("last campfire id", "bed");
                } else {
                    SaveFile.SetString("last campfire scene name", "Overworld Redux");
                    SaveFile.SetString("last campfire id", "checkpoint");
                }
            } else {
                if (SceneLoaderPatches.SceneName == "g_elements") {
                    SaveFile.SetString("last campfire scene name", "Posterity");
                    SaveFile.SetString("last campfire id", "campfire");
                    SaveFile.SetInt(RescuedLostFox, 1);
                } else if (SceneLoaderPatches.SceneName == "Posterity") {
                    SaveFile.SetString("last campfire scene name", "Overworld Redux");
                    SaveFile.SetString("last campfire id", "checkpoint");
                } else if (SaveFile.GetString("randomizer last campfire scene name for dath stone") != "" && SaveFile.GetString("randomizer last campfire id for dath stone") != "") {
                    SaveFile.SetString("last campfire scene name", SaveFile.GetString("randomizer last campfire scene name for dath stone"));
                    SaveFile.SetString("last campfire id", SaveFile.GetString("randomizer last campfire id for dath stone"));
                }
            }            
            PlayerCharacter.instance.gameObject.AddComponent<Rotate>();
            PlayerCharacterPatches.IsTeleporting = true;
            return true;
        }

        public static bool ForcewandItemBehaviour__throwBeamCoroutine_d__32_MoveNext_PrefixPatch(ForcewandItemBehaviour._throwBeamCoroutine_d__32 __instance) {

            // Checks if the toggle for disabling ice grappling is on, and if target is a frozen enemy
            if (__instance.__8__1 != null && __instance.__8__1.gameObjectToTether != null
                && __instance.__8__1.gameObjectToTether.GetComponent<Monster>() != null && __instance.__8__1.gameObjectToTether.GetComponent<Monster>().Frozen
                && TunicRandomizer.Settings.RaceMode && TunicRandomizer.Settings.DisableIceGrappling) {
                // Allow the east forest slime to be grappled still
                if (SceneManager.GetActiveScene().name == "East Forest Redux" && __instance.__8__1.gameObjectToTether.transform.parent != null && __instance.__8__1.gameObjectToTether.transform.parent.name == "_MONSTERS ALWAYS") {
                    return true;
                }

                // Cancel orb animation and refund mp to player if ice grapples are disabled
                __instance.__8__1.gameObjectToTether = null;
                __instance.__8__1.ztargOfTetheredObject = null;
                __instance.__4__this.tentacleBeamRenderer.enabled = false;
                
                __instance.__4__this.StopAllCoroutines();
                PlayerCharacter.SetMP(PlayerCharacter.GetMP() + __instance.__4__this.usageCostDat.IntValue);

            }
            return true;
        }

        public static void SetupTorchItemBehaviour(PlayerCharacter instance) {
            instance.GetComponent<BoneItemBehaviour>().item = Inventory.GetItemByName("Dath Stone").TryCast<ButtonAssignableItem>();
            List<ItemBehaviour> itemBehaviours = instance.itemBehaviours.ToList();
            BoneItemBehaviour bone = instance.gameObject.AddComponent<BoneItemBehaviour>();
            bone.confirmationPromptLine = instance.gameObject.GetComponent<BoneItemBehaviour>().confirmationPromptLine;
            bone.item = Inventory.GetItemByName("Torch").TryCast<ButtonAssignableItem>();
            itemBehaviours.Add(bone);
            instance.itemBehaviours = itemBehaviours.ToArray();
        }

        public static bool Item_shouldShowInInventory_GetterPatch(Item __instance, ref bool __result) {
            switch (__instance.name) {
                case "Cape":
                    __result = false;
                    return false;
                default:
                    return true;
            }
        }
    }
}
