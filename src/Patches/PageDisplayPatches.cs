using System.Collections.Generic;

namespace TunicRandomizer {
    public class PageDisplayPatches {

        public static void PageDisplay_Show_PostfixPatch(PageDisplay __instance) {

            for (int i = 0; i < 28; i++) {
                SaveFile.SetInt($"unlocked page {i}", SaveFile.GetInt($"randomizer obtained page {i}") == 1 ? 1 : 0);
            }

            bool[] RandomFairiesObtained = new bool[20];
            List<string> Fairies = new List<string>(ItemLookup.FairyLookup.Keys);
            int Counter = 0;
            foreach (string Key in Fairies) {
                if (SaveFile.GetInt($"randomizer obtained fairy {Key}") == 1) {
                    RandomFairiesObtained[Counter] = true;
                }
                Counter++;
            }
            for (int i = 0; i < 20; i++) {
                SaveFile.SetInt(ItemLookup.FairyLookup[Fairies[i]].Flag, RandomFairiesObtained[i] ? 1 : 0);
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
            List<string> Fairies = new List<string>(ItemLookup.FairyLookup.Keys);
            int Counter = 0;
            foreach (string Key in Fairies) {
                if (SaveFile.GetInt($"randomizer opened fairy chest {Key}") == 1) {
                    OpenedFairyChests[Counter] = true;
                }
                Counter++;
            }
            for (int i = 0; i < 20; i++) {
                SaveFile.SetInt(ItemLookup.FairyLookup[Fairies[i]].Flag, OpenedFairyChests[i] ? 1 : 0);
            }

            SaveFile.SaveToDisk();
        }

    }
}
