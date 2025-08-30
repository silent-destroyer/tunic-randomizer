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

        public static bool ScenePortal_OnTriggerEnter_PrefixPatch(ScenePortal __instance, Collider c) {
            // the collider here is the fox's UnityEngine.CapsuleCollider
            if (!GetBool(FoxPrinceEnabled)) return true;
            if (c.transform.name != "_Fox(Clone)") return true;
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
            // do something here to actually choose a portal
            BPPortalChosen(portalChoices[0]);
        }


        public static List<PortalCombo> BPGetThreePortals(int seed, string currentPortalName) {
            TunicLogger.LogInfo("starting BPGetThreePortals");
            List<PortalCombo> portalChoices = new List<PortalCombo>();
            List<Tuple<string, string>> deplando = new List<Tuple<string, string>>();
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
                    // this means it did not find a portal to match it with
                    // hopefully this means it couldn't find one and there wasn't some other generation error
                    continue;
                }
                Portal originPortal = null;
                Portal destinationPortal = null;
                foreach (PortalCombo portalCombo in randomizedPortals) {
                    if (portalCombo.Portal1.Name == currentPortalName) {
                        originPortal = portalCombo.Portal1;
                        destinationPortal = portalCombo.Portal2;
                        break;
                    }
                    if (!GetBool(Decoupled) && portalCombo.Portal2.Name == currentPortalName) {
                        originPortal = portalCombo.Portal2;
                        destinationPortal = portalCombo.Portal1;
                        break;
                    }
                }
                // remove this later if it doesn't ever actually show up
                if (destinationPortal == null) {
                    TunicLogger.LogError("Error in getting portal name in BPGetThreePortals");
                }
                portalChoices.Add(new PortalCombo(originPortal, destinationPortal));
                TunicLogger.LogInfo("portal choice is " + destinationPortal.Name);
                if (portalChoices.Count() == 3) {
                    break;
                }
                deplando.Add(new Tuple<string, string>(currentPortalName, destinationPortal.Name));
                if (!GetBool(Decoupled)) {
                    deplando.Add(new Tuple<string, string>(destinationPortal.Name, currentPortalName));
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
            TunicLogger.LogInfo("BPPortalChosen done");
        }

    }
}
