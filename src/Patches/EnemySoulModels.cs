using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using UnityEngine;
using UnityEngine.AI;

namespace TunicRandomizer {
    public class EnemySoulModels {

        public static Dictionary<string, GameObject> EnemyPresentationObjs = new Dictionary<string, GameObject>();

        public static Dictionary<string, (string, Vector3, Vector3)> enemySoulToPrefab = new Dictionary<string, (string, Vector3, Vector3)>() {
            { "Enemy Soul (Administrator)", ("administrator", Vector3.one, Vector3.one) },
            { "Enemy Soul (Phrend)", ("Bat", new Vector3(0, -8, 0), Vector3.one * 4) },
            { "Enemy Soul (Beefboy)", ("beefboy", new Vector3(0, -7, 0), Vector3.one * 1.8f) },
            { "Enemy Soul (Blobs)", ("BlobBig", new Vector3(0, 0, 0), new Vector3(1, 1, 1)) },
            { "Enemy Soul (Fleemers)", ("bomezome_easy_ghost", new Vector3(0, -7, -1), Vector3.one * 3f) },
            { "Enemy Soul (Crabs)", ("Crabbo", new Vector3(0, 0, -1), new Vector3(1, 1, 1)) },
            { "Enemy Soul (Chompignom)", ("crocodoo", new Vector3(0, -3, 0), Vector3.one * 1.75f) },
            { "Enemy Soul (Husher)", ("Crow", new Vector3(0, 0, 0), new Vector3(1, 1, 1)) },
            { "Enemy Soul (Autobolt)", ("Turret", new Vector3(0, 0, 0), new Vector3(1, 1, 1)) },
            { "Enemy Soul (Zombie Foxes)", ("Fox enemy zombie", new Vector3(0, -7, 0), Vector3.one * 4) },
            { "Enemy Soul (Frogs)", ("Frog", new Vector3(0, -5, -2), Vector3.one * 2) },
            { "Enemy Soul (Lost Echo)", ("Ghostfox_monster", new Vector3(0, -8, -1), Vector3.one * 3f) },
            { "Enemy Soul (Gunslinger)", ("Gunslinger", new Vector3(0, -7, 0), Vector3.one * 2.5f) },
            { "Enemy Soul (Hedgehogs)", ("HedgehogBig", new Vector3(0, -7, -1), Vector3.one * 4) },
            { "Enemy Soul (Laser Trap)", ("Hedgehog Trap", new Vector3(0, -6, 0), Vector3.one * 3.5f) },
            { "Enemy Soul (Envoy)", ("Honourguard", new Vector3(0, -6, 0), Vector3.one * 2.5f) },
            { "Enemy Soul (Garden Knight)", ("tech knight boss", new Vector3(0, -6, 0), Vector3.one * 2) },
            { "Enemy Soul (Librarian)", ("Librarian", new Vector3(0f, -6.5f, -3f), Vector3.one * 1.5f) },
            { "Enemy Soul (Plover)", ("plover", new Vector3(0, -6, 0), Vector3.one * 2.5f) },
            { "Enemy Soul (Fairies)", ("Fairyprobe Archipelagos", new Vector3(0, -3, -6), Vector3.one * 3) },
            { "Enemy Soul (Scavengers)", ("Scavenger", new Vector3(0, -5, 0), Vector3.one * 2) },
            { "Enemy Soul (Boss Scavenger)", ("Scavenger Boss", new Vector3(0, -7, 0), Vector3.one * 2) },
            { "Enemy Soul (Tentacle)", ("sewertentacle", new Vector3(0, 6, 0), Vector3.one * 3) },
            { "Enemy Soul (Rudelings)", ("Skuladot redux", new Vector3(0, -6, 0),  Vector3.one * 2) },
            { "Enemy Soul (Spiders)", ("Spider Big", new Vector3(0, -5, 0), Vector3.one * 3) },
            { "Enemy Soul (Siege Engine)", ("Spidertank", new Vector3(0, -6, 2), Vector3.one * 0.6f) },
            { "Enemy Soul (Slorm)", ("Spinnerbot Corrupted", new Vector3(0, -5, 0), Vector3.one * 4f) },
            { "Enemy Soul (Baby Slorm)", ("Spinnerbot Baby", new Vector3(0, -5, 0), Vector3.one * 2.5f) },
            { "Enemy Soul (Voidling)", ("voidling redux", new Vector3(0, -5, 0), Vector3.one * 1.5f) },
            { "Enemy Soul (Custodians)", ("Wizard_Candleabra", new Vector3(0, 0, 0), new Vector3(1, 1, 1)) },
            { "Enemy Soul (Voidtouched)", ("Voidtouched", new Vector3(0, -8, -1), Vector3.one * 2.5f) },
            { "Enemy Soul (The Heir)", ("Foxgod", new Vector3(0f, -9f, 1f), Vector3.one * 1.8f) },
        };

