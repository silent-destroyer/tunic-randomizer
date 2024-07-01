using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace TunicRandomizer {
    public class PaletteEditor : MonoBehaviour {
        
        public static int SelectedIndex = 0;
        public static Color SelectedColor;
        public static bool ApplyColor = true;
        public static bool EditorOpen = false;
        public static Dictionary<int, string> ColorNames = new Dictionary<int, string>() {
            {0, "Fur"},
            {1, "Hair"},
            {2, "Hero's\nLaurels"},
            {3, "Tunic"},
            {4, "Fur\n(Secondary)"},
            {5, "Paws,\nNose"},
            {6, "Cape...?"},
            {7, "Tunic\nUnderside"},
            {8, "Ear\n(Inner)"},
            {9, "Lure\n(Eye)"},
            {10, "Unused"},
            {11, "Tunic\n(Belt)"},
            {12, "Eye"},
            {13, "Eye\n(Pupil)"},
            {14, "Glasses\n(Custom)"},
            {15, "Scarf,\nMouth"},
        };
        public static Dictionary<int, int[]> ColorIndices = new Dictionary<int, int[]>() {
            {0, new int[]{ 0, 3 } },
            {1, new int[]{ 1, 3 } },
            {2, new int[]{ 2, 3 } },
            {3, new int[]{ 3, 3 } },
            {4, new int[]{ 0, 2 } },
            {5, new int[]{ 1, 2 } },
            {6, new int[]{ 2, 2 } },
            {7, new int[]{ 3, 2 } },
            {8, new int[]{ 0, 1 } },
            {9, new int[]{ 1, 1 } },
            {10, new int[]{ 2, 1 } },
            {11, new int[]{ 3, 1 } },
            {12, new int[]{ 0, 0 } },
            {13, new int[]{ 1, 0 } },
            {14, new int[]{ 2, 0 } },
            {15, new int[]{ 3, 0 } },
        };
        public static Dictionary<int, Color> DefaultColors = new Dictionary<int, Color>() {
            {0, new Color(0.9333333f, 0.5803922f, 0.3921569f) },
            {1, new Color(0.9333333f, 0.5803922f, 0.3921569f) },
            {2, new Color(0.8577f, 0.5044f, 1.7513f, 1f) },
            {3, new Color(0.5568628f, 0.8f, 0.4509804f) },
            {4, new Color(1f, 1f, 1f) },
            {5, new Color(0.4352941f, 0.2980392f, 0.2666667f) },
            {6, new Color(0.9882353f, 0.4431373f, 0.945098f) },
            {7, new Color(0.2941177f, 0.5058824f, 0.2f) },
            {8, new Color(0.7647059f, 0.7490196f, 0.7450981f) },
            {9, new Color(0f, 0f, 0f) },
            {10, new Color(0f, 0f, 0f) },
            {11, new Color(0.4352941f, 0.2980392f, 0.2666667f) },
            {12, new Color(0f, 0f, 0f) },
            {13, new Color(1f, 1f, 1f) },
            {14, new Color(0.9882353f, 0.4431373f, 0.945098f) },
            {15, new Color(0.9882353f, 0.4431373f, 0.945098f) },
        };

        public static Color Red = new Color(1f, .25f, .25f, 1f);
        public static Color Green = new Color(0.2729f, 0.7925f, 0.4009f, 1);
        public static Color Gold = new Color(0.917f, 0.65f, .08f);

        public static bool CelShadingEnabled = false;
        public static bool PartyHatEnabled = false;
        public static GameObject ToonFox;
        public static GameObject RegularFox;
        public static GameObject GhostFox;
        public static GameObject FoxCape;
        public static List<Renderer> HyperdashRenderers = new List<Renderer>();
        public static Font OdinRounded;

        private void OnGUI() {

            if (EditorOpen && SceneLoaderPatches.SceneName != "TitleScreen") {
                GUI.skin.font = OdinRounded;
                GUI.backgroundColor = Color.black;
                Cursor.visible = true;
                GUI.Window(102, new Rect(10f, 230f, 800f, 400f), new Action<int>(PaletteEditorWindow), "Palette Editor");
                GUI.backgroundColor = Color.white;
            } else if (SceneLoaderPatches.SceneName != "TitleScreen") {
                Cursor.visible = false;
            }

        }

        private static void PaletteEditorWindow(int windowID) {
            GUI.skin.label.fontSize = 20;
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.skin.label.clipping = TextClipping.Overflow;
            GUI.color = Color.white;
            GUI.DragWindow(new Rect(200f, 50f, 10000f, 30f));
            GUI.skin.button.fontSize = 15;
            for (int i = 0; i < 16; i++) {
                GUI.backgroundColor = PlayerPalette.runtimePalette.GetPixel(ColorIndices[i][0], ColorIndices[i][1]);
                bool PaletteButton = GUI.Button(new Rect((float)(10 + 75 * (i % 4)), (float)(35 + 75 * Mathf.FloorToInt((float)i / 4f)), 75f, 75f), ColorNames[i]);

                GUI.backgroundColor = Color.white;
                if (PaletteButton) {
                    SelectedIndex = i;
                }
            }
            GUI.skin.button.fontSize = 20;
            bool PaletteSelected = SelectedIndex >= 0 && SelectedIndex < 16;
            if (PaletteSelected) {

                SelectedColor = PlayerPalette.runtimePalette.GetPixel(ColorIndices[SelectedIndex][0], ColorIndices[SelectedIndex][1]);


                GUI.skin.label.fontSize = 25;
                GUI.Label(new Rect(350f, 35, 400f, 30f), "Selected: " + ColorNames[SelectedIndex].Replace("\n", " "));
                SelectedColor = RGBSlider(new Rect(350f, 85f, 400f, 10f), SelectedColor);
                bool RandomizeSelected = GUI.Button(new Rect(350f, 235f, 200f, 30f), "Randomize selected");
                if (RandomizeSelected) {
                    SelectedColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1f);
                }

                PlayerPalette.runtimePalette.SetPixel(ColorIndices[SelectedIndex][0], ColorIndices[SelectedIndex][1], SelectedColor);
                PlayerPalette.runtimePalette.Apply();
                Color SetColor = PlayerPalette.runtimePalette.GetPixel(ColorIndices[SelectedIndex][0], ColorIndices[SelectedIndex][1]);
                if (SelectedIndex == 2 && (SelectedColor.r != SetColor.r || SelectedColor.g != SetColor.g || SelectedColor.b != SetColor.b)) {
                    ChangeHyperdashColors(SelectedColor);
                }
                if (SelectedIndex == 14) {
                    ChangeSunglassesColor(SelectedColor);
                }
                if (SelectedIndex == 6) {
                    ChangeCapeColor(SelectedColor);
                }
            }
            bool RandomizeAll = GUI.Button(new Rect(555f, 235f, 200f, 30f), "Randomize all");
            if (RandomizeAll) {
                RandomizeFoxColors();
            }
            bool ResetSelected = GUI.Button(new Rect(350f, 285f, 200f, 30f), "Reset Selected");
            if (ResetSelected) {
                if (PaletteSelected) {
                    PlayerPalette.runtimePalette.SetPixel(ColorIndices[SelectedIndex][0], ColorIndices[SelectedIndex][1], DefaultColors[SelectedIndex]);
                }
            }
            bool ResetAll = GUI.Button(new Rect(555f, 285f, 200f, 30f), "Reset All");
            if (ResetAll) {
                RevertFoxColors();
            }
            bool Save = GUI.Button(new Rect(350f, 335f, 200f, 30f), "Save Texture");
            if (Save) {
                SaveTexture();
            }
            bool Load = GUI.Button(new Rect(555f, 335f, 200f, 30f), "Load Texture");
            if (Load) {
                LoadCustomTexture();
            }
            bool Close = GUI.Button(new Rect(60f, 350f, 200f, 30f), "Close Editor");
            if (Close) {
                EditorOpen = false;
                CameraController.DerekZoom = 1f;
            }
            bool ToggleCustomTextureUse = GUI.Toggle(new Rect(350f, 370f, 275f, 30f), TunicRandomizer.Settings.UseCustomTexture, "Always Apply Saved Texture");
            TunicRandomizer.Settings.UseCustomTexture = ToggleCustomTextureUse;

        }

        private static Color RGBSlider(Rect screenRect, Color rgb) {
            GUI.skin.label.fontSize = 16;
            GUI.Label(screenRect, $"Red: {(rgb.r.ToString().Length < 5 ? rgb.r.ToString() : rgb.r.ToString().Substring(0, 5))}");
            screenRect.y += 25f;
            rgb.r = GUI.HorizontalSlider(screenRect, rgb.r, 0f, 1f);
            screenRect.y += 20f;
            GUI.Label(screenRect, $"Green: {(rgb.g.ToString().Length < 5 ? rgb.g.ToString() : rgb.g.ToString().Substring(0, 5))}");
            screenRect.y += 25f;
            rgb.g = GUI.HorizontalSlider(screenRect, rgb.g, 0f, 1f);
            screenRect.y += 20f;
            GUI.Label(screenRect, $"Blue: {(rgb.b.ToString().Length < 5 ? rgb.b.ToString() : rgb.b.ToString().Substring(0, 5))}");
            screenRect.y += 25f;
            rgb.b = GUI.HorizontalSlider(screenRect, rgb.b, 0f, 1f);
            GUI.skin.label.fontSize = 20;
            Color oldcolor = GUI.backgroundColor;
            GUI.backgroundColor = rgb;
            //GUI.Button(new Rect(10f, 330f, 100f, 30f), "Color Preview");
            GUI.backgroundColor = oldcolor;
            return rgb;
        }

        private static void SaveTexture() {
            string TexturePath = Application.persistentDataPath + "/Randomizer/texture.png";
            if (File.Exists(TexturePath)) {
                File.Delete(TexturePath);
            } else {
                PlayerPalette.runtimePalette.SetPixel(2, 3, new Color(0.8577f, 0.5044f, 1.7513f, 1f));
                PlayerPalette.runtimePalette.SetPixel(2, 0, PlayerPalette.runtimePalette.GetPixel(3, 0));
            }

            File.WriteAllBytes(TexturePath, ImageConversion.EncodeToPNG(PlayerPalette.runtimePalette));
            TunicLogger.LogInfo("Saved Custom Texture to " + TexturePath);
        }

        public static void LoadCustomTexture() {
            string TexturePath = Application.persistentDataPath + "/Randomizer/texture.png";
            if (!File.Exists(TexturePath)) {
                SaveTexture();
            }
            if (File.Exists(TexturePath)) {
                Texture2D CustomPalette = new Texture2D(4, 4, TextureFormat.DXT1, false);
                byte[] CustomPaletteData = File.ReadAllBytes(TexturePath);
                ImageConversion.LoadImage(PlayerPalette.runtimePalette, CustomPaletteData);
                PlayerPalette.runtimePalette.Apply();
                ChangeHyperdashColors(PlayerPalette.runtimePalette.GetPixel(2, 3));
                ChangeSunglassesColor(PlayerPalette.runtimePalette.GetPixel(2, 0));
                ChangeCapeColor(PlayerPalette.runtimePalette.GetPixel(2, 2));
            }
        }

        public static void RandomizeFoxColors() {
            if (PlayerPalette.runtimePalette != null) {
                for (int i = 0; i < 16; i++) {
                    // Fox Palette
                    PlayerPalette.runtimePalette.SetPixel(Mathf.FloorToInt(i / 4f), i % 4, new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1));
                    PlayerPalette.runtimePalette.Apply();
                }
            }
            // Hyperdash color
            Color HyperdashColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
            ChangeHyperdashColors(HyperdashColor);
            // Scav Mask color
            GameObject ScavengerMask = GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/head/scavenger_mask");
            if (ScavengerMask != null) {
                ScavengerMask.GetComponent<MeshRenderer>().material.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 0f);
            }
            // Sunglasses color
            GameObject TheRealest = GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/head/therealest");
            if (TheRealest != null) {
                TheRealest.GetComponent<MeshRenderer>().material.mainTexture = Texture2D.whiteTexture;
                TheRealest.GetComponent<MeshRenderer>().material.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
            }

            ChangeCapeColor(new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1));
        }

        public static void RevertFoxColors() {
            Color HyperdashColor = new Color(0.8577f, 0.5044f, 1.7513f, 1f);
            ChangeHyperdashColors(HyperdashColor);
            foreach (int i in ColorIndices.Keys) {
                PlayerPalette.runtimePalette.SetPixel(ColorIndices[i][0], ColorIndices[i][1], DefaultColors[i]);
            }
            PlayerPalette.runtimePalette.Apply();
            // Sunglasses color
            GameObject TheRealest = GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/head/therealest");
            if (TheRealest != null) {
                TheRealest.GetComponent<MeshRenderer>().material.mainTexture = PlayerPalette.runtimePalette;
                TheRealest.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
            }
            ChangeCapeColor(DefaultColors[15]);
        }

        public static void GatherHyperdashRenderers() {
            HyperdashRenderers.Clear();

            foreach (AnimationEvents HyperdashAnimationEvent in Resources.FindObjectsOfTypeAll<AnimationEvents>().Where(AnimationEvent => AnimationEvent.gameObject.name == "Hyperdash FX" || AnimationEvent.gameObject.name == "Hyperdash Termination FX")) {
                GameObject HyperdashFX = HyperdashAnimationEvent.gameObject;
                for (int i = 0; i < HyperdashFX.transform.childCount; i++) {
                    if (HyperdashFX.transform.GetChild(i).GetComponent<MeshRenderer>() != null) {
                        HyperdashRenderers.Add(HyperdashFX.transform.GetChild(i).GetComponent<MeshRenderer>());
                    }
                    if (HyperdashFX.transform.GetChild(i).GetComponent<ParticleSystemRenderer>() != null) {
                        HyperdashRenderers.Add(HyperdashFX.transform.GetChild(i).GetComponent<ParticleSystemRenderer>());
                    }
                }
            }

            foreach (ParticleSystemRenderer psr in Resources.FindObjectsOfTypeAll<ParticleSystemRenderer>().Where(particles => particles.gameObject.name.Contains("DisapparFX"))) {
                HyperdashRenderers.Add(psr);
            }

            GameObject HyperdashParticles = GameObject.Find("_Fox(Clone)/Hyperdash Appear/DisapparFX");
            if (HyperdashParticles != null) {
                if (HyperdashParticles.GetComponent<ParticleSystemRenderer>() != null) {
                    HyperdashRenderers.Add(HyperdashParticles.GetComponent<ParticleSystemRenderer>());
                }
            }
            if (ModelSwaps.Items.ContainsKey("Hyperdash") && ModelSwaps.Items["Hyperdash"] != null) {
                HyperdashRenderers.Add(ModelSwaps.Items["Hyperdash"].GetComponent<MeshRenderer>());
            }
        }

        public static void ChangeHyperdashColors(Color HyperdashColor) {
            GameObject Hyperdash = GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/head/GameObject");
            if (Hyperdash != null) {
                HyperdashRenderers.Add(Hyperdash.GetComponent<MeshRenderer>());
            }
            foreach (Renderer renderer in HyperdashRenderers) {
                if (renderer != null && renderer.material != null) {
                    renderer.material.color = HyperdashColor;
                }
            }
        }

        public static void ChangeSunglassesColor(Color GlassesColor) {
            try {
                GameObject TheRealest = GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/head/therealest");
                if (TheRealest != null) {
                    TheRealest.GetComponent<MeshRenderer>().material.mainTexture = Texture2D.whiteTexture;
                    TheRealest.GetComponent<MeshRenderer>().material.color = GlassesColor;
                }
            } catch (Exception e) {
                TunicLogger.LogInfo("Error changing Sunglasses Color!" + e.Message);
            }
        }

        public static void ChangeCapeColor(Color CapeColor) {
            try {
                if (FoxCape != null) {
                    FoxCape.GetComponent<MeshRenderer>().material.mainTexture = Texture2D.whiteTexture;
                    FoxCape.GetComponent<MeshRenderer>().material.color = CapeColor;
                }
                if (ModelSwaps.Items.ContainsKey("Cape") && ModelSwaps.Items["Cape"] != null) {
                    ModelSwaps.Items["Cape"].GetComponent<MeshRenderer>().material.mainTexture = Texture2D.whiteTexture;
                    ModelSwaps.Items["Cape"].GetComponent<MeshRenderer>().material.color = CapeColor;
                }
            } catch (Exception e) {
                TunicLogger.LogInfo("Error changing Cape Color!" + e.Message);
            }
        }

        public static void SetupPartyHat(PlayerCharacter Player) {
            GameObject FloppyHat = new GameObject("floppy hat");
            FloppyHat.transform.parent = Player.transform.GetChild(0).GetChild(0).GetChild(8).GetChild(0).GetChild(3);//GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/head/").transform;
            Material ToonFox = PlayerCharacter.instance.transform.GetChild(25).GetComponent<SkinnedMeshRenderer>().material;
            FloppyHat.AddComponent<MeshFilter>().mesh = PlayerCharacter.instance.transform.GetChild(25).GetComponent<SkinnedMeshRenderer>().sharedMesh;
            FloppyHat.AddComponent<MeshRenderer>().material = ToonFox;
            FloppyHat.transform.localScale = new Vector3(0.75f, 0.5f, 0.75f);
            FloppyHat.transform.localPosition = new Vector3(-0.2f, -0.3f, 0);
            FloppyHat.transform.localRotation = new Quaternion(0f, 0f, 0f, 1f);
            FloppyHat.SetActive(false);
        }

        public static void ApplyCelShading() {
            Material[] Body = new Material[] {
                    ToonFox.GetComponent<MeshRenderer>().material,
                    PlayerCharacter.instance.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials[1]
                };
            Material[] Hair = new Material[] {
                    ToonFox.GetComponent<MeshRenderer>().material,
                    PlayerCharacter.instance.transform.GetChild(3).GetComponent<CreatureMaterialManager>().originalMaterials[1]
                };
            if (CustomItemBehaviors.CanTakeGoldenHit) {
                CustomItemBehaviors.FoxBody.GetComponent<MeshRenderer>().materials = Body;
                CustomItemBehaviors.FoxHair.GetComponent<MeshRenderer>().materials = Hair;
            } else {
                PlayerCharacter.instance.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials = Body;
                PlayerCharacter.instance.transform.GetChild(3).GetComponent<CreatureMaterialManager>().originalMaterials = Hair;
            }

            PlayerCharacter.instance.transform.GetChild(0).GetChild(0).GetChild(8).GetChild(0).GetChild(3).GetChild(9).GetComponent<MeshRenderer>().material = ToonFox.GetComponent<MeshRenderer>().material;
            //GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/head/floppy hat").GetComponent<MeshRenderer>().material = ToonFox;
            foreach (NPC npc in Resources.FindObjectsOfTypeAll<NPC>().ToList()) {
                npc.transform.GetChild(2).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = ToonFox.GetComponent<MeshRenderer>().material;
            }
            CelShadingEnabled = true;
        }

        public static void DisableCelShading() {
            Material[] Body = new Material[] {
                    RegularFox.GetComponent<MeshRenderer>().material,
                    PlayerCharacter.instance.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials[1]
                };
            Material[] Hair = new Material[] {
                    RegularFox.GetComponent<MeshRenderer>().material,
                    PlayerCharacter.instance.transform.GetChild(3).GetComponent<CreatureMaterialManager>().originalMaterials[1]
                };
            if (CustomItemBehaviors.CanTakeGoldenHit) {
                CustomItemBehaviors.FoxBody.GetComponent<MeshRenderer>().materials = Body;
                CustomItemBehaviors.FoxHair.GetComponent<MeshRenderer>().materials = Hair;
            } else {
                PlayerCharacter.instance.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials = Body;
                PlayerCharacter.instance.transform.GetChild(3).GetComponent<CreatureMaterialManager>().originalMaterials = Hair;
            }

            PlayerCharacter.instance.transform.GetChild(0).GetChild(0).GetChild(8).GetChild(0).GetChild(3).GetChild(9).GetComponent<MeshRenderer>().material = RegularFox.GetComponent<MeshRenderer>().material;
            foreach (NPC npc in Resources.FindObjectsOfTypeAll<NPC>().ToList()) {
                npc.transform.GetChild(2).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = GhostFox.GetComponent<MeshRenderer>().material;
            }
            CelShadingEnabled = false;
        }

        public static void SetupFoxCape(PlayerCharacter player) {

            if (FoxCape != null) { 
                GameObject.Destroy(FoxCape);
            }

            GameObject CapeTransform = player.transform.GetChild(0).GetChild(0).GetChild(8).GetChild(0).GetChild(2).gameObject;

            if (CapeTransform != null) {
                FoxCape = new GameObject("Cape");
                FoxCape.transform.parent = CapeTransform.transform;
                FoxCape.AddComponent<MeshFilter>().mesh = player.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().sharedMesh;
                FoxCape.AddComponent<MeshRenderer>().materials = player.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials;
                FoxCape.AddComponent<VisibleByHavingInventoryItem>().enablingItem = Inventory.GetItemByName("Cape");
                FoxCape.GetComponent<VisibleByHavingInventoryItem>().renderers = new Renderer[] { FoxCape.GetComponent<MeshRenderer>() };
                FoxCape.GetComponent<VisibleByHavingInventoryItem>().lights = new Light[] { };
                FoxCape.AddComponent<CreatureMaterialManager>();
                FoxCape.transform.localScale = Vector3.one;
                FoxCape.transform.localEulerAngles = new Vector3(30f, 0f, 180f);
                FoxCape.transform.localPosition = new Vector3(0f, 1f, 0.8f);

                GameObject CapePresentation = Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(ipg => ipg.name == "cape").First().gameObject;
                CapePresentation.GetComponent<MeshFilter>().mesh = player.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().sharedMesh;
                CapePresentation.GetComponent<MeshRenderer>().materials = player.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials;
                ChangeCapeColor(new Color(0.9882353f, 0.4431373f, 0.945098f));
            }
        }

    }
}
