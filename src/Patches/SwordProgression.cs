using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BepInEx.Logging;

namespace TunicRandomizer {
    public class SwordProgression {
        private static ManualLogSource Logger = TunicRandomizer.Logger;

        public static void UpgradeSword(int SwordLevel) {

            SaveFile.SetInt("randomizer sword progression level", SwordLevel);

            if (SwordLevel == 1) {
                //fownd ahn Itehm!
                Inventory.GetItemByName("Stick").Quantity = 1;
                Inventory.GetItemByName("Stick").collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                Inventory.GetItemByName("Stick").collectionMessage.text = TunicRandomizer.Settings.UseTrunicTranslations ? $"fownd ahn Itehm! (<#8ddc6e>lehvuhl 1<#FFFFFF>)" : $"fownd ahn Itehm! \"(<#8ddc6e>Lv. 1<#FFFFFF>)\"";

                ItemPresentation.PresentItem(Inventory.GetItemByName("Stick"));
            } else if (SwordLevel == 2) {
                Inventory.GetItemByName("Sword").Quantity = 1;
                Inventory.GetItemByName("Sword").collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                Inventory.GetItemByName("Sword").collectionMessage.text = TunicRandomizer.Settings.UseTrunicTranslations ? $"fownd ahn Itehm! (<#e99d4c>lehvuhl 2<#FFFFFF>)" : $"fownd ahn Itehm! \"(<#e99d4c>Lv. 2<#FFFFFF>)\"";
                Inventory.GetItemByName("Sword").useAlreadyHaveOneMessage = false;
                ItemPresentation.PresentItem(Inventory.GetItemByName("Sword"));
                List<ButtonAssignableItem> items = Inventory.buttonAssignedItems.ToList();
                for (int i = 0; i < items.Count; i++) {
                    if (items[i] != null && items[i].name == "Stick") {
                        items[i] = Inventory.GetItemByName("Sword").TryCast<ButtonAssignableItem>();
                        SaveFile.SetString($"Item on Button {i}", "Sword");
                    }
                }
                Inventory.buttonAssignedItems = items.ToArray();
            } else if (SwordLevel == 3) {
                Inventory.GetItemByName("Librarian Sword").Quantity = 1;
                ItemPresentation.PresentItem(Inventory.GetItemByName("Librarian Sword"));
                Inventory.GetItemByName("Level Up - Attack").Quantity += 1;
                Inventory.GetItemByName("Librarian Sword").collectionMessage.text = TunicRandomizer.Settings.UseTrunicTranslations ? $"             ? ? ?    (<#ca7be4>lehvuhl 3<#FFFFFF>)" : $"\"        ? ? ? (<#ca7be4>Lv. 3<#FFFFFF>)\"";

                TunicRandomizer.Tracker.ImportantItems["Level Up - Attack"] = Inventory.GetItemByName("Level Up - Attack").Quantity;
                List<ButtonAssignableItem> items = Inventory.buttonAssignedItems.ToList();
                for (int i = 0; i < items.Count; i++) {
                    if (items[i] != null && items[i].name == "Sword") {
                        items[i] = Inventory.GetItemByName("Librarian Sword").TryCast<ButtonAssignableItem>();
                        SaveFile.SetString($"Item on Button {i}", "Librarian Sword");
                    }
                }
                Inventory.buttonAssignedItems = items.ToArray();
            } else if (SwordLevel >= 4) {
                Inventory.GetItemByName("Heir Sword").Quantity = 1;
                ItemPresentation.PresentItem(Inventory.GetItemByName("Heir Sword"));
                Inventory.GetItemByName("Level Up - Attack").Quantity += 1;
                Inventory.GetItemByName("Heir Sword").collectionMessage.text = TunicRandomizer.Settings.UseTrunicTranslations ? $"             ! ! !    (<#5de7cf>lehvuhl 4<#FFFFFF>)" : $"\"        ! ! ! (<#5de7cf>Lv. 4<#FFFFFF>)\"";

                TunicRandomizer.Tracker.ImportantItems["Level Up - Attack"] = Inventory.GetItemByName("Level Up - Attack").Quantity;
                List<ButtonAssignableItem> items = Inventory.buttonAssignedItems.ToList();
                for (int i = 0; i < items.Count; i++) {
                    if (items[i] != null && items[i].name == "Librarian Sword") {
                        items[i] = Inventory.GetItemByName("Heir Sword").TryCast<ButtonAssignableItem>();
                        SaveFile.SetString($"Item on Button {i}", "Heir Sword");
                    }
                }
                Inventory.buttonAssignedItems = items.ToArray();
            }
        }

