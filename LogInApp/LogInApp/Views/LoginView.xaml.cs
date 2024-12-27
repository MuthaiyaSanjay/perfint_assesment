using System.Windows;
using System.Windows.Controls;
using LogInApp.ViewModels;

namespace LogInApp.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
        }

        private void VirtualKeyboardButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var value = button?.Tag.ToString();

            if (value == "C")
            {
                PinPasswordBox.Clear();
            }
            else
            {
                if (PinPasswordBox.Password.Length < PinPasswordBox.MaxLength)
                {
                    PinPasswordBox.Password += value;
                }
            }
        }

        private void PinPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null && DataContext is LoginViewModel viewModel)
            {
                viewModel.Pin = passwordBox.Password;
            }

        }
    }
}
