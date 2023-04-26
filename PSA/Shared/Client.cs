using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.Shared
{
    public class Client
    {
        public int id_Klientas { get; set; }
        public string? vardas { get; set; }
        public string? pavarde { get; set; }
        public string? slapyvardis { get; set; }
        public string? slaptazodis { get; set; }
        public DateTime gimimo_data { get; set; }
        public string? miestas { get; set; }
        public string? el_pastas { get; set; }
        public int pasto_kodas { get; set; }
        public string? adresas { get; set; }
    }
}