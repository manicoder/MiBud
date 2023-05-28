using MiBud.Views.Otp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.ForgotPassword
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();
        }

        private void submit_Clicked(object sender, EventArgs e)
        {

            string description = "Forgot Password An OTP has been sent to your mobile and will be valid for 10 mins.Pls enter the OTP here";
            //this.Navigation.PushAsync(new OtpPage(false, description));
        }
    }
}