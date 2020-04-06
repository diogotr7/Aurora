using Aurora.Settings;
using Aurora.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SBAuroraReactive;
using Aurora.Devices;

namespace Device_Creative
{
    public class CreativeConnector : AuroraDeviceConnector
    {
        private LEDManager sbScanner;
        protected override string ConnectorName => "Creative";

        protected override bool InitializeImpl()
        {
            if (sbScanner == null)
                sbScanner = new LEDManager();

            EnumeratedDevice[] devicesArr = sbScanner.EnumConnectedDevices();

            int deviceIdx;
            for (deviceIdx = 0; deviceIdx < devicesArr.Length; deviceIdx++)
            {
                if (devicesArr[deviceIdx].deviceId.Equals(EnumeratedDevice.SoundBlasterXVanguardK08_USEnglish) ||
                    devicesArr[deviceIdx].deviceId.Equals(EnumeratedDevice.SoundBlasterXVanguardK08_German) ||
                    devicesArr[deviceIdx].deviceId.Equals(EnumeratedDevice.SoundBlasterXVanguardK08_Nordic))
                {
                    devices.Add(new SoundBlasterKeyboard(devicesArr[deviceIdx]));
                }
                if (devicesArr[deviceIdx].deviceId.Equals(EnumeratedDevice.SoundBlasterXSiegeM04))
                {
                    devices.Add(new SoundBlasterMouse(devicesArr[deviceIdx]));
                }
            }
            
            return true;
        }

        protected override void ShutdownImpl()
        {
            sbScanner.Dispose();
            sbScanner = null;
        }
    }
    abstract class SoundBlasterXDevice : AuroraDevice
    {
        protected LEDManager Device;
        protected EnumeratedDevice DeviceInfo;
        protected LedSettings DeviceSettings;
        private string deviceName = "";
        protected override string DeviceName => deviceName;
        public SoundBlasterXDevice(EnumeratedDevice device)
        {
            DeviceInfo = device;

        }
        protected override bool ConnectImpl()
        {
            LEDManager newDevice = null;
            deviceName = DeviceInfo.friendlyName;
            try
            {
                newDevice = new LEDManager();
                newDevice.OpenDevice(DeviceInfo, false);
                Device = newDevice;
                newDevice = null;
                return true;
            }
            catch (Exception exc)
            {
                LogError("There was an error opening " + DeviceName + ".\r\n" + exc.Message);
            }
            finally
            {
                if (newDevice != null)
                {
                    newDevice.Dispose();
                    newDevice = null;
                }
            }
            return false;
        }
        protected override void DisconnectImpl()
        {
            if (Device != null)
            {
                if (DeviceSettings != null && DeviceSettings.payloadData.HasValue && DeviceSettings.payloadData.Value.opaqueSize > 0)
                {
                    try
                    {
                        DeviceSettings.payloadData = Device.LedPayloadCleanup(DeviceSettings.payloadData.Value, DeviceInfo.totalNumLeds);
                    }
                    catch (Exception exc)
                    {
                        LogError("There was an error freeing " + DeviceName + ".\r\n" + exc.Message);
                    }
                }
                DeviceSettings = null;
                try
                {
                    Device.CloseDevice();
                    Device.Dispose();
                    Device = null;
                }
                catch (Exception exc)
                {
                    LogError("There was an error closing " + DeviceName + ".\r\n" + exc.Message);
                }
                finally
                {
                    if (Device != null)
                    {
                        Device.Dispose();
                        Device = null;
                    }
                }
            }
        }
    }
    class SoundBlasterKeyboard : SoundBlasterXDevice
    {


        protected override AuroraDeviceType AuroraDeviceType => AuroraDeviceType.Keyboard;

