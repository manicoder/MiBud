using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MiBud.Models;

namespace MiBud.ViewModels
{
    public class SymptomsViewModel:BaseViewModel
    {
        Services.ApiServices services;
        int submodel_id = 0;
        public SymptomsViewModel(int submodel_id)
        {
            services = new Services.ApiServices();
            this.submodel_id = submodel_id;
            is_loader = true;
            GetSymptomsList();
        }

        private ObservableCollection<SymptomRtModel> _symotomps_list;
        public ObservableCollection<SymptomRtModel> symotomps_list
        {
            get => _symotomps_list;
            set
            {
                _symotomps_list = value;
                OnPropertyChanged("symotomps_list");
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

        public async void GetSymptomsList()
        {
            try
            {
                var res = await services.GetSymp(App.access_token, submodel_id);
                is_loader = false;
              
                if (!res.success)
                {
                    //DependencyService.Get<Interfaces.IToasts>().Show($"{ mode.status}");
                    Acr.UserDialogs.UserDialogs.Instance.Toast(res.status, new TimeSpan(0, 0, 0, 3));
                    return;
                }

                if (res.results == null || (!res.results.Any()))
                {
                    Acr.UserDialogs.UserDialogs.Instance.Toast("Symptoms Not found", new TimeSpan(0, 0, 0, 3));
                    return;
                }

                if (res.results.FirstOrDefault().symptom_rt_model == null || (!res.results.FirstOrDefault().symptom_rt_model.Any()))
                {
                    Acr.UserDialogs.UserDialogs.Instance.Toast("Symptoms Not found", new TimeSpan(0, 0, 0, 3));
                    return;
                }

                symotomps_list = new ObservableCollection<SymptomRtModel>(res.results.FirstOrDefault().symptom_rt_model.ToList());
            }
            catch (Exception ex)
            {
                is_loader=false;
            }
        }
    }
}
