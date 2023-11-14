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
using PSA.Client.Pages.Tournaments;
using PSA.Client.Pages.Bets;
using System.ComponentModel;
using Microsoft.JSInterop;
namespace PSA.ClientTests
{
    [TestClass]
    public class ViewTournamentsTests : BunitTestContext
    {
        public static Fixture _fixture = new();

        [TestMethod]
        public void CheckDatestest()
        {
            var tempTournament = _fixture.Build<Tournament>().With(x => x.Id, 1).With(x => x.Start_date, DateTime.Today.ToString()).With(x => x.End_date, DateTime.Today.ToString()).Create();
            var tempCurrent = _fixture.Build<CurrentUser?>().With(x => x.Id, 1).Create();
            var temptourFights = _fixture.Build<TournamentFight?>().With(x => x.id, 1).With(x=>x.fk_kova,1).CreateMany().ToList();
            var tempTourRobots = _fixture.Build<TournamentRobot>().With(x => x.id, 1).With(x => x.turi_kova, 0).With(x => x.fk_robotas, 1).CreateMany(10).ToList();
            var tempTourRobot = _fixture.Build<TournamentRobot>().With(x => x.id, 1).With(x => x.turi_kova, 0).With(x => x.fk_robotas, 1).Create();
            var tempFight = _fixture.Build<Fight>().With(x => x.id, 1).With(x => x.fk_robot1, 1).With(x => x.fk_robot2, 1).With(x => x.date, DateTime.Today.AddDays(1)).With(x => x.state, 2).Create();
            var tempTournamentFight = _fixture.Build<TournamentFight>().With(x => x.id, 1).With(x => x.fk_turnyras, 1).With(x => x.fk_kova, 1).Create();
            var tempRobot = _fixture.Build<Robot>().With(x => x.Id, 1).Create();

            using var ctx = new TestContext();
            var mock = Services.AddMockHttpClient();
            var navMan = Services.GetRequiredService<FakeNavigationManager>();


            mock.When("http://localhost/api/Tournaments/0").RespondJson(tempTournament);
            mock.When($"/api/currentuser").RespondJson(tempCurrent);
            mock.When("http://localhost/api/TournamentFights/0").RespondJson(temptourFights);
            mock.When("http://localhost/api/tournamentrobots/0").RespondJson(tempTourRobots);
            mock.When(HttpMethod.Put,$"/api/TournamentRobots/").RespondJson(tempTourRobot);
            mock.When($"/api/Fights/").RespondJson(tempFight);
            mock.When($"/api/Fights/maxid").RespondJson(1);
            mock.When($"/api/Tournamentfights").RespondJson(tempTournamentFight);
            mock.When($"/api/tournamentrobots").RespondJson(tempTourRobots);
            mock.When("http://localhost/api/Fights/1").RespondJson(tempFight);
            mock.When("http://localhost/api/Robots/1").RespondJson(tempRobot);
            mock.When("http://localhost/api/tournamentrobots/getcount/0").RespondJson(10);
            mock.When("http://localhost/api/tournamentrobots/getcountnofights/0").RespondJson(1);
            mock.When("http://localhost/api/tournamentrobots/existsnofight/0").RespondJson(tempRobot);

            var cut = RenderComponent<ViewTournament>();
            cut.WaitForState(() => cut.FindAll("td").Count > 0, timeout: TimeSpan.FromSeconds(1));

            cut.Instance.checkDates();

            Assert.AreEqual(cut.Instance.busena, "Finished");
        }
        [TestMethod]
        public void CheckDatestest_NotStarted()
        {
            var tempTournament = _fixture.Build<Tournament>().With(x => x.Id, 1).With(x => x.Start_date, DateTime.Today.AddDays(1).ToString()).With(x => x.End_date, DateTime.Today.AddDays(2).ToString()).Create();
            var tempCurrent = _fixture.Build<CurrentUser?>().With(x => x.Id, 1).Create();
            var temptourFights = _fixture.Build<TournamentFight?>().With(x => x.id, 1).With(x => x.fk_kova, 1).CreateMany().ToList();
            var tempTourRobots = _fixture.Build<TournamentRobot>().With(x => x.id, 1).With(x => x.turi_kova, 0).With(x => x.fk_robotas, 1).CreateMany(10).ToList();
            var tempTourRobot = _fixture.Build<TournamentRobot>().With(x => x.id, 1).With(x => x.turi_kova, 0).With(x => x.fk_robotas, 1).Create();
            var tempFight = _fixture.Build<Fight>().With(x => x.id, 1).With(x => x.fk_robot1, 1).With(x => x.fk_robot2, 1).With(x => x.date, DateTime.Today.AddDays(1)).With(x => x.state, 2).Create();
            var tempTournamentFight = _fixture.Build<TournamentFight>().With(x => x.id, 1).With(x => x.fk_turnyras, 1).With(x => x.fk_kova, 1).Create();
            var tempRobot = _fixture.Build<Robot>().With(x => x.Id, 1).Create();

            using var ctx = new TestContext();
            var mock = Services.AddMockHttpClient();
            var navMan = Services.GetRequiredService<FakeNavigationManager>();


            mock.When("http://localhost/api/Tournaments/0").RespondJson(tempTournament);
            mock.When($"/api/currentuser").RespondJson(tempCurrent);
            mock.When("http://localhost/api/TournamentFights/0").RespondJson(temptourFights);
            mock.When("http://localhost/api/tournamentrobots/0").RespondJson(tempTourRobots);
            mock.When(HttpMethod.Put, $"/api/TournamentRobots/").RespondJson(tempTourRobot);
            mock.When($"/api/Fights/").RespondJson(tempFight);
            mock.When($"/api/Fights/maxid").RespondJson(1);
            mock.When($"/api/Tournamentfights").RespondJson(tempTournamentFight);
            mock.When($"/api/tournamentrobots").RespondJson(tempTourRobots);
            mock.When("http://localhost/api/Fights/1").RespondJson(tempFight);
            mock.When("http://localhost/api/Robots/1").RespondJson(tempRobot);
            mock.When("http://localhost/api/tournamentrobots/getcount/0").RespondJson(10);
            mock.When("http://localhost/api/tournamentrobots/getcountnofights/0").RespondJson(1);
            mock.When("http://localhost/api/tournamentrobots/existsnofight/0").RespondJson(tempRobot);

            var cut = RenderComponent<ViewTournament>();
            cut.WaitForState(() => cut.FindAll("td").Count > 0, timeout: TimeSpan.FromSeconds(1));

            cut.Instance.checkDates();

            Assert.AreEqual(cut.Instance.busena, "Not started");
        }
    }
}
