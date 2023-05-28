using Acr.UserDialogs;
using MiBud.Models;
using MiBud.PopupPages;
using MiBud.ViewModels;
using MiBud.Views.AddAddress;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.CreateWikitekTicket
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateWikitekTicketPage : ContentPage
    {
        CreateWikitekTicketViewModel viewModel;
        Vehicle selected_vehicle;
        int last_symptom;
        string pickup_latlon =string.Empty;
        string drop_latlon = string.Empty;
        Services.ApiServices services;

        public CreateWikitekTicketPage(Vehicle selected_vehicle, WorkshopResult workshop)
        {
            InitializeComponent();

            BindingContext = viewModel = new CreateWikitekTicketViewModel(workshop);
            this.selected_vehicle = selected_vehicle;

            services = new Services.ApiServices();
            if (App.currentServiceLocation != null)
            {
                MapSpan mapSpan = MapSpan.FromCenterAndRadius(App.currentServiceLocation.Position, Distance.FromKilometers(10));
                map.MoveToRegion(mapSpan);
            }

            MessagingCenter.Subscribe<SymptomsPopupPage, SymptomRtModel>(this, "selected_symptoms", async (sender, arg) =>
            {
                try
                {
                    var item = viewModel.symotomps_list.FirstOrDefault(x => x.id == last_symptom);
                    item.name = Convert.ToString(arg.symptom.symptom_name);
                    item.id = arg.id;
                    item.text_color = (Color)Application.Current.Resources["text_color"];
                }
                catch (Exception ex)
                {
                }
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Unsubscribe<AddAddressPage, string[]>(this, "PickupAddress");
            MessagingCenter.Unsubscribe<AddAddressPage, string[]>(this, "DropAddress");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Subscribe<AddAddressPage, string[]>(this, "PickupAddress", async (sender, arg) =>
            {
                try
                {
                    txt_pickup_add.IsVisible = true;
                    txt_pickup_add.Text = "Pickup Address : " + arg[2];
                    pickup_latlon = arg[0] + "," + arg[1];
                }
                catch (Exception ex)
                {
                }
            });

            MessagingCenter.Subscribe<AddAddressPage, string[]>(this, "DropAddress", async (sender, arg) =>
            {
                try
                {
                    txt_drop_add.IsVisible = true;
                    txt_drop_add.Text = "Drop Address : " + arg[2];
                    drop_latlon= arg[0] + "," + arg[1];
                }
                catch (Exception ex)
                {
                }
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
                    last_symptom = viewModel.symotomps_list.LastOrDefault().id;
                    if (last_symptom == selectedItem.id)
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

        private async void Submit_Clicked(object sender, EventArgs e)
        {
            try
            {
                var res = await services.GenerateJobcardNumber(Xamarin.Essentials.Preferences.Get("token", null));

                if (!res.success)
                {
                    //DependencyService.Get<Interfaces.IToasts>().Show($"{ mode.status}");
                    UserDialogs.Instance.Toast(res.name, new TimeSpan(0, 0, 0, 3));
                    return;
                }

                CreateJobcardModel createJobcardModel = new CreateJobcardModel();

                createJobcardModel.job_card_name = res.name;
                createJobcardModel.status = "QuoteforTransport";
                createJobcardModel.registration_no = selected_vehicle.registration_id;
                createJobcardModel.workshop = viewModel.workshop.id;
                createJobcardModel.user_type = App.user.user_type;
                createJobcardModel.created_by = App.user.email;
                createJobcardModel.pickup = viewModel.pickup_check ? "YES" : "NO";
                createJobcardModel.drop = viewModel.drop_check ? "YES" : "NO";
                createJobcardModel.service_type = viewModel.selected_service;
                createJobcardModel.jobcard_symptom = new System.Collections.Generic.List<JobcardSymptomModel>();
                createJobcardModel.jobcard_pickupdrop = new System.Collections.Generic.List<JobcardPickupdrop>();
                for (int i = 0; i < (viewModel.symotomps_list.Count - 1)&& viewModel.symotomps_list.Count > 1; i++)
                {
                    createJobcardModel.jobcard_symptom.Add(
                        new JobcardSymptomModel
                        {
                            symptom = viewModel.symotomps_list[i+1].id,
                            customer_check = "YES",
                            entry_check = "NO",
                            service_check = "NO",
                            exit_check = "NO",
                            status = "YES",
                        });
                }

                if (viewModel.pickup_check)
                {
                    createJobcardModel.jobcard_pickupdrop.Add(new JobcardPickupdrop
                    {
                        address = txt_pickup_add.Text,
                        actual_time = DateTime.Now.ToString("yyyy-MM-dd"),
                        latlong = pickup_latlon,
                        //panned_time = "",
                        status = "Pickup_AssignTechnician",
                        //technician = "",
                        type = "Pickup",
                    });
                }

                if (viewModel.drop_check)
                {
                    createJobcardModel.jobcard_pickupdrop.Add(new JobcardPickupdrop
                    {
                        address = txt_drop_add.Text,
                        actual_time = DateTime.Now.ToString("yyyy-MM-dd"),
                        latlong = drop_latlon,
                        //panned_time = "",
                        status = "Pickup_AssignTechnician",
                        //technician = "",
                        type = "Drop",
                    });
                }

                var res1 = await services.CreateJobcard(Xamarin.Essentials.Preferences.Get("token", null), createJobcardModel);
               
                if (!res1.success)
                {
                    //DependencyService.Get<Interfaces.IToasts>().Show($"{ mode.status}");
                    UserDialogs.Instance.Toast(res.name, new TimeSpan(0, 0, 0, 3));
                    return;
                }

                UserDialogs.Instance.Toast("Ticket created.", new TimeSpan(0, 0, 0, 3));
            }
            catch (Exception ex)
            {
            }
        }

        private void AddPickupAddressClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddAddress.AddAddressPage("PickupAddress"));
        }

        private void AddDropAddressClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddAddress.AddAddressPage("DropAddress"));
        }
    }
}