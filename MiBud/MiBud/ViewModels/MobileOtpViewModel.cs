using Acr.UserDialogs;
using MiBud.Interfaces;
using MiBud.Models;
using MiBud.Services;
using MiBud.StaticInfo;
using MiBud.Views.MasterDetail;
using MiBud.Views.MobileOtp;
using MiBud.Views.MyVehicle;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MiBud.ViewModels
{
    public class MobileOtpViewModel : ViewModelBase
    {
        ApiServices apiServices;
        readonly INavigation navigationService;
        readonly Page page;
        readonly IDeviceMacAddress device_mac_id;
        public MobileOtpViewModel(Page page) : base(page)
        {


            try
            {
                mobile = "Code is sent to " + App.otpmobileno;
                apiServices = new ApiServices();
                user_detail = new LoginModel();
                App.otpmobileno = string.Empty;
                this.page = page;
                this.navigationService = page.Navigation;
                this.device_mac_id = DependencyService.Get<IDeviceMacAddress>();
                InitializeCommands();
                GetDeviceType();
                Device.InvokeOnMainThreadAsync(async () =>
                {
                    await GetMacId();
                });

                var user = Preferences.Get("user_name", null);
                var pass = Preferences.Get("password", null);

            }
            catch (Exception ex)
            {
            }
        }

        #region Properties
        private string _device_type;
        public string device_type
        {
            get => _device_type;
            set
            {
                _device_type = value;
                OnPropertyChanged("device_type");
            }
        }

        private string _mac_id;
        public string mac_id
        {
            get => _mac_id;
            set
            {
                _mac_id = value;
                OnPropertyChanged("mac_id");
            }
        }

        private LoginModel _user_detail;
        public LoginModel user_detail
        {
            get => _user_detail;
            set
            {
                _user_detail = value;
                OnPropertyChanged("user_detail");
            }
        }

        private string _mobile;
        public string mobile
        {
            get => _mobile;
            set
            {
                _mobile = value;
                OnPropertyChanged("mobile");
            }
        }

        private string _otp1;
        public string otp1
        {
            get => _otp1;
            set
            {
                _otp1 = value;
                OnPropertyChanged("otp1");
            }
        }

        private string _otp2;
        public string otp2
        {
            get => _otp2;
            set
            {
                _otp2 = value;
                OnPropertyChanged("otp2");
            }
        }

        private string _otp3;
        public string otp3
        {
            get => _otp3;
            set
            {
                _otp3 = value;
                OnPropertyChanged("otp3");
            }
        }

        private string _otp4;
        public string otp4
        {
            get => _otp4;
            set
            {
                _otp4 = value;
                OnPropertyChanged("otp4");
            }
        }
        #endregion

        #region ICommands
        public ICommand OtpVerifyCommand { get; set; }
        #endregion

        #region ICommands
        public void InitializeCommands()
        {
            OtpVerifyCommand = new Command(async (obj) =>
            {
                GoToMasterPage();
            });
        }
        #endregion


        #region Methods
        public void GetDeviceType()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    break;
                case Device.Android:
                    device_type = "android";
                    break;
                case Device.UWP:
                    device_type = "windows";
                    break;
                default:
                    break;
            }
        }

        public async Task GetMacId()
        {
            mac_id = "6E285B000000";//await device_mac_id.GetMacAddress();
        }

        public async Task<bool> Validation()
        {
            bool IsError = false;
            //if (string.IsNullOrEmpty(email))
            //{
            //    await page.DisplayAlert("Alert", "Please enter your user name", "Ok");
            //    IsError = true;
            //}
            //else if (string.IsNullOrEmpty(password))
            //{
            //    await page.DisplayAlert("Alert", "Please enter your password", "Ok");
            //    IsError = true;
            //}
            if (string.IsNullOrEmpty(device_type))
            {
                await page.DisplayAlert("Alert", "Device type not found", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(mac_id))
            {
                await page.DisplayAlert("Alert", "Mac address not found", "Ok");
                IsError = true;
            }

            return IsError;
        }


        async void GoToMasterPage()
        {
            LoginResponse user = null;

            try
            {
                string otp = this.otp1 + this.otp2 + this.otp3 + this.otp4;
                if (otp != "0000")
                {
                    await page.DisplayAlert("Alert", "Invalid Otp", "Ok");
                    return;
                }

                var IsError = await Validation();
                if (!IsError)
                {
                    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                    {
                        await Task.Delay(200);

                        if (string.IsNullOrEmpty(mac_id))
                        {
                            await page.DisplayAlert("Error", "Your device id not found.", "Ok");
                            return;
                        }

                        user_detail.email = "user.mibud@wikitek.in";
                        user_detail.password = "autopeepal123";
                        user_detail.device_type = device_type;
                        user_detail.mac_id = mac_id;

                        App.login_model = user_detail;
                        App.user = user = await apiServices.UserLogin(user_detail);

                        if (user.error == "No internet connection")
                        {
                            await page.DisplayAlert("Network Issue", "No internet connection.", "Ok");
                            return;
                        }

                        var api_status_code = StaticMethods.http_status_code(user.status_code);

                        if (user?.status_code == System.Net.HttpStatusCode.OK || user?.status_code == System.Net.HttpStatusCode.Created)
                        {
                            //if (string.IsNullOrEmpty(user.user_type))
                            //{
                            //    await page.DisplayAlert("Failed", "Please check internet connection.", "Ok");
                            //    return;
                            //}
                            //if (user.user_type == "miBud")
                            //{
                            Application.Current.MainPage = new MasterDetailView(user) { Detail = new NavigationPage(new MyVehiclePage()) };
                            //await navigationService.PushAsync(new MasterDetailView(user) { Detail = new NavigationPage(new VehicleDiagnosticsPage()) });
                            //}
                        }
                        else if (api_status_code == "Internal Server Error")
                        {
                            await page.DisplayAlert(api_status_code, "Service not working.", "Ok");
                            return;
                        }
                        else
                        {
                            await page.DisplayAlert(api_status_code, user.error, "Ok");
                            return;
                        }
                    }
                }
            }
            catch (System.Exception es)
            {
                await page.DisplayAlert("Failed", "Please check device internet connection.", "Ok");
            }
        }
        #endregion
    }
}
