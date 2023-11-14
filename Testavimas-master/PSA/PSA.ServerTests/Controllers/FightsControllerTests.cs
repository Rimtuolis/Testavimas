using AutoFixture;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PSA.Server.Services;
using PSA.Services;
using PSA.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.Server.Controllers.Tests
{
    [TestClass()]
    public class FightsControllerTests
    {
        private readonly Mock<IDatabaseOperationsService> _databaseOperationMock;
        private readonly Mock<ILogger<FightsController>> _loggerMock;
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<ICurrentUserService> _currentUserMock;

        public FightsControllerTests()
        {
            _databaseOperationMock = new Mock<IDatabaseOperationsService>();
            _currentUserMock = new Mock<ICurrentUserService>();
            _loggerMock = new Mock<ILogger<FightsController>>();
        }
        [TestMethod]
        public async Task GetActiveFightsTest()
        {
            var expectedFights = new List<Fight>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<Fight>("select * from kova where state = 3")).ReturnsAsync(expectedFights);

            FightsController _fightsController = new FightsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserMock.Object);

            var result = await _fightsController.GetAllCompletedFights();

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task GetRobotFightsTest()
        {
            var robotId = 1;
            var expectedFights = new List<Fight>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<Fight>($"select * from kova where fk_robot1 = {robotId} or fk_robot2 = {robotId}"))
                .ReturnsAsync(expectedFights);
            FightsController _fightsController = new FightsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserMock.Object);
            var result = await _fightsController.GetRobotsFights(robotId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetMaxIdTest()
        {
            var expectedMaxId = 10;
            _databaseOperationMock.Setup(x => x.ReadItemAsync<int?>("select max(id) from kova")).ReturnsAsync(expectedMaxId);
            FightsController _fightsController = new FightsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserMock.Object);
            var result = await _fightsController.GetMaxId();

            Assert.AreEqual(expectedMaxId, result);
        }
        [TestMethod]
        public async Task GetByIdTest()
        {
            var id = 1;
            var expectedFight = new Fight();
            _databaseOperationMock.Setup(x => x.ReadItemAsync<Fight?>($"SELECT * FROM kova where id = {id}")).ReturnsAsync(expectedFight);

            FightsController _fightsController = new FightsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserMock.Object);
            var result = await _fightsController.Get(id);

            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task GetTodayTournamentFights()
        {
            var id = 1;
            var expectedFights = new List<Fight>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<Fight?>($"SELECT kova.date, kova.winner, kova.id, kova.state, kova.fk_robot1, kova.fk_robot2 FROM kova join turnyro_kova on kova.id = turnyro_kova.fk_kova where turnyro_kova.fk_turnyras = {id} and kova.date <= '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'"))
                .ReturnsAsync(expectedFights);

            FightsController _fightsController = new FightsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserMock.Object);
            var result = await _fightsController.Gettodaytournamentfights(id);

            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task GetSwipeFightsTest()
        {
            var expectedFights = new List<Fight>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<Fight?>($"SELECT * FROM kova where id not in (select fk_kova from turnyro_kova) and state = 2"))
                .ReturnsAsync(expectedFights);

            FightsController _fightsController = new FightsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserMock.Object);
            var result = await _fightsController.GetSwipeFights();

            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task GetTournamentFights()
        {
            var id = 1;
            var expectedFights = new List<Fight>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<Fight?>($"SELECT kova.date, kova.winner, kova.id, kova.state, kova.fk_robot1, kova.fk_robot2 FROM kova join turnyro_kova on kova.id = turnyro_kova.fk_kova where turnyro_kova.fk_turnyras = {id} and kova.state = 2"))
                .ReturnsAsync(expectedFights);

            FightsController _fightsController = new FightsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserMock.Object);
            var result = await _fightsController.GetTournamentFights(id);

            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task InsertFightTest()
        {
            var fight = new Fight { date = DateTime.Now, fk_robot1 = 1, fk_robot2 = 2 };

            FightsController _fightsController = new FightsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserMock.Object);
            await _fightsController.Create(fight);

            _databaseOperationMock.Verify(x => x.ExecuteAsync($"insert into kova(date, winner, state, fk_robot1, fk_robot2) values('{fight.date.ToString("yyyy-MM-dd HH:mm:ss")}',0, 1, '{fight.fk_robot1}', '{fight.fk_robot2}')"), Times.Once);
        }
        [TestMethod]
        public async Task UpdateWithStateAndWinnerTest()
        {
            var fight = new Fight { id = 1, state = 2, winner = 1 };

            FightsController _fightsController = new FightsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserMock.Object);
            await _fightsController.Put(fight);

            _databaseOperationMock.Verify(x => x.ExecuteAsync($"update kova set state = {fight.state}, winner = {fight.winner} where id = {fight.id}"), Times.Once);
        }
        [TestMethod]
        public async Task UpdateWithFightStage2Test()
        {
            var id = 1;

            FightsController _fightsController = new FightsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserMock.Object);
            await _fightsController.Update(id);


            _databaseOperationMock.Verify(x => x.ExecuteAsync($"update kova set state = 2 WHERE id = {id}"), Times.Once);
        }

        [TestMethod]
        public async Task UpdateWithFightStage3Test()
        {
            var id = 1;

            FightsController _fightsController = new FightsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserMock.Object);
            await _fightsController.Update2(id);

            _databaseOperationMock.Verify(x => x.ExecuteAsync($"update kova set state = 3 WHERE id = {id}"), Times.Once);
        }
        [TestMethod]
        public async Task UpdateWithFightStateAndWinnerTest()
        {
            var fight = new Fight { id = 1, winner = 2 };

            FightsController _fightsController = new FightsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserMock.Object);
            await _fightsController.Update3(fight);

            _databaseOperationMock.Verify(x => x.ExecuteAsync($"update kova set state = 3, winner = {fight.winner} WHERE id = {fight.id}"), Times.Once);
        }
        [TestMethod]
        public async Task DeleteFightTest()
        {
            var id = 1;

            FightsController _fightsController = new FightsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserMock.Object);
            await _fightsController.Delete(id);

            _databaseOperationMock.Verify(x => x.ExecuteAsync($"DELETE FROM kova WHERE id={id}"), Times.Once);
        }
    }
}
