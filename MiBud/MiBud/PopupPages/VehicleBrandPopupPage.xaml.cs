using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiBud.Models;
using MiBud.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiBud.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VehicleBrandPopupPage : PopupPage
    {
        VehicleBrandViewModel viewModel;
        public VehicleBrandPopupPage(int segment_id)
        {
            InitializeComponent();
            BindingContext = viewModel = new VehicleBrandViewModel(segment_id);
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var vm = this.BindingContext as VehicleBrandViewModel;
            if (vm != null)
            {
                vm.CloseCountryPopup += Vm_CloseCountryPopup;
            }
        }

        private async void Vm_CloseCountryPopup(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}