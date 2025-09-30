using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TunicRandomizer.ERScripts;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {
    public class FoxPrince {
        public static ScenePortal CurrentPortal = null;
        public static Collider FoxCollider = null;
        public static List<PortalCombo> FPRandomizedPortals = new List<PortalCombo>();
        // when you choose a portal, that choice showed up because there was a successful entrance rando generation that lead to that one
        // so, we might as well save that old one for the next time you go into an entrance
        public static Dictionary<PortalCombo, List<PortalCombo>> CachePairingDict = new Dictionary<PortalCombo, List<PortalCombo>>();
        public static List<PortalCombo> CachedSuccessfulPairing = null;
        // so we aren't just reading them off the save file every time
        // this might be overengineering it though, idk
        public static Dictionary<string, string> CachedPlandoPortals = new Dictionary<string, string>();

        // for use with the pin system
        public static string PinnedPortal = "";

        // flag to tell ModifyPortals to update the sign displays
        public static bool UpdateSignsFlag = false;

        public static void Setup() {
            EntranceSelector.CreateEntranceSelector();
            ItemPresentationPatches.SetupFoxPrinceItemPresentations();
        }

        public static bool ScenePortal_OnTriggerEnter_PrefixPatch(ScenePortal __instance, Collider c) {
            // the collider here is the fox's UnityEngine.CapsuleCollider
            if (!GetBool(FoxPrinceEnabled)) return true;
            if (c.transform.name != "_Fox(Clone)") return true;
            // skip the custom portals
            if (__instance.id.Contains("customfasttravel")) return true;
            // skip the easter egg scenes
            if (SceneLoaderPatches.SceneName == "Crypt" || SceneLoaderPatches.SceneName == "Quarry") return true;
            // skip zig skip too
            if (__instance.id == "zig2_skip") return true;
            if (SaveFile.GetString($"{FPChosenPortalPrefix} {__instance.name}") == "") {
                CurrentPortal = __instance;
                FoxCollider = c;
                List<PortalCombo> portalChoices = FPGetThreePortals(SaveFile.GetInt("seed"), __instance.name);
                TunicLogger.LogInfo("portal choices below");
                foreach (var portalChoice in portalChoices) {
                    TunicLogger.LogInfo(portalChoice.Portal2.Name);
                }

                FPChoosePortal(portalChoices);
                return false;
            }
            return true;
        }


        public static void FPChoosePortal(List<PortalCombo> portalChoices) {
            EntranceSelector.instance.ShowSelection(portalChoices);
        }


        public static List<PortalCombo> FPGetThreePortals(int seed, string currentPortalName, List<PortalCombo> excludedPortals = null) {
            TunicLogger.LogInfo("starting FPGetThreePortals");
            List<PortalCombo> portalChoices = new List<PortalCombo>();
            List<Tuple<string, string>> deplando = new List<Tuple<string, string>>();

            void updateDeplando(string pname1, string pname2) {
                deplando.Add(new Tuple<string, string>(pname1, pname2));
                if (!GetBool(Decoupled)) {
                    deplando.Add(new Tuple<string, string>(pname2, pname1));
                }
            }
            
            // todo: the cache seems probably unnecessary
            Dictionary<string, string> plando = new Dictionary<string, string>();
            if (CachedPlandoPortals.Count > 0) {
                plando = CachedPlandoPortals;
            } else {
                foreach (string key in SaveFile.stringStore.Keys) {
                    if (key.StartsWith(FPChosenPortalPrefix)) {
                        string origin = key.Substring($"{FPChosenPortalPrefix} ".Length);
                        string destination = SaveFile.stringStore[key];
                        if (ItemRandomizer.InitialRandomizationDone) {
                            // this is to avoid duplicates being made later on
                            if (!GetBool(Decoupled) && plando.ContainsKey(destination)) {
                                continue;
                            }
                            plando.Add(origin, destination);
                        }
                    }
                }
                CachedPlandoPortals = plando;
            }

            // check if there's a pinned portal, if there is check if it's a valid direction pair if direction pairs, and check if it's been rerolled from already
            // if the pinned portal is valid for the current choice, then it will be the first choice
            if (PinnedPortal != ""
                && (!GetBool(PortalDirectionPairs) || TunicUtils.VerifyDirectionPair(currentPortalName, PinnedPortal))
                && excludedPortals == null) {
                // todo: this is duplicate of below, could probably deduplicate it
                int maxCount = 100;
                int curCount = 0;
                Dictionary<string, string> pinPlando = new Dictionary<string, string>(plando) { {currentPortalName, PinnedPortal} };
                while (true) {
                    if (curCount >= maxCount) { break; }
                    curCount++;
                    List<PortalCombo> randomizedPortals = RandomizePortals(seed + curCount, pinPlando, deplando, canFail: true);
                    if (randomizedPortals == null) { continue; }
                    PortalCombo newPortalCombo = TunicUtils.GetPortalComboFromRandomizedPortals(currentPortalName, randomizedPortals);
                    portalChoices.Add(newPortalCombo);
                    CachePairingDict.Add(newPortalCombo, randomizedPortals);
                    updateDeplando(currentPortalName, PinnedPortal);
                    break;
                }
            }

            if (CachedSuccessfulPairing != null) {
                PortalCombo cachedPortalCombo = TunicUtils.GetPortalComboFromRandomizedPortals(currentPortalName, CachedSuccessfulPairing);
                portalChoices.Add(cachedPortalCombo);
                updateDeplando(currentPortalName, cachedPortalCombo.Portal2.Name);
                CachePairingDict.Add(cachedPortalCombo, new List<PortalCombo>(CachedSuccessfulPairing));
                CachedSuccessfulPairing = null;
            }

            if (excludedPortals != null) {
                foreach (PortalCombo portalCombo in excludedPortals) {
                    updateDeplando(portalCombo.Portal1.Name, portalCombo.Portal2.Name);
                }
            }

            // as portals get chosen, set the contents of PlandoPortals, and reload from the save file or somewhere when needed
            // we want to fine tune this to try to get 3 different portals when possible, but not take overly long if there aren't 3+ possibilities
            int maxTrialCount = 1000;
            int trialCount = 0;
            while (portalChoices.Count < 3) {
                if (portalChoices.Count() == 3) {
                    break;
                }
                if (trialCount >= maxTrialCount && portalChoices.Count > 0) {
                    // we've done enough trials to say that we probably won't find any more connections, so it's time to give the player less than 3 choices
                    // if we don't have any choices yet, keep trying -- if it's failing, we'd wanna know that, but maybe it's just something really restrictive and weird
                    break;
                }
                trialCount++;
                TunicLogger.LogInfo($"Current trial: {trialCount}");
                List<PortalCombo> randomizedPortals = RandomizePortals(seed + trialCount, plando, deplando, canFail: true);
                if (randomizedPortals == null) {
                    // this means the generation was not successful, which is fine and intended to happen, especially with restrictive logic
                    continue;
                }

                PortalCombo newPortalCombo = TunicUtils.GetPortalComboFromRandomizedPortals(currentPortalName, randomizedPortals);
                portalChoices.Add(newPortalCombo);
                CachePairingDict.Add(newPortalCombo, randomizedPortals);

                TunicLogger.LogInfo("portal choice is " + newPortalCombo.Portal2.Name);

                // todo: remove this later when confident that it's not going to be a problem
                TunicLogger.LogInfo($"Starting check all reachable in trials for {newPortalCombo.Portal2.Name}");
                TunicUtils.CheckAllLocsReachable(randomizedPortals);

                updateDeplando(currentPortalName, newPortalCombo.Portal2.Name);
            }
            TunicLogger.LogInfo("returning portal choices");
            return portalChoices;
        }

        public static void FPPortalChosen(PortalCombo portalCombo) {
            TunicLogger.LogInfo("FPPortalChosen started");
            if (CachePairingDict.ContainsKey(portalCombo)) {
                CachedSuccessfulPairing = CachePairingDict[portalCombo];
            }
            CachePairingDict.Clear();
            Portal originPortal = portalCombo.Portal1;
            Portal destinationPortal = portalCombo.Portal2;
            SaveFile.SetString($"{FPChosenPortalPrefix} {CurrentPortal.name}", destinationPortal.Name);
            if (!GetBool(Decoupled)) {
                SaveFile.SetString($"{FPChosenPortalPrefix} {destinationPortal.Name}", CurrentPortal.name);
            }

            FPRandomizedPortals.Add(new PortalCombo(originPortal, destinationPortal));
            if (!GetBool(Decoupled)) {
                FPRandomizedPortals.Add(new PortalCombo(destinationPortal, originPortal));
            }
            CachedPlandoPortals.Add(originPortal.Name, destinationPortal.Name);
            CurrentPortal.destinationSceneName = destinationPortal.Scene;
            CurrentPortal.id = portalCombo.ComboTag + portalCombo.ComboTag;
            CurrentPortal.optionalIDToSpawnAt = portalCombo.ComboTag;
            CurrentPortal.OnTriggerEnter(FoxCollider);
            if (Hints.PortalToSignName.ContainsKey(portalCombo.Portal1.SceneDestinationTag) || Hints.PortalToSignName.ContainsKey(portalCombo.Portal2.SceneDestinationTag)) {
                UpdateSignsFlag = true;
            }

            if (portalCombo.Portal2.Name == PinnedPortal) {
                SaveFile.SetString(SaveFlags.FPPinnedPortalFlag, "");
                PinnedPortal = "";
            }

            TunicLogger.LogInfo("FPPortalChosen done");
        }

        public static void ClearFoxPrinceCaches() {
            FPRandomizedPortals.Clear();
            CachedSuccessfulPairing = null;
            CachedPlandoPortals.Clear();
        }

        public static void CreateFoxPrinceItems() {
            Item SoulDice = ScriptableObject.CreateInstance<Item>();
            SoulDice.name = "Soul Dice";
            SoulDice.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
            SoulDice.collectionMessage.text = $"dE twehntE!";
            SoulDice.controlAction = "";

            // TODO maybe think of a better name for this item lol
            Item Dart = Inventory.GetItemByName("Dart");
            Dart.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
            Dart.collectionMessage.text = "boulzI!";
        
            Inventory.itemList.Add(SoulDice);
        }
    }
}
