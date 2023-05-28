using MiBud.Models;
using MiBud.ViewModels;
using MiBud.Views.CreateMobitekTicket;
using MiBud.Views.CreateRSAngelTicket;
using MiBud.Views.CreateWikitekTicket;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        HomeViewModel viewModel;
        string selected_page = "wikitek";
        Vehicle selected_vehicle;
        public HomePage(Vehicle vehicle)
        {
            try
            {

                InitializeComponent();
                BindingContext = viewModel = new HomeViewModel(this, vehicle);
                selected_vehicle_picture = App.selected_vehicle_picture;
                img_toolbaritem.IconImageSource = "blue.png";
                App.selectedIcon = string.Empty;
                App.selected_vehicle_Service = string.Empty;
            }
            catch (System.Exception ex)
            {
            }
        }

        private string _selected_vehicle_picture;
        public string selected_vehicle_picture
        {
            get => _selected_vehicle_picture;
            set
            {
                _selected_vehicle_picture = value;
                OnPropertyChanged("selected_vehicle_picture");
            }
        }
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
           if (selected_page == "wikitek")
            {
                this.Navigation.PushAsync(new CreateWikitekTicketPage(selected_vehicle, viewModel.selected_workshops));
            }
            else if (selected_page == "mobitek")
            {
                this.Navigation.PushAsync(new CreateMobitekTicketPage(selected_vehicle));
            }
            else if (selected_page == "rsangel")
            {
                this.Navigation.PushAsync(new CreateRSAngelTicketPage(selected_vehicle));
            }
            //this.Navigation.PushAsync(new CreateRSAngelTicketPage());
        }

        private void MenuIcon_Tapped(object sender, EventArgs e)
        {
            try
            {
                var Id = sender as Grid;
                string GirdClassId = Id.ClassId;
                viewModel.wikitek_color = (Color)Application.Current.Resources["tab_unselect_color"];
                viewModel.mobitek_color = (Color)Application.Current.Resources["tab_unselect_color"];
                viewModel.rsangel_color = (Color)Application.Current.Resources["tab_unselect_color"];

                img_wikitek.Scale = 1;
                img_mobitek.Scale = 1;
                img_rsangel.Scale = 1;

                switch (GirdClassId)
                {
                    case "wikitek":
                        viewModel.wikitek_color = (Color)Application.Current.Resources["theme_color"];
                        img_wikitek.Scale = 1.4;
                        selected_page = "wikitek";
                        img_toolbaritem.IconImageSource = "blue.png";
                        App.selectedIcon = "wikitek";
                        App.selected_vehicle_Service = "wikitekMechanik";
                        break;

                    case "mobitek":
                        viewModel.mobitek_color = (Color)Application.Current.Resources["theme_color"];
                        img_mobitek.Scale = 1.4;
                        selected_page = "mobitek";
                        img_toolbaritem.IconImageSource = "orange.png";
                        App.selectedIcon = "mobitek";
                        App.selected_vehicle_Service = "mobitekMechanik";

                        break;

                    case "rsangel":
                        viewModel.rsangel_color = (Color)Application.Current.Resources["theme_color"];
                        img_rsangel.Scale = 1.4;
                        selected_page = "rsangel";
                        img_toolbaritem.IconImageSource = "green.png";
                        App.selectedIcon = "rsangel";
                        App.selected_vehicle_Service = "RSAngelMechanik";

                        break;

                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}