using MiBud.Models;
using MiBud.ViewModels;
using MiBud.Views.CreateMobitekTicket;
using MiBud.Views.CreateRSAngelTicket;
using MiBud.Views.CreateWikitekTicket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.CreateServiceTicket
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateServiceTicketPage : ContentPage
    {
        CreateServiceTicketViewModel viewModel;
        Vehicle selected_vehicle;
        string selected_page = "wikitek";
        public CreateServiceTicketPage(Vehicle selected_vehicle)
        {
            InitializeComponent();
            BindingContext = viewModel = new CreateServiceTicketViewModel();
            this.selected_vehicle = selected_vehicle;
            //img_toolbaritem.IconImageSource = "blue.png";

            Position position = new Position(22.6949509, 75.8894909);
            //Pin pin = new Pin
            //{
            //    Label = "Santa Cruz",
            //    Address = "The city with a boardwalk",
            //    Type = PinType.Place,
            //    Position = position
            //};
            //map.Pins.Add(pin);

            Device.StartTimer(new TimeSpan(0, 0, 10), () =>
            {
 
                // do something every 60 seconds
                Device.BeginInvokeOnMainThread(() =>
                {
                    GetWorkshops();
                });
                return true; // runs again, or false to stop
            });


        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await GetWorkshops();
        }

        private async Task GetWorkshops()
        {
            await viewModel.GetWorkshop();
           
            foreach (var item in viewModel.AllPins)
            {
                map.Pins.Add(item);
            }
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(viewModel.AllPins.FirstOrDefault().Position, Distance.FromKilometers(10));
            map.MoveToRegion(mapSpan);
 
            pin.Clicked += delegate
            {
                if (App.selectedIcon == "wikitek")
                {
                    this.Navigation.PushAsync(new CreateWikitekTicketPage(selected_vehicle, viewModel.selected_workshops));
                }
                else if (App.selectedIcon == "mobitek")
                {
                    this.Navigation.PushAsync(new CreateMobitekTicketPage(selected_vehicle));
                }
                else if (App.selectedIcon == "rsangel")
                {
                    this.Navigation.PushAsync(new CreateRSAngelTicketPage(selected_vehicle));
                }
            }; 
            //map.Pins.Add(pin);
         }





        //private void MenuIcon_Tapped(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var Id = sender as Grid;
        //        string GirdClassId = Id.ClassId;
        //        viewModel.wikitek_color = (Color)Application.Current.Resources["tab_unselect_color"];
        //        viewModel.mobitek_color = (Color)Application.Current.Resources["tab_unselect_color"];
        //        viewModel.rsangel_color = (Color)Application.Current.Resources["tab_unselect_color"];

        //        img_wikitek.Scale = 1;
        //        img_mobitek.Scale = 1;
        //        img_rsangel.Scale = 1;

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

        private void AddPinsToMap()
        {

        }
 
        //        switch (GirdClassId)
        //        {
        //            case "wikitek":
        //                viewModel.wikitek_color = (Color)Application.Current.Resources["theme_color"];
        //                img_wikitek.Scale = 1.4;
        //                selected_page = "wikitek";
        //                img_toolbaritem.IconImageSource = "blue.png";
        //                break;

        //            case "mobitek":
        //                viewModel.mobitek_color = (Color)Application.Current.Resources["theme_color"];
        //                img_mobitek.Scale = 1.4;
        //                selected_page = "mobitek";
        //                img_toolbaritem.IconImageSource = "orange.png";
        //                break;

        //            case "rsangel":
        //                viewModel.rsangel_color = (Color)Application.Current.Resources["theme_color"];
        //                img_rsangel.Scale = 1.4;
        //                selected_page = "rsangel";
        //                img_toolbaritem.IconImageSource = "green.png";
        //                break;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //private void ToolbarItem_Clicked(object sender, EventArgs e)
        //{
        //    if(selected_page== "wikitek")
        //    {
        //        this.Navigation.PushAsync(new CreateWikitekTicketPage(selected_vehicle,viewModel.selected_workshops));
        //    }
        //    else if(selected_page == "mobitek")
        //    {
        //        this.Navigation.PushAsync(new CreateMobitekTicketPage(selected_vehicle));
        //    }
        //    else if(selected_page == "rsangel")
        //    {
        //        this.Navigation.PushAsync(new CreateRSAngelTicketPage(selected_vehicle));
        //    }
        //    //this.Navigation.PushAsync(new CreateRSAngelTicketPage());
        //}
 
    }
}