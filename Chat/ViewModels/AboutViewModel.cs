using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Chat.Models.TypeModels;
using Firebase.Database;
using Firebase.Storage;

namespace Chat.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {

        FirebaseClient firebaseClient = new FirebaseClient("https://international-chat-60ea5-default-rtdb.europe-west1.firebasedatabase.app/");

        FirebaseStorage firebaseStorage = new FirebaseStorage("international-chat-60ea5.appspot.com");

        

        public AboutViewModel()
        {
            messanges = GetAll();
        }

     //  private  Task<List<MyDatabaseRecord>> messanges;
        ObservableCollection<MyDatabaseRecord> messanges;

        public ObservableCollection<MyDatabaseRecord> Messanges
        {
            get { return messanges; }
            set
            {
                messanges = value;
                OnPropertyChanged();
            }
        }
       

     public string Message { get; set; } 
        public string SendTime { get; set; }
        public string UserName { get; set; }
        public string File { get; set; }
        public string Video { get; set; }
        public string Audio { get; set; }
        public bool isAudio { get; set; }
        public bool isFile { get; set; }
        public bool isVideo { get; set; }
        public string Id { get; set; }

        private ObservableCollection<MyDatabaseRecord> GetAll()
         {

            var mesange = firebaseClient.Child("Records")
                .AsObservable<MyDatabaseRecord>()
                .AsObservableCollection();

            return mesange;
            /* return (await firebaseClient.Child("Records").OnceAsync<MyDatabaseRecord>()).Select(item => new MyDatabaseRecord
             {
                 Message = item.Object.Message,
                 SendTime = item.Object.SendTime,
                 UserName = item.Object.UserName,
                 File = item.Object.File,
                 Video = item.Object.Video,
                 Audio = item.Object.Audio,
                 isAudio = item.Object.isAudio,
                 isFile = item.Object.isFile,
                 isVideo = item.Object.isVideo,
                 Id = item.Key
             }).ToList();*/

          }
    }
}
