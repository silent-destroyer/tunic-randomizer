using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static TunicRandomizer.GhostHints;

namespace TunicRandomizer {
    public class Locations {
        
        public static Dictionary<string, string> LocationIdToDescription = new Dictionary<string, string>();
        public static Dictionary<string, string> LocationDescriptionToId = new Dictionary<string, string>();

        public static Dictionary<string, Check> VanillaLocations = new Dictionary<string, Check>() { };
        public static Dictionary<string, Check> RandomizedLocations = new Dictionary<string, Check> { };
        public static Dictionary<string, bool> CheckedLocations = new Dictionary<string, bool>();

        public static Dictionary<string, List<ArchipelagoHint>> MajorItemLocations = new Dictionary<string, List<ArchipelagoHint>>();

        public static List<string> AllScenes = new List<string>();

        public static string LocationNamesJson = "{\"5 [Overworld Redux]\":\"Overworld - [East] Between Ladders Near Ruined Passage\",\"55 [Archipelagos Redux]\":\"West Garden - [North] Obscured Beneath Hero's Memorial\",\"217 [Archipelagos Redux]\":\"West Garden - [North] Behind Holy Cross Door\",\"west_garden [Archipelagos Redux]\":\"West Garden - [North] Page Pickup\",\"256 [Archipelagos Redux]\":\"West Garden - [North] Across From Page Pickup\",\"280 [Archipelagos Redux]\":\"West Garden - [West] In Flooded Walkway\",\"283 [Archipelagos Redux]\":\"West Garden - [West] Past Flooded Walkway\",\"57 [Archipelagos Redux]\":\"West Garden - [West Highlands] Upper Left Walkway\",\"253 [Archipelagos Redux]\":\"West Garden - [Central Lowlands] Passage Beneath Bridge\",\"56 [Archipelagos Redux]\":\"West Garden - [Central Lowlands] Chest Near Shortcut Bridge\",\"Archipelagos Redux-(-396.3, 1.4, 42.3) [Archipelagos Redux]\":\"West Garden - [West Lowlands] Tree Holy Cross Chest\",\"58 [Archipelagos Redux]\":\"West Garden - [Central Lowlands] Chest Beneath Save Point\",\"206 [Archipelagos Redux]\":\"West Garden - [Central Lowlands] Chest Beneath Faeries\",\"94 [Archipelagos Redux]\":\"West Garden - [South Highlands] Secret Chest Beneath Fuse\",\"111 [Archipelagos Redux]\":\"West Garden - [Southeast Lowlands] Outside Cave\",\"archipelagos_night [Archipelagos Redux]\":\"West Garden - [East Lowlands] Page Behind Ice Dagger House\",\"257 [Archipelagos Redux]\":\"West Garden - [Central Lowlands] Below Left Walkway\",\"59 [Archipelagos Redux]\":\"West Garden - [Central Highlands] Behind Guard Captain\",\"Archipelagos Redux-(-236.0, 8.0, 86.3) [Archipelagos Redux]\":\"West Garden - [Central Highlands] Holy Cross (Blue Lines)\",\"223 [Archipelagos Redux]\":\"West Garden - [Central Highlands] Top of Ladder Before Boss\",\"93 [Archipelagos Redux]\":\"West Garden - [Central Highlands] After Garden Knight\",\"Stundagger [archipelagos_house]\":\"West Garden House - [Southeast Lowlands] Ice Dagger Pickup\",\"72 [Atoll Redux]\":\"Ruined Atoll - [North] From Lower Overworld Entrance\",\"67 [Atoll Redux]\":\"Ruined Atoll - [South] Upper Floor On Bricks\",\"218 [Atoll Redux]\":\"Ruined Atoll - [South] Upper Floor On Power Line\",\"219 [Atoll Redux]\":\"Ruined Atoll - [South] Chest Near Big Crabs\",\"76 [Atoll Redux]\":\"Ruined Atoll - [Southeast] Chest Near Fuse\",\"66 [Atoll Redux]\":\"Ruined Atoll - [North] Obscured Beneath Bridge\",\"220 [Atoll Redux]\":\"Ruined Atoll - [North] Guarded By Bird\",\"287 [Atoll Redux]\":\"Ruined Atoll - [Northwest] Bombable Wall\",\"69 [Atoll Redux]\":\"Ruined Atoll - [Northwest] Behind Envoy\",\"1010 [Atoll Redux]\":\"Ruined Atoll - [West] Near Kevin Block\",\"70 [Atoll Redux]\":\"Ruined Atoll - [Southwest] Obscured Behind Fuse\",\"68 [Atoll Redux]\":\"Ruined Atoll - [South] Near Birds\",\"Key [Atoll Redux]\":\"Ruined Atoll - [Northeast] Key Pickup\",\"73 [Atoll Redux]\":\"Ruined Atoll - [East] Locked Room Lower Chest\",\"71 [Atoll Redux]\":\"Ruined Atoll - [East] Locked Room Upper Chest\",\"221 [Atoll Redux]\":\"Ruined Atoll - [Northeast] Chest Beneath Brick Walkway\",\"75 [Atoll Redux]\":\"Ruined Atoll - [Northeast] Chest On Brick Walkway\",\"999 [Cathedral Arena]\":\"Cathedral Gauntlet - Gauntlet Reward\",\"1002 [Cathedral Redux]\":\"Cathedral - Secret Legend Trophy Chest\",\"240 [Cathedral Redux]\":\"Cathedral - [1F] Library\",\"244 [Cathedral Redux]\":\"Cathedral - [1F] Library Secret\",\"236 [Cathedral Redux]\":\"Cathedral - [1F] Guarded By Lasers\",\"237 [Cathedral Redux]\":\"Cathedral - [1F] Near Spikes\",\"243 [Cathedral Redux]\":\"Cathedral - [2F] Bird Room Secret\",\"238 [Cathedral Redux]\":\"Cathedral - [2F] Bird Room\",\"239 [Cathedral Redux]\":\"Cathedral - [2F] Entryway Upper Walkway\",\"241 [Cathedral Redux]\":\"Cathedral - [2F] Library\",\"242 [Cathedral Redux]\":\"Cathedral - [2F] Guarded By Lasers\",\"60 [Changing Room]\":\"Changing Room - Normal Chest\",\"52 [Crypt Redux]\":\"Dark Tomb - Skulls Chest\",\"213 [Crypt Redux]\":\"Dark Tomb - Spike Maze Upper Walkway\",\"53 [Crypt Redux]\":\"Dark Tomb - Spike Maze Near Stairs\",\"210 [Crypt Redux]\":\"Dark Tomb - Spike Maze Near Exit\",\"54 [Crypt Redux]\":\"Dark Tomb - 1st Laser Room Obscured\",\"212 [Crypt Redux]\":\"Dark Tomb - 1st Laser Room\",\"211 [Crypt Redux]\":\"Dark Tomb - 2nd Laser Room\",\"CubeRoom-(321.1, 3.0, 217.0) [CubeRoom]\":\"Cube Cave - Holy Cross Chest\",\"1011 [Dusty]\":\"Fortress Leaf Piles - Secret Chest\",\"286 [East Forest Redux]\":\"East Forest - Bombable Wall\",\"25 [East Forest Redux]\":\"East Forest - Near Telescope\",\"24 [East Forest Redux]\":\"East Forest - Near Save Point\",\"forest [East Forest Redux]\":\"East Forest - Page On Teleporter\",\"23 [East Forest Redux]\":\"East Forest - From Guardhouse 1 Chest\",\"East Forest Redux-(104.0, 16.0, 61.0) [East Forest Redux]\":\"East Forest - Dancing Fox Spirit Holy Cross\",\"26 [East Forest Redux]\":\"East Forest - Spider Chest\",\"248 [East Forest Redux]\":\"East Forest - Beneath Spider Chest\",\"East Forest Redux-(164.0, -25.0, -56.0) [East Forest Redux]\":\"East Forest - Golden Obelisk Holy Cross\",\"284 [East Forest Redux]\":\"East Forest - Lower Grapple Chest\",\"281 [East Forest Redux]\":\"East Forest - Lower Dash Chest\",\"1006 [East Forest Redux]\":\"East Forest - Ice Rod Grapple Chest\",\"21 [East Forest Redux]\":\"East Forest - Above Save Point\",\"22 [East Forest Redux]\":\"East Forest - Above Save Point Obscured\",\"29 [East Forest Redux Interior]\":\"Guardhouse 2 - Upper Floor\",\"30 [East Forest Redux Interior]\":\"Guardhouse 2 - Bottom Floor Secret\",\"27 [East Forest Redux Laddercave]\":\"Guardhouse 1 - Upper Floor Obscured\",\"28 [East Forest Redux Laddercave]\":\"Guardhouse 1 - Upper Floor\",\"270 [EastFiligreeCache]\":\"Southeast Cross Door - Chest 3\",\"271 [EastFiligreeCache]\":\"Southeast Cross Door - Chest 2\",\"272 [EastFiligreeCache]\":\"Southeast Cross Door - Chest 1\",\"forest shortcut [Forest Belltower]\":\"Forest Belltower - Page Pickup\",\"205 [Forest Belltower]\":\"Forest Belltower - Obscured Beneath Bell Bottom Floor\",\"20 [Forest Belltower]\":\"Forest Belltower - After Guard Captain\",\"204 [Forest Belltower]\":\"Forest Belltower - Obscured Near Bell Top Floor\",\"19 [Forest Belltower]\":\"Forest Belltower - Near Save Point\",\"Vault Key (Red) [Fortress Arena]\":\"Fortress Arena - Siege Engine/Vault Key Pickup\",\"Hexagon Red [Fortress Arena]\":\"Fortress Arena - Hexagon Red\",\"63 [Fortress Basement]\":\"Beneath the Fortress - Obscured Behind Waterfall\",\"61 [Fortress Basement]\":\"Beneath the Fortress - Bridge\",\"62 [Fortress Basement]\":\"Beneath the Fortress - Cell Chest 1\",\"65 [Fortress Basement]\":\"Beneath the Fortress - Cell Chest 2\",\"64 [Fortress Basement]\":\"Beneath the Fortress - Back Room Chest\",\"86 [Fortress Courtyard]\":\"Fortress Courtyard - From East Belltower\",\"96 [Fortress Courtyard]\":\"Fortress Courtyard - Below Walkway\",\"88 [Fortress Courtyard]\":\"Fortress Courtyard - Near Fuse\",\"87 [Fortress Courtyard]\":\"Fortress Courtyard - Chest Near Cave\",\"spidercave [Fortress Courtyard]\":\"Fortress Courtyard - Page Near Cave\",\"112 [Fortress East]\":\"Fortress East Shortcut - Chest Near Slimes\",\"fortress [Fortress Main]\":\"Eastern Vault Fortress - [West Wing] Page Pickup\",\"83 [Fortress Main]\":\"Eastern Vault Fortress - [West Wing] Dark Room Chest 1\",\"84 [Fortress Main]\":\"Eastern Vault Fortress - [West Wing] Dark Room Chest 2\",\"Fortress Main-(-75.0, -1.0, 17.0) [Fortress Main]\":\"Eastern Vault Fortress - [West Wing] Candles Holy Cross\",\"85 [Fortress Main]\":\"Eastern Vault Fortress - [East Wing] Bombable Wall\",\"113 [Fortress Reliquary]\":\"Fortress Grave Path - Upper Walkway\",\"114 [Fortress Reliquary]\":\"Fortress Grave Path - Chest Right of Grave\",\"115 [Fortress Reliquary]\":\"Fortress Grave Path - Obscured Chest Left of Grave\",\"Wand [frog cave main]\":\"Frog's Domain - Magic Orb Pickup\",\"77 [frog cave main]\":\"Frog's Domain - Above Vault\",\"82 [frog cave main]\":\"Frog's Domain - Side Room Grapple Secret\",\"81 [frog cave main]\":\"Frog's Domain - Side Room Chest\",\"80 [frog cave main]\":\"Frog's Domain - Side Room Secret Passage\",\"78 [frog cave main]\":\"Frog's Domain - Main Room Top Floor\",\"279 [frog cave main]\":\"Frog's Domain - Grapple Above Hot Tub\",\"79 [frog cave main]\":\"Frog's Domain - Main Room Bottom Floor\",\"222 [frog cave main]\":\"Frog's Domain - Near Vault\",\"259 [frog cave main]\":\"Frog's Domain - Slorm Room\",\"276 [frog cave main]\":\"Frog's Domain - Escape Chest\",\"Lantern [Furnace]\":\"West Furnace - Lantern Pickup\",\"92 [Furnace]\":\"West Furnace - Chest\",\"Hexagon Green [Library Arena]\":\"Librarian - Hexagon Green\",\"Library Hall-(133.3, 10.0, -43.2) [Library Hall]\":\"Library Hall - Holy Cross Chest\",\"library_2 [Library Lab]\":\"Library Lab - Page 1\",\"library_3 [Library Lab]\":\"Library Lab - Page 2\",\"library_1 [Library Lab]\":\"Library Lab - Page 3\",\"226 [Library Lab]\":\"Library Lab - Chest By Shrine 1\",\"225 [Library Lab]\":\"Library Lab - Chest By Shrine 2\",\"227 [Library Lab]\":\"Library Lab - Chest By Shrine 3\",\"228 [Library Lab]\":\"Library Lab - Behind Chalkboard by Fuse\",\"216 [Maze Room]\":\"Maze Cave - Maze Room Chest\",\"Maze Room-(1.0, 0.0, -1.0) [Maze Room]\":\"Maze Cave - Maze Room Holy Cross\",\"200 [Monastery]\":\"Monastery - Monastery Chest\",\"mountain [Mountain]\":\"Lower Mountain - Page Before Door\",\"final [Mountaintop]\":\"Top of the Mountain - Page At The Peak\",\"Overworld Cave-(-90.4, 515.0, -738.9) [Overworld Cave]\":\"Caustic Light Cave - Holy Cross Chest\",\"89 [Overworld Interiors]\":\"Old House - Normal Chest\",\"Overworld Interiors-(-28.0, 27.0, -50.5) [Overworld Interiors]\":\"Old House - Holy Cross Chest\",\"Shield [Overworld Interiors]\":\"Old House - Shield Pickup\",\"under_overworld [Overworld Interiors]\":\"Old House - Holy Cross Door Page\",\"1013 [Overworld Redux]\":\"Overworld - [South] Starting Platform Holy Cross\",\"8 [Overworld Redux]\":\"Overworld - [Central] Chest Across From Well\",\"11 [Overworld Redux]\":\"Overworld - [West] Obscured Near Well\",\"1 [Overworld Redux]\":\"Overworld - [West] Obscured Behind Windmill\",\"1003 [Overworld Redux]\":\"Overworld - [West] Windmill Holy Cross\",\"Key [Overworld Redux]\":\"Overworld - [West] Key Pickup\",\"12 [Overworld Redux]\":\"Overworld - [Central] Bombable Wall\",\"90 [Overworld Redux]\":\"Overworld - [East] Chest In Trees\",\"255 [Overworld Redux]\":\"Overworld - [Southeast] Chest Near Swamp\",\"15 [Overworld Redux]\":\"Overworld - [East] Chest Near Pots\",\"Overworld Redux-(90.4, 36.0, -122.1) [Overworld Redux]\":\"Overworld - [East] Weathervane Holy Cross\",\"overworld post-forest [Overworld Redux]\":\"Overworld - [East] Page Near Secret Shop\",\"Overworld Redux-(64.5, 44.0, -40.0) [Overworld Redux]\":\"Overworld - [Northeast] Flowers Holy Cross\",\"6 [Overworld Redux]\":\"Overworld - [Northeast] Chest Above Patrol Cave\",\"Techbow [Overworld Redux]\":\"Overworld - [Northwest] Fire Wand Pickup\",\"stonehenge_reward [Overworld Redux]\":\"Overworld - [Northwest] Golden Obelisk Page\",\"16 [Overworld Redux]\":\"Overworld - [Northwest] Chest Near Golden Obelisk\",\"9 [Overworld Redux]\":\"Overworld - [Northwest] Chest Near Quarry Gate\",\"13 [Overworld Redux]\":\"Overworld - [Northwest] Chest Near Turret\",\"207 [Overworld Redux]\":\"Overworld - [Northwest] Shadowy Corner Chest\",\"1008 [Overworld Redux]\":\"Overworld - [West] Windchimes Holy Cross\",\"town_upper [Overworld Redux]\":\"Overworld - [West] Page On Teleporter\",\"Overworld Redux-(-132.0, 28.0, -55.5) [Overworld Redux]\":\"Overworld - [West] Moss Wall Holy Cross\",\"91 [Overworld Redux]\":\"Overworld - [West] Chest Behind Moss Wall\",\"overworld_dash [Overworld Redux]\":\"Overworld - [Southwest] Fountain Page\",\"Overworld Redux-(-83.0, 20.0, -117.5) [Overworld Redux]\":\"Overworld - [Southwest] Fountain Holy Cross\",\"2 [Overworld Redux]\":\"Overworld - [Southwest] Chest Guarded By Turret\",\"209 [Overworld Redux]\":\"Overworld - [Southwest] Grapple Chest Over Walkway\",\"Key (House) [Overworld Redux]\":\"Overworld - [Southwest] Key Pickup\",\"17 [Overworld Redux]\":\"Overworld - [Southwest] South Chest Near Guard\",\"208 [Overworld Redux]\":\"Overworld - [Southwest] Obscured In Tunnel To Beach\",\"273 [Overworld Redux]\":\"Overworld - [Southwest] Beach Chest Near Flowers\",\"Overworld Redux-(-52.0, 2.0, -174.8) [Overworld Redux]\":\"Overworld - [Southwest] Flowers Holy Cross\",\"1004 [Overworld Redux]\":\"Overworld - [Southwest] Haiku Holy Cross\",\"7 [Overworld Redux]\":\"Overworld - [Southwest] Beach Chest Beneath Guard\",\"4 [Overworld Redux]\":\"Overworld - [Southwest] Tunnel Guarded By Turret\",\"18 [Overworld Redux]\":\"Overworld - [Southwest] West Beach Guarded By Turret\",\"267 [Overworld Redux]\":\"Overworld - [Southwest] West Beach Guarded By Turret 2\",\"beach [Overworld Redux]\":\"Overworld - [South] Beach Page\",\"285 [Overworld Redux]\":\"Overworld - [Southwest] Bombable Wall Near Fountain\",\"10 [Overworld Redux]\":\"Overworld - [South] Beach Chest\",\"cathedral [Overworld Redux]\":\"Overworld - [Southeast] Page on Pillar by Swamp\",\"tablet [Overworld Redux]\":\"Overworld - [Northwest] Page on Pillar by Dark Tomb\",\"town_well [Overworld Redux]\":\"Overworld - [Northwest] Page By Well\",\"245 [Overworld Redux]\":\"Overworld - [Northwest] Chest Beneath Quarry Gate\",\"14 [Overworld Redux]\":\"Overworld - [West] Near West Garden Entrance\",\"258 [Overworld Redux]\":\"Overworld - [Southwest] From West Garden\",\"3 [Overworld Redux]\":\"Overworld - [West] Chest After Bell\",\"266 [Overworld Redux]\":\"Overworld - [East] Grapple Chest\",\"214 [PatrolCave]\":\"Patrol Cave - Normal Chest\",\"PatrolCave-(74.0, 46.0, 24.0) [PatrolCave]\":\"Patrol Cave - Holy Cross Chest\",\"Quarry Redux-(0.7, 68.0, 84.7) [Quarry Redux]\":\"Quarry - [Back Entrance] Bushes Holy Cross\",\"126 [Quarry Redux]\":\"Quarry - [East] Obscured Near Telescope\",\"133 [Quarry Redux]\":\"Quarry - [Central] Obscured Below Entry Walkway\",\"116 [Quarry Redux]\":\"Quarry - [Back Entrance] Chest\",\"128 [Quarry Redux]\":\"Quarry - [Back Entrance] Obscured Behind Wall\",\"288 [Quarry Redux]\":\"Quarry - [West] Upper Area Bombable Wall\",\"127 [Quarry Redux]\":\"Quarry - [West] Upper Area Near Waterfall\",\"120 [Quarry Redux]\":\"Quarry - [West] Near Shooting Range\",\"265 [Quarry Redux]\":\"Quarry - [West] Shooting Range Secret Path\",\"121 [Quarry Redux]\":\"Quarry - [West] Below Shooting Range\",\"130 [Quarry Redux]\":\"Quarry - [West] Lower Area Below Bridge\",\"131 [Quarry Redux]\":\"Quarry - [West] Lower Area Isolated Chest\",\"262 [Quarry Redux]\":\"Quarry - [West] Lower Area After Bridge\",\"122 [Quarry Redux]\":\"Quarry - [Lowlands] Below Broken Ladder\",\"129 [Quarry Redux]\":\"Quarry - [Lowlands] Upper Walkway\",\"132 [Quarry Redux]\":\"Quarry - [Lowlands] Near Elevator\",\"123 [Quarry Redux]\":\"Quarry - [Central] Below Entry Walkway\",\"117 [Quarry Redux]\":\"Quarry - [Central] Near Shortcut Ladder\",\"224 [Quarry Redux]\":\"Quarry - [East] Near Bridge\",\"289 [Quarry Redux]\":\"Quarry - [East] Bombable Wall\",\"118 [Quarry Redux]\":\"Quarry - [East] Near Telescope\",\"268 [Quarry Redux]\":\"Quarry - [Central] Obscured Behind Staircase\",\"125 [Quarry Redux]\":\"Quarry - [East] Obscured Beneath Scaffolding\",\"124 [Quarry Redux]\":\"Quarry - [East] Obscured Near Winding Staircase\",\"119 [Quarry Redux]\":\"Quarry - [East] Upper Floor\",\"250 [Quarry Redux]\":\"Quarry - [Central] Above Ladder\",\"282 [Quarry Redux]\":\"Quarry - [Central] Above Ladder Dash Chest\",\"134 [Quarry Redux]\":\"Quarry - [Central] Top Floor Overhang\",\"Relic PIckup (6) Sword) [RelicVoid]\":\"Hero's Grave - Tooth Relic\",\"Relic PIckup (5) (MP) [RelicVoid]\":\"Hero's Grave - Mushroom Relic\",\"Relic PIckup (4) (water) [RelicVoid]\":\"Hero's Grave - Ash Relic\",\"Relic PIckup (3) (HP) [RelicVoid]\":\"Hero's Grave - Flowers Relic\",\"Relic PIckup (2) (Crown) [RelicVoid]\":\"Hero's Grave - Effigy Relic\",\"Relic PIckup (1) (SP) [RelicVoid]\":\"Hero's Grave - Feathers Relic\",\"35 [Ruined Shop]\":\"Ruined Shop - Chest 1\",\"36 [Ruined Shop]\":\"Ruined Shop - Chest 2\",\"37 [Ruined Shop]\":\"Ruined Shop - Chest 3\",\"first [Ruins Passage]\":\"Ruined Passage - Page Pickup\",\"1001 [Ruins Passage]\":\"Ruined Passage - Holy Cross Chest\",\"40 [Sewer]\":\"Beneath the Well - [Entryway] Chest\",\"43 [Sewer]\":\"Beneath the Well - [Entryway] Obscured Behind Waterfall\",\"sewer [Sewer]\":\"Beneath the Well - [Second Room] Page\",\"49 [Sewer]\":\"Beneath the Well - [Second Room] Obscured Behind Waterfall\",\"47 [Sewer]\":\"Beneath the Well - [Back Corridor] Right Secret\",\"48 [Sewer]\":\"Beneath the Well - [Back Corridor] Left Secret\",\"41 [Sewer]\":\"Beneath the Well - [Third Room] Beneath Platform Chest\",\"42 [Sewer]\":\"Beneath the Well - [Third Room] Tentacle Chest\",\"46 [Sewer]\":\"Beneath the Well - [Second Room] Underwater Chest\",\"50 [Sewer]\":\"Beneath the Well - [Side Room] Chest By Pots\",\"44 [Sewer]\":\"Beneath the Well - [Save Room] Upper Floor Chest 1\",\"45 [Sewer]\":\"Beneath the Well - [Save Room] Upper Floor Chest 2\",\"51 [Sewer]\":\"Beneath the Well - [Side Room] Chest By Phrends\",\"264 [Sewer]\":\"Beneath the Well - [Powered Secret Room] Chest\",\"below_crypt [Sewer_Boss]\":\"Dark Tomb Checkpoint - [Passage To Dark Tomb] Page Pickup\",\"Potion (First) [Shop]\":\"Shop - Potion 1\",\"Potion (West Garden) [Shop]\":\"Shop - Potion 2\",\"Trinket Coin 1 (day) [Shop]\":\"Shop - Coin 1\",\"Trinket Coin 2 (night) [Shop]\":\"Shop - Coin 2\",\"skullcave [ShopSpecial]\":\"Special Shop - Secret Page Pickup\",\"105 [Swamp Redux 2]\":\"Swamp - [Entrance] Above Entryway\",\"235 [Swamp Redux 2]\":\"Swamp - [Entrance] Obscured Inside Watchtower\",\"254 [Swamp Redux 2]\":\"Swamp - [Entrance] North Small Island\",\"246 [Swamp Redux 2]\":\"Swamp - [Entrance] South Near Fence\",\"249 [Swamp Redux 2]\":\"Swamp - [South Graveyard] Chest Near Graves\",\"247 [Swamp Redux 2]\":\"Swamp - [South Graveyard] Guarded By Big Skeleton\",\"108 [Swamp Redux 2]\":\"Swamp - [South Graveyard] Guarded By Tentacles\",\"104 [Swamp Redux 2]\":\"Swamp - [South Graveyard] Obscured Beneath Telescope\",\"98 [Swamp Redux 2]\":\"Swamp - [South Graveyard] Above Big Skeleton\",\"107 [Swamp Redux 2]\":\"Swamp - [South Graveyard] Upper Walkway On Pedestal\",\"97 [Swamp Redux 2]\":\"Swamp - [South Graveyard] Upper Walkway Dash Chest\",\"1005 [Swamp Redux 2]\":\"Swamp - [South Graveyard] 4 Orange Skulls\",\"103 [Swamp Redux 2]\":\"Swamp - [South Graveyard] Obscured Behind Ridge\",\"278 [Swamp Redux 2]\":\"Swamp - [Central] Obscured Behind Northern Mountain\",\"99 [Swamp Redux 2]\":\"Swamp - [Central] Beneath Memorial\",\"101 [Swamp Redux 2]\":\"Swamp - [Central] Near Ramps Up\",\"106 [Swamp Redux 2]\":\"Swamp - [Central] South Secret Passage\",\"109 [Swamp Redux 2]\":\"Swamp - [Upper Graveyard] Near Telescope\",\"100 [Swamp Redux 2]\":\"Swamp - [Upper Graveyard] Obscured Behind Hill\",\"102 [Swamp Redux 2]\":\"Swamp - [Upper Graveyard] Near Shield Fleemers\",\"277 [Swamp Redux 2]\":\"Swamp - [Outside Cathedral] Obscured Behind Memorial\",\"110 [Swamp Redux 2]\":\"Swamp - [Outside Cathedral] Near Moonlight Bridge Door\",\"32 [Sword Access]\":\"Forest Grave Path - Obscured Chest\",\"31 [Sword Access]\":\"Forest Grave Path - Above Gate\",\"Sword [Sword Access]\":\"Forest Grave Path - Sword Pickup\",\"1009 [Sword Access]\":\"Forest Grave Path - Holy Cross Code by Grave\",\"33 [Sword Access]\":\"Forest Grave Path - Upper Walkway\",\"19 [Sword Cave]\":\"Stick House - Stick Chest\",\"Temple-(14.0, 0.1, 42.4) [Temple]\":\"Sealed Temple - Holy Cross Chest\",\"temple [Temple]\":\"Sealed Temple - Page Pickup\",\"95 [Town Basement]\":\"Hourglass Cave - Hourglass Chest\",\"Town Basement-(-202.0, 28.0, 150.0) [Town Basement]\":\"Hourglass Cave - Holy Cross Chest\",\"town_filigree [Town_FiligreeRoom]\":\"Fountain Cross Door - Page Pickup\",\"FT_Island [Transit]\":\"Far Shore - Page Pickup\",\"1012 [Transit]\":\"Far Shore - Secret Chest\",\"Well Reward (3 Coins) [Trinket Well]\":\"Coins in the Well - 3 Coins\",\"Well Reward (6 Coins) [Trinket Well]\":\"Coins in the Well - 6 Coins\",\"Well Reward (10 Coins) [Trinket Well]\":\"Coins in the Well - 10 Coins\",\"Well Reward (15 Coins) [Trinket Well]\":\"Coins in the Well - 15 Coins\",\"Waterfall-(-47.0, 45.0, 10.0) [Waterfall]\":\"Secret Gathering Place - Holy Cross Chest\",\"waterfall [Waterfall]\":\"Secret Gathering Place - 10 Fairy Reward\",\"1007 [Waterfall]\":\"Secret Gathering Place - 20 Fairy Reward\",\"274 [ziggurat2020_1]\":\"Rooted Ziggurat Upper - Near Bridge Switch\",\"275 [ziggurat2020_1]\":\"Rooted Ziggurat Upper - Beneath Bridge To Administrator\",\"229 [ziggurat2020_2]\":\"Rooted Ziggurat Tower - Inside Tower\",\"230 [ziggurat2020_3]\":\"Rooted Ziggurat Lower - Near Corpses\",\"231 [ziggurat2020_3]\":\"Rooted Ziggurat Lower - Spider Ambush\",\"234 [ziggurat2020_3]\":\"Rooted Ziggurat Lower - Guarded By Double Turrets\",\"261 [ziggurat2020_3]\":\"Rooted Ziggurat Lower - Guarded By Double Turrets 2\",\"260 [ziggurat2020_3]\":\"Rooted Ziggurat Lower - After 2nd Double Turret Chest\",\"232 [ziggurat2020_3]\":\"Rooted Ziggurat Lower - Left Of Checkpoint Before Fuse\",\"233 [ziggurat2020_3]\":\"Rooted Ziggurat Lower - After Guarded Fuse\",\"Hexagon Blue [ziggurat2020_3]\":\"Rooted Ziggurat Lower - Hexagon Blue\"}";

