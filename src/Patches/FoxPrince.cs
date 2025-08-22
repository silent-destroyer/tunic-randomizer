using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TunicRandomizer.ERScripts;
using static TunicRandomizer.ERData;
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
                List<Tuple<Portal, Portal>> portalChoices = BPGetThreePortals(SaveFile.GetInt("seed"), __instance.name);
                TunicLogger.LogInfo("portal choices below");
                foreach (var portalChoice in portalChoices) {
                    TunicLogger.LogInfo(portalChoice.Item2.Name);
                }

                // user input goes here somehow??
                BPChoosePortal(portalChoices);
                return false;
            }
            TunicLogger.LogInfo("returning true");
            return true;
        }


        public static void BPChoosePortal(List<Tuple<Portal, Portal>> portalChoices) {
            // do something here to actually choose a portal
            BPPortalChosen(portalChoices[0].Item1, portalChoices[0].Item2);
        }


        public static List<Tuple<Portal, Portal>> BPGetThreePortals(int seed, string currentPortalName) {
            TunicLogger.LogInfo("starting BPGetThreePortals");
            List<Tuple<Portal, Portal>> portalChoices = new List<Tuple<Portal, Portal>>();
            List<Tuple<string, string>> deplando = new List<Tuple<string, string>>();
            // as portals get chosen, set the contents of PlandoPortals, and reload from the save file or somewhere when needed
            // we want to fine tune this to try to get 3 different portals when possible, but not take overly long if there aren't 3+ possibilities
            int trialCount = 10;
            for (int i = 0; i < trialCount; i++) {
                TunicLogger.LogInfo("randomizing portals in BPGetThreePortals");
                List<PortalCombo> randomizedPortals = RandomizePortals(seed + i, deplando);
                TunicLogger.LogInfo("randomized portals in BPGetThreePortals");
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
                if (destinationPortal == null) {
                    TunicLogger.LogError("Error in getting portal name");
                }
                portalChoices.Add(new Tuple<Portal, Portal>(originPortal, destinationPortal));
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

        public static void BPPortalChosen(Portal originPortal, Portal destinationPortal) {
            TunicLogger.LogInfo("BPPortalChosen started");
            SaveFile.SetString($"randomizer bp {CurrentPortal.name}", destinationPortal.Name);
            if (!GetBool(Decoupled)) {
                SaveFile.SetString($"randomizer bp {destinationPortal.Name}", CurrentPortal.name);
            }
            string comboTag = $"{CurrentPortal.name}--{destinationPortal.Name}";  // to match a portal combo's combo tag
            BPRandomizedPortals.Add(new PortalCombo(originPortal, destinationPortal));
            if (!GetBool(Decoupled)) {
                BPRandomizedPortals.Add(new PortalCombo(destinationPortal, originPortal));
            }
            PlandoPortals.Add(originPortal.Name, destinationPortal.Name);
            CurrentPortal.destinationSceneName = destinationPortal.Scene;
            CurrentPortal.id = comboTag + comboTag;
            CurrentPortal.optionalIDToSpawnAt = comboTag;
            CurrentPortal.OnTriggerEnter(FoxCollider);
            TunicLogger.LogInfo("BPPortalChosen done");
        }

    }
}
