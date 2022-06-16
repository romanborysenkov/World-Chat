using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Chat.Models
{
    public class UserRepository
    {
        public string WebApiKey = "AIzaSyAMCGRJYMlnJytYIxN0GLZIPGX5LvYMGno";
        FirebaseAuthProvider authProvider;

        public UserRepository()
        {
            authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
        }

        public async Task<bool> Register(string email, string name, string password)
        {

            var token = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password, name);
            // var token2 = await authProvider.GetLinkedAccountsAsync("world");
            if (!string.IsNullOrEmpty(token.FirebaseToken))
            {
                return true;
            }
            return false;
        }

        public async Task<string> SignIn(string email, string password)
        {
            var token = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
            if (!string.IsNullOrEmpty(token.FirebaseToken))
            {
                return token.FirebaseToken;
            }
            return "";
        }

        public async Task<bool> ResetPassword(string email)
        {
            await authProvider.SendPasswordResetEmailAsync(email);
            return true;
        }

        public async Task<string> ChangePassword(string token, string password)
        {
            var auth = await authProvider.ChangeUserPassword(token, password);
            return auth.FirebaseToken;
        }
    }
}