        public static void CreateLocationLookups() {
            foreach (Check info in JsonConvert.DeserializeObject<List<Check>>(ItemListJson.ItemList)) {
                string locationId = info.CheckId;
                VanillaLocations.Add(locationId, info);
            }
            LocationIdToDescription = JsonConvert.DeserializeObject<Dictionary<string, string>>(LocationNamesJson);
            foreach (string Key in LocationIdToDescription.Keys) {
                LocationDescriptionToId.Add(LocationIdToDescription[Key], Key);
            }
        }

        public static void PopulateMajorItemLocations(Dictionary<string, Object> SlotData) {
            MajorItemLocations.Clear();

            foreach (string Item in ItemLookup.MajorItems) {
                if(!MajorItemLocations.ContainsKey(Item)) {
                    MajorItemLocations.Add(Item, new List<ArchipelagoHint>());
                }
                if(SlotData.ContainsKey(Item)) {
                    JArray jarray = JArray.Parse(SlotData[Item].ToString());
                    int i = 0;
                    ArchipelagoHint hint = new ArchipelagoHint();
                    hint.Item = Item;
                    foreach(JValue v in jarray) {
                        if (i % 2 == 0) { 
                            hint.Location = (string)v.Value;
                        }
                        else if (i % 2 != 0) { 
                            hint.Player = (long)v.Value;
                            MajorItemLocations[Item].Add(hint);
                        }
                        i++;
                    }
                }
            }
        }

