using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.Shared
{
    public class Robot
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public int Wins { get; set; }
        public int Ties { get; set; }
        public int Loses { get; set; }
    }
}
