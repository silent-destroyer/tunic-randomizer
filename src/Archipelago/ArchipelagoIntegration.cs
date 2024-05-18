﻿using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net.Models;
using System.Collections.Concurrent;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Newtonsoft.Json;
using System.Globalization;
using Archipelago.MultiClient.Net.Packets;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using Archipelago.MultiClient.Net.Helpers;
using static TunicRandomizer.SaveFlags;

namespace TunicRandomizer {
    public class ArchipelagoIntegration {

        private ManualLogSource Logger = TunicRandomizer.Logger;
        
        public bool connected;

        public ArchipelagoSession session;
        private IEnumerator<bool> incomingItemHandler;
        private IEnumerator<bool> outgoingItemHandler;
        private IEnumerator<bool> checkItemsReceived;
        private ConcurrentQueue<(NetworkItem NetworkItem, int index)> incomingItems;
        private ConcurrentQueue<NetworkItem> outgoingItems;
        private DeathLinkService deathLinkService;
        public Dictionary<string, object> slotData;
        public bool sentCompletion = false;
        public bool sentRelease = false;
        public bool sentCollect = false;
        public int ItemIndex = 0;

        public void Update() {
            if ((SceneManager.GetActiveScene().name == "TitleScreen" && TunicRandomizer.Settings.Mode != RandomizerSettings.RandomizerType.ARCHIPELAGO) || SaveFile.GetInt("archipelago") == 0) {
                return;
            }

            if (!connected) {
                return;
            }

            if (checkItemsReceived != null) {
                checkItemsReceived.MoveNext();
            }

            if (SceneManager.GetActiveScene().name != "TitleScreen" && SceneManager.GetActiveScene().name != "Loading" && PlayerCharacter.instance != null && SpeedrunData.gameComplete == 0) {

                if (incomingItemHandler != null) {
                    incomingItemHandler.MoveNext();
                }

                if (outgoingItemHandler != null) {
                    outgoingItemHandler.MoveNext();
                }

            }

            if (SpeedrunData.gameComplete != 0 && !sentCompletion) {
                sentCompletion = true;
                SendCompletion();
            }

        }

        public void TryConnect() {
            
            TryDisconnect();

            RandomizerSettings settings = JsonConvert.DeserializeObject<RandomizerSettings>(File.ReadAllText(TunicRandomizer.SettingsPath));
            TunicRandomizer.Settings.ConnectionSettings = settings.ConnectionSettings;

            LoginResult LoginResult;
            if (connected) {
                return;
            }
            if (session == null) {
                try {
                    session = ArchipelagoSessionFactory.CreateSession(TunicRandomizer.Settings.ConnectionSettings.Hostname, int.Parse(TunicRandomizer.Settings.ConnectionSettings.Port));
                } catch (Exception e) {
                    Logger.LogInfo("Failed to create archipelago session!");
                }
            }
            incomingItemHandler = IncomingItemHandler();
            outgoingItemHandler = OutgoingItemHandler();
            checkItemsReceived = CheckItemsReceived();
            incomingItems = new ConcurrentQueue<(NetworkItem NetworkItem, int index)>();
            outgoingItems = new ConcurrentQueue<NetworkItem>();

            TunicRandomizer.Tracker = new ItemTracker();

            try {
                LoginResult = session.TryConnectAndLogin("TUNIC", TunicRandomizer.Settings.ConnectionSettings.Player, ItemsHandlingFlags.AllItems, requestSlotData: true, password: TunicRandomizer.Settings.ConnectionSettings.Password);
            } catch (Exception e) {
                LoginResult = new LoginFailure(e.GetBaseException().Message);
            }
            
            if (LoginResult is LoginSuccessful LoginSuccess) {

                slotData = LoginSuccess.SlotData;

                connected = true;
                Logger.LogInfo("Successfully connected to Archipelago Multiworld server!");

                deathLinkService = session.CreateDeathLinkService();

                deathLinkService.OnDeathLinkReceived += (deathLinkObject) => {
                    if (SceneManager.GetActiveScene().name != "TitleScreen") {
                        Logger.LogInfo("Death link received");
                        PlayerCharacterPatches.DeathLinkMessage = deathLinkObject.Cause == null ? $"\"{deathLinkObject.Source} died and took you with them.\"": $"\"{deathLinkObject.Cause}\"";
                        PlayerCharacterPatches.DiedToDeathLink = true;
                    }
                };

                if (TunicRandomizer.Settings.DeathLinkEnabled) {
                    deathLinkService.EnableDeathLink();
                }

                SetupDataStorage();

            } else {
                LoginFailure loginFailure = (LoginFailure)LoginResult;
                Logger.LogInfo("Error connecting to Archipelago:");
                Notifications.Show($"\"Failed to connect to Archipelago!\"", $"\"Check your settings and/or log output.\"");
                foreach (string Error in loginFailure.Errors) {
                    Logger.LogInfo(Error);
                }
                foreach (ConnectionRefusedError Error in loginFailure.ErrorCodes) {
                    Logger.LogInfo(Error);
                }
                connected = false;
            }
        }

