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
    public class BuilderControllerTests
    {
        private readonly Mock<IManualBuildingService> _buildingService;
        private readonly Mock<IDatabaseOperationsService> _databaseOperationMock;
        private readonly Fixture _fixture = new Fixture();

        public BuilderControllerTests()
        {
            _buildingService = new Mock<IManualBuildingService>();
            _databaseOperationMock = new Mock<IDatabaseOperationsService>();
        }
        /*[TestMethod]
        public async Task GetRobotTest()
        {
            var expectedOutput = _fixture.Build<Shared.Robot>().With(x => x.Id, 12).Create();
            _databaseOperationMock.Setup(x => x.ReadItemAsync<Robot>($"SELECT * FROM turnyras where Id = {expectedOutput.Id}")).ReturnsAsync(expectedOutput);
            var sut = new BuilderController(_buildingService.Object);
            var output = sut.GetRobot();
            Assert.AreEqual(expectedOutput, output);
        }*/
    }
}
