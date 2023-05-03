using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.Shared
{
    public class Order
    {
        public int id_Uzsakymas { get; set; }
        public double suma { get; set; }
        public DateTime data { get; set; }
        public OrderState busena { get; set; }
        public int fk_Klientasid_User { get; set; }
        public int fk_sandelininkas { get; set; }
    }
}