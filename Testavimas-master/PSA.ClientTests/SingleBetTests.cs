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
using Microsoft.JSInterop;

namespace PSA.ClientTests
{
    [TestClass]
    public class SingleBetTests : BunitTestContext
    {
        public static Fixture _fixture = new();
        [TestMethod]
        public void CalculateCoefTest()
        {
            List<Bet> bets = new List<Bet>();
            using var ctx = new TestContext();
            var mock = Services.AddMockHttpClient();
            var navMan = Services.GetRequiredService<FakeNavigationManager>();

            var tempCurrent = _fixture.Build<CurrentUser?>().With(x => x.Id, 1).Create();
            mock.When($"/api/currentuser").RespondJson(tempCurrent);
            mock.When($"/api/bets/active").RespondJson(new List<Bet> { new Bet { Id = 1, Amount = 10, Coefficient=1.2, fk_robot_id = 1, fk_fight_id = 1, fk_user_id = 1 } });

            var cut = RenderComponent<SingleBet>();
            cut.WaitForState(() => cut.FindAll("button").Count > 0, timeout: TimeSpan.FromSeconds(1));

            var coef = cut.Instance.CalculateCoef(1,1);

            Assert.IsNotNull(coef);
        }
        [TestMethod]
        public async Task HandleBetTest_InvokeError()
        {

            List<Bet> bets = new List<Bet>();
            using var ctx = new TestContext();
            var mock = Services.AddMockHttpClient();
            var navMan = Services.GetRequiredService<FakeNavigationManager>();

            var jsMock = new Mock<IJSRuntime>();
            JSInterop.Mode = JSRuntimeMode.Loose;
            var plannedInvocation = JSInterop.SetupVoid("alert");

            var tempCurrent = _fixture.Build<CurrentUser?>().With(x => x.Id, 1).Create();
            mock.When($"/api/currentuser").RespondJson(tempCurrent);
            mock.When($"/api/bets/active").RespondJson(new List<Bet> { new Bet { Id = 1, Amount = 10, Coefficient = 1.2, fk_robot_id = 1, fk_fight_id = 1, fk_user_id = 1 } });

            var cut = RenderComponent<SingleBet>();
            cut.WaitForState(() => cut.FindAll("p").Count > 0, timeout: TimeSpan.FromSeconds(1));

            await cut.InvokeAsync(() => cut.Instance.HandleBet());

            JSInterop.VerifyInvoke("alert", "test");
        }
        [TestMethod]
        public async Task HandleBetTest_InsertBet()
        {

            List<Bet> bets = new List<Bet>();
            Bet bet = new Bet();
            using var ctx = new TestContext();
            var mock = Services.AddMockHttpClient();
            var navMan = Services.GetRequiredService<FakeNavigationManager>();

            var jsMock = new Mock<IJSRuntime>();
            JSInterop.Mode = JSRuntimeMode.Loose;
            var plannedInvocation = JSInterop.SetupVoid("alert");

            var tempCurrent = _fixture.Build<CurrentUser?>().With(x => x.Id, 1).Create();
            mock.When($"/api/currentuser").RespondJson(tempCurrent);
            mock.When($"/api/bets/active").RespondJson(new List<Bet> { new Bet { Id = 1, Amount = 10, Coefficient = 1.2, fk_robot_id = 1, fk_fight_id = 1, fk_user_id = 1 } });
            mock.When($"/api/Bets").RespondJson(bet);
            mock.When($"/api/Bets/balance").RespondJson(tempCurrent);


            var cut = RenderComponent<SingleBet>();
            cut.WaitForState(() => cut.FindAll("button").Count > 0, timeout: TimeSpan.FromSeconds(1)); 
            await cut.InvokeAsync(() => cut.Instance.bet = "10,20");
            await cut.InvokeAsync(() => cut.Instance.HandleBet());


            Assert.AreEqual("http://localhost/betting/bets", navMan.Uri);
        }
    }
}
