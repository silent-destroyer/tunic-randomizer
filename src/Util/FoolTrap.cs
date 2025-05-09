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
            Trip,  // for trap link
            Zoom,  // for trap link
            Bald,  // for trap link
        }

        public static Dictionary<TrapType, int> TrapWeights = new Dictionary<TrapType, int> {
            {TrapType.Ice, 30 },
            {TrapType.Fire, 20 },
            {TrapType.Bee, 15 },
            {TrapType.Tiny, 15 },
            {TrapType.Mirror, 10 },
            {TrapType.Deisometric, 10 },
            {TrapType.Zoom, 5},
        };

        public static Dictionary<TrapType, string> TrapTypeToName = new Dictionary<TrapType, string> {
            {TrapType.Ice, "Ice Trap" },
            {TrapType.Fire, "Fire Trap" },
            {TrapType.Bee, "Bee Trap" },
            {TrapType.Tiny, "Tiny Trap" },
            {TrapType.Mirror, "Screen Flip Trap" },
            {TrapType.Deisometric, "Deisometric Trap" },
            {TrapType.Trip, "Trip Trap" },
        };

        // for TrapLink, we convert names of similar traps to our trap types for receiving traps
        public static Dictionary<string, TrapType> TrapNameToType = new Dictionary<string, TrapType> {
            {"Ice Trap", TrapType.Ice },
            {"Freeze Trap", TrapType.Ice },
            {"Frozen Trap", TrapType.Ice },
            {"Stun Trap", TrapType.Ice },
            {"Paralyze Trap", TrapType.Ice },
            {"Chaos Control Trap", TrapType.Ice },

            {"Fire Trap", TrapType.Fire },
            {"Damage Trap", TrapType.Fire },
            {"Bomb", TrapType.Fire },  // Luigi's Mansion, yes it's just Bomb
            {"Posession Trap", TrapType.Fire },  // Luigi's Mansion, damage-based trap
            {"Nut Trap", TrapType.Fire },  // DKC, damage-based trap

            {"Bee Trap", TrapType.Bee },

            {"Tiny Trap", TrapType.Tiny },
            {"Poison Mushroom", TrapType.Tiny },  // Luigi's Mansion, makes player smaller

            {"Screen Flip Trap", TrapType.Mirror },
            {"Mirror Trap", TrapType.Mirror },
            {"Reverse Trap", TrapType.Mirror },
            {"Reversal Trap", TrapType.Mirror },

            {"Deisometric Trap", TrapType.Deisometric },
            {"Confuse Trap", TrapType.Deisometric },
            {"Confusion Trap", TrapType.Deisometric },
            {"Fuzzy Trap", TrapType.Deisometric },
            {"Confound Trap", TrapType.Deisometric },

            {"Bonk Trap", TrapType.Trip },
            {"Banana Trap", TrapType.Trip },
            {"Spring Trap", TrapType.Trip },

            {"Zoom Trap", TrapType.Zoom },  // Celeste, zooms camera in

            {"Bald Trap", TrapType.Bald },  // Celeste, bald
        };

        public static bool StungByBee = false;
        public static bool TinierFox = false;
        public static bool BaldFox = false;  // for trap link
        public static bool ZoomedCamera = false;  // for trap link

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
                case TrapType.Bald:
                    (FoolMessageTop, FoolMessageBottom) = FoolBaldTrap();
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
            foreach (TrapType trapType in TrapWeights.Keys) {
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
                weightedTrapList.AddRange(Enumerable.Repeat(trapType, TrapWeights[trapType]));
            }
            TrapType trapSelected = weightedTrapList[Random.Next(weightedTrapList.Count)];
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

        public static (string, string) FoolBaldTrap() {
            SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
            string FoolMessageTop = $"yoo R A <#FFA500>bawld \"<#FFA500>FOOL<#ffffff>!!\" [fooltrap]";
            string FoolMessageBottom = $"giv it bahk!";
            BaldFox = true;
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
