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

        public FoolTrapOption FoolTrapIntensity {
            get;
            set;
        }

        // Enemy Randomization Settings
        public bool EnemyRandomizerEnabled {
            get;
            set;
        }

        public EnemyRandomizationType EnemyGeneration {
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
            HEXAGONQUEST
        }

        public enum FoolTrapOption { 
            NONE,
            NORMAL,
            DOUBLE,
            ONSLAUGHT,
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

            HeroPathHintsEnabled = true;
            GhostFoxHintsEnabled = true;
            ShowItemsEnabled = true;
            ChestsMatchContentsEnabled = true;

            HeirAssistModeEnabled = false;
            CheaperShopItemsEnabled = true;
            BonusStatUpgradesEnabled = true;
            FoolTrapIntensity = FoolTrapOption.NORMAL;
            
            EnemyRandomizerEnabled = false;
            EnemyGeneration = EnemyRandomizationType.RANDOM;
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
