using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Aurora.Devices;
using Aurora.Settings;

namespace Device_Example
{
    public class ExampleDevice : Aurora.Devices.Device
    {
        protected override string DeviceName => "Example Device";

        protected override bool InitializeImpl()
        {
            LogInfo("Initializing example!");

            return true;
        }

        protected override void ShutdownImpl()
        {
            LogInfo("Shutting down example!");
        }

        public override bool UpdateDevice(Dictionary<DeviceKeys, System.Drawing.Color> keyColors, DoWorkEventArgs e, bool forced = false)
        {
            //Log("Updating!");
            return true;
        }

        protected override void RegisterVariables(VariableRegistry local)
        {
            local.Register($"{DeviceName}_devicekey", DeviceKeys.Peripheral, "Key to Use", DeviceKeys.MOUSEPADLIGHT15, DeviceKeys.Peripheral_Logo);
        }

        public override bool HasWindow => true;

        public override Window GetWindow()
        {
            return new UserControl1();
        }
    }
}
