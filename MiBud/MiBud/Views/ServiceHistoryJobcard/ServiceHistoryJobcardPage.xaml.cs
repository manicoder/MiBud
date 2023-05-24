using MiBud.Models;
using MiBud.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.ServiceHistoryJobcard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServiceHistoryJobcardPage : ContentPage
    {
        ServiceHistoryJobcardViewModel viewModel;
        public ServiceHistoryJobcardPage(ServiceHistoryModel model)
        {
            InitializeComponent();
            BindingContext = viewModel = new ServiceHistoryJobcardViewModel();
            this.Title = model.jobcard_no;
            if(model.type=="wikitek")
            {
                img.Source = model.image;
                txt_title.Text = "Wikitek mechanik";
                txt_id.Text = "Workshop ID";
                txt_name.Text = "Workshop Name";
                txt_manager.Text = "Workshop Manager";
            }
            else if (model.type == "rsangel")
            {
                img.Source = model.image;
                txt_title.Text = "RSAngel mechanik";
                txt_id.Text = "RSAngel ID";
                txt_name.Text = "RSAngel Name";
                grd.RowDefinitions[3].Height = 0;
            }
            else if (model.type == "mobitek")
            {
                img.Source = model.image;
                txt_title.Text = "Mobitek mechanik";
                txt_id.Text = "Mobitek ID";
                txt_name.Text = "Mobitek Name";
                grd.RowDefinitions[3].Height = 0;
            }

            foreach (var symtom_item in viewModel.symotomps_list)
            {
                symtom_item.right_btn_visible = true;
                symtom_item.delete_btn_visible = false;
                symtom_item.add_btn_visible = false;
            }

            foreach (var spare_item in viewModel.spare_parts_list)
            {
                spare_item.right_btn_visible = true;
                spare_item.delete_btn_visible = false;
                spare_item.add_btn_visible = false;
            }

            foreach (var services_item in viewModel.services_list)
            {
                services_item.right_btn_visible = true;
                services_item.delete_btn_visible = false;
                services_item.add_btn_visible = false;
            }
        }

        private void wrong_btn_Clicked(object sender, EventArgs e)
        {
            var selectedItem = (CreateWikitekTicketModel)((ImageButton)sender).BindingContext;
            if (selectedItem.right_btn_visible)
            {
                selectedItem.right_btn_visible = false;
                selectedItem.wrong_btn_visible = true;
            }
            else
            {
                selectedItem.right_btn_visible = true;
                selectedItem.wrong_btn_visible = false;
            }

        }
    }
}