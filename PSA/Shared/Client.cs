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
        public int id_User { get; set; }
        public string? name { get; set; }
        public string? last_name { get; set; }
        public string? nickname { get; set; }
        public string? password { get; set; }
        public DateTime birthdate { get; set; }
        public string? city { get; set; }
        public string? email { get; set; }
        public int post_code { get; set; }
        public double balance { get; set; }
        public AccessLevelType role { get; set; }
    }
}