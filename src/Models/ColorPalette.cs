using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunicRandomizer {
    public class ColorPalette {

        public static Dictionary<int, ColorPalette> Fur = new Dictionary<int, ColorPalette>() {
            {0, new ColorPalette("<#EE9464>", "Orange")},
            {1, new ColorPalette("<#C6CACC>", "Light Gray")},
            {2, new ColorPalette("<#7D7D7D>", "Gray")},
            {3, new ColorPalette("<#363636>", "Dark Gray")},
            {4, new ColorPalette("<#BA472C>", "Red")},
            {5, new ColorPalette("<#C966D2>", "Purple")},
            {6, new ColorPalette("<#62A8D9>", "Light Blue")},
            {7, new ColorPalette("<#80CB7A>", "Lime Green")},
            {8, new ColorPalette("<#E3D457>", "Yellow")},
            {9, new ColorPalette("<#F198DF>", "Pink")},
            {10, new ColorPalette("<#2562C7>", "Blue")},
            {11, new ColorPalette("<#59044F>", "Violet")},
            {12, new ColorPalette("<#820015>", "Maroon")},
            {13, new ColorPalette("<#021E7F>", "Dark Blue")},
            {14, new ColorPalette("<#AF9900>", "Gold")},
            {15, new ColorPalette("<#027F5E>", "Green")}
        };

        public static Dictionary<int, ColorPalette> Puff = new Dictionary<int, ColorPalette>() {
            {0,  new ColorPalette("<#EFEFEF>", "Match Fur")},
            {1, new ColorPalette("<#FC71F1>", "Pink")},
            {2, new ColorPalette("<#FF1843>", "Red")},
            {3, new ColorPalette("<#A670FF>", "Purple")},
            {4, new ColorPalette("<#86A5FF>", "Sky Blue")},
            {5, new ColorPalette("<#4FF5D4>", "Turquoise")},
            {6, new ColorPalette("<#7DEB74>", "Green")},
            {7, new ColorPalette("<#ECF323>", "Yellow")},
            {8, new ColorPalette("<#EE9464>", "Orange")},
            {9, new ColorPalette("<#515151>", "Black")},
            {10, new ColorPalette("<#777777>", "Gray")},
            {11, new ColorPalette("<#EFEFEF>", "White")}
        };

        public static Dictionary<int, ColorPalette> Details = new Dictionary<int, ColorPalette>() {
            {0, new ColorPalette("<#6F4C44>", "Brown")},
            {1, new ColorPalette("<#FC71F1>", "Pink")},
            {2, new ColorPalette("<#FF1843>", "Red")},
            {3, new ColorPalette("<#A670FF>", "Purple")},
            {4, new ColorPalette("<#86A5FF>", "Sky Blue")},
            {5, new ColorPalette("<#4FF5D4>", "Turquoise")},
            {6, new ColorPalette("<#7DEB74>", "Green")},
            {7, new ColorPalette("<#ECF323>", "Yellow")},
            {8, new ColorPalette("<#EE9464>", "Orange")},
            {9, new ColorPalette("<#515151>", "Black")},
            {10, new ColorPalette("<#777777>", "Gray")},
            {11, new ColorPalette("<#EFEFEF>", "White")}
        };

        public static Dictionary<int, ColorPalette> Tunic = new Dictionary<int, ColorPalette>() {
            {0, new ColorPalette("<#8ECC73>", "Green")},
            {1, new ColorPalette("<#445B23>", "Dark Green")},
            {2, new ColorPalette("<#F3F153>", "Yellow")},
            {3, new ColorPalette("<#DB8F34>", "Orange")},
            {4, new ColorPalette("<#ED2A4F>", "Red")},
            {5, new ColorPalette("<#E52893>", "Pink")},
            {6, new ColorPalette("<#B628E5>", "Purple")},
            {7, new ColorPalette("<#6F28E5>", "Violet")},
            {8, new ColorPalette("<#2859E5>", "Blue")},
            {9, new ColorPalette("<#28A0E5>", "Light Blue")},
            {10, new ColorPalette("<#28E5BF>", "Turquoise")},
            {11, new ColorPalette("<#EFEFEF>", "White")},
            {12, new ColorPalette("<#B6B6B6>", "Light Gray")},
            {13, new ColorPalette("<#777777>", "Gray")},
            {14, new ColorPalette("<#484848>", "Dark Gray")},
            {15, new ColorPalette("<#515151>", "Black")}
        };

        public static Dictionary<int, ColorPalette> Scarf = new Dictionary<int, ColorPalette>() {
            {0, new ColorPalette("<#FC71F1>", "Pink")},
            {1, new ColorPalette("<#FF1843>", "Red")},
            {2, new ColorPalette("<#A670FF>", "Purple")},
            {3, new ColorPalette("<#86A5FF>", "Sky Blue")},
            {4, new ColorPalette("<#4FF5D4>", "Turquoise")},
            {5, new ColorPalette("<#7DEB74>", "Green")},
            {6, new ColorPalette("<#ECF323>", "Yellow")},
            {7, new ColorPalette("<#EE9464>", "Orange")},
            {8, new ColorPalette("<#515151>", "Black")},
            {9, new ColorPalette("<#777777>", "Gray")},
            {10, new ColorPalette("<#EFEFEF>", "White")}
        };

        public string HexValue {
            get;
            set;
        }

        public string ColorName {
            get;
            set;
        }

        public ColorPalette() { }
        
        public ColorPalette(string hexValue, string colorName) {
            HexValue = hexValue;
            ColorName = colorName;
        }

        public static string getDefaultPuffColor() {

            return Fur[PlayerPalette.selectionIndices[0]].HexValue + Puff[0].ColorName;
        }

        
        public override string ToString() {
            return HexValue + ColorName;
        }
    }
}
