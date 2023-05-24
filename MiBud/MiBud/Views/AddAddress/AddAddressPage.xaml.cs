using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.AddAddress
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAddressPage : ContentPage
    {
        string[] vs;
        string address_type;
        public AddAddressPage(string address_type)
        {
            InitializeComponent();
            this.address_type = address_type;  
            vs = new string[3];
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            
        }


        public void GetCenterLatLong()
        {
            try
            {
                Position position = new Position(22.6949509, 75.8894909);
                Pin pin = new Pin
                {
                    Label = "Santa Cruz",
                    Address = "The city with a boardwalk",
                    Type = PinType.Place,
                    Position = position
                };
                map.Pins.Add(pin);
                MapSpan mapSpan = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(3));
                map.MoveToRegion(mapSpan);
            }
            catch (Exception ex)
            {
            }
        }

        private async void map_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                int i = 0;

                //vs[0] = Convert.ToString(Math.Round(Convert.ToDouble(map.VisibleRegion.Center.Latitude),4));
                //vs[1] = Convert.ToString(Math.Round(Convert.ToDouble(map.VisibleRegion.Center.Longitude), 4));

                vs[0] = map.VisibleRegion.Center.Latitude.ToString("#.####");
                vs[1] = map.VisibleRegion.Center.Longitude.ToString("#.####");
                //changingLat.Text = Convert.ToString(Math.Round(Convert.ToDouble(map.VisibleRegion.Center.Latitude), 4));
                //changingLong.Text = Convert.ToString(Math.Round(Convert.ToDouble(map.VisibleRegion.Center.Longitude), 4));

                //var placemarks = await Geocoding.GetPlacemarksAsync(map.VisibleRegion.Center.Latitude, map.VisibleRegion.Center.Longitude);

                //var placemark = placemarks?.FirstOrDefault();
                //var json = JsonConvert.SerializeObject(placemarks);
                //Debug.WriteLine(json, "Address Location");
                //if (placemark != null)
                //{

                //    aa.Text = $"Admin Area :  {placemark.AdminArea}";
                //    cc.Text = $"CountryCode : {placemark.CountryCode}";
                //    cn.Text = $"CountryName : {placemark.CountryName}";
                //    fn.Text = $"FeatureName : {placemark.FeatureName}";
                //    l.Text = $"Locality : {placemark.Locality}";
                //    pc.Text = $"PostalCode : {placemark.PostalCode}";
                //    saa.Text = $"SubAdminArea : {placemark.SubAdminArea}";
                //    sl.Text = $"SubLocality : {placemark.SubLocality}";
                //    stf.Text = $"SubThoroughfare : {placemark.SubThoroughfare}";
                //    tf.Text = $"Thoroughfare : {placemark.Thoroughfare}";
                //}

                //val.Text = Convert.ToString(i++);
            }
            catch (Exception ex)
            {
            }
        }

        private void map_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            try
            {
                int i = 0;
                //changingLat.Text = Convert.ToString(Math.Round(Convert.ToDouble(map.VisibleRegion.Center.Latitude), 4));
                //changingLong.Text = Convert.ToString(Math.Round(Convert.ToDouble(map.VisibleRegion.Center.Longitude), 4));
                //val1.Text = Convert.ToString(i++);
            }
            catch (Exception ex)
            {
            }
        }

        private void AddAddressClicked(object sender, EventArgs e)
        {
            vs[2] = txtAddress.Text;
            if(address_type == "PickupAddress")
            {
                MessagingCenter.Send<AddAddressPage, string[]>(this, "PickupAddress", vs);
            }
            else
            {
                MessagingCenter.Send<AddAddressPage, string[]>(this, "DropAddress", vs);
            }
            
            this.Navigation.PopAsync();
        }
    }
}