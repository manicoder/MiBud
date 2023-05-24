using System;
using System.Collections.Generic;
using System.Linq;
using MiBud.ViewModels;
using MiBud.Views.PrivacyPolicy;
using MiBud.Views.ResetPassword;
using MiBud.Views.TermsAndConditions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.Otp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OtpPage : ContentPage
    {
        bool forgot_pass_page = false;
        OtpViewModel viewModel;
        public OtpPage(bool termcondition_visible, bool otp_visible, string description)
        {
            InitializeComponent();
            BindingContext = viewModel = new OtpViewModel(this, termcondition_visible, otp_visible, description);
        }

        //private void term_and_condition_Tapped(object sender, EventArgs e)
        //{
        //    this.Navigation.PushAsync(new TermsAndConditionsPage());
        //}

        //private void priacy_policy_Tapped(object sender, EventArgs e)
        //{
        //    this.Navigation.PushAsync(new PrivacyPolicyPage());
        //}

        //private void submit_Clicked(object sender, EventArgs e)
        //{
        //    if(!forgot_pass_page)
        //    {
        //        Navigation.PushAsync(new ResetPasswordPage(""));
        //    }
        //}

        private void FirstDigit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                SecondDigit?.Focus();
            }
        }

        private void SecondDigit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                ThirdDigit?.Focus();
            }
        }

        private void ThirdDigit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                FourthDigit?.Focus();
            }
        }

        private void FourthDigit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                FifthDigit?.Focus();
            }
        }

        private void FifthDigit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                SixthDigit?.Focus();
            }
        }

        private void SixthDigit_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}