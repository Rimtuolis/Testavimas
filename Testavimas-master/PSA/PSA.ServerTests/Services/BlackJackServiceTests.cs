using AutoFixture;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PSA.Client.Pages.Tournaments;
using PSA.Server.Controllers;
using PSA.Services;
using PSA.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.Server.Services.Tests
{
    [TestClass()]
    public class BlackJackServiceTests
    {
        private Mock<IBlackJackService> _blackJackServiceMock;
        private BlackJackService _blackJackService;
        public BlackJackServiceTests()
        {
            _blackJackServiceMock = new Mock<IBlackJackService>();
            _blackJackService = new BlackJackService();
        }
        [TestMethod]
        public void GetDeck_ShouldReturnDeck()
        {
            var expectedDeck = new List<Card>();

            _blackJackServiceMock.Setup(x => x.GetDeck()).Returns(expectedDeck);

            var result = _blackJackServiceMock.Object.GetDeck();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedDeck, result);
        }
        [TestMethod]
        public void ResetDeck_ShouldResetDeckState()
        {

            _blackJackService.GetDeck();
            _blackJackService.Hit();

            _blackJackService.ResetDeck();
            Assert.IsFalse(_blackJackService.deck.Any());
            Assert.IsFalse(_blackJackService.playerCards.Any());
            Assert.IsFalse(_blackJackService.dealerCards.Any());
            Assert.AreEqual(0, _blackJackService.betAmount);
            Assert.IsFalse(_blackJackService.Playing);
            Assert.AreEqual(0, _blackJackService.tick);
            Assert.IsNotNull(_blackJackService.hidden);
        }
        [TestMethod]
        public void Hit_ShouldAddCardToPlayerCards()
        {
            var initialPlayerCardsCount = _blackJackService.playerCards.Count;

            _blackJackService.GetDeck();
            var result = _blackJackService.Hit();

            Assert.AreEqual(initialPlayerCardsCount + 1, result.Count);
        }
        [TestMethod]
        public void Hit_ShouldChangeCardValue()
        {
            var initialPlayerCardsCount = _blackJackService.playerCards.Count;

            _blackJackService.GetDeck();
            _blackJackService.GetPlayerCards();
            Card temp = new Card("A","Spades",10);
            _blackJackService.playerCards.Add(temp);
            _blackJackService.playerCards.Add(temp);
            var initialSum = _blackJackService.playerCards.Sum(x => x.value);
            var result = _blackJackService.Hit();
            var resultSum = _blackJackService.playerCards.Sum(x => x.value);

            Assert.AreNotEqual(initialSum, resultSum);
        }
        [TestMethod]
        public void HitDealer_ShouldAddCardToDealerCards()
        {
            _blackJackService.GetDeck();
            Card temp = new Card("A", "Spades", 8);
            _blackJackService.dealerCards.Add(temp);
            _blackJackService.dealerCards.Add(temp);
            //_blackJackService.deck[0] = new Card("A", "Spades", 2);
            var initialDealerCardsCount = _blackJackService.dealerCards.Count;
            var result = _blackJackService.HitDealer();

            Assert.AreEqual(initialDealerCardsCount + 1, result.Count);
        }
        [TestMethod]
        public void HitDealer_ShouldNotAddCardToDealerCards()
        {
            Card temp = new Card("A", "Spades", 10);
            _blackJackService.dealerCards.Add(temp);
            _blackJackService.dealerCards.Add(temp);
            var initialDealerCardsCount = _blackJackService.dealerCards.Count;
            var result = _blackJackService.HitDealer();

            Assert.AreEqual(initialDealerCardsCount, result.Count);
        }
        [TestMethod]
        public void HitDealer_ShouldChangeCardValue()
        {
            var initialPlayerCardsCount = _blackJackService.playerCards.Count;

            _blackJackService.GetDeck();
            //_blackJackService.GetDealerCards();
            Card temp = new Card("A", "Spades", 7);
            _blackJackService.dealerCards.Add(temp);
            _blackJackService.dealerCards.Add(temp);
            _blackJackService.deck[0] = new Card("A", "Spades", 8);
            //_blackJackService.dealerCards.Add(temp);
            var initialSum = _blackJackService.dealerCards.Sum(x => x.value);
            var result = _blackJackService.HitDealer();
            var resultSum = _blackJackService.dealerCards.Sum(x => x.value);

            Assert.AreNotEqual(initialSum, resultSum);
        }
        [TestMethod]
        public void GetPlayerCards_ShouldReturnPlayerCards()
        {
            _blackJackService.GetDeck();
            var result = _blackJackService.GetPlayerCards();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }
        [TestMethod]
        public void GetDealerCards_ShouldReturnDealerCardsWhenNotEmpty()
        {

            _blackJackService.dealerCards.Add(new Card("A", "Spades", 11));

            var result = _blackJackService.GetDealerCards();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }
        [TestMethod]
        public void GetDealerCards_ShouldCalculateAndReturnDealerCardsWhenEmpty()
        {
            _blackJackService.GetDeck();
            var result = _blackJackService.GetDealerCards();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }
        [TestMethod]
        public void GetState_ShouldReturnInitialState()
        {
            var result = _blackJackService.GetState();

            Assert.IsFalse(result);
        }
        [TestMethod]
        public void SetState_ShouldSetState()
        {
            _blackJackService.SetState(true);

            Assert.IsTrue(_blackJackService.GetState());
        }
        [TestMethod]
        public void GetbetAmount_ShouldReturnInitialBetAmount()
        {
            var result = _blackJackService.GetbetAmount();

            Assert.AreEqual(0, result);
        }
        [TestMethod]
        public void SetbetAmount_ShouldSetBetAmount()
        {
            // Arrange
            double amount = 50;

            // Act
            _blackJackService.SetbetAmount(amount);

            // Assert
            Assert.AreEqual(amount, _blackJackService.GetbetAmount());
        }
        [TestMethod]
        public void ResetDeck_ShouldResetDeckAndProperties()
        {
            // Arrange
            _blackJackService.dealerCards.Add(new Card("A", "Spades", 11));
            _blackJackService.SetbetAmount(50);
            _blackJackService.SetState(true);
            _blackJackService.SetTick(5);
            _blackJackService.SetHiddenCard(new Card("Q", "Hearts", 10));

            // Act
            _blackJackService.ResetDeck();

            // Assert
            Assert.IsFalse(_blackJackService.deck.Any());
            Assert.IsFalse(_blackJackService.playerCards.Any());
            Assert.IsFalse(_blackJackService.dealerCards.Any());
            Assert.AreEqual(0, _blackJackService.GetbetAmount());
            Assert.AreEqual(0, _blackJackService.GetTick());
            Assert.IsFalse(_blackJackService.GetState());
            Assert.IsNotNull(_blackJackService.GetHiddenCard());
            Assert.AreEqual("Hidden", _blackJackService.GetHiddenCard().Rank);
        }
        [TestMethod]
        public void SetTick_ShouldSetTick()
        {
            int tick = 10;

            _blackJackService.SetTick(tick);

            Assert.AreEqual(tick, _blackJackService.GetTick());
        }
        [TestMethod]
        public void SetHiddenCard_ShouldSetHiddenCard()
        {
            Card hiddenCard = new Card("K", "Diamonds", 10);

            _blackJackService.SetHiddenCard(hiddenCard);

            Assert.AreEqual(hiddenCard, _blackJackService.GetHiddenCard());
        }
        [TestMethod]
        public void SetService_ShouldSetServiceProperties()
        {
            BlackJack blackJack = new BlackJack();
            blackJack.deck = new List<Card> { new Card("A", "Hearts", 11) };
            blackJack.playerCards = new List<Card> { new Card("10", "Spades", 10) };
            blackJack.dealerCards = new List<Card> { new Card("K", "Clubs", 10) };
            blackJack.hiddenCard = new Card("Q", "Diamonds", 10);
            blackJack.tick = 5;
            blackJack.betAmount = 50;
            blackJack.gameState = true;

            (_blackJackService as IBlackJackService).SetService(blackJack);

            CollectionAssert.AreEqual(blackJack.deck, _blackJackService.deck);
            CollectionAssert.AreEqual(blackJack.playerCards, _blackJackService.playerCards);
            CollectionAssert.AreEqual(blackJack.dealerCards, _blackJackService.dealerCards);
            Assert.AreEqual(blackJack.hiddenCard, _blackJackService.GetHiddenCard());
            Assert.AreEqual(blackJack.tick, _blackJackService.GetTick());
            Assert.AreEqual(blackJack.betAmount, _blackJackService.GetbetAmount());
            Assert.AreEqual(blackJack.gameState, _blackJackService.GetState());
        }
    }
}
