using Aurora.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using DK = Aurora.Devices.DeviceKeys;

namespace Device_CoolerMaster
{
    public class CoolerMasterDevice : Device
    {
        protected override string DeviceName => "CoolerMaster";
        private readonly List<Native.DEVICE_INDEX> InitializedDevices = new List<Native.DEVICE_INDEX>();

        public override string GetDeviceDetails()
        {
            return DeviceName + ": " + (isInitialized ?
              string.Join(" ", InitializedDevices.Select(d => Enum.GetName(typeof(Native.DEVICE_INDEX), d))) :
              "Not Initialized");
        }

        public override bool Initialize()
        {
            foreach (var device in Native.DEVICES.Where(d => d != Native.DEVICE_INDEX.DEFAULT))
            {
                if (Native.IsDevicePlug(device) && Native.EnableLedControl(true, device))
                    InitializedDevices.Add(device);
            }

            return isInitialized = InitializedDevices.Any();
        }

        public override void Shutdown()
        {
            foreach (var dev in InitializedDevices)
                Native.EnableLedControl(false, dev);

            isInitialized = false;
        }

        public override bool UpdateDevice(Dictionary<DK, Color> keyColors, DoWorkEventArgs e, bool forced = false)
        {
            foreach (var dev in InitializedDevices)
            {
                Native.COLOR_MATRIX colors = Native.NewColorMatrix();
                if (!LayoutMapping.TryGetValue(dev, out var dict))
                    dict = GenericKeyCoords;

                foreach (var pair in keyColors)
                {
                    if (dict.TryGetValue(pair.Key, out var c))
                        colors.KeyColor[c.row, c.column] = new Native.KEY_COLOR(CorrectAlpha(pair.Value));
                }

                Native.SetAllLedColor(colors, dev);
            }
            return true;
        }

