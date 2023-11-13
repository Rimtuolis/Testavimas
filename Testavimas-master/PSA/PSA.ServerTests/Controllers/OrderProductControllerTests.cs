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
    public class OrderProductControllerTests
    {
        private readonly Mock<IDatabaseOperationsService> _databaseOperationMock;
        private readonly Mock<ILogger<OrderProductController>> _loggerMock;
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<ICurrentUserService> _currentUserMock;

        public OrderProductControllerTests()
        {
            _databaseOperationMock = new Mock<IDatabaseOperationsService>();
            _currentUserMock = new Mock<ICurrentUserService>();
            _loggerMock = new Mock<ILogger<OrderProductController>>();
        }
        [TestMethod]
        public async Task GetProductsByIdTest()
        {
            var orderId = 1;
            var expectedOrderProducts = new List<OrderProductDto>
            {
                new OrderProductDto { fk_uzsakymas = orderId, fk_preke = 101 },
                new OrderProductDto { fk_uzsakymas = orderId, fk_preke = 102 }
            };

            _databaseOperationMock.Setup(x => x.ReadListAsync<OrderProductDto>($"select * from prekes_uzsakymas where fk_uzsakymas = {orderId}"))
                .ReturnsAsync(expectedOrderProducts);

            var _controller = new OrderProductController(_databaseOperationMock.Object, _currentUserMock.Object,_loggerMock.Object);
            var result = await _controller.Get(orderId);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedOrderProducts, result);
        }
        [TestMethod]
        public async Task CreateNewOrderProductTest()
        {
            var orderProduct = new OrderProductDto { fk_uzsakymas = 1, fk_preke = 101 };

            var _controller = new OrderProductController(_databaseOperationMock.Object, _currentUserMock.Object, _loggerMock.Object);
            await _controller.Post(orderProduct);

            _databaseOperationMock.Verify(x => x.ExecuteAsync($"insert into prekes_uzsakymas(fk_uzsakymas, fk_preke) values({orderProduct.fk_uzsakymas}, {orderProduct.fk_preke})"), Times.Once);
        }
    }
}
