using Microsoft.AspNetCore.Mvc;
using PSA.Server.Services;
using PSA.Services;
using PSA.Shared;


namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentFightsController : ControllerBase
    {
        private readonly IDatabaseOperationsService _databaseOperationsService;
        private readonly ILogger<TournamentFightsController> _logger;
        // GET: TournamentFightsController
        public TournamentFightsController(IDatabaseOperationsService databaseOperationsService, ILogger<TournamentFightsController> logger)
        {
            _databaseOperationsService = databaseOperationsService;
            _logger = logger;
        }

        // GET: api/<TournamentsController>
        [HttpGet]
        public async Task<IEnumerable<TournamentFight>> Get()
        {
            return await _databaseOperationsService.ReadListAsync<TournamentFight>($"SELECT * FROM turnyro_kova");
        }

        // GET: api/<TournamentFightsController/5>
        [HttpGet("{id}")]
        public async Task<IEnumerable<TournamentFight>> Get(int id)
        {
            return await _databaseOperationsService.ReadListAsync<TournamentFight>($"select * from turnyro_kova where fk_turnyras = {id}");
        }

        [HttpGet("getlastfight/{id}")]
        public async Task<TournamentFight> GetLastFight(int id)
        {
            return await _databaseOperationsService.ReadItemAsync<TournamentFight>($"select * from turnyro_kova where fk_turnyras = {id} and id = (SELECT max(id) FROM turnyro_kova where fk_turnyras = {id})");
        }

        // POST api/<TournamentFightsController>
        [HttpPost]
        public async Task Post([FromBody] TournamentFight op)
        {
            await _databaseOperationsService.ExecuteAsync($"insert into turnyro_kova(fk_kova, fk_turnyras) values({op.fk_kova}, {op.fk_turnyras})");
        }
    }
}
