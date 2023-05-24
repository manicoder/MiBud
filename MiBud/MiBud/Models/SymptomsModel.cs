using System;
using System.Collections.Generic;
using System.Text;

namespace MiBud.Models
{
    public class SymptomsModel
    {
        public string status { get; set; }
        public bool success { get; set; }
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<Symptoms> results { get; set; }
    }

    public class Symptoms
    {
        public int id { get; set; }
        public object oem { get; set; }
        public int s_model { get; set; }
        public int year { get; set; }
        public List<SymptomRtModel> symptom_rt_model { get; set; }
    }

    public class SymptomType
    {
        public int id { get; set; }
        public string symptom_type { get; set; }
    }

    public class Symptom
    {
        public int id { get; set; }
        public string symptom_name { get; set; }
    }

    public class SymptomRtModel
    {
        public int id { get; set; }
        public SymptomType symptom_type { get; set; }
        public Symptom symptom { get; set; }
    }
}
