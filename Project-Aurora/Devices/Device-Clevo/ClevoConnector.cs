using Aurora;
using Aurora.Devices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device_Clevo
{
    public class ClevoConnector : AuroraDeviceConnector
    {
        protected override string ConnectorName => "Clevo";

        protected override bool InitializeImpl()
        {
            devices.Add(new ClevoDevice());
            return true;
        }

        protected override void ShutdownImpl()
        {
        }
    }
    public class ClevoDevice : AuroraDevice
    {
        // Settings
        // TODO: Theese settings could be implemented with posibility of configuration from the Aurora GUI (Or external JSON, INI, Settings, etc)
        private bool useGlobalPeriphericColors = false;
        private bool useTouchpad = true;
        private bool updateLightsOnLogon = true;

        private Color ColorKBCenter = Color.Black;
        private Color ColorKBLeft = Color.Black;
        private Color ColorKBRight = Color.Black;
        private Color ColorTouchpad = Color.Black;
        private bool ColorUpdated;
        private Color LastColorKBCenter = Color.Black;
        private Color LastColorKBLeft = Color.Black;
        private Color LastColorKBRight = Color.Black;
        private Color LastColorTouchpad = Color.Black;

        // Clevo Controll Class
        private ClevoSetKBLED clevo = new ClevoSetKBLED();

        // Session Switch Handler
        private SessionSwitchEventHandler sseh;

        protected override string DeviceName => "Clevo Keyboard";

        protected override AuroraDeviceType AuroraDeviceType => AuroraDeviceType.Keyboard;

        protected override bool ConnectImpl()
        {
            // Initialize Clevo WMI Interface Connection
            if (!clevo.Initialize())
            {
                throw new Exception("Could not connect to Clevo WMI Interface");
            }

            // Update Lights on Logon (Clevo sometimes resets the lights when you Hibernate, this would fix wrong colors)
            if (updateLightsOnLogon)
            {
                sseh = new SessionSwitchEventHandler(SystemEvents_SessionSwitch);
                SystemEvents.SessionSwitch += sseh;
            }
            return true;
        }
        protected override void DisconnectImpl()
        {
            // Release Clevo Connection
            clevo.ResetKBLEDColors();
            clevo.Release();

            // Uninstantiate Session Switch
            if (sseh != null)
            {
                SystemEvents.SessionSwitch -= sseh;
                sseh = null;
            }
        }
        // Handle Logon Event
        void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            if (this.IsConnected() && e.Reason.Equals(SessionSwitchReason.SessionUnlock))
            { // Only Update when Logged In
                SendColorsToKeyboard(true);
            }
        }
        protected override bool UpdateDeviceImpl(DeviceColorComposition composition)
        {

            bool update_result = false;

            var keyColors = composition.keyColors;
            try
            {
                foreach (KeyValuePair<DeviceKeys, Color> pair in keyColors)
                {
                    if (useGlobalPeriphericColors)
                    {
                        if (pair.Key == DeviceKeys.Peripheral) // This is not working anymore. Was working in MASTER
                        {
                            ColorKBLeft = pair.Value;
                            ColorKBCenter = pair.Value;
                            ColorKBRight = pair.Value;
                            ColorTouchpad = pair.Value;
                            ColorUpdated = true;
                        }
                    }
                    else
                    {
                        // TouchPad (It would be nice to have a Touchpad Peripheral)
                        if (pair.Key == DeviceKeys.Peripheral)
                        {
                            ColorTouchpad = pair.Value;
                            ColorUpdated = true;
                        }
                    }
                }

                if (!useGlobalPeriphericColors)
                {
                    // Clevo 3 region keyboard

                    // Left Side (From ESC to Half Spacebar)
                    BitmapRectangle keymap_esc = Effects.GetBitmappingFromDeviceKey(DeviceKeys.ESC);
                    BitmapRectangle keymap_space = Effects.GetBitmappingFromDeviceKey(DeviceKeys.SPACE);
                    PointF spacebar_center = keymap_space.Center; // Key Center

                    int spacebar_x = (int)spacebar_center.X - keymap_esc.Left;
                    int height = (int)spacebar_center.Y - keymap_esc.Top;

                    BitmapRectangle region_left =
                        new BitmapRectangle(keymap_esc.Left, keymap_esc.Top, spacebar_x, height);

                    Color RegionLeftColor;

                    lock (composition.bitmapLock)
                        RegionLeftColor = Aurora.Utils.BitmapUtils.GetRegionColor(composition.keyBitmap, region_left);

                    if (!ColorKBLeft.Equals(RegionLeftColor))
                    {
                        ColorKBLeft = RegionLeftColor;
                        ColorUpdated = true;
                    }

                    // Center (Other Half of Spacebar to F11) - Clevo keyboards are very compact and the right side color bleeds over to the up/left/right/down keys)
                    BitmapRectangle keymap_f11 = Effects.GetBitmappingFromDeviceKey(DeviceKeys.F11);

                    int f11_x_width = Convert.ToInt32(keymap_f11.Center.X - spacebar_x);

                    BitmapRectangle region_center =
                        new BitmapRectangle(spacebar_x, keymap_esc.Top, f11_x_width, height);

                    Color RegionCenterColor;
                    lock (composition.bitmapLock)
                        RegionCenterColor = Aurora.Utils.BitmapUtils.GetRegionColor(composition.keyBitmap, region_center);

                    if (!ColorKBCenter.Equals(RegionCenterColor))
                    {
                        ColorKBCenter = RegionCenterColor;
                        ColorUpdated = true;
                    }

                    // Right Side
                    BitmapRectangle keymap_numenter = Effects.GetBitmappingFromDeviceKey(DeviceKeys.NUM_ENTER);
                    BitmapRectangle region_right = new BitmapRectangle(Convert.ToInt32(keymap_f11.Center.X),
                        keymap_esc.Top, Convert.ToInt32(keymap_numenter.Center.X - keymap_f11.Center.X), height);

                    Color RegionRightColor;
                    lock (composition.bitmapLock)
                        RegionRightColor = Aurora.Utils.BitmapUtils.GetRegionColor(composition.keyBitmap, region_right);

                    if (!ColorKBRight.Equals(RegionRightColor))
                    {
                        ColorKBRight = RegionRightColor;
                        ColorUpdated = true;
                    }

                }

                SendColorsToKeyboard();
                update_result = true;
            }
            catch (Exception exception)
            {
                LogError("Clevo device, error when updating device. Error: " + exception);
                update_result = false;
            }

            return update_result;
        }
        private void SendColorsToKeyboard(bool forced = false)
        {
            if (forced || ColorUpdated)
            {
                if ((forced || !LastColorKBLeft.Equals(ColorKBLeft)) && !Global.Configuration.devices_disable_keyboard)
                {
                    // MYSTERY: // Why is it B,R,G instead of R,G,B? SetKBLED uses R,G,B but only B,R,G returns the correct colors. Is bitshifting different in C# than in C++?
                    clevo.SetKBLED(ClevoSetKBLED.KBLEDAREA.ColorKBLeft, ColorKBLeft.B, ColorKBLeft.R, ColorKBLeft.G, (double)(ColorKBLeft.A / 0xff));
                    LastColorKBLeft = ColorKBLeft;
                }
                if ((forced || !LastColorKBCenter.Equals(ColorKBCenter)) && !Global.Configuration.devices_disable_keyboard)
                {
                    clevo.SetKBLED(ClevoSetKBLED.KBLEDAREA.ColorKBCenter, ColorKBCenter.B, ColorKBCenter.R, ColorKBCenter.G, (double)(ColorKBCenter.A / 0xff));
                    LastColorKBCenter = ColorKBCenter;
                }
                if ((forced || !LastColorKBRight.Equals(ColorKBRight)) && !Global.Configuration.devices_disable_keyboard)
                {
                    clevo.SetKBLED(ClevoSetKBLED.KBLEDAREA.ColorKBRight, ColorKBRight.B, ColorKBRight.R, ColorKBRight.G, (double)(ColorKBRight.A / 0xff));
                    LastColorKBRight = ColorKBRight;
                }
                if ((forced || (useTouchpad && !LastColorTouchpad.Equals(ColorTouchpad))) && !Global.Configuration.devices_disable_mouse)
                {
                    clevo.SetKBLED(ClevoSetKBLED.KBLEDAREA.ColorTouchpad, ColorTouchpad.B, ColorTouchpad.R, ColorTouchpad.G, (double)(ColorTouchpad.A / 0xff));
                    LastColorTouchpad = ColorTouchpad;
                }
                ColorUpdated = false;
            }
        }
    }
}
