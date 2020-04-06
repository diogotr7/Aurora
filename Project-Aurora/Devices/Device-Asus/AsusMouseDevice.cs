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
        protected override AuroraDeviceType AuroraDeviceType => AuroraDeviceType.Mouse;
        public AsusMouseDeviceWrapper(IAuraSyncDevice mouse, AsusDeviceConnector connector) : base(mouse, connector) { }

        protected override void AsusUpdateDeviceKey(KeyValuePair<DeviceKeys, Color> key)
        {
            ushort keyCode = (ushort)LedMap[key.Key];
            SetRGBColor(Device.Lights[keyCode], key.Value);
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
