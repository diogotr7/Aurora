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

namespace Device_Roccat
{
    public class RoccatDevice : Device
    {
        protected override string DeviceName => "Roccat";
        private TalkFxConnection talkFx;
        private RyosTalkFXConnection ryosTalkFx;
        private readonly byte[] stateStruct = new byte[110];
        private readonly Roccat_Talk.TalkFX.Color[] colorStruct = new Roccat_Talk.TalkFX.Color[110];


        public override bool Initialize()
        {
            if (isInitialized)
                return true;
            
            talkFx = new TalkFxConnection();
            ryosTalkFx = new RyosTalkFXConnection();

            if (!ryosTalkFx.Initialize())
            {
                LogError("Could not initialize Ryos TalkFX");
                return isInitialized = false;
            }

            if (!ryosTalkFx.EnterSdkMode())
            {
                return isInitialized = false;
            }

            return isInitialized = true;
        }

        public override void Shutdown()
        {
            var restore = new Roccat_Talk.TalkFX.Color(0, 0, 0);//TODO: get restore color 
            ryosTalkFx?.ExitSdkMode();
            ryosTalkFx?.Dispose();
            talkFx?.SetLedRgb(Zone.Event, KeyEffect.On, Speed.Fast, restore);
            talkFx?.Dispose();
            isInitialized = false;
        }

        public override bool UpdateDevice(Dictionary<DeviceKeys, Color> keyColors, DoWorkEventArgs e, bool forced = false)
        {
            if (!isInitialized)
                return false;

            foreach(var key in keyColors)
            {
                if (KeyMap.DeviceKeysMap.TryGetValue(key.Key, out var k))
                {
                    colorStruct[k] = ToRoccatColor(key.Value);
                    stateStruct[k] = IsLedOn(key.Value);
                }
            }
            ryosTalkFx.SetMkFxKeyboardState(stateStruct, colorStruct, 0x01);

            return true;
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
