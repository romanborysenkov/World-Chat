using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Models;
using Firebase.Database;
using Xamarin.Forms;

namespace Chat.Views
{
    public partial class VideoPage : ContentPage
    {

        public string WebApiKey = "AIzaSyAMCGRJYMlnJytYIxN0GLZIPGX5LvYMGno";
        

        public VideoPage(string video)
        {
            InitializeComponent();
           
            VideoBy.Source = video;

           /* DatabaseItems.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });*/
        }

      /* async protected override void OnAppearing()
        {
            var messages = await messageRepository.GetAll();
            DatabaseItems.ItemsSource = null;
            DatabaseItems.ItemsSource = messages;
            DatabaseItems.IsRefreshing = false;
        }*/
    }
}
