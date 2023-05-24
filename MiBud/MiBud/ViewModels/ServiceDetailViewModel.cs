using Acr.UserDialogs;
using MiBud.Models;
using MiBud.Services;
using System;
using System.Collections;
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
    public class ServiceDetailViewModel : BaseViewModel
    {
        ApiServices apiServices;
        private JobcardResult _jobcard_detail;
        private double _description_height = 250;

        private bool _show_symptoms_view = false;
        private bool _show_service_view = false;
        private bool _show_spare_part_view = false;
        private bool _action_button_enable = false;
        private bool _show_cost_view = false;

        private string _txt_search_symptoms = "";
        private string _txt_search_services = "";
        private string _txt_search_spare_parts = "";
        //private int submodel_id = 0;

        private ObservableCollection<SymptomRtModel> _symptoms;
        private ObservableCollection<ServiceModel> _services;
        //private ObservableCollection<SparepartModel> _spare_parts;

        private ObservableCollection<SymptomRtModel> static_symptoms;
        private ObservableCollection<ServiceModel> static_services;
        //private ObservableCollection<SparepartModel> static_spare_parts;

        public ServiceDetailViewModel(JobcardResult jobcard_detail)
        {
            try
            {
                apiServices = new ApiServices();
                this.jobcard_detail = jobcard_detail;
                SetActionButtonStatus();

                //Device.BeginInvokeOnMainThread(async () =>
                //{
                //    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                //    {
                //        await Task.Delay(100);

                //        await GetGobcard();
                //    }
                //});
            }
            catch (Exception ex)
            {
            }
        }

        #region Properties
        public double description_height
        {
            get => _description_height;
            set
            {
                _description_height = value;
                OnPropertyChanged("description_height");
            }
        }

        public JobcardResult jobcard_detail
        {
            get { return _jobcard_detail; }
            set
            {
                _jobcard_detail = value;
                OnPropertyChanged("jobcard_detail");
            }
        }

        public ObservableCollection<SymptomRtModel> symptoms
        {
            get { return _symptoms; }
            set
            {
                _symptoms = value;
                OnPropertyChanged("symptoms");
            }
        }

        public ObservableCollection<ServiceModel> services
        {
            get { return _services; }
            set
            {
                _services = value;
                OnPropertyChanged("services");
            }
        }

        //public ObservableCollection<SparepartModel> spare_parts
        //{
        //    get { return _spare_parts; }
        //    set
        //    {
        //        _spare_parts = value;
        //        OnPropertyChanged("spare_parts");
        //    }
        //}

        public bool show_symptoms_view
        {
            get => _show_symptoms_view;
            set
            {
                _show_symptoms_view = value;
                OnPropertyChanged("show_symptoms_view");
            }
        }

        public bool show_service_view
        {
            get => _show_service_view;
            set
            {
                _show_service_view = value;
                OnPropertyChanged("show_service_view");
            }
        }

        public bool show_spare_part_view
        {
            get => _show_spare_part_view;
            set
            {
                _show_spare_part_view = value;
                OnPropertyChanged("show_spare_part_view");
            }
        }

        public bool action_button_enable
        {
            get => _action_button_enable;
            set
            {
                _action_button_enable = value;
                OnPropertyChanged("action_button_enable");
            }
        }

        public bool show_cost_view
        {
            get => _show_cost_view;
            set
            {
                _show_cost_view = value;
                OnPropertyChanged("show_cost_view");
            }
        }

        public string txt_search_symptoms
        {
            get => _txt_search_symptoms;
            set
            {
                _txt_search_symptoms = value;
                OnPropertyChanged("txt_search_symptoms");

                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (!string.IsNullOrWhiteSpace(txt_search_symptoms))
                    {
                        try
                        {
                            symptoms = new ObservableCollection<SymptomRtModel>(static_symptoms.Where(x => x.symptom.symptom_name.Contains(txt_search_symptoms)).ToList());
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                    {
                        symptoms = static_symptoms;
                    }
                });
            }
        }

        public string txt_search_services
        {
            get => _txt_search_services;
            set
            {
                _txt_search_services = value;
                OnPropertyChanged("txt_search_services");

                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (!string.IsNullOrWhiteSpace(txt_search_services))
                    {
                        try
                        {
                            services = new ObservableCollection<ServiceModel>(static_services.Where(x => x.service.service_name.Contains(txt_search_services)).ToList());
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                    {
                        services = static_services;
                    }
                });
            }
        }

        public string txt_search_spare_parts
        {
            get => _txt_search_spare_parts;
            set
            {
                _txt_search_spare_parts = value;
                OnPropertyChanged("txt_search_services");

                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (!string.IsNullOrWhiteSpace(txt_search_spare_parts))
                    {
                        try
                        {
                            services = new ObservableCollection<ServiceModel>(static_services.Where(x => x.service.service_name.Contains(txt_search_spare_parts)).ToList());
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                    {
                        services = static_services;
                    }
                });
            }
        }
        #endregion


        #region Methods
        public async Task SetActionButtonStatus()
        {
            try
            {
                switch (jobcard_detail.status)
                {
                    case "QuoteforTransport":
                        action_button_enable = false;
                        break;
                    case "ApprovedTransport":
                        action_button_enable = true;
                        break;
                }
            }
            catch (Exception ex)
            {
            }
        }

        public async Task GetGobcard()
        {
            try
            {
                var res = await apiServices.GetJobcardDetail(Preferences.Get("token", null), jobcard_detail.id);

                if (!res.status)
                {
                    UserDialogs.Instance.Toast(res.message, new TimeSpan(0, 0, 0, 2));
                    return;
                }

                if (res.results == null || (!res.results.Any()))
                {
                    UserDialogs.Instance.Toast("Jobcard detail not found.", new TimeSpan(0, 0, 0, 2));
                    return;
                }

                jobcard_detail = res.results[0];
            }
            catch (Exception ex)
            {
            }
        }

        //public async Task<ArrayList> GetSubmodel()
        //{
        //    ArrayList arrayList = new ArrayList();
        //    try
        //    {
        //        var result = await apiServices.GetVehicleDetail(Preferences.Get("token", null), jobcard_detail.registration_no.registration_id);


        //        if (!result.status)
        //        {
        //            //DependencyService.Get<Interfaces.IToasts>().Show($"{ mode.status}");
        //            Acr.UserDialogs.UserDialogs.Instance.Toast(result.message, new TimeSpan(0, 0, 0, 3));
        //            arrayList.Add(0);
        //            return arrayList;
        //        }

        //        if (result.results == null || (!result.results.Any()))
        //        {
        //            Acr.UserDialogs.UserDialogs.Instance.Toast("Sub-Model not found", new TimeSpan(0, 0, 0, 3));
        //            arrayList.Add(0);
        //            return arrayList;
        //        }

        //        arrayList.Add(result.results[0].sub_model);
        //        arrayList.Add(result.results[0].oem);
        //        return arrayList;
        //    }
        //    catch (Exception ex)
        //    {
        //        arrayList.Add(0);
        //        return arrayList;
        //    }
        //}

        //public async Task GetSymptoms()
        //{
        //    try
        //    {


        //        int submodel_id = 0;
        //        var result = await GetSubmodel();
        //        if ((submodel_id = Convert.ToInt32(result[0])) == 0)
        //        {
        //            return;
        //        }

        //        var res = await apiServices.GetSymptoms(Preferences.Get("token", null), submodel_id);

        //        if (!res.success)
        //        {
        //            //DependencyService.Get<Interfaces.IToasts>().Show($"{ mode.status}");
        //            Acr.UserDialogs.UserDialogs.Instance.Toast(res.status, new TimeSpan(0, 0, 0, 3));
        //            return;
        //        }

        //        if (res.results == null || (!res.results.Any()))
        //        {
        //            Acr.UserDialogs.UserDialogs.Instance.Toast("Symptoms Not found", new TimeSpan(0, 0, 0, 3));
        //            return;
        //        }

        //        if (res.results.FirstOrDefault().symptom_rt_model == null || (!res.results.FirstOrDefault().symptom_rt_model.Any()))
        //        {
        //            Acr.UserDialogs.UserDialogs.Instance.Toast("Symptoms Not found", new TimeSpan(0, 0, 0, 3));
        //            return;
        //        }

        //        symptoms = new ObservableCollection<SymptomRtModel>(res.results.FirstOrDefault().symptom_rt_model.ToList());

        //        show_symptoms_view = true;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //public async Task GetServices()
        //{
        //    try
        //    {

        //        int submodel_id = 0;
        //        var result = await GetSubmodel();
        //        if ((submodel_id = Convert.ToInt32(result[0])) == 0)
        //        {
        //            return;
        //        }

        //        var res = await apiServices.GetServices(Preferences.Get("token", null), submodel_id);

        //        if (!res.success)
        //        {
        //            UserDialogs.Instance.Toast(res.status, new TimeSpan(0, 0, 0, 2));
        //            return;
        //        }

        //        if (res.results == null || (!res.results.Any()))
        //        {
        //            UserDialogs.Instance.Toast("Services not found", new TimeSpan(0, 0, 0, 2));
        //            return;
        //        }

        //        if (res.results.FirstOrDefault().service_model == null || (!res.results.FirstOrDefault().service_model.Any()))
        //        {
        //            UserDialogs.Instance.Toast("Services Not found", new TimeSpan(0, 0, 0, 2));
        //            return;
        //        }

        //        services = new ObservableCollection<ServiceModel>(res.results.FirstOrDefault().service_model.ToList());

        //        //show_symptoms_view = true;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //public async Task GetSpareParts()
        //{
        //    try
        //    {

        //        int submodel_id = 0;
        //        var result = await GetSubmodel();
        //        if ((submodel_id = Convert.ToInt32(result[0])) == 0)
        //        {
        //            return;
        //        }

        //        var res = await apiServices.GetSpareParts(Preferences.Get("token", null), submodel_id);

        //        if (!res.success)
        //        {
        //            UserDialogs.Instance.Toast(res.status, new TimeSpan(0, 0, 0, 2));
        //            return;
        //        }

        //        if (res.results == null || (!res.results.Any()))
        //        {
        //            UserDialogs.Instance.Toast("Spare parts not found", new TimeSpan(0, 0, 0, 2));
        //            return;
        //        }

        //        if (res.results.FirstOrDefault().sparepart_model == null
        //            || (!res.results.FirstOrDefault().sparepart_model.Any()))
        //        {
        //            UserDialogs.Instance.Toast("Spare parts found", new TimeSpan(0, 0, 0, 2));
        //            return;
        //        }

        //        spare_parts = new ObservableCollection<SparepartModel>(res.results.FirstOrDefault().sparepart_model.ToList());

        //        //show_symptoms_view = true;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //public async Task UpadteSymptoms(string id, string key, string value)
        //{
        //    JobcardSymptom symptomRt = jobcard_detail.jobcard_symptom.FirstOrDefault(x => x.id == int.Parse(id));
        //    UpdateSymptom updateSymptom = new UpdateSymptom();
        //    if (key.Contains("entry_check"))
        //    {
        //        updateSymptom = new UpdateSymptom
        //        {
        //            id = int.Parse(id),
        //            customer_check = symptomRt.customer_check,
        //            entry_check = value,
        //            service_check = symptomRt.service_check,
        //            exit_check = symptomRt.exit_check,
        //        };
        //    }
        //    else if (key.Contains("service_check"))
        //    {
        //        updateSymptom = new UpdateSymptom
        //        {
        //            id = int.Parse(id),
        //            customer_check = symptomRt.customer_check,
        //            entry_check = symptomRt.entry_check,
        //            service_check = value,
        //            exit_check = symptomRt.exit_check,
        //        };
        //    }
        //    else if (key.Contains("exit_check"))
        //    {
        //        updateSymptom = new UpdateSymptom
        //        {
        //            id = int.Parse(id),
        //            customer_check = symptomRt.customer_check,
        //            entry_check = symptomRt.entry_check,
        //            service_check = symptomRt.service_check,
        //            exit_check = value,
        //        };
        //    }

        //    var res = await apiServices.UpdateSymptoms(Preferences.Get("token", null), updateSymptom);

        //    if (!res.success)
        //    {
        //        UserDialogs.Instance.Toast(res.error, new TimeSpan(0, 0, 0, 2));
        //        return;
        //    }
        //}

        //public async Task UpadteServices(string id, string key, string value)
        //{
        //    JobcardService service = jobcard_detail.jobcard_service.FirstOrDefault(x => x.id == id);
        //    UpdateServices update_service = new UpdateServices();
        //    if (key.Contains("entry_check"))
        //    {
        //        update_service = new UpdateServices
        //        {
        //            id = service.id,
        //            customer_check = service.customer_check,
        //            entry_check = value,
        //            service_check = service.service_check,
        //            exit_check = service.exit_check,
        //        };
        //    }
        //    else if (key.Contains("service_check"))
        //    {
        //        update_service = new UpdateServices
        //        {
        //            id = service.id,
        //            customer_check = service.customer_check,
        //            entry_check = service.entry_check,
        //            service_check = value,
        //            exit_check = service.exit_check,
        //        };
        //    }
        //    else if (key.Contains("exit_check"))
        //    {
        //        update_service = new UpdateServices
        //        {
        //            id = id,
        //            customer_check = service.customer_check,
        //            entry_check = service.entry_check,
        //            service_check = service.service_check,
        //            exit_check = value,
        //        };
        //    }

        //    var res = await apiServices.UpdateServices(Preferences.Get("token", null), update_service);

        //    if (!res.success)
        //    {
        //        UserDialogs.Instance.Toast(res.error, new TimeSpan(0, 0, 0, 2));
        //        return;
        //    }
        //}

        //public async Task UpadteSparePart(string id, string key, string value)
        //{
        //    JobcardSparepart sparepart = jobcard_detail.jobcard_sparepart.FirstOrDefault(x => x.id == id);
        //    UpdateSparePart update_sparepart = new UpdateSparePart();
        //    if (key.Contains("entry_check"))
        //    {
        //        update_sparepart = new UpdateSparePart
        //        {
        //            id = sparepart.id,
        //            customer_check = sparepart.customer_check,
        //            entry_check = value,
        //            service_check = sparepart.service_check,
        //            exit_check = sparepart.exit_check,
        //        };
        //    }
        //    else if (key.Contains("service_check"))
        //    {
        //        update_sparepart = new UpdateSparePart
        //        {
        //            id = sparepart.id,
        //            customer_check = sparepart.customer_check,
        //            entry_check = sparepart.entry_check,
        //            service_check = value,
        //            exit_check = sparepart.exit_check,
        //        };
        //    }
        //    else if (key.Contains("exit_check"))
        //    {
        //        update_sparepart = new UpdateSparePart
        //        {
        //            id = sparepart.id,
        //            customer_check = sparepart.customer_check,
        //            entry_check = sparepart.entry_check,
        //            service_check = sparepart.service_check,
        //            exit_check = value,
        //        };
        //    }

        //    var res = await apiServices.UpdateSparePart(Preferences.Get("token", null), update_sparepart);

        //    if (!res.success)
        //    {
        //        UserDialogs.Instance.Toast(res.error, new TimeSpan(0, 0, 0, 2));
        //        return;
        //    }
        //}
        #endregion

        #region ICommands
        //public ICommand StartDaignosticCommand => new Command(async (obj) =>
        //{
        //    Device.BeginInvokeOnMainThread(async () =>
        //    {
        //        using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
        //        {
        //            await Task.Delay(100);

        //            int submodel_id = 0;
        //            var result = await GetSubmodel();
        //            if ((submodel_id = Convert.ToInt32(result[0])) == 0)
        //            {
        //                return;
        //            }

        //            var res = await apiServices.GetVehicleEcu(Preferences.Get("token", null), submodel_id);

        //            if (!res.success)
        //            {
        //                UserDialogs.Instance.Toast(res.error, new TimeSpan(0, 0, 0, 2));
        //                return;
        //            }

        //            if (res.results == null || (!res.results.Any()))
        //            {
        //                UserDialogs.Instance.Toast("Model not found.", new TimeSpan(0, 0, 0, 2));
        //                return;
        //            }

        //            var sub_model = res.results[0].sub_models.FirstOrDefault(x => x.id == submodel_id);

        //            if (sub_model == null)
        //            {
        //                UserDialogs.Instance.Toast("Sub-Model not found.", new TimeSpan(0, 0, 0, 2));
        //                return;
        //            }

        //            if (sub_model.ecus == null || (!sub_model.ecus.Any()))
        //            {
        //                UserDialogs.Instance.Toast("Ecu not found.", new TimeSpan(0, 0, 0, 2));
        //                return;
        //            }



        //            var model =
        //            App.submodel_id = sub_model.id;
        //            StaticMethods.ecu_info.Clear();
        //            foreach (var item in sub_model.ecus)
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
        //                        is_padding = item.padding
        //                    });
        //            }

        //            await page.Navigation.PushAsync(new Views.Connections.ConnectionPage(App.user, res.results[0], sub_model, (Oem)result[1]));

        //        }
        //    });
        //});

        //public ICommand ShowSympPopupCommand => new Command(async (obj) =>
        //{
        //    Device.BeginInvokeOnMainThread(async () =>
        //    {
        //        using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
        //        {
        //            await Task.Delay(100);

        //            show_symptoms_view = true;
        //            await GetSymptoms();
        //        }
        //    });
        //});

        //public ICommand ShowServicePopupCommand => new Command(async (obj) =>
        //{
        //    Device.BeginInvokeOnMainThread(async () =>
        //    {
        //        using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
        //        {
        //            await Task.Delay(100);

        //            show_service_view = true;
        //            await GetServices();
        //        }
        //    });
        //});

        //public ICommand ShowSparePartsPopupCommand => new Command(async (obj) =>
        //{
        //    Device.BeginInvokeOnMainThread(async () =>
        //    {
        //        using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
        //        {
        //            await Task.Delay(100);

        //            show_spare_part_view = true;
        //            await GetSpareParts();
        //        }
        //    });
        //});

        //public ICommand AddSymptomsCommand => new Command(async (obj) =>
        //{
        //    Device.BeginInvokeOnMainThread(async () =>
        //    {
        //        using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
        //        {
        //            await Task.Delay(100);

        //            try
        //            {
        //                var selected_symp = symptoms.Where(x => x.selected).ToList();

        //                foreach (var item in selected_symp.ToList())
        //                {
        //                    var result = await apiServices.AddSymptoms(Preferences.Get("token", null)
        //                     , new AddSymptom
        //                     {
        //                         job_card = jobcard_detail.id,
        //                         symptom = item.symptom.id,
        //                         customer_check = "NO",
        //                         entry_check = "YES",
        //                         service_check = "NO",
        //                         exit_check = "NO",
        //                         status = "YES"
        //                     });

        //                    if (!result.success)
        //                    {
        //                        UserDialogs.Instance.Toast(result.error, new TimeSpan(0, 0, 0, 2));
        //                        await Task.Delay(2000);
        //                    }
        //                }

        //                await GetGobcard();
        //            }
        //            catch (Exception ex)
        //            {
        //            }
        //            finally
        //            {
        //                show_symptoms_view = false;
        //            }
        //        }
        //    });
        //});

        //public ICommand AddServiceCommand => new Command(async (obj) =>
        //{
        //    Device.BeginInvokeOnMainThread(async () =>
        //    {
        //        using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
        //        {
        //            await Task.Delay(100);

        //            try
        //            {
        //                var selected_symp = services.Where(x => x.selected).ToList();

        //                foreach (var item in selected_symp.ToList())
        //                {
        //                    var result = await apiServices.AddServices(Preferences.Get("token", null)
        //                     , new AddService
        //                     {
        //                         job_card = jobcard_detail.id,
        //                         service = item.service.id,
        //                         customer_check = "NO",
        //                         entry_check = "YES",
        //                         service_check = "NO",
        //                         exit_check = "NO",
        //                         status = "YES",
        //                         unit_price = item.charge,
        //                         quantity = 1
        //                     });

        //                    if (!result.success)
        //                    {
        //                        UserDialogs.Instance.Toast(result.error, new TimeSpan(0, 0, 0, 2));
        //                        await Task.Delay(2000);
        //                    }
        //                }

        //                await GetGobcard();
        //            }
        //            catch (Exception ex)
        //            {
        //            }
        //            finally
        //            {
        //                show_service_view = false;
        //            }
        //        }
        //    });
        //});

        //public ICommand AddSparePartsCommand => new Command(async (obj) =>
        //{
        //    Device.BeginInvokeOnMainThread(async () =>
        //    {
        //        using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
        //        {
        //            await Task.Delay(100);

        //            try
        //            {
        //                var selected_spare_part = spare_parts.Where(x => x.selected).ToList();

        //                foreach (var item in selected_spare_part.ToList())
        //                {
        //                    var result = await apiServices.AddSparePart(Preferences.Get("token", null)
        //                         , new AddSparePart
        //                         {
        //                             job_card = jobcard_detail.id,
        //                             sparepart = item.sparepart.id,
        //                             quantity = 1,
        //                             desc = "",
        //                             customer_check = "NO",
        //                             entry_check = "YES",
        //                             service_check = "NO",
        //                             exit_check = "NO",
        //                             status = "YES",
        //                             unit_price = item.charge
        //                         });

        //                    if (!result.success)
        //                    {
        //                        UserDialogs.Instance.Toast(result.error, new TimeSpan(0, 0, 0, 2));
        //                        await Task.Delay(2000);
        //                    }
        //                }

        //                var res = await apiServices.GetJobcardDetail(Preferences.Get("token", null), jobcard_detail.id);

        //                if (!res.status)
        //                {
        //                    //DependencyService.Get<Interfaces.IToasts>().Show($"{ mode.status}");
        //                    UserDialogs.Instance.Toast(res.message, new TimeSpan(0, 0, 0, 2));
        //                    return;
        //                }

        //                if (res.results == null || (!res.results.Any()))
        //                {
        //                    UserDialogs.Instance.Toast("Jobcard detail not found.", new TimeSpan(0, 0, 0, 2));
        //                    return;
        //                }

        //                jobcard_detail = res.results[0];
        //            }
        //            catch (Exception ex)
        //            {
        //            }
        //            finally
        //            {
        //                show_symptoms_view = false;
        //            }
        //        }
        //    });
        //});

        //public ICommand UpdateSymptomCommand => new Command(async (obj) =>
        //{
        //    Device.BeginInvokeOnMainThread(async () =>
        //    {
        //        using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
        //        {
        //            await Task.Delay(100);

        //            try
        //            {

        //                var item = (ImageButton)obj;

        //                var class_id = item.ClassId;
        //                var automation_id = item.AutomationId;
        //                var style_id = item.StyleId;

        //                var result = class_id.Contains("YES") ? "NO" : "YES";
        //                if (style_id.Contains("Symptoms"))
        //                {

        //                    await UpadteSymptoms(automation_id, style_id, result);
        //                    //var selected
        //                }
        //                else if (style_id.Contains("Service"))
        //                {
        //                    await UpadteServices(automation_id, style_id, result);
        //                }
        //                else if (style_id.Contains("SparePart"))
        //                {
        //                    await UpadteSparePart(automation_id, style_id, result);
        //                }

        //                await GetGobcard();
        //            }
        //            catch (Exception ex)
        //            {
        //            }
        //            finally
        //            {
        //                show_symptoms_view = false;
        //            }
        //        }
        //    });
        //});

        public ICommand HidePopupViewCommand => new Command(async (obj) =>
        {
            show_cost_view = show_spare_part_view = show_service_view = show_symptoms_view = false;
        });

        public ICommand ActionCommand => new Command(async (obj) =>
        {
            if (jobcard_detail.status == "ApprovedTransport")
            {
                show_cost_view = true;
            }
            //await apiServices.QuoteforPickupDrop(Preferences.Get("token", null), jobcard_detail.id);
        });

        public ICommand ApproveCostCommand => new Command(async (obj) =>
        {
            await apiServices.PickupApproved(Preferences.Get("token", null), jobcard_detail.id);
            show_cost_view = false;
        });

        public ICommand CancelCostCommand => new Command(async (obj) =>
        {
            await apiServices.PickupReject(Preferences.Get("token", null), jobcard_detail.id);
            show_cost_view = false;
        });
        #endregion
    }
}
