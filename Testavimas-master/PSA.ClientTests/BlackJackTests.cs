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
using BlackJack = PSA.Client.Pages.Gambling.BlackJack;
using Microsoft.JSInterop;

namespace PSA.ClientTests
{
    [TestClass()]
    public class BlackJackTests : BunitTestContext
    {
        public static Fixture _fixture = new();

        [TestMethod]
        public async Task PlayingTests_boolFalse()
        {
            // Arrange
            var mock = Services.AddMockHttpClient();
            var tempBool = false;
            var tempCurrent = _fixture.Build<CurrentUser>().With(x => x.Id, 1).Create();
            var tempBlack = _fixture.Build<Shared.BlackJack>().With(x => x.Id, 1).With(y => y.playerCards, new List<Shared.Card> { new Shared.Card("", "", 21) }).With(y => y.dealerCards, new List<Shared.Card> { new Shared.Card("", "", 21) }).Create();

            mock.When($"/api/currentuser").RespondJson(tempCurrent);
            mock.When($"/api/blackjack/gamestate").RespondJson(tempBool);
            mock.When($"/api/blackjack/game").RespondJson(tempBlack);
            mock.When($"/api/blackjack/gamestate/{tempBlack?.Id}").RespondJson(tempBlack);
            mock.When($"/api/blackjack/resetdeck/{tempBlack?.Id}").RespondJson(tempBlack);
            var cut = RenderComponent<BlackJack>();

            cut.WaitForState(() => cut.FindAll("div").Count > 0);
            var jsMock = new Mock<IJSRuntime>();
            JSInterop.Mode = JSRuntimeMode.Loose;
            JSInterop.Setup<string>("alert").SetResult("bUnit is awesome");

            // Set up an invocation without specifying the result
            var plannedInvocation = JSInterop.SetupVoid("alert");
            // Act - Simulate form submission
            var result = cut.Instance.Playing();
            Assert.IsNotNull(result, "error");
        }
        [TestMethod]
        public async Task PlayingTests_blackjackNull()
        {
            // Arrange
            var mock = Services.AddMockHttpClient();

            var tempBool = false;
            var tempCurrent = _fixture.Build<CurrentUser>().With(x => x.Id, 1).Create();
            Shared.BlackJack? nullis = null;
            mock.When($"/api/currentuser").RespondJson(tempCurrent);
            mock.When($"/api/blackjack/gamestate").RespondJson(tempBool);
            mock.When($"/api/blackjack/game").RespondJson(nullis);

            var cut = RenderComponent<BlackJack>();
            cut.WaitForState(() => cut.FindAll("div").Count > 0);
            var jsMock = new Mock<IJSRuntime>();
            JSInterop.Mode = JSRuntimeMode.Loose;
            JSInterop.Setup<string>("alert").SetResult("bUnit is awesome");

            // Set up an invocation without specifying the result
            var plannedInvocation = JSInterop.SetupVoid("alert");
            // Act - Simulate form submission
            var result = cut.Instance.Playing();

            Assert.IsNotNull(result, "error");
        }
        [TestMethod]
        public async Task PlayingTests_boolTrue()
        {
            // Arrange
            var mock = Services.AddMockHttpClient();
            var tempBool = true;
            var tempCurrent = _fixture.Build<CurrentUser>().With(x => x.Id, 1).Create();
            mock.When($"/api/currentuser").RespondJson(tempCurrent);
            mock.When($"/api/blackjack/gamestate").RespondJson(tempBool);
            var tempBlack = _fixture.Build<Shared.BlackJack>().With(x => x.Id, 1).Create();
            mock.When($"/api/blackjack/game").RespondJson(tempBlack);
            var cut = RenderComponent<BlackJack>();

            cut.WaitForState(() => cut.FindAll("div").Count > 0);
            var jsMock = new Mock<IJSRuntime>();
            JSInterop.Mode = JSRuntimeMode.Loose;
            JSInterop.Setup<string>("alert").SetResult("bUnit is awesome");

            // Set up an invocation without specifying the result
            var plannedInvocation = JSInterop.SetupVoid("alert");
            // Act - Simulate form submission
            var result = cut.Instance.Playing();
            Assert.IsNotNull(result, "error");
        }
    }
}
