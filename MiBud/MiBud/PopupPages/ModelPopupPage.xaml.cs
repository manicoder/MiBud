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
    public partial class ModelPopupPage : PopupPage
    {
        VehicleModelViewModel viewModel;
        public ModelPopupPage(List<int> oemlist,int segmentId)
        {
            InitializeComponent();
            BindingContext = viewModel = new VehicleModelViewModel(oemlist, segmentId);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var vm = this.BindingContext as VehicleModelViewModel;
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