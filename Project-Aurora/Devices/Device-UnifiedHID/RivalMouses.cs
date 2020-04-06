using Aurora;
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

    internal class Rival100 : UnifiedHIDBaseDevice
    {
        protected override string DeviceName => "Rival 100";

        protected override AuroraDeviceType AuroraDeviceType => AuroraDeviceType.Mouse;

        protected override bool ConnectImpl()
        {
            if (!Global.Configuration.VarRegistry.GetVariable<bool>($"UnifiedHID_{this.GetType().Name}_enable"))
            {
                return false;
            }

            return this.Connect(0x1038, new[] { 0x1702 }, unchecked((short)0xFFFFFFC0));
        }

        public bool SetLogo(Color color)
        {
            HidReport report = device.CreateReport();
            report.ReportId = 0x02;
            report.Data[0] = 0x05;
            report.Data[1] = 0x00;
            report.Data[2] = color.R;
            report.Data[3] = color.G;
            report.Data[4] = color.B;
            return device.WriteReport(report);
        }

        protected override bool UpdateDeviceImpl(DeviceColorComposition composition)
        {
            foreach (var key in composition.keyColors)
            {
                if (key.Key == DeviceKeys.Peripheral_Logo)
                {
                    return SetLogo(CorrectAlpha(key.Value));
                }
            }
            return false;
        }
    }

    internal class Rival110 : UnifiedHIDBaseDevice
    {
        protected override string DeviceName => "Rival 110";

        protected override AuroraDeviceType AuroraDeviceType => AuroraDeviceType.Mouse;

        protected override bool ConnectImpl()
        {
            if (!Global.Configuration.VarRegistry.GetVariable<bool>($"UnifiedHID_{this.GetType().Name}_enable"))
            {
                return false;
            }

            return this.Connect(0x1038, new[] { 0x1729 }, unchecked((short)0xFFFFFFC0));
        }

        public bool SetLogo(Color color)
        {
            HidReport report = device.CreateReport();
            report.ReportId = 0x02;
            report.Data[0] = 0x05;
            report.Data[1] = 0x00;
            report.Data[2] = color.R;
            report.Data[3] = color.G;
            report.Data[4] = color.B;
            report.Data[5] = 0x00;
            report.Data[6] = 0x00;
            report.Data[7] = 0x00;
            report.Data[8] = 0x00;

            return device.WriteReport(report);
        }
        protected override bool UpdateDeviceImpl(DeviceColorComposition composition)
        {
            foreach (var key in composition.keyColors)
            {
                if (key.Key == DeviceKeys.Peripheral_Logo)
                {
                    return SetLogo(CorrectAlpha(key.Value));
                }
            }
            return false;
        }
    }

    internal class Rival300 : UnifiedHIDBaseDevice
    {
        protected override string DeviceName => "Rival 300";

        protected override AuroraDeviceType AuroraDeviceType => AuroraDeviceType.Mouse;

        protected override bool ConnectImpl()
        {
            if (!Global.Configuration.VarRegistry.GetVariable<bool>($"UnifiedHID_{this.GetType().Name}_enable"))
            {
                return false;
            }

            return this.Connect(0x1038, new[] { 0x1710, 0x171A, 0x1394, 0x1384, 0x1718, 0x1712 }, unchecked((short)0xFFFFFFC0));
        }

        public bool SetScrollWheel(Color color)
        {
            HidReport report = device.CreateReport();
            report.ReportId = 0x02;
            report.Data[0] = 0x08;
            report.Data[1] = 0x02;
            report.Data[2] = color.R;
            report.Data[3] = color.G;
            report.Data[4] = color.B;
            return device.WriteReport(report);
        }

        public bool SetLogo(Color color)
        {
            HidReport report = device.CreateReport();
            report.ReportId = 0x02;
            report.Data[0] = 0x08;
            report.Data[1] = 0x01;
            report.Data[2] = color.R;
            report.Data[3] = color.G;
            report.Data[4] = color.B;
            return device.WriteReport(report);
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
            }
            return success;
        }
    }

    internal class Rival500 : UnifiedHIDBaseDevice
    {
        protected override string DeviceName => "Rival 500";

        protected override AuroraDeviceType AuroraDeviceType => AuroraDeviceType.Mouse;

        protected override bool ConnectImpl()
        {
            if (!Global.Configuration.VarRegistry.GetVariable<bool>($"UnifiedHID_{this.GetType().Name}_enable"))
            {
                return false;
            }

            return this.Connect(0x1038, new[] { 0x170e }, unchecked((short)0xFFFFFFC0));
        }

        public bool SetScrollWheel(Color color)
        {
            HidReport report = device.CreateReport();
            report.ReportId = 0x03;
            report.Data[0] = 0x05;
            report.Data[1] = 0x00;
            report.Data[2] = 0x01;
            report.Data[3] = color.R;
            report.Data[4] = color.G;
            report.Data[5] = color.B;
            report.Data[6] = 0xFF;
            report.Data[7] = 0x32;
            report.Data[8] = 0xC8;
            report.Data[9] = 0xC8;
            report.Data[10] = 0x00;
            report.Data[11] = 0x01;
            report.Data[12] = 0x01;
            return device.WriteReport(report);
        }

        public bool SetLogo(Color color)
        {
            HidReport report = device.CreateReport();
            report.ReportId = 0x03;
            report.Data[0] = 0x05;
            report.Data[1] = 0x00;
            report.Data[2] = 0x00;
            report.Data[3] = color.R;
            report.Data[4] = color.G;
            report.Data[5] = color.B;
            report.Data[6] = 0xFF;
            report.Data[7] = 0x32;
            report.Data[8] = 0xC8;
            report.Data[9] = 0xC8;
            report.Data[10] = 0x00;
            report.Data[11] = 0x00;
            report.Data[12] = 0x01;

            return device.WriteReport(report);
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
            }
            return success;
        }
    }

}
