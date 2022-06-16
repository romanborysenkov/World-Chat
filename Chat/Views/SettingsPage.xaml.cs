using System;
using System.Collections.Generic;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace Chat.Views
{
    public partial class SettingsPage : ContentPage
    {

        public string WebApiKey = "AIzaSyAMCGRJYMlnJytYIxN0GLZIPGX5LvYMGno";

        public SettingsPage()
        {
            InitializeComponent();

            MyUserName.Text = Preferences.Get("UserEmail", "");
        }

        async private void GetProfileInformationAndRefreshToken()
        {
            var authProvided = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));

            var savedfirebaseauth = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
            var RefreshedContent = await authProvided.RefreshAuthAsync(savedfirebaseauth);
            Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefreshedContent));
            MyUserName.Text = Preferences.Get("UserEmail", "");//savedfirebaseauth.User.Email;

        }

        async void Logout_Clicked(object sender, EventArgs e)
        {
            Preferences.Set("Logined", false);
            Preferences.Remove("MyFirebaseRefreshToken");
            Preferences.Set("UserEmail", "");
            //  App.Current.MainPage = new NavigationPage(new LoginPage());
            await Shell.Current.GoToAsync("//login");
        }
    }
}
