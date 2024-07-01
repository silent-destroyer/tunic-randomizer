using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {
    public class SecretMayor {

        public static bool shouldBeActive = isCorrectDate();
        public static GameObject MrMayor;
        public static void SceneLoader_OnSceneLoaded_SecretMayorPatch(Scene loadingScene, LoadSceneMode mode, SceneLoader __instance) {
            if (!SceneLoaderPatches.InitialLoadDone) {
                return;
            }

            if (shouldBeActive) {
                Mesh mesh = MrMayor.GetComponent<MeshFilter>().mesh;
                Material[] materials = MrMayor.GetComponent<MeshRenderer>().materials;
                Material laurelsMat = ModelSwaps.FindMaterial("Shader Forge_laurels_nowiggle (Instance)") ?? ModelSwaps.FindMaterial("Shader Forge_laurels_nowiggle");
                laurelsMat = new Material(laurelsMat);
                laurelsMat.color = new Color(0.8577f, 0.5044f, 1.7513f, 1f);
                string scene = loadingScene.name;

                if (scene == "Shop" || scene == "ShopSpecial") {
                    GameObject merchant = Resources.FindObjectsOfTypeAll<SkinnedMeshRenderer>().Where(renderer => renderer.name == "merchant" && renderer.gameObject.scene == loadingScene).First().gameObject;
                    merchant.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                    merchant.SetActive(true);

                    GameObject.Find("Environment").transform.GetChild(3).gameObject.SetActive(false);
                    
                    GameObject mayor = GameObject.Instantiate(MrMayor);
                    if (scene == "Shop") {
                        mayor.transform.parent = GameObject.Find("merchant/merchant_armature/root/spine1/spine2/spine3/spine4/head/").transform;
                    } else if (scene == "ShopSpecial") {
                        mayor.transform.parent = GameObject.Find("merchant (1)/merchant_armature/root/spine1/spine2/spine3/spine4/head/").transform;
                    }
                    mayor.layer = 0;
                    mayor.transform.localPosition = new Vector3(0f, 0f, 1.4f);
                    mayor.transform.localEulerAngles = new Vector3(325, 0, 0);
                    mayor.transform.localScale = Vector3.one;
                    mayor.SetActive(true);
                }
                if (scene == "RelicVoid") {
                    GameObject.Find("_Environment (Center)/Cylinder (1)").SetActive(false);
                    GameObject.Find("_Environment (Center)/fox statue/fox statue (1)").SetActive(false);
                    GameObject.Find("_Environment (Center)/fox statue/laurels").SetActive(false);
                    GameObject.Find("_Environment (Center)/fox statue").GetComponent<MeshFilter>().mesh = mesh;
                    GameObject.Find("_Environment (Center)/fox statue").GetComponent<MeshRenderer>().materials = materials;
                }
                if (scene == "Archipelagos Redux") {
                    GameObject statue = GameObject.Find("_Environment Prefabs/RelicPlinth/relic plinth/fox statue");
                    ChangeHerosGrave(statue, mesh);
                }
                if (scene == "Swamp Redux 2") {
                    GameObject statue = GameObject.Find("_Setpieces Etc/RelicPlinth/relic plinth/fox statue");
                    ChangeHerosGrave(statue, mesh);

                    foreach (SpecialSwampRB skull in Resources.FindObjectsOfTypeAll<SpecialSwampRB>().Where(skull => skull.gameObject.scene.name == scene)) {
                        GameObject.Destroy(skull.GetComponent<MeshFilter>());
                        GameObject mayor = new GameObject("mayor");
                        mayor.transform.parent = skull.transform;
                        mayor.AddComponent<MeshFilter>().mesh = mesh;
                        mayor.AddComponent<MeshRenderer>().materials = materials;
                        mayor.transform.localPosition = new Vector3(0, 0, 0.7f);
                        mayor.transform.localEulerAngles = new Vector3(270, 0, 0);
                        mayor.transform.localScale = Vector3.one / 2;
                        mayor.SetActive(true);
                    }

                    foreach (MeshRenderer meshRenderer in GameObject.FindObjectsOfType<MeshRenderer>().Where(meshR => meshR.name.Contains("swamp_statue_broken") && !meshR.name.Contains("head"))) {
                        GameObject mayorStatue = GameObject.Instantiate(MrMayor, meshRenderer.transform);
                        mayorStatue.layer = 0;
                        mayorStatue.transform.localPosition = new Vector3(0f, 1.3782f, 0.3f);
                        mayorStatue.transform.localScale = Vector3.one * 0.8f;
                        mayorStatue.transform.localEulerAngles = Vector3.zero;
                        mayorStatue.GetComponent<MeshRenderer>().materials = new Material[] { meshRenderer.material, meshRenderer.material, meshRenderer.material };
                    }

                    for(int i = 9; i < 13; i++) {
                        GameObject pristineStatue = GameObject.Find($"_Environment/swamp_statue_pristine ({i})");
                        pristineStatue.GetComponent<MeshFilter>().mesh = mesh;
                        pristineStatue.GetComponent<MeshRenderer>().materials = new Material[] { pristineStatue.GetComponent<MeshRenderer>().material, pristineStatue.GetComponent<MeshRenderer>().material, pristineStatue.GetComponent<MeshRenderer>().material };

                    }
                }
                if (scene == "Sword Access") {
                    GameObject statue = GameObject.Find("_Setpieces/RelicPlinth (1)/relic plinth/fox statue");
                    ChangeHerosGrave(statue, mesh);
                }
                if (scene == "Library Hall") {
                    GameObject statue = GameObject.Find("_Special/RelicPlinth/relic plinth/fox statue");
                    ChangeHerosGrave(statue, mesh);
                }
                if (scene == "Monastery") {
                    GameObject statue = GameObject.Find("Root/RelicPlinth (1)/relic plinth/fox statue");
                    ChangeHerosGrave(statue, mesh);
                }
                if (scene == "Fortress Reliquary") {
                    GameObject statue = GameObject.Find("RelicPlinth/relic plinth/fox statue");
                    ChangeHerosGrave(statue, mesh);
                }
                if (scene == "Temple") {
                    GameObject statue1 = GameObject.Find("_ROOM: main/westgarden_niche_statue");
                    Material[] statueMats = new Material[] {
                        statue1.GetComponent<MeshRenderer>().material,
                        statue1.GetComponent<MeshRenderer>().material,
                        statue1.GetComponent<MeshRenderer>().material
                    };
                    statue1.GetComponent<MeshFilter>().mesh = mesh;
                    statue1.GetComponent<MeshRenderer>().materials = statueMats;
                    statue1.transform.localPosition = new Vector3(14f, 1.51f, 50.23f);
                    statue1.transform.localEulerAngles = new Vector3(0, 180, 0);
                    statue1.transform.localScale = Vector3.one * 1.5f;

                    GameObject statue2 = GameObject.Find("_ROOM: main/westgarden_niche_statue (1)");
                    statue2.GetComponent<MeshFilter>().mesh = mesh;
                    statue2.GetComponent<MeshRenderer>().materials = statueMats;
                    statue2.GetComponent<MeshRenderer>().receiveShadows = false;
                    statue2.transform.localPosition = new Vector3(16.2965f, 1.02f, 50.1822f);
                    statue2.transform.localEulerAngles = new Vector3(0, 200, 0);
                    statue2.transform.localScale = Vector3.one * 1.2f;

                    GameObject statue3 = GameObject.Find("_ROOM: main/westgarden_niche_statue (2)");
                    statue3.GetComponent<MeshFilter>().mesh = mesh;
                    statue3.GetComponent<MeshRenderer>().materials = statueMats;
                    statue3.transform.localPosition = new Vector3(11.7733f, 1.02f, 50.5023f);
                    statue3.transform.localEulerAngles = new Vector3(0, 160, 0);
                    statue3.transform.localScale = Vector3.one * 1.2f;

                    foreach (GameObject Questagon in Resources.FindObjectsOfTypeAll<GameObject>().Where(Obj => Obj.name == "questagon" && Obj.scene.name == loadingScene.name)) {
                        Questagon.GetComponent<MeshFilter>().mesh = mesh;
                        Questagon.GetComponent<MeshRenderer>().materials = new Material[] {
                            Questagon.GetComponent<MeshRenderer>().material,
                            Questagon.GetComponent<MeshRenderer>().material,
                            Questagon.GetComponent<MeshRenderer>().material
                        };

                        if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                            Questagon.GetComponent<MeshRenderer>().materials = materials;
                            Questagon.GetComponent<MeshRenderer>().receiveShadows = false;
                        }
                    }
                }

                if (scene == "Overworld Redux") {
                    GameObject foxStatue = GameObject.Find("_Environment/_Environment-Probuilder/elderfox (3)");
                    GameObject mayor = GameObject.Instantiate(MrMayor, foxStatue.transform);
                    mayor.layer = 0;
                    mayor.transform.localPosition = new Vector3(0f, 18.0478f, -0.694f);
                    mayor.transform.localEulerAngles = Vector3.zero;
                    mayor.transform.localScale = Vector3.one * 3.5f;
                    mayor.GetComponent<MeshRenderer>().materials = new Material[] { 
                        foxStatue.GetComponent<MeshRenderer>().material,
                        foxStatue.GetComponent<MeshRenderer>().material,
                        foxStatue.GetComponent<MeshRenderer>().material,
                    };
                }

                if (scene == "Spirit Arena") {
                    GameObject foxgod = Resources.FindObjectsOfTypeAll<Foxgod>().Where(god => god.gameObject.scene == loadingScene).First().gameObject;
                    foxgod.GetComponent<BossAnnounceOnAggro>().bossTitleTopLine.text = $"\"Mr Mayor\"";
                    foxgod.GetComponent<BossAnnounceOnAggro>().bossTitleBottomLine = ScriptableObject.CreateInstance<LanguageLine>();
                    foxgod.GetComponent<BossAnnounceOnAggro>().bossTitleBottomLine.text = $"tehlur uhv forJuhnz, rEkowntur uhv storEz, awfurur uhv snahks";
                    foxgod.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                    foxgod.transform.GetChild(1).gameObject.SetActive(false);
                    GameObject mayorgod = GameObject.Instantiate(MrMayor, foxgod.transform.GetChild(0));
                    mayorgod.layer = 0;
                    mayorgod.transform.localScale = Vector3.one*2;
                    mayorgod.transform.localPosition = new Vector3(0, 1, 0);
                    mayorgod.transform.localEulerAngles = Vector3.zero;
                    mayorgod.SetActive(true);
                }
                if (scene == "Playable Intro") {
                    GameObject.Find("++Cutscene++/foxgod_rigify/elderfox legs partial").SetActive(false);
                    GameObject foxgod = GameObject.Find("++Cutscene++/foxgod_rigify/elderfox/");
                    foxgod.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                    GameObject mayor = GameObject.Instantiate(MrMayor, foxgod.transform);
                    mayor.layer = 0;
                    mayor.transform.localScale = Vector3.one * 2;
                    mayor.transform.localPosition = Vector3.zero;
                    mayor.transform.localEulerAngles = Vector3.zero;
                }

                if(scene == "Resurrection") {
                    GameObject.Find("_CUTSCENE/Foxgod_Cutscenes/elderfox legs").SetActive(false);
                    GameObject.Find("_CUTSCENE/Foxgod_Cutscenes/elderfox legs partial").SetActive(false);
                    GameObject foxgod = GameObject.Find("_CUTSCENE/Foxgod_Cutscenes/elderfox");
                    foxgod.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                    GameObject mayor = GameObject.Instantiate(MrMayor, foxgod.transform);
                    mayor.layer = 0;
                    mayor.transform.localScale = Vector3.one * 2;
                    mayor.transform.localPosition = Vector3.zero;
                    mayor.transform.localEulerAngles = Vector3.zero;
                }
                if (scene == "Transit") {
                    GameObject.Find("Foxgod_Cutscenes/elderfox legs").SetActive(false);
                    GameObject.Find("Foxgod_Cutscenes/elderfox legs partial").SetActive(false);
                    GameObject foxgod = GameObject.Find("Foxgod_Cutscenes/elderfox");
                    foxgod.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                    GameObject mayor = GameObject.Instantiate(MrMayor, foxgod.transform);
                    mayor.layer = 0;
                    mayor.transform.localScale = Vector3.one * 2;
                    mayor.transform.localPosition = Vector3.zero;
                    mayor.transform.localEulerAngles = Vector3.zero;
                }

                if (scene == "ziggurat2020_2") {
                    GameObject voidtouched = GameObject.Find("spookytube/voidtouched in tube/voidtouched");
                    Material[] voidMats = new Material[] {
                        laurelsMat,
                        voidtouched.GetComponent<SkinnedMeshRenderer>().material,
                        voidtouched.GetComponent<SkinnedMeshRenderer>().material,
                    };
                    voidtouched.GetComponent<SkinnedMeshRenderer>().sharedMesh = mesh;
                    voidtouched.GetComponent<SkinnedMeshRenderer>().materials = voidMats;
                    voidtouched.transform.localScale= Vector3.one*1.5f;

                    GameObject voidtouched2 = GameObject.Find("FUSE_ASSEMBLY/movable root/voidtouched in tube/voidtouched");
                    voidtouched2.GetComponent<SkinnedMeshRenderer>().sharedMesh = mesh;
                    voidtouched2.GetComponent<SkinnedMeshRenderer>().materials = voidMats;
                    voidtouched2.transform.localScale = Vector3.one * 1.5f;
                    GameObject voidtouched3 = GameObject.Find("FUSE_ASSEMBLY (1)/movable root/voidtouched in tube/voidtouched");
                    voidtouched3.GetComponent<SkinnedMeshRenderer>().sharedMesh = mesh;
                    voidtouched3.GetComponent<SkinnedMeshRenderer>().materials = voidMats;
                    voidtouched3.transform.localScale = Vector3.one * 1.5f;

                    foreach(SkinnedMeshRenderer v in GameObject.FindObjectsOfType<SkinnedMeshRenderer>().Where(smr => smr.name == "voidtouched" && smr.transform.parent != null && smr.transform.parent.name == "voidtouched in tube")) {
                        v.GetComponent<SkinnedMeshRenderer>().sharedMesh = mesh;
                        v.GetComponent<SkinnedMeshRenderer>().materials = voidMats;
                        v.transform.localScale = Vector3.one * 1.5f;
                    }
                }
                if (scene == "Fortress Arena") {
                    GameObject statues = GameObject.Find("_Environment/Fox Statue Kneeling Hint/");
                    for(int i = 0; i < statues.transform.childCount; i++) {
                        statues.transform.GetChild(i).GetComponent<MeshFilter>().mesh = mesh;
                        statues.transform.GetChild(i).GetComponent<MeshRenderer>().materials = new Material[] {
                            statues.transform.GetChild(i).GetComponent<MeshRenderer>().material,
                            statues.transform.GetChild(i).GetComponent<MeshRenderer>().material,
                            statues.transform.GetChild(i).GetComponent<MeshRenderer>().material,
                        };
                        statues.transform.GetChild(i).localPosition = new Vector3(statues.transform.GetChild(i).localPosition.x, 1.4f, statues.transform.GetChild(i).localPosition.z);
                        statues.transform.GetChild(i).localScale = Vector3.one;
                    }
                }
                if (scene == "Mountaintop") {
                    GameObject bigmayor = GameObject.Instantiate(MrMayor);
                    bigmayor.layer = 0;
                    bigmayor.transform.localScale = Vector3.one*20f;
                    bigmayor.transform.position = new Vector3(34.742f, -52.599f, 108.8022f);
                    bigmayor.transform.localEulerAngles = new Vector3(0, 180, 0);
                    Material granite = ModelSwaps.FindMaterial("granite");
                    bigmayor.GetComponent<MeshRenderer>().materials = new Material[] { granite, granite, granite };
                    bigmayor.SetActive(true);
                }
                if (scene == "Cathedral Redux") {
                    GameObject voidtouched = GameObject.Find("_ROOM: Sacrifice Room/voidtouched group 1 (present)/Voidtouched/voidtouched");
                    if (voidtouched != null) {
                        voidtouched.GetComponent<SkinnedMeshRenderer>().sharedMesh = mesh;
                        voidtouched.GetComponent<CreatureMaterialManager>().originalMaterials = new Material[] {
                            laurelsMat,
                            voidtouched.GetComponent<SkinnedMeshRenderer>().material,
                            voidtouched.GetComponent<SkinnedMeshRenderer>().material,
                        };
                        voidtouched.transform.localScale = Vector3.one * 1.5f;
                        voidtouched.transform.localPosition = new Vector3(0, 1.7f, 0);
                    }
                }
                foreach(Campfire campfire in Resources.FindObjectsOfTypeAll<Campfire>().Where(camp => camp.gameObject.scene.name == scene && camp.transform.childCount >= 6 && !camp.gameObject.scene.name.Contains("ziggurat"))) {
                    GameObject statue = campfire.transform.GetChild(6).GetChild(1).gameObject;
                    GameObject mayor = GameObject.Instantiate(MrMayor, statue.transform.position, statue.transform.rotation);
                    mayor.SetActive(true);
                    mayor.layer = statue.layer;
                    mayor.GetComponent<MeshRenderer>().materials = new Material[] {
                        statue.GetComponent<MeshRenderer>().material,
                        statue.GetComponent<MeshRenderer>().material,
                        statue.GetComponent<MeshRenderer>().material,
                    };
                    mayor.transform.parent = statue.transform.parent;
                    mayor.transform.localScale = Vector3.one * 1.5f;
                    if (scene == "Cathedral Redux") {
                        mayor.transform.localPosition = new Vector3(0f, 0.7f, 1.3f);
                    }
                    statue.SetActive(false);
                    campfire.transform.GetChild(6).GetChild(2).GetChild(0).gameObject.SetActive(false);
                }
            }
        }

        private static void ChangeHerosGrave(GameObject statue, Mesh mesh) {
            statue.GetComponent<MeshFilter>().mesh = mesh;
            statue.GetComponent<MeshRenderer>().materials = new Material[] {
                        statue.GetComponent<MeshRenderer>().material,
                        statue.GetComponent<MeshRenderer>().material,
                        statue.GetComponent<MeshRenderer>().material,
                    };
            statue.transform.localPosition = new Vector3(0, 2.2f, 0);
            statue.transform.localScale = Vector3.one * 1.5f;
        }

        public static void MayorSword(GameObject sword) {
            sword.transform.localScale = new Vector3(0.5f, 0.6f, 0.5f);
            sword.transform.localEulerAngles = Vector3.zero;
        }

        public static void ToggleMayorSecret(int index) {
            shouldBeActive = !shouldBeActive;
            if (TitleVersion.Logo != null) {
                TitleVersion.Logo.GetComponent<Image>().sprite = shouldBeActive ? ModelSwaps.FindSprite("Randomizer secret_mayor") : ModelSwaps.TuncTitleImage.GetComponent<Image>().sprite;
            }
        }

        public static bool isCorrectDate() {
            return DateTime.Now.Day == 1 && DateTime.Now.Month == 4;
        }
    }
}
