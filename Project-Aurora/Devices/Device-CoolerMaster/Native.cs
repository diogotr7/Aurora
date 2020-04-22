using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Device_CoolerMaster
{
    public static class Native
    {
        public enum DEVICE_INDEX
        {
            MKeys_L = 0,
            MKeys_S = 1,
            MKeys_L_White = 2,
            MKeys_M_White = 3,
            MMouse_L = 4,
            MMouse_S = 5,
            MKeys_M = 6,
            MKeys_S_White = 7,
            MM520 = 8,
            MM530 = 9,
            MK750 = 10,
            CK372 = 11,
            CK550_552 = 12,
            CK551 = 13,
            MM830 = 14,
            CK530 = 15,
            MK850 = 16,
            SK630 = 17,
            SK650 = 18,
            SK621 = 19,
            MK730 = 20,
            DEFAULT = 0xFFFF
        };

        public static IEnumerable<DEVICE_INDEX> DEVICES => (DEVICE_INDEX[])Enum.GetValues(typeof(DEVICE_INDEX));

        public enum LAYOUT_KEYBOARD
        {
            LAYOUT_UNINIT = 0,
            LAYOUT_US = 1,
            LAYOUT_EU = 2,
            LAYOUT_JP = 3
        };

        public enum EFF_INDEX
        {
            EFF_FULL_ON = 0, EFF_BREATH = 1, EFF_BREATH_CYCLE = 2, EFF_SINGLE = 3, EFF_WAVE = 4, EFF_RIPPLE = 5,
            EFF_CROSS = 6, EFF_RAIN = 7, EFF_STAR = 8, EFF_SNAKE = 9, EFF_REC = 10,

            EFF_SPECTRUM = 11, EFF_RAPID_FIRE = 12, EFF_INDICATOR = 13, //mouse Eff
            /// New Effect
            EFF_FIRE_BALL = 14, EFF_WATER_RIPPLE = 15, EFF_REACTIVE_PUNCH = 16,
            EFF_SNOWING = 17, EFF_HEART_BEAT = 18, EFF_REACTIVE_TORNADO = 19,
            ///Multi
            EFF_MULTI_1 = 0xE0, EFF_MULTI_2 = 0xE1, EFF_MULTI_3 = 0xE2, EFF_MULTI_4 = 0xE3,
            EFF_OFF = 0xFE
        };

        public const int MAX_LED_ROW = 8;

        public const int MAX_LED_COLUMN = 24;

        public const string dll = "SDKDLL.dll";

        public static readonly IReadOnlyList<DEVICE_INDEX> Mice = new List<DEVICE_INDEX>
        {
            DEVICE_INDEX.MMouse_L,
            DEVICE_INDEX.MMouse_S,
            DEVICE_INDEX.MM520,
            DEVICE_INDEX.MM530,
            DEVICE_INDEX.MM830
        };

        public static readonly IReadOnlyList<DEVICE_INDEX> Keyboards = new List<DEVICE_INDEX>
        {
            DEVICE_INDEX.MKeys_L,
            DEVICE_INDEX.MKeys_L_White,
            DEVICE_INDEX.MKeys_M,
            DEVICE_INDEX.MKeys_M_White,
            DEVICE_INDEX.MKeys_S,
            DEVICE_INDEX.MKeys_S_White,
            DEVICE_INDEX.MK750,
            DEVICE_INDEX.CK372,
            DEVICE_INDEX.CK550_552,
            DEVICE_INDEX.CK551,
            DEVICE_INDEX.MM830,
            DEVICE_INDEX.CK530,
            DEVICE_INDEX.MK850,
            DEVICE_INDEX.MK730,
            DEVICE_INDEX.SK621,
            DEVICE_INDEX.SK630,
            DEVICE_INDEX.SK650
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct KEY_COLOR
        {
            public byte r;
            public byte g;
            public byte b;

            public KEY_COLOR(byte _r, byte _g, byte _b)
            {
                r = _r;
                g = _g;
                b = _b;
            }

            public KEY_COLOR(System.Drawing.Color clr)
            {
                r = clr.R;
                g = clr.G;
                b = clr.B;
            }
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct COLOR_MATRIX
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_LED_ROW * MAX_LED_COLUMN, ArraySubType = UnmanagedType.Struct)]
            public KEY_COLOR[,] KeyColor;
        };

        public static COLOR_MATRIX NewColorMatrix() => new COLOR_MATRIX() { KeyColor = new KEY_COLOR[MAX_LED_ROW, MAX_LED_COLUMN] };

        #region DllImport
        [DllImport(dll)]
        public static extern int GetCM_SDK_DllVer();

        [DllImport(dll)]
        public static extern void SetControlDevice(DEVICE_INDEX devIndex);

        [DllImport(dll)]
        public static extern LAYOUT_KEYBOARD GetDeviceLayout(DEVICE_INDEX devIndex = DEVICE_INDEX.DEFAULT);

        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool IsDevicePlug(DEVICE_INDEX devIndex = DEVICE_INDEX.DEFAULT);

        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool EnableLedControl(bool bEnable, DEVICE_INDEX devIndex = DEVICE_INDEX.DEFAULT);

        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool SwitchLedEffect(EFF_INDEX iEffectIndex, DEVICE_INDEX devIndex = DEVICE_INDEX.DEFAULT);

        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool RefreshLed(bool bAuto = false, DEVICE_INDEX devIndex = DEVICE_INDEX.DEFAULT);

        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool SetFullLedColor(byte r, byte g, byte b, DEVICE_INDEX devIndex = DEVICE_INDEX.DEFAULT);

        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool SetAllLedColor(COLOR_MATRIX colorMatrix, DEVICE_INDEX devIndex = DEVICE_INDEX.DEFAULT);

        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool SetLedColor(int iRow, int iColumn, byte r, byte g, byte b, DEVICE_INDEX devIndex = DEVICE_INDEX.DEFAULT);
        #endregion
    }
}
