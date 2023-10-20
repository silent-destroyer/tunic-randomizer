using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunicRandomizer {

    public class RandomizerSettings {
        // Logic Settings
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

        public bool EntranceRandoEnabled
        {
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

        // Hint Settings
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

        // Gameplay Settings
        public bool HeirAssistModeEnabled {
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

        public FoolTrapOption FoolTrapIntensity {
            get;
            set;
        }

        // Enemy Randomization Settings
        public bool EnemyRandomizerEnabled {
            get;
            set;
        }

        public EnemyRandomizationType EnemyDifficulty {
            get;
            set;
        }

        public EnemyGenerationType EnemyGeneration {
            get;
            set;
        }

        public bool ExtraEnemiesEnabled {
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

        public enum EnemyGenerationType { 
            RANDOM,
            SEEDED
        }

        public enum EnemyRandomizationType { 
            RANDOM,
            BALANCED
        }

        public RandomizerSettings() {
            GameMode = GameModes.RANDOMIZER;
            KeysBehindBosses = false;
            SwordProgressionEnabled = true;
            StartWithSwordEnabled = false;
            ShuffleAbilities = false;
            EntranceRandoEnabled = false;
            HexagonQuestGoal = 20;
            HexagonQuestExtraPercentage = 50;

            HeroPathHintsEnabled = true;
            GhostFoxHintsEnabled = true;
            ShowItemsEnabled = true;
            ChestsMatchContentsEnabled = true;
            UseTrunicTranslations = false;

            HeirAssistModeEnabled = false;
            CheaperShopItemsEnabled = true;
            BonusStatUpgradesEnabled = true;
            DisableChestInterruption = false;
            FoolTrapIntensity = FoolTrapOption.NORMAL;
            
            EnemyRandomizerEnabled = false;
            EnemyDifficulty = EnemyRandomizationType.RANDOM;
            EnemyGeneration = EnemyGenerationType.RANDOM;
            ExtraEnemiesEnabled = false;

            RandomFoxColorsEnabled = true;
            RealestAlwaysOn = false;
            UseCustomTexture = false;
        }

        public RandomizerSettings(bool hintsEnabled, bool randomFoxColorsEnabled) { 
            HeroPathHintsEnabled = hintsEnabled;
            RandomFoxColorsEnabled = randomFoxColorsEnabled;
        }

        public RandomizerSettings(bool hintsEnabled, bool randomFoxColorsEnabled, bool heirAssistEnaled, FoolTrapOption foolTrapIntensity) {
            HeroPathHintsEnabled = hintsEnabled;
            RandomFoxColorsEnabled = randomFoxColorsEnabled;
            HeirAssistModeEnabled = heirAssistEnaled;
            FoolTrapIntensity = foolTrapIntensity;
        }
    }
}
