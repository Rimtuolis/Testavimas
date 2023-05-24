using Microsoft.AspNetCore.Mvc;
using PSA.Server.Services;						  
using PSA.Services;
using PSA.Shared;

namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RobotsController : ControllerBase
    {
        private readonly ILogger<RobotsController> _logger;
        private readonly IDatabaseOperationsService _databaseOperationsService;
		private readonly ICurrentUserService _currentUserService;									 
        public RobotsController(ILogger<RobotsController> logger, IDatabaseOperationsService databaseOperationsService, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _databaseOperationsService = databaseOperationsService;
			_currentUserService = currentUserService;									 
        }

        // Gets all tournaments and retuns the list
        // GET: api/<TournamentsController>
        [HttpGet]
        public async Task<IEnumerable<Robot>> Get()
        {
            return await _databaseOperationsService.ReadListAsync<Robot>($"SELECT * FROM robotas");
        }
        // Gets tournament by ID
        // GET api/<TournamentsController>/5
        [HttpGet("{id}")]
        public async Task<Robot?> Get(int id)
        {
            return await _databaseOperationsService.ReadItemAsync<Robot?>($"SELECT * FROM robotas where Id = {id}");
        }


		[HttpGet("component/mySelf")]
        public async Task<List<Robot?>> MySelf()
        {
            
            return await _databaseOperationsService.ReadListAsync<Robot?>($"SELECT * FROM robotas WHERE fk_user_id = {_currentUserService.GetUser().Id}");
        }

        [HttpGet("component/getOponent")]
        public async Task<List<Robot?>> GetOponent()
        {
            return await _databaseOperationsService.ReadListAsync<Robot?>($"SELECT * FROM robotas WHERE fk_user_id != {_currentUserService.GetUser().Id}");
        }
        // creates tournament in DB
        // POST api/<TournamentsController>
        [HttpPost]
        public async Task Create([FromBody] Robot robot)
        {
            await _databaseOperationsService.ExecuteAsync($"insert into robotas(Nickname, Wins, Losses, Draws) values('{robot.Nickname}', '{robot.Wins}', '{robot.Losses}', '{robot.Draws}')");
        }
        // Deletes tournament from DB by ID
        // DELETE api/<TournamentsController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _databaseOperationsService.ExecuteAsync($"delete from robotas where Id = {id}");
        }
    }
}
