using AutoFixture;
using Microsoft.AspNetCore.Mvc;
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
    public class ClientControllerTests
    {
        private readonly Mock<IDatabaseOperationsService> _databaseOperationMock;
        private readonly Mock<ILogger<ClientController>> _loggerMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Fixture _fixture = new Fixture();

        public ClientControllerTests()
        {
            _databaseOperationMock = new Mock<IDatabaseOperationsService>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _loggerMock = new Mock<ILogger<ClientController>>();
        }
        [TestMethod]
        public async Task GetAllClientsTest()
        {
            var expectedClients = new List<Shared.Client>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<Shared.Client>($"select * from User")).ReturnsAsync(expectedClients);

            var sut = new ClientController(_databaseOperationMock.Object, _currentUserServiceMock.Object, _loggerMock.Object);
            var output = await sut.Get();

            Assert.AreEqual(expectedClients, output);
        }
        [TestMethod]
        public async Task GetByIdTest()
        {
            var id = 1;
            var expectedClient = new Shared.Client();
            _databaseOperationMock.Setup(x => x.ReadItemAsync<Shared.Client?>($"select * from User where id_User = {id}")).ReturnsAsync(expectedClient);

            var sut = new ClientController(_databaseOperationMock.Object, _currentUserServiceMock.Object, _loggerMock.Object);
            var outpuT = await sut.Get(id);

            Assert.AreEqual(expectedClient, outpuT);
        }
        [TestMethod]
        public async Task CreateClientTest()
        {
            var expectedClient = _fixture.Create<Shared.Client>();
            var sut = new ClientController(_databaseOperationMock.Object, _currentUserServiceMock.Object, _loggerMock.Object);
            await sut.Post(expectedClient);
            _databaseOperationMock.Verify(x => x.ExecuteAsync(It.IsAny<string>()), Times.Once);
        }
        [TestMethod]
        public async Task DeleteClientTest()
        {
            var clientId = 1;

            var _controller = new ClientController(_databaseOperationMock.Object, _currentUserServiceMock.Object, _loggerMock.Object);
            await _controller.Delete(clientId);

            _databaseOperationMock.Verify(x => x.ExecuteAsync($"delete from User where id_User = {clientId}"), Times.Once);
        }
    }
}
