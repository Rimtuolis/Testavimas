using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.Shared
{
    public class Supplier
    {
        public int id_Tiekejas { get; set; }
        public string? pavadinimas { get; set; }
        public string? email { get; set; }
        public string? slaptazodis { get; set; }
        public string? tel_nr { get; set; }
        public string? atstovas { get; set; }
        public string? miestas { get; set; }
        public string? sritis { get; set; }

        public string? nickname { get; set; }
    }
}