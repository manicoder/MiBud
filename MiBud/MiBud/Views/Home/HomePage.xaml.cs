using MiBud.Models;
using MiBud.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        HomeViewModel viewModel;
        public HomePage(Vehicle vehicle)
        {
            try
            {

                InitializeComponent();
                BindingContext = viewModel = new HomeViewModel(this, vehicle);
                selected_vehicle_picture = App.selected_vehicle_picture;
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
    }
}