        public static void OnEnemySoulItemGet(string ItemName) {
            foreach (string key in EnemyPresentationObjs.Keys) {
                EnemyPresentationObjs[key].SetActive(key == ItemName);
            }
            if (ItemName == "Enemy Soul (The Heir)") {
                SetHeirMaterials(EnemyPresentationObjs[ItemName]);
            }
            EnemySoulManager enemySoulManager = GameObject.FindObjectOfType<EnemySoulManager>();
            if (enemySoulManager != null) {
                enemySoulManager.onItemGet(ItemName);
            }
        }

        public static void SetHeirMaterials(GameObject heir) {
            foreach (SkinnedMeshRenderer renderer in heir.GetComponentsInChildren<SkinnedMeshRenderer>()) {
                if (SaveFlags.GetBool(SaveFlags.HexagonQuestEnabled)) {
                    renderer.materials = ModelSwaps.Items["Hexagon Gold"].GetComponent<MeshRenderer>().materials;
                } else {
                    renderer.materials = new Material[] { ModelSwaps.FindMaterial("foxgod_glowyfox") };
                }
            }
        }

        public static void SetupEnemyPresentations(Transform root) {
            foreach (ItemData item in ItemLookup.Items.Values.Where(item => item.Type == ItemTypes.ENEMY)) {
               // EnemyPresentationObjs.Add(item.Name, setupEnemyObject(item.Name, root.transform, null, new Material[] { }));
                //EnemyPresentationObjs.Add(item.Name, cloneEnemy(item.Name, root));

            }
            setupAdministrator(root);
            setupPhrend(root);
            setupBeefboy(root);
            setupBlobs(root);
            setupFleemer(root);
            setupCrabs(root);
            setupChompignom(root);
            setupHusher(root);
            setupAutobolt(root);
            setupZombieFox(root);
            setupFrogs(root);
            setupLostEcho(root);
            setupGunslinger(root);
            setupHedgehogs(root);
            setupLaserTrap(root);
            setupEnvoy(root);
            setupGardenKnight(root);
            setupLibrarian(root);
            setupSiegeEngine(root);
            setupBossScavenger(root);
            setupPlover(root);
            setupFairies(root);
            setupScavengers(root);
            setupTentacles(root);
            setupRudeling(root);
            setupSpiders(root);
            setupSlorm(root);
            setupBabySlorm(root);
            setupVoidlings(root);
            setupCustodians(root);
            setupVoidtouched(root);
            setupHeir(root);
        }

        private static void setupAdministrator(Transform parent) {
            SkinnedMeshRenderer renderer = EnemyRandomizer.Enemies["administrator"].transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
            GameObject administrator = setupEnemyObject("Enemy Soul (Administrator)", parent, renderer.sharedMesh, renderer.materials);
            administrator.transform.localPosition = new Vector3(0,-7, 0);
            administrator.transform.localScale = Vector3.one * 1.8f;
            GameObject eyes = GameObject.Instantiate(EnemyRandomizer.Enemies["administrator"].transform.Find("Armature/root/body/eyes/").gameObject);
            eyes.layer = 5;
            eyes.transform.parent = administrator.transform;
            eyes.transform.localPosition = new Vector3(0, 1.6f, 0);
            eyes.transform.localScale = Vector3.one;
            foreach (Transform t in eyes.GetComponentsInChildren<Transform>()) {
                t.gameObject.layer = 5;
            }
            EnemyPresentationObjs.Add("Enemy Soul (Administrator)", administrator);
        }

