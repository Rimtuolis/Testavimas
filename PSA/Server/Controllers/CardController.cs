using Microsoft.AspNetCore.Mvc;
using PSA.Services;
using PSA.Shared;

namespace PSA.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardController : Controller
    {
        private readonly IDatabaseOperationsService _databaseOperationsService;

        public CardController(IDatabaseOperationsService databaseOperationsService)
        {
            _databaseOperationsService = databaseOperationsService;
        }

        [HttpGet]
        public async Task<IEnumerable<SwipeCard>> GetCards()
        {
            ///TODO 
            return await _databaseOperationsService.ReadListAsync<SwipeCard>($"select * from card");
        }

        [HttpPost]
        [Route("swipe/{card}")]
        public ActionResult SwipeCard([FromQuery] bool liked)
        {
            if (liked)
            {
                var index = await _databaseOperationsService.ReadItemAsync<int?>("select max(id_User) from klientas");
                index++;
                await _databaseOperationsService.ExecuteAsync($"insert into klientas(name, last_name, nickname, " +
                    $"slaptazodis, gimimo_data, miestas, email, pasto_kodas, id_User) " +
                    $"values({client.name}, {client.last_name}, {client.nickname}, {client.password}, {client.birthdate}, {client.city}, " +
                    $"{client.email}, {client.post_code}, {index})");
                return Ok("ITS A MATCH");
            }

            return Ok();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
