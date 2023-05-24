using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MiBud.Models;
using MiBud.Services;
using MiBud.StaticInfo;
using Xamarin.Forms;

namespace MiBud.ViewModels
{
    public class VehicleSubModelViewModel : BaseViewModel
    {
        ApiServices apiServices;
        public event EventHandler CloseCountryPopup;

        public VehicleSubModelViewModel(ObservableCollection<VehicleSubModel> sub_model)
        {
            apiServices = new ApiServices();

            vehicle_sub_model_list = new ObservableCollection<VehicleSubModel>();
            vehicle_sub_model_static_list= new ObservableCollection<VehicleSubModel>();
            vehicle_sub_model_list = vehicle_sub_model_static_list = sub_model;
            Device.InvokeOnMainThreadAsync(async () =>
            {
                //await GetVehicleSubModelList(model_id);
            });

            ClosePopupCommand = new Command(async (obj) =>
            {
                CloseCountryPopup?.Invoke("", new EventArgs());
            });

            SubModelSelectionCommand = new Command(async (obj) =>
            {
                try
                {
                    selected_sub_model = (VehicleSubModel)obj;
                    MessagingCenter.Send<VehicleSubModelViewModel, VehicleSubModel>(this, "selected_vehicle_sub_model", selected_sub_model);
                    CloseCountryPopup?.Invoke("", new EventArgs());
                }
                catch (Exception ex)
                {
                }
            });
        }

        private ObservableCollection<VehicleSubModel> _vehicle_sub_model_static_list;
        public ObservableCollection<VehicleSubModel> vehicle_sub_model_static_list
        {
            get => _vehicle_sub_model_static_list;
            set
            {
                _vehicle_sub_model_static_list = value;
                OnPropertyChanged("vehicle_sub_model_static_list");
            }
        }

        private ObservableCollection<VehicleSubModel> _vehicle_sub_model_list;
        public ObservableCollection<VehicleSubModel> vehicle_sub_model_list
        {
            get => _vehicle_sub_model_list;
            set
            {
                _vehicle_sub_model_list = value;
                OnPropertyChanged("vehicle_sub_model_list");
            }
        }

        private VehicleSubModel _selected_sub_model;
        public VehicleSubModel selected_sub_model
        {
            get => _selected_sub_model;
            set
            {
                _selected_sub_model = value;

                if (selected_sub_model != null)
                {

                    MessagingCenter.Send<VehicleSubModelViewModel, VehicleSubModel>(this, "selected_vehicle_sub_model", selected_sub_model);
                    CloseCountryPopup?.Invoke("", new EventArgs());
                }

                OnPropertyChanged("selected_sub_model");
            }
        }

        #region Methods

        public async Task GetVehicleSubModelList(int model_id)
        {
            //var response = await apiServices.GetVehicleSubModel(StaticMethods.token, model_id,);

            //var api_status_code = StaticMethods.http_status_code(response.status_code);

            //if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
            //{
            //country_list = response.results;
            //vehicle_sub_model_list = vehicle_sub_model_static_list = new ObservableCollection<VehicleSubModel>(response.results);
            //}
            //else
            //{
            //}

        }

        #endregion

        public ICommand ClosePopupCommand { get; set; }
        public ICommand SubModelSelectionCommand { get; set; }
    }
}

