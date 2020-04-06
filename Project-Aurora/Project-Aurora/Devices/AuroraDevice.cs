using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Aurora.Settings;

namespace Aurora.Devices
{
    public abstract class AuroraDevice
    {
        private readonly Stopwatch Watch = new Stopwatch();
        private long LastUpdateTime = 0;
        private bool UpdateIsOngoing = false;
        private bool DeviceIsConnected = false;

        private VariableRegistry variableRegistry;

        public event EventHandler ConnectionHandler;
        public event EventHandler UpdateFinished;
        /// <summary>
        /// Is called every frame (30fps). Update the device here
        /// </summary>

        //[HandleProcessCorruptedStateExceptions, SecurityCritical]
        public async void UpdateDevice(DeviceColorComposition composition)
        {
            if (IsConnected())
            {
                if (Global.Configuration.devices_disabled.Contains(GetType()))
                {
                    //Initialized when it's supposed to be disabled? SMACK IT!
                    Disconnect();
                    return;
                }

                if (!UpdateIsOngoing)
                {
                    UpdateIsOngoing = true;
                    Watch.Restart();
                    try
                    {
                        if (!await Task.Run(() => UpdateDeviceImpl(composition)))
                        {
                            LogError(DeviceName + " device, error when updating device.");
                        }
                    }
                    catch(Exception exc)
                    {
                        LogError(DeviceName + " device, error when updating device. Exception: " + exc.Message);
                    }
                    

                    Watch.Stop();
                    LastUpdateTime = Watch.ElapsedMilliseconds;


                    UpdateFinished.Invoke(this, new EventArgs());
                    UpdateIsOngoing = false;
                }

            }
        }
        protected abstract bool UpdateDeviceImpl(DeviceColorComposition composition);

        public string GetDeviceUpdatePerformance()
        {
            return IsConnected() ? LastUpdateTime + " ms" : "";
        }
        public async void Connect()
        {
            if (GetDeviceType() == AuroraDeviceType.Keyboard && Global.Configuration.devices_disable_keyboard ||
                GetDeviceType() == AuroraDeviceType.Mouse && Global.Configuration.devices_disable_mouse ||
                GetDeviceType() == AuroraDeviceType.Headset && Global.Configuration.devices_disable_headset)
            {
                Disconnect();
            }
            else
            {
                try
                {
                    if (await Task.Run(() => ConnectImpl()))
                    {
                        DeviceIsConnected = true;
                        ConnectionHandler.Invoke(this, new EventArgs());
                    }
                }
                catch (Exception exc)
                {
                    Global.logger.Info("Device, " + GetDeviceName() + ", throwed exception:" + exc.ToString());
                }
            }
        }
        protected virtual bool ConnectImpl()
        {
            return true;
        }

        public void Disconnect() 
        {
            if(IsConnected())
            {
                DisconnectImpl();
                DeviceIsConnected = false;
                ConnectionHandler.Invoke(this, new EventArgs());
            }
        }
        protected virtual void DisconnectImpl()
        {
        }

        protected abstract string DeviceName { get; }
        public string GetDeviceName() => DeviceName;

        public virtual string GetDeviceDetails() => DeviceName + ": " + (IsConnected() ? "Connected" : "Not connected");

        protected abstract AuroraDeviceType AuroraDeviceType { get; }
        public AuroraDeviceType GetDeviceType() => AuroraDeviceType;

        public VariableRegistry GetRegisteredVariables()
        {
            if (variableRegistry == null)
            {
                variableRegistry = new VariableRegistry();
                RegisterVariables(variableRegistry);
            }
            return variableRegistry;
        }
        /// <summary>
        /// Only called once when registering variables. Can be empty if not needed
        /// </summary>
        protected virtual void RegisterVariables(VariableRegistry local)
        {
            //purposefully empty, if varibles are needed, this should be overridden
        }

        public bool IsConnected() => DeviceIsConnected;

        protected void LogInfo(string s) => Global.logger.Info(s);

        protected void LogError(string s) => Global.logger.Error(s);

        protected Color CorrectAlpha(Color clr) => Utils.ColorUtils.CorrectWithAlpha(clr);

        protected VariableRegistry GlobalVarRegistry => Global.Configuration.VarRegistry;
    }

    public abstract class AuroraKeyboardDevice : AuroraDevice
    {
        protected override AuroraDeviceType AuroraDeviceType => AuroraDeviceType.Keyboard;
  
    }
 }
