using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.Shared
{
    public class RobotDto
    {
        public string Nickname { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int fk_user_id { get; set; }

        public int Id { get; set; }

        public List<RobotPart> Parts { get; set; }

    }
}
