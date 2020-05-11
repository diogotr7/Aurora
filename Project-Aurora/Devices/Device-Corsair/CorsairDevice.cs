using Aurora.Devices;
using CorsairRGB.NET;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace Device_Corsair
{
    public class CorsairDevice : Device
    {
        protected override string DeviceName => "Corsair";
        private readonly List<CorsairDeviceInfo> deviceInfos = new List<CorsairDeviceInfo>();

        public override string GetDeviceDetails() => IsInitialized() ?
                                                    DeviceName + ": " + GetSubDeviceDetails() :
                                                    DeviceName + ": Not Initialized";

        protected override bool InitializeImpl()
        {
            CUE.PerformProtocolHandshake();
            var error = CUE.GetLastError();
            if (error != CorsairError.Success)
            {
                LogError("Corsair Error: " + error);
                return false;
            }

            for (int i = 0; i < CUE.GetDeviceCount(); i++)
                deviceInfos.Add(CUE.GetDeviceInfo(i));

            if (!CUE.RequestControl())
            {
                LogError("Error requesting cuesdk exclusive control:" + CUE.GetLastError());
                return false;
            }

            return true;
        }

        protected override void ShutdownImpl()
        {
            deviceInfos.Clear();
            CUE.ReleaseControl();
        }

        public override bool UpdateDevice(Dictionary<DeviceKeys, Color> keyColors, DoWorkEventArgs e, bool forced = false)
        {
            for (int i = 0; i < deviceInfos.Count; i++)
                SetDeviceColors(deviceInfos[i].Type, i, keyColors);

            return CUE.Update();
        }

        private bool SetDeviceColors(CorsairDeviceType type, int index, Dictionary<DeviceKeys, Color> keyColors)
        {
            List<CorsairLedColor> colors = new List<CorsairLedColor>();

            if (LedMaps.MapsMap.TryGetValue(type, out var dict) && dict.Count != 0)
            {
                foreach (var led in keyColors)
                {
                    if (dict.TryGetValue(led.Key, out var ledid))
                    {
                        colors.Add(new CorsairLedColor()
                        {
                            LedId = ledid,
                            R = led.Value.R,
                            G = led.Value.G,
                            B = led.Value.B
                        });
                    }
                }
            }
            else
            {
                if (keyColors.TryGetValue(DeviceKeys.Peripheral_Logo, out var clr))
                {
                    foreach (CorsairLedId led in LedMaps.DIYLeds)
                    {
                        colors.Add(new CorsairLedColor()
                        {
                            LedId = led,
                            R = clr.R,
                            G = clr.G,
                            B = clr.B
                        });
                    }
                }
            }

            if (colors.Count == 0)
                return false;

            return CUE.SetDeviceColors(index, colors.ToArray());
        }

        private string GetSubDeviceDetails()
        {
            StringBuilder a = new StringBuilder();
            for (int i = 0; i < deviceInfos.Count; i++)
            {
                a.Append(deviceInfos[i].Model);
                if (deviceInfos[i].Channels.ChannelsCount != 0)
                    a.Append(": ");

                for (int j = 0; j < deviceInfos[i].Channels.Channels.Length; j++)
                {
                    CorsairChannelInfo channels = deviceInfos[i].Channels.Channels[j];
                    for (int k = 0; k < channels.Devices.Length; k++)
                    {
                        a.Append(channels.Devices[k].Type);
                        if (k != channels.Devices.Length - 1)
                            a.Append(", ");
                    }
                    if (j != deviceInfos[i].Channels.Channels.Length - 1)
                        a.Append(", ");
                }
                if (i != deviceInfos.Count - 1)
                    a.Append("; ");
            }
            return a.ToString();
        }
    }
}
