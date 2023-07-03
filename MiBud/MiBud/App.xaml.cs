using MiBud.Models;
using MiBud.Views.Login;
using MiBud.Views.MasterDetail;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("MaterialIcons-Regular.ttf", Alias = "Material")]
namespace MiBud
{
    public partial class App : Application
    {
        public static JobcardResult selected_jobcard;
        public static bool bt_available = true;
        public static bool dongle_connected = false;
        public static string selected_vehicle_picture = string.Empty;
        public static string selectedIcon = string.Empty;
        public static string selectedColor = string.Empty;
        public static bool is_update = true;
        public static string refresh_token = string.Empty;
        public static string access_token = string.Empty;
        public static string user_mail = string.Empty;
        public static string user_first_name = string.Empty;
        public static string user_last_name = string.Empty;
        public static string user_id = string.Empty;
        public static string user_type = string.Empty;
        public static string dongle_type = string.Empty;
        public static string dongle = string.Empty;
        public static int? country_id;
        public static string selected_vehicle = string.Empty;
        public static string selected_vehicle_Service = string.Empty;
        public static string otpmobileno = string.Empty;

        public static ObservableCollection<BluetoothDevicesModel> bluetooth_devices = new ObservableCollection<BluetoothDevicesModel>();

        //public static PaymentResponseModel paymentResponseModel = new PaymentResponseModel();
        public static LoginResponse user = new LoginResponse();
        public static LoginModel login_model = new LoginModel();

        public static Pin currentServiceLocation;
        internal static WorkshopResult CurrentWorkshop;

        public App()
        {
            InitializeComponent();

            //MainPage = new Views.ClusterPage();

            var update = Preferences.Get("IsUpdate", null);

            if (string.IsNullOrEmpty(update))
            {
                is_update = true;
            }
            else
            {
                if (update.Contains("true"))
                {
                    is_update = true;
                }
                else
                {
                    is_update = false;
                }
            }

            DateTime now = DateTime.Now.Date;
            int offset = CalculateOffset(now.DayOfWeek, DayOfWeek.Monday);
            DateTime nextUpdate = now.AddDays(offset);
            nextUpdate = DateTime.Now.Date;
            if (nextUpdate <= DateTime.Now.Date)
            {
                MainPage = new CustomControls.CustomNavigationPage(new Views.Login.LoginPage());
                //MainPage = new CustomControls.CustomNavigationPage(new Views.AddAddress.AddAddressPage());
            }
            else
            {
                var token = Preferences.Get("token", null);
                if (string.IsNullOrEmpty(token))
                {
                    MainPage = new CustomControls.CustomNavigationPage(new Views.Login.LoginPage());
                    //MainPage = new CustomControls.CustomNavigationPage(new Views.AddAddress.AddAddressPage());
                }
                else
                {
                    LoginResponse loginResponse = new LoginResponse();
                    var user_data = Preferences.Get("LoginResponse", null);


                    loginResponse = JsonConvert.DeserializeObject<LoginResponse>(user_data);
                    user = loginResponse;
                    //loginResponse.status_code = httpResponse.StatusCode;
                    //Preferences.Set("user_name", model.email);
                    //Preferences.Set("password", model.password);
                    //Preferences.Set("token", loginResponse.token?.access);
                    Application.Current.Properties["refresh_token"] = App.refresh_token = loginResponse.token?.refresh;
                    Application.Current.Properties["access_token"] = App.access_token = loginResponse.token?.access;
                    Application.Current.Properties["user_mail"] = App.user_mail = loginResponse?.email;
                    App.user_first_name = loginResponse?.first_name;
                    App.user_last_name = loginResponse?.last_name;
                    Application.Current.SavePropertiesAsync();
                    App.user_id = loginResponse?.user;
                    App.user_type = loginResponse?.user_type;
                    App.country_id = loginResponse?.agent?.workshop?.country;
                    Application.Current.MainPage = new MasterDetailView(user) { Detail = new NavigationPage(new Views.MyVehicle.MyVehiclePage()) };
                }
            }

            var access_token = Application.Current.Properties.FirstOrDefault(x => x.Key == "access_token");
            var refresh_token = Application.Current.Properties.FirstOrDefault(x => x.Key == "refresh_token");
            if (string.IsNullOrEmpty(access_token.Key) || string.IsNullOrEmpty(refresh_token.Key))
            {
                Application.Current.Properties["access_token"] = "";
                Application.Current.Properties["refresh_token"] = "";
                Application.Current.Properties["mac_id"] = "";
            }
        }

        public int CalculateOffset(DayOfWeek current, DayOfWeek desired)
        {
            // f( c, d ) = [7 - (c - d)] mod 7
            // f( c, d ) = [7 - c + d] mod 7
            // c is current day of week and 0 <= c < 7
            // d is desired day of the week and 0 <= d < 7
            int c = (int)current;
            int d = (int)desired;
            int offset = (7 - c + d) % 7;
            return offset == 0 ? 7 : offset;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
