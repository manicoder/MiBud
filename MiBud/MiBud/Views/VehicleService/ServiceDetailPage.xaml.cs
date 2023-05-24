using MiBud.Models;
using MiBud.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.VehicleService
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServiceDetailPage : ContentPage
    {
        ServiceDetailViewModel viewModel;
        public ServiceDetailPage(JobcardResult jobcard_detail)
        {
            InitializeComponent();
            BindingContext = viewModel = new ServiceDetailViewModel(jobcard_detail);
        }
    }
}