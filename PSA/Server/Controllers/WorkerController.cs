using Microsoft.AspNetCore.Mvc;
using PSA.Server.Services;
using PSA.Services;
using PSA.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly IDatabaseOperationsService _databaseOperationsService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<WorkerController> _logger;

        public WorkerController(IDatabaseOperationsService databaseOperationsService, ICurrentUserService currentUserService, ILogger<WorkerController> logger)
        {
            _databaseOperationsService = databaseOperationsService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        // GET: api/<WorkerController>
        [HttpGet]
        public async Task<IEnumerable<Worker>> Get()
        {
            return await _databaseOperationsService.ReadListAsync<Worker>("select * from sandelinkas");
        }

        // GET api/<WorkerController>/5
        [HttpGet("{id}")]
        public async Task<Worker?> Get(int id)
        {
            return await _databaseOperationsService.ReadItemAsync<Worker>($"select * from sandelinkas where id_Sandelinkas = {id}");
        }

        // POST api/<WorkerController>
        [HttpPost]
        public async Task Post([FromBody] Worker worker)
        {
            var index = await _databaseOperationsService.ReadItemAsync<int?>("select max(id_Sandelinkas) from sandelinkas");
            index++;
            await _databaseOperationsService.ExecuteAsync($"insert into sandelinkas(vardas, pavarde, " +
                $"slaptazodis, isidarbinimo_data, tel_nr, el_pastas, id_Sandelinkas, slapyvardis) values(" +
                $"{worker.vardas}, {worker.pavarde}, {worker.slaptazodis}, {worker.isidarbinimo_data}, {worker.tel_nr}, " +
                $"{worker.el_pastas}, {index}, {worker.slapyvardis})");
        }

        // DELETE api/<WorkerController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _databaseOperationsService.ExecuteAsync($"delete from sandelinkas where id_Sandelinkas = {id}");
        }
    }
}