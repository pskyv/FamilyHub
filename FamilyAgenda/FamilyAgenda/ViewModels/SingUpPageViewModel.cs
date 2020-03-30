using FamilyAgenda.Services;
using MonkeyCache.SQLite;
using Plugin.FirebasePushNotification;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FamilyAgenda.ViewModels
{
    public class SingUpPageViewModel : BindableBase
    {
        private readonly IFirebaseAuthenticationService _authService;
        private readonly INavigationService _navigationService;
        private string _email;
        private string _password;
        private string _username;

        public SingUpPageViewModel(INavigationService navigationService, IFirebaseAuthenticationService authService)
        {
            _authService = authService;
            _navigationService = navigationService;

            Barrel.ApplicationId = "FamilyHub";

            SignUpCommand = new DelegateCommand(SignUpAsync);
        }        

        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public DelegateCommand SignUpCommand { get; }

        private async void SignUpAsync()
        {
            var auth = await _authService.SignUpWithEmailAndPassword(Email, Password, Username);
            if (auth != null)
            {
                Barrel.Current.Add(key: "auth", data: auth, expireIn: TimeSpan.FromSeconds(auth.ExpiresIn));
                Preferences.Set("user", auth.User.DisplayName);
                
                var topic = "Sofi";
                if (auth.User.DisplayName == "Sofi")
                {
                    topic = "Panos";
                }
                CrossFirebasePushNotification.Current.Subscribe(topic);

                await Shell.Current.GoToAsync("///main");               
            }
        }
    }
}
