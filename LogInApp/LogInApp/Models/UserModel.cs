using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInApp.Models
{
    public class UserModel
    {
        public int Id { get; set; } // Added an Id to uniquely identify each user
        public string Pin { get; set; } // PIN entered by the user
        public string UserType { get; set; } // User type (Service, Admin, or User)

        // Constructor to initialize the user model
        public UserModel(int id, string pin, string userType)
        {
            Id = id;
            Pin = pin;
            UserType = userType;
        }

        // Validate the user's PIN (a simple validation logic here)
        public bool ValidatePin(string enteredPin)
        {
            // Assuming PIN is a 4-digit or 6-digit number
            if (enteredPin.Length == 4 || enteredPin.Length == 6)
            {
                // Further validation can be added (e.g., numeric validation, pattern checking, etc.)
                return true;
            }
            return false;
        }

        // Mask the pin for display purposes (e.g., for security reasons in UI) 
        public string MaskPin(string pin)
        {
            // Return a masked version of the pin using dots
            return new string('*', pin.Length); // This will mask the pin with '*' characters
        }

        // Optionally, you can add other methods to support your logic (e.g., check user type, etc.)
    }
}
