using PSA.Server.Services;
using PSA.Services;
using PSA.Shared;
using Microsoft.AspNetCore.Mvc;

namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IDatabaseOperationsService _databaseOperationsService;
        private readonly ICurrentUserService _currentUserService;

        public ProfileController(IDatabaseOperationsService databaseOperationsService, ICurrentUserService currentUserService)
        {
            _databaseOperationsService = databaseOperationsService;
            _currentUserService = currentUserService;
        }

        // POST api/profiles/getAll
        [HttpGet("getAll")]
        public async Task<List<Profile?>> GetAll()
        {

            
            var clients = await _databaseOperationsService.ReadListAsync<Profile>($"select vardas, pavarde, el_pastas, slapyvardis from klientas");

            var workers = await _databaseOperationsService.ReadListAsync<Profile>($"select vardas, pavarde, el_pastas,  slapyvardis from sandelinkas");

            clients.AddRange(workers);

            var managers = await _databaseOperationsService.ReadListAsync<Profile>($"select vardas, pavarde, el_pastas,  slapyvardis from vadybininkas");

            clients.AddRange(managers);


            return clients;
        }

        // POST api/profile/get
        [HttpPost("get")]
        public async Task<Profile?> Get([FromBody] Profile value)
        {

            
            var client = await _databaseOperationsService.ReadItemAsync<Profile>($"select vardas, pavarde, el_pastas, slapyvardis from klientas where slapyvardis = '{value.slapyvardis}'");

            if(client!=null){
                return client;
            }
            var worker = await _databaseOperationsService.ReadItemAsync<Profile>($"select vardas, pavarde, el_pastas, slapyvardis from sandelinkas where slapyvardis = '{value.slapyvardis}'");

            if(worker!=null){
                return worker;
            }

            var manager = await _databaseOperationsService.ReadItemAsync<Profile>($"select vardas, pavarde, el_pastas, slapyvardis from vadybininkas where slapyvardis = '{value.slapyvardis}'");

            if(manager!=null){
                return manager;
            }

            return null;
        }

        // POST api/profiles/edit
        [HttpPost("edit")]
        public async void EditProfile([FromBody] ProfileCreation value)
        {   
            await _databaseOperationsService.ExecuteAsync($"UPDATE `klientas` SET `vardas`='{value.vardas}', `pavarde` = '{value.pavarde}',  `slaptazodis`='{value.slaptazodis}', `gimimo_data`='{value.gimimo_data}', `miestas`='{value.miestas}', `el_pastas`='{value.el_pastas}', `pasto_kodas`='{value.pasto_kodas}' WHERE `slapyvardis`='{value.slapyvardis}'");
        }

        // POST api/profiles/delete
        [HttpPost("delete")]
        public async void DeleteProfile([FromBody] ProfileCreation value)
        {
            await _databaseOperationsService.ExecuteAsync($"UPDATE `klientas` SET `slapyvardis`='DELETED USER' WHERE `slapyvardis`='{value.slapyvardis}'");
            await _databaseOperationsService.ExecuteAsync($"UPDATE `sandelinkas` SET `slapyvardis`='DELETED USER' WHERE `slapyvardis`='{value.slapyvardis}'");
            await _databaseOperationsService.ExecuteAsync($"UPDATE `vadybininkas` SET `slapyvardis`='DELETED USER' WHERE `slapyvardis`='{value.slapyvardis}'");

        }
    }
}