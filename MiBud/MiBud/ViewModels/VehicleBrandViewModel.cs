using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using MiBud.Models;
using MiBud.Services;
using MiBud.StaticInfo;
using MiBud.Views.NewVehicle;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MiBud.ViewModels
{
    public class VehicleBrandViewModel : BaseViewModel
    {
        ApiServices apiServices;
        public event EventHandler CloseCountryPopup;
        //int segment_id = -1;
        public VehicleBrandViewModel(int segment_id)
        {


            apiServices = new ApiServices();
            Device.InvokeOnMainThreadAsync(async () =>
            {
                IsBusy = true;
                await Task.Delay(10);

                await GetVehicleBrandList(segment_id);
                IsBusy = false;
            });



            Device.InvokeOnMainThreadAsync(async () =>
            {
                //await GetVehicleBrandList();
            });

            ClosePopupCommand = new Command(async (obj) =>
            {
                CloseCountryPopup?.Invoke("", new EventArgs());
            });

            BrandSelectionCommand = new Command(async (obj) =>
            {
                try
                {
                    selected_oem = (OemResult)obj;
                    MessagingCenter.Send<VehicleBrandViewModel, OemResult>(this, "selected_oem", selected_oem);
                    CloseCountryPopup?.Invoke("", new EventArgs());
                }
                catch (Exception ex)
                {
                }
            });

        }

        private string _search_key;
        public string search_key
        {
            get => _search_key;
            set
            {
                _search_key = value;

                if (!string.IsNullOrEmpty(search_key))
                {
                    oem_list = new ObservableCollection<OemResult>(oem_static_list.Where(x => x.oem.name.ToLower().Contains(search_key.ToLower())).ToList());
                }

                OnPropertyChanged("search_key");
            }
        }

        private ObservableCollection<OemResult> _oem_static_list;
        public ObservableCollection<OemResult> oem_static_list
        {
            get => _oem_static_list;
            set
            {
                _oem_static_list = value;
                OnPropertyChanged("oem_static_list");
            }
        }

        private ObservableCollection<OemResult> _oem_list;
        public ObservableCollection<OemResult> oem_list
        {
            get => _oem_list;
            set
            {
                _oem_list = value;
                OnPropertyChanged("oem_list");
            }
        }

        private OemResult _selected_oem;
        public OemResult selected_oem
        {
            get => _selected_oem;
            set
            {
                _selected_oem = value;
                OnPropertyChanged("selected_oem");
            }
        }

        #region Methods

        public async Task GetVehicleBrandList(int segment_id)
        {
            var response = await apiServices.GetVehicleBrand(Preferences.Get("token", null), segment_id, App.is_update);

            var api_status_code = StaticMethods.http_status_code(response.status_code);

            if (response == null)
            {
                await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Service is not working", "Failed", "Ok");
            }

            if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
            {
                oem_list = oem_static_list = new ObservableCollection<OemResult>(response.results);
            }
            else
            {
                await Acr.UserDialogs.UserDialogs.Instance.AlertAsync(response?.error, api_status_code, "Ok");
            }
        }

        #endregion

        public ICommand ClosePopupCommand { get; set; }
        public ICommand BrandSelectionCommand { get; set; }
    }
}
