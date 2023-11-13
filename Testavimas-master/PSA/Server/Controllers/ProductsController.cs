using Microsoft.AspNetCore.Mvc;
using PSA.Services;
using PSA.Shared;

namespace PSA.Server.Controllers
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
            //return await _databaseOperationsService.ReadItemAsync<Product?>($"SELECT * FROM preke where id_Preke = {id}");
            return await _databaseOperationsService.ReadItemAsync<Product?>($"SELECT * FROM preke where Id = {id}");
        }
        [HttpGet("get/{category}")]
        public async Task<IEnumerable<Product>> Get(string category)
        {
            return await _databaseOperationsService.ReadListAsync<Product>($"SELECT * FROM preke WHERE `Category`={category}");
        }
        // creates product in DB
        // POST api/<ProductsController>
        [HttpPost]
        public async Task Create([FromBody] Product product)
        {
            //var index = await _databaseOperationsService.ReadItemAsync<int?>("select max(id_Preke) from preke");
            //index++;
            //await _databaseOperationsService.ExecuteAsync($"insert into " +
            //    $"preke(pavadinimas, pagaminimo_data, kaina, miestas, modelis, aprasymas, kiekis, " +
            //    $"gamintojas, kategorija, kokybe, nuotrauka, id_Preke, fk_Tiekejasid_Tiekejas) " +
            //    $"values({product.Pavadinimas}, {product.Pagaminimo_Data}, {product.Kaina}, {product.Miestas}, {product.Modelis}, " +
            //    $"{product.Aprasymas}, {product.Kiekis}, {product.Gamintojas}, {product.Kategorija}, {product.Kokybe}, {product.Nuotrauka}, " +
            //    $"{index}, {product.Fk_Tiekejasid_Tiekejas})");
            if (string.IsNullOrEmpty(product.Picture))
                product.Picture = "https://t4.ftcdn.net/jpg/04/70/29/97/360_F_470299797_UD0eoVMMSUbHCcNJCdv2t8B2g1GVqYgs.jpg";
            await _databaseOperationsService.ExecuteAsync($"insert into preke(Name, Description, Price, Picture, Material, Connection, Category, Attack, Defense, Speed) values('{product.Name}', '{product.Description}', {product.Price}, '{product.Picture}',{product.Material}, {product.Connection}, {product.Category}, {product.Attack}, {product.Defense}, {product.Speed})");
        }

        // updates record of product in DB by ID
        // PATCH api/<ProductsController>/5
        [HttpPut]
        public async Task Update([FromBody] Product product)
        {
            if (string.IsNullOrEmpty(product.Picture))
                product.Picture = "https://t4.ftcdn.net/jpg/04/70/29/97/360_F_470299797_UD0eoVMMSUbHCcNJCdv2t8B2g1GVqYgs.jpg";
            await _databaseOperationsService.ExecuteAsync($"update preke " +
                $"set Name = '{product.Name}', Price = {product.Price}, " +
                $"Description = '{product.Description}', Picture = '{product.Picture}' where Id = {product.Id}");
        }

        // Deletes product from DB by ID
        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _databaseOperationsService.ExecuteAsync($"delete from preke where Id = {id}");
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