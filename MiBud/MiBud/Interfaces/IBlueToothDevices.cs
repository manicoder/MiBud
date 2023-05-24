using MiBud.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace MiBud.Interfaces
{
    public interface IBlueToothDevices
    {
        Task SearchBlueTooth();

        Task<string> ConnectionBT(string name, int sleepTime, bool readAsCharArray);

        Task<string> GetFirmwareVersion(string dongle_type, string txHeader, string rxHeader, string protocol);

        void SendHeader(string txHeader, string rxHeader);

        Task<ReadDtcResponseModel> ReadDtc(string indexKey);

        Task<string> ClearDtc(string dtc_index);

        Task<string[]> GetGenericObdPid();

        Task<ObservableCollection<ReadPidPresponseModel>> ReadPid(ObservableCollection<ReadParameterPID> pidList);

        void DisconnectDongle();
    }
}
