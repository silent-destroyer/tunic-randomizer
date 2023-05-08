using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BepInEx.Logging;
using UnhollowerBaseLib;

namespace TunicRandomizer {
    public class EnemyRandomizer {

        private static ManualLogSource Logger = TunicRandomizer.Logger;

        public static Dictionary<string, GameObject> Enemies = new Dictionary<string, GameObject>() { };

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
                    "Voidtouched"
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
                    "Hedgehog Trap"
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
                    "woodcutter"
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
            },
        };

        public static void InitializeEnemies(string SceneName) {
            List<Monster> Monsters = Resources.FindObjectsOfTypeAll<Monster>().ToList();
            foreach (string LocationEnemy in LocationEnemies[SceneName]) {
                string EnemyName = LocationEnemy;
                if (EnemyName.Contains("(") && !EnemyName.Contains("(Dmg)")) { 
                    EnemyName = LocationEnemy.Split('(')[0].Trim();
                }
                if (EnemyName == "_Turret") {
                    EnemyName = "Turret";
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

            }

        }

        public static void SpawnNewEnemies() {
            System.Random Random = new System.Random();
            List<GameObject> Monsters = Resources.FindObjectsOfTypeAll<GameObject>().Where(Monster => (Monster.GetComponent<Monster>() != null || Monster.GetComponent<TurretTrap>() != null) && Monster.transform.parent != null && !Monster.transform.parent.name.Contains("split tier") && !ExcludedEnemies.Contains(Monster.name)).ToList();
            if (SceneLoaderPatches.SceneName == "Archipelagos Redux") {
                Monsters = Monsters.Where(Monster => Monster.transform.parent.parent == null || Monster.transform.parent.parent.name != "_Environment Prefabs").ToList();
            }
            if (SceneLoaderPatches.SceneName == "Forest Belltower") {
                Monsters = Resources.FindObjectsOfTypeAll<GameObject>().Where(Monster => (Monster.GetComponent<Monster>() != null || Monster.GetComponent<TurretTrap>() != null) && !Monster.name.Contains("Clone")).ToList();
            }
            if (TunicRandomizer.Settings.ExtraEnemiesEnabled && SceneLoaderPatches.SceneName == "Library Hall" && !CycleController.IsNight) { 
                GameObject.Find("beefboy statues").SetActive(false);
                GameObject.Find("beefboy statues (2)").SetActive(false);
                foreach (GameObject Monster in Monsters) {
                    Monster.transform.parent = null;
                }
            }
            if (TunicRandomizer.Settings.ExtraEnemiesEnabled && SceneLoaderPatches.SceneName == "Monastery") {
                Resources.FindObjectsOfTypeAll<Voidtouched>().ToList()[0].gameObject.transform.parent = null;
            }

            foreach (GameObject Enemy in Monsters) {
                if (TunicRandomizer.Settings.ExtraEnemiesEnabled) {
                    if (Enemy.transform.parent != null && Enemy.transform.parent.name.Contains("NG+")) {
                        Enemy.transform.parent.gameObject.SetActive(true);
                    }
                }
                GameObject NewEnemy;

                if (TunicRandomizer.Settings.EnemyGeneration == RandomizerSettings.EnemyRandomizationType.RANDOM || SceneLoaderPatches.SceneName == "Cathedral Arena") {
                    NewEnemy = GameObject.Instantiate(Enemies[Enemies.Keys.ToList()[Random.Next(Enemies.Count)]]);
                } else if (TunicRandomizer.Settings.EnemyGeneration == RandomizerSettings.EnemyRandomizationType.BALANCED) {
                    List<string> EnemyTypes = null;
                    foreach (string Key in EnemyRankings.Keys.Reverse()) {
                        List<string> Rank = EnemyRankings[Key];
                        Rank.Sort();
                        Rank.Reverse();
                        foreach (string EnemyName in Rank) {
                            if (Enemy.name.Contains(EnemyName)) {
                                EnemyTypes = Rank;
                            }
                        }
                        if (EnemyTypes != null) {
                            break;
                        }
                    }
                    if (EnemyTypes == null) {
                        NewEnemy = GameObject.Instantiate(Enemies[Enemies.Keys.ToList()[Random.Next(Enemies.Count)]]);
                    } else {
                        NewEnemy = GameObject.Instantiate(Enemies[EnemyTypes[Random.Next(EnemyTypes.Count)]]);
                    }
                } else {
                    NewEnemy = GameObject.Instantiate(Enemies[Enemies.Keys.ToList()[Random.Next(Enemies.Count)]]);
                }

                NewEnemy.transform.position = Enemy.transform.position;
                NewEnemy.transform.rotation = Enemy.transform.rotation;
                NewEnemy.transform.parent = Enemy.transform.parent;
                NewEnemy.SetActive(true);
                GameObject.Destroy(Enemy.gameObject);
            }

            foreach (string Key in Enemies.Keys) {
                Enemies[Key].SetActive(false);
            }

        }

    }
}