        private static void setupPhrend(Transform parent) {
            GameObject phrend = cloneEnemy("Enemy Soul (Phrend)", parent);
            GameObject.Destroy(phrend.GetComponentInChildren<MaterialByParameter>());
            GameObject.Destroy(phrend.GetComponentInChildren<CreatureMaterialManager>());
            phrend.GetComponentInChildren<SkinnedMeshRenderer>().materials = EnemyRandomizer.Enemies["Bat"].GetComponentInChildren<SkinnedMeshRenderer>().materials;
            EnemyPresentationObjs.Add("Enemy Soul (Phrend)", phrend);

            GameObject skullHolder = new GameObject("fleemer toss skull obj holder");
            skullHolder.SetActive(false);
            GameObject.DontDestroyOnLoad(skullHolder);
            EnemyDropShuffle.FleemerTossReplacement = GameObject.Instantiate(phrend.GetComponentsInChildren<Rigidbody>(true)[1].gameObject).GetComponent<Rigidbody>();
            EnemyDropShuffle.FleemerTossReplacement.transform.parent = skullHolder.transform;
            EnemyDropShuffle.FleemerTossReplacement.gameObject.SetActive(true);
        }

        private static void setupBeefboy(Transform parent) {
            GameObject beefboy = cloneEnemy("Enemy Soul (Beefboy)", parent);
            EnemyPresentationObjs.Add("Enemy Soul (Beefboy)", beefboy);
        }

        private static void setupBlobs(Transform parent) {
            GameObject blob1 = setupEnemyObject("Enemy Soul (Blobs)", parent, null, new Material[] { });
            updateEnemyObject(blob1, EnemyRandomizer.Enemies["BlobBig"].transform.GetChild(1).gameObject, new Vector3(0, -5, 0), Vector3.one * 5);
            GameObject smallBlob = setupEnemyObject("blob 2", blob1.transform, null, new Material[] { });
            updateEnemyObject(smallBlob, EnemyRandomizer.Enemies["Blob"].transform.GetChild(1).gameObject, new Vector3(0, 1.2f, 0), Vector3.one * 0.5f, setActive: true);
            EnemyPresentationObjs.Add("Enemy Soul (Blobs)", blob1);
        }

        private static void setupFleemer(Transform parent) {
            GameObject fleemer = cloneEnemy("Enemy Soul (Fleemers)", parent);
            Material[] bomezomeMats = EnemyRandomizer.Enemies["bomezome_easy"].GetComponentInChildren<SkinnedMeshRenderer>().materials;
            foreach (SkinnedMeshRenderer renderer in fleemer.GetComponentsInChildren<SkinnedMeshRenderer>()) { 
                renderer.materials = bomezomeMats;
            }
            fleemer.GetComponentInChildren<MeshRenderer>().materials = bomezomeMats;
            EnemyPresentationObjs.Add("Enemy Soul (Fleemers)", fleemer);
        }

        private static void setupCrabs(Transform parent) {
            GameObject crab1 = setupEnemyObject("Enemy Soul (Crabs)", parent, null, new Material[] { });
            updateEnemyObject(crab1, EnemyRandomizer.Enemies["Crabbo"].transform.GetChild(1).gameObject, new Vector3(0, -4, -2), Vector3.one * 2f);
            GameObject crabbit = setupEnemyObject("crabbit", crab1.transform, null, new Material[] { });
            updateEnemyObject(crabbit, EnemyRandomizer.Enemies["Crabbit"].transform.GetChild(1).gameObject, new Vector3(0, 1.5f, 0), Vector3.one * 0.5f, true);

            EnemyPresentationObjs.Add("Enemy Soul (Crabs)", crab1);
        }

        private static void setupChompignom(Transform parent) {
            GameObject chompignom = cloneEnemy("Enemy Soul (Chompignom)", parent);
            GameObject.Destroy(chompignom.transform.Find("Armature/root/body/terryterryterry").gameObject);
            GameObject terry = setupEnemyObject("terry", chompignom.transform, null, new Material[] { });
            updateEnemyObject(terry, EnemyRandomizer.Enemies["crocodoo"].transform.GetChild(3).GetChild(0).GetChild(0).GetChild(6).gameObject, new Vector3(-0.05f, 2.8f, 2.05f), Vector3.one * 5.1603f, true);
            terry.transform.localEulerAngles = new Vector3(345f, 0, 0);
            EnemyPresentationObjs.Add("Enemy Soul (Chompignom)", chompignom);
        }

        private static void setupHusher(Transform parent) {
            GameObject husher = setupEnemyObject("Enemy Soul (Husher)", parent, null, new Material[] { });
            updateEnemyObject(husher, EnemyRandomizer.Enemies["Crow"].transform.GetChild(3).gameObject, new Vector3(0, -4, 0), Vector3.one * 0.9f);
            EnemyPresentationObjs.Add("Enemy Soul (Husher)", husher);
        }

