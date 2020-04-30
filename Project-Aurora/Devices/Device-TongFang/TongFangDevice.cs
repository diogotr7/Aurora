using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Devices;
using Aurora.Settings;
using Aurora.Utils;
using TongFang;

namespace Device_TongFang
{
    public class TongFangDevice : Device
    {
        protected override string DeviceName => "TongFang";

        public override bool Initialize()
        {
            var layout = GlobalVarRegistry.GetVariable<Layout>($"{DeviceName}_layout");
            var brightness = GlobalVarRegistry.GetVariable<int>($"{DeviceName}_brightness");

            if (Keyboard.Initialize(brightness, layout))
            {
                Keyboard.SetColorFull(Color.Black);
                LogInfo($"Initialized with {layout} layout and {brightness}% brightness");
                return isInitialized = true;
            }
            else
            {
                return isInitialized = true;
            }
        }

        public override void Shutdown()
        {
            Keyboard.SetColorFull(GlobalVarRegistry.GetVariable<Color>($"{DeviceName}_restore_color"));
            Keyboard.Update();
            Keyboard.Disconnect();
            isInitialized = false;
        }

        public override bool UpdateDevice(Dictionary<DeviceKeys, System.Drawing.Color> keyColors, DoWorkEventArgs e, bool forced = false)
        {
            if (!isInitialized)
                return false;

            foreach (KeyValuePair<DeviceKeys, Color> key in keyColors)
            {
                Color correction = Color.FromArgb(
                          (byte)(key.Value.R * (GlobalVarRegistry.GetVariable<int>($"{DeviceName}_scalar_r") / 255.0D)),
                          (byte)(key.Value.G * (GlobalVarRegistry.GetVariable<int>($"{DeviceName}_scalar_g") / 255.0D)),
                          (byte)(key.Value.B * (GlobalVarRegistry.GetVariable<int>($"{DeviceName}_scalar_b") / 255.0D)));

                if (KeyMap.TryGetValue(key.Key, out var tfKey))
                    Keyboard.SetKeyColor(tfKey, CorrectAlpha(correction));
            }

            return Keyboard.Update();
        }

        protected override void RegisterVariables(VariableRegistry local)
        {
            var black = new RealColor();
            black.SetDrawingColor(Color.Black);
            var white = new RealColor();
            white.SetDrawingColor(Color.White);
            local.Register($"{DeviceName}_restore_color",
                white, "Restore Color",
                white,
                black
            );
            local.Register($"{DeviceName}_brightness", 50, "Brightness", 100, 1, "In percent");
            local.Register($"{DeviceName}_layout", Layout.ANSI15, "Layout", remark: "All options require a restart of the device integration.");
            local.Register($"{DeviceName}_scalar_r", 255, "Red Scalar", 255, 0);
            local.Register($"{DeviceName}_scalar_g", 255, "Green Scalar", 255, 0);
            local.Register($"{DeviceName}_scalar_b", 255, "Blue Scalar", 255, 0);
        }

