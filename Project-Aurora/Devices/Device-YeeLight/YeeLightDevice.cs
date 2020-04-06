using Aurora.Devices;
using Aurora.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Device_YeeLight
{
    public class YeeLightConnector : AuroraDeviceConnector
    {
        protected override string ConnectorName => "YeeLight";


        protected override bool InitializeImpl()
        {
            devices.Add(new Device_YeeLight.YeeLightDevice());
            return true;
        }

        protected override void ShutdownImpl()
        {
        }
        protected override void RegisterVariables(VariableRegistry local)
        {
            local.Register($"{ConnectorName}_IP", "0.0.0.0", "YeeLight IP");
        }
    }
    class YeeLightDevice : AuroraDevice
    {
        private YeeLightAPI.YeeLight light = new YeeLightAPI.YeeLight();

        private const int lightListenPort = 55443; // The YeeLight smart bulb listens for commands on this port

        protected override string DeviceName => "YeeLight";

        protected override AuroraDeviceType AuroraDeviceType => AuroraDeviceType.Unkown;

        private int GetFreeTCPPort()
        {
            int freePort;

            // When a TCPListener is created with 0 as port, the TCP/IP stack will asign it a free port
            TcpListener listener = new TcpListener(IPAddress.Loopback, 0); // Create a TcpListener on loopback with 0 as the port
            listener.Start();
            freePort = ((IPEndPoint)listener.LocalEndpoint).Port; // Gets the local port the TcpListener is listening on
            listener.Stop();
            return freePort;
        }

        protected override bool ConnectImpl()
        {
            
            IPAddress lightIP = IPAddress.Parse(GlobalVarRegistry.GetVariable<string>($"{DeviceName}_IP"));
            if (!light.isConnected())
            {
                IPAddress localIP;
                int localListenPort = GetFreeTCPPort(); // This can be any port

                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect(lightIP, lightListenPort);
                    localIP = ((IPEndPoint)socket.LocalEndPoint).Address;
                }
                return light.Connect(lightIP, lightListenPort) && light.SetMusicMode(localIP, (ushort)localListenPort);
            }
            return false;
        }
        protected override void DisconnectImpl()
        {
            light.CloseConnection();

        }

        protected override void RegisterVariables(VariableRegistry local)
        {
            local.Register($"{DeviceName}_devicekey", DeviceKeys.Peripheral, "Key to Use", DeviceKeys.MOUSEPADLIGHT15, DeviceKeys.Peripheral);
            local.Register($"{DeviceName}_send_delay", 100, "Send delay (ms)");
        }


        protected override bool UpdateDeviceImpl(DeviceColorComposition composition)
        {
            Color targetColor = composition.keyColors.FirstOrDefault(pair =>
            {
                return pair.Key == GlobalVarRegistry.GetVariable<DeviceKeys>($"{DeviceName}_devicekey");
            }).Value;

            light.SetColor(targetColor.R, targetColor.G, targetColor.B);
            return true;
        }
    }
}
