using MiBud.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.ResetPassword
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetPasswordPage : ContentPage
    {
        ResetPasswordViewModel viewModel;
        public ResetPasswordPage(string url)
        {
            InitializeComponent();
            BindingContext = viewModel = new ResetPasswordViewModel(this, url);
        }
    }
}