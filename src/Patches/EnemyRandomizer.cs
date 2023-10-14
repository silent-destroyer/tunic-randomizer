using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BepInEx.Logging;
using UnhollowerBaseLib;
using UnityEngine.SceneManagement;

namespace TunicRandomizer {

    public class EnemyRandomizer {

        private static ManualLogSource Logger = TunicRandomizer.Logger;

        public static Dictionary<string, GameObject> Enemies = new Dictionary<string, GameObject>() { };

        public static Dictionary<string, List<string>> DefeatedEnemyTracker = new Dictionary<string, List<string>>();

        public static Dictionary<string, string> EnemiesInCurrentScene = new Dictionary<string, string>() { };

        public static List<string> ExcludedEnemies = new List<string>() {
            "tech knight boss",
            "Spidertank",
            "Scavenger Boss"
        };

        public static List<string> ExcludedScenes = new List<string>() {
            "Library Arena",
            "Fortress Arena",
            "Spirit Arena"
        };

        public static Dictionary<string, List<string>> LocationEnemies = new Dictionary<string, List<string>>() {
            {
                "Overworld Redux",
                new List<string>() {
                    "Blob",
                    "BlobBig",
                    "Hedgehog",
                    "HedgehogBig",
                    "Bat",
                    "Skuladot redux",
                    "Skuladot redux_shield",
                    "Skuladot redux Big",
                    "Honourguard",
                    "beefboy",
                    "bomezome big",
                    "Fox enemy zombie",
                    "_Turret",
                }
            },
            {
                "Archipelagos Redux",
                new List<string>() {
                    "crocodoo Voidtouched",
                    "crocodoo",
                    "Fairyprobe Archipelagos (2)",
                    "Fairyprobe Archipelagos (Dmg)",
                    "tunic knight void"
                }
            },
            {
                "Atoll Redux",
                new List<string>() {
                    "plover",
                    "Crow",
                    "Crabbit",
                    "Crabbo (1)",
                    "Crabbit with Shell",
                    "Spinnerbot Corrupted",
                    "Spinnerbot Baby",
                }
            },
            {
                "frog cave main",
                new List<string>() {
                    "Frog Small",
                    "Frog Spear",
                    "Frog (7)",
                    "Wizard_Support"
                }
            },
            {
                "Fortress Basement",
                new List<string>() {
                    "Spider Small",
                    "Spider Big",
                    "Wizard_Sword",
                    "Wizard_Candleabra",
                }
            },
            {
                "Quarry",
                new List<string>() {
                    "Scavenger_stunner"
                }
            },
            {
                "Quarry Redux",
                new List<string>() {
                    "Scavenger",
                    "Scavenger_miner",
                    "Scavenger_support"
                }
            },
            {
                "Swamp Redux 2",
                new List<string>() {
                    "bomezome_easy",
                    "bomezome_fencer",
                    "Ghostfox_monster",
                    "Gunslinger",
                    "sewertentacle",
                    "Crow Voidtouched"
                }
            },
            {
                "Cathedral Arena",
                new List<string>() {
                    "tech knight ghost",
                }
            },
            {
                "ziggurat2020_1",
                new List<string>() {
                    "administrator",
                }
            },
            {
                "Fortress Reliquary",
                new List<string> () {
                    "voidling redux"
                }
            },
            {
                "Fortress Main",
                new List<string> () { 
                    "woodcutter"
                }
            },
            {
                "Cathedral Redux",
                new List<string> () {
                    "Voidtouched",
                    "Fox enemy"
                }
            },
            {
                "Library Hall",
                new List<string> () {
                    "administrator_servant"
                }
            },
            { 
                "Posterity",
                new List<string> () {
                    "Phage",
                    "Ghost Knight"
                }
            }
        };

