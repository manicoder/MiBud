using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xamarin.Forms;

namespace MiBud.Models
{
    /// <summary>
    /// Request Model
    /// </summary>
    public class UserModel
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string password { get; set; }
        public string device_type { get; set; }
        public string mac_id { get; set; }
        public ImageSource user_profile_pic { get; set; }
        public string pin_code { get; set; }
        public string rs_agent_id { get; set; }
        public string segment { get; set; }
        public int segment_id { get; set; }
        public string user_type { get; set; }
        public string role { get; set; }
        //public string first_name { get; set; }
        //public string last_name { get; set; }
        //public string email { get; set; }
        //public string mobile { get; set; }
        //public string password { get; set; }
        //public string device_type { get; set; }
        //public string mac_id { get; set; }
        //public ImageSource user_profile_pic { get; set; }
        ////public string pin_code { get; set; }
        //public string rs_agent_id { get; set; }
        //public List<int> user_type { get; set; } = new List<int>();
    }

    /// <summary>
    /// Response Model
    /// </summary>
    public class UserResponse
    {
        public string detail { get; set; }

        public string error { get; set; }
        public HttpStatusCode status_code { get; set; }
    }
}
