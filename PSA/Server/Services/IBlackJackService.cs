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
        void ResetDeck();
    }
}
