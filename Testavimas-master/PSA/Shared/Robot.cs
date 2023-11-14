using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.Shared
{
    public class Robot
    {
        public string Nickname { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int fk_user_id { get; set; }

        public int Id { get; set; }
        public int Head { get; set; }
        public int LeftArm { get; set; }
        public int RightArm { get; set; }
        public int Body { get; set; }
        public int LeftLeg { get; set; }
        public int RightLeg { get; set; }
        public Robot()
        {
            this.Nickname = string.Empty;
        }

        public Robot(string nickname, int wins, int losses, int draws, int fk_user_id, int id)
        {
            Nickname = nickname;
            Wins = wins;
            Losses = losses;
            Draws = draws;
            this.fk_user_id = fk_user_id;
            Id = id;
        }
    }
}
