using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Devices;
using Aurora.Settings;
using Wooting;

namespace Device_Wooting
{
    public class WootingDevice : Aurora.Devices.Device
    {
        protected override string DeviceName => "Wooting";

        public override bool Initialize()
        {
            if (!isInitialized)
            {
                try
                {
                    if (RGBControl.IsConnected())
                    {
                        isInitialized = true;
                    }
                }
                catch (Exception exc)
                {
                    LogError("There was an error initializing Wooting SDK.\r\n" + exc.Message);

                    return false;
                }
            }

            return isInitialized;
        }

        public override void Shutdown()
        {
            if (!isInitialized)
                return;

            RGBControl.Reset();
            isInitialized = false;
        }

        public override bool UpdateDevice(Dictionary<DeviceKeys, System.Drawing.Color> keyColors, DoWorkEventArgs e, bool forced = false)
        {
            if (!isInitialized)
                return false;

            try
            {
                foreach (var key in keyColors)
                {
                    if(WootingMapping.KeyMap.TryGetValue(key.Key, out var wootKey))
                    {
                        var clr = CorrectAlpha(key.Value);
                        RGBControl.SetKey(wootKey, (byte)(clr.R * GlobalVarRegistry.GetVariable<int>($"{DeviceName}_scalar_r") / 100),
                                                   (byte)(clr.G * GlobalVarRegistry.GetVariable<int>($"{DeviceName}_scalar_g") / 100),
                                                   (byte)(clr.B * GlobalVarRegistry.GetVariable<int>($"{DeviceName}_scalar_b") / 100));
                    }
                }

                return RGBControl.UpdateKeyboard();
            }
            catch (Exception exc)
            {
                LogError("Failed to update device" + exc.ToString());
                return false;
            }
        }

        protected override void RegisterVariables(VariableRegistry local)
        {
            local.Register($"{DeviceName}_scalar_r", 100, "Red Scalar", 100, 0);
            local.Register($"{DeviceName}_scalar_g", 100, "Green Scalar", 100, 0);
            local.Register($"{DeviceName}_scalar_b", 100, "Blue Scalar", 100, 0, "In percent");
        }
    }
}
