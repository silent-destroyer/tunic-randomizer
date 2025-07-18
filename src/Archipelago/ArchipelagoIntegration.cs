﻿using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TunicRandomizer.SaveFlags;
using static TunicRandomizer.ERData;
using Archipelago.MultiClient.Net.Packets;
using Newtonsoft.Json.Linq;
using Archipelago.MultiClient.Net.Converters;

namespace TunicRandomizer {
    public class ArchipelagoIntegration {
        
        public bool connected {
            get { return session != null ? session.Socket.Connected : false; }
        }

        public ArchipelagoSession session;
        private IEnumerator<bool> incomingItemHandler;
        private IEnumerator<bool> checkItemsReceived;
        private ConcurrentQueue<(ItemInfo ItemInfo, int index)> incomingItems;
        private DeathLinkService deathLinkService;
        public Dictionary<string, object> slotData;
        public bool disableSpoilerLog = false;
        public bool sentCompletion = false;
        public bool sentRelease = false;
        public bool sentCollect = false;
        public int ItemIndex = 0;
        public List<long> locationsToSend = new List<long>();
        public float locationsToSendTimer = 0.0f;
        public float locationsToSendDelay = 5.0f;
        private Version archipelagoVersion = new Version("0.5.1");

        private IEnumerator<bool> checkTrapLink;
        private Queue<(string, string, DateTime)> trapLinkQueue;

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

                if (checkTrapLink != null) { 
                    checkTrapLink.MoveNext();
                }

                if ((locationsToSendTimer > locationsToSendDelay && locationsToSend.Count > 0) || locationsToSend.Count >= 10) {
                    SendQueuedLocations();
                    locationsToSendTimer = 0.0f;
                }
                locationsToSendTimer += Time.fixedUnscaledDeltaTime;
            }

