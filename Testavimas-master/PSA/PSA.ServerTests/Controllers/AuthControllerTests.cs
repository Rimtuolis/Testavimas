using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSA.Server.Controllers;
using PSA.Server.Services;
using PSA.Services;
using PSA.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSA.Server.Services;
using Moq;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using AutoFixture;
using Blazorise;
using PSA.Client.Pages.Users;

namespace PSA.Server.Controllers.Tests
{
    [TestClass()]
    public class AuthControllerTests
    {
        private readonly Mock<IDatabaseOperationsService> _databaseOperationMock;
        private readonly Mock<ICurrentUserService> _currentUserMock;
        private readonly Mock<ILogger<AuthController>> _loggerMock;
        private readonly Fixture _fixture = new Fixture();
        public AuthControllerTests()
        {
            _databaseOperationMock = new Mock<IDatabaseOperationsService>();
            _currentUserMock = new Mock<ICurrentUserService>();
            _loggerMock = new Mock<ILogger<AuthController>>();
        }
        [TestMethod]
        public async Task LoginTest()
        {
            var validLoginDto = _fixture.Create<LoginDto>();
            var userMock = _fixture.Build<Shared.Client>().With(x => x.email, validLoginDto.Email).With(x => x.password, validLoginDto.Password).Create();
            _databaseOperationMock.Setup(x => x.ReadItemAsync<Shared.Client>($"select * from user where email = '{validLoginDto.Email}' and password = '{validLoginDto.Password}'")).ReturnsAsync(userMock);
            var sut = new AuthController(_databaseOperationMock.Object, _currentUserMock.Object, _loggerMock.Object); // Instantiate your controller with the required dependencies (e.g., _databaseOperationsService, _currentUserService).



            // Set up your mock or fake database service to return a client when the SQL query is executed.

            // Act
            var result = await sut.Login(validLoginDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(validLoginDto.Email, result.Email);
            Assert.AreEqual(validLoginDto.Password, result.Password);
            Assert.AreEqual(userMock.id_User, result.Id);
        }
        [TestMethod]
        public async Task Login_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            var controller = new AuthController(_databaseOperationMock.Object, _currentUserMock.Object, _loggerMock.Object); // Instantiate your controller with the required dependencies (e.g., _databaseOperationsService, _currentUserService).

            _databaseOperationMock.Setup(x => x.ReadItemAsync<Shared.Client>(It.IsAny<string>())).ReturnsAsync(new Shared.Client { });
            // Set up your mock or fake database service to return null when the SQL query is executed.
            var loginDto = _fixture.Build<LoginDto>().Without(x => x.Email).Without(x => x.Password).Create();
            // Act
            var result = await controller.Login(loginDto);

            // Assert
            Assert.IsNull(result);
        }
        [TestMethod]
        public async Task RegisterTest_Invalid()
        {
            Shared.Client client = null;
            _databaseOperationMock.Setup(x => x.ReadItemAsync<Shared.Client>(It.IsAny<string>())).ReturnsAsync(client);
            _databaseOperationMock.Setup(x => x.ExecuteAsync(It.IsAny<string>()));

            var controller = new AuthController(_databaseOperationMock.Object, _currentUserMock.Object, _loggerMock.Object);
            var outPut = await controller.Register(_fixture.Create<ProfileCreation>());

            Assert.IsNull(outPut);
        }
        [TestMethod]
        public async Task RegisterTest_Valid()
        {
            var input = _fixture.Build<ProfileCreation>().With(x => x.birthdate, DateTime.Now.ToString).With(x => x.post_code, "1234").Create();
            var expectedOutput = _fixture.Build<Shared.Client>().With(x => x.email, input.email)
                .With(x => x.password, input.password)
                .With(x => x.nickname, input.nickname)
                .With(x => x.birthdate, DateTime.Parse(input.birthdate))
                .With(x => x.city, input.city)
                .With(x => x.email, input.email)
                .With(x => x.last_name, input.last_name)
                .With(x => x.name, input.name)
                .With(x => x.post_code, int.Parse(input.post_code))
                .With(x => x.city, input.city)
                .With(x => x.password, input.password)
                .Create();
            _databaseOperationMock.Setup(x => x.ExecuteAsync($"INSERT INTO `User` (`name`, `last_name`, `nickname`, `password`, " +
                $"`birthdate`, `city`, `email`, `post_code`, balance, role) " +
                $"VALUES ('{input.name}', '{input.last_name}', '{input.nickname}', '{input.password}', " +
                $"'{input.birthdate}', '{input.city}', '{input.email}', '{input.post_code}', 0, {(int)AccessLevelType.CLIENT})"));
            _databaseOperationMock.Setup(x => x.ReadItemAsync<Shared.Client>(It.IsAny<string>())).ReturnsAsync(expectedOutput);


            var controller = new AuthController(_databaseOperationMock.Object, _currentUserMock.Object, _loggerMock.Object);
            var outPut = await controller.Register(input);

            Assert.IsNotNull(outPut);
            Assert.AreEqual(expectedOutput.id_User, outPut.Id);
            Assert.AreEqual(expectedOutput.nickname, outPut.Username);
            Assert.AreEqual(expectedOutput.email, outPut.Email);
            Assert.AreEqual(expectedOutput.balance, outPut.balance);
        }
        [TestMethod]
        public void LogOutTest()
        {
            var input = _fixture.Build<CurrentUser>().With(x => x.LoggedIn, true).Create();
            var controller = new AuthController(_databaseOperationMock.Object, _currentUserMock.Object, _loggerMock.Object);
            input = controller.LogOut();
            Assert.AreEqual(input.LoggedIn, false);
        }

    }
}