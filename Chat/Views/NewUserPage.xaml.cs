using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using System.Linq;
using Chat;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Chat.Models;

namespace Chat.Views
{
    public partial class NewUserPage : ContentPage
    {

        public string WebApiKey = "AIzaSyAMCGRJYMlnJytYIxN0GLZIPGX5LvYMGno";

        // FirebaseClient firebaseClient = new FirebaseClient("https://international-chat-60ea5-default-rtdb.europe-west1.firebasedatabase.app/");
        UserRepository _userRepository = new UserRepository();

        public NewUserPage()
        {
            InitializeComponent();
        }

        async void signup_Clicked(object sender, EventArgs e)
        {
            Preferences.Set("Logined", true);
            Preferences.Set("UserEmail", userNewEmail.Text);

            string name = userNewName.Text;
            string email = userNewEmail.Text;
            string password = userNewPassword.Text;

            if (String.IsNullOrEmpty(name))
            {
                await DisplayAlert("Warning", "Type name", "Ok");
                return;
            }
            if (String.IsNullOrEmpty(email))
            {
                await DisplayAlert("Warning", "Type email", "Ok");
                return;
            }
            if (password.Length < 6)
            {
                await DisplayAlert("Warning", "Password should be 6 digit.", "Ok");
                return;
            }
            if (String.IsNullOrEmpty(password))
            {
                await DisplayAlert("Warning", "Type password", "Ok");
                return;
            }

            bool isSave = await _userRepository.Register(email, name, password);
            if (isSave)
            {
                await Shell.Current.GoToAsync("//home");
            }

            /* try
             {

                 var authProvided = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
                 var auth = await authProvided.CreateUserWithEmailAndPasswordAsync(userNewEmail.Text, userNewPassword.Text);
                 string gettoken = auth.FirebaseToken;


                 await Shell.Current.GoToAsync("///main/home");
             }
             catch (Exception ex)
             {
                 await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
             }*/
        }
    }
}
