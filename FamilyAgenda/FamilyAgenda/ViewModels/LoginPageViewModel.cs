using FamilyAgenda.Models;
using FamilyAgenda.Services;
using Firebase.Storage;
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

            //CreateUsers();

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

        private async void CreateUsers()
        {
            try
            {
                //var stream1 = await FileSystem.OpenAppPackageFileAsync("panos_profile.jpg");

                //// Constructr FirebaseStorage, path to where you want to upload the file and Put it there
                //var task = new FirebaseStorage("gs://familyagenda-9dcc8.appspot.com")
                //    .Child("photos")
                //    .Child("random")
                //    .Child("panos_profile.jpg")
                //    .PutAsync(stream1);

                //// await the task to wait until upload completes and get the download url
                //var downloadUrl1 = await task;

                //var stream2 = await FileSystem.OpenAppPackageFileAsync("sofaki.jpg");

                //// Constructr FirebaseStorage, path to where you want to upload the file and Put it there
                //task = new FirebaseStorage("gs://familyagenda-9dcc8.appspot.com")
                //    .Child("photos")
                //    .Child("random")
                //    .Child("sofaki.jpg")
                //    .PutAsync(stream2);

                //// await the task to wait until upload completes and get the download url
                //var downloadUrl2 = await task;

                await FirebaseDbService.AddUserAsync(new User
                {
                    Username = "Panos",
                    Password = "panos",
                    Email = "panos.skydev@gmail.com",
                    ProfilePhoto = ImageSource.FromUri(new Uri("gs://familyagenda-9dcc8.appspot.com/panos_profile.jpg"))
                });

                await FirebaseDbService.AddUserAsync(new User
                {
                    Username = "Sofi",
                    Password = "sofi",
                    Email = "sofi.douzeni@gmail.com",
                    ProfilePhoto = ImageSource.FromUri(new Uri("gs://familyagenda-9dcc8.appspot.com/sofaki.jpg"))
                });
                
            }
            catch (Exception ex)
            {
                
            }
        }        
    }
}
