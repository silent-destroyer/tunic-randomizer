using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TunicRandomizer {
    public class FairyTargets {
        public static Il2CppSystem.Collections.Generic.List<FairyTarget> EntranceTargets = new Il2CppSystem.Collections.Generic.List<FairyTarget> { };
        public static Il2CppSystem.Collections.Generic.List<FairyTarget> EntranceTargetsInLogic = new Il2CppSystem.Collections.Generic.List<FairyTarget> { };
        public static Il2CppSystem.Collections.Generic.List<FairyTarget> ItemTargets = new Il2CppSystem.Collections.Generic.List<FairyTarget> { };
        public static Il2CppSystem.Collections.Generic.List<FairyTarget> ItemTargetsInLogic = new Il2CppSystem.Collections.Generic.List<FairyTarget> { };
        
        public static void CreateFairyTargets() {

            foreach (FairyTarget FairyTarget in Resources.FindObjectsOfTypeAll<FairyTarget>()) {
                GameObject.Destroy(FairyTarget);
            }

            if (ItemLookup.ItemList.Count > 0 || Locations.RandomizedLocations.Count > 0) {

                bool hasChecksInLogicInScene = false;
                List<string> ItemIdsInScene = Locations.VanillaLocations.Keys.Where(ItemId => Locations.VanillaLocations[ItemId].Location.SceneName == SceneManager.GetActiveScene().name
                && SaveFile.GetInt($"randomizer picked up {ItemId}") == 0 &&
                ((SaveFlags.IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld) ? SaveFile.GetInt($"randomizer {ItemId} was collected") == 0 : true)).ToList();

                if (ItemIdsInScene.Count > 0) {
                    foreach (string ItemId in ItemIdsInScene) {
                        Location Location = Locations.VanillaLocations[ItemId].Location;

                        if (GameObject.Find($"fairy target {ItemId}") == null) {
                            CreateFairyTarget($"fairy target {ItemId}", StringToVector3(Location.Position));
                        }

                        if (TunicUtils.ChecksInLogic.Contains(ItemId)) {
                            hasChecksInLogicInScene = true;
                        }
                    }

                    // if there are no checks in logic in the current scene, go create load zone targets specifically for fairy seeking spell with logic
                    if (!hasChecksInLogicInScene) {
                        CreateLogicLoadZoneTargets();
                    }

                    if (GameObject.FindObjectOfType<TrinketWell>() != null) {
                        int CoinCount = Inventory.GetItemByName("Trinket Coin").Quantity + TunicRandomizer.Tracker.ImportantItems["Coins Tossed"];
                        List<int> CoinLevels = new List<int>() { 3, 6, 10, 15, 20 };
                        int CoinsNeededForNextReward = 3;
                        for (int i = 0; i < CoinLevels.Count - 1; i++) {
                            if (SaveFile.GetInt($"randomizer picked up Well Reward ({CoinLevels[i]} Coins) [Trinket Well]") == 1) {
                                CoinsNeededForNextReward = CoinLevels[i + 1];
                            }
                        }

                        if ((Inventory.GetItemByName("Trinket Coin").Quantity + TunicRandomizer.Tracker.ImportantItems["Coins Tossed"]) >= CoinsNeededForNextReward) {
                            CreateFairyTarget($"fairy target Well Reward ({CoinsNeededForNextReward} Coins) [Trinket Well]", GameObject.FindObjectOfType<TrinketWell>().transform.position);
                        }
                    }
                } else {
                    CreateLoadZoneTargets();
                }
            }
        }

        public static void CreateLoadZoneTargets() {
            HashSet<string> ScenesWithItems = new HashSet<string>();

            foreach (FairyTarget FairyTarget in Resources.FindObjectsOfTypeAll<FairyTarget>()) {
                FairyTarget.enabled = false;
            }

            foreach (string ItemId in Locations.VanillaLocations.Keys.Where(itemId => Locations.VanillaLocations[itemId].Location.SceneName != SceneLoaderPatches.SceneName && (SaveFile.GetInt($"randomizer picked up {itemId}") == 0 &&
            ((SaveFlags.IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld) ? SaveFile.GetInt($"randomizer {itemId} was collected") == 0 : true)))) {
                ScenesWithItems.Add(Locations.VanillaLocations[ItemId].Location.SceneName);
            }

            foreach (ScenePortal ScenePortal in Resources.FindObjectsOfTypeAll<ScenePortal>()) {
                if (ScenePortal.id.Contains("customfasttravel")) { continue; }
                if (ScenesWithItems.Contains(TunicPortals.FindPairedPortalSceneFromName(ScenePortal.name))) {
                    CreateFairyTarget($"fairy target {ScenePortal.name}", ScenePortal.transform.position);
                }
            }
        }

        // specifically for fairy seeking spell with logic
        public static void CreateLogicLoadZoneTargets(bool addImmediately = false) {
            foreach (ScenePortal ScenePortal in Resources.FindObjectsOfTypeAll<ScenePortal>()) {
                string portalRegion = TunicPortals.FindPortalRegionFromName(ScenePortal.name);
                string destScene = TunicPortals.FindPairedPortalSceneFromName(ScenePortal.name);
                // check if the entrance is logically accessible first
                if (TunicUtils.PlayerItemsAndRegions.ContainsKey(portalRegion)) {
                    // then check if the scene it leads to has checks in logic
                    foreach (string CheckId in TunicUtils.ChecksInLogic) {
                        if (CheckId.Contains($"[{destScene}]")) {
                            FairyTarget newFairyTarget = CreateFairyTarget($"alt target {ScenePortal.name}", ScenePortal.transform.position);
                            if (addImmediately == true) {
                                ItemTargetsInLogic.Add(newFairyTarget);
                            }
                            break;
                        }
                    }
                }
            }
        }

        public static void CreateEntranceTargets() {
            foreach (ScenePortal ScenePortal in Resources.FindObjectsOfTypeAll<ScenePortal>()) {
                if (ScenePortal.id.Contains("customfasttravel")) { continue; }
                if (ScenePortal.isActiveAndEnabled && SaveFile.GetInt("randomizer entered portal " + ScenePortal.name) != 1) {
                    CreateFairyTarget($"entrance target {ScenePortal.name}", ScenePortal.transform.position);
                }
            }
        }

        private static FairyTarget CreateFairyTarget(string Name, Vector3 Position) {
            GameObject fairyTarget = new GameObject(Name);
            fairyTarget.SetActive(true);
            fairyTarget.AddComponent<FairyTarget>();
            fairyTarget.GetComponent<FairyTarget>().stateVariable = StateVariable.GetStateVariableByName("false");
            fairyTarget.transform.position = Position;
            return fairyTarget.GetComponent<FairyTarget>();
        }

        // used to find fairy targets on scene load or player character start
        public static void FindFairyTargets() {
            ItemTargets.Clear();
            EntranceTargets.Clear();
            ItemTargetsInLogic.Clear();
            EntranceTargetsInLogic.Clear();
            foreach (FairyTarget fairyTarget in Resources.FindObjectsOfTypeAll<FairyTarget>()) {
                if (fairyTarget != null && fairyTarget.isActiveAndEnabled) {
                    if (fairyTarget.name.StartsWith("entrance")) {
                        EntranceTargets.Add(fairyTarget);
                        if (TunicUtils.PlayerItemsAndRegions.ContainsKey(TunicPortals.FindPortalRegionFromName(fairyTarget.name.Replace("entrance target ", "")))) {
                            EntranceTargetsInLogic.Add(fairyTarget);
                        }
                    } else if (fairyTarget.name.StartsWith("fairy")) {
                        ItemTargets.Add(fairyTarget);
                        string targetName = fairyTarget.name.Replace("fairy target ", "");
                        if (TunicUtils.ChecksInLogic.Contains(targetName)) {
                            ItemTargetsInLogic.Add(fairyTarget);
                        } else {
                            // for adjacent scenes, check if the region the portal leads to is in logic, and check if the scene has items in logic
                            string regionName = TunicPortals.FindPortalRegionFromName(targetName);
                            string destSceneName = TunicPortals.FindPairedPortalSceneFromName(targetName);
                            if (TunicUtils.PlayerItemsAndRegions.ContainsKey(regionName)) {
                                foreach (string checkId in TunicUtils.ChecksInLogic) {
                                    if (checkId.Contains(destSceneName)) {
                                        ItemTargetsInLogic.Add(fairyTarget);
                                        break;
                                    }
                                }
                            }
                        }
                    } else if (fairyTarget.name.StartsWith("alt") && !ItemTargetsInLogic.Contains(fairyTarget)) {
                        ItemTargetsInLogic.Add(fairyTarget);
                    }
                }
            }
            if (TunicRandomizer.Settings.SeekingSpellLogic) {
                FairyTarget.registered = ItemTargetsInLogic;
            } else {
                FairyTarget.registered = ItemTargets;
            }
        }

        private static Vector3 StringToVector3(string Position) {
            Position = Position.Replace("(", "").Replace(")", "");
            string[] coords = Position.Split(',');
            Vector3 vector = new Vector3(float.Parse(coords[0], CultureInfo.InvariantCulture), float.Parse(coords[1], CultureInfo.InvariantCulture), float.Parse(coords[2], CultureInfo.InvariantCulture));
            return vector;
        }

        // update what targets are in logic based on the item that was received
        public static void UpdateFairyTargetsInLogic(string newItem) {
            // convert display name to internal name
            foreach (KeyValuePair<string, string> namePair in ItemLookup.SimplifiedItemNames) {
                if (namePair.Value == newItem) {
                    newItem = namePair.Key;
                }
            }
            // add the new item received to the items the player has
            TunicUtils.AddStringToDict(TunicUtils.PlayerItemsAndRegions, newItem);
            TunicUtils.UpdateChecksInLogic();
            // loop through the regular ItemTargets, find ones that are newly in logic
            foreach (FairyTarget fairyTarget in ItemTargets) {
                if (fairyTarget == null || !fairyTarget.isActiveAndEnabled) { continue; }
                string targetName = fairyTarget.name.Replace("fairy target ", "");
                if (!ItemTargetsInLogic.Contains(fairyTarget)) {
                    if (TunicUtils.ChecksInLogic.Contains(targetName)) {
                        ItemTargetsInLogic.Add(fairyTarget);
                    } else {
                        // for adjacent scenes, check if the region the portal leads to is in logic, and check if the scene has items in logic
                        string regionName = TunicPortals.FindPortalRegionFromName(targetName);
                        string destSceneName = TunicPortals.FindPairedPortalSceneFromName(targetName);
                        if (TunicUtils.PlayerItemsAndRegions.ContainsKey(regionName)) {
                            foreach (string checkId in TunicUtils.ChecksInLogic) {
                                if (checkId.Contains(destSceneName)) {
                                    ItemTargetsInLogic.Add(fairyTarget);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            // loop through the regular EntranceTargets, find ones that are newly in logic
            foreach (FairyTarget fairyTarget in EntranceTargets) {
                if (fairyTarget == null || !fairyTarget.isActiveAndEnabled) { continue; }
                if (TunicUtils.PlayerItemsAndRegions.ContainsKey(TunicPortals.FindPortalRegionFromName(fairyTarget.name.Replace("entrance target ", "")))
                        && !EntranceTargetsInLogic.Contains(fairyTarget)) {
                    EntranceTargetsInLogic.Add(fairyTarget);
                }
            }
            // if it is still empty, check if there's locations in logic in adjacent regions
            if (ItemTargetsInLogic.Count == 0) {
                CreateLogicLoadZoneTargets(addImmediately:true);
            }
        }

    }

    public class EntranceSeekerSpell : FairySpell {
        public static List<DPAD> CustomInputs = new List<DPAD>() { };
        
        public EntranceSeekerSpell(IntPtr ptr) : base(ptr) { }

        private void Awake() {
            base.inputsToCast = new UnhollowerBaseLib.Il2CppStructArray<DPAD>(1L);

            CustomInputs = new List<DPAD>() { DPAD.RIGHT, DPAD.DOWN, DPAD.RIGHT, DPAD.UP, DPAD.LEFT, DPAD.UP };
        }

        public override bool CheckInput(Il2CppStructArray<DPAD> inputs, int length) {
            if (length == CustomInputs.Count) {
                for (int i = 0; i < length; i++) {
                    if (inputs[i] != CustomInputs[i]) {
                        return false;
                    }
                }
                DoSpell();
            }
            return false;
        }

        public void DoSpell() {
            if (TunicRandomizer.Settings.SeekingSpellLogic) {
                FairyTarget.registered = FairyTargets.EntranceTargetsInLogic;
            } else {
                FairyTarget.registered = FairyTargets.EntranceTargets;
            }
            PlayerCharacter.instance.GetComponent<FairySpell>().SpellEffect();
            if (TunicRandomizer.Settings.SeekingSpellLogic) {
                FairyTarget.registered = FairyTargets.ItemTargetsInLogic;
            } else {
                FairyTarget.registered = FairyTargets.ItemTargets;
            }
        }
    }
}
