using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Devices;
using Aurora.Settings;

namespace Device_Example
{
    public class ExampleDeviceConnector : AuroraDeviceConnector 
    {
        protected override string ConnectorName => "Example Connector";

        protected override bool InitializeImpl()
        {
            LogError("Initializing example!");
            //Add the created devices to the devices member
            return true;
        }

        protected override void ShutdownImpl()
        {
            LogError("Shutting down example!");

        }

        protected override void RegisterVariables(VariableRegistry local)
        {
            local.Register($"{ConnectorName}_devicekey", DeviceKeys.Peripheral, "Key to Use", DeviceKeys.MOUSEPADLIGHT15, DeviceKeys.Peripheral_Logo);
        }

    }
}
