using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunicRandomizer {

    public class RandomizerSettings {
        public bool HintsEnabled {
            get;
            set;
        }

        public bool RandomFoxColorsEnabled {
            get;
            set;
        }

        public bool HeirAssistModeEnabled {
            get;
            set;
        }

        public enum FoolTrapOption { 
            NONE,
            NORMAL,
            DOUBLE,
            ONSLAUGHT,
        }

        public FoolTrapOption FoolTrapIntensity {
            get;
            set;
        }

        public RandomizerSettings() {
            HintsEnabled = true;
            RandomFoxColorsEnabled = true;
            HeirAssistModeEnabled = true;
            FoolTrapIntensity = FoolTrapOption.NORMAL;
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
