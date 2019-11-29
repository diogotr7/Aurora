using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Settings;
using TongFang;

namespace Aurora.Devices.TongFang
{
    class TongFangDevice : Device
    {
        private string deviceName = "TongFang";
        private long lastUpdateTime = 0;
        private bool isInitialized => Keyboard.IsConnected;
        private readonly Stopwatch watch = new Stopwatch();
        private VariableRegistry default_registry = null;

        public string GetDeviceDetails()
        {
            return deviceName + ": " + (isInitialized ? "Connected" : "Not initialized");
        }

        public string GetDeviceName()
        {
            return deviceName;
        }

        public string GetDeviceUpdatePerformance()
        {
            return (isInitialized ? lastUpdateTime + " ms" : "");
        }

        public VariableRegistry GetRegisteredVariables()
        {
            if(default_registry == null)
            {
                default_registry = new VariableRegistry();
                default_registry.Register($"{deviceName}_restore_color",
                    new Aurora.Utils.RealColor(System.Drawing.Color.FromArgb(255, 0, 0, 255)),"Restore Color",
                    new Aurora.Utils.RealColor(System.Drawing.Color.FromArgb(255, 255, 255, 255)),
                    new Aurora.Utils.RealColor(System.Drawing.Color.FromArgb(0, 0, 0, 0)));
                default_registry.Register($"{deviceName}_brightness", 50, "Brightness", 100, 1, "In percent");
                default_registry.Register($"{deviceName}_ansi", true, "ANSI layout", remark: "All options require a restart of the device integration.");
                default_registry.Register($"{deviceName}_scalar_r", 255, "Red Scalar", 255, 0);
                default_registry.Register($"{deviceName}_scalar_g", 255, "Green Scalar", 255, 0);
                default_registry.Register($"{deviceName}_scalar_b", 255, "Blue Scalar", 255, 0);
            }
            return default_registry;
        }

        public bool Initialize()
        {
            var layout = Global.Configuration.VarRegistry.GetVariable<bool>($"{deviceName}_ansi") ?Layout.ANSI : Layout.ISO;
            var brightness = Global.Configuration.VarRegistry.GetVariable<int>($"{deviceName}_brightness");

            if (Keyboard.Initialize(brightness, layout))
            {
                Keyboard.SetColorFull(Color.Black);
                Global.logger.Info($"Initialized TongFang with {layout.ToString()} layout and {brightness}% brightness");
                return true;
            }
            else
            {
                Global.logger.Info("TongFang device not initialized");
                return false;
            }
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }

        public bool IsInitialized()
        {
            return isInitialized;
        }

        public bool IsKeyboardConnected()
        {
            return isInitialized;
        }

        public bool IsPeripheralConnected()
        {
            return isInitialized;
        }

        public bool Reconnect()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {

        }

        public void Shutdown()
        {
            Keyboard.SetColorFull(Global.Configuration.VarRegistry.GetVariable<Color>($"{deviceName}_restore_color"));
            Keyboard.Update();
            Keyboard.Disconnect();
        }

        public bool UpdateDevice(Dictionary<DeviceKeys, Color> keyColors, DoWorkEventArgs e, bool forced = false)
        {
            if (!isInitialized)
                return false;

            foreach (KeyValuePair<DeviceKeys, Color> key in keyColors)
            {
                Color correction = Color.FromArgb(
                          (byte)(key.Value.R * (Global.Configuration.VarRegistry.GetVariable<int>($"{deviceName}_scalar_r") / 255.0D)),
                          (byte)(key.Value.G * (Global.Configuration.VarRegistry.GetVariable<int>($"{deviceName}_scalar_g") / 255.0D)),
                          (byte)(key.Value.B * (Global.Configuration.VarRegistry.GetVariable<int>($"{deviceName}_scalar_b") / 255.0D)));

                Color clr = Color.FromArgb(255, Utils.ColorUtils.MultiplyColorByScalar(correction, correction.A / 255.0D));

                if (KeyMap.TryGetValue(key.Key, out var tfKey))
                    Keyboard.SetKeyColor(tfKey, clr);
            }

            return Keyboard.Update();
        }

        public bool UpdateDevice(DeviceColorComposition colorComposition, DoWorkEventArgs e, bool forced = false)
        {
            watch.Restart();

            bool update_result = UpdateDevice(colorComposition.keyColors, e, forced);

            watch.Stop();
            lastUpdateTime = watch.ElapsedMilliseconds;

            return update_result;
        }

