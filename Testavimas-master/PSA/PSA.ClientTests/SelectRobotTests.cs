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
using PSA.Client.Pages.Gambling;
using PSA.Client.Pages.Fights;
using PSA.Client.Pages.Tournaments;
using PSA.Shared;
using RichardSzalay.MockHttp;
using static System.Net.WebRequestMethods;
using TestContext = Bunit.TestContext;
using PSA.Client.Shared;
using PSA.Client.Pages.Robots;
using PSA.Client.Pages.Bets;
using System.ComponentModel;

namespace PSA.ClientTests
{
    [TestClass]
    public class SelectRobotTests : BunitTestContext
    {
        public static Fixture _fixture = new();
        [TestMethod]
        public void EditCardTest()
        {
            List<RobotDto> robotDtos = new List<RobotDto>();
            var tempCards = _fixture.Build<SwipeCard>().Create();
            using var ctx = new TestContext();
            var mock = Services.AddMockHttpClient();
            var navMan = Services.GetRequiredService<FakeNavigationManager>();
            mock.When($"/api/robots").RespondJson(new List<Robot> { new Robot { Id = 1, Nickname = "Robot1" } });
            mock.When("http://localhost/card/element/1").RespondJson(tempCards);

            var cut = RenderComponent<SelectRobot>();
            cut.WaitForState(() => cut.FindAll("button").Count > 0,timeout: TimeSpan.FromSeconds(1));


            cut.Instance.EditCard(tempCards);

            Assert.AreEqual($"http://localhost/card/edit/{tempCards.fk_robot}", navMan.Uri);
        }
        [TestMethod]
        public void EditCardTestWithDescription()
        {
            List<RobotDto> robotDtos = new List<RobotDto>();
            var tempDto = _fixture.Build<RobotDto>().Create();
            var tempCards = _fixture.Build<SwipeCard>().Create();
            using var ctx = new TestContext();
            var mock = Services.AddMockHttpClient();
            var navMan = Services.GetRequiredService<FakeNavigationManager>();
            mock.When($"/api/robots").RespondJson(new List<Robot> { new Robot { Id = 1, Nickname = "Robot1" } });
            //mock.When("http://localhost/card/element/1").RespondJson(tempCards);
            mock.When("http://localhost/card/element/1").RespondJson(new SwipeCard{  Id = 1, fk_robot = 1, ImageUrl = "test", Description="Empty" } );
            mock.When($"/card").RespondJson(tempDto);
            var cut = RenderComponent<SelectRobot>();
            cut.WaitForState(() => cut.FindAll("button").Count > 0, timeout: TimeSpan.FromSeconds(1));


            cut.Instance.EditCard(tempCards);

            Assert.AreEqual($"http://localhost/card/edit/{tempCards.fk_robot}", navMan.Uri);
        }
        [TestMethod]
        public void SwipeTest()
        {
            int robotId = 1;
            List<RobotDto> robotDtos = new List<RobotDto>();
            var tempCards = _fixture.Build<SwipeCard>().Create();
            using var ctx = new TestContext();
            var mock = Services.AddMockHttpClient();
            var navMan = Services.GetRequiredService<FakeNavigationManager>();
            mock.When($"/api/robots").RespondJson(new List<Robot> { new Robot { Id = 1, Nickname = "Robot1" } });
            mock.When("http://localhost/card/element/1").RespondJson(tempCards);

            var cut = RenderComponent<SelectRobot>();
            cut.WaitForState(() => cut.FindAll("button").Count > 0, timeout: TimeSpan.FromSeconds(1));


            cut.Instance.Swipe(robotId);

            Assert.AreEqual("http://localhost/robots/swipe/1", navMan.Uri);
        }
        [TestMethod]
        public void SelectTest()
        {
            int robotId = 1;
            List<RobotDto> robotDtos = new List<RobotDto>();
            var tempCards = _fixture.Build<SwipeCard>().Create();
            using var ctx = new TestContext();
            var mock = Services.AddMockHttpClient();
            var navMan = Services.GetRequiredService<FakeNavigationManager>();
            mock.When($"/api/robots").RespondJson(new List<Robot> { new Robot { Id = 1, Nickname = "Robot1" } });
            mock.When("http://localhost/card/element/1").RespondJson(tempCards);

            var cut = RenderComponent<SelectRobot>();
            cut.WaitForState(() => cut.FindAll("button").Count > 0, timeout: TimeSpan.FromSeconds(1));


            cut.Instance.Select(robotId);

            Assert.AreEqual("http://localhost/fights/1", navMan.Uri);
        }
        [TestMethod]
        public async Task EditPartsTest()
        {
            int robotId = 1;
            List<RobotDto> robotDtos = new List<RobotDto>();
            List<RobotPart> robotParts = new List<RobotPart>();
            var tempCards = _fixture.Build<SwipeCard>().Create();
            using var ctx = new TestContext();
            var mock = Services.AddMockHttpClient();
            var navMan = Services.GetRequiredService<FakeNavigationManager>();
            mock.When($"/api/robots").RespondJson(new List<Robot> { new Robot { Id = 1, Nickname = "Robot1" } });
            mock.When("http://localhost/card/element/1").RespondJson(tempCards);
            //mock.When("/api/robotDto/1").RespondJson(robotParts);
            mock.When("/api/robotDto/1").RespondJson(new List<RobotPart> { new RobotPart { Id = 1, Durability = 10, fk_preke_id = 1, fk_robotas = 1 } });
            var cut = RenderComponent<SelectRobot>();
            cut.WaitForState(() => cut.FindAll("button").Count > 0, timeout: TimeSpan.FromSeconds(1));

            await cut.Instance.EditParts(robotId);

            Assert.AreEqual($"http://localhost/robot/Edit/{robotId}", navMan.Uri);
        }


        [TestMethod]
        public void ShowStatisticsTest_ShouldReturnTrue()
        {
            int robotId = 1;
            List<RobotDto> robotDtos = new List<RobotDto>();
            var tempCards = _fixture.Build<SwipeCard>().Create();
            using var ctx = new TestContext();
            var mock = Services.AddMockHttpClient();
            var navMan = Services.GetRequiredService<FakeNavigationManager>();
            mock.When($"/api/robots").RespondJson(new List<Robot> { new Robot { Id = 1, Nickname = "Robot1" } });
            mock.When("http://localhost/card/element/1").RespondJson(tempCards);

            var cut = RenderComponent<SelectRobot>();
            cut.WaitForState(() => cut.FindAll("button").Count > 0);
            cut.WaitForState(() => cut.FindAll("td").Count > 0);
            cut.Instance.ShowStatistics();

            Assert.IsTrue(cut.Instance.statistics);
        }
        [TestMethod]
        public void HideStatisticsTest_ShouldReturnFalse()
        {
            int robotId = 1;
            List<RobotDto> robotDtos = new List<RobotDto>();
            var tempCards = _fixture.Build<SwipeCard>().Create();
            using var ctx = new TestContext();
            var mock = Services.AddMockHttpClient();
            var navMan = Services.GetRequiredService<FakeNavigationManager>();
            mock.When($"/api/robots").RespondJson(new List<Robot> { new Robot { Id = 1, Nickname = "Robot1" } });
            mock.When("http://localhost/card/element/1").RespondJson(tempCards);

            var cut = RenderComponent<SelectRobot>();
            cut.WaitForState(() => cut.FindAll("button").Count > 0, timeout: TimeSpan.FromSeconds(1));


            cut.Instance.HideStatistics();

            Assert.IsFalse(cut.Instance.statistics);
        }
    }
}
