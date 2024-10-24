using Archipelago.MultiClient.Net.Helpers;
using System.Collections.Generic;
using System.Linq;
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

    }
}
