using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;
using BepInEx.Logging;

namespace TunicRandomizer {
    public class SceneLoaderPatches {
        private static ManualLogSource Logger = TunicRandomizer.Logger;

        public static int Fur, Puff, Details, Tunic, Scarf;
        public static int SceneId;
        public static string SceneName;

        public static bool SceneLoader_OnSceneLoaded_PrefixPatch(Scene loadingScene, LoadSceneMode mode, SceneLoader __instance) {
            if (SceneName == "Forest Belltower") {
                SaveFile.SetInt("chest open 19", RandomItemPatches.ItemsPickedUp["19 [Forest Belltower]"] ? 1 : 0);
            }
            if (SceneName == "Sword Cave") {
                SaveFile.SetInt("chest open 19", RandomItemPatches.ItemsPickedUp["19 [Sword Cave]"] ? 1 : 0);
            }
            return true;
        }

        public static void SceneLoader_OnSceneLoaded_PostfixPatch(Scene loadingScene, LoadSceneMode mode, SceneLoader __instance) {
            ModelSwaps.SwappedThisSceneAlready = false;
/*            if (loadingScene.name == "Archipelagos Redux" && ModelSwaps.GardenKnightVoid == null) {
                ModelSwaps.SetupGardenKnightVoid();
                SceneLoader.LoadScene("TitleScreen");
            }*/
            if (loadingScene.name == "Spirit Arena" && ModelSwaps.ThirdSword == null) {
                ModelSwaps.InitializeThirdSword();
                SceneLoader.LoadScene("TitleScreen");
                return;
            }
            if(loadingScene.name == "Library Arena" && ModelSwaps.SecondSword == null) {
                ModelSwaps.InitializeSecondSword();
                SceneLoader.LoadScene("Spirit Arena");
                return;
            }
            if (loadingScene.name == "Cathedral Arena" && !ModelSwaps.Chests.ContainsKey("Hyperdash")) {
                ModelSwaps.InitializeChestType("Hyperdash");
                SceneLoader.LoadScene("Library Arena");
                return;
            }
            if (loadingScene.name == "Overworld Redux" && ModelSwaps.Chests.Count == 0) {
                ModelSwaps.InitializeItems();
                SceneLoader.LoadScene("Cathedral Arena");
                return;
            }

            if (ModelSwaps.Chests.Count == 0 && loadingScene.name == "TitleScreen") {
                SceneLoader.LoadScene("Overworld Redux");
                return;
            }
            if (Camera.main != null && Camera.main.gameObject.GetComponentInParent<CycleController>() == null) {
                Camera.main.transform.parent.gameObject.AddComponent<CycleController>();
            }
            Logger.LogInfo("Entering scene " + loadingScene.name + " (" + loadingScene.buildIndex + ")");
            SceneId = loadingScene.buildIndex;
            SceneName = loadingScene.name;
            System.Random rnd = new System.Random();
            PlayerCharacterPatches.StungByBee = false;
            // Fur, Puff, Details, Tunic, Scarf
            if (TunicRandomizer.Settings.RandomFoxColorsEnabled) {
                Fur = PlayerPalette.ChangeColourByDelta(0, rnd.Next(1, 16));
                Puff = PlayerPalette.ChangeColourByDelta(1, rnd.Next(1, 12));
                Details = PlayerPalette.ChangeColourByDelta(2, rnd.Next(1, 12));
                Tunic = PlayerPalette.ChangeColourByDelta(3, rnd.Next(1, 16));
                Scarf = PlayerPalette.ChangeColourByDelta(4, rnd.Next(1, 11));
            }

            UpdateTrackerSceneInfo();
            if (SceneName == "Overworld Redux") {
                //ModelSwaps.InitializeItems();
            } else if (SceneName == "Waterfall") {
                List<string> RandomObtainedFairies = new List<string>();
                foreach (string Key in RandomItemPatches.FairyLookup.Keys) {
                    StateVariable.GetStateVariableByName(RandomItemPatches.FairyLookup[Key].Flag).BoolValue = SaveFile.GetInt("randomizer obtained fairy " + Key) == 1;
                    if (SaveFile.GetInt("randomizer obtained fairy " + Key) == 1) {
                        RandomObtainedFairies.Add(Key);
                    }
                }

                StateVariable.GetStateVariableByName("SV_Fairy_5_Waterfall_Opened").BoolValue = SaveFile.GetInt("randomizer opened fairy chest Waterfall-(-47.0, 45.0, 10.0)") == 1;

                StateVariable.GetStateVariableByName("SV_Fairy_00_Enough Fairies Found").BoolValue = true;

                StateVariable.GetStateVariableByName("SV_Fairy_00_All Fairies Found").BoolValue = true;

            } else if (SceneName == "Spirit Arena") {
                for (int i = 0; i < 28; i++) {
                    SaveFile.SetInt("unlocked page " + i, SaveFile.GetInt("randomizer obtained page " + i) == 1 ? 1 : 0);
                }
                PlayerCharacterPatches.HeirAssistModeDamageValue = RandomItemPatches.ItemsPickedUp.Values.ToList().Where(item => item == true).ToList().Count / 15;
                if (SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST" && TunicRandomizer.Tracker.ImportantItems["Hexagon Gold"] < 20) {
                    Resources.FindObjectsOfTypeAll<Foxgod>().ToList()[0].gameObject.transform.GetChild(0).GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                    Resources.FindObjectsOfTypeAll<Foxgod>().ToList()[0].gameObject.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                }
            } else if (SceneName == "Forest Belltower") {
                SaveFile.SetInt("chest open 19", 0);
            } else if (SceneName == "Overworld Interiors") {
                foreach (string Key in RandomItemPatches.HeroRelicLookup.Keys) {
                    StateVariable.GetStateVariableByName(RandomItemPatches.HeroRelicLookup[Key].Flag).BoolValue = Inventory.GetItemByName(Key).Quantity == 1;
                }
            } else if (SceneName == "Credits First") {
                for (int i = 0; i < 28; i++) {
                    SaveFile.SetInt("unlocked page " + i, SaveFile.GetInt("randomizer obtained page " + i) == 1 ? 1 : 0);
                }
            } else if (SceneName == "TitleScreen") {
                TitleVersion.Initialize();
            } else if (SceneName == "Temple") {
                if (SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST") {
                    foreach (GameObject Questagon in Resources.FindObjectsOfTypeAll<GameObject>().Where(Obj => Obj.name == "questagon")) {
                        Questagon.GetComponent<MeshRenderer>().materials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                        Questagon.GetComponent<MeshRenderer>().receiveShadows = false;
                    }
                }
            } else {
                foreach (string Key in RandomItemPatches.FairyLookup.Keys) {
                    StateVariable.GetStateVariableByName(RandomItemPatches.FairyLookup[Key].Flag).BoolValue = SaveFile.GetInt("randomizer opened fairy chest " + Key) == 1;
                }
                for (int i = 0; i < 28; i++) {
                    SaveFile.SetInt("unlocked page " + i, SaveFile.GetInt("randomizer picked up page " + i) == 1 ? 1 : 0);
                }
                foreach (string Key in RandomItemPatches.HeroRelicLookup.Keys) {
                    StateVariable.GetStateVariableByName(RandomItemPatches.HeroRelicLookup[Key].Flag).BoolValue = SaveFile.GetInt("randomizer picked up " + RandomItemPatches.HeroRelicLookup[Key].OriginalPickupLocation) == 1;
                }
            }
            if (PlayerCharacterPatches.IsTeleporting) {
                PlayerCharacter.instance.cheapIceParticleSystemEmission.enabled = false;
                PlayerCharacter.instance.damageBoostParticleSystemEmission.enabled = false;
                PlayerCharacter.instance.staminaBoostParticleSystemEmission.enabled = false;
                PlayerCharacter.instance.ClearPoison();
                PlayerCharacterPatches.IsTeleporting = false;
            }
            if (!ModelSwaps.SwappedThisSceneAlready && (RandomItemPatches.ItemList.Count > 0 && SaveFile.GetInt("seed") != 0)) {
                ModelSwaps.SwapItemsInScene();
            }
        }

