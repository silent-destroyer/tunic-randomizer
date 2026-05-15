using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {

    public class FoolTrap {

        public enum TrapType {
            Ice,
            Fire,
            Bee,
            Tiny,
            Mirror,
            Deisometric,
            CRT,
            Vintage,
            Trip,  // for trap link
            Zoom,  // for trap link
            Bald,  // for trap link
            Home,  // for trap link
            Whoops,// for trap link
            Wide,  // for trap link
            ZoomOut, // for trap link
            Saturation, // for trap link
            Depletion, // for trap link
            Disarm, // for trap link
            Fast, // for trap link
        }

        // potential todos:
        // Chaos Trap from Anodyne, randomizes the game's textures for a short while, could mess with the fox colors or something I guess
        // Depletion Trap from The Grinch, resets ammo to zero, could reduce your mana to zero, same with Dry Trap from DK64, same for Mana Drain Trap
        // Disarm Trap from Anodyne, unequips current broom (weapon), could make it just unequip your items
        // Explosion Trap, could swap it from fire trap to actually explode you, also for bomb, tnt, etc.
        // Frog Trap from Glover, could spawn a frog
        // Invert Colors Trap from Paint, idk we could probably do something with this
        // Paper Trap from DK64, turns player and most enemies 2d, could maybe do something with the character scale

        public class Trap {
            public string Name;
            public int Weight;  // for random fool traps
            public int AltWeight; // for trap link trap types

            public Trap(string name, int weight) {
                Name = name;
                Weight = weight;
                AltWeight = 0;
            }
            public Trap(string name, int weight, int altWeight) {
                Name = name;
                Weight = weight;
                AltWeight = altWeight;
            }
        }

        public static Dictionary<TrapType, Trap> Traps = new Dictionary<TrapType, Trap> {
            {TrapType.Ice, new Trap("Ice Trap", 20) },
            {TrapType.Fire, new Trap("Fire Trap", 20) },
            {TrapType.Bee, new Trap("Bee Trap", 15) },
            {TrapType.Tiny, new Trap("Tiny Trap", 15) },
            {TrapType.Mirror, new Trap("Screen Flip Trap", 10) },
            {TrapType.Deisometric, new Trap("Deisometric Trap", 10) },
            {TrapType.Trip, new Trap("Trip Trap", 0, 5) },
            {TrapType.Zoom, new Trap("Zoom Trap", 5) },
            {TrapType.Bald, new Trap("Bald Trap", 0, 5) },
            {TrapType.Home, new Trap("Home Trap", 0, 1) },
            {TrapType.Whoops, new Trap("Whoops! Trap", 0, 5) },
            {TrapType.Wide, new Trap("W I D E Trap", 0, 5) },
            {TrapType.ZoomOut, new Trap("Zoom Out Trap", 0, 5) },
            {TrapType.CRT, new Trap("CRT Trap", 10) },
            {TrapType.Vintage, new Trap("Vintage Trap", 1, 1) },
            {TrapType.Saturation, new Trap("Saturation Trap", 0, 5) },
            {TrapType.Depletion, new Trap("Depletion Trap", 0, 5) },
            {TrapType.Disarm, new Trap("Disarm Trap", 0, 5) },
            {TrapType.Fast, new Trap("Fast Trap", 0, 5) },
        };

        // for TrapLink, we convert names of similar traps to our trap types for receiving traps
        public static Dictionary<string, TrapType> TrapNameToType = new Dictionary<string, TrapType> {
            {"Ice Trap", TrapType.Ice },
            {"Freeze Trap", TrapType.Ice },
            {"Frozen Trap", TrapType.Ice },
            {"Stun Trap", TrapType.Ice },
            {"Paralyze Trap", TrapType.Ice },
            {"Paralysis Trap", TrapType.Ice },
            {"Chaos Control Trap", TrapType.Ice },
            {"Bubble Trap", TrapType.Ice },  // DK64, freezes the player in a bubble for a short time
            {"Frost Trap", TrapType.Ice },
            {"Sleep Trap", TrapType.Ice },  // freezing is sleep right

            {"Fire Trap", TrapType.Fire },
            {"Damage Trap", TrapType.Fire },
            {"Explosion Trap", TrapType.Fire },
            {"Bomb", TrapType.Fire },  // Luigi's Mansion, yes it's just Bomb
            {"Bomb Trap", TrapType.Fire },
            {"Burn Trap", TrapType.Fire },
            {"Posession Trap", TrapType.Fire },  // Luigi's Mansion, damage-based trap
            {"Nut Trap", TrapType.Fire },  // DKC, damage-based trap
            {"TNT Trap", TrapType.Fire },
            {"TNT Barrel Trap", TrapType.Fire },

            {"Bee Trap", TrapType.Bee },

            {"Tiny Trap", TrapType.Tiny },
            {"Poison Mushroom", TrapType.Tiny },  // Luigi's Mansion, makes player smaller

            {"Screen Flip Trap", TrapType.Mirror },
            {"Mirror Trap", TrapType.Mirror },
            {"Reverse Trap", TrapType.Mirror },
            {"Reversal Trap", TrapType.Mirror },
            {"Reverse Controls Trap", TrapType.Mirror },
            {"Flip Horizontal Trap", TrapType.Mirror },
            {"Flip Vertical Trap", TrapType.Mirror },
            {"Flip Trap", TrapType.Mirror },
            {"Gas Trap", TrapType.Mirror },  // Anodyne, reverses controls, this seemed close enough

            {"Deisometric Trap", TrapType.Deisometric },
            {"Confuse Trap", TrapType.Deisometric },
            {"Confusion Trap", TrapType.Deisometric },
            {"Fuzzy Trap", TrapType.Vintage },
            {"Confound Trap", TrapType.Deisometric },
            {"Camera Rotate Trap", TrapType.Deisometric },

            {"Bonk Trap", TrapType.Trip },
            {"Banana Trap", TrapType.Trip },
            {"Banana Peel Trap", TrapType.Trip },
            {"Spring Trap", TrapType.Whoops },
            {"Ice Floor Trap", TrapType.Trip },  // DK64, makes the floor slippery for 15 seconds, we could make this ice trap instead
            {"Shake Trap", TrapType.Trip },  // Saving Princess, shakes the screen a lot, making it harder to control the player, so this seemed close
            {"Slip Trap", TrapType.Trip },

            {"Zoom Trap", TrapType.Zoom },  // Celeste, zooms camera in
            {"Fish Eye Trap", TrapType.Zoom },
            {"Zoom In Trap", TrapType.Zoom },

            {"Zoom Out Trap", TrapType.ZoomOut },

            {"Bald Trap", TrapType.Bald },  // Celeste, bald
            
            {"Whoops! Trap", TrapType.Whoops }, // Here Comes Niko, drops the player from way high up
            
            {"W I D E Trap", TrapType.Wide }, // Here Comes Niko, makes the fox W I D E
            {"Squash Trap", TrapType.Wide },  // yeahhh squish that fox

            {"Home Trap", TrapType.Home }, // Here Comes Niko, teleports the player "home", overworld in this case
            {"Teleport Trap", TrapType.Home },

            {"Trip Trap", TrapType.Trip },
            {"CRT Trap", TrapType.CRT },
            {"Vintage Trap", TrapType.Vintage },
            {"Saturation Trap", TrapType.Saturation },

            {"144p Trap", TrapType.Vintage},
            {"Chaos Trap", TrapType.Saturation},
            {"Crystal Trap", TrapType.CRT},
            {"Depletion Trap", TrapType.Depletion},
            {"Disarm Trap", TrapType.Disarm},
            {"Dry Trap", TrapType.Depletion},
            {"Eject Ability", TrapType.Disarm },
            {"Electrocution Trap", TrapType.Ice},
            {"Empty Item Box Trap", TrapType.Disarm },
            {"Energy Drain Trap", TrapType.Depletion },
            {"Fast Trap", TrapType.Fast},
            {"Fracture Trap", TrapType.CRT},
            {"Invert Colors Trap", TrapType.Saturation },
            {"Jump Trap", TrapType.Whoops },
            {"Jumping Jacks Trap", TrapType.Fire },
            {"Mana Drain Trap", TrapType.Depletion },
            {"Pixelate Trap", TrapType.CRT },
            {"Pixellation Trap", TrapType.CRT },
            {"Push Trap", TrapType.Trip},
            {"Radiation Trap", TrapType.Ice },
            {"Ranch Trap", TrapType.Home },
            {"Spotlight Trap", TrapType.Zoom },

        };

        public static bool StungByBee = false;
        public static bool TinierFox = false;
        // for trap link
        public static bool BaldFox = false;
        public static bool WideFox = false;
        public static bool ZoomedCamera = false;
        public static bool CRTTrap = false;
        public static bool VintageTrap = false;
        public static bool FastTrap = false;
        public static bool SaturatedTrap = false;

        public static (string, string) ApplyFoolEffect(TrapType trapType) {
            string FoolMessageTop = $"";
            string FoolMessageBottom = $"";
            // this is a switch instead of an if else because I wrote it for a spot where switch was reasonable, then moved it
            switch (trapType) {
                case TrapType.Ice:
                    (FoolMessageTop, FoolMessageBottom) = FoolIceTrap();
                    break;
                case TrapType.Fire:
                    (FoolMessageTop, FoolMessageBottom) = FoolFireTrap();
                    break;
                case TrapType.Bee:
                    (FoolMessageTop, FoolMessageBottom) = FoolBeeTrap();
                    break;
                case TrapType.Tiny:
                    (FoolMessageTop, FoolMessageBottom) = FoolTinyTrap();
                    break;
                case TrapType.Mirror:
                    (FoolMessageTop, FoolMessageBottom) = FoolMirrorTrap();
                    break;
                case TrapType.Deisometric:
                    (FoolMessageTop, FoolMessageBottom) = FoolDeisometricTrap();
                    break;
                case TrapType.Trip:
                    (FoolMessageTop, FoolMessageBottom) = FoolTripTrap();
                    break;
                case TrapType.Zoom:
                    (FoolMessageTop, FoolMessageBottom) = FoolZoomTrap();
                    break;
                case TrapType.CRT:
                    (FoolMessageTop, FoolMessageBottom) = FoolCRTTrap();
                    break;
                case TrapType.Vintage:
                    (FoolMessageTop, FoolMessageBottom) = FoolVintageTrap();
                    break;
                case TrapType.Bald:
                    (FoolMessageTop, FoolMessageBottom) = FoolBaldTrap();
                    break;
                case TrapType.Whoops:
                    (FoolMessageTop, FoolMessageBottom) = FoolWhoopsTrap();
                    break;
                case TrapType.Wide:
                    (FoolMessageTop, FoolMessageBottom) = FoolWideTrap();
                    break;
                case TrapType.Home:
                    (FoolMessageTop, FoolMessageBottom) = FoolHomeTrap();
                    break;
                case TrapType.ZoomOut:
                    (FoolMessageTop, FoolMessageBottom) = FoolZoomOutTrap();
                    break;
                case TrapType.Saturation:
                    (FoolMessageTop, FoolMessageBottom) = FoolSaturatedTrap();
                    break;
                case TrapType.Depletion:
                    (FoolMessageTop, FoolMessageBottom) = FoolDepletionTrap();
                    break;
                case TrapType.Disarm:
                    (FoolMessageTop, FoolMessageBottom) = FoolDisarmTrap();
                    break;
                case TrapType.Fast:
                    (FoolMessageTop, FoolMessageBottom) = FoolFastTrap();
                    break;
                default:
                    TunicLogger.LogError("No match found for trap type " + trapType.ToString());
                    break;
            }
            return (FoolMessageTop, FoolMessageBottom);
        }


        public static (string, string) ApplyRandomFoolEffect(int Player, bool fromDeathLink = false) {
            System.Random Random = new System.Random();
            string FoolMessageTop = $"";
            string FoolMessageBottom = $"";

            List<TrapType> weightedTrapList = new List<TrapType>();
            foreach (TrapType trapType in Traps.Keys) {
                if (TunicRandomizer.Settings.FoolTrapToggles[trapType] != RandomizerSettings.FoolTrapToggle.ON && !TunicRandomizer.Settings.RaceMode) {
                    continue;
                }
                if (trapType == TrapType.Mirror && CameraController.Flip) {
                    continue;
                }
                if (trapType == TrapType.Tiny && (TinierFox || TunicRandomizer.Settings.TinierFoxMode)) {
                    continue;
                }
                if (trapType == TrapType.Bee && (StungByBee || TunicRandomizer.Settings.BiggerHeadMode)) {
                    continue;
                }
                if (trapType == TrapType.Deisometric && CameraController.DerekRotationEnabled) {
                    continue;
                }
                if (trapType == TrapType.Zoom && CameraController.DerekZoom == 0.5f) {
                    continue;
                }
                if (trapType == TrapType.CRT && (VintageTrap || CRTTrap || TunicRandomizer.Settings.RetroFilterEnabled)) {
                    continue;
                }
                if (trapType == TrapType.Vintage && VintageTrap) {
                    continue;
                }
                if (trapType == TrapType.Fast && FastTrap) {
                    continue;
                }
                if (trapType == TrapType.Depletion && PlayerCharacter.GetMP() == 0) {
                    continue;
                }
                if (trapType == TrapType.Saturation && SaturatedTrap) {
                    continue;
                }
                if (trapType == TrapType.Bald && BaldFox) {
                    continue;
                }
                if (trapType == TrapType.ZoomOut && ZoomedCamera) {
                    continue;
                }
                if (trapType == TrapType.Wide && WideFox) {
                    continue;
                }
                if (TunicRandomizer.Settings.RaceMode) {
                    weightedTrapList.AddRange(Enumerable.Repeat(trapType, Traps[trapType].Weight));
                } else {
                    weightedTrapList.AddRange(Enumerable.Repeat(trapType, Math.Max(Traps[trapType].Weight, Traps[trapType].AltWeight)));
                }
            }
            TrapType trapSelected;
            if (weightedTrapList.Count == 0) {
                trapSelected = TrapType.Ice;
            } else {
                trapSelected = weightedTrapList[Random.Next(weightedTrapList.Count)];
            }

            (FoolMessageTop, FoolMessageBottom) = ApplyFoolEffect(trapSelected);

            if (IsArchipelago() && TunicRandomizer.Settings.TrapLinkEnabled && !fromDeathLink) {
                if (Player != Archipelago.instance.GetPlayerSlot()) {
                    FoolMessageTop = $"\"{Archipelago.instance.GetPlayerName(Player)}\" %i^ks {FoolMessageTop}";
                }
                Archipelago.instance.integration.SendTrapLink(trapSelected);
            }
            if (!fromDeathLink) {
                Notifications.Show(FoolMessageTop, FoolMessageBottom);
            }
            return (FoolMessageTop, FoolMessageBottom);
        }

        public static (string, string) FoolIceTrap() {
            PlayerCharacter.ApplyRadiationAsDamageInHP(PlayerCharacter.instance.maxhp * .2f);
            SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
            SFX.PlayAudioClipAtFox(PlayerCharacter.standardFreezeSFX);
            PlayerCharacter.instance.AddFreezeTime(3f);
            string FoolMessageTop = $"yoo R A \"<#86A5FF>FOOL<#ffffff>!!\" [fooltrap]";
            string FoolMessageBottom = $"hahvi^ ahn Is tIm?";
            return (FoolMessageTop, FoolMessageBottom);
        }

        public static (string, string) FoolFireTrap() {
            PlayerCharacter.ApplyRadiationAsDamageInHP(0f);
            PlayerCharacter.instance.stamina = 0;
            PlayerCharacter.instance.cachedFireController.FireAmount = 3f;
            SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
            string FoolMessageTop = $"yoo R A \"<#FF3333>FOOL<#ffffff>!!\" [fooltrap]";
            string FoolMessageBottom = $"iz it hawt in hEr?";
            PlayerCharacter.instance.Flinch(true);
            return (FoolMessageTop, FoolMessageBottom);
        }

        public static (string, string) FoolBeeTrap() {
            SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
            PlayerCharacter.instance.IDamageable_ReceiveDamage(PlayerCharacter.instance.hp / 3, 0, Vector3.zero, 0, 0);
            string FoolMessageTop = $"yoo R A \"<#ffd700>FOOL<#ffffff>!!\" [fooltrap]";
            string FoolMessageBottom = $"\"(\"it wuhz A swRm uhv <#ffd700>bEz\"...)\"";
            StungByBee = true;
            PlayerCharacter.instance.Flinch(true);
            return (FoolMessageTop, FoolMessageBottom);
        }

        public static (string, string) FoolTinyTrap() {
            SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
            PlayerCharacter.instance.IDamageable_ReceiveDamage(PlayerCharacter.instance.hp / 3, 0, Vector3.zero, 0, 0);
            string FoolMessageTop = $"yoo R A <#FFA500>tInE \"<#FFA500>FOOL<#ffffff>!!\" [fooltrap]";
            string FoolMessageBottom = $"hahf #uh sIz, duhbuhl #uh kyoot.";
            TinierFox = true;
            PlayerCharacter.instance.Flinch(true);
            return (FoolMessageTop, FoolMessageBottom);
        }

        public static (string, string) FoolMirrorTrap() {
            SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
            PlayerCharacter.instance.IDamageable_ReceiveDamage(PlayerCharacter.instance.hp / 3, 0, Vector3.zero, 0, 0);
            string FoolMessageTop = $"[fooltrap] \"!!\"<#FF00FF>lfoo \"A ERA UOY\"";
            string FoolMessageBottom = $"tAk uh mOmint too ruhflehkt.";
            CameraController.Flip = true;
            PlayerCharacter.instance.Flinch(true);
            return (FoolMessageTop, FoolMessageBottom);
        }

        public static (string, string) FoolDeisometricTrap() {
            System.Random Random = new System.Random();
            SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
            PlayerCharacter.instance.IDamageable_ReceiveDamage(PlayerCharacter.instance.hp / 3, 0, Vector3.zero, 0, 0);
            string FoolMessageTop = $"yoo R A <#FFA500>toopointfIvdE \"<#FFA500>FOOL<#ffffff>!!\" [fooltrap]";
            string FoolMessageBottom = $"toonik iz ahn IsOmehtrik... wAt, wuht?";
            CameraController.DerekRotationEnabled = true;
            List<float> cameraRotations = new List<float>() { -45f, 45f };
            CameraController.DerekRotationRangeRight = cameraRotations[Random.Next(cameraRotations.Count)];
            PlayerCharacter.instance.Flinch(true);
            return (FoolMessageTop, FoolMessageBottom);
        }

        public static (string, string) FoolTripTrap() {
            PlayerCharacter.instance.stamina = 0;
            SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
            string FoolMessageTop = $"yoo R A \"<#FFA500>FOOL<#ffffff>!!\" [fooltrap]";
            string FoolMessageBottom = $"iz #is brawl or suhm%i^?";
            PlayerCharacter.instance.Flinch(true);
            return (FoolMessageTop, FoolMessageBottom);
        }

        public static (string, string) FoolZoomTrap() {
            SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
            string FoolMessageTop = $"yoo R A <#FFA500>zoomd in \"<#FFA500>FOOL<#ffffff>!!\" [fooltrap]";
            string FoolMessageBottom = $"wehl I kahn sE juhst fIn...";
            CameraController.DerekZoom = 0.5f;
            ZoomedCamera = true;
            PlayerCharacter.instance.Flinch(true);
            return (FoolMessageTop, FoolMessageBottom);
        }

        public static (string, string) FoolZoomOutTrap() {
            SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
            string FoolMessageTop = $"yoo R A <#FFA500>zoomd owt \"<#FFA500>FOOL<#ffffff>!!\" [fooltrap]";
            string FoolMessageBottom = $"wehl I kahn sE juhst fIn...";
            CameraController.DerekZoom = 2f;
            ZoomedCamera = true;
            PlayerCharacter.instance.Flinch(true);
            return (FoolMessageTop, FoolMessageBottom);
        }

        public static (string, string) FoolBaldTrap() {
            SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
            string FoolMessageTop = $"yoo R A <#FFA500>bawld \"<#FFA500>FOOL<#ffffff>!!\" [fooltrap]";
            string FoolMessageBottom = $"giv it bahk!";
            BaldFox = true;
            PlayerCharacter.instance.Flinch(true);
            if (Inventory.GetItemByName("Hyperdash Toggle").Quantity > 0) {
                Inventory.GetItemByName("Hyperdash").Quantity = 0; // Unequip laurels
            }
            return (FoolMessageTop, FoolMessageBottom);
        }

        public static (string, string) FoolWhoopsTrap() {
            SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
            string FoolMessageTop = $"yoo R A <#FFA500>\"<#FFA500>FOOL<#ffffff>!!\" [fooltrap]";
            string FoolMessageBottom = $"woups!";
            PlayerCharacter.instance.transform.position += new Vector3(0, 450f, 0f);
            return (FoolMessageTop, FoolMessageBottom);
        }

        public static (string, string) FoolHomeTrap() {
            string FoolMessageTop = $"yoo R A <#FFA500>\"<#FFA500>FOOL<#ffffff>!!\" [fooltrap]";
            string FoolMessageBottom = $"bI bI!";
            foreach(BoneItemBehaviour bib in PlayerCharacter.instance.gameObject.GetComponentsInChildren<BoneItemBehaviour>()) {
                if (bib.item.name == "Torch") {
                    bib.confirmBoneUseCallback();
                    break;
                }
            }
            return (FoolMessageTop, FoolMessageBottom);
        }

        public static (string, string) FoolWideTrap() {
            SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
            string FoolMessageTop = $"yoo R A <#FFA500>wId \"<#FFA500>FOOL<#ffffff>!!\" [fooltrap]";
            string FoolMessageBottom = $"tuhuhuhuhuhuhuhnk";
            WideFox = true;
            PlayerCharacter.instance.Flinch(true);
            return (FoolMessageTop, FoolMessageBottom);
        }

        public static (string, string) FoolCRTTrap() {
            if (PlayerCharacter.Instanced && PlayerCharacter.instance.GetComponent<TechbowItemBehaviour>() != null) {
                SFX.PlayAudioClipAtFox(PlayerCharacter.instance.GetComponent<TechbowItemBehaviour>().sfx_fire);
            }
            string FoolMessageTop = $"yoo R A <#FF0000>kah%O<#00FF00>d rA \"<#0000FF>FOOL<#ffffff>!!\" [fooltrap]";
            string FoolMessageBottom = $"vintij!";
            CRTTrap = true;
            if (CRTMode.instance != null) { 
                CRTMode.instance.Enable();
            }
            PlayerCharacter.instance.Flinch(true);
            return (FoolMessageTop, FoolMessageBottom);
        }

        public static (string, string) FoolVintageTrap() {
            if (PlayerCharacter.Instanced && PlayerCharacter.instance.GetComponent<TechbowItemBehaviour>() != null) {
                SFX.PlayAudioClipAtFox(PlayerCharacter.instance.GetComponent<TechbowItemBehaviour>().sfx_fire);
            }
            string FoolMessageTop = $"yoo R A <#FF0000>kah%O<#00FF00>d rA \"<#0000FF>FOOL<#ffffff>!!\" [fooltrap]";
            string FoolMessageBottom = $"vintij!";
            VintageTrap = true;
            if (CRTMode.instance != null) {
                CRTMode.instance.Enable(true);
            }
            PlayerCharacter.instance.Flinch(true);
            return (FoolMessageTop, FoolMessageBottom);
        }

        public static (string, string) FoolSaturatedTrap() {
            SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
            string FoolMessageTop = $"yoo R A <#FFA500>sahJurAtid \"<#FFA500>FOOL<#ffffff>!!\" [fooltrap]";
            string FoolMessageBottom = $"wErd!";
            Texture2D texture = ModelSwaps.FindTexture("CRT", true);
            if (CameraController.instance.effectsCamera != null && CameraController.instance.effectsCamera.GetComponent<AmplifyColorEffect>() != null) {
                CameraController.instance.effectsCamera.GetComponent<AmplifyColorEffect>().LutTexture = texture;
                CameraController.instance.effectsCamera.GetComponent<AmplifyColorEffect>().enabled = true;
            } else {
                foreach (Camera camera in GameObject.FindObjectsOfType<Camera>()) {
                    if (camera.name.Contains("Camera 2") || camera.name.Contains("Post Processing")) {
                        if (camera.GetComponent<AmplifyColorEffect>() != null) {
                            camera.GetComponent<AmplifyColorEffect>().LutTexture = texture;
                            camera.GetComponent<AmplifyColorEffect>().enabled = true;
                        }
                    }
                }
            }
            PlayerCharacter.instance.Flinch(true);
            SaturatedTrap = true;
            return (FoolMessageTop, FoolMessageBottom);
        }


        public static (string, string) FoolDepletionTrap() {
            if (PlayerCharacter.Instanced && PlayerCharacter.instance.GetComponent<TechbowItemBehaviour>() != null) {
                SFX.PlayAudioClipAtFox(PlayerCharacter.instance.GetComponent<TechbowItemBehaviour>().sfx_fire);
            }
            string FoolMessageTop = $"yoo R A <#0000FF>mahnuhlehs \"<#FFA500>FOOL<#ffffff>!!\" [fooltrap]";
            string FoolMessageBottom = $"howz #aht for uh mahjik trik?";
            PlayerCharacter.SetMP(0);
            PlayerCharacter.instance.Flinch(true);
            return (FoolMessageTop, FoolMessageBottom);
        }

        public static (string, string) FoolDisarmTrap() {
            SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
            string FoolMessageTop = $"yoo R ahn <#FFA500>itehmlehs \"<#FFA500>FOOL<#ffffff>!!\" [fooltrap]";
            string FoolMessageBottom = $"I swAr I juhst hahd it uh sehkuhnd uhgO...";
            SaveFile.SetString($"Item on Button 0", "");
            SaveFile.SetString($"Item on Button 1", "");
            SaveFile.SetString($"Item on Button 2", "");
            Inventory.buttonAssignedItems = new ButtonAssignableItem[] { null, null, null };
            PlayerCharacter.instance.Flinch(true);
            return (FoolMessageTop, FoolMessageBottom);
        }

        public static (string, string) FoolFastTrap() {
            SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
            string FoolMessageTop = $"yoo R A <#FFA500>fahst \"<#FFA500>FOOL<#ffffff>!!\" [fooltrap]";
            string FoolMessageBottom = $"nyoom!";
            FastTrap = true;
            PlayerCharacter.instance.Flinch(true);
            return (FoolMessageTop, FoolMessageBottom);
        }

        public static void CheckFoolTrapSetting(string RewardId) {
            Reward Reward = Locations.RandomizedLocations[RewardId].Reward;
            if (Reward.Type == "MONEY") {
                if ((TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.NORMAL && Reward.Amount < 20)
                || (TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.DOUBLE && Reward.Amount <= 20)
                || (TunicRandomizer.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.ONSLAUGHT && Reward.Amount <= 30)) {
                    Reward.Name = "Fool";
                    Reward.Type = "FOOL";
                    Reward.Amount = 1;
                }
            }
        }

    }
}
