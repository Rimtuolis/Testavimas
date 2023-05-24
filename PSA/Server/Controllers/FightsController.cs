using Microsoft.AspNetCore.Mvc;
using PSA.Server.Services;
using PSA.Services;
using PSA.Shared;

namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FightsController : ControllerBase
    {
        private readonly ILogger<FightsController> _logger;
        private readonly IDatabaseOperationsService _databaseOperationsService;
        private readonly ICurrentUserService _currentUserService;
        public FightsController(ILogger<FightsController> logger, IDatabaseOperationsService databaseOperationsService, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _databaseOperationsService = databaseOperationsService;
            _currentUserService = currentUserService;
		}

        // GET: api/<FightsController>
        [HttpGet("view/{robotId}")]
        public async Task<IEnumerable<Fight>> GetRobotsFights(int robotId)
        {
            return await _databaseOperationsService.ReadListAsync<Fight>($"select * from kova where fk_robot1 = {robotId} or fk_robot2 = {robotId}");
        }

        [HttpGet("maxid")]
        public async Task<int?> GetMaxId()
        {
            return await _databaseOperationsService.ReadItemAsync<int?>($"select max(id) from kova");
        }

        // Gets supplier by ID
        // GET api/<FightsController>/5
        [HttpGet("{id}")]
        public async Task<Fight?> Get(int id)
        {
            Console.WriteLine("NIG");
            return await _databaseOperationsService.ReadItemAsync<Fight?>($"SELECT * FROM kova where id = {id}");
        }
        [HttpPut]
        public async Task Put([FromBody] Fight fight)
        {
            await _databaseOperationsService.ExecuteAsync($"update kova set state = {fight.state}, winner = {fight.winner} where id = {fight.id}");
        }

        [HttpPost]
        public async Task Create([FromBody] Fight fight)
        {
			string myDateTimeString = fight.date.ToString("yyyy-MM-dd hh:mm:ss");
            await _databaseOperationsService.ExecuteAsync($"insert into kova(date, winner, state, fk_robot1, fk_robot2) values('{myDateTimeString}',0, 1, '{fight.fk_robot1}', '{fight.fk_robot2}')");
        }
		[HttpPut("{id}")]
        public async Task Update(int id)
        {
            Console.WriteLine("Pirmas");
            await _databaseOperationsService.ExecuteAsync($"update kova set state = 2 WHERE id = {id}");
        }

        [HttpPut("no/{id}")]
        public async Task Update2(int id)
        {
            Console.WriteLine("Antras");

            await _databaseOperationsService.ExecuteAsync($"update kova set state = 3 WHERE id = {id}");
        }
        [HttpPut("win/mhm/")]
        public async Task Update3([FromBody] Fight fight)
        {
            Console.WriteLine("Trecias");

            await _databaseOperationsService.ExecuteAsync($"update kova set state = 3, winner = {fight.winner} WHERE id = {fight.id}");
        }
        // DELETE api/<FightsController>/5
        // DELETE api/<FightsController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            //await _databaseOperationsService.ExecuteAsync($"delete from 'turnyro_kova' where 'fk_kova' = {id}");
            await _databaseOperationsService.ExecuteAsync($"DELETE FROM kova WHERE id={id}");
        }
    }
}
