using System.ComponentModel;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Windows.Media;

namespace LogInApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string pin;
        private string errorMessage;
        private string successMessage;
        private bool canLogin;
        private Brush borderColor;
        private bool isBlinkingRed;

        public string Pin
        {
            get { return pin; }
            set
            {
                pin = value;
                OnPropertyChanged(nameof(Pin));
                ValidatePin();
                AutoLogin();  // Trigger auto-login when PIN changes
            }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public string SuccessMessage
        {
            get { return successMessage; }
            set
            {
                successMessage = value;
                OnPropertyChanged(nameof(SuccessMessage));
            }
        }

        public bool CanLogin
        {
            get { return canLogin; }
            set
            {
                canLogin = value;
                OnPropertyChanged(nameof(CanLogin));
            }
        }

        public Brush BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                OnPropertyChanged(nameof(BorderColor));
            }
        }

        public bool IsBlinkingRed
        {
            get { return isBlinkingRed; }
            set
            {
                isBlinkingRed = value;
                OnPropertyChanged(nameof(IsBlinkingRed));
            }
        }

        public ICommand SubmitPinCommand { get; set; }
        public ICommand AddToPinCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginViewModel()
        {
            SubmitPinCommand = new RelayCommand(SubmitPin);
            AddToPinCommand = new RelayCommand<string>(AddToPin);
            ExitCommand = new RelayCommand(ExitApp);
            Pin = string.Empty;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            BorderColor = Brushes.Black;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddToPin(string number)
        {
            if (Pin.Length < 6)
            {
                Pin += number;
            }
        }

        private void ValidatePin()
        {
            CanLogin = Pin.Length == 4 || Pin.Length == 6;
        }

        private void AutoLogin()
        {
            if (Pin.Length == 4 || Pin.Length == 6)
            {
                SubmitPin();
            }
        }

        private void SubmitPin()
        {
            if (string.IsNullOrEmpty(Pin))
            {
                ErrorMessage = "Please enter a PIN.";
                SuccessMessage = string.Empty;
                BorderColor = Brushes.Black;
            }
            else if (Pin == "1234")
            {
                ErrorMessage = string.Empty;
                SuccessMessage = "Welcome on board!";
                BorderColor = Brushes.Green;

                // Reset border color after 3 seconds
                System.Threading.Tasks.Task.Delay(3000).ContinueWith(_ =>
                {
                    BorderColor = Brushes.Black;
                });
            }
            else
            {
                ErrorMessage = "Invalid PIN. Please enter a correct PIN.";
                SuccessMessage = string.Empty;
                BorderColor = Brushes.Red;
                IsBlinkingRed = true;

                // Stop blinking after 3 seconds
                System.Threading.Tasks.Task.Delay(3000).ContinueWith(_ =>
                {
                    IsBlinkingRed = false;
                    BorderColor = Brushes.Black;
                });
            }

            Pin = string.Empty;
        }

        private void ExitApp()
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
