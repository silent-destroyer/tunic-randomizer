using System;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TunicRandomizer {

    public class RandomizerSettings {

        public ConnectionSettings ConnectionSettings { get; set; }

        public enum RandomizerType {
            SINGLEPLAYER,
            ARCHIPELAGO
        }

        public RandomizerType Mode {
            get;
            set;
        }

        // Single Player Settings
        private const int HEXAGON_QUEST = 1;
        private const int KEYS_BEHIND_BOSSES = 2;
        private const int START_WITH_SWORD = 4;
        private const int SWORD_PROGRESSION = 8;
        private const int SHUFFLE_ABILITIES = 16;
        private const int ENTRANCE_RANDO = 32;
        private const int ER_SHOPS = 64;
        private const int LANTERNLESS = 128;
        private const int MASKLESS = 256;
        private const int MYSTERY_SEED = 512;
        public GameModes GameMode {
            get;
            set;
        }

        public bool KeysBehindBosses {
            get;
            set;
        }

        public bool StartWithSwordEnabled {
            get;
            set;
        }

        public bool SwordProgressionEnabled {
            get;
            set;
        }

        public bool ShuffleAbilities {
            get;
            set;
        }

        public bool EntranceRandoEnabled {
            get;
            set;
        }

        public bool ERFixedShop {
            get;
            set;
        }

        public bool Lanternless {
            get;
            set;
        }
        public bool Maskless {
            get;
            set;
        }

        public FixedLaurelsType FixedLaurelsOption {
            get;
            set;
        }

        public FoolTrapOption FoolTrapIntensity {
            get;
            set;
        }

        public bool MysterySeed {
            get;
            set;
        }

        public int HexagonQuestGoal {
            get;
            set;
        }

        public int HexagonQuestExtraPercentage {
            get;
            set;
        }

        // Archipelago Settings
        public bool DeathLinkEnabled {
            get;
            set;
        }

        public bool CollectReflectsInWorld {
            get;
            set;
        }

        public bool SendHintsToServer {
            get;
            set;
        }

        // Hint Settings
        private const int PATH_OF_HERO = 1;
        private const int GHOST_FOXES = 2;
        private const int SHOW_ITEMS = 4;
        private const int CHESTS_MATCH = 8;
        private const int USE_TRUNIC = 16;
        private const int SPOILER_LOG = 32;
        public bool HeroPathHintsEnabled {
            get;
            set;
        }

        public bool GhostFoxHintsEnabled {
            get;
            set;
        }

        public bool ShowItemsEnabled {
            get;
            set;
        }

        public bool ChestsMatchContentsEnabled {
            get;
            set;
        }

        public bool UseTrunicTranslations {
            get;
            set;
        }

        public bool CreateSpoilerLog {
            get;
            set;
        }

        // Gameplay Settings
        private const int EASY_HEIR = 1;
        private const int CLEAR_BUSHES = 2;
        private const int ENABLE_CHECKPOINTS = 4;
        private const int CHEAPER_SHOP_ITEMS = 8;
        private const int BONUS_UPGRADES = 16;
        private const int CHEST_INTERRUPTION = 32;
        private const int SKIP_ITEM_POPUPS = 64;
        private const int FASTER_UPGRADES = 128;        
        private const int CAMERA_FLIP = 256;
        private const int MORE_SKULLS = 512;
        private const int ARACHNOPHOBIA_MODE = 1024;
        public bool HeirAssistModeEnabled {
            get;
            set;
        }

        public bool ClearEarlyBushes {
            get;
            set;
        }

        public bool EnableAllCheckpoints {
            get;
            set;
        }

        public bool CheaperShopItemsEnabled {
            get;
            set;
        }

        public bool BonusStatUpgradesEnabled {
            get;
            set;
        }

        public bool DisableChestInterruption {
            get;
            set;
        }

        public bool SkipItemAnimations {
            get;
            set;
        }

        public bool FasterUpgrades {
            get;
            set;
        }


        public bool CameraFlip {
            get;
            set;
        }

        public bool MoreSkulls {
            get;
            set;
        }

        public bool ArachnophobiaMode {
            get;
            set;
        }

        // Enemy Randomization Settings
        private const int ENEMY_RANDOMIZER = 1;
        private const int EXTRA_ENEMIES = 2;
        private const int BALANCED_ENEMIES = 4;
        private const int SEEDED_ENEMIES = 8;
        public bool EnemyRandomizerEnabled {
            get;
            set;
        }

        public bool ExtraEnemiesEnabled {
            get;
            set;
        }

        public bool BalancedEnemies {
            get;
            set;
        }

        public bool SeededEnemies {
            get;
            set;
        }

        // Race Mode Settings
        private const int RACE_MODE = 1;
        private const int HEIR_ICEBOLT = 2;
        private const int DISTANT_BELLS = 4;
        private const int ICE_GRAPPLING = 8;
        private const int LADDER_STORAGE = 16;
        private const int UPGRADE_STEALING = 32;
        public bool RaceMode {
            get;
            set;
        }

        public bool DisableIceboltInHeirFight {
            get;
            set;
        }

        public bool DisableDistantBellShots {
            get;
            set;
        }

        public bool DisableIceGrappling {
            get;
            set;
        }

        public bool DisableLadderStorage {
            get;
            set;
        }

        public bool DisableUpgradeStealing {
            get;
            set;
        }

        // Fox Settings
        public bool RandomFoxColorsEnabled {
            get;
            set;
        }

        public bool RealestAlwaysOn {
            get;
            set;
        }

        public bool UseCustomTexture {
            get;
            set;
        }

        public enum GameModes {
            RANDOMIZER,
            HEXAGONQUEST,
            VANILLA
        }

        public enum FoolTrapOption {
            NONE,
            NORMAL,
            DOUBLE,
            ONSLAUGHT,
        }

        public enum FixedLaurelsType {
            RANDOM,
            SIXCOINS,
            TENCOINS,
            TENFAIRIES,
        }

        public RandomizerSettings() {

            ConnectionSettings = new ConnectionSettings();
            Mode = RandomizerType.SINGLEPLAYER;

            // Single Player
            GameMode = GameModes.RANDOMIZER;
            KeysBehindBosses = false;
            SwordProgressionEnabled = true;
            StartWithSwordEnabled = false;
            ShuffleAbilities = false;
            EntranceRandoEnabled = false;
            ERFixedShop = false;
            HexagonQuestGoal = 20;
            HexagonQuestExtraPercentage = 50;
            FixedLaurelsOption = FixedLaurelsType.RANDOM;
            FoolTrapIntensity = FoolTrapOption.NORMAL;
            Lanternless = false;
            Maskless = false;
            MysterySeed = false;

            // Archipelago 
            DeathLinkEnabled = false;
            CollectReflectsInWorld = false;
            SkipItemAnimations = false;
            SendHintsToServer = false;

            // Hints
            HeroPathHintsEnabled = true;
            GhostFoxHintsEnabled = true;
            ShowItemsEnabled = true;
            ChestsMatchContentsEnabled = true;
            UseTrunicTranslations = false;
            CreateSpoilerLog = true;

            // General
            HeirAssistModeEnabled = false;
            ClearEarlyBushes = false;
            EnableAllCheckpoints = true;
            CheaperShopItemsEnabled = true;
            BonusStatUpgradesEnabled = true;
            DisableChestInterruption = false;
            FasterUpgrades = false;

            // Other
            CameraFlip = false;
            MoreSkulls = false;
            ArachnophobiaMode = false;

            // Enemy Randomizer
            EnemyRandomizerEnabled = false;
            BalancedEnemies = true;
            SeededEnemies = true;
            ExtraEnemiesEnabled = false;

            // Race Settings
            RaceMode = false;
            DisableIceboltInHeirFight = false;
            DisableDistantBellShots = false;
            DisableIceGrappling = false;
            DisableLadderStorage = false;
            DisableUpgradeStealing = false;

            // Fox Customization
            RandomFoxColorsEnabled = true;
            RealestAlwaysOn = false;
            UseCustomTexture = false;
        }

        public RandomizerSettings(bool hintsEnabled, bool randomFoxColorsEnabled) {
            HeroPathHintsEnabled = hintsEnabled;
            RandomFoxColorsEnabled = randomFoxColorsEnabled;
        }

        public RandomizerSettings(bool hintsEnabled, bool randomFoxColorsEnabled, bool heirAssistEnaled) {
            HeroPathHintsEnabled = hintsEnabled;
            RandomFoxColorsEnabled = randomFoxColorsEnabled;
            HeirAssistModeEnabled = heirAssistEnaled;
        }

        public string GetSettingsString() {
            string EncodedSettings = Convert.ToBase64String(Encoding.UTF8.GetBytes(
                $"{HexagonQuestGoal}" +
                $":{HexagonQuestExtraPercentage}" +
                $":{(int)FoolTrapIntensity}" +
                $":{(int)FixedLaurelsOption}" +
                $":{Convert.ToInt32(sToB(logicSettings()), 2)}" +
                $":{Convert.ToInt32(sToB(generalSettings()), 2)}" +
                $":{Convert.ToInt32(sToB(hintSettings()), 2)}" +
                $":{Convert.ToInt32(sToB(enemySettings()), 2)}" +
                $":{Convert.ToInt32(sToB(raceSettings()), 2)}"));
            string seed;
            if (SceneManager.GetActiveScene().name != "TitleScreen") {
                seed = SaveFile.GetInt("seed").ToString();
            } else {
                seed = QuickSettings.CustomSeed == "" ? new System.Random().Next().ToString() : QuickSettings.CustomSeed;
                QuickSettings.CustomSeed = seed;
            }
            string SettingsString = $"tunc:{PluginInfo.VERSION}:{seed}:{EncodedSettings}";
            GUIUtility.systemCopyBuffer = SettingsString;
            return SettingsString;
        }

        public void ParseSettingsString(string s) {
            try {
                string[] split = s.Split(':');

                string tunc = split[0];
                string version = split[1];
                QuickSettings.CustomSeed = split[2];

                string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(split[3]));
                string[] decodedSplit = decoded.Split(':');

                HexagonQuestGoal = int.Parse(decodedSplit[0]);
                HexagonQuestExtraPercentage = int.Parse(decodedSplit[1]);
                FoolTrapIntensity = (FoolTrapOption)int.Parse(decodedSplit[2]);
                FixedLaurelsOption = (FixedLaurelsType)int.Parse(decodedSplit[3]);

                int logic = int.Parse(decodedSplit[4]);
                GameMode = eval(logic, HEXAGON_QUEST) ? GameModes.HEXAGONQUEST : GameModes.RANDOMIZER;
                KeysBehindBosses = eval(logic, KEYS_BEHIND_BOSSES);
                StartWithSwordEnabled = eval(logic, START_WITH_SWORD);
                SwordProgressionEnabled = eval(logic, SWORD_PROGRESSION);
                ShuffleAbilities = eval(logic, SHUFFLE_ABILITIES);
                EntranceRandoEnabled = eval(logic, ENTRANCE_RANDO);
                ERFixedShop = eval(logic, ER_SHOPS);
                Lanternless = eval(logic, LANTERNLESS);
                Maskless = eval(logic, MASKLESS);
                MysterySeed = eval(logic, MYSTERY_SEED);

                int general = int.Parse(decodedSplit[5]);
                HeirAssistModeEnabled = eval(general, EASY_HEIR);
                ClearEarlyBushes = eval(general, CLEAR_BUSHES);
                EnableAllCheckpoints = eval(general, ENABLE_CHECKPOINTS);
                CheaperShopItemsEnabled = eval(general, CHEAPER_SHOP_ITEMS);
                BonusStatUpgradesEnabled = eval(general, BONUS_UPGRADES);
                DisableChestInterruption = eval(general, CHEST_INTERRUPTION);
                SkipItemAnimations = eval(general, SKIP_ITEM_POPUPS);
                FasterUpgrades = eval(general, FASTER_UPGRADES);

                CameraFlip = eval(general, CAMERA_FLIP);
                MoreSkulls = eval(general, MORE_SKULLS);
                ArachnophobiaMode = eval(general, ARACHNOPHOBIA_MODE);

                int hints = int.Parse(decodedSplit[6]);
                HeroPathHintsEnabled = eval(hints, PATH_OF_HERO);
                GhostFoxHintsEnabled = eval(hints, GHOST_FOXES);
                ShowItemsEnabled = eval(hints, SHOW_ITEMS);
                ChestsMatchContentsEnabled = eval(hints, CHESTS_MATCH);
                UseTrunicTranslations = eval(hints, USE_TRUNIC);
                CreateSpoilerLog = eval(hints, SPOILER_LOG);

                int enemies = int.Parse(decodedSplit[7]);
                EnemyRandomizerEnabled = eval(enemies, ENEMY_RANDOMIZER);
                ExtraEnemiesEnabled = eval(enemies, EXTRA_ENEMIES);
                BalancedEnemies = eval(enemies, BALANCED_ENEMIES);
                SeededEnemies = eval(enemies, SEEDED_ENEMIES);

                int race = int.Parse(decodedSplit[8]);
                RaceMode = eval(race, RACE_MODE);
                DisableIceboltInHeirFight = eval(race, HEIR_ICEBOLT);
                DisableDistantBellShots = eval(race, DISTANT_BELLS);
                DisableIceGrappling = eval(race, ICE_GRAPPLING);
                DisableLadderStorage = eval(race, LADDER_STORAGE);
                DisableUpgradeStealing = eval(race, UPGRADE_STEALING);

                OptionsGUIPatches.SaveSettings();
            } catch (Exception e) {
                TunicRandomizer.Logger.LogInfo("Error parsing settings string!");
            }
        }

        public string sToB(bool[] settings) {
            return string.Join("", settings.Reverse().Select(x => x ? "1" : "0").ToList());
        }

        private bool eval(int a, int b) {
            return (a & b) != 0;
        }

        public bool[] logicSettings() {
            if (SceneManager.GetActiveScene().name == "TitleScreen") {
                return new bool[] {
                    GameMode == GameModes.HEXAGONQUEST,
                    KeysBehindBosses, StartWithSwordEnabled, SwordProgressionEnabled,
                    ShuffleAbilities, EntranceRandoEnabled, ERFixedShop,
                    Lanternless, Maskless, MysterySeed,
                };
            } else {
                return new bool[] { 
                    SaveFile.GetInt(SaveFlags.HexagonQuestEnabled) == 1, SaveFile.GetInt(SaveFlags.KeysBehindBosses) == 1,
                    SaveFile.GetInt(SaveFlags.SwordProgressionEnabled) == 1, SaveFile.GetInt("randomizer started with sword") == 1,
                    SaveFile.GetInt(SaveFlags.AbilityShuffle) == 1, SaveFile.GetInt(SaveFlags.EntranceRando) == 1,
                    SaveFile.GetInt("randomizer ER fixed shop") == 1, SaveFile.GetInt(SaveFlags.LanternlessLogic) == 1,
                    SaveFile.GetInt(SaveFlags.MasklessLogic) == 1, SaveFile.GetInt("randomizer mystery seed") == 1
                };
            }
        }

        public bool[] generalSettings() {
            return new bool[] { HeirAssistModeEnabled, ClearEarlyBushes, EnableAllCheckpoints, CheaperShopItemsEnabled, 
                BonusStatUpgradesEnabled, DisableChestInterruption, SkipItemAnimations, FasterUpgrades, CameraFlip, MoreSkulls, ArachnophobiaMode, };
        }

        public bool[] hintSettings() {
            return new bool[] { HeroPathHintsEnabled, GhostFoxHintsEnabled, ShowItemsEnabled, ChestsMatchContentsEnabled, 
                UseTrunicTranslations, CreateSpoilerLog, };
        }

        public bool[] enemySettings() {
            return new bool[] { EnemyRandomizerEnabled, ExtraEnemiesEnabled, BalancedEnemies, SeededEnemies };
        }

        public bool[] raceSettings() {
            return new bool[] { RaceMode, DisableIceboltInHeirFight, DisableDistantBellShots, DisableIceGrappling, 
                DisableLadderStorage, DisableUpgradeStealing, };
        }

        public static void getSettings() {
            TunicRandomizer.Settings.GetSettingsString();
        }
    }
}
