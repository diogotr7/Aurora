using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Devices;
using Aurora.Settings;
using MSIRGB;

namespace Device_MSI
{
    internal struct MoboData
    {
        internal System.Windows.Media.Color[] clr;
        internal Lighting.FlashingSpeed speed;
        internal ushort step;
        internal bool breathing;
    };

    public class MSIDevice : Device
    {
        protected override string DeviceName => "MSI";
        private Lighting _msi;
        private DeviceKeys _targetKey;
        private System.Drawing.Color _lastColor;
        private MoboData _lastState = new MoboData();

        public override bool Initialize()
        {
            if (isInitialized)
                return true;

            try
            {
                _msi = new Lighting(GlobalVarRegistry.GetVariable<bool>($"{DeviceName}_ignore_mb_check"));
                _targetKey = GlobalVarRegistry.GetVariable<DeviceKeys>($"{DeviceName}_devicekey");
                GetMoboData();
                _msi.BatchBegin();
                _msi.SetStepDuration(0);
                _msi.SetFlashingSpeed(Lighting.FlashingSpeed.Disabled);
                _msi.SetBreathingModeEnabled(false);
                _msi.BatchEnd();
                isInitialized = true;
            }
            catch (Lighting.Exception exc)
            {
                switch (exc.GetErrorCode())
                {
                    case Lighting.ErrorCode.MotherboardModelNotSupported:
                        LogError("MSI SDK Error: Your motherboard is not on the list of supported motherboards. You can override this warning with the \"Ignore Motherboard Compatibility Check\" option in the device manager");
                        break;
                    case Lighting.ErrorCode.MotherboardVendorNotSupported:
                        LogError("MSI SDK Error: Your motherboard's vendor was not detected to be MSI. MSIRGB only supports MSI motherboards. ");
                        break;
                    case Lighting.ErrorCode.DriverLoadFailed:
                        LogError("MSI SDK Error: Failed to load driver.");
                        break;
                    case Lighting.ErrorCode.LoadFailed:
                        LogError("MSI SDK Error: Failed to load.");
                        break;
                    default:
                        LogError("MSI SDK Error:  " + exc.Message);
                        break;
                }
                isInitialized = false;
            }
            return isInitialized;
        }

        public override void Shutdown()
        {
            if (!isInitialized)
                return;

            SetMoboData();
            _msi?.Dispose();
            isInitialized = false;
        }

        public override bool UpdateDevice(Dictionary<DeviceKeys, System.Drawing.Color> keyColors, DoWorkEventArgs e, bool forced = false)
        {
            if (!isInitialized)
                return false;

            if (keyColors.TryGetValue(_targetKey, out var clr))
                SendColorToMotherboard(CorrectAlpha(clr));

            return false;
        }

        protected override void RegisterVariables(VariableRegistry local)
        {
            local.Register($"{DeviceName}_ignore_mb_check", false, "Ignore Motherboard Compatibility Check", remark: "May cause issues, proceed with caution!");
            local.Register($"{DeviceName}_devicekey", DeviceKeys.Peripheral_Logo, "Key to Use", DeviceKeys.MOUSEPADLIGHT15, DeviceKeys.Peripheral);
        }

        private bool SendColorToMotherboard(System.Drawing.Color clr)
        {
            try
            {
                if (clr == _lastColor)
                    return true;

                var newClr = System.Windows.Media.Color.FromRgb(
                    (byte)(clr.R / 0x11),
                    (byte)(clr.G / 0x11),
                    (byte)(clr.B / 0x11));

                _msi.BatchBegin();
                for (byte i = 1; i <= 8; i++)
                    _msi.SetColour(i, newClr);

                _msi.BatchEnd();

                _lastColor = clr;
                return true;
            }
            catch (Exception exc)
            {
                LogError("Failed to Update Device" + exc.Message);
                return false;
            }
        }

        private void GetMoboData()
        {
            _lastState.step = _msi.GetStepDuration();
            _lastState.speed = _msi.GetFlashingSpeed();
            _lastState.breathing = _msi.IsBreathingModeEnabled();
            _lastState.clr = new System.Windows.Media.Color[8];
            for (byte i = 1; i <= 8; i++)
                _lastState.clr[i - 1] = _msi.GetColour(i) ?? System.Windows.Media.Colors.Black;
        }

        private void SetMoboData()
        {
            _msi.BatchBegin();

            _msi.SetBreathingModeEnabled(_lastState.breathing);
            _msi.SetFlashingSpeed(_lastState.speed);
            _msi.SetStepDuration(_lastState.step);
            for (byte i = 1; i <= 8; i++)
                _msi.SetColour(i, _lastState.clr[i - 1]);

            _msi.BatchEnd();
        }
    }
}