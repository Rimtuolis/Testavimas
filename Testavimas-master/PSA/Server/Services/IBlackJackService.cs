using PSA.Shared;

namespace PSA.Server.Services
{
    public interface IBlackJackService
    {
        List<Card> GetDeck();
        List<Card> Hit();
        List<Card> HitDealer();
        List<Card> GetPlayerCards();
        List<Card> GetDealerCards();
        bool GetState();
        void SetState(bool state);
        double GetbetAmount();
        void SetbetAmount(double amount);
        int GetTick();
        void SetTick(int tick);
        Card GetHiddenCard();
        void SetHiddenCard(Card hiddenCard);
        void ResetDeck();
        void SetService(BlackJack blackJack);
    }
}