        private static void setupAutobolt(Transform parent) {
            GameObject autobolt = setupEnemyObject("Enemy Soul (Autobolt)", parent, null, new Material[] { });
            updateEnemyObject(autobolt, EnemyRandomizer.Enemies["Turret"].transform.GetChild(0).gameObject, new Vector3(0, -2, 0), Vector3.one * 3f);
            EnemyPresentationObjs.Add("Enemy Soul (Autobolt)", autobolt);
        }

        private static void setupZombieFox(Transform parent) {
            GameObject zombieFox = cloneEnemy("Enemy Soul (Zombie Foxes)", parent);
            EnemyPresentationObjs.Add("Enemy Soul (Zombie Foxes)", zombieFox);
        }

        private static void setupFrogs(Transform parent) {
            GameObject frog = cloneEnemy("Enemy Soul (Frogs)", parent);
            GameObject smallFrog = setupEnemyObject("small frog", frog.transform, null, new Material[] { });
            updateEnemyObject(smallFrog, EnemyRandomizer.Enemies["Frog Small"].transform.GetChild(0).gameObject, new Vector3(0f, 0f, 2.5f), Vector3.one * 0.5f);
            smallFrog.SetActive(true);
            EnemyPresentationObjs.Add("Enemy Soul (Frogs)", frog);
        }

        private static void setupLostEcho(Transform parent) {
            GameObject lostEcho = cloneEnemy("Enemy Soul (Lost Echo)", parent);
            EnemyPresentationObjs.Add("Enemy Soul (Lost Echo)", lostEcho);
        }

        private static void setupGunslinger(Transform parent) {
            GameObject gunman = cloneEnemy("Enemy Soul (Gunslinger)", parent);
            GameObject.Destroy(gunman.transform.GetChild(1).gameObject);
            GameObject.Destroy(gunman.transform.GetChild(0).gameObject);
            EnemyPresentationObjs.Add("Enemy Soul (Gunslinger)", gunman);
        }

        private static void setupHedgehogs(Transform parent) {
            GameObject hedgehog = cloneEnemy("Enemy Soul (Hedgehogs)", parent);
            EnemyPresentationObjs.Add("Enemy Soul (Hedgehogs)", hedgehog);
        }

        private static void setupLaserTrap(Transform parent) {
            GameObject hedgehogTrap = cloneEnemy("Enemy Soul (Laser Trap)", parent);
            GameObject.Destroy(hedgehogTrap.transform.GetChild(2).gameObject);
            EnemyPresentationObjs.Add("Enemy Soul (Laser Trap)", hedgehogTrap);
        }

        private static void setupEnvoy(Transform parent) {
            GameObject envoy = cloneEnemy("Enemy Soul (Envoy)", parent);
            EnemyPresentationObjs.Add("Enemy Soul (Envoy)", envoy);
        }

        private static void setupGardenKnight(Transform parent) {
            GameObject gardenKnight = cloneEnemy("Enemy Soul (Garden Knight)", parent);
            gardenKnight.transform.Find("Armature/root/torso/upper arm_R/lower arm_R/sword/").localPosition = new Vector3(0, 0, -1);
            gardenKnight.transform.Find("Armature/root/torso/upper arm_R/lower arm_R/sword/techknight sword/").localPosition = new Vector3(1, -3, 0);
            for (int i = 10; i >= 3; i--) {
                GameObject.Destroy(gardenKnight.transform.GetChild(i).gameObject);
            }
            EnemyPresentationObjs.Add("Enemy Soul (Garden Knight)", gardenKnight);
        }
        
        private static void setupLibrarian(Transform parent) {
            GameObject librarian = cloneEnemy("Enemy Soul (Librarian)", parent);
            for (int i = librarian.transform.childCount - 1; i >= 5; i--) { 
                GameObject.Destroy(librarian.transform.GetChild(i).gameObject);
            }
            GameObject.Destroy(librarian.transform.Find("Librarian_Skeleton/root/pelvis/chest/shoulder1.L/shoulder2.L/upper_arm.L/lower_arm.L/wrist.L/hand.L/hand_attach/Sword/PS: sparks short").gameObject);
            EnemyPresentationObjs.Add("Enemy Soul (Librarian)", librarian);
        }

