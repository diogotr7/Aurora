using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Devices;
using Aurora.Settings;
using LedCSharp;
using Aurora.Utils;

namespace Device_Logitech
{
    public class LogitechDevice : Device
    {
        protected override string DeviceName => "Logitech";

        public override bool Initialize()
        {
            if (GlobalVarRegistry.GetVariable<bool>($"{DeviceName}_override"))
                LogitechGSDK.GHUB = GlobalVarRegistry.GetVariable<DLLType>($"{DeviceName}_dlltype") == DLLType.GHUB;
            else
                LogitechGSDK.GHUB = Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "LGHUB"));

            LogInfo("Trying to initialize Logitech using the dll for " + (LogitechGSDK.GHUB ? "GHUB" : "LGS"));

            if (LogitechGSDK.LogiLedInit())
            {
                LogitechGSDK.LogiLedSaveCurrentLighting();
                LogitechGSDK.LogiLedSetLighting(GlobalVarRegistry.GetVariable<RealColor>($"{DeviceName}_color").GetDrawingColor());
                return isInitialized = true;
            }

            return isInitialized = false;
        }

        public override void Shutdown()
        {
            isInitialized = false;
            LogitechGSDK.LogiLedRestoreLighting();
            LogitechGSDK.LogiLedShutdown();
        }

        public override bool UpdateDevice(Dictionary<DeviceKeys, System.Drawing.Color> keyColors, DoWorkEventArgs e, bool forced = false)
        {
            foreach (var key in keyColors)
            {
                if (KeyMap.TryGetValue(key.Key, out var logiKey))
                    LogitechGSDK.LogiLedSetLightingForKeyWithKeyName(logiKey, key.Value);
                else if (PeripheralMap.TryGetValue(key.Key, out var peripheral))
                    LogitechGSDK.LogiLedSetLightingForTargetZone(peripheral.type, peripheral.zone, key.Value);
                else if(HidCodeMap.TryGetValue(key.Key, out var hidId))
                    LogitechGSDK.LogiLedSetLightingForKeyWithHidCode(hidId, key.Value);
            }
            return true;
        }

        protected override void RegisterVariables(VariableRegistry local)
        {
            //hack to not have to reference wpf stuff to get the Media.Color :(
            var black = new RealColor();
            black.SetDrawingColor(Color.Black);
            var white = new RealColor();
            white.SetDrawingColor(Color.White);

            local.Register($"{DeviceName}_color", black, "Default Color", white, black);
            local.Register($"{DeviceName}_override", false, "Override DLL");
            local.Register($"{DeviceName}_dlltype", DLLType.LGS, "DLL Type");
        }

        private static readonly Dictionary<DeviceKeys, LedId> KeyMap = new Dictionary<DeviceKeys, LedId>()
        {
            [DeviceKeys.ESC] = LedId.ESC,
            [DeviceKeys.F1] = LedId.F1,
            [DeviceKeys.F2] = LedId.F2,
            [DeviceKeys.F3] = LedId.F3,
            [DeviceKeys.F4] = LedId.F4,
            [DeviceKeys.F5] = LedId.F5,
            [DeviceKeys.F6] = LedId.F6,
            [DeviceKeys.F7] = LedId.F7,
            [DeviceKeys.F8] = LedId.F8,
            [DeviceKeys.F9] = LedId.F9,
            [DeviceKeys.F10] = LedId.F10,
            [DeviceKeys.F11] = LedId.F11,
            [DeviceKeys.F12] = LedId.F12,
            [DeviceKeys.PRINT_SCREEN] = LedId.PRINT_SCREEN,
            [DeviceKeys.SCROLL_LOCK] = LedId.SCROLL_LOCK,
            [DeviceKeys.PAUSE_BREAK] = LedId.PAUSE_BREAK,
            [DeviceKeys.TILDE] = LedId.TILDE,
            [DeviceKeys.ONE] = LedId.ONE,
            [DeviceKeys.TWO] = LedId.TWO,
            [DeviceKeys.THREE] = LedId.THREE,
            [DeviceKeys.FOUR] = LedId.FOUR,
            [DeviceKeys.FIVE] = LedId.FIVE,
            [DeviceKeys.SIX] = LedId.SIX,
            [DeviceKeys.SEVEN] = LedId.SEVEN,
            [DeviceKeys.EIGHT] = LedId.EIGHT,
            [DeviceKeys.NINE] = LedId.NINE,
            [DeviceKeys.ZERO] = LedId.ZERO,
            [DeviceKeys.MINUS] = LedId.MINUS,
            [DeviceKeys.EQUALS] = LedId.EQUALS,
            [DeviceKeys.BACKSPACE] = LedId.BACKSPACE,
            [DeviceKeys.INSERT] = LedId.INSERT,
            [DeviceKeys.HOME] = LedId.HOME,
            [DeviceKeys.PAGE_UP] = LedId.PAGE_UP,
            [DeviceKeys.NUM_LOCK] = LedId.NUM_LOCK,
            [DeviceKeys.NUM_SLASH] = LedId.NUM_SLASH,
            [DeviceKeys.NUM_ASTERISK] = LedId.NUM_ASTERISK,
            [DeviceKeys.NUM_MINUS] = LedId.NUM_MINUS,
            [DeviceKeys.TAB] = LedId.TAB,
            [DeviceKeys.Q] = LedId.Q,
            [DeviceKeys.W] = LedId.W,
            [DeviceKeys.E] = LedId.E,
            [DeviceKeys.R] = LedId.R,
            [DeviceKeys.T] = LedId.T,
            [DeviceKeys.Y] = LedId.Y,
            [DeviceKeys.U] = LedId.U,
            [DeviceKeys.I] = LedId.I,
            [DeviceKeys.O] = LedId.O,
            [DeviceKeys.P] = LedId.P,
            [DeviceKeys.OPEN_BRACKET] = LedId.OPEN_BRACKET,
            [DeviceKeys.CLOSE_BRACKET] = LedId.CLOSE_BRACKET,
            [DeviceKeys.BACKSLASH] = LedId.BACKSLASH,
            [DeviceKeys.DELETE] = LedId.KEYBOARD_DELETE,
            [DeviceKeys.END] = LedId.END,
            [DeviceKeys.PAGE_DOWN] = LedId.PAGE_DOWN,
            [DeviceKeys.NUM_SEVEN] = LedId.NUM_SEVEN,
            [DeviceKeys.NUM_EIGHT] = LedId.NUM_EIGHT,
            [DeviceKeys.NUM_NINE] = LedId.NUM_NINE,
            [DeviceKeys.NUM_PLUS] = LedId.NUM_PLUS,
            [DeviceKeys.CAPS_LOCK] = LedId.CAPS_LOCK,
            [DeviceKeys.A] = LedId.A,
            [DeviceKeys.S] = LedId.S,
            [DeviceKeys.D] = LedId.D,
            [DeviceKeys.F] = LedId.F,
            [DeviceKeys.G] = LedId.G,
            [DeviceKeys.H] = LedId.H,
            [DeviceKeys.J] = LedId.J,
            [DeviceKeys.K] = LedId.K,
            [DeviceKeys.L] = LedId.L,
            [DeviceKeys.SEMICOLON] = LedId.SEMICOLON,
            [DeviceKeys.APOSTROPHE] = LedId.APOSTROPHE,
            [DeviceKeys.ENTER] = LedId.ENTER,
            [DeviceKeys.NUM_FOUR] = LedId.NUM_FOUR,
            [DeviceKeys.NUM_FIVE] = LedId.NUM_FIVE,
            [DeviceKeys.NUM_SIX] = LedId.NUM_SIX,
            [DeviceKeys.LEFT_SHIFT] = LedId.LEFT_SHIFT,
            [DeviceKeys.Z] = LedId.Z,
            [DeviceKeys.X] = LedId.X,
            [DeviceKeys.C] = LedId.C,
            [DeviceKeys.V] = LedId.V,
            [DeviceKeys.B] = LedId.B,
            [DeviceKeys.N] = LedId.N,
            [DeviceKeys.M] = LedId.M,
            [DeviceKeys.COMMA] = LedId.COMMA,
            [DeviceKeys.PERIOD] = LedId.PERIOD,
            [DeviceKeys.FORWARD_SLASH] = LedId.FORWARD_SLASH,
            [DeviceKeys.RIGHT_SHIFT] = LedId.RIGHT_SHIFT,
            [DeviceKeys.ARROW_UP] = LedId.ARROW_UP,
            [DeviceKeys.NUM_ONE] = LedId.NUM_ONE,
            [DeviceKeys.NUM_TWO] = LedId.NUM_TWO,
            [DeviceKeys.NUM_THREE] = LedId.NUM_THREE,
            [DeviceKeys.NUM_ENTER] = LedId.NUM_ENTER,
            [DeviceKeys.LEFT_CONTROL] = LedId.LEFT_CONTROL,
            [DeviceKeys.LEFT_WINDOWS] = LedId.LEFT_WINDOWS,
            [DeviceKeys.LEFT_ALT] = LedId.LEFT_ALT,
            [DeviceKeys.SPACE] = LedId.SPACE,
            [DeviceKeys.RIGHT_ALT] = LedId.RIGHT_ALT,
            [DeviceKeys.RIGHT_WINDOWS] = LedId.RIGHT_WINDOWS,
            [DeviceKeys.APPLICATION_SELECT] = LedId.APPLICATION_SELECT,
            [DeviceKeys.RIGHT_CONTROL] = LedId.RIGHT_CONTROL,
            [DeviceKeys.ARROW_LEFT] = LedId.ARROW_LEFT,
            [DeviceKeys.ARROW_DOWN] = LedId.ARROW_DOWN,
            [DeviceKeys.ARROW_RIGHT] = LedId.ARROW_RIGHT,
            [DeviceKeys.NUM_ZERO] = LedId.NUM_ZERO,
            [DeviceKeys.NUM_PERIOD] = LedId.NUM_PERIOD,
            [DeviceKeys.G1] = LedId.G_1,
            [DeviceKeys.G2] = LedId.G_2,
            [DeviceKeys.G3] = LedId.G_3,
            [DeviceKeys.G4] = LedId.G_4,
            [DeviceKeys.G5] = LedId.G_5,
            [DeviceKeys.G6] = LedId.G_6,
            [DeviceKeys.G7] = LedId.G_7,
            [DeviceKeys.G8] = LedId.G_8,
            [DeviceKeys.G9] = LedId.G_9,
            [DeviceKeys.LOGO] = LedId.G_LOGO,
            [DeviceKeys.LOGO2] = LedId.G_BADGE,
        };

        private static readonly Dictionary<DeviceKeys, (DeviceType type, int zone)> PeripheralMap = new Dictionary<DeviceKeys, (DeviceType, int)>()
        {
            [DeviceKeys.Peripheral_Logo] = (DeviceType.Mouse, 1),
            [DeviceKeys.Peripheral_FrontLight] = (DeviceType.Mouse, 0),
            [DeviceKeys.Peripheral_ScrollWheel] = (DeviceType.Mouse, 2),
            [DeviceKeys.MOUSEPADLIGHT1] = (DeviceType.Mousemat, 0)
        };

        private static readonly Dictionary<DeviceKeys, int> HidCodeMap = new Dictionary<DeviceKeys, int>()
        {
            [DeviceKeys.BACKSLASH_UK] = 0x64,
            [DeviceKeys.HASHTAG] = 0x32
        };
    }
}