        public void TryDisconnect() {

            try {
                
                if (session != null) {
                    session.Socket.DisconnectAsync();
                    session = null;
                }

                connected = false;
                incomingItemHandler = null;
                outgoingItemHandler = null;
                checkItemsReceived = null;
                incomingItems = new ConcurrentQueue<(NetworkItem NetworkItem, int ItemIndex)>();
                outgoingItems = new ConcurrentQueue<NetworkItem>();
                deathLinkService = null;
                slotData = null;
                ItemIndex = 0;
                Locations.CheckedLocations.Clear();
                ItemLookup.ItemList.Clear();

                Logger.LogInfo("Disconnected from Archipelago");
            } catch (Exception e) {
                Logger.LogInfo("Encountered an error disconnecting from Archipelago!");
            }
        }

        private IEnumerator<bool> CheckItemsReceived() {
            while (connected) {
                if (session.Items.AllItemsReceived.Count > ItemIndex) {
                    NetworkItem Item = session.Items.AllItemsReceived[ItemIndex];
                    string ItemReceivedName = session.Items.GetItemName(Item.Item);
                    Logger.LogInfo("Placing item " + ItemReceivedName + " with index " + ItemIndex + " in queue.");
                    incomingItems.Enqueue((Item, ItemIndex));
                    ItemIndex++;
                    yield return true;
                } else {
                    yield return true;
                    continue;
                }
            }
        }

        private IEnumerator<bool> IncomingItemHandler() {
            while (connected) {

                if (!incomingItems.TryPeek(out var pendingItem)) {
                    yield return true;
                    continue;
                }

                var networkItem = pendingItem.NetworkItem;
                var itemName = session.Items.GetItemName(networkItem.Item);
                var itemDisplayName = itemName + " (" + networkItem.Item + ") at index " + pendingItem.index;

                if (SaveFile.GetInt($"randomizer processed item index {pendingItem.index}") == 1) {
                    incomingItems.TryDequeue(out _);
                    TunicRandomizer.Tracker.SetCollectedItem(itemName, false);
                    Logger.LogInfo("Skipping item " + itemName + " at index " + pendingItem.index + " as it has already been processed.");
                    yield return true;
                    continue;
                }

                // Delay until a few seconds after connecting/screen transition
                while (SaveFile.GetFloat("playtime") < SceneLoaderPatches.TimeOfLastSceneTransition + 3.0f) {
                    yield return true;
                }

                var handleResult = ItemPatches.GiveItem(itemName, networkItem);
                switch (handleResult) {
                    case ItemPatches.ItemResult.Success:
                        Logger.LogInfo("Received " + itemDisplayName + " from " + session.Players.GetPlayerName(networkItem.Player));

                        incomingItems.TryDequeue(out _);
                        SaveFile.SetInt($"randomizer processed item index {pendingItem.index}", 1);

                        // Wait for all interactions to finish
                        while (
                            GenericMessage.instance.isActiveAndEnabled ||
                            GenericPrompt.instance.isActiveAndEnabled ||
                            ItemPresentation.instance.isActiveAndEnabled ||
                            PageDisplay.instance.isActiveAndEnabled ||
                            NPCDialogue.instance.isActiveAndEnabled || 
                            PlayerCharacter.InstanceIsDead) {
                            yield return true;
                        }

                        // Pause before processing next item
                        DateTime postInteractionStart = DateTime.Now;
                        while (DateTime.Now < postInteractionStart + TimeSpan.FromSeconds(3.5)) {
                            yield return true;
                        }

                        break;

                    case ItemPatches.ItemResult.TemporaryFailure:
                        Logger.LogDebug("Player is busy, will retry processing item: " + itemDisplayName);
                        break;

                    case ItemPatches.ItemResult.PermanentFailure:
                        Logger.LogWarning("Failed to process item " + itemDisplayName);
                        incomingItems.TryDequeue(out _);
                        SaveFile.SetInt($"randomizer processed item index {pendingItem.index}", 1);
                        break;
                }

                yield return true;
            }
        }

