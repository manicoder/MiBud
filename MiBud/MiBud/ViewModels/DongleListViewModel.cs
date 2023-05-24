using Acr.UserDialogs;
using MiBud.Interfaces;
using MiBud.Models;
using MiBud.Services;
using MiBud.StaticInfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MiBud.ViewModels
{
    class DongleListViewModel : BaseViewModel
    {
        ApiServices apiServices;
        public event EventHandler ClosebtPopup;
        string vehicle_id = string.Empty;
        public DongleListViewModel(string btnText, string vehicle_id)
        {
            apiServices = new ApiServices();
            this.vehicle_id = vehicle_id;
            bt_list = new ObservableCollection<BluetoothDevicesModel>();

            btn_name = btnText = "Connect";

            Device.InvokeOnMainThreadAsync(async () =>
            {
                await GetBtList();
            });

            ClosePopupCommand = new Command(async (obj) =>
            {
                ClosebtPopup?.Invoke("", new EventArgs());
            });

            ActionCommand = new Command(async (obj) =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    try
                    {



                        if (btnText.Contains("Register"))
                        {
                            var reg_dongle = await RegisterDonlge();
                            if (reg_dongle)
                            {
                                await ConnectDongle();
                            }
                        }
                        else
                        {
                            await ConnectDongle();
                        }


                    }
                    catch (Exception ex)
                    {

                    }
                }
                ClosebtPopup?.Invoke("", new EventArgs());
            });
        }


        //private string _search_key;
        //public string search_key
        //{
        //    get => _search_key;
        //    set
        //    {
        //        _search_key = value;

        //        if (!string.IsNullOrEmpty(search_key))
        //        {
        //            bt_list = new ObservableCollection<BluetoothDevicesModel>(bt_static_list.Where(x => x.name.ToLower().Contains(search_key.ToLower())).ToList());
        //        }

        //        OnPropertyChanged("search_key");
        //    }
        //}

        //private ObservableCollection<BluetoothDevicesModel> _bt_static_list;
        //public ObservableCollection<BluetoothDevicesModel> bt_static_list
        //{
        //    get => _bt_static_list;
        //    set
        //    {
        //        _bt_static_list = value;
        //        OnPropertyChanged("bt_static_list");
        //    }
        //}


        private string _btn_name = string.Empty;
        public string btn_name
        {
            get => _btn_name;
            set
            {
                _btn_name = value;
                OnPropertyChanged("btn_name");
            }
        }

        private ObservableCollection<BluetoothDevicesModel> _bt_list;
        public ObservableCollection<BluetoothDevicesModel> bt_list
        {
            get => _bt_list;
            set
            {
                _bt_list = value;
                OnPropertyChanged("bt_list");
            }
        }

        private BluetoothDevicesModel _selected_bt_device;
        public BluetoothDevicesModel selected_bt_device
        {
            get => _selected_bt_device;
            set
            {
                _selected_bt_device = value;
                OnPropertyChanged("selected_bt_device");
            }
        }

        #region Methods
        public async Task GetBtList()
        {
            //var response = await apiServices.Countries("miBud");
            App.bt_available = true;


            Device.BeginInvokeOnMainThread(() =>
            {
                Device.StartTimer(TimeSpan.FromMilliseconds(700), () =>
                {
                    bt_list = App.bluetooth_devices;
                    return App.bt_available;
                });
            });
            await DependencyService.Get<IBlueToothDevices>().SearchBlueTooth();
            //get_list();
            //bt_list= App.bluetooth_devices;
        }

        public async Task<bool> RegisterDonlge()
        {
            try
            {
                bool return_value = false;
                selected_bt_device = bt_list.FirstOrDefault();
                if (selected_bt_device != null)
                {
                    RegisterDongleModel RDM = new RegisterDongleModel
                    {
                        mac_id = selected_bt_device.mac_address,
                        device_type = selected_bt_device.name,
                        created_by = App.user.user,
                        owner = App.user.user,
                        vehicle = this.vehicle_id,
                    };
                    var result = await apiServices.RegisterDongle(RDM, App.access_token);

                    if (result == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Failed", "Please check device internet connection.", "Ok");
                        return_value = false;
                    }

                    var api_status_code = StaticMethods.http_status_code(result.status_code);

                    if (result.status_code == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await Application.Current.MainPage.DisplayAlert("Failed!", $"User Unauthorized", "OK");
                        Preferences.Set("token", "");
                        Preferences.Set("IsUpdate", "true");
                        App.Current.MainPage = new NavigationPage(new Views.Login.LoginPage());
                        return_value = false;
                    }

                    if (result.status_code == System.Net.HttpStatusCode.OK || result.status_code == System.Net.HttpStatusCode.Created)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success!", $"{result.error}", "OK");
                        return_value = true;
                        ClosebtPopup?.Invoke("", new EventArgs());

                        //App.Current.MainPage = new NavigationPage(new Views.Login.LoginPage());
                    }
                    else if (result.status_code == System.Net.HttpStatusCode.Forbidden)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success!", $"{result.error}\n{result.message}", "OK");
                        return_value = false;
                        ClosebtPopup?.Invoke("", new EventArgs());
                        //App.Current.MainPage = new NavigationPage(new Views.Login.LoginPage());
                    }
                    else
                    {
                        await UserDialogs.Instance.AlertAsync($"{result?.message}", "Alert", "Ok");
                        return_value = false;
                    }
                }
                return return_value;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task ConnectDongle()
        {
            try
            {
                //string message = string.Empty;
                string dongle_filert = string.Empty;
                //bool is_valid_dongle = false;
                //bool is_approved_dongle = false;
                await Task.Delay(200);
                selected_bt_device = bt_list.FirstOrDefault();
                var res = await DependencyService.Get<IBlueToothDevices>().ConnectionBT(selected_bt_device.mac_address, 100, true);
                if (res == "connected")
                {

                    var rx_header = StaticMethods.ecu_info.FirstOrDefault().rx_header;
                    var tx_header = StaticMethods.ecu_info.FirstOrDefault().tx_header;
                    var protocol = StaticMethods.ecu_info.FirstOrDefault().protocol;
                    var firmwareVersion = await DependencyService.Get<IBlueToothDevices>().GetFirmwareVersion(dongle_filert, tx_header, rx_header, protocol);
                    await UserDialogs.Instance.AlertAsync($"Dongle connected ({firmwareVersion})", "Success", "Ok");
                    App.dongle_connected = true;
                    
                    //var rx_header = StaticMethods.ecu_info.FirstOrDefault().rx_header;
                    //var tx_header = StaticMethods.ecu_info.FirstOrDefault().tx_header;
                    //var protocol = StaticMethods.ecu_info.FirstOrDefault().protocol;
                    //var firmwareVersion = await DependencyService.Get<IBlueToothDevices>().GetFirmwareVersion(dongle_filert, tx_header, rx_header, protocol);
                    //await page.Navigation.PushAsync(new Views.AppFeature.AppFeaturePage(firmwareVersion, model_image, selected_model, selected_oem));
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync("Dongle not connected", "Failed", "Ok");
                }
            }
            catch (Exception ex)
            {
            }
        }


        #endregion

        public ICommand ClosePopupCommand { get; set; }
        public ICommand ActionCommand { get; set; }
    }
}
