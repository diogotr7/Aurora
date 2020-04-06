using Aurora.Devices;
using Aurora.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Aurora.Controls
{
    /// <summary>
    /// Interaction logic for Control_DeviceItem.xaml
    /// </summary>
    public partial class Control_DeviceConnectorItem : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public static readonly DependencyProperty DeviceProperty = DependencyProperty.Register("DeviceConnector", typeof(AuroraDeviceConnector), typeof(Control_DeviceConnectorItem));

        public AuroraDeviceConnector DeviceConnector
        {
            get
            {
                return (AuroraDeviceConnector)GetValue(DeviceProperty);
            }
            set
            {
                SetValue(DeviceProperty, value);

                UpdateControls();
            }
        }

        public Control_DeviceConnectorItem()
        {
            InitializeComponent();

            Global.dev_manager.NewDevicesInitialized += Update_controls_timer_Elapsed;
            Timer update_controls_timer = new Timer(1000); //Update every second
            update_controls_timer.Elapsed += Update_controls_timer_Elapsed;
            update_controls_timer.Start();
        }

        private void Update_controls_timer_Elapsed(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() => { if(IsVisible) UpdateControls(); });
            }
            catch (Exception ex)
            {
                Global.logger.Warn(ex.ToString());
            }
        }

        private void btnToggleOnOff_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button)
            {
                if(DeviceConnector.IsInitialized())
                    DeviceConnector.Shutdown();
                else
                    DeviceConnector.Initialize();

                UpdateControls();
            }
        }

        private void btnToggleEnableDisable_Click(object sender, RoutedEventArgs e)
        {
            if (Global.Configuration.devices_disabled.Contains(DeviceConnector.GetType()))
            {
                Global.Configuration.devices_disabled.Remove(DeviceConnector.GetType());
                //DeviceConnector.Initialize();
            }  
            else
            {
                Global.Configuration.devices_disabled.Add(DeviceConnector.GetType());
                DeviceConnector.Shutdown();
            }

            UpdateControls();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            if (DeviceConnector.IsInitialized())
                btnToggleOnOff.Content = "Stop";
            else
                btnToggleOnOff.Content = "Start";

            txtblk_DeviceStatus.Text = DeviceConnector.GetConnectorDetails().TrimEnd(' ');
            txtblk_DevicePerformance.Text = "";

            if(DeviceConnector is Devices.ScriptedDevice.ScriptedDeviceConnector)
                btnToggleEnableDisable.IsEnabled = false;
            else
            {
                if (Global.Configuration.devices_disabled.Contains(DeviceConnector.GetType()))
                {
                    btnToggleEnableDisable.Content = "Enable";
                    btnToggleOnOff.IsEnabled = false;
                }
                else
                {
                    btnToggleEnableDisable.Content = "Disable";
                    btnToggleOnOff.IsEnabled = true;
                }
            }

            if(DeviceConnector.GetRegisteredVariables().GetRegisteredVariableKeys().Count() == 0)
                btnViewOptions.IsEnabled = false;
            this.lstDevices.ItemsSource = DeviceConnector.Devices.OrderBy(dc => dc.GetDeviceName());


            this.lstDevices.Items.Refresh();
        }

        private void btnViewOptions_Click(object sender, RoutedEventArgs e)
        {
            Window_VariableRegistryEditor options_window = new Window_VariableRegistryEditor();
            options_window.Title = $"{DeviceConnector.GetConnectorName()} - Options";
            options_window.SizeToContent = SizeToContent.WidthAndHeight;
            options_window.VarRegistryEditor.RegisteredVariables = DeviceConnector.GetRegisteredVariables();
            options_window.Closing += (_sender, _eventArgs) =>
            {
                ConfigManager.Save(Global.Configuration);
            };

            options_window.ShowDialog();
        }
    }
}
