using Microsoft.AspNetCore.Mvc;
using PSA.Server.Services;
using PSA.Services;
using PSA.Shared;

namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RobotDtoController : ControllerBase
    {
        private readonly ILogger<RobotDtoController> _logger;
        private readonly IDatabaseOperationsService _databaseOperationsService;
        private readonly ICurrentUserService _currentUserService;
        public RobotDtoController(ILogger<RobotDtoController> logger, IDatabaseOperationsService databaseOperationsService, ICurrentUserService currentUserService)
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
            return await _databaseOperationsService.ReadListAsync<Robot>($"SELECT * FROM robotas WHERE fk_user_id = {_currentUserService.GetUser().Id}");
        }
        // Gets tournament by ID
        // GET api/<TournamentsController>/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<RobotPart?>> Get(int id)
        {
            return await _databaseOperationsService.ReadListAsync<RobotPart>($"SELECT * FROM roboto_detale where fk_robotas = {id}");
        }
    }
}