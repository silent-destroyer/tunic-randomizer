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
        public const string HexagonQuestExtras = "randomizer hexagon quest extras";
        public const string HexagonQuestRandomizedValues = "randomizer hexagon quest randomized values";
        public const string HexagonQuestPageAbilities = "randomizer hexagon quest ability pages";

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

        // Entrance Rando Flags
        public const string EntranceRando = "randomizer entrance rando enabled";
        public const string ERFixedShop = "randomizer ER fixed shop enabled";
        public const string PortalDirectionPairs = "randomizer paired portal directions enabled";
        public const string Decoupled = "randomizer decoupled ER enabled";

        // Logic Trick Flags
        public const string LaurelsZips = "randomizer laurels zips enabled";
        public const string IceGrapplingDifficulty = "randomizer ice grappling difficulty";
        public const string LadderStorageDifficulty = "randomizer ladder storage difficulty";
        public const string LadderStorageWithoutItems = "randomizer ladder storage without items enabled";

        // Other Logic Flags
        public const string StartWithSword = "randomizer started with sword";
        public const string MasklessLogic = "randomizer maskless logic enabled";
        public const string LanternlessLogic = "randomizer lanternless logic enabled";
        public const string LadderRandoEnabled = "randomizer ladder rando enabled";
        public const string LaurelsLocation = "randomizer laurels location";
        public const string GrassRandoEnabled = "randomizer grass rando enabled";
        public const string BreakableShuffleEnabled = "randomizer breakable shuffle enabled";
        public const string FuseShuffleEnabled = "randomizer fuse shuffle enabled";
        public const string BellShuffleEnabled = "randomizer bell shuffle enabled";

        // Special Flags
        public const string PlayerDeathCount = "randomizer death count";
        public const string EnemiesDefeatedCount = "randomizer enemies defeated";
        public const string DiedToHeir = "randomizer died to heir";
        public const string RescuedLostFox = "randomizer sent lost fox home";
        public const string CryptSecret = "randomizer crypt secret filigree door opened";
        public const string QuarrySecret = "randomizer quarry secret portal spawned";

        // Archipelago
        public const string ArchipelagoFlag = "archipelago";
        public const string ArchipelagoPlayerName = "archipelago player name";
        public const string ArchipelagoPort = "archipelago port";
        public const string ArchipelagoHostname = "archipelago hostname";
        public const string ArchipelagoPassword = "archipelago password";

        public static bool IsArchipelago() {
            return SaveFile.GetInt("archipelago") == 1 && (Archipelago.instance != null && Archipelago.instance.IsConnected());
        }

        public static bool IsSinglePlayer() {
            return SaveFile.GetInt("randomizer") == 1;
        }

        public static bool IsHexQuestWithPageAbilities() {
            return GetBool(HexagonQuestEnabled) && GetBool(AbilityShuffle) && GetBool(HexagonQuestPageAbilities);
        }

        public static bool IsHexQuestWithHexAbilities() {
            return GetBool(HexagonQuestEnabled) && GetBool(AbilityShuffle) && !GetBool(HexagonQuestPageAbilities);
        }

        public static bool IsHexagonAbilityQuestWithKeysOnBosses() {
            return IsHexQuestWithHexAbilities() && GetBool(KeysBehindBosses);
        }

        public static bool GetBool(string value) {
            return SaveFile.GetInt(value) == 1;
        }
    }
}
