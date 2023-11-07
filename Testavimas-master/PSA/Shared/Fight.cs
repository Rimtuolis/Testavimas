using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.Shared
{
    public class Fight
    {
        
        public DateTime date { get; set; }
        public int winner { get; set; }
        public int id { get; set; }
        public int state { get; set; }
        public int fk_robot1 { get; set; }
        public int fk_robot2 { get; set; }

    }
}
