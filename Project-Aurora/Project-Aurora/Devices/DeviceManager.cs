using Aurora.Profiles;
using CSScriptLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Aurora.Devices
{

    public class UniqueDeviceId
    {
        public AuroraDeviceType Type = AuroraDeviceType.Unkown;
        public string ContainerName = "";
        public string DeviceName = "";
        public UniqueDeviceId(){ }
        public UniqueDeviceId(AuroraDeviceType type, string containerName, string deviceName)
        {
            Type = type;
            ContainerName = containerName;
            DeviceName = deviceName;
        }
    }
    public class DeviceManager : IDisposable
    {
        public List<AuroraDeviceConnector> DeviceContainers { get; } = new List<AuroraDeviceConnector>();
        public IEnumerable<AuroraDevice> Devices => DeviceContainers.SelectMany(d => d.Devices);
        public IEnumerable<AuroraDevice> NotInitializedDevices => DeviceContainers.Where(d => !d.IsInitialized()).SelectMany(dc => dc.Devices);
        /*public IEnumerable<DeviceContainer> InitializedDeviceContainers => AuroraDevices.Where(dc => dc.Device.IsConnected());
        public IEnumerable<IAuroraDevice> InitializedDevices => Devices.Where(d => d.IsConnected());*/


        private const int retryInterval = 100000;
        private const int retryAttemps = 5;
        private int retryAttemptsLeft = retryAttemps;
        private Thread retryThread;
        private bool suspended = false;

        public int RetryAttempts
        {
            get
            {
                return retryAttemptsLeft;
            }
        }
        public event EventHandler NewDevicesInitialized;

        public DeviceManager()
        {
            string devices_scripts_path = System.IO.Path.Combine(Global.ExecutingDirectory, "Scripts", "Devices");

            if (Directory.Exists(devices_scripts_path))
            {
                foreach (string device_script in Directory.EnumerateFiles(devices_scripts_path, "*.*"))
                {
                    try
                    {
                        string ext = Path.GetExtension(device_script);
                        switch (ext)
                        {
                            case ".py":
                                var scope = Global.PythonEngine.ExecuteFile(device_script);
                                dynamic main_type;
                                if (scope.TryGetVariable("main", out main_type))
                                {
                                    dynamic script = Global.PythonEngine.Operations.CreateInstance(main_type);

                                    AuroraDeviceConnector scripted_device = new Devices.ScriptedDevice.ScriptedDeviceConnector(script);
                                    scripted_device.NewSuccessfulInitiation += NewDevicesInitialized;
                                    DeviceContainers.Add(scripted_device);
                                }
                                else
                                    Global.logger.Error("Script \"{0}\" does not contain a public 'main' class", device_script);

                                break;
                            case ".cs":
                                System.Reflection.Assembly script_assembly = CSScript.LoadFile(device_script);
                                foreach (Type typ in script_assembly.ExportedTypes)
                                {
                                    dynamic script = Activator.CreateInstance(typ);

                                    AuroraDeviceConnector scripted_device = new Devices.ScriptedDevice.ScriptedDeviceConnector(script);
                                    scripted_device.NewSuccessfulInitiation += NewDevicesInitialized;
                                    DeviceContainers.Add(scripted_device);
                                }

                                break;
                            default:
                                Global.logger.Error("Script with path {0} has an unsupported type/ext! ({1})", device_script, ext);
                                break;
                        }
                    }
                    catch (Exception exc)
                    {
                        Global.logger.Error("An error occured while trying to load script {0}. Exception: {1}", device_script, exc);
                    }
                }
            }
            
            string deviceDllFolder = Path.Combine(Global.ExecutingDirectory, "Plugins", "Devices");

            Global.logger.Info("Loading Device Plugins");
            if (Directory.Exists(deviceDllFolder))
            {
                foreach (var deviceDll in Directory.EnumerateFiles(deviceDllFolder, "Device-*.dll"))
                {
                    try
                    {
                        var deviceAssembly = Assembly.LoadFile(deviceDll);

                        foreach (var type in deviceAssembly.GetExportedTypes())
                        {
                            if (typeof(AuroraDeviceConnector).IsAssignableFrom(type))
                            {
                                AuroraDeviceConnector devDll = (AuroraDeviceConnector)Activator.CreateInstance(type);
                                devDll.NewSuccessfulInitiation += NewDevicesInitialized;
                                DeviceContainers.Add(devDll);
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        Global.logger.Error($"Error loading device dll: {deviceDll}. Exception: {e.Message}");
                    }
                }
            }

            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
        }

        bool resumed = false;
        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            Global.logger.Info($"SessionSwitch triggered with {e.Reason}");
            if (e.Reason.Equals(SessionSwitchReason.SessionUnlock) && (suspended || resumed))
            {
                Global.logger.Info("Resuming Devices -- Session Switch Session Unlock");
                suspended = false;
                resumed = false;
                this.Initialize(true);
            }
        }

        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Suspend:
                    Global.logger.Info("Suspending Devices");
                    suspended = true;
                    this.Shutdown();
                    break;
                case PowerModes.Resume:
                    Global.logger.Info("Resuming Devices -- PowerModes.Resume");
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    resumed = true;
                    suspended = false;
                    this.Initialize();
                    break;
            }
        }

        public void RegisterVariables()
        {
            //Register any variables
            foreach (var connector in DeviceContainers)
                Global.Configuration.VarRegistry.Combine(connector.GetRegisteredVariables());
        }

        public void Initialize(bool forceRetry = false)
        {
            if (suspended)
                return;

            DeviceContainers.ForEach(ds => ds.Initialize());


            if (DeviceContainers.Where(dc => !dc.IsInitialized()).Any() && (retryThread == null || forceRetry || retryThread?.ThreadState == System.Threading.ThreadState.Stopped))
            {
                if (forceRetry)
                    retryThread?.Abort();
                retryThread = new Thread(RetryInitialize);
                retryThread.Start();
                return;
            }
        }

        private void RetryInitialize()
        {
            for (int try_count = 0; try_count < retryAttemps || !AnyInitialized(); try_count++)
            {
                if (suspended)
                    return;

                Global.logger.Info("Retrying Device Initialization");

                DeviceContainers.ForEach(ds => ds.Initialize());
                retryAttemptsLeft--;

                //We don't need to continue the loop if we aren't trying to initialize anything
                if (DeviceContainers.Where(dc => !dc.IsInitialized()).Any())
                    break;

                Thread.Sleep(retryInterval);
            }

        }

        public void InitializeOnce()
        {
            /*if (!AnyInitialized())
                Initialize();*/
        }

        public bool AnyInitialized() => DeviceContainers.Where(d => d.IsInitialized()).Any();

        public void Shutdown() => DeviceContainers.ForEach(d => d.Shutdown());

        public void ResetDevices() => DeviceContainers.ForEach(d => d.Reset());
        public AuroraDevice GetDevice(UniqueDeviceId id)
        {
            var container = (AuroraDeviceConnector)DeviceContainers.Where(dc => dc.GetConnectorName() == id.ContainerName).Select(dc => dc);
            if (container != null)
            {
                var device = (AuroraDevice)container.Devices.Where(d => d.GetDeviceName() == id.DeviceName).Select(d => d);
                if(device != null && device.GetDeviceType() == id.Type)
                {
                    return device;
                }
                Global.logger.Error("Couldn't found a Device with a name: " + id.DeviceName + " in the Container: " + id.ContainerName);
            }
            else
            {
                Global.logger.Error("Couldn't found a DeviceContainer with a name: " + id.ContainerName);
            }
            return null;
        }
        public List<UniqueDeviceId> GetDeviceIds()
        {
            List<UniqueDeviceId> deviceIds = new List<UniqueDeviceId>();
            foreach (var container in DeviceContainers)
            {
                foreach (var device in container.Devices)
                {
                    deviceIds.Add(new UniqueDeviceId(device.GetDeviceType(), container.GetConnectorName(), device.GetDeviceName()));
                }
            }
            return deviceIds;
        }
        public string GetDevices()
        {
            string devices_info = "";

            foreach (var device in Devices)
                devices_info += device.GetDeviceDetails() + "\r\n";

            if (retryAttemptsLeft > 0)
                devices_info += "Retries: " + retryAttemptsLeft + "\r\n";

            return devices_info;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    Shutdown();
                    if (retryThread != null)
                    {
                        retryThread.Abort();
                        retryThread = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DeviceManager() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
