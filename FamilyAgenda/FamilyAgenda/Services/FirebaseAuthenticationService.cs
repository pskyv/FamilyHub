using FamilyAgenda.Utils;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FamilyAgenda.Services
{
    public class FirebaseAuthenticationService : IFirebaseAuthenticationService
    {
        private readonly FirebaseAuthProvider _authProvider;
        public FirebaseAuthenticationService()
        {
            _authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FirebaseApiKey));
        }

        public async Task<FirebaseAuthLink> SignUpWithEmailAndPassword(string email, string password, string username)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Helpers.ShowToastMessage(Constants.AuthConnectivityMsg);
                return null;
            }

            try
            {
                return await _authProvider.CreateUserWithEmailAndPasswordAsync(email, password, username);
            }

            catch (FirebaseAuthException ex)
            {
                Helpers.ShowToastMessage(Constants.AuthenticationFailedMsg + ex.Reason.ToString());
                return null;
            }
        }

        public async Task<FirebaseAuthLink> SignIn(string email, string password)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Helpers.ShowToastMessage(Constants.AuthConnectivityMsg);
                return null;
            }

            try
            {
                return await _authProvider.SignInWithEmailAndPasswordAsync(email, password);
            }

            catch (FirebaseAuthException ex)
            {
                Helpers.ShowToastMessage(Constants.AuthenticationFailedMsg + ex.Reason.ToString());
                return null;
            }                       
        }

        public async Task<FirebaseAuthLink> RefreshToken(FirebaseAuth auth)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Helpers.ShowToastMessage(Constants.AuthConnectivityMsg);
                return null;
            }

            try
            {
                return await _authProvider.RefreshAuthAsync(auth);
            }

            catch (FirebaseAuthException ex)
            {
                Helpers.ShowToastMessage(Constants.AuthenticationFailedMsg + ex.Reason.ToString());
                return null;
            }
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }
        
    }
}
