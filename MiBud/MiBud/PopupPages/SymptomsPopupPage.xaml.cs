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
    public partial class SymptomsPopupPage : PopupPage
    {
        SymptomsViewModel viewModel;
        int submodel_id = 0;
        public SymptomsPopupPage(int submodel_id)
        {
            InitializeComponent();
            BindingContext = viewModel = new SymptomsViewModel(submodel_id);
            this.submodel_id = submodel_id;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                symptoms_List.ItemsSource = viewModel.symotomps_list.Where(x => x.symptom.symptom_name.ToLower().Contains(e.NewTextValue.ToLower()));
            }
            catch (Exception ex)
            {
            }

        }

        private async void symptoms_List_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var selected_symptoms= (e.Item as SymptomRtModel);
                //string[] data = new string[2];
                //data[0] = Convert.ToString(submodel_id);
               //data[1] = selected_symptoms_name;

                MessagingCenter.Send<SymptomsPopupPage, SymptomRtModel>(this, "selected_symptoms", selected_symptoms);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
            }
        }

        private async void close_popup(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}