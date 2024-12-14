using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {
    public class SceneLoaderPatches {
        
        public static string SceneName;
        public static int SceneId;
        public static float TimeOfLastSceneTransition = 0.0f;
        public static bool SpawnedGhosts = false;
        public static bool InitialLoadDone = false;

        public static GameObject SpiritArenaTeleporterPrefab;
        public static GameObject GlyphTowerTeleporterPrefab;

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

            if (SaveFile.GetInt(GrassRandoEnabled) == 1) {
                foreach(Grass grass in Resources.FindObjectsOfTypeAll<Grass>().Where(grass => grass.gameObject.scene.name == loadingScene.name)) {
                    string grassId = GrassRandomizer.getGrassGameObjectId(grass);
                    if (GrassRandomizer.GrassChecks.ContainsKey(grassId)) {
                        if (SaveFile.GetInt("randomizer picked up " + grassId) == 1 || (IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld && Archipelago.instance.integration.session.Locations.AllLocationsChecked.Contains(Locations.LocationIdToArchipelagoId[grassId]))) {
                            grass.goToDeadState();
                        }
                    }
                }
            }

            if (SaveFile.GetInt(BreakableShuffleEnabled) == 1) {
                foreach (SmashableObject breakable in Resources.FindObjectsOfTypeAll<SmashableObject>().Where(pot => pot.gameObject.scene.name == loadingScene.name)) {
                    string breakableId = BreakableShuffle.getBreakableGameObjectId(breakable.gameObject);
                    if (BreakableShuffle.BreakableChecks.ContainsKey(breakableId)) {
                        if (SaveFile.GetInt("randomizer picked up " + breakableId) == 1 || (IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld && Archipelago.instance.integration.session.Locations.AllLocationsChecked.Contains(Locations.LocationIdToArchipelagoId[breakableId]))) {
                            if (breakable.name == "Physical Post") {
                                GameObject.Destroy(breakable.gameObject.transform.parent.gameObject);
                            } else {
                                GameObject.Destroy(breakable.gameObject);
                            }
                        }
                    }
                }
                if (loadingScene.name == "Dusty") {
                    foreach (DustyPile leafPile in Resources.FindObjectsOfTypeAll<DustyPile>()) {
                        string breakableId = BreakableShuffle.getBreakableGameObjectId(leafPile.gameObject, isLeafPile: true);
                        if (SaveFile.GetInt("randomizer picked up " + breakableId) == 1 || (IsArchipelago() && TunicRandomizer.Settings.CollectReflectsInWorld && Archipelago.instance.integration.session.Locations.AllLocationsChecked.Contains(Locations.LocationIdToArchipelagoId[breakableId]))) {
                            // it doesn't increment on its own if you scatter it this way
                            DustyPile.scatteredCount++;
                            leafPile.scatter();
                        }
                    }
                }
            }

            if (PlayerCharacter.Instanced && SaveFile.GetInt("archipelago") == 1 && !Archipelago.instance.IsConnected() && SaveFile.GetString(SaveFlags.ArchipelagoHostname) != "" && SaveFile.GetInt(SaveFlags.ArchipelagoPort) != 0) {
                TunicRandomizer.Settings.ReadConnectionSettingsFromSaveFile();
                Archipelago.instance.SilentReconnect();
            }

            EnemyRandomizer.BossStateVars.ForEach(s => StateVariable.GetStateVariableByName(s).BoolValue = false);

            return true;
        }

        public static void SceneLoader_OnSceneLoaded_PostfixPatch(Scene loadingScene, LoadSceneMode mode, SceneLoader __instance) {

            ModelSwaps.SwappedThisSceneAlready = false;
            EnemyRandomizer.RandomizedThisSceneAlready = false;
            EnemyRandomizer.DidArachnophoiaModeAlready = false;
            SpawnedGhosts = false;

            CameraController.Flip = TunicRandomizer.Settings.CameraFlip;

            if (loadingScene.name == "Posterity" && !EnemyRandomizer.Enemies.ContainsKey("Phage")) {
                EnemyRandomizer.InitializeEnemies("Posterity");
                ModelSwaps.CreateOtherWorldItemBlocks();
                TunicLogger.LogInfo("Done loading resources!");
                SceneLoader.LoadScene("TitleScreen");
                return;
            }
            if (loadingScene.name == "Overworld Interiors" && GlyphTowerTeleporterPrefab == null) {
                GlyphTowerTeleporterPrefab = GameObject.Instantiate(GameObject.Find("Trophy Stuff").transform.GetChild(4).gameObject);
                GlyphTowerTeleporterPrefab.SetActive(false);
                GameObject.DontDestroyOnLoad(GlyphTowerTeleporterPrefab);
                SceneLoader.LoadScene("Posterity");
                return;
            }
            if (loadingScene.name == "Library Lab" && ModelSwaps.Chalkboard == null) {
                ModelSwaps.CreateChalkboard();
                SceneLoader.LoadScene("Overworld Interiors");
                return;
            }
            if (loadingScene.name == "Library Hall" && !EnemyRandomizer.Enemies.ContainsKey("administrator_servant")) {
                EnemyRandomizer.InitializeEnemies("Library Hall");
                SceneLoader.LoadScene("Library Lab");
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
            if (loadingScene.name == "Crypt Redux" && !EnemyRandomizer.Enemies.ContainsKey("bomezome_quartet")) {
                EnemyRandomizer.InitializeEnemies("Crypt Redux");
                SceneLoader.LoadScene("Quarry Redux");
                return;
            }
            if (loadingScene.name == "Crypt" && !EnemyRandomizer.Enemies.ContainsKey("Shadowreaper")) {
                EnemyRandomizer.InitializeEnemies("Crypt");
                SceneLoader.LoadScene("Crypt Redux");
                return;
            }
            if (loadingScene.name == "Fortress Arena" && !EnemyRandomizer.Enemies.ContainsKey("Spidertank")) {
                EnemyRandomizer.InitializeEnemies("Fortress Arena");
                SceneLoader.LoadScene("Crypt");
                return;
            }
            if (loadingScene.name == "Fortress Basement" && !EnemyRandomizer.Enemies.ContainsKey("Spider Small")) {
                EnemyRandomizer.InitializeEnemies("Fortress Basement");
                ModelSwaps.BlueFire = GameObject.Instantiate(GameObject.Find("Room - Big Room/Fortress wall lamp small unlit (1)/Fire/lamp fire"));
                ModelSwaps.BlueFire.SetActive(false);
                GameObject.DontDestroyOnLoad(ModelSwaps.BlueFire);
                SceneLoader.LoadScene("Fortress Arena");
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

                ModelSwaps.LadderGraphic = GameObject.Instantiate(GameObject.Find("_INTERACTABLES/ladder_raisable ladder shortcut/ladder graphic/"));
                ModelSwaps.LadderGraphic.SetActive(false);
                GameObject.DontDestroyOnLoad(ModelSwaps.LadderGraphic);
                ItemPresentationPatches.SetupLadderPresentation();

                SceneLoader.LoadScene("Sewer");
                return;
            }
            if (loadingScene.name == "DPADTesting" && DDRSpell.DPADPool == null) {
                DDRSpell.CopyDPADTester();
                SceneLoader.LoadScene("Atoll Redux");
                return;
            }
            if (loadingScene.name == "Archipelagos Redux" && ModelSwaps.GlowEffect == null) {
                ModelSwaps.SetupGlowEffect();
                EnemyRandomizer.InitializeEnemies("Archipelagos Redux");
                ModelSwaps.InstantiateFishingRod();
                SceneLoader.LoadScene("DPADTesting");
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
                EnemyRandomizer.InitializeEnemies("Spirit Arena");
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

                LadderToggles.CreateLadderItems();
                ModelSwaps.CreateConstructionObject();

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
                GrassRandomizer.LoadGrassChecks();
                BreakableShuffle.LoadBreakableChecks();
                PaletteEditor.OdinRounded = Resources.FindObjectsOfTypeAll<Font>().Where(Font => Font.name == "Odin Rounded").ToList()[0];
                SceneLoader.LoadScene("Overworld Redux");
                return;
            }

            if (Camera.main != null && Camera.main.gameObject.GetComponentInParent<CycleController>() == null) {
                Camera.main.transform.parent.gameObject.AddComponent<CycleController>();
            }

            TunicLogger.LogInfo("Entering scene " + loadingScene.name + " (" + loadingScene.buildIndex + ")");
            SceneName = loadingScene.name;
            SceneId = loadingScene.buildIndex;

            if (SceneName == "Overworld Redux" && (StateVariable.GetStateVariableByName("Has Been Betrayed").BoolValue &&
                StateVariable.GetStateVariableByName("Has Died To God").BoolValue) && SaveFile.GetInt(DiedToHeir) != 1 && SaveFile.GetInt(HexagonQuestEnabled) == 0 && SaveFile.GetString("randomizer game mode") != "VANILLA") {
                PlayerCharacterPatches.ResetDayNightTimer = 0.0f;
                SaveFile.SetString("last campfire scene name", "Overworld Redux");
                SaveFile.SetString("last campfire id", "checkpoint");
                TunicLogger.LogInfo("Resetting time of day to daytime!"); 
                SpawnHeirFastTravel("Spirit Arena", new Vector3(2.0801f, 43.5833f, -54.0065f));
            }

            PlayerCharacterPatches.StungByBee = false;
            PlayerCharacterPatches.TinierFox = false;

            // Fur, Puff, Details, Tunic, Scarf
            if (TunicRandomizer.Settings.RandomFoxColorsEnabled) {
                try {
                    PaletteEditor.RandomizeFoxColors();
                } catch (Exception e) {
                    TunicLogger.LogInfo("Error randomizing fox colors!");
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
                int denominator = SaveFile.GetInt(GrassRandoEnabled) == 1 ? 325 : 15;
                PlayerCharacterPatches.HeirAssistModeDamageValue = Locations.CheckedLocations.Values.ToList().Where(item => item).ToList().Count / denominator;
                if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                    Foxgod foxgod = GameObject.FindObjectOfType<Foxgod>();
                    foxgod.transform.GetChild(0).GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["Hexagon Gold"].GetComponent<MeshRenderer>().materials;
                    foxgod.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["Hexagon Gold"].GetComponent<MeshRenderer>().materials;
                }

                if (GetBool(Decoupled) && GetBool("Placed Hexagons ALL")) {
                    GameObject teleporter = null;
                    foreach(FoxgodArenaCutscenes c in Resources.FindObjectsOfTypeAll<FoxgodArenaCutscenes>().Where(c => c.teleporterCollider != null)) {
                        teleporter = c.teleporterCollider.gameObject;
                    }
                    if (teleporter != null) {
                        GameObject tpClone = GameObject.Instantiate(teleporter);
                        tpClone.transform.position = teleporter.transform.position;
                        tpClone.AddComponent<FoxgodDecoupledTeleporter>();
                        teleporter.transform.position = new Vector3(-10000f, -10000f, -10000f);
                        teleporter.GetComponentInChildren<ScenePortal>().spawnTransform.position = new Vector3(0, 0, -17);
                    }
                }

                SpawnHeirFastTravel("Overworld Redux", new Vector3(-30000f, -30000f, -30000f));
            } else if (SceneName == "Overworld Interiors") {
                GameObject.FindObjectOfType<BedToggle>().canBeUsed = StateVariable.GetStateVariableByName("false");
                
                if ((StateVariable.GetStateVariableByName("Has Been Betrayed").BoolValue || StateVariable.GetStateVariableByName("Has Died To God").BoolValue) && SaveFile.GetInt(HexagonQuestEnabled) == 0) {
                    InteractionPatches.SetupDayNightHourglass();
                }

                GameObject.Find("Trophy Stuff").transform.GetChild(4).gameObject.SetActive(true);

                SetupOldHouseRelicToggles();
                if (GameObject.Find("_Offerings/ash group/")) {
                    GameObject.Find("_Offerings/ash group/").transform.position = new Vector3(-24.2824f, 29.8f, -45.4f);
                }
            } else if (SceneName == "Forest Belltower") {
                SaveFile.SetInt("chest open 19", 0);
            } else if (SceneName == "TitleScreen") {
                InitialLoadDone = true;
                TitleVersion.Initialize();
                RecentItemsDisplay.SetupRecentItemsDisplay();
                if (!Archipelago.instance.integration.connected && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO) {
                    Archipelago.instance.Connect();
                }
            } else if (SceneName == "Temple") {
                if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                    foreach (GameObject Questagon in Resources.FindObjectsOfTypeAll<GameObject>().Where(Obj => Obj.name == "questagon" && Obj.scene.name == loadingScene.name)) {
                        Questagon.GetComponent<MeshRenderer>().materials = ModelSwaps.Items["Hexagon Gold"].GetComponent<MeshRenderer>().materials;
                        Questagon.GetComponent<MeshRenderer>().receiveShadows = false;
                    }
                }
                if (TunicRandomizer.Settings.HeroPathHintsEnabled) {
                    GameObject HintStatueGlow = GameObject.Instantiate(ModelSwaps.GlowEffect);
                    HintStatueGlow.SetActive(true);
                    HintStatueGlow.transform.position = new Vector3(13f, 0f, 49f);
                    HintStatueGlow.AddComponent<VisibleByNotHavingItem>().Item = Inventory.GetItemByName("Hyperdash");
                }

                GameObject.Instantiate(ModelSwaps.Chalkboard, new Vector3(23.0934f, 7.2261f, 65.0646f), new Quaternion(0, 0.701f, 0, 0.701f)).SetActive(true);
            } else if (SceneName == "Overworld Redux") {
                GameObject.Find("_Signposts/Signpost (3)/").GetComponent<Signpost>().message.text = $"#is wA too \"West Garden\"\n<#33FF33>[death] bEwAr uhv tArE [death]";
                GameObject.Find("_Environment Special/Door (1)/door/key twist").GetComponent<MeshRenderer>().materials = ModelSwaps.Items["Key (House)"].GetComponent<MeshRenderer>().materials;
                GameObject.Find("_Environment/_Decorations/Mailbox (1)/mailbox flag").AddComponent<MailboxFlag>();

                GameObject.Find("_Bridges-Day/log bridge/").GetComponent<DayNightBridge>().dayOrNight = StateVariable.GetStateVariableByName("Is Night").BoolValue ? DayNightBridge.DayNight.NIGHT : DayNightBridge.DayNight.DAY;
                GameObject.Find("_Bridges-Day/log bridge/").GetComponent<DayNightBridge>().updateActiveness();
                GameObject.Destroy(GameObject.Find("_Bridges-Day/log bridge/").GetComponent<DayNightBridge>());

                if (SaveFile.GetInt("seed") != 0 && (SaveFile.GetInt(LadderRandoEnabled) == 0 || Inventory.GetItemByName("Ladder to Swamp").Quantity == 1) && SaveFile.GetString("randomizer game mode") != "VANILLA") {
                    for(int i = 0; i < 3; i++) {
                        GameObject.Find("_Bridges-Night").transform.GetChild(i).gameObject.AddComponent<ToggleObjectByFuse>().fuseId = 1096;
                        GameObject.Find("_Bridges-Night").transform.GetChild(i).gameObject.GetComponent<ToggleObjectByFuse>().stateWhenClosed = i != 0;
                        GameObject.Find("_Bridges-Night").transform.GetChild(i).gameObject.SetActive(true);
                    }
                }

                if (TunicRandomizer.Settings.ClearEarlyBushes) {
                    int[] bushesToClear = new int[] { 7, 2, 16, 9, 23, 26, 47, 42, 58, 62, 64 };
                    foreach (int bush in bushesToClear) {
                        GameObject bushObj = GameObject.Find($"_Bush and Grass/bush ({bush})/");
                        if (bushObj != null) {
                            bushObj.GetComponent<Grass>().onKilled();
                            bushObj.GetComponent<Grass>().doClippingAnimation();
                        }
                    }
                }
                if (SaveFile.GetInt(DiedToHeir) == 1) {
                    SpawnHeirFastTravel("Spirit Arena", new Vector3(2.0801f, 43.5833f, -54.0065f));
                }
                if (TunicRandomizer.Settings.EnemyRandomizerEnabled) {
                    GameObject.Instantiate(EnemyRandomizer.TuningFork, new Vector3(-183.9852f, 1f, -79.4829f), new Quaternion(0, 0, 0, 0)).SetActive(true);
                    GameObject.Instantiate(EnemyRandomizer.TuningFork, new Vector3(-166.9155f, 1f, -72.0338f), new Quaternion(0, 0, 0, 0)).SetActive(true);
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

                GameObject SignpostHint = GameObject.Instantiate(ModelSwaps.Signpost, new Vector3(-3.6754f, -1.175f, -74.2004f), new Quaternion(0, 0.8206f, 0, -0.5715f));
                SignpostHint.transform.localScale = Vector3.one * 1.5f;
                SignpostHint.AddComponent<BoxCollider>();
                SignpostHint.SetActive(true);

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
                if (GameObject.FindObjectOfType<ToggleObjectBySpell>() != null) {
                    GameObject.FindObjectOfType<ToggleObjectBySpell>().minDistance = 36;
                }
            } else if (SceneName == "Maze Room") {
                foreach (Chest chest in Resources.FindObjectsOfTypeAll<Chest>().Where(chest => chest.name == "Chest: Fairy")) {
                    chest.transform.GetChild(4).gameObject.SetActive(false);
                }
            } else if(SceneName == "frog cave main") { 
                SetupFrogDomainSecret();
            } else if (SceneName == "Sewer_Boss") {
                SetupCryptSecret();
            } else if (SceneName == "Crypt") {
                SetupOldCryptStuff();
            } else if(SceneName == "Sword Access") {
                GameObject Bush = GameObject.Find("_Grass/bush (70)");
                if (Bush != null) {
                    Bush.SetActive(false);
                }
            } else if (SceneName == "Fortress Courtyard") {
                if (SaveFile.GetInt(GrassRandoEnabled) == 1) {
                    GameObject sign = GameObject.Instantiate(ModelSwaps.UnderConstruction);
                    sign.GetComponent<MeshFilter>().mesh = ModelSwaps.Signpost.GetComponent<MeshFilter>().mesh;
                    sign.GetComponent<MeshRenderer>().materials = ModelSwaps.Signpost.GetComponent<MeshRenderer>().materials;
                    sign.transform.position = new Vector3(72.7274f, 8.0417f, -7.0365f);
                    sign.transform.localEulerAngles = new Vector3(0, 270, 0);
                    sign.transform.localScale = Vector3.one * 1.25f;
                    sign.GetComponent<Signpost>().message = ScriptableObject.CreateInstance<LanguageLine>();
                    sign.GetComponent<Signpost>().message.text = $"[grass]  [arrow_right]\n\n[wand]  rehkuhmehndid.";
                    sign.SetActive(true);
                }
            } else if (SceneName == "Atoll Redux") {
                if (SaveFile.GetInt(GrassRandoEnabled) == 1) {
                    // Hide thw two unbreakable pieces of grass in grass rando
                    if (GameObject.Find("_GRASS/grass beach (154)/grass base (1)") != null && GameObject.Find("_GRASS/grass beach (154)/grass base (2)") != null) {
                        GameObject.Find("_GRASS/grass beach (154)/grass base (1)").SetActive(false);
                        GameObject.Find("_GRASS/grass beach (154)/grass base (2)").SetActive(false);
                    }
                }
            } else if (SceneName == "ziggurat2020_1" && SaveFile.GetInt(SaveFlags.EntranceRando) == 1) {
                SpawnZigSkipRecovery();
            }

            EnemyRandomizer.CheckBossState();

            if (SaveFile.GetInt(EntranceRando) == 1) {
                if (ERData.RandomizedPortals.Count == 0) {
                    if (IsArchipelago()) {
                        ERScripts.CreatePortalPairs(((JObject)Archipelago.instance.GetPlayerSlotData()["Entrance Rando"]).ToObject<Dictionary<string, string>>());
                    } else if (IsSinglePlayer()) {
                        ERScripts.RandomizePortals(SaveFile.GetInt("seed"));
                    }
                }
                ERScripts.ModifyPortals(loadingScene.name);
                PlayerCharacterSpawn.OnArrivalCallback += (Action)(() => {
                    ERScripts.ModifyPortals(SceneName, sending: true);
                });
                GhostHints.SpawnTorchHintGhost();
            } else {
                ERData.RandomizedPortals = ERScripts.VanillaPortals();
                ERScripts.ModifyPortalNames(loadingScene.name);
            }
            ERScripts.MarkPortals();
            TunicRandomizer.Tracker.PopulateDiscoveredEntrances();

            if (!EnemyRandomizer.RandomizedThisSceneAlready && SaveFile.GetInt("seed") != 0 && TunicRandomizer.Settings.EnemyRandomizerEnabled && EnemyRandomizer.Enemies.Count > 0 && !EnemyRandomizer.ExcludedScenes.Contains(SceneName)) {
                EnemyRandomizer.SpawnNewEnemies();
            }

            if (TunicRandomizer.Settings.ArachnophobiaMode && !EnemyRandomizer.DidArachnophoiaModeAlready) {
                EnemyRandomizer.ToggleArachnophobiaMode();
            }

            try {
                if (!ModelSwaps.SwappedThisSceneAlready && (ItemLookup.ItemList.Count > 0 || Locations.RandomizedLocations.Count > 0) && SaveFile.GetInt("seed") != 0) {
                    ModelSwaps.SwapItemsInScene();
                }
            } catch (Exception ex) {
                TunicLogger.LogError("An error occurred swapping item models in this scene:");
                TunicLogger.LogError(ex.Message + " " + ex.StackTrace);
            }

            if (SaveFile.GetInt(AbilityShuffle) == 1 && SaveFile.GetInt(HolyCrossUnlocked) == 0) {
                ItemPatches.ToggleHolyCrossObjects(false);
            }
            try {
                if (PlayerCharacter.instance != null) {
                    TunicUtils.FindChecksInLogic();
                    FairyTargets.CreateFairyTargets();
                    FairyTargets.CreateEntranceTargets();
                    FairyTargets.FindFairyTargets();
                }
            } catch (Exception ex) {
                TunicLogger.LogError("An error occurred creating new fairy seeker spell targets:");
                TunicLogger.LogError(ex.Message + " " + ex.StackTrace);
            }

            try {
                if (SaveFile.GetInt(LadderRandoEnabled) == 1) {
                    LadderToggles.ToggleLadders();
                }
            } catch (Exception e) {
                TunicLogger.LogError("Error toggling ladders! " + e.Source + " " + e.Message + " " + e.StackTrace);
            }

            try {
                if (TunicRandomizer.Settings.UseCustomTexture) {
                    PaletteEditor.LoadCustomTexture();
                }
            } catch (Exception ex) {
                TunicLogger.LogError("An error occurred applying custom texture:");
                TunicLogger.LogError(ex.Message + " " + ex.StackTrace);
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
                TunicLogger.LogError("An error occurred spawning hint ghost foxes:");
                TunicLogger.LogError(ex.Message + " " + ex.StackTrace);
            }

            if (IsArchipelago()) {
                Archipelago.instance.integration.UpdateDataStorageOnLoad();
                Archipelago.instance.integration.SendQueuedLocations();
            }
            if (GameObject.FindObjectOfType<DDRSpell>() != null) {
                GameObject.FindObjectOfType<DDRSpell>().spellToggles = GameObject.FindObjectsOfType<ToggleObjectBySpell>().ToArray();
            }
            ItemTracker.SaveTrackerFile();

            if (SaveFile.GetInt("seed") != 0 && TunicRandomizer.Settings.CreateSpoilerLog && !TunicRandomizer.Settings.RaceMode) {
                ItemTracker.PopulateSpoilerLog();
            }
        }

        private static void SpawnHeirFastTravel(string SceneName, Vector3 position) {
            GameObject gameObject = GameObject.Instantiate<GameObject>(SpiritArenaTeleporterPrefab, position, SpiritArenaTeleporterPrefab.transform.rotation);
            ScenePortal scenePortal = gameObject.transform.GetComponentInChildren<ScenePortal>();
            scenePortal.id = "customfasttravel_spawnid";
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

        private static void SpawnZigSkipRecovery() {
            GameObject gameObject = GameObject.Instantiate<GameObject>(SpiritArenaTeleporterPrefab, new Vector3(207f, 750f, -126f), SpiritArenaTeleporterPrefab.transform.rotation);
            ScenePortal scenePortal = gameObject.transform.GetComponentInChildren<ScenePortal>();
            scenePortal.id = "zig_skip_recovery";
            scenePortal.optionalIDToSpawnAt = "zig2_skip";
            scenePortal.destinationSceneName = "ziggurat2020_1";
            scenePortal.name = "Zig Skip Recovery";
            scenePortal.spawnTransform = gameObject.transform.GetChild(0).GetChild(0).GetChild(0);
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
                    Offering.GetComponent<VisibleByHavingInventoryItem>().lights = new Light[] { };
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

        public static void SetupCryptSecret() {
            GameObject portal = GameObject.Instantiate(GlyphTowerTeleporterPrefab);
            portal.transform.position = new Vector3(100.3333f, 9.3138f, -10.201f);
            portal.transform.localEulerAngles = new Vector3(0f, 270f, 0f);
            portal.transform.localScale = Vector3.one;
            portal.GetComponentInChildren<ScenePortal>().id = "customfasttravel_spawnid";
            portal.GetComponentInChildren<ScenePortal>().destinationSceneName = "Crypt";
            portal.SetActive(true);

            GameObject spawn = new GameObject("crypt spawn");
            spawn.AddComponent<PlayerCharacterSpawn>();
            spawn.GetComponent<PlayerCharacterSpawn>().id = "Crypt_";
            spawn.transform.position = GameObject.FindObjectOfType<Campfire>().transform.position;
            spawn.SetActive(true);
        }

        public static void SetupOldCryptStuff() {
            foreach (ScenePortal portal in GameObject.FindObjectsOfType<ScenePortal>()) {
                portal.destinationSceneName = "Sewer_Boss";
            }
            foreach (Spiketrap trap in Resources.FindObjectsOfTypeAll<Spiketrap>()) {
                trap.gameObject.SetActive(true);
            }
            foreach (Monster monster in Resources.FindObjectsOfTypeAll<Monster>().Where(monster => monster.gameObject.scene.name == "Crypt")) {
                monster.gameObject.SetActive(true);
            }
            GameObject spawn = new GameObject("crypt spawn");
            spawn.AddComponent<PlayerCharacterSpawn>();
            spawn.GetComponent<PlayerCharacterSpawn>().id = "Sewer_Boss_customfasttravel_spawnid";
            spawn.transform.position = new Vector3(-79.3f, 57f, -30.8f);
            spawn.SetActive(true);
            GameObject.FindObjectOfType<ToggleObjectBySpell>().stateVar = StateVariable.GetStateVariableByName("randomizer crypt secret filigree door opened");
            GameObject.Instantiate(ModelSwaps.UnderConstruction, new Vector3(-72.0534f, 57, -15.2989f), new Quaternion(0, 0.7071f, 0, 0.7071f)).SetActive(true);
            foreach (UnderConstruction sign in GameObject.FindObjectsOfType<UnderConstruction>()) {
                sign.message = ScriptableObject.CreateInstance<LanguageLine>();
                sign.message.text = "\"???\"";
            }
        }

        public static void PauseMenu___button_ReturnToTitle_PostfixPatch(PauseMenu __instance) {

            if (InventoryDisplayPatches.HexagonQuest != null) {
                InventoryDisplayPatches.HexagonQuest.SetActive(false);
            }
            if (InventoryDisplayPatches.GrassCounter != null) {
                InventoryDisplayPatches.GrassCounter.SetActive(false);
            }
            if (IsArchipelago()) {
                Archipelago.instance.integration.SendQueuedLocations();
            }
            SceneName = "TitleScreen";
        }

    }

}