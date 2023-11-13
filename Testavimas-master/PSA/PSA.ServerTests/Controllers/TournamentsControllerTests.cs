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
    }
}
