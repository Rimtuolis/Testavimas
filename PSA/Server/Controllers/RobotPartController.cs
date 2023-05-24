using Microsoft.AspNetCore.Mvc;
using PSA.Server.Services;
using PSA.Services;
using PSA.Shared;

namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RobotPartController : ControllerBase
    {
        private readonly ILogger<RobotPartController> _logger;
        private readonly IDatabaseOperationsService _databaseOperationsService;
        private readonly ICurrentUserService _currentUserService;
        public RobotPartController(ILogger<RobotPartController> logger, IDatabaseOperationsService databaseOperationsService, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _databaseOperationsService = databaseOperationsService;
            _currentUserService = currentUserService;
        }

        // Gets all products and retuns the list
        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<IEnumerable<RobotPart>> Get()
        {
            return await _databaseOperationsService.ReadListAsync<RobotPart>($"SELECT * FROM roboto_detale");
        }

        [HttpGet("component/{getOponent}")]
        public async Task<List<Robot?>> GetOponent()
        {
            return await _databaseOperationsService.ReadListAsync<Robot?>($"SELECT * FROM robotas WHERE fk_user_id != {_currentUserService.GetUser().Id}");
        }

        /* [HttpPost]
         public async Task Create([FromBody] Fight fight)
         {
             await _databaseOperationsService.ExecuteAsync($"insert into kova(date, winner, state, fk_robot1, fk_robot2) values('{fight.date}', '1', '1', '{fight.fk_robot1}', '{fight.fk_robot2}')");
         }*/

        // DELETE api/<FightsController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _databaseOperationsService.ExecuteAsync($"delete from 'turnyro_kova' where 'fk_kova' = {id}");
            await _databaseOperationsService.ExecuteAsync($"delete from 'kova' where 'id' = {id}");
        }
    }
}
