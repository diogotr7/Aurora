using Corale.Colore.Core;
using Corale.Colore.Razer.Keyboard;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aurora.Settings;
using KeyboardCustom = Corale.Colore.Razer.Keyboard.Effects.Custom;
using MousepadCustom = Corale.Colore.Razer.Mousepad.Effects.Custom;
using Aurora.Devices;

namespace Device_Razer
{
    public class RazerDeviceConnector : AuroraDeviceConnector
    {

        protected override string ConnectorName => "Razer";

        protected override bool InitializeImpl()
        {
            if (!Chroma.SdkAvailable)
                throw new Exception("No Chroma SDK available");

            Chroma.Instance.Initialize();

            TryToAddDevice(Chroma.Instance.Keyboard, AuroraDeviceType.Keyboard);
            TryToAddDevice(Chroma.Instance.Mouse, AuroraDeviceType.Mouse);
            TryToAddDevice(Chroma.Instance.Headset, AuroraDeviceType.Headset);
            TryToAddDevice(Chroma.Instance.Mousepad, AuroraDeviceType.Mousepad);
            TryToAddDevice(Chroma.Instance.Keypad, AuroraDeviceType.Unkown);
            TryToAddDevice(Chroma.Instance.ChromaLink, AuroraDeviceType.Unkown);



            /*if (Chroma.Instance.Query(Corale.Colore.Razer.Devices.BladeStealth).Connected || Chroma.Instance.Query(Corale.Colore.Razer.Devices.Blade14).Connected)
                bladeLayout = true;*/



            return true;
        }
        protected override void RunFirstTime()
        {
            Aurora.App.Current.Dispatcher.Invoke(() =>
            {
                RazerInstallInstructions instructions = new RazerInstallInstructions();
                instructions.ShowDialog();
            });
        }
        private void TryToAddDevice(Corale.Colore.Core.IDevice device, AuroraDeviceType deviceType)
        {
            if (device == null)
                return;
            if (deviceType == AuroraDeviceType.Keyboard)
            {
                devices.Add(new RazerKeyboardDevice((IKeyboard)device));
            }
            else if (deviceType == AuroraDeviceType.Mouse)
            {
                devices.Add(new RazerMouseDevice((IMouse)device));
            }
            else if (deviceType == AuroraDeviceType.Mousepad)
            {
                devices.Add(new RazerMousepadDevice((IMousepad)device));
            }
            else
            {
                devices.Add(new RazerDeviceDevice(device));
            }
        }

        protected override void ShutdownImpl()
        {

            //Chroma.Instance.Uninitialize();

        }
    }
    public class RazerKeyboardDevice : AuroraDevice
    {
        IKeyboard Keyboard = null;
        private KeyboardCustom grid = KeyboardCustom.Create();
        protected override string DeviceName => "Razer Keyboard";

        protected override AuroraDeviceType AuroraDeviceType => AuroraDeviceType.Keyboard;

        public RazerKeyboardDevice(IKeyboard keyboard)
        {
            Keyboard = keyboard;
        }
        protected override bool UpdateDeviceImpl(DeviceColorComposition composition)
        {
            foreach (KeyValuePair<DeviceKeys, System.Drawing.Color> key in composition.keyColors)
            {

                if (!TryToSetKey(key))
                {
                    Key localKey = ToRazer(key.Key);
                    SetOneKey(localKey, key.Value);
                }

            }

            Keyboard.SetCustom(grid);
            return true;
        }

        private int[] GetKeyCoord(DeviceKeys key)
        {
            Dictionary<DeviceKeys, int[]> layout = RazerLayoutMap.GenericKeyboard;

            if (Aurora.Global.Configuration.keyboard_brand == PreferredKeyboard.Razer_Blade)
                layout = RazerLayoutMap.Blade;

            if (layout.ContainsKey(key))
                return layout[key];

            return null;
        }

