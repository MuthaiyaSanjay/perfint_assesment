using System;
using System.Management;  // For accessing WMI
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using LogInApp.Views;

namespace LogInApp
{
    public partial class MainWindow : Window
    {
        private Window shutdownMessageWindow;

        public MainWindow()
        {
            InitializeComponent();
            UpdateDateTime();
            UpdateBatteryStatus();
            CheckACPower();
        }

        private void UpdateDateTime()
        {
            TimeTextBlock.Text = DateTime.Now.ToString("F");

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (sender, e) => TimeTextBlock.Text = DateTime.Now.ToString("F");
            timer.Start();
        }

        // Update the battery status
        private void UpdateBatteryStatus()
        {
            try
            {
                string imagePath = string.Empty;
                string batteryStatusMessage = GetBatteryStatusFromWMI(out imagePath);

                Dispatcher.Invoke(() =>
                {
                    BatteryStatusTextBlock.Text = batteryStatusMessage;
                    BatteryIcon.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    BatteryStatusTextBlock.Text = $"Error retrieving battery status: {ex.Message}";
                    BatteryIcon.Source = new BitmapImage(new Uri("img/battery_error.png", UriKind.Relative));
                });
            }
        }

        // Get battery status from WMI
        private string GetBatteryStatusFromWMI(out string imagePath)
        {
            try
            {
                imagePath = "battery.png"; // Placeholder image for discharging or full

                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Battery");

                foreach (ManagementObject battery in searcher.Get())
                {
                    var batteryPercentage = battery["EstimatedChargeRemaining"];
                    var batteryLife = battery["BatteryStatus"];

                    string statusMessage = $"Battery: {batteryPercentage}%";

                    if (batteryLife != null)
                    {
                        ushort batteryLifeStatus = (ushort)batteryLife;
                        if (batteryLifeStatus == 1) // Charging
                        {
                            imagePath = "battery.png";
                        }
                        else if (batteryLifeStatus == 2 || batteryLifeStatus == 3) // Discharging or Full
                        {                            
                            imagePath = "battery_icon.png";
                        }
                    }

                    return statusMessage;
                }
            }
            catch (Exception ex)
            {
                imagePath = "battery.png";
                return $"Error retrieving battery status: {ex.Message}";
            }

            imagePath = "battery.png";
            return "Battery information not available.";
        }

       
        private void CheckACPower()
        {
            try
            {
                // Query WMI for AC power status
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Battery");

                foreach (ManagementObject battery in searcher.Get())
                {
                    var batteryPercentage = battery["EstimatedChargeRemaining"];
                    var batteryLife = battery["BatteryStatus"];

                    if (batteryLife != null)
                    {
                        ushort batteryLifeStatus = (ushort)batteryLife;
                        if (batteryLifeStatus == 1)
                        {
                            ShowShutdownMessage();
                            DisableLogin();
                        }
                        else if (batteryLifeStatus == 2 || batteryLifeStatus == 3)
                        {
                            HideShutdownMessage();
                            EnableLogin();
                        }
                      
                    }
                    else
                    {
                        HideShutdownMessage();
                        EnableLogin();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking AC power: {ex.Message}");
            }
        }


        private void ShowShutdownMessage()
        {
            if (shutdownMessageWindow == null)
            {
                shutdownMessageWindow = new Window
                {
                    Title = "Shutdown Warning",
                    Width = 300,
                    Height = 150,
                    Content = new StackPanel
                    {
                        Children =
                {
                    new TextBlock { Text = "Charger is not connected. Please connect Charger to continue.", Margin = new Thickness(10) },
                    new Button
                    {
                        Content = "Shutdown",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(10)
                    }
                }
                    }
                };

                // Access the button directly after it's created
                Button shutdownButton = (Button)((StackPanel)shutdownMessageWindow.Content).Children[1];
                shutdownButton.Click += ShutdownButton_Click;

                shutdownMessageWindow.Show();
            }
        }


        // Event handler for the shutdown button
        private void ShutdownButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        // Hide the shutdown message window
        private void HideShutdownMessage()
        {
            shutdownMessageWindow?.Close();
            shutdownMessageWindow = null;
        }

        // Disable login button when AC power is not connected
        private void DisableLogin()
        {
            LoginButton.IsEnabled = false; // Disable the login button
        }

        // Enable login button when AC power is connected
        private void EnableLogin()
        {
            LoginButton.IsEnabled = true; // Enable the login button
        }

        // Handle login button click
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoginButton.IsEnabled)
            {
                LoginView loginView = new LoginView();
                loginView.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Cannot login as AC power is not connected.");
            }
        }
    }
}
