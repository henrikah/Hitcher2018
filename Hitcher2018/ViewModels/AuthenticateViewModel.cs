using CustomExceptions;
using Hitcher2018.Services.AccountServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;

namespace Hitcher2018.ViewModels
{
    public class AuthenticateViewModel : ViewModelBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public AuthenticateViewModel()
        {

        }
        public async void Authenticate()
        {
            try
            {
                await AccountService.AuthenticateAsync(UserName, Password);
                NavigationService.Navigate(typeof(Views.MainPage));
            } catch(AuthenticationException e)
            {
                Debug.WriteLine("Exception: " + e);
            }
        }
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }
    }
}
