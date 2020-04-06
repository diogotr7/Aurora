using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HidLibrary;
using System.ComponentModel;
using System.Reflection;
using Aurora.Devices;
using Aurora.Settings;

namespace Device_UnifiedHID
{
    public class UnifidiedHIDConnector : AuroraDeviceConnector
    {
        protected override string ConnectorName => "UnifiedHID";

        protected override bool InitializeImpl()
        {
            //Copied GetLoadableTypes from https://haacked.com/archive/2012/07/23/get-all-types-in-an-assembly.aspx/
            IEnumerable<Type> GetLoadableTypes(Assembly assembly)
            {
                if (assembly == null) throw new ArgumentNullException(nameof(assembly));
                try
                {
                    return assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException e)
                {
                    return e.Types.Where(t => t != null);
                }
            }
            try
            {
                AppDomain.CurrentDomain.GetAssemblies()
                                       .SelectMany(assembly => GetLoadableTypes(assembly))
                                       .Where(type => type.IsSubclassOf(typeof(UnifiedHIDBaseDevice))).ToList()
                                       .ForEach(class_ => devices.Add((AuroraDevice)Activator.CreateInstance(class_)));
            }
            catch (Exception exc)
            {
                LogError("UnifiedHID class could not be constructed: " + exc);
                return false;
            }
            return true;
        }

        protected override void ShutdownImpl()
        {
        }
        
    }

    abstract class UnifiedHIDBaseDevice : AuroraDevice
    {
        protected HidDevice device;
        protected abstract override bool ConnectImpl();

        protected override void DisconnectImpl()
        {
            try
            {
                device.CloseDevice();
            }
            catch
            {
            }
        }

        protected bool Connect(int vendorID, int[] productIDs, short usagePage)
        {
            IEnumerable<HidDevice> devices = HidDevices.Enumerate(vendorID, productIDs);

            if (devices.Count() > 0)
            {
                try
                {
                    device = devices.First(dev => dev.Capabilities.UsagePage == usagePage);
                    device.OpenDevice();
                    return true;
                }
                catch (Exception exc)
                {
                    LogError($"Error when attempting to open UnifiedHID device:\n{exc}");
                }
            }
            return false;
        }
        protected override void RegisterVariables(VariableRegistry local)
        {
            local.Register($"UnifiedHID_{GetType().Name}_enable", false, $"Enable {(string.IsNullOrEmpty(DeviceName) ? GetType().Name : DeviceName)} in UnifiedHID");
        }
    }

    
}