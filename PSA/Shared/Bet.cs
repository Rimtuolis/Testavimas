using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.Shared
{
    public class Bet
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public int fk_robot { get; set; }
        public int fk_fight { get; set; }
        public int Coefficient { get; set; }
    }
}
