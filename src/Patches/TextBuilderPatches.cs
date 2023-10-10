using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TunicRandomizer {
    public class TextBuilderPatches {

        public static Dictionary<string, int> CustomSpriteIndices = new Dictionary<string, int>() {
            { "[stick]", 110 },
            { "[realsword]", 111 },
            { "[wand]", 112 },
            { "[dagger]", 113 },
            { "[orb]", 114 },
            { "[shield]", 115 },
            { "[gun]", 116 },
            { "[hourglass]", 117 },
            { "[lantern]", 118 },
            { "[laurels]", 119 },
            { "[coin]", 120 },
            { "[trinket]", 121 },
            { "[square]", 122 },
            { "[fairy]", 123 },
            { "[mayor]", 124 },
            { "[book]", 125 },
            { "[atk]", 126 },
            { "[def]", 127 },
            { "[potion]", 128 },
            { "[hp]", 129 },
            { "[sp]", 130 },
            { "[mp]", 131 },
            { "[yellowkey]", 132 },
            { "[housekey]", 133 },
            { "[vault]", 134 },
            { "[firecracker]", 135 },
            { "[firebomb]", 136 },
            { "[icebomb]", 137 },
            { "[hpberry]", 138 },
            { "[mpberry]", 139 },
            { "[pepper]", 140 },
            { "[ivy]", 141 },
            { "[lure]", 142 },
            { "[effigy]", 143 },
            { "[flask]", 144 },
            { "[shard]", 145 },
            { "[dath]", 146 },
            { "[torch]", 147 },
            { "[triangle]", 148 },
        };

        public static Dictionary<string, string> CustomSpriteIcons = new Dictionary<string, string>() {
            { "[stick]", "Inventory items_stick" },
            { "[realsword]", "Inventory items_sword" },
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
            { "[atk]", "Inventory items_offering_tooth" },
            { "[def]", "Inventory items_offering_effigy" },
            { "[potion]", "Inventory items_offering_ash" },
            { "[hp]", "Inventory items_offering_flower" },
            { "[sp]", "Inventory items_offering_feather" },
            { "[mp]", "Inventory items_offering_orb" },
            { "[doorkey]", "Inventory items_key" },
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
            { "[torch]", "Inventory items_torch" },
            { "[triangle]", "Inventory items_money triangle" },
        };

        public static void SetupCustomGlyphSprites() {
            List<string> cartouches = Parser.cartouche.ToList();
            if (!cartouches.Contains("[torch]")) {
                cartouches.AddRange(CustomSpriteIndices.Keys);
                cartouches.Add("[filler]");
                Parser.cartouche = cartouches.ToArray();
                List<Sprite> sprites = SpriteBuilder.spriteResources.ToList();
                foreach (string Icon in CustomSpriteIcons.Values) {
                    sprites.Add(ModelSwaps.FindSprite(Icon));
                }
                sprites.Add(Inventory.GetItemByName("Homeward Bone Statue").icon);
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
                    renderer.material = ModelSwaps.FindMaterial("Default UI Material");
                }
            }
        }
    }
}
