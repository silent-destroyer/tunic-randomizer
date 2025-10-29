using System;
using System.Linq;

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
        public int ShuffleBreakables;
        public int ShuffleFuses;
        public int ShuffleBells;

        public int LaurelsZips;
        public int LadderStorageWithoutItems;

        public bool IceGrappleOff;
        public bool IceGrappleEasy;
        public bool IceGrappleMedium;
        public bool IceGrappleHard;

        public bool LadderStorageOff;
        public bool LadderStorageEasy;
        public bool LadderStorageMedium;
        public bool LadderStorageHard;

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
            ShuffleBreakables = 50;
            ShuffleFuses = 50;
            ShuffleBells = 50;

            LaurelsZips = 0;
            LadderStorageWithoutItems = 0;

            IceGrappleOff = true;
            IceGrappleEasy = false;
            IceGrappleMedium = false;
            IceGrappleHard = false;

            LadderStorageOff = true;
            LadderStorageEasy = false;
            LadderStorageMedium = false;
            LadderStorageHard = false;

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
                $"&{ShuffleBreakables}" +
                $"&{ShuffleFuses}" +
                $"&{ShuffleBells}" +
                $"&{HexagonQuest}" +
                $"&{HexQuestAbilityShufflePages}" +
                $"&{LaurelsZips}" +
                $"&{LadderStorageWithoutItems}" +
                $"&{Convert.ToInt32(sToB(new bool[] { FoolTrapNone, FoolTrapNormal, FoolTrapDouble, FoolTrapOnslaught, LaurelsRandom, LaurelsSixCoins, LaurelsTenCoins, LaurelsTenFairies }), 2)}" +
                $"&{Convert.ToInt32(sToB(new bool[] { HexQuestGoalRandom, HexQuestGoalLow, HexQuestGoalMedium, HexQuestGoalHigh, HexQuestExtrasRandom, HexQuestExtrasLow, HexQuestExtrasMedium, HexQuestExtrasHigh }), 2)}" +
                $"&{Convert.ToInt32(sToB(new bool[] { IceGrappleOff, IceGrappleEasy, IceGrappleMedium, IceGrappleHard, LadderStorageOff, LadderStorageEasy, LadderStorageMedium, LadderStorageHard }), 2)}";
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
            ShuffleBreakables = int.Parse(split[i++]);
            ShuffleFuses = int.Parse(split[i++]);
            ShuffleBells = int.Parse(split[i++]);
            HexagonQuest = int.Parse(split[i++]);
            HexQuestAbilityShufflePages = int.Parse(split[i++]);
            LaurelsZips = int.Parse(split[i++]);
            LadderStorageWithoutItems = int.Parse(split[i++]);
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
            int trickLogic = int.Parse(split[i++]);
            IceGrappleOff = eval(trickLogic, 1);
            IceGrappleEasy = eval(trickLogic, 2);
            IceGrappleMedium = eval(trickLogic, 4);
            IceGrappleHard = eval(trickLogic, 8);
            LadderStorageOff = eval(trickLogic, 16);
            LadderStorageEasy = eval(trickLogic, 32);
            LadderStorageMedium = eval(trickLogic, 64);
            LadderStorageHard = eval(trickLogic, 128);
        }

        public string sToB(bool[] settings) {
            return string.Join("", settings.Reverse().Select(x => x ? "1" : "0").ToList());
        }

        private bool eval(int a, int b) {
            return (a & b) != 0;
        }

        public void Randomize() {
            Random random = new System.Random();
            SwordProgression = random.Next(101);
            KeysBehindBosses = random.Next(101);
            ShuffleAbilities = random.Next(101);
            ShuffleLadders = random.Next(101);
            EntranceRando = random.Next(101);
            ERFixedShop = random.Next(101);
            ERDecoupled = random.Next(101);
            ERDirectionPairs = random.Next(101);
            Maskless = random.Next(101);
            Lanternless = random.Next(101);
            GrassRando = random.Next(101);
            ShuffleBreakables = random.Next(101);
            ShuffleFuses = random.Next(101);
            ShuffleBells = random.Next(101);
            HexagonQuest = random.Next(101);
            HexQuestAbilityShufflePages = random.Next(101);

            LaurelsZips = random.Next(101);
            LadderStorageWithoutItems = random.Next(101);

            IceGrappleOff = random.Next(2) == 1;
            IceGrappleEasy = random.Next(2) == 1;
            IceGrappleMedium = random.Next(2) == 1;
            IceGrappleHard = random.Next(2) == 1;

            LadderStorageOff = random.Next(2) == 1;
            LadderStorageEasy = random.Next(2) == 1;
            LadderStorageMedium = random.Next(2) == 1;
            LadderStorageHard = random.Next(2) == 1;

            FoolTrapNone = random.Next(2) == 1;
            FoolTrapNormal = random.Next(2) == 1;
            FoolTrapDouble = random.Next(2) == 1;
            FoolTrapOnslaught = random.Next(2) == 1;

            LaurelsRandom = random.Next(2) == 1;
            LaurelsSixCoins = random.Next(2) == 1;
            LaurelsTenCoins = random.Next(2) == 1;
            LaurelsTenFairies = random.Next(2) == 1;

            HexQuestGoalRandom = random.Next(2) == 1;
            HexQuestGoalLow = random.Next(2) == 1;
            HexQuestGoalMedium = random.Next(2) == 1;
            HexQuestGoalHigh = random.Next(2) == 1;

            HexQuestExtrasRandom = random.Next(2) == 1;
            HexQuestExtrasLow = random.Next(2) == 1;
            HexQuestExtrasMedium = random.Next(2) == 1;
            HexQuestExtrasHigh = random.Next(2) == 1;
            RandomizerSettings.SaveSettings();
        }
    }
}
