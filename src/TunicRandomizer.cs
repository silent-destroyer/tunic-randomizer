using System.Reflection;
using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;

namespace TunicRandomizer {
    [BepInPlugin("glacia-silent.tunicrandomizer", "Tunic Randomizer", "0.0.0.1")]
    public class TunicRandomizer : BasePlugin {
        public static ManualLogSource Logger;
        public static System.Random Randomizer = null;

        public override void Load() {
            Log.LogInfo("Tunic Randomizer is loaded!");
            Logger = Log;

            Harmony harmony = new Harmony("glacia-silent.tunicrandomizer");

            MethodInfo originalOpenChest = AccessTools.Method(typeof(Chest), "IInteractionReceiver_Interact");
            MethodInfo patchedOpenChest = AccessTools.Method(typeof(ItemPatches), "IInteractionReceiver_Interact_ChestPatch");
            harmony.Patch(originalOpenChest, new HarmonyMethod(patchedOpenChest));

            MethodInfo originalTrinketWellToss = AccessTools.Method(typeof(TrinketWell), "TossedInCoin");
            MethodInfo patchedTrinketWellToss = AccessTools.Method(typeof(ItemPatches), "TossedInCoin_Patch");
            harmony.Patch(originalTrinketWellToss, new HarmonyMethod(patchedTrinketWellToss));

            MethodInfo originalTrinketWellTossPostfix = AccessTools.Method(typeof(TrinketWell), "TossedInCoin");
            MethodInfo patchedTrinketWellTossPostfix = AccessTools.Method(typeof(ItemPatches), "TossedInCoin_PatchPostfix");
            harmony.Patch(originalTrinketWellTossPostfix, null, new HarmonyMethod(patchedTrinketWellTossPostfix));

            MethodInfo debugLanguageLineOriginal = AccessTools.Method(typeof(NPCDialogue), "DisplayDialogue");
            MethodInfo debugLanguageLinePatched = AccessTools.Method(typeof(PlayerPatches), "Text_LanguagePatch");
            harmony.Patch(debugLanguageLineOriginal, new HarmonyMethod(debugLanguageLinePatched));

            MethodInfo originalChestMoney = AccessTools.PropertyGetter(typeof(Chest), "moneySprayQuantityFromDatabase");
            MethodInfo patchedChestMoney = AccessTools.Method(typeof(ItemPatches), "moneySprayQuantityFromDatabase_ChestPatch");
            harmony.Patch(originalChestMoney, new HarmonyMethod(patchedChestMoney));

            MethodInfo originalChestItem = AccessTools.PropertyGetter(typeof(Chest), "itemContentsfromDatabase");
            MethodInfo patchedChestItem = AccessTools.Method(typeof(ItemPatches), "itemContentsfromDatabase_ChestPatch");
            harmony.Patch(originalChestItem, new HarmonyMethod(patchedChestItem));

            MethodInfo originalChestItemQuantity = AccessTools.PropertyGetter(typeof(Chest), "itemQuantityFromDatabase");
            MethodInfo patchedChestItemQuantity = AccessTools.Method(typeof(ItemPatches), "itemQuantityFromDatabase_ChestPatch");
            harmony.Patch(originalChestItemQuantity, new HarmonyMethod(patchedChestItemQuantity));

            MethodInfo sceneInfoUpdateOriginal = AccessTools.Method(typeof(SceneLoader), "OnSceneLoaded");
            MethodInfo sceneInfoUpdatePatch = AccessTools.Method(typeof(ScenePatches), "OnSceneLoaded_SceneLoader_ScenePatches");
            harmony.Patch(sceneInfoUpdateOriginal, null, new HarmonyMethod(sceneInfoUpdatePatch));

            MethodInfo playerUpdateOriginal = AccessTools.Method(typeof(PlayerCharacter), "Update");
            MethodInfo playerUpdatePatch = AccessTools.Method(typeof(PlayerPatches), "Update_PlayerPatches");
            harmony.Patch(playerUpdateOriginal, null, new HarmonyMethod(playerUpdatePatch));

            MethodInfo playerStartOriginal = AccessTools.Method(typeof(PlayerCharacter), "Start");
            MethodInfo playerStartPatch = AccessTools.Method(typeof(PlayerPatches), "Start_PlayerPatches");
            harmony.Patch(playerStartOriginal, null, new HarmonyMethod(playerStartPatch));

            MethodInfo showPageOriginal = AccessTools.Method(typeof(PageDisplay), "ShowPage");
            MethodInfo showPagePatched = AccessTools.Method(typeof(PagePatches), "Show_PagePatches");
            harmony.Patch(showPageOriginal, new HarmonyMethod(showPagePatched));

            MethodInfo showManualOriginal = AccessTools.Method(typeof(PageDisplay), "Show");
            harmony.Patch(showManualOriginal, new HarmonyMethod(showPagePatched));

            MethodInfo closeManualOriginal = AccessTools.Method(typeof(PageDisplay), "close");
            MethodInfo closeManualPatched = AccessTools.Method(typeof(PagePatches), "Close_PagePatches");
            harmony.Patch(closeManualOriginal, new HarmonyMethod(closeManualPatched));

            MethodInfo originalPickupItem = AccessTools.Method(typeof(ItemPickup), "onGetIt");
            MethodInfo patchedPickupItem = AccessTools.Method(typeof(ItemPatches), "onGetIt_ItemPickupPatch");
            harmony.Patch(originalPickupItem, new HarmonyMethod(patchedPickupItem));

            harmony.Patch(
                AccessTools.Method(typeof(PagePickup), "onGetIt"),
                new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "onGetIt_PagePickupPatch"))
            );

        }
    }
}
