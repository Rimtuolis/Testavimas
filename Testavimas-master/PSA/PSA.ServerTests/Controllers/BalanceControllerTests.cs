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
    public class BalanceControllerTests
    {
        private readonly Mock<IDatabaseOperationsService> _databaseOperationMock;
        private readonly Mock<ICurrentUserService> _currentUserMock;
        private readonly Mock<ILogger<BalanceController>> _loggerMock;
        private readonly Fixture _fixture = new Fixture();

        public BalanceControllerTests()
        {
            _databaseOperationMock = new Mock<IDatabaseOperationsService>();
            _currentUserMock = new Mock<ICurrentUserService>();
            _loggerMock = new Mock<ILogger<BalanceController>>();
        }

        [TestMethod()]
        public void PutTest()
        {
            var currentUser = _fixture.Create<CurrentUser>();
            var id = currentUser.Id;
            _databaseOperationMock.Setup(x => x.ExecuteAsync($"update user set balance = '{currentUser.balance}' where id_User = {id}"));
            var controller = new BalanceController(_loggerMock.Object, _databaseOperationMock.Object, _currentUserMock.Object);
            controller.Put(id, currentUser);
            _currentUserMock.Verify(x => x.SetUser(currentUser), Times.Once);
        }
    }
}