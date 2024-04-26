using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Logging;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Models;
using System.Runtime.CompilerServices;
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

        public void Connect() {
            integration.TryConnect();
        }

        public void Disconnect() {
            integration.TryDisconnect();
        }

        public void ActivateCheck(string LocationId) {
            integration.ActivateCheck(LocationId);
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
            return GetPlayerGame(Slot) == "TUNIC";
        }

        public string GetItemName(long id) {
            return integration.session.Items.GetItemName(id);
        }

        public string GetLocationName(long id) { 
            return integration.session.Locations.GetLocationNameFromId(id);
        }

        public long GetLocationId(string name) {
            return integration.session.Locations.GetLocationIdFromName("TUNIC", name);
        }

    }
}
