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
using System.Collections.Generic;
using System.Threading.Tasks;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSA.Shared;
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

            await cut.InvokeAsync(() => cut.Instance.Playing());
            var jsMock = new Mock<IJSRuntime>();
            JSInterop.Mode = JSRuntimeMode.Loose;
            JSInterop.Setup<string>("alert").SetResult("bUnit is awesome");

            // Set up an invocation without specifying the result
            var plannedInvocation = JSInterop.SetupVoid("alert");
            // Act - Simulate form submission
            //await cut.Instance.Playing();
            // Assert.IsNotNull(result, "error");
        }
        [TestMethod]
        public async Task PlayingTests_blackjackNull()
        {
            // Arrange
            var mock = Services.AddMockHttpClient();
            var jsMock = new Mock<IJSRuntime>();
            JSInterop.Mode = JSRuntimeMode.Loose;
            JSInterop.Setup<string>("alert").SetResult("bUnit is awesome");
           
           
            var plannedInvocation = JSInterop.SetupVoid("alert");
            var tempBool = false;
            var tempCurrent = _fixture.Build<CurrentUser>().With(x => x.Id, 1).Create();
            Shared.BlackJack? nullis = null;
            mock.When($"/api/currentuser").RespondJson(tempCurrent);
            mock.When($"/api/blackjack/gamestate").RespondJson(tempBool);
            mock.When($"/api/blackjack/game").RespondJson(nullis);
            //mock.When($"/api/blackjack/gamestate/{nullis.Id}").RespondJson(nullis);
            //mock.When($"/api/blackjack/resetdeck/{nullis?.Id}").RespondJson(nullis);
            var cut = RenderComponent<BlackJack>();
            await cut.InvokeAsync(() => cut.Instance.Playing());

           
        
           
            
        }
        [TestMethod]
        public async Task PlayingTests_boolTrue()
        {
            // Arrange
            var mock = Services.AddMockHttpClient();
            var tempBool = true;

            var tempCurrent = _fixture.Build<CurrentUser>().With(x => x.Id, 1).Create();
            var tempBlack = _fixture.Build<Shared.BlackJack>().With(x => x.Id, 1).With(y => y.playerCards, new List<Shared.Card> { new Shared.Card("", "", 21) }).With(y => y.dealerCards, new List<Shared.Card> { new Shared.Card("", "", 21) }).Create();

            mock.When($"/api/currentuser").RespondJson(tempCurrent);
            mock.When($"/api/blackjack/gamestate").RespondJson(tempBool);
            mock.When($"/api/blackjack/game").RespondJson(tempBlack);
            mock.When($"/api/blackjack/gamestate/{tempBlack?.Id}").RespondJson(tempBlack);
            mock.When($"/api/blackjack/resetdeck/{tempBlack?.Id}").RespondJson(tempBlack);
            var cut = RenderComponent<BlackJack>();
            await cut.InvokeAsync(() => cut.Instance.Playing());

            var jsMock = new Mock<IJSRuntime>();
            JSInterop.Mode = JSRuntimeMode.Loose;
            JSInterop.Setup<string>("alert").SetResult("bUnit is awesome");

            // Set up an invocation without specifying the result
            var plannedInvocation = JSInterop.SetupVoid("alert");
            // Act - Simulate form submission
            //Assert.IsNotNull(result, "error");
        }

        [TestMethod]
        public async Task HandleHitTests()
        {
            // Arrange
            var mock = Services.AddMockHttpClient();

            var tempBool = true;
            var tempCurrent = _fixture.Build<CurrentUser>().With(x => x.Id, 1).Create();
            var tempBlack = _fixture.Build<Shared.BlackJack>().With(x => x.Id, 1).With(y => y.playerCards, new List<Shared.Card> { new Shared.Card("", "", 19) }).With(y => y.dealerCards, new List<Shared.Card> { new Shared.Card("", "", 19) }).Create();
            var tempCard = _fixture.Build<Shared.Card>().With(x => x.value, 21).CreateMany().ToList();
            mock.When($"/api/currentuser").RespondJson(tempCurrent);
            mock.When($"/api/blackjack/gamestate").RespondJson(tempBool);
            mock.When($"/api/blackjack/game").RespondJson(tempBlack);
            mock.When($"/api/blackjack/hit/{tempBlack?.Id}").RespondJson(tempBlack);
            //mock.When($"/api/blackjack/hit/").RespondJson(tempBlack);
            mock.When($"/api/blackjack/gamestate/{tempBlack?.Id}").RespondJson(tempBlack);
            mock.When($"/api/blackjack/resetdeck/{tempBlack?.Id}").RespondJson(tempBlack);

            var cut = RenderComponent<BlackJack>();

            //cut.WaitForState(() => cut.FindAll("div").Count > 0);

            await cut.InvokeAsync(() => cut.Instance.HandleHit());
            //cut.WaitForState(() => cut.FindAll("div").Count > 0);
            var jsMock = new Mock<IJSRuntime>();
            JSInterop.Mode = JSRuntimeMode.Loose;
            JSInterop.Setup<string>("alert").SetResult("bUnit is awesome");

            // Set up an invocation without specifying the result
            var plannedInvocation = JSInterop.SetupVoid("alert");
            // Act - Simulate form submission
            // Act - Simulate form submission

        }

        [TestMethod]
        public async Task HandleStandTests()
        {
            // Arrange
            var mock = Services.AddMockHttpClient();

            var tempBool = true;
            var tempCurrent = _fixture.Build<CurrentUser>().With(x => x.Id, 1).Create();
            var tempBlack = _fixture.Build<Shared.BlackJack>().With(x => x.Id, 1).With(y => y.playerCards, new List<Shared.Card> { new Shared.Card("", "", 19) }).With(y => y.dealerCards, new List<Shared.Card> { new Shared.Card("", "", 16) }).Create();
            var tempCard = _fixture.Build<Shared.Card>().With(x => x.value, 21).CreateMany().ToList();
            mock.When($"/api/currentuser").RespondJson(tempCurrent);
            mock.When($"/api/blackjack/gamestate").RespondJson(tempBool);
            mock.When($"/api/blackjack/game").RespondJson(tempBlack);
            mock.When($"/api/blackjack/hit/{tempBlack?.Id}").RespondJson(tempBlack);
            mock.When($"/api/blackjack/gamestate/{tempBlack.Id}").RespondJson(tempBlack);
            mock.When($"/api/blackjack/resetdeck/{tempBlack.Id}").RespondJson(tempBlack);
            mock.When($"/api/blackjack/hitdealer/{tempBlack.Id}").RespondJson(tempBlack);

            var cut = RenderComponent<BlackJack>();

            //cut.WaitForState(() => cut.FindAll("div").Count > 0);

            await cut.InvokeAsync(() => cut.Instance.HandleStand());
            //cut.WaitForState(() => cut.FindAll("div").Count > 0);
            var jsMock = new Mock<IJSRuntime>();
            JSInterop.Mode = JSRuntimeMode.Loose;
            JSInterop.Setup<string>("alert").SetResult("bUnit is awesome");

            // Set up an invocation without specifying the result
            var plannedInvocation = JSInterop.SetupVoid("alert");
            // Act - Simulate form submission
            // Act - Simulate form submission
          
        }
    }
}
