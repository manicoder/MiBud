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
using System.Collections.ObjectModel;

namespace MiBud.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubModelPopupPage : PopupPage
    {
        VehicleSubModelViewModel viewModel;
        public SubModelPopupPage(ObservableCollection<VehicleSubModel> sub_model)
        {
            InitializeComponent();
            BindingContext = viewModel = new VehicleSubModelViewModel(sub_model);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var vm = this.BindingContext as VehicleSubModelViewModel;
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