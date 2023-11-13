using AutoFixture;
using Blazorise;
using Blazorise.Bootstrap;
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
    public class OrderControllerTests
    {
        private readonly Mock<IDatabaseOperationsService> _databaseOperationMock;
        private readonly Mock<ILogger<OrdersController>> _loggerMock;
        private Mock<ICartService> _cartServiceMock;
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<ICurrentUserService> _currentUserMock;
        private readonly Mock<IMailService> _mailServiceMock;

        public OrderControllerTests()
        {
            _databaseOperationMock = new Mock<IDatabaseOperationsService>();
            _currentUserMock = new Mock<ICurrentUserService>();
            _cartServiceMock = new Mock<ICartService>();
            _loggerMock = new Mock<ILogger<OrdersController>>();
            _mailServiceMock = new Mock<IMailService>();
        }
        [TestMethod]
        public async Task GetOrdersByUserIdTest()
        {
            var userId = 1;
            var currentUser = new CurrentUser { Id = userId };
            _currentUserMock.Setup(x => x.GetUser()).Returns(currentUser);

            var expectedOrders = new List<OrderDto>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<OrderDto>($"SELECT * FROM uzsakymas WHERE fk_user_id={userId}"))
                .ReturnsAsync(expectedOrders);

            var _ordersController = new OrdersController(_databaseOperationMock.Object, _currentUserMock.Object, _loggerMock.Object, _cartServiceMock.Object, _mailServiceMock.Object);
            var result = await _ordersController.Get();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedOrders, result);
        }
        [TestMethod]
        public async Task GetOrdersByIdTest()
        {
            var orderId = 456;
            var expectedOrder = new OrderDto();
            _databaseOperationMock.Setup(x => x.ReadItemAsync<OrderDto>($"SELECT * FROM uzsakymas WHERE id_Uzsakymas={orderId}"))
                .ReturnsAsync(expectedOrder);

            var _ordersController = new OrdersController(_databaseOperationMock.Object, _currentUserMock.Object, _loggerMock.Object, _cartServiceMock.Object, _mailServiceMock.Object);
            var result = await _ordersController.Get(orderId);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedOrder, result);
        }
        [TestMethod]
        public async Task CreateNewOrderTest()
        {
            var total = 100.0;
            var userId = 1;
            var currentUser = new CurrentUser { Id = userId };
            _currentUserMock.Setup(x => x.GetUser()).Returns(currentUser);

            var cartItems = new List<Product>();
            _cartServiceMock.Setup(x => x.GetCart()).Returns(cartItems);

            _databaseOperationMock.Setup(x => x.ReadItemAsync<int?>("select max(id_Uzsakymas) from uzsakymas"))
                .ReturnsAsync((int?)null);
            _databaseOperationMock.Setup(x => x.ExecuteAsync(It.IsAny<string>()));

            var _ordersController = new OrdersController(_databaseOperationMock.Object, _currentUserMock.Object, _loggerMock.Object, _cartServiceMock.Object, _mailServiceMock.Object);
            await _ordersController.Post(total);

            _cartServiceMock.Verify(x => x.ClearCart(), Times.Once);
            _databaseOperationMock.Verify(x => x.ExecuteAsync(It.IsAny<string>()), Times.Once);
        }
        [TestMethod]
        public async Task UpdateOrderTest()
        {
            var order = new OrderDto();
            _databaseOperationMock.Setup(x => x.ExecuteAsync(It.IsAny<string>()));

            var _ordersController = new OrdersController(_databaseOperationMock.Object, _currentUserMock.Object, _loggerMock.Object, _cartServiceMock.Object, _mailServiceMock.Object);
            await _ordersController.Put(order);

            _databaseOperationMock.Verify(x => x.ExecuteAsync(It.IsAny<string>()), Times.Once);
        }
        [TestMethod]
        public async Task DeleteOrderTest()
        {
            var orderId = 789;
            _databaseOperationMock.Setup(x => x.ExecuteAsync(It.IsAny<string>()));

            var _ordersController = new OrdersController(_databaseOperationMock.Object, _currentUserMock.Object, _loggerMock.Object, _cartServiceMock.Object, _mailServiceMock.Object);
            await _ordersController.Delete(orderId);

            _databaseOperationMock.Verify(x => x.ExecuteAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
