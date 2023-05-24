using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using MiBud.Models;
using MiBud.Services;
using MiBud.StaticInfo;
using Xamarin.Essentials;
using Xamarin.Forms;
//using MiBud.Services;

namespace MiBud.ViewModels
{
    public class DtcViewModel:ViewModelBase
    {
        ApiServices apiServices;
        readonly INavigation navigationService;
        public DtcViewModel(Page page) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
                ecus_list = new ObservableCollection<DtcEcusModel>();
                dtc_list = new ObservableCollection<DtcCode>();
                dtc_server_list = new List<DtcCode>();
                selected_ecu = StaticMethods.ecu_info.FirstOrDefault().ecu_name;

                InitializeCommands();
                Device.InvokeOnMainThreadAsync(async () =>
                {
                    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                    {
                        //await GetDTCList();
                    }
                });

                var pack1 = App.user.subscriptions.FirstOrDefault(x => x.package_name == "AssistedDiagnosticsPack");
                if (pack1 != null)
                {
                    refresh_dtc_visible = true;
                    clear_dtc_visible = true;
                    return;
                }

                var pack2 = App.user.subscriptions.FirstOrDefault(x => x.package_name == "DIYDiagnosticsPack");
                if (pack2 != null)
                {
                    refresh_dtc_visible = true;
                    clear_dtc_visible = true;
                    return;
                }

                var pack3 = App.user.subscriptions.FirstOrDefault(x => x.package_name == "BasicPack");
                if (pack3 != null)
                {
                    refresh_dtc_visible = true;
                    clear_dtc_visible = true;
                    return;
                }
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
        private string _DTCFoundOrNotMessage;
        public string DTCFoundOrNotMessage
        {
            get { return _DTCFoundOrNotMessage; }
            set { _DTCFoundOrNotMessage = value; OnPropertyChanged("DTCFoundOrNotMessage"); }
        }

        private ObservableCollection<DtcEcusModel> _ecus_list;
        public ObservableCollection<DtcEcusModel> ecus_list
        {
            get => _ecus_list;
            set
            {
                _ecus_list = value;
                OnPropertyChanged("ecus_list");
            }
        }

        private ObservableCollection<DtcCode> _dtc_list;
        public ObservableCollection<DtcCode> dtc_list
        {
            get => _dtc_list;
            set
            {
                _dtc_list = value;
                OnPropertyChanged("dtc_list");
            }
        }

        private List<DtcCode> _dtc_server_list;
        public List<DtcCode> dtc_server_list
        {
            get => _dtc_server_list;
            set
            {
                _dtc_server_list = value;
                OnPropertyChanged("dtc_server_list");
            }
        }

        private bool _refresh_dtc_visible = false;
        public bool refresh_dtc_visible
        {
            get => _refresh_dtc_visible;
            set
            {
                _refresh_dtc_visible = value;
                OnPropertyChanged("refresh_dtc_visible");
            }
        }

        private bool _clear_dtc_visible = false;
        public bool clear_dtc_visible
        {
            get => _clear_dtc_visible;
            set
            {
                _clear_dtc_visible = value;
                OnPropertyChanged("clear_dtc_visible");
            }
        }

        private bool _is_running = true;
        public bool is_running
        {
            get => _is_running;
            set
            {
                _is_running = value;
                OnPropertyChanged("is_running");
            }
        }

        private string _empty_view_text = "Loading...";
        public string empty_view_text
        {
            get => _empty_view_text;
            set
            {
                _empty_view_text = value;
                OnPropertyChanged("empty_view_text");
            }
        }

        private string _dtc_count = "";
        public string dtc_count
        {
            get => _dtc_count;
            set
            {
                _dtc_count = value;
                OnPropertyChanged("dtc_count");
            }
        }

        #endregion


        #region Methods


