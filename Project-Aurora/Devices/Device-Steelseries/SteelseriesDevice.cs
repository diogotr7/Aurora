using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Aurora;
using Aurora.Devices;
using Aurora.Devices.SteelSeries;
using Aurora.Settings;
using Aurora.Utils;
using Newtonsoft.Json.Linq;

namespace Device_SteelSeries
{
    public partial class SteelSeriesDevice : Device
    {
        protected override string DeviceName => "SteelSeries";

        protected override bool InitializeImpl()
        {
            try
            {
                if (!baseObject.ContainsKey("game"))
                {
                    baseObject.Add("game", "PROJECTAURORA");
                    baseColorObject.AddFirst(new JProperty("game", baseObject["game"]));
                }
                client = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
                loadCoreProps();
                return true;
            }
            catch (Exception e)
            {
                LogError(e, "SteelSeries SDK could not be initialized.");
                return false;
            }
        }

        protected override void ShutdownImpl()
        {
            pingTaskTokenSource.Cancel();
            client?.Dispose();
        }

        public override void Reset()
        {
            Shutdown();

            Process.Start(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\SteelSeries\SteelSeries Engine 3\SteelSeries Engine 3.lnk");
            Task.Delay(15000).GetAwaiter().GetResult();

            Initialize();
        }

        public override bool UpdateDevice(Dictionary<DeviceKeys, Color> keyColors, DoWorkEventArgs e, bool forced = false)
        {
            dataColorObject.RemoveAll();
            if (!Global.Configuration.devices_disable_mouse || !Global.Configuration.devices_disable_headset)
            {
                if (keyColors.ContainsKey(DeviceKeys.Peripheral))
                    SetZone(keyColors[DeviceKeys.Peripheral], SteelSeriesZone.periph);
                else if(keyColors.ContainsKey(DeviceKeys.Peripheral_Logo))
                {
                    var mousePad = keyColors.Where(t => t.Key >= DeviceKeys.MOUSEPADLIGHT1 && t.Key <= DeviceKeys.MOUSEPADLIGHT12).Select(t => t.Value).ToArray();
                    var mouse = keyColors.Where(t => t.Key == DeviceKeys.Peripheral_Logo || t.Key == DeviceKeys.Peripheral_ScrollWheel).Select(t => t.Value).ToArray();
                    //mouse.AddRange(keyColors.Where(t => t.Key <= DeviceKeys.MOUSELIGHT1 && t.Key >= DeviceKeys.MOUSELIGHT6).Select(t => t.Value));
                    SetZone(keyColors[DeviceKeys.Peripheral_Logo], SteelSeriesZone.onezone);
                    if (mouse.Length <= 1)
                        SetZone(keyColors[DeviceKeys.Peripheral_Logo], SteelSeriesZone.mouse);
                    else
                    {
                        SetZone(keyColors[DeviceKeys.Peripheral_Logo], SteelSeriesZone.logo);
                        SetZone(keyColors[DeviceKeys.Peripheral_ScrollWheel], SteelSeriesZone.wheel);
                        if (mouse.Length == 8)
                            SetZones(mouse.ToArray(), SteelSeriesZone.eightzone);
                    }
                    if (mousePad.Length == 2)
                        SetZones(mousePad, SteelSeriesZone.twozone);
                    else
                        SetZones(mousePad, SteelSeriesZone.twelvezone);
                    //if (keyColors.Count(t => t.Key >= DeviceKeys.MONITORLIGHT1 && t.Key <= DeviceKeys.MONITORLIGHT103) == 103)
                    //    setHundredThreeZone(keyColors.Where(t => t.Key >= DeviceKeys.MONITORLIGHT1 && t.Key <= DeviceKeys.MONITORLIGHT103).Select(t => t.Value).ToArray());
                }

            }
            if (!Global.Configuration.devices_disable_keyboard)
            {
                foreach (var (key, color) in keyColors)
                {
                    if (TryGetHid(key, out var hid))
                    {
                        setKeyboardLed(hid, color);
                    }
                }
            }
            sendLighting();
            return true;
        }
    }
}