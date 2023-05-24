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
        [HttpGet("parts")]
        public async Task<IEnumerable<Robot>> GetParts()
        {
            return await _databaseOperationsService.ReadListAsync<Robot>($"SELECT * FROM preke");
        }
        //[HttpGet("{category}")]
        //public async Task<IEnumerable<Product>> Get(string category)
        //{
        //    return await _databaseOperationsService.ReadListAsync<Product>($"SELECT * FROM preke WHERE Category={category}");
        //}
        // Gets product by ID
        // GET api/<ProductsController>/5
        [HttpGet("parts/{id}")]
        public async Task<Product?> GetPartsW(int id)
        {
            //return await _databaseOperationsService.ReadItemAsync<Product?>($"SELECT * FROM preke where id_Preke = {id}");
            //return await _databaseOperationsService.ReadItemAsync<Product?>($"SELECT * FROM robotas JOIN roboto_detale ON {id} = roboto_detale.fk_robotas JOIN preke ON preke_id = roboto_detale.fk_preke");
            return await _databaseOperationsService.ReadItemAsync<Product?>($"SELECT * FROM preke WHERE id = {id}");
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
            var index = await _databaseOperationsService.ReadItemAsync<int?>("select max(Id) from robotas");
            index++;
            if (index == null) { index = 0; }

            await _databaseOperationsService.ExecuteAsync($"INSERT INTO `robotas`(`Nickname`, `Wins`, `Losses`, `Draws`, `Id`, `fk_user_id`) values ('{robot.Nickname}',{index}, {0}, {0}, {0}, {_currentUserService.GetUser().Id})");


            await _databaseOperationsService.ExecuteAsync($"insert into " +
                 $"roboto_detale(durability, fk_preke_id, fk_robotas)" +
                 $"values({100}, {robot.Head},{index})");
            await _databaseOperationsService.ExecuteAsync($"insert into " +
                 $"roboto_detale(durability, fk_preke_id, fk_robotas)" +
                 $"values({100}, {robot.Body},{index})");
            await _databaseOperationsService.ExecuteAsync($"insert into " +
                 $"roboto_detale(durability, fk_preke_id, fk_robotas)" +
                 $"values({100}, {robot.RightArm},{index})");
            await _databaseOperationsService.ExecuteAsync($"insert into " +
                 $"roboto_detale(durability, fk_preke_id, fk_robotas)" +
                 $"values({100}, {robot.LeftArm},{index})");
            await _databaseOperationsService.ExecuteAsync($"insert into " +
                 $"roboto_detale(durability, fk_preke_id, fk_robotas)" +
                 $"values({100}, {robot.RightLeg},{index})");
            await _databaseOperationsService.ExecuteAsync($"insert into " +
                 $"roboto_detale(durability, fk_preke_id, fk_robotas)" +
                 $"values({100}, {robot.LeftLeg},{index})");
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
