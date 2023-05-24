using System.Collections.ObjectModel;
using MiBud.Interfaces;
using MiBud.Models;
using MiBud.Views.MyVehicle;
using MiBud.Views.SelfDiagnose;
using MiBud.Views.ServiceHistory;
using MiBud.Views.Settings;
using MiBud.Views.Trips;
using MiBud.Views.VehicleService;
using Xamarin.Forms;

namespace MiBud.ViewModels
{
    public class MasterViewModel : BaseViewModel
    {
        public MasterViewModel(int roll_id, LoginResponse user)
        {
            user_name = $"{user.first_name} {user.last_name}";
            this.user = user;

            menu_list = new ObservableCollection<MasterModel>
            {
                new MasterModel
                {
                    title = "My Vehicles",
                    TargetType = typeof(MyVehiclePage),
                },
                new MasterModel
                {
                    title = "Trips",
                    TargetType = typeof(TripsPage),
                },
                new MasterModel
                {
                    title= "Vehicle Service",
                    TargetType = typeof(VehicleServicePage),
                },
                //new MasterModel
                //{
                //    title= "Service History",
                //    TargetType = typeof(ServiceHistoryPage),
                //},
                new MasterModel
                {
                    title= "Self Diagnose",
                    TargetType = typeof(SelfDiagnosePage),
                },
                new MasterModel
                {
                    title= "Settings",
                    TargetType = typeof(SettingPage),
                },
                new MasterModel
                    {
                        title = "Logout",
                        TargetType = typeof(Views.Login.LoginPage),
                    },
            };
        }

        private ObservableCollection<MasterModel> _menu_list;
        public ObservableCollection<MasterModel> menu_list
        {
            get => _menu_list;
            set
            {
                _menu_list = value;
                OnPropertyChanged("menu_list");
            }
        }

        private string _user_name;
        public string user_name
        {
            get => _user_name;
            set
            {
                _user_name = value;
                OnPropertyChanged("user_name");
            }
        }

        private LoginResponse _user;
        public LoginResponse user
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged("user");
            }
        }

        private string _version = $"App Version  {DependencyService.Get<IVersionAndBuildNumber>().GetVersionNumber()}";
        public string version
        {
            get => _version;
            set
            {
                _version = value;
                OnPropertyChanged("version");
            }
        }
    }
}