        private static void setupBossScavenger(Transform parent) {
            GameObject bossScavenger = cloneEnemy("Enemy Soul (Boss Scavenger)", parent);
            for (int i = bossScavenger.transform.childCount - 1; i >= 3; i--) {
                GameObject.Destroy(bossScavenger.transform.GetChild(i).gameObject);
            }
            GameObject.Destroy(bossScavenger.transform.Find("scavenger_boss_rig/root/torso/MCH-spine.002/MCH-spine.003/tweak_spine.003/ORG-spine.003/ORG-shoulder.R/ORG-upper_arm.R/ORG-forearm.R/ORG-hand.R/held.R 1/shieldsword/Point Light/").gameObject);
            EnemyPresentationObjs.Add("Enemy Soul (Boss Scavenger)", bossScavenger);
        }

        private static void setupSiegeEngine(Transform parent) {
            GameObject siegeEngine = cloneEnemy("Enemy Soul (Siege Engine)", parent);
            for (int i = siegeEngine.transform.childCount - 1; i >= 2; i--) {
                GameObject.Destroy(siegeEngine.transform.GetChild(i).gameObject);
            }
            siegeEngine.transform.Find("Spidertank_skeleton/root/thorax/Light: core/").gameObject.SetActive(false);
            EnemyPresentationObjs.Add("Enemy Soul (Siege Engine)", siegeEngine);
        }

        private static void setupPlover(Transform parent) {
            GameObject plover = cloneEnemy("Enemy Soul (Plover)", parent);
            EnemyPresentationObjs.Add("Enemy Soul (Plover)", plover);
        }

        private static void setupFairies(Transform parent) {
            GameObject fairies = new GameObject("Enemy Soul (Fairies)");
            fairies.transform.parent = parent;
            fairies.transform.localPosition = new Vector3(0, -2, 0);
            fairies.transform.localScale = Vector3.one * 2.5f;
            GameObject fairy1 = cloneEnemyPrefab("Fairyprobe Archipelagos (Ghost)", fairies.transform);
            fairy1.transform.localPosition = new Vector3(2f, 1.5f, -1f);
            fairy1.transform.localEulerAngles = new Vector3(345f, 130f, 0f);
            fairy1.SetActive(true);
            GameObject fairy2 = cloneEnemyPrefab("Fairyprobe Archipelagos (Dmg)", fairies.transform);
            fairy2.transform.localPosition = new Vector3(-0.5f, -2f, -1f);
            fairy2.transform.localEulerAngles = new Vector3(15f, 220f, 0f);
            fairy2.SetActive(true);
            GameObject fairy3 = cloneEnemyPrefab("Fairyprobe Archipelagos", fairies.transform);
            fairy3.transform.localPosition = new Vector3(-1f, 0f, 2f);
            fairy3.transform.localEulerAngles = new Vector3(345f, 315f, 0f);
            fairy3.SetActive(true);
            fairies.SetActive(false);
            EnemyPresentationObjs.Add("Enemy Soul (Fairies)", fairies);
        }
        
        private static void setupScavengers(Transform parent) {
            GameObject scavengers = new GameObject("Enemy Soul (Scavengers)");
            scavengers.transform.parent = parent;
            scavengers.transform.localPosition = new Vector3(0, -6, 0);
            scavengers.transform.localScale = Vector3.one * 2.25f;
            GameObject scavenger1 = cloneEnemyPrefab("Scavenger", scavengers.transform);
            scavenger1.transform.localPosition = new Vector3(2f, 0f, -1f);
            scavenger1.transform.localEulerAngles = new Vector3(0f, 130f, 0f);
            scavenger1.SetActive(true);
            GameObject scavenger2 = cloneEnemyPrefab("Scavenger_miner", scavengers.transform);
            scavenger2.transform.localPosition = new Vector3(-1.1f, 0f, -0.6f);
            scavenger2.transform.localEulerAngles = new Vector3(0f, 220f, 0f);
            scavenger2.SetActive(true);
            GameObject scavenger3 = cloneEnemyPrefab("Scavenger_support", scavengers.transform);
            scavenger3.transform.localPosition = new Vector3(0f, 0f, 2f);
            scavenger3.transform.localEulerAngles = new Vector3(0f, 345f, 0f);
            scavenger3.SetActive(true);
            scavengers.SetActive(false);
            EnemyPresentationObjs.Add("Enemy Soul (Scavengers)", scavengers);
        }

        private static void setupTentacles(Transform parent) {
            GameObject tentacle = cloneEnemy("Enemy Soul (Tentacle)", parent);
            EnemyPresentationObjs.Add("Enemy Soul (Tentacle)", tentacle);
        }
        
