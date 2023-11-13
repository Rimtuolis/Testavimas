using AutoFixture;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PSA.Client.Pages.Tournaments;
using PSA.Server.Controllers;
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
    public class TournamentsControllerTests
    {
        private readonly Mock<IDatabaseOperationsService> _databaseOperationMock;
        private readonly Mock<ILogger<TournamentsController>> _loggerMock;
        private readonly Fixture _fixture = new Fixture();
        public TournamentsControllerTests()
        {
            _databaseOperationMock = new Mock<IDatabaseOperationsService>();
            _loggerMock = new Mock<ILogger<TournamentsController>>();
        }
        [TestMethod]
        public async Task GetTournamentListTest()
        {
            var expectedTournamenttList = _fixture.Create<List<Shared.Tournament>>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<Tournament>($"SELECT * FROM turnyras")).ReturnsAsync(expectedTournamenttList);
            var sut = new TournamentsController(_loggerMock.Object, _databaseOperationMock.Object);
            var output = await sut.Get();
            Assert.AreEqual(expectedTournamenttList, output);
        }
        [TestMethod]
        public async Task GetByIdTest()
        {
            var expectedTournament = _fixture.Build<Shared.Tournament>().With(x => x.Id, 12).Create();
            _databaseOperationMock.Setup(x => x.ReadItemAsync<Tournament>($"SELECT * FROM turnyras where Id = {expectedTournament.Id}")).ReturnsAsync(expectedTournament);
            var sut = new TournamentsController(_loggerMock.Object, _databaseOperationMock.Object);
            var outpuT = await sut.Get(expectedTournament.Id);
            Assert.AreEqual(expectedTournament, outpuT);

        }
        [TestMethod]
        public async Task CreateTest()
        {
            var expectedTournament = _fixture.Create<Tournament>();
            var sut = new TournamentsController(_loggerMock.Object, _databaseOperationMock.Object);
            await sut.Create(expectedTournament);
            _databaseOperationMock.Verify(x => x.ExecuteAsync(It.IsAny<string>()), Times.Once);
        }
        [TestMethod]
        public async Task UpdateTest()
        {
            var updateTournament = _fixture.Create<Tournament>();
            var sut = new TournamentsController(_loggerMock.Object, _databaseOperationMock.Object);
            await sut.Update(updateTournament);
            _databaseOperationMock.Verify(x => x.ExecuteAsync(It.IsAny<string>()), Times.Once);
        }
        [TestMethod]
        public async Task EditTournamentTest()
        {
            var tournament = new Tournament
            {
                Id = 1,
                Name = "UpdatedName",
                Prize = 10.20,
                Organiser = "UpdatedOrganiser",
                Format = "UpdatedFormat"
            };

            var _tournamentsController = new TournamentsController(_loggerMock.Object, _databaseOperationMock.Object);
            await _tournamentsController.EditTournament(tournament);

            // Assert
            _databaseOperationMock.Verify(x =>
                x.ExecuteAsync($"update turnyras " +
                               $"set Name = '{tournament.Name}', Prize = '{tournament.Prize}'," +
                               $"Organiser = '{tournament.Organiser}', Format = '{tournament.Format}' where Id = {tournament.Id}"), Times.Once);
        }
        [TestMethod]
        public async Task DeleteTournamentTest()
        {
            var tournamentId = 1;

            var _tournamentsController = new TournamentsController(_loggerMock.Object, _databaseOperationMock.Object);
            await _tournamentsController.Delete(tournamentId);


            _databaseOperationMock.Verify(x =>
                x.ExecuteAsync($"delete kova from kova join turnyro_kova on kova.id = turnyro_kova.fk_kova join turnyras on turnyras.id = turnyro_kova.fk_turnyras where turnyras.id = {tournamentId}"), Times.Once);

            _databaseOperationMock.Verify(x =>
                x.ExecuteAsync($"delete turnyro_robotas from turnyro_robotas where fk_turnyras = {tournamentId}"), Times.Once);

            _databaseOperationMock.Verify(x =>
                x.ExecuteAsync($"delete turnyro_kova from turnyro_kova join turnyras on turnyras.id = turnyro_kova.fk_turnyras where turnyras.id = {tournamentId}"), Times.Once);

            _databaseOperationMock.Verify(x =>
                x.ExecuteAsync($"delete turnyras from turnyras where Id = {tournamentId}"), Times.Once);
        }
    }
}
