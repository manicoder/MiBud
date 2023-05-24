using System;
using MiBud.Views.Otp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;
using MiBud.PopupPages;
using MiBud.ViewModels;

namespace MiBud.Views.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {

        RegistrationViewModel vm;
        public RegistrationPage()
        {
            try
            {
                InitializeComponent();

                txt_first_name.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeSentence);
                txt_last_name.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeSentence);

                BindingContext = vm = new RegistrationViewModel(this);

                //MessagingCenter.Subscribe<CountyPopupPage, string>(this, "selected_country", async (sender, arg) =>
                //{
                //    txt_country.Text = arg;
                //    txt_country.TextColor = (Color)Application.Current.Resources["text_color"];
                //});
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

        //    await DisplayAlert("File Location", file.Path, "OK");

        //    img_user.Source = ImageSource.FromStream(() =>
        //    {
        //        var stream = file.GetStream();
        //        return stream;
        //    });
        //}

        //private void create_new_workshop_clicked(object sender, EventArgs e)
        //{
        //    //this.Navigation.PushAsync(new CreateNewWorkshopPage());
        //}

        //private void submit_Clicked(object sender, EventArgs e)
        //{
        //    string description = "User Registration authentication An OTP has been sent to your mobile and will be valid for 10 mins.Pls enter the OTP here";
        //    this.Navigation.PushAsync(new OtpPage(true,description));
        //}

        //[Obsolete]
        //private void select_country_clicked(object sender, EventArgs e)
        //{
        //    PopupNavigation.PushAsync(new CountyPopupPage());
        //}
    }
}