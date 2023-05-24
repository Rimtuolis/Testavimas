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
            return await _databaseOperationsService.ReadListAsync<Shared.Client>("select * from User");
        }

        // GET api/<ClientController>/5
        [HttpGet("{id}")]
        public async Task<Shared.Client?> Get(int id)
        {
            return await _databaseOperationsService.ReadItemAsync<Shared.Client>($"select * from User where id_User = {id}");
        }

        // POST api/<ClientController>
        [HttpPost]
        public async Task Post([FromBody] Shared.Client client)
        {
            var index = await _databaseOperationsService.ReadItemAsync<int?>("select max(id_User) from klientas");
            index++;
            await _databaseOperationsService.ExecuteAsync($"insert into klientas(name, last_name, nickname, " +
                $"slaptazodis, gimimo_data, miestas, email, pasto_kodas, id_User) " +
                $"values({client.name}, {client.last_name}, {client.nickname}, {client.password}, {client.birthdate}, {client.city}, " +
                $"{client.email}, {client.post_code}, {index})");
        }

        // DELETE api/<ClientController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _databaseOperationsService.ExecuteAsync($"delete from klientas where id_User = {id}");
        }
    }
}