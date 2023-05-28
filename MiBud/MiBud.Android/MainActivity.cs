using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using Acr.UserDialogs;
using MiBud.Droid.Receivers;
using Android.Bluetooth;
using System.Net;
//using Android.Support.V4.Content;
using Android.Support.V4.App;
using Android.Gms.Common.Apis;
using Android.Content;
using Rg.Plugins.Popup.Services;
//using Android.Gms.Location;
using Xamarin.Forms;
using MiBud.Views;
using AndroidX.Core.Content;
using AndroidX.Core.App;
using Android.Gms.Location;
using ActivityCompat = Android.Support.V4.App.ActivityCompat;

namespace MiBud.Droid
{
    [Activity(Label = "MiBud", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private BluetoothDeviceReceiver _receiver;
        const int RequestLocationId = 0;
        public const int REQUEST_CHECK_SETTINGS = 0x1;
        public static MainActivity Instance { get; private set; }
        readonly string[] LocationPermissions =
        {
             Manifest.Permission.AccessCoarseLocation ,
             Manifest.Permission.AccessFineLocation ,
             Manifest.Permission.BluetoothPrivileged,
             Manifest.Permission.ReadExternalStorage,
             Manifest.Permission.WriteExternalStorage,
             Manifest.Permission.ReadPhoneState
        };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            ServicePointManager
            .ServerCertificateValidationCallback +=
            (sender, cert, chain, sslPolicyErrors) => true;

            Instance = this;

            base.OnCreate(savedInstanceState);

            var coarseLocationPermissionGranted = ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation);
            var fineLocationPermissionGranted = ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation);
            var InternetPermissionGranted = ContextCompat.CheckSelfPermission(this, Manifest.Permission.Internet);
            if (coarseLocationPermissionGranted == Permission.Denied || fineLocationPermissionGranted == Permission.Denied || InternetPermissionGranted == Permission.Denied)
            {
                ActivityCompat.RequestPermissions(this, LocationPermissions, RequestLocationId);
            }
            else
            { }

            DisplayLocationSettingsRequest();

            //Log.Debug(TAG, "InstanceID token: " + FirebaseInstanceId.Instance.Token);
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;

            UserDialogs.Init(this);
            Rg.Plugins.Popup.Popup.Init(this);
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            _receiver = new BluetoothDeviceReceiver();
            LoadApplication(new App());

            Android.Bluetooth.BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            // is bluetooth enabled?
            //if (!bluetoothAdapter.IsEnabled)
            //{
            //    bluetoothAdapter.Enable();
            //}

            MessagingCenter.Subscribe<ClusterPage>(this, "AllowLandscape", sender =>
            {
                RequestedOrientation = ScreenOrientation.Landscape;
            });

            MessagingCenter.Subscribe<ClusterPage>(this, "PreventLandscape", sender =>
            {
                RequestedOrientation = ScreenOrientation.Portrait;
            });
        }

        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
                PopupNavigation.Instance.PopAsync();
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }
        }

        private void DisplayLocationSettingsRequest()
        {
            var googleApiClient = new GoogleApiClient.Builder(this).AddApi(LocationServices.API).Build();
            googleApiClient.Connect();

            var locationRequest = LocationRequest.Create();
            locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
            locationRequest.SetInterval(10000);
            locationRequest.SetFastestInterval(10000 / 2);

            var builder = new LocationSettingsRequest.Builder().AddLocationRequest(locationRequest);
            builder.SetAlwaysShow(true);

            var result = LocationServices.SettingsApi.CheckLocationSettings(googleApiClient, builder.Build());
            result.SetResultCallback((LocationSettingsResult callback) =>
            {
                switch (callback.Status.StatusCode)
                {
                    case LocationSettingsStatusCodes.Success:
                        {
                            //DoStuffWithLocation();
                            break;
                        }
                    case LocationSettingsStatusCodes.ResolutionRequired:
                        {
                            try
                            {
                                // Show the dialog by calling startResolutionForResult(), and check the result
                                // in onActivityResult().
                                callback.Status.StartResolutionForResult(this, REQUEST_CHECK_SETTINGS);
                            }
                            catch (IntentSender.SendIntentException e)
                            {
                            }

                            break;
                        }
                    default:
                        {
                            // If all else fails, take the user to the android location settings
                            StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
                            break;
                        }
                }
            });
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            switch (requestCode)
            {
                case REQUEST_CHECK_SETTINGS:
                    {
                        //switch (resultCode)
                        //{
                        //    case Android.App.Result.Ok:
                        //        {
                        //            DoStuffWithLocation();
                        //            break;
                        //        }
                        //    case Android.App.Result.Canceled:
                        //        {
                        //            //No location
                        //            break;
                        //        }
                        //}
                        break;
                    }
            }

            if (requestCode == RequestLocationId)
            {
                if ((grantResults.Length == 1) && (grantResults[0] == (int)Permission.Granted))
                {
                    // Permissions granted - display a message.
                }
                else
                {
                    // Permissions denied - display a message.
                }
            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnStart()
        {
            base.OnStart();

            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
                {
                    RequestPermissions(LocationPermissions, RequestLocationId);
                }
                else
                {
                    // Permissions already granted - display a message.
                }
            }
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", e.Exception);
            //LogUnhandledException(newExc);
        }
    }
}