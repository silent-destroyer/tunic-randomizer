using System.Collections.Generic;

namespace TunicRandomizer {
    public class DeathLinkMessages {

        public static List<string> SecondaryMessages = new List<string>() { 
            $"wuht wur #A %inki^?",
            $"tIm for rEvehnj",
            $"how koud #A?",
            $"hOp Im nawt nehkst...",
        };

        public static Dictionary<string, List<string>> Causes = new Dictionary<string, List<string>>() {
            {
                "Generic",
                new List<string>() {
                    "'s prayer went unheard.",
                    " didn't share their wisdom.",
                    "'s HP fell to 0.",
                    " sought their own ruin.",
                    " didn't follow the Golden Path.",
                }
            },
            {
                "Fortress Arena",
                new List<string>() {
                    " was Sieged by the Engine."
                }
            },
            {
                "Fortress Basement",
                new List<string>() {
                    " got lost in the dark.",
                }
            },
            {
                "Library Exterior",
                new List<string>() {
                    " didn't return their libray books."
                }
            },
            {
                "Library Hall",
                new List<string>() {
                    " didn't return their libray books."
                }
            },
            {
                "Library Rotunda",
                new List<string>() {
                    " didn't return their libray books."
                }
            },
            {
                "Library Arena",
                new List<string>() { 
                    " died to The Librarian",
                }
            },
            {
                "Quarry",
                new List<string>() {
                    " died in a secret place.",
                    " withered away from the miasma."
                }
            },
            {
                "Quarry Redux",
                new List<string>() {
                    " withered away from the miasma.",
                }
            },
            {
                "Mountain",
                new List<string>() {
                    " didn't follow the Golden Path."
                }
            },
            {
                "Mountaintop",
                new List<string>() {
                    " didn't follow the Golden Path."
                }
            },
            {
                "Swamp Redux 2",
                new List<string>() {
                    " joined the skeletons.",
                    " dug their own grave.",
                    " became a ghost.",
                }
            },
            {
                "Crypt Redux",
                new List<string>() {
                    " got lost in the dark.",
                }
            },
            {
                "Crypt",
                new List<string>() {
                    " died in a secret place."
                }
            },
            {
                "Posterity",
                new List<string>() {
                    " died in a secret place."
                }
            },
            {
                "Purgatory",
                new List<string>() {
                    " was trapped in purgatory."
                }
            }
        };

        public static Dictionary<string, string> HitTriggerCauses = new Dictionary<string, string>() {
            { "voidtouched", " was touched by the void." },
            { "woodcutter", " was turned to lumber." },
            { "centipede", " was too afraid of centipedes." },
            { "administrator", " tried praying to an administrator." },
            { "crow_voidtouched", " was hushed." },
            { "bomezome_easy", " could not flee the fleemers." },
            { "scavenger", " was sniped by a Scavenger." },
            { "scavenger_miner", " was mined by a Scavenger" },
            { "scavenger_stunner", " was frozen by a Scavenger." },
            { "spidertank", " was Sieged by the Engine." },
            { "wizard_extinguished", " was cleaned up by a custodian." },
            { "wizard", " was cleaned up by a custodian." },
            { "spider_big", " fell for the spider's trick." },
            { "spider_small", " was fooled by a spider." },
            { "spinnerbot", " was swarmed by a slorm." },
            { "crow", " was hushed." },
            { "crocodoo", " was chomped by Terry." },
            { "crocodoo_voidtouched", " was chomped by Terry's cousin." },
            { "foxgod_explosion", " didn't share their wisdom." },
            { "foxgod_voidhole", " didn't share their wisdom." },
            { "foxgod_bullet", " didn't share their wisdom." },
            { "foxgod", " didn't share their wisdom." },
            { "foxgod_shockwave", " didn't follow the Golden Path." },
            { "foxgod_phase2", " didn't follow the Golden Path." },
            { "fox_techbolt_stun", " froze themselves." },
            { "fox_techbolt", " hit themselves with their own wand." },
            { "wizard_bolt", " was cleaned up by a custodian." },
            { "techbolt_stun", " was frozen by a fairy." },
            { "fire", " was on fire (literally)." },
            { "explosion", " blew up." },
            { "spiketrap", " stepped on a spike trap." },
            { "spidertank_bolt", " was hit by a laser beam." }
        };

