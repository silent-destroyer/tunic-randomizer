using System.Collections.Generic;
namespace TunicRandomizer {
    public class PagePatches {

        public static List<int> PagesToShow = new List<int>();
        public static List<int> PagesToRevert = new List<int>();
        public static List<string> FairiesToShow = new List<string>();
        public static List<Fairy> FairiesToRevert = new List<Fairy>();
        public static void Show_PagePatches(PageDisplay __instance) {
            ClearLists();
            TunicRandomizer.Logger.LogInfo("Showed the manual");
            for (int i = 0; i < 28; i++) {
                if (SaveFile.GetInt("randomizer obtained page " + i) == 1) {
                    PagesToShow.Add(i);
                    SaveFile.SetInt("unlocked page " + i, 1);
                }
                if (SaveFile.GetInt("unlocked page " + i) == 1 && SaveFile.GetInt("randomizer obtained page " + i) != 1) {
                    PagesToRevert.Add(i);
                    SaveFile.SetInt("unlocked page " + i, 0);
                }
            }

            foreach (string Key in ItemPatches.FairyLookup.Keys) {
                if (SaveFile.GetInt("randomizer obtained fairy " + Key) == 1 && SaveFile.GetInt(ItemPatches.FairyLookup[Key].Flag) != 1) {
                    FairiesToShow.Add(ItemPatches.FairyLookup[Key].Flag);
                    SaveFile.SetInt(ItemPatches.FairyLookup[Key].Flag, 1);
                } else if (SaveFile.GetInt("randomizer obtained fairy " + Key) != 1 && SaveFile.GetInt(ItemPatches.FairyLookup[Key].Flag) == 1) {
                    FairiesToRevert.Add(ItemPatches.FairyLookup[Key]);
                    SaveFile.SetInt(ItemPatches.FairyLookup[Key].Flag, 0);
                }
            }
            
            SaveFile.SaveToDisk();
        }

        public static void Close_PagePatches(PageDisplay __instance) {
            TunicRandomizer.Logger.LogInfo("Closed the manual");
            foreach (int Page in PagesToShow) {
                SaveFile.SetInt("unlocked page " + Page, 0);
            }
            foreach (int Page in PagesToRevert) {
                SaveFile.SetInt("unlocked page " + Page, 1);
            }
            foreach (string Key in FairiesToShow) {
                SaveFile.SetInt(Key, 0);
            }
            foreach (Fairy Fairy in FairiesToRevert) {
                SaveFile.SetInt(Fairy.Flag, 1);
            }

            SaveFile.SaveToDisk();
            ClearLists();
        }

        private static void ClearLists() {
            PagesToShow.Clear();
            PagesToRevert.Clear();
            FairiesToShow.Clear();
            FairiesToRevert.Clear();
        }
    }
}
