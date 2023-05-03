using PSA.Shared;

namespace PSA.Server.Services
{
    public class BlackJackService : IBlackJackService
    {
        public List<Card> deck;
        public List<Card> playerCards;
        public List<Card> dealerCards;
        public bool State;
        public BlackJackService() {
            deck = new List<Card>(); 
            playerCards = new List<Card>();
            dealerCards = new List<Card>();
            State = false;
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
            return playerCards;
        }

        public List<Card> HitDealer()
        {
            dealerCards.Add(deck[0]);
            deck.RemoveAt(0);
            return dealerCards;
        }

        public List<Card> GetPlayerCards()
        {
            if(playerCards.Count != 0)
                return playerCards;
            playerCards.Add(deck[0]);
            playerCards.Add(deck[2]);
            deck.RemoveAt(0);
            deck.RemoveAt(1);
            return playerCards;
        }

        public List<Card> GetDealerCards()
        {
            if (dealerCards.Count != 0) 
                return dealerCards;
            dealerCards.Add(deck[0]);
            dealerCards.Add(deck[1]);
            deck.RemoveAt(0);
            deck.RemoveAt(0);
            return dealerCards;
        }

        public bool GetState()
        {
            return State;
        }

        public void SetState(bool state)
        {
            State = state ? true : false;
        }

        public void ResetDeck()
        {
            deck.Clear();
            playerCards.Clear();
            dealerCards.Clear();
        }
    }
}
