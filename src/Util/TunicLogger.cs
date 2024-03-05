using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunicRandomizer {
    public class TunicLogger {

        private static ManualLogSource Logger;

        public static void LogInfo(string message) {
            Logger.LogInfo(message);
        }

        public static void LogWarning(string message) { 
            Logger.LogWarning(message); 
        }

        public static void LogError(string message) { 
            Logger.LogError(message); 
        }

        public static void LogDebug(string message) {
            Logger.LogDebug(message);
        }

        public static void SetLogger(ManualLogSource logger) {
            Logger = logger;
        }
    }
}
