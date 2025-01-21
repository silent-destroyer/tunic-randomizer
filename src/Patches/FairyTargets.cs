﻿using Archipelago.MultiClient.Net.Enums;
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
        public static Il2CppSystem.Collections.Generic.List<FairyTarget> AdjItemTargets = new Il2CppSystem.Collections.Generic.List<FairyTarget> { };
        public static Il2CppSystem.Collections.Generic.List<FairyTarget> AdjItemTargetsInLogic = new Il2CppSystem.Collections.Generic.List<FairyTarget> { };

        public static void CreateFairyTargets() {
            ItemTargets.Clear();
            EntranceTargets.Clear();
            ItemTargetsInLogic.Clear();
            EntranceTargetsInLogic.Clear();
            AdjItemTargets.Clear();
            AdjItemTargetsInLogic.Clear();
            // if we don't do this, it'll add them all to whatever list is currently registered
            FairyTarget.registered = new Il2CppSystem.Collections.Generic.List<FairyTarget>();
            foreach (FairyTarget FairyTarget in Resources.FindObjectsOfTypeAll<FairyTarget>()) {
                GameObject.Destroy(FairyTarget);
            }

            if (ItemLookup.ItemList.Count > 0 || Locations.RandomizedLocations.Count > 0) {
                Dictionary<string, Check> Checks = TunicUtils.GetAllInUseChecksDictionary();
                List<string> ItemIdsInScene = Checks.Values.Where(check => check.Location.SceneName == SceneManager.GetActiveScene().name 
                && !TunicUtils.IsCheckCompletedOrCollected(check.CheckId)).Select(check => check.CheckId).ToList();

                List<SmashableObject> breakableObjects = new List<SmashableObject>();
                if (SaveFile.GetInt(SaveFlags.BreakableShuffleEnabled) == 1) {
                    breakableObjects = GameObject.FindObjectsOfType<SmashableObject>().ToList();
                }


                foreach (string ItemId in ItemIdsInScene) {
                    bool isBreakable = BreakableShuffle.BreakableChecks.ContainsKey(ItemId);
                    Location Location = Checks[ItemId].Location;

                    if (GameObject.Find($"fairy target {ItemId}") == null) {
                        FairyTarget fairyTarget = CreateFairyTarget($"fairy target {ItemId}", StringToVector3(Location.Position));
                        ItemTargets.Add(fairyTarget);
                        if (TunicUtils.ChecksInLogic.Contains(ItemId)) {
                            ItemTargetsInLogic.Add(fairyTarget);
                        }
                        if (isBreakable) {
                            foreach (SmashableObject breakable in breakableObjects) {
                                if (BreakableShuffle.getBreakableGameObjectId(breakable.gameObject) == ItemId) {
                                    fairyTarget.transform.parent = breakable.transform;
                                }
                            }
                        }
                    }
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
                        FairyTarget fairyTarget = CreateFairyTarget($"fairy target Well Reward ({CoinsNeededForNextReward} Coins) [Trinket Well]", GameObject.FindObjectOfType<TrinketWell>().transform.position);
                        ItemTargets.Add(fairyTarget);
                        ItemTargetsInLogic.Add(fairyTarget);
                    }
                }
                CreateEntranceTargets();
                CreateLoadZoneTargets();
                CreateLogicLoadZoneTargets();
                ChooseFairyTargetList();
            }
        }

        public static void CreateLoadZoneTargets() {
            HashSet<string> ScenesWithItems = new HashSet<string>();
            Dictionary<string, Check> Checks = TunicUtils.GetAllInUseChecksDictionary();
            List<string> ItemIds = Checks.Values.Where(check => check.Location.SceneName == SceneManager.GetActiveScene().name
            && !TunicUtils.IsCheckCompletedOrCollected(check.CheckId)).Select(check => check.CheckId).ToList();

            foreach (string ItemId in ItemIds) {
                ScenesWithItems.Add(Checks[ItemId].Location.SceneName);
            }

            foreach (ScenePortal ScenePortal in Resources.FindObjectsOfTypeAll<ScenePortal>()) {
                if (ScenePortal.id.Contains("customfasttravel")) { continue; }
                string sceneName = ERScripts.FindPairedPortalSceneFromName(ScenePortal.name);
                if (ScenesWithItems.Contains(sceneName)) {
                    FairyTarget fairyTarget = CreateFairyTarget($"fairy target {ScenePortal.name}", ScenePortal.transform.position);
                    AdjItemTargets.Add(fairyTarget);
                }
            }
        }

        // specifically for fairy seeking spell with logic
        public static void CreateLogicLoadZoneTargets() {
            foreach (ScenePortal ScenePortal in Resources.FindObjectsOfTypeAll<ScenePortal>()) {
                string portalRegion = ERScripts.FindPortalRegionFromName(ScenePortal.name);
                string destScene = ERScripts.FindPairedPortalSceneFromName(ScenePortal.name);
                // check if the entrance is logically accessible and if the adjacent scene has checks in logic
                if (TunicUtils.PlayerItemsAndRegions.ContainsKey(portalRegion) && TunicUtils.ChecksInLogicPerScene[destScene].Count > 0) {
                    AdjItemTargetsInLogic.Add(CreateFairyTarget($"alt target {ScenePortal.name}", ScenePortal.transform.position));
                }
            }
        }

        public static void CreateEntranceTargets() {
            foreach (ScenePortal ScenePortal in Resources.FindObjectsOfTypeAll<ScenePortal>()) {
                if (ScenePortal.id.Contains("customfasttravel")) { continue; }
                if (ScenePortal.isActiveAndEnabled && SaveFile.GetInt("randomizer entered portal " + ScenePortal.name) != 1) {
                    FairyTarget fairyTarget = CreateFairyTarget($"entrance target {ScenePortal.name}", ScenePortal.transform.position);
                    EntranceTargets.Add(fairyTarget);
                    if (TunicUtils.PlayerItemsAndRegions.ContainsKey(ERScripts.FindPortalRegionFromName(ScenePortal.name))) {
                        EntranceTargetsInLogic.Add(fairyTarget);
                    }
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


        private static Vector3 StringToVector3(string Position) {
            Position = Position.Replace("(", "").Replace(")", "");
            string[] coords = Position.Split(',');
            Vector3 vector = new Vector3(float.Parse(coords[0], CultureInfo.InvariantCulture), float.Parse(coords[1], CultureInfo.InvariantCulture), float.Parse(coords[2], CultureInfo.InvariantCulture));
            return vector;
        }


        public static void ChooseFairyTargetList() {
            Il2CppSystem.Collections.Generic.List<FairyTarget> targets = new Il2CppSystem.Collections.Generic.List<FairyTarget>();
            if (TunicRandomizer.Settings.SeekingSpellLogic) {
                if (ItemTargetsInLogic.Count > 0) {
                    targets = ItemTargetsInLogic;
                } else {
                    targets = AdjItemTargetsInLogic;
                }
            } else {
                if (ItemTargets.Count > 0) {
                    targets = ItemTargets;
                } else {
                    targets = AdjItemTargets;
                }
            }
            FairyTarget.registered = CleanList(targets);
        }
        private static Il2CppSystem.Collections.Generic.List<FairyTarget> CleanList(Il2CppSystem.Collections.Generic.List<FairyTarget> list) {
            foreach (var item in list) { 
                if (item == null) {
                    list.Remove(item);
                }
            }
            return list;
        }

        // update what targets are in logic based on the item that was received, or just updates which list to use
        public static void UpdateFairyTargetsInLogic(string newItem = "n/a") {
            if (newItem != "n/a") {
                // convert display name to internal name
                foreach (KeyValuePair<string, string> namePair in ItemLookup.SimplifiedItemNames) {
                    if (namePair.Value == newItem) {
                        newItem = namePair.Key;
                    }
                }
                if (ItemLookup.LegacyMajorItems.Contains(newItem) || newItem.StartsWith("Ladder")) {
                    // add the new item received to the items the player has
                    TunicUtils.AddStringToDict(TunicUtils.PlayerItemsAndRegions, newItem);
                    TunicUtils.UpdateChecksInLogic();
                    // loop through the regular ItemTargets, find ones that are newly in logic
                    foreach (FairyTarget fairyTarget in ItemTargets) {
                        if (fairyTarget == null || !fairyTarget.isActiveAndEnabled) { continue; }
                        string targetName = fairyTarget.name.Replace("fairy target ", "");
                        if (!ItemTargetsInLogic.Contains(fairyTarget) && TunicUtils.ChecksInLogic.Contains(targetName)) {
                            ItemTargetsInLogic.Add(fairyTarget);
                        }
                    }
                    foreach (FairyTarget fairyTarget in AdjItemTargets) {
                        if (fairyTarget == null || !fairyTarget.isActiveAndEnabled) { continue; }
                        if (!AdjItemTargetsInLogic.Contains(fairyTarget)) {
                            // these are portal names
                            string targetName = fairyTarget.name.Replace("fairy target ", "");
                            string regionName = ERScripts.FindPortalRegionFromName(targetName);
                            string destSceneName = ERScripts.FindPairedPortalSceneFromName(targetName);
                            if (TunicUtils.PlayerItemsAndRegions.ContainsKey(regionName) && TunicUtils.ChecksInLogicPerScene[destSceneName].Count > 0) {
                                AdjItemTargetsInLogic.Add(fairyTarget);
                            }
                        }
                    }

                    // loop through the regular EntranceTargets, find ones that are newly in logic
                    foreach (FairyTarget fairyTarget in EntranceTargets) {
                        if (fairyTarget == null || !fairyTarget.isActiveAndEnabled) { continue; }
                        if (!EntranceTargetsInLogic.Contains(fairyTarget)
                                && TunicUtils.PlayerItemsAndRegions.ContainsKey(ERScripts.FindPortalRegionFromName(fairyTarget.name.Replace("entrance target ", "")))) {
                            EntranceTargetsInLogic.Add(fairyTarget);
                        }
                    }
                }
            }
            ChooseFairyTargetList();
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
            var storedList = FairyTarget.registered;
            if (TunicRandomizer.Settings.SeekingSpellLogic) {
                FairyTarget.registered = FairyTargets.EntranceTargetsInLogic;
            } else {
                FairyTarget.registered = FairyTargets.EntranceTargets;
            }
            PlayerCharacter.instance.GetComponent<FairySpell>().SpellEffect();
            FairyTarget.registered = storedList;
        }
    }
}
