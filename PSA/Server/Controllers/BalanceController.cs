using Microsoft.AspNetCore.Mvc;
using PSA.Server.Services;
using PSA.Services;
using PSA.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly ILogger<BalanceController> _logger;
        private readonly IDatabaseOperationsService _databaseOperationsService;
        private readonly ICurrentUserService _currentUserService;
        
        public BalanceController(ILogger<BalanceController> logger, IDatabaseOperationsService databaseOperationsService, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _databaseOperationsService = databaseOperationsService;
            _currentUserService = currentUserService;
        }

        // PUT api/<BalanceController>/5
        [HttpPut("{id}")]
        public async void Put(int id, [FromBody] CurrentUser user)
        {
            await _databaseOperationsService.ExecuteAsync($"update user set balance = {user.balance} where id_User = {id}");
            _currentUserService.SetUser(user);
        }

    }
}
