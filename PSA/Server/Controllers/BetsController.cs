using Microsoft.AspNetCore.Mvc;
using PSA.Services;
using PSA.Shared;


namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetsController : ControllerBase
    {
        private readonly ILogger<BetsController> _logger;
        private readonly IDatabaseOperationsService _databaseOperationsService;
        public BetsController(ILogger<BetsController> logger, IDatabaseOperationsService databaseOperationsService)
        {
            _logger = logger;
            _databaseOperationsService = databaseOperationsService;
        }

        // Gets all tournaments and retuns the list
        // GET: api/<TournamentsController>
        [HttpGet]
        public async Task<IEnumerable<Bet>> Get()
        {
            return await _databaseOperationsService.ReadListAsync<Bet>($"SELECT * FROM statymas");
        }
        // Gets tournament by ID
        // GET api/<TournamentsController>/5
        [HttpGet("{id}")]
        public async Task<Bet?> Get(int id)
        {
            return await _databaseOperationsService.ReadItemAsync<Bet?>($"SELECT * FROM statymas where Id = {id}");
        }

        // creates tournament in DB
        // POST api/<TournamentsController>
        [HttpPost]
        public async Task Create([FromBody] Bet bet)
        {
            await _databaseOperationsService.ExecuteAsync($"insert into statymas(Amount, Coefficient, fk_robot, fk_fight) values('{bet.Amount}', '{bet.Coefficient}', '{bet.fk_robot}', '{bet.fk_fight}')");
        }
        // Deletes tournament from DB by ID
        // DELETE api/<TournamentsController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _databaseOperationsService.ExecuteAsync($"delete from statymas where Id = {id}");
        }
    }
}
