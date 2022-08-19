using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace TunicRandomizer {
    public class ScenePatches {
        public static int Fur, Puff, Details, Tunic, Scarf;
        public static int SceneId;
        public static string SceneName;
        public static void OnSceneLoaded_SceneLoader_ScenePatches(Scene loadingScene, LoadSceneMode mode) {
            TunicRandomizer.Logger.LogInfo("Entering scene " + loadingScene.name + " (" + loadingScene.buildIndex + ")");
            SceneId = loadingScene.buildIndex;
            SceneName = loadingScene.name;
            Random rnd = new Random();
            // Fur, Puff, Details, Tunic, Scarf
            Fur = PlayerPalette.ChangeColourByDelta(0, rnd.Next(1, 16));
            Puff = PlayerPalette.ChangeColourByDelta(1, rnd.Next(1, 12));
            Details = PlayerPalette.ChangeColourByDelta(2, rnd.Next(1, 12));
            Tunic = PlayerPalette.ChangeColourByDelta(3, rnd.Next(1, 16));
            Scarf = PlayerPalette.ChangeColourByDelta(4, rnd.Next(1, 11));

            if (SceneName == "Waterfall") {
                List<string> RandomFairiesFound = new List<string>();
                foreach (string Key in ItemPatches.FairyLookup.Keys) {
                    if (SaveFile.GetInt("randomizer obtained fairy " + Key) == 1) {
                        RandomFairiesFound.Add(Key);
                    }
                }
                List<string> FairyNames = new List<string>(ItemPatches.FairyLookup.Keys);
                for (int i = 0; i < Math.Min(19, RandomFairiesFound.Count); i++) {

                    if (i == 4) {
                        continue;
                    }
                    StateVariable.GetStateVariableByName(ItemPatches.FairyLookup[FairyNames[i]].Flag).BoolValue = true;
                }
                /*                List<string> RandomFairiesFound = new List<string>();
                                List<string> RealFairyChestsOpened = new List<string>();
                                foreach (string Key in ItemPatches.FairyLookup.Keys) {
                                    if (SaveFile.GetInt("randomizer obtained fairy " + Key) == 1) {
                                        RandomFairiesFound.Add(Key);
                                    }
                                    if (SaveFile.GetInt(ItemPatches.FairyLookup[Key].Flag) == 1) {
                                        RealFairyChestsOpened.Add(ItemPatches.FairyLookup[Key].Flag);
                                    }
                                }
                                if (RandomFairiesFound.Count >= 10) {
                                    StateVariable.GetStateVariableByName("SV_Fairy_00_Enough Fairies Found").BoolValue = true;
                                }
                                if (RandomFairiesFound.Count == 20) {
                                    StateVariable.GetStateVariableByName("SV_Fairy_00_All Fairies Found").BoolValue = true;
                                }*/
            } else {
                List<string> VanillaFairyChestsOpened = new List<string>();
                List<string> FairyNames = new List<string>(ItemPatches.FairyLookup.Keys);
                foreach (string Key in ItemPatches.FairyLookup.Keys) {
                    StateVariable.GetStateVariableByName(ItemPatches.FairyLookup[Key].Flag).BoolValue = SaveFile.GetInt("randomizer opened fairy chest " + Key) == 1 ? true : false;
                }
            }
        }

    }
}
