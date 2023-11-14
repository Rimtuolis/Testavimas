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
    public class TournamentFightsControllerTests
    {
        private readonly Mock<IDatabaseOperationsService> _databaseOperationMock;
        private readonly Mock<ILogger<TournamentFightsController>> _loggerMock;
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<ICurrentUserService> _currentUserMock;
        public TournamentFightsControllerTests()
        {
            _databaseOperationMock = new Mock<IDatabaseOperationsService>();
            _loggerMock = new Mock<ILogger<TournamentFightsController>>();
            _currentUserMock = new Mock<ICurrentUserService>();
        }
        [TestMethod]
        public async Task GetTournamentFightsListTest()
        {
            var expectedTournamenttList = _fixture.Create<List<Shared.TournamentFight>>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<TournamentFight>($"SELECT * FROM turnyro_kova")).ReturnsAsync(expectedTournamenttList);
            var sut = new TournamentFightsController(_databaseOperationMock.Object,_loggerMock.Object);
            var output = await sut.Get();
            Assert.AreEqual(expectedTournamenttList, output);
        }
        [TestMethod]
        public async Task GetByIdTest()
        {
            var tournamentId = 1;

            var expectedTournamentFights = new List<TournamentFight>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<TournamentFight>($"select * from turnyro_kova where fk_turnyras = {tournamentId}")).ReturnsAsync(expectedTournamentFights);

            TournamentFightsController _tournamentFightsController = new TournamentFightsController(_databaseOperationMock.Object, _loggerMock.Object);
            var result = await _tournamentFightsController.Get(tournamentId);

            // Assert
            Assert.AreEqual(expectedTournamentFights, result);

        }
        [TestMethod]
        public async Task CreateTest()
        {
            var expectedTournament = _fixture.Create<TournamentFight>();
            var sut = new TournamentFightsController(_databaseOperationMock.Object, _loggerMock.Object);
            await sut.Post(expectedTournament);
            _databaseOperationMock.Verify(x => x.ExecuteAsync(It.IsAny<string>()), Times.Once);
        }
        [TestMethod]
        public async Task GetLastFightByIdTest()
        {
            var expectedTournament = _fixture.Build<Shared.TournamentFight>().With(x => x.id, 12).Create();
            _databaseOperationMock.Setup(x => x.ReadItemAsync<TournamentFight>($"select * from turnyro_kova where fk_turnyras = {expectedTournament.id} and id = (SELECT max(id) FROM turnyro_kova where fk_turnyras = {expectedTournament.id})")).ReturnsAsync(expectedTournament);
            var sut = new TournamentFightsController(_databaseOperationMock.Object, _loggerMock.Object);
            var outpuT = await sut.GetLastFight(expectedTournament.id);
            Assert.AreEqual(expectedTournament, outpuT);
        }

    }
}
