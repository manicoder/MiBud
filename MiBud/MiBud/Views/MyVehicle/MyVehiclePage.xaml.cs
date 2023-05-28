using MiBud.ViewModels;
using MiBud.Views.NewVehicle;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.MyVehicle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyVehiclePage : ContentPage
    {
        MyVehicleViewModel viewModel;
        public MyVehiclePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MyVehicleViewModel(this);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ChangeStatusColor((Color)Application.Current.Resources["theme_color"]);
        }
        private static void ChangeStatusColor(Color color)
        {
            var statusbar = DependencyService.Get<IStatusBarPlatformSpecific>();
            statusbar.SetStatusBarColor(color);

            var mdPage = Application.Current.MainPage as MasterDetailPage;
            var navPage = mdPage.Detail as NavigationPage;
            navPage.BarBackgroundColor = color;
        }
        //private async void action_clicked(object sender, EventArgs e)
        //{
        //    //await Navigation.PushAsync(new JobCardDetailPage(null, "EntryCheckPage"));
        //    //await PopupNavigation.Instance.PopAsync();
        //}

        //[Obsolete]
        //private void ImageButton_Clicked(object sender, EventArgs e)
        //{
        //    //PopupNavigation.PushAsync(new EntryCheckPopupPage());
        //}

        //private void add_new_vehicle_Clicked(object sender, EventArgs e)
        //{
        //    this.Navigation.PushAsync(new AddNewVehiclePage());
        //}
    }
}