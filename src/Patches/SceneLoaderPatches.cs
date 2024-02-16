﻿using BepInEx.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {
    public class SceneLoaderPatches {
        private static ManualLogSource Logger = TunicRandomizer.Logger;

        public static string SceneName;
        public static int SceneId;
        public static float TimeOfLastSceneTransition = 0.0f;
        public static bool SpawnedGhosts = false;

        public static GameObject SpiritArenaTeleporterPrefab;

        public static bool SceneLoader_OnSceneLoaded_PrefixPatch(Scene loadingScene, LoadSceneMode mode, SceneLoader __instance) {
            // ladder storage fix
            if (PlayerCharacter.instance != null)
            {
                PlayerCharacter.instance.currentLadder = null;
                PlayerCharacter.instance.GetComponent<Animator>().SetBool("climbing", false);
            }
            TimeOfLastSceneTransition = SaveFile.GetFloat("playtime");
            if (SceneName == "Forest Belltower") {
                SaveFile.SetInt("chest open 19", SaveFile.GetInt("randomizer picked up 19 [Forest Belltower]"));
            }
            if (SceneName == "Sword Cave") {
                SaveFile.SetInt("chest open 19", SaveFile.GetInt("randomizer picked up 19 [Sword Cave]"));
            }

            if (IsArchipelago()) {
                foreach (long location in Archipelago.instance.integration.session.Locations.AllLocationsChecked) {
                    string LocationId = Archipelago.instance.integration.session.Locations.GetLocationNameFromId(location);
                    string GameObjectId = Locations.LocationDescriptionToId[LocationId];
                    if (SaveFile.GetInt(ItemCollectedKey + GameObjectId) == 0) {
                        SaveFile.SetInt($"randomizer {GameObjectId} was collected", 1);
                    }
                }
            }

            return true;
        }

        public static void SceneLoader_OnSceneLoaded_PostfixPatch(Scene loadingScene, LoadSceneMode mode, SceneLoader __instance) {

            ModelSwaps.SwappedThisSceneAlready = false;
            SpawnedGhosts = false;

            if (loadingScene.name == "Posterity" && !EnemyRandomizer.Enemies.ContainsKey("Phage")) {
                EnemyRandomizer.InitializeEnemies("Posterity");
                ModelSwaps.CreateOtherWorldItemBlocks();
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
            if (loadingScene.name == "ziggurat2020_3" && !EnemyRandomizer.Enemies.ContainsKey("Centipede")) {
                EnemyRandomizer.InitializeEnemies("ziggurat2020_3");
                SceneLoader.LoadScene("Fortress Reliquary");
                return;
            }
            if (loadingScene.name == "ziggurat2020_1" && !EnemyRandomizer.Enemies.ContainsKey("administrator")) {
                EnemyRandomizer.InitializeEnemies("ziggurat2020_1");
                SceneLoader.LoadScene("ziggurat2020_3");
                return;
            }
            if (loadingScene.name == "Swamp Redux 2" && !EnemyRandomizer.Enemies.ContainsKey("bomezome_easy")) {
                EnemyRandomizer.InitializeEnemies("Swamp Redux 2");
                SceneLoader.LoadScene("ziggurat2020_1");
                return;
            }
            if (loadingScene.name == "Quarry" && !EnemyRandomizer.Enemies.ContainsKey("Scavenger_stunner")) {
                EnemyRandomizer.InitializeEnemies("Quarry");
                SceneLoader.LoadScene("Swamp Redux 2");
                return;
            }
            if (loadingScene.name == "Quarry Redux" && !EnemyRandomizer.Enemies.ContainsKey("Scavenger")) {
                EnemyRandomizer.InitializeEnemies("Quarry Redux");
                SceneLoader.LoadScene("Quarry");
                return;
            }
            if (loadingScene.name == "Crypt" && !EnemyRandomizer.Enemies.ContainsKey("Shadowreaper")) {
                EnemyRandomizer.InitializeEnemies("Crypt");
                SceneLoader.LoadScene("Quarry Redux");
                return;
            }
            if (loadingScene.name == "Fortress Basement" && !EnemyRandomizer.Enemies.ContainsKey("Spider Small")) {
                EnemyRandomizer.InitializeEnemies("Fortress Basement");
                ModelSwaps.BlueFire = GameObject.Instantiate(GameObject.Find("Room - Big Room/Fortress wall lamp small unlit (1)/Fire/lamp fire"));
                ModelSwaps.BlueFire.SetActive(false);
                GameObject.DontDestroyOnLoad(ModelSwaps.BlueFire);
                SceneLoader.LoadScene("Crypt");
                return;
            }
            if (loadingScene.name == "frog cave main" && !EnemyRandomizer.Enemies.ContainsKey("Frog Small")) {
                EnemyRandomizer.InitializeEnemies("frog cave main");
                SceneLoader.LoadScene("Fortress Basement");
                return;
            }
            if (loadingScene.name == "Sewer" && !EnemyRandomizer.Enemies.ContainsKey("Spinnerbot Corrupted")) {
                EnemyRandomizer.InitializeEnemies("Sewer");
                SceneLoader.LoadScene("frog cave main");
                return;
            }
            if (loadingScene.name == "Atoll Redux" && !EnemyRandomizer.Enemies.ContainsKey("plover")) {
                EnemyRandomizer.InitializeEnemies("Atoll Redux");
                SceneLoader.LoadScene("Sewer");
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
                ItemPresentationPatches.SetupCustomSwordItemPresentations();
                SpiritArenaTeleporterPrefab = GameObject.Instantiate(GameObject.Find("Teleporter"));
                GameObject.DontDestroyOnLoad(SpiritArenaTeleporterPrefab);
                SpiritArenaTeleporterPrefab.transform.position = new Vector3(-30000f, -30000f, -30000f);
                SpiritArenaTeleporterPrefab.SetActive(false);
                ModelSwaps.SetupStarburstEffect();
                SceneLoader.LoadScene("Transit");
                return;
            }
            if (loadingScene.name == "Library Arena" && ModelSwaps.SecondSword == null) {
                ModelSwaps.InitializeSecondSword();
                EnemyRandomizer.InitializeEnemies("Library Arena");
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
                TextBuilderPatches.SetupCustomGlyphSprites();
                EnemyRandomizer.InitializeEnemies("Overworld Redux");
                SceneLoader.LoadScene("Cathedral Arena");
                return;
            }
            if (ModelSwaps.Chests.Count == 0 && loadingScene.name == "TitleScreen") {

                CustomItemBehaviors.CreateCustomItems();
                GameObject ArchipelagoObject = new GameObject("archipelago");
                Archipelago.instance = ArchipelagoObject.AddComponent<Archipelago>();
                GameObject.DontDestroyOnLoad(ArchipelagoObject);
                if (Locations.VanillaLocations.Count == 0) {
                    Locations.CreateLocationLookups();
                }
                PaletteEditor.OdinRounded = Resources.FindObjectsOfTypeAll<Font>().Where(Font => Font.name == "Odin Rounded").ToList()[0];
                SceneLoader.LoadScene("Overworld Redux");
                return;
            }

            if (Camera.main != null && Camera.main.gameObject.GetComponentInParent<CycleController>() == null) {
                Camera.main.transform.parent.gameObject.AddComponent<CycleController>();
            }

            Logger.LogInfo("Entering scene " + loadingScene.name + " (" + loadingScene.buildIndex + ")");
            SceneName = loadingScene.name;
            SceneId = loadingScene.buildIndex;

            if (SceneName == "Overworld Redux" && (StateVariable.GetStateVariableByName("Has Been Betrayed").BoolValue &&
                StateVariable.GetStateVariableByName("Has Died To God").BoolValue) && SaveFile.GetInt(DiedToHeir) != 1 && SaveFile.GetInt(HexagonQuestEnabled) == 0) {
                PlayerCharacterPatches.ResetDayNightTimer = 0.0f;
                Logger.LogInfo("Resetting time of day to daytime!"); 
                SpawnHeirFastTravel("Spirit Arena", new Vector3(2.0801f, 43.5833f, -54.0065f));
            }

            PlayerCharacterPatches.StungByBee = false;
            // Fur, Puff, Details, Tunic, Scarf
            if (TunicRandomizer.Settings.RandomFoxColorsEnabled) {
                PaletteEditor.RandomizeFoxColors();
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

            // Failsafe for potion flasks not combining due to receiving 3rd shard during a load zone or at some other weird moment
            if (Inventory.GetItemByName("Flask Shard").Quantity >= 3) {
                Inventory.GetItemByName("Flask Shard").Quantity -= 3;
                Inventory.GetItemByName("Flask Container").Quantity += 1;
            }

            foreach (string Key in ItemLookup.FairyLookup.Keys) {
                StateVariable.GetStateVariableByName(ItemLookup.FairyLookup[Key].Flag).BoolValue = SaveFile.GetInt("randomizer opened fairy chest " + Key) == 1;
            }
            for (int i = 0; i < 28; i++) {
                SaveFile.SetInt("unlocked page " + i, SaveFile.GetInt("randomizer picked up page " + i) == 1 ? 1 : 0);
            }

            if (SceneName == "Waterfall") {
                List<string> RandomObtainedFairies = new List<string>();
                foreach (string Key in ItemLookup.FairyLookup.Keys) {
                    StateVariable.GetStateVariableByName(ItemLookup.FairyLookup[Key].Flag).BoolValue = SaveFile.GetInt("randomizer obtained fairy " + Key) == 1;
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
                PlayerCharacterPatches.HeirAssistModeDamageValue = Locations.CheckedLocations.Values.ToList().Where(item => item).ToList().Count / 15;
                if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                    Resources.FindObjectsOfTypeAll<Foxgod>().ToList()[0].gameObject.transform.GetChild(0).GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                    Resources.FindObjectsOfTypeAll<Foxgod>().ToList()[0].gameObject.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                }

                SpawnHeirFastTravel("Overworld Redux", new Vector3(-30000f, -30000f, -30000f));
            } else if (SceneName == "Overworld Interiors") {
                GameObject.Find("Trophy Stuff").transform.GetChild(4).gameObject.SetActive(true);

                GameObject.FindObjectOfType<BedToggle>().canBeUsed = StateVariable.GetStateVariableByName("false");
                if ((StateVariable.GetStateVariableByName("Has Been Betrayed").BoolValue || StateVariable.GetStateVariableByName("Has Died To God").BoolValue) && SaveFile.GetInt(HexagonQuestEnabled) == 0) {
                    InteractionPatches.SetupDayNightHourglass();
                }

                SetupOldHouseRelicToggles();
                if (GameObject.Find("_Offerings/ash group/")) {
                    GameObject.Find("_Offerings/ash group/").transform.position = new Vector3(-24.2824f, 29.8f, -45.4f);
                }
            } else if (SceneName == "Forest Belltower") {
                SaveFile.SetInt("chest open 19", 0);
            } else if (SceneName == "TitleScreen") {
                TitleVersion.Initialize();
                if (!Archipelago.instance.integration.connected && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO) {
                    Archipelago.instance.Connect();
                }
            } else if (SceneName == "Temple") {
                if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                    foreach (GameObject Questagon in Resources.FindObjectsOfTypeAll<GameObject>().Where(Obj => Obj.name == "questagon")) {
                        Questagon.GetComponent<MeshRenderer>().materials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                        Questagon.GetComponent<MeshRenderer>().receiveShadows = false;
                    }
                }
                if (TunicRandomizer.Settings.HeroPathHintsEnabled) {
                    GameObject HintStatueGlow = GameObject.Instantiate(ModelSwaps.GlowEffect);
                    HintStatueGlow.SetActive(true);
                    HintStatueGlow.transform.position = new Vector3(13f, 0f, 49f);
                    HintStatueGlow.AddComponent<VisibleByNotHavingItem>().Item = Inventory.GetItemByName("Hyperdash");
                }
            } else if (SceneName == "Overworld Redux") {
                GameObject.Find("_Signposts/Signpost (3)/").GetComponent<Signpost>().message.text = $"#is wA too \"West Garden\"\n<#33FF33>[death] bEwAr uhv tArE [death]";
                GameObject.Find("_Environment Special/Door (1)/door/key twist").GetComponent<MeshRenderer>().materials = ModelSwaps.Items["Key (House)"].GetComponent<MeshRenderer>().materials;
                GameObject.Find("_Environment/_Decorations/Mailbox (1)/mailbox flag").AddComponent<MailboxFlag>();

                if (SaveFile.GetInt("randomizer entrance rando enabled") == 1 || (SaveFile.GetInt("seed") == 0 && 
                    ((TunicRandomizer.Settings.EntranceRandoEnabled && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER) || 
                    (TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO && Archipelago.instance.integration.connected 
                    && Archipelago.instance.integration.slotData.ContainsKey("entrance_rando") && Archipelago.instance.integration.slotData["entrance_rando"].ToString() == "1")))) {
                    GhostHints.SpawnTorchHintGhost();
                }

                if (TunicRandomizer.Settings.ClearEarlyBushes) {
                    int[] bushesToClear = new int[] { 7, 2, 16, 47, 42 };
                    foreach (int bush in bushesToClear) {
                        if (GameObject.Find($"_Bush and Grass/bush ({bush})/") != null) {
                            GameObject.Find($"_Bush and Grass/bush ({bush})/").SetActive(false);
                        }
                    }
                }
                if (SaveFile.GetInt(DiedToHeir) == 1) {
                    SpawnHeirFastTravel("Spirit Arena", new Vector3(2.0801f, 43.5833f, -54.0065f));
                }
            } else if (SceneName == "Swamp Redux 2") {
                GhostHints.SpawnCathedralDoorGhost();

                // Removes the barricades from the swamp shop during the day 
                if (GameObject.Find("_Setpieces Etc/plank_4u") != null && GameObject.Find("_Setpieces Etc/plank_4u (1)") != null) {
                    GameObject.Find("_Setpieces Etc/plank_4u").SetActive(false);
                    GameObject.Find("_Setpieces Etc/plank_4u (1)").SetActive(false);
                }
                // Activate night bridge to allow access to shortcut ladder
                GameObject.Find("_Setpieces Etc/NightBridge/").GetComponent<DayNightBridge>().dayOrNight = StateVariable.GetStateVariableByName("Is Night").BoolValue ? DayNightBridge.DayNight.NIGHT : DayNightBridge.DayNight.DAY;

                if (SaveFile.GetInt("fuseClosed 1096") == 1) {
                    if (GameObject.Find("_Setpieces Etc/plank_4u (planks on gate)/") != null) {
                        GameObject.Find("_Setpieces Etc/plank_4u (planks on gate)/").SetActive(false);
                    }
                    if (GameObject.Find("_Setpieces Etc/Gated Wooden Double Door/") != null) {
                        GameObject.Find("_Setpieces Etc/Gated Wooden Double Door/").GetComponent<ToggleGraveyardGate>().stateVar = StateVariable.GetStateVariableByName("true");
                        GameObject.Find("_Setpieces Etc/Gated Wooden Double Door/").GetComponent<ToggleGraveyardGate>().isNightstateVar = StateVariable.GetStateVariableByName("true");
                    }

                }

                if (TunicRandomizer.Settings.MoreSkulls) {
                    InteractionPatches.SpawnMoreSkulls();
                }
            } else if (SceneName == "g_elements") {
                GhostHints.SpawnLostGhostFox();
            } else if (SceneName == "Posterity") {
                GhostHints.SpawnRescuedGhostFox();
            } else if (SceneName == "Shop") {
                if (new System.Random().Next(100) < 2) {
                    GameObject.Find("merchant").SetActive(false);
                    GameObject.Find("Environment").transform.GetChild(3).gameObject.SetActive(true);
                }
                ModelSwaps.AddNewShopItems();
            } else if (SceneName == "ShopSpecial") {
                if (new System.Random().Next(100) < 2) {
                    GameObject.Find("merchant (1)").SetActive(false);
                    GameObject.Find("Environment").transform.GetChild(3).gameObject.SetActive(true);
                }
            } else if (SceneName == "Cathedral Arena") {
                if (SaveFile.GetInt("randomizer entrance rando enabled") == 1) {
                    StateVariable.GetStateVariableByName("SV_cathedral elevator").BoolValue = false;
                }
            } else if (SceneName == "Cathedral Redux") {
                if (SaveFile.GetInt("randomizer entrance rando enabled") == 1) {
                    StateVariable.GetStateVariableByName("SV_cathedral elevator").BoolValue = true;
                }
            } else if (SceneName == "Maze Room") {
                foreach (Chest chest in Resources.FindObjectsOfTypeAll<Chest>().Where(chest => chest.name == "Chest: Fairy")) {
                    chest.transform.GetChild(4).gameObject.SetActive(false);
                }
            } else if(SceneName == "frog cave main") { 
                SetupFrogDomainSecret();
            } else if(SceneName == "Sword Access") {
                GameObject Bush = GameObject.Find("_Grass/bush (70)");
                if (Bush != null) {
                    Bush.SetActive(false);
                }
            }

            if (SaveFile.GetInt("randomizer entrance rando enabled") == 1) {
                if (IsArchipelago()) {
                    TunicPortals.CreatePortalPairs(((JObject)Archipelago.instance.GetPlayerSlotData()["Entrance Rando"]).ToObject<Dictionary<string, string>>());
                } else if (IsSinglePlayer()) {
                    TunicPortals.RandomizePortals(SaveFile.GetInt("seed"));
                }
                TunicPortals.ModifyPortals(loadingScene);
                TunicPortals.MarkPortals();
            }

            if (TunicRandomizer.Settings.EnemyRandomizerEnabled && EnemyRandomizer.Enemies.Count > 0 && !EnemyRandomizer.ExcludedScenes.Contains(SceneName)) {
                EnemyRandomizer.SpawnNewEnemies();
            }

            if (TunicRandomizer.Settings.ArachnophobiaMode) {
                EnemyRandomizer.ToggleArachnophobiaMode();
            }

            try {
                if (!ModelSwaps.SwappedThisSceneAlready && (ItemLookup.ItemList.Count > 0 || Locations.RandomizedLocations.Count > 0) && SaveFile.GetInt("seed") != 0) {
                    ModelSwaps.SwapItemsInScene();
                }
            } catch (Exception ex) {
                Logger.LogError("An error occurred swapping item models in this scene:");
                Logger.LogError(ex.Message + " " + ex.StackTrace);
            }

            if (SaveFile.GetInt(AbilityShuffle) == 1 && SaveFile.GetInt(HolyCrossUnlocked) == 0) {
                ItemPatches.ToggleHolyCrossObjects(false);
            }
            try {
                if (PlayerCharacter.instance != null) {
                    FairyTargets.CreateFairyTargets();
                    FairyTargets.CreateEntranceTargets();
                    FairyTargets.FindFairyTargets();
                }
            } catch (Exception ex) {
                Logger.LogError("An error occurred creating new fairy seeker spell targets:");
                Logger.LogError(ex.Message + " " + ex.StackTrace);
            }

            try {
                if (TunicRandomizer.Settings.UseCustomTexture) {
                    PaletteEditor.LoadCustomTexture();
                }
            } catch (Exception ex) {
                Logger.LogError("An error occurred applying custom texture:");
                Logger.LogError(ex.Message + " " + ex.StackTrace);
            }

            if (TunicRandomizer.Settings.RealestAlwaysOn) {
                try {
                    GameObject.FindObjectOfType<RealestSpell>().SpellEffect();
                } catch (Exception e) {
                }
            }

            if (Hints.HeroGraveHints.Count != 0) {
                Hints.SetupHeroGraveToggle();
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

            if (IsArchipelago()) {
                Archipelago.instance.integration.UpdateDataStorageOnLoad();
            }

            ItemTracker.SaveTrackerFile();
        }

        private static void SpawnHeirFastTravel(string SceneName, Vector3 position) {
            GameObject gameObject = GameObject.Instantiate<GameObject>(SpiritArenaTeleporterPrefab, position, SpiritArenaTeleporterPrefab.transform.rotation);
            ScenePortal scenePortal = gameObject.transform.GetComponentInChildren<ScenePortal>();
            scenePortal.id = "heirfasttravel_spawnid";
            scenePortal.destinationSceneName = SceneName;
            if (SceneManager.GetActiveScene().name == "Spirit Arena") {
                scenePortal.spawnTransform = GameObject.Find("Teleporter").transform.GetChild(0).GetChild(0).GetChild(0);
                gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            } else {
                scenePortal.spawnTransform = gameObject.transform.GetChild(0).GetChild(0).GetChild(0);
            }
            scenePortal.optionalIDToSpawnAt = "";
            gameObject.SetActive(true);
        }

        public static void SetupOldHouseRelicToggles() {
            if (SceneManager.GetActiveScene().name == "Overworld Interiors" && GameObject.Find("_Offerings") != null && GameObject.Find("_Offerings").transform.childCount >= 5) {
                GameObject Offerings = GameObject.Find("_Offerings");
                GameObject.Destroy(Offerings.transform.GetChild(2).GetChild(1).gameObject);
                GameObject.Destroy(Offerings.transform.GetChild(2).GetChild(2).gameObject);
                List<string> relics = new List<string>() { "Relic - Hero Water", "Relic - Hero Crown", "Relic - Hero Pendant SP", "Relic - Hero Pendant HP", "Relic - Hero Pendant MP", "Relic - Hero Sword" };
                for (int i = 0; i < 6; i++) {
                    GameObject Offering = Offerings.transform.GetChild(i).gameObject;
                    if (Offering.GetComponent<StatefulActive>() != null) {
                        GameObject.Destroy(Offering.GetComponent<StatefulActive>());
                    }
                    Offering.AddComponent<VisibleByHavingInventoryItem>().enablingItem = Inventory.GetItemByName(relics[i]);
                    Offering.GetComponent<VisibleByHavingInventoryItem>().renderers = Offering.GetComponentsInChildren<Renderer>().ToArray();
                    Offering.SetActive(true);
                }
            }
        }

        public static void SetupFrogDomainSecret() {
            GameObject Plinth = GameObject.Find("_DR: Questagon Room/hexagon plinth/");
            if(SceneManager.GetActiveScene().name == "frog cave main" && Plinth != null) {
                GameObject CapeSecret = GameObject.Instantiate(ModelSwaps.StarburstEffect);
                CapeSecret.transform.parent = Plinth.transform;
                CapeSecret.name = "cape secret";
                CapeSecret.transform.localPosition = new Vector3(0, 1f, 0);
                CapeSecret.transform.localEulerAngles = Vector3.zero;
                CapeSecret.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                CapeSecret.transform.GetChild(0).localScale = new Vector3(0.5f, 0.5f, 0.5f);
                CapeSecret.AddComponent<VisibleByNotHavingItem>().Item = Inventory.GetItemByName("Cape");
                CapeSecret.SetActive(true);
            }
        }

        public static void PauseMenu___button_ReturnToTitle_PostfixPatch(PauseMenu __instance) {

            if (ItemStatsHUD.HexagonQuest != null) {
                ItemStatsHUD.HexagonQuest.SetActive(false);
            }
            SceneName = "TitleScreen";
        }

    }

}