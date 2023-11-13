using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.Shared
{
    public class TournamentFight
    {
        public int id { get; set; }
        public int fk_turnyras { get; set; }
        public int fk_kova { get; set; }
    }
}
