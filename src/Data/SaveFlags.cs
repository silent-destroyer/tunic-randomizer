namespace TunicRandomizer {
    public class SaveFlags {

        public const string ItemCollectedKey = "randomizer picked up ";

        // Hexagon Quest Flags
        public const string HexagonQuestEnabled = "randomizer hexagon quest enabled";
        public const string HexagonQuestPrayer = "randomizer hexagon quest prayer requirement";
        public const string HexagonQuestHolyCross = "randomizer hexagon quest holy cross requirement";
        public const string HexagonQuestIcebolt = "randomizer hexagon quest icebolt requirement";
        public const string GoldHexagonQuantity = "inventory quantity Hexagon Gold";
        public const string HexagonQuestGoal = "randomizer hexagon quest goal";

        // Ability Shuffle Flags
        public const string AbilityShuffle = "randomizer shuffled abilities";
        public const string PrayerUnlocked = "randomizer prayer unlocked";
        public const string HolyCrossUnlocked = "randomizer holy cross unlocked";
        public const string IceBoltUnlocked = "randomizer icebolt unlocked";
        public const string PrayerUnlockedTime = "randomizer prayer unlocked time";
        public const string HolyCrossUnlockedTime = "randomizer holy cross unlocked time";
        public const string IceboltUnlockedTime = "randomizer icebolt unlocked time";

        // Keys Behind Bosses Flag
        public const string KeysBehindBosses = "randomizer keys behind bosses";

        // Sword Progression Flags
        public const string SwordProgressionEnabled = "randomizer sword progression enabled";
        public const string SwordProgressionLevel = "randomizer sword progression level";

        // Entrance Rando Flag
        public const string EntranceRando = "randomizer entrance rando enabled";

        // Other Logic Flags
        public const string MasklessLogic = "randomizer maskless logic enabled";
        public const string LanternlessLogic = "randomizer lanternless logic enabled";
        public const string LadderRandoEnabled = "randomizer ladder rando enabled";
        public const string LaurelsLocation = "randomizer laurels location";

        // Special Flags
        public const string PlayerDeathCount = "randomizer death count";
        public const string EnemiesDefeatedCount = "randomizer enemies defeated";
        public const string DiedToHeir = "randomizer died to heir";
        public const string RescuedLostFox = "randomizer sent lost fox home";

        public static bool IsArchipelago() {
            return SaveFile.GetInt("archipelago") == 1 && (Archipelago.instance != null && Archipelago.instance.integration != null && Archipelago.instance.integration.connected);
        }

        public static bool IsSinglePlayer() {
            return SaveFile.GetInt("randomizer") == 1;
        }
    }
}