        private static void setupRudeling(Transform parent) {
            GameObject rudelings = new GameObject("Enemy Soul (Rudelings)");
            rudelings.transform.parent = parent;
            rudelings.transform.localPosition = new Vector3(0, -5, -1);
            rudelings.transform.localScale = Vector3.one * 2;
            GameObject rudeling1 = cloneEnemyPrefab("Skuladot redux", rudelings.transform);
            rudeling1.transform.localPosition = new Vector3(2f, 0f, -1f);
            rudeling1.transform.localEulerAngles = new Vector3(0f, 130f, 0f);
            rudeling1.transform.localScale = Vector3.one * 1.25f;
            rudeling1.SetActive(true);
            GameObject rudelingShield = cloneEnemyPrefab("Skuladot redux_shield", rudelings.transform);
            rudelingShield.transform.localPosition = new Vector3(-1.5f, 0f, -1f);
            rudelingShield.transform.localEulerAngles = new Vector3(0f, 220f, 0f);
            rudelingShield.transform.localScale = Vector3.one * 1.25f;
            rudelingShield.SetActive(true);
            GameObject guardCaptain = cloneEnemyPrefab("Skuladot redux Big", rudelings.transform);
            guardCaptain.transform.localPosition = new Vector3(0f, 0f, 2f);
            guardCaptain.transform.localEulerAngles = new Vector3(0f, 345f, 0f);
            guardCaptain.transform.localScale = Vector3.one * 1.75f;
            guardCaptain.SetActive(true);
            rudelings.SetActive(false);
            EnemyPresentationObjs.Add("Enemy Soul (Rudelings)", rudelings);
        }

        private static void setupSpiders(Transform parent) {
            GameObject spider = cloneEnemy("Enemy Soul (Spiders)", parent);
            GameObject spiderSmall = cloneEnemyPrefab("Spider Small", spider.transform);
            spiderSmall.transform.localEulerAngles = Vector3.zero;
            spiderSmall.transform.localPosition = new Vector3(0, 1.5f, 0);
            spiderSmall.transform.localScale = Vector3.one / 2;
            spiderSmall.SetActive(true);
            EnemyPresentationObjs.Add("Enemy Soul (Spiders)", spider);
        }

        private static void setupSlorm(Transform parent) {
            GameObject slorm = cloneEnemy("Enemy Soul (Slorm)", parent);
            EnemyPresentationObjs.Add("Enemy Soul (Slorm)", slorm);
        }

        private static void setupBabySlorm(Transform parent) {
            GameObject slorm = cloneEnemy("Enemy Soul (Baby Slorm)", parent);
            EnemyPresentationObjs.Add("Enemy Soul (Baby Slorm)", slorm);
        }

        private static void setupVoidlings(Transform parent) {
            GameObject voidlings = new GameObject("Enemy Soul (Voidling)");
            voidlings.transform.parent = parent;
            voidlings.transform.localPosition = new Vector3(0f, -1f, 1.5f);
            voidlings.transform.localScale = Vector3.one * 2;
            GameObject voidlingSpider = setupEnemyObject("voidling", voidlings.transform, null, new Material[] { });
            updateEnemyObject(voidlingSpider, EnemyRandomizer.Enemies["voidling redux"].transform.GetChild(1).gameObject, new Vector3(0, -2, 0), Vector3.one, true);
            voidlingSpider.GetComponentInChildren<MeshRenderer>().material.shader = ModelSwaps.FindShader("Shader Forge/oozelake");
            //GameObject voidtouched = cloneEnemyPrefab("Voidtouched", voidlings.transform);
            //voidtouched.SetActive(true);
            voidlings.SetActive(false);
            EnemyPresentationObjs.Add("Enemy Soul (Voidling)", voidlings);
        }

