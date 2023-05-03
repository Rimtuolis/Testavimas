using Microsoft.AspNetCore.Mvc;
using PSA.Server.Services;
using PSA.Services;
using PSA.Shared;


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


        [HttpGet("deck")]
        public List<Card> GetDeck(){
            //Update database
            return _blackJackService.GetDeck();
        }

        [HttpGet("hit")]
        public List<Card> Hit()
        {
            //Update database
            return _blackJackService.Hit();
        }

        [HttpGet("hitdealer")]
        public List<Card> HitDealer()
        {
            //Update database
            return _blackJackService.HitDealer();
        }

        [HttpGet("playercards")]
        public List<Card> GetPlayerCards()
        {
            //Update database
            return _blackJackService.GetPlayerCards();
        }

        [HttpGet("dealercards")]
        public List<Card> GetDealerCards()
        {
            //Update database
            return _blackJackService.GetDealerCards();
        }

        [HttpGet("gamestate")]
        public bool GetGameState()
        {
            //Update database
            return _blackJackService.GetState();
        }

        [HttpPost("gamestate")]
        public void SetGameState([FromBody] bool state)
        {
            //Update database
            _blackJackService.SetState(state);
        }

        [HttpPost("resetdeck")]
        public void ResetDeck()
        {
            //Update database
            _blackJackService.ResetDeck();
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
