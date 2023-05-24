using MiBud.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiBud.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DongleListPopupPage : PopupPage
    {
        DongleListViewModel viewModel;
        public DongleListPopupPage(string btnText, string vehicle_id)
        {
            InitializeComponent(); 
            BindingContext = viewModel = new DongleListViewModel(btnText, vehicle_id);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var vm = this.BindingContext as DongleListViewModel;
            if (vm != null)
            {
                vm.ClosebtPopup += Vm_ClosePopup;
            }
        }

        private async void Vm_ClosePopup(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}