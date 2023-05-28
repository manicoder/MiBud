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
    public class CreateWikitekTicketViewModel : BaseViewModel
    {
        
        public CreateWikitekTicketViewModel(WorkshopResult workshop)
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

            service_list = new ObservableCollection<string> { "REGULAR_MAINTENANCE", "REPAIR" };

            this.workshop = workshop;

            if (Device.Idiom == TargetIdiom.Phone)
            {
                symotomps_list_height = 56;
            }
            else
            {
                symotomps_list_height = 66;
            }
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
        private WorkshopResult _workshop;
        public WorkshopResult workshop
        {
            get => _workshop;
            set
            {
                _workshop = value;
                OnPropertyChanged("workshop");
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

        private bool _pickup_check = false;
        public bool pickup_check
        {
            get { return _pickup_check; }
            set
            {
                _pickup_check = value;
                OnPropertyChanged("pickup_check");
            }
        }

        private bool _drop_check = false;
        public bool drop_check
        {
            get { return _drop_check; }
            set
            {
                _drop_check = value;
                OnPropertyChanged("drop_check");
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


        public ICommand HideUserViewCommand => new Command(async (obj) =>
        {
            show_service_view = false;
        });

    }
}