        public static void ResetSword() {
            string SwordPath = "_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy/";
            GameObject SwordProxy = GameObject.Find(SwordPath);
            if (SwordProxy.GetComponent<MeshFilter>() == null) {
                GameObject.Destroy(SwordProxy.GetComponent<MeshFilter>());
            }
            if (SwordProxy.GetComponent<MeshRenderer>() == null) {
                GameObject.Destroy(SwordProxy.GetComponent<MeshRenderer>());
            }
            SwordProxy.AddComponent<MeshFilter>().mesh = ModelSwaps.Items["Sword"].GetComponent<MeshFilter>().mesh;
            SwordProxy.AddComponent<MeshRenderer>().materials = ModelSwaps.Items["Sword"].GetComponent<MeshRenderer>().materials;
            SwordProxy.transform.GetChild(0).localPosition = new Vector3(0f, 0.7653f, 0f);
            SwordProxy.transform.GetChild(1).GetComponent<BoxCollider>().size = new Vector3(0.33f, 1.25f, 1f);
            SwordProxy.transform.GetChild(2).GetComponent<BoxCollider>().size = new Vector3(0.2f, 1.32f, 0.2f);
            if (SwordProxy.transform.childCount == 5) {
                GameObject.Destroy(SwordProxy.transform.GetChild(4).gameObject);
            }
        }

        public static void EnableSecondSwordFromExisting(GameObject SwordProxy) {

            if (SwordProxy != null) {
                if (SwordProxy.GetComponent<MeshFilter>() != null && SwordProxy.GetComponent<MeshRenderer>() != null) {
                    GameObject.Destroy(SwordProxy.GetComponent<MeshFilter>());
                    GameObject.Destroy(SwordProxy.GetComponent<MeshRenderer>());
                }
                if (SwordProxy.transform.childCount >= 3) {
                    SwordProxy.transform.GetChild(0).localPosition = new Vector3(0f, 1.7f, 0f);
                    SwordProxy.transform.GetChild(1).GetComponent<BoxCollider>().size = new Vector3(0.66f, 4.16f, 2f);
                    SwordProxy.transform.GetChild(2).GetComponent<BoxCollider>().size = new Vector3(0.4f, 4.4f, 0.4f);
                }

                if (SwordProxy.transform.childCount == 5) {
                    GameObject.Destroy(SwordProxy.transform.GetChild(4).gameObject);
                }
                GameObject Sword = new GameObject("sword");
                Sword.AddComponent<MeshFilter>().mesh = ModelSwaps.SecondSword.GetComponent<MeshFilter>().mesh;
                Sword.AddComponent<MeshRenderer>().materials = ModelSwaps.SecondSword.GetComponent<MeshRenderer>().materials;
                Sword.transform.parent = SwordProxy.transform;
                Sword.transform.localScale = new Vector3(0.5f, 0.3f, 0.5f);
                Sword.transform.localRotation = Quaternion.identity;
                Sword.transform.localPosition = Vector3.zero;
            } else {
                Logger.LogError("Could not find sword object to replace with Sword Lvl 3!");
            }
        }

        public static void EnableThirdSwordFromExisting(GameObject SwordProxy) {

            /*            string SwordPath = "_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy/";
                        GameObject SwordProxy = GameObject.Find(SwordPath);*/
            if (SwordProxy != null) {
                if (SwordProxy.GetComponent<MeshFilter>() != null && SwordProxy.GetComponent<MeshRenderer>() != null) {
                    GameObject.Destroy(SwordProxy.GetComponent<MeshFilter>());
                    GameObject.Destroy(SwordProxy.GetComponent<MeshRenderer>());
                }
                if (SwordProxy.transform.childCount >= 3) {
                    SwordProxy.transform.GetChild(0).localPosition = new Vector3(0f, 1.85f, 0f);
                    SwordProxy.transform.GetChild(1).GetComponent<BoxCollider>().size = new Vector3(0.66f, 4.36f, 2f);
                    SwordProxy.transform.GetChild(2).GetComponent<BoxCollider>().size = new Vector3(0.4f, 4.6f, 0.4f);
                    SwordProxy.transform.GetChild(1).GetComponent<HitTrigger>().unblockable = true;
                    SwordProxy.transform.GetChild(2).GetComponent<HitTrigger>().unblockable = true;
                }
                if (SwordProxy.transform.childCount == 5) {
                    GameObject.Destroy(SwordProxy.transform.GetChild(4).gameObject);
                }
                GameObject Sword = new GameObject("sword");
                Sword.AddComponent<MeshFilter>().mesh = ModelSwaps.ThirdSword.GetComponent<MeshFilter>().mesh;
                Sword.AddComponent<MeshRenderer>().materials = ModelSwaps.ThirdSword.GetComponent<MeshRenderer>().materials;
                Sword.transform.parent = SwordProxy.transform;
                Sword.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                Sword.transform.localRotation = new Quaternion(0.7071f, 0f, 0f, -0.7071f);
                Sword.transform.localPosition = Vector3.zero;
            } else {
                Logger.LogError("Could not find sword object to replace with Sword Lvl 4!");
            }
        }

