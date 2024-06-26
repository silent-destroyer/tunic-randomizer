using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using Archipelago.MultiClient.Net;
using UnityEngine.Diagnostics;
using Newtonsoft.Json;
using System.Diagnostics.Eventing.Reader;

namespace TunicRandomizer {
    public class FairyTargets {
        public static Il2CppSystem.Collections.Generic.List<FairyTarget> EntranceTargets = new Il2CppSystem.Collections.Generic.List<FairyTarget> { };
        public static Il2CppSystem.Collections.Generic.List<FairyTarget> EntranceTargetsInLogic = new Il2CppSystem.Collections.Generic.List<FairyTarget> { };
        public static Il2CppSystem.Collections.Generic.List<FairyTarget> ItemTargets = new Il2CppSystem.Collections.Generic.List<FairyTarget> { };
        public static Il2CppSystem.Collections.Generic.List<FairyTarget> ItemTargetsInLogic = new Il2CppSystem.Collections.Generic.List<FairyTarget> { };
        // list of the CheckIds for checks that are currently in logic
        public static List<string> ChecksInLogic = new List<string>();
        // progression items the player has received
        public static Dictionary<string, int> PlayerItemsAndRegions = new Dictionary<string, int>();

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

                        if (ChecksInLogic.Contains(ItemId)) {
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
        public static void CreateLogicLoadZoneTargets() {
            foreach (ScenePortal ScenePortal in Resources.FindObjectsOfTypeAll<ScenePortal>()) {
                string destScene = TunicPortals.FindPairedPortalSceneFromName(ScenePortal.name);
                foreach (string CheckId in ChecksInLogic) {
                    if (CheckId.Contains(destScene)) {
                        CreateFairyTarget($"alt target {ScenePortal.name}", ScenePortal.transform.position);
                        break;
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

        private static void CreateFairyTarget(string Name, Vector3 Position) {
            GameObject FairyTarget = new GameObject(Name);
            FairyTarget.SetActive(true);
            FairyTarget.AddComponent<FairyTarget>();
            FairyTarget.GetComponent<FairyTarget>().stateVariable = StateVariable.GetStateVariableByName("false");
            FairyTarget.transform.position = Position;
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
                        if (PlayerItemsAndRegions.ContainsKey(TunicPortals.FindPortalRegionFromName(fairyTarget.name.Replace("entrance target ", "")))) {
                            EntranceTargetsInLogic.Add(fairyTarget);
                        }
                    } else if (fairyTarget.name.StartsWith("fairy")) {
                        ItemTargets.Add(fairyTarget);
                        string targetName = fairyTarget.name.Replace("fairy target ", "");
                        if (ChecksInLogic.Contains(targetName)) {
                            ItemTargetsInLogic.Add(fairyTarget);
                        } else {
                            // for adjacent scenes, check if the region the portal leads to is in logic, and check if the scene has items in logic
                            string regionName = TunicPortals.FindPortalRegionFromName(targetName);
                            string destSceneName = TunicPortals.FindPairedPortalSceneFromName(targetName);
                            if (PlayerItemsAndRegions.ContainsKey(regionName)) {
                                foreach (string checkId in ChecksInLogic) {
                                    if (checkId.Contains(destSceneName)) {
                                        ItemTargetsInLogic.Add(fairyTarget);
                                        break;
                                    }
                                }
                            }
                        }
                    } else if (fairyTarget.name.StartsWith("alt")) {
                        ItemTargetsInLogic.Add(fairyTarget);
                    }
                }
            }

            // todo: make an option
            FairyTarget.registered = ItemTargetsInLogic;
        }

        private static Vector3 StringToVector3(string Position) {
            Position = Position.Replace("(", "").Replace(")", "");
            string[] coords = Position.Split(',');
            Vector3 vector = new Vector3(float.Parse(coords[0], CultureInfo.InvariantCulture), float.Parse(coords[1], CultureInfo.InvariantCulture), float.Parse(coords[2], CultureInfo.InvariantCulture));
            return vector;
        }

        // sets ChecksInLogic to contain a list of CheckIds for all checks that are currently in logic with the items you have received
        public static void FindChecksInLogic() {
            PlayerItemsAndRegions.Clear();
            ChecksInLogic.Clear();

            TunicUtils.AddListToDict(PlayerItemsAndRegions, ItemRandomizer.PrecollectedItems);
            PlayerItemsAndRegions.Add("Overworld", 1);

            if (SaveFlags.IsArchipelago()) {
                TunicUtils.AddDictToDict(PlayerItemsAndRegions, Archipelago.instance.integration.GetStartInventory());
                foreach (var itemInfo in Archipelago.instance.integration.session.Items.AllItemsReceived) {
                    string itemName = itemInfo.ItemName;
                    // convert display name to internal name
                    foreach (KeyValuePair<string, string> namePair in ItemLookup.SimplifiedItemNames) {
                        if (namePair.Value == itemName) {
                            itemName = namePair.Key;
                        }
                    }
                    TunicUtils.AddStringToDict(PlayerItemsAndRegions, itemName);
                }
            } else {
                foreach (Check locationCheck in Locations.RandomizedLocations.Values) {
                    if (Locations.CheckedLocations.ContainsKey(locationCheck.CheckId) && Locations.CheckedLocations[locationCheck.CheckId] == true) {
                        TunicUtils.AddStringToDict(PlayerItemsAndRegions, locationCheck.Reward.Name);
                    }
                }
            }
            UpdateChecksInLogic();
        }

        public static void UpdateChecksInLogic() {
            ItemRandomizer.GetReachableRegions(PlayerItemsAndRegions);
            foreach (Check check in Locations.VanillaLocations.Values) {
                if (!ChecksInLogic.Contains(check.CheckId) && check.Location.reachable(PlayerItemsAndRegions) && Locations.CheckedLocations[check.CheckId] == false) {
                    ChecksInLogic.Add(check.CheckId);
                }
            }
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
            TunicUtils.AddStringToDict(PlayerItemsAndRegions, newItem);
            UpdateChecksInLogic();
            // loop through the regular ItemTargets, find ones that are newly in logic
            foreach (FairyTarget fairyTarget in ItemTargets) {
                if (fairyTarget == null || !fairyTarget.isActiveAndEnabled) { continue; }
                string targetName = fairyTarget.name.Replace("fairy target ", "");
                if (!ItemTargetsInLogic.Contains(fairyTarget)) {
                    if (ChecksInLogic.Contains(targetName)) {
                        ItemTargetsInLogic.Add(fairyTarget);
                    } else {
                        // for adjacent scenes, check if the region the portal leads to is in logic, and check if the scene has items in logic
                        string regionName = TunicPortals.FindPortalRegionFromName(targetName);
                        string destSceneName = TunicPortals.FindPairedPortalSceneFromName(targetName);
                        if (PlayerItemsAndRegions.ContainsKey(regionName)) {
                            foreach (string checkId in ChecksInLogic) {
                                if (checkId.Contains(destSceneName)) {
                                    ItemTargetsInLogic.Add(fairyTarget);
                                }
                            }
                        }
                    }
                }
            }
            // loop through the regular EntranceTargets, find ones that are newly in logic
            foreach (FairyTarget fairyTarget in EntranceTargets) {
                if (fairyTarget == null || !fairyTarget.isActiveAndEnabled) { continue; }
                if (PlayerItemsAndRegions.ContainsKey(TunicPortals.FindPortalRegionFromName(fairyTarget.name.Replace("entrance target ", "")))
                        && !EntranceTargetsInLogic.Contains(fairyTarget)) {
                    EntranceTargetsInLogic.Add(fairyTarget);
                }
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
            // todo: check options, if it lags then instead make the list that matters based on the logic
            FairyTarget.registered = FairyTargets.EntranceTargetsInLogic;
            PlayerCharacter.instance.GetComponent<FairySpell>().SpellEffect();
            FairyTarget.registered = FairyTargets.ItemTargetsInLogic;
        }
    }
}
