using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MiBud.Models;
using MiBud.Services;
using MiBud.StaticInfo;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MiBud.ViewModels
{
    public class VehicleModelViewModel : BaseViewModel
    {
        ApiServices apiServices;
        public event EventHandler CloseCountryPopup;
        public VehicleModelViewModel(string oem)
        {
            apiServices = new ApiServices();
            Device.InvokeOnMainThreadAsync(async () =>
            {
                IsBusy = true;
                await Task.Delay(10);

                await GetModelList(oem);
                IsBusy = false;
             
            });
            vehicle_model_list = new ObservableCollection<VehicleModelResult>();

            ClosePopupCommand = new Command(async (obj) =>
            {
                CloseCountryPopup?.Invoke("", new EventArgs());
            });

            ModelSelectionCommand = new Command(async (obj) =>
            {
                try
                {
                    selected_model = (VehicleModelResult)obj;
                    MessagingCenter.Send<VehicleModelViewModel, VehicleModelResult>(this, "selected_vehicle_model", selected_model);
                    CloseCountryPopup?.Invoke("", new EventArgs());
                }
                catch (Exception ex)
                {
                }
            });
        }

        private ObservableCollection<VehicleModelResult> _vehicle_model_static_list;
        public ObservableCollection<VehicleModelResult> vehicle_model_static_list
        {
            get => _vehicle_model_static_list;
            set
            {
                _vehicle_model_static_list = value;
                OnPropertyChanged("vehicle_model_static_list");
            }
        }

        private ObservableCollection<VehicleModelResult> _vehicle_model_list;
        public ObservableCollection<VehicleModelResult> vehicle_model_list
        {
            get => _vehicle_model_list;
            set
            {
                _vehicle_model_list = value;
                OnPropertyChanged("vehicle_model_list");
            }
        }

        private VehicleModelResult _selected_model;
        public VehicleModelResult selected_model
        {
            get => _selected_model;
            set
            {
                _selected_model = value;

                if (_selected_model != null)
                {
                    MessagingCenter.Send<VehicleModelViewModel, VehicleModelResult>(this, "selected_vehicle_model", selected_model);
                    CloseCountryPopup?.Invoke("", new EventArgs());
                }

                OnPropertyChanged("selected_model");
            }
        }


        #region Methods

        public async Task GetModelList(string oem)
        {
            var response = await apiServices.GetVehicleModel(Preferences.Get("token", null), oem,true);

            var api_status_code = StaticMethods.http_status_code(response.status_code);

            if (response == null)
            {
                await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Service is not working", "Failed", "Ok");
            }

            if (response.results.Count == 0)
            {
                await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Model not found for this brand", "Alert", "Ok");
                CloseCountryPopup?.Invoke("", new EventArgs());
            }

            if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
            {
                vehicle_model_list = _vehicle_model_static_list = new ObservableCollection<VehicleModelResult>(response.results);
            }
            else
            {
                await Acr.UserDialogs.UserDialogs.Instance.AlertAsync(response?.error, api_status_code, "Ok");
            }
        }

        #endregion

        public ICommand ClosePopupCommand { get; set; }
        public ICommand ModelSelectionCommand { get; set; }

    }
}
