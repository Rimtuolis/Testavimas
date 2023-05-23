using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.Shared
{
    public class BlackJackDTO
    {
        public int Id { get; set; }
        public double betAmount { get; set; }
        public string? playerCards { get; set; }
        public string? dealerCards { get; set; }
        public string? deck { get; set; }
        public string? hiddenCard { get; set; }
        public int tick { get; set; }
        public bool gameState { get; set; }
        public DateTime date { get; set; }
        public int fk_user { get; set; }
    }
}
