using FamilyAgenda.Models;
using FamilyAgenda.Services;
using Firebase.Auth;
using Firebase.Storage;
using MonkeyCache.SQLite;
using Plugin.FirebasePushNotification;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FamilyAgenda.ViewModels
{
    public class LoginPageViewModel : BindableBase
    {
        private readonly IFirebaseAuthenticationService _authService;
        private readonly INavigationService _navigationService;
        private string _email;
        private string _password;

        public LoginPageViewModel(INavigationService navigationService, IFirebaseAuthenticationService authService)
        {
            _authService = authService;
            _navigationService = navigationService;

            Barrel.ApplicationId = "FamilyHub";

            LoginCommand = new DelegateCommand(LoginAsync);
            NavigateToSignUpPageCommand = new DelegateCommand(NavigateToSignUpPage);

            Initialize();
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

        public DelegateCommand LoginCommand { get; }

        public DelegateCommand NavigateToSignUpPageCommand { get; }

        private async void Initialize()
        {
            if(Barrel.Current.Exists("auth"))
            {
                var auth = Barrel.Current.Get<FirebaseAuthLink>("auth");
                if(!auth.IsExpired())
                {
                    await Shell.Current.GoToAsync("///main");
                }
                else
                {                    
                    var refreshAuth = await _authService.RefreshToken(auth);
                    if(refreshAuth != null)
                    {
                        Barrel.Current.Add(key: "auth", data: refreshAuth, expireIn: TimeSpan.FromSeconds(refreshAuth.ExpiresIn));                        
                        await Shell.Current.GoToAsync("///main");
                    }
                }
            }                        
        }

        private async void LoginAsync()
        {
            var auth = await _authService.SignIn(Email, Password);            
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

        private async void NavigateToSignUpPage()
        {
            //await Shell.Current.GoToAsync("login/register");
            await _navigationService.NavigateAsync("SingUpPage");
        }
                
    }
}
