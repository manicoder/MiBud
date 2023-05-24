using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MiBud.Models;
using MiBud.Services;
using MiBud.StaticInfo;
using Xamarin.Forms;

namespace MiBud.ViewModels
{
    class VehicleModelYearViewModel : BaseViewModel
    {
        ApiServices apiServices;
        public event EventHandler CloseCountryPopup;
        public VehicleModelYearViewModel(ObservableCollection<VehicleSubModel> model_year)
        {
            apiServices = new ApiServices();

            vehicle_model_year_static_list = new ObservableCollection<VehicleSubModel>();
            vehicle_model_year_list = new ObservableCollection<VehicleSubModel>();
            vehicle_model_year_list = vehicle_model_year_static_list = model_year;
            Device.InvokeOnMainThreadAsync(async () =>
            {
                //await GetModelYearList();
            });

            ClosePopupCommand = new Command(async (obj) =>
            {
                CloseCountryPopup?.Invoke("", new EventArgs());
            });

            ModelYearSelectionCommand= new Command(async (obj) =>
            {
                try
                {
                    selected_model_year = (VehicleSubModel)obj;
                    MessagingCenter.Send<VehicleModelYearViewModel, VehicleSubModel>(this, "selected_vehicle_model_year", selected_model_year);
                    CloseCountryPopup?.Invoke("", new EventArgs());
                }
                catch (Exception ex)
                {
                }
            });
        }


        private ObservableCollection<VehicleSubModel> _vehicle_model_year_static_list;
        public ObservableCollection<VehicleSubModel> vehicle_model_year_static_list
        {
            get => _vehicle_model_year_static_list;
            set
            {
                _vehicle_model_year_static_list = value;
                OnPropertyChanged("vehicle_model_year_static_list");
            }
        }

        private ObservableCollection<VehicleSubModel> _vehicle_model_year_list;
        public ObservableCollection<VehicleSubModel> vehicle_model_year_list
        {
            get => _vehicle_model_year_list;
            set
            {
                _vehicle_model_year_list = value;
                OnPropertyChanged("vehicle_model_year_list");
            }
        }

        private VehicleSubModel _selected_model_year;
        public VehicleSubModel selected_model_year
        {
            get => _selected_model_year;
            set
            {
                _selected_model_year = value;

                if (selected_model_year != null)
                {
                    if (!string.IsNullOrEmpty(selected_model_year.model_year))
                    {

                        MessagingCenter.Send<VehicleModelYearViewModel, VehicleSubModel>(this, "selected_vehicle_model_year", selected_model_year);
                        CloseCountryPopup?.Invoke("", new EventArgs());
                    }
                }

                OnPropertyChanged("selected_country");
            }
        }

        #region Methods

        //public async Task GetModelYearList()
        //{
        //    var response = await apiServices.ModelYears("miBud");

        //    var api_status_code = StaticMethods.http_status_code(response.status_code);

        //    if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
        //    {
        //        //country_list = response.results;
        //        vehicle_model_year_list = vehicle_model_year_static_list = new ObservableCollection<VehicleModelYearModel>(response.results.First().rs_user_type_country);
        //    }
        //    else
        //    {

        //    }


        //}

        #endregion

        public ICommand ClosePopupCommand { get; set; }
        public ICommand ModelYearSelectionCommand { get; set; }
    }
}