        public static Dictionary<string, string> SimplifiedSceneNames = new Dictionary<string, string>() {
            {"Overworld Redux", "Overworld"},
            {"Furnace", "West Furnace"},
            {"CubeRoom", "Cube Cave"},
            {"Sword Cave", "Stick House"},
            {"Windmill", "Windmill"},
            {"EastFiligreeCache", "Southeast Cross Room"},
            {"Overworld Cave", "Caustic Light Cave"},
            {"Ruins Passage", "Ruined Passage"},
            {"PatrolCave", "Patrol Cave"},
            {"Waterfall", "Secret Gathering Place"},
            {"Town_FiligreeRoom", "Fountain Cross Room"},
            {"Town Basement", "Hourglass Cave"},
            {"Maze Room", "Maze Cave"},
            {"Ruined Shop", "Ruined Shop"},
            {"Changing Room", "Changing Room"},
            {"ShopSpecial", "Special Shop"},
            {"Overworld Interiors", "Old House"},
            {"Transit", "Far Shore"},
            {"Temple", "Sealed Temple"},
            {"Shop", "Shop"},
            {"Trinket Well", "Coins in the Well"},
            {"Forest Belltower", "Forest Belltower"},
            {"East Forest Redux", "East Forest"},
            {"Sword Access", "Forest Grave Path"},
            {"East Forest Redux Interior", "Guardhouse 2"},
            {"East Forest Redux Laddercave", "Guardhouse 1"},
            {"Forest Boss Room", "Forest Boss Room"},
            {"Sewer", "Beneath the Well"},
            {"Sewer_Boss", "Dark Tomb Checkpoint"},
            {"Crypt Redux", "Dark Tomb"},
            {"Archipelagos Redux", "West Garden"},
            {"archipelagos_house", "West Garden House"},
            {"Atoll Redux", "Ruined Atoll"},
            {"Frog Stairs", "Frog Stairway"},
            {"frog cave main", "Frog's Domain"},
            {"Library Exterior", "Library Exterior"},
            {"Library Hall", "Library Hall"},
            {"Library Rotunda", "Library Rotunda"},
            {"Library Lab", "Library Lab"},
            {"Library Arena", "Librarian"},
            {"Fortress Basement", "Beneath the Fortress"},
            {"Fortress Main", "Eastern Vault Fortress"},
            {"Fortress East", "Fortress East Shortcut"},
            {"Fortress Reliquary", "Fortress Grave Path"},
            {"Fortress Courtyard", "Fortress Courtyard"},
            {"Dusty", "Fortress Leaf Piles"},
            {"Fortress Arena", "Fortress Arena"},
            {"Mountain", "Lower Mountain"},
            {"Mountaintop", "Top of the Mountain"},
            {"Darkwoods Tunnel", "Quarry Entryway"},
            {"Quarry Redux", "Quarry"},
            {"Monastery", "Monastery"},
            {"ziggurat2020_0", "Rooted Ziggurat Entrance"},
            {"ziggurat2020_1", "Rooted Ziggurat Upper"},
            {"ziggurat2020_2", "Rooted Ziggurat Tower"},
            {"ziggurat2020_3", "Rooted Ziggurat Lower"},
            {"ziggurat2020_FTRoom", "Rooted Ziggurat Teleporter"},
            {"Swamp Redux 2", "Swamp"},
            {"Cathedral Redux", "Cathedral"},
            {"Cathedral Arena", "Cathedral Gauntlet"},
            {"RelicVoid", "Hero's Grave"},
            {"g_elements", "Glyph Tower"},
            {"Spirit Arena", "The Heir"},
            {"Purgatory", "Purgatory"},
            {"Posterity", "Posterity"}
        };

