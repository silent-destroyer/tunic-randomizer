using System.Collections.Generic;
using UnityEngine;

namespace TunicRandomizer {
    public class PageDisplayPatches {

        public static bool ShowAbilityUnlock;
        public static string AbilityUnlockPage;

        public static void PageDisplay_Show_PostfixPatch(PageDisplay __instance) {

            for (int i = 0; i < 28; i++) {
                SaveFile.SetInt($"unlocked page {i}", SaveFile.GetInt($"randomizer obtained page {i}") == 1 ? 1 : 0);
            }

            bool[] RandomFairiesObtained = new bool[20];
            List<string> Fairies = new List<string>(RandomItemPatches.FairyLookup.Keys);
            int Counter = 0;
            foreach (string Key in Fairies) {
                if (SaveFile.GetInt($"randomizer obtained fairy {Key}") == 1) {
                    RandomFairiesObtained[Counter] = true;
                }
                Counter++;
            }
            for (int i = 0; i < 20; i++) {
                SaveFile.SetInt(RandomItemPatches.FairyLookup[Fairies[i]].Flag, RandomFairiesObtained[i] ? 1 : 0);
            }

            SaveFile.SaveToDisk();
        }

        public static void PageDisplay_Close_PostfixPatch(PageDisplay __instance) {
            
            for (int i = 0; i < 28; i++) {
                // If manual is opened in the heir arena, set pages accordingly so true ending still works based on randomized pages
                if (SceneLoaderPatches.SceneName == "Spirit Arena") {
                    SaveFile.SetInt($"unlocked page {i}", SaveFile.GetInt($"randomizer obtained page {i}") == 1 ? 1 : 0);
                } else {
                    SaveFile.SetInt($"unlocked page {i}", SaveFile.GetInt($"randomizer picked up page {i}") == 1 ? 1 : 0);
                }
            }


            bool[] OpenedFairyChests = new bool[28];
            List<string> Fairies = new List<string>(RandomItemPatches.FairyLookup.Keys);
            int Counter = 0;
            foreach (string Key in Fairies) {
                if (SaveFile.GetInt($"randomizer opened fairy chest {Key}") == 1) {
                    OpenedFairyChests[Counter] = true;
                }
                Counter++;
            }
            for (int i = 0; i < 20; i++) {
                SaveFile.SetInt(RandomItemPatches.FairyLookup[Fairies[i]].Flag, OpenedFairyChests[i] ? 1 : 0);
            }

            if (ShowAbilityUnlock) {
                AreaData AreaData = ScriptableObject.CreateInstance<AreaData>();
                AreaData.topLine = ScriptableObject.CreateInstance<LanguageLine>();
                AreaData.bottomLine = ScriptableObject.CreateInstance<LanguageLine>();
                if (AbilityUnlockPage == "12") {
                    AreaData.topLine.text = $"\"PRAYER Unlocked\"";
                    AreaData.bottomLine.text = $"Jahnuhl yor wizduhm, rooin sEkur";
                } else if (AbilityUnlockPage == "21") {
                    AreaData.topLine.text = $"\"HOLY CROSS Unlocked\"";
                    AreaData.bottomLine.text = $"sEk wuht iz rItfuhlE yorz";
                    foreach (ToggleObjectBySpell SpellToggle in Resources.FindObjectsOfTypeAll<ToggleObjectBySpell>()) {
                        SpellToggle.gameObject.GetComponent<ToggleObjectBySpell>().enabled = true;
                    }
                } else if (AbilityUnlockPage == "26") {
                    AreaData.topLine.text = $"\"ICE ROD Unlocked\"";
                    AreaData.bottomLine.text = $"#A wOnt nO wuht hit #ehm";
                } else {
                    AreaData.topLine.text = $"";
                    AreaData.bottomLine.text = $"";
                }
                AreaLabel.ShowLabel(AreaData);
                ShowAbilityUnlock = false;
                AbilityUnlockPage = "";
            }
            
            SaveFile.SaveToDisk();
        }

    }
}
