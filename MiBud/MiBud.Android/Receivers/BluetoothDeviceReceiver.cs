using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MiBud.Droid.StaticClass;
using MiBud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiBud.Droid.Receivers
{
    public class BluetoothDeviceReceiver : BroadcastReceiver
    {
        public static BluetoothAdapter Adapter => BluetoothAdapter.DefaultAdapter;

        public override void OnReceive(Context context, Intent intent)
        {
            var action = intent.Action;
            //ObservableCollection<BluetoothDevicesModel> list = new ObservableCollection<BluetoothDevicesModel>();
            // Found a device
            switch (action)
            {
                case BluetoothDevice.ActionFound:
                    // Get the device
                    var device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);

                    if (device.Name == "miBud")
                    {
                        var list = App.bluetooth_devices.FirstOrDefault(x => x.mac_address== device.Address);
                        if (list == null)
                        {
                            App.bluetooth_devices.Add(
                                new BluetoothDevicesModel
                                {
                                    mac_address = device.Address,
                                    name= device.Name,
                                });
                        }
                        BluetoothStatic.bluetoothDevices.Add(device);
                    }
                    break;
                case BluetoothAdapter.ActionDiscoveryStarted:
                    //MainActivity.GetInstance().UpdateAdapterStatus("Discovery Started...");
                    break;
                case BluetoothAdapter.ActionDiscoveryFinished:
                    App.bt_available = false;
                    //MainActivity.GetInstance().UpdateAdapterStatus("Discovery Finished.");
                    break;
                default:
                    break;
            }
        }
    }
}