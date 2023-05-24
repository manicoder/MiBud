using Rg.Plugins.Popup.Services;
using System;
using MiBud.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using MiBud.PopupPages;
using MiBud.Models;

namespace MiBud.Views.NewVehicle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNewVehiclePage : ContentPage
    {
        AddNewVehicleViewMode viewMode;
        public AddNewVehiclePage(Vehicle vehicle)
        {
            try
            {
                InitializeComponent();

                BindingContext = viewMode = new AddNewVehicleViewMode(this, vehicle);
            }
            catch (Exception ex)
            {
            }
        }

        //private async void add_user_clicked(object sender, EventArgs e)
        //{
        //    await CrossMedia.Current.Initialize();

        //    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
        //    {
        //        DisplayAlert("No Camera", ":( No camera available.", "OK");
        //        return;
        //    }

        //    var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
        //    {
        //        Directory = "Sample",
        //        Name = "test.jpg"
        //    });

        //    if (file == null)
        //        return;

        //    //await DisplayAlert("File Location", file.Path, "OK");

        //    img_user.Source = ImageSource.FromStream(() =>
        //    {
        //        var stream = file.GetStream();
        //        return stream;
        //    });
        //}

        //[Obsolete]
        //private void select_vehicle_brand_clicked(object sender, EventArgs e)
        //{
        //    PopupNavigation.PushAsync(new VehicleBrandPopupPage());
        //}

        //[Obsolete]
        //private void select_model_clicked(object sender, EventArgs e)
        //{
        //    PopupNavigation.PushAsync(new ModelPopupPage());
        //}

        //[Obsolete]
        //private void select_sub_model_clicked(object sender, EventArgs e)
        //{
        //    PopupNavigation.PushAsync(new SubModelPopupPage());
        //}

        //[Obsolete]
        //private void select_model_year_clicked(object sender, EventArgs e)
        //{
        //    PopupNavigation.PushAsync(new ModelYearPopupPage());
        //}
    }
}