        private static readonly Dictionary<DK, (int row, int column)> GenericKeyCoords = new Dictionary<DK, (int row, int column)>()
        {
            [DK.ESC] = (0, 0),
            [DK.F1] = (0, 1),
            [DK.F2] = (0, 2),
            [DK.F3] = (0, 3),
            [DK.F4] = (0, 4),
            [DK.F5] = (0, 6),
            [DK.F6] = (0, 7),
            [DK.F7] = (0, 8),
            [DK.F8] = (0, 9),
            [DK.F9] = (0, 11),
            [DK.F10] = (0, 12),
            [DK.F11] = (0, 13),
            [DK.F12] = (0, 14),
            [DK.PRINT_SCREEN] = (0, 15),
            [DK.SCROLL_LOCK] = (0, 16),
            [DK.PAUSE_BREAK] = (0, 17),
            [DK.Profile_Key1] = (0, 18),
            [DK.Profile_Key2] = (0, 19),
            [DK.Profile_Key3] = (0, 20),
            [DK.Profile_Key4] = (0, 21),
            [DK.TILDE] = (1, 0),
            [DK.ONE] = (1, 1),
            [DK.TWO] = (1, 2),
            [DK.THREE] = (1, 3),
            [DK.FOUR] = (1, 4),
            [DK.FIVE] = (1, 5),
            [DK.SIX] = (1, 6),
            [DK.SEVEN] = (1, 7),
            [DK.EIGHT] = (1, 8),
            [DK.NINE] = (1, 9),
            [DK.ZERO] = (1, 10),
            [DK.MINUS] = (1, 11),
            [DK.EQUALS] = (1, 12),
            [DK.BACKSPACE] = (1, 14),
            [DK.INSERT] = (1, 15),
            [DK.HOME] = (1, 16),
            [DK.PAGE_UP] = (1, 17),
            [DK.NUM_LOCK] = (1, 18),
            [DK.NUM_SLASH] = (1, 19),
            [DK.NUM_ASTERISK] = (1, 20),
            [DK.NUM_MINUS] = (1, 21),
            [DK.TAB] = (2, 0),
            [DK.Q] = (2, 1),
            [DK.W] = (2, 2),
            [DK.E] = (2, 3),
            [DK.R] = (2, 4),
            [DK.T] = (2, 5),
            [DK.Y] = (2, 6),
            [DK.U] = (2, 7),
            [DK.I] = (2, 8),
            [DK.O] = (2, 9),
            [DK.P] = (2, 10),
            [DK.OPEN_BRACKET] = (2, 11),
            [DK.CLOSE_BRACKET] = (2, 12),
            [DK.BACKSLASH] = (2, 14),
            [DK.DELETE] = (2, 15),
            [DK.END] = (2, 16),
            [DK.PAGE_DOWN] = (2, 17),
            [DK.NUM_SEVEN] = (2, 18),
            [DK.NUM_EIGHT] = (2, 19),
            [DK.NUM_NINE] = (2, 20),
            [DK.NUM_PLUS] = (2, 21),
            [DK.CAPS_LOCK] = (3, 0),
            [DK.A] = (3, 1),
            [DK.S] = (3, 2),
            [DK.D] = (3, 3),
            [DK.F] = (3, 4),
            [DK.G] = (3, 5),
            [DK.H] = (3, 6),
            [DK.J] = (3, 7),
            [DK.K] = (3, 8),
            [DK.L] = (3, 9),
            [DK.SEMICOLON] = (3, 10),
            [DK.APOSTROPHE] = (3, 11),
            [DK.HASHTAG] = (3, 12),
            [DK.ENTER] = (3, 14),
            [DK.NUM_FOUR] = (3, 18),
            [DK.NUM_FIVE] = (3, 19),
            [DK.NUM_SIX] = (3, 20),
            [DK.LEFT_SHIFT] = (4, 0),
            [DK.BACKSLASH_UK] = (4, 1),
            [DK.Z] = (4, 2),
            [DK.X] = (4, 3),
            [DK.C] = (4, 4),
            [DK.V] = (4, 5),
            [DK.B] = (4, 6),
            [DK.N] = (4, 7),
            [DK.M] = (4, 8),
            [DK.COMMA] = (4, 9),
            [DK.PERIOD] = (4, 10),
            [DK.FORWARD_SLASH] = (4, 11),
            [DK.RIGHT_SHIFT] = (4, 14),
            [DK.ARROW_UP] = (4, 16),
            [DK.NUM_ONE] = (4, 18),
            [DK.NUM_TWO] = (4, 19),
            [DK.NUM_THREE] = (4, 20),
            [DK.NUM_ENTER] = (4, 21),
            [DK.LEFT_CONTROL] = (5, 0),
            [DK.LEFT_WINDOWS] = (5, 1),
            [DK.LEFT_ALT] = (5, 2),
            [DK.SPACE] = (5, 6),
            [DK.RIGHT_ALT] = (5, 10),
            [DK.RIGHT_WINDOWS] = (5, 11),
            [DK.APPLICATION_SELECT] = (5, 12),
            [DK.RIGHT_CONTROL] = (5, 14),
            [DK.ARROW_LEFT] = (5, 15),
            [DK.ARROW_DOWN] = (5, 16),
            [DK.ARROW_RIGHT] = (5, 17),
            [DK.NUM_ZERO] = (5, 18),
            [DK.NUM_ZEROZERO] = (5, 19),
            [DK.NUM_PERIOD] = (5, 20)
        };

