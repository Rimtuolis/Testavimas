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
        /*[TestMethod]
        public async Task GetByIdTest()
        {
            var expectedTournament = _fixture.Build<Shared.TournamentRobot>().With(x => x.fk_turnyras, 1).Create();
            _databaseOperationMock.Setup(x => x.ReadItemAsync<TournamentRobot>($"SELECT * FROM turnyro_robotas where fk_turnyras = {expectedTournament.fk_turnyras}")).ReturnsAsync(expectedTournament);
            var sut = new TournamentRobotsController(_databaseOperationMock.Object, _loggerMock.Object, _currentUserMock.Object);
            var outpuT = await sut.Get(expectedTournament.fk_turnyras);
            Assert.AreEqual(expectedTournament, outpuT);

        }*/
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

    }
}
