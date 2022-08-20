using System.Collections.Generic;
namespace TunicRandomizer {
    public class PagePatches {

        public static void Show_PagePatches(PageDisplay __instance) {

            TunicRandomizer.Logger.LogInfo("Showed the manual");
            bool[] RandomPagesObtained = new bool[28];
            for (int i = 0; i < 28; i++) {
                if (SaveFile.GetInt("randomizer obtained page " + i) == 1) {
                    RandomPagesObtained[i] = true;
                }
            }
            for (int i = 0; i < RandomPagesObtained.Length; i++) {
                SaveFile.SetInt("unlocked page " + i, RandomPagesObtained[i] ? 1 : 0);
            }

            bool[] RandomFairiesObtained = new bool[20];
            List<string> Fairies = new List<string>(ItemPatches.FairyLookup.Keys);
            int Counter = 0;
            foreach (string Key in Fairies) {
                if (SaveFile.GetInt("randomizer obtained fairy " + Key) == 1) {
                    RandomFairiesObtained[Counter] = true;
                }
                Counter++;
            }
            for (int i = 0; i < 20; i++) {
                SaveFile.SetInt(ItemPatches.FairyLookup[Fairies[i]].Flag, RandomFairiesObtained[i] ? 1 : 0);
            }

            SaveFile.SaveToDisk();
        }

        public static void Close_PagePatches(PageDisplay __instance) {
            TunicRandomizer.Logger.LogInfo("Closed the manual");
            bool[] PickedUpPages = new bool[28];
            for (int i = 0; i < PickedUpPages.Length; i++) {
                if (SaveFile.GetInt("randomizer picked up page " + i) == 1) { 
                    PickedUpPages[i] = true;
                }
            }
            for (int i = 0; i < PickedUpPages.Length; i++) {
                SaveFile.SetInt("unlocked page " + i, PickedUpPages[i] ? 1 : 0);
            }

            bool[] OpenedFairyChests = new bool[28];
            List<string> Fairies = new List<string>(ItemPatches.FairyLookup.Keys);
            int Counter = 0;
            foreach (string Key in Fairies) {
                if (SaveFile.GetInt("randomizer opened fairy chest " + Key) == 1) {
                    OpenedFairyChests[Counter] = true;
                }
                Counter++;
            }
            for (int i = 0; i < 20; i++) {
                SaveFile.SetInt(ItemPatches.FairyLookup[Fairies[i]].Flag, OpenedFairyChests[i] ? 1 : 0);
            }

            SaveFile.SaveToDisk();
        }
    }
}