        private static readonly Dictionary<DK, (int row, int column)> GenericNavlessKeyCoords = new Dictionary<DK, (int row, int column)>
        {
            [DK.ESC] = (0, 0),
            [DK.F1] = (0, 1),
            [DK.F2] = (0, 2),
            [DK.F3] = (0, 3),
            [DK.F4] = (0, 4),
            [DK.F5] = (0, 6),
            [DK.F6] = (0, 7),
            [DK.F7] = (0, 8),
            [DK.F8] = (0, 9),
            [DK.F9] = (0, 11),
            [DK.F10] = (0, 12),
            [DK.F11] = (0, 13),
            [DK.F12] = (0, 14),
            [DK.PRINT_SCREEN] = (0, 15),
            [DK.SCROLL_LOCK] = (0, 16),
            [DK.PAUSE_BREAK] = (0, 17),
            [DK.Profile_Key1] = (0, 18),
            [DK.Profile_Key2] = (0, 19),
            [DK.Profile_Key3] = (0, 20),
            [DK.Profile_Key4] = (0, 21),
            [DK.TILDE] = (1, 0),
            [DK.ONE] = (1, 1),
            [DK.TWO] = (1, 2),
            [DK.THREE] = (1, 3),
            [DK.FOUR] = (1, 4),
            [DK.FIVE] = (1, 5),
            [DK.SIX] = (1, 6),
            [DK.SEVEN] = (1, 7),
            [DK.EIGHT] = (1, 8),
            [DK.NINE] = (1, 9),
            [DK.ZERO] = (1, 10),
            [DK.MINUS] = (1, 11),
            [DK.EQUALS] = (1, 12),
            [DK.BACKSPACE] = (1, 14),
            [DK.NUM_LOCK] = (1, 15),
            [DK.NUM_SLASH] = (1, 16),
            [DK.NUM_ASTERISK] = (1, 17),
            [DK.NUM_MINUS] = (1, 18),
            [DK.TAB] = (2, 0),
            [DK.Q] = (2, 1),
            [DK.W] = (2, 2),
            [DK.E] = (2, 3),
            [DK.R] = (2, 4),
            [DK.T] = (2, 5),
            [DK.Y] = (2, 6),
            [DK.U] = (2, 7),
            [DK.I] = (2, 8),
            [DK.O] = (2, 9),
            [DK.P] = (2, 10),
            [DK.OPEN_BRACKET] = (2, 11),
            [DK.CLOSE_BRACKET] = (2, 12),
            [DK.BACKSLASH] = (2, 14),
            [DK.NUM_SEVEN] = (2, 15),
            [DK.NUM_EIGHT] = (2, 16),
            [DK.NUM_NINE] = (2, 17),
            [DK.NUM_PLUS] = (2, 18),
            [DK.CAPS_LOCK] = (3, 0),
            [DK.A] = (3, 1),
            [DK.S] = (3, 2),
            [DK.D] = (3, 3),
            [DK.F] = (3, 4),
            [DK.G] = (3, 5),
            [DK.H] = (3, 6),
            [DK.J] = (3, 7),
            [DK.K] = (3, 8),
            [DK.L] = (3, 9),
            [DK.SEMICOLON] = (3, 10),
            [DK.APOSTROPHE] = (3, 11),
            [DK.HASHTAG] = (3, 12),
            [DK.ENTER] = (3, 14),
            [DK.NUM_FOUR] = (3, 15),
            [DK.NUM_FIVE] = (3, 16),
            [DK.NUM_SIX] = (3, 17),
            [DK.LEFT_SHIFT] = (4, 0),
            [DK.BACKSLASH_UK] = (4, 1),
            [DK.Z] = (4, 2),
            [DK.X] = (4, 3),
            [DK.C] = (4, 4),
            [DK.V] = (4, 5),
            [DK.B] = (4, 6),
            [DK.N] = (4, 7),
            [DK.M] = (4, 8),
            [DK.COMMA] = (4, 9),
            [DK.PERIOD] = (4, 10),
            [DK.FORWARD_SLASH] = (4, 11),
            [DK.RIGHT_SHIFT] = (4, 14),
            [DK.NUM_ONE] = (4, 15),
            [DK.NUM_TWO] = (4, 16),
            [DK.NUM_THREE] = (4, 17),
            [DK.NUM_ENTER] = (4, 18),
            [DK.LEFT_CONTROL] = (5, 0),
            [DK.LEFT_WINDOWS] = (5, 1),
            [DK.LEFT_ALT] = (5, 2),
            [DK.SPACE] = (5, 6),
            [DK.RIGHT_ALT] = (5, 10),
            [DK.RIGHT_WINDOWS] = (5, 11),
            [DK.APPLICATION_SELECT] = (5, 12),
            [DK.RIGHT_CONTROL] = (5, 14),
            [DK.NUM_ZERO] = (5, 15),
            [DK.NUM_ZEROZERO] = (5, 16),
            [DK.NUM_PERIOD] = (5, 17)
        };

