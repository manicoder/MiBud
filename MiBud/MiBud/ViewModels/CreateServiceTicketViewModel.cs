using Acr.UserDialogs;
using MiBud.Models;
using MiBud.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MiBud.ViewModels
{
    public class CreateServiceTicketViewModel : BaseViewModel
    {
        ApiServices apiServices;
        
        public CreateServiceTicketViewModel()
        {
            apiServices = new ApiServices();
            GetWorkshop();
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

        private ObservableCollection<WorkshopResult> _workshops;
        public ObservableCollection<WorkshopResult> workshops
        {
            get { return _workshops; }
            set
            {
                _workshops = value;
                OnPropertyChanged("workshops");
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

        public async void GetWorkshop()
        {
            var result = await apiServices.GetWorkshop(Xamarin.Essentials.Preferences.Get("token", null));

            if (!result.success)
            {
                //DependencyService.Get<Interfaces.IToasts>().Show($"{ mode.status}");
                UserDialogs.Instance.Toast(result.status, new TimeSpan(0, 0, 0, 3));
                return;
            }

            if (result.results == null || (!result.results.Any()))
            {
                UserDialogs.Instance.Toast("User not found", new TimeSpan(0, 0, 0, 3));
                return;
            }



            workshops = new ObservableCollection<WorkshopResult>(result.results.ToList());
        }
    }
}
