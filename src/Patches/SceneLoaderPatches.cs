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
        public static bool SpawnedGhosts = false;

        public static bool SceneLoader_OnSceneLoaded_PrefixPatch(Scene loadingScene, LoadSceneMode mode, SceneLoader __instance) {
            
            if (SceneName == "Forest Belltower") {
                SaveFile.SetInt("chest open 19", ItemRandomizer.ItemsPickedUp["19 [Forest Belltower]"] ? 1 : 0);
            }
            if (SceneName == "Sword Cave") {
                SaveFile.SetInt("chest open 19", ItemRandomizer.ItemsPickedUp["19 [Sword Cave]"] ? 1 : 0);
            }

            return true;
        }

        public static void SceneLoader_OnSceneLoaded_PostfixPatch(Scene loadingScene, LoadSceneMode mode, SceneLoader __instance) {

            ModelSwaps.SwappedThisSceneAlready = false;
            SpawnedGhosts = false;
            if (loadingScene.name == "Posterity" && !EnemyRandomizer.Enemies.ContainsKey("Phage")) {
                EnemyRandomizer.InitializeEnemies("Posterity");
                SceneLoader.LoadScene("TitleScreen");
                return;
            }
            if (loadingScene.name == "Library Hall" && !EnemyRandomizer.Enemies.ContainsKey("administrator_servant")) {
                EnemyRandomizer.InitializeEnemies("Library Hall");
                SceneLoader.LoadScene("Posterity");
                return;
            }
            if (loadingScene.name == "Cathedral Redux" && !EnemyRandomizer.Enemies.ContainsKey("Voidtouched")) {
                EnemyRandomizer.InitializeEnemies("Cathedral Redux");
                SceneLoader.LoadScene("Library Hall");
                return;
            }
            if (loadingScene.name == "Fortress Main" && !EnemyRandomizer.Enemies.ContainsKey("woodcutter")) {
                EnemyRandomizer.InitializeEnemies("Fortress Main");
                SceneLoader.LoadScene("Cathedral Redux");
                return;
            }
            if (loadingScene.name == "Fortress Reliquary" && !EnemyRandomizer.Enemies.ContainsKey("voidling redux")) {
                EnemyRandomizer.InitializeEnemies("Fortress Reliquary");
                SceneLoader.LoadScene("Fortress Main");
                return;
            }
            if (loadingScene.name == "ziggurat2020_1" && !EnemyRandomizer.Enemies.ContainsKey("administrator")) {
                EnemyRandomizer.InitializeEnemies("ziggurat2020_1");
                SceneLoader.LoadScene("Fortress Reliquary");
                return;
            }
            if (loadingScene.name == "Swamp Redux 2" && !EnemyRandomizer.Enemies.ContainsKey("bomezome_easy")) {
                EnemyRandomizer.InitializeEnemies("Swamp Redux 2");
                SceneLoader.LoadScene("ziggurat2020_1");
                return;
            }
            if (loadingScene.name == "Quarry Redux" && !EnemyRandomizer.Enemies.ContainsKey("Scavenger")) {
                EnemyRandomizer.InitializeEnemies("Quarry Redux");
                SceneLoader.LoadScene("Swamp Redux 2");
                return;
            }
            if (loadingScene.name == "Fortress Basement" && !EnemyRandomizer.Enemies.ContainsKey("Spider Small")) {
                EnemyRandomizer.InitializeEnemies("Fortress Basement");
                SceneLoader.LoadScene("Quarry Redux");
                return;
            }
            if (loadingScene.name == "frog cave main" && !EnemyRandomizer.Enemies.ContainsKey("Frog Small")) {
                EnemyRandomizer.InitializeEnemies("frog cave main");
                SceneLoader.LoadScene("Fortress Basement");
                return;
            }
            if (loadingScene.name == "Atoll Redux" && !EnemyRandomizer.Enemies.ContainsKey("plover")) {
                EnemyRandomizer.InitializeEnemies("Atoll Redux");
                SceneLoader.LoadScene("frog cave main");
                return;
            }
            if (loadingScene.name == "Archipelagos Redux" && ModelSwaps.GlowEffect == null) {
                ModelSwaps.SetupGlowEffect();
                EnemyRandomizer.InitializeEnemies("Archipelagos Redux");
                SceneLoader.LoadScene("Atoll Redux");
                return;
            }
            if (loadingScene.name == "Transit" && !ModelSwaps.Items.ContainsKey("Relic - Hero Sword")) {
                ModelSwaps.InitializeHeroRelics();
                SceneLoader.LoadScene("Archipelagos Redux");
                return;
            }
            if (loadingScene.name == "Spirit Arena" && ModelSwaps.ThirdSword == null) {
                ModelSwaps.InitializeThirdSword();
                SceneLoader.LoadScene("Transit");
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
                EnemyRandomizer.InitializeEnemies("Cathedral Arena");
                return;
            }
            if (loadingScene.name == "Overworld Redux" && ModelSwaps.Chests.Count == 0) {
                if (GhostHints.GhostFox == null) {
                    GhostHints.InitializeGhostFox();
                }
                ModelSwaps.InitializeItems();
                EnemyRandomizer.InitializeEnemies("Overworld Redux");
                SceneLoader.LoadScene("Cathedral Arena");
                return;
            }

            if (ModelSwaps.Chests.Count == 0 && loadingScene.name == "TitleScreen") {
                QuickSettings.OdinRounded = Resources.FindObjectsOfTypeAll<Font>().Where(Font => Font.name == "Odin Rounded").ToList()[0];
                SceneLoader.LoadScene("Overworld Redux");
                return;
            }
            if (Camera.main != null && Camera.main.gameObject.GetComponentInParent<CycleController>() == null) {
                Camera.main.transform.parent.gameObject.AddComponent<CycleController>();
            }

            Logger.LogInfo("Entering scene " + loadingScene.name + " (" + loadingScene.buildIndex + ")");
            SceneId = loadingScene.buildIndex;
            SceneName = loadingScene.name;

            if (SceneName == "Overworld Redux" && (StateVariable.GetStateVariableByName("Has Been Betrayed").BoolValue || StateVariable.GetStateVariableByName("Has Died To God").BoolValue) && SaveFile.GetInt("randomizer died to heir") != 1 && SaveFile.GetString("randomizer game mode") != "HEXAGONQUEST") {
                PlayerCharacterPatches.ResetDayNightTimer = 0.0f;
                Logger.LogInfo("Resetting time of day to daytime!");
            }
     
            PlayerCharacterPatches.StungByBee = false;
            // Fur, Puff, Details, Tunic, Scarf
            if (TunicRandomizer.Settings.RandomFoxColorsEnabled) {
                PaletteEditor.RandomizeFoxColors();
            }

            try {
                if (TunicRandomizer.Settings.UseCustomTexture) {
                    PaletteEditor.LoadCustomTexture();
                }
            } catch (Exception ex) {
                Logger.LogError("An error occurred applying custom texture:");
                Logger.LogError(ex.Message + " " + ex.StackTrace);
            }

            UpdateTrackerSceneInfo();
            if (SceneName == "Waterfall") {
                List<string> RandomObtainedFairies = new List<string>();
                foreach (string Key in ItemRandomizer.FairyLookup.Keys) {
                    StateVariable.GetStateVariableByName(ItemRandomizer.FairyLookup[Key].Flag).BoolValue = SaveFile.GetInt("randomizer obtained fairy " + Key) == 1;
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
                PlayerCharacterPatches.HeirAssistModeDamageValue = ItemRandomizer.ItemsPickedUp.Values.ToList().Where(item => item == true).ToList().Count / 15;
                if (SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST") {
                    Resources.FindObjectsOfTypeAll<Foxgod>().ToList()[0].gameObject.transform.GetChild(0).GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                    Resources.FindObjectsOfTypeAll<Foxgod>().ToList()[0].gameObject.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                }
            } else if (SceneName == "Forest Belltower") {
                SaveFile.SetInt("chest open 19", 0);
            } else if (SceneName == "Overworld Interiors") {
                foreach (string Key in ItemRandomizer.HeroRelicLookup.Keys) {
                    StateVariable.GetStateVariableByName(ItemRandomizer.HeroRelicLookup[Key].Flag).BoolValue = Inventory.GetItemByName(Key).Quantity == 1;
                }
                GameObject.Destroy(GameObject.Find("_Special/Bed Toggle Trigger/"));
                if ((StateVariable.GetStateVariableByName("Has Been Betrayed").BoolValue || StateVariable.GetStateVariableByName("Has Died To God").BoolValue) && SaveFile.GetString("randomizer game mode") != "HEXAGONQUEST") {
                    SetupDayNightHourglass();
                }
                if (GameObject.Find("_Offerings/ash group/")) {
                    GameObject.Find("_Offerings/ash group/").transform.position = new Vector3(-24.2824f, 29.8f, -45.4f);
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
                if (TunicRandomizer.Settings.HeroPathHintsEnabled && Inventory.GetItemByName("Hyperdash").Quantity == 0) {

                    GameObject HintStatueGlow = GameObject.Instantiate(ModelSwaps.GlowEffect);
                    HintStatueGlow.SetActive(true);
                    HintStatueGlow.transform.position = new Vector3(13f, 0f, 49f);
                }
            } else if (SceneName == "Overworld Redux") {
                GameObject.Find("_Signposts/Signpost (3)/").GetComponent<Signpost>().message.text = $"#is wA too \"West Garden\"\n<#33FF33>[death] bEwAr uhv tArE [death]";
                if (TunicRandomizer.Settings.HeroPathHintsEnabled && SaveFile.GetInt("randomizer got mailbox hint item") == 0) {
                    GameObject.Find("_Environment/_Decorations/Mailbox (1)/mailbox flag").transform.rotation = new Quaternion(0.5f, -0.5f, 0.5f, 0.5f);
                }
            } else if (SceneName == "Swamp Redux 2") {
                GhostHints.GhostFox.GetComponent<NPC>().nPCAnimState = NPC.NPCAnimState.GAZE;
                GameObject DoorHint = GameObject.Instantiate(GhostHints.GhostFox);
                DoorHint.transform.position = new Vector3(82.5f, 14f, 143.7f);
                DoorHint.transform.transform.rotation = new Quaternion(0f, 0f, 0f, 1f);
                LanguageLine DoorSecret = ScriptableObject.CreateInstance<LanguageLine>();
                DoorSecret.text = $"$$$... dOnt tehl ehnEwuhn, buht #aht \"DOOR\" bahk #Ar\nkahn bE \"OPENED\" fruhm #E \"OUTSIDE...\"";
                DoorHint.GetComponent<NPC>().script = DoorSecret;
                DoorHint.SetActive(true);
            } else if (SceneName == "Shop") {
                if (new System.Random().Next(100) < 3) {
                    GameObject.Find("merchant").SetActive(false);
                    GameObject.Find("Environment").transform.GetChild(3).gameObject.SetActive(true);
                }
            } else {
                foreach (string Key in ItemRandomizer.FairyLookup.Keys) {
                    StateVariable.GetStateVariableByName(ItemRandomizer.FairyLookup[Key].Flag).BoolValue = SaveFile.GetInt("randomizer opened fairy chest " + Key) == 1;
                }
                for (int i = 0; i < 28; i++) {
                    SaveFile.SetInt("unlocked page " + i, SaveFile.GetInt("randomizer picked up page " + i) == 1 ? 1 : 0);
                }
                foreach (string Key in ItemRandomizer.HeroRelicLookup.Keys) {
                    StateVariable.GetStateVariableByName(ItemRandomizer.HeroRelicLookup[Key].Flag).BoolValue = SaveFile.GetInt("randomizer picked up " + ItemRandomizer.HeroRelicLookup[Key].OriginalPickupLocation) == 1;
                }
            }

            if (PlayerCharacterPatches.IsTeleporting) {
                PlayerCharacter.instance.cheapIceParticleSystemEmission.enabled = false;
                PlayerCharacter.instance.damageBoostParticleSystemEmission.enabled = false;
                PlayerCharacter.instance.staminaBoostParticleSystemEmission.enabled = false;
                PlayerCharacter.instance._CompletelyInvulnerableEvenToIFrameIgnoringAttacks_k__BackingField = false;
                PlayerCharacter.instance.ClearPoison();
                PlayerCharacterPatches.IsTeleporting = false;
                GameObject.Destroy(PlayerCharacter.instance.gameObject.GetComponent<Rotate>());
            }

            foreach (ItemData Fool in ItemRandomizer.ItemList.Values.ToList().Where(Item => Item.Reward.Type == "FOOL")) {
                Fool.Reward.Type = "MONEY";
                Fool.Reward.Name = "money";
            }

            try {
                if (!ModelSwaps.SwappedThisSceneAlready && (ItemRandomizer.ItemList.Count > 0 && SaveFile.GetInt("seed") != 0)) {
                    ModelSwaps.SwapItemsInScene();
                }
            } catch (Exception ex) {
                Logger.LogError("An error occurred swapping item models in this scene:");
                Logger.LogError(ex.Message + " " + ex.StackTrace);
            }

            if (SaveFile.GetInt("randomizer shuffled abilities") == 1 && SaveFile.GetInt("randomizer obtained page 21") == 0) {
                foreach (ToggleObjectBySpell SpellToggle in Resources.FindObjectsOfTypeAll<ToggleObjectBySpell>()) {
                    SpellToggle.gameObject.GetComponent<ToggleObjectBySpell>().enabled = false;
                }
            }

            try {
                if (TunicRandomizer.Settings.GhostFoxHintsEnabled && GhostHints.HintGhosts.Count > 0 && SaveFile.GetInt("seed") != 0) {
                    GhostHints.SpawnHintGhosts(SceneName);
                    SpawnedGhosts = true;
                }
            } catch (Exception ex) {
                Logger.LogError("An error occurred spawning hint ghost foxes:");
                Logger.LogError(ex.Message + " " + ex.StackTrace);
            }


            if (TunicRandomizer.Settings.EnemyRandomizerEnabled && EnemyRandomizer.Enemies.Count > 0 && !EnemyRandomizer.ExcludedScenes.Contains(SceneName)) {
                EnemyRandomizer.SpawnNewEnemies();
            }

            try {
                FairyTargets.CreateFairyTargets();
            } catch (Exception ex) {
                Logger.LogError("An error occurred creating new fairy seeker spell targets:");
                Logger.LogError(ex.Message + " " + ex.StackTrace);
            }

            if (TunicRandomizer.Settings.RealestAlwaysOn) {
                try {
                    GameObject.FindObjectOfType<RealestSpell>().SpellEffect();
                } catch (Exception e) { 
                    
                }
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

        public static void SetupDayNightHourglass() {
            GameObject DayNightSwitch = GameObject.Instantiate(GameObject.Find("Trophy Stuff/TROPHY POINT (1)/"));
            DayNightSwitch.name = "day night hourglass";
            DayNightSwitch.GetComponent<GoldenTrophyRoom>().item = null;
            DayNightSwitch.transform.GetChild(0).gameObject.SetActive(true);
            GameObject Hourglass = DayNightSwitch.transform.GetChild(0).GetChild(0).gameObject;
            Hourglass.GetComponent<MeshFilter>().mesh = ModelSwaps.Items["SlowmoItem"].GetComponent<MeshFilter>().mesh;
            Hourglass.GetComponent<MeshRenderer>().materials = ModelSwaps.Items["SlowmoItem"].GetComponent<MeshRenderer>().materials;
            Hourglass.GetComponent<MeshRenderer>().materials[0].color = CycleController.IsNight ? new Color(1f, 0f, 1f, 1f) : new Color(1f, 1f, 0f, 1f);
            Hourglass.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            Hourglass.transform.localPosition = Vector3.zero;
            Hourglass.name = "hourglass";
            Hourglass.SetActive(true);
            GameObject GlowEffect = GameObject.Instantiate(ModelSwaps.GlowEffect);
            GlowEffect.transform.parent = DayNightSwitch.transform;
            GlowEffect.transform.GetChild(0).gameObject.SetActive(false);
            GlowEffect.transform.GetChild(1).gameObject.SetActive(false);
            GlowEffect.transform.GetChild(2).gameObject.SetActive(false);
            GlowEffect.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            GlowEffect.transform.localPosition = new Vector3(-0.5f, -1f, -0.1f);
            GlowEffect.SetActive(true);
            DayNightSwitch.transform.position = new Vector3(-26.3723f, 28.9452f, -46.1847f);
            DayNightSwitch.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
        }
    }
}
