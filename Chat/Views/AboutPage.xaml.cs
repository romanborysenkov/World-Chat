using System;
using Xamarin.Forms;
using Firebase.Database;
using Firebase.Database.Query;
using System.Linq;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Firebase.Storage;
using System.IO;
using Xamarin.Forms.PancakeView;
using System.Threading.Tasks;
using Chat.Models;
using Plugin.Media.Abstractions;
using Xamarin.Forms.PlatformConfiguration;
using MediaManager;
using LibVLCSharp.Shared;
using Chat.Models.TypeModels;
using Chat.ViewModels;
using System.Windows.Input;
using System.IO.Compression;
using Plugin.Media;
using System.Drawing;

namespace Chat.Views
{
    public partial class AboutPage : ContentPage
    {
          public string WebApiKey = "AIzaSyAMCGRJYMlnJytYIxN0GLZIPGX5LvYMGno";

        FirebaseClient firebaseClient = new FirebaseClient("https://international-chat-60ea5-default-rtdb.europe-west1.firebasedatabase.app/");

        FirebaseStorage firebaseStorage = new FirebaseStorage("international-chat-60ea5.appspot.com");

        MessageRepository messageRepository = new MessageRepository();

       // MyDatabaseRecord myDatanaseRecord = new MyDatabaseRecord();
        public AboutPage()
        {
            InitializeComponent();

            BindingContext = this;
          
            DatabaseItems.RefreshCommand = new Command(() => { OnAppearing(); });
        }    

        async protected override void OnAppearing()
        {
            var messages = await messageRepository.GetAll();
            DatabaseItems.ItemsSource = null;
            DatabaseItems.ItemsSource = messages;
            DatabaseItems.IsRefreshing = false;
        }

