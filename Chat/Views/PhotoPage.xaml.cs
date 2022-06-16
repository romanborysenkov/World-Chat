using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Chat.Views
{
    public partial class PhotoPage : ContentPage
    {
        public PhotoPage(string photo)
        {
            InitializeComponent();

            fullPhoto.Source = photo;
           // fullPhoto.Source = photo;
        }
    }
}
