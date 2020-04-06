using Aurora.Devices;
using CorsairRGB.NET;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace Device_Corsair
{
    public class CorsairDeviceConnector : AuroraDeviceConnector
    {
        protected override string ConnectorName => "Corsair";
        private readonly List<CorsairDeviceInfo> deviceInfos = new List<CorsairDeviceInfo>();

        protected override string ConnectorSubDetails => GetSubDeviceDetails();

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
            for (int i = 0; i < deviceInfos.Count; i++)
                devices.Add(new CorsairDevice(deviceInfos[i], i));

            return devices.Any();
        }

        protected override void RunFirstTime()
        {
            Aurora.App.Current.Dispatcher.Invoke(() =>
            {
                CorsairInstallInstructions instructions = new CorsairInstallInstructions();
                instructions.ShowDialog();
            });
        }
        protected override void ShutdownImpl()
        {
            deviceInfos.Clear();
            CUE.ReleaseControl();
        }

        protected override void UpdateDevices()
        {
            CUE.Update();
        }

        private string GetSubDeviceDetails()
        {
            var ret = string.Join(", ", deviceInfos.Select(d => d.Model));
            foreach (var channels in deviceInfos.Select(d => d.Channels.Channels))
            {
                foreach (var channel in channels)
                {
                    foreach (var dev in channel.Devices)
                    {
                        ret += " " + dev.Type;
                    }
                }
            }
            return ret;
        }
    }
    public class CorsairDevice : AuroraDevice
    {
        CorsairDeviceInfo DeviceInfo;
        int Index;
        protected override string DeviceName => DeviceInfo.Model;

        public CorsairDevice(CorsairDeviceInfo deviceInfo, int index)
        {
            DeviceInfo = deviceInfo;
            Index = index;
        }
        protected override AuroraDeviceType AuroraDeviceType => GetDeviceType(DeviceInfo.Type);
        
        private AuroraDeviceType GetDeviceType(CorsairDeviceType t)
        {
            switch (t)
            {
                case CorsairDeviceType.Keyboard:
                    return AuroraDeviceType.Keyboard;
                case CorsairDeviceType.Mouse:
                    return AuroraDeviceType.Mouse;
                case CorsairDeviceType.MouseMat:
                    return AuroraDeviceType.Mousepad;
                case CorsairDeviceType.HeadsetStand:
                    return AuroraDeviceType.HeadsetStand;
                case CorsairDeviceType.Headset:
                    return AuroraDeviceType.Headset;
                case CorsairDeviceType.Cooler:
                //return AuroraDeviceType.Cooler;
                case CorsairDeviceType.CommanderPro:
                case CorsairDeviceType.LightingNodePro:
                case CorsairDeviceType.MemoryModule:
                default:
                    return AuroraDeviceType.Unkown;
            }
        }
    protected override bool UpdateDeviceImpl(DeviceColorComposition composition)
    {
            var dict = GetLedMap(DeviceInfo.Type);
            if (dict.Count == 0)
                return false;

            List<CorsairLedColor> colors = new List<CorsairLedColor>();
            foreach (var led in composition.keyColors)
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
            return CUE.SetDeviceColors(Index, colors.ToArray());
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
    }

}
