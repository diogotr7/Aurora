using Aurora.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Corale.Colore;
using Corale.Colore.Core;
using KeyboardCustom = Corale.Colore.Razer.Keyboard.Effects.Custom;
using MousepadCustom = Corale.Colore.Razer.Mousepad.Effects.Custom;
using MouseCustom = Corale.Colore.Razer.Mouse.Effects.CustomGrid;
using HeadsetStatic = Corale.Colore.Razer.Headset.Effects.Static;
using KeypadCustom = Corale.Colore.Razer.Keypad.Effects.Custom;
using ChromaLinkCustom = Corale.Colore.Razer.ChromaLink.Effects.Custom;

namespace Device_Razer
{
    public class RazerDevice : Aurora.Devices.Device
    {
        protected override string DeviceName => "Razer";

        private KeyboardCustom keyboard = KeyboardCustom.Create();
        private MousepadCustom mousepad = MousepadCustom.Create();
        private MouseCustom mouse = MouseCustom.Create();
        private Color headset = Color.Black;
        private KeypadCustom keypad = KeypadCustom.Create();
        private ChromaLinkCustom chromalink = ChromaLinkCustom.Create();

        private readonly List<string> deviceNames = new List<string>();

        public override bool Initialize()
        {
            if (!Chroma.SdkAvailable)
            {
                LogError("SDK not available. Install Razer synapse");
                return isInitialized = false;
            }

            try
            {
                Chroma.Instance.Initialize();
            }
            catch (Corale.Colore.Razer.NativeCallException e)
            {
                LogError("Error initializing:" + e.Message);
                return isInitialized = false;
            }

            if(!Chroma.Instance.Initialized)
            {
                LogError("Failed to Initialize Razer Chroma sdk");
                return isInitialized = false;
            }

            DetectDevices();

            return isInitialized = true;
        }

        public override string GetDeviceDetails()
        {
            if (isInitialized)
            {
                string devString = DeviceName + ": ";
                devString += "Connected";

                if (deviceNames.Any())
                    devString += ": " + string.Join(", ", deviceNames);
                else
                    devString += ": no devices detected";

                return devString;
            }
            else
            {
                return DeviceName + ": Not initialized";
            }
        }

        public override void Shutdown()
        {
            if (!isInitialized)
                return;

            try
            {
                Chroma.Instance.SetAll(Color.HotPink);
                Chroma.Instance.Uninitialize();
                isInitialized = false;
            }
            catch (Exception e)
            {
                LogError(e.Message);
            }
        }

        public override bool UpdateDevice(Dictionary<DeviceKeys, System.Drawing.Color> keyColors, DoWorkEventArgs e, bool forced = false)
        {
            if (!isInitialized)
                return false;

            foreach (var key in keyColors)
            {
                if (RazerMappings.keyboardDictionary.TryGetValue(key.Key, out var kbIndex))
                    keyboard[kbIndex] = ToColore(key.Value);

                if (RazerMappings.mouseDictionary.TryGetValue(key.Key, out var mouseIndex))
                    mouse[mouseIndex] = ToColore(key.Value);

                if (RazerMappings.mousepadDictionary.TryGetValue(key.Key, out var mousepadIndex))
                    mousepad[mousepadIndex] = ToColore(key.Value);

                if (RazerMappings.headsetDictionary.TryGetValue(key.Key, out var headsetIndex))
                    headset = ToColore(key.Value);

                if (RazerMappings.chromalinkDictionary.TryGetValue(key.Key, out var chromalinkIndex))
                    chromalink[chromalinkIndex] = ToColore(key.Value);
            }
            UpdateAll();
            return true;
        }

        private static Color ToColore(System.Drawing.Color source) => new Color(source.R, source.G, source.B);

        private void DetectDevices()
        {
            //get all devices from colore, with the respective names and Guids
            IEnumerable<(string Name, Guid Guid)> DeviceGuids = typeof(Corale.Colore.Razer.Devices).GetFields().Select(f =>
                    (f.Name,
                    (Guid)f.GetValue(null)));

            deviceNames.Clear();

            foreach (var device in DeviceGuids.Where(d => d.Name != "Razer Core Chroma"))//somehow this device is unsupported, can't query it
            {
                try
                {
                    var devInfo = Chroma.Instance.Query(device.Guid);
                    if (devInfo.Connected)
                    {
                        deviceNames.Add(device.Name);
                    }
                }
                catch (Corale.Colore.Razer.NativeCallException e)
                {
                    LogError("Error querying device: " + e.Message);
                }
            }
        }

        private void UpdateAll()
        {
            if (!isInitialized)
                return;
            Chroma.Instance.Keyboard.SetCustom(keyboard);
            Chroma.Instance.Keyboard.SetCustom(keyboard);
            Chroma.Instance.Mouse.SetGrid(mouse);
            Chroma.Instance.Headset.SetStatic(headset);
            Chroma.Instance.Mousepad.SetCustom(mousepad);
            Chroma.Instance.Keypad.SetCustom(keypad);
            Chroma.Instance.ChromaLink.SetCustom(chromalink);
        }
    }
}
