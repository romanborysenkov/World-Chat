using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Models.TypeModels;
using MediaManager;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using Firebase.Database.Query;

namespace Chat.Models
{
    public class MessageRepository
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://international-chat-60ea5-default-rtdb.europe-west1.firebasedatabase.app/");

        FirebaseStorage firebaseStorage = new FirebaseStorage("international-chat-60ea5.appspot.com");

      

        public async Task<List<MyDatabaseRecord>> GetAll()
        {
            return (await firebaseClient.Child("Records").OnceAsync<MyDatabaseRecord>()).Select(item => new MyDatabaseRecord
            {
                Message = item.Object.Message,
                SendTime = item.Object.SendTime,       
                UserName = item.Object.UserName,
                File = item.Object.File,
                Video = item.Object.Video,
                isFile = item.Object.isFile,
                isVideo = item.Object.isVideo,
                Id = item.Key
            }).ToList();
        }

        public async Task<List<MyDatabaseRecord>> GetAllByName(string name)
        {
            return (await firebaseClient.Child("Records").OnceAsync<MyDatabaseRecord>()).Select(item => new MyDatabaseRecord
            {
                Message = item.Object.Message,
                SendTime = item.Object.SendTime,
                File = item.Object.File,
                Video = item.Object.Video,
                isVideo = item.Object.isVideo,
                isFile = item.Object.isFile,
                UserName = item.Object.UserName,
                Id = item.Key
            }).Where(c => c.Message.ToLower().Contains(name.ToLower())).ToList(); //Where(c => c.Message.ToLower().Contains(name.ToLower())).ToList();
        }

        public async Task<bool> Delete(string id)
        {
            await firebaseClient.Child("Records" + "/" + id).DeleteAsync();
            // await firebaseStorage.Child("Files").Child(fileName).DeleteAsync();
            return true;
        }

        public async Task<MyDatabaseRecord> GetById(string id)
        {
            return (await firebaseClient.Child("Records" + "/" + id).OnceSingleAsync<MyDatabaseRecord>());
        }

        public async Task<bool> Update(MyDatabaseRecord mydb)
        {
            await firebaseClient.Child("Records" + "/" + mydb.Id).PutAsync(JsonConvert.SerializeObject(mydb));

            return true;
        }

        public async Task<string> Upload(Stream img, string fileName)
        {
            var image = await firebaseStorage.Child("Files").Child(fileName).PutAsync(img);
            return image;
            // return "";
        }
 
    }
}
