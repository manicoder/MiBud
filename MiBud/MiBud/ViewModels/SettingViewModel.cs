using MiBud.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MiBud.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        ApiServices apiServices;
        readonly INavigation navigationService;
        readonly Page page;
        //MediaFile file = null;
        public SettingViewModel(Page page) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
                this.page = page;
                this.navigationService = page.Navigation;
                InitializeCommands();
            }
            catch
            { }
        }

        #region Properties

        private string _ld_dongle = "Hello";
        public string ld_dongle
        {
            get => _ld_dongle;
            set
            {
                _ld_dongle = value;

                ld_button = string.IsNullOrEmpty(ld_dongle) ? "\U000f0417" : "\U000f08d5"; //IconFont.PlusCircle : IconFont.CircleEditOutline;
                OnPropertyChanged("ld_dongle");
            }
        }

        private string _ld_button;
        public string ld_button
        {
            get => _ld_button;
            set
            {
                _ld_button = value;
                OnPropertyChanged("ld_button");
            }
        }

        private string _hd_dongle = "Hello";
        public string hd_dongle
        {
            get => _hd_dongle;
            set
            {
                _hd_dongle = value;

                hd_button = string.IsNullOrEmpty(hd_dongle) ? "\U000f0417" : "\U000f08d5"; //IconFont.PlusCircle : IconFont.CircleEditOutline;
                OnPropertyChanged("hd_dongle");
            }
        }

        private string _hd_button;
        public string hd_button
        {
            get => _hd_button;
            set
            {
                _hd_button = value;
                OnPropertyChanged("hd_button");
            }
        }

        #endregion

        #region Methods
        [Obsolete]
        public void InitializeCommands()
        {
            DogleRegisterCommand = new Command(async (obj) =>
            {
                var ss = obj;
                var type = ss.ToString().Contains("LD") ? "12V" : "14V";
                await this.navigationService.PushAsync(new Views.DongleRegistration.DongleRegistrationPage(type));
            });

        }
        #endregion
        #region ICommands
        public ICommand DogleRegisterCommand { get; set; }
        //public ICommand CountryCommand { get; set; }
        #endregion
    }
}
