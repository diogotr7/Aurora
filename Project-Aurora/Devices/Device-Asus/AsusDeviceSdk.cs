using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Devices;
using AuraServiceLib;
using Microsoft.Win32;
using System.Threading;

namespace Device_Asus
{
    public class AsusDeviceSdk : Aurora.Devices.Device
    {

        private IAuraSdk2 AsusSdk;
        IList<AsusGenericDeviceWrapper> Devices = new List<AsusGenericDeviceWrapper>();

        protected override string DeviceName => "Asus";

        private bool CheckForAsusSdk()
        {
            const string AuraSdkRegistryEntry = @"{05921124-5057-483E-A037-E9497B523590}\InprocServer32";
            var registryClassKey = Registry.ClassesRoot.OpenSubKey("CLSID");
            if (registryClassKey != null)
            {
                var auraSdkRegistry = registryClassKey.OpenSubKey(AuraSdkRegistryEntry);
                if (auraSdkRegistry == null)
                {
                    LogError("[ASUS] Aura SDK not found in registry.");
                    return false;
                }
            }
            return true;
        }

        public override bool Initialize()
        {
            if (!isInitialized)
            {
                isInitialized = true;
                LogInfo("[ASUS] Initializing Asus Device");
                if (!CheckForAsusSdk())
                {
                    return false;
                }

                // Do this async because it may take a while to initialise
                Task.Run(() =>
                {
                    var task = Task.Run(() =>
                    {
                        try
                        {
                            AsusSdk = new AuraSdk() as IAuraSdk2;
                            AsusSdk.SwitchMode();

                            foreach (IAuraSyncDevice device in AsusSdk.Enumerate(0))
                            {

                                AsusGenericDeviceWrapper deviceWrapper = CreateDeviceWrapper(device);
                                deviceWrapper.Initialize();
                                Devices.Add(deviceWrapper);
                            }

                        }
                        catch (Exception exc)
                        {
                            LogError("There was an error initializing Asus SDK.\r\n" + exc.Message);
                        }
                    });
                    Thread.Sleep(10000);
                    if (!task.IsCompleted)
                    {
                        isInitialized = false;
                        LogError("There was an error in connecting Asus Aura (Not installed, or not working).\r\n");
                    }
                });
            }
            
            return true;
        }
        private AsusGenericDeviceWrapper CreateDeviceWrapper(IAuraSyncDevice device)
        {
            switch (device.Type)
            {
                case 0x00080000: //Keyboard
                case 0x00081000: //Notebook Keyboard
                    return new AsusKeyboardDeviceWrapper(device);

                case 0x00090000: //Mouse
                    return new AsusMouseDeviceWrapper(device);

                case 0x00010000: //Motherboard
                case 0x00011000: //Motherboard LED Strip
                case 0x00020000: //VGA
                case 0x00040000: //Headset
                case 0x00070000: //DRAM
                case 0x00081001: //Notebook Keyboard(4 - zone type)
                case 0x00000000: //All
                case 0x00012000: //All - In - One PC
                case 0x00030000: //Display
                case 0x00050000: //Microphone
                case 0x00060000: //External HDD
                case 0x00061000: //External BD Drive
                case 0x000B0000: //Chassis
                case 0x000C0000: //Projector
                    return new AsusGenericDeviceWrapper(device);
                default:
                    LogError("Asus Unknown Device, Type: " + device.Type + "Name: " + device.Name + "LedCount:" + device.Lights.Count);
                    return new AsusGenericDeviceWrapper(device);
            }
        }

        public override void Shutdown()
        {
            AsusSdk?.ReleaseControl(0);
            Devices.Clear();
            AsusSdk = null;
        }

        public override bool UpdateDevice(Dictionary<DeviceKeys, Color> keyColors, DoWorkEventArgs e, bool forced = false)
        {
            try
            {
                if (!this.isInitialized)
                    return false;

                foreach (var device in Devices)
                {
                    lock(device)
                    {
                        device.Update(keyColors);
                    }
                }
                return true;
            }
            catch (Exception exc)
            {
                LogError("Failed to Update Device" + exc.ToString());
                return false;
            }
        }
    }
}
