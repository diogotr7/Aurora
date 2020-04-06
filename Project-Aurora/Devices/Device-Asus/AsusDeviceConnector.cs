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
    public class AsusDeviceConnector : AuroraDeviceConnector
    {

        private IAuraSdk2 AsusSdk;

        protected override string ConnectorName => "Asus";

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

        protected override bool InitializeImpl()
        {
            if (!CheckForAsusSdk())
            {
                return false;
            }
            // Do this async because it may take a while to initialise
            try
            {
                AsusSdk = new AuraSdk() as IAuraSdk2;
                AsusSdk.SwitchMode();

                var asusDevices = AsusSdk.Enumerate(0);
                foreach (IAuraSyncDevice dev in asusDevices)
                {

                    AsusGenericDeviceWrapper device = CreateDeviceWrapper(dev);
                    device.Initialize();
                    device.Id = GetDeviceId(dev, asusDevices);
                    devices.Add(device);
                }

                return devices.Any();
            }
            catch (Exception exc)
            {
                LogError("There was an error initializing Asus SDK.\r\n" + exc.Message);
            }
            return false;
        }
        private AsusGenericDeviceWrapper CreateDeviceWrapper(IAuraSyncDevice device)
        {
            switch (device.Type)
            {
                case 0x00080000: //Keyboard
                case 0x00081000: //Notebook Keyboard
                    return new AsusKeyboardDeviceWrapper(device, this);

                case 0x00090000: //Mouse
                    return new AsusMouseDeviceWrapper(device, this);

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
                    return new AsusGenericDeviceWrapper(device, this);
                default:
                    LogError("Asus Unknown Device, Type: " + device.Type + "Name: " + device.Name + "LedCount:" + device.Lights.Count);
                    return new AsusGenericDeviceWrapper(device, this);
            }
        }

        protected override void ShutdownImpl()
        {
            AsusSdk?.ReleaseControl(0);
            AsusSdk = null;
        }

        public IAuraSyncDevice GetNewInstanceOfDeviceById(int id)
        {
            AsusSdk.ReleaseControl(0);
            AsusSdk = new AuraSdk() as IAuraSdk2;
            AsusSdk.SwitchMode();
            var allDevices = AsusSdk.Enumerate(0);

            foreach (IAuraSyncDevice device in allDevices)
            {
                var deviceId = GetDeviceId(device, allDevices);
                if (id == deviceId)
                    return device;
            }

            return null;
        }
        /// <summary>
        /// Generate a best attempt ID for an AuraDevice
        /// </summary>
        /// <param name="device">The device that you want the ID for</param>
        /// <param name="devices">A list of devices, to ensure uniqueness</param>
        /// <returns>Device ID</returns>
        private int GetDeviceId(IAuraSyncDevice device, IAuraSyncDeviceCollection allDevices)
        {
            // generate the ID for this device
            var id = GetDeviceId(device);

            int idCount = 0;
            // this is a really bad algorithm, sorry
            foreach (IAuraSyncDevice dev in allDevices)
            {
                if (GetDeviceId(dev) == id)
                {
                    if (device == dev)
                    {
                        return id + idCount;
                    }
                    idCount++;
                }
            }

            return id;
        }
        /// <summary>
        /// Generate an id based on the device provided
        /// </summary>
        /// <param name="device">The device that you want the ID for</param>
        /// <returns>The device's ID</returns>
        private int GetDeviceId(IAuraSyncDevice device)
        {
            return $"{device.Type}_{device.Name}_{device.Lights.Count}".GetHashCode();
        }
    }
}
