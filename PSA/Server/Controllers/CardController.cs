using Microsoft.AspNetCore.Mvc;
using PSA.Client.Pages.Orders;
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
            return await _databaseOperationsService.ReadListAsync<SwipeCard>($"SELECT * FROM card where fk_robot not in (" +
                $"SELECT Id from robotas where fk_user_id = {_currentUserService.GetUser().Id}); ");
        }

        [HttpGet("element/{robotId}")]
        public async Task<SwipeCard> GetCard(int robotId)
        {
            long count = await _databaseOperationsService.ReadItemAsync<long>($"select COUNT(*) from card where fk_robot = {robotId}");
            if (count > 0) {
                return await _databaseOperationsService.ReadItemAsync<SwipeCard>($"select * from card where fk_robot = {robotId}");
			}
            var temp = new SwipeCard();
            temp.Description = "Empty";

			return temp;
            ///TODO 
            
        }

        [HttpPost]
        [Route("swipe/{id}")]
        public async Task<bool> SwipeCard(int id, [FromBody] SwipeCard card)
        {

            Console.WriteLine("ASDASDASDSA");
            long duplicate = await _databaseOperationsService.ReadItemAsync<long>($"select COUNT(*) from matches where fk_robot_first = {id} AND fk_robot_second = {card.fk_robot}");
			long index = await _databaseOperationsService.ReadItemAsync<long>($"select COUNT(*) from matches where fk_robot_first = {card.fk_robot} AND fk_robot_second = {id}");

            if (index == 0 && duplicate == 0)
            {
                await _databaseOperationsService.ExecuteAsync($"insert into matches (fk_robot_first, fk_robot_second) values({id}, {card.fk_robot})");
                return false;
            }
            else if (index == 1 && duplicate == 0)
            {
                await _databaseOperationsService.ExecuteAsync($"delete from matches where fk_robot_first = {card.fk_robot} AND fk_robot_second = {id}");
                return true;
            }
			return false;
		}
		[HttpPost]
		public async Task AddCard([FromBody] Robot robot)
		{
            Console.WriteLine($"{robot.Id} + {robot.Nickname}");
			string defaultString = "https://img.freepik.com/premium-vector/robot-silhouette-icon-illustration-template-many-purpose-isolated-white-background_625349-837.jpg";
			await _databaseOperationsService.ExecuteAsync($"insert into card (fk_robot, ImageUrl, Description) values({robot.Id}, '{defaultString}', '{robot.Nickname}')");

		}
		[HttpPut]
		public async Task EditCard([FromBody] SwipeCard card)
		{
			await _databaseOperationsService.ExecuteAsync($"update card set ImageUrl = '{card.ImageUrl}', Description = '{card.Description}' WHERE Id = {card.Id} ");

		}
    }
}
