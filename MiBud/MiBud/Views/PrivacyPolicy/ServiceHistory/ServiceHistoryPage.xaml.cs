using MiBud.Models;
using MiBud.ViewModels;
using MiBud.Views.ServiceHistoryJobcard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.ServiceHistory
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServiceHistoryPage : ContentPage
    {
        ServiceHistoryViewModel viewModel;
        public ServiceHistoryPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ServiceHistoryViewModel(this);
        }

        private async void action_clicked(object sender, EventArgs e)
        {
            try
            {
                var selectedItem = (ServiceHistoryModel)((Grid)sender).BindingContext;
                await Navigation.PushAsync(new ServiceHistoryJobcardPage(selectedItem));
            }
            catch (Exception)
            {
            }
            //await PopupNavigation.Instance.PopAsync();
        }

        [Obsolete]
        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            //PopupNavigation.PushAsync(new EntryCheckPopupPage());
        }
    }
}