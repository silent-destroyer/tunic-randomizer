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
                List<string> RandomObtainedFairies = new List<string>();
                foreach (string Key in ItemPatches.FairyLookup.Keys) {
                    StateVariable.GetStateVariableByName(ItemPatches.FairyLookup[Key].Flag).BoolValue = SaveFile.GetInt("randomizer obtained fairy " + Key) == 1 ? true : false;
                    if (SaveFile.GetInt("randomizer obtained fairy " + Key) == 1) {
                        RandomObtainedFairies.Add(Key);
                    }
                }
                
                StateVariable.GetStateVariableByName("SV_Fairy_5_Waterfall_Opened").BoolValue = SaveFile.GetInt("randomizer opened fairy chest Waterfall-(-47.0, 45.0, 10.0)") == 1 ? true : false;
                
                StateVariable.GetStateVariableByName("SV_Fairy_00_Enough Fairies Found").BoolValue = RandomObtainedFairies.Count >= 10 ? true : false;
               
                StateVariable.GetStateVariableByName("SV_Fairy_00_All Fairies Found").BoolValue = RandomObtainedFairies.Count == 20 ? true : false;

            } else if (SceneName == "Spirit Arena") {
                for (int i = 0; i < 28; i++) {
                    SaveFile.SetInt("unlocked page " + i, SaveFile.GetInt("randomizer obtained page " + i) == 1 ? 1 : 0);
                }
            } else {
                foreach (string Key in ItemPatches.FairyLookup.Keys) {
                    StateVariable.GetStateVariableByName(ItemPatches.FairyLookup[Key].Flag).BoolValue = SaveFile.GetInt("randomizer opened fairy chest " + Key) == 1 ? true : false;
                }
                for (int i = 0; i < 28; i++) {
                    SaveFile.SetInt("unlocked page " + i, SaveFile.GetInt("randomizer picked up page " + i) == 1 ? 1 : 0);
                }
            }
        }

    }
}
