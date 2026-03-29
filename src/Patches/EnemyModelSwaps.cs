using System;
using System.Collections.Generic;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using UnityEngine;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {
    public class EnemyModelSwaps {
        // represents various enemy textures, hardcoded as hex values instead of colors or anything else because im lazy
        public static List<string> rudelingTexture = new List<string>() {
            "#4A464A", "#000000", "#804B0D", "#FFE348",
            "#000000", "#CA3434", "#CA3434", "#CA3434",
            "#A09FA0", "#C6C6C6", "#C6C6C6", "#804B0D",
            "#000000", "#000000", "#000000", "#000000",
        };
        public static List<string> envoyTexture = new List<string>() {
            "#81959E", "#FF0000", "#3BAF3B", "#3BAF3B",
            "#8F9CA5", "#DFB200", "#747474", "#747474",
            "#000000", "#133506", "#000000", "#000000",
            "#000000", "#000000", "#000000", "#000000",
        };
        public static List<string> beefboyTexture = new List<string>() {
            "#967D65", "#C14C4C", "#FFFFFF", "#6E636D",
            "#9B9FA4", "#C14C4C", "#6E636D", "#6E636D",
            "#9B9FA4", "#C14C4C", "#6E636D", "#6E636D",
            "#9B9FA4", "#C14C4C", "#000000", "#5F5F5F",
        };
        public static List<string> ploverTexture = new List<string>() {
            "#000000", "#000000", "#FDC02F", "#FDC02F",
            "#000000", "#000000", "#FDC02F", "#FDC02F",
            "#72BEBB", "#72BEBB", "#FDC02F", "#FDC02F",
            "#72BEBB", "#72BEBB", "#FDC02F", "#FDC02F",
        };
        public static bool testAP = false;
        public static ItemFlags testAPFlags = ItemFlags.None;
        public static void refresh() {
            foreach (EnemyCheck ec in Resources.FindObjectsOfTypeAll<EnemyCheck>()) {
                ec.Awake();
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
                    isProgUseful = testAPFlags.HasFlag(ItemFlags.Advancement) && testAPFlags.HasFlag(ItemFlags.NeverExclude);
                }

                if (enemy.GetComponent<Blob>() != null) {
                    ApplyBlobTexture(enemy.gameObject, material, color, isProgUseful);
                } else if (enemy.GetComponent<Skuladin>() != null) {
                    ApplyRudelingTexture(enemy.gameObject, material, color, isProgUseful);
                } else if (enemy.GetComponent<Hedgehog>() != null) {
                    ApplyHedgehogTexture(enemy.gameObject, material, color, isProgUseful);
                } else if (enemy.GetComponent<HonourGuard>() != null) {
                    ApplyEnvoyTexture(enemy.gameObject, material, color, isProgUseful);
                } else if (enemy.GetComponent<Beefboy>() != null) {
                    ApplyBeefboyTexture(enemy.gameObject, material, color, isProgUseful);
                } else if (enemy.GetComponent<Plover>() != null) {
                    ApplyPloverTexture(enemy.gameObject, material, color, isProgUseful);
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

        public static void ApplyBlobTexture(GameObject blob, Material material, UnityEngine.Color color, bool isProgUseful) {

        }

        public static void ApplyRudelingTexture(GameObject rudeling, Material material, UnityEngine.Color color, bool isProgUseful = false) {
            if (material != null) {
                rudeling.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material = material;
                if (rudeling.transform.GetChild(2).GetComponent<CreatureMaterialManager>() != null) {
                    rudeling.transform.GetChild(2).GetComponent<CreatureMaterialManager>().originalMaterials = new Material[] { material };
                }
            } else if (!color.Equals(UnityEngine.Color.clear)) {
                Texture2D texture = CreateTexture(16, rudelingTexture);
                updateTexture(texture, 4, 8, color);
                updateTexture(texture, 12, 8, color);
                if (rudeling.name.ToLower().Contains("big")) {
                    if (ColorUtility.DoTryParseHtmlColor("#FF1919", out var bigColor)) {
                        updateTexture(texture, 12, 12, bigColor);
                    }
                    updateTexture(texture, 8, 8, UnityEngine.Color.black);
                } else {
                    updateTexture(texture, 8, 8, color);
                }
                if (isProgUseful) {
                    updateTexture(texture, 12, 12, UnityEngine.Color.cyan);
                    updateTexture(texture, 12, 8, UnityEngine.Color.cyan);
                }
                rudeling.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials[0].mainTexture = texture;
            }
        }

        private static void updateTexture(Texture2D texture, int x, int y, UnityEngine.Color color, int sqSize = 4) {
            for (int i = x; i < x + sqSize; i++) {
                for (int j = y; j < y + sqSize; j++) {
                    texture.SetPixel(i, j, color);
                }
            }
            texture.Apply();
        }

        private static Texture2D CreateTexture(int size, List<string> colors) {
            Texture2D Texture = new Texture2D(size, size, TextureFormat.RGB24, false);
            Texture.filterMode = FilterMode.Point;
            for (int i = 0; i < 16; i++) {
                if (ColorUtility.TryParseHtmlString(colors[i], out UnityEngine.Color color)) {
                    for (int j = 0; j < (size / 4); j++) {
                        for (int k = 0; k < (size / 4); k++) {
                            Texture.SetPixel((PaletteEditor.ColorIndices[i][0] * (size / 4)) + j, (PaletteEditor.ColorIndices[i][1] * (size / 4)) + k, color);
                        }
                    }
                }
            }
            Texture.Apply();
            return Texture;
        }

        public static void ApplyHedgehogTexture(GameObject hedgehog, Material material, UnityEngine.Color color, bool isProgUseful = false) {
            if (material != null) {
                hedgehog.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials[0] = material;
            } else if (!color.Equals(UnityEngine.Color.clear)) {
                hedgehog.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials[0].color = color;
                if (isProgUseful) {
                    hedgehog.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials[1].color = UnityEngine.Color.cyan;
                }
            }
        }

        public static void ApplyEnvoyTexture(GameObject envoy, Material material, UnityEngine.Color color, bool isProgUseful = false) {
            if (material != null) {
                foreach (MeshRenderer renderer in envoy.transform.GetChild(0).GetComponentsInChildren<MeshRenderer>()) {
                    if (renderer.name.Contains("shield") || renderer.name.Contains("spear")) {
                        renderer.material = material;
                    }
                }
            } else if (!color.Equals(UnityEngine.Color.clear)) {
                Texture2D texture = CreateTexture(4, envoyTexture);
                updateTexture(texture, 2, 3, color, sqSize: 1);
                updateTexture(texture, 3, 3, color, sqSize: 1);
                if (isProgUseful) {
                    updateTexture(texture, 3, 3, UnityEngine.Color.cyan, sqSize: 1);
                }
                envoy.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].mainTexture = texture;
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
        public static void HonourGuard_dropShield_PostfixPatch(HonourGuard __instance) {
            if (SaveFlags.GetBool(ShuffleEnemyDropsEnabled) && __instance.GetComponent<EnemyCheck>() != null) {
                __instance.shieldPrefab.GetComponent<MeshRenderer>().materials = EnemyRandomizer.Enemies["Honourguard"].GetComponent<HonourGuard>().shieldRoot.GetComponent<MeshRenderer>().materials;
            }
        }

        public static void ApplyBeefboyTexture(GameObject beefboy, Material material, UnityEngine.Color color, bool isProgUseful) {
            if (material != null) {
                foreach (MeshRenderer renderer in beefboy.transform.GetChild(2).GetComponentsInChildren<MeshRenderer>(true)) {
                    if (renderer.name.Contains("shield") || renderer.name.Contains("sword")) {
                        renderer.material = material;
                    }
                }
            } else if (!color.Equals(UnityEngine.Color.clear)) {
                Texture2D texture = CreateTexture(16, beefboyTexture);
                updateTexture(texture, 0, 12, color);
                if (isProgUseful) {
                    updateTexture(texture, 4, 0, UnityEngine.Color.cyan);
                    updateTexture(texture, 4, 4, UnityEngine.Color.cyan);
                    updateTexture(texture, 4, 8, UnityEngine.Color.cyan);
                    updateTexture(texture, 4, 12, UnityEngine.Color.cyan);
                }
                beefboy.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].mainTexture = texture;
                foreach (MeshRenderer renderer in beefboy.transform.GetChild(2).GetComponentsInChildren<MeshRenderer>(true)) {
                    if (renderer.name.Contains("beefboy_debris")) {
                        renderer.material.mainTexture = texture;
                    }
                }
            }
        }

        public static void ApplyPloverTexture(GameObject plover, Material material, UnityEngine.Color color, bool isProgUseful) {
            if (material != null) {
                plover.GetComponentInChildren<CreatureMaterialManager>().originalMaterials = new Material[] { material };                    
            } else if (!color.Equals(UnityEngine.Color.clear)) {
                Texture2D texture = CreateTexture(4, ploverTexture);
                updateTexture(texture, 0, 0, color, sqSize: 2);
                if (isProgUseful) { 
                    updateTexture(texture, 2, 0, UnityEngine.Color.cyan, sqSize: 2);
                }

                plover.GetComponentInChildren<CreatureMaterialManager>().originalMaterials[0].mainTexture = texture;
            }
        }
    }
}