        public SoundBlasterKeyboard(EnumeratedDevice device) : base(device)
        {
            KeyboardMapping_US = KeyboardMapping_All.Where(x => (x.Key != Keyboard_LEDIndex.NonUS57 && x.Key != Keyboard_LEDIndex.NonUS61)).ToArray();
            KeyboardMapping_European = KeyboardMapping_All.Where(x => (x.Key != Keyboard_LEDIndex.BackSlash)).ToArray();
        }
        protected override bool UpdateDeviceImpl(DeviceColorComposition composition)
        {
            uint maxKbLength = 0;
            Dictionary<Color, List<Keyboard_LEDIndex>> kbIndices = null;
            if (Device != null)
                kbIndices = new Dictionary<Color, List<Keyboard_LEDIndex>>();

            foreach (KeyValuePair<DeviceKeys, Color> kv in composition.keyColors)
            {
                if (kbIndices != null)
                {
                    var kbLedIdx = GetKeyboardMappingLedIndex(kv.Key);
                    if (kbLedIdx != Keyboard_LEDIndex.NotApplicable)
                    {
                        if (!kbIndices.ContainsKey(kv.Value))
                            kbIndices[kv.Value] = new List<Keyboard_LEDIndex>(1);

                        var list = kbIndices[kv.Value];
                        list.Add(kbLedIdx);
                        if (list.Count > maxKbLength)
                            maxKbLength = (uint)list.Count;
                    }
                }
            }

            uint numKbGroups = 0;
            uint[] kbGroupsArr = null;
            LedPattern[] kbPatterns = null;
            LedColour[] kbColors = null;
            if (kbIndices != null)
            {
                numKbGroups = (uint)kbIndices.Count;
                kbGroupsArr = new uint[numKbGroups * (maxKbLength + 1)];
                kbPatterns = new LedPattern[numKbGroups];
                kbColors = new LedColour[numKbGroups];
                uint currGroup = 0;
                foreach (var kv in kbIndices)
                {

                    kbPatterns[currGroup] = LedPattern.Static;
                    kbColors[currGroup].a = kv.Key.A;
                    kbColors[currGroup].r = kv.Key.R;
                    kbColors[currGroup].g = kv.Key.G;
                    kbColors[currGroup].b = kv.Key.B;
                    uint i = currGroup * (maxKbLength + 1);
                    kbGroupsArr[i++] = (uint)kv.Value.Count;
                    foreach (Keyboard_LEDIndex idx in kv.Value)
                        kbGroupsArr[i++] = (uint)idx;

                    currGroup++;
                }
                kbIndices = null;
            }

            if (Device != null && numKbGroups > 0)
            {
                try
                {
                    if (DeviceSettings == null)
                    {
                        DeviceSettings = new LedSettings();
                        DeviceSettings.persistentInDevice = false;
                        DeviceSettings.globalPatternMode = false;
                        DeviceSettings.pattern = LedPattern.Static;
                        DeviceSettings.payloadData = new LedPayloadData();
                    }

                    DeviceSettings.payloadData = Device.LedPayloadInitialize(DeviceSettings.payloadData.Value, numKbGroups, maxKbLength, 1);
                    DeviceSettings.payloadData = Device.LedPayloadFillupAll(DeviceSettings.payloadData.Value, numKbGroups, kbPatterns, maxKbLength + 1, kbGroupsArr, 1, 1, kbColors);
                    Device.SetLedSettings(DeviceSettings);
                }
                catch (Exception exc)
                {
                    LogError("Failed to Update Device " + DeviceName + ": " + exc.ToString());
                    return false;
                }
                finally
                {
                    if (DeviceSettings != null && DeviceSettings.payloadData.HasValue && DeviceSettings.payloadData.Value.opaqueSize > 0)
                        DeviceSettings.payloadData = Device.LedPayloadCleanup(DeviceSettings.payloadData.Value, numKbGroups);
                }
                
             
            }

            return true;
        }
        public Keyboard_LEDIndex GetKeyboardMappingLedIndex(DeviceKeys devKey)
        {
            var mapping = DeviceInfo.deviceId.Equals(EnumeratedDevice.SoundBlasterXVanguardK08_USEnglish) ? KeyboardMapping_US : KeyboardMapping_European;
            for (int i = 0; i < mapping.Length; i++)
            {
                if (mapping[i].Value.Equals(devKey))
                    return mapping[i].Key;
            }

            return Keyboard_LEDIndex.NotApplicable;
        }

