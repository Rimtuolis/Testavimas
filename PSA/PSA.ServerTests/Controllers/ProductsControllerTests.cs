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
    public class ProductsControllerTests
    {
        private readonly Mock<IDatabaseOperationsService> _databaseOperationMock;
        private readonly Mock<ILogger<ProductsController>> _loggerMock;
        private readonly Fixture _fixture = new Fixture();
        public ProductsControllerTests()
        {
            _databaseOperationMock = new Mock<IDatabaseOperationsService>();
            _loggerMock = new Mock<ILogger<ProductsController>>();
        }
        [TestMethod]
        public async Task GetProductListTest()
        {
            var expectedProductList = _fixture.Create<List<Shared.Product>>();
            _databaseOperationMock.Setup(x => x.ReadListAsync<Product>($"SELECT * FROM preke")).ReturnsAsync(expectedProductList);
            var sut = new ProductsController(_loggerMock.Object, _databaseOperationMock.Object);
            var output = await sut.Get();
            Assert.AreEqual(expectedProductList, output);
        }

        [TestMethod]
        public async Task GetProductStatsTest()
        {
            var expectedStats = _fixture.Create<ProductStats>();
            var sut = new ProductsController(_loggerMock.Object, _databaseOperationMock.Object);
            _databaseOperationMock.Setup(x => x.ReadItemAsync<ProductStats>($"select * from statistika where id_Statistika = {expectedStats.id_Statistika}")).ReturnsAsync(expectedStats);
            var output = await sut.GetProductStats(expectedStats.id_Statistika);
            Assert.AreEqual(expectedStats, output);
        }
        [TestMethod]
        public async Task GetByIdTest()
        {
            var expectedProduct = _fixture.Build<Shared.Product>().With(x => x.Id, 12).Create();
            _databaseOperationMock.Setup(x => x.ReadItemAsync<Product>($"SELECT * FROM preke where Id = {expectedProduct.Id}")).ReturnsAsync(expectedProduct);
            var sut = new ProductsController(_loggerMock.Object, _databaseOperationMock.Object);
            var outpuT = await sut.Get(expectedProduct.Id);
            Assert.AreEqual(expectedProduct, outpuT);

        }
        [TestMethod]
        public async Task CreateTest()
        {
            var expectedProduct = _fixture.Create<Product>();
            var sut = new ProductsController(_loggerMock.Object,_databaseOperationMock.Object);
            await sut.Create(expectedProduct);
            _databaseOperationMock.Verify(x => x.ExecuteAsync(It.IsAny<string>()), Times.Once);
        }
        [TestMethod]
        public async Task DeleteTest ()
        {
            var sut = new ProductsController(_loggerMock.Object, _databaseOperationMock.Object);
            await sut.Delete(5);
            _databaseOperationMock.Verify(x => x.ExecuteAsync(It.IsAny<string>()),Times.Once);
        }
        [TestMethod]
        public async Task UpdateTestWithPicture()
        {
            var updateProduct = _fixture.Create<Product>();
            var sut = new ProductsController(_loggerMock.Object, _databaseOperationMock.Object);
            await sut.Update(updateProduct);
            _databaseOperationMock.Verify(x => x.ExecuteAsync(It.IsAny<string>()), Times.Once);
        }
        [TestMethod]
        public async Task UpdateTestWithoutPicture()
        {
            var updateProduct = _fixture.Build<Product>().Without(x => x.Picture).Create();
            var sut = new ProductsController(_loggerMock.Object, _databaseOperationMock.Object);
            await sut.Update(updateProduct);
            _databaseOperationMock.Verify(x => x.ExecuteAsync(It.IsAny<string>()), Times.Once);
        }
        [TestMethod]
        public async Task GetCategoryTest()
        {
            var expectedProductList = _fixture.Create<List<Shared.Product>>();
            var expectedOutput = expectedProductList.FirstOrDefault();
            var list = new List<Product>() { expectedOutput };
            _databaseOperationMock.Setup(x => x.ReadListAsync<Product>($"SELECT * FROM preke WHERE `Category`={expectedOutput.Category}")).ReturnsAsync(list);
            var sut = new ProductsController(_loggerMock.Object, _databaseOperationMock.Object);
            var output = await sut.Get(expectedOutput.Category.ToString());
            Assert.AreEqual(list, output);
        }


    }
}