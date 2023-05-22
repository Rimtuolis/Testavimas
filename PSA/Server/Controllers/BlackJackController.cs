using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Digests;
using PSA.Server.Services;
using PSA.Services;
using PSA.Shared;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlackJackController : ControllerBase
    {

        private readonly IDatabaseOperationsService _databaseOperationsService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger _logger;
        //Should create a blackjack service for it not to load always
        //Or use database calls to push/pull data to them constantly
        private readonly IBlackJackService _blackJackService;

        public BlackJackController(IDatabaseOperationsService databaseOperationsService, ICurrentUserService currentUserService, ILogger<AuthController> logger, IBlackJackService blackJackService)
        {
            _databaseOperationsService = databaseOperationsService;
            _currentUserService = currentUserService;
            _logger = logger;
            _blackJackService = blackJackService;
        }

        [HttpGet("game")]
        public async Task<BlackJack?> getGameSettings()
        {
            // Error from db cannot take json and convert it to object, need to convert to object and only then it can be accessed, create a DTO with string values for cards
            BlackJackDTO? blackJackdto = await _databaseOperationsService.ReadItemAsync<BlackJackDTO>($"select * from blackjack where fk_user={_currentUserService.GetUser().Id} and gameState=True order by date desc limit 1");

            List<Card>? playerCards;
            List<Card>? dealerCards;
            List<Card>? deck;
            Card? hidden;
            BlackJack? blackJack;

            if (blackJackdto is not null 
                && blackJackdto.playerCards is not null 
                && blackJackdto.dealerCards is not null
                && blackJackdto.deck is not null 
                && blackJackdto.hiddenCard is not null)
            {
                playerCards = JsonSerializer.Deserialize<List<Card>>(blackJackdto.playerCards);

                dealerCards = JsonSerializer.Deserialize<List<Card>>(blackJackdto.dealerCards);

                deck = JsonSerializer.Deserialize<List<Card>>(blackJackdto.deck);

                hidden = JsonSerializer.Deserialize<Card>(blackJackdto.hiddenCard);
                    
                blackJack = new BlackJack(blackJackdto.Id, blackJackdto.betAmount, playerCards, dealerCards, deck, hidden, blackJackdto.tick, blackJackdto.gameState, blackJackdto.date, blackJackdto.fk_user);

                _blackJackService.SetService(blackJack);

                return blackJack;
            }

            deck = _blackJackService.GetDeck();
            playerCards = _blackJackService.GetPlayerCards();
            dealerCards = _blackJackService.GetDealerCards();
            double betAmount = _blackJackService.GetbetAmount();
            int tick = _blackJackService.GetTick();
            hidden = _blackJackService.GetHiddenCard();
            bool gameState = true;

            blackJack = new BlackJack(0, betAmount, playerCards, dealerCards, deck, hidden, tick, gameState, DateTime.UtcNow, _currentUserService.GetUser().Id);

            string jsonDeck = JsonSerializer.Serialize(deck);
            string jsonPlayerCards = JsonSerializer.Serialize(playerCards);
            string jsonDealerCards = JsonSerializer.Serialize(dealerCards);
            string jsonHidden = JsonSerializer.Serialize(hidden);
            await _databaseOperationsService.ExecuteAsync($"insert into blackjack(betAmount, playerCards, dealerCards, deck, hiddenCard, tick, gameState, fk_user) values({betAmount}, '{jsonPlayerCards}', '{jsonDealerCards}', " +
                $"'{jsonDeck}', '{jsonHidden}', {tick}, {gameState}, {_currentUserService.GetUser().Id})");
            int index = await _databaseOperationsService.ReadItemAsync<int>($"select Id from blackjack where fk_user={_currentUserService.GetUser().Id} and gameState=True order by date desc limit 1");
            blackJack.Id = index;

            return blackJack;
        }


        [HttpGet("deck")]
        public async Task<List<Card>?> GetDeck(){
            //Update database
            List<Card>? deck = _blackJackService.GetDeck();
            if (_blackJackService.GetState())
            {
                return deck;
            }

            bool? playing = await _databaseOperationsService.ReadItemAsync<bool?>($"select gameState from blackjack where fk_user={_currentUserService.GetUser().Id} order by date desc limit 1");

            if (playing is not null && playing == true)
            {
                deck = await _databaseOperationsService.ReadItemAsync<List<Card>>($"select deck from blackjack where fk_user={_currentUserService.GetUser().Id} order by date desc limit 1");
                string? json = await _databaseOperationsService.ReadItemAsync<string?>($"select playerCards from blackjack where fk_user={_currentUserService.GetUser().Id} order by date desc limit 1");
                List<Card>? playerCards;
                if (json is not null)
                {
                    playerCards = JsonSerializer.Deserialize<List<Card>>(json);
                }

                json = await _databaseOperationsService.ReadItemAsync<string?>($"select dealerCards from blackjack where fk_user={_currentUserService.GetUser().Id} order by date desc limit 1");
                List<Card>? dealerCards;
                if (json is not null)
                {
                    dealerCards = JsonSerializer.Deserialize<List<Card>>(json);
                }

                json = await _databaseOperationsService.ReadItemAsync<string?>($"select hiddenCard from blackjack where fk_user={_currentUserService.GetUser().Id} order by date desc limit 1");
                Card? hiddenCard;
                if (json is not null)
                {
                    hiddenCard = JsonSerializer.Deserialize<Card>(json);
                }

                int? tick = await _databaseOperationsService.ReadItemAsync<int>($"select tick from blackjack where fk_user={_currentUserService.GetUser().Id} order by date desc limit 1");

                //_blackJackService

            }
            else
            {
                string json = JsonSerializer.Serialize(deck);
                await _databaseOperationsService.ExecuteAsync($"insert into blackjack(deck, fk_user) values('{json}', {_currentUserService.GetUser().Id})");
            }
            return deck;
        }

        [HttpGet("hit/{id}")]
        public async Task<List<Card>> Hit(int id)
        {
            //Update database
            List<Card> playerCards = _blackJackService.Hit();
            string playerCardsJson = JsonSerializer.Serialize(playerCards);
            List<Card> deck = _blackJackService.GetDeck();
            string deckJson = JsonSerializer.Serialize(deck);
            int tick = _blackJackService.GetTick();

            await _databaseOperationsService.ExecuteAsync($"update blackjack set playerCards = '{playerCardsJson}', deck = '{deckJson}', tick = {tick} where Id={id}");

            return playerCards;
        }

        [HttpGet("hitdealer/{id}")]
        public async Task<List<Card>> HitDealer(int id)
        {
            //Update database
            List<Card> dealerCards = _blackJackService.HitDealer();
            string dealerCardsJson = JsonSerializer.Serialize(dealerCards);
            List<Card> deck = _blackJackService.GetDeck();
            string deckJson = JsonSerializer.Serialize(deck);
            int tick = _blackJackService.GetTick();

            await _databaseOperationsService.ExecuteAsync($"update blackjack set dealerCards = '{dealerCardsJson}', deck = '{deckJson}', tick = {tick} where Id={id}");

            return dealerCards;
        }

        [HttpGet("playercards")]
        public async Task<List<Card>?> GetPlayerCards()
        {
            //Update database
            List<Card>? playerCards = _blackJackService.GetPlayerCards();
            if (_blackJackService.GetState())
            {
                return playerCards;
            }

            bool? playing = await _databaseOperationsService.ReadItemAsync<bool?>($"select gameState from blackjack where fk_user={_currentUserService.GetUser().Id} order by date desc limit 1");

            if (playing is not null && playing == true)
            {
                playerCards = await _databaseOperationsService.ReadItemAsync<List<Card>>($"select playerCards from blackjack where fk_user={_currentUserService.GetUser().Id} order by date desc limit 1");
            }
            else
            {
                string json = JsonSerializer.Serialize(playerCards);
                await _databaseOperationsService.ExecuteAsync($"update blackjack set playerCards = '{json}' where fk_user={_currentUserService.GetUser().Id} and gameState=True order by date desc limit 1");
            }
            return playerCards;
        }

        [HttpGet("dealercards")]
        public async Task<List<Card>?> GetDealerCards()
        {
            //Update database
            List<Card>? dealerCards = _blackJackService.GetDealerCards();
            if (_blackJackService.GetState())
            {
                return dealerCards;
            }

            bool? playing = await _databaseOperationsService.ReadItemAsync<bool?>($"select gameState from blackjack where fk_user={_currentUserService.GetUser().Id} order by date desc limit 1");

            if (playing is not null && playing == true)
            {
                dealerCards = await _databaseOperationsService.ReadItemAsync<List<Card>>($"select dealerCards from blackjack where fk_user={_currentUserService.GetUser().Id} order by date desc limit 1");
            }
            else
            {
                string json = JsonSerializer.Serialize(dealerCards);
                await _databaseOperationsService.ExecuteAsync($"update blackjack set dealerCards = '{json}' where fk_user={_currentUserService.GetUser().Id} and gameState=True order by date desc limit 1");
            }
            return dealerCards;
        }

        [HttpGet("gamestate/{id}")]
        public async Task<bool> GetGameState(int id)
        {
            //Update database
            bool? playing = await _databaseOperationsService.ReadItemAsync<bool?>($"select gameState from blackjack where Id={id}");
            if (playing is not null)
                return (bool)playing;

            return false;
        }

        [HttpGet("gamestate")]
        public async Task<bool> GetGameState()
        {
            //Update database
            bool? playing = await _databaseOperationsService.ReadItemAsync<bool?>($"select gameState from blackjack where fk_user={_currentUserService.GetUser().Id} and gameState=True order by date desc limit 1");
            if (playing is not null)
                return (bool)playing;

            return false;
        }

        [HttpPost("gamestate/{id}")] 
        public async void SetGameState(int id, [FromBody] bool state)
        {
            //Update database
           await _databaseOperationsService.ExecuteAsync($"update blackjack set gameState={state} where Id={id}");

            _blackJackService.SetState(state);
        }

        [HttpGet("betamount")]
        public async Task<double?> GetbetAmount()
        {
            //Update database
            double? betAmount = await _databaseOperationsService.ReadItemAsync<double?>($"select betAmount from blackjack where fk_user={_currentUserService.GetUser().Id} and gameState=True order by date desc limit 1");
            
            if (betAmount is not null)
            {
                return betAmount;
            }

            return _blackJackService.GetbetAmount();
        }

        [HttpPost("betamount")]
        public void SetbetAmount([FromBody] double betAmount)
        {
            _blackJackService.SetbetAmount(betAmount);
        }

        [HttpPost("resetdeck/{id}")]
        public async void ResetDeck(int id, [FromBody] CurrentUser user)
        {
            //Update database
            _blackJackService.ResetDeck();

            await _databaseOperationsService.ExecuteAsync($"update blackjack set gameState=False where Id={id}");
            Thread.Sleep(1000);
            await _databaseOperationsService.ExecuteAsync($"update user set balance = {user.balance} where id_User = {id}");
            _currentUserService.SetUser(user);
        }

        // GET api/<BlackJackController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<BlackJackController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BlackJackController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BlackJackController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
