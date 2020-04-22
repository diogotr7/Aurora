using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Devices;

namespace Device_SteelSeries
{
    public partial class SteelSeriesDevice
    {
        public bool TryGetHid(DeviceKeys key, out byte hid)
        {
            return SteelSeriesHidCodes.TryGetValue(key, out hid);
        }

        private static Dictionary<DeviceKeys, byte> SteelSeriesHidCodes = new Dictionary<DeviceKeys, byte>
        {
            { DeviceKeys.LOGO,          0x00 },  { DeviceKeys.A,                  0x04 },  { DeviceKeys.B,                     0x05 },  { DeviceKeys.C,            0x06 },  { DeviceKeys.D,             0x07 },
            { DeviceKeys.E,             0x08 },  { DeviceKeys.F,                  0x09 },  { DeviceKeys.G,                     0x0A },  { DeviceKeys.H,            0x0B },  { DeviceKeys.I,             0x0C },
            { DeviceKeys.J,             0x0D },  { DeviceKeys.K,                  0x0E },  { DeviceKeys.L,                     0x0F },  { DeviceKeys.M,            0x10 },  { DeviceKeys.N,             0x11 },
            { DeviceKeys.O,             0x12 },  { DeviceKeys.P,                  0x13 },  { DeviceKeys.Q,                     0x14 },  { DeviceKeys.R,            0x15 },  { DeviceKeys.S,             0x16 },
            { DeviceKeys.T,             0x17 },  { DeviceKeys.U,                  0x18 },  { DeviceKeys.V,                     0x19 },  { DeviceKeys.W,            0x1A },  { DeviceKeys.X,             0x1B },
            { DeviceKeys.Y,             0x1C },  { DeviceKeys.Z,                  0x1D },  { DeviceKeys.ONE,                   0x1E },  { DeviceKeys.TWO,          0x1F },  { DeviceKeys.THREE,         0x20 },
            { DeviceKeys.FOUR,          0x21 },  { DeviceKeys.FIVE,               0x22 },  { DeviceKeys.SIX,                   0x23 },  { DeviceKeys.SEVEN,        0x24 },  { DeviceKeys.EIGHT,         0x25 },
            { DeviceKeys.NINE,          0x26 },  { DeviceKeys.ZERO,               0x27 },  { DeviceKeys.ENTER,                 0x28 },  { DeviceKeys.ESC,          0x29 },  { DeviceKeys.BACKSPACE,     0x2A },
            { DeviceKeys.TAB,           0x2B },  { DeviceKeys.SPACE,              0x2C },  { DeviceKeys.MINUS,                 0x2D },  { DeviceKeys.EQUALS,       0x2E },  { DeviceKeys.OPEN_BRACKET,  0x2F },
            { DeviceKeys.CLOSE_BRACKET, 0x30 },  { DeviceKeys.BACKSLASH,          0x31 },  { DeviceKeys.HASHTAG,               0x32 },  { DeviceKeys.SEMICOLON,    0x33 },  { DeviceKeys.APOSTROPHE,    0x34 },
            { DeviceKeys.TILDE,         0x35 },  { DeviceKeys.JPN_HALFFULLWIDTH,  0x35 },  { DeviceKeys.OEM5,                  0x35 },  { DeviceKeys.COMMA,        0x36 },  { DeviceKeys.PERIOD,        0x37 },
            { DeviceKeys.FORWARD_SLASH, 0x38 },  { DeviceKeys.OEM8,               0x38 },  { DeviceKeys.CAPS_LOCK,             0x39 },  { DeviceKeys.F1,           0x3A },  { DeviceKeys.F2,            0x3B },
            { DeviceKeys.F3,            0x3C },  { DeviceKeys.F4,                 0x3D },  { DeviceKeys.F5,                    0x3E },  { DeviceKeys.F6,           0x3F },  { DeviceKeys.F7,            0x40 },
            { DeviceKeys.F8,            0x41 },  { DeviceKeys.F9,                 0x42 },  { DeviceKeys.F10,                   0x43 },  { DeviceKeys.F11,          0x44 },  { DeviceKeys.F12,           0x45 },
            { DeviceKeys.PRINT_SCREEN,  0x46 },  { DeviceKeys.SCROLL_LOCK,        0x47 },  { DeviceKeys.PAUSE_BREAK,           0x48 },  { DeviceKeys.INSERT,       0x49 },  { DeviceKeys.HOME,          0x4A },
            { DeviceKeys.PAGE_UP,       0x4B },  { DeviceKeys.DELETE,             0x4C },  { DeviceKeys.END,                   0x4D },  { DeviceKeys.PAGE_DOWN,    0x4E },  { DeviceKeys.ARROW_RIGHT,   0x4F },
            { DeviceKeys.ARROW_LEFT,    0x50 },  { DeviceKeys.ARROW_DOWN,         0x51 },  { DeviceKeys.ARROW_UP,              0x52 },  { DeviceKeys.NUM_LOCK,     0x53 },  { DeviceKeys.NUM_SLASH,     0x54 },
            { DeviceKeys.NUM_ASTERISK,  0x55 },  { DeviceKeys.NUM_MINUS,          0x56 },  { DeviceKeys.NUM_PLUS,              0x57 },  { DeviceKeys.NUM_ENTER,    0x58 },  { DeviceKeys.NUM_ONE,       0x59 },
            { DeviceKeys.NUM_TWO,       0x5A },  { DeviceKeys.NUM_THREE,          0x5B },  { DeviceKeys.NUM_FOUR,              0x5C },  { DeviceKeys.NUM_FIVE,     0x5D },  { DeviceKeys.NUM_SIX,       0x5E },
            { DeviceKeys.NUM_SEVEN,     0x5F },  { DeviceKeys.NUM_EIGHT,          0x60 },  { DeviceKeys.NUM_NINE,              0x61 },  { DeviceKeys.NUM_ZERO,     0x62 },  { DeviceKeys.NUM_PERIOD,    0x63 },
            { DeviceKeys.BACKSLASH_UK,  0x64 },  { DeviceKeys.APPLICATION_SELECT, 0x65 },  { DeviceKeys.JPN_HIRAGANA_KATAKANA, 0x88 },  { DeviceKeys.JPN_HENKAN,   0x8A },  { DeviceKeys.JPN_MUHENKAN,  0x8B },
            { DeviceKeys.LEFT_CONTROL,  0xE0 },  { DeviceKeys.LEFT_SHIFT,         0xE1 },  { DeviceKeys.LEFT_ALT,              0xE2 },  { DeviceKeys.LEFT_WINDOWS, 0xE3 },  { DeviceKeys.RIGHT_CONTROL, 0xE4 },
            { DeviceKeys.RIGHT_SHIFT,   0xE5 },  { DeviceKeys.RIGHT_ALT,          0xE6 },  { DeviceKeys.RIGHT_WINDOWS,         0xE7 },  { DeviceKeys.G0,           0xE8 },  { DeviceKeys.G1,            0xE9 },
            { DeviceKeys.G2,            0xEA },  { DeviceKeys.G3,                 0xEB },  { DeviceKeys.G4,                    0xEC },  { DeviceKeys.G5,           0xED },  { DeviceKeys.FN_Key,        0xEF }
        };
    }
}