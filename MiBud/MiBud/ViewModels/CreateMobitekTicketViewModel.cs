using MiBud.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace MiBud.ViewModels
{
    public class CreateMobitekTicketViewModel : BaseViewModel
    {
        public CreateMobitekTicketViewModel()
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
    }
}
