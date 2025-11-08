using InControl;

namespace TunicRandomizer {
    public class RandoActionSet : GUIActionSet {
        public RandoActionSet() {
            this.SkipCredits = base.CreatePlayerAction("Skip Credits");
            this.Release = base.CreatePlayerAction("Release");
            this.Collect = base.CreatePlayerAction("Collect");
            this.HideStats = base.CreatePlayerAction("Hide Stats");
        }

        public static RandoActionSet CreateWithDefaultBindings() {
            RandoActionSet randoActionSet = new RandoActionSet();
            randoActionSet.SetUpBindings();
            DeviceBindingSource deviceBindingSource = new DeviceBindingSource(InputControlType.RightCommand);
            KeyBindingSource keyBindingSource = new KeyBindingSource(KeyCombo.With(Key.Space));
            randoActionSet.SkipCredits.AddDefaultBinding(InputControlType.RightCommand);
            randoActionSet.SkipCredits.AddDefaultBinding(deviceBindingSource);
            randoActionSet.SkipCredits.AddBinding(deviceBindingSource);
            randoActionSet.SkipCredits.AddBinding(keyBindingSource);

            DeviceBindingSource deviceBindingSource2 = new DeviceBindingSource(InputControlType.LeftStickButton);
            KeyBindingSource keyBindingSource2 = new KeyBindingSource(KeyCombo.With(Key.R));
            randoActionSet.Release.AddDefaultBinding(InputControlType.LeftStickButton);
            randoActionSet.Release.AddDefaultBinding(deviceBindingSource2);
            randoActionSet.Release.AddBinding(deviceBindingSource2);
            randoActionSet.Release.AddBinding(keyBindingSource2);

            DeviceBindingSource deviceBindingSource3 = new DeviceBindingSource(InputControlType.RightStickButton);
            KeyBindingSource keyBindingSource3 = new KeyBindingSource(KeyCombo.With(Key.C));
            randoActionSet.Collect.AddDefaultBinding(InputControlType.RightStickButton);
            randoActionSet.Collect.AddDefaultBinding(deviceBindingSource3);
            randoActionSet.Collect.AddBinding(deviceBindingSource3);
            randoActionSet.Collect.AddBinding(keyBindingSource3);


            DeviceBindingSource deviceBindingSource4 = new DeviceBindingSource(InputControlType.LeftCommand);
            KeyBindingSource keyBindingSource4 = new KeyBindingSource(KeyCombo.With(Key.H));
            randoActionSet.HideStats.AddDefaultBinding(InputControlType.LeftCommand);
            randoActionSet.HideStats.AddDefaultBinding(deviceBindingSource4);
            randoActionSet.HideStats.AddBinding(deviceBindingSource4);
            randoActionSet.HideStats.AddBinding(keyBindingSource4);
            return randoActionSet;
        }

        public PlayerAction SkipCredits;

        public PlayerAction Release;

        public PlayerAction Collect;

        public PlayerAction HideStats;
    }
}
