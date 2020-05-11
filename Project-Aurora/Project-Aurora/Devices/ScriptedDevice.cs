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
    public class ScriptedDevice : Device
    {
        private bool crashed = false;
        private readonly dynamic script = null;

        private string devicename = "";

        public ScriptedDevice(dynamic script)
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
        protected override string DeviceName => devicename;
        public override string GetDeviceDetails()
        {
            if (crashed)
                return devicename + ": Error!";

            if (IsInitialized())
                return devicename + ": Connected";
            else
                return devicename + ": Not initialized";
        }

        protected override bool InitializeImpl()
        {
            try
            {
                return script.Initialize();
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
            if (IsInitialized())
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

        public override bool UpdateDevice(Dictionary<DeviceKeys, Color> keyColors, DoWorkEventArgs e, bool forced = false)
        {
            try
            {
                return script.UpdateDevice(keyColors, forced);
            }
            catch (Exception exc)
            {
                Global.logger.Error(
                    "Device script for {0} encountered an error during UpdateDevice. Exception: {1}",
                    devicename, exc);
                crashed = true;

                return false;
            }

        }


        public VariableRegistry GetRegisteredVariables()
        {
            if (script.GetType().GetMethod("GetRegisteredVariables") != null)
                return script.GetRegisteredVariables();
            else
                return new VariableRegistry();
        }

        public System.Windows.Window GetWindow() => null;

        public bool HasWindow { get; } = false;

    }
}
