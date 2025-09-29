using System.Collections.Generic;
using System.Linq;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;
using static TunicRandomizer.ERData;

namespace TunicRandomizer {
    public class SceneImageData {

        public struct SceneImage {
            public string PortalName;
            public string TextureName;
            public int TextureX;
            public int TextureY;
            public int Width;
            public int Height;

            public SceneImage(string portalName, string textureName, int textureX, int textureY, int width = 300, int height = 180) {
                PortalName = portalName;
                TextureName = textureName;
                TextureX = textureX;
                TextureY = textureY;
                Width = width;
                Height = height;
            }
        }
        public static int index = 0;

        public static Dictionary<string, GameObject> Pages = new Dictionary<string, GameObject>();

        public static Dictionary<string, Texture2D> PageTextures = new Dictionary<string, Texture2D>();

        public static List<string> PortalNames = new List<string>();

        public static Dictionary<string, SceneImage> SceneImages = new Dictionary<string, SceneImage>() {
             {
                 "Stick House Entrance",
                 new SceneImage("Stick House Entrance", "leaf_14r", 1090, 400)
             },
             {
                 "Windmill Entrance",
                 new SceneImage("Windmill Entrance", "leaf_14r", 750, 900)
             },
             {
                 "Old House Waterfall Entrance",
                 new SceneImage("Old House Waterfall Entrance", "leaf_14r", 1090, 975)
             },
             {
                 "Entrance to Furnace under Windmill",
                 new SceneImage("Entrance to Furnace under Windmill", "leaf_14r", 450, 825)
             },
             {
                 "Ruined Shop Entrance",
                 new SceneImage("Ruined Shop Entrance", "leaf_14r", 450, 600)
             },
             {
                 "Changing Room Entrance",
                 new SceneImage("Changing Room Entrance", "leaf_14r", 775, 500)
             },
             {
                 "Dark Tomb Main Entrance",
                 new SceneImage("Dark Tomb Main Entrance", "leaf_14r", 525, 1100)
             },
             {
                 "Secret Gathering Place Entrance",
                 new SceneImage("Secret Gathering Place Entrance", "leaf_14r", 725, 1100)
             },
             {
                 "Overworld to Forest Belltower",
                 new SceneImage("Overworld to Forest Belltower", "leaf_14r", 1750, 675)
             },
             {
                 "Overworld to Fortress",
                 new SceneImage("Overworld to Fortress", "leaf_14r", 1700, 900)
             },
             {
                 "Patrol Cave Entrance",
                 new SceneImage("Patrol Cave Entrance", "leaf_14r", 1650, 1100)
             },
             {
                 "Stairs from Overworld to Mountain",
                 new SceneImage("Stairs from Overworld to Mountain", "leaf_14r", 1175, 1290)
             },
             {
                 "Temple Rafters Entrance",
                 new SceneImage("Temple Rafters Entrance", "leaf_14r", 1050, 1150)
             },
             {
                 "Overworld to Quarry Connector",
                 new SceneImage("Overworld to Quarry Connector", "leaf_14r", 250, 1150)
             },
             {
                 "Atoll Lower Entrance",
                 new SceneImage("Atoll Lower Entrance", "leaf_14r", 500, 100)
             },
             {
                 "Hourglass Cave Entrance",
                 new SceneImage("Hourglass Cave Entrance", "leaf_14r", 605, 300)
             },
             {
                 "Maze Cave Entrance",
                 new SceneImage("Maze Cave Entrance", "leaf_14r", 1175, 325)
             },
             {
                 "Entrance to Furnace from Beach",
                 new SceneImage("Entrance to Furnace from Beach", "leaf_14r", 275, 500)
             },
             {
                 "Atoll Upper Entrance",
                 new SceneImage("Atoll Upper Entrance", "leaf_14r", 600, 100)
             },
             {
                 "Well Ladder Entrance",
                 new SceneImage("Well Ladder Entrance", "leaf_14r", 900, 1000)
             },
             {
                 "Entrance to Well from Well Rail",
                 new SceneImage("Entrance to Well from Well Rail", "leaf_14r", 275, 1000)
             },
             {
                 "Entrance to Furnace from Well Rail",
                 new SceneImage("Entrance to Furnace from Well Rail", "leaf_14r", 275, 1000)
             },
             {
                 "Old House Door Entrance",
                 new SceneImage("Old House Door Entrance", "leaf_14r", 935, 760)
             },
             {
                 "West Garden Entrance near Belltower",
                 new SceneImage("West Garden Entrance near Belltower", "leaf_14r", 50, 800)
             },
             {
                 "Entrance to Furnace near West Garden",
                 new SceneImage("Entrance to Furnace near West Garden", "leaf_14r", 150, 675)
             },
             {
                 "West Garden Entrance from Furnace",
                 new SceneImage("West Garden Entrance from Furnace", "leaf_14r", 0, 675)
             },
             {
                 "Caustic Light Cave Entrance",
                 new SceneImage("Caustic Light Cave Entrance", "leaf_14r", 1735, 250)
             },
             {
                 "Swamp Lower Entrance",
                 new SceneImage("Swamp Lower Entrance", "leaf_14r", 1750, 175)
             },
             {
                 "Swamp Upper Entrance",
                 new SceneImage("Swamp Upper Entrance", "leaf_14r", 1725, 50)
             },
             {
                 "Ruined Passage Door Entrance",
                 new SceneImage("Ruined Passage Door Entrance", "leaf_14r", 1300, 450)
             },
             {
                 "Ruined Passage Not-Door Entrance",
                 new SceneImage("Ruined Passage Not-Door Entrance", "leaf_14r", 1500, 450)
             },
             {
                 "Special Shop Entrance",
                 new SceneImage("Special Shop Entrance", "leaf_14r", 1700, 780)
             },
             {
                 "West Garden Laurels Entrance",
                 new SceneImage("West Garden Laurels Entrance", "leaf_14r", 0, 425)
             },
             {
                 "Temple Door Entrance",
                 new SceneImage("Temple Door Entrance", "leaf_14r", 1175, 1075)
             },
             {
                 "Fountain HC Door Entrance",
                 new SceneImage("Fountain HC Door Entrance", "leaf_14r", 605, 575)
             },
             {
                 "Southeast HC Door Entrance",
                 new SceneImage("Southeast HC Door Entrance", "leaf_14r", 1735, 350)
             },
             {
                 "Town to Far Shore",
                 new SceneImage("Town to Far Shore", "leaf_14r", 690, 715)
             },
             {
                 "Spawn to Far Shore",
                 new SceneImage("Spawn to Far Shore", "leaf_14r", 1175, 185)
             },
             {
                 "Cube Cave Entrance",
                 new SceneImage("Cube Cave Entrance", "leaf_14r", 1000, 525)
             },
             {
                 "Secret Gathering Place Exit",
                 new SceneImage("Secret Gathering Place Exit", "leaf_24r", 175, 850)
             },
             {
                 "Windmill Exit",
                 new SceneImage("Windmill Exit", "leaf_14r", 750, 900)
             },
             {
                 "Windmill Shop",
                 new SceneImage("Windmill Shop", "leaf_14r", 750, 900)
             },
             {
                 "Old House Door Exit",
                 new SceneImage("Old House Door Exit", "page grungifier (11r)", 600, 125)
             },
             {
                 "Old House to Glyph Tower",
                 new SceneImage("Old House to Glyph Tower", "page grungifier (11r)", 900, 140)
             },
             {
                 "Old House Waterfall Exit",
                 new SceneImage("Old House Waterfall Exit", "page grungifier (11r)", 875, 350)
             },
             {
                 "Glyph Tower Exit",
                 new SceneImage("Glyph Tower Exit", "leaf_27r", 300, 215)
             },
             {
                 "Changing Room Exit",
                 new SceneImage("Changing Room Exit", "leaf_14r", 775, 500)
             },
             {
                 "Fountain HC Room Exit",
                 new SceneImage("Fountain HC Room Exit", "leaf_14r", 605, 575)
             },
             {
                 "Cube Cave Exit",
                 new SceneImage("Cube Cave Exit", "leaf_14r", 1000, 525)
             },
             {
                 "Guard Patrol Cave Exit",
                 new SceneImage("Guard Patrol Cave Exit", "leaf_14r", 1650, 1100)
             },
             {
                 "Ruined Shop Exit",
                 new SceneImage("Ruined Shop Exit", "leaf_14r", 450, 600)
             },
             {
                 "Furnace Exit towards Well",
                 new SceneImage("Furnace Exit towards Well", "leaf_14r", 275, 1000)
             },
             {
                 "Furnace Exit to Dark Tomb",
                 new SceneImage("Furnace Exit to Dark Tomb", "leaf_15r", 900, 50)
             },
             {
                 "Furnace Exit towards West Garden",
                 new SceneImage("Furnace Exit towards West Garden", "leaf_14r", 150, 675)
             },
             {
                 "Furnace Exit to Beach",
                 new SceneImage("Furnace Exit to Beach", "leaf_14r", 275, 500)
             },
             {
                 "Furnace Exit under Windmill",
                 new SceneImage("Furnace Exit under Windmill", "leaf_14r", 450, 825)
             },
             {
                 "Stick House Exit",
                 new SceneImage("Stick House Exit", "leaf_14r", 1090, 400)
             },
             {
                 "Ruined Passage Not-Door Exit",
                 new SceneImage("Ruined Passage Not-Door Exit", "leaf_14r", 1500, 450)
             },
             {
                 "Ruined Passage Door Exit",
                 new SceneImage("Ruined Passage Door Exit", "leaf_14r", 1300, 450)
             },
             {
                 "Southeast HC Room Exit",
                 new SceneImage("Southeast HC Room Exit", "leaf_14r", 1735, 350)
             },
             {
                 "Caustic Light Cave Exit",
                 new SceneImage("Caustic Light Cave Exit", "leaf_14r", 1735, 250)
             },
             {
                 "Maze Cave Exit",
                 new SceneImage("Maze Cave Exit", "page grungifier (26r)", 1450, 80)
             },
             {
                 "Hourglass Cave Exit",
                 new SceneImage("Hourglass Cave Exit", "leaf_8v", 1175, 1050)
             },
             {
                 "Special Shop Exit",
                 new SceneImage("Special Shop Exit", "leaf_14r", 1700, 780)
             },
             {
                 "Temple Rafters Exit",
                 new SceneImage("Temple Rafters Exit", "leaf_12v", 500, 450, 600, 360)
             },
             {
                 "Temple Door Exit",
                 new SceneImage("Temple Door Exit", "leaf_12v", 500, 450, 600, 360)
             },
             {
                 "Forest Belltower to Guard Captain Room",
                 new SceneImage("Forest Belltower to Guard Captain Room", "leaf_14r", 1750, 675)
             },
             {
                 "Forest Belltower to Overworld",
                 new SceneImage("Forest Belltower to Overworld", "leaf_14r", 1750, 675)
             },
             {
                 "Forest Belltower to Fortress",
                 new SceneImage("Forest Belltower to Fortress", "leaf_14r", 1750, 675)
             },
             {
                 "Forest Belltower to Forest",
                 new SceneImage("Forest Belltower to Forest", "leaf_14r", 1750, 675)
             },
             {
                 "Forest to Belltower",
                 new SceneImage("Forest to Belltower", "leaf_28", 0, 540)
             },
             {
                 "Forest Guard House 1 Lower Entrance",
                 new SceneImage("Forest Guard House 1 Lower Entrance", "leaf_28", 600, 360)
             },
             {
                 "Forest Guard House 1 Gate Entrance",
                 new SceneImage("Forest Guard House 1 Gate Entrance", "leaf_28", 300, 360)
             },
             {
                 "Forest Guard House 2 Upper Entrance",
                 new SceneImage("Forest Guard House 2 Upper Entrance", "leaf_28", 600, 540)
             },
             {
                 "Forest Grave Path Lower Entrance",
                 new SceneImage("Forest Grave Path Lower Entrance", "leaf_28", 300, 180)
             },
             {
                 "Forest Grave Path Upper Entrance",
                 new SceneImage("Forest Grave Path Upper Entrance", "leaf_28", 600, 180)
             },
             {
                 "Forest Dance Fox Outside Doorway",
                 new SceneImage("Forest Dance Fox Outside Doorway", "leaf_28", 0, 360)
             },
             {
                 "Forest to Far Shore",
                 new SceneImage("Forest to Far Shore", "leaf_28", 300, 540)
             },
             {
                 "Forest Guard House 2 Lower Entrance",
                 new SceneImage("Forest Guard House 2 Lower Entrance", "leaf_28", 900, 180)
             },
             {
                 "Forest Grave Path Upper Exit",
                 new SceneImage("Forest Grave Path Upper Exit", "leaf_28", 900, 360)
             },
             {
                 "Forest Grave Path Lower Exit",
                 new SceneImage("Forest Grave Path Lower Exit", "leaf_28", 0, 180)
             },
             {
                 "East Forest Hero's Grave",
                 new SceneImage("East Forest Hero's Grave", "leaf_28", 900, 540)
             },
             {
                 "Guard House 1 Dance Fox Exit",
                 new SceneImage("Guard House 1 Dance Fox Exit", "leaf_14r", 1750, 675)
             },
             {
                 "Guard House 1 Lower Exit",
                 new SceneImage("Guard House 1 Lower Exit", "leaf_14r", 1750, 675)
             },
             {
                 "Guard House 1 Upper Forest Exit",
                 new SceneImage("Guard House 1 Upper Forest Exit", "leaf_14r", 1750, 675)
             },
             {
                 "Guard House 1 to Guard Captain Room",
                 new SceneImage("Guard House 1 to Guard Captain Room", "leaf_14r", 1750, 675)
             },
             {
                 "Guard House 2 Upper Exit",
                 new SceneImage("Guard House 2 Upper Exit", "leaf_14r", 1750, 675)
             },
             {
                 "Guard House 2 Lower Exit",
                 new SceneImage("Guard House 2 Lower Exit", "leaf_14r", 1750, 675)
             },
             {
                 "Guard Captain Room Non-Gate Exit",
                 new SceneImage("Guard Captain Room Non-Gate Exit", "leaf_5r", 935, 825)
             },
             {
                 "Guard Captain Room Gate Exit",
                 new SceneImage("Guard Captain Room Gate Exit", "leaf_5r", 935, 825)
             },
             {
                 "Well Ladder Exit",
                 new SceneImage("Well Ladder Exit", "leaf_14v", 750, 25)
             },
             {
                 "Well to Well Boss",
                 new SceneImage("Well to Well Boss", "leaf_14v", 260, 225)
             },
             {
                 "Well Exit towards Furnace",
                 new SceneImage("Well Exit towards Furnace", "leaf_14v", 50, 325)
             },
             {
                 "Well Boss to Well",
                 new SceneImage("Well Boss to Well", "leaf_14v", 260, 225)
             },
             {
                 "Checkpoint to Dark Tomb",
                 new SceneImage("Checkpoint to Dark Tomb", "leaf_15r", 1450, 150)
             },
             {
                 "Dark Tomb to Overworld",
                 new SceneImage("Dark Tomb to Overworld", "leaf_15r", 1400, 275)
             },
             {
                 "Dark Tomb to Checkpoint",
                 new SceneImage("Dark Tomb to Checkpoint", "leaf_15r", 1450, 275)
             },
             {
                 "Dark Tomb to Furnace",
                 new SceneImage("Dark Tomb to Furnace", "leaf_15r", 950, 80)
             },
             {
                 "West Garden Exit near Hero's Grave",
                 new SceneImage("West Garden Exit near Hero's Grave", "leaf_13v", 1750, 600)
             },
             {
                 "West Garden to Magic Dagger House",
                 new SceneImage("West Garden to Magic Dagger House", "leaf_13v", 1350, 200)
             },
             {
                 "West Garden Shop",
                 new SceneImage("West Garden Shop", "leaf_13v", 1075, 750)
             },
             {
                 "West Garden Exit after Boss",
                 new SceneImage("West Garden Exit after Boss", "leaf_13v", 1750, 750)
             },
             {
                 "West Garden Laurels Exit",
                 new SceneImage("West Garden Laurels Exit", "leaf_13v", 1750, 350)
             },
             {
                 "West Garden Hero's Grave",
                 new SceneImage("West Garden Hero's Grave", "leaf_13v", 1550, 650)
             },
             {
                 "West Garden to Far Shore",
                 new SceneImage("West Garden to Far Shore", "leaf_13v", 1350, 25)
             },
             {
                 "Magic Dagger House Exit",
                 new SceneImage("Magic Dagger House Exit", "leaf_13v", 1350, 200)
             },
             {
                 "Fortress Courtyard to Fortress Grave Path Lower",
                 new SceneImage("Fortress Courtyard to Fortress Grave Path Lower", "leaf_16r", 750, 475)
             },
             {
                 "Fortress Courtyard to Fortress Interior",
                 new SceneImage("Fortress Courtyard to Fortress Interior", "leaf_16r", 515, 550)
             },
             {
                 "Fortress Courtyard to Fortress Grave Path Upper",
                 new SceneImage("Fortress Courtyard to Fortress Grave Path Upper", "leaf_16r", 850, 485)
             },
             {
                 "Fortress Courtyard to East Fortress",
                 new SceneImage("Fortress Courtyard to East Fortress", "leaf_16r", 950, 590)
             },
             {
                 "Fortress Courtyard Shop",
                 new SceneImage("Fortress Courtyard Shop", "leaf_16r", 250, 450)
             },
             {
                 "Fortress Courtyard to Beneath the Earth",
                 new SceneImage("Fortress Courtyard to Beneath the Earth", "leaf_16r", 105, 400)
             },
             {
                 "Fortress Courtyard to Forest Belltower",
                 new SceneImage("Fortress Courtyard to Forest Belltower", "leaf_16r", 500, 0)
             },
             {
                 "Fortress Courtyard to Overworld",
                 new SceneImage("Fortress Courtyard to Overworld", "leaf_16r", 300, 0)
             },
             {
                 "Beneath the Earth to Fortress Interior",
                 new SceneImage("Beneath the Earth to Fortress Interior", "leaf_15r", 1650, 1325)
             },
             {
                 "Beneath the Earth to Fortress Courtyard",
                 new SceneImage("Beneath the Earth to Fortress Courtyard", "leaf_15r", 1650, 725)
             },
             {
                 "Fortress Interior Main Exit",
                 new SceneImage("Fortress Interior Main Exit", "leaf_16r", 520, 750)
             },
             {
                 "Fortress Interior to Beneath the Earth",
                 new SceneImage("Fortress Interior to Beneath the Earth", "leaf_16r", 115, 975)
             },
             {
                 "Fortress Interior Shop",
                 new SceneImage("Fortress Interior Shop", "leaf_16r", 300, 825)
             },
             {
                 "Fortress Interior to East Fortress Upper",
                 new SceneImage("Fortress Interior to East Fortress Upper", "leaf_16r", 800, 850)
             },
             {
                 "Fortress Interior to East Fortress Lower",
                 new SceneImage("Fortress Interior to East Fortress Lower", "leaf_16r", 775, 725)
             },
             {
                 "Fortress Interior to Siege Engine Arena",
                 new SceneImage("Fortress Interior to Siege Engine Arena", "leaf_16r", 520, 1040)
             },
             {
                 "East Fortress to Interior Lower",
                 new SceneImage("East Fortress to Interior Lower", "leaf_16r", 775, 725)
             },
             {
                 "East Fortress to Courtyard",
                 new SceneImage("East Fortress to Courtyard", "leaf_16r", 975, 750)
             },
             {
                 "East Fortress to Interior Upper",
                 new SceneImage("East Fortress to Interior Upper", "leaf_16r", 800, 850)
             },
             {
                 "Fortress Grave Path Lower Exit",
                 new SceneImage("Fortress Grave Path Lower Exit", "leaf_16r", 1050, 450)
             },
             {
                 "Fortress Grave Path Upper Exit",
                 new SceneImage("Fortress Grave Path Upper Exit", "leaf_16r", 1050, 500)
             },
             {
                 "Fortress Grave Path Dusty Entrance",
                 new SceneImage("Fortress Grave Path Dusty Entrance", "leaf_16r", 1500, 550)
             },
             {
                 "Dusty Exit",
                 new SceneImage("Dusty Exit", "leaf_16r", 1500, 550)
             },
             {
                 "Fortress Hero's Grave",
                 new SceneImage("Fortress Hero's Grave", "leaf_16r", 1550, 475)
             },
             {
                 "Siege Engine Arena to Fortress",
                 new SceneImage("Siege Engine Arena to Fortress", "leaf_16r", 350, 1200, 600, 360)
             },
             {
                 "Fortress to Far Shore",
                 new SceneImage("Fortress to Far Shore", "leaf_16r", 350, 1200, 600, 360)
             },
             {
                 "Atoll Upper Exit",
                 new SceneImage("Atoll Upper Exit", "leaf_17r", 1100, 1375)
             },
             {
                 "Atoll Shop",
                 new SceneImage("Atoll Shop", "leaf_17r", 1350, 1100)
             },
             {
                 "Atoll Lower Exit",
                 new SceneImage("Atoll Lower Exit", "leaf_17r", 1000, 1375)
             },
             {
                 "Atoll to Far Shore",
                 new SceneImage("Atoll to Far Shore", "leaf_17r", 1100, 1300)
             },
             {
                 "Atoll Statue Teleporter",
                 new SceneImage("Atoll Statue Teleporter", "leaf_17r", 1100, 775)
             },
             {
                 "Frog Stairs Eye Entrance",
                 new SceneImage("Frog Stairs Eye Entrance", "leaf_17r", 1750, 1125)
             },
             {
                 "Frog Stairs Mouth Entrance",
                 new SceneImage("Frog Stairs Mouth Entrance", "leaf_17r", 1700, 1050)
             },
             {
                 "Frog Stairs Eye Exit",
                 new SceneImage("Frog Stairs Eye Exit", "leaf_17v", 475, 1000)
             },
             {
                 "Frog Stairs Mouth Exit",
                 new SceneImage("Frog Stairs Mouth Exit", "leaf_17v", 475, 1000)
             },
             {
                 "Frog Stairs to Frog's Domain's Exit",
                 new SceneImage("Frog Stairs to Frog's Domain's Exit", "leaf_17v", 400, 1250)
             },
             {
                 "Frog's Domain Orb Exit",
                 new SceneImage("Frog's Domain Orb Exit", "leaf_17v", 1750, 1150)
             },
             {
                 "Frog Stairs to Frog's Domain's Entrance",
                 new SceneImage("Frog Stairs to Frog's Domain's Entrance", "leaf_17v", 400, 1250)
             },
             {
                 "Frog's Domain Ladder Exit",
                 new SceneImage("Frog's Domain Ladder Exit", "leaf_17v", 1280, 850)
             },
             {
                 "Library Exterior Tree",
                 new SceneImage("Library Exterior Tree", "leaf_16v", 550, 1000)
             },
             {
                 "Library Exterior Ladder",
                 new SceneImage("Library Exterior Ladder", "leaf_16v", 550, 1000)
             },
             {
                 "Library Hall Bookshelf Exit",
                 new SceneImage("Library Hall Bookshelf Exit", "leaf_16v", 500, 1050)
             },
             {
                 "Library Hero's Grave",
                 new SceneImage("Library Hero's Grave", "leaf_11v", 300, 200)
             },
             {
                 "Library Hall to Rotunda",
                 new SceneImage("Library Hall to Rotunda", "leaf_16v", 425, 1150)
             },
             {
                 "Library Rotunda Lower Exit",
                 new SceneImage("Library Rotunda Lower Exit", "leaf_16v", 425, 1150)
             },
             {
                 "Library Rotunda Upper Exit",
                 new SceneImage("Library Rotunda Upper Exit", "leaf_16v", 425, 1150)
             },
             {
                 "Library Lab to Rotunda",
                 new SceneImage("Library Lab to Rotunda", "leaf_16v", 425, 1150)
             },
             {
                 "Library to Far Shore",
                 new SceneImage("Library to Far Shore", "leaf_20v", 1650, 390)
             },
             {
                 "Library Lab to Librarian Arena",
                 new SceneImage("Library Lab to Librarian Arena", "leaf_20v", 1800, 350)
             },
             {
                 "Librarian Arena Exit",
                 new SceneImage("Librarian Arena Exit", "leaf_18r", 1000, 1050, 600, 360)
             },
             {
                 "Stairs to Top of the Mountain",
                 new SceneImage("Stairs to Top of the Mountain", "leaf_25r", 200, 300, 600, 360)
             },
             {
                 "Mountain to Quarry",
                 new SceneImage("Mountain to Quarry", "leaf_25r", 800, 100, 600, 360)
             },
             {
                 "Mountain to Overworld",
                 new SceneImage("Mountain to Overworld", "leaf_25r", 800, 100, 600, 360)
             },
             {
                 "Top of the Mountain Exit",
                 new SceneImage("Top of the Mountain Exit", "leaf_25r", 250, 375)
             },
             {
                 "Quarry Connector to Overworld",
                 new SceneImage("Quarry Connector to Overworld", "leaf_14r", 250, 1150)
             },
             {
                 "Quarry Connector to Quarry",
                 new SceneImage("Quarry Connector to Quarry", "leaf_19r", 1750, 150)
             },
             {
                 "Quarry to Overworld Exit",
                 new SceneImage("Quarry to Overworld Exit", "leaf_19r", 1750, 150)
             },
             {
                 "Quarry Shop",
                 new SceneImage("Quarry Shop", "leaf_19r", 1475, 200)
             },
             {
                 "Quarry to Monastery Front",
                 new SceneImage("Quarry to Monastery Front", "leaf_19r", 900, 1150)
             },
             {
                 "Quarry to Monastery Back",
                 new SceneImage("Quarry to Monastery Back", "leaf_19r", 700, 1250)
             },
             {
                 "Quarry to Mountain",
                 new SceneImage("Quarry to Mountain", "leaf_19r", 825, 1325)
             },
             {
                 "Quarry to Ziggurat",
                 new SceneImage("Quarry to Ziggurat", "leaf_19r", 1750, 700)
             },
             {
                 "Quarry to Far Shore",
                 new SceneImage("Quarry to Far Shore", "leaf_19r", 1550, 75)
             },
             {
                 "Monastery Rear Exit",
                 new SceneImage("Monastery Rear Exit", "leaf_19r", 700, 1250)
             },
             {
                 "Monastery Front Exit",
                 new SceneImage("Monastery Front Exit", "leaf_19r", 900, 1150)
             },
             {
                 "Monastery Hero's Grave",
                 new SceneImage("Monastery Hero's Grave", "leaf_11v", 250, 1175)
             },
             {
                 "Ziggurat Entry Hallway to Ziggurat Upper",
                 new SceneImage("Ziggurat Entry Hallway to Ziggurat Upper", "leaf_19r", 325, 650)
             },
             {
                 "Ziggurat Entry Hallway to Quarry",
                 new SceneImage("Ziggurat Entry Hallway to Quarry", "leaf_19r", 325, 650)
             },
             {
                 "Ziggurat Upper to Ziggurat Entry Hallway",
                 new SceneImage("Ziggurat Upper to Ziggurat Entry Hallway", "leaf_28", 0, 0)
             },
             {
                 "Ziggurat Upper to Ziggurat Tower",
                 new SceneImage("Ziggurat Upper to Ziggurat Tower", "leaf_28", 900, 0)
             },
             {
                 "Ziggurat Tower to Ziggurat Upper",
                 new SceneImage("Ziggurat Tower to Ziggurat Upper", "leaf_28", 900, 0)
             },
             {
                 "Ziggurat Tower to Ziggurat Lower",
                 new SceneImage("Ziggurat Tower to Ziggurat Lower", "leaf_28", 600, 0)
             },
             {
                 "Ziggurat Lower to Ziggurat Tower",
                 new SceneImage("Ziggurat Lower to Ziggurat Tower", "leaf_28", 600, 0)
             },
             {
                 "Ziggurat Portal Room Entrance",
                 new SceneImage("Ziggurat Portal Room Entrance", "leaf_18v", 535, 950, 600, 360)
             },
             {
                 "Ziggurat Lower Falling Entrance",
                 new SceneImage("Ziggurat Lower Falling Entrance", "leaf_28", 300, 0)
             },
             {
                 "Ziggurat Portal Room Exit",
                 new SceneImage("Ziggurat Portal Room Exit", "leaf_19r", 325, 650)
             },
             {
                 "Ziggurat to Far Shore",
                 new SceneImage("Ziggurat to Far Shore", "leaf_19r", 325, 650)
             },
             {
                 "Swamp Lower Exit",
                 new SceneImage("Swamp Lower Exit", "leaf_19v", 675, 450)
             },
             {
                 "Swamp Shop",
                 new SceneImage("Swamp Shop", "leaf_19v", 1075, 450)
             },
             {
                 "Swamp to Cathedral Main Entrance",
                 new SceneImage("Swamp to Cathedral Main Entrance", "leaf_19v", 1415, 950)
             },
             {
                 "Swamp to Cathedral Secret Legend Room Entrance",
                 new SceneImage("Swamp to Cathedral Secret Legend Room Entrance", "leaf_19v", 1350, 950)
             },
             {
                 "Swamp to Gauntlet",
                 new SceneImage("Swamp to Gauntlet", "leaf_19v", 1415, 850)
             },
             {
                 "Swamp Upper Exit",
                 new SceneImage("Swamp Upper Exit", "leaf_19v", 675, 550)
             },
             {
                 "Swamp Hero's Grave",
                 new SceneImage("Swamp Hero's Grave", "leaf_19v", 1530, 685)
             },
             {
                 "Cathedral Main Exit",
                 new SceneImage("Cathedral Main Exit", "leaf_20r", 650, 750)
             },
             {
                 "Cathedral Elevator",
                 new SceneImage("Cathedral Elevator", "leaf_20r", 400, 725)
             },
             {
                 "Cathedral Secret Legend Room Exit",
                 new SceneImage("Cathedral Secret Legend Room Exit", "leaf_20r", 225, 650)
             },
             {
                 "Gauntlet to Swamp",
                 new SceneImage("Gauntlet to Swamp", "leaf_20r", 540, 50)
             },
             {
                 "Gauntlet Elevator",
                 new SceneImage("Gauntlet Elevator", "leaf_20r", 400, 450)
             },
             {
                 "Gauntlet Shop",
                 new SceneImage("Gauntlet Shop", "leaf_20r", 500, 400)
             },
             {
                 "Far Shore to West Garden",
                 new SceneImage("Far Shore to West Garden", "leaf_20v", 700, 525)
             },
             {
                 "Far Shore to Library",
                 new SceneImage("Far Shore to Library", "leaf_20v", 1050, 375)
             },
             {
                 "Far Shore to Quarry",
                 new SceneImage("Far Shore to Quarry", "leaf_20v", 775, 700)
             },
             {
                 "Far Shore to East Forest",
                 new SceneImage("Far Shore to East Forest", "leaf_21r", 0, 225)
             },
             {
                 "Far Shore to Fortress",
                 new SceneImage("Far Shore to Fortress", "leaf_20v", 1050, 700)
             },
             {
                 "Far Shore to Atoll",
                 new SceneImage("Far Shore to Atoll", "leaf_20v", 775, 375)
             },
             {
                 "Far Shore to Ziggurat",
                 new SceneImage("Far Shore to Ziggurat", "leaf_20v", 900, 800)
             },
             {
                 "Far Shore to Heir",
                 new SceneImage("Far Shore to Heir", "leaf_20v", 910, 525)
             },
             {
                 "Far Shore to Town",
                 new SceneImage("Far Shore to Town", "leaf_20v", 1150, 525)
             },
             {
                 "Far Shore to Spawn",
                 new SceneImage("Far Shore to Spawn", "leaf_20v", 910, 0)
             },
             {
                 "Hero's Grave to Fortress",
                 new SceneImage("Hero's Grave to Fortress", "leaf_21r", 1515, 525)
             },
             {
                 "Hero's Grave to Monastery",
                 new SceneImage("Hero's Grave to Monastery", "leaf_21r", 1400, 500)
             },
             {
                 "Hero's Grave to West Garden",
                 new SceneImage("Hero's Grave to West Garden", "leaf_21r", 1615, 475)
             },
             {
                 "Hero's Grave to East Forest",
                 new SceneImage("Hero's Grave to East Forest", "leaf_21r", 1475, 375)
             },
             {
                 "Hero's Grave to Library",
                 new SceneImage("Hero's Grave to Library", "leaf_21r", 1350, 425)
             },
             {
                 "Hero's Grave to Swamp",
                 new SceneImage("Hero's Grave to Swamp", "leaf_21r", 1575, 400)
             },
             {
                 "Heir Arena Exit",
                 new SceneImage("Heir Arena Exit", "leaf_20v", 125, 145)
             },
             {
                 "Purgatory Bottom Exit",
                 new SceneImage("Purgatory Bottom Exit", "leaf_4v", 465, 175)
             },
             {
                 "Purgatory Top Exit",
                 new SceneImage("Purgatory Top Exit", "leaf_4v", 465, 175)
             },
             {
                 "Shop Portal",
                 new SceneImage("Shop Portal", "leaf_13r", 1115, 145)
             },
        };

