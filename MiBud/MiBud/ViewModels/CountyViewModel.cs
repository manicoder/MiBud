using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MiBud.Models;
using MiBud.Services;
using MiBud.StaticInfo;
using Xamarin.Forms;

namespace MiBud.ViewModels
{
    public class CountyViewModel : BaseViewModel
    {
        ApiServices apiServices;
        public event EventHandler CloseCountryPopup;

        public CountyViewModel()
        {
            apiServices = new ApiServices();
            country_list = new ObservableCollection<RsUserTypeCountry>();
            selected_country = new RsUserTypeCountry();
            Device.InvokeOnMainThreadAsync(async () =>
            {
                await GetCountryList();
            });

            ClosePopupCommand = new Command(async (obj) =>
            {
                CloseCountryPopup?.Invoke("", new EventArgs());
                //StaticMethods.last_page = "CreateRSAgent";
                //await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.CountyPopupPage());
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
                    country_list = new ObservableCollection<RsUserTypeCountry>(country_static_list.Where(x => x.name.ToLower().Contains(search_key.ToLower())).ToList());
                }

                OnPropertyChanged("search_key");
            }
        }

        private ObservableCollection<RsUserTypeCountry> _country_static_list;
        public ObservableCollection<RsUserTypeCountry> country_static_list
        {
            get => _country_static_list;
            set
            {
                _country_static_list = value;
                OnPropertyChanged("country_static_list");
            }
        }

        private ObservableCollection<RsUserTypeCountry> _country_list;
        public ObservableCollection<RsUserTypeCountry> country_list
        {
            get => _country_list;
            set
            {
                _country_list = value;
                OnPropertyChanged("country_list");
            }
        }

        private RsUserTypeCountry _selected_country;
        public RsUserTypeCountry selected_country
        {
            get => _selected_country;
            set
            {
                _selected_country = value;

                if (selected_country != null)
                {
                    if (!string.IsNullOrEmpty(selected_country.name))
                    {


                        switch (StaticMethods.last_page)
                        {
                            case "registration":
                                MessagingCenter.Send<CountyViewModel, RsUserTypeCountry>(this, "selected_country_registrationVM", selected_country);
                                break;

                            //case "CreateRSAgent":
                            //    MessagingCenter.Send<CountyViewModel, RsUserTypeCountry>(this, "selected_country_CreateRSAgentVM", selected_country);
                            //    break;
                        }

                        CloseCountryPopup?.Invoke("", new EventArgs());
                    }
                }

                OnPropertyChanged("selected_country");
            }
        }

        #region Methods

        public async Task GetCountryList()
        {
            var response = await apiServices.Countries("miBud");

            var api_status_code = StaticMethods.http_status_code(response.status_code);

            if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
            {
                //country_list = response.results;
                country_list = country_static_list = new ObservableCollection<RsUserTypeCountry>(response.results.First().rs_user_type_country);
            }
            else
            {

            }


        }

        #endregion

        public ICommand ClosePopupCommand { get; set; }
    }
}
