using System;
using System.Collections.Generic;
using System.Text;

namespace MiBud.Models
{
    public class ErroMsg
    {
        public int id { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public bool success { get; set; }
    }

}
