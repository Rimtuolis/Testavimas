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
    public class CardControllerTests
    {

        private readonly Mock<IDatabaseOperationsService> _databaseOperationMock;
        private readonly Mock<ILogger<AuthController>> _loggerMock;
        private readonly Mock<ICurrentUserService> _currentUserMock;
        private readonly Fixture _fixture = new Fixture();
        public CardControllerTests()
        {
            _databaseOperationMock = new Mock<IDatabaseOperationsService>();
            _currentUserMock = new Mock<ICurrentUserService>();
            _loggerMock = new Mock<ILogger<AuthController>>();
        }
        [TestMethod]
        public async Task GetCards_ShouldReturnSwipeCards()
        {
            var userId = 1;
            var currentUser = new CurrentUser { Id = userId };
            _currentUserMock.Setup(x => x.GetUser()).Returns(currentUser);
            var cards = new List<SwipeCard>();

            _databaseOperationMock.Setup(x =>
                    x.ReadListAsync<SwipeCard>($"SELECT * FROM card where fk_robot not in (" +
                                              $"SELECT Id from robotas where fk_user_id = {userId}); ")).ReturnsAsync(cards);

            var _cardController = new CardController(_databaseOperationMock.Object, _currentUserMock.Object, _loggerMock.Object);
            var result = await _cardController.GetCards();

            Assert.AreEqual(cards, result);
        }
        [TestMethod]
        public async Task GetExistingCardTest()
        {
            // Arrange
            var robotId = 123;
            var expectedCard = new SwipeCard
            {
                Id = 1,
                fk_robot = robotId,
                ImageUrl = "https://example.com/image.jpg",
                Description = "Sample Description"
            };

            _databaseOperationMock.Setup(x => x.ReadItemAsync<long>($"select COUNT(*) from card where fk_robot = {robotId}"))
                .ReturnsAsync(1);
            _databaseOperationMock.Setup(x => x.ReadItemAsync<SwipeCard>($"select * from card where fk_robot = {robotId}"))
                .ReturnsAsync(expectedCard);

            var _cardController = new CardController(_databaseOperationMock.Object, _currentUserMock.Object, _loggerMock.Object);
            var result = await _cardController.GetCard(robotId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(SwipeCard));
            Assert.AreEqual(expectedCard, result);
        }
        [TestMethod]
        public async Task GetNonExistingCardTest()
        {
            var robotId = 456;

            _databaseOperationMock.Setup(x => x.ReadItemAsync<long>($"select COUNT(*) from card where fk_robot = {robotId}"))
                .ReturnsAsync(0);

            var _cardController = new CardController(_databaseOperationMock.Object, _currentUserMock.Object, _loggerMock.Object);
            var result = await _cardController.GetCard(robotId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(SwipeCard));
            Assert.AreEqual("Empty", result.Description);
        }
        [TestMethod]
        public async Task SwipeCardTest_NonExisting()
        {
            var id = 1;
            var card = new SwipeCard { fk_robot = 2 };

            _databaseOperationMock.Setup(x => x.ReadItemAsync<long>($"select COUNT(*) from matches where fk_robot_first = {id} AND fk_robot_second = {card.fk_robot}"))
                .ReturnsAsync(0);
            _databaseOperationMock.Setup(x => x.ReadItemAsync<long>($"select COUNT(*) from matches where fk_robot_first = {card.fk_robot} AND fk_robot_second = {id}"))
                .ReturnsAsync(0);

            var _cardController = new CardController(_databaseOperationMock.Object, _currentUserMock.Object, _loggerMock.Object);
            var result = await _cardController.SwipeCard(id, card);

            Assert.IsFalse(result);
            _databaseOperationMock.Verify(x => x.ExecuteAsync($"insert into matches (fk_robot_first, fk_robot_second) values({id}, {card.fk_robot})"), Times.Once);
        }
        public async Task SwipeCardTest_Existing_DeletesMatch()
        {
            var id = 1;
            var card = new SwipeCard { fk_robot = 2 };

            _databaseOperationMock.Setup(x => x.ReadItemAsync<long>($"select COUNT(*) from matches where fk_robot_first = {id} AND fk_robot_second = {card.fk_robot}"))
                .ReturnsAsync(1);
            _databaseOperationMock.Setup(x => x.ReadItemAsync<long>($"select COUNT(*) from matches where fk_robot_first = {card.fk_robot} AND fk_robot_second = {id}"))
                .ReturnsAsync(0);

            var _cardController = new CardController(_databaseOperationMock.Object, _currentUserMock.Object, _loggerMock.Object);
            var result = await _cardController.SwipeCard(id, card);

            _databaseOperationMock.Verify(x => x.ExecuteAsync($"delete from matches where fk_robot_first = {card.fk_robot} AND fk_robot_second = {id}"), Times.Once);
            Assert.IsTrue(result);
        }
        public async Task SwipeCardTest_NonExisting_DoesNotCreateMatch()
        {
            var id = 1;
            var card = new SwipeCard { fk_robot = 2 };

            _databaseOperationMock.Setup(x => x.ReadItemAsync<long>($"select COUNT(*) from matches where fk_robot_first = {id} AND fk_robot_second = {card.fk_robot}"))
                .ReturnsAsync(0);
            _databaseOperationMock.Setup(x => x.ReadItemAsync<long>($"select COUNT(*) from matches where fk_robot_first = {card.fk_robot} AND fk_robot_second = {id}"))
                .ReturnsAsync(1);

            var _cardController = new CardController(_databaseOperationMock.Object, _currentUserMock.Object, _loggerMock.Object);
            var result = await _cardController.SwipeCard(id, card);

            Assert.IsFalse(result);
            _databaseOperationMock.Verify(x => x.ExecuteAsync(It.IsAny<string>()), Times.Never);
        }
        [TestMethod]
        public async Task AddCardTest()
        {
            var robot = new RobotDto
            {
                Id = 1,
                Nickname = "Robot1"
            };

            var _cardController = new CardController(_databaseOperationMock.Object, _currentUserMock.Object, _loggerMock.Object);
            await _cardController.AddCard(robot);

            _databaseOperationMock.Verify(x => x.ExecuteAsync(It.IsAny<string>()), Times.Once);
        }
        [TestMethod]
        public async Task EditCardTest()
        {
            var card = new SwipeCard
            {
                Id = 1,
                ImageUrl = "https://example.com/image.jpg",
                Description = "Updated Description"
            };

            var _cardController = new CardController(_databaseOperationMock.Object, _currentUserMock.Object, _loggerMock.Object);
            await _cardController.EditCard(card);

            _databaseOperationMock.Verify(x => x.ExecuteAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
