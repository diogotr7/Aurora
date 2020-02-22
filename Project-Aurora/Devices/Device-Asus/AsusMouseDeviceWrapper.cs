using AuraServiceLib;
using Aurora.Devices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device_Asus
{
    class AsusMouseDeviceWrapper : AsusGenericDeviceWrapper
    {
        public AsusMouseDeviceWrapper(IAuraSyncDevice mouse) : base(mouse) { }

        public override bool Update(Dictionary<DeviceKeys, Color> keyColors)
        {
            foreach (KeyValuePair<DeviceKeys, Color> key in keyColors)
            {

                if (!LedMap.Keys.Contains(key.Key))
                    continue;

                ushort ledIndex = (ushort)LedMap[key.Key];
                SetRGBColor(Device.Lights[ledIndex], key.Value);
            }

            Device.Apply();
            return true;
        }

        public override void Initialize()
        {
            int ledCount = Device.Lights.Count;

            for (ushort i = 0; i < ledCount; i++)
            {
                LedMap[AsusLedIdMapper(i)] = i;
            }

        }
        private DeviceKeys AsusLedIdMapper(ushort lightIndex)
        {
            switch (lightIndex)
            {
                case 0:
                    return DeviceKeys.Peripheral_Logo;
                case 1:
                    return DeviceKeys.Peripheral_ScrollWheel;
                case 2:
                    return DeviceKeys.Peripheral_FrontLight;
                default:
                    return DeviceKeys.NONE;
            }
        }
    }
}
