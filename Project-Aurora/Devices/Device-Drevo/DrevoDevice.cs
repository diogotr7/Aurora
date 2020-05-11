using Aurora.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device_Drevo
{
    public class DrevoDevice : Aurora.Devices.Device
    {
        protected override string DeviceName => "Drevo";

        protected override bool InitializeImpl() => DrevoRadiSDK.DrevoRadiInit();

        protected override void ShutdownImpl() => DrevoRadiSDK.DrevoRadiShutdown();

        public override bool UpdateDevice(Dictionary<DeviceKeys, System.Drawing.Color> keyColors, DoWorkEventArgs e, bool forced = false)
        {
            try
            {
                byte[] bitmap = new byte[392];
                bitmap[0] = 0xF3;
                bitmap[1] = 0x01;
                bitmap[2] = 0x00;
                bitmap[3] = 0x7F;
                int index = 0;
                foreach (var key in keyColors)
                {
                    if (e.Cancel) return false;

                    index = DrevoRadiSDK.ToDrevoBitmap((int)key.Key);
                    if (index != -1)
                    {
                        index = index * 3 + 4;
                        bitmap[index] = key.Value.R;
                        bitmap[index + 1] = key.Value.G;
                        bitmap[index + 2] = key.Value.B;
                    }
                }

                DrevoRadiSDK.DrevoRadiSetRGB(bitmap, 392);
                return true;
            }
            catch (Exception exc)
            {
                LogError("Drevo device, error when updating device. Error: " + exc);
                return false;
            }
        }
    }
}
