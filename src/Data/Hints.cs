﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using static TunicRandomizer.GhostHints;
using Archipelago.MultiClient.Net.Enums;
using static TunicRandomizer.SaveFlags;
using BepInEx.Logging;
using UnityEngine;
using Archipelago.MultiClient.Net.Models;
using Il2CppSystem;
using UnityEngine.InputSystem;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Helpers;

namespace TunicRandomizer {
    public class Hints {

        public static Dictionary<string, string> HintLocations = new Dictionary<string, string>() {
            {"Overworld Redux (-7.0, 12.0, -136.4)", "Mailbox"},
            {"Archipelagos Redux (-170.0, 11.0, 152.5)", "West Garden Relic"},
            {"Swamp Redux 2 (92.0, 7.0, 90.8)", "Swamp Relic"},
            {"Sword Access (28.5, 5.0, -190.0)", "East Forest Relic"},
            {"Library Hall (131.0, 19.0, -8.5)", "Library Relic"},
            {"Monastery (-6.0, 26.0, 180.5)", "Monastery Relic"},
            {"Fortress Reliquary (198.5, 5.0, -40.0)", "Fortress Relic"},
            {"Temple (14.0, -0.5, 49.0)", "Temple Statue"},
            {"Overworld Redux (89.0, 44.0, -107.0)", "East Forest Sign"},
            {"Overworld Redux (-5.0, 36.0, -70.0)", "Town Sign"},
            {"Overworld Redux (8.0, 20.0, -115.0)", "Ruined Hall Sign"},
            {"Overworld Redux (-156.0, 12.0, -44.3)", "West Garden Sign"},
            {"Overworld Redux (73.0, 44.0, -38.0)", "Fortress Sign"},
            {"Overworld Redux (-141.0, 40.0, 34.8)", "Quarry Sign"},
            {"East Forest Redux (128.0, 0.0, 33.5)", "West East Forest Sign"},
            {"East Forest Redux (144.0, 0.0, -23.0)", "East East Forest Sign"},
        };

        public struct HeroGraveHint {
            public string PathHintId;
            public string PathHint;
            public string RelicHintId;
            public string RelicHint;
            public string SceneName;
            public string GraveObjectPath;
            public bool PointLight;

            public HeroGraveHint(string pathHintId, string pathHint, string relicHintId, string relicHint, string sceneName, string graveObjectPath, bool pointLight) { 
                PathHintId = pathHintId;
                PathHint = pathHint;
                RelicHintId = relicHintId;
                RelicHint = relicHint; 
                SceneName = sceneName;
                GraveObjectPath = graveObjectPath;
                PointLight = pointLight;
            }
        }
        
        public static Dictionary<string, HeroGraveHint> HeroGraveHints = new Dictionary<string, HeroGraveHint>();

        public static Dictionary<string, string> HintMessages = new Dictionary<string, string>();

