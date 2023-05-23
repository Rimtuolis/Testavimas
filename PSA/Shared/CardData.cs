using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.Shared
{
    public class CardData
    {
        public SwipeCard card { get; }
        public int robotID { get; }

        public CardData(SwipeCard card, int id)
        {
            this.card = card;
            robotID = id;
        }
    }
}
