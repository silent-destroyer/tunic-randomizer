using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunicRandomizer {
    public class MysterySeedWeights {

        public int SwordProgression;
        public int KeysBehindBosses;
        public int ShuffleAbilities;
        public int ShuffleLadders;
        public int EntranceRando;
        public int ERFixedShop;
        public int ERDecoupled;
        public int ERDirectionPairs;
        public int Maskless;
        public int Lanternless;
        public int GrassRando;

        public bool FoolTrapNone;
        public bool FoolTrapNormal;
        public bool FoolTrapDouble;
        public bool FoolTrapOnslaught;

        public bool LaurelsRandom;
        public bool LaurelsSixCoins;
        public bool LaurelsTenCoins;
        public bool LaurelsTenFairies;

        public int HexagonQuest;
        public int HexQuestAbilityShufflePages;
        public bool HexQuestGoalRandom;
        public bool HexQuestGoalLow;
        public bool HexQuestGoalMedium;
        public bool HexQuestGoalHigh;

        public bool HexQuestExtrasRandom;
        public bool HexQuestExtrasLow;
        public bool HexQuestExtrasMedium;
        public bool HexQuestExtrasHigh;

        public MysterySeedWeights() {
            SwordProgression = 100;
            KeysBehindBosses = 50;
            ShuffleAbilities = 50;
            ShuffleLadders = 50;
            EntranceRando = 50;
            ERFixedShop = 50;
            ERDirectionPairs = 50;
            ERDecoupled = 50;
            Maskless = 25;
            Lanternless = 25;
            GrassRando = 50;

            FoolTrapNone = true;
            FoolTrapNormal = true;
            FoolTrapDouble = true;
            FoolTrapOnslaught = true;

            LaurelsRandom = true;
            LaurelsSixCoins = true;
            LaurelsTenCoins = true;
            LaurelsTenFairies = true;

            HexagonQuest = 50;
            HexQuestAbilityShufflePages = 50;
            HexQuestGoalRandom = true;
            HexQuestGoalLow = true;
            HexQuestGoalMedium = true;
            HexQuestGoalHigh = true;
            HexQuestExtrasRandom = true;
            HexQuestExtrasLow = true;
            HexQuestExtrasMedium = true;
            HexQuestExtrasHigh = true;

        }

        public string ToSettingsString() {
            string s = $"{SwordProgression}" +
                $"&{KeysBehindBosses}" +
                $"&{ShuffleAbilities}" +
                $"&{ShuffleLadders}" +
                $"&{EntranceRando}" +
                $"&{ERFixedShop}" +
                $"&{ERDecoupled}" +
                $"&{ERDirectionPairs}" +
                $"&{Maskless}" +
                $"&{Lanternless}" +
                $"&{GrassRando}" +
                $"&{HexagonQuest}" +
                $"&{HexQuestAbilityShufflePages}" +
                $"&{Convert.ToInt32(sToB(new bool[] { FoolTrapNone, FoolTrapNormal, FoolTrapDouble, FoolTrapOnslaught, LaurelsRandom, LaurelsSixCoins, LaurelsTenCoins, LaurelsTenFairies }), 2)}" +
                $"&{Convert.ToInt32(sToB(new bool[] { HexQuestGoalRandom, HexQuestGoalLow, HexQuestGoalMedium, HexQuestGoalHigh, HexQuestExtrasRandom, HexQuestExtrasLow, HexQuestExtrasMedium, HexQuestExtrasHigh }), 2)}";
            return s;
        }

        public void FromSettingsString(string s) {
            string[] split = s.Split('&');
            int i = 0;
            SwordProgression = int.Parse(split[i++]);
            KeysBehindBosses = int.Parse(split[i++]);
            ShuffleAbilities = int.Parse(split[i++]);
            ShuffleLadders = int.Parse(split[i++]);
            EntranceRando = int.Parse(split[i++]);
            ERFixedShop = int.Parse(split[i++]);
            ERDecoupled = int.Parse(split[i++]);
            ERDirectionPairs = int.Parse(split[i++]);
            Maskless = int.Parse(split[i++]);
            Lanternless = int.Parse(split[i++]);
            GrassRando = int.Parse(split[i++]);
            HexagonQuest = int.Parse(split[i++]);
            HexQuestAbilityShufflePages = int.Parse(split[i++]);
            int foolLaurels = int.Parse(split[i++]);
            FoolTrapNone = eval(foolLaurels, 1);
            FoolTrapNormal = eval(foolLaurels, 2);
            FoolTrapDouble = eval(foolLaurels, 4);
            FoolTrapOnslaught = eval(foolLaurels, 8);
            LaurelsRandom = eval(foolLaurels, 16);
            LaurelsSixCoins = eval(foolLaurels, 32);
            LaurelsTenCoins = eval(foolLaurels, 64);
            LaurelsTenFairies = eval(foolLaurels, 128);
            int hexQuest = int.Parse(split[i++]);
            HexQuestGoalRandom = eval(hexQuest, 1);
            HexQuestGoalLow = eval(hexQuest, 2);
            HexQuestGoalMedium = eval(hexQuest, 4);
            HexQuestGoalHigh = eval(hexQuest, 8);
            HexQuestExtrasRandom = eval(hexQuest, 16);
            HexQuestExtrasLow = eval(hexQuest, 32);
            HexQuestExtrasMedium = eval(hexQuest, 64);
            HexQuestExtrasHigh = eval(hexQuest, 128);
        }

        public string sToB(bool[] settings) {
            return string.Join("", settings.Reverse().Select(x => x ? "1" : "0").ToList());
        }

        private bool eval(int a, int b) {
            return (a & b) != 0;
        }
    }
}