        private static void setupCustodians(Transform parent) {
            GameObject custodians = new GameObject("Enemy Soul (Custodians)");
            custodians.transform.parent = parent;
            custodians.transform.localPosition = new Vector3(-0.5f, -7f, -1f);
            custodians.transform.localScale = Vector3.one * 2;
            GameObject custodian1 = cloneEnemyPrefab("Wizard_Sword", custodians.transform);
            custodian1.transform.localPosition = new Vector3(2f, 0f, -1f);
            custodian1.transform.localEulerAngles = new Vector3(0f, 130f, 0f);
            custodian1.transform.localScale = Vector3.one * 1.25f;
            GameObject.Destroy(custodian1.transform.Find("wizard_armature/root/body_bottom/body_mid/body_top/shoulder_L/upperarm_L/forearm_L/hand_L/held_L/sword/Lit/").gameObject);
            custodian1.SetActive(true);
            GameObject custodian2 = cloneEnemyPrefab("Wizard_Support", custodians.transform);
            custodian2.transform.localPosition = new Vector3(-1.5f, 0f, -1f);
            custodian2.transform.localEulerAngles = new Vector3(0f, 220f, 0f);
            custodian2.transform.localScale = Vector3.one * 1.25f;
            custodian2.SetActive(true);
            GameObject custodian3 = cloneEnemyPrefab("Wizard_Candleabra", custodians.transform);
            custodian3.transform.localPosition = new Vector3(0f, 0f, 2f);
            custodian3.transform.localEulerAngles = Vector3.zero;
            custodian3.transform.localScale = Vector3.one * 1.75f;
            GameObject.Destroy(custodian3.transform.Find("wizard_armature/root/body_bottom/body_mid/body_top/shoulder_L/upperarm_L/forearm_L/hand_L/held_L/sword/lit/").gameObject);
            GameObject.Destroy(custodian3.transform.Find("wizard_armature/root/body_bottom/body_mid/body_top/shoulder_R/upperarm_R/forearm_R/hand_R/held_R/candelabra/candleabra flames/").gameObject);
            custodian3.SetActive(true);

            custodians.SetActive(false);
            EnemyPresentationObjs.Add("Enemy Soul (Custodians)", custodians);
        }

        private static void setupVoidtouched(Transform parent) {
            GameObject voidtouched = cloneEnemy("Enemy Soul (Voidtouched)", parent);
            for (int i = voidtouched.transform.childCount - 1; i >= 2; i--) {
                GameObject.Destroy(voidtouched.transform.GetChild(i).gameObject);
            }
            foreach (ParticleSystem ps in voidtouched.GetComponentsInChildren<ParticleSystem>(true)) { 
                GameObject.Destroy(ps.gameObject);
            }
            EnemyPresentationObjs.Add("Enemy Soul (Voidtouched)", voidtouched);
        }

        private static void setupHeir(Transform parent) {
            GameObject heir = cloneEnemy("Enemy Soul (The Heir)", parent);
            for (int i = heir.transform.childCount - 1; i >= 4; i--) { 
                GameObject.Destroy(heir.transform.GetChild(i).gameObject);
            }
            GameObject.Destroy(heir.transform.GetChild(2).gameObject);
            GameObject.Destroy(heir.transform.Find("rig/root/torso/MCH-spine.002/MCH-spine.003/tweak_spine.003/ORG-spine.003/ORG-shoulder.R/ORG-upper_arm.R/ORG-forearm.R/ORG-hand.R/held.R 1/elderfox_sword/").gameObject);
            foreach (CreatureMaterialManager manager in heir.GetComponentsInChildren<CreatureMaterialManager>()) {
                GameObject.Destroy(manager);
            }
            EnemyPresentationObjs.Add("Enemy Soul (The Heir)", heir);
        }
        private static GameObject cloneEnemy(string enemySoul, Transform parent) {
            GameObject newObj = GameObject.Instantiate(EnemyRandomizer.Enemies[enemySoulToPrefab[enemySoul].Item1]);
            newObj.name = enemySoul;
            foreach (Transform t in newObj.GetComponentsInChildren<Transform>()) {
                t.gameObject.layer = 5;
            }
            newObj.transform.parent = parent;
            newObj.transform.localPosition = enemySoulToPrefab[enemySoul].Item2;
            newObj.transform.localScale = enemySoulToPrefab[enemySoul].Item3;
            newObj.transform.localEulerAngles = Vector3.zero;
            newObj.SetActive(false);
            cleanupEnemyComponents(newObj);
            return newObj;
        }

        private static GameObject cloneEnemyPrefab(string prefabName, Transform parent) {
            GameObject newObj = GameObject.Instantiate(EnemyRandomizer.Enemies[prefabName]);
            newObj.name = prefabName;
            foreach (Transform t in newObj.GetComponentsInChildren<Transform>()) {
                t.gameObject.layer = 5;
            }
            newObj.transform.parent = parent;
            newObj.transform.localPosition = Vector3.zero;
            newObj.transform.localScale = Vector3.one;
            newObj.transform.localEulerAngles = Vector3.zero;
            newObj.SetActive(false);
            cleanupEnemyComponents(newObj);
            return newObj;
        }