        public static Dictionary<string, string> SceneNamesForSpoilerLog = new Dictionary<string, string>() {
            {"Overworld Redux", "Overworld"},
            {"CubeRoom", "Overworld (Cube Room)"},
            {"Sword Cave", "Overworld (Stick Cave)"},
            {"EastFiligreeCache", "Overworld (Fire Sword Cave)"},
            {"Overworld Cave", "Overworld (Caustic Light Fairy Cave)"},
            {"Ruins Passage", "Overworld (Ruins Passage)"},
            {"PatrolCave", "Overworld (Patrol Fairy Cave)"},
            {"Waterfall", "Overworld (Secret Gathering Place)"},
            {"Ruined Shop", "Overworld (Ruined Shop)"},
            {"Town_FiligreeRoom", "Overworld (Holy Cross Door Cave)"},
            {"Changing Room", "Overworld (Changing Room)"},
            {"Town Basement", "Overworld (Hourglass Room)"},
            {"Overworld Interiors", "Overworld (Old House)"},
            {"Maze Room", "Overworld (Maze Room)"},
            {"Furnace", "Overworld (West Belltower)"},
            {"ShopSpecial", "Overworld (Special Shop)"},
            {"Temple", "Sealed Temple"},
            {"Transit", "Far Shore"},
            {"Shop", "Shop"},
            {"Trinket Well", "Trinket Well"},
            {"Forest Belltower", "East Forest (East Belltower)"},
            {"East Forest Redux", "East Forest"},
            {"Sword Access", "East Forest (Path to Hero's Grave)"},
            {"East Forest Redux Interior", "East Forest (Guardhouse 2)"},
            {"East Forest Redux Laddercave", "East Forest (Guardhouse 1)"},
            {"Sewer", "Beneath the Well"},
            {"Sewer_Boss", "Beneath the Well (Boss Room)"},
            {"Crypt Redux", "Dark Tomb"},
            {"Archipelagos Redux", "West Garden"},
            {"archipelagos_house", "West Garden (Ice Dagger Cave)"},
            {"Atoll Redux", "Ruined Atoll"},
            {"frog cave main", "Frog's Domain"},
            {"Library Hall", "Library (Hall)"},
            {"Library Lab", "Library (Lab)"},
            {"Library Arena", "Library (Librarian)"},
            {"Fortress Courtyard", "Eastern Vault Fortress (Fortress Courtyard)"},
            {"Fortress Basement", "Beneath the Eastern Vault"},
            {"Fortress Main", "Eastern Vault Fortress"},
            {"Fortress East", "Eastern Vault Fortress (Shortcut Path)"},
            {"Fortress Reliquary", "Eastern Vault Fortress (Path to Hero's Grave)"},
            {"Dusty", "Eastern Vault Fortress (Dusty)"},
            {"Fortress Arena", "Siege Engine"},
            {"Mountain", "Mountaintop"},
            {"Mountaintop", "Mountaintop"},
            {"Quarry Redux", "Quarry"},
            {"Monastery", "Monastery"},
            {"ziggurat2020_1", "Rooted Ziggurat Upper"},
            {"ziggurat2020_2", "Rooted Ziggurat Tower"},
            {"ziggurat2020_3", "Rooted Ziggurat Lower"},
            {"Swamp Redux 2", "Swamp"},
            {"Cathedral Redux", "Cathedral"},
            {"Cathedral Arena", "Cathedral (Gauntlet)"},
            {"RelicVoid", "Hero's Grave"}
        };