        public static Dictionary<DeviceKeys, Key> KeyMap = new Dictionary<DeviceKeys, Key>
        {
            //Row 0
            [DeviceKeys.ESC] = Key.ESCAPE,
            [DeviceKeys.F1] = Key.F1,
            [DeviceKeys.F2] = Key.F2,
            [DeviceKeys.F3] = Key.F3,
            [DeviceKeys.F4] = Key.F4,
            [DeviceKeys.F5] = Key.F5,
            [DeviceKeys.F6] = Key.F6,
            [DeviceKeys.F7] = Key.F7,
            [DeviceKeys.F8] = Key.F8,
            [DeviceKeys.F9] = Key.F9,
            [DeviceKeys.F10] = Key.F10,
            [DeviceKeys.F11] = Key.F11,
            [DeviceKeys.F12] = Key.F12,
            [DeviceKeys.INSERT] = Key.INSERT,
            [DeviceKeys.DELETE] = Key.DELETE,
            [DeviceKeys.HOME] = Key.HOME,
            [DeviceKeys.END] = Key.END,
            [DeviceKeys.PAGE_UP] = Key.PAGE_UP,
            [DeviceKeys.PAGE_DOWN] = Key.PAGE_DOWN,
            //Row 1
            [DeviceKeys.TILDE] = Key.TILDE,
            [DeviceKeys.ONE] = Key.ONE,
            [DeviceKeys.TWO] = Key.TWO,
            [DeviceKeys.THREE] = Key.THREE,
            [DeviceKeys.FOUR] = Key.FOUR,
            [DeviceKeys.FIVE] = Key.FIVE,
            [DeviceKeys.SIX] = Key.SIX,
            [DeviceKeys.SEVEN] = Key.SEVEN,
            [DeviceKeys.EIGHT] = Key.EIGHT,
            [DeviceKeys.NINE] = Key.NINE,
            [DeviceKeys.ZERO] = Key.ZERO,
            [DeviceKeys.MINUS] = Key.MINUS,
            [DeviceKeys.EQUALS] = Key.EQUALS,
            [DeviceKeys.BACKSPACE] = Key.BACKSPACE,
            [DeviceKeys.NUM_LOCK] = Key.NUMLOCK,
            [DeviceKeys.NUM_SLASH] = Key.NUMPAD_DIVIDE,
            [DeviceKeys.NUM_ASTERISK] = Key.NUMPAD_MULTIPLY,
            [DeviceKeys.NUM_MINUS] = Key.NUMPAD_MINUS,
            //Row 2
            [DeviceKeys.TAB] = Key.TAB,
            [DeviceKeys.Q] = Key.Q,
            [DeviceKeys.W] = Key.W,
            [DeviceKeys.E] = Key.E,
            [DeviceKeys.R] = Key.R,
            [DeviceKeys.T] = Key.T,
            [DeviceKeys.Y] = Key.Y,
            [DeviceKeys.U] = Key.U,
            [DeviceKeys.I] = Key.I,
            [DeviceKeys.O] = Key.O,
            [DeviceKeys.P] = Key.P,
            [DeviceKeys.OPEN_BRACKET] = Key.OPEN_BRACKET,
            [DeviceKeys.CLOSE_BRACKET] = Key.CLOSE_BRACKET,
            [DeviceKeys.BACKSLASH] = Key.ANSI_BACKSLASH,
            [DeviceKeys.ENTER] = Key.ENTER,
            [DeviceKeys.NUM_SEVEN] = Key.NUMPAD_SEVEN,
            [DeviceKeys.NUM_EIGHT] = Key.NUMPAD_EIGHT,
            [DeviceKeys.NUM_NINE] = Key.NUMPAD_NINE,
            [DeviceKeys.NUM_PLUS] = Key.NUMPAD_PLUS,
            //Row 3
            [DeviceKeys.CAPS_LOCK] = Key.CAPS_LOCK,
            [DeviceKeys.A] = Key.A,
            [DeviceKeys.S] = Key.S,
            [DeviceKeys.D] = Key.D,
            [DeviceKeys.F] = Key.F,
            [DeviceKeys.G] = Key.G,
            [DeviceKeys.H] = Key.H,
            [DeviceKeys.J] = Key.J,
            [DeviceKeys.K] = Key.K,
            [DeviceKeys.L] = Key.L,
            [DeviceKeys.SEMICOLON] = Key.SEMICOLON,
            [DeviceKeys.APOSTROPHE] = Key.APOSTROPHE,
            [DeviceKeys.HASHTAG] = Key.ISO_HASH,
            [DeviceKeys.NUM_FOUR] = Key.NUMPAD_FOUR,
            [DeviceKeys.NUM_FIVE] = Key.NUMPAD_FIVE,
            [DeviceKeys.NUM_SIX] = Key.NUMPAD_SIX,
            //Row 4
            [DeviceKeys.LEFT_SHIFT] = Key.LEFT_SHIFT,
            [DeviceKeys.BACKSLASH_UK] = Key.ISO_BACKSLASH,
            [DeviceKeys.Z] = Key.Z,
            [DeviceKeys.X] = Key.X,
            [DeviceKeys.C] = Key.C,
            [DeviceKeys.V] = Key.V,
            [DeviceKeys.B] = Key.B,
            [DeviceKeys.N] = Key.N,
            [DeviceKeys.M] = Key.M,
            [DeviceKeys.COMMA] = Key.COMMA,
            [DeviceKeys.PERIOD] = Key.PERIOD,
            [DeviceKeys.FORWARD_SLASH] = Key.FORWARD_SLASH,
            [DeviceKeys.RIGHT_SHIFT] = Key.RIGHT_SHIFT,
            [DeviceKeys.ARROW_UP] = Key.ARROW_UP,
            [DeviceKeys.NUM_ONE] = Key.NUMPAD_ONE,
            [DeviceKeys.NUM_TWO] = Key.NUMPAD_TWO,
            [DeviceKeys.NUM_THREE] = Key.NUMPAD_THREE,
            [DeviceKeys.NUM_ENTER] = Key.NUMPAD_ENTER,
            //Row 5
            [DeviceKeys.LEFT_CONTROL] = Key.LEFT_CONTROL,
            [DeviceKeys.LEFT_FN] = Key.LEFT_FN,
            [DeviceKeys.LEFT_WINDOWS] = Key.LEFT_WINDOWS,
            [DeviceKeys.LEFT_ALT] = Key.LEFT_ALT,
            [DeviceKeys.SPACE] = Key.SPACE,
            [DeviceKeys.RIGHT_ALT] = Key.RIGHT_ALT,
            [DeviceKeys.APPLICATION_SELECT] = Key.APP,
            [DeviceKeys.RIGHT_CONTROL] = Key.RIGHT_CONTROL,
            [DeviceKeys.ARROW_LEFT] = Key.ARROW_LEFT,
            [DeviceKeys.ARROW_DOWN] = Key.ARROW_DOWN,
            [DeviceKeys.ARROW_RIGHT] = Key.ARROW_RIGHT,
            [DeviceKeys.NUM_ZERO] = Key.NUMPAD_0,
            [DeviceKeys.NUM_PERIOD] = Key.NUMPAD_PERIOD,
            [DeviceKeys.PRINT_SCREEN] = Key.PRINT_SCREEN,
        };
    }
}
