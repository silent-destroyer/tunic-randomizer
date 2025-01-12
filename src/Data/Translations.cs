﻿using System.Collections.Generic;

namespace TunicRandomizer {
    public class Translations {

        public static string Translate(string Input, bool CheckSetting) {

            return ((CheckSetting ? TunicRandomizer.Settings.UseTrunicTranslations : true) && EnglishToTrunic.ContainsKey(Input)) ? EnglishToTrunic[Input] : Input.StartsWith($"\"") ? Input : $"\"{Input}\"";
        }

        public static string TranslateDefaultNoQuotes(string Input) {

            return (TunicRandomizer.Settings.UseTrunicTranslations && EnglishToTrunic.ContainsKey(Input)) ? EnglishToTrunic[Input] : Input;
        }

        public static string TranslateDefaultNoQuotes(string Input, bool CheckSetting) {

            return ((CheckSetting ? TunicRandomizer.Settings.UseTrunicTranslations : true) && EnglishToTrunic.ContainsKey(Input)) ? EnglishToTrunic[Input] : Input;
        }

        public static string TranslateDefaultQuotes(string Input) {
            return (TunicRandomizer.Settings.UseTrunicTranslations && EnglishToTrunic.ContainsKey(Input)) ? EnglishToTrunic[Input] : $"\"{Input}\"";
        }

