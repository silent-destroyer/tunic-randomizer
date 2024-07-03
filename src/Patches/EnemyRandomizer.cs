using FMODUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {

    public class BossEnemy : MonoBehaviour { }
    public class FleemerQuartet : MonoBehaviour {
        public int GroupId;
        public List<GameObject> Quartet;
        public void Awake() {
            GroupId = -1;
            Quartet = new List<GameObject>();
        }

        public void Update() {
            Quartet = Quartet.Where(monster => monster != null).ToList();
        }
    }
    public class EnemyRandomizer {

        public static Dictionary<string, GameObject> Enemies = new Dictionary<string, GameObject>() { };

        public static Dictionary<string, List<string>> DefeatedEnemyTracker = new Dictionary<string, List<string>>();

        public static Dictionary<string, string> EnemiesInCurrentScene = new Dictionary<string, string>() { };

        public static bool RandomizedThisSceneAlready = false;
        public static bool DidArachnophoiaModeAlready = false;

        public static GameObject LibrarianPools;

        public static List<GameObject> LibrarianOrbs;

        // Heir
        public static GameObject FoxgodBossfightRoot;

        public static List<List<GameObject>> FoxgodPools;

        public static GameObject TuningFork;

        public static List<string> SpecificExcludedEnemies = new List<string>() {
            "frog cave main (118.5, 29.9, -52.6)",
            "East Forest Redux (104.3, -16.0, -28.0)"
        };

        public static List<string> ExcludedEnemies = new List<string>() {
            "tech knight boss",
            "Spidertank",
            "Scavenger Boss"
        };

        public static List<string> ExcludedScenes = new List<string>() {
            "PatrolCave",
            "Library Arena",
            "Fortress Arena",
            "Spirit Arena",
        };

        public static List<string> DoNotPlaceCoffeeTableHere = new List<string>() {
            "Cathedral Redux Fox enemy zombie (13)",
            "Fortress Basement Spider Small (3)",
            "Fortress Basement Spider Small (5)",
            "Sewer Bat (7)", "Sewer Bat (8)", "Sewer Bat (9)",
            "Sewer Bat (10)", "Sewer Bat (11)", "Sewer Bat (12)",
            "East Forest Redux Skuladot redux (4)",
            "East Forest Redux Skuladot redux (5)",
            "East Forest Redux Skuladot redux (6)",
        };

        public static List<string> DoNotPlaceTurretHere = new List<string>() {
            "Cathedral Redux Fox enemy zombie (13)",
            "Fortress Basement Spider Small (3)",
            "Fortress Basement Spider Small (5)",
            "Sewer Bat (7)", "Sewer Bat (8)", "Sewer Bat (9)",
            "Sewer Bat (10)", "Sewer Bat (11)", "Sewer Bat (12)",
            "East Forest Redux Skuladot redux (4)",
            "East Forest Redux Skuladot redux (5)",
            "East Forest Redux Skuladot redux (6)",
            "East Forest Redux Spider Small (8)",
            "Overworld Redux Honourguard",
        };

        public static List<string> BossStateVars = new List<string>() {
            "SV_Forest Boss Room_Skuladot redux Big",
            "SV_Archipelagos Redux TUNIC Knight is Dead",
            "SV_Fortress Arena_Spidertank Is Dead",
            "Librarian Dead Forever",
            "SV_ScavengerBossesDead"
        };

        public static List<string> CustomBossFlags = new List<string>() {
            "randomizer defeated forest boss",
            "randomizer defeated garden knight",
            "randomizer defeated siege engine",
            "randomizer defeated librarian",
            "randomizer defeated boss scavenger"
        };

        public static Dictionary<string, string> BossFlavorText = new Dictionary<string, string>() {
            { "tech knight boss", "uh slEpi^ gRdEin, wuhn uhv mehnE" },
            { "Spidertank", "#uh lahst fuh^k$uhni^ wor muh$En" },
            { "Librarian", "#uh %Evi^ skawlur hoo sEks #uh kraws buht duhz nawt uhndurstahnd" },
            { "Scavenger Boss", "#uh kwEn uhv #Oz hoo pik #uh bOnz uhv #is lahnd" },
            { "Foxgod", "" },
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
                "Sewer",
                new List<string>() {
                    "Spinnerbot (3)",
                }
            },
            {
                "Archipelagos Redux",
                new List<string>() {
                    "crocodoo Voidtouched",
                    "crocodoo",
                    "Fairyprobe Archipelagos (2)",
                    "Fairyprobe Archipelagos (Dmg)",
                    "tunic knight void",
                    "tech knight boss"
                }
            },
            {
                "Atoll Redux",
                new List<string>() {
                    "plover",
                    "Crow",
                    "Crabbit with Shell",
                    "Spinnerbot Baby",
                    "Crabbo (1)",
                    "Crabbit"
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
                "Fortress Arena",
                new List<string>() {
                    "Spidertank"
                }
            },
            {
                "Crypt",
                new List<string>() {
                    "Shadowreaper"
                }
            },
            {
                "Crypt Redux",
                new List<string>() {
                    "bomezome (3)"
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
                    "Frog Small_Ghost",
                    "Frog Spear_Ghost",
                    "Fairyprobe Archipelagos (Ghost)",
                    "bomezome_easy_ghost",
                    "bomezome_easy_ghost (tweaked)",
                    "Wizard_Support_Ghost",
                    "Skuladot redux_ghost",
                    "Skuladot redux_shield_ghost",
                    "Skuladot redux Big_ghost",
                }
            },
            {
                "ziggurat2020_1",
                new List<string>() {
                    "administrator",
                }
            },
            {
                "ziggurat2020_3",
                new List<string>() {
                    "Centipede from egg (Varient)",
                    "Scavenger Boss"
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
                "Library Arena",
                new List<string> () {
                    "Bat_librarian add",
                    "Skuladot redux_librarian add",
                    "Librarian"
                }
            },
            {
                "Posterity",
                new List<string> () {
                    "Phage",
                    "Ghost Knight"
                }
            },
            {
                "Spirit Arena",
                new List<string> () {
                    "Foxgod"
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
                    "Skuladot redux void",
                    "Skuladot redux_ghost",
                    "plover",
                    "Spinnerbot Baby",
                    "Crabbit",
                    "Fox enemy zombie",
                    "BlobBig",
                    "BlobBigger",
                    "HedgehogBig",
                    "Bat",
                    "Bat void",
                    "Spider Small",
                    "bomezome_easy",
                    "bomezome_easy_ghost",
                    "Fairyprobe Archipelagos",
                    "Fairyprobe Archipelagos (Ghost)",
                    "Fairyprobe Archipelagos (Dmg)",
                    "Skuladot redux_shield",
                    "Skuladot redux_shield_ghost",
                    "Turret",
                    "Hedgehog Trap",
                    "administrator_servant",
                    "Phage",
                    "Spinnerbot Corrupted",
                }
            },
            {
                "Strong",
                new List<string>() {
                    "Wizard_Candleabra",
                    "sewertentacle",
                    "Honourguard",
                    "Skuladot redux Big",
                    "Skuladot redux Big_ghost",
                    "Crow",
                    "Crow Voidtouched",
                    "crocodoo",
                    "crocodoo Voidtouched",
                    "Scavenger",
                    "Scavenger_miner",
                    "Scavenger_support",
                    "Scavenger_stunner",
                    "bomezome_fencer",
                    "Ghostfox_monster",
                    "voidling redux",
                    "Frog",
                    "Frog Small",
                    "Frog Small_Ghost",
                    "Frog Spear",
                    "Frog Spear_Ghost",
                    "Spider Big",
                    "Crabbo",
                    "Crabbit with Shell",
                    "Wizard_Sword",
                    "Wizard_Support",
                    "Wizard_Support_Ghost",
                    "woodcutter",
                    "Fox enemy",
                    "Centipede",
                    "Ghost Knight",
                    "bomezome_easy_ghost (tweaked)",
                    "bomezome_quartet",
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
                    "Voidtouched",
                    "Shadowreaper",
                }
            },
            { 
                "Bosses",
                new List<string>() {
                    "tech knight boss",
                    "Spidertank",
                    "Librarian",
                    "Scavenger Boss",
                    "Foxgod"
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
            { "Centipede", $"\"Centipede\"" },
            { "Shadowreaper", $"\"Shadowreaper\"" },
            { "Bat void", $"\"Phrend\" (void)" },
            { "Skuladot redux void", $"\"Rudeling\" (void)" },
            { "Frog Small_Ghost", $"\"Frog...?\" (smawl) [frog]" },
            { "Frog Spear_Ghost", $"\"Frog...?\" (spEr) [frog]" },
            { "Fairyprobe Archipelagos (Ghost)", $"\"Fairy...?\"" },
            { "bomezome_easy_ghost", $"\"Fleemer...?\"" },
            { "bomezome_easy_ghost (tweaked)", $"\"Fleemer...?\" (lRj)" },
            { "Wizard_Support_Ghost", $"\"Custodian...?\" (suhport)" },
            { "Skuladot redux_ghost", $"\"Rudeling...?\"" },
            { "Skuladot redux_shield_ghost", $"\"Rudeling...?\" ($Eld)" },
            { "Skuladot redux Big_ghost", $"\"Guard Captain...?\"" },
            { "Librarian", $"\"Librarian\"" },
            { "Spidertank", $"\"Siege Engine\"" },
            { "Scavenger Boss", $"\"Boss Scavenger\"" },
            { "tech knight boss", $"\"Garden Knight\"" },
            { "Foxgod", $"\"The Heir\"" },
            { "bomezome_quartet", $"\"Fleemer\" (kworteht)" },
        };

        public static Dictionary<string, string> EnemyToggleOptionNames = new Dictionary<string, string>() {
            { "Blob", $"Blob" },
            { "BlobBig", $"Big Blob" },
            { "BlobBigger", $"Bigger Blob" },
            { "Hedgehog", $"Hedgehog" },
            { "HedgehogBig", $"Big Hedgehog" },
            { "Skuladot redux", $"Rudeling" },
            { "Skuladot redux_ghost", $"Rudeling (Ghost)" },
            { "Skuladot redux void", $"Rudeling (Void)" },
            { "Skuladot redux_shield", $"Rudeling w/ Shield" },
            { "Skuladot redux_shield_ghost", $"Rudeling w/ Shield (Ghost)" },
            { "Skuladot redux Big", $"Guard Captain" },
            { "Skuladot redux Big_ghost", $"Guard Captain (Ghost)" },
            { "Honourguard", $"Envoy" },
            { "beefboy", $"Beefboy" },
            { "Bat", $"Phrend" },
            { "Bat void", $"Phrend (Void)" },
            { "Spider Small", $"Spyrite" },
            { "Spider Big", $"Sappharach" },
            { "Spinnerbot Baby", $"Baby Slorm" },
            { "Spinnerbot Corrupted", $"Slorm" },
            { "Turret", $"Autobolt (Turret)" },
            { "Hedgehog Trap", $"Laser Trap" },
            { "sewertentacle", $"Tentacle" },
            { "Fairyprobe Archipelagos (Dmg)", $"Fairy" },
            { "Fairyprobe Archipelagos (Ghost)", $"Fairy (Ghost)" },
            { "Fairyprobe Archipelagos", $"Fairy (Ice)" },
            { "crocodoo", $"Chompignom (Terry)" },
            { "crocodoo Voidtouched", $"Chompignom (Void)" },
            { "tech knight boss", $"Garden Knight" },
            { "plover", $"Plover (Bluebirds)" },
            { "Crabbit", $"Crabbit" },
            { "Crabbit with Shell", $"Crabbit w/ Cube Shell" },
            { "Crabbo", $"Crabbo" },
            { "Crow", $"Husher" },
            { "Crow Voidtouched", $"Husher (Void)" },
            { "Frog", $"Frog" },
            { "Frog Small", $"Frog (Small)" },
            { "Frog Small_Ghost", $"Frog (Small Ghost)" },
            { "Frog Spear", $"Frog w/ Shield & Spear" },
            { "Frog Spear_Ghost", $"Frog w/ Shield & Spear (Ghost)" },
            { "administrator_servant", $"Administrator (Coffee Table)" },
            { "Librarian", $"Librarian" },
            { "Wizard_Sword", $"Custodian" },
            { "Wizard_Candleabra", $"Custodian w/ Candelabra" },
            { "Wizard_Support", $"Custodian (Support)" },
            { "Wizard_Support_Ghost", $"Custodian (Support Ghost)" },
            { "Spidertank", $"Siege Engine" },
            { "Scavenger", $"Scavenger (Sniper)" },
            { "Scavenger_miner", $"Scavenger (Miner)" },
            { "Scavenger_support", $"Scavenger (Support)" },
            { "Scavenger_stunner", $"Scavenger (Ice Sniper)" },
            { "voidling redux", $"Voidling Spider" },
            { "administrator", $"Administrator" },
            { "Scavenger Boss", $"Boss Scavenger" },
            { "bomezome_easy", $"Fleemer" },
            { "bomezome_easy_ghost", $"Fleemer (Ghost)" },
            { "bomezome_easy_ghost (tweaked)", $"Fleemer (Big Ghost)" },
            { "bomezome_fencer", $"Fleemer (Fencer)" },
            { "bomezome_quartet", $"Fleemer (Quartet)" },
            { "bomezome big", $"Giant Fleemer" },
            { "Ghostfox_monster", $"Lost Echo" },
            { "Gunslinger", $"Gunslinger" },
            { "Fox enemy zombie", $"Fox Zombie" },
            { "Fox enemy", $"Fox Zombie (Strong)" },
            { "tech knight ghost", $"Garden Knight (Ghost)" },
            { "Voidtouched", $"Voidtouched" },
            { "Foxgod", $"The Heir" },
            { "Ghost Knight", $"Ghost Knight (Devworld)" },
            { "Phage", $"Phage/Beta Slorm (Devworld)" },
            { "woodcutter", $"Woodcutter" },
            { "Centipede", $"Centipede" },
            { "tunic knight void", $"Garden Knight (Void)" },
            { "Shadowreaper", $"Shadowreaper" },
        };

        public static void CreateAreaSeeds() {
            System.Random Random = new System.Random(SaveFile.GetInt("seed"));
            foreach (String Scene in Locations.AllScenes) {
                SaveFile.SetInt($"randomizer enemy seed {Scene}", Random.Next());
            }
        }

        public static void InitializeEnemies(string SceneName) {
            List<Monster> Monsters = Resources.FindObjectsOfTypeAll<Monster>().ToList();
            Dictionary<string, string> RenamedEnemies = new Dictionary<string, string>() {
                { "_Turret", "Turret" },
                { "Fairyprobe Archipelagos (2)", "Fairyprobe Archipelagos" },
                { "Frog (7)", "Frog" },
                { "Hedgehog Trap (1)", "Hedgehog Trap" },
                { "Centipede from egg (Varient)", "Centipede" },
                { "Spinnerbot (3)", "Spinnerbot Corrupted" },
                { "Bat_librarian add", "Bat void" },
                { "Skuladot redux_librarian add", "Skuladot redux void" },
                { "Crabbo (1)", "Crabbo" },
                { "bomezome (3)", "bomezome_quartet" },
            };
            foreach (string LocationEnemy in LocationEnemies[SceneName]) {
                string EnemyName = LocationEnemy;
                if (RenamedEnemies.ContainsKey(EnemyName)) {
                    EnemyName = RenamedEnemies[EnemyName];
                }
                if (EnemyName == "voidling redux") {
                    Enemies[EnemyName] = GameObject.Instantiate(Monsters.Where(Monster => Monster.name == LocationEnemy && Monster.transform.parent.name == "_Night Encounters").ToList()[0].gameObject);
                } else {
                    Enemies[EnemyName] = GameObject.Instantiate(Monsters.Where(Monster => Monster.name == LocationEnemy).ToList()[0].gameObject);
                }
                GameObject.DontDestroyOnLoad(Enemies[EnemyName]);
                Enemies[EnemyName].SetActive(false);

                if (Enemies[EnemyName].GetComponent<BossAnnounceOnAggro>() != null) {
                    GameObject.Destroy(Enemies[EnemyName].GetComponent<BossAnnounceOnAggro>());
                    Enemies[EnemyName].GetComponent<ZTarget>().hideHPBar = false;
                    Enemies[EnemyName].AddComponent<BossEnemy>();
                }

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
                if (EnemyName == "Turret") {
                    TuningFork = Enemies[EnemyName].GetComponent<DefenseTurret>().turretCorpsePrefab.transform.GetChild(0).gameObject;
                }
                if (EnemyName == "Centipede") {
                    Enemies[EnemyName].GetComponent<Centipede>().maxBeamDistance = 10f;
                    Enemies[EnemyName].GetComponent<Centipede>().attackDistance = 5f;
                    Enemies[EnemyName].GetComponent<Centipede>().monsterAggroDistance = 20f;
                }
                if (EnemyName == "Bat void") {
                    Enemies[EnemyName].GetComponent<Bat>().monsterAggroDistance = 4;
                }
                if (EnemyName == "Skuladot redux_ghost") {
                    Dat.floatDatabase[Enemies[EnemyName].GetComponent<Monster>().dropValue.id] = Enemies["Skuladot redux"].GetComponent<Monster>().dropValue.Value;
                }
                if (EnemyName == "Skuladot redux_shield_ghost") {
                    Dat.floatDatabase[Enemies[EnemyName].GetComponent<Monster>().dropValue.id] = Enemies["Skuladot redux_shield"].GetComponent<Monster>().dropValue.Value;
                }
                if (EnemyName == "Skuladot redux Big_ghost") {
                    Dat.floatDatabase[Enemies[EnemyName].GetComponent<Monster>().dropValue.id] = Enemies["Skuladot redux Big"].GetComponent<Monster>().dropValue.Value;
                }
                if (EnemyName == "bomezome_easy") {
                    Dat.floatDatabase[Enemies["bomezome_easy_ghost"].GetComponent<Monster>().dropValue.id] = Enemies["bomezome_easy"].GetComponent<Monster>().dropValue.Value;
                }
                if (EnemyName == "Frog Small") {
                    Dat.floatDatabase[Enemies["Frog Small_Ghost"].GetComponent<Monster>().dropValue.id] = Enemies["Frog Small"].GetComponent<Monster>().dropValue.Value;
                }
                if (EnemyName == "Frog Spear") {
                    Dat.floatDatabase[Enemies["Frog Spear_Ghost"].GetComponent<Monster>().dropValue.id] = Enemies["Frog Spear"].GetComponent<Monster>().dropValue.Value;
                }
                if (EnemyName == "Wizard_Support") {
                    Dat.floatDatabase[Enemies["Wizard_Support_Ghost"].GetComponent<Monster>().dropValue.id] = Enemies["Wizard_Support"].GetComponent<Monster>().dropValue.Value;
                }
                if (EnemyName == "Scavenger Boss") {
                    Enemies["Scavenger Boss"].GetComponent<ScavengerBoss>().eggTossChance = 0.25f;
                }
                if (EnemyName == "bomezome_quartet") {
                    Enemies[EnemyName].AddComponent<FleemerQuartet>();
                    Enemies[EnemyName].GetComponent<FleemerQuartet>().GroupId = -1;
                }
                if(EnemyName == "Librarian") {
                    LibrarianPools = GameObject.Instantiate(GameObject.Find("_Pools/"));
                    LibrarianOrbs = new List<GameObject>();

                    foreach(GameObject orb in LibrarianPools.transform.GetChild(3).GetComponent<PooledFX>().pool) {
                        GameObject orbclone = GameObject.Instantiate(orb);
                        LibrarianOrbs.Add(orbclone);
                        GameObject.DontDestroyOnLoad(orbclone);
                        orbclone.SetActive(false);
                    }

                    GameObject.DontDestroyOnLoad(LibrarianPools);
                    Enemies[EnemyName].GetComponent<Librarian>().horizontalSlashPrefab_pool = LibrarianPools.transform.GetChild(0).GetComponent<PooledFX>();
                    Enemies[EnemyName].GetComponent<Librarian>().verticalSlashPrefab_pool = LibrarianPools.transform.GetChild(1).GetComponent<PooledFX>();
                    Enemies[EnemyName].GetComponent<Librarian>().orbPrefab_pool = LibrarianPools.transform.GetChild(3).GetComponent<PooledFX>();
                }
                if (EnemyName == "Foxgod") {
                    FoxgodBossfightRoot = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "_BOSSFIGHT ROOT").First());
                    FoxgodBossfightRoot.SetActive(false);
                    GameObject.Destroy(FoxgodBossfightRoot.transform.GetChild(2).gameObject);
                    Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "_BOSSFIGHT ROOT").First().SetActive(true);
                    FoxgodPools = new List<List<GameObject>>();
                    for(int i = 0; i < FoxgodBossfightRoot.transform.GetChild(0).childCount; i++) {
                        List<GameObject> pool = new List<GameObject>();
                        for(int j = 0; j < 64; j++) {
                            GameObject fxClone = GameObject.Instantiate(FoxgodBossfightRoot.transform.GetChild(0).GetChild(i).GetChild(0).gameObject);
                            fxClone.SetActive(false);
                            pool.Add(fxClone);
                            GameObject.DontDestroyOnLoad(fxClone);
                        }
                        FoxgodPools.Add(pool);
                    }
                    GameObject.DontDestroyOnLoad(FoxgodBossfightRoot);
                    Enemies[EnemyName].transform.GetChild(12).gameObject.SetActive(false);
                }
                Enemies[EnemyName].name = EnemyName + " Prefab";
            }
            if (SceneName == "Archipelagos Redux") {
                Enemies["tunic knight void"].GetComponent<ZTarget>().isActive = true;
                Enemies["tunic knight void"].GetComponent<CapsuleCollider>().enabled = true;
                for (int i = 0; i < Enemies["tunic knight void"].transform.childCount - 2; i++) {
                    Enemies["tunic knight void"].transform.GetChild(i).gameObject.SetActive(true);
                }
                Dat.floatDatabase["mpCost_Spear_mp2"] = 40f;
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

            string CurrentScene = SceneManager.GetActiveScene().name;

            List<BombFlask> bombFlasks = Resources.FindObjectsOfTypeAll<BombFlask>().Where(bomb => bomb.name != "Firecracker").ToList();

            List<bool> bossStateVars = BossStateVars.Select(x => StateVariable.GetStateVariableByName(x).BoolValue).ToList();
            BossStateVars.ForEach(s => StateVariable.GetStateVariableByName(s).BoolValue = false);
            List<int> bossesSpawned = new List<int>() { 0, 0, 0, 0, 0 };

            System.Random Random;
            if (TunicRandomizer.Settings.SeededEnemies) {
                Random = new System.Random(SaveFile.GetInt($"randomizer enemy seed {CurrentScene}"));
            } else {
                Random = new System.Random();
            }
            
            // Oops all enemy: pick a single enemy for the entire scene
            List<string> OopsAllEnemies = Enemies.Keys.ToList();
            if (TunicRandomizer.Settings.OopsAllEnemy) {
                if (TunicRandomizer.Settings.UseEnemyToggles) {
                    OopsAllEnemies = OopsAllEnemies.Where(enemy => TunicRandomizer.Settings.EnemyToggles[EnemyToggleOptionNames[enemy]]).ToList();
                } else if (TunicRandomizer.Settings.BalancedEnemies) {
                    OopsAllEnemies = OopsAllEnemies.Except(EnemyRankings["Intense"]).ToList();
                }
                EnemyRankings["Bosses"].ForEach(boss => {
                    OopsAllEnemies.Remove(boss);
                });
                if (OopsAllEnemies.Count > 0) {
                    OopsAllEnemies = new List<string>() { OopsAllEnemies[Random.Next(OopsAllEnemies.Count)] };
                }
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
                Monsters = Resources.FindObjectsOfTypeAll<GameObject>().Where(Monster => (Monster.GetComponent<Monster>() != null || Monster.GetComponent<TurretTrap>() != null) && !Monster.name.Contains("Prefab")).ToList();
            }
            if (CurrentScene == "Cathedral Redux") {
                Monsters.AddRange(Resources.FindObjectsOfTypeAll<GameObject>().Where(Monster => Monster.GetComponent<Crow>() != null && !Monster.name.Contains("Prefab")).ToList());
            }
            if (CurrentScene == "Forest Boss Room" && GameObject.Find("Skuladot redux") != null) {
                Monsters.Add(GameObject.Find("Skuladot redux"));
            }
            if (CurrentScene == "Fortress Basement") {
                Monsters.AddRange(Resources.FindObjectsOfTypeAll<GameObject>().Where(slorm => slorm.GetComponent<Spinnerbot>() != null && slorm.gameObject.scene.name == CurrentScene && slorm.name == "Spinnerbot Baby").ToList());
            }
            if (CurrentScene == "frog cave main") {
                Monsters.Add(GameObject.Find("Wizard_Support"));
            }
            if (CurrentScene == "Cathedral Arena") {
                Monster.ClearRuntimeDeadMonsters();
            }
            if (TunicRandomizer.Settings.ExtraEnemiesEnabled) {
                if (CurrentScene == "Monastery") {
                    Resources.FindObjectsOfTypeAll<Voidtouched>().ToList()[0].gameObject.transform.parent = null;
                } else if(CurrentScene == "ziggurat2020_3") {
                    foreach(ScavengerBoss bossScav in Resources.FindObjectsOfTypeAll<ScavengerBoss>().Where(boss => boss.gameObject.scene.name == CurrentScene)) {
                        bossScav.eggTossChance = 0.2f;
                    }
                }
            }

            Monsters = Monsters.Where(Monster => Monster.gameObject.scene.name == CurrentScene).ToList();
            
            int i = 0;
            foreach (GameObject Enemy in Monsters) {
                GameObject NewEnemy = null;
                if (SpecificExcludedEnemies.Contains($"{CurrentScene} {Enemy.transform.position.ToString()}")) {
                    continue;
                }
                try {
                    List<string> EnemyKeys = TunicRandomizer.Settings.OopsAllEnemy ? new List<string>(OopsAllEnemies) : Enemies.Keys.ToList();
                    if (CurrentScene == "Cathedral Arena") {
                        EnemyKeys.Remove("administrator_servant");
                        EnemyKeys.Remove("Hedgehog Trap");
                        if (Inventory.GetItemByName("Wand").Quantity == 0) {
                            EnemyKeys.Remove("Crabbit with Shell");
                        }
                        if (Enemy.transform.parent.name.Contains("Wave")) {
                            float x = (float)(Random.NextDouble() * 15) - 15f;
                            float z = (float)(Random.NextDouble() * 23) - 23f;
                            Enemy.transform.position = new Vector3(x, 0, z);
                        }
                    }
                    if (CurrentScene == "ziggurat2020_1") {
                        if (Enemy.GetComponent<Administrator>() != null) {
                            EnemyKeys.Remove("Hedgehog Trap");
                            EnemyKeys.Remove("administrator_servant");
                            EnemyKeys.Remove("Shadowreaper");
                        }
                    }
                    if (CurrentScene == "Forest Boss Room" && Enemy.GetComponent<BossAnnounceOnAggro>() != null) {
                        EnemyKeys.Remove("administrator_servant");
                        EnemyKeys.Remove("Hedgehog Trap");
                    }
                    if (CurrentScene == "Sewer" && Enemy.name.Contains("Spinnerbot") && !Enemy.name.Contains("Corrupted")) {
                        Enemy.name = Enemy.name.Replace("Spinnerbot", "Spinnerbot Corrupted");
                    }
                    if (CurrentScene == "Crypt Redux" && Enemy.name.Contains("bomezome") && !Enemy.name.Contains("big") && !Enemy.name.Contains("easy")) {
                        Enemy.name = Enemy.name.Replace("bomezome", "bomezome_easy");
                    }
                    if (TunicRandomizer.Settings.ExtraEnemiesEnabled) {
                        if (Enemy.transform.parent != null && (Enemy.transform.parent.name.Contains("NG+") || (Enemy.transform.parent.name.ToLower().Contains("night") && CurrentScene != "Cathedral Arena"))) {
                            Enemy.transform.parent.gameObject.SetActive(true);
                        }

                        if (CurrentScene == "Monastery") {
                            if (GameObject.Find("_NIGHT/Corruption Blocker/") != null) {
                                GameObject.Find("_NIGHT/Corruption Blocker/").SetActive(false);
                            }
                            if (GameObject.Find("_NIGHT/Corruption Blocker (retreat door)/") != null) {
                                GameObject.Find("_NIGHT/Corruption Blocker (retreat door)/").SetActive(false);
                            }
                        }
                    }
                    if (DoNotPlaceCoffeeTableHere.Contains($"{CurrentScene} {Enemy.name}")) {
                        EnemyKeys.Remove("administrator_servant");
                    }
                    if (DoNotPlaceTurretHere.Contains($"{CurrentScene} {Enemy.name}")) {
                        EnemyKeys.Remove("Turret");
                    }

                    if (TunicRandomizer.Settings.UseEnemyToggles) {
                        foreach(KeyValuePair<string, string> enemyToggle in EnemyToggleOptionNames) {
                            if (!TunicRandomizer.Settings.EnemyToggles[enemyToggle.Value] && EnemyKeys.Contains(enemyToggle.Key)) {
                                EnemyKeys.Remove(enemyToggle.Key);
                            }
                        }
                    } else if (!TunicRandomizer.Settings.OopsAllEnemy) {
                        // Make alternate variants of certain enemies slightly less common
                        EnemyKeys.Remove(Random.NextDouble() < 0.25 ? "Frog Small" : "Frog Small_Ghost");
                        EnemyKeys.Remove(Random.NextDouble() < 0.25 ? "Frog Spear" : "Frog Spear_Ghost");
                        EnemyKeys.Remove(Random.NextDouble() < 0.25 ? "Fairyprobe Archipelagos" : "Fairyprobe Archipelagos (Ghost)");
                        EnemyKeys.Remove(Random.NextDouble() < 0.25 ? "bomezome_easy" : "bomezome_easy_ghost");
                        EnemyKeys.Remove(Random.NextDouble() < 0.25 ? "Wizard_Support" : "Wizard_Support_Ghost");
                        EnemyKeys.Remove(Random.NextDouble() < 0.50 ? "Skuladot redux void" : "Skuladot redux_ghost");
                        EnemyKeys.Remove(Random.NextDouble() < 0.25 ? "Skuladot redux_shield" : "Skuladot redux_shield_ghost");
                        EnemyKeys.Remove(Random.NextDouble() < 0.25 ? "Skuladot redux Big" : "Skuladot redux Big_ghost");
                        EnemyKeys.Remove(Random.NextDouble() < 0.25 ? "Bat" : "Bat void");
                    }

                    // Limit boss spawns to one of each type max
                    if (TunicRandomizer.Settings.LimitBossSpawns) {
                        EnemyRankings["Bosses"].ForEach(x => {
                            if (bossesSpawned[EnemyRankings["Bosses"].IndexOf(x)] > 0) {
                                EnemyKeys.Remove(x);
                            }
                        });
                    }

                    if (TunicRandomizer.Settings.BalancedEnemies) {
                        EnemyRankings["Bosses"].ForEach(x => {
                            EnemyKeys.Remove(x);
                        });
                    }

                    if (EnemyKeys.Count == 0) {
                        GameObject.Destroy(Enemy.gameObject);
                        continue;
                    }

                    if (!TunicRandomizer.Settings.BalancedEnemies || TunicRandomizer.Settings.OopsAllEnemy) {
                        NewEnemy = GameObject.Instantiate(Enemies[EnemyKeys[Random.Next(EnemyKeys.Count)]]);
                    } else {
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
                            if (EnemyTypes.Where(x => EnemyKeys.Contains(x)).Count() == 0) {
                                if (EnemyTypes == EnemyRankings["Intense"]) {
                                    EnemyTypes = EnemyRankings["Strong"].Where(x => EnemyKeys.Contains(x)).Count() > 0 ? EnemyRankings["Strong"] : EnemyRankings["Average"];
                                } else if (EnemyTypes == EnemyRankings["Strong"]) {
                                    EnemyTypes = EnemyRankings["Average"];
                                }
                            }
                            EnemyTypes = EnemyTypes.Where(x => EnemyKeys.Contains(x)).ToList();
                            if (EnemyTypes.Count == 0) {
                                GameObject.Destroy(Enemy.gameObject);
                                continue;
                            }
                            NewEnemy = GameObject.Instantiate(Enemies[EnemyTypes[Random.Next(EnemyTypes.Count)]]);
                        }
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
                    if (NewEnemy.GetComponent<Shadowreaper>() != null && NewEnemy.GetComponent<Creature>().defaultStartingMaxHP != null) {
                        NewEnemy.GetComponent<Creature>().defaultStartingMaxHP._value = 100;
                    }
                    if (NewEnemy.name.Contains("BlobBigger") && NewEnemy.GetComponent<Creature>().defaultStartingMaxHP != null) {
                        NewEnemy.GetComponent<Creature>().defaultStartingMaxHP._value = 25;
                    }
                    if (SceneLoaderPatches.SceneName == "ziggurat2020_1" && Enemy.GetComponent<Administrator>() != null) {
                        GameObject.FindObjectOfType<ZigguratAdminGate>().admin = NewEnemy.GetComponent<Monster>();
                    }
                    if (SceneLoaderPatches.SceneName != "Atoll Redux" && NewEnemy.GetComponent<Crabbo>() != null) {
                        NewEnemy.transform.GetComponent<NavMeshAgent>().agentTypeID = 0;
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
                                } else if (EnemyRankings["Bosses"].Contains(Key)) {
                                    BottomLine.text = BossFlavorText[Key];
                                } else {
                                    BottomLine.text = $"A straw^ ehnuhmE";
                                }
                            }
                        }
                        NewEnemy.GetComponent<BossAnnounceOnAggro>().bossTitleTopLine = TopLine;
                        NewEnemy.GetComponent<BossAnnounceOnAggro>().bossTitleBottomLine = BottomLine;
                    }

                    // Randomize support scavengers bomb type
                    if (NewEnemy.GetComponent<Scavenger_Support>() != null) {
                        int bombChance = Random.Next(100);
                        Rigidbody randomBomb;
                        if (bombChance < 4) {

                            randomBomb = bombFlasks.Where(bomb => bomb.name == "centipede_detritus_head").ToList()[0].gameObject.GetComponent<Rigidbody>();
                        } else {
                            List<BombFlask> possibleBombs = bombFlasks.Where(bomb => bomb.name != "Firecracker" && bomb.name != "centipede_detritus_head").ToList();
                            randomBomb = possibleBombs[Random.Next(possibleBombs.Count)].gameObject.GetComponent<Rigidbody>();
                        }
                        NewEnemy.GetComponent<Scavenger_Support>().bombPrefab = randomBomb;
                        if (!randomBomb.gameObject.name.Contains("Firecracker")) {
                            NewEnemy.GetComponent<Scavenger_Support>().tossAngle = 15f;
                        }
                        if (randomBomb.gameObject.name.Contains("Ice") || (randomBomb.gameObject.name.Contains("centipede") && bombChance < 2)) {
                            NewEnemy.transform.GetChild(0).GetComponent<CreatureMaterialManager>().originalMaterials = Enemies["Scavenger_stunner"].transform.GetChild(0).GetComponent<CreatureMaterialManager>().originalMaterials;
                        }
                    }

                    // For ice snipers
                    if (NewEnemy.GetComponent<Scavenger>() != null) {
                        NewEnemy.GetComponent<Scavenger>().useStunBulletPool = NewEnemy.name.Contains("stunner");
                        if (NewEnemy.name.Contains("stunner")) {
                            NewEnemy.GetComponent<Scavenger>().laserEndSphere = Enemies["Scavenger"].GetComponent<Scavenger>().laserEndSphere;
                        }
                    }

                    if (NewEnemy.name.Contains("Spinnerbot Corrupted") && Random.Next(100) == 99) {
                        NewEnemy.transform.localScale = new Vector3(2f, 2f, 2f);
                        NewEnemy.GetComponent<Monster>().defaultStartingMaxHP._value = 30;
                    }

                    if (NewEnemy.GetComponent<Administrator>() != null && NewEnemy.name.ToLower().Contains("servant")) {
                        NewEnemy.GetComponent<BoxCollider>().extents /= 2;
                    }

                    if (NewEnemy.GetComponent<Librarian>() != null) {
                        NewEnemy.GetComponent<Librarian>().orbitCentre = PlayerCharacter.Instanced ? PlayerCharacter.instance.transform : NewEnemy.transform;
                        NewEnemy.GetComponent<Librarian>().fightCameraZone = new GameObject();
                        NewEnemy.GetComponent<Librarian>().escapeLadderGameObject = new GameObject();
                        NewEnemy.GetComponent<Librarian>().killbox = new GameObject();
                        NewEnemy.GetComponent<Librarian>().orbitDistance = 5;
                        NewEnemy.GetComponent<Librarian>().orbitAltitude = 5;
                        NewEnemy.GetComponent<Librarian>().addSpawnTransforms = new Transform[] { PlayerCharacter.Instanced ? PlayerCharacter.instance.transform : NewEnemy.transform };
                        NewEnemy.GetComponent<Librarian>().orbPrefab_pool.pool = LibrarianOrbs.ToArray();
                    }

                    if (NewEnemy.GetComponent<Spidertank>() != null) {
                        NewEnemy.transform.localScale = Vector3.one * 0.35f;
                        NewEnemy.transform.GetChild(1).GetChild(0).GetChild(4).GetChild(15).gameObject.SetActive(false);
                    }

                    if (NewEnemy.GetComponent<BossEnemy>() != null) {
                        if (NewEnemy.GetComponent<Monster>().defaultStartingMaxHP != null) {
                            NewEnemy.GetComponent<Monster>().defaultStartingMaxHP._value = 300;
                        }
                        EnemyRankings["Bosses"].ForEach(x => {
                            if (NewEnemy.name.Contains(x)) {
                                bossesSpawned[EnemyRankings["Bosses"].IndexOf(x)]++;
                            }
                        });
                    }

                    if (NewEnemy.GetComponent<Foxgod>() != null) {
                        NewEnemy.GetComponent<Foxgod>().impactFX_pool = FoxgodBossfightRoot.transform.GetChild(0).GetChild(0).GetComponent<PooledFX>();
                        NewEnemy.GetComponent<Foxgod>().impactFX_pool.pool = FoxgodPools[0].ToArray();
                        NewEnemy.GetComponent<Foxgod>().shockwave_pool = FoxgodBossfightRoot.transform.GetChild(0).GetChild(1).GetComponent<PooledFX>();
                        NewEnemy.GetComponent<Foxgod>().shockwave_pool.pool = FoxgodPools[1].ToArray();
                        NewEnemy.GetComponent<Foxgod>().bullet_pool = FoxgodBossfightRoot.transform.GetChild(0).GetChild(2).GetComponent<PooledFX>();
                        NewEnemy.GetComponent<Foxgod>().bullet_pool.pool = FoxgodPools[2].ToArray();
                        NewEnemy.GetComponent<Foxgod>().summonFX_pool = FoxgodBossfightRoot.transform.GetChild(0).GetChild(3).GetComponent<PooledFX>();
                        NewEnemy.GetComponent<Foxgod>().summonFX_pool.pool = FoxgodPools[3].ToArray();
                        NewEnemy.GetComponent<Foxgod>().rainPreview_pool = FoxgodBossfightRoot.transform.GetChild(0).GetChild(4).GetComponent<PooledFX>();
                        NewEnemy.GetComponent<Foxgod>().rainPreview_pool.pool = FoxgodPools[4].ToArray();
                        NewEnemy.GetComponent<Foxgod>().voidhole_pool = FoxgodBossfightRoot.transform.GetChild(0).GetChild(5).GetComponent<PooledFX>();
                        NewEnemy.GetComponent<Foxgod>().voidhole_pool.pool = FoxgodPools[5].ToArray();
                        if (CurrentScene != "Cathedral Arena") {
                            NewEnemy.GetComponent<Foxgod>()._monsterAggroed = true;
                        }
                    }


                    NewEnemy.name += $" {i}";
                    EnemiesInCurrentScene.Add(NewEnemy.name, NewEnemy.transform.position.ToString());

                    // Give every enemy a unique ID to fix enemies despawning (and more importantly, fixes cathedral)
                    if (NewEnemy.GetComponent<RuntimeStableID>() != null) {
                        NewEnemy.GetComponent<RuntimeStableID>().intraSceneID = Resources.FindObjectsOfTypeAll<RuntimeStableID>().OrderBy(id => id.intraSceneID).Last().intraSceneID + 1;
                    }

                    if (NewEnemy.GetComponent<FleemerQuartet>() != null && NewEnemy.GetComponentInParent<CathedralGauntletManager>() == null) {
                        NewEnemy.GetComponent<FleemerQuartet>().GroupId = i;
                        SpawnFleemerQuartet(NewEnemy);
                    }

                    i++;
                    if (DefeatedEnemyTracker.ContainsKey(CurrentScene) && DefeatedEnemyTracker[CurrentScene].Contains(Enemy.transform.position.ToString())) {
                        GameObject.Destroy(NewEnemy);
                    }
                    if (CurrentScene == "ziggurat2020_1" && NewEnemy != null) {
                        // Move fairies in bounds so replaced enemies don't fall out of the map
                        if (Enemy.GetComponent<Probe>() != null && Enemy.name.Contains("(wave 1)")) {
                            float x = 91f - (float)(Random.NextDouble() * 15f);
                            float z = 31f - (float)(Random.NextDouble() * 15f);
                            NewEnemy.transform.position = new Vector3(x, 138, z);
                        }
                    }
                    GameObject.Destroy(Enemy.gameObject);
                } catch (Exception ex) {
                    TunicLogger.LogInfo("An error occurred spawning a new randomized enemy");
                    if (NewEnemy != null) {
                        GameObject.Destroy(NewEnemy);
                        TunicLogger.LogError("An error occurred spawning the following randomized enemy: " + NewEnemy.name);
                        TunicLogger.LogError(ex.Message + " " + ex.StackTrace);
                    }
                }
            
                BossStateVars.ForEach(s => StateVariable.GetStateVariableByName(s).BoolValue = bossStateVars[BossStateVars.IndexOf(s)]);
            }

            RandomizedThisSceneAlready = true;

            try {
                foreach (string Key in Enemies.Keys) {
                    if (Enemies[Key] != null) {
                        Enemies[Key].SetActive(false);
                    }
                }
            } catch (Exception e) {

            }
        }

        public static void ToggleArachnophobiaMode() {
            Sprite sRune = ModelSwaps.FindSprite("Alphabet New_33");
            Sprite pRune = ModelSwaps.FindSprite("Alphabet New_21");
            Sprite iRune = ModelSwaps.FindSprite("Alphabet New_13");
            Sprite dRune = ModelSwaps.FindSprite("Alphabet New_24");
            Sprite urRune = ModelSwaps.FindSprite("Alphabet New_8");

            Sprite ehRune = ModelSwaps.FindSprite("Alphabet New_3");
            Sprite nRune = ModelSwaps.FindSprite("Alphabet New_19");
            Sprite tRune = ModelSwaps.FindSprite("Alphabet New_23");
            Sprite uhRune = ModelSwaps.FindSprite("Alphabet New_2");
            Sprite ERune = ModelSwaps.FindSprite("Alphabet New_6");
            Material material = ModelSwaps.FindMaterial("UI Add");
            foreach (Spider spider in Resources.FindObjectsOfTypeAll<Spider>().Where(spider => spider.gameObject.scene.name == SceneManager.GetActiveScene().name)) {
                if (spider.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>() != null) {
                    spider.transform.GetChild(0).gameObject.SetActive(false);
                }
                Color color = spider.name.ToLower().Contains("small") ? new Color(1, 0.5f, 0.2f, 1) : new Color(0, 0.5f, 1, 1);
                float localXAngle = spider.name.ToLower().Contains("small") ? 0.8f : 0.6f;
                GameObject runes = new GameObject("runes");
                runes.transform.parent = spider.transform;
                runes.transform.localPosition = spider.transform.localPosition;
                GameObject s = CreateRune("s", runes.transform, new Vector3(-0.7f, 1, -0.1f), new Vector3(1.5f, 1.5f, 1.5f), sRune, color);
                GameObject p = CreateRune("p", runes.transform, new Vector3(0.8f, 1f, -0.1f), new Vector3(1.5f, 1.5f, 1.5f), pRune, color, material);
                GameObject i = CreateRune("i", runes.transform, new Vector3(0.8f, 1f, -0.1f), new Vector3(1.5f, 1.5f, 1.5f), iRune, color);
                GameObject d = CreateRune("d", runes.transform, new Vector3(2.3f, 1f, -0.1f), new Vector3(1.5f, 1.5f, 1.5f), dRune, color, material);
                GameObject ur = CreateRune("ur", runes.transform, new Vector3(2.3f, 1f, -0.1f), new Vector3(1.5f, 1.5f, 1.5f), urRune, color);
                runes.transform.localPosition = new Vector3(localXAngle, 0, 0);
                runes.transform.localEulerAngles = new Vector3(0, 180, 0);
            }
            foreach (Centipede centipede in Resources.FindObjectsOfTypeAll<Centipede>().Where(centipede => centipede.gameObject.scene.name == SceneManager.GetActiveScene().name)) { 
                if (centipede.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>() != null) {
                    centipede.transform.GetChild(0).gameObject.SetActive(false);
                }

                Color color = new Color(1, 0, 1, 1);

                Transform armature = centipede.transform.GetChild(5);
                Vector3 localScale = new Vector3(1.1278f, 1.1278f, 1.1278f);
                CreateRune("s", armature.GetChild(0).GetChild(1), Vector3.zero, localScale, sRune, color).transform.localEulerAngles = new Vector3(0, 90, 90);
                CreateRune("eh", armature.GetChild(0).GetChild(1), Vector3.zero, localScale, ehRune, color, material).transform.localEulerAngles = new Vector3(0, 90, 90);
                CreateRune("n", armature.GetChild(0).GetChild(1).GetChild(4), Vector3.zero, localScale, nRune, color).transform.localEulerAngles = new Vector3(0, 90, 90);
                CreateRune("t", armature.GetChild(0).GetChild(1).GetChild(4).GetChild(4), Vector3.zero, localScale, tRune, color).transform.localEulerAngles = new Vector3(0, 90, 90);
                CreateRune("uh", armature.GetChild(0).GetChild(1).GetChild(4).GetChild(4), Vector3.zero, localScale, uhRune, color, material).transform.localEulerAngles = new Vector3(0, 90, 90);
                CreateRune("p", armature.GetChild(0).GetChild(1).GetChild(4).GetChild(4).GetChild(4), Vector3.zero, localScale, pRune, color).transform.localEulerAngles = new Vector3(0, 90, 90);
                CreateRune("E", armature.GetChild(0).GetChild(1).GetChild(4).GetChild(4).GetChild(4), Vector3.zero, localScale, ERune, color, material).transform.localEulerAngles = new Vector3(0, 90, 90);
                CreateRune("E", armature.GetChild(0).GetChild(1).GetChild(4).GetChild(4).GetChild(4).GetChild(4), Vector3.zero, localScale, dRune, color).transform.localEulerAngles = new Vector3(0, 90, 90);
            }

            DidArachnophoiaModeAlready = true;
        }

        private static GameObject CreateRune(string name, Transform parent, Vector3 localPosition, Vector3 localScale, Sprite sprite, Color color, Material material = null) {
            GameObject rune = new GameObject(name);
            rune.transform.parent = parent;
            rune.transform.localPosition = localPosition;
            rune.transform.localScale = localScale;
            rune.AddComponent<SpriteRenderer>().sprite = sprite;
            rune.GetComponent<SpriteRenderer>().color = color;
            if (material != null) {
                rune.GetComponent<SpriteRenderer>().material = material;
            }
            return rune;
        }

        private static void SpawnFleemerQuartet(GameObject Fleemer) {
            List<GameObject> quartet = new List<GameObject> {
                Fleemer
            };
            for (int f = 0; f < 3; f++) {
                GameObject fleemer;
                if (f == 0) {
                    fleemer = GameObject.Instantiate(Fleemer, Fleemer.transform.position + new Vector3(1, 0, -1), Fleemer.transform.rotation);
                } else if (f == 1) {
                    fleemer = GameObject.Instantiate(Fleemer, Fleemer.transform.position + new Vector3(-1, 0, -1), Fleemer.transform.rotation);
                } else {
                    fleemer = GameObject.Instantiate(Fleemer, Fleemer.transform.position + new Vector3(0, 0, -2), Fleemer.transform.rotation);
                }
                fleemer.transform.parent = Fleemer.transform.parent;
                fleemer.SetActive(true);
                fleemer.GetComponent<FleemerQuartet>().GroupId = Fleemer.GetComponent<FleemerQuartet>().GroupId;
                fleemer.name = Fleemer.name;
                quartet.Add(fleemer);
                fleemer.GetComponent<RuntimeStableID>().intraSceneID = Fleemer.GetComponent<RuntimeStableID>().intraSceneID + f;
            }
            foreach (GameObject monster in quartet) {
                monster.GetComponent<FleemerQuartet>().Quartet = quartet;
            }
        }

        public static bool Monster_Die_MoveNext_PrefixPatch(Monster._Die_d__77 __instance, ref bool __result) {
            if (__instance.__4__this.GetComponent<BossAnnounceOnAggro>() != null) {
                if (SceneManager.GetActiveScene().name == "Forest Boss Room") {
                    SaveFile.SetInt("randomizer defeated forest boss", 1);
                    Archipelago.instance.UpdateDataStorage("Defeated Guard Captain", true);
                }
                if (__instance.__4__this.GetComponent<Knightbot>() != null) {
                    SaveFile.SetInt("randomizer defeated garden knight", 1);
                    Archipelago.instance.UpdateDataStorage("Defeated Garden Knight", true);
                }
                if (__instance.__4__this.GetComponent<Spidertank>() != null) {
                    SaveFile.SetInt("randomizer defeated siege engine", 1);
                    Archipelago.instance.UpdateDataStorage("Defeated Siege Engine", true);
                }
                if (__instance.__4__this.GetComponent<Librarian>() != null) {
                    SaveFile.SetInt("randomizer defeated librarian", 1);
                    Archipelago.instance.UpdateDataStorage("Defeated Librarian", true);
                }
                if (__instance.__4__this.GetComponent<ScavengerBoss>() != null) {
                    SaveFile.SetInt("randomizer defeated boss scavenger", 1);
                    Archipelago.instance.UpdateDataStorage("Defeated Boss Scavenger", true);
                }
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
                int EnemiesDefeated = SaveFile.GetInt(EnemiesDefeatedCount);
                SaveFile.SetInt(EnemiesDefeatedCount, EnemiesDefeated + 1);
                if (TunicRandomizer.Settings.EnemyRandomizerEnabled) {
                    if (__instance.__4__this.GetComponent<FleemerQuartet>() != null) {
                        if (__instance.__4__this.GetComponent<FleemerQuartet>().Quartet.Count == 1) {
                            RecordDefeatedEnemy(__instance.__4__this);
                        }
                    } else {
                        RecordDefeatedEnemy(__instance.__4__this);
                    }
                }
            }
        }

        public static void RecordDefeatedEnemy(Monster monster) {
            string SceneName = SceneLoaderPatches.SceneName;
            if (TunicRandomizer.Settings.EnemyRandomizerEnabled && SceneName != "Cathedral Arena") {
                if (!DefeatedEnemyTracker.ContainsKey(SceneName)) {
                    DefeatedEnemyTracker.Add(SceneName, new List<string>());
                }
                if (EnemiesInCurrentScene.ContainsKey(monster.name) && !DefeatedEnemyTracker[SceneName].Contains(EnemiesInCurrentScene[monster.name])) {
                    DefeatedEnemyTracker[SceneName].Add(EnemiesInCurrentScene[monster.name]);
                    TunicLogger.LogInfo("recorded defeated enemy " + monster.name);
                }
            }
        }

        public static bool Campfire_RespawnAtLastCampfire_PrefixPatch(Campfire __instance) {
            DefeatedEnemyTracker.Clear();
            return true;
        }

        public static bool Campfire_Interact_PrefixPatch(Campfire __instance) {
            SaveFile.SetString("randomizer last campfire scene name for dath stone", __instance.gameObject.scene.name);
            SaveFile.SetString("randomizer last campfire id for dath stone", __instance.id);
            DefeatedEnemyTracker.Clear();
            return true;
        }

        public static bool TunicKnightVoid_onFlinch_PrefixPatch(TunicKnightVoid __instance) {
            return false;
        }

        public static void ToggleObjectAnimation_SetToggle_PostfixPatch(ToggleObjectAnimation __instance, ref bool state) {
            if (SceneManager.GetActiveScene().name == "Cathedral Arena" && __instance.name == "Chest Reveal" && state) {
                Archipelago.instance.UpdateDataStorage("Cleared Cathedral Gauntlet", true);
            }
        }

        public static void CathedralGauntletManager_Spawn_PostfixPatch(CathedralGauntletManager __instance, ref GameObject go, ref EventReference sfx) {
            if (go.GetComponent<FleemerQuartet>() != null) {
                TunicLogger.LogInfo("cathedral gauntlet manager spawning fleemer quartet");
                SpawnFleemerQuartet(go);
            }
        }

        public static void Librarian_BehaviourUpdate_PostfixPatch(Librarian __instance) {
            if (__instance.IsAggroed) {
                if (__instance.GetComponent<BossAnnounceOnAggro>() == null) {
                    if (PlayerCharacter.Instanced) {
                        Vector3 temp = __instance.transform.position;
                        temp.y = PlayerCharacter.Instanced ? PlayerCharacter.instance.lastPosition.y : temp.y;
                        __instance.transform.position = temp;
                        __instance.orbitCentre = PlayerCharacter.instance.transform;
                        __instance.orbSpawnPoint = PlayerCharacter.instance.transform;
                    }
                }
            }
        }

        public static bool Monster_OnTouchKillbox_PrefixPatch(Monster __instance) {
            if (__instance.GetComponent<BossEnemy>() != null) {
                GameObject.Destroy(__instance.gameObject);
                if (PlayerCharacter.Instanced) {
                    PlayerCharacter.instance.cutsceneHidden = false;
                    GUIMode.PushGameMode();
                }
                return false;
            }
            return true;
        }

        public static bool Monster_IDamageable_ReceiveDamage_PrefixPatch(Monster __instance, ref int damagePoints) {

            if (__instance.GetComponent<Foxgod>() != null && __instance.gameObject.scene.name == "Spirit Arena" && SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                return false;
            }
            if (__instance.name == "_Fox(Clone)") {
                if (CustomItemBehaviors.CanTakeGoldenHit) {
                    GameObject.Find("_Fox(Clone)/fox").GetComponent<CreatureMaterialManager>().originalMaterials = CustomItemBehaviors.FoxBody.GetComponent<MeshRenderer>().materials;
                    GameObject.Find("_Fox(Clone)/fox hair").GetComponent<CreatureMaterialManager>().originalMaterials = CustomItemBehaviors.FoxHair.GetComponent<MeshRenderer>().materials;
                    GameObject.Find("_Fox(Clone)/fox").GetComponent<CreatureMaterialManager>()._ghostMaterialArray = CustomItemBehaviors.GhostFoxBody.GetComponent<MeshRenderer>().materials;
                    GameObject.Find("_Fox(Clone)/fox hair").GetComponent<CreatureMaterialManager>()._ghostMaterialArray = CustomItemBehaviors.GhostFoxHair.GetComponent<MeshRenderer>().materials;
                    PaletteEditor.FoxCape.GetComponent<CreatureMaterialManager>()._ghostMaterialArray = CustomItemBehaviors.GhostFoxBody.GetComponent<MeshRenderer>().materials;
                    PaletteEditor.FoxCape.GetComponent<CreatureMaterialManager>().originalMaterials = CustomItemBehaviors.FoxCape.GetComponent<MeshRenderer>().materials;

                    SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
                    CustomItemBehaviors.CanTakeGoldenHit = false;
                    return false;
                }
            } else {
                if (__instance.GetComponent<BossEnemy>() != null) {
                    if (__instance.hp - damagePoints <= 0) {
                        CoinSpawner.SpawnCoins(256, __instance.transform.position);
                        GameObject.Destroy(__instance.gameObject);
                        RecordDefeatedEnemy(__instance);
                        if (GameObject.FindObjectOfType<PlayMusicOnLoad>() != null) {
                            MusicManager.PlayNewTrackIfDifferent(GameObject.FindObjectOfType<PlayMusicOnLoad>().track);
                        }
                        return false;
                    }
                }
                if (__instance.GetComponent<Foxgod>() != null) {
                    if (TunicRandomizer.Settings.HeirAssistModeEnabled) {
                        __instance.hp -= PlayerCharacterPatches.HeirAssistModeDamageValue;
                    }
                    if (__instance.hp == __instance.maxhp) {
                        __instance.Flinch(true);
                        return true;
                    }
                }
                if (CustomItemBehaviors.CanSwingGoldenSword) {
                    __instance.hp -= 30;
                    GameObject Hand = GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R");
                    if (Hand != null) {
                        Hand.transform.GetChild(1).GetComponent<MeshRenderer>().materials = ModelSwaps.Items["Sword"].GetComponent<MeshRenderer>().materials;
                        if (Hand.transform.childCount >= 12) {
                            Hand.transform.GetChild(12).GetChild(4).GetComponent<MeshRenderer>().materials = ModelSwaps.SecondSword.GetComponent<MeshRenderer>().materials;
                            Hand.transform.GetChild(13).GetChild(4).GetComponent<MeshRenderer>().materials = ModelSwaps.ThirdSword.GetComponent<MeshRenderer>().materials;
                        }
                    }
                    SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
                    CustomItemBehaviors.CanSwingGoldenSword = false;
                }
            }
            return true;
        }

        public static void CheckBossState() {
            if (GameObject.FindObjectOfType<BossAnnounceOnAggro>() != null) {
                GameObject boss = GameObject.FindObjectOfType<BossAnnounceOnAggro>().gameObject;
                if (boss.GetComponent<Knightbot>() != null && SaveFile.GetInt(CustomBossFlags[1]) == 1) {
                    GameObject.Destroy(boss);
                    if (GameObject.Find("_DAYTIME BOSS/Simple Bloodgate/") != null) {
                        GameObject.Find("_DAYTIME BOSS/Simple Bloodgate/").SetActive(false);
                    }
                }
                if (boss.GetComponent<Spidertank>() != null && SaveFile.GetInt(CustomBossFlags[2]) == 1) {
                    GameObject.Destroy(boss);
                    GameObject itemPickup = GameObject.Find("_AfterFight");
                    if (itemPickup != null && itemPickup.transform.childCount >= 3) {
                        itemPickup.transform.GetChild(3).gameObject.SetActive(true);
                    }
                }
                if (boss.GetComponent<Librarian>() != null && SaveFile.GetInt(CustomBossFlags[3]) == 1) {
                    GameObject.Destroy(boss);
                    foreach (ItemPickup itemPickup in Resources.FindObjectsOfTypeAll<ItemPickup>().Where(item => item.gameObject.scene.name == SceneManager.GetActiveScene().name && item.itemToGive.name == "Hexagon Green")) {
                        itemPickup.gameObject.SetActive(true);
                        itemPickup.transform.position = new Vector3(0.22f, 1.28f, 0.38f);
                        itemPickup.transform.parent = null;
                    }
                }
                if (boss.GetComponent<ScavengerBoss>() != null && SaveFile.GetInt(CustomBossFlags[4]) == 1) {
                    GameObject.Destroy(boss);
                    GameObject plinth = GameObject.Find("_Plinth");
                    if (plinth != null && plinth.transform.childCount >= 2) {
                        TunicLogger.LogInfo("activating plinth");
                        plinth.transform.GetChild(2).GetChild(0).parent = plinth.transform;
                    }
                }
            }
        }
    }
}
