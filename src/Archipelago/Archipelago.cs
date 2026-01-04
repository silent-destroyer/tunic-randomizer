using System;
using System.Collections.Generic;
using UnityEngine;

namespace TunicRandomizer {
    public class Archipelago : MonoBehaviour {
        public static Archipelago instance { get; set; }

        public ArchipelagoIntegration integration;

        public void Start() {
            integration = new ArchipelagoIntegration();
        }

        public void Update() {
            integration.Update();
        }

        public void OnDestroy() {
            integration.TryDisconnect();
        }

        public string Connect() {
            return integration.TryConnect();
        }

        public void SilentReconnect() {
            integration.TrySilentReconnect();
        }

        public void Disconnect() {
            integration.TryDisconnect();
        }

        public void ActivateCheck(string LocationName) {
            integration.ActivateCheck(LocationName);
        }

        public void CompleteLocationCheck(string LocationName) {
            integration.CompleteLocationCheck(LocationName);
        }

        public void UpdateDataStorage(string Key, object Value) {
            if (SaveFlags.IsArchipelago()) {
                integration.UpdateDataStorage(Key, Value);
            }
        }

        public void Release() {
            integration.Release();
        }

        public void Collect() {
            integration.Collect();
        }

        public Dictionary<string, object> GetPlayerSlotData() {
            return integration.slotData;
        }

        public int GetPlayerSlot() {
            return integration.session.ConnectionInfo.Slot;
        }

        public string GetPlayerName(int Slot) {
            return integration.session.Players.GetPlayerName(Slot).Replace("{", "").Replace("}", "");
        }

        public string GetPlayerGame(int Slot) {
            return integration.session.Players.Players[0][Slot].Game;
        }

        public bool IsTunicPlayer(int Slot) {
            return GetPlayerGame(Slot) == "TUNIC" && integration.session.Players.GetPlayerInfo(Slot).GetGroupMembers(integration.session.Players) == null;
        }

        public string GetItemName(long id, string game) {
            return integration.session.Items.GetItemName(id, game);
        }

        public string GetLocationName(long id, string game) { 
            return integration.session.Locations.GetLocationNameFromId(id, game);
        }

        public long GetLocationId(string name, string game) {
            return integration.session.Locations.GetLocationIdFromName(game, name);
        }

        public bool IsConnected() {
            return integration != null ? integration.connected : false;
        }

        public void CheckForArchipelagoLauncherArgs() {
            string[] args = Il2CppSystem.Environment.GetCommandLineArgs();

            if (args.Length > 0) {
                foreach (string arg in args) {
                    try {
                        if (ParseUri(arg)) {
                            break;
                        }
                    } catch {
                        continue;
                    }
                }
            }
        }

        public bool ParseUri(string arg) {
            Uri uri = new Uri(arg);

            //if (uri.Scheme == "archipelago") {
                string[] UserInfo = uri.UserInfo.Split(':');

                TunicRandomizer.Settings.ConnectionSettings.Player = UserInfo[0];
                TunicRandomizer.Settings.ConnectionSettings.Hostname = uri.Host;
                TunicRandomizer.Settings.ConnectionSettings.Port = uri.Port.ToString();

                if (UserInfo.Length > 1 && UserInfo[1] != "None") {
                    TunicRandomizer.Settings.ConnectionSettings.Password = UserInfo[1];
                }

                TunicRandomizer.Settings.Mode = RandomizerSettings.RandomizerType.ARCHIPELAGO;
                RandomizerSettings.SaveSettings();
                return true;
            //}
            return false;
        }

    }
}
