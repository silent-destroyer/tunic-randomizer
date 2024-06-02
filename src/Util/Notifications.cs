using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace TunicRandomizer {
    public class Notifications {

        public static void Show(string topLine, string bottomLine) {
            var topLineObject = ScriptableObject.CreateInstance<LanguageLine>();
            topLineObject.text = topLine.Replace("{", "").Replace("}", "");

            var bottomLineObject = ScriptableObject.CreateInstance<LanguageLine>();
            bottomLineObject.text = bottomLine.Replace("{", "").Replace("}", "");

            var areaData = ScriptableObject.CreateInstance<AreaData>();
            areaData.topLine = topLineObject;
            areaData.bottomLine = bottomLineObject;

            AreaLabel.ShowLabel(areaData);
        }

    }

}
