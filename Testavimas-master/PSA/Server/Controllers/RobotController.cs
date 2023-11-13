//using Microsoft.AspNetCore.Mvc;
//using PSA.Client.Pages.Robot;
//using PSA.Server.Services;
//using PSA.Services;
//using PSA.Shared;

//namespace PSA.Server.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class RobotController : ControllerBase
//    {
//        private readonly ILogger<RobotController> _logger;
//        private readonly IDatabaseOperationsService _databaseOperationsService;
//        private readonly ICurrentUserService _currentUserService;

//        public RobotController(ILogger<RobotController> logger, IDatabaseOperationsService databaseOperationsService, ICurrentUserService currentUserService)
//        {
//            _logger = logger;
//            _databaseOperationsService = databaseOperationsService;
//            _currentUserService = currentUserService;
//        }

//        // Gets all products and retuns the list
//        // GET: api/<ProductsController>
//        [HttpGet]
//        public async Task<IEnumerable<Robot>> Get()
//        {
//            return await _databaseOperationsService.ReadListAsync<Robot>($"SELECT * FROM preke");
//        }
//        //[HttpGet("{category}")]
//        //public async Task<IEnumerable<Product>> Get(string category)
//        //{
//        //    return await _databaseOperationsService.ReadListAsync<Product>($"SELECT * FROM preke WHERE Category={category}");
//        //}
//        // Gets product by ID
//        // GET api/<ProductsController>/5
//        [HttpGet("{id}")]
//        public async Task<Product?> Get(int id)
//        {
//            //return await _databaseOperationsService.ReadItemAsync<Product?>($"SELECT * FROM preke where id_Preke = {id}");
//            //return await _databaseOperationsService.ReadItemAsync<Product?>($"SELECT * FROM robotas JOIN roboto_detale ON {id} = roboto_detale.fk_robotas JOIN preke ON preke_id = roboto_detale.fk_preke");
//            return await _databaseOperationsService.ReadItemAsync<Product?>($"SELECT * FROM preke WHERE id = {id}");
//        }

//        // creates product in DB
//        // POST api/<ProductsController>
//        [HttpPost]
//        public async Task Create([FromBody] Robot robot)
//        {
//            var index = await _databaseOperationsService.ReadItemAsync<int?>("select max(id_robotas) from robotas");
//            index++;
//            if (index == null) { index = 0; }

//            await _databaseOperationsService.ExecuteAsync($"insert into " +
//                $"robotas(id_robotas, laimejimai, pralaimejimai, lygiosios, fk_user)" +
//                $"values({index}, {0}, {0}, {0}, {_currentUserService.GetUser().Id})");


//            await _databaseOperationsService.ExecuteAsync($"insert into " +
//                 $"roboto_detale(bukle, fk_preke, fk_robotas)" +
//                 $"values({100}, {robot.Head},{index})");
//            await _databaseOperationsService.ExecuteAsync($"insert into " +
//                 $"roboto_detale(bukle, fk_preke, fk_robotas)" +
//                 $"values({100}, {robot.Body},{index})");
//            await _databaseOperationsService.ExecuteAsync($"insert into " +
//                 $"roboto_detale(bukle, fk_preke, fk_robotas)" +
//                 $"values({100}, {robot.RightArm},{index})");
//            await _databaseOperationsService.ExecuteAsync($"insert into " +
//                 $"roboto_detale(bukle, fk_preke, fk_robotas)" +
//                 $"values({100}, {robot.LeftArm},{index})");
//            await _databaseOperationsService.ExecuteAsync($"insert into " +
//                 $"roboto_detale(bukle, fk_preke, fk_robotas)" +
//                 $"values({100}, {robot.RightLeg},{index})");
//            await _databaseOperationsService.ExecuteAsync($"insert into " +
//                 $"roboto_detale(bukle, fk_preke, fk_robotas)" +
//                 $"values({100}, {robot.LeftLeg},{index})");
//        }

//        ////// updates record of product in DB by ID
//        ////// PATCH api/<ProductsController>/5
//        ////[HttpPut]
//        ////public async Task Update([FromBody] Robot robot)
//        ////{
//        ////    if (string.IsNullOrEmpty(product.Picture))
//        ////        product.Picture = "https://t4.ftcdn.net/jpg/04/70/29/97/360_F_470299797_UD0eoVMMSUbHCcNJCdv2t8B2g1GVqYgs.jpg";
//        ////    await _databaseOperationsService.ExecuteAsync($"update preke " +
//        ////        $"set Name = '{product.Name}', Price = {product.Price}, " +
//        ////        $"Description = '{product.Description}', Picture = '{product.Picture}' where Id = {product.Id}");
//        ////}

//        // Deletes product from DB by ID
//        // DELETE api/<ProductsController>/5
//        [HttpDelete("{id}")]
//        public async Task Delete(int id)
//        {
//            await _databaseOperationsService.ExecuteAsync($"delete from robotas where Id = {id}");
//        }

//        // Gets product by ID
//        // GET api/<ProductsController>/5
//        [HttpGet("{id}/stats")]
//        public async Task<Robot?> GetProductStats(int id)
//        {
//            return await _databaseOperationsService.ReadItemAsync<Robot>($"select * from statistika where id_Statistika = {id}");
//        }
//    }
//}
