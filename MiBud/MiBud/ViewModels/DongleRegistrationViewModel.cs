using Acr.UserDialogs;
using MiBud.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MiBud.ViewModels
{
    public class DongleRegistrationViewModel : ViewModelBase
    {
        ApiServices apiServices;
        readonly INavigation navigationService;
        readonly Page page;
        //public string title = string.Empty;
        //MediaFile file = null;
        public DongleRegistrationViewModel(Page page, string title) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
                this.page = page;
                this.navigationService = page.Navigation;
                page.Title = $"Dongle ({title}) Registration";
                header = $"Registration of your RSAngel {title} Dongle";
                InitializeCommands();
            }
            catch
            { }
        }

        #region Properties

        //private string _title;
        //public string title
        //{
        //    get => _title;
        //    set
        //    {
        //        _title = value;
        //        OnPropertyChanged("title");
        //    }
        //}

        private string _header;// = $"Registration of your RSAngel {title} Dongle";
        public string header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged("header");
            }
        }
        #endregion

        #region Methods
        [Obsolete]
        public void InitializeCommands()
        {
            YesCommand = new Command(async (obj) =>
            {
                
            });

            NoCommand = new Command(async (obj) =>
            {
                //await this.navigationService.PushAsync(new Views.DongleRegistration.DongleRegistrationPage());
            });

            DoneCommand = new Command(async (obj) =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(200);
                    //StaticMethods.last_page = "registration";
                    //await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.DongleListPopupPage());
                }
            });

        }
        #endregion
        #region ICommands
        public ICommand YesCommand { get; set; }
        public ICommand NoCommand { get; set; }
        public ICommand DoneCommand { get; set; }
        #endregion
    }
}
