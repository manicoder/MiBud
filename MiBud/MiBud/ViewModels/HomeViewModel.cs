using Acr.UserDialogs;
using MiBud.Models;
using MiBud.Views.CreateWikitekTicket;
using MiBud.Views.VehicleService;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MiBud.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel(Page page, Vehicle vehicle) : base(page)
        {
            try
            {
                selected_vehicle = vehicle;
            }
            catch (Exception ex)
            {
            }
        }

        #region Properties
        private Color _mobitek_color = (Color)Application.Current.Resources["tab_unselect_color"];
        public Color mobitek_color
        {
            get { return _mobitek_color; }
            set
            {
                _mobitek_color = value;
                OnPropertyChanged("mobitek_color");
            }
        }

        private Vehicle _selected_vehicle;
        public Vehicle selected_vehicle
        {
            get => _selected_vehicle;
            set
            {
                _selected_vehicle = value;
                OnPropertyChanged("selected_vehicle");
            }
        }
        private WorkshopResult _selected_workshops;
        public WorkshopResult selected_workshops
        {
            get { return _selected_workshops; }
            set
            {
                _selected_workshops = value;
                OnPropertyChanged("selected_workshops");
            }
        }
        private Color _wikitek_color = (Color)Application.Current.Resources["theme_color"];
        public Color wikitek_color
        {
            get { return _wikitek_color; }
            set
            {
                _wikitek_color = value;
                OnPropertyChanged("wikitek_color");
            }
        }
        private Color _rsangel_color = (Color)Application.Current.Resources["tab_unselect_color"];
        public Color rsangel_color
        {
            get { return _rsangel_color; }
            set
            {
                _rsangel_color = value;
                OnPropertyChanged("rsangel_color");
            }
        }
        #endregion

        #region Commands
        public ICommand VehicleServiceCommand => new Command(async (obj) =>
        {
            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            {
                try
                {
                    var sender = (ImageButton)obj;
                    await sender.FadeTo(1, 250);
                    await sender.FadeTo(0, 250);
                    await sender.FadeTo(1, 250);
                    await Task.Delay(100);



                    //App.Current.MainPage = new NavigationPage(new Views.VehicleService.VehicleServicePage(selected_vehicle))
                    //{
                    //    BarBackgroundColor=Color.Blue
                    //};


                  
                    await this.page.Navigation.PushAsync(new Views.VehicleService.VehicleServicePage(selected_vehicle));
                }
                catch (Exception ex)
                {
                }
            }
        });
        #endregion
    }
}