        public static void LoadPageData() {

            List<GameObject> pages = Resources.LoadAll("PagePrefabs").Select(obj => obj.Cast<GameObject>()).ToList();
            foreach (var page in pages) {
                GameObject pageClone = GameObject.Instantiate(page);
                GameObject.DontDestroyOnLoad(pageClone.gameObject);
                pageClone.name = pageClone.name.Replace("(Clone)", "");
                Pages.Add(pageClone.name, pageClone);
                PageTextures.Add(pageClone.name, pageClone.GetComponent<SpriteRenderer>().sprite.texture);
                if (page.GetComponent<PageData>() != null) { 
                    if (page.GetComponent<PageData>().grungifyTexture != null) {
                        Pages.Add(page.GetComponent<PageData>().grungifyTexture.name, pageClone);
                        PageTextures.Add(page.GetComponent<PageData>().grungifyTexture.name, page.GetComponent<PageData>().grungifyTexture);
                    }
                }
                pageClone.SetActive(false);
            }

            GetPortalNames();
            LoadCustomMaps();
        }

        public static void createSprite(string texture, int x, int y, int width = 300, int height = 180) {
            Sprite sprite = Sprite.CreateSprite(PageTextures[texture], new Rect(x, y, width, height), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect, Vector4.zero, false);
            EntranceSelector.SceneSprite1.sprite = sprite;
            if (width > 300) {
                EntranceSelector.SceneSprite1.transform.localScale = new Vector3(35, 40, 35);
            } else {
                EntranceSelector.SceneSprite1.transform.localScale = new Vector3(70, 80, 70);
            }
        }

