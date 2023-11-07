using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.Shared
{
    public class TournamentRobot
    {
        public int id { get; set; }
        public int fk_turnyras { get; set; }
        public int fk_robotas { get; set; }
        public int turi_kova { get; set; }
    }
}
