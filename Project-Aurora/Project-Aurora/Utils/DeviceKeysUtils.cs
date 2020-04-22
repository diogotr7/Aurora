using Aurora.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Utils
{
    public static class DeviceKeysUtils
    {
        public static class Logitech
        {
            public enum keyboardNames
            {
                ESC = 0x01,
                F1 = 0x3b,
                F2 = 0x3c,
                F3 = 0x3d,
                F4 = 0x3e,
                F5 = 0x3f,
                F6 = 0x40,
                F7 = 0x41,
                F8 = 0x42,
                F9 = 0x43,
                F10 = 0x44,
                F11 = 0x57,
                F12 = 0x58,
                PRINT_SCREEN = 0x137,
                SCROLL_LOCK = 0x46,
                PAUSE_BREAK = 0x45,
                TILDE = 0x29,
                ONE = 0x02,
                TWO = 0x03,
                THREE = 0x04,
                FOUR = 0x05,
                FIVE = 0x06,
                SIX = 0x07,
                SEVEN = 0x08,
                EIGHT = 0x09,
                NINE = 0x0A,
                ZERO = 0x0B,
                MINUS = 0x0C,
                EQUALS = 0x0D,
                BACKSPACE = 0x0E,
                INSERT = 0x152,
                HOME = 0x147,
                PAGE_UP = 0x149,
                NUM_LOCK = 0x145,
                NUM_SLASH = 0x135,
                NUM_ASTERISK = 0x37,
                NUM_MINUS = 0x4A,
                TAB = 0x0F,
                Q = 0x10,
                W = 0x11,
                E = 0x12,
                R = 0x13,
                T = 0x14,
                Y = 0x15,
                U = 0x16,
                I = 0x17,
                O = 0x18,
                P = 0x19,
                OPEN_BRACKET = 0x1A,
                CLOSE_BRACKET = 0x1B,
                BACKSLASH = 0x2B,
                KEYBOARD_DELETE = 0x153,
                END = 0x14F,
                PAGE_DOWN = 0x151,
                NUM_SEVEN = 0x47,
                NUM_EIGHT = 0x48,
                NUM_NINE = 0x49,
                NUM_PLUS = 0x4E,
                CAPS_LOCK = 0x3A,
                A = 0x1E,
                S = 0x1F,
                D = 0x20,
                F = 0x21,
                G = 0x22,
                H = 0x23,
                J = 0x24,
                K = 0x25,
                L = 0x26,
                SEMICOLON = 0x27,
                APOSTROPHE = 0x28,
                ENTER = 0x1C,
                NUM_FOUR = 0x4B,
                NUM_FIVE = 0x4C,
                NUM_SIX = 0x4D,
                LEFT_SHIFT = 0x2A,
                Z = 0x2C,
                X = 0x2D,
                C = 0x2E,
                V = 0x2F,
                B = 0x30,
                N = 0x31,
                M = 0x32,
                COMMA = 0x33,
                PERIOD = 0x34,
                FORWARD_SLASH = 0x35,
                RIGHT_SHIFT = 0x36,
                ARROW_UP = 0x148,
                NUM_ONE = 0x4F,
                NUM_TWO = 0x50,
                NUM_THREE = 0x51,
                NUM_ENTER = 0x11C,
                LEFT_CONTROL = 0x1D,
                LEFT_WINDOWS = 0x15B,
                LEFT_ALT = 0x38,
                SPACE = 0x39,
                RIGHT_ALT = 0x138,
                RIGHT_WINDOWS = 0x15C,
                APPLICATION_SELECT = 0x15D,
                RIGHT_CONTROL = 0x11D,
                ARROW_LEFT = 0x14B,
                ARROW_DOWN = 0x150,
                ARROW_RIGHT = 0x14D,
                NUM_ZERO = 0x52,
                NUM_PERIOD = 0x53,
                G_1 = 0xFFF1,
                G_2 = 0xFFF2,
                G_3 = 0xFFF3,
                G_4 = 0xFFF4,
                G_5 = 0xFFF5,
                G_6 = 0xFFF6,
                G_7 = 0xFFF7,
                G_8 = 0xFFF8,
                G_9 = 0xFFF9,
                G_LOGO = 0xFFFF1,
                G_BADGE = 0xFFFF2
            };

            public enum keyboardBitmapKeys
            {
                UNKNOWN = -1,
                ESC = 0,
                F1 = 4,
                F2 = 8,
                F3 = 12,
                F4 = 16,
                F5 = 20,
                F6 = 24,
                F7 = 28,
                F8 = 32,
                F9 = 36,
                F10 = 40,
                F11 = 44,
                F12 = 48,
                PRINT_SCREEN = 52,
                SCROLL_LOCK = 56,
                PAUSE_BREAK = 60,
                //64
                //68
                //72
                //76
                //80

                TILDE = 84,
                ONE = 88,
                TWO = 92,
                THREE = 96,
                FOUR = 100,
                FIVE = 104,
                SIX = 108,
                SEVEN = 112,
                EIGHT = 116,
                NINE = 120,
                ZERO = 124,
                MINUS = 128,
                EQUALS = 132,
                BACKSPACE = 136,
                INSERT = 140,
                HOME = 144,
                PAGE_UP = 148,
                NUM_LOCK = 152,
                NUM_SLASH = 156,
                NUM_ASTERISK = 160,
                NUM_MINUS = 164,

                TAB = 168,
                Q = 172,
                W = 176,
                E = 180,
                R = 184,
                T = 188,
                Y = 192,
                U = 196,
                I = 200,
                O = 204,
                P = 208,
                OPEN_BRACKET = 212,
                CLOSE_BRACKET = 216,
                BACKSLASH = 220,
                KEYBOARD_DELETE = 224,
                END = 228,
                PAGE_DOWN = 232,
                NUM_SEVEN = 236,
                NUM_EIGHT = 240,
                NUM_NINE = 244,
                NUM_PLUS = 248,

                CAPS_LOCK = 252,
                A = 256,
                S = 260,
                D = 264,
                F = 268,
                G = 272,
                H = 276,
                J = 280,
                K = 284,
                L = 288,
                SEMICOLON = 292,
                APOSTROPHE = 296,
                HASHTAG = 300,//300
                ENTER = 304,
                //308
                //312
                //316
                NUM_FOUR = 320,
                NUM_FIVE = 324,
                NUM_SIX = 328,
                //332

                LEFT_SHIFT = 336,
                BACKSLASH_UK = 340,
                Z = 344,
                X = 348,
                C = 352,
                V = 356,
                B = 360,
                N = 364,
                M = 368,
                COMMA = 372,
                PERIOD = 376,
                FORWARD_SLASH = 380,
                OEM102 = 384,
                RIGHT_SHIFT = 388,
                //392
                ARROW_UP = 396,
                //400
                NUM_ONE = 404,
                NUM_TWO = 408,
                NUM_THREE = 412,
                NUM_ENTER = 416,

                LEFT_CONTROL = 420,
                LEFT_WINDOWS = 424,
                LEFT_ALT = 428,
                //432
                JPN_MUHENKAN = 436,//436
                SPACE = 440,
                //444
                //448
                JPN_HENKAN = 452,//452
                JPN_HIRAGANA_KATAKANA = 456,//456
                                            //460
                RIGHT_ALT = 464,
                RIGHT_WINDOWS = 468,
                APPLICATION_SELECT = 472,
                RIGHT_CONTROL = 476,
                ARROW_LEFT = 480,
                ARROW_DOWN = 484,
                ARROW_RIGHT = 488,
                NUM_ZERO = 492,
                NUM_PERIOD = 496,
                //500
            };

            public static DeviceKeys ToDeviceKey(keyboardNames key)
            {
                switch (key)
                {
                    case (keyboardNames.ESC):
                        return DeviceKeys.ESC;
                    case (keyboardNames.F1):
                        return DeviceKeys.F1;
                    case (keyboardNames.F2):
                        return DeviceKeys.F2;
                    case (keyboardNames.F3):
                        return DeviceKeys.F3;
                    case (keyboardNames.F4):
                        return DeviceKeys.F4;
                    case (keyboardNames.F5):
                        return DeviceKeys.F5;
                    case (keyboardNames.F6):
                        return DeviceKeys.F6;
                    case (keyboardNames.F7):
                        return DeviceKeys.F7;
                    case (keyboardNames.F8):
                        return DeviceKeys.F8;
                    case (keyboardNames.F9):
                        return DeviceKeys.F9;
                    case (keyboardNames.F10):
                        return DeviceKeys.F10;
                    case (keyboardNames.F11):
                        return DeviceKeys.F11;
                    case (keyboardNames.F12):
                        return DeviceKeys.F12;
                    case (keyboardNames.PRINT_SCREEN):
                        return DeviceKeys.PRINT_SCREEN;
                    case (keyboardNames.SCROLL_LOCK):
                        return DeviceKeys.SCROLL_LOCK;
                    case (keyboardNames.PAUSE_BREAK):
                        return DeviceKeys.PAUSE_BREAK;
                    case (keyboardNames.TILDE):
                        return DeviceKeys.TILDE;
                    case (keyboardNames.ONE):
                        return DeviceKeys.ONE;
                    case (keyboardNames.TWO):
                        return DeviceKeys.TWO;
                    case (keyboardNames.THREE):
                        return DeviceKeys.THREE;
                    case (keyboardNames.FOUR):
                        return DeviceKeys.FOUR;
                    case (keyboardNames.FIVE):
                        return DeviceKeys.FIVE;
                    case (keyboardNames.SIX):
                        return DeviceKeys.SIX;
                    case (keyboardNames.SEVEN):
                        return DeviceKeys.SEVEN;
                    case (keyboardNames.EIGHT):
                        return DeviceKeys.EIGHT;
                    case (keyboardNames.NINE):
                        return DeviceKeys.NINE;
                    case (keyboardNames.ZERO):
                        return DeviceKeys.ZERO;
                    case (keyboardNames.MINUS):
                        return DeviceKeys.MINUS;
                    case (keyboardNames.EQUALS):
                        return DeviceKeys.EQUALS;
                    case (keyboardNames.BACKSPACE):
                        return DeviceKeys.BACKSPACE;
                    case (keyboardNames.INSERT):
                        return DeviceKeys.INSERT;
                    case (keyboardNames.HOME):
                        return DeviceKeys.HOME;
                    case (keyboardNames.PAGE_UP):
                        return DeviceKeys.PAGE_UP;
                    case (keyboardNames.NUM_LOCK):
                        return DeviceKeys.NUM_LOCK;
                    case (keyboardNames.NUM_SLASH):
                        return DeviceKeys.NUM_SLASH;
                    case (keyboardNames.NUM_ASTERISK):
                        return DeviceKeys.NUM_ASTERISK;
                    case (keyboardNames.NUM_MINUS):
                        return DeviceKeys.NUM_MINUS;
                    case (keyboardNames.TAB):
                        return DeviceKeys.TAB;
                    case (keyboardNames.Q):
                        return DeviceKeys.Q;
                    case (keyboardNames.W):
                        return DeviceKeys.W;
                    case (keyboardNames.E):
                        return DeviceKeys.E;
                    case (keyboardNames.R):
                        return DeviceKeys.R;
                    case (keyboardNames.T):
                        return DeviceKeys.T;
                    case (keyboardNames.Y):
                        return DeviceKeys.Y;
                    case (keyboardNames.U):
                        return DeviceKeys.U;
                    case (keyboardNames.I):
                        return DeviceKeys.I;
                    case (keyboardNames.O):
                        return DeviceKeys.O;
                    case (keyboardNames.P):
                        return DeviceKeys.P;
                    case (keyboardNames.OPEN_BRACKET):
                        return DeviceKeys.OPEN_BRACKET;
                    case (keyboardNames.CLOSE_BRACKET):
                        return DeviceKeys.CLOSE_BRACKET;
                    case (keyboardNames.BACKSLASH):
                        return DeviceKeys.BACKSLASH;
                    case (keyboardNames.KEYBOARD_DELETE):
                        return DeviceKeys.DELETE;
                    case (keyboardNames.END):
                        return DeviceKeys.END;
                    case (keyboardNames.PAGE_DOWN):
                        return DeviceKeys.PAGE_DOWN;
                    case (keyboardNames.NUM_SEVEN):
                        return DeviceKeys.NUM_SEVEN;
                    case (keyboardNames.NUM_EIGHT):
                        return DeviceKeys.NUM_EIGHT;
                    case (keyboardNames.NUM_NINE):
                        return DeviceKeys.NUM_NINE;
                    case (keyboardNames.NUM_PLUS):
                        return DeviceKeys.NUM_PLUS;
                    case (keyboardNames.CAPS_LOCK):
                        return DeviceKeys.CAPS_LOCK;
                    case (keyboardNames.A):
                        return DeviceKeys.A;
                    case (keyboardNames.S):
                        return DeviceKeys.S;
                    case (keyboardNames.D):
                        return DeviceKeys.D;
                    case (keyboardNames.F):
                        return DeviceKeys.F;
                    case (keyboardNames.G):
                        return DeviceKeys.G;
                    case (keyboardNames.H):
                        return DeviceKeys.H;
                    case (keyboardNames.J):
                        return DeviceKeys.J;
                    case (keyboardNames.K):
                        return DeviceKeys.K;
                    case (keyboardNames.L):
                        return DeviceKeys.L;
                    case (keyboardNames.SEMICOLON):
                        return DeviceKeys.SEMICOLON;
                    case (keyboardNames.APOSTROPHE):
                        return DeviceKeys.APOSTROPHE;
                    //case (keyboardNames.HASHTAG):
                    //    return DeviceKeys.HASHTAG;
                    case (keyboardNames.ENTER):
                        return DeviceKeys.ENTER;
                    case (keyboardNames.NUM_FOUR):
                        return DeviceKeys.NUM_FOUR;
                    case (keyboardNames.NUM_FIVE):
                        return DeviceKeys.NUM_FIVE;
                    case (keyboardNames.NUM_SIX):
                        return DeviceKeys.NUM_SIX;
                    case (keyboardNames.LEFT_SHIFT):
                        return DeviceKeys.LEFT_SHIFT;
                    //case (keyboardNames.BACKSLASH_UK):
                    //    return DeviceKeys.BACKSLASH_UK;
                    case (keyboardNames.Z):
                        return DeviceKeys.Z;
                    case (keyboardNames.X):
                        return DeviceKeys.X;
                    case (keyboardNames.C):
                        return DeviceKeys.C;
                    case (keyboardNames.V):
                        return DeviceKeys.V;
                    case (keyboardNames.B):
                        return DeviceKeys.B;
                    case (keyboardNames.N):
                        return DeviceKeys.N;
                    case (keyboardNames.M):
                        return DeviceKeys.M;
                    case (keyboardNames.COMMA):
                        return DeviceKeys.COMMA;
                    case (keyboardNames.PERIOD):
                        return DeviceKeys.PERIOD;
                    case (keyboardNames.FORWARD_SLASH):
                        return DeviceKeys.FORWARD_SLASH;
                    case (keyboardNames.RIGHT_SHIFT):
                        return DeviceKeys.RIGHT_SHIFT;
                    case (keyboardNames.ARROW_UP):
                        return DeviceKeys.ARROW_UP;
                    case (keyboardNames.NUM_ONE):
                        return DeviceKeys.NUM_ONE;
                    case (keyboardNames.NUM_TWO):
                        return DeviceKeys.NUM_TWO;
                    case (keyboardNames.NUM_THREE):
                        return DeviceKeys.NUM_THREE;
                    case (keyboardNames.NUM_ENTER):
                        return DeviceKeys.NUM_ENTER;
                    case (keyboardNames.LEFT_CONTROL):
                        return DeviceKeys.LEFT_CONTROL;
                    case (keyboardNames.LEFT_WINDOWS):
                        return DeviceKeys.LEFT_WINDOWS;
                    case (keyboardNames.LEFT_ALT):
                        return DeviceKeys.LEFT_ALT;
                    case (keyboardNames.SPACE):
                        return DeviceKeys.SPACE;
                    case (keyboardNames.RIGHT_ALT):
                        return DeviceKeys.RIGHT_ALT;
                    case (keyboardNames.RIGHT_WINDOWS):
                        return DeviceKeys.RIGHT_WINDOWS;
                    case (keyboardNames.APPLICATION_SELECT):
                        return DeviceKeys.APPLICATION_SELECT;
                    case (keyboardNames.RIGHT_CONTROL):
                        return DeviceKeys.RIGHT_CONTROL;
                    case (keyboardNames.ARROW_LEFT):
                        return DeviceKeys.ARROW_LEFT;
                    case (keyboardNames.ARROW_DOWN):
                        return DeviceKeys.ARROW_DOWN;
                    case (keyboardNames.ARROW_RIGHT):
                        return DeviceKeys.ARROW_RIGHT;
                    case (keyboardNames.NUM_ZERO):
                        return DeviceKeys.NUM_ZERO;
                    case (keyboardNames.NUM_PERIOD):
                        return DeviceKeys.NUM_PERIOD;
                    default:
                        return DeviceKeys.NONE;
                }
            }

            public static keyboardBitmapKeys ToLogitechBitmap(DeviceKeys key)
            {
                switch (key)
                {
                    case (DeviceKeys.ESC):
                        return keyboardBitmapKeys.ESC;
                    case (DeviceKeys.F1):
                        return keyboardBitmapKeys.F1;
                    case (DeviceKeys.F2):
                        return keyboardBitmapKeys.F2;
                    case (DeviceKeys.F3):
                        return keyboardBitmapKeys.F3;
                    case (DeviceKeys.F4):
                        return keyboardBitmapKeys.F4;
                    case (DeviceKeys.F5):
                        return keyboardBitmapKeys.F5;
                    case (DeviceKeys.F6):
                        return keyboardBitmapKeys.F6;
                    case (DeviceKeys.F7):
                        return keyboardBitmapKeys.F7;
                    case (DeviceKeys.F8):
                        return keyboardBitmapKeys.F8;
                    case (DeviceKeys.F9):
                        return keyboardBitmapKeys.F9;
                    case (DeviceKeys.F10):
                        return keyboardBitmapKeys.F10;
                    case (DeviceKeys.F11):
                        return keyboardBitmapKeys.F11;
                    case (DeviceKeys.F12):
                        return keyboardBitmapKeys.F12;
                    case (DeviceKeys.PRINT_SCREEN):
                        return keyboardBitmapKeys.PRINT_SCREEN;
                    case (DeviceKeys.SCROLL_LOCK):
                        return keyboardBitmapKeys.SCROLL_LOCK;
                    case (DeviceKeys.PAUSE_BREAK):
                        return keyboardBitmapKeys.PAUSE_BREAK;
                    case (DeviceKeys.JPN_HALFFULLWIDTH):
                        return keyboardBitmapKeys.TILDE;
                    case (DeviceKeys.OEM5):
                        if (Global.kbLayout.Loaded_Localization == Settings.PreferredKeyboardLocalization.jpn)
                            return keyboardBitmapKeys.UNKNOWN;
                        else
                            return keyboardBitmapKeys.TILDE;
                    case (DeviceKeys.TILDE):
                        return keyboardBitmapKeys.TILDE;
                    case (DeviceKeys.ONE):
                        return keyboardBitmapKeys.ONE;
                    case (DeviceKeys.TWO):
                        return keyboardBitmapKeys.TWO;
                    case (DeviceKeys.THREE):
                        return keyboardBitmapKeys.THREE;
                    case (DeviceKeys.FOUR):
                        return keyboardBitmapKeys.FOUR;
                    case (DeviceKeys.FIVE):
                        return keyboardBitmapKeys.FIVE;
                    case (DeviceKeys.SIX):
                        return keyboardBitmapKeys.SIX;
                    case (DeviceKeys.SEVEN):
                        return keyboardBitmapKeys.SEVEN;
                    case (DeviceKeys.EIGHT):
                        return keyboardBitmapKeys.EIGHT;
                    case (DeviceKeys.NINE):
                        return keyboardBitmapKeys.NINE;
                    case (DeviceKeys.ZERO):
                        return keyboardBitmapKeys.ZERO;
                    case (DeviceKeys.MINUS):
                        return keyboardBitmapKeys.MINUS;
                    case (DeviceKeys.EQUALS):
                        return keyboardBitmapKeys.EQUALS;
                    case (DeviceKeys.BACKSPACE):
                        return keyboardBitmapKeys.BACKSPACE;
                    case (DeviceKeys.INSERT):
                        return keyboardBitmapKeys.INSERT;
                    case (DeviceKeys.HOME):
                        return keyboardBitmapKeys.HOME;
                    case (DeviceKeys.PAGE_UP):
                        return keyboardBitmapKeys.PAGE_UP;
                    case (DeviceKeys.NUM_LOCK):
                        return keyboardBitmapKeys.NUM_LOCK;
                    case (DeviceKeys.NUM_SLASH):
                        return keyboardBitmapKeys.NUM_SLASH;
                    case (DeviceKeys.NUM_ASTERISK):
                        return keyboardBitmapKeys.NUM_ASTERISK;
                    case (DeviceKeys.NUM_MINUS):
                        return keyboardBitmapKeys.NUM_MINUS;
                    case (DeviceKeys.TAB):
                        return keyboardBitmapKeys.TAB;
                    case (DeviceKeys.Q):
                        return keyboardBitmapKeys.Q;
                    case (DeviceKeys.W):
                        return keyboardBitmapKeys.W;
                    case (DeviceKeys.E):
                        return keyboardBitmapKeys.E;
                    case (DeviceKeys.R):
                        return keyboardBitmapKeys.R;
                    case (DeviceKeys.T):
                        return keyboardBitmapKeys.T;
                    case (DeviceKeys.Y):
                        return keyboardBitmapKeys.Y;
                    case (DeviceKeys.U):
                        return keyboardBitmapKeys.U;
                    case (DeviceKeys.I):
                        return keyboardBitmapKeys.I;
                    case (DeviceKeys.O):
                        return keyboardBitmapKeys.O;
                    case (DeviceKeys.P):
                        return keyboardBitmapKeys.P;
                    case (DeviceKeys.OPEN_BRACKET):
                        return keyboardBitmapKeys.OPEN_BRACKET;
                    case (DeviceKeys.CLOSE_BRACKET):
                        return keyboardBitmapKeys.CLOSE_BRACKET;
                    case (DeviceKeys.BACKSLASH):
                        return keyboardBitmapKeys.BACKSLASH;
                    case (DeviceKeys.DELETE):
                        return keyboardBitmapKeys.KEYBOARD_DELETE;
                    case (DeviceKeys.END):
                        return keyboardBitmapKeys.END;
                    case (DeviceKeys.PAGE_DOWN):
                        return keyboardBitmapKeys.PAGE_DOWN;
                    case (DeviceKeys.NUM_SEVEN):
                        return keyboardBitmapKeys.NUM_SEVEN;
                    case (DeviceKeys.NUM_EIGHT):
                        return keyboardBitmapKeys.NUM_EIGHT;
                    case (DeviceKeys.NUM_NINE):
                        return keyboardBitmapKeys.NUM_NINE;
                    case (DeviceKeys.NUM_PLUS):
                        return keyboardBitmapKeys.NUM_PLUS;
                    case (DeviceKeys.CAPS_LOCK):
                        return keyboardBitmapKeys.CAPS_LOCK;
                    case (DeviceKeys.A):
                        return keyboardBitmapKeys.A;
                    case (DeviceKeys.S):
                        return keyboardBitmapKeys.S;
                    case (DeviceKeys.D):
                        return keyboardBitmapKeys.D;
                    case (DeviceKeys.F):
                        return keyboardBitmapKeys.F;
                    case (DeviceKeys.G):
                        return keyboardBitmapKeys.G;
                    case (DeviceKeys.H):
                        return keyboardBitmapKeys.H;
                    case (DeviceKeys.J):
                        return keyboardBitmapKeys.J;
                    case (DeviceKeys.K):
                        return keyboardBitmapKeys.K;
                    case (DeviceKeys.L):
                        return keyboardBitmapKeys.L;
                    case (DeviceKeys.SEMICOLON):
                        return keyboardBitmapKeys.SEMICOLON;
                    case (DeviceKeys.APOSTROPHE):
                        return keyboardBitmapKeys.APOSTROPHE;
                    case (DeviceKeys.HASHTAG):
                        return keyboardBitmapKeys.HASHTAG;
                    case (DeviceKeys.ENTER):
                        return keyboardBitmapKeys.ENTER;
                    case (DeviceKeys.NUM_FOUR):
                        return keyboardBitmapKeys.NUM_FOUR;
                    case (DeviceKeys.NUM_FIVE):
                        return keyboardBitmapKeys.NUM_FIVE;
                    case (DeviceKeys.NUM_SIX):
                        return keyboardBitmapKeys.NUM_SIX;
                    case (DeviceKeys.LEFT_SHIFT):
                        return keyboardBitmapKeys.LEFT_SHIFT;
                    case (DeviceKeys.BACKSLASH_UK):
                        if (Global.kbLayout.Loaded_Localization == Settings.PreferredKeyboardLocalization.jpn)
                            return keyboardBitmapKeys.OEM102;
                        else
                            return keyboardBitmapKeys.BACKSLASH_UK;
                    case (DeviceKeys.Z):
                        return keyboardBitmapKeys.Z;
                    case (DeviceKeys.X):
                        return keyboardBitmapKeys.X;
                    case (DeviceKeys.C):
                        return keyboardBitmapKeys.C;
                    case (DeviceKeys.V):
                        return keyboardBitmapKeys.V;
                    case (DeviceKeys.B):
                        return keyboardBitmapKeys.B;
                    case (DeviceKeys.N):
                        return keyboardBitmapKeys.N;
                    case (DeviceKeys.M):
                        return keyboardBitmapKeys.M;
                    case (DeviceKeys.COMMA):
                        return keyboardBitmapKeys.COMMA;
                    case (DeviceKeys.PERIOD):
                        return keyboardBitmapKeys.PERIOD;
                    case (DeviceKeys.FORWARD_SLASH):
                        return keyboardBitmapKeys.FORWARD_SLASH;
                    case (DeviceKeys.OEM8):
                        return keyboardBitmapKeys.FORWARD_SLASH;
                    case (DeviceKeys.OEM102):
                        return keyboardBitmapKeys.OEM102;
                    case (DeviceKeys.RIGHT_SHIFT):
                        return keyboardBitmapKeys.RIGHT_SHIFT;
                    case (DeviceKeys.ARROW_UP):
                        return keyboardBitmapKeys.ARROW_UP;
                    case (DeviceKeys.NUM_ONE):
                        return keyboardBitmapKeys.NUM_ONE;
                    case (DeviceKeys.NUM_TWO):
                        return keyboardBitmapKeys.NUM_TWO;
                    case (DeviceKeys.NUM_THREE):
                        return keyboardBitmapKeys.NUM_THREE;
                    case (DeviceKeys.NUM_ENTER):
                        return keyboardBitmapKeys.NUM_ENTER;
                    case (DeviceKeys.LEFT_CONTROL):
                        return keyboardBitmapKeys.LEFT_CONTROL;
                    case (DeviceKeys.LEFT_WINDOWS):
                        return keyboardBitmapKeys.LEFT_WINDOWS;
                    case (DeviceKeys.LEFT_ALT):
                        return keyboardBitmapKeys.LEFT_ALT;
                    case (DeviceKeys.JPN_MUHENKAN):
                        return keyboardBitmapKeys.JPN_MUHENKAN;
                    case (DeviceKeys.SPACE):
                        return keyboardBitmapKeys.SPACE;
                    case (DeviceKeys.JPN_HENKAN):
                        return keyboardBitmapKeys.JPN_HENKAN;
                    case (DeviceKeys.JPN_HIRAGANA_KATAKANA):
                        return keyboardBitmapKeys.JPN_HIRAGANA_KATAKANA;
                    case (DeviceKeys.RIGHT_ALT):
                        return keyboardBitmapKeys.RIGHT_ALT;
                    case (DeviceKeys.RIGHT_WINDOWS):
                        return keyboardBitmapKeys.RIGHT_WINDOWS;
                    case (DeviceKeys.FN_Key):
                        return keyboardBitmapKeys.RIGHT_WINDOWS;
                    case (DeviceKeys.APPLICATION_SELECT):
                        return keyboardBitmapKeys.APPLICATION_SELECT;
                    case (DeviceKeys.RIGHT_CONTROL):
                        return keyboardBitmapKeys.RIGHT_CONTROL;
                    case (DeviceKeys.ARROW_LEFT):
                        return keyboardBitmapKeys.ARROW_LEFT;
                    case (DeviceKeys.ARROW_DOWN):
                        return keyboardBitmapKeys.ARROW_DOWN;
                    case (DeviceKeys.ARROW_RIGHT):
                        return keyboardBitmapKeys.ARROW_RIGHT;
                    case (DeviceKeys.NUM_ZERO):
                        return keyboardBitmapKeys.NUM_ZERO;
                    case (DeviceKeys.NUM_PERIOD):
                        return keyboardBitmapKeys.NUM_PERIOD;
                    default:
                        return keyboardBitmapKeys.UNKNOWN;
                }
            }

            public static keyboardBitmapKeys ToLogitechBitmap(keyboardNames key)
            {
                switch (key)
                {
                    case (keyboardNames.ESC):
                        return keyboardBitmapKeys.ESC;
                    case (keyboardNames.F1):
                        return keyboardBitmapKeys.F1;
                    case (keyboardNames.F2):
                        return keyboardBitmapKeys.F2;
                    case (keyboardNames.F3):
                        return keyboardBitmapKeys.F3;
                    case (keyboardNames.F4):
                        return keyboardBitmapKeys.F4;
                    case (keyboardNames.F5):
                        return keyboardBitmapKeys.F5;
                    case (keyboardNames.F6):
                        return keyboardBitmapKeys.F6;
                    case (keyboardNames.F7):
                        return keyboardBitmapKeys.F7;
                    case (keyboardNames.F8):
                        return keyboardBitmapKeys.F8;
                    case (keyboardNames.F9):
                        return keyboardBitmapKeys.F9;
                    case (keyboardNames.F10):
                        return keyboardBitmapKeys.F10;
                    case (keyboardNames.F11):
                        return keyboardBitmapKeys.F11;
                    case (keyboardNames.F12):
                        return keyboardBitmapKeys.F12;
                    case (keyboardNames.PRINT_SCREEN):
                        return keyboardBitmapKeys.PRINT_SCREEN;
                    case (keyboardNames.SCROLL_LOCK):
                        return keyboardBitmapKeys.SCROLL_LOCK;
                    case (keyboardNames.PAUSE_BREAK):
                        return keyboardBitmapKeys.PAUSE_BREAK;
                    case (keyboardNames.TILDE):
                        return keyboardBitmapKeys.TILDE;
                    case (keyboardNames.ONE):
                        return keyboardBitmapKeys.ONE;
                    case (keyboardNames.TWO):
                        return keyboardBitmapKeys.TWO;
                    case (keyboardNames.THREE):
                        return keyboardBitmapKeys.THREE;
                    case (keyboardNames.FOUR):
                        return keyboardBitmapKeys.FOUR;
                    case (keyboardNames.FIVE):
                        return keyboardBitmapKeys.FIVE;
                    case (keyboardNames.SIX):
                        return keyboardBitmapKeys.SIX;
                    case (keyboardNames.SEVEN):
                        return keyboardBitmapKeys.SEVEN;
                    case (keyboardNames.EIGHT):
                        return keyboardBitmapKeys.EIGHT;
                    case (keyboardNames.NINE):
                        return keyboardBitmapKeys.NINE;
                    case (keyboardNames.ZERO):
                        return keyboardBitmapKeys.ZERO;
                    case (keyboardNames.MINUS):
                        return keyboardBitmapKeys.MINUS;
                    case (keyboardNames.EQUALS):
                        return keyboardBitmapKeys.EQUALS;
                    case (keyboardNames.BACKSPACE):
                        return keyboardBitmapKeys.BACKSPACE;
                    case (keyboardNames.INSERT):
                        return keyboardBitmapKeys.INSERT;
                    case (keyboardNames.HOME):
                        return keyboardBitmapKeys.HOME;
                    case (keyboardNames.PAGE_UP):
                        return keyboardBitmapKeys.PAGE_UP;
                    case (keyboardNames.NUM_LOCK):
                        return keyboardBitmapKeys.NUM_LOCK;
                    case (keyboardNames.NUM_SLASH):
                        return keyboardBitmapKeys.NUM_SLASH;
                    case (keyboardNames.NUM_ASTERISK):
                        return keyboardBitmapKeys.NUM_ASTERISK;
                    case (keyboardNames.NUM_MINUS):
                        return keyboardBitmapKeys.NUM_MINUS;
                    case (keyboardNames.TAB):
                        return keyboardBitmapKeys.TAB;
                    case (keyboardNames.Q):
                        return keyboardBitmapKeys.Q;
                    case (keyboardNames.W):
                        return keyboardBitmapKeys.W;
                    case (keyboardNames.E):
                        return keyboardBitmapKeys.E;
                    case (keyboardNames.R):
                        return keyboardBitmapKeys.R;
                    case (keyboardNames.T):
                        return keyboardBitmapKeys.T;
                    case (keyboardNames.Y):
                        return keyboardBitmapKeys.Y;
                    case (keyboardNames.U):
                        return keyboardBitmapKeys.U;
                    case (keyboardNames.I):
                        return keyboardBitmapKeys.I;
                    case (keyboardNames.O):
                        return keyboardBitmapKeys.O;
                    case (keyboardNames.P):
                        return keyboardBitmapKeys.P;
                    case (keyboardNames.OPEN_BRACKET):
                        return keyboardBitmapKeys.OPEN_BRACKET;
                    case (keyboardNames.CLOSE_BRACKET):
                        return keyboardBitmapKeys.CLOSE_BRACKET;
                    case (keyboardNames.BACKSLASH):
                        return keyboardBitmapKeys.BACKSLASH;
                    case (keyboardNames.KEYBOARD_DELETE):
                        return keyboardBitmapKeys.KEYBOARD_DELETE;
                    case (keyboardNames.END):
                        return keyboardBitmapKeys.END;
                    case (keyboardNames.PAGE_DOWN):
                        return keyboardBitmapKeys.PAGE_DOWN;
                    case (keyboardNames.NUM_SEVEN):
                        return keyboardBitmapKeys.NUM_SEVEN;
                    case (keyboardNames.NUM_EIGHT):
                        return keyboardBitmapKeys.NUM_EIGHT;
                    case (keyboardNames.NUM_NINE):
                        return keyboardBitmapKeys.NUM_NINE;
                    case (keyboardNames.NUM_PLUS):
                        return keyboardBitmapKeys.NUM_PLUS;
                    case (keyboardNames.CAPS_LOCK):
                        return keyboardBitmapKeys.CAPS_LOCK;
                    case (keyboardNames.A):
                        return keyboardBitmapKeys.A;
                    case (keyboardNames.S):
                        return keyboardBitmapKeys.S;
                    case (keyboardNames.D):
                        return keyboardBitmapKeys.D;
                    case (keyboardNames.F):
                        return keyboardBitmapKeys.F;
                    case (keyboardNames.G):
                        return keyboardBitmapKeys.G;
                    case (keyboardNames.H):
                        return keyboardBitmapKeys.H;
                    case (keyboardNames.J):
                        return keyboardBitmapKeys.J;
                    case (keyboardNames.K):
                        return keyboardBitmapKeys.K;
                    case (keyboardNames.L):
                        return keyboardBitmapKeys.L;
                    case (keyboardNames.SEMICOLON):
                        return keyboardBitmapKeys.SEMICOLON;
                    case (keyboardNames.APOSTROPHE):
                        return keyboardBitmapKeys.APOSTROPHE;
                    //case (keyboardNames.HASHTAG):
                    //    return Logitech_keyboardBitmapKeys.HASHTAG;
                    case (keyboardNames.ENTER):
                        return keyboardBitmapKeys.ENTER;
                    case (keyboardNames.NUM_FOUR):
                        return keyboardBitmapKeys.NUM_FOUR;
                    case (keyboardNames.NUM_FIVE):
                        return keyboardBitmapKeys.NUM_FIVE;
                    case (keyboardNames.NUM_SIX):
                        return keyboardBitmapKeys.NUM_SIX;
                    case (keyboardNames.LEFT_SHIFT):
                        return keyboardBitmapKeys.LEFT_SHIFT;
                    //case (keyboardNames.BACKSLASH_UK):
                    //    return Logitech_keyboardBitmapKeys.BACKSLASH_UK;
                    case (keyboardNames.Z):
                        return keyboardBitmapKeys.Z;
                    case (keyboardNames.X):
                        return keyboardBitmapKeys.X;
                    case (keyboardNames.C):
                        return keyboardBitmapKeys.C;
                    case (keyboardNames.V):
                        return keyboardBitmapKeys.V;
                    case (keyboardNames.B):
                        return keyboardBitmapKeys.B;
                    case (keyboardNames.N):
                        return keyboardBitmapKeys.N;
                    case (keyboardNames.M):
                        return keyboardBitmapKeys.M;
                    case (keyboardNames.COMMA):
                        return keyboardBitmapKeys.COMMA;
                    case (keyboardNames.PERIOD):
                        return keyboardBitmapKeys.PERIOD;
                    case (keyboardNames.FORWARD_SLASH):
                        return keyboardBitmapKeys.FORWARD_SLASH;
                    case (keyboardNames.RIGHT_SHIFT):
                        return keyboardBitmapKeys.RIGHT_SHIFT;
                    case (keyboardNames.ARROW_UP):
                        return keyboardBitmapKeys.ARROW_UP;
                    case (keyboardNames.NUM_ONE):
                        return keyboardBitmapKeys.NUM_ONE;
                    case (keyboardNames.NUM_TWO):
                        return keyboardBitmapKeys.NUM_TWO;
                    case (keyboardNames.NUM_THREE):
                        return keyboardBitmapKeys.NUM_THREE;
                    case (keyboardNames.NUM_ENTER):
                        return keyboardBitmapKeys.NUM_ENTER;
                    case (keyboardNames.LEFT_CONTROL):
                        return keyboardBitmapKeys.LEFT_CONTROL;
                    case (keyboardNames.LEFT_WINDOWS):
                        return keyboardBitmapKeys.LEFT_WINDOWS;
                    case (keyboardNames.LEFT_ALT):
                        return keyboardBitmapKeys.LEFT_ALT;
                    case (keyboardNames.SPACE):
                        return keyboardBitmapKeys.SPACE;
                    case (keyboardNames.RIGHT_ALT):
                        return keyboardBitmapKeys.RIGHT_ALT;
                    case (keyboardNames.RIGHT_WINDOWS):
                        return keyboardBitmapKeys.RIGHT_WINDOWS;
                    case (keyboardNames.APPLICATION_SELECT):
                        return keyboardBitmapKeys.APPLICATION_SELECT;
                    case (keyboardNames.RIGHT_CONTROL):
                        return keyboardBitmapKeys.RIGHT_CONTROL;
                    case (keyboardNames.ARROW_LEFT):
                        return keyboardBitmapKeys.ARROW_LEFT;
                    case (keyboardNames.ARROW_DOWN):
                        return keyboardBitmapKeys.ARROW_DOWN;
                    case (keyboardNames.ARROW_RIGHT):
                        return keyboardBitmapKeys.ARROW_RIGHT;
                    case (keyboardNames.NUM_ZERO):
                        return keyboardBitmapKeys.NUM_ZERO;
                    case (keyboardNames.NUM_PERIOD):
                        return keyboardBitmapKeys.NUM_PERIOD;
                    default:
                        return keyboardBitmapKeys.UNKNOWN;
                }
            }
        }

        public static class Corsair
        {
            public enum CorsairLedId
            {
                CLI_Invalid = 0,
                K_Escape = 1,
                K_F1 = 2,
                K_F2 = 3,
                K_F3 = 4,
                K_F4 = 5,
                K_F5 = 6,
                K_F6 = 7,
                K_F7 = 8,
                K_F8 = 9,
                K_F9 = 10,
                K_F10 = 11,
                K_F11 = 12,
                K_GraveAccentAndTilde = 13,
                K_1 = 14,
                K_2 = 15,
                K_3 = 16,
                K_4 = 17,
                K_5 = 18,
                K_6 = 19,
                K_7 = 20,
                K_8 = 21,
                K_9 = 22,
                K_0 = 23,
                K_MinusAndUnderscore = 24,
                K_Tab = 25,
                K_Q = 26,
                K_W = 27,
                K_E = 28,
                K_R = 29,
                K_T = 30,
                K_Y = 31,
                K_U = 32,
                K_I = 33,
                K_O = 34,
                K_P = 35,
                K_BracketLeft = 36,
                K_CapsLock = 37,
                K_A = 38,
                K_S = 39,
                K_D = 40,
                K_F = 41,
                K_G = 42,
                K_H = 43,
                K_J = 44,
                K_K = 45,
                K_L = 46,
                K_SemicolonAndColon = 47,
                K_ApostropheAndDoubleQuote = 48,
                K_LeftShift = 49,
                K_NonUsBackslash = 50,
                K_Z = 51,
                K_X = 52,
                K_C = 53,
                K_V = 54,
                K_B = 55,
                K_N = 56,
                K_M = 57,
                K_CommaAndLessThan = 58,
                K_PeriodAndBiggerThan = 59,
                K_SlashAndQuestionMark = 60,
                K_LeftCtrl = 61,
                K_LeftGui = 62,
                K_LeftAlt = 63,
                K_Lang2 = 64,
                K_Space = 65,
                K_Lang1 = 66,
                K_International2 = 67,
                K_RightAlt = 68,
                K_RightGui = 69,
                K_Application = 70,
                K_LedProgramming = 71,
                K_Brightness = 72,
                K_F12 = 73,
                K_PrintScreen = 74,
                K_ScrollLock = 75,
                K_PauseBreak = 76,
                K_Insert = 77,
                K_Home = 78,
                K_PageUp = 79,
                K_BracketRight = 80,
                K_Backslash = 81,
                K_NonUsTilde = 82,
                K_Enter = 83,
                K_International1 = 84,
                K_EqualsAndPlus = 85,
                K_International3 = 86,
                K_Backspace = 87,
                K_Delete = 88,
                K_End = 89,
                K_PageDown = 90,
                K_RightShift = 91,
                K_RightCtrl = 92,
                K_UpArrow = 93,
                K_LeftArrow = 94,
                K_DownArrow = 95,
                K_RightArrow = 96,
                K_WinLock = 97,
                K_Mute = 98,
                K_Stop = 99,
                K_ScanPreviousTrack = 100,
                K_PlayPause = 101,
                K_ScanNextTrack = 102,
                K_NumLock = 103,
                K_KeypadSlash = 104,
                K_KeypadAsterisk = 105,
                K_KeypadMinus = 106,
                K_KeypadPlus = 107,
                K_KeypadEnter = 108,
                K_Keypad7 = 109,
                K_Keypad8 = 110,
                K_Keypad9 = 111,
                K_KeypadComma = 112,
                K_Keypad4 = 113,
                K_Keypad5 = 114,
                K_Keypad6 = 115,
                K_Keypad1 = 116,
                K_Keypad2 = 117,
                K_Keypad3 = 118,
                K_Keypad0 = 119,
                K_KeypadPeriodAndDelete = 120,
                K_G1 = 121,
                K_G2 = 122,
                K_G3 = 123,
                K_G4 = 124,
                K_G5 = 125,
                K_G6 = 126,
                K_G7 = 127,
                K_G8 = 128,
                K_G9 = 129,
                K_G10 = 130,
                K_VolumeUp = 131,
                K_VolumeDown = 132,
                K_MR = 133,
                K_M1 = 134,
                K_M2 = 135,
                K_M3 = 136,
                K_G11 = 137,
                K_G12 = 138,
                K_G13 = 139,
                K_G14 = 140,
                K_G15 = 141,
                K_G16 = 142,
                K_G17 = 143,
                K_G18 = 144,
                K_International5 = 145,
                K_International4 = 146,
                K_Fn = 147,

                M_1 = 148,
                M_2 = 149,
                M_3 = 150,
                M_4 = 151,

                H_LeftLogo = 152,
                H_RightLogo = 153,

                K_Logo = 154,

                MM_Zone1 = 155,
                MM_Zone2 = 156,
                MM_Zone3 = 157,
                MM_Zone4 = 158,
                MM_Zone5 = 159,
                MM_Zone6 = 160,
                MM_Zone7 = 161,
                MM_Zone8 = 162,
                MM_Zone9 = 163,
                MM_Zone10 = 164,
                MM_Zone11 = 165,
                MM_Zone12 = 166,
                MM_Zone13 = 167,
                MM_Zone14 = 168,
                MM_Zone15 = 169,

                KLP_Zone1 = 170,
                KLP_Zone2 = 171,
                KLP_Zone3 = 172,
                KLP_Zone4 = 173,
                KLP_Zone5 = 174,
                KLP_Zone6 = 175,
                KLP_Zone7 = 176,
                KLP_Zone8 = 177,
                KLP_Zone9 = 178,
                KLP_Zone10 = 179,
                KLP_Zone11 = 180,
                KLP_Zone12 = 181,
                KLP_Zone13 = 182,
                KLP_Zone14 = 183,
                KLP_Zone15 = 184,
                KLP_Zone16 = 185,
                KLP_Zone17 = 186,
                KLP_Zone18 = 187,
                KLP_Zone19 = 188,

                M_5 = 189,
                M_6 = 190,

                HSS_Zone1 = 191,
                HSS_Zone2 = 192,
                HSS_Zone3 = 193,
                HSS_Zone4 = 194,
                HSS_Zone5 = 195,
                HSS_Zone6 = 196,
                HSS_Zone7 = 197,
                HSS_Zone8 = 198,
                HSS_Zone9 = 199,

                D_C1_1 = 200,
                D_C1_2 = 201,
                D_C1_3 = 202,
                D_C1_4 = 203,
                D_C1_5 = 204,
                D_C1_6 = 205,
                D_C1_7 = 206,
                D_C1_8 = 207,
                D_C1_9 = 208,
                D_C1_10 = 209,
                D_C1_11 = 210,
                D_C1_12 = 211,
                D_C1_13 = 212,
                D_C1_14 = 213,
                D_C1_15 = 214,
                D_C1_16 = 215,
                D_C1_17 = 216,
                D_C1_18 = 217,
                D_C1_19 = 218,
                D_C1_20 = 219,
                D_C1_21 = 220,
                D_C1_22 = 221,
                D_C1_23 = 222,
                D_C1_24 = 223,
                D_C1_25 = 224,
                D_C1_26 = 225,
                D_C1_27 = 226,
                D_C1_28 = 227,
                D_C1_29 = 228,
                D_C1_30 = 229,
                D_C1_31 = 230,
                D_C1_32 = 231,
                D_C1_33 = 232,
                D_C1_34 = 233,
                D_C1_35 = 234,
                D_C1_36 = 235,
                D_C1_37 = 236,
                D_C1_38 = 237,
                D_C1_39 = 238,
                D_C1_40 = 239,
                D_C1_41 = 240,
                D_C1_42 = 241,
                D_C1_43 = 242,
                D_C1_44 = 243,
                D_C1_45 = 244,
                D_C1_46 = 245,
                D_C1_47 = 246,
                D_C1_48 = 247,
                D_C1_49 = 248,
                D_C1_50 = 249,
                D_C1_51 = 250,
                D_C1_52 = 251,
                D_C1_53 = 252,
                D_C1_54 = 253,
                D_C1_55 = 254,
                D_C1_56 = 255,
                D_C1_57 = 256,
                D_C1_58 = 257,
                D_C1_59 = 258,
                D_C1_60 = 259,
                D_C1_61 = 260,
                D_C1_62 = 261,
                D_C1_63 = 262,
                D_C1_64 = 263,
                D_C1_65 = 264,
                D_C1_66 = 265,
                D_C1_67 = 266,
                D_C1_68 = 267,
                D_C1_69 = 268,
                D_C1_70 = 269,
                D_C1_71 = 270,
                D_C1_72 = 271,
                D_C1_73 = 272,
                D_C1_74 = 273,
                D_C1_75 = 274,
                D_C1_76 = 275,
                D_C1_77 = 276,
                D_C1_78 = 277,
                D_C1_79 = 278,
                D_C1_80 = 279,
                D_C1_81 = 280,
                D_C1_82 = 281,
                D_C1_83 = 282,
                D_C1_84 = 283,
                D_C1_85 = 284,
                D_C1_86 = 285,
                D_C1_87 = 286,
                D_C1_88 = 287,
                D_C1_89 = 288,
                D_C1_90 = 289,
                D_C1_91 = 290,
                D_C1_92 = 291,
                D_C1_93 = 292,
                D_C1_94 = 293,
                D_C1_95 = 294,
                D_C1_96 = 295,
                D_C1_97 = 296,
                D_C1_98 = 297,
                D_C1_99 = 298,
                D_C1_100 = 299,
                D_C1_101 = 300,
                D_C1_102 = 301,
                D_C1_103 = 302,
                D_C1_104 = 303,
                D_C1_105 = 304,
                D_C1_106 = 305,
                D_C1_107 = 306,
                D_C1_108 = 307,
                D_C1_109 = 308,
                D_C1_110 = 309,
                D_C1_111 = 310,
                D_C1_112 = 311,
                D_C1_113 = 312,
                D_C1_114 = 313,
                D_C1_115 = 314,
                D_C1_116 = 315,
                D_C1_117 = 316,
                D_C1_118 = 317,
                D_C1_119 = 318,
                D_C1_120 = 319,
                D_C1_121 = 320,
                D_C1_122 = 321,
                D_C1_123 = 322,
                D_C1_124 = 323,
                D_C1_125 = 324,
                D_C1_126 = 325,
                D_C1_127 = 326,
                D_C1_128 = 327,
                D_C1_129 = 328,
                D_C1_130 = 329,
                D_C1_131 = 330,
                D_C1_132 = 331,
                D_C1_133 = 332,
                D_C1_134 = 333,
                D_C1_135 = 334,
                D_C1_136 = 335,
                D_C1_137 = 336,
                D_C1_138 = 337,
                D_C1_139 = 338,
                D_C1_140 = 339,
                D_C1_141 = 340,
                D_C1_142 = 341,
                D_C1_143 = 342,
                D_C1_144 = 343,
                D_C1_145 = 344,
                D_C1_146 = 345,
                D_C1_147 = 346,
                D_C1_148 = 347,
                D_C1_149 = 348,
                D_C1_150 = 349,

                D_C2_1 = 350,
                D_C2_2 = 351,
                D_C2_3 = 352,
                D_C2_4 = 353,
                D_C2_5 = 354,
                D_C2_6 = 355,
                D_C2_7 = 356,
                D_C2_8 = 357,
                D_C2_9 = 358,
                D_C2_10 = 359,
                D_C2_11 = 360,
                D_C2_12 = 361,
                D_C2_13 = 362,
                D_C2_14 = 363,
                D_C2_15 = 364,
                D_C2_16 = 365,
                D_C2_17 = 366,
                D_C2_18 = 367,
                D_C2_19 = 368,
                D_C2_20 = 369,
                D_C2_21 = 370,
                D_C2_22 = 371,
                D_C2_23 = 372,
                D_C2_24 = 373,
                D_C2_25 = 374,
                D_C2_26 = 375,
                D_C2_27 = 376,
                D_C2_28 = 377,
                D_C2_29 = 378,
                D_C2_30 = 379,
                D_C2_31 = 380,
                D_C2_32 = 381,
                D_C2_33 = 382,
                D_C2_34 = 383,
                D_C2_35 = 384,
                D_C2_36 = 385,
                D_C2_37 = 386,
                D_C2_38 = 387,
                D_C2_39 = 388,
                D_C2_40 = 389,
                D_C2_41 = 390,
                D_C2_42 = 391,
                D_C2_43 = 392,
                D_C2_44 = 393,
                D_C2_45 = 394,
                D_C2_46 = 395,
                D_C2_47 = 396,
                D_C2_48 = 397,
                D_C2_49 = 398,
                D_C2_50 = 399,
                D_C2_51 = 400,
                D_C2_52 = 401,
                D_C2_53 = 402,
                D_C2_54 = 403,
                D_C2_55 = 404,
                D_C2_56 = 405,
                D_C2_57 = 406,
                D_C2_58 = 407,
                D_C2_59 = 408,
                D_C2_60 = 409,
                D_C2_61 = 410,
                D_C2_62 = 411,
                D_C2_63 = 412,
                D_C2_64 = 413,
                D_C2_65 = 414,
                D_C2_66 = 415,
                D_C2_67 = 416,
                D_C2_68 = 417,
                D_C2_69 = 418,
                D_C2_70 = 419,
                D_C2_71 = 420,
                D_C2_72 = 421,
                D_C2_73 = 422,
                D_C2_74 = 423,
                D_C2_75 = 424,
                D_C2_76 = 425,
                D_C2_77 = 426,
                D_C2_78 = 427,
                D_C2_79 = 428,
                D_C2_80 = 429,
                D_C2_81 = 430,
                D_C2_82 = 431,
                D_C2_83 = 432,
                D_C2_84 = 433,
                D_C2_85 = 434,
                D_C2_86 = 435,
                D_C2_87 = 436,
                D_C2_88 = 437,
                D_C2_89 = 438,
                D_C2_90 = 439,
                D_C2_91 = 440,
                D_C2_92 = 441,
                D_C2_93 = 442,
                D_C2_94 = 443,
                D_C2_95 = 444,
                D_C2_96 = 445,
                D_C2_97 = 446,
                D_C2_98 = 447,
                D_C2_99 = 448,
                D_C2_100 = 449,
                D_C2_101 = 450,
                D_C2_102 = 451,
                D_C2_103 = 452,
                D_C2_104 = 453,
                D_C2_105 = 454,
                D_C2_106 = 455,
                D_C2_107 = 456,
                D_C2_108 = 457,
                D_C2_109 = 458,
                D_C2_110 = 459,
                D_C2_111 = 460,
                D_C2_112 = 461,
                D_C2_113 = 462,
                D_C2_114 = 463,
                D_C2_115 = 464,
                D_C2_116 = 465,
                D_C2_117 = 466,
                D_C2_118 = 467,
                D_C2_119 = 468,
                D_C2_120 = 469,
                D_C2_121 = 470,
                D_C2_122 = 471,
                D_C2_123 = 472,
                D_C2_124 = 473,
                D_C2_125 = 474,
                D_C2_126 = 475,
                D_C2_127 = 476,
                D_C2_128 = 477,
                D_C2_129 = 478,
                D_C2_130 = 479,
                D_C2_131 = 480,
                D_C2_132 = 481,
                D_C2_133 = 482,
                D_C2_134 = 483,
                D_C2_135 = 484,
                D_C2_136 = 485,
                D_C2_137 = 486,
                D_C2_138 = 487,
                D_C2_139 = 488,
                D_C2_140 = 489,
                D_C2_141 = 490,
                D_C2_142 = 491,
                D_C2_143 = 492,
                D_C2_144 = 493,
                D_C2_145 = 494,
                D_C2_146 = 495,
                D_C2_147 = 496,
                D_C2_148 = 497,
                D_C2_149 = 498,
                D_C2_150 = 499,

                I_Oem1 = 500,
                I_Oem2 = 501,
                I_Oem3 = 502,
                I_Oem4 = 503,
                I_Oem5 = 504,
                I_Oem6 = 505,
                I_Oem7 = 506,
                I_Oem8 = 507,
                I_Oem9 = 508,
                I_Oem10 = 509,
                I_Oem11 = 510,
                I_Oem12 = 511,
                I_Oem13 = 512,
                I_Oem14 = 513,
                I_Oem15 = 514,
                I_Oem16 = 515,
                I_Oem17 = 516,
                I_Oem18 = 517,
                I_Oem19 = 518,
                I_Oem20 = 519,
                I_Oem21 = 520,
                I_Oem22 = 521,
                I_Oem23 = 522,
                I_Oem24 = 523,
                I_Oem25 = 524,
                I_Oem26 = 525,
                I_Oem27 = 526,
                I_Oem28 = 527,
                I_Oem29 = 528,
                I_Oem30 = 529,
                I_Oem31 = 530,
                I_Oem32 = 531,
                I_Oem33 = 532,
                I_Oem34 = 533,
                I_Oem35 = 534,
                I_Oem36 = 535,
                I_Oem37 = 536,
                I_Oem38 = 537,
                I_Oem39 = 538,
                I_Oem40 = 539,
                I_Oem41 = 540,
                I_Oem42 = 541,
                I_Oem43 = 542,
                I_Oem44 = 543,
                I_Oem45 = 544,
                I_Oem46 = 545,
                I_Oem47 = 546,
                I_Oem48 = 547,
                I_Oem49 = 548,
                I_Oem50 = 549,
                I_Oem51 = 550,
                I_Oem52 = 551,
                I_Oem53 = 552,
                I_Oem54 = 553,
                I_Oem55 = 554,
                I_Oem56 = 555,
                I_Oem57 = 556,
                I_Oem58 = 557,
                I_Oem59 = 558,
                I_Oem60 = 559,
                I_Oem61 = 560,
                I_Oem62 = 561,
                I_Oem63 = 562,
                I_Oem64 = 563,
                I_Oem65 = 564,
                I_Oem66 = 565,
                I_Oem67 = 566,
                I_Oem68 = 567,
                I_Oem69 = 568,
                I_Oem70 = 569,
                I_Oem71 = 570,
                I_Oem72 = 571,
                I_Oem73 = 572,
                I_Oem74 = 573,
                I_Oem75 = 574,
                I_Oem76 = 575,
                I_Oem77 = 576,
                I_Oem78 = 577,
                I_Oem79 = 578,
                I_Oem80 = 579,
                I_Oem81 = 580,
                I_Oem82 = 581,
                I_Oem83 = 582,
                I_Oem84 = 583,
                I_Oem85 = 584,
                I_Oem86 = 585,
                I_Oem87 = 586,
                I_Oem88 = 587,
                I_Oem89 = 588,
                I_Oem90 = 589,
                I_Oem91 = 590,
                I_Oem92 = 591,
                I_Oem93 = 592,
                I_Oem94 = 593,
                I_Oem95 = 594,
                I_Oem96 = 595,
                I_Oem97 = 596,
                I_Oem98 = 597,
                I_Oem99 = 598,
                I_Oem100 = 599,

                DRAM_1 = 600,
                DRAM_2 = 601,
                DRAM_3 = 602,
                DRAM_4 = 603,
                DRAM_5 = 604,
                DRAM_6 = 605,
                DRAM_7 = 606,
                DRAM_8 = 607,
                DRAM_9 = 608,
                DRAM_10 = 609,
                DRAM_11 = 610,
                DRAM_12 = 611,

                D_C3_1 = 612,
                D_C3_2 = 613,
                D_C3_3 = 614,
                D_C3_4 = 615,
                D_C3_5 = 616,
                D_C3_6 = 617,
                D_C3_7 = 618,
                D_C3_8 = 619,
                D_C3_9 = 620,
                D_C3_10 = 621,
                D_C3_11 = 622,
                D_C3_12 = 623,
                D_C3_13 = 624,
                D_C3_14 = 625,
                D_C3_15 = 626,
                D_C3_16 = 627,
                D_C3_17 = 628,
                D_C3_18 = 629,
                D_C3_19 = 630,
                D_C3_20 = 631,
                D_C3_21 = 632,
                D_C3_22 = 633,
                D_C3_23 = 634,
                D_C3_24 = 635,
                D_C3_25 = 636,
                D_C3_26 = 637,
                D_C3_27 = 638,
                D_C3_28 = 639,
                D_C3_29 = 640,
                D_C3_30 = 641,
                D_C3_31 = 642,
                D_C3_32 = 643,
                D_C3_33 = 644,
                D_C3_34 = 645,
                D_C3_35 = 646,
                D_C3_36 = 647,
                D_C3_37 = 648,
                D_C3_38 = 649,
                D_C3_39 = 650,
                D_C3_40 = 651,
                D_C3_41 = 652,
                D_C3_42 = 653,
                D_C3_43 = 654,
                D_C3_44 = 655,
                D_C3_45 = 656,
                D_C3_46 = 657,
                D_C3_47 = 658,
                D_C3_48 = 659,
                D_C3_49 = 660,
                D_C3_50 = 661,
                D_C3_51 = 662,
                D_C3_52 = 663,
                D_C3_53 = 664,
                D_C3_54 = 665,
                D_C3_55 = 666,
                D_C3_56 = 667,
                D_C3_57 = 668,
                D_C3_58 = 669,
                D_C3_59 = 670,
                D_C3_60 = 671,
                D_C3_61 = 672,
                D_C3_62 = 673,
                D_C3_63 = 674,
                D_C3_64 = 675,
                D_C3_65 = 676,
                D_C3_66 = 677,
                D_C3_67 = 678,
                D_C3_68 = 679,
                D_C3_69 = 680,
                D_C3_70 = 681,
                D_C3_71 = 682,
                D_C3_72 = 683,
                D_C3_73 = 684,
                D_C3_74 = 685,
                D_C3_75 = 686,
                D_C3_76 = 687,
                D_C3_77 = 688,
                D_C3_78 = 689,
                D_C3_79 = 690,
                D_C3_80 = 691,
                D_C3_81 = 692,
                D_C3_82 = 693,
                D_C3_83 = 694,
                D_C3_84 = 695,
                D_C3_85 = 696,
                D_C3_86 = 697,
                D_C3_87 = 698,
                D_C3_88 = 699,
                D_C3_89 = 700,
                D_C3_90 = 701,
                D_C3_91 = 702,
                D_C3_92 = 703,
                D_C3_93 = 704,
                D_C3_94 = 705,
                D_C3_95 = 706,
                D_C3_96 = 707,
                D_C3_97 = 708,
                D_C3_98 = 709,
                D_C3_99 = 710,
                D_C3_100 = 711,
                D_C3_101 = 712,
                D_C3_102 = 713,
                D_C3_103 = 714,
                D_C3_104 = 715,
                D_C3_105 = 716,
                D_C3_106 = 717,
                D_C3_107 = 718,
                D_C3_108 = 719,
                D_C3_109 = 720,
                D_C3_110 = 721,
                D_C3_111 = 722,
                D_C3_112 = 723,
                D_C3_113 = 724,
                D_C3_114 = 725,
                D_C3_115 = 726,
                D_C3_116 = 727,
                D_C3_117 = 728,
                D_C3_118 = 729,
                D_C3_119 = 730,
                D_C3_120 = 731,
                D_C3_121 = 732,
                D_C3_122 = 733,
                D_C3_123 = 734,
                D_C3_124 = 735,
                D_C3_125 = 736,
                D_C3_126 = 737,
                D_C3_127 = 738,
                D_C3_128 = 739,
                D_C3_129 = 740,
                D_C3_130 = 741,
                D_C3_131 = 742,
                D_C3_132 = 743,
                D_C3_133 = 744,
                D_C3_134 = 745,
                D_C3_135 = 746,
                D_C3_136 = 747,
                D_C3_137 = 748,
                D_C3_138 = 749,
                D_C3_139 = 750,
                D_C3_140 = 751,
                D_C3_141 = 752,
                D_C3_142 = 753,
                D_C3_143 = 754,
                D_C3_144 = 755,
                D_C3_145 = 756,
                D_C3_146 = 757,
                D_C3_147 = 758,
                D_C3_148 = 759,
                D_C3_149 = 760,
                D_C3_150 = 761,

                LC_C1_1 = 762,
                LC_C1_2 = 763,
                LC_C1_3 = 764,
                LC_C1_4 = 765,
                LC_C1_5 = 766,
                LC_C1_6 = 767,
                LC_C1_7 = 768,
                LC_C1_8 = 769,
                LC_C1_9 = 770,
                LC_C1_10 = 771,
                LC_C1_11 = 772,
                LC_C1_12 = 773,
                LC_C1_13 = 774,
                LC_C1_14 = 775,
                LC_C1_15 = 776,
                LC_C1_16 = 777,
                LC_C1_17 = 778,
                LC_C1_18 = 779,
                LC_C1_19 = 780,
                LC_C1_20 = 781,
                LC_C1_21 = 782,
                LC_C1_22 = 783,
                LC_C1_23 = 784,
                LC_C1_24 = 785,
                LC_C1_25 = 786,
                LC_C1_26 = 787,
                LC_C1_27 = 788,
                LC_C1_28 = 789,
                LC_C1_29 = 790,
                LC_C1_30 = 791,
                LC_C1_31 = 792,
                LC_C1_32 = 793,
                LC_C1_33 = 794,
                LC_C1_34 = 795,
                LC_C1_35 = 796,
                LC_C1_36 = 797,
                LC_C1_37 = 798,
                LC_C1_38 = 799,
                LC_C1_39 = 800,
                LC_C1_40 = 801,
                LC_C1_41 = 802,
                LC_C1_42 = 803,
                LC_C1_43 = 804,
                LC_C1_44 = 805,
                LC_C1_45 = 806,
                LC_C1_46 = 807,
                LC_C1_47 = 808,
                LC_C1_48 = 809,
                LC_C1_49 = 810,
                LC_C1_50 = 811,
                LC_C1_51 = 812,
                LC_C1_52 = 813,
                LC_C1_53 = 814,
                LC_C1_54 = 815,
                LC_C1_55 = 816,
                LC_C1_56 = 817,
                LC_C1_57 = 818,
                LC_C1_58 = 819,
                LC_C1_59 = 820,
                LC_C1_60 = 821,
                LC_C1_61 = 822,
                LC_C1_62 = 823,
                LC_C1_63 = 824,
                LC_C1_64 = 825,
                LC_C1_65 = 826,
                LC_C1_66 = 827,
                LC_C1_67 = 828,
                LC_C1_68 = 829,
                LC_C1_69 = 830,
                LC_C1_70 = 831,
                LC_C1_71 = 832,
                LC_C1_72 = 833,
                LC_C1_73 = 834,
                LC_C1_74 = 835,
                LC_C1_75 = 836,
                LC_C1_76 = 837,
                LC_C1_77 = 838,
                LC_C1_78 = 839,
                LC_C1_79 = 840,
                LC_C1_80 = 841,
                LC_C1_81 = 842,
                LC_C1_82 = 843,
                LC_C1_83 = 844,
                LC_C1_84 = 845,
                LC_C1_85 = 846,
                LC_C1_86 = 847,
                LC_C1_87 = 848,
                LC_C1_88 = 849,
                LC_C1_89 = 850,
                LC_C1_90 = 851,
                LC_C1_91 = 852,
                LC_C1_92 = 853,
                LC_C1_93 = 854,
                LC_C1_94 = 855,
                LC_C1_95 = 856,
                LC_C1_96 = 857,
                LC_C1_97 = 858,
                LC_C1_98 = 859,
                LC_C1_99 = 860,
                LC_C1_100 = 861,
                LC_C1_101 = 862,
                LC_C1_102 = 863,
                LC_C1_103 = 864,
                LC_C1_104 = 865,
                LC_C1_105 = 866,
                LC_C1_106 = 867,
                LC_C1_107 = 868,
                LC_C1_108 = 869,
                LC_C1_109 = 870,
                LC_C1_110 = 871,
                LC_C1_111 = 872,
                LC_C1_112 = 873,
                LC_C1_113 = 874,
                LC_C1_114 = 875,
                LC_C1_115 = 876,
                LC_C1_116 = 877,
                LC_C1_117 = 878,
                LC_C1_118 = 879,
                LC_C1_119 = 880,
                LC_C1_120 = 881,
                LC_C1_121 = 882,
                LC_C1_122 = 883,
                LC_C1_123 = 884,
                LC_C1_124 = 885,
                LC_C1_125 = 886,
                LC_C1_126 = 887,
                LC_C1_127 = 888,
                LC_C1_128 = 889,
                LC_C1_129 = 890,
                LC_C1_130 = 891,
                LC_C1_131 = 892,
                LC_C1_132 = 893,
                LC_C1_133 = 894,
                LC_C1_134 = 895,
                LC_C1_135 = 896,
                LC_C1_136 = 897,
                LC_C1_137 = 898,
                LC_C1_138 = 899,
                LC_C1_139 = 900,
                LC_C1_140 = 901,
                LC_C1_141 = 902,
                LC_C1_142 = 903,
                LC_C1_143 = 904,
                LC_C1_144 = 905,
                LC_C1_145 = 906,
                LC_C1_146 = 907,
                LC_C1_147 = 908,
                LC_C1_148 = 909,
                LC_C1_149 = 910,
                LC_C1_150 = 911,

                I_Last = LC_C1_150
            };

            public static DeviceKeys ToDeviceKey(CorsairLedId ledid)
            {
                switch (ledid)
                {
                    case (CorsairLedId.K_Logo):
                        return DeviceKeys.LOGO;
                    case (CorsairLedId.K_Brightness):
                        return DeviceKeys.BRIGHTNESS_SWITCH;
                    case (CorsairLedId.K_WinLock):
                        return DeviceKeys.LOCK_SWITCH;

                    case (CorsairLedId.K_Mute):
                        return DeviceKeys.VOLUME_MUTE;
                    case (CorsairLedId.K_VolumeUp):
                        return DeviceKeys.VOLUME_UP;
                    case (CorsairLedId.K_VolumeDown):
                        return DeviceKeys.VOLUME_DOWN;
                    case (CorsairLedId.K_Stop):
                        return DeviceKeys.MEDIA_STOP;
                    case (CorsairLedId.K_PlayPause):
                        return DeviceKeys.MEDIA_PLAY_PAUSE;
                    case (CorsairLedId.K_ScanPreviousTrack):
                        return DeviceKeys.MEDIA_PREVIOUS;
                    case (CorsairLedId.K_ScanNextTrack):
                        return DeviceKeys.MEDIA_NEXT;

                    case (CorsairLedId.K_Escape):
                        return DeviceKeys.ESC;
                    case (CorsairLedId.K_F1):
                        return DeviceKeys.F1;
                    case (CorsairLedId.K_F2):
                        return DeviceKeys.F2;
                    case (CorsairLedId.K_F3):
                        return DeviceKeys.F3;
                    case (CorsairLedId.K_F4):
                        return DeviceKeys.F4;
                    case (CorsairLedId.K_F5):
                        return DeviceKeys.F5;
                    case (CorsairLedId.K_F6):
                        return DeviceKeys.F6;
                    case (CorsairLedId.K_F7):
                        return DeviceKeys.F7;
                    case (CorsairLedId.K_F8):
                        return DeviceKeys.F8;
                    case (CorsairLedId.K_F9):
                        return DeviceKeys.F9;
                    case (CorsairLedId.K_F10):
                        return DeviceKeys.F10;
                    case (CorsairLedId.K_F11):
                        return DeviceKeys.F11;
                    case (CorsairLedId.K_F12):
                        return DeviceKeys.F12;
                    case (CorsairLedId.K_PrintScreen):
                        return DeviceKeys.PRINT_SCREEN;
                    case (CorsairLedId.K_ScrollLock):
                        return DeviceKeys.SCROLL_LOCK;
                    case (CorsairLedId.K_PauseBreak):
                        return DeviceKeys.PAUSE_BREAK;
                    case (CorsairLedId.K_GraveAccentAndTilde):
                        return DeviceKeys.TILDE;
                    case (CorsairLedId.K_1):
                        return DeviceKeys.ONE;
                    case (CorsairLedId.K_2):
                        return DeviceKeys.TWO;
                    case (CorsairLedId.K_3):
                        return DeviceKeys.THREE;
                    case (CorsairLedId.K_4):
                        return DeviceKeys.FOUR;
                    case (CorsairLedId.K_5):
                        return DeviceKeys.FIVE;
                    case (CorsairLedId.K_6):
                        return DeviceKeys.SIX;
                    case (CorsairLedId.K_7):
                        return DeviceKeys.SEVEN;
                    case (CorsairLedId.K_8):
                        return DeviceKeys.EIGHT;
                    case (CorsairLedId.K_9):
                        return DeviceKeys.NINE;
                    case (CorsairLedId.K_0):
                        return DeviceKeys.ZERO;
                    case (CorsairLedId.K_MinusAndUnderscore):
                        return DeviceKeys.MINUS;
                    case (CorsairLedId.K_EqualsAndPlus):
                        return DeviceKeys.EQUALS;
                    case (CorsairLedId.K_Backspace):
                        return DeviceKeys.BACKSPACE;
                    case (CorsairLedId.K_Insert):
                        return DeviceKeys.INSERT;
                    case (CorsairLedId.K_Home):
                        return DeviceKeys.HOME;
                    case (CorsairLedId.K_PageUp):
                        return DeviceKeys.PAGE_UP;
                    case (CorsairLedId.K_NumLock):
                        return DeviceKeys.NUM_LOCK;
                    case (CorsairLedId.K_KeypadSlash):
                        return DeviceKeys.NUM_SLASH;
                    case (CorsairLedId.K_KeypadAsterisk):
                        return DeviceKeys.NUM_ASTERISK;
                    case (CorsairLedId.K_KeypadMinus):
                        return DeviceKeys.NUM_MINUS;
                    case (CorsairLedId.K_Tab):
                        return DeviceKeys.TAB;
                    case (CorsairLedId.K_Q):
                        return DeviceKeys.Q;
                    case (CorsairLedId.K_W):
                        return DeviceKeys.W;
                    case (CorsairLedId.K_E):
                        return DeviceKeys.E;
                    case (CorsairLedId.K_R):
                        return DeviceKeys.R;
                    case (CorsairLedId.K_T):
                        return DeviceKeys.T;
                    case (CorsairLedId.K_Y):
                        return DeviceKeys.Y;
                    case (CorsairLedId.K_U):
                        return DeviceKeys.U;
                    case (CorsairLedId.K_I):
                        return DeviceKeys.I;
                    case (CorsairLedId.K_O):
                        return DeviceKeys.O;
                    case (CorsairLedId.K_P):
                        return DeviceKeys.P;
                    case (CorsairLedId.K_BracketLeft):
                        return DeviceKeys.OPEN_BRACKET;
                    case (CorsairLedId.K_BracketRight):
                        return DeviceKeys.CLOSE_BRACKET;
                    case (CorsairLedId.K_Backslash):
                        return DeviceKeys.BACKSLASH;
                    case (CorsairLedId.K_Delete):
                        return DeviceKeys.DELETE;
                    case (CorsairLedId.K_End):
                        return DeviceKeys.END;
                    case (CorsairLedId.K_PageDown):
                        return DeviceKeys.PAGE_DOWN;
                    case (CorsairLedId.K_Keypad7):
                        return DeviceKeys.NUM_SEVEN;
                    case (CorsairLedId.K_Keypad8):
                        return DeviceKeys.NUM_EIGHT;
                    case (CorsairLedId.K_Keypad9):
                        return DeviceKeys.NUM_NINE;
                    case (CorsairLedId.K_KeypadPlus):
                        return DeviceKeys.NUM_PLUS;
                    case (CorsairLedId.K_CapsLock):
                        return DeviceKeys.CAPS_LOCK;
                    case (CorsairLedId.K_A):
                        return DeviceKeys.A;
                    case (CorsairLedId.K_S):
                        return DeviceKeys.S;
                    case (CorsairLedId.K_D):
                        return DeviceKeys.D;
                    case (CorsairLedId.K_F):
                        return DeviceKeys.F;
                    case (CorsairLedId.K_G):
                        return DeviceKeys.G;
                    case (CorsairLedId.K_H):
                        return DeviceKeys.H;
                    case (CorsairLedId.K_J):
                        return DeviceKeys.J;
                    case (CorsairLedId.K_K):
                        return DeviceKeys.K;
                    case (CorsairLedId.K_L):
                        return DeviceKeys.L;
                    case (CorsairLedId.K_SemicolonAndColon):
                        return DeviceKeys.SEMICOLON;
                    case (CorsairLedId.K_ApostropheAndDoubleQuote):
                        return DeviceKeys.APOSTROPHE;
                    case (CorsairLedId.K_NonUsTilde):
                        return DeviceKeys.HASHTAG;
                    case (CorsairLedId.K_Enter):
                        return DeviceKeys.ENTER;
                    case (CorsairLedId.K_Keypad4):
                        return DeviceKeys.NUM_FOUR;
                    case (CorsairLedId.K_Keypad5):
                        return DeviceKeys.NUM_FIVE;
                    case (CorsairLedId.K_Keypad6):
                        return DeviceKeys.NUM_SIX;
                    case (CorsairLedId.K_LeftShift):
                        return DeviceKeys.LEFT_SHIFT;
                    case (CorsairLedId.K_NonUsBackslash):
                        return DeviceKeys.BACKSLASH_UK;
                    case (CorsairLedId.K_Z):
                        return DeviceKeys.Z;
                    case (CorsairLedId.K_X):
                        return DeviceKeys.X;
                    case (CorsairLedId.K_C):
                        return DeviceKeys.C;
                    case (CorsairLedId.K_V):
                        return DeviceKeys.V;
                    case (CorsairLedId.K_B):
                        return DeviceKeys.B;
                    case (CorsairLedId.K_N):
                        return DeviceKeys.N;
                    case (CorsairLedId.K_M):
                        return DeviceKeys.M;
                    case (CorsairLedId.K_CommaAndLessThan):
                        return DeviceKeys.COMMA;
                    case (CorsairLedId.K_PeriodAndBiggerThan):
                        return DeviceKeys.PERIOD;
                    case (CorsairLedId.K_SlashAndQuestionMark):
                        return DeviceKeys.FORWARD_SLASH;
                    case (CorsairLedId.K_RightShift):
                        return DeviceKeys.RIGHT_SHIFT;
                    case (CorsairLedId.K_UpArrow):
                        return DeviceKeys.ARROW_UP;
                    case (CorsairLedId.K_Keypad1):
                        return DeviceKeys.NUM_ONE;
                    case (CorsairLedId.K_Keypad2):
                        return DeviceKeys.NUM_TWO;
                    case (CorsairLedId.K_Keypad3):
                        return DeviceKeys.NUM_THREE;
                    case (CorsairLedId.K_KeypadEnter):
                        return DeviceKeys.NUM_ENTER;
                    case (CorsairLedId.K_LeftCtrl):
                        return DeviceKeys.LEFT_CONTROL;
                    case (CorsairLedId.K_LeftGui):
                        return DeviceKeys.LEFT_WINDOWS;
                    case (CorsairLedId.K_LeftAlt):
                        return DeviceKeys.LEFT_ALT;
                    case (CorsairLedId.K_Space):
                        return DeviceKeys.SPACE;
                    case (CorsairLedId.K_RightAlt):
                        return DeviceKeys.RIGHT_ALT;
                    case (CorsairLedId.K_RightGui):
                        return DeviceKeys.RIGHT_WINDOWS;
                    case (CorsairLedId.K_Application):
                        return DeviceKeys.APPLICATION_SELECT;
                    case (CorsairLedId.K_RightCtrl):
                        return DeviceKeys.RIGHT_CONTROL;
                    case (CorsairLedId.K_LeftArrow):
                        return DeviceKeys.ARROW_LEFT;
                    case (CorsairLedId.K_DownArrow):
                        return DeviceKeys.ARROW_DOWN;
                    case (CorsairLedId.K_RightArrow):
                        return DeviceKeys.ARROW_RIGHT;
                    case (CorsairLedId.K_Keypad0):
                        return DeviceKeys.NUM_ZERO;
                    case (CorsairLedId.K_KeypadPeriodAndDelete):
                        return DeviceKeys.NUM_PERIOD;

                    case (CorsairLedId.K_Fn):
                        return DeviceKeys.FN_Key;

                    case (CorsairLedId.K_G1):
                        return DeviceKeys.G1;
                    case (CorsairLedId.K_G2):
                        return DeviceKeys.G2;
                    case (CorsairLedId.K_G3):
                        return DeviceKeys.G3;
                    case (CorsairLedId.K_G4):
                        return DeviceKeys.G4;
                    case (CorsairLedId.K_G5):
                        return DeviceKeys.G5;
                    case (CorsairLedId.K_G6):
                        return DeviceKeys.G6;
                    case (CorsairLedId.K_G7):
                        return DeviceKeys.G7;
                    case (CorsairLedId.K_G8):
                        return DeviceKeys.G8;
                    case (CorsairLedId.K_G9):
                        return DeviceKeys.G9;
                    case (CorsairLedId.K_G10):
                        return DeviceKeys.G10;
                    case (CorsairLedId.K_G11):
                        return DeviceKeys.G11;
                    case (CorsairLedId.K_G12):
                        return DeviceKeys.G12;
                    case (CorsairLedId.K_G13):
                        return DeviceKeys.G13;
                    case (CorsairLedId.K_G14):
                        return DeviceKeys.G14;
                    case (CorsairLedId.K_G15):
                        return DeviceKeys.G15;
                    case (CorsairLedId.K_G16):
                        return DeviceKeys.G16;
                    case (CorsairLedId.K_G17):
                        return DeviceKeys.G17;
                    case (CorsairLedId.K_G18):
                        return DeviceKeys.G18;
                    default:
                        return DeviceKeys.NONE;
                }
            }
        }

        public static class Razer
        {
            public static readonly Dictionary<DeviceKeys, int[]> GenericKeyboard = new Dictionary<DeviceKeys, int[]>
            {
                {DeviceKeys.ESC, new int [] {0,1} },
                {DeviceKeys.F1, new int [] {0, 3} },
                {DeviceKeys.F2, new int [] {0, 4} },
                {DeviceKeys.F3, new int [] {0, 5} },
                {DeviceKeys.F4, new int [] {0, 6} },
                {DeviceKeys.F5, new int [] {0, 7} },
                {DeviceKeys.F6, new int [] {0, 8} },
                {DeviceKeys.F7, new int [] {0, 9} },
                {DeviceKeys.F8, new int [] {0, 10} },
                {DeviceKeys.F9, new int [] {0, 11} },
                {DeviceKeys.F10, new int [] {0, 12} },
                {DeviceKeys.F11, new int [] {0, 13} },
                {DeviceKeys.F12, new int [] {0, 14} },
                {DeviceKeys.PRINT_SCREEN, new int [] {0, 15} },
                {DeviceKeys.SCROLL_LOCK, new int [] {0, 16} },
                {DeviceKeys.PAUSE_BREAK, new int [] {0, 17} },
                {DeviceKeys.LOGO, new int [] {0, 20 } },
                {DeviceKeys.G1, new int [] {1, 0} },
                {DeviceKeys.TILDE, new int [] {1, 1} },
                {DeviceKeys.ONE, new int [] {1, 2} },
                {DeviceKeys.TWO, new int [] {1, 3} },
                {DeviceKeys.THREE, new int [] {1, 4} },
                {DeviceKeys.FOUR, new int [] {1, 5} },
                {DeviceKeys.FIVE, new int [] {1, 6} },
                {DeviceKeys.SIX, new int [] {1, 7} },
                {DeviceKeys.SEVEN, new int [] {1, 8} },
                {DeviceKeys.EIGHT, new int [] {1, 9} },
                {DeviceKeys.NINE, new int [] {1, 10} },
                {DeviceKeys.ZERO, new int [] {1, 11} },
                {DeviceKeys.MINUS, new int [] {1, 12} },
                {DeviceKeys.EQUALS, new int [] {1, 13} },
                {DeviceKeys.BACKSPACE, new int [] {1, 14} },
                {DeviceKeys.INSERT, new int [] {1, 15} },
                {DeviceKeys.HOME, new int [] {1, 16} },
                {DeviceKeys.PAGE_UP, new int [] {1, 17} },
                {DeviceKeys.NUM_LOCK, new int [] {1, 18} },
                {DeviceKeys.NUM_SLASH, new int [] {1, 19} },
                {DeviceKeys.NUM_ASTERISK, new int [] {1, 20} },
                {DeviceKeys.NUM_MINUS, new int [] {1, 21} },
                {DeviceKeys.G2, new int [] {2, 0} },
                {DeviceKeys.TAB, new int [] {2, 1} },
                {DeviceKeys.Q, new int [] {2, 2} },
                {DeviceKeys.W, new int [] {2, 3} },
                {DeviceKeys.E, new int [] {2, 4} },
                {DeviceKeys.R, new int [] {2, 5} },
                {DeviceKeys.T, new int [] {2, 6} },
                {DeviceKeys.Y, new int [] {2, 7} },
                {DeviceKeys.U, new int [] {2, 8} },
                {DeviceKeys.I, new int [] {2, 9} },
                {DeviceKeys.O, new int [] {2, 10} },
                {DeviceKeys.P, new int [] {2, 11} },
                {DeviceKeys.OPEN_BRACKET, new int [] {2, 12} },
                {DeviceKeys.CLOSE_BRACKET, new int [] {2,13} },
                {DeviceKeys.BACKSLASH, new int [] {2, 14} },
                {DeviceKeys.DELETE, new int [] {2, 15} },
                {DeviceKeys.END, new int [] {2, 16} },
                {DeviceKeys.PAGE_DOWN, new int [] {2, 17} },
                {DeviceKeys.NUM_SEVEN, new int [] {2, 18} },
                {DeviceKeys.NUM_EIGHT, new int [] {2, 19} },
                {DeviceKeys.NUM_NINE, new int [] {2, 20} },
                {DeviceKeys.NUM_PLUS, new int [] {2, 21} },
                {DeviceKeys.G3, new int [] {3, 0} },
                {DeviceKeys.CAPS_LOCK, new int [] {3, 1} },
                {DeviceKeys.A, new int [] {3, 2} },
                {DeviceKeys.S, new int [] {3, 3} },
                {DeviceKeys.D, new int [] {3, 4} },
                {DeviceKeys.F, new int [] {3, 5} },
                {DeviceKeys.G, new int [] {3, 6} },
                {DeviceKeys.H, new int [] {3, 7} },
                {DeviceKeys.J, new int [] {3, 8} },
                {DeviceKeys.K, new int [] {3, 9} },
                {DeviceKeys.L, new int [] {3, 10} },
                {DeviceKeys.SEMICOLON, new int [] {3, 11} },
                {DeviceKeys.APOSTROPHE, new int [] {3, 12} },
                {DeviceKeys.HASHTAG, new int [] {3, 13} },
                {DeviceKeys.ENTER, new int [] {3, 14} },
                {DeviceKeys.NUM_FOUR, new int [] {3, 18} },
                {DeviceKeys.NUM_FIVE, new int [] {3, 19} },
                {DeviceKeys.NUM_SIX, new int [] {3, 20} },
                {DeviceKeys.G4, new int [] {4, 0} },
                {DeviceKeys.LEFT_SHIFT, new int [] {4, 1} },
                {DeviceKeys.BACKSLASH_UK, new int [] {4, 2} },
                {DeviceKeys.Z, new int [] {4, 3} },
                {DeviceKeys.X, new int [] {4, 4} },
                {DeviceKeys.C, new int [] {4, 5} },
                {DeviceKeys.V, new int [] {4, 6} },
                {DeviceKeys.B, new int [] {4, 7} },
                {DeviceKeys.N, new int [] {4, 8} },
                {DeviceKeys.M, new int [] {4, 9} },
                {DeviceKeys.COMMA, new int [] {4, 10} },
                {DeviceKeys.PERIOD, new int [] {4, 11} },
                {DeviceKeys.FORWARD_SLASH, new int [] {4, 12} },
                {DeviceKeys.RIGHT_SHIFT, new int [] {4, 14} },
                {DeviceKeys.ARROW_UP, new int [] {4, 16} },
                {DeviceKeys.NUM_ONE, new int [] {4, 18} },
                {DeviceKeys.NUM_TWO, new int [] {4, 19} },
                {DeviceKeys.NUM_THREE, new int [] {4, 20} },
                {DeviceKeys.NUM_ENTER, new int [] {4, 21} },
                {DeviceKeys.G5, new int [] {5, 0} },
                {DeviceKeys.LEFT_CONTROL, new int [] {5, 1} },
                {DeviceKeys.LEFT_WINDOWS, new int [] {5, 2} },
                {DeviceKeys.LEFT_ALT, new int [] {5, 3} },
                {DeviceKeys.SPACE, new int [] {5, 7} },
                {DeviceKeys.RIGHT_ALT, new int [] {5, 11} },
                {DeviceKeys.RIGHT_WINDOWS, new int [] {5, 12} },
                {DeviceKeys.FN_Key, new int [] {5, 12} },
                {DeviceKeys.APPLICATION_SELECT, new int [] {5, 13} },
                {DeviceKeys.RIGHT_CONTROL, new int [] {5, 14} },
                {DeviceKeys.ARROW_LEFT, new int [] {5, 15} },
                {DeviceKeys.ARROW_DOWN, new int [] {5, 16} },
                {DeviceKeys.ARROW_RIGHT, new int [] {5, 17} },
                {DeviceKeys.NUM_ZERO, new int [] {5, 19} },
                {DeviceKeys.NUM_PERIOD, new int [] {5, 20} }
            };
        }
    }
}
