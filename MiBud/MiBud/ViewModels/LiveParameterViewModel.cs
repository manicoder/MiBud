using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using MiBud.Interfaces;
using MiBud.Models;
using MiBud.Services;
using MiBud.StaticInfo;
using MiBud.Views.LiveParameter;
using Xamarin.Forms;

namespace MiBud.ViewModels
{
    public class LiveParameterViewModel : BaseViewModel
    {
        ApiServices apiServices;
        readonly IBlueToothDevices blueTooth;
        readonly ContentView page;
        public string read_data_fn_index = string.Empty;
        public LiveParameterViewModel(ContentView content)
        {
            try
            {
                apiServices = new ApiServices();
                this.page = content;
                this.blueTooth = DependencyService.Get<IBlueToothDevices>();
                ecus_list = new ObservableCollection<EcusModel>();

   
                InitializeCommands();
                read_data_fn_index = StaticMethods.ecu_info.FirstOrDefault().read_data_fn_index;
                Device.InvokeOnMainThreadAsync(async () =>
                {
                    GetPidList();
                });
            }
            catch (Exception ex)
            {
            }
        }

        #region properties
        private string _selected_ecu;
        public string selected_ecu
        {
            get => _selected_ecu;
            set
            {
                _selected_ecu = value;
                OnPropertyChanged("selected_ecu");
            }
        }

        private string _search_key;
        public string search_key
        {
            get => _search_key;
            set
            {
                _search_key = value;

                if (!string.IsNullOrEmpty(search_key))
                {
                    pid_list = new ObservableCollection<PidCode>(static_pid_list.Where(x => x.short_name.ToLower().Contains(search_key.ToLower())).ToList());
                }

                OnPropertyChanged("search_key");
            }
        }

        private string _txt_select_button = "Select All";
        public string txt_select_button
        {
            get => _txt_select_button;
            set
            {
                _txt_select_button = value;
                OnPropertyChanged("txt_select_button");
            }
        }

        private ObservableCollection<PidCode> _static_pid_list;
        public ObservableCollection<PidCode> static_pid_list
        {
            get => _static_pid_list;
            set
            {
                _static_pid_list = value;
                OnPropertyChanged("static_pid_list");
            }
        }

        private ObservableCollection<PidCode> _pid_list;
        public ObservableCollection<PidCode> pid_list
        {
            get => _pid_list;
            set
            {
                _pid_list = value;
                OnPropertyChanged("pid_list");
            }
        }

        private ObservableCollection<PidCode> _selected_pid_list;
        public ObservableCollection<PidCode> selected_pid_list
        {
            get => _selected_pid_list;
            set
            {
                _selected_pid_list = value;
                OnPropertyChanged("selected_pid_list");
            }
        }

        private ObservableCollection<EcusModel> _ecus_list;
        public ObservableCollection<EcusModel> ecus_list
        {
            get => _ecus_list;
            set
            {
                _ecus_list = value;
                OnPropertyChanged("ecus_list");
            }
        }
        #endregion

        #region Methods
        public void InitializeCommands()
        {
            SelectPidCommand = new Command(async (obj) =>
            {
                if (txt_select_button == "Select All")
                {
                    txt_select_button = "Unselect All";
                    foreach (var item in pid_list)
                    {
                        item.Selected = true;
                    }
                }
                else
                {
                    txt_select_button = "Select All";
                    foreach (var item in pid_list)
                    {
                        item.Selected = false;
                    }
                }
            });

            ContinueCommand = new Command(async (obj) =>
            {
                try
                {
                    var selected_pid = pid_list.Where(x => x.Selected).ToList();

                    if (selected_pid.Count > 0)
                    {
                        await page.Navigation.PushAsync(new LiveParameterSelectedPage(new ObservableCollection<PidCode>(selected_pid)));
                        //await App.Current.MainPage.Navigation.PushModalAsync(new LiveParameterSelectedPage(new ObservableCollection<PidCode>(selected_pid)));
                    }
                    else
                    {
                       await UserDialogs.Instance.AlertAsync("Please select any parameter", "Alert", "OK");
                    }
                }
                catch (Exception ex)
                {
                }
            });

            EcuTabCommand = new Command(async (obj) =>
            {
                try
                {
                    var selected_ecu = (EcusModel)obj;
                    selected_ecu.opacity = 1;
                    pid_list = new ObservableCollection<PidCode>(selected_ecu?.pid_list);
                    read_data_fn_index = StaticMethods.ecu_info.FirstOrDefault(x => x.ecu_name == selected_ecu.ecu_name).read_data_fn_index;
                    foreach (var ecu in ecus_list)
                    {
                        if (selected_ecu != ecu)
                        {
                            ecu.opacity = .5;
                        }
                    }

                    var rxHeader = StaticMethods.ecu_info.FirstOrDefault(X => X.ecu_name == selected_ecu.ecu_name).rx_header;
                    var txHeader = StaticMethods.ecu_info.FirstOrDefault(X => X.ecu_name == selected_ecu.ecu_name).tx_header;

                    DependencyService.Get<Interfaces.IBlueToothDevices>().SendHeader(txHeader, rxHeader);
                }
                catch (Exception ex)
                {
                }
            });
        }

        public async void GetPidList()
        {
            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            {
                await Task.Delay(200);
                try
                {
                    int count = 0;
                    if (read_data_fn_index.Contains("GENERIC"))
                    {
                        var suported_pid = await blueTooth.GetGenericObdPid();

                        foreach (var ecu in StaticMethods.ecu_info)
                        {
                            count++;
                            int pid_dataset = ecu.pid_dataset_id;
                            var pid_li = await apiServices.GetPid(App.access_token, pid_dataset, App.is_update);
                            List<PidCode> supported_pid_list = new List<PidCode>();
                            foreach (var item in pid_li.codes)
                            {
                                foreach (var item2 in suported_pid)
                                {
                                    if (item.code == item2)
                                    {
                                        supported_pid_list.Add(item);
                                    }
                                }
                            }
                            ecus_list.Add(
                                new EcusModel
                                {
                                    ecu_name = ecu.ecu_name,
                                    opacity = count == 1 ? 1 : .5,
                                    pid_list = supported_pid_list,
                                });
                        }

                        static_pid_list = pid_list = new ObservableCollection<PidCode>(ecus_list.FirstOrDefault().pid_list);
                    }
                    else
                    {
                        foreach (var ecu in StaticMethods.ecu_info)
                        {
                            count++;
                            int pid_dataset = ecu.pid_dataset_id;
                            var pid_li = await apiServices.GetPid(App.access_token, pid_dataset, App.is_update);

                            ecus_list.Add(
                                new EcusModel
                                {
                                    ecu_name = ecu.ecu_name,
                                    opacity = count == 1 ? 1 : .5,
                                    pid_list = pid_li.codes,
                                });
                        }
                        static_pid_list = pid_list = new ObservableCollection<PidCode>(ecus_list.FirstOrDefault().pid_list);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        #region Commmads
        public ICommand ContinueCommand { get; set; }
        public ICommand EcuTabCommand { get; set; }
        public ICommand SelectPidCommand { get; set; }
        #endregion

    }
}
