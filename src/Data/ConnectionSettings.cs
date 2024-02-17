using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace TunicRandomizer {
    public class ConnectionSettings {
        public string Player {
            get;
            set;
        }

        public string Hostname {
            get;
            set;
        }

        public string Port {
            get;
            set;
        }

        public string Password {
            get;
            set;
        }

        public ConnectionSettings() {
            Player = "Ruin Seeker";
            Hostname = "localhost";
            Port = "38281";
            Password = "";
        }
    }
}
