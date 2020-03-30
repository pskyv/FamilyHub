using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamilyAgenda.Services
{
    public interface IFirebaseAuthenticationService
    {
        Task<FirebaseAuthLink> SignUpWithEmailAndPassword(string email, string password, string username);

        Task<FirebaseAuthLink> SignIn(string email, string password);

        Task<FirebaseAuthLink> RefreshToken(FirebaseAuth auth);

        void Logout();
    }
}
