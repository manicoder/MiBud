using MiBud.Droid.Dependencies;
using MiBud.Interfaces;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceMacAddress))]
namespace MiBud.Droid.Dependencies
{
    public class DeviceMacAddress : IDeviceMacAddress
    {
        public async Task<string> GetMacAddress()
        {
            string ip = string.Empty;
            try
            {
                var ni = NetworkInterface.GetAllNetworkInterfaces().OrderBy(intf => intf.NetworkInterfaceType)
            .FirstOrDefault(intf => intf.OperationalStatus == OperationalStatus.Up
             && (intf.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
             || intf.NetworkInterfaceType == NetworkInterfaceType.Ethernet));
                {
                    var hw = ni.GetPhysicalAddress();
                    ip = string.Join(":", (from ma in hw.GetAddressBytes() select ma.ToString("X2")).ToArray());
                }
                await Task.Delay(500);
                return ip;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}