﻿using BepInEx.Logging;

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

        // set this to true to trigger logging for all log testing messages
        public static bool Testing = false;
        public static void LogTesting(string message) {
            if (Testing) {
                Logger.LogInfo(message);
            }
        }
    }
}
