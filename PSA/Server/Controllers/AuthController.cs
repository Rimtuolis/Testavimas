using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PSA.Server.Services;
using PSA.Services;
using PSA.Shared;

namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IDatabaseOperationsService _databaseOperationsService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger _logger;

        public AuthController(IDatabaseOperationsService databaseOperationsService, ICurrentUserService currentUserService, ILogger<AuthController> logger)
        {
            _databaseOperationsService = databaseOperationsService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<CurrentUser?> Login([FromBody] LoginDto value)
        {
            if (value.Email is null || value.Password is null)
            {
                return null;
            }

            Shared.Client? client = null;

            // Check if what is returned by db
            try
            {
                client = await _databaseOperationsService.ReadItemAsync<Shared.Client>($"select * from user where email = '{value.Email}' and password = '{value.Password}'");
            }
            catch (Exception)
            {
                Console.WriteLine("Errrrrrrr");
            }

            if (client is not null)
            {
                var user = new CurrentUser
                {
                    Id = client.id_User,
                    LoggedIn = true,
                    Email = client.email,
                    Username = client.nickname,
                    UserLevel = client.role,
                    balance = client.balance,
                };

                _currentUserService.SetUser(user);
                return user;
            }

            return null;
        }

        // POST api/auth/register
        [HttpPost("register")]
        public async Task<CurrentUser?> Register([FromBody] ProfileCreation value)
        {
            //Shared.Client? id = await _databaseOperationsService.ReadItemAsync<Shared.Client>($"SELECT MAX(id_User) as id_User FROM klientas;");
            await _databaseOperationsService.ExecuteAsync($"INSERT INTO `User` (`name`, `last_name`, `nickname`, `password`, " +
                $"`birthdate`, `city`, `email`, `post_code`, balance, role) " +
                $"VALUES ('{value.name}', '{value.last_name}', '{value.nickname}', '{value.password}', " +
                $"'{value.birthdate}', '{value.city}', '{value.email}', '{value.post_code}', 0, {(int) AccessLevelType.CLIENT})");
            Shared.Client? client = await _databaseOperationsService.ReadItemAsync<Shared.Client>($"select * from User where email = '{value.email}' and password = '{value.password}'");
            if (client is not null)
            {
                var user = new CurrentUser
                {
                    Id = client.id_User,
                    LoggedIn = true,
                    Email = client.email,
                    Username = client.nickname,
                    UserLevel = client.role,
                    balance = client.balance,
                };
                _currentUserService.SetUser(user);
                return user;
            }
            return null;
        }

        // POST api/auth/logout
        [HttpGet("logout")]
        public CurrentUser? LogOut()
        {
            _currentUserService.SetUser(new CurrentUser { LoggedIn = false });
            return new CurrentUser { LoggedIn = false };
        }
    }
}