        public static Dictionary<string, string> HitTriggerDescriptions = new Dictionary<string, string>() {
            { "shotgun", " a shotgun." },
            { "ghostknight", " a ghost knight." },
            { "phage_spin", " a slorm...?" },
            { "phage", " a slorm...?" },
            { "zombie", " a past ruin seeker." },
            { "zombieFast", " a seeker of ruin." },
            { "voidtouched", " a voidtouched." },
            { "woodcutter", " a woodcutter." },
            { "scavengerBoss_kick", " a kick from the Boss Scavenger." },
            { "scavengerBoss", " the Boss Scavenger." },
            { "centipede", " a centipede." },
            { "administrator", " an administrator." },
            { "crow_voidtouched", " a corrupted husher." },
            { "tentacle", " a tentacle." },
            { "gost", " a lost echo." },
            { "bomezome_easy", " a fleemer." },
            { "scavenger", " a sniper scavenger." },
            { "scavenger_miner", " a mining scavenger." },
            { "scavenger_stunner", " a sniper scavenger." },
            { "bomezome", " a fleemer." },
            { "shadowreaper", " the shadowreaper." },
            { "spidertank", " the Siege Engine." },
            { "wizard_extinguished", " a custodian." },
            { "wizard", " a custodian." },
            { "spider_big", " a sappharach." },
            { "spider_small", " a spyrite." },
            { "frog", " a frog." },
            { "frog_small", " a frog." },
            { "spinnerbot", " a slorm." },
            { "crabbit", " a crabbit." },
            { "crabbo", " a crabbo." },
            { "crow", " a husher." },
            { "knightbot", " the Garden Knight." },
            { "tunicknight_void", " the Garden Knight...?" },
            { "crocodoo", " Terry." },
            { "crocodoo_voidtouched", " Terry's cousin." },
            { "foxgod_explosion", " The Heir." },
            { "foxgod_voidhole", " The Heir." },
            { "foxgod_bullet", " The Heir." },
            { "foxgod", " The Heir." },
            { "foxgod_shockwave", " The Heir." },
            { "foxgod_phase2", " The Heir." },
            { "librarian", " The Librarian." },
            { "librarian_pushback", " The Librarian." },
            { "spider_librarianAdd", " a pawn of The Librarian." },
            { "ghost_skuladotBig", " the ghost of a Guard Captain." },
            { "ghost_skuladot", " the ghost of a rudeling." },
            { "ghost_frog_spear", " the ghost of a frog." },
            { "ghost_frog_small", " the ghost of a frog." },
            { "ghost_knightbot", " the ghost of a Garden Knight." },
            { "bomezome_big", " a giant fleemer." },
            { "beefboy", " a beefy boy." },
            { "honourguard", " an envoy." },
            { "skuladotBig", " the Guard Captain." },
            { "skuladot", " a rudeling." },
            { "bat", " a phrend." },
            { "hedgehogBig", " a hedgehog." },
            { "hedgehog", " a hedgehog." },
            { "blobBig", " a blob." },
            { "blob", " a blob." },
            { "techbolt", " an autobolt turret." },
            { "hedgehog_spine", " a hedgehog spike." },
            { "gunslinger_bullet", " a gunslinger." },
            { "wizard_bolt", " a custodian." },
            { "spidertank_bolt", " a laser beam." },
            { "techbolt_stun", " a fairy." },
            { "knightbot_bullet", " the Garden Knight." },
            { "voidling", " a voidling." },
            { "voidling 1", " a voidling." },
            { "administrator_angry", " an angered administrator." },
            { "fire", " fire." },
            { "explosion", " an explosion." },
            { "spiketrap", " a spike trap." },
        };

        public static List<string> GenericMessages = new List<string>() {
            " was killed by",
            " was defeated by",
            " died to",
            " was no match for",
        };
    }
}
