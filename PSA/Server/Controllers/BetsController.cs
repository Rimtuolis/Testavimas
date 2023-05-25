using Microsoft.AspNetCore.Mvc;
using PSA.Server.Services;
using PSA.Services;
using PSA.Shared;


namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetsController : ControllerBase
    {
        private readonly ILogger<BetsController> _logger;
		private readonly ICurrentUserService _currentUserService;
		private readonly IDatabaseOperationsService _databaseOperationsService;
        public BetsController(ILogger<BetsController> logger, ICurrentUserService currentUserService, IDatabaseOperationsService databaseOperationsService)
        {
            _logger = logger;
            _databaseOperationsService = databaseOperationsService;
			_currentUserService = currentUserService;

		}

        // Gets all tournaments and retuns the list
        // GET: api/<TournamentsController>
        [HttpGet]
        public async Task<IEnumerable<Bet>> Get()
        {
            return await _databaseOperationsService.ReadListAsync<Bet>($"SELECT * FROM statymas where fk_user_id = {_currentUserService.GetUser().Id} and state = 1");
        }
		[HttpGet("/history")]
		public async Task<IEnumerable<Bet>> GetHistory()
		{
			return await _databaseOperationsService.ReadListAsync<Bet>($"SELECT * FROM statymas where fk_user_id = {_currentUserService.GetUser().Id} and state = 2");
		}
		// Gets tournament by ID
		// GET api/<TournamentsController>/5
		[HttpGet("{id}")]
        public async Task<Bet?> Get(int id)
        {
            return await _databaseOperationsService.ReadItemAsync<Bet?>($"SELECT * FROM statymas where Id = {id}");
        }
		[HttpGet("active")]
		public async Task<List<Bet>> GetActiveAllBets()
		{
			return await _databaseOperationsService.ReadListAsync<Bet>($"SELECT * FROM statymas where state = 1");
		}

		[HttpPut("balance")]
		public async void UpdateBalance([FromBody] CurrentUser user)
		{

			await _databaseOperationsService.ExecuteAsync($"update user set balance = {user.balance} where id_User = {user.Id}");
			_currentUserService.SetUser(user);
		}
		// creates tournament in DB
		// POST api/<TournamentsController>
		[HttpPost]
        public async Task Create([FromBody] Bet bet)
        {
            await _databaseOperationsService.ExecuteAsync($"insert into statymas(Amount, Coefficient, fk_robot_id, fk_fight_id, fk_user_id, state ) values({bet.Amount}, {bet.Coefficient}, {bet.fk_robot_id}, {bet.fk_fight_id}, {bet.fk_user_id}, {bet.state})");
        }

    }
}
