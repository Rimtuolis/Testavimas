using Microsoft.AspNetCore.Mvc;
using ISP_Projektas_2022.Services;
using ISP_Projektas_2022.Shared;

namespace ISP_Projektas_2022.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IDatabaseOperationsService _databaseOperationsService;

        public ProductsController(ILogger<ProductsController> logger, IDatabaseOperationsService databaseOperationsService)
        {
            _logger = logger;
            _databaseOperationsService = databaseOperationsService;
        }

        // Gets all products and retuns the list
        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _databaseOperationsService.ReadListAsync<Product>($"SELECT * FROM preke");
        }

        // Gets product by ID
        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public async Task<Product?> Get(int id)
        {
            return await _databaseOperationsService.ReadItemAsync<Product?>($"SELECT * FROM preke where id_Preke = {id}");
        }

        // creates product in DB
        // POST api/<ProductsController>
        [HttpPost]
        public async Task Create([FromBody] Product product)
        {
            var index = await _databaseOperationsService.ReadItemAsync<int?>("select max(id_Preke) from preke");
            index++;
            await _databaseOperationsService.ExecuteAsync($"insert into " +
                $"preke(pavadinimas, pagaminimo_data, kaina, miestas, modelis, aprasymas, kiekis, " +
                $"gamintojas, kategorija, kokybe, nuotrauka, id_Preke, fk_Tiekejasid_Tiekejas) " +
                $"values({product.Pavadinimas}, {product.Pagaminimo_Data}, {product.Kaina}, {product.Miestas}, {product.Modelis}, " +
                $"{product.Aprasymas}, {product.Kiekis}, {product.Gamintojas}, {product.Kategorija}, {product.Kokybe}, {product.Nuotrauka}, " +
                $"{index}, {product.Fk_Tiekejasid_Tiekejas})");
        }

        // updates record of product in DB by ID
        // PATCH api/<ProductsController>/5
        [HttpPut]
        public async Task Update([FromBody] Product product)
        {
            await _databaseOperationsService.ExecuteAsync($"update preke " +
                $"set pavadinimas = {product.Pavadinimas}, pagaminimo_data = {product.Pagaminimo_Data.ToShortDateString()}, kaina = {product.Kaina}, " +
                $"aprasymas = {product.Aprasymas}, nuotrauka = {product.Nuotrauka} where id_Preke = {product.Id_Preke}");
        }

        // Deletes product from DB by ID
        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _databaseOperationsService.ExecuteAsync($"delete from preke where id_Preke = {id}");
        }

        // Gets product by ID
        // GET api/<ProductsController>/5
        [HttpGet("{id}/stats")]
        public async Task<ProductStats?> GetProductStats(int id)
        {
            return await _databaseOperationsService.ReadItemAsync<ProductStats>($"select * from statistika where id_Statistika = {id}");
        }
    }
}