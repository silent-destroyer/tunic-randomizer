using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {

    public class RandomizerSettings {

        public ConnectionSettings ConnectionSettings { get; set; }

        public MysterySeedWeights MysterySeedWeights { get; set; }

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
        private const int SHUFFLE_LADDERS = 1024;
        private const int GRASS_RANDOMIZER = 2048;
        private const int RANDOMIZE_HEX_QUEST = 4096;
        private const int ER_DIRECTION_PAIRS = 8192;
        private const int ER_DECOUPLED = 16384;
        private const int HEXAGON_QUEST_ABILITY_PAGES = 32768;
        private const int BREAKABLE_SHUFFLE = 65536;
        private const int FUSE_SHUFFLE = 131072;
        private const int BELL_SHUFFLE = 262144;
        private const int LAURELS_ZIPS = 524288;
        private const int LS_WITHOUT_ITEMS = 1048576;
        private const int FOX_PRINCE = 2097152;

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

        public bool PortalDirectionPairs {
            get;
            set;
        }

        public bool DecoupledER {
            get;
            set;
        }

        public bool FoxPrinceEnabled {
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

        public bool LaurelsZips {
            get;
            set;
        }
        public IceGrapplingType IceGrappling {
            get;
            set;
        }
        public LadderStorageType LadderStorage {
            get;
            set;
        }
        public bool LadderStorageWithoutItems {
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

        public bool HexQuestAbilitiesUnlockedByPages {
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

        public bool RandomizeHexQuest {
            get;
            set;
        }

        public enum HexQuestValue { 
            RANDOM,
            LOW,
            MEDIUM,
            HIGH,
        }

        public HexQuestValue HexagonQuestRandomGoal {
            get;
            set;
        }

        public HexQuestValue HexagonQuestRandomExtras {
            get;
            set;
        }

        public bool ShuffleLadders {
            get;
            set;
        }

        public bool GrassRandomizer {
            get;
            set;
        }

        public bool BreakableShuffle {
            get;
            set;
        }

        public bool FuseShuffle {
            get;
            set;
        }

        public bool BellShuffle {
            get;
            set;
        }

        // Archipelago Settings
        public bool DeathLinkEnabled {
            get;
            set;
        }

        public bool TrapLinkEnabled {
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

        public bool ShowSlotSettings {
            get;
            set;
        }

        public DeathLinkType DeathLinkEffect {
            get;
            set;
        }

        public enum DeathLinkType {
            DEATH,
            FOOLTRAP,
        }

        // Hint Settings
        private const int PATH_OF_HERO = 1;
        private const int GHOST_FOXES = 2;
        private const int SHOW_ITEMS = 4;
        private const int CHESTS_MATCH = 8;
        private const int USE_TRUNIC = 16;
        private const int SPOILER_LOG = 32;
        private const int SEEKING_SPELL_LOGIC = 64;
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

        public bool SeekingSpellLogic {
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

        public bool ShowRecentItems {
            get;
            set;
        }

        // Other settings
        private const int CAMERA_FLIP = 1;
        private const int MORE_SKULLS = 2;
        private const int ARACHNOPHOBIA_MODE = 4;
        private const int HOLY_CROSS_VIEWER = 8;
        private const int MUSIC_SHUFFLE = 16;
        private const int SEEDED_MUSIC = 32;
        private const int BIGGER_HEAD_MODE = 64;
        private const int TINIER_FOX_MODE = 128;
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

        public bool HolyCrossVisualizer {
            get;
            set;
        }

        public bool MusicShuffle {
            get;
            set;
        }

        public bool SeededMusic {
            get;
            set;
        }

        public Dictionary<string, bool> MusicToggles {
            get;
            set;
        }

        public bool BiggerHeadMode {
            get;
            set;
        }

        public bool TinierFoxMode {
            get;
            set;
        }

        // Enemy Randomization Settings
        private const int ENEMY_RANDOMIZER = 1;
        private const int EXTRA_ENEMIES = 2;
        private const int BALANCED_ENEMIES = 4;
        private const int SEEDED_ENEMIES = 8;
        private const int EXCLUDE_ENEMIES = 16;
        private const int LIMIT_BOSS_SPAWNS = 32;
        private const int OOPS_ALL_ENEMY = 64;
        private const int RANDOM_ENEMY_SIZES = 128;
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

        public bool LimitBossSpawns {
            get;
            set;
        }

        public bool OopsAllEnemy {
            get;
            set;
        }

        public bool RandomEnemySizes {
            get;
            set;
        }

        public bool UseEnemyToggles {
            get;
            set;
        }

        public Dictionary<string, bool> EnemyToggles {
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

        // Misc/Debug Settings
        public bool ShowPlayerPosition {
            get;
            set;
        }

        public bool DeathplanePatch {
            get;
            set;
        }

        public bool OptionTooltips {
            get;
            set;
        }

        public bool RunInBackground {
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

        public enum IceGrapplingType {
            OFF,
            EASY,
            MEDIUM,
            HARD,
        }
        public enum LadderStorageType {
            OFF,
            EASY,
            MEDIUM,
            HARD,
        }

        public RandomizerSettings() {

            ConnectionSettings = new ConnectionSettings();
            MysterySeedWeights = new MysterySeedWeights();
            Mode = RandomizerType.SINGLEPLAYER;

            // Single Player
            GameMode = GameModes.RANDOMIZER;
            KeysBehindBosses = false;
            SwordProgressionEnabled = true;
            StartWithSwordEnabled = false;
            ShuffleAbilities = false;
            EntranceRandoEnabled = false;
            ERFixedShop = false;
            PortalDirectionPairs = false;
            DecoupledER = false;
            FoxPrinceEnabled = false;
            HexagonQuestGoal = 20;
            HexagonQuestExtraPercentage = 50;
            FixedLaurelsOption = FixedLaurelsType.RANDOM;
            FoolTrapIntensity = FoolTrapOption.NORMAL;
            Lanternless = false;
            Maskless = false;
            MysterySeed = false;
            ShuffleLadders = false;
            GrassRandomizer = false;
            BreakableShuffle = false;
            FuseShuffle = false;
            BellShuffle = false;
            RandomizeHexQuest = false;
            HexQuestAbilitiesUnlockedByPages = false;
            HexagonQuestRandomGoal = HexQuestValue.RANDOM;
            HexagonQuestRandomExtras = HexQuestValue.RANDOM;

            // Trick & Glitch Logic
            LaurelsZips = false;
            IceGrappling = IceGrapplingType.OFF;
            LadderStorage = LadderStorageType.OFF;
            LadderStorageWithoutItems = false;

            // Archipelago 
            DeathLinkEnabled = false;
            TrapLinkEnabled = false;
            CollectReflectsInWorld = false;
            SkipItemAnimations = false;
            SendHintsToServer = false;
            ShowSlotSettings = false;
            DeathLinkEffect = DeathLinkType.DEATH;

            // Hints
            HeroPathHintsEnabled = true;
            GhostFoxHintsEnabled = true;
            ShowItemsEnabled = true;
            ChestsMatchContentsEnabled = true;
            UseTrunicTranslations = false;
            CreateSpoilerLog = true;
            SeekingSpellLogic = false;

            // General
            HeirAssistModeEnabled = false;
            ClearEarlyBushes = true;
            EnableAllCheckpoints = true;
            CheaperShopItemsEnabled = true;
            BonusStatUpgradesEnabled = true;
            DisableChestInterruption = false;
            FasterUpgrades = false;
            ShowRecentItems = true;

            // Other
            CameraFlip = false;
            MoreSkulls = false;
            ArachnophobiaMode = false;
            HolyCrossVisualizer = false;
            BiggerHeadMode = false;
            TinierFoxMode = false;
            MusicShuffle = false;
            SeededMusic = false;
            MusicToggles = new Dictionary<string, bool>();
            foreach(string track in MusicShuffler.Tracks.Keys) {
                MusicToggles.Add(track, true);
            }

            // Enemy Randomizer
            EnemyRandomizerEnabled = false;
            BalancedEnemies = true;
            SeededEnemies = true;
            ExtraEnemiesEnabled = false;
            LimitBossSpawns = true;
            UseEnemyToggles = false;
            OopsAllEnemy = false;
            RandomEnemySizes = false;
            EnemyToggles = new Dictionary<string, bool>();
            foreach(string enemy in EnemyRandomizer.EnemyToggleOptionNames.Values) {
                EnemyToggles.Add(enemy, true);
            }

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

            // Misc/Debug
            ShowPlayerPosition = false;
            DeathplanePatch = true;
            OptionTooltips = true;
            RunInBackground = true;
        }

        public string GetSettingsString() {
            bool getFromSave = SceneManager.GetActiveScene().name != "TitleScreen";
            string EncodedSettings = Convert.ToBase64String(Encoding.UTF8.GetBytes(
                $"{(getFromSave ? SaveFile.GetInt(SaveFlags.HexagonQuestGoal) : HexagonQuestGoal)}" +
                $":{(getFromSave ? SaveFile.GetInt(SaveFlags.HexagonQuestExtras) : HexagonQuestExtraPercentage)}" +
                $":{(int)FoolTrapIntensity}" +
                $":{(getFromSave ? SaveFile.GetInt(SaveFlags.LaurelsLocation) : (int)FixedLaurelsOption)}" +
                $":{(getFromSave ? SaveFile.GetInt(SaveFlags.HexagonQuestRandomGoal) : (int)HexagonQuestRandomGoal)}" +
                $":{(getFromSave ? SaveFile.GetInt(SaveFlags.HexagonQuestRandomExtras) : (int)HexagonQuestRandomExtras)}" +
                $":{(getFromSave ? SaveFile.GetInt(SaveFlags.IceGrapplingDifficulty) : (int)IceGrappling)}" +
                $":{(getFromSave ? SaveFile.GetInt(SaveFlags.LadderStorageDifficulty) : (int)LadderStorage)}" +
                $":{Convert.ToInt32(sToB(logicSettings()), 2)}" +
                $":{Convert.ToInt32(sToB(generalSettings()), 2)}" +
                $":{Convert.ToInt32(sToB(hintSettings()), 2)}" +
                $":{Convert.ToInt32(sToB(enemySettings()), 2)}" +
                $":{Convert.ToInt32(sToB(raceSettings()), 2)}" +
                $":{Convert.ToInt32(sToB(otherSettings()), 2)}" +
                $":{ConvertEnemyToggles()}" +
                $":{(MysterySeed ? getFromSave ? SaveFile.GetString("randomizer mystery seed weights") : MysterySeedWeights.ToSettingsString() : "")}"));
            string seed;
            if (getFromSave) {
                seed = SaveFile.GetInt("seed").ToString();
            } else {
                seed = QuickSettingsRedux.CustomSeed == "" ? new System.Random().Next().ToString() : QuickSettingsRedux.CustomSeed;
                QuickSettingsRedux.CustomSeed = seed;
            }
            string SettingsString = $"tunc:{PluginInfo.VERSION}:{seed}:{EncodedSettings}";
            return SettingsString;
        }
        public void ParseSettingsString(string s) {
            try {
                string[] split = s.Split(':');

                string tunc = split[0];
                string version = split[1];
                if (version != PluginInfo.VERSION) {
                    Notifications.Show($"\"Could not import settings string!\"", $"\"Settings are from a different version.\"");
                    return;
                }
                QuickSettingsRedux.CustomSeed = split[2];

                string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(split[3]));
                string[] decodedSplit = decoded.Split(':');
                HexagonQuestGoal = int.Parse(decodedSplit[0]);
                HexagonQuestExtraPercentage = int.Parse(decodedSplit[1]);
                FoolTrapIntensity = (FoolTrapOption)int.Parse(decodedSplit[2]);
                FixedLaurelsOption = (FixedLaurelsType)int.Parse(decodedSplit[3]);
                HexagonQuestRandomGoal = (HexQuestValue)int.Parse(decodedSplit[4]);
                HexagonQuestRandomExtras = (HexQuestValue)int.Parse(decodedSplit[5]);
                IceGrappling = (IceGrapplingType)int.Parse(decodedSplit[6]);
                LadderStorage = (LadderStorageType)int.Parse(decodedSplit[7]);

                int logic = int.Parse(decodedSplit[8]);
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
                ShuffleLadders = eval(logic, SHUFFLE_LADDERS);
                GrassRandomizer = eval(logic, GRASS_RANDOMIZER);
                RandomizeHexQuest = eval(logic, RANDOMIZE_HEX_QUEST);
                PortalDirectionPairs = eval(logic, ER_DIRECTION_PAIRS);
                DecoupledER = eval(logic, ER_DECOUPLED);
                HexQuestAbilitiesUnlockedByPages = eval(logic, HEXAGON_QUEST_ABILITY_PAGES);
                BreakableShuffle = eval(logic, BREAKABLE_SHUFFLE);
                FuseShuffle = eval(logic, FUSE_SHUFFLE);
                BellShuffle = eval(logic, BELL_SHUFFLE);
                LaurelsZips = eval(logic, LAURELS_ZIPS);
                LadderStorageWithoutItems = eval(logic, LS_WITHOUT_ITEMS);
                FoxPrinceEnabled = eval(logic, FOX_PRINCE);

                int general = int.Parse(decodedSplit[9]);
                HeirAssistModeEnabled = eval(general, EASY_HEIR);
                ClearEarlyBushes = eval(general, CLEAR_BUSHES);
                EnableAllCheckpoints = eval(general, ENABLE_CHECKPOINTS);
                CheaperShopItemsEnabled = eval(general, CHEAPER_SHOP_ITEMS);
                BonusStatUpgradesEnabled = eval(general, BONUS_UPGRADES);
                DisableChestInterruption = eval(general, CHEST_INTERRUPTION);
                SkipItemAnimations = eval(general, SKIP_ITEM_POPUPS);
                FasterUpgrades = eval(general, FASTER_UPGRADES);

                int hints = int.Parse(decodedSplit[10]);
                HeroPathHintsEnabled = eval(hints, PATH_OF_HERO);
                GhostFoxHintsEnabled = eval(hints, GHOST_FOXES);
                ShowItemsEnabled = eval(hints, SHOW_ITEMS);
                ChestsMatchContentsEnabled = eval(hints, CHESTS_MATCH);
                UseTrunicTranslations = eval(hints, USE_TRUNIC);
                CreateSpoilerLog = eval(hints, SPOILER_LOG);

                int enemies = int.Parse(decodedSplit[11]);
                EnemyRandomizerEnabled = eval(enemies, ENEMY_RANDOMIZER);
                ExtraEnemiesEnabled = eval(enemies, EXTRA_ENEMIES);
                BalancedEnemies = eval(enemies, BALANCED_ENEMIES);
                SeededEnemies = eval(enemies, SEEDED_ENEMIES);
                UseEnemyToggles = eval(enemies, EXCLUDE_ENEMIES);
                LimitBossSpawns = eval(enemies, LIMIT_BOSS_SPAWNS);
                OopsAllEnemy = eval(enemies, OOPS_ALL_ENEMY);
                RandomEnemySizes = eval(enemies, RANDOM_ENEMY_SIZES);

                int race = int.Parse(decodedSplit[12]);
                RaceMode = eval(race, RACE_MODE);
                DisableIceboltInHeirFight = eval(race, HEIR_ICEBOLT);
                DisableDistantBellShots = eval(race, DISTANT_BELLS);
                DisableIceGrappling = eval(race, ICE_GRAPPLING);
                DisableLadderStorage = eval(race, LADDER_STORAGE);
                DisableUpgradeStealing = eval(race, UPGRADE_STEALING);

                if (decodedSplit.Length >= 12) {
                    int other = int.Parse(decodedSplit[13]);
                    CameraFlip = eval(other, CAMERA_FLIP);
                    MoreSkulls = eval(other, MORE_SKULLS);
                    ArachnophobiaMode = eval(other, ARACHNOPHOBIA_MODE);
                    HolyCrossVisualizer = eval(other, HOLY_CROSS_VIEWER);
                    BiggerHeadMode = eval(other, BIGGER_HEAD_MODE);
                    TinierFoxMode = eval(other, TINIER_FOX_MODE);

                    ParseEnemyToggles(decodedSplit[14]);
                }
                if (MysterySeed) {
                    MysterySeedWeights.FromSettingsString(decodedSplit[15]);
                }

                RandomizerSettings.SaveSettings();
            } catch (Exception e) {
                TunicLogger.LogInfo("Error parsing settings string!" + e.Message + " " + e.Source + " " + e.StackTrace);
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
                    Lanternless, Maskless, MysterySeed, ShuffleLadders,
                    GrassRandomizer, RandomizeHexQuest,
                    PortalDirectionPairs, DecoupledER, HexQuestAbilitiesUnlockedByPages,
                    BreakableShuffle, FuseShuffle, BellShuffle, LaurelsZips, LadderStorageWithoutItems,
                    FoxPrinceEnabled,
                };
            } else {
                return new bool[] { 
                    GetBool(SaveFlags.HexagonQuestEnabled), GetBool(SaveFlags.KeysBehindBosses),
                    GetBool(SaveFlags.StartWithSword), GetBool(SaveFlags.SwordProgressionEnabled),
                    GetBool(SaveFlags.AbilityShuffle), GetBool(SaveFlags.EntranceRando),
                    GetBool(SaveFlags.ERFixedShop), GetBool(SaveFlags.LanternlessLogic),
                    GetBool(SaveFlags.MasklessLogic), GetBool(MysterySeedEnabled),
                    GetBool(SaveFlags.LadderRandoEnabled), GetBool(SaveFlags.GrassRandoEnabled),
                    GetBool(SaveFlags.HexagonQuestRandomizedValues), GetBool(SaveFlags.PortalDirectionPairs),
                    GetBool(SaveFlags.Decoupled), GetBool(SaveFlags.HexagonQuestPageAbilities),
                    GetBool(SaveFlags.BreakableShuffleEnabled), GetBool(SaveFlags.FuseShuffleEnabled),
                    GetBool(SaveFlags.BellShuffleEnabled), GetBool(SaveFlags.LaurelsZips),
                    GetBool(SaveFlags.LadderStorageWithoutItems), GetBool(SaveFlags.FoxPrinceEnabled),
                };
            }
        }

        public bool[] generalSettings() {
            return new bool[] { HeirAssistModeEnabled, ClearEarlyBushes, EnableAllCheckpoints, CheaperShopItemsEnabled, 
                BonusStatUpgradesEnabled, DisableChestInterruption, SkipItemAnimations, FasterUpgrades };
        }

        public bool[] otherSettings() {
            return new bool[] { CameraFlip, MoreSkulls,
                ArachnophobiaMode, HolyCrossVisualizer, MusicShuffle, SeededMusic, BiggerHeadMode, TinierFoxMode };
        }

        public bool[] hintSettings() {
            return new bool[] { HeroPathHintsEnabled, GhostFoxHintsEnabled, ShowItemsEnabled, ChestsMatchContentsEnabled, 
                UseTrunicTranslations, CreateSpoilerLog, SeekingSpellLogic };
        }

        public bool[] enemySettings() {
            return new bool[] { EnemyRandomizerEnabled, ExtraEnemiesEnabled, BalancedEnemies, SeededEnemies, UseEnemyToggles, LimitBossSpawns, OopsAllEnemy, RandomEnemySizes };
        }

        private string ConvertEnemyToggles() {
            string bools = sToB(EnemyToggles.Values.ToArray());
            string ret = "";
            for (int i = 0; i < bools.Length; i += 64) {
                ret += Convert.ToInt64(bools.Substring(i, i + 64 > bools.Length ? bools.Length - i : 64), 2) + ";";
            }
            return ret.TrimEnd(';');
        }

        private void ParseEnemyToggles(string Toggles) {
            string enemyFlags = "";
            foreach (string x in Toggles.Split(';')) {
                enemyFlags += Convert.ToString(Int64.Parse(x), 2).PadLeft(enemyFlags.Length + 64 > EnemyToggles.Count ? EnemyToggles.Count - 64 : 64, '0');
            }
            enemyFlags = new string(enemyFlags.ToCharArray().Reverse().ToArray());

            List<string> enemyNames = EnemyToggles.Keys.ToList();

            for (int i = 0; i < enemyFlags.Length; i++) {
                EnemyToggles[enemyNames[i]] = enemyFlags[i] == '1';
            }
        }

        public bool[] raceSettings() {
            return new bool[] { RaceMode, DisableIceboltInHeirFight, DisableDistantBellShots, DisableIceGrappling, 
                DisableLadderStorage, DisableUpgradeStealing, };
        }

        public static void copySettings() {
            GUIUtility.systemCopyBuffer = TunicRandomizer.Settings.GetSettingsString();
        }

        public void ReadConnectionSettingsFromSaveFile() {
            ConnectionSettings.Player = SaveFile.GetString(SaveFlags.ArchipelagoPlayerName);
            ConnectionSettings.Port = SaveFile.GetInt(SaveFlags.ArchipelagoPort).ToString();
            ConnectionSettings.Hostname = SaveFile.GetString(SaveFlags.ArchipelagoHostname);
            ConnectionSettings.Password = SaveFile.GetString(SaveFlags.ArchipelagoPassword);
        }

        public static void SaveSettings() {
            TunicUtils.TryWriteFile(TunicRandomizer.SettingsPath, JsonConvert.SerializeObject(TunicRandomizer.Settings, Formatting.Indented));
        }
    }
}
