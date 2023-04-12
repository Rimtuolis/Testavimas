using ISP_Projektas_2022.Services;
using ISP_Projektas_2022.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ISP_Projektas_2022.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly ILogger<ManagerController> _logger;
        private readonly IDatabaseOperationsService _databaseOperationsService;

        public ManagerController(ILogger<ManagerController> logger, IDatabaseOperationsService databaseOperationsService)
        {
            _logger = logger;
            _databaseOperationsService = databaseOperationsService;
        }

        // GET: api/<ManagerController>
        [HttpGet]
        public async Task<IEnumerable<Manager>> Get()
        {
            return await _databaseOperationsService.ReadListAsync<Manager>("select * from vadybininkas");
        }

        // GET api/<ManagerController>/5
        [HttpGet("{id}")]
        public async Task<Manager?> Get(int id)
        {
            return await _databaseOperationsService.ReadItemAsync<Manager>($"select * from vadybininkas where id_Vadybininkas = {id}");
        }

        // DELETE api/<ManagerController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _databaseOperationsService.ExecuteAsync($"DELETE FROM vadybininkas where id_Vadybininkas = {id}");
        }
    }
}