        private static readonly Dictionary<DK, (int row, int column)> MK750Coords = new Dictionary<DK, (int row, int column)>
        {
            [DK.ESC] = (0, 0),
            [DK.F1] = (0, 1),
            [DK.F2] = (0, 2),
            [DK.F3] = (0, 3),
            [DK.F4] = (0, 4),
            [DK.F5] = (0, 6),
            [DK.F6] = (0, 7),
            [DK.F7] = (0, 8),
            [DK.F8] = (0, 9),
            [DK.F9] = (0, 11),
            [DK.F10] = (0, 12),
            [DK.F11] = (0, 13),
            [DK.F12] = (0, 14),
            [DK.PRINT_SCREEN] = (0, 15),
            [DK.SCROLL_LOCK] = (0, 16),
            [DK.PAUSE_BREAK] = (0, 17),
            [DK.VOLUME_MUTE] = (0, 18),
            [DK.MEDIA_PLAY_PAUSE] = (0, 19),
            [DK.MEDIA_PREVIOUS] = (0, 20),
            [DK.MEDIA_NEXT] = (0, 21),
            [DK.ADDITIONALLIGHT1] = (0, 22),
            [DK.ADDITIONALLIGHT23] = (0, 23),
            [DK.TILDE] = (1, 0),
            [DK.ONE] = (1, 1),
            [DK.TWO] = (1, 2),
            [DK.THREE] = (1, 3),
            [DK.FOUR] = (1, 4),
            [DK.FIVE] = (1, 5),
            [DK.SIX] = (1, 6),
            [DK.SEVEN] = (1, 7),
            [DK.EIGHT] = (1, 8),
            [DK.NINE] = (1, 9),
            [DK.ZERO] = (1, 10),
            [DK.MINUS] = (1, 11),
            [DK.EQUALS] = (1, 12),
            [DK.BACKSPACE] = (1, 14),
            [DK.INSERT] = (1, 15),
            [DK.HOME] = (1, 16),
            [DK.PAGE_UP] = (1, 17),
            [DK.NUM_LOCK] = (1, 18),
            [DK.NUM_SLASH] = (1, 19),
            [DK.NUM_ASTERISK] = (1, 20),
            [DK.NUM_MINUS] = (1, 21),
            [DK.ADDITIONALLIGHT2] = (1, 22),
            [DK.ADDITIONALLIGHT24] = (1, 23),
            [DK.TAB] = (2, 0),
            [DK.Q] = (2, 1),
            [DK.W] = (2, 2),
            [DK.E] = (2, 3),
            [DK.R] = (2, 4),
            [DK.T] = (2, 5),
            [DK.Y] = (2, 6),
            [DK.U] = (2, 7),
            [DK.I] = (2, 8),
            [DK.O] = (2, 9),
            [DK.P] = (2, 10),
            [DK.OPEN_BRACKET] = (2, 11),
            [DK.CLOSE_BRACKET] = (2, 12),
            [DK.BACKSLASH] = (2, 14),
            [DK.DELETE] = (2, 15),
            [DK.END] = (2, 16),
            [DK.PAGE_DOWN] = (2, 17),
            [DK.NUM_SEVEN] = (2, 18),
            [DK.NUM_EIGHT] = (2, 19),
            [DK.NUM_NINE] = (2, 20),
            [DK.NUM_PLUS] = (2, 21),
            [DK.ADDITIONALLIGHT3] = (2, 22),
            [DK.ADDITIONALLIGHT25] = (2, 23),
            [DK.CAPS_LOCK] = (3, 0),
            [DK.A] = (3, 1),
            [DK.S] = (3, 2),
            [DK.D] = (3, 3),
            [DK.F] = (3, 4),
            [DK.G] = (3, 5),
            [DK.H] = (3, 6),
            [DK.J] = (3, 7),
            [DK.K] = (3, 8),
            [DK.L] = (3, 9),
            [DK.SEMICOLON] = (3, 10),
            [DK.APOSTROPHE] = (3, 11),
            [DK.HASHTAG] = (3, 12),
            [DK.ENTER] = (3, 14),
            [DK.NUM_FOUR] = (3, 18),
            [DK.NUM_FIVE] = (3, 19),
            [DK.NUM_SIX] = (3, 20),
            [DK.ADDITIONALLIGHT4] = (3, 22),
            [DK.ADDITIONALLIGHT26] = (3, 23),
            [DK.LEFT_SHIFT] = (4, 0),
            [DK.BACKSLASH_UK] = (4, 1),
            [DK.Z] = (4, 2),
            [DK.X] = (4, 3),
            [DK.C] = (4, 4),
            [DK.V] = (4, 5),
            [DK.B] = (4, 6),
            [DK.N] = (4, 7),
            [DK.M] = (4, 8),
            [DK.COMMA] = (4, 9),
            [DK.PERIOD] = (4, 10),
            [DK.FORWARD_SLASH] = (4, 11),
            [DK.RIGHT_SHIFT] = (4, 14),
            [DK.ARROW_UP] = (4, 16),
            [DK.NUM_ONE] = (4, 18),
            [DK.NUM_TWO] = (4, 19),
            [DK.NUM_THREE] = (4, 20),
            [DK.NUM_ENTER] = (4, 21),
            [DK.LEFT_CONTROL] = (5, 0),
            [DK.LEFT_WINDOWS] = (5, 1),
            [DK.LEFT_ALT] = (5, 2),
            [DK.SPACE] = (5, 6),
            [DK.RIGHT_ALT] = (5, 10),
            [DK.RIGHT_WINDOWS] = (5, 11),
            [DK.FN_Key] = (5, 12),
            [DK.RIGHT_CONTROL] = (5, 14),
            [DK.ARROW_LEFT] = (5, 15),
            [DK.ARROW_DOWN] = (5, 16),
            [DK.ARROW_RIGHT] = (5, 17),
            [DK.NUM_ZERO] = (5, 18),
            [DK.NUM_PERIOD] = (5, 20),
            [DK.ADDITIONALLIGHT5] = (6, 0),
            [DK.ADDITIONALLIGHT6] = (6, 1),
            [DK.ADDITIONALLIGHT7] = (6, 2),
            [DK.ADDITIONALLIGHT8] = (6, 3),
            [DK.ADDITIONALLIGHT9] = (6, 4),
            [DK.ADDITIONALLIGHT10] = (6, 5),
            [DK.ADDITIONALLIGHT11] = (6, 6),
            [DK.ADDITIONALLIGHT12] = (6, 7),
            [DK.ADDITIONALLIGHT13] = (6, 8),
            [DK.ADDITIONALLIGHT14] = (6, 9),
            [DK.ADDITIONALLIGHT15] = (6, 10),
            [DK.ADDITIONALLIGHT16] = (6, 11),
            [DK.ADDITIONALLIGHT17] = (6, 12),
            [DK.ADDITIONALLIGHT18] = (6, 13),
            [DK.ADDITIONALLIGHT19] = (6, 14),
            [DK.ADDITIONALLIGHT20] = (6, 15),
            [DK.ADDITIONALLIGHT21] = (6, 16),
            [DK.ADDITIONALLIGHT22] = (6, 17)
        };

