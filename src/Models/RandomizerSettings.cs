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

        public bool TimerOverlayEnabled {
            get;
            set;
        }

        public RandomizerSettings() {
            HintsEnabled = true;
            RandomFoxColorsEnabled = true;
            TimerOverlayEnabled = false;
        }

        public RandomizerSettings(bool hintsEnabled, bool randomFoxColorsEnabled, bool timerOverlayEnabled) { 
            HintsEnabled = hintsEnabled;
            RandomFoxColorsEnabled = randomFoxColorsEnabled;
            TimerOverlayEnabled = timerOverlayEnabled;
        }
    }
}
