using Aurora.Devices;
using HidLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device_UnifiedHID
{
    internal class AsusPugio : UnifiedHIDBaseDevice
    {
        protected override string DeviceName => "Asus Pugio";

        protected override AuroraDeviceType AuroraDeviceType => AuroraDeviceType.Mouse;

        public bool SetScrollWheel(Color color)
        {
            HidReport report = device.CreateReport();
            report.ReportId = 0x00;
            for (int i = 0; i < 64; i++)
            {
                report.Data[i] = 0x00;
            }
            report.Data[0] = 0x51;
            report.Data[1] = 0x28;
            report.Data[2] = 0x01;
            report.Data[4] = 0x00;
            report.Data[5] = 0x04;
            report.Data[6] = color.R;
            report.Data[7] = color.G;
            report.Data[8] = color.B;
            return device.WriteReport(report);
        }

        public bool SetLogo(Color color)
        {
            SetBottomLed(color);
            HidReport report = device.CreateReport();
            report.ReportId = 0x00;
            for (int i = 0; i < 64; i++)
            {
                report.Data[i] = 0x00;
            }
            report.Data[0] = 0x51;
            report.Data[1] = 0x28;
            report.Data[2] = 0x00;
            report.Data[4] = 0x00;
            report.Data[5] = 0x04;
            report.Data[6] = color.R;
            report.Data[7] = color.G;
            report.Data[8] = color.B;
            return device.WriteReport(report);
        }

        public bool SetBottomLed(Color color)
        {
            HidReport report = device.CreateReport();
            report.ReportId = 0x00;
            for (int i = 0; i < 64; i++)
            {
                report.Data[i] = 0x00;
            }
            report.Data[0] = 0x51;
            report.Data[1] = 0x28;
            report.Data[2] = 0x02;
            report.Data[4] = 0x00;
            report.Data[5] = 0x04;
            report.Data[6] = color.R;
            report.Data[7] = color.G;
            report.Data[8] = color.B;
            return device.WriteReport(report);
        }

        protected override bool ConnectImpl()
        {
            if (!Aurora.Global.Configuration.VarRegistry.GetVariable<bool>($"UnifiedHID_{this.GetType().Name}_enable"))
            {
                return false;
            }

            return this.Connect(0x0b05, new[] { 0x1846, 0x1847 }, unchecked((short)0xFFFFFF01));
        }

        protected override bool UpdateDeviceImpl(DeviceColorComposition composition)
        {
            bool success = false;
            foreach (var key in composition.keyColors)
            {
                if (key.Key == DeviceKeys.Peripheral_Logo)
                {
                    success = SetLogo(CorrectAlpha(key.Value));
                }
                else if (key.Key == DeviceKeys.Peripheral_ScrollWheel)
                {
                    success = SetScrollWheel(CorrectAlpha(key.Value));
                }
                else if (key.Key == DeviceKeys.Peripheral_FrontLight)
                {
                    success = SetBottomLed(CorrectAlpha(key.Value));
                }
            }
            return success;
        }
    }

}
