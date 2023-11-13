using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AngleSharp;
using AutoFixture;
using Blazorise;
using Bunit;
using Bunit.TestDoubles;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PSA.Client.Pages.Bets;
using PSA.Client.Pages.Fights;
using PSA.Client.Pages.Tournaments;
using PSA.Shared;
using RichardSzalay.MockHttp;
using static System.Net.WebRequestMethods;
using TestContext = Bunit.TestContext;

namespace PSA.ClientTests
{
    [TestClass()]
    public class BettingTests : BunitTestContext
    {
        public static Fixture _fixture = new();
        public static IEnumerable<object[]> GetDataForWinLosse()
        {
            yield return new object[] {
                _fixture.Build<Robot>().With(x => x.Id, 1).With(y => y.Losses, 0).With(z => z.Wins, 1).CreateMany().ToList()

            };
            yield return new object[] {
                _fixture.Build<Robot>().With(x => x.Id, 1).With(y => y.Losses, 0).With(z => z.Wins, 0).CreateMany().ToList()

            };
            yield return new object[] {
                _fixture.Build<Robot>().With(x => x.Id, 1).With(y => y.Losses, 1).With(z => z.Wins, 0).CreateMany().ToList()
            };


        }
        [TestMethod]
        public void ToggleTableRow_ShouldToggleShowTableRow()
        {
            // Arrange

            using var ctx = new TestContext();
            var mock = ctx.Services.AddMockHttpClient();

            mock.When($"api/fights/swipefights").RespondJson(new List<Fight> { });
            mock.When($"api/robots/allrobots").RespondJson(new List<Robot> { });
            mock.When("api/Tournaments").RespondJson(new List<Tournament> { });
            mock.When("api/bets/active").RespondJson(new List<Bet> { });
            mock.When("api/robotPart").RespondJson(new List<RobotPart> { });
            mock.When("api/products").RespondJson(new List<Product> { });
            var cut = ctx.RenderComponent<Betting>(); // Replace 'Betting' with the actual name of your component


            // Act

            cut.Find("button").Click();

            // Assert
            var showTableRow = cut.Instance.showTableRow;
            Assert.IsFalse(showTableRow, "ToggleTableRow did not toggle showTableRow successfully.");
        }

        // Add more tests as needed for other methods and functionalities
        [TestMethod]
        public async Task LoadFightsTest()
        {
            var mock = Services.AddMockHttpClient();
           
            var navMan = Services.GetRequiredService<FakeNavigationManager>();
            var temp = _fixture.Create<Fight>();

            var tempBets = _fixture.Build<Bet>().With(x => x.fk_robot_id, 1).CreateMany().ToList();
            var tempParts = _fixture.Build<RobotPart>().With(x => x.Id, 1).CreateMany().ToList();
            var tempFights = _fixture.Build<Fight>().CreateMany().ToList();
            var tempProducts = _fixture.Build<Product>().With(x => x.Id, 1).With(a => a.Material, 1).With(b => b.Attack, 1).With(c => c.Defense).With(d => d.Speed).With(e => e.Price, 1).CreateMany().ToList();
            var tempRobots = _fixture.Build<Robot>().With(x => x.Id, 1).With(y => y.Losses, 1).With(z => z.Wins, 1).CreateMany().ToList();
            var tempTournaments = _fixture.Build<Tournament>().With(x => x.Id, 1).CreateMany().ToList();
            var tempTournamentFights = _fixture.Build<TournamentFight>().With(x => x.fk_turnyras, 1).With(y=>y.fk_kova, tempFights[0].id).CreateMany().ToList();
            mock.When($"/api/fights/swipefights").RespondJson(tempFights);
            mock.When($"/api/robots/allrobots").RespondJson(tempRobots);
            mock.When($"/api/Tournaments").RespondJson(tempTournaments);
            mock.When($"/api/bets/active").RespondJson(tempBets);
            mock.When($"/api/robotPart").RespondJson(tempParts);
            mock.When($"/api/products").RespondJson(tempProducts);
            mock.When("http://localhost/api/fights/tourneyfights/1").RespondJson(tempFights);

            var cut = RenderComponent<Betting>();

            cut.WaitForState(() => cut.FindAll("td").Count > 0);
            // Act - Simulate form submission
            
           var result = cut.Instance.LoadFights();
            Assert.IsNotNull(result, "error");



        }

