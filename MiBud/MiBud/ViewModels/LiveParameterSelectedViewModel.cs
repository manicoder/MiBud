using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MiBud.Models;
using MiBud.Services;
using Xamarin.Forms;

namespace MiBud.ViewModels
{
    public class LiveParameterSelectedViewModel : ViewModelBase
    {
        ApiServices apiServices;
        readonly INavigation navigationService;
        readonly Page page;
        //TapGestureRecognizer tab_gesture;
        //Grid tab_view;
        //Grid list_view;
        public LiveParameterSelectedViewModel(Page page, ObservableCollection<PidCode> selectedPid) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
                this.page = page;
                this.navigationService = page.Navigation;
                //ecus_list = new ObservableCollection<EcusModel>();
                selected_pid_list = selectedPid;
                parameter_list = new ObservableCollection<ReadParameterPID>();
                final_parameter_list = new ObservableCollection<ReadParameterPID>();
                foreach (var item in selectedPid)
                {
                    parameter_list.Add(
                        new ReadParameterPID
                        {
                            pidName = item.short_name,
                            show_resolution = item.reset_value,
                            unit = item.unit,
                            pidNumber = item.id
                        });
                }

                InitializeCommands();
                Device.InvokeOnMainThreadAsync(async () =>
                {
                    //await GetPidsValue();
                    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                    {
                        await Task.Delay(200);

                        await GetPidsValue();
                    }
                });
            }
            catch (Exception ex)
            {
            }
        }

        #region properties
        private bool _play_button_enable = true;
        public bool play_button_enable
        {
            get => _play_button_enable;
            set
            {
                _play_button_enable = value;
                OnPropertyChanged("play_button_enable");
            }
        }

        private string _btnText = "Start";
        public string btnText
        {
            get => _btnText;
            set
            {
                _btnText = value;
                OnPropertyChanged("btnText");
            }
        }

        private bool _stop_button_enable = false;
        public bool stop_button_enable
        {
            get => _stop_button_enable;
            set
            {
                _stop_button_enable = value;
                OnPropertyChanged("stop_button_enable");
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

        private ObservableCollection<ReadParameterPID> _parameter_list;
        public ObservableCollection<ReadParameterPID> parameter_list
        {
            get => _parameter_list;
            set
            {
                _parameter_list = value;
                OnPropertyChanged("parameter_list");
            }
        }

        private ObservableCollection<ReadParameterPID> _final_parameter_list;
        public ObservableCollection<ReadParameterPID> final_parameter_list
        {
            get => _final_parameter_list;
            set
            {
                _final_parameter_list = value;
                OnPropertyChanged("final_parameter_list");
            }
        }
        #endregion
     
        #region Methods
        public bool StartTime = false;
        public void InitializeCommands()
        {
            PlayCommand = new Command(async (obj) =>
            {
                if (btnText == "Start")
                {
                    StartTime = true;
                    btnText = "Stop";
                }
                else
                {
                    StartTime = false;
                    btnText = "Start";
                }

                //StartTime = true;
                //play_button_enable = false;
                //stop_button_enable = true;
                while (StartTime)
                {
                    await Task.Run(async () =>
                    {
                        var read_pid = await DependencyService.Get<Interfaces.IBlueToothDevices>().ReadPid(final_parameter_list);
                        if (read_pid != null)
                        {
                            foreach (var pid in read_pid)
                            {
                                var pidlist = parameter_list.FirstOrDefault(x => x.pidNumber == pid.pidNumber);
                                if (pidlist != null)
                                {
                                    if (pid.Status == "NOERROR")
                                    {
                                        pidlist.show_resolution = pid.responseValue;
                                    }
                                    else
                                    {
                                        pidlist.show_resolution = "ERR";
                                        pidlist.unit = "";
                                    }
                                    //item.resolution= Convert.ToDouble(pid.responseValue);
                                }
                            }
                        }
                    });
                }

            });

            StopCommand = new Command(async (obj) =>
            {
                StartTime = false;
                play_button_enable = true;
                stop_button_enable = false;
            });
        }

        public async Task GetPidsValue()
        {
            try
            {
                ObservableCollection<ReadParameterPID> pidList = new ObservableCollection<ReadParameterPID>();
                parameter_list = new ObservableCollection<ReadParameterPID>();
                foreach (var item in selected_pid_list)
                {
                    var MessageModels = new List<Models.Message>();
                    pidList = new ObservableCollection<ReadParameterPID>();
                    foreach (var MessageItem in item.messages)
                    {
                        MessageModels.Add(new Models.Message { code = MessageItem.code, message = MessageItem.message });
                    }

                    pidList.Add(
                        new ReadParameterPID
                        {
                            pid = item.code,
                            totalLen = item.code.Length / 2,
                            //totalbyte -
                            startByte = item.byte_position,
                            noOfBytes = item.length,
                            IsBitcoded = item.bitcoded,
                            //noofBits = (int?)item.start_bit_position - (int?)item.end_bit_position + 1,
                            startBit = Convert.ToInt32(item.start_bit_position),
                            noofBits = item.end_bit_position.GetValueOrDefault() - item.start_bit_position.GetValueOrDefault() + 1,
                            resolution = item.resolution,
                            offset = item.offset,
                            datatype = item.message_type,
                            //totalBytes = item.length,
                            pidNumber = item.id,
                            pidName = item.short_name,
                            unit = item.unit,
                            messages = MessageModels
                        });

                    //final_parameter_list = pidList;
                    final_parameter_list.Add(pidList.FirstOrDefault());
                    parameter_list.Add(pidList.FirstOrDefault());
                    var read_pid = await DependencyService.Get<Interfaces.IBlueToothDevices>().ReadPid(pidList);

                    if (read_pid != null)
                    {
                        foreach (var pid in read_pid)
                        {
                            var pidlist = parameter_list.FirstOrDefault(x => x.pidNumber == pid.pidNumber);
                            if (pidlist != null)
                            {
                                if (pid.Status == "NOERROR")
                                {
                                    pidlist.show_resolution = pid.responseValue;
                                }
                                else
                                {
                                    pidlist.show_resolution = "ERR";
                                    pidlist.unit = "";
                                }
                                //item.resolution= Convert.ToDouble(pid.responseValue);
                            }
                        }

                        //Device.BeginInvokeOnMainThread(() =>
                        //{
                        //    //ems_List.ItemsSource = null;
                        //    //viewModel.EMSParameterList = ParameterList;
                        //    ///ems_List.ItemsSource = viewModel.EMSParameterList;
                        //});
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Commands
        public ICommand PlayCommand { get; set; }
        public ICommand StopCommand { get; set; }
        #endregion
    }
}
