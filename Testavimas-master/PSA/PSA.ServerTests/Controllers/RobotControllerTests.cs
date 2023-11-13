using AutoFixture;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
    public class RobotControllerTests
    {
        private readonly Mock<IDatabaseOperationsService> _databaseOperationMock;
        private readonly Mock<ILogger<RobotsController>> _loggerMock;
        private Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Fixture _fixture = new Fixture();
        public RobotControllerTests()
        {
            _databaseOperationMock = new Mock<IDatabaseOperationsService>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _loggerMock = new Mock<ILogger<RobotsController>>();
        }
        [TestMethod]
        public async Task GetUserRobotsTest()
        {
            var userId = 1;
            var currentUser = new CurrentUser { Id = userId }; 
            _currentUserServiceMock.Setup(x => x.GetUser()).Returns(currentUser);

            var expectedRobots = new List<Robot>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<Robot>($"SELECT * FROM robotas WHERE fk_user_id = {userId}")).ReturnsAsync(expectedRobots);

            RobotsController _robotsController = new RobotsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserServiceMock.Object);
            var result = await _robotsController.Get();

            Assert.AreEqual(expectedRobots, result);
        }
        [TestMethod]
        public async Task GetAllRobotsTest()
        {
            var expectedRobots = _fixture.Create<List<Shared.Robot>>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<Robot>($"SELECT * FROM robotas")).ReturnsAsync(expectedRobots);
            var sut = new RobotsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserServiceMock.Object);
            var output = await sut.GetAllRobots();
            Assert.AreEqual(expectedRobots, output);
        }
        [TestMethod]
        public async Task GetByIdTest()
        {
            var robotId = 1;

            var expectedRobot = new Robot();
            _databaseOperationMock.Setup(x => x.ReadItemAsync<Robot?>($"SELECT * FROM robotas where Id = {robotId}")).ReturnsAsync(expectedRobot);

            RobotsController _robotsController = new RobotsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserServiceMock.Object);
            var result = await _robotsController.Get(robotId);

            Assert.AreEqual(expectedRobot, result);
        }
        [TestMethod]
        public async Task GetAllPartsTest()
        {
            var expectedRobotParts = new List<Robot>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<Robot>("SELECT * FROM preke")).ReturnsAsync(expectedRobotParts);

            RobotsController _robotsController = new RobotsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserServiceMock.Object);
            var result = await _robotsController.GetParts();

            Assert.AreEqual(expectedRobotParts, result);
        }
        [TestMethod]
        public async Task GetPartsByIdTest()
        {
            var partId = 1;

            var expectedPart = new Product();
            _databaseOperationMock.Setup(x => x.ReadItemAsync<Product?>($"SELECT * FROM preke WHERE id = {partId}")).ReturnsAsync(expectedPart);

            RobotsController _robotsController = new RobotsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserServiceMock.Object);
            var result = await _robotsController.GetPartsW(partId);

            Assert.AreEqual(expectedPart, result);
        }
        [TestMethod]
        public async Task GetMyselfTest()
        {
            var userId = 1;
            var currentUser = new CurrentUser { Id = userId };
            _currentUserServiceMock.Setup(x => x.GetUser()).Returns(currentUser);

            var expectedRobots = new List<Robot>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<Robot>($"SELECT * FROM robotas WHERE fk_user_id = {userId}")).ReturnsAsync(expectedRobots);

            RobotsController _robotsController = new RobotsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserServiceMock.Object);
            var result = await _robotsController.MySelf();


            Assert.AreEqual(expectedRobots, result);
        }
        [TestMethod]
        public async Task GetOpponentTest()
        {
            var userId = 1;
            var currentUser = new CurrentUser { Id = userId };
            _currentUserServiceMock.Setup(x => x.GetUser()).Returns(currentUser);

            var expectedRobots = new List<Robot>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<Robot>($"SELECT * FROM robotas WHERE fk_user_id != {userId}")).ReturnsAsync(expectedRobots);

            RobotsController _robotsController = new RobotsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserServiceMock.Object);
            var result = await _robotsController.GetOponent();


            Assert.AreEqual(expectedRobots, result);
        }
        [TestMethod]
        public async Task CreateRobotWithDetailsTest()
        {
            var robot = new Robot
            {
                Nickname = "TestRobot",
                Head = 1,
                Body = 2,
                RightArm = 3,
                LeftArm = 4,
                RightLeg = 5,
                LeftLeg = 6
            };

            _databaseOperationMock.Setup(x => x.ReadItemAsync<int?>("select max(Id) from robotas")).ReturnsAsync(1);
            var userId = 1;
            var currentUser = new CurrentUser { Id = userId };
            _currentUserServiceMock.Setup(x => x.GetUser()).Returns(currentUser);

            RobotsController _robotsController = new RobotsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserServiceMock.Object);
            await _robotsController.Create(robot);

            _databaseOperationMock.Verify(x => x.ExecuteAsync(It.IsAny<string>()), Times.Exactly(7));
        }
        [TestMethod]
        public async Task WinIncrementWinsAndLossesTest()
        {
            var robotId1 = 1;
            var robotId2 = 2;

            RobotsController _robotsController = new RobotsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserServiceMock.Object);
            await _robotsController.Win(robotId1, robotId2);

            _databaseOperationMock.Verify(x => x.ExecuteAsync($"update robotas set Wins = Wins + 1 WHERE Id  = {robotId1} "), Times.Once);
            _databaseOperationMock.Verify(x => x.ExecuteAsync($"update robotas set Losses = Losses + 1 WHERE Id  = {robotId2} "), Times.Once);
        }
        [TestMethod]
        public async Task DrawIncrementTest()
        {
            var robotId1 = 1;
            var robotId2 = 2;

            RobotsController _robotsController = new RobotsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserServiceMock.Object);
            await _robotsController.Tie(robotId1, robotId2);

            _databaseOperationMock.Verify(x => x.ExecuteAsync($"update robotas set Draws = Draws + 1 WHERE Id  = {robotId1} "), Times.Once);
            _databaseOperationMock.Verify(x => x.ExecuteAsync($"update robotas set Draws = Draws + 1 WHERE Id  = {robotId2} "), Times.Once);
        }
        [TestMethod]
        public async Task DeleteRobotTest()
        {
            var robotId = 1;

            RobotsController _robotsController = new RobotsController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserServiceMock.Object);
            await _robotsController.Delete(robotId);

            _databaseOperationMock.Verify(x => x.ExecuteAsync($"delete from robotas where Id = {robotId}"), Times.Once);
        }
    }
}