        private bool TryToSetKey(KeyValuePair<DeviceKeys, System.Drawing.Color> key)
        {
            int[] coords = null;

            if ((coords = GetKeyCoord(key.Key)) != null)
            {
                System.Drawing.Color color = CorrectAlpha(key.Value);
                grid[coords[0], coords[1]] = new Color(color.R, color.G, color.B);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SetOneKey(Key key, System.Drawing.Color color)
        {
            if (key == Key.Invalid)
                return;

            grid[key] = new Color(color.R, color.G, color.B);

        }
        private Key ToRazer(DeviceKeys key)
        {
            switch (key)
            {
                case (DeviceKeys.ESC):
                    return Key.Escape;
                case (DeviceKeys.F1):
                    return Key.F1;
                case (DeviceKeys.F2):
                    return Key.F2;
                case (DeviceKeys.F3):
                    return Key.F3;
                case (DeviceKeys.F4):
                    return Key.F4;
                case (DeviceKeys.F5):
                    return Key.F5;
                case (DeviceKeys.F6):
                    return Key.F6;
                case (DeviceKeys.F7):
                    return Key.F7;
                case (DeviceKeys.F8):
                    return Key.F8;
                case (DeviceKeys.F9):
                    return Key.F9;
                case (DeviceKeys.F10):
                    return Key.F10;
                case (DeviceKeys.F11):
                    return Key.F11;
                case (DeviceKeys.F12):
                    return Key.F12;
                case (DeviceKeys.PRINT_SCREEN):
                    return Key.PrintScreen;
                case (DeviceKeys.SCROLL_LOCK):
                    return Key.Scroll;
                case (DeviceKeys.PAUSE_BREAK):
                    return Key.Pause;
                case (DeviceKeys.TILDE):
                    return Key.OemTilde;
                case (DeviceKeys.ONE):
                    return Key.D1;
                case (DeviceKeys.TWO):
                    return Key.D2;
                case (DeviceKeys.THREE):
                    return Key.D3;
                case (DeviceKeys.FOUR):
                    return Key.D4;
                case (DeviceKeys.FIVE):
                    return Key.D5;
                case (DeviceKeys.SIX):
                    return Key.D6;
                case (DeviceKeys.SEVEN):
                    return Key.D7;
                case (DeviceKeys.EIGHT):
                    return Key.D8;
                case (DeviceKeys.NINE):
                    return Key.D9;
                case (DeviceKeys.ZERO):
                    return Key.D0;
                case (DeviceKeys.MINUS):
                    return Key.OemMinus;
                case (DeviceKeys.EQUALS):
                    return Key.OemEquals;
                case (DeviceKeys.BACKSPACE):
                    return Key.Backspace;
                case (DeviceKeys.INSERT):
                    return Key.Insert;
                case (DeviceKeys.HOME):
                    return Key.Home;
                case (DeviceKeys.PAGE_UP):
                    return Key.PageUp;
                case (DeviceKeys.NUM_LOCK):
                    return Key.NumLock;
                case (DeviceKeys.NUM_SLASH):
                    return Key.NumDivide;
                case (DeviceKeys.NUM_ASTERISK):
                    return Key.NumMultiply;
                case (DeviceKeys.NUM_MINUS):
                    return Key.NumSubtract;
                case (DeviceKeys.TAB):
                    return Key.Tab;
                case (DeviceKeys.Q):
                    return Key.Q;
                case (DeviceKeys.W):
                    return Key.W;
                case (DeviceKeys.E):
                    return Key.E;
                case (DeviceKeys.R):
                    return Key.R;
                case (DeviceKeys.T):
                    return Key.T;
                case (DeviceKeys.Y):
                    return Key.Y;
                case (DeviceKeys.U):
                    return Key.U;
                case (DeviceKeys.I):
                    return Key.I;
                case (DeviceKeys.O):
                    return Key.O;
                case (DeviceKeys.P):
                    return Key.P;
                case (DeviceKeys.OPEN_BRACKET):
                    return Key.OemLeftBracket;
                case (DeviceKeys.CLOSE_BRACKET):
                    return Key.OemRightBracket;
                case (DeviceKeys.BACKSLASH):
                    return Key.OemBackslash;
                case (DeviceKeys.DELETE):
                    return Key.Delete;
                case (DeviceKeys.END):
                    return Key.End;
                case (DeviceKeys.PAGE_DOWN):
                    return Key.PageDown;
                case (DeviceKeys.NUM_SEVEN):
                    return Key.Num7;
                case (DeviceKeys.NUM_EIGHT):
                    return Key.Num8;
                case (DeviceKeys.NUM_NINE):
                    return Key.Num9;
                case (DeviceKeys.NUM_PLUS):
                    return Key.NumAdd;
                case (DeviceKeys.CAPS_LOCK):
                    return Key.CapsLock;
                case (DeviceKeys.A):
                    return Key.A;
                case (DeviceKeys.S):
                    return Key.S;
                case (DeviceKeys.D):
                    return Key.D;
                case (DeviceKeys.F):
                    return Key.F;
                case (DeviceKeys.G):
                    return Key.G;
                case (DeviceKeys.H):
                    return Key.H;
                case (DeviceKeys.J):
                    return Key.J;
                case (DeviceKeys.K):
                    return Key.K;
                case (DeviceKeys.L):
                    return Key.L;
                case (DeviceKeys.SEMICOLON):
                    return Key.OemSemicolon;
                case (DeviceKeys.APOSTROPHE):
                    return Key.OemApostrophe;
                case (DeviceKeys.HASHTAG):
                    return Key.EurPound;
                case (DeviceKeys.ENTER):
                    return Key.Enter;
                case (DeviceKeys.NUM_FOUR):
                    return Key.Num4;
                case (DeviceKeys.NUM_FIVE):
                    return Key.Num5;
                case (DeviceKeys.NUM_SIX):
                    return Key.Num6;
                case (DeviceKeys.LEFT_SHIFT):
                    return Key.LeftShift;
                case (DeviceKeys.BACKSLASH_UK):
                    return Key.EurBackslash;
                case (DeviceKeys.Z):
                    return Key.Z;
                case (DeviceKeys.X):
                    return Key.X;
                case (DeviceKeys.C):
                    return Key.C;
                case (DeviceKeys.V):
                    return Key.V;
                case (DeviceKeys.B):
                    return Key.B;
                case (DeviceKeys.N):
                    return Key.N;
                case (DeviceKeys.M):
                    return Key.M;
                case (DeviceKeys.COMMA):
                    return Key.OemComma;
                case (DeviceKeys.PERIOD):
                    return Key.OemPeriod;
                case (DeviceKeys.FORWARD_SLASH):
                    return Key.OemSlash;
                case (DeviceKeys.OEM8):
                    return Key.OemSlash;
                case (DeviceKeys.RIGHT_SHIFT):
                    return Key.RightShift;
                case (DeviceKeys.ARROW_UP):
                    return Key.Up;
                case (DeviceKeys.NUM_ONE):
                    return Key.Num1;
                case (DeviceKeys.NUM_TWO):
                    return Key.Num2;
                case (DeviceKeys.NUM_THREE):
                    return Key.Num3;
                case (DeviceKeys.NUM_ENTER):
                    return Key.NumEnter;
                case (DeviceKeys.LEFT_CONTROL):
                    return Key.LeftControl;
                case (DeviceKeys.LEFT_WINDOWS):
                    return Key.LeftWindows;
                case (DeviceKeys.LEFT_ALT):
                    return Key.LeftAlt;
                case (DeviceKeys.SPACE):
                    return Key.Space;
                case (DeviceKeys.RIGHT_ALT):
                    return Key.RightAlt;
                //case (DeviceKeys.RIGHT_WINDOWS):
                //    return Key.Right;
                case (DeviceKeys.APPLICATION_SELECT):
                    return Key.RightMenu;
                case (DeviceKeys.RIGHT_CONTROL):
                    return Key.RightControl;
                case (DeviceKeys.ARROW_LEFT):
                    return Key.Left;
                case (DeviceKeys.ARROW_DOWN):
                    return Key.Down;
                case (DeviceKeys.ARROW_RIGHT):
                    return Key.Right;
                case (DeviceKeys.NUM_ZERO):
                    return Key.Num0;
                case (DeviceKeys.NUM_PERIOD):
                    return Key.NumDecimal;
                case (DeviceKeys.FN_Key):
                    return Key.Function;
                case (DeviceKeys.G1):
                    return Key.Macro1;
                case (DeviceKeys.G2):
                    return Key.Macro2;
                case (DeviceKeys.G3):
                    return Key.Macro3;
                case (DeviceKeys.G4):
                    return Key.Macro4;
                case (DeviceKeys.G5):
                    return Key.Macro5;
                case (DeviceKeys.LOGO):
                    return Key.Logo;
                default:
                    return Key.Invalid;
            }
        }

    }
    public class RazerMouseDevice : AuroraDevice
    {
        IMouse Mouse = null;
        private System.Drawing.Color previous_peripheral_Color = System.Drawing.Color.Black;
        protected override string DeviceName => "Razer Device";

        protected override AuroraDeviceType AuroraDeviceType => AuroraDeviceType.Keyboard;

        public RazerMouseDevice(IMouse mouse)
        {
            Mouse = mouse;
        }
        protected override bool UpdateDeviceImpl(DeviceColorComposition composition)
        {
            foreach (KeyValuePair<DeviceKeys, System.Drawing.Color> key in composition.keyColors)
            {

                if (key.Key == DeviceKeys.Peripheral_Logo || key.Key == DeviceKeys.Peripheral)
                {
                    SendColorToPeripheral(key.Value);
                }

            }

            return true;
        }

        private void SendColorToPeripheral(System.Drawing.Color color, bool forced = false)
        {
            if ((!previous_peripheral_Color.Equals(color)))
            {

                System.Drawing.Color c = CorrectAlpha(color);
                Mouse.SetAll(new Color(c.R, c.G, c.B));

                previous_peripheral_Color = color;

            }
        }
    }
    public class RazerDeviceDevice : AuroraDevice
    {
        Corale.Colore.Core.IDevice Device = null;
        private System.Drawing.Color previous_peripheral_Color = System.Drawing.Color.Black;
        protected override string DeviceName => "Razer Device";

        protected override AuroraDeviceType AuroraDeviceType => AuroraDeviceType.Unkown;

        public RazerDeviceDevice(Corale.Colore.Core.IDevice device)
        {
            Device = device;
        }
        protected override bool UpdateDeviceImpl(DeviceColorComposition composition)
        {
            foreach (KeyValuePair<DeviceKeys, System.Drawing.Color> key in composition.keyColors)
            {

                if (key.Key == DeviceKeys.Peripheral_Logo || key.Key == DeviceKeys.Peripheral)
                {
                    SendColorToPeripheral(key.Value);
                }

            }

            return true;
        }

        private void SendColorToPeripheral(System.Drawing.Color color, bool forced = false)
        {
            if ((!previous_peripheral_Color.Equals(color)))
            {

                System.Drawing.Color c = CorrectAlpha(color);
                Device.SetAll(new Color(c.R, c.G, c.B));

                previous_peripheral_Color = color;

            }
        }
    }
    public class RazerMousepadDevice : AuroraDevice
    {
        IMousepad Mousepad = null;
        private MousepadCustom Grid = MousepadCustom.Create();
        protected override string DeviceName => "Razer Mousepad";

        protected override AuroraDeviceType AuroraDeviceType => AuroraDeviceType.Keyboard;

        public RazerMousepadDevice(IMousepad mousepad)
        {
            Mousepad = mousepad;
        }
        protected override bool UpdateDeviceImpl(DeviceColorComposition composition)
        {
            foreach (KeyValuePair<DeviceKeys, System.Drawing.Color> key in composition.keyColors)
            {

                if (key.Key == DeviceKeys.MOUSEPADLIGHT1)
                {

                    SendColorToMousepad(14, key.Value);
                }
                else if (key.Key == DeviceKeys.MOUSEPADLIGHT2)
                {

                    SendColorToMousepad(13, key.Value);
                }
                else if (key.Key == DeviceKeys.MOUSEPADLIGHT3)
                {

                    SendColorToMousepad(12, key.Value);
                }
                else if (key.Key == DeviceKeys.MOUSEPADLIGHT4)
                {

                    SendColorToMousepad(11, key.Value);
                }
                else if (key.Key == DeviceKeys.MOUSEPADLIGHT5)
                {

                    SendColorToMousepad(10, key.Value);
                }
                else if (key.Key == DeviceKeys.MOUSEPADLIGHT6)
                {

                    SendColorToMousepad(9, key.Value);
                }
                else if (key.Key == DeviceKeys.MOUSEPADLIGHT7)
                {

                    SendColorToMousepad(8, key.Value);
                }
                else if (key.Key == DeviceKeys.MOUSEPADLIGHT8)
                {

                    SendColorToMousepad(7, key.Value);
                }
                else if (key.Key == DeviceKeys.MOUSEPADLIGHT9)
                {

                    SendColorToMousepad(6, key.Value);
                }
                else if (key.Key == DeviceKeys.MOUSEPADLIGHT10)
                {

                    SendColorToMousepad(5, key.Value);
                }
                else if (key.Key == DeviceKeys.MOUSEPADLIGHT11)
                {

                    SendColorToMousepad(4, key.Value);
                }
                else if (key.Key == DeviceKeys.MOUSEPADLIGHT12)
                {

                    SendColorToMousepad(3, key.Value);
                }
                else if (key.Key == DeviceKeys.MOUSEPADLIGHT13)
                {

                    SendColorToMousepad(2, key.Value);
                }
                else if (key.Key == DeviceKeys.MOUSEPADLIGHT14)
                {

                    SendColorToMousepad(1, key.Value);
                }
                else if (key.Key == DeviceKeys.MOUSEPADLIGHT15)
                {

                    SendColorToMousepad(0, key.Value);
                }

            }

            Mousepad.SetCustom(Grid);
            return true;
        }

        private void SendColorToMousepad(int index, System.Drawing.Color color)
        {
            System.Drawing.Color c = CorrectAlpha(color);
            Grid[index] = new Color(c.R, c.G, c.B);

        }
    }
}