        private IEnumerator<bool> OutgoingItemHandler() {
            while (connected) {
                if (!outgoingItems.TryDequeue(out var networkItem)) {
                    yield return true;
                    continue;
                }

                var itemName = session.Items.GetItemName(networkItem.Item);
                var location = session.Locations.GetLocationNameFromId(networkItem.Location);
                var receiver = session.Players.GetPlayerName(networkItem.Player);

                Logger.LogInfo("Sent " + itemName + " at " + location + " for " + receiver);

                if (networkItem.Player != session.ConnectionInfo.Slot) {
                    SaveFile.SetInt("archipelago items sent to other players", SaveFile.GetInt("archipelago items sent to other players")+1);
                    Notifications.Show($"yoo sehnt  {(TextBuilderPatches.ItemNameToAbbreviation.ContainsKey(itemName) && Archipelago.instance.IsTunicPlayer(networkItem.Player) ? TextBuilderPatches.ItemNameToAbbreviation[itemName] : "[archipelago]")}  \"{itemName.Replace("_", " ")}\" too \"{receiver}!\"", $"hOp #A lIk it!");
                }
                
                yield return true;
            }
        }

        public void ActivateCheck(string LocationId) {
            var sceneName = SceneManager.GetActiveScene().name;
            
            if (LocationId != null) {
                Logger.LogInfo("Checked location " + LocationId);
                var location = session.Locations.GetLocationIdFromName(session.ConnectionInfo.Game, LocationId);

                session.Locations.CompleteLocationChecks(location);

                string GameObjectId = Locations.LocationDescriptionToId[LocationId];
                SaveFile.SetInt(ItemCollectedKey + GameObjectId, 1);

                Locations.CheckedLocations[GameObjectId] = true;
                if (GameObject.Find($"fairy target {GameObjectId}")) {
                    GameObject.Destroy(GameObject.Find($"fairy target {GameObjectId}"));
                }
                if (Locations.VanillaLocations.Keys.Where(key => Locations.VanillaLocations[key].Location.SceneName == SceneLoaderPatches.SceneName && !Locations.CheckedLocations[key]).ToList().Count == 0) {
                    FairyTargets.CreateLoadZoneTargets();
                }

                if (TunicRandomizer.Settings.CreateSpoilerLog && !TunicRandomizer.Settings.RaceMode) {
                    ItemTracker.PopulateSpoilerLog();
                }

                session.Locations.ScoutLocationsAsync(location)
                    .ContinueWith(locationInfoPacket =>
                        outgoingItems.Enqueue(locationInfoPacket.Result.Locations[0]));

            } else {
                Logger.LogWarning("Failed to get unique name for check " + LocationId);
                Notifications.Show($"\"Unknown Check: {LocationId}\"", $"\"Please file a bug!\"");
            }
        }

        public void SendCompletion() { 
            StatusUpdatePacket statusUpdatePacket = new StatusUpdatePacket();
            statusUpdatePacket.Status = ArchipelagoClientState.ClientGoal;
            session.Socket.SendPacket(statusUpdatePacket);
            UpdateDataStorage("Reached an Ending", true);
        }

        public void Release() {
            if (connected && sentCompletion && !sentRelease) {
                session.Socket.SendPacket(new SayPacket() { Text = "!release" });
                sentRelease = true;
                Logger.LogInfo("Released remaining checks.");
            }
        }

        public void Collect() {
            if (connected && sentCompletion && !sentCollect) {
                session.Socket.SendPacket(new SayPacket() { Text = "!collect" });
                sentCollect = true;
                Logger.LogInfo("Collected remaining items.");
            }
        }

        public void EnableDeathLink() {
            if (deathLinkService == null) {
                Logger.LogWarning("Cannot enable death link service as it is null.");
            }
            
            Logger.LogInfo("Enabled death link service");
            deathLinkService.EnableDeathLink();
        }

        public void DisableDeathLink() {
            if (deathLinkService == null) {
                Logger.LogWarning("Cannot disable death link service as it is null.");
            }

            Logger.LogInfo("Disabled death link service");
            deathLinkService.DisableDeathLink();
        }

        public void SendDeathLink() {
            string Player = TunicRandomizer.Settings.ConnectionSettings.Player;
            string AreaDiedIn = "";
            if (DeathLinkMessages.Causes.ContainsKey(SceneLoaderPatches.SceneName)) {
                AreaDiedIn = SceneLoaderPatches.SceneName;
            } else {
                foreach (string key in Locations.MainAreasToSubAreas.Keys) {
                    if (Locations.MainAreasToSubAreas[key].Contains(SceneLoaderPatches.SceneName)) {
                        AreaDiedIn = key;
                        break;
                    }
                }
            }
            if (AreaDiedIn == "") {
                AreaDiedIn = "Generic";
            }

            deathLinkService.SendDeathLink(new DeathLink(Player, $"{Player}{DeathLinkMessages.Causes[AreaDiedIn][new System.Random().Next(DeathLinkMessages.Causes[AreaDiedIn].Count)]}"));
        }

