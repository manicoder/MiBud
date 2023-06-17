using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MiBud.Models
{
    public class JobcardModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<JobcardResult> results { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
    }

    public class JobcardResult : INotifyPropertyChanged
    {
        public string id { get; set; }
        public string job_card_name { get; set; }
        public string status { get; set; }
        public RegistrationNo registration_no { get; set; }
        public Workshop workshop { get; set; }
        public UserType user_type { get; set; }
        public CreatedBy created_by { get; set; }
        public List<JobcardSymptom> jobcard_symptom { get; set; }
        public List<JobcardService> jobcard_service { get; set; }
        public List<JobcardSparepart> jobcard_sparepart { get; set; }
        public List<JobcardPickupdrop> jobcard_pickupdrop { get; set; }
        public List<object> jobcard_entryexit { get; set; }
        public string pickup { get; set; }
        public string drop { get; set; }
        public string service_type { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

    }

    public class JobcardSymptom
    {
        public int id { get; set; }
        public string job_card { get; set; }
        public symptom symptom { get; set; }
        public string customer_check { get; set; }
        public string service_check { get; set; }
        public string entry_check { get; set; }
        public string exit_check { get; set; }
        public string status { get; set; }
    }

    public class JobcardService
    {
        public string id { get; set; }
        public string job_card { get; set; }
        public Service service { get; set; }
        public int quantity { get; set; }
        public string customer_check { get; set; }
        public string service_check { get; set; }
        public string entry_check { get; set; }
        public string exit_check { get; set; }
        public string status { get; set; }
        public int unit_price { get; set; }
    }

    public class JobcardSparepart
    {
        public string id { get; set; }
        public string job_card { get; set; }
        public Sparepart sparepart { get; set; }
        public int quantity { get; set; }
        public string customer_check { get; set; }
        public string service_check { get; set; }
        public string entry_check { get; set; }
        public string exit_check { get; set; }
        public string status { get; set; }
        public int unit_price { get; set; }
    }

    public class Service
    {
        public int id { get; set; }
        public string service_name { get; set; }
        public string is_active { get; set; }
    }

    public class Sparepart
    {
        public int id { get; set; }
        public string sparepart_no { get; set; }
    }


    public class symptom
    {
        public int id { get; set; }
        public string symptom_name { get; set; }
    }

    public class CreatedBy
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }

    public class RegistrationNo
    {
        public string id { get; set; }
        public string registration_id { get; set; }
        public string picture { get; set; }
    }

    public class UserType
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class JobcardNumber
    {
        public bool success { get; set; }
        public string name { get; set; }
    }

    #region [Create Jobcard Model]
    public class CreateJobcardModel
    {
        public string job_card_name { get; set; }
        public string status { get; set; }
        public string registration_no { get; set; }
        public int workshop { get; set; }
        public string user_type { get; set; }
        public string created_by { get; set; }
        public string pickup { get; set; }
        public string drop { get; set; }
        public string service_type { get; set; }

        public List<JobcardSymptomModel> jobcard_symptom { get; set; }
        public List<JobcardPickupdrop> jobcard_pickupdrop { get; set; }
    }

    public class JobcardSymptomModel
    {
        public int symptom { get; set; }
        public string customer_check { get; set; }
        public string service_check { get; set; }
        public string entry_check { get; set; }
        public string exit_check { get; set; }
        public string status { get; set; }
    }

    public class JobcardPickupdrop
    {
        public string type { get; set; }
        public string technician { get; set; }
        public string address { get; set; }
        public string latlong { get; set; }
        public string panned_time { get; set; }
        public string actual_time { get; set; }
        public string status { get; set; }
    }

    public class CreateJobcardResponse
    {
        public string job_card_name_id { get; set; }
        public string job_card_name { get; set; }
        public string registration_no { get; set; }
        public int workshop { get; set; }
        public int user_type { get; set; }
        public string created_by { get; set; }
        public string service_type { get; set; }
        public bool success { get; set; }
        public string error { get; set; }

    }
    #endregion
}
