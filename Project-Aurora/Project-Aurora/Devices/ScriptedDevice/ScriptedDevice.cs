using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Aurora.Settings;
using Microsoft.Win32.TaskScheduler;

namespace Aurora.Devices.ScriptedDevice
{
    public class ScriptedDeviceConnector : Aurora.Devices.AuroraDeviceConnector
    {
        private bool crashed = false;
        private readonly dynamic script = null;

        private string devicename;

        protected override string ConnectorName => "Script";

        public ScriptedDeviceConnector(dynamic script)
        {
            if (
                (script != null) &&
                (script.devicename != null) &&
                (script.enabled != null && script.enabled == true) &&
                (script.GetType().GetMethod("Initialize") != null || script.Initialize != null) &&
                (script.GetType().GetMethod("Shutdown") != null || script.Shutdown != null) &&
                (script.GetType().GetMethod("Reset") != null || script.Reset != null) &&
                (script.GetType().GetMethod("UpdateDevice") != null || script.UpdateDevice != null)
                )
            {
                this.devicename = script.devicename;
                this.script = script;
            }
            else
            {
                throw new Exception("Provided script, does not meet all the requirements");
            }
        }

        private string GetDeviceDetails()
        {
            if (crashed)
                return " Error!";

            return " Connected";
        }

        protected override bool InitializeImpl()
        {
            try
            {
               if (script.Initialize())
                {
                    devices.Add(new ScriptedDevice(script));
                    return true;
                }
                return false;

            }
            catch (Exception exc)
            {
                Global.logger.Error("Device script for {0} encountered an error during Initialization. Exception: {1}", devicename, exc);
                crashed = true;

                return false;
            }
        }

        public override void Reset()
        {
            try
            {
                script.Reset();
            }
            catch (Exception exc)
            {
                Global.logger.Error("Device script for {0} encountered an error during Reset. Exception: {1}", devicename, exc);
                crashed = true;
            }
        }

        protected override void ShutdownImpl()
        {
            try
            {
                this.Reset();
                script.Shutdown();
            }
            catch (Exception exc)
            {
                Global.logger.Error("Device script for {0} encountered an error during Shutdown. Exception: {1}", devicename, exc);
                crashed = true;
            }
        }

        public override VariableRegistry GetRegisteredVariables()
        {
            if (script.GetType().GetMethod("GetRegisteredVariables") != null)
                return script.GetRegisteredVariables();
            else
                return new VariableRegistry();
        }
    }
    public class ScriptedDevice : Aurora.Devices.AuroraDevice
    {
        private bool crashed = false;
        private readonly dynamic script = null;
        protected override string DeviceName => script.devicename;
        public ScriptedDevice(dynamic script)
        {
                this.script = script;
        }

        protected override AuroraDeviceType AuroraDeviceType => throw new NotImplementedException();

        protected override bool UpdateDeviceImpl(DeviceColorComposition composition)
        {
            try
            {
                return script.UpdateDevice(composition.keyColors);
            }
            catch (Exception exc)
            {
                Global.logger.Error(
                    "Device script for {0} encountered an error during UpdateDevice. Exception: {1}",
                    DeviceName, exc);
                crashed = true;

                return false;
            }
        }
    }
}
