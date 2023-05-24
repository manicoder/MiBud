using MiBud.Models;
using MiBud.PopupPages;
using MiBud.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.CreateMobitekTicket
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateMobitekTicketPage : ContentPage
    {
        CreateMobitekTicketViewModel viewModel;
        Vehicle selected_vehicle;
        public CreateMobitekTicketPage(Vehicle selected_vehicle)
        {
            InitializeComponent();
          
            BindingContext = viewModel = new CreateMobitekTicketViewModel();
            this.selected_vehicle = selected_vehicle;

            MessagingCenter.Subscribe<SymptomsPopupPage, string[]>(this, "selected_symptoms", async (sender, arg) =>
            {
                var item = viewModel.symotomps_list.FirstOrDefault(x => x.id == Convert.ToInt32(arg[0]));
                item.name = arg[1];
                item.text_color = (Color)Application.Current.Resources["text_color"];
            });

            MessagingCenter.Subscribe<ServicesPopupPage, string[]>(this, "selected_service", async (sender, arg) =>
            {
                var item = viewModel.services_list.FirstOrDefault(x => x.id == Convert.ToInt32(arg[0]));
                item.name = arg[1];
                item.text_color = (Color)Application.Current.Resources["text_color"];
            });
        }


        [Obsolete]
        private void select_symptoms_clicked(object sender, EventArgs e)
        {
            try
            {
                var selectedItem = (CreateWikitekTicketModel)((Grid)sender).BindingContext;
                if (selectedItem.name == "Symptom")
                {
                }
                else
                {
                    var last_item = viewModel.symotomps_list.LastOrDefault().id;
                    if (last_item == selectedItem.id)
                    {
                        PopupNavigation.PushAsync(new SymptomsPopupPage(selectedItem.id));
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void create_new_symptoms_clicked(object sender, EventArgs e)
        {
            var first_item = viewModel.symotomps_list.FirstOrDefault();
            first_item.add_btn_visible = false;
            first_item.right_btn_visible = false;
            first_item.delete_btn_visible = true;
            var item = viewModel.symotomps_list.LastOrDefault();
            item.add_btn_visible = false;
            item.delete_btn_visible = true;
            item.line_visible = false;
            item.drop_down_visible = false;
            CreateWikitekTicketModel model = new CreateWikitekTicketModel
            {
                id = item.id + 1,
                name = "Add symptoms",
                text_color = (Color)Application.Current.Resources["placeholder_color"],
                add_btn_visible = true,
                delete_btn_visible = false,
                right_btn_visible = false,
                line_visible = true,
                drop_down_visible = true,
            };
            viewModel.symotomps_list.Add(model);
            if (Device.Idiom == TargetIdiom.Phone)
            {
                viewModel.symotomps_list_height = viewModel.symotomps_list.Count * 56;
            }
            else
            {
                viewModel.symotomps_list_height = viewModel.symotomps_list.Count * 66;
            }
        }

        private void delete_symptoms_clicked(object sender, EventArgs e)
        {
            try
            {
                var selectedItem = (CreateWikitekTicketModel)((ImageButton)sender).BindingContext;
                if (viewModel.symotomps_list.Count > 1)
                {
                    if (selectedItem.id == 1)
                    {
                        if (viewModel.symotomps_list.Count == 2)
                        {
                            var first_item = viewModel.symotomps_list.FirstOrDefault();
                            first_item.add_btn_visible = true;
                            first_item.delete_btn_visible = false;
                            first_item.right_btn_visible = false;
                            viewModel.symotomps_list.RemoveAt(viewModel.symotomps_list.Count - 1);
                        }
                        else
                        {
                            viewModel.symotomps_list.RemoveAt(viewModel.symotomps_list.Count - 1);
                            var last_item = viewModel.symotomps_list.LastOrDefault();
                            last_item.add_btn_visible = true;
                            last_item.delete_btn_visible = false;
                            last_item.right_btn_visible = false;
                        }
                    }
                    else
                    {
                        viewModel.symotomps_list.Remove(selectedItem);
                        var item = viewModel.symotomps_list.LastOrDefault();
                        item.add_btn_visible = true;
                        item.delete_btn_visible = false;
                    }
                }

                if (Device.Idiom == TargetIdiom.Phone)
                {
                    viewModel.symotomps_list_height = viewModel.symotomps_list.Count * 56;
                }
                else
                {
                    viewModel.symotomps_list_height = viewModel.symotomps_list.Count * 66;
                }
            }
            catch (Exception ex)
            {
            }
        }

        [Obsolete]
        private void select_service_clicked(object sender, EventArgs e)
        {
            try
            {
                var selectedItem = (CreateWikitekTicketModel)((Grid)sender).BindingContext;
                if (selectedItem.name == "Services (INR 250)")
                {
                }
                else
                {
                    var last_item = viewModel.services_list.LastOrDefault().id;
                    if (last_item == selectedItem.id)
                    {
                        PopupNavigation.PushAsync(new ServicesPopupPage(1,selectedItem.id));
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void create_new_service_clicked(object sender, EventArgs e)
        {
            var first_item = viewModel.services_list.FirstOrDefault();
            first_item.add_btn_visible = false;
            first_item.delete_btn_visible = true;
            first_item.right_btn_visible = false;
            var item = viewModel.services_list.LastOrDefault();
            item.add_btn_visible = false;
            item.delete_btn_visible = true;
            item.line_visible = false;
            item.drop_down_visible = false;
            CreateWikitekTicketModel model = new CreateWikitekTicketModel
            {
                id = item.id + 1,
                name = "Add services",
                text_color = (Color)Application.Current.Resources["placeholder_color"],
                add_btn_visible = true,
                delete_btn_visible = false,
                right_btn_visible = false,
                line_visible = true,
                drop_down_visible = true,
            };
            viewModel.services_list.Add(model);
            if (Device.Idiom == TargetIdiom.Phone)
            {
                viewModel.services_list_height = viewModel.services_list.Count * 56;
            }
            else
            {
                viewModel.services_list_height = viewModel.services_list.Count * 66;
            }

        }

        private void delete_services_clicked(object sender, EventArgs e)
        {
            var selectedItem = (CreateWikitekTicketModel)((ImageButton)sender).BindingContext;
            if (viewModel.services_list.Count > 1)
            {
                if (selectedItem.id == 1)
                {
                    if (viewModel.services_list.Count == 2)
                    {
                        var first_item = viewModel.services_list.FirstOrDefault();
                        first_item.add_btn_visible = true;
                        first_item.delete_btn_visible = false;
                        first_item.right_btn_visible = false;
                        viewModel.services_list.RemoveAt(viewModel.services_list.Count - 1);
                    }
                    else
                    {
                        viewModel.services_list.RemoveAt(viewModel.services_list.Count - 1);
                        var last_item = viewModel.services_list.LastOrDefault();
                        last_item.add_btn_visible = true;
                        last_item.delete_btn_visible = false;
                        last_item.right_btn_visible = false;
                    }
                }
                else
                {
                    viewModel.services_list.Remove(selectedItem);
                    var item = viewModel.services_list.LastOrDefault();
                    item.add_btn_visible = true;
                    item.delete_btn_visible = false;
                }
            }

            if (Device.Idiom == TargetIdiom.Phone)
            {
                viewModel.services_list_height = viewModel.services_list.Count * 56;
            }
            else
            {
                viewModel.services_list_height = viewModel.services_list.Count * 66;
            }

        }
    }
}