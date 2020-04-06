using Aurora.Devices;
using HidLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device_UnifiedHID
{
    internal class RoccatTyon : UnifiedHIDBaseDevice
    {
        private static HidDevice ctrl_device_leds;
        private static HidDevice ctrl_device;

        protected override string DeviceName => "Roccat Tyon";
        protected override AuroraDeviceType AuroraDeviceType => AuroraDeviceType.Mouse;

        private bool InitMouseColor()
        {
            return ctrl_device.WriteFeatureData(initPacket);
        }

        static bool WaitCtrlDevice()
        {
            for (int i = 1; i < 3; i++) // 3 Tries because the first one always fails.
            {
                if (ctrl_device.ReadFeatureData(out byte[] buffer, 0x04) && buffer.Length > 2)
                {
                    if (buffer[1] == 0x01)
                        return true;
                }
                else
                    return false;
            }
            return false;
        }

        protected override bool ConnectImpl()
        {
            if (!Aurora.Global.Configuration.VarRegistry.GetVariable<bool>($"UnifiedHID_{this.GetType().Name}_enable"))
            {
                return false;
            }
            IEnumerable<HidDevice> devices = HidDevices.Enumerate(0x1E7D, new int[] { 0x2E4A });
            try
            {
                if (devices.Count() > 0)
                {
                    ctrl_device_leds = devices.First(dev => dev.Capabilities.UsagePage == 0x0001 && dev.Capabilities.Usage == 0x0002);
                    ctrl_device = devices.First(dev => dev.Capabilities.FeatureReportByteLength > 50);
                    ctrl_device.OpenDevice();
                    ctrl_device_leds.OpenDevice();
                    bool success = InitMouseColor() && WaitCtrlDevice();
                    if (!success)
                    {
                        LogError($"Roccat Tyon Could not connect\n");
                        ctrl_device.CloseDevice();
                        ctrl_device_leds.CloseDevice();
                    }
                    LogInfo($"Roccat Tyon Connected\n");
                    return success;
                }
            }
            catch (Exception exc)
            {
                LogError($"Error when attempting to open UnifiedHID device:\n{exc}");
            }
            return false;
        }

        // We need to override Disconnect() too cause we have two HID devices open for this mouse.
        protected override void DisconnectImpl()
        {
            try
            {
                ctrl_device.CloseDevice();
                ctrl_device_leds.CloseDevice();
            }
            catch (Exception exc)
            {
                LogError($"Error when attempting to close UnifiedHID device:\n{exc}");
            }
        }

        public bool SetLEDColour(Color color)
        {
            try
            {

                byte[] hwmap =
                {
                    color.R,
                    color.G,
                    color.B,
                    0x00,
                    0x00,
                    color.R,
                    color.G,
                    color.B,
                    0x00,
                    0x80,
                    0x80
                };

                byte[] workbuf = new byte[30];
                Array.Copy(controlPacket, 0, workbuf, 0, controlPacket.Length);
                Array.Copy(hwmap, 0, workbuf, controlPacket.Length, hwmap.Length);

                return ctrl_device.WriteFeatureData(workbuf);
            }
            catch (Exception exc)
            {
                LogError($"Error when attempting to close UnifiedHID device:\n{exc}");
                return false;
            }
        }

        protected override bool UpdateDeviceImpl(DeviceColorComposition composition)
        {
            foreach (var key in composition.keyColors)
            {
                if (key.Key == DeviceKeys.Peripheral_Logo || key.Key == DeviceKeys.Peripheral_ScrollWheel || key.Key == DeviceKeys.Peripheral_FrontLight)
                {
                    return SetLEDColour(CorrectAlpha(key.Value));
                }
            }
            return false;
        }

        // Packet with values set to white for mouse initialisation.
        static readonly byte[] initPacket = new byte[] {
            0x06,0x1e,0x00,0x00,
            0x06,0x06,0x06,0x10,0x20,0x40,0x80,0xa4,0x02,0x03,0x33,0x00,0x01,0x01,0x03,
            0xff,0xff,0xff,0x00,0x00,0xff,0xff,0xff,0x00,0x01,0x08
        };

        // Packet with fixed values for affixing to mouse colors.
        static readonly byte[] controlPacket = new byte[] {
            0x06,0x1e,0x00,0x00,
            0x06,0x06,0x06,0x10,0x20,0x40,0x80,0xa4,0x02,0x03,0x33,0x00,0x01,0x01,0x03
        };

    }
}
