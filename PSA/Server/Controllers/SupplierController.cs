using Microsoft.AspNetCore.Mvc;
using PSA.Services;
using PSA.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ILogger<SupplierController> _logger;
        private readonly IDatabaseOperationsService _databaseOperationsService;

        public SupplierController(ILogger<SupplierController> logger, IDatabaseOperationsService databaseOperationsService)
        {
            _logger = logger;
            _databaseOperationsService = databaseOperationsService;
        }

        // GET: api/<SupplierController>
        [HttpGet]
        public async Task<IEnumerable<Supplier>> Get()
        {
            return await _databaseOperationsService.ReadListAsync<Supplier>($"select * from tiekejas");
        }

        // Gets supplier by ID
        // GET api/<SupplierController>/5
        [HttpGet("get/{id}")]
        public async Task<Supplier?> Get(int id)
        {
            return await _databaseOperationsService.ReadItemAsync<Supplier>($"select * from tiekejas where id_Tiekejas = {id}");
        }

        [HttpPost]
        public async Task Post([FromBody] Supplier supplier)
        {
            var index = await _databaseOperationsService.ReadItemAsync<int?>($"select max(id_Tiekejas) form tiekejas");
            index++;
            await _databaseOperationsService.ExecuteAsync($"insert into tiekejas(pavadinimas, el_pastas, slaptazodis, tel_nr, atstovas, miestas, sritis, " +
                $"id_Tiekejas) values({supplier.pavadinimas}, {supplier.el_pastas}, {supplier.slaptazodis}, {supplier.tel_nr}, {supplier.atstovas}, " +
                $"{supplier.miestas}, {supplier.sritis}, {index})");
        }

        // PUT api/<SupplierController>/5
        [HttpPut]
        public async Task Put([FromBody] Supplier supplier)
        {
            await _databaseOperationsService.ExecuteAsync($"update 'tiekejas' " +
                $"set 'pavadinimas' = {supplier.pavadinimas}, 'el_pastas' = {supplier.el_pastas}, 'slaptazodis' = {supplier.slaptazodis}, " +
                $"'tel_nr' = {supplier.tel_nr}, 'atstovas' = {supplier.atstovas}, 'miestas' = {supplier.miestas}, 'sritis' = {supplier.sritis} where 'id_Tiekejas' = {supplier.id_Tiekejas}");
        }

        // DELETE api/<SupplierController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _databaseOperationsService.ExecuteAsync($"delete from 'preke' where 'fk_Tiekejasid_Tiekejas' = {id}");
            await _databaseOperationsService.ExecuteAsync($"delete from 'tiekejas' where 'id_Tiekejas' = {id}");
        }
    }
}