        public static Sprite createSprite(SceneImage sceneImage) {
            Sprite sprite = Sprite.CreateSprite(PageTextures[sceneImage.TextureName], new Rect(sceneImage.TextureX, sceneImage.TextureY, sceneImage.Width, sceneImage.Height), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect, Vector4.zero, false);
            return sprite;
        }

        public static void SaveTextureInfo(string portalName, string textureName, int x, int y, int w = 300, int h = 180) {
            createSprite(textureName, x, y, w, h);
            SceneImages[portalName] = new SceneImage(portalName, textureName, x, y, w, h);
        }

        public static void GetPortalNames() {
            List<TunicPortal> portals = new List<TunicPortal>();

            foreach (Dictionary<string, List<TunicPortal>> dict in ERData.RegionPortalsList.Values) {
                foreach (List<TunicPortal> l in dict.Values) {
                    foreach (TunicPortal p in l) {
                        PortalNames.Add(p.Name);
                    }
                }
            }
        }

        public static void ShowPortalImage(string PortalName) {
            SceneImage sceneImage = SceneImages[PortalName];
            createSprite(sceneImage.TextureName, sceneImage.TextureX, sceneImage.TextureY, sceneImage.Width, sceneImage.Height);
            EntranceSelector.SceneText1.GetComponent<RTLTextMeshPro>().text = sceneImage.PortalName;
        }
        public static void ShowPortalImage(SceneImage sceneImage) {
            createSprite(sceneImage.TextureName, sceneImage.TextureX, sceneImage.TextureY, sceneImage.Width, sceneImage.Height);
            EntranceSelector.SceneText1.GetComponent<RTLTextMeshPro>().text = sceneImage.PortalName;
        }
        public static void LogDictionary() {
            foreach (KeyValuePair<string, SceneImage> pair in SceneImages) {
                TunicLogger.LogInfo("\t\t\t{");
                TunicLogger.LogInfo($"\t\t\t\t\"{pair.Key}\",");
                TunicLogger.LogInfo($"\t\t\t\tnew SceneImage(\"{pair.Value.PortalName}\", \"{pair.Value.TextureName}\", {pair.Value.TextureX}, {pair.Value.TextureY})");
                TunicLogger.LogInfo("\t\t\t},");
            }
        }

        public static void LoadCustomMaps() {
            Material m = ModelSwaps.FindMaterial("Default UI Material");
            GameObject ForestZigMap = ModelSwaps.CreateSprite(ImageData.EastForestZigMap, m, Width: 1200, Height: 720, SpriteName: "leaf_28");
            ForestZigMap.name = "leaf_28";
            ForestZigMap.AddComponent<SpriteRenderer>().sprite = ForestZigMap.GetComponent<Image>().sprite;
            GameObject.Destroy(ForestZigMap.GetComponent<Image>());
            Pages.Add("leaf_28", ForestZigMap);
            PageTextures.Add("leaf_28", ForestZigMap.GetComponent<SpriteRenderer>().sprite.texture);
        }
    }
}
