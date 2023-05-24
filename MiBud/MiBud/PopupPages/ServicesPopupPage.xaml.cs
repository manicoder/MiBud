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
    public partial class ServicesPopupPage : PopupPage
    {
        ServicesViewModel viewModel;
        int item_id = 0;
        public ServicesPopupPage(int item_id,int submodel_id)
        {
            InitializeComponent();
            BindingContext = viewModel = new ServicesViewModel(submodel_id);
            this.item_id = item_id;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                services_List.ItemsSource = viewModel.service_list.Where(x => x.service.service_name.ToLower().Contains(e.NewTextValue.ToLower()));
            }
            catch (Exception ex)
            {
            }

        }

        private async void close_popup(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private async void services_List_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var selected_service = (e.Item as ServiceModel);
                //string[] data = new string[2];
                //data[0] = Convert.ToString(item_id);
                //data[1] = selected_service;

                MessagingCenter.Send<ServicesPopupPage, ServiceModel>(this, "selected_service", selected_service);
                await PopupNavigation.Instance.PopAsync();

                //MessagingCenter.Send<ServicesPopupPage, string[]>(this, "selected_service", data);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
            }
        }
    }
}