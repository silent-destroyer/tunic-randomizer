using System;
using System.Collections.Generic;
using System.Linq;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using PigeonCoopToolkit.Effects.Trails;
using UnityEngine;
using UnityEngine.UI;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {
    public class EnemyModelSwaps {

        public static bool testAP = false;
        public static ItemFlags testAPFlags = ItemFlags.None;
        public static Dictionary<string, Texture2D> EnemyTextures = new Dictionary<string, Texture2D>();
        public static void refresh() {
            foreach (EnemyCheck ec in Resources.FindObjectsOfTypeAll<EnemyCheck>()) {
                ec.Awake();
            }
        }

        public static void CreateTextures(Material Material) {
            foreach (var pair in EnemyImageData.EnemyTextureData) {
                EnemyTextures[pair.Key] = ModelSwaps.CreateSprite(pair.Value.Item2, Material, pair.Value.Item1, pair.Value.Item1, pair.Key, TextureFormat.RGB24).GetComponent<Image>().mainTexture.Cast<Texture2D>();
            }
        }

        public static void SetupEnemyTexture(EnemyCheck enemy) {
            if (EnemyDropShuffle.IsValidEnemy(enemy.gameObject)) {
                string checkId = EnemyDropShuffle.GetEnemyCheckId(enemy.gameObject);
                if (EnemyDropShuffle.AllEnemyDropChecks[checkId].IsCompletedOrCollected) {
                    return;
                }
                Material material = null;
                UnityEngine.Color color = UnityEngine.Color.clear;
                bool isProgUseful = false;
                string colorString = null;
                if (Locations.RandomizedLocations.ContainsKey(checkId) || ItemLookup.ItemList.ContainsKey(checkId)) {
                    ItemData Item = ItemLookup.Items["Money x1"];
                    if (IsSinglePlayer()) {
                        Check check = Locations.RandomizedLocations[checkId];
                        Item = ItemLookup.GetItemDataFromCheck(check);
                        material = ModelSwaps.GetMaterialType(Item);
                    } else if (IsArchipelago()) {
                        ItemInfo itemInfo = ItemLookup.ItemList[checkId];
                        if (!Archipelago.instance.IsTunicPlayer(itemInfo.Player) || !ItemLookup.Items.ContainsKey(itemInfo.ItemDisplayName)) {
                            color = GetAPColor(itemInfo.Flags);
                            colorString = GetAPColorString(itemInfo.Flags);
                            isProgUseful = itemInfo.Flags.HasFlag(ItemFlags.Advancement) && itemInfo.Flags.HasFlag(ItemFlags.NeverExclude);
                        } else {
                            Item = ItemLookup.Items[itemInfo.ItemDisplayName];
                            material = ModelSwaps.GetMaterialType(Item);
                        }
                    }
                }
                if (testAP) {
                    material = null;
                    color = GetAPColor(testAPFlags);
                    colorString = GetAPColorString(testAPFlags);
                    isProgUseful = testAPFlags.HasFlag(ItemFlags.Advancement) && testAPFlags.HasFlag(ItemFlags.NeverExclude);
                }

                if (enemy.GetComponent<Blob>() != null) {
                    ApplyBlobTexture(enemy.gameObject, material, colorString);
                } else if (enemy.GetComponent<Skuladin>() != null) {
                    ApplyRudelingTexture(enemy.gameObject, material, colorString);
                } else if (enemy.GetComponent<Hedgehog>() != null) {
                    ApplyHedgehogTexture(enemy.gameObject, material, color, isProgUseful);
                } else if (enemy.GetComponent<HonourGuard>() != null) {
                    ApplyEnvoyTexture(enemy.gameObject, material, colorString);
                } else if (enemy.GetComponent<Beefboy>() != null) {
                    ApplyBeefboyTexture(enemy.gameObject, material, colorString);
                } else if (enemy.GetComponent<Plover>() != null) {
                    ApplyPloverTexture(enemy.gameObject, material, colorString);
                } else if (enemy.GetComponent<Crabbo>() != null) {
                    ApplyCrabTexture(enemy.gameObject, material, colorString);
                } else if (enemy.GetComponent<Crow>() != null) {
                    ApplyHusherTexture(enemy.gameObject, material, color);
                } else if (enemy.GetComponent<Frog>() != null) {
                    ApplyFrogTexture(enemy.gameObject, material, colorString);
                } else if (enemy.GetComponent<Bat>() != null) {
                    ApplyBatTexture(enemy.gameObject, material, color);
                } else if (enemy.GetComponent<Bumblebones>() != null || enemy.GetComponent<Fencer>() != null) {
                    ApplyFleemerTexture(enemy.gameObject, material, color, isProgUseful);
                } else if (enemy.GetComponent<Gunman>() != null) {
                    ApplyGunslingerTexture(enemy.gameObject, material, color, isProgUseful);
                } else if (enemy.GetComponent<Gost>() != null) {
                    ApplyLostEchoTexture(enemy.gameObject, material, colorString);
                } else if (enemy.GetComponent<SewerTentacle>() != null) {
                    ApplyTentacleTexture(enemy.gameObject, material, color);
                } else if (enemy.GetComponent<Probe>() != null) {
                    ApplyFairyTexture(enemy.gameObject, material, color);
                } else if (enemy.GetComponent<Crocodoo>() != null) {
                    ApplyChompignomTexture(enemy.gameObject, material, colorString, color);
                } else if (enemy.GetComponent<DefenseTurret>() != null) {
                    ApplyTurretTexture(enemy.gameObject, material, color);
                } else if (enemy.GetComponent<Spinnerbot>() != null) {
                    ApplySlormTexture(enemy.gameObject, material, colorString);
                } else if (enemy.GetComponent<Wizard>() != null || enemy.GetComponent<Wizard_Candleabra>() != null || enemy.GetComponent<Wizard_Sword>() != null) {
                    ApplyCustodianTexture(enemy.gameObject, material, colorString);
                } else if (enemy.GetComponent<Spider>() != null) {
                    ApplySpiderTexture(enemy.gameObject, material, colorString);
                } else if (enemy.GetComponent<Scavenger>() != null || enemy.GetComponent<Scavenger_Miner>() != null || enemy.GetComponent<Scavenger_Support>() != null) {
                    ApplyScavengerTexture(enemy.gameObject, material, colorString);
                } else if (enemy.GetComponent<FoxEnemy>() != null || enemy.GetComponent<FoxEnemyZombie>() != null) {
                    ApplyFoxEnemyTexture(enemy.gameObject, material, colorString);
                } else if (enemy.GetComponent<Voidling>() != null || enemy.GetComponent<Voidtouched>() != null) {
                    ApplyVoidlingTexture(enemy.gameObject, material, color);
                } else if (enemy.GetComponent<Administrator>() != null || enemy.GetComponent<Administrator_angry>() != null) {
                    ApplyAdministratorTexture(enemy.gameObject, material, color);
                } else if (enemy.GetComponent<ScavengerBoss>() != null) {
                    ApplyBossScavengerTexture(enemy.gameObject, material, color);
                } else if (enemy.GetComponent<Knightbot>() != null) {
                    ApplyGardenKnightTexture(enemy.gameObject, material, color);
                } else if (enemy.GetComponent<Librarian>() != null) {
                    ApplyLibrarianTexture(enemy.gameObject, material, colorString);
                } else if (enemy.GetComponent<Spidertank>() != null) {
                    ApplySiegeEngineTexture(enemy.gameObject, material, color);
                }
            }
        }

        private static UnityEngine.Color GetAPColor(ItemFlags itemFlags) {
            int randomFlag = new System.Random().Next(3);
            ItemFlags flag = itemFlags;
            UnityEngine.Color color = UnityEngine.Color.clear;
            if (flag == ItemFlags.None || (flag == ItemFlags.Trap && randomFlag == 0)) {
                color = new UnityEngine.Color(0f, 0.75f, 0f, 1f);
            }
            if (flag.HasFlag(ItemFlags.NeverExclude) || (flag == ItemFlags.Trap && randomFlag == 1)) {
                color = new UnityEngine.Color(0f, 0.5f, 0.75f, 1f);
            }
            if (flag.HasFlag(ItemFlags.Advancement) || (flag == ItemFlags.Trap && randomFlag == 2)) {
                color = PaletteEditor.Gold;
            }
            return color;
        }

        private static string GetAPColorString(ItemFlags itemFlags) {
            int randomFlag = new System.Random().Next(3);
            ItemFlags flag = itemFlags;
            string color = null;
            if (flag == ItemFlags.None || (flag == ItemFlags.Trap && randomFlag == 0)) {
                color = "Green";
            }
            if (flag.HasFlag(ItemFlags.NeverExclude) || (flag == ItemFlags.Trap && randomFlag == 1)) {
                color = "Blue";
            }
            if (flag.HasFlag(ItemFlags.Advancement) || (flag == ItemFlags.Trap && randomFlag == 2)) {
                color = "Gold";
            }
            if (flag.HasFlag(ItemFlags.Advancement) && flag.HasFlag(ItemFlags.NeverExclude)) {
                color = "GoldCyan";
            }
            return color;
        }

        private static void ApplyBlobTexture(GameObject blob, Material material, String color) {
            CreatureMaterialManager creatureMaterialManager = blob.GetComponentInChildren<CreatureMaterialManager>();
            if (material != null) {
                creatureMaterialManager.originalMaterials = new Material[] { material };
            } else if (color != null) {
                if (creatureMaterialManager.originalMaterials[0].name.Contains("harder")) {
                    creatureMaterialManager.originalMaterials[0].mainTexture = EnemyTextures[$"BlobBig{color}"];
                } else {
                    creatureMaterialManager.originalMaterials[0].mainTexture = EnemyTextures[$"BlobSmall{color}"];
                }
            }
        }

        private static void ApplyRudelingTexture(GameObject rudeling, Material material, string color) {
            if (material != null) {
                rudeling.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material = material;
                if (rudeling.transform.GetChild(2).GetComponent<CreatureMaterialManager>() != null) {
                    rudeling.transform.GetChild(2).GetComponent<CreatureMaterialManager>().originalMaterials = new Material[] { material };
                }
            } else if (color != null) {
                rudeling.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials[0].mainTexture = EnemyTextures[$"Rudeling{color}"];
            }
        }

        private static void ApplyHedgehogTexture(GameObject hedgehog, Material material, UnityEngine.Color color, bool isProgUseful = false) {
            if (material != null) {
                hedgehog.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials[0] = material;
            } else if (!color.Equals(UnityEngine.Color.clear)) {
                hedgehog.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials[0].color = color;
                if (isProgUseful) {
                    hedgehog.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials[1].color = UnityEngine.Color.cyan;
                }
            }
        }

        private static void ApplyEnvoyTexture(GameObject envoy, Material material, string color) {
            if (material != null) {
                foreach (MeshRenderer renderer in envoy.transform.GetChild(0).GetComponentsInChildren<MeshRenderer>()) {
                    if (renderer.name.Contains("shield") || renderer.name.Contains("spear")) {
                        renderer.material = material;
                    }
                }
            } else if (color != null) {
                envoy.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].mainTexture = EnemyTextures[$"Envoy{color}"];
            }
        }

        public static bool HonourGuard_dropShield_PrefixPatch(HonourGuard __instance) {
            if (SaveFlags.GetBool(ShuffleEnemyDropsEnabled) && __instance.GetComponent<EnemyCheck>() != null) {
                __instance.shieldPrefab.GetComponent<MeshRenderer>().materials = __instance.shieldRoot.GetComponent<MeshRenderer>().materials;
            } else {
                __instance.shieldPrefab.GetComponent<MeshRenderer>().materials = EnemyRandomizer.Enemies["Honourguard"].GetComponent<HonourGuard>().shieldRoot.GetComponent<MeshRenderer>().materials;
            }
            return true;
        }
        private static void HonourGuard_dropShield_PostfixPatch(HonourGuard __instance) {
            if (SaveFlags.GetBool(ShuffleEnemyDropsEnabled) && __instance.GetComponent<EnemyCheck>() != null) {
                __instance.shieldPrefab.GetComponent<MeshRenderer>().materials = EnemyRandomizer.Enemies["Honourguard"].GetComponent<HonourGuard>().shieldRoot.GetComponent<MeshRenderer>().materials;
            }
        }

        private static void ApplyBeefboyTexture(GameObject beefboy, Material material, string color) {
            if (material != null) {
                foreach (MeshRenderer renderer in beefboy.transform.GetChild(2).GetComponentsInChildren<MeshRenderer>(true)) {
                    if (renderer.name.Contains("shield") || renderer.name.Contains("sword")) {
                        renderer.material = material;
                    }
                }
            } else if (color != null) {
                Texture2D texture = EnemyTextures[$"Beefboy{color}"];
                beefboy.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].mainTexture = texture;
                foreach (MeshRenderer renderer in beefboy.transform.GetChild(2).GetComponentsInChildren<MeshRenderer>(true)) {
                    if (renderer.name.Contains("beefboy_debris")) {
                        renderer.material.mainTexture = texture;
                    }
                }
            }
        }

        private static void ApplyPloverTexture(GameObject plover, Material material, string color) {
            if (material != null) {
                plover.GetComponentInChildren<CreatureMaterialManager>().originalMaterials = new Material[] { material };
            } else if (color != null) {
                plover.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].mainTexture = EnemyTextures[$"Plover{color}"];
            }
        }

        private static void ApplyCrabTexture(GameObject crab, Material material, string color) {
            if (material != null) {
                crab.GetComponentInChildren<CreatureMaterialManager>().originalMaterials = new Material[] { material };
            } else if (color != null) {
                Material crabMaterial = crab.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0];
                crabMaterial.mainTexture = EnemyTextures[$"Crabbo{color}"];
                List<string> keywords = crabMaterial.shaderKeywords.ToList();
                keywords.Remove("_SPECGLOSSMAP");
                crabMaterial.shaderKeywords = keywords.ToArray();
            }
        }

        private static void ApplyHusherTexture(GameObject husher, Material material, UnityEngine.Color color) {
            if (material != null) {
                husher.GetComponentInChildren<SkinnedMeshRenderer>().material = material;
                if (husher.GetComponentInChildren<CreatureMaterialManager>() != null) {
                    husher.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0] = material;
                } else {
                    husher.GetComponentInChildren<SkinnedMeshRenderer>().material = material;
                }
            } else if (!color.Equals(UnityEngine.Color.clear)) {
                if (husher.GetComponentInChildren<CreatureMaterialManager>() != null) {
                    husher.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].color = color;
                }
            }
        }

        private static void ApplyFrogTexture(GameObject frog, Material material, string color) {
            if (material != null) {
                frog.GetComponentInChildren<CreatureMaterialManager>().originalMaterials = new Material[] { material };
                foreach (MeshRenderer mr in frog.GetComponentsInChildren<MeshRenderer>()) {
                    mr.material = material;
                }
            } else if (color != null) {
                Material crabMaterial = frog.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0];
                if (frog.name.ToLower().Contains("small")) {
                    crabMaterial.mainTexture = EnemyTextures[$"FrogSmall{color}"];
                } else {
                    crabMaterial.mainTexture = EnemyTextures[$"Frog{color}"];
                }
            }
        }

        private static void ApplyFleemerTexture(GameObject gameObject, Material material, UnityEngine.Color color, bool isProgUseful = false) {
            if (material != null) {
                foreach (MeshRenderer mr in gameObject.GetComponentsInChildren<MeshRenderer>()) {
                    if (mr.name.ToLower().Contains("skull")) continue;
                    mr.material = material;
                    if (mr.name == "hammer") {
                        mr.GetComponent<CreatureMaterialManager>().originalMaterials[0] = material;
                    }
                }
            } else if (!color.Equals(UnityEngine.Color.clear)) {
                foreach (CreatureMaterialManager manager in gameObject.GetComponentsInChildren<CreatureMaterialManager>()) {
                    if (manager.name == "hammer") continue;
                    manager.originalMaterials[0].color = color;
                }
                if (isProgUseful) {
                    foreach (MeshRenderer mr in gameObject.GetComponentsInChildren<MeshRenderer>()) {
                        if (mr.name == "hammer") continue;
                        mr.material.color = UnityEngine.Color.cyan;
                    }
                }
            }
        }

        private static void ApplyBatTexture(GameObject bat, Material material, UnityEngine.Color color) {
            CreatureMaterialManager creatureMaterialManager = bat.GetComponentInChildren<CreatureMaterialManager>();
            if (material != null) {
                creatureMaterialManager.originalMaterials = new Material[] { material };
            } else if (!color.Equals(UnityEngine.Color.clear)) {
                creatureMaterialManager.originalMaterials[0].color = color;
            }
        }

        private static void ApplyGunslingerTexture(GameObject gunslinger, Material material, UnityEngine.Color color, bool isProgUseful) {
            if (material != null) {
                foreach (MeshRenderer mr in gunslinger.GetComponentsInChildren<MeshRenderer>()) {
                    if (mr.name == "gun - model") {
                        mr.material = material;
                    }
                }
            } else if (!color.Equals(UnityEngine.Color.clear)) {
                gunslinger.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].color = color;
                if (isProgUseful) {
                    foreach (MeshRenderer mr in gunslinger.GetComponentsInChildren<MeshRenderer>()) {
                        if (mr.name == "gun - model") {
                            mr.material.color = UnityEngine.Color.cyan;
                        }
                    }
                }
            }
        }

        private static void ApplyLostEchoTexture(GameObject ghost, Material material, string color) {
            if (material != null) {
                foreach (SmokeTrail trail in ghost.GetComponentsInChildren<SmokeTrail>()) {
                    trail.TrailData.TrailMaterial = material;
                }
            } else if (color != null) {
                if (color == "Green") {
                    ghost.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].color = new UnityEngine.Color(0, 0.3f, 0f);
                } else if (color == "Blue") {
                    ghost.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].color = new UnityEngine.Color(0, 0.2f, 0.3f);
                } else if (color.Contains("Gold")) {
                    ghost.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].color = new UnityEngine.Color(0.2f, 0.2f, 0);
                }
            }
        }

        private static void ApplyTentacleTexture(GameObject tentacle, Material material, UnityEngine.Color color) {
            if (material != null) {
                tentacle.GetComponentInChildren<CreatureMaterialManager>().originalMaterials = new Material[] { material };
            } else if (!color.Equals(UnityEngine.Color.clear)) {
                tentacle.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].color = color;
            }
        }
        private static void ApplyFairyTexture(GameObject fairy, Material material, UnityEngine.Color color) {
            if (material != null) {
                if (material.name.ToLower().Contains("glow")) {
                    Material m = new Material(ModelSwaps.Items["Hexagon Blue"].GetComponent<MeshRenderer>().material);
                    m.color = new UnityEngine.Color(0.2492f, 1f, 1f, 1f);
                    fairy.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0] = m;
                } else {
                    fairy.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0] = material;
                }
            } else if (!color.Equals(UnityEngine.Color.clear)) {
                fairy.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].color = color;
            }
        }
        private static void ApplyChompignomTexture(GameObject terry, Material material, string colorString, UnityEngine.Color color) {
            if (material != null) {
                GameObject glasses = setupTerryGlasses(terry);
                if (glasses != null) {
                    glasses.GetComponent<MeshRenderer>().materials = new Material[] { material, glasses.GetComponent<MeshRenderer>().materials[1] };
                }
            } else if (colorString != null && !color.Equals(UnityEngine.Color.clear)) {
                if (terry.name.ToLower().Contains("voidtouched")) {
                    GameObject glasses = setupTerryGlasses(terry);
                    if (glasses != null) {
                        glasses.GetComponent<MeshRenderer>().materials[0].mainTexture = Texture2D.whiteTexture;
                        glasses.GetComponent<MeshRenderer>().materials[0].color = colorString == "GoldCyan" ? UnityEngine.Color.cyan : color;
                    }
                } else {
                    if (colorString == "GoldCyan") {
                        colorString = "Gold";
                        GameObject glasses = setupTerryGlasses(terry);
                        if (glasses != null) {
                            glasses.GetComponent<MeshRenderer>().materials[0].mainTexture = Texture2D.whiteTexture;
                            glasses.GetComponent<MeshRenderer>().materials[0].color = UnityEngine.Color.cyan;
                        }
                    }
                    terry.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].mainTexture = EnemyTextures[$"Terry{colorString}"];
                }
            }
        }

        private static GameObject setupTerryGlasses(GameObject terry) {
            GameObject glasses = EnemyRandomizer.Enemies["crocodoo"].transform.Find("Armature/root/body/terryterryterry").gameObject;
            if (glasses != null) {
                if (terry.transform.Find("Armature/root/body/terryterryterry") != null) {
                    GameObject.Destroy(terry.transform.Find("Armature/root/body/terryterryterry").gameObject);
                }
                GameObject newGlasses = new GameObject("terryterryterry");
                newGlasses.AddComponent<MeshFilter>().mesh = glasses.GetComponent<MeshFilter>().mesh;
                newGlasses.AddComponent<MeshRenderer>().materials = glasses.GetComponent<MeshRenderer>().materials;
                newGlasses.transform.parent = terry.transform.Find("Armature/root/body");
                newGlasses.transform.localScale = Vector3.one * 5.1603f;
                newGlasses.transform.localEulerAngles = new Vector3(340.544f, 0, 4.388f);
                newGlasses.transform.localPosition = new Vector3(-0.05f, 1.27f, 2.05f);
                newGlasses.layer = 8;
                newGlasses.SetActive(true);
                return newGlasses;
            }

            return null;
        }

        private static void ApplyTurretTexture(GameObject turret, Material material, UnityEngine.Color color) {
            if (material != null) {
                turret.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[3] = material;
            } else if (!color.Equals(UnityEngine.Color.clear)) {
                turret.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[3].color = color;
            }
        }

        private static void ApplySlormTexture(GameObject slorm, Material material, string color) {
            if (material != null) {
                if (material.name.ToLower().Contains("glow")) {
                    Material m = new Material(ModelSwaps.Items["Hexagon Blue"].GetComponent<MeshRenderer>().material);
                    m.color = new UnityEngine.Color(0.2492f, 1f, 1f, 1f);
                    slorm.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0] = m;
                } else {
                    slorm.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0] = material;
                }
            } else if (color != null) {
                slorm.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].mainTexture = EnemyTextures[$"Slorm{color}"];
            }
        }

        private static void ApplyCustodianTexture(GameObject custodian, Material material, string color) {
            if (material != null) {
                foreach (MeshRenderer renderer in custodian.GetComponentsInChildren<MeshRenderer>()) {
                    if (renderer.name == "sword" || renderer.name == "staff" || renderer.name == "candelabra") {
                        renderer.material = material;
                    }
                }
            } else if (color != null) {
                custodian.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].mainTexture = EnemyTextures[$"Custodian{color}"];
            }
        }

        private static void ApplySpiderTexture(GameObject spider, Material material, string color) {
            if (material != null) {
                spider.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0] = material;
            } else if (color != null) {
                spider.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].color = UnityEngine.Color.white;
                if (color.Contains("Gold")) {
                    spider.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].mainTexture = ModelSwaps.FindTexture("spider_redux", true);
                    spider.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].SetTexture("_EmissionMap", ModelSwaps.FindTexture("spider_redux_emit", true));
                    spider.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].color = new UnityEngine.Color(0.917f, 0.189f, 0.08f);
                } else {
                    spider.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].mainTexture = ModelSwaps.FindTexture("spider_redux_big", true);
                    spider.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].SetTexture("_EmissionMap", ModelSwaps.FindTexture("spider_redux_big_emit", true));
                    if (color == "Green") {
                        spider.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].color = UnityEngine.Color.green;
                    }
                    if (color == "Blue") {
                        spider.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].color = new UnityEngine.Color(0, 0.5f, 0.75f);
                    }
                }
            }
        }

        private static void ApplyScavengerTexture(GameObject scavenger, Material material, string color) {
            if (material != null) {
                if (scavenger.GetComponent<Scavenger_Support>() != null) {
                    scavenger.GetComponent<EnemyCheck>().ScavengerBombMaterial = material;
                    return;
                }
                foreach (MeshRenderer renderer in scavenger.GetComponentsInChildren<MeshRenderer>()) {
                    if (renderer.name == "Rifle" || renderer.name == "sword mesh") {
                        renderer.material = material;
                    }
                }
            } else if (color != null) {
                scavenger.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].mainTexture = EnemyTextures[$"Scavenger{color}"];
            }
        }

        public static bool Scavenger_Support__toss__PrefixPatch(Scavenger_Support __instance, out Material[] __state) {
            __state = new Material[] { };
            if (__instance.bombPrefab != null) {
                __state = __instance.bombPrefab.GetComponentInChildren<MeshRenderer>().materials;
            }
            if (SaveFlags.GetBool(ShuffleEnemyDropsEnabled) && TunicRandomizer.Settings.ChestsMatchContentsEnabled && __instance.GetComponent<EnemyCheck>() != null && __instance.bombPrefab != null) {
                if (__instance.GetComponent<EnemyCheck>().ScavengerBombMaterial != null) {
                    __instance.bombPrefab.GetComponentInChildren<MeshRenderer>().materials = new Material[] { __instance.GetComponent<EnemyCheck>().ScavengerBombMaterial };
                }
            } else {
            }
            return true;
        }

        private static void Scavenger_Support__toss__PostfixPatch(Scavenger_Support __instance, Material[] __state) {
            if (__state != null && __state.Length > 0 && __instance.bombPrefab != null) {
                __instance.bombPrefab.GetComponentInChildren<MeshRenderer>().materials = __state;
            }
        }

        private static void ApplyFoxEnemyTexture(GameObject enemy, Material material, string color) {
            if (material != null) {
                foreach (MeshRenderer renderer in enemy.GetComponentsInChildren<MeshRenderer>()) {
                    renderer.material = material;
                }
            } else if (color != null) {
                foreach (CreatureMaterialManager manager in enemy.GetComponentsInChildren<CreatureMaterialManager>()) {
                    manager.originalMaterials[0].mainTexture = EnemyTextures[$"Fox{color}"];
                }
            }
        }

        private static void ApplyVoidlingTexture(GameObject voidling, Material material, UnityEngine.Color color) {
            if (material != null) {
                voidling.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0] = material;
            } else if (!color.Equals(UnityEngine.Color.clear)) {
                voidling.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].shader = ModelSwaps.FindShader("Shader Forge/laurels");
                voidling.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].color = color;
            }
        }

        private static void ApplyAdministratorTexture(GameObject admin, Material material, UnityEngine.Color color) {
            if (material != null) {
                admin.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0] = material;
            } else if (!color.Equals(UnityEngine.Color.clear)) {
                admin.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].color = color;
            }
        }

        private static void ApplyBossScavengerTexture(GameObject boss, Material material, UnityEngine.Color color) {
            if (material != null) {
                foreach (MeshRenderer renderer in boss.GetComponentsInChildren<MeshRenderer>()) {
                    if (renderer.name == "shieldsword") {
                        renderer.materials = new Material[] { material, material, renderer.materials[2] };
                    }
                    if (renderer.name == "gun model") {
                        renderer.material = material;
                    }
                }
            } else if (!color.Equals(UnityEngine.Color.clear)) {
                foreach (Light light in boss.GetComponentsInChildren<Light>()) {
                    light.color = color;
                }
            }
        }

        private static void ApplyGardenKnightTexture(GameObject gardenKnight, Material material, UnityEngine.Color color) {
            foreach (MeshRenderer renderer in gardenKnight.GetComponentsInChildren<MeshRenderer>()) {
                if (renderer.name == "techknight sword") {
                    if (material != null) {
                        renderer.material = material;
                    } else if (!color.Equals(UnityEngine.Color.clear)) {
                        renderer.material.color = color;
                    }
                }
            }
        }

        private static void ApplyLibrarianTexture(GameObject librarian, Material material, string color) {
            foreach (MeshRenderer renderer in librarian.GetComponentsInChildren<MeshRenderer>()) {
                if (renderer.name == "Sword") {
                    if (material != null) {
                        renderer.material = material;
                    } else if (color != null) {
                        renderer.material.SetTexture("_EmissionMap", EnemyTextures[$"Librarian{color}"]);                
                    }
                }
            }
        }

        private static void ApplySiegeEngineTexture(GameObject siegeEngine, Material material, UnityEngine.Color color) {
            GameObject glow = siegeEngine.transform.Find("Spidertank glow").gameObject;
            if (glow != null) {
                if (material != null) {
                    if (material.name.ToLower().Contains("glow")) {
                        Material m = new Material(ModelSwaps.Items["Hexagon Blue"].GetComponent<MeshRenderer>().material);
                        m.color = new UnityEngine.Color(0.2492f, 1f, 1f, 1f);
                        glow.GetComponent<SkinnedMeshRenderer>().material = m;
                    } else {
                        glow.GetComponent<SkinnedMeshRenderer>().material = material;
                    }
                } else if (!color.Equals(UnityEngine.Color.clear)) {
                    glow.GetComponent <SkinnedMeshRenderer>().material.color = color;
                }
            }
        }
    }
}
