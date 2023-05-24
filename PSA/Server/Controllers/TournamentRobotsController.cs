using Microsoft.AspNetCore.Mvc;
using PSA.Server.Services;
using PSA.Services;
using PSA.Shared;


namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentRobotsController : ControllerBase
    {
        private readonly IDatabaseOperationsService _databaseOperationsService;
        private readonly ILogger<TournamentRobotsController> _logger;
        // GET: TournamentFightsController
        public TournamentRobotsController(IDatabaseOperationsService databaseOperationsService, ILogger<TournamentRobotsController> logger)
        {
            _databaseOperationsService = databaseOperationsService;
            _logger = logger;
        }

        // GET: api/<TournamentsController>
        [HttpGet]
        public async Task<IEnumerable<TournamentRobot>> Get()
        {
            return await _databaseOperationsService.ReadListAsync<TournamentRobot>($"SELECT * FROM turnyro_robotas");
        }

        // GET: api/<TournamentFightsController/5>
        [HttpGet("{id}")]
        public async Task<IEnumerable<TournamentRobot>> Get(int id)
        {
            return await _databaseOperationsService.ReadListAsync<TournamentRobot>($"select * from turnyro_robotas where fk_turnyras = {id}");
        }
        [HttpPut]
        public async Task Put([FromBody] TournamentRobot tournamentRobot)
        {
            await _databaseOperationsService.ExecuteAsync($"update turnyro_robotas set turi_kova = {tournamentRobot.turi_kova}, fk_turnyras = {tournamentRobot.fk_turnyras}, fk_robotas = {tournamentRobot.fk_robotas} where id = {tournamentRobot.id}");
        }


        [HttpGet("exists/{tournamentId}/{robotId}")]
        public async Task<bool> GetExisting(int robotId, int tournamentId)
        {
            var index = await _databaseOperationsService.ReadItemAsync<int?>($"select id from turnyro_robotas where fk_robotas = {robotId} && fk_turnyras = {tournamentId}");
            if (index == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        [HttpGet("existsUser/{tournamentId}")]
        public async Task<bool> GetExistingByUser(int tournamentId)
        {
            var index = await _databaseOperationsService.ReadItemAsync<int?>($"select robotas.id from robotas JOIN turnyro_robotas ON robotas.id = turnyro_robotas.fk_robotas && turnyro_robotas.fk_turnyras = {tournamentId}");
            if (index == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        [HttpGet("existsnofight/{id}")]
        public async Task<TournamentRobot> GetWithoutAFight(int id)
        {
            return await _databaseOperationsService.ReadItemAsync<TournamentRobot>($"select * from turnyro_robotas where turi_kova = 0 && fk_turnyras = {id}");
        }

        [HttpGet("getcountnofights/{id}")]
        public async Task<long> GetCountwithNoFights(int id)
        {
            return await _databaseOperationsService.ReadItemAsync<long>($"select count(id) from turnyro_robotas where fk_turnyras = {id} && turi_kova = 0");
        }
        [HttpGet("getcount/{id}")]
        public async Task<long> GetCount(int id)
        {
            return await _databaseOperationsService.ReadItemAsync<long>($"select count(id) from turnyro_robotas where fk_turnyras = {id}");
        }

        // POST api/<TournamentFightsController>
        [HttpPost]
        public async Task Post([FromBody] TournamentRobot op)
        {
            await _databaseOperationsService.ExecuteAsync($"insert into turnyro_robotas(fk_robotas, fk_turnyras, turi_kova) values({op.fk_robotas}, {op.fk_turnyras}, {op.turi_kova})");
        }
    }
}
