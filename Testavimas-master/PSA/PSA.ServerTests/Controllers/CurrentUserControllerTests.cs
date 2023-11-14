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
    public class CurrentUserControllerTests
    {
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;

        public CurrentUserControllerTests()
        {
            _currentUserServiceMock = new Mock<ICurrentUserService>();
        }
        [TestMethod]
        public async Task GetCurrentUser()
        {
            var expectedUser = new CurrentUser
            {
                Id = 1,
                LoggedIn = true,
                UserLevel = AccessLevelType.CLIENT,
                Birthdate = DateTime.UtcNow,
                balance = 10.1
            };

            _currentUserServiceMock.Setup(x => x.GetUser()).Returns(expectedUser);

            CurrentUserController _controller = new CurrentUserController(_currentUserServiceMock.Object);
            var result = _controller.Get();

            Assert.AreEqual(expectedUser, result);
        }
    }
}