        public static Dictionary<string, List<string>> EnemyRankings = new Dictionary<string, List<string>>() {
            {
                "Average",
                new List<string>() {
                    "Blob",
                    "Hedgehog",
                    "Skuladot redux",
                    "plover",
                    "Spinnerbot Baby",
                    "Crabbit",
                    "Crabbit with Shell",
                    "Fox enemy zombie",
                    "BlobBig",
                    "BlobBigger",
                    "HedgehogBig",
                    "Bat",
                    "Spider Small",
                    "bomezome_easy",
                    "Fairyprobe Archipelagos",
                    "Fairyprobe Archipelagos (Dmg)",
                    "Skuladot redux_shield",
                    "Crabbo",
                    "Spinnerbot Corrupted",                          
                    "Turret",
                    "Hedgehog Trap",
                    "administrator_servant",
                    "Phage",
                    "Ghost Knight"
                }
            },
            {
                "Strong",
                new List<string>() {
                    "Wizard_Candleabra",
                    "sewertentacle",
                    "Honourguard",
                    "Skuladot redux Big",
                    "Crow",
                    "crocodoo",
                    "crocodoo Voidtouched",
                    "Scavenger",
                    "Scavenger_miner",
                    "Scavenger_support",
                    "Scavenger_stunner",
                    "bomezome_fencer",
                    "Ghostfox_monster",
                    "voidling redux",
                    "Frog Spear",
                    "Frog",
                    "Frog Small",
                    "Spider Big",
                    "Wizard_Sword",
                    "Wizard_Support",
                    "Crow Voidtouched",
                    "woodcutter",
                    "Fox enemy"
                }
            },
            {
                "Intense",
                new List<string>() {
                    "administrator",
                    "Gunslinger",
                    "beefboy",
                    "bomezome big",
                    "tech knight ghost",
                    "tunic knight void",
                    "Voidtouched"
                }
            }
        };

        public static Dictionary<string, string> ProperEnemyNames = new Dictionary<string, string>() {
            { "Blob", $"\"Blob\"" },
            { "Hedgehog", $"\"Hedgehog\"" },
            { "Skuladot redux", $"\"Rudeling\"" },
            { "plover", $"\"Plover\"" },
            { "Spinnerbot Baby", $"\"Baby Slorm\"" },
            { "Crabbit", $"\"Crabbit\"" },
            { "Crabbit with Shell", $"gAmkyoob \"Crabbit\"" },
            { "Fox enemy zombie", $"yoo...\"?\"" },
            { "BlobBig", $"\"Blob\" (big)" },
            { "BlobBigger", $"\"Blob\" (bigur)" },
            { "HedgehogBig", $"\"Hedgehog\" (big)" },
            { "Bat", $"\"Phrend\"" },
            { "Spider Small", $"\"Spyrite\"" },
            { "bomezome_easy", $"\"Fleemer\"" },
            { "Fairyprobe Archipelagos", $"\"Fairy\" (Is)" },
            { "Fairyprobe Archipelagos (Dmg)", $"\"Fairy\" (fIur)" },
            { "Skuladot redux_shield", $"\"Rudeling\" ($Eld)" },
            { "Crabbo", $"\"Crabbo\"" },
            { "Spinnerbot Corrupted", $"\"Slorm\"" },
            { "Turret", $"\"Autobolt\"" },
            { "Hedgehog Trap", $"\"Laser Turret\"" },
            { "administrator_servant", $"\"Administrator\" (frehnd)" },
            { "Phage", $"slorm...\"?\"" },
            { "Ghost Knight", $"\"???\"" },
            { "Wizard_Candleabra", $"\"Custodian\" (kahnduhlahbruh)" },
            { "sewertentacle", $"\"Tentacle\"" },
            { "Honourguard", $"\"Envoy\"" },
            { "Skuladot redux Big", $"\"Guard Captain\"" },
            { "Crow", $"\"Husher\"" },
            { "crocodoo", $"\"Terry\"" },
            { "crocodoo Voidtouched", $"\"Terry\" (void)" },
            { "Scavenger", $"\"Scavenger\" (snIpur)" },
            { "Scavenger_miner", $"\"Scavenger\" (mInur)" },
            { "Scavenger_support", $"\"Scavenger\" (suhport)" },
            { "Scavenger_stunner", $"\"Scavenger\" (stuhnur)" },
            { "bomezome_fencer", $"\"Fleemer\" (fehnsur)" },
            { "Ghostfox_monster", $"\"Lost Echo\"" },
            { "voidling redux", $"\"Voidling\"" },
            { "Frog Spear", $"\"Frog\" (spEr) [frog]" },
            { "Frog", $"\"Frog\" [frog]" },
            { "Frog Small", $"\"Frog\" (smawl) [frog]" },
            { "Spider Big", $"\"Sappharach\"" },
            { "Wizard_Sword", $"\"Custodian\"" },
            { "Wizard_Support", $"\"Custodian\" (suhport)" },
            { "Crow Voidtouched", $"\"Husher\" (void)" },
            { "woodcutter", $"\"Woodcutter\"" },
            { "Fox enemy", $"\"You...?\"" },
            { "administrator", $"\"Administrator\"" },
            { "Gunslinger", $"\"Gunslinger\"" },
            { "beefboy", $"\"Beefboy\"" },
            { "bomezome big", $"\"Fleemer\" (lRj)" },
            { "tech knight ghost", $"\"Garden Knight...?\"" },
            { "tunic knight void", $"gRdin nIt...\"?\"" },
            { "Voidtouched", $"\"Voidtouched\"" },
        };

