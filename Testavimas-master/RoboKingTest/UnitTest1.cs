using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using PSA.Server.Controllers;
using PSA.Services;
using PSA.Shared;

namespace RoboKingTest
{
    [TestClass]
    public class UserControllerTests
    {
        [TestMethod]
        public async Task Login_ValidCredentials_ReturnsCurrentUser()
        {
            // Arrange
            var controller = new AuthController(); // Instantiate your controller with the required dependencies (e.g., _databaseOperationsService, _currentUserService).

            var validLoginDto = new LoginDto
            {
                Email = "valid@email.com",
                Password = "validPassword"
            };

            // Set up your mock or fake database service to return a client when the SQL query is executed.

            // Act
            var result = await controller.Login(validLoginDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("valid@email.com", result.Email);
            // Add more assertions to check other properties of the CurrentUser returned.
        }

        [TestMethod]
        public async Task Login_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            var controller = new UserController(); // Instantiate your controller with the required dependencies (e.g., _databaseOperationsService, _currentUserService).

            var invalidLoginDto = new LoginDto
            {
                Email = "nonexistent@email.com",
                Password = "invalidPassword"
            };

            // Set up your mock or fake database service to return null when the SQL query is executed.

            // Act
            var result = await controller.Login(invalidLoginDto);

            // Assert
            Assert.IsNull(result);
        }

    }
}
