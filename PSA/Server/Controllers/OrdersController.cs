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
            return await _databaseOperationsService.ReadListAsync<OrderDto>($"SELECT * FROM uzsakymas");
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
                index++;
                var sandelininkuIds = await _databaseOperationsService.ReadListAsync<int>("select id_Sandelinkas from sandelinkas");
                var sandelininkas = sandelininkuIds[new Random().Next(0, sandelininkuIds.Count - 1)];

                await _databaseOperationsService.ExecuteAsync($"insert into uzsakymas(suma, data, busena, id_Uzsakymas, fk_Klientasid_Klientas, fk_sandelininkas) values({total}, NOW(), {(int)OrderState.Nauja}, {index}, {_currentUserService.GetUser().Id}, {sandelininkas})");

                //foreach (var product in cart)
                //{
                //    for (int i = 0; i < product.Kiekis; i++)
                //    {
                //        await _databaseOperationsService.ExecuteAsync($"insert into prekes_uzakymas values({index}, {product.Id_Preke})");
                //    }
                //}
                var contract_index = await _databaseOperationsService.ReadItemAsync<int?>("select max(id_Sutartis) from sutartis");
                contract_index++;
                var ManagersIds = await _databaseOperationsService.ReadListAsync<int>("select id_Sandelinkas from sandelinkas");
                var managerID = ManagersIds[new Random().Next(0, ManagersIds.Count - 1)];
                await _databaseOperationsService.ExecuteAsync($"insert into sutartis values(NOW(), {contract_index}, {index}, {managerID})");
                await _databaseOperationsService.ExecuteAsync($"insert into email_saskaita(data, prekiu_skaicius, suma, " +
                    $"papildoma_informacija, fk_Sandelinkasid_Sandelinkas, fk_Klientasid_Klientas, fk_Sutartisid_Sutartis) " +
                    $"values(NOW(), {cart.Count}, {total}, '-', {sandelininkas}, {_currentUserService.GetUser().Id}, {contract_index})");
                Shared.Client? client = await _databaseOperationsService.ReadItemAsync<Shared.Client>($"select * from klientas where id_Klientas = {_currentUserService.GetUser().Id}");
                Contract? contract = await _databaseOperationsService.ReadItemAsync<Contract>($"select * from sutartis where id_Sutartis = {contract_index}");
                Manager? manager = await _databaseOperationsService.ReadItemAsync<Manager>($"select * from vadybininkas where id_Vadybininkas = {managerID}");
                Worker? worker = await _databaseOperationsService.ReadItemAsync<Worker>($"select * from sandelinkas where id_Sandelinkas = {sandelininkas}");
                await _mailService.SendInvoice(client, contract, manager, worker);
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
                $"fk_sandelininkas = {order.Fk_Sandelininkas}, data = NOW() where id_Uzsakymas = {order.Id_Uzsakymas}");
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