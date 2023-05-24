using Acr.UserDialogs;
using MiBud.Interfaces;
using MiBud.Models;
using MiBud.Services;
using MiBud.StaticInfo;
using MiBud.Views.MasterDetail;
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
    public class LoginViewModel : ViewModelBase
    {

        ApiServices apiServices;
        readonly INavigation navigationService;
        readonly Page page;
        readonly IDeviceMacAddress device_mac_id;

        public LoginViewModel(Page page) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
                user_detail = new LoginModel();

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

                if (!string.IsNullOrEmpty(user))
                {
                    email = user;
                }

                if (!string.IsNullOrEmpty(user))
                {
                    password = pass;
                }
            }
            catch (Exception ex)
            {
            }
        }


        #region Properties

        private string _rs_agent_name;
        public string rs_agent_name
        {
            get => _rs_agent_name;
            set
            {
                _rs_agent_name = value;
                OnPropertyChanged("rs_agent_name");
            }
        }


        private string _email;
        public string email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged("email");
            }
        }


        private string _password;
        public string password
        {
            get => _password;
            set
            {
                _password = value;

                if (string.IsNullOrEmpty(password))
                {
                    password_image_visible = false;
                }
                else
                {
                    password_image_visible = true;
                }

                OnPropertyChanged("password");
            }
        }


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

        private bool _is_password = true;
        public bool is_password
        {
            get => _is_password;
            set
            {
                _is_password = value;
                OnPropertyChanged("is_password");
            }
        }

        private string _password_image = "󰛐";//"\uF06D1";//"&#xF06D1;";
        public string password_image
        {
            get => _password_image;
            set
            {
                _password_image = value;
                OnPropertyChanged("password_image");
            }
        }

        private Color _password_image_color = Color.FromHex("#01FE2F");//"\uF06D1";//"&#xF06D1;";
        public Color password_image_color
        {
            get => _password_image_color;
            set
            {
                _password_image_color = value;
                OnPropertyChanged("password_image_color");
            }
        }

        private bool _password_image_visible = false;
        public bool password_image_visible
        {
            get => _password_image_visible;
            set
            {
                _password_image_visible = value;
                OnPropertyChanged("password_image_visible");
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

        #endregion


        #region Methods

        public void InitializeCommands()
        {
            IsPasswordCommand = new Command(async (obj) =>
            {
                if (is_password)
                {
                    password_image = "󰛑";//F06D0;
                    password_image_color = Color.Red;
                    is_password = false;
                }
                else
                {
                    is_password = true;
                    password_image_color = Color.FromHex("#01FE2F");
                    password_image = "󰛐";//F06D1
                }
            });

            LoginCommand = new Command(async (obj) =>
            {
                GoToMasterPage();
            });

            SingUpCommand = new Command(async (obj) =>
            {
                GoToRegistrationPage();
            });

            ForgotPasswordCommand = new Command(async (obj) =>
            {
                GoToForgotPasswordPage();
            });
        }

        //public async Task UserLogin()
        //{

        //    var response = await apiServices.UserLogin(user_detail);

        //    var api_status_code = StaticMethods.http_status_code(response.status_code);

        //    if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
        //    {
        //        //country_list = response.results;
        //    }
        //    else
        //    {

        //    }
        //}

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
            if (string.IsNullOrEmpty(email))
            {
                await page.DisplayAlert("Alert", "Please enter your user name", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(password))
            {
                await page.DisplayAlert("Alert", "Please enter your password", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(device_type))
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

                        user_detail.email = email;
                        user_detail.password = password;
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

        async void GoToRegistrationPage()
        {
            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            {
                await Task.Delay(200);
                await navigationService.PushAsync(new Views.Registration.RegistrationPage());
            }
        }

        async void GoToForgotPasswordPage()
        {
            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            {
                await Task.Delay(200);
                string description = "Enter your Registered mobile / Email ID number";
                //string description = "Forgot Password An OTP has been sent to your mobile and will be valid for 10 mins.Pls enter the OTP here";
                await navigationService.PushAsync(new Views.Otp.OtpPage(false, false,description));
            }
        }

        #endregion


        #region ICommands
        public ICommand LoginCommand { get; set; }
        public ICommand SingUpCommand { get; set; }
        public ICommand ForgotPasswordCommand { get; set; }
        public ICommand IsPasswordCommand { get; set; }
        #endregion
    }
}
