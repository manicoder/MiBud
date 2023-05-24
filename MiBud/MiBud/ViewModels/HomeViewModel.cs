using Acr.UserDialogs;
using MiBud.Models;
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
