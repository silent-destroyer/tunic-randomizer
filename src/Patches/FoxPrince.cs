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
        public static List<PortalCombo> BPRandomizedPortals = new List<PortalCombo>();
        // when you choose a portal, that choice showed up because there was a successful entrance rando generation that lead to that one
        // so, we might as well save that old one for the next time you go into an entrance
        public static Dictionary<PortalCombo, List<PortalCombo>> CachePairingDict = new Dictionary<PortalCombo, List<PortalCombo>>();
        public static List<PortalCombo> CachedSuccessfulPairing = null;

        // flag to tell ModifyPortals to update the sign displays
        public static bool UpdateSignsFlag = false;

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
            if (SaveFile.GetString($"randomizer bp {__instance.name}") == "") {
                CurrentPortal = __instance;
                FoxCollider = c;
                List<PortalCombo> portalChoices = BPGetThreePortals(SaveFile.GetInt("seed"), __instance.name);
                TunicLogger.LogInfo("portal choices below");
                foreach (var portalChoice in portalChoices) {
                    TunicLogger.LogInfo(portalChoice.Portal2.Name);
                }

                // user input goes here somehow??
                BPChoosePortal(portalChoices);
                return false;
            }
            TunicLogger.LogInfo("returning true");
            return true;
        }


        public static void BPChoosePortal(List<PortalCombo> portalChoices) {
            EntranceSelector.instance.ShowSelection(portalChoices);
        }


        public static List<PortalCombo> BPGetThreePortals(int seed, string currentPortalName, List<PortalCombo> excludedPortals = null) {
            TunicLogger.LogInfo("starting BPGetThreePortals");
            List<PortalCombo> portalChoices = new List<PortalCombo>();
            List<Tuple<string, string>> deplando = new List<Tuple<string, string>>();
            if (CachedSuccessfulPairing != null) {
                PortalCombo cachedPortalCombo = TunicUtils.GetPortalComboFromRandomizedPortals(currentPortalName, CachedSuccessfulPairing);
                portalChoices.Add(cachedPortalCombo);
                deplando.Add(new Tuple<string, string>(currentPortalName, cachedPortalCombo.Portal2.Name));
                if (!GetBool(Decoupled)) {
                    deplando.Add(new Tuple<string, string>(cachedPortalCombo.Portal2.Name, currentPortalName));
                }
                CachePairingDict.Add(cachedPortalCombo, new List<PortalCombo>(CachedSuccessfulPairing));
                CachedSuccessfulPairing = null;
            }
            if (excludedPortals != null) {
                foreach (PortalCombo portalCombo in excludedPortals) {
                    deplando.Add(new Tuple<string, string>(portalCombo.Portal1.Name, portalCombo.Portal2.Name));
                }
            }
            // as portals get chosen, set the contents of PlandoPortals, and reload from the save file or somewhere when needed
            // we want to fine tune this to try to get 3 different portals when possible, but not take overly long if there aren't 3+ possibilities
            int maxTrialCount = 1000;
            int trialCount = 0;
            while (portalChoices.Count < 3) {
                if (trialCount >= maxTrialCount && portalChoices.Count > 0) {
                    // we've done enough trials to say that we probably won't find any more connections, so it's time to give the player less than 3 choices
                    // if we don't have any choices yet, keep trying -- if it's failing, we'd wanna know that, but maybe it's just something really restrictive and weird
                    break;
                }
                trialCount++;
                TunicLogger.LogInfo($"Current trial: {trialCount}");
                List<PortalCombo> randomizedPortals = RandomizePortals(seed + trialCount, deplando, canFail: true);
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

                if (portalChoices.Count() == 3) {
                    break;
                }
                deplando.Add(new Tuple<string, string>(currentPortalName, newPortalCombo.Portal2.Name));
                if (!GetBool(Decoupled)) {
                    deplando.Add(new Tuple<string, string>(newPortalCombo.Portal2.Name, currentPortalName));
                }
            }
            TunicLogger.LogInfo("returning portal choices");
            return portalChoices;
        }

        public static void BPTestDialogue() {
            TunicLogger.LogInfo("test start");
            GameObject testGUI = GameObject.Instantiate(GameObject.Find("_GameGUI(Clone)"));
            TunicLogger.LogInfo("test 1");
            GameObject testCanvas = testGUI.transform.GetChild(3).gameObject;
            TunicLogger.LogInfo("test 2");
            GameObject testPanel = testCanvas.transform.GetChild(0).gameObject;
            TunicLogger.LogInfo("test 3");
            GameObject buttonRow = testPanel.transform.GetChild(1).gameObject;
            TunicLogger.LogInfo("test 4");
            GameObject newButton = GameObject.Instantiate(buttonRow.transform.GetChild(0).gameObject, buttonRow.transform);
            TunicLogger.LogInfo("test 5");
            GameObject buttonTextObj = newButton.transform.GetChild(0).gameObject;
            TunicLogger.LogInfo("test 6");
            buttonTextObj.GetComponent<RTLTMPro.RTLTextMeshPro>().originalText = "words";
            TunicLogger.LogInfo("test 7");
            buttonTextObj.GetComponent<RTLTMPro.RTLTextMeshPro>().text = "words2";
            TunicLogger.LogInfo("test 8");
            testCanvas.SetActive(true);
            TunicLogger.LogInfo("test end");
        }

        public static void BPPortalChosen(PortalCombo portalCombo) {
            TunicLogger.LogInfo("BPPortalChosen started");
            if (CachePairingDict.ContainsKey(portalCombo)) {
                CachedSuccessfulPairing = CachePairingDict[portalCombo];
            }
            CachePairingDict.Clear();
            Portal originPortal = portalCombo.Portal1;
            Portal destinationPortal = portalCombo.Portal2;
            SaveFile.SetString($"randomizer bp {CurrentPortal.name}", destinationPortal.Name);
            if (!GetBool(Decoupled)) {
                SaveFile.SetString($"randomizer bp {destinationPortal.Name}", CurrentPortal.name);
            }

            BPRandomizedPortals.Add(new PortalCombo(originPortal, destinationPortal));
            if (!GetBool(Decoupled)) {
                BPRandomizedPortals.Add(new PortalCombo(destinationPortal, originPortal));
            }
            PlandoPortals.Add(originPortal.Name, destinationPortal.Name);
            CurrentPortal.destinationSceneName = destinationPortal.Scene;
            CurrentPortal.id = portalCombo.ComboTag + portalCombo.ComboTag;
            CurrentPortal.optionalIDToSpawnAt = portalCombo.ComboTag;
            CurrentPortal.OnTriggerEnter(FoxCollider);
            if (Hints.PortalToSignName.ContainsKey(portalCombo.Portal1.SceneDestinationTag) || Hints.PortalToSignName.ContainsKey(portalCombo.Portal2.SceneDestinationTag)) {
                UpdateSignsFlag = true;
            }
            TunicLogger.LogInfo("BPPortalChosen done");
        }

        public static void CreateFoxPrinceItems() {
            Item SoulDice = ScriptableObject.CreateInstance<Item>();
            SoulDice.name = "Soul Dice";
            SoulDice.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
            SoulDice.collectionMessage.text = $"dE twehntE!";
            SoulDice.controlAction = "";

            Inventory.itemList.Add(SoulDice);

            ItemPresentationPatches.SetupFoxPrinceItemPresentations();
        }

    }
}