        private static readonly Dictionary<Native.DEVICE_INDEX, Dictionary<DK, (int row, int column)>> LayoutMapping = new Dictionary<Native.DEVICE_INDEX, Dictionary<DK, (int row, int column)>>
        {
            [Native.DEVICE_INDEX.MKeys_L] = GenericKeyCoords,
            [Native.DEVICE_INDEX.MKeys_L_White] = GenericKeyCoords,
            [Native.DEVICE_INDEX.MKeys_M] = GenericNavlessKeyCoords,
            [Native.DEVICE_INDEX.MKeys_M_White] = GenericNavlessKeyCoords,
            [Native.DEVICE_INDEX.MKeys_S] = GenericKeyCoords,
            [Native.DEVICE_INDEX.MKeys_S_White] = GenericKeyCoords,
            [Native.DEVICE_INDEX.MK750] = MK750Coords,
            [Native.DEVICE_INDEX.CK372] = GenericKeyCoords,
            [Native.DEVICE_INDEX.CK550_552] = GenericKeyCoords,
            [Native.DEVICE_INDEX.CK551] = GenericKeyCoords,
            [Native.DEVICE_INDEX.CK530] = GenericKeyCoords,
            //TODO
            [Native.DEVICE_INDEX.MMouse_L] = new Dictionary<DK, (int row, int column)>()
            {
                [DK.Peripheral] = (0, 0),
                [DK.Peripheral_FrontLight] = (0, 1),
                [DK.Peripheral_Logo] = (0, 2),
                [DK.Peripheral_ScrollWheel] = (0, 3)
            },
            [Native.DEVICE_INDEX.MMouse_S] = new Dictionary<DK, (int row, int column)>()
            {
                [DK.Peripheral_Logo] = (0, 0),
                [DK.Peripheral_ScrollWheel] = (0, 1)
            },
            [Native.DEVICE_INDEX.MM520] = new Dictionary<DK, (int row, int column)>()
            {
                [DK.Peripheral_Logo] = (0, 0),
                [DK.Peripheral_ScrollWheel] = (0, 1),
                [DK.Peripheral_FrontLight] = (0, 2),
            },
            [Native.DEVICE_INDEX.MM530] = new Dictionary<DK, (int row, int column)>() 
            {
                [DK.Peripheral_Logo] = (0, 0),
                [DK.Peripheral_ScrollWheel] = (0, 1),
                [DK.Peripheral_FrontLight] = (0, 2),
            },
            [Native.DEVICE_INDEX.MM830] = new Dictionary<DK, (int row, int column)>()
        };
    }
}
