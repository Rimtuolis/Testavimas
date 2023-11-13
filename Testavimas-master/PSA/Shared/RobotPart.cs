using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.Shared
{
    public class RobotPart
    {
        public int Id { get; set; }
        public int Durability { get; set; }    
        public int fk_preke_id { get; set; }
        public int fk_robotas { get; set; }

     
    }
}