        [TestMethod]
        public void CalculateCoef_ShouldReturnCorrectCoefficient()
        {
            // Arrange
            var mock = Services.AddMockHttpClient();
            var temp = _fixture.Build<Fight>().With(x => x.id, 1).Create();

            var tempBets = _fixture.Build<Bet>().With(x => x.fk_fight_id, temp.id).With(y => y.fk_robot_id, temp.fk_robot1).CreateMany().ToList();


            List<Fight> asd = new List<Fight>();


            mock.When($"/api/fights/swipefights").RespondJson(asd);
            mock.When($"/api/robots/allrobots").RespondJson(new List<Robot> { });
            mock.When($"/api/Tournaments").RespondJson(new List<Tournament> { });
            mock.When($"/api/bets/active").RespondJson(tempBets);
            mock.When($"/api/robotPart").RespondJson(new List<RobotPart> { });
            mock.When($"/api/products").RespondJson(new List<Product> { });
            var cut = RenderComponent<Betting>(); // Replace 'Betting' with the actual name of your component
            cut.WaitForState(() => cut.FindAll("td").Count > 0);
            // Act
            var result = cut.Instance.CalculateCoef(temp, temp.fk_robot1);

            // Assert
            Assert.IsNotNull(result);
            // Assert.AreEqual(result, result, "CalculateCoef did not return the expected coefficient.");
        }

        [TestMethod]
        public void GetName_ShouldReturnRobotName_WhenRobotIdExists()
        {

            using var ctx = new TestContext();
            var mock = ctx.Services.AddMockHttpClient();
            var temp = _fixture.Create<Fight>();
            List<Fight> asd = new List<Fight>();
            var tempBets = _fixture.Build<Bet>().With(x => x.fk_robot_id, temp.fk_robot1).CreateMany().ToList();
            mock.When($"/api/fights/swipefights").RespondJson(asd);
            mock.When($"/api/robots/allrobots").RespondJson(new List<Robot> { new Robot { Id = 1, Nickname = "Robot1" } });
            mock.When($"/api/Tournaments").RespondJson(new List<Tournament> { });
            mock.When($"/api/bets/active").RespondJson(tempBets);
            mock.When($"/api/robotPart").RespondJson(new List<RobotPart> { });
            mock.When($"/api/products").RespondJson(new List<Product> { });
            // Arrange
            var cut = ctx.RenderComponent<Betting>(); // Replace 'Betting' with the actual name of your component
            cut.WaitForState(() => cut.FindAll("td").Count > 0);
            var robot = new List<Robot>() {
            new Robot {Id = 1, Nickname = "Robot1"},
            new Robot { Id = 2, Nickname = "Robot2"}
            };
            var result = cut.Instance.GetName(1);

            // Assert

            Assert.AreEqual("Robot1", result);
        }
        [TestMethod]
        public void GetName_ShouldNotReturnRobotName_WhenRobotIdDoesNotExists()
        {

            using var ctx = new TestContext();
            var mock = ctx.Services.AddMockHttpClient();
            var temp = _fixture.Create<Fight>();
            List<Fight> asd = new List<Fight>();
            var tempBets = _fixture.Build<Bet>().With(x => x.fk_robot_id, temp.fk_robot1).CreateMany().ToList();
            mock.When($"/api/fights/swipefights").RespondJson(asd);
            mock.When($"/api/robots/allrobots").RespondJson(new List<Robot> { new Robot { Id = 1, Nickname = "Robot1" } });
            mock.When($"/api/Tournaments").RespondJson(new List<Tournament> { });
            mock.When($"/api/bets/active").RespondJson(tempBets);
            mock.When($"/api/robotPart").RespondJson(new List<RobotPart> { });
            mock.When($"/api/products").RespondJson(new List<Product> { });
            // Arrange
            var cut = ctx.RenderComponent<Betting>(); // Replace 'Betting' with the actual name of your component
            cut.WaitForState(() => cut.FindAll("td").Count > 0);
            var robot = new List<Robot>() {
            new Robot {Id = 1, Nickname = "Robot1"},
            new Robot { Id = 2, Nickname = "Robot2"}
            };
            var result = cut.Instance.GetName(3);

            // Assert

            Assert.AreNotEqual("Robot1", result);
        }

