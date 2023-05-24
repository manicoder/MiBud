using MiBud.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.Trips
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TripsPage : ContentPage
    {
        TripsViewModel viewModel;
        public TripsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new TripsViewModel();

            Position position = new Position(22.6949509, 75.8894909);
            Pin pin = new Pin
            {
                Label = "Santa Cruz",
                Address = "The city with a boardwalk",
                Type = PinType.Place,
                Position = position
            };
            map.Pins.Add(pin);
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(0.444));
            map.MoveToRegion(mapSpan);
        }
    }
}