        public static List<string> HolyCrossExcludedScenes = new List<string>() {
            "Crypt"
        };

        public static Dictionary<string, List<string>> MainAreasToSubAreas = new Dictionary<string, List<string>>() {
            {
                "Overworld",
                new List<string>() {
                    "Waterfall",
                    "Overworld Cave",
                    "Furnace",
                    "Windmill",
                    "ShopSpecial",
                    "CubeRoom",
                    "PatrolCave",
                    "Maze Room",
                    "Sword Cave",
                    "Ruined Shop",
                    "Town Basement",
                    "Ruins Passage",
                    "EastFiligreeCache",
                    "Temple",
                    "Overworld Redux",
                    "Overworld Interiors",
                    "Town_FiligreeRoom",
                    "Changing Room",
                    "Posterity"
                }
            },
            {
                "East Forest",
                new List<string>() {
                    "Forest Belltower",
                    "East Forest Redux",
                    "East Forest Redux Interior",
                    "East Forest Redux Laddercave",
                    "Sword Access",
                    "Forest Boss Room"
                }
            },
            {
                "West Garden",
                new List<string>() {
                    "Archipelagos Redux",
                    "archipelagos_house"
                }
            },
            {
                "Eastern Vault Fortress",
                new List<string>() {
                    "Fortress Basement",
                    "Fortress Main",
                    "Fortress Courtyard",
                    "Fortress Arena",
                    "Fortress East",
                    "Fortress Reliquary",
                    "Dusty"
                }
            },
            {
                "Ruined Atoll",
                new List<string>() {
                    "Atoll Redux"
                }
            },
            {
                "Library",
                new List<string>() {
                    "Library Lab",
                    "Library Hall",
                    "Library Rotunda",
                    "Library Arena",
                    "Library Exterior"
                }
            },
            {
                "Quarry/Mountain",
                new List<string>() {
                    "Quarry Redux",
                    "Monastery",
                    "Darkwoods Tunnel",
                    "Mountain",
                    "Mountaintop",
                }
            },
            {
                "Rooted Ziggurat",
                new List<string>() {
                    "ziggurat2020_2",
                    "ziggurat2020_1",
                    "ziggurat2020_3",
                    "ziggurat2020_0",
                    "ziggurat2020_FTRoom"
                }
            },
            {
                "Swamp",
                new List<string>() {
                    "Swamp Redux 2"
                }
            },
            {
                "Cathedral",
                new List<string>() {
                    "Cathedral Arena",
                    "Cathedral Redux"
                }
            },
            {
                "Dark Tomb",
                new List<string>() {
                    "Crypt Redux"
                }
            },
            {
                "Beneath the Well",
                new List<string>() {
                    "Sewer",
                    "Sewer_Boss"
                }
            },
            {
                "Frog's Domain",
                new List<string>() {
                    "Frog Stairs",
                    "frog cave main"
                }
            },
            {
                "Far Shore/Hero's Grave",
                new List<string>() {
                    "Resurrection",
                    "Transit",
                    "RelicVoid",
                    "Playable Intro",
                    "Spirit Arena",
                    "g_elements"
                }
            },
            {
                "Shop/Coin Wells",
                new List<string>() {
                    "Shop",
                    "Trinket Well"
                }
            },
        };


