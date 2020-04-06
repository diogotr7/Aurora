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
    class DS4Container : AuroraDevice
    {
        public readonly DS4Device device;
        public readonly ConnectionType connectionType;
        public readonly Color RestoreColor;
        private DeviceKeys key;

        public int Battery { get; private set; }
        public double Latency { get; private set; }
        public bool Charging { get; private set; }

        protected override string DeviceName => "Dualshock 4";

        protected override AuroraDeviceType AuroraDeviceType => throw new NotImplementedException();

        public Color sendColor;
        public DS4HapticState state;
        public DS4Container(DS4Device _device)
        {
            device = _device;
            connectionType = device.getConnectionType();
            device.Report += Device_Report;
            device.StartUpdate();
            ConnectionHandler += ConnectionHandling;
        }
        private void ConnectionHandling(object sender, EventArgs e)
        {
            AuroraDevice device = sender as AuroraDevice;
            if (device.IsConnected())
            {
                key = GlobalVarRegistry.GetVariable<DeviceKeys>($"{DeviceName}_devicekey");
            }
            else
            {
                Disconnect(GlobalVarRegistry.GetVariable<bool>($"{DeviceName}_disconnect_when_stop"));
            }
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

        public override string GetDeviceDetails()
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

        protected override bool UpdateDeviceImpl(DeviceColorComposition composition)
        {
            if (composition.keyColors.TryGetValue(key, out var clr))
            {
                sendColor = CorrectAlpha(clr);

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

    public class Dualshock4Device : AuroraDeviceConnector
    {
        protected override string ConnectorName => "Dualshock 4";

        protected override string ConnectorSubDetails => GetSubDetails();

        private string GetSubDetails()
        {
            string details = $": {Devices.Count} Device{(Devices.Count == 1 ? "" : "s")} Connected: ";

            foreach (var dev in Devices)
                details += " #" + (devices.IndexOf(dev) + 1) + dev.GetDeviceDetails();
            return details;
        }

        protected override bool InitializeImpl()
        {
            DS4Devices.findControllers();
            var controllers = DS4Devices.getDS4Controllers();

            foreach (var device in controllers)
            {
                devices.Add(new DS4Container(device));
            }
            return controllers.Any();
        }

        protected override void ShutdownImpl()
        {
            DS4Devices.stopControllers();
        }
    }
}