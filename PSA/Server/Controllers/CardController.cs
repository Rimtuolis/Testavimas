using Microsoft.AspNetCore.Mvc;
using PSA.Server.Services;
using PSA.Services;
using PSA.Shared;

namespace PSA.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardController : Controller
    {
        private readonly IDatabaseOperationsService _databaseOperationsService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger _logger;

        public CardController(IDatabaseOperationsService databaseOperationsService, ICurrentUserService currentUserService, ILogger<AuthController> logger)
        {
            _databaseOperationsService = databaseOperationsService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<SwipeCard>> GetCards()
        {
            ///TODO 
            return await _databaseOperationsService.ReadListAsync<SwipeCard>($"select * from card");
        }

        [HttpGet("element/{robotId}")]
        public async Task<SwipeCard> GetCard(int robotId)
        {
            ///TODO 
            return await _databaseOperationsService.ReadItemAsync<SwipeCard>($"select * from card where fk_robot = {robotId}");
        }

        [HttpPost]
        [Route("swipe/{id}")]
        public async Task<bool> SwipeCard(int id, [FromBody] SwipeCard card)
        {
            
            var index = await _databaseOperationsService.ReadItemAsync<int?>($"select COUNT(*) from matches where fk_robot_first = {card.fk_robot} AND fk_robot_second = {id}");

            if (index == 0) {
                await _databaseOperationsService.ExecuteAsync($"insert into matches(fk_robot_first, fk_robot_second" +
                    $"values({id}, {card.fk_robot})");
                return false;
            }
            return true;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
