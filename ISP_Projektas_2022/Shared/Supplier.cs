using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Projektas_2022.Shared
{
    public class Supplier
    {
        public int id_Tiekejas { get; set; }
        public string? pavadinimas { get; set; }
        public string? el_pastas { get; set; }
        public string? slaptazodis { get; set; }
        public string? tel_nr { get; set; }
        public string? atstovas { get; set; }
        public string? miestas { get; set; }
        public string? sritis { get; set; }

        public string? slapyvardis { get; set; }
    }
}