        public static Dictionary<DeviceKeys, Key> KeyMap = new Dictionary<DeviceKeys, Key>
        {
            { DeviceKeys.ESC, Key.ESCAPE},
            { DeviceKeys.TILDE, Key.TILDE},
            { DeviceKeys.TAB, Key.TAB},
            { DeviceKeys.LEFT_SHIFT, Key.LEFT_SHIFT},
            { DeviceKeys.CAPS_LOCK, Key.CAPS_LOCK},
            { DeviceKeys.LEFT_CONTROL, Key.LEFT_CONTROL},
            { DeviceKeys.LEFT_WINDOWS, Key.LEFT_WINDOWS },
            { DeviceKeys.ONE, Key.ONE},
            { DeviceKeys.TWO, Key.TWO },
            { DeviceKeys.THREE, Key.THREE },
            { DeviceKeys.FOUR, Key.FOUR },
            { DeviceKeys.FIVE, Key.FIVE },
            { DeviceKeys.SIX, Key.SIX },
            { DeviceKeys.SEVEN, Key.SEVEN },
            { DeviceKeys.EIGHT, Key.EIGHT },
            { DeviceKeys.NINE, Key.NINE },
            { DeviceKeys.ZERO, Key.ZERO },
            { DeviceKeys.F1, Key.F1 },
            { DeviceKeys.F2, Key.F2 },
            { DeviceKeys.F3, Key.F3 },
            { DeviceKeys.F4, Key.F4 },
            { DeviceKeys.F5, Key.F5 },
            { DeviceKeys.F6, Key.F6 },
            { DeviceKeys.F7, Key.F7 },
            { DeviceKeys.F8, Key.F8 },
            { DeviceKeys.F9, Key.F9 },
            { DeviceKeys.F10, Key.F10 },
            { DeviceKeys.F11, Key.F11 },
            { DeviceKeys.F12, Key.F12 },
            { DeviceKeys.Q, Key.Q},
            { DeviceKeys.A, Key.A},
            { DeviceKeys.W, Key.W },
            { DeviceKeys.S, Key.S },
            { DeviceKeys.Z, Key.Z },
            { DeviceKeys.LEFT_ALT, Key.LEFT_ALT },
            { DeviceKeys.E, Key.E },
            { DeviceKeys.D, Key.D },
            { DeviceKeys.X, Key.X },
            { DeviceKeys.R, Key.R },
            { DeviceKeys.C, Key.C },
            { DeviceKeys.T, Key.T },
            { DeviceKeys.G, Key.G },
            { DeviceKeys.V, Key.V },
            { DeviceKeys.Y, Key.Y },
            { DeviceKeys.H, Key.H },
            { DeviceKeys.B, Key.B },
            { DeviceKeys.SPACE, Key.SPACE },
            { DeviceKeys.U, Key.U },
            { DeviceKeys.J, Key.J },
            { DeviceKeys.N, Key.N },
            { DeviceKeys.I, Key.I },
            { DeviceKeys.K, Key.K },
            { DeviceKeys.M, Key.M },
            { DeviceKeys.O, Key.O },
            { DeviceKeys.L, Key.L },
            { DeviceKeys.F, Key.F },
            { DeviceKeys.P, Key.P },
            { DeviceKeys.COMMA, Key.COMMA },
            { DeviceKeys.SEMICOLON, Key.SEMICOLON },
            { DeviceKeys.PERIOD, Key.PERIOD },
            { DeviceKeys.RIGHT_ALT, Key.RIGHT_ALT },
            { DeviceKeys.MINUS, Key.MINUS },
            { DeviceKeys.OPEN_BRACKET, Key.OPEN_BRACKET },
            { DeviceKeys.APOSTROPHE, Key.APOSTROPHE },
            { DeviceKeys.FORWARD_SLASH, Key.FORWARD_SLASH },
            { DeviceKeys.LEFT_FN, Key.FN_Key },
            { DeviceKeys.EQUALS, Key.EQUALS },
            { DeviceKeys.CLOSE_BRACKET, Key.CLOSE_BRACKET },
            { DeviceKeys.BACKSLASH, Key.BACKSLASH },
            { DeviceKeys.RIGHT_SHIFT, Key.RIGHT_SHIFT },
            { DeviceKeys.APPLICATION_SELECT, Key.MENU },
            { DeviceKeys.BACKSPACE, Key.BACKSPACE },
            { DeviceKeys.ENTER, Key.ENTER },
            { DeviceKeys.RIGHT_CONTROL, Key.RIGHT_CONTROL },
            { DeviceKeys.PRINT_SCREEN, Key.PRINT_SCREEN },
            { DeviceKeys.INSERT, Key.INSERT },
            { DeviceKeys.DELETE, Key.DELETE },
            { DeviceKeys.ARROW_LEFT, Key.ARROW_LEFT },
           // { DeviceKeys.SCROLL_LOCK, Key.SCROLL_LOCK },
            { DeviceKeys.HOME, Key.HOME },
            { DeviceKeys.END, Key.END },
            { DeviceKeys.ARROW_UP, Key.UP_ARROW },
            { DeviceKeys.ARROW_DOWN, Key.DOWN_ARROW },
          //  { DeviceKeys.PAUSE_BREAK, Key.PAUSE_BREAK },
            { DeviceKeys.PAGE_UP, Key.PAGE_UP },
            { DeviceKeys.PAGE_DOWN, Key.PAGE_DOWN },
            { DeviceKeys.ARROW_RIGHT, Key.RIGHT_ARROW },
            { DeviceKeys.NUM_LOCK, Key.NUMLOCK },
            { DeviceKeys.NUM_SEVEN, Key.NUMPAD_SEVEN },
            { DeviceKeys.NUM_FOUR, Key.NUMPAD_FOUR },
            { DeviceKeys.NUM_ONE, Key.NUMPAD_ONE },
            { DeviceKeys.NUM_ZERO, Key.NUMPAD_0 },
            { DeviceKeys.NUM_SLASH, Key.NUMPAD_DIVIDE },
            { DeviceKeys.NUM_EIGHT, Key.NUMPAD_EIGHT },
            { DeviceKeys.NUM_FIVE, Key.NUMPAD_FIVE },
            { DeviceKeys.NUM_TWO, Key.NUMPAD_TWO },
            { DeviceKeys.NUM_ASTERISK, Key.NUMPAD_MULTIPLY },
            { DeviceKeys.NUM_NINE, Key.NUMPAD_NINE },
            { DeviceKeys.NUM_SIX, Key.NUMPAD_SIX },
            { DeviceKeys.NUM_THREE, Key.NUMPAD_THREE },
            { DeviceKeys.NUM_PERIOD, Key.NUMPAD_PERIOD },
            { DeviceKeys.NUM_MINUS, Key.NUMPAD_MINUS },
            { DeviceKeys.NUM_PLUS, Key.NUMPAD_PLUS },
            { DeviceKeys.NUM_ENTER, Key.NUMPAD_ENTER },
        };
    }
}
