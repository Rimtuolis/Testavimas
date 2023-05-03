using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.Shared
{
    public class Worker
    {
        public int id_Sandelinkas { get; set; }
        public string? name { get; set; }
        public string? last_name { get; set; }
        public string? slaptazodis { get; set; }
        public string? email { get; set; }
        public DateTime isidarbinimo_data { get; set; }
        public string? tel_nr { get; set; }

        public string? nickname { get; set; }
    }
}