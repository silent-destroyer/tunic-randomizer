using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunicRandomizer {
    public class TunicUtils {
        // add a key if it doesn't exist, otherwise increment the value by 1
        public static Dictionary<string, int> AddListToDict(Dictionary<string, int> dictionary, List<string> list) {
            foreach (string item in list) {
                dictionary.TryGetValue(item, out var count);
                dictionary[item] = count + 1;
            }
            return dictionary;
        }

        public static Dictionary<string, int> AddStringToDict(Dictionary<string, int> dictionary, string item) {
            dictionary.TryGetValue(item, out var count);
            dictionary[item] = count + 1;
            return dictionary;
        }

        public static Dictionary<string, int> AddDictToDict(Dictionary<string, int> dictionary1, Dictionary<string, int> dictionary2) {
            foreach (KeyValuePair<string, int> pair in dictionary2) {
                dictionary1.TryGetValue(pair.Key, out var count);
                dictionary1[pair.Key] = count + pair.Value;
            }
            return dictionary1;
        }
    }
}
