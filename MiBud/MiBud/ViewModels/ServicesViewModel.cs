using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MiBud.Models;

namespace MiBud.ViewModels
{
    public class ServicesViewModel:BaseViewModel
    {
        Services.ApiServices services;
        int submodel_id = 0;
        int id = 0;
        public ServicesViewModel(int submodel_id)
        {
            services = new Services.ApiServices();
            this.submodel_id = submodel_id;
            this.id = 0;
            is_loader = true;
            GetServiceList();
        }

        private ObservableCollection<ServiceModel> _service_list;
        public ObservableCollection<ServiceModel> service_list
        {
            get => _service_list;
            set
            {
                _service_list = value;
                OnPropertyChanged("service_list");
            }
        }

        private bool _is_loader = false;
        public bool is_loader
        {
            get => _is_loader;
            set
            {
                _is_loader = value;
                OnPropertyChanged("is_loader");
            }
        }

        public async void GetServiceList()
        {
            try
            {
                var res = await services.GetServices(App.access_token, id, submodel_id);
                is_loader = false;
                if (!res.success)
                {
                    Acr.UserDialogs.UserDialogs.Instance.Toast(res.status, new TimeSpan(0, 0, 0, 3));
                    return;
                }

                if (res.results == null || (!res.results.Any()))
                {
                    Acr.UserDialogs.UserDialogs.Instance.Toast("Services not found", new TimeSpan(0, 0, 0, 3));
                    return;
                }

                if (res.results.FirstOrDefault().service_model == null || (!res.results.FirstOrDefault().service_model.Any()))
                {
                    Acr.UserDialogs.UserDialogs.Instance.Toast("Services not found", new TimeSpan(0, 0, 0, 3));
                    return;
                }

                service_list = new ObservableCollection<ServiceModel>(res.results.FirstOrDefault().service_model.ToList());
            }
            catch (Exception ex)
            {
                is_loader = false;
            }
        }
    }
}
