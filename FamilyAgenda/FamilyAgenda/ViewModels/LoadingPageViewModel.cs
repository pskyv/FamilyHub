using FamilyAgenda.Services;
using Firebase.Auth;
using MonkeyCache.SQLite;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace FamilyAgenda.ViewModels
{
    public class LoadingPageViewModel : BindableBase
    {
        private readonly IFirebaseAuthenticationService _authService;

        public LoadingPageViewModel(IFirebaseAuthenticationService authService)
        {
            _authService = authService;

            Barrel.ApplicationId = "FamilyHub";

            Initialize();
        }

        private async void Initialize()
        {
            if (Barrel.Current.Exists("auth"))
            {
                var auth = Barrel.Current.Get<FirebaseAuthLink>("auth");
                if (!auth.IsExpired())
                {
                    await Shell.Current.GoToAsync("///main");
                }
                else
                {
                    var refreshAuth = await _authService.RefreshToken(auth);
                    if (refreshAuth != null)
                    {
                        Barrel.Current.Add(key: "auth", data: refreshAuth, expireIn: TimeSpan.FromSeconds(refreshAuth.ExpiresIn));
                        await Shell.Current.GoToAsync("///main");
                    }
                    else
                    {
                        await Shell.Current.GoToAsync("///login");
                    }
                }
            }
            else
            {
                await Shell.Current.GoToAsync("///login");
            }
        }
    }
}
