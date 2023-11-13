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
		public double Coefficient { get; set; }
		public int fk_robot_id { get; set; }
		public int fk_fight_id { get; set; }
		public int fk_user_id { get; set; }
		public int state { get; set; }
	}
}
