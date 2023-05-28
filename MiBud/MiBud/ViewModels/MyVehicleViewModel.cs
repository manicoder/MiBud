using Acr.UserDialogs;
using MiBud.Models;
using MiBud.Services;
using MiBud.StaticInfo;
using MiBud.Views.NewVehicle;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MiBud.ViewModels
{
    public class MyVehicleViewModel : ViewModelBase
    {
        ApiServices apiServices;
        readonly INavigation navigationService;
        readonly Page page;

        public MyVehicleViewModel(Page page) : base(page)
        {

            apiServices = new ApiServices();
            this.page = page;
            this.navigationService = page.Navigation;

            if (App.user.vehicle != null)
            {
                if (App.user.vehicle.Count > 0)
                {
                    my_vehicle_list = new ObservableCollection<Vehicle>(App.user.vehicle);
                }
                else
                {

                }
            }
            else
            {

            }
        }

        private ObservableCollection<Vehicle> _my_vehicle_list;
        public ObservableCollection<Vehicle> my_vehicle_list
        {
            get => _my_vehicle_list;
            set
            {
                _my_vehicle_list = value;
                OnPropertyChanged("my_vehicle_list");
            }
        }

        private Vehicle _selected_vehicle;
        public Vehicle selected_vehicle
        {
            get => _selected_vehicle;
            set
            {
                _selected_vehicle = value;
                App.selected_vehicle = value.id;
                OnPropertyChanged("selected_vehicle");
            }
        }

        private string _selected_vehicle_picture;
        public string selected_vehicle_picture
        {
            get => _selected_vehicle_picture;
            set
            {
                _selected_vehicle_picture = value;
                OnPropertyChanged("selected_vehicle_picture");
            }
        }

        #region Methods
        public void InitializeCommands()
        {
            //AddNewVehicleCommand = new Command(async (obj) =>
            //{
            //    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            //    {
            //        await Task.Delay(200);
            //        await this.page.Navigation.PushAsync(new AddNewVehiclePage());
            //    }
            //});

            //VehicleSelectionCommand = new Command(async (obj) =>
            //{
            //    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            //    {
            //        try
            //        {
            //            string btnText = string.Empty;
            //            await Task.Delay(200);
            //            selected_vehicle = (Vehicle)obj;
            //            selected_vehicle_picture = selected_vehicle.vehicle_picture;
            //            foreach(var color in my_vehicle_list.ToList())
            //            {
            //                if(selected_vehicle.id == color.id)
            //                {
            //                    color.bg_color = "#f5d6d7";
            //                }
            //                else
            //                {
            //                    color.bg_color = "#ffffff";
            //                }
            //            }
            //            var models = await apiServices.GetVehicleModel(Preferences.Get("token", null), selected_vehicle.oem, App.is_update);
            //            var value = models.results.FirstOrDefault().sub_models.FirstOrDefault().ecus;
            //            foreach (var item in value)
            //            {
            //                StaticMethods.ecu_info.Add(

            //                    new EcuDataSet
            //                    {
            //                        ecu_name = item.name,
            //                        clear_dtc_index = item.clear_dtc_fn_index,
            //                        dtc_dataset_id = item.datasets.FirstOrDefault(),
            //                        pid_dataset_id = item.pid_datasets.FirstOrDefault(),
            //                        read_dtc_index = item.read_dtc_fn_index,
            //                        read_data_fn_index = item.read_data_fn_index,
            //                        tx_header = item.tx_header,
            //                        rx_header = item.rx_header,
            //                        protocol = item.protocol,
            //                    });
            //            }
            //            if(selected_vehicle.dongle!=null)
            //            {
            //                if(selected_vehicle.dongle.Count>0)
            //                {
            //                    btnText = "Connect";
            //                }
            //                else
            //                {
            //                    btnText = "Register";
            //                }
            //            }
            //            else
            //            {
            //                btnText = "Register";
            //            }    
            //            await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.DongleListPopupPage(btnText, selected_vehicle.id));
            //        }
            //        catch (Exception ex)
            //        {
            //        }
            //    }
            //});

            //ActionCommand = new Command(async (obj) =>
            //{
            //    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            //    {
            //        await Task.Delay(200);
            //        await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.DongleListPopupPage());
            //    }
            //});
        }
        #endregion

        #region ICommands
        public ICommand AddNewVehicleCommand => new Command(async (obj) =>
        {
            //selected_vehicle = (BluetoothDevicesModel)obj;
            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            {
                await Task.Delay(200);
                await this.page.Navigation.PushAsync(new AddNewVehiclePage(null));
            }
        });
        public ICommand EditVehicleBrandCommand => new Command(async (obj) =>
        {
            selected_vehicle = (Vehicle)obj;
            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            {
                await Task.Delay(200);
                await this.page.Navigation.PushAsync(new AddNewVehiclePage(selected_vehicle));
            }
        });
        public ICommand VehicleSelectionCommand => new Command(async (obj) =>
        {
            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            {
                try
                {
                    string btnText = string.Empty;
                    await Task.Delay(200);
                    selected_vehicle = (Vehicle)obj;

                    StaticMethods.model_id = selected_vehicle.vehicle_model.id;
                    StaticMethods.submodel_id = selected_vehicle.sub_model.id;
                    App.selected_vehicle_picture = selected_vehicle_picture = selected_vehicle.vehicle_picture;
                    ////foreach (var color in my_vehicle_list.ToList())
                    ////{
                    ////    if (selected_vehicle.id == color.id)
                    ////    {
                    ////        color.bg_color = "#f5d6d7";
                    ////    }
                    ////    else
                    ////    {
                    ////        color.bg_color = "#ffffff";
                    ////    }
                    ////}
                    ////var models = await apiServices.GetVehicleModel(Preferences.Get("token", null), selected_vehicle.oem.name, App.is_update);
                    ////var value = models.results.FirstOrDefault().sub_models.FirstOrDefault().ecus;
                    ////foreach (var item in value)
                    ////{
                    ////    StaticMethods.ecu_info.Add(

                    ////        new EcuDataSet
                    ////        {
                    ////            ecu_name = item.name,
                    ////            clear_dtc_index = item.clear_dtc_fn_index,
                    ////            dtc_dataset_id = item.datasets.FirstOrDefault(),
                    ////            pid_dataset_id = item.pid_datasets.FirstOrDefault(),
                    ////            read_dtc_index = item.read_dtc_fn_index,
                    ////            read_data_fn_index = item.read_data_fn_index,
                    ////            tx_header = item.tx_header,
                    ////            rx_header = item.rx_header,
                    ////            protocol = item.protocol,
                    ////        });
                    ////}
                    ////if (selected_vehicle.dongle != null)
                    ////{
                    ////    if (selected_vehicle.dongle.Count > 0)
                    ////    {
                    ////        btnText = "Connect";
                    ////    }
                    ////    else
                    ////    {
                    ////        btnText = "Register";
                    ////    }
                    ////}
                    ////else
                    ////{
                    ////    btnText = "Register";
                    ////}
                    await this.page.Navigation.PushAsync(new Views.Home.HomePage(selected_vehicle));
                    //await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.DongleListPopupPage(btnText, selected_vehicle.id));
                }
                catch (Exception ex)
                {
                }
            }
        });
        #endregion
    }
}