        private static void updateEnemyObject(GameObject obj, GameObject obj2, Vector3 pos, Vector3 sc, bool setActive = false) {
            if (obj2.GetComponent<SkinnedMeshRenderer>() != null) {
                obj.GetComponent<MeshFilter>().mesh = obj2.GetComponent<SkinnedMeshRenderer>().sharedMesh;
                obj.GetComponent<MeshRenderer>().materials = obj2.GetComponent<SkinnedMeshRenderer>().materials;
            }
            if (obj2.GetComponent<MeshFilter>() != null) {
                obj.GetComponent<MeshFilter>().mesh = obj2.GetComponent<MeshFilter>().mesh;
                obj.GetComponent<MeshRenderer>().materials = obj2.GetComponent<MeshRenderer>().materials;
            }
            obj.transform.localPosition = pos;
            obj.transform.localScale = sc;
            obj.transform.localEulerAngles = Vector3.zero;
            obj.SetActive(setActive);
        }

        private static GameObject setupEnemyObject(string objName, Transform parent, Mesh mesh, Material[] materials) {
            GameObject newObj = new GameObject(objName);
            newObj.name = objName;
            newObj.AddComponent<MeshFilter>().mesh = mesh;
            newObj.AddComponent<MeshRenderer>().materials = materials;
            newObj.layer = 5;
            newObj.transform.parent = parent;
            newObj.transform.localEulerAngles = Vector3.zero;
            newObj.SetActive(false);
            return newObj;
        }

        private static void cleanupEnemyComponents (GameObject enemy) {
            if (enemy.GetComponent<Monster>() != null) GameObject.Destroy(enemy.GetComponent<Monster>());
            if (enemy.GetComponent<Animator>() != null) GameObject.Destroy(enemy.GetComponent<Animator>());
            if (enemy.GetComponent<RuntimeStableID>() != null) GameObject.Destroy(enemy.GetComponent<RuntimeStableID>());
            if (enemy.GetComponent<Rigidbody>() != null) GameObject.Destroy(enemy.GetComponent<Rigidbody>());
            if (enemy.GetComponent<FireController>() != null) GameObject.Destroy(enemy.GetComponent<FireController>());
            if (enemy.GetComponent<ZTarget>() != null) GameObject.Destroy(enemy.GetComponent<ZTarget>());
            if (enemy.GetComponent<HitReceiver>() != null) GameObject.Destroy(enemy.GetComponent<HitReceiver>());
            if (enemy.GetComponent<CapsuleCollider>() != null) GameObject.Destroy(enemy.GetComponent<CapsuleCollider>());
            if (enemy.GetComponent<TetherTarget>() != null) GameObject.Destroy(enemy.GetComponent<TetherTarget>());
            if (enemy.GetComponent<CoinSpawner>() != null) GameObject.Destroy(enemy.GetComponent<CoinSpawner>());
            if (enemy.GetComponent<AnimationEvents>() != null) GameObject.Destroy(enemy.GetComponent<AnimationEvents>());
            if (enemy.GetComponent<NavMeshAgent>() != null) GameObject.Destroy(enemy.GetComponent<NavMeshAgent>());
            if (enemy.GetComponent<WaterSplashSize>() != null) GameObject.Destroy(enemy.GetComponent<WaterSplashSize>());
            if (enemy.GetComponent<TurretTrap>() != null) GameObject.Destroy(enemy.GetComponent<TurretTrap>());
            if (enemy.GetComponent<SmashableObject>() != null) GameObject.Destroy(enemy.GetComponent<SmashableObject>());
            if (enemy.GetComponent<BossEnemy>() != null) GameObject.Destroy(enemy.GetComponent<BossEnemy>());
            if (enemy.GetComponent<ColliderByParameter>() != null) GameObject.Destroy(enemy.GetComponent<ColliderByParameter>());
            foreach (TrackingBone trackingBone in enemy.GetComponentsInChildren<TrackingBone>()) {
                GameObject.Destroy(trackingBone);
            }
            foreach (BHMBone bhBone in enemy.GetComponentsInChildren<BHMBone>()) {
                GameObject.Destroy(bhBone);
            }
            foreach (HitQuiver hq in enemy.GetComponentsInChildren<HitQuiver>()) {
                GameObject.Destroy(hq);
            }
            foreach (HitTrigger ht in enemy.GetComponentsInChildren<HitTrigger>()) {
                GameObject.Destroy(ht);
            }
            foreach (StudioEventEmitter see in enemy.GetComponentsInChildren<StudioEventEmitter>()) {
                GameObject.Destroy(see);
            }
        }
    }
}
