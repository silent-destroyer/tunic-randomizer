using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TunicRandomizer {
    public class TextBuilderPatches {

        public static Dictionary<string, int> CustomSpriteIndices = new Dictionary<string, int>() { };

        public static Dictionary<string, string> CustomSpriteIcons = new Dictionary<string, string>() {
            { "[stick]", "Inventory items_stick" },
            { "[realsword]", "Inventory items_sword" },
            { "[librariansword]", "Randomizer items_Librarian Sword" },
            { "[heirsword]", "Randomizer items_Heir Sword" },
            { "[wand]", "Inventory items_techbow" },
            { "[dagger]", "Inventory items_stundagger" },
            { "[orb]", "Inventory items_forcewand" },
            { "[shield]", "Inventory items_shield" },
            { "[gun]", "Inventory items_shotgun" },
            { "[hourglass]", "Inventory items_hourglass" },
            { "[lantern]", "Inventory items_lantern" },
            { "[laurels]", "Inventory items_cape" },
            { "[coin]", "Inventory items_coin question mark" },
            { "[trinket]", "Inventory items_trinketcard" },
            { "[square]", "Inventory items_trinketslot" },
            { "[fairy]", "Inventory items_fairy" },
            { "[mayor]", "Inventory items_trophy" },
            { "[book]", "Inventory items_book" },
            { "[att]", "Inventory items_offering_tooth" },
            { "[def]", "Inventory items_offering_effigy" },
            { "[potion]", "Inventory items_offering_ash" },
            { "[hp]", "Inventory items_offering_flower" },
            { "[sp]", "Inventory items_offering_feather" },
            { "[mp]", "Inventory items_offering_orb" },
            { "[attrelic]", "Randomizer items_Hero Relic - ATT" },
            { "[defrelic]", "Randomizer items_Hero Relic - DEF" },
            { "[potionrelic]", "Randomizer items_Hero Relic - POTION" },
            { "[hprelic]", "Randomizer items_Hero Relic - HP" },
            { "[sprelic]", "Randomizer items_Hero Relic - SP" },
            { "[mprelic]", "Randomizer items_Hero Relic - MP" },
            { "[yellowkey]", "Inventory items_key" },
            { "[housekey]", "Inventory items 3_keySpecial" },
            { "[vaultkey]", "Inventory items_vault key" },
            { "[firecracker]", "Inventory items_firecracker" },
            { "[firebomb]", "Inventory items_firebomb" },
            { "[icebomb]", "Inventory items_icebomb" },
            { "[hpberry]", "Inventory items_berry" },
            { "[mpberry]", "Inventory items_berry_blue" },
            { "[pepper]", "Inventory items_pepper" },
            { "[ivy]", "Inventory items_ivy" },
            { "[lure]", "Inventory items_bait" },
            { "[effigy]", "Inventory items_piggybank" },
            { "[flask]", "Inventory items_potion" },
            { "[shard]", "Inventory items 3_shard" },
            { "[dath]", "Inventory items_dash stone" },
            { "[torch]", "Randomizer items_Torch redux" },
            { "[triangle]", "Inventory items_money triangle" },
            { "[realmoney]", "game gui_money_icon" },
            { "[anklet]", "trinkets 1_anklet" },
            { "[perfume]", "trinkets 1_laurel" },
            { "[mufflingbell]", "trinkets 1_bell" },
            { "[rtsr]", "trinkets 1_RTSR" },
            { "[aurasgem]", "trinkets 2_aurasgem" },
            { "[invertedash]", "trinkets 1_MP Flasks" },
            { "[bonecard]", "trinkets 2_bone" },
            { "[luckycup]", "trinkets 1_heart" },
            { "[glasscannon]", "trinkets 1_glasadagger" },
            { "[daggerstrap]", "trinkets 1_dagger" },
            { "[louderecho]", "trinkets 1_ghost" },
            { "[magicecho]", "trinkets 1_bloodstain MP" },
            { "[bracer]", "trinkets 1_shield" },
            { "[tincture]", "trinkets 1_glass" },
            { "[btsr]", "trinkets 1_BTSR" },
            { "[scavengermask]", "trinkets 1_mask" },
            { "[redhex]", "Randomizer items_Red Questagon" },
            { "[greenhex]", "Randomizer items_Green Questagon" },
            { "[bluehex]", "Randomizer items_Blue Questagon" },
            { "[goldhex]", "Randomizer items_Gold Questagon" },
            { "[mrmayor]", "Randomizer items_Mr Mayor" },
            { "[secretlegend]", "Randomizer items_Secret Legend" },
            { "[sacredgeometry]", "Randomizer items_Sacred Geometry" },
            { "[vintage]", "Randomizer items_Vintage" },
            { "[justsomepals]", "Randomizer items_Just Some Pals" },
            { "[regalweasel]", "Randomizer items_Regal Weasel" },
            { "[springfalls]", "Randomizer items_Spring Falls" },
            { "[powerup]", "Randomizer items_Power Up" },
            { "[backtowork]", "Randomizer items_Back To Work" },
            { "[phonomath]", "Randomizer items_Phonomath" },
            { "[dusty]", "Randomizer items_Dusty" },
            { "[foreverfriend]", "Randomizer items_Forever Friend" },
            { "[fooltrap]", "Randomizer items_Fool Trap" },
            { "[archipelago]", "Randomizer items_Archipelago Item" },
            { "[ladder]", "Randomizer items_ladder" },
            { "[grass]", "Randomizer items_grass" },
            { "[fuse]", "Randomizer items_fuse" },
            { "[bell]", "Randomizer items_bell" },
        };

        public static Dictionary<string, string> SpriteNameToAbbreviation = new Dictionary<string, string>();

        public static Dictionary<string, string> ItemNameToAbbreviation = new Dictionary<string, string>() {
            // Consumables
            { "Firecracker", "[firecracker]" },
            { "Firecracker x2", "[firecracker]" },
            { "Firecracker x3", "[firecracker]" },
            { "Firecracker x4", "[firecracker]" },
            { "Firecracker x5", "[firecracker]" },
            { "Firecracker x6", "[firecracker]" },
            { "Fire Bomb", "[firebomb]" },
            { "Fire Bomb x2", "[firebomb]" },
            { "Fire Bomb x3", "[firebomb]" },
            { "Ice Bomb", "[icebomb]" },
            { "Ice Bomb x2", "[icebomb]" },
            { "Ice Bomb x3", "[icebomb]" },
            { "Ice Bomb x5", "[icebomb]" },
            { "Lure", "[lure]" },
            { "Lure x2", "[lure]" },
            { "Pepper", "[pepper]" },
            { "Pepper x2", "[pepper]" },
            { "Ivy", "[ivy]" },
            { "Ivy x3", "[ivy]" },
            { "Effigy", "[effigy]" },
            { "HP Berry", "[hpberry]" },
            { "HP Berry x2", "[hpberry]" },
            { "HP Berry x3", "[hpberry]" },
            { "MP Berry", "[mpberry]" },
            { "MP Berry x2", "[mpberry]" },
            { "MP Berry x3", "[mpberry]" },
            // Fairy
            { "Fairy", "[fairy]" },
            // Regular Items
            { "Stick", "[stick]" },
            { "Sword", "[realsword]" },
            { "Sword Upgrade", "[realsword]" },
            { "Magic Wand", "[wand]" },
            { "Magic Dagger", "[dagger]" },
            { "Magic Orb", "[orb]" },
            { "Hero's Laurels", "[laurels]" },
            { "Lantern", "[lantern]" },
            { "Gun", "[gun]" },
            { "Shield", "[shield]" },
            { "Dath Stone", "[dath]" },
            { "Torch", "[torch]" },
            { "Hourglass", "[hourglass]" },
            { "Old House Key", "[housekey]" },
            { "Key", "[yellowkey]" },
            { "Fortress Vault Key", "[vaultkey]" },
            { "Flask Shard", "[shard]" },
            { "Potion Flask", "[flask]" },
            { "Golden Coin", "[coin]" },
            { "Card Slot", "[square]" },
            { "Red Questagon", "[redhex]" },
            { "Green Questagon", "[greenhex]" },
            { "Blue Questagon", "[bluehex]" },
            { "Gold Questagon", "[goldhex]" },
            // Upgrades and Relics
            { "ATT Offering", "[att]" },
            { "DEF Offering", "[def]" },
            { "Potion Offering", "[potion]" },
            { "HP Offering", "[hp]" },
            { "MP Offering", "[mp]" },
            { "SP Offering", "[sp]" },
            { "Hero Relic - ATT", "[attrelic]" },
            { "Hero Relic - DEF", "[defrelic]" },
            { "Hero Relic - POTION", "[potionrelic]" },
            { "Hero Relic - HP", "[hprelic]" },
            { "Hero Relic - SP", "[sprelic]" },
            { "Hero Relic - MP", "[mprelic]" },
            // Trinket Cards
            { "Orange Peril Ring", "[rtsr]" },
            { "Tincture", "[tincture]" },
            { "Scavenger Mask", "[scavengermask]" },
            { "Cyan Peril Ring", "[btsr]" },
            { "Bracer", "[bracer]" },
            { "Dagger Strap", "[daggerstrap]" },
            { "Inverted Ash", "[invertedash]" },
            { "Lucky Cup", "[luckycup]" },
            { "Magic Echo", "[magicecho]" },
            { "Anklet", "[anklet]" },
            { "Muffling Bell", "[mufflingbell]" },
            { "Glass Cannon", "[glasscannon]" },
            { "Perfume", "[perfume]" },
            { "Louder Echo", "[louderecho]" },
            { "Aura's Gem", "[aurasgem]" },
            { "Bone Card", "[bonecard]" },
            // Golden Trophies
            { "Mr Mayor", "[mrmayor]" },
            { "Secret Legend", "[secretlegend]" },
            { "Sacred Geometry", "[sacredgeometry]" },
            { "Vintage", "[vintage]" },
            { "Just Some Pals", "[justsomepals]" },
            { "Regal Weasel", "[regalweasel]" },
            { "Spring Falls", "[springfalls]" },
            { "Power Up", "[powerup]" },
            { "Back To Work", "[backtowork]" },
            { "Phonomath", "[phonomath]" },
            { "Dusty", "[dusty]" },
            { "Forever Friend", "[foreverfriend]" },
            // Fool Trap
            { "Fool Trap", "[fooltrap]" },
            // Money
            { "Money x1", "[realmoney]" },
            { "Money x2", "[realmoney]" },
            { "Money x3", "[realmoney]" },
            { "Money x4", "[realmoney]" },
            { "Money x5", "[realmoney]" },
            { "Money x10", "[realmoney]" },
            { "Money x15", "[realmoney]" },
            { "Money x16", "[realmoney]" },
            { "Money x20", "[realmoney]" },
            { "Money x25", "[realmoney]" },
            { "Money x30", "[realmoney]" },
            { "Money x32", "[realmoney]" },
            { "Money x40", "[realmoney]" },
            { "Money x48", "[realmoney]" },
            { "Money x50", "[realmoney]" },
            { "Money x64", "[realmoney]" },
            { "Money x100", "[realmoney]" },
            { "Money x128", "[realmoney]" },
            { "Money x200", "[realmoney]" },
            { "Money x255", "[realmoney]" },
            // Pages
            { "Pages 0-1", "[book]" },
            { "Pages 2-3", "[book]" },
            { "Pages 4-5", "[book]" },
            { "Pages 6-7", "[book]" },
            { "Pages 8-9", "[book]" },
            { "Pages 10-11", "[book]" },
            { "Pages 12-13", "[book]" },
            { "Pages 14-15", "[book]" },
            { "Pages 16-17", "[book]" },
            { "Pages 18-19", "[book]" },
            { "Pages 20-21", "[book]" },
            { "Pages 22-23", "[book]" },
            { "Pages 24-25 (Prayer)", "[book]" },
            { "Pages 26-27", "[book]" },
            { "Pages 28-29", "[book]" },
            { "Pages 30-31", "[book]" },
            { "Pages 32-33", "[book]" },
            { "Pages 34-35", "[book]" },
            { "Pages 36-37", "[book]" },
            { "Pages 38-39", "[book]" },
            { "Pages 40-41", "[book]" },
            { "Pages 42-43 (Holy Cross)", "[book]" },
            { "Pages 44-45", "[book]" },
            { "Pages 46-47", "[book]" },
            { "Pages 48-49", "[book]" },
            { "Pages 50-51", "[book]" },
            { "Pages 52-53 (Icebolt)", "[book]" },
            { "Pages 54-55", "[book]" },
            // Ladders
            { "Ladders in Overworld Town", "[ladder]" },
            { "Ladders near Weathervane", "[ladder]" },
            { "Ladders near Overworld Checkpoint", "[ladder]" },
            { "Ladder to East Forest", "[ladder]" },
            { "Ladders to Lower Forest", "[ladder]" },
            { "Ladders near Patrol Cave", "[ladder]" },
            { "Ladders in Well", "[ladder]" },
            { "Ladders to West Bell", "[ladder]" },
            { "Ladder to Quarry", "[ladder]" },
            { "Ladder in Dark Tomb", "[ladder]" },
            { "Ladders near Dark Tomb", "[ladder]" },
            { "Ladder near Temple Rafters", "[ladder]" },
            { "Ladder to Swamp", "[ladder]" },
            { "Ladders in Swamp", "[ladder]" },
            { "Ladder to Ruined Atoll", "[ladder]" },
            { "Ladders in South Atoll", "[ladder]" },
            { "Ladders to Frog's Domain", "[ladder]" },
            { "Ladders in Hourglass Cave", "[ladder]" },
            { "Ladder to Beneath the Vault", "[ladder]" },
            { "Ladders in Lower Quarry", "[ladder]"},
            { "Ladders in Library", "[ladder]"},
            // Fuses added here in FuseRandomizer.CreateFuseItems()
            // Bells
            { "East Bell", "[bell]" },
            { "West Bell", "[bell]" },
            // Grass
            { "Grass", "[grass]"},
            // Non-Tunic Item
            { "Archipelago Item", "[archipelago]"}
        };

        public static void SetupCustomGlyphSprites() {
            int index = 110;
            List<string> cartouches = Parser.cartouche.ToList();
            if (!cartouches.Contains("[torch]")) {
                Parser.cartouche = cartouches.ToArray();
                List<Sprite> sprites = SpriteBuilder.spriteResources.ToList();
                foreach (string Icon in CustomSpriteIcons.Keys) {
                    sprites.Add(ModelSwaps.FindSprite(CustomSpriteIcons[Icon]));
                    SpriteNameToAbbreviation.Add(CustomSpriteIcons[Icon], Icon);
                    CustomSpriteIndices.Add(Icon, index++);
                }
                sprites.Add(Inventory.GetItemByName("Dath Stone").icon);
                cartouches.AddRange(CustomSpriteIndices.Keys);
                cartouches.Add("[filler]");
                SpriteBuilder.spriteResources = sprites.ToArray();
            }
        }

        public static bool Parser_findSymbol_PrefixPatch(ref string s, ref int __result) {
            if (CustomSpriteIndices.ContainsKey(s)) {
                __result = CustomSpriteIndices[s];
                return false;
            }
            return true;
        }

        public static void SpriteBuilder_rebuild_PostfixPatch(SpriteBuilder __instance) {
            foreach (SpriteRenderer renderer in __instance.gameObject.transform.GetComponentsInChildren<SpriteRenderer>(true)) {
                if (renderer.sprite != null && CustomSpriteIcons.Values.ToList().Contains(renderer.sprite.name)) {
                    renderer.material = ModelSwaps.FindMaterial("UI Add");
                    if (renderer.sprite.name.Contains("trinkets ")) {
                        renderer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                        renderer.material = ModelSwaps.FindMaterial("UI-trinket");
                        GameObject backing = new GameObject("backing");
                        backing.AddComponent<SpriteRenderer>().sprite = ModelSwaps.FindSprite("trinkets 2_backing");
                        backing.transform.parent = renderer.transform.parent;
                        backing.transform.localPosition = renderer.transform.localPosition;
                        backing.transform.localScale = renderer.transform.localScale * 0.9f;
                        backing.layer = renderer.gameObject.layer;
                    } else if (renderer.sprite.name == "game gui_money_icon") {
                        renderer.transform.localScale *= 1.25f;
                    } else if (renderer.sprite.name == "Randomizer items_Fool Trap") {
                        renderer.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
                        renderer.transform.localScale *= 0.75f;
                    }

                    renderer.transform.localScale *= 0.85f;
                }
            }
        }

        public static string GetSwordIconName(int level) {
            switch(level) {
                case 1:
                    return "[stick]";
                case 2:
                    return "[realsword]";
                case 3:
                    return "[librariansword]";
                case 4:
                case 5:
                    return "[heirsword]";
                default:
                    return "[realsword]";
            }
        }
    }
}
