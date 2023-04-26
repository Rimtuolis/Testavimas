using Microsoft.AspNetCore.Mvc;
using PSA.Server.Services;
using PSA.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IDatabaseOperationsService _databaseOperationsService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ClientController> _logger;

        public ClientController(IDatabaseOperationsService databaseOperationsService, ICurrentUserService currentUserService, ILogger<ClientController> logger)
        {
            _databaseOperationsService = databaseOperationsService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        // GET: api/<ClientController>
        [HttpGet]
        public async Task<IEnumerable<Shared.Client>> Get()
        {
            return await _databaseOperationsService.ReadListAsync<Shared.Client>("select * from klientas");
        }

        // GET api/<ClientController>/5
        [HttpGet("{id}")]
        public async Task<Shared.Client?> Get(int id)
        {
            return await _databaseOperationsService.ReadItemAsync<Shared.Client>($"select * from klientas where id_Klientas = {id}");
        }

        // POST api/<ClientController>
        [HttpPost]
        public async Task Post([FromBody] Shared.Client client)
        {
            var index = await _databaseOperationsService.ReadItemAsync<int?>("select max(id_Klientas) from klientas");
            index++;
            await _databaseOperationsService.ExecuteAsync($"insert into klientas(vardas, pavarde, slapyvardis, " +
                $"slaptazodis, gimimo_data, miestas, el_pastas, pasto_kodas, adresas, id_Klientas) " +
                $"values({client.vardas}, {client.pavarde}, {client.slapyvardis}, {client.slaptazodis}, {client.gimimo_data}, {client.miestas}, " +
                $"{client.el_pastas}, {client.pasto_kodas}, {client.adresas}, {index})");
        }

        // DELETE api/<ClientController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _databaseOperationsService.ExecuteAsync($"delete from klientas where id_Klientas = {id}");
        }
    }
}