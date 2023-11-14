using AutoFixture;
using Microsoft.AspNetCore.Routing;
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
    public class ProfileControllerTests
    {
        private readonly Mock<IDatabaseOperationsService> _databaseOperationMock;
        private readonly Mock<ILogger<ProfileController>> _loggerMock;
        private readonly Mock<ICurrentUserService> _currentUserMock;
        private readonly Fixture _fixture = new Fixture();
        public ProfileControllerTests()
        {
            _databaseOperationMock = new Mock<IDatabaseOperationsService>();
            _loggerMock = new Mock<ILogger<ProfileController>>();
            _currentUserMock = new Mock<ICurrentUserService>();
        }
        [TestMethod]
        public async Task GetAllProfilesTest()
        {
            var profiles = new List<Profile>();
            _databaseOperationMock.Setup(x =>
                    x.ReadListAsync<Profile>($"select name, last_name, email, nickname from User")).ReturnsAsync(profiles);

            var _profileController = new ProfileController(_databaseOperationMock.Object, _currentUserMock.Object);
            var result = await _profileController.GetAll();
            Assert.AreEqual(profiles, result);
        }
        [TestMethod]
        public async Task Get_ShouldReturnProfileOrNull()
        {
            var profileValue = new Profile { nickname = "johnny" };

            _databaseOperationMock.Setup(x =>
                    x.ReadItemAsync<Profile>($"select name, last_name, email, nickname from user where nickname = 'johnny'"))
                .ReturnsAsync(new Profile { name = "John", last_name = "Doe", email = "john.doe@example.com", nickname = "johnny" });

            var _profileController = new ProfileController(_databaseOperationMock.Object, _currentUserMock.Object);
            var resultExisting = await _profileController.Get(profileValue);

            Assert.IsNotNull(resultExisting);
            Assert.AreEqual("johnny", resultExisting.nickname);

            _databaseOperationMock.Setup(x =>
                    x.ReadItemAsync<Profile>($"select name, last_name, email, nickname from user where nickname = 'johnny'"))
                .ReturnsAsync(null as Profile);

            var resultNull = await _profileController.Get(profileValue);

            Assert.IsNull(resultNull);
        }
        [TestMethod]
        public async Task EditProfileTest()
        {
            var profileCreationValue = new ProfileCreation
            {
                name = "John",
                last_name = "Doe",
                password = "newpassword",
                birthdate = "1990-01-01",
                city = "New York",
                email = "john.doe@example.com",
                post_code = "10001",
                nickname = "johnny"
            };

            var _profileController = new ProfileController(_databaseOperationMock.Object, _currentUserMock.Object);
            await _profileController.EditProfile(profileCreationValue);

            _databaseOperationMock.Verify(x =>
                x.ExecuteAsync($"UPDATE user SET name='John', last_name = 'Doe',  password = 'newpassword', " +
                               $"birthdate = '1990-01-01', city = 'New York', email = 'john.doe@example.com', post_code='10001' WHERE `nickname`='johnny'"), Times.Once);
        }
        [TestMethod]
        public async Task DeleteProfile_ShouldDeleteProfile()
        {
            var profileCreationValue = new ProfileCreation { nickname = "johnny" }; // Replace with the desired profile details for deletion

            var _profileController = new ProfileController(_databaseOperationMock.Object, _currentUserMock.Object);
            await _profileController.DeleteProfile(profileCreationValue);

            _databaseOperationMock.Verify(x =>
                x.ExecuteAsync($"DELETE FROM user WHERE nickname ='johnny'"), Times.Once);
        }
    }
}
