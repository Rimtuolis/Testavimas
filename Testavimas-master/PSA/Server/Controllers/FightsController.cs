﻿using Microsoft.AspNetCore.Mvc;
using PSA.Server.Services;
using PSA.Services;
using PSA.Shared;

namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FightsController : ControllerBase
    {
        private readonly ILogger<FightsController> _logger;
        private readonly IDatabaseOperationsService _databaseOperationsService;
        private readonly ICurrentUserService _currentUserService;
        public FightsController(ILogger<FightsController> logger, IDatabaseOperationsService databaseOperationsService, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _databaseOperationsService = databaseOperationsService;
            _currentUserService = currentUserService;
		}
        [HttpGet]
        public async Task<IEnumerable<Fight>> GetAllCompletedFights()
		{
			return await _databaseOperationsService.ReadListAsync<Fight>($"select * from kova where state = 3");
		}
		// GET: api/<FightsController>
		[HttpGet("view/{robotId}")]
        public async Task<IEnumerable<Fight>> GetRobotsFights(int robotId)
        {
            return await _databaseOperationsService.ReadListAsync<Fight>($"select * from kova where fk_robot1 = {robotId} or fk_robot2 = {robotId}");
        }
		[HttpGet("maxid")]
        public async Task<int?> GetMaxId()
        {
            return await _databaseOperationsService.ReadItemAsync<int?>($"select max(id) from kova");
        }

        // Gets supplier by ID
        // GET api/<FightsController>/5
        [HttpGet("{id}")]
        public async Task<Fight?> Get(int id)
        {
            return await _databaseOperationsService.ReadItemAsync<Fight?>($"SELECT * FROM kova where id = {id}");
        }
        [HttpGet("todaytournamentfights/{id}")]
        public async Task<IEnumerable<Fight>?> Gettodaytournamentfights(int id)
        {
            string myDateTimeString = (DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss");
            return await _databaseOperationsService.ReadListAsync<Fight?>($"SELECT kova.date, kova.winner, kova.id, kova.state, kova.fk_robot1, kova.fk_robot2 FROM kova join turnyro_kova on kova.id = turnyro_kova.fk_kova where turnyro_kova.fk_turnyras = {id} and kova.date <= '{myDateTimeString}'");
        }

		[HttpGet("swipefights")]
		public async Task<IEnumerable<Fight>?> GetSwipeFights()
		{
			return await _databaseOperationsService.ReadListAsync<Fight?>($"SELECT * FROM kova where id not in (select fk_kova from turnyro_kova) and state = 2");
		}

		[HttpGet("tourneyfights/{id}")]
		public async Task<IEnumerable<Fight>?> GetTournamentFights(int id)
		{
			return await _databaseOperationsService.ReadListAsync<Fight?>($"SELECT kova.date, kova.winner, kova.id, kova.state, kova.fk_robot1, kova.fk_robot2 FROM kova join turnyro_kova on kova.id = turnyro_kova.fk_kova where turnyro_kova.fk_turnyras = {id} and kova.state = 2");
		}
		[HttpPut]
        public async Task Put([FromBody] Fight fight)
        {
            await _databaseOperationsService.ExecuteAsync($"update kova set state = {fight.state}, winner = {fight.winner} where id = {fight.id}");
        }

        [HttpPost]
        public async Task Create([FromBody] Fight fight)
        {
			string myDateTimeString = fight.date.ToString("yyyy-MM-dd HH:mm:ss");
            await _databaseOperationsService.ExecuteAsync($"insert into kova(date, winner, state, fk_robot1, fk_robot2) values('{myDateTimeString}',0, 1, '{fight.fk_robot1}', '{fight.fk_robot2}')");
        }
		[HttpPut("{id}")]
        public async Task Update(int id)
        {
            Console.WriteLine("Pirmas");
            await _databaseOperationsService.ExecuteAsync($"update kova set state = 2 WHERE id = {id}");
        }

        [HttpPut("no/{id}")]
        public async Task Update2(int id)
        {
            Console.WriteLine("Antras");

            await _databaseOperationsService.ExecuteAsync($"update kova set state = 3 WHERE id = {id}");
        }
        [HttpPut("win/mhm/")]
        public async Task Update3([FromBody] Fight fight)
        {
            Console.WriteLine("Trecias");

            await _databaseOperationsService.ExecuteAsync($"update kova set state = 3, winner = {fight.winner} WHERE id = {fight.id}");
        }
        // DELETE api/<FightsController>/5
        // DELETE api/<FightsController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            //await _databaseOperationsService.ExecuteAsync($"delete from 'turnyro_kova' where 'fk_kova' = {id}");
            await _databaseOperationsService.ExecuteAsync($"DELETE FROM kova WHERE id={id}");
        }
    }
}
