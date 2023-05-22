using Org.BouncyCastle.Crypto.Digests;
using PSA.Shared;

namespace PSA.Server.Services
{
    public class BlackJackService : IBlackJackService
    {
        public List<Card> deck;
        public List<Card> playerCards;
        public List<Card> dealerCards;
        public double betAmount;
        public bool Playing;
        public int tick;
        public Card hidden;
        public BlackJackService() {
            deck = new List<Card>(); 
            playerCards = new List<Card>();
            dealerCards = new List<Card>();
            betAmount = 0;
            Playing = false;
            tick = 0;
            hidden = new Card("Hidden", "", 0);
        }

        public List<Card> GetDeck()
        {
            if(deck.Count != 0)
                return deck;

            deck = Deck.GetDeck();
            deck = ShuffleDeck(deck);
            return deck;
        }

        public List<Card> ShuffleDeck(List<Card> deck)
        {
            deck = Deck.ShuffleDeck(deck);
            return deck;
        }

        public List<Card> Hit()
        {
            playerCards.Add(deck[0]);
            deck.RemoveAt(0);

            if (playerCards.Sum(card => card.value) > 21)
            {
                playerCards.ForEach(card =>
                {
                    if (playerCards.Sum((card) => card.value) > 21)
                    {
                        if (card.Rank == "A" && card.value != 1)
                        {
                            card.value = 1;
                        }
                    }
                });
            }

            tick++;
            return playerCards;
        }

        public List<Card> HitDealer()
        {
            if (dealerCards[0].Rank == "Hidden")
            {
                dealerCards[0] = hidden;
            }

            if (dealerCards.Sum(card => card.value) >= 17)
            {
                return dealerCards;
            }

            dealerCards.Add(deck[0]);
            deck.RemoveAt(0);

            if (dealerCards.Sum(card => card.value) > 21)
            {
                dealerCards.ForEach(card =>
                {
                    if (dealerCards.Sum((card) => card.value) > 21)
                    {
                        if (card.Rank == "A" && card.value != 1)
                        {
                            card.value = 1;
                        }
                    }
                });
            }

            return dealerCards;
        }

        public List<Card> GetPlayerCards()
        {
            if (playerCards.Count != 0)
                return playerCards;

            playerCards.Add(deck[1]);
            playerCards.Add(deck[3]);

            deck.RemoveAt(1);
            deck.RemoveAt(2);
            return playerCards;
        }

        public List<Card> GetDealerCards()
        {
            Console.WriteLine(tick);
            if (dealerCards.Count != 0) 
                return dealerCards;
            //CHECK the game tick and calculate to see if the cards needs to be hidden or not
            if (tick == 0)
            {
                if (deck[0].value + deck[1].value == 21)
                {
                    dealerCards.Add(deck[0]);
                    dealerCards.Add(deck[1]);
                } else if (playerCards.Sum(card => card.value) == 21)
                {
                    // If we hit 21 on the first try then we need to show the dealers hidden card
                    dealerCards.Add(deck[0]);
                    dealerCards.Add(deck[1]);
                }
                else
                {
                    dealerCards.Add(hidden);
                    dealerCards.Add(deck[1]);
                    hidden = deck[0];
                }
            }
            
            deck.RemoveAt(0);
            deck.RemoveAt(0);
            return dealerCards;
        }

        public bool GetState()
        {
            return Playing;
        }

        public void SetState(bool state)
        {
            Playing = state;
        }

        public double GetbetAmount()
        {
            return betAmount;
        }

        public void SetbetAmount(double amount)
        {
            betAmount = amount;
        }


        public void ResetDeck()
        {
            deck.Clear();
            playerCards.Clear();
            dealerCards.Clear();
            betAmount = 0;
            hidden = new Card("Hidden", "", 0);
            tick = 0;
            Playing = false;
        }

        public int GetTick()
        {
            return tick;
        }

        public void SetTick(int tick)
        {
            this.tick = tick;
        }

        public Card GetHiddenCard()
        {
            return hidden;
        }

        public void SetHiddenCard(Card hiddenCard)
        {
            this.hidden = hiddenCard;
        }

        void IBlackJackService.SetService(BlackJack blackJack)
        {
            if (blackJack is not null
                && blackJack.deck is not null
                && blackJack.playerCards is not null
                && blackJack.dealerCards is not null
                && blackJack.hiddenCard is not null)
            {
                this.deck = blackJack.deck;
                this.playerCards = blackJack.playerCards;
                this.dealerCards = blackJack.dealerCards;
                this.hidden = blackJack.hiddenCard;
                this.tick = blackJack.tick;
                this.betAmount = blackJack.betAmount;
                this.Playing = blackJack.gameState;
            }
    }
    }
}
