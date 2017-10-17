using CustomExceptions;
using Hitcher2018.Services.AccountServices;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Hitcher2018.Controls
{
    public sealed partial class Login : UserControl, INotifyPropertyChanged
    {
        public Login()
        {
            this.InitializeComponent();
        }
        private string errorMessage = "Test";
        public string ErrorMessage {
            get {
                return errorMessage;
            }
            set {
                errorMessage = value;
                NotifyPropertyChanged("ErrorMessage");
            }
        }
        public event EventHandler HideRequested;
        public event EventHandler LoggedIn;
        public event PropertyChangedEventHandler PropertyChanged;

        private async void LoginClicked(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            try
            {
                await AccountService.AuthenticateAsync(UserCredentials.UserName, UserCredentials.Password);
                LoggedIn?.Invoke(this, EventArgs.Empty);
            } catch (AuthenticationException message)
            {
                ErrorMessage = message.Message;
            } catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            } finally
            {
                this.IsEnabled = true;
            }
        }

        private void CloseClicked(object sender, RoutedEventArgs e)
        {
            HideRequested?.Invoke(this, EventArgs.Empty);
        }

        public User UserCredentials { get; set; } = new User();
        private void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
