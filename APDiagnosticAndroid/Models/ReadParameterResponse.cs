﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APDiagnosticAndroid.Models
{
    public class ReadParameterResponse
    {
        public string Status { get; set; }
        public byte[] DataArray { get; set; }
        public int pidNumber { get; set; }
        public string pidName { get; set; }
        public string responseValue { get; set; }
    }
}