            if (SpeedrunData.gameComplete != 0 && !sentCompletion) {
                sentCompletion = true;
                SendCompletion();
            }

        }

        public string TryConnect() {
            
            if (connected && TunicRandomizer.Settings.ConnectionSettings.Player == session.Players.GetPlayerName(session.ConnectionInfo.Slot)) {
                return "";
            }

            TryDisconnect();

            LoginResult LoginResult;

            if (session == null) {
                try {
                    session = ArchipelagoSessionFactory.CreateSession(TunicRandomizer.Settings.ConnectionSettings.Hostname, int.Parse(TunicRandomizer.Settings.ConnectionSettings.Port));
                } catch (Exception e) {
                    TunicLogger.LogInfo("Failed to create archipelago session!");
                }
            }
            incomingItemHandler = IncomingItemHandler();
            checkItemsReceived = CheckItemsReceived();
            incomingItems = new ConcurrentQueue<(ItemInfo ItemInfo, int index)>();
            locationsToSend = new List<long>();

            checkTrapLink = CheckTrapLinkQueue();
            trapLinkQueue = new Queue<(string, string, DateTime)>();

            try {
                LoginResult = session.TryConnectAndLogin("TUNIC", TunicRandomizer.Settings.ConnectionSettings.Player, ItemsHandlingFlags.AllItems, version: archipelagoVersion, requestSlotData: true, password: TunicRandomizer.Settings.ConnectionSettings.Password);
            } catch (Exception e) {
                LoginResult = new LoginFailure(e.GetBaseException().Message);
            }
            
            if (LoginResult is LoginSuccessful LoginSuccess) {

                slotData = LoginSuccess.SlotData;

                TunicLogger.LogInfo("Successfully connected to Archipelago Multiworld server!");

                deathLinkService = session.CreateDeathLinkService();

                deathLinkService.OnDeathLinkReceived += (deathLinkObject) => {
                    if (SceneManager.GetActiveScene().name != "TitleScreen") {
                        TunicLogger.LogInfo("Death link received");
                        PlayerCharacterPatches.DeathLinkMessage = deathLinkObject.Cause == null ? $"\"{deathLinkObject.Source} died and took you with them.\"": $"\"{deathLinkObject.Cause}\"";
                        PlayerCharacterPatches.DiedToDeathLink = true;
                    }
                };

                if (TunicRandomizer.Settings.DeathLinkEnabled) {
                    deathLinkService.EnableDeathLink();
                }

                if (TunicRandomizer.Settings.TrapLinkEnabled) {
                    EnableTrapLink();
                }

                session.Socket.PacketReceived += ReceiveTrapLink;

                if (slotData.ContainsKey("disable_local_spoiler") && slotData["disable_local_spoiler"].ToString() == "1") {
                    disableSpoilerLog = true;
                    TunicRandomizer.Settings.CreateSpoilerLog = false;
                    TunicUtils.TryDeleteFile(TunicRandomizer.SpoilerLogPath);
                } else {
                    disableSpoilerLog = false;
                }

                if (Locations.LocationIdToArchipelagoId.Count == 0) {
                    foreach (string Key in Locations.LocationDescriptionToId.Keys) {
                        Locations.LocationIdToArchipelagoId.Add(Locations.LocationDescriptionToId[Key], Archipelago.instance.integration.session.Locations.GetLocationIdFromName("TUNIC", Key));
                    }
                }

                SetupDataStorage();

            } else {
                LoginFailure loginFailure = (LoginFailure)LoginResult;
                TunicLogger.LogInfo("Error connecting to Archipelago:");
                string TopLine = $"\"Failed to connect to Archipelago!\"";
                string BottomLine = $"\"Check your settings and/or log output.\"";
                foreach (string Error in loginFailure.Errors) {
                    BottomLine = $"\"{Error}\"";
                }
                Notifications.Show(TopLine, BottomLine);
                foreach (ConnectionRefusedError Error in loginFailure.ErrorCodes) {
                    TunicLogger.LogInfo(Error.ToString());
                }
                TryDisconnect();
                return BottomLine;
            }
            return "";
        }

        public void TrySilentReconnect() {
            LoginResult LoginResult;
            try {
                LoginResult = session.TryConnectAndLogin("TUNIC", TunicRandomizer.Settings.ConnectionSettings.Player, ItemsHandlingFlags.AllItems, version: archipelagoVersion, requestSlotData: true, password: TunicRandomizer.Settings.ConnectionSettings.Password);
            } catch (Exception e) {
                LoginResult = new LoginFailure(e.GetBaseException().Message);
            }
        }

        public void TryDisconnect() {

            try {
                if (connected) {
                    TunicLogger.LogInfo("Disconnected from Archipelago");
                }
                if (session != null) {
                    session.Socket.DisconnectAsync();
                    session = null;
                }

                incomingItemHandler = null;
                checkItemsReceived = null;
                disableSpoilerLog = false;
                incomingItems = new ConcurrentQueue<(ItemInfo ItemInfo, int ItemIndex)>();
                locationsToSend = new List<long>();
                trapLinkQueue = null;
                checkTrapLink = null;
                deathLinkService = null;
                slotData = null;
                ItemIndex = 0;
                Locations.CheckedLocations.Clear();
                ItemLookup.ItemList.Clear();

            } catch (Exception e) {
                TunicLogger.LogInfo("Encountered an error disconnecting from Archipelago!");
            }
        }

        public void ClearQueue() {
            if (incomingItems != null) {
                incomingItems = new ConcurrentQueue<(ItemInfo ItemInfo, int ItemIndex)>();
            }
        }

        private IEnumerator<bool> CheckItemsReceived() {
            while (connected) {
                while (session.Items.AllItemsReceived.Count > ItemIndex) {
                    ItemInfo ItemInfo = session.Items.AllItemsReceived[ItemIndex];
                    TunicLogger.LogInfo("Placing item " + ItemInfo.ItemDisplayName + " with index " + ItemIndex + " in queue.");
                    incomingItems.Enqueue((ItemInfo, ItemIndex));
                    ItemIndex++;
                }
                yield return true;
            }
        }

        private IEnumerator<bool> IncomingItemHandler() {
            while (connected) {

                if (!incomingItems.TryPeek(out var pendingItem)) {
                    yield return true;
                    continue;
                }

                var itemInfo = pendingItem.ItemInfo;
                var itemName = itemInfo.ItemDisplayName;
                var itemDisplayName = itemName + " (" + itemInfo.ItemId + ") at index " + pendingItem.index;

                if (itemName == "Grass" && SaveFile.GetInt($"randomizer processed item index {pendingItem.index}") == 0 && (itemInfo.Player != session.ConnectionInfo.Slot || GrassRandomizer.GrassChecks.ContainsKey(Locations.LocationDescriptionToId[itemInfo.LocationDisplayName]))) {
                    SaveFile.SetInt($"randomizer processed item index {pendingItem.index}", 1);
                    Inventory.GetItemByName("Grass").Quantity += 1;
                    incomingItems.TryDequeue(out _);
                    if (incomingItems.TryPeek(out var nextItem)) {
                        if (nextItem.ItemInfo.ItemDisplayName == "Grass") {
                            continue;
                        }
                    }
                    yield return true;
                    continue;
                }

                if (SaveFile.GetInt($"randomizer processed item index {pendingItem.index}") == 1) {
                    incomingItems.TryDequeue(out _);
                    TunicLogger.LogInfo("Skipping item " + itemName + " at index " + pendingItem.index + " as it has already been processed.");
                    continue;
                }

                // Delay until a few seconds after connecting/screen transition
                while (SaveFile.GetFloat("playtime") < SceneLoaderPatches.TimeOfLastSceneTransition + 3.0f) {
                    yield return true;
                }

                var handleResult = ItemPatches.GiveItem(itemName, itemInfo);
                switch (handleResult) {
                    case ItemPatches.ItemResult.Success:
                        TunicLogger.LogInfo("Received " + itemDisplayName + " from " + itemInfo.Player.Name + " at " + itemInfo.LocationDisplayName);

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
                        if (itemInfo.ItemDisplayName != "Grass") {
                            DateTime postInteractionStart = DateTime.Now;
                            while (DateTime.Now < postInteractionStart + TimeSpan.FromSeconds(incomingItems.Count > 10 ? 1f : 2f)) {
                                yield return true;
                            }
                        }

                        break;

                    case ItemPatches.ItemResult.TemporaryFailure:
                        TunicLogger.LogDebug("Player is busy, will retry processing item: " + itemDisplayName);
                        break;

                    case ItemPatches.ItemResult.PermanentFailure:
                        TunicLogger.LogWarning("Failed to process item " + itemDisplayName);
                        incomingItems.TryDequeue(out _);
                        SaveFile.SetInt($"randomizer processed item index {pendingItem.index}", 1);
                        break;
                }

                yield return true;
            }
        }

        public void ActivateCheck(string LocationName) {
            var sceneName = SceneManager.GetActiveScene().name;
            if (LocationName != null) {
                TunicLogger.LogInfo("Checked location " + LocationName);
                string GameObjectId = Locations.LocationDescriptionToId[LocationName];
                var location = ItemLookup.ItemList[GameObjectId].LocationId;

                session.Locations.CompleteLocationChecks(location);

                SaveFile.SetInt(ItemCollectedKey + GameObjectId, 1);

                Locations.CheckedLocations[GameObjectId] = true;
                FairyTargets.RemoveFairyTarget(GameObjectId);
                if (SaveFile.GetInt(GrassRandoEnabled) != 1 && TunicRandomizer.Settings.CreateSpoilerLog && !TunicRandomizer.Settings.RaceMode) {
                    ItemTracker.PopulateSpoilerLog();
                }

                ItemInfo itemInfo = ItemLookup.ItemList[GameObjectId];
                string receiver = itemInfo.Player.Name;
                string itemName = itemInfo.ItemDisplayName;
                TunicLogger.LogInfo("Sent " + itemName + " at " + location + " to " + receiver);
                if (itemInfo.Player != session.ConnectionInfo.Slot) {
                    SaveFile.SetInt("archipelago items sent to other players", SaveFile.GetInt("archipelago items sent to other players") + 1);
                    Notifications.Show($"yoo sehnt  {(TextBuilderPatches.ItemNameToAbbreviation.ContainsKey(itemName) && Archipelago.instance.IsTunicPlayer(itemInfo.Player) ? TextBuilderPatches.ItemNameToAbbreviation[itemName] : "[archipelago]")}  \"{itemName.Replace("_", " ")}\" too \"{receiver}!\"", $"hOp #A lIk it!");
                    RecentItemsDisplay.instance.EnqueueItem(itemInfo, false);
                }

            } else {
                TunicLogger.LogWarning("Failed to get unique name for check " + LocationName);
                Notifications.Show($"\"Unknown Check: {LocationName}\"", $"\"Please file a bug!\"");
            }
        }

        public void SendQueuedLocations() {
            if (locationsToSend.Count > 0) {
                TunicLogger.LogInfo("Sending queued grass checks: " + string.Join(", ", locationsToSend));
                session.Locations.CompleteLocationChecks(locationsToSend.ToArray());
                locationsToSend.Clear();
            }
        }

        public void CompleteLocationCheck(string LocationName) {
            if (LocationName != null) {
                var location = session.Locations.GetLocationIdFromName(session.ConnectionInfo.Game, LocationName);
                session.Locations.CompleteLocationChecks(location);

            }
        }

        public void SendCompletion() {
            session.SetGoalAchieved();
            UpdateDataStorage("Reached an Ending", true);
        }

        public void Release() {
            if (connected && sentCompletion && !sentRelease) {
                session.Say("!release");
                sentRelease = true;
                TunicLogger.LogInfo("Released remaining checks.");
            }
        }

        public void Collect() {
            if (connected && sentCompletion && !sentCollect) {
                session.Say("!collect");
                sentCollect = true;
                TunicLogger.LogInfo("Collected remaining items.");
            }
        }

        public void EnableDeathLink() {
            if (deathLinkService == null) {
                TunicLogger.LogWarning("Cannot enable death link service as it is null.");
            }
            
            TunicLogger.LogInfo("Enabled death link service");
            deathLinkService.EnableDeathLink();
        }

        public void DisableDeathLink() {
            if (deathLinkService == null) {
                TunicLogger.LogWarning("Cannot disable death link service as it is null.");
            }

            TunicLogger.LogInfo("Disabled death link service");
            deathLinkService.DisableDeathLink();
        }

        public void SendDeathLink() {
            string Player = SaveFile.GetString(SaveFlags.ArchipelagoPlayerName);
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


        public void EnableTrapLink() {
            if (!session.ConnectionInfo.Tags.Contains("TrapLink")) {
                session.ConnectionInfo.UpdateConnectionOptions(session.ConnectionInfo.Tags.Concat(new string[1] { "TrapLink" }).ToArray());
            }
        }

        public void DisableTrapLink() {
            if (session.ConnectionInfo.Tags.Contains("TrapLink")) {
                session.ConnectionInfo.UpdateConnectionOptions(session.ConnectionInfo.Tags.Where(tag => tag != "TrapLink").ToArray());
            }
        }

        public void SendTrapLink(FoolTrap.TrapType trapType) {
            BouncePacket bouncePacket = new BouncePacket {
                Tags = new List<string> { "TrapLink" },
                Data = new Dictionary<string, JToken>
                {
                    { "time", (float)DateTime.Now.ToUnixTimeStamp() },
                    { "source", SaveFile.GetString(SaveFlags.ArchipelagoPlayerName) },
                    { "trap_name", FoolTrap.Traps[trapType].Name }
                }
            };
            session.Socket.SendPacketAsync(bouncePacket);
        }

        public void ReceiveTrapLink(ArchipelagoPacketBase packet) {
            if (packet is BouncedPacket bouncedPacket && bouncedPacket.Tags.Contains("TrapLink")) {
                // we don't want to receive own trap links, since the other slot will have already received a trap on its own
                // note: if two people are connected to the same slot, both players will likely send their own trap links
                // idk if we can actually fix this?
                if (bouncedPacket.Data["source"].ToString() == SaveFile.GetString(SaveFlags.ArchipelagoPlayerName)) {
                    return;
                }
                string trapName = bouncedPacket.Data["trap_name"].ToString();
                string source = bouncedPacket.Data["source"].ToString();
                DateTime time = DateTime.Now;
                if (FoolTrap.TrapNameToType.ContainsKey(trapName)) {
                    trapLinkQueue.Enqueue((trapName, source, time));
                } else {
                    return;
                }
            }
        }

        private IEnumerator<bool> CheckTrapLinkQueue() {
            while (connected) {
                if (ItemPresentation.instance.isActiveAndEnabled || GenericMessage.instance.isActiveAndEnabled ||
                        NPCDialogue.instance.isActiveAndEnabled || PageDisplay.instance.isActiveAndEnabled || GenericPrompt.instance.isActiveAndEnabled ||
                        GameObject.Find("_GameGUI(Clone)/PauseMenu/") != null || GameObject.Find("_OptionsGUI(Clone)") != null || PlayerCharacter.InstanceIsDead) {
                    yield return true;
                    continue;
                }

                if (trapLinkQueue.Count == 0) {
                    yield return true;
                    continue;
                }

                (string, string, DateTime) trapSource = trapLinkQueue.Dequeue();
                if (trapSource.Item3 + TimeSpan.FromSeconds(5f) > DateTime.Now) {
                    string FoolMessageTop = $"";
                    string FoolMessageBottom = $"";
                    (FoolMessageTop, FoolMessageBottom) = FoolTrap.ApplyFoolEffect(FoolTrap.TrapNameToType[trapSource.Item1]);
                    FoolMessageTop = $"\"{trapSource.Item2}\" %i^ks {FoolMessageTop}";

                    Notifications.Show(FoolMessageTop, FoolMessageBottom);
                }

                yield return true;
            }
        }


        private void SetupDataStorage() {
            if (session != null) {
                TunicLogger.LogInfo("Initializing DataStorage values");
                // Map to Display
                session.DataStorage[Scope.Slot, "Current Map"].Initialize("Overworld");
                session.DataStorage[Scope.Slot, "Entrance Tracker Map"].Initialize("overworld");

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

                if (slotData.ContainsKey("entrance_rando") && slotData["entrance_rando"].ToString() == "1") {
                    foreach (KeyValuePair<string, Dictionary<string, List<TunicPortal>>> portalDict in RegionPortalsList.Where(dict => dict.Key != "Shop")) {
                        foreach (KeyValuePair<string, List<TunicPortal>> portals in portalDict.Value) {
                            foreach (TunicPortal portal in portals.Value) {
                                string key = $"{portalDict.Key}, {portal.Destination}_{portal.Tag}";
                                TunicLogger.LogInfo("initializing datastorage value for portal: " + key);
                                session.DataStorage[Scope.Slot, key].Initialize(false);
                            }
                        }
                    }
                }
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
                TunicLogger.LogInfo("Setting DataStorage value \"" + Key + "\" to " + Value);
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
            foreach (string Key in Locations.EntranceTrackerMapScenes.Keys) {
                if (Locations.EntranceTrackerMapScenes[Key].Contains(SceneLoaderPatches.SceneName)) {
                    UpdateDataStorage("Entrance Tracker Map", Key);
                    break;
                }
            }

            // Boss Info
            UpdateDataStorage("Defeated Guard Captain", SaveFile.GetInt(EnemyRandomizer.CustomBossFlags[0]) == 1, false);
            UpdateDataStorage("Defeated Garden Knight", SaveFile.GetInt(EnemyRandomizer.CustomBossFlags[1]) == 1, false);
            UpdateDataStorage("Defeated Siege Engine", SaveFile.GetInt(EnemyRandomizer.CustomBossFlags[2]) == 1, false);
            UpdateDataStorage("Defeated Librarian", SaveFile.GetInt(EnemyRandomizer.CustomBossFlags[3]) == 1, false);
            UpdateDataStorage("Defeated Boss Scavenger", SaveFile.GetInt(EnemyRandomizer.CustomBossFlags[4]) == 1, false);
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
                foreach (ItemInfo item in session.Items.AllItemsReceived) {
                    if (item.LocationId == -2) {
                        string itemName = item.ItemDisplayName;
                        if (ItemLookup.Items.ContainsKey(itemName)) {
                            TunicUtils.AddStringToDict(startInventory, ItemLookup.Items[itemName].ItemNameForInventory);
                        }
                    }
                }
            }
            return startInventory;
        }

        public void ShowNotConnectedError() {
            Notifications.Show($"[archipelago] \"ERROR: Lost connection to Archipelago!\"", $"\"Unable to send or receive items. Re-connect and try again.\"");
        }
    }
}
