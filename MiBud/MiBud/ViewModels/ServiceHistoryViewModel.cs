using MiBud.Models;
using MiBud.Services;
using MiBud.Views.ServiceHistoryJobcard;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace MiBud.ViewModels
{
    public class ServiceHistoryViewModel : ViewModelBase
    {
        ApiServices apiServices;
        readonly INavigation navigationService;
        readonly Page page;
        //MediaFile file = null;
        public ServiceHistoryViewModel(Page page) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
                this.page = page;
                this.navigationService = page.Navigation;
                //InitializeCommands();

                service_history_list = new ObservableCollection<ServiceHistoryModel>
                {
                    new ServiceHistoryModel
                    {
                        image = "blue.png",
                        jobcard_no= "jobcard_number_1",
                        date="17-08-2020",
                        inr="124",
                        status="status_1",
                        type = "wikitek"
                    },
                    new ServiceHistoryModel
                    {
                        image = "green.png",
                        jobcard_no= "jobcard_number_2",
                        date="18-08-2020",
                        inr="199",
                        status="status_2",
                        type = "mobitek"
                    },
                };
            }
            catch (Exception ex)
            {
            }
        }

        #region Properties

        private ObservableCollection<ServiceHistoryModel> _service_history_list;
        public ObservableCollection<ServiceHistoryModel> service_history_list
        {
            get => _service_history_list;
            set
            {
                _service_history_list = value;
                OnPropertyChanged("service_history_list");
            }
        }

        private ServiceHistoryModel _selected_service;
        public ServiceHistoryModel selected_service
        {
            get => _selected_service;
            set
            {
                _selected_service = value;
                if (selected_service != null)
                {
                    page.Navigation.PushAsync(new ServiceHistoryJobcardPage(selected_service));
                }
                OnPropertyChanged("selected_service");
            }
        }
        #endregion 
    }
}