        static KeyValuePair<Keyboard_LEDIndex, DeviceKeys>[] KeyboardMapping_All = {
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Esc, DeviceKeys.ESC),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.F1, DeviceKeys.F1),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.F2, DeviceKeys.F2),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.F3, DeviceKeys.F3),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.F4, DeviceKeys.F4),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.F5, DeviceKeys.F5),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.F6, DeviceKeys.F6),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.F7, DeviceKeys.F7),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.F8, DeviceKeys.F8),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.F9, DeviceKeys.F9),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.F10, DeviceKeys.F10),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.F11, DeviceKeys.F11),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.F12, DeviceKeys.F12),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.M1, DeviceKeys.G1),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.BackQuote, DeviceKeys.TILDE),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Digit1, DeviceKeys.ONE),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Digit2, DeviceKeys.TWO),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Digit3, DeviceKeys.THREE),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Digit4, DeviceKeys.FOUR),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Digit5, DeviceKeys.FIVE),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Digit6, DeviceKeys.SIX),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Digit7, DeviceKeys.SEVEN),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Digit8, DeviceKeys.EIGHT),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Digit9, DeviceKeys.NINE),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Digit0, DeviceKeys.ZERO),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Minus, DeviceKeys.MINUS),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Equal, DeviceKeys.EQUALS),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Backspace, DeviceKeys.BACKSPACE),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.M2, DeviceKeys.G2),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Tab, DeviceKeys.TAB),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Q, DeviceKeys.Q),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.W, DeviceKeys.W),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.E, DeviceKeys.E),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.R, DeviceKeys.R),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.T, DeviceKeys.T),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Y, DeviceKeys.Y),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.U, DeviceKeys.U),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.I, DeviceKeys.I),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.O, DeviceKeys.O),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.P, DeviceKeys.P),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.OpenBracket, DeviceKeys.OPEN_BRACKET),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.ClosedBracket, DeviceKeys.CLOSE_BRACKET),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.BackSlash, DeviceKeys.BACKSLASH),         //Only on US
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.M3, DeviceKeys.G3),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.CapsLock, DeviceKeys.CAPS_LOCK),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.A, DeviceKeys.A),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.S, DeviceKeys.S),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.D, DeviceKeys.D),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.F, DeviceKeys.F),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.G, DeviceKeys.G),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.H, DeviceKeys.H),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.J, DeviceKeys.J),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.K, DeviceKeys.K),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.L, DeviceKeys.L),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Semicolon, DeviceKeys.SEMICOLON),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Apostrophe, DeviceKeys.APOSTROPHE),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.NonUS57, DeviceKeys.HASHTAG),             //Only on European
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Enter, DeviceKeys.ENTER),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.M4, DeviceKeys.G4),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.LeftShift, DeviceKeys.LEFT_SHIFT),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.NonUS61, DeviceKeys.BACKSLASH_UK),        //Only on European
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Z, DeviceKeys.Z),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.X, DeviceKeys.X),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.C, DeviceKeys.C),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.V, DeviceKeys.V),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.B, DeviceKeys.B),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.N, DeviceKeys.N),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.M, DeviceKeys.M),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Comma, DeviceKeys.COMMA),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Fullstop, DeviceKeys.PERIOD),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.ForwardSlash, DeviceKeys.FORWARD_SLASH),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.RightShift, DeviceKeys.RIGHT_SHIFT),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.M5, DeviceKeys.G5),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.LeftCtrl, DeviceKeys.LEFT_CONTROL),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.LeftWindows, DeviceKeys.LEFT_WINDOWS),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.LeftAlt, DeviceKeys.LEFT_ALT),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Space, DeviceKeys.SPACE),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.RightAlt, DeviceKeys.RIGHT_ALT),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Fn, DeviceKeys.FN_Key),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Menu, DeviceKeys.APPLICATION_SELECT),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.RightCtrl, DeviceKeys.RIGHT_CONTROL),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.PadMinus, DeviceKeys.NUM_MINUS),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.PadAsterisk, DeviceKeys.NUM_ASTERISK),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.PadForwardSlash, DeviceKeys.NUM_SLASH),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.PadNumLock, DeviceKeys.NUM_LOCK),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.PageUp, DeviceKeys.PAGE_UP),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Home, DeviceKeys.HOME),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Insert, DeviceKeys.INSERT),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.PadPlus, DeviceKeys.NUM_PLUS),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Pad9, DeviceKeys.NUM_NINE),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Pad8, DeviceKeys.NUM_EIGHT),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Pad7, DeviceKeys.NUM_SEVEN),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.PageDown, DeviceKeys.PAGE_DOWN),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.End, DeviceKeys.END),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Delete, DeviceKeys.DELETE),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.PrintScreen, DeviceKeys.PRINT_SCREEN),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Pad6, DeviceKeys.NUM_SIX),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Pad5, DeviceKeys.NUM_FIVE),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Pad4, DeviceKeys.NUM_FOUR),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Pad1, DeviceKeys.NUM_ONE),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.UpArrow, DeviceKeys.ARROW_UP),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.LeftArrow, DeviceKeys.ARROW_LEFT),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.ScrollLock, DeviceKeys.SCROLL_LOCK),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.PadEnter, DeviceKeys.NUM_ENTER),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Pad3, DeviceKeys.NUM_THREE),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Pad2, DeviceKeys.NUM_TWO),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.PadFullstop, DeviceKeys.NUM_PERIOD),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Pad0, DeviceKeys.NUM_ZERO),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.RightArrow, DeviceKeys.ARROW_RIGHT),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.DownArrow, DeviceKeys.ARROW_DOWN),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Pause, DeviceKeys.PAUSE_BREAK),
            new KeyValuePair<Keyboard_LEDIndex, DeviceKeys>(Keyboard_LEDIndex.Logo, DeviceKeys.LOGO)
        };

        public readonly KeyValuePair<Keyboard_LEDIndex, DeviceKeys>[] KeyboardMapping_US;
        public readonly KeyValuePair<Keyboard_LEDIndex, DeviceKeys>[] KeyboardMapping_European;


    }
    class SoundBlasterMouse : SoundBlasterXDevice
    {

        protected override AuroraDeviceType AuroraDeviceType => AuroraDeviceType.Mouse;

        public SoundBlasterMouse(EnumeratedDevice device) : base(device) { }
        
        protected override bool UpdateDeviceImpl(DeviceColorComposition composition)
        {
            LedColour[] mouseColors = null;
            foreach (KeyValuePair<DeviceKeys, Color> kv in composition.keyColors)
            {
                if (Device != null)
                {
                    int moosIdx = GetMouseMappingIndex(kv.Key);
                    if (moosIdx >= 0 && moosIdx <= MouseMapping.Length)
                    {
                        if (mouseColors == null)
                            mouseColors = new LedColour[MouseMapping.Length];

                        mouseColors[moosIdx].a = kv.Value.A;
                        mouseColors[moosIdx].r = kv.Value.R;
                        mouseColors[moosIdx].g = kv.Value.G;
                        mouseColors[moosIdx].b = kv.Value.B;
                    }
                }
            }

            if (Device != null && mouseColors != null)
            {
                if (DeviceSettings == null)
                {
                    DeviceSettings = new LedSettings();
                    DeviceSettings.persistentInDevice = false;
                    DeviceSettings.globalPatternMode = false;
                    DeviceSettings.pattern = LedPattern.Static;
                    DeviceSettings.payloadData = new LedPayloadData();
                }

                if (DeviceSettings.payloadData.Value.opaqueSize == 0)
                {
                    var mousePatterns = new LedPattern[mouseColors.Length];
                    var mouseGroups = new uint[MouseMapping.Length * 2];
                    for (int i = 0; i < MouseMapping.Length; i++)
                    {
                        mouseGroups[(i * 2) + 0] = 1;                           //1 LED in group
                        mouseGroups[(i * 2) + 1] = (uint)MouseMapping[i].Key;   //Which LED it is
                        mousePatterns[i] = LedPattern.Static;               //LED has a host-controlled static color
                    }

                    try
                    {
                        DeviceSettings.payloadData = Device.LedPayloadInitialize(DeviceSettings.payloadData.Value, DeviceInfo.totalNumLeds, 1, 1);
                        DeviceSettings.payloadData = Device.LedPayloadFillupAll(DeviceSettings.payloadData.Value, (uint)mouseColors.Length, mousePatterns, 2, mouseGroups, 1, 1, mouseColors);
                    }
                    catch (Exception exc)
                    {
                        LogError("Failed to setup data for " + DeviceName + ": " + exc.ToString());
                        if (DeviceSettings.payloadData.Value.opaqueSize > 0)
                            DeviceSettings.payloadData = Device.LedPayloadCleanup(DeviceSettings.payloadData.Value, DeviceInfo.totalNumLeds);

                        return false;
                    }
                }
                else
                {
                    try
                    {
                        for (int i = 0; i < mouseColors.Length; i++)
                            DeviceSettings.payloadData = Device.LedPayloadFillupLedColour(DeviceSettings.payloadData.Value, (uint)i, 1, mouseColors[i], false);
                    }
                    catch (Exception exc)
                    {
                        LogError("Failed to fill color data for " + DeviceName + ": " + exc.ToString());
                        return false;
                    }
                }

                try
                {
                    Device.SetLedSettings(DeviceSettings);
                }
                catch (Exception exc)
                {
                    LogError("Failed to Update Device " + DeviceName + ": " + exc.ToString());
                    return false;
                }
            }

            return true;
        }
        public static int GetMouseMappingIndex(DeviceKeys devKey)
        {
            int i;
            for (i = 0; i < MouseMapping.Length; i++)
                if (MouseMapping[i].Value.Equals(devKey))
                    break;

            return i;
        }

        public static readonly KeyValuePair<Mouse_LEDIndex, DeviceKeys>[] MouseMapping = {
            new KeyValuePair<Mouse_LEDIndex, DeviceKeys>(Mouse_LEDIndex.LED0, DeviceKeys.MOUSEPADLIGHT1),
            new KeyValuePair<Mouse_LEDIndex, DeviceKeys>(Mouse_LEDIndex.LED1, DeviceKeys.MOUSEPADLIGHT2),
            new KeyValuePair<Mouse_LEDIndex, DeviceKeys>(Mouse_LEDIndex.LED2, DeviceKeys.MOUSEPADLIGHT3),
            new KeyValuePair<Mouse_LEDIndex, DeviceKeys>(Mouse_LEDIndex.LED3, DeviceKeys.MOUSEPADLIGHT4),
            new KeyValuePair<Mouse_LEDIndex, DeviceKeys>(Mouse_LEDIndex.LED4, DeviceKeys.MOUSEPADLIGHT5),
            new KeyValuePair<Mouse_LEDIndex, DeviceKeys>(Mouse_LEDIndex.LED5, DeviceKeys.MOUSEPADLIGHT6),
            new KeyValuePair<Mouse_LEDIndex, DeviceKeys>(Mouse_LEDIndex.LED6, DeviceKeys.MOUSEPADLIGHT7),
            new KeyValuePair<Mouse_LEDIndex, DeviceKeys>(Mouse_LEDIndex.LED7, DeviceKeys.MOUSEPADLIGHT8),
            new KeyValuePair<Mouse_LEDIndex, DeviceKeys>(Mouse_LEDIndex.LED8, DeviceKeys.MOUSEPADLIGHT9),
            new KeyValuePair<Mouse_LEDIndex, DeviceKeys>(Mouse_LEDIndex.LED9, DeviceKeys.MOUSEPADLIGHT10),
            new KeyValuePair<Mouse_LEDIndex, DeviceKeys>(Mouse_LEDIndex.LED10, DeviceKeys.MOUSEPADLIGHT11),
            new KeyValuePair<Mouse_LEDIndex, DeviceKeys>(Mouse_LEDIndex.Logo, DeviceKeys.Peripheral_Logo),
            new KeyValuePair<Mouse_LEDIndex, DeviceKeys>(Mouse_LEDIndex.Wheel, DeviceKeys.Peripheral_ScrollWheel)
        };


    }
}
