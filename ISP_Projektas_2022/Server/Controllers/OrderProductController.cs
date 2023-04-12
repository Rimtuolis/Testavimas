using Microsoft.AspNetCore.Mvc;
using ISP_Projektas_2022.Server.Services;
using ISP_Projektas_2022.Services;
using ISP_Projektas_2022.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ISP_Projektas_2022.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderProductController : ControllerBase
    {
        private readonly IDatabaseOperationsService _databaseOperationsService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<OrderProductController> _logger;

        public OrderProductController(IDatabaseOperationsService databaseOperationsService, ICurrentUserService currentUserService, ILogger<OrderProductController> logger)
        {
            _databaseOperationsService = databaseOperationsService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        // GET: api/<OrderProductController/5>
        [HttpGet("{id}")]
        public async Task<IEnumerable<OrderProductDto>> Get(int id)
        {
            return await _databaseOperationsService.ReadListAsync<OrderProductDto>($"select * from prekes_uzakymas where fk_uzsakymas = {id}");
        }

        // POST api/<OrderProductController>
        [HttpPost]
        public async Task Post([FromBody] OrderProductDto op)
        {
            await _databaseOperationsService.ExecuteAsync($"insert into prekes_uzakymas(fk_uzsakymas, fk_preke) values({op.fk_uzsakymas}, {op.fk_preke})");
        }
    }
}