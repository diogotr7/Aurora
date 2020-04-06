using Aurora.Devices;
using Aurora.Settings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Device_AtmoOrb
{
    public class AtmoOrbConnector : AuroraDeviceConnector
    {
        protected override string ConnectorName => "AtmoOrb";

        protected override bool InitializeImpl()
        {
            devices.Add(new AtmoOrbDevice());
            return true;
        }

        protected override void ShutdownImpl()
        {

        }
    }
    class AtmoOrbDevice : AuroraDevice
    {
        protected override string DeviceName => "AtmoOrb";

        private Socket socket;
        private IPEndPoint ipClientEndpoint;

        protected override AuroraDeviceType AuroraDeviceType => AuroraDeviceType.Unkown;
        protected override bool ConnectImpl()
        {
            var multiCastIp = IPAddress.Parse("239.15.18.2");
            var port = 49692;

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            ipClientEndpoint = new IPEndPoint(multiCastIp, port);
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
                new MulticastOption(multiCastIp));
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);

            socket.Connect(ipClientEndpoint);

            return true;
        }
        protected override void DisconnectImpl()
        {
            if (socket != null)
            {
                // Set color to black
                SendColorsToOrb(0, 0, 0);

                // Close all connections
                socket.Close();
                socket = null;
                ipClientEndpoint = null;
            }
        }
        protected override bool UpdateDeviceImpl(DeviceColorComposition composition)
        {
            Color averageColor;
            lock (composition.bitmapLock)
            {
                //Fix conflict with debug bitmap
                lock (composition.keyBitmap)
                {

                    averageColor = Aurora.Utils.BitmapUtils.GetRegionColor(
                        (Bitmap)composition.keyBitmap,
                        new Aurora.BitmapRectangle(0, 0, composition.keyBitmap.Width,
                            composition.keyBitmap.Height)
                    );
                }
            }

            SendColorsToOrb(averageColor.R, averageColor.G, averageColor.B);
            return true;
        }
        public void SendColorsToOrb(byte red, byte green, byte blue)
        {

            List<string> orbIDs = new List<string>();
            try
            {
                string orb_ids = GlobalVarRegistry.GetVariable<string>($"{DeviceName}_orb_ids") ?? "";
                orbIDs = orb_ids.Split(',').ToList();
            }
            catch (Exception exc)
            {
                orbIDs = new List<string>() { "1" };
            }
            foreach (var orbID in orbIDs)
            {
                if (String.IsNullOrWhiteSpace(orbID))
                    continue;

                try
                {
                    byte[] bytes = new byte[5 + 24 * 3];

                    // Command identifier: C0FFEE
                    bytes[0] = 0xC0;
                    bytes[1] = 0xFF;
                    bytes[2] = 0xEE;

                    // Options parameter: 
                    // 1 = force off
                    // 2 = use lamp smoothing and validate by Orb ID
                    // 4 = validate by Orb ID

                    if (GlobalVarRegistry.GetVariable<bool>($"{DeviceName}_use_smoothing"))
                        bytes[3] = 2;
                    else
                        bytes[3] = 4;

                    // Orb ID
                    bytes[4] = byte.Parse(orbID);

                    // RED / GREEN / BLUE
                    bytes[5] = red;
                    bytes[6] = green;
                    bytes[7] = blue;

                    socket.Send(bytes, bytes.Length, SocketFlags.None);
                }
                catch (Exception)
                {
                }
            }
        }
        protected override void RegisterVariables(VariableRegistry local)
        {
            local.Register($"{DeviceName}_use_smoothing", true, "Use Smoothing");
            local.Register($"{DeviceName}_orb_ids", "1", "Orb IDs", null, null, "For multiple IDs separate with comma");

        }
    }
}
