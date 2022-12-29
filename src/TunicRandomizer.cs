using System.Reflection;
using System.IO;
using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using Il2CppSystem;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using TinyJson;

namespace TunicRandomizer {

    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    public class TunicRandomizer : BasePlugin {
        
        public static ManualLogSource Logger;
        public static System.Random Randomizer = null;
        public static RandomizerSettings Settings;
        public static string SettingsPath = Application.persistentDataPath + "/RandomizerSettings.json";
        public static string ItemTrackerPath = Application.persistentDataPath + "/RandomizerTracker.json";
        public static ItemTracker Tracker = new ItemTracker();

        public override void Load() {
            Log.LogInfo("Tunic Randomizer v" + PluginInfo.VERSION + " is loaded!");
            Logger = Log;
            Harmony harmony = new Harmony(PluginInfo.GUID);

            ClassInjector.RegisterTypeInIl2Cpp<TitleVersion>();
            GameObject TitleVersion = new GameObject("TitleVersion", new Type[] { Il2CppType.Of<TitleVersion>() });
            TitleVersion.hideFlags = HideFlags.HideAndDontSave;
            UnityEngine.Object.DontDestroyOnLoad(TitleVersion);


            if (!File.Exists(SettingsPath)) {
                Settings = new RandomizerSettings();
                File.WriteAllText(SettingsPath, JSONWriter.ToJson(Settings));
            } else {
                Settings = JSONParser.FromJson<RandomizerSettings>(File.ReadAllText(SettingsPath));
                if (Settings.SavedColorPalette == null) {
                    Settings.SavedColorPalette = new int[] { 0, 0, 0, 0, 0 };
                }
                Log.LogInfo("Loaded settings from file: " + JSONWriter.ToJson(Settings));
            }

            // Random Item Patches
            harmony.Patch(AccessTools.Method(typeof(Chest), "IInteractionReceiver_Interact"), new HarmonyMethod(AccessTools.Method(typeof(RandomItemPatches), "Chest_IInteractionReceiver_Interact_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(Chest), "InterruptOpening"), new HarmonyMethod(AccessTools.Method(typeof(RandomItemPatches), "Chest_InterruptOpening_PrefixPatch")));

            harmony.Patch(AccessTools.PropertyGetter(typeof(Chest), "moneySprayQuantityFromDatabase"), new HarmonyMethod(AccessTools.Method(typeof(RandomItemPatches), "Chest_moneySprayQuantityFromDatabase_GetterPatch")));

            harmony.Patch(AccessTools.PropertyGetter(typeof(Chest), "itemContentsfromDatabase"), new HarmonyMethod(AccessTools.Method(typeof(RandomItemPatches), "Chest_itemContentsfromDatabase_GetterPatch")));

            harmony.Patch(AccessTools.PropertyGetter(typeof(Chest), "itemQuantityFromDatabase"), new HarmonyMethod(AccessTools.Method(typeof(RandomItemPatches), "Chest_itemQuantityFromDatabase_GetterPatch")));

            harmony.Patch(AccessTools.PropertyGetter(typeof(Chest), "shouldShowAsOpen"), new HarmonyMethod(AccessTools.Method(typeof(RandomItemPatches), "Chest_shouldShowAsOpen_GetterPatch")));

            harmony.Patch(AccessTools.Method(typeof(PagePickup), "onGetIt"), new HarmonyMethod(AccessTools.Method(typeof(RandomItemPatches), "PagePickup_onGetIt_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(ItemPickup), "onGetIt"), new HarmonyMethod(AccessTools.Method(typeof(RandomItemPatches), "ItemPickup_onGetIt_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(HeroRelicPickup), "onGetIt"), new HarmonyMethod(AccessTools.Method(typeof(RandomItemPatches), "HeroRelicPickup_onGetIt_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(TrinketWell), "TossedInCoin"), new HarmonyMethod(AccessTools.Method(typeof(RandomItemPatches), "TrinketWell_TossedInCoin_PrefixPatch")), new HarmonyMethod(AccessTools.Method(typeof(RandomItemPatches), "TrinketWell_TossedInCoin_PostfixPatch")));

            harmony.Patch(AccessTools.Method(typeof(TrinketWell), "IInteractionReceiver_Interact"), new HarmonyMethod(AccessTools.Method(typeof(RandomItemPatches), "TrinketWell_IInteractionReceiver_Interact_PrefixPatch")), new HarmonyMethod(AccessTools.Method(typeof(RandomItemPatches), "TrinketWell_IInteractionReceiver_Interact_PostfixPatch")));

            harmony.Patch(AccessTools.Method(typeof(ShopItem), "buy"), new HarmonyMethod(AccessTools.Method(typeof(RandomItemPatches), "ShopItem_buy_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(ShopItem), "IInteractionReceiver_Interact"), new HarmonyMethod(AccessTools.Method(typeof(RandomItemPatches), "ShopItem_IInteractionReceiver_Interact_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(PotionCombine), "Show"), null, new HarmonyMethod(AccessTools.Method(typeof(RandomItemPatches), "PotionCombine_Show_PostFixPatch")));

            // Scene Loader Patches
            harmony.Patch(AccessTools.Method(typeof(SceneLoader), "OnSceneLoaded"), new HarmonyMethod(AccessTools.Method(typeof(SceneLoaderPatches), "SceneLoader_OnSceneLoaded_PrefixPatch")), new HarmonyMethod(AccessTools.Method(typeof(SceneLoaderPatches), "SceneLoader_OnSceneLoaded_PostfixPatch")));

            // Player Character Patches
            harmony.Patch(AccessTools.Method(typeof(PlayerCharacter), "Update"), null, new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "PlayerCharacter_Update_PostfixPatch")));

            harmony.Patch(AccessTools.Method(typeof(PlayerCharacter), "Start"), null, new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "PlayerCharacter_Start_PostfixPatch")));

            harmony.Patch(AccessTools.Method(typeof(Foxgod), "OnFlinchlessHit"), new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "Foxgod_OnFlinchlessHit_PrefixPatch")));

            // Page Display Patches
            harmony.Patch(AccessTools.Method(typeof(PageDisplay), "ShowPage"), new HarmonyMethod(AccessTools.Method(typeof(PageDisplayPatches), "PageDisplay_Show_PostfixPatch")));

            harmony.Patch(AccessTools.Method(typeof(PageDisplay), "Show"), new HarmonyMethod(AccessTools.Method(typeof(PageDisplayPatches), "PageDisplay_Show_PostfixPatch")));

            harmony.Patch(AccessTools.Method(typeof(PageDisplay), "close"), new HarmonyMethod(AccessTools.Method(typeof(PageDisplayPatches), "PageDisplay_Close_PostfixPatch")));

            // Miscellaneous Patches            
            harmony.Patch(AccessTools.Method(typeof(OptionsGUI), "page_root"), null, new HarmonyMethod(AccessTools.Method(typeof(OptionsGUIPatches), "OptionsGUI_page_root_PostfixPatch")));

            harmony.Patch(AccessTools.Method(typeof(OptionsGUI), "page_extras"), new HarmonyMethod(AccessTools.Method(typeof(OptionsGUIPatches), "OptionsGUI_page_extras_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(OptionsGUI), "page_extras"), null, new HarmonyMethod(AccessTools.Method(typeof(OptionsGUIPatches), "OptionsGUI_page_extras_PostfixPatch")));

            harmony.Patch(AccessTools.Method(typeof(InteractionTrigger), "Interact"), new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "InteractionTrigger_Interact_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(FairyCollection), "getFairyCount"), new HarmonyMethod(AccessTools.Method(typeof(RandomItemPatches), "FairyCollection_getFairyCount_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(InventoryDisplay), "Update"), new HarmonyMethod(AccessTools.Method(typeof(RandomItemPatches), "InventoryDisplay_Update_PrefixPatch")));
            
            harmony.Patch(AccessTools.Method(typeof(PauseMenu), "__button_ReturnToTitle"), null, new HarmonyMethod(AccessTools.Method(typeof(SceneLoaderPatches), "PauseMenu___button_ReturnToTitle_PostfixPatch")));

            harmony.Patch(AccessTools.Method(typeof(FileManagementGUI), "rePopulateList"), null, new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "FileManagementGUI_rePopulateList_PostfixPatch")));

            harmony.Patch(AccessTools.Method(typeof(SaveFile), "GetNewSaveFileName"), null, new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "SaveFile_GetNewSaveFileName_PostfixPatch")));
            
            harmony.Patch(AccessTools.Method(typeof(SpeedrunFinishlineDisplay), "showFinishline"), new HarmonyMethod(AccessTools.Method(typeof(SpeedrunFinishlineDisplayPatches), "SpeedrunFinishlineDisplay_showFinishline_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(SpeedrunFinishlineDisplay), "addParadeIcon"), new HarmonyMethod(AccessTools.Method(typeof(SpeedrunFinishlineDisplayPatches), "SpeedrunFinishlineDisplay_addParadeIcon_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(GameOverDecision), "__retry"), new HarmonyMethod(AccessTools.Method(typeof(SpeedrunFinishlineDisplayPatches), "GameOverDecision___retry_PrefixPatch")));
            
            harmony.Patch(AccessTools.Method(typeof(BoneItemBehaviour), "onActionButtonDown"), new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "BoneItemBehavior_onActionButtonDown_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(BoneItemBehaviour), "confirmBoneUseCallback"), new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "BoneItemBehavior_confirmBoneUseCallback_PrefixPatch")));
            
        }
    }
}
