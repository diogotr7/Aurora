using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Devices;
using Aurora.Settings;
using DS4Windows;

namespace Device_Dualshock4
{
    class DS4Container
    {
        public readonly DS4Device device;
        public readonly ConnectionType connectionType;
        public readonly Color RestoreColor;

        public int Battery { get; private set; }
        public double Latency { get; private set; }
        public bool Charging { get; private set; }

        public Color sendColor;
        public DS4HapticState state;
        public DS4Container(DS4Device _device)
        {
            device = _device;
            connectionType = device.getConnectionType();
            device.Report += Device_Report;
            device.StartUpdate();
        }

        private void Device_Report(object sender, EventArgs e)
        {
            Battery = device.Battery;
            Latency = device.Latency;
            Charging = device.Charging;
            if(!ColorsEqual(sendColor, state.LightBarColor))
            {
                state.LightBarExplicitlyOff = sendColor.R == 0 && sendColor.G == 0 && sendColor.B == 0;
                state.LightBarColor = new DS4Color(sendColor);
                device.pushHapticState(state);
            }
        }

        public void Disconnect(bool stop)
        {
            device.Report -= Device_Report;
            state.LightBarExplicitlyOff = sendColor.R == 0 && sendColor.G == 0 && sendColor.B == 0;
            state.LightBarColor = new DS4Color(RestoreColor);
            device.pushHapticState(state);
            if (stop)
            {
                device.DisconnectBT();
                device.DisconnectDongle();
            }
            device.StopUpdate();
        }

        public string GetDeviceDetails()
        {
            string details = "";

            switch (connectionType)
            {
                case ConnectionType.BT:
                    details += " over Bluetooth";
                    break;
                case ConnectionType.USB:
                    details += " over USB";
                    break;
                case ConnectionType.SONYWA:
                    details += " over DS4 Wireless adapter";
                    break;
            }

            details += Charging ? " ⚡" : " ";
            details += "🔋" + Battery + "% Delay: " + Latency.ToString("0.00") + " ms";
            return details;
        }

        private bool ColorsEqual(Color clr, DS4Color ds4clr)
        {
            return clr.R == ds4clr.red &&
                    clr.G == ds4clr.green &&
                    clr.B == ds4clr.blue;
        }
    }

    public class Dualshock4Device : Device
    {
        protected override string DeviceName => "Dualshock 4";
        private DeviceKeys key;
        private readonly List<DS4Container> Devices = new List<DS4Container>();

        public override string GetDeviceDetails()
        {
            string details = DeviceName;
            if (isInitialized)
            {
                details += $": {Devices.Count} Device{(Devices.Count == 1 ? "" : "s")} Connected: ";

                foreach (var dev in Devices)
                    details += " #" + (Devices.IndexOf(dev) + 1) + dev.GetDeviceDetails();
            }
            else
            {
                details += ": Not connected";
            }

            return details;
        }

        public override bool Initialize()
        {
            if (isInitialized)
                return true;

            key = GlobalVarRegistry.GetVariable<DeviceKeys>($"{DeviceName}_devicekey");
            DS4Devices.findControllers();
            var controllers = DS4Devices.getDS4Controllers();

            foreach (var controller in controllers)
                Devices.Add(new DS4Container(controller));

            return isInitialized = Devices.Any();
        }

        public override void Shutdown()
        {
            if (!isInitialized)
                return;

            foreach (var dev in Devices)
                dev.Disconnect(GlobalVarRegistry.GetVariable<bool>($"{DeviceName}_disconnect_when_stop"));

            DS4Devices.stopControllers();
            Devices.Clear();
            isInitialized = false;
        }

        public override bool UpdateDevice(Dictionary<DeviceKeys, System.Drawing.Color> keyColors, DoWorkEventArgs e, bool forced = false)
        {
            if (keyColors.TryGetValue(key, out var clr))
            {
                foreach (var dev in Devices)
                    dev.sendColor = CorrectAlpha(clr);

                return true;
            }

            return false;
        }

        protected override void RegisterVariables(VariableRegistry local)
        {
            local.Register($"{DeviceName}_devicekey", DeviceKeys.Peripheral, "Key to Use", DeviceKeys.MOUSEPADLIGHT15, DeviceKeys.Peripheral_Logo);
            local.Register($"{DeviceName}_disconnect_when_stop", false, "Disconnect when Stopping");
        }
    }
}