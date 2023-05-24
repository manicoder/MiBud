using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MiBud.Models
{
    public class OemModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<OemResult> results { get; set; }
        public string error { get; set; }
        public HttpStatusCode status_code { get; set; }
    }

    public class OemResult
    {

        public int id { get; set; }
        public string name { get; set; }
        public Segment segment_name { get; set; }
        //public object model_year { get; set; }
        public VehicleOem oem { get; set; }
    }

    public class Segment
    {
        public int id { get; set; }
        public string segment_name { get; set; }
    }
}
