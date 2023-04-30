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
            },{
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
                }
            },
            {
                "Fortress Basement",
                new List<string>() {
                    "Spider Small",
                    "Spider Big",
                    "Wizard_Sword",
                    "Wizard_Candleabra",
                    //"Hedgehog Trap",*//*
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
            }
/*            





*/
        };

        public static Dictionary<string, List<string>> EnemyRankings = new Dictionary<string, List<string>>() {
            {
                "Weak",
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
                }
            },
            {
                "Average",
                new List<string>() {
                    "Bat",
                    "crocodoo",                 
                    "crocodoo Voidtouched",
                    "Spider Small",
                    "bomezome_easy",
                    "Fairyprobe Archipelagos (2)",
                    "Fairyprobe Archipelagos (Dmg)",
                    "Skuladot redux_shield",
                    "Crabbo (1)",
                    "Spinnerbot Corrupted",
                    "Frog Small",                                        
                    "_Turret",

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
                    "Scavenger",
                    "Scavenger_miner",
                    "Scavenger_support",
                    "bomezome_fencer",
                    "Ghostfox_monster",
                    "voidling redux",
                    "Frog Spear",
                    "Frog (7)",
                    "Spider Big",
                    "Wizard_Sword",
                }
            },
            {
                "Intense",
                new List<string>() {
                    "administrator",
                    "Gunslinger",
                    "beefboy",
                    "bomezome big",
                }
            },
            {
                "Special",
                new List<string>() {
                     "tech knight ghost",
                     "tunic knight void"
                }
            }
        };

        public static void InitializeEnemies(string SceneName) {
            List<Monster> Monsters = Resources.FindObjectsOfTypeAll<Monster>().ToList();
            foreach (string EnemyName in LocationEnemies[SceneName]) {
                if (EnemyName == "voidling redux") {
                    Enemies[EnemyName] = GameObject.Instantiate(Monsters.Where(Monster => Monster.name == EnemyName && Monster.transform.parent.name == "_Night Encounters").ToList()[0].gameObject);
                    Enemies[EnemyName].GetComponent<Voidling>().replacementMonster = null;

                } else { 
                    Enemies[EnemyName] = GameObject.Instantiate(Monsters.Where(Monster => Monster.name == EnemyName).ToList()[0].gameObject);
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

        }

        public static void SpawnNewEnemies() {
            foreach (NavigatingMonster Enemy in Resources.FindObjectsOfTypeAll<NavigatingMonster>().Where(Monster => Monster.transform.parent != null && !Monster.transform.parent.name.Contains("split tier") && !ExcludedEnemies.Contains(Monster.name))) {
                GameObject NewEnemy;

                if (TunicRandomizer.Settings.EnemyGeneration == RandomizerSettings.EnemyRandomizationType.RANDOM) {
                    NewEnemy = GameObject.Instantiate(Enemies[Enemies.Keys.ToList()[TunicRandomizer.Randomizer.Next(Enemies.Count)]]);
                } else if (TunicRandomizer.Settings.EnemyGeneration == RandomizerSettings.EnemyRandomizationType.BALANCED) {
                    int EnemyType = TunicRandomizer.Randomizer.Next(101);
                    List<string> EnemyTypes = new List<string>();
                    if (EnemyType < 30) {
                        EnemyTypes = EnemyRankings["Weak"];
                    } else if (EnemyType >= 30 && EnemyType < 75) {
                        EnemyTypes = EnemyRankings["Average"];
                    } else if (EnemyType >= 75 && EnemyType < 92) {
                        EnemyTypes = EnemyRankings["Strong"];
                    } else if (EnemyType >= 92 && EnemyType < 98) {
                        EnemyTypes = EnemyRankings["Intense"];
                    } else {
                        EnemyTypes = EnemyRankings["Special"];
                    }
                    NewEnemy = GameObject.Instantiate(Enemies[EnemyTypes[TunicRandomizer.Randomizer.Next(EnemyTypes.Count)]]);
                } else {
                    NewEnemy = GameObject.Instantiate(Enemies[Enemies.Keys.ToList()[TunicRandomizer.Randomizer.Next(Enemies.Count)]]);
                }
                //GameObject NewEnemy = GameObject.Instantiate(Enemies["voidling redux"]);

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
