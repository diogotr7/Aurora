using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Aurora.Devices;
using System.ComponentModel;
using Roccat_Talk;
using Roccat_Talk.TalkFX;
using Roccat_Talk.RyosTalkFX;
using Aurora.Utils;
using Color = System.Drawing.Color;
using Aurora.Settings;

namespace Device_Roccat
{
    public class RoccatDevice : Device
    {
        protected override string DeviceName => "Roccat Talk";
        private DeviceKeys device_key;
        private bool enable_generic;
        private bool enable_ryos;
        private TalkFxConnection talkFx;
        private RyosTalkFXConnection ryosTalkFx;
        private readonly byte[] stateStruct = new byte[110];
        private readonly Roccat_Talk.TalkFX.Color[] colorStruct = new Roccat_Talk.TalkFX.Color[110];

        protected override bool InitializeImpl()
        {

            device_key = GlobalVarRegistry.GetVariable<DeviceKeys>($"{DeviceName}_devicekey");
            enable_generic = GlobalVarRegistry.GetVariable<bool>($"{DeviceName}_enable_generic");
            enable_ryos = GlobalVarRegistry.GetVariable<bool>($"{DeviceName}_enable_ryos");

            talkFx = new TalkFxConnection();
            ryosTalkFx = new RyosTalkFXConnection();

            //Will return true even when no Ryos is connected. Check if TalkFx is opened?
            //Is even requierd for 1 color devices with the used sdk version.
            if (!ryosTalkFx.Initialize())
            {
                LogInfo("Could not initialize Ryos Talk Fx");
                return false;
            }

            //Will return true even when no Ryos is connected. Check if TalkFx is opened?
            //Is even requierd for 1 color devices with the used sdk version.
            if (!ryosTalkFx.EnterSdkMode())
            {
                LogInfo("Could not Enter Ryos SDK Mode");
                return false;
            }

            return true;
        }

        protected override void ShutdownImpl()
        {
            //Shutdown 1 color devices.
            var restoreColor = ToRoccatColor(GlobalVarRegistry.GetVariable<RealColor>($"{DeviceName}_restore_fallback"));
            talkFx?.SetLedRgb(Zone.Event, KeyEffect.On, Speed.Fast, restoreColor); //Workaround because "RestoreLedRgb()" doesn't seam to work.
            talkFx?.RestoreLedRgb();
            talkFx?.Dispose();

            //Shutdown per key keyboards.
            ryosTalkFx?.ExitSdkMode();
            ryosTalkFx?.Dispose();
        }

        public override bool UpdateDevice(Dictionary<DeviceKeys, Color> keyColors, DoWorkEventArgs e, bool forced = false)
        {
            if (enable_ryos)
            {
                foreach (var key in keyColors)
                {
                    if (KeyMap.DeviceKeysMap.TryGetValue(KeyMap.LocalizeKey(key.Key), out var k))
                    {
                        colorStruct[k] = ToRoccatColor(key.Value);
                        stateStruct[k] = IsLedOn(key.Value);
                    }
                }
                ryosTalkFx.SetMkFxKeyboardState(stateStruct, colorStruct, (byte)KeyMap.GetLayout());
            }

            //Set 1 color devices
            if (enable_generic && keyColors.TryGetValue(device_key, out var clr))
            {
                talkFx.SetLedRgb(Zone.Event, KeyEffect.On, Speed.Fast, ToRoccatColor(clr));
            }

            return true;
        }

        protected override void RegisterVariables(VariableRegistry local)
        {
            local.Register($"{DeviceName}_devicekey", DeviceKeys.Peripheral_Logo, "Key to Use", DeviceKeys.MOUSEPADLIGHT15, DeviceKeys.Peripheral_Logo);
            local.Register($"{DeviceName}_restore_fallback", new RealColor(System.Drawing.Color.FromArgb(255, 0, 0, 255)), "Color", new Aurora.Utils.RealColor(System.Drawing.Color.FromArgb(255, 255, 255, 255)), new Aurora.Utils.RealColor(System.Drawing.Color.FromArgb(0, 0, 0, 0)), "Set restore color for your generic roccat devices");
            local.Register($"{DeviceName}_enable_generic", true, "Enable 1 color devices");
            local.Register($"{DeviceName}_enable_ryos", false, "Enable per key devices");
        }

        private static Roccat_Talk.TalkFX.Color ToRoccatColor(Color c) =>
            new Roccat_Talk.TalkFX.Color(c.R, c.G, c.B);

        private byte IsLedOn(Color roccatColor)
        {
            if (roccatColor.R == 0 && roccatColor.G == 0 && roccatColor.B == 0)
            {
                return 0;
            }
            return 1;
        }
    }
}