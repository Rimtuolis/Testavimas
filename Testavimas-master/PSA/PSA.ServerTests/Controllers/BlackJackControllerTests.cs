using AutoFixture;
using AutoFixture.Kernel;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeKit.Cryptography;
using Moq;
using Org.BouncyCastle.Bcpg.OpenPgp;
using PSA.Client.Pages.Gambling;
using PSA.Server.Controllers;
using PSA.Server.Services;
using PSA.Services;
using PSA.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PSA.Server.Controllers.Tests
{
    [TestClass()]
    public class BlackJackControllerTests
    {
        private readonly Mock<IDatabaseOperationsService> _mockDatabaseOperations;
        private readonly Mock<ICurrentUserService> _mockUserService;
        private readonly Mock<ILogger<AuthController>> _loggerMock;
        private readonly Mock<IBlackJackService> _mockBlackJackService;
        private Fixture _fixture = new Fixture();
        public BlackJackControllerTests()
        {
            _mockBlackJackService = new Mock<IBlackJackService>();
            _mockUserService = new Mock<ICurrentUserService>();
            _loggerMock = new Mock<ILogger<AuthController>>();
            _mockDatabaseOperations = new Mock<IDatabaseOperationsService>();
        }
        [TestMethod()]
        public void getGameSettingsTestDtoNotNull()
        {
            //Arrange
            var currentUserMock = _fixture.Create<CurrentUser>();
            _mockUserService.Setup(x => x.GetUser()).Returns(currentUserMock);
            var pCardsFixture = _fixture.CreateMany<Card>().ToList();
            var deckCardsFixture = _fixture.CreateMany<Card>().ToList();
            var dealerCardsFixture = _fixture.CreateMany<Card>().ToList();
            var hiddenCard = _fixture.Create<Card>();
            var blackJackDtoFixture = _fixture.Build<BlackJackDTO>().With(x => x.playerCards, JsonSerializer.Serialize(pCardsFixture))
                .With(y => y.dealerCards, JsonSerializer.Serialize(deckCardsFixture))
                .With(c => c.deck, JsonSerializer.Serialize(dealerCardsFixture))
                .With(v => v.hiddenCard, JsonSerializer.Serialize(hiddenCard))
                .Create();

            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<BlackJackDTO>($"select * from blackjack where fk_user={currentUserMock.Id} and gameState=True order by date desc limit 1")).ReturnsAsync(blackJackDtoFixture);

            var playerCards = JsonSerializer.Deserialize<List<Card>>(blackJackDtoFixture.playerCards);

            var dealerCards = JsonSerializer.Deserialize<List<Card>>(blackJackDtoFixture.dealerCards);

            var deck = JsonSerializer.Deserialize<List<Card>>(blackJackDtoFixture.deck);

            var hidden = JsonSerializer.Deserialize<Card>(blackJackDtoFixture.hiddenCard);

            var blackJack = new Shared.BlackJack(blackJackDtoFixture.Id, blackJackDtoFixture.betAmount, playerCards, dealerCards, deck, hidden, blackJackDtoFixture.tick, blackJackDtoFixture.gameState, blackJackDtoFixture.date, blackJackDtoFixture.fk_user);

            _mockBlackJackService.Setup(x => x.SetService(blackJack));

            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            //Act
            var result = sut.getGameSettings();
            //Assert
            _mockBlackJackService.Verify(x => x.SetService(It.IsAny<Shared.BlackJack>()), Times.Once);
        }
        [TestMethod()]
        public void getGameSettingsTestDtoIsNull()
        {

            //Arrange
            var currentUserMock = _fixture.Create<CurrentUser>();
            _mockUserService.Setup(x => x.GetUser()).Returns(currentUserMock);
            var pCardsFixture = _fixture.CreateMany<Card>().ToList();
            var deckCardsFixture = _fixture.CreateMany<Card>().ToList();
            var dealerCardsFixture = _fixture.CreateMany<Card>().ToList();
            var hiddenCard = _fixture.Create<Card>();
            var ticksTest = _fixture.Create<int>();
            bool gameStateTest = true;
            double betAmountTest = _fixture.Create<double>();
            var index = _fixture.Create<int>();

            _mockBlackJackService.Setup(x => x.GetDeck()).Returns(deckCardsFixture);
            _mockBlackJackService.Setup(x => x.GetPlayerCards()).Returns(pCardsFixture);
            _mockBlackJackService.Setup(x => x.GetDealerCards()).Returns(dealerCardsFixture);
            _mockBlackJackService.Setup(x => x.GetbetAmount()).Returns(betAmountTest);
            _mockBlackJackService.Setup(x => x.GetTick()).Returns(ticksTest);
            _mockBlackJackService.Setup(x => x.GetHiddenCard()).Returns(hiddenCard);


            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<BlackJackDTO?>($"select * from blackjack where fk_user={currentUserMock.Id} and gameState=True order by date desc limit 1")).ReturnsAsync(new BlackJackDTO() { });


            var blackJack = new Shared.BlackJack(0, betAmountTest, pCardsFixture, dealerCardsFixture, deckCardsFixture, hiddenCard, ticksTest, gameStateTest, DateTime.UtcNow, currentUserMock.Id);
            string jsonDeck = JsonSerializer.Serialize(deckCardsFixture);
            string jsonPlayerCards = JsonSerializer.Serialize(pCardsFixture);
            string jsonDealerCards = JsonSerializer.Serialize(dealerCardsFixture);
            string jsonHidden = JsonSerializer.Serialize(hiddenCard);

            _mockDatabaseOperations.Setup(o => o.ExecuteAsync($"insert into blackjack(betAmount, playerCards, dealerCards, deck, hiddenCard, tick, gameState, fk_user) values({betAmountTest}, '{jsonPlayerCards}', '{jsonDealerCards}', " +
                $"'{jsonDeck}', '{jsonHidden}', {ticksTest}, {gameStateTest}, {currentUserMock.Id})"));

            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<int>($"select Id from blackjack where fk_user={currentUserMock.Id} and gameState=True order by date desc limit 1")).ReturnsAsync(index);
            blackJack.Id = index;
            //Act

            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            var result = sut.getGameSettings();
            //Assert
            _mockDatabaseOperations.Verify(x => x.ExecuteAsync(It.IsAny<string>()), Times.Once);
            _mockDatabaseOperations.Verify(x => x.ReadItemAsync<int>(It.IsAny<string>()), Times.Once);
            _mockDatabaseOperations.Verify(x => x.ReadItemAsync<BlackJackDTO>(It.IsAny<string>()), Times.Once);
            Assert.AreEqual(blackJack.betAmount, result.Result.betAmount);
            Assert.AreEqual(blackJack.playerCards, result.Result.playerCards);
            Assert.AreEqual(blackJack.dealerCards, result.Result.dealerCards);
            Assert.AreEqual(blackJack.deck, result.Result.deck);
            Assert.AreEqual(blackJack.tick, result.Result.tick);
            Assert.AreEqual(blackJack.hiddenCard, result.Result.hiddenCard);
        }

        [TestMethod()]
        public void GetDeckTestFirstTrue()
        {
            var cardsTest = _fixture.CreateMany<Card>().ToList();
            _mockBlackJackService.Setup(o => o.GetDeck()).Returns(cardsTest);
            _mockBlackJackService.Setup(o => o.GetState()).Returns(true);
            //Act
            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            var result = sut.GetDeck();
            //Assert
            Assert.AreEqual(cardsTest, result.Result);
        }
        [TestMethod()]
        public void GetDeckTestFirstFalse()
        {
            var currentUserMock = _fixture.Create<CurrentUser>();
            _mockUserService.Setup(x => x.GetUser()).Returns(currentUserMock);
            var cardsTest = _fixture.CreateMany<Card>().ToList();
            var dealerTest = _fixture.CreateMany<Card>().ToList();
            var hiddenTest = _fixture.Create<Card>();
            var tick = _fixture.Create<int>();
            _mockBlackJackService.Setup(o => o.GetDeck()).Returns(cardsTest);
            _mockBlackJackService.Setup(o => o.GetState()).Returns(false);
            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<bool?>($"select gameState from blackjack where fk_user={currentUserMock.Id} order by date desc limit 1")).ReturnsAsync(true);
            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<List<Card>>($"select deck from blackjack where fk_user={currentUserMock.Id} order by date desc limit 1")).ReturnsAsync(cardsTest);
            string json = JsonSerializer.Serialize(cardsTest);
            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<string?>($"select playerCards from blackjack where fk_user={currentUserMock.Id} order by date desc limit 1")).ReturnsAsync(json);
            var playerCards = JsonSerializer.Deserialize<List<Card>>(json);
            json = JsonSerializer.Serialize(dealerTest);
            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<string?>($"select dealerCards from blackjack where fk_user={currentUserMock.Id} order by date desc limit 1")).ReturnsAsync(json);
            var dealerCards = JsonSerializer.Deserialize<List<Card>>(json);
            json = JsonSerializer.Serialize(hiddenTest);
            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<string?>($"select hiddenCard from blackjack where fk_user={currentUserMock.Id} order by date desc limit 1")).ReturnsAsync(json);
            var hiddenCard = JsonSerializer.Deserialize<Card>(json);
            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<int>($"select tick from blackjack where fk_user={currentUserMock.Id} order by date desc limit 1")).ReturnsAsync(tick);
            //Act
            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            var result = sut.GetDeck();
            //Assert
            Assert.AreEqual(cardsTest, result.Result);
        }
        [TestMethod()]
        public void GetDeckTestFirstFalseSecondFalse()
        {
            var currentUserMock = _fixture.Create<CurrentUser>();
            _mockUserService.Setup(x => x.GetUser()).Returns(currentUserMock);
            var cardsTest = _fixture.CreateMany<Card>().ToList();
            _mockBlackJackService.Setup(o => o.GetDeck()).Returns(cardsTest);
            _mockBlackJackService.Setup(o => o.GetState()).Returns(false);
            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<bool>($"select gameState from blackjack where fk_user={currentUserMock.Id} order by date desc limit 1")).ReturnsAsync(true);
            string json = JsonSerializer.Serialize(cardsTest);
            _mockDatabaseOperations.Setup(o => o.ExecuteAsync($"insert into blackjack(deck, fk_user) values('{json}', {currentUserMock.Id})"));
            //Act
            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            var result = sut.GetDeck();
            //Assert
            _mockDatabaseOperations.Verify(x => x.ExecuteAsync($"insert into blackjack(deck, fk_user) values('{json}', {currentUserMock.Id})"), Times.Once);
            Assert.AreEqual(cardsTest, result.Result);
        }

        [TestMethod()]
        public void HitTest()
        {
            //Arrange
            var cardsTest = _fixture.CreateMany<Card>().ToList();
            _mockBlackJackService.Setup(o => o.Hit()).Returns(cardsTest);
            string jsonCards = JsonSerializer.Serialize(cardsTest);
            var deckTest = _fixture.CreateMany<Card>().ToList();
            _mockBlackJackService.Setup(o => o.GetDeck()).Returns(deckTest);
            string jsonDeck = JsonSerializer.Serialize(deckTest);
            _mockBlackJackService.Setup(o => o.GetTick()).Returns(3);

            _mockDatabaseOperations.Setup(o => o.ExecuteAsync($"update blackjack set dealerCards = '{jsonCards}', deck = '{jsonDeck}', tick = {3} where Id={1}"));

            //Act

            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            var result = sut.Hit(1);

            //Assert
            Assert.AreEqual(cardsTest, result.Result);
        }

        [TestMethod()]
        public void HitDealerTest()
        {


            //Arrange
            var cardsTest = _fixture.CreateMany<Card>().ToList();
            _mockBlackJackService.Setup(o => o.HitDealer()).Returns(cardsTest);
            string jsonCards = JsonSerializer.Serialize(cardsTest);
            var deckTest = _fixture.CreateMany<Card>().ToList();
            _mockBlackJackService.Setup(o => o.GetDeck()).Returns(deckTest);
            string jsonDeck = JsonSerializer.Serialize(deckTest);
            _mockBlackJackService.Setup(o => o.GetTick()).Returns(3);

            _mockDatabaseOperations.Setup(o => o.ExecuteAsync($"update blackjack set dealerCards = '{jsonCards}', deck = '{jsonDeck}', tick = {3} where Id={1}"));
            
            //Act

            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            var result = sut.HitDealer(1);

            //Assert
            Assert.AreEqual(cardsTest, result.Result);

        }

        [TestMethod()]
        public void GetPlayerCardsTestFirstTrue()
        {
            var cardsTest = _fixture.CreateMany<Card>().ToList();
            _mockBlackJackService.Setup(o => o.GetPlayerCards()).Returns(cardsTest);
            _mockBlackJackService.Setup(o => o.GetState()).Returns(true);
            //Act
            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            var result = sut.GetPlayerCards();
            //Assert
            Assert.AreEqual(cardsTest, result.Result);
        }
        [TestMethod()]
        public void GetPlayerCardsTestFalseSecondTrue()
        {
            var currentUserMock = _fixture.Create<CurrentUser>();
            _mockUserService.Setup(x => x.GetUser()).Returns(currentUserMock);
            var cardsTest = _fixture.CreateMany<Card>().ToList();
            _mockBlackJackService.Setup(o => o.GetPlayerCards()).Returns(cardsTest);
            _mockBlackJackService.Setup(o => o.GetState()).Returns(false);
            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<bool?>($"select gameState from blackjack where fk_user={currentUserMock.Id} order by date desc limit 1")).ReturnsAsync(true);
            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<List<Card>>($"select playerCards from blackjack where fk_user={currentUserMock.Id} order by date desc limit 1")).ReturnsAsync(cardsTest);
            //Act
            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            var result = sut.GetPlayerCards();
            //Assert
            Assert.AreEqual(cardsTest, result.Result);
        }
        [TestMethod()]
        public void GetPlayerCardsTestFalseSecondFalse()
        {
            var currentUserMock = _fixture.Create<CurrentUser>();
            _mockUserService.Setup(x => x.GetUser()).Returns(currentUserMock);
            var cardsTest = _fixture.CreateMany<Card>().ToList();
            _mockBlackJackService.Setup(o => o.GetPlayerCards()).Returns(cardsTest);
            _mockBlackJackService.Setup(o => o.GetState()).Returns(false);
            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<bool>($"select gameState from blackjack where fk_user={currentUserMock.Id} order by date desc limit 1")).ReturnsAsync(true);
            string json = JsonSerializer.Serialize(cardsTest);
            _mockDatabaseOperations.Setup(o => o.ExecuteAsync($"update blackjack set playerCards = '{json}' where fk_user={currentUserMock.Id} and gameState=True order by date desc limit 1"));
            //Act
            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            var result = sut.GetPlayerCards();
            //Assert
            _mockDatabaseOperations.Verify(x => x.ExecuteAsync($"update blackjack set playerCards = '{json}' where fk_user={currentUserMock.Id} and gameState=True order by date desc limit 1"), Times.Once);
            Assert.AreEqual(cardsTest, result.Result);
        }

        [TestMethod()]
        public void GetDealerCardsTestFirstTrue()
        {
            //Arrange
            var cardsTest = _fixture.CreateMany<Card>().ToList();
            _mockBlackJackService.Setup(o => o.GetDealerCards()).Returns(cardsTest);
            _mockBlackJackService.Setup(o => o.GetState()).Returns(true);
            //Act
            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            var result = sut.GetDealerCards();
            //Assert
            Assert.AreEqual(cardsTest,result.Result);
        }
        [TestMethod()]
        public void GetDealerCardsTestFirstFalseSecondTrue()
        {
            //Arrange
            var currentUserMock = _fixture.Create<CurrentUser>();
            _mockUserService.Setup(x => x.GetUser()).Returns(currentUserMock);
            var cardsTest = _fixture.CreateMany<Card>().ToList();
            _mockBlackJackService.Setup(o => o.GetDealerCards()).Returns(cardsTest);
            _mockBlackJackService.Setup(o => o.GetState()).Returns(false);

            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<bool?>($"select gameState from blackjack where fk_user={currentUserMock.Id} order by date desc limit 1")).ReturnsAsync(true);
            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<List<Card>>($"select dealerCards from blackjack where fk_user={currentUserMock.Id} order by date desc limit 1")).ReturnsAsync(cardsTest);
            //Act
            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            var result = sut.GetDealerCards();
            //Assert
            Assert.AreEqual(cardsTest, result.Result);
        }
        [TestMethod()]
        public void GetDealerCardsTestFirstFalseSecondFalse()
        {
            //Arrange
            var currentUserMock = _fixture.Create<CurrentUser>();
            _mockUserService.Setup(x => x.GetUser()).Returns(currentUserMock);
            var cardsTest = _fixture.CreateMany<Card>().ToList();
            _mockBlackJackService.Setup(o => o.GetDealerCards()).Returns(cardsTest);
            _mockBlackJackService.Setup(o => o.GetState()).Returns(false);

            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<bool>($"select gameState from blackjack where fk_user={currentUserMock.Id} order by date desc limit 1")).ReturnsAsync(true);
            string json = JsonSerializer.Serialize(cardsTest);
            _mockDatabaseOperations.Setup(o => o.ExecuteAsync($"update blackjack set dealerCards = '{json}' where fk_user={currentUserMock.Id} and gameState=True order by date desc limit 1"));
            //Act
            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            var result = sut.GetDealerCards();
            //Assert
            _mockDatabaseOperations.Verify(x => x.ExecuteAsync($"update blackjack set dealerCards = '{json}' where fk_user={currentUserMock.Id} and gameState=True order by date desc limit 1"), Times.Once());
            Assert.AreEqual(cardsTest, result.Result);
        }

        [TestMethod()]
        public void GetGameStateTestByIdTrue()
        {
            bool test = true;
            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<bool?>($"select gameState from blackjack where Id={1}")).ReturnsAsync(test);
            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            var result = sut.GetGameState(1);
            Assert.AreEqual(test, result.Result);
        }
        [TestMethod()]
        public void GetGameStateTestByIdFalse()
        {
            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<bool>($"select gameState from blackjack where Id={1}")).ReturnsAsync(false); 
            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            var result = sut.GetGameState(1);
            Assert.AreEqual(false, result.Result);
        }

        [TestMethod()]
        public void GetGameStateTestTrue()
        {
            
            //Arrange
            var currentUserMock = _fixture.Create<CurrentUser>();
            _mockUserService.Setup(x => x.GetUser()).Returns(currentUserMock);
            var test = true;
            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<bool?>(It.IsAny<string>())).ReturnsAsync(test);
            
            //Act
            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            var result = sut.GetGameState();

            //Assert
            Assert.AreEqual(test,result.Result);
        }
        [TestMethod()]
        public void GetGameStateTestFalse()
        {

            //Arrange
            var currentUserMock = _fixture.Create<CurrentUser>();
            _mockUserService.Setup(x => x.GetUser()).Returns(currentUserMock);
            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<bool>(It.IsAny<string>())).ReturnsAsync(false);
            //Act
            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            var result = sut.GetGameState();

            //Assert
            Assert.AreEqual(false, result.Result);
        }
        [TestMethod()]
        public void SetGameStateTest()
        {
            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            sut.SetGameState(1,false);
            _mockBlackJackService.Verify(x => x.SetState(false), Times.Once());
        }
        [TestMethod()]
        public void GetbetAmountTestNotNull()
        {   //Arrange
            var currentUserMock = _fixture.Create<CurrentUser>();
            _mockUserService.Setup(x => x.GetUser()).Returns(currentUserMock);
            var expectedAmount = _fixture.Create<double>();
            _mockBlackJackService.Setup(x => x.GetbetAmount()).Returns(expectedAmount);
            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<double>($"select betAmount from blackjack where fk_user={currentUserMock.Id} and gameState=True order by date desc limit 1")).ReturnsAsync(expectedAmount);

            //Act
            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            var result = sut.GetbetAmount();

            //Assert
            Assert.AreEqual(expectedAmount, result.Result);
        }
        [TestMethod()]
        public void GetbetAmountTestIsNull()
        {   //Arrange
            var currentUserMock = _fixture.Create<CurrentUser>();
            _mockUserService.Setup(x => x.GetUser()).Returns(currentUserMock);
            var expectedAmount = _fixture.Create<double>();
            _mockBlackJackService.Setup(x => x.GetbetAmount()).Returns(expectedAmount);
            _mockDatabaseOperations.Setup(o => o.ReadItemAsync<double?>($"select betAmount from blackjack where fk_user={currentUserMock.Id} and gameState=True order by date desc limit 1")).ReturnsAsync(expectedAmount);

            //Act
            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            var result = sut.GetbetAmount();

            //Assert
            Assert.AreEqual(expectedAmount, result.Result);
        }

        [TestMethod()]
        public void SetbetAmountTest()
        {
            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            sut.SetbetAmount(55.5);
            _mockBlackJackService.Verify(x => x.SetbetAmount(55.5),Times.Once);
        }

        [TestMethod()]
        public void ResetDeckTest()
        {
            var tempUser = _fixture.Create<CurrentUser>();
            _mockBlackJackService.Setup(o => o.ResetDeck());
            _mockDatabaseOperations.Setup(o => o.ExecuteAsync($"update blackjack set gameState=False where Id={2}"));
            _mockDatabaseOperations.Setup(o => o.ExecuteAsync($"update user set balance = {tempUser.balance} where id_User = {2}"));

            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            sut.ResetDeck(2, tempUser);

            _mockDatabaseOperations.Verify(o => o.ExecuteAsync(It.IsAny<string>()), Times.Exactly(2));
            _mockBlackJackService.Verify(o => o.ResetDeck(),Times.Once);

        }

        [TestMethod()]
        public void GetTest()
        {
            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            var result = sut.Get(1);

            Assert.AreEqual("value",result);
        }

        [TestMethod()]
        public void PostTest()
        {
            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            sut.Post("Test");
            //Assert missing
        }

        [TestMethod()]
        public void PutTest()
        {
            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            sut.Put(1,"Test");
            //Assert missing
        }

        [TestMethod()]
        public void DeleteTest()
        {
            var sut = new BlackJackController(_mockDatabaseOperations.Object, _mockUserService.Object, _loggerMock.Object, _mockBlackJackService.Object);
            sut.Delete(1);
            //Assert missing
        }
    }
}