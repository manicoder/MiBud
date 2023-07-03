using MiBud.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.MobileOtp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MobileOtp : ContentPage
    {
        MobileOtpViewModel vm;
        public MobileOtp()
        {
            InitializeComponent();
            BindingContext = vm = new MobileOtpViewModel(this);
            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            otp1.Focus();
        }

        private void otp2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (otp2.Text.Length == 1)
                {
                    otp3.Focus();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void otp1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (otp1.Text.Length == 1)
                {
                    otp2.Focus();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void otp3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (otp3.Text.Length == 1)
                {
                    otp4.Focus();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        
    }
}