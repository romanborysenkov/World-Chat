using System;
using System.Collections.Generic;
using System.Windows.Input;
using Chat;
using Chat.Views;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chat.Views
{
    public partial class LoginPage : ContentPage
    {
        public string WebApiKey = "AIzaSyAMCGRJYMlnJytYIxN0GLZIPGX5LvYMGno";

        public LoginPage()
        {
            InitializeComponent();

            bool isLog = Preferences.Get("Logined", false);
            if (isLog)
            {
                Shell.Current.GoToAsync("//home");
            }

            BindingContext = this;
        }

        async void login_Clicked(System.Object sender, System.EventArgs e)
        {
            if (String.IsNullOrEmpty(userEmail.Text))
            {
                await DisplayAlert("Error", "Input data for log in", "Ok");
            }
            else
            {
                Preferences.Set("Logined", true);

                Preferences.Set("UserEmail", userEmail.Text);

                var authProvided = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
                try
                {
                    var auth = await authProvided.SignInWithEmailAndPasswordAsync(userEmail.Text, userPassword.Text);

                    var content = await auth.GetFreshAuthAsync();
                    var serializedcontnet = JsonConvert.SerializeObject(content);
                    Preferences.Set("MyFirebaseRefreshToken", serializedcontnet);

                    await Shell.Current.GoToAsync("//home");
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                }
            }
        }

        void NewUser_Clicked(Object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewUserPage());

            // await Shell.Current.GoToAsync("///signup");
            // await Navigation.PushAsync(new NewUserPage());
        }
    }
}
