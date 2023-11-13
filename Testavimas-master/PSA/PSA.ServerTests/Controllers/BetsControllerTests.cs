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
    public class BetsControllerTests
    {
        private readonly Mock<IDatabaseOperationsService> _databaseOperationMock;
        private readonly Mock<ICurrentUserService> _currentUserMock;
        private readonly Mock<ILogger<BetsController>> _loggerMock;
        private readonly Fixture _fixture = new Fixture();
        public BetsControllerTests()
        {
            _databaseOperationMock = new Mock<IDatabaseOperationsService>();
            _currentUserMock = new Mock<ICurrentUserService>();
            _loggerMock = new Mock<ILogger<BetsController>>();
        }
        [TestMethod]
        public async Task GetActiveAllBets()
        {
            var expectedBetsList = _fixture.Create<List<Shared.Bet>>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<Bet>($"SELECT * FROM statymas where state = 1")).ReturnsAsync(expectedBetsList);
            var sut = new BetsController(_loggerMock.Object, _currentUserMock.Object, _databaseOperationMock.Object);
            var output = await sut.GetActiveAllBets();
            Assert.AreEqual(expectedBetsList, output);
        }
        [TestMethod]
        public async Task GetUserBetsTest()
        {
            var expectedBetsList = _fixture.Create<List<Shared.Bet>>();
            var userId = 1;
            var currentUser = new CurrentUser { Id = userId };
            _currentUserMock.Setup(x => x.GetUser()).Returns(currentUser);

            _databaseOperationMock.Setup(x => x.ReadListAsync<Bet>($"SELECT * FROM statymas where fk_user_id = {userId} and state = 1")).ReturnsAsync(expectedBetsList);
            var sut = new BetsController(_loggerMock.Object, _currentUserMock.Object, _databaseOperationMock.Object);
            var output = await sut.Get();

            Assert.IsNotNull(output);
            Assert.AreEqual(expectedBetsList, output);
        }
        [TestMethod]
        public async Task GetUserHistoryTest()
        {
            var expectedBetsList = _fixture.Create<List<Shared.Bet>>();
            var userId = 1;
            var currentUser = new CurrentUser { Id = userId };
            _currentUserMock.Setup(x => x.GetUser()).Returns(currentUser);

            _databaseOperationMock.Setup(x => x.ReadListAsync<Bet>($"SELECT * FROM statymas where fk_user_id = {userId} and state = 2")).ReturnsAsync(expectedBetsList);
            var sut = new BetsController(_loggerMock.Object, _currentUserMock.Object, _databaseOperationMock.Object);

            var output = await sut.GetHistory();
            Assert.IsNotNull(output);
            Assert.AreEqual(expectedBetsList, output);
        }
        [TestMethod]
        public async Task GetByIdTest()
        {
            var expectedBet = _fixture.Build<Shared.Bet>().With(x => x.Id, 1).Create();
            _databaseOperationMock.Setup(x => x.ReadItemAsync<Bet>($"SELECT * FROM statymas where Id = {expectedBet.Id}")).ReturnsAsync(expectedBet);
            var sut = new BetsController(_loggerMock.Object, _currentUserMock.Object, _databaseOperationMock.Object);
            var outpuT = await sut.Get(expectedBet.Id);
            Assert.AreEqual(expectedBet, outpuT);

        }
        [TestMethod]
        public async Task CreateTest()
        {
            var expectedBet = _fixture.Create<Bet>();
            var sut = new BetsController(_loggerMock.Object, _currentUserMock.Object, _databaseOperationMock.Object);
            await sut.Create(expectedBet);
            _databaseOperationMock.Verify(x => x.ExecuteAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
