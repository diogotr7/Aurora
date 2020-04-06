using NZXTSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aurora.Settings;
using System.ComponentModel;
using Color = System.Drawing.Color;
using System.Diagnostics;
using Aurora.Devices;

namespace Device_NZXT
{
    public class NZXTDeviceData
    {
        public byte[] Colors { get; private set; }
        public bool Changed { get; set; }

        public NZXTDeviceData(int leds)
        {
            Colors = new byte[leds * 3];
            Changed = false;
        }

        public void SetColor(int index, Color clr)
        {
            if (index % 3 != 0 || index < 0 || index > Colors.Length - 2)
                throw new ArgumentOutOfRangeException();

            UpdateValueAt(index, clr.G);
            UpdateValueAt(index + 1,clr.R);
            UpdateValueAt(index + 2,clr.B);
        }

        private void UpdateValueAt(int index, byte value)
        {
            if(Colors[index] != value)
            {
                Changed = true;
                Colors[index] = value;
            }
        }
    }
    public class NZXTConnector : AuroraDeviceConnector
    {
        private DeviceLoader DeviceLoader;

        protected override string ConnectorName => "NZXT";

        protected override bool InitializeImpl()
        {
            if (Process.GetProcessesByName("NZXT CAM").Length > 0)
            {
                LogError("NZXT CAM is running. Ensure that it is not open and try again.");
                return false;
            }
            DeviceLoader = new DeviceLoader(false, DeviceLoadFilter.LightingControllers);
            DeviceLoader.ThrowExceptions = false;
            DeviceLoader.Initialize();

            if (DeviceLoader.NumDevices == 0)
            {
                LogError("NZXT device error: No devices found");
            }
            else
            {
                LogInfo("Starting NZXT debug information: Windows Build version: " + System.Runtime.InteropServices.RuntimeInformation.OSDescription);

                foreach (var device in DeviceLoader.Devices)
                {
                    if (device is NZXTSharp.KrakenX.KrakenX)
                    {
                        LogInfo("Found KrakenX, firmware version: " + (device as NZXTSharp.KrakenX.KrakenX).FirmwareVersion ?? "");
                        devices.Add(new KrakenXDevice(device));
                    }
                    if (device is NZXTSharp.HuePlus.HuePlus)
                    {
                        LogInfo("Found HuePlus, firmware version: " + (device as NZXTSharp.HuePlus.HuePlus).FirmwareVersion ?? "");
                        devices.Add(new HuePlusDevice(device));
                    }
                        
                }

                return true;
            }
            return false;
        }

        protected override void ShutdownImpl()
        {
            DeviceLoader.Dispose();
        }
    }

    class KrakenXDevice : AuroraDevice
    {
        private NZXTSharp.KrakenX.KrakenX Device;
        private NZXTDeviceData kraken = new NZXTDeviceData(8);
        private NZXTDeviceData logo = new NZXTDeviceData(1);

        protected override string DeviceName => "KrakenX";

        protected override AuroraDeviceType AuroraDeviceType => AuroraDeviceType.Unkown;

        public KrakenXDevice(INZXTDevice device)
        {
            Device = (device as NZXTSharp.KrakenX.KrakenX);
        }

        private int GetLedIndex(Dictionary<DeviceKeys, int> layout, DeviceKeys key)
        {
            if (layout.ContainsKey(key))
                return layout[key];

            return -1;
        }

        protected override bool UpdateDeviceImpl(DeviceColorComposition composition)
        {
            foreach (KeyValuePair<DeviceKeys, Color> key in composition.keyColors)
            {
                int index;

                if (key.Key == DeviceKeys.Peripheral_Logo)
                {
                    logo.SetColor(0, key.Value);
                }

                if ((index = GetLedIndex(NZXTLayoutMap.KrakenX, key.Key)) != -1)
                {
                    kraken.SetColor(index, key.Value);
                }
            }
            if (kraken.Changed)
            {
                Device?.ApplyEffect(Device.Ring, new NZXTSharp.Fixed(kraken.Colors));
                kraken.Changed = false;
            }

            if (logo.Changed)
            {
                Device?.ApplyEffect(Device.Logo, new NZXTSharp.Fixed(logo.Colors));
                logo.Changed = false;
            }

            Thread.Sleep(16);//limiting the update speed this way for now, might fry the devices otherwise

            return true;
        }
    }
    class HuePlusDevice : AuroraDevice
    {
        private NZXTSharp.HuePlus.HuePlus Device;

        private NZXTDeviceData hueplus = new NZXTDeviceData(40);

        protected override string DeviceName => "HuePlus";

        protected override AuroraDeviceType AuroraDeviceType => AuroraDeviceType.Unkown;

        public HuePlusDevice(INZXTDevice device)
        {
            Device = (device as NZXTSharp.HuePlus.HuePlus);
        }

        private int GetLedIndex(Dictionary<DeviceKeys, int> layout, DeviceKeys key)
        {
            if (layout.ContainsKey(key))
                return layout[key];

            return -1;
        }

        protected override bool UpdateDeviceImpl(DeviceColorComposition composition)
        {
            foreach (KeyValuePair<DeviceKeys, Color> key in composition.keyColors)
            {
                int index;

                if ((index = GetLedIndex(NZXTLayoutMap.HuePlus, key.Key)) != -1)
                {
                    hueplus.SetColor(index, key.Value);
                }
            }
            if (hueplus.Changed)
            {
                Device?.ApplyEffect(Device.Both, new NZXTSharp.Fixed(hueplus.Colors));
                hueplus.Changed = false;
            }

            Thread.Sleep(16);//limiting the update speed this way for now, might fry the devices otherwise

            return true;
        }
    }
}
