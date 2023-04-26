using Microsoft.AspNetCore.Mvc;
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

        public AuthController(IDatabaseOperationsService databaseOperationsService, ICurrentUserService currentUserService)
        {
            _databaseOperationsService = databaseOperationsService;
            _currentUserService = currentUserService;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<CurrentUser?> Login([FromBody] LoginDto value)
        {
            if (value.Email is null || value.Password is null)
            {
                return null;
            }

            Shared.Client? client = await _databaseOperationsService.ReadItemAsync<Shared.Client>($"select * from klientas where el_pastas = '{value.Email}' and slaptazodis = '{value.Password}'");

            if (client is not null)
            {
                var user = new CurrentUser
                {
                    Id = client.id_Klientas,
                    LoggedIn = true,
                    Email = client.el_pastas,
                    Username = client.slapyvardis,
                    UserLevel = AccessLevelType.CLIENT
                };

                _currentUserService.SetUser(user);
                return user;
            }

            Worker? worker = await _databaseOperationsService.ReadItemAsync<Worker>($"select * from sandelinkas where el_pastas = '{value.Email}' and slaptazodis = '{value.Password}'");

            if (worker is not null)
            {
                var user = new CurrentUser
                {
                    Id = worker.id_Sandelinkas,
                    LoggedIn = true,
                    Email = worker.el_pastas,
                    Username = worker.slapyvardis,
                    UserLevel = AccessLevelType.WORKER
                };

                _currentUserService.SetUser(user);
                return user;
            }

            Supplier? supplier = await _databaseOperationsService.ReadItemAsync<Supplier>($"select * from tiekejas where el_pastas = '{value.Email}' and slaptazodis = '{value.Password}'");

            if (supplier is not null)
            {
                var user = new CurrentUser
                {
                    Id = supplier.id_Tiekejas,
                    LoggedIn = true,
                    Email = supplier.el_pastas,
                    Username = supplier.slapyvardis,
                    UserLevel = AccessLevelType.SUPPLIER
                };

                _currentUserService.SetUser(user);
                return user;
            }

            Manager? manager = await _databaseOperationsService.ReadItemAsync<Manager>($"select * from vadybininkas where el_pastas = '{value.Email}' and slaptazodis = '{value.Password}'");

            if (manager is not null)
            {
                var user = new CurrentUser
                {
                    Id = manager.id_Vadybininkas,
                    LoggedIn = true,
                    Email = manager.el_pastas,
                    Username = manager.slapyvardis,
                    UserLevel = AccessLevelType.ADMIN
                };

                _currentUserService.SetUser(user);
                return user;
            }

            return null;
        }

        // POST api/auth/register
        [HttpPost("register")]
        public async Task<CurrentUser?> Register([FromBody] ProfileCreation value)
        {//                                                                                                                                                  await _databaseOperationsService.ExecuteAsync($"INSERT INTO `klientas` (`vardas`, `pavarde`, `slapyvardis`, `slaptazodis`, `gimimo_data`, `miestas`, `el_pastas`, `pasto_kodas`, `adresas`, `id_Klientas`) VALUES ('{value.vardas}', '{value.pavarde}', '{value.slapyvardis}', '{value.slaptazodis}', '{value.gimimo_data}', '{value.vardas}', '{value.miestas}', '{value.el_pastas}', '{value.pasto_kodas}', '{value.adresas}')");
            Shared.Client? id = await _databaseOperationsService.ReadItemAsync<Shared.Client>($"SELECT MAX(id_klientas) as id_Klientas FROM klientas;");
            await _databaseOperationsService.ExecuteAsync($"INSERT INTO `klientas` (`vardas`, `pavarde`, `slapyvardis`, `slaptazodis`, `gimimo_data`, `miestas`, `el_pastas`, `pasto_kodas`, `adresas`, `id_Klientas`) VALUES ('{value.vardas}', '{value.pavarde}', '{value.slapyvardis}', '{value.slaptazodis}', '{value.gimimo_data}', '{value.miestas}', '{value.el_pastas}', '{value.pasto_kodas}', 'UNUSED', '{id.id_Klientas + 1}')");
            Shared.Client? client = await _databaseOperationsService.ReadItemAsync<Shared.Client>($"select * from klientas where el_pastas = '{value.el_pastas}' and slaptazodis = '{value.slaptazodis}'");
            if (client is not null)
            {
                var user = new CurrentUser
                {
                    Id = client.id_Klientas,
                    LoggedIn = true,
                    Email = client.el_pastas,
                    Username = client.slapyvardis,
                    UserLevel = AccessLevelType.ADMIN
                };
                _currentUserService.SetUser(user);
                return user;
            }
            return null;
        }

        // POST api/auth/logout
        [HttpGet("logout")]
        public async Task<CurrentUser?> LogOut()
        {
            _currentUserService.SetUser(new CurrentUser { LoggedIn = false });
            return new CurrentUser { LoggedIn = false };
        }
    }
}