        public static void CreateAreaSeeds() { 
            System.Random Random = new System.Random(SaveFile.GetInt("seed"));
            foreach (String Scene in Hints.AllScenes) {
                SaveFile.SetInt($"randomizer enemy seed {Scene}", Random.Next());
            }
        }

        public static void InitializeEnemies(string SceneName) {
            List<Monster> Monsters = Resources.FindObjectsOfTypeAll<Monster>().ToList();
            Dictionary<string, string> RenamedEnemies = new Dictionary<string, string>() {
                { "_Turret", "Turret" },
                { "Fairyprobe Archipelagos (2)", "Fairyprobe Archipelagos" },
                { "Crabbo (1)", "Crabbo" },
                { "Frog (7)", "Frog" },
                { "Hedgehog Trap (1)", "Hedgehog Trap" }

            };
            foreach (string LocationEnemy in LocationEnemies[SceneName]) {
                string EnemyName = LocationEnemy;
                if (RenamedEnemies.ContainsKey(EnemyName)) { 
                    EnemyName = RenamedEnemies[EnemyName];
                }
                if (EnemyName == "voidling redux") {
                    Enemies[EnemyName] = GameObject.Instantiate(Monsters.Where(Monster => Monster.name == LocationEnemy && Monster.transform.parent.name == "_Night Encounters").ToList()[0].gameObject);
                    Enemies[EnemyName].GetComponent<Voidling>().replacementMonster = null;

                } else { 
                    Enemies[EnemyName] = GameObject.Instantiate(Monsters.Where(Monster => Monster.name == LocationEnemy).ToList()[0].gameObject);
                }
                GameObject.DontDestroyOnLoad(Enemies[EnemyName]);
                Enemies[EnemyName].SetActive(false);

                Enemies[EnemyName].transform.position = new Vector3(-30000f, -30000f, -30000f);
                if (EnemyName == "Blob") {
                    Enemies["BlobBigger"] = GameObject.Instantiate(Monsters.Where(Monster => Monster.name == LocationEnemy).ToList()[0].gameObject);
                    Enemies["BlobBigger"].transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                    GameObject.DontDestroyOnLoad(Enemies["BlobBigger"]);
                    Enemies["BlobBigger"].SetActive(false);
                    Enemies["BlobBigger"].transform.position = new Vector3(-30000f, -30000f, -30000f);
                    Enemies["BlobBigger"].name = "BlobBigger Prefab";
                    Enemies["BlobBigger"].GetComponent<Blob>().attackDistance = 3f;
                }

                Enemies[EnemyName].name = EnemyName + " Prefab";
            }
            if (SceneName == "Archipelagos Redux") {
                Enemies["tunic knight void"].GetComponent<ZTarget>().isActive = true;
                Enemies["tunic knight void"].GetComponent<CapsuleCollider>().enabled = true;
                for (int i = 0; i < Enemies["tunic knight void"].transform.childCount - 2; i++) {
                    Enemies["tunic knight void"].transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            if (SceneName == "Fortress Basement") {
                Enemies["Hedgehog Trap"] = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<TurretTrap>().ToList()[0].gameObject);
                GameObject.DontDestroyOnLoad(Enemies["Hedgehog Trap"]);
                Enemies["Hedgehog Trap"].SetActive(false);

                Enemies["Hedgehog Trap"].transform.position = new Vector3(-30000f, -30000f, -30000f);
                Enemies["Hedgehog Trap"].name = "Hedgehog Trap Prefab";
            }

        }

        public static void SpawnNewEnemies() {
            EnemiesInCurrentScene.Clear();
  
            string CurrentScene = SceneLoaderPatches.SceneName;

            System.Random Random;
            if (TunicRandomizer.Settings.EnemyGeneration == RandomizerSettings.EnemyGenerationType.SEEDED) {
                Random = new System.Random(SaveFile.GetInt($"randomizer enemy seed {CurrentScene}"));
            } else {
                Random = new System.Random();
            }
            List<GameObject> Monsters = Resources.FindObjectsOfTypeAll<GameObject>().Where(Monster => (Monster.GetComponent<Monster>() != null || Monster.GetComponent<TurretTrap>() != null) && Monster.transform.parent != null && !Monster.transform.parent.name.Contains("split tier") && !ExcludedEnemies.Contains(Monster.name) && !Monster.name.Contains("Prefab")).ToList();
            if (CurrentScene == "Archipelagos Redux") {
                Monsters = Monsters.Where(Monster => Monster.transform.parent.parent == null || Monster.transform.parent.parent.name != "_Environment Prefabs").ToList();
            }
            if (CurrentScene == "Forest Belltower") {
                Monsters = Resources.FindObjectsOfTypeAll<GameObject>().Where(Monster => (Monster.GetComponent<Monster>() != null || Monster.GetComponent<TurretTrap>() != null) && !Monster.name.Contains("Prefab")).ToList();
            }
            if (TunicRandomizer.Settings.ExtraEnemiesEnabled && CurrentScene == "Library Hall" && !CycleController.IsNight) { 
                GameObject.Find("beefboy statues").SetActive(false);
                GameObject.Find("beefboy statues (2)").SetActive(false);
                foreach (GameObject Monster in Monsters) {
                    Monster.transform.parent = null;
                }
            }
            if (CurrentScene == "Fortress East" || CurrentScene == "Frog Stairs") {
                Monsters = Resources.FindObjectsOfTypeAll<GameObject>().Where(Monster => (Monster.GetComponent<Monster>() != null || Monster.GetComponent<TurretTrap>() != null ) && !Monster.name.Contains("Prefab")).ToList();
            }
            if (CurrentScene == "Cathedral Redux") {
                Monsters.AddRange(Resources.FindObjectsOfTypeAll<GameObject>().Where(Monster => Monster.GetComponent<Crow>() != null && !Monster.name.Contains("Prefab")).ToList());
            }
            if (CurrentScene == "Forest Boss Room" && GameObject.Find("Skuladot redux") != null) {
                Monsters.Add(GameObject.Find("Skuladot redux"));
            }
            if (TunicRandomizer.Settings.ExtraEnemiesEnabled && CurrentScene == "Monastery") {
                Resources.FindObjectsOfTypeAll<Voidtouched>().ToList()[0].gameObject.transform.parent = null;
            }
            int i = 0;
            foreach (GameObject Enemy in Monsters) {
                GameObject NewEnemy = null;
                try {
                    List<string> EnemyKeys = Enemies.Keys.ToList();
                    if (CurrentScene == "Cathedral Arena") {
                        EnemyKeys.Remove("administrator_servant");
                        EnemyKeys.Remove("Hedgehog Trap");
                        if (Inventory.GetItemByName("Wand").Quantity == 0) {
                            EnemyKeys.Remove("Crabbit with Shell");
                        }
                        if (Enemy.transform.parent.name.Contains("Wave")) {
                            Enemy.transform.position = Vector3.zero;
                            
                        }
                    }
                    if (CurrentScene == "ziggurat2020_1" && Enemy.GetComponent<Administrator>() != null) {
                        EnemyKeys.Remove("administrator_servant");
                        EnemyKeys.Remove("Hedgehog Trap");
                    }
                    if (CurrentScene == "Forest Boss Room" && Enemy.GetComponent<BossAnnounceOnAggro>() != null) {
                        EnemyKeys.Remove("administrator_servant");
                        EnemyKeys.Remove("Hedgehog Trap");
                    }
                    if (CurrentScene == "Sewer" && Enemy.name.Contains("Spinnerbot") && !Enemy.name.Contains("Corrupted")) {
                        Enemy.name = Enemy.name.Replace("Spinnerbot", "Spinnerbot Corrupted");
                    }
                    if (TunicRandomizer.Settings.ExtraEnemiesEnabled) {
                        if (Enemy.transform.parent != null && Enemy.transform.parent.name.Contains("NG+")) {
                            Enemy.transform.parent.gameObject.SetActive(true);
                        }
                    }
                    string NewEnemyName = "";
                    if (TunicRandomizer.Settings.EnemyDifficulty == RandomizerSettings.EnemyRandomizationType.RANDOM) {
                        NewEnemy = GameObject.Instantiate(Enemies[EnemyKeys[Random.Next(EnemyKeys.Count)]]);
                    } else if (TunicRandomizer.Settings.EnemyDifficulty == RandomizerSettings.EnemyRandomizationType.BALANCED) {
                        List<string> EnemyTypes = null;
                        foreach (string Key in EnemyRankings.Keys.Reverse()) {
                            List<string> Rank = EnemyRankings[Key];
                            Rank.Sort();
                            Rank.Reverse();
                            foreach (string EnemyName in Rank) {
                                if (Enemy.name.Contains(EnemyName)) {
                                    if (Enemy.name.Contains("Voidtouched")) {
                                        if (Enemy.name.Contains("Crow") || Enemy.name.Contains("crocodoo")) {
                                            EnemyTypes = EnemyRankings["Strong"];
                                        } else {
                                            EnemyTypes = EnemyRankings["Intense"];
                                        }
                                    } else if (Enemy.name.Contains("administrator")) {
                                        EnemyTypes = Enemy.name.Contains("servant") ? EnemyRankings["Average"] : EnemyRankings["Intense"];
                                    } else {
                                        EnemyTypes = Rank;
                                    }
                                }
                            }
                            if (EnemyTypes != null) {
                                break;
                            }
                        }
                        if (EnemyTypes == null) {
                            NewEnemy = GameObject.Instantiate(Enemies[EnemyKeys[Random.Next(EnemyKeys.Count)]]);
                        } else {
                            if (CurrentScene == "Cathedral Arena") {
                                EnemyTypes.Remove("administrator_servant");
                                EnemyTypes.Remove("Hedgehog Trap");
                                if (Inventory.GetItemByName("Wand").Quantity == 0) {
                                    EnemyTypes.Remove("Crabbit with Shell");
                                }
                            }
                            NewEnemy = GameObject.Instantiate(Enemies[EnemyTypes[Random.Next(EnemyTypes.Count)]]);
                        }
                    } else {
                        NewEnemy = GameObject.Instantiate(Enemies[EnemyKeys[Random.Next(EnemyKeys.Count)]]);
                    }

                    NewEnemy.transform.position = Enemy.transform.position;
                    NewEnemy.transform.rotation = Enemy.transform.rotation;
                    NewEnemy.transform.parent = Enemy.transform.parent;
                    NewEnemy.SetActive(true);
                    if (NewEnemy.GetComponent<DefenseTurret>() != null) {
                        NewEnemy.GetComponent<Monster>().onlyAggroViaTrigger = false;
                    }
                    if (NewEnemy.GetComponent<TunicKnightVoid>() != null && NewEnemy.GetComponent<Creature>().defaultStartingMaxHP != null) {
                        NewEnemy.GetComponent<Creature>().defaultStartingMaxHP._value = 200;
                    }
                    if (NewEnemy.name.Contains("BlobBigger") && NewEnemy.GetComponent<Creature>().defaultStartingMaxHP != null) {
                        NewEnemy.GetComponent<Creature>().defaultStartingMaxHP._value = 25;
                    }
                    if (SceneLoaderPatches.SceneName == "ziggurat2020_1" && Enemy.GetComponent<Administrator>() != null) {
                        GameObject.FindObjectOfType<ZigguratAdminGate>().admin = NewEnemy.GetComponent<Monster>();
                    }
                    if (SceneLoaderPatches.SceneName == "Forest Boss Room" && Enemy.GetComponent<BossAnnounceOnAggro>() != null) {
                        NewEnemy.AddComponent<BossAnnounceOnAggro>();
                        LanguageLine TopLine = ScriptableObject.CreateInstance<LanguageLine>(); 
                        LanguageLine BottomLine = ScriptableObject.CreateInstance<LanguageLine>();
                        TopLine.text = $"\"Enemy\"";
                        foreach (string Key in ProperEnemyNames.Keys) {
                            if (NewEnemy.name.Replace(" Prefab", "").Replace("(Clone)", "") == Key) {
                                TopLine.text = Translations.TranslateDefaultNoQuotes(ProperEnemyNames[Key]);
                                if (NewEnemy.name.Contains("crocodoo")) {
                                    BottomLine.text = $"#uh wuhn ahnd OnlE";
                                } else if (EnemyRankings["Average"].Contains(Key)) {
                                    BottomLine.text = $"pEs uhv kAk";
                                } else if (EnemyRankings["Strong"].Contains(Key)) {
                                    BottomLine.text = $"A straw^ ehnuhmE";
                                } else if (EnemyRankings["Intense"].Contains(Key)) {
                                    BottomLine.text = $"A formiduhbl fO";
                                } else {
                                    BottomLine.text = $"A formiduhbl fO";
                                }
                            }
                        }
                        NewEnemy.GetComponent<BossAnnounceOnAggro>().bossTitleTopLine = TopLine;
                        NewEnemy.GetComponent<BossAnnounceOnAggro>().bossTitleBottomLine = BottomLine;
                    }
                    NewEnemy.name += $" {i}";
                    EnemiesInCurrentScene.Add(NewEnemy.name, NewEnemy.transform.position.ToString());
                    i++;
                    if (DefeatedEnemyTracker.ContainsKey(CurrentScene) && DefeatedEnemyTracker[CurrentScene].Contains(Enemy.transform.position.ToString())) {
                        GameObject.Destroy(NewEnemy);
                    }
                    GameObject.Destroy(Enemy.gameObject);
                } catch (Exception ex) {
                    if (NewEnemy != null) {
                        GameObject.Destroy(NewEnemy);
                        Logger.LogError("An error occurred spawning the following randomized enemy: " + NewEnemy.name);
                        Logger.LogError(ex.Message + " " + ex.StackTrace);
                    }
                }
            }
            try {
                foreach (string Key in Enemies.Keys) {
                    if (Enemies[Key] != null) {
                        Enemies[Key].SetActive(false);
                    }
                }
            } catch (Exception e) { 
                
            }
        }

        public static bool Monster_Die_MoveNext_PrefixPatch(Monster._Die_d__77 __instance, ref bool __result) {
            if (SceneLoaderPatches.SceneName == "Forest Boss Room" && __instance.__4__this.GetComponent<BossAnnounceOnAggro>() != null) {
                StateVariable.GetStateVariableByName("SV_Forest Boss Room_Skuladot redux Big").BoolValue = true;
            }
            if (__instance.__4__this.GetComponent<TunicKnightVoid>() != null) {
                CoinSpawner.SpawnCoins(50, __instance.__4__this.transform.position);
                MPPickup.Drop(100f, __instance.__4__this.transform.position);
                GameObject.Destroy(__instance.__4__this.gameObject);
                return false;
            }

            return true;
        }

        public static void Monster_Die_MoveNext_PostfixPatch(Monster._Die_d__77 __instance, ref bool __result) {
            if (!__result) {
                int EnemiesDefeated = SaveFile.GetInt("randomizer enemies defeated");
                SaveFile.SetInt("randomizer enemies defeated", EnemiesDefeated + 1);

                string SceneName = SceneLoaderPatches.SceneName;
                if (TunicRandomizer.Settings.EnemyRandomizerEnabled && SceneName != "Cathedral Arena") {
                    if (!DefeatedEnemyTracker.ContainsKey(SceneName)) {
                        DefeatedEnemyTracker.Add(SceneName, new List<string>());
                    }
                    if (EnemiesInCurrentScene.ContainsKey(__instance.__4__this.name)) {
                        DefeatedEnemyTracker[SceneName].Add(EnemiesInCurrentScene[__instance.__4__this.name]);
                    }

                }
            }
        }

        public static bool Campfire_RespawnAtLastCampfire_PrefixPatch(Campfire __instance) {
            DefeatedEnemyTracker.Clear();
            return true;
        }

        public static bool Campfire_Interact_PrefixPatch(Campfire __instance) {
            DefeatedEnemyTracker.Clear();
            return true;
        }

        public static bool TunicKnightVoid_onFlinch_PrefixPatch(TunicKnightVoid __instance) {
            return false;
        }

    }
}
