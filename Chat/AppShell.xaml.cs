using System;
using System.Collections.Generic;
using Chat.ViewModels;
using Chat.Views;
using Xamarin.Forms;

namespace Chat
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("main/login", typeof(LoginPage));
            Routing.RegisterRoute("main/signup", typeof(NewUserPage));

            BindingContext = this;

        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
