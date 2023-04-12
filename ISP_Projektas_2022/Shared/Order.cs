using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Projektas_2022.Shared
{
    public class Order
    {
        public int id_Uzsakymas { get; set; }
        public double suma { get; set; }
        public DateTime data { get; set; }
        public OrderState busena { get; set; }
        public int fk_Klientasid_Klientas { get; set; }
        public int fk_sandelininkas { get; set; }
    }
}