        public static Dictionary<string, List<string>> PopTrackerMapScenes = new Dictionary<string, List<string>>() {
            { 
                "Overworld",
                new List<string> () {
                    "Waterfall",
                    "Overworld Cave",
                    "Furnace",
                    "Windmill",
                    "ShopSpecial",
                    "CubeRoom",
                    "PatrolCave",
                    "Maze Room",
                    "Sword Cave",
                    "Ruined Shop",
                    "Town Basement",
                    "Ruins Passage",
                    "EastFiligreeCache",
                    "Temple",
                    "Overworld Redux",
                    "Overworld Interiors",
                    "Town_FiligreeRoom",
                    "Changing Room",
                    "Posterity",
                    "Mountain",
                    "Mountaintop",
                    "g_elements",
                    "Playable Intro",
                }
            },
            {
                "East Forest",
                new List<string> () {
                    "Forest Belltower",
                    "East Forest Redux",
                    "East Forest Redux Interior",
                    "East Forest Redux Laddercave",
                    "Sword Access",
                    "Forest Boss Room"
                }
            },
            {
                "Beneath the Well",
                new List<string> () {
                    "Sewer",
                    "Sewer_Boss"
                }
            },
            {
                "Dark Tomb",
                new List<string> () {
                    "Crypt Redux"
                }
            },
            {
                "West Gardens",
                new List<string> () {
                    "Archipelagos Redux",
                    "archipelagos_house"
                }
            },
            {
                "Beneath the Earth",
                new List<string> () {
                    "Fortress Basement",
                }
            },
            {
                "Eastern Vault",
                new List<string> () {
                    "Fortress Main",
                    "Fortress Courtyard",
                    "Fortress Arena",
                    "Fortress East",
                    "Fortress Reliquary",
                    "Dusty"
                }
            },
            {
                "Ruined Atoll",
                new List<string> () {
                    "Atoll Redux"
                }
            },
            {
                "Frog's Domain",
                new List<string> () {
                    "Frog Stairs",
                    "frog cave main"
                }
            },
            {
                "The Grand Library",
                new List<string> () {
                    "Library Lab",
                    "Library Hall",
                    "Library Rotunda",
                    "Library Arena",
                    "Library Exterior"
                }
            },
            {
                "The Quarry",
                new List<string> () {
                    "Quarry Redux",
                    "Monastery",
                    "Darkwoods Tunnel",
                }
            },
            {
                "The Rooted Ziggurat",
                new List<string> () {
                    "ziggurat2020_2",
                    "ziggurat2020_1",
                    "ziggurat2020_3",
                    "ziggurat2020_0",
                    "ziggurat2020_FTRoom"
                }
            },
            {
                "Swamp",
                new List<string> () {
                    "Swamp Redux 2"
                }
            },
            {
                "The Cathedral",
                new List<string> () {
                    "Cathedral Arena",
                    "Cathedral Redux"
                }
            },
            {
                "The Far Shore",
                new List<string> () {
                    "Transit",
                    "RelicVoid",
                    "Spirit Arena",
                }
            }
        };
    }
}
