using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.Shared
{
    public class Card
    {
        public string Rank { get; }
        public string Suit { get; }
        public int value { get; set; }

        public Card(string rank, string suit, int value)
        {
            Rank = rank;
            Suit = suit;
            this.value = value;
        }

        public override string ToString()
        {
            return Rank + " " + Suit;
        }
    }
}
