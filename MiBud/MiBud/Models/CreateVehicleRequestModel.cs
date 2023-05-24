using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xamarin.Forms;

namespace MiBud.Models
{
    public class CreateVehicleRequestModel
    {
        public string registration_id { get; set; }
        public string vin { get; set; }
        public int user_type { get; set; }
        public int segment { get; set; }
        public int vehicle_model { get; set; }
        public int sub_model { get; set; }
        public int model_year { get; set; }
        public ImageSource picture { get; set; }
        public string user { get; set; }
        public int oem { get; set; }
    }

    public class CreateVehicleResponseModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public string error { get; set; }
        public HttpStatusCode status_code { get; set; }
        public List<VehicleModelResult> results { get; set; }
    }
}
