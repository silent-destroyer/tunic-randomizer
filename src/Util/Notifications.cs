using UnityEngine;

namespace TunicRandomizer {
    public class Notifications {

        public static void Show(string topLine, string bottomLine) {
            TunicLogger.LogInfo("test 1");
            var topLineObject = ScriptableObject.CreateInstance<LanguageLine>();
            topLineObject.text = topLine.Replace("{", "").Replace("}", "");
            TunicLogger.LogInfo("test 2");
            var bottomLineObject = ScriptableObject.CreateInstance<LanguageLine>();
            bottomLineObject.text = bottomLine.Replace("{", "").Replace("}", "");
            TunicLogger.LogInfo("test 3");
            var areaData = ScriptableObject.CreateInstance<AreaData>();
            areaData.topLine = topLineObject;
            areaData.bottomLine = bottomLineObject;
            TunicLogger.LogInfo("test 4");
            AreaLabel.ShowLabel(areaData);
            TunicLogger.LogInfo("test 5");
        }

    }

}
