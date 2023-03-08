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
        public bool HintsEnabled {
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

        // Fox Settings
        public bool RandomFoxColorsEnabled {
            get;
            set;
        }

        public int[] SavedColorPalette {
            get;
            set;
        }

        // Item Tracker Settings
        public bool ItemTrackerFileEnabled {
            get;
            set;
        }

        public bool ItemTrackerOverlayEnabled {
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

        public RandomizerSettings() {
            GameMode = GameModes.RANDOMIZER;
            KeysBehindBosses = false;
            SwordProgressionEnabled = true;
            StartWithSwordEnabled = false;

            HintsEnabled = true;
            ShowItemsEnabled = true;
            ChestsMatchContentsEnabled = true;

            HeirAssistModeEnabled = false;
            CheaperShopItemsEnabled = true;
            BonusStatUpgradesEnabled = true;
            FoolTrapIntensity = FoolTrapOption.NORMAL;

            ItemTrackerFileEnabled = false;
            ItemTrackerOverlayEnabled = false;

            RandomFoxColorsEnabled = true;
            SavedColorPalette = new int[5] {0, 0, 0, 0, 0};
        }

        public RandomizerSettings(bool hintsEnabled, bool randomFoxColorsEnabled) { 
            HintsEnabled = hintsEnabled;
            RandomFoxColorsEnabled = randomFoxColorsEnabled;
        }

        public RandomizerSettings(bool hintsEnabled, bool randomFoxColorsEnabled, bool heirAssistEnaled, FoolTrapOption foolTrapIntensity) {
            HintsEnabled = hintsEnabled;
            RandomFoxColorsEnabled = randomFoxColorsEnabled;
            HeirAssistModeEnabled = heirAssistEnaled;
            FoolTrapIntensity = foolTrapIntensity;
        }
    }
}
