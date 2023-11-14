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
    public class RobotPartControllerTests
    {

        private readonly Mock<IDatabaseOperationsService> _databaseOperationMock;
        private readonly Mock<ILogger<RobotPartController>> _loggerMock;
        private Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Fixture _fixture = new Fixture();
        public RobotPartControllerTests()
        {
            _databaseOperationMock = new Mock<IDatabaseOperationsService>();
            _loggerMock = new Mock<ILogger<RobotPartController>>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
        }
        [TestMethod]
        public async Task GetRobotPartsTest()
        {
            var expectedRobotParts = _fixture.Create<List<Shared.RobotPart>>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<RobotPart>($"SELECT * FROM roboto_detale")).ReturnsAsync(expectedRobotParts);
            var sut = new RobotPartController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserServiceMock.Object);
            var output = await sut.Get();
            Assert.AreEqual(expectedRobotParts, output);
        }
        [TestMethod]
        public async Task GetOpponentTest()
        {
            var userId = 1;
            var currentUser = new CurrentUser { Id = userId };
            _currentUserServiceMock.Setup(x => x.GetUser()).Returns(currentUser);

            var expectedRobots = new List<Robot>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<Robot>($"SELECT * FROM robotas WHERE fk_user_id != {userId}")).ReturnsAsync(expectedRobots);

            RobotPartController _robotsController = new RobotPartController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserServiceMock.Object);
            var result = await _robotsController.GetOponent();

            Assert.AreEqual(expectedRobots, result);
        }
        [TestMethod]
        public async Task UpdateDamageTest() 
        {
            var damage = 10;
            var robot = new Robot { Id = 1 };

            RobotPartController _robotsController = new RobotPartController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserServiceMock.Object);
            await _robotsController.UpdateDamage(damage, robot);

            _databaseOperationMock.Verify(x => x.ExecuteAsync($"update roboto_detale set durability = durability - {damage} WHERE fk_robotas  = {robot.Id} "), Times.Once);
        }
        [TestMethod]
        public async Task DeletePartTest()
        {
            var fightId = 1;

            RobotPartController _robotsController = new RobotPartController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserServiceMock.Object);
            await _robotsController.Delete(fightId);

            _databaseOperationMock.Verify(x => x.ExecuteAsync($"delete from 'turnyro_kova' where 'fk_kova' = {fightId}"), Times.Once);
            _databaseOperationMock.Verify(x => x.ExecuteAsync($"delete from 'kova' where 'id' = {fightId}"), Times.Once);
        }
    }
}