        private void SetupDataStorage() {
            if (session != null) {
                Logger.LogInfo("Initializing DataStorage values");
                // Map to Display
                session.DataStorage[Scope.Slot, "Current Map"].Initialize("Overworld");

                // Boss Info
                session.DataStorage[Scope.Slot, "Defeated Guard Captain"].Initialize(false);
                session.DataStorage[Scope.Slot, "Defeated Garden Knight"].Initialize(false);
                session.DataStorage[Scope.Slot, "Defeated Siege Engine"].Initialize(false);
                session.DataStorage[Scope.Slot, "Defeated Librarian"].Initialize(false);
                session.DataStorage[Scope.Slot, "Defeated Boss Scavenger"].Initialize(false);
                session.DataStorage[Scope.Slot, "Cleared Cathedral Gauntlet"].Initialize(false);
                session.DataStorage[Scope.Slot, "Reached an Ending"].Initialize(false);

                // Bells
                session.DataStorage[Scope.Slot, "Rang East Bell"].Initialize(false);
                session.DataStorage[Scope.Slot, "Rang West Bell"].Initialize(false);

                // Bomb Codes
                session.DataStorage[Scope.Slot, "Granted Firecracker"].Initialize(false);
                session.DataStorage[Scope.Slot, "Granted Firebomb"].Initialize(false);
                session.DataStorage[Scope.Slot, "Granted Icebomb"].Initialize(false);

            }
        }

        public void UpdateDataStorage(string Key, object Value, bool Log = true) {

            if (Value is bool) {
                session.DataStorage[Scope.Slot, Key] = (bool)Value;
            }
            if (Value is int) {
                session.DataStorage[Scope.Slot, Key] = (int)Value;
            }
            if (Value is string) {
                session.DataStorage[Scope.Slot, Key] = (string)Value;
            }
            if (Log) {
                Logger.LogInfo("Setting DataStorage value \"" + Key + "\" to " + Value);
            }
        }

        public void UpdateDataStorageOnLoad() {
            // Map to Display
            foreach (string Key in Locations.PopTrackerMapScenes.Keys) {
                if (Locations.PopTrackerMapScenes[Key].Contains(SceneLoaderPatches.SceneName)) {
                    UpdateDataStorage("Current Map", Key);
                    break;
                }
            }

            // Boss Info
            UpdateDataStorage("Defeated Guard Captain", StateVariable.GetStateVariableByName("SV_Forest Boss Room_Skuladot redux Big").BoolValue, false);
            UpdateDataStorage("Defeated Garden Knight", StateVariable.GetStateVariableByName("SV_Archipelagos Redux TUNIC Knight is Dead").BoolValue, false);
            UpdateDataStorage("Defeated Siege Engine", StateVariable.GetStateVariableByName("SV_Fortress Arena_Spidertank Is Dead").BoolValue, false);
            UpdateDataStorage("Defeated Librarian", StateVariable.GetStateVariableByName("Librarian Dead Forever").BoolValue, false);
            UpdateDataStorage("Defeated Boss Scavenger", StateVariable.GetStateVariableByName("SV_ScavengerBossesDead").BoolValue, false);
            UpdateDataStorage("Cleared Cathedral Gauntlet", StateVariable.GetStateVariableByName("SV_Cathedral Arena Mockup_Waves Done").BoolValue, false);
            UpdateDataStorage("Reached an Ending", SpeedrunData.gameComplete != 0, false);

            // Bells
            UpdateDataStorage("Rang East Bell", StateVariable.GetStateVariableByName("Rung Bell 1 (East)").BoolValue, false);
            UpdateDataStorage("Rang West Bell", StateVariable.GetStateVariableByName("Rung Bell 2 (West)").BoolValue, false);

            // Bomb Codes
            UpdateDataStorage("Granted Firecracker", StateVariable.GetStateVariableByName("Granted Firecracker").BoolValue, false);
            UpdateDataStorage("Granted Firebomb", StateVariable.GetStateVariableByName("Granted Firebomb").BoolValue, false);
            UpdateDataStorage("Granted Icebomb", StateVariable.GetStateVariableByName("Granted Icebomb").BoolValue, false);
        }

        public Dictionary<string, int> GetStartInventory() {
            Dictionary<string, int> startInventory = new Dictionary<string, int>();
            if (connected && session != null) {
                // start inventory items have a location ID of -2, add them to a dict so we can use them for first steps
                foreach (NetworkItem item in session.Items.AllItemsReceived) {
                    if (item.Location == -2) {
                        string itemName = session.Items.GetItemName(item.Item);
                        if (ItemLookup.Items.ContainsKey(itemName)) {
                            ItemRandomizer.AddStringToDict(startInventory, ItemLookup.Items[itemName].ItemNameForInventory);
                        }
                    }
                }
            }
            return startInventory;
        }
    }
}