        public static void CreateSwordItems() {
            Logger.LogInfo("creating sword items");
            ButtonAssignableItem LibrarianSword = ScriptableObject.CreateInstance<ButtonAssignableItem>();
            ButtonAssignableItem HeirSword = ScriptableObject.CreateInstance<ButtonAssignableItem>();
            LibrarianSword.name = "Librarian Sword";
            LibrarianSword.icon = ModelSwaps.FindSprite("Inventory items_koban_hp");
            LibrarianSword.collectionMessage = new LanguageLine();
            LibrarianSword.collectionMessage.text = $"\"        ? ? ? (<#ca7be4>Lv. 3<#FFFFFF>)\"";
            HeirSword.name = "Heir Sword";
            HeirSword.icon = ModelSwaps.FindSprite("Inventory items_koban_sp");
            HeirSword.collectionMessage = new LanguageLine();
            HeirSword.collectionMessage.text = $"\"        ! ! ! (<#5de7cf>Lv. 4<#FFFFFF>)\"";
            LibrarianSword.controlAction = "";
            HeirSword.controlAction = "";
            LibrarianSword.suppressQuantity = true;
            HeirSword.suppressQuantity = true;
            for (int i = 0; i < Inventory.itemList.Count; i++) {
                if (Inventory.itemList[i].name == "Sword") {
                    Inventory.itemList.Insert(i + 1, LibrarianSword);
                    Inventory.itemList.Insert(i + 2, HeirSword);
                    break;
                }
            }
            Logger.LogInfo("Done creating swords");
        }

        public static void CreateSwordItemBehaviours(PlayerCharacter instance) {
            GameObject swordProxy = GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/").transform.GetChild(1).gameObject;
            // fix: reset localposition to 0 on instantiate sword root object
            instance.gameObject.AddComponent<SwordItemBehaviour>().item = Inventory.GetItemByName("Librarian Sword").TryCast<ButtonAssignableItem>();
            instance.gameObject.AddComponent<SwordItemBehaviour>().item = Inventory.GetItemByName("Heir Sword").TryCast<ButtonAssignableItem>();
            List<ItemBehaviour> behaviours = instance.itemBehaviours.ToList();
            foreach (SwordItemBehaviour sword in instance.gameObject.GetComponents<SwordItemBehaviour>()) {
                if (sword.item.name == "Librarian Sword") {
                    sword.itemRoot = GameObject.Instantiate(swordProxy);
                    sword.itemRoot.transform.parent = swordProxy.transform.parent;
                    sword.itemRoot.name = sword.item.name;
                    EnableSecondSwordFromExisting(sword.itemRoot);
                    sword.itemRoot.transform.localRotation = swordProxy.transform.localRotation;
                    sword.itemRoot.transform.localPosition = swordProxy.transform.localPosition;
                    behaviours.Add(sword);
                }
                if (sword.item.name == "Heir Sword") {
                    sword.itemRoot = GameObject.Instantiate(swordProxy);
                    sword.itemRoot.transform.parent = swordProxy.transform.parent;
                    sword.itemRoot.name = sword.item.name;
                    EnableThirdSwordFromExisting(sword.itemRoot);
                    sword.itemRoot.transform.localRotation = swordProxy.transform.localRotation;
                    sword.itemRoot.transform.localPosition = swordProxy.transform.localPosition;
                    behaviours.Add(sword);
                }
            }
            instance.itemBehaviours = behaviours.ToArray();
        }

        public static bool HitReceiver_ReceiveHit_PrefixPatch(HitReceiver __instance, ref HitType hitType, ref bool unblockable, ref bool isPlayerCharacterMelee) {

            if ((__instance.GetComponent<TuningForkBell>() != null || __instance.GetComponent<PowerSwitch>() != null) && isPlayerCharacterMelee) {
                unblockable = false;
            }

            return true;
        }
    }
}
