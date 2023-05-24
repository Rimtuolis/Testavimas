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

            
            var clients = await _databaseOperationsService.ReadListAsync<Profile>($"select name, last_name, email, nickname from User");


            return clients;
        }

        // POST api/profile/get
        [HttpPost("get")]
        public async Task<Profile?> Get([FromBody] Profile value)
        {

            
            var client = await _databaseOperationsService.ReadItemAsync<Profile>($"select name, last_name, email, nickname from user where nickname = '{value.nickname}'");

            if(client!=null){
                return client;
            }

            return null;
        }

        // POST api/profiles/edit
        [HttpPost("edit")]
        public async void EditProfile([FromBody] ProfileCreation value)
        {   
            await _databaseOperationsService.ExecuteAsync($"UPDATE user SET name='{value.name}', last_name = '{value.last_name}',  password = '{value.password}', birthdate = '{value.birthdate}', city = '{value.city}', email = '{value.email}', post_code='{value.post_code}' WHERE `nickname`='{value.nickname}'");
        }

        // POST api/profiles/delete
        [HttpPost("delete")]
        public async void DeleteProfile([FromBody] ProfileCreation value)
        {
            await _databaseOperationsService.ExecuteAsync($"DELETE FROM user WHERE nickname ='{value.nickname}'");
            

        }
    }
}