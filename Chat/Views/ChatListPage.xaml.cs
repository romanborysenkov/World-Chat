using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Chat.Views
{
    public partial class ChatListPage : ContentPage
    {
        public ChatListPage()
        {
            InitializeComponent();
        }

        private async void Listview(Object sender, EventArgs e)// ItemTappedEventArgs e)
        {
            
             await Navigation.PushAsync(new AboutPage());  
        }

        private async void Profile_Clicked(object sender, EventArgs e)
        {
            // await Shell.Current.GoToAsync("///setting");
            await Navigation.PushAsync(new SettingsPage());
        }

        async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //  await Shell.Current.GoToAsync("///chat");
            await Navigation.PushAsync(new AboutPage());
            // await Shell.Current.GoToAsync(nameof(AboutPage));
        }
    }
}