      async  void Button_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(recordData.Text)) { }
            else if(recordData.Text.Length>5000)
            {
               await DisplayAlert("Error", "Text is very long", "Ok");
            }
            else
            {
                var um = Preferences.Get("UserEmail", " ");

                await firebaseClient.Child("Records").PostAsync(new MyDatabaseRecord
                {
                    Message = recordData.Text,
                    SendTime = DateTime.Now.ToShortTimeString(),
                    UserName = um.Split('@').First(),
                });

                recordData.Text = "";
                OnAppearing();
            }
        }

       async void Open_VideoGalery(object sender, EventArgs e)
        {
            var um = Preferences.Get("UserEmail", " ");
            //  messageRepository.SendMessage(recordData.Text,false,true, false);

            var video = await MediaPicker.PickVideoAsync();

            var task = new FirebaseStorage("international-chat-60ea5.appspot.com/",
                new FirebaseStorageOptions
                {
                    ThrowOnCancel = true
                })
                .Child("Files")
                .Child(video.FileName)
                .PutAsync(await video.OpenReadAsync());

           var downloadlink = await task;

            await firebaseClient.Child("Records").PostAsync(new MyDatabaseRecord
            {
                Message = recordData.Text,
                SendTime = DateTime.Now.ToShortTimeString(),
                UserName = um.Split('@').First(),
                Video = downloadlink,
                isVideo = true
            });

            recordData.Text = "";
            OnAppearing();
        }

       async  void Open_galery(object sender, EventArgs e)
        {
            var um = Preferences.Get("UserEmail", " ");

            var photo = await MediaPicker.PickPhotoAsync();

            var task = new FirebaseStorage("international-chat-60ea5.appspot.com",
                new FirebaseStorageOptions
                {
                    ThrowOnCancel = true
                })
                .Child("Files")
                .Child(photo.FileName)
                .PutAsync(await photo.OpenReadAsync());

           string downloadlink = await task;

            await firebaseClient.Child("Records").PostAsync(new MyDatabaseRecord
            {
                Message = recordData.Text,
                SendTime = DateTime.Now.ToShortTimeString(),
                UserName = um.Split('@').First(),
                File = downloadlink,
                isFile = true
            });
            recordData.Text = "";
            OnAppearing();
        }

        async void Open_camera(object sender, EventArgs e)
        {
            var un = Preferences.Get("UserEmail", "");
            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo == null)
                return;
            var task = new FirebaseStorage("international-chat-60ea5.appspot.com/",
                new FirebaseStorageOptions
                {
                    ThrowOnCancel = true
                })
                .Child("Files")
                .Child(photo.FileName)
                .PutAsync(await photo.OpenReadAsync());

            var downloadlink = await task;

            await firebaseClient.Child("Records").PostAsync(new MyDatabaseRecord
            {
                Message = recordData.Text,
                SendTime = DateTime.Now.ToShortTimeString(),
                UserName = un.Split('@').First(),
                File = downloadlink,
                isFile = true
            });
            recordData.Text = "";
            OnAppearing();
        }

       /* async void AudioToolClicked(object sender, EventArgs e)
        {
            var audio = await FilePicker.PickAsync();

            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));

            var un = Preferences.Get("UserEmail", "");

            if (audio == null)
                return;
            var task = new FirebaseStorage("international-chat-60ea5.appspot.com/",
                new FirebaseStorageOptions
                {
                    ThrowOnCancel = true
                }).Child("Files").Child(audio.FileName)
                .PutAsync(await audio.OpenReadAsync());

            var downloadlink = await task;
           
                await firebaseClient.Child("Records").PostAsync(new MyDatabaseRecord
                {
                    Message = recordData.Text,
                    SendTime = DateTime.Now.ToShortTimeString(),
                    UserName = un.Split('@').First(),
                    Audio = downloadlink,
                    isAudio = true
                });
            
            recordData.Text = "";
            OnAppearing();
        }*/

        MediaFile file;
        public string deleteId;
        public MyDatabaseRecord deleteMessage;

        private async void DeleteMenuItem_Clicked(object sender, EventArgs e)
        {

            var response = await DisplayAlert("Delete", "Do you want to delete?", "Yes", "No");
            if (response)
            {
                string id = ((MenuItem)sender).CommandParameter.ToString();
                message = await messageRepository.GetById(id);

                await messageRepository.Delete(id);
                OnAppearing();
            }
        }

        public string id;
        public MyDatabaseRecord message;

        Button button = new Button()
        { HeightRequest = 50, WidthRequest = 70,
            Scale = 0.9, CornerRadius = 30,
            BackgroundColor = ColorConverters.FromHex("#03a5fc"),
            Text = "Edit",
            BorderWidth = 2, VerticalOptions = LayoutOptions.End,
            HorizontalOptions = LayoutOptions.End
        };

        private async void EditSwipeItem_Invoked(object sender, EventArgs e)
        {
            id = ((MenuItem)sender).CommandParameter.ToString();
            message = await messageRepository.GetById(id);
            if (message == null)
            {
                await DisplayAlert("Warning", "Data not found.", "Ok");
            }
            message.Id = id;

            recordData.Text = message.Message;
           
            stack.Children.Add(button);
            button.Clicked += UpdateButton;
        }     

        public async void UpdateButton(object sender, EventArgs e)
        {
            MyDatabaseRecord mydatabase = new MyDatabaseRecord();

            mydatabase.Id = id;
            mydatabase.Message = recordData.Text;
            mydatabase.SendTime = message.SendTime;
            mydatabase.UserName = message.UserName;
            mydatabase.File = message.File;
            mydatabase.Video = message.Video;
           // mydatabase.Audio = message.Audio;
            if (file != null)
            {
                string image = await messageRepository.Upload(file.GetStream(), Path.GetFileName(file.Path));
                mydatabase.File = image;
            }

            await messageRepository.Update(mydatabase);
            OnAppearing();
            recordData.Text = "";
            stack.Children.Remove(button);
           
            OnAppearing();
        }

         void SearchToolBar(object sender, EventArgs e)
        {
            TxtSearch.IsVisible = true;
        }

        private async void TxtSearch_SearchButtonPressed(object sender, EventArgs e)
        {
            string searchValue = TxtSearch.Text;
            if (!String.IsNullOrEmpty(searchValue))
            {
                var mess = await messageRepository.GetAllByName(searchValue);
                DatabaseItems.ItemsSource = null;
                DatabaseItems.ItemsSource = mess;
              
            }
            else
            {
                OnAppearing();
            }
        }     

        private async void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchValue = TxtSearch.Text;
            if (!String.IsNullOrEmpty(searchValue))
            {
               var mess = await messageRepository.GetAllByName(searchValue);
                DatabaseItems.ItemsSource = null;
                DatabaseItems.ItemsSource = mess;
            }
            else
            {
                OnAppearing();
            }
        }

        async void VideoSwipe_Invoked(System.Object sender, System.EventArgs e)
        {
            id = ((MenuItem)sender).CommandParameter.ToString();
            message = await messageRepository.GetById(id);
            message.Id = id;
            await Navigation.PushAsync(new VideoPage(message.Video));
        }

        async void PhotoSwipe_Invoked(object sender, EventArgs e)
        {
            id = ((MenuItem)sender).CommandParameter.ToString();
            message = await messageRepository.GetById(id);
            message.Id = id;
            await Navigation.PushAsync(new PhotoPage(message.File));
        }

       public bool isPlaying = false;

       /* async void AudioTap_Invoked(object sender, EventArgs e)
        {
          /*  myDatanaseRecord.IsPlaying = !myDatanaseRecord.IsPlaying;

             id = ((TappedEventArgs)e).Parameter.ToString();
           // id = ((MenuItem)sender).CommandParameter.ToString();
          
            message = await messageRepository.GetById(id);
            message.Id = id;
           // messageRepository.PlayMusic(message, isPlaying);

            var mediaInfo = CrossMediaManager.Current;

            if (myDatanaseRecord.IsPlaying)
            {
                await mediaInfo.Play(message.Audio);
            }
            else
            {  await mediaInfo.Pause(); }
        }*/
        //public string PlayIcon { get => isPlaying ? "pause.png" : "play.png"; }
    }
}