using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using PSA.Server.Services;
using PSA.Services;
using PSA.Shared;

namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IDatabaseOperationsService _databaseOperationsService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<OrdersController> _logger;
        private readonly ICartService _cartService;
        private readonly IMailService _mailService;

        public OrdersController(IDatabaseOperationsService databaseOperationsService, ICurrentUserService currentUserService, ILogger<OrdersController> logger, ICartService cartService, IMailService mailService)
        {
            _databaseOperationsService = databaseOperationsService;
            _currentUserService = currentUserService;
            _logger = logger;
            _cartService = cartService;
            _mailService = mailService;
        }

        // returns all orders by user ID
        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<IEnumerable<OrderDto>> Get()
        {
            // does not work by user yet - currently returns all
            return await _databaseOperationsService.ReadListAsync<OrderDto>($"SELECT * FROM uzsakymas WHERE fk_user_id={_currentUserService.GetUser().Id}");
        }

        // gets info about order by ID
        // GET api/<OrdersController>/5

        [HttpGet("{id}")]
        public async Task<OrderDto?> Get(int id)
        {
            return await _databaseOperationsService.ReadItemAsync<OrderDto>($"SELECT * FROM uzsakymas WHERE id_Uzsakymas={id}");
        }
  
        // creates new order
        // POST api/<OrdersController>
        [HttpPost]
        public async Task Post([FromBody] double total)
        {
            var cart = _cartService.GetCart();

            try
            {
                var index = await _databaseOperationsService.ReadItemAsync<int?>("select max(id_Uzsakymas) from uzsakymas");
                if (index is null)
                {
                    index = 0;
                }
                index++;

                await _databaseOperationsService.ExecuteAsync($"insert into uzsakymas(suma, data, busena, id_Uzsakymas, fk_user_id) values({total}, NOW(), {(int)OrderState.Patvirtintas}, {index}, {_currentUserService.GetUser().Id})");

                _cartService.ClearCart();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to create order", ex);
            }
        }

        [HttpPut]
        public async Task Put([FromBody] OrderDto order)
        {
            await _databaseOperationsService.ExecuteAsync($"update uzsakymas set busena = {(int)order.Busena}, " +
                $" data = NOW() where id_Uzsakymas = {order.Id_Uzsakymas}");
        }

        // Deletes Order by ID
        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _databaseOperationsService.ExecuteAsync($"DELETE FROM uzsakymas WHERE id_Uzsakymas={id}");
        }
    }
}