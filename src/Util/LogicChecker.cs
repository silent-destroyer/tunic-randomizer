using System.Collections.Generic;

namespace TunicRandomizer {
    public class LogicChecker {
        public static string MarkString(string text) {
            return "(x) " + text;
        }


        // Dictionary of region names to the rules required to reach them
        // format for the Region one is the first string key is the place you're trying to get to,
        // the second string keys are the origins, and the lists are the rules to get from those origins to the desired destination region
        public static Dictionary<string, Dictionary<string, List<List<string>>>> SetupRegionLogicSummaryWithStatus() {
            Dictionary<string, Dictionary<string, List<List<string>>>> regionLogicSummaryWithStatus = new Dictionary<string, Dictionary<string, List<List<string>>>>();
            foreach (string regionName in ERData.RegionDict.Keys) {
                string regionWithMarker = regionName;
                if (!TunicUtils.PlayerItemsAndRegions.ContainsKey(regionName)) {
                    regionWithMarker = MarkString(regionName);
                }
                regionLogicSummaryWithStatus.Add(regionWithMarker, new Dictionary<string, List<List<string>>>());
            }

            // go through and build the region logic summary, marking whatever regions and items we don't have yet
            foreach (KeyValuePair<string, Dictionary<string, List<List<string>>>> kvp in ERData.ModifiedTraversalReqs) {
                string originRegion = kvp.Key;
                if (!TunicUtils.PlayerItemsAndRegions.ContainsKey(originRegion)) {
                    originRegion = MarkString(originRegion);
                }
                foreach (KeyValuePair<string, List<List<string>>> kvp2 in kvp.Value) {
                    string destinationRegion = kvp2.Key;
                    if (!TunicUtils.PlayerItemsAndRegions.ContainsKey(destinationRegion)) {
                        destinationRegion = MarkString(destinationRegion);
                    }
                    List<List<string>> rules = kvp2.Value;
                    List<List<string>> markedRules = new List<List<string>>();
                    foreach (List<string> ruleSet in rules) {
                        List<string> markedList = new List<string>();
                        foreach (string rule in ruleSet) {
                            string markedRule = rule;
                            if (!TunicUtils.HasReq(rule, TunicUtils.PlayerItemsAndRegions)) {
                                markedRule = MarkString(markedRule);
                            }
                            markedList.Add(markedRule);
                        }
                        markedRules.Add(markedList);
                    }
                    regionLogicSummaryWithStatus[destinationRegion].Add(originRegion, markedRules);
                }
            }
            return regionLogicSummaryWithStatus;
        }


        public static List<string> StringifyLogicSummary(Dictionary<string, Dictionary<string, List<List<string>>>> logicSummary) {
            List<string> output = new List<string> {
                "This is for dev use, and will not look very nice for users.",
                "",
            };
            foreach (KeyValuePair<string, Dictionary<string, List<List<string>>>> kvp in logicSummary) {
                string goalRegion = kvp.Key;
                output.Add(goalRegion);
                output.Add("");
                foreach (KeyValuePair<string, List<List<string>>> kvp2 in kvp.Value) {
                    string originRegion = kvp2.Key;
                    output.Add("- " + originRegion);
                    List<List<string>> rules = kvp2.Value;
                    foreach (List<string> ruleSet in rules) {
                        string ruleSetString = "    - ";
                        bool first = true;
                        foreach (string rule in ruleSet) {
                            if (first) {
                                first = false;
                            } else {
                                ruleSetString += ", ";
                            }
                            ruleSetString += rule;
                        }
                        output.Add(ruleSetString);
                    }
                }
                output.Add("---------------------------------------------------");
            }
            return output;
        }


        public static void WriteLogicSummaryFile() {
            SetupRegionLogicSummaryWithStatus();
            TunicUtils.TryWriteFile(TunicRandomizer.RegionLogicPath, StringifyLogicSummary(SetupRegionLogicSummaryWithStatus()));
        }

    }
}
