using PSA.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.Shared
{
    public class Deck
    {
        private static string[] ranks = new[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        private static string[] suits = new[] { "Clubs", "Diamonds", "Hearts", "Spades" };
        private static List<Card> deck = new List<Card>();


        public static List<Card> GetDeck()
        {
            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    deck.Add(new Card(rank, suit));
                }
            }

            return deck;
        }

        public static List<Card> ShuffleDeck(List<Card> deck)
        {
            var random = new Random();

            for (int i = deck.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                var temp = deck[i];
                deck[i] = deck[j];
                deck[j] = temp;
            }

            return deck;
        }

    }
}
