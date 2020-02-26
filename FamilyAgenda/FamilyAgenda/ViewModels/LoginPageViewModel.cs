using FamilyAgenda.Models;
using FamilyAgenda.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FamilyAgenda.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private User _selectedUser;
        public LoginPageViewModel(INavigationService navigationService, IFirebaseDbService firebaseDbService) : base(navigationService, firebaseDbService)
        {
            SelectUserCommand = new DelegateCommand(SelectUser);
            Initialize();
        }

        public User SelectedUser 
        { 
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }

        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        public DelegateCommand SelectUserCommand { get; }

        private async void Initialize()
        {
            var currentUser = Preferences.Get("user", "");
            if (!string.IsNullOrEmpty(currentUser))
            {
                await Shell.Current.GoToAsync("//main");
                return;
            }

            var users = await FirebaseDbService.GetUsersAsync();
            Users.Clear();
            users.ForEach(Users.Add);
        }

        private async void SelectUser()
        {
            if (SelectedUser == null)
            {
                return;                
            }

            App.ApplicationUser = SelectedUser;
            Preferences.Set("user", SelectedUser.Username);
            await Shell.Current.GoToAsync("//main");
        }
    }
}