        public static void PauseMenu___button_ReturnToTitle_PostfixPatch(PauseMenu __instance) {
            if (ItemStatsHUD.HexagonQuest != null) {
                ItemStatsHUD.HexagonQuest.SetActive(false);
            }
            SceneName = "TitleScreen";
        }

        public static void UpdateTrackerSceneInfo() {
            TunicRandomizer.Tracker.CurrentScene.SceneId = SceneId;
            TunicRandomizer.Tracker.CurrentScene.SceneName = SceneName;
            TunicRandomizer.Tracker.CurrentScene.Fur = ColorPalette.Fur[PlayerPalette.selectionIndices[0]];
            TunicRandomizer.Tracker.CurrentScene.Puff = PlayerPalette.selectionIndices[1] == 0 ? ColorPalette.Fur[PlayerPalette.selectionIndices[0]] : ColorPalette.Puff[PlayerPalette.selectionIndices[1]];
            TunicRandomizer.Tracker.CurrentScene.Details = ColorPalette.Details[PlayerPalette.selectionIndices[2]];
            TunicRandomizer.Tracker.CurrentScene.Tunic = ColorPalette.Tunic[PlayerPalette.selectionIndices[3]];
            TunicRandomizer.Tracker.CurrentScene.Scarf = ColorPalette.Scarf[PlayerPalette.selectionIndices[4]];

            ItemTracker.SaveTrackerFile();
        }
    }
}
