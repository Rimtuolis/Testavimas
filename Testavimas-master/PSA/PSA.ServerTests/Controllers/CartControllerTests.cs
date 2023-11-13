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
    public class CartControllerTests
    {
        private readonly Mock<ICartService> _cartServiceMock;
        private readonly Fixture _fixture = new Fixture();
        public CartControllerTests()
        {
            _cartServiceMock = new Mock<ICartService>();

        }
        [TestMethod]
        public void GetCartTest()
        {
            var expectedCart = new List<Product>();
            _cartServiceMock.Setup(x => x.GetCart()).Returns(expectedCart);

            CartController _controller = new CartController(_cartServiceMock.Object);
            var result = _controller.GetCart();

            Assert.AreEqual(expectedCart, result);
        }
        [TestMethod]
        public void AddCartItemTest()
        {
            var productToAdd = new Product();

            CartController _controller = new CartController(_cartServiceMock.Object);
            _controller.AddCartItem(productToAdd);

            _cartServiceMock.Verify(x => x.AddProductToCart(productToAdd), Times.Once);
        }
        [TestMethod]
        public void DeleteCartItemQuantityTest()
        {
            var productToRemoveQuantity = new Product();

            CartController _controller = new CartController(_cartServiceMock.Object);
            _controller.DeleteCartItemQuantity(productToRemoveQuantity);

            _cartServiceMock.Verify(x => x.RemoveProductQuantityByOneFromCart(productToRemoveQuantity), Times.Once);
        }
        [TestMethod]
        public void DeleteCartItemTest()
        {
            var productToRemove = new Product();

            CartController _controller = new CartController(_cartServiceMock.Object);
            _controller.DeleteCartItem(productToRemove);

            _cartServiceMock.Verify(x => x.RemoveProductFromCart(productToRemove), Times.Once);
        }
    }
}
