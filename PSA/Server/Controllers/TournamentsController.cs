using Microsoft.AspNetCore.Mvc;
using PSA.Services;
using PSA.Shared;

namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        private readonly ILogger<TournamentsController> _logger;
        private readonly IDatabaseOperationsService _databaseOperationsService;
        public TournamentsController(ILogger<TournamentsController> logger, IDatabaseOperationsService databaseOperationsService)
        {
            _logger = logger;
            _databaseOperationsService = databaseOperationsService;
        }

        // Gets all tournaments and retuns the list
        // GET: api/<TournamentsController>
        [HttpGet]
        public async Task<IEnumerable<Tournament>> Get()
        {
            return await _databaseOperationsService.ReadListAsync<Tournament>($"SELECT * FROM turnyras");
        }
        // Gets tournament by ID
        // GET api/<TournamentsController>/5
        [HttpGet("{id}")]
        public async Task<Tournament?> Get(int id)
        {
            return await _databaseOperationsService.ReadItemAsync<Tournament?>($"SELECT * FROM turnyras where Id = {id}");
        }

        // creates tournament in DB
        // POST api/<TournamentsController>
        [HttpPost]
        public async Task Create([FromBody] Tournament tournament)
        {
            await _databaseOperationsService.ExecuteAsync($"insert into turnyras(Start_date, End_date, Prize, Organiser, Format, Name) values('{tournament.Start_date}', '{tournament.End_date}', {tournament.Prize}, '{tournament.Organiser}', '{tournament.Format}', '{tournament.Name}')");
        }
        // updates record of tournament in DB by ID
        // PATCH api/<TournamentsController>/5
        [HttpPut]
        public async Task Update([FromBody] Tournament tournament)
        {
            await _databaseOperationsService.ExecuteAsync($"update turnyras " +
                $"set Name = '{tournament.Name}', Prize = '{tournament.Prize}', Start_date = '{tournament.Start_date}'," +
                $"Organiser = '{tournament.Organiser}', Format = '{tournament.Format}', End_date = '{tournament.End_date}' where Id = {tournament.Id}");
        }
        // Deletes tournament from DB by ID
        // DELETE api/<TournamentsController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _databaseOperationsService.ExecuteAsync($"delete from turnyras where Id = {id}");
        }
    }
}
