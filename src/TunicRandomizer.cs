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
        public static string SpoilerLogPath = Application.persistentDataPath + "/Randomizer/Spoiler.log";
        public static string SettingsPath = Application.persistentDataPath + "/Randomizer/Settings.json";
        public static string ItemTrackerPath = Application.persistentDataPath + "/Randomizer/ItemTracker.json";
        public static ItemTracker Tracker = new ItemTracker();

        public override void Load() {
            Log.LogInfo("Tunic Randomizer v" + PluginInfo.VERSION + " is loaded!");
            Logger = Log;
            Harmony harmony = new Harmony(PluginInfo.GUID);
            ClassInjector.RegisterTypeInIl2Cpp<PaletteEditor>();
            UnityEngine.Object.DontDestroyOnLoad(new GameObject("palette editor gui", new Type[]
            {
                Il2CppType.Of<PaletteEditor>()
            }) {
                hideFlags = HideFlags.HideAndDontSave
            }); 
            ClassInjector.RegisterTypeInIl2Cpp<QuickSettings>();
            UnityEngine.Object.DontDestroyOnLoad(new GameObject("quick settings gui", new Type[]
            {
                Il2CppType.Of<QuickSettings>()
            }) {
                hideFlags = HideFlags.HideAndDontSave
            });
            if (!Directory.Exists(Application.persistentDataPath + "/Randomizer/")) {
                Directory.CreateDirectory(Application.persistentDataPath + "/Randomizer/");
            }
            if (File.Exists(Application.persistentDataPath + "/RandomizerTracker.json")) { 
                File.Delete(Application.persistentDataPath + "/RandomizerTracker.json");
            }
            if (File.Exists(Application.persistentDataPath + "/RandomizerSpoiler.log")) {
                File.Delete(Application.persistentDataPath + "/RandomizerSpoiler.log");
            }
            if (File.Exists(Application.persistentDataPath + "/RandomizerSettings.json")) {
                File.Delete(Application.persistentDataPath + "/RandomizerSettings.json");
            }

            Application.runInBackground = true;
            if (!File.Exists(SettingsPath)) {
                Settings = new RandomizerSettings();
                File.WriteAllText(SettingsPath, JSONWriter.ToJson(Settings));
            } else {
                Settings = JSONParser.FromJson<RandomizerSettings>(File.ReadAllText(SettingsPath));
                Log.LogInfo("Loaded settings from file: " + JSONWriter.ToJson(Settings));
            }

            // Item Randomizer Patches
            harmony.Patch(AccessTools.Method(typeof(Chest), "IInteractionReceiver_Interact"), new HarmonyMethod(AccessTools.Method(typeof(ItemRandomizer), "Chest_IInteractionReceiver_Interact_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(Chest), "InterruptOpening"), new HarmonyMethod(AccessTools.Method(typeof(ItemRandomizer), "Chest_InterruptOpening_PrefixPatch")));

            harmony.Patch(AccessTools.PropertyGetter(typeof(Chest), "moneySprayQuantityFromDatabase"), new HarmonyMethod(AccessTools.Method(typeof(ItemRandomizer), "Chest_moneySprayQuantityFromDatabase_GetterPatch")));

            harmony.Patch(AccessTools.PropertyGetter(typeof(Chest), "itemContentsfromDatabase"), new HarmonyMethod(AccessTools.Method(typeof(ItemRandomizer), "Chest_itemContentsfromDatabase_GetterPatch")));

            harmony.Patch(AccessTools.PropertyGetter(typeof(Chest), "itemQuantityFromDatabase"), new HarmonyMethod(AccessTools.Method(typeof(ItemRandomizer), "Chest_itemQuantityFromDatabase_GetterPatch")));

            harmony.Patch(AccessTools.PropertyGetter(typeof(Chest), "shouldShowAsOpen"), new HarmonyMethod(AccessTools.Method(typeof(ItemRandomizer), "Chest_shouldShowAsOpen_GetterPatch")));

            harmony.Patch(AccessTools.Method(typeof(PagePickup), "onGetIt"), new HarmonyMethod(AccessTools.Method(typeof(ItemRandomizer), "PagePickup_onGetIt_PrefixPatch")));
            
            harmony.Patch(AccessTools.Method(typeof(ItemPickup), "onGetIt"), new HarmonyMethod(AccessTools.Method(typeof(ItemRandomizer), "ItemPickup_onGetIt_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(HeroRelicPickup), "onGetIt"), new HarmonyMethod(AccessTools.Method(typeof(ItemRandomizer), "HeroRelicPickup_onGetIt_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(TrinketWell), "TossedInCoin"), new HarmonyMethod(AccessTools.Method(typeof(ItemRandomizer), "TrinketWell_TossedInCoin_PostfixPatch")));
            
            harmony.Patch(AccessTools.Method(typeof(TrinketWell._giveTrinketUpgrade_d__14), "MoveNext"), new HarmonyMethod(AccessTools.Method(typeof(ItemRandomizer), "TrinketWell_giveTrinketUpgrade_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(ShopItem), "buy"), new HarmonyMethod(AccessTools.Method(typeof(ItemRandomizer), "ShopItem_buy_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(ShopItem), "IInteractionReceiver_Interact"), new HarmonyMethod(AccessTools.Method(typeof(ItemRandomizer), "ShopItem_IInteractionReceiver_Interact_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(PotionCombine), "Show"), null, new HarmonyMethod(AccessTools.Method(typeof(ItemRandomizer), "PotionCombine_Show_PostFixPatch")));

            // Scene Loader Patches
            harmony.Patch(AccessTools.Method(typeof(SceneLoader), "OnSceneLoaded"), new HarmonyMethod(AccessTools.Method(typeof(SceneLoaderPatches), "SceneLoader_OnSceneLoaded_PrefixPatch")), new HarmonyMethod(AccessTools.Method(typeof(SceneLoaderPatches), "SceneLoader_OnSceneLoaded_PostfixPatch")));

            // Player Character Patches
            harmony.Patch(AccessTools.Method(typeof(PlayerCharacter), "Update"), null, new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "PlayerCharacter_Update_PostfixPatch")));

            harmony.Patch(AccessTools.Method(typeof(PlayerCharacter), "Start"), null, new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "PlayerCharacter_Start_PostfixPatch")));
            
            // Page Display Patches
            harmony.Patch(AccessTools.Method(typeof(PageDisplay), "ShowPage"), new HarmonyMethod(AccessTools.Method(typeof(PageDisplayPatches), "PageDisplay_Show_PostfixPatch")));

            harmony.Patch(AccessTools.Method(typeof(PageDisplay), "Show"), new HarmonyMethod(AccessTools.Method(typeof(PageDisplayPatches), "PageDisplay_Show_PostfixPatch")));

            harmony.Patch(AccessTools.Method(typeof(PageDisplay), "close"), new HarmonyMethod(AccessTools.Method(typeof(PageDisplayPatches), "PageDisplay_Close_PostfixPatch")));

            // Miscellaneous Patches            
            harmony.Patch(AccessTools.Method(typeof(OptionsGUI), "page_root"), new HarmonyMethod(AccessTools.Method(typeof(OptionsGUIPatches), "OptionsGUI_page_root_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(InteractionTrigger), "Interact"), new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "InteractionTrigger_Interact_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(FairyCollection), "getFairyCount"), new HarmonyMethod(AccessTools.Method(typeof(ItemRandomizer), "FairyCollection_getFairyCount_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(InventoryDisplay), "Update"), new HarmonyMethod(AccessTools.Method(typeof(ItemStatsHUD), "InventoryDisplay_Update_PrefixPatch")));
            
            harmony.Patch(AccessTools.Method(typeof(PauseMenu), "__button_ReturnToTitle"), null, new HarmonyMethod(AccessTools.Method(typeof(SceneLoaderPatches), "PauseMenu___button_ReturnToTitle_PostfixPatch")));

            harmony.Patch(AccessTools.Method(typeof(FileManagementGUI), "rePopulateList"), null, new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "FileManagementGUI_rePopulateList_PostfixPatch")));

            harmony.Patch(AccessTools.Method(typeof(SaveFile), "GetNewSaveFileName"), null, new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "SaveFile_GetNewSaveFileName_PostfixPatch")));
            
            harmony.Patch(AccessTools.Method(typeof(SpeedrunFinishlineDisplay), "showFinishline"), new HarmonyMethod(AccessTools.Method(typeof(SpeedrunFinishlineDisplayPatches), "SpeedrunFinishlineDisplay_showFinishline_PrefixPatch")), new HarmonyMethod(AccessTools.Method(typeof(SpeedrunFinishlineDisplayPatches), "SpeedrunFinishlineDisplay_showFinishline_PostfixPatch")));

            harmony.Patch(AccessTools.Method(typeof(SpeedrunFinishlineDisplay), "addParadeIcon"), new HarmonyMethod(AccessTools.Method(typeof(SpeedrunFinishlineDisplayPatches), "SpeedrunFinishlineDisplay_addParadeIcon_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(GameOverDecision), "__retry"), new HarmonyMethod(AccessTools.Method(typeof(SpeedrunFinishlineDisplayPatches), "GameOverDecision___retry_PrefixPatch")));
            
            harmony.Patch(AccessTools.Method(typeof(BoneItemBehaviour), "onActionButtonDown"), new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "BoneItemBehavior_onActionButtonDown_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(BoneItemBehaviour), "confirmBoneUseCallback"), new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "BoneItemBehavior_confirmBoneUseCallback_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(SpearItemBehaviour), "onActionButtonDown"), new HarmonyMethod(AccessTools.Method(typeof(GoldenItemBehavior), "SpearItemBehaviour_onActionButtonDown_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(Monster), "IDamageable_ReceiveDamage"), new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "Monster_IDamageable_ReceiveDamage_PrefixPatch")), new HarmonyMethod(AccessTools.Method(typeof(EnemyRandomizer), "Monster_IDamageable_ReceiveDamage_PostfixPatch")));

            harmony.Patch(AccessTools.Method(typeof(BloodstainChest), "IInteractionReceiver_Interact"), new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "BloodstainChest_IInteractionReceiver_Interact_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(UpgradeAltar), "DoOfferingSequence"), null, new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "UpgradeAltar_DoOfferingSequence_PostfixPatch")));

            harmony.Patch(AccessTools.Method(typeof(Campfire), "Interact"), new HarmonyMethod(AccessTools.Method(typeof(EnemyRandomizer), "Campfire_Interact_PrefixPatch")));

            harmony.Patch(AccessTools.Method(typeof(GameOverDecision), "Start"), null, new HarmonyMethod(AccessTools.Method(typeof(SpeedrunFinishlineDisplayPatches), "GameOverDecision_Start_PostfixPatch")));

            harmony.Patch(AccessTools.Method(typeof(Campfire), "RespawnAtLastCampfire"), new HarmonyMethod(AccessTools.Method(typeof(EnemyRandomizer), "Campfire_RespawnAtLastCampfire_PrefixPatch")));
            
            harmony.Patch(AccessTools.Method(typeof(TunicKnightVoid), "onFlinch"), new HarmonyMethod(AccessTools.Method(typeof(EnemyRandomizer), "TunicKnightVoid_onFlinch_PrefixPatch")));
            
            //harmony.Patch(AccessTools.Method(typeof(CrossbowItemBehaviour), "__fireBow"), null, new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "CrossbowItemBehavior___fireBow_PostfixPatch")));

        }

    }
}