        public static void PopulateHints() {
            HintMessages.Clear();
            HeroGraveHints.Clear();
            System.Random random = new System.Random(SaveFile.GetInt("seed"));
            string Hint = "";
            string Scene = "";
            string Prefix = "";
            List<char> Vowels = new List<char>() { 'A', 'E', 'I', 'O', 'U', 'a', 'e', 'i', 'o', 'u' };

            (bool, bool, bool, bool) SinglePlayerItemsHinted = (false, false, false, false);

            if (IsArchipelago()) {
                CreateAPMailboxHint(random);
            } else if (IsSinglePlayer()) {
                SinglePlayerItemsHinted = CreateSinglePlayerMailboxHint(random);
            }
            
            Hint = $"lehjehnd sehz <#FF00FF>suhm%i^ ehkstruhordinArE<#FFFFFF>  [laurels] ";
            if (IsArchipelago()) {
                int Player = Archipelago.instance.GetPlayerSlot();
                ArchipelagoHint Hyperdash = Locations.MajorItemLocations["Hero's Laurels"][0];
                if (Hyperdash.Player == Player) {
                    Scene = Hyperdash.Location == "Your Pocket" ? Hyperdash.Location.ToUpper() : Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[Hyperdash.Location]].Location.SceneName].ToUpper();
                    Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                    Hint += $"\nuhwAts yoo in {Prefix} \"{Scene}...\"";
                } else if (Archipelago.instance.IsTunicPlayer((int)Hyperdash.Player)) {
                    Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[Hyperdash.Location]].Location.SceneName].ToUpper();
                    Hint += $"\nuhwAts yoo in \"{Archipelago.instance.GetPlayerName((int)Hyperdash.Player).ToUpper()}'S\"\n\"{Scene}...\"";
                } else {
                    Hint += $" uhwAts yoo aht\n{WordWrapString($"\"{Hyperdash.Location.Replace("_", " ").Replace(" ", "\" \"").ToUpper()}\"").Replace("\" \"", " ")}\nin\"{Archipelago.instance.GetPlayerName((int)Hyperdash.Player).ToUpper()}'S WORLD...\"";
                }
            } else if (IsSinglePlayer()) {
                Check LaurelsCheck = ItemRandomizer.FindRandomizedItemByName("Hyperdash");
                Scene = Locations.SimplifiedSceneNames[LaurelsCheck.Location.SceneName];
                Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                Hint += TunicRandomizer.Settings.UseTrunicTranslations ? $"\n<#FFFFFF>uhwAts yoo aht {Prefix} {Translations.Translate(Scene, false)}\"...\"" : $"\nuhwAts yoo in {Prefix} \"{Scene.ToUpper()}...\"";
            }
            HintMessages.Add("Temple Statue", Hint);
            List<(string, string)> relicHints = CreateHeroRelicHints();
            List<string> HintItems = new List<string>() { SinglePlayerItemsHinted.Item1 ? "Lantern" : "Magic Wand", SinglePlayerItemsHinted.Item2 ? "Lantern" : "Magic Orb", "Magic Dagger" };
            if (SaveFile.GetInt(AbilityShuffle) == 1 && SaveFile.GetInt(HexagonQuestEnabled) == 0) {
                HintItems.Add(SinglePlayerItemsHinted.Item3 ? "Lantern" : "Pages 24-25 (Prayer)");
                HintItems.Add(SinglePlayerItemsHinted.Item4 ? "Lantern" : "Pages 42-43 (Holy Cross)");
                HintItems.Remove("Magic Dagger");
            }
            List<string> HintGraves = new List<string>() { "East Forest Relic", "Fortress Relic", "West Garden Relic" };
            while (HintGraves.Count > 0) {
                string HintItem = HintItems[random.Next(HintItems.Count)];
                string HintGrave = HintGraves[random.Next(HintGraves.Count)];
                string slotLocation = "";
                (string, string) RelicHint = relicHints[random.Next(relicHints.Count)];
                Hint = $"lehjehnd sehz ";
                if (IsArchipelago()) {
                    ArchipelagoHint ItemHint = Locations.MajorItemLocations[HintItem][0];
                    int Player = Archipelago.instance.GetPlayerSlot();

                    if (ItemHint.Player == Player) {
                        Scene = ItemHint.Location == "Your Pocket" ? ItemHint.Location : Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[ItemHint.Location]].Location.SceneName];
                        if (HintItem == "Pages 24-25 (Prayer)" && Scene == "Fortress Relic") {
                            continue;
                        }
                        Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                        Hint += $"{Prefix} {(TunicRandomizer.Settings.UseTrunicTranslations ? Translations.Translate(Scene, false) : $"\"{Scene.ToUpper()}\"")}";
                    } else if (Archipelago.instance.IsTunicPlayer((int)ItemHint.Player)) {
                        Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[ItemHint.Location]].Location.SceneName];
                        Hint += $"\"{Archipelago.instance.GetPlayerName((int)ItemHint.Player).ToUpper()}'S\"\n{(TunicRandomizer.Settings.UseTrunicTranslations ? Translations.Translate(Scene, false) : $"\"{Scene.ToUpper()}\"")}";
                    } else {
                        Hint += $"\"{Archipelago.instance.GetPlayerName((int)ItemHint.Player).ToUpper()}'S WORLD\" aht\n{WordWrapString($"\"{ItemHint.Location.Replace("_", " ").Replace(" ", "\" \"").ToUpper()}\"").Replace("\" \"", " ")}";
                    }
                    slotLocation = ItemHint.Location == "Your Pocket" ? $"0, Server" : $"{ItemHint.Player}, {ItemHint.Location}";
                } else if (IsSinglePlayer()) {
                    ItemData Item = ItemLookup.Items[HintItem];
                    Check ItemCheck = ItemRandomizer.FindRandomizedItemByName(Item.ItemNameForInventory);
                    Scene = Locations.SimplifiedSceneNames[ItemCheck.Location.SceneName];
                    Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                    Hint += TunicRandomizer.Settings.UseTrunicTranslations ? $"lehjehnd sehz {Prefix} {Translations.Translate(Scene, false)}" : $"lehjehnd sehz {Prefix} \"{Scene.ToUpper()}\"";
                    slotLocation = $"{ItemCheck.Location.LocationId} [{ItemCheck.Location.SceneName}]";
                }
                Hint += $"\niz lOkAtid awn #uh {(TunicRandomizer.Settings.UseTrunicTranslations ? $"<#ffd700>pah% uhv #uh hErO<#ffffff>\"...\"" : $"\"<#ffd700>PATH OF THE HERO<#ffffff>...\"")}";

                if (HintGrave == "East Forest Relic") {
                    HeroGraveHints.Add(HintGrave, new HeroGraveHint(slotLocation, Hint, RelicHint.Item1, RelicHint.Item2, "Sword Access", "_Setpieces/RelicPlinth (1)/", true));
                } else if (HintGrave == "Fortress Relic") {
                    HeroGraveHints.Add(HintGrave, new HeroGraveHint(slotLocation, Hint, RelicHint.Item1, RelicHint.Item2, "Fortress Reliquary", "RelicPlinth", true));
                } else if (HintGrave == "West Garden Relic") {
                    HeroGraveHints.Add(HintGrave, new HeroGraveHint(slotLocation, Hint, RelicHint.Item1, RelicHint.Item2, "Archipelagos Redux", "_Environment Prefabs/RelicPlinth/", false));
                }
                relicHints.Remove(RelicHint);
                HintItems.Remove(HintItem);
                HintGraves.Remove(HintGrave);
            }

            List<string> Hexagons;
            Dictionary<string, string> HexagonColors = new Dictionary<string, string>() { { "Red Questagon", "<#FF3333>" }, { "Green Questagon", "<#33FF33>" }, { "Blue Questagon", "<#3333FF>" }, { "Gold Questagon", "<#ffd700>" } };
            if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                Hexagons = new List<string>() { "Gold Questagon", "Gold Questagon", "Gold Questagon" };
            } else {
                Hexagons = new List<string>() { "Red Questagon", "Green Questagon", "Blue Questagon" };
            }
            List<string> strings = new List<string>();
            List<string> HexagonHintGraves = new List<string>() { "Swamp Relic", "Library Relic", "Monastery Relic" };
            for (int i = 0; i < 3; i++) {
                string Hexagon = Hexagons[random.Next(Hexagons.Count)];
                string HexagonHintArea = HexagonHintGraves[random.Next(HexagonHintGraves.Count)];
                string slotLocation = "";
                (string, string) RelicHint = relicHints[random.Next(relicHints.Count)];

                if (IsArchipelago()) {
                    ArchipelagoHint HexHint = Hexagon == "Gold Questagon" ? Locations.MajorItemLocations[Hexagon][i] : Locations.MajorItemLocations[Hexagon][0];
                    int Player = Archipelago.instance.GetPlayerSlot();
                    if (HexHint.Player == Player) {
                        Scene = HexHint.Location == "Your Pocket" ? HexHint.Location : Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[HexHint.Location]].Location.SceneName];
                        Prefix = Vowels.Contains(Scene.ToUpper()[0]) ? "#E" : "#uh";
                        Hint = $"#A sA {Prefix} {(TunicRandomizer.Settings.UseTrunicTranslations ? Translations.Translate(Scene, false) : $"\"{Scene.ToUpper()}\"")} iz \nwAr #uh {HexagonColors[Hexagon]}kwehstuhgawn [hexagram]<#FFFFFF> iz fownd\"...\"";
                    } else if (Archipelago.instance.IsTunicPlayer((int)HexHint.Player)) {
                        Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[HexHint.Location]].Location.SceneName];
                        Prefix = Vowels.Contains(Scene.ToUpper()[0]) ? "#E" : "#uh";
                        Hint = $"#A sA \"{Archipelago.instance.GetPlayerName((int)HexHint.Player).ToUpper()}'S\"\n{(TunicRandomizer.Settings.UseTrunicTranslations ? Translations.Translate(Scene, false) : $"\"{Scene.ToUpper()}\"")}\niz wAr #uh {HexagonColors[Hexagon]}kwehstuhgawn [hexagram]<#FFFFFF> iz fownd\"...\"";
                    } else {
                        Hint = $"#A sA #uh {HexagonColors[Hexagon]}kwehstuhgawn [hexagram]<#FFFFFF> iz fownd aht\n{WordWrapString($"\"{HexHint.Location.Replace("_", " ").Replace(" ", "\" \"").ToUpper()}\"").Replace("\" \"", " ")}\nin \"{Archipelago.instance.GetPlayerName((int)HexHint.Player).ToUpper()}'S WORLD...\"";
                    }
                    slotLocation = HexHint.Location == "Your Pocket" ? $"0, Server" : $"{HexHint.Player}, {HexHint.Location}";
                } else if (IsSinglePlayer()) {
                    ItemData Hex = ItemLookup.Items[Hexagon];
                    Check HexCheck = Hex.Name == "Gold Questagon" ? ItemRandomizer.FindAllRandomizedItemsByName(Hex.ItemNameForInventory)[i] : ItemRandomizer.FindRandomizedItemByName(Hex.ItemNameForInventory);
                    Scene = Locations.SimplifiedSceneNames[HexCheck.Location.SceneName];
                    Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                    Hint = $"#A sA {Prefix} {(TunicRandomizer.Settings.UseTrunicTranslations ? Translations.Translate(Scene, false) : $"\"{Scene.ToUpper()}\"")} iz \nwAr #uh {HexagonColors[Hexagon]}kwehstuhgawn [hexagram]<#FFFFFF> iz fownd\"...\"";
                    slotLocation = $"{HexCheck.Location.LocationId} [{HexCheck.Location.SceneName}]";
                }

                if (HexagonHintArea == "Swamp Relic") {
                    HeroGraveHints.Add(HexagonHintArea, new HeroGraveHint(slotLocation, Hint, RelicHint.Item1, RelicHint.Item2, "Swamp Redux 2", "_Setpieces Etc/RelicPlinth/", false));
                } else if (HexagonHintArea == "Monastery Relic") {
                    HeroGraveHints.Add(HexagonHintArea, new HeroGraveHint(slotLocation, Hint, RelicHint.Item1, RelicHint.Item2, "Monastery", "Root/RelicPlinth (1)/", false));
                } else if (HexagonHintArea == "Library Relic") {
                    HeroGraveHints.Add(HexagonHintArea, new HeroGraveHint(slotLocation, Hint, RelicHint.Item1, RelicHint.Item2, "Library Hall", "_Special/RelicPlinth/", false));
                }

                relicHints.Remove(RelicHint);
                Hexagons.Remove(Hexagon);
                HexagonHintGraves.Remove(HexagonHintArea);
            }

            // Hack to make start inventory from pool'd items toggle the hero graves
            SaveFile.SetInt($"randomizer hint found 0, Server", 1);

            // make the in-game signs tell you what area they're pointing to
            if (SaveFile.GetInt(EntranceRando) == 1) {
                foreach (PortalCombo Portal in TunicPortals.RandomizedPortals.Values) {
                    if (Portal.Portal1.SceneDestinationTag == "Overworld Redux, Forest Belltower_")
                    { HintMessages.Add("East Forest Sign", $"{Translations.TranslateDefaultQuotes(Locations.SimplifiedSceneNames[Portal.Portal2.Scene])} [arrow_right]"); }
                    if (Portal.Portal2.SceneDestinationTag == "Overworld Redux, Forest Belltower_")
                    { HintMessages.Add("East Forest Sign", $"{Translations.TranslateDefaultQuotes(Locations.SimplifiedSceneNames[Portal.Portal1.Scene])} [arrow_right]"); }

                    if (Portal.Portal1.SceneDestinationTag == "Overworld Redux, Archipelagos Redux_lower")
                    { HintMessages.Add("West Garden Sign", $"[arrow_left] {Translations.TranslateDefaultQuotes(Locations.SimplifiedSceneNames[Portal.Portal2.Scene])}"); }
                    if (Portal.Portal2.SceneDestinationTag == "Overworld Redux, Archipelagos Redux_lower")
                    { HintMessages.Add("West Garden Sign", $"[arrow_left] {Translations.TranslateDefaultQuotes(Locations.SimplifiedSceneNames[Portal.Portal1.Scene])}"); }

                    if (Portal.Portal1.SceneDestinationTag == "Overworld Redux, Fortress Courtyard_")
                    { HintMessages.Add("Fortress Sign", $"{Translations.TranslateDefaultQuotes(Locations.SimplifiedSceneNames[Portal.Portal2.Scene])}  [arrow_right]"); }
                    if (Portal.Portal2.SceneDestinationTag == "Overworld Redux, Fortress Courtyard_")
                    { HintMessages.Add("Fortress Sign", $"{Translations.TranslateDefaultQuotes(Locations.SimplifiedSceneNames[Portal.Portal1.Scene])} [arrow_right]"); }

                    if (Portal.Portal1.SceneDestinationTag == "Overworld Redux, Darkwoods Tunnel_")
                    { HintMessages.Add("Quarry Sign", $"{Translations.TranslateDefaultQuotes(Locations.SimplifiedSceneNames[Portal.Portal2.Scene])} [arrow_up]"); }
                    if (Portal.Portal2.SceneDestinationTag == "Overworld Redux, Darkwoods Tunnel_")
                    { HintMessages.Add("Quarry Sign", $"{Translations.TranslateDefaultQuotes(Locations.SimplifiedSceneNames[Portal.Portal1.Scene])} [arrow_up]"); }

                    if (Portal.Portal1.SceneDestinationTag == "Overworld Redux, Ruins Passage_west")
                    { HintMessages.Add("Ruined Hall Sign", $"{Translations.TranslateDefaultQuotes(Locations.SimplifiedSceneNames[Portal.Portal2.Scene])} [arrow_right]"); }
                    if (Portal.Portal2.SceneDestinationTag == "Overworld Redux, Ruins Passage_west")
                    { HintMessages.Add("Ruined Hall Sign", $"{Translations.TranslateDefaultQuotes(Locations.SimplifiedSceneNames[Portal.Portal1.Scene])} [arrow_right]"); }

                    if (Portal.Portal1.SceneDestinationTag == "Overworld Redux, Overworld Interiors_house")
                    { HintMessages.Add("Town Sign", $"[arrow_left]  {Translations.TranslateDefaultQuotes(Locations.SimplifiedSceneNames[Portal.Portal2.Scene])}"); }
                    if (Portal.Portal2.SceneDestinationTag == "Overworld Redux, Overworld Interiors_house")
                    { HintMessages.Add("Town Sign", $"[arrow_left] {Translations.TranslateDefaultQuotes(Locations.SimplifiedSceneNames[Portal.Portal1.Scene])}"); }

                    if (Portal.Portal1.SceneDestinationTag == "East Forest Redux, Sword Access_lower") {
                        HintMessages.Add("East East Forest Sign", $"{Translations.TranslateDefaultQuotes(Locations.SimplifiedSceneNames[Portal.Portal2.Scene])} [arrow_right]");
                    }
                    if (Portal.Portal2.SceneDestinationTag == "East Forest Redux, Sword Access_lower") {
                        HintMessages.Add("East East Forest Sign", $"{Translations.TranslateDefaultQuotes(Locations.SimplifiedSceneNames[Portal.Portal1.Scene])} [arrow_right]");
                    }

                    if (Portal.Portal1.SceneDestinationTag == "East Forest Redux, East Forest Redux Laddercave_lower") {
                        HintMessages.Add("West East Forest Sign", $"[arrow_left] {Translations.TranslateDefaultQuotes(Locations.SimplifiedSceneNames[Portal.Portal2.Scene])}");
                    }
                    if (Portal.Portal2.SceneDestinationTag == "East Forest Redux, East Forest Redux Laddercave_lower") {
                        HintMessages.Add("West East Forest Sign", $"[arrow_left] {Translations.TranslateDefaultQuotes(Locations.SimplifiedSceneNames[Portal.Portal1.Scene])}");
                    }
                }
            }
        }

        private static List<(string, string)> CreateHeroRelicHints() {
            List<ItemData> Relics = ItemLookup.Items.Values.Where(item => item.Type == ItemTypes.RELIC).ToList();
            List<(string, string)> RelicHints = new List<(string, string)>();
            string Scene = "";
            string Prefix = "";
            string RelicHint = "";
            foreach (ItemData Relic in Relics) {
                string itemDisplayText = $"{TextBuilderPatches.ItemNameToAbbreviation[Relic.Name]}  {(TunicRandomizer.Settings.UseTrunicTranslations ? Translations.Translate(ItemLookup.BonusUpgrades[Relic.ItemNameForInventory].CustomPickupMessage, false) : ItemLookup.BonusUpgrades[Relic.ItemNameForInventory].CustomPickupMessage.ToUpper())}";

                if (IsArchipelago()) {
                    int Player = Archipelago.instance.GetPlayerSlot();
                    ArchipelagoHint RelicItemHint = Locations.MajorItemLocations[Relic.Name][0];

                    if (RelicItemHint.Player == Player) {
                        Scene = RelicItemHint.Location == "Your Pocket" ? RelicItemHint.Location : Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[RelicItemHint.Location]].Location.SceneName];

                        Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                        RelicHint = $"lehjehnd sehz #uh  {itemDisplayText}\nkahn bE fownd aht {Prefix} {(TunicRandomizer.Settings.UseTrunicTranslations ? Translations.Translate(Scene, false) + "." : $"\"{Scene.ToUpper()}.\"")}";
                    } else if (Archipelago.instance.IsTunicPlayer((int)RelicItemHint.Player)) {
                        Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[RelicItemHint.Location]].Location.SceneName];
                        Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                        RelicHint = $"lehjehnd sehz #uh  {itemDisplayText}\nkahn bE fownd aht {Prefix} {(TunicRandomizer.Settings.UseTrunicTranslations ? Translations.Translate(Scene, false) : $"\"{Scene.ToUpper()}\"")}\nin \"{Archipelago.instance.GetPlayerName((int)RelicItemHint.Player).ToUpper()}'S WORLD.\"";
                    } else {
                        RelicHint = $"lehjehnd sehz #uh  {itemDisplayText}\nkahn bE fownd in \"{Archipelago.instance.GetPlayerName((int)RelicItemHint.Player).ToUpper()}'S WORLD\"\naht {WordWrapString($"\"{RelicItemHint.Location.Replace("_", " ").Replace(" ", "\" \"").ToUpper()}\"").Replace("\" \"", " ")}.";
                    }
                    string slotLocation = RelicItemHint.Player == Player && RelicItemHint.Location == "Your Pocket" ? "0, Server" : $"{RelicItemHint.Player}, {RelicItemHint.Location}";
                    RelicHints.Add((slotLocation, RelicHint));
                } else if (IsSinglePlayer()) {
                    Check RelicCheck = ItemRandomizer.FindRandomizedItemByName(Relic.ItemNameForInventory);
                    Scene = Locations.SimplifiedSceneNames[RelicCheck.Location.SceneName];
                    Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";

                    RelicHint = $"lehjehnd sehz #uh  {itemDisplayText}\nkahn bE fownd aht {Prefix} {(TunicRandomizer.Settings.UseTrunicTranslations ? Translations.Translate(Scene, false) + "." : $"\"{Scene.ToUpper()}.\"")}";
                    RelicHints.Add(($"{RelicCheck.Location.LocationId} [{RelicCheck.Location.SceneName}]", RelicHint));
                }
            }

            return RelicHints;
        }

        public static (bool, bool, bool, bool) CreateSinglePlayerMailboxHint(System.Random random) {
            string Scene = "";
            string Prefix = "";
            string HintMessage = "";
            List<string> MailboxItems = new List<string>() { "Stick", "Sword", "Sword Progression", "Stundagger", "Techbow", "Wand", "Lantern", "Shotgun", "Mask" };
            if (SaveFile.GetInt("randomizer shuffled abilities") == 1 && SaveFile.GetInt(HexagonQuestEnabled) != 1) {
                MailboxItems.Add("12");
                MailboxItems.Add("21");
            }
            // if in single player with ladder shuffle, these ladders are probably the most useful for getting you out of sphere 1. Well ladder isn't included because of the weapon requirement for Well
            if (SaveFile.GetInt(SaveFlags.LadderRandoEnabled) == 1) {
                MailboxItems.AddRange(new List<string> { "Ladders in Overworld Town", "Ladders near Weathervane", "Ladders near Overworld Checkpoint", "Ladder to Swamp" });
            }
            if (SaveFile.GetInt(SaveFlags.SwordProgressionEnabled) == 0 && SaveFile.GetInt("randomizer started with sword") == 1) {
                MailboxItems.Remove("Sword");
                MailboxItems.Remove("Stick");
            }
            List<Check> mailboxHintables = new List<Check>();
            foreach (string Item in MailboxItems) {
                mailboxHintables.AddRange(ItemRandomizer.FindAllRandomizedItemsByName(Item));
            }
            Shuffle(mailboxHintables, random);
            int n = 0;
            Check HintItem = null;
            while (HintItem == null && n < mailboxHintables.Count) {
                if (mailboxHintables[n].Location.reachable(ItemRandomizer.SphereZero)) {
                    HintItem = mailboxHintables[n];
                }
                n++;
            }
            if (HintItem == null) {
                n = 0;
                while (HintItem == null && n < mailboxHintables.Count) {
                    if (mailboxHintables[n].Location.SceneName == "Trinket Well") {
                        foreach (Check itemData in ItemRandomizer.FindAllRandomizedItemsByName("Trinket Coin")) {
                            if (itemData.Location.reachable(ItemRandomizer.SphereZero)) {
                                HintItem = itemData;
                            }
                        }
                    } else if (mailboxHintables[n].Location.SceneName == "Waterfall") {
                        foreach (Check itemData in ItemRandomizer.FindAllRandomizedItemsByType("Fairy")) {
                            if (itemData.Location.reachable(ItemRandomizer.SphereZero)) {
                                HintItem = itemData;
                            }
                        }
                    } else if (mailboxHintables[n].Location.SceneName == "Overworld Interiors" && SaveFile.GetInt("randomizer entrance rando enabled") == 0) {
                        Check itemData = ItemRandomizer.FindRandomizedItemByName("Key (House)");
                        if (itemData.Location.reachable(ItemRandomizer.SphereZero)) {
                            HintItem = itemData;
                        }
                    } else if (mailboxHintables[n].Location.LocationId == "71" || mailboxHintables[n].Location.LocationId == "73") {
                        foreach (Check itemData in ItemRandomizer.FindAllRandomizedItemsByName("Key")) {
                            if (itemData.Location.reachable(ItemRandomizer.SphereZero)) {
                                HintItem = itemData;
                            }
                        }
                    }
                    n++;
                }
            }
            if (HintItem == null) {
                HintMessage = "nO lehjehnd forsaw yor uhrIvuhl, rooin sEkur.\nyoo hahv uh difikuhlt rOd uhhehd. \"GOOD LUCK\"!";
                //TrunicHint = HintMessage;
            } else {
                Scene = Locations.SimplifiedSceneNames[HintItem.Location.SceneName];
                Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                HintMessage = $"lehjehnd sehz {Prefix} \"{Scene.ToUpper()}\"\nkuhntAnz wuhn uhv mehnE \"<#00FFFF>FIRST STEPS<#ffffff>\" ahn yor jurnE.";
                if (TunicRandomizer.Settings.UseTrunicTranslations) {
                    HintMessage = $"lehjehnd sehz {Prefix} {Translations.Translate(Scene, false)}\nkuhntAnz wuhn uhv mehnE <#00FFFF>furst stehps<#ffffff> ahn yor jurnE.";
                }
                SaveFile.SetString("randomizer mailbox hint location", $"{HintItem.Location.LocationId} [{HintItem.Location.SceneName}]");

            }
            HintMessages.Add("Mailbox", HintMessage);
            return HintItem == null ? (false, false, false, false) : (HintItem.Reward.Name == "Techbow", HintItem.Reward.Name == "Wand", HintItem.Reward.Name == "12", HintItem.Reward.Name == "21");
        }

        public static void CreateAPMailboxHint(System.Random random) {
            string Scene = "";
            string Prefix = "";
            string Hint = "";
            int Player = Archipelago.instance.GetPlayerSlot();
            List<string> MailboxItems = new List<string>() { "Stick", "Sword", "Sword Upgrade", "Magic Dagger", "Magic Wand", "Magic Orb", "Lantern", "Gun", "Scavenger Mask", "Pages 24-25 (Prayer)", "Pages 42-43 (Holy Cross)" };
            if (SaveFile.GetInt(SaveFlags.LadderRandoEnabled) == 1) {
                MailboxItems.AddRange(new List<string> { "Ladders in Overworld Town", "Ladders near Weathervane", "Ladders near Overworld Checkpoint", "Ladder to Swamp" });
            }
            if (SaveFile.GetInt(SaveFlags.SwordProgressionEnabled) == 0 && SaveFile.GetInt("randomizer started with sword") == 1) {
                MailboxItems.Remove("Sword");
                MailboxItems.Remove("Stick");
            }
            Dictionary<string, ArchipelagoItem> SphereOnePlayer = new Dictionary<string, ArchipelagoItem>();
            Dictionary<string, ArchipelagoItem> SphereOneOthersTunic = new Dictionary<string, ArchipelagoItem>();
            Dictionary<string, ArchipelagoItem> SphereOneOthers = new Dictionary<string, ArchipelagoItem>();
            ItemRandomizer.PopulatePrecollected();
            // StartInventoryItems is populated with your start inventory items, which are items with a location ID of -2
            Dictionary<string, int> StartInventoryAndPrecollected = ItemRandomizer.AddListToDict(Archipelago.instance.integration.GetStartInventory(), ItemRandomizer.PrecollectedItems);
            if (SaveFile.GetInt(EntranceRando) == 1) {
                ItemRandomizer.SphereZero = ItemRandomizer.GetERSphereOne(StartInventoryAndPrecollected);
            } else {
                TunicPortals.RandomizedPortals = TunicPortals.VanillaPortals();
                ItemRandomizer.SphereZero = ItemRandomizer.GetSphereOne(StartInventoryAndPrecollected);
            }
            foreach (string itemkey in ItemLookup.ItemList.Keys) {
                ArchipelagoItem item = ItemLookup.ItemList[itemkey];
                if (Archipelago.instance.IsTunicPlayer(item.Player) && MailboxItems.Contains(item.ItemName)) {
                    var requirements = Locations.VanillaLocations[itemkey].Location.Requirements[0];
                    foreach (KeyValuePair<string, int> req in requirements) {
                        int checkCount = 0;
                        if (ItemRandomizer.SphereZero.Keys.Contains(req.Key) && ItemRandomizer.SphereZero[req.Key] >= req.Value) {
                            checkCount++;
                        } else {
                            continue;
                        }
                        if (checkCount == requirements.Count) {
                            if (item.Player == Archipelago.instance.GetPlayerSlot()) {
                                SphereOnePlayer.Add(itemkey, item);
                            } else {
                                SphereOneOthersTunic.Add(itemkey, item);
                            }
                        }
                    }
                } else if (item.Player != Archipelago.instance.GetPlayerSlot() && item.Classification == ItemFlags.Advancement) {
                    var requirements = Locations.VanillaLocations[itemkey].Location.Requirements[0];
                    foreach (KeyValuePair<string, int> req in requirements) {
                        int checkCount = 0;
                        if (ItemRandomizer.SphereZero.Keys.Contains(req.Key) && ItemRandomizer.SphereZero[req.Key] >= req.Value) {
                            checkCount++;
                        } else {
                            continue;
                        }
                        if (checkCount == requirements.Count) { 
                            SphereOneOthers.Add(itemkey, item); 
                        }
                    }
                }
            }
            ArchipelagoItem mailboxitem = null;
            string key = "";
            if (SphereOnePlayer.Count > 0) {
                key = SphereOnePlayer.Keys.ToList()[random.Next(SphereOnePlayer.Count)];
                mailboxitem = SphereOnePlayer[key];
            } else if (SphereOneOthersTunic.Count > 0) {
                key = SphereOneOthersTunic.Keys.ToList()[random.Next(SphereOneOthersTunic.Count)];
                mailboxitem = SphereOneOthersTunic[key];
            } else if (SphereOneOthers.Count > 0) {
                key = SphereOneOthers.Keys.ToList()[random.Next(SphereOneOthers.Count)];
                mailboxitem = SphereOneOthers[key];
            }
            if (mailboxitem != null) {
                Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[key].Location.SceneName];
                Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                SaveFile.SetString("randomizer mailbox hint location", key);
                Hint = $"lehjehnd sehz {Prefix} \"{Scene.ToUpper()}\"\nkuhntAnz wuhn uhv mehnE \"<#00FFFF>FIRST STEPS<#ffffff>\" ahn yor jurnE.";
                if (TunicRandomizer.Settings.UseTrunicTranslations) {
                    Hint = $"lehjehnd sehz {Prefix} {Translations.Translate(Scene, false)}\nkuhntAnz wuhn uhv mehnE <#00FFFF>furst stehps<#ffffff> ahn yor jurnE.";
                }
            } else {
                SaveFile.SetString("randomizer mailbox hint location", "no first steps");
                Hint = $"yor frehndz muhst furst hehlp yoo fInd yor wA...\ngoud luhk, rooin sEkur.";
            }
            HintMessages.Add("Mailbox", Hint);
        }

        private static void Shuffle(List<Check> list, System.Random random) {
            int n = list.Count;
            int r;
            while (n > 1) {
                n--;
                r = random.Next(n + 1);

                Check holder = list[r];
                list[r] = list[n];
                list[n] = holder;
            }
        }

        public static string WordWrapString(string Hint) {
            string formattedHint = "";

            int length = 40;
            foreach (string split in Hint.Split(' ')) {
                string split2 = split;
                if (split.StartsWith($"\"") && !split.EndsWith($"\"")) {
                    split2 += $"\"";
                } else if (split.EndsWith($"\"") && !split.StartsWith($"\"")) {
                    split2 = $"\"{split2}";
                }
                if ((formattedHint + split2).Length < length) {
                    formattedHint += split2 + " ";
                } else {
                    formattedHint += split2 + "\n";
                    length += 40;
                }
            }

            return formattedHint;
        }

        public static void SetupHeroGraveToggle() {
            if (Hints.HeroGraveHints.Values.Where(hint => hint.SceneName == SceneManager.GetActiveScene().name).Any()) {
                HeroGraveHint hint = Hints.HeroGraveHints.Values.Where(hintgrave => hintgrave.SceneName == SceneManager.GetActiveScene().name).First();
                GameObject relicPlinth = GameObject.Find(hint.GraveObjectPath);
                if (relicPlinth.GetComponent<HeroGraveToggle>() == null) {
                    relicPlinth.AddComponent<HeroGraveToggle>().heroGravehint = hint;
                } else {
                    relicPlinth.GetComponent<HeroGraveToggle>().heroGravehint = hint;
                }
            }
        }
    }
}
