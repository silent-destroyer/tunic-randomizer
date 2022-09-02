using System.Reflection;
using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;

namespace TunicRandomizer {
    [BepInPlugin("glacia-silent.tunicrandomizer", "Tunic Randomizer", "0.0.1")]
    public class TunicRandomizer : BasePlugin {
        public static ManualLogSource Logger;
        public static System.Random Randomizer = null;

        public override void Load() {
            Log.LogInfo("Tunic Randomizer is loaded!");
            Logger = Log;

            Harmony harmony = new Harmony("glacia-silent.tunicrandomizer");

            // Item Patches
            harmony.Patch(AccessTools.Method(typeof(Chest), "IInteractionReceiver_Interact"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "IInteractionReceiver_Interact_ChestPatch")));

            harmony.Patch(AccessTools.PropertyGetter(typeof(Chest), "moneySprayQuantityFromDatabase"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "moneySprayQuantityFromDatabase_ChestPatch")));

            harmony.Patch(AccessTools.PropertyGetter(typeof(Chest), "itemContentsfromDatabase"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "itemContentsfromDatabase_ChestPatch")));

            harmony.Patch(AccessTools.PropertyGetter(typeof(Chest), "itemQuantityFromDatabase"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "itemQuantityFromDatabase_ChestPatch")));

            harmony.Patch(AccessTools.Method(typeof(PagePickup), "onGetIt"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "onGetIt_PagePickupPatch")));

            harmony.Patch(AccessTools.Method(typeof(ItemPickup), "onGetIt"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "onGetIt_ItemPickupPatch")));

            harmony.Patch(AccessTools.Method(typeof(TrinketWell), "TossedInCoin"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "TossedInCoin_Patch")));

            harmony.Patch(AccessTools.Method(typeof(TrinketWell), "TossedInCoin"), null, new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "TossedInCoin_PatchPostfix")));

            // Scene Load Patch
            harmony.Patch(AccessTools.Method(typeof(SceneLoader), "OnSceneLoaded"), null, new HarmonyMethod(AccessTools.Method(typeof(ScenePatches), "OnSceneLoaded_SceneLoader_ScenePatches")));
            harmony.Patch(AccessTools.Method(typeof(FairyCollection), "getFairyCount"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "getFairyCount_Patch")));

            // Player Character Patches
            harmony.Patch(AccessTools.Method(typeof(PlayerCharacter), "Update"), null, new HarmonyMethod(AccessTools.Method(typeof(PlayerPatches), "Update_PlayerPatches")));

            harmony.Patch(AccessTools.Method(typeof(PlayerCharacter), "Start"), null, new HarmonyMethod(AccessTools.Method(typeof(PlayerPatches), "Start_PlayerPatches")));

            // Page Patches
            harmony.Patch(AccessTools.Method(typeof(PageDisplay), "ShowPage"), new HarmonyMethod(AccessTools.Method(typeof(PagePatches), "Show_PagePatches")));

            harmony.Patch(AccessTools.Method(typeof(PageDisplay), "Show"), new HarmonyMethod(AccessTools.Method(typeof(PagePatches), "Show_PagePatches")));

            harmony.Patch(AccessTools.Method(typeof(PageDisplay), "close"), new HarmonyMethod(AccessTools.Method(typeof(PagePatches), "Close_PagePatches")));




        }
    }
}
