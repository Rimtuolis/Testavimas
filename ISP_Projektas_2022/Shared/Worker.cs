using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Projektas_2022.Shared
{
    public class Worker
    {
        public int id_Sandelinkas { get; set; }
        public string? vardas { get; set; }
        public string? pavarde { get; set; }
        public string? slaptazodis { get; set; }
        public string? el_pastas { get; set; }
        public DateTime isidarbinimo_data { get; set; }
        public string? tel_nr { get; set; }

        public string? slapyvardis { get; set; }
    }
}