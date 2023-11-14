using AutoFixture;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using PSA.Client.Pages.Fights;
using PSA.Client.Pages.Robot;
using PSA.Client.Pages.Tournaments;
using PSA.Client.Pages.Users;
using PSA.Shared;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace PSA.ClientTests
{
    [TestClass()]
    public class TournamentTests : BunitTestContext
    {
        public static Fixture _fixture = new();

        [TestMethod]
        public async Task TodaysFights()
        {
            //Arrange
            var mock = Services.AddMockHttpClient();
            var navMan = Services.GetRequiredService<FakeNavigationManager>();
            var testUser = _fixture.Create<CurrentUser>();
            var testTournaments = _fixture.Build<Tournament>().With(x=> x.Start_date, DateTime.Today.ToString()).CreateMany(1).ToList();        
            var testProducts = _fixture.Build<Product>().With(z => z.Price, 15).With(c => c.Quantity, 2).CreateMany<Product>().ToList();
            var testFights =_fixture.CreateMany<Fight>().ToList();
            var testFights2 = _fixture.Build<Fight>().With(x=>x.winner,0)
                .With(x => x.state, 1)
                .With(x => x.date, DateTime.Now)
                .CreateMany<Fight>(3)
                .ToList();
            var testRobotPart = _fixture.Build<RobotPart>().With(x => x.fk_robotas, testFights2[0].fk_robot1)
                .With(x => x.fk_preke_id, testProducts[0].Id)
                .CreateMany<RobotPart>().ToList();
            testRobotPart[1].fk_robotas = testFights2[0].fk_robot2;
            var tempRobot = _fixture.Create<Robot>();
            mock.When("/api/Tournaments").RespondJson(testTournaments);
            mock.When("/api/robotPart").RespondJson(testRobotPart);
            mock.When("/api/products").RespondJson(testProducts);
            mock.When($"/api/tournamentrobots/getcount/{testTournaments[0].Id}").RespondJson(testFights.Count);
            mock.When($"/api/fights/todaytournamentfights/{testTournaments[0].Id}").RespondJson(testFights2);
            mock.When("/api/currentuser").RespondJson(testUser);
            mock.When(HttpMethod.Put, $"/api/robots/win/{testFights2[0].fk_robot1}/{testFights2[0].fk_robot2}").RespondJson(testUser);
            mock.When(HttpMethod.Put, $"/api/robots/win/{testFights2[0].fk_robot2}/{testFights2[0].fk_robot1}").RespondJson(testUser);
            mock.When(HttpMethod.Put, $"/api/robots/win/{testFights2[1].fk_robot1}/{testFights2[0].fk_robot2}").RespondJson(testUser);
            mock.When(HttpMethod.Put, $"/api/robots/win/{testFights2[1].fk_robot2}/{testFights2[0].fk_robot1}").RespondJson(testUser);
            mock.When(HttpMethod.Put, $"/api/robots/win/{testFights2[2].fk_robot1}/{testFights2[0].fk_robot2}").RespondJson(testUser);
            mock.When(HttpMethod.Put, $"/api/robots/win/{testFights2[2].fk_robot2}/{testFights2[0].fk_robot1}").RespondJson(testUser);
            mock.When(HttpMethod.Put, $"/api/robots/win/{testFights2[0].fk_robot1}/{testFights2[0].fk_robot1}").RespondJson(testUser);
            mock.When(HttpMethod.Put, $"/api/robots/win/{testFights2[0].fk_robot2}/{testFights2[0].fk_robot2}").RespondJson(testUser);
            mock.When(HttpMethod.Put, $"/api/robots/win/{testFights2[1].fk_robot1}/{testFights2[0].fk_robot1}").RespondJson(testUser);
            mock.When(HttpMethod.Put, $"/api/robots/win/{testFights2[1].fk_robot2}/{testFights2[0].fk_robot2}").RespondJson(testUser);
            mock.When(HttpMethod.Put, $"/api/robots/win/{testFights2[2].fk_robot1}/{testFights2[0].fk_robot1}").RespondJson(testUser);
            mock.When(HttpMethod.Put, $"/api/robots/win/{testFights2[2].fk_robot2}/{testFights2[0].fk_robot2}").RespondJson(testUser);
            mock.When($"api/robots/{testFights2[0].fk_robot1}").RespondJson(tempRobot);
            mock.When($"api/robots/{testFights2[0].fk_robot2}").RespondJson(tempRobot);
            mock.When($"api/robots/{testFights2[1].fk_robot1}").RespondJson(tempRobot);
            mock.When($"api/robots/{testFights2[1].fk_robot2}").RespondJson(tempRobot);
            mock.When($"api/robots/{testFights2[2].fk_robot1}").RespondJson(tempRobot);
            mock.When($"api/robots/{testFights2[2].fk_robot2}").RespondJson(tempRobot);
            mock.When(HttpMethod.Put, $"api/robotPart/5").RespondJson(tempRobot);
            mock.When(HttpMethod.Put, $"api/robotPart/10").RespondJson(tempRobot);
            mock.When(HttpMethod.Put, $"api/robotPart/20").RespondJson(tempRobot);
            var cut = RenderComponent<Tournaments>();

            //Act
            cut.WaitForState(() => cut.FindAll("tr").Count > 0);

            //Assert

        }
        [TestMethod]
        public async Task HandleDamage_SendsCorrectRequests()
        {
            // Arrange
            var mock = Services.AddMockHttpClient();
            var cut = RenderComponent<Tournaments>();

            var robotId = 1;
            var damage = 5;

            // Set up expectations
            mock.When(HttpMethod.Get, $"http://localhost/api/robots/{robotId}").Respond("application/json", "{}");
            mock.When(HttpMethod.Put, $"http://localhost/api/robotPart/{damage}").Respond("application/json", "{}");

            // Act
            await cut.InvokeAsync(() => cut.Instance.HandleDamage(robotId, damage));

            // Assert
            mock.VerifyNoOutstandingExpectation();
        }

        [TestMethod]
        public async Task WinLoseTieCondition_SendsCorrectRequest()
        {
            // Arrange
            var mock = Services.AddMockHttpClient();
            var cut = RenderComponent<Tournaments>();

            var id = 1;
            var id2 = 2;

            // Set up expectations
            mock.When(HttpMethod.Put, $"/api/robots/win/{id}/{id2}")
                .Respond("application/json", "{}");

            // Act
            await cut.InvokeAsync(() => cut.Instance.WinLoseTieCondition(id, id2));

            // Assert
            mock.VerifyNoOutstandingExpectation();
        }

        [TestMethod]
        public async Task TieCondition_SendsCorrectRequest()
        {
            // Arrange
            var mock = Services.AddMockHttpClient();
            var cut = RenderComponent<Tournaments>();

            var id = 1;
            var id2 = 2;

            // Set up expectations
            mock.When(HttpMethod.Put, $"http://localhost/api/robots/tie/{id}/{id2}")
                .Respond("application/json", "{}");

            // Act
            await cut.InvokeAsync(() => cut.Instance.TieCondition(id, id2));

            // Assert
            mock.VerifyNoOutstandingExpectation();
        }

        [TestMethod]
        public async Task HandleFinished_SendsCorrectRequest()
        {
            // Arrange
            var mock = Services.AddMockHttpClient();
            var cut = RenderComponent<Tournaments>();

            var fight = new Fight { fk_robot1 = 1, fk_robot2 = 2 };

            // Set up expectations
            mock.When(HttpMethod.Put, "http://localhost/api/fights/win/mhm")
                .With(request => request.Content.ReadAsStringAsync().Result.Contains("fk_robot1") &&
                                 request.Content.ReadAsStringAsync().Result.Contains("fk_robot2"))
                .Respond("application/json", "{}");

            // Act
            await cut.InvokeAsync(() => cut.Instance.HandleFinished(fight));

            // Assert
            mock.VerifyNoOutstandingExpectation();
        }
        [TestMethod]
        public async Task RemoveTournament_ShouldCallHttpDeleteAsyncAndRemoveFromList()
        {
            // Arrange
            var mock = Services.AddMockHttpClient();
            var testTournaments = _fixture.CreateMany<Tournaments>().ToList();
            mock.When(HttpMethod.Delete, $"/api/Tournaments/{1}").RespondJson(testTournaments);
            var cut = RenderComponent<Tournaments>();

           

            // Act
            await cut.InvokeAsync(() => cut.Instance.RemoveTournament(1));
            // Ensure other tournaments are still present in the list
        }
        [TestMethod]
        public async Task HandleState_ShouldCallHttpPutAsJsonAsync_UpdateStateAndNavigate()
        {
            // Arrange
            var mock = Services.AddMockHttpClient();
            var navMan = Services.GetRequiredService<FakeNavigationManager>();
            var testProfile = _fixture.Create<Profile>();
            mock.When(HttpMethod.Put, $"/api/fights/{1}").RespondJson(testProfile);
            var cut = RenderComponent<Tournaments>();

            // Act
            await cut.InvokeAsync(() => cut.Instance.HandleState(1));

            // Assert
            //cut.WaitForAssertion(() => Assert.AreEqual("http://localhost/fights/", navMan.Uri));

            // Add more assertions as needed
        }
    }
}
