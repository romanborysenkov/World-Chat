using System;
using MediaManager;
using Xamarin.Forms;
using Chat.Models.TypeModels;

namespace Chat.Models.TypeModels
{
    public class MyDatabaseRecord //: BaseViewModel
    {
        public string Id { get; set; }
        public string SendTime { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string File { get; set; }
        public string Video { get; set; }
        public bool isFile { get; set; }
        public bool isVideo { get; set; }
    }
}
