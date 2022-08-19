using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunicRandomizer {
    public class Fairy {

        public string Flag {
            get;
            set;
        }
        public string Translation {
            get;
            set;
        }

        public Fairy(string flag, string translation) {
            Flag = flag;
            Translation = translation;
        }

    }
}
