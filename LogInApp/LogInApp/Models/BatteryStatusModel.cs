using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LogInApp.Models
{
    public class BatteryStatusModel
    {
        // Properties to hold battery status and time
        public bool IsACPowerConnected { get; set; }
        public string BatteryPercentage { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }

        // Timer to periodically update battery status (simulating battery updates)
        private System.Timers.Timer batteryStatusTimer;

        public BatteryStatusModel()
        {
            // Initialize properties with default values
            IsACPowerConnected = true; // Simulating AC power connection at startup
            BatteryPercentage = "100%"; // Simulating fully charged battery
            Time = DateTime.Now.ToString("HH:mm:ss");

            // Start the timer to update battery status every 10 seconds
            batteryStatusTimer = new System.Timers.Timer(10000);
            batteryStatusTimer.Elapsed += UpdateBatteryStatus;
            batteryStatusTimer.Start();
        }

        // Method to simulate battery status update
        private void UpdateBatteryStatus(object sender, ElapsedEventArgs e)
        {
            // Simulating battery percentage fluctuation and checking AC power connection
            Random rand = new Random();
            int percentage = rand.Next(10, 100);
            BatteryPercentage = $"{percentage}%";

            // Simulating AC power connection/disconnection logic
            IsACPowerConnected = rand.Next(0, 2) == 1; // Randomly simulate AC power connection/disconnection

            // Update the time to show the current system time
            Time = DateTime.Now.ToString("HH:mm:ss");
        }

        // Method to stop the timer (optional, for cleanup)
        public void StopBatteryStatusUpdate()
        {
            batteryStatusTimer.Stop();
        }

        // Optionally, you can add a method to display the battery status as a string or more complex logic
        public string GetBatteryStatusInfo()
        {
            return $"Battery: {BatteryPercentage}, AC Power Connected: {IsACPowerConnected}, Time: {Time}";
        }
    }
}