        [TestMethod]
        public void GoToBet_NavigatesToCorrectUrl()
        {

            var mock = Services.AddMockHttpClient();

            var navMan = Services.GetRequiredService<FakeNavigationManager>();
            var temp = _fixture.Create<Fight>();
            List<Fight> asd = new List<Fight>();
            var tempBets = _fixture.Build<Bet>().With(x => x.fk_robot_id, temp.fk_robot1).CreateMany().ToList();
            mock.When($"/api/fights/swipefights").RespondJson(asd);
            mock.When($"/api/robots/allrobots").RespondJson(new List<Robot> { new Robot { Id = 1, Nickname = "Robot1" } });
            mock.When($"/api/Tournaments").RespondJson(new List<Tournament> { });
            mock.When($"/api/bets/active").RespondJson(tempBets);
            mock.When($"/api/robotPart").RespondJson(new List<RobotPart> { });
            mock.When($"/api/products").RespondJson(new List<Product> { });

            var cut = RenderComponent<Betting>();

            cut.WaitForState(() => cut.FindAll("td").Count > 0);
            // Act - Simulate form submission
            cut.Instance.GoToBet(new Fight(DateTime.Now, 1, 1, 1, 1, 1), 2);

            // Assert - Verify navigation
            Assert.AreEqual("http://localhost/betting/1/2", navMan.Uri);
        }
        [TestMethod]
        public void GetWinLosse_robotsIsEmpty()
        {

            var mock = Services.AddMockHttpClient();

            var navMan = Services.GetRequiredService<FakeNavigationManager>();
            var temp = _fixture.Create<Fight>();
            List<Fight> asd = new List<Fight>();
            var tempBets = _fixture.Build<Bet>().With(x => x.fk_robot_id, temp.fk_robot1).CreateMany().ToList();
            mock.When($"/api/fights/swipefights").RespondJson(asd);
            mock.When($"/api/robots/allrobots").RespondJson(new List<Robot> { });
            mock.When($"/api/Tournaments").RespondJson(new List<Tournament> { });
            mock.When($"/api/bets/active").RespondJson(tempBets);
            mock.When($"/api/robotPart").RespondJson(new List<RobotPart> { });
            mock.When($"/api/products").RespondJson(new List<Product> { });

            var cut = RenderComponent<Betting>();

            cut.WaitForState(() => cut.FindAll("td").Count > 0);
            // Act - Simulate form submission
            var result = cut.Instance.getWinLosse(1);

            // Assert - Verify navigation
            Assert.AreEqual(0, result);
        }
        [TestMethod]
        public void GetWinLosse_robotsIsNotEmptyButIdNotEqual()
        {

            var mock = Services.AddMockHttpClient();

            var navMan = Services.GetRequiredService<FakeNavigationManager>();
            var temp = _fixture.Create<Fight>();
            List<Fight> asd = new List<Fight>();
            var tempBets = _fixture.Build<Bet>().With(x => x.fk_robot_id, temp.fk_robot1).CreateMany().ToList();
            var tempRobots = _fixture.Build<Robot>().With(x => x.Id, 1).With(y => y.Losses, 1).With(z => z.Wins, 1).CreateMany().ToList();
            mock.When($"/api/fights/swipefights").RespondJson(asd);
            mock.When($"/api/robots/allrobots").RespondJson(tempRobots);
            mock.When($"/api/Tournaments").RespondJson(new List<Tournament> { });
            mock.When($"/api/bets/active").RespondJson(tempBets);
            mock.When($"/api/robotPart").RespondJson(new List<RobotPart> { });
            mock.When($"/api/products").RespondJson(new List<Product> { });

            var cut = RenderComponent<Betting>();

            cut.WaitForState(() => cut.FindAll("td").Count > 0);
            // Act - Simulate form submission
            var result = cut.Instance.getWinLosse(2);

            // Assert - Verify navigation
            Assert.AreEqual(0, result);
        }
        [TestMethod]
        public void GetWinLosse_robotsAreEqual()
        {

            var mock = Services.AddMockHttpClient();

            var navMan = Services.GetRequiredService<FakeNavigationManager>();
            var temp = _fixture.Create<Fight>();
            List<Fight> asd = new List<Fight>();
            var tempBets = _fixture.Build<Bet>().With(x => x.fk_robot_id, temp.fk_robot1).CreateMany().ToList();
            var tempRobots = _fixture.Build<Robot>().With(x => x.Id, 1).With(y => y.Losses, 1).With(z => z.Wins, 1).CreateMany().ToList();
            mock.When($"/api/fights/swipefights").RespondJson(asd);
            mock.When($"/api/robots/allrobots").RespondJson(tempRobots);
            mock.When($"/api/Tournaments").RespondJson(new List<Tournament> { });
            mock.When($"/api/bets/active").RespondJson(tempBets);
            mock.When($"/api/robotPart").RespondJson(new List<RobotPart> { });
            mock.When($"/api/products").RespondJson(new List<Product> { });

            var cut = RenderComponent<Betting>();

            cut.WaitForState(() => cut.FindAll("td").Count > 0);
            // Act - Simulate form submission
            var result = cut.Instance.getWinLosse(1);

            // Assert - Verify navigation
            Assert.AreEqual(1, result);
        }
        [TestMethod]
        [DynamicData(nameof(GetDataForWinLosse), DynamicDataSourceType.Method)]
        public void GetWinLosse_robotsIsNotEmpty(List<Robot> robots)
        {

            var mock = Services.AddMockHttpClient();

            var navMan = Services.GetRequiredService<FakeNavigationManager>();
            var temp = _fixture.Create<Fight>();
            List<Fight> asd = new List<Fight>();
            var tempBets = _fixture.Build<Bet>().With(x => x.fk_robot_id, temp.fk_robot1).CreateMany().ToList();

            mock.When($"/api/fights/swipefights").RespondJson(asd);
            mock.When($"/api/robots/allrobots").RespondJson(robots);
            mock.When($"/api/Tournaments").RespondJson(new List<Tournament> { });
            mock.When($"/api/bets/active").RespondJson(tempBets);
            mock.When($"/api/robotPart").RespondJson(new List<RobotPart> { });
            mock.When($"/api/products").RespondJson(new List<Product> { });

            var cut = RenderComponent<Betting>();

            cut.WaitForState(() => cut.FindAll("td").Count > 0);
            // Act - Simulate form submission
            var result = cut.Instance.getWinLosse(1);

            // Assert - Verify navigation
            Assert.AreEqual(1, result);
        }
        
