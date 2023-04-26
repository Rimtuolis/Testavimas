using Microsoft.AspNetCore.Mvc;
using PSA.Services;
using PSA.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractsController : ControllerBase
    {
        private readonly ILogger<ContractsController> _logger;
        private readonly IDatabaseOperationsService _databaseOperationsService;

        public ContractsController(ILogger<ContractsController> logger, IDatabaseOperationsService databaseOperationsService)
        {
            _logger = logger;
            _databaseOperationsService = databaseOperationsService;
        }

        // GET: api/<ContractsController>
        [HttpGet]
        public async Task<IEnumerable<Contract>> Get()
        {
            return await _databaseOperationsService.ReadListAsync<Contract>("select * from sutartis");
        }

        [HttpGet("get/{id}")]
        public async Task<Contract?> Get(int id)
        {
            return await _databaseOperationsService.ReadItemAsync<Contract>($"select * from sutartis where id_Sutartis = {id}");
        }

        [HttpPost]
        public async Task Post([FromBody] Contract contract)
        {
            var index = await _databaseOperationsService.ReadItemAsync<int?>("select max(id_Sutartis) from sutartis");
            index++;
            //DATABASE ENTRY UZSAKYMASID IS NOT UNIQUE AT THE MOMENT
            await _databaseOperationsService.ExecuteAsync($"insert into sutartis(isdavimo_data, id_Sutartis, fk_Uzsakymasid_Uzsakymas, fk_Vadybininkasid_Vadybininkas) values(NOW(), {index}, {contract.fk_Uzsakymasid_Uzsakymas}, {contract.fk_Vadybininkasid_Vadybininkas})");
        }

        [HttpPut]
        public async Task Put([FromBody] Contract contract)
        {
            await _databaseOperationsService.ExecuteAsync($"update sutartis set isdavimo_data = NOW(), fk_Uzsakymasid_Uzsakymas = {contract.fk_Uzsakymasid_Uzsakymas}, fk_Vadybininkasid_Vadybininkas = {contract.fk_Vadybininkasid_Vadybininkas} where id_Sutartis = {contract.id_Sutartis}");
        }

        // DELETE api/<ContractsController>/5
        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            await _databaseOperationsService.ExecuteAsync($"delete from email_saskaita where fk_Sutartisid_Sutartis = {id}");
            await _databaseOperationsService.ExecuteAsync($"delete from sutartis where id_Sutartis = {id}");
        }
    }
}