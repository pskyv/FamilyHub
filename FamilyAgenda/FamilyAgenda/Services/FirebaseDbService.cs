using Firebase.Database;
using Firebase.Database.Query;
using FamilyAgenda.Models;
using FamilyAgenda.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FamilyAgenda.Services
{
    public class FirebaseDbService : IFirebaseDbService
    {
        private FirebaseClient _firebaseClient;
        public FirebaseDbService()
        {
            _firebaseClient = new FirebaseClient(Constants.FirebaseDataBaseUrl);
            ListenOnUsersChanges();
        }        

        private void ListenOnUsersChanges()
        {
            var observable = _firebaseClient
                .Child("users")
                .AsObservable<User>()
                .Subscribe(d => MessagingCenter.Send(this, "UserChangeEvent"));
        }

        public async Task<bool> FindUserById(string userId)
        {
            var user = await _firebaseClient
                .Child("users")
                .Child(userId)
                .OnceSingleAsync<User>();

            return user != null;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var users = await _firebaseClient
                .Child("users")
                .OnceAsync<User>();

            var usersList = new List<User>();

            foreach (var user in users)
            {
                usersList.Add(user.Object);
            }

            return usersList;
        }

        public async Task<bool> AddUserAsync(User user)
        {
            try
            {
                await _firebaseClient.Child("users")
                                     .PostAsync(user);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
