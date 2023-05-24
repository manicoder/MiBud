using System;
using MiBud.Models;
using MiBud.ViewModels;
using MiBud.Views.Login;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.MasterDetail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailView : MasterDetailPage
    {
        MasterViewModel viewModel;
        public MasterDetailView(LoginResponse user)
        {
            InitializeComponent();
            BindingContext = viewModel = new MasterViewModel(0, user);
            //Detail = new NavigationPage(new MyJobCardPage());
            IsPresented = false;
        }

        private void menu_clicked(object sender, EventArgs e)
        {

        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var item = e.SelectedItem as MasterModel;
                if (item != null)
                {
                    if (item.title == "Logout")
                    {
                        //Preferences.Set("LoginResponse", "");
                        //Preferences.Set("user_name", "");
                        //Preferences.Set("password", "");
                        Preferences.Set("token", "");
                        Preferences.Set("IsUpdate", "true");
                        App.Current.MainPage = new NavigationPage(new LoginPage());
                    }
                    if(item.title == "Self Diagnose")
                    {
                        if(App.dongle_connected)
                        {
                            Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                        }
                        else
                        {
                            await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Dongle not connected.\nFirstly connect dongle.", "Alert!", "Ok");
                        }
                    }
                    else
                    {
                        Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                        listView.SelectedItem = null;
                    }
                    IsPresented = false;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void profile_Tapped(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new UserPage(false));
        }
    }
}