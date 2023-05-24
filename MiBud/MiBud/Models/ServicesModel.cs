using System;
using System.Collections.Generic;
using System.Text;

namespace MiBud.Models
{
    public class ServicesModel
    {
        public string status { get; set; }
        public bool success { get; set; }
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<ServicesResult> results { get; set; }
    }

    public class ServicesResult
    {
        public int oem { get; set; }
        public int model { get; set; }
        public int s_model { get; set; }
        public int year { get; set; }
        public List<ServiceModel> service_model { get; set; }
    }

    public class ServiceModel
    {
        public ServiceType service_type { get; set; }
        public Service service { get; set; }
        public string charge { get; set; }
        public string is_active { get; set; }
    }

    public class ServiceType
    {
        public string service_category { get; set; }
        public string is_active { get; set; }
    }

    //public class Service
    //{
    //    public string service_name { get; set; }
    //    public string is_active { get; set; }
    //}

    
}