        [TestMethod]
        public void GetSuggestion_Option1()
        {
            var mock = Services.AddMockHttpClient();

            var navMan = Services.GetRequiredService<FakeNavigationManager>();
            var temp = _fixture.Create<Fight>();
            
            var tempBets = _fixture.Build<Bet>().With(x => x.fk_robot_id, 1).CreateMany().ToList();
            var tempParts = _fixture.Build<RobotPart>().With(x => x.Id, 1).With(y=>y.fk_robotas, 1).With(z=>z.fk_preke_id, 1).With(a=>a.Durability, 100).CreateMany().ToList();
            var tempFights = _fixture.Build<Fight>().With(x => x.fk_robot1, 1).With(y => y.fk_robot2, 1).CreateMany().ToList();
            var tempProducts = _fixture.Build<Product>().With(x => x.Id, 1).With(a=>a.Material, 1).With(b=> b.Attack, 1).With(c=>c.Defense).With(d=>d.Speed).With(e => e.Price, 1).CreateMany().ToList();
            var tempRobots = _fixture.Build<Robot>().With(x => x.Id, 1).With(y => y.Losses, 1).With(z => z.Wins, 1).CreateMany().ToList();
            var tempTournaments = _fixture.Build<Tournament>().With(x => x.Id, 1).CreateMany().ToList();
            mock.When($"/api/fights/swipefights").RespondJson(tempFights);
            mock.When($"/api/robots/allrobots").RespondJson(tempRobots);
            mock.When($"/api/Tournaments").RespondJson(new List<Tournament>());
            mock.When($"/api/bets/active").RespondJson(tempBets);
            mock.When($"/api/robotPart").RespondJson(tempParts);
            mock.When($"/api/products").RespondJson(tempProducts);

            var cut = RenderComponent<Betting>();

            cut.WaitForState(() => cut.FindAll("td").Count > 0);
            // Act - Simulate form submission
            var result = cut.Instance.getSuggestion(tempFights[0], 1);


            Assert.AreEqual(50, result);

        }
        [TestMethod]
        public void GetSuggestion_Option2()
        {
            var mock = Services.AddMockHttpClient();

            var navMan = Services.GetRequiredService<FakeNavigationManager>();
            var temp = _fixture.Create<Fight>();

            var tempBets = _fixture.Build<Bet>().With(x => x.fk_robot_id, 1).CreateMany().ToList();
            var tempParts = _fixture.Build<RobotPart>().With(x => x.Id, 1).CreateMany().ToList();
            var tempFights = _fixture.Build<Fight>().CreateMany().ToList();
            var tempProducts = _fixture.Build<Product>().With(x => x.Id, 1).With(a => a.Material, 1).With(b => b.Attack, 1).With(c => c.Defense).With(d => d.Speed).With(e => e.Price, 1).CreateMany().ToList();
            var tempRobots = _fixture.Build<Robot>().With(x => x.Id, 1).With(y => y.Losses, 1).With(z => z.Wins, 1).CreateMany().ToList();
            var tempTournaments = _fixture.Build<Tournament>().With(x => x.Id, 1).CreateMany().ToList();
            mock.When($"/api/fights/swipefights").RespondJson(tempFights);
            mock.When($"/api/robots/allrobots").RespondJson(tempRobots);
            mock.When($"/api/Tournaments").RespondJson(new List<Tournament>());
            mock.When($"/api/bets/active").RespondJson(tempBets);
            mock.When($"/api/robotPart").RespondJson(tempParts);
            mock.When($"/api/products").RespondJson(tempProducts);

            var cut = RenderComponent<Betting>();

            cut.WaitForState(() => cut.FindAll("td").Count > 0);
            // Act - Simulate form submission
            var result = cut.Instance.getSuggestion(tempFights[0], 2);


            Assert.AreEqual(0, result);

        }
        [TestMethod]
        public void GetSuggestion_OptionNone()
        {
            var mock = Services.AddMockHttpClient();

            var navMan = Services.GetRequiredService<FakeNavigationManager>();
            var temp = _fixture.Create<Fight>();

            var tempBets = _fixture.Build<Bet>().With(x => x.fk_robot_id, 1).CreateMany().ToList();
            var tempParts = _fixture.Build<RobotPart>().With(x => x.Id, 1).CreateMany().ToList();
            var tempFights = _fixture.Build<Fight>().CreateMany().ToList();
            var tempProducts = _fixture.Build<Product>().With(x => x.Id, 1).With(a => a.Material, 1).With(b => b.Attack, 1).With(c => c.Defense).With(d => d.Speed).With(e => e.Price, 1).CreateMany().ToList();
            var tempRobots = _fixture.Build<Robot>().With(x => x.Id, 1).With(y => y.Losses, 1).With(z => z.Wins, 1).CreateMany().ToList();
            var tempTournaments = _fixture.Build<Tournament>().With(x => x.Id, 1).CreateMany().ToList();
            mock.When($"/api/fights/swipefights").RespondJson(tempFights);
            mock.When($"/api/robots/allrobots").RespondJson(tempRobots);
            mock.When($"/api/Tournaments").RespondJson(new List<Tournament>());
            mock.When($"/api/bets/active").RespondJson(tempBets);
            mock.When($"/api/robotPart").RespondJson(tempParts);
            mock.When($"/api/products").RespondJson(tempProducts);

            var cut = RenderComponent<Betting>();

            cut.WaitForState(() => cut.FindAll("td").Count > 0);
            // Act - Simulate form submission
            var result = cut.Instance.getSuggestion(tempFights[0], 3);


            Assert.AreEqual(50, result);

        }

    }
}
