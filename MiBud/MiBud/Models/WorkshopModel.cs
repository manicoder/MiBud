using System;
using System.Collections.Generic;
using System.Text;

namespace MiBud.Models
{
    public class WorkshopModel
    {
        public string status { get; set; }
        public bool success { get; set; }
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<WorkshopResult> results { get; set; }
    }


    public class WorkshopResult
    {
        public int id { get; set; }
        public string name { get; set; }
        public Country country { get; set; }
        public bool status { get; set; }
        public Pincode pincode { get; set; }
        public string address { get; set; }
        public User user { get; set; }
        public string email { get; set; }
    }

    public class Country
    {
        public int id { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string name { get; set; }
        public object slug { get; set; }
        public object country_code { get; set; }
        public object currency_code { get; set; }
        public string is_active { get; set; }
        public List<int> user_type { get; set; }
    }

    public class Pincode
    {
        public int id { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string name { get; set; }
        public string area { get; set; }
        public object slug { get; set; }
        public string is_active { get; set; }
        public object state { get; set; }
        public int district { get; set; }
        public List<int> user_type { get; set; }
    }

    public class User
    {
        public string id { get; set; }
        public string email { get; set; }
        public object mobile { get; set; }
        public object first_name { get; set; }
        public object last_name { get; set; }
    }
}
