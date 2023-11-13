using AutoFixture;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PSA.Client.Pages.Tournaments;
using PSA.Server.Controllers;
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
    public class TournamentRobotsControllerTests
    {
        private readonly Mock<IDatabaseOperationsService> _databaseOperationMock;
        private readonly Mock<ICurrentUserService> _currentUserMock;
        private readonly Mock<ILogger<TournamentRobotsController>> _loggerMock;
        private readonly Fixture _fixture = new Fixture();
        public TournamentRobotsControllerTests()
        {
            _databaseOperationMock = new Mock<IDatabaseOperationsService>();
            _currentUserMock = new Mock<ICurrentUserService>();
            _loggerMock = new Mock<ILogger<TournamentRobotsController>>();
        }
        [TestMethod]
        public async Task GetTournamentFightsListTest()
        {
            var expectedTournamenttList = _fixture.Create<List<Shared.TournamentRobot>>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<TournamentRobot>($"SELECT * FROM turnyro_robotas")).ReturnsAsync(expectedTournamenttList);
            var sut = new TournamentRobotsController(_databaseOperationMock.Object, _loggerMock.Object, _currentUserMock.Object);
            var output = await sut.Get();
            Assert.AreEqual(expectedTournamenttList, output);
        }
        [TestMethod]
        public async Task GetByIdTest()
        {
            var tournamentId = 1;

            var expectedTournamentRobots = new List<TournamentRobot>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<TournamentRobot>($"select * from turnyro_robotas where fk_turnyras = {tournamentId}")).ReturnsAsync(expectedTournamentRobots);

            TournamentRobotsController _tournamentFightsController = new TournamentRobotsController(_databaseOperationMock.Object, _loggerMock.Object, _currentUserMock.Object);
            var result = await _tournamentFightsController.Get(tournamentId);

            Assert.AreEqual(expectedTournamentRobots, result);

        }
        [TestMethod]
        public async Task CreateTest()
        {
            var expectedTournament = _fixture.Create<TournamentRobot>();
            var sut = new TournamentRobotsController(_databaseOperationMock.Object, _loggerMock.Object, _currentUserMock.Object);
            await sut.Post(expectedTournament);
            _databaseOperationMock.Verify(x => x.ExecuteAsync(It.IsAny<string>()), Times.Once);
        }
        [TestMethod]
        public async Task GetWithoutAFightTest()
        {
            var expectedTournament = _fixture.Build<Shared.TournamentRobot>().With(x => x.id, 12).Create();
            _databaseOperationMock.Setup(x => x.ReadItemAsync<TournamentRobot>($"select * from turnyro_robotas where turi_kova = 0 && fk_turnyras = {expectedTournament.id}")).ReturnsAsync(expectedTournament);
            var sut = new TournamentRobotsController(_databaseOperationMock.Object, _loggerMock.Object, _currentUserMock.Object);
            var outpuT = await sut.GetWithoutAFight(expectedTournament.id);
            Assert.AreEqual(expectedTournament, outpuT);

        }
        [TestMethod]
        public async Task UpdateTournamentRobot()
        {
            var tournamentRobot = new TournamentRobot
            {
                id = 1,
                turi_kova = 0,
                fk_turnyras = 1,
                fk_robotas = 1
            };

            var _tournamentRobotsController = new TournamentRobotsController(_databaseOperationMock.Object, _loggerMock.Object, _currentUserMock.Object);
            await _tournamentRobotsController.Put(tournamentRobot);

            _databaseOperationMock.Verify(x =>
                x.ExecuteAsync($"update turnyro_robotas set turi_kova = {tournamentRobot.turi_kova}, fk_turnyras = {tournamentRobot.fk_turnyras}, fk_robotas = {tournamentRobot.fk_robotas} where id = {tournamentRobot.id}"), Times.Once);
        }
        [TestMethod]
        public async Task GetExistingTournamentRobotsTest_ShouldReturnTrue()
        {
            var robotId = 1;
            var tournamentId = 1;

            _databaseOperationMock.Setup(x =>
                    x.ReadItemAsync<int?>($"select id from turnyro_robotas where fk_robotas = {robotId} && fk_turnyras = {tournamentId}"))
                .ReturnsAsync(1);

            var _tournamentRobotsController = new TournamentRobotsController(_databaseOperationMock.Object, _loggerMock.Object, _currentUserMock.Object);
            var result = await _tournamentRobotsController.GetExisting(robotId, tournamentId);

            Assert.IsTrue(result);
        }
        [TestMethod]
        public async Task GetExistingTournamentRobotsTest_ShouldReturnFalse()
        {
            var robotId = 1;
            var tournamentId = 1;

            _databaseOperationMock.Setup(x =>
                    x.ReadItemAsync<int?>($"select id from turnyro_robotas where fk_robotas = {robotId} && fk_turnyras = {tournamentId}"))
                .ReturnsAsync((int?)null);

            var _tournamentRobotsController = new TournamentRobotsController(_databaseOperationMock.Object, _loggerMock.Object, _currentUserMock.Object);
            var result = await _tournamentRobotsController.GetExisting(robotId, tournamentId);

            Assert.IsFalse(result);
        }
        [TestMethod]
        public async Task GetTournamentRobotsByUserTest_ShouldReturnTrue()
        {
            var tournamentId = 1;

            _currentUserMock.Setup(x => x.GetUser()).Returns(new CurrentUser { Id = 1 });

            _databaseOperationMock.Setup(x =>
                    x.ReadItemAsync<int?>($"select robotas.id from robotas JOIN turnyro_robotas ON robotas.id = turnyro_robotas.fk_robotas && turnyro_robotas.fk_turnyras = {tournamentId} && robotas.fk_user_id = 1"))
                .ReturnsAsync(1);

            var _tournamentRobotsController = new TournamentRobotsController(_databaseOperationMock.Object, _loggerMock.Object, _currentUserMock.Object);
            var result = await _tournamentRobotsController.GetExistingByUser(tournamentId);

            Assert.IsTrue(result);
        }
        [TestMethod]
        public async Task GetExistingTournamentRobotsByUserTest_ShouldReturnFalse()
        {
            var tournamentId = 1;

            _currentUserMock.Setup(x => x.GetUser()).Returns(new CurrentUser { Id = 1 });

            _databaseOperationMock.Setup(x =>
                    x.ReadItemAsync<int?>($"select robotas.id from robotas JOIN turnyro_robotas ON robotas.id = turnyro_robotas.fk_robotas && turnyro_robotas.fk_turnyras = {tournamentId} && robotas.fk_user_id = 1"))
                .ReturnsAsync((int?)null);

            var _tournamentRobotsController = new TournamentRobotsController(_databaseOperationMock.Object, _loggerMock.Object, _currentUserMock.Object);
            var result = await _tournamentRobotsController.GetExistingByUser(tournamentId);

            Assert.IsFalse(result);
        }
        [TestMethod]
        public async Task GetCountwithNoFights_ShouldReturnCountWithNoFights()
        {
            var tournamentId = 1;

            _databaseOperationMock.Setup(x =>
                    x.ReadItemAsync<long>($"select count(id) from turnyro_robotas where fk_turnyras = {tournamentId} && turi_kova = 0"))
                .ReturnsAsync(5);

            var _tournamentRobotsController = new TournamentRobotsController(_databaseOperationMock.Object, _loggerMock.Object, _currentUserMock.Object);
            var result = await _tournamentRobotsController.GetCountwithNoFights(tournamentId);

            Assert.AreEqual(5, result);
        }
        [TestMethod]
        public async Task GetCount_ShouldReturnTotalCount()
        {
            var tournamentId = 1;

            _databaseOperationMock.Setup(x =>
                    x.ReadItemAsync<long>($"select count(id) from turnyro_robotas where fk_turnyras = {tournamentId}"))
                .ReturnsAsync(10);

            var _tournamentRobotsController = new TournamentRobotsController(_databaseOperationMock.Object, _loggerMock.Object, _currentUserMock.Object);
            var result = await _tournamentRobotsController.GetCount(tournamentId);

            Assert.AreEqual(10, result);
        }
    }
}
