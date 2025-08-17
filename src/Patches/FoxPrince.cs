using System;
using System.Collections.Generic;
using System.Linq;
using static TunicRandomizer.ERScripts;
using static TunicRandomizer.ERData;
using UnityEngine;

namespace TunicRandomizer {
    public class FoxPrince {

        public static bool ScenePortal_OnTriggerEnter_PrefixPatch(ScenePortal __instance, Collider c) {
            // the collider here is the fox's UnityEngine.CapsuleCollider
            // todo: check the blue prince option
            if (false) return true;
            if (c.transform.name != "_Fox(Clone)") return true;
            if (SaveFile.GetString($"randomizer bp {__instance.name}") != null) {
                List<Tuple<string, Portal, Portal>> portalChoices = BPGetThreePortals(SaveFile.GetInt("seed"), __instance.name);
                TunicLogger.LogInfo("portal choices below");
                foreach (var portalChoice in portalChoices) {
                    TunicLogger.LogInfo(portalChoice.Item1);
                }

                // user input goes here somehow??
                // let's just say they choose the first option for now
                // the stuff below probably goes in its own function
                Tuple<string, Portal, Portal> choice = portalChoices[0];
                BPPortalChosen(__instance, choice, c);
                return false;
            }

            return true;
        }


        public static void BPPortalChosen(ScenePortal currentPortal, Tuple<string, Portal, Portal> chosenPortal, Collider foxCollider) {
            SaveFile.SetString($"randomizer bp {currentPortal.name}", chosenPortal.Item1);
            Portal originPortal = chosenPortal.Item2;
            Portal destinationPortal = chosenPortal.Item3;
            string comboNumber = $"{1000 + RandomizedPortals.Count()}";
            RandomizedPortals.Add(comboNumber.ToString(), new PortalCombo(originPortal, destinationPortal));
            currentPortal.destinationSceneName = destinationPortal.Scene;
            currentPortal.id = comboNumber + comboNumber;
            currentPortal.optionalIDToSpawnAt = comboNumber;
            ScenePortal_OnTriggerEnter_PrefixPatch(currentPortal, foxCollider);
        }

    }
}
