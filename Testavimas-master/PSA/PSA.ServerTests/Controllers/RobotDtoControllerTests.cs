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
    public class RobotDtoControllerTests
    {
        private readonly Mock<IDatabaseOperationsService> _databaseOperationMock;
        private readonly Mock<ILogger<RobotDtoController>> _loggerMock;
        private Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Fixture _fixture = new Fixture();
        public RobotDtoControllerTests()
        {
            _databaseOperationMock = new Mock<IDatabaseOperationsService>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _loggerMock = new Mock<ILogger<RobotDtoController>>();
        }
        [TestMethod]
        public async Task GetAllRobotDtoTest()
        {
            var userId = 1;
            var currentUser = new CurrentUser { Id = userId };
            _currentUserServiceMock.Setup(x => x.GetUser()).Returns(currentUser);

            var expectedRobots = new List<Robot>(); // Replace with your expected data
            _databaseOperationMock.Setup(x => x.ReadListAsync<Robot>($"SELECT * FROM robotas WHERE fk_user_id = {userId}")).ReturnsAsync(expectedRobots);

            RobotDtoController _robotsController = new RobotDtoController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserServiceMock.Object);
            var result = await _robotsController.Get();

            // Assert
            //Assert.IsNotNull(result);
            Assert.AreEqual(expectedRobots, result);
        }
        [TestMethod]
        public async Task GetRobotDtoByIdTest()
        {
            var robotId = 1;
            var expectedRobots = new List<RobotPart>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<RobotPart>($"SELECT * FROM roboto_detale where fk_robotas = {robotId}")).ReturnsAsync(expectedRobots);

            RobotDtoController _robotsController = new RobotDtoController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserServiceMock.Object);
            var result = await _robotsController.Get(robotId);

            Assert.AreEqual(expectedRobots, result);
        }
    }
}
