using System;
using System.ComponentModel;
using System.Timers; // Correct namespace for System.Timers.Timer
using LogInApp.Models;

namespace LogInApp.ViewModels
{
    public class BatteryStatusViewModel : INotifyPropertyChanged
    {
        private BatteryStatusModel _batteryStatus;
        private System.Timers.Timer _batteryTimer;  // Declare the Timer

        // Constructor initializes the BatteryStatusModel
        public BatteryStatusViewModel()
        {
            _batteryStatus = new BatteryStatusModel();
            StartBatteryStatusUpdates();
        }

        // Property to bind BatteryStatusModel to the view
        public BatteryStatusModel BatteryStatus
        {
            get { return _batteryStatus; }
            set
            {
                _batteryStatus = value;
                OnPropertyChanged(nameof(BatteryStatus));
            }
        }

        // Event for notifying property changes (implement INotifyPropertyChanged)
        public event PropertyChangedEventHandler PropertyChanged;

        // Method to notify the view that a property has changed
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Timer to periodically update battery status and AC power connection
        private void StartBatteryStatusUpdates()
        {
            // Set up the timer to trigger the event every 10 seconds (10000 milliseconds)
            _batteryTimer = new System.Timers.Timer(10000);
            _batteryTimer.Elapsed += OnBatteryStatusUpdate; // Event handler for the timer
            _batteryTimer.AutoReset = true;  // Automatically reset the timer after each event
            _batteryTimer.Enabled = true;  // Start the timer
        }

        // Event handler method that is called every time the timer elapses
        private void OnBatteryStatusUpdate(object sender, ElapsedEventArgs e)
        {
            UpdateBatteryStatus();
        }

        // Update the battery status (e.g., battery percentage and AC power connection)
        private void UpdateBatteryStatus()
        {
            // Simulate changes in battery percentage and AC power connection
            Random rand = new Random();

            // Simulate random battery percentage
            int percentage = rand.Next(10, 100);
            BatteryStatus.BatteryPercentage = $"{percentage}%";

            // Simulate AC power connection status
            BatteryStatus.IsACPowerConnected = rand.Next(0, 2) == 1; // Randomly connect/disconnect AC power

            // Update time each time the battery status changes
            BatteryStatus.Time = DateTime.Now.ToString("HH:mm:ss");

            // Notify the view of property changes
            OnPropertyChanged(nameof(BatteryStatus));
        }
    }
}
