using Aurora.Devices;
using CorsairRGB.NET;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace Device_Corsair
{
    public class CorsairDevice : Device
    {
        protected override string DeviceName => "Corsair";
        private readonly List<CorsairDeviceInfo> deviceInfos = new List<CorsairDeviceInfo>();

        public override string GetDeviceDetails() => isInitialized ?
                                                    DeviceName + ": " + GetSubDeviceDetails() :
                                                    DeviceName + ": Not Initialized";

        public override bool Initialize()
        {
            CUE.PerformProtocolHandshake();
            var error = CUE.GetLastError();
            if (error != CorsairError.Success)
            {
                LogError("Corsair Error: " + error);
                return isInitialized = false;
            }

            for (int i = 0; i < CUE.GetDeviceCount(); i++)
                deviceInfos.Add(CUE.GetDeviceInfo(i));

            if (!CUE.RequestControl())
            {
                LogError("Error requesting cuesdk exclusive control:" + CUE.GetLastError());
                return isInitialized = false;
            }

            return isInitialized = true;
        }

        public override void Shutdown()
        {
            isInitialized = false;
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
            var dict = GetLedMap(type);
            if (dict.Count == 0)
                return false;

            List<CorsairLedColor> colors = new List<CorsairLedColor>();
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
            return CUE.SetDeviceColors(index, colors.ToArray());
        }

        private Dictionary<DeviceKeys, CorsairLedId> GetLedMap(CorsairDeviceType t)
        {
            switch (t)
            {
                case CorsairDeviceType.Keyboard:
                    return LedMaps.KeyboardLedMap;
                case CorsairDeviceType.Mouse:
                    return LedMaps.MouseLedMap;
                case CorsairDeviceType.MouseMat:
                    return LedMaps.MouseMatLedMap;
                case CorsairDeviceType.HeadsetStand:
                    return LedMaps.HeadsetStandLedMap;
                case CorsairDeviceType.Headset:
                    return LedMaps.HeadsetLedMap;
                case CorsairDeviceType.Cooler:
                    return LedMaps.CoolerLedMap;
                case CorsairDeviceType.CommanderPro:
                case CorsairDeviceType.LightingNodePro:
                case CorsairDeviceType.MemoryModule:
                default:
                    return new Dictionary<DeviceKeys, CorsairLedId>();
            }
        }

        private string GetSubDeviceDetails()
        {
            var ret = string.Join(", ", deviceInfos.Select(d => d.Model));
            foreach (var channels in deviceInfos.Select(d => d.Channels.Channels))
            {
                foreach (var channel in channels)
                {
                    foreach(var dev in channel.Devices)
                    {
                        ret += " " + dev.Type;
                    }
                }
            }
            return ret;
        }
    }
}
