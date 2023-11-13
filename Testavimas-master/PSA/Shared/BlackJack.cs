using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.Shared
{
    public class BlackJack
    {
        public int Id { get; set; }
        public double betAmount { get; set; }
        public List<Card>? playerCards { get; set; }
        public List<Card>? dealerCards { get; set; }
        public List<Card>? deck { get; set; }
        public Card? hiddenCard { get; set; }
        public int tick { get;set; }
        public bool gameState { get; set; }
        public DateTime date { get; set; }
        public int fk_user { get; set; }

        public BlackJack()
        {
        }

        public BlackJack(int id, double betAmount, List<Card>? playerCards, List<Card>? dealerCards, List<Card>? deck, Card? hiddenCard, int tick, bool gameState, DateTime date, int fk_user)
        {
            Id = id;
            this.betAmount = betAmount;
            this.playerCards = playerCards;
            this.dealerCards = dealerCards;
            this.deck = deck;
            this.hiddenCard = hiddenCard;
            this.tick = tick;
            this.gameState = gameState;
            this.date = date;
            this.fk_user = fk_user;
        }
    }
}
