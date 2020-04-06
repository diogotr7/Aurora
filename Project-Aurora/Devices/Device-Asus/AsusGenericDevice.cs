using AuraServiceLib;
using Aurora.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Device_Asus
{
    class AsusGenericDeviceWrapper : AuroraDevice
    {
        protected IAuraSyncDevice Device;

        private AsusDeviceConnector Connector;

        protected Dictionary<DeviceKeys, object> LedMap = new Dictionary<DeviceKeys, object>();

        protected override string DeviceName => Device.Name;

        protected override AuroraDeviceType AuroraDeviceType => AuroraDeviceType.Unkown;

        public int Id { get; set; }

        public AsusGenericDeviceWrapper(IAuraSyncDevice device, AsusDeviceConnector connector)
        {
            Device = device;
            Connector = connector;
        }

        protected void SetRGBColor(IAuraRgbLight led, Color color)
        {
            lock(led)
            {
                color = CorrectAlpha(color);
                led.Red = color.R;
                led.Green = color.G;
                led.Blue = color.B;
            }
        }

        public virtual void Initialize()
        {
            int ledCount = Device.Lights.Count;
            LedMap[DeviceKeys.Peripheral_Logo] = new List<int>();
            for (int i = 0; i < ledCount; i++)
            {
                ((List<int>)LedMap[DeviceKeys.Peripheral_Logo]).Add(i);
            }

        }

        [HandleProcessCorruptedStateExceptions, SecurityCritical]
        protected override bool UpdateDeviceImpl(DeviceColorComposition composition)
        {
            if (Device == null)
                return false;

            lock (Device)
            {
                try
                {
                    foreach (KeyValuePair<DeviceKeys, Color> key in composition.keyColors)
                    {

                        if (!LedMap.Keys.Contains(key.Key))
                            continue;

                        AsusUpdateDeviceKey(key);
                    }

                    Device.Apply();
                    return true;
                }
                catch (Exception exc)
                {
                    LogError("Failed to Update Device" + exc.ToString());
                    Device = Connector.GetNewInstanceOfDeviceById(Id);
                    return false;
                }
            }
        }
        protected virtual void AsusUpdateDeviceKey(KeyValuePair<DeviceKeys, Color> key)
        {
            foreach (int ledIndex in ((List<int>)LedMap[key.Key]))
            {
                SetRGBColor(Device.Lights[ledIndex], key.Value);
            }
        }
    }


}