        public static Dictionary<string, string> EnglishToTrunic = new Dictionary<string, string>() {
            {"Firecracker", "fIurkrahkur"},
            {"Fire Bomb", "fIur bawm"},
            {"Ice Bomb", "Is bawm"},
            {"Lure", "lour"},
            {"Pepper", "pehpur"},
            {"Ivy", "IvE"},
            {"Effigy", "ehfijE"},
            {"Money", "muhnE"},
            {"HP Berry", "Aj pE bArE"},
            {"MP Berry", "ehm pE bArE"},
            {"Fool Trap", "fool trahp"},
            {"Stick", "stik"},
            {"Sword", "sord"},
            {"Sword Upgrade", "sord uhpgrAd"},
            {"Magic Dagger", "mahjik dahgur"},
            {"Magic Wand", "mahjik wawnd"},
            {"Magic Orb", "mahjik orb"},
            {"Hero's Laurels", "hErOz loruhlz"},
            {"Lantern", "lahnturn"},
            {"Shotgun", "$awtguhn"},
            {"Gun", "guhn"},
            {"Shield", "$Eld"},
            {"Dath Stone", "dah% stOn"},
            {"Torch", "torJ"},
            {"Hourglass", "owurglahs"},
            {"Old House Key", "Old hows kE"},
            {"Key", "kE"},
            {"Fortress Vault Key", "fOrtris vawlt kE"},
            {"Flask Shard", "flahsk #Rd"},
            {"Potion Flask", "pO$uhn flahsk"},
            {"Golden Coin", "gOldin koin"},
            {"Card Slot", "kRd slawt"},
            {"Red Hexagon", "rehd kwehstuhgawn"},
            {"Green Hexagon", "grEn kwehstuhgawn"},
            {"Blue Hexagon", "bloo kwehstuhgawn"},
            {"Gold Hexagon", "gOld kwehstuhgawn"},
            {"ATT Offering", "uhtahk awfuri^"},
            {"DEF Offering", "difehns awfuri^"},
            {"Potion Offering", "pO$uhn awfuri^"},
            {"HP Offering", "Aj pE awfuri^"},
            {"SP Offering", "ehs pE awfuri^"},
            {"MP Offering", "ehm pE awfuri^"},
            {"Hero Relic - ATT", $"hErO rehlik \"-\" uhtahk"},
            {"Hero Relic - DEF", $"hErO rehlik \"-\" difehns"},
            {"Hero Relic - POTION", $"hErO rehlik \"-\" pO$uhn"},
            {"Hero Relic - HP", $"hErO rehlik \"-\" Aj pE"},
            {"Hero Relic - SP", $"hErO rehlik \"-\" ehs pE"},
            {"Hero Relic - MP", $"hErO rehlik \"-\" ehm pE"},
            {"Orange Peril Ring", "oruhnj pAruhl ri^"},
            {"Tincture", "ti^kJur"},
            {"Scavenger Mask", "skahvuhnjur mahsk"},
            {"Cyan Peril Ring", "sIahn pAruhl ri^"},
            {"Bracer", "brAsur"},
            {"Dagger Strap", "dahgur strahp"},
            {"Inverted Ash", "invurtid ah$"},
            {"Lucky Cup", "luhkE kuhp"},
            {"Magic Echo", "mahjik ehkO"},
            {"Anklet", "ah^klit"},
            {"Muffling Bell", "muhfli^ behl"},
            {"Glass Cannon", "glahs kahnuhn"},
            {"Perfume", "purfyoom"},
            {"Louder Echo", "lowdur ehkO"},
            {"Aura's Gem", "oruhz jehm"},
            {"Bone Card", "bOn kRd"},
            {"Fairy", "fArE"},
            {"Mr Mayor", "mistur mAor"},
            {"Secret Legend", "sEkrit lehjehnd"},
            {"Sacred Geometry", "sAkrid jEawmuhtrE"},
            {"Vintage", "vintij"},
            {"Just Some Pals", "juhst suhm pahlz"},
            {"Regal Weasel", "rEguhl wEzuhl"},
            {"Spring Falls", "spri^ fawlz"},
            {"Power Up", "powur uhp"},
            {"Back To Work", "bahk too wurk"},
            {"Phonomath", "fOnuhmah%"},
            {"Dusty", "duhstE"},
            {"Forever Friend", "forehvur frehnd"},
            {"Pages 0-1", $"pAjuhz \"0-1\""},
            {"Pages 2-3", $"pAjuhz \"2-3\""},
            {"Pages 4-5", $"pAjuhz \"4-5\""},
            {"Pages 6-7", $"pAjuhz \"6-7\""},
            {"Pages 8-9", $"pAjuhz \"8-9\""},
            {"Pages 10-11", $"pAjuhz \"10-11\""},
            {"Pages 12-13", $"pAjuhz \"12-13\""},
            {"Pages 14-15", $"pAjuhz \"14-15\""},
            {"Pages 16-17", $"pAjuhz \"16-17\""},
            {"Pages 18-19", $"pAjuhz \"18-19\""},
            {"Pages 20-21", $"pAjuhz \"20-21\""},
            {"Pages 22-23", $"pAjuhz \"22-23\""},
            {"Pages 24-25 (Prayer)", $"pAjuhz \"24-25\" (prAr)"},
            {"Pages 26-27", $"pAjuhz \"26-27\""},
            {"Pages 28-29", $"pAjuhz \"28-29\""},
            {"Pages 30-31", $"pAjuhz \"30-31\""},
            {"Pages 32-33", $"pAjuhz \"32-33\""},
            {"Pages 34-35", $"pAjuhz \"34-35\""},
            {"Pages 36-37", $"pAjuhz \"36-37\""},
            {"Pages 38-39", $"pAjuhz \"38-39\""},
            {"Pages 40-41", $"pAjuhz \"40-41\""},
            {"Pages 42-43 (Holy Cross)", $"pAjuhz \"42-43\" (hOlE kraws)"},
            {"Pages 44-45", $"pAjuhz \"44-45\""},
            {"Pages 46-47", $"pAjuhz \"46-47\""},
            {"Pages 48-49", $"pAjuhz \"48-49\""},
            {"Pages 50-51", $"pAjuhz \"50-51\""},
            {"Pages 52-53 (Icebolt)", $"pAjuhz \"52-53\" (IsbOlt)"},
            {"Pages 54-55", $"pAjuhz \"54-55\""},
            {"Ladders in Overworld Town", "Ovurwurld town lahdurz"},
            {"Ladders near Weathervane", "lahdurz nEr weh#urvAn"},
            {"Ladders near Overworld Checkpoint", "Ovurwurld Jehkpoint lahdurz"},
            {"Ladder to East Forest", "lahdur too Est foruhst"},
            {"Ladders to Lower Forest", "lahdur too lOur Est foruhst"},
            {"Ladders near Patrol Cave", "lahdurz nEr puhtrOl kAv"},
            {"Ladders in Well", "lahdurz in wehl"},
            {"Ladders to West Bell", "lahdurz too wehst behl"},
            {"Ladder to Quarry", "lahdur too kworE"},
            {"Ladder in Dark Tomb", "dRk toom lahdur"},
            {"Ladders near Dark Tomb", "lahdurz nEr dRk toom"},
            {"Ladder near Temple Rafters", "lahdur nEr tehmpuhl rahfturz"},
            {"Ladder to Swamp", "lahdur too swawmp"},
            {"Ladders in Swamp", "lahdurz in swawmp"},
            {"Ladder to Ruined Atoll", "lahdur too rooind ahtawl"},
            {"Ladders in South Atoll", "sow% ahtawl lahdurz"},
            {"Ladders to Frog's Domain", "lahdurz too frawgz dOmAn"},
            {"Ladders in Hourglass Cave", "owurglahs kAv lahdurz"},
            {"Ladder to Beneath the Vault", "lahdur bEnE% #uh fortruhs"},
            {"Ladders in Lower Quarry", "lahdurz in lOur kworE"},
            {"Ladders in Library", "lahdurz in lIbrArE"},
            {"Grass", "grahs"},
            {"Overworld", "Ovurwurld"},
            {"West Furnace", "wehst furnis"},
            {"Cube Cave", "kyoob kAv"},
            {"Stick House", "stik hows"},
            {"Windmill", "windmil"},
            {"Southeast Cross Room", "sow%Est kraws room"},
            {"Caustic Light Cave", "kawstik lIt kAv"},
            {"Ruined Passage", "rooind pahsuhj"},
            {"Patrol Cave", "puhtrOl kAv"},
            {"Secret Gathering Place", "sEkrit gah#uri^ plAs"},
            {"Fountain Cross Room", "fowntin kraws room"},
            {"Hourglass Cave", "owurglahs kAv"},
            {"Maze Cave", "mAz kAv"},
            {"Ruined Shop", "rooind $awp"},
            {"Changing Room", "JAnji^ room"},
            {"Special Shop", "speh$uhl $awp"},
            {"Old House", "Old hows"},
            {"Far Shore", "fR $or"},
            {"Sealed Temple", "sEld tehmpuhl"},
            {"Shop", "$awp"},
            {"Coins in the Well", "koinz in #uh wehl"},
            {"Forest Belltower", "foruhst behltowur"},
            {"East Forest", "Est foruhst"},
            {"Forest Grave Path", "foruhst grAv pah%"},
            {"Guardhouse 2", "gRdhows 2"},
            {"Guardhouse 1", "gRdhows 1"},
            {"Forest Boss Room", "forist baws room"},
            {"Bottom of the Well", "bawtuhm uhv #uh wehl"},
            {"Dark Tomb Checkpoint", "dRk toom Jehkpoint"},
            {"Dark Tomb", "dRk toom"},
            {"West Garden", "wehst gRdin"},
            {"West Garden House", "wehst gRdin hows"},
            {"Ruined Atoll", "rooind ahtawl"},
            {"Frog Stairway", "frawg stArwA"},
            {"Frog's Domain", "frawgz dOmAn"},
            {"Library Exterior", "lIbrArE ehkstErEur"},
            {"Library Hall", "lIbrArE hawl"},
            {"Library Rotunda", "lIbrArE rOtuhnduh"},
            {"Library Lab", "lIbrArE lahb"},
            {"Librarian", "lIbrArEuhn"},
            {"Beneath the Fortress", "bEnE% #uh fortruhs"},
            {"Eastern Vault Fortress", "Esturn vawlt fortruhs"},
            {"Fortress East Shortcut", "fortruhs Est $ortkuht"},
            {"Fortress Grave Path", "fortruhs grAv pah%"},
            {"Fortress Courtyard", "fortruhs kortyRd"},
            {"Fortress Leaf Piles", "fortruhs lEf pIlz"},
            {"Fortress Arena", "fortruhs uhrEnuh"},
            {"Lower Mountain", "lOur mowntin"},
            {"Top of the Mountain", "tawp uhv #uh mowntin"},
            {"Quarry Entryway", "kworE ehntrEwA"},
            {"Quarry", "kworE"},
            {"Monastery", "mawnuhstArE"},
            {"Rooted Ziggurat Entrance", "ziguraht ehntruhns"},
            {"Rooted Ziggurat Upper", "uhpur ziguraht"},
            {"Rooted Ziggurat Tower", "ziguraht towur"},
            {"Rooted Ziggurat Lower", "lOur ziguraht"},
            {"Rooted Ziggurat Teleporter", "ziguraht tehluhportur"},
            {"Swamp", "swawmp"},
            {"Cathedral", "kuh%Edruhl"},
            {"Cathedral Gauntlet", "kuh%Edruhl gawntluht"},
            {"Hero's Grave", "hErOz grAv"},
            {"Glyph Tower", "glif towur"},
            {"The Heir", "#uh Ar"},
            {"Purgatory", "purguhtorE"},
            {"Your Pocket", "yor pawkit"},
            {"FOUR SKULLS", "for skuhlz"},
            {"EAST FOREST SLIME", "Est foruhst slIm"},
            {"CATHEDRAL GAUNTLET", "kuh%Edruhl gawntluht"},
            {"SIEGE ENGINE", "sEj ehnhjuhn"},
            {"LIBRARIAN", "lIbrArEuhn"},
            {"SCAVENGER BOSS", "skahvuhnjur baws"},
            {"VAULT KEY PLINTH", "vawlt kE plin%"},
            {"20 FAIRIES", "20 fArEz"},
            {"10 COIN TOSSES", "10 koin tawsiz"},
            {"15 COIN TOSSES", "15 koin tawsiz"},
            {"PILES OF LEAVES", "pIlz uhv lEvs"},
            {"WEST GARDEN TREE", "wehst gRdin trE"},
            {"TOP OF THE MOUNTAIN", "tawp uhv #uh mowntin"},
            {"BONUS CUSTOMIZATION Unlocked", "bOnuhs kuhstuhmizA$uhn uhnlawkd"},
            {"PRAYER Unlocked", "prAr uhnlawkd"},
            {"HOLY CROSS Unlocked", "hOlE kraws uhnlawkd"},
            {"ICEBOLT Unlocked", "IsbOlt uhnlawkd"},
            {"\"Blob\"", "blawb"},
            {"\"Hedgehog\"", "hehjhawg"},
            {"\"Rudeling\"", "roodli^"},
            {"\"Plover\"", "plOvur"},
            {"\"Baby Slorm\"", "bAbE slorm"},
            {"\"Crabbit\"", "krahbit"},
            {"gAmkyoob \"Crabbit\"", "gAmkyoob krahbit"},
            {"yoo...\"?\"", "yoo...?"},
            {"\"Blob\" (big)", "blawb (big)"},
            {"\"Blob\" (bigur)", "blawb (bigur)"},
            {"\"Hedgehog\" (big)", "hehjhawg (big)"},
            {"\"Phrend\"", "frehnd"},
            {"\"Spyrite\"", "spIrIt"},
            {"\"Fleemer\"", "flEmur"},
            {"\"Fairy\" (Is)", "fArE (Is)"},
            {"\"Fairy\" (fIur)", "fArE (fIur)"},
            {"\"Rudeling\" ($Eld)", "roodli^ ($Eld)"},
            {"\"Crabbo\"", "krahbO"},
            {"\"Slorm\"", "slorm"},
            {"\"Autobolt\"", "awtObOlt"},
            {"\"Laser Turret\"", "lAzur turit"},
            {"\"Administrator\" (frehnd)", "ahdminuhstrAtur (frehnd)"},
            {"slorm...\"?\"", "slorm...?"},
            {"\"???\"", "???"},
            {"\"Custodian\" (kahnduhlahbruh)", "kuhstOdEuhn (kahnduhlahbruh)"},
            {"\"Tentacle\"", "tehntuhkuhl"},
            {"\"Envoy\"", "awnvoi"},
            {"\"Guard Captain\"", "gRd kahptuhn"},
            {"\"Husher\"", "huh$ur"},
            {"\"Terry\"", "tArE"},
            {"\"Terry\" (void)", "tArE (void)"},
            {"\"Scavenger\" (snIpur)", "skahvuhnjur (snIpur)"},
            {"\"Scavenger\" (mInur)", "skahvuhnjur (mInur)"},
            {"\"Scavenger\" (suhport)", "skahvuhnjur (suhport)"},
            {"\"Scavenger\" (stuhnur)", "skahvuhnjur (stuhnur)"},
            {"\"Fleemer\" (fehnsur)", "flEmur (fehnsur)"},
            {"\"Lost Echo\"", "lawst ehkO"},
            {"\"Voidling\"", "voidli^"},
            {"\"Frog\" (spEr) [frog]", "frawg (spEr) [frog]"},
            {"\"Frog\" [frog]", "frawg [frog]"},
            {"\"Frog\" (smawl) [frog]", "frawg (smawl) [frog]"},
            {"\"Sappharach\"", "sahfuhrahk"},
            {"\"Custodian\"", "kuhstOdEuhn"},
            {"\"Custodian\" (suhport)", "kuhstOdEuhn (suhport)"},
            {"\"Husher\" (void)", "huh$ur (void)"},
            {"\"Woodcutter\"", "woudkuhtur"},
            {"\"You...?\"", "yoo...?"},
            {"\"Administrator\"", "ahdminuhstrAtur"},
            {"\"Gunslinger\"", "guhnsli^ur"},
            {"\"Beefboy\"", "bEfboi"},
            {"\"Fleemer\" (lRj)", "flEmur (lRj)"},
            {"\"Garden Knight...?\"", "gRdin nIt...?"},
            {"gRdin nIt...\"?\"", "gRdin nIt...?"},
            {"\"Voidtouched\"", "voidtuhJd"},
            {"\"Centipede\"", "sehntipEd"},
            {"\"Shadowreaper\"", "$ahdOrEpur"},
            {"\"Phrend\" (void)", "frehnd (void)"},
            {"\"Rudeling\" (void)", "roodli^ (void)"},
            {"\"Frog...?\" (smawl) [frog]", "frawg...? (smawl) [frog]"},
            {"\"Frog...?\" (spEr) [frog]", "frawg...? (spEr) [frog]"},
            {"\"Fairy...?\"", "fArE...?"},
            {"\"Fleemer...?\"", "flEmur...?"},
            {"\"Fleemer...?\" (lRj)", "flEmur...? (lRj)"},
            {"\"Custodian...?\" (suhport)", "kuhstOdEuhn...? (suhport)"},
            {"\"Rudeling...?\"", "roodli^...?"},
            {"\"Rudeling...?\" ($Eld)", "roodli^...? ($Eld)"},
            {"\"Guard Captain...?\"", "gRd kahptuhn...?"},
            {"\"Librarian\"", $"lIbrArEuhn" },
            {"\"Siege Engine\"", $"sEj ehnjin" },
            {"\"Boss Scavenger\"", $"baws skahvehnjur" },
            {"\"Garden Knight\"", $"gRdin nIt" },
            {"\"The Heir\"", $"#E Ar" },
            {"\"Fleemer\" (kworteht)", $"flEmur (kworteht)" },
            {$"kawngrahJoulA$uhnz! \"(<#e99d4c>+1 ATT<#FFFFFF>)\"", $"kawngrahJoulA$uhnz! (<#e99d4c>[arrow_up]1 uhtahk<#FFFFFF>)"},
            {$"kawngrahJoulA$uhnz! \"(<#5de7cf>+1 DEF<#FFFFFF>)\"", $"kawngrahJoulA$uhnz! (<#5de7cf>[arrow_up]1 difehns<#FFFFFF>)"},
            {$"kawngrahJoulA$uhnz! \"(<#ca7be4>+1 POTION<#FFFFFF>)\"", $"kawngrahJoulA$uhnz! (<#ca7be4>[arrow_up]1 pO$uhn<#FFFFFF>)"},
            {$"kawngrahJoulA$uhnz! \"(<#f03c67>+1 HP<#FFFFFF>)\"", $"kawngrahJoulA$uhnz! (<#f03c67>[arrow_up]1 AJ pE<#FFFFFF>)"},
            {$"kawngrahJoulA$uhnz! \"(<#8ddc6e>+1 SP<#FFFFFF>)\"", $"kawngrahJoulA$uhnz! (<#8ddc6e>[arrow_up]1 ehs pE<#FFFFFF>)"},
            {$"kawngrahJoulA$uhnz! \"(<#2a8fed>+1 MP<#FFFFFF>)\"", $"kawngrahJoulA$uhnz! (<#2a8fed>[arrow_up]1 ehm pE<#FFFFFF>)"},
            {$"\"Hero Relic - <#e99d4c>ATT\"", $"hErO rehlik \"-\" <#e99d4c>uhtahk"},
            {$"\"Hero Relic - <#5de7cf>DEF\"", $"hErO rehlik \"-\" <#5de7cf>difehns"},
            {$"\"Hero Relic - <#ca7be4>POTION\"", $"hErO rehlik \"-\" <#ca7be4>pO$uhn"},
            {$"\"Hero Relic - <#f03c67>HP\"", $"hErO rehlik \"-\" <#f03c67>AJ pE"},
            {$"\"Hero Relic - <#8ddc6e>SP\"", $"hErO rehlik \"-\" <#8ddc6e>ehs pE"},
            {$"\"Hero Relic - <#2a8fed>MP\"", $"hErO rehlik \"-\" <#2a8fed>ehm pE"},
        };
    }
}
