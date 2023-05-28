using Acr.UserDialogs;
using MiBud.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MiBud.ViewModels
{
    public class CreateRSAngelTicketViewModel : BaseViewModel
    {
        public CreateRSAngelTicketViewModel()
        {
            symotomps_list = new ObservableCollection<CreateWikitekTicketModel>
            {
                new CreateWikitekTicketModel
                {
                     id = 1,
                     name = "Symptom",
                     text_color = (Color)Application.Current.Resources["text_color"],
                     add_btn_visible = true,
                     delete_btn_visible= false,
                     line_visible = false,
                     drop_down_visible= false,
                     right_btn_visible = false,
                     wrong_btn_visible = false,
                }
            };



            //spare_parts_list = new ObservableCollection<NewJobCardModel>
            //{
            //    new NewJobCardModel
            //    {
            //         id = 1,
            //         name= "Spare Part (INR 100)",
            //         text_color = (Color)Application.Current.Resources["text_color"],
            //         add_btn_visible = true,
            //         delete_btn_visible= false,
            //         line_visible = false,
            //         drop_down_visible= false,
            //         right_btn_visible = false,
            //         wrong_btn_visible = false
            //    }
            //};

            services_list = new ObservableCollection<CreateWikitekTicketModel>
            {
                new CreateWikitekTicketModel
                {
                     id = 1,
                     name= "Services (INR 250)",
                     text_color = (Color)Application.Current.Resources["text_color"],
                     add_btn_visible = true,
                     delete_btn_visible= false,
                     line_visible = false,
                     drop_down_visible= false,
                     right_btn_visible = false,
                     wrong_btn_visible = false
                }
            };


            if (Device.Idiom == TargetIdiom.Phone)
            {
                services_list_height = symotomps_list_height = 56;
            }
            else
            {
                services_list_height = symotomps_list_height = 66;
            }

            //vehicle_list = new ObservableCollection<VehicleModel>
            //{
            //    new VehicleModel
            //    {
            //        vehicle = "vehicle_1"
            //    },
            //    new VehicleModel
            //    {
            //        vehicle = "vehicle_2"
            //    },
            //    new VehicleModel
            //    {
            //        vehicle = "vehicle_3"
            //    },
            //    new VehicleModel
            //    {
            //        vehicle = "vehicle_4"
            //    },
            //    new VehicleModel
            //    {
            //        vehicle = "vehicle_5"
            //    },
            //    new VehicleModel
            //    {
            //        vehicle = "vehicle_6"
            //    },
            //};
            if (App.selectedColor == "blue")
            {
                selectedBgColor = Color.Blue;
            }
            else if (App.selectedColor == "orange")
            {
                selectedBgColor = Color.Orange;
            }
            else if (App.selectedColor == "green")
            {
                selectedBgColor = Color.Green;
            }
        }
        private Color _selectedBgColor;
        public Color selectedBgColor
        {
            get => _selectedBgColor;
            set
            {
                _selectedBgColor = value;
                OnPropertyChanged("selectedBgColor");
            }
        }
        private ObservableCollection<CreateWikitekTicketModel> _symotomps_list;
        public ObservableCollection<CreateWikitekTicketModel> symotomps_list
        {
            get => _symotomps_list;
            set
            {
                _symotomps_list = value;
                OnPropertyChanged("symotomps_list");
            }
        }

        //private ObservableCollection<CreateWikitekTicketModel> _spare_parts_list;
        //public ObservableCollection<CreateWikitekTicketModel> spare_parts_list
        //{
        //    get => _spare_parts_list;
        //    set
        //    {
        //        _spare_parts_list = value;
        //        OnPropertyChanged("spare_parts_list");
        //    }
        //}

        private ObservableCollection<CreateWikitekTicketModel> _services_list;
        public ObservableCollection<CreateWikitekTicketModel> services_list
        {
            get => _services_list;
            set
            {
                _services_list = value;
                OnPropertyChanged("services_list");
            }
        }

        //private ObservableCollection<VehicleModel> _vehicle_list;
        //public ObservableCollection<VehicleModel> vehicle_list
        //{
        //    get => _vehicle_list;
        //    set
        //    {
        //        _vehicle_list = value;
        //        OnPropertyChanged("vehicle_list");
        //    }
        //}

        private double _symotomps_list_height;
        public double symotomps_list_height
        {
            get => _symotomps_list_height;
            set
            {
                _symotomps_list_height = value;
                OnPropertyChanged("symotomps_list_height");
            }
        }

        //private double _spare_parts_list_height;
        //public double spare_parts_list_height
        //{
        //    get => _spare_parts_list_height;
        //    set
        //    {
        //        _spare_parts_list_height = value;
        //        OnPropertyChanged("spare_parts_list_height");
        //    }
        //}

        private double _services_list_height;
        public double services_list_height
        {
            get => _services_list_height;
            set
            {
                _services_list_height = value;
                OnPropertyChanged("services_list_height");
            }
        }

        private ObservableCollection<string> _service_list;
        public ObservableCollection<string> service_list
        {
            get { return _service_list; }
            set
            {
                _service_list = value;
                OnPropertyChanged("service_list");
            }
        }

        private string _selected_service;
        public string selected_service
        {
            get { return _selected_service; }
            set
            {
                _selected_service = value;
                OnPropertyChanged("selected_service");
                show_service_view = false;
            }
        }

        private bool _show_service_view = false;
        public bool show_service_view
        {
            get { return _show_service_view; }
            set
            {
                _show_service_view = value;
                OnPropertyChanged("show_service_view");
            }
        }

        public ICommand ShowServiceViewCommand => new Command(async (obj) =>
        {

            Device.BeginInvokeOnMainThread(async () =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(100);
                    try
                    {
                        show_service_view = true;
                    }
                    catch (Exception ex)
                    {
                    }
                }
            });
        });
    }
}
