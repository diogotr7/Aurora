using AuraServiceLib;
using Aurora.Devices;
using Aurora.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device_Asus
{
    class AsusGenericDeviceWrapper
    {
        protected IAuraSyncDevice Device;

        protected Dictionary<DeviceKeys, object> LedMap = new Dictionary<DeviceKeys, object>();
        public AsusGenericDeviceWrapper(IAuraSyncDevice device)
        {
            Device = device;
        }
        public virtual bool Update(Dictionary<DeviceKeys, Color> keyColors)
        {
            foreach (KeyValuePair<DeviceKeys, Color> key in keyColors)
            {

                if (!LedMap.Keys.Contains(key.Key))
                    continue;

                foreach (int ledIndex in ((List<int>)LedMap[key.Key]))
                {
                    SetRGBColor(Device.Lights[ledIndex], key.Value);
                }
            }

            Device.Apply();
            return true;
        }

        protected void SetRGBColor(IAuraRgbLight led, Color color)
        {
            lock(led)
            {
                color = ColorUtils.CorrectWithAlpha(color);
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
    }


}