        public void InitializeCommands()
        {
            RefreshCommand = new Command(async (obj) =>
            {
                await Device.InvokeOnMainThreadAsync(async () =>
                {
                    ecus_list = new ObservableCollection<DtcEcusModel>();
                    dtc_list = new ObservableCollection<DtcCode>();
                    dtc_server_list = new List<DtcCode>();
                    await GetDTCList();
                });
            });

            ClearCommand = new Command(async (obj) =>
            {
                try
                {
                    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                    {
                        await Task.Delay(200);
                        var clear_dtc_index = StaticMethods.ecu_info.FirstOrDefault(X => X.ecu_name == selected_ecu).clear_dtc_index;
                        var Clear_dtc = await DependencyService.Get<Interfaces.IBlueToothDevices>().ClearDtc(clear_dtc_index);
                        if (Clear_dtc.Contains("NOERROR"))
                        {
                            await UserDialogs.Instance.AlertAsync("DTC Cleared", "Success!", "OK");
                            ecus_list = new ObservableCollection<DtcEcusModel>();
                            dtc_list = new ObservableCollection<DtcCode>();
                            dtc_server_list = new List<DtcCode>();
                            await GetDTCList();
                        }
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
                    var selected = (DtcEcusModel)obj;
                    selected.opacity = 1;
                    dtc_list = new ObservableCollection<DtcCode>(selected.dtc_list);
                    selected_ecu = selected.ecu_name;
                    foreach (var ecu in ecus_list)
                    {
                        if (selected != ecu)
                        {
                            ecu.opacity = .5;
                        }
                    }

                    var rxHeader = StaticMethods.ecu_info.FirstOrDefault(X => X.ecu_name == selected_ecu).rx_header;
                    var txHeader = StaticMethods.ecu_info.FirstOrDefault(X => X.ecu_name == selected_ecu).tx_header;

                    DependencyService.Get<Interfaces.IBlueToothDevices>().SendHeader(txHeader, rxHeader);

                    //GetPidList();
                }
                catch (Exception ex)
                {
                }
            });

            //TabCommand = new Command(async (obj) =>
            //{
            //    foreach (var item in child.Children)
            //    {
            //        //var grd = ((Grid)item);

            //        ////var grd = ((Grid)grd_tab.Children.ElementAt(i));
            //        ////var collection = ((CollectionView)collection_view.Children.ElementAt(i));
            //        //if (item.Opacity == class_id && collection.ClassId == class_id)
            //        //{
            //        //    grd.Opacity = 1;
            //        //    collection.IsVisible = true;
            //        //    selected_ecu = lbl.Text;
            //        //}
            //        //else
            //        //{
            //        //    grd.Opacity = 0.5;
            //        //    collection.IsVisible = false;
            //        //}
            //    }
            //});
        }

        public async Task GetDTCList()
        {
            try
            {
                //using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    //await Task.Delay(200);
                    DTCFoundOrNotMessage = "Looking for DTC Record";
                    is_running = true;
                    empty_view_text = "Loading...";
                    int count = 0;
                    foreach (var ecu in StaticMethods.ecu_info)
                    {
                        count++;
                        int dtc_dataset = ecu.dtc_dataset_id;
                        var dtc_li = await apiServices.GetDtc(Preferences.Get("token", null), dtc_dataset, App.is_update);
                        dtc_server_list = dtc_li.codes;
                        if (dtc_li == null)
                        {
                            await UserDialogs.Instance.AlertAsync("Please check device internet connection.", "Failed", "Ok");
                            DTCFoundOrNotMessage = "Internet connection Issue.";
                            return;
                        }

                        DtcEcusModel dtcEcusModel = new DtcEcusModel();
                        dtcEcusModel.ecu_name = ecu.ecu_name;
                        dtcEcusModel.opacity = count == 1 ? 1 : .5;
                        Debug.WriteLine($"Dtc View Model : ECU NAME = {ecu.ecu_name},  DTC DATASET ID = {dtc_dataset}");
                        //DependencyService.Get<Interfaces.IBlueToothDevices>().SendHeader(ecu.tx_header, ecu.rx_header);
                        var read_dtc = await DependencyService.Get<Interfaces.IBlueToothDevices>().ReadDtc(ecu.read_dtc_index);
                        if (read_dtc != null)
                        {
                            if (read_dtc.status == "NO_ERROR")
                            {
                                var code = read_dtc.dtcs.GetLength(0);
                                var status = read_dtc.dtcs.GetLength(1);
                                for (int i = 0; i <= code - 1; i++)
                                {
                                    DtcCode dtcListModel = new DtcCode();
                                    dtcListModel.code = read_dtc.dtcs[i, 0].ToString();
                                    for (int j = 0; j <= 0; j++)
                                    {
                                        var dtc_status = read_dtc.dtcs[i, 1].ToString();
                                        string[] split_string = dtc_status.Split(':');

                                        if (dtc_status.Contains("Current"))
                                        {
                                            dtcListModel.status_activation = dtc_status;
                                            dtcListModel.status_activation_color = Color.Red;
                                        }
                                        else if (dtc_status.Contains("Pending"))
                                        {
                                            dtcListModel.status_activation = dtc_status;
                                            dtcListModel.status_activation_color = Color.Green;
                                        }
                                        else
                                        {

                                            if (split_string[0] == "Inactive")
                                            {
                                                dtcListModel.status_activation = split_string[0];
                                                dtcListModel.status_activation_color = Color.Green;
                                            }
                                            else if (split_string[0] == "Active")
                                            {

                                                dtcListModel.status_activation = split_string[0];
                                                dtcListModel.status_activation_color = Color.Red;
                                            }

                                            if (split_string[1] == "LampOff")
                                            {
                                                dtcListModel.lamp_activation = split_string[1];
                                                dtcListModel.lamp_activation_color = Color.Green;
                                            }
                                            else if (split_string[1] == "LampOn")
                                            {

                                                dtcListModel.lamp_activation = split_string[1];
                                                dtcListModel.lamp_activation_color = Color.Red;
                                            }
                                        }
                                        Debug.WriteLine($"Dtc View Model : CODE = {dtcListModel.code}, "); //STATUS= {split_string[0]}:{split_string[1]}");
                                    }
                                    if (dtc_server_list != null)
                                    {
                                        var desc = dtc_server_list.FirstOrDefault(x => x.code == dtcListModel.code);
                                        if (desc != null)
                                        {
                                            dtcListModel.description = desc.description;
                                        }
                                        else
                                        {
                                            dtcListModel.description = "Description not found";
                                        }
                                        dtc_list.Add(dtcListModel);
                                    }
                                    else
                                    {
                                        DTCFoundOrNotMessage = "Please check DataSet Record !!! \nOR\n DataSet is Active or Not !!!";
                                    }
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(read_dtc.status))
                                {
                                    is_running = false;
                                    empty_view_text = read_dtc.status;
                                    dtcEcusModel.dtc_list = new List<DtcCode>(dtc_list);
                                    return;
                                }
                                else
                                {
                                    is_running = false;
                                    empty_view_text = "ECU_COMMUNICATION_ERROR";
                                    dtcEcusModel.dtc_list = new List<DtcCode>(dtc_list);
                                    return;
                                }
                                //DTC_Error = read_dtc.status;
                            }
                            dtcEcusModel.dtc_list = new List<DtcCode>(dtc_list);
                        }
                        else
                        {
                            dtcEcusModel.dtc_list = new List<DtcCode>();
                        }
                        ecus_list.Add(dtcEcusModel);

                        if (ecus_list.FirstOrDefault().dtc_list.Count() == 0)
                        {
                            is_running = false;
                            empty_view_text = "NO DTC FOUND FOR THIS ECU";
                            return;
                        }
                        else
                        {
                            is_running = false;
                            empty_view_text = "NO DTC FOUND FOR THIS ECU";
                            dtc_count = $"{ecus_list.FirstOrDefault().dtc_list.Count()} DTCs";
                        }
                    }
                    dtc_list = new ObservableCollection<DtcCode>(ecus_list.FirstOrDefault().dtc_list);
                    selected_ecu = ecus_list[0].ecu_name;
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        public ICommand RefreshCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand EcuTabCommand { get; set; }
    }
}
