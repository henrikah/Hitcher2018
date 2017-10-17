using System;
using System.Collections.Generic;
using System.Net.Http;
using Hitcher2018.Services.ApiServices;
using Models;
using CustomExceptions;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.UI.Xaml;
using System.ComponentModel;

namespace Hitcher2018.Services.AccountServices
{
    public sealed class AccountService : INotifyPropertyChanged
    {
        # region Object
        private Visibility requireAuthentication = Visibility.Visible;

        public event PropertyChangedEventHandler PropertyChanged;

        public Visibility RequireAuthentication
        {
            get
            {
                return requireAuthentication;
            }
            private set
            {
                requireAuthentication = value;
                NotifyPropertyChanged("RequireAuthentication");
            }
        }
        private void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region Statics
        public static AccountService Instance { get; set; } = new AccountService();
        public static string AccessToken { get; set; }
        public static string UserName { get; set; }
        public static DateTime TokenExirationDate { get; set; }
        public static string TokenType { get; set; }

        public static bool TokenHasExpired()
        {
            return (TokenExirationDate.CompareTo(DateTime.UtcNow) < 0);
        }
        public static string AuthorizeHeaders()
        {
            if(AccessToken == null)
            {
                return null;
            }
            return TokenType + " " + AccessToken;
        }
        public static async Task AuthenticateAsync(string userName, string password)
        {
            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("userName", userName),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("Content-Type", "application/x-www-form-urlencoded")
            };
            var response = await Api.AuthenticateAsync<AuthorizeResponseModel>(new FormUrlEncodedContent(keyValues)); 
            if(response.error == null)
            {
                AccessToken = response.access_token;
                UserName = response.userName;
                TokenExirationDate = Convert.ToDateTime(response.expires);
                TokenType = response.token_type;
                Instance.RequireAuthentication = Visibility.Collapsed;
            } else
            {
                throw new AuthenticationException(response.error_description);
            }
        }
        public static async void LogOutAsync()
        {
            await Api.QueryAsync<object>("Account/Logout", HTTP.POST);
            AccessToken = null;
            UserName = null;
            TokenExirationDate = default(DateTime);
            TokenType = null;
            Instance.RequireAuthentication = Visibility.Visible;
        }
        #endregion
    }
}
